<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ItemLabelOverride._Default" Theme="DefaultTheme" StylesheetTheme="DefaultTheme" Title="Item Label Override" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Item Label Override</title>
    <link rel="stylesheet" type="text/css" href="default.css" />
    
    </head>

<%--<script type="text/javascript">

    function IsFmtFileExist(oSrc, args) {
        var myObject;
        args.isvalid = false;

        try {
            myObject = AJ();

            if (myObject == null || myObject == undefined) {
                args.isvalid = false;
                return false;
            }
            else {
                myObject.open("HEAD", args.Value, false);
                myObject.send(null);
                args.isvalid = (myObject.status == 200) ? true : false;
                return args.isvalid;
            }
        }
        catch (er) {
            args.isvalid = false;
            return false;
        }
    }

    function AJ() {
        var obj;
        if (window.XMLHttpRequest) {
            obj = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            try {
                obj = new ActiveXObject('MSXML2.XMLHTTP.3.0');
            }
            catch (er) {
                // alert("er");
                obj = false;
            }
        }
        //alert(obj);
        return obj;
    }
</script>--%>

<body>
    <form id="form1" runat="server" defaultbutton="btnDefault" defaultfocus="txtItemNumber">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

           <div>

            <p align="center" style="font-size: large">Item Label Override Maintenance</p>
            <!-- Prompt for user -->
            <div>

                <table style="width: 100%; margin-bottom: 20px;">
                    <tr>
                        <td class="Prompt-Facility" >Facility:</td>
                        <td class="auto-style3">
                            <asp:DropDownList ID="ddlFacility" runat="server" DataSourceID="dsFacility" DataTextField="Description" DataValueField="Facility" >
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsFacility" runat="server" ConnectionString="<%$ ConnectionStrings:CnnStrPowerPlant %>" SelectCommand="PPsp_Facility_Sel" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="SelByRegion" Name="vchAction" Type="String" />
                                    <asp:Parameter DefaultValue="Desc" Name="vchOrderBy" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td class="Prompt-ItemNumber" align="right">Item Number:</td>
                        <td class="FieldDesc_Column">
                            <asp:TextBox ID="txtItemNumber" runat="server" CssClass="txtBox_25" MaxLength="35"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvItemNumber" runat="server" ErrorMessage="Item Number is mandatory." ControlToValidate="txtItemNumber" Display="none" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vcItemNumber" runat="server" TargetControlID="rfvItemNumber" PopupPosition="BottomRight"></asp:ValidatorCalloutExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" />&nbsp;
                            <asp:Button ID="btnChange" runat="server" Text="Change" />&nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                            <asp:Button ID="btnDefault" runat="server" Text="default" style="display:none;" />
                            
                        </td>
                        <td class="auto-style3">
                            <asp:Button ID="btnAccept" runat="server" Text="Accept" BackColor="#00CC00" />
                        </td>
                    </tr>

                    </table>

            
            </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnChange" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:Label ID="lblErrMsg" runat="server" Text="Label" CssClass="Err_Message"></asp:Label>
                <asp:Panel ID="Panel1" runat="server" Height="1200px">
                    <table style="width: 100%;margin-bottom: 20px;">
            <tr>
                <td ><table style=" width: 100%;" frame="border">
                    <tr>
                        <td><asp:Label ID="lblAction" runat="server" CssClass="ActionMode"></asp:Label></td>
                        <td colspan="2" class="ColumnTitle">Item Label Override from Power Plant</td>
                        <td class="ColumnTitle">Data Originally from ERP</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="Override_YesOrNO_heading">--- Override ---</td>
                        <td class="Override_DataEntryHeading"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Facility:</td>
                        <td class="Override_YesOrNo"></td>
                        <td class="Override_DataEntry">
                            <asp:DropDownList ID="ddlFacilityDtl" runat="server" DataSourceID="dsFacility" DataTextField="Description" DataValueField="Facility">
                            </asp:DropDownList>
                        </td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblFacility" runat="server" Text="Facility"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Desc. 1:</td>
                        <td class="Override_YesOrNo">
                            
                            <asp:RadioButtonList ID="rblOvrItemDesc1" runat="server" Height="24px" RepeatDirection="Horizontal" Width="100px">
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtOvrDesc1" runat="server" CssClass="txtBox_50" MaxLength="50"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblDesc1" runat="server" Text="Desc. 1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Desc. 2:</td>
                        <td class="Override_YesOrNo">
                            </td>
                        <td class="Override_DataEntry"><asp:TextBox ID="txtOvrDesc2" runat="server" CssClass="txtBox_50" MaxLength="50"></asp:TextBox></td>
                        <td class="ERP_Column"></td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Desc. 3</td>
                        <td class="Override_YesOrNo">
                            &nbsp;</td>
                        <td class="Override_DataEntry"><asp:TextBox ID="txtOvrDesc3" runat="server" CssClass="txtBox_50" MaxLength="50"></asp:TextBox></td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Pack Size:</td>
                        <td class="Override_YesOrNo">
                            <asp:RadioButtonList ID="rblOvrPackSize" runat="server"  RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="Override_DataEntry"><asp:TextBox ID="txtOvrPackSize" runat="server" MaxLength="12"></asp:TextBox></td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblPackSize" runat="server" Text="P Size"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Net Weight:</td>
                        <td class="Override_YesOrNo">
                            <asp:RadioButtonList ID="rblOvrNetWgt" runat="server" Height="24px" RepeatDirection="Horizontal" Width="100px">
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtOvrNetWeight" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RangeValidator ID="rvOvrNetWeight" runat="server" ControlToValidate="txtOvrNetWeight" Display="none" ErrorMessage="Net Weight must be numeric." MaximumValue="9999999999" MinimumValue="0" SetFocusOnError="True" Type="Double"></asp:RangeValidator>
                            <asp:ValidatorCalloutExtender ID="vceOvrNetWeight" runat="server" TargetControlID="rvOvrNetWeight"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblNetWeight" runat="server" Text="Net Weight"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Net Weight UOM:</td>
                        <td class="Override_YesOrNo">
                            <asp:RadioButtonList ID="rblOvrNetWgtUOM" runat="server" Height="24px" RepeatDirection="Horizontal" Width="100px">
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="Override_DataEntry">
                            <asp:DropDownList ID="ddlOvrNetWgtUOM" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="">None</asp:ListItem>
                                <asp:ListItem>GM</asp:ListItem>
                                <asp:ListItem>KG</asp:ListItem>
                                <asp:ListItem>LB</asp:ListItem>
                                <asp:ListItem>OZ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="FieldDesc_Column">UPC:</td>
                        <td class="Override_YesOrNo">
                            <asp:RadioButtonList ID="rblUseSCCasUPC" runat="server" Height="24px" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="Override_DataEntry">
                            <asp:Label ID="lblOvrUPC" runat="server" Text="Use SCC as UPC"></asp:Label>
                        </td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblUPC" runat="server" Text="UPC"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">SCC:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblSCC" runat="server" Text="SCC"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Pkg Coder Date Fmt:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:DropDownList ID="ddlPkgDateFmtCode" runat="server" DataSourceID="dsCaseDateFmtCode" DataTextField="FmtDescription" DataValueField="DateCode" >
                            </asp:DropDownList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Case Label Date Fmt:</td>
                        <td class="Override_YesOrNo"></td>
                        <td class="Override_DataEntry">
                            <asp:DropDownList ID="ddlCaseDateFmtCode" runat="server" DataSourceID="dsCaseDateFmtCode" DataTextField="FmtDescription" DataValueField="DateCode">
                            </asp:DropDownList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="FieldDesc_Column">Bag Length Required:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:RadioButtonList ID="rblBagLengthRequired" runat="server" Height="15px" RepeatDirection="Horizontal" Width="95px" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Bag Length:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry"><asp:TextBox ID="txtBagLength" runat="server"></asp:TextBox>
                            <asp:MaskedEditExtender ID="meeBagLength" runat="server" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99.99" MaskType="Number" TargetControlID="txtBagLength">
                            </asp:MaskedEditExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Override Item:</td>
                        <td class="Override_YesOrNo"></td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtOvrItem" runat="server" MaxLength="35" CssClass="txtBox_50"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Print Shop Order Lot:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:RadioButtonList ID="rblPrtSOLot" runat="server" Height="15px" RepeatDirection="Horizontal" Width="95px" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                        <tr>
                            <td class="FieldDesc_Column">Print Case Label:</td>
                            <td class="Override_YesOrNo">&nbsp;</td>
                            <td class="Override_DataEntry">
                                <asp:RadioButtonList ID="rblPrintCaseLabel" runat="server" Height="15px" RepeatDirection="Horizontal" Width="95px" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td class="ERP_Column">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="FieldDesc_Column">Date to Print on Label:</td>
                            <td class="Override_YesOrNo">&nbsp;</td>
                            <td class="Override_DataEntry">
                                <asp:DropDownList ID="ddlDateToPrtOnLabel" runat="server">
                                    <asp:ListItem Value="0">None</asp:ListItem>
                                    <asp:ListItem Value="1">Expiry</asp:ListItem>
                                    <asp:ListItem Value="2">Production</asp:ListItem>
                                    <asp:ListItem Value="3">Both</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="ERP_Column">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="FieldDesc_Column">Production Date Desc. </td>
                            <td class="Override_YesOrNo"></td>
                            <td class="Override_DataEntry">
                                <asp:TextBox ID="txtProductionDateDesc" runat="server" MaxLength="25"></asp:TextBox>
                            </td>
                            <td class="ERP_Column">&nbsp;</td>
                        </tr>
                    <tr>
                        <td class="FieldDesc_Column" height="20">Expiry Date Desc.:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtExpiryDateDesc" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <!-- WO#6430 ADD Start -->
                    <tr>
                        <td class="FieldDesc_Column">*Production Date Desc. on Box</td>
                        <td class="Override_YesOrNo"></td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtProductionDateDescOnBox" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column" height="20">*Expiry Date Desc. on Box:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtExpiryDateDescOnBox" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column" height="20">*Additional Text 1:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtAdditionalText1" runat="server" CssClass="txtBox_30" MaxLength="30">123456789012345678901234567890</asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column" height="20">*Additional Text 2:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtAdditionalText2" runat="server" CssClass="txtBox_30" MaxLength="30"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <!-- WO#6430 ADD Stop -->
                    <tr>
                        <td class="FieldDesc_Column" height="20">Domicile Text 1:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtDomicile1" runat="server" CssClass="txtBox_25" MaxLength="24"></asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Domicile Text 2:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtDomicile2" runat="server" CssClass="txtBox_25" MaxLength="24">123456789012345678901234</asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Domicile Text 3:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtDomicile3" runat="server" CssClass="txtBox_25" MaxLength="24">123456789012345678901234</asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Domicile Text 4:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtDomicile4" runat="server" CssClass="txtBox_25" MaxLength="24">123456789012345678901234</asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Domicile Text 5:</td>
                        <td class="Override_YesOrNo"></td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtDomicile5" runat="server" CssClass="txtBox_25" MaxLength="24">123456789012345678901234</asp:TextBox>
                        </td>
                        <td class="Override_DataEntryHeading"></td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Domicile Text 6:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtDomicile6" runat="server" CssClass="txtBox_25" MaxLength="24">123456789012345678901234</asp:TextBox>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Case Label Format 1:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtCaseLabelFmt1" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvCaseLabelFmt1" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtCaseLabelFmt1" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vceCaseLabelFmt1" runat="server" TargetControlID="cvCaseLabelFmt1"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Case Label Format 2:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtCaseLabelFmt2" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvCaseLabelFmt2" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtCaseLabelFmt2" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vceCaseLabelFmt2" runat="server" TargetControlID="cvCaseLabelFmt2"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Case Label Format 3:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtCaseLabelFmt3" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvCaseLabelFmt3" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtCaseLabelFmt3" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vceCaseLabelFmt3" runat="server" TargetControlID="cvCaseLabelFmt3"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column" height="20">Package Coder Format 1:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtPackageCoderFmt1" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvPackageCoderFmt1" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtPackageCoderFmt1" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vcePackageCoderFmt1" runat="server" TargetControlID="cvPackageCoderFmt1"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Package Coder Format 2:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtPackageCoderFmt2" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvPackageCoderFmt2" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtPackageCoderFmt2" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vcePackageCoderFmt2" runat="server" TargetControlID="cvPackageCoderFmt2"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Package Coder Format 3:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtPackageCoderFmt3" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvPackageCoderFmt3" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtPackageCoderFmt1" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vcePackageCoderFmt3" runat="server" TargetControlID="cvPackageCoderFmt3"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Filter Coder Format:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:TextBox ID="txtFilterCoderFormat" runat="server" CssClass="txtBox_25" MaxLength="25">1234567890123456789012345</asp:TextBox>
                            <asp:CustomValidator ID="cvFilterCoderFormat" runat="server" ErrorMessage="File Does not exist." ControlToValidate="txtFilterCoderFormat" Display="None" SetFocusOnError="True"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="vceFilterCoderFormat" runat="server" TargetControlID="cvFilterCoderFormat"></asp:ValidatorCalloutExtender>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="FieldDesc_Column">Slip Sheet: </td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:RadioButtonList ID="rblSlipSheet" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="FieldDesc_Column">Pallet Code:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">
                            <asp:DropDownList ID="ddlPalletCode" runat="server">
                                <asp:ListItem Value="" Selected="True">None</asp:ListItem>
                                <asp:ListItem Value="A">A</asp:ListItem>
                                <asp:ListItem Value="B">B</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="ERP_Column">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="FieldDesc_Column">Item Major Class:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblItemMajorClass" runat="server" Text="Item Major Class"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Item Type:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblItemType" runat="server" Text="ItemType"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Label Weight:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblLabelWeight" runat="server" Text="Label Weight"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Saleable Unit Per Case:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblSaleableUnitPerCase" runat="server" Text="Saleable Unit Per Case"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Pkg Per Saleable Unit:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblPkgPerSaleableUnit" runat="server" Text="Packages Per Saleable Unit"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Prod. Shelf Life Days:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblProdShelfLifeDays" runat="server" Text="Production Shelf Life Days"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Ship Shelf Life Days:</td>
                        <td class="Override_YesOrNo"></td>
                        <td class="Override_DataEntry"></td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblShipShelfLifeDays" runat="server" Text="Ship Shelf Life Days"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Qty Per Pallet:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblQtyPerPallet" runat="server" Text="Qty/Pallet"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FieldDesc_Column">Tie x Tier:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblTieXTier" runat="server" Text="Tie x Tier"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="FieldDesc_Column">Item Number:</td>
                        <td class="Override_YesOrNo">&nbsp;</td>
                        <td class="Override_DataEntry">&nbsp;</td>
                        <td class="ERP_Column">
                            <asp:Label ID="lblItemNumber" runat="server" Text="Item Number"></asp:Label>
                        </td>
                    </tr>
                    
                </table>
                </td>
                
            </tr>

        </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        </div>

        <asp:SqlDataSource ID="dsCaseDateFmtCode" runat="server" ConnectionString="<%$ ConnectionStrings:CnnStrPowerPlant %>" SelectCommand="PPsp_DateFormatCode_Sel" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
            <SelectParameters>
                <asp:Parameter DefaultValue="LookUp" Name="vchAction" Type="String" />
                <asp:ControlParameter ControlID="ddlFacilityDtl" DefaultValue="" Name="vchFacility" PropertyName="SelectedValue" Type="String" />
                <asp:Parameter DefaultValue="True" Name="bitActive" Type="Boolean" />
            </SelectParameters>
        </asp:SqlDataSource>

    </form>
</body>
</html>
