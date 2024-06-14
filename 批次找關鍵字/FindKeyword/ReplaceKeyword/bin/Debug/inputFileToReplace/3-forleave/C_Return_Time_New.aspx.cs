using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

namespace TPPDDB._3_forleave
{
    public partial class C_Return_Time_New : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                //by MQ ------------------------------20100331           
                C.set_Panel_EnterToTAB(ref this.Panel1);


                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                //C.fill_AD_POST(DropDownList_EXAD);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_EXAD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_EXAD);
                }
                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();


                chk_TPMGroup();
            }
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //權限E & D 選擇所屬單位並鎖單位 matthew
            if (ViewState["C_strGID"].ToString() == "E" || ViewState["C_strGID"].ToString() == "D")
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
            else
            {
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
            }
            TextBox_MZ_ID.Text = "";
        }

        private void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                    break;
                case "B":

                    break;
                case "C":
                    DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_EXAD.Enabled = false;
                    break;

                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void btSearchlist_Click(object sender, EventArgs e)
        {
            if (TextBox_MZ_DATE.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入加班補休日期');", true);
                return;
            }
            //加班補休時間 TextBox_MZ_TIME 可以不輸入,若輸入了,格式就要正確 HH:MM
            string pattern = @"^([0-1]?[0-9]|2[0-3])[0-5][0-9]$";
            if (TextBox_MZ_TIME.Text != "" && Regex.Match(TextBox_MZ_TIME.Text, pattern).Success==false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假起時,格式錯誤');", true);
                return;
            }

            if (TextBox_MZ_ID.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入身分證號');", true);
                return;
            }


            string ad = DropDownList_EXAD.SelectedValue;
            string unit = DropDownList_EXUNIT.SelectedValue;

            DataTable tempDT = new DataTable();
            tempDT.Columns.Clear();
            tempDT.Columns.Add("MZ_IDATE1", typeof(string));
            tempDT.Columns.Add("MZ_HOUR", typeof(string));
            //抓取當天的請假資料
            string strSQL = string.Format(@"
                SELECT * FROM C_DLTB01 
                WHERE MZ_IDATE1='{0}'  
                AND (MZ_STATUS <> '3' or MZ_STATUS is null) ", TextBox_MZ_DATE.Text);
            //如果有指定請假開始時間
            if (TextBox_MZ_TIME.Text != "")
            {   //指定開始時間,EX: 1530 => 15:30
                string MZ_ITIME1 = TextBox_MZ_TIME.Text.Substring(0, 2) + ":" + TextBox_MZ_TIME.Text.Substring(2, 2);
                strSQL = string.Format(strSQL + " and MZ_ITIME1 = '{0}' ", MZ_ITIME1);
            }
            if (ad != "")
            {
                strSQL = string.Format(strSQL + " and MZ_EXAD = '{0}' ", ad);
            }
            if (unit != "")
            {
                strSQL = string.Format(strSQL + " and MZ_EXUNIT = '{0}' ", unit);
            }
            if (TextBox_MZ_ID.Text != "")
            {
                strSQL = string.Format(strSQL + " and MZ_ID = '{0}' ", TextBox_MZ_ID.Text);
            }

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            string MZ_DLTB01_SN = "";

            if (dt.Rows.Count > 0)
            {
                MZ_DLTB01_SN = "";
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    MZ_DLTB01_SN += "'" + dt.Rows[k]["MZ_DLTB01_SN"].ToString() + "',";
                }
                MZ_DLTB01_SN = MZ_DLTB01_SN.Substring(0, MZ_DLTB01_SN.Length - 1);
            }
            else
            {
                MZ_DLTB01_SN = "";
            }

            if(MZ_DLTB01_SN != "")
            {
                //加班補休參照檔,加班和補休資料之間的關聯
                strSQL = string.Format("SELECT * FROM C_OVERTIME_FOR_DLTB01 WHERE MZ_DLTB01_SN in ({0}) ", MZ_DLTB01_SN);
                DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (dt1.Rows.Count > 0)
                {
                    for (int k = 0; k < dt1.Rows.Count; k++)
                    {
                        //加班基本流水號
                        string OVERTIME_SN = dt1.Rows[k]["OVERTIME_SN"].ToString() ;
                        //補休時數(分)
                        string REST_HOUR = dt1.Rows[k]["REST_HOUR"].ToString();
                        //加班日期
                        string OVER_DAY = o_DBFactory.ABC_toTest.vExecSQL("SELECT OVER_DAY FROM C_OVERTIME_BASE WHERE SN='" + OVERTIME_SN + "'");

                        DataRow dr = tempDT.NewRow();
                        dr["MZ_IDATE1"] = OVER_DAY;
                        dr["MZ_HOUR"] = REST_HOUR;
                        tempDT.Rows.Add(dr);
                    }
                }
            }

            gv_data.DataSource = tempDT;
            gv_data.AllowPaging = true;
            gv_data.DataBind();
            gv_data.Visible = true;
        }
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (TextBox_MZ_DATE.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入加班補休日期');", true);
                return;
            }
            //加班補休時間 TextBox_MZ_TIME 可以不輸入,若輸入了,格式就要正確 HH:MM
            string pattern = @"^([0-1]?[0-9]|2[0-3])[0-5][0-9]$";
            if (TextBox_MZ_TIME.Text != "" && Regex.Match(TextBox_MZ_TIME.Text, pattern).Success == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假起時,格式錯誤');", true);
                return;
            }

            if (TextBox_MZ_ID.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入身分證號');", true);
                return;
            }

            doSearch();
        }

        void doSearch()
        {
            string ad = DropDownList_EXAD.SelectedValue;
            string unit = DropDownList_EXUNIT.SelectedValue;
            //抓取當天請假的資料
            string strSQL = string.Format(@"
                SELECT * FROM C_DLTB01 
                WHERE MZ_IDATE1='{0}'  
                AND (MZ_STATUS <> '3' or MZ_STATUS is null) 
            ", TextBox_MZ_DATE.Text);
            //如果有指定請假開始時間
            if (TextBox_MZ_TIME.Text != "")
            {   //指定開始時間,EX: 1530 => 15:30
                string MZ_ITIME1 = TextBox_MZ_TIME.Text.Substring(0, 2) + ":" + TextBox_MZ_TIME.Text.Substring(2, 2);
                strSQL = string.Format(strSQL + " and MZ_ITIME1 = '{0}' ", MZ_ITIME1);
            }

            if (ad != "")
            {
                strSQL = string.Format(strSQL + " and MZ_EXAD = '{0}' ", ad);
            }
            if (unit != "")
            {
                strSQL = string.Format(strSQL + " and MZ_EXUNIT = '{0}' ", unit);
            }
            if (TextBox_MZ_ID.Text != "")
            {
                strSQL = string.Format(strSQL + " and MZ_ID = '{0}' ", TextBox_MZ_ID.Text);
            }

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            //DataTable gv_dt = new DataTable();

            //gv_dt.Columns.Add("MZ_IDATE1", typeof(string));
            //gv_dt.Columns.Add("MZ_HOUR", typeof(string));

            //if (dt.Rows.Count > 0)
            //{
            //    hdf_OTIME.Value = dt.Rows[0]["OTIME"].ToString();
            //    hdf_MZ_RESTDATE.Value = dt.Rows[0]["MZ_RESTDATE"].ToString();
            //    if (!string.IsNullOrEmpty(dt.Rows[0]["MZ_RESTDATE"].ToString()))
            //    {
            //        string[] s = dt.Rows[0]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

            //        for (int j = 0; j < s.Count(); j++)
            //        {
            //            string[] y = s[j].Split('：');

            //            DataRow gv_dr = gv_dt.NewRow();

            //            gv_dr["MZ_IDATE1"] = y[0];
            //            gv_dr["MZ_HOUR"] = y[1];

            //            gv_dt.Rows.Add(gv_dr);
            //        }
            //    }
            //}
            string DLTB01_SN = "";
            string strSQL2 = "";
            string strRest = "";
            string strtmp = "";
            if (dt.Rows.Count > 0)
            {
                for(int k = 0; k< dt.Rows.Count; k++)
                { 
                    DLTB01_SN = dt.Rows[k]["MZ_DLTB01_SN"].ToString();
                    //ViewState["gv_dt"] = gv_dt;
                    //gv_data.DataSource = gv_dt;
                    //gv_data.DataBind();
                    //處理還原功能
                    //先還原 C_OVERTIME_BASE

                    //抓取加班和補修之間的關聯
                    strSQL = string.Format("SELECT * FROM C_OVERTIME_FOR_DLTB01 WHERE MZ_DLTB01_SN='{0}'", DLTB01_SN);
                    DataTable dt_base = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    for (int i = 0; i < dt_base.Rows.Count ; i++)
                    {
                        //抓取加班資料中的REST_DATE_RECORD欄位,格式: 1110429：240，
                        strSQL2 = string.Format("SELECT REST_DATE_RECORD FROM C_OVERTIME_Base WHERE SN='{0}'", dt_base.Rows[i]["OVERTIME_SN"].ToString());
                        strRest = o_DBFactory.ABC_toTest.vExecSQL(strSQL2);
                        //將補休時間加回去
                        //如果REST_DATE_RECORD裡面沒有全型逗點
                        if (strRest.IndexOf("，") < 0)
                        {
                            strSQL = string.Format("update C_OVERTIME_Base set SURPLUS_TOTAL = SURPLUS_TOTAL + {0} , REST_HOUR = REST_HOUR - {0} , REST_DATE_RECORD = replace('" + strRest + "','" + TextBox_MZ_DATE.Text + "：{0}')  WHERE SN='{1}'", dt_base.Rows[i]["REST_HOUR"].ToString(), dt_base.Rows[i]["OVERTIME_SN"].ToString());
                            o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                        }
                        //有全形逗點
                        else
                        {
                            strSQL = string.Format("update C_OVERTIME_Base set SURPLUS_TOTAL = SURPLUS_TOTAL + {0} , REST_HOUR = REST_HOUR - {0} , REST_DATE_RECORD = replace('" + strRest + "','" + TextBox_MZ_DATE.Text + "：{0}，')  WHERE SN='{1}'", dt_base.Rows[i]["REST_HOUR"].ToString(), dt_base.Rows[i]["OVERTIME_SN"].ToString());
                            o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                        }

                        //strSQL2 = string.Format("SELECT REST_DATE_RECORD FROM C_OVERTIME_Base WHERE SN='{0}'", dt_base.Rows[i]["OVERTIME_SN"].ToString());
                        //strtmp = o_DBFactory.ABC_toTest.vExecSQL(strSQL2);
                        //if (strtmp == "，")
                        //{
                        //    strSQL = string.Format("update C_OVERTIME_Base set REST_DATE_RECORD = ''  WHERE SN='{0}'", dt_base.Rows[i]["OVERTIME_SN"].ToString());
                        //    o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                        //}
                    }
                    //再還原 C_DLTB01
                    //不須處理假的部分 , 都是由各分局手動刪除 , 自行還原時數即可
                    //strSQL = string.Format("update C_OVERTIME_Base set SURPLUS_TOTAL = SURPLUS_TOTAL + {0} , REST_HOUR = REST_HOUR - {0} WHERE SN='{1}'", dt_base.Rows[i]["REST_HOUR"].ToString(), dt_base.Rows[i]["OVERTIME_SN"].ToString());
                    //再還原 C_OVERTIME_FOR_DLTB01
                    strSQL = string.Format("Delete from C_OVERTIME_FOR_DLTB01  WHERE MZ_DLTB01_SN='{0}'", DLTB01_SN);
                    o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                }
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('還原成功！');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無此日期相關資料！');", true);
                //gv_data.DataBind();
                return;
            }
        }

        protected void TextBox_MZ_HOUR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int index = gvr.RowIndex;

            TextBox txt_hour = (TextBox)gv.Rows[index].Cells[1].FindControl("TextBox_MZ_HOUR");

            int hour;

            int.TryParse(txt_hour.Text, out hour);

            DataTable dt = ViewState["gv_dt"] as DataTable;

            if (hour < 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('時數錯誤！');", true);
                return;
            }
            else if (hour == 0)
            {
                dt.Rows.Remove(dt.Rows[index]);
            }
            else
            {
                dt.Rows[index][1] = txt_hour.Text;
            }

            update_restdata(dt);

        }

        private void update_restdata(DataTable dt)
        {
            string MZ_RESTDATE = "";

            int RESTHOUR = 0;

            int parHour;

            int OTIME;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int.TryParse(dt.Rows[i][1].ToString(), out parHour);

                RESTHOUR += parHour;

                if (i == 0)
                    MZ_RESTDATE += dt.Rows[i][0].ToString() + "：" + dt.Rows[i][1].ToString();
                else
                    MZ_RESTDATE += "，" + dt.Rows[i][0].ToString() + "：" + dt.Rows[i][1].ToString();

            }

            int.TryParse(hdf_OTIME.Value, out OTIME);

            string strSQL = string.Format("UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='{2}',MZ_RESTHOUR={3} WHERE MZ_DATE='{0}' AND MZ_ID='{1}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' ", TextBox_MZ_DATE.Text, TextBox_MZ_ID.Text, MZ_RESTDATE, (OTIME - RESTHOUR < 0 ? 0 : OTIME - RESTHOUR));

            //Log
            LogModel.saveLog("COHI", "U", strSQL, new List<System.Data.SqlClient.SqlParameter>(), Request.QueryString["TPM_FION"], "補休時數回復，更新可補休日");

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            doSearch();
        }

        protected void gv_data_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Del":
                    int index = Convert.ToInt32(e.CommandArgument);

                    DataTable dt = ViewState["gv_dt"] as DataTable;
                        
                    dt.Rows.Remove(dt.Rows[index]);

                    update_restdata(dt);

                    break;
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = "";

            //gv_data.DataSource = null;
            //gv_data.AllowPaging = true;
            //gv_data.DataBind();

            gv_data.Visible = false;
        }


    }
}
