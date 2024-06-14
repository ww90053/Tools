<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryIncomeTax5.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryIncomeTax5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        扣繳憑單產生
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                        <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" AutoPostBack="true" OnSelectedIndexChanged="payadChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        單位：
                    </th>
                    <td>
                        <asp:DropDownList ID="ddl_unit" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        身分證字號：
                    </th>
                    <td>
                        <asp:TextBox ID="tbID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        所得格式：
                    </th>
                    <td>
                        <asp:TextBox ID="txt_taxType" runat="server" Text="50" Width="30px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
