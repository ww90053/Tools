<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal4-1.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style6
        {
            text-align: left;
            font-size: large;
            font-family: 標楷體;
        }
        .style7
        {
            text-align: right;
            font-size: large;
            font-family: 標楷體;
        }
        .style8
        {
            text-align: right;
        }
        .style9
        {
            text-align: center;
        }
        .style11
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 55px;
        }
        .style13
        {
            text-align: left;
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
    <table style="background-color: #6699FF; color: White; width: 100%">
        <tr>
            <td class="style6">
                核定機關設定
            </td>
            <td class="style8">
                <asp:Label ID="Label1" runat="server" CssClass="style7"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_CHKAD" Width="100%" runat="server">
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style11">
                            機關代碼
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"></asp:TextBox>
                            <asp:CustomValidator ID="CV_AD" runat="server" ControlToValidate="TextBox_MZ_AD"
                                OnServerValidate="CV_AD_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <td class="style11">
                            機關名稱
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="200px" AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">
                            主管職稱
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_MASTER_POS" runat="server"></asp:TextBox>
                        </td>
                        <td class="style11">
                            主管姓名
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_MASTER_NAME" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">
                            發&nbsp; 文&nbsp; 字
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_PRID" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td class="style11">
                            電 話
                        </td>
                        <td>
                            &nbsp;<asp:TextBox ID="TextBox_MZ_FAXNO" runat="server" Width="100px"></asp:TextBox>
                            <cc11:FilteredTextBoxExtender ID="TextBox_MZ_FAXNO_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_MZ_FAXNO"
                                ValidChars="-">
                            </cc11:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            地&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 址
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_ADDRESS" runat="server" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style11">
                            副&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 本
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_CC_NO" runat="server"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            機密級
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_PWD_NO" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td style="text-align: left" class="style13">
                            速 級
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_SPEED_NO" runat="server" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">
                            權 責 
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DropDownList1" runat="server" BackColor="#EAEAEA" 
                                Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            檔號</td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_FILENO" runat="server" MaxLength="12"></asp:TextBox>
                        </td>
                        <td class="style13" style="text-align: left">
                            使用年限</td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_MZ_YEARUSE" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                            <cc11:FilteredTextBoxExtender ID="TextBox_MZ_YEARUSE_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="TextBox_MZ_YEARUSE" 
                                FilterType="Numbers">
                            </cc11:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="color: White; width: 100%">
                    <tr bgcolor="#CCFFFF">
                        <td class="style9">
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CssClass="KEY_IN_BUTTON_BLUE" CausesValidation="False" 
                                AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btok" runat="server" Enabled="False" OnClick="btok_Click" Text="確認"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                                Text="取消" Enabled="False" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" DataSourceID="SqlDataSource1" GridLines="None" Style="text-align: left"
                    OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_AD" HeaderText="機關代碼" SortExpression="MZ_AD">
                            <ItemStyle Width="75px" Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_NAME" HeaderText="機關名稱" SortExpression="MZ_NAME">
                            <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_MASTER_NAME" HeaderText="主管姓名" SortExpression="MZ_MASTER_NAME">
                            <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_MASTER_POS" HeaderText="主管職稱" SortExpression="MZ_MASTER_POS">
                            <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_PRID" HeaderText="發文字" SortExpression="MZ_PRID">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_ADDRESS" HeaderText="地址" SortExpression="MZ_ADDRESS">
                            <ItemStyle Width="320px" />
                        </asp:BoundField>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_CHKAD.MZ_AD) AS MZ_NAME, MZ_MASTER_NAME, MZ_PRID, MZ_MASTER_POS,MZ_FAXNO,MZ_ADDRESS,PWD_NO,SPEED_NO, CC_NO  FROM A_CHKAD ORDER BY MZ_AD">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
