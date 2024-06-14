<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_HolydaySet.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_HolydaySet" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
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
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style4">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            國定假日日期
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_HOLIDAY_DATE" runat="server" Width="80px" MaxLength="7"></asp:TextBox>
                            (範例：0980101)
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            國定假日名稱
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_HOLIDAY_NAME" runat="server" Width="500px" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" BackColor="#CCFFFF">
                <table class="style12">
                    <tr>
                        <td>
                            <asp:Button ID="btInsert" runat="server" CausesValidation="False" OnClick="btInsert_Click"
                                Text="新增" class="style9" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" CausesValidation="False" OnClick="btUpdate_Click"
                                Text="修改" Enabled="False" class="style9" />
                            <asp:Button ID="btOK" runat="server" CausesValidation="False" OnClick="btOK_Click"
                                Text="確定" Enabled="False" class="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" OnClick="btCancel_Click"
                                Text="取消" Enabled="False" class="style9" />
                            <asp:Button ID="btDelete" runat="server" CausesValidation="False" OnClick="btDelete_Click"
                                Text="刪除" OnClientClick="return confirm(&quot;確定刪除？&quot;);" Enabled="False"
                                class="style9" AccessKey="d" />
                        </td>
                    </tr>
                </table>
                <cc2:TBGridView ID="GridView1" runat="server" Width="100%" CellPadding="4" EnableEmptyContentRender="True"
                    GridLines="None" AutoGenerateColumns="False" DataKeyNames="MZ_HOLIDAY_DATE" DataSourceID="SqlDataSource1"
                    OnRowCommand="GridView1_RowCommand" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_HOLIDAY_DATE" HeaderText="國定假日日期" ReadOnly="True" SortExpression="MZ_HOLIDAY_DATE" />
                        <asp:BoundField DataField="MZ_HOLIDAY_NAME" HeaderText="國定假日名稱" SortExpression="MZ_HOLIDAY_NAME" />
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc2:TBGridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT * FROM C_DUTYHOLIDAY WHERE (CAST(dbo.SUBSTR(MZ_HOLIDAY_DATE,1,3) AS NUMBER)+1911)=EXTRACT(YEAR FROM GETDATE()) ORDER BY MZ_HOLIDAY_DATE">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
