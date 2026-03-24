

Partial Public Class dsShopOrder
    Partial Class CPPsp_ShopOrderIODataTable

        Private Sub CPPsp_ShopOrderIODataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ExpiryDateDescColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
