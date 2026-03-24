Public Class frmRePrtPalletLabels
    'Dim blnHaveLabelPrinted As Boolean = False
    Dim gstrErrMsg As String = Nothing
    Private Sub frmRePrtPalletLabels_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.UcHeading1.ScreenTitle = "Reprint Pallet Labels"
            Me.Text = "Reprint Pallet Labels"
            lblMessage.Text = ""    'WO#512
            If gdrCmpCfg("PalletStation") Then
                SharedFunctions.clearInputFields(Me)
                'txtPkgLine.Text = ""
                'txtShopOrder.Text = ""
                'txtPalletID.Text = ""
                dgvPalletHst.Visible = False
            Else
                txtPkgLine.Text = gdrSessCtl("DefaultPkgLine")
                txtShopOrder.Text = gdrSessCtl("ShopOrder")
                txtPalletID.Text = ""
                btnSearch_Click(Me, e)
            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtShopOrder.MouseDown, txtPalletID.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)
            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                sender.text = gstrNumPadValue
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                sender.text = gstrNumPadValue
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Try
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvPalletHst_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvPalletHst.CellMouseDown
        Dim strJobName As String = ""

        Try
            If e.RowIndex >= 0 AndAlso dgvPalletHst.Columns(e.ColumnIndex).Name = "dgvPalletHstBtnPrint" Then
                PrintPalletLabel(dgvPalletHst.Rows(e.RowIndex).Cells("dgvPalletHstTxtPalletID").Value, "", gdrSessCtl.Facility, 0, gdrCmpCfg.PalletStation)

                'WO#512 DEL Start
                'CPPsp_PalletHstIOTableAdapter.Fill(DsPalletHst.CPPsp_PalletHstIO, "ALLByPalletID", dgvPalletHst.Rows(e.RowIndex).Cells("dgvPalletHstTxtPalletID").Value, "", gdrSessCtl.Facility, 0)
                'With Me.DsPalletHst.CPPsp_PalletHstIO

                '    If .Rows.Count > 0 Then
                '        SharedFunctions.PrintPalletByID(.Rows(0), gdrCmpCfg("PalletStation"))
                '        'SharedFunctions.ProcessFrmRePrintPallet(dgvPalletHst, e)
                '        'Me.CPPsp_PalletHstIOTableAdapter.Fill(DsPalletHst.CPPsp_PalletHstIO, "ALL_Line_SO", 0, Me.txtPkgLine.Text, CType(Me.txtShopOrder.Text, Integer))
                '    End If
                'End With
                'WO#512 DEL Stop
            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
            'Catch ex As SqlClient.SqlException
            '    SharedFunctions.SetServerCnnStatusInSessCtl(False)
            '    MessageBox.Show(gcstSvrCnnFailure, "Network or Data Server may be down", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            lblMessage.Text = ""    'WO#512
            If gstrErrMsg = Nothing Then
                If Me.txtShopOrder.Text = "" And Me.txtPkgLine.Text = "" Then
                    dgvPalletHst.Visible = False
                Else
                    dgvPalletHst.Visible = True
                    If Me.txtShopOrder.Text = "" Then
                        'FX20160509 Me.CPPsp_PalletHstIOTableAdapter.Fill(DsPalletHst.CPPsp_PalletHstIO, "AllBy_Line_SO", 0, Me.txtPkgLine.Text, gdrSessCtl.Facility, 0)
                        Me.CPPsp_PalletHstIOTableAdapter.Fill(DsPalletHst.CPPsp_PalletHstIO, "AllBy_Line_SO", 0, Me.txtPkgLine.Text, 0, gdrSessCtl.Facility)    'FX20160509
                    Else
                        'FX20160509 Me.CPPsp_PalletHstIOTableAdapter.Fill(DsPalletHst.CPPsp_PalletHstIO, "AllBy_Line_SO", 0, Me.txtPkgLine.Text, gdrSessCtl.Facility, CType(Me.txtShopOrder.Text, Integer))
                        Me.CPPsp_PalletHstIOTableAdapter.Fill(DsPalletHst.CPPsp_PalletHstIO, "AllBy_Line_SO", 0, Me.txtPkgLine.Text, CType(Me.txtShopOrder.Text, Integer), gdrSessCtl.Facility) 'FX20160509
                    End If

                End If
            Else
                MessageBox.Show(gstrErrMsg, "Invalid information")
            End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtShopOrder_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtShopOrder.Validating
        Try
            If sender.text <> "" Then
                Dim intShopOrder As Integer = CType(Me.txtShopOrder.Text, Integer)
            End If
        Catch ex As Exception
            MessageBox.Show("Please enter a valid Shop Order.", "Invalid Information")
            e.Cancel = True
        End Try
    End Sub

    Private Sub btnPrintPallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintPallet.Click
        Dim intPalletID As Integer
        Try
            If gstrErrMsg = Nothing Then
                If txtPalletID.Text <> "" Then
                    If Integer.TryParse(txtPalletID.Text, intPalletID) Then 'WO#512
                        PrintPalletLabel(intPalletID, "", gdrSessCtl.Facility, 0, gdrCmpCfg.PalletStation) 'WO#512

                        'WO#512 DEL Start
                        'CPPsp_PalletHstIOTableAdapter.Fill(Me.DsPalletHst.CPPsp_PalletHstIO, "ALLByPalletID", New System.Nullable(Of Integer)(CType(txtPalletID.Text, Integer)), "", gdrSessCtl.Facility, 0)
                        'With Me.DsPalletHst.CPPsp_PalletHstIO
                        '    If .Rows.Count > 0 Then
                        '        SharedFunctions.PrintPalletByID(.Rows(0), gdrCmpCfg("PalletStation"))
                        '        txtPalletID.Text = ""
                        '        'With .Rows(0)
                        '        'SharedFunctions.PrintPalletByID(.Item("DefaultPkgLine"), CType(.Item("ShopOrder"), Integer), CType(txtPalletID.Text, Integer), _
                        '        '                               .Item("ItemNumber"), .Item("Quantity"), .Item("StartTime"))
                        '        btnSearch_Click(sender, e)
                        '        'End With
                        '    Else
                        '        MessageBox.Show("Please enter a valid Pallet ID.", "Invalid Information")
                        '        txtPalletID.Focus()
                        '    End If
                        'End With
                        'WO#512 DEL Stop
                    Else
                        MessageBox.Show("Pallet ID must be an integer.", "Invalid Information") 'WO#512
                        txtPalletID.Focus() 'WO#512
                    End If
                Else
                    MessageBox.Show("Please enter a valid Pallet ID.", "Invalid Information")
                    txtPalletID.Focus()
                End If
                Else
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                End If
        Catch ex As SqlClient.SqlException When ex.ErrorCode = -2146232060 And gblnSvrConnIsUp = True
            Throw ex
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        gstrErrMsg = Nothing
        Dim strPkgLine As String = String.Empty
        Try
            'Retrieve equipment record using the packaging line 
            If txtPkgLine.Text <> "" Then
                If txtPkgLine.Text.Length < txtPkgLine.MaxLength Then
                    strPkgLine = txtPkgLine.Text.Trim.PadRight(txtPkgLine.MaxLength)
                Else                                'WO#650
                    strPkgLine = txtPkgLine.Text    'WO#650
                End If
                If SharedFunctions.GetEquipmentDescription(gdrSessCtl.Facility, strPkgLine) = String.Empty Then
                    gstrErrMsg = "Please enter a valid Packaging Line."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtPkgLine.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtPkgLine_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPkgLine.Validating
        Try
            If (ActiveControl.Name = "btnPrvScn") Then
                Me.Close()
            Else
                If Not IsNothing(gstrErrMsg) Then
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'WO#512 ADD Start
    Private Sub PrintPalletLabel(ByVal intPalletID As Integer, ByVal strPackagingLine As String, ByVal strFacility As String, ByVal intShopOrder As Integer, ByVal blnSbmFromPalletStation As Boolean)
        Try
            Using dtPH As New dsPalletHst.CPPsp_PalletHstIODataTable
                With dtPH
                    'FX20160509 CPPsp_PalletHstIOTableAdapter.Fill(dtPH, "ALLByPalletID", intPalletID, strPackagingLine, strFacility, intShopOrder)    'WO#512
                    CPPsp_PalletHstIOTableAdapter.Fill(dtPH, "ALLByPalletID", intPalletID, strPackagingLine, intShopOrder, strFacility)    'FX20160509
                    If .Rows.Count > 0 Then
                        SharedFunctions.PrintPalletByID(.Rows(0), blnSbmFromPalletStation)
                        lblMessage.Text = .Rows(0).Item("PalletID") & " has been submited to print."
                    Else
                        MessageBox.Show("Please enter a valid Pallet ID.", "Invalid Information")
                        txtPalletID.Focus()
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'WO#512 ADD Stop
End Class