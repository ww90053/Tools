<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Gradtsumlist_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Gradtsumlist_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="A_UCLoading.ascx" TagName="A_UCLoading" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        每月獎懲統計表</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="350px">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td align="right">
                                    年月：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MD" runat="server" MaxLength="5" Width="60px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MD_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MD">
                                    </cc1:FilteredTextBoxExtender>
                                    (範例：09812)
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    核定機關：
                                </td>
                                <td style="text-align: left"> 
                                    <asp:DropDownList ID="DropDownList_CHKAD" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                  
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="350px">
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
