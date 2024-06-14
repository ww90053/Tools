<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PoliceSearchPanel.ascx.cs"
    Inherits="TPPDDB._2_salary.UserControl.PoliceSearchPanel" %>
<table class="TableStyleBlue">
    <tr>
        <th>
            發薪機關
        </th>
        <td>
            <asp:DropDownList ID="ddl_ad" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddl_ad_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            服務單位
        </th>
        <td>
            <asp:DropDownList ID="ddl_unit" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            員工編號
        </th>
        <td>
            <asp:TextBox ID="txt_polno" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
            身份證號
        </th>
        <td>
            <asp:TextBox ID="txt_idcard" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
            姓名
        </th>
        <td>
            <asp:TextBox ID="txt_name" runat="server" MaxLength="5" Width="100px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center;">
            <asp:Button ID="btn_search" runat="server" Text="查詢" 
                onclick="btn_search_Click" />
        </td>
    </tr>
</table>
