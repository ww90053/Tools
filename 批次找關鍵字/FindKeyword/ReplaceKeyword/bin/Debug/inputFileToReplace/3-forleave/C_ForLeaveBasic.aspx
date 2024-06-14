<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveBasic.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveBasic" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
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
        .style61
        {
            text-align: left;
            height: 20px;
            width: 131px;
        }
        .style62
        {
            text-align: left;
            height: 20px;
            width: 113px;
        }
        .style63
        {
            text-align: left;
            height: 20px;
            width: 149px;
        }
        .style68
        {
            color: #0033CC;
            font-weight: bold;
        }
        .style70
        {
            text-align: left;
            width: 55px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style71
        {
            text-align: left;
            width: 60px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style72
        {
            text-align: left;
            width: 70px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style73
        {
            text-align: left;
            width: 30px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style74
        {
            color: #FF0000;
            font-weight: bold;
            background-color: #FFFF99;
            text-align: left;
            width: 55px;
        }
        .style75
        {
            text-align: left;
            width: 30px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style77
        {
            text-align: left;
            height: 20px;
            width: 71px;
        }
        .style78
        {
            text-align: left;
            height: 20px;
            width: 79px;
        }
        .style79
        {
            text-align: left;
            height: 20px;
            width: 50px;
        }
        .style81
        {
            text-align: left;
            width: 54px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style82
        {
            text-align: left;
            width: 60px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style83
        {
            text-align: left;
            width: 75px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style84
        {
            text-align: center;
            width: 189px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style85
        {
            width: 70px;
        }
        .style86
        {
            text-align: left;
            height: 20px;
            width: 93px;
        }
        .style87
        {
            width: 260px;
            height: 65px;
        }
        .style159
        {
            text-align: left;
            width: 95px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .auto-style1 {
            color: #0033CC;
            font-weight: bold;
            height: 21px;
        }
        .auto-style2 {
            text-align: left;
            height: 21px;
            margin-left: 40px;
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
                        差勤基本資料
                    </td>
                    <td class="style4">
                        <asp:Label ID="Label1" runat="server" CssClass="style10"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel7" runat="server">
                <table class="style6" border="1">
                    <tr>
                        <td class="style74">
                            身分證號
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="style75">
                            姓 名
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style70">
                            編制機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_AD" runat="server" Font-Bold="True" ForeColor="#0033CC"></asp:Label>
                        </td>
                        <td class="style70">
                            現服機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_EXAD" runat="server" Font-Bold="True" ForeColor="#0033CC"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td>
                        <asp:Panel ID="Panel_Button" runat="server">
                            <table class="style12" border="1">
                                <tr>
                                    <td class="style9">
                                        <asp:Button ID="DLTB_ADD" runat="server" OnClick="DLTB_ADD_Click" Text="請假登錄" Height="21px"
                                            CausesValidation="False" Font-Bold="True" ForeColor="Maroon" />
                                    </td>
                                    <td class="style9">
                                        <asp:Button ID="DLTB_RD" runat="server" OnClick="DLTB_RD_Click" Text="請假紀錄" CausesValidation="False"
                                            Font-Bold="True" ForeColor="Maroon" />
                                    </td>
                                    <td class="style9">
                                        <asp:Button ID="DLTB_Clocking" runat="server" Text="刷卡紀錄" OnClick="DLTB_Clocking_Click"
                                            CausesValidation="False" Font-Bold="True" ForeColor="Maroon" />
                                    </td>
                                    <td class="style9">
                                        <asp:Button ID="DLTB_UNUSUAL" runat="server" OnClick="DLTB_UNUSUAL_Click" Text="勤惰紀錄"
                                            CausesValidation="False" Font-Bold="True" ForeColor="Maroon" />
                                    </td>
                                    <td class="style9">
                                        <asp:Button ID="DLTB_DAYS" runat="server" Text="個人差假天數" OnClick="DLTB_DAYS_Click"
                                            CausesValidation="False" Font-Bold="True" ForeColor="Maroon" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <asp:Panel ID="Panel2" runat="server" Height="330px">
                        <table class="style6" border="1">
                            <tr>
                                <td class="style71">
                                    請假假別
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_CODE" runat="server" Width="35px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_CODE_TextChanged" MaxLength="4"></asp:TextBox><asp:Button
                                            ID="btCODE" runat="server" CausesValidation="False" OnClick="btCODE_Click" Text="V"
                                            TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_CODE1" runat="server" CssClass="style2" Width="105px"
                                        TabIndex="-1"></asp:TextBox>
                                </td>
                                <td class="style71">
                                    補休日期
                                </td>
                                <td style="text-align: left">
                                    &nbsp;<asp:Button ID="btRESTDATECHECK" runat="server" Text="可補休日" CausesValidation="False"
                                        Enabled="False" OnClick="btRESTDATECHECK_Click" Font-Bold="True" ForeColor="Maroon" />
                                    <asp:Button ID="BT_GV1" runat="server" CausesValidation="False" OnClick="BT_GV1_Click"
                                        TabIndex="-1" Style="display: none;" />
                                </td>
                                <td class="style71">
                                    簽核方式
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RadioButtonList_SIGN_KIND" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Selected="True">紙本</asp:ListItem>
                                        <asp:ListItem Value="2">線上</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="CheckBox_MZ_SWT" runat="server" Text="是否申請補助" Enabled="False" />
                                </td>
                            </tr>
                        </table>


                        <table id="dayfacttable" runat="server" visible="false" class="style6" border="1">
                            <tr>                                
                                <td class="style68" id="ftype"  bgcolor="#FFFF66" style="width:50px" visible="false">類型 </td>

                                <td class="style3" id="ftype6"  visible="false">
                                    <asp:DropDownList ID="DropDownList_funeral_type6" runat="server" AutoPostBack="True" OnTextChanged ="DropDownList_funeral_type6Changed">
                                            <asp:ListItem Value="1">一般公假</asp:ListItem>
                                            <asp:ListItem Value="2">健康檢查公假</asp:ListItem>
                                            <asp:ListItem Value="3">公務受傷公假</asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                                <td class="style3" id="ftype8"  visible="false">
                                    <asp:DropDownList ID="DropDownList_funeral_type8" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="1">一般娩假</asp:ListItem>
                                            <asp:ListItem Value="2">預請娩假</asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                                <td class="style3" id="ftype5"  visible="false">
                                    <asp:DropDownList ID="DropDownList_funeral_type5" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="1">配偶</asp:ListItem>
                                            <asp:ListItem Value="2">子女</asp:ListItem>
                                            <asp:ListItem Value="3">兄弟姊妹</asp:ListItem>
                                            <asp:ListItem Value="4">父母</asp:ListItem>
                                            <asp:ListItem Value="5">養父母</asp:ListItem>
                                            <asp:ListItem Value="6">繼父母</asp:ListItem>
                                            <asp:ListItem Value="7">祖父母</asp:ListItem>
                                            <asp:ListItem Value="8">曾祖父母</asp:ListItem>
                                            <asp:ListItem Value="9">配偶之父母</asp:ListItem>
                                            <asp:ListItem Value="10">配偶之養父母</asp:ListItem>
                                            <asp:ListItem Value="11">配偶之繼父母</asp:ListItem>
                                            <asp:ListItem Value="12">配偶之祖父母</asp:ListItem>
                                            <asp:ListItem Value="13">配偶之曾祖父母</asp:ListItem>
                                    </asp:DropDownList>
                                </td>


                                <td class="style68" id="qfact"  bgcolor="#FFFF66" style="width:100px" visible="false">事實發生日</td>
                                <td class="style3" id="qfact1" visible="false">
                                    <asp:TextBox ID="TextBox_dayfact" runat="server"  
                                        Width="65px" MaxLength="7"></asp:TextBox>
                                    <cc2:FilteredTextBoxExtender ID="TextBox_dayfact_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_dayfact"></cc2:FilteredTextBoxExtender>
                                    日
                                </td>
                            </tr>
                        </table>


                        <table class="style6" border="1">
                            <tr>
                                <td class="style71">
                                    請假日期
                                </td>
                                <td class="style62">
                                    <asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" Width="65px" 
                                        MaxLength="7"></asp:TextBox>
                                    <cc2:FilteredTextBoxExtender ID="TextBox_MZ_IDATE1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_IDATE1"></cc2:FilteredTextBoxExtender>
                                    日
                                </td>
                                <td class="style63">
                                    <asp:TextBox ID="TextBox_MZ_ITIME1" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                                    <cc2:FilteredTextBoxExtender ID="TextBox_MZ_ITIME1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_ITIME1"></cc2:FilteredTextBoxExtender>
                                    分 (例：0830)
                                </td>
                                <td class="style3">
                                    至
                                </td>
                                <td class="style86">
                                    <asp:TextBox ID="TextBox_MZ_ODATE" runat="server" Width="65px" 
                                        MaxLength="7"></asp:TextBox>
                                    <cc2:FilteredTextBoxExtender ID="TextBox_MZ_ODATE_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_ODATE"></cc2:FilteredTextBoxExtender>
                                    日
                                </td>
                                <td class="style61">
                                    <asp:TextBox ID="TextBox_MZ_OTIME" runat="server"  
                                        Width="50px" MaxLength="4"></asp:TextBox>
                                    <cc2:FilteredTextBoxExtender ID="TextBox_MZ_OTIME_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_OTIME"></cc2:FilteredTextBoxExtender>
                                    分(例：1730)
                                </td>
                                <td class="style3">
                                    共計
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_TDAY" runat="server" Width="40px" MaxLength="3"></asp:TextBox>日
                                    <cc2:FilteredTextBoxExtender ID="TextBox_MZ_TDAY_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_TDAY"></cc2:FilteredTextBoxExtender>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_TTIME" runat="server" Width="30px" MaxLength="1"></asp:TextBox>時
                                    <cc2:FilteredTextBoxExtender ID="TextBox_MZ_TTIME_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_TTIME"></cc2:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style72">
                                    職務代理人
                                </td>
                                <td class="style3">
                                    <asp:DropDownList ID="DropDownList_MZ_RNAME" DataTextField="MZ_NAME" DataValueField="MZ_ID"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_MZ_RNAME_SelectedIndexChanged">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="style72">
                                    代理人職稱
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_ROCCC" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
                                </td>
                                <td class="style73">
                                    附 件
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_MEMO" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td bgcolor="#FFFF66">
                                    <span class="style68">出差（旅遊）地點</span>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_TADD" runat="server" Width="250px"></asp:TextBox><span>(赴大陸相關地區！請詳細填寫地點！)</span>
                                </td>
                                <td bgcolor="#FFFF66">
                                    <asp:CheckBox ID="CheckBox_MZ_FOREIGN" runat="server" AutoPostBack="True" 
                                        Text="出國" oncheckedchanged="CheckBox_MZ_FOREIGN_CheckedChanged" />
                                    <asp:CheckBox ID="CheckBox_MZ_CHINA" runat="server" AutoPostBack="True" Text="大陸地區"
                                        OnCheckedChanged="CheckBox_MZ_CHINA_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <table id="AB_DAY" class="style6" border="1" runat="server" visible="false">
                            <tr>
                                <td class="style68" bgcolor="#FFFF66">實際出國天數</td>
                                <td class="style62">
                                    <asp:TextBox ID="TextBox_AB_IDATE" runat="server" Width="65px" 
                                        MaxLength="7"></asp:TextBox>
                                    <cc2:FilteredTextBoxExtender ID="TextBox_AB_IDATE_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_AB_IDATE"></cc2:FilteredTextBoxExtender>
                                    日
                                </td>
                                <td class="style63">
                                    <asp:TextBox ID="TextBox_AB_ITIME" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                                    分 (例：0830)
                                </td>
                                <td class="style3">
                                    至
                                </td>
                                <td class="style86">
                                    <asp:TextBox ID="TextBox_AB_ODATE" runat="server" Width="65px" 
                                        MaxLength="7"></asp:TextBox>
                                    日
                                </td>
                                <td class="style61">
                                    <asp:TextBox ID="TextBox_AB_OTIME" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                                    分(例：1730)
                                </td>
                                <td class="style3">
                                    共計
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_AB_TDAY" runat="server" Width="40px" MaxLength="3"></asp:TextBox>日
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_AB_TTIME" runat="server" Width="30px" MaxLength="1"></asp:TextBox>時
                                </td>
                            </tr>
                        </table>
                        <table id="tableLastYearJobLocation" runat="server" visible="false" class="style6" border="1">
                            <tr>
                                <td class="auto-style1"  bgcolor="#FFFF66">最近一年工作職掌</td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="TextBox_MZ_LastYearJobLocation" runat="server" Width="450px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        
                        <table class="style6" border="1">
                            <tr>
                            
                                <td class="style68"  bgcolor="#FFFF66">
                                    請假事由
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_CAUSE" runat="server" Width="450px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style71">
                                    上傳附件1
                                </td>
                                <td class="style3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="150px" Enabled="false" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                                    <asp:HyperLink ID="HyperLink_FILENAME1" runat="server" Text="查看附件" Visible="false"
                                        Width="80px" Target="_blank"></asp:HyperLink>
                                    <asp:Button ID="Button_DelFILE1" runat="server" Text="刪除" Visible="false" CausesValidation="false"
                                        OnCommand="Button_DelFILE1_Command" Width="40px" />
                                </td>
                                <td class="style71">
                                    上傳附件2
                                </td>
                                <td class="style3">
                                    <asp:FileUpload ID="FileUpload2" runat="server" Width="150px" Enabled="false" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                                    <asp:HyperLink ID="HyperLink_FILENAME2" runat="server" Text="查看附件" Visible="false"
                                        Width="80px" Target="_blank"></asp:HyperLink>
                                    <asp:Button ID="Button_DelFILE2" runat="server" Text="刪除" Visible="false" CausesValidation="false"
                                        Width="40px" OnCommand="Button_DelFILE2_Command" />
                                </td>
                                <td class="style71">
                                    上傳附件3
                                </td>
                                <td class="style3">
                                    <asp:FileUpload ID="FileUpload3" runat="server" Width="150px" Enabled="false" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                                    <asp:HyperLink ID="HyperLink_FILENAME3" runat="server" Text="查看附件" Visible="false"
                                        Width="80px" Target="_blank"></asp:HyperLink>
                                    <asp:Button ID="Button_DelFILE3" runat="server" Text="刪除" Visible="false" CausesValidation="false"
                                        Width="40px" OnCommand="Button_DelFILE3_Command" />
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style70">
                                    應休天數
                                </td>
                                <td class="style77">
                                    <asp:TextBox ID="TextBox_HDAY" runat="server" Width="35px"></asp:TextBox>
                                </td>
                                <td class="style70">
                                    應休時數
                                </td>
                                <td class="style78">
                                    <asp:TextBox ID="TextBox_HTIME" runat="server" Width="35px"></asp:TextBox>
                                </td>
                                <td class="style70">
                                    已休天數
                                </td>
                                <td class="style79">
                                    <asp:TextBox ID="TextBox_USED_HDAY" runat="server" Width="35px"></asp:TextBox>
                                </td>
                                <td class="style70">
                                    已休時數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_USED_HTIME" runat="server" Width="35px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style70">
                                    填表日期
                                </td>
                                <td class="style77">
                                    <asp:TextBox ID="TextBox_MZ_SYSDAY" runat="server" Width="65px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_SYSDAY_TextChanged"></asp:TextBox>
                                </td>
                                <td class="style70">
                                    填表時間<br />
                                </td>
                                <td class="style78">
                                    <asp:TextBox ID="TextBox_MZ_SYSTIME" runat="server" Width="65px"></asp:TextBox>
                                </td>
                                <td class="style70">
                                    差假核定
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_CHK1" runat="server" Width="30px" ReadOnly="True" Enabled="False"></asp:TextBox>
                                </td>
                                <td class="style70">
                                    補助次數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox1" runat="server" ReadOnly="True" Width="80px"></asp:TextBox>
                                </td>
                                <td class="style81">
                                    補助金額
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel8" runat="server">
                            <table class="style12">
                                <tr>
                                    <td>
                                        <asp:Button ID="Button3" runat="server" Text="印請假單" OnClick="Button3_Click" CausesValidation="False"
                                            class="style13" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button5" runat="server" Text="印出差單" OnClick="Button5_Click" CausesValidation="False"
                                            class="style13" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button4" runat="server" Text="印請假報告單" OnClick="Button4_Click" CausesValidation="False"
                                            class="style13" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button6" runat="server" Text="印核定通知單"  CausesValidation="False"
                                            class="style13" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button7" runat="server" Text="印出國請假單" OnClick="Button7_Click" CausesValidation="False"
                                            class="style13" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button8" runat="server" Text="印非請假出國報備單" OnClick="Button8_Click"
                                            class="style13" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>                            
                        </asp:Panel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:Panel ID="Panel1" runat="server" Height="350px">
                        <cc1:TBGridView ID="GridView1" runat="server" CellPadding="4" EmptyDataText="查無資料"
                            EnableEmptyContentRender="True" GridLines="None" Width="100%" DataSourceID="SqlDataSource1"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="12" 
                            ForeColor="#333333" onrowdatabound="GridView1_RowDataBound"><RowStyle BackColor="#EFF3FB" /><Columns><asp:BoundField DataField="ITEM" HeaderText="筆次" SortExpression="ITEM" /><asp:BoundField DataField="MZ_CODE" HeaderText="假別" SortExpression="MZ_CODE" /><asp:BoundField DataField="MZ_CAUSE" HeaderText="請假事由" SortExpression="MZ_CAUSE" /><asp:BoundField DataField="MZ_TADD" HeaderText="旅遊地點" SortExpression="MZ_TADD" /><asp:BoundField DataField="MZ_RNAME" HeaderText="代理人" SortExpression="MZ_RNAME" /><asp:BoundField DataField="MZ_IDATE1" HeaderText="請假日起" SortExpression="MZ_IDATE1" /><asp:BoundField DataField="MZ_ITIME1" HeaderText="時分" SortExpression="MZ_ITIME1" /><asp:BoundField DataField="MZ_ODATE" HeaderText="請假日迄" SortExpression="MZ_ODATE" /><asp:BoundField DataField="MZ_OTIME" HeaderText="時分" SortExpression="MZ_OTIME" /><asp:BoundField DataField="MZ_TDAY" HeaderText="天數" SortExpression="MZ_TDAY" /><asp:BoundField DataField="MZ_TTIME" HeaderText="時數" SortExpression="MZ_TTIME" /></Columns><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><EditRowStyle BackColor="#2461BF" /><AlternatingRowStyle BackColor="White" /></cc1:TBGridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT (ROW_NUMBER() OVER(ORDER BY MZ_IDATE1)) AS ITEM,MZ_CODE_CH AS MZ_CODE,MZ_CAUSE,MZ_TADD, MZ_RNAME,CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE1,1,3)))+'/'+dbo.SUBSTR(MZ_IDATE1,4,2)+'/'+dbo.SUBSTR(MZ_IDATE1,6,2) AS MZ_IDATE1,MZ_ITIME1,CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_ODATE,1,3)))+'/'+dbo.SUBSTR(MZ_ODATE,4,2)+'/'+dbo.SUBSTR(MZ_ODATE,6,2) AS MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME FROM VW_C_DLTB01 WHERE MZ_ID=@MZ_ID AND dbo.to_number(dbo.SUBSTR(MZ_IDATE1,1,3))=EXTRACT(YEAR FROM GETDATE())-1911 AND (MZ_TDAY>0 OR MZ_TTIME>0) ORDER BY MZ_IDATE1 DESC " >
                            <SelectParameters>
                                <asp:ControlParameter ControlID="TextBox_MZ_ID" Name="MZ_ID" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <asp:Panel ID="Panel4" runat="server" Height="350px">
                        <cc1:TBGridView ID="TBGridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            EnableEmptyContentRender="True" GridLines="None" Width="100%" OnPageIndexChanging="TBGridView1_PageIndexChanging"
                            ForeColor="#333333"><RowStyle BackColor="#EFF3FB" /><Columns><asp:BoundField DataField="TERMINALNAME" HeaderText="卡機編號" SortExpression="TERMINALNAME" /><asp:BoundField DataField="USERID" HeaderText="編號" SortExpression="USERID" /><asp:BoundField DataField="USERNAME" HeaderText="姓名" SortExpression="USERNAME" /><asp:BoundField DataField="LOGDATE" HeaderText="刷卡日期" SortExpression="LOGDATE" /><asp:BoundField DataField="LOGTIME" HeaderText="刷卡時間" SortExpression="LOGTIME" /><asp:BoundField DataField="VERIFY" HeaderText="刷卡狀態" SortExpression="FKEY" /><asp:BoundField DataField="FKEY" HeaderText="加班" SortExpression="FKEY" /><asp:BoundField DataField="DOOR" HeaderText="進出門" SortExpression="FKEY" /></Columns><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><EditRowStyle BackColor="#2461BF" /><AlternatingRowStyle BackColor="White" /></cc1:TBGridView>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View4" runat="server">
                    <asp:Panel ID="Panel3" runat="server" Height="350px">
                        <asp:GridView ID="GridView_DLTB_UNUSUAL" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" EmptyDataText="查無資料" GridLines="None" Width="100%" OnPageIndexChanging="GridView_DLTB_UNUSUAL_PageIndexChanging"
                            ForeColor="#333333">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:BoundField DataField="LOGDATE" HeaderText="日期" />
                                <asp:BoundField DataField="INTIME" HeaderText="上班刷卡" />
                                <asp:BoundField DataField="OUTTIME" HeaderText="下班刷卡" />
                                <asp:BoundField DataField="CODE" HeaderText="假別" />
                                <asp:BoundField DataField="KIND" HeaderText="遲到早退" />
                                <asp:BoundField DataField="MEMO" HeaderText="狀況" />
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
                <asp:View ID="View5" runat="server">
                    <asp:Panel ID="Panel5" runat="server" Height="350px">
                        <table class="style6" border="1">
                            <tr>
                                <td class="style82">
                                    假別年度
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="50px"></asp:TextBox>
                                </td>
                                <td class="style7">
                                    初任公職日
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_FDATE" runat="server" Width="65px"></asp:TextBox>
                                </td>
                                <td class="style159">
                                    實務訓練期滿日
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_QUA_DATE" runat="server" Width="65px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style82">
                                    服務年資
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_OFFYY" runat="server" Width="50px"></asp:TextBox>
                                </td>
                                <td class="style82">
                                    併計年資
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_TYEAR" runat="server" Width="50px"></asp:TextBox>
                                </td>
                                <td class="style72">
                                    併計年資月
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_TMONTH" runat="server" Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style82">
                                    併計文件
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_TMEMO" runat="server" Width="500px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel_SDAY" runat="server">
                            <table class="style6" border="1">
                                <tr>
                                    <td class="style84">
                                        休假（慰勞假）今年年保留天數
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY3" runat="server" Width="100px"></asp:TextBox>天
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY3_HOUR" runat="server" Width="100px"></asp:TextBox>時
                                    </td>
                                    <td class="style85">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style84">
                                        休假（慰勞假）第一年保留天數
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY" runat="server" Width="100px"></asp:TextBox>天
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY_HOUR" runat="server" Width="100px"></asp:TextBox>時
                                    </td>
                                    <td class="style72">
                                        剩餘天數
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY_CANUSE" runat="server" Width="100px"></asp:TextBox>天
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY_HOUR_CANUSE" runat="server" Width="100px"></asp:TextBox>時
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style84">
                                        休假（慰勞假）第二年保留天數
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY2" runat="server" Width="100px"></asp:TextBox>天
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY2_HOUR" runat="server" Width="100px"></asp:TextBox>時
                                    </td>
                                    <td class="style72">
                                        剩餘天數
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY2_CANUSE" runat="server" Width="100px"></asp:TextBox>天
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_SDAY2_HOUR_CANUSE" runat="server" Width="100px"></asp:TextBox>時
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style14">
                                    休假（慰勞假）今年天數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_HDAY" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_HTIME" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                                <td class="style7">
                                    剩餘天數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_HDAY_CANUSE" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_HTIME_CANUSE" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                            </tr>
                        </table>
                        <table class="style6" border="1">
                            <tr>
                                <td class="style83">
                                    事假總天數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_PDAY" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_PHOUR" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                                <td class="style7">
                                    事假剩餘天數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_PDAY_CANUSE" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_PDAY_HOUR_CANUSE" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                            </tr>
                            <tr>
                                <td class="style83">
                                    病假總天數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_SICKDAY" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_SICKHOUR" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                                <td class="style7">
                                    病假剩餘天數
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_SICKDAY_CANUSE" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_SICKDAY_HOUR_CANUSE" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                            </tr>
                            <tr>
                                <td class="style83">
                                    家庭照顧假
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_HCAREDAY" runat="server" Width="100px"></asp:TextBox>天
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="TextBox_MZ_HCAREHOUR" runat="server" Width="100px"></asp:TextBox>時
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <asp:Panel ID="Panel6" runat="server">
                <table class="style12">
                    <tr>
                        <td>
                            <asp:Button ID="btSearch" runat="server" Enabled="False" Text="查詢" OnClick="btSearch_Click"
                                CausesValidation="False" class="style9" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CausesValidation="False" class="style9" AccessKey="a" Height="21px" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" class="style9" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" class="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Enabled="False" class="style9" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;)"
                                Enabled="False" class="style9" AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" meta:resourcekey="btUpperResource1"
                                OnClick="btUpper_Click" Text="上一筆" Enabled="False" class="style9" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label><asp:Button
                                ID="btNEXT" runat="server" CausesValidation="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" Enabled="False" class="style9" />
                            <asp:Button ID="btOK2" runat="server" Text="確定修改 不重跑線上流程" class="style9" OnClick="btOK_Click"
                                Visible="False" Enabled="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btOK" />
            <asp:PostBackTrigger ControlID="btOK2" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_select" runat="server" Style="display: none;">
                <div style="border: solid 1px gray; background-color: LightBlue; width: 280px; height: 290px;">
                    <div style="margin: 10px; background-color: #FFFFFF; width: 260px; height: 270px;"
                        class="style87">
                        <div style="margin: 10px;">
                            <h3>
                                陳核
                            </h3>
                            <asp:GridView ID="GV_CHECKER" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GV_CHECKER_PageIndexChanging"
                                OnRowCommand="GV_CHECKER_RowCommand" PageSize="5">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_select" runat="server" CausesValidation="false" CommandName="checker"
                                                Text="選取" CommandArgument='<%# Eval("SN") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="100px" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <div style="text-align: center;">
                                <asp:Button ID="btn_check" runat="server" Text="送出" OnClick="btn_check_Click" Style="display: none;" />
                                <asp:Button ID="btn_exit" runat="server" Text="取消" OnClick="btn_exit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button ID="btn_popup" runat="server" Text="Button" Style="display: none;" />
            <cc2:ModalPopupExtender ID="Panel_select_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                DynamicServicePath="" Enabled="True" TargetControlID="btn_popup" PopupControlID="Panel_select">
            </cc2:ModalPopupExtender>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
