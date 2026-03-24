
Imports System.Linq
Imports PowerPlant.LocalModels

Public Class frmPrtLabelsAndControl

    Private Property _dbLocal As New LocalModels.PowerPlantLocalEntities()

    Private Sub frmPrtLabelsAndControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim blnCheckType As Boolean
            WindowState = FormWindowState.Maximized
            UcHeading1.ScreenTitle = "Print Labels/Printer Control"
            If gdrCmpCfg("PalletStation") = True Then
                blnCheckType = gblnTryConnect
            Else
                blnCheckType = gblnTrySessCtl
            End If
            btnCaseLabeler.Visible = False
            btnPackageCoder.Visible = False
            btnFilterCoder.Visible = False
            btnPrintPalletLabels.Visible = False
            btnReprintPalletLabels.Visible = False
            btnPrtCaseLabelsNoSO.Visible = False
            btnPrtCaseLabelsWithSO.Visible = False    'WO#654

            If SharedFunctions.IsSvrConnOK(blnCheckType) = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If

            If gdrCmpCfg("PalletStation") = True Then
                btnCreatePallet.Visible = True
                btnInquiry.Visible = True
                btnPrintPalletLabels.Visible = gblnSvrConnIsUp
                btnReprintPalletLabels.Visible = gblnSvrConnIsUp
            Else
                btnCreatePallet.Visible = False
                btnInquiry.Visible = False
                If gblnSvrConnIsUp = True Then
                    Dim oPrintDevice As New PrinterDevice(gdrSessCtl("Facility"), gdrSessCtl("DefaultPkgLine"))
                    If gdrSessCtl("ShopOrder") <> 0 And gblnSvrConnIsUp = True Then
                        btnCaseLabeler.Visible = oPrintDevice.HasCasePrinter
                        btnPackageCoder.Visible = oPrintDevice.HasPackageCoderPrinter
                        btnFilterCoder.Visible = oPrintDevice.HasFilterCoderPrinter
                    End If
                    'WO#654 If gdrCmpCfg("PrintPalletLabel") = True And gblnSvrConnIsUp = True _
                    'WO#654    And oPrintDevice.HasPalletPrinter Then
                    Dim staff As tblPlantStaff = _dbLocal.tblPlantStaff.FirstOrDefault(Function(x) x.StaffID.Equals(gdrSessCtl.Operator))  'Added tblPlantStaff to the code - ZX SK Dec 30 2024'
                    If gdrCmpCfg("PrintPalletLabel") = True AndAlso gblnSvrConnIsUp = True _
                        AndAlso oPrintDevice.HasPalletPrinter AndAlso gblnStartSOWithNoLabel = False AndAlso staff.PrintAccessLevel < 2 Then   'WO#654 'adding printerAccesslevel only for supervisors Dec 30 2025 SK ZX AndAlso PrintAccessLevel < 2
                        btnPrintPalletLabels.Visible = True
                        btnReprintPalletLabels.Visible = True
                    Else
                        btnPrintPalletLabels.Visible = False
                        btnReprintPalletLabels.Visible = False
                    End If
                    _dbLocal.Detach(staff)
                End If
            End If

            If gblnStartSOWithNoLabel = False Then                  'WO#654
                btnPrtCaseLabelsNoSO.Visible = gblnSvrConnIsUp
                btnPrtCaseLabelsWithSO.Visible = gblnSvrConnIsUp    'WO#654
            End If                                                  'WO#654
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            Throw New Exception("Error in frmPrtLabelsAndControl_Load" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub btnPrtCaseLabelsNoSO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrtCaseLabelsNoSO.Click
        Try
            If gblnSvrConnIsUp = True Then
                frmPrtCaseLabelNoSO.ShowDialog()
            Else
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                MessageBox.Show("Currently the connection to the Data Server is not avaliable. Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
            End If
        Catch ex As SqlClient.SqlException
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrtCaseLabelsWithSO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrtCaseLabelsWithSO.Click
        Try
            If gblnSvrConnIsUp = True Then
                frmPrtCaseLabelWithSO.ShowDialog()
            Else
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                MessageBox.Show("Currently the connection to the Data Server is not avaliable. Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
            End If
        Catch ex As SqlClient.SqlException
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCreatePallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatePallet.Click
        Try
            If gdrCmpCfg("PalletStation") = False Then
                frmCreatePallet.ShowDialog()
            Else
                frmCreatePallet_PS.ShowDialog()
            End If
            If gblnSvrConnIsUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintLabels(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCaseLabeler.Click,
                btnPackageCoder.Click, btnFilterCoder.Click
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        'Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim strDeviceType As String = Nothing
        Try
            With gdrSessCtl
                'Dim strLabelType As String = CASELABEL
                'Dim strFacility As String = .Item("Facility")
                'Dim strDftPkgLine As String = .Item("DefaultPkgLine")
                'Dim intShopOrder As Integer = .Item("ShopOrder")
                'Dim strItemNumber As String = .Item("ItemNumber")
                'Dim intQuantity As String = 0
                'Dim strOperator As String = .Item("Operator")
                'Dim dteStartTime As DateTime = .Item("StartTime")
                'Dim strDeviceType As String = ""
                'Dim strJobName As String = .Item("DefaultPkgLine") & CType(.Item("ShopOrder"), String)

                If gblnSvrConnIsUp = True Then
                    Select Case sender.name
                        Case "btnCaseLabeler"
                            strDeviceType = CASELABEL
                        Case "btnPackageCoder"
                            strDeviceType = PACKAGECODER
                        Case "btnFilterCoder"
                            strDeviceType = FILTERCODER
                    End Select
                    'WO#650 ADD Start
                    gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl.Facility, .ShopOrder, "")
                    drSO = tblSO.Rows(0)
                    'If strDeviceType = CASELABEL And tblSO.Rows(0).Item("PrintCaseLabel") <> "Y" Then
                    'WO#650 ADD Stop
                    If strDeviceType = CASELABEL And drSO.PrintCaseLabel <> "Y" Then
                        MessageBox.Show("This item will not produce case label.", "For Your Information")
                    Else
                        If gblnStartSOWithNoLabel = False Then   'WO#654
                            'WO#650 ADD Start
                            If gblnOvrExpDate = True AndAlso (drSO.DateToPrintFlag = "1" Or drSO.DateToPrintFlag = "3") Then
                                frmExpiryDate.ShowDialog()
                            End If
                            'WO#650 ADD Stop
                            'WO#350 SharedFunctions.printCaseLabel(strDeviceType, tblSO.Rows(0).Item("LotID"))
                            SharedFunctions.printCaseLabel(strDeviceType, drSO.LotID)  'WO#650
                            'SharedFunctions.PrintDiffLabels(strLabelType, strFacility, strDftPkgLine, _
                            '            intShopOrder, strItemNumber, intQuantity, strOperator, dteStartTime, _
                            '            strDeviceType, tblSO.Rows(0).Item("LotID"), Format(.Item("ProductionDate"), "yyyyMMdd"), strJobName)
                            'WO#654 ADD Start
                        Else
                            MessageBox.Show("Shop order was started with 'NO CASE LABEL', Print Case Label is not allowed.", "Print Case Label is Prohibited.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                        'WO#654 ADD Stop
                    End If
                Else
                    SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                    MessageBox.Show("Currently the connection to the Data Server is not avaliable. Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
                End If

            End With
        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrintPalletLabels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintPalletLabels.Click
        Try
            If gblnSvrConnIsUp = True Then
                frmPrtPalletLabels.ShowDialog()
            Else
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                MessageBox.Show("Currently the connection to the Data Server is not avaliable. Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
            End If
        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnReprintPalletLabels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReprintPalletLabels.Click
        Try
            If gblnSvrConnIsUp = True Then
                frmRePrtPalletLabels.ShowDialog()
            Else
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                MessageBox.Show("Currently the connection to the Data Server is not avaliable. Please try again later. If this error occurs again, please contact your Supervisor.", "Network or Data Server may be down")
            End If
        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    'WO#359 Add Start
    Private Sub btnInquiry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInquiry.Click
        frmInquiry.ShowDialog()
    End Sub
    'WO#359 Add Stop
    'WO#359 DEL Start
    'Private Sub btnCheckNetworkConn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckNetworkConn.Click

    'Dim cnnServer As New System.Data.SqlClient.SqlConnection(gstrServerConnectionString)
    'Dim strMsg As String
    'If SharedFunctions.HasConnectivity() Then
    '    Try
    '        cnnServer.Open()
    '    Catch ex As System.Data.SqlClient.SqlException
    '        strMsg = "Network connection is ready but server connection is failure. Please try again Later."
    '    End Try
    '    strMsg = "Connection is ready. Please return to the Log On screen to click on Print Control button to reconnect to server."
    'Else
    '    strMsg = "Network connection is still not ready, Please try again Later."
    'End If
    'SharedFunctions.PoPUpMSG(strMsg, "Verify Network Connection", MessageBoxButtons.OK)
    'WO#359 DEL Stop
    'End Sub

End Class