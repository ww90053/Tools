<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_DLCODE_SETDAY.aspx.cs" Inherits="TPPDDB._3_forleave.C_DLCODE_SETDAY" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style>
        .style111
        {
            border: solid 0px;
        }
    </style>
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
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            假 別
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_CODE" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_CODE_TextChanged" MaxLength="2"></asp:TextBox>
                            <asp:Button ID="btCODE" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btCODE_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_CODE1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            時數上限
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_DAY" runat="server" Width="100px" MaxLength="4"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_DAY_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_DAY">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <table class="style12">
                    <tr>
                        <td>
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="style9" CausesValidation="False" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Enabled="False"
                                CssClass="style9" />
                            <asp:Button ID="btOK" runat="server" Enabled="False" OnClick="btOK_Click" Text="確定"
                                CssClass="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                                Text="取消" CausesValidation="False" Enabled="False" CssClass="style9" />
                            <asp:Button ID="btDelete" runat="server" CausesValidation="False" meta:resourcekey="btDeleteResource1"
                                OnClick="btDelete_Click" Text="刪除" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="style9" AccessKey="d" />
                        </td>
                    </tr>
                </table>
                <cc1:TBGridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                    CellPadding="4" DataSourceID="SqlDataSource1" EnableEmptyContentRender="True"
                    GridLines="None" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                    Style="text-align: left" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_CODE" HeaderText="MZ_CODE" SortExpression="MZ_CODE" />
                        <asp:BoundField DataField="MZ_CODE_NAME" HeaderText="假別" SortExpression="MZ_CODE_NAME">
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_DAY" HeaderText="天數" SortExpression="MZ_DAY" />
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
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_CODE,(SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE=C_DLCODE_SETDAY.MZ_CODE ) AS MZ_CODE_NAME,MZ_DAY FROM C_DLCODE_SETDAY">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
