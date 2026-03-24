Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim intRowCount As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblErrMsg.Text = ""
        If Not IsPostBack Then
            GetCurrentUserName()
            BindData()
            BindFacilityData()
            ClearInput()
        End If
    End Sub
    Private Sub BindData()
        gvForm.DataSource = Me.GetData()
        gvForm.DataBind()
    End Sub
    Private Sub BindFacilityData()
        ddlFacility.DataSource = Me.GetFacilityData()
        ddlFacility.DataValueField = "Facility"
        ddlFacility.DataTextField = "ShortDescription"
        ddlFacility.DataBind()
        ddlFacility.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlFacility.SelectedIndex = 0
    End Sub
    Private Sub BindUpdatedData()
        gvForm.DataSource = Me.GetUpdatedData()
        gvForm.DataBind()
    End Sub
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub
    Public Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = "SELECT TaskDescription, Active, Facility, UpdatedAt, UpdatedBy, TaskID FROM tblQATTaskMaster " & _
             "Order By TaskDescription"
        If Query = "" Then
            sql.ExecQuery(str)
        Else
            sql.ExecQuery(Query)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetFacilityData() As DataTable
        Dim str As String = "PPsp_Facility_Sel"
        sql.AddParam("@vchAction", "SelByRegion")
        sql.AddParam("@vchOrderBy", "Desc")
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblFacility1.Text = r("Facility").ToString
        End If
        Return sql.DBDT
    End Function
    Protected Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        If strSearchText <> "" Then
            str = "SELECT TaskDescription, Active, Facility, UpdatedAt, UpdatedBy, TaskID FROM tblQATTaskMaster " & _
                " Where TaskDescription Like '%" & strSearchText & "%'"
            sql.ExecQuery(str)
        Else
            str = "SELECT TaskDescription, Active, Facility, UpdatedAt, UpdatedBy, TaskID FROM tblQATTaskMaster " & _
                   "Order By TaskDescription"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetUpdatedData() As DataTable
        Dim str As String = "SELECT TaskDescription, Active, Facility, UpdatedAt, UpdatedBy, TaskID FROM tblQATTaskMaster " & _
              "Order By TaskDescription"
        GetData(Str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Sub DeleteRecord(ByVal intTaskID As Integer)
        Try
            Dim str As String = "Delete From tblQATTaskMaster Where TaskID = @taskid"
            sql.AddParam("@taskid", intTaskID)
            GetData(str)
            If sql.HasException(True) Then
                Exit Sub
            End If
            BindUpdatedData()
            ClearInput()
            lblErrMsg.Text = "Delete record completed!"
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub UpdateRecord(ByVal intTaskID As Integer, ByVal strTaskDescription As String,
                            ByVal intActive As Integer, ByVal strFacility As String, ByVal strUser As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            If btnSave.Text = "Save New" Then
                str = "Insert Into tblQATTaskMaster (TaskDescription, Active, Facility, UpdatedAt, UpdatedBy) values " & _
                        "(@taskdescription, @active, @facility, @updatedat, @updatedby)"
                lblMsg = "Add new record completed!"
            Else
                str = "Update tblQATTaskMaster set TaskDescription=@taskdescription, " & _
                        " Active=@active, Facility=@facility, UpdatedAt=@updatedat, UpdatedBy=@updatedby" & _
                        " Where TaskID = @taskid "
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@taskid", intTaskID)
            sql.AddParam("@taskdescription", strTaskDescription)
            sql.AddParam("@active", intActive)
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)

            GetData(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                BindUpdatedData()
                ClearInput()
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & intTaskID.ToString & ". Update process aborted!"
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
            Dim strFacility As String = String.Empty
            Dim intActive As Integer
            Dim strTaskID As String = String.Empty
            Dim strTaskDescription As String = String.Empty
            Dim strUser As String = TryCast(FindControl("lblUserName"), Label).Text
            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If
                strTaskID = TryCast(FindControl("lblTaskID1"), Label).Text
                strTaskDescription = TryCast(FindControl("txtTaskDescription1"), TextBox).Text
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                UpdateRecord(strTaskID, strTaskDescription, intActive, strFacility, strUser)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub GetToalRowCount(ByVal intRowCount As Integer)
        If intRowCount = 0 Then
            lblErrMsg.Text = "No of record found: " & intRowCount.ToString
        Else
            lblErrMsg.Text = "No of records found: " & intRowCount.ToString
        End If
    End Sub
    Protected Function CheckDuplicateKey(ByVal strTaskID As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT TaskDescription " & _
           " FROM tblQATTaskMaster  Where TaskID = " & strTaskID
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
        lblTaskID1.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblTaskID1.Text = ""
        txtTaskDescription1.Text = ""
        lblFacility1.Text = ""
        chkActive1.Checked = False
    End Sub
    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0
    End Sub
    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If
        Return True
    End Function


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
        Dim strFacility As String = String.Empty
        Dim intActive As Integer = 0
        lblTaskID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTaskID"), Label).Text
        txtTaskDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTaskDescription"), Label).Text
  
        Dim IsActive As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbActive"), CheckBox)
        If IsActive.Checked Then
            intActive = 1
            chkActive1.Checked = True
        Else
            intActive = 0
            chkActive1.Checked = False
        End If
        strFacility = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        lblFacility1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        If strFacility.Length = 2 Then
            strFacility = strFacility & " "
        End If
        ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(strFacility))
        btnSave.Text = "Save"
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
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
        Dim table As DataTable = Me.GetData()
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
        If CheckInput(lblTaskID1.Text) Then
            If CheckInput(txtTaskDescription1.Text) Then
                Dim strTestFormID As String = TryCast(FindControl("lblTaskID1"), Label).Text
                DeleteRecord(strTestFormID)
            Else
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblTaskID1.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(txtTaskDescription1.Text) Then
                    SaveRecord()
                Else
                    lblErrMsg.Text = "Please enter Task Description. Update process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please select Facility. Update process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Edit. Update process aborted!"
        End If
    End Sub

    Protected Sub ddlFacility_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        lblFacility1.Text = ddl.SelectedValue.ToString
    End Sub

End Class