using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._2_salary
{
    public partial class B_Change : System.Web.UI.Page
    {
        DataTable temp = new DataTable();
        string strSQL = string.Empty;
        DataTable police_dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        string _txt_HealthPersonal = "";
        // 個人健保費
        int healthPersonal
        {
            get
            {
                if (_txt_HealthPersonal == "")
                    return 0;
                return int.Parse(_txt_HealthPersonal);
            }
            set { _txt_HealthPersonal = value.ToString(); }
        }

        DataTable healthInfo
        {
            get { return (DataTable)ViewState["healthInfo"]; }
            set { ViewState["healthInfo"] = value; }
        }

        private string _intHEALPER_Data;
        private int intHEALPER_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(_intHEALPER_Data);
            }
            set
            {
                _intHEALPER_Data = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        protected void btnDeal_Click(object sender, EventArgs e)
        {
            strSQL = @"SELECT PAY_AD,MZ_AD, MZ_UNIT,MZ_EXAD, MZ_EXUNIT, MZ_ID , MZ_NAME NAME  FROM A_DLBASE";
            police_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            int count = 0;
            foreach (DataRow pdr in police_dt.Rows)
            {
                Police police = new Police(pdr["MZ_ID"].ToString());
                healthInfo = police.healthInfo;
                if (police.isOffduty == true)
                {
                    healthPersonal = 0;
                    healthInfo = Salary.getHealthInfo(healthInfo, healthPersonal);
                    intHEALPER_Data = 0;
                }
                else
                {
                    //20140721
                    // healthPersonal = Salary.getHealthPersonal(police.salary, police.boss, police.profess, Salary.getPay(police.workpCode, "B_WORKP"), Salary.getPay(police.technicCode, "B_TECHNICS"), Salary.getPay(police.bonusCode, "B_BONUS"), Salary.getPay(police.adventiveCode, "B_ADVENTIVE"), police.far, Salary.getPay(police.electricCode, "B_ELECTRIC"), police.insuranceType);
                   
                    healthPersonal = Salary.getHealthPersonal(police.salary, police.boss, police.profess, Salary.getPay(police.workpCode, "B_WORKP"), Salary.getPay(police.technicCode, "B_TECHNICS"), Salary.getPay(police.bonusCode, "B_BONUS"), Salary.getPay(police.adventiveCode, "B_ADVENTIVE"), police.far, police.electric, police.insuranceType);
                    healthInfo = Salary.getHealthInfo(healthInfo, healthPersonal);
                    //20140618 
                    //intHEALPER_Data = Salary.getHealth(healthInfo, healthPersonal, police.is30Years == true ? true : false);
                    intHEALPER_Data = Salary.getHealth(healthInfo, healthPersonal);
                }

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    conn.Open();
                    SqlTransaction Ltransation;
                    Ltransation = conn.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand(string.Format("UPDATE B_BASE SET PERSONALHEALTHPAY={0},HEALTHPAY={1} WHERE IDCARD='{2}'", healthPersonal, intHEALPER_Data, police.id), conn);
                        cmd.Transaction = Ltransation;
                        cmd.ExecuteNonQuery();

                        police.updateHealthInfo(healthInfo);

                        Ltransation.Commit();
                    }
                    catch /*(Exception ex)*/
                    {
                        Ltransation.Rollback();
                    }
                }
                count++;
            }



            CommonCS.ShowMessage(btnDeal, string.Format("查詢：{0}筆，計算：{1}筆", police_dt.Rows.Count, count));
        }
    }
}
