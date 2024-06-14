<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal4-4.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_4" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style4
        {
            height: 18px;
            text-align: left;
        }
        .style5
        {
            text-align: center;
        }
        .style38
        {
            text-align: left;
            font-size: large;
            font-family: 標楷體;
        }
        .style39
        {
            text-align: right;
            font-size: large;
            font-family: 標楷體;
        }
        .style40
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 88px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table style="background-color: #6699FF; color: White; width: 100%">
        <tr>
            <td class="style38">
                獎懲依據對照表
            </td>
            <td class="style39">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_PROLNO" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="style40">
                            獎懲依據代碼
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_MZ_PROLNO" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style40">
                            獎懲依據條文
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_PRONAME" runat="server" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #CCFFFF; color: White; width: 100%" 
                    class="KEY_IN_BUTTON_BLUE">
                    <tr>
                        <td class="style5">
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" 
                                AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
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
            <asp:Panel ID="Panel1" runat="server" Height="300px">
                <cc1:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataSourceID="SqlDataSource1"
                    EnableEmptyContentRender="True" GridLines="None" Width="100%" AllowPaging="True"
                    OnRowCommand="GridView1_RowCommand" 
                    OnRowDataBound="GridView1_RowDataBound" Style="text-align: left"
                    PageSize="15" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_PROLNO" HeaderText="獎懲依據代碼" SortExpression="MZ_PROLNO">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_PRONAME" HeaderText="獎懲依據條文" SortExpression="MZ_PRONAME" />
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
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM &quot;A_PROLNO&quot;">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
