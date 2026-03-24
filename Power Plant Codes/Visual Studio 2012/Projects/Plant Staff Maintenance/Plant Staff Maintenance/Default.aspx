<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Plant_Staff_Maintenance._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plant Staff Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo">
        <table>
            <tr>
              <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
               <td><h1>Plant Staff Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table>
    </div>
    <div id="divSearch">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID="lblFacility2" runat="server" Text="Facility: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="true" Visible="true" Width ="404px" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="First Name: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="400px" MaxLength="50" ></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
          </table>
    </div>
    </div>
    <div id="divGrid">
             <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass="GridView" AllowPaging ="True" AllowSorting="True" PageSize="25" 
                DataKeyNames="StaffID,Facility" style="margin-top: 12px" width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" 
                EmptyDataText="There are no data records to display." OnPageIndexChanging="gvForm_PageIndexChanging"
                OnSelectedIndexChanged="gvForm_SelectedIndexChanged"
                OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting">
           <Columns>
            <asp:ButtonField Text="Edit" CommandName="Select" ItemStyle-Width="40" >
                <ItemStyle Width="40px"></ItemStyle>
            </asp:ButtonField>
            <asp:TemplateField HeaderText="Active" SortExpression="ActiveRecord" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:CheckBox ID="ckbActiveRecord" runat="server" Checked='<%#Eval("ActiveRecord")%>' Enabled="false"  />
                </ItemTemplate>
            <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Facility" SortExpression="Facility" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:Label ID="lblFacility" runat="server" Text='<%# EVAL("Facility") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Staff ID" SortExpression="StaffID" ItemStyle-Width="80">
                <ItemTemplate>
                    <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("StaffID")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="80px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="First Name" SortExpression="FirstName" ItemStyle-Width="130">
                <ItemTemplate>
                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="130px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Name" SortExpression="LastName" ItemStyle-Width="130">
                <ItemTemplate>
                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="130px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Work Group" SortExpression="WorkGroupDescription" ItemStyle-Width="130">
                <ItemTemplate>
                    <asp:Label ID="lblWorkGroupDescription" runat="server" Text='<%# Eval("WorkGroupDescription")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="130px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Work Sub Group" SortExpression="WorkSubGroupDescription" ItemStyle-Width="200">
                <ItemTemplate>
                    <asp:Label ID="lblWorkSubGroupDescription" runat="server" Text='<%# Eval("WorkSubGroupDescription")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="200px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Access Level" SortExpression="StaffClass" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblStaffClass" runat="server" Text='<%# Eval("StaffClass")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Password" SortExpression="Password" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Label ID="lblPassword" runat="server" Text='<%# Eval("Password")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="100px"></ItemStyle>
              </asp:TemplateField>
            <asp:TemplateField HeaderText="Reset Password" SortExpression="ResetPassword" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:CheckBox ID="ckbResetPassword" runat="server" Checked='<%#Eval("ResetPassword")%>' Enabled="false"  />
                </ItemTemplate>
            <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Work Group" SortExpression="WorkGroup" ItemStyle-Width="70">
                <ItemTemplate>
                    <asp:Label ID="lblWorkGroup" runat="server" Text='<%# Eval("WorkGroup")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="70px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Work Sub Group" SortExpression="WorkSubGroup" ItemStyle-Width="80">
                <ItemTemplate>
                    <asp:Label ID="lblWorkSubGroup" runat="server" Text='<%# Eval("WorkSubGroup")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="80px"></ItemStyle>
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
                    <td class="Labels"><asp:Label ID="lblActiveRecord" runat="server" Text="Active: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:CheckBox ID="chkActiveRecord1" runat="server" Checked='<%#Eval("ActiveRecord")%>' Enabled="true"  /></td>
                    <td></td>
                </tr>

                 <tr>
                    <td class="Labels"><asp:Label ID="lblStaffID1" runat="server" Text="Staff ID: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtStaffID1" runat ="server" Text='<%# Eval("StaffID")%>' Width ="400px" MaxLength="10" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="Enter Staff ID - numbers only"  ControlToValidate="txtStaffID1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                         <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic" ErrorMessage="Allow numbers only."
                        ValidationExpression="\d+" ValidationGroup="vgpAdd" ControlToValidate="txtStaffID1" ForeColor="#C00000"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblFirstName1" runat="server" Text="First Name: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtFirstName1" runat ="server" Text='<%# Eval("FirstName")%>' Width ="400px" MaxLength="50" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter First Name"  ControlToValidate="txtFirstName1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblLastName1" runat="server" Text="Last Name: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtLastName1" runat ="server" Text='<%# Eval("LastName")%>' Width ="400px" MaxLength="50" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Enter Last Name"  ControlToValidate="txtLastName1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblWorkGroup2" runat="server" Text="Work Group: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlWorkGroup" runat="server" AutoPostBack="false" Visible="true"  Width ="404px" OnSelectedIndexChanged="ddlWorkGroup_SelectedIndexChanged" ></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblWorkGroup1" runat ="server" Text='<%# Eval("WorkGroup")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblWorkSubGroup2" runat="server" Text="Work Sub Group: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlWorkSubGroup" runat="server" AutoPostBack="false" Visible="true" Width ="404px" OnSelectedIndexChanged="ddlWorkSubGroup_SelectedIndexChanged" ></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblWorkSubGroup1" runat ="server" Text='<%# Eval("WorkSubGroup")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblStaffClass2" runat="server" Text="Access Level: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlStaffClass" runat="server" AutoPostBack="false" Visible="true"  Width ="404px" OnSelectedIndexChanged="ddlStaffClass_SelectedIndexChanged"></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblStaffClass1" runat ="server" Text='<%# Eval("StaffClass")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>                    
                <tr>
                    <td class="Labels"><asp:Label ID="lblPassword1" runat="server" Text="Password: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtPassword1" runat ="server" Text='<%# Eval("Password")%>' Width ="400px" MaxLength="20" TextMode="SingleLine" ></asp:TextBox></td>
                    <td class="Labels"><asp:RegularExpressionValidator ID="revPassword2" runat="server" Display="Dynamic" ErrorMessage="Allow numbers only."
                        ValidationExpression="\d+" ValidationGroup="vgpAdd" ControlToValidate="txtPassword1" ForeColor="#C00000"></asp:RegularExpressionValidator><asp:Label ID="lblPassword2" runat="server" Text="Pass" CssClass="Labels" Visible ="false" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblResetPassword" runat="server" Text="Reset Password: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:CheckBox ID="chkResetPassword1" runat="server" Checked='<%#Eval("ResetPassword")%>' Enabled="true"  /></td>
                    <td></td>
                </tr>
                 <tr>
                     <td class="Labels">
                         <asp:Label ID="lblStaffID9" runat="server" Text="0" CssClass="Labels" Visible="false" ForeColor="#C00000"></asp:Label>
                         <asp:Label ID="lblFacility9" runat="server" Text="0" CssClass="Labels" Visible="false"></asp:Label></td>
                     <td class="LabelsRight">
                         <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" CssClass="Buttons" />
                         <asp:Button ID="btnDelete" Text="Delete" runat="server" OnClick="btnDelete_Click" CssClass="Buttons" />
                         <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="Buttons" /></td>
                    <td></td>
                </tr>
            </table>
        </div>
        &nbsp;
            <div id="errmsg">
                <asp:Label ID="lblErrMsg" runat="server" ForeColor="#C00000" EnableViewState="False" CssClass="Labels"></asp:Label>
            </div> 

    </form>
</body>
</html>
