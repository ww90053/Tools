<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_DLCODE.aspx.cs" Inherits="TPPDDB._3_forleave.C_DLCODE" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            差假別代碼管理
                        </td>
                        <td class="style4">
                            <b>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </b>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            差假別代號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_CODE" runat="server" MaxLength="2" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RV_CODE" runat="server" ControlToValidate="TextBox_MZ_CODE"
                                ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            差假別名稱
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_CNAME" runat="server" MaxLength="4" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <table class="style12">
                    <tr>
                        <td class="style6">
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="style9" CausesValidation="False" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" CssClass="style9" />
                            <asp:Button ID="btOK" runat="server" Enabled="False" OnClick="btOK_Click" 
                                Text="確定" CssClass="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" CssClass="style9" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="style9" AccessKey="d" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server">
                <cc1:TBGridView ID="GridView1" runat="server" Width="100%" CellPadding="4" EnableEmptyContentRender="True"
                    GridLines="None" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                    OnRowCommand="GridView1_RowCommand" 
                    OnRowDataBound="GridView1_RowDataBound" Style="text-align: left"
                    AllowPaging="True" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_CODE" HeaderText="假別代碼" SortExpression="MZ_CODE">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_CNAME" HeaderText="假別名稱" SortExpression="MZ_CNAME" />
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM C_DLCODE ORDER BY MZ_CODE">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
