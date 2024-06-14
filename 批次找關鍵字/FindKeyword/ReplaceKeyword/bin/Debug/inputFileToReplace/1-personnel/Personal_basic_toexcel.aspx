<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_basic_toexcel.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_basic_toexcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
        .style3
        {
            text-align: left;
            width: 37px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table style="background-color: #6699FF; color: White; width: 100%; font-family: 標楷體;
        font-size: large;">
        <tr>
            <td style="text-align: left;">
                人事基本資料轉EXCEL作業
            </td>
            <td style="text-align: right">
                <asp:Label ID="Label1" runat="server" CssClass="style38"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="1" width="100%">
        <tr>
            <td class="style3">
                機 關
            </td>
            <td class="style1">
                <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                    DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True" 
                    ondatabound="DropDownList_AD_DataBound">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                    SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="style3">
                單 位
            </td>
            <td class="style1">
                <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource_UNIT"
                    DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)" OnDataBound="DropDownList_UNIT_DataBound">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE),RTRIM(MZ_KCHI) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="style3">
                項 目
            </td>
            <td class="style1">
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                    Width="404px">
                    <asp:ListItem Value="MZ_EXAD">現服機關</asp:ListItem>
                    <asp:ListItem Value="MZ_EXUNIT">現服單位</asp:ListItem>
                    <asp:ListItem Value="MZ_OCCC">職稱</asp:ListItem>
                    <asp:ListItem Value="MZ_NAME">姓名</asp:ListItem>
                    <asp:ListItem Value="MZ_ID">身份證號</asp:ListItem>
                    <asp:ListItem Value="MZ_BIR">出生日期</asp:ListItem>
                    <asp:ListItem Value="MZ_TBDV">職序</asp:ListItem>
                    <asp:ListItem Value="MZ_SRANK">薪俸職等</asp:ListItem>
                    <asp:ListItem Value="MZ_FDATE">初任日期</asp:ListItem>
                    <asp:ListItem Value="MZ_AD">編制機關</asp:ListItem>
                    <asp:ListItem Value="MZ_UNIT">編制單位</asp:ListItem>
                    <asp:ListItem Value="MZ_ADD1">戶籍地址</asp:ListItem>
                    <asp:ListItem Value="MZ_ADD2">現住地址</asp:ListItem>
                    <asp:ListItem Value="MZ_POLNO">員工編號</asp:ListItem>
                    <asp:ListItem Value="MZ_ADATE">到職日期</asp:ListItem>
                    <asp:ListItem Value="MZ_SLVC">俸階</asp:ListItem>
                    <asp:ListItem Value="MZ_SPT">俸點</asp:ListItem>
                    <asp:ListItem Value="MZ_ARMYRANK">兵種</asp:ListItem>
                    <asp:ListItem Value="MZ_ARMYCOURSE">官階</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <table style="width: 100%; text-align: left; background-color: #6699FF; color: White;">
        <tr>
            <td style="text-align: center">
                <asp:Button ID="bt_TRANFER" runat="server" Text="匯出" OnClick="bt_TRANFER_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
