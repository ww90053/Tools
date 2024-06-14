<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Ontheojb_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_ontheojb_rpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript">
         function pageLoad() {
             var ppm = Sys.WebForms.PageRequestManager.getInstance();
             ppm.add_beginRequest(beginRequestHandler);
             //ppm.add_pageLoaded(pageLoaded);
         }

         function beginRequestHandler(sender, args) {
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_RPT') {
                 document.getElementById("ctl00_ContentPlaceHolder1_Button_MAKE_RPT1").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_RPT1') {
                 document.getElementById("ctl00_ContentPlaceHolder1_Button_MAKE_RPT").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        證明書</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="380px">
                    <table width="100%">
                        <tr>
                            <td style="text-align: right">
                                身分證字號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="True" 
                                    MaxLength="10" Width="100px"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="TextBox_MZ_ID" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="自行填寫" Width="380px">
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: right; width: 100px;">
                                發文文號：
                            </td>
                            <td align="left">
                                <asp:ListBox ID="ListBox_MZ_PRID" runat="server" DataSourceID="SqlDataSource2" DataTextField="MZ_PRID"
                                    DataValueField="MZ_AD" Rows="1"></asp:ListBox>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT MZ_PRID,MZ_AD FROM A_CHKAD ORDER BY MZ_AD" DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                                &nbsp;字
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                第&nbsp;
                            </td>
                            <td align="left">
                                &nbsp;<asp:TextBox ID="TextBox_NO" runat="server" MaxLength="10"></asp:TextBox>
                                &nbsp;號
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="380px">
                    <table width="100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="Button_MAKE_RPT" runat="server" OnClick="Button_MAKE_RPT_Click" Text="在職證明書" />
                                <asp:Button ID="Button_MAKE_RPT1" runat="server" OnClick="Button_MAKE_RPT1_Click"
                                    Text="經歷證明書" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
