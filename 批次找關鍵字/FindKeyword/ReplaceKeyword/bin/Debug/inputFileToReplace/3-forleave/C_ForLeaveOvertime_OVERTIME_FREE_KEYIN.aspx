<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_OVERTIME_FREE_KEYIN.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_OVERTIME_FREE_KEYIN" Title="超勤(勤務項目)時數輸入" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
        /*
        function onSubmitOK() {
            //抓取
            var isExist = document.getElementById('ctl00_ContentPlaceHolder1_TextBox_isExist_C_DUTYMONTHOVERTIME_HOUR').value;
            //要不要警告
            if (isExist == 'N') {
                return true;
            }
            return confirm('您輸入超過1個月的歷史超勤資料，請確認輸入之日期是否正確。');
        }
        */
    </script>

    <style type="text/css">
        #lstTimes select {
            width: 80px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: left;">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <!-- SQLDataSource 取得超勤類別 -->
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"
            SelectCommand="SELECT * FROM C_DUTYITEM ORDER BY DUTY_NO"
            DataSourceMode="DataReader"></asp:SqlDataSource>

        <!-- SQLDataSource 載入人員下拉，主要根據登入者的現服機關單位 -->
        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
            ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"
            DataSourceMode="DataReader"></asp:SqlDataSource>
        <%--當日的C_DUTYMONTHOVERTIME_HOUR(超勤資料)存在否?--%>
        <span style="display: none">
            <asp:TextBox ID="TextBox_isExist_C_DUTYMONTHOVERTIME_HOUR" runat="server" AutoPostBack="True"></asp:TextBox>

        </span>
        <div style="width: 800px; text-align: left;">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                <ContentTemplate>
                    <asp:Panel ID="Panel3" runat="server">
                        <table class="style10">
                            <tr>
                                <td class="style4" style="text-align: left;"><span>超勤(勤務項目)時數輸入</span></td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style7">人 員</td>
                                <td class="style3">

                                    <asp:DropDownList ID="DropDownList_MZ_NAME" runat="server"
                                        DataSourceID="SqlDataSource3"
                                        DataTextField="MZ_NAME"
                                        DataValueField="MZ_ID"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="DropDownList_MZ_NAME_SelectedIndexChanged">
                                    </asp:DropDownList>

                                    &nbsp;
                                <asp:Label ID="lbName" runat="server" Font-Bold="True" ForeColor="#0033FF" Visible="False"></asp:Label>
                                    <asp:Label ID="lblIDNAME" runat="server" Font-Bold="True" Text="請輸入身份證字號：" Visible="False"></asp:Label>
                                    <asp:TextBox ID="tbMZ_ID" runat="server" AutoPostBack="True"
                                        OnTextChanged="tbMZ_ID_TextChanged" Visible="False" MaxLength="10"
                                        Width="100px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                <asp:TextBox ID="TextBox_DUTYDATE" runat="server" MaxLength="7" Width="65px" AutoPostBack="true" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_DUTYDATE_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_DUTYDATE">
                                    </cc1:FilteredTextBoxExtender>
                                    (範例：0990101)&nbsp;&nbsp;<asp:Label ID="Label_ADUN" runat="server" Font-Bold="True"
                                        ForeColor="#0033FF"></asp:Label>
                                    &nbsp;&nbsp;
                                <asp:Button ID="btnJobRelocationInput" runat="server" Enabled="true" ForeColor="#CC0000" OnClick="btnJobRelocationInput_Click" Text="調離單位申請" Width="100px" />
                                    <asp:Button ID="btnJobRelocationConfrm" runat="server" Visible="false"
                                        Text="確認" OnClick="btnJobRelocationConfrm_Click" OnClientClick="return ValidateInput();" />
                                    <asp:Button ID="btnJobRelocationCancel" runat="server" Visible="false"
                                        Text="取消" OnClick="btnJobRelocationCancel_Click" />

                                </td>

                            </tr>
                            <tr>
                                <td class="style7">時 段
                                </td>
                                <td class="style3">
                                    <asp:Panel ID="Panel2" runat="server">
                                        <table id="lstTimes" style="border-style: solid; border-width: 1px; padding: 1px 4px; width: 100%;">
                                            <tr>
                                                <td>06:<asp:TextBox ID="TextBox_DUTYITEM1_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-07:<asp:TextBox
                                                    ID="TextBox_DUTYITEM1_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM1" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>07:<asp:TextBox ID="TextBox_DUTYITEM2_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-08:<asp:TextBox
                                                    ID="TextBox_DUTYITEM2_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM2" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>08:<asp:TextBox ID="TextBox_DUTYITEM3_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-09:<asp:TextBox
                                                    ID="TextBox_DUTYITEM3_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM3" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>09:<asp:TextBox ID="TextBox_DUTYITEM4_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-10:<asp:TextBox
                                                    ID="TextBox_DUTYITEM4_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM4" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>10:<asp:TextBox ID="TextBox_DUTYITEM5_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-11:<asp:TextBox
                                                    ID="TextBox_DUTYITEM5_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM5" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>11:<asp:TextBox ID="TextBox_DUTYITEM6_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-12:<asp:TextBox
                                                    ID="TextBox_DUTYITEM6_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM6" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>12:<asp:TextBox ID="TextBox_DUTYITEM7_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-13:<asp:TextBox
                                                    ID="TextBox_DUTYITEM7_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM7" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>13:<asp:TextBox ID="TextBox_DUTYITEM8_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-14:<asp:TextBox
                                                    ID="TextBox_DUTYITEM8_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM8" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>14:<asp:TextBox ID="TextBox_DUTYITEM9_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-15:<asp:TextBox
                                                    ID="TextBox_DUTYITEM9_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM9" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>15:<asp:TextBox ID="TextBox_DUTYITEM10_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-16:<asp:TextBox
                                                    ID="TextBox_DUTYITEM10_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM10" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>16:<asp:TextBox ID="TextBox_DUTYITEM11_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-17:<asp:TextBox
                                                    ID="TextBox_DUTYITEM11_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM11" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>17:<asp:TextBox ID="TextBox_DUTYITEM12_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-18:<asp:TextBox
                                                    ID="TextBox_DUTYITEM12_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM12" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>18:<asp:TextBox ID="TextBox_DUTYITEM13_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-19:<asp:TextBox
                                                    ID="TextBox_DUTYITEM13_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM13" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>19:<asp:TextBox ID="TextBox_DUTYITEM14_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-20:<asp:TextBox
                                                    ID="TextBox_DUTYITEM14_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM14" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>20:<asp:TextBox ID="TextBox_DUTYITEM15_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-21:<asp:TextBox
                                                    ID="TextBox_DUTYITEM15_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM15" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>21:<asp:TextBox ID="TextBox_DUTYITEM16_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-22:<asp:TextBox
                                                    ID="TextBox_DUTYITEM16_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM16" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>22:<asp:TextBox ID="TextBox_DUTYITEM17_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-23:<asp:TextBox
                                                    ID="TextBox_DUTYITEM17_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM17" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>23:<asp:TextBox ID="TextBox_DUTYITEM18_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-24:<asp:TextBox
                                                    ID="TextBox_DUTYITEM18_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM18" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>24:<asp:TextBox ID="TextBox_DUTYITEM19_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-01:<asp:TextBox
                                                    ID="TextBox_DUTYITEM19_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM19" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>01:<asp:TextBox ID="TextBox_DUTYITEM20_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-02:<asp:TextBox
                                                    ID="TextBox_DUTYITEM20_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM20" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>02:<asp:TextBox ID="TextBox_DUTYITEM21_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-03:<asp:TextBox
                                                    ID="TextBox_DUTYITEM21_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM21" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>03:<asp:TextBox ID="TextBox_DUTYITEM22_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-04:<asp:TextBox
                                                    ID="TextBox_DUTYITEM22_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM22" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>04:<asp:TextBox ID="TextBox_DUTYITEM23_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-05:<asp:TextBox
                                                    ID="TextBox_DUTYITEM23_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM23" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>05:<asp:TextBox ID="TextBox_DUTYITEM24_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-06:<asp:TextBox
                                                    ID="TextBox_DUTYITEM24_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM24" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>06:<asp:TextBox ID="TextBox_DUTYITEM25_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-07:<asp:TextBox
                                                    ID="TextBox_DUTYITEM25_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM25" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>07:<asp:TextBox ID="TextBox_DUTYITEM26_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-08:<asp:TextBox
                                                    ID="TextBox_DUTYITEM26_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM26" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>08:<asp:TextBox ID="TextBox_DUTYITEM27_1" runat="server" Width="15px" MaxLength="2"></asp:TextBox>-09:<asp:TextBox
                                                    ID="TextBox_DUTYITEM27_2" runat="server" Width="15px" MaxLength="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_DUTYITEM27" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SqlDataSource2" DataTextField="DUTY_NAME" DataValueField="DUTY_NO">
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="3" style="color: #0033FF; font-weight: bold;">超勤補休時數 :
                                                    <asp:TextBox ID="txtConvertRest" runat="server" Width="30px" MaxLength="2"></asp:TextBox></td>
                                            </tr>

                                            <tr>
                                                <%--<td colspan="3">超勤補休時數 : <asp:TextBox ID="txtConvertRest" runat="server" Width="30px" MaxLength="2"></asp:TextBox></td>--%>
                                                <%--<td> 是否停休：</td>--%>
                                                <td colspan="2">

                                                    <div style="float: left; color: #0033FF; font-weight: bold;">
                                                        <%--筆記:以後可能改叫做"停休(得實報實銷)"--%>
                                                &nbsp;&nbsp;&nbsp;&nbsp; 
                                                <asp:Label ID="lbColName_StopOff" runat="server" Text="停休(得實報實銷)"></asp:Label>
                                                        :
                                                    </div>
                                                    <div style="float: right">
                                                        <asp:RadioButtonList ID="RadioButtonList_StopOff" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Y">是</asp:ListItem>
                                                            <asp:ListItem Value="N" Selected="True">否</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </td>
                                                <%--<td>&nbsp;</td>--%>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="RadioButtonList_WorkP" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Outter">外勤人員</asp:ListItem>
                                                        <asp:ListItem Value="Inner">內勤人員</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td colspan="3" style="display: none">發薪機關 :&nbsp;
                                                <asp:DropDownList ID="DropDownList_AD" runat="server" AutoPostBack="True" Width="150" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged"
                                                    RepeatDirection="Horizontal">
                                                </asp:DropDownList>
                                                    編制單位 :
                                                </td>
                                                <td colspan="3" style="color: #0033FF; font-weight: bold;">編制單位 :&nbsp;
                                                <asp:DropDownList ID="DropDownList_UNIT" runat="server" RepeatDirection="Horizontal" Width="150">
                                                </asp:DropDownList>
                                                </td>
                                                <%--</tr>
                                        <tr>
                                            <td colspan="6">是否停休 : 
                                                <asp:RadioButtonList ID="RadioButtonList_StopOff" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Y" >是</asp:ListItem>
                                                <asp:ListItem Value="N" Selected ="True">否</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>--%>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="style7">備註
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_MEMO" runat="server" Width="450px" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="width: 800px; text-align: left; overflow: auto; height: 170px;">
                            <asp:GridView ID="GridView1" runat="server" Width="1000px" CellPadding="4" GridLines="None"
                                AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand"
                                OnRowDataBound="GridView1_RowDataBound" Style="text-align: center" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" SortExpression="MZ_ID" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                                    <asp:BoundField DataField="DUTYDATE" HeaderText="日期" SortExpression="DUTYDATE" />
                                    <asp:BoundField DataField="DUTYITEM1" HeaderText="6" SortExpression="DUTYITEM1" />
                                    <asp:BoundField DataField="DUTYITEM2" HeaderText="7" SortExpression="DUTYITEM2" />
                                    <asp:BoundField DataField="DUTYITEM3" HeaderText="8" SortExpression="DUTYITEM3" />
                                    <asp:BoundField DataField="DUTYITEM4" HeaderText="9" SortExpression="DUTYITEM4" />
                                    <asp:BoundField DataField="DUTYITEM5" HeaderText="10" SortExpression="DUTYITEM5" />
                                    <asp:BoundField DataField="DUTYITEM6" HeaderText="11" SortExpression="DUTYITEM6" />
                                    <asp:BoundField DataField="DUTYITEM7" HeaderText="12" SortExpression="DUTYITEM7" />
                                    <asp:BoundField DataField="DUTYITEM8" HeaderText="13" SortExpression="DUTYITEM8" />
                                    <asp:BoundField DataField="DUTYITEM9" HeaderText="14" SortExpression="DUTYITEM9" />
                                    <asp:BoundField DataField="DUTYITEM10" HeaderText="15" SortExpression="DUTYITEM10" />
                                    <asp:BoundField DataField="DUTYITEM11" HeaderText="16" SortExpression="DUTYITEM11" />
                                    <asp:BoundField DataField="DUTYITEM12" HeaderText="17" SortExpression="DUTYITEM12" />
                                    <asp:BoundField DataField="DUTYITEM13" HeaderText="18" SortExpression="DUTYITEM13" />
                                    <asp:BoundField DataField="DUTYITEM14" HeaderText="19" SortExpression="DUTYITEM14" />
                                    <asp:BoundField DataField="DUTYITEM15" HeaderText="20" SortExpression="DUTYITEM15" />
                                    <asp:BoundField DataField="DUTYITEM16" HeaderText="21" SortExpression="DUTYITEM16" />
                                    <asp:BoundField DataField="DUTYITEM17" HeaderText="22" SortExpression="DUTYITEM17" />
                                    <asp:BoundField DataField="DUTYITEM18" HeaderText="23" SortExpression="DUTYITEM18" />
                                    <asp:BoundField DataField="DUTYITEM19" HeaderText="24" SortExpression="DUTYITEM19" />
                                    <asp:BoundField DataField="DUTYITEM20" HeaderText="1" SortExpression="DUTYITEM20" />
                                    <asp:BoundField DataField="DUTYITEM21" HeaderText="2" SortExpression="DUTYITEM21" />
                                    <asp:BoundField DataField="DUTYITEM22" HeaderText="3" SortExpression="DUTYITEM22" />
                                    <asp:BoundField DataField="DUTYITEM23" HeaderText="4" SortExpression="DUTYITEM23" />
                                    <asp:BoundField DataField="DUTYITEM24" HeaderText="5" SortExpression="DUTYITEM24" />
                                    <asp:BoundField DataField="DUTYITEM25" HeaderText="6" SortExpression="DUTYITEM25" />
                                    <asp:BoundField DataField="DUTYITEM26" HeaderText="7" SortExpression="DUTYITEM26" />
                                    <asp:BoundField DataField="DUTYITEM27" HeaderText="8" SortExpression="DUTYITEM27" />
                                    <%-- <asp:BoundField DataField="DUTYITEM20" HeaderText="0" SortExpression="DUTYITEM20" />
                                <asp:BoundField DataField="DUTYITEM21" HeaderText="1" SortExpression="DUTYITEM21" />
                                <asp:BoundField DataField="DUTYITEM22" HeaderText="2" SortExpression="DUTYITEM22" />
                                <asp:BoundField DataField="DUTYITEM23" HeaderText="3" SortExpression="DUTYITEM23" />
                                <asp:BoundField DataField="DUTYITEM24" HeaderText="4" SortExpression="DUTYITEM24" />
                                <asp:BoundField DataField="DUTYITEM25" HeaderText="5" SortExpression="DUTYITEM25" />
                                <asp:BoundField DataField="DUTYITEM26" HeaderText="6" SortExpression="DUTYITEM26" />--%>

                                    <asp:BoundField DataField="TOTAL_HOURS" HeaderText="時數" SortExpression="TOTAL_HOURS" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                        <!--<SelectParameters>
                            <asp:SessionParameter Name="MZ_ID" SessionField="ADPMZ_ID" />
                        </SelectParameters>-->
                        <!--SelectCommand="SELECT  CDP.MZ_ID , MZ_NAME,  DUTYDATE ,  DUTYITEM1 ,  DUTYITEM2 ,  DUTYITEM3 ,  DUTYITEM4 ,  DUTYITEM5 ,  DUTYITEM6 ,  DUTYITEM7 ,  DUTYITEM8 ,  DUTYITEM9 ,
                            DUTYITEM10 ,  DUTYITEM11 ,  DUTYITEM12 ,  DUTYITEM13 ,  DUTYITEM14 ,  DUTYITEM15 ,  DUTYITEM16 ,  DUTYITEM17 ,  DUTYITEM18 ,  DUTYITEM19 ,  DUTYITEM20 ,  DUTYITEM21 ,  DUTYITEM22 ,  DUTYITEM23 ,  DUTYITEM24 ,  DUTYITEM25 ,  DUTYITEM26 ,(TOTAL_HOURS+ISDIRECTTIME) as TOTAL_HOURS 
                            FROM  C_DUTYTABLE_PERSONAL CDP
                            LEFT JOIN A_DLBASE AB ON AB.MZ_ID=CDP.MZ_ID   
                            WHERE CDP.MZ_ID=@MZ_ID">-->
                        <asp:Panel ID="Panel1" runat="server" Width="100%">
                            <table class="style12">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btSearch" runat="server" CausesValidation="False" CssClass="style9" OnClick="btSearch_Click" Text="查詢" />
                                        <asp:Button ID="btInsert" runat="server" OnClick="btInsert_Click" Text="新增" class="style9" AccessKey="a" />
                                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Enabled="False" class="style9" />
                                        <asp:Button ID="btOK" runat="server" Enabled="False" OnClick="btok_Click" Text="確認" class="style9" AccessKey="s"  />
                                        <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click" Text="取消" Enabled="False" class="style9" />
                                        <asp:Button ID="btDelete" runat="server" OnClick="btDelete_Click" Text="刪除" Enabled="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);" class="style9" AccessKey="d" />
                                        <asp:Button ID="btPrint" runat="server" Text="列印" Enabled="True" class="style9" OnClick="btPrint_Click" />
                                        <asp:Button ID="btnBatchModify" runat="server" CausesValidation="False" CssClass="style9" Text="服務單位整批修改" OnClick="btnBatchModify_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
