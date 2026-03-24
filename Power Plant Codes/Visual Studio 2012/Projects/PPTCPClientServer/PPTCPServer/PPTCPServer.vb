Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Diagnostics
Imports System.ServiceProcess
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Threading

Module PPTCPServer
    Dim tcpListener As TcpListener
    Dim tcpClients As New List(Of TcpClient)
    Dim strEvent As String
    Dim gstrServerConnectionString As String
    Dim gstrLocalDBConnectionString As String
    Dim gblnSvrConnIsUp As Boolean = True
    Dim objCheckWeigher As clsCheckWeigher
    Dim gthdCheckWeigher As System.Threading.Thread
    Dim gdrSessCtl As dsSessionControl.CPPsp_SessionControlIORow
    Dim gintTCPConnID As Integer
    Dim gblnLoadCheckWeigherStarted As Boolean = False
    Dim gintRequiredAction As Int16               '0 = Start; 1 = Stop
    Dim queReceivedData As Queue
    Dim gstrModel As String
    Dim gintShopOrderBackup As Integer
    Dim mre As New ManualResetEvent(False)
    Const cstrSource As String = "PPTCPServer"
    Const cstrLog As String = "Application"

    Public Enum Action
        Start
        Abort
    End Enum

    Sub Main()

        ' Must listen on correct port- must be same as port client wants to connect on.

        Dim strComputerName As String = String.Empty
        Const portNumber As Integer = 8000
        Dim ip As IPAddress = IPAddress.Parse("127.0.0.1")
        Dim P() As Process
        Try
            P = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
            If P.Length > 1 Then
                WriteToEventLog(cstrLog, cstrSource, "Another PPTCPServer has already running. Current instance will be closed.", EventLogEntryType.Information)
            Else
                strComputerName = My.Computer.Name
                gstrServerConnectionString = My.Settings.ServerPowerPlantCnnStr
                gstrLocalDBConnectionString = My.Settings.LocalPowerPlantCnnStr

                WriteToEventLog(cstrLog, cstrSource, "PPTCPServer is started", EventLogEntryType.Information)

                If isServiceRunning("MSSQL$SQLEXPRESS") Then
                    Using taCC As New dsComputerConfigTableAdapters.CPPsp_ComputerConfigIOTableAdapter
                        Using dtCC As New dsComputerConfig.CPPsp_ComputerConfigIODataTable
                            Try
                                taCC.Fill(dtCC, "SelectAllFields", strComputerName, Nothing)

                                If dtCC.Count > 0 Then
                                    If dtCC(0).QATWorkFlowInitiation <> 1 Then  'NOT using external system for QAT work flow
                                        WriteToEventLog(cstrLog, cstrSource, "NOT using external system for QAT work flow, do not start listener.", EventLogEntryType.Information)
                                        Exit Sub
                                    End If
                                Else
                                    WriteToEventLog(cstrLog, cstrSource, "Computer name is not found or inactive in the PP configuration table, do not start listener.", EventLogEntryType.Information)
                                    Exit Sub
                                End If
                            Catch ex As SqlClient.SqlException
                                If ex.Message.Contains("The server was not found or was not accessible.") Then
                                    Threading.Thread.Sleep(1000)
                                End If
                            End Try

                        End Using
                    End Using
                    tcpListener = New TcpListener(ip, portNumber)

                    tcpListener.Start()
                    WriteToEventLog(cstrLog, cstrSource, "listener started, Waiting for connections from clients.", EventLogEntryType.Information)

                    NewClients()
                Else
                    WriteToEventLog(cstrLog, cstrSource, "Service MSSQL$SQLEXPRESS has not started.", EventLogEntryType.Error)
                End If

                WriteToEventLog(cstrLog, cstrSource, "PPTCPServer is stopped", EventLogEntryType.Information)
            End If
        Catch ex As Exception
            Try
                WriteToEventLog(cstrLog, cstrSource, "Unexpected error. " & ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error)
            Catch ex1 As Exception
            End Try
        End Try
    End Sub

    Private Sub NewClients()
        Dim tcpClient As New TcpClient
        Dim strClientData As String = String.Empty
        Dim strClientResponse As String = String.Empty
        Dim sendBytes As [Byte]()
        Dim networkStream As NetworkStream
        Dim objProcessStartInfo As ProcessStartInfo
        Dim bytes() As Byte

        Try
            While True
                'Accept the pending client connection and return a TcpClient initialized for communication. 
                tcpClient = tcpListener.AcceptTcpClient()
                ' Console.WriteLine("Connection accepted.")
                tcpClients.Add(tcpClient)
                Threading.ThreadPool.QueueUserWorkItem(AddressOf NewClients)

                ' Get the stream
                networkStream = tcpClient.GetStream()

                ' Read the stream into a byte array
                ReDim bytes(tcpClient.ReceiveBufferSize)
                networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))

                ' Return the data received from the client to the console.
                strClientData = Encoding.ASCII.GetString(bytes)
                If strClientData.Length > 10000 Then
                    strClientData = strClientData.Substring(0, 10000)
                End If
                WriteToEventLog(cstrLog, cstrSource, "Client sent: " & strClientData.Trim, EventLogEntryType.Information)

                strClientResponse = "Connected to server."
                sendBytes = Encoding.ASCII.GetBytes(strClientResponse)
                networkStream.Write(sendBytes, 0, sendBytes.Length)

                If strClientData.StartsWith("@") Then
                    CollectCheckweigherData(strClientData)
                Else
                    'Call power plant .net program
                    objProcessStartInfo = New ProcessStartInfo
                    With objProcessStartInfo
                        .FileName = My.Settings.strPPPgmPath
                        .Arguments = strClientData
                        .WindowStyle = ProcessWindowStyle.Hidden
                    End With
                    Process.Start(objProcessStartInfo)
                End If

            End While
        Catch ex As Exception
            WriteToEventLog(cstrLog, cstrSource, "Error in NewClients - " & ex.Message, EventLogEntryType.Error)
            If tcpClients.Contains(tcpClient) Then
                tcpClients.Remove(tcpClient)
            End If
        Finally
            'Close TcpListener and TcpClient.
            tcpClient.Close()
            tcpListener.Stop()
        End Try
    End Sub

    Public Sub WriteToEventLog(ByVal strLogName As String, ByVal strSource As String, ByVal strErrDetail As String, ByVal elet As EventLogEntryType)
        Dim SDEventLog As System.Diagnostics.EventLog = New System.Diagnostics.EventLog()

        Try
            If My.Settings.blnLogEvent = True Or elet = EventLogEntryType.Error Then
                If Not System.Diagnostics.EventLog.SourceExists(strLogName) Then
                    System.Diagnostics.EventLog.CreateEventSource(strSource, cstrLog)
                End If

                SDEventLog.Source = strLogName
                If strErrDetail.Length > 30000 Then
                    strErrDetail = strErrDetail.Substring(0, 30000).Trim
                End If
                SDEventLog.WriteEntry(strSource & " - " & strErrDetail, elet)
            End If

        Catch ex As Exception
            SDEventLog.Source = strLogName
            SDEventLog.WriteEntry("INFORMATION: " & ex.Message, EventLogEntryType.Information)
        Finally
            SDEventLog.Dispose()
            SDEventLog = Nothing
        End Try
    End Sub

    Private Function isServiceRunning(strServiceName) As Boolean
        isServiceRunning = False
        Try

            Using sc As New ServiceController(strServiceName)
                For intCount As Int16 = 1 To 15
                    If sc.Status <> ServiceControllerStatus.Running Then
                        Threading.Thread.Sleep(1000)
                        sc.Refresh()
                    Else
                        isServiceRunning = True
                        Exit Function
                    End If
                Next
            End Using

        Catch ex As Exception
            Try
                WriteToEventLog(cstrLog, cstrSource, "Unexpected error in IsServiceRunning. " & ex.Message, EventLogEntryType.Error)
            Catch ex1 As Exception
            End Try
        End Try
    End Function

    Private Sub CollectCheckweigherData(strClientData As String)
        'Determine the executing action to the checkweigher log. 
        'If start/restart, create a new thread to run checkweigher log

        Dim strArgs() As String
        Dim cstrCurrQATEntryPoint As String = "S"
        Dim strAction As String

        Try
            'Parameter 0 = First character is "@"
            'Parameter 1 = Checkweigher TCP Connection ID
            'Parameter 2 = 0 or 1 (start or abort collecting data)

            strArgs = strClientData.Split(" ")
            gintTCPConnID = Integer.Parse(strArgs(1))
            strAction = strArgs(2)

            gintRequiredAction = Int16.Parse(strAction)

            If gintRequiredAction = Action.Start Then
                RefreshSessionControlTable()
                If gblnLoadCheckWeigherStarted = False Then
                    gblnLoadCheckWeigherStarted = True
                    gthdCheckWeigher = New Threading.Thread(AddressOf CheckWeigherManager)
                    gthdCheckWeigher.Name = "Load Checkweigher"
                    gthdCheckWeigher.IsBackground = True
                    gthdCheckWeigher.Start()
                    WriteToEventLog(cstrLog, cstrSource, "Start checkweigher logging.", EventLogEntryType.Information)
                Else
                    If Not gthdCheckWeigher.IsAlive Then
                        gthdCheckWeigher = New Threading.Thread(AddressOf CheckWeigherManager)
                        gthdCheckWeigher.Name = "Load Checkweigher"
                        gthdCheckWeigher.IsBackground = True
                        gthdCheckWeigher.Start()
                        WriteToEventLog(cstrLog, cstrSource, "Restart checkweigher logging.", EventLogEntryType.Information)
                    End If
                End If
            Else
                If gblnLoadCheckWeigherStarted = True Then
                    'Stop loading checkweigher data
                    gblnLoadCheckWeigherStarted = False
                    WriteToEventLog(cstrLog, cstrSource, "Stop checkweigher logging.", EventLogEntryType.Information)
                    'If gthdCheckWeigher.IsAlive Then
                    '    Threading.Thread.Sleep(1000)
                    '    gthdCheckWeigher.Abort()
                    'End If
                End If
            End If
        Catch ex As Exception
            Try
                WriteToEventLog(cstrLog, cstrSource, "Unexpected error in CollectCheckweigherData. " & ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error)
            Catch ex1 As Exception
            End Try
        End Try
    End Sub

    Private Sub CheckWeigherManager()
        Dim drTCPConn As dsQATTCPConn.CPPsp_QATTCPConn_SelRow = Nothing
        Try
            RefreshSessionControlTable()
            gintShopOrderBackup = gdrSessCtl.ShopOrder
            Using taTCPConn As New dsQATTCPConnTableAdapters.CPPsp_QATTCPConn_SelTableAdapter
                Using dtTCPConn As New dsQATTCPConn.CPPsp_QATTCPConn_SelDataTable
                    taTCPConn.Fill(dtTCPConn, gdrSessCtl.Facility, gintTCPConnID, True)
                    If dtTCPConn.Count > 0 Then
                        drTCPConn = dtTCPConn(0)
                    End If
                End Using
            End Using
            If Not IsNothing(drTCPConn) Then
                gstrModel = drTCPConn.Model
                queReceivedData = New Queue
                Do While gblnLoadCheckWeigherStarted = True
                    LoadDataFromCheckWeigher(drTCPConn)
                    Threading.Thread.Sleep(2000)
                Loop
            Else
                WriteToEventLog(cstrLog, cstrSource, "Can not find check weigher connection information for ID " & gintTCPConnID.ToString, EventLogEntryType.Error)
            End If

        Catch ex As Exception
            Try
                WriteToEventLog(cstrLog, cstrSource, "Unexpected error in CheckWeigherManager. " & ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error)
            Catch ex1 As Exception
            End Try
        Finally
            Thread.CurrentThread.Abort()
        End Try
    End Sub

    Private Sub LoadDataFromCheckWeigher(drTCPConn As dsQATTCPConn.CPPsp_QATTCPConn_SelRow)
        Dim strIPAddress As String = String.Empty
        Dim strServerResponse As String = Nothing
        Dim decActualWeight As Decimal
        Dim strRecievedData As String = String.Empty
        Dim QueData As New Queue
        Dim thdSaveData As System.Threading.Thread = Nothing
        Dim swForNoData As New Stopwatch
        Dim intNoOfIntervals As Integer = 0
        Static intRetryCount As Integer
        Static strLastErrMsg As String

        Try
            decActualWeight = 0

            strIPAddress = drTCPConn.IPAddress
            'Try
            objCheckWeigher = New clsCheckWeigher(strIPAddress.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", ""), drTCPConn.Port)
            'Catch ex As Exception
            '    intRetryCount = intRetryCount + 1
            '    If intRetryCount = 1 Then
            '        WriteToEventLog(cstrLog, cstrSource, "Error in LoadDataFromCheckweigher. " & ex.Message, EventLogEntryType.Error)
            '    Else
            '        'Total sleep time is 8 + 2 seconds before try to connect again , that is why it is divided by 10 seconds to calculate the count.
            '        If intRetryCount > (My.Settings.intEveryXSecWtrErrMsgToEvtViewer / 10) Then
            '            intRetryCount = 0
            '        End If
            '    End If
            '    System.Threading.Thread.Sleep(8000)
            '    Exit Sub
            'End Try

            intRetryCount = 0
            With objCheckWeigher
                strServerResponse = .TCPConnect()
                If IsNothing(strServerResponse) Then
                    If gstrModel = "XC" Or gstrModel = "XE" Then
                        If .IsHandShakeOK() Then
                            'send command e.g. "WD_SET_FORMAT 4" to change data receiving format 4 for actual weight data only.
                            .TCPSendCommand(drTCPConn.Command3)
                            System.Threading.Thread.Sleep(3000)

                            'send command "WD_START to start receiving data from checkweigher
                            .TCPSendCommand("WD_START")
                            System.Threading.Thread.Sleep(2000)

                            'Start up another thread to read the data from the queue to insert them to database
                            If IsNothing(thdSaveData) Then
                                thdSaveData = New System.Threading.Thread(AddressOf SaveData)
                                thdSaveData.Name = "SaveData"
                                thdSaveData.Start()
                            End If
                            swForNoData.Start()
                            Do While gblnLoadCheckWeigherStarted = True
                                Try
                                    System.Threading.Thread.Sleep(2000)
                                    strRecievedData = .ReceiveData()
                                Catch ex As Exception
                                    If ex.Message.Contains("was forcibly closed by the remote host") Then
                                        Exit Do
                                    End If
                                End Try
                                If strRecievedData.Length > 0 Then
                                    Debug.Print("RecData: " & strRecievedData)
                                    'put received data to the queue
                                    queReceivedData.Enqueue(strRecievedData)
                                    mre.Set()               'tell the thread queue, new data is added to the queue
                                    swForNoData.Reset()     'Reset the stop watch
                                    intNoOfIntervals = 0
                                Else
                                    If swForNoData.ElapsedMilliseconds >= 900000 Then       '900000.0ms = 1000.0 * 60 * 15 Milliseconds = 15 minutes
                                        swForNoData.Reset()                                 'Restart the stop watch.
                                        RefreshSessionControlTable()
                                        'if the line is not down, accumulate this interval, else reset the no. of intervals to 0 and restart the elapsed time
                                        If gdrSessCtl.IsStartDownTimeNull Then
                                            intNoOfIntervals = intNoOfIntervals + 1
                                            'if receive no data more than 3 hours (i.e. (15 minutes per intervals * 4 = an hour) * max. of hours.)
                                            If intNoOfIntervals >= 4 * My.Settings.intMaxIdleHours Then
                                                'stop receiving data from checkweigher
                                                gblnLoadCheckWeigherStarted = False
                                                WriteToEventLog(cstrLog, cstrSource, String.Format("Received no data from checkweigher for more than {0} hours, stop checkweigher logging.", My.Settings.intMaxIdleHours), EventLogEntryType.Information)
                                            Else
                                                WriteToEventLog(cstrLog, cstrSource, String.Format("Received no data from checkweigher for {0} minutes.", intNoOfIntervals * 15), EventLogEntryType.Information)
                                            End If
                                        Else
                                            intNoOfIntervals = 0
                                        End If
                                    End If
                                End If
                            Loop

                        Else
                            TestFailed("Handshake is not responding")
                        End If

                    End If
                Else
                    TestFailed("IP Address not responding - " & drTCPConn.IPAddress)
                End If

                .TCPDisconnect()
                'gthdCheckWeigher.Abort()
            End With
        Catch ex As ThreadAbortException
            'This is not an error for this.
        Catch ex As Exception
            intRetryCount = intRetryCount + 1
            If intRetryCount = 1 Or strLastErrMsg <> ex.Message Then
                Try
                    WriteToEventLog(cstrLog, cstrSource, "Unexpected error in LoadDataFromCheckweigher. " & ex.Message, EventLogEntryType.Error)
                    strLastErrMsg = ex.Message
                    intRetryCount = 1
                Catch ex1 As Exception
                End Try
            Else
                'Total sleep time is 8 + 2 seconds before try to connect again , that is why it is divided by 10 seconds to calculate the count.
                If intRetryCount > (My.Settings.intEveryXSecWtrErrMsgToEvtViewer / 10) Then
                    intRetryCount = 0
                End If
            End If
            System.Threading.Thread.Sleep(8000)
        Finally
            If Not IsNothing(thdSaveData) Then
                For i = 1 To 3
                    Thread.Sleep(3000)
                    If queReceivedData.Count = 0 Then
                        Exit For
                    End If
                Next
                thdSaveData.Abort()
                swForNoData.Stop()
            End If
        End Try
    End Sub

    Private Sub TestFailed(ByVal strMsg As String)
        Try
            If strMsg.Contains("IP Address not responding") Then
                WriteToEventLog(cstrLog, cstrSource, "Check Weigher TCP server is not responding.", EventLogEntryType.Error)
            End If
        Catch ex As Exception
            Try
                WriteToEventLog(cstrLog, cstrSource, "Unexpected error in TestFailed. " & ex.Message, EventLogEntryType.Error)
            Catch ex1 As Exception
            End Try
        End Try
    End Sub

    Private Sub SaveData()
        Dim strReceivedData As String = String.Empty
        Try

            While gblnLoadCheckWeigherStarted = True
                mre.WaitOne()   'Blocks the current thread until the current WaitHandle receives a signal.
                mre.Reset()     'Sets the state of the event to nonsignaled, causing threads to block.
                While queReceivedData.Count > 0
                    strReceivedData = queReceivedData.Dequeue()
                    If Not IsNothing(strReceivedData) Then
                        Debug.Print("SaveData: " & strReceivedData)
                        FormatCheckWeigherData(strReceivedData)
                    End If
                End While

            End While

        Catch ex As Exception
            WriteToEventLog(cstrLog, cstrSource, "Error in SaveData. " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub RefreshSessionControlTable()
        Try
            Using taSessCtl As New dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
                Using dtSessCtl As New dsSessionControl.CPPsp_SessionControlIODataTable
                    taSessCtl.Fill(dtSessCtl, "SelectAllFields")
                    If Not IsNothing(dtSessCtl) And dtSessCtl.Rows.Count > 0 Then
                        gdrSessCtl = dtSessCtl.Rows(0)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Try
                WriteToEventLog(cstrLog, cstrSource, "Unexpected error in RefreshSessoinControlTable. " & ex.Message, EventLogEntryType.Error)
            Catch ex1 As Exception
            End Try
        End Try
    End Sub

    Private Sub SetServerCnnStatusInSessCtl(ByVal blnStatus As Boolean)
        Dim arParms() As SqlParameter = New SqlParameter(0) {}
        Dim strsqlstmt As String

        Try
            'Server Status Input Parameter
            arParms(0) = New SqlParameter("bitStatus", SqlDbType.Bit)
            arParms(0).Value = blnStatus

            strsqlstmt = "UPDATE tblSessionControl SET ServerCnnIsOK = @bitStatus"
            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strsqlstmt, arParms)
            RefreshSessionControlTable()
            gblnSvrConnIsUp = blnStatus

        Catch ex As Exception
            Throw New Exception("Error in SetServerCnnStatusInSessCtl" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function GetActualWeight(value As String) As String
        Dim output As StringBuilder = New StringBuilder
        Dim i As Integer

        Try
            For i = 0 To value.Length - 1
                If IsNumeric(value(i)) Or value(i) = "." Then
                    output.Append(value(i))
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error in GetActualWeight" & vbCrLf & ex.Message)
        End Try

        Return output.ToString()
    End Function

    Public Function FormatCheckWeigherData(ByVal strReceivedData As String) As Boolean
        Dim intShopOrder As Integer
        Dim strRawValue As String = String.Empty
        Dim strTrimmedValue As String = String.Empty
        Dim decActualWeight As Decimal
        Dim dteTestTime As DateTime = Now()
        Dim strArrayOfWeight As String() = strReceivedData.Split(New String() {vbCrLf, vbCr, vbLf}, StringSplitOptions.None)

        Dim i As Integer = 1
        Try
            For Each strWeight As String In strArrayOfWeight
                If i.ToString = "" Then
                    Return False
                    Exit Function
                Else
                    Dim intPos As Integer
                    intPos = InStr(strWeight, " ")
                    Select Case gstrModel
                        'Case "XE"
                        'strRawValue = strWeight.Substring(31, 9)
                        Case "XC", "XE"
                            strRawValue = Microsoft.VisualBasic.Right(strWeight, strWeight.Length - intPos)
                        Case Else
                    End Select
                    strTrimmedValue = strRawValue.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "")
                    If Trim(strTrimmedValue) <> "" Then
                        decActualWeight = CDec(GetActualWeight(strTrimmedValue))
                        dteTestTime = Now.ToString
                        If gdrSessCtl.ShopOrder <> 0 Then
                            intShopOrder = gdrSessCtl.ShopOrder
                        Else
                            intShopOrder = gintShopOrderBackup
                        End If
                        SaveCheckWeigherLog(decActualWeight, gdrSessCtl.DefaultPkgLine, intShopOrder, gdrSessCtl.StartTime)
                    End If
                    End If
                    i = i + 1
            Next
            Return True
        Catch ex As Exception
            Throw New Exception("Error in FormatCheckWeigherData" & vbCrLf & ex.Message)
        End Try
    End Function

    Private Sub SaveCheckWeigherLog(
           ByVal decActualWeight As Global.System.Nullable(Of Decimal), _
           ByVal strPackagingLine As Global.System.String, _
           ByVal intShopOrder As Global.System.Nullable(Of Integer), _
           ByVal dteSOStartTime As Global.System.Nullable(Of Date)
   )

        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16
        Dim dteTestTime As DateTime = Now()

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(5)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@decActualWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decActualWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrPackagingLine", SqlDbType.Char)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            strSQLStmt = "CPPsp_CheckWeigherLog_Add"  'MH used proper sp name
            Debug.Print("I/O: " & decActualWeight.ToString)
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveCheckWeigherLog" & vbCrLf & ex.Message)
        End Try

    End Sub
End Module
