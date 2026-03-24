Public Class frmShopOrderNotes

    Private Sub frmShopOrderNotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim intRtnCde As Integer
        Dim frmStartSO As New frmStartShopOrder
        Dim sb As New System.Text.StringBuilder     'ALM#11578

        Try
            UcHeading1.ScreenTitle = "Shop Order Notes"
            Me.lblItemDesc.Text = ""
            'Retrieve Shop Order record using the Shop Order from the Session Control
            intRtnCde = taSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), CType(gstrCurrentShopOrder, Integer), "")
            If tblSO.Count > 0 Then
                With tblSO.Rows(0)
                    Me.lblShopOrder.Text = .Item("ShopOrder")
                    Me.lblItemNo.Text = .Item("ItemNumber")
                    Me.lblItemDesc.Text = RTrim(.Item("ItemDesc1")) & " " & RTrim(.Item("ItemDesc2")) & " " & RTrim(.Item("PackSize"))
                    Me.CPPsp_ItemNotesIOTableAdapter.Fill(DsItemNotes.CPPsp_ItemNotesIO, .Item("ItemNumber"), "AllByItemNo")
                    'ALM#11578 DEL Start
                    'If DsItemNotes.CPPsp_ItemNotesIO.Rows.Count > 0 Then
                    '    txtNotes.Text = DsItemNotes.CPPsp_ItemNotesIO.Rows(0).Item("Text")
                    'End If
                    'ALM#11578 DEL Stop
                    'ALM#11578 ADD Start
                    For Each drItemNotes As dsItemNotes.CPPsp_ItemNotesIORow In DsItemNotes.CPPsp_ItemNotesIO.Rows
                        sb.Append(drItemNotes.Text)
                    Next
                    'WO#3686 txtNotes.Text = sb.ToString.Trim
                    txtNotes.Text = sb.ToString.Trim.Replace(vbCrLf, vbLf).Replace(vbLf, vbCrLf)      'WO#3686
                    'ALM#11578 ADD Stop
                End With
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

End Class