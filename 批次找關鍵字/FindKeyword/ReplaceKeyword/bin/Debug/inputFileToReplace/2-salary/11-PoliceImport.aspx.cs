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
    public partial class _1_PoliceImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_getdata_Click(object sender, EventArgs e)
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

            DataTable dt = Excel.getDataTable(savePath, "Sheet1");
            System.IO.File.Delete(savePath);

            //逐筆寫入資料庫
            int success = 0;
            lbx_fail.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                Police police = new Police(row["身份證號"].ToString());

                if (police.id == null)
                {
                    ListItem li = new ListItem(row["身份證號"].ToString() + "(" + row["姓名"].ToString() + ")");
                    lbx_fail.Items.Add(li);
                    continue;
                }

                List<SqlParameter> psPolice = new List<SqlParameter>();
                List<SqlParameter> psSalary = new List<SqlParameter>();
                string srank;
                string slvc;
                string spt;
                string grade;

                srank = row["新職等代碼"].ToString();
                slvc = row["新俸階代碼"].ToString();
                spt = row["新俸點"].ToString();
                grade = row["考績等次"].ToString();

                if (srank != "")
                    psPolice.Add(new SqlParameter("MZ_SRANK", srank));
                if (slvc != "")
                    psPolice.Add(new SqlParameter("MZ_SLVC", slvc));
                if (spt != "")
                    psPolice.Add(new SqlParameter("MZ_SPT", spt.PadLeft(4, '0')));
                if (grade != "")
                    psSalary.Add(new SqlParameter("GRADE", grade));

                //有異動才要更新資料
                if (psPolice.Count > 0)
                {
                    try
                    {
                        police.updateBasicData(psPolice, true);
                        success++;
                    }
                    catch
                    {
                        ListItem li = new ListItem(row["身份證號"].ToString() + "(" + row["姓名"].ToString() + ")");
                        lbx_fail.Items.Add(li);
                    }
                }

                //更新考績等次資料
                if (psSalary.Count > 0)
                    police.updateSalaryData(psSalary);
            }

            lb_success.Text = success.ToString();
            lb_fail.Text = lbx_fail.Items.Count.ToString();
            lb_total.Text = dt.Rows.Count.ToString();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('完成匯入作業')", true);
        }
    }
}
