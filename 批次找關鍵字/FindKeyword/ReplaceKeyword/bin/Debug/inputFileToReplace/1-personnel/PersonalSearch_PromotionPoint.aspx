<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalSearch_PromotionPoint.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersonalSearch_PromotionPoint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
        }
        .style4
        {
        }
        .style6
        {
            text-align: center;
        }
        .style7
        {
            width: 103px;
            font-weight: 700;
            color: #0033CC;
            text-align: center;
            background-color: #99FFCC;
        }
    </style>

    <script type="text/javascript">
        function leave() {
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="style1">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" class="style2" width="400px">
                        <tr>
                            <td class="style7">
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 度:
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="txt_Year" runat="server" CssClass="SEARCH_DROPDOWNLIST" MaxLength="3"
                                    Width="75px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="TextBox_NAME" runat="server" CssClass="SEARCH_DROPDOWNLIST" MaxLength="6"
                                    Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                身分證號:
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="TextBox_ID" runat="server" MaxLength="10" Width="75px" CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                <asp:Label ID="Label_AD" runat="server" Text="現服機關"></asp:Label>
                                :
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" CssClass="SEARCH_DROPDOWNLIST">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04' ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                <asp:Label ID="Label_UNIT" runat="server" Text="現服單位"></asp:Label>
                                :
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" CssClass="SEARCH_DROPDOWNLIST">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table id="t1" runat="server" border="1" style="width: 400px; display: none;">
                        <tr>
                            <td class="style7">
                                <asp:Label ID="Label1" runat="server" Text="權限設定"></asp:Label>
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="DropDownList_Power" runat="server" Width="100px">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                    <asp:ListItem>A</asp:ListItem>
                                    <asp:ListItem>B</asp:ListItem>
                                    <asp:ListItem>C</asp:ListItem>
                                    <asp:ListItem>D</asp:ListItem>
                                    <asp:ListItem>E</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table id="t2" runat="server" border="1" style="width: 400px; display: none;">
                        <tr>
                            <td class="style7">
                                年度
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_YEAR">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_YEAR"
                                    Display="Dynamic" ErrorMessage="不可空白" Enabled="False"></asp:RequiredFieldValidator>
                                (例：099)
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                是否參加
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="DropDownList_MZ_SWT" runat="server">
                                    <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                    <asp:ListItem Value="0">參加</asp:ListItem>
                                    <asp:ListItem Value="1">不參加</asp:ListItem>
                                    <asp:ListItem Value="2">其他</asp:ListItem>
                                    <asp:ListItem Value="3">另考</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table border="1" style="width: 400px;">
                        <tr>
                            <td class="style6">
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" CssClass="SEARCH_BUTTON_BLUE" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="離開" CssClass="SEARCH_BUTTON_BLACK" />
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
