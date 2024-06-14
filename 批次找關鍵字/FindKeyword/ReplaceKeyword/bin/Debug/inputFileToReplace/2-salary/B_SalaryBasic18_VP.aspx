<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic18_VP.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic18_VP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>';

    </script>

    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
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
            <div style="text-align: left; font-size: 16pt; font-family: 標楷體; background-color: #6699FF;
                color: White;">
                其他基本資料設定（單一發放）</div>
            <table class="TableStyleBlue" style="width: 95%;">
                <tr>
                    <th>
                        身分證號碼／統一編號
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_SearchID" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        自訂編號
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_SearchPOLNO" runat="server" MaxLength="8"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        姓名/抬頭
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_SearchName" runat="server" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="Button_SearchDone" runat="server" Text="查詢" OnClick="Button_SearchDone_Click" />
                        <asp:Button ID="btn_Add" runat="server" Text="新增" OnClick="btn_Add_Click" />
                        <asp:Button ID="btn_export" runat="server" Text="匯出excel" OnClick="btn_export_Click" />
                    </td>
                </tr>
            </table>
            <div style="text-align: right; width: 95%;">
                <asp:Button ID="btn_showPopup" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showPopup_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showPopup" PopupControlID="Panel_Search"
                    BackgroundCssClass="modalBackground" CancelControlID="ibt_popupClose">
                </cc1:ModalPopupExtender>
            </div>
            <div style="width: 95%;">
                <asp:HiddenField ID="HiddenField_IDCARD" runat="server" Visible="False" />
                <asp:GridView ID="GridView_MANUFACTURER_BASE" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="MB_SNID" OnRowCommand="GridView_MANUFACTURER_BASE_RowCommand" Width="100%"
                    AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView_MANUFACTURER_BASE_PageIndexChanging"
                    CssClass="Grid1">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="sel"
                                    Text="修改" CommandArgument='<%# Eval("MB_SNID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="del"
                                    Text="刪除" CommandArgument='<%# Eval("MB_SNID") %>' OnClientClick="return confirm('確定要刪除這筆資料？')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MB_SNID" HeaderText="編號" SortExpression="MB_SNID" Visible="false" />
                        <asp:BoundField DataField="MZ_POLNO" HeaderText="自訂編號" SortExpression="MZ_POLNO" />
                        <asp:BoundField DataField="IDCARD" HeaderText="身分證號碼/統一編號" SortExpression="IDCARD" />
                        <asp:BoundField DataField="NAME" HeaderText="姓名/抬頭" SortExpression="NAME" />
                        <asp:BoundField DataField="LASTDA" HeaderText="最後異動日期" SortExpression="LASTDA" />
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel ID="Panel_Search" runat="server" CssClass="DivPanel" Style="width: 770px;
                display: none;">
                <div style="width: 95%; text-align: right;">
                    <asp:ImageButton ID="ibt_popupClose" runat="server" ImageUrl="~/images/Back.gif" />
                </div>
                <table width="95%" class="TableStyleBlue">
                    <tr>
                        <th>
                            身分證號碼／統一編號：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_IDCARD" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btSearch" runat="server" OnClick="btSearch_Click" Text="檢查" />
                            <asp:Label ID="LabelCheck" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            <asp:CheckBox ID="cb_check" runat="server" Text="驗證身份證字號正確性" Checked="true" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            自訂編號：
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZPOLNO" runat="server" MaxLength="8"></asp:TextBox>
                        </td>
                        <th>
                            姓名：
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_NAME" runat="server" MaxLength="25" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            證號別：
                        </th>
                        <td colspan="3">
                            <asp:DropDownList ID="DropDownList_IDTYPE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList_IDTYPE_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">0‧本國個人</asp:ListItem>
                                <asp:ListItem Value="1">1‧公司行號機關團體</asp:ListItem>
                                <asp:ListItem Value="3">3‧在中華民國境內住滿183天之外僑或大陸地區人民</asp:ListItem>
                                <asp:ListItem Value="5">5‧在中華民國境內未住滿183天之大陸地區人民</asp:ListItem>
                                <asp:ListItem Value="6">6‧大陸地區單位</asp:ListItem>
                                <asp:ListItem Value="7">7‧在中華民國境內未住滿183天之外僑</asp:ListItem>
                                <asp:ListItem Value="8">8‧總機構在中華民國境外之法人、團體或其他機構</asp:ListItem>
                                <asp:ListItem Value="9">9‧在中華民國境內未住滿183天且已除戶之本國個人</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            戶籍地址：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_ADDRESS1" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            通訊／租賃地址：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_ADDRESS2" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                            &nbsp;
                            <asp:Button ID="Button_SAME" runat="server" Text="同戶籍地址" OnClick="Button_SAME_Click" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            房屋租賃稅籍編號
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_RENTNUM" runat="server" MaxLength="12"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            手機：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_CELLPHONE" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            電話：
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_TEL" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                        <th>
                            分機：
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_EXT" runat="server" Width="70px" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trNC" runat="server">
                        <th>
                            國家代碼：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_NationCode" runat="server" MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            銀行名稱：
                        </th>
                        <td>
                            <asp:DropDownList ID="DropDownList_BANK_CODE" runat="server" DataSourceID="SqlDataSource_BANK_LIST"
                                DataTextField="IDNAME" DataValueField="BANK_ID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_BANK_LIST" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT BANK_ID, BANK_NAME, BANK_ID +'('+ BANK_NAME +')' AS IDNAME FROM B_BANK_LIST ORDER BY BANK_ID">
                            </asp:SqlDataSource>
                        </td>
                        <th>
                            銀行帳號：
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_BANK_ID" runat="server" MaxLength="16"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trDay183" runat="server">
                        <th>
                            在台是否滿183天
                        </th>
                        <td colspan="3">
                            <asp:RadioButtonList ID="RadioButtonList_Day183" runat="server" RepeatColumns="2">
                                <asp:ListItem Selected="True" Value="Y">是</asp:ListItem>
                                <asp:ListItem Value="N">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trRC" runat="server">
                        <th>
                            租稅協定代碼：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_RentCode" runat="server" MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            備註：
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_NOTE" runat="server" Width="550px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">
                            <asp:Button ID="btn_popupSave" runat="server" Text="儲存" OnClick="btn_popupSave_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
