<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal2-6.aspx.cs" Inherits="TPPDDB._1_personnel.Personal2_6" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1
        {
            text-align: left;
        }
        .style5
        {
            width: 70px;
            text-align: left;
        }
        .style8
        {
            width: 414px;
        }
        .style10
        {
        }
        .style110
        {
        }
        .style111
        {
            width: 110px;
            text-align: left;
        }
        .style112
        {
            width: 193px;
            text-align: left;
        }
        .style114
        {
            height: 45px;
        }
        .style116
        {
            height: 32px;
        }
        .style117
        {
            text-align: left;
        }
        .style118
        {
            text-align: center;
            width: 69px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
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
            <asp:Panel ID="Panel_PRK3" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%; font-family: 標楷體;
                    font-size: large;">
                    <tr>
                        <td class="style8" style="text-align: left">
                            警察人員獎懲案件請示單
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            案 號
                        </td>
                        <td class="style111">
                            <asp:TextBox ID="TextBox_MZ_NO" runat="server" Width="100px" CssClass="style10" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            受文者
                        </td>
                        <td class="style112">
                            <asp:DropDownList ID="DropDownList_MZ_USER" runat="server" CssClass="style10" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_NAME" DataValueField="MZ_AD">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KTYPE='04') AS MZ_NAME,MZ_AD FROM A_CHKAD" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                        <td class="style118">
                            單位主官（管）職稱姓名
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_MASTER_POS" runat="server" Width="70px" CssClass="style10"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="style117">
                            <asp:TextBox ID="TextBox_MZ_MASTER_NAME" runat="server" Width="100px" CssClass="style10"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            發文日期
                        </td>
                        <td class="style111">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="65px" CssClass="style10"
                                OnTextChanged="TextBox_MZ_DATE_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox_MZ_DATE_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="TextBox_MZ_DATE" PopupButtonID="ibt_SLFDATE" Format="yyyy/MM/dd"
                                OnClientDateSelectionChanged="toLocalDate">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="ibt_SLFDATE" runat="server" ImageUrl="~/1-personnel/images/Calendar_scheduleHS.png"
                                TabIndex="-1" />
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            陳報機關
                        </td>
                        <td class="style112">
                            <asp:DropDownList ID="DropDownList_MZ_OAD" runat="server" CssClass="style10" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_NAME" DataValueField="MZ_AD" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td class="style118">
                            發文字號
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_PRID" runat="server" Width="70px" CssClass="style10"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="style117">
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" Width="100px" CssClass="style10"
                                MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                    <div class="style12">
                        <table  style="background-color: #6699FF; color: White; width: 100%">
                            <tr bgcolor="#CCFFFF">
                                <td class="style34">
                                    <asp:Button ID="btSearch" runat="server" CssClass="KEY_IN_BUTTON_BLUE" meta:resourcekey="btP37_DLBASETableResource1"
                                        OnClick="btSearch_Click" Text="查詢" CausesValidation="False" />
                                    <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                        Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" 
                                        AccessKey="a" />
                                    <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                        Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                                    <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                        Text="確定" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                                    <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                        OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" 
                                        Enabled="False" />
                                    <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                        Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                        Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                                    <asp:Button ID="btPRINT" runat="server" Text="請示單列印" CausesValidation="False" 
                                        OnClick="btPRINT_Click" CssClass="KEY_IN_BUTTON_RED" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            事由
                            <td>
                                <asp:TextBox ID="TextBox_MZ_CAUSE1" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_CAUSE1_TextChanged"
                                    Width="690px"></asp:TextBox>
                                <asp:Button ID="btCAUSE" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btCAUSE_Click" TabIndex="-1" Text="V" />
                            </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;<td>
                                <asp:TextBox ID="TextBox_MZ_CAUSE" runat="server" CssClass="style10" Height="50px"
                                    TextMode="MultiLine" Width="716px"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            案 件 概 要
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_DESC1" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_DESC1_TextChanged"
                                Width="690px"></asp:TextBox>
                            <asp:Button ID="btDESC" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btDESC_Click" TabIndex="-1" Text="V" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_DESC" runat="server" CssClass="style10" Height="98px"
                                TextMode="MultiLine" Width="716px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            會 辦 &nbsp;
                        </td>
                        <td class="style114">
                            <asp:TextBox ID="TextBox_MZ_MAX" runat="server" CssClass="style10" Height="40px"
                                TextMode="MultiLine" Width="716px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            擬 辦 &nbsp;
                        </td>
                        <td class="style116">
                            <asp:TextBox ID="TextBox_MZ_PRE" runat="server" CssClass="style10" Height="40px"
                                TextMode="MultiLine" Width="716px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
