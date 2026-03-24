<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QAT_Specification_Formula_Maintenance._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>QAT Specification Formula Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo">
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Specification Formula Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table>
    </div>
    <div id="divSearch">
        <table>
           <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Description: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="400px" MaxLength="255" ></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
        </table>
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="30" AllowSorting="True" 
        DataKeyNames="FormulaID"  style="margin-top: 12px" Width="100%"  OnPageIndexChanging="gvForm_PageIndexChanging"
        OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting"
        CellPadding="4" EmptyDataText="There are no data records to display." ForeColor="#333333" GridLines="None" EnableModelValidation="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
            <asp:ButtonField Text="Edit" CommandName="Select" ItemStyle-Width="40" >
                <ItemStyle Width="40px"></ItemStyle>
            </asp:ButtonField>
             <asp:TemplateField HeaderText="Formula ID" SortExpression="FormulaID" ItemStyle-Width="100">
                <ItemTemplate>
                    <asp:Label ID="lblFormulaID" runat="server" Text='<%# Eval("FormulaID")%>'></asp:Label>
                </ItemTemplate>
                 <ItemStyle Width="100px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Width="300">
                <ItemTemplate>
                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="300px"></ItemStyle>
            </asp:TemplateField> 

            </Columns>
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />

        </asp:GridView>
    </div>
         &nbsp;
    <div id="divEdit"  class="DivLeft" style="display: inline-block; float: left">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID="lblFormulaID5" runat="server" Text="Formula ID: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlFormulaID" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFormulaID_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblFormulaID9" runat ="server" Text='<%# Eval("Description")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
 
               <tr>
                    <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblDescription" runat="server" Text="Description: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtDescription1" runat ="server" Text='<%# Eval("Description")%>' Width ="400px" Height ="100px" TextMode="MultiLine" MaxLength="255" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter Description"  ControlToValidate="txtDescription1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
                </tr>
                <tr>
                    <td class="Labels"><asp:label ID="lblFormulaID1" runat="server" Text='<%# Eval("FormulaID")%>' CssClass="Labels" Visible="false"  ></asp:label></td>
                    <td class="LabelsRight"><asp:Button ID="btnCancel" Text="Cancel" runat ="server" OnClick="btnCancel_Click" CssClass="Buttons" />    <asp:Button ID="btnDelete" Text="Delete" runat ="server" OnClick="btnDelete_Click" CssClass="Buttons" />    <asp:Button ID="btnSave" Text="Save" runat ="server" OnClick="btnSave_Click" CssClass="Buttons"  /></td>
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
