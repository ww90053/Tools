<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_ONDUTY_DAY.aspx.cs"  Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_ONDUTY_DAY"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
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
            <asp:Panel ID="Panel_ONDUTYHOUR_KEYIN" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                         <%--   <asp:Label ID="Label1" runat="server"></asp:Label>--%>
                        </td>
                    </tr>
                </table>
                
                <table class="style6" border="1">
                    <tr>
                    <td style="text-align: center; width: 60px; color: #0033CC; font-weight: 700; background-color: #FFFF66; font-weight: bold;">
                            輪值月份
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTYMONTH" runat="server" Width="50px" MaxLength="5"></asp:TextBox>(例：0990801)
                        </td>
                    <td class="style5">
                            身分證號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="80px" ></asp:TextBox><%--AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"--%>
                        </td>
                    
                    
                        <td style="text-align: center; width: 40px; color: #0033CC; font-weight: 700; background-color: #FFFF66; font-weight: bold;">
                            人員
                        </td>
                        <td class="style3">
                         <asp:DropDownList ID="DropDownList_MZ_NAME" runat="server" AutoPostBack="True"
                                 OnSelectedIndexChanged="DropDownList_MZ_NAME_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>
                         <td style="text-align: center; width: 40px; color: #0033CC; font-weight: 700; background-color: #FFFF66; font-weight: bold;">
                            單位
                        </td>
                        <td class="style3" width="120px">
                            <asp:Label ID="lb_MZ_UNIT" runat="server" Text="Label"></asp:Label>
                           <%-- <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="105px"></asp:TextBox>--%>
                        </td>
                        <td>
                        
                        <asp:Button ID="btn_search" runat="server" Text="查詢"  class="style9" 
                                onclick="btn_search_Click" />
                                
                        </td>
                        
                        </tr>
                        
                       
                        
                        
                            
                      
                    
                    
                </table>
                  </asp:Panel>
                  
                  
          
                
                <asp:Panel ID="PL_MONTH_DATA" runat="server">
                
                    <asp:DataList ID="DV_MONTH_DATA" runat="server" RepeatColumns="7" 
                        onitemdatabound="DV_MONTH_DATA_ItemDataBound" RepeatDirection="Horizontal"         >
                    <ItemTemplate>
                    <table class="style6" border="1">
                    <tr>
                    <th  style="text-align:center; background-color:#ffff66;  color:#0033cc; font-weight:bold;" width="60px" >
                    
                        <asp:Label ID="lb_DayNo" runat="server" Text='<%# Eval("DATE_TAG").ToString().Substring(5, 2)%>' ToolTip='<%# Eval("DATE_TAG")%>'   Font-Size="16" ></asp:Label>
                    <br>
                    
                    <asp:LinkButton ID="lbtn_DayDetail" runat="server"  Font-Size="12" CommandArgument='<%# Eval("SN") +";"+ Eval("DATE_TAG")+";"+ Eval("IS_CHK") +";" + Eval("IS_HOLIDAY") 
                    +";" + Eval("KIND") +";" + Eval("IS_POSITIVE")+";"+ Eval("IS_CHK_UNIT") %>' OnCommand="lbtn_DayDetail_OnCommand"   >LinkButton</asp:LinkButton>
                    
                    </th>
                    <td width="160px">                
                      
                                        
                    
                    
                    <asp:Label ID="lb_KIND" runat="server" Text='<%# Eval("KIND_CH")%>'  ></asp:Label>
                     <br>
                    <asp:Label ID="lb_POSITIVE" runat="server" Text='<%# Eval("IS_POSITIVE")%>'  ></asp:Label> 
                    <br>
                    <asp:Label ID="lb_OVERTIME_HOUR_INSIDE" runat="server" Text='<%# Eval("IS_OVERTIME_HOUR_INSIDE")%>'  ></asp:Label>
                     
               
                    </td>
                    
                    
                    
                    </tr>
                    </table>
                    
                    
                    
                    </ItemTemplate>
                    </asp:DataList>
                   
                
                </asp:Panel>
                
         
                
                
                
                      <asp:Panel ID="Panel3" runat="server" BackColor="#CCFFFF">
                <table class="style12">
                    <tr>
                        <td>
                        <asp:Button ID="btn_search_show" runat="server" Text="尋找其他單位人員"  class="style9" 
                                onclick="btn_search1_Click"        />
                            </td>
                    </tr>
                </table>
                
              
            </asp:Panel>
                
          
           
            
            
            <asp:Button ID="btn_SET" runat="server" Style="display: none;" />
                <cc1:ModalPopupExtender ID="DV_MONTH_DATA_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True" BehaviorID="DV_MONTH_DATA" TargetControlID="btn_SET" PopupControlID="pl_SET" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_SET" runat="server" Style="display: none;" CssClass="DivPanel">
               
                    
                    <div>
                    
                    
                    <div style="background-color:#1E90FF;   font-weight:bold;">
                    
                  
                      
                    </div>
                    
                    <table cellspacing="0"   >                  
                     <tr  >
                     <td colspan="5" style="background-color:#1E90FF;   font-weight:bold;"    >
                     
                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                         <asp:Label ID="lb_SN" runat="server" Text="" Visible="false"></asp:Label>
                       <asp:Label ID="lb_Title" runat="server" Text="lb_Title"  Font-Size="18" ForeColor="White"></asp:Label>
                     <asp:Label ID="lb_DATE" runat="server" Text="lb_DATE" Font-Size="18" ForeColor="White" ></asp:Label>
                    </td>
                      <td  style="background-color:#1E90FF;   font-weight:bold;"   >
                     <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/Back.gif" style="text-align: right;"  />
                     
                     </td>
                     
                     </tr>
                     <tr>
                     <td>
                    
                     </td>
                    </tr>
                    <tr style ="height: 30px;">
                    
                    <th>
                        假日別
                        </th>
                        <td width="80px" style="text-align:left;" >
                            <asp:DropDownList ID="ddl_holidaykind" runat="server"   AutoPostBack="true" onselectedindexchanged="ddl_holidaykind_SelectedIndexChanged"
                             >
                               
                            </asp:DropDownList>
                            </td>
                            
                            <th>
                        值勤別
                        </th>
                        <td width="80px" style="text-align:left;" >
                            <asp:DropDownList ID="ddl_Kind" runat="server"   AutoPostBack="true"
                                onselectedindexchanged="ddl_Kind_SelectedIndexChanged">
                               
                            </asp:DropDownList>
                            </td>
                            <th>
                        值日費
                        </th>
                      
                        <td style="text-align:left;" ><asp:TextBox ID="txt_pay" runat="server" Enabled ="false" Width="50px"></asp:TextBox></td>
                            
                            
                      </tr> 
                    
                  
                      <tr  style ="height: 20px;">
                      
                      <th>
                        實際值日時間
                        </th>
                      
                        <td colspan="5" style="text-align:left;">
                            <asp:TextBox ID="txt_DATE_START" runat="server" Width="60px"  MaxLength="7"></asp:TextBox>日  <asp:TextBox ID="txt_TIME_START" runat="server" Width="35px" MaxLength="4"></asp:TextBox>分 (例：0830)
                       
                        ~
                           <asp:TextBox ID="txt_DATE_END" runat="server" Width="60px" MaxLength="7" ></asp:TextBox>日 <asp:TextBox ID="txt_TIME_END" runat="server" Width="35px"  MaxLength="4"></asp:TextBox>分 (例：1730)
                        </td>
                          
                      </tr> 
                      <tr>
                    <th>
                        正副值
                        </th>
                      
                        <td colspan="5" style="text-align:left;"  >
                          <asp:RadioButtonList ID="rbl_POSITIVE" runat="server" RepeatDirection="Horizontal">
                          <asp:ListItem Text="正值" Value="1" Selected="True"></asp:ListItem>
                          <asp:ListItem Text="副值" Value="2" ></asp:ListItem>
                          </asp:RadioButtonList>
                        </td>
                        
                      </tr> 
                      
                      
                        <tr>
                    <th>
                        備註
                        </th>
                      
                        <td colspan="5" style="text-align:left;" ><asp:TextBox ID="txt_MEMO" runat="server"  width="400px" ></asp:TextBox></td>
                      </tr> 
                      
                       <tr>
                    <th>
                        值日加班補休
                        </th>
                      
                        <td colspan="5" style="text-align:left;"  >
                         <asp:RadioButtonList ID="rbl_OVERTIME_HOUR_INSIDE" runat="server"  
                                RepeatDirection="Horizontal"   ><%--AutoPostBack="true"
                                onselectedindexchanged="rbl_OVERTIME_HOUR_INSIDE_SelectedIndexChanged"--%>
                          <asp:ListItem Text="是" Value="Y"></asp:ListItem>
                          <asp:ListItem Text="否" Value="N" Selected="True" ></asp:ListItem>
                          </asp:RadioButtonList>
                        </td>
                         
                      </tr> 
                      
                      
                      <tr>
                      
                      <td colspan="6" style="text-align:center;">
                          <asp:Button ID="btn_Save" runat="server" Text="儲存" onclick="btn_Save_Click" />
                          <asp:Button ID="btn_Delete" runat="server" Text="刪除" 
                              onclick="btn_Delete_Click" OnClientClick="return confirm(&quot;若有加班補休會一併刪除,確定刪除？&quot;);" />
                      </td>
                      
                      
                      </tr>
                        
                            
                            
                 
                    
                    </table>
                    </div>
                    
               
                
           </asp:Panel>
            
             
            
            <asp:Button ID="btn_Search_Panel" runat="server" Style="display: none;" />
                <cc1:ModalPopupExtender ID="pl_Search_ModalPopupExtender" runat="server" DynamicServicePath=""
                    Enabled="True"  TargetControlID="btn_search_show" PopupControlID="pl_Search" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pl_Search" runat="server" Style="display: none;" CssClass="DivPanel" width="330px" >
               
                    
                    <div >
                    
                    
                    <div  style="text-align: right;">
                    
                   
                                          
                     <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" style="text-align: right;"  />
                      
                    </div>
                  <%--  <p style="font-size:x-large">查詢</p>--%>
                    <table  width="330px"  >                  
                     
                    
                    
                    <tr>
                    
                    <th width="40px">
                        姓名
                        </th>
                        <td width="100px" style="text-align:left;" >
                             <asp:TextBox ID="txt_NAME" runat="server"></asp:TextBox>
                            </td>
                       
                            
                            
                        
                        
                            
                      </tr> 
                    <tr>
                    
                    <th width="40px">
                       身分證號
                        </th>
                        <td width="100px" style="text-align:left;" >
                             <asp:TextBox ID="txt_ID" runat="server"></asp:TextBox>
                            </td>
                       
                            
                          
                        
                            
                      </tr> 
                  
                      <tr>
                      
                      <th width="40px">
                       機關
                        </th>
                      
                        <td width="100px" style="text-align:left;">
                            <asp:DropDownList ID="ddl_EXAD" runat="server" 
                                onselectedindexchanged="ddl_EXAD_SelectedIndexChanged">
                            </asp:DropDownList>
                            </td>
                          
                      </tr> 
                     <tr>
                      
                      <th width="40px">
                       單位
                        </th>
                      
                        <td width="100px" style="text-align:left;">
                            <asp:DropDownList ID="ddl_EXUNIT" runat="server">
                            </asp:DropDownList>
                            </td>
                          
                      </tr> 
                      
                     
                        
                      <tr>
                    
                    <th width="40px">
                        輪值月份
                        </th>
                        <td width="100px" style="text-align:left;" >
                             <asp:TextBox ID="txtdate_search" runat="server"></asp:TextBox>(例：0990801)
                            </td>
                       
                            
                          
                        
                            
                      </tr> 
                       
                      
                       <tr>
                      
                      <td  style="text-align:center;" colspan="2">
                          <asp:Button ID="btn_search2" runat="server" Text="尋找" 
                              onclick="btn_search2_Click"     />
                          
                      </td>
                      
                      
                      </tr>
                 
                    
                    </table>
                    </div>
                    
                </asp:Panel>
            
            
            
        </ContentTemplate>
        <%--<Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddl_type" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>

