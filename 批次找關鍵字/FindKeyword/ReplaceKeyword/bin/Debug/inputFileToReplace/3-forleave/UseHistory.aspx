<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="UseHistory.aspx.cs" Inherits="TPPDDB._3_forleave.UseHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Style23.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
        .style2
        {
            height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        使用狀況匯出(差假/線上簽核)
    </div>
    <div style="width: 100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0" EnableScriptGlobalization="true">
        </asp:ScriptManager>
        <table class="TableStyleNone" width="80%">
            <tr id="tr_group" runat="server">
                <td class="style2">
                    群組類別
                </td>
                <td style="text-align: left;" class="style2">
                    <asp:RadioButton ID="RadioButton_g1" runat="server" Text="依照分局群組" GroupName="gruoptype"
                        OnCheckedChanged="RadioButton_g1_CheckedChanged" AutoPostBack="True" Checked="True" />
                    <asp:RadioButton ID="RadioButton_g2" runat="server" Text="依照派出所群組" GroupName="gruoptype"
                        OnCheckedChanged="RadioButton_g2_CheckedChanged" AutoPostBack="True" />
                </td>
            </tr>
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
            <tr id="tr_unit2" visible="false" runat="server">
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
                    日期
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                    年<asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem>請選擇</asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                    月
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
        </table>
        <div>
        </div>
        <div style="text-align: center;">
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="匯出" />
            &nbsp;
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查詢" />
        </div>
        <div>
            <asp:GridView ID="gvData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                Width="95%">
                <RowStyle BackColor="#EFF3FB" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
