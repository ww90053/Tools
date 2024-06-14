<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYPEOPLESET1.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYPEOPLESET1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../10-knowledge/styles/J_style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Height="500px" Width="400px" ScrollBars="Vertical"
                CssClass="modalPopup" Style="display: none;" BorderStyle="Groove">
                <%--  <div>
                    <table width="330">
                        <tr>
                            <td style="text-align: right">
                                姓名：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_NAME_S" runat="server"></asp:TextBox>
                                <asp:Button ID="Button_NS_B" runat="server" OnClick="Button_NS_B_Click" Text="查詢" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                身分證字號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_IDNO_S" runat="server"></asp:TextBox>
                                <asp:Button ID="Button_IS_B" runat="server" OnClick="Button_IS_B_Click" Text="查詢" />
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="MZ_ID" EmptyDataText="無符合條件之資料" ForeColor="#333333" HorizontalAlign="Center"
                    OnRowCommand="GridView1_RowCommand" Width="320px">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                        <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                        <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" ReadOnly="True" SortExpression="MZ_ID" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:Button ID="Button2" runat="server" Text="離開" />
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" GroupingText="請輸入查詢日期"
                Style="display: none;">
                <table class="style1">
                    <tr>
                        <td class="style2" style="text-align: right">
                            日期：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button_FIND" runat="server" Text="查詢" OnClick="Button_FIND_Click" />
                            <asp:Button ID="Button_EXIT" runat="server" Text="離開" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
                DynamicServicePath="" Enabled="True" PopupControlID="Panel1" TargetControlID="Button1">
            </cc1:ModalPopupExtender>
            <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" />
            <table class="style10">
                <tr>
                    <td class="style8">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <table class="style12">
                <tr>
                    <td>
                        <asp:Button ID="Button_SEARCH" runat="server" Text="查詢" class="style9" />
                        <cc1:ModalPopupExtender ID="Button_SEARCH_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="Panel2" TargetControlID="Button_SEARCH"
                            CancelControlID="Button_EXIT">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" class="style9" />
<%--                        <asp:Button ID="Button_CANCEL" runat="server" OnClick="Button_CANCEL_Click" Text="取消動作"
                            class="style9" />
                        <asp:Button ID="btReset" runat="server" Text="重設" OnClick="btReset_Click" class="style9" />--%>
                        <asp:Button ID="Button_all" runat="server" OnClick="Button_all_Click" Text="產生全部"
                            class="style9" />
                        <asp:Button ID="btn_Print" runat="server" class="style9" Text="列印輪番表" OnClick="btn_Print_Click" />
                    </td>
                </tr>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Print" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
