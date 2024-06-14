<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Career_temp.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Career_temp" %>

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
        }
        .style14
        {
            text-align: left;
            width: 297px;
        }
        .style15
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 55px;
        }
        .style16
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 54px;
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
                        經歷檔暫存資料
                    </td>
                    <td style="text-align: right; font-size: large; font-family: 標楷體">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="1" style="width: 100%;">
                <tr>
                    <td class="style15">
                        身分證號
                    </td>
                    <td class="style14">
                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px"></asp:TextBox>
                    </td>
                    <td class="style16">
                        姓&nbsp;&nbsp;&nbsp; 名
                    </td>
                    <td class="style1">
                        <asp:Label ID="Label_MZ_NAME" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style15">
                        機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關
                    </td>
                    <td class="style14">
                        <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px"></asp:TextBox>
                        <asp:Label ID="Label_MZ_AD" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                    </td>
                    <td class="style16">
                        單&nbsp;&nbsp;&nbsp; 位
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" Font-Bold="True"></asp:TextBox>
                        <asp:Label ID="Label_MZ_UNIT" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style15">
                        職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 稱
                    </td>
                    <td class="style14">
                        <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="50px"></asp:TextBox>
                        <asp:Label ID="Label_MZ_OCCC" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                    </td>
                    <td class="style16">
                        官職等
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="TextBox_MZ_RANK" runat="server" Width="35px"></asp:TextBox>
                        <asp:Label ID="Label_MZ_RANK" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                        至
                        <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" Width="35px"></asp:TextBox>
                        <asp:Label ID="Label_MZ_RANK1" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td class="style34">
                            <asp:Button ID="btSearch" runat="server" CausesValidation="False" OnClick="btSearch_Click"
                                Text="查詢" Font-Bold="True" ForeColor="#000099" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" Font-Bold="True"
                                ForeColor="#000099" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" Font-Bold="True" ForeColor="#000099" />
                            <asp:Button ID="btInsert_all" runat="server" Text="匯入" OnClientClick="return confirm(&quot;確定匯入？&quot;);"
                                OnClick="btInsert_all_Click" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" GridLines="None" Width="100%"
                OnPageIndexChanging="GridView1_PageIndexChanging" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
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
