<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryYear-Endbonus1.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryYear_Endbonus1" %>

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
        年終獎金產生</div>
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
            <table class="TableStyleBlue" style="width: 600px;">
                <tr>
                    <th>
                        產生年份：
                    </th>
                    <td>
                        民國
                        <asp:TextBox ID="txt_year" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txt_year_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" TargetControlID="txt_year" FilterType="Numbers">
                        </cc1:FilteredTextBoxExtender>
                        年
                    </td>
                </tr>
                <tr>
                    <th>
                        發薪機關：
                    </th>
                    <td>
                        <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" DataTextField="MZ_KCHI"
                            DataValueField="MZ_KCODE" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_MZ_AD_SelectedIndexChanged">
                        </asp:DropDownList>
                        共<asp:Label ID="lb_Counts" runat="server" Text=""></asp:Label>人
                    </td>
                </tr>
                <tr>
                    <th>
                        發給月數：
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_AMONTH" runat="server" Width="70px" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        產生條件：
                    </th>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonList_ALLORPersonnel" runat="server" AutoPostBack="True"
                            RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="RadioButtonList_ALLORPersonnel_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="ALL">全部產生</asp:ListItem>
                            <asp:ListItem Value="Personnel">單一個人產生</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trIDCARD" runat="server" visible="false">
                    <th>
                        身分證號：
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_Group_MZ_ID_Data" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="產生" 
                            onclientclick="return confirm(&quot;請確認欲產生之年份是否正確，若已有資料，會將資料刪除？&quot;)" />
                    </td>
                </tr>
                <tr>
                    <th>
                        產生失敗的資料：
                    </th>
                    <td>
                        <asp:ListBox ID="ListBox_Effect_Data" runat="server" Height="240px" Width="95%">
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="color: Blue; text-align: center; width: 50%; float: left;">
                            成功：
                            <asp:Label ID="lb_success" runat="server" Text=""></asp:Label>筆
                        </div>
                        <div style="color: Red; text-align: center;">
                            失敗：
                            <asp:Label ID="lb_fail" runat="server" Text=""></asp:Label>筆
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
