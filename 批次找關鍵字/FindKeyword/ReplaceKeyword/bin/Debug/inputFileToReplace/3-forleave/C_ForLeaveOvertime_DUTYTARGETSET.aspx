<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYTARGETSET.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYTARGETSET" %>

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
                        <td class="style4">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            任務目標代號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_DUTYTARGET_NO" runat="server" MaxLength="4" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            任務目標內容
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_DUTYTARGET" runat="server" TextMode="MultiLine" Width="683px"
                                Height="109px" MaxLength="1000"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style12">
                <tr bgcolor="#CCFFFF">
                    <td>
                        <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                            Text="新增" CausesValidation="False" class="style9" AccessKey="a" />
                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                            Enabled="False" class="style9" />
                        <asp:Button ID="btOK" runat="server" Enabled="False" OnClick="btOK_Click" Text="確定"
                            class="style9" AccessKey="s" />
                        <asp:Button ID="btCancel" runat="server" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                            Text="取消" CausesValidation="False" Enabled="False" class="style9" />
                        <asp:Button ID="btDelete" runat="server" CausesValidation="False" meta:resourcekey="btDeleteResource1"
                            OnClick="btDelete_Click" Text="刪除" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                            Enabled="False" class="style9" AccessKey="d" />
                    </td>
                </tr>
            </table>
            <cc1:TBGridView ID="GridView1" runat="server" Width="100%" CellPadding="4" EnableEmptyContentRender="True"
                GridLines="None" AutoGenerateColumns="False" DataKeyNames="MZ_DUTYTARGET_NO,MZ_AD,MZ_UNIT"
                DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                Style="text-align: left" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_DUTYTARGET_NO" HeaderText="任務目標代號" ReadOnly="True"
                        SortExpression="MZ_DUTYTARGET_NO">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MZ_DUTYTARGET" HeaderText="任務目標" SortExpression="MZ_DUTYTARGET" />
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
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM C_DUTYTARGET WHERE MZ_AD=@MZ_AD AND MZ_UNIT=@MZ_UNIT ">
                <SelectParameters>
                    <asp:SessionParameter Name="MZ_AD" SessionField="ADPMZ_EXAD" />
                    <asp:SessionParameter Name="MZ_UNIT" SessionField="ADPMZ_EXUNIT" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
