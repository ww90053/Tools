<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_PROLNO_DETAIL_SHOW.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_PROLNO_DETAIL_SHOW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
            BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="RULE_NO" 
            DataSourceID="SqlDataSource1" EmptyDataText="無資料" GridLines="Vertical" 
            Width="100%" onrowcommand="GridView1_RowCommand">
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <Columns>
                <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                <ItemStyle Width="40px" />
                </asp:CommandField>
                <asp:BoundField DataField="RULE_NO" HeaderText="獎懲依據" ReadOnly="True" 
                    SortExpression="RULE_NO">
                <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="RULE_NAME" HeaderText="說明" 
                    SortExpression="RULE_NAME" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#DCDCDC" />
        </asp:GridView>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
            SelectCommand="SELECT &quot;RULE_NO&quot;, &quot;RULE_NAME&quot; FROM &quot;A_PROLNO_DETAIL&quot;">
        </asp:SqlDataSource>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
