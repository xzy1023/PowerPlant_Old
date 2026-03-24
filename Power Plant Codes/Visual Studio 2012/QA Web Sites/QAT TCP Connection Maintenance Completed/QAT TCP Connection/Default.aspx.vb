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
    Private Sub BindUpdatedData()
        gvForm.DataSource = Me.GetUpdatedData()
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
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub
    Private Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = String.Empty
        str = "SELECT 	Active, Facility, Description, IPAddress ,Model ,Port, TCPConnID, " & _
            " Command1, Command2, Command3, UpdatedAt, UpdatedBy FROM tblQATTCPConn " & _
            " Order By Description"
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
    Private Function GetUpdatedData() As DataTable
        Dim str As String = String.Empty
        str = "SELECT 	Active, Facility, Description, IPAddress ,Model ,Port, TCPConnID, " & _
             " Command1, Command2, Command3, UpdatedAt, UpdatedBy FROM tblQATTCPConn " & _
             " Order By Description"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSortData() As DataTable
        Dim strSearchText As String = String.Empty
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        strSearchText = txtSearch.Text

        str2 = "SELECT 	Active, Facility, Description, IPAddress ,Model ,Port, TCPConnID, " & _
            " Command1, Command2, Command3, UpdatedAt, UpdatedBy FROM tblQATTCPConn "

        If strSearchText <> "" Then
            str = str2 & " Where Description like '%" & strSearchText & "%' Order By Description"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By Description "
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        str2 = "SELECT 	Active, Facility, Description, IPAddress ,Model ,Port, TCPConnID, " & _
            " Command1, Command2, Command3, UpdatedAt, UpdatedBy FROM tblQATTCPConn "
  
        If strSearchText <> "" Then
            str = str2 & " Where Description like '%" & strSearchText & "%' Order By Description"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By Description "
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblTCPConnID9.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblTCPConnID9.Text = ""
        lblFacility1.Text = ""
        txtDescription1.Text = ""
        txtIPAddress1.Text = ""
        txtModel1.Text = ""
        txtPort1.Text = ""
        txtCommand11.Text = ""
        txtCommand21.Text = ""
        txtCommand31.Text = ""
        txtSearch.Text = ""
        chkActive1.Checked = False
    End Sub

    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Protected Function CheckDuplicateKey(ByVal intActive As Integer, ByVal strFacility As String, ByVal strIPAddress As String, ByVal strPort As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT Facility FROM tblQATTCPConn" & _
            " Where Active  = " & intActive & _
            " And Facility = '" & strFacility & "'" & _
            " And IPAddress = '" & strIPAddress & "'" & _
            " And Port = " & strPort

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function
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

    Private Sub DeleteRecord(ByVal strTCPConnID As String, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATTCPConn  Where TCPConnID = @tcpconnid"
            sql.AddParam("@tcpconnid", strTCPConnID)
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
    Private Sub UpdateRecord(ByVal strFacility As String, intTCPConnID As Integer, ByVal intActive As Integer, ByVal strDescription As String,
                            ByVal strIPAddress As String, ByVal strModel As String, ByVal strPort As String, ByVal strCommand1 As String,
                            ByVal strCommand2 As String, ByVal strCommand3 As String, ByVal strUser As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty

            If btnSave.Text = "Save New" Then
                str = "INSERT INTO tblQATTCPConn " & _
                    " (Active, Facility, Description, IPAddress ,Model ,Port, Command1, Command2, Command3, UpdatedAt, UpdatedBy) " & _
                    " VALUES (@active, @facility, @description, @ipaddress, @model, @port, @command1, @command2, @command3, @updatedat, @updatedby)"
                lblMsg = "Add new record completed!"
            Else
                str = "UPDATE tblQATTCPConn SET Active = @active, Facility = @Facility, IPAddress=@ipaddress," & _
                    " Description=@description, Model=@model, Port=@port, Command1=@command1, Command2=@command2, " & _
                    " Command3=@command3, UpdatedAt = @updatedat, UpdatedBy = @updatedby" & _
                    " WHERE TCPConnID = @tcpconnid"
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@tcpconnid", intTCPConnID)
            sql.AddParam("@active", intActive)
            sql.AddParam("@description", strDescription)
            sql.AddParam("@ipaddress", strIPAddress)
            sql.AddParam("@model", strModel)
            sql.AddParam("@port", strPort)
            sql.AddParam("@command1", strCommand1)
            sql.AddParam("@command2", strCommand2)
            sql.AddParam("@command3", strCommand3)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)
            GetData(str)

            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strDescription
                BindSearchData(strDescription)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & intTCPConnID.ToString & ". Update process aborted!"
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

            Dim strTCPConnID As String = String.Empty
            Dim strFacility As String = String.Empty
            Dim strDescription As String = String.Empty
            Dim intActive As Integer
            Dim strIPAddress As String = String.Empty
            Dim strModel As String = String.Empty
            Dim strPort As String = String.Empty
            Dim strCommand1 As String = String.Empty
            Dim strCommand2 As String = String.Empty
            Dim strCommand3 As String = String.Empty
            Dim strUser As String = String.Empty
            Dim strTemp1 As String = String.Empty
            Dim strActive As String = String.Empty

            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If
                strTCPConnID = TryCast(FindControl("lblTCPConnID9"), Label).Text
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                strDescription = TryCast(FindControl("txtDescription1"), TextBox).Text
                strTemp1 = TryCast(FindControl("txtIPAddress1"), TextBox).Text
                strIPAddress = strTemp1.Replace(" ", "")
                strModel = TryCast(FindControl("txtModel1"), TextBox).Text
                strPort = TryCast(FindControl("txtPort1"), TextBox).Text
                strCommand1 = TryCast(FindControl("txtCommand11"), TextBox).Text
                strCommand2 = TryCast(FindControl("txtCommand21"), TextBox).Text
                strCommand3 = TryCast(FindControl("txtCommand31"), TextBox).Text
                strUser = TryCast(FindControl("lblUserName"), Label).Text
                If strTCPConnID = "0" Then
                    If CheckDuplicateKey(intActive, strFacility, strIPAddress, strPort) Then
                        If intActive = 1 Then
                            strActive = "Active"
                        Else
                            strActive = "Inactive"
                        End If
                        lblErrMsg.Text = "Duplicate key found - " & strActive & "/" & strFacility & "/" & strIPAddress & "/" & strPort & ". Update process aborted!"
                        Exit Sub
                    End If
                End If
                UpdateRecord(strFacility, strTCPConnID, intActive, strDescription, strIPAddress, strModel, strPort, strCommand1, strCommand2, strCommand3, strUser)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
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
        Dim strFacility As String = String.Empty
        Dim intActive As Integer = 0
        lblTCPConnID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblTCPConnID"), Label).Text
        txtDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDescription"), Label).Text
        txtIPAddress1.Text = TryCast(gvForm.SelectedRow.FindControl("lblIPAddress"), Label).Text
        txtModel1.Text = TryCast(gvForm.SelectedRow.FindControl("lblModel"), Label).Text
        txtPort1.Text = TryCast(gvForm.SelectedRow.FindControl("lblPort"), Label).Text
        txtCommand11.Text = TryCast(gvForm.SelectedRow.FindControl("lblCommand1"), Label).Text
        txtCommand21.Text = TryCast(gvForm.SelectedRow.FindControl("lblCommand2"), Label).Text
        txtCommand31.Text = TryCast(gvForm.SelectedRow.FindControl("lblCommand3"), Label).Text

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
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(10).Visible = False
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(10).Visible = False
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
        Dim strTCPConnID As String = String.Empty
        Dim strDescription As String = String.Empty
        strDescription = txtDescription1.Text
        strTCPConnID = lblTCPConnID9.Text
        If CheckInput(strTCPConnID) Then
            If strTCPConnID = "0" Then
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            Else
                strTCPConnID = TryCast(FindControl("lblTCPConnID9"), Label).Text
                DeleteRecord(strTCPConnID, strDescription)
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblTCPConnID9.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(txtDescription1.Text) Then
                    If IsIPValid(txtIPAddress1.Text) Then
                        If CheckInput(txtPort1.Text) And ToInteger(txtPort1.Text) Then
                            SaveRecord()
                        Else
                            lblErrMsg.Text = "Please enter Port Number. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please enter valid IP Address. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please enter Description. Update process aborted!"
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