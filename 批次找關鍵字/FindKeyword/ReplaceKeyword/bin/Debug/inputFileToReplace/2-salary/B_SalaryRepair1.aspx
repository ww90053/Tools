<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryRepair1.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryRepair1" %>

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

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            if (year.toString().length == 2) {
                year = "0" + year;
            }
            else {
                year = year;
            }
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <div class="PageTitle">
        補發薪資維護
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
            <asp:Panel ID="Panel_Basic" runat="server" Width="700px">
                <table class="TableStyleBlue" width="100%">
                    <tr>
                        <th colspan="6">
                            <div style="text-align: right;">
                                <asp:Label ID="lb_islock" runat="server" Text="已關帳" Visible="false" Style="color: Red;"></asp:Label>
                                <asp:Label ID="Label_Pages" runat="server" Text="Label1"></asp:Label>
                                <asp:DropDownList ID="ddl_PayAD" runat="server" Visible="false">
                                </asp:DropDownList>
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <th>
                            年月
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AMonth" runat="server" MaxLength="5" Width="40px"></asp:TextBox>
                        </td>
                        <th>
                            批號
                        </th>
                        <td>
                            <asp:TextBox ID="txt_batch" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                        </td>
                        <th>
                            日期
                        </th>
                        <td>
                            起<asp:TextBox ID="TextBox_SDA" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_SDA_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_SDA">
                            </cc1:FilteredTextBoxExtender>
                            訖<asp:TextBox ID="TextBox_EDA" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_EDA_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_EDA">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            員工編號
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" AutoPostBack="True" MaxLength="8"
                                Width="80px" OnTextChanged="TextBox_MZ_POLNO_TextChanged"></asp:TextBox>
                        </td>
                        <th>
                            身分證號
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_IDCARD" runat="server" AutoPostBack="True" MaxLength="10"
                                Width="100px" OnTextChanged="TextBox_IDCARD_TextChanged"></asp:TextBox>
                        </td>
                        <th>
                            姓名
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" AutoPostBack="True" MaxLength="5"
                                Width="120px" OnTextChanged="TextBox_MZ_NAME_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            補發型態
                        </th>
                        <td colspan="5">
                            <asp:DropDownList ID="DropDownList_ATYPE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList_ATYPE_SelectedIndexChanged">
                                <asp:ListItem Value="1">1.新進人員</asp:ListItem>
                                <asp:ListItem Value="2">2.調入</asp:ListItem>
                                <asp:ListItem Value="3">3.薪資晉階晉級</asp:ListItem>
                                <asp:ListItem Value="4">4.考績晉級</asp:ListItem>
                                <asp:ListItem Value="5">5.考績晉階</asp:ListItem>
                                <asp:ListItem Value="6">6.補考績及晉級晉階差額</asp:ListItem>
                                <asp:ListItem Value="7">7.其他</asp:ListItem>
                                <asp:ListItem Value="8">8.同階10年以上</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            薪俸職等
                        </th>
                        <td colspan="5">
                            原
                            <asp:DropDownList ID="ddl_SSrank" runat="server">
                            </asp:DropDownList>
                            新
                            <asp:DropDownList ID="ddl_ESrank" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            俸點
                        </th>
                        <td colspan="5">
                            原<asp:TextBox ID="TextBox_SMZ_SPT" runat="server" Width="50px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_SMZ_SPT_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_SMZ_SPT">
                            </cc1:FilteredTextBoxExtender>
                            新<asp:TextBox ID="TextBox_EMZ_SPT" runat="server" Width="50px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_EMZ_SPT_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_EMZ_SPT">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            考績等級
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txt_Grade" runat="server" MaxLength="1" Width="20px"></asp:TextBox>
                        </td>
                            <th>
                            發放月數
                            </th>                  
                        <td style="text-align: left;" class ="tdstyle" colspan="2">
                            <asp:RadioButton ID="rbmon1" runat="server" Text="1" Checked="true" GroupName ="paymonth"/><asp:RadioButton ID="rbmon2" runat="server" Text="0.5" GroupName ="paymonth"/>
                            <asp:RadioButton ID="rbmon3" runat="server" Text="1.5" GroupName ="paymonth"/><asp:RadioButton ID="rbmon4" runat="server" Text="0" GroupName ="paymonth"/>        
                        </td>
                    </tr>
                    <tr>
                        <th>
                            應發總計
                        </th>
                        <td>
                            <asp:Label ID="lb_paySum" runat="server" Text=""></asp:Label>
                        </td>
                        <th>
                            應扣總計
                        </th>
                        <td>
                            <asp:Label ID="lb_costSum" runat="server" Text=""></asp:Label>
                        </td>
                        <th>
                            實發金額
                        </th>
                        <td>
                            <asp:Label ID="lb_realSum" runat="server" Text=""></asp:Label>
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
                    <tr>
                        <td colspan="6" style="text-align: center;">
                            <asp:Button ID="btCount" runat="server" OnClick="btCount_Click" Text="計算" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="TableStyleBlue">
                <tr>
                    <td>
                        <asp:Button ID="btIN" runat="server" Text="應發金額" OnClick="btIN_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btDE" runat="server" Text="應扣金額" OnClick="btDE_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btOtherDE" runat="server" Text="其他應扣" OnClick="btOtherDE_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 200px;">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <asp:Panel ID="Panel_Incease" runat="server" Width="700px">
                            <table class="TableStyleBlue" width="100%">
                                <tr>
                                    <th>
                                        月支數額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_SALARYPAY1" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox ID="TextBox_PROFESS" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>
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
                                        <%--20210129 - 小隊長要求開放可輸入--%>
                                        <%--<asp:TextBox ID="TextBox_WORKP" runat="server" Style="height: 19px" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>--%>
                                        <asp:TextBox ID="TextBox_WORKP" runat="server" Style="height: 19px" Width="70px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_WORKP_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_WORKP" ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        技術加給
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_TECHNICS" runat="server" Style="height: 19px" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_TECHNICS_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_TECHNICS"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        工作獎助金
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_BONUS" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_BONUS_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_BONUS" ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        外事加給
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_ADVENTIVE" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_ADVENTIVE_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_ADVENTIVE"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        偏遠加給
                                    </th>
                                    <td>
                                        <%--20210129 - 小隊長要求開放可輸入--%>
                                        <%--<asp:TextBox ID="TextBox_FAR" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>--%>
                                        <asp:TextBox ID="TextBox_FAR" runat="server" Width="70px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_FAR_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_FAR" ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        勤務繁重加給
                                    </th>
                                    <td>
                                        <%--20210129 - 小隊長要求開放可輸入--%>
                                        <%--<asp:TextBox ID="TextBox_ELECTRIC" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>--%>
                                        <asp:TextBox ID="TextBox_ELECTRIC" runat="server" Width="70px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_ELECTRIC_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_ELECTRIC"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        其他應發
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_SALARY_2_INCREASE" runat="server" Width="70px" Enabled="False" ReadOnly="True"></asp:TextBox>
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
                        <asp:Panel ID="Panel_Decease" runat="server" Width="700px">
                            <table class="TableStyleBlue" width="100%">
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
                                        <asp:TextBox ID="TextBox_HEALTHPAY" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_HEALTHPAY"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        健保費扣款
                                    </th>
                                    <td colspan="4">
                                        <asp:TextBox ID="TextBox_HEALTHPAY1" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHPAY1_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_HEALTHPAY1"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </td>
                                    <%--<th>
                                        補扣月數
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_HEALTHPAY_COUNT" runat="server" Width="70px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_HEALTHPAY_COUNT_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TextBox_HEALTHPAY_COUNT">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btHEALTHPAY1_Count" runat="server" Text="補扣計算" OnClick="btHEALTHPAY1_Count_Click" />
                                    </td>--%>
                                </tr>
                                <tr>
                                    <th>
                                        薪資扣款1
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_MONTHPAY_TAX" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_TAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MONTHPAY_TAX"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        薪資扣款2
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_MONTHPAY" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MONTHPAY"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        所得稅
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_TAX" runat="server" Width="70px" AutoPostBack="true" OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_TAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_TAX" ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        公(勞)保費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_INSURANCEPAY" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_INSURANCEPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_INSURANCEPAY"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        退撫金費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_CONCUR3PAY" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_CONCUR3PAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_CONCUR3PAY"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        其他應扣
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_SALARY_2_DECREASE" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                        <asp:Panel ID="Panel_OtherDecease" runat="server" Width="700px">
                            <table class="TableStyleBlue" width="100%">
                                <tr>
                                    <th>
                                        法院扣款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA01" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA01_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA01"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        國宅貸款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA02" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA02_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA02"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        銀行貸款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA03" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
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
                                        <asp:TextBox ID="TextBox_EXTRA04" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA04_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA04"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        優惠存款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA05" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA05_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA05"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        員工宿舍費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA06" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA06_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA06"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        伙食費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA07" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA07_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA07"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        福利互助金費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA08" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA08_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA08"
                                            ValidChars="-$,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        退撫金貸款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA09" runat="server" Width="70px" AutoPostBack="true"
                                            OnTextChanged="costChanged"></asp:TextBox>
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
            </div>
            <table class="TableStyleBlue">
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btTable" runat="server" Text="查詢" />
                        <cc1:ModalPopupExtender ID="btTable_ModalPopupExtender" runat="server" DynamicServicePath=""
                            Enabled="True" TargetControlID="btTable" PopupControlID="pl_search" BackgroundCssClass="modalBackground">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btCreate" runat="server" Text="新增" OnClick="btCreate_Click" />
                        <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" />
                        <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" OnClientClick="return confirm('確定要刪除？');" />
                        <asp:Button ID="btExit" runat="server" Text="取消" OnClick="btExit_Click" />
                        <asp:Button ID="btBack_Data" runat="server" Text="上一筆" OnClick="btBack_Data_Click" />
                        <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                        <asp:Button ID="btNext_Data" runat="server" Text="下一筆" OnClick="btNext_Data_Click" />
                        <asp:Button ID="btn_Selector" runat="server" Text="快速選單" OnClick="btn_Selector_Click" />
                    </td>
                </tr>
            </table>
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
                        <td>
                            <asp:TextBox ID="txt_searchAmonth" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                        </td>
                        <th>
                            批號
                        </th>
                        <td>
                            <asp:TextBox ID="txt_searchBatch" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
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
                            身份證號
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txt_searchId" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">
                            <asp:Button ID="btn_searchConfirm" runat="server" Text="查詢" OnClick="btn_searchConfirm_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" />
            <cc1:ModalPopupExtender ID="pl_Selector_ModalPopupExtender" runat="server" DynamicServicePath=""
                Enabled="True" TargetControlID="Button1" BackgroundCssClass="modalBackground"
                PopupControlID="pl_Selector">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pl_Selector" runat="server" Style="display: none;">
                <uc1:UserSelector ID="UserSelector1" runat="server" style="display: none;" />
            </asp:Panel>
            <asp:Button ID="btn_searchResult" runat="server" Text="Button" Style="display: none;" />
            <cc1:ModalPopupExtender ID="btn_searchResult_ModalPopupExtender" runat="server" DynamicServicePath=""
                Enabled="True" TargetControlID="btn_searchResult" BackgroundCssClass="modalBackground"
                PopupControlID="pl_searchResult">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pl_searchResult" runat="server" CssClass="DivPanel" Style="display: none;
                width: 300px; height: 400px; overflow: scroll;">
                <div style="width: 100%; text-align: right;">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" />
                </div>
                <asp:GridView ID="gv_Target" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_Target_RowCommand"
                    CssClass="Grid1">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_Select" runat="server" Text="選取" CommandName="btnSelect" CommandArgument='<%# Eval("RowIndex") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AMONTH" HeaderText="年月" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="BATCH_NUMBER" HeaderText="案號" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="50px" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
