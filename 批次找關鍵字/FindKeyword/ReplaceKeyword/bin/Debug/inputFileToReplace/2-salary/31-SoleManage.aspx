<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="31-SoleManage.aspx.cs" Inherits="TPPDDB._2_salary._1_SoleManage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>';

    </script>

    <div class="PageTitle">
        單一發放資料維護
    </div>
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
                Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <table class="TableStyleBlue" style="width: 90%;">
                <tr>
                    <th>
                        <a class="must">*發薪機關</a>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddl_payad" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_payad_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <th>
                        單位
                    </th>
                    <td>
                        <asp:DropDownList ID="ddl_unit" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        項目
                    </th>
                    <td>
                        <asp:TextBox ID="txt_num" runat="server" AutoPostBack="true" Width="50px" OnTextChanged="txt_num_TextChanged"></asp:TextBox>
                        <asp:Label ID="lbl_num" runat="server" Text=""></asp:Label>
                        <asp:Button ID="btn_showNum" runat="server" Text="V" OnClick="btn_showNum_Click" />
                        <asp:Button ID="btn_showNumInv" runat="server" Text="Button" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="btn_showNum_ModalPopupExtender" runat="server" DynamicServicePath=""
                            Enabled="True" TargetControlID="btn_showNumInv" PopupControlID="pl_num" BackgroundCssClass="modalBackground">
                        </cc1:ModalPopupExtender>
                    </td>
                    <th>
                        <a class="must">*入帳日期</a>
                    </th>
                    <td>
                        <asp:TextBox ID="txt_da" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        批號
                    </th>
                    <td>
                        <asp:TextBox ID="tbCaseID" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                    </td>
                    <th>
                        身份證字號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_idcard" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
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
                        姓名/抬頭名稱
                    </th>
                    <td>
                        <asp:TextBox ID="txt_name" runat="server" MaxLength="5" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <asp:Button ID="btn_search" runat="server" Text="查詢" OnClick="btn_search_Click" />
                    </td>
                </tr>
            </table>
            <div style="width: 90%; text-align: left;">
                <asp:Button ID="btn_deleteChecked" runat="server" Text="刪除勾選的資料" OnClick="btn_deleteChecked_Click"
                    OnClientClick="return confirm('這樣會刪除所有勾選的資料，確定要執行？');" Style="color: Blue;" />
            </div>
            <div style="height: 300px; overflow: scroll;">
                <asp:GridView ID="gv_Result" runat="server" CssClass="Grid1" AutoGenerateColumns="False"
                    Style="width: 90%;" OnRowCommand="gv_Result_RowCommand" OnPageIndexChanging="gv_Result_PageIndexChanging"
                    EmptyDataText="查無資料" OnRowDataBound="gv_Result_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_update" runat="server" Text="修改" CommandName="upd" CommandArgument='<%# Eval("S_SNID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cb_delAll" runat="server" AutoPostBack="true" OnCheckedChanged="cb_delAll_CheckedChanged"
                                    Text="全選" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hf_sn" runat="server" Value='<%# Eval("S_SNID") %>' />
                                <asp:CheckBox ID="cb_del" runat="server" />
                                <asp:Button ID="btn_delete" runat="server" Text="刪除" CommandName="del" CommandArgument='<%# Eval("S_SNID") %>'
                                    OnClientClick="return confirm('確定要刪除？')" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="入帳日期" DataField="DA" />
                        <asp:BoundField HeaderText="批號" DataField="CASEID" />
                        <asp:BoundField HeaderText="單位" DataField="PAY_UNIT" />
                        <asp:BoundField HeaderText="員工編號" DataField="MZ_POLNO" />
                        <asp:BoundField HeaderText="姓名/抬頭名稱" DataField="NAME" />
                        <asp:BoundField HeaderText="項目" DataField="ITEM" />
                        <asp:BoundField HeaderText="實發金額" DataField="TOTAL" />
                        <asp:TemplateField HeaderText="關帳">
                            <ItemTemplate>
                                <asp:Label ID="lb_lock" runat="server" Text='<%# Eval("LOCKDB") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="備註" DataField="NOTE" />
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel ID="pl_update" runat="server" CssClass="DivPanel" Style="width: 600px;
                display: none;">
                <asp:Button ID="btn_showUpdate" runat="server" Text="Button" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showUpdate_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showUpdate" PopupControlID="pl_update" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <div style="width: 100%; text-align: right;">
                    <asp:ImageButton ID="ibt_close" runat="server" ImageUrl="/images/Back.gif" OnClick="ibt_updateClose_Click" />
                </div>
                <table class="TableStyleBlue">
                    <tr>
                        <th>
                            員工編號
                        </th>
                        <td colspan="5">
                            <asp:Label ID="lb_polno" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            身分證字號/統一編號
                        </th>
                        <td colspan="5">
                            <asp:Label ID="lb_idcard" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            姓名/抬頭名稱
                        </th>
                        <td colspan="5">
                            <asp:Label ID="lb_name" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <a class="must">*項目</a>
                        </th>
                        <td colspan="5">
                            <div>
                                <asp:TextBox ID="txt_updNum" runat="server" AutoPostBack="true" Width="50px" OnTextChanged="txt_updNum_TextChanged"></asp:TextBox>
                                <asp:Label ID="lb_updNum" runat="server" Text=""></asp:Label>
                                <asp:Button ID="btn_updNum" runat="server" Text="V" OnClick="btn_updNum_Click" />
                                <asp:Button ID="btn_updNumInv" runat="server" Text="Button" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="btn_updNum_ModalPopupExtender" runat="server" DynamicServicePath=""
                                    Enabled="True" TargetControlID="btn_updNumInv" PopupControlID="pl_num" BackgroundCssClass="modalBackground">
                                </cc1:ModalPopupExtender>
                            </div>
                            <div>
                                <asp:DropDownList ID="ddl_updTaxID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_updTaxID_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl_updTaxID1" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            金額
                        </th>
                        <td>
                            <asp:TextBox ID="txt_pay" runat="server" Width="50px"    ></asp:TextBox><%--AutoPostBack="True" 
                                ontextchanged="txt_pay_TextChanged1"--%>
                        </td>
                        <th>
                            所得稅(扣項)
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txt_tax" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            說明
                        </th>
                        <td colspan="5">
                            <asp:TextBox ID="txt_note" runat="server" MaxLength="200" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            健保費(扣項)
                        </th>
                        <td>
                            <asp:TextBox ID="txt_pay1" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <th>
                            勞保費(扣項)
                        </th>
                        <td>
                            <asp:TextBox ID="txt_pay2" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <th>
                            自提離職儲金(扣項)
                        </th>
                        <td>
                            <asp:TextBox ID="txt_pay3" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            自提基金</span>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_saveUntax" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <th>
                            法院扣款
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txt_extra01" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <th>
                            二代健保補充保費</span>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_Secondhealth_pay" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                        </td>
                        <th>
                            是否外聘
                        </th>
                        <td colspan="3">
                           <asp:DropDownList ID="ddl_External" runat="server">
                        <asp:ListItem  Text="否" Value="N"></asp:ListItem>
                        <asp:ListItem  Text="是" Value ="Y"></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: center;">
                            <asp:Button ID="btn_save" runat="server" Text="儲存" OnClick="btn_save_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pl_num" runat="server" CssClass="Grid1" Style="width: 350px; display: none;">
                <div style="text-align: right; width: 95%;">
                    <asp:ImageButton ID="ibt_numClose" runat="server" ImageUrl="~/images/Back.gif" OnClick="ibt_numClose_Click" />
                </div>
                <div>
                    <asp:GridView ID="gv_num" runat="server" OnRowCommand="gv_num_RowCommand" AllowPaging="True"
                        OnPageIndexChanging="gv_num_PageIndexChanging" Style="width: 95%;" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" Text="選取" CommandName="select" CommandArgument='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="代碼" DataField="ID" />
                            <asp:BoundField HeaderText="名稱" DataField="NAME" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
