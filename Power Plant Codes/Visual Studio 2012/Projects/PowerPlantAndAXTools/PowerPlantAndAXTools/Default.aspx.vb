Imports System.Web.Configuration

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

        End If
    End Sub

    Protected Sub btnLineAccept_Click(sender As Object, e As EventArgs) Handles btnLineAccept.Click

        Dim da As New dsChangeLineTableAdapters.QueriesTableAdapter
        'da.Usp_ReassignLineToIPC(ddlLine.SelectedValue)
        da.Usp_ReassignLineToIPC(ddlLine.SelectedValue, ddlComputerName.SelectedValue)
    End Sub

    Private Sub gvPallet_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPallet.RowCommand
        If (e.CommandName = "PostPallet") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim row As GridViewRow = gvPallet.Rows(index)
            Me.dsPallet.UpdateParameters.Item(0).DefaultValue = CInt(row.Cells(1).Text)
            dsPallet.Update()
        End If
    End Sub

End Class