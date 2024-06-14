<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_EfficiencyInsert.aspx.cs" Inherits="TPPDDB._1_personnel.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style43
        {
            font-size: large;
            font-family: 標楷體;
            text-align: left;
            font-weight: bold;
        }
        .style44
        {
            text-align: right;
        }
        .style34
        {
            text-align: center;
        }
        .style111
        {
            border: solid 0px;
        }
        .style110
        {
        }
        .style45
        {
            font-size: large;
            font-family: 標楷體;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td class="style43">
                        資料轉入歷史檔
                    </td>
                    <td class="style44">
                        <asp:Label ID="Label1" runat="server" CssClass="style45" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="text-align: right;">
                        編制機關：
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                            MaxLength="10" CssClass="style133"></asp:TextBox>
                        <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                            CssClass="style110" TabIndex="-1" />
                        <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                            TabIndex="-1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        編制單位：
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                            OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                        <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                            OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                        <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                            TabIndex="-1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        年度：
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="DropDownList_Year" runat="server" DataSourceID="SqlDataSource_Year"
                            DataTextField="MZ_YEAR" DataValueField="MZ_YEAR">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_Year" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT DISTINCT (MZ_YEAR) FROM A_REV_BASE WHERE MZ_AD=@MZ_AD AND MZ_UNIT =@MZ_UNIT" DataSourceMode="DataReader">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="TextBox_MZ_AD" Name="MZ_AD" PropertyName="Text" />
                                <asp:ControlParameter ControlID="TextBox_MZ_UNIT" Name="MZ_UNIT" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td class="style34">
                            <asp:Button ID="btInsert" runat="server" Text="轉入" OnClick="btInsert_Click" Style="height: 21px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
