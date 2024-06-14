<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SearchMonthSalary1.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchMonthSalary1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  <style type="text/css">
        .style1
        {
            width: 100%;
            height: 10px;
        }
        .style3
        {
            width: 156px;
            height: 20px;
            text-align: left;
        }
        .style6
        {
            width: 117px;
            font-family: 新細明體;
        }
        .style8
        {
            height: 30px;
            font-size: 15pt;
            font-family: 標楷體;
        }
        .style14
        {
            height: 30px;
            font-size: 15pt;
            font-family: 標楷體;
            width: 188px;
        text-align: center;
    }
        .style21
        {
            height: 5px;
        }
        .style23
        {
            width: 19%;
            height: 5px;
            text-align: right;
        }
        .style25
        {
            height: 5px;
            text-align: left;
            width: 22%;
        }
        .style30
        {
            font-size: small;
        }
        .style38
        {
            font-size: medium;
        }
        .style39
        {
            font-size: 14pt;
        }
        .style40
        {
            width: 117px;
            font-size: medium;
            font-family: 新細明體;
        }
        .style41
        {
            width: 9%;
            text-align: right;
            font-size: small;
        }
        .style44
        {
            width: 54px;
            text-align: left;
        }
        .style45
        {
            text-align: right;
        }
        .style46
        {
            text-align: left;
            width: 22%;
        }
        .style47
        {
            font-size: small;
            width: 9%;
        }
        .style48
        {
            height: 30px;
            font-size: 15pt;
            font-family: 標楷體;
            width: 34%;
        }
        .style49
        {
            text-align: left;
            width: 109px;
        }
        .style50
        {
            font-size: small;
            text-align: right;
        }
        .style51
        {
            font-size: 12pt;
            width: 34%;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
    <table class="style1" style="border:solid 1px">
        <tr>
            <td class="style6">
                <span class="style38">員工編號：</td>
            <td class="style3">
                <asp:Label ID="Label_MZ_POLNO" runat="server" 
                    style="font-size: medium; font-family: 新細明體"></asp:Label>
                </span>
            </td>
            <td rowspan="2">
    <asp:Label ID="Label_TITLE" runat="server" style="font-size: 22pt"></asp:Label>
            </td>
        </tr>
        <tr class="style39">
            <td class="style40">
                身份證號：</td>
            <td style="text-align: left">
                <asp:Label ID="Label_IDCARD" runat="server" 
                    style="font-size: medium; font-family: 新細明體;"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    <div align="right" style="text-align: left">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_PRINT" runat="server" Text="列印" 
            onclick="Button_PRINT_Click" />
&nbsp; </div>
    <div>
        <table class="style1" border="1" 
            style="border-width: 1px; border-style: solid;">
            <tr>
                <td class="style14" colspan="2" width="30%">
                    &nbsp;&nbsp;
                    支領月份</td>
                <td class="style48" colspan="2">
                    應發項目</td>
                <td class="style8" colspan="2" width="36%">
                    應扣項目</td>
            </tr>
            <tr>
                <td colspan="2" class="style21" width="30%">
                    <asp:Label ID="Label_AMONTH" runat="server" CssClass="style30"></asp:Label>
                </td>
                <td class="style50">
                    月支數額：</td>
                <td class="style44">
                    <asp:Label ID="Label_SALARYPAY1" runat="server"></asp:Label>
                </td>
                <td class="style23">
                    公(勞)保費：</td>
                <td class="style25" width="36%">
                    <asp:Label ID="Label_INSURANCEPAY" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style41" width="30%">
                    姓名：</td>
                <td class="style49" width="30%">
                    <asp:Label ID="Label_MZ_NAME" runat="server" CssClass="style30"></asp:Label>
                </td>
                <td class="style50">
                    警勤加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_WORKP" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    健保費：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_HEALTHPAY" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style41" width="30%">
                    職稱：</td>
                <td class="style49" width="30%">
                    <asp:Label ID="Label_MZ_OCCC" runat="server" CssClass="style30" ></asp:Label>
                </td>
                <td class="style50">
                    專業加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_PROFESS" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    退撫金：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_CONCUR3PAY" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47" style="text-align: right" width="30%">
                    實發金額：</td>
                <td class="style49" width="30%">
                    <asp:Label ID="Label_TOTAL" runat="server" CssClass="style30" ></asp:Label>
                </td>
                <td class="style50">
                    主管加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_BOSS" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    薪資所得稅：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_TAX" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47" style="text-align: right" width="30%">
                    薪俸職等：</td>
                <td class="style49" width="30%">
                    <asp:Label ID="Label_MZ_SRANK" runat="server"></asp:Label>
                </td>
                <td class="style50">
                    技術加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_TECHNICS" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    健保費補扣款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_HEALTHPAY1" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47" style="text-align: right" width="30%">
                    俸階俸點：</td>
                <td class="style49" width="30%">
                    <asp:Label ID="Label_SLVC_SPOT" runat="server"></asp:Label>
                </td>
                <td class="style50">
                    工作獎助金：</td>
                <td class="style44">
                    <asp:Label ID="Label_BONUS" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    薪資扣款(列入所得)：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_MONTHPAY_TAX" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style30" colspan="2" align="left" rowspan="14" valign="top" 
                    width="30%">
                    <font class="style30" size="3">&nbsp; 備註：<br />
                    &nbsp;&nbsp;
                    </font>
                    <asp:TextBox ID="TextBox_NOTE" runat="server" CssClass="style30" Height="272px" 
                        TextMode="MultiLine" Width="211px"></asp:TextBox>
                </td>
                <td class="style50">
                    外事加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_ADVENTIVE" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    薪資扣款(不列入所得)：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_MONTHPAY" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50">
                    偏遠加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_FAR" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    法院扣款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA01" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50">
                    其他應發：</td>
                <td class="style44">
                    <asp:Label ID="Label_OTHER" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    國宅貸款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA02" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50">
                    勤務繁重加給：</td>
                <td class="style44">
                    <asp:Label ID="Label_ELECTRIC" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    銀行貸款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA03" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50">
                    &nbsp;</td>
                <td class="style44">
                    &nbsp;</td>
                <td class="style45">
                    分期付款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA04" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50" colspan="2">
                    &nbsp;</td>
                <td class="style45">
                    優惠存款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA05" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50" colspan="2">
                    &nbsp;</td>
                <td class="style45">
                    員工宿舍費：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA06" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style50" align="left" colspan="2">
                    &nbsp;</td>
                <td class="style45">
                    伙食費：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA07" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style51">
                    &nbsp;</td>
                <td class="style45">
                    退撫金貸款：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA08" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style51">
                    &nbsp;</td>
                <td class="style45">
                    福利互助金費：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_EXTRA09" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style51">
                    &nbsp;</td>
                <td class="style45">
                    其他應扣：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_MO" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style51">
                    &nbsp;</td>
                <td class="style45" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" class="style51">
                    &nbsp;</td>
                <td class="style45" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style50">
                    應發金額：</td>
                <td class="style44">
                    <asp:Label ID="Label_ADD_SUM" runat="server"></asp:Label>
                </td>
                <td class="style45">
                    應扣金額：</td>
                <td class="style46" width="36%">
                    <asp:Label ID="Label_DES_SUM" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
