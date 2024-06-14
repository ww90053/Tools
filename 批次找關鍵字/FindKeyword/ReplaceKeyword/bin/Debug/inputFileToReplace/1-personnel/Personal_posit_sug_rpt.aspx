<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_posit_sug_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_posit_sug_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="A_UCLoading.ascx" TagName="A_UCLoading" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style5
        {
            width: 85px;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        任免建議函</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="361px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style5">
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
                                <td class="style5">
                                    發文文號
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" CausesValidation="True"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" TargetControlID="TextBox_MZ_PRID1" FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_PRID1"
                                        Display="Dynamic" ErrorMessage="發文文號不可空白"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="361px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPRINT_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCANCEL_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
