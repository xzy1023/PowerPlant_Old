Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO

Public Class SharedFunctions
    Dim intRowCount As Integer

    Public Shared Function GetAppControlData(strKey As String, strSubkey As String, Optional strFacility As String = Nothing, Optional strAction As String = Nothing) As dsControlTable.KRsp_Control_SelRow
        Try
            Using taCtl As New dsControlTableTableAdapters.KRsp_Control_SelTableAdapter
                Using dtCtl As New dsControlTable.KRsp_Control_SelDataTable
                    taCtl.Fill(dtCtl, strKey, strSubkey, strFacility, strAction)
                    If dtCtl.Rows.Count > 0 Then
                        Return dtCtl.Rows(0)
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error in GetAppControlData - " & ex.Message)
        End Try
    End Function

End Class
