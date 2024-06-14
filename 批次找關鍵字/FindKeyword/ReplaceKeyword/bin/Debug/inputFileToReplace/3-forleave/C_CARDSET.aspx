<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_CARDSET.aspx.cs" Inherits="TPPDDB._3_forleave.C_CARDSET" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style58
        {
            color: #FF0000;
            font-weight: bold;
            background-color: #FFFF99;
            text-align: left;
            width: 55px;
        }
        .style59
        {
            text-align: left;
            width: 55px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style60
        {
            text-align: left;
            width: 55px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style61
        {
            text-align: left;
            width: 24px;
            height: 20px;
            background-color: #FFCCFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="Label2" Text="刷卡管理" runat="server" class="style8"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" class="style8"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style5">
                            身分證號
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style1">
                            姓名
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table  class="style6" border="1">
                    <tr>
                        <td class="style1">
                            編制機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_AD" runat="server" Font-Bold="True" ForeColor="#0033FF"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            現服機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_EXAD" runat="server" Font-Bold="True" 
                                ForeColor="#0033FF"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            刷卡設定
                        </td>
                        <td class="style3">
                            <asp:CheckBox ID="CheckBox_MZ_CLOCK" runat="server" Text="上下班刷卡" />　
                            <asp:CheckBox ID="CheckBox_MZ_OPENDOOR" runat="server" Text="門禁刷卡" />　
                            <asp:CheckBox ID="CheckBox_MZ_OVERTIME" runat="server" Text="加班刷卡" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" class="style9"/>
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CausesValidation="False" class="style9" AccessKey="a"/>
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" class="style9"/>
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" class="style9" AccessKey="s"/>
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Enabled="False" class="style9"/>
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" class="style9" AccessKey="d"/>
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" class="style9"/>
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" class="style9"/>
                            <asp:Button ID="btGroup_Update" runat="server" OnClick="btGroup_Update_Click" Text="整批修改" class="style13"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
 <asp:Panel ID="Panel2" runat="server">
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" GridLines="None" Width="100%"
                AutoGenerateColumns="False" DataKeyNames="MZ_ID" AllowPaging="True" 
                OnPageIndexChanging="GridView1_PageIndexChanging" 
                onrowcommand="GridView1_RowCommand" 
                onrowdatabound="GridView1_RowDataBound" ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" ReadOnly="True" SortExpression="MZ_ID" />
                    <asp:BoundField DataField="NAME" HeaderText="姓名" SortExpression="NAME" />
                    <asp:TemplateField HeaderText="上下班刷卡" SortExpression="MZ_CLOCK">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MZ_CLOCK") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox_GV_CLOCK" runat="server" AutoPostBack="true" Checked='<%# Eval("MZ_CLOCK").ToString()=="Y"?true:false %>'
                                OnCheckedChanged="CheckBox_GV_CLOCK_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="門禁刷卡" SortExpression="MZ_OPENDOOR">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("MZ_OPENDOOR") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox_GV_OPENDOOR" runat="server" AutoPostBack="true" Checked='<%# Eval("MZ_OPENDOOR").ToString()=="Y"?true:false %>'
                                OnCheckedChanged="CheckBox_GV_OPENDOOR_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="加班刷卡" SortExpression="MZ_OVERTIME">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("MZ_OVERTIME") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox_GV_OVERTIME" runat="server" AutoPostBack="true" Checked='<%# Eval("MZ_OVERTIME").ToString()=="Y"?true:false %>'
                                OnCheckedChanged="CheckBox_GV_OVERTIME_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                                                <asp:ButtonField CommandName="SELECT" Text="按鈕" />

                </Columns>
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
           <%-- <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_CARDSET.MZ_ID) AS NAME,MZ_CLOCK,MZ_OPENDOOR,MZ_OVERTIME FROM C_CARDSET WHERE 1<>1">
            </asp:SqlDataSource>--%>
 </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
