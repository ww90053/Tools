<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Efficiency2.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Efficiency2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
            margin-left: 0px;
        }
        .style2
        {
            text-align: left;
        }
        .style3
        {
            text-align: left;
        }
        .style7
        {
            text-align: left;
        }
        .style11
        {
            text-align: left;
            width: 100%;
        }
        .style34
        {
            text-align: right;
        }
        .style35
        {
            width: 220px;
        }
        .style37
        {
            text-align: left;
        }
        .style38
        {
            text-align: left;
        }
        .style40
        {
            text-align: left;
        }
        .style43
        {
            font-size: large;
            font-family: 標楷體;
            text-align: left;
            font-weight: bold;
        }
        .style44
        {
            text-align: right;
        }
        .style45
        {
            font-size: large;
            font-family: 標楷體;
        }
        .style111
        {
            border: solid 0px;
        }
        .style112
        {
            text-align: left;
            width: 314px;
        }
        .style110
        {
        }
        .style116
        {
            width: 139px;
        }
        .style117
        {
            text-align: left;
            width: 80px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style118
        {
            text-align: left;
            width: 80px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style119
        {
            text-align: left;
            width: 36px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style120
        {
            width: 35px;
        }
        .style121
        {
            text-align: left;
            width: 35px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style122
        {
            text-align: left;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style123
        {
            text-align: left;
            width: 54px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td class="style43">
                        單筆作業
                    </td>
                    <td class="style44">
                        <asp:Label ID="Label1" runat="server" CssClass="style45" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_REV_BASE" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            年 度
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="50px" Style="height: 19px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            身分證號
                        </td>
                        <td class="style116" style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" class="style1">
                    <tr>
                        <td class="style117">
                            編制機關
                        </td>
                        <td class="style112">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10" CssClass="style133"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            編制單位
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style117">
                            現服機關
                        </td>
                        <td class="style112">
                            <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXAD_TextChanged" MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btEXAD" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXAD_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            現服單位
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXUNIT_TextChanged" MaxLength="4" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btEXUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" class="style1">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 稱
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_OCCC_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btOCCC_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            職序列
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" Width="23px" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="80px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            職務編號
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_POSIND" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" class="style1">
                    <tr>
                        <td class="style117">
                            薪俸職等
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SRANK_TextChanged" MaxLength="3" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btSRANK" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSRANK_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style111" Width="180px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 系
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CHISI" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_CHISI_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btCHISI" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btCHISI_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_CHISI1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="180px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style117">
                            俸&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 階
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_SLVC" runat="server" AutoPostBack="True" MaxLength="3"
                                OnTextChanged="TextBox_MZ_SLVC_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btSLVC" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSLVC_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_SLVC1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            俸&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 點
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_SPT" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" class="style1">
                    <tr>
                        <td class="style118">
                            分數（考績）
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_NUM" runat="server" Width="60px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_NUM_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style119">
                            等 次
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_GRADE" runat="server" Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td class="style120">
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style118">
                            嘉 獎
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_P4001" runat="server" Width="60px"></asp:TextBox>
                            次
                        </td>
                        <td class="style119">
                            記 功
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_P4010" runat="server" Width="60px"></asp:TextBox>
                            次
                        </td>
                        <td class="style122">
                            記 大 功
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_P4100" runat="server" Width="60px" MaxLength="3"></asp:TextBox>
                            次
                        </td>
                        <td class="style120">
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style118">
                            申 誡
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_P5001" runat="server" Width="60px"></asp:TextBox>
                            次
                        </td>
                        <td class="style119">
                            記 過
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_P5010" runat="server" Width="60px"></asp:TextBox>
                            次
                        </td>
                        <td class="style122">
                            記 大 過
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_P5100" runat="server" Width="60px"></asp:TextBox>
                            次
                        </td>
                        <td class="style120">
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style118">
                            事 假
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE01" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                        <td class="style119">
                            病 假
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE02" runat="server" Width="60px" Height="19px"></asp:TextBox>
                            天
                        </td>
                        <td class="style122">
                            曠 職
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE18" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                        <td class="style121">
                            遲 到
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE19" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                    </tr>
                    <tr>
                        <td class="style118">
                            娩 假
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE08" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                        <td class="style119">
                            婚 假
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE04" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                        <td class="style122">
                            早 退
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE20" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                        <td class="style121">
                            喪 假
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_CODE05" runat="server" Width="60px"></asp:TextBox>
                            天
                        </td>
                    </tr>
                </table>
                <table border="1" class="style1">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            主（官）管別
                        </td>
                        <td class="style38">
                            <asp:TextBox ID="TextBox_MZ_PCHIEF" runat="server" AutoPostBack="True" MaxLength="1"
                                OnTextChanged="TextBox_MZ_PCHIEF_TextChanged" Width="20px">
                            </asp:TextBox><asp:Button ID="btPCHIEF" runat="server" CausesValidation="False" OnClick="btPCHIEF_Click"
                                TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_PCHIEF1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="80px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="style123">
                            是否參加
                        </td>
                        <td class="style40">
                            <asp:DropDownList ID="DropDownList_MZ_SWT" runat="server">
                                <asp:ListItem Value="0">參加</asp:ListItem>
                                <asp:ListItem Value="1">不參加</asp:ListItem>
                                <asp:ListItem Value="2">其他</asp:ListItem>
                                <asp:ListItem Value="3">另考</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table border="1" class="style11">
                    <tr>
                        <td class="style35">
                            <asp:Button ID="btCheck" runat="server" Text="統計獎懲、事假到備註" OnClick="btCheck_Click"
                                CssClass="KEY_IN_BUTTON_RED" />
                        </td>
                        <td class="style119">
                            備 註
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_MEMO" runat="server" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td class="style34">
                            <asp:Button ID="btSearch" runat="server" CssClass="KEY_IN_BUTTON_BLUE" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                        </td>
                        <td class="style37">
                            <asp:Button ID="btUpper" runat="server" meta:resourcekey="btUpperResource1" OnClick="btUpper_Click"
                                Text="上一筆" Enabled="False" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" meta:resourcekey="btNEXTResource1" OnClick="btNEXT_Click"
                                Text="下一筆" Enabled="False" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
