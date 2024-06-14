<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/TPPD.Master"
CodeBehind="Personal21.aspx.cs" Inherits="TPPDDB._1_personnel.Personal21" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
        .style6
        {
            text-align: left;
        }
        .style7
        {
            text-align: left;
        }
        .style8
        {
            text-align: left;
        }
        .style10
        {
            text-align: right;
            font-size: large;
            font-family: 標楷體;
        }
        .style11
        {
            text-align: left;
            font-family: 標楷體;
            font-size: large;
            width: 394px;
        }
        .style12
        {
        }
        .style111
        {
            border: solid 0px;
        }
        .style112
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 80px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td class="style11">
                        單位隸屬機關
                    </td>
                    <td class="style10">
                        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_UNIT_AD" runat="server">
                <table border="1" class="style2">
                    <tr>
                        <td class="style112">
                            單位
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            機關
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10" CssClass="style133" Height="19px"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            繁重加給
                        </td>
                        <td class="style8">
                            <asp:DropDownList ID="ddl_electric" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_electric_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                <asp:ListItem Value="8435">繁重 8435</asp:ListItem>
                                <asp:ListItem Value="7592">較重 7592</asp:ListItem>
                                <asp:ListItem Value="5905">單純 5905</asp:ListItem>
                                <asp:ListItem Value="4218">值宿所 4218</asp:ListItem>
                                <asp:ListItem Value="3373">警察局 3373</asp:ListItem>
                                <asp:ListItem Value="5905">其他機關一級 5905</asp:ListItem>
                                <asp:ListItem Value="3373">其他機關三級 3373</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            備註
                        </td>
                        <td class="style8">
                            <asp:TextBox ID="TextBox_NOTE" runat="server" CssClass="style7" MaxLength="16"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td class="style12">
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" 
                                AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" 
                                CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" 
                                OnClientClick="return confirm(&quot;確定刪除？&quot;);" 
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Height="300px">
                <cc2:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataSourceID="SqlDataSource1"
                    EnableEmptyContentRender="True" GridLines="None" Style="text-align: left"
                    Width="100%" AllowPaging="True" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                    OnPageIndexChanging="GridView1_PageIndexChanging" ForeColor="#333333" EnableModelValidation="True">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" SortExpression="MZ_UNIT">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_AD" HeaderText="機關" SortExpression="MZ_AD" />
                        <asp:BoundField DataField="electric" HeaderText="繁重加給" SortExpression="electric" />
                        <asp:BoundField DataField="note" HeaderText="備註"  SortExpression="note" />
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc2:TBGridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM &quot;a_electric&quot;">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
