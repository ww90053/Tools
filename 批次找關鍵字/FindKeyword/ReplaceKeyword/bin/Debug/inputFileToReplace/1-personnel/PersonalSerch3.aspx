<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalSerch3.aspx.cs"
    Inherits="TPPDDB._1_personnel.PersoanlSerch3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1"  width="100%">
                        <tr>
                            <td class="style7">
                                發文字號:
                            </td>
                            <td >
                                <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" 
                                    CssClass="SEARCH_DROPDOWNLIST" ondatabound="DropDownList_MZ_PRID_DataBound">
                                </asp:DropDownList>
                                
                                <asp:TextBox ID="TextBox_MZ_PRID" runat="server" MaxLength="24" Visible="False" Width="110px"
                                    CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                                <asp:Button ID="btPRID" runat="server" CausesValidation="False" CssClass="style110"
                                    OnClick="btPRID_Click" TabIndex="-1" Text="V" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                發文文號:
                            </td>
                            <td >
                                <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="10" Width="100px" CssClass="SEARCH_DROPDOWNLIST"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel2" runat="server" Visible="false">
                        <table border="1"  width="100%">
                            <tr>
                                <td class="style7">
                                    身分證號:
                                </td>
                                <td >
                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style7">
                                    姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                                </td>
                                <td >
                                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px" Style="height: 19px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style7">
                                    發文年度:
                                </td>
                                <td >
                                    <asp:TextBox ID="TextBox_YEAR" runat="server" Width="60px" MaxLength="3"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="style7">
                                    發文日期:</td>
                                <td >
                                    <asp:TextBox ID="tbSDate" runat="server" MaxLength="7" Width="70px"></asp:TextBox>
                                    &nbsp;至&nbsp;<asp:TextBox ID="tbEDate" runat="server" MaxLength="7" Width="70px"></asp:TextBox>
                                    ex:1020101</td>
                            </tr>
                            <tr>
                                <td class="style7">
                                    獎懲依據:
                                </td>
                                <td >
                                    <asp:DropDownList ID="DropDownList_MZ_PROLNO" runat="server"  OnDataBound="DropDownList_MZ_PROLNO_DataBound">
                                    </asp:DropDownList>
                                   
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="1"  width="100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" CssClass="SEARCH_BUTTON_BLUE" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button2" runat="server" CssClass="SEARCH_BUTTON_BLACK" Text="離開"
                                    OnClick="Button2_Click" CausesValidation="False" />
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
