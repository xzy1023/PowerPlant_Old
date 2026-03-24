Public Class frmBillOfMaterials

    Private Sub frmBillOfMaterials_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim intRtnCde As Integer

        Try
            UcHeading1.ScreenTitle = "Bill of Materials"
            Me.lblItemDesc.Text = ""
            'Retrieve Shop Order record using the Shop Order from the Session Control
            intRtnCde = taSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), CType(gstrCurrentShopOrder, Integer), "ALL")
            If tblSO.Count > 0 Then
                With tblSO.Rows(0)
                    Me.lblShopOrder.Text = .Item("ShopOrder")
                    Me.lblItemNo.Text = .Item("ItemNumber")
                    Me.lblItemDesc.Text = RTrim(.Item("ItemDesc1")) & " " & RTrim(.Item("ItemDesc2")) & " " & RTrim(.Item("PackSize"))
                    'If gsngSORemainQty = 0 Then
                    '    gsngSORemainQty = gdrSessCtl("CasesScheduled") - gdrSessCtl("CasesProduced")
                    'End If
                    'Me.CPPsp_BillOfMaterialsIOTableAdapter.Fill(DsBillOfMaterials.CPPsp_BillOfMaterialsIO, CType(.Item("ShopOrder"), Integer), "TotalBOM", gsngSORemainQty)
                    'WO#871 Me.CPPsp_BillOfMaterialsIOTableAdapter.Fill(DsBillOfMaterials.CPPsp_BillOfMaterialsIO, CType(.Item("ShopOrder"), Integer), "TotalBOM", CType(.Item("OrderQty"), Integer))
                    Me.CPPsp_BillOfMaterialsIOTableAdapter.Fill(DsBillOfMaterials.CPPsp_BillOfMaterialsIO, CType(.Item("ShopOrder"), Integer), "TotalBOM", CType(.Item("OrderQty"), Integer), Nothing)
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