<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_REST_RULE.aspx.cs" Inherits="TPPDDB._3_forleave.C_REST_RULE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table class="style10">
        <tr>
            <td class="style8">
                公務人員請假規則
            </td>
            <td class="style4">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style6">
                    <tr>
                        <td class="style7">
                            請假規條號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_RULE_NO" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            請假規則條文
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_RULE_DES" runat="server" Height="122px" Style="text-align: left"
                                TextMode="MultiLine" Width="699px" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table class="style12">
                    <tr>
                        <td>
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="style9" CausesValidation="False" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                CssClass="style9" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Visible="False" CssClass="style9" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Visible="False" CssClass="style9" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" Visible="False" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);" CssClass="style9"/>
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" CssClass="style9"/>
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" 
                                CssClass="style9" Enabled="False" meta:resourcekey="btUpperResource1" 
                                OnClick="btUpper_Click" Text="上一筆" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" 
                                Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" 
                                CssClass="style9" Enabled="False" meta:resourcekey="btNEXTResource1" 
                                OnClick="btNEXT_Click" Text="下一筆" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
