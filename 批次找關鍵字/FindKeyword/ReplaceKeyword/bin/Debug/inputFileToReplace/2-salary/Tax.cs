using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.IO;

namespace TPPDDB._2_salary
{
    [Serializable]
    public class Tax
    {
        public int sn;
        public string year;
        public string payad;
        public string chiad;
        public string idcard;
        public string ticketnum;
        public string name;
        public string occc;
        public string chioccc;
        public string srank;
        public string slvc;
        public string spt;
        public string unit;
        public string chiunit;
        public string polno;
        public string taxType;
        public string address;
        public bool lockdb;
        public int monthPay;
        public int monthTax;
        public int repairPay;
        public int repairTax;
        public int solePay;
        public int soleTax;
        public int yearPay;
        public int yearTax;
        public int effectPay;
        public int effectTax;
        public int bosspay;
        public int otherSub;
        /// <summary>
        /// 自行沖銷稅額
        /// </summary>
        public int othersubTax;
        /// <summary>
        /// 免納所得
        /// </summary>
        public int decrease;
        /// <summary>
        /// 勞工自提退休金
        /// </summary>
        public int laborpay;

        /// <summary>
        /// 退撫免稅額
        /// </summary>
        public int CONCUR3PAY;

        public bool day183;

        public DataTable salaryData;

        public Tax(int sn)
        {
            DataTable dt;
            string sql = @"SELECT B_TAXES.*, AD.MZ_KCHI CHIAD, UNIT.MZ_KCHI CHIUNIT, OCCC.MZ_KCHI CHIOCCC FROM B_TAXES 
                            LEFT JOIN A_KTYPE AD ON PAY_AD=AD.MZ_KCODE AND AD.MZ_KTYPE='04' 
                            LEFT JOIN A_KTYPE UNIT ON MZ_UNIT=UNIT.MZ_KCODE AND UNIT.MZ_KTYPE='25' 
                            LEFT JOIN A_KTYPE OCCC ON MZ_OCCC=OCCC.MZ_KCODE AND OCCC.MZ_KTYPE='26' 
                            WHERE T_SNID=@SN";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("SN", sn));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ps);

            if (dt.Rows.Count == 0)
                return;

            this.sn = sn;
            year = dt.Rows[0]["AYEAR"].ToString();
            payad = dt.Rows[0]["PAY_AD"].ToString();
            chiad = dt.Rows[0]["CHIAD"].ToString();
            idcard = dt.Rows[0]["IDCARD"].ToString();
            ticketnum = dt.Rows[0]["TICKETNUM"].ToString();
            name = dt.Rows[0]["MZ_NAME"].ToString();
            occc = dt.Rows[0]["MZ_OCCC"].ToString();
            chioccc = dt.Rows[0]["CHIOCCC"].ToString();
            srank = dt.Rows[0]["MZ_SRANK"].ToString();
            slvc = dt.Rows[0]["MZ_SLVC"].ToString();
            spt = dt.Rows[0]["MZ_SPT"].ToString();
            unit = dt.Rows[0]["MZ_UNIT"].ToString();
            chiunit = dt.Rows[0]["CHIUNIT"].ToString();
            polno = dt.Rows[0]["MZ_POLNO"].ToString();
            taxType = dt.Rows[0]["TAX_TYPE"].ToString();
            address = dt.Rows[0]["ADDRESS1"].ToString();
            lockdb = dt.Rows[0]["LOCKDB"].ToString() == "" ? false : true;
            int.TryParse(dt.Rows[0]["MONTHPAY"].ToString(), out monthPay);
            int.TryParse(dt.Rows[0]["MONTHPAYTAX"].ToString(), out monthTax);
            int.TryParse(dt.Rows[0]["REPAIR"].ToString(), out repairPay);
            int.TryParse(dt.Rows[0]["REPAIRTAX"].ToString(), out repairTax);
            int.TryParse(dt.Rows[0]["SOLE"].ToString(), out solePay);
            int.TryParse(dt.Rows[0]["SOLETAX"].ToString(), out soleTax);
            int.TryParse(dt.Rows[0]["YEARPAY"].ToString(), out yearPay);
            int.TryParse(dt.Rows[0]["YEARPAYTAX"].ToString(), out yearTax);
            int.TryParse(dt.Rows[0]["EFFECT"].ToString(), out effectPay);
            int.TryParse(dt.Rows[0]["EFFECTTAX"].ToString(), out effectTax);
            int.TryParse(dt.Rows[0]["BOSSPAY"].ToString(), out bosspay);
            int.TryParse(dt.Rows[0]["OTHERSUB"].ToString(), out otherSub);
            int.TryParse(dt.Rows[0]["OTHERSUBTAX"].ToString(), out othersubTax);
            int.TryParse(dt.Rows[0]["DECREASE"].ToString(), out decrease);
            int.TryParse(dt.Rows[0]["LABORPAY"].ToString(), out laborpay);
            int.TryParse(dt.Rows[0]["CONCUR3PAY"].ToString(), out CONCUR3PAY);            
            day183 = dt.Rows[0]["DAY183"].ToString() == "Y" ? true : false;

            sql = "SELECT AMONTH, (SALARYPAY1+WORKP+PROFESS+BOSS+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC+OTHERADD) SALARYPAY1, BOSS, TAX FROM B_MONTHPAY_MAIN WHERE AMONTH LIKE @YEAR AND IDCARD=@IDCARD AND PAY_AD=@PAYAD ORDER BY AMONTH";
            ps.Clear();
            ps.Add(new SqlParameter("YEAR", this.year + "%"));
            ps.Add(new SqlParameter("IDCARD", this.idcard));
            ps.Add(new SqlParameter("PAYAD", this.payad));
            this.salaryData = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
        }

        /// <summary>
        /// 編輯所得稅資料
        /// </summary>
        /// <param name="othersub">自行沖銷</param>
        /// <param name="othersubTax">自行沖銷稅額</param>
        /// <param name="decrease">免納所得金額</param>
        /// <param name="laborpay">勞工自提退休金</param>
        /// <param name="address">戶藉地</param>
        public bool update(int othersub, int othersubTax, int decrease, int laborpay, string address, int? _CONCUR3PAY)
        {
            bool isOk;
            string sql = @"UPDATE B_TAXES SET OTHERSUB=@OTHERSUB, OTHERSUBTAX=@OTHERSUBTAX, DECREASE=@DECREASE, LABORPAY=@LABORPAY, ADDRESS1=@ADDRESS1 
, CONCUR3PAY=@CONCUR3PAY 
WHERE T_SNID=@SN";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("SN", this.sn));
            ps.Add(new SqlParameter("OTHERSUB", othersub));
            ps.Add(new SqlParameter("OTHERSUBTAX", othersubTax));
            ps.Add(new SqlParameter("DECREASE", decrease));
            ps.Add(new SqlParameter("LABORPAY", laborpay));
            ps.Add(new SqlParameter("ADDRESS1", address));
            ps.Add(new SqlParameter("CONCUR3PAY", _CONCUR3PAY));
            isOk = o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
            if (isOk)
            {
                //如果戶藉地有異動，也要更新人事資料的戶藉地MZ_ADD1
                if (this.address != address)
                {
                    ps = new List<SqlParameter>();

                    sql = "UPDATE A_DLBASE SET MZ_ADD1=@ADDRESS WHERE MZ_ID=@ID";
                    ps.Add(new SqlParameter("ADDRESS", address));
                    ps.Add(new SqlParameter("ID", this.idcard));

                    isOk =  o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
                }
            }
            return isOk;
        }

        public void update(int monthpay, int monthtax, int repairpay, int repairtax, int solepay, int soletax, int yearpay, int yeartax, int effectpay, int effecttax, int othersub, int laborpay)
        {
            string sql = @"UPDATE B_TAXES SET MONTHPAY=@MONTHPAY, MONTHPAYTAX=@MONTHPAYTAX, REPAIR=@REPAIR, REPAIRTAX=@REPAIRTAX, SOLE=@SOLE, SOLETAX=@SOLETAX, 
                            YEARPAY=@YEARPAY, YEARPAYTAX=@YEARPAYTAX, EFFECT=@EFFECT, EFFECTTAX=@EFFECTTAX, OTHERSUB=@OTHERSUB, LABORPAY=@LABORPAY WHERE T_SNID=@SN";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("SN", this.sn));
            ps.Add(new SqlParameter("MONTHPAY", monthpay));
            ps.Add(new SqlParameter("MONTHPAYTAX", monthtax));
            ps.Add(new SqlParameter("REPAIR", repairpay));
            ps.Add(new SqlParameter("REPAIRTAX", repairtax));
            ps.Add(new SqlParameter("SOLE", solepay));
            ps.Add(new SqlParameter("SOLETAX", soletax));
            ps.Add(new SqlParameter("YEARPAY", yearpay));
            ps.Add(new SqlParameter("YEARPAYTAX", yeartax));
            ps.Add(new SqlParameter("EFFECT", effectpay));
            ps.Add(new SqlParameter("EFFECTTAX", effecttax));
            ps.Add(new SqlParameter("OTHERSUB", othersub));
            ps.Add(new SqlParameter("LABORPAY", laborpay));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }

        /// <summary>
        /// 刪除所得稅資料
        /// </summary>
        public void delete()
        {
            string sql = @"DELETE B_TAXES WHERE T_SNID=@SN";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("SN", this.sn));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }

        /// <summary>
        /// 產生所得稅資料
        /// </summary>
        /// <param name="payad">機關</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public static DataTable createTax(string payad, string year, string type, string idno)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt;
            DataTable dtError = new DataTable();
            dtError.Columns.Add("idno");

            if (type == "" && idno == "")
            {
                //刪除資料機關、年度的資料
                sql = "DELETE B_TAXES WHERE AYEAR=@YEAR AND PAY_AD=@PAYAD";
                ops.Add(new SqlParameter("YEAR", year));
                ops.Add(new SqlParameter("PAYAD", payad));
                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                // 從view取得所有的所得稅資料
                sql = "SELECT * FROM VW_TAX_ALL WHERE LEN(RTRIM(taxes_id))>0 AND AYEAR=@YEAR AND PAY_AD=@PAYAD";
                dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            }
            else
            {
                if (idno == "")
                {
                    //刪除資料機關、年度的資料
                    sql = "DELETE B_TAXES WHERE AYEAR=@YEAR AND PAY_AD=@PAYAD AND TAX_TYPE=@TYPE";
                    ops.Add(new SqlParameter("YEAR", year));
                    ops.Add(new SqlParameter("PAYAD", payad));
                    ops.Add(new SqlParameter("TYPE", type));
                    o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                    // 從view取得所有的所得稅資料
                    sql = "SELECT * FROM VW_TAX_ALL WHERE LEN(RTRIM(taxes_id))>0 AND AYEAR=@YEAR AND PAY_AD=@PAYAD AND TAXES_ID=@TYPE";
                    dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
                }
                else
                {
                    //刪除資料機關、年度的資料
                    sql = "DELETE B_TAXES WHERE AYEAR=@YEAR AND PAY_AD=@PAYAD AND IDCARD=@IDCARD";
                    ops.Clear();
                    ops.Add(new SqlParameter("YEAR", year));
                    ops.Add(new SqlParameter("PAYAD", payad));
                    ops.Add(new SqlParameter("IDCARD", idno));
                    o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                    // 從view取得所有的所得稅資料
                    sql = "SELECT * FROM VW_TAX_ALL WHERE LEN(RTRIM(taxes_id))>0 AND AYEAR=@YEAR AND PAY_AD=@PAYAD  AND IDNO=@IDNO";
                    ops.Clear();
                    ops.Add(new SqlParameter("YEAR", year));
                    ops.Add(new SqlParameter("PAYAD", payad));
                    ops.Add(new SqlParameter("IDNO", idno));
                    dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
                }
            }
            // 逐筆存入資料庫
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                i++;
                ops.Clear();
                sql = @"INSERT INTO B_TAXES(T_SNID, AYEAR, PAY_AD, IDCARD, MZ_NAME, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT, MZ_UNIT, MZ_POLNO, TICKETNUM, TAX_TYPE, ADDRESS1, MONTHPAY, REPAIR, SOLE, YEARPAY, EFFECT, MONTHPAYTAX, REPAIRTAX, SOLETAX, YEARPAYTAX, EFFECTTAX, BOSSPAY,ADDUSER,ADDDATE,CONCUR3PAY) 
                        VALUES( NEXT VALUE FOR dbo.B_TAXES_SN, @AYEAR, @PAY_AD, @IDCARD, @MZ_NAME, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @MZ_SPT, @MZ_UNIT, @MZ_POLNO, @TICKETNUM, @TAX_TYPE, @ADDRESS1, @MONTHPAY, @REPAIR, @SOLE, @YEARPAY, @EFFECT, @MONTHPAYTAX, @REPAIRTAX, @SOLETAX, @YEARPAYTAX, @EFFECTTAX, @BOSSPAY,@ADDUSER,@ADDDATE
                        ,@CONCUR3PAY
                        )
                        ";
                ops.Add(new SqlParameter("AYEAR", row["AYEAR"].ToString()));
                ops.Add(new SqlParameter("PAY_AD", row["PAY_AD"].ToString()));
                ops.Add(new SqlParameter("IDCARD", row["IDNO"].ToString()));
                ops.Add(new SqlParameter("MZ_NAME", row["MZ_NAME"].ToString()));
                ops.Add(new SqlParameter("MZ_OCCC", row["MZ_OCCC"].ToString()));
                ops.Add(new SqlParameter("MZ_SRANK", row["MZ_SRANK"].ToString()));
                ops.Add(new SqlParameter("MZ_SLVC", row["MZ_SLVC"].ToString()));
                ops.Add(new SqlParameter("MZ_SPT", row["MZ_SPT"].ToString()));
                ops.Add(new SqlParameter("MZ_UNIT", row["MZ_UNIT"].ToString()));
                ops.Add(new SqlParameter("MZ_POLNO", row["MZ_POLNO"].ToString()));
                ops.Add(new SqlParameter("TICKETNUM", i.ToString().PadLeft(7, '0')));
                ops.Add(new SqlParameter("TAX_TYPE", row["TAXES_ID"].ToString()));
                ops.Add(new SqlParameter("ADDRESS1", row["MZ_ADD1"].ToString()));
                ops.Add(new SqlParameter("MONTHPAY", Convert.ToInt32(row["SALARY"])));
                ops.Add(new SqlParameter("REPAIR", Convert.ToInt32(row["REPAIR"])));
                ops.Add(new SqlParameter("SOLE", Convert.ToInt32(row["SOLE"])));
                ops.Add(new SqlParameter("YEARPAY", Convert.ToInt32(row["YEARPAY"])));
                ops.Add(new SqlParameter("EFFECT", Convert.ToInt32(row["EFFECT"])));
                ops.Add(new SqlParameter("MONTHPAYTAX", Convert.ToInt32(row["SALARYTAX"])));
                ops.Add(new SqlParameter("REPAIRTAX", Convert.ToInt32(row["REPAIRTAX"])));
                ops.Add(new SqlParameter("SOLETAX", Convert.ToInt32(row["SOLETAX"])));
                ops.Add(new SqlParameter("YEARPAYTAX", Convert.ToInt32(row["YEARPAYTAX"])));
                ops.Add(new SqlParameter("EFFECTTAX", Convert.ToInt32(row["EFFECTTAX"])));
                ops.Add(new SqlParameter("BOSSPAY", Convert.ToInt32(row["BOSS"])));
                ops.Add(new SqlParameter("ADDUSER", HttpContext.Current.Session["ADPMZ_ID"].ToString()));
                ops.Add(new SqlParameter("ADDDATE", DateTime.Now));
                //追加退撫免稅額綁定
                ops.Add(new SqlParameter("CONCUR3PAY", Convert.ToInt32(row["CONCUR3PAY"])));
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
                }
                catch
                {
                    DataRow dr = dtError.NewRow();

                    dr["idno"] = row["IDNO"].ToString();

                    dtError.Rows.Add(dr);
                }
            }

            return dtError;
        }

        /// <summary>
        /// 取得所得稅資料
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="payad">機關</param>
        /// <param name="unit">單位，null=不列條件</param>
        /// <param name="taxType">所得格式，null=不列條件</param>
        /// <param name="id">身份證字號，null=不列條件</param>
        /// <param name="name">姓名，null=不列條件</param>
        /// <returns></returns>
        public static DataTable getTaxes(string year, string payad, string unit, string taxType, string id, string name)
        {
            DataTable dt;
            List<SqlParameter> ps = new List<SqlParameter>();
            string sql = "SELECT * FROM B_TAXES WHERE AYEAR=@AYEAR AND PAY_AD=@PAY_AD ";

            if (unit != null)
            {
                sql += " AND MZ_UNIT=@unit";
                ps.Add(new SqlParameter("unit", unit));
            }
            if (taxType != null)
            {
                sql += " AND TAX_TYPE=@type";
                ps.Add(new SqlParameter("type", taxType));
            }
            if (id != null)
            {
                sql += " AND IDCARD=@id";
                ps.Add(new SqlParameter("id", id));
            }
            if (name != null)
            {
                sql += " AND MZ_NAME LIKE @name";
                ps.Add(new SqlParameter("name", "%" + name + "%"));
            }

            sql += " ORDER BY MZ_UNIT, MZ_POLNO, TAX_TYPE";
            ps.Add(new SqlParameter("AYEAR", year));
            ps.Add(new SqlParameter("PAY_AD", payad));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            if (dt.Rows.Count == 0)
                return null;
            return dt;
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

            sql = "UPDATE B_TAXES SET LOCKDB='Y' WHERE PAY_AD=@PAYAD AND AYEAR=@AYEAR";
            ps.Add(new SqlParameter("PAYAD", payad));
            ps.Add(new SqlParameter("AYEAR", ayear));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }

        /// <summary>
        /// 產生媒體申報電子檔
        /// </summary>
        /// <param name="branch">機關</param>
        /// <param name="year">年份(民國年)</param>
        /// <param name="taxType">所得格式，null=不列入條件</param>
        /// <returns></returns>
        public static string makeTaxMedia(Branch branch, string year, string taxType)
        {
            var db = o_DBFactory.ABC_toTest;
            DataTable dt = new DataTable();
            string strSQL;
            int i;
            int westyear = int.Parse(year) + 1911;
            string strMediaTxt;
            string strUnitCode;//縣市別+機關別
            string strSerial;//流水號
            string strUnitID;//申報單位統一編號
            string strMark;//註記
            string strType;//格式
            string strIDCard;//所得人統一編號
            string strIDType;//證號別
            string strTotalPay;//扣繳憑單給付總額
            string strTotalWH;//扣繳憑單扣繳稅額
            string strNet;//扣繳憑單給付淨額
            string strMix1;//複合欄位
            string strSoftMark;//軟體註記~ 這什麼鬼 A也不是B也不是；B好像比較合
            string strErrorMark;//錯誤註記~ 火大 我就當身份證都對
            string strYear;//所得給付年度
            string strName;//所得人姓名
            string strAddress;//所得人地址
            string strPeriod;//所得期間
            string strMix2;//複合欄位2
            string strDay183;//是否滿183天
            string strNationCode;//國家代碼
            string strRentCode;//租稅協定代碼
            string strEmpty;//空白~ 別問我為什麼 去跟法官講
            string strFileDate;//檔案製作日期

            strMediaTxt = "";

            i = 1;
            strPeriod = year + "01" + year + "12";
            strUnitCode = branch.taxunit;
            strUnitID = branch.taxinvoice;
            strYear = year;
            strDay183 = " ";
            strNationCode = " ".PadRight(2);
            strRentCode = " ".PadRight(2);
            strEmpty = " ".PadRight(2);
            strFileDate = System.DateTime.Now.ToString("MMdd");
            int j = 0;

            List<SqlParameter> ps = new List<SqlParameter>();
            //2012/12/13更換成VW
            //strSQL = " SELECT * FROM B_TAXES WHERE PAY_AD=@ad AND AYEAR=@year";
            strSQL = " SELECT * FROM VW_TAX_INCOME WHERE IDNO IS NOT NULL AND LEN(RTRIM(TAXES_ID)) > 0 AND PAY_AD=@ad AND AYEAR=@year";
            switch (taxType)
            {
                //strSQL += " AND TAX_TYPE=@type";
                case "50":
                    strSQL += " AND TAXES_ID=@type";
                    ps.Add(new SqlParameter("type", taxType));
                    break;
                case "其他格式":
                    strSQL += " AND TAXES_ID <> '50'";
                    break;
                case "全部":
                    break;
            }


            strSQL += " ORDER BY MZ_UNIT, MZ_POLNO";

            ps.Add(new SqlParameter("ad", branch.id));
            ps.Add(new SqlParameter("year", year));
            dt = o_DBFactory.ABC_toTest.DataSelect(strSQL, ps);

            if (dt.Rows.Count == 0)
                return "";

            foreach (DataRow item in dt.Rows)
            {
                j++;

                if (item["TAXES_ID"].ToString().Length > 2)
                    strMark = item["TAXES_ID"].ToString().Substring(2, 1);
                else
                    strMark = " ";
                int tempBoss;
                int tempDecrease;
                int.TryParse(item["BOSS"].ToString(), out tempBoss);
                int.TryParse(item["DECREASE"].ToString(), out tempDecrease);
                strType = item["TAXES_ID"].ToString().Substring(0, 2);
                //2012/12/13 改成縣市別+機關別 + 5碼流水號
                strSerial = strUnitCode + i.ToString().PadLeft(5, '0');

                //2013/01/24 身份證10碼靠右，不足補0
                strIDCard = item["IDNO"].ToString().PadRight(10, ' ');
                
                //if (strIDCard == "U120152334")
                //{

                int othersub = 0;
                int othersubtax = 0;
                int larborpay = 0;
                int decrease = 0;
                int CONCUR3PAY = 0;//退撫金稅額

                DataTable tempDt = o_DBFactory.ABC_toTest.Create_Table(string.Format("SELECT OTHERSUB,LABORPAY,OTHERSUBTAX,DECREASE,CONCUR3PAY FROM B_TAXES WHERE TAX_TYPE='{0}' AND  IDCARD='{1}' AND PAY_AD='{2}' AND AYEAR='{3}'", item["TAXES_ID"].ToString(), item["IDNO"].ToString(), branch.id, year), "GET");
                if (tempDt.Rows.Count > 0)
                {
                    othersub = int.Parse(tempDt.Rows[0]["OTHERSUB"].ToString());
                    othersubtax = int.Parse(tempDt.Rows[0]["OTHERSUBTAX"].ToString());
                    larborpay = int.Parse(tempDt.Rows[0]["LABORPAY"].ToString());
                    decrease = int.Parse(tempDt.Rows[0]["DECREASE"].ToString());
                    CONCUR3PAY = int.Parse(tempDt.Rows[0]["CONCUR3PAY"].ToString());
                }

                //2013/01/24  身份證欄維10碼，則為個人
                if (item["IDNO"].ToString().Length == 10)
                {
                    strIDType = "0";
                }
                else
                {
                    strIDType = "1";
                }

                strTotalPay = (int.Parse(item["SALARY"].ToString()) +
                                            int.Parse(item["REPAIR"].ToString()) +
                                            int.Parse(item["SOLE"].ToString()) +
                                            int.Parse(item["YEARPAY"].ToString()) +
                                            int.Parse(item["EFFECT"].ToString())
                                            //減去 主管加給 和免稅額
                                            - tempBoss - decrease
                                            //2013/01/17 少扣 小隊長修改
                                            /*減去 自行沖銷稅額*/
                                            - othersub
                                            /*減去 勞工自提退休金*/
                                            - larborpay
                                            /*減去 退撫金稅額*/
                                            - CONCUR3PAY 
                                            ).ToString().PadLeft(10, '0');
                //strTotalPay = (int.Parse(item["MONTHPAY"].ToString()) +
                //                            int.Parse(item["REPAIR"].ToString()) +
                //                            int.Parse(item["SOLE"].ToString()) +
                //                            int.Parse(item["YEARPAY"].ToString()) +
                //                            int.Parse(item["EFFECT"].ToString())).ToString().PadLeft(10, '0');
                strTotalWH = (int.Parse(item["SALARYTAX"].ToString()) +
                                            int.Parse(item["REPAIRTAX"].ToString()) +
                                            int.Parse(item["SOLETAX"].ToString()) +
                                            int.Parse(item["YEARPAYTAX"].ToString()) +
                                            int.Parse(item["EFFECTTAX"].ToString()) -
                                            othersubtax).ToString().PadLeft(10, '0'); //2013/01/17 少扣 小隊長修改

                //strTotalWH = (int.Parse(item["MONTHPAYTAX"].ToString()) +
                //                            int.Parse(item["REPAIRTAX"].ToString()) +
                //                            int.Parse(item["SOLETAX"].ToString()) +
                //                            int.Parse(item["YEARPAYTAX"].ToString()) +
                //                            int.Parse(item["EFFECTTAX"].ToString())).ToString().PadLeft(10, '0');
                try
                {
                    strNet = (int.Parse(strTotalPay) - int.Parse(strTotalWH)).ToString().PadLeft(10, '0');
                }
                catch /*(Exception ex)*/
                {
                    strNet = "0".PadLeft(10, '0');
                }

                strSoftMark = "X";
                strErrorMark = " ";

                //姓名的處理(補滿6個全形字)
                strName = Strings.StrConv(item["MZ_NAME"].ToString().Replace(" ", "").Replace("  ", ""), VbStrConv.Wide, 0);
                if (!string.IsNullOrEmpty(strName))
                {
                    strName = strName.PadRight(20, '　');
                }
                else
                {
                    strName = "  ".PadLeft(20);
                }

                //地址的處理(補滿30個全形字，不會有英文地址)
                strAddress = Strings.StrConv(item["MZ_ADD1"].ToString(), VbStrConv.Wide, 0);
                //strAddress = Strings.StrConv(item["ADDRESS1"].ToString(), VbStrConv.Wide, 0);
                if (!string.IsNullOrEmpty(strAddress))
                {
                    if (strAddress.Length > 30)
                    {
                        strAddress = strAddress.Substring(0, 30);
                    }
                    else
                    {
                        strAddress = strAddress.PadRight(30, '　');
                    }
                }
                else
                {
                    strAddress = "".PadRight(30, '　');
                }

                //如果都沒有，帶入空白，如果不是50 要帶入TAXES_ID1
                switch (strType)
                {
                    case "50":
                        strMix1 = " ".PadRight(12, ' ');
                        break;
                    case "51":
                        strMix1 = item["RENTNUM"].ToString().PadRight(12, ' ');
                        break;
                    default:
                        if (strType.Substring(0, 1) == "9")
                            strMix1 = item["TAXES_ID1"].ToString().PadRight(12, ' ');
                        else
                            strMix1 = " ".PadRight(12, ' ');
                        break;
                }
                //strMix1 = "J" + j.ToString().PadLeft(5, '0').PadRight(11);//item["PB_BankAccount"].ToString().PadRight(12, 'x');//所得人代號(目前是取銀行帳號，可能不對)

                strMix2 = " ".PadRight(49, ' ');//自願提繳之退休金額 //改為空白2012/12/13

                strMediaTxt += string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}\r\n",
                                              strUnitCode, strSerial, strUnitID, strMark, strType, strIDCard, strIDType, strTotalPay, strTotalWH, strNet, strMix1, strSoftMark, strErrorMark, strYear,
                                              strName, strAddress, strPeriod, strMix2, strDay183, strNationCode, strRentCode, strEmpty, strFileDate);

                i++;
                //}
            }

            return strMediaTxt;
        }

        /// <summary>
        /// 取得扣繳憑單資料
        /// </summary>
        /// <param name="branch">機關</param>
        /// <param name="year">年份(民國年)</param>
        /// <param name="unit">單位</param>
        /// <param name="idcard">身份證字號</param>
        /// <param name="type">所得格式</param>
        /// <returns></returns>
        public static DataTable getReceipt(Branch branch, string year, string unit, string idcard, string type)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> ps = new List<SqlParameter>();
            string sql = " SELECT * FROM VW_ALL_TAX_DATA WHERE PAY_AD=@ad AND AYEAR=@year ";

            if (unit != null)
            {
                sql += " AND MZ_UNIT=@unit";
                ps.Add(new SqlParameter("unit", unit));
            }
            if (idcard != null)
            {
                sql += " AND IDNO=@id";
                ps.Add(new SqlParameter("id", idcard));
            }
            if (type != null)
            {
                sql += " AND TAX_TYPE=@type";
                ps.Add(new SqlParameter("type", type));
            }

            sql += " ORDER BY MZ_UNIT, MZ_POLNO";
            ps.Add(new SqlParameter("ad", branch.id));
            ps.Add(new SqlParameter("year", year));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            dt.Columns.Add("SERIALNUMBER");

            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                row["SERIALNUMBER"] = "A" + i.ToString("0000000").PadLeft(7);
                i++;
            }

            return dt;
        }

        /// <summary>
        /// 取得所得稅率
        /// </summary>
        /// <returns></returns>
        public static float getTaxPercent()
        {
            string sql;
            float result;

            sql = "SELECT TAX_PERCENTAGE FROM B_TAX_SET";

            float.TryParse(o_DBFactory.ABC_toTest.GetValue(sql), out result);

            return result;
        }

        /// <summary>
        /// 取得年終獎金、考績獎金需扣所得稅起始金額
        /// </summary>
        /// <returns></returns>
        public static int getTaxStart()
        {
            string sql;
            int result;

            sql = "SELECT TAX_START FROM B_TAX_SET";

            int.TryParse(o_DBFactory.ABC_toTest.GetValue(sql), out result);

            return result;
        }

        /// <summary>
        /// 更新免稅金額
        /// </summary>
        public static bool updateDecrease(string id, int amount, string year, string pay_ad)
        {
            List<SqlParameter> ps = new List<SqlParameter>();
            string sql = @"SELECT * FROM B_TAXES WHERE AYEAR=@AYEAR AND IDCARD =@IDCARD AND PAY_AD=@PAY_AD AND TAX_TYPE = '50'";
            ps.Add(new SqlParameter("AYEAR", year));
            ps.Add(new SqlParameter("IDCARD", id));
            ps.Add(new SqlParameter("PAY_AD", pay_ad));

            DataTable dt = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            if (dt.Rows.Count != 1)
                return false;
            else
            {
                ps = new List<SqlParameter>();
                sql = @"UPDATE B_TAXES SET DECREASE=@DECREASE WHERE T_SNID=@T_SNID";
                ps.Add(new SqlParameter("T_SNID", dt.Rows[0][0].ToString()));
                ps.Add(new SqlParameter("DECREASE", amount));
                o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
                return true;
            }
        }
    }
}
