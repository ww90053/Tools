<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_Search.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_Search" %>

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
            width: 81px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
 <asp:Panel ID="Panel1" runat="server">
            <table class="style56">
                <tr>
                    <td class="style58">
                        姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:</td>
                    <td class="style57">
                        <asp:TextBox ID="TextBox_NAME" runat="server" Font-Bold="True" 
                            ForeColor="Maroon" MaxLength="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style58">
                        身分證號:</td>
                    <td class="style57">
                        <asp:TextBox ID="TextBox_ID" runat="server" Font-Bold="True" ForeColor="Maroon" 
                            MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style58">
                        機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關:</td>
                    <td class="style57">
                        <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                            AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                            OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" class="style55">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                            SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'" 
                            DataSourceMode="DataReader">
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td class="style58">
                        單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位:</td>
                    <td class="style57">
                        <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" class="style55">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td class="style58">
                    輪值月份:</td>
                <td class="style57">
                    <asp:TextBox ID="TextBox_MZ_MONTH" runat="server"  MaxLength ="5" 
                        Font-Bold="True" ForeColor="Maroon" ></asp:TextBox>
                    <asp:Label ID="Label_MZ_MONTH" runat="server" Font-Bold="True" 
                        ForeColor="#0033FF"></asp:Label>
                </td>
                
                </tr>
            </table>
            <table class="style52">
                <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" class="style53" />　　
                        <asp:Button ID="Button2" runat="server" Text="離開" OnClick="Button2_Click" class="style54" />
                    </td>
                </tr>
            </table>
  </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
