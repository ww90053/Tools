<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalaryCode.ascx.cs"
    Inherits="TPPDDB._2_Salary.UserControl.SalaryCode" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<style type="text/css">
    .modalBackground
    {
        background-color: white;
        filter: alpha(opacity=75);
        opacity: 0.7;
    }
    input.cssbutton
    {
        background: #fff;
        padding: 3px;
        font-family: 微軟正黑體, Vera, Arial, Helvetica, sans-serif;
        font-size: 0.875em;
        font-variant: small-caps;
        border: 1px solid #aaa;
        cursor: hand;
        color: White;
        font-weight: bold;
    }
</style>
<div style="width: 500px; background-color: white;">
    <div style="text-align: right; width: 100%;">
        <asp:Button ID="txt_Create" runat="server" Text="新增" OnClick="txt_Create_Click">
        </asp:Button>
        <asp:Button ID="btn_Hide" runat="server" Text="" Style="display: none;" />
        <cc1:ModalPopupExtender ID="popup_ModalPopupExtender" runat="server" DynamicServicePath=""
            Enabled="True" TargetControlID="btn_Hide" BackgroundCssClass="modalBackground"
            CancelControlID="btn_Cancel" PopupControlID="pl_Popup">
        </cc1:ModalPopupExtender>
    </div>
    <div style="text-align: center; width: 100%; border: solid 2px gray;">
        <asp:GridView ID="gv_Data" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False" OnRowCommand="gv_Data_RowCommand" AllowPaging="True"
            OnPageIndexChanging="gv_Data_PageIndexChanging" Width="100%">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="代碼" HeaderStyle-Width="40px" />
                <asp:BoundField DataField="NAME" HeaderText="名稱" />
                <asp:BoundField DataField="PAY" HeaderText="金額" HeaderStyle-Width="100px" />
                <asp:TemplateField HeaderText="修改" ShowHeader="False" HeaderStyle-Width="40px">
                    <ItemTemplate>
                        <asp:Button ID="btn_Update" runat="server" CausesValidation="false" CommandName="Upd"
                            Text="修改" CommandArgument='<%# Eval("ID") %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="刪除" ShowHeader="False" HeaderStyle-Width="40px">
                    <ItemTemplate>
                        <asp:Button ID="btn_Delete" runat="server" CausesValidation="false" CommandName="Del"
                            Text="刪除" OnClientClick="alert('確定要刪除？')" CommandArgument='<%# Eval("ID") %>'>
                        </asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
</div>
<asp:Panel ID="pl_Popup" runat="server" Style="display: none; border: solid 1px gray;
    background-color: White; width: 270px; padding: 15px 15px 15px 15px;">
    <table width="100%">
        <tr>
            <td style="text-align: right; width: 25%; padding-right: 10px;">
                <asp:Label ID="lbl_ID1" runat="server" Text="代碼"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txt_ID1" runat="server" MaxLengtd="2" Width="20px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 25%; padding-right: 10px;">
                <asp:Label ID="lbl_Name1" runat="server" Text="名稱"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txt_Name1" runat="server" MaxLengtd="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 25%; padding-right: 10px;">
                <asp:Label ID="lbl_Pay1" runat="server" Text="金額"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txt_Pay1" runat="server" MaxLengtd="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: right; padding-top: 14px">
                <asp:Button ID="btn_Save" runat="server" Text="儲存" OnClick="btn_Save_Click" CssClass="cssbutton"
                    Style="background-color: #2070FF;" />
                <asp:Button ID="btn_Cancel" runat="server" Text="取消" CssClass="cssbutton" Style="color: Black;" />
            </td>
        </tr>
    </table>
</asp:Panel>
