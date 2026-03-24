Imports System.Net.Sockets
Imports System.Text
Imports System.Collections
Imports System.Threading

Public Class clsCheckWeigher
    Dim client As New System.Net.Sockets.TcpClient
    Dim strIP As String
    Dim intPort As Integer
    Dim netStream As NetworkStream
    Dim blnCounterIsStarted As Boolean

    Dim QueData As New Queue
    Dim thdSaveData As System.Threading.Thread

    Public Property IPAddress() As String
        Get
            Return strIP
        End Get
        Set(ByVal value As String)
            strIP = value
        End Set
    End Property

    Public Property Port() As Integer
        Get
            Return intPort
        End Get
        Set(ByVal value As Integer)
            intPort = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(IPAddress As String, Port As Integer)
        Dim strErrMsg As String = Nothing
        strIP = IPAddress
        intPort = Port

        strErrMsg = CheckIPAddress(strIP)
        If Not IsNothing(strErrMsg) Then
            Throw New Exception(strErrMsg)
        End If
    End Sub

    Public Function IsHandShakeOK() As Boolean
        Dim newString As String = String.Empty
        Dim strResponse As String = Nothing
        Try
            newString = TCPSendMessage("WD_TEST")
            strResponse = newString.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "")
            If strResponse = "WD_OK" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsHandShakeOK." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Sub Abort()
        Try
            TCPSendMessage("WD_STOP")
            blnCounterIsStarted = False
            If thdSaveData.IsAlive Then
                thdSaveData.Abort()
            End If
        Catch ex As Exception
            Throw New Exception("Error in Abort." & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function CheckIPAddress(strIP As String) As String

        Dim objIPAddress As System.Net.IPAddress = Nothing
        Dim strReturn As String = Nothing
        Try
            If Not System.Net.IPAddress.TryParse(strIP, objIPAddress) Then
                strReturn = String.Format("IP address {0} is not in the right format.", strIP)
            ElseIf My.Computer.Network.Ping(strIP) = False Then
                strReturn = String.Format("Network connection issue, cannot connect to IP address {0}.", strIP)
            End If
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error in CheckIPAddress." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function TCPSendMessage(ByVal strMessage As String) As String

        Dim strResponse As String = String.Empty
        Dim btyBuffer() As Byte
        Dim intBytesRead As Integer = 0
        Dim sbAllData As StringBuilder = New StringBuilder()
        Const intMaxMassageLength As Int16 = 1024
        Dim bytes(intMaxMassageLength - 1) As Byte

        Try
            strResponse = CheckIPAddress(strIP)
            If IsNothing(strResponse) Then
                strMessage = strMessage & vbCrLf
                btyBuffer = System.Text.Encoding.Default.GetBytes(strMessage.ToCharArray)

                With client
                    netStream.Write(btyBuffer, 0, btyBuffer.Length)

                    Try
                        'If the return data is bland, try to read again.
                        For intCount As Int16 = 1 To 3
                            Threading.Thread.Sleep(1000)
                            Do
                                intBytesRead = netStream.Read(bytes, 0, bytes.Length)
                                sbAllData.AppendFormat("{0}", System.Text.Encoding.ASCII.GetString(bytes, 0, intBytesRead))
                            Loop While netStream.DataAvailable
                            strResponse = sbAllData.ToString()

                            If strResponse <> String.Empty Then
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        If ex.Message.Contains("Unable to read data from the transport connection:") Then
                            'this normal exception is come from netStream.Read as no data has been read.
                            strResponse = "No Data"
                            Return strResponse
                        Else
                            Throw New Exception(ex.Message)
                        End If
                    End Try

                End With

            End If
            Return strResponse
        Catch ex As Exception
            TCPDisconnect()
            Throw New Exception("Error in TCPSenMessage." & vbCrLf & ex.Message)
        End Try

    End Function

    Public Sub TCPSendCommand(ByVal strMessage As String)

        Dim btyBuffer() As Byte

        Try
            strMessage = strMessage & vbCrLf
            btyBuffer = System.Text.Encoding.Default.GetBytes(strMessage.ToCharArray)

            With client
                netStream.Write(btyBuffer, 0, btyBuffer.Length)
            End With

        Catch ex As Exception
            TCPDisconnect()
            Throw New Exception("Error in SendCommand." & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Function ReceiveData() As String
        Dim bytBuffer(client.ReceiveBufferSize) As Byte
        Dim intBytesRead As Integer = 0
        Dim sbAllData As StringBuilder = New StringBuilder()

        Try

            Do
                intBytesRead = netStream.Read(bytBuffer, 0, bytBuffer.Length)
                sbAllData.AppendFormat("{0}", System.Text.Encoding.ASCII.GetString(bytBuffer, 0, intBytesRead))
            Loop While netStream.DataAvailable

            Return sbAllData.ToString
        Catch ex As Exception When ex.Message.Contains("Unable to read data from the transport connection")
            If ex.Message.Contains("was forcibly closed by the remote host") Then
                Throw New Exception("Error in ReceiveData." & vbCrLf & ex.Message)
            Else
                Threading.Thread.Sleep(1000)
                Return sbAllData.ToString
            End If

        Catch ex As Exception
            Throw New Exception("Error in ReceiveData." & vbCrLf & ex.Message)
        End Try

    End Function

    Public Function TCPConnect() As String
        Dim strReturn As String = Nothing
        Try
            strReturn = CheckIPAddress(strIP)
            If IsNothing(strReturn) Then
                If client.Connected Then
                    TCPDisconnect()
                End If
                client.Connect(strIP, intPort)
                netStream = client.GetStream()
                netStream.ReadTimeout = 1000   '1 second
            End If
            Return strReturn
        Catch ex As Exception
            If client.Connected Then
                TCPDisconnect()
            End If
            If ex.Message.Contains("No connection could be made") _
                    Or ex.Message.Contains("No such host is known") _
                    Or ex.Message.Contains("Unable to ping") Then
                strReturn = ex.Message
                Return strReturn
            Else
                Throw New Exception("Error in TCPConnect." & vbCrLf & ex.Message)
            End If
        End Try
    End Function

    Public Sub TCPDisconnect()
        Try
            If client.Connected Then
                client.Close()
            End If
            netStream.Close()
        Catch ex As Exception
            Throw New Exception("Error in tcpDisconnect." & vbCrLf & ex.Message)
        End Try
    End Sub
End Class
