<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic3.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: right;
        }
        .style3
        {
            text-align: left;
        }
        .styleTitle
        {
            text-align: left;
            font-size: 16pt;
            font-family: 標楷體;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="1" class="style1">
                <tr>
                    <td class="styleTitle" colspan="6" style="background-color: #6699FF; color: White;">
                        銀行資料設定
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        銀行名稱
                    </td>
                    <td class="style3">
                        <asp:DropDownList ID="DropDownList_BANK" runat="server" DataSourceID="SqlDataSource_BANK_LIST"
                            DataTextField="IDNAME" DataValueField="BANK_ID">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_BANK_LIST" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT BANK_ID, BANK_NAME, BANK_ID +'('+ BANK_NAME +')' AS IDNAME FROM B_BANK_LIST ORDER BY BANK_ID">
                        </asp:SqlDataSource>
                    </td>
                    <td class="style2">
                        分行名稱
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_NAME" runat="server" MaxLength="12"></asp:TextBox>
                    </td>
                    <td class="style2">
                        電話
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_TEL" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        傳真
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_FAX" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                    <td class="style2">
                        聯絡人姓名
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_TOUCH" runat="server" MaxLength="12"></asp:TextBox>
                    </td>
                    <td class="style2">
                        分行代號
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_BANKNO" runat="server" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div style="height: 400px;">
                            <asp:GridView ID="GridView_BANK" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="B_SNID" DataSourceID="SqlDataSource_BANK" GridLines="None" OnRowCommand="GridView_BANK_RowCommand"
                                Width="100%" ForeColor="#333333" AllowPaging="True" OnPageIndexChanging="GridView_BANK_PageIndexChanging"
                                PageSize="15" >
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" />
                                    <asp:BoundField DataField="BANK_NAME" HeaderText="銀行名稱" SortExpression="ID" />
                                    <asp:BoundField DataField="NAME" HeaderText="分行名稱" SortExpression="NAME" />
                                    <asp:BoundField DataField="TEL" HeaderText="電話" SortExpression="TEL" />
                                    <asp:BoundField DataField="FAX" HeaderText="傳真" SortExpression="FAX" />
                                    <asp:BoundField DataField="TOUCH" HeaderText="聯絡人姓名" SortExpression="TOUCH" />
                                    <asp:BoundField DataField="BANKNO" HeaderText="分行代號" SortExpression="BANKNO" />
                                    <asp:BoundField DataField="MZ_AD" HeaderText="建立單位" SortExpression="MZ_AD" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource_BANK" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: #6699FF; color: White;" colspan="6">
                        <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="新增" />
                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" />
                        <asp:Button ID="btExit" runat="server" OnClick="btExit_Click" Text="取消" />
                        <asp:Label ID="Label_MSG" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
