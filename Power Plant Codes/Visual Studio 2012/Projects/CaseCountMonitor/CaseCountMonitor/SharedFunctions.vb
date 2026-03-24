Imports System.Xml
Imports System.Data.SqlClient
Imports System.IO

Public Class SharedFunctions
    Public Structure stuLineInfoXML
        Dim Line As String
        Dim ShopOrder As Integer
        Dim SOStartTime As String
        Dim CaseCount As Integer
    End Structure


    Public Shared Function GetLineInfoXML(strFileName As String) As stuLineInfoXML

        Dim stu As stuLineInfoXML = Nothing
        Dim xdcCaseLabels As XmlDocument
        Dim xnlCaseLabels As XmlNodeList
        Dim xndCaseLabels As XmlNode

        Try

            xdcCaseLabels = New XmlDocument()
            xdcCaseLabels.Load(strFileName)
            xnlCaseLabels = xdcCaseLabels.SelectNodes("/CaseLabels/CaseLabel")

            For Each xndCaseLabels In xnlCaseLabels
                For i As Integer = 0 To xndCaseLabels.ChildNodes.Count - 1
                    Select Case xndCaseLabels.ChildNodes.Item(i).Name
                        Case "PackagingLine"
                            stu.Line = xndCaseLabels.ChildNodes.Item(i).InnerText
                        Case "ShopOrder"
                            stu.ShopOrder = CType(xndCaseLabels.ChildNodes.Item(i).InnerText, Integer)
                        Case "CaseCount"
                            stu.CaseCount = Integer.Parse(xndCaseLabels.ChildNodes.Item(i).InnerText)
                        Case "SOStartTime"
                            stu.SOStartTime = xndCaseLabels.ChildNodes.Item(i).InnerText
                    End Select
                Next
            Next
            Return stu

        Catch ex As Exception
            Throw New Exception("Error in GetLineInfoXML" & vbCrLf & ex.Message)
        Finally
            xdcCaseLabels = Nothing
            xnlCaseLabels = Nothing
            xndCaseLabels = Nothing
            stu = Nothing
        End Try
    End Function

    Public Shared Function buildIPCConnectionList() As DataTable
        Dim dtLineInfo As New DataTable
        Dim dirInput As IO.DirectoryInfo
        Dim arrFiles As IO.FileInfo()  'this is array(Of FileInfo)

        Dim da As SqlDataAdapter = Nothing
        Dim daCmpCfg As New dsCmpCfgTableAdapters.taComputerConfig
        Dim dtCmpCfg As New dsCmpCfg.dtComputerConfigDataTable

        Try

            With dtLineInfo.Columns
                .Add("Line", GetType(String))
                .Add("ComputerName", GetType(String))
                .Add("CasesProduced", GetType(Integer))
            End With

            dirInput = New IO.DirectoryInfo(My.Settings.gstrXMLInputFolder)
            arrFiles = dirInput.GetFiles()

            daCmpCfg.Fill(dtCmpCfg, "SelectAllFields", Nothing, Nothing)

            'WO#1297Dim result = From cf In dtCmpCfg
            'WO#1297   Join ln In arrFiles On cf.PackagingLine.Trim Equals Path.GetFileNameWithoutExtension(ln.FullName)
            'WO#1297 Where cf.ProcessType Like "*3,4*"
            Dim result = From cf In dtCmpCfg
                Join ln In arrFiles On cf.PackagingLine.Trim Equals Path.GetFileNameWithoutExtension(ln.FullName)
                Where Not IsDBNull(cf.AutoCountUnit) And cf.InterfaceType = "XML"
                Select New With {cf.ComputerName, cf.PackagingLine}
            'WO6314 Where cf.EnableAutoCaseCountLine = True
            'WO6314   Select New With {cf.ComputerName, cf.PackagingLine}

            For Each line In result
                dtLineInfo.Rows.Add(line.PackagingLine, line.ComputerName, 0)
            Next

            Return dtLineInfo

        Catch ex As Exception
            Throw New Exception("Error in buildIPCConnectionList" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function UpdateCaseCountToIPC(ByVal strComputerName As String, ByVal strLine As String, ByVal intShopOrder As Integer, ByVal strSOStartTime As String, ByVal intCaseCount As Integer) As Exception
        Dim cmd As New SqlCommand
        Dim cnn As New SqlConnection
        Dim strSQLStmt As String

        Try
            UpdateCaseCountToIPC = Nothing
            strSQLStmt = "CPPsp_SessionControlMaintenance"
            cnn.ConnectionString = My.Settings.IPCCnnStr.Replace(".\SQLEXPRESS", strComputerName & "\SQLEXPRESS")
            Try
                cnn.Open()
            Catch ex As Exception
                Return ex
            End Try

            With cmd
                .Connection = cnn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strSQLStmt

                .Parameters.Add(New SqlParameter("@vchAction", SqlDbType.VarChar, 50)).Value = "UpdateCaseCount"
                .Parameters.Add(New SqlParameter("@chrDefaultPkgLine", SqlDbType.VarChar, 10)).Value = strLine
                .Parameters.Add(New SqlParameter("@dteSOStartTime", SqlDbType.VarChar, 23)).Value = strSOStartTime
                .Parameters.Add(New SqlParameter("@intShopOrder", SqlDbType.Int)).Value = intShopOrder
                .Parameters.Add(New SqlParameter("@intQuantity", SqlDbType.Int)).Value = intCaseCount

                .ExecuteNonQuery()
            End With

        Catch ex As Exception
            Throw New Exception("Error in UpdateCaseCountToIPC" & vbCrLf & ex.Message)
        Finally
            cnn.Close()
        End Try
    End Function

    Public Shared Sub SendEmail(strMessage As String)
        'WO#6314    Try
        Dim conn As New SqlConnection
        Dim cmd As New SqlCommand
        Dim previousConnectionState As ConnectionState
        Dim strServerName As String
        Dim strEnvironment As String
        Try     'WO#6314
            conn.ConnectionString = My.Settings.PPCnnStr
            strServerName = Mid(conn.ConnectionString, InStr(conn.ConnectionString, "Source=") + Len("Source") + 1, InStr(conn.ConnectionString, ";") - (InStr(conn.ConnectionString, "Source=") + Len("Source") + 1))
            strEnvironment = Mid(conn.ConnectionString, InStr(conn.ConnectionString, "Plant_") + Len("Plant_"), InStr(InStr(conn.ConnectionString, "Plant_"), conn.ConnectionString, ";") - (InStr(conn.ConnectionString, "Plant_") + Len("Plant_")))
            With cmd
                .Connection = conn
                .CommandType = CommandType.StoredProcedure
                'WO#6314    .CommandText = "PPsp_SndMsgForSupport"
                .CommandText = "PPsp_SndMsgToOperator"          'WO#6314
                .Parameters.AddWithValue("@vchSubject", strServerName & " - Case Count Monitor is failure. Env: " & strEnvironment)
                .Parameters.AddWithValue("@vchMsgBody", strMessage)
                .Parameters.AddWithValue("@vchName", My.Settings.strRecipient)                   'WO#6314
            End With

            previousConnectionState = conn.State
            'WO#6314    Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            cmd.ExecuteNonQuery()

            'WO#6314    Finally
            'WO#6314    If previousConnectionState = ConnectionState.Closed Then
            'WO#6314       conn.Close()
            'WO#6314    End If
            'WO#6314    End Try
        Catch ex As Exception
            Throw New Exception("Error in SendEmail" & vbCrLf & ex.Message)         'WO#6314
        Finally
            If previousConnectionState = ConnectionState.Closed Then
                conn.Close()
            End If
        End Try

    End Sub
End Class
