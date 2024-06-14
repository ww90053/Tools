<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_PromotionPoint.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_PromotionPoint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
            font-family: 標楷體;
            width: 384px;
        }
        .style3
        {
            font-family: 標楷體;
            text-align: right;
        }
        .style4
        {
            font-size: large;
        }
        .style5
        {
            text-align: left;
        }
        .style7
        {
            text-align: left;
        }
        .style8
        {
            text-align: left;
        }
        .style9
        {
            text-align: left;
            width: 249px;
        }
        .style10
        {
            text-align: left;
        }
        .style11
        {
            text-align: left;
        }
        .style12
        {
            width: 141px;
            text-align: left;
        }
        .style13
        {
            text-align: left;
        }
        .style15
        {
            height: 19px;
            text-align: left;
        }
        .style16
        {
            text-align: left;
        }
        .style18
        {
            text-align: left;
            width: 68px;
        }
        .style19
        {
            text-align: left;
        }
        .style20
        {
            text-align: left;
        }
        .style21
        {
            text-align: left;
        }
        .style22
        {
            text-align: left;
        }
        .style23
        {
            text-align: left;
        }
        .style24
        {
            text-align: left;
        }
        .style25
        {
            text-align: left;
        }
        .style27
        {
            text-align: left;
            width: 90px;
        }
        .style28
        {
            width: 67px;
            text-align: left;
        }
        .style29
        {
            text-align: left;
            width: 91px;
        }
        .style30
        {
            text-align: left;
            width: 293px;
        }
        .style31
        {
            color: #FF0000;
        }
        .style32
        {
            text-align: left;
            color: #FF66FF;
        }
        .style33
        {
            text-align: left;
            color: #00CCFF;
        }
        .style34
        {
            text-align: center;
        }
        .style38
        {
            text-align: left;
            width: 59px;
        }
        .style111
        {
            border: solid 0px;
            color: #0033CC;
            font-weight: bold;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView_Excel1" runat="server" AutoGenerateColumns="False" BorderStyle="NotSet"
        GridLines="Both" Style="display: none;">
        <Columns>
            <asp:BoundField DataField="MZ_AD" HeaderText="編制機關" />
            <asp:BoundField DataField="MZ_UNIT" HeaderText="編制單位" />
            <asp:BoundField DataField="MZ_TBDV" HeaderText="職序" />
            <asp:BoundField DataField="MZ_SRANK" HeaderText="職等" />
            <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" />
            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
            <asp:BoundField DataField="MZ_ID" HeaderText="身分證字號" />
            <asp:BoundField DataField="MZ_SCTF1" HeaderText="訓練進修" />
            <asp:BoundField DataField="MZ_SCTF3" HeaderText="外語能力" />
            <asp:BoundField DataField="MZ_SCTF4" HeaderText="職務歷練" />
            <asp:BoundField DataField="MZ_SCTF2" HeaderText="發展潛能" />
            <asp:BoundField DataField="MZ_SCTN1" HeaderText="學識才能" />
            <asp:BoundField DataField="MZ_SCTN2" HeaderText="領導能力" />
            <asp:BoundField DataField="MZ_TOT3" HeaderText="小計" />
            <asp:BoundField DataField="MZ_SCTF6" HeaderText="綜合考評" />
            <asp:BoundField DataField="MZ_SCR1" HeaderText="甄審加分" />
            <asp:BoundField DataField="MZ_TOT" HeaderText="實得遷調資績總分" />
            <asp:BoundField DataField="MZ_TOT2" HeaderText="基本選項(七序列以下  * 80% 、六序列含以上   *  60%)" />
            <asp:BoundField DataField="MZ_TOT1" HeaderText="陞職總分" />
        </Columns>
    </asp:GridView>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr class="style4">
                    <td class="style2">
                        資績計分輸入
                    </td>
                    <td class="style3">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_PromotionPoint" runat="server">
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 50px;">
                            身分證號
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            姓 名
                        </td>
                        <td class="style9">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px"></asp:TextBox>
                            <asp:Button ID="btNameSearch" runat="server" OnClick="btNameSearch_Click" Text="姓名查詢"
                                Font-Bold="True" ForeColor="Maroon" />
                        </td>
                        <td class="KEY_IN_TITLE">
                            年度
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="65px" MaxLength="3"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_YEAR">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 50px;">
                            編制機關
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10" CssClass="style133"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_AD" runat="server" Display="Dynamic" OnServerValidate="CV_AD_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_AD"></asp:CustomValidator>
                        </td>
                        <td class="KEY_IN_TITLE">
                            編制單位
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_UNIT" runat="server" Display="Dynamic" OnServerValidate="CV_UNIT_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_UNIT"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            現服機關
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXAD_TextChanged" MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btEXAD" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXAD_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_EXAD" runat="server" Display="Dynamic" OnServerValidate="CV_EXAD_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_EXAD"></asp:CustomValidator>
                        </td>
                        <td class="KEY_IN_TITLE">
                            現服單位
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXUNIT_TextChanged" MaxLength="4" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btEXUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_EXUNIT" runat="server" Display="Dynamic" OnServerValidate="CV_EXUNIT_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_EXUNIT"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            職 稱
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_OCCC_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btOCCC_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_OCCC" runat="server" Display="Dynamic" OnServerValidate="CV_OCCC_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_OCCC"></asp:CustomValidator>
                        </td>
                        <td class="KEY_IN_TITLE">
                            職 等
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SRANK_TextChanged" MaxLength="3" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btSRANK" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSRANK_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style111" Width="180px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_SRANK" runat="server" Display="Dynamic" OnServerValidate="CV_SRANK_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_SRANK"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE">
                            職 序
                        </td>
                        <td class="style5" colspan="3">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="80px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_TBDV" runat="server" Display="Dynamic" OnServerValidate="CV_TBDV_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_TBDV"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 110px;">
                            實得遷調資機總分
                        </td>
                        <td class="style13">
                            <asp:TextBox ID="TextBox_MZ_TOT" runat="server" Width="100px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TOT_TextChanged"></asp:TextBox>
                            七序列以下人員只填職務歷練、發展潛能、綜合考評（綜合考評不能超過10分）
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            職務歷練
                        </td>
                        <td class="style16">
                            <asp:TextBox ID="TextBox_MZ_SCTF4" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCTF4_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            發展潛能
                        </td>
                        <td class="style19">
                            <asp:TextBox ID="TextBox_MZ_SCTF2" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCTF2_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            學識才能
                        </td>
                        <td class="style21">
                            <asp:TextBox ID="TextBox_MZ_SCTN1" runat="server" Width="80px" OnTextChanged="TextBox_MZ_SCTN1_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            領導能力
                        </td>
                        <td class="style23">
                            <asp:TextBox ID="TextBox_MZ_SCTN2" runat="server" Width="80px" OnTextChanged="TextBox_MZ_SCTN2_TextChanged"
                                Style="height: 19px" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            綜合考評
                        </td>
                        <td class="style25">
                            <asp:TextBox ID="TextBox_MZ_SCTF6" runat="server" Width="80px" OnTextChanged="TextBox_MZ_SCTF6_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            學歷加分
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCEA" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCEA_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            考試加分
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCTA" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCTA_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            年資加分
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCYA" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCYA_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            考績加分
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCMA" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCMA_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            獎懲加分
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCPA" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCPA_TextChanged" Style="height: 19px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            主管年資
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCPH" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCPH_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            副主管年資
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCSP" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCSP_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            訓練進修
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCTF1" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCTF1_TextChanged" Style="height: 19px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            外語能力
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_SCTF3" runat="server" Width="80px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SCTF3_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style5">
                            &nbsp;
                        </td>
                        <td class="style5">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            小 計
                        </td>
                        <td class="style27">
                            <asp:TextBox ID="TextBox_MZ_TOT3" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            甄審加分
                        </td>
                        <td class="style29">
                            <asp:TextBox ID="TextBox_MZ_SCR1" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BIGGER">
                            基本選項（七序列以下＊80%、六序列含以上＊40%）
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_TOT2" runat="server" Width="80px" 
                                AutoPostBack="false" ></asp:TextBox>
                        </td><!--20130613-->
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            陞職總分
                        </td>
                        <td class="style15" colspan="5">
                            <asp:TextBox ID="TextBox_MZ_TOT1" runat="server" Width="80px"></asp:TextBox>
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
                                Text="新增" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" meta:resourcekey="btUpperResource1" OnClick="btUpper_Click"
                                Text="上一筆" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="Button_Print" runat="server" Text="匯出" CssClass="KEY_IN_BUTTON_RED" />
                            <asp:GridView ID="GridView_Excel2" runat="server" AutoGenerateColumns="False" GridLines="Both"
                                Style="display: none;">
                                <Columns>
                                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                                    <asp:BoundField DataField="MZ_SCEA" HeaderText="發展潛能" />
                                    <asp:BoundField DataField="MZ_SCTA" HeaderText="考試加扣分" />
                                    <asp:BoundField DataField="MZ_SCYA" HeaderText="年資加扣分" />
                                    <asp:BoundField DataField="MZ_SCMA" HeaderText="考績加扣分" />
                                    <asp:BoundField DataField="MZ_SCPA" HeaderText="獎懲加扣分" />
                                    <asp:BoundField DataField="MZ_SCTF4" HeaderText="職務歷練" />
                                    <asp:BoundField DataField="MZ_SCTF2" HeaderText="發展潛能" />
                                    <asp:BoundField DataField="MZ_SCTN1" HeaderText="學識才能" />
                                    <asp:BoundField DataField="MZ_SCTN2" HeaderText="領導能力" />
                                    <asp:BoundField DataField="MZ_SCTF6" HeaderText="綜合考評" />
                                    <asp:BoundField DataField="MZ_SCR1" HeaderText="甄審加分" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style1">
                <tr>
                    <td class="style5">
                        <span class="style31">注意：學歷、考試、考績、獎懲、主管、副主管、警勤區、邊區、特勤警網及靖盧等年資與資績計分表內容無誤時請不要輸入</span><br
                            class="style31" />
                        <span class="style31">警勤區年資、邊區年資、特勤警網年資及靖盧等年資請輸入於【年資加分】欄位</span>
                    </td>
                </tr>
                <tr>
                    <td class="style32">
                        註：學歷、考試、考績、獎懲、正副主管年資等加分時需附證明文件
                    </td>
                </tr>
                <tr>
                    <td class="style33">
                        註：實得遷調資機總分=94年資績計分表（實得遷調資機總分＋學歷＋考試＋考績＋獎懲等加分＋正、副主管年資等）
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_Excel" runat="server" GroupingText="Excel匯出" BackColor="LightGray"
                Width="300px" Style="display: none;">
                <table border="1" style="text-align: left;">
                    <tr>
                        <td>
                            機關
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged"
                                OnDataBound="DropDownList_AD_DataBound">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') " DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            單位
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource2"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnDataBound="DropDownList_UNIT_DataBound">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            職序
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Report_TBDV" runat="server" Width="50px"></asp:TextBox>(例：009)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            年度
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Report_Year" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            類型
                        </td>
                        <td>
                            <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">資績分報表1</asp:ListItem>
                                <asp:ListItem Value="2">資績分報表2</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="Button_Excel" runat="server" Text="匯出" OnClick="Button_Excel_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button_Exit" runat="server" Text="離開" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Excel_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                DynamicServicePath="" Enabled="True" TargetControlID="Button_Print" PopupControlID="Panel_Excel"
                CancelControlID="Button_Exit">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="Button_PBExcel" runat="server" Text="Button" Style="display: none;"
        OnClick="Button_PBExcel_Click" />
</asp:Content>
