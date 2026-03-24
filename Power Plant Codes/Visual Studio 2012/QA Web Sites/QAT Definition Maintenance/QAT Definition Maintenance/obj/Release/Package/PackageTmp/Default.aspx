<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QAT_Definition_Maintenance._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title>QAT Definition Maintenance</title>
    <link href="App_Themes/PP/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
      <div id="divHeader" class ="DivLeft">
      <div id="divLogo">
        <table>
            <tr>
                <td><asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/PP/Images/company_logo.jpg" /></td>
                <td><h1>QAT Definition Maintenance (User: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>)</h1></td>
              </tr>
        </table>    
     </div>
     <div id="divSearch">
        <table>
        <tr>
                <td class="Labels"><asp:Label ID= "SearchLabel" Text="Description: " Runat="Server" CssClass="Labels" /></td>
                <td class="Labels"><asp:TextBox ID="txtSearch" runat="server" Width ="402px" MaxLength="100" ></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search"  OnClick="btnSearch_Click" CssClass="Buttons"  /></td>
            </tr>
        </table>    
    </div>
    </div>
    <div id="divGrid">
        <asp:GridView ID="gvForm" runat="server"  AutoGenerateColumns="False" CssClass ="GridView" AllowPaging="True" PageSize="18" AllowSorting="True" 
            DataKeyNames="QATDefnID"  style="margin-top: 12px" CellPadding="4" width="100%"
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
                <asp:TemplateField HeaderText="Description" SortExpression="QATDefnDescription" ItemStyle-Width="220" >
                    <ItemTemplate>
                        <asp:Label ID="lblQATDefnDescription" runat="server" Text='<%# Eval("QATDefnDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="220px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Alert" SortExpression="Alert" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbAlert" runat="server" Checked='<%#Eval("Alert")%>' Enabled="false"  />
                    </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>          
                <asp:TemplateField HeaderText="Allow Override" SortExpression="AllowOverride" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbAllowOverride" runat="server" Checked='<%#Eval("AllowOverride")%>' Enabled="false"  />
                    </ItemTemplate>
                <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
              <asp:TemplateField HeaderText="Entry Point" SortExpression="EntryPointDescription" ItemStyle-Width="70">
                    <ItemTemplate>
                        <asp:Label ID="lblEntryPointDesc" runat="server" Text='<%# Eval("EntryPointDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="70px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="InProc Freq Type" SortExpression="FreqTypeDescription" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblInProcFreqTypeDesc" runat="server" Text='<%# Eval("FreqTypeDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:TemplateField>
             
                 <asp:TemplateField HeaderText="InProc No Of Freq" SortExpression="InProcNoOfFreq" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblInProcNoOfFreq" runat="server" Text='<%# Eval("InProcNoOfFreq")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="No Of Lanes" SortExpression="NoOfLanes" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblNoOfLanes" runat="server" Text='<%# Eval("NoOfLanes")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="No Of Samples" SortExpression="NoOfSamples" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblNoOfSamples" runat="server" Text='<%# Eval("NoOfSamples")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Expiry Count" SortExpression="ExpiryCount" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblExpiryCount" runat="server" Text='<%# Eval("ExpiryCount")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Test Specification" SortExpression="TestSpecDesc" ItemStyle-Width="200">
                    <ItemTemplate>
                        <asp:Label ID="lblTestSpecDesc" runat="server" Text='<%# Eval("TestSpecDesc")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="200px"></ItemStyle>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="Test Category" SortExpression="TestCategory" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblTestCategory" runat="server" Text='<%# Eval("TestCategory")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="Test Form Title" SortExpression="TestFormTitle" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblTestFormTitle" runat="server" Text='<%# Eval("TestFormTitle")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="Note" SortExpression="NoteDescription" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblNoteDescription" runat="server" Text='<%# Eval("NoteDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="DefnID" SortExpression="QATDefnID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblQATDefnID" runat="server" Text='<%# Eval("QATDefnID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QAT Entry Point" SortExpression="QATEntryPoint" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblQATEntryPoint" runat="server" Text='<%# Eval("QATEntryPoint")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Test Spec ID" SortExpression="TestSpecID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblTestSpecID" runat="server" Text='<%# Eval("TestSpecID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="Test Form ID" SortExpression="TestFormID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblTestFormID" runat="server" Text='<%# Eval("TestFormID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>  
               <asp:TemplateField HeaderText="Note ID" SortExpression="NoteID" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblNoteID" runat="server" Text='<%# Eval("NoteID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="InProc Freq Type" SortExpression="InProcFreqType" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblInProcFreqType" runat="server" Text='<%# Eval("InProcFreqType")%>'></asp:Label>
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
                <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblQATDefnDescription5" runat="server" Text="Description: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:TextBox ID="txtQATDefnDescription1" runat ="server" Text='<%# Eval("Description")%>' Width ="400px" Height ="40px" TextMode="MultiLine" MaxLength="100" ></asp:TextBox></td>
                <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter Description"  ControlToValidate="txtQATDefnDescription1" ValidationGroup="vgpUpdate" ForeColor="#C00000" /></td>
           </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblAlert5" runat="server" Text="Alert: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkAlert1" runat="server" Checked='<%#Eval("Alert")%>' Enabled="true"  /></td>
                <td></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblAllowOverride5" runat="server" Text="Allow Override: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkAllowOverride1" runat="server" Checked='<%#Eval("AllowOverride")%>' Enabled="true"  /></td>
                <td></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblEntryPointDesc5" runat="server" Text="Entry Point: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlEntryPointDesc" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlEntryPointDesc_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblEntryPointDesc1" runat ="server" Text='<%# Eval("EntryPointDesc")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
             <tr>
                <td class="Labels"><asp:Label ID="lblInProcFreqType5" runat="server" Text="In Process Freq Type: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlInProcFreqType" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlInProcFreqType_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblInProcFreqType1" runat ="server" Text='<%# Eval("FreqTypeDescription")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>           
            <tr>
                <td class="Labels"><asp:Label ID="lblInProcNoOfFreq5" runat="server" Text="In Process No Of Freq: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:TextBox ID="txtInProcNoOfFreq1" runat ="server" Text='<%# Eval("InProcNoOfFreq")%>' Width ="402px" MaxLength="10" ></asp:TextBox></td>
                <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Enter In Process No Of Freq - positive numbers only"  ControlToValidate="txtInProcNoOfFreq1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic" ErrorMessage="Allow positive numbers only."
                        ValidationExpression="^[+]?\d+" ValidationGroup="vgpAdd" ControlToValidate="txtInProcNoOfFreq1" ForeColor="#C00000"></asp:RegularExpressionValidator></td>
             </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblNoOfLanes5" runat="server" Text="No Of Lanes: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlNoOfLanes" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlNoOfLanes_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblNoOfLanes1" runat ="server" Text='<%# Eval("NoOfLanes")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblNoOfSamples5" runat="server" Text="No Of Samples: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlNoOfSamples" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlNoOfSamples_SelectedIndexChanged"  Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblNoOfSamples1" runat ="server" Text='<%# Eval("NoOfSamples")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblExpiryCount5" runat="server" Text="Expiry Count: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:TextBox ID="txtExpiryCount1" runat ="server" Text='<%# Eval("ExpiryCount")%>' Width ="402px" MaxLength="10" ></asp:TextBox></td>
                <td class="Labels"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="Enter Expiry Count - positive numbers only"  ControlToValidate="txtExpiryCount1" ValidationGroup="vgpUpdate" ForeColor="#C00000" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Allow positive numbers only."
                        ValidationExpression="^[+]?\d+" ValidationGroup="vgpAdd" ControlToValidate="txtExpiryCount1" ForeColor="#C00000"></asp:RegularExpressionValidator></td>
             </tr>

         <tr>
                <td class="Labels"><asp:Label ID="lblTestSpecDesc5" runat="server" Text="Test Specification: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlTestSpecDesc" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlTestSpecDesc_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblTestSpecDesc1" runat ="server" Text='<%# Eval("TestSpecDesc")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
             <tr>
                <td class="Labels"><asp:Label ID="lblTestCategory5" runat="server" Text="Test Category: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlTestCategory" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlTestCategory_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblTestCategory1" runat ="server" Text='<%# Eval("TestCategory")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels" style="vertical-align:text-top"><asp:Label ID="lblTestFormTitle5" runat="server" Text="Test Form Title: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:TextBox ID="txtTestFormTitle1" runat ="server" Text='<%# Eval("TestFormTitle")%>' Width ="402px"  MaxLength="25" ></asp:TextBox></td>
             </tr>
              <tr>
                <td class="Labels"><asp:Label ID="lblNoteDesc5" runat="server" Text="Note: " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:DropDownList ID="ddlNoteDesc" runat="server" AutoPostBack="false" Visible="true" OnSelectedIndexChanged="ddlNoteDesc_SelectedIndexChanged" Width="406px" ></asp:DropDownList></td>
                <td class="Labels"><asp:Label ID="lblNoteDesc1" runat ="server" Text='<%# Eval("NoteDesc")%>' CssClass="Labels" Visible="false" ></asp:Label></td>
            </tr>
            <tr>
                <td class="Labels"><asp:Label ID="lblCopyRecord5" runat="server" Text="Copy Record? " CssClass="Labels"></asp:Label></td>
                <td class="Labels"><asp:CheckBox ID="chkCopyRecord1" runat="server" Checked='<%#Eval("CopyRecord")%>' Enabled="true"  /></td>
                <td></td>
            </tr>
             <tr>
                <td class="Labels"><asp:label ID="lblQATDefnID9" runat="server" Text='<%# Eval("QATDefnID")%>' CssClass="Labels" Visible="false" ></asp:label></td>
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
