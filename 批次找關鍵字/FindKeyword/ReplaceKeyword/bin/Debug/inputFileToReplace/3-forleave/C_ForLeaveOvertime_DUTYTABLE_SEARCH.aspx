<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_DUTYTABLE_SEARCH.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYTABLE_SEARCH" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style58
        {
            width: 75px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style111
        {
            border: solid 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" width="100%">
                    <table width="100%" border="1">
                        <tr>
                            <td class="style58">
                                勤務日期:
                            </td>
                            <td class="style57">
                                <asp:TextBox ID="TextBox_DUTYDATE" runat="server" Height="20px" MaxLength="9" Width="80px"
                                    OnTextChanged="TextBox_DUTYDATE_TextChanged" Font-Bold="True" 
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                勤務項目:
                            </td>
                            <td class="style57">
                                <asp:TextBox ID="TextBox_DUTYMODE" runat="server" AutoPostBack="True" MaxLength="4"
                                    OnTextChanged="TextBox_DUTYMODE_TextChanged" Style="height: 19px" 
                                    Width="42px" Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                                <asp:Button ID="btDUTYMODE" runat="server" CausesValidation="False" OnClick="btDUTYMODE_Click"
                                    TabIndex="-1" Text="V" />
                                <asp:TextBox ID="TextBox_DUTYMODE1" runat="server" TabIndex="-1" Width="400px" 
                                    CssClass="style111" ForeColor="#0033FF"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table  width="100%" border="1" align="center" >
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" class="style53" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button2" runat="server" Text="離開" OnClick="Button2_Click" class="style54" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
