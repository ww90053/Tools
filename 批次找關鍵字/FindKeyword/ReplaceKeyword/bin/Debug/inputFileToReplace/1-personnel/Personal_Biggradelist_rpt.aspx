<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Biggradelist_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Biggradelist_rpt" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 118px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">
                記功過以上獎懲明細報表</div>
            <div>
            </div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="374px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr class="style3">
                                <td class="style4" style="text-align: right">
                                    時間區間
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_YM" runat="server" Width="65px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YM_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_YM">
                                    </cc1:FilteredTextBoxExtender>
                                    ~
                                    <asp:TextBox ID="TextBox_MZ_YM_2" runat="server" Width="65px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YM_2_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_YM_2">
                                    </cc1:FilteredTextBoxExtender>
                                    (範例:09811)
                                </td>
                            </tr>
                            <tr>
                                <td class="style4" style="text-align: right">
                                    獎懲結果
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_PRRST" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Value="4001">嘉獎一次</asp:ListItem>
                                        <asp:ListItem Value="4002">嘉獎二次</asp:ListItem>
                                        <asp:ListItem Value="4010">記功一次</asp:ListItem>
                                        <asp:ListItem Value="4020">記功二次</asp:ListItem>
                                        <asp:ListItem Value="4100">記一大功</asp:ListItem>
                                        <asp:ListItem Value="4200">記二大功</asp:ListItem>
                                        <asp:ListItem Value="5001">申誡一次</asp:ListItem>
                                        <asp:ListItem Value="5002">申誡二次</asp:ListItem>
                                        <asp:ListItem Value="5010">記過一次</asp:ListItem>
                                        <asp:ListItem Value="5020">記過二次</asp:ListItem>
                                        <asp:ListItem Value="5100">記一大過</asp:ListItem>
                                        <asp:ListItem Value="5200">記二大過</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="374px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                              
                                <asp:Button ID="btn_to_excel" runat="server" Text="匯出Excel" 
                                    onclick="btn_to_excel_Click"  />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btn_to_excel"  />
         <asp:PostBackTrigger ControlID="btPrint"  />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
