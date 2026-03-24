Public Class frmNumKeyPad
    Dim strValue As String

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn0.Click, _
        btnDecimal.Click, btnMinus.Click, btnDel.Click, btnCancel.Click, _
        btnAccept.Click, btnClear.Click, btn9.Click, btn8.Click
        Try
            Select Case sender.tag
                Case "0" To "9"
                    strValue = strValue & sender.tag
                Case "."
                    If InStr(strValue, ".") = 0 Then
                        strValue = strValue & sender.tag
                    Else
                        MessageBox.Show("Entered value is not numeric.", "Invalid Information")
                    End If
                Case "-"
                    If Microsoft.VisualBasic.Left(strValue, 1) = "-" Then
                        strValue = Microsoft.VisualBasic.Right(strValue, Microsoft.VisualBasic.Len(strValue) - 1)
                    Else
                        strValue = "-" & strValue
                    End If
                Case "B"    'Back Space
                    If Microsoft.VisualBasic.Len(strValue) > 0 Then
                        strValue = Microsoft.VisualBasic.Left(strValue, Microsoft.VisualBasic.Len(strValue) - 1)
                    End If
                Case "D"    'Clear
                    strValue = ""
                Case "C"    'Cancel
                    Me.Close()
                Case "A"    'Accept
                    If IsNumeric(strValue) Or strValue = "" Then
                        gstrNumPadValue = strValue
                    Else
                        MessageBox.Show("Entered value is not numeric.", "Invalid Information")
                    End If
            End Select
            txtDisplay.Text = strValue
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmNumKeyPad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If txtDisplay.Text <> "" AndAlso CType(txtDisplay.Text, Single) <> 0 Then
                strValue = txtDisplay.Text
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class