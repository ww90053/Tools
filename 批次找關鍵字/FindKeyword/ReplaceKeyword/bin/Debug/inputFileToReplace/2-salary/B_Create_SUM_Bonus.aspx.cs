using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace TPPDDB._2_salary
{
    public partial class B_Create_SUM_Bonus : System.Web.UI.Page
    {
        string strSQL = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ADServerID"] != null)
            {
                switch (Request.QueryString["TPM_FION"])
                {
                    case "":
                    case null:
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), "0", "TPFXXX0001");
                        Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                        break;
                    default:
                        if (TPMPermissions._boolPermissionID(int.Parse(Session["TPM_MID"].ToString()), Request.QueryString["TPM_FION"].ToString(), "PVIEW") == false)
                        {
                            //無權限
                            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                            Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                        }
                        break;
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }

            Label_MSG.Visible = false;

            if (!IsPostBack)
            {
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);//只有權限為A.B能用下拉選單
                TextBox_YEAR.Text = SalaryPublic.strRepublicYear();


                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD_only);//只有權限為A.B能用下拉選單
                TextBox_YEAR_only.Text = SalaryPublic.strRepublicYear();
            }

            Label_TITLE.Text = "建立年度二代健保統計資料";
        }


        /// <summary>
        /// 判斷是否還停留在初始化設定
        /// </summary>
        /// <returns></returns>
        protected bool chkYearType()
        {
            if (TextBox_YEAR.Text.Length == 3)
            {
                return true;
            }
            return false;
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
          
            List<SqlParameter> ops = new List<SqlParameter>();

            if (chkYearType())
            {

               

              
                strSQL = "SELECT * FROM dbo.B_SUM_BONUS WHERE PAY_AD='" + DropDownList_PAY_AD.SelectedValue.ToString() + "' AND AYEAR='" + TextBox_YEAR.Text + "'";
                DataTable Select_Old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                if (Select_Old.Rows.Count > 0)
                {

                    strSQL = "DELETE dbo.B_SUM_BONUS WHERE PAY_AD=@PAY_AD AND AYEAR=@AYEAR";
                    SqlParameter[] parameterList = {
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = TextBox_YEAR.Text},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = DropDownList_PAY_AD.SelectedValue.ToString()},
                    };

                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);


                }




                string strAD = DropDownList_PAY_AD.SelectedValue.ToString();
                string Year = TextBox_YEAR.Text;
                string Month = "";

                if (Convert.ToInt32(Year) + 1911 < DateTime.Now.Year)
                {
                    Month = "12";
                }

                else
                {
                    if (DateTime.Now.Month < 10)
                        Month = "0" + DateTime.Now.Month.ToString();
                    else
                        Month = DateTime.Now.Month.ToString();

                    strSQL = string.Empty;

                }



                //收尋人員
                
                strSQL = "SELECT MZ_ID,MZ_NAME,MZ_OCCC,MZ_UNIT  FROM dbo.VW_ALL_BASE_DATA WHERE PAY_AD IS NOT NULL AND PAY_AD='" + strAD + "'";
                DataTable AD_People = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");





                for (int i = 0; i < AD_People.Rows.Count; i++)
                {
                    string note = "";


                    string Name = AD_People.Rows[i]["MZ_NAME"].ToString();
                    string IDNumber = AD_People.Rows[i]["MZ_ID"].ToString();

                   
                    string Unit = AD_People.Rows[i]["MZ_UNIT"].ToString();
                    string OCCC = AD_People.Rows[i]["MZ_OCCC"].ToString();

                    double Effect = 0;
                    double Repair_effect = 0;

                    double YearPay = 0;
                    double Sole = 0;

                    double Total = 0;

                    double Increase = 0;
                    double Increase_x4 = 0;

                    string Exceed = "";


                    // 收尋考績
                    
                    strSQL = "SELECT SUM(SALARYPAY1+PROFESS+WORKP+TECHNICS+FAR) FROM dbo.B_EFFECT WHERE AYEAR='" + (Convert.ToInt32(Year) - 1) + "' AND IDCARD='" + IDNumber + "'" +
                                            " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  ";

                    note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

                    //補發晉級考績
                    

                    strSQL = "SELECT SUM(SALARYPAY1+WORKPPAY+PROFESSPAY+TECHNICSPAY+ BONUSPAY+ADVENTIVEPAY+FARPAY+ELECTRICPAY ) TOTAL  FROM  dbo.B_REPAIRPAY WHERE ATYPE=6  AND dbo.SUBSTR(AMONTH, 1, 3)='" + (Convert.ToInt32(Year)) + "' AND IDCARD='" + IDNumber + "'" +
                         " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  GROUP BY  IDCARD";
                    string str_repair_effect = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

                    if (note != "" && note != null)
                    {
                        if (str_repair_effect != "" && str_repair_effect != null)
                        {
                            Repair_effect = Convert.ToDouble(str_repair_effect);

                        }
                        Effect = Convert.ToDouble(note) + Repair_effect;
                        
                        note = "";

                    }

                    


                    //收尋年終獎金
                    
                    strSQL = "SELECT SUM(SALARYPAY1+PROFESS) FROM dbo.B_YEARPAY WHERE AYEAR='" + (Convert.ToInt32(Year) - 1) + "' AND IDCARD='" + IDNumber + "'" +
                         " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  ";

                    note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                    if (note != "" && note != null)
                    {
                        YearPay = Convert.ToDouble(note);
                        note = "";
                    }


                    

                    //收尋單一發放50格式
                    
                    //加入種類與職等判斷
                   
                    //加入種類與職等判斷
                    strSQL = " SELECT SUM(PAY) PAY FROM dbo.B_SOLE WHERE IDCARD='" + IDNumber + "' AND   dbo.SUBSTR(DA, 1, 3) ='" + Year +
                        "' AND( NUM='08' OR NUM='16' OR NUM='19' OR NUM='20' OR NUM='22' OR NUM='27' OR NUM='40')" +
                    " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )   GROUP BY  IDCARD";

                    note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                    if (note != "" && note != null)
                    {
                        Sole = Convert.ToDouble(note);
                        note = "";
                    }

                   

                    //收尋投保金額,每月應發金額
                    strSQL = "SELECT (SALARYPAY1+BOSS+PROFESS+WORKP+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC) TOTAL FROM dbo.B_MONTHPAY_MAIN WHERE IDCARD='" + IDNumber + "' AND AMONTH='" + Year + Month + "'";
                    DataTable dt_INCREASE = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    if (dt_INCREASE.Rows.Count > 0)
                    {
                        double temp = Convert.ToDouble(dt_INCREASE.Rows[0][0]);
                        strSQL = "SELECT PAY2 FROM dbo.B_HEALTH_INSURANCE WHERE PAY1<=" + temp + " AND PAY2>=" + temp;

                        note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                        if (note != null && note != "")
                            Increase = Convert.ToDouble(note);
                        else 
                        {
                            Increase = Convert.ToDouble(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY2 FROM (SELECT * FROM dbo.B_HEALTH_INSURANCE ORDER BY ID DESC)  WHERE ROWNUM<=1"));
                        
                        }



                     
                 


                    }

                    Total = Effect + YearPay + Sole;

                    Increase_x4 = Increase * 4;

                   


                    if (Total > Increase_x4 && Increase_x4 != 0)
                        Exceed = "Y";
                    else if (Increase_x4 == 0)
                        Exceed = "N";
                    else
                        Exceed = "N";
                   

                    ops.Clear();
                    strSQL = @"INSERT INTO dbo.B_SUM_BONUS (IDCARD, MZ_NAME, AYEAR, EFFECT, YEARPAY, SOLE, TOTAL, INCREASE, INCREASE_X4, EXCEED, PAY_AD, MZ_OCCC, MZ_UNIT)" +

                    @" VALUES (@IDCARD, @MZ_NAME, @AYEAR, @EFFECT, @YEARPAY, @SOLE, @TOTAL, @INCREASE, @INCREASE_X4, @EXCEED, @PAY_AD, @MZ_OCCC, @MZ_UNIT)";

              

                    SqlParameter[] parameterList = {
                   
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = IDNumber},
                
                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = Name},
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = Year},
                    new SqlParameter("EFFECT",SqlDbType.Float){Value = Effect},
                    new SqlParameter("YEARPAY",SqlDbType.Float){Value = YearPay},
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                
                    new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                    new SqlParameter("INCREASE",SqlDbType.Float){Value = Increase},
                    new SqlParameter("INCREASE_X4",SqlDbType.Float){Value =Increase_x4},
                    new SqlParameter("EXCEED",SqlDbType.VarChar){Value =  Exceed},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = strAD},
                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = OCCC},
                    new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = Unit},
                    };

                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                
                }


               

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生完畢');", true);

           
            }
            else
            {
                Label_MSG.Text = "輸入年份格式錯誤";
                Label_MSG.Visible = true;
            }
        }

        protected void Button_SEARCH_only_Click(object sender, EventArgs e)
        {

            List<SqlParameter> ops = new List<SqlParameter>();

            if (chkYearType())
            {




                strSQL = "SELECT * FROM dbo.B_SUM_BONUS WHERE PAY_AD='" + DropDownList_PAY_AD_only.SelectedValue.ToString() + 
                    "' AND AYEAR='" + TextBox_YEAR_only.Text + "' AND IDCARD='"+ txt_ID_only.Text.Trim()   +"' " ;
                DataTable Select_Old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                if (Select_Old.Rows.Count > 0)
                {

                    strSQL = "DELETE dbo.B_SUM_BONUS WHERE PAY_AD=@PAY_AD AND AYEAR=@AYEAR AND IDCARD=@IDCARD";
                    SqlParameter[] parameterList = {
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = TextBox_YEAR_only.Text},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = DropDownList_PAY_AD_only.SelectedValue.ToString()},
                     new SqlParameter("IDCARD",SqlDbType.VarChar){Value = txt_ID_only.Text.Trim()},
                    };

                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);


                }




                string strAD = DropDownList_PAY_AD_only.SelectedValue.ToString();
                string Year = TextBox_YEAR_only.Text;
                string Month = "";

                if (Convert.ToInt32(Year) + 1911 < DateTime.Now.Year)
                {
                    Month = "12";
                }

                else
                {
                    if (DateTime.Now.Month < 10)
                        Month = "0" + DateTime.Now.Month.ToString();
                    else
                        Month = DateTime.Now.Month.ToString();

                    strSQL = string.Empty;

                }



                //收尋人員
                //VW_ALL_BASE_DATA 
                strSQL = "SELECT MZ_ID,MZ_NAME,MZ_OCCC,MZ_UNIT  FROM dbo.A_DLBASE WHERE PAY_AD IS NOT NULL AND PAY_AD='" + strAD + "' AND MZ_ID='"+ txt_ID_only.Text   +"'";
                DataTable AD_People = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


               


                for (int i = 0; i < AD_People.Rows.Count; i++)
                {
                    string note = "";


                    string Name = AD_People.Rows[i]["MZ_NAME"].ToString();
                    string IDNumber = AD_People.Rows[i]["MZ_ID"].ToString();


                    string Unit = AD_People.Rows[i]["MZ_UNIT"].ToString();
                    string OCCC = AD_People.Rows[i]["MZ_OCCC"].ToString();

                    double Effect = 0;
                    double Repair_effect = 0;

                    double YearPay = 0;
                    double Sole = 0;

                    double Total = 0;

                    double Increase = 0;
                    double Increase_x4 = 0;

                    string Exceed = "";


                    // 收尋考績

                    strSQL = "SELECT SUM(SALARYPAY1+PROFESS+WORKP+TECHNICS+FAR) FROM dbo.B_EFFECT WHERE AYEAR='" + (Convert.ToInt32(Year) - 1) + "' AND IDCARD='" + IDNumber + "'" +
                                            " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND PAY_AD='" + strAD + "' ";

                    note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

                    //補發晉級考績


                    strSQL = "SELECT SUM(SALARYPAY1+WORKPPAY+PROFESSPAY+TECHNICSPAY+ BONUSPAY+ADVENTIVEPAY+FARPAY+ELECTRICPAY ) TOTAL  FROM  dbo.B_REPAIRPAY WHERE ATYPE=6  AND dbo.SUBSTR(AMONTH, 1, 3)='" + (Convert.ToInt32(Year)) + "' AND IDCARD='" + IDNumber + "'" +
                         " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  AND PAY_AD='" + strAD + "' GROUP BY  IDCARD";
                    string str_repair_effect = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

                    if (note != "" && note != null)
                    {
                        if (str_repair_effect != "" && str_repair_effect != null)
                        {
                            Repair_effect = Convert.ToDouble(str_repair_effect);

                        }
                        Effect = Convert.ToDouble(note) + Repair_effect;

                        note = "";

                    }




                    //收尋年終獎金

                    strSQL = "SELECT SUM(SALARYPAY1+PROFESS) FROM dbo.B_YEARPAY WHERE AYEAR='" + (Convert.ToInt32(Year) - 1) + "' AND IDCARD='" + IDNumber + "'" +
                         " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  AND PAY_AD='" + strAD + "' ";

                    note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                    if (note != "" && note != null)
                    {
                        YearPay = Convert.ToDouble(note);
                        note = "";
                    }




                    //收尋單一發放50格式

                    //加入種類與職等判斷

                    //加入種類與職等判斷
                    strSQL = " SELECT SUM(PAY) PAY FROM dbo.B_SOLE WHERE IDCARD='" + IDNumber + "' AND   dbo.SUBSTR(DA, 1, 3) ='" + Year +
                        "' AND( NUM='08' OR NUM='16' OR NUM='19' OR NUM='20' OR NUM='22' OR NUM='27' OR NUM='40')" +
                    " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND PAY_AD='" + strAD + "'  GROUP BY  IDCARD";

                    note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                    if (note != "" && note != null)
                    {
                        Sole = Convert.ToDouble(note);
                        note = "";
                    }



                    //收尋投保金額,每月應發金額
                    strSQL = "SELECT (SALARYPAY1+BOSS+PROFESS+WORKP+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC) TOTAL FROM dbo.B_MONTHPAY_MAIN WHERE IDCARD='" + IDNumber + "' AND AMONTH='" + Year + Month + "' AND PAY_AD='" + strAD + "'";
                    DataTable dt_INCREASE = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    if (dt_INCREASE.Rows.Count > 0)
                    {
                        double temp = Convert.ToDouble(dt_INCREASE.Rows[0][0]);
                        strSQL = "SELECT PAY2 FROM dbo.B_HEALTH_INSURANCE WHERE PAY1<=" + temp + " AND PAY2>=" + temp;

                        note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                        if (note != null && note != "")
                            Increase = Convert.ToDouble(note);
                        else
                        {
                            Increase = Convert.ToDouble(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY2 FROM (SELECT * FROM dbo.B_HEALTH_INSURANCE ORDER BY ID DESC)  WHERE ROWNUM<=1"));

                        }







                    }

                    Total = Effect + YearPay + Sole;

                    Increase_x4 = Increase * 4;




                    if (Total > Increase_x4 && Increase_x4 != 0)
                        Exceed = "Y";
                    else if (Increase_x4 == 0)
                        Exceed = "N";
                    else
                        Exceed = "N";


                    ops.Clear();
                    strSQL = @"INSERT INTO dbo.B_SUM_BONUS (IDCARD, MZ_NAME, AYEAR, EFFECT, YEARPAY, SOLE, TOTAL, INCREASE, INCREASE_X4, EXCEED, PAY_AD, MZ_OCCC, MZ_UNIT)" +

                    @" VALUES (@IDCARD, @MZ_NAME, @AYEAR, @EFFECT, @YEARPAY, @SOLE, @TOTAL, @INCREASE, @INCREASE_X4, @EXCEED, @PAY_AD, @MZ_OCCC, @MZ_UNIT)";



                    SqlParameter[] parameterList = {
                   
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = IDNumber},
                
                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = Name},
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = Year},
                    new SqlParameter("EFFECT",SqlDbType.Float){Value = Effect},
                    new SqlParameter("YEARPAY",SqlDbType.Float){Value = YearPay},
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                
                    new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                    new SqlParameter("INCREASE",SqlDbType.Float){Value = Increase},
                    new SqlParameter("INCREASE_X4",SqlDbType.Float){Value =Increase_x4},
                    new SqlParameter("EXCEED",SqlDbType.VarChar){Value =  Exceed},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = strAD},
                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = OCCC},
                    new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = Unit},
                    };

                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);


                }




                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生完畢');", true);


            }
            else
            {
                Label_MSG.Text = "輸入年份格式錯誤";
                Label_MSG.Visible = true;
            }




        }

    }
}
