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
    public partial class B_SalaryEFFECT2 : System.Web.UI.Page
    {
        decimal bossmoney = 0;
        int currentIndex
        {
            get { return (int)ViewState["currentIndex"]; }
            set { ViewState["currentIndex"] = value; }
        }
        Police police
        {
            
            get { return (Police)ViewState["police"]; }
            set
            {
                ViewState["police"] = value;
                if (police == null)
                    return;

                currentEffect = null;

                btn_calculate.Enabled = true;
                btn_create.Enabled = true;
                btBack.Enabled = false;
                btDelete.Enabled = false;
                btNext.Enabled = false;
                btUpdate.Enabled = false;
                btn_fastMenu.Enabled = false;

                txt_polno.Text = police.polno;
                txt_idno.Text = police.id;
                txt_name.Text = police.name;
                lb_srank.Text = police.srankName;
                lb_spt.Text = police.spt;
                lb_occc.Text = police.occcName;
                lb_payad.Text = police.payadName;
                lb_unit.Text = police.unitName;

                //20140606
                lb_MZ_GRADE.Text = police.grade;

                txt_pay.Text = "0.5";// 預設0.5個月

                //2013/01/24 有任主管的話預設12個月，否則0個月
                if (police.pb2 > 0)
                    txt_bossAmonth.Text = police.pb2.ToString();
                else
                {
                    if (police.boss > 0)
                        txt_bossAmonth.Text = "12";
                    else
                        txt_bossAmonth.Text = "0";
                }

                double pay = double.Parse(txt_pay.Text);
                int bossAmonth = int.Parse(txt_bossAmonth.Text);

                txt_salary.Text = Salary.round((police.salary * pay)).ToString();
                txt_profess.Text = Salary.round((police.profess * pay)).ToString();
                txt_boss.Text = Salary.round((police.boss * pay * bossAmonth / 12.00)).ToString();
                txt_work.Text = Salary.round((police.workp * pay)).ToString();
                txt_tech.Text = Salary.round((police.technic * pay)).ToString();
                txt_far.Text = Salary.round((police.far * pay)).ToString();

                //20150121
                txt_electric.Text = Salary.round((police.electric * pay)).ToString();


                txt_extra01.Text = police.extra01.ToString();

                //2013/01/24 小隊長 法院扣款*發給月數
                if (int.Parse(txt_extra01.Text) > 0)
                {
                    txt_extra01.Text = Salary.round(Double.Parse(((int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text) + int.Parse(txt_work.Text) + int.Parse(txt_tech.Text) + int.Parse(txt_far.Text) + int.Parse(txt_electric.Text)) / 3.0).ToString())).ToString();
                }
                calculate();
            }
        }
        Effect currentEffect
        {
            get { return (Effect)ViewState["currentEffect"]; }
            set { ViewState["currentEffect"] = value; }
        }
        DataTable searchResult
        {
            get { return (DataTable)ViewState["searchResult"]; }
            set { ViewState["searchResult"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!Page.IsPostBack)
            {  SalaryPublic.checkPermission();
                //20140526
                txt_salary.Enabled = false;
                txt_profess.Enabled = false;
              
               
                txt_tech.Enabled = true;
                txt_far.Enabled = true;
                txt_extra01.Enabled = false;
                txt_tax.Enabled = false;
                
                //20140526

                

                txt_year.Text = (DateTime.Now.Year - 1912).ToString().PadLeft(3, '0');
                SalaryPublic.fillDropDownList(ref ddl_searchPayad);
                SalaryPublic.fillUnitDropDownList(ref ddl_searchUnit, ddl_searchPayad.SelectedValue);
                txt_searchAyear.Text = (DateTime.Now.Year - 1912).ToString().PadLeft(3, '0');

                btn_calculate.Enabled = false;
               
                btBack.Enabled = false;
                btDelete.Enabled = false;
                btNext.Enabled = false;
                btn_fastMenu.Enabled = false;
                
                
                //20150128 開放只能修改 主管加給.警勤加給.繁重加給
                btUpdate.Enabled = false;
                txt_boss.Enabled = true;
                txt_work.Enabled = true;
                txt_electric.Enabled = true;

                //btn_create.Enabled = false;

                //if (System.Configuration.ConfigurationManager.AppSettings["ip"].ToString() == "154")
                //{
                    
                //    txt_boss.Enabled = true;
                   

                //}
                btUpdate.Enabled = true;
                txt_work.Enabled = true;
                txt_electric.Enabled = true;
            }
        }

        protected void bt_search_Click(object sender, EventArgs e)
        {
            btn_showSearch_ModalPopupExtender.Show();
        }

        protected void ddl_searchPayad_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_showSearch_ModalPopupExtender.Show();

            SalaryPublic.fillUnitDropDownList(ref ddl_searchUnit, ddl_searchPayad.SelectedValue);
        }

        protected void btn_searchConfirm_Click(object sender, EventArgs e)
        {
            string sql = "SELECT E_SNID, PAY_UNIT, MZ_POLNO, NAME,LOCKDB FROM VW_ALL_EFFECT_DATA WHERE PAY_AD=@PAY_AD";
            List<SqlParameter> ops = new List<SqlParameter>();

            ops.Add(new SqlParameter("PAY_AD", ddl_searchPayad.SelectedValue));
            if (txt_searchAyear.Text.Length > 0)
            {
                sql += " AND AYEAR=@AYEAR";
                ops.Add(new SqlParameter("AYEAR", txt_searchAyear.Text));
            }
            if (!string.IsNullOrEmpty(txt_searchPolno.Text))
            {
                sql += " AND MZ_POLNO=@MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", txt_searchPolno.Text));
            }
            if (ddl_searchUnit.SelectedIndex > 0)
            {
                sql += " AND MZ_UNIT=@MZ_UNIT";
                ops.Add(new SqlParameter("MZ_UNIT", ddl_searchUnit.SelectedValue));
            }
            if (!string.IsNullOrEmpty(txt_searchName.Text))
            {
                sql += " AND NAME like @MZ_NAME";
                ops.Add(new SqlParameter("MZ_NAME", string.Format("%{0}%", txt_searchName.Text)));
            }
            if (!string.IsNullOrEmpty(txt_searchIDCARD.Text))
            {
                sql += " AND IDNO=@MZ_ID";
                ops.Add(new SqlParameter("MZ_ID", txt_searchIDCARD.Text));
            }

            sql += " ORDER BY AYEAR, MZ_POLNO";

            searchResult = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            gv_searchResult.DataSource = searchResult;
            gv_searchResult.DataBind();

            if (searchResult.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無資料')", true);
                return;
            }
            btn_create.Enabled = true;
            btBack.Enabled = true;
            btDelete.Enabled = true;
            btNext.Enabled = true;
            btUpdate.Enabled = true;
            btn_fastMenu.Enabled = true;
            switchTo(0);
        }

        void switchTo(int index)
        {
            currentIndex = index;
            currentEffect = new Effect(int.Parse(searchResult.Rows[currentIndex]["E_SNID"].ToString()));
            //開啟計算按鈕
            btn_calculate.Enabled = true;
            btBack.Enabled = false;
            btNext.Enabled = false;
            if (searchResult.Rows.Count > 1)
            {
                // 不在第一筆的時候
                if (currentIndex != 0)
                    btBack.Enabled = true;

                // 不在最後一筆的時候
                if (currentIndex != searchResult.Rows.Count - 1)
                    btNext.Enabled = true;
            }

            txt_year.Text = currentEffect.ayear.ToString();
            txt_idno.Text = currentEffect.idno;
            txt_name.Text = currentEffect.name;
            lb_occc.Text = currentEffect.occcName;
            lb_payad.Text = currentEffect.payadName;
            txt_polno.Text = currentEffect.polno;
            lb_spt.Text = currentEffect.spt;
            lb_srank.Text = currentEffect.srankName;
            lb_unit.Text = currentEffect.unitName;
            txt_boss.Text = currentEffect.boss.ToString();
            txt_bossAmonth.Text = currentEffect.bossAmonth.ToString();
            txt_extra01.Text = currentEffect.extra01.ToString();
            txt_pay.Text = currentEffect.pay.ToString();
            txt_profess.Text = currentEffect.profess.ToString();
            txt_salary.Text = currentEffect.salary.ToString();
            txt_work.Text = currentEffect.work.ToString();
            txt_tech.Text = currentEffect.tech.ToString();
            txt_far.Text = currentEffect.far.ToString();

            //20150121
            txt_electric.Text = currentEffect.electric.ToString();


            txt_tax.Text = currentEffect.tax.ToString();
            txt_total.Text = currentEffect.total.ToString();
            txt_des.Text = (currentEffect.extra01 + currentEffect.tax).ToString();
            txt_net.Text = (currentEffect.total - (currentEffect.extra01 + currentEffect.tax)).ToString();
            //2013/01/31 備註無法儲存
            txt_note.Text = currentEffect.note;

            //20140606
            lb_MZ_GRADE.Text = currentEffect.grade;

            //20150327
            if (currentEffect.lockdb == "Y")
            {
                btn_create.Enabled = false;
                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                lb_LockDB.Visible = true;
            }
            else
            {
                btn_create.Enabled = true;
                btDelete.Enabled = true;
                btUpdate.Enabled = true;
                lb_LockDB.Visible = false;
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            //儲存前計算
            calculate();

            //2013/01/31 備註無法儲存
            if (currentEffect.update(float.Parse(txt_pay.Text), int.Parse(txt_bossAmonth.Text), int.Parse(txt_salary.Text), int.Parse(txt_boss.Text), int.Parse(txt_profess.Text), int.Parse(txt_work.Text), int.Parse(txt_tech.Text), int.Parse(txt_far.Text), int.Parse( txt_electric.Text), int.Parse(txt_extra01.Text), int.Parse(txt_tax.Text), int.Parse(txt_total.Text), txt_note.Text))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料修改成功');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料修改失敗，請再試一次');", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            currentEffect.delete();

            searchResult.Rows.RemoveAt(currentIndex);

            if (searchResult.Rows.Count > 0)
            {
                if (searchResult.Rows.Count <= currentIndex)
                    switchTo(searchResult.Rows.Count - 1);
                else
                    switchTo(currentIndex);
            }
            else
            {
                btn_calculate.Enabled = false;
                btn_create.Enabled = false;
                btBack.Enabled = false;
                btDelete.Enabled = false;
                btNext.Enabled = false;
                btUpdate.Enabled = false;
                btn_fastMenu.Enabled = false;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料刪除成功')", true);
        }

        protected void btBack_Click(object sender, EventArgs e)
        {
            switchTo(currentIndex - 1);
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            switchTo(currentIndex + 1);
        }

        #region 快速選單

        protected void gv_searchResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                string sn = e.CommandArgument.ToString();
                switchTo(searchResult.Rows.IndexOf(searchResult.Select("e_snid='" + sn + "'").First()));
            }
        }

        protected void gv_searchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btn_showfastMenu_ModalPopupExtender.Show();
            gv_searchResult.PageIndex = e.NewPageIndex;
            gv_searchResult.DataSource = searchResult;
            gv_searchResult.DataBind();
        }

        protected void btn_fastMenu_Click(object sender, EventArgs e)
        {
            btn_showfastMenu_ModalPopupExtender.Show();
        }

        #endregion

        #region 手動建立考績獎金功能

        // 新增
        protected void btn_create_Click(object sender, EventArgs e)
        {
            if (currentEffect != null)
            {
                police = new Police(currentEffect.idno);
            }

            // 先確定沒有該年的考績獎金資料才可以新增
            if (new Effect(txt_year.Text, police.id).sn > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('此人在" + txt_year.Text + "年度已有考績獎金資料，無法再新增');", true);
                police = null;
                txt_idno.Text = string.Empty;
                txt_name.Text = string.Empty;
                lb_occc.Text = string.Empty;
                lb_payad.Text = string.Empty;
                txt_polno.Text = string.Empty;
                lb_spt.Text = string.Empty;
                lb_srank.Text = string.Empty;
                lb_unit.Text = string.Empty;
                txt_boss.Text = string.Empty;
                txt_bossAmonth.Text = string.Empty;
                txt_extra01.Text = string.Empty;
                txt_pay.Text = string.Empty;
                txt_profess.Text = string.Empty;
                txt_salary.Text = string.Empty;
                txt_work.Text = string.Empty;
                txt_tech.Text = string.Empty;
                txt_far.Text = string.Empty;

                //20140121

                txt_electric.Text = string.Empty;

                txt_tax.Text = string.Empty;
                txt_total.Text = string.Empty;
                txt_des.Text = string.Empty;
                txt_net.Text = string.Empty;
                //2013/01/31 備註無法儲存
                txt_note.Text = string.Empty;

                //20140606
                lb_MZ_GRADE.Text = string.Empty;

                return;
            }

            double pay;
            int bossAmonth;
            int salary;
            int profess;
            int boss;
            int work;
            int tech;
            int far;
            //20150121
            int electric;
            int extra01;
            int tax;
            int total;

            double.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_bossAmonth.Text, out bossAmonth);
            int.TryParse(txt_salary.Text, out salary);
            int.TryParse(txt_profess.Text, out profess);
            int.TryParse(txt_boss.Text, out boss);
            int.TryParse(txt_work.Text, out work);
            int.TryParse(txt_tech.Text, out tech);
            int.TryParse(txt_far.Text, out far);
            //20150121
            int.TryParse( txt_electric.Text, out electric);

            int.TryParse(txt_extra01.Text, out extra01);
            int.TryParse(txt_tax.Text, out tax);
            int.TryParse(txt_total.Text, out total);

            string grade = Effect.getGrade(police.id);

            if (!police.createEffect(txt_year.Text, SalaryPublic.strLoginEXAD, pay, salary, profess, boss, work, tech, far, electric,bossAmonth, extra01, tax, total, txt_note.Text, grade))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料新增失敗');", true);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料新增成功');", true);
        }

        protected void txt_polno_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = Police.searchByPolno(txt_polno.Text);

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無此人');", true);
                return;
            }
            if (dt.Rows.Count == 1)
            {
                police = new Police(dt.Rows[0]["IDCARD"].ToString());
                return;
            }

            btn_showSelector_ModalPopupExtender.Show();
            UserSelector1.SetData(dt);
        }

        protected void txt_idno_TextChanged(object sender, EventArgs e)
        {
            Police police = new Police(txt_idno.Text);

            if (police.id == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無此人');", true);
                return;
            }
            
            this.police = police;
        }

        protected void txt_name_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = Police.searchByName(txt_name.Text);

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無此人');", true);
                return;
            }
            if (dt.Rows.Count == 1)
            {
                police = new Police(dt.Rows[0]["IDCARD"].ToString());
                return;
            }

            btn_showSelector_ModalPopupExtender.Show();
            UserSelector1.SetData(dt);
        }

        protected void gv_selector_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            police = new Police(e.CommandArgument.ToString());
        }

        // 計算考績獎金金額
        protected void btn_calculate_Click(object sender, EventArgs e)
        {
            double pay;
            int bossAmonth;
            double.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_bossAmonth.Text, out bossAmonth);
            ////20150128 小隊長說計算只計算現場更改的內容,不需再到B_BASE重新撈.要的話就請他自己回產生頁面重新產生
            //新增
            if (police != null)
            {
                txt_salary.Text = Salary.round((police.salary * pay)).ToString();
                txt_profess.Text = Salary.round((police.profess * pay)).ToString();
                txt_boss.Text = Salary.round((police.boss * pay * bossAmonth / 12.00)).ToString();
                txt_work.Text = Salary.round((police.workp * pay)).ToString();
                txt_tech.Text = Salary.round((police.technic * pay)).ToString();
                txt_far.Text = Salary.round((police.far * pay)).ToString();
                //20150121
                txt_electric.Text = Salary.round((police.electric * pay)).ToString();

                txt_extra01.Text = police.extra01.ToString();
            }
            //修改
            else if (police == null)
            {
                Police policetemp = new Police(currentEffect.idno);
                txt_salary.Text = Salary.round((policetemp.salary * pay)).ToString();
                txt_profess.Text = Salary.round((policetemp.profess * pay)).ToString();
                txt_boss.Text = Salary.round((policetemp.boss * pay * bossAmonth / 12.00)).ToString();
                txt_work.Text = Salary.round((policetemp.workp * pay)).ToString();
                txt_tech.Text = Salary.round((policetemp.technic * pay)).ToString();
                txt_far.Text = Salary.round((policetemp.far * pay)).ToString();
                //20150121
                txt_electric.Text = Salary.round((policetemp.electric * pay)).ToString();

                txt_extra01.Text = policetemp.extra01.ToString();
                policetemp = null;
            }
            calculate();
        }

        // 計算考績獎金金額
        void calculate()
        {

            int total;
            int des;

            double pay;
            int bossAmonth;
            double.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_bossAmonth.Text, out bossAmonth);

            int boss = 0;
            int.TryParse(txt_boss.Text, out boss);
          

            //計算
            //20150121
            total = int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text)
                    + int.Parse(txt_work.Text) + int.Parse(txt_tech.Text) + int.Parse(txt_far.Text) + int.Parse(txt_electric.Text);
            txt_total.Text = total.ToString();

            //2013/01/23小隊長：扣除最低扣稅額
            if ((total - boss) >= Tax.getTaxStart())
                txt_tax.Text = Salary.round((total - boss) * Tax.getTaxPercent()).ToString();
            else
                txt_tax.Text = "0";

            des = int.Parse(txt_extra01.Text) + int.Parse(txt_tax.Text);
            txt_des.Text = des.ToString();

            txt_net.Text = (total - des).ToString();
        }

        #endregion

        protected void txt_bossAmonth_TextChanged(object sender, EventArgs e)
        {
            string tmpdata = txt_bossAmonth.Text;
            if (tmpdata != "")
            {
                Police police = new Police(txt_idno.Text);

                if (police.id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無此人');", true);
                    return;
                }
                //取出主管加給
                string sql = "select PAY from B_Boss where ID = '" + police.srank + police.slvc + "'";
                SqlConnection oconn = new SqlConnection();
                oconn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ToString();
                oconn.Open();
                SqlCommand ocmd = new SqlCommand();
                ocmd.Connection = oconn;
                ocmd.CommandText = sql;
                try
                {
                    bossmoney = decimal.Parse("0" + ocmd.ExecuteScalar().ToString());
                    //bossmoney = decimal.Parse(data1);
                }
                catch (Exception ex)
                {
                    string aa = ex.Message.ToString();
                }
                oconn.Close();
                oconn.Dispose();
                txt_boss.Text = MathHelper.Round(bossmoney * decimal.Parse(tmpdata) * decimal.Parse(txt_pay.Text) / 12 + decimal.Parse("0.001"), 0).ToString();
            }
        }
    }
}
