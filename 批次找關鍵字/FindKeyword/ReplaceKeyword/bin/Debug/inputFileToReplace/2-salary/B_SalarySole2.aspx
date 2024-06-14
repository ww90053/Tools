<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalarySole2.aspx.cs" Inherits="TPPDDB._2_salary.B_SalarySole2" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="1" class="style1">
                <tr>
                    <td colspan="2" style="text-align: left; font-size: 16pt; font-family: 標楷體; background-color: #6699FF;
                        color: White;">
                        單一發放代碼設定
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        ID代碼
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_ID" runat="server" MaxLength="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        名稱
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_NAME" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        是否納入所得
                    </td>
                    <td class="style3">
                        <asp:RadioButtonList ID="RadioButtonList_TAXES_YESNO" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList_TAXES_YESNO_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Y">是</asp:ListItem>
                            <asp:ListItem Value="N">否</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        所得格式
                    </td>
                    <td class="style3">
                        <asp:DropDownList ID="DropDownListTAXES_TYPE" runat="server" AutoPostBack="true"
                            Visible="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="style2">
                        所得機關
                    </td>
                    <td class="style3">
                        <asp:RadioButtonList ID="RadioButtonList_TAXES_AD" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="1" Selected="True">發薪機關</asp:ListItem>
                            <asp:ListItem Value="2">編制機關</asp:ListItem>
                            <asp:ListItem Value="3">現服機關</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="style2">
                        所得單位
                    </td>
                    <td class="style3">
                        <asp:RadioButtonList ID="RadioButtonList_TAXES_UNIT" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="1" Selected="True">編制單位</asp:ListItem>
                            <asp:ListItem Value="2">現服單位</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="height: 300px; overflow: scroll;">
                            <asp:GridView ID="GridView_SINGLEITEM" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSource_SINGLEITEM" GridLines="None"
                                OnRowCommand="GridView_SINGLEITEM_RowCommand" Width="770px" ForeColor="#333333"
                                OnRowDataBound="GridView_SINGLEITEM_RowDataBound">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="代碼" SortExpression="ID" />
                                    <asp:BoundField DataField="NAME" HeaderText="名稱" SortExpression="NAME" />
                                    <asp:BoundField DataField="TAXES_YESNO" HeaderText="是否納入所得" SortExpression="TAXES_YESNO" />
                                    <asp:BoundField DataField="TAXES_ID" HeaderText="所得格式" SortExpression="TAXES_ID" />
                                    <asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" Visible="false" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="SqlDataSource_SINGLEITEM" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background-color: #6699FF; color: White;">
                        <asp:Button ID="btTable" runat="server" OnClick="btTable_Click" Text="查詢" />
                        <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="新增" />
                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Visible="false" />
                        <asp:Button ID="btDelete" runat="server" OnClick="btDelete_Click" Text="刪除" Visible="false" />
                        <asp:Button ID="btExit" runat="server" OnClick="btExit_Click" Text="取消" />
                        <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
