<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYDAY.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYDAY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) 
        {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style10">
                <tr>
                    <td class="style8">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="style6" border="1">
                <tr>
                    <td class="style7">
                        勤務日期
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TextBox_DUTYDATE" runat="server" MaxLength="9" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="查看" OnClick="Button1_Click" class="style9" />&nbsp;
                        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="列印" class="style9" />
                    </td>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" GridLines="None" Width="100%"
                    OnRowCreated="GridView1_RowCreated" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
