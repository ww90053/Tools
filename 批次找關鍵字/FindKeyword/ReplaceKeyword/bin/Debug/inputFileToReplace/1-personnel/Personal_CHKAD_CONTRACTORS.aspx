<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="Personal_CHKAD_CONTRACTORS.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_CHKAD_CONTRACTORS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            text-align: left;
            width: 57px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            height: 28px;
        }
        .style2
        {
            text-align: left;
            width: 57px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%; font-family: 標楷體;
                    font-size: large; text-align: left;">
                    <tr>
                        <td class="style9">
                            承辦人員聯絡資料輸入
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style1">
                            核定機關
                        </td>
                        <td style="height: 28px">
                            <asp:TextBox ID="TextBox_MZ_CHKAD" runat="server" Width="75px" MaxLength="10" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_CHKAD_TextChanged"></asp:TextBox>
                            <asp:Label ID="lbl_MZ_CHKAD1" runat="server" Width="180px" CssClass="KEY_IN_LABLE"></asp:Label>
                            <asp:Label ID="Label1" runat="server" Visible="False" Font-Bold="True" 
                                ForeColor="#0033FF"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px" MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            電話號碼
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_TELNO" runat="server" Width="300px" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            傳真號碼
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_FAXNO" runat="server" MaxLength="12"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            電子信箱
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_EMAIL" runat="server" Width="600px" MaxLength="120"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="background-color: #6699FF; color: White; width: 100%; text-align: center;
                    background-color: #CCFFFF">
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="修改資料" OnClick="Button1_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
