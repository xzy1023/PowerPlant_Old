Module modStartup
    '----------------------------------------------------
    ' WO#650        Mar. 19, 2012   Bong Lee
    ' Description:  Allow operator to enter the Expiry Date for case label on specific line.
    '----------------------------------------------------
    Public gstrNumPadValue As String
    Public gstrAlphaNumKBValue As String

    Public gblnSvrConnIsUp As Boolean = True

    Public gstrCurrentShopOrder As String

    Public gstrServerConnectionString As String
    Public gstrLocalDBConnectionString As String
    Public gstrConnStr As String

    Public gstrProcessEnv As String
    Public gblnCallFromCreatePallet As Boolean
    Public gdrCmpCfg As dsComputerConfig.CPPsp_ComputerConfigIORow
    Public gdrSessCtl As dsSessionControl.CPPsp_SessionControlIORow
    Public gdrEquipment As dsEquipment.CPPsp_EquipmentIORow
    Public gdrQATWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow                           'WO17432
    Public drSO As dsShopOrder.CPPsp_ShopOrderIORow

    Public gblnDropDownIsClicked = True
    Public Const gblnTryConnect = True
    Public Const gblnTrySessCtl = False
    Public Const gcstSvrCnnFailure As String = "No Server connection, label printing request is denied."

    'Record Type
    Public Const CASELABEL As String = "C"      'Case Label for Case Labeler/Filter Coder
    Public Const PALLETLABEL As String = "P"    'Pallet Label
    Public Const PACKAGELABEL As String = "X"      'Package Coder
    Public Const ITEMLABEL As String = "I"      'Item Label

    'Device Type
    Public Const CASELABELER As String = "C"    'Case label printer
    Public Const PALLETLABELER As String = "P"  'Pallet label printer
    Public Const PACKAGECODER As String = "X"   'Package coder
    Public Const FILTERCODER As String = "F"    'Filter code

    'Adapters always use local data base
    Public gtaCmpCfg As New dsComputerConfigTableAdapters.CPPsp_ComputerConfigIOTableAdapter
    Public gtaEQ As New dsEquipmentTableAdapters.CPPsp_EquipmentIOTableAdapter
    Public gtaItemMaster As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
    Public gtaSessCtl As New dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
    Public gtaShift As New dsShiftTableAdapters.CPPsp_ShiftIOTableAdapter
    Public gtaSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
    'WO#17432   Public gtaPlantStaff As New dsPlantStaffingTableAdapters.CPPsp_PlantStaffingIOTableAdapter

    'Adapters use server data base as primary connection and local data base as secondary connection
    Public gtaCompScrap As New dsComponentScrapTableAdapters.CPPsp_EditComponentScrapTableAdapter
    Public gtaOperStaff As New dsOperationStaffingTableAdapters.CPPsp_OperationStaffingIOTableAdapter
    Public gtaPallet As New dsPalletTableAdapters.CPPsp_PalletIOTableAdapter
    Public gtaSCH As New dsSessionControlHstTableAdapters.CPPsp_SessionControlHstIOTableAdapter

    'Adapters use server data base only
    Public gtaPallethst As New dsPalletHstTableAdapters.CPPsp_PalletHstIOTableAdapter
    'Current form name
    Public gstrPrvForm As String
    Public gblnAjaxPlant As Boolean

    Public gdteExpiryDate As DateTime           'WO#650
    Public gblnOvrExpDate As Boolean            'WO#650
    Public gblnStartSOWithNoLabel As Boolean    'WO#654
    Public gbln2SOIn1Line As Boolean            'WO#654
    'WO#5370 Public gblnAutoCaseCountLine As Boolean      'WO#718
    Public gblnAutoCountLine As Boolean         'WO#5370

    Public gstrLabelInputFileName As String     'WO#718
    Public gstrLabelOutputFileName As String    'WO#718
    Public gblnEnableOutputLocationLine As Boolean      'WO#2563

    Public CaseCounter As Counter               'WO#755
    Public gstrLineName As String               'WO#755
    Public gintSOInitiallyProduced As Integer           'WO#755
    Public gintSOPreviousShiftsProduced As Integer      'WO#755
    Public gstrMyComputerName As String             'WO#871
    'ALM#11828  Public gstrLastOutputLocation As String         'WO#2563
    Public gstrLastOutputLocation As String = "RAF"         'ALM#11828
    Public gintLastDestinationShopOrder As Integer          'ALM#11828
    Public gStrLastDestinationPkgLine As String = ""        'ALM#11828
    Public gblnLastDestinationIPCConn As Boolean = 1        'ALM#11828
    Public gintAutoCountByUnit As Short                         'WO#5370
    Public gblnInterfaceType As String                          'WO#5370
    Public gblnSarongAutoCountLine As Boolean                   'WO#5370
    Public gdteTestBatchID As DateTime                          'WO#17432
    Public gstrInterfaceID As String = String.Empty             'WO#17432
    Public gstrQATWorkFlowType As String = String.Empty         'WO#17432
    Public gstrQATTesterID As String                            'WO#17432 - BL 3/28/2019
    Public gdteLastPalletCreatedAt As DateTime                  'WO#34957

    'WO#5370 ADD Start
    Public Enum AutoCountBy
        Cases
        Pallets
    End Enum
    'WO#5370 ADD Stop

    'WO#17432 ADD Start
    Public Enum QATWorkFlow
        Disabled = 0
        External = 1
        internal = 2
    End Enum

    'QAT Entry Point
    Public Const cstrStartup As String = "S"
    Public Const cstrInProcess As String = "I"
    Public Const cstrChangeShift As String = "M"
    Public Const cstrClosing As String = "C"
    Public Const cstrOnRequest As String = "O"


    'WO#17432 ADD Stop

    Public Function Main(ByVal CmdArgs() As String) As Integer
        Dim frm As Form = Nothing
        Dim strPgmID As String = ""
        ' Dim tblSessCtl As New dsSessionControl.CPPsp_SessionControlIODataTable
        'WO#650 Dim tblCmpCfg As New dsComputerConfig.CPPsp_ComputerConfigIODataTable
        'WO#650 Dim intRtnCde As Integer
        Dim i As Short
        'WO#871 Dim strMyComputerName As String = My.Computer.Name
        Dim handle As IntPtr
        Dim pInstance As Process = Nothing
        Dim strArg1 As String = Nothing             'WO#15635
        Dim IntQtyInPallet As Integer               'WO#15635
        Dim strArg2 As String = Nothing             'WO#17432
        gstrMyComputerName = My.Computer.Name       'WO#871

        Try
            Cursor.Current = Cursors.WaitCursor
            'Is current process running? If the process is not found, then wait 1 second and try check again. 
            'And check totally 4 times.
            For i = 1 To 4
                pInstance = SharedFunctions.GetRunningInstance(Process.GetCurrentProcess().ProcessName)
                If Not pInstance Is Nothing Then
                    System.Threading.Thread.Sleep(1000)
                Else
                    Exit For
                End If
            Next

            'make sure only one instance of the program can be run in a computer       
            pInstance = SharedFunctions.GetRunningInstance(Process.GetCurrentProcess().ProcessName)
            If Not pInstance Is Nothing Then
                handle = pInstance.MainWindowHandle
                If Not IntPtr.Zero.Equals(handle) Then
                    'WO#650 Win32Helper.ShowWindow(handle, 1)   '1 = nornmal
                    Win32Helper.SetForegroundWindow(handle)
                    Win32Helper.ShowWindow(handle, ProcessWindowStyle.Maximized)   'WO#650
                End If
                Application.ExitThread()
            Else

                If My.Application.CommandLineArgs.Count > 0 Then
                    'WO#15635 ADD Start
                    For intCount As Int16 = 0 To My.Application.CommandLineArgs.Count - 1
                        Select Case intCount
                            Case 0
                                'WO#15635 ADD Stop
                                strPgmID = LCase(CmdArgs(0)).Replace(vbCrLf, "")
                                'WO#15635 ADD Start
                            Case 1
                                'WO#17432 strArg1 = LCase(CmdArgs(1))         'WO#15635 - QtyInPallet
                                strArg1 = CmdArgs(1).Replace(vbCrLf, "")      'WO#17432  - QtyInPallet/interfaceID
                            Case 2                                            'WO#17432
                                strArg2 = CmdArgs(2).Replace(vbCrLf, "")      'WO#17432  - QAT Work Flow Type
                            Case Else
                        End Select

                    Next
                    'WO#15635 ADD Stop
                    gstrLocalDBConnectionString = My.Settings.LocalPowerPlantCnnStr

                    'Waiting for Local SQL Express service start before connecting the local data base 
                    If strPgmID = "logon_noplc" Or strPgmID = "logon" Then
                        Dim processList() As Process = Nothing

                        For i = 1 To 200
                            processList = Process.GetProcessesByName("sqlservr")
                            If processList.Length = 0 Then
                                System.Threading.Thread.Sleep(1000)
                            Else
                                Exit For
                            End If
                        Next

                        For i = 1 To 10
                            If SharedFunctions.IsLocalConnOK = True Then
                                Exit For
                            End If
                        Next

                        While SharedFunctions.IsLocalConnOK = False
                            MessageBox.Show("The local database connection is not ready. Please contact supervisor if this message appears again.", _
                                        "Local database connection is not ready", MessageBoxButtons.OK)
                        End While

                    End If

                    gstrServerConnectionString = My.Settings.ServerPowerPlantCnnStr
                    'WO#650 gtaCmpCfg.ClearBeforeFill = True
                    'WO#650 intRtnCde = gtaCmpCfg.Fill(tblCmpCfg, "SelectAllFields", strMyComputerName)
                    gdrCmpCfg = Nothing 'WO#650
                    'WO#871 SharedFunctions.RefreshComputerConfig(strMyComputerName)    'WO#650
                    SharedFunctions.RefreshComputerConfig(gstrMyComputerName)    'WO#871
                    'WO#650 If tblCmpCfg Is Nothing Or tblCmpCfg.Rows.Count = 0 Then
                    If gdrCmpCfg Is Nothing Then    'WO#650
                        MessageBox.Show("The computer has not been configured for the process. Please inform your supervisor.", "** ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return 1
                    Else
                        'WO#650 gdrCmpCfg = tblCmpCfg.Rows(0)
                        'WO#17432 ADD Start
                        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
                            If Not IsNothing(strArg1) Then
                                gstrInterfaceID = strArg1
                            End If
                            If Not IsNothing(strArg2) Then
                                gstrQATWorkFlowType = UCase(strArg2.TrimEnd)
                            End If
                        End If
                        'WO#17432 ADD Stop

                        gdrSessCtl = Nothing 'WO#650
                        SharedFunctions.RefreshSessionControlTable()    'WO#650
                        'WO#650 intRtnCde = gtaSessCtl.Fill(tblSessCtl, "SelectAllFields")
                        'WO#650 If tblSessCtl Is Nothing Or tblSessCtl.Rows.Count = 0 Then
                        If gdrSessCtl Is Nothing Then
                            MessageBox.Show("The Session Control Table has not been set up for the process. Please inform your supervisor.", "** ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return 1
                            'WO#650 Else
                            'WO#650    gdrSessCtl = tblSessCtl.Rows(0)
                            'WO#871 ADD Start
                        ElseIf gdrSessCtl.ComputerName <> gstrMyComputerName Then
                            MessageBox.Show("The computer name in the application does not match with the actual one. Please inform your supervisor.", "** ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return 1
                            'WO#871 ADD Stop
                        End If

                        Cursor.Current = Cursors.Default

                        If RTrim(gdrCmpCfg("Facility")) = "02" Or RTrim(gdrCmpCfg("Facility")) = "20" Then
                            gblnAjaxPlant = True
                        Else
                            gblnAjaxPlant = False
                        End If
                        Select Case strPgmID
                            Case "startshoporder"
                                If gdrSessCtl("ShopOrder") = 0 Then
                                    frm = New frmStartShopOrder
                                Else
                                    MessageBox.Show("Can not start shop order since a shop order has been started already.", "Missing information")
                                    Return 1
                                End If
                            Case "stopshoporder"
                                If gdrSessCtl("ShopOrder") <> 0 Then
                                    frm = New frmStopShopOrder
                                Else
                                    MessageBox.Show("Can not stop shop order since no shop order started.", "Missing information")
                                    Return 1
                                End If
                            Case "printcaselabel"
                                If SharedFunctions.IsSvrConnOK(gblnTrySessCtl) = True Then
                                    If gdrSessCtl("ShopOrder") <> 0 Then
                                        'WO#654 ADD Start
                                        If SharedFunctions.IsSOStartedWithNoLabel() = False Then
                                            Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                                            gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), "")
                                            If tblSO.Rows.Count > 0 Then
                                                drSO = tblSO.Rows(0)   'WO#650
                                                If tblSO.Rows(0)("PrintCaseLabel") <> "Y" Then
                                                    MessageBox.Show("This item will not produce case label.", "For Your Information")
                                                    Return 1
                                                Else
                                                    Try
                                                        'WO#650 ADD Start
                                                        If gblnOvrExpDate = True AndAlso (drSO.DateToPrintFlag = "1" Or drSO.DateToPrintFlag = "3") Then
                                                            frmExpiryDate.ShowDialog()
                                                        End If
                                                        'WO#650 ADD Stop
                                                        SharedFunctions.printCaseLabel(CASELABEL, tblSO.Rows(0)("LotID"))
                                                    Catch ex As SqlClient.SqlException
                                                        MessageBox.Show(gcstSvrCnnFailure & " Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
                                                        Return 1
                                                    End Try
                                                    Return 0
                                                End If
                                            Else
                                                MessageBox.Show("Shop order is not found, can not print case lable.", "Invalid Information")
                                                Return 1
                                            End If
                                            'WO#654 ADD Start
                                        Else
                                            MessageBox.Show("Shop order was started with 'No Case Label', Print Case Label is prohibited.", "Print Case Label is Prohibited.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            Return 0
                                        End If
                                        'WO#654 ADD Stop
                                    Else
                                        MessageBox.Show("Please start a shop order before print case label.", "Invalid Selection")
                                        Return 1
                                    End If
                                Else
                                    MessageBox.Show(gcstSvrCnnFailure & " Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
                                    Return 0
                                End If
                            Case "printlabels_control"
                                SharedFunctions.IsSOStartedWithNoLabel()    'WO#654
                                frm = New frmPrtLabelsAndControl
                            Case "palletlabels"
                                If SharedFunctions.IsSOStartedWithNoLabel() = False Then    'WO#654
                                    frm = New frmPrtPalletLabels
                                Else    'WO#654 ADD Start
                                    MessageBox.Show("Shop Order was started with 'No Case Label', Print Pallet Label is prohibited.", "Invalid Selection")
                                End If
                                'WO#654 ADD Stop
                            Case "createpallet"
                                If gdrSessCtl("ShopOrder") <> 0 Then
                                    'WO#654 ADD Start
                                    'When the shop order was started with no case label loaded, the gblnStartSOWithNoLabel is set to True.
                                    'So even Indusoft program requested to create a pallet, ignore it.
                                    If SharedFunctions.IsSOStartedWithNoLabel() = True Then
                                        MessageBox.Show("Shop Order was started with 'No Case Label', Create Pallet is prohibited.", "Invalid Selection")
                                        Return 0
                                    Else
                                        'WO#654 ADD Stop
                                        frm = New frmCreatePallet
                                    End If
                                Else
                                    MessageBox.Show("Please start a shop order before create pallet.", "Invalid Selection")
                                    Return 1
                                End If
                                ' WO #297 Add Begin
                            Case "autocreatepallet"
                                If gdrSessCtl("ShopOrder") = 0 Then
                                    MessageBox.Show("Please start a shop order before create pallet.", "Invalid Processing sequence")
                                    Return 1
                                ElseIf gdrSessCtl.OverridePkgLine = String.Empty Then
                                    MessageBox.Show("Default Packaging Line can not be blank.", "Invalid Information")
                                    Return 1
                                ElseIf gdrSessCtl.Operator = String.Empty Then
                                    MessageBox.Show("Operator No. is mandatory.", "Invalid Information")
                                    Return 1
                                    'WO#654 ADD Start
                                    'When the shop order was started with no case label loaded, the gblnStartSOWithNoLabel is set to True.
                                    'So even Indusoft program requested to create a pallet, ignore it.
                                ElseIf SharedFunctions.IsSOStartedWithNoLabel() = True Then
                                    Return 0
                                    'WO#654 ADD Stop
                                Else
                                    If strArg1 Is Nothing Then 'WO#15635
                                        SharedFunctions.AutoCreatePallet()
                                        'WO#15635 ADD Start
                                    Else
                                        If Not Integer.TryParse(strArg1, IntQtyInPallet) Then
                                            MessageBox.Show("The Qty. In Pallet is not a valid integer", "Invalid Information")
                                            Return 1
                                        Else
                                            SharedFunctions.AutoCreatePallet(gdrSessCtl, gdrCmpCfg, IntQtyInPallet, Nothing, 0, 0)
                                        End If
                                    End If
                                    'WO#15635 ADD Stop
                                    Return 0
                                End If

                                ' WO #297 Add End
                            Case "logon"
                                If gdrCmpCfg("PalletStation") = True Then
                                    frm = New frmLogOn
                                Else
                                    MessageBox.Show("This computer is not configured for Pallet Station.", "Please Contact Supervisor")
                                    Return 1
                                End If
                            Case "mainmenu"
                                frm = New frmMainMenu
                            Case "logon_noplc"
                                frm = New frmLogOn_NoPLC
                            Case "inquiry"
                                frm = New frmInquiry
                            Case "bom"
                                If gdrSessCtl("ShopOrder") <> 0 Then
                                    gstrCurrentShopOrder = gdrSessCtl("ShopOrder")
                                    frm = New frmBillOfMaterials
                                Else
                                    MessageBox.Show("Can not view Bill Of Material since no shop order started.", "Missing information")
                                    Return 1
                                End If
                            Case "shopordernotes"
                                If gdrSessCtl("ShopOrder") <> 0 Then
                                    gstrCurrentShopOrder = gdrSessCtl("ShopOrder")
                                    frm = New frmShopOrderNotes
                                Else
                                    MessageBox.Show("Can not view Shop Order Notes since no shop order started.", "Missing information")
                                    Return 1
                                End If
                            Case "tscontrolpanel"   'Touch Screen Control Panel
                                SharedFunctions.ShowTSCtlPnl()
                                Return 1
                            Case "logscrap"
                                If gdrSessCtl("ShopOrder") <> 0 Then
                                    frm = New frmLogScraps
                                Else
                                    MessageBox.Show("Can not log scrap since no shop order started.", "Missing information")
                                    Return 1
                                End If
                                'FX150505 ADD Start
                                'FX20170119    Case "ShutDown"             
                            Case "shutdown"             'FX20170119
                                frm = New frmShutDown
                                'FX150505 ADD END
                                'WO#17432 ADD Start
                            Case "qat_startup"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATStartUpChecks
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_lineclearance"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATLineClearance
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_material"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATMaterialValidation
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_weight"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATSmallestUnitWeight
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_pressure"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATPressure
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_shakeshine"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATShakeShine
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_lidpeel"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATLidPeel
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_oxygen"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATOxygen
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_packagedatecoder"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATPackageDateCoder
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_camera"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATCamerasVerification
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_reject"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATRejectResult
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_checkweigher"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATCheckWeigherValidation
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_cartondatecoder"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATCartonDateCoder
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_cartonbox"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATCartonBoxVisualCheck
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_casedatecoder"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATCaseDateCoder
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_case"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATCaseVisualVerification
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                            Case "qat_selectpallettype"
                                If gdrSessCtl.ShopOrder <> 0 Then
                                    frm = New frmQATSelectPalletType
                                Else
                                    MessageBox.Show("Please start a shop order before run QAT.", "Invalid Selection")
                                    Return 1
                                End If
                                'Case "testcw"
                                '    frm = New frmTestCheckweigher
                                'WO#17432 ADD Stop
                            Case Else
                                MessageBox.Show("Unknown Menu Option, please inform your supervisor", "** ERROR")
                                Return 1
                        End Select

                    frm.ShowDialog()

                    Return 0
                End If
                Else
                    MessageBox.Show("The program is missing parameters to execute.", "Missing Information")
                    Return (1)
                End If
            End If
        Catch ex As System.Data.SqlClient.SqlException
            Dim strLogFileName As String = String.Format(My.Application.Info.DirectoryPath & "\IPCErrorLog.txt")
            SharedFunctions.WriteLog(strLogFileName, ex.Message)
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor")
            Return (1)
        Catch ex As Exception
            Dim strLogFileName As String = String.Format(My.Application.Info.DirectoryPath & "\IPCErrorLog.txt")
            SharedFunctions.WriteLog(strLogFileName, ex.Message)
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor")
            Return (1)
        Finally
            Cursor.Current = Cursors.Default
        End Try

    End Function

End Module
