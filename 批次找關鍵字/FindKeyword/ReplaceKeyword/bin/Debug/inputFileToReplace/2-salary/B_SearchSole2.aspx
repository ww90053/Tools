<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SearchSole2.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchSole2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style3
        {
            width: 158px;
            text-align: right;
        }
        .style2
        {
            text-align: left
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        單一發放明細表</div>
    <div>
    </div>
            <asp:Panel ID="Panel_SOLE" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <table class="style1">
                    <tr>
                        <td>
                                <table class="style1">
                <tr>
                    <td style="text-align: right" class="style2">
                    </td>
                    <td style="text-align: left">
                    </td>
                </tr>
                                    <tr>
                                        <td class="style3">
                                            <asp:Label ID="Label_AD" runat="server" Text="發薪機關："></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" 
                                                DataTextField="MZ_KCHI" 
                                                DataValueField="MZ_KCODE">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            <asp:Label ID="Label_POLNO" runat="server" Text="員工編號："></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="TextBox_MZ_POLNO" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            <asp:Label ID="Label_ID" runat="server" Text="身分證號："></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="TextBox_MZ_ID" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            <asp:Label ID="Label_Name" runat="server" Text="姓名："></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            種類：</td>
                                        <td style="text-align: left">
                                            <asp:RadioButtonList ID="RadioButtonList_TYPE" runat="server" 
                                                AutoPostBack="True" onload="RadioButtonList_TYPE_Load" 
                                                onselectedindexchanged="RadioButtonList_DATA_SelectedIndexChanged" 
                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Selected="True" Value="ALL">全部</asp:ListItem>
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
                                            <cc1:FilteredTextBoxExtender ID="TextBox_DA_FilteredTextBoxExtender0" 
                                                runat="server" Enabled="True" FilterType="Numbers" 
                                                TargetControlID="TextBox_DA">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            案號：</td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="DropDownList_CASEID" runat="server" 
                                                onselectedindexchanged="DropDownList_CASEID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            類別：</td>
                                        <td style="text-align: left">
                                            <asp:RadioButtonList ID="RadioButtonList_DA_INOUT_GROUP" runat="server" 
                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="ALL" Selected>全部</asp:ListItem>
                                                <asp:ListItem Value="IN">入帳</asp:ListItem>
                                                <asp:ListItem Value="OUT">沖銷</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label_MSG" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btPrint" runat="server" Text="查詢" onclick="btPrint_Click" 
                                style="height: 21px" />
                    &nbsp;<asp:Button ID="Button_CANCEL" runat="server" onclick="Button_CANCEL_Click" 
                        Text="取消" />
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
            </asp:Panel>
</asp:Content>
