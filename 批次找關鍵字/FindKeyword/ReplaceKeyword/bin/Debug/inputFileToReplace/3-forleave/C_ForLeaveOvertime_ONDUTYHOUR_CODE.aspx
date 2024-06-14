<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_ONDUTYHOUR_CODE.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_ONDUTYHOUR_CODE" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            <asp:Label ID="Label1" runat="server"  style="TEXT-ALIGN:left; ></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" >
                    <tr>
                        <th class="style7" >
                            輪值種類
                        </th>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_Duty_TYPE" runat="server" Width="100px" MaxLength="250"></asp:TextBox>
                       
      
                        </td>
                        
                    </tr>
                    
                    <tr>
                    <td class="style3" >
                     <asp:Button ID="btn_newType" runat="server" CausesValidation="False" OnClick="btnewType_Click"
                                Text="新增種類" class="style9" AccessKey="a" />
                     </td>
                            
                    
                    </tr>
                    </table>
                    
                     <table class="style10">
                    <tr>
                        
                        <td class="style8"  >
                            <asp:Label ID="lb_set" runat="server" Text="值日設定" style="TEXT-ALIGN:left;></asp:Label>
                            </td>
                        
                    </tr>
                </table>
                    
                    <table class="style6">
                    
                    
                    <tr>
                        <td class="style7" >
                            輪值種類
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="dp_TYPE" runat="server" >
                                
                            </asp:DropDownList>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style7" >
                            值勤別
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="DP_Duty_KIND" runat="server" Width="100px" MaxLength="250">
                                
                            </asp:TextBox>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style7" >
                            值日時間
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTY_HOUR" runat="server" Width="100px" MaxLength="250"></asp:TextBox>
                            (單位為小時)
                        </td>
                    </tr>
                    <tr>
                        <td class="style7" >
                            值日費用
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTY_PAY" runat="server" Width="100px" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" BackColor="#CCFFFF">
                <table class="style12">
                    <tr>
                        <td>
                         <asp:Button ID="btSearch" runat="server" CausesValidation="False" OnClick="btSearch_Click"
                                Text="查詢" class="style9" AccessKey="a" />
                            <asp:Button ID="btInsert" runat="server" CausesValidation="False" OnClick="btInsert_Click"
                                Text="新增" class="style9" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" CausesValidation="False" OnClick="btUpdate_Click"
                                Text="修改" Enabled="False" class="style9" />
                            <asp:Button ID="btOK" runat="server" CausesValidation="False" OnClick="btOK_Click"
                                Text="確定" Enabled="False" class="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" OnClick="btCancel_Click"
                                Text="取消" Enabled="False" class="style9" />
                            <asp:Button ID="btDelete" runat="server" CausesValidation="False" OnClick="btDelete_Click"
                                Text="刪除" OnClientClick="return confirm(&quot;確定刪除？&quot;);" Enabled="False"
                                class="style9" AccessKey="d" />
                        </td>
                    </tr>
                </table>
                <cc2:TBGridView ID="GridView1" runat="server" Width="100%" CellPadding="4" EnableEmptyContentRender="True"
                    GridLines="None" AutoGenerateColumns="False"  DataKeyNames="DUTY_KIND"
                    OnRowCommand="GridView1_RowCommand" ForeColor="#333333" 
                    ondatabound="GridView1_DataBound">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="DUTY_KINDNAME" HeaderText="輪值種類" ReadOnly="True"  />
                        <asp:BoundField DataField="DUTY_KINDNAME1" HeaderText="值勤別"  />
                        <asp:BoundField DataField="DUTY_HOUR" HeaderText="值日時間"  />
                        <asp:BoundField DataField="DUTY_PAY" HeaderText="值日費"  />
                       <%-- <asp:BoundField DataField="DUTY_KIND" HeaderText="值日編號" Visible="false"  />--%>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                        
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc2:TBGridView>
              
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
