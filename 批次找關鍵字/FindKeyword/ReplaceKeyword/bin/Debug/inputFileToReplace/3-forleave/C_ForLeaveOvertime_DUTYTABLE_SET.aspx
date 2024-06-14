<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYTABLE_SET.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYTABLE_SET" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style58
        {
            text-align: left;
            width: 55px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style111
        {
            border: solid 0px;
        }
        .style112
        {
            text-align: left;
            width: 67px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style113
        {
            text-align: left;
            height: 20px;
            width: 321px;
        }
        .style116
        {
            text-align: left;
            height: 20px;
            width: 163px;
        }
        .style117
        {
            text-align: left;
            width: 65px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style10">
                <tr>
                    <td class="style8">
                        <asp:Label ID="Label27" runat="server"></asp:Label>
                    </td>
                    <td class="style4">
                        <asp:Label ID="Label28" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server">
                <table class="style6" border="1">
                    <tr>
                        <td class="style58">
                            勤務日期
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTYDATE" runat="server" Height="20px" MaxLength="9" Width="80px"
                                AutoPostBack="True" OnTextChanged="TextBox_DUTYDATE_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style7">
                            每日起班時間
                        </td>
                        <td class="style3">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="4" AutoPostBack="True"
                                OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="style58">
                            通訊代號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_COMM_CHANNEL" runat="server" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style58">
                            勤務項目
                        </td>
                        <td class="style116">
                            <asp:TextBox ID="TextBox_DUTYNO" runat="server" Width="30px" AutoPostBack="True"
                                OnTextChanged="TextBox_DUTYMODE_TextChanged" MaxLength="4" Style="height: 19px;"></asp:TextBox>
                            <asp:Button ID="btDUTYMODE" runat="server" CausesValidation="False" OnClick="btDUTYMODE_Click"
                                Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_DUTYNO1" runat="server" class="style111" Width="75px"></asp:TextBox>
                        </td>
                        <td class="style117">
                            勤務說明
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTYMODE" runat="server" TabIndex="-1" Width="35px" 
                                AutoPostBack="True" ontextchanged="TextBox_DUTYMODE_TextChanged1"></asp:TextBox>
                            <asp:Button ID="btDUTYMODE_NO" runat="server" Text="V" OnClick="btDUTYMODE_NO_Click" />
                             <asp:TextBox ID="TextBox_DUTYMODE_NO_MEMO" runat="server" TabIndex="-1" Width="410px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style58">
                            巡邏路線
                        </td>
                        <td class="style113">
                            <asp:TextBox ID="TextBox_MZ_DUTYPATROL_NO" runat="server" Width="280px"></asp:TextBox>
                            <asp:Button ID="btDUTYPATROL_NO" runat="server" Text="V" OnClick="btDUTYPATROL_NO_Click" />
                        </td>
                        <td class="style112">
                            任務目標
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_DUTYTARGET_NO" runat="server" Width="280px"></asp:TextBox>
                            <asp:Button ID="btDUTYTARGET_NO" runat="server" Text="V" OnClick="btDUTYTARGET_NO_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Visible="False">
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            時段
                        </td>
                        <td class="style7">
                            註記
                        </td>
                        <td class="style7">
                            勤區編號
                        </td>
                        <td class="style7">
                            輪值番號
                        </td>
                        <td class="style7">
                            時段
                        </td>
                        <td class="style7">
                            註記
                        </td>
                        <td class="style7">
                            勤區編號
                        </td>
                        <td class="style7">
                            輪值番號
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck1" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO1" runat="server" OnTextChanged="TextBox_MZ_PNO1_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO1" runat="server" OnTextChanged="TextBox_MZ_CNO1_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck2" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO2" runat="server" CssClass="style20" OnTextChanged="TextBox_MZ_PNO2_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO2" runat="server" OnTextChanged="TextBox_MZ_CNO2_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label3" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck3" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO3" runat="server" OnTextChanged="TextBox_MZ_PNO3_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO3" runat="server" OnTextChanged="TextBox_MZ_CNO3_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label4" runat="server" CssClass="style10"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck4" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO4" runat="server" OnTextChanged="TextBox_MZ_PNO4_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO4" runat="server" OnTextChanged="TextBox_MZ_CNO4_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label5" runat="server"></asp:Label>
                        </td>
                       <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck5" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO5" runat="server" OnTextChanged="TextBox_MZ_PNO5_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO5" runat="server" CssClass="style26" OnTextChanged="TextBox_MZ_CNO5_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label6" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck6" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO6" runat="server" OnTextChanged="TextBox_MZ_PNO6_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO6" runat="server" OnTextChanged="TextBox_MZ_CNO6_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label7" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck7" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO7" runat="server" OnTextChanged="TextBox_MZ_PNO7_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO7" runat="server" OnTextChanged="TextBox_MZ_CNO7_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label8" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck8" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO8" runat="server" OnTextChanged="TextBox_MZ_PNO8_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO8" runat="server" OnTextChanged="TextBox_MZ_CNO8_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label9" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck9" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO9" runat="server" CssClass="style23" OnTextChanged="TextBox_MZ_PNO9_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO9" runat="server" OnTextChanged="TextBox_MZ_CNO9_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label10" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck10" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO10" runat="server" OnTextChanged="TextBox_MZ_PNO10_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO10" runat="server" OnTextChanged="TextBox_MZ_CNO10_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label11" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck11" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO11" runat="server" OnTextChanged="TextBox_MZ_PNO11_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO11" runat="server" OnTextChanged="TextBox_MZ_CNO11_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label12" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck12" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO12" runat="server" OnTextChanged="TextBox_MZ_PNO12_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO12" runat="server" OnTextChanged="TextBox_MZ_CNO12_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label13" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck13" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO13" runat="server" OnTextChanged="TextBox_MZ_PNO13_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO13" runat="server" OnTextChanged="TextBox_MZ_CNO13_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label14" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck14" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO14" runat="server" OnTextChanged="TextBox_MZ_PNO14_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO14" runat="server" OnTextChanged="TextBox_MZ_CNO14_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label15" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck15" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO15" runat="server" OnTextChanged="TextBox_MZ_PNO15_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO15" runat="server" OnTextChanged="TextBox_MZ_CNO15_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label16" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck16" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO16" runat="server" OnTextChanged="TextBox_MZ_PNO16_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO16" runat="server" OnTextChanged="TextBox_MZ_CNO16_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label17" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck17" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO17" runat="server" OnTextChanged="TextBox_MZ_PNO17_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO17" runat="server" OnTextChanged="TextBox_MZ_CNO17_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label18" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck18" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO18" runat="server" OnTextChanged="TextBox_MZ_PNO18_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO18" runat="server" OnTextChanged="TextBox_MZ_CNO18_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label19" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck19" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO19" runat="server" OnTextChanged="TextBox_MZ_PNO19_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO19" runat="server" CssClass="style24" OnTextChanged="TextBox_MZ_CNO19_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label20" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck20" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO20" runat="server" OnTextChanged="TextBox_MZ_PNO20_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO20" runat="server" OnTextChanged="TextBox_MZ_CNO20_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label21" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck21" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO21" runat="server" OnTextChanged="TextBox_MZ_PNO21_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO21" runat="server" OnTextChanged="TextBox_MZ_CNO21_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label22" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck22" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO22" runat="server" OnTextChanged="TextBox_MZ_PNO22_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO22" runat="server" OnTextChanged="TextBox_MZ_CNO22_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label23" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck23" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO23" runat="server" OnTextChanged="TextBox_MZ_PNO23_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO23" runat="server" OnTextChanged="TextBox_MZ_CNO23_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label24" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck24" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO24" runat="server" OnTextChanged="TextBox_MZ_PNO24_TextChanged"
                                AutoPostBack="True" Width="150px"> </asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO24" runat="server"  OnTextChanged="TextBox_MZ_CNO24_TextChanged"
                                AutoPostBack="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label25" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck25" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO25" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_PNO25_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO25" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_CNO25_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label26" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_DutyCheck26" runat="server" 
                                AutoPostBack="True" Width="40px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PNO26" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_PNO26_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CNO26" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_CNO26_TextChanged" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="btSearch" runat="server" Text="查詢" OnClick="btSearch_Click" CausesValidation="False"
                                class="style9" />
                            <asp:Button ID="btInsert" runat="server" Text="新增" OnClick="btInsert_Click" Enabled="False"
                                class="style9" />
                            <asp:Button ID="btUpdate" runat="server" CausesValidation="False" Enabled="False"
                                OnClick="btUpdate_Click" Text="修改" class="style9" />
                            <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" Enabled="False"
                                CausesValidation="False" class="style9" />
                            <asp:Button ID="Button1_CANCEL" runat="server" OnClick="Button1_CANCEL_Click" Text="取消"
                                class="style9" />
                            <asp:Button ID="btCancel" runat="server" OnClick="btCancel_Click" Text="重新輸入" Enabled="False"
                                class="style9" />
                            <asp:Button ID="btPREVIEW" runat="server" OnClick="btPREVIEW_Click" Text="勤務表預覽"
                                class="style9" />
                            <asp:Button ID="btCreate" runat="server" Text="檢核勤務表" OnClick="btCreate_Click" class="style9" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" class="style9" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" class="style9" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
