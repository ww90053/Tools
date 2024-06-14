<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_TBDV_temp.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_TBDV_temp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
        .style2
        {
            text-align: left;
            width: 250px;
        }
        .style5
        {
            text-align: left;
            width: 59px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style6
        {
            text-align: left;
            width: 57px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        AsyncPostBackTimeout="1000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td style="text-align: left; font-size: large; font-family: 標楷體">
                        職序暫存資料
                    </td>
                    <td style="text-align: right; font-size: large; font-family: 標楷體">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="1" style="width: 100%;">
                <tr>
                    <td class="style6">
                        身分證號
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px"></asp:TextBox>
                    </td>
                    <td class="style5">
                        姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                    </td>
                    <td class="style1">
                        <asp:Label ID="Label_MZ_NAME" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 序
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="75px"></asp:TextBox>
                        <asp:Label ID="Label_MZ_TBDV" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td class="style1">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <div class="style12">
                    <table style="background-color: #CCFFFF; color: White; width: 100%">
                        <tr>
                            <td class="style34">
                                <asp:Button ID="btSearch" runat="server" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE"
                                    meta:resourcekey="btP37_DLBASETableResource1" OnClick="btSearch_Click" Text="查詢" />
                                <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                    meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                                <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                                <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                    OnClick="btNEXT_Click" Text="下一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                                <asp:Button ID="btInsert_all" runat="server" Text="匯入" OnClientClick="return confirm(&quot;確定匯入？&quot;);"
                                    OnClick="btInsert_all_Click" CssClass="KEY_IN_BUTTON_RED" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999"
                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                OnPageIndexChanging="GridView1_PageIndexChanging">
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#DCDCDC" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
