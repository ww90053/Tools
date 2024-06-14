<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SoleItemSelector.ascx.cs"
    Inherits="TPPDDB._2_salary.UserControl.SoleItemSelector" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <tr>
        <td align="left">
            <asp:TextBox ID="txt_item" AutoPostBack="True" runat="server" MaxLength="2" Width="30px"
                OnTextChanged="txt_item_TextChanged"></asp:TextBox>
            <asp:Label ID="lbl_item" runat="server" Text=""></asp:Label>
            <asp:Button ID="btn_showNum" runat="server" Text="V" OnClick="btn_showNum_Click"
                TabIndex="-1" />
            <asp:CheckBox ID="CheckBox_TAXFLAG" runat="server" Enabled="False" Text="納入所得" />
            <asp:DropDownList ID="ddl_taxid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_taxid_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:DropDownList ID="ddl_taxid1" runat="server" AutoPostBack="True">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<div>
    <asp:Button ID="btn_showNumInv" runat="server" Text="Button" Style="display: none;"
        TabIndex="-1" />
    <cc1:ModalPopupExtender ID="btn_showNum_ModalPopupExtender" runat="server" DynamicServicePath=""
        Enabled="True" TargetControlID="btn_showNumInv" PopupControlID="pl_num" CancelControlID="ibt_numClose"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pl_num" runat="server" CssClass="Grid1" Style="width: 350px; display: none;">
        <div style="text-align: right; width: 95%;">
            <asp:ImageButton ID="ibt_numClose" runat="server" ImageUrl="~/images/Back.gif" />
        </div>
        <div>
            <asp:GridView ID="gv_num" runat="server" OnRowCommand="gv_num_RowCommand" AllowPaging="True"
                OnPageIndexChanging="gv_num_PageIndexChanging" Style="width: 95%;" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" Text="選取" CommandName="select" CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="代碼" DataField="ID" />
                    <asp:BoundField HeaderText="名稱" DataField="NAME" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</div>
