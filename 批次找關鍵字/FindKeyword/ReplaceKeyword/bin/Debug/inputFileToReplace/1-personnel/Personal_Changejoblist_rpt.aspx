<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="Personal_Changejoblist_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Changejoblist_rpt" %>
<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 70%;">
        <tr>
            <td align="center" class="title_s1">
                調任人員傳知名冊
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="380px">
                            <table width="100%";>
                                <tr>
                                    <td>
                                        發文字第
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" DataSourceID="SqlDataSource1"
                                            DataTextField="MZ_PRID" DataValueField="MZ_AD">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                            SelectCommand="SELECT &quot;MZ_PRID&quot;, &quot;MZ_AD&quot; FROM &quot;A_CHKAD&quot; ORDER BY MZ_AD" DataSourceMode="DataReader">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        發文文號
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" CausesValidation="True" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="380px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" style="text-align: center">
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:Content>
