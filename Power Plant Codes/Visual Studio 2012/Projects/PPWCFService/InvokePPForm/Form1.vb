Public Class frmInvokePPForm

    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        ' Dim strCmd As String = String.Empty
        Dim client As ServiceReference1.PPIServiceClient = New InvokePPForm.ServiceReference1.PPIServiceClient
        Try
            'strCmd = txtForm.Text & " " & txtInterface.Text
            'client.ShowForm(txtForm.Text, txtInterface.Text)
            client.ShowForm(cboForm.SelectedValue, txtInterface.Text, cboWFType.SelectedValue)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub frmInvokePPForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DsQATDef1.dtQATDef' table. You can move, or remove it, as needed.
        Me.DtQATDefTableAdapter.Fill(Me.DsQATDef1.dtQATDef)

        'TODO: This line of code loads data into the 'DsQATForm.tblQATForm' table. You can move, or remove it, as needed.
        Me.TblQATFormTableAdapter.Fill(Me.DsQATForm.tblQATForm)


    End Sub


End Class
