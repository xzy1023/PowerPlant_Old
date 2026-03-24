'WO#5370 ADD Start
Imports System.Windows.Forms

Public Class frmSplash
    Dim dteStartTime As DateTime
    Dim intCount As Integer
    Dim strOption As String
    Dim strMessage As String

    Private Sub frmSplash_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Timer1.Stop()
        Timer2.Stop()
    End Sub

    Private Sub frmSplash_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Timer1 is for displaying the elapse time
        dteStartTime = Now()
        Timer1_Tick(sender, e)
        Timer1.Interval = 1000
        Timer1.Enabled = True
        Timer1.Start()

        Select Case strOption
            Case "PalletCreation"
                'System.Threading.Thread.Sleep(30000)
                pbxLoading.Visible = False
                lblElapseTime.Visible = True
                btnExit.Visible = True
                'Timer2 is for displaying the dots after the text Please Wait..
                Timer2_Tick(sender, e)
                Timer2.Enabled = True
                Timer2.Interval = 700
                Timer2.Start()

            Case "ImportData"
                lblElapseTime.Visible = False
                btnExit.Visible = False
                pbxLoading.Visible = True
                'WO#17432 ADD Start
            Case "ElapseTime"
                lblElapseTime.Visible = True
                btnExit.Visible = False
                pbxLoading.Visible = False
                'WO#17432 ADD Stop
            Case Else

        End Select


    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblElapseTime.Text = TimeSpan.FromSeconds(DateDiff(DateInterval.Second, dteStartTime, Now())).ToString
        If strOption = "PalletCreation" Then
            If SharedFunctions.IsPalletCreated(gdrSessCtl) Then
                Me.Close()
            End If
        End If
    End Sub

    Public WriteOnly Property SplashTitle() As String
        Set(ByVal value As String)
            Me.Text = value
        End Set
    End Property

    Public WriteOnly Property SplashMessage() As String
        Set(ByVal value As String)
            lblMessage.Text = value
            strMessage = value
        End Set
    End Property

    Public WriteOnly Property SplashOption() As String
        Set(ByVal value As String)
            strOption = value
        End Set
    End Property

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If intCount = 15 Then
            lblMessage.Text = strMessage
            intCount = 0
        Else
            lblMessage.Text = lblMessage.Text & ". "
        End If
        intCount += 1

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    'WO#17432 DEL Start
    'Private Sub frmSplash_Shown(sender As Object, e As EventArgs) Handles Me.Shown

    'End Sub
    'WO#17432 DEL Stop
End Class
'WO#5370 ADD Stop