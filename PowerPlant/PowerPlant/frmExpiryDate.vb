Imports System.Windows.Forms
'-----------------------------------------------------------------------------
'Creation Date: Mar. 20 2012
'Created by:    Bong Lee
'Description:   WO#650 Pop up window for entering finished goods expiry date
'-----------------------------------------------------------------------------
Public Class frmExpiryDate

    Private Sub frmExpiryDate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dteDftExpirydate As DateTime
        If gdteExpiryDate = DateTime.MinValue Then
            dteDftExpirydate = DateAdd(DateInterval.Day, drSO.ProductionShelfLifeDays, Today)
            txtExpiryYear.Text = Year(dteDftExpirydate)
            txtExpiryMonth.Text = Month(dteDftExpirydate)
            txtExpiryDay.Text = ""
        Else
            txtExpiryYear.Text = Year(gdteExpiryDate)
            txtExpiryMonth.Text = Month(gdteExpiryDate)
            txtExpiryDay.Text = DateAndTime.Day(gdteExpiryDate)
        End If
    End Sub

    Private Sub btnConfirm_Click(sender As System.Object, e As System.EventArgs) Handles btnConfirm.Click
        Dim strexpiryDate As String
        Dim dteExpiryDate As DateTime
        Dim edteItem As New clsExpiryDate(drSO.ProductionShelfLifeDays, drSO.ShipShelfLifeDays) 'WO#650

        strexpiryDate = txtExpiryMonth.Text & "/" & txtExpiryDay.Text & "/" & txtExpiryYear.Text
        If txtExpiryYear.Text = "" Then
            MessageBox.Show("Please enter Expiry Year.", "Missing information")
            txtExpiryYear.Focus()
        ElseIf txtExpiryMonth.Text = "" Then
            MessageBox.Show("Please enter Expiry Month.", "Missing information")
            txtExpiryMonth.Focus()
        ElseIf txtExpiryDay.Text = "" Then
            MessageBox.Show("Please enter Expiry Day.", "Missing information")
            txtExpiryDay.Focus()
        ElseIf DateTime.TryParse(strexpiryDate, dteExpiryDate) = False Then
            MessageBox.Show("Please enter an valid date.", "Missing information")
            txtExpiryDay.Focus()
        ElseIf edteItem.IsExpiryDateValid(dteExpiryDate, Today) = False Then
            MessageBox.Show("Expiry date must be between " & edteItem.EarilestExpiryDate.ToString("MM/dd/yyyy") & _
                            " and " & edteItem.LatestExpiryDate.ToString("MM/dd/yyyy"))
            txtExpiryDay.Focus()
        Else
            gdteExpiryDate = dteExpiryDate
            Me.Close()
        End If

    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
      txtExpiryYear.MouseDown, txtExpiryMonth.MouseDown, txtExpiryDay.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)
            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                sender.text = gstrNumPadValue
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtExpiryYear_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtExpiryYear.Validating
        Dim intThisYear As Integer = Year(Today)
        If sender.text <> "" Then
            Try
                If CType(sender.text, Integer) > intThisYear + 3 Or CType(sender.text, Integer) < intThisYear - 3 Then
                    MessageBox.Show("Entered Year can not greater than or less than 3 years of current year.", "Invalid Information")
                    e.Cancel = True
                ElseIf Not SharedFunctions.IsValidDate(txtExpiryYear.Text, txtExpiryMonth.Text, txtExpiryDay.Text) Then
                    e.Cancel = True
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtExpiryMonth_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtExpiryMonth.Validating
        Try
            If sender.text <> "" Then
                If CType(sender.text, Integer) < 1 Or CType(sender.text, Integer) > 12 Then
                    MessageBox.Show("Please enter a valid month between 1 and 12.", "Invalid Information")
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtExpiryDay_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtExpiryDay.Validating
        Try
            If sender.text <> "" Then
                If Not SharedFunctions.IsValidDate(txtExpiryYear.Text, txtExpiryMonth.Text, txtExpiryDay.Text) Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
