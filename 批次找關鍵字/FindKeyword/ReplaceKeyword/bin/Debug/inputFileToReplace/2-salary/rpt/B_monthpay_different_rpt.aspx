<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_monthpay_different_rpt.aspx.cs" Inherits="TPPDDB._2_salary.rpt.B_monthpay_different_rpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<link href="../style/Master.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
        <asp:Label ID="lb_Title" runat="server" Text="Label"></asp:Label>
    </div>
    <table class="TableStyleBlue" style="width: 90%; margin-top: 10px;">
        <tr>
            <th>
                發薪機關：
            </th>
            <td>
                <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                    DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" > 
                </asp:DropDownList>
            </td>
        </tr>
       <tr id="Tr_Dtype" runat="server" visible="false">
            <th>
                差異種類：
            </th>
            <td>
                <asp:DropDownList ID="ddl_Dtype" runat="server">
                <asp:ListItem Text="全部" Value="1"></asp:ListItem>
                <asp:ListItem Text="健保費" Value="2"></asp:ListItem>
                <asp:ListItem Text="退撫金" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                當前年月：
            </th>
            <td>
                <asp:TextBox ID="TextBox_YEAR" runat="server" MaxLength="5" Width="60px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender4" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                </cc1:FilteredTextBoxExtender>
                &nbsp;年 (民國年，如：09801)
            </td>
        </tr>
        <tr>
            <th>
                比較年月：
            </th>
            <td>
                <asp:TextBox ID="TextBox_YEAR2" runat="server" MaxLength="5" Width="60px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_YEAR2_FilteredTextBoxExtender4" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR2">
                </cc1:FilteredTextBoxExtender>
                &nbsp;年 (民國年，如：09801)
            </td>
        </tr>
       
        <tr>
            <td colspan="2" style="text-align: center;">
                
                <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="預覽" />
                <asp:Button ID="btn_Export" runat="server" Text="匯出Excel" 
                    onclick="btn_Export_Click" />
                
             
            </td>
        </tr>
    </table>
</asp:Content>

