﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryEFFECT2.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryEFFECT2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/UserSelector.ascx" TagName="UserSelector" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>'; 
    </script>

    <div class="PageTitle">
        考績獎金維護
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" AsyncPostBackTimeout="3600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                Width="200px" Height="100px" Style="display: none;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="1000">
                    <ProgressTemplate>
                        <div style="position: relative; top: 40%; text-align: center;">
                            <img src="/images/loading.gif" style="vertical-align: middle" alt="Processing" />
                            處理中 ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                    Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
            </asp:Panel>
            <table class="TableStyleBlue">
                <tr>
                    <th>
                        發給年份
                    </th>
                    <td colspan="5">
                        <asp:TextBox ID="txt_year" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                         <asp:Label ID="lb_LockDB" runat="server" Text="已關帳" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        員工編號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_polno" runat="server" MaxLength="8" Width="80px" AutoPostBack="True"
                            OnTextChanged="txt_polno_TextChanged"></asp:TextBox>
                    </td>
                    <th>
                        身份證字號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_idno" runat="server" MaxLength="10" Width="100px" AutoPostBack="True"
                            OnTextChanged="txt_idno_TextChanged"></asp:TextBox>
                    </td>
                    <th>
                        姓名
                    </th>
                    <td>
                        <asp:TextBox ID="txt_name" runat="server" MaxLength="5" Width="100px" AutoPostBack="True"
                            OnTextChanged="txt_name_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        薪俸職等
                    </th>
                    <td>
                        <asp:Label ID="lb_srank" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        俸點
                    </th>
                    <td>
                        <asp:Label ID="lb_spt" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        職稱
                    </th>
                    <td>
                        <asp:Label ID="lb_occc" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        發薪機關
                    </th>
                    <td>
                        <asp:Label ID="lb_payad" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        單位
                    </th>
                    <td >
                        <asp:Label ID="lb_unit" runat="server" Text=""></asp:Label>
                    </td>
                    
                    <th>
                        考績等級
                    </th>
                    <td >
                        <asp:Label ID="lb_MZ_GRADE" runat="server" Text=""></asp:Label>
                    </td>
                    
                </tr>
                <tr>
                    <th>
                        發給月數
                    </th>
                    <td>
                        <asp:TextBox ID="txt_pay" runat="server" MaxLength="5" Width="30px"></asp:TextBox>
                    </td>
                    <td colspan="4">
                        <asp:Button ID="btn_calculate" runat="server" Text="計算" OnClick="btn_calculate_Click" />
                    </td>
                </tr>
                <tr>
                    <th>
                        任主管月數
                    </th>
                    <td colspan="4">
                        <asp:TextBox ID="txt_bossAmonth" runat="server" MaxLength="2" Width="30px" AutoPostBack="True" OnTextChanged="txt_bossAmonth_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        薪俸
                    </th>
                    <td>
                        <asp:TextBox ID="txt_salary" runat="server" Width="80px"></asp:TextBox>
                    </td>
                    <th>
                        專業加給
                    </th>
                    <td>
                        <asp:TextBox ID="txt_profess" runat="server" Width="80px"></asp:TextBox>
                    </td>
                    <th>
                        主管加給
                    </th>
                    <td>
                        <asp:TextBox ID="txt_boss" runat="server" Width="80px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        警勤加給
                    </th>
                    <td>
                        <asp:TextBox ID="txt_work" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                    </td>
                    <th>
                        技術加給
                    </th>
                    <td>
                        <asp:TextBox ID="txt_tech" runat="server" Width="80px"></asp:TextBox>
                    </td>
                    <th>
                        偏遠加給
                    </th>
                    <td>
                        <asp:TextBox ID="txt_far" runat="server" Width="80px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                 <th>
                        勤務繁重加給
                    </th>
                    <td>
                        <asp:TextBox ID="txt_electric" runat="server" Width="70px" Enabled="False"></asp:TextBox>
                    </td>
                
                    <th>
                        法院扣款
                    </th>
                    <td>
                        <asp:TextBox ID="txt_extra01" runat="server" Width="80px"></asp:TextBox>
                    </td>
                    <th>
                        所得稅
                    </th>
                    <td>
                        <asp:TextBox ID="txt_tax" runat="server" Width="80px"></asp:TextBox>
                    </td>
                   
                </tr>
                <tr>
                    <th>
                        備註
                    </th>
                    <td colspan="5">
                        <asp:TextBox ID="txt_note" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <table>
                            <tr>
                                <th>
                                    應發金額
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_total" runat="server" Width="120px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                    －
                                </td>
                                <th>
                                    應扣款項
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_des" runat="server" Width="120px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                    ＝
                                </td>
                                <th>
                                    考績獎金實發金額
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_net" runat="server" Width="120px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button ID="bt_search" runat="server" Text="查詢" OnClick="bt_search_Click" />
                <asp:Button ID="btn_create" runat="server" Text="新增" OnClick="btn_create_Click" />
                <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" OnClientClick="return confirm('確定將資料儲存？');" />
                <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" OnClientClick="return confirm('確定將資料刪除？');" />
                <asp:Button ID="btBack" runat="server" Text="上一筆" OnClick="btBack_Click" />
                <asp:Button ID="btNext" runat="server" Text="下一筆" OnClick="btNext_Click" />
                <asp:Button ID="btn_fastMenu" runat="server" Text="快速選單" OnClick="btn_fastMenu_Click" />
            </div>
            <div>
                <asp:Button ID="btn_showSearch" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showSearch_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showSearch" PopupControlID="pl_search" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_search" runat="server" CssClass="DivPanel" Style="width: 420px;
                    display: none;">
                    <div style="text-align: right; width: 95%;">
                        <asp:ImageButton ID="ibt_numClose" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    <table class="TableStyleBlue" style="width: 95%;">
                        <tr>
                            <th>
                                年度：
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchAyear" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                發薪機關：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddl_searchPayad" runat="server" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddl_searchPayad_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                單位：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddl_searchUnit" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                員工編號：
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchPolno" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                員工姓名：
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                身分證字號：
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchIDCARD" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:Button ID="btn_searchConfirm" runat="server" Text="查詢" OnClick="btn_searchConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btn_showfastMenu" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showfastMenu_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showfastMenu" PopupControlID="pl_fastMenu"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_fastMenu" runat="server" CssClass="DivPanel" Style="width: 500px;
                    display: none;">
                    <div style="text-align: right; width: 95%;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    <asp:GridView ID="gv_searchResult" runat="server" AutoGenerateColumns="False" CssClass="Grid1"
                        OnRowCommand="gv_searchResult_RowCommand" AllowPaging="True" OnPageIndexChanging="gv_searchResult_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btn_select" runat="server" Text="選擇" CommandName="sel" CommandArgument='<%# Eval("E_SNID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PAY_UNIT" HeaderText="單位" />
                            <asp:BoundField DataField="NAME" HeaderText="姓名" />
                            <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" />
                        </Columns>
                        <Columns>
                            <asp:BoundField HeaderText="單位" DataField="PAY_UNIT" />
                        </Columns>
                        <Columns>
                            <asp:BoundField HeaderText="姓名" DataField="NAME" />
                        </Columns>
                        <Columns>
                            <asp:BoundField HeaderText="員工編號" DataField="MZ_POLNO" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btn_showSelector" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showSelector_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showSelector" PopupControlID="pl_selector"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_selector" runat="server" CssClass="DivPanel" Style="width: 500px;
                    display: none;">
                    <uc1:UserSelector ID="UserSelector1" runat="server" />
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
