<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_punish_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_punish_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript">
        function pageLoad() {
            var ppm = Sys.WebForms.PageRequestManager.getInstance();
            ppm.add_beginRequest(beginRequestHandler);
            //ppm.add_pageLoaded(pageLoaded);
        }

        function beginRequestHandler(sender, args) {
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint') {
                document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                document.getElementById("ctl00_ContentPlaceHolder1_btPrint1").disabled = true;
                args.get_postBackElement().disabled = true;
            }
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint1') {
                document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                document.getElementById("ctl00_ContentPlaceHolder1_btPrint").disabled = true;
                args.get_postBackElement().disabled = true;
            }
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btCancel') {
                document.getElementById("ctl00_ContentPlaceHolder1_btPrint").disabled = true;
                document.getElementById("ctl00_ContentPlaceHolder1_btPrint1").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    </div>
    <table style="width: 70%;">
        <tr>
            <td align="center" class="title_s1">
                獎 懲 令
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px" Style="text-align: center">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        發文字第
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        發文文號
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" CausesValidation="True" MaxLength="10"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_PRID1"
                                            Display="Dynamic" ErrorMessage="發文文號不可空白"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        職等
                                    </td>
                                    <td align="left">
                                        <asp:ListBox ID="ListBox_MZ_SRANK" runat="server" Rows="1">
                                            <asp:ListItem Value="P9G23">警正二階(含)以上</asp:ListItem>
                                            <asp:ListItem Value="P8G22" Selected="true">警正二階以下</asp:ListItem>
                                            <asp:ListItem Value="G3" >警監</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Button ID="btPrint" runat="server" Text="列印獎懲令" OnClick="btPrint_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btPrint1" runat="server" Text="列印獎懲令（稿）" OnClick="btPrint1_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Button ID="btn_PrintODF" runat="server" Text="印獎懲令(ODF)" OnClick="btn_PrintODF_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btn_Print1ODF" runat="server" Text="印獎懲令（稿）(ODF)" OnClick="btn_Print1ODF_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
