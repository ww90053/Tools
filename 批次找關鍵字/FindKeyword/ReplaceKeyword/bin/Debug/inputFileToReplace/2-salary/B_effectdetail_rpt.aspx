﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_effectdetail_rpt.aspx.cs" Inherits="TPPDDB._2_salary.B_effectdetail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 158px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        考績獎金印領清冊-明細表</div>
    <div>
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
            <table class="style1">
                <tr>
                    <td style="text-align: right" class="style2">
                        &nbsp;
                    </td>
                    <td style="text-align: left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="text-align: right">
                        發薪機關：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                            AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                            <asp:ListItem>請選擇</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="text-align: right">
                        編制單位：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="DropDownList_UNIT" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="text-align: right">
                        年份：
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" Width="85px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender2" runat="server"
                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                        </cc1:FilteredTextBoxExtender>
                        &nbsp;年 (民國年，如：098)
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="text-align: right">
                        &nbsp;
                    </td>
                    <td style="text-align: left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p>
                            <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="預覽" />
                            <asp:Button ID="btn_Export" runat="server" Text="匯出excel" OnClick="btn_Export_Click" />
                            <asp:Button ID="Button_CANCEL" runat="server" OnClick="Button_CANCEL_Click" Text="取消" />
                        </p>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
