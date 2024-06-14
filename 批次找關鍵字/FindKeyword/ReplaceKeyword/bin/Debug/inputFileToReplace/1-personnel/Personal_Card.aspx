<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Card.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Card" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: center;
            width: 60px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style111
        {
            border-style: solid;
            border-color: inherit;
            border-width: 0px;
            background-color: #FFFFFF;
        }
        .style12
        {
        }
        .style33
        {
            text-align: right;
        }
        .style116
        {
            text-align: center;
            width: 61px;
            background-color: #FFCCFF;
        }
        .style117
        {
            text-align: center;
            width: 67px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        .style118
        {
            width: 327px;
        }
        .style119
        {
            width: 326px;
        }.style124
        {
            text-align: left;
            width: 94px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            height: 29px;
        }.modalBackground
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
        .popup_outside
        {
            border: solid 1px gray;
            background-color: LightBlue;
        }
        .popup_inside
        {
            margin: 10px;
            background-color: White;
        }
        .modalPopup
        {
            border: 3px solid White;
            background-color: White;
            padding: 3px;
        }
    </style>
    <%--  <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_Card" runat="server">
                <div>
                    <div style="width: 100%;">
                        <table style="background-color: #6699FF; color: White; width: 100%; font-size: large;
                            font-family: 標楷體; text-align: left;">
                            <tr>
                                <td class="style112">
                                    服務證管理
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="Label1" runat="server" CssClass="style33"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left; width: 83%;">
                        <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                            <tr>
                                <td class="style116">
                                    身分證號
                                </td>
                                <td class="style119">
                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_ID_TextChanged"
                                        Width="100px" MaxLength="10"></asp:TextBox>
                                    <asp:CustomValidator ID="CV_ID" runat="server" Display="Dynamic" OnServerValidate="CV_ID_ServerValidate"
                                        ValidateEmptyText="True" ControlToValidate="TextBox_MZ_ID"></asp:CustomValidator>
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="style26"
                                        Enabled="False" Font-Bold="True" ForeColor="Maroon" OnClick="Button2_Click" Text="姓名查詢（輸入）"
                                        Width="123px" TabIndex="-1" />
                                </td>
                                <td class="style116">
                                    姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Height="19px" Width="100px" MaxLength="12"
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style116">
                                    編制機關
                                </td>
                                <td class="style119">
                                    <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                        MaxLength="10"></asp:TextBox>
                                    <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                        TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                        TabIndex="-1" BackColor="White" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                                <td class="style116">
                                    編制單位
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                                    <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                        OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                                        TabIndex="-1" BackColor="White" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style116">
                                    現服機關
                                </td>
                                <td class="style119">
                                    <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" Width="75px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_EXAD_TextChanged" MaxLength="10"></asp:TextBox>
                                    <asp:Button ID="btEXAD" runat="server" CausesValidation="False" CssClass="style110"
                                        OnClick="btEXAD_Click" Text="V" TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" CssClass="style111" Width="200px"
                                        TabIndex="-1" BackColor="White" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                                <td class="style116">
                                    現服單位
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" Width="35px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_EXUNIT_TextChanged" MaxLength="4"></asp:TextBox>
                                    <asp:Button ID="btEXUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                        OnClick="btEXUNIT_Click" Text="V" TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" CssClass="style111" Width="105px"
                                        TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style116">
                                    官職等起
                                </td>
                                <td class="style119">
                                    <asp:TextBox ID="TextBox_MZ_RANK" runat="server" Width="35px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_RANK_TextChanged" CssClass="style112"></asp:TextBox>
                                    <asp:Button ID="btRANK" runat="server" CausesValidation="False" CssClass="style110"
                                        Text="V" OnClick="btRANK_Click" TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_RANK_1" runat="server" CssClass="style111" Width="120px"
                                        TabIndex="-1" BackColor="White" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                                <td class="style116">
                                    官職等迄
                                </td>
                                <td class="style2">
                                    <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" Width="35px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_RANK1_TextChanged" MaxLength="3" Style="height: 19px"></asp:TextBox>
                                    <asp:Button ID="btRANK1" runat="server" CausesValidation="False" CssClass="style110"
                                        Text="V" OnClick="btRANK1_Click" TabIndex="-1" />
                                    <asp:TextBox ID="TextBox_MZ_RANK1_1" runat="server" CssClass="style111" Width="120px"
                                        TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style116">
                                    職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 稱
                                </td>
                                <td class="style119">
                                    <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" AutoPostBack="True" MaxLength="4"
                                        OnTextChanged="TextBox_MZ_OCCC_TextChanged" Width="35px"></asp:TextBox>
                                    <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                        OnClick="btOCCC_Click" TabIndex="-1" Text="V" />
                                    <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" BackColor="White" CssClass="style111"
                                        TabIndex="-1" Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="style2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: right; width: 17%">
                        <asp:Image ID="Image1" runat="server" Height="130px" ImageUrl="~/1-personnel/images/nopic.jpg"
                            meta:resourcekey="Image1Resource1" Width="101px" Style="text-align: left" />
                    </div>
                </div>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                    <tr>
                        <td class="style1">
                            服務證號
                        </td>
                        <td class="style118">
                            <asp:TextBox ID="TextBox_MZ_IDNO" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style117">
                            服務證類別
                        </td>
                        <td class="style12">
                            <asp:DropDownList ID="DropDownList_MZ_SWT" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">1.行政警察</asp:ListItem>
                                <asp:ListItem Value="2">2.刑事警察</asp:ListItem>
                                <asp:ListItem Value="3">3.外事警察</asp:ListItem>
                                <asp:ListItem Value="4">4.一般行政</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            發證原因
                        </td>
                        <td class="style118">
                            <asp:DropDownList ID="DropDownList_MZ_INO" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">1.換發</asp:ListItem>
                                <asp:ListItem Value="2">2.補發</asp:ListItem>
                                <asp:ListItem Value="3">3.註銷</asp:ListItem>
                                <asp:ListItem Value="4">4.繳回</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style117">
                            職&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 稱
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="TextBox_OCCC" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            發證日期
                        </td>
                        <td class="style118">
                            <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" OnTextChanged="TextBox_MZ_DATE1_TextChanged"
                                Width="65px" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="style117">
                            徽&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 號
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="TextBox_MZ_NO1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            繳回日期
                        </td>
                        <td class="style118">
                            <asp:TextBox ID="TextBox_MZ_ODATE" runat="server" OnTextChanged="TextBox_MZ_ODATE_TextChanged"
                                Width="65px" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="style117">
                            遺失日期
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="TextBox_MZ_EDATE" runat="server" OnTextChanged="TextBox_MZ_EDATE_TextChanged"
                                Width="65px" AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            備&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 註
                        </td>
                        <td class="style118">
                            <asp:TextBox ID="TextBox_MZ_MEMO" runat="server"></asp:TextBox>
                        </td>
                        <td class="style117">
                            是否已列印
                        </td>
                        <td class="style12">
                            <asp:DropDownList ID="DropDownList_MZ_MEMO1" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="Y">是</asp:ListItem>
                                <asp:ListItem Value="N">否</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td class="style22">
                            <asp:Button ID="btSearch" runat="server" CausesValidation="False" CssClass="style29"
                                OnClick="btSearch_Click" Text="查詢" Font-Bold="True" ForeColor="#000099" />
                            <asp:Button ID="btInsert" runat="server" OnClick="btInsert_Click" Text="新增" CssClass="style28"
                                CausesValidation="False" Font-Bold="True" ForeColor="#000099" 
                                AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" Font-Bold="True" ForeColor="#000099" />
                            <asp:Button ID="btOK" runat="server" Text="確認" OnClick="btOK_Click" Enabled="False"
                                Font-Bold="True" ForeColor="#000099" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Enabled="False" Font-Bold="True" ForeColor="#000099" />
                            <asp:Button ID="btDelete" runat="server" OnClick="btDelete_Click" Text="刪除" CausesValidation="False"
                                OnClientClick="return confirm(&quot;確定刪除？&quot;);" CssClass="style50" Enabled="False"
                                Font-Bold="True" ForeColor="#000099" AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" Font-Bold="True"
                                ForeColor="#000099" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            &nbsp;<asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btNEXTResource1" OnClick="btNEXT_Click" Text="下一筆" Font-Bold="True"
                                ForeColor="#000099" />
                            <asp:Button ID="btExcel" runat="server" CausesValidation="False" Font-Bold="True"
                                ForeColor="#000099" Text="合併列印" OnClick="btExcel_Click" />
                             <asp:Button ID="Button1" runat="server" CausesValidation="False" Font-Bold="True"
                                ForeColor="#000099" Text="領用清冊" OnClick="btExcel1_Click" />
                             <asp:Button ID="Button3" runat="server" OnClick="btMEMO_1_Click" Text="設定已列印" CausesValidation="False"
                                OnClientClick="return confirm(&quot;此功能會將所有資料設為已列印，確定執行？&quot;);" CssClass="style50" 
                                Font-Bold="True" ForeColor="#000099" AccessKey="d" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
               <%-- 合併列印 --%>
                        <asp:Button ID="Button4" runat="server" Text="Button" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="btn_ModalPop" runat="server" DynamicServicePath="" Enabled="True"
                BackgroundCssClass="modalBackground" TargetControlID="Button4" PopupControlID="pl_Excel">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pl_Excel" runat="server" Width="45%" Style="background-color: #FFFFFF;
                display: none;">
                <table width="100%">
                    <tr>
                        <td class="style124">
                            發證日期
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txt_Insert_YEAR" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style124">
                            有效日期
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txt_Exp_DATE" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btExcel_Export" runat="server" Text="匯出" Font-Bold="True" ForeColor="#000099"
                                OnClick="btExcel_Export_Click" />
                            <asp:Button ID="btExcel_Leave" runat="server" Text="離開" Font-Bold="True" ForeColor="#000099"
                                OnClick="btExcel_Leave_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- 領用清冊 --%>
                                    <asp:Button ID="Button5" runat="server" Text="Button" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="btn_ModalPop2" runat="server" DynamicServicePath="" Enabled="True"
                BackgroundCssClass="modalBackground" TargetControlID="Button5" PopupControlID="pl_Excel1">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pl_Excel1" runat="server" Width="45%" Style="background-color: #FFFFFF;
                display: none;">
                <table width="100%">
                     <tr>
                        <td class="style124">
                            現服機關
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DropDownList_Excel_AD" runat="server" DataSourceID="SqlDataSource_AD"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True" AppendDataBoundItems="true">
                                <asp:ListItem Text ="全部" Value="" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                           <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="style124">
                            發證日期
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txt_Insert_YEAR1" runat="server" Width="100px"></asp:TextBox>～<asp:TextBox ID="txt_Insert_YEAR2" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btExcel_Export1" runat="server" Text="匯出" Font-Bold="True" ForeColor="#000099"
                                OnClick="btExcel_Export1_Click" />
                            <asp:Button ID="btExcel_Leave1" runat="server" Text="離開" Font-Bold="True" ForeColor="#000099"
                                OnClick="btExcel_Leave1_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <cc1:TBGridView ID="GridView1" runat="server" Width="100%" CellPadding="4" EnableEmptyContentRender="True"
                GridLines="None" AutoGenerateColumns="False" DataKeyNames="MZ_IDNO,MZ_ID" OnRowCommand="GridView1_RowCommand"
                OnRowDataBound="GridView1_RowDataBound" Style="text-align: left" OnPageIndexChanging="GridView1_PageIndexChanging"
                ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" ReadOnly="True" SortExpression="MZ_ID">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                    <asp:BoundField DataField="MZ_IDNO" HeaderText="服務證號" SortExpression="MZ_IDNO" />
                    <asp:BoundField DataField="MZ_SWT" HeaderText="服務證類別" SortExpression="MZ_SWT" />
                    <asp:BoundField DataField="MZ_INO" HeaderText="發證原因" SortExpression="MZ_INO" />
                    <asp:BoundField DataField="MZ_DATE" HeaderText="發證日期" SortExpression="MZ_IDNO" />
                    <asp:BoundField DataField="MZ_NO1" HeaderText="徽號" SortExpression="MZ_IDNO" />
                    <asp:BoundField DataField="MZ_ODATE" HeaderText="繳回日期" SortExpression="MZ_IDNO" />
                    <asp:BoundField DataField="MZ_EDATE" HeaderText="遺失日期 " SortExpression="MZ_IDNO" />
                    <asp:BoundField DataField="MZ_MEMO" HeaderText="備註" SortExpression="MZ_IDNO" />
                    <asp:BoundField DataField="MZ_MEMO1" HeaderText="列印" SortExpression="MZ_IDNO" />
                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </cc1:TBGridView>
        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="btExcel_Export" />
              <asp:PostBackTrigger ControlID="btExcel_Export1" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
