<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal2-1.aspx.cs" Inherits="TPPDDB._1_personnel.personal2_1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        #form1 {
            height: 614px;
            text-align: left;
        }

        .style2 {
            text-align: left;
        }

        .style8 {
            text-align: left;
        }

        .style26 {
        }

        .style31 {
            text-align: left;
        }

        .style32 {
            font-size: large;
            font-family: �з���;
            text-align: left;
            width: 392px;
        }

        .style33 {
            text-align: right;
            font-size: large;
            font-family: �з���;
        }

        .style34 {
            text-align: right;
        }

        .style40 {
            text-align: left;
        }

        .style41 {
            text-align: center;
            width: 86px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style45 {
            text-align: left;
        }

        .style110 {
        }

        .style111 {
            border-style: solid;
            border-color: inherit;
            border-width: 0px;
            font-weight: 700;
        }

        .style112 {
            height: 19px;
        }

        .style116 {
            text-align: center;
            width: 60px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style121 {
            text-align: left;
            width: 190px;
        }

        .style122 {
            width: 30px;
            text-align: center;
            background-color: #FFCCFF;
        }

        .style124 {
            text-align: center;
            width: 63px;
            background-color: #FFCCFF;
        }

        .style126 {
            text-align: left;
            width: 348px;
        }

        .style129 {
            text-align: left;
            height: 26px;
            font-weight: 700;
            color: #0033CC;
        }

        .style130 {
            text-align: left;
            color: #0033CC;
            font-weight: bold;
            background-color: #FFFF66;
        }

        .style131 {
            text-align: left;
        }

        .style132 {
            width: 61px;
            background-color: #FFFF99;
            text-align: left;
        }

        .style133 {
            text-align: left;
            width: 61px;
            background-color: #FFCCFF;
        }

        .style134 {
            color: #FF0000;
        }

        .style135 {
            text-align: left;
        }

        .style136 {
            text-align: center;
            width: 63px;
            background-color: #FFCCFF;
        }

        .style137 {
            width: 59px;
        }

        .style138 {
            width: 96px;
        }

        .style139 {
            text-align: left;
            width: 380px;
        }

        .style140 {
            text-align: left;
            width: 155px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
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
                    <td class="style32">���g��ĳ��Ƶn��
                    </td>
                    <td class="style34">
                        <asp:Button ID="btDetail" runat="server" Font-Bold="True" ForeColor="Maroon" OnClick="btDetail_Click" Text="����" Visible="false" />
                        <asp:Button ID="btn_Open_Import" runat="server" Font-Bold="true" ForeColor="Maroon" OnClick="btn_Open_Import_Click" Text="�פJ" Visible="true" />
                        <asp:Button ID="btExport" runat="server" Font-Bold="true" ForeColor="Maroon" OnClick="btExport_Click" Text="�ץX" Visible="true" />
                        &nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" CssClass="style33"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel_Personal2" runat="server">
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style132">
                            <b><span class="style134">��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��</span> </b>
                        </td>
                        <td class="style40" colspan="3">
                            <asp:TextBox ID="TextBox_MZ_NO" runat="server" Width="120px" MaxLength="15" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_NO_TextChanged"></asp:TextBox>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style133">�����Ҹ�
                        </td>
                        <td class="style139">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_ID_TextChanged"
                                Width="100px" MaxLength="10"></asp:TextBox>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="style26"
                                OnClick="Button2_Click" Text="�m�W�d�ߡ]��J�^" Width="123px" TabIndex="-1" Font-Bold="True"
                                ForeColor="Maroon" />
                        </td>
                        <td class="style136">�m&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �W
                        </td>
                        <td class="style135">
                            <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Height="19px" Width="100px" MaxLength="12"
                                AutoPostBack="True" OnTextChanged="TextBox_MZ_NAME_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style133">�s�����
                        </td>
                        <td class="style139">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                CssClass="style110" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="style136">�s����
                        </td>
                        <td class="style135">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_UNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style133">�{�A����
                        </td>
                        <td class="style139">
                            <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXAD_TextChanged" MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btEXAD" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXAD_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                        <td class="style136">�{�A���
                        </td>
                        <td class="style135">
                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_EXUNIT_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btEXUNIT" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXUNIT_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" CssClass="style111" Width="105px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style133">�x¾���_
                        </td>
                        <td class="style139">
                            <asp:TextBox ID="TextBox_MZ_RANK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_RANK_TextChanged" CssClass="style112" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btRANK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btRANK_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_RANK_1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="style136">�x¾����
                        </td>
                        <td class="style135">
                            <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_RANK1_TextChanged" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btRANK1" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btRANK1_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_RANK1_1" runat="server" CssClass="style111" Width="120px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style137" style="background-color: #FFCCFF">¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td class="style121">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_OCCC_TextChanged" MaxLength="4"></asp:TextBox>
                            <asp:Button ID="btOCCC" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btOCCC_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                        <td class="style122">¾��
                        </td>
                        <td class="style140">
                            <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_TBDV_TextChanged"></asp:TextBox>
                            <asp:Button ID="btTBDV" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" OnClick="btTBDV_Click" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" CssClass="style111" Width="80px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                        <td class="style124">�~��¾��
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_SRANK_TextChanged" MaxLength="3"></asp:TextBox>
                            <asp:Button ID="btSRANK" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btSRANK_Click" Text="V" TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style111" Width="100px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style116">���g���O
                        </td>
                        <td class="style126">
                            <asp:TextBox ID="TextBox_MZ_PRK" runat="server" MaxLength="3" OnTextChanged="TextBox_MZ_PRK_TextChanged"
                                Width="35px" AutoPostBack="True"></asp:TextBox>
                            <asp:Button ID="btPRK" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRK_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRK_1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                        <td class="style41">���g�έp����
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="TextBox_MZ_PRK1" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PRK1_TextChanged" MaxLength="3" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btPRK1" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRK1_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRK1_1" runat="server" CssClass="style111" Width="150px"
                                TabIndex="-1" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style116">���g���e
                        </td>
                        <td class="style45">
                            <asp:TextBox ID="TextBox_MZ_PRCT" runat="server" Width="600px" MaxLength="200"></asp:TextBox>
                            <asp:Button ID="btPRCT1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btPRCT1_Click" TabIndex="-1" Text="V" />
                            <asp:Button ID="btAddNote" runat="server" CausesValidation="False" OnClick="btAddNote_Click"
                                OnClientClick="return confirm(&quot;�T�w�s�W�H&quot;);" Text="�s�W���y" Width="60px" />
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style116">���g���G
                        </td>
                        <td class="style131">
                            <asp:TextBox ID="TextBox_MZ_PRRST" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PRRST_TextChanged" MaxLength="4" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btPRRST" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPRRST_Click" />
                            <asp:TextBox ID="TextBox_MZ_PRRST1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                        <td class="style130">�̾����O
                        </td>
                        <td class="style8">
                            <asp:DropDownList ID="DropDownList_MZ_POLK" runat="server" AppendDataBoundItems="True"
                                Enabled="false" Style="height: 19px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>A</asp:ListItem>
                                <asp:ListItem>B</asp:ListItem>
                                <asp:ListItem>C</asp:ListItem>
                                <asp:ListItem>D</asp:ListItem>
                                <asp:ListItem Selected="True">H</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style116">���g�̾�
                        </td>
                        <td class="style131">
                            <asp:TextBox ID="TextBox_MZ_PROLNO" runat="server" Width="55px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PROLNO_TextChanged" MaxLength="8" Style="height: 19px"></asp:TextBox>
                            <asp:Button ID="btPROLNO" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPROLNO_Click" />
                            <asp:Button ID="btPROLNO_DETAIL" runat="server" CausesValidation="False" Text="����"
                                OnClick="btPROLNO_DETAIL_Click" Font-Bold="True" ForeColor="Maroon" />
                            <asp:TextBox ID="TextBox_MZ_PROLNO1" runat="server" CssClass="style111" Width="200px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                            <td class="style130">�[������
                            </td>
                            <td class="style31">
                                <asp:TextBox ID="TextBox_MZ_PROLNO2" runat="server" Width="250px" MaxLength="40"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td class="style116">�O�_�t��
                        </td>
                        <td class="style131">
                            <asp:DropDownList ID="DropDownList_MZ_PCODE" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_MZ_PCODE_SelectedIndexChanged"
                                AppendDataBoundItems="True" Width="80px">
                                <asp:ListItem Value="1">1.�t��</asp:ListItem>
                                <asp:ListItem Value="9" Selected="True">9.���t��</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style130">�t���ڶ�
                        </td>
                        <td class="style8">
                            <asp:TextBox ID="TextBox_MZ_PCODEM" runat="server" Width="35px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_PCODEM_TextChanged" MaxLength="2"></asp:TextBox>
                            <asp:Button ID="btPCODEM" runat="server" CausesValidation="False" CssClass="style110"
                                Text="V" TabIndex="-1" OnClick="btPCODEM_Click" />
                            <asp:TextBox ID="TextBox_MZ_PCODEM1" runat="server" CssClass="style111" Width="220px"
                                TabIndex="-1" ForeColor="#0033CC" Font-Bold="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style116">��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                        </td>
                        <td class="style129">
                            <asp:TextBox Width="350px" ID="TextBox_MZ_MEMO" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td style="font-weight: 700; color: #0033CC; background-color: #FFFF66;">�� ��
                        </td>
                        <td class="style129">
                            <asp:TextBox ID="TextBox_MZ_REMARK" runat="server" Width="200px" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="1" style="border-style: solid; border-width: 1px; width: 100%;">
                    <tr>
                        <td class="style116">�o���v�d
                        </td>
                        <td class="style8">
                            <asp:DropDownList ID="DropDownList_MZ_SWT3" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">1.ĵ��v�d</asp:ListItem>
                                <asp:ListItem Value="2">2.ĵ�����v�d</asp:ListItem>
                                <asp:ListItem Value="3">3.�եL����</asp:ListItem>
                                <asp:ListItem Value="4">4.����ĵ�F�p</asp:ListItem>
                                <asp:ListItem Value="5">5.��L</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style138" style="color: #0033CC; font-weight: 700; background-color: #FFFF66;">�w�ýZ�]�o���^
                        </td>
                        <td class="style8">
                            <asp:TextBox ID="TextBox_MZ_SWT1" runat="server" Width="67px" ReadOnly="True" ForeColor="Red"></asp:TextBox>
                        </td>
                        <td style="color: #0033CC; font-weight: 700; background-color: #FFFF66;">�f�֧_
                        </td>
                        <td class="style8">
                            <asp:TextBox ID="TextBox_MZ_SWT4" runat="server" Width="67px" ReadOnly="True" ForeColor="Red"
                                Height="19px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server">
                    <table style="background-color: #CCFFFF; color: White; width: 100%">
                        <tr>
                            <td>
                                <asp:Button ID="btSearch" runat="server" CausesValidation="False" OnClick="btSearch_Click"
                                    Text="�d��" Font-Bold="True" ForeColor="#000099" />
                                <asp:Button ID="btInsert" runat="server" OnClick="btInsert_Click" Text="�s�W" CssClass="style28"
                                    CausesValidation="False" Font-Bold="True" ForeColor="#000099" AccessKey="a" />
                                <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="�ק�" CausesValidation="False"
                                    Enabled="False" Font-Bold="True" ForeColor="#000099" />
                                <asp:Button ID="btOK" runat="server" Text="�T�{" OnClick="btok_Click" OnClientClick="return Check_Before_btok_Click()" Enabled="False"
                                    Font-Bold="True" ForeColor="#000099" AccessKey="s" />
                                <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                    OnClick="btCancel_Click" Text="����" Enabled="False" Font-Bold="True" ForeColor="#000099" />
                                <asp:Button ID="btDelete" runat="server" OnClick="btDelete_Click" Text="�R��" CausesValidation="False"
                                    OnClientClick="return confirm(&quot;�T�w�R���H&quot;);" CssClass="style50" Enabled="False"
                                    Font-Bold="True" ForeColor="#000099" AccessKey="d" />
                                <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                    meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="�W�@��" Font-Bold="True"
                                    ForeColor="#000099" Width="57px" />
                                <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                                <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                    OnClick="btNEXT_Click" Text="�U�@��" Font-Bold="True" ForeColor="#000099" Width="59px" />
                                <asp:Button ID="Button_MZ_SWT4" runat="server" CausesValidation="False" OnClick="Button_MZ_SWT4_Click"
                                    Text="�f��" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="btGroupUpdate" runat="server" CausesValidation="False" OnClick="btGroupUpdate_Click"
                                    Text="���ק�" Width="64px" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="btGroupMZ_NOUpdate" runat="server" Text="���ק�׸�" CausesValidation="False"
                                    OnClick="btGroupMZ_NOUpdate_Click" Width="98px" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="Button5" runat="server" CausesValidation="False" Text="�C�L��ĳ�W�U" Width="101px"
                                    OnClick="Button5_Click" Font-Bold="True" ForeColor="Red" />
                                <asp:Button ID="btn_bysn" runat="server" CausesValidation="False" Text="�C�L��ĳ�W�U(�̬y���)"
                                    Width="101px" OnClick="btn_bysn_Click" Font-Bold="True" ForeColor="Red" Visible="false" />
                                <asp:Button ID="btReplace" runat="server" Text="����" CausesValidation="False" OnClick="btReplace_Click"
                                    Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="Panel_Excel_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                PopupDragHandleControlID="Panel_Excel" Enabled="true" PopupControlID="Panel_Excel"
                TargetControlID="btn_Open_Import" CancelControlID="btExit">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_Excel" runat="server" GroupingText="Excel�פJ" BackColor="LightGray" Width="300px" Style="display: none;">
                <table border="1" style="text-align: left;">
                    <tr>
                        <td>�פJ�ɮסG</td>
                        <td>
                            <asp:FileUpload ID="fuImport" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btImport" runat="server" Text="�פJ" OnClick="btImport_Click" />
                            <asp:Button ID="btExit" runat="server" Text="����" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Label ID="lbl_result" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btImport" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        //�T�w���s���e���ˬd�ʧ@
        function Check_Before_btok_Click() {
            /*
             "2.1.���g��ĳ��ƿ�J
~/1-personnel/Personal2-1.aspx 
�u�T�{�v�ɴ�������ˮ֡G�C
����1�GMZ_TBDB in (007,008,009) �B MZ_PRRST in (4010,4020,4100,5010,5220,5100)�BMZ_SWT3 = 2�C 
����2�GMZ_TBDB in (010,011) �BMZ_PRRST in (4100, 5100) �BMZ_SWT3 = 2�C 
������r�G�̾ڡu�s�_���F��ĵ���z���g�ץ�֩w�v�@����v���׵o���v�d��ĵ��v�d�A�O�_���n�s�W�H(�O/�_) "
            */
            var MZ_TBDV = document.getElementById('ctl00_ContentPlaceHolder1_TextBox_MZ_TBDV').value;
            var MZ_PRRST = document.getElementById('ctl00_ContentPlaceHolder1_TextBox_MZ_PRRST').value;
            var MZ_SWT3 = document.getElementById('ctl00_ContentPlaceHolder1_DropDownList_MZ_SWT3').value;
            if (MZ_SWT3 != "2") {
                return true;
            }
            /*
�u�T�{�v�ɴ�������ˮ֡G�C
����1�GMZ_TBDB in (007,008,009) �B MZ_PRRST in (4010,4020,4100,5010,5220,5100)�BMZ_SWT3 = 2�C 
����2�GMZ_TBDB in (010,011) �BMZ_PRRST in (4100, 5100) �BMZ_SWT3 = 2�C 
������r�G�̾ڡu�s�_���F��ĵ���z���g�ץ�֩w�v�@����v���׵o���v�d��ĵ��v�d�A�O�_���n�s�W�H(�O/�_) "
             */ 
            var isAlert = false;
            switch (MZ_TBDV) {
                case '007':
                case '008':
                case '009':
                    switch (MZ_PRRST) {
                        case '4010':
                        case '4020':
                        case '4100':
                        case '5010':
                        case '5220':
                        case '5100':
                            isAlert = true;
                            break;
                    }
                    break;
                case '010':
                case '011':
                    switch (MZ_PRRST) {
                        case '4100':
                        case '5100':
                            isAlert = true;
                            break;
                    }
                    break;
            }

            //��ĵ�i�T�{�F
            if (isAlert == false)
                return true;

            return confirm('�̾ڡu�s�_���F��ĵ���z���g�ץ�֩w�v�@����v���׵o���v�d��ĵ��v�d�A�O�_���n�s�W�H');
        }
    </script>
</asp:Content>
