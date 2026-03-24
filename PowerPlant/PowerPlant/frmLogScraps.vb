Public Class frmLogScraps

    Private Sub frmLogScraps_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        'Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim intRtnCde As Integer
        Dim strAction As String

        Try
            UcHeading1.ScreenTitle = "Log Scraps"
            Me.lblItemDesc.Text = ""

            gdrEquipment = SharedFunctions.GetEquipmentInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine)
            strAction = "ALLBySO"
            If gdrEquipment.SubType = "T" Then
                strAction = "ALLBySO+OtherScrap"
            End If

            'Retrieve Shop Order record using the Shop Order from the Session Control
            intRtnCde = gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), "")
            With tblSO.Rows(0)
                Me.lblShopOrder.Text = .Item("ShopOrder")
                Me.lblItemNo.Text = .Item("ItemNumber")
                Me.lblItemDesc.Text = .Item("ItemDesc1")
                Try
                    gtaCompScrap.Fill(Me.DsComponentScrap.CPPsp_EditComponentScrap, .Item("Facility"), CType(.Item("ShopOrder"), Integer), _
                            gdrSessCtl("StartTime"), "", 0, strAction)
                Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    gtaCompScrap.Fill(Me.DsComponentScrap.CPPsp_EditComponentScrap, .Item("Facility"), CType(.Item("ShopOrder"), Integer), _
                            gdrSessCtl("StartTime"), "", 0, strAction)
                End Try
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        Dim dgvdr As DataGridViewRow
        Dim sgnQty As Single

        For Each dgvdr In dgvBOM.Rows
            With dgvdr
                If IsDBNull(.Cells("dgvBOMtxtQty").Value) Then
                    sgnQty = 0
                Else
                    sgnQty = .Cells("dgvBOMtxtQty").Value
                End If
                If sgnQty >= 0 Then
                    SharedFunctions.UpdateLogScraps(gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), gdrSessCtl("StartTime"), _
                            .Cells("dgvBOMtxtComponent").Value, sgnQty)
                Else
                    MessageBox.Show("Quantity can not be less than zero, entry is ignored.", "Invalid Entry")
                End If
            End With
        Next
        Me.Close()
    End Sub

    Private Sub dgvBillOfMaterials_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvBOM.CellMouseDown
        Dim dgrKeyPad As DialogResult
        Dim ctl As New Control
        Try
            If e.RowIndex >= 0 AndAlso dgvBOM.Columns(e.ColumnIndex).Name = "dgvBOMtxtQty" Then
                With dgvBOM.Rows(e.RowIndex).Cells("dgvBOMtxtQty")
                    If Not IsDBNull(.Value) Then
                        ctl.Text = .Value
                    End If
                    ctl.Location = dgvBOM.Location
                    dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, ctl)
                    If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                        If gstrNumPadValue = "" Then
                            .Value = 0
                        Else
                            .Value = gstrNumPadValue
                        End If
                        If .Value < 0 Then
                            MessageBox.Show("Scrap quantity can not be less than zero. Entry is ignored", "Invalid Entry")
                            .Value = 0
                        End If
                        dgvBOM.RefreshEdit()

                    End If
                End With
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ctl.Dispose()
        End Try
    End Sub

End Class