Public Class frmCalculator
    Dim Operand1 As Double
    Dim Operand2 As Double
    Dim strOperator As String
    Dim strOperatorPressed As String
    Dim Result As Single

    Private Sub btn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn8.Click, btn9.Click, btn0.Click
        If strOperatorPressed <> "" Then
            txtDisplay.Text = ""
            strOperatorPressed = ""
        End If
        txtDisplay.Text = txtDisplay.Text & sender.text
    End Sub


    Private Sub btndecimal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDecimal.Click
        If strOperatorPressed <> "" Then
            txtDisplay.Text = ""
            strOperatorPressed = ""
        End If

        If InStr(txtDisplay.Text, ".") > 0 Then
            Exit Sub
        Else

            txtDisplay.Text = txtDisplay.Text & "."
        End If
    End Sub

    Private Sub btnEqual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEqual.Click

        If strOperatorPressed = "" Then
            If txtDisplay.Text <> "" Then
                Operand2 = txtDisplay.Text
            Else
                Operand2 = 0
            End If

        End If
        Result = Calculate()
        If Double.IsInfinity(Result) Or Double.IsNaN(Result) Then
            txtDisplay.Text = "Cannot divide by zero."
        Else
            txtDisplay.Text = Result
        End If
        'txtDisplay.Text = Result.ToString("#,###.00")
        Operand1 = Result
        strOperatorPressed = "="

    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        If (strOperatorPressed = "" And Operand1 = 0) Or strOperator = "C" Then
            Operand1 = Val(txtDisplay.Text)
        ElseIf strOperatorPressed <> "+" _
            And strOperatorPressed <> "-" _
            And strOperatorPressed <> "*" _
            And strOperatorPressed <> "/" _
            And strOperatorPressed <> "=" Then
            Operand2 = Val(txtDisplay.Text)
            Result = Calculate()
            'result = Operand1 + Operand2
            txtDisplay.Text = Result
            Operand1 = Result
        End If
        strOperator = "+"
        strOperatorPressed = strOperator
    End Sub

    Private Sub btnMinus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMinus.Click

        If (strOperatorPressed = "" And Operand1 = 0) Or strOperator = "C" Then
            Operand1 = Val(txtDisplay.Text)
        ElseIf strOperatorPressed <> "+" _
            And strOperatorPressed <> "-" _
            And strOperatorPressed <> "*" _
            And strOperatorPressed <> "/" _
            And strOperatorPressed <> "=" Then
            Operand2 = Val(txtDisplay.Text)
            Result = Calculate()
            'result = Operand1 - Operand2
            txtDisplay.Text = Result
            Operand1 = Result
        End If
        strOperator = "-"
        strOperatorPressed = strOperator
    End Sub

    Private Sub btnMultiply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnmultiply.Click

        If (strOperatorPressed = "" And Operand1 = 0) Or strOperator = "C" Then
            Operand1 = Val(txtDisplay.Text)
        ElseIf strOperatorPressed <> "+" _
            And strOperatorPressed <> "-" _
            And strOperatorPressed <> "*" _
            And strOperatorPressed <> "/" _
            And strOperatorPressed <> "=" Then
            Operand2 = Val(txtDisplay.Text)
            Result = Calculate()
            txtDisplay.Text = Result
            'txtDisplay.Text = result.ToString("#,###.00")
            Operand1 = Result
        End If
        'Operand1 = Val(txtDisplay.Text)
        'txtDisplay.Text = ""
        'txtDisplay.Focus()
        strOperator = "*"
        strOperatorPressed = strOperator
    End Sub

    Private Sub btnDivide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivide.Click

        If (strOperatorPressed = "" And Operand1 = 0) Or strOperator = "C" Then
            Operand1 = Val(txtDisplay.Text)
        ElseIf strOperatorPressed <> "+" _
            And strOperatorPressed <> "-" _
            And strOperatorPressed <> "*" _
            And strOperatorPressed <> "/" _
            And strOperatorPressed <> "=" Then
            Operand2 = Val(txtDisplay.Text)
            Result = Calculate()
            'txtDisplay.Text = result.ToString("#,###.00")
            txtDisplay.Text = Result
            Operand1 = Result
        Else

        End If
        'Operand1 = Val(txtDisplay.Text)
        'txtDisplay.Text = ""
        'txtDisplay.Focus()
        strOperator = "/"
        strOperatorPressed = strOperator
    End Sub

    Private Sub btnAddMinus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMinus.Click
        If IsNumeric(txtDisplay.Text) Then
            txtDisplay.Text = -1 * txtDisplay.Text
            If strOperator = "C" Or strOperatorPressed = "=" Then
                Operand1 = txtDisplay.Text
            Else
                Operand2 = txtDisplay.Text
            End If
        End If
    End Sub

    Private Sub btnReciprocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReciprocal.Click
        Dim convert As Single
        If IsNumeric(txtDisplay.Text) Then
            If txtDisplay.Text <> 0 Then
                convert = 1 / Val(txtDisplay.Text)
                txtDisplay.Text = convert
            Else
                txtDisplay.Text = "Cannot divide by zero."
            End If
        End If
        strOperatorPressed = "R"

    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtDisplay.Text = ""
        Operand1 = 0
        Operand2 = 0
        strOperatorPressed = ""
        strOperator = "C"
    End Sub

    Private Sub btnClearEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearEntry.Click
        txtDisplay.Text = ""

    End Sub

    Private Sub btnPercentage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPercentage.Click
        If IsNumeric(txtDisplay.Text) Then
            Operand2 = CType(txtDisplay.Text, Single)
            txtDisplay.Text = (Operand1 * Operand2 / 100).ToString("#,###.00")
        End If
        strOperatorPressed = "%"
        strOperator = ""
    End Sub

    Private Sub btnBackSpace_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackSpace.Click
        If txtDisplay.Text <> "" Then
            txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1)
        End If
    End Sub

    Private Sub frmCalculator_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call btnequal_Click(sender, e)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Function Calculate() As Single
        Dim Result As Single
        Select Case strOperatorPressed
            Case "R", "%"
                Result = Val(txtDisplay.Text)
            Case Is <> "="
                If ActiveControl.Tag = "=" And strOperator = "" Then
                    Operand2 = Val(txtDisplay.Text)
                End If
        End Select

        Select Case strOperator
            Case "+"
                Result = Operand1 + Operand2
            Case "-"
                Result = Operand1 - Operand2
            Case "/"
                Result = Operand1 / Operand2
            Case "*"
                Result = Operand1 * Operand2
        End Select
        Return (Result)

    End Function
End Class