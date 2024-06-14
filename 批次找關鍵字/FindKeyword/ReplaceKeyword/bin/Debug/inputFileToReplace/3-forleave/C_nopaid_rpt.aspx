<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_nopaid_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_nopaid_rpt" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: right;
        }
        .style2
        {
            color: red;
            font-weight: bold;
        }
        .style3
        {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
    </style>
    <script type="text/javascript">
         function pageLoad() {
             var ppm = Sys.WebForms.PageRequestManager.getInstance();
             ppm.add_beginRequest(beginRequestHandler);
             //ppm.add_pageLoaded(pageLoaded);
         }

         function beginRequestHandler(sender, args) {
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint') {
                 document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="title_s1">
                        未請領強制休假補助費明細表
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="423px">
                            <table>
                                <tr>
                                    <td class="style1">
                                        年　　度：</td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="TextBox_MZ_YEAR" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Numbers" 
                                            TargetControlID="TextBox_MZ_YEAR">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ControlToValidate="TextBox_MZ_YEAR" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例:098)
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        服務機關：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXAD" runat="server"  AutoPostBack="True" 
                                            AppendDataBoundItems="True" 
                                            onselectedindexchanged="DropDownList_EXAD_SelectedIndexChanged">
                                            <asp:ListItem Value =""></asp:ListItem>
                                        </asp:DropDownList>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        服務單位：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" 
                                            ondatabound="DropDownList_EXUNIT_DataBound">
                                            <asp:ListItem Value =""></asp:ListItem>
                                        </asp:DropDownList>
                                       
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="423px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click"  />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                            AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" 
                                    style="width: 328px; height: 19px" /><br />
                                <span class="style3">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
