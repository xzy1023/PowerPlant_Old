

Partial Public Class dsComputerCfg
    Partial Class tblComputerConfigDataTable

        Private Sub tblComputerConfigDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ComputerNameColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class

Namespace dsComputerCfgTableAdapters
    
    Partial Public Class tblComputerConfigTableAdapter
    End Class
End Namespace
