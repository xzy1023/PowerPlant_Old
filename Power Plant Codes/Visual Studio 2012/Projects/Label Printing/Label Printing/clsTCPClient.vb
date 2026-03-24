Imports System.Net.Sockets

Public Class clsTCPClient
    Dim client As New System.Net.Sockets.TcpClient
    Dim strIP As String
    Dim intPort As Integer
    Public Const strResponseOK As String = "OK|0000|"

    Public Sub New()
        'default construction
    End Sub

    Public Sub New(ByVal IPAddress As String, ByVal Port As Integer)
        strIP = IPAddress
        intPort = Port
    End Sub

    Public Sub New(ByVal IPAddress As String)
        strIP = IPAddress
    End Sub

    Public Property IPAddress() As String
        Get
            Return strIP
        End Get
        Set(ByVal value As String)
            'Dim strReturn As String = Nothing
            strIP = value
            'strReturn = CheckIPAddress(strIP)
            'If Not IsNothing(strReturn) Then
            '    Throw New Exception(strReturn)
            'End If
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

    Public Function TCPSendMessage(ByVal strMessage As String) As String
        Dim strResponse As String = Nothing
        Dim btyBuffer() As Byte
        Const intMaxMassageLength As Int16 = 1000
        Dim bytes(intMaxMassageLength - 1) As Byte
        Try
            strResponse = CheckIPAddress(strIP)
            If IsNothing(strResponse) Then
                strMessage = strMessage & vbCrLf
                btyBuffer = System.Text.Encoding.Default.GetBytes(strMessage.ToCharArray)

                With client
                    .GetStream.Write(btyBuffer, 0, btyBuffer.Length)
                    .GetStream.ReadTimeout = 1000   '1 second
                    Try
                        'If the return data is bland, try to read again.
                        For intCount As Int16 = 1 To 3
                            Threading.Thread.Sleep(1000)
                            client.GetStream.Read(bytes, 0, bytes.Length)
                            strResponse = System.Text.Encoding.ASCII.GetString(bytes)

                            If strResponse.Contains(strResponseOK) Then
                                Exit For
                            End If
                            If strResponse = String.Empty Then
                                Exit For
                            Else
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        If ex.Message.Contains("Unable to read data from the transport connection:") Then
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
            If client.Connected Then
                client.Close()
            End If
            Throw New Exception("Error in TCPSenMessage." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Sub TCPSendCommand(ByVal strMessage As String)
        Const intMaxMassageLength As Int16 = 1000

        Dim strResponse As String = Nothing
        Dim btyBuffer() As Byte
        Dim bytes(intMaxMassageLength - 1) As Byte

        Try
            strMessage = strMessage & vbCrLf
            btyBuffer = System.Text.Encoding.Default.GetBytes(strMessage.ToCharArray)

            With client
                .GetStream.Write(btyBuffer, 0, btyBuffer.Length)
            End With

        Catch ex As Exception
            If client.Connected Then
                client.Close()
            End If
            Throw New Exception("Error in SendCommand." & vbCrLf & ex.Message)
        End Try

    End Sub
    ' WO#17432 ADD Stop – AT 10/05/2018

    Public Function tcpConnect(strMessage As String) As String
        Dim strReturn As String = Nothing
        Try
            'If Not client.Connected Then
            tcpConnect()
            'End If
            strReturn = TCPSendMessage(strMessage)
            Return strReturn
        Catch ex As Exception
            If client.Connected Then
                client.Close()
            End If
            Throw New Exception("Error in tcpConnect with message." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function TCPConnect() As String
        Dim strReturn As String = Nothing
        Try
            If Not client.Connected Then
                strReturn = CheckIPAddress(strIP)
                If IsNothing(strReturn) Then
                    client.Connect(strIP, intPort)
                End If
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
            client.Close()
        Catch ex As Exception
            Throw New Exception("Error in TCPDisconnect." & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function TCPDisconnect(strLogOutMessage As String) As String
        Dim strReturn As String = Nothing
        Try
            strReturn = TCPSendMessage(strLogOutMessage)
            client.Close()
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error in TCPDisconnect with log-out" & vbCrLf & ex.Message)
        End Try
    End Function

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
            Throw New Exception("Error in CheckIPAddress" & vbCrLf & ex.Message)
        End Try
    End Function
End Class
