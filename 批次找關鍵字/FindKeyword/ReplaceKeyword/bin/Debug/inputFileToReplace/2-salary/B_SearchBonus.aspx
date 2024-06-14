<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SearchBonus.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchBonus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
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
    <div class="title_s1">

        
        <script src="jsUpdateProgress.js" type="text/javascript"></script>

        <script type="text/javascript" language="javascript">
            var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>'; 
        </script>

        <asp:Label ID="Label_TITLE" runat="server"></asp:Label>
    </div>
    
     
          <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="6000" >
        </asp:ScriptManager><!--AsyncPostBackTimeout設定伺服器處理時間-->
        
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
           
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
                            <br />
                            產生資料需要數分鐘，請耐心等候
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                Enabled="True" TargetControlID="Panel_Progress" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <!-- 彈跳視窗-->
           
        
    <asp:Panel ID="Panel1" runat="server" Width="417px" GroupingText="在下列條件選擇，欲產生之報表">
       
      
                <table class="style1">
                    <tr>
                        <td style="text-align: right" class="style2">
                        </td>
                        <td style="text-align: left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" style="text-align: right">
                            <asp:Label ID="Label_AD" runat="server" Text="發薪機關："></asp:Label>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCHI"
                                DataValueField="MZ_KCODE">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" style="text-align: right">
                            年度：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="TextBox_YEAR" runat="server" Height="19px" MaxLength="3" Width="85px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_YEAR_FilteredTextBoxExtender7" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_YEAR">
                            </cc1:FilteredTextBoxExtender>
                            &nbsp;年 (民國年，如：098)
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" style="text-align: right">
                            種類：
                        </td>
                        <td style="text-align: left">
                            <asp:RadioButtonList ID="RadioButtonList_TYPE" runat="server" 
                                
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="ALL">全部</asp:ListItem>
                                <asp:ListItem Value="EXCEED">超過投保金額人員</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style2" style="text-align: right">
                            身分證字號：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txt_IDCard" runat="server"  MaxLength="10" ></asp:TextBox>
                          
                            
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button_SEARCH" runat="server" OnClick="Button_SEARCH_Click" Text="產生" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
                
            
                
                
        
    </asp:Panel>  </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
