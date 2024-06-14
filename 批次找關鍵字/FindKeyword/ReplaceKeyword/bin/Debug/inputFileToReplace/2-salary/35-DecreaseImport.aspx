<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="35-DecreaseImport.aspx.cs" Inherits="TPPDDB._2_salary._5_DecreaseImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="PageTitle">
        免稅金額匯入
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="TableStyleBlue" style="width: 100%;">
                <tr>
                    <th colspan="6" style="text-align: center;">
                        免稅金額Excel檔
                    </th>
                </tr>
                <tr>
                    <th>
                        檔案
                    </th>
                    <td colspan="5">
                        <asp:FileUpload ID="fl_import" runat="server" />
                        <asp:Button ID="btn_getExcel" runat="server" Text="取得資料" 
                            OnClick="btn_getExcel_Click" style="color: #FF0000; font-weight: 700" />
                        &nbsp;<asp:Button ID="btn_Back" runat="server" OnClick="btn_Back_Click" 
                            style="color: #0000FF; font-weight: 700" Text="上一頁" />
                        <asp:Label ID="lb_count" runat="server" Style="color: Red;" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="tr_data" visible="false">
                    <th>
                        年度
                    </th>
                    <td colspan="5">
                        <asp:TextBox ID="txt_Year" runat="server" Width="100px" MaxLength="3"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="fteYear" runat="server" TargetControlID="txt_Year"
                            FilterType="Numbers" />
                        &nbsp;
                        <asp:Button ID="btn_Add" runat="server" OnClick="btn_Add_Click" OnClientClick="return confirm('確定要匯入勾選的資料？')"
                            Text="匯入勾選資料" style="color: #009900; font-weight: 700" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gv_Result" runat="server" CssClass="Grid1" AutoGenerateColumns="False"
                Style="width: 50%;" Visible="False" Width="1px">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_add" runat="server" Checked="true" />
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="單位" DataField="CHIUNIT" />--%>
                    <%--<asp:BoundField HeaderText="員工編號" DataField="MZ_POLNO" />--%>
                    <asp:BoundField HeaderText="身份證字號" DataField="MZ_ID">
                        <ItemStyle Width="40%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="金額" DataField="AMOUNT">
                        <ItemStyle Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="姓名" DataField="MZ_NAME">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_getExcel" />
            <asp:PostBackTrigger ControlID="btn_Add" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
