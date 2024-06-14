<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal4-4Search.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_4Search" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style3
        {
            text-align: right;
        }
        .style4
        {
            width: 114px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table border="1" style="width:70%">
            <tr>
                <td class="style4">
                    獎懲依據代碼</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_PROLNO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    獎懲依據條文</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_PRONAME" runat="server" Width="100%"></asp:TextBox>
                </td>
            </tr>
        </table>
    <table style="width:70%">
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
