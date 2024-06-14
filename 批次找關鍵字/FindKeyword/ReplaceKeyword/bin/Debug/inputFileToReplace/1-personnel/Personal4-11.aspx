<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal4-11.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_11" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            width: 100%;
        }
        .style4
        {
            text-align: left;
        }
        .style5
        {
            text-align: left;
        }
        .style38
        {
            font-family: 標楷體;
            text-align: left;
        }
        .style39
        {
            font-size: large;
        }
        .style40
        {
            font-family: 標楷體;
            text-align: left;
            width: 432px;
        }
        .style41
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 118px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_STATIC" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr class="style39">
                        <td class="style40">
                            獎懲統計分類
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" CssClass="style38"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table border="1" class="style2">
                    <tr>
                        <td class="style41">
                            獎懲統計分類代碼
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_STATIC_NO" runat="server" Style="margin-left: 0px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style41">
                            獎懲統計分類名稱
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_STATIC_NAME" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                    <table style="background-color: #CCFFFF; color: White; width: 100%">
                        <tr >
                            <td style="text-align: center;">
                                <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                    Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" 
                                    AccessKey="a" />
                                <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                    CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                                <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                    Text="確定" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                                <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                    OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" 
                                    Enabled="False" />
                                <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                    Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                    Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server">
                <cc1:TBGridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" DataSourceID="SqlDataSource1" EnableEmptyContentRender="True"
                    GridLines="None" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                    Style="text-align: left" Width="100%" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="STATIC_NO" HeaderText="獎懲分類統計代碼" SortExpression="STATIC_NO">
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="STATIC_NAME" HeaderText="獎懲分類統計名稱" SortExpression="STATIC_NAME" />
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
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM A_STATIC ">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
