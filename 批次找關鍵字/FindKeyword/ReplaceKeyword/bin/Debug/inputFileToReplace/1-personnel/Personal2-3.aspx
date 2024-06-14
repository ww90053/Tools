<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal2-3.aspx.cs" Inherits="TPPDDB._1_personnel.Personal2_3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style3
        {
            text-align: left;
        }
        .style9
        {
            margin-bottom: 0px;
        }
        .style11
        {
            text-align: left;
        }
        .style15
        {
            text-align: left;
        }
        .style21
        {
            font-size: large;
            font-family: 標楷體;
        }
        .style22
        {
            text-align: right;
        }
        .style23
        {
            font-size: large;
            font-family: 標楷體;
            text-align: left;
            width: 407px;
        }
        .style110
        {
            width: 23px;
        }
        .style111
        {
            border: solid 0px;
            color: #0033CC;
            font-weight: bold;
        }
        .style112
        {
            text-align: left;
            width: 313px;
        }
        .style113
        {
            text-align: left;
            width: 233px;
        }
    </style>
    <%--  <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) 
        {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }

    </script>
--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="1" style="background-color: #6699FF; color: White; width: 100%;">
                <tr>
                    <td class="style23">
                        獎懲令修改，審核，統計
                    </td>
                    <td class="style22">
                        <asp:Button ID="btDetail" runat="server" Font-Bold="True" ForeColor="Maroon" OnClick="btDetail_Click"
                            Text="明細" Visible="False" />
                        <asp:Label ID="Label1" runat="server" CssClass="style21"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server">
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            身分證號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="True" MaxLength="10"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            姓 名
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" AutoPostBack="True" MaxLength="12"
                                OnTextChanged="TextBox_MZ_NAME_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            生效日期
                        </td>
                        <td class="style113">
                            <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_IDATE_TextChanged"
                                MaxLength="9" Width="80px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            發文日期
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" AutoPostBack="True" MaxLength="9"
                                OnTextChanged="TextBox_MZ_DATE_TextChanged" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            發文文號
                        </td>
                        <td class="style113">
                            <asp:TextBox ID="TextBox_MZ_PRID" runat="server" AutoPostBack="True" MaxLength="24"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            字第
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            核定機關
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_CHKAD" runat="server" AutoPostBack="True" MaxLength="10"
                                Width="75px" Enabled="False"></asp:TextBox>
                            <asp:Button ID="btCHKAD" runat="server" CausesValidation="False" CssClass="style110"
                                TabIndex="-1" Text="V" Enabled="False" />
                            <asp:TextBox ID="TextBox_MZ_CHKAD1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="240px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            編制機關
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" AutoPostBack="True" MaxLength="10"
                                OnTextChanged="TextBox_MZ_AD_TextChanged" Width="75px"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btAD_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="240px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            編制單位
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            職 序
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            職 稱
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_OCCC_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btOCCC_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            官職等起
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_RANK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_RANK_TextChanged" CssClass="style112" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btRANK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btRANK_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_RANK_1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            官職等迄
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_RANK1_TextChanged" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btRANK1" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btRANK1_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_RANK1_1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            獎懲類別
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PRK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PRK_TextChanged" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btPRK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRK_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRK_1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            薪資職等
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SRANK_TextChanged" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btSRANK" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSRANK_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            獎懲內容
                        </td>
                        <td class="style3" colspan="3">
                            <asp:TextBox ID="TextBox_MZ_PRCT" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                            <asp:Button ID="btPRCT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btPRCT_Click" TabIndex="-1" Text="V" />
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            獎懲結果
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PRRST" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PRRST_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btPRRST" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRRST_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRRST1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            獎懲統計分類
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PRK1" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PRK1_TextChanged" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btPRK1" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRK1_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRK1_1" runat="server" CssClass="style111" Width="150px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            獎懲依據
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PROLNO" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PROLNO_TextChanged" MaxLength="5"></asp:TextBox>
                            <asp:Button ID="btPROLNO" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPROLNO_Click" />
                            <asp:TextBox ID="TextBox_MZ_PROLNO1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            加重條款
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PROLNO2" runat="server" MaxLength="40" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            備 註
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_MEMO" runat="server" Width="350px" MaxLength="25"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            附 件
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_REMARK" runat="server" Width="200px" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE">
                            已上傳警政署
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="TextBox_MZ_SWT" runat="server" Width="100px"></asp:TextBox>(Y/N)
                            <cc1:FilteredTextBoxExtender ID="Filtered_TextBox_MZ_SWT" runat="server"
                                TargetControlID="TextBox_MZ_SWT" ValidChars="YN" />
                        </td>
                        <td class="KEY_IN_TITLE">
                            已發布令
                        </td>
                        <td class="KEY_IN_BUTTON_BLUE">
                            <asp:TextBox ID="TextBox_MZ_SWT2" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            密 碼
                        </td>
                        <td class="style15">
                            <asp:TextBox ID="TextBox_MZ_PRPASS" runat="server" CssClass="style9" Width="100px"
                                TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr bgcolor="#CCFFFF">
                        <td>
                            <asp:Button ID="btSearch" runat="server" Text="查詢" OnClick="btSearch_Click" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" CausesValidation="False"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="確定" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" Enabled="False"
                                CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" OnClientClick="return confirm(&quot;確定刪除？&quot;)" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btUpper" runat="server" Enabled="False" meta:resourcekey="btUpperResource1"
                                OnClick="btUpper_Click" Text="上一筆" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btSWT2" runat="server" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE"
                                Enabled="False" meta:resourcekey="btNEXTResource1" OnClick="btSWT2_Click" Text="發佈整批修改" />
                            <asp:Button ID="btEXCEL" runat="server" Text="匯出EXCEL" CssClass="KEY_IN_BUTTON_BLUE"
                                OnClick="btEXCEL_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btEXCEL" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
