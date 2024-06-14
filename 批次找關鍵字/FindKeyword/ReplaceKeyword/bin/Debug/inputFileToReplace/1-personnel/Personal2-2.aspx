<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal2-2.aspx.cs" Inherits="TPPDDB._1_personnel.Personal2_2" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1
        {
            text-align: left;
        }
        .style1
        {
            width: 52px;
            text-align: center;
        }        
        .style4
        {
            text-align: center;
        }       
        .style9
        {
            text-align: left;
            font-size: large;
            font-family: �з���;
        }      
        .style11
        {
            text-align: center;
        }
        .style110
        {
            width: 23px;
        }
        .style112
        {
            width: 215px;
        }
        .style113
        {
            width: 61px;
            text-align: left;
            font-weight: 700;
            color: #0033CC;
            background-color: #FF99CC;
        }
        .style114
        {
            width: 192px;
        }
        .style116
        {
            width: 52px;
            text-align: left;
            background-color: #FFFF66;
        }
        .style117
        {
            background-color: #FFFF66;
        }
        .style118
        {
            width: 42px;
            text-align: left;
            font-weight: 700;
            color: #0033CC;
            background-color: #FF99CC;
        }
        .style121
        {
            width: 44px;
            text-align: left;
            font-weight: 700;
            color: #0033CC;
            background-color: #FF99CC;
        }
        .style122
        {
            width: 214px;
        }
        .style123
        {
            width: 193px;
        }
    </style>
   
    
    <%-- <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>   
   
    <table style="background-color: #6699FF; color: White; width: 100%;">
        <tr>
            <td class="style9">
                ���g�O�o���D��
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_PRK1" runat="server">
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            width="53">
                            �֩w����
                        </td>
                        <td align="left" class="style112"> 
                            <asp:DropDownList ID="DropDownList_MZ_CHKAD" runat="server" AutoPostBack="True" CssClass="style6"
                               
                                Enabled="False" AppendDataBoundItems="True" OnSelectedIndexChanged="DropDownList_MZ_CHKAD_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style113">
                            �o��帹
                        </td>
                        <td class="style114">
                            <asp:TextBox ID="TextBox_MZ_PRID" runat="server" Width="100px" MaxLength="24" ReadOnly="True"></asp:TextBox>
                           
                        </td>
                        <td class="style118">
                            �r&nbsp;&nbsp; ��
                        </td>
                        <td> 
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender"  runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                            </cc2:FilteredTextBoxExtender>
                            ��
                           
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td style="text-align: center; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            width="53">
                            �o����
                        </td>
                        <td align="left" class="style122">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" MaxLength="9" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_DATE_TextChanged" Width="75px"></asp:TextBox>
                          
                        </td>
                        <td class="style113">
                            �ͮĤ��
                        </td>
                        <td class="style123">
                            <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" MaxLength="9" Width="75px"></asp:TextBox>
                           
                        </td>
                        <td class="style121">
                            �`�@��
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" Width="70px" ReadOnly="True"></asp:TextBox>
                            ��
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td style="text-align: center; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            width="53">
                            ���K��
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_PWD_NO" runat="server" Width="100px" 
                                ></asp:TextBox>
                        </td>
                        <td style="text-align: center; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            width="53">
                            �t��
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_SPEED_NO" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td style="text-align: center; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            width="53">
                            �ɸ�
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_FILENO" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td style="text-align: center; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            width="53">
                            �ϥΦ~��
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_YEARUSE" runat="server" Width="30px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="style117" style="text-align: left" width="53">
                            �� &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownList_MZ_EXPLAIN0" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>��������ΦU���O�H</asp:ListItem>
                                <asp:ListItem>������ΦU���O�H</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style116">
                            �� &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox_MZ_EXPLAIN1" runat="server" Width="500px"></asp:TextBox>
                            <asp:Button ID="bt_EXPLAIN1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="bt_EXPLAIN1_Click" TabIndex="-1" Text="V" />
                            <asp:Button ID="btAddNote" runat="server" CausesValidation="False" OnClick="btAddNote_Click"
                                OnClientClick="return confirm(&quot;�T�w�s�W�H&quot;);" Text="�s�W���y" Width="60px" ForeColor="Maroon" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style116">
                            �� &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox_MZ_EXPLAIN" runat="server" Height="47px" TextMode="MultiLine"
                                Width="635px" AutoPostBack="True" 
                                OnTextChanged="TextBox_MZ_EXPLAIN_TextChanged"></asp:TextBox>
                            <asp:Button ID="btPRCT1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btPRCT1_Click" TabIndex="-1" Text="V" />
                            <asp:Button ID="btAddNote1" runat="server" CausesValidation="False" OnClick="btAddNote_Click"
                                OnClientClick="return confirm(&quot;�T�w�s�W�H&quot;);" Text="�s�W���y" Width="60px" ForeColor="Maroon" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel3" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style4">
                                <asp:Button ID="Button1" runat="server" Style="text-align: center" Text="�L���g�O" Width="68px"
                                    CausesValidation="False" OnClick="Button1_Click" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="Button3" runat="server" CausesValidation="False" Font-Bold="True"
                                    ForeColor="Red" OnClick="Button3_Click" Style="text-align: center" Text="�L���g�O �Z"
                                    Width="85px" />
                                <asp:Button ID="Button2" runat="server" Text="�L���g��" CausesValidation="False" OnClick="Button2_Click"
                                    Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="Button_IDSearch" runat="server" Text="�ýZ�@�~" OnClick="Button_IDSearch_Click"
                                    CausesValidation="False" Enabled="False" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="BT_GV1" runat="server" CausesValidation="False" OnClick="BT_GV1_Click"
                                    TabIndex="-1" Style="display: none;" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" Height="180px">
                   <!--�Ψӱ����w�ýZ�w���ɪ����.�٦�PRK2-->
                    <cc1:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                         EmptyDataText="�d�L���" EnableEmptyContentRender="True" onpageindexchanging="GridView1_PageIndexChanging"
                        GridLines="None" Width="100%" ForeColor="#333333" AllowPaging="True" 
                        PageSize="6" >
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="MZ_ID" HeaderText="�����Ҹ�" SortExpression="MZ_ID" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="�m�W" SortExpression="MZ_NAME" />
                            <asp:BoundField DataField="MZ_AD" HeaderText="�A�Ⱦ���" SortExpression="MZ_AD" />
                            <asp:BoundField DataField="MZ_UNIT" HeaderText="�A�ȳ��" SortExpression="MZ_UNIT" />
                            <asp:BoundField DataField="MZ_OCCC" HeaderText="¾��" SortExpression="MZ_OCCC" />
                            <asp:BoundField DataField="MZ_SRANK" HeaderText="�x�]¾�^��" SortExpression="MZ_SRANK" />
                            <asp:BoundField DataField="MZ_PRRST" HeaderText="���g���G" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </cc1:TBGridView>
                    <!--�Ψӱ����ýZ�e�٥����ɪ����.�٦�PRKB + �w�ýZ�w���ɪ����.�٦�PRK2-->
                    <asp:GridView ID="GridView2" runat="server" CellPadding="4" GridLines="None" Width="100%"
                        AutoGenerateColumns="False" OnDataBound="GridView2_DataBound" OnPageIndexChanging="GridView2_PageIndexChanging"
                        OnRowDataBound="GridView2_RowDataBound" Visible="False" ForeColor="#333333">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="MZ_NO" HeaderText="MZ_NO" />
                            <asp:BoundField DataField="MZ_PRCT" HeaderText="MZ_PRCT" />
                            <asp:BoundField DataField="MZ_ID" HeaderText="�����Ҹ�" SortExpression="MZ_ID" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="�m�W" SortExpression="MZ_NAME" />
                            <asp:BoundField DataField="MZ_AD" HeaderText="�A�Ⱦ���" SortExpression="MZ_AD" />
                            <asp:BoundField DataField="MZ_UNIT" HeaderText="�A�ȳ��" SortExpression="MZ_UNIT" />
                            <asp:BoundField DataField="MZ_OCCC" HeaderText="¾��" SortExpression="MZ_OCCC" />
                            <asp:BoundField DataField="MZ_SRANK" HeaderText="�x�]¾�^��" SortExpression="MZ_SRANK" />
                            <asp:BoundField DataField="MZ_PRRST" HeaderText="���g���G" />
                        </Columns>
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server">
                    <table style="background-color: #6699FF; color: White; width: 100%">
                        <tr bgcolor="#CCFFFF">
                            <td class="style10 style11">
                                <asp:Button ID="btSearch" runat="server" CssClass="KEY_IN_BUTTON_BLUE" OnClick="btSearch_Click"
                                    Text="�d��" CausesValidation="False" />
                                <asp:Button ID="btInsert" runat="server" OnClick="btInsert_Click" Text="�s�W" CausesValidation="False"
                                    CssClass="KEY_IN_BUTTON_BLUE" />
                                <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="�ק�" Enabled="False"
                                    CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                                <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="�T�w" Enabled="False"
                                    CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                                <asp:Button ID="btDelete" runat="server" OnClick="btDelete_Click" Text="�R��" Enabled="False"
                                    CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                                <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                    OnClick="btCancel_Click" Text="����" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                                <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                    meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="�W�@��" CssClass="KEY_IN_BUTTON_BLUE" />
                                <asp:Label ID="xcount" runat="server" Visible="False" ></asp:Label> <!--ForeColor ="Black" Font-Size ="X-Large" -->
                                <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                    OnClick="btNEXT_Click" Text="�U�@��" CssClass="KEY_IN_BUTTON_BLUE" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
