<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveBasicSearch_New.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveBasicSearch_New" %>

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
            width: 79px;
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
                <asp:Panel ID="Panel2" runat="server" Width="100%">
                    <table class="style52" border="1" width="100%">
                        <tr>
                            <td class="style58">
                                姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                            </td>
                            <td class="style57">
                                <asp:TextBox ID="TextBox_NAME" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                身分證號:
                            </td>
                            <td class="style57">
                                <asp:TextBox ID="TextBox_ID" runat="server"  MaxLength="10" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關:
                            </td>
                            <td class="style57">
                                 <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AutoPostBack="True"  
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged">
                                </asp:DropDownList>
                               <%-- <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" class="style55">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    SelectCommand="(SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') UNION ALL (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '376411%') " 
                                    DataSourceMode="DataReader">
                                </asp:SqlDataSource>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位:
                            </td>
                            <td class="style57">
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" class="style55">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <table border="1" class="style52" width="100%">
                        <tr>
                            <td class="style58">
                                請假日期:
                            </td>
                            <td class="style57">
                                <asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" Width="65px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server">
                    <table class="style52" width="100%">
                        <tr>
                            <td style="text-align: left">
                                &nbsp;&nbsp;
                                <asp:Button ID="bt_DLBASE" runat="server" OnClick="Button_Click" Text="基本資料查詢" class="style53"
                                    Width="135px" />&nbsp;&nbsp;<asp:Button ID="bt_DLTB01" runat="server" Text="請假資料查詢"
                                        OnClick="Button_Click" class="style53" />
                                &nbsp;&nbsp;<asp:Button ID="Button2" runat="server" Text="離開" OnClick="Button2_Click"
                                    CausesValidation="false" class="style54" />
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
