<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryYear-Endbonus3.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryYear_Endbonus3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" AsyncPostBackTimeout="3600">
    </asp:ScriptManager>
    <div class="PageTitle">
        年終獎金查詢
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="TableStyleBlue">
                <tr>
                    <th>
                        年份：
                    </th>
                    <td>
                        <asp:DropDownList ID="DropDownList_AYEARt" runat="server" DataSourceID="SqlDataSource_YEAR"
                            DataTextField="AYEAR" DataValueField="AYEAR" AppendDataBoundItems="True">
                            <asp:ListItem>全部</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_YEAR" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT DISTINCT AYEAR FROM B_YEARPAY ORDER BY AYEAR">
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <th>
                        發薪機關：
                    </th>
                    <td>
                        <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCHI"
                            DataValueField="MZ_KCODE" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_PAY_AD_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        單位：
                    </th>
                    <td>
                        <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        員工編號：
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_VIEWIDt" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btEFFECTTable" runat="server" Text="查詢" OnClick="btEFFECTTable_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="height: 360px; overflow: scroll; width: 750px;">
                            <asp:GridView ID="GridView_YEARENDBONUS" runat="server" CellPadding="4" GridLines="None"
                                AutoGenerateColumns="False" DataKeyNames="Y_SNID" OnRowCommand="GridView_EFFECT_RowCommand"
                                CssClass="Grid1">
                                <Columns>
                                    <asp:CommandField HeaderText="檢視" ShowSelectButton="True">
                                        <HeaderStyle Wrap="False" />
                                    </asp:CommandField>
                                    <%--<asp:BoundField DataField="NUM" HeaderText="編號">
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>--%>
                                    <asp:BoundField DataField="Y_SNID" HeaderText="流水號">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IDCARD" HeaderText="身份證編號" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AYEAR" HeaderText="年度">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PAY_AD" HeaderText="發薪機關" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_SRANK" HeaderText="薪俸職等" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_SLVC" HeaderText="俸階" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_SPT" HeaderText="俸點" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="LOCKDB" HeaderText="結卡旗標">
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>--%>
                                    <asp:BoundField DataField="PAY" HeaderText="發給月數">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BOSS_AMONTH" HeaderText="主管月數" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SALARYPAY1" HeaderText="薪俸">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROFESS" HeaderText="專業加給">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BOSS" HeaderText="主管加給">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TAX" HeaderText="所得稅">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EXTRA01" HeaderText="法院扣款">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL" HeaderText="總計">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOTE" HeaderText="備註" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LASTDA" HeaderText="最後異動">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
