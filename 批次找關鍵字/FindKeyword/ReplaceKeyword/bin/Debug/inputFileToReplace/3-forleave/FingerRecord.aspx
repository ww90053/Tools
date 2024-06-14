<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="FingerRecord.aspx.cs" Inherits="TPPDDB._3_forleave.FingerRecord" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 174px;
        }
        .style3
        {
            width: 435px;
        }
        .modalPopup 
        { 
            border: 3px solid White;
            background-color:White; 
            padding: 3px; 
        } 
        .style10
        {
            margin-bottom: 0px;
        }
        .style11
        {
            font-family: 標楷體;
            font-weight: bold;
            font-size: 16pt;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <span class="style11">指紋打卡記錄</span><asp:ScriptManager ID="ScriptManager1" runat="server" 
            EnableScriptGlobalization="True">
        </asp:ScriptManager>
                    <br />
        <table class="style1">
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td class="style3">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <cc1:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="TextBox1">
        </cc1:CalendarExtender>
                至<asp:TextBox ID="TextBox_DateEnd" runat="server"></asp:TextBox>
        <cc1:CalendarExtender ID="TextBox_DateEnd_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="TextBox_DateEnd">
        </cc1:CalendarExtender>
                &nbsp;<asp:Button ID="btTable" runat="server" onclick="btTable_Click" Text="查詢" 
                        Height="21px" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td class="style3">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            </table>
        <table class="style1">
            <tr>
                <td>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
        CellPadding="3" CssClass="style10" DataSourceID="SqlDataSource1" 
        GridLines="Vertical" Width="100%">
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <Columns>
           
            <asp:BoundField DataField="TERMINALNAME" HeaderText="卡機編號"  SortExpression="TERMINALNAME" />
            <asp:BoundField DataField="USERID" HeaderText="卡號"   SortExpression="USERID" />
            <asp:BoundField DataField="USERNAME" HeaderText="姓名" SortExpression="USERNAME" />
            <asp:BoundField DataField="F_LOGDATE" HeaderText="日期" SortExpression="F_LOGDATE" />
            <asp:BoundField DataField="LOGTIME" HeaderText="時間" SortExpression="LOGTIME" />
            <asp:BoundField DataField="VERIFY" HeaderText="進/出" SortExpression="VERIFY" />
            <asp:BoundField DataField="FKEY" HeaderText="狀況" SortExpression="FKEY" />
           
        </Columns>
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#DCDCDC" />
    </asp:GridView>
                </td>
            </tr>
        </table>
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                
        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                
    </p>
    <p>
        &nbsp;</p>
</asp:Content>
