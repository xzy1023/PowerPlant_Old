Public Class frmPrtPalletLabels
    Dim blnHaveLabelPrinted As Boolean = False
    Dim gstrErrMsg As String
    Private Sub frmPalletLabels_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Me.UcHeading1.ScreenTitle = "Print Pallet Labels"
            Me.Text = "Print Pallet Labels"
            txtPkgLine.Text = ""
            txtShopOrder.Text = ""
            lblMessage.Text = ""    'WO#512
            dgvPallet.Enabled = True
            If Not gdrCmpCfg("PalletStation") Then
                txtPkgLine.Text = gdrSessCtl("DefaultPkgLine")
            End If
            'If any pallet records are found in local data base, upload them to server first
            SharedFunctions.uploadPalletToServer()

            Fill_dgvPallet()

        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            Throw ex
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtShopOrder.MouseDown
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
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        'Try
        '    'if it is screen called by label printing package line and at lease a pallet label has been printed, reload case label
        '    With gdrSessCtl
        '        If blnHaveLabelPrinted = True Then
        '            If Not IsDBNull(gdrSessCtl("ShopOrder")) Then
        '                If gdrSessCtl("ShopOrder") > 0 Then
        '                    taSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("ShopOrder"), "")
        '                    SharedFunctions.CreateAndPrintLabel(CASELABEL, .Item("Facility"), .Item("DefaultPkgLine"), .Item("ShopOrder"), _
        '                                   .Item("ItemNumber"), 0, .Item("Operator"), .Item("StartTime"), CASELABELER, _
        '                                   .Item("DefaultPkgLine") + CType(.Item("ShopOrder"), String), tblSO.Rows(0).Item("LotID"))
        '                End If
        '            End If
        '        End If
        '    End With
        Me.Close()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Private Sub dgvPallet_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvPallet.CellMouseDown
        Dim strWhichButton As String
        Dim strJobName As String = ""
        Dim blnRtnFlag As Boolean           'WO#5370

        Try
            dgvPallet.Enabled = False
            strWhichButton = dgvPallet.Columns(e.ColumnIndex).Name
            'Debug.Print("row= " & e.RowIndex)
            If e.RowIndex >= 0 AndAlso (strWhichButton = "btnPrint" Or strWhichButton = "btnDelete") Then
                With dgvPallet.Rows(e.RowIndex)
                    If .Cells(0).Value = "Repost" And strWhichButton = "btnPrint" Then
                        SharedFunctions.EditPallet("RePost", .Cells("PalletID").Value)
                        lblMessage.Text = .Cells("PalletID").Value & " has been reposted."  'WO#512
                    Else
                        'WO#5370    SharedFunctions.ProcessFrmPrintPallet(dgvPallet, e, gdrCmpCfg("PalletStation"), gdrSessCtl("OverrideShiftNo"))
                        blnRtnFlag = SharedFunctions.ProcessFrmPrintPallet(dgvPallet, e, gdrCmpCfg("PalletStation"), gdrSessCtl("OverrideShiftNo"))  'WO#5370
                        If strWhichButton = "btnPrint" Then
                            blnHaveLabelPrinted = True
                            lblMessage.Text = .Cells("PalletID").Value & " has been submited to print." 'WO#512
                        Else
                            'WO#5370 ADD Start
                            If blnRtnFlag = True Then   'deletion has been cancelled.
                                lblMessage.Text = "The deletion of pallet, " & .Cells("PalletID").Value & " has been cancelled."
                            Else
                                lblMessage.Text = "Pallet " & .Cells("PalletID").Value & " has been deleted."
                                'WO#5370 ADD Stop
                                'WO#5370    lblMessage.Text = .Cells("PalletID").Value & " has been deleted."  'WO#512
                            End If          'WO#5370
                        End If
                    End If
                    Fill_dgvPallet()
                End With
            End If
        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
            Throw ex
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dgvPallet.Enabled = True
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtPallet0 As New dsPallet.CPPsp_PalletIODataTable
        Dim dtPallet1 As New dsPallet.CPPsp_PalletIODataTable
        Dim strSortFormat As String = String.Empty
        Try
            If gstrErrMsg = Nothing Then
                lblMessage.Text = ""    'WO#512
                Fill_dgvPallet()
            Else
                MessageBox.Show(gstrErrMsg, "Invalid information")
            End If
        Catch ex As SqlClient.SqlException
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
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

    Private Sub Fill_dgvPallet()
        Dim dtPallet1 As New dsPallet.CPPsp_PalletIODataTable
        Dim dgvrPallet As DataGridViewRow
        Dim dgvbcPrint As DataGridViewButtonCell
        Dim strSortFormat As String = String.Empty
        Dim strSelect As String

        TblPalletBindingSource.DataSource = Nothing     'WO#512

        'WO#2563    Me.CPPsp_PalletIOTableAdapter.Fill(DsPallet.CPPsp_PalletIO, Me.txtPkgLine.Text, Me.txtShopOrder.Text, gdrSessCtl.Facility, 0)
        'WO#2563    Me.CPPsp_PalletIOTableAdapter.Fill(dtPallet1, Me.txtPkgLine.Text, Me.txtShopOrder.Text, gdrSessCtl.Facility, 1)

        Me.CPPsp_PalletIOTableAdapter.Fill(DsPallet.CPPsp_PalletIO, Me.txtPkgLine.Text, Me.txtShopOrder.Text, 0, gdrSessCtl.Facility)   'WO#2563
        Me.CPPsp_PalletIOTableAdapter.Fill(dtPallet1, Me.txtPkgLine.Text, Me.txtShopOrder.Text, 1, gdrSessCtl.Facility)                 'WO#2563

        DsPallet.CPPsp_PalletIO.Merge(dtPallet1)

        If txtPkgLine.Text = "" And txtShopOrder.Text = "" Then
            strSortFormat = "DefaultPkgLine, ShopOrder, CreationDateTime"
        ElseIf txtPkgLine.Text <> "" And txtShopOrder.Text = "" Then
            strSortFormat = "ShopOrder, CreationDateTime"
        ElseIf txtPkgLine.Text <> "" And txtShopOrder.Text <> "" Then
            strSortFormat = "CreationDateTime"
        End If

        strSelect = "PrintStatus = '0' or  (PrintStatus = '1' and LastUpdate <= '" & DateAdd(DateInterval.Second, CType(My.Settings.gintTimeDelayToCheckPalletRepost, Int16) * -1, Now()) & "') "
        DsPallet.CPPsp_PalletIO.DefaultView.RowFilter = strSelect
        DsPallet.CPPsp_PalletIO.DefaultView.Sort = strSortFormat
        'WO#512 dgvPallet.DataSource = DsPallet.CPPsp_PalletIO.DefaultView.Table   
        TblPalletBindingSource.DataSource = DsPallet.CPPsp_PalletIO.DefaultView.Table   'WO#512

        For Each dgvrPallet In dgvPallet.Rows

            dgvbcPrint = dgvrPallet.Cells("btnPrint")
            dgvbcPrint.UseColumnTextForButtonValue = False
            If dgvrPallet.Cells("PrintStatus").Value = 1 Then
                dgvbcPrint.Value = "Repost"
            Else
                dgvbcPrint.Value = "Print"
            End If
        Next
        dgvPallet.Refresh()  'WO#512
    End Sub

    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        gstrErrMsg = Nothing
        Dim strPkgLine As String = String.Empty
        Try
            'Retrieve equipment record using the packaging line 
            If txtPkgLine.Text <> "" Then
                If txtPkgLine.Text.Length < txtPkgLine.MaxLength Then
                    strPkgLine = txtPkgLine.Text.Trim.PadRight(txtPkgLine.MaxLength)
                Else                                'WO#359
                    strPkgLine = txtPkgLine.Text    'WO#359
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

End Class