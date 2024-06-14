<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryIncomeTax1.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryIncomeTax1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        所得稅設定
    </div>
    <table class="TableStyleBlue" width="400px">
        <tr>
            <th style="width: 160px;">
                所得稅比例
            </th>
            <td>
                <asp:TextBox ID="txt_taxpercent" runat="server" Width="50px" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                需扣所得稅起始金額(不含)
            </th>
            <td>
                <asp:TextBox ID="txt_taxstart" runat="server" Width="50px" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="確定" />
                <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
