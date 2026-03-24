Public Class clsCoLOSCLI
    Dim CoLOS_CLI As clsTCPClient
    Dim strResponse As String = Nothing
    Dim StrIPAddress As String = Nothing
    Dim intPort As Integer
    'strIPAddress = "10.21.104.45"

    Private newPropertyValue As Integer

    Public WriteOnly Property CoLOS_IPAddress() As String
        Set(ByVal value As String)
            StrIPAddress = value
        End Set
    End Property

    Public WriteOnly Property CoLOS_Port() As Integer
        Set(ByVal value As Integer)
            intPort = value
        End Set
    End Property

    Public Sub New()
        CoLOS_CLI = New clsTCPClient()
    End Sub

    Public Sub New(IPAddress As String, Port As Integer)
        StrIPAddress = IPAddress
        intPort = Port
        CoLOS_CLI = New clsTCPClient(StrIPAddress, intPort)
    End Sub
    ' Public Function LogOnToCoLOS(ByVal strIP As String) As String
    Public Function LogOnToCoLOS() As String
        Try
            Return CoLOS_CLI.tcpConnect("Login|CLIUser|cliuser1")
        Catch ex As Exception
            Throw New Exception("Error in LogOnToCoLOS" & vbCrLf & ex.Message)
        End Try
    End Function

    'Public Function LogOffFromCoLOS(ByVal strIP As String) As String
    Public Function LogOffFromCoLOS() As String

        Try
            CoLOS_CLI.tcpDisconnect("Logout")
            Return strResponse
        Catch ex As Exception
            Throw New Exception("Error in LogOffFromCoLOS" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function SendMessage(ByVal strMessage As String) As String
        Try
            strResponse = CoLOS_CLI.TCPSendMessage(strMessage)
            Return strResponse
        Catch ex As Exception
            Throw New Exception("Error in SendMessage" & vbCrLf & ex.Message)
        End Try
    End Function
End Class
