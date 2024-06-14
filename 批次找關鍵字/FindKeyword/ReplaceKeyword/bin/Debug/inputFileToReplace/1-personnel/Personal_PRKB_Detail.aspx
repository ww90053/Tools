<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_PRKB_Detail.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_PRKB_Detail" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                Visible="false" GridLines="None" Width="2500px" AutoGenerateColumns="False" DataKeyNames="MZ_NO,MZ_ID,MZ_PRCT"
                EnableEmptyContentRender="True" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="MZ_NO" HeaderText="案號" ReadOnly="True" SortExpression="MZ_NO" />
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" ReadOnly="True" SortExpression="MZ_ID" />
                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                    <asp:BoundField DataField="MZ_AD" HeaderText="編制機關" SortExpression="MZ_AD" />
                    <asp:BoundField DataField="MZ_UNIT" HeaderText="編制單位" SortExpression="MZ_UNIT" />
                    <asp:BoundField DataField="MZ_EXAD" HeaderText="現服機關" SortExpression="MZ_EXAD" />
                    <asp:BoundField DataField="MZ_EXUNIT" HeaderText="現服單位" SortExpression="MZ_EXUNIT" />
                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                    <asp:BoundField DataField="MZ_RANK" HeaderText="官職等起" SortExpression="MZ_RANK" />
                    <asp:BoundField DataField="MZ_RANK1" HeaderText="官職等迄" SortExpression="MZ_RANK1" />
                    <asp:BoundField DataField="MZ_SRANK" HeaderText="薪俸職等" SortExpression="MZ_SRANK" />
                    <asp:BoundField DataField="MZ_TBDV" HeaderText="職序" SortExpression="MZ_TBDV" />
                    <asp:BoundField DataField="MZ_PRCT" HeaderText="獎懲內容" ReadOnly="True" SortExpression="MZ_PRCT" />
                    <asp:BoundField DataField="MZ_PRK" HeaderText="獎懲類別" SortExpression="MZ_PRK" />
                    <asp:BoundField DataField="MZ_PRK1" HeaderText="獎懲統計分類" SortExpression="MZ_PRK1" />
                    <asp:BoundField DataField="MZ_PRRST" HeaderText="獎懲結果" SortExpression="MZ_PRRST" />
                    <asp:BoundField DataField="MZ_PROLNO" HeaderText="獎懲依據編號" SortExpression="MZ_PROLNO" />
                    <asp:BoundField DataField="MZ_POLK" HeaderText="依據類別" SortExpression="MZ_POLK" />
                    <asp:BoundField DataField="MZ_PCODE" HeaderText="是否配分" SortExpression="MZ_PCODE" />
                    <asp:BoundField DataField="MZ_PCODEM" HeaderText="配分款項" SortExpression="MZ_PCODEM" />
                    <asp:BoundField DataField="MZ_PROLNO2" HeaderText="加重條款" SortExpression="MZ_PROLNO2" />
                    <asp:BoundField DataField="MZ_SWT3" HeaderText="權責" SortExpression="MZ_SWT3" />
                    <asp:BoundField DataField="MZ_SWT4" HeaderText="是否核定" SortExpression="MZ_SWT4" />
                    <asp:BoundField DataField="MZ_SWT1" HeaderText="是否謄稿" SortExpression="MZ_SWT1" />
                    <asp:BoundField DataField="MZ_MEMO" HeaderText="說明" SortExpression="MZ_MEMO" />
                    <asp:BoundField DataField="MZ_REMARK" HeaderText="附件" SortExpression="MZ_REMARK" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </cc1:TBGridView>
            <cc1:TBGridView ID="TBGridView1" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" Width="2500px" AutoGenerateColumns="False" DataKeyNames="MZ_NO,MZ_ID,MZ_PRCT"
                EnableEmptyContentRender="True" OnRowDataBound="TBGridView1_RowDataBound">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_NO" HeaderText="案號" ReadOnly="True" SortExpression="MZ_NO" />
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" ReadOnly="True" SortExpression="MZ_ID" />
                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                    <asp:BoundField DataField="MZ_AD" HeaderText="編制機關" SortExpression="MZ_AD" />
                    <asp:BoundField DataField="MZ_UNIT" HeaderText="編制單位" SortExpression="MZ_UNIT" />
                    <asp:BoundField DataField="MZ_EXAD" HeaderText="現服機關" SortExpression="MZ_EXAD" />
                    <asp:BoundField DataField="MZ_EXUNIT" HeaderText="現服單位" SortExpression="MZ_EXUNIT" />
                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                    <asp:BoundField DataField="MZ_RANK" HeaderText="官職等起" SortExpression="MZ_RANK" />
                    <asp:BoundField DataField="MZ_RANK1" HeaderText="官職等迄" SortExpression="MZ_RANK1" />
                    <asp:BoundField DataField="MZ_SRANK" HeaderText="薪俸職等" SortExpression="MZ_SRANK" />
                    <asp:BoundField DataField="MZ_TBDV" HeaderText="職序" SortExpression="MZ_TBDV" />
                    <asp:BoundField DataField="MZ_PRCT" HeaderText="獎懲內容" ReadOnly="True" SortExpression="MZ_PRCT" />
                    <asp:BoundField DataField="MZ_PRK" HeaderText="獎懲類別" SortExpression="MZ_PRK" />
                    <asp:BoundField DataField="MZ_PRK1" HeaderText="獎懲統計分類" SortExpression="MZ_PRK1" />
                    <asp:BoundField DataField="MZ_PRRST" HeaderText="獎懲結果" SortExpression="MZ_PRRST" />
                    <asp:BoundField DataField="MZ_PROLNO" HeaderText="獎懲依據編號" SortExpression="MZ_PROLNO" />
                    <asp:BoundField DataField="MZ_POLK" HeaderText="依據類別" SortExpression="MZ_POLK" />
                    <asp:BoundField DataField="MZ_PCODE" HeaderText="是否配分" SortExpression="MZ_PCODE" />
                    <asp:BoundField DataField="MZ_PCODEM" HeaderText="配分款項" SortExpression="MZ_PCODEM" />
                    <asp:BoundField DataField="MZ_PROLNO2" HeaderText="加重條款" SortExpression="MZ_PROLNO2" />
                    <asp:BoundField DataField="MZ_SWT3" HeaderText="權責" SortExpression="MZ_SWT3" />
                    <asp:BoundField DataField="MZ_SWT4" HeaderText="是否核定" SortExpression="MZ_SWT4" />
                    <asp:BoundField DataField="MZ_SWT1" HeaderText="是否謄稿" SortExpression="MZ_SWT1" />
                    <asp:BoundField DataField="MZ_MEMO" HeaderText="說明" SortExpression="MZ_MEMO" />
                    <asp:BoundField DataField="MZ_REMARK" HeaderText="附件" SortExpression="MZ_REMARK" />
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
                    <asp:Button ID="Button1" runat="server" Text="匯出EXCEL" Style="text-align: center"
                        CssClass="SEARCH_BUTTON_BLUE" OnClick="Button1_Click" />
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
