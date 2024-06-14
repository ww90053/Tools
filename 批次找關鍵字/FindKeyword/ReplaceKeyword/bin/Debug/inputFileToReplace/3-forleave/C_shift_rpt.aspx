<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_shift_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_shift_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 98px;
            text-align: right;
        }
        .style5
        {
            text-align: center;
            font-size: medium;
            font-weight: bold;
            color: #FF0000;
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
    <div class="title_s1">
        年度勤惰記錄卡</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style4">
                                    年　　度：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" MaxLength="7" Width="80px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RF_YEAR" runat="server" ControlToValidate="TextBox_MZ_IDATE1"
                                        Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                    -<cc1:FilteredTextBoxExtender ID="TextBox_MZ_IDATE1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_IDATE1">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:TextBox ID="TextBox_MZ_IDATE1_1" runat="server" 
                                        MaxLength="7" Width="80px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_IDATE1_1_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_IDATE1_1">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RF_YEAR0" runat="server" 
                                        ControlToValidate="TextBox_MZ_IDATE1_1" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                    (範例:0980101)
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    身分證號：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="85px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RF_ID" runat="server" ControlToValidate="TextBox_MZ_ID"
                                        Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                </td>
                            </tr>                           
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="500px">
                    <table width="100%">
                        <tr>
                            <td  colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
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
                        <span class="style5">資料量多，產生中 請稍待…</span>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
