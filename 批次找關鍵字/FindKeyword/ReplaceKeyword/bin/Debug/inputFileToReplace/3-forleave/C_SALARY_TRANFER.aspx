<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_SALARY_TRANFER.aspx.cs" Inherits="TPPDDB._3_forleave.C_SALARY_TRANFER" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style10">
                <tr>
                    <td class="style8">
                        加班、超勤、值日轉檔作業
                    </td>
                    <td class="style4">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="style6" border="1">
                <tr>
                    <td class="style7">
                        年度月份
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="50px"></asp:TextBox>年<asp:TextBox
                            ID="TextBox_MZ_MONTH" runat="server" Width="40px"></asp:TextBox>月 (例：099年12月)
                    </td>
                </tr>
                <tr>
                    <td class="style7">
                        發薪機關
                    </td>
                    <td class="style3">
                         <asp:DropDownList ID="DropDownList_AD" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                      <%--  <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                            DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                            SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" 
                            DataSourceMode="DataReader">
                        </asp:SqlDataSource>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style7">
                        編制單位
                    </td>
                    <td class="style3">
                        <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource_UNIT"
                            DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)" 
                            ondatabound="DropDownList_UNIT_DataBound">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                            SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE),RTRIM(MZ_KCHI) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" 
                            DataSourceMode="DataReader">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td class="style7">
                        項目
                    </td>
                    <td class="style3">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                            RepeatDirection="Horizontal" AutoPostBack="True" 
                            onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                            <asp:ListItem Value="1" Selected="True">超勤</asp:ListItem>
                            <asp:ListItem Value="2">加班</asp:ListItem>
                            <asp:ListItem Value="3">值日</asp:ListItem>
                            <asp:ListItem Value="4">不休假</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <table class="style12">
                <tr>
                    <td style="text-align: center">
                        <asp:Button ID="bt_TRANFER" runat="server" Text="轉檔" OnClick="bt_TRANFER_Click" OnClientClick="return confirm(&quot;確定轉檔？&quot;)"
                            class="style9" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
