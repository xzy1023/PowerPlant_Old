Public Class frmInquiry

    Private Sub frmInquiry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        gstrPrvForm = Me.Name
    End Sub

    Private Sub frmInquiry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'WO#359 UcHeading1.ScreenTitle = "Inquiry"
        'WO#871 UcHeading1.ScreenTitle = "Inquiry - " & My.Computer.Name.ToLower   'WO#359
        UcHeading1.ScreenTitle = "Inquiry - " & gstrMyComputerName.ToLower      'WO#871
        Try
            gstrPrvForm = Name
            If gdrCmpCfg("PalletStation") = True Then
                btnChkBOM.Visible = False
                btnPalletsProduced.Visible = False
                btnPrcMntor.Visible = False
                BtnChkSONotes.Visible = False
                'WO#755 ADD Start
                'btnRefreshData.Visible = False
                If gblnSvrConnIsUp = True Then
                    btnRefreshData.Visible = True
                End If
            Else
                btnChkBOM.Visible = True                'WO#871
                btnPalletsProduced.Visible = True       'WO#871
                BtnChkSONotes.Visible = True            'WO#871
                btnPrcMntor.Visible = gdrCmpCfg.ShowEfficiency
                If gdrSessCtl.ShopOrder = 0 Then
                    If gblnSvrConnIsUp = True Then
                        btnRefreshData.Visible = True
                    Else
                        btnRefreshData.Visible = False
                    End If
                Else
                    btnRefreshData.Visible = False
                End If
                'WO#755 ADD Stop
                'WO#871 ADD Start
                If gdrCmpCfg.ProbatEnabled = True Then
                    btnProbat.Visible = True
                Else
                    btnProbat.Visible = False
                End If
                'WO#871 ADD Stop
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnChkBOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChkBOM.Click
        If gdrSessCtl("ShopOrder") <> 0 Then
            gstrCurrentShopOrder = gdrSessCtl("ShopOrder")
            CloseIfOpened(frmBillOfMaterials)   'WO#5370
            frmBillOfMaterials.Show()
        Else
            MessageBox.Show("Can not view Bill Of Material since no shop order started.", "Missing information")
        End If
    End Sub

    Private Sub BtnChkSONotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnChkSONotes.Click
        If gdrSessCtl("ShopOrder") <> 0 Then
            gstrCurrentShopOrder = gdrSessCtl("ShopOrder")
            CloseIfOpened(frmShopOrderNotes)   'WO#5370
            frmShopOrderNotes.Show()
        Else
            MessageBox.Show("Can not view Shop Order Notes since no shop order started.", "Missing information")
        End If
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub btnPrcMntor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrcMntor.Click
        If gdrSessCtl.ServerCnnIsOk = True Then
            If gdrCmpCfg.ShowEfficiency = True Then
                gstrPrvForm = Me.Name
                CloseIfOpened(frmProcessMonitor)   'WO#5370
                frmProcessMonitor.Show()
            End If
        Else
            MessageBox.Show("Network connection failure. Can not view Process Monitor.", "Network Connection failure", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnSOSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSOSchedule.Click
        Try
            CloseIfOpened(frmShopOrderSchedule)   'WO#5370
            frmShopOrderSchedule.Show()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "S.O. Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub btnCheckNetworkConn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckNetworkConn.Click
        frmCheckConnection.ShowDialog() 'WO#359
        'WO#359 DEL Start
        'Dim cnnServer As New System.Data.SqlClient.SqlConnection(gstrServerConnectionString)
        'Dim strMsg As String
        ''WO#359 If SharedFunctions.HasConnectivity() Then
        'If My.Computer.Network.IsAvailable = True Then 'WO#359 
        '    Try
        '        cnnServer.Open()
        '    Catch ex As System.Data.SqlClient.SqlException
        '        strMsg = "Network connection is ready but server connection is failure. Please try again Later."
        '    End Try
        '    strMsg = "Connection is ready. If connection was failure when shop order was running, Please restart the shop order."
        'Else
        '    strMsg = "Network connection is still not ready, Please try again Later."
        'End If
        'SharedFunctions.PoPUpMSG(strMsg, "Verify Network Connection", MessageBoxButtons.OK)
        'WO#359 DEL End
    End Sub

    Private Sub btnPalletsProduced_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPalletsProduced.Click
        CloseIfOpened(frmPalletInquiry)   'WO#5370
        frmPalletInquiry.Show()
    End Sub
    'WO#359 ADD Start
    Private Sub btnCalculator_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculator.Click
        frmCalculator.ShowDialog()
    End Sub

    Private Sub ChkLastDownLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkLastDownLoad.Click
        Dim dteLastDownLoadTime As DateTime
        Dim strMsg As String
        Try

            dteLastDownLoadTime = SharedFunctions.GetLastDownLoadTime()

            If IsNothing(dteLastDownLoadTime) Or dteLastDownLoadTime = DateTime.MinValue Then
                strMsg = "Data has not been downloaded yet."
            Else
                strMsg = "Data was downloaded at " & dteLastDownLoadTime.ToString & "."
            End If

            If SharedFunctions.IsDataReadyForRefresh() Then
                strMsg = strMsg & " Data is ready for refresh."
            End If

            SharedFunctions.PoPUpMSG(strMsg, "Data DownLoad Status", MessageBoxButtons.OK)

        Catch ex As Exception
            MessageBox.Show("Error in ChkLastDownLoad" & vbCrLf & ex.Message)
        End Try
    End Sub
    'WO#359 ADD Stop
    'WO#755 ADD Start
    Private Sub btnRefreshData_Click(sender As Object, e As System.EventArgs) Handles btnRefreshData.Click
        Dim thdShowSplash As New System.Threading.Thread(AddressOf SharedFunctions.showSplash)  'WO#359
        Dim drRefreshData = DialogResult
        Try
            If gdrSessCtl("ShopOrder") = 0 Then
                drRefreshData = MessageBox.Show("Are you sure to refresh the local master data now?", "Refresh Local Data", MessageBoxButtons.YesNo)
                If drRefreshData = vbYes Then
                    Dim strMessage As String
                    strMessage = "Exchanging data with server, Please Wait . . ."
                    thdShowSplash.Start(strMessage)
                    'WO#871 SharedFunctions.DownloadDataFromServer(gdrSessCtl.Facility, gdrSessCtl.ComputerName)
                    SharedFunctions.DownloadDataFromServer(gdrSessCtl.Facility, gstrMyComputerName)    'WO#871
                    frmSplash.Close()
                    If thdShowSplash.IsAlive Then
                        thdShowSplash.Abort()
                    End If
                    'SharedFunctions.DownloadDataFromServer(gdrSessCtl.Facility, My.Computer.Name)

                    'import data from the staging area into the local data base if it is ready
                    'WO#718     SharedFunctions.ImportMasterTables(strMyComputerName)   
                    'WO#871 SharedFunctions.ImportMasterTables(gdrSessCtl.ComputerName)      'WO#718
                    SharedFunctions.ImportMasterTables(gstrMyComputerName)                  'WO#871
                End If
            Else
                MessageBox.Show("Shop order has been started, can not refresh local master data.", "improper Request")
            End If

        Catch ex As Exception
            MessageBox.Show("Error in RefreshData_Click" & vbCrLf & ex.Message)
            'WO#359 Add Start
        Finally
            'close the form and end the thread
            frmSplash.Close()
            If thdShowSplash.IsAlive Then
                thdShowSplash.Abort()
            End If
            'WO#359 Add Stop
        End Try
    End Sub
    'WO#755 ADD Stop
    'WO#871 ADD Start
    Private Sub btnProbat_Click(sender As Object, e As EventArgs) Handles btnProbat.Click
        Dim psiProbat As New ProcessStartInfo
        Dim drPE As dsProbatEquipment.dtProbatEquipmentRow
        Dim arProbat As String()
        Dim strFlavoredCoffee As String
        Dim intOrderType As Integer
        Dim securePassword As New Security.SecureString()

        Try
            Cursor.Current = Cursors.WaitCursor
            'If shop order is not started, use the whole bean receiver station as the default.
            strFlavoredCoffee = "N"
            intOrderType = 3

            If gdrSessCtl.ShopOrder > 0 Then
                If IsNothing(drSO) Then
                    With gdrSessCtl
                        drSO = SharedFunctions.GetSOInfo("GetSO&Item", .Facility, .ShopOrder, .DefaultPkgLine)

                    End With
                End If
                If IsNothing(drSO) Then
                    MessageBox.Show(String.Format("shop order {0} not found, please enter an valid shop order and try again.", gdrSessCtl.ShopOrder), "Missing inforamtion")
                    btnProbat.Focus()
                    Exit Sub
                End If
                strFlavoredCoffee = drSO.FlavoredCoffee
                intOrderType = drSO.OrderType
            End If

            'WO#17432   arProbat = SharedFunctions.GetProbatClientPgm()
            arProbat = SharedFunctions.GetIPCControl("ProbatClientPgm")         'WO#17432

            'make sure only one instance of the Probat Client program can be run in a computer       
            SharedFunctions.WakeUpAPgm(arProbat(1))

            'FX150505 drPE = SharedFunctions.GetProbatEquipmentXref("S", Nothing, SharedFunctions.GetReceivingStation(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strFlavoredCoffee, intOrderType), gdrSessCtl.Facility)
            drPE = SharedFunctions.GetProbatEquipmentXref("S", gdrSessCtl.DefaultPkgLine, SharedFunctions.GetReceivingStation(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strFlavoredCoffee, intOrderType), gdrSessCtl.Facility)      'FX150505

            psiProbat.FileName = arProbat(0)
            If Not IsNothing(drPE) Then
                psiProbat.Arguments = drPE.GroupID & " " & drPE.EquipmentGroupPos
            Else
                MessageBox.Show("Can not find the default receiving station of the line?", "Warning! Default Receiving Station not found.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            psiProbat.Verb = "RunAs"
            ' psiProbat.UserName = ""
            psiProbat.UseShellExecute = False

            'For Each c As Char In ""
            'securePassword.AppendChar(c)
            ' Next c
            ' psiProbat.Password = securePassword

            Process.Start(psiProbat)

        Catch ex As Exception
            Throw New Exception("Error in btnProbat_Click" & vbCrLf & ex.Message)
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub
    'WO#871 ADD Stop
    'WO#6059 ADD Start
    Private Sub btnPrintQueue_Click(sender As Object, e As EventArgs) Handles btnPrintQueue.Click
        CloseIfOpened(frmLabelPrintQueue)   'WO#5370
        frmLabelPrintQueue.Show()
    End Sub
    'WO#6059 ADD Stop
    Private Sub CloseIfOpened(frm As Form)
        'WO#5370 ADD Start
        If frm.IsHandleCreated Then
            frm.Close()
        End If
        'WO#5370 ADD Stop
    End Sub

End Class