<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal4-7Search.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_7Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
            width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table border="1" class="style1">
            <tr>
                <td class="style4">
                    配分代碼</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_POLNO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    配分條文</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_PNAME" runat="server" Width="100%"></asp:TextBox>
                </td>
            </tr>
        </table>
    <table class="style1">
                <tr>
                <td class="style3" >
                    <asp:Button ID="btOk" runat="server" onclick="btOk_Click" Text="確定" />
                    <asp:Button ID="btLeave" runat="server" Text="離開" onclick="btLeave_Click" />
                </td>
                </tr>
         </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
