using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    [Serializable]
    public class Monthpay
    {
        public int sn;
        public string payad;
        public string payadName;
        public string unit;
        public string unitName;
        public string polno;
        public string idcard;
        public string name;
        public string occc;
        public string srank;
        public string slvc;
        public string spt;
        public string spt1;
        public string amonth;
        public int salary;
        public int work;
        public int profess;
        public int boss;
        public int tech;
        public int bonus;
        public int adventive;
        public int far;
        public int electric;
        public int otheradd;
        public int insurance;
        public int healthPersonal;
        public int healthMan;
        public int health;
        public int healthpay1;
        public int monthpayTax;
        public int monthpay;
        public int concur3;
        public int tax;
        public int otherminus;
        public int extra01;
        public int extra02;
        public int extra03;
        public int extra04;
        public int extra05;
        public int extra06;
        public int extra07;
        public int extra08;
        public int extra09;
        public string note;
        public bool lockdb;

        public Monthpay(int sn)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            sql = "SELECT * FROM VW_ALL_SALARY_DATA WHERE MM_SNID=@MM_SNID";
            ops.Add(new SqlParameter("MM_SNID", sn));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;

            this.sn = sn;
            payad = dt.Rows[0]["PAY_AD"].ToString();
            payadName = dt.Rows[0]["CHIAD"].ToString();
            unit = dt.Rows[0]["MZ_UNIT"].ToString();
            unitName = dt.Rows[0]["PAY_UNIT"].ToString();
            polno = dt.Rows[0]["MZ_POLNO"].ToString();
            idcard = dt.Rows[0]["IDNO"].ToString();
            name = dt.Rows[0]["NAME"].ToString();
            occc = dt.Rows[0]["MZ_OCCC"].ToString();
            srank = dt.Rows[0]["MZ_SRANK"].ToString();
            slvc = dt.Rows[0]["MZ_SLVC"].ToString();
            spt = dt.Rows[0]["MZ_SPT"].ToString();
            spt1 = dt.Rows[0]["MZ_SPT1"].ToString();
            amonth = dt.Rows[0]["AMONTH"].ToString();
            salary = int.Parse(dt.Rows[0]["SALARYPAY1"].ToString());
            work = int.Parse(dt.Rows[0]["WORKP"].ToString());
            profess = int.Parse(dt.Rows[0]["PROFESS"].ToString());
            boss = int.Parse(dt.Rows[0]["BOSS"].ToString());
            tech = int.Parse(dt.Rows[0]["TECHNICS"].ToString());
            bonus = int.Parse(dt.Rows[0]["BONUS"].ToString());
            adventive = int.Parse(dt.Rows[0]["ADVENTIVE"].ToString());
            far = int.Parse(dt.Rows[0]["FAR"].ToString());
            electric = int.Parse(dt.Rows[0]["ELECTRIC"].ToString());
            otheradd = int.Parse(dt.Rows[0]["OTHERADD"].ToString());

            insurance = int.Parse(dt.Rows[0]["INSURANCEPAY"].ToString());
            healthPersonal = int.Parse(dt.Rows[0]["HEALTHID"].ToString());
            healthMan = int.Parse(dt.Rows[0]["HEALTHMAN"].ToString());
            health = int.Parse(dt.Rows[0]["HEALTHPAY"].ToString());
            healthpay1 = int.Parse(dt.Rows[0]["HEALTHPAY1"].ToString());
            monthpayTax = int.Parse(dt.Rows[0]["MONTHPAY_TAX"].ToString());
            monthpay = int.Parse(dt.Rows[0]["MONTHPAY"].ToString());
            concur3 = int.Parse(dt.Rows[0]["CONCUR3PAY"].ToString());
            tax = int.Parse(dt.Rows[0]["TAX"].ToString());
            otherminus = int.Parse(dt.Rows[0]["OTHERMINUS"].ToString());

            extra01 = int.Parse(dt.Rows[0]["EXTRA01"].ToString());
            extra02 = int.Parse(dt.Rows[0]["EXTRA02"].ToString());
            extra03 = int.Parse(dt.Rows[0]["EXTRA03"].ToString());
            extra04 = int.Parse(dt.Rows[0]["EXTRA04"].ToString());
            extra05 = int.Parse(dt.Rows[0]["EXTRA05"].ToString());
            extra06 = int.Parse(dt.Rows[0]["EXTRA06"].ToString());
            extra07 = int.Parse(dt.Rows[0]["EXTRA07"].ToString());
            extra08 = int.Parse(dt.Rows[0]["EXTRA08"].ToString());
            extra09 = int.Parse(dt.Rows[0]["EXTRA09"].ToString());
            note = dt.Rows[0]["NOTE"].ToString();

            if (dt.Rows[0]["LOCKDB"].ToString() == "Y")
                lockdb = true;
            else
                lockdb = false;
        }

        public void update(int salary, int work, int profess, int boss, int technics, int bonus,
            int adventive, int far, int electric, int otheradd,
            int insurance, int healthPersonal, int healthman, int healthpay,
            int healthpay1, int monthpayTax, int monthpay, int concur3, int tax, int otherminus,
            int extra01, int extra02, int extra03, int extra04, int extra05, int extra06,
            int extra07, int extra08, int extra09, string note)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"UPDATE B_MONTHPAY_MAIN SET  
                    SALARYPAY1=@SALARYPAY1, WORKP=@WORKPPAY, PROFESS=@PROFESSPAY, BOSS=@BOSSPAY, TECHNICS=@TECHNICSPAY, BONUS=@BONUSPAY, ADVENTIVE=@ADVENTIVEPAY, FAR=@FARPAY, ELECTRIC=@ELECTRICPAY, OTHERADD=@OTHERADD, 
                    INSURANCEPAY=@INSURANCEPAY, HEALTHID=@HEALTHID, HEALTHMAN=@HEALTHMAN, HEALTHPAY=@HEALTHPAY, HEALTHPAY1=@HEALTHPAY1, MONTHPAY_TAX=@MONTHPAY_TAX, MONTHPAY=@MONTHPAY, CONCUR3PAY=@CONCUR3PAY, TAX=@TAX, OTHERMINUS=@OTHERMINUS, 
                    EXTRA01=@EXTRA01, EXTRA02=@EXTRA02, EXTRA03=@EXTRA03, EXTRA04=@EXTRA04, EXTRA05=@EXTRA05, EXTRA06=@EXTRA06, EXTRA07=@EXTRA07, EXTRA08=@EXTRA08, EXTRA09=@EXTRA09, NOTE=@NOTE, LASTDA=@LASTDA, CREATOR=@CREATOR WHERE MM_SNID=@MM_SNID";
            ops.Add(new SqlParameter("SALARYPAY1", salary));
            ops.Add(new SqlParameter("WORKPPAY", work));
            ops.Add(new SqlParameter("PROFESSPAY", profess));
            ops.Add(new SqlParameter("BOSSPAY", boss));
            ops.Add(new SqlParameter("TECHNICSPAY", tech));
            ops.Add(new SqlParameter("BONUSPAY", bonus));
            ops.Add(new SqlParameter("ADVENTIVEPAY", adventive));
            ops.Add(new SqlParameter("FARPAY", far));
            ops.Add(new SqlParameter("ELECTRICPAY", electric));
            ops.Add(new SqlParameter("OTHERADD", otheradd));
            ops.Add(new SqlParameter("INSURANCEPAY", insurance));
            ops.Add(new SqlParameter("HEALTHID", healthPersonal));
            ops.Add(new SqlParameter("HEALTHMAN", healthman));
            ops.Add(new SqlParameter("HEALTHPAY", healthpay));
            ops.Add(new SqlParameter("HEALTHPAY1", healthpay1));
            ops.Add(new SqlParameter("MONTHPAY_TAX", monthpayTax));
            ops.Add(new SqlParameter("MONTHPAY", monthpay));
            ops.Add(new SqlParameter("CONCUR3PAY", concur3));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("OTHERMINUS", otherminus));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("EXTRA02", extra02));
            ops.Add(new SqlParameter("EXTRA03", extra03));
            ops.Add(new SqlParameter("EXTRA04", extra04));
            ops.Add(new SqlParameter("EXTRA05", extra05));
            ops.Add(new SqlParameter("EXTRA06", extra06));
            ops.Add(new SqlParameter("EXTRA07", extra07));
            ops.Add(new SqlParameter("EXTRA08", extra08));
            ops.Add(new SqlParameter("EXTRA09", extra09));
            ops.Add(new SqlParameter("NOTE", note));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));
            ops.Add(new SqlParameter("MM_SNID", this.sn));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        public void delete()
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "DELETE B_MONTHPAY_MAIN WHERE MM_SNID=@MM_SNID";
            ops.Add(new SqlParameter("MM_SNID", this.sn));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        // 產生每月薪資資料
        public static DataTable createMonthpay(string amonth, string payad)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dtError = new DataTable();

            dtError.Columns.Add("Name");
            dtError.Columns.Add("Polno");

            // 先把已存在的資料刪除
            sql = "DELETE B_MONTHPAY_MAIN WHERE PAY_AD=@PAY_AD AND AMONTH=@AMONTH AND LOCKDB='N' ";
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("AMONTH", amonth));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            // 開始建立資料
            sql = "select * from VW_ALL_BASE_DATA where PAY_AD=@PAY_AD ORDER BY MZ_POLNO";
            ops.Clear();
            ops.Add(new SqlParameter("PAY_AD", payad));

            foreach (DataRow item in o_DBFactory.ABC_toTest.DataSelect(sql, ops).Rows)
            {
                // 從薪資基本資料取資料填入每月薪資資料
                string strMZ_ID;
                string strPOLNO;
                string strPAY_AD;
                string strName;
                string strOCCC;
                string strRank;
                string strSLVC;
                string strSPT;
                string strSPT1;
                string strUnit;
                string strNote;
                int intBoss;
                int intProfess;
                int intHealthID;
                int intHealth;
                int intInsurance;
                int intTech;
                int intBonus;
                int intWork;
                int intAdven;
                int intFar;
                int intElec;
                int intSalary;
                int intOtherAdd;
                int intMonthTax;
                int intMonth;
                int intConcur3;
                int intExtra01;
                int intExtra02;
                int intExtra03;
                int intExtra04;
                int intExtra05;
                int intExtra06;
                int intExtra07;
                int intExtra08;
                int intExtra09;
                int intDEHealth;
                int intTaxper;
                int otherminus;
                //2016-10-12 matthew 新增 編制機關的欄位 MZ_AD 小隊長新增功能
                string strMZ_AD;

                strMZ_ID = item["MZ_ID"].ToString();
                strPOLNO = item["MZ_POLNO"].ToString();
                strPAY_AD = item["PAY_AD"].ToString();
                strName = item["MZ_NAME"].ToString();
                strOCCC = item["MZ_OCCC"].ToString();
                strRank = item["MZ_SRANK"].ToString();
                strSLVC = item["MZ_SLVC"].ToString();
                strSPT = item["MZ_SPT"].ToString();
                strSPT1 = item["MZ_SPT1"].ToString();
                strUnit = strPOLNO.Length >= 4 ? strPOLNO.Substring(0, 4) : "";
                strNote = item["NOTE"].ToString();
                intBoss = int.Parse(item["BossPay"].ToString());
                intProfess = int.Parse(item["ProfessPay"].ToString());
                intHealthID = int.Parse(item["PERSONALHEALTHPAY"].ToString());
                intHealth = int.Parse(item["HealthPay"].ToString());
                intInsurance = int.Parse(item["InsurancePay"].ToString());
                intTech = int.Parse(item["TechnicsPay"].ToString());
                intBonus = int.Parse(item["BonusPay"].ToString());
                intWork = int.Parse(item["WorkPPay"].ToString());
                intAdven = int.Parse(item["AdventivePay"].ToString());
                intFar = int.Parse(item["FarPay"].ToString());
                intElec = int.Parse(item["ElectricPay"].ToString());
                intSalary = int.Parse(item["SalaryPay"].ToString());
                intOtherAdd = int.Parse(item["OtherAdd"].ToString());
                intMonthTax = int.Parse(item["MonthPay_Tax"].ToString());
                intMonth = int.Parse(item["MonthPay"].ToString());
                intConcur3 = int.Parse(item["Concur3Pay"].ToString());
                intExtra01 = int.Parse(item["Extra01"].ToString());
                intExtra02 = int.Parse(item["Extra02"].ToString());
                intExtra03 = int.Parse(item["Extra03"].ToString());
                intExtra04 = int.Parse(item["Extra04"].ToString());
                intExtra05 = int.Parse(item["Extra05"].ToString());
                intExtra06 = int.Parse(item["Extra06"].ToString());
                intExtra07 = int.Parse(item["Extra07"].ToString());
                intExtra08 = int.Parse(item["Extra08"].ToString());
                intExtra09 = int.Parse(item["Extra09"].ToString());
                intDEHealth = int.Parse(item["DE_HEALTH"].ToString());
                intTaxper = int.Parse(item["TAXPER"].ToString());
                otherminus = int.Parse(item["OTHERMINUS"].ToString());
                strMZ_AD = item["MZ_AD"].ToString();

                if (!insertData(strMZ_ID, amonth, strPOLNO, strPAY_AD, strName, strOCCC, strRank, strSLVC, strSPT, strSPT1,
                    "N", intSalary, intWork, intProfess, intBoss, intTech, intBonus, intAdven, intFar, intElec, intOtherAdd,
                    intInsurance, intHealthID, intDEHealth, intHealth, 0, intMonthTax, intMonth, intConcur3, intTaxper,
                    intExtra01, intExtra02, intExtra03, intExtra04, intExtra05, intExtra06, intExtra07, intExtra08, intExtra09, otherminus, strNote, strMZ_AD))
                {
                    DataRow dr = dtError.NewRow();

                    dr["Name"] = strName;
                    dr["Polno"] = strPOLNO;

                    dtError.Rows.Add(dr);
                }
            }

            return dtError;
        }
        public static DataTable createMonthpay(string amonth, string payad, string idcard)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dtError = new DataTable();

            dtError.Columns.Add("Name");
            dtError.Columns.Add("Polno");

            // 先把已存在的資料刪除
            sql = "DELETE B_MONTHPAY_MAIN WHERE PAY_AD=@PAY_AD AND AMONTH=@AMONTH AND IDCARD=@IDCARD";
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("AMONTH", amonth));
            ops.Add(new SqlParameter("IDCARD", idcard));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            // 開始建立資料
            sql = "select * from VW_ALL_BASE_DATA where PAY_AD=@PAY_AD AND MZ_ID=@MZ_ID ORDER BY MZ_POLNO";
            ops.Clear();
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("MZ_ID", idcard));

            foreach (DataRow item in o_DBFactory.ABC_toTest.DataSelect(sql, ops).Rows)
            {
                // 從薪資基本資料取資料填入每月薪資資料
                string strMZ_ID;
                string strPOLNO;
                string strPAY_AD;
                string strName;
                string strOCCC;
                string strRank;
                string strSLVC;
                string strSPT;
                string strSPT1;
                string strNote;
                int intBoss;
                int intProfess;
                int intHealthID;
                int intHealth;
                int intInsurance;
                int intTech;
                int intBonus;
                int intWork;
                int intAdven;
                int intFar;
                int intElec;
                int intSalary;
                int intOtherAdd;
                int intMonthTax;
                int intMonth;
                int intConcur3;
                int intExtra01;
                int intExtra02;
                int intExtra03;
                int intExtra04;
                int intExtra05;
                int intExtra06;
                int intExtra07;
                int intExtra08;
                int intExtra09;
                int intDEHealth;
                int intTaxper;
                int otherminus;
                //2016-10-12 matthew 新增 編制機關的欄位 MZ_AD 小隊長新增功能
                string strMZ_AD;
                strMZ_ID = item["MZ_ID"].ToString();
                strPOLNO = item["MZ_POLNO"].ToString();
                strPAY_AD = item["PAY_AD"].ToString();
                strName = item["MZ_NAME"].ToString();
                strOCCC = item["MZ_OCCC"].ToString();
                strRank = item["MZ_SRANK"].ToString();
                strSLVC = item["MZ_SLVC"].ToString();
                strSPT = item["MZ_SPT"].ToString();
                strSPT1 = item["MZ_SPT1"].ToString();
                strNote = item["NOTE"].ToString();
                intBoss = int.Parse(item["BossPay"].ToString());
                intProfess = int.Parse(item["ProfessPay"].ToString());
                intHealthID = int.Parse(item["PERSONALHEALTHPAY"].ToString());
                intHealth = int.Parse(item["HealthPay"].ToString());
                intInsurance = int.Parse(item["InsurancePay"].ToString());
                intTech = int.Parse(item["TechnicsPay"].ToString());
                intBonus = int.Parse(item["BonusPay"].ToString());
                intWork = int.Parse(item["WorkPPay"].ToString());
                intAdven = int.Parse(item["AdventivePay"].ToString());
                intFar = int.Parse(item["FarPay"].ToString());
                intElec = int.Parse(item["ElectricPay"].ToString());
                intSalary = int.Parse(item["SalaryPay"].ToString());
                intOtherAdd = int.Parse(item["Otheradd"].ToString());
                intMonthTax = int.Parse(item["MonthPay_Tax"].ToString());
                intMonth = int.Parse(item["MonthPay"].ToString());
                intConcur3 = int.Parse(item["Concur3Pay"].ToString());
                intExtra01 = int.Parse(item["Extra01"].ToString());
                intExtra02 = int.Parse(item["Extra02"].ToString());
                intExtra03 = int.Parse(item["Extra03"].ToString());
                intExtra04 = int.Parse(item["Extra04"].ToString());
                intExtra05 = int.Parse(item["Extra05"].ToString());
                intExtra06 = int.Parse(item["Extra06"].ToString());
                intExtra07 = int.Parse(item["Extra07"].ToString());
                intExtra08 = int.Parse(item["Extra08"].ToString());
                intExtra09 = int.Parse(item["Extra09"].ToString());
                intDEHealth = int.Parse(item["DE_HEALTH"].ToString());
                intTaxper = int.Parse(item["TAXPER"].ToString());
                otherminus = int.Parse(item["OTHERMINUS"].ToString());
                strMZ_AD = item["MZ_AD"].ToString();

                if (!insertData(strMZ_ID, amonth, strPOLNO, strPAY_AD, strName, strOCCC, strRank, strSLVC, strSPT,strSPT1,
                    "N", intSalary, intWork, intProfess, intBoss, intTech, intBonus, intAdven, intFar, intElec, intOtherAdd,
                    intInsurance, intHealthID, intDEHealth, intHealth, 0, intMonthTax, intMonth, intConcur3, intTaxper,
                    intExtra01, intExtra02, intExtra03, intExtra04, intExtra05, intExtra06, intExtra07, intExtra08, intExtra09, otherminus, strNote, strMZ_AD))
                {
                    DataRow dr = dtError.NewRow();

                    dr["Name"] = strName;
                    dr["Polno"] = strPOLNO;

                    dtError.Rows.Add(dr);
                }
            }

            return dtError;
        }

        public static bool insertData(string idcard, string amonth, string polno, string payad, string name, string occc, string srank, string slvc, string spt,string spt1, string lockdb,
            int salarypay, int work, int profess, int boss, int technics, int bonus, int adventive, int far, int electric, int otherAdd,
            int insurance, int healthid, int healthman, int healthpay, int healthpay1, int monthpayTax, int monthpay, int concur3, int tax,
            int extra01, int extra02, int extra03, int extra04, int extra05, int extra06, int extra07, int extra08, int extra09, int otherminus, string note, string strMZ_AD)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"Insert Into B_MONTHPAY_MAIN (MM_SNID, IDCARD, AMONTH, MZ_POLNO, PAY_AD, MZ_NAME, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT,MZ_SPT1, MZ_UNIT, LOCKDB, 
                    SALARYPAY1, WORKP, PROFESS, BOSS, TECHNICS, BONUS, ADVENTIVE, FAR, ELECTRIC, 
                    INSURANCEPAY, HEALTHID, HEALTHMAN, HEALTHPAY, HEALTHPAY1, MONTHPAY_TAX, MONTHPAY, CONCUR3PAY, TAX, 
                    EXTRA01, EXTRA02, EXTRA03, EXTRA04, EXTRA05, EXTRA06, EXTRA07, EXTRA08, EXTRA09, OTHERMINUS, NOTE, LASTDA, CREATOR, OTHERADD, MZ_AD)
                    Values 
                    ( NEXT VALUE FOR dbo.B_MONTHPAY_SN, @IDCARD, @AMONTH, @MZ_POLNO, @PAY_AD, @MZ_NAME, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @MZ_SPT,@MZ_SPT1, @MZ_UNIT, @LOCKDB, 
                    @SALARYPAY1, @WORKP, @PROFESS, @BOSS, @TECHNICS, @BONUS, @ADVENTIVE, @FAR, @ELECTRIC, 
                    @INSURANCEPAY, @HEALTHID, @HEALTHMAN, @HEALTHPAY, @HEALTHPAY1, @MONTHPAY_TAX, @MONTHPAY, @CONCUR3PAY, @TAX, 
                    @EXTRA01, @EXTRA02, @EXTRA03, @EXTRA04, @EXTRA05, @EXTRA06, @EXTRA07, @EXTRA08, @EXTRA09, @OTHERMINUS, @NOTE, @LASTDA, @CREATOR, @OTHERADD,@MZ_AD)";

            ops.Add(new SqlParameter("IDCARD", idcard));
            ops.Add(new SqlParameter("AMONTH", amonth));
            ops.Add(new SqlParameter("MZ_POLNO", polno));
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("MZ_NAME", name));
            ops.Add(new SqlParameter("MZ_OCCC", occc));
            ops.Add(new SqlParameter("MZ_SRANK", srank));
            ops.Add(new SqlParameter("MZ_SLVC", slvc));
            ops.Add(new SqlParameter("MZ_SPT", spt));
            ops.Add(new SqlParameter("MZ_SPT1", spt1));
            ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
            ops.Add(new SqlParameter("LOCKDB", lockdb));
            ops.Add(new SqlParameter("SALARYPAY1", salarypay));
            ops.Add(new SqlParameter("WORKP", work));
            ops.Add(new SqlParameter("PROFESS", profess));
            ops.Add(new SqlParameter("BOSS", boss));
            ops.Add(new SqlParameter("TECHNICS", technics));
            ops.Add(new SqlParameter("BONUS", bonus));
            ops.Add(new SqlParameter("ADVENTIVE", adventive));
            ops.Add(new SqlParameter("FAR", far));
            ops.Add(new SqlParameter("ELECTRIC", electric));
            ops.Add(new SqlParameter("INSURANCEPAY", insurance));
            ops.Add(new SqlParameter("HEALTHID", healthid));
            ops.Add(new SqlParameter("HEALTHMAN", healthman));
            ops.Add(new SqlParameter("HEALTHPAY", healthpay));
            ops.Add(new SqlParameter("HEALTHPAY1", healthpay1));
            ops.Add(new SqlParameter("MONTHPAY_TAX", monthpayTax));
            ops.Add(new SqlParameter("MONTHPAY", monthpay));
            ops.Add(new SqlParameter("CONCUR3PAY", concur3));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("EXTRA02", extra02));
            ops.Add(new SqlParameter("EXTRA03", extra03));
            ops.Add(new SqlParameter("EXTRA04", extra04));
            ops.Add(new SqlParameter("EXTRA05", extra05));
            ops.Add(new SqlParameter("EXTRA06", extra06));
            ops.Add(new SqlParameter("EXTRA07", extra07));
            ops.Add(new SqlParameter("EXTRA08", extra08));
            ops.Add(new SqlParameter("EXTRA09", extra09));
            ops.Add(new SqlParameter("OTHERMINUS", otherminus));
            ops.Add(new SqlParameter("NOTE", note));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));
            ops.Add(new SqlParameter("OTHERADD", otherAdd));
            ops.Add(new SqlParameter("MZ_AD", strMZ_AD));

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
                return true;
            }
            catch { return false; }
        }


        /// <summary>
        /// 把每月薪資/補發薪資應發總表的資料列處理加入每頁小計、總計資料列(目前設定每頁20筆)
        /// </summary>
        /// <param name="dt">要處理的資料</param>
        public static void mixMonthpayList(ref DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return;

            int pager = 22;//每頁幾筆
            List<DataRow> drs = new List<DataRow>();
            //計算總共需要分成幾頁
            int pages = (int)Math.Ceiling((float)dt.Rows.Count / pager);

            //總計資料列
            DataRow drSum = dt.NewRow();
            drSum["PAY_AD"] = dt.Rows[0]["PAY_AD"];
            drSum["AMONTH"] = dt.Rows[0]["AMONTH"];
            drSum["UNITCODE"] = "";
            drSum["UNIT"] = "總計";
            drSum["PEOPLE_ACCOUNT"] = 0;
            drSum["SALARYPAY1"] = 0;
            drSum["BOSS"] = 0;
            drSum["PROFESS"] = 0;
            drSum["WORKP"] = 0;
            drSum["TECHNICS"] = 0;
            drSum["BONUS"] = 0;
            drSum["ADVENTIVE"] = 0;
            drSum["FAR"] = 0;
            drSum["ELECTRIC"] = 0;
            drSum["OTHERADD"] = 0;
            drSum["TOTAL"] = 0;

            for (int i = 0; i < pages; i++)
            {
                //單位小計
                DataRow unitSum = dt.NewRow();
                unitSum["PAY_AD"] = dt.Rows[0]["PAY_AD"];
                unitSum["AMONTH"] = dt.Rows[0]["AMONTH"];
                unitSum["UNITCODE"] = "";
                unitSum["UNIT"] = "單位小計";
                unitSum["PEOPLE_ACCOUNT"] = 0;
                unitSum["SALARYPAY1"] = 0;
                unitSum["BOSS"] = 0;
                unitSum["PROFESS"] = 0;
                unitSum["WORKP"] = 0;
                unitSum["TECHNICS"] = 0;
                unitSum["BONUS"] = 0;
                unitSum["ADVENTIVE"] = 0;
                unitSum["FAR"] = 0;
                unitSum["ELECTRIC"] = 0;
                unitSum["OTHERADD"] = 0;
                unitSum["TOTAL"] = 0;

                for (int j = pager * i; j < pager * (i + 1); j++)
                {
                    if (j >= dt.Rows.Count)
                        break;

                    drSum["PEOPLE_ACCOUNT"] = int.Parse(drSum["PEOPLE_ACCOUNT"].ToString()) + int.Parse(dt.Rows[j]["PEOPLE_ACCOUNT"].ToString());
                    drSum["SALARYPAY1"] = int.Parse(drSum["SALARYPAY1"].ToString()) + int.Parse(dt.Rows[j]["SALARYPAY1"].ToString());
                    drSum["BOSS"] = int.Parse(drSum["BOSS"].ToString()) + int.Parse(dt.Rows[j]["BOSS"].ToString());
                    drSum["PROFESS"] = int.Parse(drSum["PROFESS"].ToString()) + int.Parse(dt.Rows[j]["PROFESS"].ToString());
                    drSum["WORKP"] = int.Parse(drSum["WORKP"].ToString()) + int.Parse(dt.Rows[j]["WORKP"].ToString());
                    drSum["TECHNICS"] = int.Parse(drSum["TECHNICS"].ToString()) + int.Parse(dt.Rows[j]["TECHNICS"].ToString());
                    drSum["BONUS"] = int.Parse(drSum["BONUS"].ToString()) + int.Parse(dt.Rows[j]["BONUS"].ToString());
                    drSum["ADVENTIVE"] = int.Parse(drSum["ADVENTIVE"].ToString()) + int.Parse(dt.Rows[j]["ADVENTIVE"].ToString());
                    drSum["FAR"] = int.Parse(drSum["FAR"].ToString()) + int.Parse(dt.Rows[j]["FAR"].ToString());
                    drSum["ELECTRIC"] = int.Parse(drSum["ELECTRIC"].ToString()) + int.Parse(dt.Rows[j]["ELECTRIC"].ToString());
                    drSum["OTHERADD"] = int.Parse(drSum["OTHERADD"].ToString()) + int.Parse(dt.Rows[j]["OTHERADD"].ToString());
                    drSum["TOTAL"] = int.Parse(drSum["TOTAL"].ToString()) + int.Parse(dt.Rows[j]["TOTAL"].ToString());

                    if (dt.Rows[j]["UNITCODE"].ToString().StartsWith("ZA"))
                        continue;

                    unitSum["PEOPLE_ACCOUNT"] = int.Parse(unitSum["PEOPLE_ACCOUNT"].ToString()) + int.Parse(dt.Rows[j]["PEOPLE_ACCOUNT"].ToString());
                    unitSum["SALARYPAY1"] = int.Parse(unitSum["SALARYPAY1"].ToString()) + int.Parse(dt.Rows[j]["SALARYPAY1"].ToString());
                    unitSum["BOSS"] = int.Parse(unitSum["BOSS"].ToString()) + int.Parse(dt.Rows[j]["BOSS"].ToString());
                    unitSum["PROFESS"] = int.Parse(unitSum["PROFESS"].ToString()) + int.Parse(dt.Rows[j]["PROFESS"].ToString());
                    unitSum["WORKP"] = int.Parse(unitSum["WORKP"].ToString()) + int.Parse(dt.Rows[j]["WORKP"].ToString());
                    unitSum["TECHNICS"] = int.Parse(unitSum["TECHNICS"].ToString()) + int.Parse(dt.Rows[j]["TECHNICS"].ToString());
                    unitSum["BONUS"] = int.Parse(unitSum["BONUS"].ToString()) + int.Parse(dt.Rows[j]["BONUS"].ToString());
                    unitSum["ADVENTIVE"] = int.Parse(unitSum["ADVENTIVE"].ToString()) + int.Parse(dt.Rows[j]["ADVENTIVE"].ToString());
                    unitSum["FAR"] = int.Parse(unitSum["FAR"].ToString()) + int.Parse(dt.Rows[j]["FAR"].ToString());
                    unitSum["ELECTRIC"] = int.Parse(unitSum["ELECTRIC"].ToString()) + int.Parse(dt.Rows[j]["ELECTRIC"].ToString());
                    unitSum["OTHERADD"] = int.Parse(unitSum["OTHERADD"].ToString()) + int.Parse(dt.Rows[j]["OTHERADD"].ToString());
                    unitSum["TOTAL"] = int.Parse(unitSum["TOTAL"].ToString()) + int.Parse(dt.Rows[j]["TOTAL"].ToString());
                }

                drs.Add(unitSum);
            }

            dt.Rows.Add(drs[drs.Count - 1]);
            dt.Rows.Add(drSum);
            for (int i = drs.Count - 2; i >= 0; i--)
            {
                DataRow drSumClone = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    drSumClone[col] = drSum[col];
                }

                dt.Rows.InsertAt(drs[i], (i + 1) * pager);
                dt.Rows.InsertAt(drSumClone, ((i + 1) * pager) + 1);
            }
        }

        /// <summary>
        /// 把每月薪資應扣總表的資料列處理加入每頁小計、總計資料列(目前設定每頁20筆)
        /// </summary>
        /// <param name="dt">要處理的資料</param>
        public static void mixMonthpayTakeoffList(ref DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return;

            int pager = 22;
            List<DataRow> drs = new List<DataRow>();
            //計算總共需要分成幾頁
            int pages = (int)Math.Ceiling((float)dt.Rows.Count / pager);

            //總計資料列
            DataRow drSum = dt.NewRow();
            drSum["PAY_AD"] = dt.Rows[0]["PAY_AD"];
            drSum["AMONTH"] = dt.Rows[0]["AMONTH"];
            drSum["UNITCODE"] = "";
            drSum["UNIT"] = "總計";
            drSum["PEOPLE_ACCOUNT"] = 0;
            drSum["ADD_SUM"] = 0;
            drSum["INSURANCEPAY"] = 0;
            drSum["HEALTHPAY"] = 0;
            drSum["HEALTHPAY1"] = 0;
            drSum["MONTHPAY_TAX"] = 0;
            drSum["MONTHPAY"] = 0;
            drSum["CONCUR3PAY"] = 0;
            drSum["TAX"] = 0;
            drSum["EXTRA01"] = 0;
            drSum["EXTRA02"] = 0;
            drSum["EXTRA03"] = 0;
            drSum["EXTRA04"] = 0;
            drSum["EXTRA05"] = 0;
            drSum["EXTRA06"] = 0;
            drSum["EXTRA07"] = 0;
            drSum["EXTRA08"] = 0;
            drSum["EXTRA09"] = 0;
            drSum["OTHERMINUS"] = 0;
            drSum["DES_SUM"] = 0;
            drSum["TOTAL"] = 0;

            for (int i = 0; i < pages; i++)
            {
                //科室小計
                DataRow unitSum = dt.NewRow();
                unitSum["PAY_AD"] = dt.Rows[0]["PAY_AD"];
                unitSum["AMONTH"] = dt.Rows[0]["AMONTH"];
                unitSum["UNITCODE"] = "";
                unitSum["UNIT"] = "單位小計";
                unitSum["PEOPLE_ACCOUNT"] = 0;
                unitSum["ADD_SUM"] = 0;
                unitSum["INSURANCEPAY"] = 0;
                unitSum["HEALTHPAY"] = 0;
                unitSum["HEALTHPAY1"] = 0;
                unitSum["MONTHPAY_TAX"] = 0;
                unitSum["MONTHPAY"] = 0;
                unitSum["CONCUR3PAY"] = 0;
                unitSum["TAX"] = 0;
                unitSum["EXTRA01"] = 0;
                unitSum["EXTRA02"] = 0;
                unitSum["EXTRA03"] = 0;
                unitSum["EXTRA04"] = 0;
                unitSum["EXTRA05"] = 0;
                unitSum["EXTRA06"] = 0;
                unitSum["EXTRA07"] = 0;
                unitSum["EXTRA08"] = 0;
                unitSum["EXTRA09"] = 0;
                unitSum["OTHERMINUS"] = 0;
                unitSum["DES_SUM"] = 0;
                unitSum["TOTAL"] = 0;

                for (int j = pager * i; j < pager * (i + 1); j++)
                {
                    if (j >= dt.Rows.Count)
                        break;

                    drSum["PEOPLE_ACCOUNT"] = int.Parse(drSum["PEOPLE_ACCOUNT"].ToString()) + int.Parse(dt.Rows[j]["PEOPLE_ACCOUNT"].ToString());
                    drSum["ADD_SUM"] = int.Parse(drSum["ADD_SUM"].ToString()) + int.Parse(dt.Rows[j]["ADD_SUM"].ToString());
                    drSum["INSURANCEPAY"] = int.Parse(drSum["INSURANCEPAY"].ToString()) + int.Parse(dt.Rows[j]["INSURANCEPAY"].ToString());
                    drSum["HEALTHPAY"] = int.Parse(drSum["HEALTHPAY"].ToString()) + int.Parse(dt.Rows[j]["HEALTHPAY"].ToString());
                    drSum["HEALTHPAY1"] = int.Parse(drSum["HEALTHPAY1"].ToString()) + int.Parse(dt.Rows[j]["HEALTHPAY1"].ToString());
                    drSum["MONTHPAY_TAX"] = int.Parse(drSum["MONTHPAY_TAX"].ToString()) + int.Parse(dt.Rows[j]["MONTHPAY_TAX"].ToString());
                    drSum["MONTHPAY"] = int.Parse(drSum["MONTHPAY"].ToString()) + int.Parse(dt.Rows[j]["MONTHPAY"].ToString());
                    drSum["CONCUR3PAY"] = int.Parse(drSum["CONCUR3PAY"].ToString()) + int.Parse(dt.Rows[j]["CONCUR3PAY"].ToString());
                    drSum["TAX"] = int.Parse(drSum["TAX"].ToString()) + int.Parse(dt.Rows[j]["TAX"].ToString());
                    drSum["EXTRA01"] = int.Parse(drSum["EXTRA01"].ToString()) + int.Parse(dt.Rows[j]["EXTRA01"].ToString());
                    drSum["EXTRA02"] = int.Parse(drSum["EXTRA02"].ToString()) + int.Parse(dt.Rows[j]["EXTRA02"].ToString());
                    drSum["EXTRA03"] = int.Parse(drSum["EXTRA03"].ToString()) + int.Parse(dt.Rows[j]["EXTRA03"].ToString());
                    drSum["EXTRA04"] = int.Parse(drSum["EXTRA04"].ToString()) + int.Parse(dt.Rows[j]["EXTRA04"].ToString());
                    drSum["EXTRA05"] = int.Parse(drSum["EXTRA05"].ToString()) + int.Parse(dt.Rows[j]["EXTRA05"].ToString());
                    drSum["EXTRA06"] = int.Parse(drSum["EXTRA06"].ToString()) + int.Parse(dt.Rows[j]["EXTRA06"].ToString());
                    drSum["EXTRA07"] = int.Parse(drSum["EXTRA07"].ToString()) + int.Parse(dt.Rows[j]["EXTRA07"].ToString());
                    drSum["EXTRA08"] = int.Parse(drSum["EXTRA08"].ToString()) + int.Parse(dt.Rows[j]["EXTRA08"].ToString());
                    drSum["EXTRA09"] = int.Parse(drSum["EXTRA09"].ToString()) + int.Parse(dt.Rows[j]["EXTRA09"].ToString());
                    drSum["OTHERMINUS"] = int.Parse(drSum["OTHERMINUS"].ToString()) + int.Parse(dt.Rows[j]["OTHERMINUS"].ToString());
                    drSum["DES_SUM"] = int.Parse(drSum["DES_SUM"].ToString()) + int.Parse(dt.Rows[j]["DES_SUM"].ToString());
                    drSum["TOTAL"] = int.Parse(drSum["TOTAL"].ToString()) + int.Parse(dt.Rows[j]["TOTAL"].ToString());

                    if (dt.Rows[j]["UNITCODE"].ToString().StartsWith("ZA"))
                        continue;

                    unitSum["PEOPLE_ACCOUNT"] = int.Parse(unitSum["PEOPLE_ACCOUNT"].ToString()) + int.Parse(dt.Rows[j]["PEOPLE_ACCOUNT"].ToString());
                    unitSum["ADD_SUM"] = int.Parse(unitSum["ADD_SUM"].ToString()) + int.Parse(dt.Rows[j]["ADD_SUM"].ToString());
                    unitSum["INSURANCEPAY"] = int.Parse(unitSum["INSURANCEPAY"].ToString()) + int.Parse(dt.Rows[j]["INSURANCEPAY"].ToString());
                    unitSum["HEALTHPAY"] = int.Parse(unitSum["HEALTHPAY"].ToString()) + int.Parse(dt.Rows[j]["HEALTHPAY"].ToString());
                    unitSum["HEALTHPAY1"] = int.Parse(unitSum["HEALTHPAY1"].ToString()) + int.Parse(dt.Rows[j]["HEALTHPAY1"].ToString());
                    unitSum["MONTHPAY_TAX"] = int.Parse(unitSum["MONTHPAY_TAX"].ToString()) + int.Parse(dt.Rows[j]["MONTHPAY_TAX"].ToString());
                    unitSum["MONTHPAY"] = int.Parse(unitSum["MONTHPAY"].ToString()) + int.Parse(dt.Rows[j]["MONTHPAY"].ToString());
                    unitSum["CONCUR3PAY"] = int.Parse(unitSum["CONCUR3PAY"].ToString()) + int.Parse(dt.Rows[j]["CONCUR3PAY"].ToString());
                    unitSum["TAX"] = int.Parse(unitSum["TAX"].ToString()) + int.Parse(dt.Rows[j]["TAX"].ToString());
                    unitSum["EXTRA01"] = int.Parse(unitSum["EXTRA01"].ToString()) + int.Parse(dt.Rows[j]["EXTRA01"].ToString());
                    unitSum["EXTRA02"] = int.Parse(unitSum["EXTRA02"].ToString()) + int.Parse(dt.Rows[j]["EXTRA02"].ToString());
                    unitSum["EXTRA03"] = int.Parse(unitSum["EXTRA03"].ToString()) + int.Parse(dt.Rows[j]["EXTRA03"].ToString());
                    unitSum["EXTRA04"] = int.Parse(unitSum["EXTRA04"].ToString()) + int.Parse(dt.Rows[j]["EXTRA04"].ToString());
                    unitSum["EXTRA05"] = int.Parse(unitSum["EXTRA05"].ToString()) + int.Parse(dt.Rows[j]["EXTRA05"].ToString());
                    unitSum["EXTRA06"] = int.Parse(unitSum["EXTRA06"].ToString()) + int.Parse(dt.Rows[j]["EXTRA06"].ToString());
                    unitSum["EXTRA07"] = int.Parse(unitSum["EXTRA07"].ToString()) + int.Parse(dt.Rows[j]["EXTRA07"].ToString());
                    unitSum["EXTRA08"] = int.Parse(unitSum["EXTRA08"].ToString()) + int.Parse(dt.Rows[j]["EXTRA08"].ToString());
                    unitSum["EXTRA09"] = int.Parse(unitSum["EXTRA09"].ToString()) + int.Parse(dt.Rows[j]["EXTRA09"].ToString());
                    unitSum["OTHERMINUS"] = int.Parse(unitSum["OTHERMINUS"].ToString()) + int.Parse(dt.Rows[j]["OTHERMINUS"].ToString());
                    unitSum["DES_SUM"] = int.Parse(unitSum["DES_SUM"].ToString()) + int.Parse(dt.Rows[j]["DES_SUM"].ToString());
                    unitSum["TOTAL"] = int.Parse(unitSum["TOTAL"].ToString()) + int.Parse(dt.Rows[j]["TOTAL"].ToString());
                }

                drs.Add(unitSum);
            }

            dt.Rows.Add(drs[drs.Count - 1]);
            dt.Rows.Add(drSum);
            for (int i = drs.Count - 2; i >= 0; i--)
            {
                DataRow drSumClone = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    drSumClone[col] = drSum[col];
                }

                dt.Rows.InsertAt(drs[i], (i + 1) * pager);
                dt.Rows.InsertAt(drSumClone, ((i + 1) * pager) + 1);
            }
        }
    }
}
