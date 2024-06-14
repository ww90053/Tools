<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_Change_Dltbb.aspx.cs" Inherits="TPPDDB._3_forleave.C_Change_Dltbb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="100000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="65%">
                <tr>
                    <td class="title_s1">
                        應休假天數修改
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style2">
                                        身分證號：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="514px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btn_Search" runat="server" Text="查詢" OnClick="btn_Search_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" runat="server" GroupingText="查詢結果" Width="514px">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            服務機關
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label_EXAD" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            服務單位
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label_EXUNIT" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            身份證號
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label_ID" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            姓名
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label_NAME" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            職稱
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label_OCCC" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            應休假天數
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="TextBox_MZ_HDAY" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            應休假時數
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="TextBox_MZ_HTIME" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btn_Update" runat="server" Text="修改" Enabled="false" OnClick="btn_Update_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
