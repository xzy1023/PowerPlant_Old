Public Class frmCreatePallet
    Dim intPalletFull As Int16
    Dim intLastPallet As Int16
    Dim gintQtyPerPallet As Integer
    Dim gstrLotID As String
    Dim gintTotalRemainingQty As Integer
    Dim gintShiftRemainingQty As Integer
    Dim gstrErrMsg As String
    Dim gblnReqExpiryDate As Boolean 'WO#650
    Dim lgdrSO As dsShopOrder.CPPsp_ShopOrderIORow 'WO#650
    Dim taOL As New dsOutputLocationTableAdapters.CPPsp_OutputLocation_SelTableAdapter  'ALM#11828
    Dim dtOL As New dsOutputLocation.CPPsp_OutputLocation_SelDataTable                  'ALM#11828


    Private Sub frmCreatePallet_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        gstrPrvForm = Me.Name
    End Sub

    Private Sub frmCreatePallet_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        'Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim intRtnCde As Integer
        Dim dr As DialogResult
        Dim strMsg As String = String.Empty
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim intCasesProducedInShift As Integer
        Dim intCarriedForwardCases As Integer
        Dim intLooseCases As Integer
        Dim blnFirstTimeInShift As Boolean = False
        'WO#2563
        'ALM#11828  Dim taOL As New dsOutputLocationTableAdapters.CPPsp_OutputLocation_SelTableAdapter
        'ALM#11828  Dim dtOL As New dsOutputLocation.CPPsp_OutputLocation_SelDataTable
        Dim intTimeLimitPalletCreation As Integer = 30

        Try
            UcHeading1.ScreenTitle = "Create Pallet"
            gblnCallFromCreatePallet = False
            intPalletFull = 2
            intLastPallet = 2
            Me.txtCasesInPallet.Text = ""
            'WO#650 ADD Start
            txtExpiryYear.Visible = False
            txtExpiryMonth.Visible = False
            txtExpiryDay.Visible = False
            lblExpiryYear.Visible = False
            lblExpiryMonth.Visible = False
            lblExpiryDay.Visible = False
            lblExpiryDate.Visible = False
            'WO#650 ADD Stop

            btnPalletFull.BackColor = Color.LemonChiffon
            btnPalletNotFull.BackColor = Color.LemonChiffon
            btnNotLastPallet.BackColor = Color.LemonChiffon
            btnLastPallet.BackColor = Color.LemonChiffon

            If SharedFunctions.IsSvrConnOK(gblnTrySessCtl) = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If

            With gdrSessCtl
                Me.lblShopOrder.Text = .Item("ShopOrder")
                Me.lblItemNo.Text = .Item("ItemNumber")
                Me.lblItemDesc.Text = .Item("ItemDesc")
                Me.lblCaseScheduled.Text = .Item("CasesScheduled")
                lblCasesInPallet.Visible = False
                Me.txtCasesInPallet.Visible = False

                'Calculate cases produced of the shop order in the current shift
                intCasesProducedInShift = SharedFunctions.GetSOCasesProducedFromPallet(.ShopOrder, .OverrideShiftNo, .StartTime.ToString, .Operator)

                tblSCH = SharedFunctions.GetSessionControlHst("LastRec_Line_SO_Shift", .OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, .Operator)
                If tblSCH.Rows.Count > 0 Then   'If the shop order has been stopped before
                    intCarriedForwardCases = SharedFunctions.CarriedForwardCasesFromShift(.OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, String.Empty)
                    'if current session has created at least 1 pallet for the SO, the loose cases should be consumed. So the loose case is 0.
                    If gdrSessCtl.PalletsCreated = 0 Then
                        intLooseCases = tblSCH.Rows(0).Item("LooseCases")
                    End If
                    blnFirstTimeInShift = False
                Else
                    blnFirstTimeInShift = True
                    If intCasesProducedInShift <> 0 Then 'at least a pallet has been created and consumed loose cases
                        intCarriedForwardCases = SharedFunctions.CarriedForwardCasesFromShift(.OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, String.Empty)
                    End If
                End If


                'Cases produced in the shift = Sum of Cases produced from the pallets in the shift + loose cases recorded in the last stop shop order in the shift 
                ' - carried forward cases recorded when the shop order was started at the first time in the shift.
                intCasesProducedInShift = intCasesProducedInShift + intLooseCases - intCarriedForwardCases
                'cases remaining in the current shift = shift cases scheduled - Cases Produced By Shift
                Me.lblCasesRemain.Text = CType(.Item("CasesScheduled"), Integer) - intCasesProducedInShift

                gintShiftRemainingQty = CType(lblCasesRemain.Text, Integer)

                'Retrieve Shop Order record using the Shop Order from the Session Control
                intRtnCde = gtaSO.Fill(tblSO, "GetSO&Item", .Facility, .ShopOrder, "")

                If tblSO.Rows.Count > 0 Then
                    lgdrSO = tblSO.Rows(0)  'WO#650
                    With tblSO.Rows(0)
                        lblTotalScheduled.Text = CType(.Item("OrderQty"), Integer)

                        'If this is the first time stop the S.O.in the shift and no pallet created in the shift, get loose cases from last session history of this S.O.
                        'else use the loose cases in the shift
                        If blnFirstTimeInShift = True And gdrSessCtl.PalletsCreated = 0 Then
                            intLooseCases = SharedFunctions.GetLooseCases(gdrSessCtl.DefaultPkgLine, gdrSessCtl.ShopOrder, gdrSessCtl.OverrideShiftNo, Now, gdrSessCtl.Facility, gdrSessCtl.Operator)
                        End If

                        'Total shop order remain qty. = Shop Order qty. - Total pallet qty. - loose cases
                        gintTotalRemainingQty = .Item("OrderQty") - CType(SharedFunctions.GetSOCasesProducedFromPallet(.Item("ShopOrder")), Integer) _
                                                - intLooseCases
                        '- SharedFunctions.GetLooseCases(gdrSessCtl.DefaultPkgLine, .Item("ShopOrder"), gdrSessCtl.OverrideShiftNo, Now, gdrSessCtl.Facility, gdrSessCtl.Operator)
                        lblTotalRemain.Text = gintTotalRemainingQty
                        gintQtyPerPallet = .Item("QtyPerPallet")
                        lblQtyPerPallet.Text = gintQtyPerPallet
                        'Me.txtCasesInPallet.Text = gintQtyPerPallet
                        gstrLotID = .Item("LotID")

                        'WO#2563 ADD Start
                        If gblnEnableOutputLocationLine = True Then
                            lblOutputLocation.Visible = True
                            cboOutputLocation.Visible = True
                            taOL.Fill(dtOL, Nothing, gdrCmpCfg.Facility, gdrCmpCfg.PackagingLine)

                            'ALM#11828  cboOutputLocation.DataSource = dtOL

                            cboOutputLocation.DisplayMember = "Location"
                            cboOutputLocation.ValueMember = "Location"

                            'ALM#11828 ADD Start
                            If gdrSessCtl.DefaultPkgLine <> gdrCmpCfg.PackagingLine Then 'ALM#11828
                                cboOutputLocation.DataSource = dtOL.Select("DestinationPackagingLine = ''")
                            Else
                                cboOutputLocation.DataSource = dtOL
                                'ALM#11828 ADD Stop
                                Try
                                    cboOutputLocation.SelectedValue = gstrLastOutputLocation
                                Catch
                                End Try
                            End If 'ALM#11828
                        Else
                            lblOutputLocation.Visible = False
                            cboOutputLocation.Visible = False
                        End If
                        'WO#2563 ADD Stop

                        'WO#650 Add Start
                        If gblnOvrExpDate = True Then
                            gblnReqExpiryDate = True
                            txtExpiryYear.Visible = True
                            txtExpiryMonth.Visible = True
                            txtExpiryDay.Visible = True
                            lblExpiryYear.Visible = True
                            lblExpiryMonth.Visible = True
                            lblExpiryDay.Visible = True
                            lblExpiryDate.Visible = True
                            Dim dteExiryDate As DateTime
                            If gdteExpiryDate = Date.MinValue Then
                                If Not IsDBNull(.Item("ProductionShelfLifeDays")) Then
                                    dteExiryDate = DateAdd(DateInterval.Day, .Item("ProductionShelfLifeDays"), Today)
                                Else
                                    dteExiryDate = Today
                                End If
                            Else
                                dteExiryDate = gdteExpiryDate
                            End If

                            txtExpiryYear.Text = Year(dteExiryDate)
                            txtExpiryMonth.Text = Month(dteExiryDate)
                            txtExpiryDay.Text = DateAndTime.Day(dteExiryDate)
                            If lgdrSO.DateToPrintFlag <> "1" And lgdrSO.DateToPrintFlag <> "3" Then
                                txtExpiryYear.Enabled = False
                                txtExpiryMonth.Enabled = False
                                txtExpiryDay.Enabled = False
                            Else
                                txtExpiryYear.Enabled = True
                                txtExpiryMonth.Enabled = True
                                txtExpiryDay.Enabled = True
                            End If
                        Else
                            gblnReqExpiryDate = False
                            txtExpiryYear.Visible = False
                            txtExpiryMonth.Visible = False
                            txtExpiryDay.Visible = False
                            lblExpiryYear.Visible = False
                            lblExpiryMonth.Visible = False
                            lblExpiryDay.Visible = False
                            lblExpiryDate.Visible = False
                        End If

                        'WO#650 Add Stop
                        'WO#755 ADD Start
                        'WO#5370 If gblnAutoCaseCountLine = True Then
                        If gblnAutoCountLine = True Then                'WO#5370
                            If CaseCounter.CaseCountInPallet > 0 Then
                                btnPalletNotFull_Click(btnPalletNotFull, e)
                                txtCasesInPallet.Text = CaseCounter.CaseCountInPallet.ToString
                            End If
                        End If
                        'WO#755 ADD Stop
                    End With
                Else
                    MessageBox.Show("Shop order detail information is not found.", "** Unexpected Application Error")
                    Me.Close()
                End If

            End With
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If

            'If gdrCmpCfg.ShowEfficiency = True Then
            'WO#2645    Dim sft As New WorkShift(Now(), gdrCmpCfg("WorkShiftType"))  
            Dim sft As New WorkShift    'WO#2645
            sft.GetExpectedShiftInfoByTime(Now(), gdrCmpCfg("WorkShiftType"), True)   'WO#2645
            If gdrSessCtl.OverrideShiftNo <> sft.Shift Or gdrSessCtl.ProductionDate > sft.ToTime Or gdrSessCtl.ProductionDate < sft.FromTime Then
                Dim sb As New System.Text.StringBuilder
                sb.Length = 0
                sb.Append("Entered shift is different from the shift based on current time. Did the last operator forget to stop the shop order ")
                sb.Append("or you forgot to stop the shop order to enter the new shift?")
                sb.AppendLine()
                sb.AppendLine()
                sb.AppendFormat("If 'Yes', you will be required to stop the shop order and log on to current shift '{0}'.", sft.Shift)
                sb.AppendLine()
                'WO#2645    sb.Append("If 'No', you will continue to create the pallet belongs to shift ")
                'WO#2645    sb.AppendFormat("'{0}' and {1}.", gdrSessCtl.OverrideShiftNo, SharedFunctions.GetStaffName(gdrSessCtl.Operator, "P"))
                sb.Append("If 'No', you will continue to create the pallet belongs to ")                                                        'WO#2645
                sb.AppendFormat("'{0}' for shift {1}.", SharedFunctions.GetStaffName(gdrSessCtl.Operator, "P"), gdrSessCtl.OverrideShiftNo)     'WO#2645
                strMsg = sb.ToString

                dr = SharedFunctions.PoPUpMSG(strMsg, "Work Shift Changed?", MessageBoxButtons.YesNo)
                If dr = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                    frmStopShopOrder.ShowDialog()
                End If
            End If
            'End If

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
        lblCasesRemain.Text = gintShiftRemainingQty - Me.lblQtyPerPallet.Text
        lblTotalRemain.Text = gintTotalRemainingQty - Me.lblQtyPerPallet.Text
    End Sub

    Private Sub btnPalletNotFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPalletNotFull.Click
        btnPalletFull.BackColor = Color.LemonChiffon
        btnPalletNotFull.BackColor = Color.LightSeaGreen
        intPalletFull = False
        txtCasesInPallet.Text = ""
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

    'Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtCasesInPallet.MouseDown
    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtCasesInPallet.MouseDown, txtExpiryYear.MouseDown, _
        txtExpiryMonth.MouseDown, txtExpiryDay.MouseDown    'WO#650
        Dim dgrKeyPad As DialogResult
        Try
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

    Private Sub btnCreatePallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatePallet.Click
        Dim strOrderComplete As String
        Dim intLooseCases As Integer
        Dim dteProductionDate As DateTime
        Dim strExpiryDate As String 'WO#650
        Dim edteItem As New clsExpiryDate(lgdrSO.ProductionShelfLifeDays, lgdrSO.ShipShelfLifeDays) 'WO#650
        Dim dteExpiryDate As DateTime 'WO#650
        Dim strMsg As String = Nothing 'WO#650
        Dim strOutputLoacation As String = Nothing     'WO#2563
        Dim intDestinationShopOrder As Integer = 0                  'ALM#11828
        Dim strDestinationPackagingLine As String = ""              'ALM#11828
        Dim strResult As String()                                   'ALM#11828
        'WO#5370    Dim drOL As dsOutputLocation.CPPsp_OutputLocation_SelRow    'ALM#11828
        Dim blnConnected As Boolean = False                         'ALM#11828
        Dim drOutputLocationMSG As DialogResult                     'ALM#11828
        Dim intPalletCreationTimeLimit As Integer = -1              'WO#34957
        Dim strArrCtlValues As String()                             'WO#34957
        Dim dtePreviousPalletCreationTime As DateTime               'WO#34957
        Dim strArrIPCCtlValues As String()                          'WO#34957
        Dim drOL As dsOutputLocation.CPPsp_OutputLocation_SelRow    'WO#34957
        Try
            'WO#650 ADD Start
            If gblnReqExpiryDate Then
                strExpiryDate = txtExpiryMonth.Text & "/" & txtExpiryDay.Text & "/" & txtExpiryYear.Text
            Else
                strExpiryDate = String.Empty
            End If
            'WO#650 ADD Stop

            'ALM#11828 ADD Start
            If gblnEnableOutputLocationLine = True Then
                strOutputLoacation = cboOutputLocation.SelectedValue
                'WO#5370 DEL Start
                'If strOutputLoacation = gstrLastOutputLocation Then
                '    strDestinationPackagingLine = gStrLastDestinationPkgLine.Trim
                'Else
                '    strDestinationPackagingLine = Nothing
                '    Dim query = From OL In dtOL.AsEnumerable()
                '                Where OL.Location = strOutputLoacation
                '                Select OL
                '    For Each drOL In query
                '        strDestinationPackagingLine = drOL.DestinationPackagingLine.Trim
                '    Next drOL
                'End If
                'WO#5370 DEL Stop
                'FX20200616 ADD Start 
                'If the line is enabled to specify the output location but does not automatic counting production, 
                'find the Destination Packaging Line from the output location
                If gblnSarongAutoCountLine = False Then
                    If strOutputLoacation = gstrLastOutputLocation Then
                        strDestinationPackagingLine = gStrLastDestinationPkgLine.Trim
                    Else
                        strDestinationPackagingLine = Nothing
                        Dim query = From OL In dtOL.AsEnumerable()
                                    Where OL.Location = strOutputLoacation
                                    Select OL
                        For Each drOL In query
                            strDestinationPackagingLine = drOL.DestinationPackagingLine.Trim
                        Next drOL
                    End If
                End If
                'FX20200616 ADD Stop

                'get current active destination Shop order no. by the destination packaging line.
                If IsNothing(strDestinationPackagingLine) = False AndAlso strDestinationPackagingLine <> "" Then
                    strResult = SharedFunctions.GetCurDestinationShopOrder(strOutputLoacation, Not gblnLastDestinationIPCConn, dtOL).Split(New Char() {","})   'WO#5370
                    'WO#5370    strResult = SharedFunctions.GetCurDestinationShopOrder(strDestinationPackagingLine, Not gblnLastDestinationIPCConn).Split(New Char() {","})
                    intDestinationShopOrder = strResult(0)
                    blnConnected = strResult(1)
                End If
            End If
            'ALM#11828 ADD Stop
            'WO#34957 ADD Start
            strArrCtlValues = SharedFunctions.GetConrolTableValues("PalletCreationTimeLimit", "FrmCreatePallet")
            If Not Integer.TryParse(strArrCtlValues(0), intPalletCreationTimeLimit) Then
                intPalletCreationTimeLimit = -1
            End If
            strArrIPCCtlValues = SharedFunctions.GetIPCControl("PreviousPalletCreationTime")
            If Not DateTime.TryParse(strArrIPCCtlValues(0), dtePreviousPalletCreationTime) Then
                dtePreviousPalletCreationTime = DateTime.MinValue
            End If
            'WO#34957 ADD Stop
            If intPalletFull = False And txtCasesInPallet.Text = "" Then
                MessageBox.Show("Case in pallet is required.", "Missing Information")
                txtCasesInPallet.Focus()
            ElseIf intLastPallet = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' for the question, 'Last Pallet for this order?'.", "Missing information")
                Me.btnLastPallet.Focus()
            ElseIf intPalletFull = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' for the question, 'Is this a full pallet full?'.", "Missing information")
                Me.btnPalletFull.Focus()
                'WO#650 Add Start
            ElseIf gblnReqExpiryDate AndAlso Me.txtExpiryYear.Text = "" Then
                MessageBox.Show("Please enter Expiry Year.", "Missing information")
                txtExpiryYear.Focus()
            ElseIf gblnReqExpiryDate AndAlso txtExpiryMonth.Text = "" Then
                MessageBox.Show("Please enter Expiry Month.", "Missing information")
                txtExpiryMonth.Focus()
            ElseIf gblnReqExpiryDate AndAlso txtExpiryDay.Text = "" Then
                MessageBox.Show("Please enter Expiry Day.", "Missing information")
                txtExpiryDay.Focus()
            ElseIf gblnReqExpiryDate AndAlso DateTime.TryParse(strExpiryDate, dteExpiryDate) = False Then
                txtExpiryDay.Focus()
            ElseIf gblnReqExpiryDate AndAlso edteItem.IsExpiryDateValid(dteExpiryDate, Today) = False Then
                MessageBox.Show("Expiry date must be between " & edteItem.EarilestExpiryDate.ToString("MM/dd/yyyy") & _
                                " and " & edteItem.LatestExpiryDate.ToString("MM/dd/yyyy"))
                txtExpiryDay.Focus()
                'WO#650 ADD Stop
                'ALM#11828 ADD Start
            ElseIf IsNothing(strDestinationPackagingLine) = True Then
                MessageBox.Show("Cannot find the related Destination Packaging Line from the selected Output Locaton. Please contact supervisor.", "Missing information")
                cboOutputLocation.Focus()
                'ALM#11828 ADD Stop
                'WO#34957 ADD Start
            ElseIf DateDiff(DateInterval.Second, dtePreviousPalletCreationTime, Now) <= intPalletCreationTimeLimit Then
                MessageBox.Show(String.Format("Cannot create more than a pallet within {0} seconds. Last time was at {1}. No pallet will be created.", intPalletCreationTimeLimit, dtePreviousPalletCreationTime), "Pallet Creation Constraint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                btnCreatePallet.Focus()
                'WO#34957 ADD Stop
            Else
                'WO#650 ADD Start
                strMsg = SharedFunctions.IsItemChangedOnServer(lgdrSO)
                If Not IsNothing(strMsg) Then
                    MessageBox.Show(strMsg, "Data Integrity Error")
                Else
                    'WO#650 ADD Stop
                    'ALM#11828 ADD Start
                    'strDestinationPackagingLine is blank means the the destination line is bulk pack.
                    If gblnEnableOutputLocationLine = True AndAlso strDestinationPackagingLine <> "" Then
                        If gblnSvrConnIsUp = False Then
                            drOutputLocationMSG = MessageBox.Show("Cannot connect to data server to find out the current destination shop order. Do you want to use the last destination shop order """ & gintLastDestinationShopOrder.ToString & """?", "Warning - What is the Destination Shop Order?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            If drOutputLocationMSG = Windows.Forms.DialogResult.Yes Then
                                intDestinationShopOrder = gintLastDestinationShopOrder
                            Else
                                intDestinationShopOrder = 0
                            End If
                        ElseIf blnConnected = False Then
                            drOutputLocationMSG = MessageBox.Show("Cannot connect to destination line """ & Trim(strDestinationPackagingLine) & """ to find out the current destination shop order. Do you want to use the last destination shop order """ & gintLastDestinationShopOrder.ToString & """?", "Warning - What is the Destination Shop Order?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            If drOutputLocationMSG = Windows.Forms.DialogResult.Yes Then
                                intDestinationShopOrder = gintLastDestinationShopOrder
                            Else
                                intDestinationShopOrder = 0
                            End If
                        ElseIf intDestinationShopOrder = 0 AndAlso blnConnected = True Then
                            drOutputLocationMSG = MessageBox.Show("Destination line " & Trim(strDestinationPackagingLine) & " has not started a shop order. Do you want to continue anyway?", "Warning - What is the Destination Shop Order?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            If drOutputLocationMSG = Windows.Forms.DialogResult.Yes Then
                                intDestinationShopOrder = 0
                            Else
                                Exit Sub
                            End If
                        End If
                        If intDestinationShopOrder <> 0 AndAlso gintLastDestinationShopOrder <> intDestinationShopOrder AndAlso SharedFunctions.IsComponentInBOM(intDestinationShopOrder, lblItemNo.Text) = False Then
                            drOutputLocationMSG = MessageBox.Show("Shop order item is not the component of the destination shop order " & intDestinationShopOrder.ToString & " item. Do you want to continue anyway?", "Warning - Check the Shop Order item?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            If drOutputLocationMSG = Windows.Forms.DialogResult.No Then
                                Exit Sub
                            End If
                        End If
                    End If
                    'ALM#11828 ADD Stop

                    Cursor = Cursors.WaitCursor
                    If intLastPallet = True Then
                        strOrderComplete = "Y"
                    Else
                        strOrderComplete = "N"
                    End If

                    If intPalletFull = True Then
                        intLooseCases = gintQtyPerPallet
                    Else
                        intLooseCases = CType(Me.txtCasesInPallet.Text, Integer)
                    End If

                    gdteExpiryDate = dteExpiryDate   'WO#650

                    'ALM#11828 DEL Start
                    ''WO#2563 ADD Start
                    'If gblnEnableOutputLocationLine = True Then
                    '    strOutputLoacation = cboOutputLocation.SelectedValue
                    'End If
                    ''WO#2563 ADD Stop
                    'ALM#11828 DEL Stop

                    'Create Pallet
                    dteProductionDate = Now()
                    With gdrSessCtl
                        'WO#2563 DEL Start
                        'SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), CType(lblShopOrder.Text, Integer), lblItemNo.Text, .Item("DefaultPkgLine"), _
                        '                                       .Item("Operator"), intLooseCases, .Item("StartTime"), strOrderComplete, gintQtyPerPallet, _
                        '                                       gstrLotID, Format(dteProductionDate, "yyyyMMdd"), .OverrideShiftNo, gdrCmpCfg("PalletStation"))
                        'WO#2563 DEL Stop
                        'ALM#11828 ADD Start
                        SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), CType(lblShopOrder.Text, Integer), lblItemNo.Text, .Item("DefaultPkgLine"),
                                        .Operator, intLooseCases, .Item("StartTime"), strOrderComplete, gintQtyPerPallet,
                                        gstrLotID, dteProductionDate, .OverrideShiftNo, strOutputLoacation, gdrCmpCfg("PalletStation") _
                                        , intDestinationShopOrder _
                                        , 0, "Production Line IPC") 'WO#5370
                        'WO#5370 )
                        'ALM#11828 ADD Stop 
                        'WO#2563 ADD Start
                        'ALM#11828 DEL Start
                        'SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), CType(lblShopOrder.Text, Integer), lblItemNo.Text, .Item("DefaultPkgLine"), _
                        '                                      .Operator, intLooseCases, .Item("StartTime"), strOrderComplete, gintQtyPerPallet, _
                        '                                       gstrLotID, dteProductionDate, .OverrideShiftNo, strOutputLoacation, gdrCmpCfg("PalletStation"))
                        'ALM#11828 DEL Stop
                        If gblnEnableOutputLocationLine = True Then                         'ALM#11828 
                            gStrLastDestinationPkgLine = strDestinationPackagingLine        'ALM#11828 
                            gintLastDestinationShopOrder = intDestinationShopOrder          'ALM#11828
                            gblnLastDestinationIPCConn = blnConnected                       'ALM#11828
                            gstrLastOutputLocation = strOutputLoacation
                            'WO#2563 ADD Stop
                        End If                                                              'ALM#11828 
                    End With
                    Cursor = Cursors.Default
                    'Me.lblCasesRemain.Text = CType(Me.lblCasesRemain.Text, Integer) - intLooseCases

                    'If it is the Last Pallet For This Shop Order, then display the Stop Shop Order screen.
                    If intLastPallet = True Then
                        gblnCallFromCreatePallet = True
                        'If gstrQATWorkFlowType <> QATWorkFlow.Disabled Then                 'WO#19498
                        frmStopShopOrder.ShowDialog()
                        'End If                                                              'WO#19498
                    End If

                    If gdrCmpCfg.ShowEfficiency = True And gdrSessCtl.ServerCnnIsOk = True Then
                        frmProcessMonitor.ShowDialog()
                    End If

                    Me.Close()
                End If 'WO#650
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

    Private Sub txtCasesInPallet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCasesInPallet.TextChanged
        Try
            gstrErrMsg = Nothing
            'WO#755 If ActiveControl.Name = "btnPalletFull" Then
            If Not IsNothing(ActiveControl) AndAlso ActiveControl.Name = "btnPalletFull" Then   'WO#755
                txtCasesInPallet.Text = ""
                txtCasesInPallet.Visible = False
                lblCasesInPallet.Visible = False
            Else
                If txtCasesInPallet.Text <> "" Then
                    If InStr(txtCasesInPallet.Text, ".") > 0 Then
                        gstrErrMsg = "Please enter the number without an decimal."
                        'MessageBox.Show(gstrErrMsg, "Invalid Information")
                        txtCasesInPallet.Focus()
                    Else
                        Dim intCasesInPallet As Integer = CType(Me.txtCasesInPallet.Text, Integer)
                        If intCasesInPallet <= 0 Then
                            gstrErrMsg = "Cases in pallet must be greater than zero."
                            'MessageBox.Show(gstrErrMsg, "Invalid Information")
                            txtCasesInPallet.Focus()
                        ElseIf intCasesInPallet > gintQtyPerPallet Then
                            gstrErrMsg = "Cases in pallet can not greater than the pallet quantity."
                            'MessageBox.Show(gstrErrMsg, "Invalid Information")
                            txtCasesInPallet.Focus()
                        Else
                            lblCasesRemain.Text = gintShiftRemainingQty - txtCasesInPallet.Text
                            lblTotalRemain.Text = gintTotalRemainingQty - txtCasesInPallet.Text
                        End If
                    End If
                Else
                    If intPalletFull = False Then
                        gstrErrMsg = "Case in pallet is required."
                        'MessageBox.Show(gstrErrMsg, "Missing Information")
                        txtCasesInPallet.Focus()
                    End If
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtCasesInPallet_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCasesInPallet.Validating
        If ActiveControl.Name <> "btnPalletFull" Then
            If ActiveControl.Name <> "btnPrvScn" Then
                If Not IsNothing(gstrErrMsg) Then
                    MessageBox.Show(gstrErrMsg, "Invalid/Missing Information")
                    e.Cancel = True
                End If
            Else
                Close()
            End If
        End If
    End Sub

    'WO#650 ADD Start
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
    'WO#650 ADD Stop
End Class