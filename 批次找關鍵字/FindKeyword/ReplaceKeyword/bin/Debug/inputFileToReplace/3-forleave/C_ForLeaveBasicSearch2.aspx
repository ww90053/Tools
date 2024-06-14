<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveBasicSearch2.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveBasicSearch2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
        .style59
        {
            text-align: left;
            width: 349px;
        }
        .style61
        {
            width: 118px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style62
        {
            width: 103px;
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
            <asp:Panel ID="Panel1" runat="server" Width="100%">
            <table class="style52" border="1" width="100%">
                <tr>
                   <td class="style61">
                        姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:     
                   </td>
                    <td class="style59">
                        <asp:TextBox ID="TextBox_NAME" runat="server"></asp:TextBox>
                   </td>
                </tr>
                <tr>
                   <td class="style61">
                            身分證號:
                   </td>
                   <td class="style59">
                   <asp:TextBox ID="TextBox_ID" runat="server"></asp:TextBox> 
                   </td>
                </tr>
                <tr>
                  <td class="style61">
                            機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關:
                  </td>
                  <td class="style59">                 
                      <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" 
                          DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" 
                          DataValueField="MZ_KCODE" class="style55" 
                          onselectedindexchanged="DropDownList_MZ_EXAD_SelectedIndexChanged" 
                          AutoPostBack="True">
                      </asp:DropDownList>
                      <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                          ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                          ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                          SelectCommand="(SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') UNION ALL (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '376411%') " DataSourceMode="DataReader">
                   </asp:SqlDataSource>
                  </td>
                </tr> 
                <tr>
                     <td class="style61">
                            單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位:
                     </td>
                     <td class="style59">        
                         <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server"  class="style55">
                         </asp:DropDownList>
                     
                     </td>
                </tr> 
            </table>
            <table class="style52" border="1" width="100%">
                    <tr>
                        <td>
                        
                            <asp:Button ID="Button1" runat="server" Text="確定" class="style53" 
                                onclick="Button1_Click"/>
                            &nbsp;&nbsp;&nbsp;
                            
                        
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" Text="離開" 
                                class="style54" onclick="Button2_Click"/>
                            
                        
                        </td>
                    </tr>
             </table>       
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
   
    </form>
</body>
</html>
