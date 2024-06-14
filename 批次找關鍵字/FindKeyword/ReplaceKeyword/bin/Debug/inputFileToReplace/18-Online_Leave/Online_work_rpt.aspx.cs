using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._18_Online_Leave
{
    public partial class Online_work_rpt : System.Web.UI.Page
    {
        string C_strGID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["ADServerID"] != null)
            //{
            //    switch (Request.QueryString["TPM_FION"])
            //    {
            //        case "":
            //        case null:
            //            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), "0", "TPFXXX0001");
            //            Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
            //            break;
            //        default:
            //            if (TPMPermissions._boolPermissionID(int.Parse(Session["TPM_MID"].ToString()), Request.QueryString["TPM_FION"].ToString(), "PVIEW") == false)
            //            {
            //                //無權限
            //                TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
            //                Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
            //            }
            //            break;
            //    }
            //}
            //else
            //{
            //    Response.Redirect("~/Login.aspx");
            //}

            ///群組權限
            C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            //by MQ ------------------------------20100331

            if (!IsPostBack)
            {

                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                chk_TPMGroup();
            }
        }

        protected void chk_TPMGroup()
        {
            switch (C_strGID)
            {
                case "A":

                    break;
                case "B":

                    break;
                case "C":

                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = string.Format("SELECT MZ_DLTB01_SN FROM C_DLTB01 WHERE MZ_IDATE1='{0}'", TextBox_MZ_DATE.Text);

            if (TextBox_MZ_ID.Text.Length > 0)
            {
                strSQL += " AND MZ_ID='" + TextBox_MZ_ID.Text + "'";
            }

            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                strSQL += " AND MZ_NAME='" + TextBox_MZ_NAME.Text + "'";
            }

            string DLTB01_SN = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

            if (DLTB01_SN.Length > 0)
            {
                DataTable dt = new DataTable();

                dt = o_DBFactory.ABC_toTest.Create_Table(string.Format("SELECT * FROM C_LEAVE_HISTORY WHERE DLTB01_SN='{0}' ORDER BY SN", DLTB01_SN), "12223");

                DataTable excel_dt = new DataTable();

                excel_dt.Columns.Add("簽核時間", typeof(string));
                excel_dt.Columns.Add("機關", typeof(string));
                excel_dt.Columns.Add("單位", typeof(string));
                excel_dt.Columns.Add("職稱", typeof(string));
                excel_dt.Columns.Add("審核者", typeof(string));
                excel_dt.Columns.Add("狀態", typeof(string));
                excel_dt.Columns.Add("意見", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow excel_dr = excel_dt.NewRow();

                    if (dr["PROCESS_DATE"].ToString().Length == 0)
                    {
                        excel_dr["簽核時間"] = "尚未批";
                    }
                    else
                    {
                        excel_dr["簽核時間"] = dr["PROCESS_DATE"].ToString().Substring(0, 3) + "/" +
                                dr["PROCESS_DATE"].ToString().Substring(3, 2) + "/" +
                                dr["PROCESS_DATE"].ToString().Substring(5, 2) + " " +
                                dr["PROCESS_TIME"].ToString();
                    }

                    string exad = o_A_DLBASE.CAD(dr["REVIEW_ID"].ToString());
                    excel_dr["機關"] = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    excel_dr["單位"] = o_A_DLBASE.CUNIT(dr["REVIEW_ID"].ToString());
                    excel_dr["職稱"] = o_A_DLBASE.OCCC(dr["REVIEW_ID"].ToString());
                    excel_dr["審核者"] = o_A_DLBASE.CNAME(dr["REVIEW_ID"].ToString());
                    excel_dr["狀態"] = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT C_STATUS_NAME FROM C_STATUS WHERE C_STATUS_SN={0} ", dr["PROCESS_STATUS"].ToString()));
                    excel_dr["意見"] = dr["REVIEW_MESSAGE"].ToString();

                    excel_dt.Rows.Add(excel_dr);
                }
                excel_dt.Columns[0].ColumnName = "S1";
                excel_dt.Columns[1].ColumnName = "S2";
                excel_dt.Columns[2].ColumnName = "S3";
                excel_dt.Columns[3].ColumnName = "S4";
                excel_dt.Columns[4].ColumnName = "S5";
                excel_dt.Columns[5].ColumnName = "S6";
                excel_dt.Columns[6].ColumnName = "S7";
                Session["onRptSn"] = DLTB01_SN;
                Session["on_history_rpt"] = excel_dt;
                string tmp_url = "On_rpt.aspx?fn=History&TPM_FION=" + Request["TPM_FION"];
                 ScriptManager.RegisterClientScriptBlock(TextBox_MZ_DATE, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "btn_message", "alert('無相關資料！');", true);
            }
        }
    }
}
