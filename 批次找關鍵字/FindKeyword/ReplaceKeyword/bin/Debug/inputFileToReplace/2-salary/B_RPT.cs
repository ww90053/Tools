using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public class B_RPT
    { 
        
        public class pay_compare
        {
            public static DataTable doSearch(string strAD, string TYPE, string BANKID, string CASEID, string DATADATE, string IN_ACCOUNT_DATE)
            {
                string TABLE_NAME = "";
                string WHERE = "";
                string TXT_WHERE = "";
                string SOLE_exception = "";

                switch (TYPE)
                {
                    case "MONTH":                   
                        TABLE_NAME = "dbo.VW_SALARYTOTXT";
                        WHERE = string.Format("WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' AND IN_ACCOUNT_DATE={3} ", strAD, DATADATE, BANKID, IN_ACCOUNT_DATE);
                        break;

                    case "REPAIR":
                        TABLE_NAME = "dbo.VW_REPAIRTOTXT";
                        WHERE = string.Format("WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' AND IN_ACCOUNT_DATE={3} {4} ", strAD, DATADATE, BANKID, IN_ACCOUNT_DATE, CASEID.Length == 0 ? null : "AND BATCH_NUMBER = '" + CASEID + "'");
                        TXT_WHERE = " AND CASEID='" + CASEID + "' ";
                        break;

                    case "YEAR":
                        TABLE_NAME = "dbo.VW_YEARBONUSTOTXT";
                        WHERE = string.Format("WHERE PAY_AD = '{0}' AND AYEAR = '{1}' AND BANKID='{2}' AND IN_ACCOUNT_DATE={3} ", strAD, DATADATE, BANKID, IN_ACCOUNT_DATE);
                        break;

                    case "EFFECT":
                        TABLE_NAME = "dbo.VW_EFFECTBONUSTOTXT";
                        WHERE = string.Format("WHERE PAY_AD = '{0}' AND AYEAR = '{1}' AND BANKID='{2}' AND IN_ACCOUNT_DATE={3} ", strAD, DATADATE, BANKID, IN_ACCOUNT_DATE);
                        break;

                    case "SOLE":
                        TABLE_NAME = "dbo.VW_SOLETOTXT";
                        WHERE = string.Format("WHERE PAY_AD = '{0}' AND DA = '{1}' AND BANKID='{2}' AND IN_ACCOUNT_DATE={3} {4} ", strAD, DATADATE, BANKID, IN_ACCOUNT_DATE, CASEID.Length == 0 ? null : "AND CASEID = '" + CASEID + "'");
                        TXT_WHERE = " AND CASEID='" + CASEID + "' ";
                        //SOLE_exception = "AND nvl(A1.PAY,0) = nvl( B1.PAY,0)";//單一發放因為有同一批號 同一人有兩筆的情形.要特殊處理
                        break;

                    case "OFFER":
                        TABLE_NAME = "dbo.VW_SAVELIST";
                        WHERE = string.Format("WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' AND IN_ACCOUNT_DATE={3} ", strAD, DATADATE, BANKID, IN_ACCOUNT_DATE);
                        break;

                    default:


                        break;
                }

                //            string strSQL = @"SELECT * FROM (
                //SELECT  * FROM  VW_SALARYTOTXT WHERE IN_ACCOUNT_DATE='1030105' AND PAY_AD ='382133400C'  AND AMONTH = '10301'
                //) A 
                //FULL OUTER JOIN  
                //( SELECT * FROM B_BANK_BACK_TXT WHERE IN_ACCOUNT_DATE='1030105'  ) B  
                //ON  A.IDCARD=B.IDCARD
                //
                //WHERE (nvl(A.PAY,0) != nvl( B.PAY,0) OR A.ACCOUNT_NAME !=B.NAME OR A.ACCOUNT_NO !=B.STOCKPILE_BANKID  )   ";

                //這樣比對一定漏洞很多.待強者修復
                string strSQL = string.Format(@" SELECT  VW.MZ_OCCC_CH, VW.MZ_SRANK_CH ,VW.MZ_SLVC ,VW.MZ_SLVC_CH + ',' + VW.MZ_SPT SLVC_SPT ,BB.*   FROM
                                                 (

                                                SELECT * FROM (
                                                SELECT SN ,	SALARY_TYPE   SALARY_TYPE1,	IDCARD IDCARD1 , IN_ACCOUNT_DATE IN_ACCOUNT_DATE1 ,ACCOUNT_NAME ACCOUNT_NAME1,
                                                ACCOUNT_NO ACCOUNT_NO1,PAY PAY1,BANKID BANKID1,MEMO MEMO1, BANK_BACK_MEMO BANK_BACK_MEMO1,PAY_AD PAY_AD1 , CASEID CASEID1
                                                FROM dbo.B_BANK_BACK_TXT WHERE PAY_AD = '{6}' AND  SALARY_TYPE='{2}' AND  IN_ACCOUNT_DATE='{3}'  {4}
                                                ) A1 
                                                FULL OUTER JOIN  
                                                (   SELECT  * FROM  {0} {1}  ) B1  
                                                ON  A1.IDCARD1=B1.IDCARD {5}
                                                WHERE (nvl(A1.PAY1,0) != nvl( B1.PAY,0)  OR A1.ACCOUNT_NO1 !=B1.STOCKPILE_BANKID   )   )
                                                BB

                                                LEFT JOIN VW_A_DLBASE_S2 VW ON  IDCARD =VW.MZ_ID  ORDER BY VW.MZ_UNIT,VW.MZ_OCCC ", TABLE_NAME, WHERE, TYPE, IN_ACCOUNT_DATE, TXT_WHERE, SOLE_exception, strAD);


                if (TYPE == "SOLE" || TYPE == "REPAIR")
                {

                    if (BANKID == "700")//因為中華郵政不會照著我們給的順序回來
                    {
                        strSQL = string.Format(@" SELECT  VW.MZ_OCCC_CH, VW.MZ_SRANK_CH ,VW.MZ_SLVC ,VW.MZ_SLVC_CH + ',' + VW.MZ_SPT SLVC_SPT ,BB.*   FROM
                                                 (

                                                SELECT * FROM (
                                                SELECT SN ,	SALARY_TYPE   SALARY_TYPE1,	IDCARD IDCARD1 , IN_ACCOUNT_DATE IN_ACCOUNT_DATE1 ,ACCOUNT_NAME ACCOUNT_NAME1,
                                                ACCOUNT_NO ACCOUNT_NO1,PAY PAY1,BANKID BANKID1,MEMO MEMO1, BANK_BACK_MEMO BANK_BACK_MEMO1,PAY_AD PAY_AD1 , CASEID CASEID1 ,IMPORT_SN
                                                FROM dbo.B_BANK_BACK_TXT WHERE PAY_AD = '{6}' AND  SALARY_TYPE='{2}' AND  IN_ACCOUNT_DATE='{3}' {4}
                                                ) A1 
                                                FULL OUTER JOIN  
                                                (   SELECT  * FROM  {0} {1}  ) B1  
                                                ON  (A1.IDCARD1 + A1.ACCOUNT_NO1 + (nvl(A1.PAY1,0) )  =  (B1.IDCARD  + B1.STOCKPILE_BANKID + nvl( B1.PAY,0)) ) {5}
                                                WHERE (nvl(A1.PAY1,0) != nvl( B1.PAY,0)  OR A1.ACCOUNT_NO1 !=B1.STOCKPILE_BANKID   )   )
                                                BB

                                                LEFT JOIN VW_A_DLBASE_S2 VW ON  IDCARD =VW.MZ_ID  ", TABLE_NAME, WHERE, TYPE, IN_ACCOUNT_DATE, TXT_WHERE, SOLE_exception, strAD);
                    }
                    else
                    {
                        strSQL = string.Format(@" SELECT  VW.MZ_OCCC_CH, VW.MZ_SRANK_CH ,VW.MZ_SLVC ,VW.MZ_SLVC_CH + ',' + VW.MZ_SPT SLVC_SPT ,BB.*   FROM
                                                 (

                                                SELECT * FROM (
                                                SELECT SN ,	SALARY_TYPE   SALARY_TYPE1,	IDCARD IDCARD1 , IN_ACCOUNT_DATE IN_ACCOUNT_DATE1 ,ACCOUNT_NAME ACCOUNT_NAME1,
                                                ACCOUNT_NO ACCOUNT_NO1,PAY PAY1,BANKID BANKID1,MEMO MEMO1, BANK_BACK_MEMO BANK_BACK_MEMO1,PAY_AD PAY_AD1 , CASEID CASEID1 ,IMPORT_SN
                                                FROM dbo.B_BANK_BACK_TXT WHERE PAY_AD = '{6}' AND  SALARY_TYPE='{2}' AND  IN_ACCOUNT_DATE='{3}' {4}
                                                ) A1 
                                                FULL OUTER JOIN  
                                                (   SELECT  * FROM  {0} {1}  ) B1  
                                                ON  (A1.IDCARD1  + A1.IMPORT_SN )  =  (B1.IDCARD  + B1.TO_BANK_SN) {5}
                                                WHERE (nvl(A1.PAY1,0) != nvl( B1.PAY,0)  OR A1.ACCOUNT_NO1 !=B1.STOCKPILE_BANKID   )   )
                                                BB

                                                LEFT JOIN VW_A_DLBASE_S2 VW ON  IDCARD =VW.MZ_ID  ", TABLE_NAME, WHERE, TYPE, IN_ACCOUNT_DATE, TXT_WHERE, SOLE_exception, strAD);

                    }
                }
                //OR A1.ACCOUNT_NAME1 !=B1.NAME

                DataTable result = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


                Salary.movetoFirst(ref result, 0, "副分局長室");
                Salary.movetoFirst(ref result, 0, "分局長室");
                Salary.movetoLast(ref result, 0, "工友");
                Salary.movetoLast(ref result, 0, "技工");
                Salary.movetoLast(ref result, 0, "駕駛");

                return result;

            }

        }

        #region 2.2 每月薪資作業

        /// <summary>
        /// 2.2.3 每月薪資應發總表 
        /// </summary>
        public class monthpay_list
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');

                //TODO 找所需欄位
                string strSQL = string.Format("select * from VW_MONTHPAY_LIST_POLNO WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                Salary.movetoFirst(ref dt, 3, "副分局長室");
                Salary.movetoFirst(ref dt, 3, "分局長室");
                Salary.movetoLast(ref dt, 3, "工友");

                ////找出第一組人員(警職)
                //string strSQL = string.Format("select * from VW_MONTHPAY_LIST_POLNO_G WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                //DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                //Salary.movetoFirst(ref dt, 3, "副分局長室");
                //Salary.movetoFirst(ref dt, 3, "分局長室");
                //Salary.movetoLast(ref dt, 3, "工友");
                //dt = AlterDataTable(dt,"G");

                ////找出第二組人員(約聘&志工)
                //        strSQL = string.Format("select * from VW_MONTHPAY_LIST_POLNO_BP WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                //DataTable dt_other = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                //Salary.movetoFirst(ref dt_other, 3, "副分局長室");
                //Salary.movetoFirst(ref dt_other, 3, "分局長室");
                //Salary.movetoLast(ref dt_other, 3, "工友");
                //dt_other = AlterDataTable(dt_other, "O");

                //dt.Merge(dt_other);



                dt = Salary.addPageNumber(dt, 22, "PAY_AD");
                return dt;
               
            }

            public static DataTable doSearch2(string strAYEAR, string strMONTH, string PAY_AD)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');

                //找出第一組人員(警職)
                string strSQL = string.Format("select * from VW_MONTHPAY_LIST_POLNO_G WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");

                //20210119 - 問題：第3頁會是空白到第4頁才又有資料。　處理：註解即可　200行至218行
                if (dt.Rows.Count > 0)
                {
                    DataRow newRow = dt.NewRow();

                    //dt.Rows.Add(newRow);


                    //if (dt.Rows.Count % 22 != 0)
                    //{

                    //    int i = dt.Rows.Count % 22;
                    //    for (int j = i; j < 22; j++)
                    //    {
                    //        dt.ImportRow(newRow);
                    //    }

                    //}

                    int tmp = dt.Rows.Count % 22;

                    if(tmp > 0)
                    {
                        dt.Rows.Add(newRow);
                        int tmpadd = 22 - tmp;

                        for (int j = 1; j < tmpadd; j++)
                        {
                            dt.ImportRow(newRow);
                        }

                    }

                }
                Salary.movetoFirst(ref dt, 3, "副分局長室");
                Salary.movetoFirst(ref dt, 3, "分局長室");
                Salary.movetoLast(ref dt, 3, "工友");
                dt = Common_B_RPT.AlterDataTable(dt, "G");

                //找出第二組人員(約聘&志工)
                strSQL = string.Format("select * from VW_MONTHPAY_LIST_POLNO_BP WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                DataTable dt_other = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                Salary.movetoFirst(ref dt_other, 3, "副分局長室");
                Salary.movetoFirst(ref dt_other, 3, "分局長室");
                Salary.movetoLast(ref dt_other, 3, "工友");
                dt_other = Common_B_RPT.AlterDataTable(dt_other, "O");

                dt.Merge(dt_other);



                dt = Salary.addPageNumber(dt, 22, "PAY_AD");
                return dt;

            }
        }

         

        /// <summary>
        /// 2.2.4 每月薪資應發個人明細表
        /// </summary>
        public class monthpay_detail
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD , string UNIT)
            {
                string strSQL = "";

                string date = strAYEAR + strMONTH.PadLeft(2, '0');

                if (string.IsNullOrEmpty(UNIT))
                {
                    //全部單位                                     
                    strSQL = string.Format(@"SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_MONTHPAY_DETAIL_POLNO WHERE AMONTH='{0}' AND PAY_AD='{1}') VW  ORDER BY VW.MZ_UNIT, VW.MZ_POLNO,NUM", date, PAY_AD);
                }
                else
                {
                    //特定單位                                        
                    strSQL = string.Format(@"SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_MONTHPAY_DETAIL_POLNO WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}') VW  ORDER BY VW.MZ_UNIT, VW.MZ_POLNO,NUM", date, PAY_AD, UNIT);
                }

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                dt = Salary.addPageNumber(dt, 20, "PAY_UNIT");
                return dt;
            }
        }




        /// <summary>
        /// 2.2.5 每月薪資應扣總表 
        /// </summary>
        public class monthpaytakeoff_list
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                //TODO 找所需欄位
                string strSQL = string.Format("SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_POLNO WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");

                Salary.movetoFirst(ref dt, 3, "副分局長室");
                Salary.movetoFirst(ref dt, 3, "分局長室");
                Salary.movetoLast(ref dt, 3, "工友");

                ///
                ////找出第一組人員(警職)
                //string strSQL = string.Format("SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_G WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                //DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                //Salary.movetoFirst(ref dt, 3, "副分局長室");
                //Salary.movetoFirst(ref dt, 3, "分局長室");
                //Salary.movetoLast(ref dt, 3, "工友");
                //dt = AlterDataTable(dt, "G");

                ////找出第二組人員(約聘&志工)
                //strSQL = string.Format("SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_BP WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                //DataTable dt_other = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                //Salary.movetoFirst(ref dt_other, 3, "副分局長室");
                //Salary.movetoFirst(ref dt_other, 3, "分局長室");
                //Salary.movetoLast(ref dt_other, 3, "工友");
                //dt_other = AlterDataTable(dt_other, "O");

                //dt.Merge(dt_other);

                dt = Salary.addPageNumber(dt, 22, "PAY_AD");
                return dt;

            }

            public static DataTable doSearch2(string strAYEAR, string strMONTH, string PAY_AD)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                //TODO 找所需欄位

                //找出第一組人員(警職)
                string strSQL = string.Format("SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_G WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");

                //20210119 - 問題：第3頁會是空白到第4頁才又有資料。　處理：註解即可　326行至347行
                if (dt.Rows.Count > 0)
                {

                    DataRow newRow = dt.NewRow();

                    //dt.Rows.Add(newRow);
                    
                    //if (dt.Rows.Count % 22 != 0)
                    //{

                    //    int i = dt.Rows.Count % 22;
                    //    for (int j = i; j < 22; j++)
                    //    {
                    //        dt.ImportRow(newRow);
                    //    }
                    //}

                    int tmp = dt.Rows.Count % 22;

                    if (tmp > 0)
                    {
                        dt.Rows.Add(newRow);
                        int tmpadd = 22 - tmp;

                        for (int j = 1; j < tmpadd; j++)
                        {
                            dt.ImportRow(newRow);
                        }

                    }
                }



                Salary.movetoFirst(ref dt, 3, "副分局長室");
                Salary.movetoFirst(ref dt, 3, "分局長室");
                Salary.movetoLast(ref dt, 3, "工友");
                dt = Common_B_RPT.AlterDataTable(dt, "G");

                //找出第二組人員(約聘&志工)
                strSQL = string.Format("SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_BP WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ORDER BY UNITCODE", PAY_AD, strDate);
                DataTable dt_other = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                Salary.movetoFirst(ref dt_other, 3, "副分局長室");
                Salary.movetoFirst(ref dt_other, 3, "分局長室");
                Salary.movetoLast(ref dt_other, 3, "工友");
                dt_other = Common_B_RPT.AlterDataTable(dt_other, "O");

                dt.Merge(dt_other);

                dt = Salary.addPageNumber(dt, 22, "PAY_AD");
                return dt;

            }
        }



          /// <summary>
        /// 2.2.6 每月薪資應扣個人明細表
        /// </summary>
        public class monthpaytakeoff_detail
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD, string UNIT)
            {
                string date = strAYEAR + strMONTH.PadLeft(2, '0');
                string strSQL="";
                if (string.IsNullOrEmpty(UNIT))
                {
                   
                    strSQL = string.Format(@"SELECT ROWNUM NUM,  PAY_AD, AMONTH, MZ_UNIT, PAY_UNIT, IDCARD, MZ_SRANK,  MZ_SPT, MZ_OCCC, MZ_NAME, ACCOUNT, INSURANCEPAY, HEALTHPAY, HEALTHPAY1, CONCUR3PAY, MONTHPAY_TAX, MONTHPAY, TAX,
EXTRA01, EXTRA02, EXTRA03, EXTRA04, EXTRA05, EXTRA06, EXTRA07, EXTRA08, EXTRA09, OTHERMINUS, ADD_SUM, DES_SUM, TOTAL, MZ_POLNO, NOTE 
FROM VW_MONTHPAY_TAKEOFF_DETAIL_POL VW  WHERE AMONTH='{0}' AND PAY_AD='{1}'   ORDER BY VW.MZ_UNIT, VW.MZ_POLNO,NUM", date, PAY_AD);

                }
                else
                {
                                      
                    strSQL = string.Format(@"SELECT ROWNUM NUM,   PAY_AD, AMONTH, MZ_UNIT, PAY_UNIT, IDCARD, MZ_SRANK,  MZ_SPT, MZ_OCCC, MZ_NAME, ACCOUNT, INSURANCEPAY, HEALTHPAY, HEALTHPAY1, CONCUR3PAY, MONTHPAY_TAX, MONTHPAY, TAX,
EXTRA01, EXTRA02, EXTRA03, EXTRA04, EXTRA05, EXTRA06, EXTRA07, EXTRA08, EXTRA09, OTHERMINUS, ADD_SUM, DES_SUM, TOTAL, MZ_POLNO, NOTE 
FROM VW_MONTHPAY_TAKEOFF_DETAIL_POL VW WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}'   ORDER BY VW.MZ_UNIT, VW.MZ_POLNO,NUM", date, PAY_AD, UNIT);
                }

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                dt = Salary.addPageNumber(dt, 20, "PAY_UNIT");
                return dt;
            }

        }



        #endregion 2.2 每月薪資作業

        #region 2.3 補發薪資作業


         /// <summary>
        /// 2.3.2 補發薪資應發總表 
        /// </summary>
        public class repairpay_list
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD, string BATCH_NUMBER)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                string strSQL = string.Format("SELECT * FROM VW_REPAIRPAY_LIST WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY UNITCODE", PAY_AD, strDate, BATCH_NUMBER);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");

                if (PAY_AD != "382130000C")
                {
                    Salary.movetoFirst(ref dt, 3, "副分局長室");
                    Salary.movetoFirst(ref dt, 3, "分局長室");
                    Salary.movetoLast(ref dt, 3, "工友");
                }



                dt = Salary.addPageNumber(dt, 22, "PAY_AD");

                return dt;
            }

            public static DataTable doSearchClassify(string strAYEAR, string strMONTH, string PAY_AD, string BATCH_NUMBER)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                //找出第一組人員(警職)
                string strSQL = string.Format("SELECT * FROM VW_REPAIRPAY_LIST_G WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY UNITCODE", PAY_AD, strDate, BATCH_NUMBER);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                if (dt.Rows.Count > 0)
                {
                    DataRow newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                    if (dt.Rows.Count % 22 != 0)
                    {
                        int i = dt.Rows.Count % 22;
                        for (int j = i; j < 22; j++)
                        {
                            dt.ImportRow(newRow);
                        }
                    }
                }
                if (PAY_AD != "382130000C")
                {
                    Salary.movetoFirst(ref dt, 3, "副分局長室");
                    Salary.movetoFirst(ref dt, 3, "分局長室");
                    Salary.movetoLast(ref dt, 3, "工友");
                }
                dt = Common_B_RPT.AlterDataTable(dt, "G");

                //找出第二組人員(約聘&志工)
                strSQL = string.Format("SELECT * FROM VW_REPAIRPAY_LIST_BP WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY UNITCODE", PAY_AD, strDate, BATCH_NUMBER);
                DataTable dt_other = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                if (PAY_AD != "382130000C")
                {
                    Salary.movetoFirst(ref dt_other, 3, "副分局長室");
                    Salary.movetoFirst(ref dt_other, 3, "分局長室");
                    Salary.movetoLast(ref dt_other, 3, "工友");
                }
                dt_other = Common_B_RPT.AlterDataTable(dt_other, "O");


                //合併兩個DataTable
                dt.Merge(dt_other);

                //增加頁次
                dt = Salary.addPageNumber(dt, 22, "PAY_AD");
                return dt;
            }
        }

          /// <summary>
        /// 2.3.3 補發薪資明細表(應發)
        /// </summary>
        public class repairpay_detail
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD, string  UNIT, string BATCH_NUMBER)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                string strSQL;

                if (string.IsNullOrEmpty(UNIT))
                    strSQL = string.Format("SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_REPAIRPAY_DETAIL WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}') VW  ORDER BY VW.MZ_UNIT, VW.MZ_POLNO,NUM", PAY_AD, strDate, BATCH_NUMBER);
                else
                    strSQL = string.Format("SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_REPAIRPAY_DETAIL WHERE PAY_AD = '{0}' AND MZ_UNIT = '{1}' AND AMONTH = '{2}' AND BATCH_NUMBER='{3}') VW  ORDER BY VW.MZ_UNIT, VW.MZ_POLNO,NUM", PAY_AD, UNIT, strDate, BATCH_NUMBER);
                DataTable dt = Salary.addPageNumber(o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW"), 20, "PAY_UNIT");

                return dt;
            }

        }



          /// <summary>
        /// 2.3.4 補發薪資應扣總表 
        /// </summary>
        public class repairtakeoff_list
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD, string BATCH_NUMBER)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                string strSQL = string.Format("SELECT * FROM VW_REPAIRPAY_TAKEOFF_LIST WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY UNITCODE", PAY_AD, strDate, BATCH_NUMBER);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                if (PAY_AD != "382130000C")
                {
                    Salary.movetoFirst(ref dt, 3, "副分局長室");
                    Salary.movetoFirst(ref dt, 3, "分局長室");
                    Salary.movetoLast(ref dt, 3, "工友");
                }
                dt = Salary.addPageNumber(dt, 22, "PAY_AD");

                return dt;

            }

            public static DataTable doSearchClassify(string strAYEAR, string strMONTH, string PAY_AD, string BATCH_NUMBER)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');
                //找出第一組人員(警職)
                string strSQL = string.Format("SELECT * FROM VW_REPAIRPAY_TAKEOFF_LIST_G WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY UNITCODE", PAY_AD, strDate, BATCH_NUMBER);
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                if (dt.Rows.Count > 0)
                {
                    DataRow newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                    if (dt.Rows.Count % 22 != 0)
                    {
                        int i = dt.Rows.Count % 22;
                        for (int j = i; j < 22; j++)
                        {
                            dt.ImportRow(newRow);
                        }
                    }
                }
                if (PAY_AD != "382130000C")
                {
                    Salary.movetoFirst(ref dt, 3, "副分局長室");
                    Salary.movetoFirst(ref dt, 3, "分局長室");
                    Salary.movetoLast(ref dt, 3, "工友");
                }
                dt = Common_B_RPT.AlterDataTable(dt, "G");

                //找出第二組人員(約聘&志工)
                strSQL = string.Format("SELECT * FROM VW_REPAIRPAY_TAKEOFF_LIST_BP WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY UNITCODE", PAY_AD, strDate, BATCH_NUMBER);
                DataTable dt_other = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");
                if (PAY_AD != "382130000C")
                {
                    Salary.movetoFirst(ref dt_other, 3, "副分局長室");
                    Salary.movetoFirst(ref dt_other, 3, "分局長室");
                    Salary.movetoLast(ref dt_other, 3, "工友");
                }
                dt_other = Common_B_RPT.AlterDataTable(dt_other, "O");


                //合併兩個DataTable
                dt.Merge(dt_other);

                //增加頁次
                dt = Salary.addPageNumber(dt, 22, "PAY_AD");
                return dt;
            }

        }


          /// <summary>
        /// 2.3.5 補發薪資應扣明細表 
        /// </summary>
        public class repairpaytakeoff_detail
        {
            public static DataTable doSearch(string strAYEAR, string strMONTH, string PAY_AD, string UNIT, string BATCH_NUMBER)
            {
                string strDate = strAYEAR + strMONTH.PadLeft(2, '0');

          

                string strSQL;

                if (string.IsNullOrEmpty(UNIT))
                    strSQL = string.Format("SELECT ROWNUM NUM, VW_REPAIRPAY_TAKEOFF_DETAIL.* FROM VW_REPAIRPAY_TAKEOFF_DETAIL WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BATCH_NUMBER='{2}' ORDER BY MZ_UNIT, MZ_POLNO,NUM", PAY_AD, strDate, BATCH_NUMBER);
                else
                    strSQL = string.Format("SELECT ROWNUM NUM, VW_REPAIRPAY_TAKEOFF_DETAIL.* FROM VW_REPAIRPAY_TAKEOFF_DETAIL WHERE PAY_AD = '{0}' AND MZ_UNIT = '{1}' AND AMONTH = '{2}' AND BATCH_NUMBER='{3}' ORDER BY MZ_UNIT, MZ_POLNO,NUM", PAY_AD, UNIT, strDate, BATCH_NUMBER);

                DataTable dt = Salary.addPageNumber(o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW"), 20, "PAY_UNIT");

                return dt;
            }

        }


         #endregion 2.3 補發薪資作業


        /// <summary>
        /// 2.8.7 個人全年薪資明細表
        /// </summary>
        public class B_79
        {
            public static DataTable doSearch(string YEAR, string IDCARD)
            {
                List<SqlParameter> ops = new List<SqlParameter>();
                string sql = @"SELECT IDNO, dbo.SUBSTR(DA, 1, 3) YEAR, DA, AMOUNT, ITEM, MEMO FROM 
                            (
                            SELECT IDNO, MZ_OCCC CHIOCCC, NAME, AMONTH DA, TOTAL-TOTALSUB AMOUNT, N'每月薪資' ITEM, NOTE MEMO FROM VW_ALL_SALARY_DATA WHERE PAY_AD IS NOT NULL AND  dbo.SUBSTR(AMONTH,1,3)=@YEAR AND IDNO=@IDNO
                            UNION ALL 
                            SELECT IDNO, MZ_OCCC CHIOCCC, NAME, AMONTH DA, TOTAL-TOTALSUB AMOUNT, N'補發薪資' ITEM, NOTE MEMO FROM VW_ALL_REPAIR_DATA WHERE  PAY_AD IS NOT NULL AND dbo.SUBSTR(AMONTH,1,3)=@YEAR AND IDNO=@IDNO
                            UNION ALL 
                            SELECT IDNO, CHIOCCC, NAME, DA, PAY-TOTALSUB AMOUNT, NVL(ITEM, '') ITEM, NOTE MEMO FROM VW_ALL_SOLE_DATA WHERE  PAY_AD IS NOT NULL AND  dbo.SUBSTR(DA,1,3)=@YEAR AND IDNO=@IDNO
                            UNION ALL
                            SELECT IDNO, CHIOCCC, NAME, AYEAR DA, TOTAL-TOTALSUB AMOUNT, N'年終獎金', NOTE MEMO FROM VW_ALL_YEARPAY_DATA WHERE  PAY_AD IS NOT NULL AND dbo.TO_CHAR(AYEAR+1)=@YEAR AND IDNO=@IDNO AND LOCKDB='Y'
                            UNION ALL
                            SELECT IDNO, CHIOCCC, NAME, AYEAR DA, TOTAL-TOTALSUB AMOUNT, N'考績獎金', NOTE MEMO FROM VW_ALL_EFFECT_DATA WHERE  PAY_AD IS NOT NULL AND   dbo.TO_CHAR(AYEAR+1)=@YEAR AND IDNO=@IDNO AND LOCKDB='Y'
                            ) VW  ORDER BY DA";
                ops.Add(new SqlParameter("YEAR", YEAR));
                ops.Add(new SqlParameter("IDNO", IDCARD));


                return o_DBFactory.ABC_toTest.DataSelect(sql, ops);


            }


        }


        #region 2.10金融機構轉存
        /// <summary>
        /// 2.10.1金融機構轉存-機關團體戶存款單
        /// </summary>
        public class bank_list
        {
            public static DataTable doSearch(string PAYAD, string BANK, string type, string BNO, string caseid, string date)
            {
                string strPAY_AD = String.Empty;
                string strBANKID = String.Empty;
                string strSQL = String.Empty;
                string strDate = String.Empty;
                string strDateType = String.Empty;
                DataTable dtToRPT = new DataTable();

                strPAY_AD = PAYAD;
                strBANKID = BANK;
                strDate = date;

                dtToRPT.Clear();
                dtToRPT.Columns.Clear();
                dtToRPT.Columns.Add("PAY_UNIT", typeof(String));
                dtToRPT.Columns.Add("UNITACC", typeof(String));
                dtToRPT.Columns.Add("ACC_COUNT", typeof(Int16));
                dtToRPT.Columns.Add("LISTCOUNT", typeof(Int16));
                dtToRPT.Columns.Add("AMOUNT", typeof(Int32));

                switch (type)
                {
                    case "MONTH"://  case "B_MONTHPAY_MAIN":
                        strSQL = string.Format("select PAY_UNIT, UNITACC, ACC_COUNT, LISTCOUNT, AMOUNT FROM VW_SALARYGROUPLIST WHERE AMONTH = '{0}' AND bankid='{1}' AND PAY_AD='{2}' ORDER BY MZ_UNIT", strDate, strBANKID, strPAY_AD);

                        break;
                    case "REPAIR"://  case "B_REPAIRPAY":
                        strSQL = string.Format("select PAY_UNIT, UNITACC, ACC_COUNT, LISTCOUNT, AMOUNT FROM VW_REPAIRGROUPLIST WHERE AMONTH = '{0}' AND bankid='{1}' AND PAY_AD='{2}' {3} ORDER BY MZ_UNIT", strDate, strBANKID, strPAY_AD, BNO.Length == 0 ? null : "AND BATCH_NUMBER = '" + BNO + "'");
                        break;
                    case "YEAR"://  case "B_YEARPAY":
                        strSQL = string.Format("select PAY_UNIT, UNITACC, ACC_COUNT, LISTCOUNT, AMOUNT FROM VW_YEARGROUPLIST WHERE AYEAR = '{0}' AND bankid='{1}' AND PAY_AD='{2}' ORDER BY MZ_UNIT", strDate, strBANKID, strPAY_AD);
                        break;

                    case "EFFECT"://                    case "B_EFFECT":
                        strSQL = string.Format("select PAY_UNIT, UNITACC, ACC_COUNT, LISTCOUNT, AMOUNT FROM VW_EFFECTGROUPLIST WHERE AYEAR = '{0}' AND bankid='{1}' AND PAY_AD='{2}' ORDER BY MZ_UNIT", strDate, strBANKID, strPAY_AD);
                        break;

                    case   "SOLE" ://case "B_SOLE":
                        //strSQL = string.Format("select PAY_UNIT, UNITACC, ACC_COUNT, LISTCOUNT, AMOUNT FROM VW_SOLEGROUPLIST WHERE DA = '{0}' AND bankid='{1}' AND PAY_AD='{2}' {3} ORDER BY MZ_UNIT", strDate, strBANKID, strPAY_AD, caseid.Length == 0 ? null : "AND CASEID = '" + caseid + "'");
                        strSQL = string.Format("select PAY_UNIT, UNITACC, ACC_COUNT, LISTCOUNT, AMOUNT FROM VW_SOLEGROUPLIST WHERE DA = '{0}' AND PAY_AD='{1}' {2} ORDER BY MZ_UNIT", strDate, strPAY_AD, caseid.Length == 0 ? null : "AND CASEID = '" + caseid + "'");
                        break;
                    default:
                        strSQL = "";
                        break;
                }

                dtToRPT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GroupAmount");

                return dtToRPT;


            }

            public static string doTitle(string type)
            {
                string Title = "";
                switch (type)
                {
                    case "MONTH"://case "B_MONTHPAY_MAIN":
                        Title = "每月薪資";
                        break;
                    case "REPAIR"://case "B_REPAIRPAY":
                        Title = "補發薪資";
                        break;
                    case "YEAR"://case "B_YEARPAY":
                        Title = "年終獎金";
                        break;
                    case "EFFECT"://      case "B_EFFECT":
                        Title = "考績獎金";
                        break;
                    case "SOLE"://case "B_SOLE":
                        Title = "單一發放";
                        break;
                }

                return Title;
            
            }

            public static string doAGENCYNAME(string PAYAD, string BANK)
            {
                SalaryPublic sp = new SalaryPublic();

                string AGENCYNAME = sp.getBankName(PAYAD, BANK);
                

                return AGENCYNAME;
            }

            public static string doAGENCYID(string PAYAD, string BANK)
            {
                SalaryPublic sp = new SalaryPublic();

                string AGENCYID = sp.getFianceno(PAYAD, BANK);


                return AGENCYID;
            }

            public static string GetTransDate(string TRANSDATE)
            {
                return TRANSDATE.Substring(0, 3) + "年" + TRANSDATE.Substring(3, 2) + "月" + TRANSDATE.Substring(5, 2) + "日";
            }

        }

        /// <summary>
        /// 2.10.1金融機構轉存-存款帳戶移送單
        /// </summary>
        public class AmountDetail
        {
            public static DataTable doSearch(string PAYAD, string BANK, string type, string BNO, string caseid, string date,string order,string rpt_type)
            {
                string strPAY_AD = String.Empty;
                string strBANKID = String.Empty;
                string strSQL = String.Empty;
                //string strTmpUnit = String.Empty;
                //string strUNITACC = String.Empty;
                string strDate = String.Empty;
                //string strDateType = String.Empty;
                DataTable dtToRPT = new DataTable();

                strPAY_AD = PAYAD;
                strBANKID = BANK;
                strDate = date;

                dtToRPT.Clear();
                dtToRPT.Columns.Clear();
                dtToRPT.Columns.Add("PAY_UNIT", typeof(String));
                dtToRPT.Columns.Add("ACCOUNT", typeof(String));
                dtToRPT.Columns.Add("NAME", typeof(String));
                dtToRPT.Columns.Add("IDNO", typeof(String));
                dtToRPT.Columns.Add("AMOUNT", typeof(Int32));

                switch (type)
                {
                    case "MONTH"://case "B_MONTHPAY_MAIN":

                        strSQL = string.Format("SELECT MZ_UNIT, PAY_UNIT, ACCOUNT, NAME, IDNO, AMOUNT " +
                                            "FROM VW_SALARYMEMBERLIST WHERE AMONTH = '{0}' AND BANKID ='{1}' AND PAY_AD='{2}' " +
                                            "{3}", strDate, strBANKID, strPAY_AD, order == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY MM_SNID ASC");
                        break;
                    case "REPAIR"://case "B_REPAIRPAY":
                        strSQL = string.Format("SELECT MZ_UNIT, PAY_UNIT, ACCOUNT, NAME, IDNO, AMOUNT " +
                                            "FROM VW_REPAIRMEMBERLIST WHERE AMONTH = '{0}' AND BANKID ='{1}' AND PAY_AD='{2}' {3}" +
                                            "{4}", strDate, strBANKID, strPAY_AD, BNO.Length == 0 ? null : "AND BATCH_NUMBER = '" + BNO + "'", order == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY R_SNID ASC");
                        break;
                    case "YEAR"://case "B_YEARPAY":
                        strSQL = string.Format("SELECT MZ_UNIT, PAY_UNIT, ACCOUNT, NAME, IDNO, AMOUNT " +
                                            "FROM VW_YEARMEMBERLIST WHERE AYEAR = '{0}' AND BANKID ='{1}' AND PAY_AD='{2}' " +
                                            "{3}", strDate, strBANKID, strPAY_AD, order == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY Y_SNID ASC");
                        break;
                    case "EFFECT"://      case "B_EFFECT":
                        strSQL = string.Format("SELECT MZ_UNIT, PAY_UNIT, ACCOUNT, NAME, IDNO, AMOUNT " +
                                            "FROM VW_EFFECTMEMBERLIST WHERE AYEAR = '{0}' AND BANKID ='{1}' AND PAY_AD='{2}' " +
                                            "{3}", strDate, strBANKID, strPAY_AD, order == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY E_SNID ASC");
                        break;
                    case "SOLE"://case "B_SOLE":                        
                        strSQL = string.Format(@"SELECT *                            
                                               FROM VW_SOLEMEMBERLIST WHERE DA = '{0}'  AND PAY_AD='{1}' AND BANKID ='{4}' {2} {3}"
                                                , strDate, strPAY_AD, caseid.Length == 0 ? null : "AND CASEID = '" + caseid + "'"
                                                , order =="1"?"ORDER BY MZ_POLNO": (rpt_type=="1" ? "ORDER BY  S_SNID ASC" : "ORDER BY MZ_UNIT, S_SNID ASC" )
                                                , strBANKID);
                        //2013/09/12
                        break;
                }

                dtToRPT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GroupAmount");

                return dtToRPT;


            }

            public static string doTitle(string type)
            {
                string Title = "";
                switch (type)
                {
                    case "MONTH"://case "B_MONTHPAY_MAIN":
                        Title = "每月薪資";
                        break;
                    case "REPAIR"://case "B_REPAIRPAY":
                        Title = "補發薪資";
                        break;
                    case "YEAR"://case "B_YEARPAY":
                        Title = "年終獎金";
                        break;
                    case "EFFECT"://      case "B_EFFECT":
                        Title = "考績獎金";
                        break;
                    case "SOLE"://case "B_SOLE":
                        Title = "單一發放";
                        break;
                }

                return Title;

            }

            public static string doAGENCYNAME(string PAYAD, string BANK)
            {
                SalaryPublic sp = new SalaryPublic();

                string AGENCYNAME = sp.getBankName(PAYAD, BANK);


                return AGENCYNAME;
            }

            public static string doAGENCYID(string PAYAD, string BANK)
            {
                SalaryPublic sp = new SalaryPublic();

                string AGENCYID = sp.getFianceno(PAYAD, BANK);


                return AGENCYID;
            }

            public static string GetTransDate(string TRANSDATE)
            {
                return TRANSDATE.Substring(0, 3) + "年" + TRANSDATE.Substring(3, 2) + "月" + TRANSDATE.Substring(5, 2) + "日";
            }

        }


        #endregion 2.10金融機構轉存


        #region 2.11.薪資異動差異報表

        /// <summary>
        /// 加項(應發)差異明細表
        /// </summary>
        public class monthpay_detail_different
        {
            public static DataTable doSearch(string date1, string date2, string strAD)
            {
               string strSQL = String.Empty;
                
                

                strSQL = string.Format(@"SELECT ROWNUM NUM,C.*  FROM (

SELECT * FROM (
SELECT *  FROM VW_MONTHPAY_DETAIL_POLNO WHERE AMONTH='{0}' AND PAY_AD='{2}' ) A
FULL OUTER JOIN  
(SELECT * FROM VW_MONTHPAY_DETAIL_POLNO WHERE AMONTH='{1}' AND PAY_AD='{2}') B 

ON A.IDCARD=B.IDCARD

WHERE nvl(A.TOTAL,0) != nvl( B.TOTAL,0)   ORDER BY A.MZ_UNIT, A.MZ_POLNO ,B.MZ_UNIT, B.MZ_POLNO) C", date1, date2, strAD);

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                return dt;


            }
        }

        /// <summary>
        /// 減項(應扣)差異明細表
        /// </summary>
        public class monthpaytakeoff_detail_different
        {
            public static DataTable doSearch(string date1, string date2, string strAD, string type)
            {
                string strSQL = String.Empty;
                string part = "1=1";

                switch (type)
                {
                    case "1":
                        part = "nvl(A.TOTAL,0) != nvl(B.TOTAL,0)";
                        break;
                    case "2":
                        part = "nvl(A.HEALTHPAY,0) != nvl(B.HEALTHPAY,0)";
                        break;
                    case "3":
                        part = "nvl(A.CONCUR3PAY,0) != nvl(B.CONCUR3PAY,0)";
                        break;



                }

                strSQL = string.Format(@"SELECT ROWNUM NUM,C.*  FROM (

SELECT * FROM (
SELECT *  FROM VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{2}' ) A
FULL OUTER JOIN  
(SELECT * FROM VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{1}' AND PAY_AD='{2}') B 

ON A.IDCARD=B.IDCARD

WHERE {3}   ORDER BY A.MZ_UNIT, A.MZ_POLNO ,B.MZ_UNIT, B.MZ_POLNO) C", date1, date2, strAD, part);

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                return dt;


            }
        }

        /// <summary>
        /// 加項(應發)差異總表
        /// </summary>
        public class monthpay_list_different
        {
            public static DataTable doSearch(string date1, string date2, string strAD)
            {
                

                string strSQL = string.Format(@"SELECT ROWNUM NUM,C.*  FROM (SELECT * FROM (SELECT * FROM VW_MONTHPAY_LIST_POLNO WHERE AMONTH='{1}' AND PAY_AD='{0}' ) A
LEFT JOIN  (SELECT * FROM VW_MONTHPAY_LIST_POLNO WHERE AMONTH='{2}' AND PAY_AD='{0}') B ON A.UNITCODE=B.UNITCODE
WHERE A.TOTAL != B.TOTAL   ORDER BY A.UNITCODE) C", strAD, date1, date2);


                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");

                Salary.movetoFirst(ref dt, 3, "副分局長室");
                Salary.movetoFirst(ref dt, 3, "分局長室");
                Salary.movetoLast(ref dt, 3, "工友");

                return dt;


            }
        }

        /// <summary>
        /// 減項(應扣)差異總表
        /// </summary>
        public class monthpaytakeoff_list_different
        {
            public static DataTable doSearch(string date1, string date2, string strAD , string type)
            {

                string part = " 1=1";

                switch (type)
                {
                    case "1" :
                        part = "A.TOTAL != B.TOTAL";
                        break;
                    case "2":
                        part = "A.HEALTHPAY != B.HEALTHPAY";
                        break;
                    case "3":
                        part = "A.CONCUR3PAY != B.CONCUR3PAY";
                        break;

                
                
                }


                string strSQL = string.Format(@"SELECT ROWNUM NUM,C.*  FROM (SELECT * FROM (SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_POLNO WHERE AMONTH='{1}' AND PAY_AD='{0}' ) A
LEFT JOIN  (SELECT * FROM VW_MONTHPAY_TAKEOFF_LIST_POLNO WHERE AMONTH='{2}' AND PAY_AD='{0}') B ON A.UNITCODE=B.UNITCODE
WHERE {3}   ORDER BY A.UNITCODE) C", strAD, date1, date2, part);


                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "vw");

                Salary.movetoFirst(ref dt, 3, "副分局長室");
                Salary.movetoFirst(ref dt, 3, "分局長室");
                Salary.movetoLast(ref dt, 3, "工友");

                return dt;


            }
        }

        #endregion 2.11.薪資異動差異報表



        private class Common_B_RPT
        {
            /// <summary>
            /// 根據組別在單位前加上補充文字
            /// </summary>
            /// <param name="dt">DataTable Source</param>
            /// <param name="type">G:(警職) O:(ㄧ般)</param>
            /// <returns></returns>
            public static DataTable AlterDataTable(DataTable dt, string type)
            {
                if (type == "G")
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (!String.IsNullOrEmpty(dt.Rows[i]["UNIT"].ToString()))
                        {
                            dt.Rows[i]["UNIT"] = dt.Rows[i]["UNIT"].ToString() + "(警職)";
                        }
                        else
                        {
                            //分頁次群組判斷使用
                            dt.Rows[i]["PAY_AD"] = dt.Rows[0]["PAY_AD"];
                        }
                    }

                }
                else if (type == "O")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (dt.Rows[i]["UNIT"].ToString() != "駕駛、工友")
                        {
                            dt.Rows[i]["UNIT"] = dt.Rows[i]["UNIT"].ToString() + "(ㄧ般)";
                        }
                    }
                }

                return dt;
            }
        }
    }
}
