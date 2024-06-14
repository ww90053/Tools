<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic1.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/DLBASESeardhPanel.ascx" TagName="DLBASESeardhPanel"
    TagPrefix="uc1" %>
<%@ Register Src="UserControl/UserSelector.ascx" TagName="UserSelector" TagPrefix="uc2" %>
<%@ Register Src="UserControl/PoliceSearchPanel.ascx" TagName="PoliceSearchPanel"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <link href="style/Master.css" rel="stylesheet" />

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
            background-color: #FFFF99;
        }
        .style3
        {
            text-align: right;
            background-color: #FFFF99;
        }
        .style4
        {
            border-style: Solid;
            border-color: inherit;
            border-width: 0px;
            margin-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Basic" runat="server">
                <table class="TableStyleYellow" style="width: 98%;">
                    <tr>
                        <td colspan="5">
                            <div class="PageTitle">
                                薪資基本資料
                            </div>
                        </td>
                        <td style="text-align: right; background-color: #6699FF; color: White;">
                            <div>
                                <asp:Label ID="Label_Pages" runat="server" Text=""></asp:Label>
                                <asp:TextBox ID="txt_PageIndex" runat="server" Width="30px" MaxLength="4" Style="text-align: right;"
                                    OnTextChanged="txt_PageIndex_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txt_PageIndex_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txt_PageIndex" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <asp:Label ID="Label_PagesFooter" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 75px;">
                            <a class="must">*身分證號</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                            <asp:Label ID="lb_idcard" runat="server" Text=""></asp:Label>
                        </td>
                        <th style="width: 75px;">
                            <a class="must">*姓名</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="4" Width="80px"></asp:TextBox>
                            <asp:Label ID="lb_name" runat="server" Text=""></asp:Label>
                        </td>
                        <th style="width: 75px;">
                            <a class="must">*職稱</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" AutoPostBack="true" MaxLength="4"
                                Width="40px" OnTextChanged="txt_occc_TextChanged"></asp:TextBox>
                            <asp:Button ID="btn_occc" runat="server" Text="V" OnClick="btn_occc_Click" />
                            <asp:Label ID="lb_occc" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <a class="must">*編制機關</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" AutoPostBack="true" MaxLength="10"
                                Width="80px" OnTextChanged="txt_ad_TextChanged"></asp:TextBox>
                            <asp:Button ID="btn_ad" runat="server" Text="V" OnClick="btn_ad_Click" />
                            <asp:Label ID="lb_ad" runat="server" Text=""></asp:Label>
                        </td>
                        <th>
                            <a class="must">*編制單位</a>
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" AutoPostBack="true" MaxLength="4"
                                Width="40px" OnTextChanged="txt_unit_TextChanged"></asp:TextBox>
                            <asp:Button ID="btn_unit" runat="server" Text="V" OnClick="btn_unit_Click" />
                            <asp:Label ID="lb_unit" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <a class="must">*現服機關</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" AutoPostBack="true" MaxLength="10"
                                Width="80px" OnTextChanged="txt_exad_TextChanged"></asp:TextBox>
                            <asp:Button ID="btn_exad" runat="server" Text="V" OnClick="btn_exad_Click" />
                            <asp:Label ID="lb_exad" runat="server" Text=""></asp:Label>
                        </td>
                        <th>
                            <a class="must">*現服單位</a>
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" AutoPostBack="true" MaxLength="4"
                                Width="40px" OnTextChanged="txt_exunit_TextChanged"></asp:TextBox>
                            <asp:Button ID="btn_exunit" runat="server" Text="V" OnClick="btn_exunit_Click" />
                            <asp:Label ID="lb_exunit" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel_ButtonChange" runat="server">
                <asp:Button ID="btBasic" runat="server" Text="基本資料" OnClick="btBasic_Click" />
                <asp:Button ID="btMonneySet" runat="server" OnClick="btMonneySet_Click" Text="薪資設定(應發)" />
                <asp:Button ID="btMonneySet_DE" runat="server" Text="薪資設定(應扣)" OnClick="btMonneySet_DE_Click" />
                <asp:Button ID="btBankSet" runat="server" OnClick="btBankSet_Click" Text="銀行帳號設定" />
            </asp:Panel>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="vw_Basic" runat="server">
                    <asp:Panel ID="Panel_v1" runat="server" Height="350px" GroupingText="人員基本資料">
                        <table border="1" class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th>
                                    <a class="must">發薪機關</a>
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCHI"
                                        DataValueField="MZ_KCODE" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    <a class="must">*員工編號</a>
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    出生年月日
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox_MZ_BIR" runat="server" CssClass="style4" Width="100px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                                <th>
                                    性別
                                </th>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rbl_sex" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                        <asp:ListItem Value="1" Selected="True">男</asp:ListItem>
                                        <asp:ListItem Value="2">女</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    聯絡電話(宅)
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox_MZ_PHONE" runat="server" CssClass="style4" Width="100px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                                <th>
                                    手機
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox_MZ_PHONO" runat="server" CssClass="style4" Width="100px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    通訊地址
                                </th>
                                <td colspan="7">
                                    <asp:TextBox ID="TextBox_MZ_ADD2" runat="server" CssClass="style4" Width="80%" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    戶籍地址
                                </th>
                                <td colspan="7">
                                    <asp:TextBox ID="TextBox_MZ_ADD1" runat="server" CssClass="style4" Width="80%" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    薪俸職等
                                </th>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txt_MZ_SRANK" runat="server"  MaxLength="3" Width="30px" ReadOnly="True" Enabled="False"
                                       ></asp:TextBox>
                                   
                                    <asp:Label ID="lbl_MZ_SRANK" runat="server" Text=""></asp:Label>
                                </td>
                                <th style="width: 60px;">
                                    俸階
                                </th>
                                <td style="width: 140px;">
                                    <asp:TextBox ID="txt_slvc" runat="server"  MaxLength="3" Width="30px" ReadOnly="True" Enabled="False"
                                        ></asp:TextBox>
                                  
                                    <asp:Label ID="lbl_slvc" runat="server" Text=""></asp:Label>
                                </td>
                                <th>
                                    俸點
                                </th>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="TextBox_MZ_SPT" AutoPostBack="true" runat="server" Width="40px"
                                        MaxLength="4" OnTextChanged="TextBox_MZ_SPT_TextChanged" ReadOnly="True" Enabled="False"></asp:TextBox>
                                </td>
                                <th>
                                    暫支俸點
                                </th>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txt_tempSPT" AutoPostBack="true" runat="server" Width="40px" MaxLength="4"
                                        OnTextChanged="TextBox_MZ_SPT_TextChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <th>
                                    健保百分比
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_HEALPER_Data" runat="server" CssClass="style4" Width="100%" ReadOnly="True" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    鑑識人員
                                </th>
                                <td>
                                    <asp:RadioButtonList ID="rbl_IsCrimelab" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rbl_IsCrimelab_SelectedIndexChanged1" Enabled="False">
                                        <asp:ListItem Value="否" Text="否" Selected="True">否</asp:ListItem>
                                        <asp:ListItem Value="是" Text="是"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <th>
                                    考績等級
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_GRADE" runat="server" Width="40px" MaxLength="1" ReadOnly="True" Enabled="False"></asp:TextBox>
                                </td>
                                <th>
                                    初任公職日
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_FDATE" runat="server" Width="60px" MaxLength="7" Enabled="false" ReadOnly="True"></asp:TextBox>
                                </td>
                                <th>
                                    到職日
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_ADATE" runat="server" Width="60px" MaxLength="7" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    離職日
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_LDATE" runat="server" Width="60px" MaxLength="7" AutoPostBack="true"
                                        OnTextChanged="txt_MZ_LDATE_TextChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                                <th>
                                    停職日
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_MZ_TDATE" runat="server" Width="60px" MaxLength="7" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                                <th>
                                    復職日
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_MZ_ODATE" runat="server" MaxLength="7" Width="60px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    主管級別
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_pchief" runat="server" MaxLength="3" Width="50px" ReadOnly="True" Enabled="False" ></asp:TextBox>
                                  
                                    <asp:Label ID="lbl_pchief" runat="server" Text=""></asp:Label>
                                </td>
                                <th>
                                    任主管月數
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox_MZ_PCHIEFDATE" runat="server" CssClass="style4" Width="30px"
                                        MaxLength="2" Enabled="False" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    是否兼代職
                                </th>
                                <td>
                                    <asp:RadioButtonList ID="rbl_isExtpos" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                        <asp:ListItem Value="Y">是</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="True">否</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <th>
                                    兼代職名稱
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_MZ_EXTPOS" runat="server" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    兼代職職等
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_extposSrank" runat="server" MaxLength="3" Width="50px" OnTextChanged="txt_extposSrank_TextChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                    <asp:Button ID="btn_showExtposSrank" runat="server" Text="V" OnClick="btn_showExtposSrank_Click" Enabled="False" />
                                    <asp:Label ID="lbl_extposSrank" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    任職原因
                                </th>
                                <td colspan="7">
                                    <asp:DropDownList ID="ddl_MZ_NREA" runat="server" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_ShowNREA" runat="server" Text="V" OnClick="btn_ShowNREA_Click" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vw_Amount" runat="server">
                    <asp:Panel ID="Panel_v2" runat="server" Height="350px">
                        <asp:Panel ID="Panel_IN" runat="server" GroupingText="應發項目">
                            <table class="TableStyleBlue" style="width: 100%;">
                                <tr>
                                    <th>
                                        是否停職
                                    </th>
                                    <td>
                                        <asp:RadioButtonList ID="rbl_IsOffDuty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbl_IsOffDuty_SelectedIndexChanged"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="否" Selected="True">未停職</asp:ListItem>
                                            <asp:ListItem Value="是">停職中</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        薪俸
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_SALARY_PAY1" runat="server" Width="80px" AutoPostBack="True"
                                            OnTextChanged="payChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_SALARY_PAY1_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_SALARY_PAY1"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        主管加給
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_BOSS" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="payChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_BOSS_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_BOSS" ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        <a class="must">*專業加給</a>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox_PROFESS" runat="server" Width="80px" AutoPostBack="True"
                                            OnTextChanged="payChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_PROFESS_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_PROFESS"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <a class="must">*技術加給</a>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_TECHNICS" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_TECHNICS"
                                            DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_TECHNICS_DataBound"
                                            OnSelectedIndexChanged="DropDownList_TECHNICS_SelectedIndexChanged" Enabled="False">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_TECHNICS" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY FROM B_TECHNICS ORDER BY PAY">
                                        </asp:SqlDataSource>
                                    </td>
                                    <th>
                                        <a class="must">*警勤加給</a>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_WORKP" runat="server" DataSourceID="SqlDataSource_WORKP"
                                            DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_WORKP_DataBound"
                                            OnSelectedIndexChanged="DropDownList_WORKP_SelectedIndexChanged" AutoPostBack="True" Enabled="False">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_WORKP" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY FROM B_WORKP ORDER BY PAY">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        工作獎助金
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_BONUS" runat="server" DataSourceID="SqlDataSource_BONUS"
                                            DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_BONUS_DataBound"
                                            AutoPostBack="True" OnSelectedIndexChanged="payChanged" Enabled="False">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_BONUS" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY FROM B_BONUS ORDER BY PAY">
                                        </asp:SqlDataSource>
                                    </td>
                                    <th>
                                        外事加給
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_ADVENTIVE" runat="server" DataSourceID="SqlDataSource_ADVENTIVE"
                                            DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_ADVENTIVE_DataBound"
                                            AutoPostBack="True" OnSelectedIndexChanged="payChanged" Enabled="False">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_ADVENTIVE" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY FROM B_ADVENTIVE ORDER BY PAY">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        偏遠加給
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_Far" runat="server" MaxLength="5" Width="80px" AutoPostBack="True"
                                            OnTextChanged="payChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <th>
                                        勤務繁重加給
                                    </th>
                                    <td>
                                    
                                        <asp:TextBox ID="txt_ELECTRIC" runat="server" Width="80px" AutoPostBack="True" 
                                            ontextchanged="txt_ELECTRIC_TextChanged" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <%--<asp:DropDownList ID="DropDownList_ELECTRIC" runat="server" DataSourceID="SqlDataSource_ELECTRIC"
                                            DataTextField="NAME" DataValueField="ID" OnDataBound="DropDownList_ELECTRIC_DataBound"
                                            AutoPostBack="True" OnSelectedIndexChanged="payChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_ELECTRIC" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, NAME, PAY, CREATEDATE FROM B_ELECTRIC ORDER BY PAY">
                                        </asp:SqlDataSource>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        其他應發
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_OtherAdd" runat="server" Width="60px" AutoPostBack="True" OnTextChanged="payChanged"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        薪資備註
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox_SALARYPAY_NOTE" runat="server" Width="85%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vw_Des" runat="server">
                    <asp:Panel ID="Panel_v6" runat="server" Height="350px">
                        <asp:Panel ID="Panel_DE" runat="server" GroupingText="應扣項目">
                            <table class="TableStyleBlue" style="width: 100%;">
                                <%--<tr>
                                    <th>
                                        任公職是否滿30年
                                    </th>
                                    <td colspan="5">
                                        <asp:RadioButtonList ID="rbl_30Year" runat="server" OnSelectedIndexChanged="rbl_30Year_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" AutoPostBack="true">
                                            <asp:ListItem Value="否" Text="否" Selected="True">否</asp:ListItem>
                                            <asp:ListItem Value="是" Text="是"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <th>
                                        投保類別
                                    </th>
                                    <td>
                                        <asp:RadioButtonList ID="RadioButtonList_INSURANCE_GROUP" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow"  Enabled="False" >
                                            <asp:ListItem Selected="True" Value="公保">公保</asp:ListItem>
                                            <asp:ListItem Value="勞保">勞保</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        <a class="must">*公(勞)保費</a>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox_SALARY_GOV_INSURANCE" runat="server" Width="80px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_SALARY_GOV_INSURANCE_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_SALARY_GOV_INSURANCE"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        是否預扣所得
                                    </th>
                                    <td>
                                        <asp:RadioButtonList ID="RadioButtonList_TAXPER_GROUP" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList_TAXPER_GROUP_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="否">否</asp:ListItem>
                                            <asp:ListItem Value="是">是</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        所得稅撫養人
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_TAXCHILD" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_TAXCHILD_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_TAXCHILD"></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        所得稅扣款金額
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_TAXPER" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_TAXPER_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_TAXPER"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        個人健保費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_HealthPersonal" runat="server" Width="50px"></asp:TextBox>
                                        狀態：
                                        <asp:DropDownList ID="ddl_Healper_Insurance" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_Healper_Insurance_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <th>
                                        <a class="must">*健保費(含本人)</a>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_HEALPER" runat="server" Width="80px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_HEALPER_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_HEALPER"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        健保加保人數
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_DE_HEALTH" runat="server" Width="20px" AutoPostBack="True"
                                            MaxLength="1" Enabled="false">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_DE_HEALTH_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_DE_HEALTH"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                        <asp:Button ID="btn_HealthInfo" runat="server" Text="加保狀態" OnClick="btn_HealthInfo_Click" />
                                        <asp:Button ID="btn_HealthInfoFake" runat="server" Text="加保狀態" Style="display: none;" />
                                        <cc1:ModalPopupExtender ID="btn_HealthInfo_ModalPopupExtender" runat="server" DynamicServicePath=""
                                            Enabled="True" TargetControlID="btn_HealthInfoFake" PopupControlID="pnl_HealthInfo"
                                            CancelControlID="btn_HealthClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
                                        <asp:Panel ID="pnl_HealthInfo" runat="server" Style="display: none; text-align: center;
                                            background-color: white; border: solid 5px gray; padding: 5px 5px 5px 5px;">
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        加保眷屬設定
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        狀態
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:DropDownList ID="ddl_HealthMode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_HealthMode_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        百分比
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox ID="txt_HealthPer" runat="server" Width="50px" MaxLength="4" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        金額
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox ID="txt_HealthCost" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        關係
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:DropDownList ID="ddl_HealthRelation" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Button ID="btn_HealthAdd" runat="server" Text="新增加保眷屬" OnClick="btn_HealthAdd_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:GridView ID="gv_HealthInfo" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None" OnRowCommand="gv_HealthInfo_RowCommand">
                                                            <RowStyle BackColor="#EFF3FB" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Mode" HeaderText="加保狀態" />
                                                                <asp:BoundField DataField="Relation" HeaderText="關係" />
                                                                <asp:BoundField DataField="Cost" HeaderText="費用" />
                                                                <asp:TemplateField><ItemTemplate><asp:Button ID="Button1" runat="server" Text="移除" OnClientClick="return confirm('確定執行嗎？');"
                                                                            CommandArgument='<%# Eval("SN") %>' /></ItemTemplate></asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <EditRowStyle BackColor="#2461BF" />
                                                            <AlternatingRowStyle BackColor="White" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center;">
                                                        <asp:Button ID="btn_HealthClose" runat="server" Text="關閉視窗" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        是否扣退撫金
                                    </th>
                                    <td>
                                        <asp:RadioButtonList ID="RadioButtonList_CONCUR3" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="RadioButtonList_CONCUR3_SelectedIndexChanged" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Selected="True" Value="1">扣退撫金</asp:ListItem>
                                            <asp:ListItem Value="2">不扣退撫金</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        退撫金費
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox_CONCUR3" runat="server" Width="80px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_CONCUR3_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_CONCUR3"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        薪資扣款1
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_MONTHPAY_TAX" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_TAX_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MONTHPAY_TAX"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        薪資扣款2
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox_MONTHPAY" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MONTHPAY_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MONTHPAY"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        其他應扣
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_otherMinus" runat="server" Width="60px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel_O_DE" runat="server" GroupingText="其他應扣項目">
                            <table class="TableStyleBlue" style="width: 100%;">
                                <tr>
                                    <th>
                                        是否法院扣款
                                    </th>
                                    <td>
                                        <asp:RadioButtonList ID="RadioButtonList_EXTRA01_GROUP" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="RadioButtonList_EXTRA01_GROUP_SelectedIndexChanged" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="是">是</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="否">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        法院扣款
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox ID="TextBox_EXTRA01" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA01_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA01"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        國宅貸款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA02" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA02_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA02"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        銀行貸款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA03" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA03_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA03"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        分期付款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA04" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA04_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA04"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        優惠存款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA05" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA05_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA05"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        員工宿舍費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA06" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA06_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA06"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        福利互助金費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA07" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA07_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA07"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        伙食費
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA08" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA08_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA08"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <th>
                                        退撫金貸款
                                    </th>
                                    <td>
                                        <asp:TextBox ID="TextBox_EXTRA09" runat="server" Width="50px" AutoPostBack="True">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_EXTRA09_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_EXTRA09"
                                            ValidChars="-$,."></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vw_Account" runat="server">
                    <asp:Panel ID="Panel_v4" runat="server" Height="350px" GroupingText="銀行資料">
                        <table class="TableStyleBlue" style="width: 100%;">
                            <tr>
                                <th>
                                    <a class="must">*群組</a>
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList ID="DropDownList_GROUP" runat="server" Enabled="False">
                                        <asp:ListItem Selected="True" Value="1">優惠存款</asp:ListItem>
                                        <asp:ListItem Value="2">薪資轉帳</asp:ListItem>
                                        <asp:ListItem Value="3">國宅貸款</asp:ListItem>
                                        <asp:ListItem Value="4">退撫金貸款</asp:ListItem>
                                        <asp:ListItem Value="5">分期付款</asp:ListItem>
                                        <asp:ListItem Value="6">銀行代款</asp:ListItem>
                                        <asp:ListItem Value="7">法院扣款</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <a class="must">*銀行</a>
                                </th>
                                <td>
                                    <asp:DropDownList ID="DropDownList_BANK" runat="server" DataSourceID="SqlDataSource_BANK_LIST"
                                        DataTextField="IDNAME" DataValueField="BANK_ID" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource_BANK_LIST" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT BANK_ID, BANK_NAME, BANK_ID +'('+ BANK_NAME +')' AS IDNAME FROM B_BANK_LIST ORDER BY BANK_ID">
                                    </asp:SqlDataSource>
                                </td>
                                <th>
                                    <a class="must">*帳號</a>
                                </th>
                                <td>
                                    <asp:TextBox ID="TextBox_STOCKPILE_BANKID" runat="server" MaxLength="16" Enabled="False" ReadOnly="True"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_STOCKPILE_BANKID_FilteredTextBoxExtender"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_STOCKPILE_BANKID"
                                        ValidChars="-"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="GridView_STOCKPILE" runat="server" CellPadding="4" GridLines="None"
                            Width="100%" AutoGenerateColumns="False" DataKeyNames="BS_SNID,PAY_AD" EmptyDataText="無銀行資料"
                            OnRowCommand="GridView_STOCKPILE_RowCommand" ForeColor="#333333" OnRowDataBound="GridView_STOCKPILE_RowDataBound"
                            AllowPaging="True" PageSize="7" OnPageIndexChanging="GridView_STOCKPILE_PageIndexChanging">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <%--<asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" />--%>
                                <asp:TemplateField><ItemTemplate><asp:Button ID="btnSelect" Visible="false" runat="server" Text="選取" CommandName="btSelect" CommandArgument='<%# Eval("BS_SNID") %>' /></ItemTemplate></asp:TemplateField>
                                <asp:BoundField DataField="BANK_NAME_DATA" HeaderText="銀行" SortExpression="BANK_NAME_DATA" />
                                <asp:BoundField DataField="STOCKPILE_BANKID" HeaderText="銀行帳號" SortExpression="STOCKPILE_BANKID" />
                                <asp:BoundField DataField="GROUP" HeaderText="群組" SortExpression="GROUP" />
                                <asp:BoundField DataField="MZ_KCHI" HeaderText="機關" SortExpression="PAY_AD" />
                                <asp:TemplateField ShowHeader="False" Visible="false"><ItemTemplate><asp:Button ID="btn_DeleteAccount" runat="server" CausesValidation="false" CommandName="btDelete"
                                            CommandArgument='<%# Eval("BS_SNID") %>' OnClientClick="return confirm('確定要刪除？');"
                                            Text="刪除" /></ItemTemplate></asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <asp:Panel ID="Panel_00" runat="server">
                <table class="style1">
                    <tr>
                        <td style="background-color: #6699FF; color: White;">
                            <asp:Button ID="btTable" runat="server" OnClick="btTable_Click" Text="查詢" />
                            <asp:Button ID="btCreate" Visible="false" runat="server" OnClick="btCreate_Click" Text="新增" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="儲存" />
                           
                            <asp:Button ID="btExit" runat="server" OnClick="btExit_Click" Text="取消" />
                            <asp:Button ID="btBack_Data" runat="server" Text="上一筆" OnClick="btBack_Data_Click" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNext_Data" runat="server" Text="下一筆" OnClick="btNext_Data_Click" />
                            <asp:Button ID="btn_fastMenu" runat="server" Text="快速選單" OnClick="btn_fastMenu_Click" />
                            <asp:Button ID="btn_export" runat="server" Text="匯出excel" OnClick="btn_export_Click" />
                            <asp:Button ID="btn_import" runat="server" Text="考績晉階晉級excel匯入" OnClick="btn_import_Click" />
                            <asp:Button ID="btn_recalculate" runat="server" Text="重新計算個人基本薪資" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div>
                <asp:Button ID="btn_ShowCodeSelector" runat="server" Text="Button" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_ShowModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_ShowCodeSelector" PopupControlID="pnl_CodeSelector"
                    BackgroundCssClass="modalBackground" CancelControlID="ibt_CloseCodeSelector">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnl_CodeSelector" runat="server" CssClass="DivPanel" Style="display: none;">
                    <div style="text-align: right;">
                        <asp:ImageButton ID="ibt_CloseCodeSelector" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    <uc1:DLBASESeardhPanel ID="DLBASESeardhPanel1" runat="server" />
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btn_showFastMenu" runat="server" Text="Button" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showFastMenu_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showFastMenu" PopupControlID="pnl_fastMenu"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnl_fastMenu" runat="server" Style="display: none;">
                    <uc2:UserSelector ID="UserSelector1" runat="server" />
                </asp:Panel>
            </div>
            <div>
                <cc1:ModalPopupExtender ID="btn_showSearch_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btTable" PopupControlID="Panel1" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel1" runat="server" Style="display: none;" CssClass="DivPanel">
                    <div style="text-align: right;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    <uc3:PoliceSearchPanel ID="PoliceSearchPanel1" runat="server" />
                </asp:Panel>
            </div>
            <div>
                <%--跳窗區塊,點下 重新計算個人薪資 按鈕後,輸入 身分證號 等資訊--%> 
            <cc1:ModalPopupExtender ID="btn_recalculate_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_recalculate" PopupControlID="pl_recalculate" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_recalculate" runat="server" Style="display: none;" CssClass="DivPanel">
                <div style="text-align: right;">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    
                    <div>
                    <table>
                    <tr>
                    <th>
                        發薪機關
                        </th>
                        <td>
                            <asp:Label ID="lb_RE_PAYAD" runat="server" ></asp:Label>
                        </td>
                      </tr> 
                    <tr>
                    <th>
                        身分證號
                        </th>
                        <td><asp:TextBox ID="txt_recalculate_ID" runat="server"></asp:TextBox></td>
                      </tr> 
                      <tr>
                      <td colspan="2">
                      未輸入身分證號以所屬機關計算                      
                     </td>
                      </tr>
                        <tr>
                        <td colspan="2">
                        <center>
                        <asp:Button ID="btn_recalculate_start" runat="server" Text="計算"  onclick="btn_recalculate_start_Click" />
                         </center> <br />  

                            
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                            AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" 
                                    style="width: 328px; height: 19px" /><br />
                                <span class="style3">處理中 請稍候…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                            
                            
                    </td>
                    </tr>
                    
                    </table>
                    </div>
                    
                </asp:Panel>
            
            
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
