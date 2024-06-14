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
    public partial class B_SalaryYear_Endbonus2 : System.Web.UI.Page
    {
        int? tempAmonth
        {
            get { return (int)ViewState["tempAmonth"]; }
            set { ViewState["tempAmonth"] = value; }
        }
        int? tempBossamonth
        {
            get { return (int)ViewState["tempBossamonth"]; }
            set { ViewState["tempBossamonth"] = value; }
        }
        double? tempPay
        {
            get { return (double)ViewState["tempPay"]; }
            set { ViewState["tempPay"] = value; }
        }

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

                currentYearend = null;

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
                txt_pay.Text = "1.5";// 預設1.5個月
                txt_amonth.Text = "12";// 預設12個月

                // 有任主管的話預設12個月，否則0個月
                if (police.pb2 > 0)
                    txt_bossAmonth.Text = "12";
                else
                {
                    //2013/01/24 有任主管的話預設12個月，否則0個月
                    if (police.boss > 0)
                        txt_bossAmonth.Text = "12";
                    else
                        txt_bossAmonth.Text = "0";
                }

                DropDownList_REDUCE.SelectedValue = "3";// 預設無減發
                //2012/12/14 儲存預設資訊

                tempPay = double.Parse(txt_pay.Text);
                tempAmonth = int.Parse(txt_amonth.Text);
                tempBossamonth = int.Parse(txt_bossAmonth.Text);

                //新增的狀況
                txt_salary.Text = Salary.round((double)(Salary.round((double)(police.salary * tempAmonth / 12.00 * tempPay)))).ToString();
                txt_profess.Text = Salary.round((double)(Salary.round((double)(police.profess * tempAmonth / 12.00 * tempPay)))).ToString();
                txt_boss.Text = Salary.round((double)(Salary.round((double)(police.boss * tempBossamonth / 12.00 * tempPay)))).ToString();

                txt_extra01.Text = police.extra01.ToString();

                //2013/01/22 小隊長 法院扣款*發給月數
                if (int.Parse(txt_extra01.Text) > 0)
                {
                    txt_extra01.Text = Salary.round(Double.Parse(((int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text)) / 3.0).ToString())).ToString();
                }
                calculate();
            }
        }
        YearEnd currentYearend
        {
            get { return (YearEnd)ViewState["currentYearend"]; }
            set { ViewState["currentYearend"] = value; }
        }
        DataTable searchResult
        {
            get { return (DataTable)ViewState["searchResult"]; }
            set { ViewState["searchResult"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!Page.IsPostBack)
            {
                SalaryPublic.checkPermission();

                txt_year.Text = (DateTime.Now.Year - 1912).ToString().PadLeft(3, '0');
                SalaryPublic.fillDropDownList(ref ddl_searchPayad);
                SalaryPublic.fillUnitDropDownList(ref ddl_searchUnit, ddl_searchPayad.SelectedValue);
                txt_searchAyear.Text = (DateTime.Now.Year - 1912).ToString().PadLeft(3, '0');

                btn_calculate.Enabled = false;
               
                btBack.Enabled = false;
                btDelete.Enabled = false;
                btNext.Enabled = false;
               
                btn_fastMenu.Enabled = false;

                //20150128 開放只能修改 主管加給.
                

                //btn_create.Enabled = false;

               
                    txt_boss.Enabled = true;
                    btUpdate.Enabled = true;

                //20191216 暫時解除
                if (txt_year.Text != "107")
                {
                    txt_salary.Enabled = false;
                    txt_profess.Enabled = false;
                    txt_extra01.Enabled = false;
                    txt_tax.Enabled = false;
                }

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
            string sql = "SELECT Y_SNID, PAY_UNIT, MZ_POLNO, NAME , LOCKDB FROM VW_ALL_YEARPAY_DATA WHERE PAY_AD=@PAY_AD";
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
            //查詢 姓名(模糊搜尋)
            if (!string.IsNullOrEmpty(txt_searchName.Text))
            {
                sql += " AND NAME like @MZ_NAME";
                ops.Add(new SqlParameter("MZ_NAME", string.Format("%{0}%", txt_searchName.Text)));
            }
            //查詢IDCard
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

            btn_calculate.Enabled = true;
            btn_create.Enabled = true;
            btBack.Enabled = true;
            btDelete.Enabled = true;
            btNext.Enabled = true;
            btUpdate.Enabled = true;
            btn_fastMenu.Enabled = true;

            

            switchTo(0);
        }

        //選擇查詢結果
        void switchTo(int index)
        {
            currentIndex = index;

            currentYearend = new YearEnd(int.Parse(searchResult.Rows[currentIndex]["Y_SNID"].ToString()));

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
            txt_year.Text = currentYearend.ayear.ToString();
            txt_idno.Text = currentYearend.idno;
            txt_name.Text = currentYearend.name;
            lb_occc.Text = currentYearend.occcName;
            lb_payad.Text = currentYearend.payadName;
            txt_polno.Text = currentYearend.polno;
            lb_spt.Text = currentYearend.spt;
            lb_srank.Text = currentYearend.srankName;
            lb_unit.Text = currentYearend.unitName;
            txt_amonth.Text = currentYearend.amonth.ToString();
            txt_boss.Text = currentYearend.boss.ToString();
            txt_bossAmonth.Text = currentYearend.bossAmonth.ToString();
            txt_extra01.Text = currentYearend.extra01.ToString();
            txt_pay.Text = currentYearend.pay.ToString();
            txt_profess.Text = currentYearend.profess.ToString();
            txt_salary.Text = currentYearend.salary.ToString();


            //20140114 
            //txt_tax.Text = currentYearend.tax.ToString();
            
            //txt_total.Text = (currentYearend.salary + currentYearend.profess + currentYearend.boss).ToString();
            //txt_des.Text = (currentYearend.extra01 + currentYearend.tax).ToString();
            ////2013/01/22年終
            //txt_net.Text = (currentYearend.salary + currentYearend.profess + currentYearend.boss - currentYearend.tax - currentYearend.extra01).ToString();
            ////txt_net.Text = currentYearend.total.ToString();

            //txt_reduce.Text = currentYearend.reduce + "/3";
            //20140114

            txt_note.Text = currentYearend.note;
            DropDownList_REDUCE.SelectedValue = currentYearend.reduce.ToString();
            //儲存資訊用來判斷需不需用重新計算
            tempPay = double.Parse(txt_pay.Text);
            tempAmonth = int.Parse(txt_amonth.Text);
            tempBossamonth = int.Parse(txt_bossAmonth.Text);
            //20140114
            calculate();
            //20140114
            //20150327
            if (currentYearend.lockdb == "Y")
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
                btUpdate.Enabled =true;
                lb_LockDB.Visible = false;
            }

            //20191216 暫時解除
            if (txt_year.Text != "107")
            {
                txt_salary.Enabled = false;
                txt_profess.Enabled = false;
                txt_extra01.Enabled = false;
                txt_tax.Enabled = false;
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            calculate();
            //2013/01/24 確保報表金額無誤
            string sql = "update b_yearpay set total = (salarypay1+boss+profess)  where (salarypay1+boss+profess) <> total";
            o_DBFactory.ABC_toTest.Edit_Data(sql);
            //2013/01/22 txt_net 改成 txt_total
            if (currentYearend.update(float.Parse(txt_pay.Text), int.Parse(txt_amonth.Text), int.Parse(txt_bossAmonth.Text), int.Parse(DropDownList_REDUCE.SelectedValue), int.Parse(txt_salary.Text), int.Parse(txt_boss.Text), int.Parse(txt_profess.Text), int.Parse(txt_extra01.Text), int.Parse(txt_tax.Text), int.Parse(txt_total.Text), txt_note.Text))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料修改成功');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料修改失敗，請再試一次');", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            currentYearend.delete();

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
                tempPay = null;
                tempAmonth = null;
                tempBossamonth = null;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料刪除成功');", true);
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
                switchTo(searchResult.Rows.IndexOf(searchResult.Select("y_snid='" + sn + "'").First()));
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

        #region 手動建立年終獎金功能

        // 新增
        protected void btn_create_Click(object sender, EventArgs e)
        {
            //2012/12/13   新增此功能查詢完可點新增
            if (currentYearend != null)
                police = new Police(currentYearend.idno);
            // 先確定沒有該年的年終獎金資料才可以新增
            if (new YearEnd(txt_year.Text, police.id).sn > 0)
            {
                police = null;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('此人在" + txt_year.Text + "年度已有年終獎金資料，無法再新增');", true);
                txt_polno.Text = string.Empty;
                txt_idno.Text = string.Empty;
                txt_name.Text = string.Empty;
                lb_srank.Text = string.Empty;
                lb_spt.Text = string.Empty;
                lb_occc.Text = string.Empty;
                lb_payad.Text = string.Empty;
                lb_unit.Text = string.Empty;
                txt_pay.Text = string.Empty;
                txt_amonth.Text = string.Empty;
                txt_bossAmonth.Text = string.Empty;

                txt_salary.Text = string.Empty;
                txt_profess.Text = string.Empty;
                txt_boss.Text = string.Empty;
                txt_extra01.Text = string.Empty;
                txt_tax.Text = string.Empty;
                txt_note.Text = string.Empty;

                txt_total.Text = string.Empty;
                txt_des.Text = string.Empty;
                txt_reduce.Text = string.Empty;
                txt_net.Text = string.Empty;

                tempPay = null;
                tempAmonth = null;
                tempBossamonth = null;
                txt_idno.Focus();
                return;
            }

            double pay;
            int amonth;
            int bossAmonth;
            int salary;
            int profess;
            int boss;
            int extra01;
            int tax;
            int total;

            double.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_amonth.Text, out amonth);
            int.TryParse(txt_bossAmonth.Text, out bossAmonth);
            int.TryParse(txt_salary.Text, out salary);
            int.TryParse(txt_profess.Text, out profess);
            int.TryParse(txt_boss.Text, out boss);

            int.TryParse(txt_extra01.Text, out extra01);
            int.TryParse(txt_tax.Text, out tax);
            int.TryParse(txt_total.Text, out total);



            if (!police.createYearEnd(txt_year.Text, SalaryPublic.strLoginEXAD, pay, amonth, salary, profess, boss, bossAmonth, extra01, tax, total, txt_note.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料新增失敗');", true);

                return;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料新增成功');", true);
            police = null;
            tempPay = null;
            tempAmonth = null;
            tempBossamonth = null;
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
            gv_selector.DataSource = dt;
            gv_selector.DataBind();
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
            gv_selector.DataSource = dt;
            gv_selector.DataBind();
        }

        protected void gv_selector_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            police = new Police(e.CommandArgument.ToString());
        }

        // 計算年終獎金金額
        protected void btn_calculate_Click(object sender, EventArgs e)
        {

            //發給月數
            double pay;
            //在職月數
            int amonth;
            //任主管月數
            int bossAmonth;
            double.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_amonth.Text, out amonth);
            int.TryParse(txt_bossAmonth.Text, out bossAmonth);

            ////20150128 小隊長說計算只計算現場更改的內容,不需再到B_BASE重新撈.要的話就請他自己回產生頁面重新產生
            ////新增的狀況
            if (police != null)
            {
                txt_salary.Text = Salary.round(Salary.round(police.salary * amonth / 12.00 * pay)).ToString();
                txt_profess.Text = Salary.round(Salary.round(police.profess * amonth / 12.00 * pay)).ToString();
                txt_boss.Text = Salary.round(Salary.round(police.boss * bossAmonth / 12.00 * pay)).ToString();

                txt_extra01.Text = police.extra01.ToString();
                //2013/01/22 小隊長 法院扣款*發給月數
                if (int.Parse(txt_extra01.Text) > 0)
                {
                    txt_extra01.Text = Salary.round(Double.Parse(((int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text)) / 3.0).ToString())).ToString();
                }
            }
            //發給月數、在職月數、任主管月數 有修改的狀況
            else if (police == null)
            {
                Police policetemp = new Police(currentYearend.idno);
                txt_salary.Text = Salary.round((Salary.round(policetemp.salary * amonth / 12.00 * pay))).ToString();
                txt_profess.Text = Salary.round(Salary.round(policetemp.profess * amonth / 12.00 * pay)).ToString();
                txt_boss.Text = Salary.round(Salary.round(policetemp.boss * bossAmonth / 12.00 * pay)).ToString();

                txt_extra01.Text = policetemp.extra01.ToString();
                //2013/01/22 小隊長 法院扣款*發給月數
                if (int.Parse(txt_extra01.Text) > 0)
                {
                    txt_extra01.Text = Salary.round(Double.Parse(((int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text)) / 3.0).ToString())).ToString();
                }
                tempPay = pay;
                tempAmonth = amonth;
                tempBossamonth = bossAmonth;
                policetemp = null;
            }
            calculate();
        }

       
        //修正版
        void calculate()
        {
            int total;
            int des;
            //發給月數
            double pay;
            //在職月數
            int amonth;
            //任主管月數
            int bossAmonth;
            double.TryParse(txt_pay.Text, out pay);
            int.TryParse(txt_amonth.Text, out amonth);
            int.TryParse(txt_bossAmonth.Text, out bossAmonth);

            total = int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text);
            txt_total.Text = total.ToString(); //應發金額

            double temp1 = int.Parse(txt_salary.Text);
            double temp2 = int.Parse(txt_profess.Text);
            double temp3 = int.Parse(txt_boss.Text);
            string reduce = DropDownList_REDUCE.SelectedValue;
            if (reduce == "0")
            {
                temp1 = 0;
                temp2 = 0;
                temp3 = 0;
            }
            else if(reduce=="1")
            {
                temp1 = int.Parse(txt_salary.Text) * 0.66;
                temp2 = int.Parse(txt_profess.Text) * 0.66;
                temp3 = int.Parse(txt_boss.Text) * 0.66;
            }
            else if (reduce == "2")
            {
                temp1 = int.Parse(txt_salary.Text) * 0.33;
                temp2 = int.Parse(txt_profess.Text) * 0.33;
                temp3 = int.Parse(txt_boss.Text) * 0.33;
            }

            double temp = temp1 + temp2 + temp3;


            string s_temp = Convert.ToString(Convert.ToInt64(temp));
            txt_total.Text = s_temp; //應發金額
            txt_total.Text = Convert.ToString(int.Parse(txt_salary.Text) + int.Parse(txt_profess.Text) + int.Parse(txt_boss.Text)); //應發金額

            //如果大於需繳稅的底限
            //2013/01/23小隊長：扣除最低扣稅額
            if ((total - temp3) >= Tax.getTaxStart())
            {
                //sam.hsu 20201208 
                txt_tax.Text = Salary.round((total - temp3) * Tax.getTaxPercent()).ToString();
            }
            else
            {
                txt_tax.Text = "0";
            }

            //2018.1.22 by andy: 用新的金額算所得稅
            if ((temp - temp3) >= Tax.getTaxStart())
            {
                //sam.hsu 20201208 
                txt_tax.Text = Salary.round((temp - temp3) * Tax.getTaxPercent()).ToString();
            }
            else
            {
                txt_tax.Text = "0";
            }

            des = int.Parse(txt_extra01.Text) + int.Parse(txt_tax.Text);
            txt_des.Text = des.ToString();

            txt_reduce.Text = DropDownList_REDUCE.SelectedValue + "/3";

            txt_net.Text = Salary.round((double)((total - des) * int.Parse(DropDownList_REDUCE.SelectedValue) / 3)).ToString();

            double _total = int.Parse(txt_total.Text);
            if (reduce == "0")
            {
                _total = 0;
            }
            else if (reduce == "1")
            {
                _total = (_total - des) * 0.66;
            }
            else if (reduce == "2")
            {
                _total = (_total - des) * 0.33;
            }

            //2018.1.22 by andy
            //txt_net.Text = Salary.round((double)((temp - des))).ToString();
            //txt_net.Text = Salary.round((double)((_total))).ToString(); 
        }

        #endregion

        protected void txt_bossAmonth_TextChanged(object sender, EventArgs e)
        {
            string tmpdata = txt_bossAmonth.Text;
            decimal bossmoney = 0;
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

