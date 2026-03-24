Public Class frmQATCamerasVerification
    Dim strCurrQATEntryPoint As String
    Dim drwf As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim dteTestStartTime As DateTime = Now()
    Dim blnTestResult As Boolean
    Dim _dbServer As New ServerModels.PowerPlantEntities()
    Dim result As New ServerModels.tblQATCameraResult()
    Private Sub frmQATCamerasVerificatoin_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Is Start-Up or In-Process test?
            strCurrQATEntryPoint = SharedFunctions.FindCurrQATEntryPoint(gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

            'Retrieve QAT work flow and test information
            drwf = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strCurrQATEntryPoint, Me.Name)

            If Not IsNothing(drwf) Then             ' WO#17432 – AT 11/29/2018
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
                ' WO#17432 ADD Stop – AT 12/

                'Load instruction to the screen

                lblShopOrder.Text = gdrSessCtl.ShopOrder

                'Retrieve Shop Order record using the Shop Order from the Session Control
                Using tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                    gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl.ShopOrder, "")
                    If tblSO.Count > 0 Then
                        lblItemNo.Text = tblSO(0).ItemNumber
                    Else
                        MessageBox.Show("Cannot find the shop order information. Please contact Supervisor.", "Error - Missing shop order information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End Using

                frmQATTester.ShowDialog()
                ' WO#17432 ADD Start – AT 11/29/2018
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
    Private Sub btnMinWidthPass_Click(sender As Object, e As EventArgs) Handles btnMinWidthPass.Click
        btnMinWidthPass.BackColor = Color.Yellow
        btnMinWidthFail.BackColor = Color.LemonChiffon
        result.MinWidthReuslt = True
    End Sub

    Private Sub btnMinWidthFail_Click(sender As Object, e As EventArgs) Handles btnMinWidthFail.Click
        btnMinWidthFail.BackColor = Color.Yellow
        btnMinWidthPass.BackColor = Color.LemonChiffon
        result.MinWidthReuslt = False
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        result.TestResult = result.MinWidthReuslt

        result.BatchID = gdteTestBatchID
        ' result.ConfirmedBy = 
        result.Facility = gdrSessCtl.Facility
        result.InterfaceID = gstrInterfaceID
        result.PackagingLine = gdrSessCtl.DefaultPkgLine
        result.ShopOrder = gdrSessCtl.ShopOrder
        result.SOStartTime = gdrSessCtl.StartTime
        result.TestStartTime = dteTestStartTime
        result.TesterID = gstrQATTesterID
        result.QATEntryPoint = strCurrQATEntryPoint
        result.TestEndTime = DateTime.Now

        Try
            _dbServer.AddTotblQATCameraResult(result)
            _dbServer.SaveChanges()
            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
            Me.Close()
        Catch ex As Exception
            Throw New Exception("Error in SaveCameraVerificationResult" & vbCrLf & ex.Message)
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub
    ' WO#17432 ADD Start – AT 11/29/2018
    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub
End Class