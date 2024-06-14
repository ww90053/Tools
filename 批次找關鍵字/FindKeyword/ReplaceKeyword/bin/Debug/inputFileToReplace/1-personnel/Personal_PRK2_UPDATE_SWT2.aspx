<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_PRK2_UPDATE_SWT2.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_PRK2_UPDATE_SWT2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="Panel1" runat="server" Width="552px">
                    <table  border="1" width="400px">
                        <tr>
                            <td class="SEARCH_TITLE">
                                發文文號：
                            </td>
                            <td >
                                <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" CssClass="SEARCH_DROPDOWNLIST"
                                    >
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="SEARCH_TITLE">
                                發文字號：
                            </td>
                            <td >
                                <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" CssClass="SEARCH_DROPDOWNLIST"
                                    ForeColor="Maroon" Width="75px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="SEARCH_TITLE">
                                整批審核：
                            </td>
                            <td >
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatColumns="2" CssClass="SEARCH_DROPDOWNLIST">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="2">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button1" runat="server" CssClass="SEARCH_BUTTON_BLUE" Text="確認" OnClick="Button1_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button3" runat="server" CssClass="SEARCH_BUTTON_BLACK" OnClick="Button3_Click"
                                    Text="離開" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
