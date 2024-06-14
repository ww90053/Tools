<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal1-4.aspx.cs" Inherits="TPPDDB._1_personnel.Personal1_4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function pageLoad() {
            var ppm = Sys.WebForms.PageRequestManager.getInstance();
            ppm.add_beginRequest(beginRequestHandler);
            //ppm.add_pageLoaded(pageLoaded);
        }

        function beginRequestHandler(sender, args) {
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button1') {
                document.getElementById("ctl00_ContentPlaceHolder1_Button1").disabled = true;
                args.get_postBackElement().disabled = true;
            }
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_Button1') {
                document.getElementById("ctl00_ContentPlaceHolder1_Button1").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

    <style type="text/css">
        #form1
        {
        }
        .style6
        {
            width: 665px;
        }
        .style38
        {
        }
        .style39
        {
            text-align: right;
        }
        .style40
        {
            text-align: left;
            width: 380px;
        }
        .style43
        {
            width: 291px;
        }
        .style45
        {
        }
        .style46
        {
            width: 119px;
        }
        .style48
        {
            width: 135px;
        }
        .style50
        {
            width: 229px;
        }
        .style53
        {
            width: 170px;
        }
        .style54
        {
            border: solid 0px;
            color: #0033CC;
            font-weight: bold;
        }
        .style55
        {
            width: 247px;
        }
        .style111
        {
            border: solid 0px;
        }
        .style112
        {
            width: 68px;
        }
        .style116
        {
            width: 115px;
        }
        .style118
        {
            width: 117px;
        }
        .style122
        {
            text-align: center;
            background-color: #FFCCFF;
        }
        .style123
        {
            text-align: center;
            background-color: #FFCCFF;
        }
        .style124
        {
            text-align: center;
            background-color: #FFCCFF;
        }
        .style125
        {
            text-align: center;
            background-color: #FFCCFF;
        }
        .style126
        {
            text-align: center;
            background-color: #FFCCFF;
            width: 27px;
        }
        .style127
        {
            text-align: left;
            background-color: #FFCCFF;
            width: 91px;
        }
        .style128
        {
            text-align: center;
            background-color: #FFCCFF;
            width: 55px;
        }
        .style129
        {
            text-align: left;
            background-color: #FFCCFF;
            width: 54px;
        }
        .style130
        {
            text-align: left;
            background-color: #FFCCFF;
            width: 56px;
        }
        .style131
        {
            text-align: left;
            width: 54px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style132
        {
            text-align: left;
            width: 55px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style133
        {
            text-align: left;
            width: 62px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style135
        {
            text-align: left;
            width: 52px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style136
        {
            text-align: left;
            width: 72px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style137
        {
            text-align: left;
            width: 30px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style138
        {
            text-align: left;
            width: 29px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style139
        {
            text-align: left;
            width: 60px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style140
        {
            text-align: left;
            width: 57px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style141
        {
            text-align: left;
            width: 43px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style142
        {
            text-align: left;
            width: 69px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style143
        {
            text-align: left;
            width: 58px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style144
        {
            text-align: left;
            width: 63px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style145
        {
            width: 126px;
        }
        .style146
        {
            width: 219px;
        }
        .style147
        {
            text-align: left;
            width: 76px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style148
        {
            width: 230px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div style="text-align: center;">
                        <img alt="" src="../images/ajax-loader.gif" style="width: 220px; height: 19px" /><br />
                        <span style="color: Red; font-weight: bold; font-size: x-medium;"><b style="font-size: medium">
                            ��ƶq�h�A�פJ���A�еy�ݡK</b></span></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td class="style40" style="font-family: �з���; font-size: large;">
                        �H�����ɼȦs���
                    </td>
                    <td class="style39">
                        <asp:Label ID="Label2" runat="server" Style="font-size: large"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Style="font-size: large"></asp:Label>
                        <asp:Button ID="Button1" runat="server" CssClass="style38" Text="�T�w�פJ" 
                            OnClick="Button1_Click" onclientclick="return confirm(&quot;�T�w�פJ�H&quot;)" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1">
                <tr>
                    <td class="style130">
                        �m&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �W
                    </td>
                    <td class="style46">
                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Width="75px" BackColor="White" meta:resourcekey="TextBox_MZ_NAMEResource1"
                            Enabled="False" MaxLength="12" AutoPostBack="True"></asp:TextBox>
                    </td>
                    <td class="style132">
                        �����Ҹ�
                    </td>
                    <td class="style48">
                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px" meta:resourcekey="TextBox_MZ_IDResource1"
                            Enabled="False" MaxLength="10"></asp:TextBox>
                    </td>
                    <td class="style132">
                        �^��m�W
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ENAME" runat="server" MaxLength="60" meta:resourceKey="MZ_ENAMEResource1"
                            Width="200px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1">
                <tr>
                    <td class="style129">
                        �s�����
                    </td>
                    <td class="style43">
                        <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" Enabled="False" MaxLength="10"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_AD1" runat="server" Width="200px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style135">
                        �s����
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="35px" Enabled="False" MaxLength="4"> </asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" Width="105px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style136">
                        ����¾���
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_FDATE" runat="server" MaxLength="9" meta:resourcekey="TextBox_MZ_PHONO0Resource1"
                            Width="65px"></asp:TextBox>
                    </td>
                </tr>
                <tr style="width: 55px;">
                    <td class="style131">
                        �{�A����
                    </td>
                    <td class="style43">
                        <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" Width="75px" Enabled="False" MaxLength="10"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" Width="200px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style135">
                        �{�A���
                    </td>
                    <td class="style53">
                        <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" Width="35px" Enabled="False" MaxLength="4"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" Width="105px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style136">
                        ��¾���
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ADATE" runat="server" MaxLength="9" meta:resourceKey="TextBox_MZ_ADATEResource1"
                            Width="65px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1">
                <tr>
                    <td class="style127">
                        ����¾��]
                    </td>
                    <td class="style12">
                        <asp:TextBox ID="TextBox_MZ_TBCD9" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_TBCD91" runat="server" Width="200px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style139">
                        ��¾�帹
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_NID" runat="server" MaxLength="36" meta:resourceKey="TextBox_MZ_NIDResource1"
                            Style="margin-right: 0px" Width="200px" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="KEY_IN_TITLE">
                        ���¾�~��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_OFFYY" runat="server" Width="60px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table border="1" style="width: 100%">
                <tr>
                    <td class="style132">
                        �X�ͤ��
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="TextBox_MZ_BIR" runat="server" MaxLength="9" Width="65px"></asp:TextBox>
                    </td>
                    <td class="style137">
                        �y �e
                    </td>
                    <td style="text-align: left;" class="style146">
                        <asp:TextBox ID="TextBox_MZ_TBCD3" runat="server" Width="25px" MaxLength="2"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_TBCD31" runat="server" CssClass="style54" TabIndex="-1"
                            Width="75px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_CITY" runat="server" CssClass="style54" MaxLength="8"
                            Width="70px"></asp:TextBox>
                    </td>
                    <td class="style137">
                        �B ��
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_MZ_SM" runat="server" meta:resourcekey="DropDownList_MZ_SMResource1"
                            AppendDataBoundItems="True">
                            <asp:ListItem meta:resourcekey="ListItemResource3" Value="2">���B</asp:ListItem>
                            <asp:ListItem meta:resourcekey="ListItemResource4" Value="1">�w�B</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style137">
                        �� �O
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_MZ_SEX" runat="server" meta:resourcekey="DropDownList_MZ_SEXResource1"
                            AppendDataBoundItems="True">
                            <asp:ListItem meta:resourcekey="ListItemResource5" Value="1">�k</asp:ListItem>
                            <asp:ListItem meta:resourcekey="ListItemResource6" Value="2">�k</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style138">
                        �嫬
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="TextBox_MZ_BL" runat="server" meta:resourcekey="TextBox_MZ_BLResource1"
                            Style="margin-right: 0px" Width="30px" MaxLength="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1" class="style45">
                <tr>
                    <td class="style128">
                        ���y�a�}
                    </td>
                    <td class="style112">
                        <asp:TextBox ID="TextBox_MZ_ZONE1" runat="server" Width="60px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ADD1" runat="server" meta:resourcekey="TextBox_MZ_ADD1Resource1"
                            Style="margin-right: 0px" Width="400px" MaxLength="120"></asp:TextBox>
                    </td>
                    <td class="style132">
                        ��ʹq��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_MOVETEL" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style128">
                        �{�b���
                    </td>
                    <td class="style112">
                        <asp:TextBox ID="TextBox_MZ_ZONE2" runat="server" Width="60px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ADD2" runat="server" MaxLength="120" meta:resourcekey="TextBox_MZ_ADD2Resource1"
                            Style="margin-right: 0px" Width="400px"></asp:TextBox>
                    </td>
                    <td class="style132">
                        ��v�q��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_PHONE" runat="server" MaxLength="12" meta:resourcekey="TextBox_MZ_PHONEResource1"
                            Style="margin-right: 0px" Width="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1">
                <tr>
                    <td class="style132">
                        &nbsp;�q�ܤ�
                    </td>
                    <td class="style116">
                        <asp:TextBox ID="TextBox_MZ_PHONO" runat="server" meta:resourcekey="TextBox_MZ_PHONOResource1"
                            Style="margin-right: 0px" Width="100px" MaxLength="19"></asp:TextBox>
                    </td>
                    <td class="style141">
                        �q�ܦv
                    </td>
                    <td class="style118">
                        <asp:TextBox ID="TextBox_MZ_PHONH" runat="server" meta:resourcekey="TextBox_MZ_PHONHResource1"
                            Width="100px" MaxLength="12"></asp:TextBox>
                    </td>
                    <td class="style142">
                        ����p���H
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_EMNAM" runat="server" meta:resourcekey="TextBox_MZ_EMNAMResource1"
                            Style="margin-right: 0px" Width="75px" MaxLength="12"></asp:TextBox>
                    </td>
                    <td class="KEY_IN_TITLE">
                        �������O
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonList_MZ_ABORIGINE" runat="server" AutoPostBack="True"
                            Height="23px" RepeatColumns="2" Width="76px">
                            <asp:ListItem Value="Y">�O</asp:ListItem>
                            <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="style126">
                        �ڧO
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ABORIGINENAME" runat="server" Enabled="False" MaxLength="20"
                            Width="75px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1">
                <tr>
                    <td class="style147">
                        �L�а_�W���
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_SLFDATE" runat="server" MaxLength="9" meta:resourcekey="TextBox_MZ_SLFDATEResource1"
                            Width="65px"></asp:TextBox>
                        ��<asp:TextBox ID="TextBox_MZ_SLEDATE" runat="server" MaxLength="9" meta:resourcekey="TextBox_MZ_SLEDATEResource1"
                            Width="65px"></asp:TextBox>
                    </td>
                    <td class="style122">
                        �L�ЧO
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ARMYSTATE" runat="server" MaxLength="1" Width="20px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_ARMYSTATE1" runat="server" CssClass="style111" TabIndex="-1"
                            Width="50px"></asp:TextBox>
                    </td>
                    <td class="style125">
                        �x��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ARMYRANK" runat="server" MaxLength="1" Width="20px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_ARMYRANK1" runat="server" CssClass="style111" TabIndex="-1"
                            Width="60px"></asp:TextBox>
                    </td>
                    <td class="style123">
                        �L��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ARMYKIND" runat="server" MaxLength="2" Width="20px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_ARMYKIND1" runat="server" CssClass="style111" TabIndex="-1"
                            Width="60px"></asp:TextBox>
                    </td>
                    <td class="style124">
                        �x��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_ARMYCOURSE" runat="server" MaxLength="2" Width="20px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_ARMYCOURSE1" runat="server" CssClass="style111" TabIndex="-1"
                            Width="60px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1">
                <tr>
                    <td class="style132">
                        �x¾���_
                    </td>
                    <td class="style50">
                        <asp:TextBox ID="TextBox_MZ_RANK" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_RANK_1" runat="server" Width="180px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style140">
                        �x¾����
                    </td>
                    <td class="style55">
                        <asp:TextBox ID="TextBox_MZ_RANK1" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_RANK1_1" runat="server" Width="120px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style132">
                        ¾�Ƚs��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_POSIND" runat="server" Width="75px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style132">
                        ¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                    </td>
                    <td class="style50">
                        <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" MaxLength="4" Width="35px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_OCCC1" runat="server" CssClass="style54" TabIndex="-1"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td class="style140">
                        ��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                    </td>
                    <td class="style55">
                        <asp:TextBox ID="TextBox_MZ_SLVC" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_SLVC1" runat="server" Width="200px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style132">
                        �H���Ϥ�
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_PESN" runat="server" MaxLength="2" Width="17px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_PESN1" runat="server" TabIndex="-1" Width="75px" CssClass="style54"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style132">
                        ¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �t
                    </td>
                    <td class="style50">
                        <asp:TextBox ID="TextBox_MZ_CHISI" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_CHISI1" runat="server" Width="180px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style140">
                        ¾&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                    </td>
                    <td class="style55">
                        <asp:TextBox ID="TextBox_MZ_TBDV" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_TBDV1" runat="server" Width="150px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style132">
                        ��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_NRT" runat="server" MaxLength="1" Width="35px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_NRT1" runat="server" TabIndex="-1" Width="50px" CssClass="style54"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table border="1" style="width: 100%; text-align: left;">
                <tr>
                    <td class="style132">
                        ��¾��]
                    </td>
                    <td class="style148">
                        <asp:TextBox ID="TextBox_MZ_NREA" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_NREA1" runat="server" Width="120px" TabIndex="-1" CssClass="style54"></asp:TextBox>
                    </td>
                    <td class="style143">
                        �D�ޯŧO
                    </td>
                    <td class="style145">
                        <asp:TextBox ID="TextBox_MZ_PCHIEF" runat="server" Width="35px" MaxLength="1"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_PCHIEF1" runat="server" CssClass="style54" Width="60px"
                            TabIndex="-1"></asp:TextBox>
                    </td>
                    <td class="style144">
                        �O�_�ݥN¾
                    </td>
                    <td class="style12">
                        <asp:RadioButtonList ID="RadioButtonList_MZ_ISEXTPOS" runat="server" AutoPostBack="True"
                            RepeatColumns="2">
                            <asp:ListItem Value="Y">�O</asp:ListItem>
                            <asp:ListItem Selected="True" Value="N">�_</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="style133">
                        �ݥN¾�W��
                    </td>
                    <td class="style12">
                        <asp:TextBox ID="TextBox_MZ_EXTPOS" runat="server" Enabled="False" MaxLength="10"
                            Width="75px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; text-align: left" border="1" class="style45">
                <tr>
                    <td class="style132">
                        �~��¾��
                    </td>
                    <td class="style50">
                        <asp:TextBox ID="TextBox_MZ_SRANK" runat="server" MaxLength="3" Width="35px"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MZ_SRANK1" runat="server" CssClass="style54" TabIndex="-1"
                            Width="180px"></asp:TextBox>
                    </td>
                    <td class="style132">
                        ��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �I
                    </td>
                    <td class="style145">
                        <asp:TextBox ID="TextBox_MZ_SPT" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                    </td>
                    <td class="style132">
                        �Ȥ���I
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_SPT1" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style132">
                        �o����
                    </td>
                    <td class="style50">
                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server" MaxLength="9" meta:resourceKey="TextBox_MZ_DATEResource1"
                            Width="65px"></asp:TextBox>
                    </td>
                    <td class="style132">
                        �ͮĤ��
                    </td>
                    <td class="style145">
                        <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" MaxLength="9" meta:resourceKey="TextBox_MZ_IDATEResource1"
                            Style="margin-right: 0px" Width="65px"></asp:TextBox>
                    </td>
                    <td class="style132">
                        ��¾��
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_MZ_LDATE" runat="server" Width="65px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="background-color: #CCFFFF; color: White; width: 100%; text-align: center;">
                <tr>
                    <td class="style6">
                        <asp:Button ID="btTP37_DLBASETable" runat="server" OnClick="btTP37_DLBASETable1_Click"
                            Text="�d��" Font-Bold="True" ForeColor="#000099" />
                        <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                            Font-Bold="True" ForeColor="#000099" meta:resourcekey="btUpperResource1" OnClick="btUpper_Click"
                            Text="�W�@��" />
                        <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                        <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" Font-Bold="True"
                            ForeColor="#000099" meta:resourcekey="btNEXTResource1" OnClick="btNEXT_Click"
                            Text="�U�@��" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
