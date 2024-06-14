<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_PRK2_DETAIL.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_PRK2_DETAIL" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <div>
        <div>
            <cc1:TBGridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" Width="2500px" AutoGenerateColumns="False" DataKeyNames="MZ_NO,MZ_ID,MZ_PRCT"
                EnableEmptyContentRender="True" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" ReadOnly="True"  />
                    <asp:BoundField DataField="姓名" HeaderText="姓名" />
                    <asp:BoundField DataField="編制機關" HeaderText="編制機關" />
                    <asp:BoundField DataField="編制單位" HeaderText="編制單位" />
                    <asp:BoundField DataField="現服機關" HeaderText="現服機關"/>
                    <asp:BoundField DataField="現服單位" HeaderText="現服單位"/>
                    <asp:BoundField DataField="職稱" HeaderText="職稱" />
                    <asp:BoundField DataField="官職等起" HeaderText="官職等起" />
                    <asp:BoundField DataField="官職等迄" HeaderText="官職等迄"/>
                    <asp:BoundField DataField="薪俸職等" HeaderText="薪俸職等" />
                    <asp:BoundField DataField="職序" HeaderText="職序" />
                    <asp:BoundField DataField="獎懲內容" HeaderText="獎懲內容" ReadOnly="True" />
                    <asp:BoundField DataField="獎懲類別" HeaderText="獎懲類別" />
                    <asp:BoundField DataField="獎懲統計分類" HeaderText="獎懲統計分類" />
                    <asp:BoundField DataField="獎懲結果" HeaderText="獎懲結果" />
                    <asp:BoundField DataField="獎懲依據編號" HeaderText="獎懲依據編號" />
                    <asp:BoundField DataField="依據類別" HeaderText="依據類別" />
                    <asp:BoundField DataField="是否配分" HeaderText="是否配分" />
                    <asp:BoundField DataField="配分款項" HeaderText="配分款項" />
                    <asp:BoundField DataField="加重條款" HeaderText="加重條款" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </cc1:TBGridView>
        </div>
        <table width="100%" style="text-align: center;">
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="離開" CssClass="SEARCH_BUTTON_BLACK"
                        OnClick="Button2_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
