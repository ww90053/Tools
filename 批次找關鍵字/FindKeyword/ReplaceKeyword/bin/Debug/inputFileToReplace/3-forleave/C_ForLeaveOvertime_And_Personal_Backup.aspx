<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_And_Personal_Backup.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_And_Personal_Backup" Title="未命名頁面" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server">
        
    <table class="style10">
        <tr>
            <td class="style8">
                年度資料備份
            </td>
        </tr>
    </table>
    <table  class="style6">
    <tr>
    <td class="style16">
    人事基本資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_DLBASE" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btDLBASE_BACKUP" runat="server" Text="確定" onclick="btDLBASE_BACKUP_Click" class="style9" />
    </td>
    </tr>
    <tr>
    <td class="style16">
    獎懲基本資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_PRKB" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btPRKB_BACKUP" runat="server" Text="確定" onclick="btPRKB_BACKUP_Click" class="style9"/>
    </td>
    </tr>
    <tr>
    <td class="style16">
    獎懲發布主檔資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_PRK1" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btPRK1_BACKUP" runat="server" Text="確定" onclick="btPRK1_BACKUP_Click" class="style9" />
    </td>
    </tr>
    <tr>
    <td class="style16">
    獎懲令修改，審核，統計資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_PRK2" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btPRK2_BACKUP" runat="server" Text="確定" onclick="btPRK2_BACKUP_Click"  class="style9"/>
    </td>
    </tr>
    <tr>
    <td class="style16">
    任免基本資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_POSIT" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btPOSIT_BACKUP" runat="server" Text="確定" onclick="btPOSIT_BACKUP_Click" class="style9" />
    </td>
    </tr>
    <tr>
    <td class="style16">
    任免發布主檔資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_POSIT1" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btPOSIT1_BACKUP" runat="server" Text="確定" onclick="btPOSIT1_BACKUP_Click" class="style9" />
    </td>
    </tr>
    <tr>
    <td class="style16">
    任免令修改，審核，統計資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_POSIT2" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btPOSIT2_BACKUP" runat="server" Text="確定" onclick="btPOSIT2_BACKUP_Click" class="style9" />
    </td>
    </tr>
    <tr>
    <td class="style16">
    差假資料備份
    </td>
    <td class="style3">
        <asp:DropDownList ID="DropDownList_MZ_YEAR_DLTB01" runat="server">
        </asp:DropDownList>
        年度
    </td>
    <td>
        <asp:Button ID="btDLTB01_BACKUP" runat="server" Text="確定" onclick="btDLTB01_BACKUP_Click" class="style9" />
    </td>
    </tr>
    </table> 
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
