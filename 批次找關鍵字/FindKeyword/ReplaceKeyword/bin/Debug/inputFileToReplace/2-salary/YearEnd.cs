using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{

    [Serializable]
    public class YearEnd
    {
        // 產生年終獎金資料
        public static DataTable createYearEnd(string year, string payad, double plus, string idcard)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dtError = new DataTable();

            dtError.Columns.Add("Name");
            dtError.Columns.Add("Polno");

            // 先把已存在的資料刪除,但是要避開已經被鎖定的
            sql = "DELETE B_YEARPAY WHERE PAY_AD=@PAY_AD AND AYEAR=@AYEAR AND LOCKDB='N' ";
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("AYEAR", year));
            if (idcard != null)
            {
                sql += " AND IDCARD=@IDCARD";
                ops.Add(new SqlParameter("IDCARD", idcard));
            }
            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", SalaryPublic.RegixSQL(sql, ops));
            }

            // 開始建立年終獎金資料
            //sql = "select * from VW_ALL_BASE_DATA where PAY_AD=@PAY_AD"; --非在職不領 //2013/01/10 小隊長說不用算
            sql = "select * from VW_ALL_BASE_DATA where PAY_AD=@PAY_AD AND (ISOFFDUTY ='否' or ISOFFDUTY is null) ";
            ops = new List<SqlParameter>();
            ops.Add(new SqlParameter("PAY_AD", payad));
            if (idcard != null)
            {
                sql += " AND MZ_ID=@IDCARD";
                ops.Add(new SqlParameter("IDCARD", idcard));
            }
            int years;
            int.TryParse(year, out years);
            foreach (DataRow item in o_DBFactory.ABC_toTest.DataSelect(sql, ops).Rows)
            {

                string strMZ_ID;
                string strPOLNO;
                string strPAY_AD;
                string strName;
                string strOCCC;
                string strRank;
                string strSLVC;
                string strSPT;
                int intBoss;
                int intProfess;
                int intSalary;
                int intExtra01;
                int intTotal;
                int intTax = 0;
                double dblBossMonth;
                //在職月數
                double dblOndutyMonth;

                strMZ_ID = item["MZ_ID"].ToString();
                strPOLNO = item["MZ_POLNO"].ToString();
                strPAY_AD = item["PAY_AD"].ToString();
                strName = item["MZ_NAME"].ToString();
                strOCCC = item["MZ_OCCC"].ToString();
                strRank = item["MZ_SRANK"].ToString();
                strSLVC = item["MZ_SLVC"].ToString();
                strSPT = item["MZ_SPT"].ToString();
                //intBoss = Convert.ToInt32(MathHelper.Round(int.Parse(item["BossPay"].ToString()) * plus));
                //intProfess = Convert.ToInt32(MathHelper.Round(int.Parse(item["ProfessPay"].ToString()) * plus));
                //intSalary = Convert.ToInt32(MathHelper.Round(int.Parse(item["SalaryPay"].ToString()) * plus));
                intExtra01 = int.Parse(item["Extra01"].ToString());
                dblBossMonth = double.Parse(item["MZ_PB2"].ToString());

                //2013/01/24 有任主管的話預設12個月，否則0個月
                if (dblBossMonth > 0)
                {
                    dblBossMonth = 12;
                }
                else
                {
                    if (int.Parse(item["BOSSPAY"].ToString()) > 0)
                        dblBossMonth = 12;
                    else
                        dblBossMonth = 0;
                }

                #region 依輸入時間計算在職月數

                int tempYear;
                int tempMonth;
                string mz_adate = item["MZ_FDATE"].ToString().PadLeft(7, '0');

                int.TryParse(mz_adate.Substring(0, 3), out tempYear);
                int.TryParse(mz_adate.Substring(3, 2), out tempMonth);

                if (years > tempYear)
                {
                    dblOndutyMonth = 12;
                }
                else if (years == tempYear)
                {
                    if (tempMonth == 0)
                        dblOndutyMonth = 0;
                    else
                        dblOndutyMonth = 13 - tempMonth;
                }
                else
                {
                    dblOndutyMonth = 0;
                }
                //dblOndutyMonth = int.Parse(item["MZ_ONDUTYMONTH"].ToString());
                #endregion

                intBoss = Salary.round(Salary.round(int.Parse(item["BossPay"].ToString()) * dblBossMonth / 12.00 * plus));
                intProfess = Salary.round(Salary.round(int.Parse(item["ProfessPay"].ToString()) * dblOndutyMonth / 12.00 * plus));
                intSalary = Salary.round(Salary.round(int.Parse(item["SalaryPay"].ToString()) * dblOndutyMonth / 12.00 * plus));

                //20210118 Mark計算所得稅時 , 去掉主管加給
                int intTaxTotal = int.Parse((intSalary + intProfess).ToString());
                intTotal = int.Parse((intSalary + intBoss + intProfess).ToString());

                //2013/01/15 小隊長 法院扣款*發給月數
                if (intExtra01 > 0)
                    intExtra01 = Salary.round((intTotal / 3.0));

                //2013/01/23小隊長：扣除最低扣稅額
                if (intTaxTotal >= Tax.getTaxStart())
                {
                    //intTax = Salary.round((intTotal * Tax.getTaxPercent()));
                    //20210118 Mark調整去掉主管加給的所得稅
                    intTax = Salary.round((intTaxTotal * Tax.getTaxPercent()));
                }

                sql = string.Format("select * from B_YEARPAY WHERE  AYEAR='{0}' AND IDCARD='{1}'", year, strMZ_ID);
                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
                if (temp.Rows.Count > 0)
                {
                    DataRow dr = dtError.NewRow();

                    dr["Name"] = strName;
                    dr["Polno"] = strPOLNO + " :該員於" + o_A_KTYPE.CODE_TO_NAME(temp.Rows[0]["PAY_AD"].ToString(), "04").Replace("新北市政府警察局", "") + "已有資料 ";

                    dtError.Rows.Add(dr);
                }
                else
                {
                    if (!insertData(strMZ_ID, strName, year, strPAY_AD, strOCCC, strRank, strSLVC, strSPT, strPOLNO, "N", plus, dblOndutyMonth, intSalary, intProfess, intBoss, dblBossMonth, intExtra01, intTax, intTotal, ""))
                    {
                        DataRow dr = dtError.NewRow();

                        dr["Name"] = strName;
                        dr["Polno"] = strPOLNO;

                        dtError.Rows.Add(dr);
                    }
                }
            }

            return dtError;
        }

        public static bool insertData(string idcard, string name, string ayear,
            string payad, string occc, string srank, string slvc, string spt,
            string polno, string lockdb, double plus, double amonth, int salarypay,
            int profess, int boss, double bossMonth, int extra01, int tax, int total, string note)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"INSERT INTO B_YEARPAY (Y_SNID, IDCARD, MZ_NAME, AYEAR, PAY_AD, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT, MZ_UNIT, MZ_POLNO, LOCKDB, 
                    PAY, SALARYPAY1, PROFESS, BOSS, BOSS_AMONTH, AMONTH, 
                    EXTRA01, TAX, TOTAL, NOTE, LASTDA, REDUCE, LASTUSER) 
                    VALUES 
                    ( NEXT VALUE FOR dbo.B_YEARPAY_SN, @IDCARD, @MZ_NAME, @AYEAR, @PAY_AD, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @MZ_SPT, @MZ_UNIT, @MZ_POLNO, @LOCKDB, 
                    @PAY, @SALARYPAY1, @PROFESS, @BOSS, @BOSS_AMONTH, @AMONTH, 
                    @EXTRA01, @TAX, @TOTAL, @NOTE, @LASTDA, 3, @LASTUSER) ";

            ops.Add(new SqlParameter("IDCARD", idcard));
            ops.Add(new SqlParameter("MZ_NAME", name));
            ops.Add(new SqlParameter("AYEAR", ayear));
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("MZ_OCCC", occc));
            ops.Add(new SqlParameter("MZ_SRANK", srank));
            ops.Add(new SqlParameter("MZ_SLVC", slvc));
            ops.Add(new SqlParameter("MZ_SPT", spt));
            ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
            ops.Add(new SqlParameter("MZ_POLNO", polno));
            ops.Add(new SqlParameter("LOCKDB", lockdb));
            ops.Add(new SqlParameter("PAY", plus));
            ops.Add(new SqlParameter("AMONTH", amonth));
            ops.Add(new SqlParameter("SALARYPAY1", salarypay));
            ops.Add(new SqlParameter("PROFESS", profess));
            ops.Add(new SqlParameter("BOSS", boss));
            ops.Add(new SqlParameter("BOSS_AMONTH", bossMonth));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("TOTAL", total));
            ops.Add(new SqlParameter("NOTE", note));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("LASTUSER", SalaryPublic.strLoginID));

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
                {
                    TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ops));
                }

                return true;
            }
            catch
            {
                // 先把已存在的資料刪除
                //sql = "DELETE B_YEARPAY WHERE  AYEAR=@AYEAR AND IDCARD=@IDCARD";
                //ops = new List<SqlParameter>();
                //ops.Add(new SqlParameter("IDCARD", idcard));
                //ops.Add(new SqlParameter("AYEAR", ayear));
                //o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
                return false;
            }
        }


        public int sn;
        public string idno;
        public string polno;
        public string name;
        public string srankName;
        public string spt;
        public string occcName;
        public string payadName;
        public string unitName;
        public int ayear;
        public float pay;
        public int amonth;
        public int bossAmonth;
        public int reduce;
        public int salary;
        public int boss;
        public int profess;
        public int extra01;
        public int tax;
        public int total;
        public string note;
        //20150327
        /// <summary>
        /// 是否關帳
        /// </summary>
        public string lockdb;

        public YearEnd(int sn)
        {
            string sql = "SELECT * FROM VW_ALL_YEARPAY_DATA WHERE Y_SNID=@SN";
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            ops.Add(new SqlParameter("SN", sn));

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;

            this.sn = sn;
            idno = dt.Rows[0]["IDNO"].ToString();
            polno = dt.Rows[0]["MZ_POLNO"].ToString();
            name = dt.Rows[0]["NAME"].ToString();
            srankName = dt.Rows[0]["CHISRANK"].ToString();
            spt = dt.Rows[0]["MZ_SPT"].ToString();
            occcName = dt.Rows[0]["CHIOCCC"].ToString();
            payadName = dt.Rows[0]["CHIAD"].ToString();
            unitName = dt.Rows[0]["PAY_UNIT"].ToString();
            int.TryParse(dt.Rows[0]["AYEAR"].ToString(), out ayear);
            float.TryParse(dt.Rows[0]["PAY"].ToString(), out pay);
            int.TryParse(dt.Rows[0]["AMONTH"].ToString(), out amonth);
            int.TryParse(dt.Rows[0]["BOSS_AMONTH"].ToString(), out bossAmonth);
            int.TryParse(dt.Rows[0]["REDUCE"].ToString(), out reduce);
            int.TryParse(dt.Rows[0]["SALARYPAY1"].ToString(), out salary);
            int.TryParse(dt.Rows[0]["BOSS"].ToString(), out boss);
            int.TryParse(dt.Rows[0]["PROFESS"].ToString(), out profess);
            int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out extra01);
            int.TryParse(dt.Rows[0]["TAX"].ToString(), out tax);
            int.TryParse(dt.Rows[0]["TOTAL"].ToString(), out total);
            note = dt.Rows[0]["NOTE"].ToString();

            //20150327
            lockdb = dt.Rows[0]["LOCKDB"].ToString();
        }
        public YearEnd(string ayear, string idcard)
        {
            string sql = "SELECT * FROM VW_ALL_YEARPAY_DATA WHERE AYEAR=@AYEAR AND IDNO=@IDCARD";
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            ops.Add(new SqlParameter("AYEAR", ayear));
            ops.Add(new SqlParameter("IDCARD", idcard));

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;

            this.sn = int.Parse(dt.Rows[0]["Y_SNID"].ToString());
            idno = dt.Rows[0]["IDNO"].ToString();
            polno = dt.Rows[0]["MZ_POLNO"].ToString();
            name = dt.Rows[0]["NAME"].ToString();
            srankName = dt.Rows[0]["CHISRANK"].ToString();
            spt = dt.Rows[0]["MZ_SPT"].ToString();
            occcName = dt.Rows[0]["CHIOCCC"].ToString();
            payadName = dt.Rows[0]["CHIAD"].ToString();
            unitName = dt.Rows[0]["PAY_UNIT"].ToString();
            int.TryParse(dt.Rows[0]["AYEAR"].ToString(), out this.ayear);
            float.TryParse(dt.Rows[0]["PAY"].ToString(), out pay);
            int.TryParse(dt.Rows[0]["AMONTH"].ToString(), out amonth);
            int.TryParse(dt.Rows[0]["BOSS_AMONTH"].ToString(), out bossAmonth);
            int.TryParse(dt.Rows[0]["REDUCE"].ToString(), out reduce);
            int.TryParse(dt.Rows[0]["SALARYPAY1"].ToString(), out salary);
            int.TryParse(dt.Rows[0]["BOSS"].ToString(), out boss);
            int.TryParse(dt.Rows[0]["PROFESS"].ToString(), out profess);
            int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out extra01);
            int.TryParse(dt.Rows[0]["TAX"].ToString(), out tax);
            int.TryParse(dt.Rows[0]["TOTAL"].ToString(), out total);
            note = dt.Rows[0]["NOTE"].ToString();
            //20150327
            lockdb = dt.Rows[0]["LOCKDB"].ToString();
        }

        public bool update(float pay, int amonth, int bossAmonth, int reduce, int salary, int boss, int profess, int extra01, int tax, int total, string Note)
        {
            string sql = "UPDATE B_YEARPAY SET PAY=@PAY, AMONTH=@AMONTH, BOSS_AMONTH=@BOSS_AMONTH, REDUCE=@REDUCE, SALARYPAY1=@SALARYPAY1, BOSS=@BOSS, PROFESS=@PROFESS, EXTRA01=@EXTRA01, TAX=@TAX, TOTAL=@TOTAL, LASTDA=@LASTDA, LASTUSER=@LASTUSER ,NOTE=@NOTE WHERE Y_SNID=@SN";
            List<SqlParameter> ops = new List<SqlParameter>();
            bool isOk;
            ops.Add(new SqlParameter("PAY", pay));
            ops.Add(new SqlParameter("AMONTH", amonth));
            ops.Add(new SqlParameter("BOSS_AMONTH", bossAmonth));
            ops.Add(new SqlParameter("REDUCE", reduce));
            ops.Add(new SqlParameter("SALARYPAY1", salary));
            ops.Add(new SqlParameter("BOSS", boss));
            ops.Add(new SqlParameter("PROFESS", profess));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("TOTAL", total));
            ops.Add(new SqlParameter("SN", this.sn));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("NOTE", Note));
            ops.Add(new SqlParameter("LASTUSER", SalaryPublic.strLoginID));
            
             isOk = o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
            }

            return isOk;
        }

        public void delete()
        {
            string sql = "DELETE B_YEARPAY WHERE Y_SNID=@SN";
            List<SqlParameter> ops = new List<SqlParameter>();
            ops.Add(new SqlParameter("SN", this.sn));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        /// <summary>
        /// 關帳
        /// </summary>
        /// <param name="payad">發薪機關</param>
        /// <param name="da">資料年度</param>
        public static void lockDB(string payad, string ayear)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();

            sql = "UPDATE B_YEARPAY SET LOCKDB='Y' WHERE PAY_AD=@PAYAD AND AYEAR=@AYEAR";
            ps.Add(new SqlParameter("PAYAD", payad));
            ps.Add(new SqlParameter("AYEAR", ayear));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }
    }
}
