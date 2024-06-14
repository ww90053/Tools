<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="81-B_UnitCheckPage.aspx.cs" Inherits="TPPDDB._2_salary._1_B_UnitCheckPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="height: 500px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table style="background-color: #99CCFF">
                        <tr>
                            <th>
                                機關
                            </th>
                            <td>
                                <asp:DropDownList ID="ddl_AD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_AD_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 350px; width: 395px; float: left;">
                    <table>
                        <tr>
                            <td style="background-color: #FFFFCC">
                                無薪資資料
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gv_base" runat="server" AllowPaging="True" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" OnPageIndexChanging="gv_base_PageIndexChanging" AutoGenerateColumns="False"
                                    PageSize="10">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:BoundField DataField="PAY_AD" HeaderText="發薪機關" />
                                        <asp:BoundField DataField="MZ_ID" HeaderText="身份證字號" />
                                        <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" />
                                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 350px; width: 395px; float: left;">
                    <table>
                        <tr>
                            <td style="background-color: #FFFFCC">
                                於現發薪機關無薪資帳戶
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gv_Account" runat="server" AllowPaging="True" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" OnPageIndexChanging="gv_Account_PageIndexChanging" AutoGenerateColumns="False"
                                    PageSize="10">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:BoundField DataField="PAY_AD" HeaderText="發薪機關" />
                                        <asp:BoundField DataField="MZ_ID" HeaderText="身份證字號" />
                                        <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" />
                                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
