<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OVERTIME_OVERWORK_OVER45HR_RPT.aspx.cs" Inherits="TPPDDB._3_forleave.C_OVERTIME_OVERWORK_OVER45HR_RPT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1 {
            text-align: right;
        }

        .style3 {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        <%=ReportName %>
    </div>
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
                            <td class="style1">現服機關：
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
                            <td class="style1">現服單位：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">年度月份：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_YEARMONTH" runat="server" Style="text-align: left" MaxLength="5"
                                    Width="65px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RV1" runat="server" ControlToValidate="TextBox_YEARMONTH"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                <br />
                                (範例 ：11106，代表民國111年06月)
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="423px">
                    <table width="100%">
                        <tr>
                            <td class="style2" style="text-align: center">
                                <%----%>
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
                <asp:PostBackTrigger ControlID="btPrintExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

</asp:Content>
