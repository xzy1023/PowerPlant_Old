Public Class ucHeading

    Private Sub ucHeading_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'WO#755 ADD Start
        If InStr(1, My.Settings.ServerPowerPlantCnnStr, "PowerPlant_Prd", CompareMethod.Text) > 0 Then
            Me.BackColor = Color.Blue
        Else
            Me.BackColor = Color.YellowGreen
        End If
        'WO#755 ADD Stop
        Timer1_Tick(sender, e)
        Timer1.Interval = 1000
        Timer1.Start()
    End Sub
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        lblDate.Text = Date.Today.ToShortDateString
        lblTime.Text = Format(Now(), "HH:mm:ss")
    End Sub
    Public Property ScreenTitle() As String
        Get
            ScreenTitle = lblScreenTitle.Text
        End Get
        Set(ByVal value As String)
            lblScreenTitle.Text = value
        End Set
    End Property
End Class
