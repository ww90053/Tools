<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_bussinesstrip.aspx.cs" Inherits="TPPDDB._3_forleave.C_bussinesstrip" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style>
        .style112
        {
            text-align: left;
            width: 55px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .popup_outside
        {
            border: solid 1px gray;
            background-color: LightBlue;
        }
        .popup_inside
        {
            margin: 10px;
            background-color: White;
        }
        .style113
        {
            text-align: left;
            width: 70px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style114
        {
            text-align: left;
            width: 111px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style115
        {
            text-align: left;
            width: 70px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            height: 25px;
        }
        .style116
        {
            height: 25px;
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
                            身份證號
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_ID" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style112">
                            姓名
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_NAME" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style112">
                            請假日期
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_DATE" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style112">
                            請假事由
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_MZ_CAUSE" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style114">
                            出差（旅遊）地點
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lbl_MZ_TADD" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style12">
                <tr>
                    <td>
                        <asp:Button ID="bt_Search" runat="server" Text="查詢" CssClass="style9" CausesValidation="False"
                            OnClick="bt_Search_Click" />
                        <asp:Button ID="bt_Insert" runat="server" Text="新增" CssClass="style9" CausesValidation="False"
                            OnClick="btInsert_Click" Enabled="false" />
                        <asp:Label ID="lbl_MODE" runat="server" Visible="false"></asp:Label>
                        <asp:Button ID="btn_onlinecheck" runat="server" Enabled="false" CssClass="style9"
                            Text="送核" onclick="btn_onlinecheck_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_EachDay" runat="server" Style="display:none;">
                <div class="popup_outside" style="width: 600px; height: 440px;">
                    <div class="popup_inside" style="width: 580px; height: 420px">
                        <div style="padding-top: 16px; font-weight: 700; color: #196E8A;">
                            <div style="text-align: center">
                                <asp:Label ID="lbl_Title" runat="server" Text="Label"></asp:Label>
                                <table width="100%" border="1" class="style6">
                                    <tr>
                                        <td class="style113">
                                            月
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_MONTH" runat="server" Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style115">
                                            日
                                        </td>
                                        <td style="text-align: left" class="style116">
                                            <asp:TextBox ID="txt_DAY" runat="server" Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style115">
                                            起迄地點
                                        </td>
                                        <td style="text-align: left" class="style116">
                                            <asp:TextBox ID="txt_LOCATION" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            工作記要
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_WORKNOTES" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            飛機
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_AIRPLANE" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="txt_Count_Total_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            汽車及捷運
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_CARMRT" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="txt_Count_Total_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            火車
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_TRAIN" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="txt_Count_Total_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            輪船
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_BOAT" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="txt_Count_Total_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            住宿費
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_ACCOMMODATION" runat="server" Width="75px" AutoPostBack="True"
                                                OnTextChanged="txt_Count_Total_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            膳雜費
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_Charges" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="txt_Count_Total_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style113">
                                            總計
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txt_TOTAL" runat="server" Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" colspan="2">
                                            <asp:Button ID="btn_save" runat="server" Text="確定" OnClick="btn_save_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btn_clear" runat="server" Text="離開" OnClick="btn_clear_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" />
            <cc2:ModalPopupExtender ID="btn_fake_ModalPopupExtender" runat="server" DynamicServicePath=""
                Enabled="True" TargetControlID="Button1" PopupControlID="Panel_EachDay" CancelControlID="">
            </cc2:ModalPopupExtender>
            <asp:GridView ID="gv_BUSSINESSTRIP" runat="server" BackColor="White" Width="100%"
                BorderColor="#999999" DataKeyNames="SN,BUSSINESSTRIP_STATUS" 
                BorderStyle="None" BorderWidth="1px"
                CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False" 
                OnRowCommand="gv_BUSSINESSTRIP_RowCommand" 
                onrowdatabound="gv_BUSSINESSTRIP_RowDataBound" >
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <Columns>
                    <asp:BoundField HeaderText="月" DataField="MONTH"></asp:BoundField>
                    <asp:BoundField HeaderText="日" DataField="DAY"></asp:BoundField>
                    <asp:BoundField HeaderText="起迄地點" DataField="LOCATION"></asp:BoundField>
                    <asp:BoundField HeaderText="工作記要" DataField="WORKNOTES"></asp:BoundField>
                    <asp:BoundField HeaderText="交通費" DataField="B1"></asp:BoundField>
                    <asp:BoundField HeaderText="住宿費" DataField="ACCOMMODATION"></asp:BoundField>
                    <asp:BoundField HeaderText="總計" DataField="TOTAL"></asp:BoundField>
                    <asp:ButtonField ButtonType="Button" HeaderText="修改" Text="修改" CommandName="EDT">
                    </asp:ButtonField>
                    <asp:ButtonField ButtonType="Button" HeaderText="刪除" Text="刪除" CommandName='DEL'>
                    </asp:ButtonField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#DCDCDC" />
            </asp:GridView>
            <asp:Panel ID="Panel_select" runat="server" Style="display: none;">
                <div style="border: solid 1px gray; background-color: LightBlue; width: 280px; height: 290px;">
                    <div style="margin: 10px; background-color: #FFFFFF; width: 260px; height: 270px;"
                        class="style87">
                        <div style="margin: 10px;">
                            <h3>
                                陳核
                            </h3>
                            <asp:GridView ID="GV_CHECKER" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GV_CHECKER_PageIndexChanging"
                                OnRowCommand="GV_CHECKER_RowCommand" PageSize="5" EmptyDataText="查無資料">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_select" runat="server" CausesValidation="false" CommandName="checker"
                                                Text="選取" CommandArgument='<%# Eval("SN") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="100px" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <div style="text-align: center;">
                                <asp:Button ID="btn_check" runat="server" Text="送出" Style="display: none;" />
                                <asp:Button ID="btn_exit" runat="server" Text="取消" OnClick="btn_exit_Click" />
                            </div>
                            <asp:Button ID="btn_popup" runat="server" Text="Button" Style="display: none;" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="Panel_select_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                DynamicServicePath="" Enabled="True" TargetControlID="btn_popup" PopupControlID="Panel_select">
            </cc2:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
