<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QATTaskMasterMaintenance._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QAT Task Master Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo">
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Task Master Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table>
    </div>
    <div id="divSearch">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Task Description: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="400px" MaxLength="50" ></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
        </table>
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="20" AllowSorting="True" 
        style="margin-top: 12px" Width="100%"  CellPadding="4" EmptyDataText="There are no data records to display." ForeColor="#333333" GridLines="None" EnableModelValidation="True"
            OnPageIndexChanging="gvForm_PageIndexChanging" OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound"
                   OnRowCreated="gvForm_RowCreated"   OnSorting="gvForm_Sorting" >
            <Columns>
                      <asp:ButtonField Text="Edit" CommandName="Select" ItemStyle-Width="40" >
            <ItemStyle Width="40px"></ItemStyle>
            </asp:ButtonField>
            <asp:TemplateField HeaderText="Active" SortExpression="Active" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:CheckBox ID="ckbActive" runat="server" Checked='<%#Eval("Active")%>' Enabled="false"  />
                </ItemTemplate>
            <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Facility" SortExpression="Facility" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:Label ID="lblFacility" runat="server" Text='<%# EVAL("Facility") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>                     
      <asp:TemplateField HeaderText="Task Description" SortExpression="TaskDescription" ItemStyle-Width="200">
                <ItemTemplate>
                    <asp:Label ID="lblTaskDescription" runat="server" Text='<%# Eval("TaskDescription")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="200px"></ItemStyle>
            </asp:TemplateField>
                          <asp:TemplateField HeaderText="Updated At" SortExpression="UpdatedAt">
                <ItemTemplate>
                    <asp:Label ID="lblUpdatedAt" runat="server" Text='<%# Eval("UpdatedAt")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Updated By" SortExpression="UpdatedBy">
                <ItemTemplate>
                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Eval("UpdatedBy")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Task ID" SortExpression="TaskID">
                <ItemTemplate>
                    <asp:Label ID="lbltaskID" runat="server" Text='<%# Eval("TaskID")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
 
            </Columns>
            <AlternatingRowStyle BackColor="White" />

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />

        </asp:GridView>
    </div>
               &nbsp;
    <div id="divEdit" class="DivLeft" style="display: inline-block; float: left">
        <table>
             <tr>
                    <td class="Labels"><asp:Label ID="lblActive" runat="server" Text="Active: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:CheckBox ID="chkActive1" runat="server" Checked='<%#Eval("Active")%>' Enabled="true"  /></td>
                    <td></td>
                </tr>
                       <tr>
                    <td class="Labels"><asp:Label ID="lblFacility" runat="server" Text="Facility: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" ></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                 </tr>
             <tr>
                    <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblTaskDescription" runat="server" Text="Task Description: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtTaskDescription1" runat ="server" Text='<%# Eval("TaskDescription")%>' Width ="400px" MaxLength="50" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter Task Description"  ControlToValidate="txtTaskDescription1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
                </tr>
                            <tr>
                    <td class="Labels"><asp:label ID="lblTaskID1" runat="server" Text='<%# Eval("TaskID")%>' CssClass="Labels" Visible="false"  ></asp:label></td>
                    <td class="LabelsRight"><asp:Button ID="btnCancel" Text="Cancel" runat ="server" OnClick="btnCancel_Click" CssClass="Buttons" />    <asp:Button ID="btnDelete" Text="Delete" runat ="server" OnClick="btnDelete_Click"  CssClass="Buttons" />    <asp:Button ID="btnSave" Text="Save" runat ="server" OnClick="btnSave_Click"  CssClass="Buttons"  /></td>
                    <td></td>
                </tr>
        </table>
    </div>
    <div id="divMessage" class="DivLeft">
        <table>
         <tr>
            <td></td>
            <td class="LabelsCenter">
                <asp:Label ID="lblErrMsg" runat="server" ForeColor="#C00000"></asp:Label>
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
