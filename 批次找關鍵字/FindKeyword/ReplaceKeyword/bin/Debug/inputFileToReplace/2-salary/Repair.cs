using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{

    [Serializable]
    public class Repair
    {
        public int sn;
        public string payad;
        public string unit;
        public string polno;
        public string idcard;
        public string name;
        public string occc;
        public string srank;
        public string slvc;
        public string spt;
        public string amonth;
        public int batch;
        public string sda;
        public string eda;
        public string ssrank;
        public string esrank;
        public string sspt;
        public string espt;
        public int atype;
        public int adays;
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
        public string grade;
        public bool lockdb;

        public Repair(int sn)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            sql = "SELECT * FROM B_REPAIRPAY WHERE R_SNID=@R_SNID";
            ops.Add(new SqlParameter("R_SNID", sn));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;

            this.sn = sn;
            payad = dt.Rows[0]["PAY_AD"].ToString();
            unit = dt.Rows[0]["MZ_UNIT"].ToString();
            polno = dt.Rows[0]["MZ_POLNO"].ToString();
            idcard = dt.Rows[0]["IDCARD"].ToString();
            name = dt.Rows[0]["MZ_NAME"].ToString();
            occc = dt.Rows[0]["MZ_OCCC"].ToString();
            srank = dt.Rows[0]["MZ_SRANK"].ToString();
            slvc = dt.Rows[0]["MZ_SLVC"].ToString();
            spt = dt.Rows[0]["MZ_SPT"].ToString();
            amonth = dt.Rows[0]["AMONTH"].ToString();
            batch = int.Parse(dt.Rows[0]["BATCH_NUMBER"].ToString());
            sda = dt.Rows[0]["SDA"].ToString();
            eda = dt.Rows[0]["EDA"].ToString();
            ssrank = dt.Rows[0]["SMZ_SRANK"].ToString();
            esrank = dt.Rows[0]["EMZ_SRANK"].ToString();
            sspt = dt.Rows[0]["SMZ_SPT"].ToString();
            espt = dt.Rows[0]["EMZ_SPT"].ToString();
            atype = int.Parse(dt.Rows[0]["ATYPE"].ToString());
            int.TryParse(dt.Rows[0]["ADAYS"].ToString(), out adays);
            int.TryParse(dt.Rows[0]["SALARYPAY1"].ToString(), out salary);
            int.TryParse(dt.Rows[0]["WORKPPAY"].ToString(), out work);
            int.TryParse(dt.Rows[0]["PROFESSPAY"].ToString(), out profess);
            int.TryParse(dt.Rows[0]["BOSSPAY"].ToString(), out boss);
            int.TryParse(dt.Rows[0]["TECHNICSPAY"].ToString(), out tech);
            int.TryParse(dt.Rows[0]["BONUSPAY"].ToString(), out bonus);
            int.TryParse(dt.Rows[0]["ADVENTIVEPAY"].ToString(), out adventive);
            int.TryParse(dt.Rows[0]["FARPAY"].ToString(), out far);
            int.TryParse(dt.Rows[0]["ELECTRICPAY"].ToString(), out electric);
            int.TryParse(dt.Rows[0]["OTHERADD"].ToString(), out otheradd);

            int.TryParse(dt.Rows[0]["INSURANCEPAY"].ToString(), out insurance);
            int.TryParse(dt.Rows[0]["HEALTHID"].ToString(), out healthPersonal);
            int.TryParse(dt.Rows[0]["HEALTHMAN"].ToString(), out healthMan);
            int.TryParse(dt.Rows[0]["HEALTHPAY"].ToString(), out health);
            int.TryParse(dt.Rows[0]["HEALTHPAY1"].ToString(), out healthpay1);
            int.TryParse(dt.Rows[0]["MONTHPAY_TAX"].ToString(), out monthpayTax);
            int.TryParse(dt.Rows[0]["MONTHPAY"].ToString(), out monthpay);
            int.TryParse(dt.Rows[0]["CONCUR3PAY"].ToString(), out concur3);
            int.TryParse(dt.Rows[0]["TAX"].ToString(), out tax);
            int.TryParse(dt.Rows[0]["OTHERMINUS"].ToString(), out otherminus);

            int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out extra01);
            int.TryParse(dt.Rows[0]["EXTRA02"].ToString(), out extra02);
            int.TryParse(dt.Rows[0]["EXTRA03"].ToString(), out extra03);
            int.TryParse(dt.Rows[0]["EXTRA04"].ToString(), out extra04);
            int.TryParse(dt.Rows[0]["EXTRA05"].ToString(), out extra05);
            int.TryParse(dt.Rows[0]["EXTRA06"].ToString(), out extra06);
            int.TryParse(dt.Rows[0]["EXTRA07"].ToString(), out extra07);
            int.TryParse(dt.Rows[0]["EXTRA08"].ToString(), out extra08);
            int.TryParse(dt.Rows[0]["EXTRA09"].ToString(), out extra09);
            note = dt.Rows[0]["NOTE"].ToString();
            grade = dt.Rows[0]["GRADE"].ToString();

            if (dt.Rows[0]["LOCKDB"].ToString() == "Y")
                lockdb = true;
            else
                lockdb = false;
        }


        /// <summary>
        /// 補發-修改
        /// </summary>
        /// <param name="polno"></param>
        /// <param name="sda"></param>
        /// <param name="eda"></param>
        /// <param name="ssrank"></param>
        /// <param name="esrank"></param>
        /// <param name="sspt"></param>
        /// <param name="espt"></param>
        /// <param name="atype"></param>
        /// <param name="salary"></param>
        /// <param name="work"></param>
        /// <param name="profess"></param>
        /// <param name="boss"></param>
        /// <param name="tech"></param>
        /// <param name="bonus"></param>
        /// <param name="adventive"></param>
        /// <param name="far"></param>
        /// <param name="electric"></param>
        /// <param name="otheradd"></param>
        /// <param name="insurance"></param>
        /// <param name="healthPersonal"></param>
        /// <param name="healthMan"></param>
        /// <param name="health"></param>
        /// <param name="healthPay1"></param>
        /// <param name="monthpayTax"></param>
        /// <param name="monthpay"></param>
        /// <param name="concur3"></param>
        /// <param name="tax"></param>
        /// <param name="otherminus"></param>
        /// <param name="extra01"></param>
        /// <param name="extra02"></param>
        /// <param name="extra03"></param>
        /// <param name="extra04"></param>
        /// <param name="extra05"></param>
        /// <param name="extra06"></param>
        /// <param name="extra07"></param>
        /// <param name="extra08"></param>
        /// <param name="extra09"></param>
        /// <param name="note"></param>
        public void update(string polno, string sda, string eda, string ssrank, string esrank, string sspt, string espt, int atype,
            int salary, int work, int profess, int boss, int tech, int bonus, int adventive, int far, int electric, int otheradd,
            int insurance, int healthPersonal, int healthMan, int health, int healthPay1, int monthpayTax, int monthpay, int concur3, int tax, int otherminus,
            int extra01, int extra02, int extra03, int extra04, int extra05, int extra06, int extra07, int extra08, int extra09, string note)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            
                sql = @"UPDATE B_REPAIRPAY SET MZ_POLNO=@MZ_POLNO, MZ_UNIT=@MZ_UNIT, SDA=@SDA, EDA=@EDA, SMZ_SRANK=@SMZ_SRANK, EMZ_SRANK=@EMZ_SRANK, SMZ_SPT=@SMZ_SPT, EMZ_SPT=@EMZ_SPT, ATYPE=@ATYPE, MZ_SPT=@MZ_SPT, 
                    SALARYPAY1=@SALARYPAY1, WORKPPAY=@WORKPPAY, PROFESSPAY=@PROFESSPAY, BOSSPAY=@BOSSPAY, TECHNICSPAY=@TECHNICSPAY, BONUSPAY=@BONUSPAY, ADVENTIVEPAY=@ADVENTIVEPAY, FARPAY=@FARPAY, ELECTRICPAY=@ELECTRICPAY, OTHERADD=@OTHERADD, 
                    INSURANCEPAY=@INSURANCEPAY, HEALTHID=@HEALTHID, HEALTHMAN=@HEALTHMAN, HEALTHPAY=@HEALTHPAY, HEALTHPAY1=@HEALTHPAY1, MONTHPAY_TAX=@MONTHPAY_TAX, MONTHPAY=@MONTHPAY, CONCUR3PAY=@CONCUR3PAY, TAX=@TAX, OTHERMINUS=@OTHERMINUS, 
                    EXTRA01=@EXTRA01, EXTRA02=@EXTRA02, EXTRA03=@EXTRA03, EXTRA04=@EXTRA04, EXTRA05=@EXTRA05, EXTRA06=@EXTRA06, EXTRA07=@EXTRA07, EXTRA08=@EXTRA08, EXTRA09=@EXTRA09, NOTE=@NOTE, LASTDA=@LASTDA, CREATOR=@CREATOR WHERE R_SNID=@R_SNID";
                ops.Add(new SqlParameter("MZ_POLNO", polno));
                ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
                ops.Add(new SqlParameter("SDA", sda));
                ops.Add(new SqlParameter("EDA", eda));
                ops.Add(new SqlParameter("SMZ_SRANK", ssrank));
                ops.Add(new SqlParameter("EMZ_SRANK", esrank));
                ops.Add(new SqlParameter("SMZ_SPT", sspt));
                ops.Add(new SqlParameter("EMZ_SPT", espt));

               

                ops.Add(new SqlParameter("ATYPE", atype));

            //2013/04/17 如果espt為null會有問題
            ops.Add(new SqlParameter("MZ_SPT", espt));

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
                ops.Add(new SqlParameter("HEALTHMAN", healthMan));
                ops.Add(new SqlParameter("HEALTHPAY", health));
                ops.Add(new SqlParameter("HEALTHPAY1", healthPay1));
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
                ops.Add(new SqlParameter("R_SNID", this.sn));

            
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

            sql = "DELETE B_REPAIRPAY WHERE R_SNID=@R_SNID";
            ops.Add(new SqlParameter("R_SNID", this.sn));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        public static void fillCaseid(ref System.Web.UI.WebControls.DropDownList ddl, string date)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT DISTINCT BATCH_NUMBER FROM B_REPAIRPAY WHERE AMONTH=@AMONTH ORDER BY BATCH_NUMBER";

            ops.Add(new SqlParameter("AMONTH", date));

            ddl.DataTextField = "BATCH_NUMBER";
            ddl.DataValueField = "BATCH_NUMBER";
            ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            ddl.DataBind();
        }

        /// <summary>
        /// 取得該發薪機關年月的最大批號
        /// </summary>
        /// <param name="payad"></param>
        /// <param name="amonth"></param>
        /// <returns></returns>
        public static int getCurrentBatch(string payad, string amonth)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            int batch;

            sql = "SELECT MAX(BATCH_NUMBER) FROM B_REPAIRPAY WHERE PAY_AD=@PAY_AD AND AMONTH=@AMONTH";
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("AMONTH", amonth));

            int.TryParse(o_DBFactory.ABC_toTest.GetValue(sql, ops), out batch);

            return batch;
        }

        /// <summary>
        /// 判斷該批號是不是已關帳
        /// </summary>
        /// <param name="payad"></param>
        /// <param name="amonth"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static bool isLockDB(string payad, string amonth, int batch)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            int tmpbatch;

            sql = "SELECT MAX(BATCH_NUMBER) FROM B_REPAIRPAY WHERE LOCKDB='Y' AND PAY_AD=@PAY_AD AND AMONTH=@AMONTH";
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("AMONTH", amonth));

            int.TryParse(o_DBFactory.ABC_toTest.GetValue(sql, ops), out tmpbatch);

            if (batch <= tmpbatch)
                return true;
            return false;
        }

        // 計算補發金額
        public static int countRepair(string sdate, string edate, int inValue)
        {
            DateTime DateS;
            DateTime DateE;
            double per = 0; ;

            // 起始月補發比例
            double sdays = 0;
            // 結束月補發比例
            double edays = 0;

            // 把起迄日期轉換成西元
            DateS = DateTime.Parse(SalaryPublic.ToADDateWithDash(sdate));
            DateE = DateTime.Parse(SalaryPublic.ToADDateWithDash(edate));

            if (DateS.Month != DateE.Month)
            {
                int months = 0;

                sdays = (double)((new DateTime(DateS.Year, DateS.Month, 1).AddMonths(1).AddDays(-1).Date - DateS).TotalDays + 1) / (new DateTime(DateS.Year, DateS.Month, 1).AddMonths(1).AddDays(-1).Day);
                edays = ((double)DateE.Day) / new DateTime(DateE.Year, DateE.Month, 1).AddMonths(1).AddDays(-1).Day;

                if (DateE.Year > DateS.Year)
                {
                    months = (DateE.Year - DateS.Year - 1) * 12;

                    months += 12 - DateS.Month;
                    months += DateE.Month - 1;// 扣掉起始、結束月後 中間有多少個月
                }
                else
                {
                    months = DateE.Month - DateS.Month - 1;
                }

                per = sdays + edays + months;
            }
            else
            {
                // 如果起始日跟結束日同一個月，只要算一個就可以了
                per = ((DateE - DateS).TotalDays + 1) / (new DateTime(DateE.Year, DateE.Month, 1).AddMonths(1).AddDays(-1).Day);
            }

            return Salary.round(inValue * per);
        }

        /// <summary>
        /// 關帳
        /// </summary>
        /// <param name="payad">發薪機關</param>
        /// <param name="amonth">資料年月</param>
        /// <param name="batch">批號</param>
        public static void lockDB(string payad, string amonth, string batch)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();

            sql = "UPDATE B_REPAIRPAY SET LOCKDB='Y' WHERE PAY_AD=@PAYAD AND AMONTH=@AMONTH AND BATCH_NUMBER=@BATCH";
            ps.Add(new SqlParameter("PAYAD", payad));
            ps.Add(new SqlParameter("AMONTH", amonth));
            ps.Add(new SqlParameter("BATCH", batch));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }
    }
}
