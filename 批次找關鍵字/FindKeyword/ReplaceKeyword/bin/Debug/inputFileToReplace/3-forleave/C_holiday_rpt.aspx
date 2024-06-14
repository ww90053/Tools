<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_holiday_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_holiday_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 186px;
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
                        個人差假明細表
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="604px">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right" class="style1">
                                        請假起迄日期區間：
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                        <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="65px"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="RF_YEAR" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE1" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        &nbsp;至<b>&nbsp; </b>
                                        <asp:TextBox ID="TextBox_MZ_DATE2" runat="server" Width="65px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RF_YEAR0" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE2" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例：0980101)
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style1">
                                        身分證號：
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="596px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" 
                                            CausesValidation="False" />
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
