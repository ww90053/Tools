<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYMEMO.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYMEMO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style58
        {
            text-align: left;
            width: 55px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
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
                        <td class="style58">
                            日期
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RV_PATROL_NO" runat="server" ControlToValidate="TextBox_MZ_DATE"
                                Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style58">
                            重點記事
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_MEMO" runat="server" TextMode="MultiLine" Width="683px"
                                Height="109px" MaxLength="1000"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style12">
                <tr>
                    <td>
                        <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                            Text="新增" CssClass="style9" CausesValidation="False" AccessKey="a" />
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
                GridLines="None" AutoGenerateColumns="False" DataKeyNames="MZ_DATE,MZ_AD,MZ_UNIT"
                DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                Style="text-align: left" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_DATE" HeaderText="日期" ReadOnly="True" SortExpression="MZ_DATE">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MZ_MEMO" HeaderText="重點記事" SortExpression="MZ_MEMO" />
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
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM C_DUTYMEMO WHERE MZ_AD=@MZ_AD AND MZ_UNIT=@MZ_UNIT ">
                <SelectParameters>
                    <asp:SessionParameter Name="MZ_AD" SessionField="ADPMZ_EXAD" />
                    <asp:SessionParameter Name="MZ_UNIT" SessionField="ADPMZ_EXUNIT" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
