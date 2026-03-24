Imports System.Threading
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http
Imports System.Web.Script.Serialization

Module Module1
    Public objMutex As Mutex
    Dim strLogFileName As String = String.Empty
    Dim hcHelper As HttpClientHelper
    Dim res As HttpResponseMessage
    Const cstrJsonContentType = "application/json"

    Structure Download
        'The values must be in lower case
        Const Correction = "correction"
        Const Employee = "employee"
        Const Schedule = "schedule"
        Const TimeCardDetail = "timecarddetail"
        Const TimeCardTotal = "timecardtotal"
        Const Paycode = "paycode"
    End Structure

    Sub Main(ByVal CmdArgs() As String)

        Dim strInboundFolder As String = String.Empty
        Dim strResultContent As String = String.Empty
        Dim strAccessToken As String = String.Empty
        Dim gstrDownloadID As String = String.Empty
        Dim gstrFacility As String = String.Empty = "00"
        Dim gstrPeriod As String = String.Empty
        Dim gblnClrWrkData As Boolean = True
        Dim strJobParms As String = String.Empty
        Dim blnHasError As Boolean = False
        Dim strErrMsg As String = String.Empty

        Try

            strLogFileName = String.Format(My.Settings.gstrLogFilePath, Now.Date.ToString("yyyyMMdd"))
            'Prevent user from starting multiple copies.
            If My.Application.CommandLineArgs.Count > 0 Then
                'Parameter 1: Download Id
                gstrDownloadID = LCase(CmdArgs(0)).Replace(vbCrLf, "")
                'Parameter 2: facility - 2 digits numeric
                If CmdArgs.Length > 1 Then
                    gstrFacility = CmdArgs(1).Replace(vbCrLf, "")
                Else
                    gstrFacility = "00"
                End If
                'Parameter 3: period - numeric
                If CmdArgs.Length > 2 Then
                    gstrPeriod = CmdArgs(2).Replace(vbCrLf, "")
                End If
                'Parameter 4: Clear data from work tables - 0 = not clear or 1 = clear.
                If CmdArgs.Length > 3 Then
                    gblnClrWrkData = CmdArgs(3).Replace(vbCrLf, "")
                End If

                strJobParms = String.Format("Download ID: {0}, Facility: {1}, Period: {2}, Clear table: {3} ", gstrDownloadID, gstrFacility, gstrPeriod, gblnClrWrkData)

                If gstrDownloadID <> Download.Correction And
                    gstrDownloadID <> Download.Employee And
                    gstrDownloadID <> Download.Paycode And
                    gstrDownloadID <> Download.Schedule And
                    gstrDownloadID <> Download.TimeCardDetail And
                    gstrDownloadID <> Download.TimeCardTotal Then
                    strErrMsg = "Invalid download id."
                    blnHasError = True
                Else
                    If gstrFacility = String.Empty Or CmdArgs.Length < 2 Then
                        strErrMsg = "Missing parameter for Facility."
                        blnHasError = True
                    End If
                End If
            Else
                strErrMsg = "First 2 input parameters are required. 1(R) - Download ID, 2(R) - Facility, 3(O) - Period, 4(O) - Clear table flag"
                blnHasError = True
            End If

            objMutex = New Mutex(False, gstrDownloadID)
            If objMutex.WaitOne(0, False) = False Then
                objMutex.Close()
                objMutex = Nothing
                'WriteLog(strLogFileName, "Kronos ETL is already running. Download ID: " & gstrDownloadID & ". Facility: " & gstrFacility)
                WriteLog(strLogFileName, "Kronos ETL is already running. " & strJobParms)
            ElseIf blnHasError = True Then
                Throw New Exception(strErrMsg & " " & strJobParms)
            Else
                'WriteLog(strLogFileName, "Kronos ETL starts running. Download ID: " & gstrDownloadID & ". Facility: " & gstrFacility)
                WriteLog(strLogFileName, "Kronos ETL starts running. " & strJobParms)

                hcHelper = New HttpClientHelper
                PrepareAPIHeaderForAccessToken()

                strInboundFolder = CreateInboundFolder(String.Empty)

                'Post a request to Kronos cloud for the temporary access token.
                res = hcHelper.PostAsyncForAccessToken.Result
                If res.IsSuccessStatusCode = True Then
                    If res.StatusCode = HttpStatusCode.OK Then
                        strResultContent = res.Content.ReadAsStringAsync().Result
                        'Parse the data and extra the access token from it 
                        strAccessToken = GetKronosAccessToken(strResultContent)
                        If strAccessToken <> String.Empty Then
                            'Send the access token, API name and script or parameters to down load the desired data
                            hcHelper.APIAccessToken = strAccessToken
                            strResultContent = DownloadData(strAccessToken, gstrDownloadID, gstrFacility, gstrPeriod)
                            WriteLog(strLogFileName, "The size of the download data in character is " & strResultContent.Length.ToString)
                            'Parse the API result and write the desired data to the SQL tables
                            ParseAPIResultToTable(strResultContent, gstrDownloadID, gstrFacility, gblnClrWrkData)
                        End If
                    End If
                Else
                    Dim strMessage As String
                    strMessage = "No content was return when requested the access token."
                    WriteLog(strLogFileName, strMessage)
                    sendEmail(strMessage, My.Settings.gstrPPServerCnnStr, "KronosETL failed. " & strJobParms)
                End If
                objMutex.ReleaseMutex()
                WriteLog(strLogFileName, "Kronos ETL finished running. Download ID: " & strJobParms)
            End If
        Catch ex As Exception
            Try
                WriteLog(strLogFileName, "Error in Main: " & ex.Message & vbCrLf & ex.StackTrace)
            Catch
            End Try
            Try
                sendEmail(ex.Message & vbCrLf & ex.StackTrace, My.Settings.gstrPPServerCnnStr, "KronosETL failed. " & strJobParms)
            Catch
            End Try
        End Try
    End Sub

    Sub PrepareAPIHeaderForAccessToken()
        Dim drCtl As dsControlTable.KRsp_Control_SelRow
        Try
            'Get the Http client header parameter values
            With hcHelper
                Using taCtl As New dsControlTableTableAdapters.KRsp_Control_SelTableAdapter
                    Using dtCtl As New dsControlTable.KRsp_Control_SelDataTable
                        taCtl.Fill(dtCtl, Nothing, "KronosAPI", Nothing, "BySubKey")
                        For Each drCtl In dtCtl
                            Select Case drCtl.Key
                                Case "KronosHdrParam1"
                                    .ClientId = drCtl.Value1
                                    .ClientSecret = drCtl.Value2
                                Case "KronosHdrParam2"
                                    .AppKey = drCtl.Value1
                                    .ContentType = drCtl.Value2
                                Case "KronosHdrParam3"
                                    .BaseUrl = drCtl.Value1
                                    .GrantType = drCtl.Value2
                                Case "KronosHdrParam4"
                                    .AuthenticationAPI = drCtl.Value1
                                    .CommonRecDtaAPIName = drCtl.Value2
                            End Select
                        Next
                    End Using
                End Using
                .UserName = My.Settings.gstrUID
                .Password = My.Settings.gstrPID
                .ContentType = cstrJsonContentType
            End With
        Catch ex As Exception
            Throw New Exception("Error in PrepareAPIHeaderForAccessToken - " & ex.Message)
        End Try

    End Sub

    Private Function CreateInboundFolder(ByVal strFolderName As String) As String
        Dim strInboudtPath As String = String.Empty
        Try
            strInboudtPath = My.Settings.gstrInboundPath & "\" & strFolderName
            If (Not System.IO.Directory.Exists(strInboudtPath)) Then
                System.IO.Directory.CreateDirectory(strInboudtPath)
            End If
            Return strInboudtPath
        Catch ex As Exception
            Throw New Exception("Error in CreateInboundFolder - " & ex.Message)
        End Try
    End Function

    Private Function ConstructName(ByVal strName As String, ByVal strFacility As String, ByVal strExt As String) As String
        Dim strFileName As String = String.Empty
        Dim oDate As DateTime = Convert.ToDateTime(Now.ToString)
        Try
            Select Case strExt
                Case "folder"
                    'strFileName = strName & "_" & oDate.Year & oDate.Month & oDate.Day
                    strFileName = String.Format("{0}_{1:yyyyMMdd}", strName, oDate)
                Case "txt", "html"
                    'strFileName = strName & "_" & oDate.Year & oDate.Month & oDate.Day & "." & strExt
                    strFileName = String.Format("{0}_{1:yyyyMMdd}.{2}", strName, oDate, strExt)
                Case Else
                    'strFileName = strName & "_" & strFacility & "_" & oDate.Year & oDate.Month & oDate.Day & oDate.Hour & oDate.Minute & oDate.Second & "." & strExt
                    strFileName = String.Format("{0}_{1}_{2:yyyyMMddHHmmss}.{3}", strName, strFacility, oDate, strExt)
            End Select
            Return strFileName
        Catch ex As Exception
            Throw New Exception("Error in ConstructName - " & ex.Message)
        End Try
    End Function

    Private Function GetKronosAccessToken(ByVal APIRawResult As String) As String
        Dim strAccessToken As String = String.Empty
        Try
            Dim data As dsAccessTokenResult() = (New JavaScriptSerializer()).Deserialize(Of dsAccessTokenResult())("[" & APIRawResult & "]")
            If data.Count > 0 Then
                For Each content As dsAccessTokenResult In data
                    strAccessToken = content.access_token.ToString
                Next
            Else
                WriteLog(strLogFileName, "Post Header - Status: Rejected.")
            End If
            Return strAccessToken
        Catch ex As Exception
            Throw New Exception("Error in GetKronosAccessToken - " & ex.Message)
        End Try
    End Function

    Function DownloadData(strAccessToken As String, strDownloadID As String, strFacility As String, strPeriod As String) As String
        Dim hrmResult As HttpResponseMessage
        Dim strResultContent As String = String.Empty
        Dim strCurrentFileName As String
        Dim strScriptContent As Http.StringContent = Nothing
        Dim strScript As String = String.Empty
        Dim strAPIName As String = String.Empty
        Dim strAPIScriptParms As String = String.Empty
        Dim drCtl As dsControlTable.KRsp_Control_SelRow
        Dim blnIsPostAPI As Boolean
        Dim blnHasScript As Boolean
        Dim strAPIParms As String = String.Empty

        Try

            'create a temporary file with hour and minute in the name to hold the return data
            strCurrentFileName = ConstructName(strDownloadID, strFacility, "json")

            Select Case strDownloadID

                Case Download.Employee
                    strAPIName = "EmployeeAPIName"
                    'strAPIScriptParms = "EmployeeScriptParms"
                    blnIsPostAPI = True
                    blnHasScript = True
                Case Download.TimeCardTotal
                    strAPIName = "TimeCardTotalAPIName"
                    'strAPIScriptParms = "TimeCardTotalScriptParms"
                    blnIsPostAPI = True
                    blnHasScript = True
                Case Download.TimeCardDetail
                    strAPIName = "TimeCardDetailAPIName"
                    'strAPIScriptParms = "TimeCardTotalScriptParms"
                    blnIsPostAPI = True
                    blnHasScript = True
                Case Download.Schedule
                    strAPIName = "ScheduleAPIName"
                    'strAPIScriptParms = "ScheduleScriptParms"
                    blnIsPostAPI = True
                    blnHasScript = True
                Case Download.Correction
                    strAPIName = "CorrectionAPIName"
                    ' strAPIScriptParms = "CorrectionScriptParms"
                    blnIsPostAPI = True
                    blnHasScript = True
                Case Download.Paycode
                    strAPIName = "PayCodeAPIName"
                    'Pay code does not need script file but a parameter
                    'strAPIScriptParms = "PayCodeScriptParms"
                    blnIsPostAPI = False
                    blnHasScript = False
            End Select

            If blnHasScript = True Then
                strScript = GetAPIScriptBody(strDownloadID)
            End If

            strAPIScriptParms = "EmpHyperfind"

            drCtl = SharedFunctions.GetAppControlData(strAPIName, Nothing, Nothing, "ByKey")
            If IsNothing(drCtl) Then
                hcHelper.APIName = hcHelper.CommonRecDtaAPIName
            Else
                hcHelper.APIName = drCtl.Item("Value1")
                hcHelper.APIParameter = drCtl.Item("Value2")
            End If

            If blnHasScript = True Then
                'Get the Kronos pre-defined hyperfind id for site and time frame period id from the pre-saved on-premise control table in SQL
                drCtl = SharedFunctions.GetAppControlData(strAPIScriptParms, Nothing, strFacility, "ByKey")
                If drCtl Is Nothing Then
                    Throw New Exception
                Else
                    If strPeriod = String.Empty Then
                        strScript = strScript.Replace("@hyperfindId", drCtl.Item("Value1")).Replace("@periodId", drCtl.Item("Value2"))
                    Else
                        strScript = strScript.Replace("@hyperfindId", drCtl.Item("Value1")).Replace("@periodId", strPeriod)
                    End If
                End If
                'Description            Hyperfind       count
                'All FW Employees       157             158
                'All CA Employees       3               343
                'All Main Plant         155             123
                'All Real Cup Unit A    156             116
                'All Ajax               53              74
                'All Head Office DC     154             30

                strScriptContent = New Http.StringContent(strScript, Text.UnicodeEncoding.UTF8, cstrJsonContentType)
            End If

            'Run Post API
            If blnIsPostAPI Then
                If blnHasScript Then
                    hrmResult = hcHelper.PostAsync(strScriptContent, strAccessToken).Result
                Else
                    hrmResult = hcHelper.PostAsync(strAPIParms).Result
                End If
            Else        'Run Get API
                If blnHasScript Then
                    hrmResult = hcHelper.GetAsync(strScriptContent).Result
                Else
                    hrmResult = hcHelper.GetAsync().Result
                End If

            End If

            If hrmResult.IsSuccessStatusCode = True Then
                If hrmResult.StatusCode = HttpStatusCode.OK Then
                    strResultContent = hrmResult.Content.ReadAsStringAsync().Result

                    'Write the API response to a text file
                    My.Computer.FileSystem.WriteAllText(My.Settings.gstrInboundPath & "\" & strCurrentFileName, strResultContent, True)

                End If
            Else
                strResultContent = hrmResult.Content.ReadAsStringAsync().Result
                WriteLog(strLogFileName, strResultContent)
                strResultContent = String.Empty
            End If
            Return strResultContent
        Catch ex As Exception
            Throw New Exception("Error in DownloadData - " & ex.Message)
        End Try
    End Function

    Private Function GetAPIScriptBody(strDownLoadID As String) As String
        Dim strFileName As String = String.Empty
        Dim sb As New System.Text.StringBuilder

        Try
            strFileName = My.Settings.gstAPIScriptFilePath & strDownLoadID & ".txt"
            If System.IO.File.Exists(strFileName) = True Then
                Using objreader As New System.IO.StreamReader(strFileName)
                    Do While objreader.Peek() <> -1
                        sb.AppendFormat("{0}", objreader.ReadLine().Trim)
                    Loop
                End Using
            Else
                WriteLog(strLogFileName, strFileName & ", API Script File Does Not Exist")
            End If
            Return sb.ToString
        Catch ex As Exception
            Throw New Exception("Error in GetAPIScriptBody - " & ex.Message)
        End Try
    End Function

    Private Sub ParseAPIResultToTable(strResultContent As String, strDownloadID As String, strFacility As String, blnClrWrkTbl As Boolean)
        Dim strSPName As String = String.Empty
        Dim arParms() As SqlParameter

        Try
            ReDim arParms(2)
            arParms = New SqlParameter(UBound(arParms)) {}
            arParms(0) = New SqlParameter("@vchJson", SqlDbType.VarChar)
            arParms(0).Value = strResultContent

            arParms(1) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(1).Value = strFacility

            arParms(2) = New SqlParameter("@bitClrWrkTbl", SqlDbType.VarChar)
            arParms(2).Value = blnClrWrkTbl

            Select Case strDownloadID
                Case Download.Correction
                    strSPName = "KRsp_CvtJsonToTable_Correction"
                Case Download.Employee
                    strSPName = "KRsp_CvtJsonToTable_Employee"
                Case Download.Paycode
                    strSPName = "KRsp_CvtJsonToTable_Paycode"
                Case Download.TimeCardDetail
                    strSPName = "KRsp_CvtJsonToTable_TimeCardDetail"
                Case Download.TimeCardTotal
                    strSPName = "KRsp_CvtJsonToTable_TimeCardTotal"
                Case Download.Schedule
                    strSPName = "KRsp_CvtJsonToTable_Schedule"
            End Select
            SqlHelper.ExecuteNonQuery(My.Settings.gstrPPServerCnnStr, strSPName, arParms)
        Catch ex As Exception
            Throw New Exception("Error in ParseAPIResultToTable - " & ex.Message)
        End Try
    End Sub

    Sub WriteLog(ByVal strFileName As String, ByVal strMsg As String)
        Try
            If My.Computer.FileSystem.FileExists(strFileName) = False Then
                My.Computer.FileSystem.WriteAllText(strFileName, String.Empty, False)
            End If
            My.Computer.FileSystem.WriteAllText(strFileName, DateTime.Now & " - " & strMsg & vbCrLf, True)
        Catch ex As Exception
            Throw New Exception("Error in WriteLog " & ex.Message)
        End Try
    End Sub

    Sub sendEmail(ByVal strMsgBody As String, ByVal cnnServerDB As String, Optional ByVal strSubject As String = Nothing)
        Dim arParms() As SqlParameter
        Dim strSQL As String

        Try
            strSQL = "KRsp_SndMsgToOperator"
            ReDim arParms(1)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Message Body Input Parameter
            arParms(0) = New SqlParameter("@vchMsgBody", SqlDbType.VarChar, Integer.MaxValue)
            arParms(0).Value = strMsgBody

            ' Message Subject Input Parameter                                       
            arParms(1) = New SqlParameter("@vchSubject", SqlDbType.VarChar, 512)
            arParms(1).Value = strSubject

            Using sqlConn As New SqlConnection()
                sqlConn.ConnectionString = cnnServerDB
                sqlConn.Open()

                Using cmd As New SqlCommand(strSQL, sqlConn)
                    cmd.Parameters.Add(arParms(0))
                    cmd.Parameters.Add(arParms(1))
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in sendEmail - " & ex.Message)
        End Try
    End Sub
End Module
