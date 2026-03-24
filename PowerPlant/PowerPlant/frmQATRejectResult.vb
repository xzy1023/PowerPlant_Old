Public Class frmQATRejectResult
    Dim intRetestNo As Int16 = 0
    Dim strCurrQATEntryPoint As String
    Dim drwf As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim dteTestStartTime As DateTime = Now()
    Dim blnTestResult As Boolean

    Dim _dbServer As New ServerModels.PowerPlantEntities()
    Dim resultServerDetails As New List(Of ServerModels.tblQATRejectResultDetail)()

    Private Sub frmQATRejectResult_Load(sender As Object, e As EventArgs) Handles Me.Load
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

    Private Sub btnNA_Click(sender As Object, e As EventArgs) Handles btnNA.Click
        Try
            SaveTestHeader(testResult:=2)
            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnFail_Click(sender As Object, e As EventArgs) Handles btnFail.Click
        Try
            SaveTestDetail(testResult:=0)
            'SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try

        intRetestNo += 1
        btnPass.Visible = False
        btnFail.Visible = False
        btnNA.Visible = False
        btnRetest.Visible = True
    End Sub

    Private Sub btnPass_Click(sender As Object, e As EventArgs) Handles btnPass.Click
        Try
            SaveTestDetail(testResult:=1)
            SaveTestHeader(testResult:=1)
            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub

    Private Sub btnRetest_Click(sender As Object, e As EventArgs) Handles btnRetest.Click
        btnFail.Visible = True
        btnPass.Visible = True
        btnRetest.Visible = False
        lblRetestNo.Text = String.Format("Retest No: {0}", intRetestNo)
    End Sub

    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub

    Private Sub SaveTestHeader(testResult As Int16)
        Dim resultServerHeader As New ServerModels.tblQATRejectResultHeader With {
            .BatchID = gdteTestBatchID,
            .Facility = gdrSessCtl.Facility,
            .InterfaceID = gstrInterfaceID,
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
            _dbServer.AddTotblQATRejectResultHeader(resultServerHeader)
            _dbServer.SaveChanges()
        Catch ex As Exception
            Throw New Exception("Error in SaveTestHeader" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub SaveTestDetail(testResult As Int16)
        Dim resultServerDetail As New ServerModels.tblQATRejectResultDetail With {
            .BatchID = gdteTestBatchID,
            .RetestNo = intRetestNo,
            .TestResult = testResult,
            .TestTime = Now()
        }

        Try
            _dbServer.AddTotblQATRejectResultDetail(resultServerDetail)
            _dbServer.SaveChanges()
        Catch ex As Exception
            Throw New Exception("Error in SaveTestDetail" & vbCrLf & ex.Message)
        End Try
    End Sub
End Class

