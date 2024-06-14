<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="90-B_BankListManager.aspx.cs" Inherits="TPPDDB._2_salary._0_B_BankListManager" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="font-size: 16pt; font-family: 標楷體; background-color: #6699FF; color: White;">
                銀行資料維護
            </div>
            <div style="height: 450px;">
                <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1" GroupItemCount="3" style="display: none;"
                    OnItemDataBound="ListView1_ItemDataBound">
                    <EmptyItemTemplate>
                        <td runat="server" />
                    </EmptyItemTemplate>
                    <ItemTemplate>
                        <td runat="server" style="background-color: #FFFBD6; color: #333333;">
                            銀行代碼:
                            <asp:Label ID="BANK_IDLabel" runat="server" Text='<%# Eval("BANK_ID") %>' />
                            <br />
                            銀行名稱:
                            <asp:Label ID="BANK_NAMELabel" runat="server" Text='<%# Eval("BANK_NAME") %>' />
                            <br />
                            <asp:Panel ID="pl_Btn" runat="server">
                            </asp:Panel>
                            <asp:Button ID="btn_Fake" runat="server" Text="" Style="display: none;" />
                        </td>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <td runat="server" style="background-color: #FAFAD2; color: #284775;">
                            銀行代碼:
                            <asp:Label ID="BANK_IDLabel" runat="server" Text='<%# Eval("BANK_ID") %>' />
                            <br />
                            銀行名稱:
                            <asp:Label ID="BANK_NAMELabel" runat="server" Text='<%# Eval("BANK_NAME") %>' />
                            <br />
                            <asp:Panel ID="pl_Btn" runat="server">
                            </asp:Panel>
                            <asp:Button ID="btn_Fake" runat="server" Text="" Style="display: none;" />
                        </td>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse;
                            border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td>
                                    未傳回資料。
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <InsertItemTemplate>
                        <td runat="server" style="">
                            BANK_ID:
                            <asp:TextBox ID="BANK_IDTextBox" runat="server" Text='<%# Bind("BANK_ID") %>' />
                            <br />
                            BANK_NAME:
                            <asp:TextBox ID="BANK_NAMETextBox" runat="server" Text='<%# Bind("BANK_NAME") %>' />
                            <br />
                            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="插入" />
                            <br />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="清除" />
                            <br />
                        </td>
                    </InsertItemTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table id="groupPlaceholderContainer" runat="server" border="1" style="background-color: #FFFFFF;
                                        border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;
                                        font-family: Verdana, Arial, Helvetica, sans-serif;">
                                        <tr id="groupPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td runat="server" style="text-align: center; background-color: #FFCC66; font-family: Verdana, Arial, Helvetica, sans-serif;
                                    color: #333333;">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="12">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False"
                                                ShowPreviousPageButton="False" />
                                            <asp:NumericPagerField />
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False"
                                                ShowPreviousPageButton="False" />
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EditItemTemplate>
                        <td runat="server" style="background-color: #FFCC66; color: #000080;">
                            BANK_ID:
                            <asp:TextBox ID="BANK_IDTextBox" runat="server" Text='<%# Bind("BANK_ID") %>' />
                            <br />
                            BANK_NAME:
                            <asp:TextBox ID="BANK_NAMETextBox" runat="server" Text='<%# Bind("BANK_NAME") %>' />
                            <br />
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="更新" />
                            <br />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="取消" />
                            <br />
                        </td>
                    </EditItemTemplate>
                    <GroupTemplate>
                        <tr id="itemPlaceholderContainer" runat="server">
                            <td id="itemPlaceholder" runat="server">
                            </td>
                        </tr>
                    </GroupTemplate>
                    <SelectedItemTemplate>
                        <td runat="server" style="background-color: #FFCC66; font-weight: bold; color: #000080;">
                            BANK_ID:
                            <asp:Label ID="BANK_IDLabel" runat="server" Text='<%# Eval("BANK_ID") %>' />
                            <br />
                            BANK_NAME:
                            <asp:Label ID="BANK_NAMELabel" runat="server" Text='<%# Eval("BANK_NAME") %>' />
                            <br />
                        </td>
                    </SelectedItemTemplate>
                </asp:ListView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM &quot;B_BANK_LIST&quot; ORDER BY &quot;BANK_ID&quot;">
                </asp:SqlDataSource>
                <asp:Panel ID="pl_Popup" runat="server" Style="display: ; background-color: White;
                    width: 250px; height: 85px;">
                    <div style="padding-top: 10px;">
                        銀行代碼：<asp:TextBox ID="txt_ID" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        銀行名稱：<asp:TextBox ID="txt_Name" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button ID="btn_Search" runat="server" Text="查詢" 
                            onclick="btn_Search_Click" />
                        <asp:Button ID="btn_Save" runat="server" Text="儲存" OnClick="btn_Save_Click" />
                        <asp:Button ID="btn_Cancel" runat="server" Text="取消" />
                    </div>
                </asp:Panel>
                <div>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" GridLines="None" 
                        onrowcommand="GridView1_RowCommand">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="BANK_ID" HeaderText="銀行代碼" />
                            <asp:BoundField DataField="BANK_NAME" HeaderText="銀行名稱" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" 
                                        CommandName="" Text="修改" CommandArgument='<%# Eval("BANK_ID") %>'></asp:LinkButton>
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
            
            <div style="background-color: #6699FF; color: White;">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" />
                <asp:Button ID="btnCreate" runat="server" Text="新增" />
                <cc1:ModalPopupExtender ID="btnCreate_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btnCreate" CancelControlID="btn_Cancel" PopupControlID="pl_Popup">
                </cc1:ModalPopupExtender>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
