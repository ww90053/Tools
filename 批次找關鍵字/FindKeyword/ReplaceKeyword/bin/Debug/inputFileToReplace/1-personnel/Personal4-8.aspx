<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal4-8.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_8" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 70px;
        }
        .style3
        {
            text-align: center;
        }
        .style4
        {
            text-align: left;
            font-size: large;
            font-family: �з���;
            width: 406px;
        }
        .style5
        {
            text-align: right;
            font-size: large;
            font-family: �з���;
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
                    <td class="style4">
                        �`�Τ��y
                    </td>
                    <td class="style5">
                        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_NOTE" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ���y�N��
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_NOTE" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ���y���e
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_NOTE_NAME" runat="server" Height="135px" TextMode="MultiLine"
                                Width="705px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td class="style3">
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="�d��" CausesValidation="False" Visible="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="�s�W" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="�ק�" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="�T�w" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="����" CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="�R��" CausesValidation="False" OnClientClick="return confirm(&quot;�T�w�R���H&quot;);"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Height="315px">
                <cc1:TBGridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" DataSourceID="SqlDataSource1" EnableEmptyContentRender="True"
                    GridLines="None" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                    Style="text-align: left" Width="100%" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_NOTE" HeaderText="���y�N��" SortExpression="MZ_NOTE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="75px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_NOTE_NAME" HeaderText="���y�W��" SortExpression="MZ_NOTE_NAME">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
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
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM &quot;A_NOTE&quot; WHERE MZ_NOTE=@MZ_NOTE">
                    <SelectParameters>
                        <asp:SessionParameter Name="MZ_NOTE" SessionField="ADPMZ_ID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
