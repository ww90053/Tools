using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    [Serializable]
    public class Sole
    {
        public int sn;
        public string idcard;
        public string polno;
        public string ad;
        public string name;
        public string da;
        public string caseid;
        public string num;
        public string taxesid;
        public string taxesid1;
        public string inout;
        public int pay;
        public bool hasTax;
        public int tax;
        public int pay1;
        public int pay2;
        public int pay3;
        public string note;
        public int saveUntax;
        public int extra01;
        public bool lockdb;
        //// 
        public int second_health_pay;
        public string external;
        //// 
        public Sole(string sn)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            sql = "SELECT * FROM B_SOLE WHERE S_SNID=@S_SNID";
            ops.Add(new SqlParameter("S_SNID", sn));

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;
            
            this.sn = int.Parse(sn);
            idcard = dt.Rows[0]["IDCARD"].ToString();
            polno = dt.Rows[0]["MZ_POLNO"].ToString();
            ad = dt.Rows[0]["PAY_AD"].ToString();
            name = dt.Rows[0]["MZ_NAME"].ToString();
            da = dt.Rows[0]["DA"].ToString();
            caseid = dt.Rows[0]["CASEID"].ToString();
            num = dt.Rows[0]["NUM"].ToString();
            taxesid = dt.Rows[0]["TAXES_ID"].ToString();
            taxesid1 = dt.Rows[0]["TAXES_ID1"].ToString();
            inout = dt.Rows[0]["DA_INOUT_GROUP"].ToString();
            pay = int.Parse(dt.Rows[0]["PAY"].ToString());
            hasTax = dt.Rows[0]["TAXFLAG"].ToString() == "0" ? false : true;
            tax = int.Parse(dt.Rows[0]["TAX"].ToString());
            pay1 = int.Parse(dt.Rows[0]["PAY1"].ToString());
            pay2 = int.Parse(dt.Rows[0]["PAY2"].ToString());
            pay3 = int.Parse(dt.Rows[0]["PAY3"].ToString());
            note = dt.Rows[0]["NOTE"].ToString();
            saveUntax = 0;
            int.TryParse(dt.Rows[0]["SAVEUNTAX"].ToString(), out saveUntax);
            extra01 = 0;
            int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out extra01);

            if (dt.Rows[0]["LOCKDB"].ToString() == "Y")
                lockdb = true;
            else
                lockdb = false;
            
            ////
            if (dt.Rows[0]["SECOND_HEALTHPAY_PAY"].ToString() != "" && dt.Rows[0]["SECOND_HEALTHPAY_PAY"].ToString() != null)
            {
                second_health_pay = Convert.ToInt32(dt.Rows[0]["SECOND_HEALTHPAY_PAY"]);
            }
            else
            {

                second_health_pay = 0;
            }


            if (dt.Rows[0]["EXTERNAL"].ToString() != "" && dt.Rows[0]["EXTERNAL"].ToString() != null)
            {
                if (dt.Rows[0]["EXTERNAL"].ToString() == "Y")
                    external = "Y";
                else
                    external = "N";
            }
            else
            {
                external = "N";
            }
          ////
        }

        public void update(string polno, string num, string taxid, string taxid1, int pay, int tax, int pay1, int pay2, int pay3, int saveUntax, int extra01, string note)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "UPDATE B_SOLE SET MZ_POLNO=@MZ_POLNO, MZ_UNIT=@MZ_UNIT, NUM=@NUM, TAXES_ID=@TAXES_ID, TAXES_ID1=@TAXES_ID1, PAY=@PAY, TAX=@TAX, PAY1=@PAY1, PAY2=@PAY2, PAY3=@PAY3, SAVEUNTAX=@SAVEUNTAX, EXTRA01=@EXTRA01, NOTE=@NOTE, CREATEDATE=@CREATEDATE, CREATOR=@CREATOR WHERE S_SNID=@S_SNID";

            ops.Add(new SqlParameter("MZ_POLNO", polno));
            ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
            ops.Add(new SqlParameter("NUM", num));
            if (taxid == null)
                ops.Add(new SqlParameter("TAXES_ID", DBNull.Value));
            else
                ops.Add(new SqlParameter("TAXES_ID", taxid));
            if (taxid1 == null)
                ops.Add(new SqlParameter("TAXES_ID1", DBNull.Value));
            else
                ops.Add(new SqlParameter("TAXES_ID1", taxid1));
            ops.Add(new SqlParameter("PAY", pay));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("PAY1", pay1));
            ops.Add(new SqlParameter("PAY2", pay2));
            ops.Add(new SqlParameter("PAY3", pay3));
            ops.Add(new SqlParameter("SAVEUNTAX", saveUntax));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("CREATEDATE", DateTime.Now));
            ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));
            ops.Add(new SqlParameter("NOTE", note));
            ops.Add(new SqlParameter("S_SNID", this.sn));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        //二代健保.新增頁面用
        public void update(string polno, string num, string taxid, string taxid1, int pay, int tax, int pay1, int pay2, int pay3, int saveUntax, int extra01, string note, int Second_Health_Pay, string External)
        {
            try
            {
                string sql;
                List<SqlParameter> ops = new List<SqlParameter>();

                sql = "UPDATE B_SOLE SET MZ_POLNO=@MZ_POLNO, MZ_UNIT=@MZ_UNIT, NUM=@NUM, TAXES_ID=@TAXES_ID, TAXES_ID1=@TAXES_ID1, PAY=@PAY, TAX=@TAX, PAY1=@PAY1, PAY2=@PAY2, PAY3=@PAY3, SAVEUNTAX=@SAVEUNTAX, EXTRA01=@EXTRA01, NOTE=@NOTE, CREATEDATE=@CREATEDATE, CREATOR=@CREATOR,SECOND_HEALTHPAY_PAY=@SECOND_HEALTHPAY_PAY,EXTERNAL=@EXTERNAL WHERE S_SNID=@S_SNID";

                ops.Add(new SqlParameter("MZ_POLNO", polno));
                ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
                ops.Add(new SqlParameter("NUM", num));
                if (taxid == null)
                    ops.Add(new SqlParameter("TAXES_ID", DBNull.Value));
                else
                    ops.Add(new SqlParameter("TAXES_ID", taxid));
                if (taxid1 == null)
                    ops.Add(new SqlParameter("TAXES_ID1", DBNull.Value));
                else
                    ops.Add(new SqlParameter("TAXES_ID1", taxid1));
                ops.Add(new SqlParameter("PAY", pay));
                ops.Add(new SqlParameter("TAX", tax));
                ops.Add(new SqlParameter("PAY1", pay1));
                ops.Add(new SqlParameter("PAY2", pay2));
                ops.Add(new SqlParameter("PAY3", pay3));
                ops.Add(new SqlParameter("SAVEUNTAX", saveUntax));
                ops.Add(new SqlParameter("EXTRA01", extra01));
                ops.Add(new SqlParameter("CREATEDATE", DateTime.Now));
                ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));
                ops.Add(new SqlParameter("NOTE", note));
                ops.Add(new SqlParameter("S_SNID", this.sn));
                ////
                ops.Add(new SqlParameter("SECOND_HEALTHPAY_PAY", Second_Health_Pay));
                ops.Add(new SqlParameter("EXTERNAL", External));
                ////

                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
                {
                    TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
                }
            }
            catch
            {
                update(polno, num, taxid, taxid1, pay, tax, pay1, pay2, pay3, saveUntax, extra01, note);
        
            }
        }

        public void delete()
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            
            sql = "DELETE B_SOLE WHERE S_SNID=@S_SNID";
            ops.Add(new SqlParameter("S_SNID", this.sn));

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
            
            sql = "SELECT DISTINCT CASEID FROM B_SOLE WHERE DA LIKE @DA ORDER BY CASEID";

            ops.Add(new SqlParameter("DA", date + "%"));

            ddl.DataTextField = "CASEID";
            ddl.DataValueField = "CASEID";
            ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            ddl.DataBind();
        }

        public static string getName(string id)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT NAME FROM B_SOLEITEM WHERE ID=@ID";

            ops.Add(new SqlParameter("ID", id));

            return o_DBFactory.ABC_toTest.GetValue(sql, ops);
        }

        // 依發薪機關/年月取得案號
        public static int getCaseid(string payad, string month)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT NVL(MAX(CASEID), 0) FROM B_SOLE WHERE LOCKDB='Y' AND DA LIKE @DA AND PAY_AD=@PAY_AD";

            ops.Add(new SqlParameter("DA", month + "%"));
            ops.Add(new SqlParameter("PAY_AD", payad));

            return int.Parse(o_DBFactory.ABC_toTest.GetValue(sql, ops)) + 1;
        }
        //ADD BY KIM 20120223
        public static void insertData(string id, string polno, string payad, string name, string occc, string srank, string slvc, string inout, string da, string caseid, string num, string taxid, string taxid1, int pay, string memo, int pay1, int pay2, int pay3, int tax, int saveuntax, int extra01, bool isImport)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            //2013/01/22 允許單一發放多筆重複輸入
            //if (isImport)
            //{
            //    sql = "DELETE B_SOLE WHERE IDCARD=@IDCARD AND NUM=@NUM AND DA=@DA AND CASEID=@CASEID AND FROMIMPORT='Y'";
            //    ops.Add(new SqlParameter("DA", da));
            //    ops.Add(new SqlParameter("CASEID", caseid.PadLeft(2, '0')));
            //    ops.Add(new SqlParameter("IDCARD", id));
            //    ops.Add(new SqlParameter("NUM", num));
            //    o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
            //}

            sql = @"INSERT INTO B_SOLE(S_SNID, IDCARD, MZ_POLNO, PAY_AD, MZ_UNIT, MZ_NAME, MZ_OCCC, MZ_SRANK, MZ_SLVC, DA_INOUT_GROUP, DA, CASEID, NUM, TAXES_ID, TAXES_ID1, PAY, NOTE, FROMIMPORT,PAY1,PAY2,PAY3,TAX,SAVEUNTAX, EXTRA01, CREATEDATE, CREATOR) 
                    VALUES( NEXT VALUE FOR dbo.B_SOLE_SN, @IDCARD, @MZ_POLNO, @PAY_AD, @MZ_UNIT, @MZ_NAME, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @DA_INOUT_GROUP, @DA, @CASEID, @NUM, @TAXES_ID, @TAXES_ID1, @PAY, @NOTE, @FROMIMPORT, @PAY1, @PAY2, @PAY3, @TAX, @SAVEUNTAX, @EXTRA01, @CREATEDATE, @CREATOR)";
            ops.Clear();
            ops.Add(new SqlParameter("IDCARD", id));
            ops.Add(new SqlParameter("MZ_POLNO", polno));
            ops.Add(new SqlParameter("PAY_AD", payad));
            if (polno == null)
                ops.Add(new SqlParameter("MZ_UNIT", DBNull.Value));
            else
                ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
            ops.Add(new SqlParameter("MZ_NAME", name));
            if (occc == null)
                ops.Add(new SqlParameter("MZ_OCCC", DBNull.Value));
            else
                ops.Add(new SqlParameter("MZ_OCCC", occc));
            if (srank == null)
                ops.Add(new SqlParameter("MZ_SRANK", DBNull.Value));
            else
            { //2013/04/08 by 立廷
                //ops.Add(new SqlParameter("MZ_SRANK", occc));
                ops.Add(new SqlParameter("MZ_SRANK", srank));
            }
            if (slvc == null)
                ops.Add(new SqlParameter("MZ_SLVC", DBNull.Value));
            else
            {//2013/04/08 by 立廷
                //ops.Add(new SqlParameter("MZ_SLVC", occc));
                ops.Add(new SqlParameter("MZ_SLVC", slvc));
            }
            ops.Add(new SqlParameter("DA_INOUT_GROUP", inout));
            ops.Add(new SqlParameter("DA", da));
            ops.Add(new SqlParameter("CASEID", caseid));
            ops.Add(new SqlParameter("NUM", num));
            if (taxid == null)
                ops.Add(new SqlParameter("TAXES_ID", DBNull.Value));
            else
                ops.Add(new SqlParameter("TAXES_ID", taxid));
            if (taxid1 == null)
                ops.Add(new SqlParameter("TAXES_ID1", DBNull.Value));
            else
                ops.Add(new SqlParameter("TAXES_ID1", taxid1));
            ops.Add(new SqlParameter("PAY", pay));
            ops.Add(new SqlParameter("NOTE", memo));
            if (isImport)
                ops.Add(new SqlParameter("FROMIMPORT", "Y"));
            else
                ops.Add(new SqlParameter("FROMIMPORT", DBNull.Value));
            ops.Add(new SqlParameter("PAY1", pay1));
            ops.Add(new SqlParameter("PAY2", pay2));
            ops.Add(new SqlParameter("PAY3", pay3));
            ops.Add(new SqlParameter("TAX", tax));
            ops.Add(new SqlParameter("SAVEUNTAX", saveuntax));
            ops.Add(new SqlParameter("EXTRA01", extra01));
            ops.Add(new SqlParameter("CREATEDATE", DateTime.Now));
            ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"] == null ? "237" : HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"] == null ? "237" : HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        //由差勤系統匯入
        public static void insertData(string id, string polno, string payad, string unit, string name, string occc, string srank, string slvc, string inout, string da, string caseid, string num, string taxid, string taxid1, int pay, string memo, bool isImport)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            if (isImport)
            {
                sql = "DELETE B_SOLE WHERE IDCARD=@IDCARD AND NUM=@NUM AND DA=@DA AND CASEID=@CASEID AND FROMIMPORT='Y'";
                ops.Add(new SqlParameter("DA", da));
                ops.Add(new SqlParameter("CASEID", caseid.PadLeft(2, '0')));
                ops.Add(new SqlParameter("IDCARD", id));
                ops.Add(new SqlParameter("NUM", num));
                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
            }

            sql = @"INSERT INTO B_SOLE(S_SNID, IDCARD, MZ_POLNO, PAY_AD, MZ_UNIT, MZ_NAME, MZ_OCCC, MZ_SRANK, MZ_SLVC, DA_INOUT_GROUP, DA, CASEID, NUM, TAXES_ID, TAXES_ID1, PAY, NOTE, FROMIMPORT, CREATEDATE, CREATOR) 
                    VALUES( NEXT VALUE FOR dbo.B_SOLE_SN, @IDCARD, @MZ_POLNO, @PAY_AD, @MZ_UNIT, @MZ_NAME, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @DA_INOUT_GROUP, @DA, @CASEID, @NUM, @TAXES_ID, @TAXES_ID1, @PAY, @NOTE, @FROMIMPORT, @CREATEDATE, @CREATOR)";
            ops.Clear();
            ops.Add(new SqlParameter("IDCARD", id));
            ops.Add(new SqlParameter("MZ_POLNO", polno));
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
            ops.Add(new SqlParameter("MZ_NAME", name));
            ops.Add(new SqlParameter("MZ_OCCC", occc));
            ops.Add(new SqlParameter("MZ_SRANK", srank));
            ops.Add(new SqlParameter("MZ_SLVC", slvc));
            ops.Add(new SqlParameter("DA_INOUT_GROUP", inout));
            ops.Add(new SqlParameter("DA", da));
            ops.Add(new SqlParameter("CASEID", caseid));
            ops.Add(new SqlParameter("NUM", num));
            if (taxid == null)
                ops.Add(new SqlParameter("TAXES_ID", DBNull.Value));
            else
                ops.Add(new SqlParameter("TAXES_ID", taxid));
            if (taxid1 == null)
                ops.Add(new SqlParameter("TAXES_ID1", DBNull.Value));
            else
                ops.Add(new SqlParameter("TAXES_ID1", taxid1));
            ops.Add(new SqlParameter("PAY", pay));
            ops.Add(new SqlParameter("NOTE", memo));
            if (isImport)
                ops.Add(new SqlParameter("FROMIMPORT", "Y"));
            else
                ops.Add(new SqlParameter("FROMIMPORT", DBNull.Value));
            ops.Add(new SqlParameter("CREATEDATE", DateTime.Now));
            ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        //2013/04/11 目前只有B_SalarySole1有用二代健保
        public static void insertData(string id, string polno, string payad, string name, string occc, string srank, string slvc, string inout, string da, string caseid, string num, string taxid, string taxid1, int pay, string memo, int pay1, int pay2, int pay3, int tax, int saveuntax, int extra01, int Second_Health_Pay , string External,  bool isImport)
        {
            try
            {
                string sql;
                List<SqlParameter> ops = new List<SqlParameter>();
                               

                sql = @"INSERT INTO B_SOLE(S_SNID, IDCARD, MZ_POLNO, PAY_AD, MZ_UNIT, MZ_NAME, MZ_OCCC, MZ_SRANK, MZ_SLVC, DA_INOUT_GROUP, DA, CASEID, NUM, TAXES_ID, TAXES_ID1, PAY, NOTE, FROMIMPORT,PAY1,PAY2,PAY3,TAX,SAVEUNTAX, EXTRA01, CREATEDATE, CREATOR,SECOND_HEALTHPAY_PAY,EXTERNAL) 
                    VALUES( NEXT VALUE FOR dbo.B_SOLE_SN, @IDCARD, @MZ_POLNO, @PAY_AD, @MZ_UNIT, @MZ_NAME, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @DA_INOUT_GROUP, @DA, @CASEID, @NUM, @TAXES_ID, @TAXES_ID1, @PAY, @NOTE, @FROMIMPORT, @PAY1, @PAY2, @PAY3, @TAX, @SAVEUNTAX, @EXTRA01, @CREATEDATE, @CREATOR ,@SECOND_HEALTHPAY_PAY,@EXTERNAL)";
                ops.Clear();
                ops.Add(new SqlParameter("IDCARD", id));
                ops.Add(new SqlParameter("MZ_POLNO", polno));
                ops.Add(new SqlParameter("PAY_AD", payad));
                if (polno == null)
                    ops.Add(new SqlParameter("MZ_UNIT", DBNull.Value));
                else
                    ops.Add(new SqlParameter("MZ_UNIT", polno.Length >= 4 ? polno.Substring(0, 4) : ""));
                ops.Add(new SqlParameter("MZ_NAME", name));
                if (occc == null)
                    ops.Add(new SqlParameter("MZ_OCCC", DBNull.Value));
                else
                    ops.Add(new SqlParameter("MZ_OCCC", occc));
                if (srank == null)
                    ops.Add(new SqlParameter("MZ_SRANK", DBNull.Value));
                else
                { 
                    ops.Add(new SqlParameter("MZ_SRANK", srank));
                }
                if (slvc == null)
                    ops.Add(new SqlParameter("MZ_SLVC", DBNull.Value));
                else
                {//2013/04/08 by 立廷
                    //ops.Add(new SqlParameter("MZ_SLVC", occc));
                    ops.Add(new SqlParameter("MZ_SLVC", slvc));
                }
                ops.Add(new SqlParameter("DA_INOUT_GROUP", inout));
                ops.Add(new SqlParameter("DA", da));
                ops.Add(new SqlParameter("CASEID", caseid));
                ops.Add(new SqlParameter("NUM", num));
                if (taxid == null)
                    ops.Add(new SqlParameter("TAXES_ID", DBNull.Value));
                else
                    ops.Add(new SqlParameter("TAXES_ID", taxid));
                if (taxid1 == null)
                    ops.Add(new SqlParameter("TAXES_ID1", DBNull.Value));
                else
                    ops.Add(new SqlParameter("TAXES_ID1", taxid1));
                ops.Add(new SqlParameter("PAY", pay));
                ops.Add(new SqlParameter("NOTE", memo));
                if (isImport)
                    ops.Add(new SqlParameter("FROMIMPORT", "Y"));
                else
                    ops.Add(new SqlParameter("FROMIMPORT", DBNull.Value));
                ops.Add(new SqlParameter("PAY1", pay1));
                ops.Add(new SqlParameter("PAY2", pay2));
                ops.Add(new SqlParameter("PAY3", pay3));
                ops.Add(new SqlParameter("TAX", tax));
                ops.Add(new SqlParameter("SAVEUNTAX", saveuntax));
                ops.Add(new SqlParameter("EXTRA01", extra01));
                ops.Add(new SqlParameter("CREATEDATE", DateTime.Now));
                ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));


                ////
                ops.Add(new SqlParameter("SECOND_HEALTHPAY_PAY", Second_Health_Pay));
                ops.Add(new SqlParameter("EXTERNAL", External));
                ////

                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"] == null ? "237" : HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
                {
                    TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"] == null ? "237" : HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ops));
                }
            }
            catch
            {
                insertData(id, polno, payad, name, occc, srank, slvc, inout, da, caseid, num, taxid, taxid1, pay, memo, pay1, pay2, pay3, tax, saveuntax, extra01, isImport);
       
            }
        }


        /// <summary>
        /// 填入單一發放項目代碼
        /// </summary>
        /// <param name="ddl">下拉式選單</param>
        public static void fillItems(ref System.Web.UI.WebControls.DropDownList ddl)
        {
            string sql = @"SELECT * FROM B_SOLEITEM ORDER BY ID";

            ddl.DataValueField = "ID";
            ddl.DataTextField = "NAME";
            ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            ddl.DataBind();
        }

        public static void setDDLTaxID(ref System.Web.UI.WebControls.DropDownList ddl, string num)
        {
            string sql = @"(SELECT DISTINCT B_TAXES_IDTYPE.TAXES_ID, TAXES_NOTE TAXES_NOTE FROM B_SOLEITEM RIGHT JOIN B_TAXES_IDTYPE ON B_SOLEITEM.TAXES_ID=B_TAXES_IDTYPE.TAXES_ID WHERE B_SOLEITEM.ID=@NUM AND TAXES_ID1 IS NULL ) 
                            UNION
                            (SELECT DISTINCT B_TAXES_IDTYPE.TAXES_ID, B_TAXES_IDTYPE.TAXES_ID TAXES_NOTE FROM B_SOLEITEM RIGHT JOIN B_TAXES_IDTYPE ON B_SOLEITEM.TAXES_ID=B_TAXES_IDTYPE.TAXES_ID WHERE B_SOLEITEM.ID=@NUM AND  TAXES_ID1 IS NOT NULL ) ";
            List<SqlParameter> ops = new List<SqlParameter>();

            ops.Add(new SqlParameter("NUM", num));

            ddl.DataValueField = "TAXES_ID";
            ddl.DataTextField = "TAXES_NOTE";
            ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            ddl.DataBind();

            if (ddl.Items.Count == 0)
                ddl.Visible = false;
            else
                ddl.Visible = true;
        }

        public static void setDDLTaxID1(ref System.Web.UI.WebControls.DropDownList ddl, string tax)
        {
            //2013/01/17 單一發放資料建立時，發放項目為30提貨卷，所得項目的預設選項為：其他
            string sql = "SELECT TAXES_ID1, TAXES_NOTE FROM B_TAXES_IDTYPE WHERE TAXES_ID1 IS NOT NULL AND TAXES_ID=@TAXES_ID ORDER BY TAXES_ID1 desc";
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            ops.Add(new SqlParameter("TAXES_ID", tax));

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            ddl.DataValueField = "TAXES_ID1";
            ddl.DataTextField = "TAXES_NOTE";
            ddl.DataSource = dt;
            ddl.DataBind();

            if (ddl.Items.Count == 0)
                ddl.Visible = false;
            else
                ddl.Visible = true;
        }

        public static bool isLocked(string da, int caseid)
        {
            
            string sql = "SELECT COUNT(*) FROM B_SOLE WHERE DA=@DA AND CASEID=@CASEID AND LOCKDB='Y'";
            List<SqlParameter> ops = new List<SqlParameter>();

            ops.Add(new SqlParameter("DA", da));
            ops.Add(new SqlParameter("CASEID", SqlDbType.VarChar) { Value=caseid});

            if (int.Parse(o_DBFactory.ABC_toTest.GetValue(sql, ops)) > 0)
                return true;

            return false;
        }

        public static bool isLocked(string da, int caseid,string payAd)
        {

            //2013/01/28 單一發放整批匯入時，應依「登入者單位/入帳日期/入帳案號/結案狀態」決定是否可新增
            //string sql = "SELECT COUNT(*) FROM B_SOLE WHERE DA=@DA AND CASEID=@CASEID AND LOCKDB='Y'";
            string sql = "SELECT COUNT(*) FROM B_SOLE WHERE PAY_AD=@PAY_AD AND DA=@DA AND CASEID=@CASEID AND LOCKDB='Y'";
            List<SqlParameter> ops = new List<SqlParameter>();

            //2013/01/28
            ops.Add(new SqlParameter("PAY_AD", payAd));
            ops.Add(new SqlParameter("DA", da));
            ops.Add(new SqlParameter("CASEID", caseid));

            if (int.Parse(o_DBFactory.ABC_toTest.GetValue(sql, ops)) > 0)
                return true;

            return false;
        }
        /// <summary>
        /// 取得單一發放資料是否已存在
        /// </summary>
        /// <param name="da">日期</param>
        /// <param name="caseid">批號</param>
        /// <param name="item">項目</param>
        /// <returns></returns>
        public static bool hasData(string da, int caseid, string item)
        {
            string sql = "SELECT COUNT(*) FROM B_SOLE WHERE DA=@DA AND CASEID=@CASEID AND NUM=@NUM";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("DA", da));
            ps.Add(new SqlParameter("CASEID", caseid));
            ps.Add(new SqlParameter("NUM", item));

            if (int.Parse(o_DBFactory.ABC_toTest.GetValue(sql, ps)) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 關帳
        /// </summary>
        /// <param name="payad">發薪機關</param>
        /// <param name="da">資料日期</param>
        /// <param name="batch">批號</param>
        public static void lockDB(string payad, string da, string batch)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();

            sql = "UPDATE B_SOLE SET LOCKDB='Y' WHERE PAY_AD=@PAYAD AND DA=@DA AND CASEID=@BATCH";
            ps.Add(new SqlParameter("PAYAD", payad));
            ps.Add(new SqlParameter("DA", da));
            ps.Add(new SqlParameter("BATCH", batch));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }
    }
}
