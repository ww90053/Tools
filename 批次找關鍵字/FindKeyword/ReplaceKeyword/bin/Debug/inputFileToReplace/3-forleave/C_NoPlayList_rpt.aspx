<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_NoPlayList_rpt.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_NoPlayList_rpt" Title="因公未休假改發加班費印領清冊" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
        .style2
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">
                因公未休假改發加班費印領清冊</div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">
                                年 度：
                            </td>
                            <td class="style1">
                                <asp:TextBox ID="TextBox_YEAR" runat="server" MaxLength="3" Width="65px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                                </cc1:FilteredTextBoxExtender>
                                &nbsp;<asp:RequiredFieldValidator ID="RV1" runat="server" ControlToValidate="TextBox_YEAR"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                (範例：098)
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                機 關：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_EXAD" runat="server"  AutoPostBack="True" 
                                    onselectedindexchanged="DropDownList_EXAD_SelectedIndexChanged">
                                </asp:DropDownList>
                               
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                單 位：
                            </td> 
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" AppendDataBoundItems="false"
                                    OnDataBound="DropDownList_EXUNIT_DataBound">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                             
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                列印方式：
                            </td>
                            <td style="text-align: left">
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" 
                                    RepeatDirection="Horizontal" AutoPostBack="True" 
                                    onselectedindexchanged="RadioButtonList2_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="0">編制機關</asp:ListItem>
                                    <asp:ListItem Value="1">現服機關</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                列印條件：
                            </td>
                            <td style="text-align: left">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="0">一般人員</asp:ListItem>
                                    <asp:ListItem Value="1">司機工友</asp:ListItem>
                                    <asp:ListItem Value="2">離退職人員</asp:ListItem>
                                    <%--<asp:ListItem Value="3">約僱人員</asp:ListItem>--%>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="RadioButtonList3" runat="server" AutoPostBack="True" Visible="false"
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="RadioButtonList3_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="0">一般人員</asp:ListItem>
                                    <asp:ListItem Value="1">司機工友</asp:ListItem>
                                    <asp:ListItem Value="2">支援他機關</asp:ListItem>
                                    <%--<asp:ListItem Value="3">約僱人員</asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="500px">
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
                        <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                        <span class="style3">資料量多，產生中 請稍待…</span>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
