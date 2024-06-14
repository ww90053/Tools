<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_Inside_Overtime_Search.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_Inside_Overtime_Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            color: #800000;
            font-weight: bold;
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
                <table width="100%" border="1">
                    <tr>
                        <td class="SEARCH_TITLE">
                            機關：
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AutoPostBack="True"  
                                  OnSelectedIndexChanged="DropDownList_MZ_EXAD_SelectedIndexChanged" >
                                </asp:DropDownList>
                           <%-- <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True" CssClass="SEARCH_DROPDOWNLIST">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')" 
                                DataSourceMode="DataReader">
                            </asp:SqlDataSource>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="SEARCH_TITLE">
                            單位：
                        </td>
                        <td style="text-align: left">
                               <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server" Width="100px" class="style55">
                                </asp:DropDownList>
                           <%-- <asp:DropDownList ID="DropDownList_UNIT" runat="server" DataSourceID="SqlDataSource_UNIT"
                                DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)" CssClass="SEARCH_DROPDOWNLIST">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" 
                                DataSourceMode="DataReader">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="SEARCH_TITLE">
                            年月：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" ForeColor="Maroon" Width="75px"
                                MaxLength="5"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox1">
                            </cc1:FilteredTextBoxExtender>
                            <span class="style1">(範例：10001)</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                                Display="Dynamic" ErrorMessage="不可空白" Font-Bold="True"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <table class="style52" border="1" width="100%">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" class="style53"
                                CssClass="SEARCH_BUTTON_BLUE" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" Text="離開" OnClick="Button2_Click" class="style54"
                                CausesValidation="False" CssClass="SEARCH_BUTTON_BLACK" />
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
