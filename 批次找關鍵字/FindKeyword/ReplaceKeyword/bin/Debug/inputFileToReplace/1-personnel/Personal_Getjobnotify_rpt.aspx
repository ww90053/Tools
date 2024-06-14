<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Getjobnotify_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Getjobnotify_rpt" %>

<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        員警就職傳知書</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="380px">
                    <table width="100%">
                        <tbody class="style3">
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
                                    <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4" style="text-align: center">
                                    身分證字號
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="380px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
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
    </div>
</asp:Content>
