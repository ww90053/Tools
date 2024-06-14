<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal4-2.aspx.cs" Inherits="TPPDDB._1_personnel.WebForm4" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
        }
        .style9
        {
            text-align: center;
        }
        .style38
        {
        }
        .style39
        {
            text-align: right;
        }
        .style40
        {
            text-align: left;
            font-size: large;
            font-family: 標楷體;
        }
        .style41
        {
            font-size: large;
            font-family: 標楷體;
        }
        .style42
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 54px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function CheckUp() {
            var vMode = document.getElementById('<%=hfd_Mode.ClientID%>');
            if (vMode != null && vMode.value == 'UPDATE') {
                if (!confirm("修改代碼可能會造成系統異常，確定修改?")) {
                    return false;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td class="style40">
                        人事代碼作業
                    </td>
                    <td class="style39">
                        <asp:Label ID="Label1" runat="server" CssClass="style41" TabIndex="-1"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_KTYPE" runat="server">
                <table border="1" class="style1">
                    <tr>
                        <td class="style42">
                            序&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_KTYPE" runat="server" Style="text-align: left" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style42" style="text-align: left">
                            代&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 碼
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_KCODE" runat="server" Style="text-align: left" Width="100px"
                                AutoPostBack="True" OnTextChanged="TextBox_MZ_KCODE_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style42">
                            中文名稱
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_KCHI" runat="server" Style="text-align: left" Width="450px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style42" style="text-align: left">
                            備&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 註
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_KFIL" runat="server" Style="text-align: left" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField runat="server" ID="hfd_Mode" />
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td class="style9">
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" 
                                AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" Enabled="False" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" OnClientClick="return CheckUp();" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" CssClass="KEY_IN_BUTTON_BLUE" 
                                Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"
                                TabIndex="-1"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="說明" 
                                CssClass="KEY_IN_BUTTON_RED" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:TBGridView ID="GridView1" runat="server" CellPadding="4" EnableEmptyContentRender="True"
                GridLines="None" Width="100%" AllowPaging="True" AutoGenerateColumns="False"
                EmptyDataText="無資料" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand"
                OnRowDataBound="GridView1_RowDataBound" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_KTYPE" HeaderText="序號" SortExpression="MZ_KTYPE" />
                    <asp:BoundField DataField="MZ_KCODE" HeaderText="代碼" SortExpression="MZ_KCODE" />
                    <asp:BoundField DataField="MZ_KCHI" HeaderText="中文說明" SortExpression="MZ_KCHI" />
                    <asp:BoundField DataField="MZ_KFIL" HeaderText="備註" SortExpression="MZ_KFIL" />
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </cc1:TBGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
