<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QAT_Workflow_Maintenance._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QAT Workflow Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="divHeader" class ="DivLeft">
    <div id="divLogo">
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Workflow Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
            </tr>
        </table> 
    </div>
    <div id="divSearch">
        <table>
            <tr>
                <td class="Labels"><asp:Label ID="lblFacilitySearch" runat="server" Text="Facility: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlFacilitySearch" runat="server" AutoPostBack="true" Visible="true" OnSelectedIndexChanged="ddlFacilitySearch_SelectedIndexChanged"  Width="100px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lbllPackagingLineSearch" runat="server" Text="Packaging Line: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlPackagingLineSearch" runat="server" AutoPostBack="true" Visible="true" OnSelectedIndexChanged="ddlPackagingLineSearch_SelectedIndexChanged"  Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblEntryPointDesc5" runat="server" Text="Workflow Type: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlEntryPointSearch" runat="server" AutoPostBack="true" Visible="true" OnSelectedIndexChanged="ddlEntryPointSearch_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Font-Size="Small" Visible ="false" ></asp:TextBox> </td>
                <td class="Labels"><asp:TextBox ID="txtSearch1" runat="server" Font-Size="Small" Visible ="false" ></asp:TextBox> </td>
                <td class="Labels"><asp:Label ID="lblQATEntryPoint1" runat ="server" Text='<%# Eval("QATEntryPoint")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
        </table>     
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="20" AllowSorting="True" 
            DataKeyNames="QATDefnID,Facility"  style="margin-top: 12px"  CellPadding="4" width="100%"
            EmptyDataText="There are no data records to display." ForeColor="#333333" GridLines="None" EnableModelValidation="True" 
            OnPageIndexChanging="gvForm_PageIndexChanging"
            OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnRowDataBound="gvForm_RowDataBound" OnRowCreated="gvForm_RowCreated" OnSorting="gvForm_Sorting">
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
                 <asp:TemplateField HeaderText="Packaging Line" SortExpression="PackagingLineDesc" ItemStyle-Width="260">
                    <ItemTemplate>
                        <asp:Label ID="lblPackagingLineDesc" runat="server" Text='<%# Eval("PackagingLineDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="260px"></ItemStyle>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="QAT Definition" SortExpression="QATDefnDesc" ItemStyle-Width="300">
                    <ItemTemplate>
                        <asp:Label ID="lblQATDefnDesc" runat="server" Text='<%# Eval("QATDefnDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="Workflow Type" SortExpression="EntryPointDesc" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:Label ID="lblEntryPointDesc" runat="server" Text='<%# Eval("EntryPointDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px"></ItemStyle>
                </asp:TemplateField>

               <asp:TemplateField HeaderText="Test Seq" SortExpression="TestSeq" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblTestSeq" runat="server" Text='<%# Eval("TestSeq")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px"></ItemStyle>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="Serial Connection" SortExpression="SerialConnDesc" ItemStyle-Width="200">
                    <ItemTemplate>
                        <asp:Label ID="lblSerialConnDesc" runat="server" Text='<%# Eval("SerialConnDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="200px"></ItemStyle>
                </asp:TemplateField>
              <asp:TemplateField HeaderText="TCP Connection" SortExpression="TCPConnDesc" ItemStyle-Width="200">
                    <ItemTemplate>
                        <asp:Label ID="lblTCPConnDesc" runat="server" Text='<%# Eval("TCPConnDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="200px"></ItemStyle>
                </asp:TemplateField>
            <asp:TemplateField HeaderText="DefnID" SortExpression="QATDefnID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblQATDefnID" runat="server" Text='<%# Eval("QATDefnID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="SerialID" SortExpression="SerialConnID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblSerialConnID" runat="server" Text='<%# Eval("SerialConnID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
              <asp:TemplateField HeaderText="TCPID" SortExpression="TCPConnID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblTCPConnID" runat="server" Text='<%# Eval("TCPConnID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="Packaging Line" SortExpression="PackagingLine" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblPackagingLine" runat="server" Text='<%# Eval("PackagingLine")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:TemplateField>
            
                <asp:TemplateField HeaderText="Entry Point" SortExpression="QATEntryPoint" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:Label ID="lblQATEntryPoint" runat="server" Text='<%# Eval("QATEntryPoint")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Exception Code" SortExpression="ExceptionCode" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:Label ID="lblExceptionCode" runat="server" Text='<%# Eval("ExceptionCode")%>'></asp:Label>
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
                <td class="Labels"><asp:Label ID="lblActive" runat="server" Text="Active: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkActive1" runat="server" Checked='<%#Eval("Active")%>' Enabled="true"  /></td>
                <td></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblFacility" runat="server" Text="Facility: " Visible="false" CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Visible="false" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblFacility1" runat ="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblPackagingLine" runat="server" Text="Packaging Line: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlPackagingLine" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlPackagingLine_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>
                 <td class="Labels"><asp:Label ID="lblPackagingLine1" runat ="server" Text='<%# Eval("PackagingLine")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblQATDefnID" runat="server" Text="QAT Definition: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlQATDefnID" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlQATDefnID_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblQATDefnID1" runat ="server" Text='<%# Eval("QATDefnID")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblTestSeq" runat="server" Text="Test Seq: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlTestSeq" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlTestSeq_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblTestSeq1" runat ="server" Text='<%# Eval("TestSeq")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblSerialConnID" runat="server" Text="Serial Connection: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlSerialConnID" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlSerialConnID_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblSerialConnID1" runat ="server" Text='<%# Eval("SerialConnID")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblTCPConnID" runat="server" Text="TCP Connection: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlTCPConnID" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlTCPConnID_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>                   
                <td class="Labels"><asp:Label ID="lblTCPConnID1" runat ="server" Text='<%# Eval("TCPConnID")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblExceptionCode" runat="server" Text="Exception Code: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlExceptionCode" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlExceptionCode_SelectedIndexChanged" Width="400px" ></asp:DropDownList></td>                   
                <td class="Labels"><asp:Label ID="lblExceptionCode1" runat ="server" Text='<%# Eval("ExceptionCode")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblCopyRecord" runat="server" Text="Copy Packaging Line: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkCopyRecord1" runat="server" Checked='<%#Eval("CopyRecord")%>' Enabled="true"  OnCheckedChanged="chkCopyRecord1_CheckedChanged"   AutoPostBack="true" />Copy records to new Packaging Line</td>
                <td></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblPackagingLineFrom" runat="server" Text="Packaging Line (From): " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlPackagingLineFrom" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlPackagingLineFrom_SelectedIndexChanged"  Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblPackagingLineFrom1" runat ="server" Text='<%# Eval("PackagingLine")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblPackagingLineTo" runat="server" Text="Packaging Line (To): " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlPackagingLineTo" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlPackagingLineTo_SelectedIndexChanged"  Width="400px" ></asp:DropDownList></td>
                 <td class="Labels"><asp:Label ID="lblPackagingLineTo1" runat ="server" Text='<%# Eval("PackagingLine")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblDeleteRecord" runat="server" Text="Delete Packaging Line: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkDeleteRecord1" runat="server" Checked='<%#Eval("DeleteRecord")%>' Enabled="true"  OnCheckedChanged="chkDeleteRecord1_CheckedChanged"   AutoPostBack="true" />Delete records by selected Packaging Line</td>
                <td></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblPackagingLineDelete" runat="server" Text="Packaging Line: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlPackagingLineDelete" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlPackagingLineDelete_SelectedIndexChanged"  Width="400px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblPackagingLineDelete1" runat ="server" Text='<%# Eval("PackagingLine")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:label ID="lblQATDefnID9" runat="server" Text='<%# Eval("QATDefnID")%>' CssClass="Labels" Visible="false" ></asp:label>
                        <asp:label ID="lblPackagingLine9" runat="server" Text='<%# Eval("PackagingLine")%>' CssClass="Labels" Visible="false" ></asp:label>
                    <asp:label ID="lblFacility9" runat="server" Text='<%# Eval("Facility")%>' CssClass="Labels" Visible="false" ></asp:label>
                </td>
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
    </form>
</body>
</html>
