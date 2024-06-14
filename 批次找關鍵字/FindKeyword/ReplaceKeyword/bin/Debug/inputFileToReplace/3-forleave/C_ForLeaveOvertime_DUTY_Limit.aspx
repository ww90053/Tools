<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTY_Limit.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTY_Limit" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style>
        .style111
        {
            border: solid 0px;
        }
        .style112
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
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style112">
                            發薪機關
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10" CssClass="style133"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="300px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="211px" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            身分證號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_ID_TextChanged"
                                Style="height: 19px" Width="100px"></asp:TextBox>
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#0033FF"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            時數上限
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_HOUR_LIMIT" runat="server" Width="100px" MaxLength="3"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_HOUR_LIMIT_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_HOUR_LIMIT">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            金額上限
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_MONEY_LIMIT" runat="server" Width="100px" MaxLength="6"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_MONEY_LIMIT_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_MONEY_LIMIT">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style12">
                <tr>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" 
                            Text="查詢" CssClass="style9" CausesValidation="False" AccessKey="a" 
                            onclick="btnSearch_Click" />
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
                        <asp:Button ID="btCaluLimit" runat="server" CausesValidation="False" Text="計算"
                            Enabled="True" class="style9" AccessKey="d" onclick="btCaluLimit_Click" />
                    </td>
                </tr>
            </table>
            <cc1:TBGridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                CellPadding="4" DataKeyNames="MZ_AD,MZ_UNIT,MZ_ID,MZ_UNIT1,AD" DataSourceID="SqlDataSource1"
                EnableEmptyContentRender="True" GridLines="None" OnRowCommand="GridView1_RowCommand"
                OnRowDataBound="GridView1_RowDataBound" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_AD" HeaderText="機關" ReadOnly="True" SortExpression="MZ_AD" />
                    <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" ReadOnly="True" SortExpression="MZ_UNIT" />
                    <asp:BoundField DataField="MZ_ID" HeaderText="姓名" ReadOnly="True" SortExpression="MZ_ID" />
                    <asp:BoundField DataField="MZ_HOUR_LIMIT" HeaderText="時數上限" SortExpression="MZ_HOUR_LIMIT" />
                    <asp:BoundField DataField="MZ_MONEY_LIMIT" HeaderText="金額上限" SortExpression="MZ_MONEY_LIMIT" />
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
                DeleteCommand="DELETE FROM &quot;C_DUTYLIMIT&quot; WHERE &quot;MZ_AD&quot; = @MZ_AD AND &quot;MZ_UNIT&quot; = @MZ_UNIT AND &quot;MZ_ID&quot; = @MZ_ID"
                InsertCommand="INSERT INTO &quot;C_DUTYLIMIT&quot; (&quot;MZ_AD&quot;, &quot;MZ_UNIT&quot;, &quot;MZ_ID&quot;, &quot;MZ_HOUR_LIMIT&quot;, &quot;MZ_MONEY_LIMIT&quot;) VALUES (@MZ_AD, @MZ_UNIT, @MZ_ID, @MZ_HOUR_LIMIT, @MZ_MONEY_LIMIT)"
                SelectCommand=""
                UpdateCommand="UPDATE &quot;C_DUTYLIMIT&quot; SET &quot;MZ_HOUR_LIMIT&quot; = @MZ_HOUR_LIMIT, &quot;MZ_MONEY_LIMIT&quot; = @MZ_MONEY_LIMIT WHERE &quot;MZ_AD&quot; = @MZ_AD AND &quot;MZ_UNIT&quot; = @MZ_UNIT AND &quot;MZ_ID&quot; = @MZ_ID"
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>">
                <SelectParameters>
                    <asp:SessionParameter Name="MZ_AD" SessionField="ADPMZ_AD" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="MZ_AD" Type="String" />
                    <asp:Parameter Name="MZ_UNIT" Type="String" />
                    <asp:Parameter Name="MZ_ID" Type="String" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="MZ_HOUR_LIMIT" Type="Decimal" />
                    <asp:Parameter Name="MZ_MONEY_LIMIT" Type="Decimal" />
                    <asp:Parameter Name="MZ_AD" Type="String" />
                    <asp:Parameter Name="MZ_UNIT" Type="String" />
                    <asp:Parameter Name="MZ_ID" Type="String" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="MZ_AD" Type="String" />
                    <asp:Parameter Name="MZ_UNIT" Type="String" />
                    <asp:Parameter Name="MZ_ID" Type="String" />
                    <asp:Parameter Name="MZ_HOUR_LIMIT" Type="Decimal" />
                    <asp:Parameter Name="MZ_MONEY_LIMIT" Type="Decimal" />
                </InsertParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
