using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._3_forleave
{
    public partial class C_Change_Dltbb : System.Web.UI.Page
    {
        string year = (DateTime.Now.Year - 1911).ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {

            string strSQL = "SELECT * FROM C_DLTBB WHERE MZ_ID=@MZ_ID AND MZ_YEAR=@MZ_YEAR";

            List<SqlParameter> oralist = new List<SqlParameter>();
            oralist.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = TextBox_MZ_ID.Text });
            oralist.Add(new SqlParameter("MZ_YEAR", SqlDbType.NVarChar) { Value = year });
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.DataSelect(strSQL, oralist);

            if (dt.Rows.Count == 1)
            {
                Label_EXAD.Text = o_A_DLBASE.CAD(TextBox_MZ_ID.Text);
                Label_EXUNIT.Text = o_A_DLBASE.CUNIT(TextBox_MZ_ID.Text);
                Label_NAME.Text = o_A_DLBASE.CNAME(TextBox_MZ_ID.Text);
                Label_OCCC.Text = o_A_DLBASE.OCCC(TextBox_MZ_ID.Text);
                Label_ID.Text = TextBox_MZ_ID.Text;
                TextBox_MZ_HDAY.Text = dt.Rows[0]["MZ_HDAY"].ToString();
                TextBox_MZ_HTIME.Text = dt.Rows[0]["MZ_HTIME"].ToString();
                TextBox_MZ_HDAY.Enabled = true;
                TextBox_MZ_HTIME.Enabled = true;
                btn_Update.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無相關資料！');", true);
                TextBox_MZ_HDAY.Enabled = false;
                TextBox_MZ_HTIME.Enabled = false;
                btn_Update.Enabled = false;
            }
        }

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            var tmp = new System.Globalization.TaiwanCalendar();
            string stryear = tmp.GetYear(DateTime.Now).ToString();
            string MDATE = stryear + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            string MUSER = Session["ADPMZ_ID"].ToString();
            string strSQL = "UPDATE C_DLTBB SET MZ_HDAY=@MZ_HDAY,MZ_HTIME=@MZ_HTIME,MDATE=@MDATE,MUSER=@MUSER WHERE MZ_ID=@MZ_ID AND MZ_YEAR=@MZ_YEAR";

            SqlParameter[] parameterList ={
                                                 new SqlParameter("MZ_HDAY",SqlDbType.Float){Value=TextBox_MZ_HDAY.Text.Length>0?TextBox_MZ_HDAY.Text:"0"},
                                                 new SqlParameter("MZ_HTIME",SqlDbType.Float){Value=TextBox_MZ_HTIME.Text.Length>0?TextBox_MZ_HTIME.Text:"0"},
                                                 new SqlParameter("MZ_ID",SqlDbType.NVarChar){Value=Label_ID.Text},
                                                 new SqlParameter("MZ_YEAR",SqlDbType.NVarChar){Value=year},
                                                 new SqlParameter("MDATE",SqlDbType.NVarChar){Value=MDATE},
                                                 new SqlParameter("MUSER",SqlDbType.NVarChar){Value=MUSER},
                                            };
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功！');", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗！');", true);
            }
        }
    }
}
