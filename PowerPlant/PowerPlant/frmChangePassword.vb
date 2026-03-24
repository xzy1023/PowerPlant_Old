Public Class frmChangePassword
    Dim drPlantStaff As dsPlantStaffing.CPPsp_PlantStaffingIORow     ' WO#17432 – AT 12/05/2018
    Dim strUserName As String
    Dim strUserID As String

    ' WO#17432 ADD Start – AT 12/05/2018
    Dim gstrErrMsg As String
    Dim blnResetPassword As Boolean
    Dim strCurrPass As String
    Dim strOldPass As String
    Dim strFacility As String
    Public Property OldPass As String
        Get
            Return strOldPass
        End Get
        Set(value As String)
            strOldPass = value
        End Set
    End Property
    ' WO#17432 ADD Stop – AT 12/05/2018

    Public Property UserName As String
        Get
            Return strUserName
        End Get
        Set(value As String)
            strUserName = value
        End Set
    End Property

    Public Property UserID As String
        Get
            Return strUserID
        End Get
        Set(value As String)
            strUserID = value
        End Set
    End Property

    Private Sub frmChangePassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.DialogResult = Windows.Forms.DialogResult.None
        Me.UcHeading1.lblScreenTitle.Text = "Change Password"
        txtUserID.Text = strUserID
        lblUserName.Text = strUserName
        ' WO#17432 ADD Start – AT 12/05/2018
        txtUserID.Enabled = False
        'txtUserID.BackColor = Color.Red
        txtUserID.ForeColor = Color.White
        txtNewPassword.Text = ""
        txtConfirmPassword.Text = ""
        strCurrPass = strOldPass
        drPlantStaff = SharedFunctions.GetPlantStaff(txtUserID.Text, Nothing, True, "Supervisor")
        If Not IsNothing(drPlantStaff) Then
            strUserName = drPlantStaff.FullName
            strFacility = drPlantStaff.Facility
            blnResetPassword = drPlantStaff.ResetPassword
        End If
        ' WO#17432 ADD Stop – AT 12/05/2018
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Ignore
        Me.Close()
    End Sub

    ' WO#17432 ADD Start – AT 12/05/2018
    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        Dim strNewPass As String = String.Empty
        Dim strConfirmPass As String = String.Empty
        strNewPass = txtNewPassword.Text
        strConfirmPass = txtConfirmPassword.Text
        Try
            gstrErrMsg = Nothing
            'If IsPasswordValid(strNewPass) = False Then
            '    gstrErrMsg = "Please enter valid new password."
            '    MessageBox.Show(gstrErrMsg, "Missing information")
            '    txtNewPassword.Focus()
            '    Exit Sub
            'End If
            'If IsPasswordValid(strConfirmPass) = False Then
            '    gstrErrMsg = "Please enter valid confirm password."
            '    MessageBox.Show(gstrErrMsg, "Missing information")
            '    txtConfirmPassword.Focus()
            '    Exit Sub
            'End If
            If strNewPass = "" Then
                gstrErrMsg = "Please enter New Password."
                MessageBox.Show(gstrErrMsg, "Missing information")
                txtNewPassword.Focus()
                Exit Sub
            End If
            If strConfirmPass = "" Then
                gstrErrMsg = "Please enter Confirm Password."
                MessageBox.Show(gstrErrMsg, "Missing information")
                txtConfirmPassword.Focus()
                Exit Sub
            End If
            If strNewPass <> strConfirmPass Then
                gstrErrMsg = "New Password does not match with Confirm Password."
                MessageBox.Show(gstrErrMsg, "Missing information")
                txtNewPassword.Focus()
                Exit Sub
            End If
            If strNewPass = strCurrPass Then
                gstrErrMsg = "New password cannot be the same as current password."
                MessageBox.Show(gstrErrMsg, "Missing information")
                txtNewPassword.Focus()
                Exit Sub
            End If
            SharedFunctions.SavePlantStaffPassword(strFacility, UserID, Cryptography.Encrypt(strNewPass), False, Now)
            With frmQATOverrideLogOn
                .NewPass = strNewPass
            End With
            Me.DialogResult = Windows.Forms.DialogResult.Yes
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        ' WO#17432 ADD Stop – AT 12/05/2018
    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtNewPassword.MouseDown, _
        txtConfirmPassword.MouseDown

        Dim dgrKeyPad As DialogResult
        Try
            'WO#17432 ADD Start
            'WO#17432 – AT 12/05/2018   If sender.name = "txtPassword" Then
            If sender.name = "txtNewPassword" Or sender.name = "txtConfirmPassword" Then
                dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender, , , "*")
            Else
                dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)

            End If
            'WO#17432 ADD Stop

            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(gstrNumPadValue) AndAlso gstrNumPadValue <> "" Then
                    sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue, sender.maxLength)
                Else
                    sender.text = ""
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' WO#17432 ADD Start – AT 12/05/2018
    Private Function IsPasswordValid(ByVal s As String) As Boolean
        For Each c As Char In s
            If String.IsNullOrEmpty(c) Then
                Return False
            Else
                If Integer.TryParse(c, Nothing) Then
                Else
                    Return False
                End If
            End If
        Next
        Return True
    End Function

    Private Sub txtNewPassword_TextChanged(sender As Object, e As EventArgs) Handles txtNewPassword.TextChanged
        Try
            If sender.text <> String.Empty Then
                gstrErrMsg = Nothing
                If IsPasswordValid(sender.text) = False Then
                    gstrErrMsg = "Please enter valid new password."
                    MessageBox.Show(gstrErrMsg, "Invalid input")
                    txtNewPassword.Focus()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtConfirmPassword_TextChanged(sender As Object, e As EventArgs) Handles txtConfirmPassword.TextChanged
        Try
            If sender.text <> String.Empty Then
                gstrErrMsg = Nothing
                If IsPasswordValid(sender.text) = False Then
                    gstrErrMsg = "Please enter valid confirm password."
                    MessageBox.Show(gstrErrMsg, "Invalid input")
                    txtConfirmPassword.Focus()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ValidatingPassword(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtNewPassword.Validating, txtConfirmPassword.Validating
        If (ActiveControl.Name = "btnPrvScn") Then
            Me.Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub
End Class