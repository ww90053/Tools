<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_savelist_rpt.aspx.cs" Inherits="TPPDDB._2_salary.B_savelist_rpt" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
        職員優惠存款名冊</div>
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
                        <asp:DropDownList ID="DropDownList_AD" runat="server" 
                            AppendDataBoundItems="True"
                            DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="text-align: right">
                        年份：
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" 
                            Width="85px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender4" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="TextBox_YEAR">
                        </cc1:FilteredTextBoxExtender>
                        &nbsp;年 (民國年，如：098)
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="text-align: right">
                        月份：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="DropDownList_MONTH" runat="server" Style="margin-left: 3px">
                            <asp:ListItem>請選擇</asp:ListItem>
                            <asp:ListItem Value="1">一月</asp:ListItem>
                            <asp:ListItem Value="2">二月</asp:ListItem>
                            <asp:ListItem Value="3">三月</asp:ListItem>
                            <asp:ListItem Value="4">四月</asp:ListItem>
                            <asp:ListItem Value="5">五月</asp:ListItem>
                            <asp:ListItem Value="6">六月</asp:ListItem>
                            <asp:ListItem Value="7">七月</asp:ListItem>
                            <asp:ListItem Value="8">八月</asp:ListItem>
                            <asp:ListItem Value="9">九月</asp:ListItem>
                            <asp:ListItem Value="10">十月</asp:ListItem>
                            <asp:ListItem Value="11">十一月</asp:ListItem>
                            <asp:ListItem Value="12">十二月</asp:ListItem>
                        </asp:DropDownList>
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
                        <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="預覽" />
                        &nbsp;<asp:Button ID="Button_CANCEL" runat="server" Text="取消" OnClick="Button_CANCEL_Click" />
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>