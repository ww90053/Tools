<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryBasicDetail.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryBasicDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 158px;
            text-align: right;
        }
        .style3
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        薪資明細報表</div>
    <div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_LOCKDB" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
                <table class="style1">
                    <tr>
                        <td style="text-align: right" class="style2">
                            &nbsp;
                        </td>
                        <td style="text-align: left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            發薪機關：
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCHI"
                                DataValueField="MZ_KCODE">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            員工編號：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_POLNO" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            身分證號：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            姓名：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Label ID="Label_MSG" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btSave" runat="server" Text="查詢" OnClick="btSave_Click" />
                            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
                            </asp:ScriptManager>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajax-loader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
                </td> </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
