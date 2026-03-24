Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim encrypt As New Cryptography
    Dim intRowCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblErrMsg.Text = ""
        If Not IsPostBack Then
            GetCurrentUserName()
            'WO#35671   BindData()
            BindFacilityData()
            ddlFacility.SelectedValue = GetDefaultFacility()            'WO#35671
            lblFacility1.Text = ddlFacility.SelectedValue               'WO#35671
            BindData()  'WO#35671
            BindWorkGroupData()
            BindWorkSubGroupData()
            BindStaffClassData()
            ClearInput()
        End If
    End Sub
    Private Sub BindData()
        'WO#35671   gvForm.DataSource = Me.GetData()
        ViewState("gvFormData") = Me.GetData()                          'WO#35671
        gvForm.DataSource = ViewState("gvFormData")                     'WO#35671
        gvForm.DataBind()
    End Sub
    'WO#35671 DEL Start
    'Private Sub BindUpdatedData()
    '    gvForm.DataSource = Me.GetUpdatedData()
    '    gvForm.DataBind()
    'End Sub
    'WO#35671 DEL Stop
    Private Sub BindSearchData(ByVal strSearch As String)
        gvForm.DataSource = Me.GetSearchData(strSearch)
        gvForm.DataBind()
    End Sub
 
    Private Sub BindFacilityData()
        ddlFacility.DataSource = Me.GetFacilityData()
        ddlFacility.DataValueField = "Facility"
        'WO#35671   ddlFacility.DataTextField = "ShortDescription"
        ddlFacility.DataTextField = "Description"                       'WO#35671 
        ddlFacility.DataBind()
        'WO#35671   ddlFacility.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        'WO#35671   ddlFacility.SelectedIndex = 0
    End Sub
    Private Sub BindWorkGroupData()
        ddlWorkGroup.DataSource = Me.GetWorkGroupData()
        ddlWorkGroup.DataValueField = "WorkGroup"
        ddlWorkGroup.DataTextField = "Description"
        ddlWorkGroup.DataBind()
        ddlWorkGroup.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlWorkGroup.SelectedIndex = 0
    End Sub
    Private Sub BindWorkSubGroupData()
        ddlWorkSubGroup.DataSource = Me.GetWorkSubGroupData()
        ddlWorkSubGroup.DataValueField = "WorkSubGroup"
        ddlWorkSubGroup.DataTextField = "Description"
        ddlWorkSubGroup.DataBind()
        ddlWorkSubGroup.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlWorkSubGroup.SelectedIndex = 0
    End Sub
    Private Sub BindStaffClassData()
        ddlStaffClass.DataSource = Me.GetStaffClassData()
        ddlStaffClass.DataValueField = "UserType"
        ddlStaffClass.DataTextField = "UserType"
        ddlStaffClass.DataBind()
        ddlStaffClass.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlStaffClass.SelectedIndex = 0
    End Sub
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub

    Private Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = String.Empty
        'WO#35671 DEL Start
        'str = "SELECT A.ActiveRecord, A.Facility, A.WorkGroup, A.StaffID, A.FirstName, A.LastName, A.StaffClass, A.Password, " & _
        '    " A.ResetPassword, A.DateLastChange, A.WorkSubGroup, " & _
        '    " B.Description as 'WorkGroupDescription', C.Description as 'WorkSubGroupDescription' " & _
        '    " FROM tblPlantStaff A " & _
        '    " Inner Join tblWorkGroup B ON A.WorkGroup = B.WorkGroup " & _
        '    " Inner Join (Select WorkSubGroup, Description From tblWorkSubGroup Group By WorkSubGroup, Description) as C ON A.WorkSubGroup = C.WorkSubGroup " & _
        '    " Order By A.Facility, A.FirstName, A.LastName "
        'WO#35671 DEL Stop
        If Query = "" Then
            str = CrtSQLStmtForGVData(ddlFacility.SelectedValue)            'WO#35671
            sql.ExecQuery(str)
        Else
            sql.ExecQuery(Query)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function

    'WO#35671 ADD Start
    Private Function CrtSQLStmtForGVData(strFacility As String, Optional strSearchText As String = Nothing) As String
        Dim sb As New StringBuilder
        sb.Append("SELECT A.ActiveRecord, A.Facility, A.WorkGroup, A.StaffID, A.FirstName, A.LastName, A.StaffClass, A.Password, ")
        sb.Append(" A.ResetPassword, A.DateLastChange, A.WorkSubGroup, ")
        sb.Append(" B.Description as 'WorkGroupDescription', C.Description as 'WorkSubGroupDescription' ")
        sb.Append(" FROM tblPlantStaff A ")
        sb.Append(" Inner Join tblWorkGroup B ON A.WorkGroup = B.WorkGroup ")
        sb.Append(" Inner Join (Select WorkSubGroup, Description From tblWorkSubGroup Group By WorkSubGroup, Description) as C ON A.WorkSubGroup = C.WorkSubGroup ")
        sb.AppendFormat(" Where A.Facility = '{0}'", strFacility)
        If Not IsNothing(strSearchText) Then
            sb.AppendFormat(" AND A.FirstName like '%{0}%'", strSearchText)
        End If
        sb.Append(" Order By A.Facility, A.FirstName, A.LastName ")
        Return sb.ToString
    End Function
    Private Sub PostData(strQuery As String)
        sql.ExecQuery(strQuery)
    End Sub
    'WO#35671 ADD Stop
    Private Function GetFacilityData() As DataTable
        Dim str As String = "PPsp_Facility_Sel"
        sql.AddParam("@vchAction", "SelByRegion")
        sql.AddParam("@vchOrderBy", "Desc")
        GetData(str)
        'WO#35671 DEL Start
        'intRowCount = sql.DBDT.Rows.Count
        'If sql.DBDT.Rows.Count > 0 Then
        '    Dim r As DataRow = sql.DBDT.Rows(0)
        '    lblFacility1.Text = r("Facility").ToString
        'End If
        'WO#35671 DEL Stop
        Return sql.DBDT
    End Function

    'WO#35671 ADD Start
    Protected Function GetDefaultFacility() As String
        Dim strCommandText As String = ""
        strCommandText = "Select Value1 from tblControl where [key] = 'Facility'"
        sql.ExecQuery(strCommandText)

        If sql.DBDT.Rows.Count > 0 Then
            Return sql.DBDT.Rows(0).Item("Value1")
        Else
            Return "00"
        End If
    End Function
    'WO#35671 ADD Stop

    Private Function GetWorkGroupData() As DataTable
        Dim str As String = " SELECT WorkGroup, Description FROM tblWorkGroup Group By WorkGroup, Description ORDER BY Description"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblWorkGroup1.Text = r("WorkGroup").ToString
        End If
        Return sql.DBDT
    End Function

    Private Function GetWorkSubGroupData() As DataTable
        Dim str As String = "SELECT [WorkSubGroup], [Description] FROM [tblWorkSubGroup] " & _
                " WHERE Active=1  GROUP BY [WorkSubGroup], [Description]" & _
                " ORDER BY [Description]"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblWorkSubGroup1.Text = r("WorkSubGroup").ToString
        End If
        Return sql.DBDT
    End Function

    Private Function GetStaffClassData() As DataTable
        Dim str As String = " SELECT UserType FROM tblStaffClass ORDER BY UserType"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblStaffClass1.Text = r("UserType").ToString
        End If
        Return sql.DBDT
    End Function
    'WO#35671 DEL Start
    'Private Function GetUpdatedData() As DataTable
    '    Dim str As String = String.Empty
    '    Dim strStaffID As String = String.Empty
    '    strStaffID = TryCast(FindControl("txtStaffID1"), TextBox).Text
    '    If lblFacility9.Text = "0" And lblStaffID9.Text = "0" Then
    '        str = "SELECT A.ActiveRecord, A.Facility, A.WorkGroup, A.StaffID, A.FirstName, A.LastName, A.StaffClass, A.Password, " & _
    '            " A.ResetPassword, A.DateLastChange, A.WorkSubGroup, " & _
    '            " B.Description as 'WorkGroupDescription', C.Description as 'WorkSubGroupDescription' " & _
    '            " FROM	tblPlantStaff A " & _
    '            " Inner Join tblWorkGroup B ON A.WorkGroup = B.WorkGroup " & _
    '            " Inner Join (Select WorkSubGroup, Description From tblWorkSubGroup Group By WorkSubGroup, Description) as C ON A.WorkSubGroup = C.WorkSubGroup " & _
    '            " Where A.StaffID = '" & strStaffID & "'" & _
    '            " Order By A.Facility, A.FirstName, A.LastName "
    '    Else
    '        str = "SELECT A.ActiveRecord, A.Facility, A.WorkGroup, A.StaffID, A.FirstName, A.LastName, A.StaffClass, A.Password, " & _
    '            " A.ResetPassword, A.DateLastChange, A.WorkSubGroup, " & _
    '            " B.Description as 'WorkGroupDescription', C.Description as 'WorkSubGroupDescription' " & _
    '            " FROM	tblPlantStaff A " & _
    '            " Inner Join tblWorkGroup B ON A.WorkGroup = B.WorkGroup " & _
    '            " Inner Join (Select WorkSubGroup, Description From tblWorkSubGroup Group By WorkSubGroup, Description) as C ON A.WorkSubGroup = C.WorkSubGroup " & _
    '            " Order By A.Facility, A.FirstName, A.LastName "
    '    End If
    '    GetData(str)
    '    intRowCount = sql.DBDT.Rows.Count
    '    Return sql.DBDT
    'End Function
    'WO#35671 DEL Stop

    Private Function GetSearchData(ByVal strSearchText As String) As DataTable

        Dim str As String = String.Empty
        'WO#35671 DEL Start
        'Dim str2 As String = String.Empty
        'str2 = "SELECT A.ActiveRecord, A.Facility, A.WorkGroup, A.StaffID, A.FirstName, A.LastName, A.StaffClass, A.Password, " & _
        '    " A.ResetPassword, A.DateLastChange, A.WorkSubGroup, " & _
        '    " B.Description as 'WorkGroupDescription', C.Description as 'WorkSubGroupDescription' " & _
        '    " FROM	tblPlantStaff A " & _
        '    " Inner Join tblWorkGroup B ON A.WorkGroup = B.WorkGroup " & _
        '    " Inner Join (Select WorkSubGroup, Description From tblWorkSubGroup Group By WorkSubGroup, Description) as C ON A.WorkSubGroup = C.WorkSubGroup "

        'If strSearchText <> "" Then
        '    str = str2 & " Where A.FirstName like '%" & strSearchText & "%' Order By A.Facility, A.FirstName, A.LastName "
        '    sql.ExecQuery(str)
        'Else
        '    str = str2 & " Order By A.Facility, A.FirstName, A.LastName "
        '    sql.ExecQuery(str)
        'End If
        'WO#35671 DEL Stop
        'WO#35671 ADD Start
        Str = CrtSQLStmtForGVData(ddlFacility.SelectedValue, strSearchText)            'WO#35671
        sql.ExecQuery(Str)
        'WO#35671 ADD Stop
        intRowCount = sql.DBDT.Rows.Count
        ViewState("gvFormData") = sql.DBDT          'WO#35671
        Return sql.DBDT
    End Function
    'WO#35671 DEL Start
    'Private Function GetSortData() As DataTable
    '    Dim str As String = String.Empty
    '    Dim str2 As String = String.Empty
    '    Dim strSearchText As String = String.Empty
    '    strSearchText = txtSearch.Text

    '    str2 = "SELECT A.ActiveRecord, A.Facility, A.WorkGroup, A.StaffID, A.FirstName, A.LastName, A.StaffClass, A.Password, " & _
    '        " A.ResetPassword, A.DateLastChange, A.WorkSubGroup, " & _
    '        " B.Description as 'WorkGroupDescription', C.Description as 'WorkSubGroupDescription' " & _
    '        " FROM	tblPlantStaff A " & _
    '        " Inner Join tblWorkGroup B ON A.WorkGroup = B.WorkGroup " & _
    '        " Inner Join (Select WorkSubGroup, Description From tblWorkSubGroup Group By WorkSubGroup, Description) as C ON A.WorkSubGroup = C.WorkSubGroup "

    '    If strSearchText <> "" Then
    '        str = str2 & " Where A.FirstName like '%" & strSearchText & "%' Order By A.Facility, A.FirstName, A.LastName "
    '        sql.ExecQuery(str)
    '    Else
    '        str = str2 & " Order By A.Facility, A.FirstName, A.LastName "
    '        sql.ExecQuery(str)
    '    End If
    '    intRowCount = sql.DBDT.Rows.Count
    '    Return sql.DBDT
    'End Function
    'WO#35671 DEL Stop
    Private Sub EnableControl()
        Dim txtStaffID As TextBox = TryCast(FindControl("txtStaffID1"), TextBox)
        txtStaffID.Enabled = True
        txtStaffID.ReadOnly = False

        Dim txtPassword As TextBox = TryCast(FindControl("txtPassword1"), TextBox)
        txtPassword.TextMode = TextBoxMode.SingleLine
        txtPassword.AutoCompleteType = AutoCompleteType.None

        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddl.Enabled = True


    End Sub
    Private Sub DisableControl()
        Dim txtStaffID As TextBox = TryCast(FindControl("txtStaffID1"), TextBox)
        txtStaffID.Enabled = False
        txtStaffID.ReadOnly = True

        Dim txtPassword As TextBox = TryCast(FindControl("txtPassword1"), TextBox)
        txtPassword.TextMode = TextBoxMode.Password

        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddl.Enabled = False
    End Sub
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        EnableControl()
        lblStaffID9.Text = "0"
        lblFacility9.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblStaffID9.Text = ""
        'WO#35671   lblFacility9.Text = ""
        'WO#35671   lblFacility1.Text = ""
        txtStaffID1.Text = ""
        txtFirstName1.Text = ""
        txtLastName1.Text = ""
        lblWorkGroup1.Text = ""
        lblWorkSubGroup1.Text = ""
        lblStaffClass1.Text = ""
        txtPassword1.Text = ""
        lblPassword2.Text = ""
        chkActiveRecord1.Checked = False
        chkResetPassword1.Checked = False
        'WO#35671   txtSearch.Text = ""
    End Sub

    Private Sub ClearDropDownValue()
        'WO#35671   Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        'WO#35671   ddlFacility2.SelectedIndex = 0

        Dim ddlWorkGroup2 As DropDownList = TryCast(FindControl("ddlWorkGroup"), DropDownList)
        ddlWorkGroup2.SelectedIndex = 0

        Dim ddlWorkSubGroup2 As DropDownList = TryCast(FindControl("ddlWorkSubGroup"), DropDownList)
        ddlWorkSubGroup2.SelectedIndex = 0

        Dim ddlStaffClass2 As DropDownList = TryCast(FindControl("ddlStaffClass"), DropDownList)
        ddlStaffClass2.SelectedIndex = 0
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strWorkGroup As String
        Dim strWorkSubGroup As String
        Dim strStaffClass As String

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlWorkGroup2 As DropDownList = TryCast(FindControl("ddlWorkGroup"), DropDownList)
        strWorkGroup = ddlWorkGroup2.SelectedValue.ToString
        If CheckInput(strWorkGroup) = True Then
            lblWorkGroup1.Text = strWorkGroup
        Else
            lblErrMsg.Text = "Please select Work Group."
            Return False
            Exit Function
        End If

        Dim ddlWorkSubGroup2 As DropDownList = TryCast(FindControl("ddlWorkSubGroup"), DropDownList)
        strWorkSubGroup = ddlWorkSubGroup2.SelectedValue.ToString
        If CheckInput(strWorkSubGroup) = True Then
            lblWorkSubGroup1.Text = strWorkSubGroup
        Else
            lblErrMsg.Text = "Please select Work Sub Group."
            Return False
            Exit Function
        End If

        Dim ddlStaffClass2 As DropDownList = TryCast(FindControl("ddlStaffClass"), DropDownList)
        strStaffClass = ddlStaffClass2.SelectedValue.ToString
        If CheckInput(strStaffClass) = True Then
            lblStaffClass1.Text = ddlStaffClass2.SelectedValue.ToString
        Else
            lblErrMsg.Text = "Please select Access Level."
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub DeleteRecord(ByVal strStaffID As String, ByVal strFacility As String, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblPlantStaff  Where Facility = @facility And StaffID = @staffid"
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@staffid", strStaffID)
            PostData(str)                       'WO#35671
            lblMsg = "Delete record completed!"
            'WO#35671   GetData(str)
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
    Private Sub UpdateRecord(ByVal strFacility As String, strStaffID As String, ByVal intActiveRecord As Integer, ByVal strWorkGroup As String, ByVal strWorkSubGroup As String,
                             ByVal strFirstName As String, ByVal strLastName As String, ByVal strPassword As String, ByVal strStaffClass As String, ByVal intResetPassword As Integer)
        Try

            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            If btnSave.Text = "Save New" Then
                str = "INSERT INTO tblPlantStaff " & _
                    " (ActiveRecord, Facility, WorkGroup, StaffID, FirstName, LastName, StaffClass, ResetPassword, DateLastChange, WorkSubGroup, Password) " & _
                    " VALUES (@activerecord, @facility, @workgroup, @staffid, @firstname, @lastname, @staffclass, @resetpassword, @datelastchange, @worksubgroup, @password)"
                lblMsg = "Add new record completed!"
            Else
                str = "UPDATE tblPlantStaff SET ActiveRecord = @activerecord, WorkGroup = @workgroup, FirstName = @firstname, LastName = @lastname," & _
                    " StaffClass = @staffclass, ResetPassword = @resetpassword, DateLastChange = @datelastchange, WorkSubGroup = @WorkSubGroup, Password = @password " & _
                    " WHERE StaffID = @StaffID AND Facility = @Facility"
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@staffid", strStaffID)
            sql.AddParam("@activerecord", intActiveRecord)
            sql.AddParam("@workgroup", strWorkGroup)
            sql.AddParam("@worksubgroup", strWorkSubGroup)
            sql.AddParam("@firstname", strFirstName)
            sql.AddParam("@lastname", strLastName)
            If strPassword = "" Then
                sql.AddParam("@password", DBNull.Value)
            Else
                sql.AddParam("@password", strPassword)
            End If
            sql.AddParam("@staffclass", strStaffClass)
            sql.AddParam("@resetpassword", intResetPassword)
            sql.AddParam("@datelastchange", Now.ToString)

            'WO#35671   GetData(str)
            PostData(str)                       'WO#35671
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strFirstName
                BindSearchData(strFirstName)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strStaffID & ". Update process aborted!"
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
            Dim strFacility9 As String = String.Empty
            Dim strStaffID9 As String = String.Empty
            Dim strFacility As String = String.Empty
            Dim strStaffID As String = String.Empty
            Dim intActiveRecord As Integer
            Dim strWorkGroup As String = String.Empty
            Dim strWorkSubGroup As String = String.Empty
            Dim strFirstName As String = String.Empty
            Dim strLastName As String = String.Empty
            Dim strPassword As String = String.Empty
            Dim strStaffClass As String = String.Empty
            Dim intResetPassword As Integer
            Dim strPassword1 As String = String.Empty

            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActiveRecord1"), CheckBox)
                If IsActive.Checked Then
                    intActiveRecord = 1
                Else
                    intActiveRecord = 0
                End If
                Dim IsReset As CheckBox = TryCast(FindControl("chkResetPassword1"), CheckBox)
                If IsReset.Checked Then
                    intResetPassword = 1
                Else
                    intResetPassword = 0
                End If
                strFacility9 = TryCast(FindControl("lblFacility9"), Label).Text
                strStaffID9 = TryCast(FindControl("lblStaffID9"), Label).Text

                strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                strStaffID = TryCast(FindControl("txtStaffID1"), TextBox).Text
                strFirstName = TryCast(FindControl("txtFirstName1"), TextBox).Text
                strLastName = TryCast(FindControl("txtLastName1"), TextBox).Text
                strWorkGroup = TryCast(FindControl("lblWorkGroup1"), Label).Text
                strWorkSubGroup = TryCast(FindControl("lblWorkSubGroup1"), Label).Text
                strStaffClass = TryCast(FindControl("lblStaffClass1"), Label).Text
                strPassword1 = TryCast(FindControl("txtPassword1"), TextBox).Text
                If CheckInput(strPassword1) = False Then
                    strPassword1 = TryCast(FindControl("lblPassword2"), Label).Text
                End If
                If CheckInput(strPassword1) Then
                    If strPassword1.ToUpper = "NULL" Or strPassword1 = "0" Then
                        strPassword = ""
                    Else
                        strPassword = encrypt.Encrypt(strPassword1)
                    End If
                Else
                    strPassword = ""
                End If

                If strFacility9 = "0" And strStaffID9 = "0" Then
                    If CheckDuplicateKey(strFacility, strStaffID) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strStaffID & "/" & strFirstName & " " & strLastName & ". Update process aborted!"
                        Exit Sub
                    End If
                End If
                UpdateRecord(strFacility, strStaffID, intActiveRecord, strWorkGroup, strWorkSubGroup, strFirstName, strLastName, strPassword, strStaffClass, intResetPassword)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Function IsPasswordValid(ByVal s As String) As Boolean
        For Each c As Char In s
            If String.IsNullOrEmpty(c) Then
                Return False
            Else
                If Integer.TryParse(c, Nothing) Then
                Else
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Protected Function CheckDuplicateKey(ByVal strFacility As String, ByVal strStaffID As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT FirstName FROM tblPlantStaff" & _
            " Where Facility= '" & strFacility & "'" & _
            " And StaffID = '" & strStaffID & "'"

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
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
        Dim strSearch As String = String.Empty
        Dim dt As DataTable                                                     'WO#35671

        gvForm.PageIndex = e.NewPageIndex
        strSearch = txtSearch.Text
        If strSearch = "" Then
            'WO#35671 BindData()
            'WO#35671 ADD Start
            dt = ViewState("gvFormData")
            If Not IsNothing(ViewState("SortExpression")) Then
                dt.DefaultView.Sort = ViewState("SortExpression").ToString
            End If
            gvForm.DataSource = dt
            gvForm.DataBind()
            'WO#35671 ADD Stop
        Else
            gvForm.DataSource = Me.GetSearchData(strSearch)
            gvForm.DataBind()
        End If
        'Unselect the highlighted record                             WO#35671
        gvForm.SelectedIndex = -1                                   'WO#35671
    End Sub

    Protected Sub gvForm_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim strFacility As String = String.Empty
        Dim strStaffID As String = String.Empty
        Dim strFirstName As String = String.Empty
        Dim strLastName As String = String.Empty
        Dim strWorkGroup As String = String.Empty
        Dim strWorkSubGroup As String = String.Empty
        Dim strStaffClass As String = String.Empty
        Dim intActive As Integer = 0
        Dim strWorkGroupDesc As String = String.Empty
        Dim strWorkSubGroupDesc As String = String.Empty
        Dim strPassword As String = String.Empty
        Dim intResetPassword As Integer = 0

        lblStaffID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblStaffID"), Label).Text
        txtStaffID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblStaffID"), Label).Text
        txtFirstName1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFirstName"), Label).Text
        txtLastName1.Text = TryCast(gvForm.SelectedRow.FindControl("lblLastName"), Label).Text
        strPassword = TryCast(gvForm.SelectedRow.FindControl("lblPassword"), Label).Text
        If CheckInput(strPassword) Then
            If UCase(strPassword) = "NULL" Then
                txtPassword1.Text = ""
                lblPassword2.Text = ""
            Else
                txtPassword1.Text = encrypt.Decrypt(strPassword)
                lblPassword2.Text = encrypt.Decrypt(strPassword)
            End If
        Else
            txtPassword1.Text = ""
            lblPassword2.Text = ""
        End If

        Dim IsActive As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbActiveRecord"), CheckBox)
        If IsActive.Checked Then
            intActive = 1
            chkActiveRecord1.Checked = True
        Else
            intActive = 0
            chkActiveRecord1.Checked = False
        End If

        Dim IsReset As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbResetPassword"), CheckBox)
        If IsReset.Checked Then
            intResetPassword = 1
            chkResetPassword1.Checked = True
        Else
            intResetPassword = 0
            chkResetPassword1.Checked = False
        End If

        strFacility = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        lblFacility1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        lblFacility9.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(strFacility))

        strWorkGroup = TryCast(gvForm.SelectedRow.FindControl("lblWorkGroup"), Label).Text
        strWorkGroupDesc = TryCast(gvForm.SelectedRow.FindControl("lblWorkGroupDescription"), Label).Text
        lblWorkGroup1.Text = TryCast(gvForm.SelectedRow.FindControl("lblWorkGroup"), Label).Text
        ddlWorkGroup.SelectedIndex = ddlWorkGroup.Items.IndexOf(ddlWorkGroup.Items.FindByValue(strWorkGroup))

        strWorkSubGroup = TryCast(gvForm.SelectedRow.FindControl("lblWorkSubGroup"), Label).Text
        strWorkSubGroupDesc = TryCast(gvForm.SelectedRow.FindControl("lblWorkSubGroupDescription"), Label).Text
        lblWorkSubGroup1.Text = TryCast(gvForm.SelectedRow.FindControl("lblWorkSubGroup"), Label).Text
        ddlWorkSubGroup.SelectedIndex = ddlWorkSubGroup.Items.IndexOf(ddlWorkSubGroup.Items.FindByValue(strWorkSubGroup))

        strStaffClass = TryCast(gvForm.SelectedRow.FindControl("lblStaffClass"), Label).Text
        lblStaffClass1.Text = TryCast(gvForm.SelectedRow.FindControl("lblStaffClass"), Label).Text
        ddlStaffClass.SelectedIndex = ddlStaffClass.Items.IndexOf(ddlStaffClass.Items.FindByValue(strStaffClass))
        btnSave.Text = "Save"
        DisableControl()
        btnCancel.Focus()                   'WO#35671
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(11).Visible = False
            e.Row.Cells(12).Visible = False
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(11).Visible = False
            e.Row.Cells(12).Visible = False
        End If
    End Sub

    Protected Sub gvForm_Sorting(sender As Object, e As GridViewSortEventArgs)
        Dim sortExpression As String = e.SortExpression
        Dim direction As String = String.Empty
        Dim table As DataTable                                      'WO#35671
        If SortDirection = SortDirection.Ascending Then
            SortDirection = SortDirection.Descending
            direction = " DESC"
        Else
            SortDirection = SortDirection.Ascending
            direction = " ASC"
        End If
        'WO#35671   Dim table As DataTable = Me.GetSortData()
        ViewState("SortExpression") = sortExpression & direction    'WO#35671
        table = ViewState("gvFormData")                             'WO#35671
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
        Dim strStaffID As String = String.Empty
        Dim strFacility As String = String.Empty
        strStaffID = lblStaffID9.Text
        strFacility = lblFacility9.Text
        If CheckInput(strFacility) Then
            If CheckInput(strStaffID) Then
                If strStaffID = "0" And strFacility = "0" Then
                    lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
                Else
                    strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                    strStaffID = TryCast(FindControl("txtStaffID1"), TextBox).Text
                    DeleteRecord(strStaffID, strFacility, txtFirstName1.Text)
                End If
            Else
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs)

        If CheckInput(lblFacility1.Text) Then
            If CheckInput(txtStaffID1.Text) Then
                If CheckInput(txtFirstName1.Text) Then
                    If CheckInput(txtLastName1.Text) Then
                        If CheckInput(lblWorkGroup1.Text) Then
                            If CheckInput(lblWorkSubGroup1.Text) Then
                                If CheckInput(lblStaffClass1.Text) Then
                                    If CheckInput(txtPassword1.Text) Then
                                        If IsPasswordValid(txtPassword1.Text) Then
                                            lblPassword2.Text = txtPassword1.Text
                                            SaveRecord()
                                        Else
                                            lblErrMsg.Text = "Please enter Password - numbers only. Update process aborted!"
                                        End If
                                    Else
                                        If lblStaffClass1.Text = "Supervisor" Then
                                            If CheckInput(lblPassword2.Text) Then
                                                SaveRecord()
                                            Else
                                                lblErrMsg.Text = "Please enter Password - numbers only. Update process aborted!"
                                            End If
                                        Else
                                            SaveRecord()
                                        End If
                                    End If
                                Else
                                    lblErrMsg.Text = "Please select Access Level. Update process aborted!"
                                End If
                            Else
                                lblErrMsg.Text = "Please select Work Sub Group. Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please select Work Group. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please enter Last Name. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please enter First Name. Update process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please enter Staff ID - numbers only. Update process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please select Facility. Update process aborted!"
        End If
    End Sub

    Protected Sub ddlFacility_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        lblFacility1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlWorkGroup_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlWorkGroup"), DropDownList)
        lblWorkGroup1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlWorkSubGroup_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlWorkSubGroup"), DropDownList)
        lblWorkSubGroup1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlStaffClass_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlStaffClass"), DropDownList)
        lblStaffClass1.Text = ddl.SelectedValue.ToString
    End Sub
End Class