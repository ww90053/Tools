<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personal_Salary_Search.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Salary_Search" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 157px;
        }

        .style3 {
            width: 83px;
        }

        .style4 {
            text-align: left;
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
                    <table class="style1">
                        <tr>
                            <td class="style3">機 關
                            </td>
                            <td class="style2">
                                <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                    CssClass="SEARCH_DROPDOWNLIST">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">是 否 在 職
                            </td>
                            <td class="style2">
                                <asp:DropDownList ID="ddlStatus2" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04' ORDER BY MZ_KCODE" DataSourceMode="DataReader"></asp:SqlDataSource>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnExportExcel" runat="server" Text="匯出Excel"
            CssClass="KEY_IN_BUTTON_BLUE" OnClick="btnExportExcel_Click" />
    </form>
</body>
</html>
