<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Basic_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Basic_rpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 62px;
            text-align: right;
        }
    </style>

    <script type="text/javascript">
         function pageLoad() {
             var ppm = Sys.WebForms.PageRequestManager.getInstance();
             ppm.add_beginRequest(beginRequestHandler);
             //ppm.add_pageLoaded(pageLoaded);
         }

         function beginRequestHandler(sender, args) {
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_ALL') {
                // document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
         }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        全員基本資料名冊</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="380px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                機關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_AD" runat="server"  AutoPostBack="True" 
                                    onselectedindexchanged="DropDownList_AD_SelectedIndexChanged">
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                單位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_UNIT" runat="server" 
                                    ondatabound="DropDownList_UNIT_DataBound">
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="380px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="Button_MAKE_ALL" runat="server" Text="列印" OnClick="Button_MAKE_ALL_Click" />
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div style="text-align:center;">
                            <img alt="" src="../images/ajax-loader.gif" 
                                style="width: 220px; height: 19px" /><br />
                            <span style="color:Red;font-weight:bold;font-size:x-medium;">
                            <b style="font-size: medium">資料量多，產生中，請稍待…</b></span></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
