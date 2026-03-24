Public Class _Default
    Inherits System.Web.UI.Page
    Dim sql As New SQLCentral
    Dim db As New PowerPlantDbContext
    Private overrideMachines As List(Of OverrideMachineRate)
    Private overrideMachineViews As List(Of OverrideMachineRateView)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetValidator()
        If Not IsPostBack Then
            lblUserName.Text = sql.GetCurrentUserName()
            BindFacilityData()
            ddlFacility.SelectedValue = GetDefaultFacility()
            lblFacility1.Text = ddlFacility.SelectedValue
            ddlPackagingLineSearch_BindData()
            Panel1.Visible = False
        End If
    End Sub

#Region "Facility Data"
    Private Sub BindFacilityData()
        ddlFacility.DataSource = db.Facilities.ToList()
        ddlFacility.DataValueField = "Facility"
        ddlFacility.DataTextField = "Description"
        ddlFacility.DataBind()
    End Sub
    Private Function GetDefaultFacility() As String
        Dim facilityNumber = db.ControlEntity.Where(Function(item) item.Key = "Facility").FirstOrDefault().Value1
        If facilityNumber = Nothing Then
            Return "00"
        Else
            Return facilityNumber
        End If
    End Function
    Protected Sub ddlFacility_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlFacility"), DropDownList)
        lblFacility1.Text = ddl.SelectedValue.ToString
        ddlPackagingLineSearch_BindData()
        ddlPackagingLineSearch.SelectedIndex = 0
        Panel1.Visible = False
    End Sub
#End Region

#Region "Machine Override Rate Data"
    Private Sub GetOverrideMachinsData()
        overrideMachines = db.OverrideMachineRates.ToList()
        Dim query = From m In overrideMachines Join eq In db.Equipments On eq.EquipmentID Equals m.MachineID
                    Select New OverrideMachineRateView With {.RRN = m.RRN, .Facility = m.Facility, .Active = m.Active, .Description = eq.Description, .ItemNumber = m.ItemNumber,
                    .LogicForRateMultiplier = m.LogicForRateMultiplier, .MachineID = m.MachineID, .RateMultiplier = m.RateMultiplier, .RunOperatorsMultiplier = m.RunOperatorsMultiplier}
        overrideMachineViews = query.ToList()
    End Sub
    Private Sub GetOverrideMachinsDataByEquipmentID(pkgId As String)
        overrideMachines = db.OverrideMachineRates.Where(Function(pkg) pkg.MachineID = pkgId And pkg.Facility = ddlFacility.SelectedValue).ToList
        Dim query = From m In overrideMachines Join eq In db.Equipments On eq.EquipmentID Equals m.MachineID
                    Select New OverrideMachineRateView With {.RRN = m.RRN, .Facility = m.Facility, .Active = m.Active, .Description = eq.Description, .ItemNumber = m.ItemNumber,
                        .LogicForRateMultiplier = m.LogicForRateMultiplier, .MachineID = m.MachineID, .RateMultiplier = m.RateMultiplier, .RunOperatorsMultiplier = m.RunOperatorsMultiplier}
        overrideMachineViews = query.ToList()
    End Sub
#End Region

#Region "ddlPackagingLineSearch"
    Private Sub ddlPackagingLineSearch_BindData()
        GetOverrideMachinsData()

        Dim queryOverrideMachines = From machine In overrideMachineViews
                                    Group By machine.MachineID Into m = Group
                                    Select New With {.LineItem = m.FirstOrDefault.MachineID & m.FirstOrDefault.Description, .LineValue = m.FirstOrDefault.MachineID}
        DdlBind(ddlPackagingLineSearch, queryOverrideMachines.ToList())
    End Sub
    Protected Sub ddlPackagingLineSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = TryCast(FindControl("ddlPackagingLineSearch"), DropDownList)
        txtSearch.Text = ddl.SelectedValue.ToString
    End Sub
    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click
        Try
            GetOverrideMachinsData()
            SortDirection = SortDirection.Descending
            gvOverrideRates_BindData(pageIndex:=0)
            ddlLine_BindData()
            Panel1.Visible = True
            ClearSelections()
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
#End Region

#Region "gvOverrideRates"
    Private Sub gvOverrideRates_BindData(pageIndex As Integer)
        Dim strSearch = ddlPackagingLineSearch.SelectedValue.ToString
        If (String.IsNullOrEmpty(ViewState("SortExpression"))) Then
            ViewState("SortExpression") = "MachineID"
        End If
        GetOverrideMachinsData()

        Dim query As IOrderedEnumerable(Of OverrideMachineRateView)
        Select Case ViewState("SortExpression")
            Case "MachineID"
                If SortDirection = SortDirection.Descending Then
                    query = From item In overrideMachineViews Order By item.MachineID
                Else
                    query = From item In overrideMachineViews Order By item.MachineID Descending
                End If
            Case "Description"
                If SortDirection = SortDirection.Descending Then
                    query = From item In overrideMachineViews Order By item.Description
                Else
                    query = From item In overrideMachineViews Order By item.Description Descending
                End If
            Case "ItemNumber"
                If SortDirection = SortDirection.Descending Then
                    query = From item In overrideMachineViews Order By item.ItemNumber
                Else
                    query = From item In overrideMachineViews Order By item.ItemNumber Descending
                End If
            Case "RateMultiplier"
                If SortDirection = SortDirection.Descending Then
                    query = From item In overrideMachineViews Order By item.RateMultiplier
                Else
                    query = From item In overrideMachineViews Order By item.RateMultiplier Descending
                End If
            Case "RunOperatorsMultiplier"
                If SortDirection = SortDirection.Descending Then
                    query = From item In overrideMachineViews Order By item.RunOperatorsMultiplier
                Else
                    query = From item In overrideMachineViews Order By item.RunOperatorsMultiplier Descending
                End If
        End Select
        overrideMachineViews = query.ToList()

        If strSearch = "" Then
            gvOverrideRates.DataSource = overrideMachineViews
            'gvOverrideRates.DataSource = overrideMachineViews.Skip(gvOverrideRates.PageSize * pageIndex).ToList()
        Else
            gvOverrideRates.DataSource = overrideMachineViews.Where(Function(m) m.MachineID = ddlPackagingLineSearch.SelectedValue.ToString).ToList()
        End If
        gvOverrideRates.PageIndex = pageIndex
        gvOverrideRates.DataBind()
    End Sub
    Protected Sub gvOverrideRates_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rrnKey = gvOverrideRates.SelectedDataKey.Value
        Dim overrideMachine = db.OverrideMachineRates.Find(rrnKey)

        'Write to the editing form and disable Drop Down List
        ddlLine.Text = overrideMachine.MachineID
        lblRRN.Text = rrnKey
        'ddlLine.Enabled = False
        ddlItemNumber_BindData()
        ddlItemNumber.Text = overrideMachine.ItemNumber
        'ddlItemNumber.Enabled = False
        txtRateMultiplier.Text = overrideMachine.RateMultiplier
        txtLogicForRateMultiplier.Text = overrideMachine.LogicForRateMultiplier
        txtRunOperatorsMultiplier.Text = overrideMachine.RunOperatorsMultiplier
        chkActive.Checked = overrideMachine.Active
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
    Protected Sub gvOverrideRates_Sorting(sender As Object, e As GridViewSortEventArgs)
        If SortDirection = SortDirection.Ascending Then
            SortDirection = SortDirection.Descending
        Else
            SortDirection = SortDirection.Ascending
        End If
        ViewState("SortExpression") = e.SortExpression
        gvOverrideRates_BindData(gvOverrideRates.PageIndex)
        ClearSelections()
    End Sub
    Protected Sub gvForm_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvOverrideRates_BindData(e.NewPageIndex)
    End Sub
#End Region

#Region "ddlLine"
    Private Sub ddlLine_BindData()
        Dim pkgLines = From pkg In db.Equipments Where pkg.Active = True Order By pkg.EquipmentID
                       Select LineItem = pkg.EquipmentID & pkg.Description, LineValue = pkg.EquipmentID
        DdlBind(ddlLine, pkgLines.ToList())
    End Sub
    Protected Sub ddlLine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLine.SelectedIndexChanged
        ddlItemNumber_BindData()
    End Sub
#End Region

#Region "ddlItemNumber"
    Private Sub ddlItemNumber_BindData()
        Dim stdRates = From rate In db.StdMachineRates
                       Where rate.MachineID = ddlLine.SelectedValue And rate.Facility = ddlFacility.SelectedValue
                       Group By rate.ItemNumber Into Machines = Group

        Dim items = From rate In stdRates Join item In db.ItemMasters On rate.ItemNumber Equals item.ItemNumber
                    Select LineItem = item.ItemNumber & " " & item.ItemDesc1 & " " & item.ItemDesc2, LineValue = item.ItemNumber
        DdlBind(ddlItemNumber, items.ToList())
    End Sub
#End Region

#Region "EditInput"
    Private Sub SetValidator()
        vldRateMultiplier.MinimumValue = ConfigurationManager.AppSettings.Get("RateMultiplierMin")
        vldRateMultiplier.MaximumValue = ConfigurationManager.AppSettings.Get("RateMultiplierMax")
        vldRateMultiplier.Text = "Rate Multiplier must be between " & vldRateMultiplier.MinimumValue & " to " & vldRateMultiplier.MaximumValue
        vldRunOperatorsMultiplier.MinimumValue = ConfigurationManager.AppSettings.Get("RunOperatorsMultiplierMin")
        vldRunOperatorsMultiplier.MaximumValue = ConfigurationManager.AppSettings.Get("RunOperatorsMultiplierMax")
        vldRunOperatorsMultiplier.Text = "Run Operators Multiplier must be between " & vldRunOperatorsMultiplier.MinimumValue & " to " & vldRunOperatorsMultiplier.MaximumValue
    End Sub
    Private Sub ClearSelections()
        gvOverrideRates.SelectedIndex = -1
        ddlLine.SelectedIndex = -1
        ddlLine.Enabled = True
        ddlItemNumber.SelectedIndex = -1
        ddlItemNumber.Enabled = True
        txtRateMultiplier.Text = ""
        txtLogicForRateMultiplier.Text = ""
        txtRunOperatorsMultiplier.Text = ""
        lblRRN.Text = ""
        chkActive.Checked = False
    End Sub
#End Region

#Region "Buttons"
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ClearSelections()
    End Sub
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Try
            Dim overrideMachine = db.OverrideMachineRates.Find(Int(lblRRN.Text))
            If overrideMachine Is Nothing Then
                lblErrMsg.Text = "Can not find same record to delete"
            Else
                db.OverrideMachineRates.Remove(overrideMachine)
                db.SaveChanges()
                lblErrMsg.Text = "Delete record completed!"
                gvOverrideRates_BindData(0)
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim overrideMachine As OverrideMachineRate = Nothing
        Dim overrideMachineNew As OverrideMachineRate = Nothing
        Try
            If Not (String.IsNullOrEmpty(lblRRN.Text)) Then
                overrideMachine = db.OverrideMachineRates.Find(Int(lblRRN.Text))
            End If
            If overrideMachine Is Nothing Then
                overrideMachineNew = New OverrideMachineRate With {
                    .Facility = ddlFacility.SelectedValue,
                    .MachineID = ddlLine.Text,
                    .Active = chkActive.Checked,
                    .RateMultiplier = txtRateMultiplier.Text,
                    .LogicForRateMultiplier = txtLogicForRateMultiplier.Text,
                    .CreatedBy = lblUserName.Text,
                    .ItemNumber = ddlItemNumber.Text,
                    .RunOperatorsMultiplier = txtRunOperatorsMultiplier.Text,
                    .CreatedAt = DateTime.Now,
                    .LastUpdated = DateTime.Now
                }
                db.OverrideMachineRates.Add(overrideMachineNew)
                lblErrMsg.Text = "Save New record completed!"
            Else
                overrideMachine.MachineID = ddlLine.Text
                overrideMachine.Active = chkActive.Checked
                overrideMachine.ItemNumber = ddlItemNumber.Text
                overrideMachine.RateMultiplier = txtRateMultiplier.Text
                overrideMachine.LogicForRateMultiplier = txtLogicForRateMultiplier.Text
                overrideMachine.RunOperatorsMultiplier = txtRunOperatorsMultiplier.Text
                overrideMachine.LastUpdated = DateTime.Now
                lblErrMsg.Text = "Update record completed!"
            End If

            db.SaveChanges()

            GetOverrideMachinsDataByEquipmentID(ddlLine.Text)
            gvOverrideRates.DataSource = overrideMachineViews
            gvOverrideRates.DataBind()
            ddlPackagingLineSearch_BindData()
            ddlPackagingLineSearch.SelectedValue = ddlLine.Text
            ClearSelections()

        Catch ex As Exception
            If ex.InnerException.InnerException.ToString().Contains("Cannot insert duplicate key row") Then
                lblErrMsg.Text = $"Duplicate key found - {overrideMachineNew.Facility}, {overrideMachineNew.MachineID}, {overrideMachineNew.ItemNumber}. Update process aborted!"
            Else
                lblErrMsg.Text = ex.Message
            End If

        End Try
    End Sub
#End Region

#Region "Generic Procedures"
    Private Sub DdlBind(Of T As DropDownList)(ddl As T, list As Object)
        ddl.DataSource = list
        ddl.DataValueField = "LineValue"
        ddl.DataTextField = "LineItem"
        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(String.Empty, String.Empty))
        ddl.SelectedIndex = 0
    End Sub
#End Region
End Class