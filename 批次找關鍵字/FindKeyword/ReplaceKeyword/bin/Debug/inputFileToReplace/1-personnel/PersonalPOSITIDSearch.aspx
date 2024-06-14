<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalPOSITIDSearch.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersonalPOSITIDSearch" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" style="width: 100%; text-align: left;">
                        <tr>
                            <td class="SEARCH_TITLE">
                                案號起 ：</td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_NO1" runat="server" Width="100px" MaxLength="15"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_NO1"
                                    Display="Dynamic" ErrorMessage="RequiredFieldValidator">不可空白</asp:RequiredFieldValidator>
                            </td>
                            <td class="SEARCH_TITLE">
                                案號迄：
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_NO2" runat="server" Width="100px" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%; text-align: right;">
                        <tr>
                            <td class="style2">
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" 
                                    CssClass="SEARCH_BUTTON_BLUE" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button2" runat="server" Text="離開" OnClick="Button2_Click" 
                                    CssClass="SEARCH_BUTTON_BLACK" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
