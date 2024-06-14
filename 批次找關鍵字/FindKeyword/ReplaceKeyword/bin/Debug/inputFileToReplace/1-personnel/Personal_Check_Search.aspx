<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_Check_Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_Check_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未命名頁面</title>
    <style type="text/css">
        .style1
        {
            width: 113px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style2
        {
            width: 113px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
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
                    <table border="1" style="text-align: left; width: 100%;">
                        <tr>
                            <td class="style2">
                                姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="12" Width="100px" 
                                    Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                身分證號
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="75px" 
                                    Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                 檢索號碼</td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_RETRIEVE" runat="server" MaxLength="12"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="style2">
                                列管別
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList_MZ_CONTENT" runat="server" Font-Bold="True" 
                                    ForeColor="Maroon">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="01">教育輔導</asp:ListItem>
                                    <asp:ListItem Value="02">風紀評估</asp:ListItem>
                                    <asp:ListItem Value="03">違法案件</asp:ListItem>
                                    <asp:ListItem Value="04">違紀案件</asp:ListItem>
                                    <asp:ListItem Value="05">其他不良案件</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table border="1" style="text-align: left; width: 100%;">
                        <tr>
                            <td class="style1">
                                列管日期起
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" Width="65px" Font-Bold="True" 
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                            <td class="style1">
                                列管日期迄
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" Width="65px" 
                                    Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table border="1" style="text-align: left; width: 100%;">
                        <tr>
                            <td class="style1">
                                列管機關
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList_MZ_UNAD" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" 
                                    DataValueField="MZ_KCODE" Font-Bold="True" ForeColor="Maroon">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@92'" DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                    <table border="1" style="text-align: left; width: 100%;">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" 
                                    Font-Bold="True" Font-Size="12pt" ForeColor="#0033CC" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btLeave" runat="server" Text="離開" OnClick="btLeave_Click" 
                                    Font-Bold="True" Font-Size="12pt" ForeColor="Maroon" />
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
