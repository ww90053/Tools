<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Criminal_police_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Criminal_police_rpt" %>

<%@ Register Src="A_UCLoading.ascx" TagName="A_UCLoading" TagPrefix="uc1" %>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        服務證名冊</div>
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
                        <tr>
                        <td class="style1">
                                發證日期：
                            </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txt_Year" runat="server" Width="74px"></asp:TextBox>~<asp:TextBox ID="txt_Year2" runat="server" Width="83px"></asp:TextBox>&nbsp; 
                            (範例:1020701)
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
                                <asp:Button ID="Button_MAKE_ALL1" runat="server" OnClick="Button_MAKE_ALL1_Click"
                                    Text="行政警察" />
                                    &nbsp;&nbsp;
                                     <asp:Button ID="Button_MAKE_ALL2" runat="server" OnClick="Button_MAKE_ALL2_Click"
                                    Text="一般行政" />
                                    &nbsp;&nbsp; <asp:Button ID="btn_excel" runat="server" 
                                    Text="匯出Excel" onclick="btn_excel_Click" />
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
        <Triggers >
         <asp:postbacktrigger  ControlID ="btn_excel" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
