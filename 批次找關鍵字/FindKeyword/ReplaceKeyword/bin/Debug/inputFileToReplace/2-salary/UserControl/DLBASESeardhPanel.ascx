<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DLBASESeardhPanel.ascx.cs"
    Inherits="TPPDDB._2_salary.UserControl.DLBASESeardhPanel" %>
<link href="../style/Master.css" rel="stylesheet" type="text/css" />
<div style="width: 300px;">
    <table class="TableStyleBlue" style="width: 100%;">
        <tr>
            <td>
                代 碼
            </td>
            <td>
                <asp:TextBox ID="TextBox_MZ_KCODE" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                名 稱
            </td>
            <td>
                <asp:TextBox ID="TextBox_MZ_KCHI" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btn_Search" runat="server" Text="過濾" OnClick="btn_Search_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gv_Result" runat="server" CssClass="Grid1" AutoGenerateColumns="False"
        AllowPaging="True" OnPageIndexChanging="gv_Result_PageIndexChanging" OnRowCommand="gv_Result_RowCommand">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="btnSelect"
                        Text="選取" CommandArgument='<%# Eval("MZ_KCODE") %>' />
                </ItemTemplate>
                <ItemStyle Width="5%" />
            </asp:TemplateField>
            <asp:BoundField DataField="MZ_KCODE" HeaderText="代碼" SortExpression="MZ_KCODE">
                <ItemStyle Width="20%" />
            </asp:BoundField>
            <asp:BoundField DataField="MZ_KCHI" HeaderText="名稱" SortExpression="MZ_KCHI">
                <ItemStyle Width="75%" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</div>
