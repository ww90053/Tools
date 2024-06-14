<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SearchYearSalary3.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchYearSalary3" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="title_s1">
        <asp:Label ID="Label_TITLE" runat="server"></asp:Label>
    </div>
    <asp:Panel ID="Panel1" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
        <table class="style1">
            <tr>
                <td style="text-align: right" class="style2">
                </td>
                <td style="text-align: left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    <asp:Label ID="Label_AD" runat="server" Text="發薪機關："></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCHI"
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
                    年份：
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" Width="50px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender7" runat="server"
                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                    </cc1:FilteredTextBoxExtender>
                    年 (民國年，如：098)
                </td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                </td>
                <td style="text-align: left">
                    <asp:Label ID="Label_MSG" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="查詢" />
                    <asp:Button ID="Button_CANCEL" runat="server" OnClick="Button_CANCEL_Click" Text="取消" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
