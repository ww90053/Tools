<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_noplaylistTotal_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_noplaylistTotal_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
        .style2
        {
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
                        因公未休假改發加班費彙總表
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關：
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                                            AutoPostBack="True" >
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 度：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="65px" MaxLength="3"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_YEAR">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RF_YEAR" runat="server" ControlToValidate="TextBox_MZ_YEAR"
                                            Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例：099)
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        列印方式：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0">編制機關</asp:ListItem>
                                            <asp:ListItem Value="1">現服機關</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        列印條件：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="2">
                                            <asp:ListItem Selected="True" Value="0">一般人員</asp:ListItem>
                                            <asp:ListItem Value="1">司機工友</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Horizontal"
                                            Visible="false">
                                            <asp:ListItem Selected="True" Value="0">一般人員</asp:ListItem>
                                            <asp:ListItem Value="1">司機工友</asp:ListItem>
                                            <asp:ListItem Value="2">支援他機關</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="500px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                                <span class="style3">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
