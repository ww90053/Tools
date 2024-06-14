<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ReviewManagement.aspx.cs" Inherits="TPPDDB._3_forleave.C_ReviewManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
      <link href="style/SearchControl.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .style58
        {
            color: #FF0000;
            font-weight: bold;
            background-color: #FFFF99;
            text-align: left;
            width: 55px;
        }
        .style59
        {
            text-align: left;
            width: 55px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style60
        {
            text-align: left;
            width: 55px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style61
        {
            text-align: left;
            width: 24px;
            height: 20px;
            background-color: #FFCCFF;
        }
    </style>
   
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="Label2" Text="差假審核資格管理" runat="server" class="style8"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" class="style8"></asp:Label>
                            <asp:Label ID="lb_count"  runat="server"  Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style5">
                            身分證號
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="True" MaxLength="10"
                                Width="75px" OnTextChanged="TextBox_MZ_ID_TextChanged" Style="height: 19px"></asp:TextBox>
                        </td>
                        <td class="style1">
                            姓名
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" Font-Bold="True"
                                OnClick="Button1_Click" Text="姓名查詢" />
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style1">
                            現服機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_EXAD" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            現服單位
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_EXUNIT" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            職稱
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_OCCC" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            層級設定
                        </td>
                        <td class="style3">
                            <asp:RadioButtonList ID="rbl" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text ="單位承辦人" Value="1" ></asp:ListItem>
                            <asp:ListItem Text ="單位主管" Value="2"></asp:ListItem>
                            <asp:ListItem Text ="勤務中心值日官" Value="2"></asp:ListItem>
                            <asp:ListItem Text ="差勤承辦人(人事室)" Value="4"></asp:ListItem>
                            <asp:ListItem Text ="加班承辦人(人事室)" Value="5"></asp:ListItem>
                            </asp:RadioButtonList>
                            
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="btsearch" runat="server" Text="查詢" CausesValidation="False" class="style9"
                                OnClick="Btsearch_Click" />
                            <asp:Button ID="btInsert" runat="server" Text="新增" AccessKey="a" CausesValidation="False"
                                class="style9" OnClick="btInsert_Click" />
                            <asp:Button ID="btUpdate" runat="server" Text="修改" Style="width: 40px" CausesValidation="False"
                                class="style9" OnClick="btUpdate_Click" Enabled="False" />
                            <asp:Button ID="btOK" runat="server" Text="確定" class="style9" OnClick="btOK_Click"
                                Enabled="False" />
                            <asp:Button ID="btCancel" runat="server" Text="取消" CausesValidation="False" class="style9"
                                OnClick="btCancel_Click" Enabled="False" />
                            <asp:Button ID="btDelete" runat="server" Text="刪除" AccessKey="d" CausesValidation="False"
                                class="style9" OnClick="btDelete_Click" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" />
                        
                            <asp:Button ID="btGroup_Update" runat="server" OnClick="btGroup_Update_Click" Text="整批修改"
                                class="style13" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="MZ_ID,SN,REVIEW_LEVEL" ForeColor="#333333" GridLines="None"
                    Width="100%" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                    OnPageIndexChanging="GridView1_PageIndexChanging" EmptyDataText="查無資料"  PageSize="10">
                    <Columns>
                        <asp:BoundField DataField="MZ_ID" HeaderText="身份證字號" SortExpression="MZ_ID" />
                        <asp:BoundField DataField="NAME" HeaderText="姓名" SortExpression="NAME" />
                        <asp:BoundField DataField="MZ_EXAD_CH" HeaderText="現服機關" SortExpression="MZ_EXAD" />
                        <asp:BoundField DataField="MZ_EXUNIT_CH" HeaderText="現服單位" SortExpression="MZ_EXUNIT" />
                        <asp:BoundField DataField="MZ_OCCC_CH" HeaderText="職稱" SortExpression="MZ_OCCC" />
                        <asp:BoundField DataField="REVIEW_LEVEL" HeaderText="層級" SortExpression="REVIEW_LEVEL" />
                        <asp:TemplateField HeaderText=" 科長/主任">
                            <ItemTemplate>
                                <asp:CheckBox ID="ck_isTopManager" Checked='<%# Eval("TOP_MANAGER").ToString()=="Y"?true:false %>'
                                    AutoPostBack="true" runat="server" OnCheckedChanged="ck_isTopManager_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField CommandName="SELECT" Text="按鈕" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
              
            </asp:Panel>
            
            <cc1:ModalPopupExtender ID="btn_search_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btsearch" PopupControlID="pl_search" BackgroundCssClass="modalBackground" >
                </cc1:ModalPopupExtender>
            
            <asp:Panel ID="pl_search" runat="server" Style="display: none; " CssClass="DivPanel" >
            <table class="style52" border="1" >
                <tr>
                   <td class="style102">
                        姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:     
                   </td>
                    <td class="style101">
                        <asp:TextBox ID="txt_search_NAME" runat="server"></asp:TextBox>
                   </td>
                </tr>
                <tr>
                   <td class="style102">
                            身分證號:
                   </td>
                   <td class="style101">
                   <asp:TextBox ID="txt_search_ID" runat="server"></asp:TextBox> 
                   </td>
                </tr>
                <tr>
                  <td class="style102">
                            機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關:
                  </td>
                  <td class="style101">                 
                      <asp:DropDownList ID="DropDownList_MZ_EXAD" runat="server"                         
                          onselectedindexchanged="DropDownList_MZ_EXAD_SelectedIndexChanged" 
                          AutoPostBack="True">
                      </asp:DropDownList>                    
                  </td>
                </tr> 
                <tr>
                     <td class="style102">
                            單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位:
                     </td>
                     <td class="style101">        
                         <asp:DropDownList ID="DropDownList_MZ_EXUNIT" runat="server"  class="style55">
                         </asp:DropDownList>
                     
                     </td>
                </tr> 
            </table>
            <table class="style52" border="1">
                    <tr>
                        <td>
                        
                            <asp:Button ID="txt_search_OK" runat="server" Text="確定" class="style53" 
                                onclick="txt_search_OK_Click"      />
                            &nbsp;&nbsp;&nbsp;
                            
                        
                            <asp:Button ID="txt_search_cancel" runat="server" CausesValidation="False" Text="離開" 
                                class="style54" />
                            
                        
                        </td>
                    </tr>
             </table>       
            </asp:Panel>
            
            
             <cc1:ModalPopupExtender ID="pl_SearchName_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="Button1" PopupControlID="pl_SearchName" BackgroundCssClass="modalBackground" >
                </cc1:ModalPopupExtender>
             <asp:Panel ID="pl_SearchName" runat="server" Style="display: none; " CssClass="DivPanel" >
                <table border="1" class="style52"  >
                    <tr>
                        <td class="style102">
                            姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:
                        </td>
                        <td class="style101">
                            <asp:TextBox ID="txt_SearchName_NAME" runat="server" Width="120px" MaxLength="6" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style102">
                            服務機關:
                        </td>
                        <td class="style101">
                            <asp:DropDownList ID="ddl_SearchName_MZ_EXAD" runat="server" AppendDataBoundItems="True"
                                AutoPostBack="True" 
                                onselectedindexchanged="ddl_SearchName_MZ_EXAD_SelectedIndexChanged" >
                            </asp:DropDownList>
                           
                        </td>
                    </tr>
                    <tr>
                        <td class="style102">
                            服務單位
                        </td>
                        <td class="style101">
                            <asp:DropDownList ID="ddl_SearchName_MZ_EXUNIT" runat="server" Width="100px" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>              
                <table style="width: 100%; text-align: right;">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="btn_SearchName_OK" runat="server"  Text="確定" 
                                onclick="btn_SearchName_OK_Click"  />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_SearchName_cancel" runat="server"  Text="離開" 
                                onclick="btn_SearchName_cancel_Click"  />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4" PageSize="10" AllowPaging="true"
                                GridLines="None" OnRowCommand="GridView2_RowCommand" Width="100%" OnPageIndexChanging="GridView2_PageIndexChanging"
                                Style="text-align: left" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                                    
                                    <asp:BoundField HeaderText="現服機關" DataField="MZ_EXAD" />
                                    <asp:BoundField HeaderText="現服單位" DataField="MZ_EXUNIT" />
                                    <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
