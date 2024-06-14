<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryIncomeTax3.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryIncomeTax3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:scriptmanager id="ScriptManager1" runat="server" asyncpostbacktimeout="0">
    </asp:scriptmanager>
    <div class="PageTitle">
        所得稅資料編輯
    </div>
    <asp:updatepanel id="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="TableStyleBlue" style="width: 100%;">
                <tr>
                    <td colspan="8" style="text-align: right;">
                        <asp:Label ID="Label_Pages" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        年度
                    </th>
                    <td style="width: 300px;">
                        <asp:TextBox ID="TextBox_AYEAR" runat="server" Width="80px" MaxLength="3" Enabled="false"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="TextBox_AYEAR_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_AYEAR">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                    <th style="width: 100px;">
                        所得格式
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_TAX_TYPE" runat="server" Width="50px" Enabled="false" AutoPostBack="True"
                            MaxLength="3" OnTextChanged="TextBox_TAX_TYPE_TextChanged"></asp:TextBox>
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
                </tr>
                <tr>
                    <th>
                        員工編號
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <th>
                        職稱
                    </th>
                    <td>
                        <asp:Label ID="lbl_occc" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        姓名/名稱
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <th>
                        身分證號/統編
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        戶籍地址
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="TextBox_ADDRESS1" runat="server" Width="80%"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataSource_TAXES_DATA" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
            <asp:Panel ID="Panel1" runat="server" GroupingText="總計">
                <table class="TableStyleBlue" style="width: 100%;">
                    <tr>
                        <th style="text-align: center;">
                            應發總額
                        </th>
                        <th style="text-align: center;">
                            -主管加給
                        </th>
                        <th style="text-align: center;">
                            -免稅金額
                        </th>
                        <th style="text-align: center;">
                            -退撫免稅額
                        </th>
                        <th style="text-align: center;">
                            =給付總額
                        </th>
                        <th style="text-align: center;">
                            -扣繳稅額
                        </th>
                        <th style="text-align: center;">
                            =給付淨額
                        </th>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:TextBox ID="TextBox_INCREASE" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_INCREASE_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_INCREASE"
                                ValidChars="-$,.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txt_boss" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txt_decrease" runat="server" Width="80px" AutoPostBack="true" 
                                OnTextChanged="refreshTax" Height="19px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="txt_decrease">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        
                        <td style="text-align: center;">
                            <%--退撫免稅額--%>
                            <asp:TextBox ID="txt_CONCUR3PAY" runat="server" Width="80px" AutoPostBack="true" 
                                OnTextChanged="refreshTax" ></asp:TextBox>
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txt_paysum" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="TextBox_DECREASE" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_DECREASE_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_DECREASE"
                                ValidChars="-$,.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="TextBox_TOTAL" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_TOTAL_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_TOTAL" ValidChars="-$,.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" GroupingText="各項給付">
                <table>
                    <tr>
                        <td style="width: 300px; vertical-align: top;">
                            <asp:GridView ID="gv_salary" runat="server" AutoGenerateColumns="false" EmptyDataText="查無資料"
                                CssClass="Grid1" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="AMONTH" HeaderText="月份" />
                                    <asp:BoundField DataField="SALARYPAY1" HeaderText="應發金額" DataFormatString="{0:$#,#0}" />
                                    <asp:BoundField DataField="BOSS" HeaderText="主管加給" DataFormatString="{0:$#,#0}" />
                                    <asp:BoundField DataField="TAX" HeaderText="預扣稅額" DataFormatString="{0:$#,#0}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td style="width: 450px; vertical-align: top;">
                            <table class="TableStyleBlue" style="width: 100%;">
                                <tr>
                                    <th>
                                        薪俸所得
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_MONTHPAY" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MONTHPAY">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        薪俸預扣金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_MONTHPAYTAX" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAYTAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MONTHPAYTAX">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        補發所得
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_REPAIR" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_REPAIR_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_REPAIR">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        補發預扣金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_REPAIRTAX" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_REPAIRTAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_REPAIRTAX">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        單一發放所得
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_SOLE" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_SOLE_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_SOLE">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        單一發放預扣金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_SOLETAX" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_SOLETAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_SOLETAX">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        年終獎金所得
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_YEARPAY" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_YEARPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEARPAY">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        年終獎金預扣金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_YEARPAYTAX" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_YEARPAYTAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEARPAYTAX">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        考績獎金所得
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EFFECT" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EFFECT_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_EFFECT">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        考績獎金預扣金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EFFECTTAX" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EFFECTTAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_EFFECTTAX">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        自行沖銷金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_OTHERSUB" runat="server" Width="80px" AutoPostBack="true"
                                            OnTextChanged="refreshTax"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_OTHERSUB_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_OTHERSUB">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        自行沖銷稅額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_othersubTax" runat="server" Width="80px" AutoPostBack="true"
                                            OnTextChanged="refreshTax"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            FilterType="Numbers" TargetControlID="txt_othersubTax">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="trLabor" runat="server">
                                    <th>
                                        勞工自提退休金
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox_LABORPAY" runat="server" Width="80px" AutoPostBack="true"
                                            OnTextChanged="refreshTax"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_LABORPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_LABORPAY">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="TableStyleBlue" style="width: 100%;">
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btBack_Data" runat="server" Text="上一筆" OnClick="btBack_Click" />
                        <asp:Button ID="btTable" runat="server" Text="查詢" />
                        <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" />
                        <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" />
                        <asp:Button ID="btExit" runat="server" Text="取消" OnClick="btExit_Click" />
                        <asp:Button ID="btNext_Data" runat="server" Text="下一筆" OnClick="btNext_Click" />
                        <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                        <asp:Button ID="bt_InData" runat="server" Text="免稅金額匯入" 
                            onclick="bt_InData_Click" />
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button ID="btn_popup" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_popup_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btTable" PopupControlID="pl_popup" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_popup" runat="server" CssClass="DivPanel" Style="display: none;
                    width: 400px;">
                    <div style="width: 100%; text-align: right;">
                        <asp:ImageButton ID="ibt_close" runat="server" ImageUrl="/images/Back.gif" />
                    </div>
                    <table class="TableStyleBlue" style="width: 100%;">
                        <tr>
                            <th>
                                年度
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchAyear" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                機關
                            </th>
                            <td>
                                <asp:DropDownList ID="ddl_searchPayad" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_searchPayad_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                單位
                            </th>
                            <td>
                                <asp:DropDownList ID="ddl_searchUnit" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                身份證字號
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchID" runat="server" Width="100px" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                姓名
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchName" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                所得格式
                            </th>
                            <td>
                                <asp:TextBox ID="txt_searchTaxType" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
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
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>
