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
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;


namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTYHOUR_KEYIN : System.Web.UI.Page
    {
        List<String> ONDUTY_HOUR_ID = new List<string>();
        List<String> ONDUTY_HOUR_DUTYMONTH = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel_ONDUTYHOUR_KEYIN);

                //20140707
                //輪值類型 
                string strsql = "SELECT DUTY_KIND, DUTY_KINDNAME FROM C_ONDUTY_CODE WHERE DUTY_TYPE='1' ";
                ddl_type.DataSource = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");
                ddl_type.DataTextField = "DUTY_KINDNAME";
                ddl_type.DataValueField = "DUTY_KIND";
                ddl_type.DataBind();
                //值勤別
                ddl_type_SelectedIndexChanged(null, null);

                C.controlEnable(ref this.Panel_ONDUTYHOUR_KEYIN, false);
                HttpCookie ONDUTYHOUR_KEYIN_ID_Cookie = new HttpCookie("ONDUTYHOUR_KEYIN_Search_ID");
                ONDUTYHOUR_KEYIN_ID_Cookie = Request.Cookies["ONDUTYHOUR_KEYIN_Search_ID"];


                if (ONDUTYHOUR_KEYIN_ID_Cookie == null)
                {
                    ViewState["MZ_ID"] = null;
                    Response.Cookies["ONDUTYHOUR_KEYIN_Search_ID"].Expires = DateTime.Now.AddYears(-1);
                }
                else
                {
                    ViewState["MZ_ID"] = TPMPermissions._strDecod(ONDUTYHOUR_KEYIN_ID_Cookie.Value.ToString());
                    Response.Cookies["ONDUTYHOUR_KEYIN_Search_ID"].Expires = DateTime.Now.AddYears(-1);

                }
                HttpCookie ONDUTYHOUR_KEYIN_NAME_Cookie = new HttpCookie("ONDUTYHOUR_KEYIN_Search_NAME");
                ONDUTYHOUR_KEYIN_NAME_Cookie = Request.Cookies["ONDUTYHOUR_KEYIN_Search_NAME"];


                if (ONDUTYHOUR_KEYIN_NAME_Cookie == null)
                {
                    ViewState["MZ_NAME"] = null;
                    Response.Cookies["ONDUTYHOUR_KEYIN_Search_NAME"].Expires = DateTime.Now.AddYears(-1);
                }
                else
                {
                    ViewState["MZ_NAME"] = TPMPermissions._strDecod(ONDUTYHOUR_KEYIN_NAME_Cookie.Value.ToString());
                    Response.Cookies["ONDUTYHOUR_KEYIN_Search_NAME"].Expires = DateTime.Now.AddYears(-1);

                }

                ViewState["AD"] = Request["MZ_EXAD"];
                ViewState["UNIT"] = Request["MZ_EXUNIT"];
                ViewState["MZ_MONTH"] = Request["MZ_MONTH"];



                Label2.Text = "輪值表排班";
                TextBox_DUTYMONTH.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0');

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && ONDUTY_HOUR_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < ONDUTY_HOUR_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == ONDUTY_HOUR_ID.Count - 1)
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
                    string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME,  DUTY_KINDNAME ,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                                           "\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR " +
                                           " LEFT JOIN  C_ONDUTY_CODE ON DUTY_KIND= MZ_ONDUTY_KIND WHERE 1=1";

                    string where = "";
                    
                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        where += " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }
                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        where += " AND MZ_ID in (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '%" + ViewState["MZ_NAME"].ToString() + "%')";
                    }
                    if (ViewState["AD"].ToString() != "")
                    {
                        where += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + ViewState["AD"].ToString() + "')";
                    }
                    if (ViewState["UNIT"].ToString() != "")
                    {
                        where += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT='" + ViewState["UNIT"].ToString() + "')";
                    }
                    if (ViewState["MZ_MONTH"].ToString() != "")
                    {
                        where += " AND MZ_YEAR = " + ViewState["MZ_MONTH"].ToString().Substring(0, 3) + " AND MZ_MONTH =" + ViewState["MZ_MONTH"].ToString().Substring(3, 2);
                    }
                    where += " ORDER BY C_ONDUTY_HOUR.MZ_ID";
                    SqlDataSource1.SelectCommand = strSQL + where;
                    SqlDataSource1.SelectParameters.Clear();
                    GridView1.DataBind();
                    strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID FROM C_ONDUTY_HOUR WHERE 1=1";


                    ONDUTY_HOUR_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL + where, "MZ_ID");
                    ONDUTY_HOUR_DUTYMONTH = o_DBFactory.ABC_toTest.DataListArray(strSQL + where, "MZ_DUTYMONTH");

                    ViewState["ONDUTY_HOUR_ID"] = ONDUTY_HOUR_ID;
                    ViewState["ONDUTY_HOUR_DUTYMONTH"] = ONDUTY_HOUR_DUTYMONTH;

                    //Session["ONDUTY_HOUR_ID"] = ONDUTY_HOUR_ID;
                    //Session["ONDUTY_HOUR_DUTYMONTH"] = ONDUTY_HOUR_DUTYMONTH;

                    if (ONDUTY_HOUR_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');</script>");
                    }
                    else if (ONDUTY_HOUR_ID.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        GridView1.SelectedIndex = 0;
                    }
                    //可以像personal1-1選人  但要加入C_Personal_Search_Result.aspx
                    //else if (ONDUTY_HOUR_ID.Count > 5)
                    //{
                    //    HttpCookie Cookie1 = new HttpCookie("Personal_NAME");
                    //    Cookie1.Value = TPMPermissions._strEncood(ViewState["MZ_NAME"].ToString());
                    //    Response.Cookies.Add(Cookie1);
                    //    xcount.Text = "0";

                    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_Personal_Search_Result.aspx?AD=" + (ViewState["AD"] == null ? string.Empty : ViewState["AD"].ToString()) +
                    //                                                                                                                         "&UNIT=" + (ViewState["UNIT"] == null ? string.Empty : ViewState["UNIT"].ToString()) +
                    //                                                                                                                         "&MONTH=" + (ViewState["MZ_MONTH"] == null ? string.Empty : ViewState["MZ_MONTH"].ToString()) +
                    //                                                                                                                         "&DTable=ONDUTYHOUR&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin1','top=200,left=680,width=350,height=400,scrollbars=yes,toolbar=no,location=no,menubar=no,status=no,resizable=no');", true);
                    //}
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        GridView1.SelectedIndex = 0;
                    }
                    //共幾筆資料
                    if (ONDUTY_HOUR_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + ONDUTY_HOUR_ID.Count.ToString() + "筆";
                    }
                }
                DropDownList_MZ_NAME.DataBind();
                TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue;
                TextBox_MZ_UNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') AS MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'");
                C.controlEnable(ref this.Panel_ONDUTYHOUR_KEYIN, false);
            }
        }

        protected void finddata(int dataCount)
        {

            //ONDUTY_HOUR_ID = Session["ONDUTY_HOUR_ID"] as List<string>;
            //ONDUTY_HOUR_DUTYMONTH = Session["ONDUTY_HOUR_DUTYMONTH"] as List<string>;


            ONDUTY_HOUR_ID = ViewState["ONDUTY_HOUR_ID"] as List<string>;
            ONDUTY_HOUR_DUTYMONTH = ViewState["ONDUTY_HOUR_DUTYMONTH"] as List<string>;

            string strSQL = "SELECT * FROM C_ONDUTY_HOUR WHERE MZ_ID='" + ONDUTY_HOUR_ID[dataCount] +
                                                        "' AND MZ_YEAR='" + ONDUTY_HOUR_DUTYMONTH[dataCount].Substring(0, 3) +
                                                        "' AND MZ_MONTH='" + ONDUTY_HOUR_DUTYMONTH[dataCount].Substring(3, 2) + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (temp.Rows.Count == 1)
            {
                TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();
                DropDownList_MZ_NAME.DataBind();
                DropDownList_MZ_NAME.SelectedItem.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[0]["MZ_ID"].ToString() + "'");


                
                TextBox_DUTYMONTH.Text = temp.Rows[0]["MZ_YEAR"].ToString() + temp.Rows[0]["MZ_MONTH"].ToString();
                 ddl_Kind.SelectedValue = temp.Rows[0]["MZ_ONDUTY_KIND"].ToString();
                //DropDownList_MZ_ONDUTY_KIND.SelectedValue = temp.Rows[0]["MZ_ONDUTY_KIND"].ToString();
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

                //20140707
                lb_PAY.Text = temp.Rows[0]["MZ_ONDUTY_PAY"].ToString();
                //DropDownList_MZ_ONDUTY_PAY.SelectedValue = temp.Rows[0]["MZ_ONDUTY_PAY"].ToString();
                TextBox_MZ_REMARK.Text = temp.Rows[0]["MZ_REMARK"].ToString();
                TextBox_TOTAL.Text = temp.Rows[0]["MZ_TOTAL_HOUR"].ToString();
            }
            btUpdate.Enabled = true;
            btDelete.Enabled = true;
        }

        protected static int OneOrZero(TextBox tb1)
        {
            int x;

            if (string.IsNullOrEmpty(tb1.Text))
            {
                x = 0;
            }
            else
            {
                x = 1;
            }

            return x;
        }

        protected void count()
        {
            TextBox_TOTAL.Text = (OneOrZero(TextBox1) + OneOrZero(TextBox2) + OneOrZero(TextBox3) + OneOrZero(TextBox4)
                                + OneOrZero(TextBox5) + OneOrZero(TextBox6) + OneOrZero(TextBox7) + OneOrZero(TextBox8)
                                + OneOrZero(TextBox9) + OneOrZero(TextBox10) + OneOrZero(TextBox11) + OneOrZero(TextBox12)
                                + OneOrZero(TextBox13) + OneOrZero(TextBox14) + OneOrZero(TextBox15) + OneOrZero(TextBox16)
                                + OneOrZero(TextBox17) + OneOrZero(TextBox18) + OneOrZero(TextBox19) + OneOrZero(TextBox20)
                                + OneOrZero(TextBox21) + OneOrZero(TextBox22) + OneOrZero(TextBox23) + OneOrZero(TextBox24)
                                + OneOrZero(TextBox25) + OneOrZero(TextBox26) + OneOrZero(TextBox27) + OneOrZero(TextBox28)
                                + OneOrZero(TextBox29) + OneOrZero(TextBox30) + OneOrZero(TextBox31)).ToString();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_Search.aspx?TableName=ONDUTYHOUR&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);

        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";

            ViewState["CMDSQL"] = "INSERT INTO " +
                                           "C_ONDUTY_HOUR" +
                                           "(MZ_ID,MZ_YEAR,MZ_MONTH,MZ_ONDUTY_KIND,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                                           "\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\"," +
                                           "MZ_ONDUTY_PAY,MZ_TOTAL_PAY,MZ_TOTAL_HOUR,MZ_REMARK) " +
                                   "VALUES (@MZ_ID,@MZ_YEAR,@MZ_MONTH,@MZ_ONDUTY_KIND,:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11," +
                                           ":12,:13,:14,:15,:16,:17,:18,:19,:20,:21,:22,:23,:24,:25,:26,:27,:28,:29,:30,:31," +
                                           ":MZ_ONDUTY_PAY,@MZ_TOTAL_PAY,@MZ_TOTAL_HOUR,@MZ_REMARK) ";
            btUpdate.Enabled = false;
            btNEXT.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btCancel.Enabled = true;
            C.controlEnable(ref this.Panel_ONDUTYHOUR_KEYIN, true);
            gv_show();
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";

            ViewState["CMDSQL"] = "UPDATE " +
                                             "C_ONDUTY_HOUR " +
                                  "SET " +
                                             "MZ_ID = @MZ_ID,MZ_YEAR = @MZ_YEAR,MZ_MONTH = @MZ_MONTH,MZ_ONDUTY_KIND = @MZ_ONDUTY_KIND," +
                                             "\"1\" = :1,\"2\" = :2,\"3\"= :3,\"4\" = :4,\"5\" = :5,\"6\" = :6,\"7\" = :7,\"8\" = :8,\"9\" = :9,\"10\" = :10,\"11\" = :11,\"12\" = :12," +
                                             "\"13\" = :13,\"14\" = :14,\"15\" = :15,\"16\" = :16,\"17\" = :17,\"18\" = :18,\"19\" = :19,\"20\" = :20,\"21\" = :21,\"22\" = :22," +
                                             "\"23\" = :23,\"24\" = :24,\"25\" = :25,\"26\" = :26,\"27\" = :27,\"28\" = :28,\"29\" = :29,\"30\" = :30,\"31\"= :31," +
                                             "MZ_ONDUTY_PAY = @MZ_ONDUTY_PAY,MZ_TOTAL_PAY = @MZ_TOTAL_PAY,MZ_TOTAL_HOUR = @MZ_TOTAL_HOUR,MZ_REMARK=@MZ_REMARK WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_YEAR = '" + TextBox_DUTYMONTH.Text.Trim().Substring(0, 3) + "' AND MZ_MONTH = '" + TextBox_DUTYMONTH.Text.Trim().Substring(3, 2) + "'";

            HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
            Cookie1.Value = TextBox_MZ_ID.Text.Trim();
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_YEAR");
            Cookie2.Value = TextBox_DUTYMONTH.Text.Trim().Substring(0, 3);
            Response.Cookies.Add(Cookie2);

            HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_MONTH");
            Cookie3.Value = TextBox_DUTYMONTH.Text.Trim().Substring(3, 2);
            Response.Cookies.Add(Cookie3);

            btUpdate.Enabled = false;
            btNEXT.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btCancel.Enabled = true;
            gv_show();
            C.controlEnable(ref this.Panel_ONDUTYHOUR_KEYIN, true);
        }

        protected void gv_show()
        {
            if (ViewState["MZ_ID"] != null)
            {
                string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME, DUTY_KINDNAME ,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                                       "\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR WHERE 1=1";
                if (ViewState["MZ_ID"].ToString() != "")
                {
                    strSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                }
                if (ViewState["MZ_NAME"].ToString() != "")
                {
                    strSQL += " AND MZ_ID in (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '%" + ViewState["MZ_NAME"].ToString() + "%')";
                }
                if (ViewState["AD"].ToString() != "")
                {
                    strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + ViewState["AD"].ToString() + "')";
                }
                if (ViewState["UNIT"].ToString() != "")
                {
                    strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT='" + ViewState["UNIT"].ToString() + "')";
                }
                if (ViewState["MZ_MONTH"].ToString() != "")
                {
                    strSQL += " AND MZ_YEAR = " + ViewState["MZ_MONTH"].ToString().Substring(0, 3) + " AND MZ_MONTH =" + ViewState["MZ_MONTH"].ToString().Substring(3, 2);
                }
                strSQL += " ORDER BY C_ONDUTY_HOUR.MZ_ID";
                SqlDataSource1.SelectCommand = strSQL;
                SqlDataSource1.SelectParameters.Clear();
                GridView1.DataBind();
            }
            else
            {
                string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME,MZ_ONDUTY_KIND,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                   "\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR "+
                "LEFT JOIN  C_ONDUTY_CODE ON DUTY_KIND= MZ_ONDUTY_KIND WHERE 1=1";
                strSQL += " and MZ_YEAR=dbo.LPAD(dbo.TO_CHAR(GETDATE(),'YYYY')-1911,3,'0') AND MZ_MONTH =dbo.TO_CHAR(GETDATE(),'MM') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD = '" + Session["ADPMZ_EXAD"] + "') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT = '" + Session["ADPMZ_EXUNIT"] + "') ORDER BY C_ONDUTY_HOUR.MZ_ID";
                SqlDataSource1.SelectCommand = strSQL;
                SqlDataSource1.SelectParameters.Clear();
                GridView1.DataBind();
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_ID = "NULL";

            string old_YEAR = "NULL";

            string old_MONTH = "NULL";

            //違反唯一值得BUG BY青 20140701
            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
                Cookie1 = Request.Cookies["PKEY_MZ_ID"];
                old_ID = Cookie1.Value.ToString();

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_YEAR");
                Cookie2 = Request.Cookies["PKEY_MZ_YEAR"];
                old_YEAR = Cookie2.Value.ToString();

                HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_MONTH");
                Cookie3 = Request.Cookies["PKEY_MZ_MONTH"];
                old_MONTH = Cookie3.Value.ToString();
            }

            string pkey_check;

            if (old_ID == TextBox_MZ_ID.Text && old_YEAR == TextBox_DUTYMONTH.Text.Trim().Substring(0, 3) && old_MONTH == TextBox_DUTYMONTH.Text.Trim().Substring(3, 2) && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_ONDUTY_HOUR WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_YEAR = '" + TextBox_DUTYMONTH.Text.Trim().Substring(0, 3) + "' AND MZ_MONTH = '" + TextBox_DUTYMONTH.Text.Trim().Substring(3, 2) + "'");

            if (pkey_check != "0")
            {
                ErrorString += "身分證號及輪值月份違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
                TextBox_DUTYMONTH.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.White;
                TextBox_DUTYMONTH.BackColor = Color.White;
            }

            //增加備考防呆 BY青 20140701           

            if (textjuge(ref this.Panel_TextBox) == true)
            {
                ErrorString += "欄位請輸入1或2" + "\\r\\n";
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('欄位請輸入1或2');", true);
               // return;
            }
            //增加備考防呆 BY青 20140701          
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_ONDUTYHOUR_KEYIN.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_ONDUTY_HOUR", tbox.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            tbox.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            tbox.BackColor = Color.White;
            //        }
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            count();

            //忘了轉型的BUG BY青 20140701
            SqlParameter[] parameterList = {
            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
            new SqlParameter("MZ_YEAR",SqlDbType.VarChar){Value = TextBox_DUTYMONTH.Text.Trim().Substring(0,3)},
            new SqlParameter("MZ_MONTH",SqlDbType.VarChar){Value = TextBox_DUTYMONTH.Text.Trim().Substring(3,2)},
            //new SqlParameter("MZ_ONDUTY_KIND",SqlDbType.VarChar){Value = DropDownList_MZ_ONDUTY_KIND.SelectedValue},
             new SqlParameter("MZ_ONDUTY_KIND",SqlDbType.VarChar){Value = ddl_Kind.SelectedValue},
            new SqlParameter("1",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox1.Text)?Convert.DBNull:int.Parse(TextBox1.Text)},
            new SqlParameter("2",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox2.Text)?Convert.DBNull:int.Parse(TextBox2.Text)},
            new SqlParameter("3",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox3.Text)?Convert.DBNull:int.Parse(TextBox3.Text)},
            new SqlParameter("4",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox4.Text)?Convert.DBNull:int.Parse(TextBox4.Text)},
            new SqlParameter("5",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox5.Text)?Convert.DBNull:int.Parse(TextBox5.Text)},
            new SqlParameter("6",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox6.Text)?Convert.DBNull:int.Parse(TextBox6.Text)},
            new SqlParameter("7",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox7.Text)?Convert.DBNull:int.Parse(TextBox7.Text)},
            new SqlParameter("8",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox8.Text)?Convert.DBNull:int.Parse(TextBox8.Text)},
            new SqlParameter("9",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox9.Text)?Convert.DBNull:int.Parse(TextBox9.Text)},
            new SqlParameter("10",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox10.Text)?Convert.DBNull:int.Parse(TextBox10.Text)},
            new SqlParameter("11",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox11.Text)?Convert.DBNull:int.Parse(TextBox11.Text)},
            new SqlParameter("12",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox12.Text)?Convert.DBNull:int.Parse(TextBox12.Text)},
            new SqlParameter("13",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox13.Text)?Convert.DBNull:int.Parse(TextBox13.Text)},
            new SqlParameter("14",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox14.Text)?Convert.DBNull:int.Parse(TextBox14.Text)},
            new SqlParameter("15",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox15.Text)?Convert.DBNull:int.Parse(TextBox15.Text)},
            new SqlParameter("16",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox16.Text)?Convert.DBNull:int.Parse(TextBox16.Text)},
            new SqlParameter("17",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox17.Text)?Convert.DBNull:int.Parse(TextBox17.Text)},
            new SqlParameter("18",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox18.Text)?Convert.DBNull:int.Parse(TextBox18.Text)},
            new SqlParameter("19",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox19.Text)?Convert.DBNull:int.Parse(TextBox19.Text)},
            new SqlParameter("20",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox20.Text)?Convert.DBNull:int.Parse(TextBox20.Text)},
            new SqlParameter("21",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox21.Text)?Convert.DBNull:int.Parse(TextBox21.Text)},
            new SqlParameter("22",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox22.Text)?Convert.DBNull:int.Parse(TextBox22.Text)},
            new SqlParameter("23",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox23.Text)?Convert.DBNull:int.Parse(TextBox23.Text)},
            new SqlParameter("24",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox24.Text)?Convert.DBNull:int.Parse(TextBox24.Text)},
            new SqlParameter("25",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox25.Text)?Convert.DBNull:int.Parse(TextBox25.Text)},
            new SqlParameter("26",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox26.Text)?Convert.DBNull:int.Parse(TextBox26.Text)},
            new SqlParameter("27",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox27.Text)?Convert.DBNull:int.Parse(TextBox27.Text)},
            new SqlParameter("28",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox28.Text)?Convert.DBNull:int.Parse(TextBox28.Text)},
            new SqlParameter("29",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox29.Text)?Convert.DBNull:int.Parse(TextBox29.Text)},
            new SqlParameter("30",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox30.Text)?Convert.DBNull:int.Parse(TextBox30.Text)},
            new SqlParameter("31",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox31.Text)?Convert.DBNull:int.Parse(TextBox31.Text)},
            //new SqlParameter("MZ_ONDUTY_PAY",SqlDbType.Float){Value = DropDownList_MZ_ONDUTY_PAY.SelectedValue.Trim()},
            //new SqlParameter("MZ_TOTAL_PAY",SqlDbType.Float){Value =int.Parse(DropDownList_MZ_ONDUTY_PAY.SelectedValue.Trim())*int.Parse(TextBox_TOTAL.Text) },
            //20140707
              new SqlParameter("MZ_ONDUTY_PAY",SqlDbType.Float){Value =  lb_PAY.Text.Trim()},
            new SqlParameter("MZ_TOTAL_PAY",SqlDbType.Float){Value =int.Parse(lb_PAY.Text.Trim())*int.Parse(TextBox_TOTAL.Text) },
            new SqlParameter("MZ_TOTAL_HOUR",SqlDbType.Float){Value = TextBox_TOTAL.Text},
            new SqlParameter("MZ_REMARK",SqlDbType.VarChar){Value = TextBox_MZ_REMARK.Text}
            };

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                if (ViewState["Mode"].ToString() == "INSERT")
                {
                    //20140714
                    //紀錄單筆資料



                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), parameterList));
                    if (ViewState["MZ_ID"] != null)
                    {
                        //20140707
                        string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID FROM C_ONDUTY_HOUR WHERE 1=1";               

                       // string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME,MZ_ONDUTY_KIND,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                       //"\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR WHERE 1=1";
                        if (ViewState["MZ_ID"].ToString() != "")
                        {
                            strSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                        }
                        if (ViewState["MZ_NAME"].ToString() != "")
                        {
                            strSQL += " AND MZ_ID in (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '%" + ViewState["MZ_NAME"].ToString() + "%')";
                        }
                        if (ViewState["AD"].ToString() != "")
                        {
                            strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + ViewState["AD"].ToString() + "')";
                        }
                        if (ViewState["UNIT"].ToString() != "")
                        {
                            strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT='" + ViewState["UNIT"].ToString() + "')";
                        }
                        if (ViewState["MZ_MONTH"].ToString() != "")
                        {
                            strSQL += " AND MZ_YEAR = " + ViewState["MZ_MONTH"].ToString().Substring(0, 3) + " AND MZ_MONTH =" + ViewState["MZ_MONTH"].ToString().Substring(3, 2);
                        }
                        strSQL += " ORDER BY C_ONDUTY_HOUR.MZ_ID";
                        ONDUTY_HOUR_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                        ONDUTY_HOUR_DUTYMONTH = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_DUTYMONTH");

                        //Session["ONDUTY_HOUR_ID"] = ONDUTY_HOUR_ID;
                        //Session["ONDUTY_HOUR_DUTYMONTH"] = ONDUTY_HOUR_DUTYMONTH;

                        ViewState["ONDUTY_HOUR_ID"] = ONDUTY_HOUR_ID;
                        ViewState["ONDUTY_HOUR_DUTYMONTH"] = ONDUTY_HOUR_DUTYMONTH;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + GridView1.Rows.Count + "筆";
                    }
                }
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), parameterList));

                    if (ViewState["MZ_ID"] != null)
                    {
                        //ONDUTY_HOUR_ID = Session["ONDUTY_HOUR_ID"] as List<string>;
                        ONDUTY_HOUR_ID = ViewState["ONDUTY_HOUR_ID"] as List<string>;
                        if (int.Parse(xcount.Text.Trim()) == 0 && ONDUTY_HOUR_ID.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && ONDUTY_HOUR_ID.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == ONDUTY_HOUR_ID.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < ONDUTY_HOUR_ID.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                    }
                }
                btUpdate.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = false;
                gv_show();
                ViewState.Remove("Mode");
                C.controlEnable(ref this.Panel_ONDUTYHOUR_KEYIN, false);
            }
            catch
            {
                if (ViewState["Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('C_ForLeaveOvertime_ONDUTYHOUR_KEYIN.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);

                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                foreach (object tb in Panel_ONDUTYHOUR_KEYIN.Controls)
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
                if (ViewState["MZ_ID"] != null)
                {
                    finddata(int.Parse(xcount.Text.Trim()));

                    //ONDUTY_HOUR_ID = Session["ONDUTY_HOUR_ID"] as List<string>;
                    ONDUTY_HOUR_ID = ViewState["ONDUTY_HOUR_ID"] as List<string>;

                    if (int.Parse(xcount.Text.Trim()) == 0 && ONDUTY_HOUR_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && ONDUTY_HOUR_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == ONDUTY_HOUR_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < ONDUTY_HOUR_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                }
                else
                {
                    //foreach (object tb in Panel_ONDUTYHOUR_KEYIN.Controls)
                    //{
                    //    if (tb is TextBox)
                    //    {
                    //        TextBox tbox = tb as TextBox;
                    //        tbox.Text = "";
                    //    }
                    //}
                }
            }
            gv_show();

            btOK.Enabled = false;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            C.controlEnable(ref this.Panel_ONDUTYHOUR_KEYIN, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE " +
                                  "FROM " +
                                             "C_ONDUTY_HOUR " +
                                  "WHERE " +
                                             "MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) +
                                             "' AND MZ_YEAR='" + o_str.tosql(TextBox_DUTYMONTH.Text.Trim().Substring(0, 3)) +
                                             "' AND MZ_MONTH='" + o_str.tosql(TextBox_DUTYMONTH.Text.Trim().Substring(3, 2)) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                if (ViewState["MZ_ID"] != null)
                {
                    //ONDUTY_HOUR_ID = Session["ONDUTY_HOUR_ID"] as List<String>;
                    //ONDUTY_HOUR_DUTYMONTH = Session["ONDUTY_HOUR_DUTYMONTH"] as List<String>;

                    ONDUTY_HOUR_ID = ViewState["ONDUTY_HOUR_ID"] as List<String>;
                    ONDUTY_HOUR_DUTYMONTH = ViewState["ONDUTY_HOUR_DUTYMONTH"] as List<String>;
                    ONDUTY_HOUR_ID.RemoveAt(int.Parse(xcount.Text.Trim()));
                    ONDUTY_HOUR_DUTYMONTH.RemoveAt(int.Parse(xcount.Text.Trim()));

                    if (ONDUTY_HOUR_ID.Count == 0)
                    {
                        btUpdate.Enabled = false;
                        btDelete.Enabled = false;
                        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_ForLeaveOvertime_DUTYOVERTIME_KEYIN.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    }
                    else
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        if (ONDUTY_HOUR_ID.Count > 1)
                        {
                            btNEXT.Enabled = true;
                        }
                        btUpdate.Enabled = true;
                        btDelete.Enabled = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + ONDUTY_HOUR_ID.Count.ToString() + "筆";
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    }
                    string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME,MZ_ONDUTY_KIND,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                                           "\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR WHERE 1=1";
                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }
                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID in (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '%" + ViewState["MZ_NAME"].ToString() + "%')";
                    }
                    if (ViewState["AD"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + ViewState["AD"].ToString() + "')";
                    }
                    if (ViewState["UNIT"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT='" + ViewState["UNIT"].ToString() + "')";
                    }
                    if (ViewState["MZ_MONTH"].ToString() != "")
                    {
                        strSQL += " AND MZ_YEAR = " + ViewState["MZ_MONTH"].ToString().Substring(0, 3) + " AND MZ_MONTH =" + ViewState["MZ_MONTH"].ToString().Substring(3, 2);
                    }
                    strSQL += " ORDER BY C_ONDUTY_HOUR.MZ_ID";
                    SqlDataSource1.SelectCommand = strSQL;
                    SqlDataSource1.SelectParameters.Clear();
                    GridView1.DataBind();
                }
                else
                {
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_ForLeaveOvertime_DUTYOVERTIME_KEYIN.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    string strSQL = "SELECT (MZ_YEAR+MZ_MONTH) AS MZ_DUTYMONTH,MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME,MZ_ONDUTY_KIND,\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\"," +
                       "\"12\",\"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\",\"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR WHERE 1=1";
                    strSQL += " and MZ_YEAR=dbo.LPAD(dbo.TO_CHAR(GETDATE(),'YYYY')-1911,3,'0') AND MZ_MONTH =dbo.TO_CHAR(GETDATE(),'MM') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD = '" + Session["ADPMZ_EXAD"] + "') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT = '" + Session["ADPMZ_EXUNIT"] + "') ORDER BY C_ONDUTY_HOUR.MZ_ID";
                    SqlDataSource1.SelectCommand = strSQL;
                    SqlDataSource1.SelectParameters.Clear();
                    GridView1.DataBind();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
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
                if (int.Parse(xcount.Text) != ONDUTY_HOUR_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + ONDUTY_HOUR_ID.Count.ToString() + "筆";

            gv_show();

            GridView1.SelectedIndex = int.Parse(xcount.Text);

        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == ONDUTY_HOUR_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) == ONDUTY_HOUR_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + ONDUTY_HOUR_ID.Count.ToString() + "筆";
            gv_show();

            GridView1.SelectedIndex = int.Parse(xcount.Text);
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.Trim().ToUpper();
            string strSQL = "SELECT " +
                                      "MZ_NAME," +
                                      "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') AS MZ_EXUNIT " +
                            "FROM " +
                                      "A_DLBASE " +
                            "WHERE " +
                                      "MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無此人');", true);
            }
            else
            {
                DropDownList_MZ_NAME.SelectedItem.Text = dt.Rows[0]["MZ_NAME"].ToString();
                TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_EXUNIT"].ToString();

            }
            gv_show();
        }

        protected void DropDownList_MZ_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue;
            TextBox_MZ_UNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') AS MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'");
            gv_show();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECT")
            {
                GridView1.SelectedIndex = int.Parse(e.CommandArgument.ToString());
                string strSQL = "SELECT * FROM C_ONDUTY_HOUR WHERE MZ_ID='" + GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())].Values[0].ToString() +
                                                            "' AND MZ_YEAR='" + GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())].Values[1].ToString().Substring(0, 3) +
                                                            "' AND MZ_MONTH='" + GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())].Values[1].ToString().Substring(3, 2) + "'";
                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                if (temp.Rows.Count == 1)
                {
                    TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();
                    DropDownList_MZ_NAME.DataBind();
                    DropDownList_MZ_NAME.SelectedItem.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[0]["MZ_ID"].ToString() + "'");


                    //TextBox_MZ_YEAR.Text = temp.Rows[0]["MZ_YEAR"].ToString();
                    //TextBox_MZ_MONTH.Text = temp.Rows[0]["MZ_MONTH"].ToString();
                    TextBox_DUTYMONTH.Text = temp.Rows[0]["MZ_YEAR"].ToString() + temp.Rows[0]["MZ_MONTH"].ToString();
                  
                    //20140707
                    ddl_Kind.SelectedValue = temp.Rows[0]["MZ_ONDUTY_KIND"].ToString();
                   // DropDownList_MZ_ONDUTY_KIND.SelectedValue = temp.Rows[0]["MZ_ONDUTY_KIND"].ToString();
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
                    //20140707
                    lb_PAY.Text = temp.Rows[0]["MZ_ONDUTY_PAY"].ToString();
                    //DropDownList_MZ_ONDUTY_PAY.SelectedValue = temp.Rows[0]["MZ_ONDUTY_PAY"].ToString();
                    TextBox_MZ_REMARK.Text = temp.Rows[0]["MZ_REMARK"].ToString();
                    TextBox_TOTAL.Text = temp.Rows[0]["MZ_TOTAL_HOUR"].ToString();
                    TextBox_MZ_UNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') AS MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'");
                }
                if (ViewState["MZ_ID"] != null)
                {
                    xcount.Text = e.CommandArgument.ToString();
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + GridView1.Rows.Count + "筆";
                    gv_show();
                    //ONDUTY_HOUR_ID = Session["ONDUTY_HOUR_ID"] as List<string>;
                    ONDUTY_HOUR_ID = ViewState["ONDUTY_HOUR_ID"] as List<string>;

                    if (int.Parse(xcount.Text.Trim()) == 0 && ONDUTY_HOUR_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && ONDUTY_HOUR_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == ONDUTY_HOUR_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < ONDUTY_HOUR_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                }
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[GridView1.Columns.Count - 1].Attributes.Add("Style", "display:none");
                e.Row.Cells[GridView1.Columns.Count - 2].Attributes.Add("Style", "display:none");
            }
        }

        //20140701 BY青 不等於1和2防呆
        public bool  textjuge(ref Panel Pl)
        {

            bool chk_Result = false;

            foreach (Object ob in Panel_TextBox.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;
                    if (tbox.Text != "1" && tbox.Text != "2" && tbox.Text != "")
                    {
                        tbox.BackColor = Color.Orange;
                        chk_Result = true;
                    }
                    else
                    {
                        tbox.BackColor = Color.White;
                    }
                }


                


            }
            return chk_Result;

       
        }

        protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strsql = string.Format("SELECT DUTY_KIND, DUTY_KINDNAME,DUTY_PAY FROM C_ONDUTY_CODE WHERE DUTY_TYPE='2' AND  substr(DUTY_KIND,0,1)='{0}' ", ddl_type.SelectedValue);
            DataTable dt_kind=o_DBFactory.ABC_toTest.Create_Table(strsql, "get");
            ddl_Kind.DataSource = dt_kind;
            ddl_Kind.DataTextField = "DUTY_KINDNAME";
            ddl_Kind.DataValueField = "DUTY_KIND";


            ddl_Kind.DataBind();

            if(dt_kind.Rows.Count>0)            
                lb_PAY.Text = dt_kind.Rows[0]["DUTY_PAY"].ToString();
        }

        protected void ddl_Kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strsql = string.Format("SELECT DUTY_PAY FROM C_ONDUTY_CODE WHERE DUTY_TYPE='2' AND  DUTY_KIND='{0}' ",ddl_Kind.SelectedValue);
            lb_PAY.Text = o_DBFactory.ABC_toTest.vExecSQL(strsql);
        }
    }
}
