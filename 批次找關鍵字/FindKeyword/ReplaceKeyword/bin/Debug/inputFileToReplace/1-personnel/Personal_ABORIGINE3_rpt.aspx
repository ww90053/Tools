<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_ABORIGINE3_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_ABORIGINE3_rpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 62px;
        }
    </style>

    <script type="text/javascript">
        function pageLoad() {
            var ppm = Sys.WebForms.PageRequestManager.getInstance();
            ppm.add_beginRequest(beginRequestHandler);
            //ppm.add_pageLoaded(pageLoaded);
        }

        function beginRequestHandler(sender, args) {
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btABORIGINE2C') {
                //document.getElementById("ctl00_ContentPlaceHolder1_btABORIGINE2").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        原住民人數統計管制表</div>
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
                                機關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                                    DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btABORIGINE1" runat="server" Text="原住民人員統計表" OnClick="btABORIGINE1_Click"
                                    Visible="False" />
                                <asp:Button ID="btABORIGINE2" runat="server" Text="預覽列印" OnClick="btABORIGINE2_Click" />
                                <asp:Button ID="btABORIGINE3" runat="server" Text="原住民員警名冊" OnClick="btABORIGINE3_Click"
                                    Visible="False" />
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
