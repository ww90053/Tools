<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_DUTYPEOPLESET.aspx.cs" Inherits="TPPDDB._3_forleave.C_DUTYPEOPLESET" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_NAME FROM A_DLBASE WHERE MZ_AD=@MZ_AD AND MZ_UNIT=@MZ_UNIT">
        <SelectParameters>
            <asp:SessionParameter Name="MZ_AD" SessionField="ADPMZ_EXAD" />
            <asp:SessionParameter Name="MZ_UNIT" SessionField="ADPMZ_EXUNIT" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT ITEM,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C.MZ_ID) AS MZ_NAME, DUTYDATE, MZ_PNO, MZ_CNOBEGIN,MZ_CNOEND,MZ_CNO  FROM 
C_DUTYPEOPLE C WHERE MZ_AD=@MZ_AD AND MZ_UNIT=@MZ_UNIT AND (dbo.to_number(dbo.SUBSTR(DUTYDATE,1,3))+1911)=EXTRACT(YEAR FROM GETDATE()) AND dbo.to_number(dbo.SUBSTR(DUTYDATE,4,2))=EXTRACT(MONTH FROM GETDATE()) AND dbo.to_number(dbo.SUBSTR(DUTYDATE,6,2))=EXTRACT(DAY FROM SYSDATE) ORDER BY ITEM"
        ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}"
        UpdateCommand="UPDATE A_DUTYPEOPLE SET MZ_PNO=@MZ_PNO,MZ_CNOBEGIN=@MZ_CNOBEGIN,MZ_CNOEND=@MZ_CNOEND,MZ_CNO=@MZ_CNO WHERE MZ_ID=@MZ_ID AND MZ_AD=@MZ_AD AND MZ_UNTI=@MZ_UNIT">
        <SelectParameters>
            <asp:SessionParameter Name="MZ_AD" SessionField="ADPMZ_EXAD" />
            <asp:SessionParameter Name="MZ_UNIT" SessionField="ADPMZ_EXUNIT" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="MZ_PNO" />
            <asp:Parameter Name="MZ_CNOBEGIN" />
            <asp:Parameter Name="MZ_CNOEND" />
            <asp:Parameter Name="MZ_CNO" />
            <asp:Parameter Name="MZ_ID" />
            <asp:Parameter Name="MZ_AD" />
            <asp:Parameter Name="MZ_UNIT" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <table class="style10">
        <tr>
            <td class="style8">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="MZ_ID"
                    DataSourceID="SqlDataSource2" GridLines="Vertical" Width="100%" CssClass="style6">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField DataField="ITEM" HeaderText="項次" SortExpression="ITEM" />
                        <asp:TemplateField HeaderText="姓名" SortExpression="MZ_NAME">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MZ_NAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <cc2:ComboBox ID="ComboBox_MZ_NAME" runat="server" RenderMode="Block" AutoCompleteMode="SuggestAppend"
                                    AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="MZ_NAME" DataValueField="MZ_NAME"
                                    MaxLength="0" OnTextChanged="ComboBox1_TextChanged" Width="80px" SelectedValue='<%# Bind("MZ_NAME") %>'
                                    AppendDataBoundItems="True">
                                    <asp:ListItem></asp:ListItem>
                                </cc2:ComboBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="身分證號" SortExpression="MZ_ID">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox_MZ_ID" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_MZ_ID" runat="server" Text='<%# Eval("MZ_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DUTYDATE" HeaderText="勤務日期" SortExpression="MZ_DUTYDATE" />
                        <asp:TemplateField HeaderText="勤區編號" SortExpression="MZ_PNO">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_MZ_PNO" runat="server" Text='<%# Eval("MZ_PNO") %>' Width="100px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="輪番編號起" SortExpression="MZ_CNOBEGIN">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_MZ_CNOBEGIN" runat="server" Text='<%# Eval("MZ_CNOBEGIN") %>'
                                    Width="100px" AutoPostBack="True" OnTextChanged="TextBox_MZ_CNOBEGIN_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="輪番編號迄" SortExpression="MZ_CNOEND">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_MZ_CNOEND" runat="server" Text='<%# Eval("MZ_CNOEND") %>'
                                    Width="100px" AutoPostBack="True" OnTextChanged="TextBox_MZ_CNOEND_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="輪番編號" SortExpression="MZ_CNO">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_MZ_CNO" runat="server" Text='<%# Eval("MZ_CNO") %>' Width="100px"
                                    AutoPostBack="True" OnTextChanged="TextBox_MZ_CNO_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>
                <asp:DataList ID="DataList1" runat="server">
                    <FooterTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>
                    </FooterTemplate>
                </asp:DataList>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" class="style9" />
                <asp:Button ID="bt1" runat="server" OnClick="bt1_Click" Text="產生輪值表" class="style9" />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
