using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace TPPDDB._2_salary
{
    public partial class B_effectdetail_rpt : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
       
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            { SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicYear();
                strSQL = string.Format("SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')", strAD);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                DropDownList_UNIT.DataSource = temp;
                DropDownList_UNIT.DataTextField = "MZ_KCHI";
                DropDownList_UNIT.DataValueField = "MZ_KCODE";
                DropDownList_UNIT.DataBind();
                DropDownList_UNIT.Items.Insert(0, "請選擇");
            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text;
            }
        }

        private string strUNIT
        {
            get
            {
                return DropDownList_UNIT.SelectedValue;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            #region 舊的

            /*
             
            rpt_dt = rpt_dt_init();
            //發薪機關及編制單位下的所有人。
            int num = 1;
            strSQL = string.Format("SELECT E_SNID,(SELECT MZ_ADATE FROM A_DLBASE WHERE MZ_ID=BE.IDCARD) MZ_ADATE,BE.IDCARD,MZ_NAME,AYEAR,BE.PAY_AD,MZ_OCCC,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_UNIT,MZ_POLNO,PAY,SALARYPAY1,PROFESS,BOSS,WORKP,TECHNICS,FAR,(SALARYPAY1+PROFESS+BOSS+WORKP+TECHNICS+FAR) SSUM,TAX,EXTRA01,TOTAL,NOTE ,BANKID,BBS.STOCKPILE_BANKID FROM B_EFFECT BE LEFT JOIN B_BASE_STOCKPILE BBS ON BBS.IDCARD = BE.IDCARD AND \"GROUP\" = '2' WHERE BE.MZ_UNIT='{0}' AND BE.AYEAR='{1}' AND BE.PAY_AD='{2}'",
                                   strUNIT, strAYEAR, strAD);
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow dr1 = rpt_dt.NewRow();
                    dr1[0] = num;//流水編號
                    num++;
                    dr1[1] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_SRANK"].ToString(), "09");
                    dr1[2] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_SLVC"].ToString(), "64") + "," + dr["MZ_SPT"].ToString();
                    dr1[3] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_OCCC"].ToString(), "26");
                    dr1[4] = dr["MZ_NAME"];
                    dr1[5] = dr["MZ_ADATE"];
                    dr1[6] = dr["SALARYPAY1"];
                    dr1[7] = dr["BOSS"];
                    dr1[8] = dr["PROFESS"];
                    dr1[9] = dr["WORKP"].ToString() == "0" ? dr["TECHNICS"].ToString() : dr["WORKP"].ToString();
                    dr1[10] = dr["FAR"];
                    dr1[11] = dr["PAY"].ToString() == "0" ? "0個月" : string.Format("{0:0.0}", float.Parse(dr["PAY"].ToString())) + "個月";
                    dr1[12] = dr["SSUM"];
                    dr1[13] = dr["TAX"];
                    dr1[14] = dr["EXTRA01"];
                    dr1[15] = dr["TOTAL"];
                    dr1[16] = dr["NOTE"];
                    dr1[17] = dr["IDCARD"];
                    dr1[18] = dr["MZ_POLNO"];
                    dr1[19] = dr["BANKID"].ToString() + dr["STOCKPILE_BANKID"].ToString();
                    rpt_dt.Rows.Add(dr1);
                }
            }

            /*
            for (int i = 0; i < 14; i++)
            {
                DataRow dr = rpt_dt.NewRow();
                dr["num"] = num;
                num++;
                rpt_dt.Rows.Add(dr);
            }
            */

            //GridView1.DataSource = rpt_dt;
            //GridView1.DataBind();


            #endregion

            string strSQL;

            if (DropDownList_UNIT.SelectedIndex == 0)
                strSQL = string.Format("SELECT  ROWNUM NUM, VW.* FROM (SELECT  * FROM VW_EFFECT_DETAIL WHERE PAY_AD = '{0}' AND AYEAR = '{1}'   ORDER BY  MZ_UNIT,MZ_POLNO) VW  ORDER BY NUM", strAD, strAYEAR);
            else
                strSQL = string.Format("SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_EFFECT_DETAIL WHERE PAY_AD = '{0}' AND MZ_UNIT = '{1}' AND AYEAR = '{2}' ORDER BY  MZ_UNIT,MZ_POLNO) VW  ORDER BY NUM", strAD, strUNIT, strAYEAR);

            //2013/01/29 修改考績獎金報表格式
            Session["rpt_dt"] = Salary.addPageNumber(o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW"),13,"PAY_UNIT");

            Session["SalaryReportAD"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04");
            Session["TITLE"] = strAYEAR + "年";
            Session["TITLE1"] = "";
            string tmp_url = "B_rpt.aspx?fn=effect_detail";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

      

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            strSQL = string.Format("SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')", strAD);
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_UNIT.DataSource = temp;
            DropDownList_UNIT.DataTextField = "MZ_KCHI";
            DropDownList_UNIT.DataValueField = "MZ_KCODE";
            DropDownList_UNIT.DataBind();
            DropDownList_UNIT.Items.Insert(0, "請選擇");
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_effectdetail_rpt.aspx");
        }

        //2013/02/04新增考績獎金匯出EXCEL資料表
        protected void btn_Export_Click(object sender, EventArgs e)
        {
            string excelTitle = strAYEAR + "年";
            excelTitle += o_A_KTYPE.RAD(strAD);
           

            if (DropDownList_UNIT.SelectedIndex == 0)
            {
                strSQL = string.Format(@"SELECT ROWNUM 編號,MZ_SRANK 等級,SLVC_SPT 俸階,MZ_OCCC 職務名稱,MZ_NAME 姓名,MZ_ADATE 在職日期,SALARYPAY1 薪俸,BOSS 主管加給,
                                         PROFESS 專業加給,WORKP_TECHNICS 警勤技術加給,FAR 偏遠加給,ELECTRICPAY 繁重加給,
                                         PAY 發給月數,SSUM 合計,TAX 所得稅稅率,EXTRA01 法院扣款,TOTAL 實領金額, NOTE 備註,IDCARD 身分證號,ACCOUNT 帳號 
                                         FROM (SELECT  * FROM VW_EFFECT_DETAIL WHERE PAY_AD = '{0}' AND AYEAR = '{1}' ORDER BY  MZ_UNIT,MZ_POLNO) VW  ORDER BY 編號", strAD, strAYEAR);
            }
            else
            {
                excelTitle += o_A_KTYPE.RUNIT(strUNIT);
                strSQL = string.Format(@"SELECT ROWNUM 編號,MZ_SRANK 等級,SLVC_SPT 俸階,MZ_OCCC 職務名稱,MZ_NAME 姓名,MZ_ADATE 在職日期,SALARYPAY1 薪俸,BOSS 主管加給,
                                         PROFESS 專業加給,WORKP_TECHNICS 警勤技術加給,FAR 偏遠加給,ELECTRICPAY 繁重加給,
                                         PAY 發給月數,SSUM 合計,TAX 所得稅稅率,EXTRA01 法院扣款,TOTAL 實領金額, NOTE 備註,IDCARD 身分證號,ACCOUNT 帳號 
                                         FROM (SELECT  * FROM VW_EFFECT_DETAIL WHERE PAY_AD = '{0}' AND MZ_UNIT = '{1}' AND AYEAR = '{2}'  ORDER BY  MZ_UNIT,MZ_POLNO) VW  ORDER BY 編號", strAD, strUNIT, strAYEAR);
            }

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            excelTitle += "考績獎金明細表";
            Excel.Dt2Excel(dt, excelTitle);
        }
    }
}
