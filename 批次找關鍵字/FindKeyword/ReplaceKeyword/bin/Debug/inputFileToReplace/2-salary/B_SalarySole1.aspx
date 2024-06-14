<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalarySole1.aspx.cs" Inherits="TPPDDB._2_salary.B_SalarySole1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />

    <script src="../1-personnel/jquery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function changeFocus(textbox) {
            if (event.keyCode == 13 || event.keyCode == 9) {
                textbox.focus();
            }
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 158px;
        }
        .modalBackground
        {
            /*彈跳視窗背景暗化*/
            filter: alpha(opacity=70); /*for IE*/
            opacity: 0.7; /*for Other*/
            background-color: #E0E0E0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      <ContentTemplate>
            <div class="PageTitle">

                <script src="jsUpdateProgress.js" type="text/javascript"></script>

                <script type="text/javascript" language="javascript">
                    var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>'; 
                </script>

                單一發放資料建立
            </div>
           
            
            <table class="TableStyleBlue" style="width: 95%;">
                <tr>
                    <th>
                        <a class="must">*入帳日期</a>
                    </th>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonList_DA_INOUT_GROUP" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList_DA_INOUT_GROUP_SelectedIndexChanged"
                            Style="display: none;">
                            <asp:ListItem Selected="True" Value="IN">入帳</asp:ListItem>
                            <asp:ListItem Value="OUT">沖銷</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:TextBox ID="TextBox_DA" runat="server" Width="70px" MaxLength="7" ></asp:TextBox>依日期查詢輸入：0980101
                    </td><%--OnTextChanged="TextBox_DA_TextChanged1"
                            AutoPostBack="True"--%>
                    <th>
                        <a class="must">*入帳批號</a>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txt_caseid" runat="server" MaxLength="2" Width="30px" AutoPostBack="True"
                            OnTextChanged="txt_caseid_TextChanged1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <a class="must">*發放項目</a>
                    </th>
                    <td align="left">
                        <asp:TextBox ID="txt_num" AutoPostBack="True" runat="server" MaxLength="2" Width="30px"
                            OnTextChanged="txt_num_TextChanged"></asp:TextBox>
                        <asp:Label ID="lbl_num" runat="server" Text=""></asp:Label>
                        <asp:Button ID="btn_showNum" runat="server" Text="V" OnClick="btn_showNum_Click"
                            TabIndex="-1" />
                    </td>
                    <th>
                        所得
                    </th>
                    <td colspan="3">
                        <asp:CheckBox ID="CheckBox_TAXFLAG" runat="server" Enabled="False" Text="納入所得" />
                        <asp:DropDownList ID="ddl_taxid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_taxid_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_taxid1" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        項目說明
                    </th>
                    <td align="left" colspan="5">
                        <asp:TextBox ID="TextBox_NOTE" runat="server" Width="70%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <a class="must">*員工編號</a>
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" AutoPostBack="True" MaxLength="8"
                            OnTextChanged="TextBox_MZ_POLNO_TextChanged" Width="80px"></asp:TextBox>
                    </td>
                    <th>
                        <a class="must">*身分證號/統一編號</a>
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_IDCARD" runat="server" AutoPostBack="True" MaxLength="10"
                            OnTextChanged="TextBox_IDCARD_TextChanged" Width="100px"></asp:TextBox>
                    </td>
                    <th>
                        <a class="must">*姓名/抬頭名稱</a>
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" AutoPostBack="True" MaxLength="5"
                            OnTextChanged="TextBox_MZ_NAME_TextChanged" Width="80px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        應發金額
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_PAY" runat="server" Width="50px" ></asp:TextBox><%--OnTextChanged="TextBox_PAY_TextChanged"
                            AutoPostBack="True"--%>
                    </td>
                    <th>
                        所得稅
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_TAX" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <th>
                        自提基金
                    </th>
                    <td>
                        <asp:TextBox ID="TextBoxSaveUNTax" runat="server" Width="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        健保費
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_PAY1" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <th>
                        勞保費
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_PAY2" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <th>
                        自提離職儲金
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox_PAY3" runat="server" Width="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        法院扣款
                    </th>
                    <td>
                        <asp:TextBox ID="txt_extra01" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <th>
                        二代健保補充保費
                    </th>
                    
                    <td>
                        <asp:TextBox ID="txt_Secand_HealthyPay" runat="server" Width="50px" ></asp:TextBox>
                    </td>
                    <th>
                        外聘人員
                    </th>
                    
                    <td>
                        <asp:DropDownList ID="ddl_External" runat="server">
                        <asp:ListItem  Text="否" Value="N"></asp:ListItem>
                        <asp:ListItem  Text="是" Value ="Y"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div style="text-align: center;">
                <asp:Button ID="btCreate" runat="server" Text="新增" OnClick="btCreate_Click" AccessKey="A" />
                <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" />
                <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click" OnClientClick="return confirm('確定要刪除？')" />
                <asp:Button ID="btExit" runat="server" Text="取消" OnClick="btExit_Click" />
                <!-- 彈跳視窗-->
                <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                    Width="200px" Height="100px" Style="display: none;">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                        DisplayAfter="300">
                        <ProgressTemplate>
                            <div style="position: relative; top: 40%; text-align: center;">
                                <br />
                                <img src="/images/loading.gif" style="vertical-align: middle" alt="Processing" />
                                <br />
                                處理中 ...
                                <br />
                              
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </asp:Panel>
                <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                    Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <!-- 彈跳視窗-->
            </div>
            <asp:Panel ID="Panel_ADD" runat="server" GroupingText="新增列表">
                <table class="TableStyleBlue">
                    <tr>
                        <th>
                            應發總計
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_CREATE_IN" runat="server" Width="100px" Enabled="False"></asp:TextBox>
                        </td>
                        <th>
                            應扣總計
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_CREATE_DE" runat="server" Width="100px" Enabled="False"></asp:TextBox>
                        </td>
                        <th>
                            實發總計
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_CREATE_REAL" runat="server" Width="100px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <cc2:TBGridView ID="TBGridView_Create" runat="server" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                        CellPadding="3" EmptyDataText="無新增資料" EnableEmptyContentRender="True" GridLines="Vertical"
                        Width="100%" DataKeyNames="S_SNID" AllowPaging="True" OnPageIndexChanging="TBGridView_Create_PageIndexChanging"
                        OnRowCommand="TBGridView_Create_RowCommand">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btn_tempUpdate" runat="server" Text="選取" CommandName="sel" CommandArgument='<%# Eval("S_SNID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                            <asp:BoundField DataField="DA" HeaderText="日期" />
                            <asp:BoundField DataField="PAY" HeaderText="應發金額" />
                            <asp:BoundField DataField="DES" HeaderText="應扣金額" />
                            <asp:BoundField DataField="TAX" HeaderText="所得稅" />
                            <asp:BoundField DataField="REAL" HeaderText="實發金額" />
                            <asp:BoundField DataField="NUM" HeaderText="項目說明" />
                        </Columns>
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </cc2:TBGridView>
                </div>
            </asp:Panel>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" />
                <asp:Panel ID="pl_Selector" runat="server" Width="500px" Height="300px" Style="display: none;"
                    CssClass="DivPanel">
                    <div style="width: 100%; text-align: right;">
                        <asp:ImageButton ID="btn_Cancel" runat="server" ImageUrl="~/images/Back.gif" OnClick="btn_Cancel_Click" />
                    </div>
                    <asp:GridView ID="gv_Target" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_Target_RowCommand"
                        CssClass="Grid1" Style="width: 100%;" DataKeyNames="IDCARD,MZ_POLNO,NAME">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                            <asp:BoundField DataField="PAY_AD" HeaderText="機關" />
                            <asp:BoundField DataField="UNIT" HeaderText="單位" />
                            <asp:BoundField DataField="IDCARD" HeaderText="身份證字號" ItemStyle-Width="50px">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MZ_POLNO" HeaderText="員工編號" ItemStyle-Width="60px">
                                <ItemStyle Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NAME" HeaderText="姓名" ItemStyle-Width="50px">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </asp:Panel>
                <cc1:ModalPopupExtender ID="pl_Selector_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="Button1" BackgroundCssClass="modalBackground"
                    PopupControlID="pl_Selector">
                </cc1:ModalPopupExtender>
            </div>
            <div>
                <asp:Button ID="btn_showNumInv" runat="server" Text="Button" Style="display: none;"
                    TabIndex="-1" />
                <cc1:ModalPopupExtender ID="btn_showNum_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btn_showNumInv" PopupControlID="pl_num" CancelControlID="ibt_numClose"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_num" runat="server" CssClass="Grid1" Style="width: 350px; display: none;">
                    <div style="text-align: right; width: 95%;">
                        <asp:ImageButton ID="ibt_numClose" runat="server" ImageUrl="~/images/Back.gif" />
                    </div>
                    <div>
                        <asp:GridView ID="gv_num" runat="server" OnRowCommand="gv_num_RowCommand" AllowPaging="True"
                            OnPageIndexChanging="gv_num_PageIndexChanging" Style="width: 95%;" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="選取" CommandName="select" CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="代碼" DataField="ID" />
                                <asp:BoundField HeaderText="名稱" DataField="NAME" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="txt_caseid" />
            <asp:PostBackTrigger ControlID="TextBox_MZ_POLNO" />
            <asp:PostBackTrigger ControlID="TextBox_IDCARD" />
            <asp:PostBackTrigger ControlID="TextBox_MZ_NAME" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
