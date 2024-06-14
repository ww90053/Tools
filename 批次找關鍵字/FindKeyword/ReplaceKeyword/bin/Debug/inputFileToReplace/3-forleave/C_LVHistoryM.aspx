<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_LVHistoryM.aspx.cs" Inherits="TPPDDB._3_forleave.C_LVHistoryM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Style23.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 26px;
            width: 324px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            border: 3px solid White;
            background-color: White;
            padding: 3px;
        }
        .style2
        {
            height: 26px;
            font-weight: bold;
            color: #009933;
            font-size: x-large;
        }
        .style3
        {
            height: 26px;
            width: 71px;
        }
        .style4
        {
            width: 71px;
        }
        .style5
        {
            width: 324px;
        }
        .style6
        {
            color: #CC0000;
            font-weight: bold;
        }
        .style7
        {
            height: 26px;
            width: 326px;
        }
        .style8
        {
            width: 326px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        留職停薪時間管理
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%;">
                <!---->
                <asp:Panel ID="pnlSearch" runat="server" Width="417px" CssClass="modalPopup" Style="display: none;">
                    <table class="TableStyleNone" width="100%">
                        <tr>
                            <td class="style2" colspan="2">
                                查詢
                            </td>
                        </tr>
                        
                         <tr>
                            <td class="style4">
                                身分證字號
                            </td>
                            <td>
                                <asp:TextBox ID="tbSId" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                &nbsp;<asp:Button ID="btn_Search_People" runat="server" Text="依機關單位尋找人員" 
                                    onclick="btn_Search_People_Click"  />
                            </td>
                        </tr>
                        
                        <tr id="tr_unit1" runat="server"  visible="false">
                            <td class="style3">
                                機關
                            </td>
                            <td class="style7" style="text-align: left;">
                                <asp:DropDownList ID="ddl_Select_AD" runat="server" AutoPostBack="True" Width="150px"
                                    OnSelectedIndexChanged="ddl_Select_AD_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="tr6" runat="server" visible="false">
                            <td class="style4">
                                單位
                            </td>
                            <td style="text-align: left;" class="style8">
                                <asp:DropDownList ID="ddl_Select_Unit" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddl_Select_Unit_SelectedIndexChanged">
                                    <asp:ListItem>請先選擇機關</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="tr_unit2"  runat="server" visible="false">
                            <td class="style4">
                                人員
                            </td>
                            <td class="style8" style="text-align: left;">
                                <asp:DropDownList ID="ddlSPeople" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSPeople_SelectedIndexChanged">
                                    <asp:ListItem>請先選擇機關單位</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
                       
                        <tr id="tr3" runat="server" visible="false">
                            <td class="style4">
                                日期區間
                            </td>
                            <td style="text-align: left;" class="style8">
                                &nbsp;&nbsp; 起
                                <asp:TextBox ID="tbStart" runat="server" Width="80px" MaxLength="7"></asp:TextBox>
                                &nbsp; 至&nbsp;
                                <asp:TextBox ID="tbEnd" runat="server" Width="80px" MaxLength="7"></asp:TextBox>
                                <span class="style6">(範例：1011001)</span>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server" visible="false">
                            <td class="style4">
                                備註
                            </td>
                            <td style="text-align: left;" class="style8">
                                <asp:TextBox ID="tbSMemo" runat="server" Width="280px" Height="42px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查詢" />
                                &nbsp;<asp:Button ID="btnSCancel" runat="server" Text="取消" OnClick="btnSCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <!---->
                <asp:Panel ID="pnlInsert" runat="server" Width="413px" CssClass="modalPopup" Style="display: none;">
                    <table class="TableStyleNone">
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:Label ID="Label1" runat="server" Text="新增"></asp:Label>
                                &nbsp;
                            </td>
                        </tr>
                      <tr id="tr5" runat="server">
                            <td class="style3">
                                身分證字號
                            </td>
                            <td class="style1" style="text-align: left;">
                                <asp:TextBox ID="txt_IDCard_Insert" runat="server" AutoPostBack="True" 
                                    ontextchanged="txt_IDCard_Insert_TextChanged"></asp:TextBox>
                                
                            </td>
                        </tr>
                      
                        <tr id="tr1" runat="server">
                            <td class="style3">
                                機關
                            </td>
                            <td class="style1" style="text-align: left;">
                                <asp:Label ID="lb_Ad_Insert" runat="server" Text="請先輸入身分證字號"></asp:Label>
                             
                               <%-- <asp:DropDownList ID="ddlIAd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlIAd_SelectedIndexChanged"
                                    Width="150px">
                                </asp:DropDownList>--%>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td class="style4">
                                單位
                            </td>
                            <td style="text-align: left;" class="style5">
                                <asp:Label ID="lb_Unit_Insert" runat="server" Text="請先輸入身分證字號"></asp:Label>
                                
                               <%-- <asp:DropDownList ID="ddlIUnit" runat="server" Width="150px" OnSelectedIndexChanged="ddlIUnit_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem>請先選擇機關</asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                        </tr>
                        <tr id="tr2">
                            <td class="style4">
                                人員
                            </td>
                            <td class="style5" style="text-align: left;">
                              <%--  <asp:DropDownList ID="ddlIPeople" runat="server" Width="150px">
                                    <asp:ListItem>請先選擇機關單位</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;--%><asp:Label ID="lb_Name_Insert" runat="server" Text="請先輸入身分證字號"></asp:Label>
                                <%--<asp:TextBox ID="txt_NAME_Insert" runat="server" MaxLength="10" Width="100px" Visible="False"
                                    ReadOnly="True"></asp:TextBox>--%>
                            </td>
                        </tr>
                        
                        
                        
                        <tr>
                            <td class="style4">
                                日期區間
                            </td>
                            <td style="text-align: left;" class="style5">
                                &nbsp;&nbsp; 起
                                <asp:TextBox ID="tbIStart" runat="server" Width="80px" MaxLength="7"></asp:TextBox>
                                &nbsp; 至&nbsp;
                                <asp:TextBox ID="tbIEnd" runat="server" Width="80px" MaxLength="7"></asp:TextBox>
                                <span class="style6">(範例：1011001)</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                                備註
                            </td>
                            <td style="text-align: left;" class="style5">
                                <asp:TextBox ID="tbIMemo" runat="server" Width="280px" Height="42px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="新增" />
                                &nbsp;<asp:Button ID="btnICancel" runat="server" Text="取消" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                </div>
                <div style="text-align: right;">
                    <asp:Button ID="btnI" runat="server" Text="新增" Style="display: none;" />
                    <asp:Button ID="btnAdd" runat="server" Text="新增" OnClick="btnAdd_Click" />
                    &nbsp;
                    <cc1:ModalPopupExtender ID="btnI_ModalPopupExtender" runat="server" DynamicServicePath=""
                        Enabled="True" TargetControlID="btnI" PopupControlID="pnlInsert" BackgroundCssClass="modalBackground"
                        CancelControlID="btnICancel">
                    </cc1:ModalPopupExtender>
                    &nbsp;
                    <asp:Button ID="btnS" runat="server" Text="查詢個人"  />
                    <cc1:ModalPopupExtender ID="btnS_ModalPopupExtender" runat="server" DynamicServicePath=""
                        Enabled="True" TargetControlID="btnS" PopupControlID="pnlSearch" BackgroundCssClass="modalBackground"
                        >
                    </cc1:ModalPopupExtender>
                </div>
                <div>
                    <center>
                        <table cellpadding="1px" border="1px">
                            <tr>
                                <th class="style3">
                                    機關
                                </th>
                                <td class="style7" style="text-align: left;">
                                    <asp:DropDownList ID="ddlSAd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSAd_SelectedIndexChanged"
                                        Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="style4">
                                    單位
                                </th>
                                <td style="text-align: left;" class="style8">
                                    <asp:DropDownList ID="ddlSUnit" runat="server" Width="150px" OnSelectedIndexChanged="ddlSUnit_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem>請先選擇機關</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    確認狀態
                                </th>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList ID="rblBack" runat="server" RepeatDirection="Horizontal" CssClass="rdlst"
                                        OnSelectedIndexChanged="rblBack_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem  Value="2">全部</asp:ListItem>
                                        <asp:ListItem Value="0">未確認</asp:ListItem>
                                        <asp:ListItem Value="1">確認</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div>
                    <br />
                    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="SN,MZ_ID,SDATE,EDATE,MEMO" EmptyDataText="該條件下無資料" ForeColor="#333333"
                        GridLines="None" OnRowCommand="gvData_RowCommand" Width="95%">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="SN" HeaderText="編號">
                                <ItemStyle Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="mz_name" HeaderText="姓名">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="sdate" DataFormatString="{0:d}" HeaderText="日期起">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="edate" DataFormatString="{0:d}" HeaderText="日期迄">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="memo" HeaderText="備註">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="back" HeaderText="確認">
                                <ItemStyle Width="8%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="upddate" HeaderText="修改日期">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="upduser" HeaderText="修改人">
                                <ItemStyle Width="8%" />
                            </asp:BoundField>
                            <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="編輯" Text="修改">
                                <ItemStyle Width="8%" />
                            </asp:ButtonField>
                            <asp:TemplateField HeaderText="確認" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SN") %>'
                                        OnClientClick="return confirm(&quot;確認日期已到？&quot;)" OnCommand="Button1_Command"
                                        Text="確認" />
                                </ItemTemplate>
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
