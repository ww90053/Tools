<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="Personal_Efficiency_rpt.aspx.cs" Inherits="TPPDDB.Personal_Efficiency_rpt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 62px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        考績報表</div>
    <div>
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
            <table width="100%">
                <tr>
                    <td class="style1">
                        機關：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                            DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                            SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                        </asp:SqlDataSource>
                    </td>
                  
                    
                </tr>
                <tr>
                    <td class="style1">
                        單位：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource_UNIT"
                            DataTextField="RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td >
                       
                    </td>
                    <td>
                       
                        <asp:Button ID="Button1" runat="server" Text="考績(成)清冊" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="考績人數統計表" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button3" runat="server" Text="取消" />
                       
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
