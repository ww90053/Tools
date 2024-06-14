<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Card_Insert.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Card_Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="style10">
        <tr>
            <td class="style8">
                服務證管理轉檔作業
            </td>
            <td class="style4">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:FileUpload ID="FileUpload1" runat="server" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="轉檔" />
    <br />
    <asp:Label
        ID="Label2" runat="server" Text="Label"></asp:Label>
    <asp:GridView
        ID="GridView1" runat="server">
    </asp:GridView>
</asp:Content>
