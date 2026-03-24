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
            BindQATEntryPointData()
            BindInProcFreqTypeData()
            BindNoOfLanesData()
            BindNoOfSamplesData()
            BindQATNoteDescriptionData()
            BindQATFormCategoryData()
            BindQATSpecificationDescrptionData()
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
    Private Sub BindQATEntryPointData()
        ddlEntryPointDesc.DataSource = Me.GetQATEntryPointData()
        ddlEntryPointDesc.DataValueField = "QATEntryPoint"
        ddlEntryPointDesc.DataTextField = "EntryPointDescription"
        ddlEntryPointDesc.DataBind()
        ddlEntryPointDesc.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlEntryPointDesc.SelectedIndex = 0
    End Sub
    Private Sub BindInProcFreqTypeData()
        ddlInProcFreqType.DataSource = Me.GetInProcFreqTypeData
        ddlInProcFreqType.DataValueField = "FreqTypeID"
        ddlInProcFreqType.DataTextField = "FreqTypeDescription"
        ddlInProcFreqType.DataBind()
        ddlInProcFreqType.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlInProcFreqType.SelectedIndex = 0
    End Sub
    Private Sub BindNoOfLanesData()
        ddlNoOfLanes.DataSource = Me.GetNoOfLanesData
        ddlNoOfLanes.DataValueField = "NoOfLanes"
        ddlNoOfLanes.DataTextField = "Description"
        ddlNoOfLanes.DataBind()
        ddlNoOfLanes.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlNoOfLanes.SelectedIndex = 0
    End Sub
    Private Sub BindNoOfSamplesData()
        ddlNoOfSamples.DataSource = Me.GetNoOfSamplesData
        ddlNoOfSamples.DataValueField = "NoOfSamples"
        ddlNoOfSamples.DataTextField = "Description"
        ddlNoOfSamples.DataBind()
        ddlNoOfSamples.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlNoOfSamples.SelectedIndex = 0
    End Sub
    Private Sub BindQATNoteDescriptionData()
        ddlNoteDesc.DataSource = Me.GetQATNoteDescData()
        ddlNoteDesc.DataValueField = "NoteID"
        ddlNoteDesc.DataTextField = "NoteDescription"
        ddlNoteDesc.DataBind()
        ddlNoteDesc.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlNoteDesc.SelectedIndex = 0
    End Sub
    Private Sub BindQATFormCategoryData()
        ddlTestCategory.DataSource = Me.GetQATFormCategoryData()
        ddlTestCategory.DataValueField = "TestFormID"
        ddlTestCategory.DataTextField = "TestCategory"
        ddlTestCategory.DataBind()
        ddlTestCategory.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlTestCategory.SelectedIndex = 0
    End Sub
    Private Sub BindQATSpecificationDescrptionData()
        ddlTestSpecDesc.DataSource = Me.GetQATSpecDescData()
        ddlTestSpecDesc.DataValueField = "TestSpecID"
        ddlTestSpecDesc.DataTextField = "TestSpecDesc"
        ddlTestSpecDesc.DataBind()
        ddlTestSpecDesc.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddlTestSpecDesc.SelectedIndex = 0
    End Sub
    Public Sub GetCurrentUserName()
        lblUserName.Text = sql.GetCurrentUserName()
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
    Private Function GetQATEntryPointData() As DataTable
        GetQATEntryPointData = Nothing
        Using daEP As New dsQATEntryPointTableAdapters.tblQATEntryPointTableAdapter
            Using dtEP As New dsQATEntryPoint.tblQATEntryPointDataTable
                daEP.Fill(dtEP)
                GetQATEntryPointData = dtEP
                If dtEP.Rows.Count > 0 Then
                    lblEntryPointDesc1.Text = dtEP.Rows(0).Item("EntryPointDescription")
                End If
            End Using
        End Using
        'Dim dt As DataTable = New DataTable("QATEntryPoint")
        'dt.Columns.Add("Description")
        'dt.Columns.Add("QATEntryPoint")
        'dt.Rows.Add("Start Up", "S")
        'dt.Rows.Add("In Process", "I")
        'dt.Rows.Add("On Request", "O")
        'dt.Rows.Add("Closed", "C")
        'intRowCount = dt.Rows.Count
        'If dt.Rows.Count > 0 Then
        '    Dim r As DataRow = dt.Rows(0)
        '    lblEntryPointDesc1.Text = r("Description").ToString
        'End If
        'Return dt
    End Function
    Private Function GetInProcFreqTypeData() As DataTable
        GetInProcFreqTypeData = Nothing
        'Dim dt As DataTable = New DataTable("InProcFreqType")
        'dt.Columns.Add("Description")
        'dt.Columns.Add("InProcFreqType")
        'dt.Rows.Add("*** No Freq Type", 0)
        'dt.Rows.Add("By Unit", 1)
        'dt.Rows.Add("By Pallet", 2)
        'dt.Rows.Add("By Time", 3)
        'intRowCount = dt.Rows.Count
        'If dt.Rows.Count > 0 Then
        '    Dim r As DataRow = dt.Rows(0)
        '    lblInProcFreqType1.Text = r("Description").ToString
        'End If
        'Return dt

        Using daIPFT As New dsQATInProcFreqTypeTableAdapters.tblQATInProcFreqTypeTableAdapter
            Using dtIPFT As New dsQATInProcFreqType.tblQATInProcFreqTypeDataTable
                daIPFT.Fill(dtIPFT)
                GetInProcFreqTypeData = dtIPFT
                If dtIPFT.Rows.Count > 0 Then
                    lblInProcFreqType1.Text = dtIPFT.Rows(0).Item("FreqTypeDescription")
                End If
            End Using
        End Using

    End Function
    Private Function GetNoOfLanesData() As DataTable
        Dim dt As DataTable = New DataTable("NoOfLanes")
        dt.Columns.Add("Description")
        dt.Columns.Add("NoOfLanes")
        For i = 0 To 100
            dt.Rows.Add(i, i)
        Next
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblNoOfLanes1.Text = r("Description").ToString
        End If
        Return dt
    End Function
    Private Function GetNoOfSamplesData() As DataTable
        Dim dt As DataTable = New DataTable("NoOfSamples")
        dt.Columns.Add("Description")
        dt.Columns.Add("NoOfSamples")
        For i = 0 To 100
            dt.Rows.Add(i, i)
        Next
        intRowCount = dt.Rows.Count
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            lblNoOfSamples1.Text = r("Description").ToString
        End If
        Return dt
    End Function
    Private Function GetQATSpecDescData() As DataTable
        Dim str As String = String.Empty
        str = "Select 0 as 'TestSpecID', '*** No Test Specification' as 'TestSpecDesc' " & _
            " Union " & _
            "Select TestSpecID, TestSpecDesc From tblQATSpec Where active=1 Order by TestSpecDesc "
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblTestSpecDesc1.Text = r("TestSpecDesc").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetQATFormCategoryData() As DataTable
        Dim str As String = String.Empty
        str = "Select TestFormID, TestCategory From tblQATForm Where active=1 Order by TestCategory "
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblTestCategory1.Text = r("TestCategory").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetQATNoteDescData() As DataTable
        Dim str As String = String.Empty
        str = "Select 0 as 'NoteID', '*** No Note' as 'NoteDescription' " & _
            " Union " & _
            " Select  NoteID, NoteDescription  From tblQATNote Where active=1 Order By NoteDescription "
        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        If sql.DBDT.Rows.Count > 0 Then
            Dim r As DataRow = sql.DBDT.Rows(0)
            lblNoteDesc1.Text = r("NoteDescription").ToString
        End If
        Return sql.DBDT
    End Function
    Private Function GetData(Optional Query As String = "") As DataTable
        Dim str As String = String.Empty
        'str = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
        '     " Case When A.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When A.QATEntryPoint ='I' Then 'In Process'" & _
        '    " When A.QATEntryPoint ='C' Then 'Closed'" & _
        '    " When A.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc," & _
        '    " Case When A.InProcFreqType = 0 Then '***' " & _
        '    " When A.InProcFreqType = 1 Then 'By Unit' " & _
        '    " When A.InProcFreqType = 2 Then 'By Pallet'" & _
        '    " When A.InProcFreqType = 3 Then 'By Time'" & _
        '    " Else 'Unknown' End as InProcFreqTypeDesc," & _
        '    " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
        '    " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
        '    " FROM tblQATDefinition A " & _
        '    " Left Outer Join (Select 0 as 'TestSpecID', '***' as 'TestSpecDesc' " & _
        '    " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
        '    " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
        '    " Left Outer Join (Select 0 as 'NoteID', '***' as 'NoteDescription' " & _
        '    " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID " & _
        '    " Order By A.QATDefnDescription "

        str = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
             " tEP.EntryPointDescription, " & _
            " tIPFT.FreqTypeDescription," & _
            " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
            " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
            " FROM tblQATDefinition A " & _
            " Left Outer Join tblQATEntryPoint tEP " & _
            " ON A.QATEntryPoint = tEP.QATEntryPoint " & _
            " Left Outer Join tblQATInProcFreqType tIPFT " & _
            " ON A.InProcFreqType = tIPFT.FreqTypeID " & _
            " Left Outer Join (Select 0 as 'TestSpecID', '***' as 'TestSpecDesc' " & _
            " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
            " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
            " Left Outer Join (Select 0 as 'NoteID', '***' as 'NoteDescription' " & _
            " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID " & _
            " Order By A.QATDefnDescription "
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

        'str2 = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
        '     " Case When A.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When A.QATEntryPoint ='I' Then 'In Process'" & _
        '    " When A.QATEntryPoint ='C' Then 'Closed'" & _
        '    " When A.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc," & _
        '    " Case When A.InProcFreqType = 0 Then '***' " & _
        '    " When A.InProcFreqType = 1 Then 'By Unit' " & _
        '    " When A.InProcFreqType = 2 Then 'By Pallet'" & _
        '    " When A.InProcFreqType = 3 Then 'By Time'" & _
        '    " Else 'Unknown' End as FreqTypeDescription," & _
        '    " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
        '    " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
        '    " FROM tblQATDefinition A " & _
        '    " Left Outer Join (Select 0 as 'TestSpecID', '***' as 'TestSpecDesc' " & _
        '    " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
        '    " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
        '    " Left Outer Join (Select 0 as 'NoteID', '***' as 'NoteDescription' " & _
        '    " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID "

        str2 = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
             " tEP.EntryPointDescription, " & _
            " tIPFT.FreqTypeDescription," & _
            " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
            " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
            " FROM tblQATDefinition A " & _
            " Left Outer Join tblQATEntryPoint tEP " & _
            " ON A.QATEntryPoint = tEP.QATEntryPoint " & _
            " Left Outer Join tblQATInProcFreqType tIPFT " & _
            " ON A.InProcFreqType = tIPFT.FreqTypeID " & _
            " Left Outer Join (Select 0 as 'TestSpecID', '***' as 'TestSpecDesc' " & _
            " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
            " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
            " Left Outer Join (Select 0 as 'NoteID', '***' as 'NoteDescription' " & _
            " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID "

        If strSearchText <> "" Then
            str = str2 & " Where A.QATDefnDescription like '%" & strSearchText & "%' Order By A.QATDefnDescription"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By A.QATDefnDescription"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
  
    Private Function GetUpdatedData() As DataTable
        Dim str As String = String.Empty
        'str = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
        '     " Case When A.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When A.QATEntryPoint ='I' Then 'In Process'" & _
        '    " When A.QATEntryPoint ='C' Then 'Closed'" & _
        '    " When A.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc," & _
        '    " Case When A.InProcFreqType = 0 Then '***' " & _
        '    " When A.InProcFreqType = 1 Then 'By Unit' " & _
        '    " When A.InProcFreqType = 2 Then 'By Pallet'" & _
        '    " When A.InProcFreqType = 3 Then 'By Time'" & _
        '    " Else 'Unknown' End as FreqTypeDescription," & _
        '    " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
        '    " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
        '    " FROM tblQATDefinition A " & _
        '    " Left Outer Join (Select 0 as 'TestSpecID', '***' as 'TestSpecDesc' " & _
        '    " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
        '    " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
        '    " Left Outer Join (Select 0 as 'NoteID', '***' as 'NoteDescription' " & _
        '    " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID " & _
        '    " Order By A.QATDefnDescription "
        str = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
            " tEP.EntryPointDescription, " & _
            " tIPFT.FreqTypeDescription," & _
            " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
            " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
            " FROM tblQATDefinition A " & _
            " Left Outer Join tblQATEntryPoint tEP " & _
            " ON A.QATEntryPoint = tEP.QATEntryPoint " & _
            " Left Outer Join tblQATInProcFreqType tIPFT " & _
            " ON A.InProcFreqType = tIPFT.FreqTypeID " & _
            " Left Outer Join (Select 0 as 'TestSpecID', '***' as 'TestSpecDesc' " & _
            " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
            " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
            " Left Outer Join (Select 0 as 'NoteID', '***' as 'NoteDescription' " & _
            " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID " & _
            " Order By A.QATDefnDescription "

        GetData(str)
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Function GetSearchData(ByVal strSearchText As String) As DataTable
        Dim str As String = String.Empty
        Dim str2 As String = String.Empty
        'str2 = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
        '     " Case When A.QATEntryPoint ='S' Then 'Start Up' " & _
        '    " When A.QATEntryPoint ='I' Then 'In Process'" & _
        '    " When A.QATEntryPoint ='C' Then 'Closed'" & _
        '    " When A.QATEntryPoint ='O' Then 'On Request' " & _
        '    " Else 'Unknown' End as EntryPointDesc," & _
        '    " Case When A.InProcFreqType = 0 Then '*** No Freq Type' " & _
        '    " When A.InProcFreqType = 1 Then 'By Unit' " & _
        '    " When A.InProcFreqType = 2 Then 'By Pallet'" & _
        '    " When A.InProcFreqType = 3 Then 'By Time'" & _
        '    " Else 'Unknown' End as FreqTypeDescription," & _
        '    " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
        '    " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
        '    " FROM tblQATDefinition A " & _
        '    " Left Outer Join (Select 0 as 'TestSpecID', '*** No Test Specification' as 'TestSpecDesc' " & _
        '    " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
        '    " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
        '    " Left Outer Join (Select 0 as 'NoteID', '*** No Note' as 'NoteDescription' " & _
        '    " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID "
        str2 = "SELECT A.Active, A.Facility, A.QATDefnDescription, A.ExpiryCount," & _
            " tEP.EntryPointDescription, " & _
            " tIPFT.FreqTypeDescription," & _
            " A.InProcFreqType, A.InProcNoOfFreq, A.NoOfLanes, A.NoOfSamples, B.TestSpecDesc, C.TestCategory, A.TestFormTitle, " & _
            " D.NoteDescription, A.QATDefnID, A.Alert, A.AllowOverride, A.QATEntryPoint, A.TestSpecID, A.TestFormID, A.NoteID " & _
            " FROM tblQATDefinition A " & _
            " Left Outer Join tblQATEntryPoint tEP " & _
            " ON A.QATEntryPoint = tEP.QATEntryPoint " & _
            " Left Outer Join tblQATInProcFreqType tIPFT " & _
            " ON A.InProcFreqType = tIPFT.FreqTypeID " & _
            " Left Outer Join (Select 0 as 'TestSpecID', '*** No Test Specification' as 'TestSpecDesc' " & _
            " Union Select  TestSpecID, TestSpecDesc  From tblQATSpec) B  On A.TestSpecID = B.TestSpecID " & _
            " Left Outer Join tblQATForm C on A.TestFormID = C.TestFormID " & _
            " Left Outer Join (Select 0 as 'NoteID', '*** No Note' as 'NoteDescription' " & _
            " Union Select  NoteID, NoteDescription  From tblQATNote) D  On A.NoteID = D.NoteID "

        If strSearchText <> "" Then
            str = str2 & " Where A.QATDefnDescription like '%" & strSearchText & "%' Order By A.QATDefnDescription"
            sql.ExecQuery(str)
        Else
            str = str2 & " Order By A.QATDefnDescription"
            sql.ExecQuery(str)
        End If
        intRowCount = sql.DBDT.Rows.Count
        Return sql.DBDT
    End Function
    Private Sub ClearInput()
        ClearDropDownValue()
        ClearTextBox()
        lblQATDefnID9.Text = "0"
        btnSave.Text = "Save New"
       End Sub
    Private Sub ClearTextBox()
        lblFacility1.Text = ""
        lblQATDefnID9.Text = ""
        txtQATDefnDescription1.Text = ""
        lblEntryPointDesc1.Text = ""
        lblInProcFreqType1.Text = ""
        txtInProcNoOfFreq1.Text = ""
        lblNoOfLanes1.Text = ""
        lblNoOfSamples1.Text = ""
        txtExpiryCount1.Text = ""
        lblTestSpecDesc1.Text = ""
        lblTestCategory1.Text = ""
        txtTestFormTitle1.Text = ""
        lblNoteDesc1.Text = ""
        chkActive1.Checked = False
        chkAlert1.Checked = False
        chkAllowOverride1.Checked = False
        chkCopyRecord1.Enabled = False
        chkCopyRecord1.Checked = False
        txtSearch.Text = ""
    End Sub

    Private Sub ClearDropDownValue()
        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        ddlFacility2.SelectedIndex = 0

        Dim ddlEntryPointDesc2 As DropDownList = TryCast(FindControl("ddlEntryPointDesc"), DropDownList)
        ddlEntryPointDesc2.SelectedIndex = 0

        Dim ddlInProcFreqType2 As DropDownList = TryCast(FindControl("ddlInProcFreqType"), DropDownList)
        ddlInProcFreqType2.SelectedIndex = 0
        Dim ddlNoOfLanes2 As DropDownList = TryCast(FindControl("ddlNoOfLanes"), DropDownList)
        ddlNoOfLanes2.SelectedIndex = 0

        Dim ddlNoOfSamples2 As DropDownList = TryCast(FindControl("ddlNoOfSamples"), DropDownList)
        ddlNoOfSamples2.SelectedIndex = 0

        Dim ddlTestSpecDesc2 As DropDownList = TryCast(FindControl("ddlTestSpecDesc"), DropDownList)
        ddlTestSpecDesc2.SelectedIndex = 0

        Dim ddlTestCategory2 As DropDownList = TryCast(FindControl("ddlTestCategory"), DropDownList)
        ddlTestCategory2.SelectedIndex = 0

        Dim ddlNoteDesc2 As DropDownList = TryCast(FindControl("ddlNoteDesc"), DropDownList)
        ddlNoteDesc2.SelectedIndex = 0
    End Sub
    Private Function CheckInput(ByVal strTemp As String) As Boolean
        If strTemp.ToString() IsNot Nothing AndAlso strTemp.ToString() <> String.Empty Then
            Return True
        End If
        Return False
    End Function
    Private Function CheckNoOfFreqType(ByVal strFreqType As String, ByVal strNoOfFreq As String, ByVal strEntryPoint As String) As Boolean
        If strEntryPoint = "I" Then
            If strFreqType.Trim.Length > 0 And CInt(strNoOfFreq) > 0 Then
                Return True
            End If
        Else
            If strFreqType.Trim.Length = 0 Or CInt(strNoOfFreq) = 0 Then
                Return True
                'If CInt(strFreqType) = 0 Then
                '    If CInt(strNoOfFreq) = 0 Then
                '        Return True
                '    End If
                'Else
                '    If CInt(strNoOfFreq) > 0 Then
                '        Return True
                '    End If
                'End If
            End If
        End If
        Return False
    End Function
    Private Function CheckDuplicateDescription(ByVal strDesc As String) As Boolean
        Dim str As String = String.Empty
        str = "Select QATDefnID From tblQATDefinition" & _
            " Where QATDefnDescription = '" & strDesc & "'"
        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
            Return True
            End If
        Return False
    End Function
    Private Function CheckDuplicateKey(ByVal intQATDefnID As Integer) As Boolean
        Dim str As String = String.Empty
        str = "Select QATDefnDescription From tblQATDefinition" & _
            " Where QATDefnID = " & intQATDefnID

        sql.ExecQuery(str)
        intRowCount = sql.DBDT.Rows.Count
        If intRowCount > 0 Then
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
    Private Function GetCurrentValue() As Boolean
        Dim strFacility As String = String.Empty
        Dim strEntryPointDesc As String = String.Empty
        Dim strInProcFreqType As String = String.Empty
        Dim strInProcNoOfFreq As String = String.Empty
        Dim strNoOfLanes As String = String.Empty
        Dim strNoOfSamples As String = String.Empty
        Dim strTestSpecDesc As String = String.Empty
        Dim strTestCategory As String = String.Empty
        Dim strNoteDesc As String = String.Empty

        Dim ddlFacility2 As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        strFacility = ddlFacility2.SelectedValue.ToString
        If CheckInput(strFacility) = True Then
            lblFacility1.Text = strFacility
        Else
            lblErrMsg.Text = "Please select Facility."
            Return False
            Exit Function
        End If

        Dim ddlEntryPointDesc2 As DropDownList = TryCast(FindControl("ddlEntryPointDesc"), DropDownList)
        strEntryPointDesc = ddlEntryPointDesc2.SelectedValue.ToString
        If CheckInput(strEntryPointDesc) = True Then
            lblEntryPointDesc1.Text = strEntryPointDesc
        Else
            lblErrMsg.Text = "Please select Entry Point."
            Return False
            Exit Function
        End If

        Dim ddlInProcFreqType2 As DropDownList = TryCast(FindControl("ddlInProcFreqType"), DropDownList)
        strInProcFreqType = ddlInProcFreqType2.SelectedValue.ToString
        If CheckInput(strInProcFreqType) = True Then
            If ddlEntryPointDesc2.SelectedValue.ToString = "I" Then
                lblInProcFreqType1.Text = strInProcFreqType
            Else
                lblErrMsg.Text = "In Prococess Freq Type must be blank for Entry Point is not In-Process."
                Return False
                Exit Function
            End If
        Else
            If ddlEntryPointDesc2.SelectedValue.ToString = "I" Then
                lblErrMsg.Text = "Please select In Prococess Freq Type."
                Return False
                Exit Function
            End If
        End If

            Dim ddlNoOfLanes2 As DropDownList = TryCast(FindControl("ddlNoOfLanes"), DropDownList)
            strNoOfLanes = ddlNoOfLanes2.SelectedValue.ToString
            If CheckInput(strNoOfLanes) = True Then
                lblNoOfLanes1.Text = strNoOfLanes
            Else
                lblErrMsg.Text = "Please select No Of Lanes."
                Return False
                Exit Function
            End If


            Dim ddlNoOfSamples2 As DropDownList = TryCast(FindControl("ddlNoOfSamples"), DropDownList)
            strNoOfSamples = ddlNoOfSamples2.SelectedValue.ToString
            If CheckInput(strNoOfSamples) = True Then
                lblNoOfSamples1.Text = strNoOfSamples
            Else
                lblErrMsg.Text = "Please select No Of Samples."
                Return False
                Exit Function
            End If

            Dim ddlTestSpecDesc2 As DropDownList = TryCast(FindControl("ddlTestSpecDesc"), DropDownList)
            strTestSpecDesc = ddlTestSpecDesc2.SelectedValue.ToString
            If CheckInput(strTestSpecDesc) = True Then
                lblTestSpecDesc1.Text = strTestSpecDesc
            Else
                lblErrMsg.Text = "Please select Test Specification."
                Return False
                Exit Function
            End If

            Dim ddlTestCategory2 As DropDownList = TryCast(FindControl("ddlTestCategory"), DropDownList)
            strTestCategory = ddlTestCategory2.SelectedValue.ToString
            If CheckInput(strTestCategory) = True Then
                lblTestCategory1.Text = strTestCategory
            Else
                lblErrMsg.Text = "Please select Test Category."
                Return False
                Exit Function
            End If

            Dim ddlNoteDesc2 As DropDownList = TryCast(FindControl("ddlNoteDesc"), DropDownList)
            strNoteDesc = ddlNoteDesc2.SelectedValue.ToString
            If CheckInput(strNoteDesc) = True Then
                lblNoteDesc1.Text = strNoteDesc
            Else
                lblErrMsg.Text = "Please select Note."
                Return False
                Exit Function
            End If
            Return True
    End Function

    Private Sub DeleteRecord(ByVal intQATDefnID As Integer, ByVal strDescription As String)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = "Delete From tblQATDefinition " & _
            " Where QATDefnID = @qatdefnid"
            sql.AddParam("@qatdefnid", intQATDefnID)
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
    Private Sub UpdateRecord(ByVal strQATDefnID As String, ByVal intActive As Integer, ByVal strFacility As String, ByVal intAlert As Integer,
                            ByVal intAllowOverride As Integer, ByVal intExpiryCount As Integer, ByVal intInProcFreqType As Integer, ByVal intInProcNoOfFreq As Integer,
                            ByVal intNoOfLanes As Integer, ByVal intNoOfSamples As Integer, ByVal intNoteID As Integer, ByVal strQATDefnDescription As String,
                            ByVal strQATEntryPoint As String, ByVal intTestFormID As Integer, ByVal strTestFormTitle As String,
                            ByVal intTestSpecID As Integer, ByVal strUser As String, ByVal intCopyRecord As Integer)
        Try
            Dim lblMsg As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim str As String = String.Empty

            If btnSave.Text = "Save New" Or intCopyRecord = 1 Then
                str = "INSERT INTO tblQATDefinition " & _
                    " (Active, Facility, Alert, AllowOverride, ExpiryCount, InProcFreqType, InProcNoOfFreq, NoOfLanes, NoOfSamples, NoteID," & _
                    " QATDefnDescription, QATEntryPoint, TestFormID, TestFormTitle, TestSpecID, UpdatedAt, UpdatedBy) " & _
                    " VALUES (@active, @facility, @alert, @allowOverride, @expirycount, @inprocfreqtype, @inprocnooffreq, @nooflanes, @noofsamples, @noteid," & _
                    " @qatdefndescription, @qatentrypoint, @testformid, @testformtitle, @testspecid, @updatedat, @updatedby) "
                lblMsg = "Add new record completed!"
            Else
                str = "UPDATE tblQATDefinition SET Active = @active, " & _
                    " Facility=@facility, Alert=@alert, AllowOverride=@allowOverride, ExpiryCount=@expirycount, InProcFreqType=@inprocfreqtype, InProcNoOfFreq=@inprocnooffreq, " & _
                    " NoOfLanes=@nooflanes, NoOfSamples=@noofsamples, NoteID=@noteid, QATDefnDescription=@qatdefndescription, QATEntryPoint=@qatentrypoint, " & _
                    " TestFormID=@testformid, TestFormTitle=@testformtitle, TestSpecID=@testspecid, UpdatedAt=@updatedat, UpdatedBy=@updatedby " & _
                    " WHERE QATDefnID=@qatdefnid"
                lblMsg = "Update record completed!"
            End If
            sql.AddParam("@qatdefnid", CInt(strQATDefnID))
            sql.AddParam("@active", intActive)
            sql.AddParam("@facility", strFacility)
            sql.AddParam("@alert", intAlert)
            sql.AddParam("@allowoverride", intAllowOverride)
            sql.AddParam("@expirycount", intExpiryCount)
            sql.AddParam("@inprocfreqtype", intInProcFreqType)
            sql.AddParam("@inprocnooffreq", intInProcNoOfFreq)
            sql.AddParam("@nooflanes", intNoOfLanes)
            sql.AddParam("@noofsamples", intNoOfSamples)
            sql.AddParam("@noteid", intNoteID)
            sql.AddParam("@qatdefndescription", strQATDefnDescription)
            sql.AddParam("@qatentrypoint", strQATEntryPoint)
            sql.AddParam("@testformid", intTestFormID)
            sql.AddParam("@testformtitle", strTestFormTitle)
            sql.AddParam("@testspecid", intTestSpecID)
            sql.AddParam("@updatedat", Now.ToString)
            sql.AddParam("@updatedby", strUser)
            GetData(str)

            strMsg = sql.HasExceptionMsg(True)
            If strMsg = "" Then
                ClearInput()
                txtSearch.Text = strQATDefnDescription
                BindSearchData(strQATDefnDescription)
                lblErrMsg.Text = lblMsg
            Else
                If strMsg.Contains("Cannot insert duplicate key") Then
                    lblErrMsg.Text = "Duplicate key found - " & strQATDefnID & ". Update process aborted!"
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

        Dim strQATDefnID9 As String = String.Empty
        Dim strFacility As String = String.Empty
        Dim strQATDefnDescription As String = String.Empty
        Dim strQATEntryPoint As String = String.Empty
        Dim strTestFormTitle As String = String.Empty
        Dim strUser As String = String.Empty
        Dim intActive As Integer
        Dim intAlert As Integer
        Dim intAllowOverride As Integer
        Dim intInProcFreqType As Integer
        Dim intInProcNoOfFreq As Integer
        Dim intNoteID As Integer
        Dim intTestFormID As Integer
        Dim intTestSpecID As Integer
        Dim intNoOfLanes As Integer
        Dim intNoOfSamples As Integer
        Dim intCopyRecord As Integer
        Dim intExpiryCount As Integer
        Try
            If GetCurrentValue() = True Then
                Dim IsActive As CheckBox = TryCast(FindControl("chkActive1"), CheckBox)
                If IsActive.Checked Then
                    intActive = 1
                Else
                    intActive = 0
                End If

                Dim IsAlert As CheckBox = TryCast(FindControl("chkAlert1"), CheckBox)
                If IsAlert.Checked Then
                    intAlert = 1
                Else
                    intAlert = 0
                End If

                Dim IsAllowOverride As CheckBox = TryCast(FindControl("chkAllowOverride1"), CheckBox)
                If IsAllowOverride.Checked Then
                    intAllowOverride = 1
                Else
                    intAllowOverride = 0
                End If
                Dim IsCopyRecord As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
                If IsCopyRecord.Checked Then
                    intCopyRecord = 1
                Else
                    intCopyRecord = 0
                End If
                strQATDefnID9 = TryCast(FindControl("lblQATDefnID9"), Label).Text
                strFacility = TryCast(FindControl("lblFacility1"), Label).Text
                strQATDefnDescription = TryCast(FindControl("txtQATDefnDescription1"), TextBox).Text
                strQATEntryPoint = TryCast(FindControl("lblEntryPointDesc1"), Label).Text

                If TryCast(FindControl("lblInProcFreqType1"), Label).Text.Trim.Length > 0 Then
                    intInProcFreqType = TryCast(FindControl("lblInProcFreqType1"), Label).Text
                Else
                    intInProcFreqType = 0
                End If

                If TryCast(FindControl("txtInProcNoOfFreq1"), TextBox).Text.Trim.Length > 0 Then
                    intInProcNoOfFreq = TryCast(FindControl("txtInProcNoOfFreq1"), TextBox).Text
                Else
                    intInProcNoOfFreq = 0
                End If

                intNoOfLanes = TryCast(FindControl("lblNoOfLanes1"), Label).Text
                intNoOfSamples = TryCast(FindControl("lblNoOfSamples1"), Label).Text
                intExpiryCount = TryCast(FindControl("txtExpiryCount1"), TextBox).Text
                intTestSpecID = TryCast(FindControl("lblTestSpecDesc1"), Label).Text
                intTestFormID = TryCast(FindControl("lblTestCategory1"), Label).Text
                strTestFormTitle = TryCast(FindControl("txtTestFormTitle1"), TextBox).Text
                intNoteID = TryCast(FindControl("lblNoteDesc1"), Label).Text
                strUser = TryCast(FindControl("lblUserName"), Label).Text

                If strQATDefnID9 = "0" Then
                    If CheckDuplicateKey(strQATDefnID9) Then
                        lblErrMsg.Text = "Duplicate key found - " & strFacility & "/" & strQATDefnID9 & ". Update process aborted!"
                        Exit Sub
                    End If
                End If

                UpdateRecord(strQATDefnID9, intActive, Trim(strFacility), intAlert, intAllowOverride, intExpiryCount, _
                             intInProcFreqType, intInProcNoOfFreq, intNoOfLanes, intNoOfSamples, intNoteID, strQATDefnDescription, _
                             strQATEntryPoint, intTestFormID, strTestFormTitle, intTestSpecID, strUser, intCopyRecord)
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
        Dim strQATDefnDesc As String = String.Empty
        Dim strEntryPoint As String = String.Empty
        Dim strInProcFreqType As String = String.Empty
        Dim strInProcNoOfFreq As String = String.Empty
        Dim strNoOfLanes As String = String.Empty
        Dim strNoOfSamples As String = String.Empty
        Dim strTestSpecDesc As String = String.Empty
        Dim strTestCategory As String = String.Empty
        Dim strTestFormTitle As String = String.Empty
        Dim strNoteDesc As String = String.Empty
        Dim strExpiryCount As String = String.Empty

        Dim intActive As Integer = 0
        Dim intAlert As Integer = 0
        Dim intAllowOverride As Integer = 0

        lblQATDefnID9.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnID"), Label).Text
        Dim IsActive As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbActive"), CheckBox)
        If IsActive.Checked Then
            intActive = 1
            chkActive1.Checked = True
        Else
            intActive = 0
            chkActive1.Checked = False
        End If

        Dim IsAlert As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbAlert"), CheckBox)
        If IsAlert.Checked Then
            intAlert = 1
            chkAlert1.Checked = True
        Else
            intAlert = 0
            chkAlert1.Checked = False
        End If

        Dim IsAllowOverride As CheckBox = TryCast(gvForm.SelectedRow.FindControl("ckbAllowOverride"), CheckBox)
        If IsAllowOverride.Checked Then
            intAllowOverride = 1
            chkAllowOverride1.Checked = True
        Else
            intAllowOverride = 0
            chkAllowOverride1.Checked = False
        End If

        strFacility = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        lblFacility1.Text = TryCast(gvForm.SelectedRow.FindControl("lblFacility"), Label).Text
        If strFacility.Length = 2 Then
            strFacility = strFacility & " "
        End If
        ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(strFacility))

        txtQATDefnDescription1.Text = TryCast(gvForm.SelectedRow.FindControl("lblQATDefnDescription"), Label).Text

        strEntryPoint = TryCast(gvForm.SelectedRow.FindControl("lblQATEntryPoint"), Label).Text
        lblEntryPointDesc1.Text = TryCast(gvForm.SelectedRow.FindControl("lblEntryPointDesc"), Label).Text
        ddlEntryPointDesc.SelectedIndex = ddlEntryPointDesc.Items.IndexOf(ddlEntryPointDesc.Items.FindByValue(strEntryPoint))

        strInProcFreqType = TryCast(gvForm.SelectedRow.FindControl("lblInProcFreqType"), Label).Text
        lblInProcFreqType1.Text = TryCast(gvForm.SelectedRow.FindControl("lblInProcFreqType"), Label).Text
        ddlInProcFreqType.SelectedIndex = ddlInProcFreqType.Items.IndexOf(ddlInProcFreqType.Items.FindByValue(strInProcFreqType))

        txtInProcNoOfFreq1.Text = TryCast(gvForm.SelectedRow.FindControl("lblInProcNoOfFreq"), Label).Text

        strNoOfLanes = TryCast(gvForm.SelectedRow.FindControl("lblNoOfLanes"), Label).Text
        lblNoOfLanes1.Text = TryCast(gvForm.SelectedRow.FindControl("lblNoOfLanes"), Label).Text
        ddlNoOfLanes.SelectedIndex = ddlNoOfLanes.Items.IndexOf(ddlNoOfLanes.Items.FindByValue(strNoOfLanes))

        strNoOfSamples = TryCast(gvForm.SelectedRow.FindControl("lblNoOfSamples"), Label).Text
        lblNoOfSamples1.Text = TryCast(gvForm.SelectedRow.FindControl("lblNoOfSamples"), Label).Text
        ddlNoOfSamples.SelectedIndex = ddlNoOfSamples.Items.IndexOf(ddlNoOfSamples.Items.FindByValue(strNoOfSamples))

        txtExpiryCount1.Text = TryCast(gvForm.SelectedRow.FindControl("lblExpiryCount"), Label).Text

        strTestSpecDesc = TryCast(gvForm.SelectedRow.FindControl("lblTestSpecID"), Label).Text
        lblTestSpecDesc1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestSpecID"), Label).Text
        ddlTestSpecDesc.SelectedIndex = ddlTestSpecDesc.Items.IndexOf(ddlTestSpecDesc.Items.FindByValue(strTestSpecDesc))

        strTestCategory = TryCast(gvForm.SelectedRow.FindControl("lblTestFormID"), Label).Text
        lblTestCategory1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestFormID"), Label).Text
        ddlTestCategory.SelectedIndex = ddlTestCategory.Items.IndexOf(ddlTestCategory.Items.FindByValue(strTestCategory))

        strNoteDesc = TryCast(gvForm.SelectedRow.FindControl("lblNoteID"), Label).Text
        lblNoteDesc1.Text = TryCast(gvForm.SelectedRow.FindControl("lblNoteID"), Label).Text
        ddlNoteDesc.SelectedIndex = ddlNoteDesc.Items.IndexOf(ddlNoteDesc.Items.FindByValue(strNoteDesc))
        txtTestFormTitle1.Text = TryCast(gvForm.SelectedRow.FindControl("lblTestFormTitle"), Label).Text

        chkCopyRecord1.Enabled = True
        chkCopyRecord1.Checked = False

        btnSave.Text = "Save"
    End Sub

    Protected Sub gvForm_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(3).VerticalAlign = VerticalAlign.Top
            For i = 6 To 15
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells(i).VerticalAlign = VerticalAlign.Top
            Next
            For i = 16 To 21
                e.Row.Cells(i).Visible = False
            Next
        End If
    End Sub

    Protected Sub gvForm_RowCreated(sender As Object, e As GridViewRowEventArgs)
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(3).VerticalAlign = VerticalAlign.Top
            For i = 6 To 15
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells(i).VerticalAlign = VerticalAlign.Top
            Next
            For i = 16 To 21
                e.Row.Cells(i).Visible = False
            Next
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
        Dim strQATDefnID9 As String = String.Empty
        Dim intQATDefnID As Integer
        Dim strDescription As String = String.Empty
        strDescription = txtQATDefnDescription1.Text
        strQATDefnID9 = lblQATDefnID9.Text

        If CheckInput(strQATDefnID9) Then
            If strQATDefnID9 = "0" Then
                lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
            Else
                intQATDefnID = TryCast(FindControl("lblQATDefnID9"), Label).Text
                DeleteRecord(intQATDefnID, strDescription)
            End If
        Else
            lblErrMsg.Text = "Please pick one record to Delete. Delete process aborted!"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim intInProcNoOfFreq As Integer
        Dim intInProcFreqType As Integer
        Dim IsCopyRecord As CheckBox = TryCast(FindControl("chkCopyRecord1"), CheckBox)
        Dim intCopyRecord As Integer
        If IsCopyRecord.Checked Then
            intCopyRecord = 1
        Else
            intCopyRecord = 0
        End If

        If txtInProcNoOfFreq1.Text.Trim.Length > 0 Then
            Integer.TryParse(txtInProcNoOfFreq1.Text, intInProcNoOfFreq)
        Else
            intInProcNoOfFreq = 0
        End If

        If lblInProcFreqType1.Text.Trim.Length > 0 Then
            Integer.TryParse(lblInProcFreqType1.Text, intInProcFreqType)
        Else
            intInProcFreqType = 0
        End If

        If CheckInput(lblQATDefnID9.Text) Then
            If CheckInput(lblFacility1.Text) Then
                If CheckInput(txtQATDefnDescription1.Text) Then
                    If CheckInput(lblEntryPointDesc1.Text) Then
                        If CheckInput(lblInProcFreqType1.Text) AndAlso ddlEntryPointDesc.SelectedValue = "I" _
                            Or (Not CheckInput(lblInProcFreqType1.Text) Or intInProcFreqType = 0) AndAlso ddlEntryPointDesc.SelectedValue <> "I" Then
                            If CheckInput(txtInProcNoOfFreq1.Text) And ToInteger32(txtInProcNoOfFreq1.Text) AndAlso ddlEntryPointDesc.SelectedValue = "I" _
                                Or (Not CheckInput(txtInProcNoOfFreq1.Text) Or intInProcNoOfFreq = 0) AndAlso ddlEntryPointDesc.SelectedValue <> "I" Then
                                If CheckNoOfFreqType(lblInProcFreqType1.Text, intInProcNoOfFreq.ToString, ddlEntryPointDesc.SelectedValue) Then
                                    If CheckInput(lblNoOfLanes1.Text) Then
                                        If CheckInput(lblNoOfSamples1.Text) Then
                                            If CheckInput(txtExpiryCount1.Text) And ToInteger32(txtExpiryCount1.Text) Then
                                                If CheckInput(lblTestSpecDesc1.Text) Then
                                                    If CheckInput(lblTestCategory1.Text) Then
                                                        If CheckInput(lblNoteDesc1.Text) Then
                                                            If intCopyRecord = 1 Then
                                                                If CheckDuplicateDescription(txtQATDefnDescription1.Text) Then
                                                                    lblErrMsg.Text = "Duplicated Description found. Update process aborted!"
                                                                Else
                                                                    SaveRecord()
                                                                End If
                                                            Else
                                                                SaveRecord()
                                                            End If
                                                        Else
                                                            lblErrMsg.Text = "Please select Note. Update process aborted!"
                                                        End If
                                                    Else
                                                        lblErrMsg.Text = "Please select Test Category. Update process aborted!"
                                                    End If
                                                Else
                                                    lblErrMsg.Text = "Please select Test Specification. Update process aborted!"
                                                End If
                                            Else
                                                lblErrMsg.Text = "Invalid Expiry Count entered. Update process aborted!"
                                            End If
                                        Else
                                            lblErrMsg.Text = "Please select No Of Samples. Update process aborted!"
                                        End If
                                    Else
                                        lblErrMsg.Text = "Please select No Of Lanes. Update process aborted!"
                                    End If
                                Else
                                    If ddlEntryPointDesc.SelectedValue = "I" Then
                                        If intInProcNoOfFreq <= 0 Then
                                            lblErrMsg.Text = "In Process No Of Freq should be greater than 0 since the In-Process Freq. Type is not blank. Update process aborted!"
                                            Exit Sub
                                        End If
                                        'If CInt(lblInProcFreqType1.Text) = 0 Then
                                        '    If CInt(txtInProcNoOfFreq1.Text) <> 0 Then
                                        '        lblErrMsg.Text = "In Process No Of Freq should be 0. Update process aborted!"
                                        '        Exit Sub
                                        '    End If
                                        'End If
                                        'If CInt(lblInProcFreqType1.Text) > 0 Then
                                        '    If CInt(txtInProcNoOfFreq1.Text) = 0 Then
                                        '        lblErrMsg.Text = "In Process No Of Freq should not be 0. Update process aborted!"
                                        '        Exit Sub
                                        '    End If
                                        'End If
                                    Else
                                        If intInProcNoOfFreq > 0 Then
                                            lblErrMsg.Text = "In Process No Of Freq should be 0 since the In-Process Freq. Type is blank. Update process aborted!"
                                            Exit Sub
                                        End If
                                        'If CInt(lblInProcFreqType1.Text) <> 0 Then
                                        '    lblErrMsg.Text = "In Process Freq. Type should be blank. Update process aborted!"
                                        '    Exit Sub
                                        'End If
                                        'If CInt(txtInProcNoOfFreq1.Text) <> 0 Then
                                        '    lblErrMsg.Text = "In Process No Of Freq should be 0. Update process aborted!"
                                        '    Exit Sub
                                        'End If
                                    End If
                                    lblErrMsg.Text = "Invalid In Process No Of Freq entered. Update process aborted!"
                                End If
                            Else
                                'lblErrMsg.Text = "Please enter In Process No Of Freq - positive numbers only. Update process aborted!"
                                If ddlEntryPointDesc.SelectedValue = "I" Then
                                    lblErrMsg.Text = "Please enter In Process No Of Freq - positive numbers only since the In Process Freq Type is not blank. Update process aborted!"
                                Else
                                    lblErrMsg.Text = "Please leave In Process No Of Freq to zero since the In Process Freq Type is blank. Update process aborted!"
                                End If
                            End If
                        Else
                            If ddlEntryPointDesc.SelectedValue = "I" Then
                                lblErrMsg.Text = "Please select In Process Freq Type since the Entry Point is In-Process. Update process aborted!"
                            Else
                                lblErrMsg.Text = "Please leave In Process Freq Type to blank since the Entry Point is not In-Process. Update process aborted!"
                            End If
                        End If

                    Else
                        lblErrMsg.Text = "Please select Entry Point. Update process aborted!"
                    End If
                Else
                    lblErrMsg.Text = "Please select Description. Update process aborted!"
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
    Protected Sub ddlEntryPointDesc_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlEntryPointDesc"), DropDownList)
        lblEntryPointDesc1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlInProcFreqType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlInProcFreqType"), DropDownList)
        lblInProcFreqType1.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ddlNoOfLanes_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlNoOfLanes"), DropDownList)
        lblNoOfLanes1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlNoOfSamples_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlNoOfSamples"), DropDownList)
        lblNoOfSamples1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlTestSpecDesc_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlTestSpecDesc"), DropDownList)
        lblTestSpecDesc1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlTestCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlTestCategory"), DropDownList)
        lblTestCategory1.Text = ddl.SelectedValue.ToString
    End Sub

    Protected Sub ddlNoteDesc_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlNoteDesc"), DropDownList)
        lblNoteDesc1.Text = ddl.SelectedValue.ToString
    End Sub
End Class