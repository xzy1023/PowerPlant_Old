Public Class frmQATCartonBoxVisualCheck

    Dim strCurrQATEntryPoint As String
    Dim drwf As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim dteTestStartTime As DateTime = Now()

    Private Enum TestResult
        Failed = 0
        Passsed = 1
        NA = 2
    End Enum

    Private Sub frmQATCartonBoxVisualCheck_Load(sender As Object, e As EventArgs) Handles Me.Load
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
                End If
                ' WO#17432 ADD Stop – AT 12/03/2018

                'Load instruction to the screen
                If drwf.NoteID = 0 Then
                    txtNotes.Visible = False
                Else
                    txtNotes.Visible = True
                    txtNotes.Text = drwf.Note
                End If

                lblShopOrder.Text = gdrSessCtl.ShopOrder

                'Retrieve Shop Order record using the Shop Order from the Session Control
                Using tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
                    gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl.ShopOrder, "")
                    If tblSO.Count > 0 Then
                        lblUnitPerCarton.Text = tblSO(0).PackagesPerSaleableUnit
                    Else
                        MessageBox.Show("Cannot find the shop order information. Please contact Supervisor.", "Error - Missing shop order information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        CloseForm()
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
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnPass_Click(sender As Object, e As EventArgs) Handles btnPass.Click
        Try
            SaveCartonBoxTestResult(TestResult.Passsed)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnFail_Click(sender As Object, e As EventArgs) Handles btnFail.Click
        Try
            SaveCartonBoxTestResult(TestResult.Failed)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnNA_Click(sender As Object, e As EventArgs) Handles btnNA.Click
        Try
            SaveCartonBoxTestResult(TestResult.NA)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub SaveCartonBoxTestResult(intTestResult As Integer)
        Try
            SharedFunctions.SaveQATCartonBoxTestResult(Me, intTestResult, gdteTestBatchID, gdrSessCtl.Facility, gdrSessCtl.ShopOrder _
                                                       , gdrSessCtl.StartTime, gdrSessCtl.DefaultPkgLine, Now(), dteTestStartTime _
                                                       , gstrQATTesterID, strCurrQATEntryPoint)
            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
        Catch ex As Exception
            Throw New Exception("Error in SaveCartonBoxTestResult" & vbCrLf & ex.Message)
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