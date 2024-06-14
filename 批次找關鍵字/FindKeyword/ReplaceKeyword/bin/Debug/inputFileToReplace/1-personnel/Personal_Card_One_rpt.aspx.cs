using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Card_One_rpt : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //判斷使用者是否有權限進入系統
            if (!IsPostBack)
            {
                A.check_power();           

                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                case "C":
                    break;
                case "D":                    
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void Button_MAKE_ALL_Click(object sender, EventArgs e)
        {
            DataTable Card_One = new DataTable();

            Card_One.Columns.Add("MZ_AD", typeof(string));
            Card_One.Columns.Add("MZ_OCCC", typeof(string));
            Card_One.Columns.Add("MZ_NAME", typeof(string));
            Card_One.Columns.Add("MZ_BIR", typeof(string));
            Card_One.Columns.Add("MZ_IDNO", typeof(string));
            Card_One.Columns.Add("MZ_DATE", typeof(string));


            string strSQL = "SELECT * FROM A_POLICE WHERE MZ_SWT='2'  AND MZ_MEMO1!='Y' ";//AND MZ_INO='2'

            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "')";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim()))
            {
                strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_NAME='" + TextBox_MZ_NAME.Text.Trim() + "')";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_IDNO.Text.Trim()))
            {
                strSQL += " AND MZ_IDNO='" + TextBox_MZ_IDNO.Text + "'";
            }

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (tempDT.Rows.Count > 0)
            {
                DataRow dr = Card_One.NewRow();

                dr["MZ_AD"] = "新北市政府警察局";
                dr["MZ_OCCC"] = o_A_DLBASE.OCCC(tempDT.Rows[0]["MZ_ID"].ToString());
                dr["MZ_BIR"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[0]["MZ_ID"].ToString() + "'");
                dr["MZ_NAME"] = tempDT.Rows[0]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[i]["MZ_ID"].ToString());
                dr["MZ_IDNO"] = "　北警字第" + tempDT.Rows[0]["MZ_IDNO"].ToString() + "號";
                string DATE = tempDT.Rows[0]["MZ_DATE"].ToString();
                dr["MZ_DATE"] = DATE.Substring(0, 3) + "年" + DATE.Substring(3, 2).PadLeft(2, '0') + "月" + DATE.Substring(5, 2).PadLeft(2, '0') + "日";

                Card_One.Rows.Add(dr);
            }

            Session["rpt_dt"] = Card_One;

            string tmp_url = "A_rpt.aspx?fn=Card_One_Criminal_Horizontal";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void Button_MAKE_ALL1_Click(object sender, EventArgs e)
        {
            DataTable Card_One = new DataTable();

            Card_One.Columns.Add("MZ_AD", typeof(string));
            Card_One.Columns.Add("MZ_OCCC", typeof(string));
            Card_One.Columns.Add("MZ_NAME", typeof(string));
            Card_One.Columns.Add("MZ_BIR", typeof(string));
            Card_One.Columns.Add("MZ_IDNO", typeof(string));
            Card_One.Columns.Add("MZ_DATE", typeof(string));


            string strSQL = "SELECT * FROM A_POLICE WHERE (MZ_SWT='1'  OR MZ_SWT='3' OR MZ_SWT='4')  AND MZ_MEMO1!='Y' ";//AND MZ_INO='2'

            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "')";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim()))
            {
                strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_NAME='" + TextBox_MZ_NAME.Text.Trim() + "')";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_IDNO.Text.Trim()))
            {
                strSQL += " AND MZ_IDNO='" + TextBox_MZ_IDNO.Text + "'";
            }

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (tempDT.Rows.Count > 0)
            {
                DataRow dr = Card_One.NewRow();

                dr["MZ_AD"] = "新北市政府警察局";
                dr["MZ_OCCC"] = o_A_DLBASE.OCCC(tempDT.Rows[0]["MZ_ID"].ToString());
                dr["MZ_BIR"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[0]["MZ_ID"].ToString() + "'");
                dr["MZ_NAME"] = tempDT.Rows[0]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[i]["MZ_ID"].ToString());
                dr["MZ_IDNO"] = "　北警" + "    " + tempDT.Rows[0]["MZ_IDNO"].ToString();
                string DATE = tempDT.Rows[0]["MZ_DATE"].ToString();
                dr["MZ_DATE"] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "年" + DATE.Substring(3, 2).PadLeft(2, '0') + "月" + DATE.Substring(5, 2).PadLeft(2, '0') + "日";

                Card_One.Rows.Add(dr);
            }

            Session["rpt_dt"] = Card_One;

            string tmp_url = "A_rpt.aspx?fn=Card_One_Vertical";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
