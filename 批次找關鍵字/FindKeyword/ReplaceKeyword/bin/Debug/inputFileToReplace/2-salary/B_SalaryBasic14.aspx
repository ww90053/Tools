<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic14.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic14" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: right;
        }
        .style3
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
            <table border="1" class="style1">
                <tr>
                    <td colspan="4" style="text-align: left; font-size: 16pt; font-family: 標楷體; background-color: #6699FF;
                        color: White;">
                        勞保費設定
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        勞保費代碼
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_ID" runat="server" MaxLength="2"></asp:TextBox>
                    </td>
                    <td class="style2">
                        勞保級距金額
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_PAY1" runat="server" AutoPostBack="True" OnTextChanged="voidChanged"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="TextBox_PAY1_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_PAY1" ValidChars="-$,.">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        勞保費
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_INSURANCE" runat="server" AutoPostBack="True" OnTextChanged="voidChanged"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="TextBox_INSURANCE_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_INSURANCE"
                            ValidChars="-$,.">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="height: 380px; overflow: scroll;">
                            <asp:GridView ID="GridView_LABOR_INSURANCE" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSource_LABOR_INSURANCE"
                                GridLines="None" OnRowCommand="GridView_HEALTH_INSURANCE_RowCommand" Width="770px"
                                ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="代碼" SortExpression="ID" />
                                    <asp:BoundField DataField="PAY1" HeaderText="勞保級距金額" SortExpression="PAY1" DataFormatString="{0:$#,#0}" />
                                    <asp:BoundField DataField="INSURANCE" HeaderText="勞保費金額" SortExpression="INSURANCE"
                                        DataFormatString="{0:$#,#0}" />
                                    <asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" Visible="false" />
                                    <asp:ButtonField CommandName="btDelete" HeaderText="刪除" Text="刪除" Visible="False" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="SqlDataSource_LABOR_INSURANCE" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="background-color: #6699FF; color: White;">
                        <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="新增" />
                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Visible="False" />
                        <asp:Button ID="btExit" runat="server" OnClick="btExit_Click" Text="取消" Visible="False" />
                        <asp:Button ID="btOLD_DATA" runat="server" OnClick="btOLD_DATA_Click" Text="轉入歷史檔" />
                        <asp:DropDownList ID="DropDownList_OLD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_OLD_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="NEW">現存資料</asp:ListItem>
                            <asp:ListItem Value="OLD">歷史資料</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
