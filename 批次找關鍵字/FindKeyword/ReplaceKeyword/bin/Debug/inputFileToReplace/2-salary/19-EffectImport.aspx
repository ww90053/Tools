<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="19-EffectImport.aspx.cs" Inherits="TPPDDB._2_salary._9_EffectImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        考績評議清冊匯入
    </div>
    <table class="TableStyleBlue" style="width: 90%; margin-top: 10px;">
         <tr>
            <th>
                年度
            </th>
            <td>
                <asp:TextBox ID="txt_Year" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
             
            </td>
        </tr>
        <tr>
            <th>
                評議清冊檔案
            </th>
            <td>
                <asp:FileUpload ID="fl_import" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="Button1" runat="server" Text="開始匯入" OnClick="Button1_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
