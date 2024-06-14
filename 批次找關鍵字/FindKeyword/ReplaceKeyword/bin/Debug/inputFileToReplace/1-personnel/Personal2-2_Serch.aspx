<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal2-2_Serch.aspx.cs" Inherits="TPPDDB._1_personnel.Personal2_2_Serch" %>


<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">        
        .style7
        {
            width: 79px;
            font-weight: 700;
            color: #0033CC;
            text-align: center;
            background-color: #99FFCC;
        }
    </style>
    
    
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1"  width="100%">
                        <tr>
                            <td class="style7">
                                發文字號:
                            </td>
                            <td >
                                <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server"  CssClass="SEARCH_DROPDOWNLIST">
                                </asp:DropDownList>
                                
                              
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_MZ_PRID"
                                    Display="Dynamic" ErrorMessage="不可空白" Visible="False"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                發文文號:
                            </td>
                            <td >
                                <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="10" Width="100px" CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                               
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_PRID1"
                                    Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
              
                    <table border="1"  width="100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" CssClass="SEARCH_BUTTON_BLUE" />
                              <%--  &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button2" runat="server" CssClass="SEARCH_BUTTON_BLACK" Text="離開"
                                    OnClick="Button2_Click" CausesValidation="False" />--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</body>
</html>
