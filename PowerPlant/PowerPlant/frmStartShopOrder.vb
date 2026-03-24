Public Class frmStartShopOrder
    Dim gshtCurrentShift As Short
    Dim gshtShiftOnLastSOStarted As Short
    Dim gstrPrintCaseLabel As String
    Dim gstrLotID As String = ""
    Dim gstrBagLengthRequired As String
    Dim gstrErrMsg As String
    Dim gtblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable

    Dim gintSODftWidth As Integer
    Dim gintShopOrder As Integer
    'Dim gblnAjaxPlant As Boolean
    Dim gblnInitialLoad As Boolean
    Dim gfntSOdefaultFont As New Font("Arial", 18, System.Drawing.FontStyle.Regular)
    'WO#871 Dim gstrMyComputerName As String    'WO#718

    Private Sub frmStartShopOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim intRtnCde As Integer
        'WO#718 Dim strMyComputerName As String = My.Computer.Name
        Dim strErrMsg As String

        Try
            'WO#871 gstrMyComputerName = My.Computer.Name   'WO#718
            Me.WindowState = FormWindowState.Maximized
            Me.Cursor = Cursors.WaitCursor
            UcHeading1.ScreenTitle = "START SHOP ORDER"
            gblnInitialLoad = True
            SharedFunctions.ClearInputFields(Me)

            'If RTrim(gdrCmpCfg("Facility")) = "02" Or RTrim(gdrCmpCfg("Facility")) = "20" Then
            If gblnAjaxPlant = True Then
                gintSODftWidth = 625
            Else
                'gblnAjaxPlant = False
                gintSODftWidth = Me.cboShopOrder.Size.Width
            End If

            Me.lblOperator.Visible = False

            'Check network connect by try to connect to the server
            SharedFunctions.IsSvrConnOK(gblnTryConnect, Me)

            Try

                cboShopOrder.Text = ""
                'gshtShiftOnLastSOStarted = gdrSessCtl("OverrideShiftNo")

                btnStartWithNoLabel.Visible = gbln2SOIn1Line 'WO#651

                'import data from the staging area into the local data base if it is ready
                'WO#718     SharedFunctions.ImportMasterTables(strMyComputerName)   
                'WO#871 SharedFunctions.ImportMasterTables(gstrMyComputerName)      'WO#718

                'If the network connection is fine then upload the transactions that were 
                'temporary stored in the local data base during the network failure to the server if any
                If gblnSvrConnIsUp = True Then

                    'Upload the session control history records from local, if any ,to server
                    SharedFunctions.uploadSessCtlHstToServer()
                    SharedFunctions.uploadPalletToServer()
                    SharedFunctions.uploadLogScrapToServer()
                    SharedFunctions.uploadLocalDataToServer("tblWeightLog")
                    SharedFunctions.uploadLocalDataToServer("tblPLCLog")
                    SharedFunctions.UploadToBeLoadedShopOrderToServer()        'WO#297
                    SharedFunctions.uploadOperationStaffingToServer()
                    'WO#17432 ADD Start
                    If gdrCmpCfg.QATWorkFlowInitiation <> QATWorkFlow.Disabled Then
                        SharedFunctions.uploadLocalDataToServer("tblQATCameraResult")
                        SharedFunctions.uploadLocalDataToServer("tblQATCartonboxResult")
                        SharedFunctions.uploadLocalDataToServer("tblQATCaseVisualResult")
                        SharedFunctions.uploadLocalDataToServer("tblQATPalletTypeResult")
                        ' WO#17432 ADD Start – AT 11/06/2018

                        'SharedFunctions.UploadQATLineClearanceHeaderLocalDataToServer("tblQATLineClearanceResultHeader")
                        'SharedFunctions.uploadLocalDataToServer("tblQATLineClearanceResultDetail")

                        SharedFunctions.UploadQATStartupHeaderLocalDataToServer("tblQATStartupResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATStartupResultDetail")

                        SharedFunctions.UploadQATWeightHeaderLocalDataToServer("tblQATWeightResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATWeightResultDetail")

                        SharedFunctions.UploadQATMaterialsValidationHeaderLocalDataToServer("tblQATMaterialsResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATMaterialsResultDetail")

                        SharedFunctions.UploadQATOxygenHeaderLocalDataToServer("tblQATOxygenResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATOxygenResultDetail")

                        SharedFunctions.UploadQATPressureHeaderLocalDataToServer("tblQATPressureResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATPressureResultDetail")

                        SharedFunctions.UploadQATCheckWeigherHeaderLocalDataToServer("tblQATCheckWeigherResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATCheckWeigherResultDetail")

                        SharedFunctions.UploadQATDateCodeHeaderLocalDataToServer("tblQATDateCodeResultHeader")
                        SharedFunctions.uploadLocalDataToServer("tblQATDateCodeResultDetail")

                        SharedFunctions.uploadLocalDataToServer("tblQATOverride")

                        ' WO#17432 ADD Stop – AT 11/06/2018
                    End If
                    'WO#17432 ADD Stop
                End If

                'WO#871 ADD start
                'import data from the staging area into the local data base if it is ready
                If gdrSessCtl.ShopOrder = 0 Then
                    SharedFunctions.ImportMasterTables(gstrMyComputerName)
                End If
                'WO#718 ADD Stop

                ' Find the default Shop Order Number to fill the Shop Order Drop Down box

                'Get last record of the desired packaging line(the assoicated or overrode line) from the session control history for default shop order no.
                'WO#755 intRtnCde = gtaSCH.Fill(gtblSCH, "SelectLastRecord", gdrSessCtl.DefaultPkgLine, 0, 0, Now, New System.Nullable(Of Date), gdrSessCtl.Facility, String.Empty)
                gtblSCH = SharedFunctions.GetSessionControlHst("SelectLastRecord", gdrSessCtl.DefaultPkgLine, 0, 0, Now, Now, gdrSessCtl.Facility, String.Empty) 'WO#755
            Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231)
                SharedFunctions.SetServerCnnStatusInSessCtl(False)
            Catch ex As Exception
                Throw ex
            End Try

            If gtblSCH.Rows.Count > 0 Then
                'Retrieve Shop Order record using the Shop Order from the Session Control History
                intRtnCde = gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gtblSCH.Rows(0).Item("ShopOrder"), "")
                With gtblSCH.Rows(0)
                    'get Utility staffs based on the Default Pkg Line and Start Time from Session Control History
                    FillUtilityTech(.Item("DefaultPkgLine"), .Item("StartTime"))
                End With
                If tblSO.Rows.Count > 0 AndAlso IsDBNull(tblSO.Rows(0).Item("Closed")) Then
                    'If the Shop Order in the session history is still open, use it & SO record to load SO data
                    drSO = tblSO.Rows(0)
                    'cboShopOrder.Text = tblSO.Rows(0).Item("ShopOrder")
                Else
                    'Select next Shop Order based on the scheduled time from the Shop Order table if SO is closed.
                    GetNextSOInfo()
                End If
            Else
                'Get next SO based on the scheduled time from the Shop Order table if Session History is not found or server connection is down.
                GetNextSOInfo()
            End If

            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If
            ' Fill the default values from sesscion control record
            With gdrSessCtl
                '    txtPkgLine.Text = .OverridePkgLine
                '    Dim objShift As New WorkShift                                                   'WO#2645
                '    objShift.GetExpectedShiftInfoByTime(Now(), gdrCmpCfg("WorkShiftType"), True)    'WO#2645
                '    gshtCurrentShift = objShift.Shift                                               'WO#2645
                '    'WO#2645    gshtCurrentShift = New WorkShift(Now, gdrCmpCfg("WorkShiftType")).Shift    
                '    txtShift.Text = .OverrideShiftNo
                Dim objShift As New WorkShift                                                   'WO#5370
                objShift.GetExpectedShiftInfoByTime(Now(), gdrCmpCfg("WorkShiftType"), True)    'WO#5370
                gshtCurrentShift = objShift.Shift                                               'WO#5370
                txtOperator.Text = .Operator
                txtPkgLine.Text = .OverridePkgLine                                              'WO#5370
            End With

            If Not IsNothing(drSO) Then
                'Save the error message and display it after the screen is shown.
                gstrErrMsg = String.Empty
                cboShopOrder.Text = drSO.ShopOrder 'This will trigger the text changed event
                strErrMsg = gstrErrMsg
                If gstrErrMsg = String.Empty Then           'WO#817
                    cboShopOrder.SelectedValue = drSO.ShopOrder
                    LoadDataToScreen(drSO)
                    'restore the error message created after shop order text changed event
                    If strErrMsg <> String.Empty Then
                        gstrErrMsg = strErrMsg
                    End If
                End If                                      'WO#817
            End If

            gblnInitialLoad = False

        Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            Throw New Exception("Error in frmStartShopOrder_Load" & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Private Sub LoadSOInfo(ByVal drShopOrder As dsShopOrder.CPPsp_ShopOrderIORow)
        Try
            If Not IsNothing(drShopOrder) Then                  'WO#5370
                With drShopOrder
                    lblItemNo.Text = .Item("ItemNumber")
                    lblItemDesc.Text = .Item("ItemDesc1")
                    gstrLotID = .Item("LotID")
                    lblUnitPerCase.Text = CType(.Item("SaleableUnitPerCase"), String)
                    lblLabelWeight.Text = FormatNumber(CType(.Item("LabelWeight"), Single), 2)

                    If .Item("BagLengthRequired") = "Y" Then
                        lblBagLength.Visible = True
                        lblStdBagLength.Visible = True
                        lblActBagLength.Visible = True
                        txtBagLengthUsed.Visible = True
                        txtBagLengthUsed.Text = .Item("Baglength")
                        lblStdBagLength.Text = .Item("BagLength")
                    Else
                        lblBagLength.Visible = False
                        lblStdBagLength.Visible = False
                        lblActBagLength.Visible = False
                        txtBagLengthUsed.Visible = False
                    End If

                    lblTotalScheduled.Text = CType(.Item("OrderQty"), Integer)

                End With
            End If                      'WO#5370
            ' Pre-fill the packaging line, shift,txtOperator, utility techs and BagLengthUsed 
            ' only in the first load.

            If gblnInitialLoad = True Then
                With gdrSessCtl
                    If Not IsDBNull(.Item("OverridePkgLine")) Then
                        Me.txtPkgLine.Text = .Item("OverridePkgLine")
                    End If

                    If Not IsDBNull(.Item("OverrideShiftNo")) And txtShift.Text = "" Then
                        txtShift.Text = .Item("OverrideShiftNo")
                    End If

                End With
                'For operator and utility technicians,if shift is different from the previous session, 
                'get operator from current session and do not fill the utility technicians 
                'else default from previous session
                If gtblSCH.Rows.Count > 0 Then
                    With gtblSCH.Rows(0)
                        If gdrSessCtl.Item("OverrideShiftNo") <> gtblSCH.Rows(0).Item("OverrideShiftNo") Then
                            txtOperator.Text = gdrSessCtl.Operator
                            If .Item("StopTime") > gdrSessCtl("LogOnTime") Then
                                FillUtilityTech(.Item("DefaultPkgLine"), .Item("StartTime"))
                            End If
                        Else
                            If Not IsDBNull(.Item("StopTime")) And .Item("StopTime") > gdrSessCtl("LogOnTime") Then
                                txtPkgLine.Text = .Item("OverridePkgLine")
                                If gdrSessCtl.Operator = String.Empty Then
                                    txtOperator.Text = .Item("[Operator]")
                                End If
                            End If
                            'get Utility staffs based on the Default Pkg Line and Start Time from Session Control History
                            FillUtilityTech(.Item("DefaultPkgLine"), .Item("StartTime"))
                        End If

                        If .Item("BagLengthUsed") > 0 Then
                            txtBagLengthUsed.Text = .Item("BagLengthUsed")
                        End If
                    End With
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in LoadSOInfo" & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
                    txtShift.MouseDown, txtBagLengthUsed.MouseDown, txtOperator.MouseDown, txtCasesScheduledInShift.MouseDown, _
                    txtUtilityTech1.MouseDown, txtUtilityTech2.MouseDown, txtUtilityTech3.MouseDown, txtUtilityTech4.MouseDown, txtCFCases.MouseDown
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
    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown, cboShopOrder.MouseDown
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

    Private Sub btnPrvScn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub LoadDataToScreen(ByVal drSOItem As dsShopOrder.CPPsp_ShopOrderIORow)

        Try
            LoadSOInfo(drSOItem)
            'Find the carried forward cases
            With gdrSessCtl
                txtCFCases.Text = SharedFunctions.GetCarriedForwardCases(.OverridePkgLine, drSOItem.ShopOrder, .OverrideShiftNo, .ShiftProductionDate, Now, .Facility, .Operator)
            End With
            CalculatePackingProgress(drSOItem)

        Catch ex As Exception
            Throw New Exception("Error in LoadDataToScreen" & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Sub CalculatePackingProgress(ByVal drSOItem As dsShopOrder.CPPsp_ShopOrderIORow)

        'WO#2645    Dim myShift As New WorkShift(Now, gdrCmpCfg("WorkShiftTYpe"))
        Dim myShift As New WorkShift        'WO#2645
        Dim intCapacity As Integer
        Dim sngPkgRate As Single
        Dim intCasesScheduledInShift As Integer
        Dim intCasesProducedInShift As Integer
        Dim intLooseCases As Integer
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        'Dim tblSCH2 As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable

        With drSOItem

            'Clear all the data from the data set table
            'tblSCH.Clear()
            'tblSCH = SharedFunctions.GetSectionControlHst("FindCarriedForwardCases", gdrSessCtl("DefaultPkgLine"), .Item("ShopOrder"), txtShift.Text, _
            '                    SharedFunctions.GetProductionDateByShift(txtShift.Text), Now(), gdrSessCtl.Facility, txtOperator.Text)
            'txtCFCases.Text = String.Empty
            'If tblSCH.Rows.Count > 0 Then
            '    txtCFCases.Text = tblSCH.Rows(0).Item("LooseCases")
            'End If

            If .Item("FinishedQty") >= .Item("OrderQty") Then
                intCasesScheduledInShift = 0
                lblCasesProducedInShift.Text = 0   'S.O. Shift total
                lblTotalProduced.Text = CType(.Item("FinishedQty"), Integer) 'S.O. total
                lblTotalRemain.Text = 0
            Else
                'Get cases produced of the whole shop order from pallet and pallet history tables.

                If txtShift.Text = "" Then
                    txtShift.Text = gdrSessCtl.OverrideShiftNo
                End If
                'lblTotalProduced.Text = CType(.Item("FinishedQty"), Integer) + SharedFunctions.GetSOCasesProducedFromPallet(.Item("ShopOrder")) + _
                lblTotalProduced.Text = SharedFunctions.GetSOCasesProducedFromPallet(.Item("ShopOrder")) + _
                            SharedFunctions.GetLooseCases(gdrSessCtl.OverridePkgLine, .ShopOrder, txtShift.Text, Now, gdrSessCtl.Facility, gdrSessCtl.Operator)
                'SharedFunctions.GetCarriedForwardCases(gdrSessCtl.OverridePkgLine, .Item("ShopOrder"), CType(txtShift.Text, Integer), Now, gdrSessCtl.Facility, gdrSessCtl.Operator)
                lblTotalRemain.Text = CType(.Item("OrderQty"), Integer) - CType(lblTotalProduced.Text, Integer)

                'Get the packing capacity in the shift based on the packing rate of the line of the shop order.
                sngPkgRate = .Item("PkgRate")
                'If DateDiff(DateInterval.Second, .Item("StartDateTime"), .Item("EndDateTime")) = 0 Then
                '    intCapacity = CType(.Item("OrderQty"), Integer)
                'Else
                If sngPkgRate = 0 Then
                    intCapacity = 0
                Else
                    'sngPkgRate = CType(.Item("OrderQty"), Integer) / DateDiff(DateInterval.Second, .Item("StartDateTime"), .Item("EndDateTime"))
                    Try
                        myShift.GetExpectedShiftInfoByTime(Now, gdrCmpCfg("WorkShiftTYpe"), True)
                        'WO#2645     If Not IsNothing(myShift) Then
                        If Not IsNothing(myShift.Shift) Then                'WO#2645
                            'intCapacity = sngPkgRate * DateDiff(DateInterval.Second, Now, SharedFunctions.getShiftDetail(Now, gdrCmpCfg("WorkShiftTYpe")).Item("ToTime"))
                            intCapacity = Math.Round(sngPkgRate * DateDiff(DateInterval.Second, Now, myShift.ToTime))
                        Else
                            intCapacity = 0
                        End If
                    Catch ex As Exception
                        intCapacity = 0
                    End Try
                End If

                'Has the SO been packed by the line before? Find last record of the shop order
                tblSCH.Clear()
                tblSCH = SharedFunctions.GetSessionControlHst("SelectLastRecByLineSO", gdrSessCtl("DefaultPkgLine"), .Item("ShopOrder"), 0, Now, Now, gdrSessCtl.Facility, String.Empty)
                'S.O. has not been packed before in this line. 
                If tblSCH.Rows.Count = 0 Then
                    If intCapacity >= lblTotalRemain.Text Then
                        intCasesScheduledInShift = lblTotalRemain.Text
                    Else
                        intCasesScheduledInShift = intCapacity
                    End If
                    Me.lblCasesProducedInShift.Text = 0
                Else
                    'S.O. has been packed before in this line. 

                    'get the session control history records by line,shoporder, shift and production date to calculate 
                    'case produced for the shop order in the shift
                    intCasesProducedInShift = SharedFunctions.GetSOCasesProducedFromPallet(.ShopOrder, gdrSessCtl.OverrideShiftNo)

                    'Clear all the data from the data set table
                    tblSCH.Clear()
                    'Has the SO been stopped in this shift? 
                    tblSCH = SharedFunctions.GetSessionControlHst("LastRec_Line_SO_Shift", gdrSessCtl("DefaultPkgLine"), .Item("ShopOrder"), txtShift.Text, _
                                    SharedFunctions.GetProductionDateByShift(txtShift.Text, Now()), Now(), gdrSessCtl.Facility, String.Empty)
                    'If the SO has not been stopped in this shift, recalculate the Cases schecdule in shift
                    If tblSCH.Rows.Count = 0 Then
                        'If intCasesProducedInShift = 0 Then
                        If lblTotalRemain.Text <= 0 Then
                            intCasesScheduledInShift = 0
                            lblCasesProducedInShift.Text = 0
                        Else
                            If intCapacity >= lblTotalRemain.Text Then
                                intCasesScheduledInShift = lblTotalRemain.Text
                            Else
                                intCasesScheduledInShift = intCapacity
                            End If
                            lblCasesProducedInShift.Text = 0
                        End If
                    Else
                        'the SO has been stopped in this shift before

                        ''intCasesScheduledInShift = tblSCH.Rows(0)("CasesScheduled")
                        ''If Cases Scheduled in shift > cases shop order remain for packing,
                        ''set Cases Scheduled in shift = cases shop order remain for packing
                        ''If intCasesScheduledInShift > lblTotalRemain.Text Then
                        ''    intCasesScheduledInShift = lblTotalRemain.Text
                        ''End If

                        ''lblTotalProduced.Text = lblTotalProduced.Text + tblSCH.Rows(0)("LooseCases")
                        ''Calculate cases produced by shift
                        ''For Each dr In tblSCH.Rows
                        ''    intCasesProducedInShift = intCasesProducedInShift + dr.CasesProduced
                        ''    intLooseCases = dr.LooseCases
                        ''Next

                        'Cases produced in the shift = Sum of Cases produced from the pallets in the shift + loose cases recorded in the last stop shop order in the shift 
                        '                               - carried forward cases recorded when the shop order was started at the first time in the shift.
                        intCasesScheduledInShift = gdrSessCtl.CasesScheduled
                        intLooseCases = SharedFunctions.GetLooseCases(gdrSessCtl.OverridePkgLine, .ShopOrder, CType(txtShift.Text, Short), Now, gdrSessCtl.Facility, gdrSessCtl.Operator)

                        'If the shift is different from current, use the one from input data else use the carried forward cases of the shop order from the first record of shift change
                        'WO#2645 ADD Start
                        Dim wsShiftFromLastSession As New WorkShift
                        wsShiftFromLastSession.GetShiftInfoByShiftNo(tblSCH.Rows(0).Item("OverrideshiftNo"), tblSCH.Rows(0).Item("StartTime"), gdrCmpCfg("WorkShiftType"), True)
                        Dim wsEnteredShiftAsOfNow As New WorkShift
                        wsEnteredShiftAsOfNow.GetShiftInfoByShiftNo(txtShift.Text, Now, gdrCmpCfg("WorkShiftType"), True)
                        If wsShiftFromLastSession.FromTime = wsEnteredShiftAsOfNow.FromTime Then
                            'WO#2645 ADD Stop
                            'WO#2645    If New WorkShift(tblSCH.Rows(0).Item("OverrideshiftNo"), tblSCH.Rows(0).Item("StartTime"), gdrCmpCfg("WorkShiftType")).FromTime = New WorkShift(txtShift.Text, Now, gdrCmpCfg("WorkShiftType")).FromTime Then
                            lblCasesProducedInShift.Text = intCasesProducedInShift + intLooseCases - SharedFunctions.CarriedForwardCasesFromShift(gdrSessCtl.OverridePkgLine, .ShopOrder, CType(txtShift.Text, Short), gdrSessCtl.ShiftProductionDate, Now, gdrSessCtl.Facility, String.Empty)
                        Else
                            lblCasesProducedInShift.Text = intCasesProducedInShift + intLooseCases - txtCFCases.Text
                        End If

                        'SharedFunctions.CarriedForwardCasesFromShift(gdrSessCtl.DefaultPkgLine, gdrSO.ShopOrder, txtShift.Text, Now, gdrSessCtl.Facility, txtOperator.Text)

                    End If
                End If
                'Cases Remain in shift = cases scheduled in shift - cases produced in shift
                Me.lblCasesRemainInShift.Text = intCasesScheduledInShift - Me.lblCasesProducedInShift.Text
                If intCasesScheduledInShift <= 0 Then
                    txtCasesScheduledInShift.Text = ""
                Else
                    txtCasesScheduledInShift.Text = intCasesScheduledInShift
                End If

            End If
        End With
    End Sub
    Private Sub FillUtilityTech(ByVal strPkgLine As String, ByVal dtmStartTime As DateTime)
        Dim intRtnCde As Integer
        Dim i As Int16
        Dim strUICtlName As String
        Try
            Dim tblOperStaffing As New dsOperationStaffing.CPPsp_OperationStaffingIODataTable
            Dim rcdOperStaffing As DataRow
            'Try
            'clear the Utility Tech input fields
            For i = 1 To 4
                strUICtlName = "txtUtilityTech" & CStr(i)
                Me.gbxUtilityTech.Controls.Item(strUICtlName).Text = String.Empty
                strUICtlName = "lblUtilityTech" & CStr(i)
                Me.gbxUtilityTech.Controls.Item(strUICtlName).Text = String.Empty
                Me.gbxUtilityTech.Controls.Item(strUICtlName).Visible = False
            Next
            intRtnCde = gtaOperStaff.Fill(tblOperStaffing, strPkgLine, dtmStartTime, "SelectALL")
            'Catch ex As SqlClient.SqlException
            '    If gblnSvrConnIsUp = True Then
            '        SharedFunctions.SetServerCnnStatusInSessCtl(False)
            '    Else
            '        Throw New Exception("Error in FillUtilityTech" & vbCrLf & ex.Message)
            '    End If
            'End Try
            i = 1
            If tblOperStaffing.Rows.Count > 0 Then
                For Each rcdOperStaffing In tblOperStaffing.Rows
                    If IsDBNull(rcdOperStaffing.Item("StaffName")) = False Then
                        strUICtlName = "txtUtilityTech" & CStr(i)
                        Me.gbxUtilityTech.Controls.Item(strUICtlName).Text = rcdOperStaffing.Item("StaffID")
                        strUICtlName = "lblUtilityTech" & CStr(i)
                        Me.gbxUtilityTech.Controls.Item(strUICtlName).Text = rcdOperStaffing.Item("StaffName")
                        Me.gbxUtilityTech.Controls.Item(strUICtlName).Visible = True
                    End If
                    i = i + 1
                Next
            End If
        Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
        Catch ex As Exception
            Throw New Exception("Error in FillUtilityTech" & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Function strReadyToStartShopOrder() As String
        Dim daSMR As New dsStrMchEffRateTableAdapters.QueriesTableAdapter
        Dim sngMchHrs, sngStdWCEfficiency As Single
        Dim strBasisCode As String = String.Empty
        Try
            strReadyToStartShopOrder = String.Empty
            With drSO
                ' If the line is configured to show Line Efficiency, get the machine run rate
                If gdrCmpCfg.ShowEfficiency = True And gdrSessCtl.ServerCnnIsOk = True Then
                    Try
                        daSMR.SelectStdMchEffRate("ChkExistance", gdrSessCtl.Facility, drSO.Item("ItemNumber"), gdrSessCtl.DefaultPkgLine, sngMchHrs, strBasisCode, sngStdWCEfficiency)
                    Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001 Or ex.Number = 10054) And gblnSvrConnIsUp = True
                        SharedFunctions.SetServerCnnStatusInSessCtl(False)
                        SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                    End Try
                End If
                If drSO("QtyPerPallet") = 0 Then
                    gstrErrMsg = "Quantity Per Pallet of the item is missing. Please contact Supervisor."
                ElseIf drSO.ItemNumber = String.Empty Or lblItemNo.Text = String.Empty Then                                 'WO#2563
                    gstrErrMsg = "Item number in the shop order is missing. Please contact Supervisor."                     'WO#2563
                ElseIf drSO("PrintCaseLabel") = "Y" AndAlso drSO("DateToPrintFlag") <> "0" AndAlso Trim(drSO("LabelDateFmtCode")) = "" Then
                    gstrErrMsg = "Label Date Format of the item is missing. Please contact Supervisor."
                ElseIf drSO("PrintCaseLabel") = "Y" AndAlso drSO("DateToPrintFlag") = "1" AndAlso drSO("ProductionShelfLifeDays") = 0 Then
                    gstrErrMsg = "Production Shelf Lift Days of the item is missing. Please contact Supervisor."
                ElseIf drSO("PrintCaseLabel") = "Y" AndAlso Trim(drSO("CaseLabelFmt1")) = "" AndAlso Trim(drSO("CaseLabelFmt2")) = "" AndAlso Trim(drSO("CaseLabelFmt3")) = "" Then
                    gstrErrMsg = "Case Label has not been setup for the SKU. Please contact Supervisor."
                ElseIf drSO("BagLength") = 0 And drSO("BagLengthRequired") = "Y" Then
                    gstrErrMsg = "Bag Length of the item is missing. Please contact Supervisor."
                ElseIf drSO("PrintSOLot") = "Y" AndAlso Trim(drSO("LotID")) = "" Then
                    gstrErrMsg = "Lot No. of the shop order is missing. Please contact Supervisor."
                ElseIf gdrCmpCfg.ShowEfficiency = True And gdrSessCtl.ServerCnnIsOk = True And sngMchHrs = 0 Then
                    gstrErrMsg = "The Standard Machine Run Rate for the item is missing. Please contact Supervisor."
                ElseIf gblnOvrExpDate AndAlso drSO.ShipShelfLifeDays = 0 Then                                              'WO#650
                    gstrErrMsg = "The Ship Shelf Life Days for the item cannot be zero. Please contact Supervisor."         'WO#650
                ElseIf gblnOvrExpDate AndAlso drSO.ProductionShelfLifeDays = 0 Then                                        'WO#650
                    gstrErrMsg = "The Production Shelf Life Days for the item cannot be zero. Please contact Supervisor."   'WO#650
                    'FX150521 DEL Start
                    '    'WO#871 ADD Start
                    'ElseIf gdrCmpCfg.ProbatEnabled = True AndAlso drSO.ComponentItem = "" Then
                    '    gstrErrMsg = "The raw material component for the shop order is missing. Please contact Supervisor."
                    'ElseIf gdrCmpCfg.ProbatEnabled = True AndAlso SharedFunctions.GetReceivingStation(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, drSO.FlavoredCoffee, drSO.OrderType) = String.Empty Then
                    '    gstrErrMsg = "The Probat receiving station for the shop order is missing. Please contact Supervisor."
                    '    'WO#871 ADD Stop
                    'FX150521 DEL Stop
                End If

            End With
            strReadyToStartShopOrder = gstrErrMsg
        Catch ex As Exception
            Throw New Exception("Error in strReadyToStartShopOrder" & vbCrLf & ex.Message)
        End Try
    End Function

    'WO#654 Private Sub btnStartShopOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStartShopOrder.Click
    Private Sub btnStartShopOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStartShopOrder.Click, btnStartWithNoLabel.Click  'WO#654
        Dim objShift As New WorkShift               'WO#3965
        Try
            'WO#654 Add Start
            gblnStartSOWithNoLabel = False
            If gbln2SOIn1Line = True Then
                If ActiveControl.Name = "btnStartShopOrder" Then
                    gblnStartSOWithNoLabel = False
                Else
                    gblnStartSOWithNoLabel = True
                End If
                SharedFunctions.SaveStartSOOption(gblnStartSOWithNoLabel)
            End If
            'WO#654 Add Stop

            If Me.cboShopOrder.Text = "" Then
                MessageBox.Show("Please enter a Shop Order.", "Missing information")
                Me.cboShopOrder.Focus()
            ElseIf Me.txtPkgLine.Text.Trim = "" Then
                MessageBox.Show("Please enter a Packaging Line.", "Missing information")
                Me.txtPkgLine.Focus()
            ElseIf Me.txtShift.Text = "" Then
                MessageBox.Show("Please enter a Shift No.", "Missing information")
                Me.txtShift.Focus()
                'WO#3695 Add Start
            ElseIf Not IsNothing(objShift.IsEnteredShiftValid(CInt(txtShift.Text), Now(), gdrCmpCfg.WorkShiftType, gshtCurrentShift)) Then
                MessageBox.Show("Entered shift no. is not the current, previous or next of shift no., please enter a valid shift no.", "Invalid information")
                Me.txtShift.Focus()
                'WO#3695 Add Stop
            ElseIf Me.txtOperator.Text = "" Then
                MessageBox.Show("Please enter a Operator ID.", "Missing information")
                Me.txtOperator.Focus()
            ElseIf Me.txtBagLengthUsed.Text = "" AndAlso gstrBagLengthRequired = "Y" Then
                MessageBox.Show("Please enter Bag Length.", "Missing information")
                Me.txtBagLengthUsed.Focus()
            ElseIf Me.txtCasesScheduledInShift.Text = "" Then
                MessageBox.Show("Please enter Cases Scheduled.", "Missing information")
                Me.txtCasesScheduledInShift.Focus()
            ElseIf strReadyToStartShopOrder() <> String.Empty Then
                MessageBox.Show(gstrErrMsg, "Missing information")
                cboShopOrder.Focus()
                'WO#718 ADD Start
            ElseIf gdrCmpCfg.ProcessType = "3" AndAlso gblnSvrConnIsUp = True AndAlso gstrLabelInputFileName.Substring(0, 2) <> "\\" Then
                MessageBox.Show("Systems interface file path is missing, please log on again to get it.", "Missing information")
                btnPrvScn.Focus()
                'WO#718 ADD Stop
            Else
                'WO#650 ADD Start
                If gblnOvrExpDate = True AndAlso (drSO.DateToPrintFlag = "1" Or drSO.DateToPrintFlag = "3") Then
                    gdteExpiryDate = DateTime.MinValue  'force the operator to enter the expiry date again.
                    frmExpiryDate.ShowDialog()
                Else
                    gdteExpiryDate = Nothing
                End If

                'WO#17432 ADD Start
                If drSO.PrintCaseLabel <> "Y" Then
                    MessageBox.Show("This item will not produce case label.", "For Your Information")
                End If
                'WO#17432 ADD Stop

                'WO#650 ADD Stop
                'WO#650 SharedFunctions.StartShopOrderUpdate(Me, gintShopOrder, gdrSessCtl.OverrideShiftNo, gstrPrintCaseLabel, gstrLotID)
                'WO#654 SharedFunctions.StartShopOrderUpdate(Me, gintShopOrder, gdrSessCtl.OverrideShiftNo, gstrPrintCaseLabel, gstrLotID, gdteExpiryDate)  'WO#650
                SharedFunctions.RefreshComputerConfig(gdrSessCtl.ComputerName, txtPkgLine.Text)                         'WO#5370 
                SharedFunctions.StartShopOrderUpdate(Me, gintShopOrder, gdrSessCtl.OverrideShiftNo, gstrPrintCaseLabel, gstrLotID, gdteExpiryDate, gblnStartSOWithNoLabel) 'WO#654
                gstrLineName = SharedFunctions.GetEquipmentDescription(gdrCmpCfg.Facility, gdrSessCtl.DefaultPkgLine)   'WO#5370
                ''WO#755 ADD Start
                'If gblnAutoCaseCountLine = True AndAlso gdrSessCtl.ShopOrder <> 0 Then
                '    If IsNothing(CaseCounter) Then
                '        CaseCounter = New Counter(gdrSessCtl.Facility, gdrSO.ShopOrder, gdrSO.QtyPerPallet, "Stopped")
                '    End If
                '    If CaseCounter.CounterIsStarted = False Then
                '        CaseCounter.Start(False)
                '    End If

                '    Dim xmlInput As New PowerPlant.XMLInterface(gstrLabelInputFileName)
                '    xmlInput.UpdateSOInitialCaseCount(CaseCounter.CasesProducedRunningTotal)
                '    SharedFunctions.RefreshSessionControlTable()

                'End If
                ''WO#755 ADD End 
                'ALM#11828 gstrLastOutputLocation = "RAF"  'WO#2563
                Me.Close()
                End If
                If gblnSvrConnIsUp = False Then
                    SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtShift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtShift.TextChanged
        Dim drResponse As DialogResult
        Dim daShift As New dsShiftTableAdapters.CPPsp_ShiftIOTableAdapter           'WO#2645
        Dim dtShift As New dsShift.CPPsp_ShiftIODataTable                           'WO#2645
        Dim intEnteredShift As Integer                                              'WO#3695
        Dim objShift As New WorkShift()                                             'WO#3695
        Try
            gstrErrMsg = Nothing

            If txtShift.Text <> "" Then
                intEnteredShift = CType(txtShift.Text, Integer)                         'WO#3695
                'WO#2645 If CType(txtShift.Text, Integer) < 1 Or CType(txtShift.Text, Integer) > 4 Then
                'WO#2645 Add Start
                daShift.Fill(dtShift, "AllShifts", gdrSessCtl.Facility, gdrCmpCfg.WorkShiftType, Now(), Nothing, 1)
                If dtShift.Rows.Count = 0 Then
                    gstrErrMsg = "The pre-defined shift information is missing, please contact supervisor."
                    MessageBox.Show(gstrErrMsg, "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtShift.Focus()
                ElseIf CType(txtShift.Text, Integer) < 1 Or CType(txtShift.Text, Integer) > dtShift.Rows.Count Then
                    'WO#3695    gstrErrMsg = "Please entered a valid shift no. from 1 to " & CType(dtShift.Rows.Count, String) & "."
                    gstrErrMsg = "Please enter a valid shift no. from 1 to " & CType(dtShift.Rows.Count, String) & "."          'WO#3695
                    'WO#2645 Add Stop
                    'WO#2645    gstrErrMsg = "Please entered a valid shift no. 1 to 4."
                    MessageBox.Show(gstrErrMsg, "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtShift.Focus()
                    'WO#3695 Add Start
                ElseIf Not IsNothing(objShift.IsEnteredShiftValid(intEnteredShift, Now(), gdrCmpCfg.WorkShiftType, gshtCurrentShift)) Then
                    gstrErrMsg = "Entered shift no. is not the current, previous or next of shift no., please enter a valid shift no."
                    MessageBox.Show(gstrErrMsg, "Invalid Shift Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtShift.Focus()
                ElseIf txtPkgLine.Text <> "" Then
                    If txtShift.Text <> gdrSessCtl.OverrideShiftNo Or txtShift.Text <> gshtCurrentShift Then
                        If gblnInitialLoad = False Then
                            If txtShift.Text <> gdrSessCtl.OverrideShiftNo Then
                                gstrErrMsg = "Entered shift no. is different from the shift you log on or shop order last started. Do you want to continue?"
                            Else
                                gstrErrMsg = "Entered shift no. is different from the shift based on currrent time. Do you want to continue?"
                            End If
                            drResponse = MessageBox.Show(gstrErrMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        Else
                            If txtShift.Text <> gshtCurrentShift Then
                                'WO#5370 gstrErrMsg = "Log on shift no. is different from the one based on currrent time. Please verify it on next screen."
                                gstrErrMsg = "Log on shift no. is different from the one based on currrent time."       'WO#5370
                            End If
                            MessageBox.Show(gstrErrMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)            'WO#3695
                            'WO#3695    MessageBox.Show(gstrErrMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question)
                        End If

                        If drResponse = Windows.Forms.DialogResult.No Then
                            txtShift.Focus()
                            Exit Sub
                        Else
                            gstrErrMsg = Nothing
                            If gtblSCH.Rows.Count = 0 Or (gtblSCH.Rows.Count <> 0 AndAlso txtShift.Text <> gtblSCH.Rows(0).Item("OverrideShiftNo")) Then
                                txtOperator.Text = ""
                                lblOperator.Text = ""
                                lblOperator.Visible = False

                                txtUtilityTech1.Text = ""
                                lblUtilityTech1.Text = ""
                                lblUtilityTech1.Visible = False

                                txtUtilityTech2.Text = ""
                                lblUtilityTech2.Text = ""
                                lblUtilityTech2.Visible = False

                                txtUtilityTech3.Text = ""
                                lblUtilityTech3.Text = ""
                                lblUtilityTech3.Visible = False

                                txtUtilityTech4.Text = ""
                                lblUtilityTech4.Text = ""
                                lblUtilityTech4.Visible = False
                            End If
                        End If
                    End If
                    'txtCasesScheduled.Text = lblTotalScheduled.Text - SharedFunctions.GetSOCasesProducedFromPallet(gintShopOrder, True)
                    ''Calculate cases produced in the current shift
                    'Try
                    '    'lblCasesProduced.Text = SharedFunctions.GetCaseProducedByShift(txtPkgLine.Text, gintShopOrder, _
                    '    '                        txtShift.Text, SharedFunctions.GetProductionDateByShift(txtShift.Text))
                    '    lblCasesProduced.Text = SharedFunctions.GetSOCasesProducedFromPallet(gintShopOrder, True, CType(txtShift.Text, Short))
                    'Catch ex As SqlClient.SqlException
                    '    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    '    SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                    'Catch ex As Exception
                    '    Throw New Exception("Error in GetCaseProducedByShift" & vbCrLf & ex.Message)
                    'End Try
                    ''Calculate cases remain in the current shift
                    'lblCasesRemain.Text = txtCasesScheduled.Text - lblCasesProduced.Text
                    'LoadDataToScreen(gdrSO)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub txtShift_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtShift.Validating
        If ActiveControl.Name <> "btnPrvScn" Then
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid Information")
                e.Cancel = True
            End If
        Else
            Close()
        End If
    End Sub

    Private Sub OperOrUTechTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUtilityTech1.TextChanged, txtUtilityTech2.TextChanged, _
                                        txtUtilityTech3.TextChanged, txtUtilityTech4.TextChanged, txtOperator.TextChanged
        Dim strName As String = ""
        Dim strAssociatedLabelName As String = Nothing
        gstrErrMsg = Nothing
        Try
            strAssociatedLabelName = Replace(sender.name, "txt", "lbl", 1, 1)
            If sender.text <> "" Then
                If strAssociatedLabelName = "lblOperator" Then
                    Me.Controls(strAssociatedLabelName).Text = ""
                    Me.Controls(strAssociatedLabelName).Visible = False
                    'WO#5370_    If gblnInitialLoad = False Then
                    If gblnInitialLoad = False And Not IsNothing(drSO) Then               'WO#5370_ 
                        'only for value of operator textbox changed 
                        txtCFCases.Text = SharedFunctions.GetCarriedForwardCases(gdrSessCtl.OverridePkgLine, drSO.ShopOrder, txtShift.Text, gdrSessCtl.ShiftProductionDate, Now, gdrSessCtl.Facility, sender.text)
                        ' the change on the txtCFCases will trigger the calculaton of cases produced totals
                    End If
                Else
                    Me.gbxUtilityTech.Controls(strAssociatedLabelName).Text = ""
                    Me.gbxUtilityTech.Controls(strAssociatedLabelName).Visible = False
                End If
                strName = SharedFunctions.GetStaffName(sender.Text, "P")

                If Trim(strName) <> "" Then
                    'WO#755 ADD Start
                    If Trim(strName) = "Deactivated" Then
                        gstrErrMsg = "Please enter an active ID."
                        MessageBox.Show(gstrErrMsg, "Invalid information")
                        sender.focus()
                    Else
                        'WO#755 ADD End
                        If strAssociatedLabelName = "lblOperator" Then
                            Me.Controls(strAssociatedLabelName).Text = strName
                            Me.Controls(strAssociatedLabelName).Visible = True
                        Else
                            Me.gbxUtilityTech.Controls(strAssociatedLabelName).Text = strName
                            Me.gbxUtilityTech.Controls(strAssociatedLabelName).Visible = True
                        End If
                    End If  'WO#755
                Else
                    gstrErrMsg = "Invalid ID, Please entry again."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    sender.focus()
                    Exit Sub
                End If

                If ChkDupIDs(Me, sender.name, sender.text) Then
                    gstrErrMsg = "ID is already entered, Please entry again."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    sender.focus()
                    Exit Sub
                End If
            Else
                If strAssociatedLabelName = "lblOperator" Then
                    Me.Controls(strAssociatedLabelName).Text = ""
                    Me.Controls(strAssociatedLabelName).Visible = True
                Else
                    Me.gbxUtilityTech.Controls(strAssociatedLabelName).Text = ""
                    Me.gbxUtilityTech.Controls(strAssociatedLabelName).Visible = False
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub validateOperOrUTech(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtUtilityTech1.Validating, txtUtilityTech2.Validating, _
        txtUtilityTech3.Validating, txtUtilityTech4.Validating, txtOperator.Validating
        If (ActiveControl.Name = "btnPrvScn") Then
            Me.Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub GetNextSOInfo()
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim intRtnCde As Integer
        Dim blnErrorFound As Boolean

        Try
            blnErrorFound = False
            intRtnCde = taSO.Fill(tblSO, "GetNextSO", gdrSessCtl("Facility"), 0, gdrSessCtl("DefaultPkgLine"))
            If tblSO.Rows.Count > 0 Then
                'LoadSOInfo(tblSO.Rows(0), True)
                'cboShopOrder.Text = tblSO.Rows(0).Item("ShopOrder")
                drSO = tblSO.Rows(0)
                With gdrSessCtl
                    'For operator an utility technicians,if shift different from the previous session, 
                    'get operator from current session and blanks out utility technicians 
                    'else default from previous session
                    If gtblSCH.Rows.Count > 0 Then
                        If .Item("OverrideShiftNo") <> gtblSCH.Rows(0).Item("OverrideShiftNo") Then
                            'If Not IsDBNull(.Item("Operator")) Then
                            txtOperator.Text = .Operator
                            'End If
                        Else
                            With gtblSCH.Rows(0)
                                txtOperator.Text = .Item("Operator")
                                'get Utility staffs based on the Default Pkg Line and Start Time from Session Control History
                                FillUtilityTech(.Item("DefaultPkgLine"), .Item("StartTime"))
                            End With
                        End If

                    End If
                End With
                blnErrorFound = True
            Else
                Me.txtPkgLine.Text = gdrSessCtl("OverridePkgLine")
            End If
            If blnErrorFound = False Then
                cboShopOrder.Text = ""
                'WO#2563  MessageBox.Show("No Scheduled Shop Order found.", "Warning.")
                InzInputFields()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Shared Function ChkDupIDs(ByVal frm As Form, ByVal strCtlName As String, ByVal strID As String) As Boolean
        'check duplication of ID on the Operator or Utility Tech. fields on the screen
        Dim ctl As Control
        Try
            ChkDupIDs = False

            For Each ctl In frm.Controls
                If TypeOf ctl Is TextBox AndAlso ctl.Name <> strCtlName AndAlso ctl.Name = "txtOperator" Then
                    If ctl.Text = strID Then
                        ChkDupIDs = True
                        Exit Function
                    End If
                End If
                If TypeOf ctl Is GroupBox AndAlso ctl.Name = "gbxUtilityTech" Then
                    Dim ctlInside As Control
                    For Each ctlInside In ctl.Controls
                        If TypeOf ctlInside Is TextBox AndAlso ctlInside.Name <> strCtlName AndAlso ctlInside.Name.Substring(0, Len(ctlInside.Name) - 1) = "txtUtilityTech" Then
                            If ctlInside.Text = strID Then
                                ChkDupIDs = True
                                Exit Function
                            End If
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub txtBagLengthUsed_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtBagLengthUsed.Validating
        Try
            If (ActiveControl.Name = "btnPrvScn") Then
                Me.Close()
            Else
                If sender.text <> "" Then
                    Try
                        If CType(sender.text, Single) = 0 Then
                            MessageBox.Show("Please enter Bag Length.", "Missing information")
                            e.Cancel = True
                        ElseIf CType(sender.text, Single) < 0 Then
                            MessageBox.Show("Bag Length can not be a negative value.", "Invalid information")
                            e.Cancel = True
                        End If
                    Catch ex As Exception
                        MessageBox.Show("Please enter a valid Bag Length.", "Invalid information")
                    End Try
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnChkBOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChkBOM.Click
        Try
            If cboShopOrder.Text <> "" Then
                frmBillOfMaterials.ShowDialog()
            Else
                MessageBox.Show("Please select a Shop Order before check the BOM.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        Dim strPkgLineDesc As String
        Dim strPackagingLine As String
        Try
            gstrErrMsg = Nothing
            'Retrieve equipment record using the packaging line 
            If txtPkgLine.Text <> "" Then
                If txtPkgLine.Text.Length < txtPkgLine.MaxLength Then
                    'FX151220    txtPackagingLine.text = txtPkgLine.Text.Trim.PadRight(txtPkgLine.MaxLength)
                    strPackagingLine = txtPkgLine.Text.Trim.PadRight(txtPkgLine.MaxLength)  'FX151220
                Else                                                                        'FX151220
                    strPackagingLine = txtPkgLine.Text                                      'FX151220
                End If
                strPkgLineDesc = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), strPackagingLine)       'FX151220
                'FX151220 strPkgLineDesc = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text)
                'WO#359 gdrEquipment = SharedFunctions.GetEquipmentInfo(gdrCmpCfg("Facility"), sender.text)
                If strPkgLineDesc = String.Empty Then
                    'WO#359 If gdrEquipment Is Nothing Then
                    gstrErrMsg = "Please enter a valid Packaging Line."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtPkgLine.Focus()
                Else
                    cboShopOrder_DropDown(cboShopOrder, e)
                    cboShopOrder_DropDownClosed(cboShopOrder, e)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtPkgLine_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPkgLine.Validating
        Try
            If (ActiveControl.Name = "btnPrvScn") Then
                Me.Close()
            Else
                If Not IsNothing(gstrErrMsg) Then
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    e.Cancel = True     'FX151220
                End If
                'Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable
                ''Dim taEQ As New dsEquipmentTableAdapters.CPPsp_EquipmentIOTableAdapter
                'Dim intRtnCde As Integer
                'If txtPkgLine.Text <> "" Then
                '    'Retrieve equipment record using the packaging line from the Session Control History
                '    intRtnCde = gtaEQ.Fill(tblEQ, gdrSessCtl("Facility"), sender.text, "P", "")
                '    If tblEQ.Rows.Count = 0 Then
                '        MessageBox.Show("Please enter a valid Packaging Line.", "Invalid information")
                '        e.Cancel = True
                '    End If
                'End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnChkSONotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnChkSONotes.Click
        Try
            If cboShopOrder.Text <> "" Then
                frmShopOrderNotes.ShowDialog()
            Else
                MessageBox.Show("Please select a Shop Order before check the Shop Order Notes.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub InzInputFields()
        lblCasesProducedInShift.Text = "0"
        lblCasesRemainInShift.Text = "0"
        lblItemDesc.Text = ""
        lblItemNo.Text = ""
        lblLabelWeight.Text = "0.00"
        lblStdBagLength.Text = "0.0"
        lblUnitPerCase.Text = ""
        txtBagLengthUsed.Text = ""
        txtCasesScheduledInShift.Text = ""
        txtCFCases.Text = String.Empty
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub txtCasesScheduled_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCasesScheduledInShift.TextChanged
        gstrErrMsg = Nothing
        If txtCasesScheduledInShift.Text <> "" Then
            Try
                If CType(txtCasesScheduledInShift.Text, Single) = 0 Then
                    gstrErrMsg = "Please enter Cases Scheduled."
                    MessageBox.Show(gstrErrMsg, "Missing information")
                    txtCasesScheduledInShift.Focus()
                ElseIf CType(txtCasesScheduledInShift.Text, Single) < 0 Then
                    gstrErrMsg = "Cases Scheduled can not be a negative value."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtCasesScheduledInShift.Focus()
                Else
                    lblCasesRemainInShift.Text = txtCasesScheduledInShift.Text - lblCasesProducedInShift.Text
                End If
            Catch ex As Exception
                gstrErrMsg = "Please enter a valid Cases Scheduled."
                MessageBox.Show(gstrErrMsg, "Invalid information")
                txtCasesScheduledInShift.Focus()
            End Try
        End If
    End Sub

    Private Sub txtCasesScheduled_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCasesScheduledInShift.Validating
        If ActiveControl.Name = "btnPrvScn" Then
            Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub cboShopOrder_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboShopOrder.DropDown
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Try
            With cboShopOrder
                'gtaSO.Fill(tblSO, "GetSOList", gdrSessCtl("Facility"), 0, gdrSessCtl("DefaultPkgLine"))
                gtaSO.Fill(tblSO, "GetSOList", gdrSessCtl("Facility"), 0, txtPkgLine.Text)
                .DataSource = tblSO
                .DisplayMember = "SODescription"
                .ValueMember = "ShopOrder"
                .BackColor = Color.DarkGreen
                'WO1297 DEL If gblnAjaxPlant Then
                'WO#@@@@    .Width = .DropDownWidth + 220
                .Width = .DropDownWidth + 200   'WO#@@@@
                .Font = New Font("Arial", 20, System.Drawing.FontStyle.Regular)
                'WO1297 DEL Start
                'Else
                '    .Width = .DropDownWidth
                '    .Font = New Font("Arial", 30, System.Drawing.FontStyle.Regular)
                'End If
                'WO1297 DEL Stop
                LoadSOInfo(drSO)        'WO#5370
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
                'WO#5370    .Width = 425    'WO#2563
                .Width = 625    'WO#5370
            End If
            '.Text = .Text.Substring(0, 6)
            .BackColor = Color.Black
        End With
    End Sub

    Private Sub cboShopOrder_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboShopOrder.TextChanged
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim intRtnCde As Integer
        Dim drResponse As DialogResult
        'Dim daSMR As New dsStrMchEffRateTableAdapters.QueriesTableAdapter
        'Dim sngMchHrs, sngStdWCEfficiency As Single
        Dim strBasisCode As String = String.Empty

        Try
            'WO#5370    InzInputFields()
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
                    InzInputFields()           'WO#5370
                    gstrCurrentShopOrder = CType(gintShopOrder, String)
                    cboShopOrder.Font = gfntSOdefaultFont
                    intRtnCde = gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gintShopOrder, "")
                    With tblSO
                        If .Rows.Count > 0 Then
                            drSO = .Rows(0)
                            'if one of the fields from Item Master table is null, it means the item of requested the shop order is not found.
                            If Not drSO.IsBagLengthRequiredNull Then                'WO#871
                                gstrBagLengthRequired = drSO("BagLengthRequired")
                                gstrPrintCaseLabel = drSO("PrintCaseLabel")
                                Me.lblItemNo.Text = drSO("itemnumber")
                                Me.lblItemDesc.Text = drSO("ItemDesc1")
                                '' If the line is configured to show Line Efficiency, get the machine run rate
                                'If gdrCmpCfg.ShowEfficiency = True And gdrSessCtl.ServerCnnIsOk = True Then
                                '    daSMR.SelectStdMchEffRate("ChkExistance", gdrSessCtl.Facility, gdrSO.Item("ItemNumber"), gdrSessCtl.DefaultPkgLine, sngMchHrs, strBasisCode, sngStdWCEfficiency)
                                'End If

                                If Not IsDBNull(drSO("Closed")) Then
                                    drResponse = MessageBox.Show("The shop order is closed. Do you want to reopen it?", "Warning!", MessageBoxButtons.YesNo)
                                    If drResponse = Windows.Forms.DialogResult.Yes Then
                                        'Update the shop order table to re-open the SO
                                        SharedFunctions.ChangeSOStatus(drSO.ShopOrder, SharedFunctions.ShopOrderStatus.Open)
                                        'load data to screen
                                        If gblnInitialLoad = False Then
                                            LoadDataToScreen(drSO)
                                        End If
                                    Else
                                        cboShopOrder.Text = ""
                                        gstrErrMsg = "Please enter another Shop Order."
                                        MessageBox.Show(gstrErrMsg, "Invalid Information.")
                                        cboShopOrder.Focus()
                                    End If
                                ElseIf strReadyToStartShopOrder() <> String.Empty Then
                                    cboShopOrder.Focus()
                                    'ElseIf gdrSO("QtyPerPallet") = 0 Then
                                    '    gstrErrMsg = "Quantity Per Pallet of the item is missing, please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrSO("PrintCaseLabel") = "Y" AndAlso gdrSO("DateToPrintFlag") <> "0" AndAlso Trim(gdrSO("LabelDateFmtCode")) = "" Then
                                    '    gstrErrMsg = "Label Date Format of the item is missing, Please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrSO("PrintCaseLabel") = "Y" AndAlso gdrSO("DateToPrintFlag") = "1" AndAlso gdrSO("ProductionShelfLifeDays") = 0 Then
                                    '    gstrErrMsg = "Production Shelf Lift Days of the item is missing, Please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrSO("PrintCaseLabel") = "Y" AndAlso Trim(gdrSO("CaseLabelFmt1")) = "" Then
                                    '    gstrErrMsg = "Case Label has not been setup for the SKU, please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrSO("BagLength") = 0 And gdrSO("BagLengthRequired") = "Y" Then
                                    '    gstrErrMsg = "Bag Length of the item is missing, Please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrSO("PrintSOLot") = "Y" AndAlso Trim(gdrSO("LotID")) = "" Then
                                    '    gstrErrMsg = "Lot No. of the shop order is missing, Please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrCmpCfg.ShowEfficiency = True And gdrSessCtl.ServerCnnIsOk = True And sngMchHrs = 0 Then
                                    '    gstrErrMsg = "The Standard Machine Run Rate for the item is missing, Please contact Supervisor."
                                    '    MessageBox.Show(gstrErrMsg, "Missing Information.")
                                    '    cboShopOrder.Focus()
                                    'ElseIf gdrSO("PackagingLine") <> gdrSessCtl("DefaultPkgLine") Then
                                    'WO#5370 ElseIf RTrim(drSO("PackagingLine")) <> RTrim(txtPkgLine.Text) Then
                                ElseIf RTrim(txtPkgLine.Text) <> "" AndAlso RTrim(drSO("PackagingLine")) <> RTrim(txtPkgLine.Text) Then     'WO#5370
                                    drResponse = MessageBox.Show(String.Format("Shop order {0} is not for this packaging line. Do you want to continue?", drSO.ShopOrder), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 'WO#5370
                                    'WO#5370 drResponse = MessageBox.Show("Shop order is not for this packaging line. Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    If drResponse = Windows.Forms.DialogResult.No Then
                                        'gstrErrMsg = "Shop order is not for this packaging line. Do you want to continue?"
                                        cboShopOrder.Focus()
                                    Else
                                        'Pack for other packaging line order, load data to screen
                                        If gblnInitialLoad = False Then
                                            LoadDataToScreen(drSO)
                                        End If
                                        cboShopOrder.Width = gintSODftWidth
                                    End If
                                Else
                                    'Every thing is fine, load data to screen
                                    If gblnInitialLoad = False Then
                                        LoadDataToScreen(drSO)
                                    End If
                                End If
                                'WO#871 ADD Start
                            Else
                                gstrErrMsg = String.Format("The item {0} in the Shop order {1} is not found. please enter another shop order number or fix the item.", drSO.ItemNumber, drSO.ShopOrder)
                                MessageBox.Show(gstrErrMsg, "Missing Information")
                                cboShopOrder.Focus()
                            End If
                            'WO#871 ADD Stop
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

    Private Sub txtCFCases_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCFCases.TextChanged
        gstrErrMsg = Nothing
        If sender.Text <> "" Then
            Try
                If CType(txtCFCases.Text, Integer) < 0 Then
                    gstrErrMsg = "Carried Forward Cases can not be a negative value."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtCFCases.Focus()
                ElseIf CType(txtCFCases.Text, Single) > drSO.QtyPerPallet Then
                    gstrErrMsg = "Carried Forward Cases can not greater than cases per pallet."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtCFCases.Focus()
                Else
                    If gblnInitialLoad = False Then
                        CalculatePackingProgress(drSO)
                    End If
                End If
            Catch ex As Exception
                gstrErrMsg = "Please enter a valid Carried Forward Cases."
                MessageBox.Show(gstrErrMsg, "Invalid information")
                txtCFCases.Focus()
            End Try
        End If
    End Sub
    Private Sub txtCFCases_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCFCases.Validating
        If ActiveControl.Name = "btnPrvScn" Then
            Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub frmStartShopOrder_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        'Dim strErrMsg As String
        With gdrSessCtl
            txtPkgLine.Text = .OverridePkgLine
            Dim objShift As New WorkShift                                                   'WO#2645
            objShift.GetExpectedShiftInfoByTime(Now(), gdrCmpCfg("WorkShiftType"), True)    'WO#2645
            gshtCurrentShift = objShift.Shift                                               'WO#2645
            'WO#2645    gshtCurrentShift = New WorkShift(Now, gdrCmpCfg("WorkShiftType")).Shift
            txtShift.Text = .OverrideShiftNo
            'txtOperator.Text = .Operator
        End With

        'If Not IsNothing(drSO) Then
        '    'Save the error message and display it after the screen is shown.
        '    gstrErrMsg = String.Empty
        '    cboShopOrder.Text = drSO.ShopOrder 'This will trigger the text changed event
        '    strErrMsg = gstrErrMsg
        '    If gstrErrMsg = String.Empty Then           'WO#817
        '        cboShopOrder.SelectedValue = drSO.ShopOrder
        '        LoadDataToScreen(drSO)
        '        'restore the error message created after shop order text changed event
        '        If strErrMsg <> String.Empty Then
        '            gstrErrMsg = strErrMsg
        '        End If
        '    End If                                      'WO#817
        'End If

        If gstrErrMsg <> String.Empty Then
            MessageBox.Show(gstrErrMsg, "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class
