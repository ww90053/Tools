<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/TPPD.Master"  
    CodeBehind="A3_SalaryImport.aspx.cs" Inherits="TPPDDB._3_forleave.A3_SalaryImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .tdstyle {
            border: solid 1px #999999;
            font-size:medium;
        }
    </style>
        <script type="text/javascript">
            <%--  window.onsubmit = function () {
                if (Page_IsValid) {
                    var updateProgress = $find("<%= UpdateProgress1.ClientID %>");
                window.setTimeout(function () {
                    updateProgress.set_visible(true);
                }, 100);
            }
        }--%>
            window.onsubmit = function () {
                var updateProgress = $find("<%= UpdateProgress1.ClientID %>");
                window.setTimeout(function () {
                    updateProgress.set_visible(true);
                }, 100);
            }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1"></div>
            <asp:Panel ID="panel_Apply" runat="server" Width="90%" runat="server" BackColor="#cccccc"
                BorderColor="#999999" BorderStyle="Double" BorderWidth="5px">
                <table id="tb1" width="100%">
                    <tr>
                        <td style="text-align: right; width: 30%;" class="tdstyle">機關：
                        </td>
                        <td style="text-align: left; width: 80%" class="tdstyle">
                             <asp:DropDownList ID="ddlAd" runat="server" Width="220px" AutoPostBack="True"></asp:DropDownList>
                        </td>
                    </tr>
                      <tr>
                        <td style="text-align: right; width: 30%;" class="tdstyle">年度月份：
                        </td>
                        <td style="text-align: left; width: 80%" class="tdstyle">
                            <asp:TextBox ID="txt_Year" runat="server"></asp:TextBox>
                            範例:10501<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txt_Year" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="tdstyle">檔案：
                        </td>
                        <td style="text-align: left;" class="tdstyle">
                            <asp:FileUpload ID="FileUpload_Excel" runat="server" Width="99%"/>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="FileUpload_Excel" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tdstyle">
                            <asp:Button ID="btn_Save" runat="server" Text="匯入" OnClick="btn_Save_Click"/>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tdstyle">
                            <asp:Label ID="ErrorMsg" runat="server" Text="" Style="color:red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>            
            <asp:PostBackTrigger ControlID="btn_Save" />
        </Triggers>
    </asp:UpdatePanel>
                  <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal">
                <div class="center">
                   <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                        <span class="style3">資料匯入中，請稍待…</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
