'WO#3282 ADD Start
Public Class frmComment

    Private Sub frmComment_Load(sender As Object, e As EventArgs) Handles Me.Load
        UcHeading1.ScreenTitle = "Down Time Comment"
        lblShopOrderNo.Text = gintShopOrder
        lblOperator.Text = frmDownTime.lblOperator.Text
        With frmDownTime
            If .cboReasonCode.DisplayMember = "ReasonCode" Then
                lblReasonCodeDesc.Text = .cboReasonCode.Text + " - " + .lblReasonCodeDesc.Text
            Else
                lblReasonCodeDesc.Text = .cboReasonCode.SelectedValue.ToString + " - " + .lblReasonCodeDesc.Text
            End If
        End With
        gstrComment = ""
        RTBComment.Text = ""
        RTBComment.Focus()
        gblnCancelComment = False
    End Sub

    Private Sub btnPrvScn_Click(sender As Object, e As EventArgs) Handles btnPrvScn.Click
        gblnCancelComment = True
        Me.Close()
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If RTBComment.Text = "" Then
            MessageBox.Show("Comment is mandatory.", "Missing information")
            RTBComment.Focus()
        Else
            gstrComment = RTBComment.Text
            Me.Close()
        End If
    End Sub

End Class
'WO#3282 ADD Stop