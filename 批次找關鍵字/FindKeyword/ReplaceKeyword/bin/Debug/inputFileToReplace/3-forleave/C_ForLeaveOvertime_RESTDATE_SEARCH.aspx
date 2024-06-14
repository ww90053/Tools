<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_RESTDATE_SEARCH.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_RESTDATE_SEARCH" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="MZ_DATE" DataSourceID="SqlDataSource1" GridLines="None" Width="100%"
                    ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MZ_DATE" HeaderText="可補休日期" ReadOnly="True" SortExpression="MZ_DATE" />
                        <asp:BoundField DataField="MZ_RESTHOUR" HeaderText="可休時數" SortExpression="OTIME" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <table border="1" width="100%">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" class="style53" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" Text="離開" OnClick="Button2_Click" class="style54"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
