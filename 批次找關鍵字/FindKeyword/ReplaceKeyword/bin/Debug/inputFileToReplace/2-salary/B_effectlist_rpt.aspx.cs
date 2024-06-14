using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace TPPDDB._2_salary
{
    public partial class B_effectlist_rpt : System.Web.UI.Page
    {
        //string strSQL;
        DataTable temp = new DataTable();
       
        int TPM_FION=0 ;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicYear();
            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            #region 舊的

            /*
             * 
            rpt_dt = rpt_dt_init();
            strSQL = string.Format("SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')", strAD);
            DataTable unit_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //工友用
            int v_people_acount = 0;
            int v_salarypay1 = 0;
            int v_boss = 0;
            int v_profess = 0;
            int v_workp_technics = 0;
            int v_far = 0;
            int v_ssum = 0;
            int v_tax = 0;
            int v_extra01 = 0;
            int v_total = 0;

            foreach (DataRow unit in unit_dt.Rows)
            {
                //課員用
                int people_acount = 0;
                int salarypay1 = 0;
                int boss = 0;
                int profess = 0;
                int workp_technics = 0;
                int far = 0;
                int ssum = 0;
                int tax = 0;
                int extra01 = 0;
                int total = 0;

                strSQL = string.Format("SELECT MZ_OCCC,SALARYPAY1,PROFESS,BOSS,WORKP,TECHNICS,FAR,(SALARYPAY1+PROFESS+BOSS+WORKP+TECHNICS+FAR) SSUM,TAX,EXTRA01,TOTAL FROM B_EFFECT WHERE PAY_AD='{0}' AND AYEAR='{1}' AND MZ_UNIT='{2}'",
                                        strAD, strAYEAR, unit["MZ_KCODE"]);
                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string occc = o_A_KTYPE.CODE_TO_NAME(dr["MZ_OCCC"].ToString(), "26");
                        if (occc == "工友" || occc == "司機" || occc == "技工" || occc == "約雇")
                        {
                            v_people_acount++;
                            v_salarypay1 += int.Parse(dr["SALARYPAY1"].ToString());
                            v_boss += int.Parse(dr["BOSS"].ToString());
                            v_profess += int.Parse(dr["PROFESS"].ToString());
                            v_workp_technics += int.Parse(dr["WORKP"].ToString()) + int.Parse(dr["TECHNICS"].ToString());
                            v_far += int.Parse(dr["FAR"].ToString());
                            v_ssum += int.Parse(dr["SSUM"].ToString());
                            v_tax += int.Parse(dr["TAX"].ToString());
                            v_extra01 += int.Parse(dr["EXTRA01"].ToString());
                            v_total += int.Parse(dr["TOTAL"].ToString());
                        }
                        else
                        {
                            people_acount++;
                            salarypay1 += int.Parse(dr["SALARYPAY1"].ToString());
                            boss += int.Parse(dr["BOSS"].ToString());
                            profess += int.Parse(dr["PROFESS"].ToString());
                            workp_technics += int.Parse(dr["WORKP"].ToString()) + int.Parse(dr["TECHNICS"].ToString());
                            far += int.Parse(dr["FAR"].ToString());
                            ssum += int.Parse(dr["SSUM"].ToString());
                            tax += int.Parse(dr["TAX"].ToString());
                            extra01 += int.Parse(dr["EXTRA01"].ToString());
                            total += int.Parse(dr["TOTAL"].ToString());
                        }
                    }
                }
                //新增課員
                DataRow ins = rpt_dt.NewRow();
                ins[0] = o_A_KTYPE.CODE_TO_NAME(unit["MZ_KCODE"].ToString(), "25");
                ins[1] = people_acount;
                ins[2] = salarypay1;
                ins[3] = boss;
                ins[4] = profess;
                ins[5] = workp_technics;
                ins[6] = far;
                ins[7] = ssum;
                ins[8] = tax;
                ins[9] = extra01;
                ins[10] = total;
                rpt_dt.Rows.Add(ins);
                //}
            }

            //加總課員數字用
            int s_people_acount = 0;
            int s_salarypay1 = 0;
            int s_boss = 0;
            int s_profess = 0;
            int s_workp_technics = 0;
            int s_far = 0;
            int s_ssum = 0;
            int s_tax = 0;
            int s_extra01 = 0;
            int s_total = 0;
            foreach (DataRow dr in rpt_dt.Rows)
            {
                s_people_acount += int.Parse(dr["PEOPLE_ACCOUNT"].ToString());
                s_salarypay1 += int.Parse(dr["SALARYPAY1"].ToString());
                s_boss += int.Parse(dr["BOSS"].ToString());
                s_profess += int.Parse(dr["PROFESS"].ToString());
                s_workp_technics += int.Parse(dr["WORKP_TECHNICS"].ToString());
                s_far += int.Parse(dr["FAR"].ToString());
                s_ssum += int.Parse(dr["SSUM"].ToString());
                s_tax += int.Parse(dr["TAX"].ToString());
                s_extra01 += int.Parse(dr["EXTRA01"].ToString());
                s_total += int.Parse(dr["TOTAL"].ToString());
            }

            int count = rpt_dt.Rows.Count;
            if (count < 29)
            {
                count = 29 - count;
            }
            else
            {
                count = 0;
            }
            //加空白欄位用
            for (int i = 0; i < count; i++)
            {
                DataRow dr1 = rpt_dt.NewRow();
                dr1[0] = "";
                rpt_dt.Rows.Add(dr1);
            }

            //加課室小計
            DataRow s_ins = rpt_dt.NewRow();
            s_ins[0] = "課室小計";
            s_ins[1] = s_people_acount;
            s_ins[2] = s_salarypay1;
            s_ins[3] = s_boss;
            s_ins[4] = s_profess;
            s_ins[5] = s_workp_technics;
            s_ins[6] = s_far;
            s_ins[7] = s_ssum;
            s_ins[8] = s_tax;
            s_ins[9] = s_extra01;
            s_ins[10] = s_total;
            rpt_dt.Rows.Add(s_ins);

            //加工友小計
            DataRow v_ins = rpt_dt.NewRow();
            v_ins[0] = "工友小計";
            v_ins[1] = v_people_acount;
            v_ins[2] = v_salarypay1;
            v_ins[3] = v_boss;
            v_ins[4] = v_profess;
            v_ins[5] = v_workp_technics;
            v_ins[6] = v_far;
            v_ins[5] = v_ssum;
            v_ins[6] = v_tax;
            v_ins[7] = v_extra01;
            v_ins[8] = v_total;
            rpt_dt.Rows.Add(v_ins);

            //加員警小計
            DataRow r_ins = rpt_dt.NewRow();
            r_ins[0] = "員警小計";
            r_ins[1] = s_people_acount;
            r_ins[2] = s_salarypay1;
            r_ins[3] = s_boss;
            r_ins[4] = s_profess;
            r_ins[5] = s_workp_technics;
            r_ins[6] = s_far;
            r_ins[7] = s_ssum;
            r_ins[8] = s_tax;
            r_ins[9] = s_extra01;
            r_ins[10] = s_total;
            rpt_dt.Rows.Add(r_ins);

            //加合計員工及總計員工
            DataRow u_ins = rpt_dt.NewRow();
            u_ins[0] = "合計員工";
            u_ins[1] = s_people_acount + v_people_acount;
            u_ins[2] = s_salarypay1 + v_salarypay1;
            u_ins[3] = s_boss + v_boss;
            u_ins[4] = s_profess + v_profess;
            u_ins[5] = s_workp_technics + v_workp_technics;
            u_ins[6] = s_far + v_far;
            u_ins[7] = s_ssum + v_ssum;
            u_ins[8] = s_tax + v_tax;
            u_ins[9] = s_extra01 + v_extra01;
            u_ins[10] = s_total + v_total;
            rpt_dt.Rows.Add(u_ins);
            DataRow k_ins = rpt_dt.NewRow();
            k_ins[0] = "總計員工";
            k_ins[1] = s_people_acount + v_people_acount;
            k_ins[2] = s_salarypay1 + v_salarypay1;
            k_ins[3] = s_boss + v_boss;
            k_ins[4] = s_profess + v_profess;
            k_ins[5] = s_workp_technics + v_workp_technics;
            k_ins[6] = s_far + v_far;
            k_ins[7] = s_ssum + v_ssum;
            k_ins[8] = s_tax + v_tax;
            k_ins[9] = s_extra01 + v_extra01;
            k_ins[10] = s_total + v_total;
            rpt_dt.Rows.Add(k_ins);
             
             
             */

            #endregion

            string strSQL = string.Format("SELECT * FROM VW_EFFECT_LIST WHERE PAY_AD = '{0}' AND AYEAR = '{1}' ORDER BY MZ_UNIT", strAD, strAYEAR);

            Session["rpt_dt"] = o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW");
            Session["TITLE"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04") + strAYEAR + "年";
            Session["TITLE1"] = "";
            string tmp_url = "B_rpt.aspx?fn=effect_list&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

    

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_effectlist_rpt.aspx");
        }
    }
}
