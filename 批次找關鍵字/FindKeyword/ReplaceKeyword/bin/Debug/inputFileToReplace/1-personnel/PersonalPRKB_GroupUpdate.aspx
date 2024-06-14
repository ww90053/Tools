<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalPRKB_GroupUpdate.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_PRKB_GroupUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style3
        {
            color: #FFFFFF;
            font-size: small;
        }
        .style6
        {
            width: 120px;
        }
        .style111
        {
            border-style: solid;
            border-color: inherit;
            border-width: 0px;
            font-size: small;
        }
        .style110
        {
            width: 23px;
            font-size: small;
        }
        .style113
        {
            text-align: left;
            width: 60px;
            height: 20px;
            background-color: #FFCCFF;
            font-size: small;
        }
        .style114
        {
            width: 411px;
        }
        .style115
        {
            font-size: small;
        }
        .style116
        {
            font-weight: bold;
            color: #000099;
            text-align: left;
            font-size: small;
        }
        .style117
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 60px;
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" style="width: 100%; text-align: left">
                        <tr>
                            <td class="style113">
                                案&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_NO" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_NO_TextChanged"
                                    MaxLength="12" Width="120px" CssClass="style115"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="style3"
                                    ForeColor="White" ControlToValidate="TextBox_MZ_NO" ErrorMessage="不可空白" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style113">
                                機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_AD" runat="server" AutoPostBack="True" CssClass="style133"
                                    MaxLength="10" OnTextChanged="TextBox_MZ_AD_TextChanged" Width="75px" 
                                    style="font-size: small"></asp:TextBox>
                                <asp:Button ID="btAD" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btAD_Click" TabIndex="-1" Text="V" />
                                <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" TabIndex="-1"
                                    Width="200px" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style113">
                                職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 稱</td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" AutoPostBack="True" MaxLength="4"
                                    OnTextChanged="TextBox_MZ_OCCC_TextChanged" Width="35px" 
                                    CssClass="style115"></asp:TextBox>
                                <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btOCCC_Click" TabIndex="-1" Text="V" />
                                <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" TabIndex="-1"
                                    Width="100px" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style113">
                                獎懲結果
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_PRRST" runat="server" AutoPostBack="True" MaxLength="4"
                                    OnTextChanged="TextBox_MZ_PRRST_TextChanged" Width="35px" 
                                    CssClass="style115"></asp:TextBox>
                                <asp:Button ID="btPRRST" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btPRRST_Click" TabIndex="-1" Text="V" />
                                <asp:TextBox ID="TextBox_MZ_PRRST1" runat="server" CssClass="style111" TabIndex="-1"
                                    Width="200px" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_MZ_PRRST"
                                    CssClass="style3" ErrorMessage="不可空白" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                <span class="style115">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </span>
                                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="查看" 
                                    CssClass="style115" Font-Bold="True" ForeColor="Maroon" />
                            </td>
                        </tr>
                    </table>
                    <table border="1" style="width: 100%; text-align: left">
                        <tr>
                            <td class="style117">
                                獎懲內容
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_PRCT" runat="server" MaxLength="50" Width="600px" 
                                    CssClass="style115"></asp:TextBox>
                                <asp:Button ID="btPRCT" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btPRCT_Click" TabIndex="-1" Text="V" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style117">
                                獎懲依據
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_MZ_PROLNO" runat="server" AutoPostBack="True" MaxLength="8"
                                    OnTextChanged="TextBox_MZ_PROLNO_TextChanged" Width="70px" 
                                    CssClass="style115"></asp:TextBox>
                                <asp:Button ID="btPROLNO" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btPROLNO_Click" TabIndex="-1" Text="V" />
                                <asp:TextBox ID="TextBox_MZ_PROLNO1" runat="server" CssClass="style111" TabIndex="-1"
                                    Width="200px" Font-Bold="True" ForeColor="#0033FF"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table border="1" style="width: 100%; text-align: left">
                        <tr>
                            <td class="style117">
                                是否配分
                            </td>
                            <td class="style6">
                                <asp:DropDownList ID="DropDownList_MZ_PCODE" runat="server" AppendDataBoundItems="True"
                                    OnSelectedIndexChanged="DropDownList_MZ_PCODE_SelectedIndexChanged" 
                                    CssClass="style115">
                                    <asp:ListItem Value="1">1.配分</asp:ListItem>
                                    <asp:ListItem Value="9" Selected="True">9.不配分</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style117">
                                配分款項
                            </td>
                            <td class="style114">
                                <asp:TextBox ID="TextBox_MZ_PCODEM" runat="server" AutoPostBack="True" MaxLength="2"
                                    OnTextChanged="TextBox_MZ_PCODEM_TextChanged" Width="35px" 
                                    CssClass="style115"></asp:TextBox>
                                <asp:Button ID="btPCODEM" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btPCODEM_Click" TabIndex="-1" Text="V" />
                                <asp:TextBox ID="TextBox_MZ_PCODEM1" runat="server" CssClass="style111" TabIndex="-1"
                                    Width="220px" Font-Bold="True" ForeColor="#0033FF" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="確認" OnClick="Button1_Click" 
                                    CssClass="style116" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" GridLines="None"
                        PageSize="5" Visible="False" Width="100%" ForeColor="#333333" 
                        CssClass="style115">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="MZ_NO" HeaderText="案號" />
                            <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                            <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" />
                            <asp:BoundField DataField="MZ_PRRST" HeaderText="獎懲內容" />
                        </Columns>
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM A_PRKB">
                    </asp:SqlDataSource>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
