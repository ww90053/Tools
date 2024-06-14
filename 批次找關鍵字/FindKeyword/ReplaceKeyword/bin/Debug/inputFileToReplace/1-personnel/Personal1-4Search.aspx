<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal1-4Search.aspx.cs"
    Inherits="TPPDDB._1_personnel.Personal1_4Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未命名頁面</title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 88px;
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
            <div>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style1">
                            機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關：</td>
                        <td>
                            <asp:DropDownList ID="DropDownList_AD" runat="server" AutoPostBack="True" 
                                DataSourceID="SqlDataSource_AD" DataTextField="MZ_KCHI" 
                                DataValueField="MZ_KCODE" CssClass="SEARCH_DROPDOWNLIST" 
                                ondatabound="DropDownList_AD_DataBound" 
                                onselectedindexchanged="DropDownList_AD_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                
                                SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位：</td>
                        <td>
                            <asp:DropDownList ID="DropDownList_UNIT" runat="server" 
                                DataSourceID="SqlDataSource_UNIT" DataTextField="RTRIM(MZ_KCHI)" 
                                DataValueField="RTRIM(MZ_KCODE)" ondatabound="DropDownList_UNIT_DataBound" 
                                CssClass="SEARCH_DROPDOWNLIST">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" 
                                        PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            身分證號：</td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Font-Bold="True" 
                                ForeColor="Maroon" Width="75px" CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; text-align: right;">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" 
                                CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btLeave" runat="server" Text="離開" OnClick="btLeave_Click" 
                                CssClass="SEARCH_BUTTON_BLACK" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
