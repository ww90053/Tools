<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="11-PoliceImport.aspx.cs" Inherits="TPPDDB._2_salary._1_PoliceImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>'; 
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
        基本資料整批匯入
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                Width="200px" Height="100px" Style="display: none;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
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
            <table class="TableStyleBlue" style="width: 600px; margin-top: 10px;">
                <tr>
                    <th>
                        基本資料檔案(Excel)
                    </th>
                    <td>
                        <asp:FileUpload ID="fl_import" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btn_getdata" runat="server" Text="開始匯入" OnClick="btn_getdata_Click" />
                    </td>
                </tr>
                <tr>
                    <th>
                        匯入失敗的資料：
                    </th>
                    <td>
                        <asp:ListBox ID="lbx_fail" runat="server" Width="95%" Height="300px">
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="color: Blue; text-align: center; width: 30%; float: left;">
                            資料異動：
                            <asp:Label ID="lb_success" runat="server" Text=""></asp:Label>筆
                        </div>
                        <div style="color: Red; text-align: center;">
                            失敗：
                            <asp:Label ID="lb_fail" runat="server" Text=""></asp:Label>筆
                        </div>
                        <div style="text-align: center; width: 30%; float: left;">
                            檔案資料共：
                            <asp:Label ID="lb_total" runat="server" Text=""></asp:Label>筆
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_getdata" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
