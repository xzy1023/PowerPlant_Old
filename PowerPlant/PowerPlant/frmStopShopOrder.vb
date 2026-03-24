' WO #297 Palletizer Signal
' - Add Close Shop Order label, Yes and No buttons
' - Show these label and buttons only not if it is called from Create Pallet Screen

Imports System.Linq

Public Class frmStopShopOrder
    Dim intPalletFull As Int16
    Dim intLogScraps As Int16
    Dim intLogRejectPoints As Int16
    Dim blnCloseShopOrder As Boolean        'WO#297
    Dim gintQtyPerPallet As Integer
    Dim gstrBagLengthRequired As String
    Dim gstrErrmsg As String
    Dim gintTotalProduced As Integer
    Dim gintOriginalShiftProduced As Integer

    Dim _dbServer As New ServerModels.PowerPlantEntities()

    Private Sub frmStopShopOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim drSO As dsShopOrder.CPPsp_ShopOrderIORow
        Dim intRtnCde As Integer
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim intCarriedForwardCases As Integer
        Dim intLooseCases As Integer
        'Dim wsCurrent As WorkShift
        Dim intCasesProducedInShift As Integer
        Dim blnFirstTimeInShift As Boolean = False

        Try
            UcHeading1.ScreenTitle = "STOP SHOP ORDER"

            Me.DialogResult = Windows.Forms.DialogResult.None

            txtLooseCases.Visible = False
            lblLooseCases.Visible = False
            txtLooseCases.Text = String.Empty
            If gblnCallFromCreatePallet Then
                btnPrvScn.Visible = False
            Else
                btnPrvScn.Visible = True
            End If
            If gblnCallFromCreatePallet Then
                btnPalletFull.Visible = False
                btnPalletNotFull.Visible = False
                lblIsLastPallet.Visible = False
                lblFullPalletQty.Visible = False
                lblQtyPerPallet.Visible = False
                lblCloseSO.Visible = False      'WO#297
                btnCloseSO.Visible = False      'WO#297
                btnNoCloseSO.Visible = False    'WO#297
                blnCloseShopOrder = True        'WO#297
            Else
                btnPalletFull.Visible = True
                btnPalletNotFull.Visible = True
                lblIsLastPallet.Visible = True
                lblFullPalletQty.Visible = True
                lblQtyPerPallet.Visible = True
                btnPalletFull.BackColor = Color.LemonChiffon
                btnPalletNotFull.BackColor = Color.LemonChiffon
                btnLogScraps.BackColor = Color.LemonChiffon
                btnNoLogScraps.BackColor = Color.LemonChiffon
                btnLogRejectPoints.BackColor = Color.LemonChiffon
                btnNoLogRejectPoints.BackColor = Color.LemonChiffon
                lblCloseSO.Visible = True       'WO#297
                btnCloseSO.Visible = True       'WO#297
                btnNoCloseSO.Visible = True     'WO#297
                btnNoCloseSO.BackColor = Color.LightSeaGreen        'WO#297
                btnCloseSO.BackColor = Color.LemonChiffon           'WO#297
                blnCloseShopOrder = False       'WO#297
            End If

            lblMessage.Text = ""    'WO#650

            intPalletFull = 2       'WO#755 
            intLogScraps = 2        'WO#755 
            intLogRejectPoints = 2

            'SharedFunctions.clearInputFields(Me)
            SharedFunctions.IsSvrConnOK(gblnTrySessCtl, Me)

            With gdrSessCtl

                lblShopOrder.Text = .Item("ShopOrder")
                lblItemNo.Text = .Item("ItemNumber")
                lblCaseScheduledInShift.Text = .Item("CasesScheduled")
                lblFromCarriedFwd.Text = .CarriedForwardCases
                'Calculate cases produced of the shop order in the current shift
                intCasesProducedInShift = SharedFunctions.GetSOCasesProducedFromPallet(.ShopOrder, .OverrideShiftNo, .StartTime.ToString, .Operator)

                'If the shop order is the first time to stop in the shift, 
                '--If at least a pallet has been created in the shift, 
                '----subtract the carried forward cases from Cases Produced By Shift only (i.e. carried forward cases were consumed in a pallet)
                '--Else
                '----exclude both loose and carried forward cases to Cases Produced By Shift (i.e. has not produced any thing yet)
                'Else
                '--if current session has created at least 1 pallet for the SO
                '-----include carried forward case only to Cases Produced By Shift
                '--Else
                '-----include both loose and carried forward case to Cases Produced By Shift

                tblSCH = SharedFunctions.GetSessionControlHst("SelectLastRecByLineSO", .OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, .Operator)
                If tblSCH.Rows.Count > 0 Then   'If the shop order has been stopped before
                    tblSCH.Clear()
                    tblSCH = SharedFunctions.GetSessionControlHst("LastRec_Line_SO_Shift", .OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, .Operator)
                    'wsCurrent = New WorkShift(.OverrideShiftNo, .StartTime, gdrCmpCfg("WorkShiftType"))
                    'If the shop order has been stopped in the shift
                    'If wsCurrent.FromTime <= tblSCH.Rows(0).Item("StartTime") And wsCurrent.ToTime >= tblSCH.Rows(0).Item("StartTime") Then
                    If tblSCH.Rows.Count > 0 Then
                        intCarriedForwardCases = SharedFunctions.CarriedForwardCasesFromShift(.OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, String.Empty)
                        'if current session has created at least 1 pallet for the SO, the loose cases should be consumed. So the loose case is 0.
                        If gdrSessCtl.PalletsCreated = 0 Then
                            intLooseCases = tblSCH.Rows(0).Item("LooseCases")
                        End If
                        blnFirstTimeInShift = False
                    Else
                        blnFirstTimeInShift = True
                        'If intCasesProducedInShift <> 0 Then 'at least a pallet has been created and consumed loose cases
                        intCarriedForwardCases = SharedFunctions.CarriedForwardCasesFromShift(.OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, String.Empty)
                        'End If
                    End If
                End If

                gintOriginalShiftProduced = intCasesProducedInShift + intLooseCases - intCarriedForwardCases
                'WO#359 
                If gintOriginalShiftProduced < 0 And intCarriedForwardCases > 0 Then
                    gintOriginalShiftProduced = 0
                End If
                lblCasesProducedInShift.Text = gintOriginalShiftProduced

                'Cases remain in the current shift = case Scheduled in the shift - cases produced in the current shift 
                Me.lblCasesRemainInShift.Text = .Item("CasesScheduled") - lblCasesProducedInShift.Text
                Me.txtRework.Text = ""

                'Retrieve Shop Order record using the Shop Order from the Session Control
                intRtnCde = gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), .Item("ShopOrder"), "")

                'FX150505 ADD Start'
                'If the shop order can not be found in the current Shop Order table, try to get it from shop order history from the server.
                If tblSO.Rows.Count = 0 Then
                    gtaSO.Connection.ConnectionString = My.Settings.ServerPowerPlantCnnStr
                    gtaSO.Fill(tblSO, "GetSOHist&Item", gdrSessCtl("Facility"), .Item("ShopOrder"), "")
                    gtaSO.Connection.ConnectionString = My.Settings.LocalPowerPlantCnnStr
                End If
                'FX150505 ADD Stop'

                If tblSO.Rows.Count > 0 Then
                    drSO = tblSO.Rows(0)
                    With drSO
                        If .Item("BagLengthRequired") = "Y" Then
                            Me.txtBagLengthUsed.Visible = True
                            Me.lblBagLengthUsed.Visible = True
                            Me.txtBagLengthUsed.Text = gdrSessCtl("BagLengthUsed")
                        Else
                            Me.txtBagLengthUsed.Visible = False
                            Me.lblBagLengthUsed.Visible = False
                        End If
                        gstrBagLengthRequired = .Item("BagLengthRequired")
                        gintQtyPerPallet = .Item("QtyPerPallet")
                        lblQtyPerPallet.Text = gintQtyPerPallet
                        Me.lblItemDesc.Text = .Item("ItemDesc1")
                        If lblItemNo.Text = String.Empty Then                               'FX151220
                            lblItemNo.Text = .ItemNumber                                    'FX151220
                        End If                                                              'FX151220
                        lblTotalScheduled.Text = CType(.Item("OrderQty"), Integer)

                        'If this is the first time stop the S.O.in the shift and no pallet created in the shift, get loose cases from last session history of this S.O.
                        'else use the loose cases in the shift

                        If blnFirstTimeInShift = True And gdrSessCtl.PalletsCreated = 0 Then
                            intLooseCases = SharedFunctions.GetLooseCases(gdrSessCtl.DefaultPkgLine, .ShopOrder, gdrSessCtl.OverrideShiftNo, Now, gdrSessCtl.Facility, gdrSessCtl.Operator)
                        End If

                        'Total Producted of the SO = Previous quantity finished from the SO table + Cases produced of the shop order + loose cases from last session of the shop order (if no pallet created in the current session)
                        gintTotalProduced = .FinishedQty + CType(SharedFunctions.GetSOCasesProducedFromPallet(.Item("ShopOrder")), Integer) +
                                   intLooseCases
                        lblTotalProduced.Text = gintTotalProduced
                        lblTotalRemain.Text = CType(lblTotalScheduled.Text, Integer) - gintTotalProduced

                        'WO#755 ADD Start
                        'WO#5370    If gblnAutoCaseCountLine = True And Not IsNothing(CaseCounter) Then
                        If gblnAutoCountLine = True And Not IsNothing(CaseCounter) Then                 'WO#5370
                            lblTotalProduced.Text = CaseCounter.CasesProducedRunningTotal
                            lblTotalRemain.Text = CType(.Item("OrderQty"), Integer) - CaseCounter.CasesProducedRunningTotal
                            lblCasesProducedInShift.Text = CaseCounter.CasesProducedInShift
                            lblCasesRemainInShift.Text = gdrSessCtl.CasesScheduled - CaseCounter.CasesProducedInShift
                            If (CaseCounter.CaseCountInPallet Mod drSO.QtyPerPallet > 0) Or intCarriedForwardCases > 0 Then
                                btnPalletNotFull_Click(btnPalletNotFull, e)
                                'txtLooseCases.Text = gintLooseCases.ToString
                                'txtLooseCases.Text = intCarriedForwardCases + CaseCounter.CaseCountInPallet
                                txtLooseCases.Text = CaseCounter.CaseCountInPallet
                            Else
                                btnPalletFull_Click(btnPalletFull, e)
                            End If
                        End If
                        'WO#755 ADD Stop
                    End With
                Else
                    MessageBox.Show("Shop order detail information is not found.", "** Unexpected Application Error")
                    Me.Close()
                End If
            End With
            'WO#755 intPalletFull = 2
            'WO#755 intLogScraps = 2
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If

            ' If there is no scrap reject point for the line, hide the button
            If _dbServer.tblEquipment_ScrapRejectPoint.AsQueryable.Where(Function(x) x.EquipmentID = gdrSessCtl.DefaultPkgLine).Count = 0 Then
                lblLogRejectPoints.Visible = False
                btnLogRejectPoints.Visible = False
                btnNoLogRejectPoints.Visible = False
                intLogRejectPoints = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub btnPalletFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPalletFull.Click
        btnPalletFull.BackColor = Color.LightSeaGreen
        btnPalletNotFull.BackColor = Color.LemonChiffon
        intPalletFull = True
        txtLooseCases.Text = 0
        txtLooseCases.Visible = False
        lblLooseCases.Visible = False
    End Sub

    Private Sub btnPalletNotFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPalletNotFull.Click
        btnPalletFull.BackColor = Color.LemonChiffon
        btnPalletNotFull.BackColor = Color.LightSeaGreen
        intPalletFull = False
        txtLooseCases.Visible = True
        lblLooseCases.Visible = True
        txtLooseCases.Focus()
    End Sub

    Private Sub btnLogScraps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogScraps.Click
        btnLogScraps.BackColor = Color.LightSeaGreen
        btnNoLogScraps.BackColor = Color.LemonChiffon
        intLogScraps = True
        frmLogScraps.ShowDialog()
    End Sub

    Private Sub btnNoLogScraps_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoLogScraps.Click
        btnNoLogScraps.BackColor = Color.LightSeaGreen
        btnLogScraps.BackColor = Color.LemonChiffon
        intLogScraps = False
    End Sub
    Private Sub btnLogRejectPoints_Click(sender As Object, e As EventArgs) Handles btnLogRejectPoints.Click
        btnLogRejectPoints.BackColor = Color.LightSeaGreen
        btnNoLogRejectPoints.BackColor = Color.LemonChiffon
        intLogRejectPoints = True
        frmLogScrapsRejectPoint.ShowDialog()
    End Sub

    Private Sub btnNoLogRejectPoints_Click(sender As Object, e As EventArgs) Handles btnNoLogRejectPoints.Click
        btnNoLogRejectPoints.BackColor = Color.LightSeaGreen
        btnLogRejectPoints.BackColor = Color.LemonChiffon
        intLogRejectPoints = False
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnCloseSO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseSO.Click
        btnCloseSO.BackColor = Color.LightSeaGreen          'WO#297
        btnNoCloseSO.BackColor = Color.LemonChiffon         'WO#297
        blnCloseShopOrder = True                            'WO#297
    End Sub

    Private Sub btnNoCloseSO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoCloseSO.Click
        btnNoCloseSO.BackColor = Color.LightSeaGreen        'WO#297
        btnCloseSO.BackColor = Color.LemonChiffon           'WO#297
        blnCloseShopOrder = False                           'WO#297
    End Sub
    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
       txtBagLengthUsed.MouseDown, txtRework.MouseDown, txtLooseCases.MouseDown
        Dim dgrKeyPad As DialogResult
        dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)
        If dgrKeyPad = Windows.Forms.DialogResult.OK Then
            sender.text = gstrNumPadValue
        End If
    End Sub

    Private Sub btnStopShopOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopShopOrder.Click
        Dim blnHasError As Boolean = False

        Try
            'If Trim(Me.txtCasesProduced.Text) = "" Or Me.txtCasesProduced.Text = "0" Then
            '    MessageBox.Show("Please enter Case Produced.", "Missing information")
            '    blnHasError = True
            '    Me.txtCasesProduced.Focus()
            'Else
            If gstrBagLengthRequired = "Y" AndAlso (RTrim(Me.txtBagLengthUsed.Text) = "" Or Me.txtBagLengthUsed.Text = "0.0") Then
                MessageBox.Show("Please enter Bag Length.", "Missing information")
                blnHasError = True
                Me.txtBagLengthUsed.Focus()
            ElseIf intLogScraps = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' on Log Scraps.", "Missing information")
                blnHasError = True
                Me.btnLogScraps.Focus()
            ElseIf intLogRejectPoints = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' on Log Reject Points.", "Missing information")
                blnHasError = True
                Me.btnLogRejectPoints.Focus()
            ElseIf Not gblnCallFromCreatePallet AndAlso intPalletFull = 2 Then
                'ElseIf intPalletFull = 2 Then
                MessageBox.Show("Please select 'Yes' or 'No' for the question, Is the Last Pallet Full?", "Missing information")
                blnHasError = True
                Me.btnPalletFull.Focus()
                'ElseIf intPalletFull = 0 Then
                '    If Me.txtLooseCases.Text = "" Or Me.txtLooseCases.Text = "0" Then
                '        drLooseCase = MessageBox.Show("Entered Loose Cases is zero. Are you sure?", "Warning Message - Loose Cases", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                '        If drLooseCase = Windows.Forms.DialogResult.No Then
                '            'MessageBox.Show("Please enter No. of Loose Cases", "Missing information")
                '            blnHasError = True
                '            txtLooseCases.Focus()
                '        End If
                '        'Else
                '        '    If chkLooseCases() = False Then
                '        '        blnHasError = True
                '        '        txtLooseCases.Focus()
                '        '    End If
                '    End If
                'WO#650 ADD Start
            ElseIf SharedFunctions.IsProcessActive("DownTime") Then
                SharedFunctions.PoPUpMSG("Please the finish up logging the down time and stop the shop order again. Click OK to briing back the Down Time Screen.", "Warning", MessageBoxButtons.OK)
                SharedFunctions.WakeUpAPgm("DownTime")
                blnHasError = True
                'WO#650 ADD Stop
            End If

            If blnHasError = False Then
                Cursor = Cursors.WaitCursor
                'WO#297 SharedFunctions.StopShopOrderUpdate(Me) 

                ''WO#755 ADD Start
                'WO#5370    Dim intCasesProducedInSession As Integer = 0
                'WO#5370    Dim intCasesProducedRunningTotal As Integer = 0
                'WO#5370 If gblnAutoCaseCountLine = True AndAlso gdrSessCtl.ShopOrder <> 0 Then
                If Not gblnSarongAutoCountLine AndAlso gdrSessCtl.ShopOrder <> 0 Then                      'WO#5370
                    '    If Not IsNothing(CaseCounter) AndAlso CaseCounter.CounterIsStarted = True Then

                    '        Dim xmlInput As New PowerPlant.XMLInterface(gstrLabelInputFileName)
                    '        xmlInput.StopShopOrder()
                    '    End If
                    If Not CaseCounter Is Nothing Then
                        'WO#5370    intCasesProducedInSession = CaseCounter.CasesProducedInSession
                        'WO#5370    intCasesProducedRunningTotal = CaseCounter.CasesProducedRunningTotal
                        CaseCounter.Abort()
                    End If
                End If

                SharedFunctions.RefreshSessionControlTable()
                'WO#755 ADD Stop


                'WO#755 SharedFunctions.StopShopOrderUpdate(Me, blnCloseShopOrder) 'WO#297
                'WO#5370 SharedFunctions.StopShopOrderUpdate(Me, blnCloseShopOrder, intCasesProducedInSession, intCasesProducedRunningTotal) 'WO#755
                blnHasError = SharedFunctions.StopShopOrderUpdate(Me, blnCloseShopOrder) 'WO#5370

                'WO#5370 ADD Start
                If blnHasError = False Then
                    If Not CaseCounter Is Nothing Then
                        CaseCounter.Abort()
                        'frmProcessMonitor.Close()
                    End If
                    'WO#5370 ADD Start

                    CaseCounter = Nothing

                    gblnCallFromCreatePallet = False

                    'WO#17432 ADD Start         Update shop order close column in the QATStatus table
                    Dim drQATStatus As dsQATStatus.tblQATStatusRow
                    If blnCloseShopOrder = True AndAlso gdrCmpCfg.QATWorkFlowInitiation <> QATWorkFlow.Disabled Then
                        drQATStatus = SharedFunctions.GetQATStatus()
                        If Not IsNothing(drQATStatus) Then
                            With drQATStatus
                                SharedFunctions.UpdateQATStatus(.ByPassAllTests, .ByPassTest, .ShopOrder,
                                           .QATEntryPoint, .QATDefnID, .InterfaceID, .WFTestSeq, False, True)
                            End With
                        End If
                    End If
                    'WO#17432 ADD Stop

                    SharedFunctions.IsSvrConnOK(gblnTryConnect, Me)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If              'WO#5370
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    'Private Sub btnPalletFull_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPalletFull.Validated, btnCloseSO.Validated
    '    txtLooseCases.Text = ""
    '    txtLooseCases.Visible = False
    '    lblLooseCases.Visible = False
    'End Sub


    Private Sub txtLooseCases_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtLooseCases.Validating, btnPalletFull.Validating, btnCloseSO.Validating
        Dim drtLooseCases As DialogResult
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim drSCH As dsSessionControlHst.CPPsp_SessionControlHstIORow               'WO#755
        Dim blnWarning As Boolean = False
        Dim intCasesInPallet As Integer
        Try
            Select Case ActiveControl.Name
                Case "btnPrvScn"
                    Me.Close()
                    'Case "btnPalletFull"
                    '    txtLooseCases.Text = ""
                    '    txtLooseCases.Visible = False
                    '    lblLooseCases.Visible = False
                Case "btnPalletFull"
                    intCasesInPallet = 0
                    txtLooseCases.Text = String.Empty
                Case "txtLooseCases"
                    e.Cancel = True
                    btnPalletFull.Focus()
                    Exit Sub
                Case Else

                    'If sender.name = "btnPalletFull" Then
                    '    txtLooseCases.Text = 0
                    'End If
                    'WO#871                    If txtLooseCases.Text <> "" Then
                    If txtLooseCases.Text <> "" AndAlso txtLooseCases.Text <> "0" Then        'WO#871
                        'If ActiveControl.Name <> "btnPalletFull" Then
                        intCasesInPallet = CType(txtLooseCases.Text, Integer)
                        'End If
                        If intCasesInPallet = 0 And sender.name <> "btnPalletFull" Then
                            MessageBox.Show("Case in pallet is required. If no loose cases, please reply YES on Zero Loose Cases.", "Missing Information")
                            e.Cancel = True
                        ElseIf intCasesInPallet < 0 Then
                            MessageBox.Show("Case on pallet can not be less than zero.", "Invalid Information")
                            e.Cancel = True
                        ElseIf intCasesInPallet > gintQtyPerPallet Then
                            MessageBox.Show("Case on pallet can not greater than the pallet quantity.", "Invalid Information")
                            e.Cancel = True
                        ElseIf intCasesInPallet < gdrSessCtl.CarriedForwardCases And gdrSessCtl.PalletsCreated = 0 Then
                            With gdrSessCtl
                                ' If entered loose cases is less then the carried forward cases and no pallet has been created in the session
                                tblSCH = SharedFunctions.GetSessionControlHst("LastRec_Line_SO_Shift", .OverridePkgLine, .ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, .Operator)
                                If tblSCH.Rows.Count > 0 Then
                                    drSCH = tblSCH.Rows(0)                  'WO#755
                                    'WO#755 If tblSCH.Rows(0).item("[Operator]") <> .Operator Then
                                    If drSCH._Operator <> .Operator Then
                                        blnWarning = True
                                    Else
                                        If tblSCH.Rows(0).Item("PalletsCreated") = 0 Then 'created pallet consumed the carried forward cases
                                            blnWarning = True
                                        End If
                                    End If
                                Else
                                    MessageBox.Show(String.Format("Loose Cases cannot less than From Carried Forward Cases ({0}).", .CarriedForwardCases), "Error Message - Loose Cases")
                                    e.Cancel = True
                                End If
                            End With
                            If blnWarning = True Then
                                drtLooseCases = MessageBox.Show(String.Format("No pallets created in the shift but Loose Cases is less than From Carried Forward Cases ({0}). Do you want to continue?", gdrSessCtl.CarriedForwardCases), "Warning Message - Loose Cases", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                If drtLooseCases = Windows.Forms.DialogResult.No Then
                                    e.Cancel = True
                                End If
                            End If
                        End If
                    Else
                        If intPalletFull = 0 Then
                            MessageBox.Show("Case in pallet is required. If no loose cases, please reply YES on Zero Loose Cases.", "Missing Information")
                            'drLooseCases = MessageBox.Show("No. of Cases on imcomplete pallet is zero or not entered. Do you want to continue?", "Warning Message - Cases On Imcomplete Pallet", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            'If drLooseCases = Windows.Forms.DialogResult.No Then
                            '    e.Cancel = True
                            'End If
                        End If
                    End If
                    If e.Cancel = True And sender.name = "btnPalletFull" Then
                        btnPalletFull.BackColor = Color.LemonChiffon
                        btnPalletNotFull.BackColor = Color.LightSeaGreen
                        intPalletFull = 0
                        txtLooseCases.Visible = True
                        lblLooseCases.Visible = True
                    End If
            End Select
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Function chkLooseCases() As Boolean
    '    Dim intQtyPerPallet As Integer
    '    chkLooseCases = True
    '    If IsDBNull(gtblSO.Rows(0).Item("QtyPerPallet")) Then
    '        intQtyPerPallet = 0
    '    Else
    '        intQtyPerPallet = gtblSO.Rows(0).Item("QtyPerPallet")
    '    End If
    '    If Me.txtLooseCases.Text > IIf(IsDBNull(gtblSO.Rows(0).Item("QtyPerPallet")), 0, gtblSO.Rows(0).Item("QtyPerPallet")) Then
    '        chkLooseCases = False
    '        MessageBox.Show(String.Format("Quantity entered is greater than full pallet quantity,{0}.", intQtyPerPallet), "Invalid information")
    '    End If
    'End Function

    Private Sub txtBagLengthUsed_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtBagLengthUsed.Validating
        If ActiveControl.Name = "btnPrvScn" Then
            Me.Close()
        Else
            If RTrim(txtBagLengthUsed.Text) <> "" Then
                Try
                    Dim sngBagLengthUsed As Single = CType(txtBagLengthUsed.Text, Single)
                    If sngBagLengthUsed = 0 Then
                        MessageBox.Show("Please enter Actual Bag Length.", "Missing Information")
                        e.Cancel = True
                    ElseIf sngBagLengthUsed < 0 Then
                        MessageBox.Show("Actual Bag Length must be greater than zero.", "Invavlid Information")
                        e.Cancel = True
                    End If
                Catch ex As Exception
                    MessageBox.Show("Please enter an valid Bag Length.", "Invalid Information")
                End Try
            End If
        End If
    End Sub

    'Private Sub txtCasesProducedInShift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        gstrErrmsg = Nothing
    '        If RTrim(lblCasesProducedInShift.Text) = "" Then
    '            gstrErrmsg = "Case produced can not be blank, put an zero if it is nothing was produced."
    '            MessageBox.Show(gstrErrmsg, "Missing information")
    '            lblCasesProducedInShift.Focus()
    '            'ElseIf CType(RTrim(txtCasesProducedInShift.Text), Single) < 0 Then
    '            '    gstrErrmsg = "Case produced can not be less than zero."
    '            '    MessageBox.Show(gstrErrmsg, "Invalid Information")
    '            '    txtCasesProducedInShift.Focus()
    '        Else
    '            'Cases remain in the shift = cases Scheduled in the shift - cases produced in the shift
    '            Me.lblCasesRemainInShift.Text = gdrSessCtl.Item("CasesScheduled") - lblCasesProducedInShift.Text
    '            'total cases produced for the shop order = total cases produced for the shop order before change + cases produced in the shift - cases produced in the shift before change
    '            gintTotalProduced = gintTotalProduced + lblCasesProducedInShift.Text - gintOriginalShiftProduced
    '            lblTotalProduced.Text = gintTotalProduced
    '            'Cases remain for the whole shop order =  cases scheduled for the shop order - total cases produced for the shop order
    '            lblTotalRemain.Text = lblTotalScheduled.Text - lblTotalProduced.Text

    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub txtCasesProduced_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            If ActiveControl.Name = "btnPrvScn" Then
                Me.Close()
            Else
                'If Not IsNothing(gstrErrmsg) AndAlso ActiveControl.Name = sender.Name Then
                If Not IsNothing(gstrErrmsg) Then
                    MessageBox.Show(gstrErrmsg, "Missing Information")
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtRework_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRework.TextChanged
        Try
            gstrErrmsg = Nothing
            If txtRework.Text <> "" Then
                If CType(RTrim(txtRework.Text), Single) < 0 Then
                    gstrErrmsg = "Rework can not be less than zero."
                    MessageBox.Show(gstrErrmsg, "Invalid Information")
                    txtRework.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtRework_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtRework.Validating
        Try
            If ActiveControl.Name = "btnPrvScn" Then
                Me.Close()
            Else
                'If Not IsNothing(gstrErrmsg) AndAlso ActiveControl.Name = sender.Name Then
                If Not IsNothing(gstrErrmsg) Then
                    MessageBox.Show(gstrErrmsg, "Invalid Information")
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class