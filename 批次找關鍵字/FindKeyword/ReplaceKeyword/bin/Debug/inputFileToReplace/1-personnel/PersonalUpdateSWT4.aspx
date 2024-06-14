<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalUpdateSWT4.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersonalUpdateSWT4" %>

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
            text-align: center;
        }
        .style3
        {
            text-align: right;
        }
        .style4
        {
            text-align: left;
        }
        .style6
        {
            width: 552px;
        }
        .style7
        {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="Panel1" runat="server" Width="552px">
                    <table class="style1" border="1" width="400px">
                        <tr>
                            <td class="SEARCH_TITLE">
                                案&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號:
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="TextBox_MZ_NO" runat="server" CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="SEARCH_TITLE">
                                審核方式:
                            </td>
                            <td class="style4">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="2" 
                                    CssClass="SEARCH_DROPDOWNLIST" 
                                    onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                                    <asp:ListItem Selected="False">單筆</asp:ListItem>
                                    <asp:ListItem Selected="True">整批</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SEARCH_TITLE">
                                整批審核:
                            </td>
                            <td class="style4">
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatColumns="2" CssClass="SEARCH_DROPDOWNLIST">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:Button ID="Button1" runat="server" CssClass="SEARCH_BUTTON_BLUE" Text="確認" OnClick="Button1_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button3" runat="server" CssClass="SEARCH_BUTTON_BLACK" OnClick="Button3_Click"
                                    Text="離開" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Visible="False"
                Width="552px" DataKeyNames="MZ_ID">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox_SWT4" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" SortExpression="MZ_ID" />
                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                    <asp:BoundField DataField="MZ_AD" HeaderText="機關" SortExpression="MZ_AD" />
                    <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" SortExpression="MZ_UNIT" />
                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                    <asp:BoundField DataField="MZ_SRANK" HeaderText="薪俸職等" SortExpression="MZ_SRANK" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_SWT4,MZ_ID,MZ_NAME,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD)  AS MZ_AD,
(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT) AS MZ_UNIT,
(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '26' AND MZ_KCODE =MZ_OCCC) AS MZ_OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_SRANK ) AS MZ_SRANK FROM A_PRKB  WHERE  (MZ_AD=@MZ_AD OR MZ_EXAD=@MZ_AD) AND MZ_NO=@MZ_NO AND (MZ_SWT4&lt;&gt;'Y' OR MZ_SWT4 IS NULL)">
                <SelectParameters>
                    <asp:SessionParameter Name="MZ_AD" SessionField="MZ_AD" />
                    <asp:ControlParameter ControlID="TextBox_MZ_NO" Name="MZ_NO" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
            <table class="style6">
                <tr>
                    <td class="style7">
                        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="確定" Visible="False"
                            CssClass="SEARCH_BUTTON_BLUE" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button4" runat="server" CssClass="SEARCH_BUTTON_BLACK" Visible="False"
                            OnClick="Button3_Click" Text="離開" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
