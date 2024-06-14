<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_ONDUTY_CHECK_UNIT.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_ONDUTY_CHECK_UNIT" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
    .modalBackground
{
    background-color: Gray;
    filter: alpha(opacity=70);
    opacity: 0.7;
}
.DivPanel
{
    border: medium double #A6D2FF;
    padding: 20px;
    background-color: #DFDFDF;
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="Label1" runat="server" Style="font-family: 標楷體; font-size: large;
                                font-weight: 700"></asp:Label>
                        </td>
                    </tr>
                </table>
                
                
              
                
                
                <asp:Panel ID="Panel3" runat="server" Height="460px" ScrollBars="Both" Width="830">
                    <cc1:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                         EmptyDataText="無資料" EnableEmptyContentRender="True"
                        GridLines="None" Width="100%" ForeColor="#333333" OnRowDataBound="GridView1_RowDataBound">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                                
                           
                           
                            <asp:TemplateField HeaderText="核可否" SortExpression="IS_CHK">                             
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox_CHK1" runat="server" Checked='<%# Eval("IS_CHK_UNIT").ToString()=="Y"?true:false %>' />
                                <asp:Label ID="lb_SN" runat="server" Text='<%# Eval("SN") %>' Visible="false"></asp:Label>
                                 <asp:Label ID="lb_CHK" runat="server" Text='<%# Eval("IS_CHK_UNIT").ToString() %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>     
                                          
                            <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                            <asp:BoundField DataField="DATE_TAG" HeaderText="請假日起" SortExpression="DATE_TAG" />
                            <asp:BoundField DataField="DUTY_KINDNAME" HeaderText="假日別" />   
                            <asp:BoundField DataField="HOURTYPE" HeaderText="值勤別" />                         
                            
                            <asp:BoundField DataField="REAL_DATE" HeaderText="實際值日時間"   />
                            <asp:BoundField DataField="PAY" HeaderText="值日費"  />
                            <asp:BoundField DataField="IS_OVERTIME_HOUR_INSIDE" HeaderText="是否補休"  />
                            <asp:BoundField DataField="IS_POSITIVE" HeaderText="正副值"  />
                             <asp:BoundField DataField="MEMO" HeaderText="備註"   />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </cc1:TBGridView>
                   
                </asp:Panel>
            </asp:Panel>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td>
                        <asp:Button ID="btSearch_Show" runat="server" Text="查詢" OnClick="btSearch_Show_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                        <asp:Button ID="Button_Check" runat="server" Text="確定" OnClick="Button_Check_Click"
                            CssClass="KEY_IN_BUTTON_BLUE" />
                     
                    </td>
                </tr>
            </table>
            
            
            <asp:Button ID="btn_Search_Panel" runat="server" Style="display: none;" />
                <cc1:ModalPopupExtender ID="pl_Search_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True"  TargetControlID="btn_Search_Panel" PopupControlID="pl_Search" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_Search" runat="server" Style="display: none;" CssClass="DivPanel">
               
                    
                    <div >
                    
                    
                    <div  style="text-align: right;">
                    
                   
                                          
                     <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/Back.gif" style="text-align: right;"  />
                      
                    </div>
                
                    <table>                  
                     
                    
                    
                    <tr>
                    
                    <th>
                        姓名
                        </th>
                        <td width="80px" style="text-align:left;" >
                             <asp:TextBox ID="txt_NAME" runat="server"></asp:TextBox>
                            </td>
                       
                            
                            
                        
                        
                            
                      </tr> 
                    <tr>
                    
                    <th>
                       身分證號
                        </th>
                        <td width="80px" style="text-align:left;" >
                             <asp:TextBox ID="txt_ID" runat="server"></asp:TextBox>
                            </td>
                       
                            
                          
                        
                            
                      </tr> 
                  
                      <tr>
                      
                      <th>
                       機關
                        </th>
                      
                        <td width="80px" style="text-align:left;">
                            <asp:DropDownList ID="ddl_EXAD" runat="server" 
                                onselectedindexchanged="ddl_EXAD_SelectedIndexChanged">
                            </asp:DropDownList>
                            </td>
                          
                      </tr> 
                     <tr>
                      
                      <th>
                       單位
                        </th>
                      
                        <td width="80px" style="text-align:left;">
                            <asp:DropDownList ID="ddl_EXUNIT" runat="server">
                            </asp:DropDownList>
                            </td>
                          
                      </tr> 
                      
                      
                        <tr>
                      
                      <th>
                        日期
                        </th>
                      
                        <td  style="text-align:left;">
                            <asp:TextBox ID="txt_DATE1" runat="server" Width="60px"  MaxLength="7"></asp:TextBox>
                       
                        -
                           <asp:TextBox ID="txt_DATE2" runat="server" Width="60px" MaxLength="7" ></asp:TextBox>(例0990131)
                        </td>
                          
                      </tr> 
                      
                       <tr>
                    <th>
                        核可
                        </th>
                      
                        <td  style="text-align:left;"  >
                         <asp:RadioButtonList ID="rbl_chk" runat="server"  RepeatDirection="Horizontal" >                         
                          <asp:ListItem Text="是" Value="Y"></asp:ListItem>
                          <asp:ListItem Text="否" Value="N"  ></asp:ListItem>
                          </asp:RadioButtonList>
                        </td>
                         
                      </tr> 
                      
                       <tr>
                      
                      <td  style="text-align:center;" colspan="2">
                          <asp:Button ID="btn_Search" runat="server" Text="尋找" 
                              onclick="btn_Search_Click" />
                          
                      </td>
                      
                      
                      </tr>
                 
                    
                    </table>
                    </div>
                    
                </asp:Panel>
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

