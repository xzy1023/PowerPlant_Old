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
            BindFormulaData()
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
    Private Sub BindFormulaData()
        ddlFormulaDesc.DataSource = Me.GetFormulaData()
        ddlFormulaDesc.DataValueField = "FormulaID"
        ddlFormulaDesc.DataTextField = "Description"
        ddlFormulaDesc.DataBind()
        ddlFormulaDesc.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlFormulaDesc.SelectedIndex = 0
    End Sub

    Private Sub BindUpdatedData()
        gvForm.DataSource = Me.GetUpdatedData()
        gvForm.DataBind()
    End Sub
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub

    Public Function GetData(Optional Query As String = "") As DataTable
        Dim str As String
        str = "SELECT A.Active, A.Facility, A.Formula, A.LwLmtFromTarget, A.TestSpecDesc, A.TestSpecID, A.UpdatedAt, A.UpdatedBy, A.UpLmtFromTarget, B.Description as 'FormulaDesc'" & _
            " FROM tblQATSpec A Inner Join tblQATSpecFormula B On A.Formula = B.FormulaID" & _
            " Order By A.TestSpecDesc"

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
    Private Function GetFormulaData() As DataTable
        Dim str As String = String.Empty
        str = "SELECT FormulaID, Description FROM tblQATSpecFormula Where Active=1 Order by Description"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblFormulaDesc1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function

    Protected Function GetUpdatedData() As DataTable
        Dim str As String
        str = "SELECT A.Active, A.Facility, A.Formula, A.LwLmtFromTarget, A.TestSpecDesc, A.TestSpecID, A.UpdatedAt, A.UpdatedBy, A.UpLmtFromTarget, B.Description as 'FormulaDesc'" & _
            " FROM tblQATSpec A Inner Join tblQATSpecFormula B On A.Formula = B.FormulaID" & _
            " Order By A.TestSpecDesc"

        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        str2 = "SELECT A.Active, A.Facility, A.Formula, A.LwLmtFromTarget, A.TestSpecDesc, A.TestSpecID, A.UpdatedAt, A.UpdatedBy, A.UpLmtFromTarget, B.Description as 'FormulaDesc'" & _
                " FROM tblQATSpec A Inner Join tblQATSpecFormula B On A.Formula = B.FormulaID"

        If strSearchText <> "" Then
            str = str2 & " Where A.TestSpecDesc Like '%" & strSearchText & "%' Order By A.TestSpecDesc"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By A.TestSpecDesc"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Protected Function GetSortData() As DataTable
        Dim strSearchText As String = String.Empty
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        strSearchText = txtSearch.Text
        str2 = "SELECT A.Active, A.Facility, A.Formula, A.LwLmtFromTarget, A.TestSpecDesc, A.TestSpecID, A.UpdatedAt, A.UpdatedBy, A.UpLmtFromTarget, B.Description as 'FormulaDesc'" & _
                " FROM tblQATSpec A Inner Join tblQATSpecFormula B On A.Formula = B.FormulaID"

        If strSearchText <> "" Then
            str = str2 & " Where A.TestSpecDesc Like '%" & strSearchText & "%' Order By A.TestSpecDesc"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By A.TestSpecDesc"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function

    Private Sub DeleteRecord(ByVal intTestSpecId As Integer, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete FROM tblQATSpec Where TestSpecID = @testspecid"
            sql.AddParam("@testspecid", intTestSpecId)
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
    Private Sub UpdateRecord(ByVal intTestSpecId As Integer, ByVal strTestSpecDesc As String, ByVal intFormula As Integer, ByVal decLwLmtFromTarget As Decimal, ByVal decUpLmtFromTarget As Decimal,
                       ByVal intActive As Integer, ByVal strFacility As String, ByVal strUser As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            If btnSave.Text = "Save New" Then
                str = "Insert Into tblQATSpec (TestSpecDesc, Formula, LwLmtFromTarget, UpLmtFromTarget, Active, Facility, UpdatedAt, UpdatedBy) values " & _
                        "(@testspecdesc, @formula, @lwlmtfromtarget, @uplmtfromtarget, @active, @facility, @updatedat, @updatedby)"
                lblMsg = "Add new record completed!"
            Else
                str = "Update tblQATSpec set TestSpecDesc=@testspecdesc, Formula=@formula, LwLmtFromTarget=@lwlmtfromtarget, UpLmtFromTarget=@uplmtfromtarget, " & _
                    " Active=@active, Facility=@facility, UpdatedAt=@updatedat, UpdatedBy=@updatedby" & _
                    " Where TestSpecID = @testspecid "
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@testspecid", intTestSpecId)
            sql.AddParam("@TestSpecDesc", strTestSpecDesc)
            sql.AddParam("@Formula", intFormula)
            sql.AddParam("@lwlmtfromtarget", decLwLmtFromTarget)
            sql.AddParam("@uplmtfromtarget", decUpLmtFromTarget)
            sql.AddParam("@active", intActive)
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)
            GetData(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strTestSpecDesc
                BindSearchData(strTestSpecDesc)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & intTestSpecId.ToString & ". Update process aborted!"
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
            Dim intTestSpecID As String = String.Empty
            Dim strTestSpecDesc As String = String.Empty
            Dim decLwLmtFromTarget As Decimal = 0.0
            Dim decUpLmtFromTarget As Decimal = 0.0
            Dim intFormula As Integer = 0
            Dim strUser As String = String.Empty
            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)

                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If
                intTestSpecID = TryCast(FindControl("lblTestSpecID1"), Label).Text
                strTestSpecDesc = TryCast(FindControl("txtTestSpecDesc1"), TextBox).Text
                intFormula = TryCast(FindControl("lblFormulaDesc1"), Label).Text
                decLwLmtFromTarget = Convert.ToDecimal(TryCast(FindControl("txtLwLmtFromTarget1"), TextBox).Text)
                decUpLmtFromTarget = Convert.ToDecimal(TryCast(FindControl("txtUpLmtFromTarget1"), TextBox).Text)
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                strUser = TryCast(FindControl("lblUserName"), Label).Text
                UpdateRecord(intTestSpecID, strTestSpecDesc, intFormula, decLwLmtFromTarget, decUpLmtFromTarget, intActive, strFacility, strUser)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Protected Function CheckDuplicateKey(ByVal strTestSpecID As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT Formula " & _
           " FROM tblQATSpec  Where TestSpecID = " & strTestSpecID
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
    Private Function CheckLimitValue(ByVal strTemp As String) As Boolean
        Dim number As Decimal
        If Decimal.TryParse(strTemp, number) Then
            Return True
        End If
        Return False
    End Function

    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblTestSpecID1.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblTestSpecID1.Text = ""
        txtTestSpecDesc1.Text = ""
        txtLwLmtFromTarget1.Text = ""
        txtUpLmtFromTarget1.Text = ""
        lblFacility1.Text = ""
        lblFormulaDesc1.Text = ""
        chkActive1.Checked = False
    End Sub
    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0

        Dim ddlFormulaDesc2 As DropDownList = TryCast(FindControl("ddlFormulaDesc"), DropDownList)
        ddlFormulaDesc2.SelectedIndex = 0

    End Sub
    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strFormulaDesc As String = String.Empty

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlFormulaDesc2 As DropDownList = TryCast(FindControl("ddlFormulaDesc"), DropDownList)
        strFormulaDesc = ddlFormulaDesc2.SelectedValue.ToString
        If CheckInput(strFormulaDesc) = True Then
            lblFormulaDesc1.Text = strFormulaDesc
        Else
            lblErrMsg.Text = "Please select Formula."
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
        Dim strFormulaDesc As String = String.Empty
        Dim intActive As Integer = 0
        lblTestSpecID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestSpecID"), Label).Text
        txtTestSpecDesc1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestSpecDesc"), Label).Text
        txtLwLmtFromTarget1.Text = TryCast(gvForm.SelectedRow.FindControl("lblLwLmtFromTarget"), Label).Text
        txtUpLmtFromTarget1.Text = TryCast(gvForm.SelectedRow.FindControl("lblUpLmtFromTarget"), Label).Text
   
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

        strFormulaDesc = TryCast(gvForm.SelectedRow.FindControl("lblFormula"), Label).Text
        lblFormulaDesc1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFormulaDesc"), Label).Text
        ddlFormulaDesc.SelectedIndex = ddlFormulaDesc.Items.IndexOf(ddlFormulaDesc.Items.FindByValue(strFormulaDesc))
        btnSave.Text = "Save"
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(4).VerticalAlign = VerticalAlign.Top
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(4).VerticalAlign = VerticalAlign.Top
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
        If CheckInput(lblTestSpecID1.Text) Then
            If CheckInput(txtTestSpecDesc1.Text) Then
                Dim strTestSpecID As String = TryCast(FindControl("lblTestSpecID1"), Label).Text
                DeleteRecord(strTestSpecID, txtTestSpecDesc1.Text)
            Else
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblTestSpecID1.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(txtTestSpecDesc1.Text) Then
                    If CheckInput(lblFormulaDesc1.Text) Then
                        If CheckInput(txtLwLmtFromTarget1.Text) And CheckLimitValue(txtLwLmtFromTarget1.Text) Then
                            If CheckInput(txtUpLmtFromTarget1.Text) And CheckLimitValue(txtUpLmtFromTarget1.Text) Then
                                SaveRecord()
                            Else
                                lblErrMsg.Text = "Please enter Upper Limit From Target (e.g. -123.456789). Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please enter Lower Limit From Target (e.g. -123.456789). Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please select Formula. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please enter Test Specification. Update process aborted!"
                End If
                Else
                    lblErrMsg.Text = "Please select Facility. Update process aborted!"
                End If

        Else
            lblErrMsg.Text = "Please pick one record to Edit or click on Add New to create new note. Update process aborted!"
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

    Protected Sub ddlFormulaDesc_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFormulaDesc"), DropDownList)
        lblFormulaDesc1.Text = ddl.SelectedValue.ToString
    End Sub
End Class