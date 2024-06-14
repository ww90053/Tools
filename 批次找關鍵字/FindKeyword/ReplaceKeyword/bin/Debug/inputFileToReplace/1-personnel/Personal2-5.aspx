<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal2-5.aspx.cs" Inherits="TPPDDB._1_personnel.Personal2_5"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1
        {
            text-align: left;
            height: 129px;
        }
        .style1
        {
            width: 100%;
        }
        .style4
        {
        }
        .style6
        {
            text-align: left;
        }
        .style7
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 81px;
        }
    </style>
    <%--<script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server">
        <table style="background-color: #6699FF; color: White; width: 100%;">
            <tr>
                <td style="text-align: left; font-size: large; font-family: 標楷體;">
                    獎懲資料上傳OA作業
                </td>
            </tr>
        </table>
        <table border="1" class="style1">
            <tr>
                <td class="style7">
                    人事更新密碼
                </td>
                <td class="style6">
                    <asp:TextBox ID="TextBox_MZ_PRPASS" runat="server" CssClass="style4" 
                        MaxLength="5" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style7">
                    發文日期
                </td>
                <td class="style6">
                    <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" CssClass="style4" 
                        MaxLength="7" Width="65px"></asp:TextBox>
                    至<asp:TextBox ID="TextBox_MZ_DATE2" runat="server" CssClass="style4" 
                        MaxLength="7" Width="65px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table border="1" class="style1" style="background-color: #CCFFFF;">
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="ExcelTable" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="MZ_NAME" HeaderText="姓名"  />
                <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                <asp:BoundField DataField="MZ_PRK" HeaderText="獎懲類別" />
                <asp:BoundField DataField="MZ_PRRST" HeaderText="獎懲結果" />
                <asp:BoundField DataField="MZ_IDATE" HeaderText="核定日期" />
                <asp:BoundField DataField="MZ_CHKAD" HeaderText="核定機關" />
                <asp:BoundField DataField="MZ_CODE" HeaderText="輸入者密碼" />
                <asp:BoundField DataField="MZ_POLK" HeaderText="項別" />
                <asp:BoundField DataField="MZ_PROLNO1" HeaderText="款" />
                <asp:BoundField DataField="MZ_PROLNO2" HeaderText="項" />
                <asp:BoundField DataField="MZ_PROLNO3" HeaderText="目" />
                <asp:BoundField DataField="MZ_PCODE" HeaderText="配分" />
                <asp:BoundField DataField="MZ_PRCT" HeaderText="獎懲內容" />
                <asp:BoundField DataField="MZ_PRID" HeaderText="核定文號" />
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </asp:Panel>
</asp:Content>
