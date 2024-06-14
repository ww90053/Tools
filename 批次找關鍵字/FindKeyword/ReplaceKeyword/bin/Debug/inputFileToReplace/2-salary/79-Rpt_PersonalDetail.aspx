<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="79-Rpt_PersonalDetail.aspx.cs" Inherits="TPPDDB._2_salary._9_Rpt_PersonalDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>'; 
    </script>

    <div class="PageTitle">
        全年薪資明細表
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
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
            <cc1:modalpopupextender id="Panel_Progress_ModalPopupExtender" runat="server" popupcontrolid="Panel_Progress"
                enabled="True" targetcontrolid="Panel_Progress" backgroundcssclass="modalBackground">
            </cc1:modalpopupextender>
            <table class="TableStyleBlue">
                <tr>
                    <th>
                        身份證字號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_idcard" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        年度
                    </th>
                    <td>
                        <asp:TextBox ID="txt_year" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btn_report" runat="server" Text="產生報表" OnClick="btn_report_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
