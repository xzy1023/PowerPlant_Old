Public Class frmCreatePallet_PS
    Dim gintQtyPerPallet As Integer
    Dim intPalletFull As Int16
    Dim intLastPallet As Int16
    Dim gstrLotID As String
    Dim gstrErrMsg As String
    Dim gblnReOpenShopOrder As Boolean = False 'WO#297

    Private Sub frmCreatePallet_PS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim dteToday As DateTime
            UcHeading1.ScreenTitle = "Create Pallet (Pallet Station)"
            'Me.txtCasesInPallet.Text = ""
            lblQtyPerPallet.Text = ""
            SharedFunctions.clearInputFields(Me)
            intPalletFull = 2
            intLastPallet = 2
            btnPalletFull.BackColor = Color.LemonChiffon
            btnPalletNotFull.BackColor = Color.LemonChiffon
            btnNotLastPallet.BackColor = Color.LemonChiffon
            btnLastPallet.BackColor = Color.LemonChiffon
            lblItemNo.Visible = False
            lblItemDesc.Visible = False
            lblCasesInPallet.Visible = False
            Me.txtCasesInPallet.Visible = False
            lblPkgLine.Visible = False
            lblPkgLine.Text = ""
            lblPalletMsg.Visible = False
            If SharedFunctions.IsSvrConnOK(gblnTrySessCtl) = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If
            dteToday = SharedFunctions.GetProductionDateByShift(gdrSessCtl("OverrideShiftNo"), Now)
            txtProdYear.Text = Year(dteToday)
            txtProdMonth.Text = Month(dteToday)
            txtProdDay.Text = DateAndTime.Day(dteToday)

            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnPalletFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPalletFull.Click
        btnPalletFull.BackColor = Color.LightSeaGreen
        btnPalletNotFull.BackColor = Color.LemonChiffon
        intPalletFull = True
        lblCasesInPallet.Visible = False
        Me.txtCasesInPallet.Visible = False
    End Sub

    Private Sub btnPalletNotFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPalletNotFull.Click
        btnPalletFull.BackColor = Color.LemonChiffon
        btnPalletNotFull.BackColor = Color.LightSeaGreen
        intPalletFull = False
        lblCasesInPallet.Visible = True
        Me.txtCasesInPallet.Visible = True
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub btnLastPallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLastPallet.Click
        btnNotLastPallet.BackColor = Color.LemonChiffon
        btnLastPallet.BackColor = Color.LightSeaGreen
        intLastPallet = True
    End Sub

    Private Sub btnNotLastPallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNotLastPallet.Click
        btnLastPallet.BackColor = Color.LemonChiffon
        btnNotLastPallet.BackColor = Color.LightSeaGreen
        intLastPallet = False
    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtCasesInPallet.MouseDown, _
                txtShopOrder.MouseDown, txtProdYear.MouseDown, txtProdMonth.MouseDown, txtProdDay.MouseDown
        Try
            Dim dgrKeyPad As DialogResult
            dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)
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
    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
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

    Private Sub btnCrtPrtPallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrtPrtPallet.Click
        Dim strOrderComplete As String
        Dim intQtyPerPallet As Integer
        Dim intLooseCases As Integer
        Dim dteProductionDate As DateTime
        Dim intPalletID As Integer
        Dim strErr As String = ""   'WO#650

        Try
            strErr = SharedFunctions.IsValidProductionDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text, , , lblItemNo.Text)    'WO#650
            If txtShopOrder.Text = "" Then
                MessageBox.Show("Please enter a Shop Order.", "Missing Information")
                txtShopOrder.Focus()
            ElseIf Me.txtPkgLine.Text = "" Then
                MessageBox.Show("Please enter a Packaging Line.", "Missing Information")
                txtShopOrder.Focus()
            ElseIf SharedFunctions.IsActiveProbatEnableLine(gdrSessCtl.Facility, txtPkgLine.Text, True) = True Then
                MessageBox.Show("Please enter a non Probat DMS enabled packaging line.", "Invalid Information")
                txtShopOrder.Focus()
            ElseIf Me.txtProdYear.Text = "" Then
                MessageBox.Show("Please enter Production Year.", "Missing information")
                txtProdYear.Focus()
            ElseIf txtProdMonth.Text = "" Then
                MessageBox.Show("Please enter Production Month.", "Missing information")
                txtProdMonth.Focus()
            ElseIf txtProdDay.Text = "" Then
                MessageBox.Show("Please enter Production Day.", "Missing information")
                txtProdDay.Focus()
            ElseIf Not IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
                MessageBox.Show("Invalid Production Date", "Invalid Information")   'WO#650
                txtProdDay.Focus()
            ElseIf strErr <> "" Then    'WO#650
                MessageBox.Show(strErr, "Invalid Information")  'WO#650
                txtProdDay.Focus()  'WO#650
            ElseIf intPalletFull = False And txtCasesInPallet.Text = "" Then
                MessageBox.Show("Case in pallet is required.", "Missing Information")
                txtCasesInPallet.Focus()
            ElseIf intLastPallet = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' for the question, 'Last Pallet for this order?'.", "Missing information")
                Me.btnLastPallet.Focus()
            ElseIf intPalletFull = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' for the question, 'Is this a full pallet full?'.", "Missing information")
                Me.btnPalletFull.Focus()
            Else
                Cursor = Cursors.WaitCursor
                If intLastPallet = True Then
                    strOrderComplete = "Y"
                Else
                    strOrderComplete = "N"
                End If

                If intPalletFull = True Then
                    intQtyPerPallet = gintQtyPerPallet
                Else
                    intQtyPerPallet = CType(Me.txtCasesInPallet.Text, Integer)
                End If

                'Create Pallet
                If txtCasesInPallet.Text = "" Then
                    intLooseCases = 0
                Else
                    intLooseCases = CType(Me.txtCasesInPallet.Text, Integer)
                End If
                dteProductionDate = CDate(txtProdYear.Text & "/" & txtProdMonth.Text & "/" & txtProdDay.Text)
                With gdrSessCtl
                    Dim strPkgLine As String
                    If Len(txtPkgLine.Text) = 10 Then
                        strPkgLine = txtPkgLine.Text
                    Else
                        strPkgLine = Trim(txtPkgLine.Text) & Space(10 - Len(txtPkgLine.Text))
                    End If
                    'WO#2563 DEL Start
                    'intPalletID = SharedFunctions.ProcessFrmCreatePallet(.Facility, CType(txtShopOrder.Text, Integer), lblItemNo.Text, strPkgLine, _
                    '                                        .Operator, intQtyPerPallet, Now, strOrderComplete, _
                    '                                         'gintQtyPerPallet, gstrLotID, dteProductionDate, .OverrideShiftNo, gdrCmpCfg.PalletStation)
                    'WO#2563 DEL Stop
                    'ALM#11828 DEL Start
                    'intPalletID = SharedFunctions.ProcessFrmCreatePallet(.Facility, CType(txtShopOrder.Text, Integer), lblItemNo.Text, strPkgLine, _
                    '                                       .Operator, intQtyPerPallet, Now, strOrderComplete, _
                    '                                        gintQtyPerPallet, gstrLotID, dteProductionDate, _
                    '                                        .OverrideShiftNo, Nothing, gdrCmpCfg.PalletStation)
                    'ALM#11828 DEL Stop
                    'ALM#11828 ADD Start
                    intPalletID = SharedFunctions.ProcessFrmCreatePallet(.Facility, CType(txtShopOrder.Text, Integer), lblItemNo.Text, strPkgLine,
                                                           .Operator, intQtyPerPallet, Now, strOrderComplete,
                                                            gintQtyPerPallet, gstrLotID, dteProductionDate,
                                                            .OverrideShiftNo, Nothing, gdrCmpCfg.PalletStation, 0,
                                                            0, "Pallet Station IPC")      'WO#5370
                    'WO#5370 .OverrideShiftNo, Nothing, gdrCmpCfg.PalletStation, 0)
                    ''ALM#11828 ADD Stop
                End With
                'If it is the Last Pallet For This Shop Order, then display the Stop Shop Order screen.
                If intLastPallet = True Then
                    'frmStopShopOrder.ShowDialog()

                    'clear input fields
                    SharedFunctions.ClearInputFields(Me)
                    Me.lblItemNo.Visible = False
                    Me.lblQtyPerPallet.Visible = False
                    Me.lblItemDesc.Visible = False
                Else    'WO#297
                    If gblnReOpenShopOrder = True Then  'WO#297
                        SharedFunctions.ChangeSOStatus(CType(txtShopOrder.Text, Integer), SharedFunctions.ShopOrderStatus.Open)    'WO#297
                        gblnReOpenShopOrder = False 'WO#297
                    End If  'WO#297
                End If
                'Display message for pallet id
                lblPalletMsg.Text = "Pallet has been created with ID: " & CType(intPalletID, String)
                lblPalletMsg.Visible = True

                intPalletFull = 2
                intLastPallet = 2
                btnPalletFull.BackColor = Color.LemonChiffon
                btnPalletNotFull.BackColor = Color.LemonChiffon
                btnNotLastPallet.BackColor = Color.LemonChiffon
                btnLastPallet.BackColor = Color.LemonChiffon
                txtCasesInPallet.Text = ""
                txtCasesInPallet.Visible = False
                lblCasesInPallet.Visible = False

            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub txtShopOrder_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtShopOrder.TextChanged
        Try
            Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
            Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
            Dim intRtnCde As Integer
            Dim drResponse As DialogResult  'WO#297
            gstrErrMsg = Nothing

            If txtShopOrder.Text <> "" Then
                'Retrieve Shop Order record using the Shop Order from the Session Control
                intRtnCde = taSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), txtShopOrder.Text, "")

                If tblSO.Rows.Count > 0 Then
                    With tblSO.Rows(0)
                        Me.lblItemNo.Text = .Item("ItemNumber")
                        Me.lblItemDesc.Text = .Item("ItemDesc1")
                        lblItemNo.Visible = True
                        lblItemDesc.Visible = True
                        Me.txtPkgLine.Text = .Item("PackagingLine")
                        'Me.lblCaseScheduled.Text = .Item("CasesScheduled")
                        gstrLotID = .Item("LotID")
                        gintQtyPerPallet = CType(.Item("QtyPerPallet"), Integer)
                        lblQtyPerPallet.Text = gintQtyPerPallet
                        Me.txtCasesInPallet.Text = gintQtyPerPallet
                        If Not IsDBNull(.Item("Closed")) Then
                            drResponse = MessageBox.Show("The shop order is closed. Do you want to reopen it?", "Warning!", MessageBoxButtons.YesNo) 'WO#297
                            If drResponse = Windows.Forms.DialogResult.Yes Then     'WO#297
                                'Update the shop order table to re-open the SO
                                gblnReOpenShopOrder = True  'WO#297
                            Else    'WO#297
                                gstrErrMsg = "The shop order is closed, please enter another one."
                                'WO#297 MessageBox.Show(gstrErrMsg, "Invalid Information")
                                gblnReOpenShopOrder = False 'WO#297
                                txtShopOrder.Focus()
                            End If  'WO#297
                            'WO#359 ADD Start
                        ElseIf .Item("QtyPerPallet") = 0 Then
                            gstrErrMsg = "Quantity Per Pallet of the item is missing, please contact Supervisor."
                            MessageBox.Show(gstrErrMsg, "Missing Information.")
                            txtShopOrder.Focus()
                            'WO#359 ADD Stop
                        Else
                            'WO#650 ADD Start
                            gstrErrMsg = SharedFunctions.IsItemChangedOnServer(tblSO.Rows(0))
                            If Not IsNothing(gstrErrMsg) Then
                                MessageBox.Show(gstrErrMsg, "Data Integrity Error")
                                txtShopOrder.Focus()
                                'WO#650 ADD Stop
                            End If
                        End If
                    End With
                Else
                    lblItemNo.Visible = False
                    lblItemDesc.Visible = False
                    Me.txtPkgLine.Text = ""
                    lblPkgLine.Visible = False
                    gstrErrMsg = "The shop order is not found, please enter another Shop Order No."
                    MessageBox.Show(gstrErrMsg, "Invalid Information")
                    txtShopOrder.Focus()
                End If
            End If
            'WO#871 ADD Start
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            'WO#871 ADD Stop
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtShopOrder_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtShopOrder.Validating
        If Me.ActiveControl.Name = "btnPrvScn" Then
            Me.Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid Information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        Try
            Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable
            Dim taEQ As New dsEquipmentTableAdapters.CPPsp_EquipmentIOTableAdapter

            Dim intRtnCde As Integer

            'Retrieve equipment record using the packaging line        
            If txtPkgLine.Text <> "" Then
                'WO#871 intRtnCde = taEQ.Fill(tblEQ, gdrCmpCfg("Facility"), sender.text, "P", "")
                intRtnCde = taEQ.Fill(tblEQ, gdrCmpCfg("Facility"), sender.text, "P", "", "ProbatDisabled")     'WO#871
                If tblEQ.Rows.Count = 0 Then
                    lblPkgLine.Text = ""
                    lblPkgLine.Visible = False
                    gstrErrMsg = "Please enter a valid non Probat DMS enabled Packaging Line."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtPkgLine.Focus()
                Else
                    lblPkgLine.Visible = True
                    lblPkgLine.Text = tblEQ.Rows(0)("Description")
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtPkgLine_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPkgLine.Validating
        If Me.ActiveControl.Name = "btnPrvScn" Then
            Me.Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid Information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub txtCasesInPallet_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCasesInPallet.Validating
        Try
            If ActiveControl.Name = "btnPalletFull" Then
                txtCasesInPallet.Text = ""
                txtCasesInPallet.Visible = False
                lblCasesInPallet.Visible = False
            Else
                If txtCasesInPallet.Text <> "" Then
                    If CType(txtCasesInPallet.Text, Integer) <= 0 Then
                        MessageBox.Show("Case in pallet must be greater than zero.", "Invalid information")
                        e.Cancel = True
                    ElseIf CType(txtCasesInPallet.Text, Integer) > gintQtyPerPallet Then
                        MessageBox.Show("Case in pallet can not be greater than the pallet quantity.", "Invalid Entry")
                        e.Cancel = True
                    End If
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
                If CType(sender.text, Integer) > intThisYear + 1 Or CType(sender.text, Integer) < intThisYear - 1 Then
                    MessageBox.Show("Entered Year can not greater than next year or less than last year.", "Invalid Information")
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
            Throw New Exception("Error in IsValidDate" & vbCrLf & ex.Message)
        End Try
    End Function
End Class