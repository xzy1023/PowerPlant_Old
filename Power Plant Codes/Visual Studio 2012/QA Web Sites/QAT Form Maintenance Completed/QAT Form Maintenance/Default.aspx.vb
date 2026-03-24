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
    Private Sub BindSearchData(ByVal strSearch As String)
        gvForm.DataSource = Me.GetSearchData(strSearch)
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
        Dim str As String = "SELECT InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy, TestFormID FROM tblQATForm " & _
             "Order By InterfaceFormID"
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
            str = "Select InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy, TestFormID FROM tblQATForm " & _
                " Where InterfaceFormID Like '%" & strSearchText & "%'"
            sql.ExecQuery(str)
        Else
            str = "Select InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy, TestFormID FROM tblQATForm "
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetSortData() As DataTable
        Dim str As String = String.Empty
        Dim strSearchText As String = String.Empty
        strSearchText = txtSearch.Text

        If strSearchText <> "" Then
            str = "Select InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy, TestFormID FROM tblQATForm " & _
                " Where InterfaceFormID Like '%" & strSearchText & "%'"
            sql.ExecQuery(str)
        Else
            str = "Select InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy, TestFormID FROM tblQATForm "
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function

    Protected Function GetUpdatedData() As DataTable
        Dim str As String = "SELECT InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy, TestFormID FROM tblQATForm " & _
            "Order By InterfaceFormID"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Sub DeleteRecord(ByVal intTestFormId As Integer, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATForm Where TestFormID = @testformid"
            sql.AddParam("@testformid", intTestFormId)
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

    Private Sub UpdateRecord(ByVal intTestFormId As Integer, ByVal strInterfaceFormID As String, ByVal strTestCategory As String, ByVal strFormName As String,
                           ByVal strTableName As String, ByVal intActive As Integer, ByVal strFacility As String, ByVal strUser As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            If btnSave.Text = "Save New" Then
                str = "Insert Into tblQATForm (InterfaceFormID, TestCategory, FormName, TableName, Active, Facility, UpdatedAt, UpdatedBy) values " & _
                        "(@interfaceformid, @testcategory, @formname, @tablename, @active, @facility, @updatedat, @updatedby)"
                lblMsg = "Add new record completed!"
            Else
                str = "Update tblQATForm set InterfaceFormID=@interfaceformid, TestCategory=@testcategory, FormName=@formname, TableName=@tablename," & _
                        " Active=@active, Facility=@facility, UpdatedAt=@updatedat, UpdatedBy=@updatedby" & _
                        " Where TestFormID = @testformid "
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@testformid", intTestFormId)
            sql.AddParam("@interfaceformid", strInterfaceFormID)
            sql.AddParam("@testcategory", strTestCategory)
            sql.AddParam("@formname", strFormName)
            sql.AddParam("@tablename", strTableName)
            sql.AddParam("@active", intActive)
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)

            GetData(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strInterfaceFormID
                BindSearchData(strInterfaceFormID)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & strInterfaceFormID.ToString & ". Update process aborted!"
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
            Dim strTestFormID As String = String.Empty
            Dim strInterfaceFormID As String = String.Empty
            Dim strTestCategory As String = String.Empty
            Dim strFormName As String = String.Empty
            Dim strTableName As String = String.Empty
            Dim strUser As String = TryCast(FindControl("lblUserName"), Label).Text
            Dim strActive As String = String.Empty

            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If
                strTestFormID = TryCast(FindControl("lblTestFormID1"), Label).Text
                strInterfaceFormID = TryCast(FindControl("txtInterfaceFormID1"), TextBox).Text
                strTestCategory = TryCast(FindControl("txtTestCategory1"), TextBox).Text
                strFormName = TryCast(FindControl("txtFormName1"), TextBox).Text
                strTableName = TryCast(FindControl("txtTableName1"), TextBox).Text
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text

                If strTestFormID = "0" Then
                    If CheckDuplicateKey(strInterfaceFormID, intActive) Then
                        lblErrMsg.Text = "Duplicate key found - " & IIf(intActive = 0, "Inactive", "Active") & "/" & strInterfaceFormID & ". Update process aborted!"
                        Exit Sub
                    End If
                End If
                UpdateRecord(strTestFormID, strInterfaceFormID, strTestCategory, strFormName, strTableName, intActive, strFacility, strUser)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Protected Function CheckDuplicateKey(ByVal strInterfaceFormID As String, ByVal intActive As Integer) As Boolean
        Dim str As String = String.Empty
        str = "SELECT TestFormID " & _
           " FROM tblQATForm " & _
           " Where InterfaceFormID = '" & strInterfaceFormID & "'" & _
           " And Active = " & intActive

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
        lblTestFormID1.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblTestFormID1.Text = ""
        txtInterfaceFormID1.Text = ""
        txtTestCategory1.Text = ""
        txtFormName1.Text = ""
        txtTableName1.Text = ""
        lblFacility1.Text = ""
        chkActive1.Checked = False
        txtSearch.Text = ""
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

    Protected Sub gvForm_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim strFacility As String = String.Empty
        Dim intActive As Integer = 0
        lblTestFormID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestFormID"), Label).Text
        txtInterfaceFormID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblInterfaceFormID"), Label).Text
        txtTestCategory1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestCategory"), Label).Text
        txtFormName1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFormName"), Label).Text
        txtTableName1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTableName"), Label).Text

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
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
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
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ClearInput()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        If CheckInput(lblTestFormID1.Text) Then
            If CInt(lblTestFormID1.Text) > 0 Then
                If CheckInput(txtInterfaceFormID1.Text) Then
                    Dim strTestFormID As String = TryCast(FindControl("lblTestFormID1"), Label).Text
                    DeleteRecord(strTestFormID, txtInterfaceFormID1.Text)
                Else
                    lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            End If

        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblTestFormID1.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(txtInterfaceFormID1.Text) Then
                    If CheckInput(txtTestCategory1.Text) Then
                        If CheckInput(txtFormName1.Text) Then
                            If CheckInput(txtTableName1.Text) Then
                                SaveRecord()
                            Else
                                lblErrMsg.Text = "Please enter Table Name. Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please enter Form Name. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please enter Test Category. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please enter Interface Form ID. Update process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please select Facility. Update process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Edit. Update process aborted!"
        End If
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
    Protected Sub ddlFacility_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        lblFacility1.Text = ddl.SelectedValue.ToString
    End Sub
End Class