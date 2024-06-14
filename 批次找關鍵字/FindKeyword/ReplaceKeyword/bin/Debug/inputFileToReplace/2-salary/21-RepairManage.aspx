<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="21-RepairManage.aspx.cs" Inherits="TPPDDB._2_salary._1_RepairManage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>'; 
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="background-color: #6699FF; color: White; text-align: left; font-size: 16pt;
        font-family: 標楷體;">
        補發薪資管理</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                Width="200px" Height="100px" Style="display: none;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="300">
                    <ProgressTemplate>
                        <div style="position: relative; top: 40%; text-align: center;">
                            <img src="/images/loading.gif" style="vertical-align: middle" alt="Processing" />
                            處理中 ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground"
                CancelControlID="ibt_close">
            </cc1:ModalPopupExtender>
            <table class="TableStyleBlue" style="width: 90%;">
                <tr>
                    <th>
                        發薪機關
                    </th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_payad" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        年月
                    </th>
                    <td>
                        <asp:TextBox ID="txt_amonth" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                    </td>
                    <th>
                        批號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_batch" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        員工編號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_polno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                    </td>
                    <th>
                        姓名
                    </th>
                    <td>
                        <asp:TextBox ID="txt_name" runat="server" MaxLength="5" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <asp:Button ID="btn_search" runat="server" Text="查詢" 
                            onclick="btn_search_Click" />
                    </td>
                </tr>
            </table>
            <div style="width: 90%; text-align: right;">
                <asp:Button ID="btn_add" runat="server" Text="新增" style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_add_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_add" PopupControlID="pl_add" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
            </div>
            <div style="width: 90%;">
                <asp:GridView ID="gv_search" runat="server" CssClass="Grid1" Width="100%" 
                    AutoGenerateColumns="False" onrowcommand="gv_search_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="年月" />
                        <asp:BoundField HeaderText="批號" />
                        <asp:BoundField HeaderText="員工編號" />
                        <asp:BoundField HeaderText="姓名" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="btn_update" runat="server" CausesValidation="false" CommandName="upd" CommandArgument='<%# Eval("R_SNID") %>'
                                    Text="修改" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel ID="pl_add" runat="server" CssClass="DivPanel" Style="width: 90%;">
                <div style="width: 100%; text-align: right;">
                    <asp:ImageButton ID="ibt_close" runat="server" ImageUrl="/images/Back.gif" />
                </div>
                <table class="TableStyleBlue" style="width: 100%;">
                    <tr>
                        <th>
                            年月
                        </th>
                        <td>
                            <asp:Label ID="lb_addAmonth" runat="server" Text=""></asp:Label>
                        </td>
                        <th>
                            批號
                        </th>
                        <td colspan="3">
                            <asp:Label ID="lb_addBatch" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            員工編號
                        </th>
                        <td>
                            <asp:Label ID="lb_polno" runat="server"></asp:Label>
                        </td>
                        <th>
                            身分證號
                        </th>
                        <td>
                            <asp:Label ID="lb_idcard" runat="server"></asp:Label>
                        </td>
                        <th>
                            姓名
                        </th>
                        <td>
                            <asp:Label ID="lb_name" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            補發日期
                        </th>
                        <td colspan="5">
                            <asp:TextBox ID="txt_sdate" runat="server" MaxLength="7" Width="80px"></asp:TextBox>~
                            <asp:TextBox ID="txt_edate" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            補發型態
                        </th>
                        <td colspan="5">
                            <asp:DropDownList ID="ddl_type" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="1">1.新進人員</asp:ListItem>
                                <asp:ListItem Value="2">2.調入</asp:ListItem>
                                <asp:ListItem Value="3">3.薪級變更</asp:ListItem>
                                <asp:ListItem Value="4">4.考績晉級</asp:ListItem>
                                <asp:ListItem Value="5">5.考績晉階</asp:ListItem>
                                <asp:ListItem Value="6">6.考績晉階晉級</asp:ListItem>
                                <asp:ListItem Value="7">7.其他</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            薪俸職等
                        </th>
                        <td>
                            原
                            <asp:DropDownList ID="ddl_SSrank" runat="server">
                            </asp:DropDownList>
                            新
                            <asp:DropDownList ID="ddl_ESrank" runat="server">
                            </asp:DropDownList>
                        </td>
                        <th>
                            俸點
                        </th>
                        <td colspan="3">
                            原<asp:TextBox ID="TextBox_SMZ_SPT" runat="server" Width="50px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                ID="TextBox_SMZ_SPT_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers"
                                TargetControlID="TextBox_SMZ_SPT">
                            </cc1:FilteredTextBoxExtender>
                            新<asp:TextBox ID="TextBox_EMZ_SPT" runat="server" Width="50px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                ID="TextBox_EMZ_SPT_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers"
                                TargetControlID="TextBox_EMZ_SPT">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            去年考績
                        </th>
                        <td colspan="5">
                            <asp:TextBox ID="txt_Grade" runat="server" MaxLength="1" Width="20px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            應發總計
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_INCREASE" runat="server"></asp:TextBox>
                        </td>
                        <th>
                            應扣總計
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_DECREASE" runat="server"></asp:TextBox>
                        </td>
                        <th>
                            實發金額
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_TPAY" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            備考
                        </th>
                        <td colspan="5">
                            <asp:TextBox ID="TextBox_NOTE" runat="server" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="TableStyleBlue" style="width: 100%;">
                    <tr>
                        <th rowspan="5" style="width: 70px;">
                            應發
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 100px;">
                            月支數額
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_SALARYPAY1" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th style="width: 100px;">
                            主管加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_BOSS" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th style="width: 100px;">
                            專業加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_PROFESS" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            警勤加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_WORKP" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            技術加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_TECHNICS" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            工作獎助金
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_BONUS" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            外事加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_ADVENTIVE" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            偏遠加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_FAR" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            勤務繁重加給
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_ELECTRIC" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            其他應發
                        </th>
                        <td colspan="5">
                            <asp:TextBox ID="TextBox_SALARY_2_INCREASE" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="TableStyleBlue" style="width: 100%;">
                    <tr>
                        <th rowspan="5" style="width: 70px;">
                            應扣
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 100px;">
                            健保年功俸
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_HEALTHID" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th style="width: 100px;">
                            健保眷口數
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_HEALTHMAN" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th style="width: 100px;">
                            健保費
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_HEALTHPAY" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            健保費扣款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_HEALTHPAY1" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            補扣月數
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_HEALTHPAY_COUNT" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            薪資扣款1
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MONTHPAY_TAX" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            薪資扣款2
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MONTHPAY" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            薪津所得稅
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_TAX" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            公(勞)保費
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_INSURANCEPAY" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            退撫金費
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_CONCUR3PAY" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            其他應扣
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_SALARY_2_DECREASE" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="TableStyleBlue" style="width: 100%;">
                    <tr>
                        <th rowspan="4" style="width: 70px;">
                            其他應扣
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 100px;">
                            法院扣款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA01" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th style="width: 100px;">
                            國宅貸款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA02" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th style="width: 100px;">
                            銀行貸款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA03" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            分期付款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA04" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            優惠存款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA05" runat="server" Width="70px" Height="19px"></asp:TextBox>
                        </td>
                        <th>
                            員工宿舍費
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA06" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            伙食費
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA07" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            福利互助金費
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA08" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <th>
                            退撫金貸款
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXTRA09" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button ID="btn_save" runat="server" Text="儲存" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
