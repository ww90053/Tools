<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Online_duty.aspx.cs" Inherits="TPPDDB._18_Online_Leave.Online_duty" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .TableStyleWithTD
        {
            border-collapse: collapse;
            border: solid 1px dimgray;
            font-family: 微軟正黑體;
        }
        .TableStyleWithTD th
        {
            text-align: right;
            background-color: #EEEEFC;
            border: solid 1px dimgray;
            font-size: 10pt;
            padding-right: 6px;
        }
        .TableStyleWithTD td
        {
            height: 25px;
            border: solid 1px dimgray;
            padding-left: 6px;
            font-size: 10pt;
            text-align: left;
            background-color: White;
        }
        .headerA
        {
            background-image: url('../18-Online_Leave/images/K/headerAA.jpg');
            background-repeat: no-repeat;
            height: 67px;
            line-height: 25px;
            font-size: 16px;
            font-weight: bold;
            width: 930px;
        }
        .headerA2
        {
            background-image: url('../18-Online_Leave/Images/K/headerBB.JPG');
            background-repeat: no-repeat;
            height: 38px;
            line-height: 25px;
            font-size: 16px;
            font-weight: bold;
            width: 930px;
        }
    </style>
    <title></title>
</head>
<body style="margin-top: -1px; margin-left: -1px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table cellpadding="0">
                    <tr>
                        <td class="headerA">
                        </td>
                    </tr>
                    <tr>
                        <td class="headerA2" style="text-align: center; font-size: 14pt; font-family: 微軟正黑體;">
                            <asp:Label ID="Label2" runat="server" Text="勤務中心線上簽核"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 930px">
                <div style="float: left; width: 20%">
                    <table style="width: 100%; background-color: #B0E2FF; min-height: 480px; height: 480px;
                        font-family: 微軟正黑體;">
                        <tr>
                            <td style="height: 5%;">
                                <asp:LinkButton ID="lbtn_duty" runat="server" OnClick="lbtn_duty_Click">加班簽核</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 65%;">
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: right; width: 80%">
                    <asp:Panel ID="pl_decide" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="width: 30%">
                                </td>
                                <td style="text-align: right">
                                    <asp:Button ID="btn_return" runat="server" Text="退回" OnClick="btn_return_Click" Style="background-color: White;" />
                                    <asp:Button ID="btn_decision" runat="server" Text="確認" OnClick="btn_decision_Click"
                                        Style="background-color: #DC143C; color: White;" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pl_gv" runat="server">
                        <asp:GridView ID="gv_duty" runat="server" Width="100%" DataKeyNames="SN" AutoGenerateColumns="False"
                            CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gv_duty_RowDataBound">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField HeaderText="選取">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ck_select_ot" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班日期">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_date_ot" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班者">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_name_ot" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="OTREASON" HeaderText="加班事由" />
                                <asp:BoundField DataField="OTIME" HeaderText="加班時數" />
                                <asp:TemplateField HeaderText="本月加班時數">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_sumhour_ot" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="刷卡時間">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_sumhour_card" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PAY_SUM" HeaderText="加班金額" />
                                <asp:TemplateField HeaderText="狀態">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_s" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="歷程">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_history_ot" runat="server" CommandArgument='<%# Eval("SN") %>'
                                            Text="歷程" OnCommand="btn_history_ot_Command" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="pl_history" runat="server">
                        <asp:GridView ID="gv_history" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_history_RowDataBound"
                            CellPadding="4" ForeColor="#333333" GridLines="None" Style="margin-right: 0px">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField HeaderText="歷程O_SN" Visible="False">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_osn" runat="server" CommandArgument='<%# Eval("O_SN") %>' Text="Button" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="簽核時間">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_OTDATE" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="機關">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_overtime_exad" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="單位">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_overtime_exunit" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="職稱">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_occc_history" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="審核者">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_OTNAME" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="C_STATUS_NAME" HeaderText="狀態" />
                                <asp:BoundField DataField="REVIEW_MESSAGE" HeaderText="意見" />
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
            <cc1:ModalPopupExtender ID="M_P_E_MESSAGE" runat="server" BackgroundCssClass="modalBackground"
                DynamicServicePath="" Enabled="True" PopupControlID="pl_message" TargetControlID="btn_fake">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btn_fake" runat="server" Style="display: none;" Text="Button" />
            <asp:Panel ID="pl_message" runat="server" Width="50%" BackColor="White" Style="display: none;">
                <table class="TableStyleWithTD" width="100%">
                    <tr>
                        <th style="text-align: center;">
                            填寫意見
                        </th>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txt_message" runat="server" TextMode="MultiLine" Rows="5" Height="109px"
                                Width="287px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_message" runat="server" Text="確定" OnClick="btn_message_yes">
                            </asp:Button>
                            <asp:Button ID="btn_message_no" runat="server" Text="取消"></asp:Button>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
