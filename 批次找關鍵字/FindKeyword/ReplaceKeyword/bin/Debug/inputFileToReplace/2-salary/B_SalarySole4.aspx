<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SalarySole4.aspx.cs" Inherits="TPPDDB._2_salary.B_SalarySole4" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table class="style1" border="1">
        <tr>
            <td colspan="2">
                單一發放自提儲蓄金清冊</td>
        </tr>
        <tr>
            <td class="style2">
                年份</td>
            <td class="style3">
                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="TextBox4_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="TextBox4">
                </cc1:CalendarExtender>
&nbsp;~
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="TextBox5_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="TextBox5">
                </cc1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="style2">
                局別</td>
            <td class="style3">
                <asp:DropDownList ID="DropDownList1" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style3" colspan="2">
                報表檔案名稱</td>
        </tr>
        <tr>
            <td class="style3" colspan="2">
                <asp:TextBox ID="TextBox3" runat="server" Width="80%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style3" colspan="2">
                <asp:CheckBox ID="CheckBox1" runat="server" Text="完成後顯示報表" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="Button4" runat="server" Text="產生Excel報表" />
                <asp:Button ID="Button5" runat="server" Text="顯示Excel報表" />
            </td>
        </tr>
    </table>
</asp:Content>
