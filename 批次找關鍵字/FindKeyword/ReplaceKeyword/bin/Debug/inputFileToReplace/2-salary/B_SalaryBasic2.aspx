<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic2.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            text-align: right;
            width: 14%;
            background-color: #BBFFBB;
        }
        .style3
        {
            text-align: left;
            width: 36%;
            background-color: #BBFFBB;
        }
        .styleTitle
        {
            font-size: 16pt;
            font-family: 標楷體;
        }
        .style4
        {
            color: #FF0000;
        }
        .styleBankH
        {
            text-align: right;
            width: 14%;
            background-color: #FFFF99;
        }
        .styleBankD
        {
            text-align: left;
            width: 36%;
            background-color: #FFFF99;
        }
        .styleTable
        {
            border: 1;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="jsUpdateProgress.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>';

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <div class="PageTitle">
        分局行庫設定</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                Width="200px" Height="100px" Style="display: none;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="300">
                    <ProgressTemplate>
                        <div style="position: relative; top: 40%; text-align: center;">
                            <img src="/images/loading.gif" style="vertical-align: middle" alt="Processing" />
                            處理中 ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <table border="1" width="100%">
                <tr>
                    <td>
                        <div>
                            <table class="styleTable">
                                <tr>
                                    <td colspan="4" style="font-weight: bold; font-size: medium; background-color: #00FF22;">
                                        分局資料
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        分局名稱
                                    </td>
                                    <td class="style3">
                                        <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCODEMZ_KCHI"
                                            DataValueField="MZ_KCODE">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style2">
                                        分局IP
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_IP" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        稽徵機關
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_TAXUNIT" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                    </td>
                                    <td class="style2">
                                        所得稅地址
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_TAXADDR" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        所得稅統一編號
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_TAXINVOICE" runat="server" MaxLength="12" Width="110px"></asp:TextBox>
                                    </td>
                                    <td class="style2">
                                        所得稅名稱
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_TAXNAME" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        所得稅扣繳義務人
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_TAXPERS" runat="server" MaxLength="12" Width="70px"></asp:TextBox>
                                    </td>
                                    <td class="style2">
                                        所得稅製單編號
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_TICKETNUM" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divBank" runat="server">
                            <table class="styleTable">
                                <tr>
                                    <td colspan="4" style="font-weight: bold; font-size: medium; background-color: #FFFF00;">
                                        銀行資料
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleBankH">
                                        群組
                                    </td>
                                    <td class="styleBankD" colspan="3">
                                        <asp:DropDownList ID="ddl_group" runat="server">
                                            <asp:ListItem Selected="True" Value="1">優惠存款</asp:ListItem>
                                            <asp:ListItem Value="2">薪資轉帳</asp:ListItem>
                                            <asp:ListItem Value="3">國宅貸款</asp:ListItem>
                                            <asp:ListItem Value="4">退撫金貸款</asp:ListItem>
                                            <asp:ListItem Value="5">分期付款</asp:ListItem>
                                            <asp:ListItem Value="6">銀行代款</asp:ListItem>
                                            <asp:ListItem Value="7">法院扣款</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleBankH">
                                        <span class="style4">*</span>銀行
                                    </td>
                                    <td class="styleBankD">
                                        <asp:DropDownList ID="DropDownList_BANK" runat="server" DataSourceID="SqlDataSource_BANK_LIST"
                                            DataTextField="IDNAME" DataValueField="BANK_ID" AutoPostBack="true" OnSelectedIndexChanged="DropDownList_BANK_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_BANK_LIST" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT BANK_ID, BANK_NAME, BANK_ID +'('+ BANK_NAME +')' AS IDNAME FROM B_BANK_LIST ORDER BY BANK_ID">
                                        </asp:SqlDataSource>
                                    </td>
                                    <td class="styleBankH">
                                        <span class="style4">*</span>銀行分行
                                    </td>
                                    <td class="styleBankD">
                                        <asp:DropDownList ID="DropDownList_BankBranch" runat="server" OnSelectedIndexChanged="DropDownList_BankBranch_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleBankH">
                                        委託代號
                                    </td>
                                    <td class="styleBankD">
                                        <asp:TextBox ID="TextBox_FIANCENO" runat="server" MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td class="styleBankH">
                                        轉帳帳號
                                    </td>
                                    <td class="styleBankD">
                                        <asp:TextBox ID="TextBox_FIANCENO1" runat="server" MaxLength="14"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="styleBankH">
                                        電子郵件
                                    </td>
                                    <td class="styleBankD">
                                        <asp:TextBox ID="txt_email" runat="server" MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td class="styleBankH">
                                       
                                    </td>
                                    <td class="styleBankD">
                                      
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 785px; height: 210px; overflow: scroll; text-align: center;">
                            <asp:GridView ID="GridView_BRANCH" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataSourceID="SqlDataSource_BRANCH" GridLines="None" DataKeyNames="B_SNID" OnRowCommand="GridView_BRANCH_RowCommand"
                                ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:ButtonField CommandName="btSelect" HeaderText="細節" Text="細節" ItemStyle-Wrap="False"
                                        HeaderStyle-Wrap="False">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ID" HeaderText="分局名稱" SortExpression="ID" ItemStyle-Wrap="False"
                                        HeaderStyle-Wrap="False">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IP" HeaderText="分局IP" ItemStyle-Wrap="False">
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TAXUNIT" HeaderText="稽徵機關" ItemStyle-Wrap="False" HeaderStyle-Wrap="False">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TAXINVOICE" HeaderText="所得稅統一編號" ItemStyle-Wrap="False"
                                        HeaderStyle-Wrap="False">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TAXNAME" HeaderText="所得稅名稱" ItemStyle-Wrap="False">
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TAXADDR" HeaderText="所得稅地址" ItemStyle-Wrap="False">
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TAXPERS" HeaderText="所得稅扣繳義務人" ItemStyle-Wrap="False"
                                        HeaderStyle-Wrap="False">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TICKETNUM" HeaderText="所得稅製單編號" ItemStyle-Wrap="False"
                                        HeaderStyle-Wrap="False">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <asp:GridView ID="GridView_BANK_DATA" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" DataKeyNames="BB_SNID" DataSourceID="SqlDataSource_BANK_DATA"
                                GridLines="None" OnRowCommand="GridView_BANK_DATA_RowCommand" CssClass="Grid1">
                                <Columns>
                                    <asp:BoundField DataField="BANK_ID" HeaderText="銀行" SortExpression="BANK_ID" />
                                    <asp:BoundField DataField="BANK_NAME" HeaderText="分行" SortExpression="BANK_NAME" />
                                    <asp:BoundField DataField="FIANCENO" HeaderText="委託代號" SortExpression="FIANCENO" />
                                    <asp:BoundField DataField="FIANCENO1" HeaderText="帳號" SortExpression="FIANCENO1" />
                                    <asp:BoundField DataField="GROUPNAME" HeaderText="群組" SortExpression="GROUP" />
                                     <asp:BoundField DataField="MAIL" HeaderText="電子郵件" SortExpression="MAIL" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="選取" CommandName="btSelect" CommandArgument='<%# Eval("BB_SNID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClientClick="return confirm('確定要刪除這筆資料？');"
                                                CommandName="del" CommandArgument='<%# Eval("BB_SNID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource_BRANCH" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSource_BANK_DATA" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>"></asp:SqlDataSource>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: #6699FF; color: White; text-align: center;">
                        <asp:Button ID="btCreate" runat="server" Text="建立分局資料" OnClick="btCreate_Click" />
                        <asp:Button ID="btUpdate" runat="server" Text="修改分局資料" OnClick="btUpdate_Click" />
                        <asp:Button ID="btExit" runat="server" Text="取消" OnClick="btExit_Click" />
                        <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
