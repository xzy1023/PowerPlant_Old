Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not Page.IsPostBack) Then
            lblAction.Text = ""
            lblErrMsg.Text = ""
            txtItemNumber.Focus()
            ' WO#17432 ADD Start – AT 11/27/2018
            Tab1.CssClass = "Clicked"
            Tab2.CssClass = "Initial"
            Tab3.CssClass = "Initial"
            Tab4.CssClass = "Initial"
            MainView.ActiveViewIndex = 0
            ' WO#17432 ADD Stop – AT 11/27/2018
        End If

    End Sub

    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            If (Not Page.IsPostBack) Then
                ddlFacility.DataBind()
                ' if the facility value is not provided from the calling page, use the default facility as default selected value
                If Not Request.QueryString("Facility") Is Nothing Then
                    ddlFacility.SelectedValue = Request.QueryString("Facility")
                Else
                    ddlFacility.SelectedValue = Session("strDefaultFacility").ToString
                End If

                If Not ddlFacility.SelectedValue = String.Empty Then
                    If Not Request.QueryString("itemNumber") Is Nothing Then
                        txtItemNumber.Text = Request.QueryString("ItemNumber")
                        LoadDataWithoutKnowingAction()
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        ClearInputOutputFields()
        lblAction.Text = "Mode: ADD"
        ddlFacilityDtl.SelectedValue = ddlFacility.SelectedValue
        If LoadERPData(ddlFacility.SelectedValue, txtItemNumber.Text) = True Then
            If LoadOvrrData("ADD", ddlFacility.SelectedValue, txtItemNumber.Text) = True Then
                lblErrMsg.Text = "Item already has override record, please click on Change button to modify or enter another item."
                Panel1.Visible = False
            Else
                Panel1.Visible = True
            End If
        Else
            lblErrMsg.Text = "Item is not found in the ERP Item Master table."
            Panel1.Visible = False
        End If

    End Sub

    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        ClearInputOutputFields()
        'LoadOvrrData("UPDATE", ddlFacility.SelectedValue, txtItemNumber.Text)

        lblAction.Text = "Mode: UPDATE"
        If LoadERPData(ddlFacility.SelectedValue, txtItemNumber.Text) = True Then
            If LoadOvrrData("UPDATE", ddlFacility.SelectedValue, txtItemNumber.Text) = False Then
                lblErrMsg.Text = "Item does not has override record, please click on Add button to add a new one or enter another item."
                Panel1.Visible = False
            Else
                Panel1.Visible = True
            End If
        Else
            lblErrMsg.Text = "Item is not found in the ERP Item Master table."
            Panel1.Visible = False
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        ClearInputOutputFields()
        lblAction.Text = "Mode: DELETE"
        If LoadERPData(ddlFacility.SelectedValue, txtItemNumber.Text) = True Then
            If LoadOvrrData("DELETE", ddlFacility.SelectedValue, txtItemNumber.Text) = False Then
                lblErrMsg.Text = "Item does not has override record, please enter another item."
                Panel1.Visible = False
            Else
                Panel1.Visible = True
            End If
        Else
            lblErrMsg.Text = "Item is not found in the ERP Item Master table."
            Panel1.Visible = False
        End If
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        Dim daOvr As New dsItemFromOvrTableAdapters.PPsp_ItemLabelOverride_SelTableAdapter
        Dim cmd As SqlClient.SqlCommand
        Dim strErrMsg As String = ""
        Dim strAction As String = ""

        Try
            If Panel1.Visible = False Then
                lblErrMsg.Text = "Please select one of the action buttons and fill in the information before select the Accept button."
            Else
                strAction = lblAction.Text.Substring(6, lblAction.Text.Length - 6)

                If strAction <> "DELETE" Then
                    lblErrMsg.Text = ValidateInputData()
                End If
                If lblErrMsg.Text = "" Then
                    With daOvr.Adapter
                        If strAction = "ADD" Then
                            cmd = daOvr.Adapter.InsertCommand
                        ElseIf strAction = "UPDATE" Then
                            cmd = daOvr.Adapter.UpdateCommand
                        ElseIf strAction = "DELETE" Then
                            cmd = daOvr.Adapter.DeleteCommand
                        Else
                            lblErrMsg.Text = "Error on Accept."
                            Exit Sub
                        End If

                        With cmd.Parameters
                            .Item("@vchAction").Value = strAction
                            .Item("@decBagLength").Value = txtBagLength.Text
                            .Item("@chrBagLengthRequired").Value = rblBagLengthRequired.SelectedValue
                            .Item("@chrCaseLabelDateFmtCode").Value = ddlCaseDateFmtCode.SelectedValue
                            .Item("@vchCaseLabelFmt1").Value = txtCaseLabelFmt1.Text
                            .Item("@vchCaseLabelFmt2").Value = txtCaseLabelFmt2.Text
                            .Item("@vchCaseLabelFmt3").Value = txtCaseLabelFmt3.Text
                            .Item("@chrDateToPrintFlag").Value = ddlDateToPrtOnLabel.SelectedValue
                            .Item("@vchDomicileText1").Value = txtDomicile1.Text
                            .Item("@vchDomicileText2").Value = txtDomicile2.Text
                            .Item("@vchDomicileText3").Value = txtDomicile3.Text
                            .Item("@vchDomicileText4").Value = txtDomicile4.Text
                            .Item("@vchDomicileText5").Value = txtDomicile5.Text
                            .Item("@vchDomicileText6").Value = txtDomicile6.Text
                            .Item("@vchExpiryDateDesc").Value = txtExpiryDateDesc.Text
                            .Item("@vchFacility").Value = ddlFacilityDtl.SelectedValue
                            .Item("@vchFilterCoderFmt").Value = txtFilterCoderFormat.Text
                            .Item("@vchItemDesc1").Value = txtOvrDesc1.Text
                            .Item("@vchItemDesc2").Value = txtOvrDesc2.Text
                            .Item("@vchItemDesc3").Value = txtOvrDesc3.Text
                            .Item("@vchItemNumber").Value = lblItemNumber.Text
                            .Item("@vchNetWeight").Value = txtOvrNetWeight.Text
                            .Item("@chrNetWeightUOM").Value = ddlOvrNetWgtUOM.SelectedValue
                            .Item("@vchOverrideItem").Value = txtOvrItem.Text
                            .Item("@bitOvrDesc1Flag").Value = rblOvrItemDesc1.SelectedValue
                            .Item("@bitOvrNetWeightFlag").Value = rblOvrNetWgt.SelectedValue
                            .Item("@bitOvrNetWeightUOMFlag").Value = rblOvrNetWgtUOM.SelectedValue
                            .Item("@bitOvrPackSizeFlag").Value = rblOvrPackSize.SelectedValue
                            .Item("@vchPackageCoderFmt1").Value = txtPackageCoderFmt1.Text
                            .Item("@vchPackageCoderFmt2").Value = txtPackageCoderFmt2.Text
                            .Item("@vchPackageCoderFmt3").Value = txtPackageCoderFmt3.Text
                            .Item("@vchPackSize").Value = txtOvrPackSize.Text
                            .Item("@chrPalletCode").Value = ddlPalletCode.SelectedValue
                            .Item("@chrPkgLabelDateFmtCode").Value = ddlPkgDateFmtCode.SelectedValue
                            .Item("@chrPrintCaseLabel").Value = rblPrintCaseLabel.SelectedValue
                            .Item("@chrPrintSOLot").Value = rblPrtSOLot.SelectedValue
                            .Item("@vchProductionDateDesc").Value = txtProductionDateDesc.Text
                            .Item("@bitSlipSheet").Value = rblSlipSheet.SelectedValue
                            .Item("@chrUseSCCAsUPC").Value = rblUseSCCasUPC.SelectedValue
                            .Item("@vchLastUpdatedBy").Value = User.Identity.Name
                            'WO#6437 ADD Start
                            .Item("@vchProductionDateDescOnBox").Value = txtProductionDateDescOnBox.Text
                            .Item("@vchExpiryDateDescOnBox").Value = txtExpiryDateDescOnBox.Text
                            .Item("@vchAdditionalText1").Value = txtAdditionalText1.Text
                            .Item("@vchAdditionalText2").Value = txtAdditionalText2.Text
                            .Item("@vchPalletLabelFmt").Value = ""
                            'WO#6437 ADD Stop
                            ' WO#17432 ADD Start – AT 11/27/2018
                            .Item("@bitInsertBrewerFilter").Value = rblInsertBrewerFilter.SelectedValue
                            .Item("@intCaseLabelApplicator").Value = ddlCaseLabelApplicator.SelectedValue
                            ' WO#17432 ADD Stop – AT 11/27/2018
                        End With

                        daOvr.Connection.Open()
                        If strAction = "ADD" Then
                            .InsertCommand = CType(cmd, SqlClient.SqlCommand)
                            .InsertCommand.ExecuteNonQuery()
                        ElseIf strAction = "UPDATE" Then
                            .UpdateCommand = CType(cmd, SqlClient.SqlCommand)
                            .UpdateCommand.ExecuteNonQuery()
                        ElseIf strAction = "DELETE" Then
                            .DeleteCommand = CType(cmd, SqlClient.SqlCommand)
                            .DeleteCommand.ExecuteNonQuery()
                        End If

                    End With

                    ClearInputOutputFields()
                    Panel1.Visible = False
                End If
            End If
        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        Finally
            If daOvr.Connection.State = ConnectionState.Open Then
                daOvr.Connection.Close()
            End If
        End Try
    End Sub

    Private Function ValidateInputData() As String
        Dim strErrMsg As String = ""
        Try
            If txtItemNumber.Text = "" Then
                strErrMsg = "Item Number is mandatory."
            ElseIf rblOvrItemDesc1.SelectedValue = False AndAlso txtOvrDesc1.Text <> "" Then
                strErrMsg = "Overrided Description 1 must be blank, if the related override option is NO."
            ElseIf rblOvrPackSize.SelectedValue = False AndAlso txtOvrPackSize.Text <> "" Then
                strErrMsg = "Overrided Pack Size must be blank, if the relaced override option is NO."
            ElseIf txtOvrNetWeight.Text <> "" AndAlso Not IsNumeric(txtOvrNetWeight.Text) Then
                strErrMsg = "Overrided Net Weight must be Numeric."
            ElseIf rblOvrNetWgt.SelectedValue = False AndAlso txtOvrNetWeight.Text <> "" AndAlso CType(txtOvrNetWeight.Text, Single) <> 0.0 Then
                strErrMsg = "Overrided Net Weight must be blank, if the relaced override option is NO."
            ElseIf rblOvrNetWgtUOM.SelectedValue = False AndAlso ddlOvrNetWgtUOM.SelectedValue <> "" Then
                strErrMsg = "Overrided Net Weight UOM must be none, if the relaced override option is NO."
            ElseIf txtCaseLabelFmt1.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtCaseLabelFmt1.Text) = False Then
                strErrMsg = "File in Case Label Format 1 cannot be found in Colos repository."
            ElseIf txtCaseLabelFmt2.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtCaseLabelFmt2.Text) = False Then
                strErrMsg = "File in Case Label Format 2 cannot be found in Colos repository."
            ElseIf txtCaseLabelFmt3.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtCaseLabelFmt3.Text) = False Then
                strErrMsg = "File in Case Label Format 3 cannot be found in Colos repository."
            ElseIf txtPackageCoderFmt1.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtPackageCoderFmt1.Text) = False Then
                strErrMsg = "File in Package Coder Format 1 cannot be found in Colos repository."
            ElseIf txtPackageCoderFmt2.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtPackageCoderFmt2.Text) = False Then
                strErrMsg = "File in Package Coder Format 2 cannot be found in Colos repository."
            ElseIf txtPackageCoderFmt3.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtPackageCoderFmt3.Text) = False Then
                strErrMsg = "File in Package Coder Format 3 cannot be found in Colos repository."
            ElseIf txtFilterCoderFormat.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtFilterCoderFormat.Text) = False Then
                strErrMsg = "File in Filter Coder Format cannot be found in Colos repository."
            ElseIf rblPrintCaseLabel.SelectedValue = "Y" AndAlso (txtCaseLabelFmt1.Text & txtCaseLabelFmt2.Text & txtCaseLabelFmt3.Text).ToString.Trim = "" Then
                strErrMsg = "Print Case Label is selected but no Case Label Formats are entered."
                'WO#6437 ADD Start
                'WO#6437    ElseIf txtPalletLabelFmt.Text <> "" AndAlso SharedUtilities.IsFileExisted(txtPalletLabelFmt.Text) = False Then
                'WO#6437    strErrMsg = "File in Pallet Label Format cannot be found in Colos repository."
                'WO#6437 ADD Stop
            ElseIf txtBagLength.Text <> "" AndAlso Not IsNumeric(txtBagLength.Text) Then
                strErrMsg = "Bag Length must be numeric."
            ElseIf rblBagLengthRequired.SelectedValue = "Y" AndAlso txtBagLength.Text = "" Then
                strErrMsg = "Bag Length must be greater then zero, if Bag Length is required."
            ElseIf rblBagLengthRequired.SelectedValue = "Y" AndAlso CSng(txtBagLength.Text) <= 0 Then
                strErrMsg = "Bag Length must be greater then zero, if Bag Length is required."
            ElseIf rblBagLengthRequired.SelectedValue = "N" AndAlso CSng(txtBagLength.Text) <> 0 Then
                strErrMsg = "Bag Length must be zero, if Bag Length is not required."
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return strErrMsg
    End Function

    Private Function LoadOvrrData(ByVal strAction As String, ByVal strFacility As String, ByVal strItemNumber As String) As Boolean
        Dim daOvr As New dsItemFromOvrTableAdapters.PPsp_ItemLabelOverride_SelTableAdapter
        Dim dtOvr As New dsItemFromOvr.PPsp_ItemLabelOverride_SelDataTable
        Dim drOvr As dsItemFromOvr.PPsp_ItemLabelOverride_SelRow

        daOvr.Fill(dtOvr, Nothing, strFacility, strItemNumber)
        If dtOvr.Rows.Count > 0 Then
            If strAction <> "ADD" Then
                drOvr = dtOvr.Rows(0)
                With drOvr
                    ddlFacilityDtl.SelectedValue = .Facility

                    ' Item descriptions
                    If .ItemDesc1 <> "" Then
                        rblOvrItemDesc1.SelectedValue = True
                    Else
                        rblOvrItemDesc1.SelectedValue = False
                    End If
                    txtOvrDesc1.Text = .ItemDesc1
                    txtOvrDesc2.Text = .ItemDesc2
                    txtOvrDesc3.Text = .ItemDesc3

                    'Pack size
                    If .PackSize = "" Then
                        rblOvrPackSize.SelectedValue = False
                    Else
                        rblOvrPackSize.SelectedValue = True
                    End If
                    txtOvrPackSize.Text = .PackSize

                    'Net weight
                    If .NetWeight <> "" Then
                        rblOvrNetWgt.SelectedValue = True
                    Else
                        rblOvrNetWgt.SelectedValue = False
                    End If
                    txtOvrNetWeight.Text = .NetWeight

                    If .NetWeightUOM.Trim <> "" Then
                        rblOvrNetWgtUOM.SelectedValue = True
                    Else
                        rblOvrNetWgtUOM.SelectedValue = False
                    End If
                    ddlOvrNetWgtUOM.SelectedValue = .NetWeightUOM.Trim

                    ' UPC
                    rblUseSCCasUPC.SelectedValue = .UseSCCAsUPC

                    If .UseSCCAsUPC = "Y" Then
                        lblOvrUPC.Text = lblSCC.Text
                    Else
                        lblOvrUPC.Text = lblUPC.Text
                    End If

                    'Date code formats
                    ddlCaseDateFmtCode.DataBind()       'ADD 2019/04/01
                    ddlCaseDateFmtCode.SelectedValue = .CaseLabelDateFmtCode

                    'ADD Start 2019/04/01
                    ddlPkgDateFmtCode.DataBind()    'ADD 2019/04/01
                    ddlPkgDateFmtCode.SelectedValue = .PkgLabelDateFmtCode

                    'Bag length
                    rblBagLengthRequired.SelectedValue = .BagLengthRequired
                    txtBagLength.Text = .BagLength

                    'Override item
                    txtOvrItem.Text = .OverrideItem

                    'Domicile1 texts
                    txtDomicile1.Text = .DomicileText1
                    txtDomicile2.Text = .DomicileText2
                    txtDomicile3.Text = .DomicileText3
                    txtDomicile4.Text = .DomicileText4
                    txtDomicile5.Text = .DomicileText5
                    txtDomicile6.Text = .DomicileText6

                    'Print shop order lot
                    rblPrtSOLot.SelectedValue = .PrintSOLot
                    'Print Case label
                    rblPrintCaseLabel.SelectedValue = .PrintCaseLabel
                    'What kind of dates to print on the label
                    ddlDateToPrtOnLabel.SelectedValue = .DateToPrintFlag
                    'Production date description
                    txtProductionDateDesc.Text = .ProductionDateDesc
                    'Expiry date description
                    txtExpiryDateDesc.Text = .ExpiryDateDesc

                    'Label formats'
                    txtCaseLabelFmt1.Text = .CaseLabelFmt1
                    txtCaseLabelFmt2.Text = .CaseLabelFmt2
                    txtCaseLabelFmt3.Text = .CaseLabelFmt3
                    txtPackageCoderFmt1.Text = .PackageCoderFmt1
                    txtPackageCoderFmt2.Text = .PackageCoderFmt2
                    txtPackageCoderFmt3.Text = .PackageCoderFmt3
                    txtFilterCoderFormat.Text = .FilterCoderFmt
                    'WO#6437 ADD Start
                    txtProductionDateDescOnBox.Text = .ProductionDateDescOnBox
                    txtExpiryDateDescOnBox.Text = .ExpiryDateDescOnBox
                    txtAdditionalText1.Text = .AdditionalText1
                    txtAdditionalText2.Text = .AdditionalText2
                    'txtPalletLabelFmt.Text = .PalletLabelFmt
                    'WO#6437 ADD Stop

                    'Slip sheet
                    rblSlipSheet.SelectedValue = .SlipSheet
                    'Pallet Code
                    ddlPalletCode.SelectedValue = Trim(.PalletCode)

                    ' WO#17432 ADD Start – AT 11/27/2018
                    ddlCaseLabelApplicator.SelectedValue = .CaseLabelApplicator
                    rblInsertBrewerFilter.SelectedValue = .InsertBrewerFilter
                    ' WO#17432 ADD Stop – AT 11/27/2018

                End With
            End If
            Return True

        Else
            'If strAction = "UPDATE" Or strAction = "DELETE" Then
            '    lblErrMsg.Text = "Item is not found in the Item Label Override Table. Please enter a valid item."
            'End If
            ' WO#17432 ADD Start – BL 2019/02/17
            ddlFacilityDtl.SelectedValue = strFacility

            '' Item descriptions
            'rblOvrItemDesc1.SelectedValue = False
            'txtOvrDesc1.Text = String.Empty
            'txtOvrDesc2.Text = String.Empty
            'txtOvrDesc3.Text = String.Empty

            ''Packsize
            'rblOvrPackSize.SelectedValue = False

            ''Net weight
            'txtOvrNetWeight.Text = String.Empty
            'rblOvrNetWgt.SelectedValue = False
            'rblOvrNetWgtUOM.SelectedValue = False
            'ddlOvrNetWgtUOM.SelectedValue = ""

            '' UPC
            'rblUseSCCasUPC.SelectedValue = "N"
            'lblOvrUPC.Text = String.Empty

            ''Bag Length
            'rblBagLengthRequired.SelectedValue = "N"
            'txtBagLength.Text = String.Empty

            ''Override item
            'txtOvrItem.Text = String.Empty

            ''Domicile1 texts
            'txtDomicile1.Text = String.Empty
            'txtDomicile2.Text = String.Empty
            'txtDomicile3.Text = String.Empty
            'txtDomicile4.Text = String.Empty
            'txtDomicile5.Text = String.Empty
            'txtDomicile6.Text = String.Empty

            ''Print shop order lot
            'rblPrtSOLot.SelectedValue = "N"

            ''Print Case label
            'rblPrintCaseLabel.SelectedValue = "Y"

            ''What kind of dates to print on the label
            'ddlDateToPrtOnLabel.SelectedValue = "0"

            ''Production date description
            'txtProductionDateDesc.Text = String.Empty

            ''Expiry date description
            'txtExpiryDateDesc.Text = String.Empty

            ''Label formats'
            'txtCaseLabelFmt1.Text = String.Empty
            'txtCaseLabelFmt2.Text = String.Empty
            'txtCaseLabelFmt3.Text = String.Empty
            'txtPackageCoderFmt1.Text = String.Empty
            'txtPackageCoderFmt2.Text = String.Empty
            'txtPackageCoderFmt3.Text = String.Empty
            'txtFilterCoderFormat.Text = String.Empty

            'txtProductionDateDescOnBox.Text = String.Empty
            'txtExpiryDateDescOnBox.Text = String.Empty
            'txtAdditionalText1.Text = String.Empty
            'txtAdditionalText2.Text = String.Empty
            ''txtPalletLabelFmt.Text = .PalletLabelFmt

            ''Slip sheet
            'rblSlipSheet.SelectedValue = False
            ''Pallet Code
            'ddlPalletCode.SelectedValue = String.Empty

            ''Case Label Applicator
            'ddlCaseLabelApplicator.SelectedValue = 0

            ''Insert Brewer Filter
            'rblInsertBrewerFilter.SelectedValue = False
            '' WO#17432 ADD Stop –  BL 2019/02/17

            Return False

        End If

    End Function

    Private Function LoadERPData(ByVal strFacility As String, ByVal strItemNumber As String) As Boolean
        Dim daERP As New dsItemFromERPTableAdapters.PPsp_ItemMasterFrmERP_SelTableAdapter
        Dim dtERP As New dsItemFromERP.PPsp_ItemMasterFrmERP_SelDataTable
        Dim drERP As dsItemFromERP.PPsp_ItemMasterFrmERP_SelRow
        daERP.Fill(dtERP, Nothing, strFacility, strItemNumber)
        If dtERP.Rows.Count > 0 Then
            drERP = dtERP.Rows(0)
            With drERP
                lblDesc1.Text = .ItemDesc1
                lblFacility.Text = .Facility
                lblItemMajorClass.Text = .ItemMajorClass
                lblItemType.Text = .ItemType
                lblLabelWeight.Text = .LabelWeight & " " & .LabelWeightUOM
                lblNetWeight.Text = .NetWeight
                'lblOvrUPC.Text = .SCCCode
                lblPackSize.Text = .PackSize
                lblPkgPerSaleableUnit.Text = .PackagesPerSaleableUnit
                lblProdShelfLifeDays.Text = .ProductionShelfLifeDays & " days"
                If .QtyPerPallet <> 0 Then
                    lblQtyPerPallet.Text = .QtyPerPallet
                Else
                    lblQtyPerPallet.Text = .Tie * .Tier
                End If
                lblSaleableUnitPerCase.Text = .SaleableUnitPerCase
                lblSCC.Text = .SCCCode
                lblShipShelfLifeDays.Text = .ShipShelfLifeDays & " days"
                lblTieXTier.Text = .Tie & " x " & .Tier
                lblUPC.Text = .UPCCode
                lblItemNumber.Text = .ItemNumber
                lblDim.Text = String.Format("{0} x {1} x {2}", Decimal.Round(.GrsDepth, 2), Decimal.Round(.GrsHeight, 2), Decimal.Round(.GrsWidth, 2))
            End With
            Return True
        Else
            lblErrMsg.Text = "Item is not found in the ERP Item Master table."
            Panel1.Visible = False
            Return False
        End If

    End Function

    Public Sub ClearInputOutputFields()
        Dim pnl As Panel
        Dim txtBox As TextBox
        Dim lbl As Label
        ' WO#17432 DEL Start – AT 11/27/2018
        ' Dim rbl As RadioButtonList
        ' WO#17432 DEL Stop – AT 11/27/2018
        Dim ctl As Control


        lblErrMsg.Text = ""

        pnl = Panel1
        If Not IsNothing(pnl) Then
            For Each ctl In pnl.Controls
                If TypeOf ctl Is TextBox Then
                    txtBox = CType(ctl, TextBox)
                    txtBox.Text = ""     'Clear all text on the text boxes
                ElseIf TypeOf ctl Is Label Then
                    lbl = CType(ctl, Label)
                    lbl.Text = ""     'Clear all text on the labels
                    '               ElseIf TypeOf ctl Is RadioButtonList Then
                    '                  rbl = CType(ctl, RadioButtonList)
                    'rbl.SelectedValue = False   'Set default to false to radio button lists
                    '                 rbl.SelectedIndex = 0   'Set default to false to radio button lists
                End If
            Next ctl

            'WO#17432 ADD Start – BL 2019/02/18
            For Each ctl In View1.Controls
                If TypeOf ctl Is TextBox Then
                    txtBox = CType(ctl, TextBox)
                    txtBox.Text = ""     'Clear all text on the text boxes
                End If
            Next ctl

            For Each ctl In View2.Controls
                If TypeOf ctl Is TextBox Then
                    txtBox = CType(ctl, TextBox)
                    txtBox.Text = ""     'Clear all text on the text boxes
                End If
            Next ctl


            For Each ctl In View3.Controls
                If TypeOf ctl Is TextBox Then
                    txtBox = CType(ctl, TextBox)
                    txtBox.Text = ""     'Clear all text on the text boxes
                End If
            Next ctl

            For Each ctl In View4.Controls
                If TypeOf ctl Is TextBox Then
                    txtBox = CType(ctl, TextBox)
                    txtBox.Text = ""     'Clear all text on the text boxes
                End If
            Next ctl

            'Item descriptions
            rblOvrItemDesc1.SelectedValue = False

            'Packsize
            rblOvrPackSize.SelectedValue = False

            'Net weight
            rblOvrNetWgt.SelectedValue = False
            rblOvrNetWgtUOM.SelectedValue = False
            ddlOvrNetWgtUOM.SelectedValue = ""

            'UPC
            rblUseSCCasUPC.SelectedValue = "N"

            'Bag Length
            rblBagLengthRequired.SelectedValue = "N"

            'Print shop order lot
            rblPrtSOLot.SelectedValue = "N"

            'Print Case label
            rblPrintCaseLabel.SelectedValue = "Y"

            'What kind of dates to print on the label
            ddlDateToPrtOnLabel.SelectedValue = "0"

            'Slip sheet
            rblSlipSheet.SelectedValue = False

            'Case Label Applicator
            ddlCaseLabelApplicator.SelectedValue = 0

            'Insert Brewer Filter
            rblInsertBrewerFilter.SelectedValue = False
            'WO#17432 ADD Stop – BL 2019/02/18
        End If
        txtBagLength.Text = "0.00"
        'ddlCaseDateFmtCode.SelectedIndex = 0
        'ddlPkgDateFmtCode.SelectedIndex = 0
        ddlCaseDateFmtCode.SelectedValue = "  "
        ddlPkgDateFmtCode.SelectedValue = "  "

        '' WO#17432 ADD Start – AT 11/27/2018
        'ddlCaseLabelApplicator.SelectedValue = "0"
        '' WO#17432 ADD Stop – AT 11/27/2018
    End Sub

    Private Sub LoadDataWithoutKnowingAction()
        Panel1.Visible = False
        lblAction.Text = "Mode: UNKNOWN"
        ClearInputOutputFields()
        If LoadERPData(ddlFacility.SelectedValue, txtItemNumber.Text) = True Then
            If LoadOvrrData("UPDATE", ddlFacility.SelectedValue, txtItemNumber.Text) = True Then
                lblAction.Text = "Mode: UPDATE"
            Else
                lblAction.Text = "Mode: ADD"
            End If
            Panel1.Visible = True
        Else
            lblErrMsg.Text = "Item is not found in the ERP Item Master table."
        End If
    End Sub

    Private Sub btnDefault_Click(sender As Object, e As EventArgs) Handles btnDefault.Click
        lblErrMsg.Text = "Please click on one of the action buttons to contnue."
    End Sub

    ''Protected Sub IsFmtExist(source As Object, args As ServerValidateEventArgs) Handles cvCaseLabelFmt1.ServerValidate, cvCaseLabelFmt2.ServerValidate, cvCaseLabelFmt3.ServerValidate, cvPackageCoderFmt1.ServerValidate, cvPackageCoderFmt2.ServerValidate, cvPackageCoderFmt3.ServerValidate, cvFilterCoderFormat.ServerValidate
    ''    'args.IsValid = SharedUtilities.IsFileExisted(txtCaseLabelFmt2.Text)
    ''    args.IsValid = SharedUtilities.IsFileExisted(args.Value)
    ''End Sub

    Private Sub Page_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        If (Not Page.IsPostBack) Then
            Panel1.Visible = False
        End If
    End Sub

    Protected Sub txtCaseLabelFmt3_TextChanged(sender As Object, e As EventArgs) Handles txtCaseLabelFmt3.TextChanged

    End Sub
    ' WO#17432 ADD Start – AT 11/27/2018
    Protected Sub Tab1_Click(sender As Object, e As EventArgs)
        Tab1.CssClass = "Clicked"
        Tab2.CssClass = "Initial"
        Tab3.CssClass = "Initial"
        Tab4.CssClass = "Initial"
        MainView.ActiveViewIndex = 0
    End Sub

    Protected Sub Tab2_Click(sender As Object, e As EventArgs)
        Tab1.CssClass = "Initial"
        Tab2.CssClass = "Clicked"
        Tab3.CssClass = "Initial"
        Tab4.CssClass = "Initial"
        MainView.ActiveViewIndex = 1
    End Sub

    Protected Sub Tab3_Click(sender As Object, e As EventArgs)
        Tab1.CssClass = "Initial"
        Tab2.CssClass = "Initial"
        Tab3.CssClass = "Clicked"
        Tab4.CssClass = "Initial"
        MainView.ActiveViewIndex = 2
    End Sub

    Protected Sub Tab4_Click(sender As Object, e As EventArgs)
        Tab1.CssClass = "Initial"
        Tab2.CssClass = "Initial"
        Tab3.CssClass = "Initial"
        Tab4.CssClass = "Clicked"
        MainView.ActiveViewIndex = 3
    End Sub

    Protected Sub ddlOvrNetWgtUOM_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
    ' WO#17432 ADD Stop– AT 11/27/2018
End Class