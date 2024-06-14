<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_Forleave_Overtime_RewardCheck.aspx.cs" Inherits="TPPDDB._3_forleave.C_Forleave_Overtime_RewardCheck" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="540"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">超勤賸餘時數審核</div>

            <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                <table width="100%">
                    <tr>
                        <td class="style1">年度：</td>
                        <td style="text-align: left">
                            <asp:TextBox ID = "tb_Year"  runat = "server" Width="50" MaxLength="3"></asp:TextBox>（例如：104, 099）
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">機關名稱：</td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DropDownList_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" onselectedindexchanged="DropDownList_AD_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource1" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" >
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'" 
                                DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">單位名稱：</td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DropDownList_EXUNIT" runat="server"  AppendDataBoundItems="false" OnDataBound="DropDownList_UNIT_DataBound" Width = "140px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                               
                        </td>
                    </tr>
                    <tr>
                        <td class = "style1">上下半年</td>
                        <td style="text-align: left">
                            <asp:RadioButtonList ID = "YearRange" runat = "server" RepeatDirection="Horizontal">
                                <asp:ListItem Value = "Up" Selected="True">上半年度</asp:ListItem>
                                <asp:ListItem Value = "Down">下半年度</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="500px">
                <table width="100%">
                    <tr>
                        <td class="style2" colspan="2" style="text-align: center">
                            <asp:Button ID="btnStatistics" runat="server" Text="統計" OnClick = "btnStatistics_Click"  />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="列印" OnClick = "btnPrint_click"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="查詢" onclick="btnSearch_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btOut" runat="server" Text="審核"  OnClick = "btnReward_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>


            <asp:GridView ID="gvList" runat="server" HeaderStyle-BackColor="Aqua"
                          AutoGenerateColumns="False" Font-Size="Small"
                          DataKeyNames="MZ_ID" CellPadding="4" ForeColor="#333333" 
                onrowdatabound="gvList_RowDataBound">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:TemplateField >
                        <HeaderTemplate>
                            <asp:CheckBox ID = "AllCheck" runat = "server" OnCheckedChanged = "AllChek_Click" AutoPostBack = "true"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server"  Checked = '<%# Eval("MZ_VERIFY").ToString() =="1" ? true:false %>' /> <!--  -->
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證"></asp:BoundField>
                    <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" ></asp:BoundField>
                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名"></asp:BoundField>
                    <asp:BoundField DataField="Month1"></asp:BoundField>
                    <asp:BoundField DataField="Month2"></asp:BoundField>
                    <asp:BoundField DataField="Month3"></asp:BoundField>
                    <asp:BoundField DataField="Month4"></asp:BoundField>
                    <asp:BoundField DataField="Month5"></asp:BoundField>
                    <asp:BoundField DataField="Month6"></asp:BoundField>
                    <%--<asp:BoundField DataField="UPYEAR_LEFT_HOUR" HeaderText="上半年度剩餘"></asp:BoundField>--%>
                    <asp:TemplateField HeaderText="上半年度剩餘">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUPYEAR_LEFT_HOUR" runat="server" AutoPostBack="True" 
                                        Text='<%# Eval("MZ_HOUR_H1_LEFT") %>' Enabled='<%# Eval("MZ_VERIFY").ToString() =="1" ? false:true %>' 
                                        Width="25px">
                            </asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="合計">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLEFT_HOUR" runat="server" AutoPostBack="True" 
                                        Text='<%# Eval("MZ_HOUR_H1") %>' Enabled='<%# Eval("MZ_VERIFY").ToString() =="1" ? false:true %>'
                                        Width="25px">
                            </asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="敘獎時數">
                        <ItemTemplate>
                            <asp:TextBox ID="txtREWARD_HORU" runat="server" AutoPostBack="True" 
                                Text='<%# Eval("MZ_HOUT_AWARD") %>' Enabled='<%# Eval("MZ_VERIFY").ToString() =="1"?false:true %>'
                                Width="25px">
                            </asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="備註" Visible = "false" >
                        <ItemTemplate>
                            <asp:TextBox ID="txtMEMO" runat="server" AutoPostBack="True" 
                                Text='<%# Eval("MZ_MEMO") %>' Enabled='<%# Eval("MZ_VERIFY").ToString() =="1"?false:true %>'
                                Width="60px">
                            </asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="MZ_EXUNIT" ItemStyle-cssclass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>


            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <br />
                    <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                    <span class="style3">資料量多，產生中 請稍待…</span>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
             <asp:PostBackTrigger ControlID = "btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
    <style type="text/css">
    .hiddencol
    {
        display:none;
    }
    </style>
</asp:Content>
