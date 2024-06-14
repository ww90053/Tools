using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYOVERTIME_KEYIN : System.Web.UI.Page
    {
        List<String> OVERTIME_HOUR_ID = new List<string>();
        List<String> OVERTIME_HOUR_DUTYMONTH = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            }

            //查詢ID
            HttpCookie OVERTIME_HOUR_ID_Cookie = new HttpCookie("C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_ID");
            OVERTIME_HOUR_ID_Cookie = Request.Cookies["C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_ID"];

            if (OVERTIME_HOUR_ID_Cookie == null)
            {
                ViewState["MZ_ID"] = null;
                Response.Cookies["C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_ID"] = TPMPermissions._strDecod(OVERTIME_HOUR_ID_Cookie.Value.ToString());
                Response.Cookies["C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            //查詢姓名
            HttpCookie OVERTIME_HOUR_NAME_Cookie = new HttpCookie("C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_NAME");
            OVERTIME_HOUR_NAME_Cookie = Request.Cookies["C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_NAME"];

            if (OVERTIME_HOUR_NAME_Cookie == null)
            {
                ViewState["MZ_NAME"] = null;
                Response.Cookies["C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_NAME"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_NAME"] = TPMPermissions._strDecod(OVERTIME_HOUR_NAME_Cookie.Value.ToString());
                Response.Cookies["C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_NAME"].Expires = DateTime.Now.AddYears(-1);
            }

            ViewState["XCOUNT"] = Request["XCOUNT"];
            ViewState["AD"] = Request["AD"];
            ViewState["UNIT"] = Request["UNIT"];

   //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel_Overtime_hour);  
     
            if (!IsPostBack)
            {
                C.controlEnable(ref this.Panel1,false );
                Label2.Text = "超勤自由輸入";
                TextBox_DUTYMONTH.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0');

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && OVERTIME_HOUR_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < OVERTIME_HOUR_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == OVERTIME_HOUR_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                }

                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS DUTYMONTH,MZ_ID FROM C_DUTYMONTHOVERTIME_HOUR WHERE 1=1";
                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }
                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID=(SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '%" + ViewState["MZ_NAME"].ToString() + "%')";
                    }
                    if (ViewState["AD"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_AD='" + ViewState["AD"].ToString() + "')";
                    }
                    if (ViewState["UNIT"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_UNIT='" + ViewState["UNIT"].ToString() + "')";
                    }
                    strSQL += " ORDER BY MZ_ID";

                    OVERTIME_HOUR_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                    OVERTIME_HOUR_DUTYMONTH = o_DBFactory.ABC_toTest.DataListArray(strSQL, "DUTYMONTH");

                    Session["OVERTIME_HOUR_ID"] = OVERTIME_HOUR_ID;
                    Session["OVERTIME_HOUR_DUTYMONTH"] = OVERTIME_HOUR_DUTYMONTH;

                    if (OVERTIME_HOUR_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');</script>");
                    }
                    else if (OVERTIME_HOUR_ID.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    //共幾筆資料
                    if (OVERTIME_HOUR_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + OVERTIME_HOUR_ID.Count.ToString() + "筆";
                    }
                }

            }
        }

        protected void finddata(int dataCount)
        {
            OVERTIME_HOUR_ID = Session["OVERTIME_HOUR_ID"] as List<String>;
            OVERTIME_HOUR_DUTYMONTH = Session["OVERTIME_HOUR_DUTYMONTH"] as List<String>;

            string strSQL = "SELECT * FROM C_DUTYMONTHOVERTIME_HOUR WHERE MZ_ID='" + OVERTIME_HOUR_ID[dataCount] + "' AND MZ_YEAR='" + OVERTIME_HOUR_DUTYMONTH[dataCount].Substring(0, 3) + "' AND MZ_MONTH='" + OVERTIME_HOUR_DUTYMONTH[dataCount].Substring(3, 2) + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (temp.Rows.Count == 1)
            {
                TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();


                string selectSQL = "SELECT MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_AD AND MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%') AS MZ_AD1," +
                                         " MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_UNIT AND MZ_KTYPE='25') AS MZ_UNIT1," +
                                         "         (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC," +
                                         " MZ_POLNO,MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[0]["MZ_ID"].ToString() + "'";

                DataTable dt = new DataTable();

                dt = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GET");

                TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString();
                TextBox_MZ_AD1.Text = dt.Rows[0]["MZ_AD1"].ToString();
                TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString();
                TextBox_MZ_UNIT1.Text = dt.Rows[0]["MZ_UNIT1"].ToString();
                TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString();
                TextBox_MZ_POLNO.Text = dt.Rows[0]["MZ_POLNO"].ToString();
                TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString();

                TextBox_DUTYMONTH.Text = temp.Rows[0]["MZ_YEAR"].ToString() + temp.Rows[0]["MZ_MONTH"].ToString();
                TextBox1.Text = temp.Rows[0]["1"].ToString();
                TextBox2.Text = temp.Rows[0]["2"].ToString();
                TextBox3.Text = temp.Rows[0]["3"].ToString();
                TextBox4.Text = temp.Rows[0]["4"].ToString();
                TextBox5.Text = temp.Rows[0]["5"].ToString();
                TextBox6.Text = temp.Rows[0]["6"].ToString();
                TextBox7.Text = temp.Rows[0]["7"].ToString();
                TextBox8.Text = temp.Rows[0]["8"].ToString();
                TextBox9.Text = temp.Rows[0]["9"].ToString();
                TextBox10.Text = temp.Rows[0]["10"].ToString();
                TextBox11.Text = temp.Rows[0]["11"].ToString();
                TextBox12.Text = temp.Rows[0]["12"].ToString();
                TextBox13.Text = temp.Rows[0]["13"].ToString();
                TextBox14.Text = temp.Rows[0]["14"].ToString();
                TextBox15.Text = temp.Rows[0]["15"].ToString();
                TextBox16.Text = temp.Rows[0]["16"].ToString();
                TextBox17.Text = temp.Rows[0]["17"].ToString();
                TextBox18.Text = temp.Rows[0]["18"].ToString();
                TextBox19.Text = temp.Rows[0]["19"].ToString();
                TextBox20.Text = temp.Rows[0]["20"].ToString();
                TextBox21.Text = temp.Rows[0]["21"].ToString();
                TextBox22.Text = temp.Rows[0]["22"].ToString();
                TextBox23.Text = temp.Rows[0]["23"].ToString();
                TextBox24.Text = temp.Rows[0]["24"].ToString();
                TextBox25.Text = temp.Rows[0]["25"].ToString();
                TextBox26.Text = temp.Rows[0]["26"].ToString();
                TextBox27.Text = temp.Rows[0]["27"].ToString();
                TextBox28.Text = temp.Rows[0]["28"].ToString();
                TextBox29.Text = temp.Rows[0]["29"].ToString();
                TextBox30.Text = temp.Rows[0]["30"].ToString();
                TextBox31.Text = temp.Rows[0]["31"].ToString();
                TextBox_TOTAL.Text = temp.Rows[0]["TOTAL"].ToString();
                TextBox_MZ_REMARK.Text = temp.Rows[0]["MZ_REMARK"].ToString();
                TextBox_MZ_BUDGET_HOUR.Text = temp.Rows[0]["MZ_BUDGET_HOUR"].ToString();
                TextBox_MZ_REAL_HOUR.Text = temp.Rows[0]["MZ_REAL_HOUR"].ToString();
                TextBox_MZ_BALANCE_HOUR.Text = temp.Rows[0]["MZ_BALANCE_HOUR"].ToString();
                TextBox_MZ_PROJECT_HOUR.Text = temp.Rows[0]["MZ_PROJECT_HOUR"].ToString();
                TextBox_MZ_PROJECT_PAY.Text = temp.Rows[0]["MZ_PROJECT_PAY"].ToString();
                TextBox_PAY1.Text = temp.Rows[0]["PAY1"].ToString();
                TextBox_PROFESS.Text = temp.Rows[0]["PROFESS"].ToString();
                TextBox_BOSS.Text = temp.Rows[0]["BOSS"].ToString();
                TextBox_MZ_HOUR_PAY.Text = temp.Rows[0]["MZ_HOUR_PAY"].ToString();
                TextBox_MZ_OVERTIME_PAY.Text = temp.Rows[0]["MZ_OVERTIME_PAY"].ToString();
                TextBox_MZ_HIGH_PAY.Text = temp.Rows[0]["MZ_HIGH_PAY"].ToString();
                TextBox_MZ_BUDGET_PAY.Text = temp.Rows[0]["MZ_BUDGET_PAY"].ToString();
                TextBox_MZ_HOUR_LIMIT.Text = temp.Rows[0]["MZ_HOUR_LIMIT"].ToString();
                TextBox_MZ_MONEY_LIMIT.Text = temp.Rows[0]["MZ_MONEY_LIMIT"].ToString();
            }
            btUpdate.Enabled = true;
            btDelete.Enabled = true;
        }
        protected static int stringtoInt(TextBox tb1)
        {

            int x = 0;
            if (tb1.Text.Trim().Length == 0)
            {
                x = 0;
            }
            else
            {
                x = int.Parse(tb1.Text.Trim());
            }
            return x;
        }

        protected void HoursSum()
        {
            TextBox_TOTAL.Text = (stringtoInt(TextBox1) + stringtoInt(TextBox2) + stringtoInt(TextBox3) + stringtoInt(TextBox4)
                              + stringtoInt(TextBox5) + stringtoInt(TextBox6) + stringtoInt(TextBox7) + stringtoInt(TextBox8)
                              + stringtoInt(TextBox9) + stringtoInt(TextBox10) + stringtoInt(TextBox11) + stringtoInt(TextBox12)
                              + stringtoInt(TextBox13) + stringtoInt(TextBox14) + stringtoInt(TextBox15) + stringtoInt(TextBox16)
                              + stringtoInt(TextBox17) + stringtoInt(TextBox18) + stringtoInt(TextBox19) + stringtoInt(TextBox20)
                              + stringtoInt(TextBox21) + stringtoInt(TextBox22) + stringtoInt(TextBox23) + stringtoInt(TextBox24)
                              + stringtoInt(TextBox25) + stringtoInt(TextBox26) + stringtoInt(TextBox27) + stringtoInt(TextBox28)
                              + stringtoInt(TextBox29) + stringtoInt(TextBox30) + stringtoInt(TextBox31)).ToString();

            TextBox_MZ_BUDGET_HOUR.Text = TextBox_TOTAL.Text;
            TextBox_MZ_REAL_HOUR.Text = TextBox_TOTAL.Text;
            TextBox_MZ_OVERTIME_PAY.Text = (MathHelper.Round(float.Parse(TextBox_TOTAL.Text) * (float.Parse(TextBox_MZ_HOUR_PAY.Text)))).ToString();
            TextBox_MZ_BUDGET_PAY.Text = TextBox_MZ_OVERTIME_PAY.Text;
            TextBox_MZ_HIGH_PAY.Text = TextBox_MZ_OVERTIME_PAY.Text;
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_AD.Text = TextBox_MZ_AD.Text.Trim().ToUpper();
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");

            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_AD1.Text = string.Empty;
                TextBox_MZ_AD.Text = string.Empty;
                TextBox_MZ_AD.Focus();
            }
            else
            {
                TextBox_MZ_AD1.Text = CName;
                TextBox_MZ_UNIT.Focus();
            }
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_UNIT.Text = TextBox_MZ_UNIT.Text.Trim().ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");

            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_UNIT1.Text = string.Empty;
                TextBox_MZ_UNIT.Text = string.Empty;
                TextBox_MZ_UNIT.Focus();
            }
            else
            {
                TextBox_MZ_UNIT1.Text = CName;
                TextBox1.Focus();
            }

        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?MZ_KTYPE=25&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.ToUpper();

            string strSQL = "SELECT MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04') AS MZ_AD1," +
                                  " MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') AS MZ_UNIT1," +
                                  " MZ_NAME,MZ_POLNO,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC," +
                                  " (SELECT PAY1 FROM B_SALARY WHERE ORIGIN2=A_DLBASE.MZ_SPT) AS PAY1, " +
                                  " NVL((SELECT PAY FROM B_PROFESS WHERE \"ID\"=A_DLBASE.MZ_SRANK),0) AS PROFESS," +
                                  " NVL((SELECT PAY FROM B_BOSS WHERE \"ID\"=A_DLBASE.MZ_SRANK AND A_DLBASE.MZ_PCHIEF IS NOT NULL),0) AS BOSS" +
                                  " FROM A_DLBASE WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無此人');", true);
            }
            else if (dt.Rows.Count == 1)
            {
                TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString().Trim();
                TextBox_MZ_AD1.Text = dt.Rows[0]["MZ_AD1"].ToString().Trim();
                TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_UNIT1.Text = dt.Rows[0]["MZ_UNIT1"].ToString().Trim();
                TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_POLNO.Text = dt.Rows[0]["MZ_POLNO"].ToString().Trim();
                TextBox_BOSS.Text = dt.Rows[0]["BOSS"].ToString().Trim();
                TextBox_PAY1.Text = dt.Rows[0]["PAY1"].ToString().Trim();
                TextBox_PROFESS.Text = dt.Rows[0]["PROFESS"].ToString().Trim();
                TextBox_MZ_HOUR_PAY.Text = (MathHelper.Round((float.Parse(dt.Rows[0]["BOSS"].ToString().Trim())
                                                      + float.Parse(dt.Rows[0]["PAY1"].ToString().Trim())
                                                      + float.Parse(dt.Rows[0]["PROFESS"].ToString().Trim())) / 240, 2)).ToString();

                string MZ_HOUR_LIMIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + o_A_DLBASE.PAD(o_str.tosql(TextBox_MZ_ID.Text)) +
                                                                                               "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(o_str.tosql(TextBox_MZ_ID.Text)) +
                                                                                               "' AND MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text) + "'");

                string MZ_HOUR_LIMIT1 = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + o_A_DLBASE.PAD(o_str.tosql(TextBox_MZ_ID.Text)) +
                                                                                              "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(o_str.tosql(TextBox_MZ_ID.Text)) + "'");

                string MZ_MONEY_LIMIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + o_A_DLBASE.PAD(o_str.tosql(TextBox_MZ_ID.Text)) +
                                                                                               "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(o_str.tosql(TextBox_MZ_ID.Text)) +
                                                                                               "' AND MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text) + "'");

                string MZ_MONEY_LIMIT1 = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + o_A_DLBASE.PAD(o_str.tosql(TextBox_MZ_ID.Text)) +
                                                                                              "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(o_str.tosql(TextBox_MZ_ID.Text)) + "'");

                if (!string.IsNullOrEmpty(MZ_HOUR_LIMIT))
                {
                    TextBox_MZ_HOUR_LIMIT.Text = MZ_HOUR_LIMIT;
                }
                else if (string.IsNullOrEmpty(MZ_HOUR_LIMIT) && !string.IsNullOrEmpty(MZ_HOUR_LIMIT1))
                {
                    TextBox_MZ_HOUR_LIMIT.Text = MZ_HOUR_LIMIT1;
                }
                else
                {
                    TextBox_MZ_HOUR_LIMIT.Text = "-1";
                }

                if (!string.IsNullOrEmpty(MZ_MONEY_LIMIT))
                {
                    TextBox_MZ_MONEY_LIMIT.Text = MZ_MONEY_LIMIT;
                }
                else if (string.IsNullOrEmpty(MZ_MONEY_LIMIT) && !string.IsNullOrEmpty(MZ_MONEY_LIMIT1))
                {
                    TextBox_MZ_MONEY_LIMIT.Text = MZ_MONEY_LIMIT1;
                }
                else
                {
                    TextBox_MZ_MONEY_LIMIT.Text = "-1";
                }

            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_DUTYOVERTIME_KEYIN_SEARCH.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=380,height=200,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO C_DUTYMONTHOVERTIME_HOUR (MZ_ID,MZ_YEAR,MZ_MONTH,\"1\",\"2\",\"3\",\"4\"," +
                                                                       " \"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\",\"12\"," +
                                                                       " \"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\"," +
                                                                       " \"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\"," +
                                                                       " \"29\",\"30\",\"31\",TOTAL,MZ_REMARK,MZ_BUDGET_HOUR,MZ_REAL_HOUR," +
                                                                       " MZ_BALANCE_HOUR,MZ_PROJECT_HOUR,MZ_PROJECT_PAY,PAY1,PROFESS,BOSS," +
                                                                       " MZ_HOUR_PAY,MZ_OVERTIME_PAY,MZ_HIGH_PAY,MZ_BUDGET_PAY,MZ_HOUR_LIMIT," +
                                                                       " MZ_MONEY_LIMIT)" +
                                                              " VALUES (@MZ_ID,@MZ_YEAR,@MZ_MONTH,:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12," +
                                                                      " :13,:14,:15,:16,:17,:18,:19,:20,:21,:22,:23,:24,:25,:26,:27,:28,:29," +
                                                                      " :30,:31,@TOTAL,@MZ_REMARK,@MZ_BUDGET_HOUR,@MZ_REAL_HOUR,@MZ_BALANCE_HOUR," +
                                                                      " @MZ_PROJECT_HOUR,@MZ_PROJECT_PAY,@PAY1,@PROFESS,@BOSS,@MZ_HOUR_PAY," +
                                                                      " @MZ_OVERTIME_PAY,@MZ_HIGH_PAY,@MZ_BUDGET_PAY,@MZ_HOUR_LIMIT,@MZ_MONEY_LIMIT) ";
            btUpdate.Enabled = false;
            btNEXT.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btCancel.Enabled = true;
            C.controlEnable(ref this.Panel1, true );
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";
            ViewState["CMDSQL"] = "UPDATE C_DUTYMONTHOVERTIME_HOUR SET MZ_ID = @MZ_ID,MZ_YEAR = @MZ_YEAR,MZ_MONTH = @MZ_MONTH,\"1\" = :1,\"2\"= :2,\"3\" = :3,\"4\" = :4," +
                                                 " \"5\" = :5,\"6\" = :6,\"7\" = :7,\"8\"= :8,\"9\" = :9,\"10\" = :10,\"11\" = :11,\"12\" = :12,\"13\" = :13," +
                                                 " \"14\" = :14,\"15\" = :15,\"16\"= :16,\"17\" = :17,\"18\" = :18,\"19\" = :19,\"20\" = :20,\"21\" = :21," +
                                                 " \"22\" = :22,\"23\" = :23,\"24\" = :24,\"25\" = :25,\"26\" = :26,\"27\" = :27,\"28\" = :28,\"29\" = :29," +
                                                 " \"30\" = :30,\"31\" = :31,TOTAL = @TOTAL,MZ_REMARK = @MZ_REMARK,MZ_BUDGET_HOUR = @MZ_BUDGET_HOUR," +
                                                 " MZ_REAL_HOUR = @MZ_REAL_HOUR,MZ_BALANCE_HOUR = @MZ_BALANCE_HOUR,MZ_PROJECT_HOUR = @MZ_PROJECT_HOUR," +
                                                 " MZ_PROJECT_PAY = @MZ_PROJECT_PAY,PAY1 = @PAY1,PROFESS = @PROFESS,BOSS = @BOSS,MZ_HOUR_PAY = @MZ_HOUR_PAY," +
                                                 " MZ_OVERTIME_PAY = @MZ_OVERTIME_PAY,MZ_HIGH_PAY = @MZ_HIGH_PAY,MZ_BUDGET_PAY = @MZ_BUDGET_PAY," +
                                                 " MZ_HOUR_LIMIT = @MZ_HOUR_LIMIT,MZ_MONEY_LIMIT = @MZ_MONEY_LIMIT WHERE MZ_ID='"
                                                 + o_str.tosql(TextBox_MZ_ID.Text.Trim().ToUpper()) + "' AND MZ_YEAR='"
                                                 + o_str.tosql(TextBox_DUTYMONTH.Text.Trim().Substring(0, 3)) + "'AND MZ_MONTH='"
                                                 + o_str.tosql(TextBox_DUTYMONTH.Text.Trim().Substring(3, 2)) + "'";

            btUpdate.Enabled = false;
            btNEXT.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btCancel.Enabled = true;
            C.controlEnable(ref this.Panel1, true );

        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            HoursSum();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);

                cmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                cmd.Parameters.Add("MZ_YEAR", SqlDbType.VarChar).Value = TextBox_DUTYMONTH.Text.Trim().Substring(0, 3);
                cmd.Parameters.Add("MZ_MONTH", SqlDbType.VarChar).Value = TextBox_DUTYMONTH.Text.Trim().Substring(3, 2);
                cmd.Parameters.Add("1", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox1.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox1.Text.Trim());
                cmd.Parameters.Add("2", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox2.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox2.Text.Trim());
                cmd.Parameters.Add("3", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox3.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox3.Text.Trim());
                cmd.Parameters.Add("4", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox4.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox4.Text.Trim());
                cmd.Parameters.Add("5", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox5.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox5.Text.Trim());
                cmd.Parameters.Add("6", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox6.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox6.Text.Trim());
                cmd.Parameters.Add("7", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox7.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox7.Text.Trim());
                cmd.Parameters.Add("8", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox8.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox8.Text.Trim());
                cmd.Parameters.Add("9", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox9.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox9.Text.Trim());
                cmd.Parameters.Add("10", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox10.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox10.Text.Trim());
                cmd.Parameters.Add("11", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox11.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox11.Text.Trim());
                cmd.Parameters.Add("12", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox12.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox12.Text.Trim());
                cmd.Parameters.Add("13", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox13.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox13.Text.Trim());
                cmd.Parameters.Add("14", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox14.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox14.Text.Trim());
                cmd.Parameters.Add("15", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox15.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox15.Text.Trim());
                cmd.Parameters.Add("16", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox16.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox16.Text.Trim());
                cmd.Parameters.Add("17", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox17.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox17.Text.Trim());
                cmd.Parameters.Add("18", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox18.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox18.Text.Trim());
                cmd.Parameters.Add("19", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox19.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox19.Text.Trim());
                cmd.Parameters.Add("20", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox20.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox20.Text.Trim());
                cmd.Parameters.Add("21", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox21.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox21.Text.Trim());
                cmd.Parameters.Add("22", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox22.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox22.Text.Trim());
                cmd.Parameters.Add("23", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox23.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox23.Text.Trim());
                cmd.Parameters.Add("24", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox24.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox24.Text.Trim());
                cmd.Parameters.Add("25", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox25.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox25.Text.Trim());
                cmd.Parameters.Add("26", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox26.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox26.Text.Trim());
                cmd.Parameters.Add("27", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox27.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox27.Text.Trim());
                cmd.Parameters.Add("28", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox28.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox28.Text.Trim());
                cmd.Parameters.Add("29", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox29.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox29.Text.Trim());
                cmd.Parameters.Add("30", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox30.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox30.Text.Trim());
                cmd.Parameters.Add("31", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox31.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox31.Text.Trim());
                cmd.Parameters.Add("TOTAL", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_TOTAL.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_TOTAL.Text.Trim());
                cmd.Parameters.Add("MZ_REMARK", SqlDbType.VarChar).Value = TextBox_MZ_REMARK.Text.Trim();
                cmd.Parameters.Add("MZ_BUDGET_HOUR", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_BUDGET_HOUR.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_BUDGET_HOUR.Text.Trim());
                cmd.Parameters.Add("MZ_REAL_HOUR", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_REAL_HOUR.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_REAL_HOUR.Text.Trim());
                cmd.Parameters.Add("MZ_BALANCE_HOUR", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_BALANCE_HOUR.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_BALANCE_HOUR.Text.Trim());
                cmd.Parameters.Add("MZ_PROJECT_HOUR", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_PROJECT_HOUR.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_PROJECT_HOUR.Text.Trim());
                cmd.Parameters.Add("MZ_PROJECT_PAY", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_PROJECT_PAY.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_PROJECT_PAY.Text.Trim());
                cmd.Parameters.Add("PAY1", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_PAY1.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_PAY1.Text.Trim());
                cmd.Parameters.Add("PROFESS", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_PROFESS.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_PROFESS.Text.Trim());
                cmd.Parameters.Add("BOSS", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_BOSS.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_BOSS.Text.Trim());
                cmd.Parameters.Add("MZ_HOUR_PAY", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_HOUR_PAY.Text.Trim()) ? Convert.DBNull : float.Parse(TextBox_MZ_HOUR_PAY.Text.Trim());
                cmd.Parameters.Add("MZ_OVERTIME_PAY", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_OVERTIME_PAY.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_OVERTIME_PAY.Text.Trim());
                cmd.Parameters.Add("MZ_HIGH_PAY", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_HIGH_PAY.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_HIGH_PAY.Text.Trim());
                cmd.Parameters.Add("MZ_BUDGET_PAY", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_BUDGET_PAY.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_BUDGET_PAY.Text.Trim());
                cmd.Parameters.Add("MZ_HOUR_LIMIT", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_HOUR_LIMIT.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_HOUR_LIMIT.Text.Trim());
                cmd.Parameters.Add("MZ_MONEY_LIMIT", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_MONEY_LIMIT.Text.Trim()) ? Convert.DBNull : int.Parse(TextBox_MZ_MONEY_LIMIT.Text.Trim());

                try
                {
                    cmd.ExecuteNonQuery();

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('C_ForLeaveOvertime_DUTYOVERTIME_KEYIN.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));


                        OVERTIME_HOUR_ID = Session["OVERTIME_HOUR_ID"] as List<string>;

                        if (int.Parse(xcount.Text.Trim()) == 0 && OVERTIME_HOUR_ID.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && OVERTIME_HOUR_ID.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == OVERTIME_HOUR_ID.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < OVERTIME_HOUR_ID.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                    }
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                    ViewState.Remove("Mode");
                    C.controlEnable(ref this.Panel1, false);
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('C_ForLeaveOvertime_DUTYOVERTIME_KEYIN.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    }
                }
                finally
                {
                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {

                foreach (object tb in Panel_Overtime_hour.Controls)
                {

                    if (tb is TextBox)
                    {
                        TextBox tbox = tb as TextBox;
                        tbox.Text = "";
                    }
                }
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
            }
            else if (ViewState["Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                OVERTIME_HOUR_ID = Session["OVERTIME_HOUR_ID"] as List<string>;

                if (int.Parse(xcount.Text.Trim()) == 0 && OVERTIME_HOUR_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && OVERTIME_HOUR_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == OVERTIME_HOUR_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < OVERTIME_HOUR_ID.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }

            btOK.Enabled = false;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            C.controlEnable(ref this.Panel1, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE " +
                                  "FROM " +
                                            "C_DUTYMONTHOVERTIME_HOUR " +
                                  "WHERE " +
                                            "MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) +
                                            "' AND MZ_YEAR='" + o_str.tosql(TextBox_DUTYMONTH.Text.Trim().Substring(0, 3)) +
                                            "' AND MZ_MONTH='" + o_str.tosql(TextBox_DUTYMONTH.Text.Trim().Substring(3, 2)) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                OVERTIME_HOUR_ID = Session["OVERTIME_HOUR_ID"] as List<String>;
                OVERTIME_HOUR_DUTYMONTH = Session["OVERTIME_HOUR_DUTYMONTH"] as List<String>;
                OVERTIME_HOUR_ID.RemoveAt(int.Parse(xcount.Text.Trim()));
                OVERTIME_HOUR_DUTYMONTH.RemoveAt(int.Parse(xcount.Text.Trim()));

                if (OVERTIME_HOUR_ID.Count == 0)
                {
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_ForLeaveOvertime_DUTYOVERTIME_KEYIN.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (OVERTIME_HOUR_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + OVERTIME_HOUR_ID.Count.ToString() + "筆";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                }

                btCancel.Enabled = false;
                btOK.Enabled = false;

                btInsert.Enabled = true;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != OVERTIME_HOUR_ID.Count - 1)
                {
                    btNEXT.Enabled = true;
                }
                if (int.Parse(xcount.Text) == 0)
                {
                    btUpper.Enabled = false;
                }
            }
            else if (int.Parse(xcount.Text) == 0)
            {
                finddata(int.Parse(xcount.Text));
                btUpper.Enabled = false;
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + OVERTIME_HOUR_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == OVERTIME_HOUR_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == OVERTIME_HOUR_ID.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + OVERTIME_HOUR_ID.Count.ToString() + "筆";
        }
    }
}
