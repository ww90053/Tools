<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="A3_SalaryEdit.aspx.cs" Inherits="TPPDDB._3_forleave.A3_SalaryEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <link href="style/Master.css" rel="stylesheet" type="text/css" />

<%--    <style type="text/css">
        .Grid1 {
            width: 100%;
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: 微軟正黑體;
            color: #1E4979;
            letter-spacing: 0em;
        }

        .DivPanel {
            border: medium double #A6D2FF;
            padding: 20px;
            background-color: #DFDFDF;
        }

        .auto-style1 {
            text-align: left;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>--%>
        <style type="text/css">
            .auto-style1 {
                color: red;
                font-size: medium;
            }
            .auto-style2 {
                color: red;
                font-size: medium;
                text-align: left;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>';

    </script>

    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                Width="200px" Height="100px" Style="display: none;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="300">
                    <ProgressTemplate>
                        <div style="position: relative; top: 40%; text-align: center;">
                            <img src="/images/loading.gif" style="vertical-align: middle" alt="Processing" />
                            處理中 ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <div style="text-align: left; font-size: 16pt; font-family: 標楷體; background-color: #6699FF; color: White;">
                ECPA-A3薪資修改
            </div>
            <table class="TableStyleBlue" style="width: 95%;">
                <tr>
                    <th>編制機關</th>
                    <td class="auto-style2">
                        <asp:DropDownList ID="ddlAd" runat="server" AutoPostBack="True" Width="220px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>年月份</th>
                    <td class="auto-style2">
                        <asp:TextBox ID="TextBox_YearMonth" runat="server" MaxLength="5"></asp:TextBox>
                        (範例:10501)</td>
                </tr>
                <tr>
                    <th>身分證號碼(前六碼)</th>
                    <td class="auto-style2">
                        <asp:TextBox ID="TextBox_SearchID" runat="server" MaxLength="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>姓名</th>
                    <td class="auto-style2">
                        <asp:TextBox ID="TextBox_SearchName" runat="server" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="Button_SearchDone" runat="server" Text="條件查詢" OnClick="Button_SearchDone_Click" />
                    </td>
                </tr>
            </table>
            <div style="text-align: left; width: 95%;">
                <asp:Button ID="btn_showPopup" runat="server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="btn_showPopup_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showPopup" PopupControlID="Panel_Search"
                    BackgroundCssClass="modalBackground" CancelControlID="ibt_popupClose">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btn_different" runat="server" OnClick="btn_different_Click" Text="比對所有異常查詢" />
                &nbsp;<span class="auto-style1"><strong>需要輸入條件編制機關&amp;年月份</strong></span></div>
            <div style="width: 95%;">
                <asp:HiddenField ID="HiddenField_IDCARD" runat="server" Visible="False" />
                <asp:GridView ID="GridView_MANUFACTURER_BASE" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="SN" OnRowCommand="GridView_MANUFACTURER_BASE_RowCommand" Width="100%"
                    AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView_MANUFACTURER_BASE_PageIndexChanging"
                    CssClass="Grid1">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="sel"
                                    Text="修改" CommandArgument='<%# Eval("SN") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SN" HeaderText="編號" SortExpression="SN" Visible="false" />
                        <asp:BoundField DataField="MZ_ID" HeaderText="身分證號碼" SortExpression="MZ_ID" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                        <asp:BoundField DataField="ADName" HeaderText="編制機關" SortExpression="ADName" />
                        <asp:BoundField DataField="EXUNIT_NAME" HeaderText="編制單位" SortExpression="EXUNIT_NAME" />
                        <asp:BoundField DataField="AMONTH" HeaderText="年月份" SortExpression="AMONTH" />
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel ID="Panel_Search" runat="server" CssClass="DivPanel" Style="width: 700px; display: none;">
                <div style="width: 70%; text-align: right;">
                    <asp:ImageButton ID="ibt_popupClose" runat="server" ImageUrl="~/images/Back.gif" />
                </div>
                <table width="95%" class="TableStyleBlue">
                    <tr>
                        <th>身分證號碼：
                        </th>
                        <td colspan="3">
                            <asp:Label ID="Label_IDCARD" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>姓名：
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_NAME" runat="server" MaxLength="25" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">
                            <asp:Button ID="btn_popupSave" runat="server" Text="儲存" OnClick="btn_popupSave_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
