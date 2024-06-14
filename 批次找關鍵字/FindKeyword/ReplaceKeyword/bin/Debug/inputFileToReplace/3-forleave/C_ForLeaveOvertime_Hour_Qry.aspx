<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_Hour_Qry.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_Hour_Qry" Title="未命名頁面" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script src="/Scripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.loadmask.min.js" type="text/javascript"></script>
    <link href="/CSS/jquery.loadmask.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style58
        {
            text-align: left;
            width: 30px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style59
        {
            text-align: left;
            height: 20px;
            width: 185px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server">
                <table class="style10" border="1">
                    <tr>
                        <td class="style8">
                            每月加班費時數表產生
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style58">
                         現服機關
                        </td>
                        <td class="style3">
                        <%--DataSourceID="SqlDataSource1"--%>
                            <asp:DropDownList ID="DropDownList_AD" runat="server" AutoPostBack="True" 
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND (MZ_KCODE=@KCODE1 OR MZ_KCODE=@KCODE2)" 
                                DataSourceMode="DataReader">
                                <SelectParameters>
                                    <asp:SessionParameter Name="KCODE1" SessionField="ADPMZ_AD" />
                                    <asp:SessionParameter Name="KCODE2" SessionField="ADPMZ_EXAD" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td class="style58">
                            現服單位
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="ddl_MZUNIT" runat="server" AutoPostBack="true" >
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_MZ_UNIT" runat="server" ReadOnly="True" Width="50px"></asp:TextBox>
                            <asp:Label ID="lb_unit" runat="server" Font-Bold="True" ForeColor="#0033FF"></asp:Label>--%>
                        </td>
                        <td class="style58" style="width:100px;">身分證字號</td>
                        <td>
                            <asp:TextBox ID="txtMZ_ID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    </tr>
                    <tr>
                        <td class="style58">
                            年 度
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTYYEAR" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <td class="style58">
                            月 份
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="DropDownList_DUTYMONTH" runat="server" AppendDataBoundItems="True"
                                AutoPostBack="True" OnSelectedIndexChanged="DropDownList_DUTYMONTH_SelectedIndexChanged">
                                <asp:ListItem Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="Button2" runat="server" OnClick="Button2_NewClick" Text="產生印領清冊" class="style9" OnClientClick="$('body').mask('產生中...');" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        OnRowCreated="GridView1_RowCreated">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
