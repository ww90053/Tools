using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_CHANGE_FORLEAVEBASIC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Label_MZ_AD.Text = o_A_KTYPE.CODE_TO_NAME(Session["ADPMZ_AD"].ToString(), "04");
                Label_MZ_EXAD.Text = o_A_KTYPE.CODE_TO_NAME(Session["ADPMZ_EXAD"].ToString(), "04");
                Label_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                Label_MZ_NAME.Text = o_A_DLBASE.CNAME(Session["ADPMZ_ID"].ToString());
                
                txt_Close_OR_Open(false);
            }
        }

        protected void bt_Search_Click(object sender, EventArgs e)
        {

        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["MODE"] = "I";
            ViewState["SQL"] = @"INSERT INTO C_CHANGE_DLTB01(CHANGE_DLTB01_SN,ORGINAL_MZ_IDATE1,ORGINAL_MZ_ITIME1,ORGINAL_MZ_ODATE,ORGINAL_MZ_OTIME,NEW_MZ_IDATE1,NEW_MZ_ITIME1,NEW_MZ_ODATE,NEW_MZ_OTIME,ORGINAL_MZ_TDAY,ORGINAL_MZ_TTIME,NEW_MZ_TDAY,NEW_MZ_TTIME,MZ_DLTB01_SN) 
                                               VALUES( NEXT VALUE FOR dbo.C_CHANGE_DLTB01_SN,@ORGINAL_MZ_IDATE1,@ORGINAL_MZ_ITIME1,@ORGINAL_MZ_ODATE,@ORGINAL_MZ_OTIME,@NEW_MZ_IDATE1,@NEW_MZ_ITIME1,@NEW_MZ_ODATE,@NEW_MZ_OTIME,@ORGINAL_MZ_TDAY,@ORGINAL_MZ_TTIME,@NEW_MZ_TDAY,@NEW_MZ_TTIME,@MZ_DLTB01_SN)";

            txt_Close_OR_Open(true);

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btCancel.Enabled = true;
            btDelete.Enabled = false;
            btn_onlinecheck.Enabled = false;
        }

        protected void btn_onlinecheck_Click(object sender, EventArgs e)
        {
            GV_CHECK_show();
            Panel_select_ModalPopupExtender.Show();
        }

        private void GV_CHECK_show()
        {
            DataTable temp = new DataTable();
            try
            {
                temp = o_DBFactory.ABC_toTest.Create_Table("SELECT c_review_management.sn,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC) AS MZ_OCCC,a_dlbase.mz_name FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString()) + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString()) + "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'", "get");
            }
            catch
            {
            }
            GV_CHECKER.DataSource = temp;
            GV_CHECKER.DataBind();
            ViewState["s"] = "y";
        }

        protected void btn_Search_DLTB01_Click(object sender, EventArgs e)
        {
            Panel_choice_ModalPopupExtender.Show();
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string strSQL = string.Format("SELECT MZ_DLTB01_SN,MZ_IDATE1||MZ_ITIME1 AS MZ_IDATE1 FROM C_DLTB01 WHERE MZ_ID='{0}' AND  MZ_IDATE1='{1}'", Session["ADPMZ_ID"].ToString(), TextBox_Search_MZ_IDATE1.Text);

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");
            //如果恰好一筆
            if (dt.Rows.Count == 1)
            {
                btInsert.Enabled = true;
                btDelete.Enabled = false;
                
                btCancel.Enabled = false;
                GetValue(dt.Rows[0]["MZ_DLTB01_SN"].ToString());
                ViewState["MZ_DLTB01_SN"] = dt.Rows[0]["MZ_DLTB01_SN"];
            }
            else if (dt.Rows.Count > 1)
            {
                //btDelete.Enabled = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                Panel_choice_ModalPopupExtender.Show();

                //ViewState["MZ_DLTB01_SN"] = dt.Rows[0]["MZ_DLTB01_SN"];
                btCancel.Enabled = false;

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無此請假資料！');", true);
            }
        }
        /// <summary>
        /// 查詢原請假資料
        /// </summary>
        /// <param name="MZ_DLTB01_SN">流水號</param>
        private void GetValue(string MZ_DLTB01_SN)
        {
            string strSQL1 = string.Format(@"SELECT * FROM C_CHANGE_DLTB01 WHERE MZ_DLTB01_SN='{0}'", MZ_DLTB01_SN);

            DataTable dt1 = new DataTable();

            dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL1, "GET1");

            if (dt1.Rows.Count == 0)//有沒有銷假紀錄
            {   
                //還沒有核銷紀錄
                string strSQL = string.Format(@"SELECT * FROM C_DLTB01 WHERE MZ_DLTB01_SN='{0}'", MZ_DLTB01_SN);

                DataTable dt = new DataTable();

                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                if (dt.Rows.Count > 0)
                {
                    Label_ORGINAL_MZ_TTIME.Text = dt.Rows[0]["MZ_TTIME"].ToString();
                    Label_ORGINAL_MZ_IDATE1.Text = dt.Rows[0]["MZ_IDATE1"].ToString();
                    Label_ORGINAL_MZ_ITIME1.Text = dt.Rows[0]["MZ_ITIME1"].ToString();
                    Label_ORGINAL_MZ_ODATE.Text = dt.Rows[0]["MZ_ODATE"].ToString();
                    Label_ORGINAL_MZ_OTIME.Text = dt.Rows[0]["MZ_OTIME"].ToString();
                    Label_ORGINAL_MZ_TDAY.Text = dt.Rows[0]["MZ_TDAY"].ToString();
                    hdf_MZ_DLTB01_SN.Value = dt.Rows[0]["MZ_DLTB01_SN"].ToString();
                    //2013/07/11
                    //允許新增銷假資料
                    btInsert.Enabled = true;
                    //
                    //如查詢之日期，MZ_CODE為11、16、22，則提醒視窗：「補休銷假請逕洽機關差勤承辦人辦理。」阻擋(新增功能鍵反灰鎖定)
                    switch (dt.Rows[0]["MZ_CODE"].ToString())
                    {
                        case "11":
                        case "16":
                        case "22":
                            btInsert.Enabled = false;
                            string msg = @"補休銷假請逕洽機關差勤承辦人辦理。";
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('"+ msg + "');", true);
                            break;
                    }
                }
            }
            else
            {
                //已經有核銷紀錄
                Label_ORGINAL_MZ_TTIME.Text = dt1.Rows[0]["ORGINAL_MZ_TTIME"].ToString();
                Label_ORGINAL_MZ_IDATE1.Text = dt1.Rows[0]["ORGINAL_MZ_IDATE1"].ToString();
                Label_ORGINAL_MZ_ITIME1.Text = dt1.Rows[0]["ORGINAL_MZ_ITIME1"].ToString();
                Label_ORGINAL_MZ_ODATE.Text = dt1.Rows[0]["ORGINAL_MZ_ODATE"].ToString();
                Label_ORGINAL_MZ_OTIME.Text = dt1.Rows[0]["ORGINAL_MZ_OTIME"].ToString();
                Label_ORGINAL_MZ_TDAY.Text = dt1.Rows[0]["ORGINAL_MZ_TDAY"].ToString();
                TextBox_MZ_TDAY.Text = dt1.Rows[0]["NEW_MZ_TDAY"].ToString();
                TextBox_MZ_TTIME.Text = dt1.Rows[0]["NEW_MZ_TTIME"].ToString();
                TextBox_MZ_OTIME.Text = dt1.Rows[0]["NEW_MZ_OTIME"].ToString();
                TextBox_MZ_ODATE.Text = dt1.Rows[0]["NEW_MZ_ODATE"].ToString();
                TextBox_MZ_ITIME1.Text = dt1.Rows[0]["NEW_MZ_ITIME1"].ToString();
                TextBox_MZ_IDATE1.Text = dt1.Rows[0]["NEW_MZ_IDATE1"].ToString();
                hdf_MZ_DLTB01_SN.Value = dt1.Rows[0]["MZ_DLTB01_SN"].ToString();
                hdf_C_CHANGE_DLTB01_SN.Value = dt1.Rows[0]["CHANGE_DLTB01_SN"].ToString();
                //新增按鈕鎖住 不允許新增銷假資料
                btInsert.Enabled = false;

                if (dt1.Rows[0]["CHANGE_DLTB01_STATUS"].ToString() != "Y")
                {
                    btUpdate.Enabled = true;
                    btn_onlinecheck.Enabled = true;
                }
                else
                {
                    btn_onlinecheck.Enabled = false;
                    
                }

            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["MODE"] = "U";
//            ViewState["SQL"] = string.Format(@"UPDATE C_CHANGE_DLTB01 SET ORGINAL_MZ_IDATE1=@ORGINAL_MZ_IDATE1,ORGINAL_MZ_ITIME1=@ORGINAL_MZ_ITIME1,ORGINAL_MZ_ODATE=@ORGINAL_MZ_ODATE,ORGINAL_MZ_OTIME=@ORGINAL_MZ_OTIME,
//                                                     NEW_MZ_IDATE1=@NEW_MZ_IDATE1,NEW_MZ_ITIME1=@NEW_MZ_ITIME1,NEW_MZ_ODATE=@NEW_MZ_ODATE,NEW_MZ_OTIME=@NEW_MZ_OTIME,ORGINAL_MZ_TDAY=@ORGINAL_MZ_TDAY,
//                                                     ORGINAL_MZ_TTIME=@ORGINAL_MZ_TTIME,NEW_MZ_TDAY=@NEW_MZ_TDAY,NEW_MZ_TTIME=@NEW_MZ_TTIME,MZ_DLTB01_SN=@MZ_DLTB01_SN WHERE CHANGE_DLTB01_SN='{0}'", hdf_C_CHANGE_DLTB01_SN.Value);
           ViewState["SQL"] = string.Format(@"UPDATE C_CHANGE_DLTB01 SET ORGINAL_MZ_IDATE1=@ORGINAL_MZ_IDATE1,ORGINAL_MZ_ITIME1=@ORGINAL_MZ_ITIME1,ORGINAL_MZ_ODATE=@ORGINAL_MZ_ODATE,ORGINAL_MZ_OTIME=@ORGINAL_MZ_OTIME,
                                                     NEW_MZ_IDATE1=@NEW_MZ_IDATE1,NEW_MZ_ITIME1=@NEW_MZ_ITIME1,NEW_MZ_ODATE=@NEW_MZ_ODATE,NEW_MZ_OTIME=@NEW_MZ_OTIME,ORGINAL_MZ_TDAY=@ORGINAL_MZ_TDAY,
                                                     ORGINAL_MZ_TTIME=@ORGINAL_MZ_TTIME,NEW_MZ_TDAY=@NEW_MZ_TDAY,NEW_MZ_TTIME=@NEW_MZ_TTIME,MZ_DLTB01_SN=@MZ_DLTB01_SN WHERE MZ_DLTB01_SN='{0}'", ViewState["MZ_DLTB01_SN"]);

            txt_Close_OR_Open(true);

            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btOK.Enabled = true;
            btn_onlinecheck.Enabled = false;
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            foreach (object obj in Panel1.Controls)
            {
                if (obj is TextBox)
                {
                    TextBox tb = (TextBox)obj;

                    if (string.IsNullOrEmpty(tb.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入完整資料！');", true);
                        return;
                    }

                }
            }

            SqlParameter[] parameterList ={
             new SqlParameter("ORGINAL_MZ_IDATE1",SqlDbType.NVarChar){Value=Label_ORGINAL_MZ_IDATE1.Text},
             new SqlParameter("ORGINAL_MZ_ITIME1",SqlDbType.NVarChar){Value=Label_ORGINAL_MZ_ITIME1.Text},
             new SqlParameter("ORGINAL_MZ_ODATE",SqlDbType.NVarChar){Value=Label_ORGINAL_MZ_ODATE.Text},
             new SqlParameter("ORGINAL_MZ_OTIME",SqlDbType.NVarChar){Value=Label_ORGINAL_MZ_OTIME.Text},
             new SqlParameter("NEW_MZ_IDATE1",SqlDbType.NVarChar){Value=string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim())?Convert.DBNull:TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0')},
             new SqlParameter("NEW_MZ_ITIME1",SqlDbType.NVarChar){Value=string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2)},
             new SqlParameter("NEW_MZ_ODATE",SqlDbType.NVarChar){Value=string.IsNullOrEmpty(TextBox_MZ_ODATE.Text.Trim())?Convert.DBNull:TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
             new SqlParameter("NEW_MZ_OTIME",SqlDbType.NVarChar){Value=string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2)},
             new SqlParameter("ORGINAL_MZ_TDAY",SqlDbType.Float){Value=Label_ORGINAL_MZ_TDAY.Text},
             new SqlParameter("ORGINAL_MZ_TTIME",SqlDbType.Float){Value=Label_ORGINAL_MZ_TTIME.Text},
             new SqlParameter("NEW_MZ_TDAY",SqlDbType.Float){Value=int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text)?"0":TextBox_MZ_TDAY.Text)},
             new SqlParameter("NEW_MZ_TTIME",SqlDbType.Float){Value=int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text)?"0":TextBox_MZ_TTIME.Text)},
             new SqlParameter("MZ_DLTB01_SN",SqlDbType.Float){Value=hdf_MZ_DLTB01_SN.Value},
                                    };
            // 
            ViewState["MZ_DLTB01_SN"] = hdf_MZ_DLTB01_SN.Value;
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["SQL"].ToString(), parameterList);

                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["SQL"].ToString(), parameterList));

                if (ViewState["MODE"].ToString() == "I")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                }
                else if (ViewState["MODE"].ToString() == "U")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                }

                btUpdate.Enabled = true;
               // btInsert.Enabled = true;
                btDelete.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btn_onlinecheck.Enabled = true;
                ViewState.Remove("MODE");
            }
            catch
            {
                if (ViewState["MODE"].ToString() == "I")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                
                
                }
                else if (ViewState["MODE"].ToString() == "U")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            txt_Close_OR_Open(false);
            /*
            Label_ORGINAL_MZ_TTIME.Text = "";
            Label_ORGINAL_MZ_IDATE1.Text = "";
            Label_ORGINAL_MZ_ITIME1.Text = "";
            Label_ORGINAL_MZ_ODATE.Text = "";
            Label_ORGINAL_MZ_OTIME.Text = "";
            Label_ORGINAL_MZ_TDAY.Text = "";
            
            TextBox_MZ_TDAY.Text = "";
            TextBox_MZ_TTIME.Text = "";
            TextBox_MZ_OTIME.Text = "";
            TextBox_MZ_ODATE.Text = "";
            TextBox_MZ_ITIME1.Text = "";
            TextBox_MZ_IDATE1.Text = "";
*/
            btOK.Enabled = false;

            if (ViewState["MODE"].ToString() == "I")
            {
                btInsert.Enabled = true;
                btUpdate.Enabled = false;
                btn_onlinecheck.Enabled = false;
                btDelete.Enabled = false;
            }
            if (ViewState["MODE"].ToString() == "U")
            {
                btInsert.Enabled = false;
                btUpdate.Enabled = true;
                btn_onlinecheck.Enabled = true;
                btDelete.Enabled = true;
            }
            btCancel.Enabled = false;
          
            ViewState.Remove("MODE");

        }

        protected void GV_CHECKER_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_CHECKER.PageIndex = e.NewPageIndex;
            GV_CHECK_show();
        }

        protected void GV_CHECKER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "checker")
            {
                //先刪除不是退回的資料
                string DstrSQL = "DELETE FROM C_CHANGE_DLTB01_HISTORY WHERE DLTB01_SN='" + hdf_MZ_DLTB01_SN.Value + "' AND RETURN_FLAG IS NULL";
                o_DBFactory.ABC_toTest.Edit_Data(DstrSQL);

                //新增陳核資料
                string strSQL = @"INSERT INTO C_CHANGE_DLTB01_HISTORY (SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID) 
                                    VALUES ( NEXT VALUE FOR dbo.C_CHANGE_DLTB01_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID) ";

                //取得審核人編號
                string RID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + e.CommandArgument.ToString() + "'");
                //設定假別編號
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

                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("LEAVE_SN", LEAVE_SN),
                    new SqlParameter("REVIEW_ID", RID),
                    new SqlParameter("LETTER_DATE", ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyyMMdd")),
                    new SqlParameter("LETTER_TIME", DateTime.Now.ToString("HH:mm:ss")),
                    new SqlParameter("LEAVE_SCHEDULE_SN", 2),
                    new SqlParameter("DLTB01_SN", hdf_MZ_DLTB01_SN.Value),
                    new SqlParameter("PROCESS_STATUS", "4"),
                    new SqlParameter("MZ_MID", Session["ADPMZ_ID"].ToStringNullSafe())
                };

                if (o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('送核失敗!')", true);
                }
                ViewState.Remove("s");
                Panel_select_ModalPopupExtender.Hide();
            }
        }

        protected void btn_exit_Click(object sender, EventArgs e)
        {
            Panel_select_ModalPopupExtender.Hide();
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            //string strSQL = string.Format("DELETE FROM C_CHANGE_DLTB01_HISTORY WHERE SN={0}", hdf_C_CHANGE_DLTB01_SN.Value);
            string strSQL = string.Format("DELETE FROM C_CHANGE_DLTB01 WHERE MZ_DLTB01_SN={0}", ViewState["MZ_DLTB01_SN"]);

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btn_onlinecheck.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗，請洽資訊人員！')", true);
            }

            txt_Close_OR_Open(false);
            /*
            Label_ORGINAL_MZ_TTIME.Text = "";
            Label_ORGINAL_MZ_IDATE1.Text = "";
            Label_ORGINAL_MZ_ITIME1.Text = "";
            Label_ORGINAL_MZ_ODATE.Text = "";
            Label_ORGINAL_MZ_OTIME.Text = "";
            Label_ORGINAL_MZ_TDAY.Text = "";
            */
            TextBox_MZ_TDAY.Text = "";
            TextBox_MZ_TTIME.Text = "";
            TextBox_MZ_OTIME.Text = "";
            TextBox_MZ_ODATE.Text = "";
            TextBox_MZ_ITIME1.Text = "";
            TextBox_MZ_IDATE1.Text = "";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetValue(GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())]["MZ_DLTB01_SN"].ToString());
            }
        }

        /// <summary>
        /// text 開或關
        /// </summary>
        /// <param name="YN"></param>
        public void txt_Close_OR_Open(bool YN)
        {
            foreach (object obj in Panel1.Controls)
            {
                if (obj is TextBox)
                {
                    TextBox tb = (TextBox)obj;
                    tb.Enabled = YN;
                    
                }
            }
        
        
        }
    }
}
