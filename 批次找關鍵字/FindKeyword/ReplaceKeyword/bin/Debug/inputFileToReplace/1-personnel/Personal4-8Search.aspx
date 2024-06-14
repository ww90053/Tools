<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal4-8Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal4_8Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 70%;
        }
        .style3
        {
            text-align: right;
        }
        .style4
        {
            width: 94px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_NOTE" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="style4">
                            片語代號
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NOTE" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            片語內容
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NOTE_NAME" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style1">
                    <tr>
                        <td class="style3">
                            <asp:Button ID="btOk" runat="server" OnClick="btOk_Click" Text="確定" />
                            <asp:Button ID="btLeave" runat="server" Text="離開" OnClick="btLeave_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
