<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OverTime_PRIZE_Input.aspx.cs" Inherits="TPPDDB._3_forleave.C_OverTime_PRIZE_Input" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../styles/style09.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .row_header {
            width: 100px;
            font-weight: 700;
            color: #0033CC;
            text-align: justify;
            text-align-last: justify;
            background-color: #99FFCC;
        }

        .ViewHeader {
            color: #000000;
            font-weight: 700;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .EditHeader {
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .GV_Header {
            background-color: #507CD1;
            font-weight: bold;
            color: white;
            border: solid 1px #e4e3e3;
            text-align: center;
        }

        .GV_Hidden {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="style10 style8">
        <%= PageTitle %>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_Query" runat="server" GroupingText="查詢條件" Width="370px">
                <asp:Table runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            機 關：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_AD" AutoPostBack="true" OnSelectedIndexChanged="ddl_Search_AD_SelectedIndexChanged"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            單 位：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_Unit"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            年 度：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left; white-space:nowrap;">
                            <asp:TextBox runat="server" ID="txt_Year" MaxLength="3" Width="40px"></asp:TextBox>
                            <asp:RadioButtonList runat="server" ID="rbl_Year_Interval" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">上半年</asp:ListItem>
                                <asp:ListItem Value="1">下半年</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            身 分 證 號：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:TextBox runat="server" ID="txt_MZ_ID" MaxLength="10" Width="100px"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <table width="100%">
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button runat="server" ID="btn_Search" Text="查詢" CssClass="style9" OnClick="btn_Search_Click" />
                            <asp:Button runat="server" ID="btn_Apply" Text="申請" CssClass="style9" OnClick="btn_Apply_Click" />
                            <asp:Button runat="server" ID="btn_Cancel" Text="退回申請" CssClass="style9" OnClick="btn_Cancel_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="pnl_GridView" Width="400px" Height="300px">
                <cc1:TBGridView runat="server" ID="gv_resultat" Width="400px" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                    Style="text-align: left; margin-top: 0px;" ForeColor="#333333" EmptyDataText="無資料" EnableEmptyContentRender="true"
                    Visible="false">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="OVER_MONTH" HeaderText="加班月份" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="SURPLUS_HOUR" HeaderText="可申請時數" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PRIZE_HOUR" HeaderText="已申請時數" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle CssClass="GV_Header" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
                <cc1:TBGridView runat="server" ID="gv_manage_resultat" Width="400px" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                    Style="text-align: left; margin-top: 0px;" ForeColor="#333333" EmptyDataText="無資料" EnableEmptyContentRender="true"
                    Visible="false" DataKeyNames="MZ_ID,MZ_EXAD,MZ_EXUNIT">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="AD_NAME" HeaderText="機關" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="UNIT_NAME" HeaderText="單位" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SURPLUS_HOUR" HeaderText="可申請時數" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PRIZE_AMOUNT" HeaderText="已申請次數" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle CssClass="GV_Header" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
                <asp:Panel runat="server" ID="pnl_apply" Visible="false">
                    <table border="1" class="style6">
                        <tr>
                            <td colspan="2" style="width: 300px; font-weight: bold; text-align: center;">
                                申 請 嘉 獎 時 數
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox runat="server" ID="txt_apply_prize" Width="20px" OnTextChanged="txt_apply_prize_TextChanged" AutoPostBack="true"
                                    onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: left;">
                                備註:確定申請時數一經送出將無法修改，請再次確認後送出
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Apply" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Cancel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div style="color:red;">
        *本申請範圍包含業務加班及值日補休時數，不含超勤賸餘時數
    </div>
</asp:Content>
