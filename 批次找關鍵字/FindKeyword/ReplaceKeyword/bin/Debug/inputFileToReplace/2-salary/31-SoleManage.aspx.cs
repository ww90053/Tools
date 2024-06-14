using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public partial class _1_SoleManage : System.Web.UI.Page
    {
        string caseid
        {
            set { ViewState["caseid"] = value; }
            get { return ViewState["caseid"].ToString(); }
        }
        Sole currentSole
        {
            set { ViewState["currentSole"] = value; }
            get { return (Sole)ViewState["currentSole"]; }
        }

        // 項目名稱
        private string strNUM
        {
            get
            {
                return txt_num.Text;
            }
            set
            {
                txt_num.Text = value;
                lbl_num.Text = Sole.getName(txt_num.Text);
            }
        }

        string updNum
        {
            get
            {
                return txt_updNum.Text;
            }
            set
            {
                txt_updNum.Text = value;
                lb_updNum.Text = Sole.getName(txt_updNum.Text);
                updNumChanged();
            }
        }

        ////
        int intSecand_HealthyPay
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_Secondhealth_pay.Text);
            }
            set
            {
                txt_Secondhealth_pay.Text = value.ToString();
            }
        }

        string strExternal
        {
            get
            {
                return ddl_External.SelectedValue;
            }
            set
            {
                ddl_External.SelectedValue = value.ToString();
            }


        }


        // 2013/11/07
        private string strPAY_AD
        {
            get
            {
                return SalaryPublic.strLoginEXAD;
            }
        }
        // 2013/11/07
        ////

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.fillDropDownList(ref ddl_payad);
                SalaryPublic.fillUnitDropDownList(ref ddl_unit, ddl_payad.SelectedValue);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            search();
        }

        protected void gv_Result_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "upd":

                    currentSole = new Sole(e.CommandArgument.ToString());

                    lb_polno.Text = currentSole.polno;
                    lb_idcard.Text = currentSole.idcard;
                    lb_name.Text = currentSole.name;
                    updNum = currentSole.num;
                    txt_note.Text = currentSole.note;
                    txt_pay.Text = currentSole.pay.ToString();
                    txt_tax.Text = currentSole.tax.ToString();
                    txt_pay1.Text = currentSole.pay1.ToString();
                    txt_pay2.Text = currentSole.pay2.ToString();
                    txt_pay3.Text = currentSole.pay3.ToString();
                    txt_saveUntax.Text = currentSole.saveUntax.ToString();
                    txt_extra01.Text = currentSole.extra01.ToString();
                    ddl_updTaxID.SelectedValue = currentSole.taxesid;
                    ddl_updTaxID1.SelectedValue = currentSole.taxesid1;
                    btn_showUpdate_ModalPopupExtender.Show();

                    ////
                    txt_Secondhealth_pay.Text = currentSole.second_health_pay.ToString();
                    ddl_External.SelectedValue = currentSole.external;
                    //ViewState["SN"] = e.CommandArgument.ToString();
                    ////

                    break;

                case "del":

                    ////

                    ////

                    currentSole = new Sole(e.CommandArgument.ToString());
                    currentSole.delete();






                    search();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功')", true);
                    break;
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int pay;
            int tax;
            int pay1;
            int pay2;
            int pay3;
            int saveUntax;
            int extra01;

            int.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_tax.Text, out tax);
            int.TryParse(txt_pay1.Text, out pay1);
            int.TryParse(txt_pay2.Text, out pay2);
            int.TryParse(txt_pay3.Text, out pay3);
            int.TryParse(txt_saveUntax.Text, out saveUntax);
            int.TryParse(txt_extra01.Text, out extra01);




            //currentSole.update(lb_polno.Text, updNum, ddl_updTaxID.SelectedValue, ddl_updTaxID1.SelectedValue, pay, tax, pay1, pay2, pay3, saveUntax, extra01, txt_note.Text);

            ////


            string EXCEED = UPDATE_Update_SUM_Bonus(lb_idcard.Text);
            if (EXCEED == "Y")
                intSecand_HealthyPay = (int)(pay * (0.02));
            //20141216
            //else
            //    intSecand_HealthyPay = 0;

            currentSole.update(lb_polno.Text, updNum, ddl_updTaxID.SelectedValue, ddl_updTaxID1.SelectedValue, pay, tax, pay1, pay2, pay3, saveUntax, extra01, txt_note.Text, intSecand_HealthyPay, strExternal);



            // ViewState.Remove("SN");
            //ViewState["Secand_HealthyPay"] = false;
            ////

            currentSole = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改成功')", true);



            search();




        }

        protected void gv_Result_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Result.PageIndex = e.NewPageIndex;
            search();
        }

        void search()
        {
            string msg = "";
            if (txt_da.Text.Trim().Length == 0)
                msg += "入帳日期不可空白\\r\\n";

            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + msg + "')", true);
                return;
            }

            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"SELECT S_SNID,DA ,CASEID ,PAY_UNIT , NAME , ITEM, LOCKDB , NOTE ,
               MZ_POLNO , (PAY-TOTALSUB) TOTAL FROM VW_ALL_SOLE_DATA WHERE ROWNUM<=500 AND PAY_AD=@PAY_AD";


            //sql = @"SELECT VW_ALL_SOLE_DATA.*, (PAY-TOTALSUB) TOTAL FROM VW_ALL_SOLE_DATA WHERE ROWNUM<=500 AND PAY_AD=@PAY_AD";

            ops.Add(new SqlParameter("PAY_AD", ddl_payad.SelectedValue));

            if (ddl_unit.SelectedIndex > 0)
            {
                sql += " AND MZ_UNIT=@MZ_UNIT";
                ops.Add(new SqlParameter("MZ_UNIT", ddl_unit.SelectedValue));
            }
            if (txt_num.Text.Length > 0)
            {
                sql += " AND NUM=@NUM";
                ops.Add(new SqlParameter("NUM", txt_num.Text));
            }
            if (txt_da.Text.Length > 0)
            {
                sql += " AND DA LIKE @DA";
                ops.Add(new SqlParameter("DA", "%" + txt_da.Text + "%"));
            }
            if (txt_idcard.Text.Length > 0)
            {
                sql += " AND IDNO LIKE @IDNO";
                ops.Add(new SqlParameter("IDNO", "%" + txt_idcard.Text + "%"));
            }
            if (txt_polno.Text.Length > 0)
            {
                sql += " AND MZ_POLNO LIKE @MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", "%" + txt_polno.Text + "%"));
            }
            if (txt_name.Text.Length > 0)
            {
                sql += " AND NAME LIKE @NAME";
                ops.Add(new SqlParameter("NAME", "%" + txt_name.Text + "%"));
            }
            if (tbCaseID.Text.Length > 0)
            {
                sql += " AND CASEID LIKE @CASEID";
                ops.Add(new SqlParameter("CASEID", "%" + tbCaseID.Text + "%"));
            }
            sql += " ORDER BY DA DESC, CASEID ASC, MZ_POLNO ASC";

            DataTable dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            gv_Result.DataSource = dt;
            gv_Result.DataBind();
        }

        #region 呈現項目選擇panel

        protected void txt_num_TextChanged(object sender, EventArgs e)
        {
            strNUM = txt_num.Text;


        }

        protected void btn_showNum_Click(object sender, EventArgs e)
        {
            showCode();
            btn_showNum_ModalPopupExtender.Show();
        }

        protected void gv_num_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (currentSole == null)
            {
                strNUM = e.CommandArgument.ToString().PadLeft(2, '0');

            }
            else
            {
                updNum = e.CommandArgument.ToString().PadLeft(2, '0');

                //Calculate_PAY();
                btn_showUpdate_ModalPopupExtender.Show();

            }
        }

        protected void gv_num_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_num.PageIndex = e.NewPageIndex;
            showCode();

            btn_showNum_ModalPopupExtender.Show();
        }

        // 產生代碼選擇器的資料
        void showCode()
        {
            string sql;

            sql = "SELECT ID, NAME FROM B_SOLEITEM ORDER BY ID";

            gv_num.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            gv_num.DataBind();
        }

        #endregion

        #region 修改用的panel的postback事件

        protected void btn_updNum_Click(object sender, EventArgs e)
        {
            showCode();
            btn_updNum_ModalPopupExtender.Show();
        }

        protected void txt_updNum_TextChanged(object sender, EventArgs e)
        {
            updNum = txt_updNum.Text;


            //Calculate_PAY();
            btn_showUpdate_ModalPopupExtender.Show();
        }

        #endregion

        protected void DropDownList_TAXES_ID1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Label_TAXES_DATA.Text = Sole.getTaxNote(DropDownList_TAXES_ID1.SelectedValue,
        }

        void updNumChanged()
        {
            Sole.setDDLTaxID(ref ddl_updTaxID, updNum);
            Sole.setDDLTaxID1(ref ddl_updTaxID1, ddl_updTaxID.SelectedValue);
        }

        protected void ddl_updTaxID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sole.setDDLTaxID1(ref ddl_updTaxID1, ddl_updTaxID.SelectedValue);
            btn_updNum_ModalPopupExtender.Show();
        }

        protected void ddl_payad_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref ddl_unit, ddl_payad.SelectedValue);
        }

        protected void ibt_updateClose_Click(object sender, ImageClickEventArgs e)
        {
            currentSole = null;

            ////
            //ViewState["Secand_HealthyPay"] = false;
            //ViewState.Remove("SN");
            ////
        }

        protected void ibt_numClose_Click(object sender, ImageClickEventArgs e)
        {
            if (currentSole != null)
                btn_showUpdate_ModalPopupExtender.Show();
        }

        protected void gv_Result_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            Label lbl = (Label)e.Row.Cells[9].FindControl("lb_lock");
            if (lbl.Text == "Y")
            {
                ((Button)e.Row.Cells[0].FindControl("btn_update")).Enabled = false;
                ((Button)e.Row.Cells[1].FindControl("btn_delete")).Enabled = false;
            }
        }

        protected void btn_deleteChecked_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_Result.Rows)
            {
                if (!((CheckBox)row.Cells[1].FindControl("cb_del")).Checked)
                    continue;

                // sam.hsu 20201203
                //if (!((CheckBox) row.Cells[1].FindControl("cb_del")).Checked)
                //{
                Label lbl = (Label)row.Cells[9].FindControl("lb_lock");
                if (lbl.Text == "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('有勾選到已關帳故不可刪除，請重新確認！')", true);
                    continue;
                }
                //}

                string sn = ((HiddenField)row.Cells[1].FindControl("hf_sn")).Value;
                Sole sole = new Sole(sn);


                DELETE_Update_SUM_Bonus(sole);



                sole.delete();
            }
            search();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功')", true);
        }

        protected void cb_delAll_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = ((CheckBox)sender).Checked;
            foreach (GridViewRow row in gv_Result.Rows)
            {
                // sam.hsu 20201203

                ((CheckBox)row.Cells[1].FindControl("cb_del")).Checked = chk;
            }
        }





        /// <summary>
        /// 修改後SOLE後 ---Update 二代健保table
        /// </summary>
        public string UPDATE_Update_SUM_Bonus(string MZ_ID)
        {
            try
            {
                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE  (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND  MZ_ID='" + MZ_ID + "' ";


                DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (Check_Srank.Rows.Count > 0)
                {

                    //??????
                    //strSQL = "SELECT * FROM B_SOLE WHERE   S_SNID='" + currentSole.sn + "'";

                    //DataTable old = new DataTable();
                    double Calculate = 0;
                    //old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    string Exceed = "N";
                    //如果有修改發送項目
                    //if ((old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40") && (!(updNum == "08" || updNum == "16" || updNum == "19" || updNum == "20" || updNum == "22" || updNum == "27" || updNum == "40")))

                    if ((currentSole.num == "08" || currentSole.num == "16" || currentSole.num == "19" || currentSole.num == "20" || currentSole.num == "22" || currentSole.num == "27" || currentSole.num == "40") &&
                        (!(updNum == "08" || updNum == "16" || updNum == "19" || updNum == "20" || updNum == "22" || updNum == "27" || updNum == "40")))
                    {
                        strSQL = "SELECT SOLE , TOTAL , INCREASE_X4 , EXCEED  FROM B_SUM_BONUS WHERE  IDCARD='" + MZ_ID + "' AND AYEAR='" + txt_da.Text.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                        if (data.Rows.Count > 0)
                        {
                            Calculate = 0 - Convert.ToDouble(currentSole.pay);
                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole += Calculate;
                            Total += Calculate;

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";

                            }
                            else { Exceed = "N"; }
                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD='" + strPAY_AD + "'";

                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                     new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                      new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                     new SqlParameter("IDCARD",SqlDbType.VarChar){Value = MZ_ID},
                     new SqlParameter("AYEAR",SqlDbType.VarChar){Value = txt_da.Text.Substring(0, 3)},
                    };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                            return Exceed;
                        }

                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保統計資料');", true);

                            return Exceed;
                        }


                    }
                    //如果"沒"有修改發送項目
                    else if (updNum == "08" || updNum == "16" || updNum == "19" || updNum == "20" || updNum == "22" || updNum == "27" || updNum == "40")
                    {

                        strSQL = "SELECT SOLE , TOTAL , INCREASE_X4 , EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + MZ_ID + "' AND AYEAR='" + txt_da.Text.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                        if (data.Rows.Count > 0)
                        {


                            if (txt_pay.Text != "" && txt_pay.Text != null)
                            {
                                //if (old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")

                                if (currentSole.num == "08" || currentSole.num == "16" || currentSole.num == "19" || currentSole.num == "20" || currentSole.num == "22" || currentSole.num == "27" || currentSole.num == "40")
                                {
                                    //Calculate = Convert.ToDouble(txt_pay.Text) - Convert.ToDouble(old.Rows[0]["PAY"]);
                                    Calculate = Convert.ToDouble(txt_pay.Text) - Convert.ToDouble(currentSole.pay);

                                }

                                else
                                {

                                    Calculate = Convert.ToDouble(txt_pay.Text);

                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('應發金額不可為空白');", true);

                                return Exceed;
                            }

                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole += Calculate;
                            Total += Calculate;

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";

                            }
                            else { Exceed = "N"; }
                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD='" + strPAY_AD + "'";

                            SqlParameter[] parameterList = {

                                                              new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},

                                                              new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},

                                                              new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},

                                                              new SqlParameter("IDCARD",SqlDbType.VarChar){Value = MZ_ID},

                                                              new SqlParameter("AYEAR",SqlDbType.VarChar){Value = txt_da.Text.Substring(0, 3)},

                                                          };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                            return Exceed;

                        }

                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保統計資料');", true);

                            return Exceed;
                        }

                    }

                }
            }
            catch
            {

            }
            return "N";
        }


        /// <summary>
        /// 刪除SOLE後 --- Update二代健保table
        /// </summary>
        public string DELETE_Update_SUM_Bonus(Sole sole)
        {
            try
            {
                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE   (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND  MZ_ID='" + sole.idcard + "'";


                DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (Check_Srank.Rows.Count > 0)
                {


                    //strSQL = "SELECT * FROM B_SOLE WHERE   S_SNID='" + sole.sn.ToString() + "'";

                    //DataTable old = new DataTable();
                    double Calculate = 0;
                    //old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    string Exceed = "N";
                    //如果舊資料是指定項目時
                    //if (old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")
                    if (sole.num == "08" || sole.num == "16" || sole.num == "19" || sole.num == "20" || sole.num == "22" || sole.num == "27" || sole.num == "40")
                    {

                        strSQL = "SELECT SOLE ,TOTAL ,INCREASE_X4 ,EXCEED   FROM B_SUM_BONUS WHERE  IDCARD='" + sole.idcard + "' AND AYEAR='" + sole.da.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                        if (data.Rows.Count > 0)
                        {
                            Calculate = Convert.ToDouble(sole.pay); //old.Rows[0]["PAY"]
                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole -= Calculate;
                            Total -= Calculate;

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";

                            }
                            else { Exceed = "N"; }
                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD='" + strPAY_AD + "'";

                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                    new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                    new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = sole.idcard},
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = sole.da.Substring(0, 3)},
                    };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                            return Exceed;
                        }

                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保統計資料');", true);
                            //
                            return Exceed;
                        }


                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保統計資料');", true);

                        return Exceed;

                    }
                }
            }

            catch
            {

            }
            return "N";
        }



        //public void Calculate_PAY()
        //{

        //    try
        //    {
        //        //計算是否超過 超過給狀態給true.可計算...沒超過給false.給0
        //        string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + lb_idcard.Text + "' AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )";


        //        DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //        if (Check_Srank.Rows.Count > 0)
        //        {

        //            //??????
        //            strSQL = "SELECT * FROM B_SOLE WHERE   S_SNID='" + currentSole.sn + "'";

        //            DataTable old = new DataTable();
        //            double Calculate = 0;
        //            old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //            //如果有修改發送項目
        //            if ((old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40") && (!(updNum == "08" || updNum == "16" || updNum == "19" || updNum == "20" || updNum == "22" || updNum == "27" || updNum == "40")))
        //            {
        //                strSQL = "SELECT * FROM B_SUM_BONUS WHERE  IDCARD='" + lb_idcard.Text + "' AND AYEAR='" + txt_da.Text.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

        //                DataTable data = new DataTable();

        //                data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //                if (data.Rows.Count > 0)
        //                {
        //                    Calculate = 0 - Convert.ToDouble(old.Rows[0]["PAY"]);
        //                    double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
        //                    double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
        //                    double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
        //                    string Exceed = data.Rows[0]["EXCEED"].ToString();

        //                    Sole += Calculate;
        //                    Total += Calculate;

        //                    if (Total > Increase_x4)
        //                    {
        //                        //if (Exceed != "Y")
        //                        if ((bool)ViewState["Secand_HealthyPay"] == false)
        //                        {
        //                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過獎金總額的4倍，須扣二代健保');", true);
        //                        }
        //                        Exceed = "Y";
        //                        ViewState["Secand_HealthyPay"] = true;
        //                    }
        //                    else
        //                    {
        //                        Exceed = "N";
        //                        ViewState["Secand_HealthyPay"] = false;
        //                    }

        //                }

        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保統計資料');", true);

        //                    return;
        //                }


        //            }
        //            //如果"沒"有修改發送項目
        //            else if (updNum == "08" || updNum == "16" || updNum == "19" || updNum == "20" || updNum == "22" || updNum == "27" || updNum == "40")
        //            {

        //                strSQL = "SELECT * FROM B_SUM_BONUS WHERE  IDCARD='" + lb_idcard.Text + "' AND AYEAR='" + txt_da.Text.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

        //                DataTable data = new DataTable();

        //                data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //                if (data.Rows.Count > 0)
        //                {


        //                    if (txt_pay.Text != "" && txt_pay.Text != null)
        //                    {

        //                        if (old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")
        //                        {
        //                            Calculate = Convert.ToDouble(txt_pay.Text) - Convert.ToDouble(old.Rows[0]["PAY"]);


        //                        }

        //                        else
        //                        {

        //                            Calculate = Convert.ToDouble(txt_pay.Text);

        //                        }
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('應發金額不可為空白');", true);

        //                        return;
        //                    }

        //                    double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
        //                    double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
        //                    double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
        //                    string Exceed = data.Rows[0]["EXCEED"].ToString();

        //                    Sole += Calculate;
        //                    Total += Calculate;

        //                    if (Total > Increase_x4)
        //                    {
        //                        //if (Exceed != "Y")
        //                        if ((bool)ViewState["Secand_HealthyPay"] == false)
        //                        {
        //                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過獎金總額的4倍，須扣二代健保');", true);
        //                        }
        //                        Exceed = "Y";
        //                        ViewState["Secand_HealthyPay"] = true;
        //                    }
        //                    else
        //                    {
        //                        Exceed = "N";
        //                        ViewState["Secand_HealthyPay"] = false;
        //                    }



        //                }

        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保統計資料');", true);

        //                    return;
        //                }

        //            }
        //        }



        //        if ((bool)ViewState["Secand_HealthyPay"] == true)
        //        {
        //            double Pay = Convert.ToDouble(txt_pay.Text);
        //            txt_Secondhealth_pay.Text = (Pay * 0.02).ToString();




        //        }

        //        else
        //        {

        //            txt_Secondhealth_pay.Text = "0";

        //        }

        //    }

        //    catch
        //    { 

        //    }
        //    //btn_showUpdate_ModalPopupExtender.Show();


        //}



        protected void txt_pay_TextChanged1(object sender, EventArgs e)
        {

            //Calculate_PAY();
            btn_showUpdate_ModalPopupExtender.Show();

            //if (updNum == "08" || updNum == "16" || updNum == "19" || updNum == "20" || updNum == "22" || updNum == "27" || updNum == "40")
            //{

            //    strSQL = "SELECT * FROM B_SUM_BONUS WHERE  IDCARD='" + lb_idcard.Text + "' AND AYEAR='" + txt_da.Text.Substring(0, 3) + "'";

            //    DataTable data = new DataTable();

            //    data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            //    if (data.Rows.Count > 0)
            //    {
            //        double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
            //        double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
            //        double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
            //        string Exceed = data.Rows[0]["EXCEED"].ToString();

            //        Sole += Convert.ToDouble(txt_pay.Text);
            //        Total += Convert.ToDouble(txt_pay.Text);

            //        if (Total > Increase_x4)
            //        {


            //            if (Exceed != "Y")
            //            {
            //                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過獎金總額的4倍，須扣二代健保');", true);
            //            }
            //            Exceed = "Y";
            //            ViewState["Secand_HealthyPay"] = true;

            //        }
            //        else
            //        {
            //            Exceed = "N";
            //            ViewState["Secand_HealthyPay"] = false;
            //        }
            //    }
            //}







        }



    }
}
