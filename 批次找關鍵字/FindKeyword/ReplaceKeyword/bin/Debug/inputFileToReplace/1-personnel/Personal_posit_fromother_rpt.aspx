<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_posit_fromother_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_posit_fromother_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            text-align: right;
        }
        .style5
        {
            width: 105px;
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
                document.getElementById("ctl00_ContentPlaceHolder1_btPrint1").disabled = true;

                args.get_postBackElement().disabled = true;
            }
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint1') {
                document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                document.getElementById("ctl00_ContentPlaceHolder1_btPrint").disabled = true;

                args.get_postBackElement().disabled = true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        轉發文件通知書</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style4">
                                    發文字第：
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="MZ_PRID" DataValueField="MZ_AD">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                        SelectCommand="SELECT &quot;MZ_PRID&quot;, &quot;MZ_AD&quot; FROM &quot;A_CHKAD&quot; ORDER BY MZ_AD" DataSourceMode="DataReader">
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    發文文號 ：
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" CausesValidation="True" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_PRID1"
                                        Display="Dynamic" ErrorMessage="發文文號不可空白"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="自行輸入條件" Width="514px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td style="text-align: right" class="style5">
                                    副本：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox1" runat="server" Height="67px" TextMode="MultiLine" Width="228px"
                                        Style="margin-left: 6px"></asp:TextBox>
                                    （請用、號分隔）
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="轉發文件通知書" OnClick="btPrint_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btPrint1" runat="server" Text="轉發文件通知書(稿)" OnClick="btPrint1_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                                <br />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
