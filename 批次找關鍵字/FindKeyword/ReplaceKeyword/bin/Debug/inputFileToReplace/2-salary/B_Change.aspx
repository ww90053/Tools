<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_Change.aspx.cs" Inherits="TPPDDB._2_salary.B_Change" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: x-large;
            font-weight: bold;
            font-family: 微軟正黑體;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="style1">
級距修改資料調整
</div>
<div>
    <br />
    <asp:Button ID="btnDeal" runat="server" onclick="btnDeal_Click" 
        onclientclick="return confirm(&quot;確定執行？&quot;)" Text="健保級距執行" />
    </div>
</asp:Content>
