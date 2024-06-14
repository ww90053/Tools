<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="34-SoleImportBasic.aspx.cs" Inherits="TPPDDB._2_salary._4_SoleImportBasic" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/SoleItemSelector.ascx" TagName="SoleItemSelector" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="PageTitle">
        單一發放整批匯入
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="TableStyleBlue" style="width: 100%;">
                <tr>
                    <th colspan="6" style="text-align: center;">
                        單一發放Excel檔
                    </th>
                </tr>
                <tr>
                    <th>
                        檔案
                    </th>
                    <td colspan="5">
                        <asp:FileUpload ID="fl_import" runat="server" />
                        <asp:Button ID="btn_getExcel" runat="server" OnClick="btn_getExcel_Click" Text="取得資料" />
                        <asp:Label ID="lb_count" runat="server" Style="color: Red;" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="data1" Visible="false">
                    <th style="color: Red;">
                        入帳日期
                    </th>
                    <td>
                        <asp:TextBox ID="txt_DA" runat="server" MaxLength="7" Width="70px" AutoPostBack="True"
                            OnTextChanged="txt_DA_TextChanged"></asp:TextBox>
                    </td>
                    <th style="color: Red;">
                        入帳案號
                    </th>
                    <td>
                        <asp:TextBox ID="txt_caseid" runat="server" MaxLength="3" Width="30px" AutoPostBack="True"
                            OnTextChanged="txt_caseid_TextChanged"></asp:TextBox>
                    </td>
                    <th style="color: Red;">
                        項目
                    </th>
                    <td>
                        <uc1:SoleItemSelector ID="SoleItemSelector1" runat="server" />
                    </td>
                </tr>
                <tr runat="server" id="data2" Visible="false">
                    <th>
                        說明
                    </th>
                    <td colspan="5">
                        <asp:TextBox ID="txt_note" runat="server" Width="500px"></asp:TextBox>
                        <asp:Button ID="btn_Add" runat="server" OnClick="btn_Add_Click" OnClientClick="return confirm('確定要匯入勾選的資料？')"
                            Text="匯入勾選資料" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gv_Result" runat="server" CssClass="Grid1" AutoGenerateColumns="False"
                Style="width: 95%;" Visible="false" AllowPaging="True" 
                onpageindexchanging="gv_Result_PageIndexChanging" PageSize="200">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_add" runat="server" Checked="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="單位" DataField="CHIUNIT" />--%>
                    <%--<asp:BoundField HeaderText="員工編號" DataField="MZ_POLNO" />--%>
                    <asp:BoundField HeaderText="身份證字號" DataField="MZ_ID" />
                    <asp:BoundField HeaderText="姓名" DataField="MZ_NAME" />
                    <asp:BoundField HeaderText="金額" DataField="AMOUNT" />
                    <asp:BoundField HeaderText="所得稅" DataField="tax" />
                    <asp:BoundField HeaderText="健保費" DataField="pay1" />
                    <asp:BoundField HeaderText="公(勞)保費" DataField="pay2" />
                    <asp:BoundField HeaderText="自提基金" DataField="untax" />
                    <asp:BoundField HeaderText="自提離職儲金" DataField="pay3" />
                    <asp:BoundField HeaderText="法院扣款" DataField="extra01" />
                    <asp:BoundField HeaderText="備註說明" DataField="memo" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_getExcel" />
            <asp:PostBackTrigger ControlID="btn_Add" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
