<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal4-1Search.aspx.cs" Inherits="TPPDDB._1_personnel.Personal4_1Search" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 70%;
        }
        .style2
        {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
        
            <table border="1" class="style1">
                <tr>
                    <td>
                        機關代碼</td>
                    <td>
                        <cc1:ComboBox ID="ComboBox_CHKAD_AD" runat="server" AppendDataBoundItems="True" 
                            AutoCompleteMode="SuggestAppend" RenderMode="Block" 
                            DataSourceID="SqlDataSource1" DataTextField="RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)" 
                            DataValueField="RTRIM(MZ_KCODE)" MaxLength="0">
                            <asp:ListItem></asp:ListItem>
                        </cc1:ComboBox>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                            SelectCommand="SELECT RTRIM(MZ_KCODE)+RTRIM( MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' " DataSourceMode="DataReader">
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
           <table class="style1">
                <tr>
                <td class="style2">
                    <asp:Button ID="btOk" runat="server"  Text="確定" onclick="btOk_Click" />
                    <asp:Button ID="btLeave" runat="server" Text="離開" onclick="btLeave_Click" />
                </td>
                </tr>
           </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    
    </form>
</body>
</html>
