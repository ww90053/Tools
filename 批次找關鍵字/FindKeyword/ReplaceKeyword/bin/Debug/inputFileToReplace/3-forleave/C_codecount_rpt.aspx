<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_codecount_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_codecount_rpt" %>

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
            text-align: right;
            width: 164px;
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
                員警差假統計表</div>
            <div>
            </div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="606px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                機關名稱：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" AutoPostBack="True"  
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged">
                                </asp:DropDownList>
                               
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                單位名稱：
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server" AutoPostBack="True" Width="100px"
                                    OnDataBound="DropDownList_MZ_UNIT_DataBound">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                日期區間：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_IDATE1" runat="server" Width="85px" MaxLength="7"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RF_YEAR" runat="server" ControlToValidate="TextBox_IDATE1"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                &nbsp;至<b>&nbsp;</b><asp:TextBox ID="TextBox_IDATE2" runat="server" Width="85px"
                                    MaxLength="7"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RF_YEAR0" runat="server" ControlToValidate="TextBox_IDATE2"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                (範例：0990101)
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="598px">
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
