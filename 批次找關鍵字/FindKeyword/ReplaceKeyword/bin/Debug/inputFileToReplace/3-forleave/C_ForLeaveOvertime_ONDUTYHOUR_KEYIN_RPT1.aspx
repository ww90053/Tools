<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_RPT1.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_RPT1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="title_s1">輪值表</div>
<div>
    <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Height="165px" 
        Width="361px">
        <table class="style1" width="100%">
            <tr>
                <td class="style2">
                    機關：</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_AD" runat="server" 
                        DataSourceID="SqlDataSource_AD" DataTextField="MZ_KCHI" 
                        DataValueField="MZ_KCODE" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                        SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')" DataSourceMode="DataReader">
                   
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    單位：</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_UNIT" runat="server" 
                        DataSourceID="SqlDataSource_UNIT" 
                        DataTextField="RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                        SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" 
                                PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    年度：</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_YEAR" runat="server" >
                       <%-- DataSourceID="SqlDataSource_YEAR" DataTextField="MZ_YEAR" 
                        DataValueField="MZ_YEAR">--%>
                    </asp:DropDownList>
                   <%-- <asp:SqlDataSource ID="SqlDataSource_YEAR" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                        SelectCommand="SELECT DISTINCT MZ_YEAR FROM C_ONDUTY_HOUR order by  MZ_YEAR desc" DataSourceMode="DataReader">
                    </asp:SqlDataSource>--%>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    月份：</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_MONTH" runat="server" 
                        style="margin-left: 3px">
                        <asp:ListItem Value="1">一月</asp:ListItem>
                        <asp:ListItem Value="2">二月</asp:ListItem>
                        <asp:ListItem Value="3">三月</asp:ListItem>
                        <asp:ListItem Value="4">四月</asp:ListItem>
                        <asp:ListItem Value="5">五月</asp:ListItem>
                        <asp:ListItem Value="6">六月</asp:ListItem>
                        <asp:ListItem Value="7">七月</asp:ListItem>
                        <asp:ListItem Value="8">八月</asp:ListItem>
                        <asp:ListItem Value="9">九月</asp:ListItem>
                        <asp:ListItem Value="10">十月</asp:ListItem>
                        <asp:ListItem Value="11">十一月</asp:ListItem>
                        <asp:ListItem Value="12">十二月</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2" colspan="2">
                    <asp:Button ID="Button_MAKE_RPT" runat="server" Text="產生報表" 
                        onclick="Button_MAKE_RPT_Click" />
                    &nbsp;<asp:Button ID="Button_CANCEL" runat="server" Text="取消" 
                        onclick="Button_CANCEL_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </div>
</asp:Content>
