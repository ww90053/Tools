<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal4-2Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal4_2Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 70%;
        }
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 80px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_KTYPE" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="style4">
                            序&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號:</td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_KTYPE" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            代&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號:</td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_KCODE" runat="server" OnTextChanged="TextBox_MZ_KCODE_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            中文名稱:</td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_KCHI" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style1" border="1">
                    <tr>
                        <td class="style3">
                            <asp:Button ID="btOk" runat="server" OnClick="btOk_Click" Text="確定" CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btLeave" runat="server" Text="離開" OnClick="btLeave_Click" CssClass="SEARCH_BUTTON_BLACK" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
