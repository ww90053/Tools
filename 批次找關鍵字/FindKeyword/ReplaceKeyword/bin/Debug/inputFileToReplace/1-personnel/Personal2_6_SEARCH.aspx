<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal2_6_SEARCH.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal2_6_SEARCH" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="SEARCH_TITLE">
                            案號 ：
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NO" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; text-align: right;">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btLEAVE" runat="server" Text="離開" OnClick="btLEAVE_Click" CssClass="SEARCH_BUTTON_BLACK" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
