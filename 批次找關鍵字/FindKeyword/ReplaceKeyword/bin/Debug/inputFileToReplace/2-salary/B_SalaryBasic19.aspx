<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic19.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic19" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
            font-size: 16pt;
            font-family: 標楷體;
        }
        .style3
        {
            text-align: right;
        }
        .style4
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style1">
                <tr>
                    <td class="style2" style="background-color: #6699FF; color: White;">
                        通匯
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_GR" runat="server" Height="430px">
                <table class="style1" border="1">
                    <tr>
                        <td>
                            <asp:GridView ID="GridView_ER" runat="server" BackColor="White" BorderColor="#999999"
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" DataSourceID="SqlDataSource_ER" OnRowCommand="GridView_ER_RowCommand"
                                DataKeyNames="ER_SNID">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:ButtonField CommandName="btUpdate" HeaderText="修改" Text="修改" Visible="false" />
                                    <asp:ButtonField CommandName="btDelete" HeaderText="刪除" Text="刪除" Visible="false" />
                                    <asp:BoundField DataField="NAME" HeaderText="分局" SortExpression="NAME" />
                                    <asp:BoundField DataField="DA" HeaderText="存款日期" SortExpression="DA" />
                                    <asp:BoundField DataField="PAY" HeaderText="金額" SortExpression="PAY" DataFormatString="{0:$#,#0}" />
                                    <asp:BoundField DataField="NOTE" HeaderText="事項" SortExpression="NOTE" />
                                    <asp:BoundField DataField="PAY_NO" HeaderText="領據編號" SortExpression="PAY_NO" />
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource_ER" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ER_SNID, NAME, DA, PAY, NOTE, PAY_NO FROM B_EXCHANGE_RATE ER, B_EXCHANGE_RATE_BRANCH ERB WHERE ER.PAY_ID = ERB.PAY_ID">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel_con" runat="server" Height="430px">
                <table class="style1">
                    <tr>
                        <td class="style4">
                            日期<asp:TextBox ID="TextBox_DAs" runat="server" Width="80px" AutoPostBack="True" 
                                ontextchanged="TextBox_DAs_TextChanged"></asp:TextBox>
                            存款事項<asp:TextBox ID="TextBox_NOTEs" runat="server" Width="500px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox_JOIN" runat="server" Text="加入新增" AutoPostBack="True" 
                                OnCheckedChanged="CheckBox_JOIN_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <br />
                <table class="style1" border="1">
                    <tr>
                        <td class="style3">
                            分局
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_PAY_ID" runat="server" Width="50px" AutoPostBack="True"
                                OnTextChanged="TextBox_PAY_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style4" colspan="2">
                            名稱<asp:TextBox ID="TextBox_NAME" runat="server"></asp:TextBox>
                            帳號<asp:TextBox ID="TextBox_BANK_ID" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style3">
                            日期
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_DA" runat="server" Width="80px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_DA_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_DA">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style3">
                            金額
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_PAY" runat="server" AutoPostBack="True" OnTextChanged="voidChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_PAY_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_PAY">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            存款<br />
                            事項
                        </td>
                        <td class="style4" colspan="3">
                            <asp:TextBox ID="TextBox_NOTE" runat="server" Width="85%"></asp:TextBox>
                        </td>
                        <td class="style3">
                            領據<br />
                            編號
                        </td>
                        <td class="style4" colspan="3">
                            <asp:TextBox ID="TextBox_PAY_NO" runat="server" Width="85%" AutoPostBack="True" OnTextChanged="voidChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:GridView ID="GridView_ER_V" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource_ER_V"
                                GridLines="Vertical" Width="100%" DataKeyNames="ER_SNID" OnRowCommand="GridView_ER_V_RowCommand">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:ButtonField CommandName="btUpdate" HeaderText="修改" Text="修改" />
                                    <asp:BoundField DataField="NAME" HeaderText="分局" SortExpression="NAME" />
                                    <asp:BoundField DataField="DA" HeaderText="存款日期" SortExpression="DA" />
                                    <asp:BoundField DataField="PAY" HeaderText="金額" SortExpression="PAY" DataFormatString="{0:$#,#0}" />
                                    <asp:BoundField DataField="NOTE" HeaderText="事項" SortExpression="NOTE" />
                                    <asp:BoundField DataField="PAY_NO" HeaderText="領據編號" SortExpression="PAY_NO" />
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource_ER_V" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style1">
                <tr>
                    <td style="background-color: #6699FF; color: White;">
                        <asp:Button ID="btC" runat="server" OnClick="btC_Click" Text="畫面切換" />
                        <asp:Button ID="btCreate" runat="server" Text="新增" OnClick="btCreate_Click" />
                        <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" />
                        <asp:Button ID="btExit" runat="server" Text="取消" OnClick="btExit_Click" />
                        <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
