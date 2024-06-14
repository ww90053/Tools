<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_busitrip50_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_busitrip50_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

  <%--  <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>

    <style type="text/css">

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
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="title_s1">
                        出差前50名報表
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        服務機關：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXAD" runat="server"  AutoPostBack="True">
                                        </asp:DropDownList>
                                        <%--<asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                            
                                            SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" 
                                            DataSourceMode="DataReader">
                                        </asp:SqlDataSource>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        出差起迄日：
                                    </td>
                                    <td align="left">
                                        &nbsp;<asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="65px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RF_YEAR" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE1" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        &nbsp;至
                                        <asp:TextBox ID="TextBox_MZ_DATE2" runat="server" Width="65px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RF_YEAR0" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE2" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例：0980101)
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="500px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />&nbsp;&nbsp;&nbsp;
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
