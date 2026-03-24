Imports System.Windows.Forms

Public Class frmMsgBox
    Dim gBtnFont As New Font("Arial", 17.25, FontStyle.Bold, GraphicsUnit.Point)
    Private Sub frmMsgBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case Me.Tag
            Case MessageBoxButtons.OK
                Build_OK_Button()
            Case MessageBoxButtons.YesNo
                Build_YesNo_Button()
        End Select
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Yes_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub
    Private Sub No_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Close()
    End Sub
    Private Sub Build_OK_Button()
        Dim btn_OK As New Button
        Dim btn_Yes As New Button
        Dim btn_NO As New Button
        btn_OK.Text = "OK"
        With btn_OK
            .Location = New Point(248, 251)
            .Size = New Size(150, 65)
            .Font = gBtnFont
            .BackColor = Color.LightGray
            .Focus()
        End With
        AddHandler btn_OK.Click, AddressOf OK_Button_Click
        Controls.Add(btn_OK)
    End Sub
    Private Sub Build_YesNo_Button()
        Dim btn_Yes As New Button
        Dim btn_NO As New Button
        With btn_Yes
            .Text = "Yes"
            .Location = New Point(155, 251)
            .Size = New Size(150, 65)
            .Font = gBtnFont
            .BackColor = Color.LightGray
            .Focus()
        End With
        AddHandler btn_Yes.Click, AddressOf Yes_Button_Click
        Controls.Add(btn_Yes)

        With btn_NO
            .Text = "No"
            .Location = New Point(316, 251)
            .Size = New Size(150, 65)
            .Font = gBtnFont
            .BackColor = Color.LightGray
        End With
        AddHandler btn_NO.Click, AddressOf No_Button_Click
        Controls.Add(btn_NO)
    End Sub
End Class
