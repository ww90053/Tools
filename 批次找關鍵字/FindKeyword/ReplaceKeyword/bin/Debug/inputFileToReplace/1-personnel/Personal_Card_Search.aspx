<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_Card_Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal_Card_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 100px;
            font-weight: 700;
            color: #0033CC;
            text-align: justify;
            text-align-last: justify;
            background-color: #99FFCC;
        }
        .style4
        {
            text-align: left;
        }
        .style5
        {
            border: solid 0px;
        }
        .style6
        {
            text-align: center;
        }
    </style>

    <script type="text/javascript">
        function leave() {
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="style1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                        <table border="1" width="400px">
                            <tr>
                                <td class="style2">
                                    機 關：
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                                        DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True" OnDataBound="DropDownList_AD_DataBound"
                                        Font-Bold="True" ForeColor="Maroon">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')" DataSourceMode="DataReader">
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    單 位：
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource_UNIT"
                                        DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)" OnDataBound="DropDownList_UNIT_DataBound"
                                        Font-Bold="True" ForeColor="Maroon">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    服 務 證 號：
                                </td>
                                <td class="style4">
                                    <asp:TextBox ID="TextBox_MZ_IDNO" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    姓 名：
                                </td>
                                <td class="style4">
                                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Font-Bold="True" ForeColor="Maroon"
                                        MaxLength="6" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    職 稱：
                                </td>
                                <td class="style4">
                                    <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" AutoPostBack="True" MaxLength="4"
                                        OnTextChanged="TextBox_MZ_OCCC_TextChanged" Width="35px" Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                                    <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                        OnClick="btOCCC_Click" TabIndex="-1" Text="V" />
                                    <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style5" TabIndex="-1"
                                        Width="90px" Font-Bold="True" ForeColor="#99FFCC"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    是否已列印：
                                </td>
                                <td class="style4">
                                    <asp:DropDownList ID="DropDownList_MZ_MEMO1" runat="server" Font-Bold="True" ForeColor="Maroon">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="Y">是</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="True">否</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table border="1" style="width: 400px;">
                            <tr>
                                <td class="style6">
                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" Font-Bold="True"
                                        Font-Size="12pt" ForeColor="#0033CC" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="離開" Font-Bold="True"
                                        Font-Size="12pt" ForeColor="Maroon" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
