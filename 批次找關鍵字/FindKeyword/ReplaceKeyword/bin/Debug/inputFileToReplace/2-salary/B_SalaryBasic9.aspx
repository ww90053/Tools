﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic9.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic9" %>

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
                        偏遠加給
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        偏遠加給代碼
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_ID" runat="server" MaxLength="2"></asp:TextBox>
                    </td>
                    <td class="style2">
                        偏遠加給名稱
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_NAME" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        偏遠加給金額
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_PAY" runat="server" AutoPostBack="True" OnTextChanged="voidChanged"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="TextBox_PAY_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_PAY" ValidChars="-$,.">
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
                        <asp:GridView ID="GridView_FAR" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" DataKeyNames="ID"
                            DataSourceID="SqlDataSource_FAR" GridLines="None" OnRowCommand="GridView_ADVENTIVE_RowCommand"
                            Width="100%" ForeColor="#333333">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="代碼" SortExpression="ID" />
                                <asp:BoundField DataField="NAME" HeaderText="名稱" SortExpression="NAME" />
                                <asp:BoundField DataField="PAY" HeaderText="金額" SortExpression="PAY" DataFormatString="{0:$#,#0}" />
                                <asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" Visible="false" />
                            </Columns>
                            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource_FAR" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE FROM B_FAR ORDER BY ID ASC">
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="background-color: #6699FF; color: White;">
                        <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="新增" />
                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Visible="False" />
                        <asp:Button ID="btExit" runat="server" OnClick="btExit_Click" Text="取消" Visible="False" />
                        <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
