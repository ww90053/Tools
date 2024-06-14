<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYMODE_SET.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYMODE_SET" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style58
        {
            text-align: left;
            width: 69px;
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
            <asp:Panel ID="Panel3" runat="server">
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
                                勤務代號
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="TextBox_DUTYMODE_NO" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"
                                    Style="height: 19px" Width="100px"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#0033FF"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                勤務說明
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="TextBox_DUTYMODE_NAME" runat="server" Width="600px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                無線電代號
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="TextBox_COMM_CHANNEL" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server">
                    <table class="style12">
                        <tr>
                            <td>
                                <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                    Text="新增" CausesValidation="False" class="style9" AccessKey="a" />
                                <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                    Enabled="False" class="style9" />
                                <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" class="style9"
                                    Text="確定" AccessKey="s" />
                                <asp:Button ID="btCancel" runat="server" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                                    Text="取消" CausesValidation="False" Enabled="False" class="style9" />
                                <asp:Button ID="btDelete" runat="server" CausesValidation="False" meta:resourcekey="btDeleteResource1"
                                    OnClick="btDelete_Click" Text="刪除" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                    Enabled="False" class="style9" AccessKey="d" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT DUTY_NO||DUTY_NAME, DUTY_NO FROM C_DUTYITEM ORDER BY DUTY_NO">
                </asp:SqlDataSource>
                <cc1:TBGridView ID="GridView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1"
                    EnableEmptyContentRender="True" GridLines="None" Width="100%" AutoGenerateColumns="False"
                    DataKeyNames="DUTYMODE_NO" OnRowCommand="GridView1_RowCommand" AllowPaging="True"
                    OnRowDataBound="GridView1_RowDataBound" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="DUTYMODE_NO" HeaderText="勤務方式編號" ReadOnly="True" SortExpression="DUTYMODE_NO">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DUTYMODE_NAME" HeaderText="勤務方式名稱" SortExpression="DUTYMODE_NAME">
                            <ItemStyle Width="75%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="COMM_CHANNEL" HeaderText="通訊代號" ReadOnly="True" SortExpression="COMM_CHANNEL">
                            <ItemStyle Width="10%" />
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
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM &quot;C_DUTYMODE&quot;  WHERE MZ_AD=@MZ_AD AND MZ_UNIT=@MZ_UNIT  ORDER BY &quot;DUTY_NO&quot;">
                    <SelectParameters>
                        <asp:SessionParameter Name="MZ_AD" SessionField="ADPMZ_EXAD" />
                        <asp:SessionParameter Name="MZ_UNIT" SessionField="ADPMZ_EXUNIT" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
