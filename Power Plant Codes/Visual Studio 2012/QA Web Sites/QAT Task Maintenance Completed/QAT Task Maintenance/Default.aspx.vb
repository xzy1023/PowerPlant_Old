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
            BindTaskDescData()
            BindTaskSequenceData()
            BindDefinitionDescData()
            BindNoteDescData()
            ClearInput()
        End If
    End Sub
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
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
    Private Sub BindTaskDescData()
        ddlTaskDescription.DataSource = Me.GetTaskDescData()
        ddlTaskDescription.DataValueField = "TaskID"
        ddlTaskDescription.DataTextField = "TaskDescription"
        ddlTaskDescription.DataBind()
        ddlTaskDescription.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlTaskDescription.SelectedIndex = 0
    End Sub
    Private Sub BindTaskSequenceData()
        ddlTaskSeq.DataSource = Me.GetTaskSequenceData()
        ddlTaskSeq.DataValueField = "TaskSeq"
        ddlTaskSeq.DataTextField = "Description"
        ddlTaskSeq.DataBind()
        ddlTaskSeq.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlTaskSeq.SelectedIndex = 0
    End Sub
    Private Sub BindNoteDescData()
        ddlNoteDescription.DataSource = Me.GetNoteDescData()
        ddlNoteDescription.DataValueField = "NoteID"
        ddlNoteDescription.DataTextField = "NoteDescription"
        ddlNoteDescription.DataBind()
        ddlNoteDescription.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlNoteDescription.SelectedIndex = 0
    End Sub
    Private Sub BindDefinitionDescData()
        ddlQATDefnDescription.DataSource = Me.GetQATDefinitionDescData()
        ddlQATDefnDescription.DataValueField = "QATDefnID"
        ddlQATDefnDescription.DataTextField = "Description"
        ddlQATDefnDescription.DataBind()
        ddlQATDefnDescription.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlQATDefnDescription.SelectedIndex = 0
    End Sub
 
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
    Private Function GetTaskDescData() As DataTable
        Dim str As String = String.Empty
        str = " Select TaskID, TaskDescription " & _
            " From tblQATTaskMaster Where Active=1  Order By TaskDescription"
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblTaskDescription1.Text = r("TaskDescription").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetTaskSequenceData() As DataTable
        Dim dt As DataTable = New DataTable("TaskSeq")
        dt.Columns.Add("Description")
        dt.Columns.Add("TaskSeq")
        For i = 1 To 100
            dt.Rows.Add(i, i)
        Next
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblTaskSeq1.Text = r("Description").ToString
        End If
        Return dt
    End Function
    Private Function GetQATDefinitionDescData() As DataTable
        Dim str As String = String.Empty
        str = "  Select  '*** No Definition' as 'Description', 0 as 'QATDefnID'" & _
            " union " & _
            " SELECT QATDefnDescription + ' - ' + CAST(QATDefnID as varchar(4)) AS 'Description', QATDefnID  " & _
            " FROM tblQATDefinition Where Active=1 Order By Description "
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblQATDefnDescription1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function

    Private Function GetNoteDescData() As DataTable
        Dim str As String = String.Empty
        str = "Select 0 as 'NoteID', '*** No Note' as 'NoteDescription' " & _
            " Union " & _
            " Select NoteID, NoteDescription  From tblQATNote  Where Active=1 Order By NoteDescription "

        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblNoteDescription1.Text = r("NoteDescription").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = String.Empty
        str = "  SELECT  A.Active,A.Facility,A.QATDefnID,A.TaskID,A.TaskSeq,A.UpdatedAt,A.UpdatedBy,A.NoteID, " & _
            " B.TaskDescription, C.NoteDescription, D.QATDefnDescription" & _
            " FROM tblQATTask A " & _
            " Left Outer Join tblQATTaskMaster B On A.TaskID = B.TaskID " & _
            " Left Outer Join tblQATNote C on A.NoteID = C.NoteID " & _
            " Left Outer Join tblQATDefinition D on A.QATDefnID = D.QATDefnID " & _
            " Order By A.TaskID "

        If Query = "" Then
            sql.ExecQuery(str)
        Else
            sql.ExecQuery(Query)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSortData() As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        Dim strSearchText As String = String.Empty
        strSearchText = txtSearch.Text

        str2 = "  SELECT  A.Active,A.Facility,A.QATDefnID,A.TaskID,A.TaskSeq,A.UpdatedAt,A.UpdatedBy,A.NoteID, " & _
          " B.TaskDescription, C.NoteDescription, D.QATDefnDescription" & _
          " FROM tblQATTask A " & _
          " Left Outer Join tblQATTaskMaster B On A.TaskID = B.TaskID " & _
          " Left Outer Join tblQATNote C on A.NoteID = C.NoteID " & _
          " Left Outer Join tblQATDefinition D on A.QATDefnID = D.QATDefnID "

        If strSearchText <> "" Then
            str = str2 & " Where B.TaskDescription like '%" & strSearchText & "%' Order By B.TaskDescription"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By B.TaskDescription"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        str2 = "  SELECT  A.Active,A.Facility,A.QATDefnID,A.TaskID,A.TaskSeq,A.UpdatedAt,A.UpdatedBy,A.NoteID, " & _
            " B.TaskDescription, C.NoteDescription, D.QATDefnDescription" & _
            " FROM tblQATTask A " & _
            " Left Outer Join tblQATTaskMaster B On A.TaskID = B.TaskID " & _
            " Left Outer Join tblQATNote C on A.NoteID = C.NoteID " & _
            " Left Outer Join tblQATDefinition D on A.QATDefnID = D.QATDefnID "

        If strSearchText <> "" Then
            str = str2 & " Where B.TaskDescription like '%" & strSearchText & "%' Order By B.TaskDescription"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By B.TaskDescription"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblTaskID9.Text = "0"
        lblFacility9.Text = "0"
        lblQATDefnID9.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblFacility1.Text = ""
        lblTaskID9.Text = ""
        lblQATDefnDescription1.Text = ""
        lblNoteDescription1.Text = ""
        lblTaskDescription1.Text = ""
        lblTaskSeq1.Text = ""
        lblFacility9.Text = ""
        lblQATDefnID9.Text = ""
        chkActive1.Checked = False
    End Sub

    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0

        Dim ddlQATDefnDescription2 As DropDownList = TryCast(FindControl("ddlQATDefnDescription"), DropDownList)
        ddlQATDefnDescription2.SelectedIndex = 0

        Dim ddlNoteDescription2 As DropDownList = TryCast(FindControl("ddlNoteDescription"), DropDownList)
        ddlNoteDescription2.SelectedIndex = 0

        Dim ddlTaskDescription2 As DropDownList = TryCast(FindControl("ddlTaskDescription"), DropDownList)
        ddlTaskDescription2.SelectedIndex = 0

        Dim ddlTaskSeq2 As DropDownList = TryCast(FindControl("ddlTaskSeq"), DropDownList)
        ddlTaskSeq2.SelectedIndex = 0
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Private Function CheckDuplicateKey(ByVal strFacility As String, ByVal intQATDefnID As Integer, ByVal intTaskID As Integer) As Boolean
        Dim str As String = String.Empty
        str = " Select TaskID  FROM tblQATTask" & _
            " Where Facility = '" & strFacility & "'" & _
            " And QATDefnID = " & intQATDefnID & _
            " And TaskID = " & intTaskID

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Function CheckSameKey(ByVal strFacility As String, ByVal intQATDefnID As Integer, ByVal intTaskID As Integer, ByVal strFacilityNew As String, ByVal intQATDefnIDNew As Integer, ByVal intTaskIDnew As Integer) As Boolean
        If Trim(strFacility) = Trim(strFacilityNew) Then
            If intQATDefnID = intQATDefnIDNew Then
                If intTaskID = intTaskIDnew Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strTaskDescription As String = String.Empty
        Dim strTaskSeq As String = String.Empty
        Dim strQATDefnDescription As String = String.Empty
        Dim strNoteDescription As String = String.Empty

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlTaskDescription2 As DropDownList = TryCast(FindControl("ddlTaskDescription"), DropDownList)
        strTaskDescription = ddlTaskDescription2.SelectedValue.ToString
        If CheckInput(strTaskDescription) = True Then
            lblTaskDescription1.Text = strTaskDescription
        Else
            lblErrMsg.Text = "Please select Task."
            Return False
            Exit Function
        End If

        Dim ddlTaskSeq2 As DropDownList = TryCast(FindControl("ddlTaskSeq"), DropDownList)
        strTaskSeq = ddlTaskSeq2.SelectedValue.ToString
        If CheckInput(strTaskSeq) = True Then
            lblTaskSeq1.Text = strTaskSeq
        Else
            lblErrMsg.Text = "Please select Task Seq."
            Return False
            Exit Function
        End If

        Dim ddlQATDefnDescription2 As DropDownList = TryCast(FindControl("ddlQATDefnDescription"), DropDownList)
        strQATDefnDescription = ddlQATDefnDescription2.SelectedValue.ToString
        If CheckInput(strQATDefnDescription) = True Then
            lblQATDefnDescription1.Text = strQATDefnDescription
        Else
            lblErrMsg.Text = "Please select Definition."
            Return False
            Exit Function
        End If
    
        Dim ddlNoteDescription2 As DropDownList = TryCast(FindControl("ddlNoteDescription"), DropDownList)
        strNoteDescription = ddlNoteDescription2.SelectedValue.ToString
        If CheckInput(strNoteDescription) = True Then
            lblNoteDescription1.Text = strNoteDescription
        Else
            lblErrMsg.Text = "Please select Note."
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub DeleteRecord(ByVal strFacility As String, ByVal intQATDefnID As Integer, ByVal intTaskID As Integer, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATTask " & _
            " Where Facility = '" & strFacility & "'" & _
            " And QATDefnID = " & intQATDefnID & _
            " And TaskID = " & intTaskID

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
    Private Sub UpdateRecord(ByVal intTaskID As String, ByVal intActive As Integer, ByVal strFacility As String, ByVal intTaskSeq As Integer,
                            ByVal intQATDefnID As Integer, ByVal intNoteID As Integer, ByVal strUser As String, ByVal strTaskDescription As String,
                            ByVal intAction As Integer, ByVal intTaskIDCurr As String, ByVal strFacilityCurr As String, ByVal intQATDefnIDCurr As Integer)

        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty

            Select Case intAction
                Case 0
                    If btnSave.Text = "Save New" Then
                        str = "INSERT INTO tblQATTask " & _
                            " (Active, Facility, TaskID, TaskSeq, QATDefnID, NoteID, UpdatedAt, UpdatedBy) " & _
                            " VALUES (@active, @facility, @taskid, @taskseq, @qatdefnid, @noteid, @updatedat, @updatedby) "
                        lblMsg = "Add new record completed!"
                    End If
                Case 1
                    str = "UPDATE tblQATTask SET Active = @active, " & _
                        " TaskSeq=@taskseq, NoteID=@noteid, UpdatedAt=@updatedat, UpdatedBy=@updatedby " & _
                        " WHERE Facility=@facility and TaskID=@taskid and QATDefnID=@qatdefnid "
                    lblMsg = "Update record completed!"
                Case 2
                    str = "UPDATE tblQATTask SET Active = @active, " & _
                        " Facility=@facility, TaskID=@taskid, QATDefnID=@qatdefnid, " & _
                        " TaskSeq=@taskseq, NoteID=@noteid, UpdatedAt=@updatedat, UpdatedBy=@updatedby " & _
                        " WHERE Facility=@facilitycurr and TaskID=@taskidcurr and QATDefnID=@qatdefnidcurr "
                    lblMsg = "Update record completed!"
                Case Else
            End Select

            sql.AddParam("@active", intActive)
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@taskid", intTaskID)
            sql.AddParam("@taskseq", intTaskSeq)
            sql.AddParam("@qatdefnid", intQATDefnID)
            sql.AddParam("@noteid", intNoteID)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)

            sql.AddParam("@taskidcurr", intTaskIDCurr)
            sql.AddParam("@facilitycurr", strFacilityCurr)
            sql.AddParam("@qatdefnidcurr", intQATDefnIDCurr)
            GetData(str)

            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strTaskDescription
                BindSearchData(strTaskDescription)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key") Then
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
            Dim strTaskID9 As String = String.Empty
            Dim strFacility9 As String = String.Empty
            Dim strQATDefnID9 As String = String.Empty
            Dim strFacilityNew As String = String.Empty
            Dim strQATDefnIDNew As String = String.Empty
            Dim strTaskIDNew As String = String.Empty

            Dim strTaskDescription As String = String.Empty
            Dim strQATDefnDescription As String = String.Empty
            Dim strTaskSeq As String = String.Empty
            Dim strUser As String = String.Empty
            Dim strNoteID As String = String.Empty
            Dim intActive As Integer
            Dim intAction As Integer

            If GetCurrentValue() = True Then
                intAction = 0
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If
                strTaskSeq = TryCast(FindControl("lblTaskSeq1"), Label).Text
                strNoteID = TryCast(FindControl("lblNoteDescription1"), Label).Text
                strUser = TryCast(FindControl("lblUserName"), Label).Text
                strQATDefnDescription = ddlQATDefnDescription.SelectedItem.Text
                strTaskDescription = ddlTaskDescription.SelectedItem.Text

                strFacilityNew = TryCast(FindControl("lblFacility1"), Label).Text
                strTaskIDNew = TryCast(FindControl("lblTaskDescription1"), Label).Text
                strQATDefnIDNew = TryCast(FindControl("lblQATDefnDescription1"), Label).Text

                strFacility9 = TryCast(FindControl("lblFacility9"), Label).Text
                strQATDefnID9 = TryCast(FindControl("lblQATDefnID9"), Label).Text
                strTaskID9 = TryCast(FindControl("lblTaskID9"), Label).Text

                If strTaskID9 = "0" Then
                    intAction = 0
                    If CheckDuplicateKey(strFacilityNew, strQATDefnIDNew, strTaskIDNew) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFacilityNew & "/" & strTaskDescription & "/" & strQATDefnDescription & ". Update process aborted!"
                        Exit Sub
                    End If
                Else
                    If CheckSameKey(strFacilityNew, strQATDefnIDNew, strTaskIDNew, strFacility9, strQATDefnID9, strTaskID9) Then
                        intAction = 1
                    Else
                        intAction = 2
                        If CheckDuplicateKey(strFacilityNew, strQATDefnIDNew, strTaskIDNew) Then
                            lblErrMsg.Text = "Duplicate key found - " & strFacilityNew & "/" & strTaskDescription & "/" & strQATDefnDescription & ". Update process aborted!"
                            Exit Sub
                        End If
                    End If
                End If
                UpdateRecord(strTaskIDNew, intActive, Trim(strFacilityNew), strTaskSeq, strQATDefnIDNew, strNoteID, strUser, strTaskDescription, intAction, strTaskID9, Trim(strFacility9), strQATDefnID9)
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
        Dim strQATDefnID As String = String.Empty
        Dim strTaskID As String = String.Empty
        Dim strNoteID As String = String.Empty
        Dim strTaskSeq As String = String.Empty
        Dim intActive As Integer = 0

        lblTaskID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblTaskID"), Label).Text
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
        lblFacility9.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text

        strTaskSeq = TryCast(gvForm.SelectedRow.FindControl("lblTaskSeq"), Label).Text
        lblTaskSeq1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTaskSeq"), Label).Text
        ddlTaskSeq.SelectedIndex = ddlTaskSeq.Items.IndexOf(ddlTaskSeq.Items.FindByValue(strTaskSeq))

        strTaskID = TryCast(gvForm.SelectedRow.FindControl("lblTaskID"), Label).Text
        lblTaskDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTaskID"), Label).Text
        ddlTaskDescription.SelectedIndex = ddlTaskDescription.Items.IndexOf(ddlTaskDescription.Items.FindByValue(strtaskid))
        lblTaskID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblTaskID"), Label).Text

        strQATDefnID = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text
        If CheckInput(strQATDefnID) = False Then
            strQATDefnID = "0"
            lblQATDefnDescription1.Text = "0"
        Else
            lblQATDefnDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text
        End If
        ddlQATDefnDescription.SelectedIndex = ddlQATDefnDescription.Items.IndexOf(ddlQATDefnDescription.Items.FindByValue(strQATDefnID))
        lblQATDefnID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text

        strNoteID = TryCast(gvForm.SelectedRow.FindControl("lblNoteID"), Label).Text
        If CheckInput(strNoteID) = False Then
            strNoteID = "0"
            lblNoteDescription1.Text = "0"
        Else
            lblNoteDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblNoteID"), Label).Text
        End If
        ddlNoteDescription.SelectedIndex = ddlNoteDescription.Items.IndexOf(ddlNoteDescription.Items.FindByValue(strNoteID))
        btnSave.Text = "Save"
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
          If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(3).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(5).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(6).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(3).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(5).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(6).VerticalAlign = VerticalAlign.Top
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
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
        Dim strTaskID9 As String = String.Empty
        Dim intTaskID As Integer
        Dim strDescription As String = String.Empty
        Dim intQATDefnID As Integer = 0
        Dim strFacility As String = String.Empty
        strTaskID9 = TryCast(FindControl("lblTaskID9"), Label).Text
        If CheckInput(strTaskID9) Then
            If strTaskID9 = "0" Then
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            Else
                strDescription = ddlTaskDescription.SelectedItem.Text
                intTaskID = TryCast(FindControl("lblTaskID9"), Label).Text
                strFacility = TryCast(FindControl("lblFacility9"), Label).Text
                intQATDefnID = TryCast(FindControl("lblQATDefnID9"), Label).Text

                DeleteRecord(strFacility, intQATDefnID, intTaskID, strDescription)
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblTaskID9.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(lblTaskDescription1.Text) Then
                    If CheckInput(lblTaskSeq1.Text) Then
                        If CheckInput(lblQATDefnDescription1.Text) Then
                            If CheckInput(lblNoteDescription1.Text) Then
                                SaveRecord()
                            Else
                                lblErrMsg.Text = "Please enter Note. Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please select Definition. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please select Task Sequence. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please select Task. Update process aborted!"
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

    Protected Sub ddlTaskDescription_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlTaskDescription"), DropDownList)
        lblTaskDescription1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ddlTaskSeq_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlTaskSeq"), DropDownList)
        lblTaskSeq1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlQATDefnDescription_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlQATDefnDescription"), DropDownList)
        lblQATDefnDescription1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlNoteDescription_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlNoteDescription"), DropDownList)
        lblNoteDescription1.Text = ddl.SelectedValue.ToString
    End Sub
End Class