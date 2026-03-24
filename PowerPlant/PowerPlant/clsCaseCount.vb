'WO#755 ADD Start
Public Class Counter
    Private thd As System.Threading.Thread
    Private intLastCaseCount As Integer
    Private intCurrCaseCount As Integer 'contain total cases produced of the shop order
    Private blnCounterIsStarted As Boolean

    Private intLooseCases As Integer
    Private tblSessCtl As New dsSessionControl.CPPsp_SessionControlIODataTable
    Private taSessCtl As New dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
    Private drSessCtl As dsSessionControl.CPPsp_SessionControlIORow
    Private intInitialSOProduced As Integer
    'Private intInitialSOPalletProduced As Integer
    Private intInitialShiftProduced As Integer
    Private intIntialSOCFCases As Integer
    Private intInitialQtyFromPallets As Integer
    Private intTotalQtyFromPallet As Integer
    Private intInitialShiftAdjustment As Integer
    Private blnRefreshMainMenu As Boolean
    Private intMessageCode As Integer                               'WO#3686
    Private blnCrtPallet As Boolean                                 'WO#3686
    Private intCasesFromCreatedPallets As Integer                   'WO#3686

    Private gstrFacility As String
    Private gintQtyPerPallet As Integer
    Private gstrSOStatus As String
    Private gdtDateTime As DateTime
    Private gintShift As Integer
    Private gintShopOrder As Integer
    Private gstrOperator As String
    Private gstrPkgLine As String
    Private gdteShiftProductionDate As DateTime                     'WO#3686
    Private gstrInterfaceType As String                             'WO#5370


    Public ReadOnly Property CounterIsStarted() As Boolean
        Get
            Return blnCounterIsStarted
        End Get
    End Property

    Public Property RefreshMainMenu() As Boolean
        Get
            Return blnRefreshMainMenu
        End Get

        Set(value As Boolean)
            blnRefreshMainMenu = value
        End Set
    End Property

    Public ReadOnly Property CaseCountInPallet() As Integer
        Get
            'In here, the current case count already includes the carried forward cases. The Total quantity from Pallet includes adjustment.
            Return (intCurrCaseCount - intTotalQtyFromPallet)
        End Get
    End Property

    Public ReadOnly Property CasesProducedRunningTotal() As Integer
        Get
            Return intCurrCaseCount
        End Get
    End Property

    Public ReadOnly Property CasesProducedInShift() As Integer
        Get
            Return intInitialShiftProduced + (intCurrCaseCount - (intInitialSOProduced - intIntialSOCFCases))
        End Get
    End Property

    Public ReadOnly Property CasesProducedInSession() As Integer
        Get
            Return intCurrCaseCount - (intInitialSOProduced - intIntialSOCFCases)
        End Get
    End Property

    Public ReadOnly Property PalletProducedInSession() As Integer
        Get
            Return drSessCtl.PalletsCreated
        End Get
    End Property

    Public ReadOnly Property InitialSOCasesProduced() As Integer
        Get
            Return intInitialSOProduced
        End Get
    End Property

    'Public ReadOnly Property InitialShiftCasesProduced() As Integer
    '    Get
    '        Return intInitialShiftProduced
    '    End Get
    'End Property

    Public ReadOnly Property InitialCaseCountInPallet() As Integer
        Get
            'Return (intInitialSOProduced - intCFCases) Mod intQtyPerPallet
            Return intInitialSOProduced - intInitialQtyFromPallets
        End Get
    End Property

    'WO#3686 ADD Start
    Public ReadOnly Property CasesFromCreatedPallets() As Integer
        Get
            Return intTotalQtyFromPallet
        End Get
    End Property
    'WO#3686 ADD Stop

    Public WriteOnly Property Facility() As String
        Set(value As String)
            gstrFacility = value
        End Set
    End Property

    Public WriteOnly Property ShopOrder() As Integer
        Set(value As Integer)
            gintShopOrder = value
        End Set
    End Property

    Public WriteOnly Property QtyPerPallet() As Integer
        Set(value As Integer)
            gintQtyPerPallet = value
        End Set
    End Property

    Public WriteOnly Property SOStatus() As String
        Set(value As String)
            gstrSOStatus = value
        End Set
    End Property

    Public WriteOnly Property Shift() As Integer
        Set(value As Integer)
            gintShift = value
        End Set
    End Property

    Public WriteOnly Property DateTime() As DateTime
        Set(value As DateTime)
            gdtDateTime = value
        End Set
    End Property

    Public WriteOnly Property OperatorID() As String
        Set(value As String)
            gstrOperator = value
        End Set
    End Property

    Public WriteOnly Property PkgLine() As String
        Set(value As String)
            gstrPkgLine = value
        End Set
    End Property

    'WO#3686 ADD Start
    Public Property MessageCode() As Integer
        Get
            Return intMessageCode
        End Get
        Set(ByVal value As Integer)
            intMessageCode = value
        End Set
    End Property

    Public Property CrtPallet() As Boolean
        Get
            Return blnCrtPallet
        End Get
        Set(ByVal value As Boolean)
            blnCrtPallet = value
        End Set
    End Property
    'WO#3686 ADD Stop
    'WO#5370 ADD Start
    Public Property InterfaceType() As String
        Get
            Return gstrInterfaceType
        End Get
        Set(value As String)
            gstrInterfaceType = value
        End Set
    End Property
    'WO#5370 ADD Stop

    Public Sub New()
        ClassInitialization()
    End Sub

    'FIX20161214    Public Sub New(Facility As String, ShopOrder As Integer, QtyPerPallet As Integer, SOStatus As String, _
    'FIX20161214           DateTime As DateTime, Shift As Integer, OperatorID As String)
    'WO#3686    Public Sub New(Facility As String, ShopOrder As Integer, QtyPerPallet As Integer, SOStatus As String, _
    'WO#3686       DateTime As DateTime, Shift As Integer, OperatorID As String, Packagingline As String)                       'FIX20161214
    'WO#5370    Public Sub New(Facility As String, ShopOrder As Integer, QtyPerPallet As Integer, SOStatus As String, _
    'WO#5370        DateTime As DateTime, Shift As Integer, OperatorID As String, Packagingline As String, ShiftProductionDate As DateTime)      'WO#3686
    Public Sub New(Facility As String, ShopOrder As Integer, QtyPerPallet As Integer, SOStatus As String, _
               DateTime As DateTime, Shift As Integer, OperatorID As String, Packagingline As String, _
               ShiftProductionDate As DateTime, strinterfaceType As String)      'WO#5370
        gintShopOrder = ShopOrder
        gintQtyPerPallet = QtyPerPallet
        gstrFacility = Facility
        gstrSOStatus = SOStatus
        gstrOperator = OperatorID
        gdtDateTime = DateTime
        gintShift = Shift
        gstrPkgLine = Packagingline                         'FIX20161214
        gdteShiftProductionDate = ShiftProductionDate       'WO#3686
        gstrInterfaceType = strinterfaceType                'WO#5370 
        ClassInitialization()
    End Sub

    Private Sub ClassInitialization()
        intLastCaseCount = 0
        intMessageCode = 0                              'WO#3686
        blnCrtPallet = 0                                'WO#3686
        blnCounterIsStarted = False
        ReadSessionControlTable()

        'Initiailize cases and pallet produced for the shift and the whole shop order.
        InitializeSOCasesProducedInfo()
        thd = New System.Threading.Thread(AddressOf DoSomeWork)
    End Sub

    Public Sub Start(blnSOStarted As Boolean)
        'If blnSOStarted = True Then
        'intCurrCaseCount = drSessCtl.CasesProduced
        'Since the shop order has been started, the drSessCtl.CasesProduced should contain the total SO cases produced. 
        'intInitialSOProduced = drSessCtl.CasesProduced
        'Since the shop order has been started so that ..
        '1. intInitialSOProduced is shop order cases produced up to last session
        '2. intInitialShiftProduced is the cases produced in the current shift up to last session
        '3. drSessCtl.CasesProduced temporary contains the total case count of the shop order
        '=> drSessCtl.CasesProduced - intInitialSOProduced = the cases produced in the current session
        'intInitialShiftProduced = (drSessCtl.CasesProduced - intInitialSOProduced) + intInitialShiftProduced
        'intPalletCountATSessionStart = CType(intCaseCountAtSessionStart / intQtyPerPallet, Integer)
        'End If
        blnCounterIsStarted = True
        thd.Start()
    End Sub

    Public Sub Abort()
        blnCounterIsStarted = False
        Threading.Thread.Sleep(1000)
        thd.Abort()
    End Sub

    Private Sub DoSomeWork()
        Dim intTempCaseCountInPallet As Integer                                                     'WO#3686
        'WO#5370 ADD Start
        Dim daUnitCountInBound As New dsUnitCountInboundTableAdapters.PPsp_UnitCountInbound_SelTableAdapter
        Dim dtUnitcountINbound As New dsUnitCountInbound.PPsp_UnitCountInbound_SelDataTable
        Dim drUnitcountINbound As dsUnitCountInbound.PPsp_UnitCountInbound_SelRow
        Dim strOutputLocation As String
        Dim intDestinationShopOrder As Integer
        'WO#5370 ADD Stop

        Do
            If drSessCtl.ShopOrder <> 0 Then
                intCurrCaseCount = drSessCtl.CaseCounter

                'WO#3686 If intLastCaseCount <> intCurrCaseCount Then
                If intLastCaseCount <> intCurrCaseCount And intMessageCode = 0 Then                 'WO#3686

                    If gintAutoCountByUnit = AutoCountBy.Cases Then                                 'WO#5370
                        'create pallet is the pallet is full
                        If CaseCountInPallet >= gintQtyPerPallet Then
                            Try
                                'WO#3686 ADD Start
                                If CaseCountInPallet >= (2 * gintQtyPerPallet) Then
                                    intMessageCode = 1      'trigger to prompt an message box on Main Screen
                                End If

                                If blnCrtPallet = True Then
                                    'Result from the prompt an message box on Main Screen is "Yes" to continue to create pallet
                                    intMessageCode = 0
                                    blnCrtPallet = False
                                End If

                                If intMessageCode = 0 Then
                                    intTempCaseCountInPallet = CaseCountInPallet
                                    Do Until intTempCaseCountInPallet < gintQtyPerPallet
                                        'WO#3686 ADD Stop
                                        SharedFunctions.AutoCreatePallet()
                                        'WO#3686 ADD Start
                                        intTempCaseCountInPallet = intTempCaseCountInPallet - gintQtyPerPallet
                                    Loop
                                End If
                                'WO#3686 ADD Stop
                                'The pallet count in session will be maintained and updated from the ProcessFrmCreatePallet routine.
                            Catch ex As Exception
                                SharedFunctions.PoPUpMSG(ex.Message & vbCrLf & "** Contact Supervisor **", "Error On AutoCreatePallet", MessageBoxButtons.OK)
                            End Try
                        End If
                        'WO#5370 ADD Start 
                        'If it is Sarong line with auto count enabled then create the pallet based on the information from tblUnitCountInBound.
                        'If the destination output location is changed from PLC, after use the current information to create one more pallet and then change 
                        'the current information from tblUnitCountInBound for next pallet creation.
                    ElseIf gblnSarongAutoCountLine = True Then
                        If gblnSvrConnIsUp = True Then
                            Try
                                'WO#37864 ADD start
                                If gdrCmpCfg.PkgLineType = "Sarong" Then
                                    strOutputLocation = "DF"
                                Else
                                    strOutputLocation = Nothing
                                End If
                                daUnitCountInBound.Fill(dtUnitcountINbound, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, gdrSessCtl.StartTime, gdrSessCtl.ShopOrder, 1, Nothing, strOutputLocation)
                        'WO#37864 ADD stop
                                'WO#37864   daUnitCountInBound.Fill(dtUnitcountINbound, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, gdrSessCtl.StartTime, gdrSessCtl.ShopOrder, 1, Nothing, "DF")
                            Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True
                            SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            SharedFunctions.AddMessageLineToForm(Form.ActiveForm, gcstSvrCnnFailure)
                        End Try
                        If dtUnitcountINbound.Rows.Count > 0 Then
                            For Each drUnitcountINbound In dtUnitcountINbound
                                    If drUnitcountINbound.OrderChange = "DestLine" Then
                                        strOutputLocation = gstrLastOutputLocation
                                        intDestinationShopOrder = gintLastDestinationShopOrder
                                    Else
                                        strOutputLocation = drUnitcountINbound.OutputLocation
                                        intDestinationShopOrder = drUnitcountINbound.DestinationShopOrder
                                    End If
                                SharedFunctions.AutoCreatePallet(gdrSessCtl, gdrCmpCfg, drUnitcountINbound.UnitCount, strOutputLocation, _
                                                                 intDestinationShopOrder, drUnitcountINbound.TxID)
                                If drUnitcountINbound.OrderChange = "DestLine" Then
                                    gstrLastOutputLocation = drUnitcountINbound.OutputLocation
                                    gintLastDestinationShopOrder = drUnitcountINbound.DestinationShopOrder
                                    Using qta As New dsUnitCountOutBoundTableAdapters.QueriesTableAdapter
                                        With gdrSessCtl
                                            Try
                                                qta.PPsp_InitializeUnitCountOutbound(gdrCmpCfg.ComputerName, Now, .Facility, .ItemNumber, gdrSessCtl.Operator, Nothing, _
                                                     .DefaultPkgLine, .OverrideShiftNo, .ShopOrder, .StartTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), 0, gintQtyPerPallet)
                                            Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True
                                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                                                SharedFunctions.AddMessageLineToForm(Form.ActiveForm, gcstSvrCnnFailure)
                                            End Try
                                        End With
                                    End Using
                                End If
                            Next
                        End If
                        End If
                    End If

                    If intMessageCode = 0 And gblnSvrConnIsUp = True Then
                        'WO#5370 ADD Stop
                        'WO#5370 If intMessageCode = 0 Then          'WO#3686
                        intLastCaseCount = intCurrCaseCount
                    End If                              'WO#3686
                End If
            End If

            'Wait for a while before check the case counter again.
            Threading.Thread.Sleep(My.Settings.gintCaseCountInterval)

            ReadSessionControlTable()

        Loop Until blnCounterIsStarted = False

    End Sub

    Private Sub InitializeSOCasesProducedInfo()

        Dim dtSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim drSCH As dsSessionControlHst.CPPsp_SessionControlHstIORow

        Dim blnFirstTime As Boolean = True
        Dim dtPA As New dsPalletsOnServer.PPsp_Pallet_SelDataTable
        Dim taPA As New dsPalletsOnServerTableAdapters.PPsp_Pallet_SelTableAdapter
        Dim drPA As dsPalletsOnServer.PPsp_Pallet_SelRow
        Dim intSOAdjustment As Integer
        Dim intSOCaseProduced As Integer
        Dim intSOPalletProduced As Integer
        ' Dim intSOPalletProducedAdjustment As Integer
        Dim dvSCH As DataView
        Dim dteShiftProductionDate As DateTime

        Try
            'WO#3686    With drSessCtl
            'Read each session control history record of the shop by shop order starting time sequance
            'WO#5370    dtSCH = SharedFunctions.GetSessionControlHst("BySoLine", Nothing, gintShopOrder, 0, Now, Now, gstrFacility, String.Empty)
            dtSCH = SharedFunctions.GetSessionControlHst("BySoLine", gstrPkgLine, gintShopOrder, 0, Now, Now, gstrFacility, String.Empty)               'WO#5370
            If dtSCH.Rows.Count > 0 Then
                dvSCH = dtSCH.DefaultView
                dvSCH.Sort = "StartTime ASC"

                dteShiftProductionDate = SharedFunctions.GetProductionDateByShift(gintShift, gdtDateTime)
                For Each drvSCH As DataRowView In dvSCH
                    drSCH = CType(drvSCH.Row, dsSessionControlHst.CPPsp_SessionControlHstIORow)
                    If blnFirstTime Then
                        intIntialSOCFCases = drSCH.CarriedForwardCases
                        blnFirstTime = False
                    End If
                    intSOCaseProduced = intSOCaseProduced + drSCH.CasesProduced
                    intSOPalletProduced = intSOPalletProduced + drSCH.PalletsCreated

                    'For Initialize Case Produced in entered shift, ccumulate the CasesProduced on each session control history record of the shop order by operator and entered production shift 
                    'usually the running shift does not has the pallet adjustment record yet.
                    If drSCH._Operator = gstrOperator _
                        AndAlso drSCH.ShiftProductionDate = dteShiftProductionDate _
                        AndAlso drSCH.OverrideShiftNo = gintShift _
                        AndAlso drSCH.OverridePkgLine = gstrPkgLine _
                        Then
                        intInitialShiftProduced = intInitialShiftProduced + drSCH.CasesProduced
                    End If

                Next

            End If

            intInitialShiftProduced = intInitialShiftProduced + drSessCtl.CasesProduced                                         'WO#5370


            'taPA.Fill(dtPA, "AllPalletsWithReasonAdj", .Facility, Nothing, .DefaultPkgLine, gintShopOrder, Nothing, Nothing, Nothing)
            'WO#3686    taPA.Fill(dtPA, "AllPalletsWithReasonAdj", .Facility, Nothing, Nothing, gintShopOrder, Nothing, Nothing, Nothing)
            'WO#5370    taPA.Fill(dtPA, "AllPalletsWithReasonAdj", gstrFacility, Nothing, Nothing, gintShopOrder, Nothing, Nothing, Nothing)       'WO#3686
            taPA.Fill(dtPA, "AllPalletsWithReasonAdj", gstrFacility, Nothing, gstrPkgLine, gintShopOrder, Nothing, Nothing, Nothing)       'WO#5370
            If dtPA.Rows.Count > 0 Then

                For Each drPA In dtPA
                    If drPA.TransactionReasonCode <> "" Then
                        'Get Pallet Adjustments of the shop order 
                        If drPA.AffectBPCS = True Then
                            intInitialQtyFromPallets = intInitialQtyFromPallets + drPA.Quantity + drPA.AdjustedQty
                        End If
                        If drPA.AffectPowerPlant = True Then
                            intSOAdjustment = intSOAdjustment + drPA.AdjustedQty
                            'WO#3686    If drPA.Operator = .Operator AndAlso drPA.ShiftProductionDate = .ShiftProductionDate AndAlso drPA.ShiftNo = .OverrideShiftNo Then
                            If drPA.Operator = gstrOperator AndAlso drPA.ShiftProductionDate = gdteShiftProductionDate AndAlso drPA.ShiftNo = gintShift Then        'WO#3686
                                intInitialShiftAdjustment = intInitialShiftAdjustment + drPA.AdjustedQty
                            End If
                        End If

                        ''Only adjust the pallet count if the adjusted cases quantity = quantity in the pallet
                        'If drPA.Quantity = drPA.AdjustedQty Then
                        '    intSOPalletProducedAdjustment = intSOPalletProducedAdjustment - 1
                        'End If
                    Else
                        intInitialQtyFromPallets = intInitialQtyFromPallets + drPA.Quantity
                    End If
                Next
            End If
            'WO#3686    End With

            If gstrSOStatus = "Started" Then
                intInitialSOProduced = drSessCtl.CaseCounter
            Else
                'intInitialSOProduced = 0
                'Dim dtSCHLastRcd As dsSessionControlHst.CPPsp_SessionControlHstIODataTable
                'Dim drSCHLastRcd As dsSessionControlHst.CPPsp_SessionControlHstIORow
                'dtSCHLastRcd = SharedFunctions.GetSectionControlHst("SelectLastRecord", drSessCtl.DefaultPkgLine, Nothing, Nothing, Nothing, Nothing, drSessCtl.Facility, Nothing)
                'If dtSCHLastRcd.Rows.Count > 0 Then
                '    drSCHLastRcd = dtSCHLastRcd.Rows(0)
                '    If drSCHLastRcd.ShopOrder = intShopOrder Then
                '        intInitialSOProduced = drSessCtl.CasesProduced
                '    Else

                'If the shop order has not been started, aggregate the cases produced of the shop order from the session control history
                If gintAutoCountByUnit = AutoCountBy.Cases Then   'WO#5370
                    intInitialSOProduced = intSOCaseProduced + intSOAdjustment + intIntialSOCFCases
                    'WO#5370 ADD Start
                Else
                    intInitialSOProduced = intInitialQtyFromPallets
                End If
                'WO#5370 ADD End

                taSessCtl.UpdateCaseCounter(intInitialSOProduced)
                ReadSessionControlTable()
                'End If
                '    End If
            End If

            intTotalQtyFromPallet = intInitialQtyFromPallets
            intCurrCaseCount = intInitialSOProduced
            intInitialShiftProduced = intInitialShiftProduced + intInitialShiftAdjustment
            'intInitialSOPalletProduced = intSOPalletProduced + intSOPalletProducedAdjustment
            blnRefreshMainMenu = True

        Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231)
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub PostCreatePallet(ByVal intPalletQty As Integer)
        intTotalQtyFromPallet = intTotalQtyFromPallet + intPalletQty
        blnRefreshMainMenu = True
    End Sub

    Private Sub ReadSessionControlTable()
        Try
            taSessCtl.Fill(tblSessCtl, "SelectAllFields")
            If Not IsNothing(tblSessCtl) And tblSessCtl.Rows.Count > 0 Then
                drSessCtl = tblSessCtl.Rows(0)
            End If
        Catch ex As Exception
            Throw New Exception("Error in CheckSessionControlTable" & vbCrLf & ex.Message)
        End Try
    End Sub

    'WO#3686 ADD Start
    Private Function GetCasesFromCreatedPallets() As Integer
        Dim intCasesFromCreatedPallets As Integer
        Dim dtPA As New dsPalletsOnServer.PPsp_Pallet_SelDataTable
        Dim taPA As New dsPalletsOnServerTableAdapters.PPsp_Pallet_SelTableAdapter
        Dim drPA As dsPalletsOnServer.PPsp_Pallet_SelRow

        taPA.Fill(dtPA, "AllPalletsWithReasonAdj", gstrFacility, Nothing, Nothing, gintShopOrder, Nothing, Nothing, Nothing)

        If dtPA.Rows.Count > 0 Then

            For Each drPA In dtPA
                If drPA.TransactionReasonCode <> "" Then
                    'Get Pallet Adjustments of the shop order 
                    If drPA.AffectBPCS = True Then
                        intCasesFromCreatedPallets = intInitialQtyFromPallets + drPA.Quantity + drPA.AdjustedQty
                    End If
                Else
                    intCasesFromCreatedPallets = intInitialQtyFromPallets + drPA.Quantity
                End If
            Next
        End If
        Return intCasesFromCreatedPallets
    End Function
    'WO#3686 ADD Stop

End Class
'WO#755 ADD Stop
