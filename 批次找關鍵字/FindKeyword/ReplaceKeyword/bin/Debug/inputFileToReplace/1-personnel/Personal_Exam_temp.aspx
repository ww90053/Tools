<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Exam_temp.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Exam_temp" %>

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
            width: 330px;
        }
        .style5
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 54px;
        }
        .style6
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 55px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td style="text-align: left; font-size: large; font-family: 標楷體">
                        考試檔暫存資料
                    </td>
                    <td style="text-align: right; font-size: large; font-family: 標楷體">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="1" style="width: 100%;">
                <tr>
                    <td class="style5">
                        身分證號
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px"></asp:TextBox>
                    </td>
                    <td class="style6">
                        姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                    </td>
                    <td class="style1">
                        <asp:Label ID="Label_MZ_NAME" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        考試名稱
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="TextBox_EXAM_NAME" runat="server" Width="75px"></asp:TextBox>
                        <asp:Label ID="Label_EXAM_NAME" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                    </td>
                    <td class="style6">
                        考試類別
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="TextBox_EXAM_CLASS" runat="server" Width="75px"></asp:TextBox>
                        <asp:Label ID="Label_EXAM_CLASS" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        錄取等第
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="TextBox_EXAM_ADMISSION" runat="server" Width="50px"></asp:TextBox>
                        <asp:Label ID="Label_EXAM_ADMISSION" runat="server" CssClass="KEY_IN_LABLE"></asp:Label>
                    </td>
                    <td class="style6">
                        考試年度
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="50px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <div class="style12">
                    <table style="background-color: #CCFFFF; color: White; width: 100%">
                        <tr>
                            <td class="style34">
                                <asp:Button ID="btSearch" runat="server" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE"
                                    meta:resourcekey="btP37_DLBASETableResource1" OnClick="btSearch_Click" 
                                    Text="查詢" />
                                <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                    meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" 
                                    CssClass="KEY_IN_BUTTON_BLUE" />
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
