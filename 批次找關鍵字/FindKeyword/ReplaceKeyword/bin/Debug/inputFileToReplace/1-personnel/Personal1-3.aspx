<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal1-3.aspx.cs" Inherits="TPPDDB._1_personnel.Personal1_3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 108px;
            text-align: left;
        }
        .style2
        {
            width: 242px;
            text-align: left;
        }
        .style3
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="background-color: #6699FF; color: White; width: 100%; font-size: large;
        font-family: 標楷體;">
        <tr>
            <td style="text-align: left">
                人事基本資料轉檔作業
            </td>
        </tr>
    </table>
    <table border="1" style="width: 100%;">
        <tr>
            <td class="style1">
                人事基本資料檔
            </td>
            <td class="style2">
                <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
            </td>
            <td class="style3">
                <asp:Button ID="Button1" runat="server" Text="轉檔" OnClick="Button1_Click" 
                    CssClass="KEY_IN_BUTTON_BLUE" />
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="3">
                <asp:Label ID="lab_Msg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <%--<tr>
            <td class="style1">
                獎懲依據代碼檔</td>
            <td class="style2">
                <asp:FileUpload ID="FileUpload2" runat="server" Width="300px" />
            </td>
            <td class="style3">
                <asp:Button ID="Button2" runat="server" Text="轉檔" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                人事代碼檔</td>
            <td class="style2">
                <asp:FileUpload ID="FileUpload3" runat="server" Width="300px" />
            </td>
            <td class="style3">
                <asp:Button ID="Button3" runat="server" Text="轉檔" />
            </td>
        </tr>--%>
    </table>
</asp:Content>
