<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="Personal_Card_One_rpt_other.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Card_One_rpt_other" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript">
        function pageLoad() {
            var ppm = Sys.WebForms.PageRequestManager.getInstance();
            ppm.add_beginRequest(beginRequestHandler);
            //ppm.add_pageLoaded(pageLoaded);
        }

        function beginRequestHandler(sender, args) {
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_ALL') {
                document.getElementById("ctl00_ContentPlaceHolder1_Button_MAKE_ALL").disabled = true;
                args.get_postBackElement().disabled = true;
            }
            else if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_ALL1') {
                document.getElementById("ctl00_ContentPlaceHolder1_Button_MAKE_ALL1").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

    <style type="text/css">
        .style1
        {
            text-align: right;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        服務證單張列印</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                身分證號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                證&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號：</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_IDNO" runat="server" Width="100px"></asp:TextBox>
                                <asp:HiddenField ID="EXP_DATE" runat="server" Value="1110430" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="Button_MAKE_ALL" runat="server" Text="刑事警察" OnClick="Button_MAKE_ALL_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="Button_MAKE_ALL1" runat="server" 
                                    OnClick="Button_MAKE_ALL1_Click" Text="行政警察" />
                                </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div style="text-align: center;">
                            <img alt="" src="../images/ajax-loader.gif" style="width: 220px; height: 19px" /><br />
                            <span style="color: Red; font-weight: bold; font-size: x-medium;"><b style="font-size: medium">
                                資料量多，產生中，請稍待…</b></span></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
