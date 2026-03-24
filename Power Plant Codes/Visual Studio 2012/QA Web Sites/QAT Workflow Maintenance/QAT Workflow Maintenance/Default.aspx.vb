Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim intRowCount As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strCurrentFacility As String = String.Empty
        lblErrMsg.Text = ""
        If Not IsPostBack Then
            GetCurrentUserName()
            strCurrentFacility = GetDefaultFacility()
            BindData()
            BindFacilityData(strCurrentFacility)
            BindPackagingLineData(strCurrentFacility)
            BindDefinitionData()
            BindTCPConnectionData()
            BindSerialConnectionData()
            BindTestSequenceData()
            BindWorkFlowPackagingLineData(strCurrentFacility)
            BindQATEntryPointData()
            BindExceptionData(strCurrentFacility)
            ClearInput()
        End If
    End Sub

    Private Sub BindData()
        'execute the default SQL statement to get all the data to display on the screen.
        gvForm.DataSource = Me.ExeSQLStmt()
        gvForm.DataBind()
    End Sub
    Private Sub BindNewPackagingLineData(ByVal strSearch As String, ByVal strSearch1 As String)
        gvForm.DataSource = Me.GetNewPackagingLineData(strSearch, strSearch1)
        gvForm.DataBind()
    End Sub
    Private Sub BindQATEntryPointData()
        '        ddlEntryPointSearch.DataSource = Me.GetQATEntryPointData()
        ddlEntryPointSearch.DataSource = Me.GetWindowTypeData()
        ddlEntryPointSearch.DataValueField = "QATEntryPoint"
        ddlEntryPointSearch.DataTextField = "Description"
        ddlEntryPointSearch.DataBind()
        ddlEntryPointSearch.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlEntryPointSearch.SelectedIndex = 0
    End Sub
    Private Sub BindWindowTypeData()
        ddlEntryPointSearch.DataSource = Me.GetWindowTypeData()
        ddlEntryPointSearch.DataValueField = "QATEntryPoint"
        ddlEntryPointSearch.DataTextField = "Description"
        ddlEntryPointSearch.DataBind()
        ddlEntryPointSearch.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlEntryPointSearch.SelectedIndex = 0
    End Sub

    Private Sub BindFacilityData(strFacilty As String)
        ddlFacility.DataSource = Me.GetFacilityData()
        ddlFacility.DataValueField = "Facility"
        ddlFacility.DataTextField = "ShortDescription"
        ddlFacility.DataBind()
        'ddlFacility.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        'ddlFacility.SelectedIndex = 0
        ddlFacility.SelectedValue = strFacilty

        With ddlFacilitySearch
            .DataSource = Me.GetFacilityData()
            .DataValueField = "Facility"
            .DataTextField = "ShortDescription"
            .DataBind()
            '.Items.Insert(0, New ListItem(String.Empty, String.Empty))
            '.SelectedIndex = 0
            .SelectedValue = strFacilty
        End With
    End Sub
    Private Sub BindPackagingLineData(strFacility As String)
        ddlPackagingLine.DataSource = Me.GetPackagingLineData(strFacility)
        ddlPackagingLine.DataValueField = "EquipmentID"
        ddlPackagingLine.DataTextField = "Description"
        ddlPackagingLine.DataBind()
        ddlPackagingLine.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlPackagingLine.SelectedIndex = 0

        ddlPackagingLineTo.DataSource = Me.GetPackagingLineData(strFacility)
        ddlPackagingLineTo.DataValueField = "EquipmentID"
        ddlPackagingLineTo.DataTextField = "Description"
        ddlPackagingLineTo.DataBind()
        ddlPackagingLineTo.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlPackagingLineTo.SelectedIndex = 0
    End Sub
    Private Sub BindWorkFlowPackagingLineData(strFacility As String)
        ddlPackagingLineFrom.DataSource = Me.GetWorkFlowPackagingLineData(strFacility)
        ddlPackagingLineFrom.DataValueField = "EquipmentID"
        ddlPackagingLineFrom.DataTextField = "Description"
        ddlPackagingLineFrom.DataBind()
        ddlPackagingLineFrom.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlPackagingLineFrom.SelectedIndex = 0

        ddlPackagingLineDelete.DataSource = Me.GetWorkFlowPackagingLineData(strFacility)
        ddlPackagingLineDelete.DataValueField = "EquipmentID"
        ddlPackagingLineDelete.DataTextField = "Description"
        ddlPackagingLineDelete.DataBind()
        ddlPackagingLineDelete.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlPackagingLineDelete.SelectedIndex = 0

        ddlPackagingLineSearch.DataSource = Me.GetWorkFlowPackagingLineData(strFacility)
        ddlPackagingLineSearch.DataValueField = "EquipmentID"
        ddlPackagingLineSearch.DataTextField = "Description"
        ddlPackagingLineSearch.DataBind()
        ddlPackagingLineSearch.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlPackagingLineSearch.SelectedIndex = 0

    End Sub
    Private Sub BindDefinitionData()
        ddlQATDefnID.DataSource = Me.GetQATDefinitionData()
        ddlQATDefnID.DataValueField = "QATDefnID"
        ddlQATDefnID.DataTextField = "Description"
        ddlQATDefnID.DataBind()
        ddlQATDefnID.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlQATDefnID.SelectedIndex = 0
    End Sub

    Private Sub BindTCPConnectionData()
        ddlTCPConnID.DataSource = Me.GetTCPConnectionData()
        ddlTCPConnID.DataValueField = "TCPConnID"
        ddlTCPConnID.DataTextField = "Description"
        ddlTCPConnID.DataBind()
        ddlTCPConnID.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlTCPConnID.SelectedIndex = 0
    End Sub
    Private Sub BindSerialConnectionData()
        ddlSerialConnID.DataSource = Me.GetSerialConnectionData()
        ddlSerialConnID.DataValueField = "SerialConnID"
        ddlSerialConnID.DataTextField = "Description"
        ddlSerialConnID.DataBind()
        ddlSerialConnID.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlSerialConnID.SelectedIndex = 0
    End Sub
    Private Sub BindTestSequenceData()
        ddlTestSeq.DataSource = Me.GetTestSequenceData()
        ddlTestSeq.DataValueField = "TestSeq"
        ddlTestSeq.DataTextField = "Description"
        ddlTestSeq.DataBind()
        ddlTestSeq.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlTestSeq.SelectedIndex = 0
    End Sub
    Private Sub BindExceptionData(strFacility As String)
        With ddlExceptionCode
            .DataSource = Me.GetExceptionCodeData(strFacility)
            .DataValueField = "ExceptionCode"
            .DataTextField = "Description"
            .DataBind()
            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0
        End With
    End Sub
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub
    Private Function GetSearchQuery(ByVal strSearchText As String, ByVal strSearchText1 As String) As String
        Dim str1 As String = String.Empty
        Dim str2 As String = String.Empty
        If strSearchText = "" And strSearchText1 = "" Then
            str1 = str2 & " ORDER BY A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq"
        Else
            If strSearchText <> "" And strSearchText1 <> "" Then
                str1 = str2 & " Where A.PackagingLine = '" & strSearchText & "' And B.QATEntryPoint = '" & strSearchText1 & "' ORDER BY A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq"
            End If
            If strSearchText <> "" And strSearchText1 = "" Then
                str1 = str2 & " Where A.PackagingLine = '" & strSearchText & "' ORDER BY A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq"
            End If
            If strSearchText = "" And strSearchText1 <> "" Then
                str1 = str2 & " Where B.QATEntryPoint = '" & strSearchText1 & "' ORDER BY A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq"
            End If
        End If
        Return str1
    End Function

    Private Function GetSortData() As DataTable
        Dim str As String = String.Empty
        Dim str1 As String = String.Empty
        Dim str2 As String = String.Empty

        Dim strSearchText = String.Empty
        Dim strSearchText1 = String.Empty
        strSearchtext = txtSearch.Text
        strSearchText1 = TxtSearch1.Text

        'str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
        '    " B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
        '    " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
        '    " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc, " & _
        '    " B.QATEntryPoint, Case When B.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When B.QATEntryPoint ='I' Then 'In Process' " & _
        '    " When B.QATEntryPoint ='C' Then 'Closed' " & _
        '    " When B.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc " & _
        '    " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
        '    " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
        '    " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
        '    " LEFT OUTER JOIN " & _
        '    "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
        '    "    FROM tblEquipment" & _
        '    "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
        '    "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
            " A.ExceptionCode, B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
            " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
            " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc, " & _
            " B.QATEntryPoint, tEP.EntryPointDescription as EntryPointDesc " & _
            " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
            " LEFT OUTER JOIN tblQATEntryPoint tEP On B.QATEntryPoint = tEP.QATEntryPoint  " & _
            " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
            " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
            " LEFT OUTER JOIN " & _
            "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
            "    FROM tblEquipment" & _
            "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
            "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        str = str2 & GetSearchQuery(strSearchText, strSearchText1)
        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function

    Private Function ExeSQLStmt(Optional Query As String = "") As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        Dim strSearchText = String.Empty
        Dim strSearchText1 = String.Empty
        strSearchText = txtSearch.Text
        strSearchText1 = txtSearch1.Text

        'str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
        '    " B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
        '    " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
        '    " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc, " & _
        '    " B.QATEntryPoint, Case When B.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When B.QATEntryPoint ='I' Then 'In Process' " & _
        '    " When B.QATEntryPoint ='C' Then 'Closed' " & _
        '    " When B.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc " & _
        '    " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
        '    " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
        '    " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
        '    " LEFT OUTER JOIN " & _
        '    "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
        '    "    FROM tblEquipment" & _
        '    "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
        '    "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        If Query = "" Then

            str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
                " A.ExceptionCode, B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
                " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
                " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc, " & _
                " B.QATEntryPoint, tEP.EntryPointDescription as EntryPointDesc " & _
                " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
                " LEFT OUTER JOIN tblQATEntryPoint tEP On B.QATEntryPoint = tEP.QATEntryPoint  " & _
                " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
                " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
                " LEFT OUTER JOIN " & _
                "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
                "    FROM tblEquipment" & _
                "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
                "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

            str = str2 & GetSearchQuery(strSearchText, strSearchText1)
            sql.ExecQuery(str)
        Else
            sql.ExecQuery(Query)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function

    Private Function GetSearchData(ByVal strSearchText As String, ByVal strSearchText1 As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        'str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
        '    " B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
        '    " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
        '    " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc," & _
        '    " B.QATEntryPoint, Case When B.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When B.QATEntryPoint ='I' Then 'In Process' " & _
        '    " When B.QATEntryPoint ='C' Then 'Closed' " & _
        '    " When B.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc " & _
        '    " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
        '    " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
        '    " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
        '    " LEFT OUTER JOIN " & _
        '    "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
        '    "    FROM tblEquipment" & _
        '    "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
        '    "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
            " A.ExceptionCode, B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
            " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
            " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc," & _
            " B.QATEntryPoint, tEP.EntryPointDescription as EntryPointDesc " & _
            " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
            " LEFT OUTER JOIN tblQATEntryPoint tEP On B.QATEntryPoint = tEP.QATEntryPoint  " & _
            " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
            " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
            " LEFT OUTER JOIN " & _
            "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
            "    FROM tblEquipment" & _
            "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
            "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        str = str2 & GetSearchQuery(strSearchText, strSearchText1)
        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetNewPackagingLineData(ByVal strSearchText As String, ByVal strSearchText1 As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        'str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
        '    " B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
        '    " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
        '    " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc," & _
        '    " B.QATEntryPoint, Case When B.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When B.QATEntryPoint ='I' Then 'In Process' " & _
        '    " When B.QATEntryPoint ='C' Then 'Closed' " & _
        '    " When B.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc " & _
        '    " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
        '    " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
        '    " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
        '    " LEFT OUTER JOIN " & _
        '    "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
        '    "    FROM tblEquipment" & _
        '    "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
        '    "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        str2 = "SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq, A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, " & _
            " A.ExceptionCode, B.QATDefnDescription + ' - ' + cast(B.QATDefnID as varchar(4)) as 'QATDefnDesc', " & _
            " RTRIM(LTRIM(C.Description)) + ' - ' + CAST(C.TCPConnID AS varchar(4)) AS 'TCPConnDesc', " & _
            " D.SerialConnDesc + ' - ' + cast(D.SerialConnID as varchar(4)) as 'SerialConnDesc', E.PackagingLineDesc," & _
            " B.QATEntryPoint, tEP.EntryPointDescription as EntryPointDesc " & _
            " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID  " & _
            " LEFT OUTER JOIN tblQATEntryPoint tEP On B.QATEntryPoint = tEP.QATEntryPoint  " & _
            " LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
            " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID  " & _
            " LEFT OUTER JOIN " & _
            "   (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID" & _
            "    FROM tblEquipment" & _
            "    WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')" & _
            "	 AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID "

        str = str2 & GetSearchQuery(strSearchText, strSearchText1)
        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetQATEntryPointData() As DataTable
        Dim str As String = String.Empty

        'Dim dt As DataTable = New DataTable("QATEntryPoint")
        'dt.Columns.Add("Description")
        'dt.Columns.Add("QATEntryPoint")
        'dt.Rows.Add("Start Up", "S")
        'dt.Rows.Add("In Process", "I")
        'dt.Rows.Add("On Request", "O")
        'dt.Rows.Add("Closed", "C")

        str = "SELECT QATEntryPoint, EntryPointDescription as Description FROM tblQATEntryPoint"
        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblQATEntryPoint1.Text = r("QATEntryPoint").ToString
        End If
        'Return dt
        Return sql.DBDT
    End Function
    Private Function GetWindowTypeData() As DataTable
        Dim dt As DataTable = New DataTable("QATEntryPoint")
        Dim str As String = String.Empty
        Dim strSearch As String = String.Empty
        strSearch = txtSearch.Text
        If strSearch <> "" Then
            'str = " SELECT Distinct  B.QATEntryPoint, " & _
            '    " Case When B.QATEntryPoint ='S' Then 'Start Up'  When B.QATEntryPoint ='I' Then 'In Process'  " & _
            '    " When B.QATEntryPoint ='C' Then 'Closed'  When B.QATEntryPoint ='O' Then 'On Request'  Else 'Unknown' End as 'Description'" & _
            '    " FROM tblQATWorkFlow A LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID   LEFT OUTER JOIN tblQATTCPConn C On A.TCPConnID = C.TCPConnID  " & _
            '    " LEFT OUTER JOIN tblQATSerialConn D On A.SerialConnID = D.SerialConnID   LEFT OUTER JOIN    (SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) as 'PackagingLineDesc', RTRIM(LTRIM(EquipmentID)) as EquipmentID " & _
            '    " FROM tblEquipment    " & _
            '    " WHERE Active = 1 AND EquipmentID not in ('SPARE', 'Tote')  AND Type ='P') as E  On RTRIM(LTRIM(A.PackagingLine))  = E.EquipmentID " & _
            '    " Where  A.PackagingLine ='" & strSearch & "'"
            str = " SELECT Distinct B.QATEntryPoint, " & _
                " tEP.EntryPointDescription as 'Description'" & _
                " FROM tblQATWorkFlow A " & _
                " LEFT OUTER JOIN tblQATDefinition B On A.QATDefnID = B.QATDefnID " & _
                " LEFT OUTER JOIN tblQATEntryPoint tEP On B.QATEntryPoint = tEP.QATEntryPoint  " & _
                " Where  A.PackagingLine ='" & strSearch & "'"
            ExeSQLStmt(str)
            intRowCount = sql.DBDT.Rows.Count
            If sql.DBDT.Rows.Count > 0 Then
                Dim r As DataRow = sql.DBDT.Rows(0)
                lblQATDefnID1.Text = r("Description").ToString
            End If
            Return sql.DBDT
        Else
            dt = GetQATEntryPointData()
            'dt.Columns.Add("Description")
            'dt.Columns.Add("QATEntryPoint")
            'dt.Rows.Add("Start Up", "S")
            'dt.Rows.Add("In Process", "I")
            'dt.Rows.Add("On Request", "O")
            'dt.Rows.Add("Closed", "C")
            intRowCount = dt.Rows.Count
            Return dt
        End If
    End Function
    Private Function GetPackagingLineData(strFacility As String) As DataTable
        Dim str As String = String.Empty
        str = "SELECT [Description] + ' - ' + RTRIM(LTRIM(EquipmentID)) AS 'Description', RTRIM(LTRIM(EquipmentID)) AS EquipmentID" & _
                " FROM tblEquipment" & _
                " Where Active = 1 And Type ='P' AND EquipmentID not in ('SPARE', 'Tote')" & _
                " AND Facility = '" & strFacility & "'" & _
                " Order By Description"
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblQATDefnID1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetWorkFlowPackagingLineData(strFacility As String) As DataTable
        Dim str As String = String.Empty
        str = "SELECT Distinct A.Description + ' - ' + RTRIM(LTRIM(A.EquipmentID)) AS 'Description', RTRIM(LTRIM(A.EquipmentID)) AS EquipmentID" & _
            " FROM  tblEquipment A " & _
            " Inner Join tblQATWorkFlow B ON A.Facility = B.Facility AND A.EquipmentID = B.PackagingLine " & _
            " Where A.Active = 1 And Type ='P' AND A.EquipmentID not in ('SPARE', 'Tote') " & _
            " AND A.Facility = '" & strFacility & "'" & _
            " Order By Description"
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblQATDefnID1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function

    Private Function GetDefaultFacility() As String
        Dim strDefaultFacility As String = String.Empty
        Dim str As String = String.Empty
        str = "SELECT * FROM tblControl where [Key] = 'Facility'"
        'Dim str As String = "PPsp_Control_Sel"

        'sql.AddParam("@vchKey", "Facility")
        'sql.AddParam("@vchSubKey", "General")
        'sql.AddParam("@vchAction", "ByKey")

        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            strDefaultFacility = r("Value1").ToString
        End If
        Return strDefaultFacility
    End Function

    Private Function GetFacilityData() As DataTable
        Dim str As String = "PPsp_Facility_Sel"
        sql.AddParam("@vchAction", "SelByRegion")
        sql.AddParam("@vchOrderBy", "Desc")
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblFacility1.Text = r("Facility").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetQATDefinitionData() As DataTable
        Dim str As String = String.Empty
        str = "SELECT QATDefnDescription + ' - ' + CAST(QATDefnID as varchar(4)) AS 'Description', QATDefnID " & _
            "  FROM tblQATDefinition Where Active=1 Order By QATDefnDescription"
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblQATDefnID1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function

    Private Function GetTCPConnectionData() As DataTable
        Dim str As String = String.Empty
        str = "SELECT RTRIM(LTRIM(Description)) + ' - ' + CAST(TCPConnID AS varchar(4)) AS Description, " & _
            " TCPConnID AS TCPConnID" & _
            " FROM tblQATTCPConn Where Active=1 Order By Description "
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblTCPConnID1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetSerialConnectionData() As DataTable
        Dim str As String = String.Empty
        str = "SELECT SerialConnDesc + ' - ' + cast(SerialConnID as varchar(4)) AS Description, SerialConnID AS SerialConnID " & _
            " FROM tblQATSerialConn Where Active=1 Order By SerialConnDesc"
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblSerialConnID1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetTestSequenceData() As DataTable
        Dim dt As DataTable = New DataTable("TestSeq")
        dt.Columns.Add("Description")
        dt.Columns.Add("TestSeq")
        For i = 1 To 100
            dt.Rows.Add(i, i)
        Next
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblTestSeq1.Text = r("Description").ToString
        End If
        Return dt
    End Function

    Private Function GetExceptionCodeData(strFacility As String) As DataTable
        Dim str As String = String.Empty
        str = "SELECT ExceptionCode, Description " & _
            " FROM tblQATExceptionCode Where Active=1 AND Facility='" & strFacility & "' " & _
            " Order By Description"
        ExeSQLStmt(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblExceptionCode1.Text = r("Description").ToString
        End If
        Return sql.DBDT
    End Function

    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        EnableControlEdit()
        EnableControlEdit2()
        lblPackagingLine9.Text = "0"
        lblQATDefnID9.Text = "0"
        lblFacility9.Text = "0"

        btnSave.Text = "Save New"
        DisableControlCopy()
        DisableControlDelete()
        EnableCheckboxCopy()
        EnableCheckboxDelete()
        EnableButtonSave()
        DisableButtonDelete()
    End Sub
    Private Sub ClearTextBox()
        lblFacility1.Text = ""
        lblPackagingLine9.Text = ""
        lblQATDefnID9.Text = ""
        lblFacility9.Text = ""
        lblPackagingLine1.Text = ""
        lblQATDefnID1.Text = ""
        lblTestSeq1.Text = ""
        lblSerialConnID1.Text = ""
        lblTCPConnID1.Text = ""
        lblQATEntryPoint1.Text = ""
        chkActive1.Checked = False
    End Sub

    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0

        Dim ddlQATDefnID2 As DropDownList = TryCast(FindControl("ddlQATDefnID"), DropDownList)
        ddlQATDefnID2.SelectedIndex = 0

        Dim ddlTCPConnID2 As DropDownList = TryCast(FindControl("ddlTCPConnID"), DropDownList)
        ddlTCPConnID2.SelectedIndex = 0

        Dim ddlSerialConnID2 As DropDownList = TryCast(FindControl("ddlSerialConnID"), DropDownList)
        ddlSerialConnID2.SelectedIndex = 0

        Dim ddlPackagingLine2 As DropDownList = TryCast(FindControl("ddlPackagingLine"), DropDownList)
        ddlPackagingLine2.SelectedIndex = 0

        Dim ddlPackagingLine3 As DropDownList = TryCast(FindControl("ddlPackagingLineFrom"), DropDownList)
        ddlPackagingLine3.SelectedIndex = 0

        Dim ddlPackagingLine4 As DropDownList = TryCast(FindControl("ddlPackagingLineTo"), DropDownList)
        ddlPackagingLine4.SelectedIndex = 0

        Dim ddlPackagingLine5 As DropDownList = TryCast(FindControl("ddlPackagingLineDelete"), DropDownList)
        ddlPackagingLine5.SelectedIndex = 0

        Dim ddlPackagingLine6 As DropDownList = TryCast(FindControl("ddlPackagingLineSearch"), DropDownList)
        ddlPackagingLine6.SelectedIndex = 0

        Dim ddlTestSeq2 As DropDownList = TryCast(FindControl("ddlTestSeq"), DropDownList)
        ddlTestSeq2.SelectedIndex = 0

        Dim ddlExceptionCode As DropDownList = TryCast(FindControl("ddlExceptionCode"), DropDownList)
        ddlExceptionCode.SelectedIndex = 0
    End Sub
    Private Sub EnableControlEdit()
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddl.Enabled = True
        ddl.BackColor = System.Drawing.Color.White

        Dim ddl2 As DropDownList = TryCast(FindControl("ddlQATDefnID"), DropDownList)
        ddl2.Enabled = True
        ddl2.BackColor = System.Drawing.Color.White

        Dim ddl3 As DropDownList = TryCast(FindControl("ddlPackagingLine"), DropDownList)
        ddl3.Enabled = True
        ddl3.BackColor = System.Drawing.Color.White

        Dim ddl4 As DropDownList = TryCast(FindControl("ddlTestSeq"), DropDownList)
        ddl4.Enabled = True
        ddl4.BackColor = System.Drawing.Color.White

        Dim ddl5 As DropDownList = TryCast(FindControl("ddlExceptionCode"), DropDownList)
        ddl5.Enabled = True
        ddl5.BackColor = System.Drawing.Color.White
    End Sub
    Private Sub EnableControlEdit2()
        Dim ddl As DropDownList = TryCast(FindControl("ddlTCPConnID"), DropDownList)
        ddl.Enabled = True
        ddl.BackColor = System.Drawing.Color.White

        Dim ddl2 As DropDownList = TryCast(FindControl("ddlSerialConnID"), DropDownList)
        ddl2.Enabled = True
        ddl2.BackColor = System.Drawing.Color.White

        Dim chk2 As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
        chk2.Enabled = True
    End Sub
    Private Sub DisableControlEdit()
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddl.Enabled = False
        ddl.BackColor = System.Drawing.Color.LightGray

        Dim ddl2 As DropDownList = TryCast(FindControl("ddlQATDefnID"), DropDownList)
        ddl2.Enabled = False
        ddl2.BackColor = System.Drawing.Color.LightGray

        Dim ddl3 As DropDownList = TryCast(FindControl("ddlPackagingLine"), DropDownList)
        ddl3.Enabled = False
        ddl3.BackColor = System.Drawing.Color.LightGray

        Dim ddl4 As DropDownList = TryCast(FindControl("ddlTestSeq"), DropDownList)
        ddl4.Enabled = False
        ddl4.BackColor = System.Drawing.Color.LightGray

        Dim ddl5 As DropDownList = TryCast(FindControl("ddlExceptionCode"), DropDownList)
        ddl5.Enabled = False
        ddl5.BackColor = System.Drawing.Color.LightGray
    End Sub
    Private Sub DisableControlEdit2()
        Dim ddl As DropDownList = TryCast(FindControl("ddlTCPConnID"), DropDownList)
        ddl.Enabled = False
        ddl.BackColor = System.Drawing.Color.LightGray

        Dim ddl2 As DropDownList = TryCast(FindControl("ddlSerialConnID"), DropDownList)
        ddl2.Enabled = False
        ddl2.BackColor = System.Drawing.Color.LightGray

        Dim chk2 As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
        chk2.Enabled = False
        chk2.Checked = False
    End Sub
    Private Sub EnableControlCopy()
        Dim ddl6 As DropDownList = TryCast(FindControl("ddlPackagingLineFrom"), DropDownList)
        ddl6.Enabled = True
        ddl6.SelectedIndex = 0
        ddl6.BackColor = System.Drawing.Color.Cyan

        Dim ddl7 As DropDownList = TryCast(FindControl("ddlPackagingLineTo"), DropDownList)
        ddl7.Enabled = True
        ddl7.SelectedIndex = 0
        ddl7.BackColor = System.Drawing.Color.Cyan
    End Sub
    Private Sub EnableCheckboxCopy()
        Dim chk As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
        chk.Enabled = True
        chk.Checked = False
    End Sub
    Private Sub DisableCheckboxCopy()
        Dim chk As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
        chk.Enabled = False
        chk.Checked = False
    End Sub
    Private Sub DisableControlCopy()
        Dim chk As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
        chk.Enabled = True
        chk.Checked = False

        Dim ddl6 As DropDownList = TryCast(FindControl("ddlPackagingLineFrom"), DropDownList)
        ddl6.Enabled = False
        ddl6.SelectedIndex = 0
        ddl6.BackColor = System.Drawing.Color.LightGray

        Dim ddl7 As DropDownList = TryCast(FindControl("ddlPackagingLineTo"), DropDownList)
        ddl7.Enabled = False
        ddl7.SelectedIndex = 0
        ddl7.BackColor = System.Drawing.Color.LightGray
    End Sub
    Private Sub EnableControlDelete()
        Dim ddl6 As DropDownList = TryCast(FindControl("ddlPackagingLineDelete"), DropDownList)
        ddl6.Enabled = True
        ddl6.SelectedIndex = 0
        ddl6.BackColor = System.Drawing.Color.Cyan
    End Sub
    Private Sub DisableControlDelete()
        Dim chk As CheckBox = TryCast(FindControl("chkDeleteRecord1"), CheckBox)
        chk.Enabled = True
        chk.Checked = False

        Dim ddl6 As DropDownList = TryCast(FindControl("ddlPackagingLineDelete"), DropDownList)
        ddl6.Enabled = False
        ddl6.SelectedIndex = 0
        ddl6.BackColor = System.Drawing.Color.LightGray
    End Sub
    Private Sub DisableCheckboxDelete()
        Dim chk As CheckBox = TryCast(FindControl("chkDeleteRecord1"), CheckBox)
        chk.Enabled = False
        chk.Checked = False
    End Sub
    Private Sub EnableCheckboxDelete()
        Dim chk As CheckBox = TryCast(FindControl("chkDeleteRecord1"), CheckBox)
        chk.Enabled = True
        chk.Checked = False
    End Sub
    Private Sub EnableButtonDelete()
        Dim btn As Button = TryCast(FindControl("btnDelete"), Button)
        btn.Enabled = True
    End Sub
    Private Sub DisableButtonDelete()
        Dim btn As Button = TryCast(FindControl("btnDelete"), Button)
        btn.Enabled = False
    End Sub
    Private Sub EnableButtonSave()
        Dim btn As Button = TryCast(FindControl("btnSave"), Button)
        btn.Enabled = True
    End Sub
    Private Sub DisableButtonSave()
        Dim btn As Button = TryCast(FindControl("btnSave"), Button)
        btn.Enabled = False
    End Sub
    Private Sub EnableControlCopyRecord()
        If chkCopyRecord1.Checked = True Then
            DisableControlEdit()
            DisableControlEdit2()
            EnableControlCopy()
            DisableControlDelete()
            EnableButtonSave()
            DisableButtonDelete()
            lblErrMsg.Text = "Please select Packaging Line from highlighted combobox"
        Else
            EnableControlEdit()
            EnableControlEdit2()
            DisableControlCopy()
            DisableControlDelete()
            EnableButtonSave()
            DisableButtonDelete()
            lblErrMsg.Text = ""
        End If
    End Sub
    Private Sub EnableControlDeleteRecord()
        If chkDeleteRecord1.Checked = True Then
            DisableControlCopy()
            DisableControlEdit()
            DisableControlEdit2()
            EnableControlDelete()
            DisableButtonSave()
            EnableButtonDelete()
            lblErrMsg.Text = "Please select Packaging Line from highlighted combobox"
        Else
            EnableControlEdit()
            EnableControlEdit2()
            DisableControlCopy()
            DisableControlDelete()
            EnableButtonSave()
            DisableButtonDelete()
            lblErrMsg.Text = ""
        End If
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function

    Private Function CheckSameKey(ByVal strFacility As String, ByVal strPackagingLine As String, ByVal strQATDefnID As String, ByVal strFacilityNew As String, ByVal strPackagingLineNew As String, ByVal strQATDefnIDNew As String) As Boolean
        If Trim(strFacility) = Trim(strFacilityNew) Then
            If Trim(strPackagingLine) = Trim(strPackagingLineNew) Then
                If strQATDefnID = strQATDefnIDNew Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckDuplicateKey(ByVal strFacility As String, ByVal strPackagingLine As String, ByVal strQATDefnID As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT PackagingLine FROM tblQATWorkFlow" & _
            " Where Facility = '" & strFacility & "'" & _
            " And PackagingLine = '" & strPackagingLine & "'" & _
            " And QATDefnID = " & strQATDefnID

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Function CheckPackagingLine(ByVal strPackagingLine As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT PackagingLine FROM tblQATWorkFlow" & _
            " Where PackagingLine = '" & strPackagingLine & "'"

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function GetQATEntryPoint(ByVal strTemp As String) As String
        Dim strEntryPoint As String = String.Empty
        Dim strTemp1 As String = String.Empty
        Dim intPos As Integer
        Dim intPos1 As Integer

        intPos = InStrRev(strTemp, "-")
        strTemp1 = Left(strTemp, intPos - 1)
        intPos1 = InStrRev(strTemp1, "-")
        If intPos1 > 1 Then
            strEntryPoint = RemoveXtraSpaces(Right(strTemp1, strTemp1.Length - intPos1))
        Else
            strEntryPoint = ""
            Return ""
        End If
        Select Case UCase(strEntryPoint)
            Case "STARTUP"
                Return "S"
            Case "INPROCESS"
                Return "I"
            Case "ONREQUEST"
                Return "O"
            Case Else
                Return "C"
        End Select
    End Function
    Public Function RemoveXtraSpaces(strVal As String) As String
        Dim iCount As Integer = 1
        Dim sTempstrVal As String
        sTempstrVal = ""
        For iCount = 1 To Len(strVal)
            sTempstrVal = sTempstrVal + Mid(strVal, iCount, 1).Trim
        Next
        RemoveXtraSpaces = sTempstrVal
        Return RemoveXtraSpaces
    End Function


    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strQATDefnID As String = String.Empty
        Dim strTCPConnID As String = String.Empty
        Dim strSerialConnID As String = String.Empty
        Dim strTestSeq As String = String.Empty
        Dim strPackagingLine As String = String.Empty

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlPackagingLine2 As DropDownList = TryCast(FindControl("ddlPackagingLine"), DropDownList)
        strPackagingLine = ddlPackagingLine2.SelectedValue.ToString
        If CheckInput(strPackagingLine) = True Then
            lblPackagingLine1.Text = strPackagingLine
        Else
            lblErrMsg.Text = "Please select Packaging Line."
            Return False
            Exit Function
        End If

        Dim ddlQATDefnID2 As DropDownList = TryCast(FindControl("ddlQATDefnID"), DropDownList)
        strQATDefnID = ddlQATDefnID2.SelectedValue.ToString
        If CheckInput(strQATDefnID) = True Then
            lblQATDefnID1.Text = strQATDefnID
        Else
            lblErrMsg.Text = "Please select QAT Definition."
            Return False
            Exit Function
        End If

        Dim ddlTestSeq2 As DropDownList = TryCast(FindControl("ddlTestSeq"), DropDownList)
        strTestSeq = ddlTestSeq2.SelectedValue.ToString
        If CheckInput(strTestSeq) = True Then
            lblTestSeq1.Text = strTestSeq
        Else
            lblErrMsg.Text = "Please select Test Seq."
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Function GetCurrentValuePackagingLine(ByVal strAction As String) As Boolean
        Dim strPackagingLine1 As String = String.Empty
        Dim strPackagingLine2 As String = String.Empty
        Dim strPackagingLine3 As String = String.Empty
        Select Case strAction
            Case "AddNew"
                Dim ddlPackagingLine1 As DropDownList = TryCast(FindControl("ddlPackagingLineFrom"), DropDownList)
                strPackagingLine1 = ddlPackagingLine1.SelectedValue.ToString
                If CheckInput(strPackagingLine1) = True Then
                    lblPackagingLine1.Text = strPackagingLine1
                Else
                    lblErrMsg.Text = "Please select Packaging Line (From)."
                    Return False
                    Exit Function
                End If

                Dim ddlPackagingLine2 As DropDownList = TryCast(FindControl("ddlPackagingLineTo"), DropDownList)
                strPackagingLine2 = ddlPackagingLine2.SelectedValue.ToString
                If CheckInput(strPackagingLine2) = True Then
                    lblPackagingLineTo1.Text = strPackagingLine2
                Else
                    lblErrMsg.Text = "Please select Packaging Line (To)."
                    Return False
                    Exit Function
                End If

                If strPackagingLine1 = strPackagingLine2 Then
                    lblErrMsg.Text = "Packaging Line (From) should not be same as Packaging Line (To)."
                    Return False
                    Exit Function
                End If
            Case "Delete"
                Dim ddlPackagingLine3 As DropDownList = TryCast(FindControl("ddlPackagingLineDelete"), DropDownList)
                strPackagingLine3 = ddlPackagingLine3.SelectedValue.ToString
                If CheckInput(strPackagingLine3) = True Then
                    lblPackagingLine1.Text = strPackagingLine3
                Else
                    lblErrMsg.Text = "Please select Packaging Line."
                    Return False
                    Exit Function
                End If
            Case Else
                lblErrMsg.Text = "Please contact Administrator for unexpected error."
                Return False
                Exit Function
        End Select
        Return True
    End Function

    Private Sub SearchRecord()
        Dim strSearch As String = String.Empty
        Dim strSearch1 As String = String.Empty
        strSearch = txtSearch.Text
        strSearch1 = txtSearch1.Text
        If strSearch = "" And strSearch1 = "" Then
            BindData()
        Else
            gvForm.DataSource = Me.GetSearchData(strSearch, strSearch1)
            gvForm.DataBind()
        End If

    End Sub
    Private Sub DeleteRecord(ByVal strFacility As String, ByVal strPackagingLine As String, ByVal intQATDefnID As Integer, ByVal strSearch1 As String)
        Try
            Dim intCount As Integer = 0
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATWorkFlow " & _
            " Where Facility = @facility " & _
            " And PackagingLine = @packagingline " & _
            " And QATDefnID = @qatdefnid"

            sql.AddParam("@facility", strFacility)
            sql.AddParam("@packagingline", strPackagingLine)
            sql.AddParam("@qatdefnid", intQATDefnID)

            lblMsg = "Delete record completed!"
            ExeSQLStmt(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                BindWorkFlowPackagingLineData(ddlFacilitySearch.SelectedValue.ToString)
                txtSearch.Text = strPackagingLine
                txtSearch1.Text = strSearch1
                ddlPackagingLineSearch.SelectedIndex = ddlPackagingLineSearch.Items.IndexOf(ddlPackagingLineSearch.Items.FindByValue(strPackagingLine))
                ddlEntryPointSearch.SelectedIndex = ddlEntryPointSearch.Items.IndexOf(ddlEntryPointSearch.Items.FindByValue(strSearch1))
                BindNewPackagingLineData(strPackagingLine, strSearch1)
                lblErrMsg.Text = lblMsg
            Else
                lblErrMsg.Text = strMsg
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub DeletePackagingLine(ByVal strSearch1 As String)
        Try
            Dim strPackagingLineDelete As String = String.Empty
            Dim intDeleteRecord As Integer

            If GetCurrentValuePackagingLine("Delete") = True Then

                Dim IsDeleteRecord As CheckBox = TryCast(FindControl("chkDeleteRecord1"), CheckBox)
                If IsDeleteRecord.Checked Then
                    intDeleteRecord = 1
                Else
                    intDeleteRecord = 0
                End If
                If intDeleteRecord = 1 Then
                    strPackagingLineDelete = TryCast(FindControl("lblPackagingLineDelete1"), Label).Text
                    If CheckPackagingLine(strPackagingLineDelete) = False Then
                        lblErrMsg.Text = "Packaging Line - " & strPackagingLineDelete & " not found. Delete process aborted!"
                        Exit Sub
                    End If
                    DeleteRecordPackagingLine(strPackagingLineDelete, strSearch1)
                End If
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub DeleteRecordPackagingLine(ByVal strPackagingLineDelete As String, ByVal strSearch1 As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty

            str = "Delete  From tblQATWorkFlow" & _
              " Where PackagingLine=@packagingline "

            sql.AddParam("@PackagingLine", strPackagingLineDelete)

            lblMsg = "Delete Packaging Line completed!"

            ExeSQLStmt(str)
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                BindWorkFlowPackagingLineData(ddlFacilitySearch.SelectedValue.ToString)
                BindQATEntryPointData()
                txtSearch.Text = strPackagingLineDelete
                txtSearch1.Text = strSearch1
                BindNewPackagingLineData(strPackagingLineDelete, strSearch1)

                lblErrMsg.Text = lblMsg
            Else
                lblErrMsg.Text = strMsg
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub UpdateRecord(ByVal strFacility As String, strQATDefnID As String, ByVal intActive As Integer, ByVal strPackagingLine As String,
                            ByVal strTestSeq As String, ByVal strSerialConnID As String, ByVal strTCPConnID As String, ByVal strUser As String,
                            ByVal intExceptionCode As Integer, ByVal intAction As Integer, ByVal strFacilityCurr As String, strQATDefnIDCurr As String,
                            ByVal strPackagingLineCurr As String, ByVal strSearch1 As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            Select Case intAction
                Case 0
                    If btnSave.Text = "Save New" Then
                        str = "INSERT INTO tblQATWorkFlow " & _
                            " (Active, Facility, PackagingLine, QATDefnID, TestSeq, SerialConnID, TCPConnID, UpdatedAt, UpdatedBy, ExceptionCode) " & _
                            " VALUES (@active, @facility, @packagingline, @qatdefnid, @testseq, @serialconnid, @tcpconnid, @updatedat, @updatedby, @exceptioncode)"
                        lblMsg = "Add new record completed!"
                    End If
                Case 1
                    str = "UPDATE tblQATWorkFlow SET Active = @active, " & _
                        " Facility=@facility, PackagingLine=@packagingline, QATDefnID=@qatdefnid, TestSeq=@testseq," & _
                        " SerialConnID=@serialconnid, TCPConnID=@tcpconnid, UpdatedAt = @updatedat, UpdatedBy = @updatedby, ExceptionCode = @exceptioncode " & _
                        " WHERE Facility=@facility AND PackagingLine=@packagingline AND QATDefnID=@qatdefnid"
                    lblMsg = "Update record completed!"
                Case 2
                    str = "UPDATE tblQATWorkFlow SET Active = @active, " & _
                        " Facility=@facility, PackagingLine=@packagingline, QATDefnID=@qatdefnid, TestSeq=@testseq," & _
                        " SerialConnID=@serialconnid, TCPConnID=@tcpconnid, UpdatedAt = @updatedat, UpdatedBy = @updatedby, ExceptionCode = @exceptioncode " & _
                        " WHERE Facility=@facilitycurr AND PackagingLine=@packaginglinecurr AND QATDefnID=@qatdefnidcurr"
                    lblMsg = "Update record completed!"

                Case Else

            End Select
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@qatdefnid", CInt(strQATDefnID))
            sql.AddParam("@active", intActive)
            sql.AddParam("@PackagingLine", strPackagingLine)
            sql.AddParam("@testseq", CInt(strTestSeq))
            If strSerialConnID = "" Then
                sql.AddParam("@serialconnid", DBNull.Value)
            Else
                sql.AddParam("@serialconnid", CInt(strSerialConnID))
            End If
            If strTCPConnID = "" Then
                sql.AddParam("@tcpconnid", DBNull.Value)
            Else
                sql.AddParam("@tcpconnid", CInt(strTCPConnID))
            End If
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)
            sql.AddParam("@exceptioncode", intExceptionCode)

            sql.AddParam("@facilitycurr", strFacilityCurr)
            sql.AddParam("@qatdefnidcurr", CInt(strQATDefnIDCurr))
            sql.AddParam("@PackagingLinecurr", strPackagingLineCurr)

            'execute the above constructed SQL statement to update/insert a record.
            ExeSQLStmt(str)

            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strPackagingLine
                txtSearch1.Text = strSearch1
                BindWorkFlowPackagingLineData(ddlFacilitySearch.SelectedValue.ToString)
                ddlPackagingLineSearch.SelectedIndex = ddlPackagingLineSearch.Items.IndexOf(ddlPackagingLineSearch.Items.FindByValue(strPackagingLine))
                ddlEntryPointSearch.SelectedIndex = ddlEntryPointSearch.Items.IndexOf(ddlEntryPointSearch.Items.FindByValue(strSearch1))

                BindNewPackagingLineData(strPackagingLine, strSearch1)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key") Then
                    lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strPackagingLine & "/" & strQATDefnID & "/" & strTestSeq & ". Update process aborted!"
                    Exit Sub
                Else
                    lblErrMsg.Text = strMsg
                End If
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub UpdateRecordNewPackagingLine(ByVal strPackagingLineFrom As String, ByVal strPackagingLineTo As String, ByVal strUser As String, ByVal strSearch1 As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            str = "Insert into tblQATWorkFlow (Active,Facility,PackagingLine,QATDefnID,SerialConnID,TestSeq,TCPConnID,UpdatedAt,UpdatedBy,ExceptionCode) " & _
                " SELECT Active,Facility,'" & strPackagingLineTo & "' as PackagingLine,QATDefnID,SerialConnID,TestSeq," & _
                " TCPConnID, GetDate()" & ",'" & strUser & "'" & ",ExceptionCode " & _
                " FROM tblQATWorkFlow " & _
                " Where PackagingLine=@packagingline "
            lblMsg = "Copy new Packaging Line completed!"

            sql.AddParam("@PackagingLine", strPackagingLineFrom)
            'execute the above constructed SQL statement to copy a set records from one packaging line to another packaging line.
            ExeSQLStmt(str)

            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strPackagingLineTo
                txtSearch1.Text = strSearch1
                BindWorkFlowPackagingLineData(ddlFacilitySearch.SelectedValue.ToString)
                ddlPackagingLineSearch.SelectedIndex = ddlPackagingLineSearch.Items.IndexOf(ddlPackagingLineSearch.Items.FindByValue(strPackagingLineTo))
                ddlEntryPointSearch.SelectedIndex = ddlEntryPointSearch.Items.IndexOf(ddlEntryPointSearch.Items.FindByValue(strSearch1))
                BindNewPackagingLineData(strPackagingLineTo, strSearch1)

                EnableControlCopyRecord()
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key") Then
                    lblErrMsg.Text = "Duplicate key found - " & strPackagingLineTo & ". Update process aborted!"
                    Exit Sub
                Else
                    lblErrMsg.Text = strMsg
                End If
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub SaveRecordNewPackagingLine()
        Try
            Dim strPackagingLineFrom As String = String.Empty
            Dim strPackagingLineTo As String = String.Empty
            Dim strUser As String = String.Empty
            Dim intCopyRecord As Integer
            Dim strSearch1 As String

            If GetCurrentValuePackagingLine("AddNew") = True Then
                Dim IsCopyRecord As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
                If IsCopyRecord.Checked Then
                    intCopyRecord = 1
                Else
                    intCopyRecord = 0
                End If
                If intCopyRecord = 1 Then
                    strPackagingLineFrom = TryCast(FindControl("lblPackagingLineFrom1"), Label).Text
                    strPackagingLineTo = TryCast(FindControl("lblPackagingLineTo1"), Label).Text
                    strUser = TryCast(FindControl("lblUserName"), Label).Text
                    strSearch1 = ""
                    If CheckPackagingLine(strPackagingLineTo) Then
                        lblErrMsg.Text = "New Packaging Line already exist - " & strPackagingLineTo & ". Update process aborted!"
                        Exit Sub
                    End If
                    UpdateRecordNewPackagingLine(strPackagingLineFrom, strPackagingLineTo, strUser, strSearch1)
                End If
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub SaveRecord()
        Try
            Dim strPackagingLine9 As String = String.Empty
            Dim strQATDefnID9 As String = String.Empty
            Dim strFacility9 As String = String.Empty

            Dim strPackagingLineNew As String = String.Empty
            Dim strQATDefnIDNew As String = String.Empty
            Dim strTestSeqNew As String = String.Empty
            Dim strFacilityNew As String = String.Empty
            Dim strSerialConnID As String = String.Empty
            Dim strTCPConnID As String = String.Empty
            Dim strUser As String = String.Empty
            Dim intExceptionCode As Integer
            Dim intAction As Integer
            Dim intActive As Integer
            Dim strSearch1 As String
      
            If GetCurrentValue() = True Then
         
                intAction = 0
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If

                strQATDefnID9 = TryCast(FindControl("lblQATDefnID9"), Label).Text
                strPackagingLine9 = TryCast(FindControl("lblPackagingLine9"), Label).Text

                strFacilityNew = TryCast(FindControl("lblFacility1"), Label).Text
                strPackagingLineNew = TryCast(FindControl("lblPackagingLine1"), Label).Text
                strQATDefnIDNew = TryCast(FindControl("lblQATDefnID1"), Label).Text
                strTestSeqNew = TryCast(FindControl("lblTestSeq1"), Label).Text

                strSerialConnID = TryCast(FindControl("lblSerialConnID1"), Label).Text
                strTCPConnID = TryCast(FindControl("lblTCPConnID1"), Label).Text
                strUser = TryCast(FindControl("lblUserName"), Label).Text
                Integer.TryParse(TryCast(FindControl("lblExceptionCode1"), Label).Text.Trim, intExceptionCode)

                strQATDefnID9 = TryCast(FindControl("lblQATDefnID9"), Label).Text
                strPackagingLine9 = TryCast(FindControl("lblPackagingLine9"), Label).Text
                strFacility9 = TryCast(FindControl("lblFacility9"), Label).Text

                If strQATDefnID9 = "0" And strPackagingLine9 = "0" Then
                    'if strQATDefnID9 = "0" And strPackagingLine9 = "0", means a record has not been picked from the grid view
                    If CheckDuplicateKey(strFacilityNew, strPackagingLineNew, strQATDefnIDNew) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFacilityNew & "/" & strPackagingLineNew & "/" & strQATDefnIDNew & ". Update process aborted!"
                        Exit Sub
                    End If
                    strSearch1 = TryCast(FindControl("lblQATEntryPoint1"), Label).Text
                Else
                    If CheckSameKey(strFacilityNew, strPackagingLineNew, strQATDefnIDNew, strFacility9, strPackagingLine9, strQATDefnID9) Then
                        intAction = 1
                    Else
                        intAction = 2
                        If CheckDuplicateKey(strFacilityNew, strPackagingLineNew, strQATDefnIDNew) Then
                            lblErrMsg.Text = "Duplicate key found - " & strFacilityNew & "/" & strPackagingLineNew & "/" & strQATDefnIDNew & ". Update process aborted!"
                            Exit Sub
                        End If
                    End If
                    strSearch1 = TryCast(FindControl("lblQATEntryPoint1"), Label).Text
                End If
                UpdateRecord(Trim(strFacilityNew), strQATDefnIDNew, intActive, strPackagingLineNew, strTestSeqNew, strSerialConnID, strTCPConnID, strUser, _
                             intExceptionCode, intAction, Trim(strFacility9), strQATDefnID9, strPackagingLine9, strSearch1)
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
        Dim strSearch1 As String = String.Empty
        strSearch = txtSearch.Text
        strSearch1 = txtSearch1.Text
        If strSearch = "" And strSearch = "" Then
            BindData()
        Else
            gvForm.DataSource = Me.GetSearchData(strSearch, strSearch1)
            gvForm.DataBind()
        End If
    End Sub

    Protected Sub gvForm_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim strFacility As String = String.Empty
        Dim strQATDefnID As String = String.Empty
        Dim strTCPConnID As String = String.Empty
        Dim strSerialConnID As String = String.Empty
        Dim strPackagingLine As String = String.Empty
        Dim strTestSeq As String = String.Empty
        Dim intExceptionCode As Integer
        Dim intActive As Integer = 0

        'Save the packaging line, QAT Definition, facility of the selected row from the grid
        lblPackagingLine9.Text = TryCast(gvForm.SelectedRow.FindControl("lblPackagingLine"), Label).Text
        lblQATDefnID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text
        lblFacility9.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text

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

        strPackagingLine = TryCast(gvForm.SelectedRow.FindControl("lblPackagingLine"), Label).Text
        lblPackagingLine1.Text = TryCast(gvForm.SelectedRow.FindControl("lblPackagingLine"), Label).Text
        ddlPackagingLine.SelectedIndex = ddlPackagingLine.Items.IndexOf(ddlPackagingLine.Items.FindByValue(RemoveXtraSpaces(strPackagingLine)))

        strQATDefnID = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text
        lblQATDefnID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text
        ddlQATDefnID.SelectedIndex = ddlQATDefnID.Items.IndexOf(ddlQATDefnID.Items.FindByValue(strQATDefnID))

        strTCPConnID = TryCast(gvForm.SelectedRow.FindControl("lblTCPConnID"), Label).Text
        lblTCPConnID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTCPConnID"), Label).Text
        ddlTCPConnID.SelectedIndex = ddlTCPConnID.Items.IndexOf(ddlTCPConnID.Items.FindByValue(strTCPConnID))

        strSerialConnID = TryCast(gvForm.SelectedRow.FindControl("lblSerialConnID"), Label).Text
        lblSerialConnID1.Text = TryCast(gvForm.SelectedRow.FindControl("lblSerialConnID"), Label).Text
        ddlSerialConnID.SelectedIndex = ddlSerialConnID.Items.IndexOf(ddlSerialConnID.Items.FindByValue(strSerialConnID))

        strTestSeq = TryCast(gvForm.SelectedRow.FindControl("lblTestSeq"), Label).Text
        lblTestSeq1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestSeq"), Label).Text
        ddlTestSeq.SelectedIndex = ddlTestSeq.Items.IndexOf(ddlTestSeq.Items.FindByValue(strTestSeq))

        intExceptionCode = TryCast(gvForm.SelectedRow.FindControl("lblExceptionCode"), Label).Text
        lblExceptionCode1.Text = TryCast(gvForm.SelectedRow.FindControl("lblExceptionCode"), Label).Text
        ddlExceptionCode.SelectedIndex = ddlExceptionCode.Items.IndexOf(ddlExceptionCode.Items.FindByValue(intExceptionCode))

        lblQATEntryPoint1.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATEntryPoint"), Label).Text

        btnSave.Text = "Save"
        DisableControlCopy()
        DisableCheckboxCopy()
        DisableControlDelete()
        DisableCheckboxDelete()
        EnableButtonDelete()
        EnableButtonSave()
        EnableControlEdit()
        EnableControlEdit2()
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False
            e.Row.Cells(12).Visible = False
            e.Row.Cells(13).Visible = False
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False
            e.Row.Cells(12).Visible = False
            e.Row.Cells(13).Visible = False
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
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ClearInput()
    End Sub
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim strQATDefnID9 As String = String.Empty
        Dim strPackagingLine9 As String = String.Empty
        Dim strFacility As String
        Dim strPackagingLine As String
        Dim intQATDefnID As Integer
        Dim strSearch1 As String = String.Empty
        Dim IsDeleteRecord As CheckBox = TryCast(FindControl("chkDeleteRecord1"), CheckBox)
        Dim intDeleteRecord As Integer = 0
        If IsDeleteRecord.Checked Then
            intDeleteRecord = 1
        Else
            intDeleteRecord = 0
        End If
        If intDeleteRecord = 1 Then
            strSearch1 = ""
            DeletePackagingLine(strSearch1)
            Exit Sub
        Else
            strQATDefnID9 = lblQATDefnID9.Text
            strPackagingLine9 = lblPackagingLine9.Text
            If CheckInput(strQATDefnID9) And CheckInput(strPackagingLine9) Then
                If strQATDefnID9 = "0" And strPackagingLine9 = "0" Then
                    'if strQATDefnID9 = "0" And strPackagingLine9 = "0", means a record has not been picked from the grid view
                    lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
                Else
                    strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                    strPackagingLine = TryCast(FindControl("lblPackagingLine1"), Label).Text
                    intQATDefnID = TryCast(FindControl("lblQATDefnID1"), Label).Text
                    strSearch1 = TryCast(FindControl("lblQATEntryPoint1"), Label).Text
                    DeleteRecord(strFacility, strPackagingLine, intQATDefnID, strSearch1)
                End If
            Else
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim IsCopyRecord As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
        Dim intCopyRecord As Integer = 0

        If lblFacility1.Text = String.Empty Then
            lblFacility1.Text = ddlFacilitySearch.SelectedValue
        End If

        If IsCopyRecord.Checked Then
            intCopyRecord = 1
        Else
            intCopyRecord = 0
        End If
        If intCopyRecord = 1 Then
            SaveRecordNewPackagingLine()
            Exit Sub
        Else
            If CheckInput(lblQATDefnID9.Text) And CheckInput(lblPackagingLine9.Text) Then
                If CheckInput(lblFacility1.Text) Then
                    If CheckInput(lblPackagingLine1.Text) Then
                        If CheckInput(lblQATDefnID1.Text) Then
                            If CheckInput(lblTestSeq1.Text) Then
                                SaveRecord()
                            Else
                                lblErrMsg.Text = "Please select Test Seq. Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please select QAT Definition. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please select Packaging Line. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please select Facility. Update process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please pick one record to Edit. Update process aborted!"
            End If
        End If
    End Sub
    Protected Sub ddlFacilitySearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim strCurentFacility As String = String.Empty
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacilitySearch"), DropDownList)
        strCurentFacility = ddl.SelectedValue.ToString
        BindWorkFlowPackagingLineData(strCurentFacility)
        BindExceptionData(strCurentFacility)
    End Sub
    Protected Sub ddlFacility_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        lblFacility1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ddlQATDefnID_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlQATDefnID"), DropDownList)
        lblQATDefnID1.Text = ddl.SelectedValue.ToString
        lblQATEntryPoint1.Text = GetQATEntryPoint(ddl.SelectedItem.ToString)
    End Sub
    Protected Sub ddlTCPConnID_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlTCPConnID"), DropDownList)
        lblTCPConnID1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlSerialConnID_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlSerialConnID"), DropDownList)
        lblSerialConnID1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlPackagingLine_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLine"), DropDownList)
        lblPackagingLine1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ddlExceptionCode_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlExceptionCode"), DropDownList)
        lblExceptionCode1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlTestSeq_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlTestSeq"), DropDownList)
        lblTestSeq1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlPackagingLineTo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLineTo"), DropDownList)
        lblPackagingLineTo1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ddlPackagingLineFrom_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLineFrom"), DropDownList)
        lblPackagingLineFrom1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub chkCopyRecord1_CheckedChanged(sender As Object, e As EventArgs)
        EnableControlCopyRecord()
        BindWorkFlowPackagingLineData(ddlFacilitySearch.SelectedValue.ToString)
        BindQATEntryPointData()
    End Sub

    Protected Sub chkDeleteRecord1_CheckedChanged(sender As Object, e As EventArgs)
        EnableControlDeleteRecord()
        BindWorkFlowPackagingLineData(ddlFacilitySearch.SelectedValue.ToString)
        BindQATEntryPointData()
    End Sub

    Protected Sub ddlPackagingLineDelete_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLineDelete"), DropDownList)
        lblPackagingLineDelete1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlPackagingLineSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLineSearch"), DropDownList)
        txtSearch.Text = ddl.SelectedValue.ToString
        txtSearch1.Text = ""
        BindQATEntryPointData()
        SearchRecord()
    End Sub

    Protected Sub ddlEntryPointSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlEntryPointSearch"), DropDownList)
        txtSearch1.Text = ddl.SelectedValue.ToString
        SearchRecord()
    End Sub

End Class