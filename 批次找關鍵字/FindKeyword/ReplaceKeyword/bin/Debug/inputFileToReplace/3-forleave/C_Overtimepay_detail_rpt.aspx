<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_Overtimepay_detail_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_Overtimepay_detail_rpt" %>

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
    <div class="title_s1">
        超勤時數統計表列印</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="378px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                現服務機關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_AD" runat="server" AutoPostBack="True"  
                                    OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                                </asp:DropDownList>
                              <%--  <asp:DropDownList ID="DropDownList_AD" runat="server"  AppendDataBoundItems="True"
                                    AutoPostBack="True" 
                                    onselectedindexchanged="DropDownList_AD_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    >
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'" 
                                    DataSourceMode="DataReader">
                                </asp:SqlDataSource>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                現服務單位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_UNIT" runat="server"  AppendDataBoundItems="false" OnDataBound="DropDownList_UNIT_DataBound">
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                年度月份：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_DUTYDATE" runat="server" MaxLength="5"></asp:TextBox>
                                （範例:09801）
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                身分證號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                對象：
                            </td>
                            <td style="text-align: left">
                              <asp:RadioButtonList ID="Radio_Type" runat="server" RepeatLayout="Flow" AutoPostBack="true"  RepeatDirection="horizontal">
                                <asp:ListItem Value="0" Selected="True">ㄧ般人員</asp:ListItem>
                                <asp:ListItem   Value="1">刑事、少年、婦幼</asp:ListItem>
                               </asp:RadioButtonList>   
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="383px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="btPrint" runat="server" Text="確定" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                                <div class="style3" style="float:left;text-align:left">＊員警參加講習、會議、常訓、勤教等時間，如有超過8小時者，因非執行勤務，不宜發給超勤加班費(79年8月6日79警署人字第39437號函)。上班時間以外執行術科訓練者除外(108年10月31日警署人字1080154965號函)。<br />
                                ＊警察機關勤前教育實施規定第七點：員警參加勤前教育時間，不列入警察勤務條例第15條第2項前段所指之勤務時數。但得併計服勤時數，惟不得以該時段報支超勤時數。<br />
                                    ＊超勤加班費之核支，應依據勤務分配表、員警出入登記簿、工作紀錄簿或其他足資證明之出勤資料核計超勤時數，確實審核。有冒領或審查不實者，依法究辦並追究有關人員刑事及行政責任。
                                </div>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <br />
                        <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                        <span class="style3">資料量多，產生中 請稍待…</span>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
