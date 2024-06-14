<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ReviewManagement_GROUPUPDATE.aspx.cs" Inherits="TPPDDB._3_forleave.C_ReviewManagement_GROUPUPDATE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style58
        {
            width: 78px;
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
                    <table border="1">
                        <tr>
                            <td class="style58">
                                服務機關:</td>
                            <td>
                                <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AutoPostBack="True" 
                                    DataSourceID="SqlDataSource_AD" DataTextField="MZ_KCHI" 
                                    DataValueField="MZ_KCODE" class="style55"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    
                                    SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" 
                                    DataSourceMode="DataReader">
                                </asp:SqlDataSource>
                             </td>   
                        </tr>
                        <tr>
                            <td class="style58">
                                服務單位:</td>
                            <td>  
                                <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" CssClass="style55"
                                    DataSourceID="SqlDataSource_UNIT" DataTextField="RTRIM(MZ_KCHI)" 
                                    DataValueField="RTRIM(MZ_KCODE)">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                    
                                    SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" 
                                    DataSourceMode="DataReader">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DropDownList_MZ_EXAD" Name="MZ_AD" 
                                            PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="style58">
                                刷卡設定:</td>
                            <td>
                            
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="一層" />
                                <asp:CheckBox ID="CheckBox2" runat="server" Text="二層" />
                            
                            </td>
                       </tr>         
                    </table> 
                    <table class="style52">
                        <tr>
                            <td>
                            
                                <asp:Button ID="btOk" runat="server" Text="確定"  onclick="btOk_Click" class="style53"/>
                                <asp:Button ID="btLeave" runat="server" Text="離開" onclick="btLeave_Click" class="style54" />
                            
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
