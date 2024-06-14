<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SalaryBasic15.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic15" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
                公保費設定</td>
        </tr>
        <tr>
            <td class="style2">
                公保費代碼</td>
            <td class="style3">
                <asp:TextBox ID="TextBox_ID" runat="server"></asp:TextBox>
            </td>
            <td class="style2">
                公保費</td>
            <td class="style3">
                <asp:TextBox ID="TextBox_INSURANCE" runat="server" AutoPostBack="True" 
                    ontextchanged="voidChanged"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_INSURANCE_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                    TargetControlID="TextBox_INSURANCE" ValidChars="-$,.">
                </cc1:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="style2">
                薪資月額起點</td>
            <td class="style3">
                <asp:TextBox ID="TextBox_PAY1" runat="server" ontextchanged="voidChanged" 
                    AutoPostBack="True"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_PAY1_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                    TargetControlID="TextBox_PAY1" ValidChars="-$,.">
                </cc1:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                薪資月額終點</td>
            <td class="style3">
                <asp:TextBox ID="TextBox_PAY2" runat="server" ontextchanged="voidChanged" 
                    AutoPostBack="True"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_PAY2_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                    TargetControlID="TextBox_PAY2" ValidChars="-$,.">
                </cc1:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:GridView ID="GridView_GOV_INSURANCE" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSource_GOV_INSURANCE" 
                    GridLines="None" onrowcommand="GridView_GOV_INSURANCE_RowCommand" 
                    Width="100%" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="代碼" SortExpression="ID" />
                        <asp:BoundField DataField="INSURANCE" HeaderText="公保費金額" SortExpression="INSURANCE" DataFormatString="{0:$#,#0}" />
                        <asp:BoundField DataField="PAY1" HeaderText="薪資月額起點" SortExpression="PAY1" DataFormatString="{0:$#,#0}" />
                        <asp:BoundField DataField="PAY2" HeaderText="薪資月額終點" SortExpression="PAY2" DataFormatString="{0:$#,#0}" />
                        <asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" Visible="false" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource_GOV_INSURANCE" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                    SelectCommand="SELECT ID, INSURANCE, PAY1, PAY2, CREATEDATE FROM B_GOV_INSURANCE ORDER BY CREATEDATE DESC">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="background-color: #6699FF; color: White;">
                <asp:Button ID="btCreate" runat="server" onclick="btCreate_Click" Text="新增" />
                <asp:Button ID="btUpdate" runat="server" onclick="btUpdate_Click" Text="修改" 
                    Visible="False" />
                <asp:Button ID="btExit" runat="server" onclick="btExit_Click" Text="取消" 
                    Visible="False" />
                <asp:Button ID="btOLD_DATA" runat="server" onclick="btOLD_DATA_Click" 
                    Text="轉入歷史檔" />
                <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
