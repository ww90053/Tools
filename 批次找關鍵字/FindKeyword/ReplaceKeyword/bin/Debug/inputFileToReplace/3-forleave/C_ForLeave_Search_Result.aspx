<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeave_Search_Result.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeave_Search_Result" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    Width="100%" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
                    OnRowDataBound="GridView1_RowDataBound">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="是否審核">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MZ_CHK1") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Checked='<%# Eval("MZ_CHK1").ToString()=="Y"?true:false %>'
                                    OnCheckedChanged="CheckBox1_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                        <asp:BoundField DataField="MZ_CODE" HeaderText="假別" />
                        <asp:BoundField DataField="MZ_IDATE1" HeaderText="請假起日" />
                        <asp:BoundField DataField="MZ_ODATE" HeaderText="請假迄日" />
                        <asp:BoundField DataField="MZ_ITIME1" HeaderText="請假起時" />
                        <asp:BoundField DataField="MZ_OTIME" HeaderText="請假迄時" />
                        <asp:BoundField DataField="MZ_TDAY" HeaderText="天數" />
                        <asp:BoundField DataField="MZ_TTIME" HeaderText="時數" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
