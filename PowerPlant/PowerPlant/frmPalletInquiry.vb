Public Class frmPalletInquiry
    Dim gstrErrMsg As String
    Private Sub frmPalletInquiry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'WO#871 Dim strMyComputerName As String = My.Computer.Name

        Try
            UcHeading1.ScreenTitle = "Pallet Inquiry"
            txtPkgLine.Text = ""
            txtPkgLine.Text = gdrSessCtl.DefaultPkgLine

            'If the latest downloaded data is ready in staging area and shop order is not started, import data
            'from the staging area into the local data base
            If gdrSessCtl.ShopOrder = 0 Then
                'WO#871 SharedFunctions.ImportMasterTables(strMyComputerName)
                SharedFunctions.ImportMasterTables(gstrMyComputerName)          'WO#871
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        Try
            'Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable
            Dim tblPallet As New dsPallet.CPPsp_PalletIODataTable
            Dim tblPalletOnServer As New dsPalletsOnServer.PPsp_Pallet_SelDataTable

            gstrErrMsg = Nothing
            'Retrieve equipment record using the packaging line 
            If txtPkgLine.Text <> "" Then
                lblPkgLine.Visible = False
                lblPkgLine.Text = String.Empty
                'WO#359 lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text)
                'WO#359If lblPkgLine.Text = String.Empty Then
                'WO#359 If Not SharedFunctions.IsLineActive(gdrSessCtl.ComputerName, txtPkgLine.Text) Then 'WO#359
                'WO#359 dgvPalletProduced.DataSource = Nothing
                'WO#359 gstrErrMsg = "Please enter a valid Packaging Line."
                'WO#359 MessageBox.Show(gstrErrMsg, "Invalid information")
                'WO#359 txtPkgLine.Focus()
                'WO#359 Else
                lblPkgLine.Visible = True
                lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text)
                With gdrSessCtl
                    If gblnSvrConnIsUp Then
                        Try
                            'Fill the data grid view with Pallet information from server.
                            Me.PPsp_Pallet_SelTableAdapter.Fill(tblPalletOnServer, "AllPalletsOrderByID", .Facility, Nothing, txtPkgLine.Text, IIf(.ShopOrder = 0, Nothing, .ShopOrder), .ShiftProductionDate, .OverrideShiftNo, .Operator)
                            LblNoOfPallets.Text = tblPalletOnServer.Rows.Count.ToString
                            dgvPalletProduced.DataSource = tblPalletOnServer
                        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060
                            SharedFunctions.SetServerCnnStatusInSessCtl(False)
                            gtaPallet.Connection.ConnectionString = gstrLocalDBConnectionString
                            'WO#2563    gtaPallet.Fill(tblPallet, txtPkgLine.Text, IIf(.ShopOrder = 0, String.Empty, .ShopOrder.ToString), .Facility, 0)
                            gtaPallet.Fill(tblPallet, txtPkgLine.Text, IIf(.ShopOrder = 0, String.Empty, .ShopOrder.ToString), 0, .Facility)    'WO#2563
                            LblNoOfPallets.Text = tblPallet.Rows.Count.ToString
                            dgvPalletProduced.DataSource = tblPallet
                        End Try
                    Else
                        gtaPallet.Connection.ConnectionString = gstrLocalDBConnectionString
                        gtaPallet.Fill(tblPallet, txtPkgLine.Text, IIf(.ShopOrder = 0, String.Empty, .ShopOrder.ToString), .Facility, 0)
                        LblNoOfPallets.Text = tblPallet.Rows.Count.ToString
                        dgvPalletProduced.DataSource = tblPallet
                        MessageBox.Show("Server is not available. Data may not be accurate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) 'WO#6059
                    End If
                End With
                'WO#359 End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub txtPkgLine_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPkgLine.Validating
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable

        If Me.ActiveControl.Name = "btnPrvScn" Then
            Me.Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
            End If
        End If
    End Sub

    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            If TypeOf sender Is TextBox Or (sender.name = "cboShopOrder" And gblnDropDownIsClicked = False) Then
                dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
                If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                    If Not IsNothing(gstrNumPadValue) AndAlso gstrNumPadValue <> "" Then
                        sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue, sender.maxLength)
                    Else
                        sender.text = ""
                    End If
                End If
            End If
            If TypeOf sender Is ComboBox Then
                gblnDropDownIsClicked = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class