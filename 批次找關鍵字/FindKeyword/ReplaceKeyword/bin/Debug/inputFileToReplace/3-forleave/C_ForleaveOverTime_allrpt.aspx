<%@ Page  Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"  CodeBehind="C_ForleaveOverTime_allrpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForleaveOverTime_allrpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../styles/style09.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .row_header {
            width: 100px;
            font-weight: 700;
            color: #0033CC;
            text-align: justify;
            text-align-last: justify;
            background-color: #99FFCC;
        }

        .ViewHeader {
            color: #000000;
            font-weight: 700;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .EditHeader {
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .GV_Header {
            background-color: #507CD1;
            font-weight: bold;
            color: white;
            border: solid 1px #e4e3e3;
            text-align: center;
        }

        .GV_Hidden {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="style10 style8">
          5.12 加班補發總清冊列印
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_Query" runat="server" GroupingText="查詢條件" Width="370px">
                <asp:Table runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            機 關：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_AD" AutoPostBack="true"  onselectedindexchanged="ddl_Search_AD_SelectedIndexChanged"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            單 位：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_Unit"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            年月：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left; white-space:nowrap;">
                            <asp:TextBox runat="server" ID="txt_Year" MaxLength="5" value="" Width="40px"></asp:TextBox>
                            ~
                            <asp:TextBox runat="server" ID="txt_endYear" MaxLength="5" value="" Width="40px"></asp:TextBox>
                        </asp:TableCell>
                        
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            身 分 證 號：
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:TextBox runat="server" ID="txt_MZ_ID" MaxLength="10" Width="100px"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                           
                        </asp:TableCell>
                        <asp:TableCell style="text-align: left;">
                            <asp:RadioButtonList ID="rbl_print" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text ="依編制單位列印" Value="0" Selected="True"></asp:ListItem>  
                                        <asp:ListItem Text ="依現服單位列印" Value="1" ></asp:ListItem>  
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
             <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="370px">
                <table width="100%">
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button runat="server" ID="btn_allrpt2" Text="列印"  OnClick="btn_allrpt1_Click" />
                            <input type="reset" value="取消" />
                            <asp:Button runat="server" ID="btn_allrpt2_1" Text="列印Excel"  OnClick="btn_allrpt1_1_Click" />
                            <%--<asp:Button runat="server" ID="btn_allrpt2" Text="超勤加班補發總清冊" CssClass="style9" OnClick="btn_allrpt2_Click" />
                            <asp:Button runat="server" ID="btn_allrpt2_1" Text="超勤加班補發總清冊(Excel)" CssClass="style9" OnClick="btn_allrpt2_1_Click" />--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>