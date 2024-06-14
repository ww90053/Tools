<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_insideToNouse.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_insideToNouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style10">
                <tr>
                    <td class="style8">
                        差勤基本資料
                    </td>
                    <td class="style4">
                        <asp:Label ID="Label1" runat="server" CssClass="style10"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr>
                    <td>
                        機關
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        單位
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        姓名
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        身分證
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        期別
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="2">
                            <asp:ListItem Value="6" Text="上半年"></asp:ListItem>
                            <asp:ListItem Value="12" Text="下半年"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical">
                <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" GridLines="Vertical">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="加班日期" DataField="MZ_DATE" />
                        <asp:BoundField HeaderText="事由" DataField="OTREASON" />
                        <asp:BoundField HeaderText="時數" DataField="OTIME" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
