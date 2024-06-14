<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_Ktype_Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_Ktype_Search" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 157px;
        }
        .style3
        {
            width: 83px;
        }
        .style4
        {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style1">
                    <tr>
                        <td class="style3">
                            代 碼
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_KCODE" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btKCODE_Search" runat="server" Text="過濾" 
                                OnClick="Button1_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            名 稱
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_KCHI" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btKCHI_Search" runat="server" Text="過濾" 
                                OnClick="btKCHI_Search_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
                <cc1:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataSourceID="SqlDataSource1"
                    EnableEmptyContentRender="True" GridLines="None" Width="100%" AllowPaging="True"
                    CssClass="style4" EmptyDataText="查無資料" OnDataBound="GridView1_DataBound" 
                    OnRowCommand="GridView1_RowCommand" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                            <ItemStyle Width="5%" />
                        </asp:CommandField>
                        <asp:BoundField DataField="MZ_KCODE" HeaderText="代碼" SortExpression="MZ_KCODE">
                            <ItemStyle Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_KCHI" HeaderText="名稱" SortExpression="MZ_KCHI">
                            <ItemStyle Width="75%" />
                        </asp:BoundField>
                    </Columns>
                    <PagerTemplate>
                        <asp:LinkButton ID="LinkFirst" runat="server" OnClick="Quick_Click" 
                            ForeColor="#33CC33">第一頁</asp:LinkButton>
                        <asp:LinkButton ID="LinkPrevious" runat="server" OnClick="Quick_Click" 
                            ForeColor="#33CC33">上一頁</asp:LinkButton>
                        <asp:LinkButton ID="LinkNext" runat="server" OnClick="Quick_Click" 
                            ForeColor="#33CC33">下一頁</asp:LinkButton>
                        <asp:LinkButton ID="LinkLast" runat="server" OnClick="Quick_Click" 
                            ForeColor="#33CC33">最後頁</asp:LinkButton>
                        <asp:DropDownList ID="ddlPageJump" runat="server" OnSelectedIndexChanged="ddlPageJump_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        /<asp:Label ID="lbAllPage" runat="server" ForeColor="#33CC33"></asp:Label>
                    </PagerTemplate>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
