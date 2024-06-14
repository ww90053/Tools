using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYPEOPLESET1 : System.Web.UI.Page
    {
        //string DUTYDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.AddDays(0).Day.ToString().PadLeft(2, '0');
        string DUTYDATE;
        TextBox txtName;
        Label lbID;
        Label lbID_temp;
        Label lbITEM;
        Label lbDUTYDATE;
        TextBox txtPNO;
        TextBox txtPNO1;
        TextBox txtCNOBEGIN;
        TextBox txtCNOEND;
        TextBox txtCNO;
        Button btSearchName;
        Button btClear;
        Button btAdd;
        DataTable idno_dt;
        int count;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                C.check_power();
                //TextBox1.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.AddDays(0).Day.ToString().PadLeft(2, '0');
                //DUTYDATE = TextBox1.Text;
            }
            if (Request["DD"] == null)
            {
                DUTYDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.AddDays(0).Day.ToString().PadLeft(2, '0');
                //Response.Redirect("C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?DD=" + DUTYDATE + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
            }
            else
            {
                DUTYDATE = Request["DD"].ToString();
            }

            string countColnum = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + DUTYDATE + "'");

            count = int.Parse(string.IsNullOrEmpty(countColnum) ? "0" : countColnum);

            string selectString = "SELECT * FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + DUTYDATE + "' ORDER BY ITEM";

            Label1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + Session["ADPMZ_EXUNIT"].ToString() + "'")
                           + "勤務分配表輪番設定";

            DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");

            Session["DUTYPEOPLESET_TEMPDT"] = tempDT;


            System.Globalization.CultureInfo tc = new System.Globalization.CultureInfo("zh-TW");
            tc.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();


            if (!String.IsNullOrEmpty(DUTYDATE))
            {
                string a = DUTYDATE.Trim().Substring(0, 3) + "/" + DUTYDATE.Trim().Substring(3, 2) + "/" + DUTYDATE.Trim().Substring(5, 2);
                DateTime d = DateTime.Parse(a, tc);

                if (d.AddDays(1) < DateTime.Now)
                {
                    btOK.Enabled = false;
                }
            }

           
            if (IsPostBack)
            { 
                //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);
            C.set_Panel_EnterToTAB(ref this.Panel2);
               

                if (count != 0)
                {
                    PlaceHolder1.Controls.Add(new LiteralControl("<table width=\"100%\" border=\"1\" style=\"text-align:left;\">"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"4%\">項次</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"12%\">姓　　名</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"12%\">身分證號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"8%\">勤務日期</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"8%\">勤區番號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"6%\">兼勤區</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">輪番番號起</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">輪番番號迄</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">當日輪番番號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">刪除</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">插入</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));

                    //                    idno_dt_init();

                    for (int i = 0; i < count; i++)
                    {
                        lbITEM = new Label();
                        txtName = new TextBox();
                        lbID = new Label();
                        lbID_temp = new Label();
                        txtPNO = new TextBox();
                        txtPNO1 = new TextBox();
                        txtCNOBEGIN = new TextBox();
                        txtCNOEND = new TextBox();
                        txtCNO = new TextBox();
                        btSearchName = new Button();
                        btClear = new Button();
                        lbDUTYDATE = new Label();
                        btAdd = new Button();

                        if (ViewState["SID"] == null)
                        {
                            add_idno2dt(tempDT.Rows[i]["MZ_ID"].ToString());
                        }


                        PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbITEM.ID = "Label_MZ_ITEM" + (i + 1).ToString();
                        lbITEM.Width = 20;
                        lbITEM.Text = tempDT.Rows[i]["ITEM"].ToString();
                        PlaceHolder1.Controls.Add(lbITEM);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtName.ID = "TextBox_MZ_NAME" + (i + 1).ToString();
                        txtName.Width = 75;
                        txtName.AutoPostBack = true;
                        txtName.Text = tempDT.Rows[i]["MZ_NAME"].ToString();
                        txtName.TextChanged += new EventHandler(TextBox_NAME_TextChanged);
                        PlaceHolder1.Controls.Add(txtName);

                        btSearchName.ID = "btSearchName" + (i + 1).ToString();
                        btSearchName.Text = "V";
                        btSearchName.Width = 20;
                        btSearchName.Click += new EventHandler(btSearchName_Click);
                        PlaceHolder1.Controls.Add(btSearchName);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbID.ID = "Label_MZ_ID" + (i + 1).ToString();
                        lbID.Width = 80;
                        lbID.Text = ID_Marker(tempDT.Rows[i]["MZ_ID"].ToString());
                        lbID_temp.ID = "Label_MZ_ID_temp" + (i + 1).ToString();
                        lbID_temp.Text = tempDT.Rows[i]["MZ_ID"].ToString();
                        lbID_temp.Visible = false;
                        PlaceHolder1.Controls.Add(lbID_temp);
                        PlaceHolder1.Controls.Add(lbID);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbDUTYDATE.ID = "DUTYDATE" + (i + 1).ToString();
                        lbDUTYDATE.Width = 65;
                        lbDUTYDATE.Text = o_CommonService.Personal_ReturnDateString(tempDT.Rows[i]["DUTYDATE"].ToString());
                        PlaceHolder1.Controls.Add(lbDUTYDATE);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtPNO.ID = "TextBox_MZ_PNO" + (i + 1).ToString();
                        txtPNO.Width = 40;
                        //    txtPNO.AutoPostBack = true;
                        txtPNO.Text = tempDT.Rows[i]["MZ_PNO"].ToString();
                        // txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                        PlaceHolder1.Controls.Add(txtPNO);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtPNO1.ID = "TextBox_MZ_PNO1_" + (i + 1).ToString();
                        txtPNO1.Width = 40;
                        //    txtPNO.AutoPostBack = true;
                        txtPNO1.Text = tempDT.Rows[i]["MZ_PNO1"].ToString();
                        // txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                        PlaceHolder1.Controls.Add(txtPNO1);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNOBEGIN.ID = "TextBox_MZ_CNOBEGIN" + (i + 1).ToString();
                        txtCNOBEGIN.Width = 40;
                        //   txtCNOBEGIN.AutoPostBack = true;
                        //txtCNOBEGIN.TextChanged += new EventHandler(TextBox_CNOBEGIN_TextChanged);
                        txtCNOBEGIN.Text = tempDT.Rows[i]["MZ_CNOBEGIN"].ToString();
                        PlaceHolder1.Controls.Add(txtCNOBEGIN);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNOEND.ID = "TextBox_MZ_CNOEND" + (i + 1).ToString();
                        txtCNOEND.Width = 40;
                        // txtCNOEND.AutoPostBack = true;
                        //txtCNOEND.TextChanged += new EventHandler(TextBox_CNOEND_TextChanged);
                        txtCNOEND.Text = tempDT.Rows[i]["MZ_CNOEND"].ToString();
                        PlaceHolder1.Controls.Add(txtCNOEND);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNO.ID = "TextBox_MZ_CNO" + (i + 1).ToString();
                        txtCNO.Width = 40;
                        // txtCNO.AutoPostBack = true;
                        //  txtCNO.TextChanged += new EventHandler(TextBox_CNO_TextChanged);
                        txtCNO.EnableViewState = false;
                        txtCNO.Text = tempDT.Rows[i]["MZ_CNO"].ToString();
                        PlaceHolder1.Controls.Add(txtCNO);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        btClear.ID = "btclear" + (i + 1).ToString();
                        btClear.Text = "X";
                        btClear.Width = 20;
                        btClear.Click += new EventHandler(btClear_Click);
                        PlaceHolder1.Controls.Add(btClear);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        btAdd.ID = "btAdd" + (i + 1).ToString();
                        btAdd.Text = "+";
                        btAdd.Width = 20;
                        btAdd.Click += new EventHandler(btAdd_Click);
                        PlaceHolder1.Controls.Add(btAdd);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));
                    }

                    PlaceHolder1.Controls.Add(new LiteralControl("</table>"));
                }

            }
            else
            {
                

                //選取人初始化
                if (count != 0)
                {
                    PlaceHolder1.Controls.Add(new LiteralControl("<table width=\"100%\" border=\"1\" style=\"text-align:left;\">"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"4%\">項次</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"12%\">姓　　名</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"12%\">身分證號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"8%\">勤務日期</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"8%\">勤區番號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"6%\">兼勤區</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">輪番番號起</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">輪番番號迄</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">當日輪番番號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">刪除</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">插入</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));

                    idno_dt_init();
                    for (int i = 0; i < count; i++)
                    {
                        lbITEM = new Label();
                        txtName = new TextBox();
                        lbID = new Label();
                        lbID_temp = new Label();
                        txtPNO = new TextBox();
                        txtPNO1 = new TextBox();
                        txtCNOBEGIN = new TextBox();
                        txtCNOEND = new TextBox();
                        txtCNO = new TextBox();
                        btSearchName = new Button();
                        btClear = new Button();
                        lbDUTYDATE = new Label();
                        btAdd = new Button();

                        add_idno2dt(tempDT.Rows[i]["MZ_ID"].ToString());

                        PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbITEM.ID = "Label_MZ_ITEM" + (i + 1).ToString();
                        lbITEM.Width = 20;
                        lbITEM.Text = tempDT.Rows[i]["ITEM"].ToString();
                        PlaceHolder1.Controls.Add(lbITEM);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtName.ID = "TextBox_MZ_NAME" + (i + 1).ToString();
                        txtName.Width = 75;
                        txtName.AutoPostBack = true;
                        txtName.Text = tempDT.Rows[i]["MZ_NAME"].ToString();
                        txtName.TextChanged += new EventHandler(TextBox_NAME_TextChanged);
                        PlaceHolder1.Controls.Add(txtName);

                        btSearchName.ID = "btSearchName" + (i + 1).ToString();
                        btSearchName.Text = "V";
                        btSearchName.Width = 20;
                        btSearchName.Click += new EventHandler(btSearchName_Click);
                        PlaceHolder1.Controls.Add(btSearchName);

                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbID.ID = "Label_MZ_ID" + (i + 1).ToString();
                        lbID.Width = 80;
                        lbID.Text = ID_Marker(tempDT.Rows[i]["MZ_ID"].ToString());
                        lbID_temp.ID = "Label_MZ_ID_temp" + (i + 1).ToString();
                        lbID_temp.Text = tempDT.Rows[i]["MZ_ID"].ToString();
                        lbID_temp.Visible = false;
                        PlaceHolder1.Controls.Add(lbID_temp);
                        PlaceHolder1.Controls.Add(lbID);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbDUTYDATE.ID = "DUTYDATE" + (i + 1).ToString();
                        lbDUTYDATE.Width = 40;
                        lbDUTYDATE.Text = o_CommonService.Personal_ReturnDateString(tempDT.Rows[i]["DUTYDATE"].ToString());
                        PlaceHolder1.Controls.Add(lbDUTYDATE);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtPNO.ID = "TextBox_MZ_PNO" + (i + 1).ToString();
                        txtPNO.Width = 40;
                        //   txtPNO.AutoPostBack = true;
                        //txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                        txtPNO.Text = tempDT.Rows[i]["MZ_PNO"].ToString();
                        PlaceHolder1.Controls.Add(txtPNO);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtPNO1.ID = "TextBox_MZ_PNO1_" + (i + 1).ToString();
                        txtPNO1.Width = 40;
                        //   txtPNO.AutoPostBack = true;
                        //txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                        txtPNO1.Text = tempDT.Rows[i]["MZ_PNO1"].ToString();
                        PlaceHolder1.Controls.Add(txtPNO1);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNOBEGIN.ID = "TextBox_MZ_CNOBEGIN" + (i + 1).ToString();
                        txtCNOBEGIN.Width = 40;
                        // txtCNOBEGIN.AutoPostBack = true;
                        //txtCNOBEGIN.TextChanged += new EventHandler(TextBox_CNOBEGIN_TextChanged);
                        txtCNOBEGIN.Text = tempDT.Rows[i]["MZ_CNOBEGIN"].ToString();
                        PlaceHolder1.Controls.Add(txtCNOBEGIN);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNOEND.ID = "TextBox_MZ_CNOEND" + (i + 1).ToString();
                        txtCNOEND.Width = 40;
                        //   txtCNOEND.AutoPostBack = true;
                        //txtCNOEND.TextChanged += new EventHandler(TextBox_CNOEND_TextChanged);
                        txtCNOEND.Text = tempDT.Rows[i]["MZ_CNOEND"].ToString();
                        PlaceHolder1.Controls.Add(txtCNOEND);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNO.ID = "TextBox_MZ_CNO" + (i + 1).ToString();
                        txtCNO.Width = 40;
                        // txtCNO.AutoPostBack = true;
                        // txtCNO.TextChanged += new EventHandler(TextBox_CNO_TextChanged);
                        txtCNO.Text = tempDT.Rows[i]["MZ_CNO"].ToString();
                        txtCNO.EnableViewState = false;
                        PlaceHolder1.Controls.Add(txtCNO);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        btClear.ID = "btclear" + (i + 1).ToString();
                        btClear.Text = "X";
                        btClear.Width = 20;
                        btClear.Click += new EventHandler(btClear_Click);
                        PlaceHolder1.Controls.Add(btClear);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        btAdd.ID = "btAdd" + (i + 1).ToString();
                        btAdd.Text = "+";
                        btAdd.Width = 20;
                        btAdd.Click += new EventHandler(btAdd_Click);
                        PlaceHolder1.Controls.Add(btAdd);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));
                    }
                    PlaceHolder1.Controls.Add(new LiteralControl("</table>"));
                }
                else
                {
                    string InsertString = "INSERT INTO C_DUTYPEOPLE(MZ_ID,MZ_AD,MZ_UNIT,DUTYDATE,ITEM) SELECT '   ',MZ_AD,MZ_UNIT,'" + DUTYDATE + "',ROWNUM FROM A_DLBASE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";

                    o_DBFactory.ABC_toTest.Edit_Data(InsertString);

                    lbITEM = new Label();
                    txtName = new TextBox();
                    lbID = new Label();
                    lbID_temp = new Label();
                    txtPNO = new TextBox();
                    txtPNO1 = new TextBox();
                    txtCNOBEGIN = new TextBox();
                    txtCNOEND = new TextBox();
                    txtCNO = new TextBox();
                    btSearchName = new Button();
                    btClear = new Button();
                    lbDUTYDATE = new Label();
                    btAdd = new Button();

                    PlaceHolder1.Controls.Add(new LiteralControl("<table with:'100%' broder='1' style=\"text-align:left;\">"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"4%\">項次</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"12%\">姓　　名</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"12%\">身分證號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"8%\">勤務日期</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"8%\">勤區番號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"6%\">兼勤區</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">輪番番號起</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">輪番番號迄</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"10%\">當日輪番番號</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">刪除</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">插入</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));

                    selectString = "SELECT * FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'　AND DUTYDATE='" + DUTYDATE + "'";
                    tempDT = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");

                    idno_dt_init();

                    for (int i = 0; i < count; i++)
                    {
                        add_idno2dt(tempDT.Rows[i]["MZ_ID"].ToString());

                        PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbITEM.ID = "Label_MZ_ITEM" + (i + 1).ToString();
                        lbITEM.Width = 20;
                        lbITEM.Text = tempDT.Rows[i]["ITEM"].ToString();
                        PlaceHolder1.Controls.Add(lbITEM);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtName.ID = "TextBox_MZ_NAME" + (i + 1).ToString();
                        txtName.AutoPostBack = true;
                        txtName.Width = 75;
                        txtName.TextChanged += new EventHandler(TextBox_NAME_TextChanged);
                        PlaceHolder1.Controls.Add(txtName);

                        btSearchName.ID = "btSearchName" + (i + 1).ToString();
                        btSearchName.Text = "V";
                        btSearchName.Width = 20;
                        btSearchName.Click += new EventHandler(btSearchName_Click);
                        PlaceHolder1.Controls.Add(btSearchName);

                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbID.ID = "Label_MZ_ID" + (i + 1).ToString();
                        lbID.Width = 80;
                        lbID_temp.ID = "Label_MZ_ID_temp" + (i + 1).ToString();
                        lbID_temp.Visible=false;
                        PlaceHolder1.Controls.Add(lbID_temp);
                        PlaceHolder1.Controls.Add(lbID);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        lbDUTYDATE.ID = "DUTYDATE" + (i + 1).ToString();
                        lbDUTYDATE.Width = 65;
                        lbDUTYDATE.Text = o_CommonService.Personal_ReturnDateString(DUTYDATE);
                        PlaceHolder1.Controls.Add(lbDUTYDATE);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtPNO.ID = "TextBox_MZ_PNO" + (i + 1).ToString();
                        //txtPNO.AutoPostBack = true;
                        txtPNO.Width = 40;
                        txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                        PlaceHolder1.Controls.Add(txtPNO);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtPNO1.ID = "TextBox_MZ_PNO1_" + (i + 1).ToString();
                        //txtPNO.AutoPostBack = true;
                        txtPNO1.Width = 40;
                        //txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                        PlaceHolder1.Controls.Add(txtPNO1);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNOBEGIN.ID = "TextBox_MZ_CNOBEGIN" + (i + 1).ToString();
                        //txtCNOBEGIN.AutoPostBack = true;
                        txtCNOBEGIN.Width = 40;
                        //txtCNOBEGIN.TextChanged += new EventHandler(TextBox_CNOBEGIN_TextChanged);
                        PlaceHolder1.Controls.Add(txtCNOBEGIN);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNOEND.ID = "TextBox_MZ_CNOEND" + (i + 1).ToString();
                        //txtCNOEND.AutoPostBack = true;
                        txtCNOEND.Width = 40;
                        txtCNOEND.TextChanged += new EventHandler(TextBox_CNOEND_TextChanged);
                        PlaceHolder1.Controls.Add(txtCNOEND);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        txtCNO.ID = "TextBox_MZ_CNO" + (i + 1).ToString();
                        //txtCNO.AutoPostBack = true;
                        txtCNO.Width = 40;
                        // txtCNO.TextChanged += new EventHandler(TextBox_CNO_TextChanged);
                        PlaceHolder1.Controls.Add(txtCNO);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        btClear.ID = "btclear" + (i + 1).ToString();
                        btClear.Text = "X";
                        btClear.Width = 20;
                        btClear.Click += new EventHandler(btClear_Click);
                        PlaceHolder1.Controls.Add(btClear);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                        btAdd.ID = "btAdd" + (i + 1).ToString();
                        btAdd.Text = "+";
                        btAdd.Width = 20;
                        btAdd.Click += new EventHandler(btAdd_Click);
                        PlaceHolder1.Controls.Add(btAdd);
                        PlaceHolder1.Controls.Add(new LiteralControl("</td>"));


                        PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));
                    }

                    PlaceHolder1.Controls.Add(new LiteralControl("</table>"));
                }

            }
            idno_bind();
        }

        Button btSearch = new Button();

        protected void btSearchName_Click(object sender, System.EventArgs e)
        {
            btSearch = sender as Button;

            string strbtSearch = "";

            if (btSearch.ID.Length == 13)
            {
                strbtSearch = btSearch.ID.Substring(12, 1);
            }
            else if (btSearch.ID.Length == 14)
            {
                strbtSearch = btSearch.ID.Substring(12, 2);
            }
            else if (btSearch.ID.Length == 15)
            {
                strbtSearch = btSearch.ID.Substring(12, 3);
            }

            ViewState["strbtSearch"] = strbtSearch;
            ViewState["SID"] = ((PlaceHolder1.FindControl("Label_MZ_ID" + strbtSearch)) as Label).Text;

            ModalPopupExtender1.Show();
        }

        protected void btAdd_Click(object sender, System.EventArgs e)
        {
            btAdd = sender as Button;

            int i = 0;

            if (btAdd.ID.Length == 6)
            {
                i = int.Parse(btAdd.ID.Substring(5, 1));
            }
            else if (btAdd.ID.Length == 7)
            {
                i = int.Parse(btAdd.ID.Substring(5, 2));
            }
            else if (btAdd.ID.Length == 8)
            {
                i = int.Parse(btAdd.ID.Substring(5, 3));
            }

            string selectString = "SELECT COUNT(*) FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'　AND DUTYDATE='" + DUTYDATE + "'";
            int sum = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(selectString));
            for (int k = sum; k >= i + 1; k--)
            {
                selectString = "UPDATE C_DUTYPEOPLE SET ITEM='" + (k + 1) + "' WHERE ITEM='" + k + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'　AND DUTYDATE='" + DUTYDATE + "'";
                o_DBFactory.ABC_toTest.Edit_Data(selectString);
            }
            selectString = "INSERT INTO C_DUTYPEOPLE(MZ_ID,MZ_AD,MZ_UNIT,DUTYDATE,ITEM) VALUES(' ','" + Session["ADPMZ_EXAD"] + "','" + Session["ADPMZ_EXUNIT"] + "','" + DUTYDATE + "','" + (i + 1) + "')";
            o_DBFactory.ABC_toTest.Edit_Data(selectString);
            if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), selectString) == "N")
            {
                TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", selectString);
            }
            
            //show();
            if (Request["DD"] == null)
            {
                Response.Redirect("C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?DD=" + DUTYDATE + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
            }
            else
            {
                Response.Redirect("C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?DD=" + Request["DD"] + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
            }
            // Page_Load(sender, e);
        }

        protected void btClear_Click(object sender, System.EventArgs e)
        {
            btClear = sender as Button;
            int i = 0;
            if (btClear.ID.Length == 8)
            {
                i = int.Parse(btClear.ID.Substring(7, 1));
            }
            else if (btClear.ID.Length == 9)
            {
                i = int.Parse(btClear.ID.Substring(7, 2));
            }
            else if (btClear.ID.Length == 10)
            {
                i = int.Parse(btClear.ID.Substring(7, 3));
            }
            TextBox a = PlaceHolder1.FindControl("TextBox_MZ_NAME" + (i).ToString()) as TextBox;
            Label b = PlaceHolder1.FindControl("Label_MZ_ID" + (i).ToString()) as Label;
            Label b_t = PlaceHolder1.FindControl("Label_MZ_ID_temp" + (i).ToString()) as Label;
            Label c = PlaceHolder1.FindControl("Label_MZ_ITEM" + (i).ToString()) as Label;
            TextBox d = PlaceHolder1.FindControl("TextBox_MZ_PNO" + (i).ToString()) as TextBox;
            TextBox f = PlaceHolder1.FindControl("TextBox_MZ_CNOBEGIN" + (i).ToString()) as TextBox;
            TextBox g = PlaceHolder1.FindControl("TextBox_MZ_CNOEND" + (i).ToString()) as TextBox;
            TextBox h = PlaceHolder1.FindControl("TextBox_MZ_CNO" + (i).ToString()) as TextBox;
            
            idno_dt = ViewState["idno_dt"] as DataTable;
            DataRow[] dr = idno_dt.Select("IDNO='" + b.Text + "'");
            if (dr.Count() > 0)
            {
                dr[0].Delete();
                idno_dt.AcceptChanges();
            }
            ViewState["idno"] = idno_dt;
            idno_bind();

            a.Text = string.Empty;
            b.Text = string.Empty;
            //c.Text = string.Empty;
            d.Text = string.Empty;
            f.Text = string.Empty;
            g.Text = string.Empty;
            h.Text = string.Empty;
            b_t.Text = string.Empty;
        }

        protected void checkID(TextBox tb1)
        {
            string strtbNAME = "";

            if (tb1.ID.Length == 16)
            {
                strtbNAME = tb1.ID.Substring(15, 1);
            }
            else if (tb1.ID.Length == 17)
            {
                strtbNAME = tb1.ID.Substring(15, 2);
            }
            else if (tb1.ID.Length == 18)
            {
                strtbNAME = tb1.ID.Substring(15, 3);
            }

            for (int i = 0; i < count; i++)
            {
                if ((i + 1).ToString() != strtbNAME)
                {
                    if ((PlaceHolder1.FindControl("Textbox_MZ_NAME" + (i + 1).ToString()) as TextBox).Text == tb1.Text.Trim())
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('人員重複,請重新輸入！');", true);
                        tb1.Text = string.Empty;
                        (PlaceHolder1.FindControl("Label_MZ_ID_temp" + strtbNAME) as Label).Text = string.Empty;
                        tb1.Focus();
                        return;
                    }
                }
            }

            string strMZ_ID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME=N'" + tb1.Text + "' AND MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"] + "'");

            if (string.IsNullOrEmpty(strMZ_ID))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無此人,請重新輸入！');", true);
                tb1.Text = string.Empty;
                (PlaceHolder1.FindControl("Label_MZ_ID_temp" + strtbNAME) as Label).Text = string.Empty;
                tb1.Focus();
            }
            else
            {
                (PlaceHolder1.FindControl("Label_MZ_ID_temp" + strtbNAME) as Label).Text = strMZ_ID;
                add_idno2dt(strMZ_ID);
                idno_bind();
            }
        }

        protected void TextBox_NAME_TextChanged(object sender, System.EventArgs e)
        {
            TextBox tbNAME = new TextBox();

            tbNAME = sender as TextBox;

            if (tbNAME.Text.Trim() != string.Empty)
            {
                checkID(tbNAME);
            }
            else
            {
                TextBox tb = sender as TextBox;
                string i = string.Empty;
                if (tb.ID.Length == 16)
                {
                    i = tb.ID.Substring(15, 1);
                }
                else if (tb.ID.Length == 17)
                {
                    i = tb.ID.Substring(15, 2);
                }
                else if (tb.ID.Length == 18)
                {
                    i = tb.ID.Substring(15, 3);
                }

                string strSQL = "SELECT MZ_ID FROM C_DUTYPEOPLE  WHERE ITEM='" + i +
                                                                 "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                                 "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                                 "' AND DUTYDATE='" + DUTYDATE + "'";
                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                if (temp.Rows.Count > 0)
                {
                    idno_dt = ViewState["idno_dt"] as DataTable;
                    DataRow[] dr = idno_dt.Select("IDNO='" + temp.Rows[0]["MZ_ID"].ToString() + "'");
                    if (dr.Count() > 0)
                    {
                        dr[0].Delete();
                        idno_dt.AcceptChanges();
                    }
                    ViewState["idno"] = idno_dt;
                    idno_bind();
                }
            }
        }

        protected void TextBox_PNO_TextChanged(object sender, System.EventArgs e)
        {
            TextBox tbPNO = new TextBox();

            tbPNO = sender as TextBox;

            string strtbPNO = "";

            if (tbPNO.ID.Length == 15)
            {
                strtbPNO = tbPNO.ID.Substring(14, 1);
            }
            else if (tbPNO.ID.Length == 16)
            {
                strtbPNO = tbPNO.ID.Substring(14, 2);
            }
            else if (tbPNO.ID.Length == 17)
            {
                strtbPNO = tbPNO.ID.Substring(14, 3);
            }

            for (int i = 0; i < count; i++)
            {
                if ((i + 1).ToString() != strtbPNO)
                {
                    if ((PlaceHolder1.FindControl("Textbox_MZ_PNO" + (i + 1).ToString()) as TextBox).Text == tbPNO.Text.Trim())
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('勤區編號重複,請重新輸入！');", true);
                        tbPNO.Text = string.Empty;
                        tbPNO.Focus();
                        return;
                    }
                }
            }
        }

        protected void TextBox_CNOBEGIN_TextChanged(object sender, System.EventArgs e)
        {
            TextBox tbCNOBEGIN = new TextBox();

            tbCNOBEGIN = sender as TextBox;

            string strtbCNOBEGIN = "";

            if (tbCNOBEGIN.ID.Length == 20)
            {
                strtbCNOBEGIN = tbCNOBEGIN.ID.Substring(19, 1);
            }
            else if (tbCNOBEGIN.ID.Length == 21)
            {
                strtbCNOBEGIN = tbCNOBEGIN.ID.Substring(19, 2);
            }
            else if (tbCNOBEGIN.ID.Length == 22)
            {
                strtbCNOBEGIN = tbCNOBEGIN.ID.Substring(19, 3);
            }

            for (int i = int.Parse(strtbCNOBEGIN); i < count; i++)
            {
                (PlaceHolder1.FindControl("TextBox_MZ_CNOBEGIN" + (i + 1).ToString()) as TextBox).Text = tbCNOBEGIN.Text;
            }

        }

        protected void TextBox_CNOEND_TextChanged(object sender, System.EventArgs e)
        {
            TextBox tbCNOEND = new TextBox();

            tbCNOEND = sender as TextBox;

            string strtbCNOEND = "";

            if (tbCNOEND.ID.Length == 18)
            {
                strtbCNOEND = tbCNOEND.ID.Substring(17, 1);
            }
            else if (tbCNOEND.ID.Length == 19)
            {
                strtbCNOEND = tbCNOEND.ID.Substring(17, 2);
            }
            else if (tbCNOEND.ID.Length == 20)
            {
                strtbCNOEND = tbCNOEND.ID.Substring(17, 3);
            }

            for (int i = int.Parse(strtbCNOEND); i < count; i++)
            {
                (PlaceHolder1.FindControl("TextBox_MZ_CNOEND" + (i + 1).ToString()) as TextBox).Text = tbCNOEND.Text;
            }
        }

        protected void TextBox_CNO_TextChanged(object sender, System.EventArgs e)
        {
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                if (ViewState["SB"] == null || int.Parse(ViewState["SB"].ToString()) == 0)
                {
                    GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                    string idno = GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString();
                    if (ViewState["SID"] != null)
                    {
                        delidno_dt(ViewState["SID"].ToString());
                    }

                    (PlaceHolder1.FindControl("TextBox_MZ_NAME" + ViewState["strbtSearch"].ToString()) as TextBox).Text = o_A_DLBASE.CNAME(idno);

                    (PlaceHolder1.FindControl("Label_MZ_ID" + ViewState["strbtSearch"].ToString()) as Label).Text = ID_Marker(idno);
                    (PlaceHolder1.FindControl("Label_MZ_ID_temp" + ViewState["strbtSearch"].ToString()) as Label).Text = idno;

                    checkID(PlaceHolder1.FindControl("TextBox_MZ_NAME" + ViewState["strbtSearch"].ToString()) as TextBox);

                }
                else if (ViewState["SB"] != null || int.Parse(ViewState["SB"].ToString()) == 1)
                {
                    if (ViewState["SQL"] != null)
                    {
                        if (ViewState["SID"] != null)
                        {
                            delidno_dt(ViewState["SID"].ToString());
                        }
                        string strSQL = ViewState["SQL"].ToString();
                        DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                        GridView1.DataSource = temp;
                        GridView1.DataBind();
                        GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                        string idno = GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString();
                        (PlaceHolder1.FindControl("TextBox_MZ_NAME" + ViewState["strbtSearch"].ToString()) as TextBox).Text = o_A_DLBASE.CNAME(idno);

                        (PlaceHolder1.FindControl("Label_MZ_ID" + ViewState["strbtSearch"].ToString()) as Label).Text = ID_Marker(idno);
                        (PlaceHolder1.FindControl("Label_MZ_ID_temp" + ViewState["strbtSearch"].ToString()) as Label).Text = idno;

                        checkID(PlaceHolder1.FindControl("TextBox_MZ_NAME" + ViewState["strbtSearch"].ToString()) as TextBox);
                        ViewState["SB"] = 0;
                    }
                }
                ModalPopupExtender1.Hide();
            }
        }

        protected void delidno_dt(string idno)
        {
            idno_dt = ViewState["idno_dt"] as DataTable;
            DataRow[] dr = idno_dt.Select("IDNO='" + idno + "'");
            if (dr.Count() > 0)
            {
                foreach (DataRow dr1 in dr)
                {
                    dr1.Delete();
                    idno_dt.AcceptChanges();
                }
            }
            ViewState["idno"] = idno_dt;
            idno_bind();
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    int item = 1;
                    for (int i = 0; i < count; i++)
                    {
                        TextBox txtName1 = PlaceHolder1.FindControl("TextBox_MZ_NAME" + (i + 1).ToString()) as TextBox;
                        Label lbID1 = PlaceHolder1.FindControl("Label_MZ_ID_temp" + (i + 1).ToString()) as Label;
                        Label lbITEM1 = PlaceHolder1.FindControl("Label_MZ_ITEM" + (i + 1).ToString()) as Label;
                        TextBox txtPNO1 = PlaceHolder1.FindControl("TextBox_MZ_PNO" + (i + 1).ToString()) as TextBox;
                        TextBox txtPNO1_1 = PlaceHolder1.FindControl("TextBox_MZ_PNO1_" + (i + 1).ToString()) as TextBox;
                        TextBox txtCNOBEGIN1 = PlaceHolder1.FindControl("TextBox_MZ_CNOBEGIN" + (i + 1).ToString()) as TextBox;
                        TextBox txtCNOEND1 = PlaceHolder1.FindControl("TextBox_MZ_CNOEND" + (i + 1).ToString()) as TextBox;
                        TextBox txtCNO1 = PlaceHolder1.FindControl("TextBox_MZ_CNO" + (i + 1).ToString()) as TextBox;

                        string strSQL = "";
                        if (lbID1.Text.Trim() != "")
                        {
                            string pno1 = txtPNO1.Text.Trim() == string.Empty ? txtCNO1.Text.Trim() : txtPNO1.Text.Trim();
                            strSQL = "UPDATE C_DUTYPEOPLE SET " +
                                                                 "  MZ_PNO='" + pno1 +
                                                                 "',MZ_PNO1='" + txtPNO1_1.Text.Trim() +
                                                                 "',MZ_CNOBEGIN='" + txtCNOBEGIN1.Text.Trim() +
                                                                 "',MZ_CNOEND='" + txtCNOEND1.Text.Trim() +
                                                                 "',MZ_CNO='" + txtCNO1.Text.Trim() +
                                                                 "',MZ_ID='" + lbID1.Text.Trim() +
                                                                 "',MZ_NAME='" + txtName1.Text.Trim() +
                                                                  "',ITEM='" + item +
                                                                 "' WHERE ITEM='" + (i + 1) +
                                                                 "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                                 "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                                 "' AND DUTYDATE='" + DUTYDATE + "'";
                            item++;
                        }
                        else
                        {
                            
                            strSQL = "DELETE FROM C_DUTYPEOPLE WHERE ITEM='" + lbITEM1.Text.Trim() +
                                                                  "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                                  "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                                  "' AND DUTYDATE='" + DUTYDATE + "'";
                        }

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        cmd.Transaction = transaction;

                        cmd.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }
            }

            var date = DateTime.Parse((int.Parse(DUTYDATE.Substring(0, 3)) + 1911).ToString() + "/" + DUTYDATE.Substring(3, 2) + "/" + "01").AddMonths(1).AddDays(-1);
            //計算次月的最後一天
            int MonthLastDay = DateTime.Parse((int.Parse(DUTYDATE.Substring(0, 3)) + 1911).ToString() + "/" + DUTYDATE.Substring(3, 2) + "/" + "01").AddMonths(1).AddDays(-1).Day;
            //次月的日期
            string DUTYDATE2 = (date.Year - 1911).ToString() + (date.AddMonths(1).Month).ToString().PadLeft(2, '0') + "00";
            //因為要塞30天資料...所以先把今天到月底的日期差先算出來
            DateTime dt = new DateTime(int.Parse(DUTYDATE.Substring(0, 3)) + 1911, int.Parse(DUTYDATE.Substring(3, 2)), int.Parse(DUTYDATE.Substring(5, 2)));
            int DayDiff = MonthLastDay - dt.Day;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                string DeleteString = "DELETE FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE>'" + DUTYDATE + "'";
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                string strSQL;
                try
                {
                    for (int i = 1; i <= 30; i++)
                    {
                        if (i <= DayDiff)
                        {
                            strSQL = " INSERT INTO C_DUTYPEOPLE" +
                                     " SELECT MZ_ID," +
                                             "'" + (int.Parse(DUTYDATE) + i).ToString().PadLeft(7, '0') + "'," +
                                             "MZ_PNO," +
                                             "MZ_CNOBEGIN," +
                                             "MZ_CNOEND," +
                                             "CASE WHEN dbo.to_number(MZ_CNO)+1<=dbo.to_number(MZ_CNOEND) THEN CONVERT(VARCHAR(200),dbo.to_number(MZ_CNO)+1) ELSE MZ_CNOBEGIN END, " +
                                             "MZ_AD," +
                                             "MZ_UNIT," +
                                             "ITEM," +
                                             "MZ_NAME," +
                                             "MZ_PNO1" +
                                     " FROM C_DUTYPEOPLE " +
                                     " WHERE  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                       "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                       "' AND DUTYDATE='" + (int.Parse(DUTYDATE) + i - 1).ToString().PadLeft(7, '0') + "'";
                        }
                        else
                        {
                            if (i == DayDiff + 1)
                            {
                                strSQL = " INSERT INTO C_DUTYPEOPLE" +
                                         " SELECT MZ_ID," +
                                                 "'" + (int.Parse(DUTYDATE2) + i - DayDiff).ToString().PadLeft(7, '0') + "'," +
                                                 "MZ_PNO," +
                                                 "MZ_CNOBEGIN," +
                                                 "MZ_CNOEND," +
                                                 "CASE WHEN dbo.to_number(MZ_CNO)+1<=dbo.to_number(MZ_CNOEND) THEN CONVERT(VARCHAR(200),dbo.to_number(MZ_CNO)+1) ELSE MZ_CNOBEGIN END, " +
                                                 "MZ_AD," +
                                                 "MZ_UNIT," +
                                                 "ITEM," +
                                                 "MZ_NAME," +
                                                 "MZ_PNO1" +
                                         " FROM C_DUTYPEOPLE " +
                                         " WHERE  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                           "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                           "' AND DUTYDATE='" + (int.Parse(DUTYDATE) + i - 1).ToString().PadLeft(7, '0') + "'";
                            }
                            else
                            {
                                strSQL = " INSERT INTO C_DUTYPEOPLE" +
                                         " SELECT MZ_ID," +
                                                 "'" + (int.Parse(DUTYDATE2) + i - DayDiff).ToString().PadLeft(7, '0') + "'," +
                                                 "MZ_PNO," +
                                                 "MZ_CNOBEGIN," +
                                                 "MZ_CNOEND," +
                                                 "CASE WHEN dbo.to_number(MZ_CNO)+1<=dbo.to_number(MZ_CNOEND) THEN CONVERT(VARCHAR(200),dbo.to_number(MZ_CNO)+1) ELSE MZ_CNOBEGIN END, " +
                                                 "MZ_AD," +
                                                 "MZ_UNIT," +
                                                 "ITEM," +
                                                 "MZ_NAME," +
                                                 "MZ_PNO1" +
                                         " FROM C_DUTYPEOPLE " +
                                         " WHERE  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                           "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                           "' AND DUTYDATE='" + (int.Parse(DUTYDATE2) + i - 1 - DayDiff).ToString().PadLeft(7, '0') + "'";
                            }
                        }

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
        }

        protected void btReset_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE>='" + DUTYDATE + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                string InsertString = "INSERT INTO C_DUTYPEOPLE(MZ_ID,MZ_AD,MZ_UNIT,DUTYDATE,ITEM) SELECT '   ',MZ_AD,MZ_UNIT,'" + DUTYDATE + "',ROWNUM FROM A_DLBASE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";

                o_DBFactory.ABC_toTest.Edit_Data(InsertString);

                string selectString = "SELECT * FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'　AND DUTYDATE='" + DUTYDATE + "'";

                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");

                PlaceHolder1.Controls.Clear();

                lbITEM = new Label();
                txtName = new TextBox();
                lbID = new Label();
                lbID_temp = new Label();
                txtPNO = new TextBox();
                txtCNOBEGIN = new TextBox();
                txtCNOEND = new TextBox();
                txtCNO = new TextBox();
                btSearchName = new Button();
                lbDUTYDATE = new Label();
                btClear = new Button();
                btAdd = new Button();

                PlaceHolder1.Controls.Add(new LiteralControl("<table with:'100%' broder='1' style=\"text-align:left;\">"));
                PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"5%\">項次</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"15%\">姓　　名</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"15%\">身分證號</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"14%\">勤務日期</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"14%\">勤區編號</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"14%\">輪番編號起</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"14%\">輪番編號迄</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<td width=\"14%\">輪番編號</td>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));

                for (int i = 0; i < count; i++)
                {
                    PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    lbITEM.ID = "Label_MZ_ITEM" + (i + 1).ToString();
                    lbITEM.Width = 20;
                    lbITEM.Text = tempDT.Rows[i]["ITEM"].ToString();
                    PlaceHolder1.Controls.Add(lbITEM);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    txtName.ID = "TextBox_MZ_NAME" + (i + 1).ToString();
                    txtName.AutoPostBack = true;
                    txtName.Width = 100;
                    txtName.TextChanged += new EventHandler(TextBox_NAME_TextChanged);
                    PlaceHolder1.Controls.Add(txtName);

                    btSearchName.ID = "btSearchName" + (i + 1).ToString();
                    btSearchName.Text = "V";
                    btSearchName.Width = 20;
                    btSearchName.Click += new EventHandler(btSearchName_Click);
                    PlaceHolder1.Controls.Add(btSearchName);

                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    lbID.ID = "Label_MZ_ID" + (i + 1).ToString();
                    lbID.Width = 100;
                    lbID_temp.ID = "Label_MZ_ID_temp" + (i + 1).ToString();
                    lbID_temp.Visible = false;
                    PlaceHolder1.Controls.Add(lbID_temp);
                    PlaceHolder1.Controls.Add(lbID);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    lbDUTYDATE.ID = "DUTYDATE" + (i + 1).ToString();
                    lbDUTYDATE.Width = 65;
                    lbDUTYDATE.Text = o_CommonService.Personal_ReturnDateString(DUTYDATE);
                    PlaceHolder1.Controls.Add(lbDUTYDATE);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    txtPNO.ID = "TextBox_MZ_PNO" + (i + 1).ToString();
                    //   txtPNO.AutoPostBack = true;
                    txtPNO.Width = 60;
                    // txtPNO.TextChanged += new EventHandler(TextBox_PNO_TextChanged);
                    PlaceHolder1.Controls.Add(txtPNO);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    txtCNOBEGIN.ID = "TextBox_MZ_CNOBEGIN" + (i + 1).ToString();
                    //   txtCNOBEGIN.AutoPostBack = true;
                    txtCNOBEGIN.Width = 60;
                    //txtCNOBEGIN.TextChanged += new EventHandler(TextBox_CNOBEGIN_TextChanged);
                    PlaceHolder1.Controls.Add(txtCNOBEGIN);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    txtCNOEND.ID = "TextBox_MZ_CNOEND" + (i + 1).ToString();
                    //  txtCNOEND.AutoPostBack = true;
                    txtCNOEND.Width = 60;
                    // txtCNOEND.TextChanged += new EventHandler(TextBox_CNOEND_TextChanged);
                    PlaceHolder1.Controls.Add(txtCNOEND);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    txtCNO.ID = "TextBox_MZ_CNO" + (i + 1).ToString();
                    //  txtCNO.AutoPostBack = true;
                    txtCNO.Width = 60;
                    //   txtCNO.TextChanged += new EventHandler(TextBox_CNO_TextChanged);
                    PlaceHolder1.Controls.Add(txtCNO);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    btClear.ID = "btclear" + (i + 1).ToString();
                    btClear.Text = "X";
                    btClear.Width = 20;
                    btClear.Click += new EventHandler(btClear_Click);
                    PlaceHolder1.Controls.Add(btClear);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    btAdd.ID = "btAdd" + (i + 1).ToString();
                    btAdd.Text = "+";
                    btAdd.Width = 20;
                    btAdd.Click += new EventHandler(btAdd_Click);
                    PlaceHolder1.Controls.Add(btAdd);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));
                }

                PlaceHolder1.Controls.Add(new LiteralControl("</table>"));

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);

            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('重設失敗！');", true);
            }
        }

        protected void Button_all_Click(object sender, EventArgs e)
        {
            string selectString = "SELECT MAX(ITEM) FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'　AND DUTYDATE='" + DUTYDATE + "'";
            int sum = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(selectString));

            selectString = "SELECT COUNT(MZ_ID) FROM A_DLBASE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
            int sum1 = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(selectString));

            if (sum != sum1)
            {
                for (int i = sum; i < sum1; i++)
                {
                    selectString = "INSERT INTO C_DUTYPEOPLE(MZ_ID,MZ_AD,MZ_UNIT,DUTYDATE,ITEM) VALUES(' ','" + Session["ADPMZ_EXAD"] + "','" + Session["ADPMZ_EXUNIT"] + "','" + DUTYDATE + "','" + (i + 1) + "')";
                    o_DBFactory.ABC_toTest.Edit_Data(selectString);
                }
                Response.Redirect("C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?DD=" + DUTYDATE + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
            }
            else if (sum >= sum1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已達此最大人數，若還是要加人，請使用「+」。');", true);
            }
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?DD=" + DUTYDATE + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
        }

        protected void Button_FIND_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text.Trim()))
            {
                DUTYDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.AddDays(0).Day.ToString().PadLeft(2, '0');
            }
            else
            {
                string selectString = "SELECT * FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'　AND DUTYDATE='" + TextBox1.Text + "'";
                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");
                if (temp.Rows.Count > 0)
                {

 
                    Response.Redirect("C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?DD=" + TextBox1.Text + "&TPM_FION=" + Request.QueryString["TPM_FION"]);

                   
                    //show();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('該日期無相關資料');location.href='C_ForLeaveOvertime_DUTYPEOPLESET1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
                }
            }
        }

        protected void idno_dt_init()
        {
            idno_dt = new DataTable();
            idno_dt.Columns.Add("IDNO", typeof(string));
            ViewState["idno_dt"] = idno_dt;
        }

        protected void idno_bind()
        {
            idno_dt = ViewState["idno_dt"] as DataTable;
            string strSQL = string.Empty;
            DataTable temp = new DataTable();
            if (idno_dt.Rows.Count == 0)
            {
                strSQL = string.Format("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC,MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_SRANK Like 'G%' AND MZ_EXAD='{0}' AND MZ_EXUNIT='{1}'", Session["ADPMZ_EXAD"], Session["ADPMZ_EXUNIT"]);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                GridView1.DataSource = temp;
                GridView1.DataBind();
                GridView1.SelectedIndex = -1;
            }
            else
            {
                string nid = string.Empty;
                foreach (DataRow dr in idno_dt.Rows)
                {
                    nid += "'" + dr["IDNO"] + "',";
                }
                nid = nid.Substring(0, nid.Length - 1);
                strSQL = string.Format("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC,MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_SRANK Like 'G%' AND MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' AND MZ_ID NOT IN ({2}) ", Session["ADPMZ_EXAD"], Session["ADPMZ_EXUNIT"], nid);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                GridView1.DataSource = temp;
                GridView1.DataBind();
                GridView1.SelectedIndex = -1;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            idno_bind();
            ModalPopupExtender1.Show();
        }

        protected void add_idno2dt(string idno)
        {
            //存取已加入之id
            idno_dt = ViewState["idno_dt"] as DataTable;
            DataRow dr = idno_dt.NewRow();
            dr["IDNO"] = idno;
            idno_dt.Rows.Add(dr);
            ViewState["idno_dt"] = idno_dt;
            //idno_bind();
        }


        //--------------MQ----20110720
        protected void btn_Print_Click(object sender, EventArgs e)
        {

            var a = DUTYDATE;
            double frequanceDay = (double)DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            string nowYear = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');//取得目前民國年分

            //取原始值
//            string selectString = String.Format(@"SELECT A.MZ_ID,A.MZ_NAME ,A.DUTYDATE ,A.MZ_CNO,B.MZ_KCHI MZ_AD
//                                                                        FROM C_DUTYPEOPLE A
//                                                                        LEFT JOIN A_KTYPE B on A.MZ_AD = B.MZ_KCODE
//                                                                        WHERE  A.MZ_ID IN(SELECT DISTINCT  MZ_ID  FROM C_DUTYPEOPLE WHERE (MZ_ID IS NOT NULL AND LEN(RTRIM(MZ_ID))>0)
//                                                                       
//                                                                          )
//                                                                        AND RTRIM(A.MZ_AD)='{0}'
//                                                                        AND RTRIM(A.MZ_UNIT)='{1}'    
//                                                                        ORDER BY A.MZ_ID,A.MZ_NAME,A.DUTYDATE
//                                                                        ", Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString());
//            string selectString = String.Format(@"SELECT A.MZ_ID,A.MZ_NAME ,A.DUTYDATE ,A.MZ_CNO,A.MZ_PNO,B.MZ_KCHI MZ_AD
//                                                                        FROM C_DUTYPEOPLE A
//                                                                        LEFT JOIN A_KTYPE B on A.MZ_AD = B.MZ_KCODE
//                                                                        WHERE  A.MZ_ID IN(SELECT DISTINCT  MZ_ID  FROM C_DUTYPEOPLE WHERE (MZ_ID IS NOT NULL AND LEN(RTRIM(MZ_ID))>0)
//                                                                       
//                                                                          )
//                                                                        AND RTRIM(A.MZ_AD)='{0}'
//                                                                        AND RTRIM(A.MZ_UNIT)='{1}' 
//                                                                        AND A.DUTYDATE  >= '{2}'
//                                                                        ORDER BY dbo.to_number(A.MZ_PNO),A.DUTYDATE
//                                                                        ", Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString(), nowYear+DateTime.Now.ToString("MMdd"));
            string selectString = String.Format(@"SELECT A.MZ_ID,A.MZ_NAME ,A.DUTYDATE ,A.MZ_CNO,A.MZ_PNO,A.MZ_PNO1,B.MZ_KCHI MZ_AD
                                                                        FROM C_DUTYPEOPLE A
                                                                        LEFT JOIN A_KTYPE B on A.MZ_AD = B.MZ_KCODE
                                                                        WHERE  A.MZ_ID IN(SELECT DISTINCT  MZ_ID  FROM C_DUTYPEOPLE WHERE (MZ_ID IS NOT NULL AND LEN(RTRIM(MZ_ID))>0)
                                                                       
                                                                          )
                                                                        AND RTRIM(A.MZ_AD)='{0}'
                                                                        AND RTRIM(A.MZ_UNIT)='{1}' 
                                                                        AND A.DUTYDATE  >= '{2}'
                                                                        ORDER BY dbo.to_number(A.MZ_PNO),A.DUTYDATE
                                                                        ", Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString(), DUTYDATE);
            DataTable dt_Duty = o_DBFactory.ABC_toTest.Create_Table(selectString, "GETVALUE");
            //格式化
            DataTable dt_Duty_Ordered = new DataTable();
            dt_Duty_Ordered.Columns.Add("MZ_NAME");

            string s_d = (int.Parse(DUTYDATE.Substring(0, 3)) + 1911) + "/" + DUTYDATE.Substring(3, 2) + "/" + DUTYDATE.Substring(5, 2);
            DateTime date = Convert.ToDateTime(s_d);

            for (int d = 0; d < frequanceDay; ++d)
            {
                dt_Duty_Ordered.Columns.Add(date.AddDays(d).ToString("MMdd"));
            }
            
            NPOIS.TitleTransferCht = makeColums(frequanceDay);

            //整理值
            if (dt_Duty.Rows.Count > 0)
            {
                DataRow newdr = dt_Duty_Ordered.NewRow();
                string id = "";

                

                foreach (DataRow dr in dt_Duty.Rows)
                {
                    if (String.IsNullOrEmpty(id) || id.Equals(dr["MZ_ID"].ToString()))
                    {
                        id = dr["MZ_ID"].ToString();
                        //newdr["MZ_NAME"] = dr["MZ_NAME"];
                        if (string.IsNullOrEmpty(dr["MZ_PNO1"].ToString()))
                        {
                            newdr["MZ_NAME"] = dr["MZ_PNO"].ToString() + dr["MZ_NAME"];
                        }
                        else
                        {
                            newdr["MZ_NAME"] = dr["MZ_PNO"].ToString() + dr["MZ_NAME"] + "(兼" + dr["MZ_PNO1"].ToString() + ")";
                        }

                       
                        if (!String.IsNullOrEmpty(dr["DUTYDATE"].ToString()))
                        {
                            string settingDay = dr["DUTYDATE"].ToString().Substring(3, 4);

                            //判定年份
                            if ( dr["DUTYDATE"].ToString().Equals(nowYear+settingDay)    &     dt_Duty_Ordered.Columns.Contains(settingDay))                          
                            {
                                newdr[settingDay] = dr["MZ_CNO"].ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        //加入
                        dt_Duty_Ordered.Rows.Add(newdr);
                        //再開新ROW
                        newdr = dt_Duty_Ordered.NewRow();
                        //id = "";

                        id = dr["MZ_ID"].ToString();
                        //newdr["MZ_NAME"] = dr["MZ_NAME"];
                       

                        if (string.IsNullOrEmpty(dr["MZ_PNO1"].ToString()))
                        {
                            newdr["MZ_NAME"] = dr["MZ_PNO"].ToString() + " " + dr["MZ_NAME"];
                        }
                        else
                        {
                            newdr["MZ_NAME"] = dr["MZ_PNO"].ToString()+" " + dr["MZ_NAME"] + "(兼" + dr["MZ_PNO1"].ToString() + ")";
                        }

                        if (!String.IsNullOrEmpty(dr["DUTYDATE"].ToString()))
                        {
                            string settingDay = dr["DUTYDATE"].ToString().Substring(3, 4);

                            //判定年份
                            if (dr["DUTYDATE"].ToString().Equals(nowYear + settingDay) & dt_Duty_Ordered.Columns.Contains(settingDay))
                            {
                                newdr[settingDay] = dr["MZ_CNO"].ToString().Trim();
                            }
                        }
                    }
                }
                //最後一位
                dt_Duty_Ordered.Rows.Add(newdr);
                id = "";
            }
            //局
            string strSQL1 = String.Format(@"SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='{0}' ", Session["ADPMZ_EXAD"].ToString());
            //所
            string strSQL2 = String.Format(@"SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='{0}' ", Session["ADPMZ_EXAD"].ToString());

            string ListTitle = o_DBFactory.ABC_toTest.vExecSQL(strSQL1) + o_DBFactory.ABC_toTest.vExecSQL(strSQL2) + "勤務分配表";
            string printDate = "               列印日期：" + DateTime.Now.ToShortDateString();





            for (int d = 0; d < frequanceDay; ++d)
            {
                dt_Duty_Ordered.Columns[date.AddDays(d).ToString("MMdd")].ColumnName = date.AddDays(d).ToString("MM/dd");
            }

            NPOIS.Dt2Excel(dt_Duty_Ordered, DateTime.Now.ToShortDateString() + ListTitle, ListTitle + printDate);

        }

        private Dictionary<string, string> makeColums(double fd)
        {
            Dictionary<string, string> titleCht = new Dictionary<string, string>();
            titleCht.Add("MZ_NAME".ToLower(), "勤區 姓名");

            for (int d = 0; d < fd; ++d)
            {
                titleCht.Add(DateTime.Now.AddDays(d).ToString("MMdd"), DateTime.Now.AddDays(d).ToString("MM/dd"));
            }
            return titleCht;
        }
        
        /// <summary>
        /// 身分證字號遮蔽 
        /// </summary>
        /// <param name="MZ_ID"></param>
        private string ID_Marker(string MZ_ID){

            if (!String.IsNullOrEmpty(MZ_ID.Trim()))
            {
                var f2 = MZ_ID.Substring(0, 2);//前兩位
                var b3 = MZ_ID.Substring(7, 3);//後三位

                string Marked_ID = f2 + "XXXXX" + b3;

                return Marked_ID;
            }
            else
            {
                return MZ_ID;
            }
         
        }


    }
}
