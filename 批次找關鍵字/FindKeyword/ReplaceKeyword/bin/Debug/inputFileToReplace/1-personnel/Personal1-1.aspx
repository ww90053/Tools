<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal1-1.aspx.cs" Inherits="TPPDDB._1_personnel.Personal1_1" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" ValidateRequest="false"   %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="TPPDDB" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style2 {
            text-align: left;
            margin-left: 0px;
        }

        .style12 {
            text-align: left;
        }

        .style30 {
            text-align: left;
            width: 298px;
        }

        .style34 {
            text-align: center;
        }

        .style35 {
            font-size: large;
            font-family: �з���;
        }

        .style90 {
            width: 42px;
        }

        .style93 {
            text-align: left;
            background-color: #CCFFFF;
        }

        .style94 {
            width: 43px;
        }

        .style96 {
            text-align: right;
        }

        .style97 {
            text-align: left;
            width: 409px;
        }

        .style98 {
            text-align: left;
        }

        .style99 {
            text-align: left;
            width: 41px;
        }

        .style101 {
            text-align: right;
        }

        .style104 {
            text-align: left;
        }

        .style109 {
            text-align: left;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }
        
        .style109 {
            text-align: left;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style110 {
            text-align: center;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style111 {
            border: solid 0px;
        }

        .style118 {
            text-align: left;
        }

        .style119 {
            text-align: left;
        }

        .style132 {
            text-align: left;
        }

        .style134 {
            text-align: left;
        }

        .style136 {
            text-align: left;
        }

        .style138 {
        }

        .style140 {
            text-align: left;
        }

        .style143 {
            width: 44px;
        }

        .style144 {
            width: 41px;
        }

        .style145 {
            width: 50px;
            text-align: left;
        }

        .style153 {
            text-align: left;
            width: 210px;
        }

        .style154 {
            width: 245px;
        }

        .style158 {
            text-align: left;
            width: 77px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style159 {
            text-align: left;
            width: 95px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style160 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style161 {
        }

        .style_tr {
            border: 1px solid maroon;
        }

        .style162 {
            text-align: center;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 48px;
        }

        .style163 {
            text-align: left;
            width: 68px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style164 {
            text-align: left;
            width: 66px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style165 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 32px;
        }

        .style166 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 46px;
        }

        .style167 {
            text-align: left;
            width: 386px;
        }

        .style168 {
            text-align: left;
            width: 40px;
        }
    </style>

    <script src="jquery/IDNO_Vaild.js" type="text/javascript"></script>

    <script src="jquery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        var errorMsg;
        function CV_ID(sender, args) {
            inputValue = args.Value;
            if (inputValue === '') {
                args.IsValid = false;
                sender.innerHTML = "�п�J�����Ҧr��"
            }
            else if (!idChech($(":text[id$=TextBox_MZ_ID]").get(0))) { //idChech�禡�ǤJ�ѼƬ� text �� Dom ����
                args.IsValid = false;
                sender.innerHTML = errorMsg;
            }
        }
    </script>

    <%--<script language="javascript" type="text/javascript">
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
    <div style="width: 100%;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="width: 100%;">
                    <div style="width: 100%;">
                        <table style="background-color: #6699FF; color: White; width: 100%;">
                            <tr>
                                <td class="style97">
                                    <span class="style35">�ӤH�򥻸�ƺ޲z</span>
                                </td>
                                <td class="style96">
                                    <span class="style35">
                                        <asp:Label ID="Label1" runat="server" CssClass="style101" Text="Label" Visible="False"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 83%; height: 135px; float: left;">
                        <asp:Panel ID="Panel_Title" runat="server">
                            <table border="1" style="width: 100%; text-align: left;">
                                <tr>
                                    <td colspan="4">
                                        <table border="1" style="width: 100%; text-align: left;">
                                            <tr>
                                                <td class="style110" style="width: 40px;">�m�W
                                                </td>
                                                <td class="style2" style="width: 130px;">
                                                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Enabled="False" MaxLength="12" OnTextChanged="TextBox_MZ_NAME_TextChanged"
                                                        Width="70px"></asp:TextBox>


                                                    <asp:TextBox ID="txt_Account" runat="server" BackColor="White" CssClass="style111"
                                                        TabIndex="-1" Width="50px" Font-Bold="True" ForeColor="#0033CC" Enabled="False"></asp:TextBox>

                                                </td>

                                                <td class="style110">�����Ҹ�
                                                </td>
                                                <td class="style14" style="width: 85px;">
                                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" Enabled="False" MaxLength="10" meta:resourcekey="TextBox_MZ_IDResource1"
                                                        OnTextChanged="TextBox_MZ_ID_TextChanged" Width="80px"></asp:TextBox>
                                                    <%--<asp:CustomValidator ID="CV_ID" runat="server" ControlToValidate="TextBox_MZ_ID"
                                            Display="Dynamic" ValidateEmptyText="True" ClientValidationFunction="CV_ID"></asp:CustomValidator>--%>
                                                </td>
                                                <td class="style110">�o�~����
                                                </td>
                                                <td class="style161">
                                                    <asp:TextBox ID="TextBox_PAY_AD" runat="server" AutoPostBack="True" Enabled="False"
                                                        MaxLength="10" Width="75px" OnTextChanged="TextBox_PAY_AD_TextChanged"></asp:TextBox>
                                                    <asp:Button ID="btPAY_AD" runat="server" CausesValidation="False" Enabled="False" OnClick="btPAY_AD_Click"
                                                        TabIndex="-1" Text="V" />
                                                    <asp:TextBox ID="TextBox_PAY_AD1" runat="server" BackColor="White" CssClass="style111" Font-Bold="True" ForeColor="#0033CC" TabIndex="-1" Width="115px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="style110">�s�����
                                    </td>
                                    <td class="style161">
                                        <asp:TextBox ID="TextBox_MZ_AD" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="10" OnTextChanged="TextBox_MZ_AD_TextChanged" Width="75px"></asp:TextBox>
                                        <asp:Button ID="btAD" runat="server" CausesValidation="False" Enabled="False" OnClick="btAD_Click"
                                            TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_AD1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style110">�s����
                                    </td>
                                    <td class="style14">
                                        <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="4" OnTextChanged="TextBox_MZ_UNIT_TextChanged" Width="35px"></asp:TextBox>
                                        <asp:Button ID="btUNIT" runat="server" CausesValidation="False" Enabled="False" OnClick="btUNIT_Click"
                                            TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="105px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style110">�{�A����
                                    </td>
                                    <td class="style161">
                                        <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="10" OnTextChanged="TextBox_MZ_EXAD_TextChanged" Width="75px"></asp:TextBox>
                                        <asp:Button ID="btEXAD" runat="server" CausesValidation="False" Enabled="False" OnClick="btEXAD_Click"
                                            TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style110">�{�A���
                                    </td>
                                    <td class="style14">
                                        <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="4" OnTextChanged="TextBox_MZ_EXUNIT_TextChanged" Width="35px"></asp:TextBox>
                                        <asp:Button ID="btEXUNIT" runat="server" CausesValidation="False" Enabled="False"
                                            OnClick="btEXUNIT_Click" TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="105px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                            <tr>
                                <td style="background-color: #CCFFFF;">
                                    <asp:Panel ID="Panel_ButtonChange" runat="server">
                                        <table style="background-color: #CCFFFF; width: 100%; text-align: left;">
                                            <tr>
                                                <td class="style90">
                                                    <asp:Button ID="btBasic" runat="server" CausesValidation="false" OnClick="btBasic_Click"
                                                        TabIndex="-1" Text="��" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style90">
                                                    <asp:Button ID="btNow" runat="server" CausesValidation="false" OnClick="btNow_Click"
                                                        TabIndex="-1" Text="�{¾" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style143">
                                                    <asp:Button ID="btEducation" runat="server" CausesValidation="false" OnClick="btEducation_Click"
                                                        TabIndex="-1" Text="�Ǿ�" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style94">
                                                    <asp:Button ID="btExam" runat="server" CausesValidation="false" OnClick="btExam_Click"
                                                        TabIndex="-1" Text="�Ҹ�" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style144">
                                                    <asp:Button ID="btCareer" runat="server" CausesValidation="false" CssClass="style20"
                                                        OnClick="btCareer_Click" TabIndex="-1" Text="�g��" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style94">
                                                    <asp:Button ID="btefficiency" runat="server" CausesValidation="false" OnClick="btefficiency_Click"
                                                        TabIndex="-1" Text="���Z" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style94">
                                                    <asp:Button ID="btFamily" runat="server" CausesValidation="false" OnClick="btFamily_Click"
                                                        TabIndex="-1" Text="����" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                                <td class="style99">
                                                    <asp:Button ID="btCard" runat="server" CausesValidation="False" OnClick="btCard_Click"
                                                        TabIndex="-1" Text="�A����" Font-Bold="True" ForeColor="Maroon" />
                                                </td>

                                                <td class="style93">
                                                    <asp:Button ID="btefficiency_insert" runat="server" CssClass="style98" TabIndex="-1"
                                                        Text="���Z�פJ" Width="75px" CausesValidation="False" Font-Bold="True"
                                                        ForeColor="Maroon" OnClick="btefficiency_insert_Click" />
                                                    <asp:Button ID="btBankSet" runat="server" CssClass="style98" TabIndex="-1"
                                                        Text="�~��b��" Width="75px" CausesValidation="False" Font-Bold="True"
                                                        ForeColor="Maroon" OnClick="btBankSet_Click" Enabled="False" />

                                                </td>
                                                <td style="background-color: #CCFFFF">
                                                    <asp:Button ID="btPicture" runat="server" CausesValidation="false" OnClick="btPicture_Click"
                                                        TabIndex="-1" Text="�ۤ���J" Font-Bold="True" ForeColor="Maroon" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 17%; height: 135px; float: right;">
                        <div style="width: 83%; height: 135px; float: left;">
                            <asp:Image ID="Image1" runat="server" Height="130px" ImageUrl="~/1-personnel/images/nopic.jpg"
                                meta:resourcekey="Image1Resource1" Width="101px" Style="text-align: left" />
                        </div>
                        <div style="width: 17%; height: 135px; float: right;">
                            <div>
                                <asp:Button ID="btUpperPic" runat="server" Text="��" CausesValidation="False" Enabled="False"
                                    OnClick="btUpperPic_Click" Font-Bold="True" ForeColor="Maroon" />
                            </div>
                            <div>
                                <asp:Button ID="btDeletePic" runat="server" Text="��" CausesValidation="False" OnClick="btDeletePic_Click"
                                    Font-Bold="True" ForeColor="Maroon" />
                            </div>
                            <div>
                                <asp:Button ID="btNextPic" runat="server" Text="��" CausesValidation="False" Enabled="False"
                                    OnClick="btNextPic_Click" Font-Bold="True" ForeColor="Maroon" />
                            </div>
                            <asp:Label ID="xcount1" runat="server" Text="" Visible="False"></asp:Label>
                        </div>
                    </div>
                </div>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <asp:Panel ID="Panel_Basic" runat="server" Height="315px">
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">���u�s��
                                    </td>
                                    <td class="style132">
                                        <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" MaxLength="8" meta:resourcekey="TextBox_MZ_POLNOResource1"
                                            Style="margin-right: 0px; margin-bottom: 0px;" Width="75px"></asp:TextBox>
                                    </td>
                                    <td class="style109">�X�ͤ��
                                    </td>
                                    <td class="style134">
                                        <asp:TextBox ID="TextBox_MZ_BIR" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_BIR_TextChanged" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style158">���¾�~��
                                    </td>
                                    <td class="style134">
                                        <asp:TextBox ID="TextBox_MZ_OFFYY" runat="server" Width="30px"></asp:TextBox>
                                    </td>
                                    <td class="style109">�^��m�W
                                    </td>
                                    <td class="style134">
                                        <asp:TextBox ID="TextBox_MZ_ENAME" runat="server" MaxLength="60" meta:resourceKey="MZ_ENAMEResource1"
                                            Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">��¾���
                                    </td>
                                    <td class="style138">
                                        <asp:TextBox ID="TextBox_MZ_ADATE" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_ADATE_TextChanged" Style="margin-right: 0px; height: 19px;"
                                            Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style159">��ȰV�m������
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_MZ_QUA_DATE" runat="server" AutoPostBack="true" OnTextChanged="TextBox_MZ_QUA_DATE_TextChanged"
                                            Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style164">��(�h)¾��
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_MZ_LDATE" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_LDATE_TextChanged"
                                            Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style164">��(�~)¾��
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_MZ_TDATE" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_TDATE_TextChanged"
                                            Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style166">�_¾��
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_MZ_ODAY" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_ODAY_TextChanged"
                                            Width="65px" Style="margin-left: 0px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109" style="text-align: left;">�y�e(��)
                                    </td>
                                    <td style="text-align: left;" class="style154">
                                        <asp:TextBox ID="TextBox_MZ_TBCD3" runat="server" AutoPostBack="True" MaxLength="2"
                                            OnTextChanged="TextBox_MZ_TBCD3_TextChanged" Width="25px"></asp:TextBox>
                                        <asp:Button ID="btTBCD3" runat="server" CausesValidation="False" OnClick="btTBCD3_Click"
                                            TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_TBCD31" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="75px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        <asp:TextBox ID="TextBox_MZ_CITY" runat="server" MaxLength="8" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left;" class="style165">�B ��
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="DropDownList_MZ_SM" runat="server" AppendDataBoundItems="True"
                                            meta:resourcekey="DropDownList_MZ_SMResource1">
                                            <asp:ListItem meta:resourcekey="ListItemResource3" Value="2">���B</asp:ListItem>
                                            <asp:ListItem meta:resourcekey="ListItemResource4" Value="1">�w�B</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style165" style="text-align: left;">�� �O
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="DropDownList_MZ_SEX" runat="server" AppendDataBoundItems="True"
                                            meta:resourcekey="DropDownList_MZ_SEXResource1">
                                            <asp:ListItem meta:resourcekey="ListItemResource5" Value="1">�k</asp:ListItem>
                                            <asp:ListItem meta:resourcekey="ListItemResource6" Value="2">�k</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style165" style="text-align: left;">�� ��
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="TextBox_MZ_BL" runat="server" MaxLength="2" meta:resourcekey="TextBox_MZ_BLResource1"
                                            Style="margin-right: 0px" Width="30px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">���y�a�}
                                    </td>
                                    <td class="style145">
                                        <asp:TextBox ID="TextBox_MZ_ZONE1" runat="server" MaxLength="3" Width="45px"></asp:TextBox>
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_ADD1" runat="server" MaxLength="120" meta:resourcekey="TextBox_MZ_ADD1Resource1"
                                            Style="margin-right: 0px" Width="450px"></asp:TextBox>
                                    </td>
                                    <td class="style109">��ʹq��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_MOVETEL" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style109">�{�b���
                                    </td>
                                    <td class="style145">
                                        <asp:TextBox ID="TextBox_MZ_ZONE2" runat="server" MaxLength="3" Width="45px"></asp:TextBox>
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_ADD2" runat="server" MaxLength="120" meta:resourcekey="TextBox_MZ_ADD2Resource1"
                                            Style="margin-right: 0px" Width="450px"></asp:TextBox>
                                    </td>
                                    <td class="style109">��v�q��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_PHONE" runat="server" MaxLength="12" meta:resourcekey="TextBox_MZ_PHONEResource1"
                                            Style="margin-right: 0px" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style163">����p���H
                                    </td>
                                    <td class="style146">
                                        <asp:TextBox ID="TextBox_MZ_EMNAM" runat="server" MaxLength="12" meta:resourcekey="TextBox_MZ_EMNAMResource1"
                                            Style="margin-right: 0px; margin-left: 0px;" Width="100px"></asp:TextBox>
                                    </td>
                                    <td class="style159">�p���H�q��(��)
                                    </td>
                                    <td class="style147">
                                        <asp:TextBox ID="TextBox_MZ_PHONO" runat="server" MaxLength="19" meta:resourcekey="TextBox_MZ_PHONOResource1"
                                            Style="margin-right: 0px" Width="100px"></asp:TextBox>
                                    </td>
                                    <td class="style159">�p���H�q��(�v)
                                    </td>
                                    <td class="style150">
                                        <asp:TextBox ID="TextBox_MZ_PHONH" runat="server" MaxLength="12" meta:resourcekey="TextBox_MZ_PHONHResource1"
                                            Width="100px"></asp:TextBox>
                                    </td>
                                    <td class="style159">���O�[�O���A
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="DropDownList_MZ_INSURANCEMODE" runat="server">
                                            <asp:ListItem Selected="True" Value="1">���`</asp:ListItem>
                                            <asp:ListItem Value="2">����</asp:ListItem>
                                            <asp:ListItem Value="3">�b��</asp:ListItem>
                                            <asp:ListItem Value="4">����</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">�L�ЧO
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_ARMYSTATE" runat="server" Width="20px" OnTextChanged="TextBox_MZ_ARMYSTATE_TextChanged"
                                            AutoPostBack="true" MaxLength="1"></asp:TextBox><asp:Button ID="btARMYSTATE" runat="server"
                                                CausesValidation="False" OnClick="btARMYSTATE_Click" TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_ARMYSTATE1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="75px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">�x ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_ARMYRANK" runat="server" Width="20px" MaxLength="1" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_ARMYRANK_TextChanged"></asp:TextBox><asp:Button ID="btARMYRANK"
                                                runat="server" CausesValidation="False" OnClick="btARMYRANK_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_ARMYRANK1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="75px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">�L ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_ARMYKIND" runat="server" Width="20px" MaxLength="2" OnTextChanged="TextBox_MZ_ARMYKIND_TextChanged"
                                            AutoPostBack="true"></asp:TextBox><asp:Button ID="btARMYKIND" runat="server" CausesValidation="False"
                                                OnClick="btARMYKIND_Click" TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_ARMYKIND1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="75px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">�x ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_ARMYCOURSE" runat="server" Width="20px" MaxLength="2"
                                            OnTextChanged="TextBox_MZ_ARMYCOURSE_TextChanged"></asp:TextBox><asp:Button ID="btARMYCOURSE"
                                                runat="server" CausesValidation="False" OnClick="btARMYCOURSE_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_ARMYCOURSE1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="75px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style159">�L�а_�W���
                                    </td>
                                    <td class="style136">
                                        <asp:TextBox ID="TextBox_MZ_SLFDATE" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_SLFDATE_TextChanged" Width="65px"></asp:TextBox>��<asp:TextBox
                                                ID="TextBox_MZ_SLEDATE" runat="server" MaxLength="9" AutoPostBack="true" OnTextChanged="TextBox_MZ_SLEDATE_TextChanged"
                                                Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style158">�������O
                                    </td>
                                    <td class="style104">
                                        <asp:RadioButtonList ID="RadioButtonList_MZ_ABORIGINE" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="RadioButtonList_MZ_ABORIGINE_SelectedIndexChanged" RepeatColumns="2"
                                            Width="116px">
                                            <asp:ListItem Value="Y">�O</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="style104">
                                        <asp:RadioButtonList ID="RadioButtonList_MZ_ABORIGINEKIND" runat="server" Enabled="False"
                                            RepeatColumns="2" Style="margin-left: 0px">
                                            <asp:ListItem Selected="True" Value="1">�s�a</asp:ListItem>
                                            <asp:ListItem Value="2">���a</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="style160">�� �O
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="DropDownList_MZ_ABORIGINENAME" runat="server" AppendDataBoundItems="True"
                                            DataSourceID="SqlDataSource2" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                            Enabled="False">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@90'" DataSourceMode="DataReader"></asp:SqlDataSource>
                                    </td>
                                </tr>

                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">ĵ�԰ϧO
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="DropDownList_MZ_PNO" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="1">�@��</asp:ListItem>
                                            <asp:ListItem Value="2">�c��</asp:ListItem>
                                        </asp:DropDownList>
                                        ��
                                        <asp:TextBox ID="TextBox_MZ_PNO1" runat="server" MaxLength="2" Width="40px"></asp:TextBox>ĵ�԰�
                                    &nbsp;(�d��:��05ĵ�԰�)</td>


                                    <td class="style109">�N�z�԰ϧO
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="DropDownList_MZ_PNO_Second" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="1">�@��</asp:ListItem>
                                            <asp:ListItem Value="2">�c��</asp:ListItem>
                                        </asp:DropDownList>
                                        ��
                                        <asp:TextBox ID="TextBox_MZ_PNO1_Second" runat="server" MaxLength="2" Width="40px"></asp:TextBox>ĵ�԰�
                                     &nbsp;(�d��:��05ĵ�԰�)</td>

                                </tr>

                            </table>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <asp:Panel ID="Panel_Now" runat="server" Height="315px">
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">¾ ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" AutoPostBack="True" MaxLength="4"
                                            OnTextChanged="TextBox_MZ_OCCC_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btOCCC" runat="server" CausesValidation="False" OnClick="btOCCC_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="90px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style109">�D�ޯŧO
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_PCHIEF" runat="server" AutoPostBack="True" MaxLength="1"
                                            OnTextChanged="TextBox_MZ_PCHIEF_TextChanged" Width="20px"></asp:TextBox><asp:Button
                                                ID="btPCHIEF" runat="server" CausesValidation="False" OnClick="btPCHIEF_Click"
                                                TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_PCHIEF1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="80px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style12">
                                        <table>
                                            <tr>
                                                <td>�ݥN¾
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="RadioButtonList_MZ_ISEXTPOS" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" RepeatColumns="2">
                                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                                        <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    </td><td class="style158">�ݥN¾�W��
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="DropDownList_MZ_EXTPOS" runat="server" Enabled="False" DataSourceID="SqlDataSource1"
                                            DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@91'" DataSourceMode="DataReader"></asp:SqlDataSource>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">�x¾���_
                                    </td>
                                    <td class="style167">
                                        <asp:TextBox ID="TextBox_MZ_RANK" runat="server" AutoPostBack="True" MaxLength="3"
                                            OnTextChanged="TextBox_MZ_RANK_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btRANK" runat="server" CausesValidation="False" OnClick="btRANK_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_RANK_1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>��
                                        <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" AutoPostBack="True" MaxLength="3"
                                            OnTextChanged="TextBox_MZ_RANK1_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btRANK1" runat="server" CausesValidation="False" OnClick="btRANK1_Click"
                                                TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_RANK1_1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style158">��D�ޤ��
                                    </td>
                                    <td class="style168">
                                        <asp:TextBox ID="TextBox_MZ_PB2" runat="server" Width="25px"></asp:TextBox>
                                    </td>
                                    <td class="style109">��¾¾��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_EXTPOS_SRANK" runat="server" AutoPostBack="True" MaxLength="3"
                                            OnTextChanged="TextBox_MZ_EXTPOS_SRANK_TextChanged" Width="35px" Enabled="False"></asp:TextBox>
                                        <asp:Button ID="bt_MZ_EXTPOS_SRANK" runat="server" CausesValidation="False" OnClick="bt_MZ_EXTPOS_SRANK_Click"
                                            TabIndex="-1" Text="V" Enabled="False" />
                                        <asp:TextBox ID="TextBox_MZ_EXTPOS_SRANK1" runat="server" CssClass="style111" Font-Bold="True"
                                            ForeColor="#0033CC" TabIndex="-1" Width="60px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">����]
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_TBCD9" runat="server" AutoPostBack="True" MaxLength="4"
                                            OnTextChanged="TextBox_MZ_TBCD9_TextChanged" Width="30px" Height="19px"></asp:TextBox><asp:Button
                                                ID="btTBCD9" runat="server" CausesValidation="False" OnClick="btTBCD9_Click"
                                                TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_TBCD91" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style109">�����
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_FDATE" runat="server" AutoPostBack="True" MaxLength="9"
                                            meta:resourcekey="TextBox_MZ_PHONO0Resource1" OnTextChanged="TextBox_MZ_FDATE_TextChanged"
                                            Style="margin-right: 0px" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style160">¾ ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" AutoPostBack="True" MaxLength="3"
                                            OnTextChanged="TextBox_MZ_TBDV_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btTBDV" runat="server" CausesValidation="False" OnClick="btTBDV_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="120px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style158">��(�h)¾��]
                                    </td>
                                    <td class="style153">
                                        <asp:TextBox ID="TextBox_MZ_NREA" runat="server" AutoPostBack="True" MaxLength="4"
                                            OnTextChanged="TextBox_MZ_NREA_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btNREA" runat="server" CausesValidation="False" OnClick="btNREA_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_NREA1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="140px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style109">�v�z¾��
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_MZ_AHP_RANK" runat="server" AutoPostBack="true" MaxLength="3" Width="35px" OnTextChanged="txt_MZ_AHP_RANK_TextChanged"></asp:TextBox>
                                        <asp:Button ID="btnAHPRANK" runat="server" CausesValidation="false" TabIndex="-1" Text="V" OnClick="btnAHPRANK_Click" />
                                        <asp:TextBox ID="txt_MZ_AHP_RANK1" runat="server" ReadOnly="true" CssClass="style111" TabIndex="-1" Width="100px" Font-Bold="true" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style109">��¾�帹
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_NID" runat="server" MaxLength="36" meta:resourceKey="TextBox_MZ_NIDResource1"
                                            Style="margin-right: 0px" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">�~��¾��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" AutoPostBack="True" MaxLength="3"
                                            OnTextChanged="TextBox_MZ_SRANK_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btSRANK" runat="server" CausesValidation="False" OnClick="btSRANK_Click"
                                                TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="150px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">�� ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_SLVC" runat="server" AutoPostBack="True" MaxLength="3"
                                            OnTextChanged="TextBox_MZ_SLVC_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btSLVC" runat="server" CausesValidation="False" OnClick="btSLVC_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_SLVC1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="150px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">���I
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_SPT" runat="server" MaxLength="4" OnTextChanged="TextBox_MZ_SPT_TextChanged"
                                            Width="50px"></asp:TextBox>
                                    </td>
                                    <td class="style109">�Ȥ���I
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_SPT1" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">�o����
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_DATE_TextChanged" Style="margin-right: 0px" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style109">�ͮĤ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_IDATE_TextChanged" Style="margin-right: 0px" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style109">¾�Ƚs��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_POSIND" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td class="style160">¾ �t
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_CHISI" runat="server" AutoPostBack="True" MaxLength="4"
                                            OnTextChanged="TextBox_MZ_CHISI_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btCHISI" runat="server" CausesValidation="False" OnClick="btCHISI_Click"
                                                TabIndex="-1" Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_CHISI1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="180px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">�H���Ϥ�
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_PESN" runat="server" AutoPostBack="True" MaxLength="2"
                                            OnTextChanged="TextBox_MZ_PESN_TextChanged" Width="17px"></asp:TextBox><asp:Button
                                                ID="btPESN" runat="server" CausesValidation="False" OnClick="btPESN_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_PESN1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="75px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">�� ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_NRT" runat="server" AutoPostBack="True" MaxLength="1"
                                            OnTextChanged="TextBox_MZ_NRT_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btNRT" runat="server" CausesValidation="False" OnClick="btNRT_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_NRT1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="50px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style160">�s��X
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_NIN" runat="server" AutoPostBack="True" MaxLength="1"
                                            OnTextChanged="TextBox_MZ_NIN_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                ID="btNIN" runat="server" CausesValidation="False" OnClick="btNIN_Click" TabIndex="-1"
                                                Text="V" />
                                        <asp:TextBox ID="TextBox_MZ_NIN1" runat="server" CssClass="style111" TabIndex="-1"
                                            Width="50px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style158">�~��ͮĤ�
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="txt_MZ_SALARY_ISDATE" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="txt_MZ_SALARY_ISDATE_TextChanged" Style="margin-right: 0px" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style109">Ų�ѤH��
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="ddl_ISCRIMELAB" runat="server" style="font-weight: 700">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>�O</asp:ListItem>
                                            <asp:ListItem>�_</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                                <tr>
                                    <td class="style109">�S��ĵ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_TNO" runat="server" Width="50px"></asp:TextBox>
                                    </td>
                                    <td class="style159">ĵ�԰Ϩ�¾���
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_MZ_OPEDATE" runat="server" MaxLength="9" AutoPostBack="true"
                                            OnTextChanged="TextBox_MZ_OPEDATE_TextChanged" Style="margin-right: 0px" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style159">�O�_ĵ¾�H��
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="ddl_MZ_ISPOLICE" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="Y">�O</asp:ListItem>
                                            <asp:ListItem Value="N">�_</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style109">��Ϧ~��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_FYEAR" runat="server" MaxLength="2" meta:resourceKey="TextBox_MZ_RETResource1"
                                            Style="margin-right: 0px" Width="20px"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<span style="color: #0033cc; background: #ffff66">�a¾�i��</span><asp:DropDownList ID="dll_MZ_TRAINING" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="Y">�O</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    <td class="style159">�R�O�h��~��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_RET" runat="server" MaxLength="2" meta:resourceKey="TextBox_MZ_RETResource1"
                                            Style="margin-right: 0px" Width="100px"></asp:TextBox>
                                    </td>
                                    <td class="style159">��¾���A
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="dll_MZ_STATUS2" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="Y">�b¾</asp:ListItem>
                                            <asp:ListItem Value="N">�D�b¾</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                        <asp:Panel ID="Panel4" runat="server" Height="315px">
                            <cc3:TBGridView ID="GridView_Edu" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" DataSourceID="SqlDataSource_Edu" EmptyDataText="�d�L���" EnableEmptyContentRender="True"
                                GridLines="None" meta:resourcekey="GridView_EduResource1" PageSize="5" Width="100%"
                                Style="text-align: left" OnRowCommand="GridView_Edu_RowCommand" OnRowDataBound="GridView_Edu_RowDataBound"
                                ForeColor="#333333" DataKeyNames="DEPT">
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="MZ_SCHOOL" HeaderText="�׷~�Ǯ�" SortExpression="MZ_SCHOOL" />
                                    <asp:BoundField DataField="MZ_YEAR" HeaderText="���O" SortExpression="MZ_YEAR" />
                                    <asp:BoundField DataField="MZ_DEPARTMENT" HeaderText="��t" SortExpression="MZ_DEPARTMENT" />
                                    <asp:BoundField DataField="MZ_EDULEVEL" HeaderText="�Ш|�{��" SortExpression="MZ_EDULEVEL" />
                                    <asp:BoundField DataField="MZ_EDUKIND" HeaderText="�׷~���p" SortExpression="MZ_EDUKIND" />
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    <asp:BoundField DataField="SCHOOL" HeaderText="SCHOOL" SortExpression="SCHOOL" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </cc3:TBGridView>
                            <asp:SqlDataSource ID="SqlDataSource_Edu" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT  MZ_SCHOOL AS SCHOOL,MZ_SCHOOL,
                                                                                                                    MZ_DEPARTMENT, MZ_DEPARTMENT DEPT,
                                                                                                                    MZ_YEAR, MZ_BEGINDATE, MZ_EDUCLASS, (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EDULEVEL AND MZ_KTYPE='EDL') AS MZ_EDULEVEL, 
                                                                                                                    (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE= MZ_EDUKIND AND MZ_KTYPE='EDT') AS MZ_EDUKIND, MZ_ENDDATE FROM A_EDUCATION WHERE MZ_ID=@MZ_ID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="TextBox_MZ_ID" Name="MZ_ID" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Panel ID="Panel_TableEdu" runat="server" meta:resourcekey="Panel_TableEduResource1"
                                Visible="False">
                                <table border="1" class="style2" width="100%">
                                    <tr>
                                        <td class="style109">�׷~�Ǯ�
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_SCHOOL" runat="server" AutoPostBack="True" MaxLength="10"
                                                Width="75px" OnTextChanged="TextBox_MZ_SCHOOL_TextChanged" Visible="false"></asp:TextBox>
                                            <asp:Button
                                                ID="btSCHOOL" runat="server" CausesValidation="False" OnClick="btSCHOOL_Click"
                                                TabIndex="-1" Text="V" Visible="false" />
                                            <asp:TextBox ID="TextBox_MZ_SCHOOL1" runat="server" TabIndex="-1" AutoPostBack="true"
                                                Width="450px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�Ш|���O
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_EDUCLASS" runat="server" AutoPostBack="True" MaxLength="1"
                                                Width="35px" OnTextChanged="TextBox_MZ_EDUCLASS_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btEDUCLASS" runat="server" CausesValidation="False" OnClick="btEDUCLASS_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_MZ_EDUCLASS1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�Ш|�{��
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_EDULEVEL" runat="server" AutoPostBack="True" MaxLength="2"
                                                Width="35px" Height="19px" OnTextChanged="TextBox_MZ_EDULEVEL_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btEDULEVEL" runat="server" CausesValidation="False" OnClick="btEDULEVEL_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_MZ_EDULEVEL1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�� �t
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_DEPARTMENT" runat="server" AutoPostBack="True" MaxLength="3"
                                                Width="50px" OnTextChanged="TextBox_MZ_DEPARTMENT_TextChanged" Enabled="True"
                                                Height="19px" Visible="false"></asp:TextBox>
                                            <asp:Button ID="btDEPARTMENT" runat="server" CausesValidation="False"
                                                OnClick="btDEPARTMENT_Click" TabIndex="-1" Text="V" Enabled="True" Visible="false" />
                                            <asp:TextBox ID="TextBox_MZ_DEPARTMENT1" runat="server" TabIndex="-1" AutoPostBack="true"
                                                Width="250px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�� �O
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" meta:resourcekey="TextBox_MZ_YEAR_NOResource1"
                                                MaxLength="4" Width="50px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�_�W�~��
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_BEGINDATE" runat="server" MaxLength="9" AutoPostBack="true"
                                                Width="65px" OnTextChanged="TextBox_MZ_BEGINDATE_TextChanged"></asp:TextBox><asp:RequiredFieldValidator
                                                    ID="RF_BEGINDATE" runat="server" ErrorMessage="���i�ť�" ControlToValidate="TextBox_MZ_BEGINDATE"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>��<asp:TextBox ID="TextBox_MZ_ENDDATE"
                                                        runat="server" MaxLength="9" AutoPostBack="true" Width="65px" OnTextChanged="TextBox_MZ_ENDDATE_TextChanged"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RF_ENDDATE" runat="server" ErrorMessage="���i�ť�" ControlToValidate="TextBox_MZ_ENDDATE"
                                                            Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;&nbsp; (Ex. 072/01/01 9�X)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�׷~���p
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_MZ_EDUKIND" runat="server" AutoPostBack="True" MaxLength="2"
                                                Width="35px" OnTextChanged="TextBox_MZ_EDUKIND_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btEDUKIND" runat="server" CausesValidation="False" OnClick="btEDUKIND_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_MZ_EDUKIND1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View4" runat="server">
                        <asp:Panel ID="Panel5" runat="server" Height="315px">
                            <cc3:TBGridView ID="GridView_Exam" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" DataSourceID="SqlDataSource_Exam" EmptyDataText="�d�L���" EnableEmptyContentRender="True"
                                GridLines="None" PageSize="5" Width="100%" Style="text-align: left" OnRowCommand="GridView_Exam_RowCommand"
                                OnRowDataBound="GridView_Exam_RowDataBound" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="EXAM_YEAR" HeaderText="�Ҹզ~��" SortExpression="EXAM_YEAR">
                                        <ItemStyle Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EXAM_NAME" HeaderText="�ҸզW��" SortExpression="EXAM_NAME">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EXAM_CLASS" HeaderText="�Ҹ�����" SortExpression="EXAM_CLASS">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EXAM_ADMISSION" HeaderText="��������" SortExpression="EXAM_ADMISSION" />
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </cc3:TBGridView>
                            <asp:SqlDataSource ID="SqlDataSource_Exam" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT EXAM_YEAR,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=EXAM_NAME AND MZ_KTYPE='EXK') AS EXAM_NAME,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=EXAM_CLASS AND MZ_KTYPE='EXS') AS EXAM_CLASS,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=EXAM_ADMISSION AND MZ_KTYPE='EXG') AS EXAM_ADMISSION  FROM A_EXAM WHERE MZ_ID = @MZ_ID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="TextBox_MZ_ID" Name="MZ_ID" PropertyName="Text"
                                        Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Panel ID="Panel_Exam" runat="server" Visible="false">
                                <table border="1" class="style2" width="100%">
                                    <tr>
                                        <td class="style109">�~ ��
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_EXAM_YEAR" runat="server" meta:resourcekey="TextBox11Resource1"
                                                Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�� ��
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_EXAM_NAME" runat="server" AutoPostBack="True" MaxLength="6"
                                                Width="35px" OnTextChanged="TextBox_EXAM_NAME_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btEXAM_NAME" runat="server" CausesValidation="False" OnClick="btEXAM_NAME_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_EXAM_NAME1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="400px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�� ��
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_EXAM_CLASS" runat="server" AutoPostBack="True" MaxLength="6"
                                                Width="60px" OnTextChanged="TextBox_EXAM_CLASS_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btEXAM_CLASS" runat="server" CausesValidation="False" OnClick="btEXAM_CLASS_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_EXAM_CLASS1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">��������
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_EXAM_ADMISSION" runat="server" AutoPostBack="True" MaxLength="1"
                                                Width="20px" OnTextChanged="TextBox_EXAM_ADMISSION_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btEXAM_ADMISSION" runat="server" CausesValidation="False" OnClick="btEXAM_ADMISSION_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_EXAM_ADMISSION1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�ҥ�r��
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_EXAM_DOCUMENTS" runat="server" meta:resourcekey="TextBox15Resource1"
                                                MaxLength="30" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View5" runat="server">
                        <asp:Panel ID="Panel8" runat="server" Height="315px">
                            <cc3:TBGridView ID="GridView_Career" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" DataKeyNames="MZ_AD,MZ_UNIT,MZ_ID" DataSourceID="SqlDataSource_career"
                                EmptyDataText="�d�L���" EnableEmptyContentRender="True" GridLines="None" PageSize="5"
                                Width="100%" Style="text-align: left" OnRowCommand="GridView_Career_RowCommand"
                                OnRowDataBound="GridView_Career_RowDataBound" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="MZ_AD1" HeaderText="����" ReadOnly="True" SortExpression="MZ_AD1" />
                                    <asp:BoundField DataField="MZ_UNIT1" HeaderText="���" ReadOnly="True" SortExpression="MZ_UNIT1" />
                                    <asp:BoundField DataField="MZ_OCCC1" HeaderText="¾��1" ReadOnly="True" SortExpression="MZ_OCCC1" />
                                    <asp:BoundField DataField="MZ_AD" HeaderText="�A�Ⱦ���" ReadOnly="True" SortExpression="MZ_AD" />
                                    <asp:BoundField DataField="MZ_UNIT" HeaderText="�A�ȳ��" ReadOnly="True" SortExpression="MZ_UNIT" />
                                    <asp:BoundField DataField="MZ_OCCC" HeaderText="¾��" SortExpression="MZ_OCCC" />
                                    <asp:BoundField DataField="MZ_IDATE" HeaderText="�ͮĤ��" SortExpression="MZ_IDATE" />
                                    <asp:BoundField DataField="MZ_TBDATE" HeaderText="��¾���	" SortExpression="MZ_TBDATE" />
                                    <asp:BoundField DataField="MZ_TBNREA" HeaderText="��¾��]" SortExpression="MZ_TBNREA" />
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </cc3:TBGridView>
                            <asp:SqlDataSource ID="SqlDataSource_career" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_AD AS MZ_AD1,MZ_UNIT AS MZ_UNIT1,MZ_OCCC AS MZ_OCCC1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_AD) AS MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') AS MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26')AS MZ_OCCC,MZ_IDATE,MZ_TBDATE,MZ_TBNREA,MZ_ID  FROM  A_CAREER WHERE MZ_ID=@MZ_ID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="TextBox_MZ_ID" Name="MZ_ID" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Panel ID="Panel_Career" runat="server">
                                <table border="1" style="width: 100%">
                                    <tr>
                                        <td class="style109">�A�Ⱦ���
                                        </td>
                                        <td class="style118">
                                            <asp:TextBox ID="TextBox_CAREER_AD" runat="server" AutoPostBack="True" MaxLength="10"
                                                OnTextChanged="TextBox_CAREER_AD_TextChanged" Width="75px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_AD" runat="server" CausesValidation="False" OnClick="btCAREER_AD_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_AD1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="240px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">�A�ȳ��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_UNIT" runat="server" AutoPostBack="True" MaxLength="4"
                                                OnTextChanged="TextBox_CAREER_UNIT_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_UNIT" runat="server" CausesValidation="False" OnClick="btCAREER_UNIT_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_UNIT1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="100px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">�x¾���_
                                        </td>
                                        <td class="style118">
                                            <asp:TextBox ID="TextBox_CAREER_RANK" runat="server" AutoPostBack="True" MaxLength="3"
                                                OnTextChanged="TextBox_CAREER_RANK_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_RANK" runat="server" CausesValidation="False" OnClick="btCAREER_RANK_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_RANK_1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">�x¾����
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_RANK1" runat="server" AutoPostBack="True" MaxLength="3"
                                                OnTextChanged="TextBox_CAREER_RANK1_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_RANK1" runat="server" CausesValidation="False" OnClick="btCAREER_RANK1_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_RANK1_1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                                        </td>
                                        <td class="style118">
                                            <asp:TextBox ID="TextBox_CAREER_OCCC" runat="server" AutoPostBack="True" MaxLength="4"
                                                OnTextChanged="TextBox_CAREER_OCCC_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_OCCC" runat="server" CausesValidation="False" OnClick="btCAREER_OCCC_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_OCCC1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="120px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_TBDV" runat="server" AutoPostBack="True" MaxLength="3"
                                                OnTextChanged="TextBox_CAREER_TBDV_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_TBDV" runat="server" CausesValidation="False" OnClick="btCAREER_TBDV_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_TBDV1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">¾ �t
                                        </td>
                                        <td class="style118">
                                            <asp:TextBox ID="TextBox_CAREER_CHISI" runat="server" AutoPostBack="True" MaxLength="4"
                                                OnTextChanged="TextBox_CAREER_CHISI_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_CHISI" runat="server" CausesValidation="False" OnClick="btCAREER_CHISI_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_CHISI1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">¾�Ƚs��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_POSIND" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table border="1" style="width: 100%">
                                    <tr>
                                        <td class="style109">�D�ޯŧO
                                        </td>
                                        <td class="style118">
                                            <asp:TextBox ID="TextBox_CAREER_PCHIEF" runat="server" AutoPostBack="True" MaxLength="1"
                                                OnTextChanged="TextBox_CAREER_PCHIEF_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_PCHIEF" runat="server" CausesValidation="False" OnClick="btCAREER_PCHIEF_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_PCHIEF1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="80px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">�O�_��¾
                                        </td>
                                        <td class="style12">
                                            <asp:RadioButtonList ID="RadioButtonList_CAREER_ISEXTPOS" runat="server" RepeatColumns="2">
                                                <asp:ListItem Value="Y">�O</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="style109">��¾�W��
                                        </td>
                                        <td class="style12">
                                            <asp:DropDownList ID="DropDownList_CAREER_EXTPOS" runat="server" AppendDataBoundItems="True"
                                                DataSourceID="SqlDataSource3" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"
                                                Enabled="False">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@91'" DataSourceMode="DataReader"></asp:SqlDataSource>
                                        </td>
                                    </tr>
                                </table>
                                <table border="1" style="width: 100%">
                                    <tr>
                                        <td class="style109">�H���Ϥ�
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_PESN" runat="server" AutoPostBack="True" OnTextChanged="TextBox_CAREER_PESN_TextChanged"
                                                Width="35px"></asp:TextBox><asp:Button ID="btCAREER_PESN" runat="server" CausesValidation="False"
                                                    OnClick="btCAREER_PESN_Click" TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_PESN1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="50px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style160">�s��O
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_NIN" runat="server" AutoPostBack="True" Height="19px"
                                                OnTextChanged="TextBox_CAREER_NIN_TextChanged" Width="35px"></asp:TextBox><asp:Button
                                                    ID="btCAREER_NIN" runat="server" CausesValidation="False" OnClick="btCAREER_NIN_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_NIN1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="50px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style160">�ͱԧO
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_NRT" runat="server" AutoPostBack="True" OnTextChanged="TextBox_CAREER_NRT_TextChanged"
                                                Width="35px"></asp:TextBox><asp:Button ID="btCAREER_NRT" runat="server" CausesValidation="False"
                                                    OnClick="btCAREER_NRT_Click" TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_NRT1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="50px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table border="1" style="width: 100%; text-align: left;">
                                    <tr>
                                        <td class="style109">�o�����
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_DATE" runat="server" OnTextChanged="TextBox_CAREER_DATE_TextChanged"
                                                AutoPostBack="true" Width="65px"></asp:TextBox>
                                        </td>
                                        <td class="style109">�ͮĤ��
                                        </td>
                                        <td class="style119">
                                            <asp:TextBox ID="TextBox_CAREER_IDATE" runat="server" OnTextChanged="TextBox_CAREER_IDATE_TextChanged"
                                                AutoPostBack="true" Width="65px"></asp:TextBox>
                                        </td>
                                        <td class="style109">��¾���
                                        </td>
                                        <td class="style109">
                                            <asp:TextBox ID="TextBox_CAREER_ADATE" runat="server" OnTextChanged="TextBox_CAREER_ADATE_TextChanged"
                                                AutoPostBack="true" Width="65px"></asp:TextBox>
                                        </td>
                                        <td class="style109">��¾���
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_CAREER_TBDATE" runat="server" OnTextChanged="TextBox_CAREER_TBDATE_TextChanged"
                                                AutoPostBack="true" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table border="1" style="width: 100%">
                                    <tr>
                                        <td class="style109">��¾��]
                                        </td>
                                        <td class="style140">
                                            <asp:TextBox ID="TextBox_CAREER_NREA" runat="server" AutoPostBack="True" OnTextChanged="TextBox_CAREER_NREA_TextChanged"
                                                Width="35px"></asp:TextBox><asp:Button ID="btCAREER_NREA" runat="server" CausesValidation="False"
                                                    OnClick="btCAREER_NREA_Click" TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_NREA1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">��¾�帹
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_EXID" runat="server" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style109">��¾��]
                                        </td>
                                        <td class="style140">
                                            <asp:TextBox ID="TextBox_CAREER_TBNREA" runat="server" AutoPostBack="True" OnTextChanged="TextBox_CAREER_TBNREA_TextChanged"
                                                Width="35px"></asp:TextBox><asp:Button ID="btCAREER_TBNREA" runat="server" CausesValidation="False"
                                                    OnClick="btCAREER_TBNREA_Click" TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_CAREER_TBNREA1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                        <td class="style109">��¾�帹
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_CAREER_TBID" runat="server" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View6" runat="server">
                        <asp:Panel ID="Panel6" runat="server" Height="315px">
                            <cc3:TBGridView ID="GridView_efficiency" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" EmptyDataText="�d�L���" EnableEmptyContentRender="True" GridLines="None"
                                Width="100%" OnRowCommand="GridView_efficiency_RowCommand" OnRowDataBound="GridView_efficiency_RowDataBound"
                                DataSourceID="SqlDataSource_EFFICIENCY" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField HeaderText="�~��" DataField="T01" SortExpression="T01" />
                                    <asp:BoundField HeaderText="�Ϥ�" DataField="T25" SortExpression="T25" />
                                    <asp:BoundField HeaderText="�`��" DataField="T17" SortExpression="T17" />
                                    <asp:BoundField HeaderText="����" DataField="T26" SortExpression="T26" />
                                    <asp:BoundField HeaderText="�֩w���g" DataField="T18" SortExpression="T18" />
                                    <asp:BoundField HeaderText="�֩w�x¾��" DataField="T19" SortExpression="T19" />
                                    <asp:BoundField HeaderText="�֩w�įš]���^" DataField="T27" SortExpression="T27" />
                                    <asp:BoundField HeaderText="�֩w���I�]�B�^" DataField="T28" SortExpression="T28" />
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </cc3:TBGridView>
                            <asp:SqlDataSource ID="SqlDataSource_EFFICIENCY" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT T01,T25 ,T17 ,T26 ,T18 ,T19 ,T27 ,T28    FROM &quot;A_EFFICIENCY&quot; WHERE (&quot;MZ_ID&quot; = @MZ_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="TextBox_MZ_ID" Name="MZ_ID" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Panel ID="Panel_efficiency" runat="server">
                                <table border="1" class="style12" width="100%">
                                    <tr>
                                        <td class="style159">�~�׿n��
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox" runat="server" meta:resourceKey="TextBox16Resource1"></asp:TextBox><asp:Label
                                                ID="Label_Year" runat="server" Text="Label" Visible="false"></asp:Label>
                                        </td>
                                        <td class="style159">�� ��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox25" runat="server" meta:resourceKey="TextBox25Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�` ��
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox17" runat="server" meta:resourceKey="TextBox17Resource1"></asp:TextBox>
                                        </td>
                                        <td class="style159">�� ��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox26" runat="server" meta:resourceKey="TextBox26Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�֩w����
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox18" runat="server" meta:resourceKey="TextBox18Resource1"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="style12">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�֩w�x¾��
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox19" runat="server" meta:resourceKey="TextBox19Resource1"></asp:TextBox>
                                        </td>
                                        <td class="style159">�֩w�įš]���^
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox27" runat="server" meta:resourceKey="TextBox27Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�֩w���I�]�B�^
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox20" runat="server" meta:resourceKey="TextBox20Resource1"></asp:TextBox>
                                        </td>
                                        <td class="style159">�Ȥ���I�]�B�^
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox28" runat="server" meta:resourceKey="TextBox28Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�֩w���
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox21" runat="server" meta:resourceKey="TextBox21Resource1"
                                                Width="65px"></asp:TextBox>
                                        </td>
                                        <td class="style159">�֩w�r��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox29" runat="server" meta:resourceKey="TextBox29Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�֩w����
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox22" runat="server" meta:resourceKey="TextBox22Resource1"></asp:TextBox>
                                        </td>
                                        <td class="style159">�֩w���
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox30" runat="server" meta:resourceKey="TextBox30Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�ͼf���
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox23" runat="server" meta:resourceKey="TextBox23Resource1"
                                                Width="65px"></asp:TextBox>
                                        </td>
                                        <td class="style159">�ͼf�r��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox31" runat="server" meta:resourceKey="TextBox31Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style159">�ͼf����
                                        </td>
                                        <td class="style30">
                                            <asp:TextBox ID="TextBox24" runat="server" meta:resourceKey="TextBox24Resource1"></asp:TextBox>
                                        </td>
                                        <td class="style159">�ͼf���
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox32" runat="server" meta:resourceKey="TextBox32Resource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View7" runat="server">
                        <asp:Panel ID="Panel7" runat="server" Height="315px">
                            <cc3:TBGridView ID="GridView_FAMILY" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" DataSourceID="SqlDataSource_Family" EmptyDataText="�d�L���" EnableEmptyContentRender="True"
                                GridLines="None" PageSize="5" Width="100%" Style="text-align: left" OnRowCommand="GridView_FAMILY_RowCommand"
                                OnRowDataBound="GridView_FAMILY_RowDataBound" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="MZ_FAMILYID" HeaderText="�����Ҹ�" SortExpression="MZ_FAMILYID">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_FAMILYNAME" HeaderText="�m�W" SortExpression="MZ_FAMILYNAME">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_BIRTHDAY" HeaderText="�ͤ�" SortExpression="MZ_BIRTHDAY">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_WORK" HeaderText="¾�~" SortExpression="MZ_WORK">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_TITLE" HeaderText="�ٿ�" SortExpression="MZ_TITLE" />
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </cc3:TBGridView>
                            <asp:SqlDataSource ID="SqlDataSource_Family" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_FAMILYID,MZ_FAMILYNAME,MZ_BIRTHDAY,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_WORK AND MZ_KTYPE='@03') AS MZ_WORK,(SELECT MZ_KCHI FROM A_KTYPE  WHERE MZ_KCODE=MZ_TITLE AND MZ_KTYPE='FAP') AS MZ_TITLE FROM A_FAMILY WHERE MZ_ID=@MZ_ID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="TextBox_MZ_ID" Name="MZ_ID" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Panel ID="Panel_FAMILY" runat="server" Visible="False">
                                <table border="1" class="style12" width="100%">
                                    <tr>
                                        <td class="style158">��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; ��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_MZ_TITLE" runat="server" AutoPostBack="True" Width="35px"
                                                MaxLength="1" OnTextChanged="TextBox_MZ_TITLE_TextChanged"></asp:TextBox><asp:Button
                                                    ID="btTITLE" runat="server" CausesValidation="False" OnClick="btTITLE_Click"
                                                    TabIndex="-1" Text="V" />
                                            <asp:TextBox ID="TextBox_MZ_TITLE1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style158">�m&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �W
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_MZ_FAMILYNAME" runat="server" MaxLength="12" Width="100px"
                                                OnTextChanged="TextBox_MZ_FAMILYNAME_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style158">�����Ҧr��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_MZ_FAMILYID" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style158">�X�ͤ��
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_MZ_BIRTHDAY" runat="server" OnTextChanged="TextBox_MZ_BIRTHDAY_TextChanged"
                                                AutoPostBack="true" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style158">¾ �~
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="TextBox_MZ_WORK" runat="server" AutoPostBack="True" Width="35px"
                                                MaxLength="2" OnTextChanged="TextBox_MZ_WORK_TextChanged" Style="height: 19px"></asp:TextBox><asp:Button
                                                    ID="btWORK" runat="server" CausesValidation="False" OnClick="btWORK_Click" TabIndex="-1"
                                                    Text="V" />
                                            <asp:TextBox ID="TextBox_MZ_WORK1" runat="server" CssClass="style111" TabIndex="-1"
                                                Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style158">�[�O���O
                                        </td>
                                        <td class="style12">
                                            <asp:RadioButtonList ID="RadioButtonList_MZ_ISINSURANCE" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="RadioButtonList_MZ_ISINSURANCE_SelectedIndexChanged"
                                                RepeatColumns="2">
                                                <asp:ListItem Value="Y">�O</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style158">�[�O���A
                                        </td>
                                        <td class="style12">
                                            <asp:DropDownList ID="DropDownList_Family_INSURANCEMODE" runat="server" Enabled="False">
                                                <asp:ListItem Selected="True" Value="1">���`</asp:ListItem>
                                                <asp:ListItem Value="2">����</asp:ListItem>
                                                <asp:ListItem Value="3">�b��</asp:ListItem>
                                                <asp:ListItem Value="4">����</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View8" runat="server">
                        <asp:Panel ID="Panel_Card" runat="server" Height="315">
                            <table border="1" style="width: 100%">
                                <tr>
                                    <td class="style109">�A���Ҹ�
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_IDNO" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="style158">�A�������O
                                    </td>
                                    <td class="style12">
                                        <asp:DropDownList ID="DropDownList_MZ_MEMO1" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="1">1.��Fĵ��</asp:ListItem>
                                            <asp:ListItem Value="2">2.�D��ĵ��</asp:ListItem>
                                            <asp:ListItem>3.�~��ĵ��</asp:ListItem>
                                            <asp:ListItem Value="4">4.�@���F</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style109">�o�ҭ�]
                                    </td>
                                    <td class="style12" colspan="3">
                                        <asp:DropDownList ID="DropDownList_MZ_INO" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="1">1.���o</asp:ListItem>
                                            <asp:ListItem Value="2">2.�ɵo</asp:ListItem>
                                            <asp:ListItem Value="3">3.���P</asp:ListItem>
                                            <asp:ListItem Value="4">4.ú�^</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style109">�o�Ҥ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" OnTextChanged="TextBox_MZ_DATE1_TextChanged"
                                            AutoPostBack="true" Width="65px"></asp:TextBox>
                                    </td>
                                    <td class="style160">�� ��
                                    </td>
                                    <td class="style12">
                                        <asp:TextBox ID="TextBox_MZ_NO1" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:View>
                </asp:MultiView><asp:Panel ID="Panel3" runat="server" meta:resourcekey="Panel3Resource2">
                    <div class="style12">
                        <table style="background-color: #CCFFFF; color: White; width: 100%;">
                            <tr>
                                <td class="style34">
                                    <asp:Button ID="btSearch" runat="server" CausesValidation="False" CssClass="style12"
                                        meta:resourcekey="btP37_DLBASETableResource1" OnClick="btSearch_Click" Text="�d��"
                                        Font-Bold="True" ForeColor="#000099" />
                                    <asp:Button ID="btInsert" runat="server" CausesValidation="False" CssClass="style12"
                                        meta:resourcekey="btInsertResource1" OnClick="btInsert_Click" Text="�s�W" Font-Bold="True"
                                        ForeColor="#000099" AccessKey="a" />
                                    <asp:Button ID="btUpdate" runat="server" CausesValidation="False" Enabled="False"
                                        OnClick="btUpdate_Click" Text="�ק�" Font-Bold="True" ForeColor="#000099" />
                                    <asp:Button ID="btOK" runat="server" Enabled="False" meta:resourcekey="btOKResource1"
                                        OnClick="btOK_Click" Text="�T�w" Font-Bold="True" ForeColor="#000099" AccessKey="s" />
                                    <asp:Button ID="btCancel" runat="server" CausesValidation="False" CssClass="style108"
                                        Enabled="False" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                                        Text="����" Font-Bold="True" ForeColor="#000099" />
                                    <asp:Button ID="btDelete" runat="server" CausesValidation="False" Enabled="False"
                                        meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click" OnClientClick="return confirm(&quot;�T�w�R���H&quot;);"
                                        Text="�R��" Font-Bold="True" ForeColor="#000099" AccessKey="d" Visible="False" />
                                    <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                        meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="�W�@��" Font-Bold="True"
                                        ForeColor="#000099" />
                                    <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label><asp:Button
                                        ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                        OnClick="btNEXT_Click" Text="�U�@��" Font-Bold="True" ForeColor="#000099" />
                                    &nbsp;
                                    <asp:Button ID="btnExportSalaryMetaData" runat="server" CausesValidation="False" CssClass="style98" Font-Bold="True" ForeColor="Maroon" OnClick="btnExportSalaryMetaData_Click" TabIndex="-1" Text="�ץX�������" Width="95px" />
                                    <asp:Button ID="btnChangSalary" runat="server" CausesValidation="False" CssClass="style98" Font-Bold="True" ForeColor="Maroon" TabIndex="-1" Text="�ק�ĵ���c��" Width="95px" OnClick="btnChangSalary_Click" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
