using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public partial class B_SearchSole2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (IsPostBack != true)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
                Label_MSG.Text = "";
                Label_MSG.ForeColor = System.Drawing.Color.White;
                voidCASEID();
            }
        }

        protected bool chkYearType()
        {
            if (TextBox_DA.Text.Length != 0 || RadioButtonList_TYPE.SelectedValue == "ALL")
            {
                return true;
            }
            return false;
        }

        //查詢   VW_ALL_SOLE_DATA
        protected void btPrint_Click(object sender, EventArgs e)
        {
            if (chkYearType())
            {
                sendToReport();
            }
            else
            {
                Label_MSG.Text = "輸入日期格式錯誤";
                Label_MSG.Visible = true;
            }
        }

        private void voidTYPE()
        {
            tr_DA.Visible = true;
            DateTime DateTime_Data = DateTime.Now;
            switch (strTYPE)
            {
                case "ALL":
                    tr_DA.Visible = false;
                    strDA = "";
                    break;
                case "AYEAR":
                    strDA = (DateTime_Data.Year - 1911).ToString("000");
                    break;
                case "AMONTH":
                    strDA = (DateTime_Data.Year - 1911).ToString("000") + DateTime_Data.Month.ToString("00");
                    break;
                case "ADAY":
                    strDA = (DateTime_Data.Year - 1911).ToString("000") + DateTime_Data.Month.ToString("00") + DateTime_Data.Day.ToString("00");
                    break;
            }
        }


        private void sendToReport()
        {
            DataTable dt = new DataTable();

            bool boolPASS = false;
            Label_MSG.Text = "輸入日期格式錯誤";
            Label_MSG.ForeColor = System.Drawing.Color.Red;
            switch (strTYPE)
            {
                case "AYEAR":
                    if (strDA.Length == 3)
                    {
                        boolPASS = true;
                    }
                    break;
                case "AMONTH":
                    if (strDA.Length == 5)
                    {
                        boolPASS = true;
                    }
                    break;
                case "ADAY":
                    if (strDA.Length == 7)
                    {
                        boolPASS = true;
                    }
                    break;
            }

            if (boolPASS || strTYPE.Equals("ALL"))
            {
                dt = voidSelectSOLE(strIDCARD, strTYPE, strDA, strCASEID, strDA_INOUT_GROUP);
                Label_MSG.Text = "";
                Label_MSG.ForeColor = System.Drawing.Color.White;
            }

            //dt.Columns.Add("PAGE", typeof(int));
            //int count = 1;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (i % 11 == 0)
            //    {
            //        count++;
            //    }
            //    dt.Rows[i]["PAGE"] = count;
            //}
            //測試 DataTable，以上呼叫程式，請勿刪除；請在以下呼叫 Report

            if (dt.Rows.Count > 0)
            {
                Session["rpt_dt"] = dt;
                string tmp_url = "B_rpt.aspx?fn=Sole&TPM_FION=" + Request.QueryString["TPM_FION"];
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
            }
        }

        private void voidCASEID()
        {
            ListItem liData_0 = new ListItem("全部", "ALL");
            DropDownList_CASEID.Items.Insert(0, liData_0);

            int intB = 1;
            for (int i = 1; i < 16; i++)
            {
                ListItem liData = new ListItem(intB.ToString("00"), intB.ToString("00"));
                DropDownList_CASEID.Items.Insert(i, liData);
                intB++;
            }
            ListItem liData_A1 = new ListItem("A1", "A1");
            DropDownList_CASEID.Items.Insert(16, liData_A1);
            ListItem liData_A2 = new ListItem("A2", "A2");
            DropDownList_CASEID.Items.Insert(17, liData_A2);
            ListItem liData_A3 = new ListItem("A3", "A3");
            DropDownList_CASEID.Items.Insert(18, liData_A3);
            ListItem liData_A4 = new ListItem("A4", "A4");
            DropDownList_CASEID.Items.Insert(19, liData_A4);
            ListItem liData_A5 = new ListItem("A5", "A4");
            DropDownList_CASEID.Items.Insert(20, liData_A5);
            ListItem liData_H1 = new ListItem("H1", "H1");
            DropDownList_CASEID.Items.Insert(21, liData_H1);
        }

        private string strIDCARD
        {
            get
            {
                //Session["ADPMZ_ID"] = "T222222222";
                return Session["ADPMZ_ID"].ToString();
            }
        }

        private string strTYPE
        {
            get
            {
                return RadioButtonList_TYPE.SelectedValue;
            }
        }

        private string strDA
        {
            get
            {
                return TextBox_DA.Text;
            }
            set
            {
                TextBox_DA.Text = value;
            }
        }

        private string strCASEID
        {
            get
            {
                return DropDownList_CASEID.SelectedValue;
            }
        }

        private string strDA_INOUT_GROUP
        {
            get
            {
                return RadioButtonList_DA_INOUT_GROUP.SelectedValue;
            }
        }

        private DataTable voidSelectSOLE(string strIDCARD_Data, string strTYPE_Data, string strDA_Data, string strCASEID_Data, string strDA_INOUT_GROUP_Data)
        {
            List<String> lsSQL_Data = new List<string>();
            if (!strDA_Data.Equals(""))
            {
                switch (strTYPE_Data)
                {
                    case "AYEAR":
                        lsSQL_Data.Add("dbo.SUBSTR(DA, 1, 3) = '" + strDA_Data.Substring(0, 3) + "'");
                        break;
                    case "AMONTH":
                        lsSQL_Data.Add("dbo.SUBSTR(DA, 1, 5) = '" + strDA_Data.Substring(0, 5) + "'");
                        break;
                    case "ADAY":
                        lsSQL_Data.Add("dbo.SUBSTR(DA, 1, 7) = '" + strDA_Data.Substring(0, 7) + "'");
                        break;
                }
            }
            if (!strCASEID_Data.Equals("ALL"))
            {
                lsSQL_Data.Add("CASEID = '" + strCASEID_Data + "'");
            }
            if (!strDA_INOUT_GROUP_Data.Equals("ALL"))
            {
                lsSQL_Data.Add("DA_INOUT_GROUP = '" + strDA_INOUT_GROUP_Data + "'");
            }
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                lsSQL_Data.Add("IDNO ='" + TextBox_MZ_ID.Text.ToString().ToUpper() + "'");
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                lsSQL_Data.Add("NAME ='" + TextBox_MZ_NAME.Text.ToString() + "'");
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                lsSQL_Data.Add("MZ_POLNO ='" + TextBox_MZ_POLNO.Text.ToString() + "'");
            }

            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = String.Format("SELECT * FROM VW_ALL_SOLE_DATA WHERE PAY_AD = '{0}' {1}", DropDownList_PAY_AD.SelectedValue.ToString(), lsSQL_Data.Count > 0 ? " AND " + String.Join(" AND ", lsSQL_Data.ToArray()) : String.Empty);
                    strSQL+=" group by S_SNID, DA, DA_INOUT_GROUP, CASEID, BANKID, PAY_AD, CHIAD, MZ_UNIT, PAY_UNIT, FIANCENO, UNITACC, TAXINVOICE, BANKNO, MZ_ADATE, ACCOUNT, IDNO, MZ_POLNO, NAME, MZ_SRANK, MZ_OCCC, MZ_SLVC, MZ_SPT, CHISRANK, CHIOCCC, CHISLVC, NUM, ITEM, TAXID, PAY, TAX, PAY1, PAY2, PAY3, SAVEUNTAX, EXTRA01, SECOND_HEALTHPAY_PAY, TOTALSUB, NOTE, INOUTNAME, LOCKDB, IN_ACCOUNT_DATE, TO_BANK_SN";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Session["RPT_SQL_B"] = strSQL;

                    return o_DBFactory.ABC_toTest.DataSelect(strSQL);
                }
                catch { throw; }
                finally { Selectconn.Close(); }
            }
        }

        protected void DropDownList_CASEID_SelectedIndexChanged(object sender, EventArgs e)
        {
            voidTYPE();
        }

        protected void RadioButtonList_DATA_SelectedIndexChanged(object sender, EventArgs e)
        {
            voidTYPE();
        }

        protected void RadioButtonList_TYPE_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                voidTYPE();
            }
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {

            Response.Redirect("B_SearchSole2.aspx?&TPM_FION=" + Request.QueryString["TPM_FION"]);
        }
    }
}
