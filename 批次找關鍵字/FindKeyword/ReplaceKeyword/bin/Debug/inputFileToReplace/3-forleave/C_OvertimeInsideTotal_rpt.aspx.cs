using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_OvertimeInsideTotal_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            
            //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

           
                C.fill_AD_POST(DropDownList_AD);
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
                DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();

                chk_TPMGroup();
            }
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //SqlDataSource_AD.SelectCommand = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            string SQL = "";
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                SQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            else
            {
                SQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";

            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            DropDownList_AD.DataSource = dt;
            DropDownList_AD.DataTextField = "MZ_KCHI";
            DropDownList_AD.DataValueField = "MZ_KCODE";
            DropDownList_AD.DataBind();
            DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
            
            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
            
            
            
            DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
        }

        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {

                case "A":                   
                case "B":
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    break;
                case "C":
                    ChangeDropDownList_AD();
                    //DropDownList_AD.Enabled = false;
                    break;
                case "D":
                    ChangeDropDownList_AD();
                    // DropDownList_AD.Enabled = false;
                    DropDownList_UNIT.Enabled = false;
                    break;
                case "E":
                    ChangeDropDownList_AD();
                    //DropDownList_AD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_AD.Enabled = false;
                    }
                    DropDownList_UNIT.Enabled = false;
                    break;
            }
        }

        string strSQL1;

        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = string.Format(@"SELECT COHI.MZ_ID, AD.MZ_POLNO, RTRIM(AK.MZ_KCHI) MZ_OCCC, RTRIM(AD.MZ_NAME) MZ_NAME, 0 HOUR_PAY, COHI.TOTALTIME, COHI.TOTALPAY, UNIT.MZ_KCHI MZ_EXUNIT, '{0}' MZ_EXAD, 
                                                   dbo.to_number(CASE AD.MZ_TBDV WHEN NULL THEN '999' WHEN 'Z99' THEN '999' ELSE AD.MZ_TBDV END) As TBDV 
                                            FROM (SELECT MZ_ID,MZ_OCCC,SUM(OTIME) AS TOTALTIME,SUM(PAY_SUM) AS TOTALPAY,MZ_EXUNIT 
                                                  FROM C_OVERTIME_HOUR_INSIDE 
                                                  WHERE MZ_OCCC!='Z011' AND MZ_EXAD='{0}' AND (RESTFLAG='N' OR RESTFLAG IS NULL) {1} {2} 
                                                  GROUP BY MZ_ID,MZ_OCCC,MZ_EXUNIT) COHI
                                            left join A_DLBASE AD on AD.MZ_ID = COHI.MZ_ID 
                                            left join A_KTYPE AK on AK.MZ_KTYPE = '26' And AK.MZ_KCODE = COHI.MZ_OCCC 
                                            left join A_KTYPE UNIT on UNIT.MZ_KTYPE = '25' And UNIT.MZ_KCODE = COHI.MZ_EXUNIT 
                                            ORDER BY TBDV,AD.MZ_PCHIEF,COHI.MZ_OCCC "
                                        , DropDownList_AD.SelectedValue
                                        , !string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue) ? string.Format(@"AND MZ_EXUNIT='{0}' ", DropDownList_UNIT.SelectedValue) : ""
                                        , !string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim()) ? string.Format(@"AND dbo.SUBSTR(MZ_DATE,1,5)='{0}' ", o_str.tosql(TextBox_MZ_DATE.Text.Trim())) : "");

            //strSQL = @"SELECT MZ_ID,MZ_OCCC,MZ_EXUNIT,SUM(OTIME) AS TOTALTIME,SUM(PAY_SUM) AS TOTALPAY
            //                  FROM C_OVERTIME_HOUR_INSIDE 
            //                  WHERE MZ_OCCC!='Z011' AND MZ_EXAD='" + DropDownList_AD.SelectedValue + "' AND (RESTFLAG='N' OR RESTFLAG IS NULL) ";
            ////strSQL1 = "SELECT DISTINCT MZ_ID,MZ_EXUNIT,HOUR_PAY FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";

            //if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            //{
            //    strSQL += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            //    //strSQL1 += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim()))
            //{
            //    TextBox_MZ_DATE.Text = TextBox_MZ_DATE.Text.PadLeft(5, '0');

            //    strSQL += " AND dbo.SUBSTR(MZ_DATE,1,5)='" + o_str.tosql(TextBox_MZ_DATE.Text.Trim()) + "'";
            //    //strSQL1 += " AND dbo.SUBSTR(MZ_DATE,1,5)='" + o_str.tosql(TextBox_MZ_DATE.Text.Trim()) + "'";
            //}

            //strSQL += " GROUP BY MZ_ID,MZ_OCCC,MZ_EXUNIT ";
            ////strSQL1 += " AND (RESTFLAG='N' OR RESTFLAG IS NULL)";

            DataTable rpt = new DataTable();
            rpt.Columns.Add("MZ_ID", typeof(string));
            rpt.Columns.Add("MZ_POLNO", typeof(string));
            rpt.Columns.Add("MZ_OCCC", typeof(string));
            rpt.Columns.Add("MZ_NAME", typeof(string));
            rpt.Columns.Add("HOUR_PAY", typeof(int));
            rpt.Columns.Add("TOTALTIME", typeof(int));
            rpt.Columns.Add("TOTALPAY", typeof(int));
            rpt.Columns.Add("MZ_EXUNIT", typeof(string));//中文
            rpt.Columns.Add("MZ_EXAD", typeof(string));//代碼 只是為了給政風室判別用
            rpt.Columns.Add("TBDV", typeof(string));

            DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                //dave issue 20200805 員工加班費彙總表, 單價錯誤, 
                //改成用 [金額]/[時數]
                if (Int32.Parse(tempDT.Rows[i]["TOTALPAY"].ToString())==0 || Int32.Parse(tempDT.Rows[i]["TOTALTIME"].ToString()) == 0)
                {
                    tempDT.Rows[i]["HOUR_PAY"] = Hour_Pay(tempDT.Rows[i]["MZ_ID"].ToString());
                }
                else {
                    tempDT.Rows[i]["HOUR_PAY"] = Hour_Pay2(tempDT.Rows[i]["TOTALPAY"].ToString(), tempDT.Rows[i]["TOTALTIME"].ToString());
                }
                rpt.Rows.Add(tempDT.Rows[i].ItemArray);
            }

            //for (int i = 0; i < tempDT.Rows.Count; i++)
            //{
            //    DataRow dr = rpt.NewRow();

            //    List<String> get = new List<string>();

            //    get = getValue(tempDT.Rows[i]["MZ_ID"].ToString());
            //    //TODO 又跑迴圈去轉代碼,要改掉
            //    dr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
            //    dr["MZ_POLNO"] = o_DBFactory.ABC_toTest.POLNO(tempDT.Rows[i]["MZ_ID"].ToString());
            //    dr["MZ_OCCC"] = o_A_DLBASE.OCCC(tempDT.Rows[i]["MZ_ID"].ToString());
            //    dr["MZ_NAME"] = o_A_DLBASE.CNAME(tempDT.Rows[i]["MZ_ID"].ToString());
            //    dr["HOUR_PAY"] = get[1];
            //    dr["TOTALTIME"] = tempDT.Rows[i]["TOTALTIME"];
            //    dr["TOTALPAY"] = tempDT.Rows[i]["TOTALPAY"];
            //    dr["MZ_EXUNIT"] = get[0];
            //    dr["MZ_EXAD"] = DropDownList_AD.SelectedValue;
            //    rpt.Rows.Add(dr);
            //}

            Session["rpt_dt"] = rpt;

            Session["TITLE"] = DropDownList_AD.SelectedItem.Text;

            Session["TITLE2"] = TextBox_MZ_DATE.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE.Text.Substring(3, 2) + "月";

            string tmp_url = "C_rpt.aspx?fn=OvertimeInsideTotal&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
        }

        protected List<string> getValue(string MZ_ID)
        {
            List<String> result = new List<string>();

            string strSQL2 = "";

            strSQL2 = strSQL1 + " AND MZ_ID='" + MZ_ID + "'";

            DataTable tempDT1 = new DataTable();
            tempDT1 = o_DBFactory.ABC_toTest.Create_Table(strSQL2, "GET2");

            tempDT1.Rows[0]["MZ_EXUNIT"] = o_A_KTYPE.RUNIT(tempDT1.Rows[0]["MZ_EXUNIT"].ToString());

            result.Insert(0, tempDT1.Rows[0]["MZ_EXUNIT"].ToString());

            //result.Insert(1, tempDT1.Rows[0]["HOUR_PAY"].ToString());
            result.Insert(1, Hour_Pay(MZ_ID));

            return result;
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }

        /// <summary>每小時薪資</summary>
        protected string Hour_Pay(string MZ_ID)
        {
            _2_salary.Police Police = new TPPDDB._2_salary.Police(MZ_ID);
            if (Police.occc.Substring(0, 2) == "Z0")
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240 * 1.33).ToString();
            else
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240).ToString();
        }

        /// <summary>每小時薪資</summary>
        protected string Hour_Pay2(string TOTALPAY, string TOTALTIME)
        {
            //dave issue 20200805 員工加班費彙總表, 單價錯誤, 
            //改成用 [金額]/[時數]
            int hourPay = Int32.Parse(TOTALPAY) / Int32.Parse(TOTALTIME);
            return hourPay.ToString();
        }

    }
}
