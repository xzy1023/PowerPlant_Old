<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Package_Line_Printer_Maintenance._Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Package Line Printer Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                   <asp:UpdatePanel ID="UpdatePanel0" runat="server">
                <ContentTemplate>
        <div id="divHeader" class="DivLeft">
            <div id="divLogo">
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                        <td>
                            <h1>Printer Maintenance (User:
                                <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1>
                        </td>
                    </tr>
                </table>
            </div>

 
                    <div id="divSearch">
                        <table>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblFacility2" runat="server" Text="Facility: " CssClass="Labels"></asp:Label></td>

                                <td class="Labels">
                                    <asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="True" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" Width="200px" ></asp:DropDownList>

                                </td>
                                <td class="Labels">
                                    <asp:Label ID="lblFacility1" runat="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false"></asp:Label></td>

                                <td class="Labels">
                                    <asp:Label ID="SearchLabel" Text="Packaging Line: " runat="Server" CssClass="Labels" /></td>
                                <td class="Labels">
                                    <asp:DropDownList ID="ddlPackagingLineSearch" runat="server" Visible="true" Width="400px" OnSelectedIndexChanged="ddlPackagingLineSearch_SelectedIndexChanged" AutoPostBack="False"></asp:DropDownList></td>
                                <td class="Labels">
                                    <asp:TextBox ID="txtSearch" runat="server" Font-Size="Small" Visible="false"></asp:TextBox></td>
                                <td style="width: 100px; text-align: left; vertical-align: middle;">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/App_Themes/PP/Images/go.png" /></td>
                            </tr>
                        </table>
                    </div>
                  </div>
                </ContentTemplate>
            </asp:UpdatePanel>
  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ibtnSearch" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnSave" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                <asp:AsyncPostBackTrigger ControlID="ddlDeviceType"/>
                 <asp:AsyncPostBackTrigger ControlID="ddlFacility" EventName="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <div id="divGrid">
                        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass="GridView" AllowPaging="True" PageSize="25" AllowSorting="True"
                            DataKeyNames="RRN,Facility" Style="margin-top: 12px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                            OnPageIndexChanging="gvForm_PageIndexChanging"
                            OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting"
                            EmptyDataText="-- There are no data records to display in grid. --" HeaderStyle-VerticalAlign="Top">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:ButtonField Text="Edit" CommandName="Select" ItemStyle-Width="40">
                                    <ItemStyle Width="40px"></ItemStyle>
                                </asp:ButtonField>
                                <asp:TemplateField HeaderText="Facility" SortExpression="Facility" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFacility" runat="server" Text='<%# EVAL("Facility") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Packaging Line" SortExpression="PackagingLine" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPackagingLine" runat="server" Text='<%# Eval("PackagingLine")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Width="200">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="200px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Label Type" SortExpression="LabelType" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLabelType" runat="server" Text='<%# Eval("LabelType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Label Sub Type" SortExpression="LabelSubType" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLabelSubType" runat="server" Text='<%# Eval("LabelSubType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Device Name" SortExpression="DeviceName" ItemStyle-Width="230">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeviceName" runat="server" Text='<%# Eval("DeviceName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="230px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IP Address" SortExpression="IPAddress" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIPAddress" runat="server" Text='<%# Eval("IPAddress")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Native Driver" SortExpression="UseNativeDriver" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbUseNativeDriver" runat="server" Checked='<%#Eval("UseNativeDriver")%>' Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="80px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Device Type" SortExpression="DeviceType" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeviceType" runat="server" Text='<%# Eval("DeviceType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Device Sub Type" SortExpression="DeviceSubType" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeviceSubType" runat="server" Text='<%# Eval("DeviceSubType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RRN" SortExpression="RRN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRRN" runat="server" Text='<%# Eval("RRN")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60px"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>

                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        </asp:GridView>
                        &nbsp;
                    </div>
                    <!-- <div id="divEdit" class="DivLeft" style="display: inline-block; float: left"> -->
                    <div id="divEdit" class="DivLeft">
                        <table>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblLine2" runat="server" Text="Packaging Line: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:DropDownList ID="ddlLine" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlLine_SelectedIndexChanged" Width="404px"></asp:DropDownList></td>
                                <td>
                                    <asp:Label ID="lblPackagingLine1" runat="server" Text="" CssClass="Labels" Visible="false"></asp:Label>
                                    <asp:Label ID="lblDescription1" runat="server" Text="" CssClass="Labels" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblDeviceType2" runat="server" Text="Label Type: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:DropDownList ID="ddlDeviceType" runat="server" Visible="true" OnSelectedIndexChanged="ddlDeviceType_SelectedIndexChanged" Width="404px" AutoPostBack="True"></asp:DropDownList></td>
                                <td class="Labels">
                                    <asp:Label ID="lblDeviceType1" runat="server" Text="" CssClass="Labels" Visible="false"></asp:Label>
                                    <asp:Label ID="lblLabelType1" runat="server" Text="" CssClass="Labels" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblDeviceSubType2" runat="server" Text="Label Sub Type: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:DropDownList ID="ddlDeviceSubType" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlDeviceSubType_SelectedIndexChanged" Width="404px"></asp:DropDownList></td>
                                <td class="Labels">
                                    <asp:Label ID="lblDeviceSubType1" runat="server" Text="" CssClass="Labels" Visible="false"></asp:Label>
                                    <asp:Label ID="lblLabelSubType1" runat="server" Text="" CssClass="Labels" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblDeviceName1" runat="server" Text="Device Name: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:TextBox ID="txtDeviceName1" runat="server" Text='<%# Eval("DeviceName")%>' Width="400px" MaxLength="50"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="Enter Device Name" ControlToValidate="txtDeviceName1" ValidationGroup="vgpUpdate" ForeColor="#C00000" SetFocusOnError="True" /></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblIPAddress1" runat="server" Text="IP Address: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:TextBox ID="txtIPAddress1" runat="server" Text='<%# Eval("IPAddress")%>' Width="400px"></asp:TextBox></td>
                                <td class="Labels">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="Enter IP Address" ControlToValidate="txtIPAddress1" ValidationGroup="vgpUpdate" ForeColor="#C00000" SetFocusOnError="True" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Invalid IP Address"
                                        ValidationExpression="\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b" ControlToValidate="txtIPAddress1" ValidationGroup="vgpUpdate" ForeColor="#C00000" SetFocusOnError="True"></asp:RegularExpressionValidator></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblUseNativeDriver" runat="server" Text="Native Driver: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:CheckBox ID="chkUseNativeDriver1" runat="server" Checked='<%#Eval("Active")%>' Enabled="true" /></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblRRN1" runat="server" Text='<%# Eval("RRN")%>' CssClass="Labels" Visible="false"></asp:Label></td>
                                <td class="LabelsRight">
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" CssClass="Buttons" CausesValidation="False" />
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" OnClick="btnDelete_Click" CssClass="Buttons" CausesValidation="False" />
                                    <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="Buttons" ValidationGroup="vgpUpdate" /></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="errmsg" style="margin-left: auto">
                     <asp:Label ID="lblErrMsg" runat="server" ForeColor="#C00000" EnableViewState="False" CssClass="Labels"></asp:Label>
                </div> 
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
