<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" 
    CodeBehind="C_CARDHISTORY_SURE.aspx.cs" Inherits="TPPDDB._15_score.C_CARDHISTORY_SURE" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 158px;
        }

        .dt_header {
            background-color: #000084;
            color: white;
            font-weight: bold;
        }

        .dt_content {
            background-color: #EEEEEE;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')  order by mz_kcode"></asp:SqlDataSource>
    <div class="title_s1">
        因公疏未辦理按卡紀錄線上收件
    </div>    
    <div>
        <asp:Panel ID="Panel1" runat="server" GroupingText="">
            <table class="style1">
                <tr>
                    <td style="text-align: right;" class="style2">
                        申請日期：
                    </td>
                    <td style="text-align: left" colspan="3">
                        <asp:TextBox ID = "txt_SDATETIME" runat = "server" />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txt_SDATETIME" format="yyyy/MM/dd">
                        </cc1:CalendarExtender>~
                        <asp:TextBox ID = "txt_EDATETIME" runat = "server" />
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txt_EDATETIME" format="yyyy/MM/dd">
                        </cc1:CalendarExtender>
                    </td>
                    <td style="text-align: right;" class="style2">
                        目前狀態：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddl_STATUS" runat="server">
                            <asp:ListItem Value="">全部</asp:ListItem>
                            <asp:ListItem Value="0">退回</asp:ListItem>
                            <asp:ListItem Value="1">新增</asp:ListItem>
                            <asp:ListItem Value="2">陳核</asp:ListItem>
                            <asp:ListItem Value="3">決行</asp:ListItem>
                            <asp:ListItem Value="4">收件</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>

                    <td style="text-align: right;" class="style2">服務機關：
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="DropDownList_AD" runat="server" DataSourceID="SqlDataSource_AD" AutoPostBack="true"
                            DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" OnDataBound="DropDownList_AD_DataBound"
                            OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right;" class="style2">現服單位：
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="DropDownList_UNIT" runat="server" AppendDataBoundItems="false" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="true"
                            OnDataBound="DropDownList_UNIT_DataBound" OnSelectedIndexChanged="DropDownList_UNIT_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right;" class="style2">審查者：
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="DropDownList_NAME" runat="server" AppendDataBoundItems="true" OnDataBound="DropDownList_NAME_DataBound">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td colspan="3">
                        <asp:Button ID="Button1" runat="server" Text="搜尋" OnClick="Button1_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Excel" runat="server" Text="匯出" OnClick="Btn_Excel_Click"  />
                    </td>
                </tr>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </table>
        </asp:Panel>
    </div>
    <div>
        <asp:UpdatePanel ID="Panel2" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gv_C_CARDHISTORY_EDIT" runat="server" Width="95%" AllowSorting="True"
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
                    Style="font-family: 微軟正黑體;" DataKeyNames="SN" OnRowDataBound="gv_C_CARDHISTORY_EDIT_RowDataBound"
                     OnRowCommand="gv_C_CARDHISTORY_EDIT_RowCommand">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="MZ_NAME" HeaderText="申請者">
                            <ItemStyle Width="40%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="申請日期">
                            <ItemTemplate>
                                <asp:Label ID="lbl_DATETIME" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="目前狀態">
                            <ItemTemplate>
                                <asp:Label ID="lbl_STATUS" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="審查方式">
                            <ItemTemplate>
                                <asp:Label ID="lb2_STATUS" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="審查者">
                            <ItemTemplate>
                                <asp:Label ID="lbl_MOD_USER" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>                     
                        <asp:TemplateField HeaderText="收件">
                            <ItemTemplate>
                                <asp:Button ID="btnSure" runat="server" Text="收件" CommandName="doSure" CommandArgument='<%#Eval("SN") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                      
                        <asp:TemplateField HeaderText="內容">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "C_CARDHISTORY_edit.aspx?TPM_FION=9&sn="+Eval("SN") %>'>內容</asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID ="gv_C_CARDHISTORY_EDIT"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>



