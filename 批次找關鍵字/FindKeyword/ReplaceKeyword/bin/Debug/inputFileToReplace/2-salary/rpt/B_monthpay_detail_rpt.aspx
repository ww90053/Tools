<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_monthpay_detail_rpt.aspx.cs" Inherits="TPPDDB._2_salary.rpt.B_monthpay_detail_rpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
         <asp:Label ID="lb_Title" runat="server" Text=""></asp:Label>
    </div>
    <table class="TableStyleBlue" style="margin-top: 10px;">
        <tr>
            <th>
                發薪機關：
            </th>
            <td>
                <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                    <asp:ListItem>請選擇</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                編制單位：
            </th>
            <td>
                <asp:DropDownList ID="DropDownList_UNIT" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                年份：
            </th>
            <td>
                <asp:TextBox ID="TextBox_YEAR" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender3" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                </cc1:FilteredTextBoxExtender>
                &nbsp;年 (民國年，如：098)
            </td>
        </tr>
        <tr>
            <th>
                月份：
            </th>
            <td>
                <asp:DropDownList ID="DropDownList_MONTH" runat="server">
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
        
         <tr id="tr_batch" runat ="server"  visible="false" >
                <th>
                    批號：
                </th>
                <td>
                    <asp:TextBox ID="txt_batch" runat="server" Width="30px"></asp:TextBox>
                </td>
            </tr>
        
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="預覽" />
                <asp:Button ID="btn_Export" runat="server" Text="匯出excel" OnClick="btn_Export_Click" />
                <asp:Button ID="Button_CANCEL" runat="server" Text="取消" OnClick="Button_CANCEL_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

