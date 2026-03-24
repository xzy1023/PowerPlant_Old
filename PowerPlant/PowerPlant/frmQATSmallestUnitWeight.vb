Public Class frmQATSmallestUnitWeight

    Delegate Sub SendDataCallBack(strString As String)
    Dim strErrMsg As String
    Dim intReadingCount As Int16
    Dim decTargetWgt As Decimal
    Dim decLowerLimit As Decimal
    Dim decUpperLimit As Decimal
    Dim dteTestStartTime As DateTime
    Dim intRetestNo As Int16
    Dim strReceivedData As String = String.Empty
    Dim blnTempTestResult As Boolean
    Dim blnFinalTestResult As Boolean
    Dim strByPassLanes As String
    Dim intLastSampleNo As Int16
    Dim strCurrQATEntryPoint As String
    Dim blnByPassTest As Boolean = False
    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Const cstrWgtTextBoxPrefix As String = "txtWgt_"
    Const cstrSkip As String = "Skip"

    Private Sub frmQATSmallestUnitWeight_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            'Is Start-Up or In-Process test?
            strCurrQATEntryPoint = SharedFunctions.FindCurrQATEntryPoint(gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

            'Retrieve QAT work flow and test information
            drWF = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strCurrQATEntryPoint, Me.Name)

            If Not IsNothing(drWF) Then             ' WO#17432 – AT 11/29/2018
                'Set form title
                If drWF.TestFormTitle <> "" Then
                    UcHeading1.ScreenTitle = drWF.TestFormTitle
                    Me.Text = drWF.TestFormTitle
                End If

                'Retrieve Shop Order record using the Shop Order from the Session Control
                Using tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                    gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl.ShopOrder, "")
                    If tblSO.Count > 0 Then
                        drSO = tblSO(0)
                    Else
                        MessageBox.Show("Cannot find the shop order information. Please contact Supervisor.", "Error - Missing shop order inoformation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        CloseForm()
                        Exit Sub
                    End If
                End Using

                'Get weight information of the item and the weight spec.
                decTargetWgt = drSO.LabelWeight
                If drWF.TestSpecID <> 0 Then
                    Using daSpec As New dsQATSpecTableAdapters.CPPsp_QATSpec_SelTableAdapter
                        Using dtSpec As New dsQATSpec.CPPsp_QATSpec_SelDataTable
                            daSpec.Fill(dtSpec, gdrSessCtl.Facility, drWF.TestSpecID, True)
                            If dtSpec.Count = 1 Then
                                decTargetWgt = drSO.LabelWeight
                                decLowerLimit = dtSpec(0).LwLmtFromTarget + drSO.LabelWeight
                                decUpperLimit = dtSpec(0).UpLmtFromTarget + drSO.LabelWeight
                                lblTargetWgt.Text = String.Format("Target: {0}", decTargetWgt)
                                lblMaxWgt.Text = String.Format("Max: {0}", decUpperLimit)
                                lblMinWgt.Text = String.Format("Min: {0}", decLowerLimit)
                            Else
                                MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                CloseForm()
                                Exit Sub
                            End If
                        End Using
                    End Using
                Else
                    MessageBox.Show("The weight spec. is not setup in the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CloseForm()
                    Exit Sub
                End If

                'Initiaize the form to display different no. of test boxes for the different tests.
                InitializeTestResults()

                'Calculate maximium no. of  test samples.
                intLastSampleNo = CalculateLastSampleNo(strByPassLanes)

                'initialize the sampling counter
                intReadingCount = 1

                'If the test is allowed Override, then display the button.
                btnOverride.Visible = drWF.AllowOverride

                'Get the test Batch ID
                gdteTestBatchID = SharedFunctions.GetQATBatchID(drWF.TestSeq, strCurrQATEntryPoint)
                ' WO#17432 ADD Start – AT 12/03/2018
                If SharedFunctions.QATIsTested(drWF.FormName, gdteTestBatchID) = True Then
                    MsgBox("The test has already done in the same QAT workflow batch.")
                    CloseForm()
                    Exit Sub
                End If
                ' WO#17432 ADD Stop – AT 12/03/2018

                frmQATTester.ShowDialog()
                ' WO#17432 ADD Start – AT 11/29/2018

                'WO#17432 ADD Start – BL 2019/08/19
                If drWF.IsSerialConnIDNull Then
                    MessageBox.Show("Please define Serial connection parameters ASAP. You may click on the Current text box to enter the sample weight after contact supvervisor.", _
                                                         "Warning", MessageBoxButtons.OK)
                Else
                    Using daSerialConn As New dsSerialConnTableAdapters.CPPsp_QATSerialConn_SelTableAdapter
                        Using dtSerialConn As New dsSerialConn.CPPsp_QATSerialConn_SelDataTable
                            daSerialConn.Fill(dtSerialConn, gdrSessCtl.Facility, drWF.SerialConnID, Nothing)
                            If dtSerialConn.Count > 0 Then
                                With SerialPort1
                                    .PortName = "COM" & dtSerialConn(0).ComPort.ToString
                                    .BaudRate = dtSerialConn(0).BaudRate
                                    .DataBits = dtSerialConn(0).DataBits
                                    .Parity = dtSerialConn(0).Parity
                                    .StopBits = dtSerialConn(0).StopBits

                                    '.PortName = "COM5"  '.BaudRate = 1200   '.DataBits = 7 '.Parity = IO.Ports.Parity.Odd  '.StopBits = IO.Ports.StopBits.One

                                    .Encoding = System.Text.Encoding.GetEncoding(28591)
                                    .DtrEnable = True
                                    .RtsEnable = True
                                    .ReadTimeout = 2000

                                    If Not .IsOpen Then
                                        Try
                                            .Open()
                                        Catch ex As Exception
                                            MessageBox.Show("Can not connect to the scale. You may click on the Current text box to enter the sample weight after contact supvervisor.", _
                                                             "Warning", MessageBoxButtons.OK)
                                        End Try
                                    End If
                                End With
                            End If
                        End Using
                    End Using
                End If
                'WO#17432 ADD Start – BL 2019/08/19

            Else
                MessageBox.Show("Cannot find the QAT workflow information. Please contact Supervisor.", "Error - Missing workflow information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CloseForm()
                Exit Sub
            End If
            ' WO#17432 ADD Stop – AT 11/29/2018
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived

        Try

            'Waiting for all the data to be received from serial port.
            Threading.Thread.Sleep(800)
            'strReceivedData = SerialPort1.ReadExisting()

            Const intBufferSize As Int16 = 50
            Dim buffer(intBufferSize - 1) As Byte
            SerialPort1.Read(buffer, 0, intBufferSize)

            'Trim the necessary spaces and characters to result as 9.32g ...etc.
            strReceivedData = System.Text.RegularExpressions.Regex.Replace(System.Text.Encoding.ASCII.GetString(buffer), "[^0-9\-/./g]", "")

            ' WO#17432 DEL Start – AT 12/03/2018
            ' Debug.Print(strReceivedData)
            ' Debug.Print(strReceivedData.IndexOf("g"))
            ' WO#17432 DEL Stop – AT 12/03/2018
            If strReceivedData.IndexOf("g") > 0 Then
                'only pick the first reading before the character 'g'
                strReceivedData = strReceivedData.Substring(0, strReceivedData.IndexOf("g"))
                'strReceivedData = System.Text.RegularExpressions.Regex.Replace(System.Text.Encoding.ASCII.GetString(buffer), "[^0-9\-/.]", "")
                ReceivedData(strReceivedData)
                strReceivedData = String.Empty
            End If

        Catch ex As TimeoutException
            MessageBox.Show("Please try to record the test data again.", "Data could not be read within the desired time.", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmDataFmSerialPort_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ReceivedData(ByVal strData As String)
        Try
            'The InvokeRequired will be true when this routine is run first time after receive data. 
            'SO it invokes the same subroutine back to update the UI control on the primary thread.
            'That is when InvokeRequired is false in SendDataCallBack .
            If txtCurrentReading.InvokeRequired Then
                txtCurrentReading.Invoke(New SendDataCallBack(AddressOf ReceivedData), strData)
            Else
                txtCurrentReading.Text = strData
            End If
        Catch ex As Exception
            Throw New Exception("Error in ReceivedData" & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtCurrentReading.MouseDown
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

    Private Sub txtCurrentReading_TextChanged(sender As Object, e As EventArgs) Handles txtCurrentReading.TextChanged
        Try
            If txtCurrentReading.Text <> "" Then
                If VerifyReadingData(txtCurrentReading.Text) = True Then
                    UpdateWeightData(txtCurrentReading.Text)
                    txtCurrentReading.Text = ""
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub txtCurrentReading_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtCurrentReading.Validating
        If sender.text <> "" And ActiveControl.Name <> "btnRetest" Then
            Try
                If VerifyReadingData(sender.text) = False Then
                    e.Cancel = True
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnOverride_Click(sender As Object, e As EventArgs) Handles btnOverride.Click
        Dim dteOverrideID As DateTime
        Dim drScreen As DialogResult
        Try
            'Create the values of the parameters for the form before calling it.
            dteOverrideID = Now()
            With frmQATOverrideLogOn
                .BatchID = gdteTestBatchID
                .OverrideID = dteOverrideID
                .QATDefnID = drWF.QATDefnID
                .QATEntryPoint = strCurrQATEntryPoint
                .TestTitle = UcHeading1.ScreenTitle
                .Alert = drWF.Alert
                drScreen = .ShowDialog(Me)
            End With

            '"Accept" button was pressed in the Override Screen
            If drScreen = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(dteOverrideID) AndAlso IsDate(dteOverrideID) Then
                    If intRetestNo > 0 Then
                        intRetestNo = intRetestNo - 1
                    End If
                    ' WO#17432 ADD Start – AT 11/23/2018
                    decUpperLimit = 0
                    decLowerLimit = 0
                    decTargetWgt = 0
                    ' WO#17432 ADD Start – AT 12/03/2018
                    blnFinalTestResult = 0
                    dteTestStartTime = Now

                    'Insert a header record
                    SharedFunctions.SaveQATSmallestSalesUnitWeight( _
                        gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, decUpperLimit, decLowerLimit, _
                        dteOverrideID, gdrSessCtl.DefaultPkgLine, intRetestNo, gdrSessCtl.ShopOrder, gdrSessCtl.StartTime, Nothing, decTargetWgt, _
                        Now(), blnFinalTestResult, dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                        Nothing, Nothing, Nothing, Nothing, Nothing, Nothing _
                         )

                    'Update QAT processing status
                    SharedFunctions.UpdateQATStatus(False, blnByPassTest, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, _
                                                    drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                    Me.Close()

                    ' WO#17432 DEL – AT 10/03/2018  btnRetest_Click(sender, e)  
                End If
            ElseIf drScreen = Windows.Forms.DialogResult.Yes Then
                '"Accept" button was pressed in the Override Screen and the bypass lanes have been changed
                btnRetest_Click(sender, e)
                ' WO#17432 ADD Stop – AT 10/03/2018
            ElseIf drScreen = Windows.Forms.DialogResult.Cancel Then
                '"ByPass Test" button was pressed in the Override Screen
                blnByPassTest = True

                ' WO#17432 ADD Start – AT 11/23/2018
                decUpperLimit = 0
                decLowerLimit = 0
                decTargetWgt = 0
                If intRetestNo > 0 Then
                    intRetestNo = intRetestNo - 1
                End If
                blnFinalTestResult = 0
                dteTestStartTime = Now

                'Insert a header record
                SharedFunctions.SaveQATSmallestSalesUnitWeight( _
                   gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, decUpperLimit, decLowerLimit, _
                   dteOverrideID, gdrSessCtl.DefaultPkgLine, intRetestNo, gdrSessCtl.ShopOrder, gdrSessCtl.StartTime, Nothing, decTargetWgt, _
                   Now(), blnFinalTestResult, dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                   Nothing, Nothing, Nothing, Nothing, Nothing, Nothing _
                    )

                'Update QAT processing status
                SharedFunctions.UpdateQATStatus(False, blnByPassTest, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, _
                                                drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                Me.Close()
            ElseIf drScreen = Windows.Forms.DialogResult.Ignore Then
                '"Previous" button was pressed in the Override Screen
                Exit Sub
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnRetest_Click(sender As Object, e As EventArgs) Handles btnRetest.Click
        Dim lbl As Label
        Dim intLaneNo As Int16

        Const intLblWidth As Int16 = 150, intLblHeight As Int32 = 27
        Const intIntialLblX As Int16 = 27, intIntialLblY As Int16 = 510
        Const strLabelName As String = "lblRetestNo"

        Try
            'If there was no testing in the last test, do not increase the test no.
            'If intReadingCount > 1 Then
            '    intRetestNo = intRetestNo + 1
            'End If

            'If the test is allowed Override, then display the button.
            btnOverride.Visible = drWF.AllowOverride

            dteTestStartTime = Now()

            If drWF.NoOfLanes > 0 Then
                strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            End If

            For Each ctrl As Control In Controls
                If TypeOf ctrl Is TextBox AndAlso ctrl.Name.Length > cstrWgtTextBoxPrefix.Length AndAlso _
                    ctrl.Name.Substring(0, 7) = cstrWgtTextBoxPrefix Then
                    ctrl.Text = ""
                    ctrl.ForeColor = Color.White
                    ctrl.BackColor = Color.Black

                    If strByPassLanes <> String.Empty Then
                        intLaneNo = (CInt(ctrl.Tag) Mod drWF.NoOfLanes)
                        If intLaneNo = 0 Then
                            intLaneNo = drWF.NoOfLanes
                        End If

                        If strByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                            ctrl.Text = cstrSkip
                            ctrl.ForeColor = Color.Gold
                        End If
                    End If

                Else
                    If TypeOf ctrl Is Label AndAlso ctrl.Name = strLabelName Then
                        Me.Controls.Remove(ctrl)
                    End If
                End If
            Next
            blnTempTestResult = True
            blnFinalTestResult = False
            btnRetest.Visible = False
            intReadingCount = 1
            lblTestResault.Text = "N/A"
            txtCurrentReading.Text = ""

            'Create a label to display the ReTest no.
            If intRetestNo > 0 Then
                lbl = New Label
                With lbl
                    .Location = New System.Drawing.Point(intIntialLblX, intIntialLblY)
                    .Size = New System.Drawing.Size(intLblWidth, intLblHeight)
                    .Text = String.Format("Retest No: {0}", intRetestNo)
                    .Name = strLabelName
                    .ForeColor = Color.White
                    .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                End With
                Me.Controls.Add(lbl)
            End If

            'Calculate maximium no. of  test samples.
            intLastSampleNo = CalculateLastSampleNo(strByPassLanes)

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try

    End Sub

    Private Function VerifyReadingData(strReadingData As String) As Boolean

        Dim decData As Decimal

        Try
            If strReadingData <> "" Then
                If Not Decimal.TryParse(strReadingData, decData) Then
                    MessageBox.Show("The data is not valid.", "Invalid data format", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                ElseIf decData < 0 Then
                    MessageBox.Show("The data can not be less than zero.", "Error - Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                ElseIf intReadingCount > intLastSampleNo Then
                    MessageBox.Show("The number of collected sample data is more than required. ", "Error - Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtCurrentReading.Text = String.Empty
                    Return False
                Else
                    Return True
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in VerifyReadingData" & vbCrLf & ex.Message)
        End Try

    End Function

    Private Sub UpdateWeightData(strReadingData)
        Dim decActualWgt As Decimal
        Dim blnTestResult As Boolean
        Dim blnLastSample As Boolean
        Dim intLaneNo As Int16
        Dim blnFound As Boolean

        Try
            'Get current lane no. of the sample only if the lane no. is defined in test definition
            If drWF.NoOfLanes <> 0 Then
                Do
                    'Assume the initial value of intReadingCount is 1
                    intLaneNo = (intReadingCount Mod drWF.NoOfLanes)
                    If intLaneNo = 0 Then
                        intLaneNo = drWF.NoOfLanes
                    End If

                    'check the overrode lane nos.
                    'If the lane is overridden for no testing, the current test sample is assumed for the next lane. 
                    If strByPassLanes <> String.Empty Then
                        If strByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                            intReadingCount = intReadingCount + 1
                            blnFound = True
                        Else
                            blnFound = False
                        End If
                    End If

                Loop Until strByPassLanes = String.Empty Or blnFound = False Or intReadingCount >= drWF.NoOfSamples
            End If

            'check the sample weight with the weight sepecification to provide test result
            decActualWgt = Math.Round(Decimal.Parse(strReadingData), 1)
            If decActualWgt > decUpperLimit Or decActualWgt < decLowerLimit Then
                blnTestResult = False
                blnTempTestResult = False
                Controls.Item(cstrWgtTextBoxPrefix & intReadingCount.ToString).BackColor = Color.Red
            Else
                blnTestResult = True
                If blnTempTestResult <> False Then
                    blnTempTestResult = True
                End If
            End If

            'Update the sample weight value to the corresponding text box 
            Controls.Item(cstrWgtTextBoxPrefix & intReadingCount.ToString).Text = decActualWgt.ToString

            'If the current sample is the last sample of the batch, finalized the final result of the whole batch.
            blnLastSample = False

            btnOverride.Visible = False
            If intReadingCount = intLastSampleNo Then
                btnOverride.Visible = drWF.AllowOverride
                blnLastSample = True
                blnFinalTestResult = blnTempTestResult
                If blnFinalTestResult Then
                    lblTestResault.Text = "Pass"
                Else
                    lblTestResault.Text = "Fail"
                End If
            End If

            'insert the weight detail information to table on DB server
            SharedFunctions.SaveQATSmallestSalesUnitWeight( _
                            gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, decUpperLimit, decLowerLimit, Nothing, _
                            gdrSessCtl.DefaultPkgLine, intRetestNo, gdrSessCtl.ShopOrder, _
                            gdrSessCtl.StartTime, Nothing, decTargetWgt, Now(), blnFinalTestResult, _
                            dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                            decActualWgt, blnLastSample, intLaneNo, intReadingCount, blnTestResult, Now() _
                            )

            Me.Refresh()

            'If the final test result is pass then show the test result for 2 seconds and close the screen
            'else display the retest button.
            If blnLastSample = True Then
                SharedFunctions.UpdateQATStatus(False, blnByPassTest, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, _
                                                drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                If blnFinalTestResult = True Then
                    Threading.Thread.Sleep(2000)
                    Me.Close()
                Else
                    btnRetest.Visible = True
                    intRetestNo = intRetestNo + 1
                End If

            End If

            intReadingCount = intReadingCount + 1

        Catch ex As Exception
            Throw New Exception("Error in UpdateWeightData" & vbCrLf & ex.Message)
        End Try

    End Sub


    Private Sub InitializeTestResults()
        '
        ' Assumption: Maximum no. of samples is 36,
        '             Maximum no. of lanes is 6,
        '             no. of lanes if specified can not be less than no. of samples.
        '
        Dim lbls As Label()
        Dim txtBoxes As TextBox()
        Dim lbl As Label
        Dim txtBox As TextBox
        Dim intLblX As Int16, intlblY As Int16
        Dim intBoxX As Int16, intBoxY As Int16
        Dim intSampleNo As Int16, intRowNo As Int16, intColNo As Int16

        Const intLblWidth As Int16 = 38, intLblHeight As Int32 = 27
        Const intBoxWidth As Int16 = 90, intBoxHeight As Int32 = 35
        Const intIntialLblX As Int16 = 4, intIntialLblY As Int16 = 126
        Const intIntialBoxX As Int16 = 44, intIntialBoxY As Int16 = 123
        Const intLabelXGap As Int16 = 132, intLabelYGap As Int16 = 50
        Const intBoxXGap As Int16 = 133, intBoxYGap As Int16 = 50
        Const intMaxRows As Int16 = 6, intMaxCols As Int16 = 6

        Dim intNoOfLanes As Int16
        Dim intNoOfSamples As Int16
        Dim intNoOfRows As Int16

        Dim intLaneNo As Int16

        Try
            'Initialize test result fields.
            blnTempTestResult = True
            blnFinalTestResult = False

            'initialize test start time
            dteTestStartTime = Now()

            'Retrieve the last supervisor override information of the shop order in the packaging line.
            ' WO#17432 ADD Start – AT 10/02/2018
            strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            ' WO#17432 ADD Stop – AT 10/02/2018

            ' WO#17432 DEL Start – AT 10/02/2018
            'Retrieve the last supervisor override information of the shop order in the packaging line.
            'Using daOvr As New dsQATOverrideTableAdapters.CPPsp_QATOverride_SelTableAdapter
            '    Using dtOvr As New dsQATOverride.CPPsp_QATOverride_SelDataTable
            '        daOvr.Fill(dtOvr, gdrSessCtl.Facility, Nothing, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            '        If dtOvr.Count > 0 Then
            '            For Each drOvr As dsQATOverride.CPPsp_QATOverride_SelRow In dtOvr.Rows
            '                'The return rows are sorted in descending sequence of the record inserted sequence
            '                If drOvr.ByPassTest = False Then
            '                    strByPassLanes = drOvr.ByPassLanes
            '                    Exit For
            '                End If
            '            Next
            '        Else
            '            strByPassLanes = String.Empty
            '        End If
            '    End Using
            'End Using
            ' WO#17432 DEL Stop – AT 10/02/2018

            intNoOfLanes = drWF.NoOfLanes
            intNoOfSamples = drWF.NoOfSamples

            'Find out no. of rows of sample data will be displayed.
            If intNoOfSamples <= intMaxRows Then
                intNoOfRows = intNoOfSamples
            Else
                If intNoOfLanes = 0 Then
                    intNoOfRows = intMaxRows
                Else
                    intNoOfRows = intNoOfLanes
                End If
            End If

            ReDim txtBoxes(intNoOfSamples)
            ReDim lbls(intNoOfSamples)


            If intNoOfLanes <= intMaxRows Then
                intSampleNo = 1
                For intColNo = 0 To intMaxCols - 1
                    For intRowNo = 0 To intNoOfRows - 1 Step 1
                        'Create label controls for samples
                        lbl = New Label
                        With lbl
                            intLblX = intIntialLblX + intColNo * intLabelXGap
                            intlblY = intIntialLblY + intRowNo * intLabelYGap
                            .Location = New System.Drawing.Point(intLblX, intlblY)
                            .Size = New System.Drawing.Size(intLblWidth, intLblHeight)
                            .Text = Format(intSampleNo, "00")
                            .Name = "lblWgt_" & (intSampleNo).ToString
                            .ForeColor = Color.White
                            .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                            .Tag = (intSampleNo).ToString
                        End With
                        lbls(intSampleNo) = lbl
                        Me.Controls.Add(lbl)

                        'Create text box controls to hold sample data
                        txtBox = New TextBox
                        With txtBox
                            intBoxX = intIntialBoxX + intColNo * intBoxXGap
                            intBoxY = intIntialBoxY + intRowNo * intBoxYGap
                            .Location = New System.Drawing.Point(intBoxX, intBoxY)
                            .Text = String.Empty
                            .Size = New System.Drawing.Size(intBoxWidth, intBoxHeight)
                            .Name = cstrWgtTextBoxPrefix & (intSampleNo).ToString
                            .ForeColor = Color.White
                            .BackColor = Color.Black
                            .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                            .ReadOnly = True
                            .TabStop = False
                            .Tag = (intSampleNo).ToString
                            .BringToFront()

                            'indicate the lane is skipped for test if it is recorded in the Override table.
                            If strByPassLanes <> String.Empty Then
                                intLaneNo = (intSampleNo Mod drWF.NoOfLanes)
                                If intLaneNo = 0 Then
                                    intLaneNo = drWF.NoOfLanes
                                End If
                                If strByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                                    .Text = cstrSkip
                                    .ForeColor = Color.Gold
                                End If
                            End If
                        End With
                        txtBoxes(intSampleNo) = txtBox
                        Me.Controls.Add(txtBox)

                        intSampleNo = intSampleNo + 1
                        If intSampleNo > intNoOfSamples Then
                            Exit Sub
                        End If
                    Next
                Next

            End If


        Catch ex As Exception
            Throw New Exception("Error in InitializeTestResults" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Function CalculateLastSampleNo(strByPassLanes As String) As Integer
        'Calculate what is the last test sample number.
        Dim intSampleNo As Integer
        Dim intLaneNo As Int16
        Try
            intSampleNo = drWF.NoOfSamples
            'if the samples are by lane, find out what is the last sanple no. with the skipped lane numbers in QAT Override table.
            If drWF.NoOfLanes <> 0 AndAlso strByPassLanes <> String.Empty Then
                'Start from from the last sample no to find out the lane no. of it and compare it with the skipped lane numbers.
                'The first sample no. with first unmatched with the skipped lane numbers is the last sample no.
                For i As Int16 = drWF.NoOfSamples To 1 Step -1
                    intLaneNo = (i Mod drWF.NoOfLanes)
                    If intLaneNo = 0 Then
                        intLaneNo = drWF.NoOfLanes
                    End If
                    If strByPassLanes.IndexOf(intLaneNo & ",") < 0 Then
                        intSampleNo = i
                        Exit For
                    End If
                Next
            End If
            Return intSampleNo
        Catch ex As Exception
            Throw New Exception("Error in CalculateLastSampleNo" & vbCrLf & ex.Message)
        End Try
    End Function

    ' WO#17432 ADD Start – AT 11/29/2018
    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub
    ' WO#17432 ADD Stop – AT 11/29/2018
End Class