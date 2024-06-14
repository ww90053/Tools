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
    public partial class _3_SoleImport : System.Web.UI.Page
    {
        string caseid
        {
            set { ViewState["caseid"] = value; }
            get { return ViewState["caseid"].ToString(); }
        }

        string num
        {
            set { ViewState["num"] = value; }
            get { return ViewState["num"].ToString(); }
        }
        string numName
        {
            set { ViewState["numName"] = value; }
            get { return ViewState["numName"].ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                customInit();
            }
        }

        void customInit()
        {
            SalaryPublic.fillDropDownList(ref ddl_payad);
            SalaryPublic.fillUnitDropDownList(ref ddl_unit, ddl_payad.SelectedValue);
            txt_DA.Text = DateTime.Now.Year - 1911 + DateTime.Now.ToString("MMdd");
        }

        protected void rbl_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            data.Visible = false;

            if (rbl_type.SelectedValue == "1")
            {
                by1.Visible = true;
                by2.Visible = false;
            }
            else
            {
                by1.Visible = false;
                by2.Visible = true;
            }
        }

        // 取得資料來源資料
        protected void btn_get_Click(object sender, EventArgs e)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"SELECT MZ_YEAR, MZ_MONTH, AMOUNT, C_SALARY.PAY_AD ADCODE, AKP.MZ_KCHI PAY_AD, C_SALARY.MZ_UNIT MZ_UNIT, AKU.MZ_KCHI CHIUNIT, AKO.MZ_KCHI MZ_OCCC, A_DLBASE.MZ_ID, MZ_POLNO, MZ_NAME, B_SOLEITEM.ID KINDCODE, B_SOLEITEM.NAME KIND 
                    FROM C_SALARY LEFT JOIN A_DLBASE ON C_SALARY.MZ_ID=A_DLBASE.MZ_ID 
                    LEFT JOIN A_KTYPE AKP ON C_SALARY.PAY_AD=AKP.MZ_KCODE 
                    LEFT JOIN A_KTYPE AKU ON A_DLBASE.MZ_UNIT=AKU.MZ_KCODE AND AKU.MZ_KTYPE='25' 
                    LEFT JOIN A_KTYPE AKO ON A_DLBASE.MZ_OCCC=AKO.MZ_KCODE AND AKO.MZ_KTYPE='26' 
                    LEFT JOIN B_SOLEITEM ON MZ_KIND=CSALARY_MAP 
                    WHERE AMOUNT>0 AND PAD=@PAY_AD AND MZ_KIND=@MZ_KIND"; //20151119 Neil 修正Where條件中的 PAD (原先為參考 A_DLBASE中的資料，但應以 C_SLARAY 為主才對)

            ops.Add(new SqlParameter("PAY_AD", ddl_payad.SelectedValue));
            ops.Add(new SqlParameter("MZ_KIND", ddl_kind.SelectedValue));

            if (txt_year.Text.Length > 0)
            {
                sql += " AND MZ_YEAR=@MZ_YEAR";
                ops.Add(new SqlParameter("MZ_YEAR", txt_year.Text));
            }
            if (txt_month.Text.Length > 0)
            {
                sql += " AND MZ_MONTH=@MZ_MONTH";
                ops.Add(new SqlParameter("MZ_MONTH", txt_month.Text));
            }
            if (ddl_unit.SelectedIndex > 0)
            {
                // 加班費取現服單位；其他都用編制單位
                if (ddl_kind.SelectedValue == "2")
                    sql += " AND MZ_EXUNIT=@UNIT";
                else
                    sql += " AND C_SALARY.MZ_UNIT=@UNIT";
                ops.Add(new SqlParameter("UNIT", ddl_unit.SelectedValue));
            }

            sql += " ORDER BY MZ_TBDV, MZ_OCCC,A_DLBASE.MZ_ID";

            DataTable dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            lb_count.Text = "取得資料共" + dt.Rows.Count + "筆";

            gv_Result.DataSource = dt;
            gv_Result.DataBind();

            if (dt.Rows.Count > 0)
                data.Visible = true;
            else
                data.Visible = false;
        }

        // 開始匯入資料
        protected void btn_Add_Click(object sender, EventArgs e)
        {
            ///20150709 Neil 
            ///新增訊息判斷
            if(ddl_kind.SelectedItem == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請選擇匯入項目')", true);
                return;
            }
            //判斷是否有輸入入賬日期
            if (String.IsNullOrEmpty(txt_DA.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入入帳日期')", true);
                return;
            }
            //判斷是否有輸入年份
            if (String.IsNullOrEmpty(txt_year.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入年份')", true);
                return;
            }
            //2016/1/4
            //Jack修改，加入若選擇4則排除判斷月份。
            //判斷是否有輸入月份
            //if (String.IsNullOrEmpty(txt_month.Text))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入年份')", true);
            //    return;
            //}

            if (String.IsNullOrEmpty(txt_month.Text) && (ddl_kind.SelectedValue != "4" && ddl_kind.SelectedValue != "6"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入月份')", true);
                return;
            }

            numName = ddl_kind.SelectedItem.Text;
            switch (ddl_kind.SelectedValue)
            {
                case "1" : //超勤
                    numName += "加班費";
                    break;
                case "2" : //加班
                case "3" : //值日
                case "4":  //不休假改發加班費
                case "5":  //獎勵休假
                case "6":  //休假超過14日補助費
                    //20150709 其他項目應不需要額外加字眼
                    break;
            }
            numName = txt_year.Text + "年" + txt_month.Text + "月 " + numName; 

            int i = 0;
            int sum = 0;
            List<int> err = new List<int>();
            DataTable dtErr = new DataTable();
            dtErr.Columns.Add("身份證字號/員工編號");
            dtErr.Columns.Add("金額");

            txt_DA.BackColor = System.Drawing.Color.White;
            txt_caseid.BackColor = System.Drawing.Color.White;
            if (txt_DA.Text.Length != 7)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('入帳日期格式不合')", true);
                txt_DA.BackColor = System.Drawing.Color.Orange;
                return;
            }
            try { DateTime.Parse((int.Parse(txt_DA.Text.Substring(0, 3)) + 1911) + "/" + txt_DA.Text.Substring(3, 2) + "/" + txt_DA.Text.Substring(5, 2)); }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('入帳日期格式錯誤')", true);
                txt_DA.BackColor = System.Drawing.Color.Orange;
                return;
            }
            if (txt_caseid.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入入帳案號')", true);
                txt_caseid.BackColor = System.Drawing.Color.Orange;
                return;
            }

            int j = 1;
            foreach (GridViewRow row in gv_Result.Rows)
            {
                // 有勾選的資料才要匯入
                if (!((CheckBox)row.Cells[0].FindControl("cb_add")).Checked)
                    continue;

                try
                {
                    string payad = SalaryPublic.strLoginEXAD;
                    string unit = row.Cells[2].Text;
                    string id = row.Cells[5].Text;
                    string polno = row.Cells[1].Text;
                    string name = row.Cells[6].Text;
                    string num = row.Cells[7].Text;
                    int pay = int.Parse(row.Cells[9].Text);

                    Police police = new Police(id);

                    //超勤不納所得
                    //20150119
                    //if (num == "01")
                    if (ddl_kind.SelectedValue == "5" || ddl_kind.SelectedValue == "6")
                        Sole.insertData(id, polno, payad, unit, name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, num.PadLeft(2, '0'), "50", null, pay, numName, true);
                    else
                        Sole.insertData(id, polno, payad, unit, name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, num.PadLeft(2, '0'), null, null, pay, numName, true);
                    
                       
                    i++;
                    sum += pay;
                    j++;
                }
                catch(Exception ex)
                {
                    String a = ex.Message;
                    err.Add(j);
                    DataRow dr = dtErr.NewRow();
                    dr["身份證字號/員工編號"] = row.Cells[5].Text;
                    dr["金額"] = row.Cells[9].Text;
                    dtErr.Rows.Add(dr);
                }
            }
            ViewState["dtErr"] = dtErr;
            ViewState["MsgData"] = "匯入作業完成，共匯入" + i + "筆、金額共" + sum + "元。匯入失敗的資料如下：";

            string msg = "";
            if (err.Count > 0)
            {
                msg += "匯入失敗的資料：\\r\\n第";
                j = 0;
                foreach (int er in err)
                {
                    msg += (er + 1) + ",";
                    if (j != 0 && j % 10 == 0)
                        msg += "\\r\\n";
                    j++;
                }
                msg = msg.Substring(0, msg.Length - 1) + "筆。\\r\\n";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('匯入作業完成，共匯入" + i + "筆、金額共" + sum + "元。\\r\\n" + msg + "')", true);
        }

        protected void ddl_payad_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref ddl_unit, ddl_payad.SelectedValue);
        }

        protected void txt_DA_TextChanged(object sender, EventArgs e)
        {
            int caseid;
            int.TryParse(txt_caseid.Text, out caseid);
            if (Sole.isLocked(txt_DA.Text, caseid))
                txt_caseid.BackColor = System.Drawing.Color.Orange;
            else
                txt_caseid.BackColor = System.Drawing.Color.White;
        }

        protected void txt_caseid_TextChanged(object sender, EventArgs e)
        {
            //int caseid;
            //int.TryParse(txt_caseid.Text, out caseid);

            //if (Sole.isLocked(txt_DA.Text, caseid))
            //{
            //    btn_Add.Enabled = false;
            //    txt_caseid.BackColor = System.Drawing.Color.Orange;
            //}
            //else
            //{
            txt_caseid.BackColor = System.Drawing.Color.White;
            //}
        }

        // 開啟超勤印領清冊匯入介面
        protected void btn_getOverwork_Click(object sender, EventArgs e)
        {
            // 超勤在單一發放的代碼是01
            num = "01";
            numName = "超勤";
            lb_excel.Text = "超勤印領清冊";
            trCol.Visible = true;
            btn_showExcel_ModalPopupExtender.Show();
        }

        // 開啟交通獎勵金印領清冊匯入介面
        protected void btn_getTraffic_Click(object sender, EventArgs e)
        {
            // 交通獎勵金在單一發放的代碼是16
            num = "16";
            numName = "獎勵金";
            lb_excel.Text = "交通獎勵金印領清冊";
            trCol.Visible = false;
            btn_showExcel_ModalPopupExtender.Show();
        }

        // 取得excel檔案資料
        protected void btn_getExcel_Click(object sender, EventArgs e)
        {
            if (!fl_import.HasFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請選擇檔案')", true);
                return;
            }

            string fileName = fl_import.FileName;
            string tempfilename = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString();// +".xls";
            string up_extension = System.IO.Path.GetExtension(fileName);// 取得上傳檔副檔名
            string savePath = "\\Files\\" + tempfilename + up_extension;
            savePath = System.Web.HttpContext.Current.Server.MapPath(savePath);
            fl_import.SaveAs(savePath);

            DataTable dt;
            try
            {
                dt = Excel.getDataTable(savePath, "Sheet1");
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('取得檔案內容時發生錯誤，請確定檔案未經加密')", true);
                return;
            }

            System.IO.File.Delete(savePath);
            DataTable dtDlbase = o_DBFactory.ABC_toTest.DataSelect("SELECT PAY_AD, MZ_ID, MZ_POLNO, MZ_NAME, MZ_UNIT, AKU.MZ_KCHI CHIUNIT, AKO.MZ_KCHI MZ_OCCC, MZ_SRANK, MZ_SLVC FROM A_DLBASE JOIN A_KTYPE AKO ON MZ_OCCC=MZ_KCODE AND AKO.MZ_KTYPE='26' LEFT JOIN A_KTYPE AKU ON A_DLBASE.MZ_UNIT=AKU.MZ_KCODE AND AKU.MZ_KTYPE='25' ");

            string msg = "";

            if (num == "01")// 超勤
            {
                //如果第一欄是員工編號，用機關+員工編號比對員警資料；如果是身份證字號，直接用身份證字號比對
                if (rbl_firstCol.SelectedValue == "pol")
                {
                    msg = "以員工編號對應不到員警資料，請再確認檔案內容";

                    var query = from f in dt.AsEnumerable()
                                join a in dtDlbase.AsEnumerable()
                                    //on new { name = f.Field<string>(1), occc = f.Field<string>(2) } equals new { name = a.Field<string>("MZ_OCCC"), occc = a.Field<string>("MZ_NAME") }
                                on new { payad = SalaryPublic.strLoginEXAD, polno = f.Field<string>(0).Trim() } equals new { payad = a.Field<string>("PAY_AD"), polno = a.Field<string>("MZ_POLNO") }
                                where f.Field<string>(41) != "0"
                                select new
                                {
                                    MZ_ID = a.Field<string>("MZ_ID"),
                                    MZ_POLNO = a.Field<string>("MZ_POLNO"),
                                    MZ_NAME = a.Field<string>("MZ_NAME"),
                                    MZ_UNIT = a.Field<string>("MZ_UNIT"),
                                    CHIUNIT = a.Field<string>("CHIUNIT"),
                                    MZ_OCCC = a.Field<string>("MZ_OCCC"),
                                    KINDCODE = num,
                                    KIND = numName,
                                    AMOUNT = f.Field<string>(41)
                                };

                    lb_count.Text = "取得資料共" + query.Count() + "筆";

                    gv_Result.DataSource = query;
                }
                else
                {
                    msg = "以身份證字號對應不到員警資料，請再確認檔案內容";

                    var query = from f in dt.AsEnumerable()
                                join a in dtDlbase.AsEnumerable()
                                    //on new { name = f.Field<string>(1), occc = f.Field<string>(2) } equals new { name = a.Field<string>("MZ_OCCC"), occc = a.Field<string>("MZ_NAME") }
                                on f.Field<string>(0).Trim() equals a.Field<string>("MZ_ID")
                                where f.Field<string>(41) != "0"
                                select new
                                {
                                    MZ_ID = a.Field<string>("MZ_ID"),
                                    MZ_POLNO = a.Field<string>("MZ_POLNO"),
                                    MZ_NAME = a.Field<string>("MZ_NAME"),
                                    MZ_UNIT = a.Field<string>("MZ_UNIT"),
                                    CHIUNIT = a.Field<string>("CHIUNIT"),
                                    MZ_OCCC = a.Field<string>("MZ_OCCC"),
                                    KINDCODE = num,
                                    KIND = numName,
                                    AMOUNT = f.Field<string>(41)
                                };

                    lb_count.Text = "取得資料共" + query.Count() + "筆";

                    gv_Result.DataSource = query;
                }
            }
            else if (num == "16")// 交通獎勵金
            {
                msg = "以身份證字號對應不到員警資料，請再確認檔案內容";

                var query = from f in dt.AsEnumerable()
                            join a in dtDlbase.AsEnumerable()
                            on f.Field<string>(0).Trim() equals a.Field<string>("MZ_ID")
                            where f.Field<string>(13) != "0"
                            select new
                            {
                                MZ_ID = a.Field<string>("MZ_ID"),
                                MZ_POLNO = a.Field<string>("MZ_POLNO"),
                                MZ_NAME = a.Field<string>("MZ_NAME"),
                                MZ_UNIT = a.Field<string>("MZ_UNIT"),
                                CHIUNIT = a.Field<string>("CHIUNIT"),
                                MZ_OCCC = a.Field<string>("MZ_OCCC"),
                                KINDCODE = num,
                                KIND = numName,
                                AMOUNT = f.Field<string>(13)
                            };

                lb_count.Text = "取得資料共" + query.Count() + "筆";

                gv_Result.DataSource = query;
            }

            gv_Result.DataBind();
            if (gv_Result.Rows.Count == 0)
            {
                data.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + msg + "')", true);
            }
            else
            {
                data.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('資料取得完成')", true);
            }
        }

        protected void btn_default_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "window.location='34-SoleImportBasic.aspx'", true);
        }


        //20150216 考績匯入改由人事作業.移到 1-personnel/Personal1-1.aspx作業
        //protected void btn_effect_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "window.location='19-EffectImport.aspx'", true);
        //}

        protected void ddl_kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_kind.SelectedValue == "4" || ddl_kind.SelectedValue == "6")
            {
                txt_month.Text = string.Empty;
                txt_month.Enabled = false;
            }
            else
            {
                txt_month.Enabled = true;
            }
            numName = ddl_kind.SelectedItem.Text;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (ViewState["dtErr"] != null)
                Excel.Dt2Excel(ViewState["dtErr"] as DataTable, "匯入結果", ViewState["MsgData"].ToString());
        }

    }
}
