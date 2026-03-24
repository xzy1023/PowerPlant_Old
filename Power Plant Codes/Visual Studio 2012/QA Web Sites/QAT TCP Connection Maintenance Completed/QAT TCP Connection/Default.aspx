<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QAT_TCP_Connection._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>QAT TCP Connection Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo">
        <table>
          <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT TCP Connection Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table>    
        </div>
    <div id="divSearch">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Description: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server"  Width ="398px" MaxLength="100"></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  CssClass="Buttons"  /></td>
            </tr>
        </table>     
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="20" AllowSorting="True" 
        DataKeyNames="TCPConnID,Facility"  style="margin-top: 12px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" EnableModelValidation="True"
            OnPageIndexChanging="gvForm_PageIndexChanging"
        OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting"
            EmptyDataText="There are no data records to display.">
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
            <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Width="300">
                <ItemTemplate>
                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="300px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="IP Address" SortExpression="IPAddress" ItemStyle-Width="80">
                <ItemTemplate>
                    <asp:Label ID="lblIPAddress" runat="server" Text='<%# Eval("IPAddress")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="80px"></ItemStyle>
            </asp:TemplateField>
          <asp:TemplateField HeaderText="Model" SortExpression="Model" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:Label ID="lblModel" runat="server" Text='<%# Eval("Model")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="Port" SortExpression="Port" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:Label ID="lblPort" runat="server" Text='<%# Eval("Port")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
            </asp:TemplateField> 
 
            <asp:TemplateField HeaderText="Command 1" SortExpression="Command1" ItemStyle-Width="200">
                <ItemTemplate>
                    <asp:Label ID="lblCommand1" runat="server" Text='<%# Eval("Command1")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="200px"></ItemStyle>
            </asp:TemplateField>  

            <asp:TemplateField HeaderText="Command 2" SortExpression="Command2" ItemStyle-Width="200">
                <ItemTemplate>
                    <asp:Label ID="lblCommand2" runat="server" Text='<%# Eval("Command2")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="200px"></ItemStyle>
            </asp:TemplateField> 
                
            <asp:TemplateField HeaderText="Command 3" SortExpression="Command3" ItemStyle-Width="200">
                <ItemTemplate>
                    <asp:Label ID="lblCommand3" runat="server" Text='<%# Eval("Command3")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="200px"></ItemStyle>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Conn ID" SortExpression="TCPConnID" ItemStyle-Width="60">
                <ItemTemplate>
                    <asp:Label ID="lblTCPConnID" runat="server" Text='<%# Eval("TCPConnID")%>'></asp:Label>
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
                    <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" ></asp:DropDownList></td>
                    <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
                 </tr>
               <tr>
                    <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblDescription" runat="server" Text="Description: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtDescription1" runat ="server" Text='<%# Eval("Description")%>' Width ="398px" CssClass="Textboxs" Height ="30px" TextMode="MultiLine" MaxLength="100" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="rfvNoteDescription1" runat="server" Display="Dynamic" ErrorMessage="Enter Description"  ControlToValidate="txtDescription1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
                </tr>
 
              <tr>
                    <td class="Labels"><asp:Label ID="lblIPAddress1" runat="server" Text="IP Address: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtIPAddress1" runat ="server" Text='<%# Eval("IPAddress")%>' Width ="400px"></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter IP Address"  ControlToValidate="txtIPAddress1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"  ErrorMessage="Invalid IP Address"
                            ValidationExpression="\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b" ControlToValidate="txtIPAddress1" ValidationGroup="vgpUpdate" ForeColor="#C00000" ></asp:RegularExpressionValidator></td>       
            </tr>               
            <tr>
                    <td class="Labels"><asp:Label ID="lblModel" runat="server" Text="Model: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtModel1" runat ="server" Text='<%# Eval("Model")%>' Width ="400px" CssClass="Textboxs" MaxLength="50" ></asp:TextBox></td>
                    <td></td>
                </tr>
                 <tr>
                    <td class="Labels"><asp:Label ID="lblPort" runat="server" Text="Port: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtPort1" runat ="server" Text='<%# Eval("Port")%>' Width ="400px" CssClass="Textboxs" MaxLength="50" ></asp:TextBox></td>
                    <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Enter Port"  ControlToValidate="txtPort1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                        <asp:RangeValidator runat="server" id="rngUpdate" controltovalidate="txtPort1" type="integer" minimumvalue="0" maximumvalue="65535" errormessage="Please enter a number range from 0 to 65535" ForeColor="#C00000"  /></td>
                </tr>

               <tr>
                    <td class="Labels"><asp:Label ID="lblCommand11" runat="server" Text="Command 1: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtCommand11" runat ="server" Text='<%# Eval("Command1")%>' Width ="400px" CssClass="Textboxs" MaxLength="50" ></asp:TextBox></td>
                    <td></td>
                </tr>
               <tr>
                    <td class="Labels"><asp:Label ID="lblCommand21" runat="server" Text="Command 2: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtCommand21" runat ="server" Text='<%# Eval("Command2")%>' Width ="400px" CssClass="Textboxs" MaxLength="50" ></asp:TextBox></td>
                    <td></td>
                </tr>
               <tr>
                    <td class="Labels"><asp:Label ID="lblCommand31" runat="server" Text="Command 3: " CssClass="Labels"></asp:Label></td>
                    <td class="Labels"><asp:TextBox ID="txtCommand31" runat ="server" Text='<%# Eval("Command3")%>' Width ="400px" CssClass="Textboxs" MaxLength="50" ></asp:TextBox></td>
                    <td></td>
                </tr>
               <tr>
                    <td class="Labels"><asp:label ID="lblTCPConnID9" runat="server" Text='<%# Eval("TCPConnID")%>' CssClass="Labels" Visible="false"  ></asp:label></td>
                    <td class="LabelsRight"><asp:Button ID="btnCancel" Text="Cancel" runat ="server" OnClick="btnCancel_Click" CssClass="Buttons" />    <asp:Button ID="btnDelete" Text="Delete" runat ="server" OnClick="btnDelete_Click" CssClass="Buttons" />    <asp:Button ID="btnSave" Text="Save" runat ="server" OnClick="btnSave_Click"  CssClass="Buttons"  /></td>
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
