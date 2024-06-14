<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal3-1.aspx.cs" Inherits="TPPDDB._1_personnel.Personal3_1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1
        {
        }
        .style10
        {
            text-align: left;
        }
        .style11
        {
            text-align: left;
        }
        .style12
        {
            text-align: left;
        }
        .style15
        {
            text-align: left;
        }
        .style20
        {
            text-align: left;
        }
        .style26
        {
            text-align: left;
        }
        .style28
        {
            text-align: left;
        }
        .style32
        {
            text-align: left;
        }
        .style34
        {
            text-align: left;
        }
        .style35
        {
            text-align: left;
        }
        .style38
        {
            text-align: left;
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
        .style44
        {
            text-align: left;
        }
        .style49
        {
            text-align: left;
            width: 355px;
        }
        .style57
        {
            text-align: right;
            font-size: large;
            font-family: �з���;
        }
        .style58
        {
            text-align: right;
            height: 14px;
        }
        .style59
        {
            font-size: large;
            text-align: left;
            font-family: �з���;
            height: 14px;
            width: 364px;
        }
        .style110
        {
        }
        .style111
        {
            border: solid 0px;
        }
        .style112
        {
            text-align: left;
            width: 399px;
        }
        .style117
        {
            text-align: left;
        }
        .style122
        {
            text-align: left;
        }
        .style125
        {
            text-align: left;
        }
        .style126
        {
            text-align: left;
        }
        .style133
        {
            text-align: left;
        }
        .style134
        {
            text-align: left;
        }
        .style138
        {
            text-align: left;
        }
        .style139
        {
            text-align: left;
        }
        .style140
        {
            text-align: left;
        }
        .style141
        {
            text-align: left;
        }
        .style142
        {
            text-align: left;
        }
        .style144
        {
            text-align: center;
            width: 59px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style145
        {
            text-align: left;
            width: 30px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style146
        {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            width: 55px;
        }
        .style147
        {
            text-align: left;
            width: 43px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style148
        {
            text-align: left;
            width: 55px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style149
        {
            text-align: left;
            width: 58px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style150
        {
            text-align: left;
            width: 32px;
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
            <table style="background-color: #6699FF; color: White; width: 100%;">
                <tr>
                    <td class="style59">
                        ��K�򥻸��
                    </td>
                    <td class="style58">
                        <asp:Label ID="Label1" runat="server" CssClass="style57" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_Personal3" runat="server">
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style144" style="width: 55px;">
                            �� ��
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_NO" runat="server" Width="120px" MaxLength="15"></asp:TextBox>
                            <asp:CustomValidator ID="CV_NO" runat="server" ControlToValidate="TextBox_MZ_NO"
                                Display="None" OnServerValidate="CV_NO_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <td style="text-align: left" class="KEY_IN_TITLE">
                            �����Ҹ�
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="120px" MaxLength="10" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                            <asp:CustomValidator ID="CV_ID" runat="server" ControlToValidate="TextBox_MZ_ID"
                                Display="None" OnServerValidate="CV_ID_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <td style="text-align: left" class="style150">
                            �m �W
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="100px" MaxLength="12"></asp:TextBox>
                            <asp:CustomValidator ID="CV_NAME" runat="server" ControlToValidate="TextBox_MZ_NAME"
                                Display="None" OnServerValidate="CV_NAME_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="Button1" runat="server" Text="�m�W�d�߿�J" OnClick="Button1_Click" CausesValidation="False"
                                Font-Bold="True" ForeColor="Maroon" />
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            ������
                        </td>
                        <td class="style117">
                            <asp:TextBox ID="TextBox_MZ_EXOAD" runat="server" AutoPostBack="True" MaxLength="10"
                                OnTextChanged="TextBox_MZ_EXOAD_TextChanged" Width="75px"></asp:TextBox>
                            <asp:Button ID="btEXOAD" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXOAD_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXOAD1" runat="server" CssClass="style111" BackColor="White"
                                TabIndex="-1" Width="200px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            �����
                        </td>
                        <td class="style142">
                            <asp:TextBox ID="TextBox_MZ_EXOUNIT" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_EXOUNIT_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btEXOUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXOUNIT_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXOUNIT1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="105px" BackColor="White"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            ¾�ȦC��
                        </td>
                        <td class="style112">
                            <asp:TextBox ID="TextBox_MZ_EXORANK" runat="server" Width="30px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXORANK_TextChanged"></asp:TextBox>
                            <asp:Button ID="btEXORANK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btEXORANK_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXORANK_1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1" BackColor="White"></asp:TextBox>
                            ��
                            <asp:TextBox ID="TextBox_MZ_EXRANK1" runat="server" AutoPostBack="True" MaxLength="3"
                                OnTextChanged="TextBox_MZ_EXRANK1_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btEXRANK1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXRANK1_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXRANK1_1" runat="server" BackColor="White" CssClass="style111"
                                TabIndex="-1" Width="120px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            ¾�Ƚs��
                        </td>
                        <td class="style20">
                            <asp:TextBox ID="TextBox_MZ_EXOPOS" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE" style="width: 55px;">
                            ¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td class="style122">
                            <asp:TextBox ID="TextBox_MZ_EXOOCC" runat="server" AutoPostBack="True" MaxLength="4"
                                OnTextChanged="TextBox_MZ_EXOOCC_TextChanged" Width="35px"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXOOCC_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_EXOOCC1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="120px" BackColor="White"></asp:TextBox>
                        </td>
                        <td class="style149" style="width: 55px;">
                            �O�_�ݥ�
                        </td>
                        <td class="style133">
                            <asp:RadioButtonList ID="RadioButtonList_MZ_ISEXTPOS" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="RadioButtonList_MZ_ISEXTPOS_SelectedIndexChanged" RepeatColumns="2">
                                <asp:ListItem Value="Y">�O</asp:ListItem>
                                <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="style145">
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
                            <asp:CustomValidator ID="CV_EXTPOS" runat="server" ControlToValidate="DropDownList_MZ_EXTPOS"
                                Display="Dynamic" OnServerValidate="CV_EXTPOS_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <td class="style148">
                            �԰ϰϤ�
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="DropDownList_MZ_EXPNO1" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">�@��</asp:ListItem>
                                <asp:ListItem Value="2">�c��</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style147">
                            ��԰�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_EXOPNO" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                            �԰�
                        </td>
                    </tr>
                </table>
                <%--<table style="background-color: #FFFF99; width: 100%; text-align: left;" border="1">
                    <tr>
                        <td class="style16">
                            ��¾��]
                        </td>
                        <td class="style115">
                            <asp:TextBox ID="TextBox_MZ_TBNREA" runat="server" Width="35px" MaxLength="4" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBNREA_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBNREA" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btTBNREA_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBNREA1" runat="server" CssClass="style111" Width="150px"
                                TabIndex="-1" BackColor="#FFFF99"></asp:TextBox>
                        </td>
                        <td class="style116">
                            ��¾�帹
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_TBID" runat="server" Width="250px" MaxLength="36"></asp:TextBox>
                        </td>
                        <td class="style10">
                            ��¾���
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_MZ_TBDATE" runat="server" MaxLength="9" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDATE_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>--%>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
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
                        <td class="style146">
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
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            ¾�ȦC��
                        </td>
                        <td class="style134">
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
                        <td class="style26">
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
                        <td class="style28">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="90px"
                                TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            ¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td class="style126">
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
                        <td class="style125">
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
                            <asp:CustomValidator ID="CV_EXTPOS2" runat="server" ControlToValidate="DropDownList_MZ_EXTPOS2"
                                Display="Dynamic" OnServerValidate="CV_EXTPOS2_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
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
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
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
                        <td class="style34">
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
                        <td class="style35">
                            <asp:TextBox ID="TextBox_MZ_SPT" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �Ȥ���I
                        </td>
                        <td class="style38">
                            <asp:TextBox ID="TextBox_MZ_SPT1" MaxLength="4" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            ��¾��]
                        </td>
                        <td class="style15">
                            <asp:TextBox ID="TextBox_MZ_NREA" runat="server" Width="35px" MaxLength="4" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_NREA_TextChanged"></asp:TextBox>
                            <asp:Button ID="btNREA" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btNREA_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_NREA1" runat="server" CssClass="style111" Width="150px"
                                TabIndex="-1"></asp:TextBox>
                            <asp:CustomValidator ID="CV_NREA" runat="server" Display="None" OnServerValidate="CV_NREA_ServerValidate"
                                ValidateEmptyText="True" ControlToValidate="TextBox_MZ_NREA"></asp:CustomValidator>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �� ��
                        </td>
                        <td class="style15">
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
                        <td class="style15">
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
                        <td class="style15">
                            <asp:TextBox ID="TextBox_MZ_NIN" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_NIN_TextChanged"
                                Width="25px" MaxLength="1"></asp:TextBox>
                            <asp:Button ID="btNIN" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btNIN_Click" TabIndex="-1" Text="V" />
                            <asp:TextBox ID="TextBox_MZ_NIN1" runat="server" CssClass="style111" TabIndex="-1"
                                Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            ¾�Ƚs��
                        </td>
                        <td class="style141">
                            <asp:TextBox ID="TextBox_MZ_POSIND" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ĵ�԰ϧO
                        </td>
                        <td class="style138">
                            <asp:TextBox ID="TextBox_MZ_PNO" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ĵ�԰ϰϤ�
                        </td>
                        <td class="style139">
                            <asp:DropDownList ID="DropDownList_MZ_PNO1" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">�@��</asp:ListItem>
                                <asp:ListItem Value="2">�c��</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �S��ĵ��
                        </td>
                        <td class="style140">
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
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            ��L�ƶ�
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_OTH_THING" runat="server" Width="280px" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="btOTH_THING" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btOTH_THING_Click" TabIndex="-1" Text="V" />
                        </td>
                        <td class="style10">
                            �Ҩ������
                        </td>
                        <td class="style10">
                            <asp:TextBox ID="TextBox_CONDITION" runat="server" Width="280px" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="btCONDITION" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btCONDITION_Click" TabIndex="-1" Text="V" />
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            �� ��
                        </td>
                        <td class="style40">
                            <asp:TextBox ID="TextBox_MZ_MEMO" runat="server"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            ����
                        </td>
                        <td class="style42">
                            <asp:TextBox ID="TextBox_MZ_REMARK" runat="server"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �h��~��
                        </td>
                        <td class="style44">
                            <asp:TextBox ID="TextBox_MZ_RET" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �O�_�ýZ
                        </td>
                        <td class="style44">
                            <asp:TextBox ID="TextBox_MZ_SWT1" runat="server" Width="35px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            �f�֧_
                        </td>
                        <td class="style44">
                            <asp:TextBox ID="TextBox_MZ_SWT4" runat="server" Width="35px"></asp:TextBox>
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
                            <asp:Button ID="btInsert" runat="server" Text="�s�W" OnClick="btInsert_Click" CausesValidation="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" Text="�ק�" OnClick="btUpdate_Click" CausesValidation="False"
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btok" runat="server" OnClick="btok_Click" Text="�T�{" Enabled="False"
                                CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="����" Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btDelete" runat="server" Text="�R��" CssClass="KEY_IN_BUTTON_BLUE"
                                OnClick="btDelete_Click" CausesValidation="False" Enabled="False" 
                                AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="�W�@��" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="�U�@��" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="Button_MZ_SWT4" runat="server" Text="�f��" CausesValidation="False"
                                OnClick="Button_MZ_SWT4_Click" CssClass="KEY_IN_BUTTON_RED" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
