<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="A_EffectiveList_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.A_EffectiveList_rpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
    </style>

    <script type="text/javascript">
        function pageLoad() {
            var ppm = Sys.WebForms.PageRequestManager.getInstance();
            ppm.add_beginRequest(beginRequestHandler);
            //ppm.add_pageLoaded(pageLoaded);
        }

        function beginRequestHandler(sender, args) {
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint') {
                document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="title_s1">
                        年度考績清冊
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關：
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                                            AutoPostBack="True" DataSourceID="SqlDataSource_AD" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE" DataSourceMode="DataReader">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位：
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_UNIT" runat="server" AppendDataBoundItems="false"
                                            DataSourceID="SqlDataSource_UNIT" DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)"
                                            OnDataBound="DropDownList_UNIT_DataBound">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)" DataSourceMode="DataReader">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="DropDownList_AD" Name="MZ_AD" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 度：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="65px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RF_YEAR" runat="server" ControlToValidate="TextBox_MZ_DATE1"
                                            Display="Dynamic" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                                        (範例：099)
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        參&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 加：
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownList_MZ_SWT" runat="server">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">參加</asp:ListItem>
                                            <asp:ListItem Value="1">不參加</asp:ListItem>
                                            <asp:ListItem Value="2">其他</asp:ListItem>
                                            <asp:ListItem Value="3">另考</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="500px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                                <span class="style3">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
