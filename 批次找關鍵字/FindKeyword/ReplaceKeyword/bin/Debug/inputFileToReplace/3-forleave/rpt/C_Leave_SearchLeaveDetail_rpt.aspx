<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_Leave_SearchLeaveDetail_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.rpt.C_Leave_SearchLeaveDetail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

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

    <style type="text/css">
        .style3
        {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
        .style4
        {
            width: 161px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
    </asp:ScriptManager>
    <table width="75%">
        <tr>
            <td class="title_s1">
                分局差假清單
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="557px">
                    <table width="100%">
                        <tr>
                            <td align="right" class="style4">
                                機關名稱：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_EXAD" runat="server" AppendDataBoundItems="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                              
                            </td>
                        </tr>
                        <%-- <tr>
                                    <td align="right" class="style4">
                                        服務單位：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" 
                                            ondatabound="DropDownList_EXUNIT_DataBound">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>--%>
                        <tr>
                            <td align="right" class="style4">
                                請假月份：
                            </td>
                            <td align="left">
                                <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="65px" MaxLength="5"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RF_YEAR" runat="server" ControlToValidate="TextBox_MZ_DATE1"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                               
                                (範例：09801)
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="558px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                                <span class="style3">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        </td> </tr> </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btPrint" />
                    </Triggers>
                </asp:UpdatePanel>
</asp:Content>
