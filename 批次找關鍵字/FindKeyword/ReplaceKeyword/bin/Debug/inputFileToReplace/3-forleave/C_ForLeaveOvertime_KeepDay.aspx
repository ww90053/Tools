<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_KeepDay.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_KeepDay" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style111
        {
            border: solid 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            保留假及併計年資修改
                        </td>
                        <td class="style4">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style5">
                            年度：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="txt_Year" runat="server" MaxLength="3" Width="35px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            身分證字號：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="true" OnTextChanged="TextBox_MZ_ID_TextChanged"
                                Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            編制機關：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10" CssClass="style133"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            編制單位：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" OnClick="btUNIT_Click"
                                Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" Width="105px" CssClass="style111"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style12">
                    <tr>
                        <td>
                            <asp:Button ID="Button_Search" runat="server" Text="查詢" OnClick="Button_Search_Click"
                                class="style9" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server" Width="800px" Height="310px" ScrollBars="Horizontal">
                    <asp:GridView ID="GridView1" runat="server" Width="1000px" CellPadding="4" EnableEmptyContentRender="True"
                        GridLines="None" AutoGenerateColumns="False" EmptyDataText="無資料" Style="text-align: left; margin-top: 0px;" 
                        AllowPaging="True" DataKeyNames="MZ_YEAR"
                        OnPageIndexChanging="GridView1_PageIndexChanging"
                        ForeColor="#333333" PageSize="8">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField HeaderText="年度" DataField="MZ_YEAR">
                                <ItemStyle Width="35px" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="姓名" DataField="MZ_NAME">
                                <ItemStyle Width="70px" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="身分證號" DataField="MZ_ID">
                                <ItemStyle Width="85px" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="保留假1 天">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_SDAY" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_SDAY") %>' OnTextChanged="TextBox_MZ_SDAY_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_SDAY_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_SDAY">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留假1 時">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_SDAY_HOUR" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_SDAY_HOUR") %>' OnTextChanged="TextBox_MZ_SDAY_HOUR_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_SDAY_HOUR_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_SDAY_HOUR">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留假2 天">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_SDAY2" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_SDAY2") %>' OnTextChanged="TextBox_MZ_SDAY2_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_SDAY2_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_SDAY2">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留假2 時">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_SDAY2_HOUR" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_SDAY2_HOUR") %>' OnTextChanged="TextBox_MZ_SDAY2_HOUR_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_SDAY2_HOUR_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_SDAY2_HOUR">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留假 天">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_SDAY3" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_SDAY3") %>' OnTextChanged="TextBox_MZ_SDAY3_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_SDAY3_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_SDAY3">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留假 時">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_SDAY3_HOUR" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_SDAY3_HOUR") %>' OnTextChanged="TextBox_MZ_SDAY3_HOUR_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_SDAY3_HOUR_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_SDAY3_HOUR">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="併計年">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_TYEAR" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_TYEAR") %>' OnTextChanged="TextBox_MZ_TYEAR_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_TYEAR_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_TYEAR">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="併計月">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_TMONTH" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_TMONTH") %>' OnTextChanged="TextBox_MZ_TMONTH_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_TMONTH_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_TMONTH">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="併計年資相關文件">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_MEMO" runat="server" Width="120px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_MEMO") %>' OnTextChanged="TextBox_MZ_MEMO_TextChanged"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="120px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="減年資年">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_RYEAR" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_RYEAR") %>' 
                                        OnTextChanged="TextBox_MZ_RYEAR_TextChanged" Height="19px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_RYEAR_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_RYEAR">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="減年資月">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox_MZ_RMONTH" runat="server" Width="30px" AutoPostBack="True"
                                        Text='<%# Bind("MZ_RMONTH") %>' OnTextChanged="TextBox_MZ_RMONTH_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_RMONTH_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="TextBox_MZ_RMONTH">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
