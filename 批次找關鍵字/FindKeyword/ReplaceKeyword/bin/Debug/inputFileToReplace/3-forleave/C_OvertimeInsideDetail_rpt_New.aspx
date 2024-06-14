<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OvertimeInsideDetail_rpt_New.aspx.cs" Inherits="TPPDDB._3_forleave.C_OvertimeInsideDetail_rpt_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1 {
            font-size: x-large;
            font-weight: bold;
            text-align: center;
            font-family: DFKai-SB;
            color: #000000;
            padding: 5px;
        }

        .style2 {
            text-align: right;
        }

        .style3 {
            font-size: medium;
            font-weight: bold;
            color: #FF0000;
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
    <div class="style1">
        加班費明細表
    </div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
                    <table width="100%">
                        <tr>
                            <td class="style2">
                                機　　關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_AD" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                單　　位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_UNIT" runat="server">
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                年度月份：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_DATE" runat="server" MaxLength="5" Width="80px"></asp:TextBox>
                                （範例:09801）<asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                    runat="server" ControlToValidate="TextBox_MZ_DATE" Display="Dynamic"
                                    ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2">
                                身分證號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="btPrint" runat="server" Text="確定" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
