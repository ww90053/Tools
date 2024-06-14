<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_InsuranceStatistics_rpt_2.aspx.cs" Inherits="TPPDDB._2_salary.B_InsuranceStatistics_rpt_2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: right;
        }
        .style2
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        保費統計表</div>
    <div>
    </div>
    <asp:Panel ID="Panel1" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
    <div>
    <table class="style2">
    <tr><td colspan="7">
    </td></tr>
        <tr>
            <td class="style1">
                發薪機關：</td>
            <td style="text-align: left">
                <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" 
                    DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style1">
                年月份：</td>
            <td style="text-align: left">
                <asp:TextBox ID="TextBox_AMONTH" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <asp:Label ID="Label_MSG" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <asp:Button ID="Button_Send" runat="server" Text="查詢" 
                    onclick="Button_Send_Click" style="height: 21px" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
                </asp:Content>
