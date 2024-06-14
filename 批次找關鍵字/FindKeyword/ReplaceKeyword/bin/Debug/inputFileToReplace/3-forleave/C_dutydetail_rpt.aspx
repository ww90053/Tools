<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_dutydetail_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_dutydetail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

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
    <style type="text/css">

        .style3
        {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="90%">
                <tr>
                    <td class="title_s1">
                        員警差假明細表
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="584px">
                            <table>
                                <tr>
                                    <td align="right">
                                        服務機關：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXAD" runat="server"  AutoPostBack="True" 
                                            onselectedindexchanged="DropDownList_EXAD_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        服務單位：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" 
                                            AppendDataBoundItems="false" ondatabound="DropDownList_EXUNIT_DataBound">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                       
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        請假日期區間：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="90px"></asp:TextBox>
                                        &nbsp;
                                        <asp:RequiredFieldValidator ID="RF_YEAR" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE1" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        至<b> </b>
                                        <asp:TextBox ID="TextBox_MZ_DATE2" runat="server" Width="90px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RF_YEAR0" runat="server" 
                                            ControlToValidate="TextBox_MZ_DATE2" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例：0980101)
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="580px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="false" />
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
