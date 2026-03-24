Imports System
Imports System.IO
Imports System.Data.SqlClient

Public Class SharedFunctions

    Public Shared Function GetConrolTableValues(strKey As String, strSubKey As String, Optional ByVal strAction As String = Nothing) As Array
        Dim daCT As New dsControlTableTableAdapters.PPsp_Control_SelTableAdapter
        Dim dtCT As New dsControlTable.PPsp_Control_SelDataTable
        Dim strValues() As String = {String.Empty, String.Empty}
        Try
            daCT.Fill(dtCT, strKey, strSubKey, strAction)
            If dtCT.Rows.Count > 0 Then
                strValues(0) = dtCT.Rows(0).Item("Value1")

                If IsDBNull(dtCT.Rows(0).Item("Value2")) Then
                    strValues(1) = String.Empty
                Else
                    strValues(1) = dtCT.Rows(0).Item("Value2")
                End If

            End If
        Catch ex As SqlClient.SqlException
            'Just do nothing to return zero values if network connection failure or failare on SQL 
        Catch ex As Exception
            Throw New Exception("Error in GetConrolTableValues" & vbCrLf & ex.Message)
        Finally
            GetConrolTableValues = strValues
        End Try
    End Function

    'Write print event to the log text file
    Public Shared Sub LogPrintEvent(ByVal strMessage As String)
        Dim strDateTime As String = String.Empty
        Dim strUserName As String = String.Empty
        Dim strFmtMessage As String = String.Empty
        Dim strFileName As String
        Dim strDirectory As String = String.Empty

        Try
            strFileName = My.Settings.strLogFileDir & My.Settings.strLogFileName.Replace("@today@", Now.ToString("yyyyMMdd"))
            'Dim strFileName As String = AppDomain.CurrentDomain.BaseDirectory & "PrintLabelLog.txt"

            strDateTime = Format(DateAndTime.Now, "yyyy-MM-dd hh:mm:ss tt")
            strFmtMessage = String.Format("{0}{1}{2}{3}", strDateTime, vbTab, strMessage, Environment.NewLine)

            'If the log file is not found, create one
            If My.Computer.FileSystem.FileExists(strFileName) = False Then
                My.Computer.FileSystem.WriteAllText(strFileName, String.Empty, False)
                'Clear the old log files that are older than the given retention days
                strDirectory = My.Settings.strLogFileDir
                DeleteLogFile(strDirectory, My.Settings.intLogRetentionDays)
            End If

            File.AppendAllText(strFileName, strFmtMessage)
        Catch ex As Exception
            Throw New Exception("Error in LogPrintEvent -" & vbCrLf & strMessage & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub DeleteLogFile(strDirectory As String, intRetentionDays As Integer)
        Dim fileCreatedDate As DateTime
        Dim strFiles() As String = IO.Directory.GetFiles(strDirectory)
        Dim strfile As String
        Try
            For Each strfile In strFiles
                fileCreatedDate = File.GetCreationTime(strfile)
                If DateDiff(DateInterval.Day, fileCreatedDate, Now) > intRetentionDays Then
                    File.Delete(strfile)
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error in DeleteLogFile" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub SendEmail(strMessage As String, strSubject As String)
        'Send Email Alert
        Dim conn As New SqlConnection
        Dim cmd As New SqlCommand
        Dim previousConnectionState As ConnectionState
        Dim strServerName As String
        Dim strEnvironment As String
        Dim intStartPos As Int32

        Try
            conn.ConnectionString = My.Settings.PPCnnStr
            strServerName = Mid(conn.ConnectionString, InStr(conn.ConnectionString, "Source=") + Len("Source") + 1, InStr(conn.ConnectionString, ";") - (InStr(conn.ConnectionString, "Source=") + Len("Source") + 1))
            'Find out the processing environment id from the catalog of the connection string
            intStartPos = InStr(conn.ConnectionString, "Plant")
            strEnvironment = Mid(conn.ConnectionString, InStr(intStartPos, conn.ConnectionString, "_") + Len("_"), InStr(InStr(intStartPos, conn.ConnectionString, "_"), conn.ConnectionString, ";") - (InStr(intStartPos, conn.ConnectionString, "_") + Len("_")))
            With cmd
                .Connection = conn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "PPsp_SndMsgToOperator"
                .Parameters.AddWithValue("@vchSubject", strServerName & " - " & " Env: " & strEnvironment & " - " & strSubject)
                .Parameters.AddWithValue("@vchMsgBody", strMessage)
                .Parameters.AddWithValue("@vchName", My.Settings.strRecipient)
            End With

            previousConnectionState = conn.State

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw New Exception("Error in SendEmail" & vbCrLf & ex.Message)
        Finally
            If previousConnectionState = ConnectionState.Closed Then
                conn.Close()
            End If
        End Try

    End Sub

End Class
