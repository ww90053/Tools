<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_yearendbonuslist_rpt.aspx.cs" Inherits="TPPDDB._2_salary.B_yearendbonuslist_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        年終獎金印領清冊
    </div>
    <div>
        <table class="TableStyleBlue" style="margin-top: 10px;">
            <tr>
                <th>
                    發薪機關：
                </th>
                <td>
                    <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                        DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    年份：
                </th>
                <td>
                    <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" Width="85px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender4" runat="server"
                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                    </cc1:FilteredTextBoxExtender>
                    &nbsp;年 (民國年，如：098)
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="預覽" />
                    &nbsp;<asp:Button ID="Button_CANCEL" runat="server" Text="取消" OnClick="Button_CANCEL_Click" />
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
