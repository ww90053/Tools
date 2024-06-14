<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalGroupUpdatePRKB_MZ_NO.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersonalGroupUpdatePRKB_MZ_NO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style1" border="1">
                    <tr>
                        <td class="SEARCH_TITLE">
                            原案號 ：</td>
                        <td>
                            <asp:TextBox ID="TextBox_OLD_MZ_NO" runat="server" 
                                MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SEARCH_TITLE">
                            新案號 ：</td>
                        <td>
                            <asp:TextBox ID="TextBox_NEW_MZ_NO" runat="server"
                                MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style1" align="center" border="1">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="Button3" runat="server" Text="確定" OnClick="Button1_Click" 
                                CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button4" runat="server" Text="離開" OnClick="Button2_Click" 
                                CssClass="SEARCH_BUTTON_BLACK" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
