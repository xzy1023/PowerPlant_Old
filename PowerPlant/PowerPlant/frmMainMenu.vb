Imports System.Linq
Imports System.Threading
Imports PowerPlant.LocalModels
Imports PowerPlant.ServerModels

Public Class frmMainMenu
    Public intLastCaseCount As Integer
    Private Const strDashes As String = "----"
    Private blnDisplayMessage As Boolean                             'WO#3686
    Dim _dbServer As New ServerModels.PowerPlantEntities()

    Private Sub frmMainMenu_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try                         'WO#5370
            ModifySOButton()
            'WO#755 gstrLineName = SharedFunctions.GetEquipmentDescription(gdrCmpCfg.Facility, gdrSessCtl.DefaultPkgLine)

            'WO#755 ADD Start
            lblLineName.Text = gstrLineName
            If gdrSessCtl.ShopOrder = 0 Then
                lblQtyPerPallet.Visible = False
                lblQtyPerPalletValue.Visible = False
                lblSKU.Visible = False
                ' WO#17432 ADD Start
                btnQATInProcess.Visible = False
                btnQATStartUp.Visible = False
                ' WO#17432 ADD Stop
            Else
                drSO = SharedFunctions.GetSOInfo("GetSO&Item", gdrSessCtl.Facility, gdrSessCtl.ShopOrder)

                lblQtyPerPalletValue.Text = drSO.QtyPerPallet
                lblQtyPerPallet.Visible = True
                lblQtyPerPalletValue.Visible = True
                lblSKU.Visible = True

                ' WO#17432 ADD Start

                If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.internal Then
                    btnQATInProcess.Visible = True
                    btnQATStartUp.Visible = True
                Else
                    btnQATInProcess.Visible = False
                    btnQATStartUp.Visible = False
                End If
                ' WO#17432 ADD Stop
            End If

            'WO#5370    If gblnAutoCaseCountLine = True
            If gblnAutoCountLine = True Then    'WO#5370
                If gdrSessCtl("ShopOrder") = 0 Then
                    Panel1.Visible = False
                Else
                    If lblShopOrder.Text = gdrSessCtl.ShopOrder.ToString Then
                        If blnDisplayMessage = False Then        'WO#3686
                            InitializeCaseCounters()
                        End If                                   'WO#3686
                    End If
                End If
                'WO#755 ADD Stop
            End If
            'WO#5370 ADD Start

            ' If there is no scrap reject point for the line, hide the button
            If _dbServer.tblEquipment_ScrapRejectPoint.AsQueryable.Where(Function(x) x.EquipmentID = gdrSessCtl.DefaultPkgLine).Count = 0 Then
                btnLogScrapRejectPoint.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        'WO#5370 ADD Stop

    End Sub

    Private Sub frmMainMenu_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'WO#5370    If gblnAutoCaseCountLine = True Then
        If gblnAutoCountLine = True Then        'WO#5370
            If Timer1.Enabled = True Then
                Timer1.Enabled = False
                Timer1.Stop()
            End If
        End If
    End Sub
    'WO#755 ADD Stop

    Private Sub frmMainMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim blnCnnType As Boolean
        Try
            UcHeading1.ScreenTitle = "Main Menu"

            'WO#755 ADD Start
            gstrLineName = SharedFunctions.GetEquipmentDescription(gdrCmpCfg.Facility, gdrSessCtl.DefaultPkgLine)
            If gstrLineName = String.Empty Then
                gstrLineName = "The default packaging Line " & RTrim(gdrSessCtl.DefaultPkgLine) & " is invalid."
                lblLineName.Text = "Line: Undefined"
                lblMessage.Text = "Please Contact Supervisor."
                MessageBox.Show(gstrLineName & ", Please contact supervisor.", "*** Packaging Line Error ***", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            Else
                gstrLineName = "Line: " & gstrLineName
            End If

            lblLineName.Text = gstrLineName

            Panel1.Visible = False
            'WO#755 ADD Stop

            'ModifySOButton()
            ''If shop order has not started, check the server connection by connecting to it.
            ''else check the flag from the session control table.
            If gdrSessCtl("ShopOrder") = 0 Then
                lblSKU.Visible = False
                lblQtyPerPallet.Visible = False
                lblQtyPerPalletValue.Visible = False
                blnCnnType = gblnTryConnect
            Else
                lblSKU.Visible = True
                lblQtyPerPallet.Visible = True
                lblQtyPerPalletValue.Visible = True
                blnCnnType = gblnTrySessCtl
                'WO#755 ADD Start
                'WO#5370    If gblnAutoCaseCountLine = True Then
                If gblnAutoCountLine = True Then    'WO#5370
                    txtSOProduced.Text = strDashes
                    txtShiftProduced.Text = strDashes
                    txtCaseCounterInPallet.Text = strDashes
                    Panel1.Visible = True
                    InitializeCaseCounters()
                End If
                'WO#755 ADD Stop
                ' WO#17432 ADD Start
                If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.internal Then
                    btnQATInProcess.Visible = True
                    btnQATStartUp.Visible = True
                Else
                    btnQATInProcess.Visible = False
                    btnQATStartUp.Visible = False
                End If
                ' WO#17432 ADD Stop
            End If

            SharedFunctions.IsSvrConnOK(blnCnnType, Me)
            'If SharedFunctions.IsSvrConnOK(blnCnnType) = False Then
            '    SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            'Else
            '    SharedFunctions.RmvMessageLineFromForm(Me)
            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnShopOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShopOrder.Click
        Try
            If gdrSessCtl("ShopOrder") = 0 Then
                frmStartShopOrder.ShowDialog()
            Else
                frmStopShopOrder.ShowDialog()
            End If
            ModifySOButton()
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDownTime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownTime.Click
        'WO#650 Dim strCmdArgs As String = Nothing
        'Dim strAppName As String = Nothing
        Try
            'If SharedFunctions.IsProcessActive("DownTime") Then
            'SharedFunctions.WakeUpAPgm("DownTime")
            'Else
            SharedFunctions.StartDownTimePgm(gdrSessCtl("DefaultPkgLine"), lblShopOrder.Text.ToString, gdrSessCtl.Operator,
                    gdrSessCtl("OverrideShiftNo"), gdrSessCtl("Facility"), gdrCmpCfg("PkgLineType"))    'WO#650 
            'End If

            'WO#650 DEL Start
            'Dim strSessionStartTime As String = Nothing

            'Dim sb As New System.Text.StringBuilder

            'sb.AppendFormat("{0} {1} {2} {3} {4} {5} {6}", "P", gdrSessCtl("DefaultPkgLine"), lblShopOrder.Text.ToString, gdrSessCtl.Operator, _
            '        gdrSessCtl("OverrideShiftNo"), gdrSessCtl("Facility"), gdrCmpCfg("PkgLineType"))

            'If gdrSessCtl.ShopOrder <> 0 Then
            '    sb.Append(" ")
            '    sb.Append(gdrSessCtl.StartTime.ToString("yyyy-MM-ddHH:mm:ss.fff"))
            'End If

            'strCmdArgs = sb.ToString

            ''strCmdArgs = "P " & gdrSessCtl("DefaultPkgLine") & " " & lblShopOrder.Text & " " & gdrSessCtl.Operator & _
            ''            " " & gdrSessCtl("OverrideShiftNo") & " " & gdrSessCtl("Facility") & " " & gdrCmpCfg("PkgLineType") & " " & strSessionStartTime

            'Process.Start(My.Settings.strDownTime, strCmdArgs)
            'WO#650 DEL Stop

            'strAppName = My.Settings.strDownTime & " " & strCmdArgs
            'Dim processID As Integer
            'processID = Shell(strAppName, AppWinStyle.Hide)

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Unexpected Application Error")
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnLogOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogOut.Click
        Dim strMsg As String
        Dim dgrSSO As DialogResult
        If gdrSessCtl.ShopOrder <> 0 Then
            strMsg = "Please Stop the current shop order before log out."
            MessageBox.Show(strMsg, "Warning! Shop Order is Still Active", MessageBoxButtons.OK)

            dgrSSO = frmStopShopOrder.ShowDialog()
            If dgrSSO = Windows.Forms.DialogResult.OK Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If

    End Sub

    Private Sub btnInquiry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInquiry.Click
        Try
            frmInquiry.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

    Private Sub btnCaseLabel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaseLabel.Click
        Try
            If gblnSvrConnIsUp = True Then
                If gdrSessCtl("ShopOrder") <> 0 Then
                    If gblnStartSOWithNoLabel = False Then  'WO#654
                        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                        gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), "")
                        If tblSO.Rows.Count > 0 Then
                            drSO = tblSO.Rows(0)   'WO#650
                            If tblSO.Rows(0)("PrintCaseLabel") <> "Y" Then
                                MessageBox.Show("This item will not produce case label.", "For Your Information")
                            Else
                                'WO#650 ADD Start
                                If gblnOvrExpDate = True AndAlso (drSO.DateToPrintFlag = "1" Or drSO.DateToPrintFlag = "3") Then
                                    frmExpiryDate.ShowDialog()
                                End If
                                'WO#650 ADD Stop
                                SharedFunctions.printCaseLabel(CASELABEL, tblSO.Rows(0)("LotID"))
                            End If
                        Else
                            MessageBox.Show("Shop order is not found, can not print case lable.", "Invalid Information")
                        End If
                    Else    'WO#654
                        MessageBox.Show("Shop order was started with 'NO CASE LABEL', Print Case Label is not allowed.", "Print Case Label is Prohibited.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)   'WO#654
                    End If  'WO#654
                Else
                    MessageBox.Show("Please start a shop order before print case label.", "Invalid Selection")
                End If
            Else
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

    Private Sub btnCreatePallet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreatePallet.Click
        Try
            If gdrSessCtl("ShopOrder") <> 0 Then
                'WO#654 ADD Start
                'When the shop order was started with no case label loaded, the gblnStartSOWithNoLabel is set to True.
                'So even Indusoft program requested to create a pallet, ignore it.
                If gblnStartSOWithNoLabel = True Then
                    MessageBox.Show("Shop order was started with 'NO CASE LABEL', pallet creation is not allowed.", "Create Pallet is Prohibited.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    'WO#654 ADD Stop
                    frmCreatePallet.ShowDialog()
                    If gblnSvrConnIsUp = False Then
                        SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                    End If
                End If  'WO#654
            Else
                MessageBox.Show("Please start a shop order before create pallet.", "Invalid Selection")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

    Private Sub btnPrinterControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrinterControl.Click
        Try
            frmPrtLabelsAndControl.ShowDialog()
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

    Private Sub ModifySOButton()
        Try
            Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
            lblShopOrder.Text = 0
            lblItemNo.Text = ""
            lblItemDesc.Visible = False
            lblTieTier.Visible = False
            lblTieTierValues.Text = ""
            lblLabelWeight.Visible = False
            lblLabelWeightValue.Visible = False
            lblLabelWeightValue.Text = ""

            If gdrSessCtl("ShopOrder") = 0 Then
                btnShopOrder.Text = "Start Shop Order"
                btnShopOrder.BackColor = Color.OrangeRed
                btnLogScrap.Visible = False
                btnLogScrapRejectPoint.Visible = False
            Else
                btnShopOrder.Text = "Stop Shop Order"
                btnShopOrder.BackColor = Color.Lime
                btnLogScrap.Visible = True
                btnLogScrapRejectPoint.Visible = True
                gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), gdrSessCtl("DefaultPkgLine"))
                If tblSO.Rows.Count > 0 Then
                    With tblSO.Rows(0)
                        lblShopOrder.Text = gdrSessCtl("ShopOrder")
                        lblItemNo.Text = .Item("ItemNumber")
                        lblItemDesc.Visible = True
                        lblItemDesc.Text = RTrim(.Item("ItemDesc1")) & " " & RTrim(.Item("ItemDesc2")) & " " & RTrim(.Item("PackSize"))
                        lblTieTierValues.Text = RTrim(CType(CType(.Item("Tie"), Integer), String)) & " x " & RTrim(CType(CType(.Item("Tier"), Integer), String))
                        lblTieTier.Visible = True
                        lblLabelWeight.Visible = True
                        lblLabelWeightValue.Visible = True
                        lblLabelWeightValue.Text = String.Format("{0:D} g.", FormatNumber(.Item("LabelWeight"), 2))
                    End With
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

    Private Sub btnLogScrap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogScrap.Click
        Try
            If gdrSessCtl("ShopOrder") <> 0 Then
                frmLogScraps.ShowDialog()
                If gblnSvrConnIsUp = False Then
                    SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                End If
            Else
                MessageBox.Show("Please start a shop order before log scrap.", "Invalid Selection")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim frmCollection = System.Windows.Forms.Application.OpenForms  'WO#3686
        Dim drtCrtPallet As DialogResult                                'WO#3686
        Dim sb As New System.Text.StringBuilder                         'WO#3686


        Try
            If SharedFunctions.IsSvrConnOK(gblnTrySessCtl, Me) Then
                With drSO
                    If .ShopOrder > 0 AndAlso Not IsNothing(CaseCounter) Then
                        If CaseCounter.MessageCode <> 1 Then                'WO#3686
                            If intLastCaseCount <> CaseCounter.CasesProducedRunningTotal Or CaseCounter.RefreshMainMenu = True Then
                                CaseCounter.RefreshMainMenu = False
                                lblSOScheduled.Text = CType(.OrderQty, Integer).ToString
                                txtCaseCounterInPallet.Text = CaseCounter.CaseCountInPallet
                                txtSOProduced.Text = CaseCounter.CasesProducedRunningTotal

                                txtShiftProduced.Text = CaseCounter.CasesProducedInShift

                                Me.Refresh()
                                intLastCaseCount = CaseCounter.CasesProducedRunningTotal
                            End If
                            'WO#3686 ADD Start
                        Else
                            If blnDisplayMessage = False Then
                                blnDisplayMessage = True
                                Timer1.Stop()
                                Timer1.Enabled = False
                                sb.Length = 0
                                'Message code 1 means cases in pallet are 2 times more than the case per pallet of the item.
                                If CaseCounter.MessageCode = 1 Then
                                    sb.AppendFormat("Cases-in-Pallet is {0}. It is 2 times or more than the Cases-per-Pallet.", CaseCounter.CaseCountInPallet)
                                    sb.AppendLine("")
                                    sb.AppendFormat("Cases from created pallets are {0}.", CaseCounter.CasesFromCreatedPallets)
                                    sb.AppendLine("")
                                    sb.AppendFormat("Case count in MESPIC is {0}.", CaseCounter.CasesProducedRunningTotal)
                                    sb.AppendLine("")
                                    sb.AppendLine("")
                                    sb.AppendLine("Yes - Continue to create all outstanding pallets;")
                                    sb.AppendLine("No  - Stop Shop order and contact Supervisor to reload from Mespic or adjust case count in Power Plant system.")
                                    drtCrtPallet = SharedFunctions.PoPUpMSG(sb.ToString, "Warning On Auto Pallet Creation", MessageBoxButtons.YesNo)
                                    If drtCrtPallet = DialogResult.No Then
                                        CaseCounter.CrtPallet = False
                                        If Not IsNothing(frmCollection.item("frmStopShopOrder")) Then
                                            frmCollection.Item("frmStopShopOrder").Activate()
                                        Else
                                            btnShopOrder_Click(Me, e)
                                        End If

                                    Else
                                        CaseCounter.CrtPallet = True
                                        CaseCounter.MessageCode = 0

                                    End If
                                    blnDisplayMessage = False
                                    Timer1.Enabled = True
                                End If
                            End If
                        End If
                        'WO#3686 ADD Stop
                    End If
                End With
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    'WO#755 ADD Start
    Private Sub InitializeCaseCounters()
        Try

            Panel1.Visible = True

            If IsNothing(drSO) AndAlso gdrSessCtl.ShopOrder <> 0 Then
                drSO = SharedFunctions.GetSOInfo("GetSO&Item", gdrSessCtl.Facility, gdrSessCtl.ShopOrder)
                lblQtyPerPalletValue.Text = drSO.QtyPerPallet
            End If

            If IsNothing(CaseCounter) Then
                'FIX20161224    CaseCounter = New Counter(gdrSessCtl.Facility, drSO.ShopOrder, drSO.QtyPerPallet, "Started", _
                'FIX20161224               gdrSessCtl.LogOnTime, gdrSessCtl.OverrideShiftNo, gdrSessCtl.Operator)
                CaseCounter = New Counter(gdrSessCtl.Facility, drSO.ShopOrder, drSO.QtyPerPallet, "Started",
                                                                      gdrSessCtl.LogOnTime, gdrSessCtl.OverrideShiftNo, gdrSessCtl.Operator, gdrSessCtl.DefaultPkgLine,
                            gdrSessCtl.ShiftProductionDate, gdrCmpCfg.InterfaceType) 'WO#5370
                'WO#5370        gdrSessCtl.LogOnTime, gdrSessCtl.OverrideShiftNo, gdrSessCtl.Operator, gdrSessCtl.DefaultPkgLine, gdrSessCtl.ShiftProductionDate)  'WO#3686
                'WO#3686    gdrSessCtl.LogOnTime, gdrSessCtl.OverrideShiftNo, gdrSessCtl.Operator, gdrSessCtl.DefaultPkgLine)       'FIX20161224
            End If
            If CaseCounter.CounterIsStarted = False Then
                CaseCounter.Start(True)
                lblSOScheduled.Text = CType(drSO.OrderQty, Integer).ToString
                'txtSOProduced.Text = CaseCounter.InitialSOCasesProduced
                'txtShiftProduced.Text = CaseCounter.InitialShiftCasesProduced
                'txtCaseCounterInPallet.Text = CaseCounter.InitialCaseCountInPallet
                txtSOProduced.Text = strDashes
                txtShiftProduced.Text = strDashes
                txtCaseCounterInPallet.Text = strDashes
            End If

            If Timer1.Enabled = False Then
                'Initialize the count from the table 
                Timer1.Interval = My.Settings.gintMainScreenTimerInterval
                Timer1.Enabled = True
                Timer1.Start()
            End If

        Catch ex As Exception
            Throw New Exception("Error in InitializeCaseCounters" & vbCrLf & ex.Message)
        End Try
    End Sub
    'WO#755 ADD Stop
    'WO#17432 ADD Start
    Private Sub btnQATInProcess_Click(sender As System.Object, e As System.EventArgs) Handles btnQATInProcess.Click
        Try
            With frmQATWorkflow
                .ScreenTitle = "In-Process QA Tests"
                frmQATWorkflow.QATEntryPoint = "I"
                frmQATWorkflow.ShowDialog()
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnQATTest_Click(sender As Object, e As EventArgs) Handles btnQATStartUp.Click
        Try
            'SharedFunctions.QATProcessWorkFlow("S")
            With frmQATWorkflow
                .ScreenTitle = "Start-up QA Tests"
                frmQATWorkflow.QATEntryPoint = "S"
                frmQATWorkflow.ShowDialog()
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    'WO#17432 ADD Stop

    Private Sub btnLogScrapRejectPoint_Click(sender As Object, e As EventArgs) Handles btnLogScrapRejectPoint.Click
        Try
            If gdrSessCtl("ShopOrder") <> 0 Then
                frmLogScrapsRejectPoint.ShowDialog()
                If gblnSvrConnIsUp = False Then
                    SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                End If
            Else
                MessageBox.Show("Please start a shop order before log scrap reject point.", "Invalid Selection")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unexpected Application Error")
        End Try
    End Sub

End Class
