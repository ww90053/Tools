<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Gradesuglist_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Gradesuglist_rpt" %>

<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        AsyncPostBackTimeout="10000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">
                獎懲建議名冊</div>
            <div>
            </div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="530px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td align="right">
                                    案號(流水號)：
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_NO1" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_NO1"
                                        Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                    &nbsp;<b>─&nbsp; </b>
                                    <asp:TextBox ID="TextBox_MZ_NO2" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    機關：
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_EXAD" runat="server" AutoPostBack="True" 
                                        AppendDataBoundItems="True" 
                                        onselectedindexchanged="DropDownList_EXAD_SelectedIndexChanged">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>                                 
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    單位：
                                </td>
                                <td style="text-align: left"> 
                                    <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" AutoPostBack="True" Width="150px" 
                                        ondatabound="DropDownList_EXUNIT_DataBound">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>                                 
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    權責單位：
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_SWT3" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="1">1.警察局權責</asp:ListItem>
                                        <asp:ListItem Value="2">2.警分局權責</asp:ListItem>
                                        <asp:ListItem Value="3">3.調他機關</asp:ListItem>
                                        <asp:ListItem Value="4">4.陳報警政署</asp:ListItem>
                                        <asp:ListItem Value="5">5.其他</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="535px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btn_bysn" runat="server" Text="列印(依流水號)" OnClick="btn_bysn_Click" />
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
