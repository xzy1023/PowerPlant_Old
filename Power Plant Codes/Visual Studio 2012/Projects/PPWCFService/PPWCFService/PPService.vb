Imports System.Net.Sockets
Imports System.Text
' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in both code and config file together.
Public Class PPService
    Implements PPIService

    Dim strSource As String = "PPWCFServer"
    Dim strLog As String = "Application"
    Dim strEvent As String
    Dim EL As EventLog

    Public Function GetData(ByVal value As Integer) As String Implements PPIService.GetData
        Return String.Format("You entered: {0}", value)
    End Function

    Public Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType Implements PPIService.GetDataUsingDataContract
        If composite Is Nothing Then
            Throw New ArgumentNullException("composite")
        End If
        If composite.BoolValue Then
            composite.StringValue &= "Suffix"
        End If
        Return composite
    End Function

    Public Sub ShowForm(ByVal strFormID As String, ByVal strInterfaceID As String, ByVal strQATWorkFlowType As String) Implements PPIService.ShowForm
        Dim strAllInterfaceData As String = String.Empty
        Dim tcpClient As New System.Net.Sockets.TcpClient()
        Dim networkStream As NetworkStream
        Dim sendBytes As [Byte]()
        Dim bytes() As Byte
        Dim strReturndata As String = String.Empty
        Dim strMsg As String = String.Empty

        Try
            tcpClient.Connect("127.0.0.1", 8000)
            networkStream = tcpClient.GetStream()
            If networkStream.CanWrite And networkStream.CanRead Then
                ' Do a simple write.
                If strFormID = "" Then
                    strAllInterfaceData = "inquiry"
                Else
                    strAllInterfaceData = strFormID & " " & strInterfaceID & " " & strQATWorkFlowType
                End If

                'Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(strAllInterfaceData
                sendBytes = Encoding.ASCII.GetBytes(strAllInterfaceData)
                networkStream.Write(sendBytes, 0, sendBytes.Length)

                ' Read the NetworkStream into a byte buffer.
                ReDim bytes(tcpClient.ReceiveBufferSize)
                networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
                ' Output the data received from the host to the console.
                strReturndata = Encoding.ASCII.GetString(bytes)
                'Console.WriteLine(("Host returned: " + returndata))
            Else
                If Not networkStream.CanRead Then
                    ' Console.WriteLine("cannot not write data to this stream")
                    strMsg = "Cannot write data from this stream."
                Else
                    If Not networkStream.CanWrite Then
                        'Console.WriteLine("cannot read data from this stream")
                        strMsg = "Cannot read data from this stream."
                    End If
                End If
                WriteToEventLog(strLog, strSource, strMsg, EventLogEntryType.Information)
                tcpClient.Close()
            End If
        Catch ex As Exception
            WriteToEventLog(strLog, strSource, ex.Message, EventLogEntryType.Error)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub WriteToEventLog(ByVal strLogName As String, ByVal strSource As String, ByVal strErrDetail As String, ByVal elet As EventLogEntryType)
        Dim SQLEventLog As System.Diagnostics.EventLog = New System.Diagnostics.EventLog()

        Try
            If My.Settings.blnLogEvent = True Or elet = EventLogEntryType.Error Then
                If Not System.Diagnostics.EventLog.SourceExists(strLogName) Then
                    System.Diagnostics.EventLog.CreateEventSource(strSource, strLog)
                End If

                SQLEventLog.Source = strLogName
                SQLEventLog.WriteEntry(strSource & " - " & strErrDetail, elet)
            End If

        Catch ex As Exception
            SQLEventLog.Source = strLogName
            SQLEventLog.WriteEntry("INFORMATION: " & ex.Message, EventLogEntryType.Information)
        Finally
            SQLEventLog.Dispose()
            SQLEventLog = Nothing
        End Try
    End Sub

End Class
