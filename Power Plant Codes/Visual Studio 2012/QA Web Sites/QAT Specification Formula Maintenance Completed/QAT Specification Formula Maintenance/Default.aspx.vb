Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim intRowCount As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblErrMsg.Text = ""
        If Not IsPostBack Then
            GetCurrentUserName()
            BindData()
            BindFormulaIDData()
            ClearInput()
        End If
    End Sub
    Private Sub BindData()
        gvForm.DataSource = Me.GetData()
        gvForm.DataBind()
    End Sub
    Private Sub BindUpdatedData()
        gvForm.DataSource = Me.GetUpdatedData()
        gvForm.DataBind()
    End Sub
    Private Sub BindSearchData(ByVal strSearch As String)
        gvForm.DataSource = Me.GetSearchData(strSearch)
        gvForm.DataBind()
    End Sub

    Private Sub BindFormulaIDData()
        ddlFormulaID.DataSource = Me.GetFormulaIDData()
        ddlFormulaID.DataValueField = "FormulaID"
        ddlFormulaID.DataTextField = "Description"
        ddlFormulaID.DataBind()
        ddlFormulaID.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlFormulaID.SelectedIndex = 0
    End Sub
  Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub
    Private Function GetFormulaIDData() As DataTable
        Dim dt As DataTable = New DataTable("SpecFormula")
        dt.Columns.Add("Description")
        dt.Columns.Add("FormulaID")
        For i = 1 To 30
            dt.Rows.Add(i, i)
        Next
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblFormulaID9.Text = r("Description").ToString
        End If
        Return dt
    End Function

    Public Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = "SELECT Description, FormulaID FROM tblQATSpecFormula Order By Description"
        If Query = "" Then
            sql.ExecQuery(str)
        Else
            sql.ExecQuery(Query)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        If strSearchText <> "" Then
            str = "SELECT Description, FormulaID FROM tblQATSpecFormula Where Description Like '%" & strSearchText & "%' Order By Description"
            sql.ExecQuery(str)
        Else
            str = "SELECT Description, FormulaID FROM tblQATSpecFormula Order By Description"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetSortData() As DataTable
        Dim strSearchText As String = String.Empty
        Dim str As String = String.Empty
        strSearchText = txtSearch.Text

        If strSearchText <> "" Then
            str = "SELECT Description, FormulaID FROM tblQATSpecFormula Where Description Like '%" & strSearchText & "%' Order By Description"
            sql.ExecQuery(str)
        Else
            str = "SELECT Description, FormulaID FROM tblQATSpecFormula Order By Description"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetUpdatedData() As DataTable
        Dim str As String = "SELECT Description, FormulaID FROM tblQATSpecFormula Order By Description"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetCurrentValue() As Boolean
        Dim strFormulaID As String = String.Empty
        Dim ddlFormulaID2 As DropDownList = TryCast(FindControl("ddlFormulaID"), DropDownList)
        strFormulaID = ddlFormulaID2.SelectedValue.ToString
        If CheckInput(strFormulaID) = True Then
            lblFormulaID9.Text = strFormulaID
        Else
            lblErrMsg.Text = "Please select Formula ID."
            Return False
            Exit Function
        End If

        Return True
    End Function
    Private Sub DeleteRecord(ByVal intFormulaId As Integer, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATSpecFormula Where FormulaID = @formulaid"
            sql.AddParam("@formulaid", intFormulaId)
            lblMsg = "Delete record completed!"
            GetData(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strDescription
                BindSearchData(strDescription)
                lblErrMsg.Text = lblMsg
            Else
                lblErrMsg.Text = strMsg
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub UpdateRecord(ByVal intFormulaId As Integer, ByVal strDescription As String, ByVal strUser As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty

            If btnSave.Text = "Save New" Then
                str = "Insert Into tblQATSpecFormula (Description, FormulaID) values (@description, @formulaid)"
                lblMsg = "Add new record completed!"
            Else
                str = "Update tblQATSpecFormula set Description=@description Where FormulaID = @formulaid "
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@description", strDescription)
            sql.AddParam("@formulaid", intFormulaId)

            GetData(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strDescription
                BindSearchData(strDescription)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key") Then
                    lblErrMsg.Text = "Duplicate key found - " & intFormulaId.ToString & ". Update process aborted!"
                    Exit Sub
                Else
                    lblErrMsg.Text = strMsg
                End If
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub SaveRecord()
        Try
            Dim strDescription As String = String.Empty
            Dim strFormulaNewID As String = String.Empty
            Dim strFormulaID As String = String.Empty

            Dim strUser As String = String.Empty
            If GetCurrentValue() = True Then
                strUser = TryCast(FindControl("lblUserName"), Label).Text
                strFormulaID = TryCast(FindControl("lblFormulaID1"), Label).Text
                strDescription = TryCast(FindControl("txtDescription1"), TextBox).Text
                strFormulaNewID = TryCast(FindControl("lblFormulaID9"), Label).Text
                If strFormulaID = "0" Then
                    If CheckDuplicateKey(strFormulaNewID) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFormulaNewID & ". Update process aborted!"
                        Exit Sub
                    End If
                End If
                UpdateRecord(strFormulaNewID, strDescription, strUser)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Protected Function CheckDuplicateKey(ByVal intFormulaID As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT FormulaID " & _
           " FROM tblQATSpecFormula Where FormulaID = " & intFormulaID
        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function
  Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblFormulaID1.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblFormulaID1.Text = ""
        txtDescription1.Text = ""
        lblFormulaID9.Text = ""
        txtSearch.Text = ""
    End Sub
    Private Sub ClearDropDownValue()
        Dim ddlFormulaID2 As DropDownList = TryCast(FindControl("ddlFormulaID"), DropDownList)
        ddlFormulaID.SelectedIndex = 0
    End Sub
    Public Property SortDirection() As SortDirection
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = SortDirection.Ascending
            End If
            Return DirectCast(ViewState("SortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("SortDirection") = value
        End Set
    End Property


    Protected Sub gvForm_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvForm.PageIndex = e.NewPageIndex
        Dim strSearch As String = String.Empty
        strSearch = txtSearch.Text
        If strSearch = "" Then
            BindData()
        Else
            gvForm.DataSource = Me.GetSearchData(strSearch)
            gvForm.DataBind()
        End If
    End Sub

    Protected Sub gvForm_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim strDescription As String = String.Empty
        Dim strFormulaID As String = String.Empty
        strFormulaID = TryCast(gvForm.SelectedRow.FindControl("lblFormulaID"), Label).Text
        lblFormulaID1.Text = strFormulaID
        lblFormulaID9.Text = strFormulaID
        txtDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDescription"), Label).Text
        ddlFormulaID.SelectedIndex = ddlFormulaID.Items.IndexOf(ddlFormulaID.Items.FindByValue(strFormulaID))
        btnSave.Text = "Save"
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(2).VerticalAlign = VerticalAlign.Top
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(2).VerticalAlign = VerticalAlign.Top
        End If
    End Sub

    Protected Sub gvForm_Sorting(sender As Object, e As GridViewSortEventArgs)
        Dim sortExpression As String = e.SortExpression
        Dim direction As String = String.Empty
        If SortDirection = SortDirection.Ascending Then
            SortDirection = SortDirection.Descending
            direction = " DESC"
        Else
            SortDirection = SortDirection.Ascending
            direction = " ASC"
        End If
        Dim table As DataTable = Me.GetSortData()
        table.DefaultView.Sort = sortExpression & direction
        gvForm.DataSource = table
        gvForm.DataBind()
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        Dim strSearch As String = String.Empty
        strSearch = txtSearch.Text
        If strSearch = "" Then
            BindData()
        Else
            gvForm.DataSource = Me.GetSearchData(strSearch)
            gvForm.DataBind()
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ClearInput()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        If CheckInput(lblFormulaID1.Text) Then
            If CheckInput(lblFormulaID9.Text) Then
                Dim strFormulaID As String = TryCast(FindControl("lblFormulaID1"), Label).Text
                If strFormulaID = "0" Then
                    lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
                Else
                    DeleteRecord(strFormulaID, txtDescription1.Text)
                End If
            Else
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblFormulaID1.Text) Then
            If CheckInput(lblFormulaID9.Text) Then
                If CheckInput(txtDescription1.Text) Then
                    SaveRecord()
                Else
                    lblErrMsg.Text = "Please enter Description. Update process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please select Formula ID. Update process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Edit. Update process aborted!"
        End If
    End Sub

    Protected Sub ddlFormulaID_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFormulaID"), DropDownList)
        lblFormulaID9.Text = ddl.SelectedValue.ToString
    End Sub
End Class