using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace TPPDDB._2_salary
{
    public class SalaryPublic
    {
        //檢核登入者權限
        public static void checkPermission()
        {
            if (HttpContext.Current.Session["ADServerID"] != null)
            {
                switch (HttpContext.Current.Request.QueryString["TPM_FION"])
                {
                    case "":
                    case null:
                        TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), "0", "TPFXXX0001");
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                        break;
                    default:
                        if (TPMPermissions._boolPermissionID(int.Parse(HttpContext.Current.Session["TPM_MID"].ToString()), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "PVIEW") == false)
                        {
                            //無權限
                            TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                        }
                        break;
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }
        }

        //取得登入者群組權限
        public static string GetGroupPermission()
        {
            string rs = "";

            // 要加註解
            if (HttpContext.Current.Session["TPM_MID"] != null)
            {
                int TPM_MID = int.Parse(HttpContext.Current.Session["TPM_MID"].ToString());

                // ABCD權限判斷
                rs = TPPDDB.TPMPermissions._strGroupData_ID(TPM_MID, int.Parse("221"));
            }
            return rs;
        }

       

        //登入者現服機關
        public static string strLoginEXAD
        {
            get
            {
                if (HttpContext.Current.Session["ADPMZ_EXAD"] != null)
                {
                    SalaryPublic SP = new SalaryPublic();
                    return HttpContext.Current.Session["ADPMZ_EXAD"].ToString();
                }
                return "";
            }
        }

        


        //登入者
        public static string strLoginID
        {
            get
            {
                if (HttpContext.Current.Session["ADPMZ_ID"] != null)
                {
                    SalaryPublic SP = new SalaryPublic();
                    return HttpContext.Current.Session["ADPMZ_ID"].ToString();
                }
                return "";
            }
        }

        //public static DataTable DataSelect(string sql, List<SqlParameter> SqlPrams)
        //{
        //    DataTable DT = new DataTable();
        //    SqlCommand cmd;
        //    SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);

        //    if (conn.State == ConnectionState.Closed)
        //    {
        //        conn.Open();
        //    }

        //    cmd = new SqlCommand(sql, conn);

        //    try
        //    {
        //        if (SqlPrams != null && SqlPrams.Count() > 0)
        //        {
        //            foreach (SqlParameter para in SqlPrams)
        //            {

        //                cmd.Parameters.Add(para);
        //            }
        //        }

        //        DT.Load(cmd.ExecuteReader());

        //        return DT;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {//XX2013/06/18 
        //        //if (conn.State == ConnectionState.Open)
        //        //{
        //        //    conn.Close();
        //        //}
        //        conn.Close();
                
        //       conn.Dispose();
        //    }
        //}

        /// <summary>
        /// 將使用SqlParameter的字串轉換為完整的SQL陳述式
        /// </summary>
        /// <param name="sqlcmd">原SQL字串</param>
        /// <param name="parameters">參數[]</param>
        /// <returns>SQL陳述式</returns>
        public static string RegixSQL(string sqlcmd, List<SqlParameter> parameters)
        {
            //宣告sqlcmd轉大寫後的字串
            string fnlstr = sqlcmd.Trim().ToUpper();
            fnlstr = fnlstr.Replace("\"", "");

            //將參數依照參數名稱長度由長到短排序，避免部分長字串中包含短字串目標造成的錯誤取代
            var sqlodby = from qry in parameters
                          orderby qry.ParameterName.Length descending
                          select qry;

            foreach (var item in sqlodby)
            {
                string strValue;


                //參數值如果是DBNull或null，就寫入值 null
                if (item.Value == Convert.DBNull || item.Value == null)
                {
                    strValue = "null";
                }
                //參數型態如果是VarChar、NVarChar、Char、NChar，在值的前後加上單引號 -> 'value'
                else if (item.SqlDbType == SqlDbType.VarChar || item.SqlDbType == SqlDbType.NVarChar || item.SqlDbType == SqlDbType.Char || item.SqlDbType == SqlDbType.NChar)
                {
                    strValue = "'" + item.Value.ToString() + "'";
                }
                else if (item.SqlDbType == SqlDbType.DateTime)
                {
                    //以空格將日期值分為日期、上下午、時間
                    string[] strarr = item.Value.ToString().Split(' ');
                    //strarr[2]為時間，將時間區以":"分號分為時、分、秒
                    string[] arrTime = new string[3];
                    if (strarr.Length == 3)
                    {
                        arrTime = strarr[2].Split(':');

                        //如果是下午，把小時數+12
                        if (strarr[1] == "下午")
                            arrTime[0] = (int.Parse(arrTime[0]) + 12).ToString();
                    }
                    else
                    {
                        arrTime[0] = "00";
                        arrTime[1] = "00";
                        arrTime[2] = "00";
                    }

                    strValue = "dbo.TO_DATE('" + strarr[0] + " " + arrTime[0] + ":" + arrTime[1] + ":" + arrTime[2] + "','YYYY-MM-DD HH24:MI:SS')";
                }
                else
                {
                    strValue = item.Value.ToString();
                }

                fnlstr = fnlstr.Replace(":" + item.ParameterName, strValue);
            }
            return fnlstr.Trim();
        }

        #region 下拉選單

        /// <summary>
        /// 在下拉式選單加入"全部"的選項，值為""
        /// </summary>
        /// <param name="ddl"></param>
        public static void AddFirstSelection(ref DropDownList ddl)
        {
            ListItem liT = new ListItem("全部", "");
            ddl.Items.Insert(0, liT);
            ddl.SelectedIndex = 0;
        }

        /// <summary>
        /// 裝填所得稅格式下拉選單
        /// </summary>
        /// <param name="ddl">下拉控制項</param>
        public static void fillTaxTypeDropDownList(ref DropDownList ddl)
        {
            string cmd = string.Format("SELECT DISTINCT TAXES_ID FROM B_TAXES_IDTYPE ORDER BY TAXES_ID");
            ddl.Items.Clear();
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(cmd, "DropDownList");
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["TAXES_ID"].ToString().Trim(), dr["TAXES_ID"].ToString().Trim());
                ddl.Items.Add(li);
            }
        }

        /// <summary>
        /// 依所屬機關裝填機關下拉選單(加入admin判斷)
        /// </summary>
        /// <param name="ddl">下拉控制項</param>
        public static void fillDropDownList(ref DropDownList ddl)
        {
            string cmd;
            string strGroup_Data_Function;
            string strPAY_AD = strLoginEXAD;// 2011/01/06 改抓登入者(出納)的現服機關                   
            //群組權限
            if (HttpContext.Current.Session["ADPMZ_ID"] != null)
            {
                strGroup_Data_Function = GetGroupPermission();
            }
            else
            {
                strGroup_Data_Function = "";
            }

            //除TPMIDISAdmin、A、B群組可出現完整下拉選單外，其餘鎖定登入者發薪機關()
            switch (strGroup_Data_Function)
            {
                case "TPMIDISAdmin":
                case "A":
                case "B":
                    //cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '37641%') UNION ALL SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE");
                    cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE");
                    break;
                default:
                    //如果現服機關是中和分局下拉需多加中和一&中和二 matthew
                    if (strPAY_AD == "382133600C")
                    {
                        cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) in ('382133400C','382133500C','382133600C')) ORDER BY MZ_KCODE", strPAY_AD);
                    }
                    else
                    {
                        cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') AND (RTRIM(MZ_KCODE) = '{0}') ORDER BY MZ_KCODE", strPAY_AD);

                    }
                    break;
            }

            if (!string.IsNullOrEmpty(strPAY_AD))
            {
                ddl.Items.Clear();
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(cmd, "DropDownList");
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["MZ_KCHI"].ToString().Trim(), dr["MZ_KCODE"].ToString().Trim());
                    ddl.Items.Add(li);
                }
            }
        }
        /// <summary>
        /// 中和分局合併後依發薪機關裝填機關下拉選單(matthew)
        /// </summary>
        /// <param name="ddl">下拉控制項</param>
        public static void fillDropDownList_Chang(ref DropDownList ddl)
        {
            string cmd;
            string strGroup_Data_Function;
            string strPAY_AD = strLoginEXAD;// 2011/01/06 改抓登入者(出納)的現服機關                   
            //群組權限
            if (HttpContext.Current.Session["ADPMZ_ID"] != null)
            {
                strGroup_Data_Function = GetGroupPermission();
            }
            else
            {
                strGroup_Data_Function = "";
            }

            //除TPMIDISAdmin、A、B群組可出現完整下拉選單外，其餘鎖定登入者發薪機關()
            switch (strGroup_Data_Function)
            {
                case "TPMIDISAdmin":
                case "A":
                case "B":
                    //cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '37641%') UNION ALL SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE");
                    cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE");
                    break;
                default:
                    cmd = string.Format("SELECT (MZ_KCODE + RTRIM(MZ_KCHI)) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') AND (RTRIM(MZ_KCODE) = '{0}') ORDER BY MZ_KCODE", strPAY_AD);
                    break;
            }

            if (!string.IsNullOrEmpty(strPAY_AD))
            {
                ddl.Items.Clear();
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(cmd, "DropDownList");
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["MZ_KCHI"].ToString().Trim(), dr["MZ_KCODE"].ToString().Trim());
                    ddl.Items.Add(li);
                }
            }
        }
        /// <summary>
        /// 依所屬機關裝填單位下拉選單(加入admin判斷)
        /// </summary>
        /// <param name="ddl">下拉控制項</param>
        public static void fillUnitDropDownList(ref DropDownList ddl, string strPAY_AD)
        {
            string cmd;
            string strGroup_Data_Function;

            //群組權限
            if (HttpContext.Current.Session["ADPMZ_ID"] != null)
            {
                strGroup_Data_Function = o_a_Function.strGID(HttpContext.Current.Session["ADPMZ_ID"].ToString());
            }
            else
            {
                strGroup_Data_Function = "";
            }

            //除TPMIDISAdmin、A、B群組可出現完整下拉選單外，其餘鎖定登入者發薪機關()
            switch (strGroup_Data_Function)
            {
                case "TPMIDISAdmin":
                case "A":
                case "B":
                    cmd = string.Format("SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE,A_UNIT_AD WHERE MZ_KCODE=MZ_UNIT AND (RTRIM(MZ_KTYPE) = '25') AND (RTRIM(MZ_AD) = '" + strPAY_AD + "') ORDER BY MZ_KCODE");
                    break;
                default:
                    cmd = string.Format("SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE,A_UNIT_AD WHERE MZ_KCODE=MZ_UNIT AND (RTRIM(MZ_KTYPE) = '25') AND (RTRIM(MZ_AD) = '" + strPAY_AD + "') ORDER BY MZ_KCODE");
                    break;
            }

            if (!string.IsNullOrEmpty(strPAY_AD))
            {
                ddl.Items.Clear();
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(cmd, "DropDownList");
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["MZ_KCHI"].ToString().Trim(), dr["MZ_KCODE"].ToString().Trim());
                    ddl.Items.Add(li);
                }
            }

            ListItem liT = new ListItem("全部", "");
            ddl.Items.Insert(0, liT);
        }

        /// <summary>
        /// 依所得格式裝填項目下拉選單
        /// </summary>
        /// <param name="ddl">下拉控制項</param>
        /// <param name="strTaxType">所得格式</param>
        public static void fillsubTaxDropDownList(ref DropDownList ddl, string strTaxType)
        {
            string cmd = string.Format("SELECT TAXES_ID1,TAXES_NOTE FROM B_TAXES_IDTYPE WHERE TAXES_ID = '{0}' ORDER BY TAXES_ID1", strTaxType);
            if (!string.IsNullOrEmpty(strTaxType))
            {
                ddl.Items.Clear();
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(cmd, "DropDownList");
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["TAXES_NOTE"].ToString().Trim(), dr["TAXES_ID1"].ToString().Trim());
                    ddl.Items.Add(li);
                }
            }
        }

        /// <summary>
        /// 填入單一發放案號下拉選單
        /// </summary>
        /// <param name="ddl">下拉控制項</param>
        public static void FillCaseIDDropDownList(ref DropDownList ddl)
        {
            int intB = 1;
            for (int i = 0; i < 15; i++)
            {
                ListItem liData = new ListItem(intB.ToString("00"), intB.ToString("00"));
                ddl.Items.Insert(i, liData);
                intB++;
            }
            ListItem liData_A1 = new ListItem("超勤", "A1");
            ddl.Items.Insert(15, liData_A1);
            ListItem liData_A2 = new ListItem("加班", "A2");
            ddl.Items.Insert(16, liData_A2);
            ListItem liData_A3 = new ListItem("值日費", "A3");
            ddl.Items.Insert(17, liData_A3);
            ListItem liData_A4 = new ListItem("不休假獎金", "A4");
            ddl.Items.Insert(18, liData_A4);
            ListItem liData_A5 = new ListItem("獎勵休假", "A5");
            ddl.Items.Insert(19, liData_A5);
            ListItem liData_H1 = new ListItem("協勤時數", "H1");
            ddl.Items.Insert(20, liData_H1);
        }

        public static void FillSRankDropDownList(ref DropDownList ddl)
        {
            DataTable dt;
            string sql;

            sql = "SELECT * FROM A_KTYPE WHERE MZ_KTYPE='09' AND (MZ_KCODE LIKE 'B%' OR MZ_KCODE LIKE 'D%' OR MZ_KCODE LIKE 'G%' OR MZ_KCODE LIKE 'J%' OR MZ_KCODE LIKE 'M%' OR MZ_KCODE LIKE 'P%') ORDER BY MZ_KCODE";
            dt = o_DBFactory.ABC_toTest.Create_Table(sql, "tag");

            ddl.DataValueField = "MZ_KCODE";
            ddl.DataTextField = "MZ_KCHI";
            ddl.DataSource = dt;
            ddl.DataBind();

            ListItem li = new ListItem("", "");
            ddl.Items.Insert(0, li);
        }

        public static void FillSLVCDropDownList(ref DropDownList ddl)
        {
            DataTable dt;
            string sql;

            sql = "SELECT * FROM A_KTYPE WHERE MZ_KTYPE='64' ORDER BY MZ_KCODE";
            dt = o_DBFactory.ABC_toTest.Create_Table(sql, "tag");

            ddl.DataValueField = "MZ_KCODE";
            ddl.DataTextField = "MZ_KCHI";
            ddl.DataSource = dt;
            ddl.DataBind();

            ListItem li = new ListItem("", "");
            ddl.Items.Insert(0, li);
        }

        public static void FillHealthMode(ref DropDownList ddl)
        {
            DataTable dt;
            string sql;

            sql = "SELECT * FROM B_HEALTH_MODE ORDER BY SORT";
            dt = o_DBFactory.ABC_toTest.Create_Table(sql, "tag");

            ddl.DataValueField = "ID";
            ddl.DataTextField = "TEXT";
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        /// <summary>
        /// 抓取 個人健保費的狀態選項 
        /// </summary>
        /// <param name="ddl"></param>
        public static void FillHealthMode_noold(ref DropDownList ddl)
        {
            DataTable dt;
            string sql;

            sql = "SELECT * FROM B_HEALTH_MODE ORDER BY SORT";
            dt = o_DBFactory.ABC_toTest.Create_Table(sql, "tag");

            ddl.DataValueField = "COST";
            ddl.DataTextField = "TEXT";
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        public static void FillHealthRelation(ref DropDownList ddl)
        {
            DataTable dt;
            string sql;

            sql = "SELECT * FROM B_HEALTH_RELATION ORDER BY SORT";
            dt = o_DBFactory.ABC_toTest.Create_Table(sql, "tag");

            ddl.DataValueField = "ID";
            ddl.DataTextField = "TEXT";
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        // 主管級別
        public static void fillMZ_PCHIEF(ref DropDownList ddl)
        {
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataTextField = "MZ_KCHI";
            ddl.DataSource = o_DBFactory.ABC_toTest.Create_Table("SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='56' ORDER BY MZ_KCODE) WHERE 1=1", "TAG");
            ddl.DataBind();

            ListItem li = new ListItem("", "");
            ddl.Items.Insert(0, li);
        }

        // 兼代職
        public static void fillMZ_EXTPOS(ref DropDownList ddl)
        {
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataTextField = "MZ_KCHI";
            ddl.DataSource = o_DBFactory.ABC_toTest.Create_Table("SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='@91'", "TAG");
            ddl.DataBind();

            ListItem li = new ListItem("", "");
            ddl.Items.Insert(0, li);
        }

        // 任(退)職原因
        public static void fillMZ_NREA(ref DropDownList ddl)
        {
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataTextField = "MZ_KCHI";
            ddl.DataSource = o_DBFactory.ABC_toTest.Create_Table("SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='11' ORDER BY MZ_KCODE) WHERE 1=1", "TAG");
            ddl.DataBind();

            ListItem li = new ListItem("", "");
            ddl.Items.Insert(0, li);
        }

        #endregion

                      

        /// <summary>
        /// 取得立帳局號，銀行委託代號
        /// </summary>
        /// <param name="strPAY_AD">發薪機關代碼</param>
        /// <param name="strBANK_ID">銀行代碼</param>
        /// <returns>立帳局號</returns>
        public string getFianceno(string strPAY_AD, string strBANK_ID)
        {
            string strSQL;
            DataTable dtFianceno;
            strSQL = string.Format(@"SELECT FIANCENO FROM B_BRANCH BB JOIN B_BRANCH_BANK BBB ON BB.B_SNID=BBB.B_SNID 
                                    JOIN B_BANK ON B_BANK.B_SNID = BBB.BANK_ID 
                                    JOIN B_BANK_LIST ON B_BANK.ID = B_BANK_LIST.BANK_ID WHERE BB.ID='{0}' AND B_BANK_LIST.BANK_ID='{1}'", strPAY_AD, strBANK_ID);

            dtFianceno = o_DBFactory.ABC_toTest.Create_Table(strSQL, "FIANCENO");
            if (dtFianceno.Rows.Count == 0)
            {
                return "";
            }
            return dtFianceno.Rows[0]["FIANCENO"].ToString();
        }

        

        /// <summary>
        /// 取得受託局名
        /// </summary>
        /// <param name="strPAY_AD">發薪機關代碼</param>
        /// <param name="strBANK_ID">銀行代碼</param>
        /// <returns>受託局名</returns>
        public string getBankName(string strPAY_AD, string strBANK_ID)
        {
            string strSQL;
            DataTable dtBankName = new DataTable();
            strSQL = string.Format(@"SELECT NAME FROM B_BRANCH BB JOIN B_BRANCH_BANK BBB ON BB.B_SNID=BBB.B_SNID 
                                    JOIN B_BANK ON B_BANK.B_SNID = BBB.BANK_ID 
                                    JOIN B_BANK_LIST ON B_BANK.ID = B_BANK_LIST.BANK_ID WHERE BB.ID='{0}' AND B_BANK_LIST.BANK_ID='{1}'", strPAY_AD, strBANK_ID);

            dtBankName = o_DBFactory.ABC_toTest.Create_Table(strSQL, "NAME");
            if (dtBankName.Rows.Count == 0)
            {
                return "";
            }
            return dtBankName.Rows[0]["NAME"].ToString();
        }

      

       

       
        #region 健保費

        /// <summary>
        /// 取得各健保狀態之人數並加入至參考之List中
        /// </summary>
        /// <param name="lstMember">參考之List</param>
        /// <param name="strMZ_ID">對象之身分證字號</param>
        /// <param name="intInsuranceMode">請設定為4</param>
        public void getInsuranceMemberCount(ref List<int> lstMember, string strMZ_ID, int intInsuranceMode)
        {
            //健保模式為0則跳出此函式
            if (intInsuranceMode == 0)
                return;

            int intMemberCount;

            //取得InsuranceMode的加保人數
            intMemberCount = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_FAMILY WHERE MZ_ID='" + strMZ_ID + "' AND MZ_ISINSURANCE='Y' AND MZ_INSURANCEMODE='" + intInsuranceMode + "'"));

            //將加保人數加入至lstMember中
            lstMember.Add(intMemberCount);

            //遞迴，每次將健保模式-1
            getInsuranceMemberCount(ref lstMember, strMZ_ID, intInsuranceMode - 1);
        }

      

        /// <summary>
        /// 取得個人健保費級距金額(已乘以0.9352)
        /// </summary>
        /// <param name="strMZ_ID">對象之身分證字號</param>
        /// <returns>個人健保費級距金額</returns>
        public int getHealthInsurancebyMZ_ID(string strMZ_ID)
        {
            DataTable dtBase = new DataTable();

            //健保費金額
            int intInsurance;
            //職稱
            string strMZ_OCCC;
            //現俸職等
            string strMZ_SRANK;
            //俸點
            string strMZ_SPT;
            //薪俸編號
            int intSALARY_NAME1;
            //判斷使用俸點或俸階對照薪俸級距表，1為俸點、2為俸階
            int intSALARY_int1_2;

            //薪俸對照編號
            int intS_SNID;
            //薪俸月支數額
            int intSALARYPAY1;
            //主管加給
            int intBoss;
            //專業加給
            int intProfess;
            //警勤加給
            int intWorkp;
            //技術加給
            int intTechnics;
            //工作獎助金
            int intBonus;
            //外事加給
            int intAdventive;
            //偏遠加給
            int intFar;
            //水電補助
            int intElectric;

            //取得職稱、現俸職等、俸點資料
            dtBase = o_DBFactory.ABC_toTest.Create_Table("SELECT mz_occc, MZ_SRANK, MZ_SPT, mz_pchief FROM A_DLBASE where MZ_ID = '" + strMZ_ID + "'", "B_Base");

            //如果有資料才計算健保費，否則傳回0
            if (dtBase.Rows.Count > 0)
            {
                strMZ_OCCC = dtBase.Rows[0]["MZ_OCCC"].ToString().Trim();
                strMZ_SRANK = dtBase.Rows[0]["MZ_SRANK"].ToString().Trim();
                strMZ_SPT = dtBase.Rows[0]["MZ_SPT"].ToString().Trim();

                //依職稱取得對照B_SALARY中的NAME1群組
                intSALARY_NAME1 = int.Parse(SalaryPublic.getSalaryName1(strMZ_OCCC));

                //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
                //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
                intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(strMZ_SRANK);

                //判斷是否任主管，並計算加給金額
                if (!String.IsNullOrEmpty(dtBase.Rows[0]["mz_pchief"].ToString()))
                {
                    intBoss = SalaryBasic.intBossPAY_byID(strMZ_ID);
                }
                else
                {
                    intBoss = 0;
                }

                intS_SNID = SalaryPublic.intSALARY_ID_Data_Serach(intSALARY_NAME1, strMZ_SPT, intSALARY_int1_2);
                intSALARYPAY1 = SalaryMonth.intOrigin_PAY_Data_Serach(intS_SNID);
                intProfess = SalaryBasic.intProfess_PAY_Data_Serach(strMZ_SRANK);
                intWorkp = intWorkp_Data(strMZ_ID);
                intTechnics = intTechnics_Data(strMZ_ID);
                intBonus = intBonus_Data(strMZ_ID);
                intAdventive = intAdventive_Data(strMZ_ID);
                intFar = intFar_Data(strMZ_ID);
                intElectric = intElectric_Data(strMZ_ID);

                intInsurance = intRound_HEALTH_PAY_CHK_Data(intSALARYPAY1 + intBoss + intProfess + intWorkp + intTechnics + intBonus + intAdventive + intFar + intElectric, 1);
            }
            else
            {
                return 0;
            }

            return intInsurance;
        }

        

        #endregion

        #region 薪資基本資料B_BASE

        public static int getSalaryOriginType(string strMZ_SRANK)
        {
            //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
            //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
            if (strMZ_SRANK.StartsWith("G"))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public static string getSalaryName1(string strMZ_OCCC)
        {
            //B_SALARY中的NAME1欄位，3代表約雇、臨時人員
            //B_SALARY中的NAME1欄位，2代表司機、工友、技工
            //B_SALARY中的NAME1欄位，1代表警察、公務人員
            switch (strMZ_OCCC)
            {
                case "Z011":
                case "Z013":
                case "1179":
                    return "3";

                case "Z014":
                case "Z015":
                case "Z016":
                    return "2";

                default:
                    return "1";
            }
        }

        public static string getInsuranceGroup(string strMZ_OCCC)
        {
            //2表示勞保、1表示公保
            //只有警察、公務人員採用公保
            switch (strMZ_OCCC)
            {
                //約雇、臨時人員
                case "0011":
                case "0013":
                case "1179":
                    return "2";

                //司機、工友、技工
                case "0014":
                case "0015":
                case "0016":
                    return "2";

                //其餘為警察、公務人員
                default:
                    return "1";
            }
        }

      

        /// <summary>
        /// 警勤加給 - 個人
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        private static string strWorkp_MZ_ID_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT WORKP FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["WORKP"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 警勤加給 - 金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intWorkp_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_WORKP WHERE \"ID\" = '" + strWorkp_MZ_ID_Data(strMZ_ID) + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 技術加給 - 個人
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        private static string strTechnics_MZ_ID_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT TECHNICS FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["TECHNICS"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 技術加給 - 金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intTechnics_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_TECHNICS WHERE \"ID\" = '" + strTechnics_MZ_ID_Data(strMZ_ID) + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 工作獎助金 - 個人
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        private static string strBonus_MZ_ID_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT BONUS FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["BONUS"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 工作獎助金 - 金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intBonus_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_BONUS WHERE \"ID\" = '" + strBonus_MZ_ID_Data(strMZ_ID) + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 外事加給 - 個人
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        private static string strAdventive_MZ_ID_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT ADVENTIVE FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["ADVENTIVE"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 外事加給 - 金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intAdventive_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_ADVENTIVE WHERE \"ID\" = '" + strAdventive_MZ_ID_Data(strMZ_ID) + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 偏遠加給 - 個人
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        private static string strFar_MZ_ID_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT FAR FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["FAR"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 偏遠加給 - 金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intFar_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_FAR WHERE \"ID\" = '" + strFar_MZ_ID_Data(strMZ_ID) + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 水電補助 - 個人
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        private static string strElectric_MZ_ID_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT ELECTRIC FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {

                        return dt.Rows[0]["ELECTRIC"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 水電補助 - 金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intElectric_Data(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_ELECTRIC WHERE \"ID\" = '" + strElectric_MZ_ID_Data(strMZ_ID) + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        
       

       

        #endregion

        /// <summary>
        /// 檢查是否符合民國7碼日期
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsValidRepulicDate(string strDate)
        {
            if (strDate.Length == 0)
                return true;

            if (strDate.Length != 7)
                return false;// 長度不是7

            int intYear = int.Parse(strDate.Substring(0, 3));
            int intMonth = int.Parse(strDate.Substring(3, 2));

            if (intMonth > 12)
                return false;// 月份大於12

            if (int.Parse(strDate.Substring(5, 2)) > System.DateTime.DaysInMonth(intYear + 1911, intMonth))
                return false;// 日期大於當月應有的日數

            return true;
        }

        /// <summary>
        /// 檢查是否符合考績等級(只能是甲乙丙)
        /// </summary>
        /// <param name="strGrade"></param>
        /// <returns></returns>
        public static bool IsValidGrade(string strGrade)
        {
            if (strGrade != "甲" && strGrade != "乙" && strGrade != "丙" && strGrade != "")
                return false;

            return true;
        }

        /// <summary>
        /// 指定 目標容器中 控制項的開或關
        /// </summary>
        /// <param name="Pl">容器</param>
        /// <param name="sw">開/關</param>
        public static void changeControlItemsEnabled(ref Panel Pl, Boolean sw)
        {
            foreach (Object ob in Pl.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;
                    tbox.Enabled = sw;
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;
                    ddlist.Enabled = sw;
                }
                else if (ob is RadioButton)
                {
                    RadioButton rd = (RadioButton)ob;
                    rd.Enabled = sw;
                }
                else if (ob is RadioButtonList)
                {
                    RadioButtonList rd = (RadioButtonList)ob;
                    rd.Enabled = sw;
                }
                else if (ob is ImageButton)
                {
                    ImageButton IB = (ImageButton)ob;
                    IB.Enabled = sw;
                }
            }
        }

        /// <summary>
        /// 去年民國3碼年
        /// </summary>
        /// <returns>去年民國3碼年</returns>
        public static string strRepublicLastYear()
        {
            string strYear;
            strYear = Convert.ToString((Convert.ToInt16(DateTime.Today.Year.ToString()) - 1911 - 1)).PadLeft(3, '0');

            return strYear;
        }

        /// <summary>
        /// 今年民國3碼年
        /// </summary>
        /// <returns>今年民國3碼年</returns>
        public static string strRepublicYear()
        {
            string strYear;
            strYear = Convert.ToString((Convert.ToInt16(DateTime.Today.Year.ToString()) - 1911)).PadLeft(3, '0');

            return strYear;
        }

        /// <summary>
        /// 取得現在民國年月5碼
        /// </summary>
        /// <returns>現在民國年月5碼</returns>
        public static string strRepublicYearMonth()
        {
            string strYear, strMonth, strDate;

            strYear = Convert.ToString((Convert.ToInt16(DateTime.Today.Year.ToString()) - 1911)).PadLeft(3, '0');
            strMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
            strDate = strYear + strMonth;

            return strDate;
        }

        /// <summary>
        /// 現在7碼民國年月日
        /// </summary>
        /// <returns>7碼民國年月日</returns>
        public static string strRepublicDate()
        {
            string strYear, strMonth, strDate;
            strYear = Convert.ToString((Convert.ToInt16(DateTime.Today.Year.ToString()) - 1911)).PadLeft(3, '0');
            strMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
            strDate = DateTime.Today.Day.ToString().PadLeft(2, '0');

            return strYear + strMonth + strDate;
        }

        /// <summary>
        /// 現在8碼西元年月日
        /// </summary>
        /// <returns>8碼西元年月日</returns>
        public static string strADDate()
        {
            string strYear, strMonth, strDate;
            strYear = DateTime.Today.Year.ToString();
            strMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
            strDate = DateTime.Today.Day.ToString().PadLeft(2, '0');

            return strYear + strMonth + strDate;
        }

        /// <summary>
        /// 將民國7碼年月日轉為西元8碼年月日
        /// </summary>
        /// <returns>8碼西元年月日</returns>
        public static string ToADDate(string strDate)
        {
            string strYear, strMonth, strDay;
            strYear = (int.Parse(strDate.Substring(0, 3)) + 1911).ToString();
            strMonth = strDate.Substring(3, 2);
            strDay = strDate.Substring(5, 2);

            return strYear + strMonth + strDay;
        }

        /// <summary>
        /// 將民國7碼年月日轉為以-分隔年月日的西元8碼年月日
        /// </summary>
        /// <returns>8碼西元年月日</returns>
        public static string ToADDateWithDash(string strDate)
        {
            if (strDate.Length != 7)
                return "2001-01-01";

            string strYear, strMonth, strDay;
            strYear = (int.Parse(strDate.Substring(0, 3)) + 1911).ToString();
            strMonth = strDate.Substring(3, 2);
            strDay = strDate.Substring(5, 2);

            return strYear + "-" + strMonth + "-" + strDay;
        }

       
      
      

        /// <summary>
        /// 將貨幣字元轉整數
        /// </summary>
        /// <param name="strWords_Data">貨幣字元</param>
        /// <returns></returns>
        public static int intdelimiterChars(string strWords_Data)
        {
            if (strWords_Data == "")
            {
                strWords_Data = "0";
            }
            return int.Parse(strWords_Data.Replace(",", string.Empty).Replace("$", string.Empty));
        }

        /// <summary>
        /// 將整數轉貨幣字元
        /// </summary>
        /// <param name="money">金額</param>
        /// <returns></returns>
        public static string strMoneyFormat(string money)
        {
            // 感覺上好像不需要特定用成$加,的金錢格式，先直接return
            return money;
           /*
            if (money == "")
            {
                return "$0";
            }
            return String.Format("{0:$#,#0}", Convert.ToInt32(money));*/
        }

        /// <summary>
        /// 資料轉整數，空值傳回0
        /// </summary>
        /// <param name="money">金額資料</param>
        /// <returns></returns>
        public static int intMoneyDatatoInt(string money)
        {
            int intMoney=0;
            
            int.TryParse(money, out intMoney);
            return intMoney;
            
          
        }

        /// <summary>
        /// 計算公式，將小數點第二位捨去後，無條件進位至整數。
        /// </summary>
        /// <param name="decimalMathematics_Data">計算數字</param>
        /// <returns></returns>
        public static int intRound_Data(double doubleMathematics_Data)
        {
            string strData;

            strData = doubleMathematics_Data.ToString("0.00");
            // 捨去小數點第二位後的資料
            strData = strData.Substring(0, strData.IndexOf('.') + 1);


            return Convert.ToInt32(Math.Ceiling(double.Parse(strData)));
        }

       
        

        /// <summary>
        /// 取得健保費乘以0.9352後之健保級距金額
        /// </summary>
        /// <param name="intInsurance_PAY_Data">個人健保金額</param>
        /// <returns>健保級距金額</returns>
        public static int intRound_HEALTH_PAY_Data(int intInsurance_PAY_Data)
        {
            //總額(無條件捨去) = 應發金額 * 93.52% (原 90.67%)
            double doubleTotal_Data = Math.Floor(intInsurance_PAY_Data * 0.9352);
            return int.Parse(doubleTotal_Data.ToString());
        }

        /// <summary>
        /// 健保費計算，比對健保費級距表
        /// </summary>
        /// <param name="intRound_HEALTH_PAY">健保級距金額</param>
        /// <param name="intIN">比對級距</param>
        /// <returns></returns>
        public static int intRound_HEALTH_PAY_CHK_Data(int intRound_HEALTH_PAY, int intIN)
        {
            if (intRound_HEALTH_PAY <= 0)
            {
                return 0;
            }
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "";
                    if (intIN == 1)
                    {
                        strSQL = "SELECT INSURANCE FROM B_HEALTH_INSURANCE WHERE PAY1 <= " + intRound_HEALTH_PAY_Data(intRound_HEALTH_PAY) + " AND PAY2 >= " + intRound_HEALTH_PAY_Data(intRound_HEALTH_PAY) + "";
                    }
                    else
                    {
                        strSQL = "SELECT INSURANCE FROM (SELECT B_HEALTH_INSURANCE.*,(RANK() OVER(ORDER BY PAY2 DESC)) AS PAY2_DESC FROM B_HEALTH_INSURANCE  ORDER BY PAY2 DESC) WHERE PAY2_DESC=1";
                    }
                    DataTable dt = new DataTable();
                    SqlCommand Selectcmd = new SqlCommand(strSQL, SelectConn);
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    { return int.Parse(dt.Rows[0]["INSURANCE"].ToString()); }
                    else
                    { return intRound_HEALTH_PAY_CHK_Data(intRound_HEALTH_PAY, 2); }
                }
                catch { throw; }
                finally { SelectConn.Close();
                //XX2013/06/18 
                SelectConn.Dispose();
                }
            }
        }

        
       

        

        /// <summary>
        /// 找人事資料的欄位值
        /// </summary>
        /// <param name="Column">欄位名稱</param>
        /// <param name="type">1 身分證號 2 員工編號</param>
        /// <param name="Where_Column">WHERE 的欄位值</param>
        /// <returns></returns>
        public static string str_A_Column(string Column,int type, string Where_Column_val)
        {
            string strSQL = string.Format("SELECT {0} FROM A_DLBASE WHERE {1} = '{2}'", Column, type == 1 ? "MZ_ID" : "MZ_POLNO", Where_Column_val);
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "PAY_AD");
            if (dt.Rows.Count > 0)
                return dt.Rows[0][Column].ToString();
            return "";
        }
        
             
             

        /// <summary>
        /// 查詢薪俸主編號
        /// </summary>
        /// <param name="strNAME1">項目</param>
        /// <param name="strORIGIN1_2">俸點(點)或俸級(元)</param>
        /// <param name="int1_2">1為俸點(點)，2為俸級(元)</param>
        /// <returns></returns>
        public static int intSALARY_ID_Data_Serach(int intNAME1, string strORIGIN1_2, int int1_2)
        {
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "";
                    strORIGIN1_2 = strORIGIN1_2.PadLeft(4, '0');
                    if (int1_2 == 2)
                    {
                        strSQL = "SELECT S_SNID FROM B_SALARY WHERE NAME1 = '" + intNAME1 + "' AND ORIGIN2 = '" + strORIGIN1_2 + "'";
                    }
                    else
                    {
                        strSQL = "SELECT S_SNID FROM B_SALARY WHERE NAME1 = '" + intNAME1 + "' AND ORIGIN1 = '" + strORIGIN1_2 + "'";
                    }

                    SqlCommand Selectcmd = new SqlCommand(strSQL, SelectConn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["S_SNID"].ToString());
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch { return 0; }
                finally { SelectConn.Close();
                //XX2013/06/18 
                SelectConn.Dispose();
                }
            }
        }

        
        /// <summary>
        /// 結卡旗標鎖定 - 回傳西元年份(系統自動鎖定前二個月資料)
        /// </summary>
        /// <param name="strYEAR_Data">民國年份</param>
        /// <param name="strMONTH_Data">月份</param>
        /// <returns></returns>
        public static string strLOCKDB_YEAR(string strYEAR_Data, string strMONTH_Data)
        {
            int intYEAR = int.Parse(strYEAR_Data) + 1911;
            int intMONTH = int.Parse(strMONTH_Data) - 2;
            int intYEAR_Data = 0;
            if (intMONTH <= 0)
            {
                return (intYEAR_Data = intYEAR - 1).ToString("0000");
            }
            else
            {
                return (intYEAR_Data = intYEAR).ToString("0000");
            }
        }

        /// <summary>
        /// 結卡旗標鎖定 - 回傳西元月份(系統自動鎖定前二個月資料)
        /// </summary>
        /// <param name="strMONTH_Data">月份</param>
        /// <returns></returns>
        public static string strLOCKDB_MONTH(string strMONTH_Data)
        {
            int intMONTH = int.Parse(strMONTH_Data) - 2;
            int intMONTH_Data = 0;
            if (intMONTH <= 0)
            {
                return (intMONTH_Data = intMONTH + 12).ToString("00");
            }
            else
            {
                return (intMONTH_Data = intMONTH).ToString("00");
            }
        }

        /// <summary>
        /// 判斷字串是否為Big5編碼文字
        /// </summary>
        /// <param name="word">判斷字串</param>
        /// <returns></returns>
        public static bool IsBig5Code(string word)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254))) //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
