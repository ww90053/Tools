using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Check_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
            }
        }

        protected string police_school(string MZ_ID)
        {
            string SelectString = "SELECT * FROM A_EDUCATION WHERE MZ_ID='" + MZ_ID + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

            int Y = 0;
            string police_school = "";
            string police_department = "";

            for (int i = 0; i < temp.Rows.Count; i++)
            {
                if (temp.Rows[i]["MZ_EDUCLASS"].ToString() == "C")
                {
                    if (Y < int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString()))
                    {
                        Y = int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString());
                        police_school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                        police_department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();

                    }
                }
            }

            string SCHOOL = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                              " AND (dbo.SUBSTR(MZ_KCODE,10,1)='T') " +
                                                              " AND MZ_KCODE='" + police_school + "'");

            string DEPARTMENT = "";

            if (!string.IsNullOrEmpty(police_school))
            {
                if (police_school.Trim() == "301210000T")
                {
                    DEPARTMENT = o_A_KTYPE.Find_Ktype_Cname(police_department, "DP1");
                }
                else if (police_school != "301210000T" && !string.IsNullOrEmpty(police_school))
                {
                    DEPARTMENT = o_A_KTYPE.Find_Ktype_Cname(police_department, "DP2");
                }

                else
                {
                    DEPARTMENT = string.Empty;
                }
            }
            else
            {
                DEPARTMENT = string.Empty;
            }

            return SCHOOL + DEPARTMENT;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            List<String> condition = new List<string>();
            DataTable Check = new DataTable();
            Check.Columns.Clear();
            Check.Columns.Add("MZ_ADUNIT", typeof(string));
            Check.Columns.Add("MZ_OCCC", typeof(string));
            Check.Columns.Add("MZ_NAME", typeof(string));
            Check.Columns.Add("MZ_ID", typeof(string));
            Check.Columns.Add("MZ_BIR", typeof(string));
            Check.Columns.Add("MZ_FDATE", typeof(string));
            Check.Columns.Add("MZ_ADATE", typeof(string));
            Check.Columns.Add("MZ_POLICE_SCHOOL", typeof(string));
            Check.Columns.Add("MZ_SM", typeof(string));
            Check.Columns.Add("MZ_ADD2", typeof(string));
            Check.Columns.Add("MZ_CONTENT", typeof(string));
            Check.Columns.Add("MZ_IDATE", typeof(string));
            Check.Columns.Add("MZ_UNAD", typeof(string));
            Check.Columns.Add("MZ_PRCT", typeof(string));
            Check.Columns.Add("MZ_RESULT", typeof(string));
            Check.Columns.Add("MZ_ODATE", typeof(string));
            Check.Columns.Add("MZ_OUNAD", typeof(string));
            Check.Columns.Add("MZ_RETRIEVE", typeof(string));

            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                condition.Add("MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'");
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim()))
            {
                condition.Add("MZ_ID=(SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME='" + o_str.tosql(TextBox_MZ_NAME.Text.Trim()) + "')");
            }

            string where = (condition.Count > 0 ? string.Join(" AND ", condition.ToArray()) : string.Empty);

            string strSQL = string.Format("SELECT * FROM A_PER_CHECK WHERE {0}", where);

            strSQL += " ORDER BY MZ_IDATE,MZ_CONTENT DESC ";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");


            for (int i = 0; i < temp.Rows.Count; i++)
            {
                DataRow dr = Check.NewRow();

                string CONTENT = temp.Rows[i]["MZ_CONTENT"].ToString();

                if (CONTENT == "01")
                    CONTENT = "教育輔導";
                else if (CONTENT == "02")
                    CONTENT = "風紀評估";
                else if (CONTENT == "03")
                    CONTENT = "違法案件";
                else if (CONTENT == "04")
                    CONTENT = "違紀案件";
                else
                    CONTENT = "其他不良案件";

                dr["MZ_ADUNIT"] = o_A_DLBASE.CAD(temp.Rows[i]["MZ_ID"].ToString()) + o_A_DLBASE.CUNIT(temp.Rows[i]["MZ_ID"].ToString());
                dr["MZ_OCCC"] = o_A_DLBASE.OCCC(temp.Rows[i]["MZ_ID"].ToString());
                dr["MZ_NAME"] = o_A_DLBASE.CNAME(temp.Rows[i]["MZ_ID"].ToString());
                dr["MZ_ID"] = temp.Rows[i]["MZ_ID"].ToString();
                string MZ_BIR = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                dr["MZ_BIR"] = o_CommonService.Personal_ReturnDateString(MZ_BIR).Replace("/", ".");
                string MZ_FDATE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FDATE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                dr["MZ_FDATE"] = o_CommonService.Personal_ReturnDateString(MZ_FDATE).Replace("/", ".");
                string MZ_ADATE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ADATE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                dr["MZ_ADATE"] = o_CommonService.Personal_ReturnDateString(MZ_ADATE).Replace("/", ".");
                dr["MZ_POLICE_SCHOOL"] = police_school(temp.Rows[i]["MZ_ID"].ToString());
                dr["MZ_SM"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SM FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'") == "1" ? "未婚" : "已婚";
                dr["MZ_ADD2"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ADD2 FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                dr["MZ_CONTENT"] = CONTENT;
                dr["MZ_IDATE"] = o_CommonService.Personal_ReturnDateString(temp.Rows[i]["MZ_IDATE"].ToString()).Replace("/", ".");
                dr["MZ_UNAD"] = o_CommonService.d_report_break_line(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='@92' AND MZ_KCODE='" + temp.Rows[i]["MZ_UNAD"].ToString() + "'"), 8, "&N");
                dr["MZ_PRCT"] = temp.Rows[i]["MZ_PRCT"].ToString();
                dr["MZ_RESULT"] = o_CommonService.d_report_break_line(temp.Rows[i]["MZ_RESULT"].ToString(), 14, "&N");
                dr["MZ_ODATE"] = o_CommonService.Personal_ReturnDateString(temp.Rows[i]["MZ_ODATE"].ToString()).Replace("/", ".");
                dr["MZ_OUNAD"] = o_CommonService.d_report_break_line(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='@92' AND MZ_KCODE='" + temp.Rows[i]["MZ_OUNAD"].ToString() + "'"), 8, "&N");
                dr["MZ_PRCT"] = temp.Rows[i]["MZ_PRCT"].ToString();
                dr["MZ_RETRIEVE"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RETRIEVE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                Check.Rows.Add(dr);
            }

            Session["rpt_dt"] = Check;

            string tmp_url = "A_rpt.aspx?fn=check&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }
    }
}
