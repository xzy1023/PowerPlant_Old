Public Class frmQATCheckWeigherValidation
    Dim strCurrQATEntryPoint As String
    Dim drwf As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim dteTestStartTime As DateTime = Now()
    Dim intRetestNo As Int16
    'Dim blnTestResult As Boolean
    Dim objCheckWeigher As clsCheckWeigher
    Dim decTargetWgt As Decimal
    Const cstrZeros As String = "00.00"

    ' WO#17432 ADD Start – AT 10/05/2018
    Dim decActualWeight As Decimal
    Const strNoData As String = "No Data"
    Dim blnByPassTest As Boolean = False
    ' WO#17432 ADD Stop – AT 10/05/2018
    ' WO#17432 ADD Start – AT 10/23/2018
    Dim decCalcUpperLimit As Decimal
    Dim decCalcLowerLimit As Decimal
    Dim decLabelWeight As Decimal
    Dim decLabelUnit As Decimal
    ' WO#17432 ADD Stop – AT 10/23/2018
    Dim drQATSpec As dsQATSpec.CPPsp_QATSpec_SelRow
    Dim thdShowSplash As System.Threading.Thread

    Private Enum TestResult
        Failed = 0
        Passsed = 1
        NA = 2
    End Enum

    Private Sub frmQATCheckWeigherValidation_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Is Start-Up or In-Process test?
            strCurrQATEntryPoint = SharedFunctions.FindCurrQATEntryPoint(gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

            'Retrieve QAT work flow and test information
            drwf = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strCurrQATEntryPoint, Me.Name)

            If Not IsNothing(drwf) Then
                'Set form title
                If drwf.TestFormTitle <> "" Then
                    UcHeading1.ScreenTitle = drwf.TestFormTitle
                    Me.Text = drwf.TestFormTitle
                End If

                'Get the test Batch ID
                gdteTestBatchID = SharedFunctions.GetQATBatchID(drwf.TestSeq, strCurrQATEntryPoint)
                ' WO#17432 ADD Start – AT 12/03/2018
                If SharedFunctions.QATIsTested(drwf.FormName, gdteTestBatchID) = True Then
                    MsgBox("The test has already done in the same QAT workflow batch.")
                    CloseForm()
                    Exit Sub
                End If
                ' WO#17432 ADD Stop – AT 12/03/2018

                lblRecipeName.Text = cstrZeros
                lblTargetWgt.Text = cstrZeros
                lblMaxWgt.Text = cstrZeros
                lblMinWgt.Text = cstrZeros
                lblTareWgt.Text = cstrZeros
                lblActualWgt.Text = cstrZeros

                'Load instruction to the screen
                If drwf.NoteID = 0 Then
                    txtNotes.Visible = False
                    lblNotes.Visible = False
                Else
                    txtNotes.Visible = True
                    lblNotes.Visible = True
                    txtNotes.Text = drwf.Note
                End If

                lblShopOrder.Text = gdrSessCtl.ShopOrder
                lblItemNo.Text = gdrSessCtl.ItemNumber

                'Get weight spec.
                If drwf.TestSpecID <> 0 Then
                    Using daSpec As New dsQATSpecTableAdapters.CPPsp_QATSpec_SelTableAdapter
                        Using dtSpec As New dsQATSpec.CPPsp_QATSpec_SelDataTable
                            daSpec.Fill(dtSpec, gdrSessCtl.Facility, drwf.TestSpecID, True)
                            If dtSpec.Count > 0 Then
                                drQATSpec = dtSpec(0)
                            Else
                                MessageBox.Show("The required weight spec. is not found. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                CloseForm()         ' WO#17432 – AT 11/29/2018
                                Exit Sub
                            End If
                        End Using
                    End Using
                Else
                    MessageBox.Show("The weight spec. is not setup in the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CloseForm()         ' WO#17432 – AT 11/29/2018
                    Exit Sub
                End If

                ' WO#17432 ADD Start – AT 10/23/2018
                decLabelWeight = 0
                decLabelUnit = 0
                decCalcLowerLimit = 0
                decCalcUpperLimit = 0
                decActualWeight = 0.0

                'Retrieve Shop Order record using the Shop Order from the Session Control
                Using tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                    gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl.ShopOrder, "")
                    If tblSO.Count > 0 Then
                        lblSNNWgt.Text = tblSO(0).LabelWeight.ToString
                        decLabelWeight = Convert.ToDecimal(tblSO(0).LabelWeight)
                        decLabelUnit = Convert.ToDecimal(tblSO(0).PackagesPerSaleableUnit)
                        Select Case drQATSpec.Formula
                            Case 1
                                decCalcLowerLimit = Convert.ToDecimal(tblSO(0).LabelWeight * tblSO(0).PackagesPerSaleableUnit) - drQATSpec.LwLmtFromTarget
                                decCalcUpperLimit = Convert.ToDecimal(tblSO(0).LabelWeight * tblSO(0).PackagesPerSaleableUnit) + drQATSpec.UpLmtFromTarget
                            Case 11      'V6.73 - OLSOV Checkweigher 03/13/2023
                                'Upper tolerance: (Capsule weight +0.7) * (Total units per package +1).
                                decCalcUpperLimit = Convert.ToDecimal((tblSO(0).LabelWeight + 0.7) * (tblSO(0).PackagesPerSaleableUnit + 1))

                                'Lower tolerance lookup table
                                'Labeled Qty	Tolerance (T1)	Lower Limit Calculation
                                '0-50g	        9%	            Carton net weight minus 9%
                                '>50 to 100	    4.5g	        Carton net weight minus 4.5g
                                '>100 to 200g	4.50%	        Carton net weight minus 4.5%
                                '>200 to 300g	9g	            Carton net weight minus 9g
                                '>300 to 500g	3%	            Carton net weight minus 3%
                                '>500 to 1kg	15g	            Carton net weight minus 15g

                                Dim decCartonNetWeight As Decimal = Convert.ToDecimal(tblSO(0).LabelWeight * tblSO(0).PackagesPerSaleableUnit)
                                Select Case decCartonNetWeight
                                    Case 0 To 50
                                        decCalcLowerLimit = decCartonNetWeight * (1 - 0.09)
                                    Case 51 To 100
                                        decCalcLowerLimit = decCartonNetWeight - 4.5
                                    Case 101 To 200
                                        decCalcLowerLimit = decCartonNetWeight * (1 - 0.045)
                                    Case 201 To 300
                                        decCalcLowerLimit = decCartonNetWeight - 9.0
                                    Case 301 To 500
                                        decCalcLowerLimit = decCartonNetWeight * (1 - 0.03)
                                    Case 501 To 1000
                                        decCalcLowerLimit = decCartonNetWeight - 15.0
                                    Case Else
                                        MessageBox.Show("Carton weight out of bounds.Please contact Supervisor.", "Error - Invalid specification formula", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        CloseForm()
                                        Exit Sub
                                End Select

                            Case Else
                                MessageBox.Show("Formula for the Specification is not defined. Please contact Supervisor.", "Error - Invalid specification formula", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                CloseForm()         ' WO#17432 – AT 11/29/2018
                                Exit Sub
                        End Select
                    Else
                        MessageBox.Show("Cannot find the shop order information. Please contact Supervisor.", "Error - Missing shop order information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        CloseForm()         ' WO#17432 – AT 11/29/2018
                        Exit Sub
                    End If
                End Using

                lblMinWgt.Text = decCalcLowerLimit.ToString
                lblMaxWgt.Text = decCalcUpperLimit.ToString
                lblTargetWgt.Text = decLabelUnit * decLabelWeight

                ' WO#17432 ADD Stop – AT 10/23/2018

                If drwf.NoteID <> 0 Then
                    lblNotes.Visible = True
                    txtNotes.Visible = True
                Else
                    lblNotes.Visible = False
                    txtNotes.Visible = False
                End If

                frmQATTester.ShowDialog()

                'Picking up the data from checkweigher if it is defined
                If Not drwf.IsTCPConnIDNull Then
                    LoadDataFromCheckWeigher(drwf.TCPConnID)
                Else
                    MessageBox.Show("QAT definition has not been setup to connect to which checkweigher. Test will be aborted. Contact Supervisor.", "Invalid QAT Defination Setup", MessageBoxButtons.OK)
                    CloseForm()         ' WO#17432 – AT 11/29/2018
                    Exit Sub
                End If
            Else
                MessageBox.Show("Cannot find the QAT workflow information. Please contact Supervisor.", "Error - Missing workflow information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CloseForm()
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub

    ' WO#17432 ADD Start – AT 10/05/2018
    Private Sub btnRetest_Click(sender As Object, e As EventArgs) Handles btnRetest.Click
        Dim lbl As Label
        Const intLblWidth As Int16 = 150, intLblHeight As Int32 = 27
        '       Const intIntialLblX As Int16 = 27, intIntialLblY As Int16 = 510
        Const intIntialLblX As Int16 = 200, intIntialLblY As Int16 = 530
        Const cstrLabelName As String = "lblRetestNo"

        Try
            '' WO#17432 ADD Start – AT 11/29/2018
            'intRetestNo = intRetestNo + 1
            'If btnRetest.Text = "Retry" Then
            '    If intRetestNo > 0 Then
            '        intRetestNo = intRetestNo - 1
            '    End If
            'End If
            '' WO#17432 ADD Stop – AT 11/29/2018
            btnNA.Visible = False
            If decActualWeight <> 0 Then
                SaveCheckWeigherResult(TestResult.Failed, False, Nothing)

                dteTestStartTime = Now()
                ' WO#17432 DEL Start – AT 11/29/2018
                intRetestNo = intRetestNo + 1
                ' WO#17432 DEL Stop – AT 11/29/2018
                For Each ctrl As Control In Controls
                    If TypeOf ctrl Is Label AndAlso ctrl.Name = cstrLabelName Then
                        Me.Controls.Remove(ctrl)
                    End If
                Next

                ' Create a label to display the ReTest no.
                lbl = New Label
                With lbl
                    .Location = New System.Drawing.Point(intIntialLblX, intIntialLblY)
                    .Size = New System.Drawing.Size(intLblWidth, intLblHeight)
                    .Text = String.Format("Retest No: {0}", intRetestNo)
                    .Name = cstrLabelName
                    .ForeColor = Color.White
                    .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                End With
                Me.Controls.Add(lbl)
            End If

            'Picking up the data from checkweigher if it is defined
            If Not drwf.IsTCPConnIDNull Then
                LoadDataFromCheckWeigher(drwf.TCPConnID)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnOverride_Click(sender As Object, e As EventArgs) Handles btnOverride.Click
        Dim drScreen As DialogResult
        ' WO#17432 ADD Start – AT 11/23/2018
        Dim dteTestEndTime As DateTime
        Dim dteTestTime As Nullable(Of DateTime)
        Dim decMaxWeight As Decimal
        Dim decMinWeight As Decimal
        ' WO#17432 ADD Stop – AT 11/23/2018
        Try
            'Create the values of the parameters for the form before calling it.
            Dim dteOverrideID As DateTime = Now()

            With frmQATOverrideLogOn
                .BatchID = gdteTestBatchID
                .QATDefnID = drwf.QATDefnID
                .QATEntryPoint = strCurrQATEntryPoint
                .OverrideID = dteOverrideID
                .TestTitle = UcHeading1.ScreenTitle
                .Alert = drwf.Alert
                drScreen = .ShowDialog(Me)
            End With

            'Update the override id on the header record
            If drScreen = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(dteOverrideID) AndAlso IsDate(dteOverrideID) Then
                    btnNA.Visible = False
                    '"Accept" button was pressed in the Override Screen
                    SaveCheckWeigherResult(TestResult.Failed, True, dteOverrideID)
                    'SharedFunctions.SaveQATCheckWeigherResult(gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, Nothing, gdrSessCtl.ShopOrder, gdrSessCtl.StartTime, gdrSessCtl.DefaultPkgLine, _
                    '                           intRetestNo, Now, blnTestResult, dteTestStartTime, decActualWeight, lblMaxWgt.Text, lblMinWgt.Text, lblRecipeName.Text, _
                    '                           lblTareWgt.Text, lblTargetWgt.Text, blnTestResult, Now _
                    '                           )

                    'SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
                    Me.Close()
                End If
            ElseIf drScreen = Windows.Forms.DialogResult.Cancel Then
                btnNA.Visible = False
                '"ByPass Test" button was pressed in the Override Screen
                ' WO#17432 ADD Start – AT 11/23/2018
                dteOverrideID = Now
                dteTestStartTime = Now
                dteTestEndTime = Now
                dteTestTime = Nothing
                decActualWeight = 0
                decMaxWeight = 0
                decMinWeight = 0
                SharedFunctions.SaveQATCheckWeigherResult(gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, dteOverrideID, gdrSessCtl.ShopOrder,
                                                Now, gdrSessCtl.DefaultPkgLine, intRetestNo, dteTestEndTime, _
                                                TestResult.Failed, dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                                                decActualWeight, decMaxWeight, decMinWeight, Nothing, _
                                                Nothing, Nothing, TestResult.Failed, dteTestTime _
                                                )
                ' WO#17432 ADD Stop – AT 11/23/2018

                'Update QAT processing status
                SharedFunctions.UpdateQATStatus(False, True, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
                Me.Close()
                ' WO#17432 ADD Stop – AT 10/02/2018
            ElseIf drScreen = Windows.Forms.DialogResult.Ignore Then
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
        End Try
    End Sub
    ' WO#17432 ADD Stop – AT 10/05/2018

    Private Sub btnPass_Click(sender As Object, e As EventArgs) Handles btnPass.Click
        Try
            SaveCheckWeigherResult(TestResult.Passsed, True, Nothing)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnFail_Click(sender As Object, e As EventArgs) Handles btnFail.Click
        Try
            btnRetest.Text = "Test After RCA"
            btnRetest.Visible = True
            btnOverride.Visible = drwf.AllowOverride
            btnNA.Visible = False
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNA_Click(sender As Object, e As EventArgs) Handles btnNA.Click
        Try
            SaveCheckWeigherResult(TestResult.NA, True, Nothing)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub SaveCheckWeigherResult(intTestResult As Integer, Optional blnUpdateQATStatus As Boolean = True, Optional dteOverrideID As Nullable(Of DateTime) = Nothing)
        Dim dteTestTime As Nullable(Of DateTime) = Now
        Try
            'It the test is N/A, set the test time to nothing for not writing detail record.
            If intTestResult = TestResult.NA Then
                dteTestTime = Nothing
            End If

            'Set Override ID to nothing to force to insert detail and header (without Override ID) records.
            SharedFunctions.SaveQATCheckWeigherResult(gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, Nothing, gdrSessCtl.ShopOrder, _
                                                    gdrSessCtl.StartTime, gdrSessCtl.DefaultPkgLine, _
                                                    intRetestNo, Now, intTestResult, dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                                                    decActualWeight, lblMaxWgt.Text, lblMinWgt.Text, lblRecipeName.Text, _
                                                    lblTareWgt.Text, lblTargetWgt.Text, intTestResult, dteTestTime _
                                                    )
            'If Override ID is not nothing, update the Override ID to the header only.
            If Not IsNothing(dteOverrideID) Then
                SharedFunctions.SaveQATCheckWeigherResult(gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, dteOverrideID, gdrSessCtl.ShopOrder, _
                                                        gdrSessCtl.StartTime, gdrSessCtl.DefaultPkgLine, _
                                                        intRetestNo, Now, intTestResult, dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                                                        decActualWeight, lblMaxWgt.Text, lblMinWgt.Text, lblRecipeName.Text, _
                                                        lblTareWgt.Text, lblTargetWgt.Text, intTestResult, dteTestTime _
                                                        )
            End If
            If blnUpdateQATStatus Then
                SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveCartonBoxTestResult" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub LoadDataFromCheckWeigher(intTCPConnID As Integer)
        ' WO#17432 ADD Start – AT 10/23/2018
        Dim strIPAddress As String = String.Empty
        ' WO#17432 ADD Stop – AT 10/23/2018
        Dim strResponse As String = Nothing
        Dim drTCPConn As dsQATTCPConn.CPPsp_QATTCPConn_SelRow
        Dim strMessage As String() = {"Please wait ...", "ElapseTime", "Retrieving data from Checkweigher"}
        'Dim strSplashOption As String = "ElapseTime"
        'Dim strSplashTitle As String = "Retrieving data from Checkweigher"
        Try
            thdShowSplash = New System.Threading.Thread(AddressOf SharedFunctions.showSplash2)
            thdShowSplash.Start(strMessage)
            'intPort = 2305
            'strCheckWeigerIP = "192.168.50.91"
            decActualWeight = 0
            lblActualWgt.Text = cstrZeros
            Using taTCPConn As New dsQATTCPConnTableAdapters.CPPsp_QATTCPConn_SelTableAdapter
                Using dtTCPConn As New dsQATTCPConn.CPPsp_QATTCPConn_SelDataTable
                    taTCPConn.Fill(dtTCPConn, gdrSessCtl.Facility, drwf.TCPConnID, True)
                    If dtTCPConn.Count > 0 Then
                        drTCPConn = dtTCPConn(0)
                        'objCheckWeigher = New clsCheckWeigher(drTCPConn.IPAddress, drTCPConn.Port)
                        ' WO#17432 ADD Start – AT 10/23/2018
                        strIPAddress = drTCPConn.IPAddress
                        objCheckWeigher = New clsCheckWeigher(strIPAddress.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", ""), drTCPConn.Port)
                        ' WO#17432 ADD Stop – AT 10/23/2018
                        With objCheckWeigher
                            strResponse = .TCPConnect()
                            'If strResponse <> "No Connection" Then
                            If IsNothing(strResponse) Then
                                If dtTCPConn(0).Model = "XE" Then
                                    If .IsHandShakeOK() Then
                                        'send command "WD_SET_FORMAT 2100" to change data receiving format 2100 for all the detail data.
                                        .TCPSendCommand(drTCPConn.Command1)
                                        strResponse = objCheckWeigher.TCPReceiveMessage()
                                        If strResponse <> strNoData Then
                                            ReceivedFormat2100Data(strResponse)
                                            If drTCPConn.Command2 <> String.Empty Then
                                                'send command "WD_SET_FORMAT x" to change data receiving format x for recepe name.
                                                .TCPSendCommand(drTCPConn.Command2)
                                                System.Threading.Thread.Sleep(3000)
                                                strResponse = .TCPReceiveMessage()
                                                If strResponse <> strNoData Then
                                                    ReceivedXSFormat3Data(strResponse)
                                                    ' WO#17432 ADD Start – AT 10/23/2018
                                                Else
                                                    ' WO#17432 ADD Stop – AT 10/23/2018
                                                    TestFailed()
                                                    MessageBox.Show("No data is received " & drTCPConn.Command2 & ". Is checkweigher operating properly? Please click on RETRY button to receive data again.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                                End If
                                            End If
                                            ' WO#17432 ADD Start – AT 10/23/2018
                                        Else
                                            ' WO#17432 ADD Stop – AT 10/23/2018
                                            TestFailed()
                                            MessageBox.Show("No data is received from " & drTCPConn.Command1 & ". Is checkweigher operating properly? Please click on RETRY button to receive data again.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                        End If
                                    Else
                                        TestFailed()
                                        MessageBox.Show("The checkweigher is not responding in handshake or being allocated by another session. If retry few times still having same issue, please contact supervisor.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                    End If
                                ElseIf dtTCPConn(0).Model = "XC" Then
                                    ' WO#17432 ADD Start – BL 2019/08/16
                                    'model XC interface data format does not provide tare weight, so hide the tare weight info on the screen.
                                    lblTareWgtLabel.Visible = False
                                    lblTareWgt.Visible = False
                                    ' WO#17432 ADD Stop – BL 2019/08/16
                                    If .IsHandShakeOK() Then
                                        'send command "WD_SET_FORMAT 3" to change data receiving format 3 for detail data.
                                        .TCPSendCommand(drTCPConn.Command1)
                                        System.Threading.Thread.Sleep(3000)
                                        'send command "WD_START to start receiving data from checkweigher
                                        strResponse = .Start()

                                        ' WO#17432 ADD Start – BL 2019/08/07
                                        'Dim intRetry As Integer = 0
                                        'While strResponse = strNoData And intRetry < 15     'Retry 15 times or data received
                                        '    strResponse = .Start()
                                        '    intRetry = intRetry + 1
                                        'End While
                                        ' WO#17432 ADD Stop – BL 2019/08/07

                                        If strResponse <> strNoData Then
                                            ReceivedXCFormat3Data(strResponse)
                                            .Abort()
                                            ' WO#17432 ADD Start – AT 10/23/2018

                                        Else
                                            ' WO#17432 ADD Stop– AT 10/23/2018
                                            TestFailed()
                                            ' Me.Refresh()
                                            MessageBox.Show("No data is received from checkweigher. Is it operating properly? Please click on RETRY button to receive data again.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                        End If
                                    Else
                                        TestFailed()
                                        'Me.Refresh()
                                        MessageBox.Show("The checkweigher is not responding in handshake or being allocated by another session. If retry few times still having same issue, please contact supervisor.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                    End If
                                End If
                            Else
                                ' WO#17432 ADD Start – AT 10/23/2018
                                'MessageBox.Show("The checkweigher is not responding.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                TestFailed()
                                'Me.Refresh()
                                MessageBox.Show(strResponse, "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                ' WO#17432 ADD Stop – AT 10/23/2018

                            End If
                            .TCPDisconnect()
                        End With
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error in LoadDataFromCheckWeigher" & vbCrLf & ex.Message)
        Finally
            'close the form and end the thread
            frmSplash.Close()
            If thdShowSplash.IsAlive Then
                thdShowSplash.Abort()
            End If
        End Try
    End Sub

    Private Sub ReceivedFormat2100Data(strResponse As String)
        Dim decTargetWgt As Decimal
        Dim strUnit As String = String.Empty
        Dim strActualWeight As String
        Dim decValue As Decimal

        Decimal.TryParse(strResponse.Substring(0, 6), decTargetWgt)
        'lblTargetWgt.Text = decTargetWgt.ToString
        'lblMaxWgt.Text = (decTargetWgt + Convert.ToDecimal(strResponse.Substring(8, 6))).ToString
        'lblMinWgt.Text = (decTargetWgt + Convert.ToDecimal(strResponse.Substring(15, 6))).ToString
        'lblTareWgt.Text = Convert.ToDecimal(strResponse.Substring(43, 6)).ToString

        Decimal.TryParse(strResponse.Substring(0, 6), decTargetWgt)
        lblTareWgt.Text = Convert.ToDecimal(strResponse.Substring(43, 6)).ToString

        ' WO#17432 ADD Start – AT 10/05/2018
        strUnit = strResponse.Substring(39, 3).TrimEnd(" ", "")
        strActualWeight = strResponse.Substring(32, 6)
        decActualWeight = Convert.ToDecimal(strActualWeight).ToString
        lblActualWgt.Text = Convert.ToDecimal(strActualWeight).ToString & " " & strUnit.Substring(0, 3).TrimEnd(".", "")
        ' lblActualWgt.Text = Convert.ToDecimal(strResponse.Substring(32, 6)).ToString 
        ' WO#17432 ADD Stop – AT 10/05/2018
        If Decimal.TryParse(decActualWeight, decValue) Then
            If IsWeightPass(decActualWeight) Then
                TestFailed()
            Else
                TestPassed()
            End If
        Else
            TestFailed()
        End If
    End Sub

    Private Sub ReceivedXSFormat3Data(strResponse As String)
        lblRecipeName.Text = strResponse.Substring(0, 10).Trim
    End Sub

    Private Sub ReceivedXCFormat3Data(strResponse As String)
        Dim strActualWeight As String
        Dim decValue As Decimal

        Try
            lblRecipeName.Text = strResponse.Substring(4, 6).Trim
            strActualWeight = strResponse.Substring(10, 7)
            decActualWeight = Convert.ToDecimal(strActualWeight).ToString
            lblActualWgt.Text = Convert.ToDecimal(strActualWeight).ToString & " " & strResponse.Substring(17, 3).TrimEnd(" ", "")
            If Decimal.TryParse(decActualWeight, decValue) Then
                If IsWeightPass(decActualWeight) Then
                    TestFailed()
                Else
                    TestPassed()
                End If
            Else
                TestFailed()
            End If

        Catch ex As Exception
            Throw New Exception("Error in ReceivedXCFormat3Data" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Function IsWeightPass(ByVal decActualWeight As Single) As Boolean
        Try
            If decActualWeight < Convert.ToDecimal(lblMinWgt.Text) Or decActualWeight > Convert.ToDecimal(lblMaxWgt.Text) Then
                Return True
            Else
                Return False
            End If
            Return False
        Catch ex As Exception
            Throw New Exception("Error in IsWeightPass" & vbCrLf & ex.Message)
        End Try
    End Function

    Private Sub TestFailed()
        btnFail.Visible = True
        btnPass.Visible = True
        btnRetest.Visible = True
        btnOverride.Visible = drwf.AllowOverride
        If decActualWeight <> 0 Then
            btnRetest.Text = "Test After RCA"
        Else
            btnRetest.Text = "Retry"
        End If
        lblActualWgt.ForeColor = Color.Red
        'close the form and end the thread
        frmSplash.Close()
        If thdShowSplash.IsAlive Then
            thdShowSplash.Abort()
        End If
        Me.Activate()
    End Sub

    Private Sub TestPassed()
        btnFail.Visible = True
        btnPass.Visible = True
        btnOverride.Visible = False
        btnRetest.Visible = False
        lblActualWgt.ForeColor = Color.White
    End Sub

    ' WO#17432 ADD Start – AT 11/29/2018
    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub

    Private Sub lblMaxWgt_Click(sender As Object, e As EventArgs) Handles lblMaxWgt.Click

    End Sub
    ' WO#17432 ADD Stop – AT 11/29/2018


End Class
