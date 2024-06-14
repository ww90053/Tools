<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OverTime_Detail_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_OverTime_Detail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1 {
            text-align: left;
        }

        .style2 {
            text-align: right;
        }

        .style3 {
            font-weight: bold;
            color: red;
            font-size: medium;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="style10 style8">
        加班明細表
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnl_Query" runat="server" GroupingText="查詢條件" Width="500px">
                <table style="width: 100%;">
                    <tr>
                        <th class="style2">日期區間：
                        </th>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txt_DateS" Width="85px" MaxLength="7"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txt_DateS" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="txt_DateS">
                            </cc1:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_txt_DateS" runat="server" ControlToValidate="txt_DateS"
                                Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                            &nbsp;至&nbsp;
                            <asp:TextBox runat="server" ID="txt_DateE" Width="85px" MaxLength="7"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txt_DateE" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="txt_DateE">
                            </cc1:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_txt_DateE" runat="server" ControlToValidate="txt_DateE"
                                Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                            (範例：1090101)
                        </td>
                    </tr>
                    <tr>
                        <th class="style2">服務機關：</th>
                        <td class="style1">
                            <asp:DropDownList runat="server" ID="ddl_Search_EXAD" AutoPostBack="true" OnSelectedIndexChanged="ddl_Search_EXAD_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th class="style2">服務單位：</th>
                        <td class="style1">
                            <asp:DropDownList runat="server" ID="ddl_Search_EXUNIT"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th class="style2">身分證號：</th>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txt_Search_ID" Width="100px" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="style2">姓 名：</th>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txt_Search_NAME" Width="100px" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnl_button" runat="server" GroupingText="功能列" Width="500px">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button runat="server" ID="btn_Print" Text="列印" OnClick="btn_Print_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table width="500px">
                <tr>
                    <td>
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
    </asp:UpdatePanel>
</asp:Content>
