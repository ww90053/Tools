<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SalarySole3.aspx.cs" Inherits="TPPDDB._2_salary.B_SalarySole3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: right;
        }
        .style3
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="style1" border="1">
        <tr>
            <td colspan="2">
                單一發放個人報表</td>
        </tr>
        <tr>
            <td class="style2">
                日期</td>
            <td class="style3">
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                課室</td>
            <td class="style3">
                <asp:DropDownList ID="DropDownList2" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style2">
                局別</td>
            <td class="style3">
                <asp:DropDownList ID="DropDownList3" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="2">
                <asp:Panel ID="Panel1" runat="server" GroupingText="列印範圍(以員工編號為值)">
                    <table class="style1">
                        <tr>
                            <td rowspan="2" style="width: 10%">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                    <asp:ListItem>區間</asp:ListItem>
                                    <asp:ListItem>單一筆</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td class="style3">
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="Button1" runat="server" Text="列印" />
                <asp:Button ID="Button2" runat="server" Text="套表列印" />
            </td>
        </tr>
    </table>
</asp:Content>
