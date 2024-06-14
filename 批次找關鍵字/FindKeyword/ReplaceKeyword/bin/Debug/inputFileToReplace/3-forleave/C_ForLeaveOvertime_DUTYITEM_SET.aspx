<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYITEM_SET.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYITEM_SET" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style10">
                <tr>
                    <td class="style8">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            勤務項目編號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTY_NO" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_DUTY_NO"
                                Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            勤務項目
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTY_NAME" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" BackColor="#CCFFFF">
                <table class="style6">
                    <tr>
                        <td>
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CausesValidation="False" class="style9" AccessKey="a" />
                           <%-- <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" class="style9" />--%>
                            <asp:Button ID="btOK" runat="server" Enabled="False" OnClick="btOK_Click" Text="確定"
                                class="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                                Text="取消" CausesValidation="False" Enabled="False" class="style9" />
                           <%-- <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" Enabled="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                class="style9" AccessKey="d" />--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:TBGridView ID="GridView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1"
                EmptyDataText="查無資料" EnableEmptyContentRender="True" GridLines="None" AutoGenerateColumns="False"
                DataKeyNames="DUTY_NO" Width="100%" CssClass="style7" OnRowCommand="GridView1_RowCommand"
                OnRowDataBound="GridView1_RowDataBound" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="DUTY_NO" HeaderText="勤務編號" ReadOnly="True" SortExpression="DUTY_NO">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DUTY_NAME" HeaderText="勤務名稱" SortExpression="DUTY_NAME">
                        <ItemStyle Width="85%" />
                    </asp:BoundField>
                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </cc1:TBGridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM  C_DUTYITEM ORDER BY DUTY_NO">
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
