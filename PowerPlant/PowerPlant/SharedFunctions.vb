' WO#297 Palletizer Signal
' - Added: function CloseShopOrder() and subroutine AutoCreatePallet()
' - Modified: function ImportMasterFile

Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Xml  'WO#654
Imports System.IO   'WO#654
Imports Microsoft.Win32 'WO#755
Imports System.Security.Cryptography 'WO#17432 
Imports System.Linq

Public Class SharedFunctions
    'WO#17432 DEL Start
    'Const intFormWidth As Short = 800
    'Const intFormHeight As Short = 600
    'WO#17432 DEL Stop
    'V6.78 Creator of pallets - Add Models

    Shared Property _dbServer As New ServerModels.PowerPlantEntities()
    'Dim resultServerDetails As New List(Of ServerModels.tblPalletExt)()
    Shared Property _dbLocal As New LocalModels.PowerPlantLocalEntities()
    'Dim resultLocalDetails As New List(Of LocalModels.tblPalletExt)()

    'WO#297 ADD Begin
    Public Enum CallingFrom
        StopShopOrder
        CreatePallet
        UploadToServer
    End Enum

    Public Enum ShopOrderStatus
        Open
        Closed
    End Enum
    'WO#297 ADD End

    'WO#17432 DEL Start
    'WO#718 ADD Start
    'Public Enum DBLocation
    '    Server
    '    Local
    '    Server_Local
    'End Enum
    'WO#718 ADD Stop
    'WO#17432 DEL Stop

    'WO#5370 ADD Start
    'Public Enum AutoCountBy
    '    Cases
    '    Pallets
    'End Enum
    'WO#5370 ADD Stop

    Private Sub New()
    End Sub

    'Public Shared Function PopNumKeyPad(ByVal frm As Form, ByVal ctl As Object, Optional ByVal intRowIdx As Integer = 0, Optional ByVal intColIdx As Integer = 0) As DialogResult  'WO#17432
    'WO#17432 ADD Start
    Public Shared Function PopNumKeyPad(ByVal frm As Form, ByVal ctl As Object, Optional ByVal intRowIdx As Integer = 0, Optional ByVal intColIdx As Integer = 0,
        Optional ByVal strPasswordChar As String = "") As DialogResult
        'WO#17432 ADD Stop
        Dim ptCtlStartPoint As New Point
        Dim ptFormLocation As New Point
        Dim intX As Integer
        Dim intY As Integer
        Dim KeyPad As New frmNumKeyPad

        'WO#17432 ADD Start
        Const intFormWidth As Short = 800
        Const intFormHeight As Short = 600
        'WO#17432 ADD Stop

        ptFormLocation = frm.Location
        ptCtlStartPoint = ctl.Location
        intX = ptFormLocation.X + ptCtlStartPoint.X
        intY = ptFormLocation.Y + ptCtlStartPoint.Y + ctl.Height

        ' if the key pad is outside the right boundary of the form, set X = form width - keypad width
        If intX + KeyPad.Size.Width > intFormWidth Then
            intX = intFormWidth - KeyPad.Size.Width
        End If

        ' if the key pad is outside the bottom boundary of the form, set X = form height - keypad height
        If intY + KeyPad.Size.Height > intFormHeight Then
            intY = intFormHeight - KeyPad.Size.Height
        End If
        KeyPad.txtDisplay.Text = RTrim(ctl.Text)
        KeyPad.StartPosition = FormStartPosition.Manual
        KeyPad.Location = New Point(intX, intY)

        KeyPad.PasswordChar = strPasswordChar       'WO#17432 
        PopNumKeyPad = KeyPad.ShowDialog()

    End Function

    'WO#17432   Public Shared Function PopAlphaNumKB(ByVal frm As Form, ByVal ctl As Control) As DialogResult
    'WO#17432 ADD Start
    Public Shared Function PopAlphaNumKB(ByVal frm As Form, ByVal ctl As Control, Optional ByVal strPasswordChar As String = "") As DialogResult
        'WO#17432 ADD Stop
        Dim ptCtlStartPoint As New Point
        Dim ptFormLocation As New Point
        Dim intX As Integer
        Dim intY As Integer
        Dim KeyPad As New frmAlphaNumKB

        'WO#17432 ADD Start
        Const intFormWidth As Short = 800
        Const intFormHeight As Short = 600
        'WO#17432 ADD Stop

        ptFormLocation = frm.Location
        ptCtlStartPoint = ctl.Location
        intX = ptFormLocation.X + ptCtlStartPoint.X
        'intY = ptFormLocation.Y + ptCtlStartPoint.Y + ctl.Height + 30
        intY = ptFormLocation.Y + ptCtlStartPoint.Y + ctl.Height

        ' if the key pad is outside the right boundary of the form, set X = form width - keypad width
        'If intX + KeyPad.Size.Width > frm.Size.Width Then
        '    intX = frm.Size.Width - KeyPad.Size.Width
        If intX + KeyPad.Size.Width > intFormWidth Then
            intX = intFormWidth - KeyPad.Size.Width
        End If

        ' if the key pad is outside the bottom boundary of the form, set X = form height - keypad height
        'If intY + KeyPad.Size.Height > frm.Size.Height Then
        '    intY = frm.Size.Height - KeyPad.Size.Height
        If intY + KeyPad.Size.Height > intFormHeight Then
            intY = intFormHeight - KeyPad.Size.Height
        End If
        KeyPad.txtDisplay.Text = RTrim(ctl.Text)
        KeyPad.Location = New Point(intX, intY)
        KeyPad.StartPosition = FormStartPosition.Manual
        KeyPad.PasswordChar = strPasswordChar       'WO#17432

        PopAlphaNumKB = KeyPad.ShowDialog()

    End Function

    Public Shared Sub ClearInputFields(ByVal frm As Form)
        Dim ctl As Control
        For Each ctl In frm.Controls
            If TypeOf ctl Is TextBox Then
                ctl.Text = ""
            End If
        Next
    End Sub
    Public Shared Sub ClearLabelsText(ByVal frm As Form)
        Dim ctl As Control
        For Each ctl In frm.Controls
            If TypeOf ctl Is Label AndAlso Left(ctl.Name, 3) = "lbl" Then
                ctl.Text = ""
            End If
        Next
    End Sub

    'WO#650 Public Shared Sub StartShopOrderUpdate(ByVal frm As frmStartShopOrder, ByVal intShopOrder As Integer, ByVal shtExpectedShift As Short, ByVal strPrintCaseLabel As String, _
    'WO#650                                       ByVal strLotID As String)
    'WO#654 Public Shared Sub StartShopOrderUpdate(ByVal frm As frmStartShopOrder, ByVal intShopOrder As Integer, ByVal shtExpectedShift As Short, ByVal strPrintCaseLabel As String, _
    'WO#654                               ByVal strLotID As String, ByVal dteExpiryDate As DateTime)   'WO#650
    Public Shared Sub StartShopOrderUpdate(ByVal frm As frmStartShopOrder, ByVal intShopOrder As Integer, ByVal shtExpectedShift As Short, ByVal strPrintCaseLabel As String,
                  ByVal strLotID As String, ByVal dteExpiryDate As DateTime, ByVal blnStartSOWithNoLabel As Boolean)   'WO#654


        Dim arParms() As SqlParameter
        Dim strCtlName As String
        Dim strUtilityTechCtlName As String                             'WO#17432 - BL 3/28/2019
        Dim strSQLStmt As String
        Dim dteSOStartTime As DateTime = Now
        Dim dteProductionDate As DateTime
        Dim strProductionDate As String = ""
        'Dim blnSvrConnFailure As Boolean = False
        Dim cnnServer As New SqlConnection(gstrServerConnectionString)
        Dim cnnLocal As New SqlConnection(gstrLocalDBConnectionString)
        Dim trnServer As SqlTransaction = Nothing
        Dim strDefaultPkgLine As String
        Dim i As Int16

        cnnLocal.Open()

        If gblnSvrConnIsUp = True Then
            Try
                cnnServer.Open()
                trnServer = cnnServer.BeginTransaction()
            Catch ex As SqlException
                SetServerCnnStatusInSessCtl(False)
            End Try
        End If

        Try
            'WO#17432 ADD Start - BL 3/28/2019
            'Clear the QAT Tester table first and insert the current operator information to the table
            If gdrCmpCfg.QATWorkFlowInitiation <> QATWorkFlow.Disabled Then
                UpdateQATTesters(frm.txtOperator.Text, frm.lblOperator.Text, True)
            End If
            'WO#17432 ADD Stop - BL 3/28/2019

            'Insert Utility Techicians to table
            For i = 1 To 4
                strCtlName = "txtUtilityTech" + CStr(i)
                If frm.gbxUtilityTech.Controls.Item(strCtlName).Text <> "" Then
                    SharedFunctions.UpdateOperationStaffing(gdrSessCtl("DefaultPkgLine"), dteSOStartTime, gdrCmpCfg("Facility"),
                                    frm.gbxUtilityTech.Controls.Item(strCtlName).Text, trnServer)
                    'WO#17432 ADD Start - BL 3/28/2019
                    If gdrCmpCfg.QATWorkFlowInitiation <> QATWorkFlow.Disabled Then
                        strUtilityTechCtlName = "lblUtilityTech" + CStr(i)
                        UpdateQATTesters(frm.gbxUtilityTech.Controls.Item(strCtlName).Text, frm.gbxUtilityTech.Controls.Item(strUtilityTechCtlName).Text, False)
                        'WO#17432 ADD Stop - BL 3/28/2019
                    End If
                End If
            Next

            'Get production date by shift
            'dteProductionDate = SharedFunctions.GetProductionDateByShift(CType(frm.txtShift.Text, Integer))
            dteProductionDate = Now()
            If IsDate(dteProductionDate) Then
                strProductionDate = Format(dteProductionDate, "yyyyMMdd")
            End If

            ' If the overridden packaging line is it's associated line, 
            ' change the default line with the overridden packaging line id
            strDefaultPkgLine = GetSessionDefaultPkgLine(gdrCmpCfg.Facility, gdrSessCtl.DefaultPkgLine, frm.txtPkgLine.Text)

            'Print labels
            If gblnSvrConnIsUp = True Then
                Try
                    'Dim oPrintDevice As New PrinterDevice(gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"))
                    Dim oPrintDevice As New PrinterDevice(gdrSessCtl("Facility"), strDefaultPkgLine)

                    'Clear label data
                    'ClearLabelData(frm.txtPkgLine.Text, dteSOStartTime, gdrSessCtl("Facility"))
                    ClearLabelData(strDefaultPkgLine, dteSOStartTime, gdrSessCtl("Facility"))

                    'Create case label to label source file and print it         
                    'WP#650 If strPrintCaseLabel = "Y" Then

                    'CreateLabelData(gdrSessCtl("Facility"), CASELABEL, CASELABELER, frm.txtPkgLine.Text, _
                    '                frm.txtShopOrder.Text, frm.lblItemNo.Text, 0, 0, frm.txtOperator.Text, _
                    '                frm.txtPkgLine.Text & frm.txtShopOrder.Text, strLotID, strProductionDate, trnServer)

                    'CreatePrintRequest(CASELABEL, gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"), CASELABELER, _
                    '                dteSOStartTime, frm.txtPkgLine.Text & frm.txtShopOrder.Text, , trnServer)
                    'WO#654 If oPrintDevice.HasCasePrinter Then
                    'FX20200708 If oPrintDevice.HasCasePrinter AndAlso blnStartSOWithNoLabel = False Then 'WO#654
                    If oPrintDevice.HasCasePrinter AndAlso blnStartSOWithNoLabel = False AndAlso strPrintCaseLabel = "Y" Then 'FX20200708
                        CreateLabelData(gdrSessCtl("Facility"), CASELABEL, CASELABELER, strDefaultPkgLine, strDefaultPkgLine,
                                        intShopOrder, frm.lblItemNo.Text, 0, 0, frm.txtOperator.Text,
                                         strDefaultPkgLine & intShopOrder, strLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, IIf(dteExpiryDate = DateTime.MinValue, Nothing, dteExpiryDate), trnServer)   'WO#650
                        'WO#650          strDefaultPkgLine & intShopOrder, strLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, trnServer)

                        'WO#512 CreatePrintRequest(CASELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, CASELABELER, _
                        'WO#512          dteSOStartTime, strDefaultPkgLine & intShopOrder, gdrCmpCfg("PalletStation"), gdrCmpCfg("NoOfLabels"), trnServer)
                        CreatePrintRequest(CASELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, CASELABELER,
                                        dteSOStartTime, strDefaultPkgLine & intShopOrder, gdrCmpCfg("PalletStation"), gdrSessCtl.Operator, gdrCmpCfg("NoOfLabels"), trnServer)  'WO#512
                        'WO#718 ADD Start
                        'WO#755 If gblnAutoCaseCountLine = True Then
                        'WO#755 WriteLabelInfoToXMLFile(gstrLabelInputFileName, strDefaultPkgLine, .SCCCode, .UPCCode, intShopOrder, .SaleableUnitPerCase, .QtyPerPallet, .OrderQty, .ItemNumber, .ItemDesc1 & " " & .ItemDesc1 & " " & .ItemDesc3, .Tie, .Tier, dteSOStartTime.ToString("yyyy/MM/dd HH:mm:ss.fff"))
                        'WO#755 WriteLabelInfoToXMLFile(gstrLabelOutputFileName, strDefaultPkgLine, .SCCCode, .UPCCode, intShopOrder, .SaleableUnitPerCase, .QtyPerPallet, .OrderQty, .ItemNumber, .ItemDesc1 & " " & .ItemDesc1 & " " & .ItemDesc3, .Tie, .Tier, dteSOStartTime.ToString("yyyy/MM/dd HH:mm:ss.fff"))
                        'WO#755 End If
                        'WO#718 ADD Stop

                        '    CreateLabelData(gdrSessCtl("Facility"), CASELABEL, CASELABELER, gdrSessCtl("DefaultPkgLine"), _
                        '                    intShopOrder, frm.lblItemNo.Text, 0, 0, frm.txtOperator.Text, _
                        '                    gdrSessCtl("DefaultPkgLine") & intShopOrder, strLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, trnServer)

                        '    CreatePrintRequest(CASELABEL, gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"), CASELABELER, _
                        '                    dteSOStartTime, gdrSessCtl("DefaultPkgLine") & intShopOrder, gdrCmpCfg("PalletStation"), gdrCmpCfg("NoOfLabels"), trnServer)
                    End If

                    'CreateAndPrintLabel(CASELABEL, gdrSessCtl("Facility"), frm.txtPkgLine.Text, frm.txtShopOrder.Text, frm.lblItemNo.Text, 0, _
                    'frm.txtOperator.Text, dteSOStartTime, CASELABEL, frm.txtShopOrder.Text, trnServer)

                    'Print package coder 
                    If oPrintDevice.HasPackageCoderPrinter Then
                        CreateLabelData(gdrSessCtl("Facility"), PACKAGELABEL, PACKAGECODER, strDefaultPkgLine, strDefaultPkgLine,
                                        intShopOrder, frm.lblItemNo.Text, 0, 0, frm.txtOperator.Text,
                                         strDefaultPkgLine & intShopOrder & PACKAGECODER, strLotID, gdrSessCtl("OverrideShiftNo"), strProductionDate, IIf(dteExpiryDate = DateTime.MinValue, Nothing, dteExpiryDate), trnServer)

                        'hard code the machine number here to put put the x label key on coder, as didn't have chance to test on all machines
                        'otherwise it might affect to the x code on existing machines
                        If strDefaultPkgLine.TrimEnd.Contains("3980") Then
                            PrintDiffLabels(PACKAGELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, strDefaultPkgLine, intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text,
                                dteSOStartTime, PACKAGECODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, strDefaultPkgLine & intShopOrder & PACKAGECODER, gdrSessCtl("OverrideShiftNo"), , , trnServer) 'WO#650
                        Else
                            PrintDiffLabels(CASELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, strDefaultPkgLine, intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text,
                                dteSOStartTime, PACKAGECODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, strDefaultPkgLine & intShopOrder, gdrSessCtl("OverrideShiftNo"), , , trnServer) 'WO#650
                        End If

                        'WO#650     PrintDiffLabels(CASELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, strDefaultPkgLine, intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text, _
                        'WO#650               dteSOStartTime, PACKAGECODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, strDefaultPkgLine & intShopOrder, gdrSessCtl("OverrideShiftNo"), , trnServer)
                        'PrintDiffLabels(CASELABEL, gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"), intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text, _
                        '                dteSOStartTime, PACKAGECODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, gdrSessCtl("DefaultPkgLine") & intShopOrder, gdrSessCtl("OverrideShiftNo"), , trnServer)
                    End If

                    'Print filter coder 
                    If oPrintDevice.HasFilterCoderPrinter Then
                        PrintDiffLabels(CASELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, strDefaultPkgLine, intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text,
                                        dteSOStartTime, FILTERCODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, strDefaultPkgLine & intShopOrder, gdrSessCtl("OverrideShiftNo"), , , trnServer) 'WO#650
                        'WO#650     PrintDiffLabels(CASELABEL, gdrSessCtl("Facility"), strDefaultPkgLine, strDefaultPkgLine, intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text, _
                        'WO#650         dteSOStartTime, FILTERCODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, strDefaultPkgLine & intShopOrder, gdrSessCtl("OverrideShiftNo"), , trnServer)
                        'PrintDiffLabels(CASELABEL, gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"), intShopOrder, frm.lblItemNo.Text, 0, frm.txtOperator.Text, _
                        '                dteSOStartTime, FILTERCODER, gdrCmpCfg("PalletStation"), strLotID, strProductionDate, gdrSessCtl("DefaultPkgLine") & intShopOrder, gdrSessCtl("OverrideShiftNo"), , trnServer)
                    End If
                    'WO#17432  Catch ex As SqlException When ex.ErrorCode = -2146232060
                Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                         Or ex.Number = 1231 Or ex.Number = 10054) And ex.ErrorCode = -2146232060       'WO#17432
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                End Try
            End If


            'WO#755 ADD Start
            'Reason to put the codes here is the CaseCounter need the latest shift production date from the session control table
            'WO#5370 If gblnAutoCaseCountLine = True Then
            If gblnAutoCountLine = True Then                'WO#5370
                With drSO
                    Try
                        If IsNothing(CaseCounter) Then
                            'initialize the values on counters.
                            'FIX20161224 CaseCounter = New Counter(.Facility, .ShopOrder, .QtyPerPallet, "Stopped", dteSOStartTime, _
                            'FIX20161224              CInt(frm.txtShift.Text), frm.txtOperator.Text)
                            CaseCounter = New Counter(.Facility, .ShopOrder, .QtyPerPallet, "Stopped", dteSOStartTime,
                                                CInt(frm.txtShift.Text), frm.txtOperator.Text, frm.txtPkgLine.Text,
                                                gdrSessCtl.ShiftProductionDate, gdrCmpCfg.InterfaceType)                        'WO#5370
                            'WO#5370           CInt(frm.txtShift.Text), frm.txtOperator.Text, frm.txtPkgLine.Text, gdrSessCtl.ShiftProductionDate)       'WO#3686
                            'WO#3686                  CInt(frm.txtShift.Text), frm.txtOperator.Text, frm.txtPkgLine.Text)       'FIX20161224
                        End If
                        If CaseCounter.CounterIsStarted = False Then
                            CaseCounter.Start(False)
                        End If

                        If gdrCmpCfg.InterfaceType = "XML" Then                                 'WO#5370
                            Dim xmlInput As New PowerPlant.XMLInterface(gstrLabelInputFileName)
                            'update the shop order information on the Mespec interface file in Input folder
                            xmlInput.StartShopOrder(strDefaultPkgLine, .SCCCode, .UPCCode, intShopOrder, .PackagesPerSaleableUnit, .SaleableUnitPerCase, .QtyPerPallet, .OrderQty, CaseCounter.CasesProducedRunningTotal,
                                                    .ItemNumber, .ItemDesc1 & " " & .ItemDesc2 & " " & .ItemDesc3, .Tie, .Tier, gdrCmpCfg.ComputerName,
                                                     dteSOStartTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), .PalletCode, .SlipSheet)
                            'WO#5370 ADD Start
                        ElseIf gdrCmpCfg.InterfaceType = "SQL" Then
                            Using qta As New dsUnitCountOutBoundTableAdapters.QueriesTableAdapter
                                'qta.PPsp_InitializeUnitCountOutbound(gdrCmpCfg.ComputerName, Now, .Facility, .ItemNumber, gdrSessCtl.Operator, Nothing, _
                                '     strDefaultPkgLine, gdrSessCtl.OverrideShiftNo, .ShopOrder, dteSOStartTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), 0, .QtyPerPallet)
                                qta.PPsp_InitializeUnitCountOutbound(gdrCmpCfg.ComputerName, Now, .Facility, .ItemNumber, gdrSessCtl.Operator, Nothing,
                                     strDefaultPkgLine, gdrSessCtl.OverrideShiftNo, .ShopOrder, dteSOStartTime, 0, .QtyPerPallet)
                            End Using
                        End If
                        'WO#5370 ADD Stop
                    Catch ex As Exception
                        Throw ex
                    End Try
                End With

            End If
            'WO#755 ADD Stop

            'Update current session control
            'WO#755 ReDim arParms(12)
            ReDim arParms(13)   'WO#755

            arParms = New SqlParameter(UBound(arParms)) {}

            ' Default Packaging Line Input Parameter
            arParms(0) = New SqlParameter("@chrDefaultPkgLine", SqlDbType.Char)
            'arParms(0).Value = gdrSessCtl("DefaultPkgLine")
            arParms(0).Value = strDefaultPkgLine

            ' Override Packaging Line Input Parameter
            arParms(1) = New SqlParameter("@chrOverridePkgLine", SqlDbType.Char)
            arParms(1).Value = frm.txtPkgLine.Text

            ' Operator Input Parameter
            arParms(2) = New SqlParameter("@vchOperator", SqlDbType.VarChar)
            arParms(2).Value = frm.txtOperator.Text

            ' Override Shift No Input Parameter
            arParms(3) = New SqlParameter("@tnyOverrideShiftNo", SqlDbType.TinyInt)
            arParms(3).Value = CInt(frm.txtShift.Text)

            ' Default Shift No Input Parameter
            arParms(4) = New SqlParameter("@tnyDefaultShiftNo", SqlDbType.TinyInt)
            arParms(4).Value = shtExpectedShift

            ' Shop Order Input Parameter
            arParms(5) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(5).Value = CInt(intShopOrder)

            'dteSOStartTime = CType(Format(Now, "M/d/yyyy H:mm:ss.fff"), DateTime)
            ' Shop Order Start Time Input Parameter
            arParms(6) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(6).Value = dteSOStartTime

            ' Cases Scheduled Input Parameter
            arParms(7) = New SqlParameter("@intCasesScheduled", SqlDbType.Int)
            arParms(7).Value = CInt(frm.txtCasesScheduledInShift.Text)

            ' Bag Length Used Input Parameter
            arParms(8) = New SqlParameter("@decBagLengthUsed", SqlDbType.Decimal)
            arParms(8).Value = CSng(IIf(RTrim(frm.txtBagLengthUsed.Text) = "", 0, frm.txtBagLengthUsed.Text))

            ' Item Number Input Parameter
            arParms(9) = New SqlParameter("@vchItemNumber", SqlDbType.VarChar)
            arParms(9).Value = frm.lblItemNo.Text

            ' Production Date Input Parameter
            arParms(10) = New SqlParameter("@dteProductionDate", SqlDbType.DateTime)
            arParms(10).Value = dteProductionDate

            ' Carry Forward Input Parameter
            arParms(11) = New SqlParameter("@intCFCases", SqlDbType.Int)
            arParms(11).Value = CType(IIf(RTrim(frm.txtCFCases.Text) = String.Empty, 0, RTrim(frm.txtCFCases.Text)), Integer)

            ' Facility Input Parameter
            arParms(12) = New SqlParameter("@chrFacility", SqlDbType.Char, 3)
            arParms(12).Value = gdrCmpCfg.Facility

            'WO#755 Add Started
            ' CasesProduced Input Parameter
            arParms(13) = New SqlParameter("@intCaseCounter", SqlDbType.Int)
            arParms(13).Value = 0
            'WO#5370 If gblnAutoCaseCountLine = True Then
            If gblnAutoCountLine = True Then                'WO#5370
                arParms(13).Value = CaseCounter.CasesProducedRunningTotal
                'WO#5370 Add Start
            Else
                arParms(13).Value = gdrSessCtl.CaseCounter
                'WO#5370 Add Stop
            End If

            strSQLStmt = "UPDATE tblSessionControl " &
                        "SET DefaultPkgLine = @chrDefaultPkgLine, OverridePkgLine = @chrOverridePkgLine, Operator = @vchOperator, OverrideShiftNo = @tnyOverrideShiftNo, " &
                        "DefaultShiftNo = @tnyDefaultShiftNo, ShopOrder = @intShopOrder, StartTime = @dteSOStartTime, CasesScheduled = @intCasesScheduled, " &
                        "ItemNumber = @vchItemNumber, BagLengthUsed = @decBagLengthUsed, StopTime = NULL, CasesProduced = 0, ReworkWgt = 0, LooseCases = 0,  " &
                        "PalletsCreated = 0, ProductionDate = @dteProductionDate, CarriedForwardCases = @intCFCases, " &
                        "ShiftProductionDate = dbo.fnGetProdDateByShift(@chrFacility,@tnyOverrideShiftNo,@dteProductionDate,@chrOverridePkgLine,NULL)" &
                        ", CaseCounter = @intCaseCounter"
            'WO#755 Add Stop

            'WO#755 strSQLStmt = "UPDATE tblSessionControl " & _
            '             "SET DefaultPkgLine = @chrDefaultPkgLine, OverridePkgLine = @chrOverridePkgLine, Operator = @vchOperator, OverrideShiftNo = @tnyOverrideShiftNo, " & _
            '             "DefaultShiftNo = @tnyDefaultShiftNo, ShopOrder = @intShopOrder, StartTime = @dteSOStartTime, CasesScheduled = @intCasesScheduled, " & _
            '             "ItemNumber = @vchItemNumber, BagLengthUsed = @decBagLengthUsed, StopTime = NULL, CasesProduced = 0, ReworkWgt = 0, LooseCases = 0,  " & _
            '             "PalletsCreated = 0, ProductionDate = @dteProductionDate, CarriedForwardCases = @intCFCases, " & _
            '             "ShiftProductionDate = dbo.fnGetProdDateByShift(@chrFacility,@tnyOverrideShiftNo,@dteProductionDate,@chrOverridePkgLine,NULL)"

            SqlHelper.ExecuteNonQuery(cnnLocal, CommandType.Text, strSQLStmt, arParms)
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
            RefreshSessionControlTable()
            'WO#17432 ADD Start
            'Update the Shift Changed and Started New Shop Order flags on the QAT status table in the IPC
            If gdrCmpCfg.QATWorkFlowInitiation <> QATWorkFlow.Disabled Then
                Dim dtSCH As dsSessionControlHst.CPPsp_SessionControlHstIODataTable
                Dim drSCH As dsSessionControlHst.CPPsp_SessionControlHstIORow
                Dim drQATStatus As dsQATStatus.tblQATStatusRow
                Dim blnShiftChanged As Boolean = False
                Dim blnStartedNewShopOrder As Boolean = False

                dtSCH = SharedFunctions.GetSessionControlHst("SelectLastRecord", gdrSessCtl.DefaultPkgLine, 0, 0, Now, Now, gdrSessCtl.Facility, String.Empty)
                If Not IsNothing(dtSCH) Then
                    If dtSCH.Count > 0 Then
                        drSCH = dtSCH.Rows(0)
                        If gdrSessCtl.ShiftProductionDate <> drSCH.ShiftProductionDate Or gdrSessCtl.OverrideShiftNo <> drSCH.OverrideShiftNo _
                            Or gdrSessCtl.ShopOrder <> drSCH.ShopOrder Then
                            drQATStatus = GetQATStatus()
                            If Not IsNothing(drQATStatus) Then
                                With drQATStatus
                                    If gdrSessCtl.ShopOrder <> drSCH.ShopOrder Then
                                        blnStartedNewShopOrder = True
                                    End If
                                    If gdrSessCtl.ShiftProductionDate <> drSCH.ShiftProductionDate Or gdrSessCtl.OverrideShiftNo Then
                                        blnShiftChanged = True
                                    End If
                                    UpdateQATStatus(.ByPassAllTests, .ByPassTest, .ShopOrder, .QATEntryPoint, .QATDefnID,
                                                    .InterfaceID, .WFTestSeq, blnShiftChanged, .ShopOrderClosed, blnStartedNewShopOrder)
                                End With
                            End If
                        End If
                    End If
                End If
            End If

            'WO#17432 ADD Start 2019/03/11
            'Send request to TCPServer to start running Checkweigher log for the shop order
            Dim blnStartCheckWeigherLog As Boolean = True
            CheckWeigherLog(blnStartCheckWeigherLog)
            'WO#17432 ADD Stop 2019/03/11

            'WO#17432 ADD Stop

            'WO#17432   Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And (ex.Number = 64 Or ex.Number = 1231) And ex.ErrorCode = -2146232060
        Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                         Or ex.Number = 1231 Or ex.Number = 10054) And ex.ErrorCode = -2146232060       'WO#17432
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            'Catch ex As Exception
            '    If Not IsNothing(trnServer) Then
            '        trnServer.Rollback()
            '    End If
            '    Throw New Exception("Error in StartShopOrderUpdate" & vbCrLf & ex.Message)
        Catch ex As SqlClient.SqlException
            'WO#17432   Throw ex
            Throw New Exception("Error in StartShopOrderUpdate" & vbCrLf & ex.Message)      'WO#17432
        Catch ex As Exception                                                               'WO#17432
            Throw New Exception("Error in StartShopOrderUpdate" & vbCrLf & ex.Message)      'WO#17432
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            If Not IsNothing(cnnLocal) Then
                cnnLocal.Close()
            End If
        End Try
    End Sub

    'WO#17432 ADD Start 2019/03/11
    Public Shared Sub CheckWeigherLog(blnStartCheckWeigherLog As Boolean)
        'Retrieve QAT work flow and test information
        Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
        Dim strCmd As String = String.Empty
        Try
            drWF = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, cstrStartup, "frmQATCheckWeigherValidation", False)
            If Not IsNothing(drWF) Then
                If drWF.ExceptionCode <> 0 Then
                    Using taEC As New dsQATExceptionCodeTableAdapters.CPPsp_QATExceptionCode_SelTableAdapter
                        Using dtEC As New dsQATExceptionCode.CPPsp_QATExceptionCode_SelDataTable
                            taEC.Fill(dtEC, gdrSessCtl.Facility, drWF.ExceptionCode, True, Nothing)
                            If dtEC.Count > 0 Then
                                Select Case dtEC(0).ExceptionCode
                                    Case 1      'Gima bulk off packing
                                        If drSO.PackagesPerSaleableUnit > CInt(dtEC(0).Value1) And dtEC(0).InclOrExcl = True Then
                                            blnStartCheckWeigherLog = False
                                        End If
                                    Case 2      'Require QAT checkweight validation but not the checkweigher log 
                                        'If dtEC(0).InclOrExcl = True Then
                                        blnStartCheckWeigherLog = False
                                        'End If
                                End Select
                            End If
                        End Using
                    End Using
                End If
                If blnStartCheckWeigherLog Then
                    strCmd = String.Format("@CWLog {0} 0", drWF.TCPConnID)  'Start checkweigher logging
                Else
                    strCmd = String.Format("@CWLog {0} 1", drWF.TCPConnID)  'Stop checkweigher logging
                End If
                CallTcpServer("127.0.0.1", 8000, strCmd)
            End If
        Catch ex As Exception
            Throw New Exception("Error in CheckWeigherLog" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Function CallTcpServer(IPAddress As String, intPort As Integer, strCmd As String) As String

        Dim tcpClient As New System.Net.Sockets.TcpClient()
        Dim networkStream As System.Net.Sockets.NetworkStream
        Dim bytSendBuffer As [Byte]()
        Dim strReturnData As String = String.Empty
        Dim bytRecBuffer() As Byte

        Try
            If Not IsProcessActive("PPTCPServer") Then
                Using NewProcess As New Process
                    With NewProcess
                        .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                        .StartInfo.FileName = My.Settings.strPPTCPServer
                        .Start()
                    End With
                End Using
            End If

            CallTcpServer = String.Empty
            tcpClient.Connect("127.0.0.1", 8000)
            ReDim bytRecBuffer(tcpClient.ReceiveBufferSize)

            networkStream = tcpClient.GetStream()
            If networkStream.CanWrite And networkStream.CanRead Then
                ' Do a write to host
                bytSendBuffer = Encoding.ASCII.GetBytes(strCmd)
                networkStream.Write(bytSendBuffer, 0, bytSendBuffer.Length)
                ' Read the NetworkStream into a byte buffer.
                networkStream.Read(bytRecBuffer, 0, CInt(tcpClient.ReceiveBufferSize))
                ' Convert receive data in byte to string
                strReturnData = Encoding.ASCII.GetString(bytRecBuffer)
                Return strReturnData
            Else

                If Not networkStream.CanRead Then
                    Return ("cannot not write data to this stream")
                Else
                    If Not networkStream.CanWrite Then
                        Return ("cannot read data from this stream")
                    End If
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in CallTcpServer" & vbCrLf & ex.Message)
        Finally
            tcpClient.Close()
        End Try

    End Function
    'WO#17432 ADD Stop 2019/03/11

    'WO#755 Public Shared Function StopShopOrderUpdate(ByVal frm As frmStopShopOrder, ByVal blnCloseShopOrder As Boolean) As Boolean
    'WO#5370 Public Shared Function StopShopOrderUpdate(ByVal frm As frmStopShopOrder, ByVal blnCloseShopOrder As Boolean, ByVal intCasesProducedInSession As Integer, ByVal intCasesProducedRunningTotal As Integer) As Boolean 'WO#755 
    Public Shared Function StopShopOrderUpdate(ByVal frm As frmStopShopOrder, ByVal blnCloseShopOrder As Boolean) As Boolean            'WO#5370
        Dim trnServer As SqlTransaction = Nothing
        Dim strSQLStmt As String
        Dim arParms() As SqlParameter
        Dim cnnServer As New SqlConnection(gstrServerConnectionString)
        Dim dteSOStopTime As DateTime = Now
        Dim intRtnCde As Integer = 0
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim strOrderChange As String = String.Empty             'WO#5370
        Dim drNoCnn As DialogResult                             'WO#5370
        Dim sbMessage As New StringBuilder()                    'WO#5370

        Try
            'WO#5370 ADD Start
            'Signal Majik Systems that PP stopped the shop order and request to send message to create a pallet in PP.
            If gblnSarongAutoCountLine Then
                With sbMessage
                    .AppendLine("No Server connection during stop shop order. Contact supervisor.")
                    .AppendLine("Retry - Abort stop shop order and allow to re-establish network connection for retry.")
                    .AppendLine("Cancel - Ignore to create partial pallet and continue to stop shop order. After connection is established, start, stop and restart the shop order to resume the counters.")
                End With
                If gblnSvrConnIsUp = False Then
                    IsSvrConnOK(True)
                End If
                If gblnSvrConnIsUp = True Then
                    If blnCloseShopOrder Then
                        strOrderChange = "CloseSO"
                    Else
                        strOrderChange = "StopSO"
                    End If
                    Using qta As New dsUnitCountOutBoundTableAdapters.QueriesTableAdapter
                        Try
                            qta.PPsp_UnitCountOutbound_Upd(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, CType(frm.lblShopOrder.Text, Integer), strOrderChange)
                        Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True
                            drNoCnn = MessageBox.Show(sbMessage.ToString, "No Server Connection", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation)
                            If drNoCnn = DialogResult.Retry Then
                                StopShopOrderUpdate = True                  'Failure
                                Exit Function
                            Else
                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            End If
                        End Try
                    End Using

                    If gblnSvrConnIsUp = True Then
                        If gdrCmpCfg.PkgLineType = "Sarong" Then   'WO#37864
                            With frmSplash
                                .SplashTitle = "Waiting for the complete of pallet creation."
                                .SplashOption = "PalletCreation"
                                .SplashMessage = "    Please wait . . "
                                .ShowDialog()
                            End With
                        End If                                      'WO#37864
                    End If
                Else
                    drNoCnn = MessageBox.Show(sbMessage.ToString, "No Server Connection", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation)
                    If drNoCnn = DialogResult.Retry Then
                        StopShopOrderUpdate = True                  'Failure
                        Exit Function
                    Else
                        SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    End If
                End If

            End If
            'WO#5370 ADD Stop

            'Close the shop order if requested
            If blnCloseShopOrder Then
                With gdrSessCtl
                    'WO#871 ADD Start
                    If gdrCmpCfg.ProbatEnabled = True Then
                        If IsNothing(drSO) Then
                            drSO = SharedFunctions.GetSOInfo("GetSO&Item", .Facility, .ShopOrder, .DefaultPkgLine)
                        End If
                        If Not IsNothing(drSO) Then
                            'WO#2563 SharedFunctions.CloseShopOrderForProbatLine(.Facility, .ShopOrder, drSO.ItemNumber, .DefaultPkgLine, .Operator, 0, .StartTime, "Y", drSO.QtyPerPallet, drSO.LotID, .ShiftProductionDate, .OverrideShiftNo, trnServer)
                            SharedFunctions.CloseShopOrderForProbatLine(.Facility, .ShopOrder, drSO.ItemNumber, .DefaultPkgLine, .Operator, 0, .StartTime, "Y", drSO.QtyPerPallet, drSO.LotID, .ShiftProductionDate, .OverrideShiftNo, Nothing, 0, trnServer)   'ALM#11828
                        End If
                    End If
                    'WO#871 ADD END
                    SharedFunctions.CloseShopOrder(.Facility, .ShopOrder, .DefaultPkgLine, .Operator, .StartTime, Now(), Now(), Now(), CallingFrom.StopShopOrder, trnServer)

                    'stop check weigher log for the shop order
                    CheckWeigherLog(False)             'WO#17432 2019/03/11

                End With
                dteSOStopTime = Now
            End If

            'Update session control record
            'WO#755 ReDim arParms(5)
            ReDim arParms(6)    'WO#755
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Shop Order Stop Time Input Parameter
            arParms(0) = New SqlParameter("@dteSOStopTime", SqlDbType.DateTime)
            arParms(0).Value = dteSOStopTime

            ' Cases Produced Input Parameter
            arParms(1) = New SqlParameter("@intCasesProduced", SqlDbType.Int)
            arParms(1).Value = 0    'WO#755
            'arParms(1).Value = CInt(frm.lblCasesProducedInShift.Text) + CInt(IIf(frm.txtLooseCases.Text = "", 0, frm.txtLooseCases.Text))
            'WO#755 arParms(1).Value = gdrSessCtl.CasesProduced + CInt(IIf(frm.txtLooseCases.Text = "", 0, frm.txtLooseCases.Text))

            Dim intLooseCases As Integer  'WO#755
            Integer.TryParse(Val(frm.txtLooseCases.Text.Trim), intLooseCases) 'WO#755

            'WO#3686    If gblnAutoCaseCountLine = False Then        'WO#755
            With gdrSessCtl
                arParms(1).Value = .CasesProduced + intLooseCases 'WO#755
                tblSCH = SharedFunctions.GetSessionControlHst("SelectLastNonZeroRecByLineSO", .OverridePkgLine, .ShopOrder, .OverrideShiftNo,
                          Now, Now, .Facility, .Operator)
                If tblSCH.Rows.Count > 0 Then
                    arParms(1).Value = arParms(1).Value - tblSCH.Rows(0).Item("LooseCases")
                Else
                    '
                    If .CarriedForwardCases > 0 Then
                        arParms(1).Value = arParms(1).Value - .CarriedForwardCases
                    End If
                End If
            End With
            'WO#3686  DEL Start
            ''WO#755 ADD Start
            'Else
            'If intLooseCases >= 0 Then
            '    arParms(1).Value = intCasesProducedInSession
            'End If
            'End If
            ''WO#755 ADD Stop
            'WO#3686  DEL Stop

            ' Bag Length Used Input Parameter
            arParms(2) = New SqlParameter("@decBagLengthUsed", SqlDbType.Decimal)
            arParms(2).Value = CSng(IIf(frm.txtBagLengthUsed.Text = "", 0, frm.txtBagLengthUsed.Text))

            ' Loose Cases Input Parameter
            arParms(3) = New SqlParameter("@intLooseCases", SqlDbType.Int)
            'WO#755 arParms(3).Value = CInt(IIf(frm.txtLooseCases.Text = "", 0, frm.txtLooseCases.Text))
            arParms(3).Value = intLooseCases    'WO#755

            ' Rework Input Parameter
            arParms(4) = New SqlParameter("@decRework", SqlDbType.Decimal)
            arParms(4).Value = CSng(IIf(frm.txtRework.Text = "", 0, frm.txtRework.Text))

            ' Carried Forward Cases Input Parameter
            arParms(5) = New SqlParameter("@intCarriedForwardCases", SqlDbType.Int)
            arParms(5).Value = gdrSessCtl.CarriedForwardCases

            ' Rework Input Parameter
            arParms(6) = New SqlParameter("@intCaseCounter", SqlDbType.Int)
            arParms(6).Value = gdrSessCtl.CaseCounter

            strSQLStmt = "UPDATE tblSessionControl " &
                         "SET StopTime = @dteSOStopTime, casesProduced = @intCasesProduced, BagLengthUsed = @decBagLengthUsed, " &
                         "ReworkWgt = @decRework, LooseCases = @intLooseCases, CarriedForwardCases = @intCarriedForwardCases" &
                         ", CaseCounter = @intCaseCounter" 'WO#755
            intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
            RefreshSessionControlTable()

            'Write session control record to history
            'Dim strServerName As String
            'Dim strCatalogName As String
            'Dim intStartPos As Integer
            'Dim intEndPos As Integer
            'intStartPos = InStr(1, My.Settings.PowerPlant_ConnectionString, "Source=") + 6
            'intEndPos = InStr(intStartPos + 1, My.Settings.PowerPlant_ConnectionString, ";")
            'strServerName = My.Settings.PowerPlant_ConnectionString.Substring(intStartPos, intEndPos - intStartPos - 1)

            'intStartPos = InStr(intEndPos + 1, My.Settings.PowerPlant_ConnectionString, "Catalog=") + 7
            'intEndPos = InStr(intStartPos + 1, My.Settings.PowerPlant_ConnectionString, ";")
            'strCatalogName = My.Settings.PowerPlant_ConnectionString.Substring(intStartPos, intEndPos - intStartPos - 1)

            'strSQLStmt = "INSERT " + strServerName + "." + strCatalogName + ".dbo.tblSessionControlHst " & _
            '             "SELECT * from tblSessionControl "

            Try
                'SharedFunctions.RefreshSessionControlTable()
                With gdrSessCtl
                    UpdateSessCtlHst(.Item("Facility"), .Item("ComputerName"), .Item("StartTime"), arParms(0).Value, .Item("DefaultPkgLine"),
                        .Item("OverridePkgLine"), .Item("ShopOrder"), .Item("ItemNumber"),
                        .Operator, .Item("LogOnTime"), .Item("DefaultShiftNo"), .Item("OverrideShiftNo"), .Item("CasesScheduled"), arParms(1).Value, .Item("PalletsCreated"), arParms(2).Value,
                        arParms(4).Value, arParms(3).Value, .Item("ProductionDate"), .CarriedForwardCases, .ShiftProductionDate,
                        .CaseCounter)   'WO#755
                    'WO#755 arParms(4).Value, arParms(3).Value, .Item("ProductionDate"), .CarriedForwardCases, .ShiftProductionDate)
                End With

                'WO#17432  Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231)
            Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)                    'WO#17432
                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                strSQLStmt = "INSERT tblSessionControlHst SELECT * from tblSessionControl "
                intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
            End Try

            'Clear the session control record 

            strSQLStmt = "UPDATE tblSessionControl " &
                         "SET ShopOrder = 0, StopTime = NULL, casesProduced = 0, PalletsCreated = 0, BagLengthUsed = 0, " &
                         "ReworkWgt = 0, LooseCases = 0 , CarriedForwardCases = 0 , ItemNumber = ''"


            intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
            If Not IsNothing(trnServer) Then
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
            RefreshSessionControlTable()
            'uploadLogScrapToServer()
            'uploadOperationStaffingToServer()
        Catch ex As SqlException
            If ex.ErrorCode = -2146232060 Then
                If gblnSvrConnIsUp = True Then
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                Else
                    StopShopOrderUpdate = False
                    Throw New Exception("Error in StopShopOrderUpdate" & vbCrLf & ex.Message)
                End If
            End If
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            StopShopOrderUpdate = False
            Throw New Exception("Error in StopShopOrderUpdate" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If

        End Try
    End Function
    'WO#17432 Public Shared Function GetStaffName(ByVal strStaffID As String, ByVal strWorkGroup As String) As String
    Public Shared Function GetStaffName(ByVal strStaffID As String, ByVal strWorkGroup As String, Optional ByVal strStaffClass As String = Nothing) As String     'WO#17432
        'WO#17432   Dim tblPlantStaffing As New dsPlantStaffing.CPPsp_PlantStaffingIODataTable
        Dim intRtnCde As Integer
        Try
            GetStaffName = ""
            Using taPlantStaff As New dsPlantStaffingTableAdapters.CPPsp_PlantStaffingIOTableAdapter                'WO#17432
                Using tblPlantStaffing As New dsPlantStaffing.CPPsp_PlantStaffingIODataTable                        'WO#17432
                    'get the staff record by staff ID
                    'WO#17432   intRtnCde = gtaPlantStaff.Fill(tblPlantStaffing, "SelectAllFields", strStaffID, strWorkGroup)
                    intRtnCde = taPlantStaff.Fill(tblPlantStaffing, "SelectAllFields", strStaffID, strWorkGroup, True, strStaffClass)   'WO#17432
                    If tblPlantStaffing.Rows.Count > 0 Then
                        If tblPlantStaffing.Rows(0).Item("ActiveRecord") = True Then 'WO#755 
                            GetStaffName = tblPlantStaffing.Rows(0).Item("FullName")
                            'WO#755 ADD Start
                        Else
                            GetStaffName = "Deactivated"
                        End If
                        'WO#755 ADD Stop
                        'WO#359 ADD Start ---
                        'If it fails on packaging operators, try look for set up operators
                    Else
                        If strWorkGroup = "P" Then
                            'WO#17432 gtaPlantStaff.Fill(tblPlantStaffing, "SelectAllFields", strStaffID, "SETUP")
                            taPlantStaff.Fill(tblPlantStaffing, "SelectAllFields", strStaffID, "SETUP", True, strStaffClass)             'WO#17432
                            If tblPlantStaffing.Rows.Count > 0 Then
                                GetStaffName = tblPlantStaffing.Rows(0).Item("FullName")
                            End If
                        End If
                        'WO#359 ADD Stop ---
                    End If
                End Using                                                                                               'WO#17432
            End Using                                                                                                   'WO#17432
        Catch ex As Exception
            Throw New Exception("Error in GetStaffName" & vbCrLf & ex.Message)
        End Try
    End Function

    'Public Shared Sub CreateLabelData(ByVal strFacility As String, ByVal strLabelType As String, ByVal strDeviceType As String, _
    '            ByVal strDefaultPkgLine As String, ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal strItemNo As String, _
    '            ByVal intPalletNo As Integer, ByVal intQuantity As Integer, ByVal strOperator As String, ByVal strJobName As String, ByVal strLotID As String, _
    '            ByVal intShift As Short, Optional ByVal strProductionDate As String = "", Optional ByVal sqlTrn As SqlTransaction = Nothing)
    Public Shared Sub CreateLabelData(ByVal strFacility As String, ByVal strLabelType As String, ByVal strDeviceType As String,
            ByVal strDefaultPkgLine As String, ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal strItemNo As String,
            ByVal intPalletNo As Integer, ByVal intQuantity As Integer, ByVal strOperator As String, ByVal strJobName As String, ByVal strLotID As String,
            ByVal intShift As Short, Optional ByVal strProductionDate As String = "",
            Optional ByVal strExpiryDate As String = Nothing,
            Optional ByVal sqlTrn As SqlTransaction = Nothing)      'WO#650

        Dim i As Short = 0
        'WP#650  Dim arParms() As SqlParameter = New SqlParameter(12) {}
        Dim arParms() As SqlParameter = New SqlParameter(13) {}     'WP#650
        Dim strSQLStmt As String
        Try
            'Label Type Input Parameter
            arParms(i) = New SqlParameter("chrLabelType", SqlDbType.Char)
            arParms(i).Value = strLabelType
            i = i + 1

            'Facility Input Parameter
            arParms(i) = New SqlParameter("chrFacility", SqlDbType.Char)
            arParms(i).Value = gdrSessCtl("Facility")
            i = i + 1

            'Packaging line Input Parameter
            arParms(i) = New SqlParameter("chrDefaultPkgLine", SqlDbType.Char)
            arParms(i).Value = strDefaultPkgLine
            i = i + 1

            'Packaging line Input Parameter
            arParms(i) = New SqlParameter("chrPkgLine", SqlDbType.Char)
            arParms(i).Value = strPkgLine
            i = i + 1

            'Shop Order Input Parameter
            arParms(i) = New SqlParameter("intShopOrder", SqlDbType.Int)
            arParms(i).Value = intShopOrder
            i = i + 1

            'Item Number Input Parameter
            arParms(i) = New SqlParameter("vchItemNo", SqlDbType.VarChar)
            arParms(i).Value = strItemNo
            i = i + 1

            'Operator Input Parameter
            arParms(i) = New SqlParameter("@intPalletID", SqlDbType.Int)
            arParms(i).Value = intPalletNo
            i = i + 1

            'Quantity Input Parameter
            arParms(i) = New SqlParameter("intQuantity", SqlDbType.VarChar)
            arParms(i).Value = intQuantity
            i = i + 1

            'Operator Input Parameter
            arParms(i) = New SqlParameter("@vchOperator", SqlDbType.VarChar)
            arParms(i).Value = strOperator
            i = i + 1

            'Job Name Input Parameter
            arParms(i) = New SqlParameter("@vchJobName", SqlDbType.VarChar)
            arParms(i).Value = strJobName
            i = i + 1

            'Production Date Input Parameter
            arParms(i) = New SqlParameter("@vchLotID", SqlDbType.VarChar)
            arParms(i).Value = strLotID
            i = i + 1

            'Production Date Input Parameter
            arParms(i) = New SqlParameter("@vchOvrdProductionDate", SqlDbType.VarChar)
            arParms(i).Value = strProductionDate
            i = i + 1

            'Shift Input Parameter
            arParms(i) = New SqlParameter("@intShift", SqlDbType.TinyInt)
            arParms(i).Value = intShift
            'WO#650 ADD Start
            i = i + 1

            'Expiry Date Input Parameter
            arParms(i) = New SqlParameter("@vchExpiryDate", SqlDbType.VarChar)
            arParms(i).Value = strExpiryDate
            'WO#650 ADD Stop

            strSQLStmt = "PPsp_CreateLabelData"

            If sqlTrn Is Nothing Then
                SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            Else
                SqlHelper.ExecuteNonQuery(sqlTrn, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
            'WO#17432   Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True                 'WO#17432
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in CreateLabelData" & vbCrLf & ex.Message)
        End Try

    End Sub
    'WO512 Public Shared Sub CreatePrintRequest(ByVal strLabelType As String, ByVal strFacility As String, _
    'ByVal strDftPkgLine As String, ByVal strDeviceType As String, ByVal dteStartTime As DateTime, _
    'ByVal strJobName As String, ByVal blnSbmFromPalletStaton As Boolean, Optional ByVal intCopies As Integer = 1, Optional ByVal sqlTrn As SqlTransaction = Nothing)
    Public Shared Sub CreatePrintRequest(ByVal strLabelType As String, ByVal strFacility As String,
        ByVal strDftPkgLine As String, ByVal strDeviceType As String, ByVal dteStartTime As DateTime,
        ByVal strJobName As String, ByVal blnSbmFromPalletStaton As Boolean, ByVal strRequestor As String, Optional ByVal intCopies As Integer = 1, Optional ByVal sqlTrn As SqlTransaction = Nothing) 'WO#512

        'WO#512 Dim arParms() As SqlParameter = New SqlParameter(7) {}
        Dim arParms() As SqlParameter = New SqlParameter(8) {} 'WO#512 
        Dim strSQLStmt As String
        Try
            'WO#6059 Add Start
            If IsPrintRequestOverLimit(strFacility, strJobName, strDeviceType) Then
                Dim strMsg As String = String.Empty
                strMsg = "The same print request had been submitted earlier. It is in the print queue position " & PositionInPrintQueue(strFacility, strJobName, strDeviceType).ToString() & ". Print request will be ignored."
                MessageBox.Show(strMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                'WO#6059 Add Stop
                'Label Type Input Parameter
                arParms(0) = New SqlParameter("@chrLabelType", SqlDbType.Char)
                arParms(0).Value = strLabelType

                'Facility nput Parameter
                arParms(1) = New SqlParameter("chrFacility", SqlDbType.Char)
                arParms(1).Value = strFacility

                'Default Packaging Line nput Parameter
                arParms(2) = New SqlParameter("@chrDftPkgLine", SqlDbType.Char)
                arParms(2).Value = strDftPkgLine

                'Device Type Input Parameter
                arParms(3) = New SqlParameter("@chrDeviceType", SqlDbType.Char)
                arParms(3).Value = strDeviceType

                'Start Time nput Parameter
                arParms(4) = New SqlParameter("@dteStartTime", SqlDbType.DateTime)
                arParms(4).Value = dteStartTime

                'Job Name input Parameter
                arParms(5) = New SqlParameter("@vchJobName", SqlDbType.VarChar)
                arParms(5).Value = strJobName

                'Submit from Pallet Station input Parameter
                arParms(6) = New SqlParameter("@bitSbmFromPalletStation", SqlDbType.Bit)
                arParms(6).Value = blnSbmFromPalletStaton

                'No Of Labels input Parameter
                arParms(7) = New SqlParameter("@intCopies", SqlDbType.SmallInt)
                arParms(7).Value = intCopies

                'WO#512 ADD Start
                'Requestor
                arParms(8) = New SqlParameter("@vchRequestor", SqlDbType.VarChar)
                arParms(8).Value = strRequestor
                'WO#512 ADD Stop

                strSQLStmt = "PPsp_CreatePrintRequest"
                If sqlTrn Is Nothing Then
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Else
                    SqlHelper.ExecuteNonQuery(sqlTrn, CommandType.StoredProcedure, strSQLStmt, arParms)
                End If
            End If
        Catch ex As SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in CreatePrintRequest" & vbCrLf & ex.Message)
        End Try
    End Sub
    'WO#650 DEL Start
    'Public Shared Sub CreateAndPrintLabel(ByVal strLabelType As String, ByVal strFacility As String, ByVal strDftPkgLine As String, ByVal strOvrPkgLine As String, _
    '                                    ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal intQuantity As Integer, _
    '                                    ByVal strOperator As String, ByVal dteSOStartTime As DateTime, ByVal strDeviceType As String, _
    '                                    ByVal strJobName As String, ByVal blnSbmFromPalletStation As Boolean, ByVal strLotID As String, _
    '                                    Optional ByVal strProductionDate As String = "", Optional ByVal intPalletID As Integer = 0, _
    '                                    Optional ByVal intCopies As Integer = 1, Optional ByVal intShift As Short = 1, Optional ByVal trnServer As SqlTransaction = Nothing)
    'WO#650 DEL Stop
    'WO#650 ADD Start
    Public Shared Sub CreateAndPrintLabel(ByVal strLabelType As String, ByVal strFacility As String, ByVal strDftPkgLine As String, ByVal strOvrPkgLine As String,
                                ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal intQuantity As Integer,
                                ByVal strOperator As String, ByVal dteSOStartTime As DateTime, ByVal strDeviceType As String,
                                ByVal strJobName As String, ByVal blnSbmFromPalletStation As Boolean, ByVal strLotID As String,
                                ByVal strRequestor As String, ByVal blnIsManualRequest As Boolean,
                                Optional ByVal strProductionDate As String = "", Optional ByVal intPalletID As Integer = 0,
                                Optional ByVal intCopies As Integer = 1, Optional ByVal intShift As Short = 1,
                                Optional ByVal strExpiryDate As String = "", Optional ByVal trnServer As SqlTransaction = Nothing)
        'WO#650 ADD Stop
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim dteCurrentTime As DateTime = Now
        Dim i As Short = 0

        Try
            'The routine will be executed only if the server is up
            If gblnSvrConnIsUp = True Then
                'WO#6059 Add Start
                If IsPrintRequestOverLimit(strFacility, strJobName, strDeviceType) Then
                    Dim strMsg As String = String.Empty
                    strMsg = "The same print request had been submitted earlier. It is in the print queue position " & PositionInPrintQueue(strFacility, strJobName, strDeviceType).ToString() & ". Print request will be ignored."
                    MessageBox.Show(strMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    'WO#6059 Add Stop

                    'Create case label to label source file and print it
                    ReDim arParms(16)
                    'WO#512  ReDim arParms(17) WO#650
                    ReDim arParms(18)   'WO#512
                    ReDim arParms(19)   'V6.77
                    arParms = New SqlParameter(UBound(arParms)) {}

                    ' Label Type Input Parameter
                    arParms(i) = New SqlParameter("@chrLabelType", SqlDbType.VarChar)
                    arParms(i).Value = strLabelType
                    i = i + 1

                    'Facility Input Parameter
                    arParms(i) = New SqlParameter("chrFacility", SqlDbType.VarChar)
                    arParms(i).Value = strFacility
                    i = i + 1

                    'Default Packaging Line input Parameter
                    arParms(i) = New SqlParameter("chrDftPkgLine", SqlDbType.Char)
                    arParms(i).Value = strDftPkgLine
                    i = i + 1

                    'Override Packaging Line input Parameter
                    arParms(i) = New SqlParameter("@chrOvrPkgLine", SqlDbType.Char)
                    arParms(i).Value = strOvrPkgLine
                    i = i + 1

                    'Shop Order Input Parameter
                    arParms(i) = New SqlParameter("intShopOrder", SqlDbType.Int)
                    arParms(i).Value = intShopOrder
                    i = i + 1

                    'Item Number Input Parameter
                    arParms(i) = New SqlParameter("vchItemNo", SqlDbType.VarChar)
                    arParms(i).Value = strItemNo
                    i = i + 1

                    'Quantity Input Parameter
                    arParms(i) = New SqlParameter("intQuantity", SqlDbType.Int)
                    arParms(i).Value = intQuantity
                    i = i + 1

                    'Operator Input Parameter
                    arParms(i) = New SqlParameter("@vchOperator", SqlDbType.VarChar)
                    arParms(i).Value = strOperator
                    i = i + 1

                    'Additional parameters for printing label for CIMControl

                    'Device Type Input Parameter
                    arParms(i) = New SqlParameter("@chrDeviceType", SqlDbType.Char)
                    arParms(i).Value = strDeviceType
                    i = i + 1

                    'Start Time Parameter
                    arParms(i) = New SqlParameter("@dteStartTime", SqlDbType.DateTime)
                    arParms(i).Value = dteSOStartTime
                    i = i + 1

                    'Job Name input Parameter
                    arParms(i) = New SqlParameter("@vchJobName", SqlDbType.VarChar)
                    arParms(i).Value = strJobName
                    i = i + 1

                    'Submit From Pallet Station input Parameter
                    arParms(i) = New SqlParameter("@bitSbmFromPalletStation", SqlDbType.Bit)
                    arParms(i).Value = blnSbmFromPalletStation
                    i = i + 1

                    'Production Date input Parameter
                    arParms(i) = New SqlParameter("@vchLotID", SqlDbType.VarChar)
                    arParms(i).Value = strLotID
                    i = i + 1

                    'Production Date input Parameter
                    arParms(i) = New SqlParameter("@vchProductionDate", SqlDbType.VarChar)
                    arParms(i).Value = strProductionDate
                    i = i + 1

                    'Pallet No input Parameter
                    arParms(i) = New SqlParameter("@intPalletID", SqlDbType.Int)
                    arParms(i).Value = intPalletID
                    i = i + 1

                    'No of Copies input Parameter
                    arParms(i) = New SqlParameter("@intCopies", SqlDbType.Int)
                    arParms(i).Value = intCopies
                    i = i + 1

                    'Shift input Parameter
                    arParms(i) = New SqlParameter("@intShift", SqlDbType.Int)
                    arParms(i).Value = intShift
                    'WO#650 ADD Start 
                    i = i + 1

                    'Shift input Parameter
                    arParms(i) = New SqlParameter("@vchExpiryDate", SqlDbType.VarChar)
                    arParms(i).Value = strExpiryDate
                    i = i + 1

                    'Shift input Parameter
                    arParms(i) = New SqlParameter("@vchRequestor", SqlDbType.VarChar)
                    arParms(i).Value = strRequestor
                    i = i + 1
                    'WO#650 ADD Stop

                    'Bit indicating if the print is request is Manual
                    arParms(i) = New SqlParameter("@bitIsManualRequest", SqlDbType.Bit)
                    arParms(i).Value = blnIsManualRequest
                    'V6.77

                    strSQLStmt = "PPsp_CreateAndPrintLabel"
                    If trnServer Is Nothing Then
                        SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                    Else
                        SqlHelper.ExecuteNonQuery(trnServer, CommandType.StoredProcedure, strSQLStmt, arParms)
                    End If
                End If          'WO#6059

            End If

            'Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            'WO#17432   Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
        Catch ex As SqlClient.SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True         'WO#17432
            SetServerCnnStatusInSessCtl(False)  'WO#871
            'WO#871 Throw ex
        Catch ex As Exception
            Throw New Exception("Error in CreateAndPrintLabel" & vbCrLf & ex.Message)
        End Try
    End Sub
    'WO#2563 DEL Start
    'Public Shared Function ProcessFrmCreatePallet(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strItemNo As String, _
    '                                    ByVal strDftPkgLine As String, ByVal strOperator As String, ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, _
    '                                    ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, ByVal strLotID As String, ByVal dteProductionDate As DateTime, _
    '                                    ByVal intShiftNo As Short, ByVal blnIsPalletStation As Boolean) As Integer
    'WO#2563 DEL Stop
    'ALM#11828 DEL Start
    'Public Shared Function ProcessFrmCreatePallet(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strItemNo As String, _
    '                            ByVal strDftPkgLine As String, ByVal strOperator As String, ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, _
    '                            ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, ByVal strLotID As String, ByVal dteProductionDate As DateTime, _
    '                            ByVal intShiftNo As Short, ByVal strOutputLocation As String, ByVal blnIsPalletStation As Boolean) As Integer          'WO#2563
    'ALM#11828 DEL Stop
    'WO#5370 DEL Start
    Public Shared Function ProcessFrmCreatePallet(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strItemNo As String,
                        ByVal strDftPkgLine As String, ByVal strOperator As String, ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime,
                        ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, ByVal strLotID As String, ByVal dteProductionDate As DateTime,
                        ByVal intShiftNo As Short, ByVal strOutputLocation As String, ByVal blnIsPalletStation As Boolean,
                        ByVal intDestinationShopOrder As Integer, ByVal intTxID As Integer, ByVal strCreator As String) As Integer          'WO#5370 and V6.78

        'WO#5370     ByVal intDestinationShopOrder As Integer) As Integer          'ALM#11828
        'WO#5370 DEL Stop


        Dim cnnServer As SqlConnection = New SqlConnection(gstrServerConnectionString)
        Dim trnServer As SqlTransaction = Nothing

        Dim arParms() As SqlParameter = New SqlParameter(1) {}
        Dim strSQLStmt As String
        Dim intPalletID As Integer
        Dim strJobName As String
        'Dim ds As New DataSet
        'Dim dr As DataRow

        Try
            ' Get production date from session control record
            'Shop Order Input Parameter
            'arParms(0) = New SqlParameter("@chrAction", SqlDbType.Char)
            'arParms(0).Value = "SelectAllFields"
            'ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.StoredProcedure, "CPPsp_SessionControlIO", arParms)
            'dr = ds.Tables(0).Rows(0)
            'strProductionDate = Format(dr("ProductionDate"), "yyyyMMdd")
            If gblnSvrConnIsUp = True Then
                Try
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                Catch ex As SqlException
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                End Try
            End If
            'WO#2563 DEL Start
            'intPalletID = CreatePallet(strFacility, 0, intShopOrder, strItemNo, strDftPkgLine, strOperator, _
            '                intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate, intShiftNo, blnIsPalletStation, trnServer)
            'WO#2563 DEL Stop
            'ALM#11828  DEL Start
            'intPalletID = CreatePallet(strFacility, 0, intShopOrder, strItemNo, strDftPkgLine, strOperator, _
            '                intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate, _
            '                intShiftNo, blnIsPalletStation, strOutputLocation, trnServer)       'WO#2563
            'ALM#11828  DEL Stop
            intPalletID = CreatePallet(strFacility, 0, intShopOrder, strItemNo, strDftPkgLine, strOperator,
                intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate,
                intShiftNo, blnIsPalletStation, strOutputLocation, intDestinationShopOrder, intTxID, strCreator, trnServer)       'WO#5370
            'WO#5370    intShiftNo, blnIsPalletStation, strOutputLocation, intDestinationShopOrder, trnServer)       'ALM#11828
            'Update Cases Produced and Pallet created in Session Control for packaging lines
            If blnIsPalletStation = False Then
                arParms(0) = New SqlParameter("@intQuantity", SqlDbType.Int)
                arParms(0).Value = intQuantity

                arParms(1) = New SqlParameter("@dteProductionDate", SqlDbType.DateTime)
                arParms(1).Value = dteProductionDate

                'arParms(1) = New SqlParameter("@chrFacility", SqlDbType.Char)
                'arParms(1).Value = strFacility

                'arParms(2) = New SqlParameter("@intShift", SqlDbType.TinyInt)
                'arParms(2).Value = intShiftNo

                'arParms(3) = New SqlParameter("@chrPkgLine", SqlDbType.Char)
                'arParms(3).Value = strDftPkgLine
                'WO#3686 If gblnAutoCaseCountLine = False Then    'WO#755
                strSQLStmt = "UPDATE tblSessionControl set LooseCases = 0, CasesProduced = CasesProduced +  @intQuantity, " &
                                    " PalletsCreated = PalletsCreated + 1, ProductionDate = CONVERT(datetime,@dteProductionDate,120)"
                'WO#3686  DEL Start
                '    'WO#755 ADD Start
                'Else
                '    strSQLStmt = "UPDATE tblSessionControl set LooseCases = 0,  " & _
                '                        " PalletsCreated = PalletsCreated + 1, ProductionDate = CONVERT(datetime,@dteProductionDate,120)"
                'End If
                ''WO#755 ADD Stop
                'WO#3686  DEL Stop

                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                RefreshSessionControlTable()
            Else    'WO#297
                'Set the close shop order flag to Y in the Shop Order table on Local and Server data base
                If strOrderComplete = "Y" Then  'WO#297
                    With gdrSessCtl 'WO#297
                        SharedFunctions.CloseShopOrder(strFacility, intShopOrder, strDftPkgLine, strOperator, dteSOStartTime, Now(), Now(), Now(), CallingFrom.CreatePallet, trnServer) 'WO#297
                    End With        'WO#297

                    'stop check weigher log for the shop order
                    CheckWeigherLog(False)             'WO#17432 2019/03/11
                End If
            End If

            'WO#34957 ADD Start
            SharedFunctions.IPCControlUpdate("PreviousPalletCreationTime", Now, Nothing)
            'WO#34957 ADD Stop

            'Set the close shop order flag to Y in the Shop Order table on Local and Server data base
            'if 'Last Pallet For This Shop Order' is Yes
            'WO# 297 DEL begin
            'If strOrderComplete = "Y" Then
            ''Shop Order Input Parameter
            'arParms(0) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            'arParms(0).Value = intShopOrder

            'strSQLStmt = "UPDATE tblShopOrder SET Closed = 1 WHERE (ShopOrder = @intShopOrder)"
            ''Update Shop order table for close shop order on local data base
            'SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
            'If gblnSvrConnIsUp = True Then
            '    'Update Shop order table for close shop order on server data base
            '    Try
            '        SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.Text, strSQLStmt, arParms)
            '    Catch ex As SqlException
            '        SharedFunctions.SetServerCnnStatusInSessCtl(False)
            '    End Try
            'End If
            'End If
            'WO# 297 DEL End

            If Not trnServer Is Nothing Then
                Try
                    trnServer.Commit()
                    _dbServer.SaveChanges()
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

            'If the package line is print pallet label enable, 
            'create a "Pallet" label record to tblLabalData and send to print
            ProcessFrmCreatePallet = intPalletID
            If gdrCmpCfg("PrintPalletLabel") = True AndAlso gblnSvrConnIsUp = True Then
                If intPalletID > 0 Then
                    strJobName = strDftPkgLine & CType(intPalletID, String)
                    Try
                        trnServer = cnnServer.BeginTransaction()
                        'For pallet label, the overrode packaging line should use the value of default packaging line
                        CreateAndPrintLabel(PALLETLABEL, strFacility, strDftPkgLine, strDftPkgLine, intShopOrder, strItemNo,
                                             intQuantity, strOperator, dteSOStartTime, PALLETLABELER, strJobName, blnIsPalletStation, strLotID, gdrSessCtl.Operator, False, Format(dteProductionDate, "yyyyMMdd"), intPalletID, , , , trnServer) 'WP#512
                        'WO#512 intQuantity, strOperator, dteSOStartTime, PALLETLABELER, strJobName, blnIsPalletStation, strLotID, Format(dteProductionDate, "yyyyMMdd"), intPalletID, , , , trnServer) 'WP#650
                        'WO#650     intQuantity, strOperator, dteSOStartTime, PALLETLABELER, strJobName, blnIsPalletStation, strLotID, Format(dteProductionDate, "yyyyMMdd"), intPalletID, , , trnServer)
                        EditPallet("SubmitedToPrint", intPalletID, trnServer)
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

                        'Reload the case label on the printer
                        'strJobName = strDftPkgLine & CType(intShopOrder, String)
                        'trnServer = cnnServer.BeginTransaction()
                        'CreateAndPrintLabel(CASELABEL, chrFacility, strDftPkgLine, intShopOrder, strItemNo, _
                        '            intQuantity, strOperator, dteSOStartTime, CASELABELER, strJobName, "", 0, trnServer)

                        'trnServer.Commit()
                    Catch ex As SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
                        Throw ex
                    End Try
                End If
            End If
            'WO#17432   Catch ex As SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
        Catch ex As SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                    Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True        'WO#17432
            Throw ex
        Catch ex As Exception
            trnServer.Rollback()
            Throw New Exception("Error in ProcessFrmCreatePallet" & vbCrLf & ex.Message)
        Finally
            'ds = Nothing
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
        End Try
    End Function
    Public Shared Function ProcessFrmPrintPallet(ByVal dgvPallet As DataGridView, ByVal e As DataGridViewCellMouseEventArgs, ByVal blnSbmFromPalletStation As Boolean,
    ByVal intShift As Short) As Boolean
        Dim strWhichButton As String
        Dim strJobName As String = ""
        Dim drResponse As DialogResult
        Dim intShopOrder As Integer
        Dim dvRow As DataGridViewRow
        Dim blnSameSO As Boolean = False
        Dim cnnServer As SqlConnection = New SqlConnection(gstrServerConnectionString)
        Dim trnServer As SqlTransaction = Nothing
        Try
            ProcessFrmPrintPallet = False           'WO#5370
            cnnServer.Open()
            trnServer = cnnServer.BeginTransaction()
            With dgvPallet.Rows(e.RowIndex)
                strWhichButton = .Cells(e.ColumnIndex).Value
                If strWhichButton = "Print" Then
                    If .Cells("OrderComplete").Value = "Y" Then
                        intShopOrder = .Cells("ShopOrder").Value
                        For Each dvRow In dgvPallet.Rows
                            If dvRow.Cells("ShopOrder").Value = intShopOrder Then
                                blnSameSO = True
                                If dvRow.Cells("OrderComplete").Value = "N" Then
                                    MessageBox.Show("This Last Pallet can not be selected while the related shop order still has unprinted pallet(s).",
                                                "Pallet is selected in an improper sequence.")
                                    Exit Function
                                End If
                            Else
                                'The list is sorted by shop order no. So if it is changed after the selected shop order, the checking is done. Exit the for loop
                                If blnSameSO = True Then
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                    'Debug.Print("qty=" & .Cells("Quantity").Value)
                    strJobName = .Cells("DefaultPkgLine").Value & .Cells("PalletID").Value
                    'Debug.Print("job name=" & strJobName)
                    'For printing pallet, the overrode packaging line should not be applied. thererfore, the overrode packaging line should be the value of default packaging line.
                    CreateAndPrintLabel(PALLETLABEL, gdrCmpCfg("Facility"), .Cells("DefaultPkgLine").Value, .Cells("DefaultPkgLine").Value, .Cells("ShopOrder").Value,
                     .Cells("ItemNumber").Value, .Cells("Quantity").Value, "", .Cells("StartTime").Value, PALLETLABELER, strJobName,
                      blnSbmFromPalletStation, .Cells("LotID").Value, gdrSessCtl.Operator, True, .Cells("ProductionDate").Value, .Cells("PalletID").Value, 1, intShift, , trnServer) 'WO#512
                    'WO#512 blnSbmFromPalletStation, .Cells("LotID").Value, .Cells("ProductionDate").Value, .Cells("PalletID").Value, 1, intShift, , trnServer) 'WO#650
                    'WO#650     blnSbmFromPalletStation, .Cells("LotID").Value, .Cells("ProductionDate").Value, .Cells("PalletID").Value, 1, intShift, trnServer)
                    EditPallet("SubmitedToPrint", .Cells("PalletID").Value, trnServer)
                ElseIf strWhichButton = "Delete" Then
                    'WO#512 drResponse = MessageBox.Show("Are you sure to delete the selected pallet?", "Delete a Pallet", MessageBoxButtons.YesNo)
                    drResponse = MessageBox.Show("Are you sure to delete the selected pallet " & .Cells("PalletID").Value & "?", "Delete a Pallet", MessageBoxButtons.YesNo)
                    If drResponse = Windows.Forms.DialogResult.No Then
                        ProcessFrmPrintPallet = True                    'WO#5370
                        'WO#5370        Exit Function
                    Else
                        SharedFunctions.EditPallet("Delete", .Cells("PalletID").Value, trnServer)
                    End If
                End If
            End With
            If Not trnServer Is Nothing Then
                Try
                    trnServer.Commit()
                Catch ex As Exception
                    Try
                        trnServer.Rollback()
                    Catch exRollback As Exception
                        SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    End Try
                End Try
            End If
        Catch ex As Exception
            'trnServer.Rollback()
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
        End Try
    End Function

    Public Shared Function PrintPalletByID(ByVal dr As dsPalletHst.CPPsp_PalletHstIORow, ByVal blnSbmFromPalletStation As Boolean) As Boolean
        Dim strJobName As String = ""
        Try
            With dr
                strJobName = .Item("DefaultPkgLine") & CType(.Item("PalletID"), String)
                'For printing pallet, the overrode packaging line should not be applied. thererfore, the overrode packaging line should be the value of default packaging line.
                'WO#512 CreateAndPrintLabel(PALLETLABEL, .Facility, .DefaultPkgLine, .DefaultPkgLine, .ShopOrder, .ItemNumber, _
                'WO#512            .Quantity, .Operator, .StartTime, PALLETLABELER, strJobName, blnSbmFromPalletStation, .LotID, _
                'WO#512            .ProductionDate, .PalletID)
                CreateAndPrintLabel(PALLETLABEL, .Facility, .DefaultPkgLine, .DefaultPkgLine, .ShopOrder, .ItemNumber,
                                    .Quantity, .Operator, .StartTime, PALLETLABELER, strJobName, blnSbmFromPalletStation, .LotID,
                                     gdrSessCtl.Operator, True, .ProductionDate, .PalletID)     'WO#512 'V6.77 -True for blnIsManualRequest
                'strJobName = strPkgLine & CType(intPalletID, String)
                'CreateAndPrintLabel(PALLETLABEL, gdrCmpCfg("Facility"), strPkgLine, intShopOrder, strItemNumber, _
                '                    intQty, gdrSessCtl("Operator"), dteStartTime, PALLETLABELER, strJobName, "", intPalletID)
            End With
        Catch ex As SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
    'WO#2563    Public Shared Function CreatePallet(ByVal strFacility As String, ByVal intPalletID As Integer, ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal strDftPkgLine As String, ByVal strOperator As String, _
    'WO#2563                                 ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, _
    'WO#2563                                 ByVal strLotID As String, ByVal dteProductionDate As DateTime, ByVal intShiftNo As Short, ByVal blnIsPalletStation As Boolean, Optional ByVal sqlTrn As SqlTransaction = Nothing) As Integer
    'ALM#11828  DEL Start
    'WO#2563 ADD Start
    'Public Shared Function CreatePallet(ByVal strFacility As String, ByVal intPalletID As Integer, ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal strDftPkgLine As String, ByVal strOperator As String, _
    '                                 ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, _
    '                                 ByVal strLotID As String, ByVal dteProductionDate As DateTime, ByVal intShiftNo As Short, ByVal blnIsPalletStation As Boolean, _
    '                                 ByVal strOutputLocation As String, Optional ByVal sqlTrn As SqlTransaction = Nothing) As Integer
    'WO#2563 ADD Stop
    'ALM#11828  DEL Stop
    Public Shared Function CreatePallet(ByVal strFacility As String, ByVal intPalletID As Integer, ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal strDftPkgLine As String, ByVal strOperator As String,
                             ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer,
                             ByVal strLotID As String, ByVal dteProductionDate As DateTime, ByVal intShiftNo As Short, ByVal blnIsPalletStation As Boolean,
                             ByVal strOutputLocation As String, ByVal intDestinationShopOrder As Integer, ByVal intTxID As Integer, ByVal strCreator As String, Optional ByVal sqlTrn As SqlTransaction = Nothing) As Integer      'WO#5370
        'WO#5370             ByVal strOutputLocation As String, ByVal intDestinationShopOrder As Integer, Optional ByVal sqlTrn As SqlTransaction = Nothing) As Integer      'ALM#11828
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim arParmsUpd() As SqlParameter                                            'WO#5370
        Dim strSQLStmtHst As String                                                 'WO#5370
        Dim strProductionDate As String = Format(dteProductionDate, "yyyyMMdd")

        Try
            If gblnSvrConnIsUp = True AndAlso intPalletID = 0 Then
                Try
                    If sqlTrn Is Nothing Then
                        cnnServer = New SqlConnection(gstrServerConnectionString)
                        cnnServer.Open()
                        trnServer = cnnServer.BeginTransaction()
                    Else
                        trnServer = sqlTrn
                    End If

                    ReDim arParms(0)
                    arParms = New SqlParameter(UBound(arParms)) {}

                    'Facility Input Parameter
                    arParms(0) = New SqlParameter("@chrFacility", SqlDbType.VarChar)
                    arParms(0).Value = strFacility

                    strSQLStmt = "PPsp_GetPalletNo"
                    intPalletID = CType(SqlHelper.ExecuteScalar(trnServer, CommandType.StoredProcedure, strSQLStmt, arParms), Integer)

                Catch ex As SqlException
                    'If server connection failure, connect to local data base
                    SetServerCnnStatusInSessCtl(False)
                End Try
            End If

            'WO#871 ReDim arParms(15)
            'WO#2563    ReDim arParms(16)       'WO#871
            'ALM#11828  ReDim arParms(17)       'WO#2563 
            ReDim arParms(18)                   'ALM#11828
            arParms = New SqlParameter(UBound(arParms)) {}

            'Facility Input Parameter
            arParms(0) = New SqlParameter("@chrFacility", SqlDbType.VarChar)
            arParms(0).Value = strFacility

            'Pallet ID Input Parameter
            arParms(1) = New SqlParameter("@intPalletID", SqlDbType.Int)
            arParms(1).Value = intPalletID

            'Shop Order Input Parameter
            arParms(2) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(2).Value = intShopOrder

            'Item Number Input Parameter
            arParms(3) = New SqlParameter("@vchItemNumber", SqlDbType.VarChar)
            arParms(3).Value = strItemNo

            'Quantity Input Parameter
            arParms(4) = New SqlParameter("intQuantity", SqlDbType.Int)
            arParms(4).Value = intQuantity

            'Start Time Input Parameter
            arParms(5) = New SqlParameter("@dteStartTime", SqlDbType.DateTime)
            arParms(5).Value = dteSOStartTime

            'Default Packaging Line Input Parameter
            arParms(6) = New SqlParameter("@chrDefaultPkgLine", SqlDbType.Char, 10)
            arParms(6).Value = strDftPkgLine

            'Operator Input Parameter
            arParms(7) = New SqlParameter("@vchOperator", SqlDbType.VarChar)
            arParms(7).Value = strOperator

            'Order Complete Flag Input Parameter
            arParms(8) = New SqlParameter("@chrOrderComplete", SqlDbType.Char)
            arParms(8).Value = strOrderComplete

            'Print Status Input Parameter
            'ALM#11828  arParms(9) = New SqlParameter("@bitPrintStatus", SqlDbType.Bit)
            arParms(9) = New SqlParameter("@intPrintStatus", SqlDbType.SmallInt)     'ALM#11828
            arParms(9).Value = 0

            'Quantity per Pallet Input Parameter
            arParms(10) = New SqlParameter("@intQtyPerPallet", SqlDbType.Int)
            arParms(10).Value = intQtyPerPallet

            'Lot ID Input Parameter
            arParms(11) = New SqlParameter("@vchLotID", SqlDbType.VarChar)
            arParms(11).Value = strLotID

            'Production Date Input Parameter
            'arParms(12) = New SqlParameter("@chrProductionDate", SqlDbType.VarChar)
            arParms(12) = New SqlParameter("@dteProductionDate", SqlDbType.DateTime)
            arParms(12).Value = dteProductionDate

            'Expiry Date Input Parameter
            arParms(13) = New SqlParameter("@chrExpiryDate", SqlDbType.VarChar)

            'Calculate expiry date
            'WO#650 ADD Start
            Dim tblItemMaster As New dsItemMaster.CPPsp_ItemMasterIODataTable
            gtaItemMaster.Fill(tblItemMaster, strFacility, strItemNo, "AllByItemNo")
            If gblnOvrExpDate = True And gdrCmpCfg.PalletStation = False Then
                With tblItemMaster.Rows(0)
                    arParms(13).Value = DateAdd(DateInterval.Day, .Item("ShipShelfLifeDays") - .Item("ProductionShelfLifeDays"), gdteExpiryDate).ToString("yyyyMMdd") 'WO#650
                End With
            Else
                'WO#650 ADD Stop
                'WO#650 Dim tblItemMaster As New dsItemMaster.CPPsp_ItemMasterIODataTable   
                'WO#650 gtaItemMaster.Fill(tblItemMaster, chrFacility, strItemNo, "AllByItemNo")
                If tblItemMaster.Rows.Count > 0 Then
                    'Dim dteProductionDate As DateTime = CDate(strProductionDate.Insert(4, "/").Insert(7, "/"))
                    arParms(13).Value = Format(DateAdd(DateInterval.Day, tblItemMaster.Rows(0)("ProductionShelfLifeDays"), dteProductionDate), "yyyyMMdd")
                Else
                    arParms(13).Value = strProductionDate
                End If
            End If  'WO#650

            'Shift No Input Parameter
            arParms(14) = New SqlParameter("@intShiftNo", SqlDbType.TinyInt)
            arParms(14).Value = intShiftNo

            'Is PalletStation Input Parameter
            arParms(15) = New SqlParameter("@bitIsPalletStation", SqlDbType.Bit)
            arParms(15).Value = blnIsPalletStation

            'WO#871 Add Start
            'Is Probat Enabled Input Parameter
            arParms(16) = New SqlParameter("@bitProbatEnabled", SqlDbType.Bit)
            arParms(16).Value = gdrCmpCfg.ProbatEnabled
            'WO#871 Add Stop

            'WO#2563 Add Start
            arParms(17) = New SqlParameter("@vchOutputLocation", SqlDbType.VarChar)
            arParms(17).Value = strOutputLocation
            'WO#2563 Add Stop

            'ALM#11828 Add Start
            arParms(18) = New SqlParameter("@intDestinationShopOrder", SqlDbType.Int)
            arParms(18).Value = intDestinationShopOrder
            'ALM#11828 Add Stop

            strSQLStmt = "CPPsp_Pallet_Add"

            If gblnSvrConnIsUp = True AndAlso Not IsNothing(trnServer) Then
                'if the server connection failure when create the pallet record in the server, 
                'write it to local database.
                Try

                    SqlHelper.ExecuteNonQuery(trnServer, CommandType.StoredProcedure, strSQLStmt, arParms)

                    _dbServer.AddTotblPalletExt(New ServerModels.tblPalletExt With {.PalletID = intPalletID, .Creator = strCreator})

                    'WO#5370 ADD Start
                    If gblnSarongAutoCountLine Then
                        strSQLStmtHst = "PPsp_UnitCountInbound_Upd"
                        ReDim arParmsUpd(3)
                        arParmsUpd = New SqlParameter(UBound(arParmsUpd)) {}

                        'Facility Input Parameter
                        arParmsUpd(0) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
                        arParmsUpd(0).Value = strFacility

                        'Transaction ID Parameter
                        arParmsUpd(1) = New SqlParameter("@intTxID", SqlDbType.Int)
                        arParmsUpd(1).Value = intTxID

                        'Pallet ID Input Parameter
                        arParmsUpd(2) = New SqlParameter("@intPalletID", SqlDbType.Int)
                        arParmsUpd(2).Value = intPalletID

                        'Processing Status Input Parameter
                        arParmsUpd(3) = New SqlParameter("@intProcessStatus", SqlDbType.TinyInt)
                        arParmsUpd(3).Value = 2

                        SqlHelper.ExecuteNonQuery(trnServer, CommandType.StoredProcedure, strSQLStmtHst, arParmsUpd)
                    End If
                    'WO#5370 ADD Stop

                    'If the sql transaction is created inside this function, then commit it.
                    'Else commit by the calling function.
                    If sqlTrn Is Nothing Then
                        Try
                            trnServer.Commit()
                            _dbServer.SaveChanges()
                        Catch ex As Exception
                            Try
                                trnServer.Rollback()
                            Catch exRollback As Exception
                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            End Try
                        End Try
                    End If
                    'WO#17432  Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
                Catch ex As SqlClient.SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True                 'WO#17432
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    If Not IsNothing(trnServer) Then
                        trnServer = Nothing
                    End If
                    'set pallet id = 0 when create pallet to local database
                    arParms(1).Value = 0
                    arParms(18).Value = intDestinationShopOrder
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                Dim arLocalParms() As SqlParameter
                ReDim arLocalParms(19)
                Array.Copy(arParms, arLocalParms, arParms.Length)
                arLocalParms(19) = New SqlParameter("@vchCreator", SqlDbType.VarChar)
                arLocalParms(19).Value = strCreator

                'set pallet id = 0 when create pallet to local database
                arLocalParms(1).Value = 0

                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arLocalParms)
            End If

            'WO#755 ADD Start
            'WO#5370    If gblnAutoCaseCountLine = True AndAlso Not IsNothing(CaseCounter) Then
            If gblnAutoCountLine = True AndAlso Not IsNothing(CaseCounter) Then         'WO#5370
                CaseCounter.PostCreatePallet(intQuantity)
            End If
            'WO#755 ADD Stop

            'Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            'WO#17432   Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                   Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True         'WO#17432 
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
        Catch ex As Exception
            'FX150505 DEL Start
            'If Not IsNothing(trnServer) Then
            '    Try
            '        trnServer.Rollback()
            '    Catch exRollback As Exception
            '        SharedFunctions.SetServerCnnStatusInSessCtl(False)
            '    End Try
            'End If
            'FX150505 DEL Stop
            Throw New Exception("Error in CreatePallet" & vbCrLf & ex.Message)
        Finally
            CreatePallet = intPalletID
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            If Not IsNothing(trnServer) Then
                trnServer = Nothing
            End If
        End Try
    End Function

    Public Shared Sub ClearLabelData(ByVal strDftPkgLine As String, ByVal dteSOStartTime As DateTime, ByVal strFacility As String, Optional ByVal sqlTrn As SqlTransaction = Nothing)
        'Delete rows from tblDynamicLabelData 
        'If not pallet label and package line matched or
        'If Pallet label is marked printed and the creation time is greater than a minute from now or
        'creation time is greater than 7 days from now.
        Dim i As Short
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Try
            If gblnSvrConnIsUp = True Then

                If sqlTrn Is Nothing Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                Else
                    trnServer = sqlTrn
                End If

                ReDim arParms(2)
                arParms = New SqlParameter(UBound(arParms)) {}

                'Packaging Line Input Parameter (use overrode packaging line to clear label data)
                arParms(i) = New SqlParameter("@chrDftPkgLine", SqlDbType.Char)
                arParms(i).Value = strDftPkgLine
                i = i + 1

                'Current Time Input Parameter
                arParms(i) = New SqlParameter("@chrFacility", SqlDbType.Char)
                arParms(i).Value = strFacility
                i = i + 1

                'Current Time Input Parameter
                arParms(i) = New SqlParameter("@dteCurrentTime", SqlDbType.DateTime)
                arParms(i).Value = dteSOStartTime

                strSQLStmt = "DELETE tblDynamicLabelData " &
                             "WHERE (Facility = @chrFacility AND defaultPkgLine = @chrDftPkgLine AND (RecordType = 'I' Or RecordType = 'C')) OR (RecordType = 'P' " &
                             "AND PrintedFlag = 1 AND DATEDIFF(mi, CreationTime, @dteCurrentTime) > 1) OR (DATEDIFF(day, CreationTime, @dteCurrentTime) > 7)"

                'strSQLStmt = "DELETE tblDynamicLabelData " & _
                '             "WHERE (Facility = @chrFacility AND PackagingLine = @chrPackagingLine AND (RecordType = 'I' OR RecordType = 'C')) OR (RecordType = 'P' " & _
                '             "AND PrintedFlag = 1 AND DATEDIFF(mi, CreationTime, @dteCurrentTime) > 1) OR (DATEDIFF(day, CreationTime, @dteCurrentTime) > 7)"
                'If sqlTrn Is Nothing Then
                '    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.Text, strSQLStmt, arParms)
                'Else
                SqlHelper.ExecuteNonQuery(trnServer, CommandType.Text, strSQLStmt, arParms)
                'End If

                ReDim Preserve arParms(1)
                'use default packaging line to clear the print request data since labels are physically printed from the default line printer.
                strSQLStmt = "DELETE tblCimControlJob " &
                        "WHERE Facility = @chrFacility AND DefaultPkgLine = @chrDftPkgLine AND (LabelType = 'I' OR LabelType = 'C')"

                'strSQLStmt = "DELETE tblCimControlJob " & _
                '        "WHERE (Facility = @chrFacility AND DefaultPkgLine = @chrPackagingLine AND (LabelType = 'I' OR LabelType = 'C'))"

                SqlHelper.ExecuteNonQuery(trnServer, CommandType.Text, strSQLStmt, arParms)

                If sqlTrn Is Nothing Then
                    Try
                        trnServer.Commit()
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        If Not IsNothing(trnServer) Then
                            Try
                                trnServer.Rollback()
                            Catch exRollBack As Exception
                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            End Try
                        End If

                    End Try
                End If
            End If
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            Throw New Exception("Error in ClearLabelData" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
        End Try
    End Sub

    'WO650 DEL Start
    'Public Shared Sub PrintDiffLabels(ByVal strLabelType As String, ByVal strFacility As String, ByVal strDftPkgLine As String, ByVal strOvrPkgLine As String, _
    '                                    ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal intQuantity As Integer, _
    '                                    ByVal strOperator As String, ByVal dteSOStartTime As DateTime, ByVal strDeviceType As String, _
    '                                    ByVal blnSbmFromPalletStation As Boolean, ByVal strLotID As String, ByVal strProductionDate As String, _
    '                                    ByVal strJobName As String, ByVal intShift As Short, Optional ByVal intCopies As Integer = 1, _
    '                                    Optional ByVal sqlTrn As SqlTransaction = Nothing)
    'WO650 DEL Stop
    'WO650 ADD Start
    Public Shared Sub PrintDiffLabels(ByVal strLabelType As String, ByVal strFacility As String, ByVal strDftPkgLine As String, ByVal strOvrPkgLine As String,
                                ByVal intShopOrder As Integer, ByVal strItemNo As String, ByVal intQuantity As Integer,
                                ByVal strOperator As String, ByVal dteSOStartTime As DateTime, ByVal strDeviceType As String,
                                ByVal blnSbmFromPalletStation As Boolean, ByVal strLotID As String, ByVal strProductionDate As String,
                                ByVal strJobName As String, ByVal intShift As Short, Optional ByVal intCopies As Integer = 1,
                                Optional ByVal strExpiryDate As String = "", Optional ByVal sqlTrn As SqlTransaction = Nothing)
        'WO650 ADD Stop
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing
        Try
            If sqlTrn Is Nothing Then
                cnnServer = New SqlConnection(gstrServerConnectionString)
                cnnServer.Open()
                trnServer = cnnServer.BeginTransaction()
            Else
                trnServer = sqlTrn
            End If
            CreateAndPrintLabel(strLabelType, strFacility, strDftPkgLine, strOvrPkgLine, intShopOrder, strItemNo,
                                intQuantity, strOperator, dteSOStartTime, strDeviceType, strJobName,
                                blnSbmFromPalletStation, strLotID, gdrSessCtl.Operator, False, strProductionDate, , intCopies, intShift, strExpiryDate, trnServer)     'WO#512
            'WO#512            blnSbmFromPalletStation, strLotID, strProductionDate, , intCopies, intShift, strExpiryDate, trnServer) 'WO#650
            'WO#650            blnSbmFromPalletStation, strLotID, strProductionDate, , intCopies, intShift, trnServer)
            If sqlTrn Is Nothing Then
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

        Catch ex As SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            Throw New Exception("Error in PrintDiffLabels" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
        End Try

    End Sub

    Public Shared Sub EditPallet(ByVal strAction As String, ByVal intPalletID As Integer, Optional ByVal sqlTrn As SqlTransaction = Nothing)
        Dim strSQLStmt As String = ""
        Dim arParms() As SqlParameter
        Dim i As Int16 'WO#512
        Try
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'WO#512 ReDim arParms(1)
                ReDim arParms(2)    'WO#512
                arParms = New SqlParameter(UBound(arParms)) {}

                'WO#512 ADD Start
                If strAction = "Delete" Then
                    i = 0

                    'Pallet ID Input Parameter
                    arParms(i) = New SqlParameter("@intPalletID", SqlDbType.Int)
                    arParms(i).Value = intPalletID
                    i = i + 1

                    'Deletion Time Input Parameter
                    arParms(i) = New SqlParameter("@dteDeletionTime", SqlDbType.DateTime)
                    arParms(i).Value = Now()
                    i = i + 1

                    'Deletion By Input Parameter
                    arParms(i) = New SqlParameter("vchDeletedBy", SqlDbType.VarChar)
                    arParms(i).Value = gdrSessCtl.Operator

                    strSQLStmt = "PPsp_PalletDeletion_Add"
                    If sqlTrn Is Nothing Then
                        SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                    Else
                        SqlHelper.ExecuteNonQuery(sqlTrn, CommandType.StoredProcedure, strSQLStmt, arParms)
                    End If

                End If

                ReDim arParms(1)
                'WO#512 ADD Stop

                'Action Input Parameter
                arParms(0) = New SqlParameter("@chrAction", SqlDbType.VarChar)
                arParms(0).Value = strAction

                'Pallet ID Input Parameter
                arParms(1) = New SqlParameter("@intPalletID", SqlDbType.Int)
                arParms(1).Value = intPalletID

                strSQLStmt = "PPsp_EditPallet"
                If sqlTrn Is Nothing Then
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Else
                    SqlHelper.ExecuteNonQuery(sqlTrn, CommandType.StoredProcedure, strSQLStmt, arParms)
                End If



            End If
        Catch ex As SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in EditPallet" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Function uploadPalletToServer() As Boolean

        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim daP As New dsPalletTableAdapters.CPPsp_PalletIOTableAdapter
        Dim dtP As New dsPallet.CPPsp_PalletIODataTable
        Dim dr As dsPallet.CPPsp_PalletIORow

        Dim trnServer As SqlTransaction = Nothing
        Dim cnnServer As SqlConnection = Nothing
        uploadPalletToServer = True
        'if the connection to server failure, end the routine
        Try
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then

                ' read pallet record from local database
                daP.Connection.ConnectionString = gstrLocalDBConnectionString
                'WO#2563 daP.Fill(dtP, "", "", gdrSessCtl.Facility, 0)
                daP.Fill(dtP, "", "", 0, gdrSessCtl.Facility) 'WO#2563
                If dtP.Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()

                    trnServer = cnnServer.BeginTransaction()
                    'ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt)
                    'For Each dr In ds.Tables(0).Rows
                    For Each dr In dtP.Rows
                        If gblnSvrConnIsUp = False Then
                            Exit For
                        End If
                        With dr
                            ' create a pallet record in server
                            'WO#2563 DEL Start
                            'CreatePallet(dr.Facility, dr.PalletID, dr.ShopOrder, dr.ItemNumber, dr.DefaultPkgLine, _
                            '    dr.Operator, dr.Quantity, dr.StartTime, dr.OrderComplete, dr.QtyPerPallet, _
                            '    dr.LotID, dr.ProductionDate.Substring(0, 4) & "-" & dr.ProductionDate.Substring(4, 2) & "-" & dr.ProductionDate.Substring(6, 2) & " 00:00:00", _
                            '    dr.ShiftNo, gdrCmpCfg.PalletStation, trnServer)
                            'WO#2563 DEL Stop
                            'ALM#11828 DEL Start
                            'CreatePallet(dr.Facility, dr.PalletID, dr.ShopOrder, dr.ItemNumber, dr.DefaultPkgLine, _
                            '    dr.Operator, dr.Quantity, dr.StartTime, dr.OrderComplete, dr.QtyPerPallet, _
                            '    dr.LotID, dr.ProductionDate.Substring(0, 4) & "-" & dr.ProductionDate.Substring(4, 2) & "-" & dr.ProductionDate.Substring(6, 2) & " 00:00:00", _
                            '    dr.ShiftNo, gdrCmpCfg.PalletStation, dr.OutputLocation, trnServer)
                            'ALM#11828 DEL Stop

                            Dim strCreator As String = _dbLocal.tblPalletExt.FirstOrDefault(Function(x) x.CreationDateTime = dr.CreationDateTime).Creator

                            CreatePallet(dr.Facility, dr.PalletID, dr.ShopOrder, dr.ItemNumber, dr.DefaultPkgLine,
                                dr.Operator, dr.Quantity, dr.StartTime, dr.OrderComplete, dr.QtyPerPallet,
                                dr.LotID, dr.ProductionDate.Substring(0, 4) & "-" & dr.ProductionDate.Substring(4, 2) & "-" & dr.ProductionDate.Substring(6, 2) & " 00:00:00",
                                dr.ShiftNo, gdrCmpCfg.PalletStation, dr.OutputLocation, dr.DestinationShopOrder, 0, strCreator, trnServer)      'WO#5370
                            'WO#5370    dr.ShiftNo, gdrCmpCfg.PalletStation, dr.OutputLocation, dr.DestinationShopOrder, trnServer)      'ALM#11828
                            ' delete the uploaded pallet record from local database
                            'WO#871 strSQLStmt = "Delete tblPallet where rrn = " & dr.RRN
                            'WO#871 ADD Start
                            strSQLStmt = "Delete tblPallet where rrn = " & dr.RRN &
                                         "; Delete tblProbatPallet where CUSTOMER_ID = " & dr.PalletID &
                                         "; Delete PRO_IMP_PALLET where CUSTOMER_ID = " & dr.PalletID &
                                        "; Delete tblPalletExt where CreationDateTime = " & "'" & dr.CreationDateTime.ToString("yyyy-MM-dd HH:mm:ss:fff") & "'"
                            'WO#871 ADD Stop
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                        End With
                    Next
                    If Not trnServer Is Nothing Then
                        Try
                            trnServer.Commit()
                            _dbServer.SaveChanges()
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
                End If
            End If
        Catch ex As SqlException When gblnSvrConnIsUp = True
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            'WO#17432 If (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) Then
            If (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054 Or ex.Number = 11001) Then          'WO#17432
                SetServerCnnStatusInSessCtl(False)
            End If
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                Try
                    trnServer.Rollback()
                Catch ex2 As Exception
                End Try
            End If
            Throw New Exception("Error in uploadPalletToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function
    Public Shared Function uploadOperationStaffingToServer() As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing
        uploadOperationStaffingToServer = True

        Try
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then

                strSQLStmt = "select * from tblOperationStaffing"
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    'Try
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    'Catch ex As SqlException
                    '    Exit Function
                    'End Try
                    trnServer = cnnServer.BeginTransaction()

                    'Update Operation Staffing Table
                    ReDim arParms(3)
                    arParms = New SqlParameter(UBound(arParms)) {}
                    'Facility Input Parameter
                    arParms(0) = New SqlParameter("@chrFacility", SqlDbType.VarChar)
                    'Shop Order Input Parameter

                    arParms(1) = New SqlParameter("@chrPackagingLine", SqlDbType.VarChar)

                    'Start Time Parameter
                    arParms(2) = New SqlParameter("@dtmStartTime", SqlDbType.DateTime)

                    'Item Number Input Parameter
                    arParms(3) = New SqlParameter("@vchStaffID", SqlDbType.VarChar)

                    For Each dr In ds.Tables(0).Rows
                        If gblnSvrConnIsUp = False Then
                            Exit For
                        End If

                        With dr
                            UpdateOperationStaffing(.Item("PackagingLine"), .Item("StartTime"), .Item("facility"), .Item("StaffID"), trnServer)
                            arParms(0).Value = .Item("facility")
                            arParms(1).Value = .Item("PackagingLine")
                            arParms(2).Value = .Item("StartTime")
                            arParms(3).Value = .Item("StaffID")
                            strSQLStmt = "Delete tblOperationStaffing WHERE facility = @chrFacility AND PackagingLine = @chrPackagingLine AND " &
                                         "StartTime = @dtmStartTime AND StaffID = @vchStaffID"

                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException When gblnSvrConnIsUp = True
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            If ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) Then
                SetServerCnnStatusInSessCtl(False)
            End If
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            uploadOperationStaffingToServer = False
            Throw New Exception("Error in uploadOperationStaffingToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try

    End Function

    Public Shared Function UploadSessCtlHstToServer() As Boolean
        Dim strSQLStmt As String = ""
        Dim daSCH As New dsSessionControlHstTableAdapters.CPPsp_SessionControlHstIOTableAdapter
        Dim dtSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim drSCH As dsSessionControlHst.CPPsp_SessionControlHstIORow
        Dim trnServer As SqlTransaction = Nothing
        Dim cnnServer As SqlConnection = Nothing

        Try
            If gblnSvrConnIsUp = True Then

                With daSCH
                    .Connection.ConnectionString = gstrLocalDBConnectionString
                    'WO#755 .Fill(dtSCH, "SelectAllRecords", String.Empty, 0, 0, New System.Nullable(Of Date), New System.Nullable(Of Date), String.Empty, String.Empty)
                    .Fill(dtSCH, "SelectAllRecords", String.Empty, 0, 0, New System.Nullable(Of Date), New System.Nullable(Of Date), String.Empty, String.Empty, 0) 'WO#755
                    .Connection.ConnectionString = gstrServerConnectionString
                End With
                If dtSCH.Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()

                    For Each drSCH In dtSCH.Rows
                        If gblnSvrConnIsUp = False Then
                            Exit For
                        End If
                        With drSCH
                            trnServer = cnnServer.BeginTransaction()
                            UpdateSessCtlHst(.Facility, .ComputerName, .StartTime, .StopTime, .DefaultPkgLine,
                                           .OverridePkgLine, .ShopOrder, .ItemNumber,
                                           ._Operator, .LogOnTime, .DefaultShiftNo, .OverrideShiftNo,
                                           .CasesScheduled, .CasesProduced, .PalletsCreated, .BagLengthUsed,
                                           .ReworkWgt, .LooseCases, .ProductionDate, .CarriedForwardCases, .ShiftProductionDate,
                                           .CaseCounter, trnServer) 'WO#755
                            'WO#755 .ReworkWgt, .LooseCases, .ProductionDate, .CarriedForwardCases, .ShiftProductionDate, trnServer)
                            strSQLStmt = "Delete tblSessionControlHst WHERE rrn = " & .RRN
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
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
                        End With
                    Next
                End If
            End If
        Catch ex As SqlException When gblnSvrConnIsUp = True
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            'WO#17432 If ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) Then
            If ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                        Or ex.Number = 1231 Or ex.Number = 10054 Or ex.Number = 11001) Then            'WO#17432
                SetServerCnnStatusInSessCtl(False)
            End If
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            Throw New Exception("Error in uploadSessCtlHstToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
        End Try
    End Function

    Public Shared Function UploadToBeLoadedShopOrderToServer() As Boolean
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing
        Try
            If gblnSvrConnIsUp = True Then
                Dim dr As dsToBeClosedShopOrder.dtToBeClosedShopOrderRow

                Using da As New dsToBeClosedShopOrderTableAdapters.daToBeClosedShopOrder
                    da.Connection.ConnectionString = gstrLocalDBConnectionString
                    Using dt As New dsToBeClosedShopOrder.dtToBeClosedShopOrderDataTable
                        da.Fill(dt, "ALL", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                        If dt.Rows.Count > 0 Then
                            cnnServer = New SqlConnection(gstrServerConnectionString)
                            cnnServer.Open()
                            trnServer = cnnServer.BeginTransaction()
                            For Each dr In dt
                                With dr
                                    CloseShopOrder(.Facility, .ShopOrder, .DefaultPkgLine, ._Operator, .SessionStartTime, .ClosingTime, .LastUpdated, .CreationTime, CallingFrom.UploadToServer, trnServer)
                                End With
                                dr.Delete()
                            Next
                            da.Update(dt)
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
                        End If
                    End Using
                End Using
            End If

        Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
            If Not trnServer Is Nothing Then
                trnServer.Rollback()
            End If
            'WO#17432   If (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) Then
            If (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                  Or ex.Number = 1231 Or ex.Number = 10054 Or ex.Number = 11001) Then        'WO#17432
                SetServerCnnStatusInSessCtl(False)
            End If
            Throw New Exception("Error in UploadToBeLoadedShopOrderToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not trnServer Is Nothing Then
                trnServer.Rollback()
            End If
            Throw New Exception("Error in UploadToBeLoadedShopOrderToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
        End Try

    End Function

    Public Shared Function uploadLogScrapToServer() As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing
        uploadLogScrapToServer = True

        Try
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then

                strSQLStmt = "select * from tblComponentScrap"
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    'Try
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    '        Catch ex As SqlException
                    '    Exit Function
                    'End Try
                    trnServer = cnnServer.BeginTransaction()

                    'Update Component Scraps Table
                    ReDim arParms(3)
                    arParms = New SqlParameter(UBound(arParms)) {}
                    'Facility Input Parameter
                    arParms(0) = New SqlParameter("@chrFacility", SqlDbType.VarChar)
                    'Shop Order Input Parameter

                    arParms(1) = New SqlParameter("@intShopOrder", SqlDbType.Int)

                    'Start Time Parameter
                    arParms(2) = New SqlParameter("@dteStartTime", SqlDbType.DateTime)

                    'Item Number Input Parameter
                    arParms(3) = New SqlParameter("@vchComponent", SqlDbType.VarChar)

                    For Each dr In ds.Tables(0).Rows
                        'If the last update to the server failure, exit the loop
                        If gblnSvrConnIsUp = False Then
                            Exit For
                        End If
                        With dr
                            UpdateLogScraps(.Item("facility"), .Item("ShopOrder"), .Item("StartTime"), .Item("Component"),
                                            .Item("Quantity"))

                            arParms(0).Value = .Item("facility")
                            arParms(1).Value = .Item("ShopOrder")
                            arParms(2).Value = .Item("StartTime")
                            arParms(3).Value = .Item("Component")
                            strSQLStmt = "Delete tblComponentScrap WHERE facility = @chrFacility AND ShopOrder = @intShopOrder AND " &
                                         "StartTime = @dteStartTime AND Component = @vchComponent "

                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
            If Not trnServer Is Nothing Then
                trnServer.Rollback()
            End If
            'WO#17432   If (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) Then
            If (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                Or ex.Number = 1231 Or ex.Number = 10054 Or ex.Number = 11001) Then        'WO#17432
                SetServerCnnStatusInSessCtl(False)
            End If
        Catch ex As Exception
            If Not trnServer Is Nothing Then
                trnServer.Rollback()
            End If
            uploadLogScrapToServer = False
            Throw New Exception("Error in uploadLogScrapToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function
    Public Shared Function ProcessLogOn(ByVal strOverridePkgLine As String, ByVal strOperator As String,
    ByVal shtOverrideShift As Short, ByVal shtExpectedShift As Short) As Boolean
        Dim strSQLStmt As String
        Dim arParms() As SqlParameter
        Try
            'Update current session control
            ReDim arParms(5)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Default Packaging Line Input Parameter
            arParms(0) = New SqlParameter("@chrDefaultPkgLine", SqlDbType.Char)
            'arParms(0).Value = gdrCmpCfg("PackagingLine")
            arParms(0).Value = SharedFunctions.GetSessionDefaultPkgLine(gdrCmpCfg.Facility, gdrSessCtl.DefaultPkgLine, strOverridePkgLine)

            ' Override Packaging Line Input Parameter
            arParms(1) = New SqlParameter("@chrOverridePkgLine", SqlDbType.Char)
            arParms(1).Value = strOverridePkgLine

            ' Operator Input Parameter
            arParms(2) = New SqlParameter("@vchOperator", SqlDbType.VarChar)
            arParms(2).Value = strOperator

            ' Override Shift No Input Parameter
            arParms(3) = New SqlParameter("@tnyOverrideShiftNo", SqlDbType.TinyInt)
            arParms(3).Value = shtOverrideShift

            ' Default Shift No Input Parameter
            arParms(4) = New SqlParameter("@tnyDefaultShiftNo", SqlDbType.TinyInt)
            arParms(4).Value = shtExpectedShift

            ' Log On Time Input Parameter
            arParms(5) = New SqlParameter("@dteLogOnTime", SqlDbType.DateTime)
            arParms(5).Value = Now

            strSQLStmt = "UPDATE tblSessionControl " &
                            "SET DefaultPkgLine = @chrDefaultPkgLine, OverridePkgLine = @chrOverridePkgLine, Operator = @vchOperator, OverrideShiftNo = @tnyOverrideShiftNo, LogOnTime = @dteLogOnTime"

            If gdrCmpCfg("PalletStation") Then
                strSQLStmt = strSQLStmt & ", StartTime = @dteLogOnTime ,DefaultShiftNo = @tnyDefaultShiftNo, StopTime = NULL, CasesScheduled = 0, CasesProduced = 0, " &
                             "PalletsCreated = 0, BagLengthUsed = 0, ReworkWgt = 0, LooseCases = 0"
            End If

            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)

            'With gdrSessCtl
            '    .DefaultPkgLine = SharedFunctions.GetSessionDefaultPkgLine(gdrCmpCfg.Facility, gdrSessCtl.DefaultPkgLine, strOverridePkgLine)
            '    .OverridePkgLine = strOverridePkgLine
            '    .Operator = strOperator
            '    .OverrideShiftNo = shtExpectedShift
            '    .LogOnTime = Now
            '    SharedFunctions.UpdateSessCtl(gdrSessCtl, DBLocation.Server_Local)
            'End With

            RefreshSessionControlTable()
        Catch ex As Exception
            Throw New Exception("Error in ProcessSignOn" & vbCrLf & ex.Message)
        End Try
    End Function
    Public Shared Sub UpdateOperationStaffing(ByVal strPkgLine As String, ByVal dteSOStartTime As DateTime,
    ByVal strFacility As String, ByVal strStaffID As String, Optional ByVal trnServer As SqlTransaction = Nothing)
        Dim arParms() As SqlParameter = Nothing
        Dim strSQLStmt As String = Nothing
        Dim strCnnStr As String
        Try
            'Insert Utility Techicians to table
            ReDim arParms(4)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Default Packaging Line Input Parameter
            arParms(0) = New SqlParameter("@chrPackagingLine", SqlDbType.Char, 10)
            arParms(0).Value = strPkgLine

            ' Start Time Input Parameter
            arParms(1) = New SqlParameter("@dtmStartTime", SqlDbType.DateTime)
            arParms(1).Value = dteSOStartTime

            ' Action Input Parameter
            arParms(2) = New SqlParameter("@vchAction", SqlDbType.VarChar)
            arParms(2).Value = "Insert"

            ' Facility Input Parameter
            arParms(3) = New SqlParameter("@chrFacility", SqlDbType.Char)
            arParms(3).Value = strFacility

            ' Staff ID Input Parameter
            arParms(4) = New SqlParameter("@vchStaffID", SqlDbType.VarChar)
            arParms(4).Value = strStaffID

            strSQLStmt = "CPPsp_OperationStaffingIO"

            If Not trnServer Is Nothing Then
                SqlHelper.ExecuteNonQuery(trnServer, CommandType.StoredProcedure, strSQLStmt, arParms)
            Else
                strCnnStr = gstrServerConnectionString
                If gblnSvrConnIsUp = False Then
                    strCnnStr = gstrLocalDBConnectionString
                End If
                SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
            SetServerCnnStatusInSessCtl(False)
            strCnnStr = gstrLocalDBConnectionString
            If Not IsNothing(strSQLStmt) Then
                SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in UpdateOperationStaffing" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Sub UpdateLogScraps(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal dteStartTime As DateTime,
                                      ByVal strComponent As String, ByVal sngQuantity As Single)
        Dim strSQLStmt As String = Nothing
        Dim arParms() As SqlParameter = Nothing
        Dim strCnnStr As String
        Try

            'Update Component Scraps Table
            ReDim arParms(5)
            arParms = New SqlParameter(UBound(arParms)) {}
            'Facility Input Parameter
            arParms(0) = New SqlParameter("@chrFacility", SqlDbType.VarChar)
            'Shop Order Input Parameter

            arParms(1) = New SqlParameter("@intShopOrder", SqlDbType.Int)

            'Start Time Parameter
            arParms(2) = New SqlParameter("@dteStartTime", SqlDbType.DateTime)

            'Item Number Input Parameter
            arParms(3) = New SqlParameter("@vchComponent", SqlDbType.VarChar)

            'Quantity Input Parameter
            arParms(4) = New SqlParameter("decQuantity", SqlDbType.Decimal)

            'Action Input Parameter
            arParms(5) = New SqlParameter("@vchAction", SqlDbType.VarChar)

            arParms(0).Value = strFacility
            arParms(1).Value = intShopOrder
            arParms(2).Value = dteStartTime
            arParms(3).Value = strComponent
            arParms(4).Value = sngQuantity
            arParms(5).Value = "Edit"

            strSQLStmt = "CPPsp_EditComponentScrap"
            strCnnStr = gstrServerConnectionString
            If gblnSvrConnIsUp = False Then
                strCnnStr = gstrLocalDBConnectionString
            End If

            SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.StoredProcedure, strSQLStmt, arParms)
            'WO#17432   Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                     Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True     'WO#17432
            SetServerCnnStatusInSessCtl(False)
            strCnnStr = gstrLocalDBConnectionString
            If Not IsNothing(strSQLStmt) Then
                SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in UpdateLogScraps" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Sub UpdateSessCtlHst(ByVal strFacility As String, ByVal strComputerName As String, ByVal dteStartTime As DateTime,
                                    ByVal dteStopTime As DateTime, ByVal strDftPkgLine As String, ByVal strOvrrPkgLine As String,
                                    ByVal intShopOrder As Integer, ByVal strItemNumber As String, ByVal strOperator As String,
                                    ByVal dteLogOnTime As DateTime, ByVal strDftShiftNo As String, ByVal strOvrrShiftNo As String,
                                    ByVal intCasesScheduled As Integer, ByVal intCasesProduced As Integer, ByVal intPalletsCreated As Integer,
                                    ByVal sngBagLengthUsed As Single, ByVal sngReworkWgt As Single, ByVal intLooseCases As Integer,
                                    ByVal dteProductionDate As DateTime, ByVal intCarriedForwardCases As Integer, ByVal dteShiftProductionDate As DateTime,
                                    ByVal intCaseCounter As Integer,
                                    Optional ByVal trnServer As SqlTransaction = Nothing) 'WO#755
        'WO#755 ByVal dteProductionDate As DateTime, ByVal intCarriedForwardCases As Integer, ByVal dteShiftProductionDate As DateTime, Optional ByVal trnServer As SqlTransaction = Nothing)
        Dim strSQLStmt As String
        Dim arParms() As SqlParameter
        Dim strCnnStr As String = Nothing
        Try

            'Update Component Scraps Table
            'WO#755 ReDim arParms(21)
            ReDim arParms(22)   'WO#755
            arParms = New SqlParameter(UBound(arParms)) {}

            'Action Input Parameter
            arParms(0) = New SqlParameter("@vchAction", SqlDbType.VarChar, 30)

            'Facility Input Parameter
            arParms(1) = New SqlParameter("@chrFacility", SqlDbType.Char, 3)

            'Computer Name Input Parameter
            arParms(2) = New SqlParameter("@vchComputerName", SqlDbType.VarChar, 50)

            'Start Time Parameter
            arParms(3) = New SqlParameter("@dteStartTime", SqlDbType.DateTime)

            'Stop Time Parameter
            arParms(4) = New SqlParameter("@dteStopTime", SqlDbType.DateTime)

            'Default Packaging Line Input Parameter
            arParms(5) = New SqlParameter("@chrPkgLine", SqlDbType.[Char], 10)

            'Override Packaging Line Input Parameter
            arParms(6) = New SqlParameter("@chrOverridePkgLine", SqlDbType.[Char], 10)

            'Shop Order Input Parameter
            arParms(7) = New SqlParameter("@intShopOrder", System.Data.SqlDbType.Int, 4)

            'Item Number Input Parameter
            arParms(8) = New SqlParameter("@vchItemNumber", System.Data.SqlDbType.VarChar, 35)
            arParms(9) = New SqlParameter("@vchOperator", System.Data.SqlDbType.VarChar, 10)
            arParms(10) = New SqlParameter("@dteLogOnTime", System.Data.SqlDbType.DateTime, 8)
            arParms(11) = New SqlParameter("@intDefaultShiftNo", System.Data.SqlDbType.Int, 4)
            arParms(12) = New SqlParameter("@intOverrideShiftNo", System.Data.SqlDbType.Int, 4)
            arParms(13) = New SqlParameter("@intCasesScheduled", System.Data.SqlDbType.Int, 4)
            arParms(14) = New SqlParameter("@intCasesProduced", System.Data.SqlDbType.Int, 4)
            arParms(15) = New SqlParameter("@intPalletsCreated", System.Data.SqlDbType.Int, 4)
            arParms(16) = New SqlParameter("@decBagLengthUsed", System.Data.SqlDbType.[Decimal], 5)
            arParms(17) = New SqlParameter("@decReworkWgt", System.Data.SqlDbType.[Decimal], 5)
            arParms(18) = New SqlParameter("@intLooseCases", System.Data.SqlDbType.Int, 4)
            arParms(19) = New SqlParameter("@dteProductionDate", System.Data.SqlDbType.DateTime, 8)
            arParms(20) = New SqlParameter("@intCarriedForwardCases", System.Data.SqlDbType.Int, 4)
            arParms(21) = New SqlParameter("@dteShiftProductionDate", System.Data.SqlDbType.DateTime, 8)
            arParms(22) = New SqlParameter("@intCaseCounter", System.Data.SqlDbType.Int, 4)
            arParms(0).Value = "Insert"
            arParms(1).Value = strFacility
            arParms(2).Value = strComputerName
            arParms(3).Value = dteStartTime
            arParms(4).Value = dteStopTime
            arParms(5).Value = strDftPkgLine
            arParms(6).Value = strOvrrPkgLine
            arParms(7).Value = intShopOrder
            arParms(8).Value = strItemNumber
            arParms(9).Value = strOperator
            arParms(10).Value = dteLogOnTime
            arParms(11).Value = strDftShiftNo
            arParms(12).Value = strOvrrShiftNo
            arParms(13).Value = intCasesScheduled
            arParms(14).Value = intCasesProduced
            arParms(15).Value = intPalletsCreated
            arParms(16).Value = sngBagLengthUsed
            arParms(17).Value = sngReworkWgt
            arParms(18).Value = intLooseCases
            arParms(19).Value = dteProductionDate
            arParms(20).Value = intCarriedForwardCases
            arParms(21).Value = dteShiftProductionDate
            arParms(22).Value = intCaseCounter  'WO#755

            strSQLStmt = "CPPsp_SessionControlHstEdit"
            If Not IsNothing(trnServer) Then
                SqlHelper.ExecuteNonQuery(trnServer, CommandType.StoredProcedure, strSQLStmt, arParms)
            Else
                If gblnSvrConnIsUp = True Then
                    strCnnStr = gstrServerConnectionString
                Else
                    strCnnStr = gstrLocalDBConnectionString
                End If
                Try
                    SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.StoredProcedure, strSQLStmt, arParms)
                    'WO#17432   Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
                Catch ex As SqlException When ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                         Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True     'WO#17432
                    SetServerCnnStatusInSessCtl(False)
                    strCnnStr = gstrLocalDBConnectionString
                    SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try
            End If

        Catch ex As Exception
            Throw New Exception("Error in UpdateSessCtlHst" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Function IsLocalConnOK() As Boolean
        'Function: Is Local connection OK?
        Dim cnnLocal As New SqlConnection(gstrLocalDBConnectionString)
        IsLocalConnOK = True
        Try
            cnnLocal.Open()
        Catch ex As SqlException
            Return False
        Catch ex As Exception
            Throw New Exception("Error in IsLocalConnOK" & vbCrLf & ex.Message)
        Finally
            If Not cnnLocal.State <> ConnectionState.Closed Then
                cnnLocal.Close()
            End If
        End Try
    End Function
    Public Shared Function IsSvrConnOK(ByVal blnTryConnect As Boolean, Optional ByVal frm As Form = Nothing) As Boolean
        'Function: Is server connection OK?
        Dim cnnServer As New SqlConnection(gstrServerConnectionString)
        Dim Up As Boolean = True
        Dim Down As Boolean = False
        IsSvrConnOK = True
        gblnSvrConnIsUp = True

        Try
            'Test server connection by trying to connect the server
            If blnTryConnect = True Then
                Cursor.Current = Cursors.WaitCursor
                If My.Computer.Network.IsAvailable Then
                    cnnServer.Open()
                    SetServerCnnStatusInSessCtl(Up)
                    If Not IsNothing(frm) Then
                        RmvMessageLineFromForm(frm)
                    End If
                Else
                    SetServerCnnStatusInSessCtl(Down)
                    If Not IsNothing(frm) Then
                        AddMessageLineToForm(frm, gcstSvrCnnFailure)
                    End If
                End If
            Else
                'Test server connection by checking the flag from the session control record
                gblnSvrConnIsUp = ChgServerCnnStatusFromSessCtl()
                If Not IsNothing(frm) Then
                    If gblnSvrConnIsUp = True Then
                        RmvMessageLineFromForm(frm)
                    Else
                        AddMessageLineToForm(frm, gcstSvrCnnFailure)
                    End If
                End If
                Return gblnSvrConnIsUp
            End If
        Catch ex As SqlException
            SetServerCnnStatusInSessCtl(Down)
            If Not IsNothing(frm) Then
                AddMessageLineToForm(frm, gcstSvrCnnFailure)
            End If
            Return False
        Catch ex As Exception
            Throw New Exception("Error in IsSvrConnOK" & vbCrLf & ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            If cnnServer.State <> ConnectionState.Closed Then
                cnnServer.Close()
            End If
        End Try
    End Function

    Private Shared Sub ChgDataBaseConn(ByVal strCnnType As String)
        Dim strCnnString As String
        Try
            If strCnnType = "SERVER" Then
                strCnnString = My.Settings.ServerPowerPlantCnnStr
            Else
                strCnnString = My.Settings.LocalPowerPlantCnnStr
            End If
            gtaSCH.Connection.ConnectionString = strCnnString
            gtaCompScrap.Connection.ConnectionString = strCnnString
            gtaOperStaff.Connection.ConnectionString = strCnnString
            gtaPallet.Connection.ConnectionString = strCnnString
            gstrConnStr = strCnnString

        Catch ex As Exception
            Throw New Exception("Error in DataBaseConn" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub ImportMasterTables(ByVal strComputerName As String)
        Dim arParms() As SqlParameter
        Dim thdShowSplash As New System.Threading.Thread(AddressOf showSplash)  'WO#359

        Try
            'WO#359 Add Start
            'start another thread to show the Splash screen
            If SharedFunctions.IsDataReadyForRefresh() Then
                Dim strMessage As String
                strMessage = "Exchanging data with server, Please Wait . . ."
                thdShowSplash.Start(strMessage)

                ReDim arParms(0)
                arParms = New SqlParameter(UBound(arParms)) {}

                ' Computer name Input Parameter
                arParms(0) = New SqlParameter("@vchComputerName", SqlDbType.VarChar, 50)
                arParms(0).Value = strComputerName

                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, "LPPsp_CopyImportedDataToProduction", arParms)
            End If
            'WO#359 Add Stop

        Catch ex As Exception
            Throw New Exception("Error in ImportMasterTables" & vbCrLf & ex.Message)
            'WO#359 Add Start
        Finally
            'close the form and end the thread
            'WO#5370 frmSplash.Close()
            Try
                System.Threading.Thread.Sleep(1000)
                If thdShowSplash.IsAlive Then
                    thdShowSplash.Abort()
                End If
            Catch ex As Exception
                Throw New Exception("Error in ImportMasterTables" & vbCrLf & ex.Message)
            End Try
            'WO#359 Add Stop
        End Try
    End Sub

    'WO#359 Add Start
    Public Shared Sub showSplash(ByVal objMessage As Object)
        'WO#5370    frmSplash.lblMessage.Text = CType(objMessage, String)
        frmSplash.SplashMessage = CType(objMessage, String)     'WO#5370
        frmSplash.SplashOption = "ImportData"                   'WO#5370
        frmSplash.SplashTitle = "Refresh data"                  'WO#5370
        frmSplash.ShowDialog()
    End Sub
    'WO#359 Add Stop

    Public Shared Sub RefreshSessionControlTable()
        Try
            Dim tblSessCtl As New dsSessionControl.CPPsp_SessionControlIODataTable
            gtaSessCtl.Fill(tblSessCtl, "SelectAllFields")
            If Not IsNothing(tblSessCtl) And tblSessCtl.Rows.Count > 0 Then
                gdrSessCtl = tblSessCtl.Rows(0)
            End If
        Catch ex As Exception
            Throw New Exception("Error in RefreshSessionControlTable" & vbCrLf & ex.Message)
        End Try
    End Sub

    'WO#650 Public Shared Sub RefreshComputerConfig()
    'WO#5370  Public Shared Sub RefreshComputerConfig(strMyComputerName As String)    'WO#650
    Public Shared Sub RefreshComputerConfig(strMyComputerName As String, Optional ByVal strPackagingLine As String = Nothing) 'WO#5370
        Try
            'Dim strMyComputerName As String = My.Computer.Name
            Dim tblCmpCfg As New dsComputerConfig.CPPsp_ComputerConfigIODataTable
            'WO#650 gtaCmpCfg.Fill(tblCmpCfg, "SelectAllFields", strMyComputerName)
            gtaCmpCfg.Fill(tblCmpCfg, "SelectAllFields", strMyComputerName, Nothing) 'WO#650
            If Not IsNothing(tblCmpCfg) And tblCmpCfg.Rows.Count > 0 Then
                gdrCmpCfg = tblCmpCfg.Rows(0)
                'WO#5370 ADD Start
                If Not IsNothing(strPackagingLine) AndAlso strPackagingLine <> tblCmpCfg.Rows(0).Item("PackagingLine") Then
                    gtaCmpCfg.Fill(tblCmpCfg, "AllActiveInclVirtual", Nothing, strPackagingLine)
                    If Not IsNothing(tblCmpCfg) And tblCmpCfg.Rows.Count > 0 Then
                        gdrCmpCfg = tblCmpCfg.Rows(0)
                        gdrCmpCfg.ComputerName = strMyComputerName
                    End If
                End If
                'WO#5370 ADD Stop

                'WO#2563 DEL Start
                ''WG#650 ADD Start
                'Dim arrProcessType() As String = Split(gdrCmpCfg.ProcessType, ",")
                'For Each StrProcessType As String In arrProcessType
                '    Select Case StrProcessType
                '        Case "1"
                '            'Allow to enter the expiry date of WIP to calculate the expiry date of finised goods.
                '            gblnOvrExpDate = True
                '        Case "2"
                '            'Allow to run 2 shop orders in 1 line temporary until the remaining of the earler shop order is packed.
                '            gbln2SOIn1Line = True
                '            'WO#718 Add Start
                '        Case "3"
                '            'Ajax Automation, get the interface full file name
                '            Dim arrCtlValues As String()
                '            Dim strLabelInputFilePath As String = String.Empty
                '            Dim strLabelOutputFilePath As String = String.Empty

                '            'Get the XML interafce files paths
                '            gblnAutoCaseCountLine = True
                '            arrCtlValues = SharedFunctions.GetConrolTableValues("InterfaceFilePath", "X-Lines/printer")

                '            strLabelInputFilePath = arrCtlValues(0)
                '            strLabelOutputFilePath = arrCtlValues(1)

                '            gstrLabelInputFileName = strLabelInputFilePath & gdrCmpCfg.PackagingLine.TrimEnd & ".xml"
                '            gstrLabelOutputFileName = strLabelOutputFilePath & gdrCmpCfg.PackagingLine.TrimEnd & ".xml"
                '            'WO#718 Add Stop

                '    End Select
                'Next
                ''WG#650 ADD Stop
                'WO#2563 DEL Stop
                'WO#2563 ADD Start
                With gdrCmpCfg
                    'Allow to enter the expiry date of WIP to calculate the expiry date of finised goods.
                    gblnOvrExpDate = .EnableOvrExpDate
                    'Allow to run 2 shop orders in 1 line temporary until the remaining of the earler shop order is packed.
                    gbln2SOIn1Line = .Enable2SOIn1Line
                    'Allow to select output location when creating pallet.
                    gblnEnableOutputLocationLine = .EnableOutputLocation
                    'Ajax Automation, get the interface full file name
                    'WO#5370    gblnAutoCaseCountLine = .EnableAutoCaseCountLine
                    'WO#5370 ADD Start
                    gblnSarongAutoCountLine = False
                    If Not IsNothing(.AutoCountUnit) Then
                        gblnAutoCountLine = True
                        If .AutoCountUnit.ToUpper = "CASE" Then
                            gintAutoCountByUnit = AutoCountBy.Cases
                        Else
                            gintAutoCountByUnit = AutoCountBy.Pallets
                        End If

                        If .InterfaceType.ToUpper = "XML" Then
                            'WO#5370 ADD Stop

                            'WO#5370 If .EnableAutoCaseCountLine Then
                            'Ajax Automation, get the interface full file name
                            Dim arrCtlValues As String()
                            Dim strLabelInputFilePath As String = String.Empty
                            Dim strLabelOutputFilePath As String = String.Empty

                            'Get the XML interafce files paths
                            'WO#5370 gblnAutoCaseCountLine = True
                            arrCtlValues = SharedFunctions.GetConrolTableValues("InterfaceFilePath", "X-Lines/printer")

                            strLabelInputFilePath = arrCtlValues(0)
                            strLabelOutputFilePath = arrCtlValues(1)

                            gstrLabelInputFileName = strLabelInputFilePath & gdrCmpCfg.PackagingLine.TrimEnd & ".xml"
                            gstrLabelOutputFileName = strLabelOutputFilePath & gdrCmpCfg.PackagingLine.TrimEnd & ".xml"
                            'WO#5370 ADD Start
                        ElseIf .InterfaceType = "SQL" Then
                            If gintAutoCountByUnit = AutoCountBy.Pallets AndAlso .EnableOutputLocation = True Then
                                gblnSarongAutoCountLine = True
                            End If
                        End If
                        'WO#5370 ADD Start
                    Else
                        gblnAutoCountLine = False
                        'WO#5370 ADD Stop
                    End If

                End With
                'WO#2563 ADD Stop
            End If

        Catch ex As Exception
            Throw New Exception("Error in RefreshComputerConfig" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Function GetProductionDateByShift(ByVal intOverrideShift As Integer, ByVal dteGivenDateTime As DateTime) As DateTime
        'Get production date by shift
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Try
            ReDim arParms(5)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Faccility Input Parameter
            arParms(0) = New SqlParameter("@chrFacility", SqlDbType.Char, 3)
            arParms(0).Value = gdrSessCtl("Facility")

            ' Overrided Shift Input Parameter
            arParms(1) = New SqlParameter("@intGivenShiftNo", SqlDbType.TinyInt)
            arParms(1).Value = intOverrideShift

            ' Current Data time Input Parameter
            arParms(2) = New SqlParameter("@dteGivenDateTime", SqlDbType.DateTime)
            arParms(2).Value = dteGivenDateTime

            ' Machine ID Input Parameter
            arParms(3) = New SqlParameter("@vchMachineID", SqlDbType.VarChar, 10)
            arParms(3).Value = gdrSessCtl.DefaultPkgLine

            ' Work Group Input Parameter
            arParms(4) = New SqlParameter("@vchWorkGroup", SqlDbType.VarChar, 3)
            arParms(4).Value = Nothing

            ' Production Date Output Parameter
            arParms(5) = New SqlParameter("@dteProductionDate", SqlDbType.DateTime)
            arParms(5).Direction = ParameterDirection.Output

            strSQLStmt = "CPPsp_GetProdDateByShift"
            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            GetProductionDateByShift = arParms(5).Value
        Catch ex As Exception
            Throw New Exception("Error in GetProductionDateByShift" & vbCrLf & ex.Message)
        End Try
    End Function

    'Public Shared Function GetCaseProducedByShift(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShiftNo As Integer, _
    '                             ByVal dteProductionDate As DateTime) As Integer
    '    'Get Total Cases Produced by SO/Production date/shift

    '    'Dim strSQLStmt As String
    '    Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
    '    Dim dr As dsSessionControlHst.CPPsp_SessionControlHstIORow
    '    Dim intLooseCases As Integer
    '    Try

    '        'strSQLStmt = "CPPsp_SessionControlHstIO"

    '        GetCaseProducedByShift = 0
    '        Try
    '            gtaSCH.Fill(tblSCH, "By_Line_SO_Shift_PrdDate", strPkgLine, intShopOrder, intShiftNo, dteProductionDate, New System.Nullable(Of Date), gdrSessCtl.Facility, String.Empty)
    '        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
    '            SharedFunctions.SetServerCnnStatusInSessCtl(False)
    '        End Try

    '        If tblSCH.Rows.Count > 0 Then
    '            For Each dr In tblSCH.Rows
    '                'GetCaseProducedByShift = tblSCH.Rows(0)("CasesProduced")
    '                GetCaseProducedByShift = GetCaseProducedByShift + dr.CasesProduced
    '                intLooseCases = dr.LooseCases
    '            Next
    '            GetCaseProducedByShift = GetCaseProducedByShift + intLooseCases
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception("Error in GetCaseProducedByShift" & vbCrLf & ex.Message)
    '    End Try
    'End Function

    'Public Shared Function GetCasedProducedByLineSO(ByVal strPkgLine As String, ByVal intShopOrder As Integer) As Integer

    '    Dim arParms() As SqlParameter = New SqlParameter(2) {}
    '    Dim strSQLStmt As String = Nothing
    '    Dim ds As DataSet
    '    Try
    '        GetCasedProducedByLineSO = 0

    '        'Action Input Parameter
    '        arParms(0) = New SqlParameter("@vchAction", SqlDbType.VarChar, 30)
    '        arParms(0).Value = "SelectLastRecByLineSO"

    '        'Shop Order Input Parameter
    '        arParms(1) = New SqlParameter("@chrPkgLine", SqlDbType.Char, 10)
    '        arParms(1).Value = strPkgLine

    '        'Shop Order Input Parameter
    '        arParms(2) = New SqlParameter("@intShopOrder", SqlDbType.Int)
    '        arParms(2).Value = intShopOrder

    '        'strSQLStmt = "SELECT SUM(casesProduced) AS TotalCasesProducted FROM tblSessionControlHst WHERE RRN IN " & _
    '        '             "(SELECT MAX(RRN) AS ShiftLastRec FROM tblSessionControlHst " & _
    '        '             "WHERE ShopOrder = @intShopOrder AND DefaultPkgLine = @chrPkgLine " & _
    '        '             "GROUP BY  DefaultPkgLine, ShopOrder, ProductionDate, OverrideShiftNo)"

    '        If gblnSvrConnIsUp = True Then
    '            GetCasedProducedByLineSO = SqlHelper.ExecuteScalar(gstrServerConnectionString, CommandType.Text, strSQLStmt, arParms)
    '            strSQLStmt = "CPPsp_SessionControlHstIO"
    '            ds = SqlHelper.ExecuteDataset(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
    '        Else
    '            GetCasedProducedByLineSO = SqlHelper.ExecuteScalar(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
    '            strSQLStmt = "CPPsp_SessionControlHstIO"
    '            ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
    '        End If
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            GetCasedProducedByLineSO = GetCasedProducedByLineSO + ds.Tables(0).Rows(0)("LooseCases")
    '        End If
    '    Catch ex As SqlException
    '        SharedFunctions.SetServerCnnStatusInSessCtl(False)
    '    Catch ex As Exception
    '        Throw New Exception("Error in GetCasedProducedByLineSO" & vbCrLf & ex.Message)
    '    End Try

    'End Function

    Public Shared Sub printCaseLabel(ByVal strDeviceType As String, ByVal strLotID As String)
        'print case label from the default line and for the default line
        Dim dteProductionDate As DateTime
        Dim strExpirydate As String = ""
        Try
            Dim strLabelType As String = CASELABEL
            With gdrSessCtl
                Dim strFacility As String = .Item("Facility")
                Dim strDftPkgLine As String = .Item("DefaultPkgLine")
                Dim strOvrPkgLine As String = .DefaultPkgLine
                Dim intShopOrder As Integer = .Item("ShopOrder")
                Dim strItemNumber As String = .Item("ItemNumber")
                Dim intQuantity As String = 0
                Dim strOperator As String = .Operator
                Dim dteStartTime As DateTime = .Item("StartTime")
                Dim strJobName As String = .Item("DefaultPkgLine") & CType(.Item("ShopOrder"), String)
                dteProductionDate = Now()

                'WO#650 Add Start
                If gblnOvrExpDate = True AndAlso (drSO.DateToPrintFlag = "1" Or drSO.DateToPrintFlag = "3") Then
                    strExpirydate = gdteExpiryDate.ToString("yyyyMMdd")
                Else
                    strExpirydate = ""
                End If
                'WO#650 Add Stop
                'For printing case label from the original line, the overrode packaging line should not be applied, use default packaging line value instead.
                'WO#654 If strDeviceType = CASELABEL Then
                If strDeviceType = CASELABEL AndAlso gblnStartSOWithNoLabel = False Then   'WO#654
                    'WO#650 ADD Start
                    SharedFunctions.PrintDiffLabels(strLabelType, strFacility, strDftPkgLine, strDftPkgLine,
                                intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime,
                                strDeviceType, gdrCmpCfg("PalletStation"), strLotID, Format(dteProductionDate, "yyyyMMdd"),
                                strJobName, gdrSessCtl("OverrideShiftNo"), gdrCmpCfg("NoOfLabels"), strExpirydate)

                    'hard code the machine number "3980" here to put put the x label key on coder, as didn't have chance to test on all machines
                    'otherwise it might affect to the x code on existing machines
                ElseIf strDeviceType = PACKAGECODER AndAlso gblnStartSOWithNoLabel = False AndAlso strDftPkgLine.TrimEnd.Contains("3980") Then
                    SharedFunctions.PrintDiffLabels(PACKAGELABEL, strFacility, strDftPkgLine, strDftPkgLine,
                                    intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime,
                                    strDeviceType, gdrCmpCfg("PalletStation"), strLotID, Format(dteProductionDate, "yyyyMMdd"),
                                    strJobName & PACKAGELABEL, gdrSessCtl("OverrideShiftNo"), gdrCmpCfg("NoOfLabels"), strExpirydate)

                ElseIf strDeviceType = PACKAGECODER AndAlso gblnStartSOWithNoLabel = False Then
                    SharedFunctions.PrintDiffLabels(strLabelType, strFacility, strDftPkgLine, strDftPkgLine,
                                    intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime,
                                    strDeviceType, gdrCmpCfg("PalletStation"), strLotID, Format(dteProductionDate, "yyyyMMdd"),
                                    strJobName & PACKAGELABEL, gdrSessCtl("OverrideShiftNo"), gdrCmpCfg("NoOfLabels"), strExpirydate)

                Else
                    SharedFunctions.PrintDiffLabels(strLabelType, strFacility, strDftPkgLine, strDftPkgLine,
                            intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime,
                            strDeviceType, gdrCmpCfg("PalletStation"), strLotID, Format(dteProductionDate, "yyyyMMdd"),
                            strJobName, gdrSessCtl("OverrideShiftNo"), , strExpirydate)
                    'WO#650 ADD Stop
                    'WO#650 DEL Start
                    '    SharedFunctions.PrintDiffLabels(strLabelType, strFacility, strDftPkgLine, strDftPkgLine, _
                    '                intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime, _
                    '                strDeviceType, gdrCmpCfg("PalletStation"), strLotID, Format(dteProductionDate, "yyyyMMdd"), _
                    '                strJobName, gdrSessCtl("OverrideShiftNo"), gdrCmpCfg("NoOfLabels"), strExpirydate)
                    'Else
                    '    SharedFunctions.PrintDiffLabels(strLabelType, strFacility, strDftPkgLine, strDftPkgLine, _
                    '                intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime, _
                    '                strDeviceType, gdrCmpCfg("PalletStation"), strLotID, Format(dteProductionDate, "yyyyMMdd"), _
                    '                strJobName, gdrSessCtl("OverrideShiftNo"))
                    'WO#650 DEL Stop
                End If

            End With
        Catch ex As SqlException When ex.ErrorCode = -2146232060
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in printCaseLabel" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub ChangeSOStatus(ByVal intShopOrder As String, ByVal enShopOrderStatus As ShopOrderStatus, Optional ByVal trnServer As SqlTransaction = Nothing)
        Dim arParms() As SqlParameter = New SqlParameter(0) {}
        Dim strSQLStmt As String
        Try

            'Shop Order Input Parameter
            arParms(0) = New SqlParameter("intShopOrder", SqlDbType.Int)
            arParms(0).Value = intShopOrder

            If enShopOrderStatus = ShopOrderStatus.Open Then
                strSQLStmt = "UPDATE tblShopOrder SET Closed = NULL WHERE ShopOrder = @intShopOrder"
            Else
                strSQLStmt = "UPDATE tblShopOrder SET Closed = 1 WHERE ShopOrder = @intShopOrder"
            End If

            'Update Shop order table for close shop order on local data base
            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)

            If gblnSvrConnIsUp = True Then
                'Update Shop order table for close shop order on server data base
                Try
                    If trnServer Is Nothing Then
                        SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.Text, strSQLStmt, arParms)
                    Else
                        SqlHelper.ExecuteNonQuery(trnServer, CommandType.Text, strSQLStmt, arParms)
                    End If
                Catch ex As SqlException
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                End Try
            End If

        Catch ex As Exception
            Throw New Exception("Error in ChangeSOStatus" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Function GetRunningInstance(ByVal processName As String) As Process
        Try
            Dim proclist() As Process = Process.GetProcessesByName(processName)
            'if the process(es) has the same name but not the same ID,
            '(that means same "program" are running more than one time), return the process .
            For Each p As Process In proclist
                If p.Id <> Process.GetCurrentProcess().Id Then
                    Return p
                End If
            Next
            Return Nothing
        Catch ex As Exception
            Throw New Exception("Error in GetRunningInstance" & vbCrLf & ex.Message)
        End Try
    End Function
    Public Shared Sub AddMessageLineToForm(ByVal frm As Form, ByVal strMsg As String)
        Try
            'RmvMessageLineFromForm(frm)
            'Dim lblNote As New Label
            'With lblNote
            '    .Name = "lblMessage"
            '    .Text = strMsg
            '    .Location = New Point(22, 565)
            '    .ForeColor = Color.LightSalmon
            '    .AutoSize = True
            '    .Font = New Font(FontFamily.GenericSansSerif, 18.0F, FontStyle.Bold)
            'End With
            'frm.Controls.Add(lblNote)
            If Not IsNothing(frm.Controls.Item("lblMessage")) Then
                With frm.Controls.Item("lblMessage")
                    .Text = strMsg
                    .Visible = True
                End With
            End If
        Catch ex As Exception
            Throw New Exception("Error in AddMessageLineToForm" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub RmvMessageLineFromForm(ByVal frm As Form)
        'Dim ctl As Control
        'For Each ctl In frm.Controls
        '    If ctl.Name = "lblMessage" AndAlso TypeOf ctl Is Label Then
        '        frm.Controls.Remove(ctl)
        '    End If
        'Next
        If Not IsNothing(frm.Controls.Item("lblMessage")) Then
            With frm.Controls.Item("lblMessage")
                .Visible = False
            End With
        End If
    End Sub

    Public Shared Sub SetServerCnnStatusInSessCtl(ByVal blnStatus As Boolean)
        Dim arParms() As SqlParameter = New SqlParameter(0) {}
        Dim strsqlstmt As String
        Try
            'Server Status Input Parameter
            arParms(0) = New SqlParameter("bitStatus", SqlDbType.Bit)
            arParms(0).Value = blnStatus

            strsqlstmt = "UPDATE tblSessionControl SET ServerCnnIsOK = @bitStatus"
            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strsqlstmt, arParms)
            RefreshSessionControlTable()
            If blnStatus = True Then
                ChgDataBaseConn("SERVER")
            Else
                ChgDataBaseConn("LOCAL")
            End If

            gblnSvrConnIsUp = blnStatus

        Catch ex As Exception
            Throw New Exception("Error in SetServerCnnStatusInSessCtl" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Function ChgServerCnnStatusFromSessCtl() As Boolean
        Dim strsqlstmt As String
        Try
            strsqlstmt = "Select ServerCnnIsOk from tblSessionControl"
            ChgServerCnnStatusFromSessCtl = CType(SqlHelper.ExecuteScalar(gstrLocalDBConnectionString, CommandType.Text, strsqlstmt), Boolean)
            If ChgServerCnnStatusFromSessCtl = True Then
                ChgDataBaseConn("SERVER")
            Else
                ChgDataBaseConn("LOCAL")
            End If
        Catch ex As Exception
            Throw New Exception("Error in ChgServerCnnStatusFromSessCtl" & vbCrLf & ex.Message)
        End Try
    End Function
    Public Shared Function uploadLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing
        Dim blnFlagFirst As Boolean
        Dim strSqlInsert As String
        Dim intColIndex As Short
        Dim typeCode As TypeCode
        Dim sbValues As New StringBuilder()

        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            uploadLocalDataToServer = True

            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Try
                'cnnServer = New SqlConnection(gstrServerConnectionString)
                'cnnServer.Open()
                'Catch ex As SqlException
                '    Exit Function
                'End Try

                'Read the required Table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then

                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()

                    For Each dr In ds.Tables(0).Rows
                        'If the last update to the server failure, exit the loop
                        If gblnSvrConnIsUp = False Then
                            Exit For
                        End If
                        With dr
                            blnFlagFirst = True
                            sbValues.Length = 0
                            For intColIndex = 0 To (dr.Table.Columns.Count - 1) 'Loop through all fields of one the current record
                                If dr.Table.Columns.Item(intColIndex).ColumnName <> "RRN" Then
                                    If blnFlagFirst = False Then sbValues = sbValues.Append(",")
                                    blnFlagFirst = False
                                    typeCode = Type.GetTypeCode(dr.Item(intColIndex).GetType)
                                    Select Case typeCode
                                        Case TypeCode.Boolean
                                            If dr.Item(intColIndex) Then
                                                sbValues = sbValues.Append(1)
                                            Else
                                                sbValues = sbValues.Append(0)
                                            End If
                                            'Numeric value
                                        Case TypeCode.Byte, System.TypeCode.Decimal, System.TypeCode.Double,
                                            System.TypeCode.Int16, System.TypeCode.Int32, System.TypeCode.Int64, System.TypeCode.Single
                                            sbValues = sbValues.Append(dr.Item(intColIndex))
                                        Case TypeCode.DateTime
                                            sbValues = sbValues.Append("'")
                                            sbValues = sbValues.Append(String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", dr.Item(intColIndex)))
                                            sbValues = sbValues.Append("'")
                                        Case TypeCode.DBNull
                                            sbValues = sbValues.Append("NULL")
                                        Case Else   'Alphanumeric value
                                            sbValues = sbValues.Append("'")
                                            sbValues = sbValues.Append(dr.Item(intColIndex))
                                            sbValues = sbValues.Append("'")
                                    End Select
                                End If
                                Debug.Print(sbValues.ToString)
                            Next

                            strSQLStmt = "INSERT INTO " & strTableName & " VALUES(" & sbValues.ToString & ")"
                            SqlHelper.ExecuteNonQuery(trnServer, CommandType.Text, strSQLStmt)
                            blnFlagFirst = 0
                            strSqlInsert = ""

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"

                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            Throw New Exception("Error in uploadLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            uploadLocalDataToServer = False
            Throw New Exception("Error in uploadLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    ' WO#17432 ADD Start 
    Public Shared Function UploadQATWeightHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTestTime As Nullable(Of DateTime)
        Dim decActualWeight As Decimal
        Dim blnLastSample As Nullable(Of Boolean)
        Dim intLaneNo As Integer
        Dim intSampleNo As Integer
        Dim blnDetailTestResult As Nullable(Of Boolean)

        Dim dteOverrideID As Nullable(Of DateTime)
        Dim strInterfaceID As String = Nothing
        Dim decTareWeight As Nullable(Of Decimal)
        Dim intRetestNo As Nullable(Of Integer)
        Dim dteTestEndTime As Nullable(Of DateTime)

        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATWeightHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            If Not IsDBNull(dr.Item("OverrideID")) Then
                                dteOverrideID = dr.Item("OverrideID")
                            End If
                            If Not IsDBNull(.Item("TareWeight")) Then
                                decTareWeight = .Item("TareWeight")
                            End If

                            intRetestNo = Convert.ToInt16(dr.Item("RetestNo"))

                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If

                            SharedFunctions.SaveQATSmallestSalesUnitWeight(
                                               .Item("BatchID"), .Item("Facility") _
                                               , .Item("InterfaceID") _
                                               , .Item("MaxWeight"), .Item("MinWeight") _
                                               , dteOverrideID _
                                               , .Item("PackagingLine") _
                                                , intRetestNo _
                                               , .Item("ShopOrder") _
                                               , .Item("SOStartTime") _
                                               , decTareWeight _
                                               , .Item("TargetWeight") _
                                               , dteTestEndTime _
                                               , .Item("TestResult") _
                                               , .Item("TestStartTime") _
                                                , .Item("TesterID") _
                                                , .Item("QATEntryPoint") _
                                               , decActualWeight, blnLastSample, intLaneNo, intSampleNo, blnDetailTestResult, dteTestTime)

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            UploadQATWeightHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATWeightHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATWeightHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATWeightHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    Public Shared Function UploadQATStartUpHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTaskEndTime As Nullable(Of DateTime)
        Dim dteTaskStartTime As Nullable(Of DateTime)
        Dim intTaskID As Nullable(Of Integer)
        Dim intTaskStatus As Nullable(Of Integer)
        Dim intByPassAllTest As Nullable(Of Integer)
        Dim dteTestEndTime As Nullable(Of DateTime)
        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATStartUpHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            intByPassAllTest = Convert.ToInt16(dr.Item("ByPassAllTest"))
                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If
                            SharedFunctions.SaveQATStartUpChecks(
                                        .Item("BatchID"), intByPassAllTest,
                                        .Item("Facility"), .Item("InterfaceID"),
                                        .Item("PackagingLine"), .Item("QATEntryPoint"), .Item("ShopOrder"),
                                         .Item("SOStartTime"), .Item("TestEndTime"),
                                        .Item("TestStartTime"),
                                        dteTaskEndTime, intTaskID, dteTaskStartTime, intTaskStatus, .Item("TesterID"))

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATStartUpHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATLineClearanceHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    'Public Shared Function UploadQATLineClearanceHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
    '    Dim arParms() As SqlParameter
    '    Dim strSQLStmt As String = ""
    '    Dim ds As New DataSet
    '    Dim dr As DataRow = Nothing
    '    Dim cnnServer As SqlConnection = Nothing
    '    Dim trnServer As SqlTransaction = Nothing

    '    Dim dteTaskEndTime As Nullable(Of DateTime)
    '    Dim dteTaskStartTime As Nullable(Of DateTime)
    '    Dim intTaskID As Nullable(Of Integer)
    '    Dim intTaskStatus As Nullable(Of Integer)
    '    Dim intByPassAllTest As Nullable(Of Integer)
    '    Dim dteTestEndTime As Nullable(Of DateTime)
    '    Try
    '        ReDim arParms(0)
    '        arParms = New SqlParameter(UBound(arParms)) {}

    '        UploadQATLineClearanceHeaderLocalDataToServer = True
    '        'if the connection to server failure, end the routine
    '        If gblnSvrConnIsUp = True Then
    '            'Read transaction table from local data base
    '            strSQLStmt = "select * from " & strTableName
    '            ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                cnnServer = New SqlConnection(gstrServerConnectionString)
    '                cnnServer.Open()
    '                trnServer = cnnServer.BeginTransaction()
    '                For Each dr In ds.Tables(0).Rows
    '                    With dr
    '                        intByPassAllTest = Convert.ToInt16(dr.Item("ByPassAllTest"))
    '                        If Not IsDBNull(.Item("TestEndTime")) Then
    '                            dteTestEndTime = .Item("TestEndTime")
    '                        End If
    '                        SharedFunctions.SaveQATLineClearance( _
    '                                    .Item("BatchID"), intByPassAllTest, _
    '                                    .Item("Facility"), .Item("InterfaceID"), _
    '                                    .Item("PackagingLine"), .Item("ShopOrder"), _
    '                                    .Item("SOStartTime"), dteTestEndTime, _
    '                                    .Item("TestStartTime"), .Item("TesterID"), .Item("QATEntryPoint"), _
    '                                    dteTaskEndTime, intTaskID, dteTaskStartTime, intTaskStatus)

    '                        arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
    '                        arParms(0).Value = .Item("RRN")

    '                        strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
    '                        SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
    '                    End With

    '                Next
    '                If Not trnServer Is Nothing Then
    '                    Try
    '                        trnServer.Commit()
    '                    Catch ex As Exception
    '                        If Not IsNothing(trnServer) Then
    '                            Try
    '                                trnServer.Rollback()
    '                            Catch exRollback As Exception
    '                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
    '                            End Try
    '                        End If
    '                    End Try
    '                End If
    '            End If
    '        End If
    '    Catch ex As SqlException
    '        If Not IsNothing(trnServer) Then
    '            trnServer.Rollback()
    '        End If
    '        SetServerCnnStatusInSessCtl(False)
    '        UploadQATLineClearanceHeaderLocalDataToServer = False
    '        Throw New Exception("Error in UploadQATLineClearanceHeaderLocalDataToServer" & vbCrLf & ex.Message)
    '    Catch ex As Exception
    '        If Not IsNothing(trnServer) Then
    '            trnServer.Rollback()
    '        End If
    '        UploadQATLineClearanceHeaderLocalDataToServer = False
    '        Throw New Exception("Error in UploadQATLineClearanceHeaderLocalDataToServer" & vbCrLf & ex.Message)
    '    Finally
    '        If Not IsNothing(cnnServer) Then
    '            cnnServer.Close()
    '        End If
    '        ds = Nothing
    '    End Try
    'End Function

    Public Shared Function UploadQATMaterialsValidationHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTestEndTime As Nullable(Of DateTime)
        Dim dteTestTime As Nullable(Of DateTime)
        Dim dteOverrideID As Nullable(Of DateTime)
        Dim strComponentNo As String = Nothing
        Dim strScannedComponentNo As String = Nothing
        Dim strScannedLotNo As String = Nothing
        Dim blnTestResult As Nullable(Of Boolean)

        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATMaterialsValidationHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If
                            SharedFunctions.SaveQATMaterialsValidation(
                                                .Item("BatchID"), .Item("Facility"),
                                                .Item("InterFaceID"), .Item("PackagingLine"),
                                                .Item("ShopOrder"), .Item("SOStartTime"),
                                                dteTestEndTime, .Item("TestStartTime"),
                                                .Item("TesterID"), .Item("QATEntryPoint"),
                                                strComponentNo, dteOverrideID, strScannedComponentNo, strScannedLotNo, blnTestResult, dteTestTime)

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            UploadQATMaterialsValidationHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATMaterialsValidationHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATMaterialsValidationHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATMaterialsValidationHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    Public Shared Function UploadQATOxygenHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTestEndTime As Nullable(Of DateTime)
        Dim dteTestTime As Nullable(Of DateTime)
        Dim dteOverrideID As Nullable(Of DateTime)
        Dim intRetestNo As Nullable(Of Integer)

        Dim decOxygen As Nullable(Of Decimal)
        Dim blnLastSample As Nullable(Of Boolean)
        Dim intLaneNo As Nullable(Of Integer)
        Dim intSampleNo As Nullable(Of Integer)
        Dim blnDetailTestResult As Nullable(Of Boolean)
        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATOxygenHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            If Not IsDBNull(dr.Item("OverrideID")) Then
                                dteOverrideID = dr.Item("OverrideID")
                            End If

                            intRetestNo = Convert.ToInt16(dr.Item("RetestNo"))

                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If

                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If
                            SharedFunctions.SaveQATOxygen(
                                        .Item("BatchID"), .Item("Facility"),
                                        .Item("InterfaceID"), .Item("MaxOxygen"),
                                        dteOverrideID, .Item("ShopOrder"),
                                        .Item("SOStartTime"), .Item("PackagingLine"),
                                        intRetestNo, dteTestEndTime,
                                        .Item("TestResult"), .Item("TestStartTime"),
                                        .Item("TesterID"), .Item("QATEntryPoint"),
                                        decOxygen, blnLastSample, intLaneNo, intSampleNo, blnDetailTestResult, dteTestTime)

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            UploadQATOxygenHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATOxygenHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATOxygenHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATOxygenHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    Public Shared Function UploadQATCheckWeigherHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTestEndTime As Nullable(Of DateTime)
        Dim dteTestTime As Nullable(Of DateTime)
        Dim dteOverrideID As Nullable(Of DateTime)
        Dim intRetestNo As Nullable(Of Integer)

        Dim blnDetailTestResult As Nullable(Of Boolean)

        Dim strRecipe As String = Nothing
        Dim decActualWeight As Nullable(Of Decimal)
        Dim decMaxWeight As Nullable(Of Decimal)
        Dim decMinWeight As Nullable(Of Decimal)
        Dim decTareWeight As Nullable(Of Decimal)
        Dim decTargetWeight As Nullable(Of Decimal)

        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATCheckWeigherHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            If Not IsDBNull(dr.Item("OverrideID")) Then
                                dteOverrideID = dr.Item("OverrideID")
                            End If

                            intRetestNo = Convert.ToInt16(dr.Item("RetestNo"))

                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If

                            SharedFunctions.SaveQATCheckWeigherResult(
                                        .Item("BatchID"), .Item("Facility"),
                                        .Item("InterfaceID"), dteOverrideID,
                                        .Item("ShopOrder"), .Item("SOStartTime"),
                                        .Item("PackagingLine"), intRetestNo,
                                        dteTestEndTime, .Item("TestResult"), .Item("TestStartTime"), .Item("TesterID"), .Item("QATEntryPoint"),
                                        decActualWeight, decMaxWeight, decMinWeight, strRecipe, decTareWeight, decTargetWeight, blnDetailTestResult, dteTestTime)

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            UploadQATCheckWeigherHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATCheckWeigherHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATCheckWeigherHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATCheckWeigherHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    Public Shared Function UploadQATPressureHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTestEndTime As Nullable(Of DateTime)
        Dim dteTestTime As Nullable(Of DateTime)
        Dim dteOverrideID As Nullable(Of DateTime)
        Dim intRetestNo As Nullable(Of Integer)

        Dim intLaneNo As Nullable(Of Integer)
        Dim intSampleNo As Nullable(Of Integer)
        Dim blnDetailTestResult As Nullable(Of Boolean)

        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATPressureHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            If Not IsDBNull(dr.Item("OverrideID")) Then
                                dteOverrideID = dr.Item("OverrideID")
                            End If

                            intRetestNo = Convert.ToInt16(dr.Item("RetestNo"))

                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If

                            SharedFunctions.SaveQATPressure(
                                        .Item("BatchID"), .Item("Facility"),
                                        .Item("InterfaceID"), dteOverrideID,
                                        .Item("ShopOrder"), .Item("SOStartTime"),
                                        .Item("PackagingLine"), intRetestNo,
                                        dteTestEndTime, .Item("TestResult"), .Item("TestStartTime"),
                                        .Item("TesterID"), .Item("QATEntryPoint"),
                                        intLaneNo, intSampleNo, blnDetailTestResult, dteTestTime)

                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            UploadQATPressureHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATPressureHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATPressureHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATPressureHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    'WO#17432 ADD Start – AT 11/15/2018
    Public Shared Function UploadQATDateCodeHeaderLocalDataToServer(ByVal strTableName As String) As Boolean
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String = ""
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim cnnServer As SqlConnection = Nothing
        Dim trnServer As SqlTransaction = Nothing

        Dim dteTestEndTime As Nullable(Of DateTime)
        Dim dteTestTime As Nullable(Of DateTime)
        'WO#17432 AT 12/10/2018    Dim dteOverrideID As Nullable(Of DateTime)
        Dim intRetestNo As Nullable(Of Integer)

        Dim strDateCodeValue As String = String.Empty
        'WO#17432 AT 12/10/2018     Dim blnTestResult As Nullable(Of Boolean)

        Try
            ReDim arParms(0)
            arParms = New SqlParameter(UBound(arParms)) {}

            UploadQATDateCodeHeaderLocalDataToServer = True
            'if the connection to server failure, end the routine
            If gblnSvrConnIsUp = True Then
                'Read transaction table from local data base
                strSQLStmt = "select * from " & strTableName
                ds = SqlHelper.ExecuteDataset(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt)
                If ds.Tables(0).Rows.Count > 0 Then
                    cnnServer = New SqlConnection(gstrServerConnectionString)
                    cnnServer.Open()
                    trnServer = cnnServer.BeginTransaction()
                    For Each dr In ds.Tables(0).Rows
                        With dr
                            'WO#17432 DEL Start – AT 12/10/2018
                            'If Not IsDBNull(dr.Item("OverrideID")) Then
                            '    dteOverrideID = dr.Item("OverrideID")
                            'End If
                            'WO#17432 DEL Stop – AT 12/10/2018
                            intRetestNo = Convert.ToInt16(dr.Item("RetestNo"))

                            If Not IsDBNull(.Item("TestEndTime")) Then
                                dteTestEndTime = .Item("TestEndTime")
                            End If

                            'SharedFunctions.SaveQATDateCodeResult_2( _
                            '    .Item("BatchID") _
                            '    , strDateCodeValue _
                            '    , .Item("DateCodeType") _
                            '    , .Item("Facility") _
                            '    , .Item("InterfaceID") _
                            '    , .Item("PackagingLine") _
                            '    , intRetestNo _
                            '    , .Item("ShopOrder") _
                            '    , .Item("SOStartTime") _
                            '     , dteTestEndTime _
                            '                                     , .Item("TestResult") _
                            '    )
                            'WO#17432 ADD Start – AT 12/10/2018
                            SharedFunctions.SaveQATDateCodeResult(
                                .Item("BatchID") _
                                , strDateCodeValue _
                                , .Item("DateCodeType") _
                                , .Item("Facility") _
                                , .Item("InterfaceID") _
                                , .Item("PackagingLine") _
                                , intRetestNo _
                                , .Item("ShopOrder") _
                                , .Item("SOStartTime") _
                                , dteTestEndTime _
                                , .Item("TestResult") _
                                , .Item("TestStartTime") _
                                , dteTestTime _
                                , .Item("TesterID") _
                                , .Item("QATEntryPoint")
                                )
                            'WO#17432 ADD Stop – AT 12/10/2018
                            'WO#17432 DEL Start – AT 12/10/2018
                            'SharedFunctions.SaveQATDateCodeResult( _
                            '            .Item("BatchID"), strDateCodeValue, _
                            '           .Item("DateCodeType"), .Item("Facility"), _
                            '            .Item("InterfaceID"), .Item("PackagingLine"), _
                            '            intRetestNo, .Item("ShopOrder"), _
                            '            .Item("SOStartTime"), dteTestEndTime, _
                            '            blnTestResult, .Item("TestStartTime"), dteTestTime)
                            'WO#17432 DEL Stop – AT 12/10/2018
                            arParms(0) = New SqlParameter("@intRRN", SqlDbType.Int)
                            arParms(0).Value = .Item("RRN")

                            strSQLStmt = "Delete " & strTableName & " WHERE RRN = @intRRN"
                            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                        End With

                    Next
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
                End If
            End If
        Catch ex As SqlException
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            SetServerCnnStatusInSessCtl(False)
            UploadQATDateCodeHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATDateCodeHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Catch ex As Exception
            If Not IsNothing(trnServer) Then
                trnServer.Rollback()
            End If
            UploadQATDateCodeHeaderLocalDataToServer = False
            Throw New Exception("Error in UploadQATDateCodeHeaderLocalDataToServer" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnServer) Then
                cnnServer.Close()
            End If
            ds = Nothing
        End Try
    End Function

    Public Shared Function GetLineEfficiency(ByVal strFacility As String,
                    ByVal strPkgLine As String,
                    ByVal strStaffID As String,
                    ByVal intShift As System.Nullable(Of Integer),
                    ByVal intShopOrder As System.Nullable(Of Integer),
                    ByVal dteDateTime As System.Nullable(Of Date),
                    ByVal dte_sc_StartTime As System.Nullable(Of Date),
                    ByVal str_Sc_DefaultPkgLine As String,
                    ByVal str_Sc_OverridePkgLine As String,
                    ByVal int_Sc_ShopOrder As System.Nullable(Of Integer),
                    ByVal str_Sc_ItemNumber As String,
                    ByVal str_Sc_Operator As String,
                    ByVal str_Sc_DefaultShiftNo As String,
                    ByVal str_Sc_OverrideShiftNo As String,
                    ByVal int_Sc_CasesScheduled As System.Nullable(Of Integer),
                    ByVal int_Sc_CasesProduced As System.Nullable(Of Integer),
                    ByVal int_Sc_PalletsCreated As System.Nullable(Of Integer),
                    ByVal int_Sc_LooseCases As System.Nullable(Of Integer),
                    ByVal dte_Sc_ProductionDate As System.Nullable(Of Date),
                    ByVal int_Sc_CarriedForwardCases As System.Nullable(Of Integer),
                    ByVal dte_Sc_ShiftProductionDate As System.Nullable(Of Date),
                    ByRef int_So_Act_Produced As Integer,
                    ByRef int_So_Act_CsPerHour As Integer,
                    ByRef int_So_Sch_CsPerHour As Integer,
                    ByRef dec_So_Efficiency As Decimal,
                    ByRef bln_So_Alert As Boolean,
                    ByRef int_So_Progress As Integer,
                    ByRef int_Sft_Act_Produced As Integer,
                    ByRef int_Sft_Act_CsPerHour As Integer,
                    ByRef int_Sft_Sch_Produced As Integer,
                    ByRef int_Sft_Sch_CsPerHour As Integer,
                    ByRef dec_Sft_Efficiency As Decimal,
                    ByRef bln_Sft_Alert As Boolean,
                    ByRef int_Sft_Progress As Integer) As Integer

        Dim cnnServer As New SqlConnection(gstrServerConnectionString)
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim returnValue As Integer
        Try
            ReDim arParms(33)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Faccility Input Parameter
            arParms(0) = New SqlParameter("@chrFacility", SqlDbType.Char, 3)
            arParms(0).Value = strFacility

            arParms(1) = New SqlParameter("@chrPkgLine", SqlDbType.Char, 10)
            arParms(1).Value = strPkgLine

            arParms(2) = New SqlParameter("@vchStaffID", System.Data.SqlDbType.VarChar, 10)
            arParms(2).Value = strStaffID

            arParms(3) = New SqlParameter("@intShift", System.Data.SqlDbType.Int, 4)
            arParms(3).Value = intShift

            arParms(4) = New SqlParameter("@intShopOrder", System.Data.SqlDbType.Int, 4)
            arParms(4).Value = intShopOrder

            arParms(5) = New SqlParameter("@dteDateTime", System.Data.SqlDbType.DateTime, 8)
            arParms(5).Value = dteDateTime

            arParms(6) = New SqlParameter("@sc_StartTime", System.Data.SqlDbType.DateTime, 8)
            arParms(6).Value = dte_sc_StartTime

            arParms(7) = New SqlParameter("@sc_DefaultPkgLine", System.Data.SqlDbType.[Char], 10)
            arParms(7).Value = str_Sc_DefaultPkgLine

            arParms(8) = New SqlParameter("@sc_OverridePkgLine", System.Data.SqlDbType.[Char], 10)
            arParms(8).Value = str_Sc_OverridePkgLine

            arParms(9) = New SqlParameter("@sc_ShopOrder", System.Data.SqlDbType.Int, 4)
            arParms(9).Value = int_Sc_ShopOrder

            arParms(10) = New SqlParameter("@sc_ItemNumber", System.Data.SqlDbType.VarChar, 35)
            arParms(10).Value = str_Sc_ItemNumber

            arParms(11) = New SqlParameter("@sc_Operator", System.Data.SqlDbType.VarChar, 10)
            arParms(11).Value = str_Sc_Operator

            arParms(12) = New SqlParameter("@sc_DefaultShiftNo", System.Data.SqlDbType.[Char], 1)
            arParms(12).Value = str_Sc_DefaultShiftNo

            arParms(13) = New SqlParameter("@sc_OverrideShiftNo", System.Data.SqlDbType.[Char], 1)
            arParms(13).Value = str_Sc_OverrideShiftNo

            arParms(14) = New SqlParameter("@sc_CasesScheduled", System.Data.SqlDbType.Int, 4)
            arParms(14).Value = int_Sc_CasesScheduled

            arParms(15) = New SqlParameter("@sc_CasesProduced", System.Data.SqlDbType.Int, 4)
            arParms(15).Value = int_Sc_CasesProduced

            arParms(16) = New SqlParameter("@sc_PalletsCreated", System.Data.SqlDbType.Int, 4)
            arParms(16).Value = int_Sc_PalletsCreated

            arParms(17) = New SqlParameter("@sc_LooseCases", System.Data.SqlDbType.Int, 4)
            arParms(17).Value = int_Sc_LooseCases

            arParms(18) = New SqlParameter("@sc_ProductionDate", System.Data.SqlDbType.DateTime, 8)
            arParms(18).Value = dte_Sc_ProductionDate

            arParms(19) = New SqlParameter("@sc_CarriedForwardCases", System.Data.SqlDbType.Int, 4)
            arParms(19).Value = int_Sc_CarriedForwardCases

            arParms(20) = New SqlParameter("@sc_ShiftProductionDate", System.Data.SqlDbType.DateTime, 8)
            arParms(20).Value = dte_Sc_ShiftProductionDate

            arParms(21) = New SqlParameter("@so_Act_Produced", System.Data.SqlDbType.Int, 4)
            arParms(21).Direction = ParameterDirection.Output

            arParms(22) = New SqlParameter("@so_Act_CsPerHour", System.Data.SqlDbType.Int, 4)
            arParms(22).Direction = ParameterDirection.Output

            arParms(23) = New SqlParameter("@so_Sch_CsPerHour", System.Data.SqlDbType.Int, 4)
            arParms(23).Direction = ParameterDirection.Output

            arParms(24) = New SqlParameter("@so_Efficiency", System.Data.SqlDbType.Decimal, 9)
            arParms(24).Direction = ParameterDirection.Output

            arParms(25) = New SqlParameter("@so_Alert", System.Data.SqlDbType.Bit, 1)
            arParms(25).Direction = ParameterDirection.Output

            arParms(26) = New SqlParameter("@so_Progress", System.Data.SqlDbType.Int, 4)
            arParms(26).Direction = ParameterDirection.Output

            arParms(27) = New SqlParameter("@sft_Act_Produced", System.Data.SqlDbType.Int, 4)
            arParms(27).Direction = ParameterDirection.Output

            arParms(28) = New SqlParameter("@sft_Act_CsPerHour", System.Data.SqlDbType.Int, 4)
            arParms(28).Direction = ParameterDirection.Output

            arParms(29) = New SqlParameter("@sft_Sch_Produced", System.Data.SqlDbType.Int, 4)
            arParms(29).Direction = ParameterDirection.Output

            arParms(30) = New SqlParameter("@sft_Sch_CsPerHour", System.Data.SqlDbType.Int, 4)
            arParms(30).Direction = ParameterDirection.Output

            arParms(31) = New SqlParameter("@sft_Efficiency", System.Data.SqlDbType.Decimal, 9)
            arParms(31).Direction = ParameterDirection.Output

            arParms(32) = New SqlParameter("@sft_Alert", System.Data.SqlDbType.Bit, 1)
            arParms(32).Direction = ParameterDirection.Output

            arParms(33) = New SqlParameter("@sft_Progress", System.Data.SqlDbType.Int, 4)
            arParms(33).Direction = ParameterDirection.Output

            Try
                cnnServer.Open()
            Catch ex As SqlException
                SetServerCnnStatusInSessCtl(False)
            End Try
            If cnnServer.State = ConnectionState.Open Then
                strSQLStmt = "PPsp_LineEfficiency"
                returnValue = SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)

                If IsDBNull(arParms(21).Value) Then
                    int_So_Act_Produced = 0
                Else
                    int_So_Act_Produced = CType(arParms(21).Value, Integer)
                End If

                If IsDBNull(arParms(22).Value) Then
                    int_So_Act_CsPerHour = 0
                Else
                    int_So_Act_CsPerHour = CType(arParms(22).Value, Integer)
                End If

                If IsDBNull(arParms(23).Value) Then
                    int_So_Sch_CsPerHour = 0
                Else
                    int_So_Sch_CsPerHour = CType(arParms(23).Value, Integer)
                End If

                If IsDBNull(arParms(24).Value) Then
                    dec_So_Efficiency = 0
                Else
                    dec_So_Efficiency = CType(arParms(24).Value, Single)
                End If

                If IsDBNull(arParms(25).Value) Then
                    bln_So_Alert = False
                Else
                    bln_So_Alert = CType(arParms(25).Value, Boolean)
                End If

                If IsDBNull(arParms(26).Value) Then
                    int_So_Progress = 0
                Else
                    int_So_Progress = CType(arParms(26).Value, Integer)
                End If

                If IsDBNull(arParms(27).Value) Then
                    int_Sft_Act_Produced = 0
                Else
                    int_Sft_Act_Produced = CType(arParms(27).Value, Integer)
                End If

                If IsDBNull(arParms(28).Value) Then
                    int_Sft_Act_CsPerHour = 0
                Else
                    int_Sft_Act_CsPerHour = CType(arParms(28).Value, Integer)
                End If

                If IsDBNull(arParms(29).Value) Then
                    int_Sft_Sch_Produced = 0
                Else
                    int_Sft_Sch_Produced = CType(arParms(29).Value, Integer)
                End If

                If IsDBNull(arParms(30).Value) Then
                    int_Sft_Sch_CsPerHour = 0
                Else
                    int_Sft_Sch_CsPerHour = CType(arParms(30).Value, Integer)
                End If

                If IsDBNull(arParms(31).Value) Then
                    dec_Sft_Efficiency = 0
                Else
                    dec_Sft_Efficiency = CType(arParms(31).Value, Single)
                End If

                If IsDBNull(arParms(32).Value) Then
                    bln_Sft_Alert = False
                Else
                    bln_Sft_Alert = CType(arParms(32).Value, Boolean)
                End If

                If IsDBNull(arParms(33).Value) Then
                    int_Sft_Progress = 0
                Else
                    int_Sft_Progress = CType(arParms(33).Value, Integer)
                End If

            End If
            Return returnValue
        Catch ex As Exception
            Throw ex
        Finally
            cnnServer.Close()
        End Try
    End Function
    Public Shared Function GetEquipmentDescription(ByVal strFacility As String, ByVal strEquipmentID As String) As String
        Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable
        Try
            GetEquipmentDescription = String.Empty
            gtaEQ.Fill(tblEQ, strFacility, strEquipmentID, "P", "", Nothing)
            If tblEQ.Rows.Count <> 0 Then
                GetEquipmentDescription = (tblEQ.Rows(0).Item("Description"))
            End If
        Catch ex As Exception
            Throw New Exception("Error in GetEquipmentDescription" & vbCrLf & ex.Message)
        End Try
    End Function
    Public Shared Function GetEquipmentInfo(ByVal strFacility As String, ByVal strEquipmentID As String) As dsEquipment.CPPsp_EquipmentIORow
        Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable
        Try
            gtaEQ.Fill(tblEQ, strFacility, strEquipmentID, "P", "", Nothing)
            If tblEQ.Rows.Count <> 0 Then
                Return (tblEQ.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As SqlException When ex.ErrorCode = -2146232060
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in GetEquipmentInfo" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function PoPUpMSG(ByVal strMsg As String, ByVal strCaption As String, ByVal buttons As System.Windows.Forms.MessageBoxButtons) As DialogResult
        Try
            Using frm As New frmMsgBox
                frm.Text = strCaption
                frm.txtMessage.Text = strMsg
                frm.Tag = buttons
                Return frm.ShowDialog()
            End Using

        Catch ex As Exception
            Throw New Exception("Error in PoPUpMSG" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetSessionControlHst(ByVal strAction As String, ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal strShift As String, ByVal dteProductonDate As DateTime,
            ByVal dteCurrentTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As dsSessionControlHst.CPPsp_SessionControlHstIODataTable

        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Try
            If gblnSvrConnIsUp = True Then         'ALM#11828
                Using taSCH As New dsSessionControlHstTableAdapters.CPPsp_SessionControlHstIOTableAdapter
                    'WO#755 gtaSCH.Fill(tblSCH, strAction, strPkgLine, intShopOrder, strShift, dteProductonDate, dteCurrentTime, strFacility, strOperator)
                    taSCH.Fill(tblSCH, strAction, strPkgLine, intShopOrder, strShift, dteProductonDate, dteCurrentTime, strFacility, strOperator, 0)    'WO#755
                End Using
            End If                                  'ALM#11828
            'WO#17432   Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001)
        Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054 Or ex.Number = 11001)       'WO#17432 
            'If gblnSvrConnIsUp = True Then
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            'Else
            '    Throw New Exception("Error in GetSectionControlHst" & vbCrLf & ex.Message)
            'End If
        Catch ex As Exception
            Throw New Exception("Error in GetSectionControlHst" & vbCrLf & ex.Message)
        Finally
            GetSessionControlHst = tblSCH
        End Try
    End Function

    'FX20160811 Public Shared Function GetCarriedForwardCases(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShift As Integer, ByVal dteShiftProductionDate As DateTime, ByVal dteGivenTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As Short
    Public Shared Function GetCarriedForwardCases(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShift As Integer, ByVal dteShiftProductionDate As DateTime, ByVal dteGivenTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As Integer           'FX20160811
        'Carried Forward Cases recorded when the shop order was started for shift change or operator change
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Try
            tblSCH = SharedFunctions.GetSessionControlHst("CarriedForwardCases_Opr_SO_Shift", strPkgLine, intShopOrder, intShift,
                    dteShiftProductionDate, dteGivenTime, strFacility, strOperator)
            If tblSCH.Rows.Count > 0 Then
                GetCarriedForwardCases = tblSCH.Rows(0).Item("LooseCases")
                If GetCarriedForwardCases = 0 Then
                    GetCarriedForwardCases = tblSCH.Rows(0).Item("CarriedForwardCases")
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in GetCarriedForwardCases" & vbCrLf & ex.Message)
        End Try

    End Function

    'FX20160811 Public Shared Function CarriedForwardCasesFromShift(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShift As Integer, ByVal dteShiftProductionDate As DateTime, ByVal dteGivenTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As Short
    Public Shared Function CarriedForwardCasesFromShift(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShift As Integer, ByVal dteShiftProductionDate As DateTime, ByVal dteGivenTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As Integer        'FX20160811
        'Carried Forward Cases recorded when the shop order was started at the first time in the shift
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Try
            tblSCH = SharedFunctions.GetSessionControlHst("CarriedForwardCases_SO_Shift", strPkgLine, intShopOrder, intShift,
                    dteShiftProductionDate, dteGivenTime, strFacility, strOperator)
            If tblSCH.Rows.Count > 0 Then
                CarriedForwardCasesFromShift = tblSCH.Rows(0).Item("CarriedForwardCases")
            Else
                CarriedForwardCasesFromShift = gdrSessCtl.CarriedForwardCases
            End If
        Catch ex As Exception
            Throw New Exception("Error in CarriedForwardCasesFromShift" & vbCrLf & ex.Message)
        End Try

    End Function
    'FX20160811 Public Shared Function GetLooseCases(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShift As Integer, ByVal dteGivenTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As Short
    Public Shared Function GetLooseCases(ByVal strPkgLine As String, ByVal intShopOrder As Integer, ByVal intShift As Integer, ByVal dteGivenTime As DateTime, ByVal strFacility As String, ByVal strOperator As String) As Integer         'FX20160811
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Try
            tblSCH = SharedFunctions.GetSessionControlHst("SelectLastRecByLineSO", strPkgLine, intShopOrder, intShift,
                    Now, dteGivenTime, strFacility, strOperator)

            If tblSCH.Rows.Count > 0 Then
                GetLooseCases = tblSCH.Rows(0).Item("LooseCases")
            End If
        Catch ex As Exception
            Throw New Exception("Error in GetLooseCases" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Function GetSOCasesProducedFromPallet(ByVal intShopOrder As String, Optional ByVal intShift As Integer = 0, Optional ByVal strTimeInShift As String = "", Optional ByVal strOperator As String = "") As Integer
        '----------------------------------------------------------------------------------------
        'If  shift is specified, it will get the cases produced of the shop order in that shift.
        '----------------------------------------------------------------------------------------
        Dim arParms() As SqlParameter = New SqlParameter(7) {}
        Dim strSQLStmt As String
        Dim tblSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
        Dim dteTimeinShift As DateTime

        Try
            If strTimeInShift = "" Then
                dteTimeinShift = Now
            Else
                dteTimeinShift = CType(strTimeInShift, DateTime)
            End If

            'Action Parameter
            arParms(0) = New SqlParameter("@vchAction", SqlDbType.VarChar)
            arParms(0).Value = String.Empty

            'Facility Input Parameter
            arParms(1) = New SqlParameter("@chrFacility", SqlDbType.Char)
            arParms(1).Value = gdrSessCtl.Facility

            'Packaging Line Input Parameter
            arParms(2) = New SqlParameter("@chrPkgLine ", SqlDbType.Char)
            arParms(2).Value = gdrSessCtl.OverridePkgLine

            'Shop Order Input Parameter
            arParms(3) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(3).Value = intShopOrder

            arParms(4) = New SqlParameter("@intShiftNo", SqlDbType.Int)
            arParms(4).Value = intShift

            arParms(5) = New SqlParameter("@dteGivenTime", SqlDbType.DateTime)
            arParms(5).Value = dteTimeinShift

            arParms(6) = New SqlParameter("@dteShiftProductionDate", SqlDbType.DateTime)
            arParms(6).Value = gdrSessCtl.ShiftProductionDate

            arParms(7) = New SqlParameter("@intCasesProduced", SqlDbType.Int)
            arParms(7).Direction = ParameterDirection.Output

            strSQLStmt = "CPPsp_CasesProducedFromPallet"
            If gblnSvrConnIsUp = True Then

                'If intShift <> 0 Then 'Cases produced for the shop order in the given shift no.
                '    strSQLStmt = "Select (Select Isnull(Sum(quantity),0) from tblPallethst Where shoporder = @intShopOrder and (CreationDateTime Between @dteShiftStart and @dteShiftEnd " & _
                '        "OR StartTime Between @dteShiftStart and @dteShiftEnd)) " & _
                '        " + (Select Isnull(Sum(quantity),0) from tblPallet Where shoporder = @intShopOrder and (CreationDateTime Between @dteShiftStart and @dteShiftEnd " & _
                '        "OR StartTime Between @dteShiftStart and @dteShiftEnd)) "
                'Else    'Cases produced for the whole shop order
                '    strSQLStmt = "Select (Select Isnull(Sum(quantity),0) from tblPallethst Where shoporder = @intShopOrder)" & _
                '        " + (Select Isnull(Sum(quantity),0) from tblPallet Where shoporder = @intShopOrder)"
                'End If
                Try
                    SqlHelper.ExecuteScalar(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    Return 0
                End Try
            Else    'when network failure, calculate from the pallet file in the local database
                'If intShift <> 0 Then
                '    strSQLStmt = "Select Isnull(sum(quantity),0) from tblPallet where shoporder = @intShopOrder and (CreationDateTime Between dteShiftStart and dteShiftEnd)" & _
                '                "OR StartTime Between @dteShiftStart and @dteShiftEnd)) "
                'Else
                '    strSQLStmt = "Select Isnull(sum(quantity),0) from tblPallet where shoporder = @intShopOrder"
                'End If

                SqlHelper.ExecuteScalar(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
            GetSOCasesProducedFromPallet = arParms(UBound(arParms)).Value
        Catch ex As Exception
            Throw New Exception("Error in GetSOCasesProducedFromPallet" & vbCrLf & ex.Message)
        End Try

    End Function
    Public Shared Function HasConnectivity() As Boolean
        Dim hostInfo As System.Net.IPHostEntry
        Try 'to look for the logon server
            Dim sServer As String = Environment.GetEnvironmentVariable("logonserver")
            hostInfo = System.Net.Dns.GetHostEntry(sServer.Remove(0, 2))
            Return True
        Catch 'theres no network connection
            Return False
        End Try
    End Function

    Public Shared Sub ShowTSCtlPnl()
        'Show Touch Screen Control Panel
        Dim pInstance As Process = Nothing
        Dim handle As IntPtr
        Dim strTouchScreenPgmName As String
        Dim blnUSBConnection As Boolean
        Dim comPort As Ports.SerialPort

        Try
            'Find out the IPC is connected to the touch screen by USB or Serial Port.
            blnUSBConnection = True
            Try
                'If the com. port can be allocated, that means the touch screen is connected by USB. 
                comPort = My.Computer.Ports.OpenSerialPort("COM1")
                If comPort.IsOpen = True Then  'WO#755
                    'Release the allocated Serial port.
                    comPort.Close()
                End If                          'WO#755
            Catch ex As Exception
                'When OpenSerialPort has exception error - Access to the port "COM1" is denied. i.e. Com 1 is being used.
                blnUSBConnection = False
            End Try

            If blnUSBConnection = False Then
                strTouchScreenPgmName = My.Settings.gstrSerialTouchScreenPgm
                strTouchScreenPgmName = strTouchScreenPgmName.Substring(strTouchScreenPgmName.LastIndexOf("\") + 1, strTouchScreenPgmName.LastIndexOf(".") - strTouchScreenPgmName.LastIndexOf("\") - 1)
                'if the Touch Screen control panel has already been called, use the same instance.
                pInstance = SharedFunctions.GetRunningInstance(strTouchScreenPgmName)
                If pInstance Is Nothing Then
                    Process.Start(My.Settings.gstrSerialTouchScreenPgm)
                Else
                    handle = pInstance.MainWindowHandle
                    If Not IntPtr.Zero.Equals(handle) Then
                        Win32Helper.ShowWindow(handle, 1)
                        Win32Helper.SetForegroundWindow(handle)
                    End If
                End If
            Else    'Find out which USB touch screen calibration program
                'WO#755 Process.Start(My.Settings.gstrUSBTouchScreenPgm)
                Process.Start(SharedFunctions.GetTouchScreenCalibrationPgm())   'WO#755
            End If

        Catch ex As Exception
            Throw New Exception("Error in ShowTSCtlPnl" & vbCrLf & ex.Message, ex)
        End Try

    End Sub

    Public Shared Function GetSessionDefaultPkgLine(ByVal strFacility As String, ByVal strOriginalDftLine As String, ByVal strOverrideLine As String) As String

        ' If the overridden packaging line is it's associated line, 
        ' change the default line with the overridden packaging line id

        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim blnIsSharedIPCLine As Boolean
        Dim strDefaultPkgLine As String

        Try
            strDefaultPkgLine = strOriginalDftLine
            If RTrim(strDefaultPkgLine) <> RTrim(strOverrideLine) Then

                ReDim arParms(4)
                arParms = New SqlParameter(UBound(arParms)) {}

                ' Faccility Input Parameter
                arParms(0) = New SqlParameter("@vchFacility", SqlDbType.VarChar, 3)
                arParms(0).Value = strFacility

                ' Original Default Line Input Parameter
                arParms(1) = New SqlParameter("@vchDefaultPkgLine", SqlDbType.VarChar)
                arParms(1).Value = strOriginalDftLine

                ' Machine ID Input Parameter
                arParms(2) = New SqlParameter("@vchOverridePkgLine", SqlDbType.VarChar, 10)
                arParms(2).Value = strOverrideLine

                ' Production Date Output Parameter
                arParms(3) = New SqlParameter("@bitResult", SqlDbType.Bit)
                arParms(3).Direction = ParameterDirection.Output

                strSQLStmt = "CPPsp_IsSharedIPCLine"
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                blnIsSharedIPCLine = arParms(3).Value

                If blnIsSharedIPCLine = True Then
                    strDefaultPkgLine = strOverrideLine
                End If
            End If
            Return strDefaultPkgLine
        Catch ex As Exception
            Throw New Exception("Error in GetSessionDefaultPkgLine" & vbCrLf & ex.Message, ex)
        End Try
    End Function
    ' WO #297 Add Begin
    Public Shared Function CloseShopOrder(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strDefaultPkgLine As String, ByVal strOperator As String,
        ByVal dteStartTime As DateTime, ByVal dteClosingTime As DateTime, ByVal dteLastUpdated As DateTime, ByVal dteCreationTime As DateTime,
        ByVal enuCallingFrom As CallingFrom, Optional ByVal trnServer As SqlTransaction = Nothing) As Int16

        Dim strSQLStmt As String
        Dim arParms() As SqlParameter
        Dim intRtnCde As Int16
        Try
            'If the shop order is closed from a line, it needs to do from the Close Shop Order screen.
            'If the shop order is closed from a pallet station, it is though the create pallet

            'Create a "close shop order history" record to close the shop order in BPCS 
            If enuCallingFrom = CallingFrom.StopShopOrder Or CallingFrom.UploadToServer Or gdrCmpCfg.PalletStation = True Then
                'Close Shop Order -  wirte a record in tblClosedShopOrderHst
                strSQLStmt = "CPPsp_ToBeClosedShopOrder_Maint"

                ReDim arParms(8)
                arParms = New SqlParameter(UBound(arParms)) {}

                ' Action Input Parameter
                arParms(0) = New SqlParameter("@vchAction", SqlDbType.VarChar, 50)
                arParms(0).Value = "ADD"

                ' Shop Order Facility Input Parameter
                arParms(1) = New SqlParameter("@Facility", SqlDbType.VarChar, 3)
                arParms(1).Value = strFacility

                ' Shop Order Number Input Parameter
                arParms(2) = New SqlParameter("@ShopOrder", SqlDbType.Int)
                arParms(2).Value = intShopOrder

                ' Default Packaging Line Input Parameter
                arParms(3) = New SqlParameter("@DefaultPkgLine", SqlDbType.VarChar, 10)
                arParms(3).Value = strDefaultPkgLine

                ' Operator Input Parameter
                arParms(4) = New SqlParameter("@Operator", SqlDbType.VarChar, 10)
                arParms(4).Value = strOperator

                ' Shop Order Session Start Time Input Parameter
                arParms(5) = New SqlParameter("@SessionStartTime", SqlDbType.DateTime)
                arParms(5).Value = dteStartTime

                ' Current local system time Input Parameter
                arParms(6) = New SqlParameter("@ClosingTime", SqlDbType.DateTime)
                arParms(6).Value = dteClosingTime

                ' Current local system time Input Parameter
                arParms(7) = New SqlParameter("@LastUpdated", SqlDbType.DateTime)
                arParms(7).Value = dteLastUpdated

                ' Current local system time Input Parameter
                arParms(8) = New SqlParameter("@CreationTime", SqlDbType.DateTime)
                arParms(8).Value = dteCreationTime

                If gblnSvrConnIsUp = True Then
                    Try
                        If trnServer Is Nothing Then
                            intRtnCde = SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                        Else
                            intRtnCde = SqlHelper.ExecuteNonQuery(trnServer, CommandType.StoredProcedure, strSQLStmt, arParms)
                        End If
                        'WO#17432   Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001)
                    Catch ex As SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060 And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                    Or ex.Number = 1231 Or ex.Number = 10054 Or ex.Number = 11001)       'WO#17432 
                        SharedFunctions.SetServerCnnStatusInSessCtl(False)
                        MessageBox.Show(gcstSvrCnnFailure)
                        intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                    Finally
                        CloseShopOrder = intRtnCde
                    End Try
                Else
                    intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End If
            End If

            'Update Shop order table to indicate it is closed, so that it will be open accidentally when importing shop orders from staging area.
            ChangeSOStatus(intShopOrder, ShopOrderStatus.Closed, trnServer)

        Catch ex As Exception
            Throw New Exception("Error in CloseShopOrder" & vbCrLf & ex.Message)
        End Try
    End Function
    'WO#5370 ADD Start
    'WO#15635   Public Shared Sub AutoCreatePallet(drSessCtl As dsSessionControl.CPPsp_SessionControlIORow, drCmpCfg As dsComputerConfig.CPPsp_ComputerConfigIORow, _
    'WO#15635                   intQtyInPallet As Integer, strOutputLocation As String, intDestinationShopOrder As Integer, intTxID As Integer)  
    Public Overloads Shared Sub AutoCreatePallet(drSessCtl As dsSessionControl.CPPsp_SessionControlIORow, drCmpCfg As dsComputerConfig.CPPsp_ComputerConfigIORow,
                        intQtyInPallet As Integer, strOutputLocation As String, intDestinationShopOrder As Integer, intTxID As Integer)                  'WO#15635
        Dim dteProductionDate As DateTime
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim dr As dsShopOrder.CPPsp_ShopOrderIORow
        Dim strErrMsg As String = ""

        Try
            gtaSO.Fill(tblSO, "GetSO&Item", drSessCtl.Facility, drSessCtl.ShopOrder, "")
            If tblSO.Rows.Count > 0 Then
                dr = tblSO.Rows(0)
                If intQtyInPallet <= dr.QtyPerPallet Then
                    strErrMsg = SharedFunctions.IsItemChangedOnServer(dr)
                    If strErrMsg = "" Then
                        dteProductionDate = Now()
                        With drSessCtl
                            SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), .ShopOrder, .ItemNumber, .Item("DefaultPkgLine"),
                                                                  .Operator, intQtyInPallet, .Item("StartTime"), "N", dr.QtyPerPallet,
                                                                   dr.LotID, dteProductionDate, .OverrideShiftNo, strOutputLocation,
                                                                   drCmpCfg("PalletStation"), intDestinationShopOrder, intTxID, "autocreatepallet - Indusoft")
                            If drCmpCfg.ShowEfficiency = True And drSessCtl.ServerCnnIsOk = True Then
                                frmProcessMonitor.ShowDialog()
                            End If
                        End With
                    Else
                        MessageBox.Show(strErrMsg, "Data Integrity Error")
                    End If
                Else
                    MessageBox.Show("Quantity in pallet " & CType(intQtyInPallet, String) & " is greater then quantity per pallet. Contact supervisor.", "Error - Pallet can not be created", MessageBoxButtons.OK)
                End If
            Else
                MessageBox.Show("Shop order is not found, can not create pallet for the shop order, " + drSessCtl("ShopOrder").ToString + ".", "Invalid Shop Order")
            End If


            'WO#17432   Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True And (ex.Number = 64 Or ex.Number = 1231)
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                   Or ex.Number = 1231)     'WO#17432 
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            MessageBox.Show(gcstSvrCnnFailure)
        Catch ex As Exception
            Throw New Exception("Error in AutoCreatepallet by auto count by pallet" & vbCrLf & ex.Message)
        End Try
    End Sub
    'WO#5370 ADD Stop
    'WO#15635   Public Shared Sub AutoCreatePallet()    
    Public Overloads Shared Sub AutoCreatePallet()    'WO#15635 
        Dim dteProductionDate As DateTime
        Dim tblso As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim dr As dsShopOrder.CPPsp_ShopOrderIORow
        Dim strErrMsg As String = "" 'WO#359

        Try
            gtaSO.Fill(tblso, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), "")
            If tblso.Rows.Count > 0 Then
                dr = tblso.Rows(0)
                'WO#359 ADD Start

                strErrMsg = SharedFunctions.IsItemChangedOnServer(dr)
                If strErrMsg = "" Then
                    'WO#359 ADD Stop
                    dteProductionDate = Now()
                    With gdrSessCtl
                        'WO#2563 DEL Start
                        'SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), .ShopOrder, .ItemNumber, .Item("DefaultPkgLine"), _
                        '                                      .Operator, dr.QtyPerPallet, .Item("StartTime"), "N", dr.QtyPerPallet, _
                        '                                       dr.LotID, dteProductionDate, .OverrideShiftNo, gdrCmpCfg("PalletStation"))
                        'WO#2563 DEL Stop
                        'ALM#11828 DEL Start
                        'SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), .ShopOrder, .ItemNumber, .Item("DefaultPkgLine"), _
                        '                                      .Operator, dr.QtyPerPallet, .Item("StartTime"), "N", dr.QtyPerPallet, _
                        '                                       dr.LotID, dteProductionDate, .OverrideShiftNo, Nothing, _
                        '                                       gdrCmpCfg("PalletStation"))      'WO#2563
                        'ALM#11828 DEL Stop
                        SharedFunctions.ProcessFrmCreatePallet(.Item("Facility"), .ShopOrder, .ItemNumber, .Item("DefaultPkgLine"),
                                      .Operator, dr.QtyPerPallet, .Item("StartTime"), "N", dr.QtyPerPallet,
                                       dr.LotID, dteProductionDate, .OverrideShiftNo, Nothing,
                                       gdrCmpCfg("PalletStation"), 0, 0, "autocreatepallet - Indusoft")                        'WO#5370
                        'WO#5370       gdrCmpCfg("PalletStation"), 0)      'ALM#11828
                        If gdrCmpCfg.ShowEfficiency = True And gdrSessCtl.ServerCnnIsOk = True Then
                            frmProcessMonitor.ShowDialog()
                        End If
                    End With
                Else    'WO#359 
                    MessageBox.Show(strErrMsg, "Data Integrity Error") 'WO#359 
                End If  'WO#359 
            Else
                MessageBox.Show("Shop order is not found, can not create pallet for the shop order, " + gdrSessCtl("ShopOrder").ToString + ".", "Invalid Shop Order")
            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True And (ex.Number = 64 Or ex.Number = 1231)
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            MessageBox.Show(gcstSvrCnnFailure)
        Catch ex As Exception
            Throw New Exception("Error in AutoCreatepallet" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub WriteLog(ByVal strFileName As String, ByVal strMsg As String)
        Try
            'Console.Write(DateTime.Now & " - ")
            'Console.WriteLine(strMsg)
            If My.Computer.FileSystem.FileExists(strFileName) = False Then
                My.Computer.FileSystem.WriteAllText(strFileName, String.Empty, False)
            End If
            My.Computer.FileSystem.WriteAllText(strFileName, DateTime.Now & " - " & strMsg & vbCrLf, True)
        Catch ex As Exception
            Throw New Exception("- <Error> in WriteLog " & ex.Message)
        End Try
    End Sub
    ' WO #297 Add End
    ' WO #359 Add Start
    Public Shared Function IsLineActive(ByVal strComputerName As String, ByVal strLineID As String) As Boolean
        Dim dtCF As New dsComputerConfig.CPPsp_ComputerConfigIODataTable
        Dim daCF As New dsComputerConfigTableAdapters.CPPsp_ComputerConfigIOTableAdapter

        Try
            'WO#755 daCF.FillByIPCName_LineID(dtCF, "SelectAllFields", strComputerName, strLineID)
            daCF.FillByIPCName_LineID(dtCF, "AllActiveInclVirtual", strComputerName, strLineID)  'WO#755
            If dtCF.Rows.Count <> 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As SqlException When ex.ErrorCode = -2146232060
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in IsLineActive" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetLastDownLoadTime() As DateTime
        Dim taLDLT As New dsDownLoadTableListTableAdapters.CPPsp_DownLoadTableList_SelTableAdapter
        Dim dtLDLT As New dsDownLoadTableList.CPPsp_DownLoadTableList_SelDataTable
        Try
            Return taLDLT.getLastDownLoadTime("LastDownLoadTime", gdrSessCtl.Facility, Nothing)

        Catch ex As SqlException When ex.ErrorCode = -2146232060
            Throw ex
        Catch ex As Exception
            Throw New Exception("Error in GetLastDownLoadTime" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Function IsDataReadyForRefresh() As Boolean
        Dim blnIsReady As Boolean = False
        Dim ta As New dsIsDataReadyForRefreshTableAdapters.QueriesTableAdapter
        ta.qryIsDataReadyForRefresh(gdrCmpCfg.ComputerName, blnIsReady)
        Return blnIsReady
    End Function
    Public Shared Function IsValidDate(ByVal strYear As String, ByVal strMonth As String, ByVal strDay As String) As Boolean
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
    Public Shared Function IsProcessActive(ByVal strProcessName As String) As Boolean
        Dim processList() As Process = Nothing
        Dim blnIsActive As Boolean = False
        Try
            For i As Integer = 1 To 2
                processList = Process.GetProcessesByName(strProcessName)
                If processList.Length = 0 Then
                    System.Threading.Thread.Sleep(1000)
                Else
                    blnIsActive = True
                    Exit For
                End If
            Next
            Return blnIsActive
        Catch ex As Exception
            Throw New Exception("Error in IsProcessActive" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Sub StartDownTimePgm(strPkgLine As String, intShopOrder As Integer, strOperator As String, intShift As Integer, strFacility As String, strPkgLineType As String)
        Dim strCmdArgs As String = Nothing
        Dim strSessionStartTime As String = Nothing
        Dim sb As New System.Text.StringBuilder

        Try
            sb.AppendFormat("{0} {1} {2} {3} {4} {5} {6}", "P", strPkgLine, intShopOrder.ToString, strOperator, intShift.ToString, strFacility, strPkgLineType)

            If gdrSessCtl.ShopOrder <> 0 Then
                sb.Append(" ")
                sb.Append(gdrSessCtl.StartTime.ToString("yyyy-MM-ddHH:mm:ss.fff"))
            End If

            strCmdArgs = sb.ToString

            Process.Start(My.Settings.strDownTime, strCmdArgs)

        Catch ex As Exception
            Throw New Exception("Error in StartDownTimePgm" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Sub WakeUpAPgm(strProcessName As String)
        Dim pInstance As Process = Nothing
        Dim handle As IntPtr
        Try
            For i As Int16 = 1 To 3
                pInstance = SharedFunctions.GetRunningInstance(strProcessName)
                If IsNothing(pInstance) Then
                    System.Threading.Thread.Sleep(1000)
                Else
                    Exit For
                End If
            Next

            If Not IsNothing(pInstance) Then
                handle = pInstance.MainWindowHandle
                If Not IntPtr.Zero.Equals(handle) Then
                    Win32Helper.ShowWindow(handle, ProcessWindowStyle.Maximized)
                    Win32Helper.SetForegroundWindow(handle)
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in WakeUpAPgm" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Function IsItemChangedOnServer(Optional ByVal drLocalSO As dsShopOrder.CPPsp_ShopOrderIORow = Nothing, Optional ByVal drLocalItem As dsItemMaster.CPPsp_ItemMasterIORow = Nothing) As String
        Dim taServerItem As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
        Dim dtServerItem As New dsItemMaster.CPPsp_ItemMasterIODataTable
        Dim drServerItem As dsItemMaster.CPPsp_ItemMasterIORow
        Dim strItemNo As String
        Dim strErrMsg As String = Nothing
        Dim strValue As String = ""
        Dim intQtyPerPallet As Integer
        Dim sglTie As Single
        Dim sglTier As Single
        Dim intProductionShelfLifeDays As Integer
        Dim intShipShelfLifeDays As Integer

        Try
            If gblnSvrConnIsUp Then
                If drLocalItem Is Nothing Then
                    With drLocalSO
                        strItemNo = .ItemNumber
                        intQtyPerPallet = .QtyPerPallet
                        sglTie = .Tie
                        sglTier = .Tier
                        intProductionShelfLifeDays = .ProductionShelfLifeDays
                        intShipShelfLifeDays = .ShipShelfLifeDays
                    End With
                Else
                    With drLocalItem
                        strItemNo = .ItemNumber
                        intQtyPerPallet = .QtyPerPallet
                        sglTie = .Tie
                        sglTier = .Tier
                        intProductionShelfLifeDays = .ProductionShelfLifeDays
                        intShipShelfLifeDays = .ShipShelfLifeDays
                    End With
                End If

                taServerItem.Connection.ConnectionString = My.Settings.ServerPowerPlantCnnStr
                taServerItem.Fill(dtServerItem, gdrSessCtl.Facility, strItemNo, "AllByItemNo")
                If dtServerItem.Rows.Count > 0 Then
                    drServerItem = dtServerItem.Rows(0)
                    If intQtyPerPallet <> drServerItem.QtyPerPallet Then
                        strErrMsg = "Full pallet Quantity"
                        strValue = drServerItem.QtyPerPallet.ToString
                    ElseIf sglTie <> drServerItem.Tie Then
                        strErrMsg = "Tie"
                        strValue = drServerItem.Tie.ToString
                    ElseIf sglTier <> drServerItem.Tier Then
                        strErrMsg = "Tier"
                        strValue = drServerItem.Tier.ToString
                    ElseIf intProductionShelfLifeDays <> drServerItem.ProductionShelfLifeDays Then
                        strErrMsg = "Production Shelf Life Days"
                        strValue = drServerItem.ProductionShelfLifeDays.ToString
                    ElseIf intShipShelfLifeDays <> drServerItem.ShipShelfLifeDays Then
                        strErrMsg = "Ship Shelf Life Days"
                        strValue = drServerItem.ShipShelfLifeDays.ToString
                    End If
                End If

                'WO#871 If strErrMsg <> "" Then
                If Not IsNothing(strErrMsg) Then     'WO#871
                    If gdrCmpCfg.PalletStation = False Then
                        strErrMsg = strErrMsg & " has been changed to " & strValue & " on Server after shop order was started. Pallet can not be created, please contact supervisor."
                    Else
                        strErrMsg = strErrMsg & " has been changed to " & strValue & " on Server after logged in. Pallet can not be created, please contact supervisor."
                    End If
                End If
            End If
            Return strErrMsg
            'WO#871 ADD Start
            'WO#17432  Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
        Catch ex As SqlClient.SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                    Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True         'WO#17432
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            Return strErrMsg
            'WO#871 ADD Stop
        Catch ex As Exception
            Throw New Exception("Error in IsItemChangedOnServer" & vbCrLf & ex.Message)
        End Try

    End Function
    'WO#359 Add Stop
    'WO#650 Add Start
    Public Shared Function IsValidProductionDate(ByVal strYear As String, ByVal strMonth As String, ByVal strDay As String, Optional ByVal intProductionShelfLifeDays As Int16 = 0,
                                   Optional ByVal intShipShelfLifeDays As Int16 = 0, Optional ByVal strItemNo As String = "") As String
        Dim strProductionDate As String
        Dim sb As New StringBuilder
        Dim edteItem As New clsExpiryDate

        Try
            If strItemNo <> "" Then
                Dim daItem As New dsItemMasterTableAdapters.CPPsp_ItemMasterIOTableAdapter
                Dim taItem As New dsItemMaster.CPPsp_ItemMasterIODataTable
                daItem.Fill(taItem, gdrSessCtl.Facility, strItemNo, "AllByItemNo")
                If taItem.Rows.Count > 0 Then
                    edteItem.ProductionShelfLifeDays = taItem.Rows(0).Item("ProductionShelfLifeDays")
                    edteItem.ShipShelfLifeDays = taItem.Rows(0).Item("ShipShelfLifeDays")
                End If
            Else
                edteItem.ProductionShelfLifeDays = intProductionShelfLifeDays
                edteItem.ShipShelfLifeDays = intShipShelfLifeDays
            End If

            IsValidProductionDate = ""
            If strYear <> "" AndAlso strMonth <> "" AndAlso strDay <> "" Then
                sb.Append(strYear)
                sb.Append("/")
                sb.Append(strMonth)
                sb.Append("/")
                sb.Append(strDay)
                strProductionDate = sb.ToString
                If Not IsDate(strProductionDate) Then
                    IsValidProductionDate = "Please enter a valid date for Production Date."
                Else
                    If edteItem.IsProductionDateValid(CDate(strProductionDate)) = False Then
                        IsValidProductionDate = "Production date must be between " & edteItem.EarilestProductionDate.ToString("MM/dd/yyyy") &
                                " and " & edteItem.LatestProductionDate.ToString("MM/dd/yyyy")
                    End If
                End If
            Else
                IsValidProductionDate = "Please enter a valid date for Production Date."
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsValidProductionDate" & vbCrLf & ex.Message)
        End Try
    End Function
    'WO#650 Add Stop
    'WO#654 Add Start
    Public Shared Sub SaveStartSOOption(ByVal blnStartWithNoLabel As Boolean)
        'Dim xmldWorkFile As New XmlDataDocument
        'Dim strFileName As String

        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim cnnLocal As New SqlConnection(gstrLocalDBConnectionString)

        Try
            'strFileName = My.Application.Info.DirectoryPath & "/" & My.Settings.gstrWorkFileName
            'With xmldWorkFile
            '    .Load(strFileName)
            '    If blnStartWithNoLabel Then
            '        SaveElementValue(xmldWorkFile, "IsSOStartedWithNoLabel", "YES")
            '    Else
            '        SaveElementValue(xmldWorkFile, "IsSOStartedWithNoLabel", "NO")
            '    End If
            '    .Save(strFileName)
            'End With


            cnnLocal.Open()

            'Update current IPC control table
            ReDim arParms(2)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Default Packaging Line Input Parameter
            arParms(0) = New SqlParameter("@vchValue1", SqlDbType.VarChar)
            If blnStartWithNoLabel Then
                arParms(0).Value = "YES"
            Else
                arParms(0).Value = "NO"
            End If

            ' Default Packaging Line Input Parameter
            arParms(1) = New SqlParameter("vchControlKey", SqlDbType.VarChar)

            arParms(1).Value = "IsSOStartedWithNoLabel"


            strSQLStmt = "UPDATE tblIPCControl " &
                         "SET Value1 = @vchValue1 WHERE ControlKey = @vchControlKey"


            SqlHelper.ExecuteNonQuery(cnnLocal, CommandType.Text, strSQLStmt, arParms)

        Catch ex As Exception
            Throw New Exception("Error in  SaveStartSOOption" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(cnnLocal) Then
                cnnLocal.Close()
            End If
        End Try
    End Sub

    Public Shared Function IsSOStartedWithNoLabel() As Boolean
        'Dim xmldoc As New XmlDataDocument
        ' Dim fs As FileStream = Nothing
        'WO#17432   Dim strElementValue As String = "NO"
        Dim strElementValue As String           'WO#17432
        Dim strIPCCtrlValues As String()        'WO#17432
        'WO#17432 Dim daIPCControl As New dsIPCControlTableAdapters.tblIPCControlTableAdapter
        'WO#17432   Dim dtIPCControl As New dsIPCControl.tblIPCControlDataTable

        Try
            If gbln2SOIn1Line Then
                strIPCCtrlValues = SharedFunctions.GetIPCControl("IsSOStartedWithNoLabel")      'WO#17432
                strElementValue = strIPCCtrlValues(0)                                           'WO#17432
                'WO#17432 DEL Start
                'daIPCControl.Fill(dtIPCControl, "IsSOStartedWithNoLabel")
                'If dtIPCControl.Rows.Count > 0 Then
                '    strElementValue = dtIPCControl.Rows(0).Item("Value1")
                'End If
                'WO#17432 DEL Stop
                'fs = New FileStream(My.Application.Info.DirectoryPath & "\" & My.Settings.gstrWorkFileName, FileMode.Open, FileAccess.Read)
                'xmldoc.Load(fs)
                'strElementValue = GetElementValue(xmldoc, "IsSOStartedWithNoLabel")
                If UCase(strElementValue) = "YES" Then
                    gblnStartSOWithNoLabel = True
                Else
                    gblnStartSOWithNoLabel = False
                End If
            Else
                gblnStartSOWithNoLabel = False
            End If
            Return gblnStartSOWithNoLabel
        Catch ex As Exception
            Throw New Exception("Error in IsSOStartedWithNoLabel" & vbCrLf & ex.Message)
            'Finally
            '   fs.Close()
        End Try

    End Function
    Friend Shared Sub SaveElementValue(ByVal xmldoc As XmlDataDocument, ByVal strElementName As String, ByVal strValue As String)

        Dim xnl As XmlNodeList
        Dim xn As XmlNode
        Try
            xnl = xmldoc.GetElementsByTagName(strElementName)
            For Each xn In xnl
                'WO#755 xn.ChildNodes.Item(0).InnerText = strValue
                xn.InnerText = strValue     'WO#755
            Next
        Catch ex As Exception
            Throw New Exception("Error in  SaveElementValue" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Shared Function GetElementValue(ByVal xmldoc As XmlDataDocument, ByVal strElementName As String) As String

        Dim xnl As XmlNodeList
        Dim xn As XmlNode
        Dim str As String = Nothing
        Try
            xnl = xmldoc.GetElementsByTagName(strElementName)
            For Each xn In xnl
                str = xn.ChildNodes.Item(0).InnerText.Trim()
            Next
            Return str
        Catch ex As Exception
            Throw New Exception("Error in GetElementValue" & vbCrLf & ex.Message)
        End Try
    End Function
    'WO#654 Add Stop
    'WO#718 Add Start
    Public Shared Function GetConrolTableValues(strKey As String, strSubKey As String, Optional ByVal strAction As String = Nothing) As Array
        Dim daCT As New dsControlTableTableAdapters.PPsp_Control_SelTableAdapter
        Dim dtCT As New dsControlTable.PPsp_Control_SelDataTable
        Dim strValues() As String = {String.Empty, String.Empty}
        Try
            daCT.Fill(dtCT, strKey, strSubKey, strAction)
            If dtCT.Rows.Count > 0 Then
                strValues(0) = dtCT.Rows(0).Item("Value1")
                strValues(1) = dtCT.Rows(0).Item("Value2")
            End If
        Catch ex As SqlClient.SqlException
            'Just do nothing to return zero values if network connection failure or failare on SQL 
        Catch ex As Exception
            Throw New Exception("Error in GetConrolTableValues" & vbCrLf & ex.Message)
        Finally
            GetConrolTableValues = strValues
        End Try
    End Function

    'Private Shared Sub WriteLabelInfoToXMLFile(ByVal strCaseLabelFileName As String, ByVal strPkgLine As String, ByVal strSCC As String, ByVal strUPC As String, ByVal intShopOrder As Integer, ByVal intUnitsPerCase As Integer, _
    '                                       ByVal intCasesPerPallet As Integer, ByVal intScheduledCases As Integer, ByVal strItemNumber As String, ByVal strItemDescription As String, ByVal intTie As Integer, _
    '                                       ByVal intTier As Integer, ByVal strSOStartTime As String)
    '    Dim xmldoc As New XmlDataDocument
    '    Dim strLabelKey As String

    '    Try

    '        With xmldoc

    '            .Load(strCaseLabelFileName)
    '            strLabelKey = strPkgLine & intShopOrder.ToString

    '            SaveElementValue(xmldoc, "SCC", strSCC)
    '            SaveElementValue(xmldoc, "UPC", strUPC)
    '            SaveElementValue(xmldoc, "PackagingLine", strPkgLine)
    '            SaveElementValue(xmldoc, "ShopOrder", intShopOrder.ToString)
    '            SaveElementValue(xmldoc, "LabelKey", strLabelKey)
    '            SaveElementValue(xmldoc, "UnitsPerCase", intUnitsPerCase.ToString)
    '            SaveElementValue(xmldoc, "CasesPerPallet", intCasesPerPallet.ToString)
    '            SaveElementValue(xmldoc, "ScheduledCases", intScheduledCases.ToString)
    '            SaveElementValue(xmldoc, "CaseCount", "0")
    '            SaveElementValue(xmldoc, "ItemNumber", strItemNumber)
    '            SaveElementValue(xmldoc, "ItemDescription", strItemDescription)
    '            SaveElementValue(xmldoc, "ScheduledCases", intScheduledCases.ToString)
    '            SaveElementValue(xmldoc, "Tie", intTie.ToString)
    '            SaveElementValue(xmldoc, "Tier", intTier.ToString)
    '            SaveElementValue(xmldoc, "ScheduledCases", intScheduledCases.ToString)
    '            SaveElementValue(xmldoc, "CreationTime", strSOStartTime)
    '            SaveElementValue(xmldoc, "LastUpdateTime", Now().ToString("yyyy/MM/dd HH:mm:ss"))
    '            .Save(strCaseLabelFileName)
    '        End With

    '    Catch ex As Exception
    '        Throw New Exception("Error in WriteLabelInfoToXML" & vbCrLf & ex.Message)
    '    End Try
    'End Sub

    'Public Shared Sub UpdateSessCtl(drSC As dsSessionControl.CPPsp_SessionControlIORow, strWhere As DBLocation)
    '    Using taSessCtl As New dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
    '        If strWhere = DBLocation.Local Or strWhere = DBLocation.Server_Local Then
    '            taSessCtl.Connection.ConnectionString = gstrLocalDBConnectionString
    '            taSessCtl.Update(drSC)
    '        ElseIf strWhere = DBLocation.Server Or strWhere = DBLocation.Server_Local Then
    '            taSessCtl.Connection.ConnectionString = gstrServerConnectionString
    '            taSessCtl.Update(drSC)
    '        End If
    '    End Using
    'End Sub
    'WO#718 Add Stop

    Public Shared Function GetTouchScreenCalibrationPgm() As String

        Dim regKey As RegistryKey = Nothing
        Dim strValue As String = Nothing
        Dim intValue As Integer
        GetTouchScreenCalibrationPgm = My.Settings.gstrUSBTouchScreenPgm2
        Try

            regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\Mouclass\Enum", False)
            intValue = regKey.GetValue("Count")
            'MessageBox.Show(intValue.ToString)
            For i As Short = intValue - 1 To 0 Step -1
                'If regKey.GetValue(i.ToString) = "USB\VID_04E7&PID_0050\50U12936" Then  'Arista USB 5 wired touch screen 
                'If Left(regKey.GetValue(i.ToString), 21) = "USB\VID_04E7&PID_0050" Then  'Arista USB 5 wired touch screen 
                If UCase(regKey.GetValue(i.ToString)).indexof("\VID_04E7&PID_0050") <> -1 Then
                    GetTouchScreenCalibrationPgm = My.Settings.gstrUSBTouchScreenPgm
                    Exit For
                    'ElseIf regKey.GetValue(i.ToString) = "USB\VID_0EEF&PID_0001\5&10de756c&0&1" Then    'TTX USB 5 wired touch screen
                ElseIf UCase(Left(regKey.GetValue(i.ToString), 21)) = "USB\VID_0EEF&PID_0001" Then    'TTX USB 5 wired touch screen
                    Exit For
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error in GetTouchScreenCalibrationPgm" & vbCrLf & ex.Message)
        Finally
            If Not IsNothing(regKey) Then
                regKey.Close()
            End If
        End Try

    End Function

    'WO#755 ADD Start

    Public Shared Function GetSOInfo(ByVal strAction As String, ByVal strFacility As String, ByVal intShopOrder As Integer, Optional ByVal strPackagingLine As String = "") As dsShopOrder.CPPsp_ShopOrderIORow
        Dim tblSo As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Try
            GetSOInfo = Nothing
            gtaSO.Fill(tblSo, strAction, strFacility, intShopOrder, strPackagingLine)
            With tblSo
                If .Rows.Count > 0 Then
                    GetSOInfo = .Rows(0)
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error in GetSOInfo" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Sub DownloadDataFromServer(ByVal strFacility As String, ByVal strComputerName As String)
        Dim connServer As New SqlConnection()
        Dim connStaging As New SqlConnection()
        Dim strstagingCnnStr As String
        Dim strSqlStmt As String
        Dim sdrDLTL As SqlDataReader
        Dim arlTableName As ArrayList
        Dim strTableName As String
        Dim blnHasFacilityColumn As Boolean
        Dim blnAtLeast1Success As Boolean
        Dim obj As Object
        Dim sdTableName As New SortedDictionary(Of String, Boolean)
        Dim dt As DataTable
        Dim ds As New DataSet

        Try
            connServer.ConnectionString = gstrServerConnectionString
            strstagingCnnStr = gstrLocalDBConnectionString
            connStaging.ConnectionString = strstagingCnnStr.Replace("LocalPowerPlant", "ImportData")

            Using cmdServer As New SqlCommand
                strSqlStmt = "SELECT TableName FROM tblDownLoadTableList WHERE Facility = '" & strFacility & "'"
                With cmdServer
                    .Connection = connServer
                    .CommandType = CommandType.Text
                    .CommandText = strSqlStmt
                    .Connection.Open()
                    sdrDLTL = .ExecuteReader()

                    arlTableName = New ArrayList
                    While sdrDLTL.Read
                        arlTableName.Add(sdrDLTL("TableName"))
                    End While
                    .Connection.Close()
                    'loop through the array list to get the table names and load those tables to a dataset for reuse
                    'check if the table has Facility Column, includes it in the filter

                    For Each strTableName In arlTableName

                        strSqlStmt = "SELECT 1 from information_schema.columns where table_name = '" & strTableName & "' and column_name = 'facility'"
                        .CommandText = strSqlStmt

                        .Connection.Open()     'if the connection failure, send email 

                        obj = .ExecuteScalar()
                        .Connection.Close()
                        blnHasFacilityColumn = CType(obj, Boolean)
                        sdTableName.Add(strTableName, blnHasFacilityColumn)

                        'WO#14867 If strTableName = "tblEquipment" Then
                        'WO#14867   strSqlStmt = "SELECT Active, facility, EquipmentID, [Type] ,SubType, Description, IPCSharedGroup,WorkCenter FROM tblEquipment Where facility = '" & strFacility & "'"
                        'WO#14867   Else
                        If blnHasFacilityColumn Then
                            strSqlStmt = "SELECT * from " & strTableName & " where facility = '" & strFacility & " '"
                        Else
                            strSqlStmt = "SELECT * from " & strTableName
                        End If
                        'WO#14867   End If
                        'MessageBox.Show(String.Format("Table - {0}", strTableName)) 'To be Deleted

                        .CommandText = strSqlStmt
                        .Connection.Open()
                        dt = New DataTable(strTableName)
                        dt.Load(.ExecuteReader)
                        .Connection.Close()
                        ds.Tables.Add(dt)
                    Next

                End With

            End Using

            blnAtLeast1Success = False
            Using cmdStaging As New SqlCommand

                strSqlStmt = "Update tblComputerConfig SET ReadyForDownLoad = 0 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & strComputerName & "'"
                With cmdStaging
                    .Connection = connStaging
                    .CommandType = CommandType.Text
                    .CommandText = strSqlStmt
                    If .Connection.State = ConnectionState.Closed Then
                        .Connection.Open()     'if the connection failure, send email 
                    End If
                    .ExecuteNonQuery()
                    '.Connection.Close()

                    'loop through the table download list to export the table on the list to IPC
                    For Each strTableName In arlTableName
                        'MessageBox.Show(strTableName) 'to be deleted
                        'For Spare IPC and the table has facility column than use Delete statement otherwise use Truncate statement
                        If sdTableName.Item(strTableName) = True Then
                            strSqlStmt = "DELETE " + strTableName + " Where facility = '" & strFacility & "'"
                        Else
                            strSqlStmt = "Truncate table " & strTableName
                        End If

                        .CommandText = strSqlStmt
                        '.Connection.Open()
                        .ExecuteNonQuery()
                        '.Connection.Close()
                        'MessageBox.Show("Before copy") 'to be deleted
                        Using bcp As SqlBulkCopy = New SqlBulkCopy(.Connection.ConnectionString, SqlBulkCopyOptions.TableLock)
                            bcp.BulkCopyTimeout = 1800  ' set processing timeout in Seconds
                            bcp.DestinationTableName = strTableName
                            bcp.WriteToServer(ds.Tables(strTableName))  'Save the data to a table of the dataset 
                        End Using
                        'MessageBox.Show("After copy") 'to be deleted
                        'In the DownloadTablelist Table of IPC staging area, update the download status flag = 1 (i.e. success) 
                        'and set the record Active to indicate the table is ready for download to local database.
                        'WO#359 strSQL = "Update tblDownLoadTableList set DownLoadStatus = 1, Active = 1 WHERE Facility = '" & strFacility & " ' AND TableName ='" & strTableName & "'"
                        strSqlStmt = "Update tblDownLoadTableList set DownLoadStatus = 0, Active = 1, LastDownload = '" & Now() & "' WHERE Facility = '" & strFacility & " ' AND TableName ='" & strTableName & "'"
                        .CommandText = strSqlStmt
                        'MessageBox.Show(strSqlStmt) 'to be deleted
                        .ExecuteNonQuery()
                        'MessageBox.Show("Updated") 'to be deleted
                        blnAtLeast1Success = True

                    Next    'Loop ended for Table Names
                End With

                Using cmdServer As New SqlCommand
                    With cmdServer
                        .Connection = connServer
                        .CommandType = CommandType.Text
                        .CommandText = strSqlStmt
                        .Connection.Open()

                        If blnAtLeast1Success Then

                            strSqlStmt = "Update tblComputerConfig set ReadyForDownLoad = 1 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & strComputerName & "'"
                            'Set Ready_For_DownLoad status to Yes in the remote IPC
                            'strSQL = "Update tblComputerConfig SET ReadyForDownLoad = 1 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & row("ComputerName") & "'"
                            cmdStaging.CommandText = strSqlStmt
                            cmdStaging.ExecuteNonQuery()

                            ' Set Ready_For_DownLoad status to Yes in the Power Plant SQL server for audit purpose
                            .CommandText = strSqlStmt
                            .ExecuteNonQuery()
                        End If

                        'Reset the Active flag in the Down Load table list to indicate all the files have been successfully downloaded
                        strSqlStmt = "Update tblDownLoadTableList set Active = 0 WHERE Facility = '" & strFacility & " '"
                        .ExecuteNonQuery()
                        .Connection.Close()
                    End With
                End Using
            End Using
        Catch ex As Exception
            'Throw ex
            Throw New Exception("Error in DownloadDataFromServer" & vbCrLf & ex.Message)
        Finally
            ds.Dispose()
        End Try

    End Sub
    'WO#755 ADD Stop

    Public Shared Function IsDeviceConnected(strDeviceName As String) As Boolean
        Dim strMsg As String
        Dim blnIsConnected As Boolean = False
        Dim ta As New dsIsDeviceConnectedTableAdapters.QueriesTableAdapter
        If My.Computer.Network.IsAvailable = True Then
            ta.IsDeviceConnected(strDeviceName, blnIsConnected)
        Else
            strMsg = "Network connection is not ready, Please check the IPC connection before any printer connections."
            SharedFunctions.PoPUpMSG(strMsg, "Check Network Connection", MessageBoxButtons.OK)
        End If

        Return blnIsConnected
    End Function

    'WO#871 ADD Start
    'WO#17432 DEL Start
    'Public Shared Function GetProbatClientPgm() As String()
    '    Dim daIPCControl As New dsIPCControlTableAdapters.tblIPCControlTableAdapter
    '    Dim dtIPCControl As New dsIPCControl.tblIPCControlDataTable
    '    Dim strResult(1) As String

    '    Try

    '        daIPCControl.Fill(dtIPCControl, "ProbatClientPgm")
    '        If dtIPCControl.Rows.Count > 0 Then
    '            'Probat client program path
    '            strResult(0) = dtIPCControl.Rows(0).Item("Value1")
    '            'Probat client program name
    '            strResult(1) = dtIPCControl.Rows(0).Item("Value2")
    '        End If
    '        Return (strResult)

    '    Catch ex As Exception
    '        Throw New Exception("Error in GetProbatClientPgm" & vbCrLf & ex.Message)
    '    End Try

    'End Function
    'WO#17432 DEL Stop

    Public Shared Function GetProbatEquipmentXref(strMachineType As String, strBPCSMachineID As String, strProbatEqID As String, strFacility As String) As dsProbatEquipment.dtProbatEquipmentRow
        Dim daProbatEQ As New dsProbatEquipmentTableAdapters.daProbatEquipment
        Dim dtProbatEQ As New dsProbatEquipment.dtProbatEquipmentDataTable
        Dim drProbatEQ As dsProbatEquipment.dtProbatEquipmentRow
        Dim blnActive As Boolean = 1

        Try
            drProbatEQ = Nothing
            daProbatEQ.Fill(dtProbatEQ, Nothing, blnActive, strMachineType, strBPCSMachineID, strProbatEqID, strFacility)
            If dtProbatEQ.Rows.Count > 0 Then
                drProbatEQ = dtProbatEQ.Rows(0)
            End If

            Return drProbatEQ
        Catch ex As Exception
            Throw New Exception("Error in GetProbatEquipmentXref" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Function GetReceivingStation(strFacility As String, strPackagingLine As String, strFlavoredCoffee As String, intOrderType As Integer) As String
        Dim drPE As dsProbatEquipment.dtProbatEquipmentRow
        Try
            GetReceivingStation = String.Empty
            drPE = GetProbatEquipmentXref("P", strPackagingLine, Nothing, strFacility)
            If Not IsNothing(drPE) Then
                If UCase(strFlavoredCoffee) = "Y" Then
                    GetReceivingStation = drPE.MachineFlavor
                ElseIf intOrderType = 3 Then
                    GetReceivingStation = drPE.MachineWholeBean
                ElseIf intOrderType = 5 Then
                    GetReceivingStation = drPE.MachineGround
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in GetReceivingStation" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetRawMaterialRequiredForSO(intShopOrder As Integer, strComponentItem As String) As Single
        Dim taBOM As New dsBillOfMaterialsTableAdapters.CPPsp_BillOfMaterialsIOTableAdapter
        Dim dtBOM As New dsBillOfMaterials.CPPsp_BillOfMaterialsIODataTable
        Dim drBOM As dsBillOfMaterials.CPPsp_BillOfMaterialsIORow
        Dim sglResult As Single
        Try
            taBOM.Fill(dtBOM, intShopOrder, "ALL", Nothing, strComponentItem)
            For Each drBOM In dtBOM
                sglResult = drBOM.Quantity
            Next
            Return sglResult
        Catch ex As Exception
            Throw New Exception("Error in GetRawMaterialRequiredForSO" & vbCrLf & ex.Message)
        End Try
    End Function

    'WO#2563    Public Shared Sub CloseShopOrderForProbatLine(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strItemNo As String, _
    'WO#2563                                    ByVal strDftPkgLine As String, ByVal strOperator As String, ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, _
    'WO#2563                                    ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, ByVal strLotID As String, ByVal dteProductionDate As DateTime, _
    'WO#2563                                    ByVal intShiftNo As Short, Optional ByVal trnServer As SqlTransaction = Nothing)
    'ALM#11828 DEL Start
    'Public Shared Sub CloseShopOrderForProbatLine(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strItemNo As String, _
    '                            ByVal strDftPkgLine As String, ByVal strOperator As String, ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime, _
    '                            ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, ByVal strLotID As String, ByVal dteProductionDate As DateTime, _
    '                            ByVal intShiftNo As Short, ByVal strOutputLocation As Short, Optional ByVal trnServer As SqlTransaction = Nothing)      'WO#2563
    'ALM#11828 DEL Stop
    Public Shared Sub CloseShopOrderForProbatLine(ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal strItemNo As String,
                           ByVal strDftPkgLine As String, ByVal strOperator As String, ByVal intQuantity As Integer, ByVal dteSOStartTime As DateTime,
                           ByVal strOrderComplete As String, ByVal intQtyPerPallet As Integer, ByVal strLotID As String, ByVal dteProductionDate As DateTime,
                           ByVal intShiftNo As Short, ByVal strOutputLocation As Short, ByVal intDestinationShopOrder As Integer, Optional ByVal trnServer As SqlTransaction = Nothing)           'ALM#11828
        'Create a pallet record with zero quantity and set the order complete flag to yes.

        Dim strSQLStmt As String
        Dim arParms() As SqlParameter = New SqlParameter(1) {}

        Try
            'WO#2563    CreatePallet(strFacility, 0, intShopOrder, strItemNo, strDftPkgLine, strOperator, _
            'WO#2563   intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate, intShiftNo, False, trnServer)

            'ALM#11828  CreatePallet(strFacility, 0, intShopOrder, strItemNo, strDftPkgLine, strOperator, _
            'ALM#11828    intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate, intShiftNo, False, strOutputLocation, trnServer)   'WO#2563
            CreatePallet(strFacility, 0, intShopOrder, strItemNo, strDftPkgLine, strOperator,
               intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate, intShiftNo, False, strOutputLocation, intDestinationShopOrder, 0, "ProbatLine Close SO", trnServer)   'WO#5370
            'WO#5370  intQuantity, dteSOStartTime, strOrderComplete, intQtyPerPallet, strLotID, dteProductionDate, intShiftNo, False, strOutputLocation, intDestinationShopOrder, trnServer)   'ALM#11828
            'Update Cases Produced and Pallet created in Session Control for packaging lines
            arParms(0) = New SqlParameter("@intQuantity", SqlDbType.Int)
            arParms(0).Value = intQuantity

            arParms(1) = New SqlParameter("@dteProductionDate", SqlDbType.DateTime)
            arParms(1).Value = dteProductionDate

            'It will not create a pallet in the tblPallet because the printing pallet label is suppressed,
            'so no need to add 1 to pallet count in session control table
            'WO#5370 If gblnAutoCaseCountLine = False Then
            If gblnAutoCountLine = False Then          'WO#5370
                strSQLStmt = "UPDATE tblSessionControl set LooseCases = 0, CasesProduced = CasesProduced +  @intQuantity, " &
                                    " ProductionDate = CONVERT(datetime,@dteProductionDate,120)"
            Else
                strSQLStmt = "UPDATE tblSessionControl set LooseCases = 0, " &
                                    " ProductionDate = CONVERT(datetime,@dteProductionDate,120)"
            End If

            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
            _dbServer.SaveChanges()

        Catch ex As Exception
            Throw New Exception("Error in CloseShopOrderForProbatLine" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Function IsActiveProbatEnableLine(strFacility As String, strLineID As String, blnYesOrNo As Boolean) As Boolean
        Try
            Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable
            Dim taEQ As New dsEquipmentTableAdapters.CPPsp_EquipmentIOTableAdapter
            Dim strAction As String = "ProbatEnabled"
            If blnYesOrNo = False Then
                strAction = "ProbatDisabled"
            End If
            taEQ.Fill(tblEQ, strFacility, strLineID, "P", "", strAction)
            If tblEQ.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsActiveProbatEnableLine " & vbCrLf & ex.Message)
        End Try
    End Function

    'WO#5370 Public Shared Function GetCurDestinationShopOrder(strPackagingLine As String, blnChkDestinationIPCConnFirst As Boolean) As String
    Public Shared Function GetCurDestinationShopOrder(strOutputLoacation As String, blnChkDestinationIPCConnFirst As Boolean, dtOL As dsOutputLocation.CPPsp_OutputLocation_SelDataTable) As String

        Dim strSQLStmt As String
        Dim arParms() As SqlParameter
        Dim intActiveShopOrder As Integer
        Dim blnConnected As Boolean
        Dim strResult As String = "0,0,0"
        Dim strDestinationPackagingLine As String = Nothing                          'WO#5370
        Dim drOL As dsOutputLocation.CPPsp_OutputLocation_SelRow                     'WO#5370
        Try

            'WO#5370 ADD Start
            If strOutputLoacation = "RAF" Then
                strResult = "0,1,0"
                Return strResult
            End If

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
            If IsNothing(strDestinationPackagingLine) = False AndAlso strDestinationPackagingLine <> "" Then
                'WO#5370 ADD Stop

                ReDim arParms(3)
                arParms = New SqlParameter(UBound(arParms)) {}

                If gblnSvrConnIsUp = True Then
                    arParms(0) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
                    arParms(0).Value = strDestinationPackagingLine     'WO#5370
                    'WO#5370   arParms(0).Value = strPackagingLine

                    arParms(1) = New SqlParameter("@bitChkDestinationIPCConnFirst", SqlDbType.Bit)
                    arParms(1).Value = blnChkDestinationIPCConnFirst

                    arParms(2) = New SqlParameter("@intActiveShopOrder", SqlDbType.Int)
                    arParms(2).Direction = ParameterDirection.Output

                    arParms(3) = New SqlParameter("@bitConnected", SqlDbType.Bit)
                    arParms(3).Direction = ParameterDirection.Output

                    strSQLStmt = "PPsp_GetCurrentActiveShopOrder"
                    SqlHelper.ExecuteScalar(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                    intActiveShopOrder = arParms(2).Value
                    blnConnected = arParms(3).Value
                    'WO#5370    strResult = intActiveShopOrder.ToString & "," & blnConnected.ToString
                    strResult = intActiveShopOrder.ToString & "," & blnConnected.ToString & "," & strDestinationPackagingLine   'WO#5370
                End If
            End If          'WO#5370
            'WO#17432   Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True And (ex.Number = 64 Or ex.Number = 1231)
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)        'WO#17432
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            MessageBox.Show(gcstSvrCnnFailure)
        Catch ex As Exception
            'WO#17432       Throw New Exception("Error in GetCurrentActiveShopOrder" & vbCrLf & ex.Message)
            Throw New Exception("Error in GetCurDestinationShopOrder" & vbCrLf & ex.Message)            'WO#17432
        End Try
        Return strResult
    End Function

    Public Shared Function IsComponentInBOM(ByVal intShopOrder As Integer, ByVal strComponent As String) As Boolean
        Dim daBOM As New dsBillOfMaterialsTableAdapters.CPPsp_BillOfMaterialsIOTableAdapter
        Dim dtBOM As New dsBillOfMaterials.CPPsp_BillOfMaterialsIODataTable
        Try
            daBOM.Fill(dtBOM, intShopOrder, "All", Nothing, strComponent)
            If dtBOM.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsComponentInBOM" & vbCrLf & ex.Message)
        End Try

    End Function
    'ALM#11828 ADD Stop
    'WO#6059 ADD Start
    Public Shared Function IsPrintRequestOverLimit(ByVal strFacility As String, ByVal strJobName As String, ByVal strDeviceType As String) As Boolean
        Dim daPROL As New dsLabelPrintJobsTableAdapters.PPsp_LabelPrintJobs_Sel_1TableAdapter
        Dim dtPROL As New dsLabelPrintJobs.PPsp_LabelPrintJobs_Sel_1DataTable
        daPROL.Fill(dtPROL, "CheckOverLimit", strFacility, strJobName, strDeviceType)
        Try
            If dtPROL.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw New Exception("Error in IsPrintRequestOverLimit" & vbCrLf & ex.Message)
        End Try

    End Function
    Public Shared Function PositionInPrintQueue(ByVal strFacility As String, ByVal strJobName As String, ByVal strDeviceType As String) As Integer
        Dim daPROL As New dsLabelPrintJobsTableAdapters.PPsp_LabelPrintJobs_SelTableAdapter
        Dim dtPROL As New dsLabelPrintJobs.PPsp_LabelPrintJobs_SelDataTable
        Try
            daPROL.Fill(dtPROL, "PositionInQueue", strFacility, strJobName, strDeviceType)
            If dtPROL.Rows.Count > 0 Then
                Return dtPROL.Rows(0).Item("RowNumber")
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw New Exception("Error in PositionInPrintQueue" & vbCrLf & ex.Message)
        End Try

    End Function
    Public Shared Function JobsInPrintQueue(ByVal strFacility As String, ByVal strJobName As String, ByVal strDeviceType As String) As dsLabelPrintJobs.PPsp_LabelPrintJobs_SelDataTable
        Dim daPROL As New dsLabelPrintJobsTableAdapters.PPsp_LabelPrintJobs_SelTableAdapter
        Dim dtPROL As New dsLabelPrintJobs.PPsp_LabelPrintJobs_SelDataTable
        Try
            daPROL.Fill(dtPROL, "JobList", strFacility, strJobName, Nothing)
            Return dtPROL

        Catch ex As Exception
            Throw New Exception("Error in PositionInPrintQueue" & vbCrLf & ex.Message)
        End Try

    End Function
    'WO#6059 ADD Stop
    'WO#5370 ADD Start
    Public Shared Function IsPalletCreated(drSessCtl As dsSessionControl.CPPsp_SessionControlIORow) As Boolean
        Dim daUnitCountInBound As New dsUnitCountInboundTableAdapters.PPsp_UnitCountInbound_SelTableAdapter
        Dim dtUnitcountINbound As New dsUnitCountInbound.PPsp_UnitCountInbound_SelDataTable
        Dim drUnitcountINbound As dsUnitCountInbound.PPsp_UnitCountInbound_SelRow
        IsPalletCreated = False
        'Check whether the related pallet record has been created
        daUnitCountInBound.Fill(dtUnitcountINbound, drSessCtl.Facility, drSessCtl.DefaultPkgLine, drSessCtl.StartTime, drSessCtl.ShopOrder, 2, Nothing, "DF")
        If dtUnitcountINbound.Rows.Count > 0 Then
            For Each drUnitcountINbound In dtUnitcountINbound
                If (Not IsNothing(drUnitcountINbound.OrderChange) AndAlso drUnitcountINbound.OrderChange <> "" AndAlso drUnitcountINbound.PalletId <> 0) Then
                    IsPalletCreated = True
                    Exit For
                End If
            Next
        Else
            'Check whether the related pallet creation request has been created but the pallet quantity is 0.
            daUnitCountInBound.Fill(dtUnitcountINbound, drSessCtl.Facility, drSessCtl.DefaultPkgLine, drSessCtl.StartTime, drSessCtl.ShopOrder, 1, Nothing, "DF")
            If dtUnitcountINbound.Rows.Count > 0 Then
                For Each drUnitcountINbound In dtUnitcountINbound
                    If (drUnitcountINbound.UnitCount = 0 AndAlso Not IsNothing(drUnitcountINbound.OrderChange)) Then
                        IsPalletCreated = True
                        Exit For
                    End If
                Next
            Else
                'If order change is not empty and the output location is "RAF", assume the shop order is stopped during the output location is set for RAF. 
                daUnitCountInBound.Fill(dtUnitcountINbound, drSessCtl.Facility, drSessCtl.DefaultPkgLine, drSessCtl.StartTime, drSessCtl.ShopOrder, 0, Nothing, "RAF")
                If dtUnitcountINbound.Rows.Count > 0 Then
                    For Each drUnitcountINbound In dtUnitcountINbound
                        If drUnitcountINbound.OrderChange <> "" Then
                            IsPalletCreated = True
                            Exit For
                        End If
                    Next
                End If
            End If
        End If

    End Function

    'WO#5370 ADD Stop

    'WO#17432 ADD Start
    Public Shared Function GetQATWorkFlowInfo(strFacility As String, strPackagingLine As String, strCurrQATEntryPoint As String, strFormName As String, Optional blnShowErrMsg As Boolean = True) As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
        Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow = Nothing
        Try

            'Retrieve QAT work flow and test information
            Using daWorkFlow As New dsQATWorkFlowTableAdapters.CPPsp_QATWorkFlow_SelTableAdapter
                Using dtWorkFlow As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
                    daWorkFlow.Fill(dtWorkFlow, strFacility, strPackagingLine, strCurrQATEntryPoint, True, strFormName)
                    If dtWorkFlow.Count = 1 Then
                        drWF = dtWorkFlow(0)
                    ElseIf dtWorkFlow.Count = 0 Then
                        If blnShowErrMsg = True Then
                            MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test definition", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        If blnShowErrMsg = True Then
                            MessageBox.Show("QAT definition was created incorrectly. Please contact QA.", "Error - Invalid test definition", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End If
                End Using
            End Using

            Return drWF
        Catch ex As Exception
            Throw New Exception("Error in GetQATWorkFlowInfo with form name" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetQATWorkFlowInfo(strFacility As String, strPackagingLine As String, strCurrQATEntryPoint As String) As dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
        Dim dtWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable = Nothing
        Try

            'Retrieve QAT work flow and test information
            Using daWorkFlow As New dsQATWorkFlowTableAdapters.CPPsp_QATWorkFlow_SelTableAdapter
                Using dtWorkFlow As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
                    daWorkFlow.Fill(dtWorkFlow, strFacility, strPackagingLine, strCurrQATEntryPoint, True, Nothing)
                    If dtWorkFlow.Count > 0 Then
                        dtWF = dtWorkFlow
                    ElseIf dtWorkFlow.Count = 0 Then
                        MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test definition", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        MessageBox.Show("QAT definition was created incorrectly. Please contact QA.", "Error - Invalid test definition", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End Using

            Return dtWF
        Catch ex As Exception
            Throw New Exception("Error in GetQATWorkFlowInfo" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Sub QATProcessWorkFlow(strEntryPoint As String)
        Dim drQATWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
        Const cblnActive As Boolean = 1
        Const cstrProjectName As String = "PowerPlant."
        Dim strFormFullName As String = Nothing
        Dim frm As Form

        Dim drScreen As DialogResult
        Try

            Using taQATWR As New dsQATWorkFlowTableAdapters.CPPsp_QATWorkFlow_SelTableAdapter
                Using dtQATWF As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
                    taQATWR.Fill(dtQATWF, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strEntryPoint, cblnActive, Nothing)
                    For Each drQATWF In dtQATWF
                        strFormFullName = cstrProjectName & drQATWF.FormName
                        If Type.GetType(strFormFullName) Is Nothing Then
                            MessageBox.Show(String.Format("QAT Form {0} can not been found. Related test is skipped. Please contact Supervisor.", drQATWF.TestFormID), "QAT Set up Error",
                                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            gdrQATWF = drQATWF
                            frm = ActivateForm(strFormFullName)
                            drScreen = frm.ShowDialog()
                            If drScreen = DialogResult.Cancel Then
                                Exit For
                            End If
                        End If
                    Next
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in QATProcessWorkFlow" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Function ActivateForm(strFormFullName As String) As Form
        Dim frm As Form
        Try
            frm = Activator.CreateInstance(Type.GetType(strFormFullName))
            Return frm
        Catch ex As Exception
            Throw New Exception("Error in ActivateForm" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Sub SaveQATCartonBoxTestResult(frm As Form, intTestResult As Integer, dteTestBatchID As DateTime, strFacility As String _
                        , intShopOrder As Integer, dteSOStartTime As DateTime, strDefaultPkgLine As String, dteTestEndTime As DateTime _
                        , dteTestStartTime As DateTime, strTestID As String, strQATEntryPoint As String)
        Dim drCBR As dsQATCartonBoxResult.CPPsp_QATCartonBoxResult_SelRow
        Try
            Using taCBR As New dsQATCartonBoxResultTableAdapters.CPPsp_QATCartonBoxResult_SelTableAdapter
                Using dtCBR As New dsQATCartonBoxResult.CPPsp_QATCartonBoxResult_SelDataTable
                    drCBR = dtCBR.NewRow()
                    With drCBR
                        .BatchID = dteTestBatchID
                        .Facility = strFacility
                        .InterfaceID = gstrInterfaceID
                        .PackagingLine = strDefaultPkgLine
                        .ShopOrder = gdrSessCtl.ShopOrder
                        .SOStartTime = dteSOStartTime
                        .TestEndTime = dteTestEndTime
                        .TestResult = intTestResult
                        .TestStartTime = dteTestStartTime
                        .TesterID = strTestID
                        .QATEntryPoint = strQATEntryPoint
                    End With
                    dtCBR.Rows.Add(drCBR)
                    If gblnSvrConnIsUp = True Then
                        Try
                            taCBR.Connection.ConnectionString = gstrServerConnectionString
                            taCBR.Update(drCBR)
                        Catch ex As SqlClient.SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True
                            SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            taCBR.Connection.ConnectionString = gstrLocalDBConnectionString
                            taCBR.Update(drCBR)
                        End Try
                    Else
                        taCBR.Connection.ConnectionString = gstrLocalDBConnectionString
                        taCBR.Update(drCBR)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error in SaveQATCartonBoxTestResult" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Sub SaveQATCheckWeigherResult(
                ByVal dteBatchID As Global.System.Nullable(Of DateTime),
                ByVal strFacility As String,
                ByVal strInterfaceID As String,
                ByVal dteOverrideID As Global.System.Nullable(Of DateTime),
                ByVal intShopOrder As Global.System.Nullable(Of Integer),
                ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
                ByVal strPackagingLine As String,
                ByVal intRetestNo As Global.System.Nullable(Of Integer),
                ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
                ByVal intTestResult As Global.System.Nullable(Of Byte),
                ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
                ByVal strTesterID As String,
                ByVal strQATEntryPoint As String,
                ByVal decActualWeight As Global.System.Nullable(Of Decimal),
                ByVal decMaxWeight As Global.System.Nullable(Of Decimal),
                ByVal decMinWeight As Global.System.Nullable(Of Decimal),
                ByVal strRecipe As String,
                ByVal decTareWeight As Global.System.Nullable(Of Decimal),
                ByVal decTargetWeight As Global.System.Nullable(Of Decimal),
                ByVal intDetailTestResult As Global.System.Nullable(Of Integer),
                ByVal dteTestTime As Global.System.Nullable(Of DateTime)
                )

        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(21)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = strFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = strInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteOverrideID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteOverrideID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intRetestNo", SqlDbType.Int)
            arParms(iCnt).Value = intRetestNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTestResult", SqlDbType.TinyInt)
            arParms(iCnt).Value = intTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decActualWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decActualWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decMaxWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decMaxWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decMinWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decMinWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchRecipe", SqlDbType.VarChar)
            arParms(iCnt).Value = strRecipe

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decTareWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decTareWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decTargetWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decTargetWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intDetailTestResult", SqlDbType.Int)
            arParms(iCnt).Value = intDetailTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            strSQLStmt = "CPPsp_QATCheckWeigherResult_Maint"

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlClient.SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATCheckWeigherResult" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveQATCaseVisualTestResult(frm As Form, intTestResult As Integer, dteTestBatchID As DateTime, strFacility As String _
                , intShopOrder As Integer, dteSOStartTime As DateTime, strDefaultPkgLine As String, dteTestEndTime As DateTime _
                , dteTestStartTime As DateTime, strTestID As String, strQATEntryPoint As String)
        Dim drCVR As dsQATCaseVisualResult.CPPsp_QATCaseVisualResult_SelRow
        Try
            Using taCVR As New dsQATCaseVisualResultTableAdapters.CPPsp_QATCaseVisualResult_SelTableAdapter
                Using dtCVR As New dsQATCaseVisualResult.CPPsp_QATCaseVisualResult_SelDataTable
                    drCVR = dtCVR.NewRow()
                    With drCVR
                        .BatchID = dteTestBatchID
                        .Facility = strFacility
                        .InterfaceID = gstrInterfaceID
                        .PackagingLine = strDefaultPkgLine
                        .ShopOrder = gdrSessCtl.ShopOrder
                        .SOStartTime = dteSOStartTime
                        .TestEndTime = dteTestEndTime
                        .TestResult = intTestResult
                        .TestStartTime = dteTestStartTime
                        .TesterID = strTestID
                        .QATEntryPoint = strQATEntryPoint
                    End With
                    dtCVR.Rows.Add(drCVR)
                    If gblnSvrConnIsUp = True Then
                        Try
                            taCVR.Connection.ConnectionString = gstrServerConnectionString
                            taCVR.Update(drCVR)
                        Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True
                            SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            taCVR.Connection.ConnectionString = gstrLocalDBConnectionString
                            taCVR.Update(drCVR)
                        End Try
                    Else
                        taCVR.Connection.ConnectionString = gstrLocalDBConnectionString
                        taCVR.Update(drCVR)
                    End If
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in SaveQATCaseVisualTestResult" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveQATCameraVerificationTestResult(frm As Form, blnTestResult As Boolean, dteTestBatchID As DateTime, strFacility As String, intShopOrder As Integer _
                                                          , dteSOStartTime As DateTime, strDefaultPkgLine As String, dteTestEndTime As DateTime, dteTestStartTime As DateTime _
                                                          , strTesterID As String, strQATEntryPoint As String)
        Dim drCV As dsQATCameraResult.CPPsp_QATCameraResult_SelRow
        Try
            Using taCV As New dsQATCameraResultTableAdapters.CPPsp_QATCameraResult_SelTableAdapter
                Using dtCV As New dsQATCameraResult.CPPsp_QATCameraResult_SelDataTable
                    drCV = dtCV.NewRow
                    With drCV
                        .BatchID = dteTestBatchID
                        .Facility = strFacility
                        .InterfaceID = gstrInterfaceID
                        .PackagingLine = strDefaultPkgLine
                        .ShopOrder = gdrSessCtl.ShopOrder
                        .SOStartTime = dteSOStartTime
                        .TestEndTime = dteTestEndTime
                        .TestResult = blnTestResult
                        .TestStartTime = dteTestStartTime
                        .TesterID = strTesterID
                        .QATEntryPoint = strQATEntryPoint
                    End With
                    dtCV.Rows.Add(drCV)
                    If gblnSvrConnIsUp = True Then
                        Try
                            taCV.Connection.ConnectionString = gstrServerConnectionString
                            taCV.Update(drCV)
                        Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True
                            SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            taCV.Connection.ConnectionString = gstrLocalDBConnectionString
                            taCV.Update(drCV)
                        End Try
                    Else
                        taCV.Connection.ConnectionString = gstrLocalDBConnectionString
                        taCV.Update(drCV)
                    End If
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in SaveQATCameraVerificationTestResult" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveQATSelectPalletTypeResult(frm As Form, strPalletCode As String, dteTestBatchID As DateTime, strFacility As String, intShopOrder As Integer _
                                                    , dteSOStartTime As DateTime, strDefaultPkgLine As String, dteTestEndTime As DateTime, dteTestStartTime As DateTime _
                                                    , strTestID As String, strQATEntryPoint As String)
        Dim drSPT As dsQATSelectPalletTypeResult.CPPsp_QATPalletTypeResult_SelRow
        Try
            Using taSPT As New dsQATSelectPalletTypeResultTableAdapters.CPPsp_QATPalletTypeResult_SelTableAdapter
                Using dtSPT As New dsQATSelectPalletTypeResult.CPPsp_QATPalletTypeResult_SelDataTable
                    drSPT = dtSPT.NewRow
                    With drSPT
                        .BatchID = dteTestBatchID
                        .Facility = strFacility
                        .InterfaceID = gstrInterfaceID
                        .PackagingLine = strDefaultPkgLine
                        .ShopOrder = gdrSessCtl.ShopOrder
                        .SOStartTime = dteSOStartTime
                        .TestEndTime = dteTestEndTime
                        .PalletCode = strPalletCode
                        .TestStartTime = dteTestStartTime
                        .TesterID = strTestID
                        .QATEntryPoint = strQATEntryPoint
                    End With
                    dtSPT.Rows.Add(drSPT)
                    If gblnSvrConnIsUp = True Then
                        Try
                            taSPT.Connection.ConnectionString = gstrServerConnectionString
                            taSPT.Update(drSPT)
                        Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                Or ex.Number = 1231 Or ex.Number = 10054) And gblnSvrConnIsUp = True
                            SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            taSPT.Connection.ConnectionString = gstrLocalDBConnectionString
                            taSPT.Update(drSPT)
                        End Try
                    Else
                        taSPT.Connection.ConnectionString = gstrLocalDBConnectionString
                        taSPT.Update(drSPT)
                    End If
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in SaveQATSelectPalletTypeResult" & vbCrLf & ex.Message)
        End Try
    End Sub

    'Public Shared Function GetLastQATOverrideByPassLanesRecord(strFacility As String, intShopOrder As Integer, strDefaultPkgLine As String) As dsQATOverride.CPPsp_QATOverride_SelRow
    '    GetLastQATOverrideByPassLanesRecord = Nothing

    '    Try
    '        'Retrieve the last supervisor override information of the shop order in the packaging line.
    '        Using daOvr As New dsQATOverrideTableAdapters.CPPsp_QATOverride_SelTableAdapter
    '            Using dtOvr As New dsQATOverride.CPPsp_QATOverride_SelDataTable
    '                If gblnSvrConnIsUp = True Then
    '                    daOvr.Connection.ConnectionString = gstrServerConnectionString
    '                Else
    '                    daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
    '                End If
    '                Try
    '                    daOvr.Fill(dtOvr, strFacility, Nothing, intShopOrder, strDefaultPkgLine)
    '                Catch ex As SqlException When gblnSvrConnIsUp = True And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
    '                           Or ex.Number = 1231 Or ex.Number = 10054)
    '                    daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
    '                    daOvr.Fill(dtOvr, strFacility, Nothing, intShopOrder, strDefaultPkgLine)
    '                End Try
    '                If dtOvr.Count > 0 Then
    '                    For Each drOvr As dsQATOverride.CPPsp_QATOverride_SelRow In dtOvr.Rows
    '                        'The return rows are sorted in descending sequence of the record inserted sequence
    '                        If drOvr.ByPassTest = False Then
    '                            GetLastQATOverrideByPassLanesRecord = drOvr
    '                            Exit For
    '                        End If
    '                    Next
    '                Else
    '                    GetLastQATOverrideByPassLanesRecord = Nothing
    '                End If
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        Throw New Exception("Error in GetLastQATOverrideByPassLanesRecord" & vbCrLf & ex.Message)
    '    End Try

    'End Function

    Public Shared Function GetLastQATOverrideByPassLanes(strFacility As String, intShopOrder As Integer, strDefaultPkgLine As String) As String
        Dim strByPassLanes As String = String.Empty

        Try
            'Retrieve the last supervisor override information of the shop order in the packaging line.
            Using daOvr As New dsQATOverrideTableAdapters.CPPsp_QATOverride_SelTableAdapter
                Using dtOvr As New dsQATOverride.CPPsp_QATOverride_SelDataTable
                    If gblnSvrConnIsUp = True Then
                        daOvr.Connection.ConnectionString = gstrServerConnectionString
                    Else
                        daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
                    End If
                    Try
                        daOvr.Fill(dtOvr, strFacility, Nothing, intShopOrder, strDefaultPkgLine)
                    Catch ex As SqlException When gblnSvrConnIsUp = True And (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054)
                        daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
                        daOvr.Fill(dtOvr, strFacility, Nothing, intShopOrder, strDefaultPkgLine)
                    End Try
                    If dtOvr.Count > 0 Then
                        For Each drOvr As dsQATOverride.CPPsp_QATOverride_SelRow In dtOvr.Rows
                            'The return rows are sorted in descending sequence of the record inserted sequence
                            If drOvr.ByPassTest = False Then
                                strByPassLanes = drOvr.ByPassLanes
                                Exit For
                            End If
                        Next
                    Else
                        strByPassLanes = String.Empty
                    End If
                End Using
            End Using
            Return strByPassLanes
        Catch ex As Exception
            Throw New Exception("Error in GetLastQATOverrideByPassLanes" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetPlantStaff(ByVal strStaffID As String, ByVal strWorkGroup As String, ByVal blnActive As Boolean, ByVal strStaffClass As String) As dsPlantStaffing.CPPsp_PlantStaffingIORow
        Dim drPlantStaffing As dsPlantStaffing.CPPsp_PlantStaffingIORow
        Try
            drPlantStaffing = Nothing
            Using taPlantStaff As New dsPlantStaffingTableAdapters.CPPsp_PlantStaffingIOTableAdapter
                Using dtPlantStaffing As New dsPlantStaffing.CPPsp_PlantStaffingIODataTable
                    taPlantStaff.Fill(dtPlantStaffing, "SelectAllFields", strStaffID, strWorkGroup, blnActive, strStaffClass)
                    If dtPlantStaffing.Count > 0 Then
                        drPlantStaffing = dtPlantStaffing.Rows(0)
                    End If
                End Using
            End Using

            Return drPlantStaffing

        Catch ex As Exception
            Throw New Exception("Error in GetPlantStaff" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function FindCurrQATEntryPoint(intCurShopOrder As Integer, strPackagingLine As String) As String
        ' Get last completed QAT information
        Dim strResultEntryPoint As String = String.Empty
        Dim intLastShopOrder As Integer
        Dim strLastEntryPoint As String
        Dim intLastWorkFlowSequence As Int16
        Dim drQATStatus As dsQATStatus.tblQATStatusRow

        Try
            If gstrQATWorkFlowType <> String.Empty Then
                strResultEntryPoint = gstrQATWorkFlowType
            Else
                If gstrQATWorkFlowType = cstrOnRequest Then
                    strResultEntryPoint = gstrQATWorkFlowType
                Else

                    drQATStatus = GetQATStatus()
                    With drQATStatus
                        If Not IsNothing(drQATStatus) Then
                            intLastShopOrder = .ShopOrder
                            strLastEntryPoint = .QATEntryPoint
                            intLastWorkFlowSequence = .WFTestSeq

                            'If shop order is closed, the test will the tests during the shop order closing
                            If .ShopOrderClosed Then
                                strResultEntryPoint = cstrClosing
                            ElseIf .ShiftChanged Then
                                strResultEntryPoint = cstrStartup
                            Else
                                'If the shop order no. of last completed test is different from the current one. It must be a Start-Up test.
                                If intCurShopOrder <> intLastShopOrder Or .ByPassAllTests = True Then
                                    strResultEntryPoint = cstrStartup
                                    'If the entry point of last completed test is In-Process test. It must be a In-Process test too.
                                ElseIf strLastEntryPoint = cstrInProcess Then
                                    strResultEntryPoint = cstrInProcess
                                Else
                                    'Retrieve QAT work flow and test information
                                    Using daWorkFlow As New dsQATWorkFlowTableAdapters.CPPsp_QATWorkFlow_SelTableAdapter
                                        Using dtWorkFlow As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
                                            daWorkFlow.Fill(dtWorkFlow, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strLastEntryPoint, True, Nothing)
                                            If dtWorkFlow.Count > 0 Then
                                                'If test sequance of the last completed QAT is less than the last one in the work flow, remain the same entry point.
                                                'Else it should be a In-Process test.
                                                If intLastWorkFlowSequence < dtWorkFlow(dtWorkFlow.Count - 1).TestSeq Then
                                                    strResultEntryPoint = strLastEntryPoint
                                                Else
                                                    strResultEntryPoint = cstrInProcess
                                                End If
                                            Else
                                                MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        End Using
                                    End Using
                                End If
                            End If
                        Else
                            strResultEntryPoint = cstrStartup
                        End If
                    End With
                End If
            End If
            Return strResultEntryPoint
        Catch ex As Exception
            Throw New Exception("Error in FindCurrQATEntryPoint" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Function IsShiftChanged() As Boolean
        Dim blnIsShiftChanged As Boolean = False
        Try

            Return blnIsShiftChanged
        Catch ex As Exception
            Throw New Exception("Error in FindCurrQATEntryPoint" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetIPCControl(strSearchKey) As String()

        Dim strResult As String() = {Nothing, Nothing}

        Try
            Using daIPCControl As New dsIPCControlTableAdapters.CPPsp_IPCControl_SelTableAdapter
                Using dtIPCControl As New dsIPCControl.CPPsp_IPCControl_SelDataTable
                    daIPCControl.Fill(dtIPCControl, strSearchKey)

                    If dtIPCControl.Rows.Count > 0 Then
                        'Probat client program path
                        strResult(0) = dtIPCControl.Rows(0).Item("Value1")
                        'Probat client program name
                        strResult(1) = dtIPCControl.Rows(0).Item("Value2")
                    End If
                End Using
            End Using
            Return (strResult)

        Catch ex As Exception
            Throw New Exception("Error in GetIPCControl" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Sub UpdateQATBatchID(dteTestBatchID As DateTime)
        Try
            IPCControlUpdate("QAT_BatchID", dteTestBatchID.ToString, "")
        Catch ex As Exception
            Throw New Exception("Error in UpdateQATBatchID" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Function GetQATStatus() As dsQATStatus.tblQATStatusRow
        Try
            Dim drQATStatus As dsQATStatus.tblQATStatusRow = Nothing
            Using taQATStatus As New dsQATStatusTableAdapters.tblQATStatusTableAdapter
                Using dtQATStatus As New dsQATStatus.tblQATStatusDataTable
                    taQATStatus.Fill(dtQATStatus)
                    If dtQATStatus.Count > 0 Then
                        drQATStatus = dtQATStatus(0)
                    End If
                End Using
            End Using
            Return drQATStatus
        Catch ex As Exception
            Throw New Exception("Error in GetQATStatus" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetQATBatchID(intTestSeqNo As Integer, strCurrQATEntryPoint As String) As DateTime
        Dim dteTestBatchID As DateTime
        Dim strResult As String()
        Try
            'if the test seq. no of the work flow is 1, that is the beginning of the test, reset the test batch id and save it 
            'if the form is on request, just reset the test batch id for this form and do not change the one on the table. 
            'else get the test batch id from the control table.
            If strCurrQATEntryPoint = cstrOnRequest Then
                dteTestBatchID = Now()
            ElseIf intTestSeqNo = 1 Then
                dteTestBatchID = Now()
                UpdateQATBatchID(dteTestBatchID)
            Else
                strResult = SharedFunctions.GetIPCControl("QAT_BatchID")
                dteTestBatchID = strResult(0)
            End If
            Return dteTestBatchID
        Catch ex As Exception
            Throw New Exception("Error in GetQATBatchID" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Sub UpdateQATStatus(blnByPassAllTests As Boolean, blnByPassTest As Boolean, intShopOrder As Integer,
                        strEntryPoint As String, intQATDefinID As Integer, strInterfaceID As String, intWorkFlowTestSeq As Int16,
                         Optional ByVal blnShiftChanged As Boolean = False, Optional ByVal blnShopOrderClosed As Boolean = False,
                         Optional ByVal blnStartedNewShopOrder As Boolean = False)
        Dim intNextDefinID As Integer
        Dim strNextQATEntryPoint As String = String.Empty
        Dim intNextWFTestSeq As Integer
        Dim strNextInterfaceFormID As String = String.Empty
        Dim drQATStatus As dsQATStatus.tblQATStatusRow

        Dim drWorkFlow As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
        Try
            'Find the next test only if the strEntry Point is not On Request
            If strEntryPoint = cstrOnRequest Then
                drQATStatus = GetQATStatus()
                With drQATStatus
                    intNextDefinID = .NextQATDefnID
                    strNextQATEntryPoint = .NextQATEntryPoint
                    intNextWFTestSeq = .NextWFTestSeq
                    strNextInterfaceFormID = .NextInterfaceFormID
                    blnShiftChanged = .ShiftChanged
                    blnShopOrderClosed = .ShopOrderClosed
                End With
            Else
                'Read work flow table to find the next seqence from the current test sequence.
                Using daWorkFlow As New dsQATWorkFlowTableAdapters.CPPsp_QATWorkFlow_SelTableAdapter
                    Using dtWorkFlow As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
                        Try
                            If blnShopOrderClosed = False And blnShiftChanged = False And blnStartedNewShopOrder = False Then
                                daWorkFlow.Fill(dtWorkFlow, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strEntryPoint, True, Nothing)
                                If dtWorkFlow.Count > 0 Then
                                    'If shop order closed was selected, next test will be the 'Line clearance' (last test of inProcess)

                                    For i As Integer = 0 To dtWorkFlow.Count - 1
                                        If dtWorkFlow(i).TestSeq > intWorkFlowTestSeq Then
                                            drWorkFlow = dtWorkFlow(i)
                                            intNextDefinID = drWorkFlow.QATDefnID
                                            strNextQATEntryPoint = strEntryPoint
                                            intNextWFTestSeq = drWorkFlow.TestSeq
                                            strNextInterfaceFormID = drWorkFlow.InterfaceFormID
                                            Exit For
                                        End If
                                    Next
                                Else
                                    MessageBox.Show("Cannot find the QAT definition for this line. QAT Status is not updated. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Exit Sub
                                End If
                            End If

                            'If can not find the next test within the same QAT Entry Point
                            If intNextDefinID = 0 Then
                                If blnShopOrderClosed = False Then
                                    If blnShiftChanged = True Or blnStartedNewShopOrder = True Then
                                        strNextQATEntryPoint = cstrStartup
                                    ElseIf blnShiftChanged = True Then                                          '2019/02/12
                                        strNextQATEntryPoint = cstrChangeShift                                  '2019/02/12
                                    Else
                                        strNextQATEntryPoint = cstrInProcess
                                    End If
                                Else
                                    strNextQATEntryPoint = cstrStartup
                                End If
                                daWorkFlow.Fill(dtWorkFlow, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strNextQATEntryPoint, True, Nothing)
                                If dtWorkFlow.Count > 0 Then
                                    dtWorkFlow.DefaultView.Sort = "TestSeq ASC"
                                    intNextWFTestSeq = dtWorkFlow(0).TestSeq
                                    intNextDefinID = dtWorkFlow(0).QATDefnID
                                    strNextInterfaceFormID = dtWorkFlow(0).InterfaceFormID
                                Else
                                    MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
            End If

            Using taQATStatus As New dsQATStatusTableAdapters.tblQATStatusTableAdapter
                taQATStatus.Update(blnByPassAllTests, blnByPassTest, intShopOrder, strEntryPoint, intQATDefinID, strInterfaceID, strNextInterfaceFormID, intNextDefinID, strNextQATEntryPoint, intNextWFTestSeq, Now(), intWorkFlowTestSeq, blnShiftChanged, blnShopOrderClosed)
            End Using

        Catch ex As Exception
            Throw New Exception("Error in UpdateQATStatus" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Sub IPCControlUpdate(strControlKey As String, strValue1 As String, strValue2 As String)
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try

            ReDim arParms(2)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@vchControlKey", SqlDbType.VarChar)
            arParms(iCnt).Value = strControlKey

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchValue1", SqlDbType.VarChar)
            arParms(iCnt).Value = strValue1

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchValue2", SqlDbType.VarChar)
            arParms(iCnt).Value = strValue2

            strSQLStmt = "CPPsp_IPCControl_Upd"
            SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)

        Catch ex As Exception
            Throw New Exception("Error in IPCControlUpdate" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveQATSmallestSalesUnitWeight(
                ByVal dteBatchID As Global.System.Nullable(Of Date),
                ByVal strFacility As String,
                ByVal strInterfaceID As String,
                ByVal decMaxWeight As Global.System.Nullable(Of Decimal),
                ByVal decMinWeight As Global.System.Nullable(Of Decimal),
                ByVal dteOverrideID As Global.System.Nullable(Of Date),
                ByVal strPackagingLine As String,
                ByVal intRetestNo As Global.System.Nullable(Of Integer),
                ByVal intShopOrder As Global.System.Nullable(Of Integer),
                ByVal dteSOStartTime As Global.System.Nullable(Of Date),
                ByVal decTareWeight As Global.System.Nullable(Of Decimal),
                ByVal decTargetWeight As Global.System.Nullable(Of Decimal),
                ByVal dteTestEndTime As Global.System.Nullable(Of Date),
                ByVal blnTestResult As Global.System.Nullable(Of Boolean),
                ByVal dteTestStartTime As Global.System.Nullable(Of Date),
                ByVal strTesterID As String,
                ByVal strQATEntryPoint As String,
                ByVal decActualWeight As Global.System.Nullable(Of Decimal),
                ByVal blnLastSample As Global.System.Nullable(Of Boolean),
                ByVal intLaneNo As Global.System.Nullable(Of Integer),
                ByVal intSampleNo As Global.System.Nullable(Of Integer),
                ByVal blnDetailTestResult As Global.System.Nullable(Of Boolean),
                ByVal dteTestTime As Global.System.Nullable(Of Date)
                )

        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(22)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = strFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = strInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decMaxWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decMaxWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decMinWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decMinWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteOverrideID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteOverrideID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intRetestNo", SqlDbType.Int)
            arParms(iCnt).Value = intRetestNo
            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decTareWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decTareWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decTargetWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decTargetWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitTestResult", SqlDbType.Bit)
            arParms(iCnt).Value = blnTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decActualWeight", SqlDbType.Decimal)
            arParms(iCnt).Value = decActualWeight

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitLastSample", SqlDbType.Bit)
            arParms(iCnt).Value = blnLastSample

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intLaneNo", SqlDbType.Int)
            arParms(iCnt).Value = intLaneNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intSampleNo", SqlDbType.Int)
            arParms(iCnt).Value = intSampleNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitDetailTestResult", SqlDbType.Bit)
            arParms(iCnt).Value = blnDetailTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            strSQLStmt = "CPPsp_QATWeightResult_Maint"

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATSmallestSalesUnitWeight" & vbCrLf & ex.Message)
        End Try

    End Sub

    ' WO#17432 ADD Start – AT 9/26/2018
    Public Shared Sub SaveQATLineClearance(
               ByVal dteBatchID As DateTime,
               ByVal intByPassAllTest As Integer,
               ByVal strFacility As String,
               ByVal strInterfaceID As String,
               ByVal strPackagingLine As String,
               ByVal intShopOrder As Integer,
               ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
               ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
               ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
               ByVal strTesterID As String,
               ByVal strQATEntryPoint As String,
               ByVal dteTaskEndTime As Global.System.Nullable(Of DateTime),
               ByVal intTaskID As Global.System.Nullable(Of Integer),
               ByVal dteTaskStartTime As Global.System.Nullable(Of DateTime),
               ByVal intTaskStatus As Global.System.Nullable(Of Integer)
               )

        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(15)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitByPassAllTest", SqlDbType.Int)
            arParms(iCnt).Value = intByPassAllTest

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = strFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = strInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTaskEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTaskEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTaskID", SqlDbType.Int)
            arParms(iCnt).Value = intTaskID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTaskStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTaskStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTaskStatus", SqlDbType.Int)
            arParms(iCnt).Value = intTaskStatus

            strSQLStmt = "CPPsp_QATLineClearanceResult_Maint"

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                    ' WO#17432 ADD Start – AT 9/25/2018
                    ' Original 
                    ' Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True

                    ' Error: 40: Could Not Open a Connection to SQL Server
                    ' Error: 53 A Network-related or instance-specific error occured while establishing a connection to SQL Server.
                    ' Error: 64 Transport-level error has occurred when receiving results from the server. 
                    ' Error: 1231 could not open a connection to sql server
                    ' Error: 2601 - Violation in unique index (Cannot insert duplicate key row in object)
                    ' Error: 2627 - Violation in unique constraint (although it is implemented using unique index)
                    ' Error: 10054 A transport-level error has occurred when sending the request to the server.

                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATLineClearance." & vbCrLf & ex.Message)
        End Try

    End Sub
    ' WO#17432 ADD Stop – AT 9/26/2018
    ' WO#17432 ADD Start – AT 3/27/2019
    Public Shared Sub SaveQATStartUpChecks(
               ByVal dteBatchID As DateTime,
               ByVal intByPassAllTest As Integer,
               ByVal strFacility As String,
               ByVal strInterfaceID As String,
               ByVal strPackagingLine As String,
               ByVal strQATEntryPoint As String,
               ByVal intShopOrder As Integer,
               ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
               ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
               ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
               ByVal dteTaskEndTime As Global.System.Nullable(Of DateTime),
               ByVal intTaskID As Global.System.Nullable(Of Integer),
               ByVal dteTaskStartTime As Global.System.Nullable(Of DateTime),
               ByVal intTaskStatus As Global.System.Nullable(Of Integer),
               ByVal strTesterID As String
               )

        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ReDim arParms(15)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitByPassAllTest", SqlDbType.Int)
            arParms(iCnt).Value = intByPassAllTest

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = strFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = strInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTaskEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTaskEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTaskID", SqlDbType.Int)
            arParms(iCnt).Value = intTaskID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTaskStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTaskStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTaskStatus", SqlDbType.Int)
            arParms(iCnt).Value = intTaskStatus

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            strSQLStmt = "CPPsp_QATStartUpResult_Maint"

            'The insert and update of header and detail records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)

                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATStartupChecks." & vbCrLf & ex.Message)
        End Try

    End Sub
    ' WO#17432 ADD Stop – AT 3/27/2019
    ' WO#17432 ADD Start – AT 1/23/2019
    Public Shared Function GetSORawMaterialInProbat(ByVal intShopOrder As Integer, ByVal strFacility As String) As String
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Try
            ReDim arParms(2)
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Faccility Input Parameter
            arParms(0) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(0).Value = intShopOrder

            ' Overrided Shift Input Parameter
            arParms(1) = New SqlParameter("@varFacility", SqlDbType.VarChar, 3)
            arParms(1).Value = strFacility

            ' Production Date Output Parameter
            arParms(2) = New SqlParameter("@varRawMaterialID", SqlDbType.VarChar, 35)
            arParms(2).Direction = ParameterDirection.Output
            strSQLStmt = "PPsp_GetSORawMaterialInProbat"
            SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)

            If IsDBNull(arParms(2).Value) Then
                GetSORawMaterialInProbat = String.Empty
            Else
                GetSORawMaterialInProbat = arParms(2).Value.ToString
            End If

        Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                       Or ex.Number = 1231 Or ex.Number = 10054)
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            Return String.Empty
        Catch ex As Exception
            Throw New Exception("Error in GetSORawMaterialInProbat" & vbCrLf & ex.Message)
        End Try

    End Function

    ' WO#17432 ADD Stpf – AT 1/23/2019

    Public Shared Sub SaveQATMaterialsValidation(
                ByVal dteBatchID As DateTime,
                ByVal strFacility As String,
                ByVal strInterfaceID As String,
                ByVal strPackagingLine As String,
                ByVal intShopOrder As Integer,
                ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
                ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
                ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
                ByVal strTesterID As String,
                ByVal strQATEntryPoint As String,
                ByVal strComponentNo As String,
                ByVal dteOverrideID As Global.System.Nullable(Of DateTime),
                ByVal strScannedComponentNo As String,
                ByVal intScannedLotNo As String,
                ByVal blnTestResult As Global.System.Nullable(Of Boolean),
                ByVal dteTestTime As Global.System.Nullable(Of DateTime)
                )

        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(15)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = strFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = strInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchComponentNo", SqlDbType.VarChar)
            arParms(iCnt).Value = strComponentNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteOverrideID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteOverrideID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchScannedComponentNo", SqlDbType.VarChar)
            arParms(iCnt).Value = strScannedComponentNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intScannedLotNo", SqlDbType.VarChar)
            arParms(iCnt).Value = intScannedLotNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitTestResult", SqlDbType.Bit)
            arParms(iCnt).Value = blnTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            strSQLStmt = "CPPsp_QATMaterialsResult_Maint"

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATMaterialsValidation." & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Sub SaveQATDateCodeResult(
            ByVal dteBatchID As Global.System.Nullable(Of DateTime),
            ByVal strDateCodeValue As String,
            ByVal intDateCodeType As Global.System.Nullable(Of Integer),
            ByVal strFacility As String,
            ByVal strInterfaceID As String,
            ByVal strPackagingLine As String,
            ByVal intRetestNo As Global.System.Nullable(Of Integer),
            ByVal intShopOrder As Global.System.Nullable(Of Integer),
            ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
            ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
            ByVal intTestResult As Global.System.Nullable(Of Byte),
            ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
            ByVal dteTestTime As Global.System.Nullable(Of DateTime) _
            , ByVal strTesterID As String _
            , ByVal strQATEntryPoint As String
           )                                                                    'WO#17432 AT 11/15/201

        'Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(14)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchDateCodeValue", SqlDbType.VarChar)
            arParms(iCnt).Value = strDateCodeValue

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intDateCodeType", SqlDbType.Int)
            arParms(iCnt).Value = intDateCodeType

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = strFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = strInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = strPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intRetestNo", SqlDbType.Int)
            arParms(iCnt).Value = intRetestNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTestResult", SqlDbType.TinyInt)
            arParms(iCnt).Value = intTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            ' WO#17432 ADD Start – AT 11/15/2018
            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            strSQLStmt = "CPPsp_QATDateCodeResult_Maint"
            'strSQLStmt = "CPPsp_QATDateCodeResult_Add"
            ' WO#17432 ADD Stop – AT 11/15/2018

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATDateCodeResult" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveQATOxygen(
                  ByVal dteBatchID As Global.System.Nullable(Of Date),
                  ByVal vchFacility As String,
                  ByVal vchInterfaceID As String,
                  ByVal decMaxOxygen As Global.System.Nullable(Of Decimal),
                  ByVal dteOverrideID As Global.System.Nullable(Of DateTime),
                  ByVal intShopOrder As Global.System.Nullable(Of Integer),
                  ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
                  ByVal vchPackagingLine As String,
                  ByVal intRetestNo As Global.System.Nullable(Of Integer),
                  ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
                  ByVal intTestResult As Global.System.Nullable(Of Byte),
                  ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
                  ByVal strTesterID As String,
                  ByVal strQATEntryPoint As String,
                  ByVal decOxygen As Global.System.Nullable(Of Decimal),
                  ByVal bitLastSample As Global.System.Nullable(Of Boolean),
                  ByVal intLaneNo As Global.System.Nullable(Of Integer),
                  ByVal intSampleNo As Global.System.Nullable(Of Integer),
                  ByVal bitDetailTestResult As Global.System.Nullable(Of Boolean),
                  ByVal dteTestTime As Global.System.Nullable(Of DateTime)
   )

        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(19)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = vchFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = vchInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decMaxOxygen", SqlDbType.Decimal)
            arParms(iCnt).Value = decMaxOxygen

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteOverrideID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteOverrideID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = vchPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intRetestNo", SqlDbType.Int)
            arParms(iCnt).Value = intRetestNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intTestResult", SqlDbType.TinyInt)
            arParms(iCnt).Value = intTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@decOxygen", SqlDbType.Decimal)
            arParms(iCnt).Value = decOxygen

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitLastSample", SqlDbType.Bit)
            arParms(iCnt).Value = bitLastSample

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intLaneNo", SqlDbType.Int)
            arParms(iCnt).Value = intLaneNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intSampleNo", SqlDbType.Int)
            arParms(iCnt).Value = intSampleNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitDetailTestResult", SqlDbType.Bit)
            arParms(iCnt).Value = bitDetailTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            'strSQLStmt = "CPPsp_QATWeightResult_Maint"
            strSQLStmt = "CPPsp_QATOxygenResult_Maint"  'MH used proper sp name

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in UpdateOxygenLog" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Shared Sub SaveQATPressure(
              ByVal dteBatchID As Global.System.Nullable(Of Date),
              ByVal vchFacility As String,
              ByVal vchInterfaceID As String,
              ByVal dteOverrideID As Global.System.Nullable(Of DateTime),
              ByVal intShopOrder As Global.System.Nullable(Of Integer),
              ByVal dteSOStartTime As Global.System.Nullable(Of DateTime),
              ByVal vchPackagingLine As String,
              ByVal intRetestNo As Global.System.Nullable(Of Integer),
              ByVal dteTestEndTime As Global.System.Nullable(Of DateTime),
              ByVal bitTestResult As Global.System.Nullable(Of Boolean),
              ByVal dteTestStartTime As Global.System.Nullable(Of DateTime),
              ByVal strTesterID As String,
              ByVal strQATEntryPoint As String,
              ByVal intLaneNo As Global.System.Nullable(Of Integer),
              ByVal intSampleNo As Global.System.Nullable(Of Integer),
              ByVal bitDetailTestResult As Global.System.Nullable(Of Boolean),
              ByVal dteTestTime As Global.System.Nullable(Of DateTime)
    )

        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(16)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = vchFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchInterfaceID", SqlDbType.VarChar)
            arParms(iCnt).Value = vchInterfaceID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteOverrideID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteOverrideID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intShopOrder", SqlDbType.Int)
            arParms(iCnt).Value = intShopOrder

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteSOStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteSOStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPackagingLine", SqlDbType.VarChar)
            arParms(iCnt).Value = vchPackagingLine

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intRetestNo", SqlDbType.Int)
            arParms(iCnt).Value = intRetestNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestEndTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestEndTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitTestResult", SqlDbType.Bit)
            arParms(iCnt).Value = bitTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestStartTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestStartTime

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(iCnt).Value = strTesterID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@chrQATEntryPoint", SqlDbType.Char)
            arParms(iCnt).Value = strQATEntryPoint

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intLaneNo", SqlDbType.Int)
            arParms(iCnt).Value = intLaneNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intSampleNo", SqlDbType.Int)
            arParms(iCnt).Value = intSampleNo

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitDetailTestResult", SqlDbType.Bit)
            arParms(iCnt).Value = bitDetailTestResult

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteTestTime", SqlDbType.DateTime)
            arParms(iCnt).Value = dteTestTime

            'strSQLStmt = "CPPsp_QATWeightResult_Maint"
            strSQLStmt = "CPPsp_QATPressureResult_Maint"  'MH used proper sp name

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveQATPressure" & vbCrLf & ex.Message)
        End Try

    End Sub

    ' WO#17432 ADD Start – AT 12/04/2018
    Public Shared Function DescryptPassword(ByVal vchPassword As String)
        Dim strNewPass As String = String.Empty
        strNewPass = Cryptography.Decrypt(vchPassword)
        Return strNewPass
    End Function
    Public Shared Function EncryptPassword(ByVal vchPassword As String)
        Dim strNewPass As String = String.Empty
        strNewPass = Cryptography.Encrypt(vchPassword)
        Return strNewPass
    End Function

    Public Shared Sub SavePlantStaffPassword(
            ByVal vchFacility As String,
            ByVal intStaffID As Global.System.Nullable(Of Integer),
            ByVal vchPassword As String,
            ByVal bitResetPassword As Global.System.Nullable(Of Boolean),
            ByVal dteDateLastChange As Global.System.Nullable(Of DateTime)
            )

        Dim intRtnCde As Integer = 0
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16

        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(4)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@vchFacility", SqlDbType.VarChar)
            arParms(iCnt).Value = vchFacility

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@intStaffID ", SqlDbType.Int)
            arParms(iCnt).Value = intStaffID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchPassword", SqlDbType.VarChar)
            arParms(iCnt).Value = vchPassword

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitResetPassword", SqlDbType.Bit)
            arParms(iCnt).Value = bitResetPassword

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@dteDateLastChange", SqlDbType.DateTime)
            arParms(iCnt).Value = dteDateLastChange

            ' strSQLStmt = "CPPsp_QATPressureResult_Maint"  'MH used proper sp name

            strSQLStmt = "UPDATE tblPlantStaff SET ResetPassword = @bitResetPassword, DateLastChange = @dteDateLastChange, Password = @vchPassword " &
                " WHERE StaffID = @intStaffID AND Facility = @vchFacility"

            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    intRtnCde = SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.Text, strSQLStmt, arParms)
                    intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
                Catch ex As SqlException When (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121 _
                               Or ex.Number = 1231 Or ex.Number = 10054)
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
                End Try

            Else
                intRtnCde = SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.Text, strSQLStmt, arParms)
                intRtnCde = SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, CommandType.Text, strSQLStmt, arParms)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SavePlantStaffPassword" & vbCrLf & ex.Message)
        End Try

    End Sub
    ' WO#17432 ADD Stop – AT 12/04/2018
    ' WO#17432 ADD Start – AT 12/03/2018
    Public Shared Function QATIsTested(ByVal strFormName As String, ByVal dteBatchID As Global.System.Nullable(Of Date)) As Boolean
        ' Dim cnnServer As SqlConnection = Nothing
        Dim arParms() As SqlParameter
        Dim strSQLStmt As String
        Dim iCnt As Int16
        Try
            ' cnnServer = New SqlConnection(gstrServerConnectionString)
            ' cnnServer.Open()
            ReDim arParms(2)
            arParms = New SqlParameter(UBound(arParms)) {}

            iCnt = 0
            arParms(iCnt) = New SqlParameter("@dteBatchID", SqlDbType.DateTime)
            arParms(iCnt).Value = dteBatchID

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@vchFormName", SqlDbType.VarChar)
            arParms(iCnt).Value = strFormName

            iCnt = iCnt + 1
            arParms(iCnt) = New SqlParameter("@bitIsTested", SqlDbType.Bit)
            arParms(iCnt).Direction = ParameterDirection.Output
            strSQLStmt = "CPPsp_QATIsTested"

            If gblnSvrConnIsUp = True Then
                Try
                    SqlHelper.ExecuteScalar(gstrServerConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)

                Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    Return 0
                End Try
            Else
                SqlHelper.ExecuteScalar(gstrLocalDBConnectionString, CommandType.StoredProcedure, strSQLStmt, arParms)
            End If
            QATIsTested = arParms(iCnt).Value
        Catch ex As Exception
            Throw New Exception("Error in QATIsTested" & vbCrLf & ex.Message)
        End Try
    End Function
    ' WO#17432 ADD Stop – AT 12/03/2018

    Public Shared Function PopRegularKeyPad(ByVal frm As Form, ByVal ctl As Control, Optional ByVal strPasswordChar As String = "") As DialogResult
        Dim ptCtlStartPoint As New Point
        Dim ptFormLocation As New Point
        Dim intX As Integer
        Dim intY As Integer
        Dim KeyPad As New frmRegularKeyPad

        Const intFormWidth As Short = 800
        Const intFormHeight As Short = 600

        ptFormLocation = frm.Location
        ptCtlStartPoint = ctl.Location
        intX = ptFormLocation.X + ptCtlStartPoint.X
        intY = ptFormLocation.Y + ptCtlStartPoint.Y + ctl.Height

        ' if the key pad is outside the right boundary of the form, set X = form width - keypad width
        If intX + KeyPad.Size.Width > intFormWidth Then
            intX = intFormWidth - KeyPad.Size.Width
        End If

        ' if the key pad is outside the bottom boundary of the form, set X = form height - keypad height
        If intY + KeyPad.Size.Height > intFormHeight Then
            intY = intFormHeight - KeyPad.Size.Height
        End If
        KeyPad.txtDisplay.Text = RTrim(ctl.Text)
        KeyPad.Location = New Point(intX, intY)
        KeyPad.StartPosition = FormStartPosition.Manual
        KeyPad.PasswordChar = strPasswordChar
        PopRegularKeyPad = KeyPad.ShowDialog()

    End Function

    Public Shared Sub showSplash2(ByVal objMessage As Object)
        Dim strMessage As String()
        strMessage = DirectCast(objMessage, String())
        frmSplash.SplashMessage = strMessage(0)
        frmSplash.SplashOption = strMessage(1)
        frmSplash.SplashTitle = strMessage(2)                '
        frmSplash.ShowDialog()
    End Sub

    'WO#17432 ADD Start - BL 1/23/2019
    Public Shared Function GetImage(strCoLOSIP As String, intCOLOSPort As Integer, strDeviceName As String, strLabelImageLocation As String, strPackagingLine As String, intShopOrder As Integer) As String

        Dim ipcClient As clsCoLOSCLI
        Dim strMessage As String = Nothing
        Dim strLabelImageFileName As String = Nothing
        Dim strResponse As String = Nothing

        Try

            strLabelImageFileName = strLabelImageLocation & "\" & strDeviceName & ".jpg"
            ipcClient = New clsCoLOSCLI(strCoLOSIP, intCOLOSPort)
            strMessage = String.Format("devices|lookup|{0}|preview|{1}{2}|@location={3}", strDeviceName, strPackagingLine, intShopOrder, strLabelImageFileName)

            With ipcClient
                strResponse = .LogOnToCoLOS()
                If strResponse.Substring(0, 3) <> "ER|" Then
                    strResponse = .SendMessage(strMessage)
                End If
                .LogOffFromCoLOS()
            End With

            If strResponse.Substring(0, 3) = "ER|" Then
                Return strResponse.Substring(strResponse.LastIndexOf("|") + 1)
            Else
                Return String.Empty
            End If

        Catch ex As Exception
            Throw New Exception("Error in GetImage." & vbCrLf & ex.Message)
        End Try

    End Function
    'WO#17432 ADD Stop - BL 1/23/2019
    'WO#17432 ADD Start - BL 3/28/2019
    Public Shared Sub UpdateQATTesters(ByVal strTesterID As String, ByVal strTesterName As String, ByVal blnRefreshData As Boolean)
        Dim arParms() As SqlParameter = Nothing
        Dim strSQLStmt As String = Nothing
        Dim strCnnStr As String
        Try
            'Insert Utility Techicians to table
            ReDim arParms(1)
            arParms = New SqlParameter(UBound(arParms)) {}

            'Tester ID Input Parameter
            arParms(0) = New SqlParameter("@vchTesterID", SqlDbType.VarChar)
            arParms(0).Value = strTesterID

            ' Action Input Parameter
            arParms(1) = New SqlParameter("@vchTesterName", SqlDbType.VarChar)
            arParms(1).Value = strTesterName

            strCnnStr = gstrLocalDBConnectionString

            If blnRefreshData = True Then
                strSQLStmt = "Truncate Table tblQATTester"
                SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.Text, strSQLStmt)
            End If

            strSQLStmt = "INSERT INTO tblQATTester (TesterID, TesterName) VALUES (@vchTesterID, @vchTesterName)"
            SqlHelper.ExecuteNonQuery(strCnnStr, CommandType.Text, strSQLStmt, arParms)

        Catch ex As Exception
            Throw New Exception("Error in UpdateQATTesters" & vbCrLf & ex.Message)
        End Try
    End Sub
    'WO#17432 ADD Stop - BL 3/28/2019

    'WO#17432 ADD Stop

    'Shared Sub SaveQATSmallestSalesUnitWeight(gdteTestBatchID As Date, p2 As String, gstrInterfaceID As String, decUpperLimit As Decimal, decLowerLimit As Decimal, dteOverrideID As Date, p7 As String, intRetestNo As Short, p9 As Integer, p10 As Date, p11 As Object, decTargetWgt As Decimal, p13 As Date, blnFinalTestResult As Boolean, dteTestStartTime As Date, p16 As Object, p17 As Object, p18 As Object, p19 As Object, p20 As Object, p21 As Object)
    '    Throw New NotImplementedException
    'End Sub

End Class


