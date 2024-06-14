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
    public partial class C_bussinesstrip : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
              
            }

            Label1.Text = "差勤費用管銷管理作業";

            //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            //查詢ID
            HttpCookie ForLeaveBasic_ID_Cookie = new HttpCookie("ForLeaveBasicSearch_ID");
            ForLeaveBasic_ID_Cookie = Request.Cookies["ForLeaveBasicSearch_ID"];

            if (ForLeaveBasic_ID_Cookie == null)
            {
                ViewState["ID"] = null;
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["ID"] = TPMPermissions._strDecod(ForLeaveBasic_ID_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            //查詢姓名
            HttpCookie ForLeaveBasic_NAME_Cookie = new HttpCookie("ForLeaveBasicSearch_NAME");
            ForLeaveBasic_NAME_Cookie = Request.Cookies["ForLeaveBasicSearch_NAME"];

            if (ForLeaveBasic_NAME_Cookie == null)
            {
                ViewState["NAME"] = null;
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["NAME"] = TPMPermissions._strDecod(ForLeaveBasic_NAME_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }

            ViewState["MZ_EXAD"] = Request["MZ_EXAD"];
            ViewState["MZ_EXUNIT"] = Request["MZ_EXUNIT"];
            ViewState["MZ_IDATE1"] = Request["MZ_IDATE1"];

            if (!Page.IsPostBack)
            {
                if (ViewState["ID"] != null)
                {
                    string strSQL = "SELECT C_DLTB01.*,A_DLBASE.MZ_ID FROM C_DLTB01,A_DLBASE WHERE C_DLTB01.MZ_ID=A_DLBASE.MZ_ID AND (MZ_CODE='07' OR MZ_CODE='06') ";

                    if (ViewState["ID"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_ID='" + ViewState["ID"].ToString().Trim().ToUpper() + "'";
                    }

                    if (ViewState["NAME"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_NAME='" + ViewState["NAME"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_EXAD"].ToString() != "" && ViewState["MZ_EXUNIT"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_EXAD ='" + ViewState["MZ_EXAD"].ToString() + "' AND C_DLTB01.MZ_EXUNIT='" + ViewState["MZ_EXUNIT"].ToString() + "'";
                    }
                    else if (ViewState["MZ_EXAD"].ToString() != "")
                    {
                        strSQL += " AND (C_DLTB01.MZ_EXAD='" + ViewState["MZ_EXAD"].ToString() + "' OR A_DLBASE.MZ_AD='" + ViewState["MZ_EXAD"].ToString() + "' OR A_DLBASE.PAY_AD='" + ViewState["MZ_EXAD"].ToString() + "')";
                    }

                    if (ViewState["MZ_IDATE1"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_IDATE1='" + ViewState["MZ_IDATE1"].ToString().Trim() + "'";
                    }

                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                    if (dt.Rows.Count == 1)
                    {
                        lbl_ID.Text = dt.Rows[0]["MZ_ID"].ToString();
                        lbl_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString();
                        lbl_DATE.Text = dt.Rows[0]["MZ_IDATE1"].ToString() + " " + dt.Rows[0]["MZ_ITIME1"].ToString() + "至" + dt.Rows[0]["MZ_ODATE"].ToString() + " " + dt.Rows[0]["MZ_OTIME"].ToString() + "共" + dt.Rows[0]["MZ_TDAY"].ToString() + "日" + dt.Rows[0]["MZ_TTIME"].ToString() + "時";
                        lbl_MZ_CAUSE.Text = dt.Rows[0]["MZ_CAUSE"].ToString();
                        lbl_MZ_TADD.Text = dt.Rows[0]["MZ_TADD"].ToString();
                        bt_Insert.Enabled = true;
                        doSearch(dt.Rows[0]["MZ_DLTB01_SN"].ToString());
                        ViewState["MZ_DLTB01_SN"] = dt.Rows[0]["MZ_DLTB01_SN"].ToString();

                        //0826→Dean 送核按鈕控制與MZ_ID記錄
                        if (gv_BUSSINESSTRIP.Rows.Count > 0)
                        {
                            btn_onlinecheck.Enabled = true;
                        }
                        ViewState["MZ_ID"] = dt.Rows[0]["MZ_ID"].ToString();
                    }
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {

            string strSQL = "";

            string Message = "";

            if (lbl_MODE.Text == "INSERT")
            {
                Message = "新增";
                strSQL = @"INSERT INTO C_BUSSINESSTRIP(SN,BOAT,WORKNOTES,CARMRT,DAY,LOCATION,MONTH,TRAIN,AIRPLANE,ACCOMMODATION,TOTAL,ADUSER,ADATE,MZ_DLTB01_SN,CHARGES)
                                       VALUES ( NEXT VALUE FOR dbo.C_BUSSINESSTRIP_SN,@BOAT,@WORKNOTES,@CARMRT,@DAY,@LOCATION,@MONTH,@TRAIN,@AIRPLANE,@ACCOMMODATION,@TOTAL,@ADUSER,@ADATE,@MZ_DLTB01_SN,@CHARGES)";
            }
            else
            {
                Message = "修改";
                strSQL = string.Format(@"UPDATE C_BUSSINESSTRIP SET BOAT=@BOAT,WORKNOTES=@WORKNOTES,CARMRT=@CARMRT,DAY=@DAY,LOCATION=@LOCATION,
                                                            MONTH=@MONTH,TRAIN=@TRAIN,AIRPLANE=@AIRPLANE,ACCOMMODATION=@ACCOMMODATION,
                                                            TOTAL=@TOTAL,ADUSER=@ADUSER,ADATE=@ADATE,CHARGES=@CHARGES,MZ_DLTB01_SN=@MZ_DLTB01_SN WHERE SN={0}", lbl_MODE.Text);
            }

            Sum_total();

            SqlParameter[] ParamaterList ={
                new SqlParameter("BOAT",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_BOAT.Text)?0:int.Parse(txt_BOAT.Text)},
                new SqlParameter("WORKNOTES",SqlDbType.NVarChar){Value =txt_WORKNOTES.Text},
                new SqlParameter("CARMRT",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_CARMRT.Text)?0:int.Parse(txt_CARMRT.Text)},
                new SqlParameter("DAY",SqlDbType.NVarChar){Value =txt_DAY.Text},
                new SqlParameter("LOCATION",SqlDbType.NVarChar){Value =txt_LOCATION.Text},
                new SqlParameter("MONTH",SqlDbType.NVarChar){Value =txt_MONTH.Text},
                new SqlParameter("TRAIN",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_TRAIN.Text)?0:int.Parse(txt_TRAIN.Text)},
                new SqlParameter("AIRPLANE",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_AIRPLANE.Text)?0:int.Parse(txt_AIRPLANE.Text)},
                new SqlParameter("ACCOMMODATION",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_ACCOMMODATION.Text)?0:int.Parse(txt_ACCOMMODATION.Text)},
                new SqlParameter("TOTAL",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_TOTAL.Text)?0:int.Parse(txt_TOTAL.Text)},
                new SqlParameter("ADUSER",SqlDbType.NVarChar){Value =""},
                new SqlParameter("ADATE",SqlDbType.DateTime){Value =DateTime.Now},
                new SqlParameter("MZ_DLTB01_SN",SqlDbType.Float){Value =int.Parse(ViewState["MZ_DLTB01_SN"].ToString())},
                new SqlParameter("CHARGES",SqlDbType.Float){Value =string.IsNullOrEmpty(txt_Charges.Text)?0:int.Parse(txt_Charges.Text)},
            };

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            conn.Open();
            SqlCommand oracmd = new SqlCommand(strSQL, conn);

            if (ParamaterList != null && ParamaterList.Count() > 0)
            {
                foreach (SqlParameter para in ParamaterList)
                {
                    oracmd.Parameters.Add(para);
                }
            }

            try
            {
                oracmd.ExecuteNonQuery();
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "click", "alert('" + Message + "成功！');", true);
                btn_fake_ModalPopupExtender.Hide();
                doSearch(ViewState["MZ_DLTB01_SN"].ToString());
                //0826→Dean 送核按鈕控制
                if (gv_BUSSINESSTRIP.Rows.Count > 0)
                {
                    btn_onlinecheck.Enabled = true;
                }
            }
            catch
            {

                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "click", "alert('" + Message + "失敗！');", true);
                btn_fake_ModalPopupExtender.Show();
            }

        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            doSearch(ViewState["MZ_DLTB01_SN"].ToString());
            btn_fake_ModalPopupExtender.Hide();
        }

        protected void bt_Search_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('ForLeaveBasicSearch.aspx?TableName=BASIC&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            lbl_MODE.Text = "INSERT";
            lbl_Title.Text = "新增差旅費資料";
            btn_fake_ModalPopupExtender.Show();
        }

        void CHECK_DLTB01()
        {
            if (o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT COUNT(*) FROM C_BUSSINESSTRIP WHERE MZ_DLTB01_SN={0} AND BUSSINESSTRIP_STATUS='{1}'", ViewState["MZ_DLTB01_SN"].ToString(), "Y")) != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "click", "alert('該日已有申請紀錄！');", true);
                return;
            }
        }

        void doSearch(string C_DLTB01_SN)
        {
            string strSQL = string.Format(@"SELECT C_BUSSINESSTRIP.*,(BOAT+CARMRT+TRAIN+AIRPLANE) AS B1 FROM C_BUSSINESSTRIP WHERE MZ_DLTB01_SN={0} ORDER BY SN ", C_DLTB01_SN);

            gv_BUSSINESSTRIP.DataSource = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            gv_BUSSINESSTRIP.DataBind();
        }

        protected void gv_BUSSINESSTRIP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strSQL = "";

            switch (e.CommandName)
            {
                case "EDT":
                    strSQL = string.Format(@"SELECT * FROM C_BUSSINESSTRIP WHERE SN={0}", gv_BUSSINESSTRIP.DataKeys[Convert.ToInt32(e.CommandArgument)]["SN"].ToString());
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    lbl_Title.Text = "修改差旅費資料";
                    lbl_MODE.Text = gv_BUSSINESSTRIP.DataKeys[Convert.ToInt32(e.CommandArgument)]["SN"].ToString();

                    if (dt.Rows.Count > 0)
                    {
                        txt_BOAT.Text = dt.Rows[0]["BOAT"].ToString();
                        txt_WORKNOTES.Text = dt.Rows[0]["WORKNOTES"].ToString();
                        txt_CARMRT.Text = dt.Rows[0]["CARMRT"].ToString();
                        txt_DAY.Text = dt.Rows[0]["DAY"].ToString();
                        txt_LOCATION.Text = dt.Rows[0]["LOCATION"].ToString();
                        txt_MONTH.Text = dt.Rows[0]["MONTH"].ToString();
                        txt_TRAIN.Text = dt.Rows[0]["TRAIN"].ToString();
                        txt_AIRPLANE.Text = dt.Rows[0]["AIRPLANE"].ToString();
                        txt_ACCOMMODATION.Text = dt.Rows[0]["ACCOMMODATION"].ToString();
                        txt_TOTAL.Text = dt.Rows[0]["TOTAL"].ToString();
                        txt_Charges.Text = dt.Rows[0]["Charges"].ToString();
                        btn_fake_ModalPopupExtender.Show();
                    }
                    break;
                case "DEL":
                    strSQL = string.Format(@"DELETE FROM C_BUSSINESSTRIP WHERE SN={0}", gv_BUSSINESSTRIP.DataKeys[Convert.ToInt32(e.CommandArgument)]["SN"].ToString());
                    try
                    {
                        o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                        doSearch(ViewState["MZ_DLTB01_SN"].ToString());
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "click", "alert('刪除成功！');", true);

                        //0826→Dean 送核按鈕控制
                        if (gv_BUSSINESSTRIP.Rows.Count == 0)
                        {
                            btn_onlinecheck.Enabled = false;
                        }
                    }
                    catch
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "click", "alert('刪除失敗！');", true);
                    }
                    break;
            }
        }

        void Sum_total()
        {
            txt_TOTAL.Text = ((string.IsNullOrEmpty(txt_CARMRT.Text) ? 0 : int.Parse(txt_CARMRT.Text)) +
                                (string.IsNullOrEmpty(txt_BOAT.Text) ? 0 : int.Parse(txt_BOAT.Text)) +
                                (string.IsNullOrEmpty(txt_TRAIN.Text) ? 0 : int.Parse(txt_TRAIN.Text)) +
                                (string.IsNullOrEmpty(txt_AIRPLANE.Text) ? 0 : int.Parse(txt_AIRPLANE.Text)) +
                                (string.IsNullOrEmpty(txt_ACCOMMODATION.Text) ? 0 : int.Parse(txt_ACCOMMODATION.Text)) +
                                (string.IsNullOrEmpty(txt_Charges.Text) ? 0 : int.Parse(txt_Charges.Text))).ToString();

        }

        protected void txt_Count_Total_TextChanged(object sender, EventArgs e)
        {
            Sum_total();

            btn_fake_ModalPopupExtender.Show();
        }

        //0826→Dean　送核相關
        #region 呈核選擇

        protected void btn_onlinecheck_Click(object sender, EventArgs e)
        {
            GV_CHECK_show();
            Panel_select_ModalPopupExtender.Show();
        }


        protected void btn_exit_Click(object sender, EventArgs e)
        {
            ViewState.Remove("s");
            Panel_select_ModalPopupExtender.Hide();
        }

        protected void GV_CHECK_show()
        {
            DataTable temp = new DataTable();
            try
            {
                temp = o_DBFactory.ABC_toTest.Create_Table("SELECT c_review_management.sn,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC) AS MZ_OCCC,a_dlbase.mz_name FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + o_A_DLBASE.PAD(ViewState["MZ_ID"].ToString()) + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + o_A_DLBASE.PUNIT(ViewState["MZ_ID"].ToString()) + "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'", "get");
            }
            catch
            {
            }
            GV_CHECKER.DataSource = temp;
            GV_CHECKER.DataBind();
            ViewState["s"] = "y";
        }

        protected void GV_CHECKER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "checker")
            {
                string RID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RID FROM C_DLTB01 WHERE MZ_DLTB01_SN = '" + ViewState["MZ_DLTB01_SN"].ToString() + "'");
                string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + e.CommandArgument.ToString() + "'");

                string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                string today = year + DateTime.Now.ToString("MMdd");
                string now = DateTime.Now.ToString("HH:mm:ss");

                string strSQL = "INSERT INTO " + "C_BUSSINESSTRIP_HISTORY" +
                                                               "(SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS)" +
                                                         " VALUES" +
                                                               "( NEXT VALUE FOR dbo.C_BUSSINESSTRIP_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS)";

                string LEAVE_SN = "";

                switch (Session["ADPMZ_EXAD"].ToString())
                {
                    case "382130000C":
                        LEAVE_SN = "02";
                        break;
                    case "382130100C":
                    case "382130200C":
                    case "382130300C":
                        LEAVE_SN = "05";
                        break;
                    case "382130500C":
                    case "382135000C":
                        LEAVE_SN = "13";
                        break;
                    default:
                        LEAVE_SN = "09";
                        break;
                }

                SqlParameter[] parameterList = {
                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = LEAVE_SN},
                    new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value =temp},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.Float){Value = 2},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = ViewState["MZ_DLTB01_SN"].ToString()},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                   
                                                  };

                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                ViewState.Remove("s");
                Panel_select_ModalPopupExtender.Hide();
            }
        }

        protected void GV_CHECKER_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_CHECKER.PageIndex = e.NewPageIndex;
            GV_CHECK_show();
        }

        #endregion

        protected void gv_BUSSINESSTRIP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (drv["BUSSINESSTRIP_STATUS"].ToString() == "Y")
                {
                    e.Row.Cells[7].Enabled = false;
                    e.Row.Cells[8].Enabled = false;
                }
            }
        }
    }
}
