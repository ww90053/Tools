<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic16.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic16" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
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
            <div class="PageTitle">
                其他金額
            </div>
            <div style="height: 450px; overflow: scroll;">
                <table border="1" class="style1">
                    <tr>
                        <td class="style2">
                            其他金額代碼
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_ID" runat="server" MaxLength="4"></asp:TextBox>
                        </td>
                        <td class="style2">
                            其他金額名稱
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_NAME" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            其他金額金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_PAY" runat="server" AutoPostBack="True" OnTextChanged="voidChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_PAY_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_PAY" ValidChars="-$,.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style2">
                            其他金額應扣(發)
                        </td>
                        <td class="style3">
                            <asp:RadioButtonList ID="RadioButtonList_MP" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="P">應發</asp:ListItem>
                                <asp:ListItem Value="M">應扣</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="GridView_SALARY_2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="ID" DataSourceID="SqlDataSource_SALARY_2" GridLines="None" OnRowCommand="GridView_SALARY_2_RowCommand"
                                Width="100%" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="代碼" SortExpression="ID" />
                                    <asp:BoundField DataField="NAME" HeaderText="名稱" SortExpression="NAME" />
                                    <asp:BoundField DataField="PAY" HeaderText="金額" SortExpression="PAY" DataFormatString="{0:$#,#0}" />
                                    <asp:BoundField DataField="MP" HeaderText="應扣(發)" SortExpression="MP" />
                                    <asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" Visible="false" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource_SALARY_2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, MP, CREATEDATE FROM B_SALARY_2 ORDER BY CREATEDATE DESC">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="background-color: #6699FF; color: White;">
                            <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="新增" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Visible="False" />
                            <asp:Button ID="btExit" runat="server" OnClick="btExit_Click" Text="取消" />
                            <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
