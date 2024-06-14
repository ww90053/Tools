<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSelector.ascx.cs"
    Inherits="TPPDDB._2_salary.UserControl.UserSelector" %>
<asp:Panel ID="pl_Selector" runat="server" Width="420px" Height="300px" CssClass="DivPanel">
    <div style="width: 100%; text-align: right;">
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" OnClick="btn_Cancel_Click" />
    </div>
    <asp:GridView ID="gv_Target" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_Target_RowCommand"
        CssClass="Grid1" AllowPaging="True" 
        onpageindexchanging="gv_Target_PageIndexChanging" PageSize="7">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btn_Select" runat="server" Text="選取" CommandName="btnSelect" CommandArgument='<%# Eval("IDCARD") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PAY_AD" HeaderText="機關" />
            <asp:BoundField DataField="IDCARD" HeaderText="身份證" ItemStyle-Width="50px">
                <ItemStyle Width="50px" />
            </asp:BoundField>
            <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" ItemStyle-Width="60px">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="CHIOCCC" HeaderText="職稱" ItemStyle-Width="60px">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="NAME" HeaderText="姓名" ItemStyle-Width="50px">
                <ItemStyle Width="50px" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</asp:Panel>
