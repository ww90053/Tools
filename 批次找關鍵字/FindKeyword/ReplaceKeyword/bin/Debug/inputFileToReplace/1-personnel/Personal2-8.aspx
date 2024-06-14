<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal2-8.aspx.cs" Inherits="TPPDDB._1_personnel.Personal2_8" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1 {
            text-align: left;
            height: 129px;
        }

        .style1 {
            width: 100%;
        }

        .style4 {
        }

        .style6 {
            text-align: left;
        }

        .style7 {
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
                <td style="text-align: left; font-size: large; font-family: 標楷體;">獎懲上傳WEBHR作業
                </td>
            </tr>
        </table>
        <table border="1" class="style1">
            <tr>
                <td class="style7">人事更新密碼
                </td>
                <td class="style6">
                    <asp:TextBox ID="TextBox_MZ_PRPASS" runat="server" CssClass="style4"
                        MaxLength="5" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style7">發文日期
                </td>
                <td class="style6">
                    <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" CssClass="style4"
                        MaxLength="7" Width="65px"></asp:TextBox>
                    至<asp:TextBox ID="TextBox_MZ_DATE2" runat="server" CssClass="style4"
                        MaxLength="7" Width="65px"></asp:TextBox>
                    <span style="color:red">
                        註：發文日期（迄）不得大於系統日期
                    </span>
                </td>
            </tr>
            <tr>
                <td class="style7">核定機關
                </td>
                <td class="style6">
                    <asp:DropDownList ID="DropDownList_MZ_CHKAD" runat="server" AutoPostBack="True" CssClass="style6"
                        Enabled="False" AppendDataBoundItems="True">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">發布人ID
                </td>
                <td class="style6">
                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="120px" MaxLength="24" ></asp:TextBox>
                </td>
            </tr>
        </table>
        <table border="1" class="style1" style="background-color: #CCFFFF;">
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                    <%--隱藏測試按鈕--%>
                    <asp:Button ID="btTEST" runat="server" Text="測試查詢,不更新資料" OnClick="btTEST_Click" CssClass="KEY_IN_BUTTON_BLUE" Visible="false" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="ExcelTable" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                <asp:BoundField DataField="MZ_PRK" HeaderText="事由類別" />
                <asp:BoundField DataField="content" HeaderText="事由內容" />
                <asp:BoundField DataField="MZ_PRRST" HeaderText="核定結果" />
                <asp:BoundField DataField="MZ_DATE" HeaderText="核定日期" />
                <asp:BoundField DataField="check_num" HeaderText="核定字號" />
                <asp:BoundField DataField="MZ_AD" HeaderText="核定機關" />
                <asp:BoundField DataField="sign_date" HeaderText="銓敘部登記日期" />
                <asp:BoundField DataField="sign_num" HeaderText="銓敘部登記文號" />
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
