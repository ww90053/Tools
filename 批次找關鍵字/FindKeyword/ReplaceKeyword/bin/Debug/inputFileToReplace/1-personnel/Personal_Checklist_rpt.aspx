<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Checklist_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Checklist_rpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 62px;
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
                 document.getElementById("ctl00_ContentPlaceHolder1_btPrint2").disabled = true;
                 document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint2') {
                 document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                 document.getElementById("ctl00_ContentPlaceHolder1_btPrint").disabled = true;
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
                獎懲核定名冊</div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style4" style="text-align: left">
                                    案號
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_NO1" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_MZ_NO1"
                                        Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                                    &nbsp;<b>─&nbsp; </b>
                                    <asp:TextBox ID="TextBox_MZ_NO2" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4" style="text-align: left">
                                    發佈權責
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_SWT3" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="1">1.警察局權責</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="2">2.警分局權責</asp:ListItem>
                                        <asp:ListItem Value="3">3.調他機關</asp:ListItem>
                                        <asp:ListItem Value="4">4.陳報警政署</asp:ListItem>
                                        <asp:ListItem Value="5">5.其他</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="514px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint2" runat="server" Text="全部" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btPrint" runat="server" Text="依機關" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btn_PrintODF" runat="server" Text="依機關(ODF)" OnClick="btn_PrintODF_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;    
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div style="text-align: center;">
                            <img alt="" src="../images/ajax-loader.gif" style="width: 220px; height: 19px" /><br />
                            <span style="color: Red; font-weight: bold; font-size: x-medium;"><b style="font-size: medium">
                                資料量多，產生中，請稍待…</b></span></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
