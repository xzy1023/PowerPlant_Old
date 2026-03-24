Public Class frmQATPackageDateCoder
    '---------------------------------------------------------------------------------
    'Author:        Bong Lee
    'Creation date: Apr. 12,2018
    'Description:   2018/4/12 - allow user to preview CoLOS label image
    '---------------------------------------------------------------------------------
    Dim strCurrQATEntryPoint As String
    Dim drwf As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim dteTestStartTime As DateTime = Now()
    Dim intRetestNo As Int16
    Dim strLabelImageLocation As String = Nothing
    Dim strLabelImageLocation_UNC As String = Nothing
    Dim strCoLOSIP As String = Nothing
    Dim intCOLOSPort As Integer
    Const strMsgBoxTitle As String = "Preview Labels"

    Private Enum TestResult
        Failed = 0
        Passsed = 1
        NA = 2
    End Enum

    Private Sub frmQATPackageDateCoder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strResult As String()
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

                If drwf.IsTCPConnIDNull Then
                    MessageBox.Show("QAT definition has not been setup to connect to CoLOS server. Test will be aborted. Contact Supervisor.", "Invalid QAT Defination Setup", MessageBoxButtons.OK)
                    CloseForm()
                    Exit Sub
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

                strResult = SharedFunctions.GetConrolTableValues("LabelImageLocation", "QAT")
                strLabelImageLocation = strResult(0)
                strLabelImageLocation_UNC = strResult(1)
                If strLabelImageLocation = String.Empty Then
                    MessageBox.Show("Cannot determine the image file location, please verify the network connection or database server.", "No Server connection", MessageBoxButtons.OK)
                    CloseForm()
                    Exit Sub
                Else
                    Using taTCPConn As New dsQATTCPConnTableAdapters.CPPsp_QATTCPConn_SelTableAdapter
                        Using dtTCPConn As New dsQATTCPConn.CPPsp_QATTCPConn_SelDataTable
                            taTCPConn.Fill(dtTCPConn, gdrSessCtl.Facility, drwf.TCPConnID, True)
                            If dtTCPConn.Count > 0 Then
                                strCoLOSIP = dtTCPConn(0).IPAddress
                                intCOLOSPort = dtTCPConn(0).Port
                            End If
                        End Using
                    End Using
                End If

                LoadPrinterList("me.Load")

                frmQATTester.ShowDialog()
                ' WO#17432 ADD Stop – AT 11/29/2018
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

    ' WO#17432 ADD Start – AT 11/20/2018
    Private Sub ChangePrtDevColor(ByVal blnEnabled As Boolean)
        Dim i As Integer = 0
        For i = 0 To Me.dgvPrtDev.Rows.Count - 1
            If Me.dgvPrtDev.Rows(i).IsNewRow = False Then
                If blnEnabled = True Then
                    dgvPrtDev.Rows(i).Cells("dgvPrtDev_btnPrtName").Style.SelectionBackColor = Color.Green
                Else
                    dgvPrtDev.Rows(i).Cells("dgvPrtDev_btnPrtName").Style.SelectionBackColor = Color.Blue
                End If
            End If
        Next
    End Sub
    ' WO#17432 ADD Stop – AT 11/20/2018

    Private Sub dgvPrtDev_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvPrtDev.CellMouseDown
        Dim strPicImageLocation As String = Nothing
        Dim strResponse As String                                                   'WO#17432 BL 1/23/2019
        Try
            If My.Computer.Network.IsAvailable = True Then 'WO#359
                'Clear the result in each row

                pbxLabelImage.Visible = False
                If e.RowIndex >= 0 AndAlso dgvPrtDev.Columns(e.ColumnIndex).Name = "dgvPrtDev_btnPrtName" Then
                    With dgvPrtDev.Rows(e.RowIndex)
                        Try
                            'WO#17432 BL 1/23/2019 GetImage(.Cells("dgvPrtDev_btnPrtName").Value)
                            'WO#17432 ADD Start BL 1/23/2019
                            strResponse = SharedFunctions.GetImage(strCoLOSIP, intCOLOSPort, .Cells("dgvPrtDev_btnPrtName").Value, strLabelImageLocation, gdrSessCtl.DefaultPkgLine, gdrSessCtl.ShopOrder)
                            If strResponse <> String.Empty Then
                                MessageBox.Show(strResponse, "Error on Getting Label Image")
                            Else
                                'WO#17432 ADD Stop BL 1/23/2019
                                strPicImageLocation = String.Format("\\{0}\{1}\{2}.jpg", strCoLOSIP, strLabelImageLocation_UNC, dgvPrtDev.Rows(e.RowIndex).Cells(0).Value)
                                pbxLabelImage.ImageLocation = strPicImageLocation
                                pbxLabelImage.Visible = True
                            End If                  'WO#17432 BL 1/23/2019
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End With
                End If
            Else
                Dim strMsg As String
                strMsg = "Network connection is not ready, Please check the IPC connection before any printer connections."
                SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnRetest_Click(sender As Object, e As EventArgs) Handles btnRetest.Click
        Dim lbl As Label
        Const intLblWidth As Int16 = 150, intLblHeight As Int32 = 27
        Const intIntialLblX As Int16 = 20, intIntialLblY As Int16 = 510
        Const strRetestLabelName As String = "lblRetestNo"

        Try
            intRetestNo = intRetestNo + 1

            ' WO#17432 ADD Start – AT 11/20/2018
            'Initial test start time
            dteTestStartTime = Now()
            btnFail.Visible = True
            dgvPrtDev.Enabled = True
            ChangePrtDevColor(True)
            lblDateFormat.Visible = False
            btnPass.Visible = True
            ' WO#17432 ADD Stop – AT 11/20/2018

            'Remove the label for Retest No if exist.
            For Each ctrl As Control In Controls
                If TypeOf ctrl Is Label AndAlso ctrl.Name = strRetestLabelName Then
                    Me.Controls.Remove(ctrl)
                End If
            Next

            'Create a label to display the ReTest no.
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

            'submit a request to load the packagingecoder image
            SharedFunctions.printCaseLabel(PACKAGECODER, String.Empty)

            ' WO#17432 ADD Start – AT 11/20/2018
            btnRetest.Visible = False
            ' WO#17432 ADD Stop – AT 11/20/2018

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub LoadPrinterList(Optional ByVal strWhen As String = "")
        Dim da As New dsPrinterDeviceTableAdapters.taPrinterDevice
        Dim dt As New dsPrinterDevice.tblPkgLinePrinterDeviceDataTable
        Dim strDeviceTypeFilter As String = Nothing
        Dim strMsg As String
        Try

            If My.Computer.Network.IsAvailable = True Then
                Try 'FX150604
                    da.Fill(dt, "ListAll", gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine)
                    strDeviceTypeFilter = "DeviceType = '" & PACKAGECODER & "' AND DeviceSubType = ''"
                    Me.dgvPrtDev.DataSource = dt.Select(strDeviceTypeFilter)
                    dgvPrtDev.Refresh()
                    'FX150604 ADD Start
                Catch sqEx As SqlClient.SqlException
                    strMsg = "Can not retrieve the printer list for the line. Please check the IPC or data server connection."
                    SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
                End Try
            Else
                If strWhen = "me.Load" Then
                    strMsg = "Please check the IPC network connection." 'FX150604
                Else
                    strMsg = "Please check the IPC network connection before any printer network connections." 'FX150604
                End If
                SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
  
    Private Sub btnFail_Click(sender As Object, e As EventArgs) Handles btnFail.Click
        Try
            SavePackageDateCoderResult(TestResult.Failed)
            btnRetest.Visible = True

            ' WO#17432 ADD Start – AT 11/20/2018
            pbxLabelImage.Visible = False
            btnFail.Visible = False
            dgvPrtDev.Enabled = False
            ChangePrtDevColor(False)
            lblDateFormat.Visible = False
            btnPass.Visible = False
            ' WO#17432 ADD Stop – AT 11/20/2018
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub btnPass_Click(sender As Object, e As EventArgs) Handles btnPass.Click
        Try
            SavePackageDateCoderResult(TestResult.Passsed)
            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drwf.QATDefnID, gstrInterfaceID, drwf.TestSeq)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()             'WO#17432 – AT 11/29/2018
        End Try
    End Sub

    Private Sub SavePackageDateCoderResult(intTestResult As Integer)
        Try
            Dim strDateCodeValue As String = "N/A"
            Const cintDateCodeType As Integer = 1
            ' Date code type:  where 1 = Package Coder, 2 = Cartonbox Coder, 3 = Case Coder

            'WO#17432 ADD Start – AT 11/20/2018
            'If lblDateFormat.Text <> String.Empty Then
            '    strDateCodeValue = lblDateFormat.Text
            'End If
            'WO#17432 ADD Stop – AT 11/20/2018

            'WO#17432 ADD Start – AT 11/15/2018
            SharedFunctions.SaveQATDateCodeResult(gdteTestBatchID, strDateCodeValue, cintDateCodeType, gdrSessCtl.Facility, gstrInterfaceID, gdrSessCtl.DefaultPkgLine, intRetestNo,
                                      gdrSessCtl.ShopOrder, gdrSessCtl.StartTime, Now, intTestResult, dteTestStartTime, Now, gstrQATTesterID, strCurrQATEntryPoint)
            'WO#17432 ADD Stop – AT 11/15/2018
        Catch ex As Exception
            Throw New Exception("Error in SavePackageDateCoderResult" & vbCrLf & ex.Message)
        End Try
    End Sub
    ' WO#17432 ADD Stop – AT 10/10/2018
    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub
End Class