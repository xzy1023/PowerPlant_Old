Public Class clsCheckWeigher
    Inherits clsTCPClient

    Private thd As System.Threading.Thread
    Private blnCounterIsStarted As Boolean
    Dim CheckWeigher As clsTCPClient
    Dim strResponse As String = Nothing
    'Dim strIPAddress As String
    'Dim intPort As Integer

    'WD_SET_PROT(SPACE)X(CR)(LF)
    '192.168.50.91
    'WD_SET_FORMAT(SPACE)X(CR)(LF)
    'WD_SET_FORMAT 1
    'WD_SET_FORMAT 2 (default)
    'WD_SET_FORMAT 2100
    'WD_START
    'WD_STOP

    Public Sub New()
    End Sub

    Public Sub New(IPAddress As String, Port As Integer)
        Try
            MyBase.IPAddress = IPAddress
            MyBase.Port = Port
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function IsHandShakeOK() As Boolean
        Dim newString As String = String.Empty
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

    Public Function Start() As String
        Try
            Dim strResponse As String = String.Empty
            blnCounterIsStarted = True
            strResponse = TCPSendMessage("WD_START")
            Return strResponse
        Catch ex As Exception
            Throw New Exception("Error in Start." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Sub Abort()
        Try
            TCPSendMessage("WD_STOP")
            blnCounterIsStarted = False
        Catch ex As Exception
            Throw New Exception("Error in Abort." & vbCrLf & ex.Message)
        End Try
    End Sub

End Class
