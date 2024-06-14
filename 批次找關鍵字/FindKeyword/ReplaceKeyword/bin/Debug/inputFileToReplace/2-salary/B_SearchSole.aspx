<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SearchSole.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchSole" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left
        }
        .style3
        {
            width: 158px;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        單一發放明細表</div>
                            <asp:Panel ID="Panel_SOLE" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <table class="style1">
                <tr><td></td></tr>
                    <tr>
                        <td>
                                <table class="style1">
                                    <tr>
                                        <td class="style3">
                                            種類：</td>
                                        <td style="text-align: left">
                                            <asp:RadioButtonList ID="RadioButtonList_TYPE" runat="server" 
                                                RepeatDirection="Horizontal" RepeatLayout="Flow" 
                                                onselectedindexchanged="RadioButtonList_DATA_SelectedIndexChanged" 
                                                AutoPostBack="True" onload="RadioButtonList_TYPE_Load">
                                                <asp:ListItem Value="ALL" Selected="True">全部</asp:ListItem>
                                                <asp:ListItem Value="AYEAR">年度</asp:ListItem>
                                                <asp:ListItem Value="AMONTH">月份</asp:ListItem>
                                                <asp:ListItem Value="ADAY">日期</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="tr_DA" runat="server">
                                        <td class="style3">
                                            日期：</td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="TextBox_DA" runat="server" MaxLength="7"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="TextBox_DA_FilteredTextBoxExtender" 
                                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TextBox_DA">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            案號：</td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="DropDownList_CASEID" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            類別：</td>
                                        <td style="text-align: left">
                                            <asp:RadioButtonList ID="RadioButtonList_DA_INOUT_GROUP" runat="server" 
                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="ALL" Selected="True">全部</asp:ListItem>
                                                <asp:ListItem Value="IN">入帳</asp:ListItem>
                                                <asp:ListItem Value="OUT">沖銷</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                            <asp:Label ID="Label_MSG" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                            </td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                </table>
                <table class="style1">
                    <tr>
                        <td>
                            <asp:Button ID="btPrint" runat="server" Text="查詢" onclick="btPrint_Click" />
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
            </asp:Panel>
</asp:Content>
