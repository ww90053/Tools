<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasic4.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasic4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageTitle">
        薪俸級距表
    </div>
    <div style="width: 770px; text-align: right;">
        <asp:Button ID="btn_Export" runat="server" Text="匯出" OnClick="btn_Export_Click" />
    </div>
    <div style="height: 450px; overflow: scroll;">
        <asp:GridView ID="GridView_SALARY" runat="server" AutoGenerateColumns="False" CellPadding="4"
            GridLines="None" Width="770px" CssClass="Grid1">
            <Columns>
                <asp:BoundField DataField="NAME1" HeaderText="項目" SortExpression="NAME1" />
                <asp:BoundField DataField="ORIGIN1" HeaderText="俸點" SortExpression="ORIGIN1" />
                <asp:BoundField DataField="ORIGIN2" HeaderText="俸級" SortExpression="ORIGIN2" />
                <asp:BoundField DataField="PAY1" HeaderText="薪俸" SortExpression="PAY1" DataFormatString="{0:$#,#0}" />
                <asp:BoundField DataField="PAY2" HeaderText="福利互助金" SortExpression="PAY2" DataFormatString="{0:$#,#0}" />
                <asp:BoundField DataField="PAY3" HeaderText="警察共濟金" SortExpression="PAY3" DataFormatString="{0:$#,#0}" />
                <asp:BoundField DataField="INSURANCE_F" HeaderText="公保費" SortExpression="INSURANCE_F"
                    DataFormatString="{0:$#,#0}" />
                <asp:BoundField DataField="CONCUR3" HeaderText="退撫基金(自繳)" SortExpression="CONCUR3"
                    DataFormatString="{0:$#,#0}" />
                <asp:BoundField DataField="GOVERNMENT" HeaderText="退撫基金(政府撥繳)" SortExpression="GOVERNMENT"
                    DataFormatString="{0:$#,#0}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>