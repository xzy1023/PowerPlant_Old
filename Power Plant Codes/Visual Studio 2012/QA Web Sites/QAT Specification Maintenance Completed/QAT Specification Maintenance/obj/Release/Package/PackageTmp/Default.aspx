<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QATSpecificationMaintenance._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QAT Specification Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo" >
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Specification Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table>   
    </div>
    <div id="divSearch">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Test Specification: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="400px" MaxLength="125"></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
        </table>
      </div>
      </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="20" AllowSorting="True" 
        DataKeyNames="TestSpecID,Facility"  style="margin-top: 12px" Width="100%"  OnPageIndexChanging="gvForm_PageIndexChanging"
        OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting"
        CellPadding="4" EmptyDataText="There are no data records to display." ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
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
            <asp:TemplateField HeaderText="Test Specification" SortExpression="TestSpecDesc" ItemStyle-Width="320">
                <ItemTemplate>
                    <asp:Label ID="lblTestSpecDesc" runat="server" Text='<%# Eval("TestSpecDesc")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="320px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Formula" SortExpression="FormulaDesc" ItemStyle-Width="300">
                <ItemTemplate>
                    <asp:Label ID="lblFormulaDesc" runat="server" Text='<%# Eval("FormulaDesc")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="300px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Lower Limit From Target" SortExpression="LwLmtFromTarget" ItemStyle-Width="110">
                <ItemTemplate>
                    <asp:Label ID="lblLwLmtFromTarget" runat="server" Text='<%# Eval("LwLmtFromTarget")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="110px"></ItemStyle>
            </asp:TemplateField>      
            
             <asp:TemplateField HeaderText="Upper Limit From Target" SortExpression="UpLmtFromTarget" ItemStyle-Width="110">
                <ItemTemplate>
                    <asp:Label ID="lblUpLmtFromTarget" runat="server" Text='<%# Eval("UpLmtFromTarget")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="110px"></ItemStyle>
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
            <asp:TemplateField HeaderText="ID" SortExpression="TestSpecID">
                <ItemTemplate>
                    <asp:Label ID="lblTestSpecID" runat="server" Text='<%# Eval("TestSpecID")%>'></asp:Label>
                </ItemTemplate>
                 <ItemStyle Width="60px"></ItemStyle>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Formula" SortExpression="Formula" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:Label ID="lblFormula" runat="server" Text='<%# Eval("Formula")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>
        </Columns> 
        <FooterStyle CssClass="GridView-Footer" BackColor="#990000" ForeColor="White" Font-Bold="True" />
        <HeaderStyle CssClass="GridView-Header" BackColor="#990000" Font-Bold="True" ForeColor="White" /> 
        <PagerStyle CssClass="GridView-Pager" BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
		    <RowStyle CssClass="GridView-RowStyle" BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle CssClass="GridView-SelectedRow" BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" /> 
    </asp:GridView>
    &nbsp;
    </div>
    <div id="divEdit" class="DivLeft" style="display: inline-block; float: left">
              <table >    
                <tr>
                    <td class="Labels"><asp:Label ID="lblActive5" runat="server" Text="Active: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:CheckBox ID="chkActive1" runat="server" Checked='<%#Eval("Active")%>' Enabled="true"  /></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblFacility5" runat="server" Text="Facility: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>           
                <tr>
                    <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblTestSpecDesc5" runat="server" Text="Test Specification: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtTestSpecDesc1" runat ="server" Text='<%# Eval("TestSpecDesc")%>' Width ="400px" Height ="100px" TextMode="MultiLine" MaxLength="125" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter Test Specification Description"  ControlToValidate="txtTestSpecDesc1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
                </tr>
                 <tr>
                    <td class="Labels"><asp:Label ID="lblFormulaDesc5" runat="server" Text="Formula: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlFormulaDesc" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFormulaDesc_SelectedIndexChanged" Width ="406px"></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblFormulaDesc1" runat ="server" Text='<%# Eval("FormulaDesc")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>  
                 <tr>
                    <td class="Labels"><asp:Label ID="lblLwLmtFromTarget5" runat="server" Text="Lower Limit From Target: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtLwLmtFromTarget1" runat ="server" Text='<%# Eval("TestCategory")%>' Width ="150px" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Enter Lower Limit From Target"  ControlToValidate="txtLwLmtFromTarget1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator2"  runat="server" Display="Dynamic" ErrorMessage="Allow numbers only (e.g. -123.456789)" ControlToValidate="txtLwLmtFromTarget1"  ValidationExpression="-?\d+(\.\d{1,6})?"   ForeColor="#C00000" />
                     </td>
                </tr>
                <tr>
                    <td class="Labels"><asp:Label ID="lblUpLmtFromTarget5" runat="server" Text="Upper Limit From Target: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtUpLmtFromTarget1" runat ="server" Text='<%# Eval("FormName")%>' Width ="150px" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="Enter Upper Limit From Target"  ControlToValidate="txtUpLmtFromTarget1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator3"  runat="server" Display="Dynamic" ErrorMessage="Allow numbers only (e.g. -123.456789)" ControlToValidate="txtUpLmtFromTarget1"  ValidationExpression="-?\d+(\.\d{1,6})?"  ForeColor="#C00000"  /></td>
                </tr>

                <tr>
                    <td class="Labels"><asp:label ID="lblTestSpecID1" runat="server" Text='<%# Eval("TestSpecID")%>' CssClass="Labels" Visible="false"  ></asp:label></td>
                    <td class="LabelsRight"><asp:Button ID="btnCancel" Text="Cancel" runat ="server" OnClick="btnCancel_Click" CssClass="Buttons" />    <asp:Button ID="btnDelete" Text="Delete" runat ="server" OnClick="btnDelete_Click"  CssClass="Buttons" />    <asp:Button ID="btnSave" Text="Save" runat ="server" OnClick="btnSave_Click"  CssClass="Buttons"  /></td>
                    <td></td>
                </tr>
            </table>    
  
    </div>
    &nbsp;
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
    &nbsp;&nbsp;
    </form>
</body>
</html>
