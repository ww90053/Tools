<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalPicInput.aspx.cs" Inherits="TPPDDB._1_personnel.PersonalPicInput" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 18%;
        }
        .style2
        {
            text-align: right;
        }
        .style3
        {
            text-align: right;
            width: 316px;
        }
        .style4
        {
            font-size: small;
        }
        .style5
        {
            width: 316px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table border="1" align="center" class="style1">
            <tr>
                <td class="style5">
                    選擇圖片來源</td>
            </tr>
            <tr>
                <td class="style5">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="Label_SMG" runat="server" CssClass="style4"></asp:Label>
                    <asp:Button ID="Button1" runat="server" CssClass="style2" 
                        onclick="Button1_Click" Text="上傳" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
