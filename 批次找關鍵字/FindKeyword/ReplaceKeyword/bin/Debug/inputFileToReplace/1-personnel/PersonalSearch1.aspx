<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalSearch1.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersonalSearch1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
       
        .style8
        {
            width: 80px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
    </style>
    <%--  <script type="text/javascript">
        function leave()
        {
            window.close();
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="style1">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server">
                    <table border="1"  width="480px">
                        <tr>
                            <td class="style8">
                                案&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號:
                            </td>
                            <td >
                                <asp:TextBox ID="TextBox_NO" runat="server" MaxLength="15" Font-Bold="True" ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel1" runat="server">
                        <table border="1"  width="480px">
                            <tr>
                                <td class="style8">
                                    服務機關:
                                </td>
                                <td >
                                    <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" AppendDataBoundItems="True" AutoPostBack="true" 
                                       
                                        OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" Font-Bold="True"
                                        ForeColor="Maroon">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td class="style8">
                                    服務單位:
                                </td>
                                <td >
                                    <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server" Width="100px" Font-Bold="True"
                                        ForeColor="Maroon" ondatabound="DropDownList_MZ_UNIT_DataBound">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="1"  width="480px">
                        <tr>
                            <td class="style8">
                                姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                            </td>
                            <td >
                                <asp:TextBox ID="TextBox_NAME" runat="server" MaxLength="12" Width="100px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style8">
                                身分證號:
                            </td>
                            <td >
                                <asp:TextBox ID="TextBox_ID" runat="server" MaxLength="10" Width="75px" Font-Bold="True"
                                    ForeColor="Maroon"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 480px; text-align: right;" border="1">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" Font-Bold="True"
                                    Font-Size="12pt" ForeColor="#0033CC" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="離開" Font-Bold="True"
                                    Font-Size="12pt" ForeColor="Maroon" Style="color: #000000" />
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
