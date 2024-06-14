<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal4-9Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal4_9Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style3
        {
            text-align: center;
        }
        .ajax__combobox_buttoncontainer button
        {
            background-image: url(mvwres://AjaxControlToolkit, Version=3.0.30512.17815, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e/AjaxControlToolkit.ComboBox.arrow-down.gif);
            background-position: center;
            background-repeat: no-repeat;
            border-color: ButtonFace;
            height: 15px;
            width: 15px;
        }
        .style5
        {
            width: 60px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style111
        {
            border: solid 0px;
        }
    </style>
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_UNIT_AD" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="style5">
                            單位:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" AutoPostBack="True" Enabled="False"
                                MaxLength="4" OnTextChanged="TextBox_MZ_UNIT_TextChanged" Width="35px" 
                                CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="105px" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            機關:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource2"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" 
                                AppendDataBoundItems="True" CssClass="SEARCH_DROPDOWNLIST">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM( MZ_KCHI) AS MZ_KCHI,RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213'" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
                <table class="style1" border="1">
                    <tr>
                        <td class="style3">
                            <asp:Button ID="btOk" runat="server" OnClick="btOk_Click" Text="確定" 
                                CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btLeave" runat="server" Text="離開" OnClick="btLeave_Click" 
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
