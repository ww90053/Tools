<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="82-B_BaseDataFilter.aspx.cs" Inherits="TPPDDB._2_salary._2_B_BaseDataFilter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 800px; background-color: #6699FF; color: White; font-size: 16pt;
        font-family: 標楷體; text-align: left;">
        員警快速查詢
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding-top: 10px;">
                <table class="TableStyleBlue" width="600px">
                    <tr>
                        <th>
                            發薪機關
                        </th>
                        <td>
                            <asp:DropDownList ID="ddl_PayAD" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            任公職滿30年
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rbl_Is30" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="是">是</asp:ListItem>
                                <asp:ListItem Selected="True" Value="否">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            領有主管加給
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rbl_IsBoss" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btn_Send" runat="server" Text="查詢" OnClick="btn_Send_Click" />
                            <asp:Button ID="btn_Export" runat="server" Text="輸出為Excel" OnClick="btn_Export_Click"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:GridView ID="gv_Result" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    AllowPaging="True" OnPageIndexChanging="gv_Result_PageIndexChanging" Width="600px"
                    PageSize="15" CssClass="Grid1">
                    <RowStyle BackColor="#EFF3FB" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
