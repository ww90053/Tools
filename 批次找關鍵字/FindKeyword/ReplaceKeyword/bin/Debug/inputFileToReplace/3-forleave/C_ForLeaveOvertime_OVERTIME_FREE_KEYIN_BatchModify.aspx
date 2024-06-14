<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_BatchModify.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_BatchModify" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style60
        {
            text-align: left;
            width: 351px;
        }
        .style63
        {
            width: 336px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style64
        {
            text-align: left;
        }
        .style65
        {
            width: 152px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style67
        {
            width: 207px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style68
        {
            width: 114px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
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
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" class="style56">
                        <tr>
                            <td class="style68">身分證號:</td>
                            <td class="style64">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" Font-Bold="True" Enabled="false" ForeColor="Maroon" MaxLength="10" Width="75px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style68">姓名:</td>
                            <td class="style64">
                                <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Font-Bold="True" Enabled="false" ForeColor="Maroon" MaxLength="6" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style67">發薪機關</td>
                            <td class="style64" colspan="3">
                                <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" 
                                    AppendDataBoundItems="True" AutoPostBack="True" class="style55" 
                                    DataSourceID="SqlDataSource_MZ_AD" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" Enabled="false"  BackColor="#EBEBE4">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_MZ_AD" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                                    DataSourceMode="DataReader" 
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td class="style67" style="color:Red;">原編制單位</td>
                            <td class="style64">
                                <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server" class="style55" Width="100px" Enabled="false" BackColor="#EBEBE4">
                                </asp:DropDownList>
                            </td>
                            <td class="style63" style="color:Red;">新編制單位</td>
                            <td class="style60">
                                <asp:DropDownList ID="DropDownList_MZ_UNIT_Update" runat="server" class="style55" Width="100px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style67">現服機關:</td>
                            <td class="style64" colspan="3">
                                <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataSource_MZ_EXAD" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" Enabled="false"
                                    class="style55"  BackColor="#EBEBE4">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownList_MZ_EXAD"
                                    Display="Dynamic" ErrorMessage="請選擇機關" class="style55"></asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource_MZ_EXAD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'" 
                                    DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td class="style67" style="color:Red;">原現服單位:</td>
                            <td class="style64">
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" class="style55" Enabled="false" BackColor="#EBEBE4">
                                </asp:DropDownList>
                            </td>
                            <td class="style63" style="color:Red;">新現服單位</td>
                            <td class="style60">
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT_Update" runat="server" Width="100px" class="style55">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="style67">修改年度月份:</td>
                            <td class="style64" colspan="3">
                                <asp:TextBox ID="TextBox_MZ_DUTYDATE" runat="server" Font-Bold="True" ForeColor="Maroon" MaxLength="6" Width="100px"></asp:TextBox>
                                （範例:09801）
                            </td>
                        </tr>
                    </table>

                    <asp:Button ID="btOK" runat="server" Text="確認整批修改" OnClick="btOK_Click" class="style53" OnClientClick="return confirm('確認執行?');" />
                    <asp:Button ID="btCancel" runat="server" Text="取消" class="style54" />
                    <br />
                    <span style="color:red;">注意，此功能為修改整個月份之資料，請確認後再進行使用</span>
                            
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
