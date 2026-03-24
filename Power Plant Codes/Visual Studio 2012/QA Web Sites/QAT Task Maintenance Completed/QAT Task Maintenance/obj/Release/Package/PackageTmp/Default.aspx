<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QAT_Task_Maintenance._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
          <title>QAT Task Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="divHeader" class ="DivLeft">
      <div id="divLogo">
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Task Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
              </tr>
        </table>    
     </div>
     <div id="divSearch">
        <table>
        <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Task: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="402px" MaxLength="50" ></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
        </table>    
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server"  AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="18" AllowSorting="True" 
            DataKeyNames="TaskID"  style="margin-top: 12px" CellPadding="4" width="100%"
            EmptyDataText="There are no data records to display." ForeColor="#333333" GridLines="None" EnableModelValidation="True" 
            OnPageIndexChanging="gvForm_PageIndexChanging"
            OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting" HeaderStyle-VerticalAlign="Top">
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
                        <asp:Label ID="lblFacility" runat="server" Text='<%# Eval("Facility")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                  </asp:TemplateField>
                    <asp:TemplateField HeaderText="Task" SortExpression="TaskDescription" ItemStyle-Width="150" >
                    <ItemTemplate>
                        <asp:Label ID="lblTaskDescription" runat="server" Text='<%# Eval("TaskDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                           <asp:TemplateField HeaderText="Task Seq" SortExpression="TaskSeq" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblTaskSeq" runat="server" Text='<%# Eval("TaskSeq")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Definition" SortExpression="QATDefnDescription" ItemStyle-Width="300" >
                    <ItemTemplate>
                        <asp:Label ID="lblQATDefnDescription" runat="server" Text='<%# Eval("QATDefnDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Note" SortExpression="NoteDescription" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblNoteDescription" runat="server" Text='<%# Eval("NoteDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField> 

               <asp:TemplateField HeaderText="TaskID" SortExpression="TaskID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblTaskID" runat="server" Text='<%# Eval("TaskID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="DefnID" SortExpression="QATDefnID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblQATDefnID" runat="server" Text='<%# Eval("QATDefnID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="NoteID" SortExpression="NoteID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblNoteID" runat="server" Text='<%# Eval("NoteID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
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
                <td class="Labels"><asp:Label ID="lblActive5" runat="server" Text="Active: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkActive1" runat="server" Checked='<%#Eval("Active")%>' Enabled="true"  /></td>
                <td></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblFacility5" runat="server" Text="Facility: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
         <tr>
                <td class="Labels"><asp:Label ID="lblTaskDescription5" runat="server" Text="Task: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlTaskDescription" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlTaskDescription_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblTaskDescription1" runat ="server" Text='<%# Eval("TaskDescription")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
          <tr>
                <td class="Labels"><asp:Label ID="lblTaskSeq" runat="server" Text="Task Seq: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlTaskSeq" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlTaskSeq_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblTaskSeq1" runat ="server" Text='<%# Eval("TaskSeq")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
          <tr>
                <td class="Labels"><asp:Label ID="lblQATDefnDescription5" runat="server" Text="Definition: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlQATDefnDescription" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlQATDefnDescription_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblQATDefnDescription1" runat ="server" Text='<%# Eval("QATDefnDescription")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>

                     <tr>
                <td class="Labels"><asp:Label ID="lblNoteDescription5" runat="server" Text="Note: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlNoteDescription" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlNoteDescription_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblNoteDescription1" runat ="server" Text='<%# Eval("NoteDescription")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>

 
                <tr>
                <td class="Labels"><asp:label ID="lblTaskID9" runat="server" Text='<%# Eval("TaskID")%>' CssClass="Labels" Visible="false" ></asp:label><asp:label ID="lblFacility9" runat="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:label><asp:label ID="lblQATDefnID9" runat="server" Text='<%# Eval("QATDefnID")%>' CssClass="Labels" Visible="false" ></asp:label></td>
                <td class="LabelsRight"><asp:Button ID="btnCancel" Text="Cancel" runat ="server" OnClick="btnCancel_Click" CssClass="Buttons" />    <asp:Button ID="btnDelete" Text="Delete" runat ="server" OnClick="btnDelete_Click"  CssClass="Buttons" />    <asp:Button ID="btnSave" Text="Save" runat ="server" OnClick="btnSave_Click" CssClass="Buttons"  /></td>
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
    </div>
    </form>
</body>
</html>
