<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_businessTrip_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_businessTrip_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .style2
        {
            width: 100%;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            font-family: DFKai-SB;
            color: #000000;
            padding: 5px;
            height: 32px;
        }
        .style3
        {
            text-align: right;
        }
        .style4
        {
            height: 23px;
            text-align: right;
        }
        .style5
        {
            text-align: right;
            color: #FF0000;
            font-size: medium;
            font-weight: bold;
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

    <%--<script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 70%;">
        <tr>
            <td align="center" class="style2">
                出差請示單
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="423px">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style3">
                                        出差日期起日：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server" AutoPostBack="True" MaxLength="7"
                                            Width="65px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RV1" runat="server" ControlToValidate="TextBox_MZ_DATE"
                                            Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例:0981123)
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        身分證號 ：
                                    </td>
                                    <td align="left" class="style1">
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" CausesValidation="True" MaxLength="10"
                                            Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style3">
                                        姓名 ：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="12" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="423px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btPrint" runat="server" Text="列印出差請示單" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btPrint1" runat="server" Text="列印旅費報告表" OnClick="btPrint1_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                                <span class="style5">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
