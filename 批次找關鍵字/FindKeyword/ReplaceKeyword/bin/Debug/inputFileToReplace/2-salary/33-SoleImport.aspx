<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="33-SoleImport.aspx.cs" Inherits="TPPDDB._2_salary._3_SoleImport" %>

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
        單一發放整批匯入</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                Width="200px" Height="100px" Style="display: none;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="300">
                    <ProgressTemplate>
                        <div style="position: relative; top: 40%; text-align: center;">
                            <img src="/images/loading.gif" style="vertical-align: middle" alt="Processing" />
                            資料匯入中 ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <table style="width: 90%;">
                <tr>
                    <th>
                        <asp:RadioButtonList ID="rbl_type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbl_type_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Selected="True">由差勤系統匯入</asp:ListItem>
                            <asp:ListItem Value="2">由excel匯入</asp:ListItem>
                        </asp:RadioButtonList>
                    </th>
                </tr>
                <tr runat="server" id="by1">
                    <td>
                        <table class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th colspan="4" style="text-align: center;">
                                    由差勤系統匯入
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    發薪機關
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_payad" runat="server"  AutoPostBack = "true" OnSelectedIndexChanged="ddl_payad_SelectedIndexChanged">
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
                                    <asp:DropDownList ID="ddl_kind" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddl_kind_SelectedIndexChanged">
                                        <asp:ListItem Value="1">超勤</asp:ListItem>
                                        <asp:ListItem Value="2">加班</asp:ListItem>
                                        <asp:ListItem Value="3">值日</asp:ListItem>
                                        <asp:ListItem Value="4">不休假改發加班費</asp:ListItem>
                                        <asp:ListItem Value="6">休假超過10日補助費</asp:ListItem>
                                        <asp:ListItem Value="5">獎勵休假</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    日期
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_year" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                                    年
                                    <asp:TextBox ID="txt_month" runat="server" MaxLength="2" Width="30px"></asp:TextBox>月
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center;">
                                    <asp:Button ID="btn_get" runat="server" Text="取得資料" OnClick="btn_get_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="by2" visible="false">
                    <td>
                        <table class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th colspan="4" style="text-align: center;">
                                    由excel匯入
                                </th>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btn_getOverwork" runat="server" Text="超勤印領清冊" OnClick="btn_getOverwork_Click" />
                                    <asp:Button ID="btn_getTraffic" runat="server" Text="獎勵金印領清冊" OnClick="btn_getTraffic_Click" />
                                 <%--   <asp:Button ID="btn_effect" runat="server" Text="考績評議清冊" OnClick="btn_effect_Click" />--%>
                                    <asp:Button ID="btn_default" runat="server" Text="標準Excel" OnClick="btn_default_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div runat="server" id="data" visible="false">
                <table class="TableStyleBlue" style="width: 90%; margin-top: 10px;">
                    <tr>
                        <th style="color: Red;">
                            入帳日期
                        </th>
                        <td>
                            <asp:TextBox ID="txt_DA" runat="server" MaxLength="7" Width="70px" AutoPostBack="True"
                                OnTextChanged="txt_DA_TextChanged"></asp:TextBox>
                        </td>
                        <th style="color: Red;">
                            入帳案號
                        </th>
                        <td>
                            <asp:TextBox ID="txt_caseid" runat="server" MaxLength="2" Width="30px" AutoPostBack="True"
                                OnTextChanged="txt_caseid_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lb_count" runat="server" Text="" Style="color: Red;"></asp:Label>
                            <asp:Button ID="btn_Add" runat="server" Text="匯入勾選資料" OnClick="btn_Add_Click" 
                                OnClientClick="return confirm('確定要匯入勾選的資料？')" 
                                style="color: #0000FF; font-weight: 700" />
                            &nbsp;
                            <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                                style="color: #009900; font-weight: 700" Text="開啟匯入結果" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gv_Result" runat="server" CssClass="Grid1" AutoGenerateColumns="False"
                    Style="width: 90%;">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_add" runat="server" Checked="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="員工編號" DataField="MZ_POLNO" />
                        <asp:BoundField HeaderText="單位代碼" DataField="MZ_UNIT" />
                        <asp:BoundField HeaderText="單位" DataField="CHIUNIT" />
                        <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                        <asp:BoundField HeaderText="身份證字號" DataField="MZ_ID" />
                        <asp:BoundField HeaderText="姓名" DataField="MZ_NAME" />
                        <asp:BoundField HeaderText="項目代碼" DataField="KINDCODE" />
                        <asp:BoundField HeaderText="項目" DataField="KIND" />
                        <asp:BoundField HeaderText="金額" DataField="AMOUNT" />
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Button ID="btn_showExcel" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showExcel_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showExcel" PopupControlID="pl_file" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_file" runat="server" CssClass="DivPanel" Style="display: none;
                    width: 400px;">
                    <div style="width: 100%; text-align: right;">
                        <asp:ImageButton ID="ibt_close" runat="server" ImageUrl="/images/Back.gif" />
                    </div>
                    <table class="TableStyleBlue" style="width: 100%;">
                        <tr>
                            <th colspan="2" style="text-align: center;">
                                <asp:Label ID="lb_excel" runat="server" Text=""></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                檔案
                            </th>
                            <td>
                                <asp:FileUpload ID="fl_import" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" id="trCol">
                            <th>
                                第一欄資料
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rbl_firstCol" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="pol" Selected="True">員工編號</asp:ListItem>
                                    <asp:ListItem Value="id">身份證字號</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:Button ID="btn_getExcel" runat="server" Text="取得資料" OnClick="btn_getExcel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_getExcel" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
