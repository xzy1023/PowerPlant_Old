<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Package_Line_Rate_Maintenance._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>servePackage Line Rate Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            color: black;
            font-size: 12px;
            margin-left: 5px;
            text-align: left;
            height: 34px;
        }

        .auto-style2 {
            height: 34px;
        }
    </style>
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
                                    <h1>Machine Rate Maintenance (User:
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
                                    <asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="True" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" Width="200px"></asp:DropDownList>

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
                                    <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/App_Themes/PP/Images/go.png" CausesValidation="False" /></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ibtnSearch" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnSave" />
                <%--<asp:AsyncPostBackTrigger ControlID="btnDelete" />--%>
                <asp:AsyncPostBackTrigger ControlID="ddlFacility" EventName="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <div id="divGrid">
                        <asp:GridView ID="gvOverrideRates" runat="server" AutoGenerateColumns="false" CssClass="GridView" AllowPaging="True" PageSize="5" AllowSorting="True"
                            HeaderStyle-VerticalAlign="Top" Style="margin-top: 12px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                            EmptyDataText="-- There are no data records to display in grid. --" DataKeyNames="RRN"
                            OnSelectedIndexChanged="gvOverrideRates_SelectedIndexChanged" OnSorting="gvOverrideRates_Sorting" OnPageIndexChanging="gvForm_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" />
                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                            <SortedAscendingCellStyle BackColor="#FDF5AC" />
                            <SortedAscendingHeaderStyle BackColor="#4D0000" />
                            <SortedDescendingCellStyle BackColor="#FCF6C0" />
                            <SortedDescendingHeaderStyle BackColor="#820000" />
                            <Columns>
                                <asp:ButtonField Text="Edit" CommandName="Select" ItemStyle-Width="30">
                                    <ItemStyle Width="40px"></ItemStyle>
                                </asp:ButtonField>

                                <asp:TemplateField HeaderText="Facility" ItemStyle-Width="30">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Facility") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Packaging Line" SortExpression="MachineID" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("MachineID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Description")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Item Number" SortExpression="ItemNumber" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("ItemNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rate Multiplier" SortExpression="RateMultiplier" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("RateMultiplier")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Logic For Rate Multiplier" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("LogicForRateMultiplier")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Run Operators Multiplier" SortExpression="RunOperatorsMultiplier" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("RunOperatorsMultiplier")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" ItemStyle-Width="30">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%#Eval("Active")%>' Enabled="false"></asp:CheckBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px"></ItemStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="RRN" SortExpression="RRN" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRRN" runat="server" Text='<%# Eval("RRN")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60px"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </div>

                    <br />

                    <div id="divEdit" class="DivLeft">
                        <table>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblLine" runat="server" Text="Packaging Line: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:DropDownList ID="ddlLine" runat="server" AutoPostBack="true" Visible="true" OnSelectedIndexChanged="ddlLine_SelectedIndexChanged" Width="404px"></asp:DropDownList></td>
                                <td>
                                    <asp:RequiredFieldValidator ControlToValidate="ddlLine" Text="Packaging Line field is required!" runat="server" Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblItemNumber" runat="server" Text="Item Number: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:DropDownList ID="ddlItemNumber" runat="server" Visible="true" Width="404px" AutoPostBack="True"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblRateMultiplier" runat="server" Text="Rate Multiplier: " CssClass="Labels"></asp:Label>

                                </td>
                                <td class="Labels">
                                    <asp:TextBox ID="txtRateMultiplier" runat="server" Text='<%# Eval("RateMultiplier")%>' Width="400px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ControlToValidate="txtRateMultiplier" Text="Rate Multiplier field is required!" runat="server" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ControlToValidate="txtRateMultiplier" Text="Rate Multiplier field is required Integer or Decimal(up to four decimal places) format."
                                        runat="Server" ValidationExpression="^(([^0][0-9]+|0)\.([0-9]{1,4})$)|^(([^0][0-9]+|0)$)|^(([1-9]+)\.([0-9]{1,4})$)|^(([1-9]+)$)" Display="Dynamic" />
                                    <asp:RangeValidator runat="server" ID="vldRateMultiplier" ControlToValidate="txtRateMultiplier" Type="Double" Display="Dynamic" />

                                </td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblLogicForRateMultiplier" runat="server" Text="Logic For Rate Multiplier: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:TextBox ID="txtLogicForRateMultiplier" runat="server" Text='<%# Eval("LogicForRateMultiplier")%>' Width="400px" MaxLength="50"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ControlToValidate="txtLogicForRateMultiplier" Text="Logic For Rate Multiplier field is required!" runat="server" Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblRunOperatorsMultiplier" runat="server" Text="Run Operators Multiplier: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:TextBox ID="txtRunOperatorsMultiplier" runat="server" Text='<%# Eval("RunOperatorsMultiplier")%>' Width="400px" MaxLength="50"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ControlToValidate="txtRunOperatorsMultiplier" Text="Run Operators Multiplier field is required!" runat="server" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ControlToValidate="txtRunOperatorsMultiplier" Text="Run Operators Multiplier field is required Integer format."
                                        runat="Server" ValidationExpression="^(([^0][0-9]+|0)$)|^(([1-9]+)$)" Display="Dynamic" />
                                    <asp:RangeValidator runat="server" ID="vldRunOperatorsMultiplier" ControlToValidate="txtRunOperatorsMultiplier" Type="Integer" Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblActive" runat="server" Text="Active: " CssClass="Labels"></asp:Label></td>
                                <td class="Labels">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Eval("Active")%>' Enabled="true" /></td>
                            </tr>
                            <%--                            <tr>
                                <td class="LabelsRight">
                                    <asp:Label ID="lblRRN" runat="server" Text='<%# Eval("RRN")%>' CssClass="Labels" Visible="false"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="Labels">
                                    <asp:Label ID="lblRRN" runat="server" Text='<%# Eval("RRN")%>' CssClass="Labels" Visible="false"></asp:Label>
                                </td>
                                <td class="LabelsRight">
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" CssClass="Buttons" CausesValidation="False" />
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" OnClick="btnDelete_Click" CssClass="Buttons" CausesValidation="False" />
                                    <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="Buttons" />
                                </td>

                            </tr>
                            <%--<asp:ValidationSummary ID="Validation" runat="server" ShowSummary="true" DisplayMode="BulletList" Style="color: Red" />--%>
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
