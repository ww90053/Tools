using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class _5_DecreaseImport : System.Web.UI.Page
    {
        //匯入結果
        DataTable excel
        {
            get { return (DataTable)ViewState["excel"]; }
            set { ViewState["excel"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btn_getExcel_Click(object sender, EventArgs e)
        {
            if (!fl_import.HasFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請選擇檔案')", true);
                return;
            }

            #region 匯入Excel並轉成DataTable
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
            #endregion

            //取得ID不為空白的資料
            var query = from f in excel.AsEnumerable()
                        where f.Field<string>(0) != ""
                        select new
                        {
                            MZ_ID = f.Field<string>(0),
                            MZ_NAME = f.Field<string>(2),
                            AMOUNT = f.Field<string>(1)
                        };

            lb_count.Text = "取得資料共" + query.Count() + "筆";
            if (query.Count() > 0)
            {
                tr_data.Visible = true;
                gv_Result.Visible = true;
                excel = query.AsEnumerable().ToDataTable();
            }
            else
            {
                tr_data.Visible = false;
                gv_Result.Visible = false;
            }

            gv_Result.DataSource = query;
            gv_Result.DataBind();
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            int i = 0;
            int sum = 0;
            string msg = "";

            if (txt_Year.Text.Length != 3)
            {
                msg += "請輸入年度\\r\\n";
            }

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
            dtErr.Columns.Add("姓名");

            for (int j = 0; j < gv_Result.Rows.Count; j++)
            {
                // 有勾選的資料才要匯入
                if (!((CheckBox)gv_Result.Rows[j].Cells[0].FindControl("cb_add")).Checked)
                {
                    notCheck.Add(j);
                    continue;
                }

                try
                {
                    string id = excel.Rows[j][0].ToString();
                    string payad = SalaryPublic.strLoginEXAD;
                    int pay;
                    if (excel.Rows[j][2].ToString() == "")
                        pay = 0;
                    else
                        pay = int.Parse(excel.Rows[j][2].ToString());

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
                            dr["姓名"] = excel.Rows[j][1];
                            dr["金額"] = pay;
                            dtErr.Rows.Add(dr);
                            continue;
                        }
                        if (!Tax.updateDecrease(op.id, pay, txt_Year.Text, payad))
                        {
                            err.Add(j);
                            DataRow dr = dtErr.NewRow();
                            dr["身份證字號"] = excel.Rows[j][0].ToString();
                            dr["姓名"] = excel.Rows[j][1].ToString();
                            dr["金額"] = excel.Rows[j][2].ToString();
                            dtErr.Rows.Add(dr);
                        }
                        else
                        {
                            i++;
                            sum += pay;
                        }
                        //Sole.insertData(op.id, op.polno, payad, op.name, null, null, null, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, true);
                    }
                    else
                    {
                        if (!Tax.updateDecrease(police.id, pay, txt_Year.Text, payad))
                        {
                            err.Add(j);
                            DataRow dr = dtErr.NewRow();
                            dr["身份證字號"] = excel.Rows[j][0].ToString();
                            dr["姓名"] = excel.Rows[j][1].ToString();
                            dr["金額"] = excel.Rows[j][2].ToString();
                            dtErr.Rows.Add(dr);
                        }
                        else
                        {
                            i++;
                            sum += pay;
                        }
                    }
                    //Sole.insertData(id, police.polno, payad, police.name, police.occc, police.srank, police.slvc, "IN", txt_DA.Text, txt_caseid.Text, SoleItemSelector1.item, SoleItemSelector1.tax_id == "" ? null : SoleItemSelector1.tax_id, SoleItemSelector1.tax_id1 == "" ? null : SoleItemSelector1.tax_id1, pay, txt_note.Text, pay1, pay2, pay3, tax, untax, extra01, true);
                    
                }
                catch
                {
                    err.Add(j);
                    DataRow dr = dtErr.NewRow();
                    dr["身份證字號"] = excel.Rows[j][0].ToString();
                    dr["姓名"] = excel.Rows[j][1].ToString();
                    dr["金額"] = excel.Rows[j][2].ToString();
                    dtErr.Rows.Add(dr);
                }
            }

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
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", string.Format("window.location='B_SalaryIncomeTax3.aspx?TPM_FION={0}'", Request.QueryString["TPM_FION"]), true);
        }
    }
}
