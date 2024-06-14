<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Othergrade_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Othergrade_rpt" %>

<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style5
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">
                調他機關名冊</div>
            <div>
            </div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="423px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style5">
                                    案號區間：</td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_NO1" runat="server" Width="100px" MaxLength="15"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="TextBox_MZ_NO1" Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                    &nbsp;至<b>&nbsp; </b>
                                    <asp:TextBox ID="TextBox_MZ_NO2" runat="server" Width="100px" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="style3">
                                <td class="style5">
                                    發布權責：</td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_SWT3" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="1">1.警察局權責</asp:ListItem>
                                        <asp:ListItem Value="2">2.警分局權責</asp:ListItem>
                                        <asp:ListItem Value="3" Selected="True">3.調他機關</asp:ListItem>
                                        <asp:ListItem Value="4">4.陳報警政署</asp:ListItem>
                                        <asp:ListItem Value="5">5.其他</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="423px">
                    <table width="100%">
                        <tr>
                            <td class="style2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                <asp:Button ID="btPrint1" runat="server" Text="列印word" OnClick="btPrint1_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
