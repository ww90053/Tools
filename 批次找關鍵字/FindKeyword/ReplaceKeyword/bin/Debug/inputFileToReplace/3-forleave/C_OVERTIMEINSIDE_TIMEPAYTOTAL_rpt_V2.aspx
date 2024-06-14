<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OVERTIMEINSIDE_TIMEPAYTOTAL_rpt_V2.aspx.cs" Inherits="TPPDDB._3_forleave.C_OVERTIMEINSIDE_TIMEPAYTOTAL_rpt_V2" %>

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
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrintExcel') {
                document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        超勤、業務加班費時數及金額統計表</div>
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
                                機關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                單位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                身分證號：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                人員職等：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_SRANK" runat="server">
                                    <asp:ListItem Value="0" Text="全選" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="警職"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="一般職員"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="其他"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                年月起迄：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_DATE_S" runat="server" Style="text-align: left" MaxLength="5"
                                    Width="65px"></asp:TextBox>
                                ~
                                <asp:TextBox ID="TextBox_DATE_E" runat="server" Style="text-align: left" MaxLength="5"
                                    Width="65px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RV1" runat="server" ControlToValidate="TextBox_DATE_S"
                                    Display="Dynamic" ErrorMessage="起始日不可空白"></asp:RequiredFieldValidator>
                                <br />
                                (範例 ：11106，代表民國111年06月)
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                列印項目
                            </td>
                            <td style="text-align: left"> 
                                <asp:RadioButtonList ID="RadioButtonList_TYPE" runat="server"  RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text="業務" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="超勤"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:Label runat="server" ForeColor="Red">超勤統計表僅以月份區隔產生</asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="423px">
                    <table width="100%">
                        <tr>
                            <td class="style2" style="text-align: center">
                                <asp:Button ID="btPrintExcel" runat="server" Text="列印Excel" OnClick="btPrintExcel_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
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
    </div>
</asp:Content>
