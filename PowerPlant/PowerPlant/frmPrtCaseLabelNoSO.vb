Public Class frmPrtCaseLabelNoSO
    Dim gstrErrMsg As String
    Dim gintProductionShelfLifeDays As Integer
    Private Sub frmPrtCaseLabelNoSO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dteToday As DateTime
        Try
            Me.UcHeading1.ScreenTitle = "Print Case Labels - By Item"
            lblItemDesc.Visible = False
            SharedFunctions.ClearInputFields(Me)

            If SharedFunctions.IsSvrConnOK(gblnTrySessCtl) = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If
            'dteToday = SharedFunctions.GetProductionDateByShift(gdrSessCtl("OverrideShiftNo"), Now())
            dteToday = Now()
            txtProdYear.Text = Year(dteToday)
            txtProdMonth.Text = Month(dteToday)
            txtProdDay.Text = DateAndTime.Day(dteToday)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
      txtNoOfLabels.MouseDown, txtProdYear.MouseDown, txtProdMonth.MouseDown, txtProdDay.MouseDown
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
    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtItemNo.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            'If gdrCmpCfg("PalletStation") = False Then
            dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                sender.text = gstrNumPadValue
            End If
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub txtItemNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemNo.TextChanged
        Try
            Dim taItemMaster As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
            Dim tblIM As New dsItemMaster.CPPsp_ItemMasterIODataTable
            taItemMaster.Connection.ConnectionString = gstrServerConnectionString
            'Dim taIM As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
            lblItemDesc.Text = ""
            gstrErrMsg = Nothing
            If txtItemNo.Text <> "" Then
                taItemMaster.Fill(tblIM, gdrCmpCfg("Facility"), txtItemNo.Text, "AllByItemNo")
                If tblIM.Rows.Count > 0 Then
                    With tblIM.Rows(0)
                        lblItemDesc.Text = RTrim(.Item("ItemDesc1")) & " " & RTrim(.Item("ItemDesc2")) & " " & RTrim(.Item("PackSize"))
                        lblItemDesc.Visible = True
                        'FX20200708 If RTrim(.Item("CaseLabelFmt1")) = "" AndAlso RTrim(.Item("CaseLabelFmt2")) = "" AndAlso RTrim(.Item("CaseLabelFmt3")) = "" Then
                        'FX20200708 ADD Start
                        If .Item("PrintCaseLabel") = "N" Then
                            gstrErrMsg = "The item has been setup to not produce case label. No label will be printed."
                            MessageBox.Show(gstrErrMsg, "For Your Information.")
                            txtItemNo.Focus()
                        ElseIf RTrim(.Item("CaseLabelFmt1")) = "" AndAlso RTrim(.Item("CaseLabelFmt2")) = "" AndAlso RTrim(.Item("CaseLabelFmt3")) = "" Then
                            'FX20200708 ADD Stop
                            gstrErrMsg = "Case Label has not been setup for the SKU, please contact Supervisor."
                            MessageBox.Show(gstrErrMsg, "Missing Information.")
                            txtItemNo.Focus()
                        ElseIf .Item("DateToPrintFlag") <> "0" AndAlso RTrim(.Item("LabelDateFmtCode")) = "" Then
                            gstrErrMsg = "Label Date Format Code has not been setup for the SKU, please contact Supervisor."
                            MessageBox.Show(gstrErrMsg, "Missing Information.")
                            txtItemNo.Focus()
                        Else
                            gintProductionShelfLifeDays = .Item("ProductionShelfLifeDays")
                        End If
                    End With
                Else
                    gstrErrMsg = "Please Enter a valid Item No."
                    MessageBox.Show(gstrErrMsg, "Invalid Information")
                    txtItemNo.Focus()
                End If

            End If
        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub txtItemNo_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemNo.Validated
    '    lblItemDesc.Visible = True
    'End Sub

    Private Sub txtItemNo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtItemNo.Validating
        Try
            'Dim tblIM As New dsItemMaster.CPPsp_ItemMasterIODataTable
            'Dim taIM As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
            'Try

            If ActiveControl.Name = "btnPrvScn" Then
                Me.Close()
            Else
                If Not IsNothing(gstrErrMsg) Then
                    MessageBox.Show(gstrErrMsg, "Invalid Information")
                    e.Cancel = True
                End If
            End If
            'Catch ex As Exception
            '    MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'End Try
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnPrtCaseLabels_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrtCaseLabels.Click

        Dim strProductionDate As String
        Dim strJobName As String
        Dim dteInputDate As DateTime
        Try
            strProductionDate = txtProdMonth.Text & "/" & txtProdDay.Text & "/" & txtProdYear.Text
            ' C350 printer does not support "/" in the job key. So we change to use "_" instead.
            strJobName = gdrSessCtl("DefaultPkgLine") & txtItemNo.Text & " " & txtProdMonth.Text & "_" & txtProdDay.Text & "_" & txtProdYear.Text
            If gblnSvrConnIsUp = False Then
                MessageBox.Show(gcstSvrCnnFailure, "Warning")
                Me.txtItemNo.Focus()
            ElseIf Me.txtItemNo.Text = "" Then
                MessageBox.Show("Please enter an Item Number.", "Missing information")
                Me.txtItemNo.Focus()
            ElseIf Me.txtNoOfLabels.Text = "" Then
                MessageBox.Show("Please enter Number Of Labels to be printed.", "Missing information")
                Me.txtNoOfLabels.Focus()
            ElseIf Me.txtProdYear.Text = "" Then
                MessageBox.Show("Please enter Production Year.", "Missing information")
                txtProdYear.Focus()
            ElseIf txtProdMonth.Text = "" Then
                MessageBox.Show("Please enter Production Month.", "Missing information")
                txtProdMonth.Focus()
            ElseIf txtProdDay.Text = "" Then
                MessageBox.Show("Please enter Production Day.", "Missing information")
                txtProdDay.Focus()
            ElseIf DateTime.TryParse(strProductionDate, dteInputDate) = False Then
                MessageBox.Show("Please enter a valid Production Date.", "Invalid information")
                'Not IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
                txtProdDay.Focus()
            Else

                If DateDiff(DateInterval.Day, dteInputDate, Now()) > gintProductionShelfLifeDays Then
                    'WO#359 Dim dr As DialogResult = MessageBox.Show("Entered date is greater than the Production Shelf Life Days of the item. Do you want to continue?", "Production Date Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    Dim dr As DialogResult = MessageBox.Show("Entered Production Date will cause the item to be exprired. Do you want to continue?", "Production Date Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)   'WO#359
                    If dr = Windows.Forms.DialogResult.No Then
                        txtProdYear.Focus()
                        Exit Sub
                    End If
                End If

                ' The Shop Order start time parameter use Now since it is insignificant

                'In Print Case Label With No SO, the overrode package line value should be the value of the default packaging line since the overrode packaging should not be applied.
                SharedFunctions.ClearLabelData(gdrSessCtl("DefaultPkgLine"), Now(), gdrSessCtl("Facility"))

                'WO#512 SharedFunctions.CreateAndPrintLabel(ITEMLABEL, gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"), gdrSessCtl.DefaultPkgLine, 0, txtItemNo.Text, 0, gdrSessCtl.Operator, _
                'WO#512        Now, CASELABELER, gdrSessCtl("DefaultPkgLine") & txtItemNo.Text & " " & strProductionDate, gdrCmpCfg("PalletStation"), "", strProductionDate, , CType(Me.txtNoOfLabels.Text, Integer), gdrSessCtl("OverrideShiftNo"))
                SharedFunctions.CreateAndPrintLabel(ITEMLABEL, gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"), gdrSessCtl.DefaultPkgLine, 0, txtItemNo.Text, 0, gdrSessCtl.Operator,
                                Now, CASELABELER, strJobName, gdrCmpCfg("PalletStation"), "", gdrSessCtl.Operator, False, strProductionDate, , CType(Me.txtNoOfLabels.Text, Integer), gdrSessCtl("OverrideShiftNo")) 'WO#512

                Me.Close()
            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtNoOfLabels_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtNoOfLabels.Validating
        Try
            If sender.text <> "" Then
                If IsNumeric(sender.text) Then
                    Try
                        If CType(sender.text, Integer) < 0 Then
                            MessageBox.Show("Please Enter a positive number.", "Invalid Information")
                            e.Cancel = True
                        End If
                    Catch ex As Exception
                        MessageBox.Show("Please Enter an valid number.", "Invalid Information")
                        e.Cancel = True
                    End Try
                Else
                    MessageBox.Show("Please Enter an valid number.", "Invalid Information")
                    e.Cancel = True
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txtProdYear_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtProdYear.Validating
        Dim intThisYear As Integer = Year(Today)
        If sender.text <> "" Then
            Try
                If CType(sender.text, Integer) > intThisYear + 3 Or CType(sender.text, Integer) < intThisYear - 3 Then
                    MessageBox.Show("Entered Year can not greater than or less than 3 years of current year.", "Invalid Information")
                    e.Cancel = True
                ElseIf Not IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
                    e.Cancel = True
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtProdMonth_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtProdMonth.Validating
        Try
            If sender.text <> "" Then
                If CType(sender.text, Integer) < 1 Or CType(sender.text, Integer) > 12 Then
                    MessageBox.Show("Please enter a valid month between 1 and 12.", "Invalid Information")
                    e.Cancel = True
                    '
                    'e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtProdDay_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtProdDay.Validating
        Try
            If sender.text <> "" Then
                If Not IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Function IsValidDate(ByVal strYear As String, ByVal strMonth As String, ByVal strDay As String) As Boolean
        Try
            IsValidDate = True
            If strYear <> "" AndAlso strMonth <> "" AndAlso strDay <> "" Then
                If Not IsDate(strYear & "/" & strMonth & "/" & strDay) Then
                    MessageBox.Show("Please enter a valid day.", "Invalid Information")
                    IsValidDate = False

                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
End Class