Public Class frmPrtCaseLabelWithSO
    Dim gintShopOrder As Integer
    Dim lgtaSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
    Dim lgtaIM As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
    Dim lgdrSO As dsShopOrder.CPPsp_ShopOrderIORow
    Dim gintSODftWidth As Integer
    Dim gstrErrMsg As String
    Dim gintProductionShelfLifeDays As Integer
    Dim gfntSOdefaultFont As New Font("Arial", 18, System.Drawing.FontStyle.Regular)
    Dim gstrLotID As String

    Private Sub frmPrtCaseLabeWithSO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dteToday As DateTime
        'WO#871 Dim strMyComputerName As String = My.Computer.Name

        Try
            Me.UcHeading1.ScreenTitle = "Print Case Labels"
            'FX20200708 ADD Start
            gintShopOrder = 0
            cboShopOrder.Text = String.Empty
            lblItemNo.Text = String.Empty
            'FX20200708 ADD Stop
            'WO#650 ADD Start
            txtExpiryYear.Visible = False
            txtExpiryMonth.Visible = False
            txtExpiryDay.Visible = False
            lblExpiryYear.Visible = False
            lblExpiryMonth.Visible = False
            lblExpiryDay.Visible = False
            lblExpiryDate.Visible = False
            'WO#650 ADD Stop
            txtItemNo.Enabled = False
            lblItemDesc.Text = String.Empty

            cboShopOrder.Text = ""
            cboShopOrder.DataSource = Nothing

            SharedFunctions.ClearInputFields(Me)

            'Connect to the server data base instead of local data base
            lgtaSO.Connection.ConnectionString = gstrServerConnectionString
            lgtaIM.Connection.ConnectionString = gstrServerConnectionString

            If SharedFunctions.IsSvrConnOK(gblnTrySessCtl) = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If
            txtPkgLine.Text = gdrSessCtl.OverridePkgLine
            txtNoOfLabels.Text = gdrCmpCfg.NoOfLabels
            'dteToday = SharedFunctions.GetProductionDateByShift(gdrSessCtl("OverrideShiftNo"), Now())
            dteToday = Now()
            txtProdYear.Text = Year(dteToday)
            txtProdMonth.Text = Month(dteToday)
            txtProdDay.Text = DateAndTime.Day(dteToday)


            'If the latest downloaded data is ready in staging area and shop order is not started, import data
            'from the staging area into the local data base (This is specially for Ajax lines that do not
            'start shop order but only use this option to print case labels.
            If gdrSessCtl.ShopOrder = 0 Then
                'WO#871 SharedFunctions.ImportMasterTables(strMyComputerName)
                SharedFunctions.ImportMasterTables(gstrMyComputerName)          'WO#871
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
      txtNoOfLabels.MouseDown, txtProdYear.MouseDown, txtProdMonth.MouseDown, txtProdDay.MouseDown, txtExpiryYear.MouseDown, txtExpiryMonth.MouseDown, txtExpiryDay.MouseDown
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
    'FX20200708 Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown, txtItemNo.MouseDown
    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown            'FX20200708 
        Dim dgrKeyPad As DialogResult
        Try
            If TypeOf sender Is TextBox Or (sender.name = "cboShopOrder" And gblnDropDownIsClicked = False) Then
                dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
                If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                    If Not IsNothing(gstrNumPadValue) AndAlso gstrNumPadValue <> "" Then
                        sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue, sender.maxLength)
                    Else
                        sender.text = ""
                    End If
                End If
            End If
            If TypeOf sender Is ComboBox Then
                gblnDropDownIsClicked = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    'FX20200708 Private Sub txtItemNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemNo.TextChanged
    Private Sub LoadItemInfo(strItemNo As String)       'FX20200708
        Dim tblIM As New dsItemMaster.CPPsp_ItemMasterIODataTable
        Try

            lblItemDesc.Text = ""
            'FX20200708 gstrErrMsg = Nothing
            'FX20200708   If txtItemNo.Text <> "" Then
            'FX20200708 lgtaIM.Fill(tblIM, gdrCmpCfg("Facility"), txtItemNo.Text, "AllByItemNo")
            gstrErrMsg = Nothing                                                     'IC#14515
            'IC#14515 gstrErrMsg = String.Empty                                               'FX20200708
            If strItemNo <> String.Empty Then                                       'FX20200708 
                lgtaIM.Fill(tblIM, gdrCmpCfg("Facility"), strItemNo, "AllByItemNo") 'FX20200708
                If tblIM.Rows.Count > 0 Then
                    With tblIM.Rows(0)
                        lblItemDesc.Text = RTrim(.Item("ItemDesc1")) & " " & RTrim(.Item("ItemDesc2")) & " " & RTrim(.Item("PackSize"))
                        lblItemDesc.Visible = True
                        '--Feb. 6, 2014 -- If RTrim(.Item("CaseLabelFmt1")) = "" Then
                        'FX20200708 If RTrim(.Item("CaseLabelFmt1")) = "" AndAlso RTrim(.Item("CaseLabelFmt2")) = "" AndAlso RTrim(.Item("CaseLabelFmt3")) = "" Then    '--Feb. 6, 2014 --
                        'FX20200708 ADD Start
                        If .Item("PrintCaseLabel") = "N" Then
                            gstrErrMsg = "The item has been setup to not produce case label. No label will be printed."
                            'FX20200708 MessageBox.Show(gstrErrMsg, "Warning.", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        ElseIf RTrim(.Item("CaseLabelFmt1")) = "" AndAlso RTrim(.Item("CaseLabelFmt2")) = "" AndAlso RTrim(.Item("CaseLabelFmt3")) = "" Then    'FX20200708
                            'FX20200708 ADD Stop
                            gstrErrMsg = "Case Label has not been setup for the SKU, please contact Supervisor."
                            'FX20200708 MessageBox.Show(gstrErrMsg, "Missing Information.")
                            'FX20200708 txtItemNo.Focus()
                        ElseIf .Item("DateToPrintFlag") <> "0" AndAlso RTrim(.Item("LabelDateFmtCode")) = "" Then
                            gstrErrMsg = "Label Date Format Code has not been setup for the SKU, please contact Supervisor."
                            'FX20200708 MessageBox.Show(gstrErrMsg, "Missing Information.")
                            'FX20200708 txtItemNo.Focus()
                        Else
                            gintProductionShelfLifeDays = .Item("ProductionShelfLifeDays")
                        End If
                    End With
                Else
                    gstrErrMsg = "Please select a shop order."
                    MessageBox.Show(gstrErrMsg, "Invalid Information")
                    'FX20200708 cboShopOrder.Focus()
                End If
                'FX20200708 ADD Start
                If gstrErrMsg <> String.Empty Then
                    txtNoOfLabels.Focus()       'Focus to another control to force to trigger the Validating event of the control to display the error message.
                End If
                'FX20200708 ADD Stop
            End If
        Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            'FX20200708 MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name 'FX20200708
            Throw New Exception("Error in LoadItemInfo" & vbCrLf & ex.Message) 'FX20200708
        End Try
    End Sub
    'FX20200708 DEL Start
    'Private Sub txtItemNo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtItemNo.Validating
    '    Try

    '        If ActiveControl.Name = "btnPrvScn" Then
    '            Me.Close()
    '        Else
    '            If Not IsNothing(gstrErrMsg) Then
    '                MessageBox.Show(gstrErrMsg, "Invalid Information")
    '                e.Cancel = True
    '            End If
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try

    'End Sub
    'FX20200708 DEL Stop

    Private Sub btnPrtCaseLabels_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrtCaseLabels.Click
        Dim strProductionDate As String
        Dim dteInputDate As DateTime
        Dim strJobName As String
        Dim cnnServer As New SqlClient.SqlConnection(gstrServerConnectionString)
        Dim strExpiryDate As String 'WO#650
        Dim blnReqExpiryDate As Boolean 'WO#650
        Dim edteItem As New clsExpiryDate(lgdrSO.ProductionShelfLifeDays, lgdrSO.ShipShelfLifeDays) 'WO#650
        Dim dteExpiryDate As DateTime 'WO#650

        Try
            strProductionDate = txtProdMonth.Text & "/" & txtProdDay.Text & "/" & txtProdYear.Text

            'WO#650 ADD Start
            If gblnOvrExpDate = True AndAlso (lgdrSO.DateToPrintFlag = "1" Or lgdrSO.DateToPrintFlag = "3") Then
                strExpiryDate = txtExpiryMonth.Text & "/" & txtExpiryDay.Text & "/" & txtExpiryYear.Text
                blnReqExpiryDate = True
            Else
                strExpiryDate = String.Empty
                blnReqExpiryDate = False
            End If
            'WO#650 ADD Stop

            If gblnSvrConnIsUp = False Then
                MessageBox.Show(gcstSvrCnnFailure, "Warning")
                txtPkgLine.Focus()
            ElseIf txtPkgLine.Text = "" Then
                MessageBox.Show("Please enter an Packaging Line ID.", "Missing information")
                txtPkgLine.Focus()
            ElseIf cboShopOrder.Text = "" AndAlso cboShopOrder.SelectedValue Then
                MessageBox.Show("Please enter an Shop Order Number.", "Missing information")
                cboShopOrder.Focus()
                'FX20200708 DEL Start
                'ElseIf Me.txtItemNo.Text = "" Then
                '    MessageBox.Show("Please enter an Item Number.", "Missing information")
                '    Me.txtItemNo.Focus()
                'FX20200708 DEL Stop
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
                'Not IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
                txtProdDay.Focus()
                'WO#650 Add Start
            ElseIf blnReqExpiryDate AndAlso Me.txtExpiryYear.Text = "" Then
                MessageBox.Show("Please enter Expiry Year.", "Missing information")
                txtExpiryYear.Focus()
            ElseIf blnReqExpiryDate AndAlso txtExpiryMonth.Text = "" Then
                MessageBox.Show("Please enter Expiry Month.", "Missing information")
                txtExpiryMonth.Focus()
            ElseIf blnReqExpiryDate AndAlso txtExpiryDay.Text = "" Then
                MessageBox.Show("Please enter Expiry Day.", "Missing information")
                txtExpiryDay.Focus()
            ElseIf blnReqExpiryDate AndAlso DateTime.TryParse(strExpiryDate, dteExpiryDate) = False Then
                txtExpiryDay.Focus()
            ElseIf blnReqExpiryDate AndAlso lgdrSO.ShipShelfLifeDays = 0 Then
                MessageBox.Show("The Ship Shelf Life Days for the item cannot be zero, Please contact Supervisor.", "Missing information")         'WO#650
            ElseIf blnReqExpiryDate AndAlso lgdrSO.ProductionShelfLifeDays = 0 Then
                MessageBox.Show("The Production Shelf Life Days for the item cannot be zero, Please contact Supervisor.", "Missing information")
            ElseIf blnReqExpiryDate AndAlso edteItem.IsExpiryDateValid(dteExpiryDate, dteInputDate) = False Then
                MessageBox.Show("Expiry date must be between " & edteItem.EarilestExpiryDate.ToString("MM/dd/yyyy") &
                                " and " & edteItem.LatestExpiryDate.ToString("MM/dd/yyyy"))
                txtExpiryDay.Focus()
                'WO#650 Add Stop
            Else
                If DateDiff(DateInterval.Day, dteInputDate, Now()) > gintProductionShelfLifeDays Then
                    'WO#359 Dim dr As DialogResult = MessageBox.Show("Entered date is greater than the Production Shelf Life Days of the item. Do you want to continue?", "Production Date Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    Dim dr As DialogResult = MessageBox.Show("Entered Production Date will cause the item to be exprired. Do you want to continue?", "Production Date Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)   'WO#359
                    If dr = Windows.Forms.DialogResult.No Then
                        txtProdYear.Focus()
                        Exit Sub
                    End If
                End If

                gdteExpiryDate = dteExpiryDate   'WO#650

                ' The Shop Order start time parameter use Now since it is insignificant
                ' Use the default packaging line to clear the label data that was created from the default packaging line.
                SharedFunctions.ClearLabelData(gdrSessCtl.DefaultPkgLine, Now(), gdrSessCtl("Facility"))

                Dim trnServer As SqlClient.SqlTransaction = Nothing
                If gblnSvrConnIsUp = True Then
                    Try
                        cnnServer.Open()
                        trnServer = cnnServer.BeginTransaction()
                    Catch ex As SqlClient.SqlException
                        SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    End Try
                End If

                strJobName = String.Concat(txtPkgLine.Text.Trim.PadRight(txtPkgLine.MaxLength), gintShopOrder, " ", gdrSessCtl.DefaultPkgLine.Trim)

                'Create the data for the label as of the overrode line. That is the overrode packaging line ID will be show on the label and the "LabelKey"
                SharedFunctions.CreateLabelData(gdrSessCtl("Facility"), ITEMLABEL, CASELABELER, gdrSessCtl.DefaultPkgLine, txtPkgLine.Text,
                               gintShopOrder, lblItemNo.Text, 0, 0, gdrSessCtl.Operator,
                                 strJobName, gstrLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, IIf(blnReqExpiryDate, gdteExpiryDate.ToString("MM/dd/yyyy"), Nothing), trnServer)    'FX2022404
                'FX2022404  SharedFunctions.CreateLabelData(gdrSessCtl("Facility"), ITEMLABEL, CASELABELER, gdrSessCtl.DefaultPkgLine, txtPkgLine.Text, _
                'FX2022404  gintShopOrder, txtItemNo.Text, 0, 0, gdrSessCtl.Operator, _
                'FX2022404  strJobName, gstrLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, IIf(blnReqExpiryDate, gdteExpiryDate.ToString("MM/dd/yyyy"), Nothing), trnServer)    'WO#650 
                'WO#650               strJobName, gstrLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, trnServer)


                'Create the print request for the line submit the request. That is the default line.
                'WO#512 SharedFunctions.CreatePrintRequest(ITEMLABEL, gdrSessCtl("Facility"), gdrSessCtl.DefaultPkgLine, CASELABELER, _
                'WO#512        Now, strJobName, gdrCmpCfg("PalletStation"), CType(txtNoOfLabels.Text, Integer), trnServer)
                SharedFunctions.CreatePrintRequest(ITEMLABEL, gdrSessCtl("Facility"), gdrSessCtl.DefaultPkgLine, CASELABELER,
                               Now, strJobName, gdrCmpCfg("PalletStation"), gdrSessCtl.Operator, CType(txtNoOfLabels.Text, Integer), trnServer)      'WO#512

                If Not trnServer Is Nothing Then
                    Try
                        trnServer.Commit()
                    Catch ex As Exception
                        If Not IsNothing(trnServer) Then
                            Try
                                trnServer.Rollback()
                            Catch exRollback As Exception
                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            End Try
                        End If
                    End Try
                End If
                Me.Close()
            End If

        Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
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
                ElseIf Not SharedFunctions.IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
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
                If Not SharedFunctions.IsValidDate(txtProdYear.Text, txtProdMonth.Text, txtProdDay.Text) Then
                    e.Cancel = True
                End If
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
    'WO#650 DEL Start
    'Function IsValidDate(ByVal strYear As String, ByVal strMonth As String, ByVal strDay As String) As Boolean
    '    Try
    '        IsValidDate = True
    '        If strYear <> "" AndAlso strMonth <> "" AndAlso strDay <> "" Then
    '            If Not IsDate(strYear & "/" & strMonth & "/" & strDay) Then
    '                MessageBox.Show("Please enter a valid day.", "Invalid Information")
    '                IsValidDate = False

    '            End If
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Function
    'WO#650 DEL Start

    Private Sub cboShopOrder_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboShopOrder.DropDown
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Try
            With cboShopOrder
                'WO#650 gtaSO.Fill(tblSO, "GetSOList", gdrSessCtl("Facility"), 0, gdrSessCtl("DefaultPkgLine"))
                lgtaSO.Fill(tblSO, "GetSOList", gdrSessCtl("Facility"), 0, txtPkgLine.Text)     'WO#650
                .DataSource = tblSO
                .DisplayMember = "SODescription"
                .ValueMember = "ShopOrder"
                .BackColor = Color.DarkGreen
                If gblnAjaxPlant Then
                    'WO#@@@@    .Width = .DropDownWidth + 220
                    .Width = .DropDownWidth + 200   'WO#@@@@
                    'WO#2563   .Font = New Font("Arial", 20, System.Drawing.FontStyle.Regular)
                Else
                    .Width = .DropDownWidth
                    'WO#2563    .Font = New Font("Arial", 30, System.Drawing.FontStyle.Regular)
                End If
                gblnDropDownIsClicked = True
            End With
        Catch ex As Exception
            Throw New Exception("Error in cboShopOrder_DropDown" & Environment.NewLine & ex.Message)
        End Try
    End Sub

    Private Sub cboShopOrder_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboShopOrder.DropDownClosed
        With cboShopOrder
            .Font = gfntSOdefaultFont
            If Not gblnAjaxPlant Then
                'WO#@@@@    .Width = 250
                'WO#2563    .Width = 265    'WO#@@@@
                .Width = 425    'WO#2563
            End If
            .BackColor = Color.Black
        End With
    End Sub

    Private Sub cboShopOrder_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboShopOrder.TextChanged
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim strBasisCode As String = String.Empty

        Try
            gstrErrMsg = Nothing
            If sender.text <> "" _
                AndAlso Not TypeOf (sender.SelectedValue) Is DataRowView Then
                If Not IsNumeric(sender.text) And Not IsNumeric(sender.SelectedValue) Then
                    gstrErrMsg = "Shop order is not numeric. please enter another shop order number."
                    MessageBox.Show(gstrErrMsg, "Invalid Entry")
                    cboShopOrder.Focus()
                Else
                    If IsNumeric(cboShopOrder.Text) Then
                        gintShopOrder = cboShopOrder.Text
                    Else
                        gintShopOrder = cboShopOrder.SelectedValue
                    End If
                    gstrCurrentShopOrder = CType(gintShopOrder, String)
                    cboShopOrder.Font = gfntSOdefaultFont
                    lgtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gintShopOrder, "")
                    With tblSO
                        If .Rows.Count > 0 Then
                            lgdrSO = .Rows(0)
                            'FX20200708 txtItemNo.Text = lgdrSO("itemnumber")
                            lblItemNo.Text = lgdrSO("itemnumber")                 'FX20200708
                            lblItemDesc.Text = lgdrSO("ItemDesc1")
                            gstrLotID = lgdrSO.LotID

                            'WO#650 Add Start
                            If gblnOvrExpDate = True AndAlso (lgdrSO.DateToPrintFlag = "1" Or lgdrSO.DateToPrintFlag = "3") Then
                                txtExpiryYear.Visible = True
                                txtExpiryMonth.Visible = True
                                txtExpiryDay.Visible = True
                                lblExpiryYear.Visible = True
                                lblExpiryMonth.Visible = True
                                lblExpiryDay.Visible = True
                                lblExpiryDate.Visible = True
                                Dim dteExiryDate As DateTime
                                If Not lgdrSO.IsProductionShelfLifeDaysNull Then
                                    dteExiryDate = DateAdd(DateInterval.Day, lgdrSO.ProductionShelfLifeDays, Today)
                                Else
                                    dteExiryDate = Today
                                End If
                                txtExpiryYear.Text = Year(dteExiryDate)
                                txtExpiryMonth.Text = Month(dteExiryDate)
                                txtExpiryDay.Text = ""
                            Else
                                txtExpiryYear.Visible = False
                                txtExpiryMonth.Visible = False
                                txtExpiryDay.Visible = False
                                lblExpiryYear.Visible = False
                                lblExpiryMonth.Visible = False
                                lblExpiryDay.Visible = False
                                lblExpiryDate.Visible = False
                            End If
                            'WO#650 Add Stop
                            LoadItemInfo(lgdrSO("itemnumber"))          'FX20200708
                        Else
                            gstrErrMsg = "Shop order is not found, please enter another shop order number."
                            MessageBox.Show(gstrErrMsg, "Invalid Information")
                            cboShopOrder.Focus()
                        End If

                    End With
                End If
            End If
        Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub cboShopOrder_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cboShopOrder.Validating
        If ActiveControl.Name <> "btnPrvScn" Then
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid/Missing Information")
                e.Cancel = True
            End If
        Else
            Close()
        End If
    End Sub
    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Try
            gstrErrMsg = Nothing
            'Retrieve equipment record using the packaging line 
            lblPkgLine.Visible = False
            cboShopOrder.DataSource = Nothing
            'FX20200708 txtItemNo.Text = String.Empty
            If txtPkgLine.Text <> "" Then
                lblPkgLine.Text = String.Empty
                lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text)
                If lblPkgLine.Text = String.Empty Then
                    gstrErrMsg = "Please enter a valid Packaging Line."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtPkgLine.Focus()
                Else
                    lblPkgLine.Visible = True
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
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub

End Class