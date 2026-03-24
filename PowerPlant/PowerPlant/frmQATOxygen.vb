Public Class frmQATOxygen

    Delegate Sub SendDataCallBack(strString As String)
    Dim strErrMsg As String
    Dim intReadingCount As Int16
    Dim strReceivedData As String = String.Empty
    Dim decMinOxygen As Decimal      'MH added this variable for upper limit of oxygen entry
    Dim decMaxOxygen As Decimal      'MH added this variable for lowr limit of Oxygen entry
    Dim dteTestStartTime As DateTime
    Dim intRetestNo As Int16
    Dim blnTempTestResult As Boolean
    Dim blnFinalTestResult As Boolean
    Dim decActualOxy As Decimal
    Dim blnLastSample As Integer
    Dim intLaneNo As Integer
    Dim blnTestResult As Integer
    Dim strByPassLanes As String
    Dim intLastSampleNo As Int16
    Dim strCurrQATEntryPoint As String
    Dim blnByPassTest As Boolean = False
    Dim blnGimaNonStartup12Samples = False
    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Const cstrOxygenTextBoxPrefix As String = "txtOxy_"
    Const cstrOxygenLabelPrefix As String = "lblOxy_"
    Const cstrSkip As String = "Skip"
    Const cstrNA As String = "N/A"
    ' WO#17432 ADD Start – AT 10/10/2018
    Const cstrCO2 As String = "CO2 "
    Const cstrO2 As String = "O2 "
    ' WO#17432 ADD Stop – AT 10/10/2018
    ' Dim dteOriginalOverrideID As DateTime

    Private Enum TestResult
        Failed = 0
        Passsed = 1
        NA = 2
    End Enum
    Private Sub frmQATOxygen_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            'Is Start-Up or In-Process test?
            strCurrQATEntryPoint = SharedFunctions.FindCurrQATEntryPoint(gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

            'Retrieve QAT work flow and test information
            drWF = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strCurrQATEntryPoint, Me.Name)

            If Not IsNothing(drWF) Then
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

                'Get oxygen information of the item and the oxygen spec.

                If drWF.TestSpecID <> 0 Then
                    Using daSpec As New dsQATSpecTableAdapters.CPPsp_QATSpec_SelTableAdapter
                        Using dtSpec As New dsQATSpec.CPPsp_QATSpec_SelDataTable
                            daSpec.Fill(dtSpec, gdrSessCtl.Facility, drWF.TestSpecID, True)
                            If dtSpec.Count = 1 Then

                                decMinOxygen = dtSpec(0).LwLmtFromTarget
                                decMaxOxygen = dtSpec(0).UpLmtFromTarget
                                lblMaxOxygen.Text = String.Format("Max: {0:P}", decMaxOxygen)
                                lblMinOxygen.Text = String.Format("Min: {0:P}", decMinOxygen)
                            Else
                                MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                CloseForm()         ' WO#17432 – AT 11/29/2018
                                Exit Sub
                            End If
                        End Using
                    End Using
                Else
                    MessageBox.Show("The oxygen spec. is not setup in the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CloseForm()         ' WO#17432 – AT 11/29/
                    Exit Sub
                End If

                ' WO#17432 ADD Start – AT 11/19/2018
                'Initiaize the form to display different no. of test boxes for the different tests.
                If gdrCmpCfg.PkgLineType = "Gima" And drWF.QATEntryPoint <> "S" And drWF.NoOfSamples = 12 Then
                    blnGimaNonStartup12Samples = True
                    InitializeFormGima()
                Else
                    InitializeForm()
                End If
                ' WO#17432 ADD Stop – AT 11/19/2018

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
                                        MessageBox.Show("Can not connect to the Oxygen tester! You may click on the Current text box to enter the sample readings after contact supvervior.", _
                                                         "Warning", MessageBoxButtons.OK)
                                    End Try
                                End If
                            End With
                        End If
                    End Using
                End Using

                'initialize test start time
                dteTestStartTime = Now()

                'Calculate maximium no. of  test samples.
                ' WO#17432 ADD Start – AT 11/19/2018
                If blnGimaNonStartup12Samples = True Then
                    intLastSampleNo = CalculateLastSampleNoGima(strByPassLanes)
                Else
                    intLastSampleNo = CalculateLastSampleNo(strByPassLanes)
                End If
                ' WO#17432 ADD Stop – AT 11/19/2018

                'initialize the sampling counter
                intReadingCount = 1

                'If the test is allowed Override, then display the button.
                btnOverride.Visible = drWF.AllowOverride

                'Get the test Batch ID
                gdteTestBatchID = SharedFunctions.GetQATBatchID(drWF.TestSeq, strCurrQATEntryPoint)
                ' WO#17432 ADD Start – AT 12/03/2018
                If SharedFunctions.QATIsTested(drWF.FormName, gdteTestBatchID) = True Then
                    MsgBox("The test has already done in the same QAT workflow batch.", MsgBoxStyle.Exclamation)
                    CloseForm()
                    Exit Sub
                End If
                ' WO#17432 ADD Stop – AT 12/03/2018

                frmQATTester.ShowDialog()
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

    Private Sub frmDataFmSerialPort_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim buffer() As Byte
        Try
            'Waiting for all the data to be received from serial port.
            Threading.Thread.Sleep(1000)

            Const intBufferSize As Int16 = 100
            ReDim buffer(intBufferSize - 1)
            SerialPort1.Read(buffer, 0, intBufferSize)

            'Debug.Print(System.Text.Encoding.ASCII.GetString(buffer))
            strReceivedData = GetO2Data(System.Text.Encoding.ASCII.GetString(buffer))
            ReceivedData(strReceivedData)
            strReceivedData = String.Empty

        Catch ex As TimeoutException
            MessageBox.Show("Please try to record the test data again.", "Data could not be read within the desired time.", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' WO#17432 ADD Start – AT 10/10/2018
    Private Function GetO2Data(ByVal strReceivedData As String) As String
        'Mocon Pac Check model 450 and 650 data format
        'St = "O2 1.146 CO2 98.9" & vbCrLf
        'St = "O2 87.854 CO2 12.1"
        'St = "O2 100.000 CO2 0.0"
        'St = "O2 0.000 CO2 100.0" & vbCrLf
        'Mocon model CheckMate 2 and 3 data format
        'Frist output field is for O2 % in dec(7,4). Data looks like this.
        '034,8365;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
        Dim strReturnValue As String = String.Empty
        Dim intFromPos As Integer
        Dim intToPos As Integer
        Try
            'Mocon model 450 and 650 data format
            If strReceivedData.IndexOf(cstrO2) <> -1 Then
                intFromPos = strReceivedData.IndexOf(cstrO2) + cstrO2.Length
                intToPos = strReceivedData.LastIndexOf(" " & cstrCO2)
                If intToPos - intFromPos > 0 Then
                    strReturnValue = strReceivedData.Substring(intFromPos, intToPos - intFromPos)
                End If
            ElseIf strReceivedData.IndexOf(";") <> -1 Then
                'Mocon model CheckMate 2 and 3 data format
                intToPos = strReceivedData.IndexOf(";")
                If intToPos > 0 Then
                    strReturnValue = strReceivedData.Substring(0, intToPos - 1).Replace(",", ".")
                End If
            End If
            Return strReturnValue
        Catch ex As Exception
            Throw New Exception("Error in GetO2Data" & vbCrLf & ex.Message)
        End Try
    End Function
    ' WO#17432 ADD Stop – AT 10/10/2018

    Private Sub ReceivedData(ByVal strData As String)
        Try
            'The InvokeRequired will be true when this routine is run first time after receive data. 
            'So, it invokes the same subroutine back to update the UI control on the primary thread.
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
                    ' WO#17432 ADD Start – AT 11/19/2018
                    If blnGimaNonStartup12Samples = True Then
                        UpdateOxygenDataGima(txtCurrentReading.Text)
                    Else
                        UpdateOxygenData(txtCurrentReading.Text)
                    End If
                    ' WO#17432 ADD Stop – AT 11/19/2018
                    txtCurrentReading.Text = ""
                End If
                End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub txtCurrentReading_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtCurrentReading.Validating
        If sender.text <> "" Then
            Try
                If VerifyReadingData(sender.text) = False Then
                    e.Cancel = True
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Function VerifyReadingData(strReadingData As String) As Boolean

        Dim decData As Decimal
        If strReadingData <> "" Then
            Try
                If Not Decimal.TryParse(strReadingData, decData) Then
                    MessageBox.Show("The data is not valid.", "Invalid data format", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                ElseIf decData < 0 Then
                    MessageBox.Show("The data can not be less than zero.", "Error - Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                ElseIf intReadingCount > intLastSampleNo Then
                    MessageBox.Show("The number of collected sample data is more than required. ", "Error - Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Throw New Exception("Error in VerifyReadingData" & vbCrLf & ex.Message)
            End Try
        End If
    End Function

    Private Sub btnRetest_Click(sender As Object, e As EventArgs) Handles btnRetest.Click
        Dim lbl As Label

        Const intLblWidth As Int16 = 150, intLblHeight As Int32 = 27
        Const intIntialLblX As Int16 = 27, intIntialLblY As Int16 = 510
        Const cstrLabelName As String = "lblRetestNo"

        Try
            'If there was no testing in the last test, do not increase the test no.
            'If intReadingCount > 1 Then
            '    intRetestNo = intRetestNo + 1
            'End If

            'initialize test start time
            dteTestStartTime = Now()

            ' WO#17432 ADD Start – AT 10/02/2018
            If drWF.NoOfLanes > 0 Then
                strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            End If
            ' WO#17432 ADD Start – AT 10/02/2018
            For Each ctrl As Control In Controls
                If TypeOf ctrl Is TextBox AndAlso ctrl.Name.Length > cstrOxygenTextBoxPrefix.Length _
                    AndAlso ctrl.Name.Substring(0, 7) = cstrOxygenTextBoxPrefix Then

                    ctrl.Text = ""
                    ctrl.BackColor = Color.Black
                    ctrl.ForeColor = Color.White

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
                End If
                If TypeOf ctrl Is Label AndAlso ctrl.Name = cstrLabelName Then
                    Me.Controls.Remove(ctrl)
                End If
            Next
            blnTempTestResult = True
            blnFinalTestResult = False
            btnRetest.Visible = False
            intReadingCount = 1
            lblTestResault.Text = cstrNA
            txtCurrentReading.Text = ""

            'Create a label to display the ReTest no.
            If intRetestNo > 0 Then
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
            ' WO#17432 ADD Start – AT 11/19/2018
            'Calculate maximium no. of  test samples.
            If blnGimaNonStartup12Samples = True Then
                intLastSampleNo = CalculateLastSampleNoGima(strByPassLanes)
            Else
                intLastSampleNo = CalculateLastSampleNo(strByPassLanes)
            End If
            ' WO#17432 ADD Stop – AT 11/19/2018

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnOverride_Click(sender As Object, e As EventArgs) Handles btnOverride.Click
        Dim drScreen As DialogResult
        Dim intFinalTestResult As Int16

        Try
            'Create the values of the parameters for the form before calling it.
            Dim dteOverrideID As DateTime = Now()
            With frmQATOverrideLogOn
                .BatchID = gdteTestBatchID
                .QATDefnID = drWF.QATDefnID
                .QATEntryPoint = strCurrQATEntryPoint
                .OverrideID = dteOverrideID
                .TestTitle = UcHeading1.ScreenTitle
                .Alert = drWF.Alert
                drScreen = .ShowDialog(Me)
            End With

            'Update the override id on the header record
            '"Accept" button was pressed in the Override Screen to accept the test was failed.
            If drScreen = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(dteOverrideID) AndAlso IsDate(dteOverrideID) Then
                    If intRetestNo > 0 Then
                        intRetestNo = intRetestNo - 1
                    End If

                    If blnFinalTestResult = True Then
                        intFinalTestResult = TestResult.Passsed
                    Else
                        intFinalTestResult = TestResult.Failed
                    End If
                    SharedFunctions.SaveQATOxygen( _
                             gdteTestBatchID, _
                             gdrSessCtl.Facility, _
                             gstrInterfaceID, _
                             decMaxOxygen, _
                             dteOverrideID, _
                             gdrSessCtl.ShopOrder, _
                             gdrSessCtl.StartTime, _
                             gdrSessCtl.DefaultPkgLine, _
                             intRetestNo, _
                             Now(), _
                             intFinalTestResult, _
                             dteTestStartTime, _
                             gstrQATTesterID, _
                             strCurrQATEntryPoint, _
                             decActualOxy, _
                             blnLastSample, _
                             intLaneNo, _
                             Nothing, _
                             blnTestResult, _
                             Nothing
                             )
                    ' WO#17432 ADD Start – AT 10/02/2018
                    'Update QAT processing status
                    SharedFunctions.UpdateQATStatus(False, blnByPassTest, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, _
                                                 drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                    Me.Close()
                    ' WO#17432 ADD Stop – AT 10/02/2018
                End If
            ElseIf drScreen = Windows.Forms.DialogResult.Yes Then
                '"Accept" button was pressed in the Override Screen and the bypass lanes have been changed
                btnRetest_Click(sender, e)
            ElseIf drScreen = Windows.Forms.DialogResult.Cancel Then
                '"ByPass Test" button was pressed in the Override Screen
                blnByPassTest = True
                ' WO#17432 ADD Start – AT 10/02/2018
                If Not IsNothing(dteOverrideID) AndAlso IsDate(dteOverrideID) Then
                    SharedFunctions.SaveQATOxygen( _
                             gdteTestBatchID, _
                             gdrSessCtl.Facility, _
                             gstrInterfaceID, _
                             decMaxOxygen, _
                             dteOverrideID, _
                             gdrSessCtl.ShopOrder, _
                             gdrSessCtl.StartTime, _
                             gdrSessCtl.DefaultPkgLine, _
                             intRetestNo, _
                             Now(), _
                             blnFinalTestResult, _
                             dteTestStartTime, _
                             gstrQATTesterID, _
                             strCurrQATEntryPoint, _
                             decActualOxy, _
                             blnLastSample, _
                             intLaneNo, _
                             Nothing, _
                             blnTestResult, _
                             Nothing
                             )
                End If
                'Update QAT processing status
                SharedFunctions.UpdateQATStatus(False, blnByPassTest, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, _
                                                 drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                Me.Close()
                ' WO#17432 ADD Stop – AT 10/02/2018
            ElseIf drScreen = Windows.Forms.DialogResult.Ignore Then
                Exit Sub
            End If

            ' WO#17432 ADD Start – AT 10/01/2018
            '' Update QAT processing status
            ' SharedFunctions.UpdateQATStatus(False, blnByPassTest, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, _
            '                                 drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)

            ' Me.Close()
            ' WO#17432 ADD Stop – AT 10/01/2018
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
        End Try
    End Sub

    ' WO#17432 ADD Start – AT 11/19/2018
    Private Sub InitializeFormGima()
        '
        ' Assumption: Maximum no. of samples is 36,
        '             Maximum no. of lanes is 6,
        '             no. of lanes if specified can not be less than no. of samples.
        '
        ' This is only for Gima lines with 12 samples and run on not start up test.
        ' It will show the sample nos. as below
        '   1       7       13      19      25      31
        '   6      12       18      24      30      36
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

            ' WO#17432 ADD Start – AT 10/02/2018
            'Retrieve the last supervisor override information of the shop order in the packaging line.
            strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            ' WO#17432 ADD Stop – AT 10/02/2018

            'intNoOfLanes = drWF.NoOfLanes
            'intNoOfSamples = drWF.NoOfSamples
            intNoOfLanes = 6
            intNoOfSamples = 36
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

            Dim intFirstRowNo As Integer = 0
            Dim intLastRowNo As Integer = 0
            Dim intNewRowNo As Integer = 0
            intFirstRowNo = 1
            intLastRowNo = intMaxRows - 2

            If intNoOfLanes <= intMaxRows Then
                intSampleNo = 1
                For intColNo = 0 To intMaxCols - 1
                    For intRowNo = 0 To intNoOfRows - 1 Step 1
                        If intRowNo < intFirstRowNo Or intRowNo > intLastRowNo Then
                            If intRowNo > intLastRowNo Then
                                intNewRowNo = 1
                                intLaneNo = 6
                            Else
                                intNewRowNo = intRowNo
                                intLaneNo = 1
                            End If
                            'Create label controls for samples
                            lbl = New Label
                            With lbl
                                intLblX = intIntialLblX + intColNo * intLabelXGap
                                intlblY = intIntialLblY + intNewRowNo * intLabelYGap
                                .Location = New System.Drawing.Point(intLblX, intlblY)
                                .Size = New System.Drawing.Size(intLblWidth, intLblHeight)
                                .Text = Format(intSampleNo, "00")
                                .Name = cstrOxygenLabelPrefix & (intSampleNo).ToString
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
                                intBoxY = intIntialBoxY + intNewRowNo * intBoxYGap
                                .Location = New System.Drawing.Point(intBoxX, intBoxY)
                                .Text = String.Empty
                                .Size = New System.Drawing.Size(intBoxWidth, intBoxHeight)
                                .Name = cstrOxygenTextBoxPrefix & (intSampleNo).ToString
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
                        End If
                        intSampleNo = intSampleNo + 1
                        If intSampleNo > intNoOfSamples Then
                            Exit Sub
                        End If
                    Next
                Next

            End If

        Catch ex As Exception
            Throw New Exception("Error in InitializeFormGima" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub UpdateOxygenDataGima(strReadingData)
        Dim decActualOxy As Decimal
        Dim blnTestResult As Boolean
        Dim blnLastSample As Boolean
        Dim intLaneNo As Int16
        Dim blnFound As Boolean
        Dim intFinalTestResult As Int16

        Try
            'Get current lane no. of the sample only if the lane no. is defined in test definition

            If drWF.NoOfLanes <> 0 Then
                Do
                    'Assume the initial value of intReadingCount is 1
                    intLaneNo = GetNewLaneNoGima(intReadingCount)
                    ' intLaneNo = (intReadingCount Mod drWF.NoOfLanes)
                    ' If intLaneNo = 0 Then
                    ' intLaneNo = drWF.NoOfLanes
                    ' ElseIf intLaneNo = 1 Then
                    ' intLaneNo = 1
                    ' End If

                    'check the overrode lane nos.
                    'If the lane is overridden for no testing, the current test sample is assumed for the next lane. 
                    If strByPassLanes <> String.Empty Then
                        If strByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                            intReadingCount = intReadingCount + GetReadingCountByPassGima(intLaneNo)
                            'If intLaneNo = 1 Then
                            ' intReadingCount = intReadingCount + 5
                            ' intLaneNo = 6
                            'ElseIf intLaneNo = 6 Then
                            '   intReadingCount = intReadingCount + 1
                            '   intLaneNo = 1
                            'Else
                            'End If
                            ' intReadingCount = intReadingCount + 1
                            blnFound = True
                        Else
                            blnFound = False
                        End If
                    End If

                Loop Until strByPassLanes = String.Empty Or blnFound = False Or intReadingCount >= 36

            End If

            'check the current sample reading with the sepecification to provide test result
            decActualOxy = Math.Round(Decimal.Parse(strReadingData), 5)
            If decActualOxy > decMaxOxygen * 100 Or decActualOxy < decMinOxygen * 100 Then
                blnTestResult = False
                blnTempTestResult = False
                Controls.Item(cstrOxygenTextBoxPrefix & intReadingCount.ToString).BackColor = Color.Red
            Else
                blnTestResult = True
                If blnTempTestResult <> False Then
                    blnTempTestResult = True
                End If
            End If

            'Update the current sample reading value to the corresponding text box 
            Controls.Item(cstrOxygenTextBoxPrefix & intReadingCount.ToString).Text = decActualOxy

            'If the current sample is the last sample of the batch, finalized the final result of the whole batch.
            blnLastSample = False

            btnOverride.Visible = False
            If intReadingCount = intLastSampleNo Then
                btnOverride.Visible = drWF.AllowOverride
                blnLastSample = True
                blnFinalTestResult = blnTempTestResult
                If blnFinalTestResult Then
                    lblTestResault.Text = "Pass"
                    intFinalTestResult = TestResult.Passsed
                Else
                    lblTestResault.Text = "Fail"
                    intFinalTestResult = TestResult.Failed
                End If
            End If

            ' WO#17432 ADD Start – AT 11/23/2018
            intLaneNo = GetNewLaneNoGima(intReadingCount)
            ' WO#17432 ADD Start – AT 11/23/2018

            'insert the Oxygen information to table on DB server
            SharedFunctions.SaveQATOxygen( _
                             gdteTestBatchID, _
                             gdrSessCtl.Facility, _
                             gstrInterfaceID, _
                             decMaxOxygen, _
                             Nothing, _
                             gdrSessCtl.ShopOrder, _
                             gdrSessCtl.StartTime, _
                             gdrSessCtl.DefaultPkgLine, _
                             intRetestNo, _
                             Now(), _
                             intFinalTestResult, _
                             dteTestStartTime, _
                             gstrQATTesterID, _
                             strCurrQATEntryPoint, _
                             decActualOxy / 100.0, _
                             blnLastSample, _
                             intLaneNo, _
                             intReadingCount, _
                             blnTestResult, _
                             Now() _
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
                    Exit Sub
                Else
                    btnRetest.Visible = True
                    intRetestNo = intRetestNo + 1
                End If

            End If
            intReadingCount = intReadingCount + GetNewReadingCountGima(intReadingCount)

            '  intReadingCount = intReadingCount + 1

        Catch ex As Exception
            Throw New Exception("Error in UpdateOxygenDataGima" & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Function GetNewLaneNoGima(ByVal intReading As Integer) As Integer
        Dim intLaneNo As Integer = 0
        intLaneNo = (intReading Mod drWF.NoOfLanes)
        If intLaneNo = 0 Then
            Return drWF.NoOfLanes
        ElseIf intLaneNo = 1 Then
            Return 1
        End If
        Return 0
    End Function

    Private Function GetReadingCountByPassGima(ByVal intReading As Integer) As Integer
        If intReading = 1 Then
            Return 5
        ElseIf intReading = 6 Then
            Return 1
        Else
        End If
        Return 0
    End Function

    Private Function GetNewReadingCountGima(ByVal intReading As Integer) As Integer
        Dim intLaneNo As Integer = 0
        intLaneNo = (intReading Mod drWF.NoOfLanes)
        If intLaneNo = 0 Then
            Return 1
        ElseIf intLaneNo = 1 Then
            Return 5
        End If
        Return 0
    End Function

    Private Function CalculateLastSampleNoGima(strByPassLanes As String) As Integer
        'Calculate what is the last test sample number.
        Dim intSampleNo As Integer
        Dim intLaneNo As Int16
        Try
            '            intSampleNo = drWF.NoOfSamples
            intSampleNo = 36
            'if the samples are by lane, find out what is the last sanple no. with the skipped lane numbers in QAT Override table.
            If drWF.NoOfLanes <> 0 AndAlso strByPassLanes <> String.Empty Then
                'Start from from the last sample no to find out the lane no. of it and compare it with the skipped lane numbers.
                'The first sample no. with first unmatched with the skipped lane numbers is the last sample no.
                For i As Int16 = intSampleNo To 1 Step -5
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
            Throw New Exception("Error in CalculateLastSampleNoGima" & vbCrLf & ex.Message)
        End Try
    End Function
    ' WO#17432 ADD Stop – AT 11/19/2018

    Private Sub InitializeForm()
        '
        ' Assumption: Maximum no. of samples is 36,
        '             Maximum no. of lanes is 6,
        '             no. of lanes if specified can not be less than no. of samples.
        ' It will show the sample nos. as below
        '   1       7       13      19      25      31
        '   2       8       14      20      26      32
        '   3       .                               .
        '   4       .                               .
        '   5       .                               .
        '   6      12       18      24      30      36
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

            ' WO#17432 ADD Stop – AT 10/02/2018
            'Retrieve the last supervisor override information of the shop order in the packaging line.
            strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            'Dim drOvr As dsQATOverride.CPPsp_QATOverride_SelRow = SharedFunctions.GetLastQATOverrideByPassLanesRecord(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            'If IsNothing(drOvr) Then
            '    dteOriginalOverrideID = Nothing
            '    strByPassLanes = String.Empty
            'Else
            '    dteOriginalOverrideID = drOvr.OverrideID
            '    strByPassLanes = drOvr.ByPassLanes
            'End If

            ' WO#17432 ADD Stop – AT 10/02/2018

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
                            .Name = cstrOxygenLabelPrefix & (intSampleNo).ToString
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
                            .Name = cstrOxygenTextBoxPrefix & (intSampleNo).ToString
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
            Throw New Exception("Error in InitializeForm" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub UpdateOxygenData(strReadingData)
        Dim decActualOxy As Decimal
        Dim blnTestResult As Boolean
        Dim blnLastSample As Boolean
        Dim intLaneNo As Int16
        Dim blnFound As Boolean
        Dim intFinalTestResult As Int16

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

            'check the current sample reading with the sepecification to provide test result
            decActualOxy = Math.Round(Decimal.Parse(strReadingData), 5)
            If decActualOxy > decMaxOxygen * 100 Or decActualOxy < decMinOxygen * 100 Then
                blnTestResult = False
                blnTempTestResult = False
                Controls.Item(cstrOxygenTextBoxPrefix & intReadingCount.ToString).BackColor = Color.Red
            Else
                blnTestResult = True
                If blnTempTestResult <> False Then
                    blnTempTestResult = True
                End If
            End If

            'Update the current sample reading value to the corresponding text box 
            Controls.Item(cstrOxygenTextBoxPrefix & intReadingCount.ToString).Text = decActualOxy

            'If the current sample is the last sample of the batch, finalized the final result of the whole batch.
            blnLastSample = False

            btnOverride.Visible = False
            If intReadingCount = intLastSampleNo Then
                btnOverride.Visible = drWF.AllowOverride
                blnLastSample = True
                blnFinalTestResult = blnTempTestResult
                If blnFinalTestResult Then
                    lblTestResault.Text = "Pass"
                    intFinalTestResult = TestResult.Passsed
                Else
                    lblTestResault.Text = "Fail"
                    intFinalTestResult = TestResult.Failed
                End If
            End If

            'insert the Oxygen information to table on DB server
            SharedFunctions.SaveQATOxygen( _
                             gdteTestBatchID, _
                             gdrSessCtl.Facility, _
                             gstrInterfaceID, _
                             decMaxOxygen, _
                             Nothing, _
                             gdrSessCtl.ShopOrder, _
                             gdrSessCtl.StartTime, _
                             gdrSessCtl.DefaultPkgLine, _
                             intRetestNo, _
                             Now(), _
                             intFinalTestResult, _
                             dteTestStartTime, _
                             gstrQATTesterID, _
                             strCurrQATEntryPoint, _
                             decActualOxy / 100.0, _
                             blnLastSample, _
                             intLaneNo, _
                             intReadingCount, _
                             blnTestResult, _
                             Now() _
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
            Throw New Exception("Error in UpdateOxygenData" & vbCrLf & ex.Message)
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

    Private Sub btnNA_Click(sender As Object, e As EventArgs) Handles btnNA.Click
        Try
            SharedFunctions.SaveQATOxygen( _
                 gdteTestBatchID, _
                 gdrSessCtl.Facility, _
                 gstrInterfaceID, _
                 decMaxOxygen, _
                 Nothing, _
                 gdrSessCtl.ShopOrder, _
                 gdrSessCtl.StartTime, _
                 gdrSessCtl.DefaultPkgLine, _
                 intRetestNo, _
                 Now(), _
                 TestResult.NA, _
                 dteTestStartTime, _
                 gstrQATTesterID, _
                 strCurrQATEntryPoint, _
                 0.0, _
                 blnLastSample, _
                 intLaneNo, _
                 intReadingCount, _
                 blnTestResult, _
                 Nothing _
                )
            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub
End Class