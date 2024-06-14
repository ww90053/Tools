<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_InsuranceDetail.aspx.cs" Inherits="TPPDDB._2_salary.B_InsuranceDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">

        .style1
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
    <div style="width:417;">
    <table style="width:417;">
        <tr>
            <td class="style1">
                發薪機關</td>
            <td style="text-align: left">
                <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" 
                    DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style1" style="width:150px">
                年月份</td>
            <td style="text-align: left">
                <asp:TextBox ID="TextBox_AMONTH" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="Button_Send0" runat="server" Text="送出" 
                    onclick="Button_Send_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label_MSG" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </asp:Content>
