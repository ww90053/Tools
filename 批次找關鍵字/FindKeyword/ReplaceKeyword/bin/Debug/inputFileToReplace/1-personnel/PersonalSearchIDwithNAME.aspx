<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalSearchIDwithNAME.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersonalSearchPRK1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
        }
        .style3
        {
        }
        .style6
        {
        }
        #form1
        {
        }
        .style7
        {
            width: 66px;
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
<body style="font-size: small">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table border="1" class="style2" width="100%">
                    <tr>
                        <td class="style7">
                            姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="TextBox_NAME" runat="server" Width="120px" MaxLength="6" CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            服務機關:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" CssClass="SEARCH_DROPDOWNLIST">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04' ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            服務單位
                        </td>
                        <td class="style6">
                            <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" CssClass="SEARCH_DROPDOWNLIST">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel2" runat="server" Visible="false">
                    <table border="1" class="style2" width="100%">
                        <tr>
                            <td class="style7">
                                檢索號碼
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_RETRIEVE" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 100%; text-align: right;">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="離開" CssClass="SEARCH_BUTTON_BLACK" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                GridLines="None" OnRowCommand="GridView1_RowCommand" Width="100%" OnPageIndexChanging="GridView1_PageIndexChanging"
                                Style="text-align: left" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                                    <asp:BoundField HeaderText="現服機關" DataField="MZ_EXAD" />
                                    <asp:BoundField HeaderText="現服單位" DataField="MZ_EXUNIT" />
                                    <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
