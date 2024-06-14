<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_SEARCH.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_SEARCH" %>

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
        .style58
        {
            width: 73px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style59
        {
            text-align: left;
            width: 72px;
        }
        .style60
        {
            text-align: left;
            width: 351px;
        }
        .style61
        {
            width: 78px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style62
        {
            width: 86px;
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
                            <td class="style61">
                                身分證號:
                            </td>
                            <td class="style60">
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="75px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style61">
                                姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                            </td>
                            <td class="style60">
                                <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="6" Width="100px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style61">
                                機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關:
                            </td>
                            <td class="style60">
                                 <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AutoPostBack="True"  
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged">
                                </asp:DropDownList>
                               <%-- <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" class="style55">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownList_MZ_EXAD"
                                    Display="Dynamic" ErrorMessage="請選擇機關" class="style55"></asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'" 
                                    DataSourceMode="DataReader">
                                </asp:SqlDataSource>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style61">
                                單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位:
                            </td>
                            <td class="style60">
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" class="style55">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style61">
                                年度月份:
                            </td>
                            <td class="style60">
                                <asp:TextBox ID="TextBox_MZ_DUTYDATE" runat="server" Font-Bold="True" 
                                    ForeColor="Maroon" MaxLength="6" Width="100px"></asp:TextBox>
                                    （範例:09801）
                            </td>
                        </tr>
                    </table>
                    <%--<table border="1" class="style56">
                        <tr>
                            <td class="style61">
                                日期起：
                            </td>
                            <td class="style59">
                                <asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" MaxLength="9" Width="65px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                            <td class="style58">
                                日期迄：
                            </td>
                            <td class="style57">
                                <asp:TextBox ID="TextBox_MZ_IDATE2" runat="server" MaxLength="9" Width="65px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                                (範例：0990101)
                            </td>
                        </tr>
                    </table>--%>

                    <table class="style52">
                        <tr>
                            <td>
                                <asp:Button ID="btOK" runat="server" Text="確認" OnClick="btOK_Click" class="style53" />
                                <asp:Button ID="btCancel" runat="server" Text="取消" class="style54" />
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
