<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SearchYearSalary.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchYearSalary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 158px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        <asp:Label ID="Label_TITLE" runat="server"></asp:Label>
    </div>
    <div>
    </div>
    <div>
        <div>
            <asp:Panel ID="Panel1" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
                <table class="style1">
                    <tr>
                        <td style="text-align: right" class="style2">
                        </td>
                        <td style="text-align: left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" style="text-align: right">
                            年份：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" Width="85px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender6" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                            </cc1:FilteredTextBoxExtender>
                            &nbsp;年 (民國年，如：098)
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" style="text-align: right">
                            &nbsp;
                        </td>
                        <td style="text-align: left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="查詢" />
                            &nbsp;<asp:Button ID="Button_CANCEL" runat="server" OnClick="Button_CANCEL_Click"
                                Text="取消" />
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:Label ID="Label_MSG" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
    <div>
    </div>
</asp:Content>
