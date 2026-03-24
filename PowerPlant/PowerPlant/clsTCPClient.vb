Imports System.Net.Sockets

Public Class clsTCPClient
    Dim client As New System.Net.Sockets.TcpClient
    Dim strIP As String
    Dim intPort As Integer

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

    Public Function TCPReceiveMessage() As String
        Dim netStream As NetworkStream = client.GetStream()
        Dim bytes(client.ReceiveBufferSize) As Byte


        Dim strResponse As String = Nothing
        Try
            With client
                ' client.GetStream.Write(btyBuffer, 0, btyBuffer.Length)
                .GetStream.ReadTimeout = 5000   '5 second
                Try
                    'client.GetStream.Read(bytes, 0, bytes.Length)
                    netStream.Read(bytes, 0, CInt(.ReceiveBufferSize))
                    'If the return data is blank, try to read again.
                    For intCount As Int16 = 1 To 3
                        Threading.Thread.Sleep(1000)
                        client.GetStream.Read(bytes, 0, bytes.Length)
                        strResponse = System.Text.Encoding.ASCII.GetString(bytes)
                        If strResponse <> String.Empty Then
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    If ex.Message.Contains("Unable to read data from the transport connection:") Or _
                            ex.Message.Contains("not allowed on non-connected sockets") Then
                        strResponse = "No Data"
                        Return strResponse
                    Else
                        Throw New Exception("Error in TCPReceiveMessage." & vbCrLf & ex.Message)
                    End If
                End Try
                Return strResponse
            End With
        Catch ex As Exception
            If client.Connected Then
                client.Close()
            End If
            Throw New Exception("Error in TCPReceiveMessage." & vbCrLf & ex.Message)
        End Try
    End Function

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
                    Try     'WO#17432 BL 2019/08/22 
                        .GetStream.ReadTimeout = 5000   '5 second
                        .GetStream.WriteTimeout = 5000
                        .GetStream.Write(btyBuffer, 0, btyBuffer.Length)
                        'WO#17432 BL 2019/08/22         Try
                        'If the return data is bland, try to read again.
                        For intCount As Int16 = 1 To 3
                            Threading.Thread.Sleep(1000)
                            client.GetStream.Read(bytes, 0, bytes.Length)
                            strResponse = System.Text.Encoding.ASCII.GetString(bytes)
                            If strResponse <> String.Empty Then
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        'WO#17432 BL 2019/08/22 If ex.Message.Contains("Unable to read data from the transport connection:") Then
                        If ex.Message.Contains("Unable to read data from the transport connection:") Or _
                            ex.Message.Contains("not allowed on non-connected sockets") Then                    'WO#17432 BL 2019/08/22 
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

    ' WO#17432 ADD Start – AT 10/05/2018
    Public Sub TCPSendCommand(ByVal strMessage As String)

        Dim strResponse As String = Nothing
        Dim btyBuffer() As Byte

        Const intMaxMassageLength As Int16 = 1000

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

    Public Function tcpConnect(strLogInMessage As String) As String
        Dim strReturn As String = Nothing
        Try
            client.Connect(strIP, intPort)
            strReturn = TCPSendMessage(strLogInMessage)
            Return strReturn
        Catch ex As Exception
            If client.Connected Then
                client.Close()
            End If
            Throw New Exception("Error in tcpConnect with log-in." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function TCPConnect() As String
        Dim strReturn As String = Nothing
        Try
            strReturn = CheckIPAddress(strIP)
            If IsNothing(strReturn) Then  
                client.Connect(strIP, intPort)
            End If
            Return strReturn
        Catch ex As Exception
            If client.Connected Then
                TCPDisconnect()
            End If
            'WO#17432 BL 12/10/2018 If ex.Message.Contains("No connection could be made") Then
            If ex.Message.Contains("No connection could be made") _
                    Or ex.Message.Contains("No such host is known") _
                    Or ex.Message.Contains("Unable to ping") Then               'WO#17432 BL 12/10/2018 
                'strReturn = "No Connection"
                strReturn = ex.Message
                Return strReturn
                'WO#17432 DEL Start - BL 12/10/2018 
                '    ' WO#17432 ADD Start – AT 10/31/2018
                'ElseIf ex.Message.Contains("No such host is known") Then
                '    'strReturn = "No Connection"
                '    strReturn = ex.Message
                '    Return strReturn
                '    ' WO#17432 ADD Stop – AT 10/31/2018
                'WO#17432 DEL Start - BL 12/10/2018 
            Else
                Throw New Exception("Error in TCPConnect." & vbCrLf & ex.Message)
            End If
        End Try
    End Function

    Public Sub TCPDisconnect()
        Try
            client.Close()
        Catch ex As Exception
            Throw New Exception("Error in tcpDisconnect." & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub tcpDisconnect(strLogOutMessage As String)
        Try
            TCPSendMessage(strLogOutMessage)
            client.Close()
        Catch ex As Exception
            Throw New Exception("Error in tcpDisconnect with log-out" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function CheckIPAddress(strIP As String) As String
        Dim objIPAddress As System.Net.IPAddress = Nothing
        Dim strReturn As String = Nothing
        If Not System.Net.IPAddress.TryParse(strIP, objIPAddress) Then
            strReturn = String.Format("IP address {0} is not in the right format.", strIP)
        ElseIf My.Computer.Network.Ping(strIP) = False Then
            strReturn = String.Format("Network connection issue, cannot connect to IP address {0}.", strIP)
        End If
        Return strReturn
    End Function
End Class
