<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_RESTDATE_SEARCH_New.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_RESTDATE_SEARCH_New" %>

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
                <asp:GridView ID="gv_resultat" runat="server" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OVER_TIME" HeaderText="可補休日期" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SURPLUS_HOUR" HeaderText="可休時數" ItemStyle-HorizontalAlign="Center" />
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
                            <asp:Button ID="btn_Ok" runat="server" Text="確定" OnClick="btn_Ok_Click" class="style53" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_Leave" runat="server" Text="離開" OnClick="btn_Leave_Click" class="style54"
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