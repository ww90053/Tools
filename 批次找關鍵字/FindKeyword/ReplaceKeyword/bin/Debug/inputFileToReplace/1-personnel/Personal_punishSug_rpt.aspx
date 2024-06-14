<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_punishSug_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_punishSug_rpt" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div  class="title_s1">
        獎 懲 建 議 函
    </div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="450px">
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                發文字第 ：
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" >
                                </asp:DropDownList>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                發文文號：
                            </td>
                            <td align="left">
                                <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" CausesValidation="True" 
                                    MaxLength="10" Width="100px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True" FilterType="Numbers" 
                                    TargetControlID="TextBox_MZ_PRID1">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_PRID1"
                                    Display="Dynamic" ErrorMessage="發文文號不可空白"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                職　　等；
                            </td>
                            <td align="left">
                                <asp:ListBox ID="ListBox_MZ_SRANK" runat="server" Rows="1">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="P9G23">警正二階(含)以上</asp:ListItem>
                                    <asp:ListItem Value="P8G22">警正二階以下</asp:ListItem>
                                     <asp:ListItem Value="G3">警監</asp:ListItem>
                                </asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="450px">
                    <table width="100%">
                        <tr>
                            <td class="style2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        </td>
    </div>
</asp:Content>
