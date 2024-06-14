<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_taxdetail_rpt.aspx.cs" Inherits="TPPDDB._2_salary.B_taxdetail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        員工各項稅額清冊
    </div>
    <div>
        <table class="TableStyleBlue" style="margin-top: 10px;">
            <tr>
                <th>
                    發薪機關：
                </th>
                <td>
                    <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    編制單位：
                </th>
                <td>
                    <asp:DropDownList ID="DropDownList_UNIT" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    年份：
                </th>
                <td>
                    <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" Width="85px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender3" runat="server"
                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                    </cc1:FilteredTextBoxExtender>
                    &nbsp;年 (民國年，如：098)
                </td>
            </tr>
            <tr>
                <th>
                    所得格式：
                </th>
                <td>
                   <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="50">50格式</asp:ListItem>
                    <asp:ListItem>其他格式</asp:ListItem>
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                </asp:RadioButtonList>
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
