<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal1-3_1.aspx.cs" Inherits="TPPDDB._1_personnel.Personal1_3_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="background-color: #6699FF; color: White; width: 100%">
        <tr>
            <td style="text-align: left; font-size: large; font-family: 標楷體">
               WEBHR基本資料轉檔
            </td>
            <td style="text-align: right; font-size: large; font-family: 標楷體">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
        <asp:FileUpload ID="FileUpload_Excel" runat="server" />                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="FileUpload_Excel" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
    <asp:Button ID="Button" runat="server" OnClick="Button1_Click" Text="轉檔" 
        CssClass="KEY_IN_BUTTON_BLUE" />
</asp:Content>
