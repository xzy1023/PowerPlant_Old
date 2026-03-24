Public Class clsCoLOSCLI
    Inherits clsTCPClient

    'Dim CoLOS_CLI As clsTCPClient
    Dim strResponse As String = Nothing
    Dim StrIPAddress As String = Nothing
    Dim intPort As Integer

    'Private newPropertyValue As Integer

    'Public WriteOnly Property CoLOS_IPAddress() As String
    '    Set(ByVal value As String)
    '        StrIPAddress = value
    '    End Set
    'End Property

    'Public WriteOnly Property CoLOS_Port() As Integer
    '    Set(ByVal value As Integer)
    '        intPort = value
    '    End Set
    'End Property

    'Public Sub New()
    '    CoLOS_CLI = New clsTCPClient()
    'End Sub

    'Public Sub New(IPAddress As String, Port As Integer)
    '    StrIPAddress = IPAddress
    '    intPort = Port
    '    CoLOS_CLI = New clsTCPClient(StrIPAddress, intPort)
    'End Sub

    Public Function LogInToCoLOS() As String
        Try
            'Return CoLOS_CLI.TCPConnect("Login|CLIUser|cliuser1")
            'Return tcpConnect("Login|CLIUser|cliuser1")
            Return tcpConnect(My.Settings.strCLILogIn)
        Catch ex As Exception
            Throw New Exception("Error in LogInToCoLOS" & vbCrLf & ex.Message)
        End Try
    End Function

    'Public Function LogOffFromCoLOS(ByVal strIP As String) As String
    Public Function LogOutFromCoLOS() As String

        Try
            'CoLOS_CLI.TCPDisconnect("Logout")
            'TCPDisconnect("Logout")
            'Return strResponse
            Return SendMessage("Logout")
        Catch ex As Exception
            Throw New Exception("Error in LogOutFromCoLOS" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function SendMessage(ByVal strMessage As String) As String
        Try
            'strResponse = CoLOS_CLI.TCPSendMessage(strMessage)
            strResponse = MyBase.TCPSendMessage(strMessage)
            Return strResponse
        Catch ex As Exception
            Throw New Exception("Error in SendMessage" & vbCrLf & ex.Message)
        End Try
    End Function
End Class
