<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="UserGroupSearch.aspx.cs" Inherits="TPPDDB.UserGroupSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    
        <table style="width: 400px">
            <tr>
                <td class="style2" align="right">
                    姓名</td>
                <td class="style1">
    <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" align="right">
                    身分證字號</td>
                <td class="style1">
    <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" align="right">
                    AD帳號</td>
                <td class="style1">
    <asp:TextBox ID="ADTextBox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td class="style1">
    <asp:Button ID="SearchButton" runat="server" Text="查詢" onclick="SearchButton_Click" />
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td class="style1">
                    <asp:Label ID="MSGLabel" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    
    </div>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <asp:GridView ID="TargetSelectGridView" runat="server" 
        onselectedindexchanged="TargetSelectGridView_SelectedIndexChanged" DataKeyNames="MZ_ID">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
