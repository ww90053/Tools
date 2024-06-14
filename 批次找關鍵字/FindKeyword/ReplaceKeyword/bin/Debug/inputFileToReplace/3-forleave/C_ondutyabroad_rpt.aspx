<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ondutyabroad_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_ondutyabroad_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: right;
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
    <table style="width: 70%;">
        <tr>
            <td align="center" class="title_s1">
                職員非請假出國/赴大陸報備表
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="423px">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style1">
                                        請假起日：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server"  MaxLength="7"
                                            Width="65px"></asp:TextBox>
                                        (範例:0981123)
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        身分證號：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" CausesValidation="True" MaxLength="10"
                                            Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        姓 名：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="423px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint_CN" runat="server" Text="赴大陸報備表" OnClick="btPrint_Click_CN" />
                                        <asp:Button ID="btPrint" runat="server" Text="出國報備表" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
