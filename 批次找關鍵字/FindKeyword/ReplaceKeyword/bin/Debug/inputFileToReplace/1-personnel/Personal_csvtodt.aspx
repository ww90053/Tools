<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_csvtodt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_csvtodt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="background-color: #6699FF; color: White; width: 100%">
        <tr>
            <td style="text-align: left; font-size: large; font-family: 標楷體">
                經歷、考試、學歷轉檔作業
            </td>
            <td style="text-align: right; font-size: large; font-family: 標楷體">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:FileUpload ID="FileUpload1" runat="server" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="轉檔" 
        CssClass="KEY_IN_BUTTON_BLUE" />
</asp:Content>
