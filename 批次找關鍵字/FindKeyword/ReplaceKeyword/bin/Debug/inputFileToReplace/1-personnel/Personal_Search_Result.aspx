<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_Search_Result.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_Search_Result" %>

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
                <asp:GridView ID="GridView1" runat="server" Width="100%" CellPadding="4" 
                    GridLines="None" AutoGenerateColumns="False"
                    OnRowCommand="GridView1_RowCommand" AllowPaging="True" OnDataBound="GridView1_DataBound"
                    DataSourceID="SqlDataSource1" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:CommandField ItemStyle-Width="50px" ShowSelectButton="True">
                            <ItemStyle Width="50px" />
                        </asp:CommandField>
                        <asp:BoundField DataField="MZ_ID" ItemStyle-Width="100px" HeaderText="身分證號">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_NAME" ItemStyle-Width="100px" HeaderText="姓名">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerTemplate>
                        <asp:LinkButton ForeColor="#33CC33" ID="LinkFirst" runat="server" OnClick="Quick_Click">第一頁</asp:LinkButton>
                        <asp:LinkButton ForeColor="#33CC33" ID="LinkPrevious" runat="server" OnClick="Quick_Click">上一頁</asp:LinkButton>
                        <asp:LinkButton ForeColor="#33CC33" ID="LinkNext" runat="server" OnClick="Quick_Click">下一頁</asp:LinkButton>
                        <asp:LinkButton ForeColor="#33CC33" ID="LinkLast" runat="server" OnClick="Quick_Click">最後頁</asp:LinkButton>
                        <asp:DropDownList ID="ddlPageJump" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPageJump_SelectedIndexChanged">
                        </asp:DropDownList>
                        /<asp:Label ID="lbAllPage" runat="server" ForeColor="#33CC33"></asp:Label>
                    </PagerTemplate>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
