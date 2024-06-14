<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="SQLHistory.aspx.cs" Inherits="TPPDDB._3_forleave.SQLHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Style23.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            border: 3px solid White;
            background-color: White;
            padding: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        操作記錄管理(差假/線上簽核)
    </div>
    <div style="width: 100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0" EnableScriptGlobalization="True">
        </asp:ScriptManager>
        <table class="TableStyleNone" width="80%">
            <tr id="tr_unit1" runat="server">
                <td class="style1">
                    機關
                </td>
                <td style="text-align: left;" class="style1">
                    <asp:DropDownList ID="DropDownList_unit1" DataSourceID="SqlDataSource_unit1" runat="server"
                        OnDataBound="DropDownList_unit1_DataBound" OnSelectedIndexChanged="DropDownList_unit1_SelectedIndexChanged"
                        DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource_unit1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                        DataSourceMode="DataReader"></asp:SqlDataSource>
                </td>
            </tr>
            <tr id="tr_unit2">
                <td>
                    單位
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="DropDownList_unit2" runat="server" OnDataBound="DropDownList_unit2_DataBound">
                        <asp:ListItem>請先選擇機關</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource_unit2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                        DataSourceMode="DataReader"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    日期區間
                </td>
                <td style="text-align: left;">
                    起：<asp:TextBox ID="tbStart" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="tbStart" CssClass="cssCalendar">
                    </cc1:CalendarExtender>
                    &nbsp;~ 迄：<asp:TextBox ID="tbEnd" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="tbEnd" CssClass="cssCalendar">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    類型
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="ddlType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    關鍵字
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="tbSQL" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div>
        </div>
        <div style="text-align: center;">
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="匯出" />
            &nbsp;
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查詢" />
            <br />
        </div>
        <div>
            <br />
            <asp:GridView ID="gvData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                Width="95%" AutoGenerateColumns="False" EmptyDataText="該條件下無資料" OnRowCommand="gvData_RowCommand"
                DataKeyNames="SQLTEXT">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="SN" HeaderText="編號" />
                    <asp:BoundField DataField="MEMO" HeaderText="動作" />
                    <asp:BoundField DataField="ADDDATE" HeaderText="時間" />
                    <asp:BoundField DataField="MZ_NAME" HeaderText="新增人" />
                    <asp:BoundField DataField="ADNAME" HeaderText="機關" />
                    <asp:BoundField DataField="UNITNAME" HeaderText="單位" />
                    <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="指令" Text="查看" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server" Width="400px" CssClass="modalPopup" Style="display: none;">
            <div style="background-color: #0000FF; font-size: medium; font-weight: bold; color: #FFFFFF">
                指令
            </div>
            <div style="text-align: left; word-break: break-all;">
                <br />
                <asp:Label ID="lbSQL" runat="server" Text=""></asp:Label>
            </div>
            <div>
                <br />
                <asp:Button ID="btnLeave" runat="server" Text="關閉" /></div>
        </asp:Panel>
        <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" />
        <cc1:ModalPopupExtender ID="Button1_ModalPopupExtender" runat="server" DynamicServicePath=""
            CancelControlID="btnLeave" Enabled="True" TargetControlID="Button1" PopupControlID="Panel1"
            BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
    </div>
</asp:Content>
