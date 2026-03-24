Public Class frmQATMaterialValidation

    Dim strCurrQATEntryPoint As String
    Dim intActiveRow As Int16
    Dim gstrErrMsg As String = Nothing
    Dim dteTestStartTime As DateTime
    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow

    ' WO#17432 ADD Start – AT 10/26/2018
    Dim strCurrLotCellValue As String = Nothing
    Dim intActiveCol As Int16
    Dim blnIsLotCellValue As Boolean = False
    Dim blnIsKeyPadValue As Boolean = False
    ' WO#17432 ADD Stop – AT 10/26/2018
    ' WO#17432 ADD Start – AT 11/01/2018
    Dim strCurrMaterialID As String = String.Empty
    Dim intInvalidEntry As Int16 = 0
    ' WO#17432 ADD Stop – AT 11/01/2018

    Dim strMaterialID As String = String.Empty          ' WO#17432  AT 1/23/2019


    Private Sub frmQATMaterialValidation_Load(sender As Object, e As EventArgs) Handles Me.Load
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

                'Get the test Batch ID
                gdteTestBatchID = SharedFunctions.GetQATBatchID(drWF.TestSeq, strCurrQATEntryPoint)
                ' WO#17432 ADD Start – AT 12/03/2018
                If SharedFunctions.QATIsTested(drWF.FormName, gdteTestBatchID) = True Then
                    MsgBox("The test has already done in the same QAT workflow batch.")
                    CloseForm()
                    Exit Sub
                End If
                ' WO#17432 ADD Stop – AT 12/03/2018

                'initialize test start time
                dteTestStartTime = Now()

                'Fill the grid with the BOM information from table.
                CPPsp_BillOfMaterialsIOTableAdapter.Fill(DsBillOfMaterials.CPPsp_BillOfMaterialsIO, gdrSessCtl.ShopOrder, "TotalBOM", 1, Nothing)
                If DsBillOfMaterials.CPPsp_BillOfMaterialsIO.Count = 0 Then
                    MessageBox.Show("Cannot find the Bill of Materials of the shop order. Please contact Supervisor.", "Error - Missing workflow information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CloseForm()
                    Exit Sub
                End If
                '' WO#17432 ADD Start – AT 9/24/2018
                'Using tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                '    gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl.ShopOrder, "")
                '    If tblSO.Count > 0 Then
                '        lblItemNo.Text = tblSO(0).ItemNumber
                '    Else
                '        lblItemNo.Text = ""
                '        MessageBox.Show("Cannot find the shop order information. Please contact Supervisor.", "Error - Missing shop order inoformation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '        Exit Sub
                '    End If
                'End Using
                '' WO#17432 ADD Stop – AT 9/24/2018

                lblItemNo.Text = gdrSessCtl.ItemNumber
                txtLotID.Text = String.Empty
                'txtMaterialID.Text = String.Empty
                'txtMaterialID.Focus()
                ' WO#17432 ADD Start – AT 11/29/2018

                ' WO#17432 ADD Start – AT 1/23/2019
                If strCurrQATEntryPoint = "S" And gdrCmpCfg.ProbatEnabled = True And gblnSvrConnIsUp = True Then
                    strMaterialID = SharedFunctions.GetSORawMaterialInProbat(gdrSessCtl.ShopOrder, gdrSessCtl.Facility)
                    txtMaterialID.Text = strMaterialID
                Else
                    txtMaterialID.Text = String.Empty
                End If
                txtMaterialID.Focus()
                ' WO#17432 ADD Stop – AT 1/23/2019

                frmQATTester.ShowDialog()
            Else
                MessageBox.Show("Cannot find the QAT workflow information. Please contact Supervisor.", "Error - Missing workflow information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CloseForm()
                Exit Sub
                ' WO#17432 ADD Stop – AT 11/29/2018
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub

    Private Sub popupRegularKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtMaterialID.MouseDown, txtLotID.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            ' WO#17432 ADD Start – AT 10/26/2018
            blnIsKeyPadValue = True
            ' WO#17432 ADD Stop – AT 10/26/2018
            dgrKeyPad = SharedFunctions.PopRegularKeyPad(Me, sender)
            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(gstrNumPadValue) AndAlso gstrNumPadValue <> "" Then
                    sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue & vbCrLf, sender.maxLength)    'WO#17432 AT 10/30/2018
                    'WO#17432 AT 10/30/2018 sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue, sender.maxLength)
                Else
                    sender.text = "" & vbCrLf       'WO#17432 AT 10/30/2018
                    'WO#17432 AT 10/30/2018sender.text = ""
                End If
                ' WO#17432 ADD Start – AT 10/30/2018
            Else
                blnIsKeyPadValue = False
                ' WO#17432 ADD Stop – AT 10/30/2018
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtLotID_TextChanged(sender As Object, e As EventArgs) Handles txtLotID.TextChanged
        'Debug.Print(sender.text)
        If sender.Text <> "" Then
            dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value = sender.text
            With dgvMaterialsValidation.Rows(intActiveRow)
                .Cells("ItemDesc1").Style.BackColor = Color.White
                .Cells("ComponentItem").Style.BackColor = Color.White
                .Cells("Verified").Style.BackColor = Color.White
                .Cells("Lot").Style.BackColor = Color.White
            End With
            If sender.ToString.Contains(vbCrLf) Then
                'Debug.Print("CR_LF")
                sender.Text = sender.Text.Substring(0, InStr(sender.Text, vbCrLf) - 1)
                txtMaterialID.Focus()
                If sender.text = "" Then                    ' WO#17432 - AT 11/01/2018
                    dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value = sender.text

                    ' WO#17432 ADD Start – AT 11/01/2018
                Else
                    If VerifyGridValue("Lot", sender.text, intActiveRow) = False Then
                        dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value = sender.text
                    Else
                        dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value = ""
                        MsgBox("The Lot: " & sender.text & " has already entered with Component: " & strCurrMaterialID, MsgBoxStyle.Exclamation, "Update Aborted")
                        sender.Focus()
                    End If
                End If
                ' WO#17432 ADD Start – AT 11/01/2018
                txtMaterialID.Text = String.Empty
                sender.text = String.Empty
                ' WO#17432 ADD Start – AT 10/30/2018
                blnIsKeyPadValue = False
                ' WO#17432 ADD Stop – AT 10/30/2018
            End If
        End If
    End Sub

    Private Sub txtMaterialID_TextChanged(sender As Object, e As EventArgs) Handles txtMaterialID.TextChanged
        '  Debug.Print(sender.text)
        If sender.Text <> "" Then
            If sender.ToString.Contains(vbCrLf) Then
                ' Debug.Print("CR_LF")
                sender.Text = sender.Text.Substring(0, InStr(sender.Text, vbCrLf) - 1)
                txtLotID.Focus()
            End If
            If VerifyMaterial(sender.text) Then
                ' WO#17432 ADD Start – AT 10/30/2018
                If blnIsKeyPadValue = True Then
                    Me.txtLotID.Focus()
                End If
                ' WO#17432 ADD Stop – AT 10/30/2018
                sender.text = String.Empty
            End If
        End If
    End Sub

    Private Function VerifyMaterial(strMaterialID As String) As Boolean
        Dim dgvRow As DataGridViewRow
        Dim blnFound As Boolean = False
        Dim intRow As Int16

        Try
            For intRow = 0 To dgvMaterialsValidation.Rows.Count - 1
                dgvRow = dgvMaterialsValidation.Rows(intRow)
                If dgvRow.Cells("ComponentItem").Value = strMaterialID Then
                    dgvRow.Cells("Verified").Value = "Yes"
                    ' WO#17432 ADD Start – AT 9/24/2018
                    dgvRow.Cells("TestTime").Value = Now
                    ' WO#17432 ADD Stop – AT 9/24/2018
                    intActiveRow = intRow
                    blnFound = True
                End If
            Next
            Return blnFound
        Catch ex As Exception
            Throw New Exception("Error in VerifyMaterial" & vbCrLf & ex.Message)
        End Try
    End Function

    ' WO#17432 ADD Start – AT 11/01/2018
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function

    Private Function VerifyGridValue(ByVal strColName As String, ByVal strValue As String, ByVal intRow As Integer) As Boolean
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim count As Integer = 0
        Dim strDesc As String = String.Empty
        Dim strComponent As String = String.Empty
        Dim strLotID As String = String.Empty
        Dim strVerified As String = String.Empty
        Dim strTemp As String = String.Empty
        intInvalidEntry = 0
        For i = 0 To Me.dgvMaterialsValidation.Rows.Count - 1
            If Me.dgvMaterialsValidation.Rows(i).IsNewRow = False Then
                If IsDBNull(Me.dgvMaterialsValidation.Item(0, i).Value) = True Then
                    strDesc = ""
                Else
                    If String.IsNullOrEmpty(Me.dgvMaterialsValidation.Item(0, i).Value) Then
                        strDesc = ""
                    Else
                        strDesc = IIf(CheckInput(Me.dgvMaterialsValidation.Item(0, i).Value), Me.dgvMaterialsValidation.Item(0, i).Value, "")
                    End If
                End If

                If IsDBNull(Me.dgvMaterialsValidation.Item(1, i).Value) = True Then
                    strComponent = ""
                Else
                    If String.IsNullOrEmpty(Me.dgvMaterialsValidation.Item(1, i).Value) Then
                        strComponent = ""
                    Else
                        strComponent = IIf(CheckInput(Me.dgvMaterialsValidation.Item(1, i).Value), Me.dgvMaterialsValidation.Item(1, i).Value, "")
                    End If
                End If

                If IsDBNull(Me.dgvMaterialsValidation.Item(2, i).Value) = True Then
                    strLotID = ""
                Else
                    If String.IsNullOrEmpty(Me.dgvMaterialsValidation.Item(2, i).Value) Then
                        strLotID = ""
                    Else
                        strLotID = IIf(CheckInput(Me.dgvMaterialsValidation.Item(2, i).Value), Me.dgvMaterialsValidation.Item(2, i).Value, "")
                    End If
                End If

                If IsDBNull(Me.dgvMaterialsValidation.Item(3, i).Value) = True Then
                    strVerified = ""
                Else
                    If String.IsNullOrEmpty(Me.dgvMaterialsValidation.Item(3, i).Value) Then
                        strVerified = ""
                    Else
                        strVerified = IIf(CheckInput(Me.dgvMaterialsValidation.Item(3, i).Value), Me.dgvMaterialsValidation.Item(3, i).Value, "")
                    End If
                End If
                Select Case strColName
                    Case "Component"
                    Case "Lot"
                        If i <> intRow Then
                            If strValue = strLotID Then
                                strCurrMaterialID = strComponent
                                Return True
                            End If
                        End If
                    Case "Verified"
                        If strLotID <> "" And strVerified = "" Then
                            dgvMaterialsValidation.Rows(i).Cells("ItemDesc1").Style.BackColor = Color.LightPink
                            dgvMaterialsValidation.Rows(i).Cells("ComponentItem").Style.BackColor = Color.LightPink
                            dgvMaterialsValidation.Rows(i).Cells("Verified").Style.BackColor = Color.LightPink
                            dgvMaterialsValidation.Rows(i).Cells("Lot").Style.BackColor = Color.LightPink
                            intInvalidEntry = intInvalidEntry + 1
                            'ElseIf strLotID = "" And strVerified <> "" Then
                            '    dgvMaterialsValidation.Rows(i).Cells("ItemDesc1").Style.BackColor = Color.LightPink
                            '    dgvMaterialsValidation.Rows(i).Cells("ComponentItem").Style.BackColor = Color.LightPink
                            '    dgvMaterialsValidation.Rows(i).Cells("Verified").Style.BackColor = Color.LightPink
                            '    dgvMaterialsValidation.Rows(i).Cells("Lot").Style.BackColor = Color.LightPink
                            '    intInvalidEntry = intInvalidEntry + 1
                        Else
                            dgvMaterialsValidation.Rows(i).Cells("ItemDesc1").Style.BackColor = Color.White
                            dgvMaterialsValidation.Rows(i).Cells("ComponentItem").Style.BackColor = Color.White
                            dgvMaterialsValidation.Rows(i).Cells("Verified").Style.BackColor = Color.White
                            dgvMaterialsValidation.Rows(i).Cells("Lot").Style.BackColor = Color.White
                        End If
                    Case Else
                End Select
            End If
        Next
        strCurrMaterialID = ""
        Return False
    End Function

    ' WO#17432 ADD Start – AT 10/30/2018
    Private Sub dgvMaterialsValidation_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMaterialsValidation.CellClick
        Dim strVerified As String = String.Empty
        Dim strLot As String = String.Empty

        intActiveRow = e.RowIndex
        intActiveCol = e.ColumnIndex
        If intActiveRow <> -1 Then
            If intActiveCol = 2 Then
                blnIsKeyPadValue = False
                strCurrLotCellValue = dgvMaterialsValidation.Rows(intActiveRow).Cells(intActiveCol).Value
                If IsNothing(strCurrLotCellValue) Or Trim(strCurrLotCellValue) = "" Then
                    blnIsLotCellValue = False
                    txtLotID.Text = String.Empty
                Else
                    blnIsLotCellValue = False
                    txtLotID.Text = strCurrLotCellValue
                End If
                txtLotID.Focus()
            Else
                blnIsLotCellValue = False
                If intActiveCol = 3 Then
                    If String.IsNullOrEmpty(dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value) Then
                        strLot = ""
                    Else
                        strLot = IIf(CheckInput(dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value), dgvMaterialsValidation.Rows(intActiveRow).Cells("Lot").Value, "")
                    End If

                    If String.IsNullOrEmpty(dgvMaterialsValidation.Rows(intActiveRow).Cells("Verified").Value) Then
                        strVerified = ""
                    Else
                        strVerified = IIf(CheckInput(dgvMaterialsValidation.Rows(intActiveRow).Cells("Verified").Value), dgvMaterialsValidation.Rows(intActiveRow).Cells("Verified").Value, "")
                    End If
                    If strLot = "" Then
                        If strVerified <> "" Then
                            If strVerified <> "Yes" Then
                                Dim n As String = MsgBox("Do you want to clear current value?", MsgBoxStyle.YesNo, "Clear Verified Value")
                                If n = vbYes Then
                                    dgvMaterialsValidation.Rows(intActiveRow).Cells("Verified").Value = ""
                                End If
                            End If
                        End If
                    End If
                End If
            End If

        End If
    End Sub
    ' WO#17432 ADD Stop – AT 11/01/2018

    Private Sub btnDone_Click(sender As Object, e As EventArgs) Handles btnDone.Click
        Dim dgvRow As DataGridViewRow
        Dim blnTestResult As Boolean
        Dim strScannedComponentID As String
        Try
            If VerifyGridValue("Verified", "", 0) = False And intInvalidEntry <> 0 Then
                MsgBox("Invalid entry found in the grid" & vbCrLf & "- Either Lot or Verified value is missing" & vbCrLf & "Please check!", MsgBoxStyle.Information, "Save Aborted")
            Else
                ' If drWF.QATEntryPoint = "S" Then AndAlso AreAllMaterialsScanned() = False Then
                ' MessageBox.Show("All Meterals must be scanned for verification", "Check all materials for the order", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'txtMaterialID.Focus()
                'Else

                'loop though each rows of the grid to update all the test results to the data base
                For Each dgvRow In dgvMaterialsValidation.Rows
                    With dgvRow
                        If .Cells("Verified").Value <> "" Then
                            If .Cells("Verified").Value = "Yes" Then
                                blnTestResult = True
                                strScannedComponentID = .Cells("ComponentItem").Value
                            Else
                                blnTestResult = False
                                strScannedComponentID = .Cells("Verified").Value
                            End If
                            SharedFunctions.SaveQATMaterialsValidation(gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, gdrSessCtl.DefaultPkgLine, _
                                    gdrSessCtl.ShopOrder, gdrSessCtl.StartTime, Now(), dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                                    .Cells("ComponentItem").Value, .Cells("OverrideID").Value, _
                                    strScannedComponentID, .Cells("Lot").Value, blnTestResult, .Cells("TestTime").Value)
                        End If
                    End With
                Next

                SharedFunctions.SaveQATMaterialsValidation(gdteTestBatchID, gdrSessCtl.Facility, gstrInterfaceID, gdrSessCtl.DefaultPkgLine, _
                     gdrSessCtl.ShopOrder, gdrSessCtl.StartTime, Now(), dteTestStartTime, gstrQATTesterID, strCurrQATEntryPoint, _
                     Nothing, Nothing, Nothing, Nothing, Nothing, Now())

                'Update QAT processing status
                SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnOverride_Click(sender As Object, e As EventArgs) Handles btnOverride.Click
        Dim drScreen As DialogResult
        Dim strComponentID As String

        Try
            'Create the values of the parameters for the form before calling it.
            Dim dteOverrideID As DateTime = Now()
            With frmQATOverrideLogOn
                .BatchID = gdteTestBatchID
                .OverrideID = dteOverrideID
                .QATEntryPoint = strCurrQATEntryPoint
                .QATDefnID = drWF.QATDefnID
                .TestTitle = UcHeading1.ScreenTitle
                .Alert = drWF.Alert
                drScreen = .ShowDialog(Me)
            End With

            'Accepted override
            If drScreen = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(dteOverrideID) AndAlso IsDate(dteOverrideID) Then
                    With dgvMaterialsValidation.Rows(intActiveRow)
                        strComponentID = .Cells("ComponentItem").Value
                        .Cells("Verified").Value = txtMaterialID.Text
                        .Cells("OverrideID").Value = dteOverrideID
                        ' WO#17432 ADD Start – AT 9/24/2018
                        .Cells("TestTime").Value = Now
                        ' WO#17432 ADD Stop – AT 9/24/2018
                    End With
                    txtMaterialID.Text = String.Empty
                    txtLotID.Focus()
                End If
                'Bypass the whole test
            ElseIf drScreen = Windows.Forms.DialogResult.Cancel Then
                'SharedFunctions.SaveQATMaterialsValidation(gdteTestBatchID, gdrSessCtl.Facility, _
                '        gstrInterfaceID, gdrSessCtl.DefaultPkgLine, gdrSessCtl.ShopOrder, _
                '        gdrSessCtl.StartTime, Now(), dteTestStartTime, Nothing, Nothing, _
                '        Nothing, Nothing, False, Now())

                'Update QAT processing status
                SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, drWF.QATEntryPoint, _
                            drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()         ' WO#17432 – AT 11/29/2018
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