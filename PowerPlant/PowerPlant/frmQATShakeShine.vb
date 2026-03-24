Public Class frmQATShakeShine
    Dim strErrMsg As String
    Dim intReadingCount As Int16
    Dim decMinOxygen As Decimal         'MH added this variable for upper limit of oxygen entry
    Dim decMaxOxygen As Decimal         'MH added this variable for lowr limit of Oxygen entry
    Dim dteTestStartTime As DateTime
    Dim intRetestNo As Int16
    Dim strReceivedData As String = String.Empty
    Dim blnTempTestResult As Boolean
    Dim blnFinalTestResult As Boolean
    Dim decActualOxy As Decimal
    Dim blnLastSample As Integer
    Dim intLaneNo As Integer
    Dim blnTestResult As Integer
    Dim intLastSampleCount As Int16
    Dim strCurrQATEntryPoint As String
    Dim blnByPassTest As Boolean = False
    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow

    Const cstrPressureButtonPrefix As String = "btnPre_"
    Const cstrPressureLabelPrefix As String = "lblPressure_"
    Const cstrSkip As String = "Skip"
    Const cstrPass As String = "Pass"
    Const cstrFail As String = "Fail"
    Const cstrNA As String = "N/A"

    Dim _dbServer As New ServerModels.PowerPlantEntities()
    Dim resultServerDetails As New List(Of ServerModels.tblQATShakeShineResultDetail)()
    Dim _dbLocal As New LocalModels.PowerPlantLocalEntities()
    Dim resultLocalDetails As New List(Of LocalModels.tblQATShakeShineResultDetail)()

    Private Sub frmQATPressure_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strByPassLanes As String = String.Empty
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

                If drWF.TestSpecID <> 0 Then
                    Using daSpec As New dsQATSpecTableAdapters.CPPsp_QATSpec_SelTableAdapter
                        Using dtSpec As New dsQATSpec.CPPsp_QATSpec_SelDataTable
                            daSpec.Fill(dtSpec, gdrSessCtl.Facility, drWF.TestSpecID, True)
                        End Using
                    End Using
                    'Else
                    '    MessageBox.Show("The Pressure spec. is not setup in the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                If drWF.NoOfLanes <> 0 Then
                    strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
                End If
                'Initiaize the form to display different no. of test boxes for the different tests.
                InitializeTestResults(strByPassLanes)

                'Find what is the last sample no.
                intLastSampleCount = drWF.NoOfSamples
                'if the samples are by lane, find out what is the last sanple no. with the skipped lane numbers in QAT Override table.
                If drWF.NoOfLanes <> 0 AndAlso strByPassLanes <> String.Empty Then
                    For i As Int16 = drWF.NoOfLanes To 1 Step -1
                        If strByPassLanes.IndexOf(i.ToString & ",") >= 0 Then
                            intLastSampleCount = intLastSampleCount - 1
                        Else
                            Exit For
                        End If
                    Next
                End If

                'initialize test start time
                dteTestStartTime = Now()

                'Get the test Batch ID
                gdteTestBatchID = SharedFunctions.GetQATBatchID(drWF.TestSeq, strCurrQATEntryPoint)
                ' WO#17432 ADD Stop – AT 11/29/2018
                ' WO#17432 ADD Start – AT 12/03/2018
                If SharedFunctions.QATIsTested(drWF.FormName, gdteTestBatchID) = True Then
                    MsgBox("The test has already done in the same QAT workflow batch.")
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
            ' WO#17432 ADD Stop – AT 11/29/2018
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        SaveTestHeader(True)
        SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint,
                    drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
        Me.Close()
    End Sub

    Private Sub btnPressure_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button

        Try
            btn = CType(sender, Button)

            'If the button is "Pass", change it to strFail vice versa.
            If btn.BackColor = Color.LawnGreen Then
                btn.BackColor = Color.Red
                btn.Text = cstrFail
                btnRetest.Visible = True
                btnAccept.Visible = False
            ElseIf btn.BackColor = Color.Red Then
                btnRetest.Visible = False
                btnAccept.Visible = True
                btn.BackColor = Color.LawnGreen
                btn.Text = cstrPass
            End If

            btnAccept.Visible = isAllPass()
            btnRetest.Visible = Not btnAccept.Visible

            If btnAccept.Visible Then
                lblTestResult.Text = cstrPass
            Else
                lblTestResult.Text = cstrFail
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRetest_Click(sender As Object, e As EventArgs) Handles btnRetest.Click
        'Save all the detail test result and re-n=initialize the test result screen.
        Dim lbl As Label
        Dim strBypassLanes As String = String.Empty
        Const intLblWidth As Int16 = 150, intLblHeight As Int32 = 27
        Const intIntialLblX As Int16 = 189, intIntialLblY As Int16 = 510
        Const strRetestLabelName As String = "lblRetestNo"

        Try
            'Save the test result if "Test After RCA" button was pressed.
            If sender.text <> "Override" Then
                SaveTestResults()
            End If
            'Initial test start time
            dteTestStartTime = Now()

            'Create a label to display the Retest no. and add 1 to the Retest no. if at least one test is failed.
            If lblTestResult.Text = cstrFail Then
                intRetestNo = intRetestNo + 1
            End If

            'Reset the buttons which text is not equal to "Skip"
            For Each ctrl As Control In Controls
                'Remove the label for test buttons. 
                If TypeOf ctrl Is Label Then
                    If ctrl.Name.Length > cstrPressureLabelPrefix.Length _
                    AndAlso ctrl.Name.Substring(0, cstrPressureLabelPrefix.Length) = cstrPressureLabelPrefix Then
                        Me.Controls.Remove(ctrl)
                    ElseIf ctrl.Name = strRetestLabelName Then
                        'Remove the label for Retest No if exist.
                        Me.Controls.Remove(ctrl)
                    End If
                End If
            Next

            'Remove the test buttons 
            For intCount As Integer = 1 To drWF.NoOfSamples
                Controls.RemoveByKey(cstrPressureButtonPrefix & intCount.ToString)
            Next

            'rebuild the test result buttons and sample no. labels.
            InitializeTestResults(strBypassLanes)

            'Create a label to show the retest number
            If intRetestNo > 0 Then
                lbl = New Label
                With lbl
                    .Location = New System.Drawing.Point(intIntialLblX, intIntialLblY)
                    .Size = New System.Drawing.Size(intLblWidth, intLblHeight)
                    .Text = String.Format("Retest No: {0}", intRetestNo)
                    .Name = strRetestLabelName
                    .ForeColor = Color.White
                    .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                End With
                Me.Controls.Add(lbl)
            End If

            lblTestResult.Text = cstrNA

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub SaveTestResults()
        Dim intSampleNo As Integer
        Dim ctrl As Control

        Try
            'Only save the failed tests to the detail table 
            For Each ctrl In Controls
                If TypeOf ctrl Is Button AndAlso ctrl.Name.Substring(0, 7) = cstrPressureButtonPrefix AndAlso ctrl.Text = cstrFail Then
                    blnTestResult = False
                    intSampleNo = Convert.ToInt32(ctrl.Tag)
                    Dim intLaneNo As Integer
                    intLaneNo = (intSampleNo Mod drWF.NoOfLanes)
                    If intLaneNo = 0 Then
                        intLaneNo = drWF.NoOfLanes
                    End If
                    SetTestDetails(intLaneNo, intSampleNo)
                End If
            Next
            SaveTestDetails()

            'Update table header
            intSampleNo = 0
            SaveTestHeader(False)

        Catch ex As Exception
            Throw New Exception("SaveTestResults" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub SaveTestHeader(testResult As Boolean)
        Dim resultServerHeader As New ServerModels.tblQATShakeShineResultHeader With {
            .BatchID = gdteTestBatchID,
            .Facility = gdrSessCtl.Facility,
            .InterfaceID = gstrInterfaceID,
            .OverrideID = Nothing,
            .PackagingLine = gdrSessCtl.DefaultPkgLine,
            .RetestNo = intRetestNo,
            .ShopOrder = gdrSessCtl.ShopOrder,
            .SOStartTime = gdrSessCtl.StartTime,
            .TestStartTime = dteTestStartTime,
            .TesterID = gstrQATTesterID,
            .QATEntryPoint = strCurrQATEntryPoint,
            .TestEndTime = DateTime.Now,
            .TestResult = testResult
        }

        Dim resultLocalHeader As New LocalModels.tblQATShakeShineResultHeader With {
            .BatchID = gdteTestBatchID,
            .Facility = gdrSessCtl.Facility,
            .InterfaceID = gstrInterfaceID,
            .OverrideID = Nothing,
            .PackagingLine = gdrSessCtl.DefaultPkgLine,
            .RetestNo = intRetestNo,
            .ShopOrder = gdrSessCtl.ShopOrder,
            .SOStartTime = gdrSessCtl.StartTime,
            .TestStartTime = dteTestStartTime,
            .TesterID = gstrQATTesterID,
            .QATEntryPoint = strCurrQATEntryPoint,
            .TestEndTime = DateTime.Now,
            .TestResult = testResult
        }

        Try
            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    _dbServer.AddTotblQATShakeShineResultHeader(resultServerHeader)
                    _dbServer.SaveChanges()
                Catch ex As Exception
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    _dbLocal.AddTotblQATShakeShineResultHeader(resultLocalHeader)
                    _dbLocal.SaveChanges()
                End Try
            Else
                _dbLocal.AddTotblQATShakeShineResultHeader(resultLocalHeader)
                _dbLocal.SaveChanges()
            End If
        Catch ex As Exception
            Throw New Exception("Error in SaveTestHeader" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub SetTestDetails(intLaneNo As Integer, intSampleNo As Integer)
        resultServerDetails.Add(New ServerModels.tblQATShakeShineResultDetail With {
            .BatchID = gdteTestBatchID,
            .LaneNo = intLaneNo,
            .RetestNo = intRetestNo,
            .SampleNo = intSampleNo,
            .TestResult = blnTestResult,
            .TestTime = Now()
        })

        resultLocalDetails.Add(New LocalModels.tblQATShakeShineResultDetail With {
            .BatchID = gdteTestBatchID,
            .LaneNo = intLaneNo,
            .RetestNo = intRetestNo,
            .SampleNo = intSampleNo,
            .TestResult = blnTestResult,
            .TestTime = Now()
        })
    End Sub

    Private Sub SaveTestDetails()
        Dim resultServerDetail As ServerModels.tblQATShakeShineResultDetail
        Dim resultLocalDetail As LocalModels.tblQATShakeShineResultDetail

        Try
            'The insert and update of header and detail weight records with transaction commitment will be handled by the stored procedure.
            If gblnSvrConnIsUp = True Then
                Try
                    For Each resultServerDetail In resultServerDetails
                        _dbServer.AddTotblQATShakeShineResultDetail(resultServerDetail)
                    Next
                    _dbServer.SaveChanges()
                Catch ex As Exception
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    For Each resultLocalDetail In resultLocalDetails
                        _dbLocal.AddTotblQATShakeShineResultDetail(resultLocalDetail)
                    Next
                    _dbLocal.SaveChanges()
                End Try
            Else
                For Each resultLocalDetail In resultLocalDetails
                    _dbLocal.AddTotblQATShakeShineResultDetail(resultLocalDetail)
                Next
                _dbLocal.SaveChanges()
            End If

            resultServerDetails.Clear()
            resultLocalDetails.Clear()
        Catch ex As Exception
            Throw New Exception("Error in SaveTestDetails" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function isAllPass() As Boolean
        Dim intSampleNo As Integer

        'Check all the pressure test result buttons, return true if all are "Pass"
        Dim ctrl As Control
        Try
            isAllPass = True
            For Each ctrl In Controls
                If TypeOf ctrl Is Button AndAlso ctrl.Name.Substring(0, 7) = cstrPressureButtonPrefix AndAlso ctrl.Text = cstrFail Then
                    isAllPass = False
                    intSampleNo = Convert.ToInt32(ctrl.Tag)
                    Dim intLaneNo As Integer
                    intLaneNo = (intSampleNo Mod drWF.NoOfLanes)
                    If intLaneNo = 0 Then
                        intLaneNo = drWF.NoOfLanes
                    End If
                End If
            Next
            Return isAllPass
        Catch ex As Exception
            Throw New Exception("Error in isAllPass" & vbCrLf & ex.Message)
        End Try
    End Function

    Private Sub InitializeTestResults(strByPassLanes As String)
        '
        ' Assumption: Maximum no. of samples is 36,
        '             Maximum no. of lanes is 6,
        '             no. of lanes if specified can not be less than no. of samples.
        '
        Dim lbls As Label()
        Dim buttons As Button()
        Dim lbl As Label
        Dim btn As Button

        Dim intLblX As Int16, intlblY As Int16
        Dim intBoxX As Int16, intBoxY As Int16
        Dim intSampleNo As Int16, intRowNo As Int16, intColNo As Int16
        Const intLblWidth As Int16 = 40, intLblHeight As Int32 = 30
        Const intBoxWidth As Int16 = 75, intBoxHeight As Int32 = 50
        Const intIntialLblX As Int16 = 5, intIntialLblY As Int16 = 93
        Const intIntialBoxX As Int16 = 45, intIntialBoxY As Int16 = 80
        Const intLabelXGap As Int16 = 135, intLabelYGap As Int16 = 70
        Const intBoxXGap As Int16 = 135, intBoxYGap As Int16 = 70
        Const intMaxRows As Int16 = 6, intMaxCols As Int16 = 6

        Dim intNoOfLanes As Int16
        Dim intNoOfSamples As Int16
        Dim intNoOfRows As Int16

        Try
            'Initialize test result fields.
            blnTempTestResult = True
            blnFinalTestResult = False

            btnAccept.Visible = True
            btnRetest.Visible = False

            strByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

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

            ReDim buttons(intNoOfSamples)
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
                            .Name = cstrPressureLabelPrefix & (intSampleNo).ToString
                            .ForeColor = Color.White
                            .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                            .Tag = (intSampleNo).ToString
                        End With
                        lbls(intSampleNo) = lbl
                        Me.Controls.Add(lbl)

                        'Create button controls to hold display data
                        btn = New Button
                        With btn
                            intBoxX = intIntialBoxX + intColNo * intBoxXGap
                            intBoxY = intIntialBoxY + intRowNo * intBoxYGap
                            .Location = New System.Drawing.Point(intBoxX, intBoxY)
                            .Text = cstrPass
                            .Size = New System.Drawing.Size(intBoxWidth, intBoxHeight)
                            .Name = cstrPressureButtonPrefix & (intSampleNo).ToString
                            .ForeColor = Color.Black
                            .BackColor = Color.LawnGreen
                            .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)
                            '.ReadOnly = True
                            .TabStop = False
                            .Tag = (intSampleNo).ToString

                            'indicate the lane is skipped for test if it is recorded in the Override table.
                            If strByPassLanes <> String.Empty Then
                                intLaneNo = (intSampleNo Mod drWF.NoOfLanes)
                                If intLaneNo = 0 Then
                                    intLaneNo = drWF.NoOfLanes
                                End If
                                If strByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                                    .Text = cstrSkip
                                    .ForeColor = Color.Red
                                    .Font = New Font("Arial", 18, System.Drawing.FontStyle.Bold)
                                End If
                            End If
                        End With
                        buttons(intSampleNo) = btn
                        If strByPassLanes.IndexOf(intLaneNo.ToString & ",") < 0 Then
                            AddHandler buttons(intSampleNo).Click, AddressOf btnPressure_click  'MH this line has been added
                        End If
                        'Debug.Print(btn.Name)
                        ' Debug.Print(btn.Text)
                        Me.Controls.Add(btn)
                        intSampleNo = intSampleNo + 1   'MH total number of instances of labels and buttons
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

    ' WO#17432 ADD Start – AT 11/29/2018
    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub
    ' WO#17432 ADD Stop – AT 11/29/2018
End Class
