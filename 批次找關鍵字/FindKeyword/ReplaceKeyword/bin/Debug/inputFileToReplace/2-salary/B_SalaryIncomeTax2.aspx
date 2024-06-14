<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryIncomeTax2.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryIncomeTax2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 190px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <div class="PageTitle">
        所得稅資料產生
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="TableStyleBlue">
                <tr>
                    <th>
                        年度：
                    </th>
                    <td class="style1">
                        <asp:TextBox ID="txt_year" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        發薪機關：
                    </th>
                    <td class="style1">
                        <asp:DropDownList ID="DropDownList_PAY_AD" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        範圍：</th>
                    <td class="style1">
                        <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True">全部</asp:ListItem>
                            <asp:ListItem>身份證字號</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th>
                        &nbsp;&nbsp;&nbsp;&nbsp; 身份證字號：</th>
                    <td class="style1">
                        <asp:TextBox ID="tbIDNO" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr id="r11" runat="server" visible="false">
                    <th>
                        刪除</th>
                    <td class="style1">
                        &nbsp;&nbsp;<asp:TextBox ID="TextBox1" runat="server" Width="50px">92</asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                            onclientclick="return confirm(&quot;會刪除已產生之所得資料，請確認?&quot;)" Text="刪除重跑" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btCreateTAX" runat="server" Text="產生" 
                            OnClick="btCreateTAX_Click" 
                            onclientclick="return confirm(&quot;請確認欲產生之年度，若該年已有資料將會刪除原先資料？&quot;);" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
