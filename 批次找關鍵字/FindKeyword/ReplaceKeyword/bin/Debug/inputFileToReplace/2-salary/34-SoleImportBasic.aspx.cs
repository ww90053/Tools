using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TPPDDB.Helpers;

namespace TPPDDB._2_salary
{
    public partial class _4_SoleImportBasic : System.Web.UI.Page
    {
        DataTable excel
        {
            get { return (DataTable)ViewState["excel"]; }
            set { ViewState["excel"] = value; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SoleItemSelector1.itemSelected += new TPPDDB._2_salary.UserControl.SoleItemSelector.ItemSelected(SoleItemSelector1_itemSelected);
        }

        //項目選擇時的處理，帶出說明
        void SoleItemSelector1_itemSelected()
        {
            int caseid;
            int.TryParse(txt_caseid.Text, out caseid);
            txt_note.Text = Sole.getName(SoleItemSelector1.item) + DateTime.Now.ToString("yyyyMMdd") + "匯入";
            if (Sole.hasData(txt_DA.Text, caseid, SoleItemSelector1.item))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('此日期、批號、項目已有資料，若強制執行匯入可能造成資料重覆。')", true);
        }

        protected void txt_DA_TextChanged(object sender, EventArgs e)
        {
            int caseid;
            int.TryParse(txt_caseid.Text, out caseid);
            if (Sole.isLocked(txt_DA.Text, caseid))
            {
                btn_Add.Enabled = false;
                txt_caseid.BackColor = System.Drawing.Color.Orange;
            }
            else
                txt_caseid.BackColor = System.Drawing.Color.White;
            if (Sole.hasData(txt_DA.Text, caseid, SoleItemSelector1.item))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('此日期、批號、項目已有資料，若強制執行匯入可能造成資料重覆。')", true);
            txt_caseid.Focus();
        }

        protected void txt_caseid_TextChanged(object sender, EventArgs e)
        {
            int caseid;
            int.TryParse(txt_caseid.Text, out caseid);

            //2013/01/28 單一發放整批匯入時，應依「登入者單位/入帳日期/入帳案號/結案狀態」決定是否可新增
            // if (Sole.isLocked(txt_DA.Text, caseid))
            string ad = Session["ADPMZ_EXAD"].ToString();

            if (Sole.isLocked(txt_DA.Text, caseid,ad))
            {
                btn_Add.Enabled = false;
                txt_caseid.BackColor = System.Drawing.Color.Orange;
            }
            else
                txt_caseid.BackColor = System.Drawing.Color.White;
            if (Sole.hasData(txt_DA.Text, caseid, SoleItemSelector1.item))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('此日期、批號、項目已有資料，若強制執行匯入可能造成資料重覆。')", true);
        }

        protected void btn_getExcel_Click(object sender, EventArgs e)
        {
            if (!fl_import.HasFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請選擇檔案')", true);
                return;
            }

            string fileName = fl_import.FileName;
            string tempfilename = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString() + ".xls";
            string up_extension = System.IO.Path.GetExtension(fileName);// 取得上傳檔副檔名
            string savePath = "\\Files\\" + tempfilename;
            savePath = System.Web.HttpContext.Current.Server.MapPath(savePath);
            fl_import.SaveAs(savePath);

            try
            {
                excel = Excel.getDataTable(savePath, "Sheet1");
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('取得檔案內容時發生錯誤，請確定檔案未經加密')", true);
                return;
            }

            System.IO.File.Delete(savePath);

            //DataTable dtDlbase = o_DBFactory.ABC_toTest.DataSelect("SELECT PAY_AD, MZ_ID, MZ_POLNO, MZ_NAME, MZ_UNIT, AKU.MZ_KCHI CHIUNIT, AKO.MZ_KCHI MZ_OCCC, MZ_SRANK, MZ_SLVC FROM A_DLBASE JOIN A_KTYPE AKO ON MZ_OCCC=MZ_KCODE AND AKO.MZ_KTYPE='26' LEFT JOIN A_KTYPE AKU ON A_DLBASE.MZ_UNIT=AKU.MZ_KCODE AND AKU.MZ_KTYPE='25' ");
            //var query = from f in dt.AsEnumerable()
            //            join a in dtDlbase.AsEnumerable()
            //            on f.Field<string>(0) equals a.Field<string>("MZ_ID")
            //            select new
            //            {
            //                MZ_ID = a.Field<string>("MZ_ID"),
            //                MZ_POLNO = a.Field<string>("MZ_POLNO"),
            //                MZ_NAME = a.Field<string>("MZ_NAME"),
            //                CHIUNIT = a.Field<string>("CHIUNIT"),
            //                AMOUNT = f.Field<object>(2),
            //                tax = f.Field<object>(3),
            //                pay1 = f.Field<object>(4),
            //                pay2 = f.Field<object>(5),
            //                untax = f.Field<object>(6),
            //                pay3 = f.Field<object>(7),
            //                extra01 = f.Field<object>(8)
            //            };

            var query = from f in excel.AsEnumerable()
                        where f.Field<string>(0) != "" && f.Field<string>(2) != "0"
                        select new
                        {
                            MZ_ID = f.Field<string>(0),
                            MZ_NAME = f.Field<string>(1),
                            AMOUNT = f.Field<string>(2),
                            tax = f.Field<string>(3),
                            pay1 = f.Field<string>(4),
                            pay2 = f.Field<string>(5),
                            untax = f.Field<string>(6),
                            pay3 = f.Field<string>(7),
                            extra01 = f.Field<string>(8),
                            memo = f.Field<string>(10)
                        };

            lb_count.Text = "取得資料共" + query.Count() + "筆";
            if (query.Count() > 0)
            {
                data1.Visible = true;
                data2.Visible = true;
                gv_Result.Visible = true;
                excel = query.AsEnumerable().ToDataTable();
            }
            else
            {
                data1.Visible = false;
                data2.Visible = false;
                gv_Result.Visible = false;
            }

            //2013/01/22 修改分頁輸入
            gv_Result.DataSource = excel;
            gv_Result.DataBind();

            if (query.Count() >= 200)
            {
                CommonCS.ShowMessage(txt_caseid, "匯入資料大於 200 筆，若資料正確無誤，\\r\\n請按下『匯入全部資料」");
                btn_Add.Text = "匯入全部資料";
                btn_Add.OnClientClick = " return confirm('確定要匯入全部資料？')";
            }
            else
            {
                btn_Add.Text = "匯入勾選資料";
                btn_Add.OnClientClick = " return confirm('確定要匯入勾選的資料？')";
            }
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            int i = 0;
            int sum = 0;
            string msg = "";
            if (txt_DA.Text.Length == 0)
                msg += "入帳日期不可空白\\r\\n";
            try { DateTime.Parse((int.Parse(txt_DA.Text.Substring(0, 3)) + 1911) + "/" + txt_DA.Text.Substring(3, 2) + "/" + txt_DA.Text.Substring(5, 2)); }
            catch
            {
                msg += "入帳日期格式錯誤\\r\\n";
            }
            if (txt_caseid.Text.Length == 0)
                msg += "入帳案號不可空白";

            if (msg.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + msg + "')", true);
                return;
            }

            List<int> notCheck = new List<int>();
            List<int> err = new List<int>();
            DataTable dtErr = new DataTable();
            dtErr.Columns.Add("身份證字號");
            dtErr.Columns.Add("金額");
            dtErr.Columns.Add("所得稅");
            dtErr.Columns.Add("自提基金");
            dtErr.Columns.Add("健保費");
            dtErr.Columns.Add("勞保費");
            dtErr.Columns.Add("自提離職儲金");
            dtErr.Columns.Add("法院扣款");
            dtErr.Columns.Add("備註說明");

            //2013/01/22 修改分頁輸入
            if (excel.Rows.Count <= 200)
            {
                for (int j = 0; j < gv_Result.Rows.Count; j++)
                {
                    // 有勾選的資料才要匯入
                    if (!((CheckBox)gv_Result.Rows[j].Cells[0].FindControl("cb_add")).Checked)
                    {
                        notCheck.Add(j);
                        continue;
                        //沒有被勾選.跳出
                    }

                    try
                    {
                        string id = excel.Rows[j][0].ToString();
                        string payad = SalaryPublic.strLoginEXAD;
                        int pay;
                        int tax;
                        int pay1;
                        int pay2;
                        int pay3;
                        int untax;
                        int extra01;
                        string memo;
                        //沒寫金額給零
                        if (excel.Rows[j][2].ToString() == "")
                            pay = 0;
                        else
                            pay = int.Parse(excel.Rows[j][2].ToString());

                        if (excel.Rows[j][3].ToString() == "")
                            tax = 0;
                        else
                            tax = int.Parse(excel.Rows[j][3].ToString());

                        if (excel.Rows[j][4].ToString() == "")
                            pay1 = 0;
                        else
                            pay1 = int.Parse(excel.Rows[j][4].ToString());

                        if (excel.Rows[j][5].ToString() == "")
                            pay2 = 0;
                        else
                            pay2 = int.Parse(excel.Rows[j][5].ToString());

                        if (excel.Rows[j][7].ToString() == "")
                            pay3 = 0;
                        else
                            pay3 = int.Parse(excel.Rows[j][7].ToString());

                        if (excel.Rows[j][6].ToString() == "")
                            untax = 0;
                        else
                            untax = int.Parse(excel.Rows[j][6].ToString());

                        if (excel.Rows[j][8].ToString() == "")
                            extra01 = 0;
                        else
                            extra01 = int.Parse(excel.Rows[j][8].ToString());
                        // Joy 新增 備註說明選項 當備註說明空白 則給說明欄位值
                        if (string.IsNullOrEmpty(excel.Rows[j][9].ToString()))
                        {
                            memo = txt_note.Text;
                        }
                        else
                        { 
                            memo = excel.Rows[j][9].ToString();
                        }

                        Police police = new Police(id);
                        if (police.id == null)
                        {
                            //找不到員警，去找其他基本資料
                            OtherPeople op = new OtherPeople(payad, id);

                            //都找不到就當是錯誤資料
                            if (op.sn == 0)
                            {
                                err.Add(j);
                                DataRow dr = dtErr.NewRow();
                                dr["身份證字號"] = id;
                                dr["金額"] = pay;
                                dr["所得稅"] = tax;
                                dr["自提基金"] = pay1;
                                dr["健保費"] = pay2;
                                dr["勞保費"] = pay3;
                                dr["自提離職儲金"] = untax;
                                dr["法院扣款"] = extra01;
                                dr["備註說明"] = memo;
                                dtErr.Rows.Add(dr);
                                continue;
                            }
                            //Sole.insertData(op.id, op.polno, payad, op.name, null, null, null, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, true);//沒有寫入職等
                            
                            ///新增資料
                            // Joy 修改 備註選項改為excel輸入
                            Sole.insertData(op.id, op.polno, payad, op.name, null, null, null, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, memo, pay1, pay2, pay3, tax, untax, extra01, true);//沒有寫入職等
                        }
                        else
                        {
  
                            string EXCEED = Check_Secand_HealthyPay(id, pay);
                            int Second_Health_Pay = 0;

                            if (EXCEED == "Y")
                                Second_Health_Pay = Convert.ToInt32(MathHelper.Round(pay * 0.02, 0));

                            //Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01,Second_Health_Pay,"N", true);
                            ///新增資料
                            // Joy 修改 備註選項改為excel輸入
                            Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay,memo, pay1, pay2, pay3, tax, untax, extra01, Second_Health_Pay, "N", true);
                            //Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, true);
                          
                            //Check_Secand_HealthyPay(id, pay);
                        }
                        i++;
                        sum += pay;
                    }
                    catch
                    {
                        err.Add(j);
                        DataRow dr = dtErr.NewRow();
                        dr["身份證字號"] = excel.Rows[j][0].ToString();
                        dr["金額"] = excel.Rows[j][2].ToString();
                        dr["所得稅"] = excel.Rows[j][3].ToString();
                        dr["自提基金"] = excel.Rows[j][4].ToString();
                        dr["健保費"] = excel.Rows[j][5].ToString();
                        dr["勞保費"] = excel.Rows[j][7].ToString();
                        dr["自提離職儲金"] = excel.Rows[j][6].ToString();
                        dr["法院扣款"] = excel.Rows[j][8].ToString();
                        dr["備註說明"] = excel.Rows[j][9].ToString();
                        dtErr.Rows.Add(dr);
                    }
                }
            }
            else
            {
                for (int j = 0; j < excel.Rows.Count; j++)
                {
                    try
                    {
                        string id = excel.Rows[j][0].ToString();
                        string payad = SalaryPublic.strLoginEXAD;
                        int pay;
                        int tax;
                        int pay1;
                        int pay2;
                        int pay3;
                        int untax;
                        int extra01;
                        /// Joy 新增備註說明
                        /// 
                        string memo;
                        if (excel.Rows[j][2].ToString() == "")
                            pay = 0;
                        else
                            pay = int.Parse(excel.Rows[j][2].ToString());

                        if (excel.Rows[j][3].ToString() == "")
                            tax = 0;
                        else
                            tax = int.Parse(excel.Rows[j][3].ToString());

                        if (excel.Rows[j][4].ToString() == "")
                            pay1 = 0;
                        else
                            pay1 = int.Parse(excel.Rows[j][4].ToString());

                        if (excel.Rows[j][5].ToString() == "")
                            pay2 = 0;
                        else
                            pay2 = int.Parse(excel.Rows[j][5].ToString());

                        if (excel.Rows[j][7].ToString() == "")
                            pay3 = 0;
                        else
                            pay3 = int.Parse(excel.Rows[j][7].ToString());

                        if (excel.Rows[j][6].ToString() == "")
                            untax = 0;
                        else
                            untax = int.Parse(excel.Rows[j][6].ToString());

                        if (excel.Rows[j][8].ToString() == "")
                            extra01 = 0;
                        else
                            extra01 = int.Parse(excel.Rows[j][8].ToString());
                        // Joy 新增 備註說明選項 當備註說明空白 則給說明欄位值
                        if (string.IsNullOrEmpty(excel.Rows[j][9].ToString()))
                        {
                            memo = txt_note.Text;
                        }
                        else
                        {
                            memo = excel.Rows[j][9].ToString();
                        }
                        Police police = new Police(id);
                        if (police.id == null)
                        {
                            //找不到員警，去找其他基本資料
                            OtherPeople op = new OtherPeople(payad, id);

                            //都找不到就當是錯誤資料
                            if (op.sn == 0)
                            {
                                err.Add(j);
                                DataRow dr = dtErr.NewRow();
                                dr["身份證字號"] = id;
                                dr["金額"] = pay;
                                dr["所得稅"] = tax;
                                dr["自提基金"] = pay1;
                                dr["健保費"] = pay2;
                                dr["勞保費"] = pay3;
                                dr["自提離職儲金"] = untax;
                                dr["法院扣款"] = extra01;
                                dr["備註說明"] = memo;
                                dtErr.Rows.Add(dr);
                                continue;
                            }
                            /// 新增資料
                            /// Joy 修正 備註說明欄位 由excel 匯入
                            /// 
                            Sole.insertData(op.id, op.polno, payad, op.name, null, null, null, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, memo, pay1, pay2, pay3, tax, untax, extra01, true);//沒有寫入職等
                            //Sole.insertData(op.id, op.polno, payad, op.name, null, null, null, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, true);//沒有寫入職等
                        }
                        else
                        {
                            string EXCEED = Check_Secand_HealthyPay(id, pay);
                            int Second_Health_Pay = 0;

                            if (EXCEED == "Y")
                                Second_Health_Pay = Convert.ToInt32(Math.Round(pay * 0.02, 0, MidpointRounding.AwayFromZero));

                            /// 備註說明
                            /// Joy 修正 備註說明欄位 由excel 匯入
                            Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, memo, pay1, pay2, pay3, tax, untax, extra01, Second_Health_Pay, "N", true);
                            //Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, Second_Health_Pay, "N", true);


                            //Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, true);

                            //Check_Secand_HealthyPay(id, pay);
                        }
                                              
                        
                        i++;
                        sum += pay;
                    }
                    catch
                    {
                        err.Add(j);
                        DataRow dr = dtErr.NewRow();
                        dr["身份證字號"] = excel.Rows[j][0].ToString();
                        dr["金額"] = excel.Rows[j][2].ToString();
                        dr["所得稅"] = excel.Rows[j][3].ToString();
                        dr["自提基金"] = excel.Rows[j][4].ToString();
                        dr["健保費"] = excel.Rows[j][5].ToString();
                        dr["勞保費"] = excel.Rows[j][7].ToString();
                        dr["自提離職儲金"] = excel.Rows[j][6].ToString();
                        dr["法院扣款"] = excel.Rows[j][8].ToString();
                        dr["備註說明"] = excel.Rows[j][9].ToString();
                        dtErr.Rows.Add(dr);
                    }
                }
            }
            //if (notCheck.Count > 0)
            //{
            //    msg += "未勾選的資料：\\r\\n第";
            //    int j = 0;
            //    foreach (int nc in notCheck)
            //    {
            //        msg += (nc + 1) + ",";
            //        if (j != 0 && j % 10 == 0)
            //            msg += "\\r\\n";
            //        j++;
            //    }
            //    msg = msg.Substring(0, msg.Length - 1) + "筆。\\r\\n";
            //}

            Excel.Dt2Excel(dtErr, "匯入結果", "匯入作業完成，共匯入" + i + "筆、金額共" + sum + "元。匯入失敗的資料如下：");
            if (err.Count > 0)
            {
                msg += "匯入失敗的資料：\\r\\n第";
                int j = 0;
                foreach (int er in err)
                {
                    msg += (er + 1) + ",";
                    if (j != 0 && j % 10 == 0)
                        msg += "\\r\\n";
                    j++;
                }
                msg = msg.Substring(0, msg.Length - 1) + "筆。\\r\\n";
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('匯入作業完成，共匯入" + i + "筆、金額共" + sum + "元。\\r\\n" + msg + "')", true);
        }

        protected void gv_Result_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Result.PageIndex = e.NewPageIndex;
            gv_Result.DataSource = excel;
            gv_Result.DataBind();
        }


        public string Check_Secand_HealthyPay(string ID_NO, int PAY)
        {
            try
            {
                string Exceed = "N";
                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + ID_NO + "' AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )";


                DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (Check_Srank.Rows.Count > 0)//是否為職等R G B
                {

                    if (SoleItemSelector1.item == "08" || SoleItemSelector1.item == "16" || SoleItemSelector1.item == "19" || SoleItemSelector1.item == "20" || SoleItemSelector1.item == "22" || SoleItemSelector1.item == "27" || SoleItemSelector1.item == "40")//是否符合項目.皆為50格式
                    {

                        strSQL = "SELECT * FROM B_SUM_BONUS WHERE  IDCARD='" + ID_NO + "' AND AYEAR='" + txt_DA.Text.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                        if (data.Rows.Count > 0)//table是否有資料
                        {
                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole += Convert.ToDouble(PAY);
                            Total += Convert.ToDouble(PAY);

                            if (Total > Increase_x4)
                            {


                                //if (Exceed != "Y")
                                //{
                                //    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過獎金總額的4倍，須扣二代健保');", true);
                                //}
                                Exceed = "Y";
                                //ViewState["Secand_HealthyPay"] = true;

                            }
                            else
                            {
                                Exceed = "N";
                                // ViewState["Secand_HealthyPay"] = false;
                            }


                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD='" + strPAY_AD + "'";

                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                    new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                    new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = ID_NO},
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = txt_DA.Text.Substring(0, 3)},
                    };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);




                        }
                    }
                }

                return Exceed;

            }

            catch
            {
                return "N";
            }
        }
        ///// <summary>
        ///// 檢查是否超過4倍,並寫回資料庫 B_SUM_BONUS
        ///// </summary>
        //public void Check_Secand_HealthyPay(string ID_NO,int PAY )
        //{

        //    string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + ID_NO + "' AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )";


        //    DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //    if (Check_Srank.Rows.Count > 0)//是否為職等R G B
        //    {

        //        if (SoleItemSelector1.item == "08" || SoleItemSelector1.item == "16" || SoleItemSelector1.item == "19" || SoleItemSelector1.item == "20" || SoleItemSelector1.item == "22" || SoleItemSelector1.item == "27" || SoleItemSelector1.item == "40")//是否符合項目.皆為50格式
        //        {

        //            strSQL = "SELECT * FROM B_SUM_BONUS WHERE  IDCARD='" + ID_NO + "' AND AYEAR='" + txt_DA.Text.Substring(0, 3) + "'";

        //            DataTable data = new DataTable();

        //            data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //            if (data.Rows.Count > 0)//table是否有資料
        //            {
        //                double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
        //                double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
        //                double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
        //                string Exceed = data.Rows[0]["EXCEED"].ToString();

        //                Sole += Convert.ToDouble(PAY);
        //                Total += Convert.ToDouble(PAY);

        //                if (Total > Increase_x4)
        //                {


        //                    //if (Exceed != "Y")
        //                    //{
        //                    //    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過獎金總額的4倍，須扣二代健保');", true);
        //                    //}
        //                    Exceed = "Y";
        //                    //ViewState["Secand_HealthyPay"] = true;

        //                }
        //                else
        //                {
        //                    Exceed = "N";
        //                   // ViewState["Secand_HealthyPay"] = false;
        //                }


        //                strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR";

        //                SqlParameter[] parameterList = {
        //            new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
        //            new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
        //            new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
        //            new SqlParameter("IDCARD",SqlDbType.VarChar){Value = ID_NO},
        //            new SqlParameter("AYEAR",SqlDbType.VarChar){Value = txt_DA.Text.Substring(0, 3)},
        //            };
        //                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);




        //            }
        //        }
        //    }

        //}






    }
}
