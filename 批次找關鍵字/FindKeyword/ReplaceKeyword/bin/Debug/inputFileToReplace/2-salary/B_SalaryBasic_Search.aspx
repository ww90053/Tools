<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="B_SalaryBasic_Search.aspx.cs"
    Inherits="TPPDDB._2_salary.B_SalaryBasic_Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 25%;
        }
        .style3
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="style1" border="1">
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Panel ID="PanelAdmin" runat="server">
                                <table class="style1" border="1">
                                    <tr>
                                        <td class="style2">
                                            編制機關
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" AutoPostBack="True" MaxLength="10"
                                                OnTextChanged="TextBox_MZ_AD_TextChanged" Width="75px"></asp:TextBox>
                                            <asp:Button ID="btMZ_AD" runat="server" CausesValidation="False" OnClick="btMZ_AD_Click"
                                                TabIndex="-1" Text="V" />
                                            <asp:Label ID="Label_MZ_AD" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                            編制單位
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" AutoPostBack="True" MaxLength="4"
                                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" Width="35px"></asp:TextBox>
                                            <asp:Button ID="btMZ_UNIT" runat="server" CausesValidation="False" OnClick="btMZ_UNIT_Click"
                                                TabIndex="-1" Text="V" />
                                            <asp:Label ID="Label_MZ_UNIT" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                            現服機關
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" AutoPostBack="True" MaxLength="10"
                                                OnTextChanged="TextBox_MZ_EXAD_TextChanged" Width="75px"></asp:TextBox>
                                            <asp:Button ID="btMZ_EXAD" runat="server" CausesValidation="False" OnClick="btMZ_EXAD_Click"
                                                Style="height: 21px" TabIndex="-1" Text="V" />
                                            <asp:Label ID="Label_MZ_EXAD" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                            現服單位
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" AutoPostBack="True" MaxLength="4"
                                                OnTextChanged="TextBox_MZ_EXUNIT_TextChanged" Width="35px"></asp:TextBox>
                                            <asp:Button ID="btMZ_EXUNIT" runat="server" CausesValidation="False" OnClick="btMZ_EXUNIT_Click"
                                                TabIndex="-1" Text="V" />
                                            <asp:Label ID="Label_MZ_EXUNIT" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label_CLASS" runat="server" Text="發薪機關"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_PAY_AD" runat="server" AutoPostBack="True" MaxLength="10"
                                OnTextChanged="TextBox_PAY_AD_TextChanged" Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                            <asp:Button ID="btPAY_AD" runat="server" CausesValidation="False" OnClick="btPAY_AD_Click"
                                TabIndex="-1" Text="V" Enabled="false" Visible="false" />
                            <asp:Label ID="Label_PAY_AD" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList_PAY_AD_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RadioButtonList ID="RadioButtonList_ADType" runat="server" Font-Size="10pt"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">發薪機關</asp:ListItem>
                                <asp:ListItem Value="1">編制機關</asp:ListItem>
                                <asp:ListItem Value="2">現服機關</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            服務單位
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            員工編號
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" MaxLength="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label_IDNO" runat="server" Text="身分證號"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label_Name" runat="server" Text="姓名"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_AYEAR" runat="server">
                        <td class="style2">
                            入帳年度
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_AYEAR" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_AMONTH" runat="server">
                        <td class="style2">
                            入帳月份
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_AMONTH" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_SOLE_DA" runat="server">
                        <td class="style2">
                            入帳日期
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_DA" runat="server"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_DA_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_DA">
                            </cc1:FilteredTextBoxExtender>
                            <br />
                            <span class="style3">依日期查詢輸入：0980101</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" />
                            <asp:Button ID="btExit" runat="server" Text="離開" OnClick="btExit_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
