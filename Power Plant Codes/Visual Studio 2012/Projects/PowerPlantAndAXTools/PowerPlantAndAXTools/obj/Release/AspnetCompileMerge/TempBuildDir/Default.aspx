<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="PowerPlantAndAXTools._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 358px;
        }
        .auto-style2
        {
            width: 250px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="828px" Height="500px" style="margin-right: 108px">
            <asp:TabPanel runat="server" HeaderText="Pallets" ID="TabPanel1">
                <HeaderTemplate>Change Line on IPC</HeaderTemplate>
                <ContentTemplate>
                      <table style="width:100%;">
                        <tr>
                            <td class="auto-style1">
                                <asp:Label ID="Label4" runat="server" Text="Change to line: "></asp:Label>
                                <asp:DropDownList ID="ddlLine" runat="server" DataSourceID="dsEquipment" DataTextField="Description" DataValueField="EquipmentID" Height="25px" Width="235px">
                                </asp:DropDownList>&nbsp;&nbsp;
                                 
                            </td>
                            <td class="auto-style2">
                                <asp:Label ID="lblDefaultIPC" runat="server" Text="For IPC:"></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlComputerName" runat="server" DataSourceID="dsComputerConfig" DataTextField="ComputerName" DataValueField="ComputerName" Height="25px" Width="129px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;<asp:Button ID="btnLineAccept" runat="server" Text="Accept" /></td>
                        </tr>
                    </table>
                                      <asp:SqlDataSource ID="dsEquipment" runat="server" ConnectionString="<%$ ConnectionStrings:cnnStrPowerPlantAX_UA %>" SelectCommand="PPsp_Equipment_Sel" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="ListByType" Name="vchAction" Type="String" />
                        <asp:SessionParameter DefaultValue="" Name="chrFacility" SessionField="Facility" Type="String" />
                        <asp:Parameter DefaultValue="P" Name="chrType" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                      <asp:SqlDataSource ID="dsComputerConfig" runat="server" ConnectionString="<%$ ConnectionStrings:cnnStrPowerPlantAX_UA %>" SelectCommand="CPPsp_ComputerConfigIO" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="SelectAllFields" Name="chrAction" Type="String" />
                                        <asp:Parameter DefaultValue="" Name="chrComputerName" Type="String" />
                                        <asp:Parameter Name="vchMachineID" Type="String" DefaultValue="" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                 </ContentTemplate>
             </asp:TabPanel>
 <asp:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
            <HeaderTemplate>Change Pallet Print Status</HeaderTemplate>
            <ContentTemplate><asp:UpdatePanel ID="UpdatePanel2" runat="server">
              <ContentTemplate>
 
              <table style="width: 100%;"><tr><td><asp:GridView ID="gvPallet" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="PalletID" DataSourceID="dsPallet" 
                        EmptyDataText="No data!" AllowPaging="True" AllowSorting="True" 
                        CellPadding="4" ForeColor="#333333" GridLines="None" EnableModelValidation="True">
                  <RowStyle BackColor="#F7F6F3" ForeColor="#333333" VerticalAlign="Top" /><Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="PostPallet" Text="Post" />
                            <asp:BoundField DataField="PalletID" HeaderText="Pallet ID" SortExpression="PalletID" ReadOnly="True" />
                            <asp:BoundField DataField="ItemNumber" HeaderText="Item" SortExpression="ItemNumber" ReadOnly="True" />
                            <asp:BoundField DataField="ShopOrder" HeaderText="Shop Order" SortExpression="ShopOrder" ReadOnly="True" />
                            <asp:BoundField DataField="CreationDateTime" HeaderText="Created at" ReadOnly="True" SortExpression="CreationDateTime" />
                            <asp:BoundField DataField="DefaultPkgLine" HeaderText="Pkg Line" ReadOnly="True" SortExpression="DefaultPkgLine" />
                            <asp:BoundField DataField="Quantity" HeaderText="Qty" ReadOnly="True" SortExpression="Quantity" />
                            <asp:BoundField DataField="QtyPerPallet" HeaderText="Qty/Pallet" ReadOnly="True" SortExpression="QtyPerPallet" />
                  </Columns>
                  <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" /><HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  VerticalAlign="Top"/><EditRowStyle BackColor="#999999" /><AlternatingRowStyle BackColor="White" ForeColor="#284775" /></asp:GridView></td>
                  <td valign="top" align="left">&nbsp;</td></table>
                  <asp:Label ID="lblErrMsg2" runat="server" ForeColor="Red"> </asp:Label>
                </ContentTemplate>
                </asp:UpdatePanel> 
                 </ContentTemplate>
             </asp:TabPanel>
 </asp:TabContainer>
    <asp:SqlDataSource ID="dsPallet" runat="server" CancelSelectOnNullParameter="False"
        ConnectionString="<%$ ConnectionStrings:cnnStrPowerPlantAX_UA %>" 
        SelectCommand="SELECT RRN, Facility, PalletID, QtyPerPallet, Quantity, ItemNumber, DefaultPkgLine, Operator, CreationDate, CreationTime, OrderComplete, LotID, ShopOrder, StartTime, ProductionDate, ExpiryDate, PrintStatus, CreationDateTime, ShiftProductionDate, ShiftNo, LastUpdate FROM tblPallet WHERE (Facility = @vchFacility) AND (PrintStatus &lt;&gt; 2) order by rrn desc" 
        UpdateCommand="update tblPallet set PrintStatus = 2 WHERE Palletid = @intPalletID and PrintStatus &lt;&gt; 2" >
        <SelectParameters>
            <asp:SessionParameter Name="vchFacility" SessionField="Facility" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="intPalletID" DbType="Int32" />
        </UpdateParameters>
   </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
