Imports System.Threading

Public Class clsMidnightScheduler
    Private _timer As Timer
    Private _callback As Action

    Public Sub New(callback As Action)
        _callback = callback
        CalculateAndStartTimer()
    End Sub

    Private Sub CalculateAndStartTimer()
        ' Calculate the time until the next midnight
        Dim firstTimeDueTime = DateTime.Now.Date.AddDays(1) - DateTime.Now
        ' Start the timer: first run at next midnight, then every 24 hours
        _timer = New Timer(AddressOf TimerCallback, Nothing, firstTimeDueTime, TimeSpan.FromHours(24))
    End Sub

    Private Sub TimerCallback(state As Object)
        ' Call the provided callback (should be UI-thread safe)
        If _callback IsNot Nothing Then
            _callback.Invoke()
        End If
    End Sub

    Public Sub Dispose()
        _timer?.Dispose()
    End Sub
End Class