<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_Efficiency_reach.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_Efficiency_reach" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-weight: bold;
            font-size: medium;
            color: #000000;
            font-family: 新細明體;
        }
        .style2
        {
            font-family: 新細明體;
        }
        .style3
        {
            font-weight: bold;
            font-size: medium;
            color: #0033CC;
            font-family: 新細明體;
        }
        .style5
        {
            font-weight: bold;
            color: Maroon;
            font-family: 新細明體;
            font-size: medium;
        }
        .style7
        {
            font-family: 新細明體;
            font-size: medium;
        }
        .style8
        {
            font-size: medium;
        }
        .style9
        {
            width: 83px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
            font-family: 新細明體;
            font-size: medium;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="KEY_IN_TITLE_LEFT">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" style="width: 400px;">
                        <tr>
                            <td class="style9">
                                機　　關:</td>
                            <td >
                                <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" 
                                    CssClass="style5">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04' ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td class="style9">
                                單　　位:</td>
                            <td >
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" 
                                    CssClass="style5">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style9">
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 度:</td>
                            <td >
                                <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" MaxLength="3" Width="50px" 
                                    Font-Bold="True" ForeColor="Maroon" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_YEAR">
                                </cc1:FilteredTextBoxExtender>
                                <span class="style8">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="TextBox_MZ_YEAR" CssClass="style2" Display="Dynamic" 
                                    ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                </span><span class="style2"><span class="style8">(例：099) </span></span>
                            </td>
                        </tr>
                    </table>
                    </table>
                    <table border="1" style="width: 400px;">
                        <tr>
                            <td class="SEARCH_TD">
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" 
                                    CssClass="style3" />
                                <span class="style7">&nbsp;&nbsp;&nbsp;&nbsp; </span>
                                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="離開" 
                                    CssClass="style1" />
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
