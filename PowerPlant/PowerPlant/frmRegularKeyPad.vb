Public Class frmRegularKeyPad
    Dim strValue As String
    Dim blnShiftPressed As Boolean = False
    Dim blnCapLockPressed As Boolean = False
    Dim intCursorPos As Integer = 0
    ' WO#17432 ADD Start – AT 9/25/2018
    Dim intCursorColumn As Integer = 0
    ' WO#17432 ADD Stop – AT 9/25/2018
    Dim strPasswordChar As String = String.Empty

    Public Property PasswordChar As String
        Get
            Return strPasswordChar
        End Get
        Set(value As String)
            strPasswordChar = value
        End Set
    End Property

    Private Sub GetCurrentDisplayText()
        ' WO#17432 ADD Start – AT 10/26/2018
        ' Capture scanned input text
        Dim v1 As Integer = 0
        Dim v2 As Integer = 0
        Dim v3 As Integer = 0
        v1 = intCursorColumn
        v2 = strValue.Length
        v3 = txtDisplay.TextLength

        If strValue = "" Then
            strValue = txtDisplay.Text
            intCursorColumn = txtDisplay.TextLength
        Else
            If strValue <> txtDisplay.Text Then
                strValue = txtDisplay.Text
                intCursorColumn = v3 - v2 + v1
            End If
        End If
        ' WO#17432 ADD Stop – AT 10/26/2018
    End Sub

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btn_1_Exclamation.Click, btn_2_Ampersat.Click, btn_3_Pound.Click, btn_4_Dollar.Click, btn_5_Percent.Click, btn_6_Caret.Click, _
        btn_7_Ampersand.Click, btn_8_Asterisk.Click, btn_9_OpenParenthesis.Click, btn_0_CloseParenthesis.Click, btn_Decimal_GreaterThan.Click, _
        btn_Minus_UnderScore.Click, btn_SemiColon_Colon.Click, btn_apostrophe_Quote.Click, btn_Equal_Plus.Click, btn_ForwardSlash_Pipe.Click, _
        btn_BackSlash_QuestionMark.Click, btn_Acute_Tilde.Click, btn_Close_Bracket_Brace.Click, btn_Open_Bracket_Brace.Click, _
        btn_A.Click, btn_B.Click, btn_C.Click, btn_M.Click, btn_Comma_LessThan.Click, btn_N.Click, btn_L.Click, Btn_P.Click, btn_Q.Click, _
        btn_V.Click, btn_W.Click, btn_E.Click, btn_R.Click, btn_T.Click, btn_Y.Click, btn_U.Click, btn_I.Click, btn_O.Click, btn_S.Click, _
        btn_D.Click, btn_F.Click, btn_G.Click, btn_H.Click, btn_J.Click, btn_K.Click, btn_X.Click, btn_Z.Click

        Try
            ' WO#17432 ADD Start – AT 10/26/2018
            GetCurrentDisplayText()
            ' WO#17432 ADD Stop – AT 10/26/2018

            If sender.text = "&&" Then
                sender.text = "&"
            End If

            ' WO#17432 ADD Start – AT 9/25/2018
            intCursorPos = intCursorColumn
            ' WO#17432 ADD Stop – AT 9/25/2018

            If intCursorPos <> txtDisplay.TextLength Then
                strValue = strValue.Insert(intCursorPos, sender.text)
            Else
                strValue = strValue & sender.text
            End If

            If sender.text = "&" Then
                sender.text = "&&"
            End If

            intCursorPos = intCursorPos + 1
            ' WO#17432 ADD Start – AT 9/25/2018
            intCursorColumn = intCursorPos
            ' WO#17432 ADD Stop – AT 9/25/2018
            txtDisplay.Text = strValue
            If blnShiftPressed = True Then
                blnShiftPressed = False
                DownShiftKeys()
            End If

            ' WO#17432 ADD Start – AT 10/26/2018
            txtDisplay.Focus()
            txtDisplay.SelectionStart = intCursorPos
            txtDisplay.Select()
            strValue = txtDisplay.Text
            ' WO#17432 ADD Stop– AT 10/26/2018

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmRegularKeyPad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If txtDisplay.Text <> "" Then
                strValue = txtDisplay.Text
                ' WO#17432 ADD Start – AT 9/25/2018
                intCursorColumn = strValue.Length
                txtDisplay.Focus()
                txtDisplay.SelectionStart = intCursorColumn
                ' WO#17432 ADD Stop – AT 9/25/2018
            Else
                ' WO#17432 ADD Start– AT 10/25/2018
                strValue = ""
                intCursorColumn = 0
                ' WO#17432 ADD Stop – AT 10/25/2018
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btn_Shift_Click(sender As Object, e As EventArgs) Handles btn_Shift.Click
        Try
            ' WO#17432 ADD Start – AT 10/26/2018
            GetCurrentDisplayText()
            ' WO#17432 ADD Stop – AT 10/26/2018

            If blnShiftPressed = False Then
                blnShiftPressed = True
                UpShiftKeys()
            Else
                blnShiftPressed = False
                DownShiftKeys()
            End If

            ' WO#17432 ADD Start – AT 10/26/2018 
            txtDisplay.Focus()
            txtDisplay.SelectionStart = intCursorColumn
            ' WO#17432 ADD Stop – AT 10/26/2018
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btn_CapLock_Click(sender As Object, e As EventArgs) Handles btn_CapLock.Click
        Try
            ' WO#17432 ADD Start – AT 10/26/2018
            GetCurrentDisplayText()
            ' WO#17432 ADD Stop – AT 10/26/2018

            If blnCapLockPressed = False Then
                blnCapLockPressed = True
                UpShiftKeys()
            Else
                blnCapLockPressed = False
                DownShiftKeys()
            End If

            ' WO#17432 ADD Start – AT 10/26/2018 
            txtDisplay.Focus()
            txtDisplay.SelectionStart = intCursorColumn
            ' WO#17432 ADD Stop – AT 10/26/2018
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        ' WO#17432 ADD Start – AT 10/26/2018
        ' gstrNumPadValue = strValue
        gstrNumPadValue = txtDisplay.Text
        ' WO#17432 ADD Stop – AT 10/26/2018
    End Sub

    Private Function GetNewValue(ByVal intPos As Integer, Optional ByVal blnSpace As Boolean = False) As String
        ' WO#17432 ADD Start – AT 9/25/2018
        Dim strNewValue As String = String.Empty
        Dim strNewValue1 As String = String.Empty
        Dim intLength As Integer = 0

        intLength = strValue.Length
        Select Case intPos
            Case 0
                strNewValue = strValue
                strNewValue1 = " " & strValue
            Case 1
                strNewValue = strValue.Substring(intCursorColumn, strValue.Length - 1)
                strNewValue1 = Microsoft.VisualBasic.Left(strValue, intCursorColumn) & " " & Microsoft.VisualBasic.Right(strValue, intLength - intCursorColumn)
            Case Is = strValue.Length
                strNewValue = strValue.Substring(0, strValue.Length - 1)
                strNewValue1 = strValue & " "
            Case Else
                strNewValue = Microsoft.VisualBasic.Left(strValue, intCursorColumn - 1) & Microsoft.VisualBasic.Right(strValue, intLength - intCursorColumn)
                strNewValue1 = Microsoft.VisualBasic.Left(strValue, intCursorColumn) & " " & Microsoft.VisualBasic.Right(strValue, intLength - intCursorColumn)
        End Select
        If blnSpace = True Then
            Return strNewValue1
        Else
            Return strNewValue
        End If
        ' WO#17432 ADD Stop – AT 9/25/2018
    End Function

    Private Sub btnBackSpace_Click(sender As Object, e As EventArgs) Handles btnBackSpace.Click
        Try
            ' WO#17432 ADD Start – AT 10/26/2018
            GetCurrentDisplayText()
            ' WO#17432 ADD Stop – AT 10/26/2018

            If Microsoft.VisualBasic.Len(strValue) > 0 Then
                ' WO#17432 DEL Start – AT 9/25/2018
                'strValue = Microsoft.VisualBasic.Left(strValue, Microsoft.VisualBasic.Len(strValue) - 1)
                'txtDisplay.Text = strValue
                'intCursorPos = strValue.Length
                'If intCursorPos < 0 Then
                '    intCursorPos = 0
                'End If
                ' WO#17432 DEL Start – AT 9/25/2018

                ' WO#17432 ADD Start – AT 9/25/2018
                Dim strNewValue As String = String.Empty
                Dim intNewValue As Integer = 0
                strNewValue = GetNewValue(intCursorColumn)
                intNewValue = intCursorColumn

                If intCursorColumn <= 0 Then
                    intNewValue = 0
                Else
                    If intCursorColumn - 1 = 0 Then
                        intNewValue = 0
                    Else
                        intNewValue = intCursorColumn - 1
                    End If
                End If

                txtDisplay.Text = strNewValue
                strValue = strNewValue
                intCursorPos = strValue.Length
                If intCursorPos < 0 Then
                    intCursorPos = 0
                End If
                intCursorColumn = intNewValue
                txtDisplay.Focus()
                txtDisplay.SelectionStart = intCursorPos
                txtDisplay.SelectionStart = intNewValue
                ' WO#17432 ADD Stop – AT 9/25/2018

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        strValue = ""
        intCursorPos = 0
        txtDisplay.Text = strValue
        ' WO#17432 ADD Start – AT 9/25/2018
        txtDisplay.Focus()
        txtDisplay.SelectionStart = intCursorPos
        intCursorColumn = 0
        ' WO#17432 ADD Stop – AT 9/25/2018
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btn_ArrowLeft_Click(sender As Object, e As EventArgs) Handles btn_ArrowLeft.Click
        ' WO#17432 ADD Start – AT 10/26/2018
        GetCurrentDisplayText()
        ' WO#17432 ADD Stop – AT 10/26/2018

        ' WO#17432 ADD Start – AT 9/25/2018
        intCursorPos = intCursorColumn
        ' WO#17432 ADD Stop – AT 9/25/2018

        If intCursorPos > 0 Then
            intCursorPos = intCursorPos - 1
            ' WO#17432 ADD Start – AT 9/25/2018
            intCursorColumn = intCursorPos
            ' WO#17432 ADD Stop – AT 9/25/2018
        End If

        ' WO#17432 ADD Start – AT 9/25/2018 
        txtDisplay.Focus()
        txtDisplay.SelectionStart = intCursorPos
        ' WO#17432 ADD Stop – AT 9/25/2018
    End Sub

    Private Sub btn_ArrowRight_Click(sender As Object, e As EventArgs) Handles btn_ArrowRight.Click
        ' WO#17432 ADD Start – AT 10/26/2018
        GetCurrentDisplayText()
        ' WO#17432 ADD Stop – AT 10/26/2018

        ' WO#17432 ADD Start – AT 9/25/2018
        intCursorPos = intCursorColumn
        ' WO#17432 ADD Stop – AT 9/25/2018

        If intCursorPos < txtDisplay.TextLength Then
            intCursorPos = intCursorPos + 1
            ' WO#17432 ADD Start – AT 9/25/2018
            intCursorColumn = intCursorPos
            ' WO#17432 ADD Stop – AT 9/25/2018
        End If

        ' WO#17432 ADD Start – AT 9/25/2018
        txtDisplay.Focus()
        txtDisplay.SelectionStart = intCursorPos
        ' WO#17432 ADD Stop– AT 9/25/2018
    End Sub

    Private Sub txtDisplay_Click(sender As Object, e As EventArgs) Handles txtDisplay.Click
        intCursorPos = txtDisplay.TextLength
    End Sub

    Private Sub txtDisplay_MouseClick(sender As Object, e As MouseEventArgs) Handles txtDisplay.MouseClick
        ' WO#17432 ADD Start – AT 9/25/2018
        Dim index = txtDisplay.SelectionStart
        Dim intCursorLine = txtDisplay.GetLineFromCharIndex(index)
        intCursorColumn = index - txtDisplay.GetFirstCharIndexFromLine(intCursorColumn)
        ' WO#17432 ADD Stop– AT 9/25/2018
    End Sub

    Private Sub btn_Space_Click(sender As Object, e As EventArgs) Handles btn_Space.Click
        Try
            ' WO#17432 ADD Start – AT 10/26/2018
            GetCurrentDisplayText()
            ' WO#17432 ADD Stop – AT 10/26/2018

            ' WO#17432 DEL Start – AT 9/25/2018
            'strValue = strValue & " "
            'intCursorPos = intCursorPos + 1
            'txtDisplay.Text = strValue
            ' WO#17432 DEL Start – AT 9/25/2018

            ' WO#17432 ADD Start – AT 9/25/2018
            Dim strNewValue As String = String.Empty
            Dim intNewValue As Integer = 0
            intNewValue = intCursorColumn + 1
            strNewValue = GetNewValue(intCursorColumn, True)
            intCursorColumn = intNewValue
            strValue = strNewValue
            intCursorPos = intCursorColumn
            txtDisplay.Text = strValue

            txtDisplay.Focus()
            txtDisplay.SelectionStart = strValue.Length
            txtDisplay.SelectionStart = intCursorColumn
            ' WO#17432 ADD Stop – AT 9/25/2018

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpShiftKeys()
        Dim ctl As Control
        Try
            For Each ctl In Me.Controls
                If TypeOf ctl Is Button Then
                    If ctl.Tag.ToString.Substring(0, 6) = "letter" Then
                        ctl.Text = UCase(ctl.Text)
                    ElseIf ctl.Tag.ToString.Substring(0, 6) = "number" Then
                        If ctl.Tag.ToString.Substring(9, 1) = "&" Then
                            ctl.Text = "&&"
                        Else
                            ctl.Text = ctl.Tag.ToString.Substring(9, 1)
                        End If
                    ElseIf ctl.Tag.ToString.Substring(0, 4) = "sign" Then
                        ctl.Text = ctl.Tag.ToString.Substring(7, 1)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error in UpShiftKeys" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub DownShiftKeys()
        Dim ctl As Control
        Try
            For Each ctl In Me.Controls
                If TypeOf ctl Is Button Then
                    If ctl.Tag.ToString.Substring(0, 6) = "letter" Then
                        ctl.Text = LCase(ctl.Text)
                    ElseIf ctl.Tag.ToString.Substring(0, 6) = "number" Then
                        ctl.Text = ctl.Tag.ToString.Substring(7, 1)
                    ElseIf ctl.Tag.ToString.Substring(0, 4) = "sign" Then
                        ctl.Text = ctl.Tag.ToString.Substring(5, 1)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error in DownShiftKeys" & vbCrLf & ex.Message)
        End Try
    End Sub
End Class