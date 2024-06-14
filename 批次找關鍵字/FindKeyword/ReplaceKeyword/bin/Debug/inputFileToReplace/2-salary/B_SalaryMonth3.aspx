<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryMonth3.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryMonth3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="PageTitle">
                每月薪資編輯
            </div>
            <table border="1" class="TableStyleBlue" style="width: 100%;">
                <tr>
                    <th colspan="6">
                        <asp:Label ID="lb_islock" runat="server" Text="已關帳" Visible="false" Style="color: Red;"></asp:Label>
                        <asp:Label ID="Label_Pages" runat="server" Text="Label1"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <th>
                        年月
                    </th>
                    <td colspan="5">
                        <asp:Label ID="lb_amonth" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        員工編號
                    </th>
                    <td>
                        <asp:Label ID="lbl_polno" runat="server" Text="Label"></asp:Label>
                    </td>
                    <th>
                        身分證號
                    </th>
                    <td>
                        <asp:Label ID="lbl_idcard" runat="server" Text="Label"></asp:Label>
                    </td>
                    <th>
                        姓名
                    </th>
                    <td>
                        <asp:Label ID="lbl_name" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        發薪機關
                    </th>
                    <td>
                        <asp:Label ID="lbl_payad" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        單位
                    </th>
                    <td>
                        <asp:Label ID="lbl_unit" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        職稱
                    </th>
                    <td>
                        <asp:Label ID="lbl_occc" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        薪俸職等
                    </th>
                    <td>
                        <asp:Label ID="lbl_srank" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        俸階
                    </th>
                    <td>
                        <asp:Label ID="lbl_slvc" runat="server" Text=""></asp:Label>
                    </td>
                    <th>
                        俸點
                    </th>
                    <td>
                        <asp:Label ID="lbl_spt" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        應發總計
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_INCREASE" runat="server" BackColor="#FFFF99"></asp:TextBox>
                    </td>
                    <th>
                        應扣總計
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_DECREASE" runat="server" BackColor="#FFFF99"></asp:TextBox>
                    </td>
                    <th>
                        實發金額
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_TPAY" runat="server" BackColor="#FFFF99"></asp:TextBox>
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
                    <td style="text-align: center;">
                        <asp:Button ID="btIN" runat="server" Text="應發金額" OnClick="btIN_Click" />
                        <asp:Button ID="btDE" runat="server" Text="應扣金額" OnClick="btDE_Click" />
                        <asp:Button ID="btOtherDE" runat="server" Text="其他應扣" OnClick="btOtherDE_Click" />
                    </td>
                </tr>
            </table>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <asp:Panel ID="Panel_Incease" runat="server" Height="250px">
                        <table class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th>
                                    月支數額
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_SALARYPAY1" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_SALARYPAY1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_SALARYPAY1"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    主管加給
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_BOSS" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_BOSS_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_BOSS" ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    專業加給
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_PROFESS" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_PROFESS_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_PROFESS"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    警勤加給
                                </th>
                                <td>
                                    <asp:DropDownList ID="DropDownList_WORKP" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_WORKP"
                                        DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_WORKP_DataBound"
                                        OnSelectedIndexChanged="DropDownList_WORKP_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TextBox_WORKP" runat="server" AutoPostBack="True" OnTextChanged="TextBox_WORKP_TextChanged"
                                        Style="height: 19px" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_WORKP_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_WORKP" ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:SqlDataSource ID="SqlDataSource_WORKP" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE, '代碼：'+ID+'，名稱：'+NAME+'，金額：'+PAY AS TEXT_DATA FROM B_WORKP ORDER BY CREATEDATE DESC">
                                    </asp:SqlDataSource>
                                </td>
                                <th>
                                    技術加給
                                </th>
                                <td>
                                    <asp:DropDownList ID="DropDownList_TECHNICS" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_TECHNICS"
                                        DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_TECHNICS_DataBound"
                                        OnSelectedIndexChanged="DropDownList_TECHNICS_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TextBox_TECHNICS" runat="server" AutoPostBack="True" OnTextChanged="TextBox_TECHNICS_TextChanged"
                                        Style="height: 19px" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_TECHNICS_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_TECHNICS"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:SqlDataSource ID="SqlDataSource_TECHNICS" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE, '代碼：'+ID+'，名稱：'+NAME+'，金額：'+PAY AS TEXT_DATA FROM B_TECHNICS ORDER BY CREATEDATE DESC">
                                    </asp:SqlDataSource>
                                </td>
                                <th>
                                    工作獎助金
                                </th>
                                <td>
                                    <asp:DropDownList ID="DropDownList_BONUS" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_BONUS"
                                        DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_BONUS_DataBound"
                                        OnSelectedIndexChanged="DropDownList_BONUS_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TextBox_BONUS" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_BONUS_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_BONUS" ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:SqlDataSource ID="SqlDataSource_BONUS" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE, '代碼：'+ID+'，名稱：'+NAME+'，金額：'+PAY AS TEXT_DATA FROM B_BONUS ORDER BY CREATEDATE DESC">
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    外事加給
                                </th>
                                <td>
                                    <asp:DropDownList ID="DropDownList_ADVENTIVE" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource_ADVENTIVE" DataTextField="NAME" DataValueField="ID"
                                        OnDataBound="DropDownList_ADVENTIVE_DataBound" OnSelectedIndexChanged="DropDownList_ADVENTIVE_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TextBox_ADVENTIVE" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_ADVENTIVE_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_ADVENTIVE"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:SqlDataSource ID="SqlDataSource_ADVENTIVE" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE, '代碼：'+ID+'，名稱：'+NAME+'，金額：'+PAY AS TEXT_DATA FROM B_ADVENTIVE ORDER BY CREATEDATE DESC">
                                    </asp:SqlDataSource>
                                </td>
                                <th>
                                    偏遠加給
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_FAR" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_FAR_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_FAR" ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    勤務繁重加給
                                </th>
                                <td>
                                    <asp:DropDownList ID="DropDownList_ELECTRIC" runat="server" DataSourceID="SqlDataSource_ELECTRIC"
                                        DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_ELECTRIC_DataBound"
                                        AutoPostBack="True" OnSelectedIndexChanged="DropDownList_ELECTRIC_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TextBox_ELECTRIC" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_ELECTRIC_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_ELECTRIC"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:SqlDataSource ID="SqlDataSource_ELECTRIC" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE, '代碼：'+ID+'，名稱：'+NAME+'，金額：'+PAY AS TEXT_DATA FROM B_ELECTRIC ORDER BY CREATEDATE DESC">
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    其他應發
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_otheradd" runat="server" Width="70px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:Panel ID="Panel_Decease" runat="server" Height="250px">
                        <table class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th>
                                    健保年功俸
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_HEALTHID" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHID_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_HEALTHID"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    健保眷口數
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_HEALTHMAN" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHMAN_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_HEALTHMAN">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    健保費
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_HEALTHPAY" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHPAY_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_HEALTHPAY"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    健保費補差扣款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_HEALTHPAY1" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHPAY1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_HEALTHPAY1"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    薪資扣款1
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_MONTHPAY_TAX" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_TAX_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MONTHPAY_TAX"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    薪資扣款2
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_MONTHPAY" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MONTHPAY"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    薪津所得稅
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_TAX" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_TAX_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_TAX" ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    公(勞)保費
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_INSURANCEPAY" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_INSURANCEPAY_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_INSURANCEPAY"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    退撫金費
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_CONCUR3PAY" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_CONCUR3PAY_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_CONCUR3PAY"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    其他應扣
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_otherminus" runat="server" Width="70px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <asp:Panel ID="Panel_OtherDecease" runat="server" Height="250px">
                        <table class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th>
                                    法院扣款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA01" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA01_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA01"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    國宅貸款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA02" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA02_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA02"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    銀行貸款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA03" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA03_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA03"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    分期付款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA04" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA04_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA04"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    優惠存款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA05" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA05_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA05"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    員工宿舍費
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA06" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA06_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA06"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    福利互助金費
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA07" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA07_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA07"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    伙食費
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA08" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA08_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA08"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <th>
                                    退撫金貸款
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_EXTRA09" runat="server" Width="70px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA09_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA09"
                                        ValidChars="-$,.">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <table class="TableStyleBlue" style="width: 100%;">
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btTable" runat="server" Text="查詢" />
                        <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" />
                        <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" OnClientClick="return confirm('確定刪除此筆資料？');" />
                        <asp:Button ID="btSave" runat="server" Text="確定" OnClick="btSave_Click" />
                        <asp:Button ID="btExit" runat="server" Text="取消" OnClick="btExit_Click" />
                        <asp:Button ID="btBack_Data" runat="server" Text="上一筆" OnClick="btBack_Data_Click" />
                        <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                        <asp:Button ID="btNext_Data" runat="server" Text="下一筆" OnClick="btNext_Data_Click" />
                        <asp:Button ID="btn_Selector" runat="server" Text="快速選單" OnClick="btn_Selector_Click" />
                    </td>
                </tr>
            </table>
            <div>
                <cc1:ModalPopupExtender ID="btTable_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btTable" PopupControlID="pl_search" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_search" runat="server" Width="400px" Style="display: none;" CssClass="DivPanel">
                    <div style="width: 100%; text-align: right;">
                        <asp:ImageButton ID="ibt_close" runat="server" ImageUrl="/images/Back.gif" />
                    </div>
                    <table class="TableStyleBlue" width="100%">
                        <tr>
                            <th>
                                發薪機關
                            </th>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_searchPayad" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                年月
                            </th>
                            <td colspan="3">
                                <asp:TextBox ID="txt_searchAmonth" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                員工編號
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchPolno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                            </td>
                            <th>
                                姓名
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchName" runat="server" MaxLength="5" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                身份證字號
                            </th>
                            <td colspan="3">
                                <asp:TextBox ID="txt_idcard" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: center;">
                                <asp:Button ID="btn_searchConfirm" runat="server" Text="查詢" OnClick="btn_searchConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btn_searchResult" runat="server" Text="Button" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_searchResult_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_searchResult" BackgroundCssClass="modalBackground"
                    PopupControlID="pl_searchResult">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_searchResult" runat="server" CssClass="DivPanel"  Style="width: 500px;">
                    <div style="width: 100%; text-align: right;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    <div style="overflow: auto; height: 300px;">
                        <asp:GridView ID="gv_Target" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_Target_RowCommand"
                            CssClass="Grid1">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_Select" runat="server" Text="選取" CommandName="btnSelect" CommandArgument='<%# Eval("RowIndex") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="AMONTH" HeaderText="年月">
                                    <ItemStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_NAME" HeaderText="姓名">
                                    <ItemStyle Width="50%" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
