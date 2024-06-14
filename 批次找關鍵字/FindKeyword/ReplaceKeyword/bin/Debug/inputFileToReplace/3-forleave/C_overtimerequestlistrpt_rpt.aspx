<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_overtimerequestlistrpt_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_overtimerequestlistrpt_rpt" %>

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
        超勤印領清冊</div>
    <div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="423px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                發薪機關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnSelectedIndexChanged="DropDownList_MZ_AD_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                編制單位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server"  OnDataBound="DropDownList_MZ_UNIT_DataBound">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                               
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                年度月份：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_DATE" runat="server" Style="text-align: left" MaxLength="5"
                                    Width="65px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RV1" runat="server" ControlToValidate="TextBox_DATE"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                (範例 ：09901)
                            </td>
                        </tr>
                         <tr>
                            <td class="style1">
                                身分證號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_ID" runat="server" Style="text-align: left" MaxLength="10"
                                    Width="100px"></asp:TextBox>
                        </tr>
                        <tr>
                            <td class="style1">
                            </td>
                            <td style="text-align: left"> 
                                <asp:RadioButtonList ID="RadioButtonList_UNIT" runat="server"  RepeatDirection="Horizontal">
                                <asp:ListItem Value="UNIT" Selected ="True">依編制單位列印</asp:ListItem>
                                <asp:ListItem Value="EXUNIT" >依現服單位列印</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="423px">
                    <table width="100%">
                        <tr>
                            <td class="style2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btPrintExcel" runat="server" Text="列印Excel" OnClick="btPrintExcel_Click" />
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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID = "btPrintExcel" />
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>
