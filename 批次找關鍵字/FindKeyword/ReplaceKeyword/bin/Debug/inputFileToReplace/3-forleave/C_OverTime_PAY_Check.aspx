<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OverTime_PAY_Check.aspx.cs" Inherits="TPPDDB._3_forleave.C_OverTime_PAY_Check" %>

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
                <table width="100%">
                    <tr>
                        <td class="ViewHeader">機 關：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_AD" AutoPostBack="true" OnSelectedIndexChanged="ddl_Search_AD_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">單 位：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_Unit"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">年 月：
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox runat="server" ID="txt_Year" MaxLength="5" Width="60px"></asp:TextBox>(ex:10812)
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">身 分 證 號：
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox runat="server" ID="txt_MZ_ID" MaxLength="10" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">審 核 狀 態：
                        </td>
                        <td style="text-align: left;">
                            <asp:RadioButtonList runat="server" ID="rbl_PAY_CHK" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="">全部</asp:ListItem>
                                <asp:ListItem Value="N">未審核</asp:ListItem>
                                <asp:ListItem Value="Y">已審核</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button runat="server" ID="btn_Search" Text="查詢" CssClass="style9" OnClick="btn_Search_Click" />
                            <asp:Button runat="server" ID="btn_Check" Text="核准" CssClass="style9" OnClick="btn_Check_Click" />
                            <asp:Button runat="server" ID="btn_Cancel" Text="退回申請" CssClass="style9" OnClick="btn_Cancel_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="pnl_GridView" Width="100%" Height="300px">
                <cc1:TBGridView runat="server" ID="gv_resultat" Width="100%" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                    Style="text-align: left; margin-top: 0px;" ForeColor="#333333" Visible="false"
                    EmptyDataText="無資料" EnableEmptyContentRender="true" DataKeyNames="MZ_ID,OVER_DAY"
                    OnRowCreated="gv_resultat_RowCreated">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField HeaderText="選取" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:CheckBox ID="ckb_PAY_CHK" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PAY_CHK" HeaderText="審核狀態" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="OVER_TIME" HeaderText="加班日期" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="REASON" HeaderText="加班事由" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="PAY_HOUR" HeaderText="申請時數" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PAY_UNIT" HeaderText="每時金額" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PAY_SUM" HeaderText="金額小計" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle CssClass="GV_Header" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Check" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Cancel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
