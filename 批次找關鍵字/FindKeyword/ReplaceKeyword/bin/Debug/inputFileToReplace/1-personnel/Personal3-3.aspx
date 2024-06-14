<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal3-3.aspx.cs" Inherits="TPPDDB._1_personnel.Personal3_3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet"  type="text/css" />
    <style type="text/css">
        .style10
        {
            text-align: left;
        }       
        .style32
        {
            text-align: left;
            font-family: �з���;
            font-size: large;
            font-weight: 700;
        }      
        
        
        .style40
        {
            text-align: left;
            width: 152px;
        }
        .style42
        {
            text-align: left;
            width: 157px;
        }
        
        .style49
        {
            text-align: left;
            width: 355px;
        }       
        .style111
        {
            border: solid 0px;
        }        
        .style117
        {
            text-align: left;
            width: 344px;
        }
        .style122
        {
            text-align: left;
            width: 167px;
        }      
        
        .style143
        {
            text-align: right;
        }
        
        .style152
        {
            text-align: left;
            width: 34px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style153
        {
            text-align: left;
            width: 40px;
            height: 20px;
            background-color: #FFCCFF;
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table style="background-color: #6699FF; color: White; width: 100%">
        <tr>
            <td class="style32">
                ��K�O�ק�B��J
            </td>
            <td class="style143">
                <asp:Label ID="Label1" runat="server" CssClass="style32" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server">
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px">
                            �����Ҹ�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px" Style="text-align: left"
                                MaxLength="10" AutoPostBack="True" OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            �m �W
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="110px" MaxLength="12"></asp:TextBox>
                        </td>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style10">
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" OnClick="Button1_Click"
                                Text="�m�W�d�߿�J" Font-Bold="True" ForeColor="Maroon" />
                        </td>
                        <td class="KEY_IN_TITLE">
                            ��¾���
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_ADATE" runat="server" MaxLength="9" OnTextChanged="TextBox_MZ_ADATE_TextChanged"
                                Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px">
                            �o����
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="70px" MaxLength="9" OnTextChanged="TextBox_MZ_DATE_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            �ͮĤ��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" Width="70px" MaxLength="9" OnTextChanged="TextBox_MZ_IDATE_TextChanged"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            �o��帹
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_PRID" runat="server" MaxLength="24" Width="110px"></asp:TextBox>
                            <asp:Button ID="btPRID" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btPRID_Click" TabIndex="-1" Text="V" />
                        </td>
                        <td class="KEY_IN_TITLE">
                            �r��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="10" Style="margin-left: 0px"
                                Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px">
                            �֩w����
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_MZ_BPRID" runat="server" DataSourceID="SqlDataSource3"
                                DataTextField="MZ_PRID" DataValueField="MZ_AD" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_PRID,MZ_AD FROM A_CHKAD " DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                        <td class="KEY_IN_TITLE">
                            �֩w�帹
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_BPRID1" runat="server" MaxLength="10" Width="110px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_MZ_BPRID1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_BPRID1">
                            </cc1:FilteredTextBoxExtender>
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="TextBox_MZ_BPRID2" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_MZ_BPRID2_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_BPRID2">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="KEY_IN_TITLE">
                            �֩w���
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_BDATE" runat="server" MaxLength="9" Width="65px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_BDATE_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px">
                            ������
                        </td>
                        <td class="style117">
                            <asp:TextBox ID="TextBox_MZ_EXOAD" runat="server" AutoPostBack="True" MaxLength="10"
                                OnTextChanged="TextBox_MZ_EXOAD_TextChanged" Width="75px"></asp:TextBox>
                            <asp:Button ID="btEXOAD" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXOAD_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXOAD1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            �����
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_EXOUNIT" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_EXOUNIT_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btEXOUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXOUNIT_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXOUNIT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="105px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px">
                            ¾�ȦC��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_EXORANK" runat="server" Width="30px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXORANK_TextChanged"></asp:TextBox>
                            <asp:Button ID="btEXORANK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btEXORANK_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXORANK_1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1"></asp:TextBox>
                            ��
                            <asp:TextBox ID="TextBox_MZ_EXRANK1" runat="server" AutoPostBack="True" MaxLength="3"
                                OnTextChanged="TextBox_MZ_EXRANK1_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btEXRANK1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXRANK1_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXRANK1_1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="120px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE">
                            ¾�Ƚs��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_EXOPOS" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px">
                            ¾ ��
                        </td>
                        <td class="style122">
                            <asp:TextBox ID="TextBox_MZ_EXOOCC" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_EXOOCC_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXOOCC_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXOOCC1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="90px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            �O�_�ݥ�
                        </td>
                        <td class="style10">
                            <asp:RadioButtonList ID="RadioButtonList_MZ_ISEXTPOS" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="RadioButtonList_MZ_ISEXTPOS_SelectedIndexChanged" RepeatColumns="2">
                                <asp:ListItem Value="Y">�O</asp:ListItem>
                                <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="style152">
                            �� ��
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="DropDownList_MZ_EXTPOS" runat="server" Enabled="False" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@91'" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            �԰ϰϤ�
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="DropDownList_MZ_EXPNO1" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">�@��</asp:ListItem>
                                <asp:ListItem Value="2">�c��</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style153">
                            ��԰�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_EXOPNO" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                            �԰�
                        </td>
                    </tr>
                </table>
                
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            �s�����
                        </td>
                        <td class="style49">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �s����
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            ¾�ȦC��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_RANK" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_RANK_TextChanged"></asp:TextBox>
                            <asp:Button ID="btRANK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btRANK_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_RANK_1" runat="server" CssClass="style111" Width="90px"
                                TabIndex="-1"></asp:TextBox>
                            ��
                            <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" AutoPostBack="True" MaxLength="3"
                                OnTextChanged="TextBox_MZ_RANK1_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btRANK1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btRANK1_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_RANK1_1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="90px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ¾ �t
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_CHISI" runat="server" Width="35px" MaxLength="4" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_CHISI_TextChanged"></asp:TextBox>
                            <asp:Button ID="btCHISI" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btCHISI_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_CHISI1" runat="server" CssClass="style111" Width="90px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ¾ ��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="90px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            ¾ ��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" MaxLength="4" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_OCCC_TextChanged"></asp:TextBox>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btOCCC_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �O�_�ݥ�
                        </td>
                        <td class="style10">
                            <asp:RadioButtonList ID="RadioButtonList_MZ_ISEXTPOS2" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="RadioButtonList_MZ_ISEXTPOS2_SelectedIndexChanged" RepeatColumns="2">
                                <asp:ListItem Value="Y">�O</asp:ListItem>
                                <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �� ��
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="DropDownList_MZ_EXTPOS2" runat="server" Enabled="False" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@91'" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �Ϥ�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_PESN" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_PESN_TextChanged" Width="25px"></asp:TextBox>
                            <asp:Button ID="btPESN" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btPESN_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_PESN1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            �~��¾��
                        </td>
                        <td class="style32">
                            <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SRANK_TextChanged"></asp:TextBox>
                            <asp:Button ID="btSRANK" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSRANK_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style111" Width="130px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �Ķ�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_SLVC" runat="server" AutoPostBack="True" MaxLength="3"
                                OnTextChanged="TextBox_MZ_SLVC_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btSLVC" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSLVC_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_SLVC1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="100px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ���I
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_SPT" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �Ȥ���I
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_SPT1" MaxLength="4" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            ��¾��]
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_NREA" runat="server" Width="35px" MaxLength="4" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_NREA_TextChanged"></asp:TextBox>
                            <asp:Button ID="btNREA" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btNREA_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_NREA1" runat="server" CssClass="style111" Width="150px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �� ��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_NRT" runat="server" AutoPostBack="True" MaxLength="1"
                                OnTextChanged="TextBox_MZ_NRT_TextChanged" Width="25px"></asp:TextBox>
                            <asp:Button ID="btNRT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btNRT_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_NRT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �D�ޯŧO
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_PCHIEF" runat="server" AutoPostBack="True" MaxLength="1"
                                OnTextChanged="TextBox_MZ_PCHIEF_TextChanged" Width="25px"></asp:TextBox>
                            <asp:Button ID="Button3" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btPCHIEF_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_PCHIEF1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �s��O
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_NIN" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_NIN_TextChanged"
                                Width="25px" MaxLength="1"></asp:TextBox>
                            <asp:Button ID="btNIN" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btNIN_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_NIN1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            ¾�Ƚs��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_POSIND" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ĵ�԰ϧO
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_PNO" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ĵ�԰ϰϤ�
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="DropDownList_MZ_PNO1" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">�@��</asp:ListItem>
                                <asp:ListItem Value="2">�c��</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �S��ĵ��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_TNO" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �԰ϡ]ĵ��^��¾���
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_WDATE" runat="server" Width="70px" MaxLength="9" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_WDATE_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            ��L�ƶ�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_OTH_THING" runat="server" Width="280px"></asp:TextBox>
                            <asp:Button ID="btOTH_THING" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btOTH_THING_Click" TabIndex="-1" Text="V" />
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �Ҩ������
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_CONDITION" runat="server" Width="280px"></asp:TextBox>
                            <asp:Button ID="btCONDITION" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btCONDITION_Click" TabIndex="-1" Text="V" />
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px">
                            �� ��
                        </td>
                        <td class="style40">
                            <asp:TextBox ID="TextBox_MZ_MEMO" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ����
                        </td>
                        <td class="style42">
                            <asp:TextBox ID="TextBox_MZ_REMARK" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �h��~��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_RET" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ���ɧ_
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_SWT" runat="server" Width="35px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr bgcolor="#CCFFFF">
                        <td>
                            <asp:Button ID="btSearch" runat="server" Text="�d��" OnClick="btSearch_Click" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btInsert" runat="server" OnClick="btInsert_Click" Text="�s�W" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" Text="�ק�" OnClick="btUpdate_Click" CausesValidation="False"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" OnClick="btok_Click" Text="�T�{" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="����" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btDelete" runat="server" Text="�R��" OnClick="btDelete_Click" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" Enabled="False" meta:resourcekey="btUpperResource1"
                                OnClick="btUpper_Click" Text="�W�@��" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="�U�@��" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
