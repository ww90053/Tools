<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryIncomeTax4.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryIncomeTax4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        媒體申報電子檔產生
    </div>
    <table class="TableStyleBlue">
        <tr>
            <th>
                年度：
            </th>
            <td>
                <asp:TextBox ID="txt_year" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                發薪機關：
            </th>
            <td>
                <asp:DropDownList ID="DropDownList_AD" runat="server">
                </asp:DropDownList>
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
                <asp:Button ID="Button_PRINT" runat="server" OnClick="btToTXT_Click" Text="產生檔案" />
            </td>
        </tr>
    </table>
</asp:Content>
