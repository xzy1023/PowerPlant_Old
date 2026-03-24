Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim intRowCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblErrMsg.Text = ""
        If Not IsPostBack Then
            GetCurrentUserName()
            Panel1.Visible = False          'WO#35671
            'WO#35671 BindData()
            BindFacilityData()
            ddlFacility.SelectedValue = GetDefaultFacility()
            lblFacility1.Text = ddlFacility.SelectedValue               'WO#35671 
            'WO#35671
            'WO#35671 DEL Start
            'BindDeviceSubTypeData()
            'BindPackagingLineData()
            'BindDeviceTypeData()
            'WO#35671 DEL Stop
            BindPrinterPackagingLineData()
            'WO#35671   ClearInput()
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
    '    'WO#35671   gvForm.DataSource = Me.GetUpdatedData()
    '    'WO#35671 ADDD Start
    '    Dim strPackagingLine As String = String.Empty
    '    strPackagingLine = TryCast(FindControl("lblPackagingLine1"), Label).Text
    '    gvForm.DataSource = Me.GetSearchData(strPackagingLine)
    '    'WO#35671 ADDD Stop
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
    Private Sub BindDeviceSubTypeData()
        'WO#35671 ddlDeviceSubType.DataSource = Me.GetDeviceSubTypeData()
        ddlDeviceSubType.DataSource = Me.GetDeviceSubTypeData(lblFacility1.Text) 'WO#35671 
        ddlDeviceSubType.DataValueField = "DeviceSubTypeValue"
        ddlDeviceSubType.DataTextField = "DeviceSubTypeItem"
        ddlDeviceSubType.DataBind()
        ddlDeviceSubType.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlDeviceSubType.SelectedIndex = 0
    End Sub
    Private Sub BindDeviceSubTypeNoneData()
        ddlDeviceSubType.DataSource = Me.GetDeviceSubTypeNoneData()
        ddlDeviceSubType.DataValueField = "DeviceSubTypeValue"
        ddlDeviceSubType.DataTextField = "DeviceSubTypeItem"
        ddlDeviceSubType.DataBind()
        ddlDeviceSubType.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlDeviceSubType.SelectedIndex = 0
    End Sub
    Private Sub BindPackagingLineData()
        'WO#35671 ddlLine.DataSource = Me.GetPackagingLineData()
        ddlLine.DataSource = Me.GetPackagingLineData(lblFacility1.Text)      'WO#35671 
        ddlLine.DataValueField = "LineValue"
        ddlLine.DataTextField = "LineItem"
        ddlLine.DataBind()
        ddlLine.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlLine.SelectedIndex = 0
    End Sub

    'WO#35671 DEL Start
    'Private Sub BindNewPackagingLineData(ByVal strSearch As String)
    '    gvForm.DataSource = Me.GetNewPackagingLineData(strSearch)
    '    gvForm.DataBind()
    'End Sub
    'WO#35671 DEL stop

    Private Sub BindDeviceTypeData()
        'WO#35671   ddlDeviceType.DataSource = Me.GetDeviceTypeData()
        ddlDeviceType.DataSource = Me.GetDeviceTypeData(lblFacility1.Text)                              'WO#35671
        ddlDeviceType.DataValueField = "DeviceTypeValue"
        ddlDeviceType.DataTextField = "DeviceTypeItem"
        ddlDeviceType.DataBind()
        ddlDeviceType.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlDeviceType.SelectedIndex = 0
    End Sub
    Private Sub BindPrinterPackagingLineData()
        'WO#35671   ddlPackagingLineSearch.DataSource = Me.GetPrinterPackagingLineData()
        ddlPackagingLineSearch.DataSource = Me.GetPrinterPackagingLineData(lblFacility1.Text)            'WO#35671 
        ddlPackagingLineSearch.DataValueField = "LineValue"
        ddlPackagingLineSearch.DataTextField = "LineItem"
        ddlPackagingLineSearch.DataBind()
        ddlPackagingLineSearch.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlPackagingLineSearch.SelectedIndex = 0
    End Sub

    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
    End Sub
    Public Function GetData(Optional Query As String = "") As DataTable
        'WO#35671 DEL Start
        'Dim str As String = "SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType, " & _
        '  " Case when A.DeviceType ='C' Then 'Case' " & _
        '  " when A.DeviceType ='P' Then 'Pallet'" & _
        '  " when A.DeviceType ='F' Then 'Filter Coder' " & _
        '  " when A.DeviceType ='X' Then 'Package Coder' " & _
        '  " Else 'Unknown' End as LabelType, " & _
        '  " Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType " & _
        '  " FROM tblPkgLinePrinterDevice A" & _
        '  " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility " & _
        '  " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') " & _
        '  " ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
        'WO#35671 DEL Stop
        Dim str As String = String.Empty                            'WO#35671

        If Query = "" Then
            str = CrtSQLStmtForGVData(lblFacility1.Text)            'WO#35671
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
        With sb
            .Append("SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType")
            .Append(", Case when A.DeviceType ='C' Then 'Case' ")
            .Append(" when A.DeviceType ='P' Then 'Pallet'")
            .Append(" when A.DeviceType ='F' Then 'Filter Coder' ")
            .Append(" when A.DeviceType ='X' Then 'Package Coder' ")
            .Append(" Else 'Unknown' End as LabelType")
            .Append(", Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType ")
            .Append(" FROM tblPkgLinePrinterDevice A")
            .Append(" LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility ")
            .Append(" AND A.PackagingLine = B.EquipmentID ")
            .Append("WHERE (B.Type = 'P') ")
            .AppendFormat("AND A.Facility = '{0}' ", strFacility)
            If Not IsNothing(strSearchText) Then
                sb.AppendFormat(" AND A.PackagingLine = '{0}'", strSearchText)
            End If
            .Append(" ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName")
            Return .ToString
        End With
    End Function

    Private Sub PostData(strQuery As String)
        sql.ExecQuery(strQuery)
    End Sub
    'WO#35671 ADD Stop

    'WO#35671 DEL Start
    'Private Function GetSortData() As DataTable
    '    Dim strSearchText As String = String.Empty
    '    Dim str As String = String.Empty
    '    Dim str2 As String = String.Empty
    '    'WO#35671 strSearchText = txtSearch.Text
    '    strSearchText = ddlPackagingLineSearch.SelectedValue.ToString   'WO#35671

    '    str2 = "SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType, " & _
    '       " Case when A.DeviceType ='C' Then 'Case' " & _
    '       " when A.DeviceType ='P' Then 'Pallet'" & _
    '       " when A.DeviceType ='F' Then 'Filter Coder' " & _
    '       " when A.DeviceType ='X' Then 'Package Coder' " & _
    '       " Else 'Unknown' End as LabelType, " & _
    '       " Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType " & _
    '       " FROM tblPkgLinePrinterDevice A" & _
    '       " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility " & _
    '       " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') "

    '    If strSearchText <> "" Then
    '        str = str2 & " AND A.PackagingLine Like '%" & strSearchText & "%' ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
    '        sql.ExecQuery(str)
    '    Else
    '        str = str2 & " ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
    '        sql.ExecQuery(str)
    '    End If
    '    intRowCount = sql.DBDT.Rows.Count
    '    Return sql.DBDT
    'End Function
    'WO#35671 DEL Stop

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

    Private Function GetDeviceSubTypeData(strFacility) As DataTable
        'WO#35671 DEL Start
        'Dim str As String = " Select DeviceSubType AS 'DeviceSubTypeValue', " & _
        '    " Case When DeviceSubType = 'N' Then 'None' Else 'Carton Box' End as DeviceSubTypeItem " & _
        '    " From ( " & _
        '    " Select  Case When DeviceSubType ='' then 'N' Else DeviceSubType End as DeviceSubType " & _
        '    " FROM tblPkgLinePrinterDevice Group By DeviceSubType " & _
        '    " ) as X "
        'WO#35671 DEL Stop
        'WO#35671 ADD Start
        Dim sb As New StringBuilder
        Dim str As String
        With sb
            .Append("Select DeviceSubType AS 'DeviceSubTypeValue', ")
            .Append("Case When DeviceSubType = 'N' Then 'None' Else 'Carton Box' End as DeviceSubTypeItem ")
            .Append("From ( ")
            .Append("Select  Case When DeviceSubType ='' then 'N' Else DeviceSubType End as DeviceSubType ")
            .Append("FROM tblPkgLinePrinterDevice ")
            .AppendFormat("WHERE Facility = {0} ", strFacility)
            .Append("Group By DeviceSubType ")
            .Append(") as X ")
            str = sb.ToString
        End With
        'WO#35671 ADD Stop
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblDeviceSubType1.Text = r("DeviceSubTypeValue").ToString
        End If
        Return sql.DBDT
    End Function

    Private Function GetDeviceSubTypeNoneData() As DataTable
        Dim dt As DataTable = New DataTable("DeviceSubTypeValueNone")
        dt.Columns.Add("DeviceSubTypeValue")
        dt.Columns.Add("DeviceSubTypeItem")
        dt.Rows.Add("N", "None")
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblDeviceSubType1.Text = r("DeviceSubTypeValue").ToString
        End If
        Return dt
    End Function
    'WO#35671 DEL Start
    'Private Function GetPackagingLineData() As DataTable
    'Dim str As String = "Select LineItem, RTRIM(PackagingLine) AS 'LineValue'  " & _
    '  " From (" & _
    '  " SELECT  A.PackagingLine, B.Description, RTRIM(A.PackagingLine) + ' ' + RTrim(B.Description) as 'LineItem'" & _
    '  " FROM tblPkgLinePrinterDevice A" & _
    '  " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility" & _
    '  " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') And B.Active=1 " & _
    '  " AND A.DeviceName like A.DeviceName " & _
    '  " ) as X Group By LineItem, PackagingLine"
    'WO#35671 DEL Stop
    'WO#35671 ADD Start
    Private Function GetPackagingLineData(strFacility As String) As DataTable
        Dim str As String = "Select RTRIM(EquipmentID) + ' ' + RTrim(Description) as LineItem, RTRIM(EquipmentID) AS 'LineValue'  " & _
          "From tblEquipment " & _
          "WHERE Facility = '" + strFacility + "' " & _
            "AND [Type] = 'P' " & _
            "AND Active = 1 " & _
            "AND EquipmentID not in ('SPARE', 'Tote') " & _
         "Order BY Description"
        'WO#35671 ADD Stop
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblPackagingLine1.Text = r("LineValue").ToString
        End If
        Return sql.DBDT
    End Function
    'WO#35671 DEL Start
    'Private Function GetPrinterPackagingLineData() As DataTable
    'Dim str As String = "Select LineItem, RTRIM(PackagingLine) AS 'LineValue'  " & _
    '  " From (" & _
    '  " SELECT  A.PackagingLine, B.Description, RTRIM(A.PackagingLine) + ' ' + RTrim(B.Description) as 'LineItem'" & _
    '  " FROM tblPkgLinePrinterDevice A" & _
    '  " INNER JOIN tblEquipment B ON A.facility = B.facility" & _
    '  " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') " & _
    '  " AND A.DeviceName like A.DeviceName " & _
    '  " ) as X Group By LineItem, PackagingLine"
    'WO#35671 DEL Stop
    'WO#35671 ADD Start
    Private Function GetPrinterPackagingLineData(strFacility As String) As DataTable
        Dim Str As String = String.Empty
        Dim sb As New StringBuilder
        With sb
            .Append("SELECT LineItem, RTRIM(PackagingLine) AS 'LineValue' ")
            .Append("FROM (")
            .Append("SELECT  A.PackagingLine, B.Description, RTRIM(A.PackagingLine) + ' ' + RTrim(B.Description) as 'LineItem' ")
            .Append("FROM tblPkgLinePrinterDevice A ")
            .Append("LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility ")
            .Append("AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') ")
            .Append("AND A.DeviceName like A.DeviceName ")
            .AppendFormat("AND A.Facility = {0} ", strFacility)
            .Append(") as X Group By LineItem, PackagingLine")
            Str = .ToString
        End With
        'WO#35671 ADD Stop
        GetData(Str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblPackagingLine1.Text = r("LineValue").ToString
        End If
        Return sql.DBDT
    End Function

    'WO#35671 DEL Start
    'Private Function GetNewPackagingLineData(ByVal strSearchText As String) As DataTable
    '    Dim str As String = String.Empty
    '    Dim str2 As String = String.Empty

    '    str2 = "SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType, " & _
    '        " Case when A.DeviceType ='C' Then 'Case' " & _
    '        " when A.DeviceType ='P' Then 'Pallet'" & _
    '        " when A.DeviceType ='F' Then 'Filter Coder' " & _
    '        " when A.DeviceType ='X' Then 'Package Coder' " & _
    '        " Else 'Unknown' End as LabelType, " & _
    '        " Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType " & _
    '        " FROM tblPkgLinePrinterDevice A" & _
    '        " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility " & _
    '        " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') "

    '    If strSearchText <> "" Then
    '        str = str2 & " AND A.PackagingLine Like '%" & strSearchText & "%' ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
    '        sql.ExecQuery(str)
    '    Else
    '        str = str2 & " ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
    '        sql.ExecQuery(str)
    '    End If
    '    intRowCount = sql.DBDT.Rows.Count
    '    Return sql.DBDT
    'End Function
    'WO#35671 DEL Stop

    'WO#35671   Private Function GetDeviceTypeData() As DataTable
    Private Function GetDeviceTypeData(strFacility As String) As DataTable   'WO#35671
        'WO#35671 DEL Start
        'Dim str As String = "Select DeviceTypeItem, DeviceType AS 'DeviceTypeValue'" & _
        '    " From (" & _
        '    " Select Distinct A.DeviceType," & _
        '    " Case when A.DeviceType ='C' Then 'Case'" & _
        '    " when A.DeviceType ='P' Then 'Pallet'" & _
        '    " when A.DeviceType ='F' Then 'Filter Coder' " & _
        '    " when A.DeviceType ='X' Then 'Package Coder' " & _
        '    " Else 'Unknown' End as DeviceTypeItem " & _
        '    " FROM tblPkgLinePrinterDevice A" & _
        '    " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility " & _
        '    " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P')  And B.Active=1" & _
        '    " AND A.DeviceName like A.DeviceName " & _
        '    " ) as X Group By DeviceTypeItem, DeviceType"
        'WO#35671 DEL Stop
        'WO#35671 ADD Start
        Dim str As String = String.Empty
        Dim sb As New StringBuilder
        With sb
            .Append("Select DeviceTypeItem, DeviceType AS 'DeviceTypeValue'")
            .Append(" From ")
            .Append(" (Select Distinct A.DeviceType,")
            .Append(" Case when A.DeviceType ='C' Then 'Case'")
            .Append(" when A.DeviceType ='P' Then 'Pallet'")
            .Append(" when A.DeviceType ='F' Then 'Filter Coder'")
            .Append(" when A.DeviceType ='X' Then 'Package Coder'")
            .Append(" Else 'Unknown' End as DeviceTypeItem ")
            .Append(" FROM tblPkgLinePrinterDevice A")
            .Append(" LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility ")
            .Append(" AND A.PackagingLine = B.EquipmentID ")
            .Append(" WHERE (B.Type = 'P') And B.Active=1")
            .AppendFormat(" AND A.Facility = '{0}'", strFacility)
            .Append(" ) as X ")
            .Append("Group By DeviceTypeItem, DeviceType")
            str = .ToString
        End With
        'WO#35671 ADD Stop
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblDeviceType1.Text = r("DeviceTypeValue").ToString
        End If
        Return sql.DBDT
    End Function

    'WO#35671 DEL Start
    'Protected Function GetUpdatedData() As DataTable
    '    Dim str As String = String.Empty
    '    Dim strPackagingLine As String = String.Empty
    '    strPackagingLine = TryCast(FindControl("lblPackagingLine1"), Label).Text
    '    str = "SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType, " & _
    '           " Case when A.DeviceType ='C' Then 'Case' " & _
    '           " when A.DeviceType ='P' Then 'Pallet'" & _
    '           " when A.DeviceType ='F' Then 'Filter Coder' " & _
    '           " when A.DeviceType ='X' Then 'Package Coder' " & _
    '           " Else 'Unknown' End as LabelType, " & _
    '           " Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType " & _
    '           " FROM tblPkgLinePrinterDevice A" & _
    '           " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility " & _
    '           " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') " & _
    '           " And PackagingLine= '" & strPackagingLine & "'" & _
    '             " ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
    '    GetData(str)
    '    intRowCount = sql.DBDT.Rows.Count
    '    ViewState("gvFormData") = sql.DBDT          'WO#35671
    '    Return sql.DBDT
    'End Function
    'WO#35671 DEL Stop

    Protected Function GetSearchData(ByVal strSearchText As String) As DataTable
        'WO#35671   Protected Function GetSearchData(ByVal strFacility As String, ByVal strSearchText As String) As DataTable        
        'WO#35671 DEL Start
        'Dim str As String = String.Empty
        'Dim str2 As String = String.Empty

        'str2 = "SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType, " & _
        '    " Case when A.DeviceType ='C' Then 'Case' " & _
        '    " when A.DeviceType ='P' Then 'Pallet'" & _
        '    " when A.DeviceType ='F' Then 'Filter Coder' " & _
        '    " when A.DeviceType ='X' Then 'Package Coder' " & _
        '    " Else 'Unknown' End as LabelType, " & _
        '    " Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType " & _
        '    " FROM tblPkgLinePrinterDevice A" & _
        '    " LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility " & _
        '    " AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') "
        'WO#35671 DEL Stop
        'WO#35671 ADD Start
        Dim sb As New StringBuilder
        Dim strFacility As String
        sb.Append("SELECT A.facility, A.PackagingLine, B.Description, A.DeviceType, A.DeviceName, A.IPAddress, A.RRN, A.UseNativeDriver, A.DeviceSubType, ")
        sb.Append(" Case when A.DeviceType ='C' Then 'Case' ")
        sb.Append(" when A.DeviceType ='P' Then 'Pallet'")
        sb.Append(" when A.DeviceType ='F' Then 'Filter Coder' ")
        sb.Append(" when A.DeviceType ='X' Then 'Package Coder' ")
        sb.Append(" Else 'Unknown' End as LabelType, ")
        sb.Append(" Case when A.DeviceSubType ='B' Then 'Carton Box' Else 'None' End as LabelSubType ")
        sb.Append(" FROM tblPkgLinePrinterDevice A")
        sb.Append(" LEFT OUTER JOIN tblEquipment B ON A.facility = B.facility ")
        sb.Append(" AND A.PackagingLine = B.EquipmentID WHERE (B.Type = 'P') ")
        strFacility = ddlFacility.SelectedValue
        'WO#35671 ADD Stop

        If strSearchText <> "" Then
            'WO#35671   str = str2 & " AND A.PackagingLine Like '%" & strSearchText & "%' ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
            sb.AppendFormat(" AND A.Facility = '{0}' AND A.PackagingLine = '{1}'", strFacility, strSearchText)   'WO#35671
            'WO#35671   sql.ExecQuery(str)
            'WO#35671   Else
            'WO#35671   str = str2 & " ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName"
            'WO#35671   sql.ExecQuery(str)
        End If
        sb.Append(" ORDER BY A.facility, A.PackagingLine, A.DeviceType, A.DeviceName")      'WO#35671
        sql.ExecQuery(sb.ToString)                                                          'WO#35671

        intRowCount = sql.DBDT.Rows.Count
        ViewState("gvFormData") = sql.DBDT          'WO#35671
        Return sql.DBDT
    End Function
    Private Sub SearchRecord()
        Dim strSearch As String = String.Empty
        'WO#35671   strSearch = txtSearch.Text
        strSearch = ddlPackagingLineSearch.SelectedValue.ToString       'WO#35671
        txtSearch.Text = strSearch                                      'INC#10350

        If strSearch = "" Then
            BindData()
        Else
            gvForm.DataSource = Me.GetSearchData(strSearch)
            gvForm.DataBind()
        End If
    End Sub
    Private Sub DeleteRecord(ByVal intRRNId As Integer, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblPkgLinePrinterDevice Where RRN = @rrnid"
            sql.AddParam("@rrnid", intRRNId)

            lblMsg = "Delete record completed!"
            'WO#35671   GetData(str)
            PostData(str)                       'WO#35671
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                BindPrinterPackagingLineData()
                txtSearch.Text = strDescription
                ddlPackagingLineSearch.SelectedIndex = ddlPackagingLineSearch.Items.IndexOf(ddlPackagingLineSearch.Items.FindByValue(strDescription))
                'WO#35671   BindNewPackagingLineData(strDescription)
                'WO#35671  ADD Start
                BindSearchData(strDescription)
                If ddlPackagingLineSearch.Items.IndexOf(ddlPackagingLineSearch.Items.FindByValue(strDescription)) = -1 Then
                    lblMsg = lblMsg & String.Format(" There are no data records for line {0}.", strDescription)
                End If
                lblErrMsg.Text = lblMsg
            Else
                lblErrMsg.Text = strMsg
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub UpdateRecord(ByVal intRRNId As Integer, ByVal strPackagingLine As String, ByVal strDeviceType As String, ByVal strDeviceName As String,
                           ByVal strIPAddress As String, ByVal intUseNativeDriver As Integer, ByVal strFacility As String, strDeviceSubType As String, ByVal strLabelType As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty
            If btnSave.Text = "Save New" Then
                str = "Insert Into tblPkgLinePrinterDevice (PackagingLine, DeviceType, DeviceSubType, DeviceName, IPAddress, UseNativeDriver, Facility) values " & _
                        " (@packagingline, @devicetype, @devicesubtype, @devicename, @ipaddress, @usenativedriver, @facility)"
                lblMsg = "Add new record completed!"
            Else
                str = "UPDATE tblPkgLinePrinterDevice SET DeviceType = @devicetype, DeviceName = @devicename, IPAddress = @ipaddress, UseNativeDriver = @usenativedriver," & _
                       " DeviceSubType = @devicesubtype, PackagingLine=@packagingline, Facility=@facility WHERE (RRN = @rrnid)"
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@rrnid", intRRNId)
            sql.AddParam("@packagingline", strPackagingLine)
            sql.AddParam("@devicetype", strDeviceType)
            sql.AddParam("@devicename", strDeviceName)
            sql.AddParam("@ipaddress", strIPAddress)
            sql.AddParam("@usenativedriver", intUseNativeDriver)
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@devicesubtype", strDeviceSubType)
            'WO#35671   GetData(str)
            PostData(str)                       'WO#35671
            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                'If adding new record, refresh the list of the packaging line data          'WO#35671
                If btnSave.Text = "Save New" Then   'WO#35671
                    BindPrinterPackagingLineData()
                End If                              'WO#35671
                'Refresh the data in the grid form after the change.             WO#35671 
                txtSearch.Text = strPackagingLine
                ddlPackagingLineSearch.SelectedIndex = ddlPackagingLineSearch.Items.IndexOf(ddlPackagingLineSearch.Items.FindByValue(strPackagingLine))
                BindSearchData(strPackagingLine)
                'WO#35671 BindNewPackagingLineData(strPackagingLine)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key row") Then
                    lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strPackagingLine & "/" & strLabelType & "/" & strDeviceName & ". Update process aborted!"
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
            Dim strDeviceSubType As String = String.Empty
            Dim intintUseNativeDriver As Integer
            Dim strRRNID As String = String.Empty
            Dim strPackagingLine As String = String.Empty
            Dim strDeviceType As String = String.Empty
            Dim strDeviceName As String = String.Empty
            Dim strIPAddress As String = String.Empty
            Dim strLabelType As String = String.Empty

            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkUseNativeDriver1"), CheckBox)
                If IsActive.Checked Then
                    intintUseNativeDriver = 1
                Else
                    intintUseNativeDriver = 0
                End If
                strRRNID = TryCast(FindControl("lblRRN1"), Label).Text
                strPackagingLine = TryCast(FindControl("lblPackagingLine1"), Label).Text
                strDeviceType = TryCast(FindControl("lblDeviceType1"), Label).Text
                strLabelType = TryCast(FindControl("lblLabelType1"), Label).Text

                strDeviceName = TryCast(FindControl("txtDeviceName1"), TextBox).Text
                strIPAddress = TryCast(FindControl("txtIPAddress1"), TextBox).Text
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text

                If TryCast(FindControl("lblDeviceSubType1"), Label).Text = "N" Then
                    strDeviceSubType = ""
                Else
                    strDeviceSubType = TryCast(FindControl("lblDeviceSubType1"), Label).Text
                End If
                If strRRNID = "0" Then
                    If CheckDuplicateKey(strPackagingLine, strDeviceType, strDeviceName, strFacility) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strPackagingLine & "/" & strLabelType & "/" & strDeviceName & ". Update process aborted!"
                        Exit Sub
                    End If
                End If
                UpdateRecord(strRRNID, strPackagingLine, strDeviceType, strDeviceName, strIPAddress, intintUseNativeDriver, strFacility, strDeviceSubType, strLabelType)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Protected Function CheckDuplicateKey(ByVal strPackagingLine As String, ByVal strDeviceType As String, ByVal strDeviceName As String,
                         ByVal strFacility As String) As Boolean
        Dim str As String = String.Empty
        str = "SELECT RRN FROM tblPkgLinePrinterDevice" & _
            " Where Facility= '" & strFacility & "'" & _
            " And PackagingLine= '" & strPackagingLine & "'" & _
            " And DeviceType = '" & strDeviceType & "'" & _
            " And DeviceName = '" & strDeviceName & "'"

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblRRN1.Text = "0"
        btnSave.Text = "Save New"
    End Sub
    Private Sub ClearTextBox()
        lblRRN1.Text = ""
        lblPackagingLine1.Text = ""
        lblDescription1.Text = ""
        lblDeviceType1.Text = ""
        lblDeviceSubType1.Text = ""
        lblLabelType1.Text = ""
        lblLabelSubType1.Text = ""
        txtDeviceName1.Text = ""
        txtIPAddress1.Text = ""
        'WO#35671 lblFacility1.Text = ""
        'WO#35671   txtSearch.Text = ""
        chkUseNativeDriver1.Checked = False
    End Sub

    Private Sub ClearDropDownValue()
        'WO#35671 Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        'WO#35671 ddlFacility2.SelectedIndex = 0

        Dim ddlLine2 As DropDownList = TryCast(FindControl("ddlLine"), DropDownList)
        ddlLine2.SelectedIndex = 0

        Dim ddlDeviceType2 As DropDownList = TryCast(FindControl("ddlDeviceType"), DropDownList)
        ddlDeviceType2.SelectedIndex = 0

        Dim ddlDeviceSubType2 As DropDownList = TryCast(FindControl("ddlDeviceSubType"), DropDownList)
        ddlDeviceSubType2.SelectedIndex = 0

        Dim ddlLineSearch2 As DropDownList = TryCast(FindControl("ddlPackagingLineSearch"), DropDownList)
        ddlLineSearch2.SelectedIndex = 0
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strLineItem As String = String.Empty
        Dim strDeviceType As String = String.Empty
        Dim strDeviceSubType As String = String.Empty
        Dim strLineText As String = String.Empty

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlLine2 As DropDownList = TryCast(FindControl("ddlLine"), DropDownList)
        strLineItem = ddlLine2.SelectedValue.ToString
        strLineText = ddlLine2.SelectedItem.ToString

        If CheckInput(strLineItem) = True Then
            lblPackagingLine1.Text = Trim(strLineItem)
            lblDescription1.Text = Trim(strLineText)
        Else
            lblErrMsg.Text = "Please select Line."
            Return False
            Exit Function
        End If

        Dim ddlDeviceType2 As DropDownList = TryCast(FindControl("ddlDeviceType"), DropDownList)
        strDeviceType = ddlDeviceType2.SelectedValue.ToString
        If CheckInput(strDeviceType) = True Then
            lblDeviceType1.Text = ddlDeviceType2.SelectedValue.ToString
            lblLabelType1.Text = ddlDeviceType2.SelectedItem.ToString
        Else
            lblErrMsg.Text = "Please select Device Type."
            Return False
            Exit Function
        End If

        Dim ddlDeviceSubType2 As DropDownList = TryCast(FindControl("ddlDeviceSubType"), DropDownList)
        strDeviceSubType = ddlDeviceSubType2.SelectedValue.ToString
        If CheckInput(strDeviceSubType) = True Then
            lblDeviceSubType1.Text = ddlDeviceSubType2.SelectedValue.ToString
            lblLabelSubType1.Text = ddlDeviceSubType2.SelectedItem.ToString
        Else
            lblErrMsg.Text = "Please select Device Sub Type."
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
        Dim strSearch As String = String.Empty
        Dim dt As DataTable                                                     'WO#35671

        gvForm.PageIndex = e.NewPageIndex
        strSearch = txtSearch.Text
        If strSearch = "" Then
            'WO#35671   BindData()
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
        'WO#35671   Dim strFacility As String = String.Empty

        Dim strDeviceType As String = String.Empty
        Dim strDeviceTypeItem As String = String.Empty
        Dim strLineItem As String = String.Empty
        Dim strDeviceSubType As String = String.Empty
        Dim strDeviceSubTypeItem As String = String.Empty
        Dim intActive As Integer = 0
        lblRRN1.Text = TryCast(gvForm.SelectedRow.FindControl("lblRRN"), Label).Text
        txtDeviceName1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDeviceName"), Label).Text
        txtIPAddress1.Text = TryCast(gvForm.SelectedRow.FindControl("lblIPAddress"), Label).Text

        Dim IsActive As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbUseNativeDriver"), CheckBox)
        If IsActive.Checked Then
            intActive = 1
            chkUseNativeDriver1.Checked = True
        Else
            intActive = 0
            chkUseNativeDriver1.Checked = False
        End If

        'WO#35671 DEL Start
        'strFacility = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        'lblFacility1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        'ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(strFacility))
        'WO#35671 DEL Stop

        strDeviceTypeItem = TryCast(gvForm.SelectedRow.FindControl("lblDeviceType"), Label).Text
        lblDeviceType1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDeviceType"), Label).Text
        lblLabelType1.Text = TryCast(gvForm.SelectedRow.FindControl("lblLabelType"), Label).Text
        ddlDeviceType.SelectedIndex = ddlDeviceType.Items.IndexOf(ddlDeviceType.Items.FindByValue(strDeviceTypeItem))

        If lblDeviceType1.Text = "X" Then
            BindDeviceSubTypeData()
        Else
            BindDeviceSubTypeNoneData()
        End If

        strDeviceSubType = TryCast(gvForm.SelectedRow.FindControl("lblDeviceSubType"), Label).Text
        If strDeviceSubType = "B" Then
            strDeviceSubTypeItem = strDeviceSubType
        Else
            strDeviceSubTypeItem = "N"
        End If

        lblPackagingLine1.Text = TryCast(gvForm.SelectedRow.FindControl("lblPackagingLine"), Label).Text
        lblDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDescription"), Label).Text
        ' strLineItem = Trim(lblPackagingLine1.Text) & " " & Trim(lblDescription1.Text)
        strLineItem = Trim(lblPackagingLine1.Text)
        ddlLine.SelectedIndex = ddlLine.Items.IndexOf(ddlLine.Items.FindByValue(strLineItem))

        lblDeviceSubType1.Text = TryCast(gvForm.SelectedRow.FindControl("lblDeviceSubType"), Label).Text
        lblLabelSubType1.Text = TryCast(gvForm.SelectedRow.FindControl("lblLabelSubType"), Label).Text
        ddlDeviceSubType.SelectedIndex = ddlDeviceSubType.Items.IndexOf(ddlDeviceSubType.Items.FindByValue(strDeviceSubTypeItem))
        btnSave.Text = "Save"
        btnCancel.Focus()         'WO#35671

    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(9).Visible = False
            e.Row.Cells(10).Visible = False
            e.Row.Cells(11).Visible = False
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Left
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
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ClearInput()
        ddlPackagingLineSearch.Focus()                              'WO#35671
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim strDesc As String = String.Empty
        If CheckInput(lblRRN1.Text) Then
            If CInt(lblRRN1.Text) > 0 Then
                If CheckInput(lblPackagingLine1.Text) Then
                    Dim strTestFormID As String = TryCast(FindControl("lblRRN1"), Label).Text
                    strDesc = Trim(lblPackagingLine1.Text)
                    DeleteRecord(strTestFormID, strDesc)
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
        If CheckInput(lblRRN1.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(lblPackagingLine1.Text) Then
                    If CheckInput(lblDeviceType1.Text) Then
                        If CheckInput(lblDeviceSubType1.Text) Then
                            If CheckInput(txtDeviceName1.Text) Then
                                If IsIPValid(txtIPAddress1.Text) Then
                                    SaveRecord()
                                Else
                                    lblErrMsg.Text = "Please enter valid IP Address. Update process aborted!"
                                End If
                            Else
                                lblErrMsg.Text = "Please enter Device Name. Update process aborted!"
                            End If
                        Else
                            lblErrMsg.Text = "Please select Label Sub Type. Update process aborted!"
                        End If
                    Else
                        lblErrMsg.Text = "Please select Label Type. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please select Line. Update process aborted!"
                End If
            Else
                lblErrMsg.Text = "Please select Facility. Update process aborted!"
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Edit. Update process aborted!"
        End If
    End Sub

    'WO#35671 ADD Start
    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click

        Try
            lblErrMsg.Text = String.Empty
            SearchRecord()
            gvForm.SelectedIndex = -1
            Panel1.Visible = True
            BindDeviceSubTypeData()
            BindPackagingLineData()
            BindDeviceTypeData()
            ClearInput()
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try

    End Sub
    'WO#35671 ADD Stop

    Protected Sub ddlFacility_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        'If lblFacility1.Text <> ddl.SelectedValue.ToString Then     'WO#35671
        lblFacility1.Text = ddl.SelectedValue.ToString
        'WO#35671 ADD Start
        ' Else
        'lblFacility1.Text = "0"
        'End If
        BindPrinterPackagingLineData()
        ddlPackagingLineSearch.SelectedIndex = 0
        Panel1.Visible = False
        'End If
        'WO#35671 ADD Stop
    End Sub

    Protected Sub ddlLine_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlLine"), DropDownList)
        lblPackagingLine1.Text = ddl.SelectedValue.ToString
        lblDescription1.Text = ddl.SelectedItem.ToString
    End Sub

    Protected Sub ddlDeviceType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlDeviceType"), DropDownList)
        lblDeviceType1.Text = ddl.SelectedValue.ToString
        lblLabelType1.Text = ddl.SelectedValue.ToString
        If lblDeviceType1.Text = "X" Then
            BindDeviceSubTypeData()
        Else
            BindDeviceSubTypeNoneData()
        End If
    End Sub
    Protected Sub ddlDeviceSubType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlDeviceSubType"), DropDownList)
        lblDeviceSubType1.Text = ddl.SelectedValue.ToString
        lblLabelSubType1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ddlDeviceSubTypeNone_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlDeviceSubType"), DropDownList)
        lblDeviceSubType1.Text = ddl.SelectedValue.ToString
        lblLabelSubType1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlPackagingLineSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLineSearch"), DropDownList)
        txtSearch.Text = ddl.SelectedValue.ToString
        'WO#35671    SearchRecord()
    End Sub

End Class