<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="Personal_Efficiency1.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Efficiency1" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
        }
        .style3
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table border="1" class="style1">
            <tr>
                <td class="style2">
                    年度</td>
                <td class="style3">
                    <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="40px"></asp:TextBox>
                </td>
                <td>
                    身分證號</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    服務單位</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    職稱</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    職等</td>
                <td>
                    <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" Width="100%" Height="400px">
            <cc1:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" EmptyDataText="無資料" EnableEmptyContentRender="True" 
                GridLines="None" Width="100%" DataSourceID="SqlDataSource1" 
                AllowPaging="True" onrowcommand="GridView1_RowCommand" 
                ondatabound="GridView1_DataBound" onrowdatabound="GridView1_RowDataBound" 
                ForeColor="#333333">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" SortExpression="MZ_ID" />
                    <asp:BoundField HeaderText="姓名" DataField="MZ_NAME" />
                    <asp:BoundField HeaderText="職序" DataField="MZ_TBDV" />
                    <asp:TemplateField HeaderText="分數">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MZ_NUM") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox_POINT" runat="server" AutoPostBack="true" 
                                ontextchanged="TextBox_POINT_TextChanged" Text='<%# Bind("MZ_NUM") %>' 
                                Width="40px" MaxLength="2"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_POINT_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="TextBox_POINT">
                            </cc2:FilteredTextBoxExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="嘉獎" DataField="MZ_P4001" />
                    <asp:BoundField HeaderText="記功" DataField="MZ_P4010" />
                    <asp:BoundField HeaderText="大功" DataField="MZ_P4100" />
                    <asp:BoundField HeaderText="申誡" DataField="MZ_P5001" />
                    <asp:BoundField HeaderText="記過" DataField="MZ_P5010" />
                    <asp:BoundField HeaderText="大過" DataField="MZ_P5100" />
                    <asp:BoundField HeaderText="事假" DataField="MZ_CODE01" />
                    <asp:BoundField HeaderText="病假" DataField="MZ_CODE02" />
                    <asp:BoundField HeaderText="曠職" DataField="MZ_CODE18" />
                    <asp:TemplateField HeaderText="是否參加">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList_MZ_SWT" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_MZ_SWT_SelectedIndexChanged" 
                                SelectedValue='<%# BIND("MZ_SWT") %>'>
                                <asp:ListItem Value="0">參加</asp:ListItem>
                                <asp:ListItem Value="1">不參加</asp:ListItem>
                                <asp:ListItem Value="2">其他</asp:ListItem>
                                <asp:ListItem Value="3">另考</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
                <PagerTemplate>
                    <asp:LinkButton ID="LinkFirst" runat="server" onclick="Quick_Click">第一頁</asp:LinkButton>
                    　<asp:LinkButton ID="LinkPrevious" runat="server" onclick="Quick_Click">上一頁</asp:LinkButton>
                    　<asp:LinkButton ID="LinkNext" runat="server" onclick="Quick_Click">下一頁</asp:LinkButton>
                    　<asp:LinkButton ID="LinkLast" runat="server" onclick="Quick_Click">最後頁</asp:LinkButton>
                    　<asp:DropDownList ID="ddlPageJump" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlPageJump_SelectedIndexChanged">
                    </asp:DropDownList>
                    /<asp:Label ID="lbAllPage" runat="server"></asp:Label>
                </PagerTemplate>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </cc1:TBGridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>" 
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>">
                
            </asp:SqlDataSource>
        </asp:Panel>
    
    <asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2" >
                  <table style="background-color:#6699FF; color:White; width:100%">
                      <tr>
                          <td class="style34">
                              <asp:Button ID="btSearch" runat="server" CssClass="style40" 
                                  meta:resourcekey="btP37_DLBASETableResource1" onclick="btSearch_Click" 
                                  Text="查詢" CausesValidation="False" />
                             
                             
                          </td>
                      </tr>
                  </table>
            </asp:Panel>
            </ContentTemplate>
    </asp:UpdatePanel>
    
    
</asp:Content>
