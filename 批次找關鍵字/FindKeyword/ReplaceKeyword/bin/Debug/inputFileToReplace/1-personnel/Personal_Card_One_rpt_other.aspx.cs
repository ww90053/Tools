using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Card_One_rpt_other : System.Web.UI.Page
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

        /// <summary>
        /// 按鈕：刑事警察
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL_Click(object sender, EventArgs e)
        {
            DataTable Card_One = new DataTable();

            #region 資料欄位定義
            Card_One.Columns.Add("MZ_AD", typeof(string));
            Card_One.Columns.Add("MZ_OCCC", typeof(string));
            Card_One.Columns.Add("MZ_NAME", typeof(string));
            Card_One.Columns.Add("MZ_BIR", typeof(string));
            Card_One.Columns.Add("MZ_IDNO", typeof(string));
            Card_One.Columns.Add("MZ_NO1", typeof(string));
            Card_One.Columns.Add("MZ_DATE", typeof(string));

            Card_One.Columns.Add("EXP_DATE", typeof(string));
            #endregion

            #region 取得列印資料
            string strSQL = "SELECT * FROM A_POLICE WHERE MZ_SWT='2' AND MZ_MEMO1!='Y' "; //AND MZ_INO='2'

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
            #endregion

            //刑事警察
            if (tempDT.Rows.Count > 0)
            {
                DataRow dr = Card_One.NewRow();

                dr["MZ_AD"] = "新北市政府警察局";
                dr["MZ_OCCC"] = tempDT.Rows[0]["MZ_OCCC"].ToString();
                dr["MZ_BIR"] = tempDT.Rows[0]["MZ_BIR"].ToString();
                dr["MZ_NAME"] = tempDT.Rows[0]["MZ_NAME"].ToString();
                dr["MZ_IDNO"] = "新北警字第" + tempDT.Rows[0]["MZ_IDNO"].ToString() + "號";
                dr["MZ_NO1"] = tempDT.Rows[0]["MZ_NO1"].ToString();
                string DATE = tempDT.Rows[0]["MZ_DATE"].ToString();
                dr["MZ_DATE"] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "年" + DATE.Substring(3, 2).PadLeft(2, '0') + "月" + DATE.Substring(5, 2).PadLeft(2, '0') + "日";

                dr["EXP_DATE"] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";

                Card_One.Rows.Add(dr);
            }

            Session["rpt_dt"] = Card_One;
            
            string tmp_url = "A_rpt.aspx?fn=Card_One_Criminal_Vertical";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        /// <summary>
        /// 按鈕：行政警察
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL1_Click(object sender, EventArgs e)
        {
            DataTable Card_One = new DataTable();

            #region 資料欄位定義
            Card_One.Columns.Add("MZ_AD", typeof(string));
            Card_One.Columns.Add("MZ_OCCC", typeof(string));
            Card_One.Columns.Add("MZ_BIR", typeof(string));
            Card_One.Columns.Add("MZ_NAME", typeof(string));
            Card_One.Columns.Add("MZ_IDNO", typeof(string));
            Card_One.Columns.Add("MZ_DATE", typeof(string));

            Card_One.Columns.Add("EXP_DATE", typeof(string));
            #endregion

            #region 取得列印資料
            string strSQL = "SELECT * FROM A_POLICE WHERE (MZ_SWT='1'   OR MZ_SWT='3' OR MZ_SWT='4')  AND MZ_MEMO1!='Y' ";//AND MZ_INO='2'

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
            #endregion

            //行政警察
            if (tempDT.Rows.Count > 0)
            {
                DataRow dr = Card_One.NewRow();
                dr["MZ_AD"] = "新北市政府警察局";
                dr["MZ_OCCC"] = tempDT.Rows[0]["MZ_OCCC"].ToString();
                string BIR = tempDT.Rows[0]["MZ_BIR"].ToString();
                if (BIR.Length < 7)
                    BIR = BIR.PadLeft(7, '0');
                dr["MZ_BIR"] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "　 " + BIR.Substring(3, 2).PadLeft(2, '0') + "　　" + BIR.Substring(5, 2).PadLeft(2, '0');

                dr["MZ_NAME"] = tempDT.Rows[0]["MZ_NAME"].ToString();
                dr["MZ_IDNO"] = "新北警　　" + tempDT.Rows[0]["MZ_IDNO"].ToString();
                string DATE = tempDT.Rows[0]["MZ_DATE"].ToString();
                dr["MZ_DATE"] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "　 " + DATE.Substring(3, 2).PadLeft(2, '0') + "　　" + DATE.Substring(5, 2).PadLeft(2, '0');

                dr["EXP_DATE"] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";

                Card_One.Rows.Add(dr);
            }

            Session["rpt_dt"] = Card_One;

            string tmp_url = "A_rpt.aspx?fn=Card_One_Horizontal";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    
    }
}
