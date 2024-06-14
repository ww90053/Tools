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
    public partial class B_SearchMonthSalary1 : System.Web.UI.Page
    {
        private string strIDCARD
        {
            set
            {
                Label_IDCARD.Text = value;
            }
        }

        private string strAMONTH
        {
            get
            {
                return Label_AMONTH.Text;
            }
            set
            {
                Label_AMONTH.Text = value;
            }
        }

        private string strMZ_POLNO
        {
            set
            {
                Label_MZ_POLNO.Text = value;
            }
        }

        private string strMZ_NAME
        {
            set
            {
                Label_MZ_NAME.Text = value;
            }
        }

        private string strMZ_OCCC
        {
            set
            {
                Label_MZ_OCCC.Text = value;
            }
        }

        private string strMZ_SLVC_SPOT
        {
            set
            {
                Label_SLVC_SPOT.Text = value;
            }
        }

        private string strMZ_SRANK
        {
            get
            {
                return Label_MZ_SRANK.Text;
            }
            set
            {
                Label_MZ_SRANK.Text = value;
            }
        }

        private int numSALARYPAY1
        {
            set
            {
                Label_SALARYPAY1.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numWORKP
        {
            set
            {
                Label_WORKP.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numPROFESS
        {
            set
            {
                Label_PROFESS.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numBOSS
        {
            set
            {
                Label_BOSS.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numTECHNICS
        {
            set
            {
                Label_TECHNICS.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numBONUS
        {
            set
            {
                Label_BONUS.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numADVENTIVE
        {
            set
            {
                Label_ADVENTIVE.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numFAR
        {
            set
            {
                Label_FAR.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numELECTRIC
        {
            set
            {
                Label_ELECTRIC.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numOTHER
        {
            set
            {
                Label_OTHER.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numINSURANCEPAY
        {
            set
            {
                Label_INSURANCEPAY.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numHEALTHPAY
        {
            set
            {
                Label_HEALTHPAY.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numHEALTHPAY1
        {
            set
            {
                Label_HEALTHPAY1.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numMONTHPAY_TAX
        {
            set
            {
                Label_MONTHPAY_TAX.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numMONTHPAY
        {
            set
            {
                Label_MONTHPAY.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numCONCUR3PAY
        {
            set
            {
                Label_CONCUR3PAY.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numTAX
        {
            set
            {
                Label_TAX.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA01
        {
            set
            {
                Label_EXTRA01.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA02
        {
            set
            {
                Label_EXTRA02.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA03
        {
            set
            {
                Label_EXTRA03.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA04
        {
            set
            {
                Label_EXTRA04.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA05
        {
            set
            {
                Label_EXTRA05.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA06
        {
            set
            {
                Label_EXTRA06.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA07
        {
            set
            {
                Label_EXTRA07.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA08
        {
            set
            {
                Label_EXTRA08.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numEXTRA09
        {
            set
            {
                Label_EXTRA09.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private string strNOTE
        {
            set
            {
                TextBox_NOTE.Text = value;
            }
        }

        private int numADD_SUM
        {
            set
            {
                Label_ADD_SUM.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numDES_SUM
        {
            set
            {
                Label_DES_SUM.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private int numTOTAL
        {
            set
            {
                Label_TOTAL.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        private string strPAY_AD
        {
            get
            {
                return Label_TITLE.Text;
            }
            set
            {
                Label_TITLE.Text = o_A_KTYPE.RAD(value.ToString()) + "薪資明細單";
            }
        }

        private int numMO
        {
            set
            {
                Label_MO.Text = String.Format("{0:#,#0}", int.Parse(value.ToString()));
            }
        }

        string strSQL;
        DataTable temp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!Page.IsPostBack)
            {  SalaryPublic.checkPermission();
                string mm_snid = Request["mm_snid"];
                LoadData(mm_snid);
            }
        }

        protected void LoadData(string mm_snid)
        {
            strSQL = string.Format("SELECT AAA.*,(ADD_SUM-DES_SUM) TOTAL FROM (SELECT NVL((SELECT PAY FROM B_MONTHPAY_OTHER_PAY WHERE MP='P' AND MO_SNID=MM_SNID),0) PAY,NVL((SELECT PAY FROM B_MONTHPAY_OTHER_PAY WHERE MP='M' AND MO_SNID=MM_SNID),0) MO,MM_SNID,IDCARD,AMONTH,MZ_POLNO,PAY_AD,MZ_NAME,MZ_OCCC,MZ_SRANK,MZ_SLVC,MZ_SPT,SALARYPAY1,WORKP,PROFESS,BOSS,TECHNICS,BONUS,ADVENTIVE,FAR,ELECTRIC,(SALARYPAY1+WORKP+PROFESS+BOSS+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC+NVL((SELECT PAY FROM B_MONTHPAY_OTHER_PAY WHERE MP='P' AND MO_SNID=MM_SNID),0)) as ADD_SUM,INSURANCEPAY,HEALTHPAY,HEALTHPAY1,MONTHPAY_TAX,MONTHPAY,CONCUR3PAY,TAX,EXTRA01,EXTRA02,EXTRA03,EXTRA04,EXTRA05,EXTRA06,EXTRA07,EXTRA08,EXTRA09,(INSURANCEPAY+HEALTHPAY+HEALTHPAY1+MONTHPAY_TAX+MONTHPAY+CONCUR3PAY+TAX+EXTRA01+EXTRA02+EXTRA03+EXTRA04+EXTRA05+EXTRA06+EXTRA07+EXTRA08+EXTRA09+NVL((SELECT PAY FROM B_MONTHPAY_OTHER_PAY WHERE MP='M' AND MO_SNID=MM_SNID),0)) AS DES_SUM,NOTE FROM B_MONTHPAY_MAIN WHERE MM_SNID='{0}') AAA"
                                   , mm_snid);
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                strIDCARD = temp.Rows[0]["IDCARD"].ToString();
                strAMONTH = temp.Rows[0]["AMONTH"].ToString();
                strMZ_POLNO = temp.Rows[0]["MZ_POLNO"].ToString();
                strPAY_AD = temp.Rows[0]["PAY_AD"].ToString();
                strMZ_NAME = temp.Rows[0]["MZ_NAME"].ToString();
                strMZ_OCCC = temp.Rows[0]["MZ_OCCC"].ToString();
                strMZ_SRANK = o_A_KTYPE.CODE_TO_NAME(temp.Rows[0]["MZ_SRANK"].ToString(),"09");
                strMZ_SLVC_SPOT = o_A_KTYPE.CODE_TO_NAME(temp.Rows[0]["MZ_SLVC"].ToString(), "64") + temp.Rows[0]["MZ_SPT"].ToString();
                numSALARYPAY1 = int.Parse(temp.Rows[0]["SALARYPAY1"].ToString());
                numWORKP = int.Parse(temp.Rows[0]["WORKP"].ToString());
                numPROFESS = int.Parse(temp.Rows[0]["PROFESS"].ToString());
                numBOSS = int.Parse(temp.Rows[0]["BOSS"].ToString());
                numTECHNICS = int.Parse(temp.Rows[0]["TECHNICS"].ToString());
                numBONUS = int.Parse(temp.Rows[0]["BONUS"].ToString());
                numADVENTIVE = int.Parse(temp.Rows[0]["ADVENTIVE"].ToString());
                numFAR = int.Parse(temp.Rows[0]["FAR"].ToString());
                numOTHER = int.Parse(temp.Rows[0]["PAY"].ToString());
                numELECTRIC = int.Parse(temp.Rows[0]["ELECTRIC"].ToString());
                numINSURANCEPAY = int.Parse(temp.Rows[0]["INSURANCEPAY"].ToString());
                numHEALTHPAY = int.Parse(temp.Rows[0]["HEALTHPAY"].ToString());
                numHEALTHPAY1 = int.Parse(temp.Rows[0]["HEALTHPAY1"].ToString());
                numMONTHPAY_TAX = int.Parse(temp.Rows[0]["MONTHPAY_TAX"].ToString());
                numMONTHPAY = int.Parse(temp.Rows[0]["MONTHPAY"].ToString());
                numCONCUR3PAY = int.Parse(temp.Rows[0]["CONCUR3PAY"].ToString());
                numTAX = int.Parse(temp.Rows[0]["TAX"].ToString());
                numEXTRA01 = int.Parse(temp.Rows[0]["EXTRA01"].ToString());
                numEXTRA02 = int.Parse(temp.Rows[0]["EXTRA02"].ToString());
                numEXTRA03 = int.Parse(temp.Rows[0]["EXTRA03"].ToString());
                numEXTRA04 = int.Parse(temp.Rows[0]["EXTRA04"].ToString());
                numEXTRA05 = int.Parse(temp.Rows[0]["EXTRA05"].ToString());
                numEXTRA06 = int.Parse(temp.Rows[0]["EXTRA06"].ToString());
                numEXTRA07 = int.Parse(temp.Rows[0]["EXTRA07"].ToString());
                numEXTRA08 = int.Parse(temp.Rows[0]["EXTRA08"].ToString());
                numEXTRA09 = int.Parse(temp.Rows[0]["EXTRA09"].ToString());
                numMO = int.Parse(temp.Rows[0]["MO"].ToString());
                numADD_SUM = int.Parse(temp.Rows[0]["ADD_SUM"].ToString());
                numDES_SUM = int.Parse(temp.Rows[0]["DES_SUM"].ToString());
                numTOTAL = int.Parse(temp.Rows[0]["TOTAL"].ToString());
                strNOTE = temp.Rows[0]["NOTE"].ToString();
            }
        }

        protected void Button_PRINT_Click(object sender, EventArgs e)
        {
            Session["TITLE"] = strPAY_AD;
            Session["IDNO"] = Session["MZ_IDforSalary"].ToString();
            Session["AMONTH"] = strAMONTH;
            Session["SRANK"] = strMZ_SRANK;
            string tmp_url = "B_rpt.aspx?fn=PersonalMonthSalary";
            ScriptManager.RegisterClientScriptBlock(TextBox_NOTE, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
