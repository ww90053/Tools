<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SearchMonthSalary3.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchMonthSalary3" %>
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
    <asp:Label ID="Label_TITLE" runat="server"></asp:Label>
    </div>
    <asp:Panel ID="Panel1" runat="server" Width="417px" 
        GroupingText="在下列條件選擇，欲產生之報表">
        <table class="style1">
            <tr>
                <td style="text-align: right" class="style2">
                </td>
                <td style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    <asp:Label ID="Label_AD" runat="server" Text="發薪機關："></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" 
                        DataTextField="MZ_KCHI" 
                        DataValueField="MZ_KCODE">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    <asp:Label ID="Label_POLNO" runat="server" Text="員工編號："></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBox_MZ_POLNO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    <asp:Label ID="Label_ID" runat="server" Text="身分證號："></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBox_MZ_ID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    <asp:Label ID="Label_Name" runat="server" Text="姓名："></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    年份：</td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" 
                        Width="85px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender7" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="TextBox_YEAR">
                    </cc1:FilteredTextBoxExtender>
                    &nbsp;年 (民國年，如：098)</td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    月份：</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_MONTH" runat="server" 
                        style="margin-left: 3px">
                        <asp:ListItem Value="-1">請選擇</asp:ListItem>
                        <asp:ListItem Value="01">一月</asp:ListItem>
                        <asp:ListItem Value="02">二月</asp:ListItem>
                        <asp:ListItem Value="03">三月</asp:ListItem>
                        <asp:ListItem Value="04">四月</asp:ListItem>
                        <asp:ListItem Value="05">五月</asp:ListItem>
                        <asp:ListItem Value="06">六月</asp:ListItem>
                        <asp:ListItem Value="07">七月</asp:ListItem>
                        <asp:ListItem Value="08">八月</asp:ListItem>
                        <asp:ListItem Value="09">九月</asp:ListItem>
                        <asp:ListItem Value="10">十月</asp:ListItem>
                        <asp:ListItem Value="11">十一月</asp:ListItem>
                        <asp:ListItem Value="12">十二月</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: left">
                    <asp:Label ID="Label_MSG" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button_SEARCH" runat="server" onclick="Button_SEARCH_Click" 
                        Text="查詢" />
                    &nbsp;<asp:Button ID="Button_CANCEL" runat="server" onclick="Button_CANCEL_Click" 
                        Text="取消" />
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
