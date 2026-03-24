Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim intRowCount As Integer = 0
    Public dtParity As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblErrMsg.Text = ""
        If Not IsPostBack Then
            GetCurrentUserName()
            BindData()
            BindFacilityData()
            BindBaudRateData()
            BindDataBitsData()
            BindStopBitsData()
            BindParityData()
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
    Private Sub BindBaudRateData()
        ddlBaudRate.DataSource = Me.GetBaudRateData()
        ddlBaudRate.DataValueField = "BaudRate"
        ddlBaudRate.DataTextField = "Description"
        ddlBaudRate.DataBind()
        ddlBaudRate.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlBaudRate.SelectedIndex = 0
    End Sub
    Private Sub BindDataBitsData()
        ddlDataBits.DataSource = Me.GetDataBitsData()
        ddlDataBits.DataValueField = "DataBits"
        ddlDataBits.DataTextField = "Description"
        ddlDataBits.DataBind()
        ddlDataBits.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlDataBits.SelectedIndex = 0
    End Sub
    Private Sub BindStopBitsData()
        ddlStopBits.DataSource = Me.GetStopBitsData()
        ddlStopBits.DataValueField = "StopBits"
        ddlStopBits.DataTextField = "Description"
        ddlStopBits.DataBind()
        ddlStopBits.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlStopBits.SelectedIndex = 0
    End Sub
    Private Sub BindParityData()
        ddlParity.DataSource = Me.GetParityData()
        ddlParity.DataValueField = "Parity"
        ddlParity.DataTextField = "Description"
        ddlParity.DataBind()
        ddlParity.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlParity.SelectedIndex = 0
    End Sub

    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub
    Private Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = String.Empty
        str = "SELECT Active, Facility, SerialConnDesc, BaudRate, ComPort, DataBits, Parity, StopBits, UpdatedAt, UpdatedBy, SerialConnID, " & _
          " Case when StopBits = 1 Then 1" & _
          "       when StopBits = 2 Then 2" & _
          "       when StopBits = 3 Then 1.5" & _
          "       Else NULL End AS StopBitsDesc," & _
          " Case when Parity = 0 Then 'None'" & _
          "      when Parity = 1 Then 'Odd'" & _
          "      when Parity = 2 Then 'Even'" & _
          "	     when Parity = 3 Then 'Mark'" & _
          "      when Parity = 4 Then 'Space'" & _
          "      Else NULL End AS ParityDesc " & _
          " From  tblQATSerialConn Order By ComPort, BaudRate "

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
    Private Function GetBaudRateData() As DataTable
        Dim dt As DataTable = New DataTable("BaudRate")
        dt.Columns.Add("Description")
        dt.Columns.Add("BaudRate")
        dt.Rows.Add(110, 110)
        dt.Rows.Add(300, 300)
        dt.Rows.Add(1200, 1200)
        dt.Rows.Add(2400, 2400)
        dt.Rows.Add(4800, 4800)
        dt.Rows.Add(9600, 9600)
        dt.Rows.Add(19200, 19200)
        dt.Rows.Add(38400, 38400)
        dt.Rows.Add(57600, 57600)
        dt.Rows.Add(115200, 115200)
        dt.Rows.Add(230400, 230400)
        dt.Rows.Add(460800, 460800)

        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblBaudRate1.Text = r("Description").ToString
        End If
        Return dt
    End Function
    Private Function GetParityData() As DataTable
        dtParity = New DataTable("Parity")
        dtParity.Columns.Add("Description")
        dtParity.Columns.Add("Parity")
        dtParity.Rows.Add("None", 0)
        dtParity.Rows.Add("Odd", 1)
        dtParity.Rows.Add("Even", 2)
        dtParity.Rows.Add("Mark", 3)
        dtParity.Rows.Add("Space", 4)
        intRowCount = dtParity.Rows.Count
        If dtParity.Rows.Count > 0 Then
            Dim r As DataRow = dtParity.Rows(0)
            lblParity1.Text = r("Description").ToString
        End If
        Return dtParity
    End Function
    Private Function GetDataBitsData() As DataTable
        Dim dt As DataTable = New DataTable("DataBits")
        dt.Columns.Add("Description")
        dt.Columns.Add("DataBits")
        For i = 5 To 8
            dt.Rows.Add(i, i)
        Next

        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblDataBits1.Text = r("Description").ToString
        End If
        Return dt
    End Function

    Private Function GetStopBitsData() As DataTable
        Dim dt As DataTable = New DataTable("StopBits")
        dt.Columns.Add("Description")
        dt.Columns.Add("StopBits")
        dt.Rows.Add(1, 1)
        dt.Rows.Add(1.5, 3)
        dt.Rows.Add(2, 2)
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblStopBits1.Text = r("Description").ToString
        End If
        Return dt
    End Function


    Private Function GetUpdatedData() As DataTable
        Dim str As String = String.Empty
        str = "SELECT Active, Facility, SerialConnDesc, BaudRate, ComPort, DataBits, Parity, StopBits, UpdatedAt, UpdatedBy, SerialConnID, " & _
          " Case when StopBits = 1 Then 1" & _
          "       when StopBits = 2 Then 2" & _
          "       when StopBits = 3 Then 1.5" & _
          "       Else NULL End AS StopBitsDesc," & _
          " Case when Parity = 0 Then 'None'" & _
          "      when Parity = 1 Then 'Odd'" & _
          "      when Parity = 2 Then 'Even'" & _
          "	     when Parity = 3 Then 'Mark'" & _
          "      when Parity = 4 Then 'Space'" & _
          "      Else NULL End AS ParityDesc " & _
          " From  tblQATSerialConn Order By ComPort, BaudRate  "
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSortData() As DataTable
        Dim strSearchText As String = String.Empty
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        strSearchText = txtSearch.Text

        str2 = "SELECT Active, Facility, SerialConnDesc, BaudRate, ComPort, DataBits, Parity, StopBits, UpdatedAt, UpdatedBy, SerialConnID, " & _
          " Case when StopBits = 1 Then 1" & _
          "       when StopBits = 2 Then 2" & _
          "       when StopBits = 3 Then 1.5" & _
          "       Else NULL End AS StopBitsDesc," & _
          " Case when Parity = 0 Then 'None'" & _
          "      when Parity = 1 Then 'Odd'" & _
          "      when Parity = 2 Then 'Even'" & _
          "	     when Parity = 3 Then 'Mark'" & _
          "      when Parity = 4 Then 'Space'" & _
          "      Else NULL End AS ParityDesc " & _
          " From  tblQATSerialConn  "

        If strSearchText <> "" Then
            str = str2 & " Where SerialConnDesc like '%" & strSearchText & "%' Order By ComPort, BaudRate "
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By ComPort, BaudRate  "
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        str2 = "SELECT Active, Facility, SerialConnDesc, BaudRate, ComPort, DataBits, Parity, StopBits, UpdatedAt, UpdatedBy, SerialConnID, " & _
          " Case when StopBits = 1 Then 1" & _
          "       when StopBits = 2 Then 2" & _
          "       when StopBits = 3 Then 1.5" & _
          "       Else NULL End AS StopBitsDesc," & _
          " Case when Parity = 0 Then 'None'" & _
          "      when Parity = 1 Then 'Odd'" & _
          "      when Parity = 2 Then 'Even'" & _
          "	     when Parity = 3 Then 'Mark'" & _
          "      when Parity = 4 Then 'Space'" & _
          "      Else NULL End AS ParityDesc " & _
          " From  tblQATSerialConn  "

        If strSearchText <> "" Then
            str = str2 & " Where SerialConnDesc like '%" & strSearchText & "%' Order By ComPort, BaudRate "
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By ComPort, BaudRate  "
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblSerialConnID9.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblSerialConnID9.Text = ""
        lblFacility1.Text = ""
        lblBaudRate1.Text = ""
        txtComPort1.Text = ""
        lblDataBits1.Text = ""
        lblParity1.Text = ""
        lblStopBits1.Text = ""
        chkActive1.Checked = False
        txtSearch.Text = ""
        txtSerialConnDesc1.Text = ""
    End Sub

    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0

        Dim ddlBaudRate2 As DropDownList = TryCast(FindControl("ddlBaudRate"), DropDownList)
        ddlBaudRate2.SelectedIndex = 0

        Dim ddlParity2 As DropDownList = TryCast(FindControl("ddlParity"), DropDownList)
        ddlParity2.SelectedIndex = 0

        Dim ddlDataBits2 As DropDownList = TryCast(FindControl("ddlDataBits"), DropDownList)
        ddlDataBits2.SelectedIndex = 0

        Dim ddlStopBits2 As DropDownList = TryCast(FindControl("ddlStopBits"), DropDownList)
        ddlStopBits2.SelectedIndex = 0
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Protected Function CheckDuplicateKey(ByVal strSerialConnID As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT SerialConnDesc FROM tblQATSerialConn" & _
            " Where SerialConnID = " & strSerialConnID

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strBaudRate As String = String.Empty
        Dim strDataBits As String = String.Empty
        Dim strStopBits As String = String.Empty
        Dim strParity As String = String.Empty

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlBaudRate2 As DropDownList = TryCast(FindControl("ddlBaudRate"), DropDownList)
        strBaudRate = ddlBaudRate2.SelectedValue.ToString
        If CheckInput(strBaudRate) = True Then
            lblBaudRate1.Text = strBaudRate
        Else
            lblErrMsg.Text = "Please select Baud Rate."
            Return False
            Exit Function
        End If
        Dim ddlParity2 As DropDownList = TryCast(FindControl("ddlParity"), DropDownList)
        strParity = ddlParity2.SelectedValue.ToString
        If CheckInput(strParity) = True Then
            lblParity1.Text = strParity
        Else
            lblErrMsg.Text = "Please select Parity."
            Return False
            Exit Function
        End If
        Dim ddlDataBits2 As DropDownList = TryCast(FindControl("ddlDataBits"), DropDownList)
        strDataBits = ddlDataBits2.SelectedValue.ToString
        If CheckInput(strDataBits) = True Then
            lblDataBits1.Text = strDataBits
        Else
            lblErrMsg.Text = "Please select Data Bits."
            Return False
            Exit Function
        End If

        Dim ddlStopBits2 As DropDownList = TryCast(FindControl("ddlStopBits"), DropDownList)
        strStopBits = ddlStopBits2.SelectedValue.ToString
        If CheckInput(strStopBits) = True Then
            lblStopBits1.Text = strStopBits
        Else
            lblErrMsg.Text = "Please select Stop Bits."
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Shared Function RemoveXtraSpaces(strVal As String) As String
        Dim iCount As Integer = 1
        Dim sTempstrVal As String
        sTempstrVal = ""
        For iCount = 1 To Len(strVal)
            sTempstrVal = sTempstrVal + Mid(strVal, iCount, 1).Trim
        Next
        RemoveXtraSpaces = sTempstrVal
        Return RemoveXtraSpaces
    End Function

    Private Sub DeleteRecord(ByVal strSerialConnID As String, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATSerialConn Where SerialConnID = @serialconnid"
            sql.AddParam("@serialconnid", strSerialConnID)
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
    Private Sub UpdateRecord(ByVal strFacility As String, intSerialConnID As Integer, ByVal intActive As Integer, ByVal strSerialConnDesc As String,
                            ByVal intBaudRate As Integer, ByVal intComPort As Integer, ByVal intDataBits As Integer, ByVal intParity As Integer,
                            ByVal intStopBits As Integer, ByVal strUser As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty

            If btnSave.Text = "Save New" Then
                str = "INSERT INTO tblQATSerialConn " & _
                    " (Active, Facility, SerialConnDesc, BaudRate, ComPort, DataBits, Parity, StopBits, UpdatedAt, UpdatedBy) " & _
                    " VALUES (@active, @facility, @serialconndesc, @baudrate, @comport, @databits, @parity, @stopbits, @updatedat, @updatedby)"
                lblMsg = "Add new record completed!"
            Else
                str = "UPDATE tblQATSerialConn SET Active = @active, Facility = @facility, " & _
                    " SerialConnDesc=@serialconndesc, BaudRate=@baudrate, ComPort=@comport, DataBits=@databits, Parity=@parity, StopBits=@stopbits, " & _
                    "  UpdatedAt = @updatedat, UpdatedBy = @updatedby" & _
                    " WHERE SerialConnID = @serialconnid"
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@serialconnid", intSerialConnID)
            sql.AddParam("@active", intActive)
            sql.AddParam("@serialconndesc", strSerialConnDesc)
            sql.AddParam("@baudrate", intBaudRate)
            sql.AddParam("@comport", intComPort)
            sql.AddParam("@databits", intDataBits)
            sql.AddParam("@parity", intParity)
            sql.AddParam("@stopbits", intStopBits)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)
            GetData(str)

            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strSerialConnDesc
                BindSearchData(strSerialConnDesc)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & intSerialConnID.ToString & ". Update process aborted!"
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
            Dim strSerialConnID As String = String.Empty
            Dim strFacility As String = String.Empty
            Dim strSerialConnDesc As String = String.Empty
            Dim intActive As Integer
            Dim intBaudRate As Integer
            Dim intComPort As Integer
            Dim intDataBits As Integer
            Dim intParity As Integer
            Dim intStopBits As Integer
            Dim strUser As String = String.Empty
            Dim strParityDesc As String = String.Empty
            Dim strDataBitsDesc As String = String.Empty
            Dim strStopBitsDesc As String = String.Empty
            Dim strBaudRate As Integer
            Dim strDataBits As String = String.Empty
            Dim strStopBits As String = String.Empty

            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If
                strSerialConnID = TryCast(FindControl("lblSerialConnID9"), Label).Text
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                intBaudRate = TryCast(FindControl("lblBaudRate1"), Label).Text
                intComPort = TryCast(FindControl("txtComPort1"), TextBox).Text
                intDataBits = TryCast(FindControl("lblDataBits1"), Label).Text
                intParity = TryCast(FindControl("lblParity1"), Label).Text
                intStopBits = TryCast(FindControl("lblStopBits1"), Label).Text
                strUser = TryCast(FindControl("lblUserName"), Label).Text

                strParityDesc = TryCast(FindControl("lblParity1"), Label).Text
                strDataBitsDesc = TryCast(FindControl("lblDataBits1"), Label).Text
                strSerialConnDesc = TryCast(FindControl("txtSerialConnDesc1"), TextBox).Text
                strBaudRate = ddlBaudRate.SelectedItem.Text
                strParityDesc = ddlParity.SelectedItem.Text
                strDataBits = ddlDataBits.SelectedItem.Value
                strStopBitsDesc = ddlStopBits.SelectedItem.Text
                If CheckInput(strSerialConnDesc) = False Then
                    strSerialConnDesc = strBaudRate & "," & Left(strParityDesc, 1) & "," & strDataBits & "," & strStopBitsDesc
                End If
                If strSerialConnID = "0" Then
                    If CheckDuplicateKey(strSerialConnID) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strSerialConnID & ". Update process aborted!"
                        Exit Sub
                    End If
                End If
                UpdateRecord(strFacility, strSerialConnID, intActive, Trim(strSerialConnDesc), intBaudRate, intComPort, intDataBits, intParity, intStopBits, strUser)
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
        Dim strBaudRate As String = String.Empty
        Dim strDataBits As String = String.Empty
        Dim strStopBits As String = String.Empty
        Dim strParity As String = String.Empty

        Dim strFacility As String = String.Empty
        Dim intActive As Integer = 0
        lblSerialConnID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblSerialConnID"), Label).Text
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

        txtComPort1.Text = TryCast(gvForm.SelectedRow.FindControl("lblComPort"), Label).Text

        strBaudRate = TryCast(gvForm.SelectedRow.FindControl("lblBaudRate"), Label).Text
        lblBaudRate1.Text = TryCast(gvForm.SelectedRow.FindControl("lblBaudRate"), Label).Text
        ddlBaudRate.SelectedIndex = ddlBaudRate.Items.IndexOf(ddlBaudRate.Items.FindByValue(strBaudRate))

        strDataBits = TryCast(gvForm.SelectedRow.FindControl("lblDataBits"), Label).Text
        lblDataBits1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDataBits"), Label).Text
        ddlDataBits.SelectedIndex = ddlDataBits.Items.IndexOf(ddlDataBits.Items.FindByValue(strDataBits))

        strStopBits = TryCast(gvForm.SelectedRow.FindControl("lblStopBits"), Label).Text
        lblStopBits1.Text = TryCast(gvForm.SelectedRow.FindControl("lblStopBits"), Label).Text
        ddlStopBits.SelectedIndex = ddlStopBits.Items.IndexOf(ddlStopBits.Items.FindByValue(strStopBits))

        strParity = TryCast(gvForm.SelectedRow.FindControl("lblParity"), Label).Text
        lblParity1.Text = TryCast(gvForm.SelectedRow.FindControl("lblParity"), Label).Text
        ddlParity.SelectedIndex = ddlParity.Items.IndexOf(ddlParity.Items.FindByValue(strParity))

        txtSerialConnDesc1.Text = TryCast(gvForm.SelectedRow.FindControl("lblSerialConnDesc"), Label).Text

        btnSave.Text = "Save"
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False
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
        Dim strSerialConnID As String = String.Empty
        Dim strSerialConnDesc As String = String.Empty

        strSerialConnID = lblSerialConnID9.Text
        strSerialConnDesc = txtSerialConnDesc1.Text

        If CheckInput(strSerialConnID) Then
            If strSerialConnID = "0" Then
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            Else
                strSerialConnID = TryCast(FindControl("lblSerialConnID9"), Label).Text
                DeleteRecord(strSerialConnID, strSerialConnDesc)
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If CheckInput(lblSerialConnID9.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(txtComPort1.Text) Then
                    If CheckInput(lblBaudRate1.Text) Then
                        If CheckInput(lblParity1.Text) Then
                            If CheckInput(lblDataBits1.Text) Then
                                If CheckInput(lblStopBits1.Text) Then
                                    SaveRecord()
                                Else
                                    lblErrMsg.Text = "Please enter Stop Bits. Update process aborted!"
                                End If
                            Else
                                lblErrMsg.Text = "Please enter Data Bits. Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please enter Parity. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please enter Baud Rate. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please enter Com Port. Update process aborted!"
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

    Protected Sub ddlBaudRate_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlBaudRate"), DropDownList)
        lblBaudRate1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlParity_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlParity"), DropDownList)
        lblParity1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlDataBits_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlDataBits"), DropDownList)
        lblDataBits1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlStopBits_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlStopBits"), DropDownList)
        lblStopBits1.Text = ddl.SelectedValue.ToString
    End Sub
End Class