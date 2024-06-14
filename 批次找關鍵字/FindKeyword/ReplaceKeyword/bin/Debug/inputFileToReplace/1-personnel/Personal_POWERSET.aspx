<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_POWERSET.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_POWERSET" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style3
        {
            width: 94px;
        }
        .style5
        {
            text-align: left;
            width: 31px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style6
        {
            width: 54px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style7
        {
            width: 54px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="Label2" Text="權限管理" runat="server" Style="font-family: 標楷體; font-size: large"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" Style="font-family: 標楷體; font-size: large"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style6">
                            身分證號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style5">
                            姓 名
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style7">
                            編制機關
                        </td>
                        <td>
                            <asp:Label ID="Label_MZ_AD" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            現服機關
                        </td>
                        <td>
                            <asp:Label ID="Label_MZ_EXAD" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style7">
                            權限設定
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_MZ_POWER" runat="server">
                                <asp:ListItem>A</asp:ListItem>
                                <asp:ListItem>B</asp:ListItem>
                                <asp:ListItem>C</asp:ListItem>
                                <asp:ListItem Selected="True">D</asp:ListItem>
                                <asp:ListItem>E</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style7">
                            權限範本
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server" 
                                ondatabound="DropDownList1_DataBound">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" OnClientClick="return confirm(&quot;確定修改？&quot;);" CssClass="KEY_IN_BUTTON_BLUE"
                                AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                GridLines="None" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand"
                OnRowDataBound="GridView1_RowDataBound" Width="100%" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" />
                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" Visible="false" />
                    <asp:BoundField DataField="MZ_POWER" HeaderText="權限" />
                    <asp:BoundField DataField="MZ_ID_HIDE" HeaderText="身分證號"  />
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
