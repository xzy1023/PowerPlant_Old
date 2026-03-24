'FX150505 Add Start
Public Class frmShutDown

    Private Sub frmShutDown_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        gstrPrvForm = Me.Name
    End Sub

    Private Sub frmShutDown_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            UcHeading1.ScreenTitle = "Shut Down Computer "
            gstrPrvForm = Name

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnShutdown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShutDown.Click
        System.Diagnostics.Process.Start("shutdown", "-s -t 00")
        'This will make the computer Shutdown
    End Sub

    Private Sub btnRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRestart.Click
        System.Diagnostics.Process.Start("shutdown", "-r -t 00")
        'This will make the computer Restart
    End Sub

    Private Sub btnLogOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOff.Click
        'System.Diagnostics.Process.Start("shutdown", "-l -t -f 00")
        Shell("shutdown -l")
        'This will make the computer Log Off 
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

End Class
'FX150505 Add Stop