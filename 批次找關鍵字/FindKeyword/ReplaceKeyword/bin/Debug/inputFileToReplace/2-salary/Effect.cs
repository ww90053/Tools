using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    [Serializable]
    public class Effect
    {
        //TODO 這邊用的屬性名稱竟然跟下面function的相同,這最後好修正
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
        public int bossAmonth;
        public int salary;
        public int boss;
        public int profess;
        public int work;
        public int tech;
        public int far;
        //20150121
        /// <summary>
        /// 繁重加給
        /// </summary>
        public int electric;
        public int extra01;
        public int tax;
        public int total;
        //2013/01/31 備註無法儲存
        public string note;
        //20150327
        /// <summary>
        /// 是否關帳
        /// </summary>
        public string lockdb;


        //20140606
        /// <summary>
        /// 考績等級
        /// </summary>
        public string grade;

        public Effect(int sn)
        {
            string sql = "SELECT * FROM VW_ALL_EFFECT_DATA WHERE E_SNID=@SN";
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
            int.TryParse(dt.Rows[0]["BOSS_AMONTH"].ToString(), out bossAmonth);
            int.TryParse(dt.Rows[0]["SALARYPAY1"].ToString(), out salary);
            int.TryParse(dt.Rows[0]["BOSS"].ToString(), out boss);
            int.TryParse(dt.Rows[0]["PROFESS"].ToString(), out profess);
            int.TryParse(dt.Rows[0]["WORKP"].ToString(), out work);
            int.TryParse(dt.Rows[0]["TECHNICS"].ToString(), out tech);
            int.TryParse(dt.Rows[0]["FAR"].ToString(), out far);
            //20150121
            int.TryParse(dt.Rows[0]["ELECTRICPAY"].ToString(), out electric);
            int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out extra01);
            int.TryParse(dt.Rows[0]["TAX"].ToString(), out tax);
            int.TryParse(dt.Rows[0]["TOTAL"].ToString(), out total);
            //2013/01/31 備註無法儲存
            note = dt.Rows[0]["NOTE"].ToString();

            //20140606
            grade = dt.Rows[0]["GRADE"].ToString();
            //20150327
            lockdb = dt.Rows[0]["LOCKDB"].ToString();

        }
        public Effect(string year, string idcard)
        {
            string sql = "SELECT * FROM VW_ALL_EFFECT_DATA WHERE AYEAR=@AYEAR AND IDNO=@IDCARD";
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            ops.Add(new SqlParameter("AYEAR", year));
            ops.Add(new SqlParameter("IDCARD", idcard));

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;

            this.sn = int.Parse(dt.Rows[0]["E_SNID"].ToString());
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
            int.TryParse(dt.Rows[0]["BOSS_AMONTH"].ToString(), out bossAmonth);
            int.TryParse(dt.Rows[0]["SALARYPAY1"].ToString(), out salary);
            int.TryParse(dt.Rows[0]["BOSS"].ToString(), out boss);
            int.TryParse(dt.Rows[0]["PROFESS"].ToString(), out profess);
            int.TryParse(dt.Rows[0]["WORKP"].ToString(), out work);
            int.TryParse(dt.Rows[0]["TECHNICS"].ToString(), out tech);
            int.TryParse(dt.Rows[0]["FAR"].ToString(), out far);
            //20150121
            int.TryParse(dt.Rows[0]["ELECTRICPAY"].ToString(), out electric);
            int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out extra01);
            int.TryParse(dt.Rows[0]["TAX"].ToString(), out tax);
            int.TryParse(dt.Rows[0]["TOTAL"].ToString(), out total);


            //20140606
            grade = dt.Rows[0]["GRADE"].ToString();

            //20150327
            lockdb = dt.Rows[0]["LOCKDB"].ToString();
        }

        //2013/01/31 備註無法儲存
        public bool update(float pay, int bossAmonth, int salary, int boss, int profess, int work, int tech, int far, int electric, int extra01, int tax, int total,string Note)
        {
            string sql = @"UPDATE B_EFFECT SET PAY=@PAY, BOSS_AMONTH=@BOSS_AMONTH, SALARYPAY1=@SALARYPAY1, BOSS=@BOSS, PROFESS=@PROFESS, WORKP=@WORKP, TECHNICS=@TECHNICS, FAR=@FAR, ELECTRICPAY=@ELECTRICPAY ,
                           EXTRA01=@EXTRA01, TAX=@TAX, TOTAL=@TOTAL, LASTDA=@LASTDA, LASTUSER=@LASTUSER,NOTE=@NOTE 
                           WHERE E_SNID=@SN";
            List<SqlParameter> ops = new List<SqlParameter>();
            bool isOk;
            ops.Add(new SqlParameter("PAY", pay));
            ops.Add(new SqlParameter("BOSS_AMONTH", bossAmonth));
            ops.Add(new SqlParameter("SALARYPAY1", salary));
            ops.Add(new SqlParameter("BOSS", boss));
            ops.Add(new SqlParameter("PROFESS", profess));
            ops.Add(new SqlParameter("WORKP", work));
            ops.Add(new SqlParameter("TECHNICS", tech));
            ops.Add(new SqlParameter("FAR", far));
            //20150121
            ops.Add(new SqlParameter("ELECTRICPAY", electric));
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
            string sql = "DELETE B_EFFECT WHERE E_SNID=@SN";
            List<SqlParameter> ops = new List<SqlParameter>();
            ops.Add(new SqlParameter("SN", this.sn));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        // 產生考績獎金資料
        public static DataTable createEffect(string year, string payad, string idcard)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dtError = new DataTable();

            dtError.Columns.Add("Name");
            dtError.Columns.Add("Polno");

            // 先把已存在的資料刪除
            sql = "DELETE B_EFFECT WHERE PAY_AD=@PAY_AD AND AYEAR=@AYEAR AND LOCKDB='N' ";
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

            // 開始建立考績獎金資料
            sql = "select * from VW_ALL_BASE_DATA where PAY_AD=@PAY_AD";
            ops = new List<SqlParameter>();
            ops.Add(new SqlParameter("PAY_AD", payad));
            if (idcard != null)
            {
                //20150121
                sql += " AND MZ_ID=@IDCARD";
                ops.Add(new SqlParameter("IDCARD", idcard));
            }

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
                int work;
                int tech;
                int far;

                //20150121
                int electric;
                int intExtra01;
                //應發金額
                int intTotal;
                //所得稅
                int intTax = 0;
                double dblBossMonth;
                //在職月數
                double dblOndutyMonth;
                double plus = 0;

                //20150216 改抓人事的最後一筆考績等第
                string strGrade = "SELECT T26 FROM A_EFFICIENCY WHERE MZ_ID=@MZ_ID AND T01=@T01";
                  List<SqlParameter> para = new List<SqlParameter>();
                        para.Add(new SqlParameter("T01", SqlDbType.VarChar) { Value = year });
                        para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = item["MZ_ID"].ToString() });

                        string grade = o_DBFactory.ABC_toTest.GetValue(strGrade, para);
                
                // 甲等發1個月、乙等發0.5個月、其餘0
                //string grade = item["GRADE"].ToString().Trim();
                if (grade == "甲")
                    plus = 1;
                else if (grade == "乙")
                    plus = 0.5;
                else
                    plus = 0;

                //plus = 1;//測試

                strMZ_ID = item["MZ_ID"].ToString();
                strPOLNO = item["MZ_POLNO"].ToString();
                strPAY_AD = item["PAY_AD"].ToString();
                strName = item["MZ_NAME"].ToString();
                strOCCC = item["MZ_OCCC"].ToString();
                strRank = item["MZ_SRANK"].ToString();
                strSLVC = item["MZ_SLVC"].ToString();
                strSPT = item["MZ_SPT"].ToString();

                //2013/01/24 有任主管的話預設12個月，否則0個月
                double.TryParse(item["MZ_PB2"].ToString(), out dblBossMonth);
                if (dblBossMonth > 0)
                {
                }
                else
                {
                    if (int.Parse(item["BOSSPAY"].ToString()) > 0)
                        dblBossMonth = 12;
                    else
                        dblBossMonth = 0;
                }
                       
                intBoss = Salary.round(int.Parse(item["BossPay"].ToString()) * plus * dblBossMonth / 12.00);
                //intBoss = Salary.round(int.Parse(item["BossPay"].ToString()) * plus);

                intProfess = Salary.round(int.Parse(item["ProfessPay"].ToString()) * plus);
                intSalary = Salary.round(int.Parse(item["SalaryPay"].ToString()) * plus);
                work = Salary.round(int.Parse(item["WorkpPay"].ToString()) * plus);
                tech = Salary.round(int.Parse(item["TechnicsPay"].ToString()) * plus);
                far = Salary.round(int.Parse(item["FarPay"].ToString()) * plus);
                //20150121
                electric=Salary.round(int.Parse(item["ELECTRICPAY"].ToString()) * plus);

                intExtra01 = Salary.round(int.Parse(item["Extra01"].ToString()));
                //dblBossMonth = double.Parse(item["MZ_PB2"].ToString());
                dblOndutyMonth = int.Parse(item["MZ_ONDUTYMONTH"].ToString());
                //20150121
                //20210118 Mark 考績計算所得稅時 , 扣除主管加給
                //計算所得收入:薪俸+專業加給+警勤加給+技術加給+偏遠加給+警勤加給+繁重加給總和
                //計算應發金額:薪俸+主管加給+專業加給+警勤加給+技術加給+偏遠加給+警勤加給+繁重加給總和
                //兩者最大的區別在於,主管加給在計算所得稅的時候,不會納入

                //計算所得收入
                int intTaxTotal = intSalary + intProfess + work + tech + far + electric;
                //計算應發金額
                intTotal = intSalary + intBoss + intProfess + work + tech + far + electric;

                //2013/01/24 小隊長 法院扣款*發給月數
                if (intExtra01 > 0)
                    intExtra01 = Salary.round((intTotal / 3.0));

                //2013/01/23小隊長：扣除最低扣稅額
                //EX: 假設最低稅額 86001(含),稅率0.05
                //  計算出所得收入(不含主管加給)
                //  所得收入86000以下,免稅
                //  所得收入86001以上,將 所得收入*稅率 得所得稅
                if (intTaxTotal >= Tax.getTaxStart())
                {
                    // intTax = Convert.ToInt32(Salary.round(intTotal * Tax.getTaxPercent()));
                    //20210118 Mark 考績計算所得稅時 , 扣除主管加給
                    intTax = Convert.ToInt32(Salary.round(intTaxTotal * Tax.getTaxPercent()));
                }
               
                if (!insertData(strMZ_ID, strName, year, strPAY_AD, strOCCC, strRank, strSLVC, strSPT, strPOLNO, "N", plus, intSalary, intProfess, intBoss, work, tech, far,electric, dblBossMonth, intExtra01, intTax, intTotal, "", grade))
                {
                    DataRow dr = dtError.NewRow();

                    dr["Name"] = strName;
                    dr["Polno"] = strPOLNO;

                    dtError.Rows.Add(dr);
                }
            }

            return dtError;
        }

        public static bool insertData(string idcard, string name, string ayear,
            string payad, string occc, string srank, string slvc, string spt,
            string polno, string lockdb, double plus, int salarypay,
            int profess, int boss, int work, int tech, int far, int electric, double bossMonth, int extra01, int tax, int total, string note, string grade )
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            //20150121
            sql = @"INSERT INTO B_EFFECT (E_SNID, IDCARD, MZ_NAME, AYEAR, PAY_AD, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT, MZ_UNIT, MZ_POLNO, LOCKDB, 
                    PAY, SALARYPAY1, PROFESS, BOSS, WORKP, TECHNICS, FAR, ELECTRICPAY, BOSS_AMONTH, 
                    EXTRA01, TAX, TOTAL, NOTE, LASTDA, LASTUSER,GRADE) 
                    VALUES 
                    ( NEXT VALUE FOR dbo.B_EFFECT_SN, @IDCARD, @MZ_NAME, @AYEAR, @PAY_AD, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @MZ_SPT, @MZ_UNIT, @MZ_POLNO, @LOCKDB, 
                    @PAY, @SALARYPAY1, @PROFESS, @BOSS, @WORKP, @TECHNICS, @FAR, @ELECTRICPAY, @BOSS_AMONTH, 
                    @EXTRA01, @TAX, @TOTAL, @NOTE, @LASTDA, @LASTUSER,@GRADE) ";

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
            ops.Add(new SqlParameter("SALARYPAY1", salarypay));
            ops.Add(new SqlParameter("PROFESS", profess));
            ops.Add(new SqlParameter("BOSS", boss));
            ops.Add(new SqlParameter("WORKP", work));
            ops.Add(new SqlParameter("TECHNICS", tech));
            ops.Add(new SqlParameter("FAR", far));
            //20150121
            ops.Add(new SqlParameter("ELECTRICPAY", electric));
            ops.Add(new SqlParameter("BOSS_AMONTH", bossMonth));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("TOTAL", total));
            ops.Add(new SqlParameter("NOTE", note));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("LASTUSER", SalaryPublic.strLoginID));
            ops.Add(new SqlParameter("GRADE", grade));

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
                {
                    TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ops));
                }

                return true;
            }
            catch { return false; }
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

            sql = "UPDATE B_EFFECT SET LOCKDB='Y' WHERE PAY_AD=@PAYAD AND AYEAR=@AYEAR";
            ps.Add(new SqlParameter("PAYAD", payad));
            ps.Add(new SqlParameter("AYEAR", ayear));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }


        public static string getGrade(string idcard)
        {
            //2013/01/18 MZ_ID='{0'} 改成 MZ_ID='{0}'
            string sql = string.Format("SELECT GRADE FROM VW_ALL_BASE_DATA WHERE MZ_ID='{0}'", idcard);
            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(sql, "GET");
            if (temp.Rows.Count > 0)
            {
                return temp.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
