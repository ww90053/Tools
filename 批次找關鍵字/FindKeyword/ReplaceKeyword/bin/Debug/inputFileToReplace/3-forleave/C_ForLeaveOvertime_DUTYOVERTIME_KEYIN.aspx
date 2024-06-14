<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYOVERTIME_KEYIN.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYOVERTIME_KEYIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Overtime_hour" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server">
                
                <table class="style6">
                    <tr>
                        <td class="style5">
                            身分證號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style7">
                            自行輸入月份
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTYMONTH" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6">
                    <tr>
                        <td class="style7">
                            機關
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" Width="200px" TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="style7">
                            單 位
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" Width="105px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6">
                    <tr>
                        <td class="style7">
                            姓名
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            職稱
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            員警編號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6">
                    <tr>
                        <td class="style7">
                            1
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox1" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox1">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            2
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox2" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox2_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox2">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            3
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox3" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox3_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox3">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            4
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox4" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox4_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox4">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            5
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox5" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox5_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox5">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            6
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox6" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox6_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox6">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            7
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox7" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox7_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox7">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            8
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox8" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox8">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            9
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox9" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox9">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            10
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox10" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox10">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            11
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox11" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox11">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            12
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox12" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox12">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            13
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox13" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox13">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            14
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox14" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox14">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            15
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox15" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox15">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            16
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox16" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox16">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            17
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox17" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox17">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            18
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox18" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox18">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            19
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox19" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox19">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            20
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox20" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox20">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            21
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox21" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox21">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            22
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox22" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox22">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            23
                        </td class="style3">
                        <td>
                            <asp:TextBox ID="TextBox23" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox23">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            24
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox24" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox24">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            25
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox25" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox25">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            26
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox26" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox26">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            27
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox27" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox27">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            28
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox28" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox28">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            29
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox29" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox29">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            30
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox30" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox30">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style7">
                            31
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox31" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox31">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td colspan="3" class="style7">
                            合計
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_TOTAL" runat="server" Width="35px" AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6">
                    <tr>
                        <td class="style7">
                            備註
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_REMARK" runat="server" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6">
                    <tr>
                        <td class="style7">
                            預算（實際）小時
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_BUDGET_HOUR" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            最高可報小時
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_REAL_HOUR" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            剩餘時數
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_BALANCE_HOUR" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            專案小時
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PROJECT_HOUR" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            專案金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_PROJECT_PAY" runat="server" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style7">
                            薪俸
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_PAY1" runat="server" Width="80px" Height="19px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            專業加給
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_PROFESS" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            主管加給
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_BOSS" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            每小時金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_HOUR_PAY" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            超勤金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_OVERTIME_PAY" runat="server" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6">
                    <tr>
                        <td class="style7">
                            最高支領金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_HIGH_PAY" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            預算金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_BUDGET_PAY" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            訂定最高小時
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_HOUR_LIMIT" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td class="style7">
                            訂定最高金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_MONEY_LIMIT" runat="server" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <table class="style12">
                    <tr>
                        <td class="style34">
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" class="style9" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CausesValidation="False" class="style9" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" class="style9" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" class="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" class="style9" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" class="style9" AccessKey="d" />
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
