<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Basic2_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Basic2_rpt" %>

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
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_BOSS') {
                 document.getElementById("ctl00_ContentPlaceHolder1_Button_MAKE_BOSS0").disabled = true;
                 args.get_postBackElement().disabled = true;
             } 
             else if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button_MAKE_BOSS0') {
             document.getElementById("ctl00_ContentPlaceHolder1_Button_MAKE_BOSS").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
         }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        主官(管)基本資料名冊</div>
   <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
  <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="380px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                機關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                                    DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" ><%--AutoPostBack="True"--%>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <%--<tr style="display:none;">
                            <td class="style1">
                                單位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource_UNIT"
                                    DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>--%>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="380px">
                    <table width="100%">
                        <tr>
                            <td>
                                &nbsp;<asp:Button ID="Button_MAKE_BOSS" runat="server" Text="主官(管)列印" OnClick="Button_MAKE_BOSS_Click" />
                                &nbsp;<asp:Button ID="Button_MAKE_BOSS0" runat="server" OnClick="Button_MAKE_BOSS0_Click"
                                    Text="正、副主官(管)列印" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
              <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div style="text-align:center;">
                            <img alt="" src="../images/ajax-loader.gif" 
                                style="width: 220px; height: 19px" /><br />
                            <span style="color:Red;font-weight:bold;font-size:x-medium;">
                            <b style="font-size: medium">資料量多，產生中，請稍待…</b></span></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
            </div>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
