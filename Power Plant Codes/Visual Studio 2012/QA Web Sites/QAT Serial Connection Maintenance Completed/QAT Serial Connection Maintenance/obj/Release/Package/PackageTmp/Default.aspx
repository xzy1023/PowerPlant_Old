<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QAT_Serial_Connection_Maintenance._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
            <title>QAT Serial Connection Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo">
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Serial Connection Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table>    
    </div>
    <div id="divSearch">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Description: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="400px" MaxLength="50"></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
        </table>       
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="25" AllowSorting="True" 
            DataKeyNames="SerialConnID,Facility"  style="margin-top: 12px" Width="100%" CellPadding="4" 
            EmptyDataText="There are no data records to display." ForeColor="#333333" GridLines="None" EnableModelValidation="True" 
            OnPageIndexChanging="gvForm_PageIndexChanging"
            OnSelectedIndexChanged="gvForm_SelectedIndexChanged"  OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting" >
            <Columns>
                 <asp:ButtonField Text="Edit" CommandName="Select" ItemStyle-Width="40" >
                <ItemStyle Width="40px"></ItemStyle>
                </asp:ButtonField>
               <asp:TemplateField HeaderText="Active" SortExpression="Active" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbActive" runat="server" Checked='<%#Eval("Active")%>' Enabled="false"  />
                    </ItemTemplate>
                <ItemStyle Width="80px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Facility" SortExpression="Facility" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblFacility" runat="server" Text='<%# EVAL("Facility") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Com Port" SortExpression="ComPort" ItemStyle-Width="110">
                    <ItemTemplate>
                        <asp:Label ID="lblComPort" runat="server" Text='<%# Eval("ComPort")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Baud Rate" SortExpression="BaudRate" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblBaudRate" runat="server" Text='<%# Eval("BaudRate")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Parity" SortExpression="ParityDesc" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:Label ID="lblParityDesc" runat="server" Text='<%# Eval("ParityDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Bits" SortExpression="DataBits" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblDataBits" runat="server" Text='<%# Eval("DataBits")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:TemplateField>      
                <asp:TemplateField HeaderText="Stop Bits" SortExpression="StopBitsDesc" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblStopBitsDesc" runat="server" Text='<%# Eval("StopBitsDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:TemplateField>

               <asp:TemplateField HeaderText="Description" SortExpression="SerialConnDesc" ItemStyle-Width="400">
                    <ItemTemplate>
                        <asp:Label ID="lblSerialConnDesc" runat="server" Text='<%# Eval("SerialConnDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="400px"></ItemStyle>
                </asp:TemplateField>
          
                <asp:TemplateField HeaderText="Parity" SortExpression="Parity" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblParity" runat="server" Text='<%# Eval("Parity")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px"></ItemStyle>
                </asp:TemplateField>
  
                <asp:TemplateField HeaderText="Stop Bits" SortExpression="StopBits" ItemStyle-Width="60">
                   <ItemTemplate>
                        <asp:Label ID="lblStopBits" runat="server" Text='<%# Eval("StopBits")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px"></ItemStyle>
                </asp:TemplateField>

               <asp:TemplateField HeaderText="Serial Conn ID" SortExpression="SerialConnID" ItemStyle-Width="6">
                    <ItemTemplate>
                        <asp:Label ID="lblSerialConnID" runat="server" Text='<%# Eval("SerialConnID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px"></ItemStyle>
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
                    <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" Width="100"></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>
                <tr>
                <td class="Labels"><asp:Label ID="lblComPort5" runat="server" Text="Com Port: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:TextBox ID="txtComPort1" runat ="server" Text='<%# Eval("ComPort")%>' Width ="96px" MaxLength="2" ></asp:TextBox></td>
                <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Enter Com Port - positive numbers only"  ControlToValidate="txtComPort1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic" ErrorMessage="Allow positive numbers only."
                        ValidationExpression="^[+]?\d+" ValidationGroup="vgpAdd" ControlToValidate="txtComPort1" ForeColor="#C00000"></asp:RegularExpressionValidator></td>
             </tr>
                 <tr>
                    <td class="Labels"><asp:Label ID="lblBaudRate" runat="server" Text="Baud Rate: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:DropDownList ID="ddlBaudRate" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlBaudRate_SelectedIndexChanged" Width="100" ></asp:DropDownList></td>
                     <td class="Labels"><asp:Label ID="lblBaudRate1" runat ="server" Text='<%# Eval("BaudRate")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
              </tr>
             <tr>
                    <td class="Labels"><asp:Label ID="lblParity" runat="server" Text="Parity: " CssClass="Labels"></asp:Label></td>
                  <td class="Labels"><asp:DropDownList ID="ddlParity" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlParity_SelectedIndexChanged" Width="100"></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblParity1" runat ="server" Text='<%# Eval("Parity")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
               </tr>
                 <tr>
                    <td class="Labels"><asp:Label ID="lblDataBits" runat="server" Text="Data Bits: " CssClass="Labels"></asp:Label></td>
                   <td class="Labels"><asp:DropDownList ID="ddlDataBits" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlDataBits_SelectedIndexChanged" Width="100" ></asp:DropDownList></td>
                      <td class="Labels"><asp:Label ID="lblDataBits1" runat ="server" Text='<%# Eval("DataBits")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                </tr>
                  <tr>
                    <td class="Labels"><asp:Label ID="lblStopBits" runat="server" Text="Stop Bits: " CssClass="Labels"></asp:Label></td>
                  <td class="Labels"><asp:DropDownList ID="ddlStopBits" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlStopBits_SelectedIndexChanged" Width="100"></asp:DropDownList></td>
                      <td class="Labels"><asp:Label ID="lblStopBits1" runat ="server" Text='<%# Eval("StopBits")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
               </tr>
               <tr>
                    <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblSerialConnDesc5" runat="server" Text="Description: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtSerialConnDesc1" runat ="server" Text='<%# Eval("SerialConnDesc")%>' Width ="400px" MaxLength="50" ></asp:TextBox></td>
                    <td></td>
               </tr>
                <tr>
                    <td class="Labels"><asp:label ID="lblSerialConnID9" runat="server" Text='<%# Eval("SerialConnID")%>' CssClass="Labels" Visible="false"  ></asp:label></td>
                    <td class="LabelsRight"><asp:Button ID="btnCancel" Text="Cancel" runat ="server" OnClick="btnCancel_Click" CssClass="Buttons" />    <asp:Button ID="btnDelete" Text="Delete" runat ="server" OnClick="btnDelete_Click" CssClass="Buttons" />    <asp:Button ID="btnSave" Text="Save" runat ="server" OnClick="btnSave_Click" CssClass="Buttons"  /></td>
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
