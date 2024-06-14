<%@ Page Language="C#" AutoEventWireup="true"MasterPageFile="~/TPPD.Master"  CodeBehind="Personal1-1-Salary.aspx.cs" Inherits="TPPDDB._1_personnel.Personal1_1_Salary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TPPDDB" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc3" %>
<%@ Register Src="~/2-salary/UserControl/DLBASESeardhPanel.ascx" TagName="DLBASESeardhPanel"
    TagPrefix="uc1" %>
<%@ Register Src="~/2-salary/UserControl/UserSelector.ascx" TagName="UserSelector" TagPrefix="uc2" %>
<%@ Register Src="~/2-salary/UserControl/PoliceSearchPanel.ascx" TagName="PoliceSearchPanel"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../2-salary/style/Master.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2 {
            text-align: left;
            margin-left: 0px;
        }

        .style12 {
            text-align: left;
        }

        .style30 {
            text-align: left;
            width: 298px;
        }

        .style34 {
            text-align: center;
        }

        .style35 {
            font-size: large;
            font-family: 標楷體;
        }

        .style90 {
            width: 42px;
        }

        .style93 {
            text-align: left;
            background-color: #CCFFFF;
        }

        .style94 {
            width: 43px;
        }

        .style96 {
            text-align: right;
        }

        .style97 {
            text-align: left;
            width: 409px;
        }

        .style98 {
            text-align: left;
        }

        .style99 {
            text-align: left;
            width: 41px;
        }

        .style101 {
            text-align: right;
        }

        .style104 {
            text-align: left;
        }

        .style109 {
            text-align: left;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style110 {
            text-align: center;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFCCFF;
        }

        .style111 {
            border: solid 0px;
        }

        .style118 {
            text-align: left;
        }

        .style119 {
            text-align: left;
        }

        .style132 {
            text-align: left;
        }

        .style134 {
            text-align: left;
        }

        .style136 {
            text-align: left;
        }

        .style138 {
        }

        .style140 {
            text-align: left;
        }

        .style143 {
            width: 44px;
        }

        .style144 {
            width: 41px;
        }

        .style145 {
            width: 50px;
            text-align: left;
        }

        .style153 {
            text-align: left;
            width: 210px;
        }

        .style154 {
            width: 245px;
        }

        .style158 {
            text-align: left;
            width: 77px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style159 {
            text-align: left;
            width: 95px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style160 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style161 {
        }

        .style_tr {
            border: 1px solid maroon;
        }

        .style162 {
            text-align: center;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 48px;
        }

        .style163 {
            text-align: left;
            width: 68px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style164 {
            text-align: left;
            width: 66px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style165 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 32px;
        }

        .style166 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 46px;
        }

        .style167 {
            text-align: left;
            width: 386px;
        }

        .style168 {
            text-align: left;
            width: 40px;
        }
    </style>

    <script src="jquery/IDNO_Vaild.js" type="text/javascript"></script>

    <script src="jquery/jquery.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div style="width: 100%;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div style="width: 100%;">
                    <div style="width: 100%;">
                        <table style="background-color: #6699FF; color: White; width: 100%;">
                            <tr>
                                <td class="style97">
                                    <span class="style35">警勤繁重編輯</span>
                                </td>
                                <td class="style96">
                                    <span class="style35">
                                        <asp:Label ID="Label1" runat="server" CssClass="style101" Text="Label" Visible="False"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 83%; height: 135px; float: left;">
                        <asp:Panel ID="Panel_Title" runat="server">
                            <table border="1" style="width: 100%; text-align: left;">
                                
                                <tr>
                                    <td class="style110">專業加給
                                    </td>
                                    <td class="style161">
                                        <asp:TextBox ID="txtPro" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="style110">警勤加給
                                    </td>
                                    <td class="style14">
                                        <asp:TextBox ID="txtWork" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style110">繁重加給
                                    </td>
                                    <td class="style161">
                                        <asp:TextBox ID="txtElectric" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="style110">主管加給
                                    </td>
                                    <td class="style14">
                                        <asp:TextBox ID="txtBoss" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <asp:Panel ID="Panel_00" runat="server">
                    <table class="style1">
                        <tr>
                            <td style="background-color: #6699FF; color: White;">
                                <asp:Button ID="btUpdate" runat="server" Text="儲存" OnClick="btUpdate_Click" />
                                <asp:Button ID="btExit" runat="server" Text="返回" OnClientClick="history.back();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
