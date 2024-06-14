<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_askleave_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_askleave_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 88px;
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
    <table style="width: 60%;">
        <tr>
            <td align="center" class="title_s1">
                請假報告單
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
                </asp:ScriptManager>              
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: right;" class="style1">
                                        請假日期：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server" AutoPostBack="True" 
                                            MaxLength="7" Width="65px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RF_YEAR" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例:0990101)
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style1">
                                        身分證號：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" CausesValidation="True" 
                                            Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style1">
                                        姓名：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style1">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="自行填寫">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: right" class="style1">
                                        受文者：
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" AppendDataBoundItems="True"
                                            AutoPostBack="True" 
                                            Style="margin-bottom: 0px">
                                        </asp:DropDownList>                                      
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        發文日期：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_PDATE" runat="server" MaxLength="7" Width="65px"></asp:TextBox>
                                        (範例:0981123)
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        發文文號：
                                    </td>
                                    <td align="left">
                                        <asp:ListBox ID="ListBox_MZ_PRID" runat="server"  Rows="1"></asp:ListBox>
                                        &nbsp;字
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        第
                                        <asp:TextBox ID="TextBox_NO" runat="server" MaxLength="10"></asp:TextBox>
                                        &nbsp;號
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        附件：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MEMO" runat="server" Height="48px" TextMode="MultiLine"
                                            Width="291px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" 
                                            CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                            AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" 
                                    style="width: 328px; height: 19px" /><br />
                                <span class="style3">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div>
    </div>
    <div>
    </div>
</asp:Content>
