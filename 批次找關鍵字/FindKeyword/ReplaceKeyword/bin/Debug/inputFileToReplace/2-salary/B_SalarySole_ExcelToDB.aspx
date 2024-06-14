<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="B_SalarySole_ExcelToDB.aspx.cs"
    Inherits="TPPDDB._2_salary.B_SalarySole_ExcelToDB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 400px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="style1" border="1">
            <tr>
                <td colspan="2">
                    請選擇 Excel 來源
                </td>
            </tr>
            <tr>
                <td >
                    項目名稱
                </td>
                <td align="left" colspan="7">
                    <asp:DropDownList ID="DropDownList_NUM" runat="server" DataSourceID="SqlDataSource_NUM"
                        DataTextField="TEXT_DATA" DataValueField="ID">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource_NUM" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ID, dbo.LPAD(ID,2,'0') + ' ' + NAME AS TEXT_DATA FROM B_SOLEITEM ORDER BY TEXT_DATA">
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:FileUpload ID="FileUpload_ExcelToDB" runat="server" />
                    <asp:Button ID="btExcelToDB" runat="server" OnClick="btExcelToDB_Click" Text="匯入" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label_MSG" runat="server" Text="Label"></asp:Label>
                    <asp:Label ID="Label_MSGT" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel1" runat="server" GroupingText="已產生資料" Width="380px">
                        <asp:ListBox ID="ListBox_Data" runat="server" Width="100%" Height="130px"></asp:ListBox>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    範例檔下載：<br />
                    <a href="Sample/SOLE_2007.xlsx">Excel 2007以上</a>、<a href="Sample/SOLE_2003.xls">Excel
                        2003以下</a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
