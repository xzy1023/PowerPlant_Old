Public Class ItemFromOvrr
    Dim daOvr As New dsItemFromOvrTableAdapters.PPsp_ItemLabelOverride_SelTableAdapter
    Dim dtOvr As New dsItemFromOvr.PPsp_ItemLabelOverride_SelDataTable
    Dim drOvr As dsItemFromOvr.PPsp_ItemLabelOverride_SelRow

    Public Sub New(ByVal strFacility As String, ByVal strItemNumber As String)
        daOvr.Fill(dtOvr, Nothing, strFacility, strItemNumber)
        If dtOvr.Rows.Count > 0 Then
            drOvr = dtOvr.Rows(0)
        End If
    End Sub


End Class
