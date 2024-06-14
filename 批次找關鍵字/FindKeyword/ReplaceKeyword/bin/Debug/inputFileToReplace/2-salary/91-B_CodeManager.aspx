<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="91-B_CodeManager.aspx.cs" Inherits="TPPDDB._2_salary.CodeManager" %>

<%@ Register Src="UserControl/SalaryCode.ascx" TagName="SalaryCode" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .styleTitle
        {
            text-align: left;
            font-size: 16pt;
            font-family: 標楷體;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="1">
                <tr>
                    <td class="styleTitle" style="background-color: #6699FF; color: White;">
                        加給代碼維護
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Height="400px"
                            Width="780px" Style="text-align: left;">
                            <cc1:TabPanel runat="server" HeaderText="技術加給" ID="tbp_Technics">
                                <ContentTemplate>
                                    <div style="padding-left: 100px;">
                                        <uc1:SalaryCode ID="sc_Technics" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="警勤加給" ID="tbp_Workp">
                                <ContentTemplate>
                                    <div style="padding-left: 100px;">
                                        <uc1:SalaryCode ID="sc_Workp" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="工作獎金" ID="tbp_Bonus">
                                <ContentTemplate>
                                    <div style="padding-left: 100px;">
                                        <uc1:SalaryCode ID="sc_Bonus" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="外事加給" ID="tbp_Adventive">
                                <ContentTemplate>
                                    <div style="padding-left: 100px;">
                                        <uc1:SalaryCode ID="sc_Adventive" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="勤務繁重加給" ID="tbp_Electric">
                                <ContentTemplate>
                                    <div style="padding-left: 100px;">
                                        <uc1:SalaryCode ID="sc_Electric" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
