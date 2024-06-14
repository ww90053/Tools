<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal4-3.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_3" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
        }
        .style5
        {
            text-align: right;
        }
        .style6
        {
            font-size: large;
            font-family: 標楷體;
        }
        .style7
        {
            font-family: 標楷體;
            text-align: left;
            font-size: large;
        }
        .style9
        {
            text-align: center;
        }
        .style38
        {
        }
        .style111
        {
            border-style: solid;
            border-color: inherit;
            border-width: 0px;
            font-weight: 700;
        }
        .style110
        {
        }
        .style112
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 63px;
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
                    <td class="style7">
                        權責對照表
                    </td>
                    <td class="style5">
                        <asp:Label ID="Label1" runat="server" CssClass="style6"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_SWT3" runat="server">
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style112">
                            編制機關
                        </td>
                        <td class="style40">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10" CssClass="style133" Height="19px"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            編制單位
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            職 稱
                        </td>
                        <td class="style121">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_OCCC_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btOCCC_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            職 序
                        </td>
                        <td class="style127">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="80px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            獎懲結果
                        </td>
                        <td class="style131">
                            <asp:TextBox ID="TextBox_MZ_PRRST" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PRRST_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btPRRST" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRRST_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRRST1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td class="style9">
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" 
                                AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="確定" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc2:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                DataSourceID="SqlDataSource1" EnableEmptyContentRender="True" GridLines="None"
                OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" Width="100%"
                EmptyDataText="查無資料" AllowPaging="True" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_AD" HeaderText="機關" SortExpression="MZ_AD" />
                    <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" SortExpression="MZ_UNIT" />
                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                    <asp:BoundField DataField="MZ_TBDV" HeaderText="職序" SortExpression="MZ_TBDV" />
                    <asp:BoundField DataField="MZ_PRRST" HeaderText="獎懲結果" SortExpression="MZ_PRRST" />
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
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM &quot;A_SWT3&quot;">
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
