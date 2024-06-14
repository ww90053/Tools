<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalPOSIT1IDSearch_WithBP.aspx.cs" Inherits="TPPDDB._1_personnel.PersonalPOSIT1IDSearch_WithBP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 78px;
        }
        .style3
        {
            width: 89px;
        }
        .style4
        {
            width: 62px;
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
                <table class="style1" border="1">
                    <tr>
                        <td class="style2">
                            文號</td>
                        <td class="style3">
                            北縣警人</td>
                        <td class="style4">
                            字號</td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="確定" onclick="Button1_Click" />
                    <asp:Button ID="Button2" runat="server" Text="離開" onclick="Button2_Click" />
                </td>
                </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
