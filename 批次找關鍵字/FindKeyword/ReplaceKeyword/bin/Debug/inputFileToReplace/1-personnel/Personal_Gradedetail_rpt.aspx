<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Gradedetail_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Gradedetail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 98px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        個人獎懲明細</div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="451px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style4" style="text-align: right">
                                    日&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 期：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_IDATE" runat="server"  MaxLength="7"
                                        Width="80px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_IDATE_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_IDATE">
                                    </cc1:FilteredTextBoxExtender>
                                    -<asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" 
                                        MaxLength="7" Width="80px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_IDATE1_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_IDATE1">
                                    </cc1:FilteredTextBoxExtender>
                                    (範例:0980101)
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_IDATE"
                                        Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4" style="text-align: right">
                                    姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" OnTextChanged="TextBox_MZ_NAME_TextChanged"
                                        Style="height: 19px" MaxLength="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4" style="text-align: right">
                                    身分證號：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="451px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                    OnRowCommand="GridView1_RowCommand" Width="100%" OnPageIndexChanging="GridView1_PageIndexChanging"
                    Visible="False">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                        <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                        <asp:BoundField HeaderText="現服機關" DataField="MZ_EXAD" />
                        <asp:BoundField HeaderText="現服單位" DataField="MZ_EXUNIT" />
                        <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" SelectText="列印" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
