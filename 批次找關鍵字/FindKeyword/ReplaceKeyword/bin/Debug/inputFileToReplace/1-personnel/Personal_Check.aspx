<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Check.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Check" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style2
        {
            text-align: left;
        }
        .style3
        {
            text-align: left;
            background-color: #CCFFCC;
        }
        .style4
        {
            text-align: center;
            background-color: #FFCCFF;
            width: 50px;
        }
        .style5
        {
            text-align: left;
            width: 314px;
        }
        .style6
        {
            text-align: left;
            height: 25px;
        }
        .style7
        {
            text-align: left;
            width: 314px;
        }
        .style9
        {
            text-align: left;
        }
        .style12
        {
            text-align: left;
            width: 446px;
        }
        .style14
        {
            border: solid 0px;
        }
        .style15
        {
            text-align: center;
            width: 60px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style110
        {
            background-color: #FFCCFF;
            text-align: left;
        }
        .style111
        {
            border: solid 0px;
        }
        .style114
        {
            text-align: left;
            width: 127px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style116
        {
            text-align: left;
            width: 111px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style117
        {
            text-align: left;
            width: 265px;
        }
        .style118
        {
            text-align: left;
            width: 60px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style119
        {
            text-align: left;
            width: 108px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style120
        {
            text-align: left;
            background-color: #CCFFCC;
            width: 71px;
        }
        .style121
        {
            text-align: left;
            width: 299px;
        }
        .style122
        {
            text-align: left;
            background-color: #CCFFCC;
            width: 55px;
        }
        .style123
        {
            width: 190px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td style="text-align: left; font-size: large; font-family: 標楷體">
                        考核作業
                    </td>
                    <td style="text-align: right; font-size: large; font-family: 標楷體">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_Personal_Check" runat="server">
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style4">
                            身分證號
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style110">
                            姓名
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px"></asp:TextBox>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="style26"
                                OnClick="Button2_Click" TabIndex="-1" Text="姓名查詢（輸入）" Width="108px" />
                        </td>
                        <td class="style110">
                            職稱
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style14" Width="100px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="style110">
                            出生日期
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_BIR" runat="server" Width="65px"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox_MZ_BIR_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="TextBox_MZ_BIR" PopupButtonID="ibt_BIR">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style4">
                            編制機關
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" MaxLength="10"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style14" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="style4">
                            編制單位
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style14" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="style110">
                            初任公職日期
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="TextBox_MZ_FDATE" runat="server" Width="65px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            現服機關
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" Width="75px" MaxLength="10"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" CssClass="style14" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="style4">
                            現服單位
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" CssClass="style14" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="style110">
                            任現職日期
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ADATE" runat="server" Width="65px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style3">
                            最高警察學歷
                        </td>
                        <td class="style121">
                            <asp:TextBox ID="TextBox_POLICE_SCHOOL" runat="server" Width="155px" OnTextChanged="TextBox_POLICE_SCHOOL_TextChanged"></asp:TextBox>
                            <asp:Button ID="btPOLICE_SCHOOL" runat="server" CausesValidation="False" CssClass="style2"
                                OnClick="btPOLICE_SCHOOL_Click" TabIndex="-1" Text="V" Enabled="false" />
                            <asp:TextBox ID="TextBox_POLICE_SCHOOL1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="100px"></asp:TextBox>
                        </td>
                        <td class="style122">
                            科系別
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_POLICE_DEPARTMENT" runat="server" Width="100px" AutoCompleteType="Disabled"
                                OnTextChanged="TextBox_POLICE_DEPARTMENT_TextChanged"></asp:TextBox>
                            <asp:Button ID="btPOLICE_DEPARTMENT" runat="server" CausesValidation="False" CssClass="style2"
                                OnClick="btPOLICE_DEPARTMENT_Click" TabIndex="-1" Text="V" Enabled="false"/>
                            <asp:TextBox ID="TextBox_POLICE_DEPARTMENT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                        <td class="style3">
                            期別
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="75px" MaxLength="25"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            最高學歷
                        </td>
                        <td class="style121">
                            <asp:TextBox ID="TextBox_SCHOOL" runat="server" Width="155px" OnTextChanged="TextBox_SCHOOL_TextChanged"></asp:TextBox>
                            <asp:Button ID="btSCHOOL" runat="server" CausesValidation="False" OnClick="btSCHOOL_Click"
                                TabIndex="-1" Text="V" Enabled="false" />
                            <asp:TextBox ID="TextBox_SCHOOL1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="100px"></asp:TextBox>
                                <asp:Label ID="lb_School_Code" runat="server" Text="" Visible="false"></asp:Label> <%--學校代碼--%>
                        </td>
                        <td class="style122">
                            科系別
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_SCHOOL_DEPARTMENT" runat="server" Width="100px" OnTextChanged="TextBox_SCHOOL_DEPARTMENT_TextChanged"></asp:TextBox>
                            <asp:Button ID="btSCHOOL_DEPARTMENT" runat="server" CausesValidation="False" CssClass="style2"
                                OnClick="btDEPARTMENT_Click" TabIndex="-1" Text="V" Enabled="false"/>
                            <asp:TextBox ID="TextBox_SCHOOL_DEPARTMENT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                        <td class="style3">
                            檢索號碼
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_RETRIEVE" runat="server" Width="90px" MaxLength="12"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style120">
                            現居住所
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="TextBox_MZ_ADD2" runat="server" Width="400px"></asp:TextBox>
                        </td>
                        <td class="style3">
                            婚姻狀況
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="DropDownList_MZ_SM" runat="server" AppendDataBoundItems="True"
                                meta:resourcekey="DropDownList_MZ_SMResource1">
                                <asp:ListItem meta:resourcekey="ListItemResource3" Value="1">未婚</asp:ListItem>
                                <asp:ListItem meta:resourcekey="ListItemResource4" Value="2">已婚</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style3">
                            備&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 註
                        </td>
                        <td class="style117">
                            <asp:TextBox ID="TextBox_MZ_MEMO2" runat="server" Width="200px" MaxLength="20" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="style3">
                            離開本局日期
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ODDMY" runat="server" Width="65px" OnTextChanged="TextBox_MZ_ODDMY_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox_MZ_ODDMY_CalendarExtender" runat="server" Enabled="True"
                                Format="yyyy/MM/dd" OnClientDateSelectionChanged="toLocalDate" PopupButtonID="ibt_ODDMY"
                                PopupPosition="BottomRight" TargetControlID="TextBox_MZ_ODDMY">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="ibt_ODDMY" runat="server" ImageUrl="~/1-personnel/images/Calendar_scheduleHS.png"
                                TabIndex="-1" />
                        </td>
                        <td class="style3">
                            離開本局原因
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_MEMO1" runat="server" Width="150px" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style116">
                            列管別
                        </td>
                        <td class="style9">
                            <asp:DropDownList ID="DropDownList_MZ_CONTENT" runat="server">
                                <asp:ListItem Value="01">教育輔導</asp:ListItem>
                                <asp:ListItem Value="02">風紀評估</asp:ListItem>
                                <asp:ListItem Value="03">違法案件</asp:ListItem>
                                <asp:ListItem Value="04">違紀案件</asp:ListItem>
                                <asp:ListItem Value="06">關懷輔導</asp:ListItem>
                                <asp:ListItem Value="07">違紀傾向</asp:ListItem>
                                <asp:ListItem Value="05">其他不良案件</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style15">
                            列管日期
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" Width="65px" OnTextChanged="TextBox_MZ_IDATE_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox_MZ_IDATE_CalendarExtender" runat="server" Enabled="True"
                                Format="yyyy/MM/dd" OnClientDateSelectionChanged="toLocalDate" PopupButtonID="ibt_IDATE"
                                PopupPosition="BottomRight" TargetControlID="TextBox_MZ_IDATE">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="ibt_IDATE" runat="server" ImageUrl="~/1-personnel/images/Calendar_scheduleHS.png"
                                TabIndex="-1" />
                        </td>
                    </tr>
                </table>
                <div style="text-align: center">
                    <cc1:DragPanelExtender ID="DragPanelExtender1" runat="server" BehaviorID="BH4" DragHandleID="Panel_Print"
                        Enabled="True" TargetControlID="Panel_Print">
                    </cc1:DragPanelExtender>
                    <asp:Panel ID="Panel_Print" runat="server" Visible="false" Style="z-index: 999999;">
                        <table border="1" width="400px" style="background-color: White;">
                            <tr style="background-color: blue; font-size: xx-large; color: White;">
                                <td colspan="2">
                                    列印條件
                                </td>
                            </tr>
                            <tr>
                                <td class="style123">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="列印空表" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_Print_ok" runat="server" Text="確定" OnClick="btn_Print_ok_Click" />
                                    <asp:Button ID="btn_Print_Leave" runat="server" Text="離開" OnClick="btn_Print_Leave_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style114">
                            查處（列管）機關
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="DropDownList_MZ_UNAD" runat="server" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnDataBound="DropDownList_MZ_UNAD_DataBound">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@92'" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="style114">
                            原因及概要
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PRCT" runat="server" Height="50px" TextMode="MultiLine"
                                Width="673px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style118">
                            處理情形
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_RESULT" runat="server" Width="650px" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="bt_RESULT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="bt_EXPLAIN1_Click" TabIndex="-1" Text="V" />
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style119">
                            撤管日期
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ODATE" runat="server" Width="65px"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox_MZ_ODATE_CalendarExtender" runat="server" Enabled="True"
                                Format="yyyy/MM/dd" OnClientDateSelectionChanged="toLocalDate" TargetControlID="TextBox_MZ_ODATE"
                                PopupButtonID="ibt_ODATE">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="ibt_ODATE" runat="server" ImageUrl="~/1-personnel/images/Calendar_scheduleHS.png"
                                TabIndex="-1" />
                        </td>
                        <td class="style15">
                            撤管單位
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="DropDownList_MZ_OUNAD" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                OnDataBound="DropDownList_MZ_OUNAD_DataBound">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server">
                    <table style="background-color: #CCFFFF; color: White; width: 100%;">
                        <tr>
                            <td>
                                <asp:Button ID="btSearch" runat="server" Font-Bold="True" ForeColor="#000099" Text="查詢"
                                    OnClick="btSearch_Click" CausesValidation="False" />
                                <asp:Button ID="btInsert" runat="server" Font-Bold="True" ForeColor="#000099" Text="新增"
                                    OnClick="btInsert_Click" CausesValidation="False" AccessKey="a" />
                                <asp:Button ID="btUpdate" runat="server" Font-Bold="True" ForeColor="#000099" Text="修改"
                                    OnClick="btUpdate_Click" CausesValidation="False" Enabled="False" />
                                <asp:Button ID="btok" runat="server" Font-Bold="True" ForeColor="#000099" OnClick="btok_Click"
                                    Text="確認" Enabled="False" AccessKey="s" />
                                <asp:Button ID="btCancel" Font-Bold="True" ForeColor="#000099" runat="server" CausesValidation="False"
                                    meta:resourcekey="btCancelResource1" OnClick="btCancel_Click" Text="取消" Enabled="False" />
                                <asp:Button ID="btDelete" Font-Bold="True" ForeColor="#000099" runat="server" Text="刪除"
                                    CssClass=" " OnClick="btDelete_Click" CausesValidation="False" Enabled="False"
                                    AccessKey="d" />
                                <asp:Button ID="btUpper" runat="server" Font-Bold="True" ForeColor="#000099" CausesValidation="False"
                                    Enabled="False" meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" />
                                <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                                <asp:Button ID="btNEXT" runat="server" Font-Bold="True" ForeColor="#000099" CausesValidation="False"
                                    Enabled="False" meta:resourcekey="btNEXTResource1" OnClick="btNEXT_Click" Text="下一筆" />
                                <asp:Button ID="bt_UPDATEDLBASE" runat="server" Text="更新基本資料" OnClick="bt_UPDATEDLBASE_Click"
                                    CausesValidation="False" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="列印考核表" CausesValidation="False"
                                    Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
