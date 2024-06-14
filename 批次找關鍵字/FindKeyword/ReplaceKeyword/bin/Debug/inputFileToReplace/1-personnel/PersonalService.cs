using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public class PersonalService
    {
        /// <summary>
        /// 獎懲資料重複檢查
        /// </summary>
        /// <param name="model"></param>
        /// <returns>重複:false 非重複:true</returns>
        public bool DuplicateCheckFor_APRKB(A_PRKB_Model model)
        {
            try
            {
                string strSQL = string.Format(@"SELECT COUNT(*) c FROM A_PRKB WHERE MZ_ID=@MZ_ID AND MZ_PRCT=@MZ_PRCT ");
                List<SqlParameter> oracleParameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", model.MZ_ID.SafeTrim()),
                    new SqlParameter("MZ_PRCT", model.MZ_PRCT.SafeTrim())
                };

                if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, oracleParameters) != "0")
                {
                    //已有資料
                    return false;
                }
                else
                {
                    //無資料
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 獎懲類別與獎懲結果檢查內容
        /// </summary>
        /// <param name="model"></param>
        /// <returns>回傳結果字串</returns>
        public string ConsistencyCheckFor_PRRST(A_PRKB_Model model)
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> oracleParameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_PRRST", model.MZ_PRRST.ToUpper())
                };

                if (model.MZ_PRK.ToUpper().Substring(0, 1) == "A")
                {
                    strSQL = string.Format(@"SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE=@MZ_PRRST 
                                            AND (dbo.SUBSTR(MZ_KCODE,1,1)='4' OR dbo.SUBSTR(MZ_KCODE,1,1)='7' OR dbo.SUBSTR(MZ_KCODE,1,1)='8' 
                                            OR dbo.SUBSTR(MZ_KCODE,1,1)='9') ");
                }
                else
                {
                    strSQL = string.Format(@"SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE=@MZ_PRRST 
                                            AND (dbo.SUBSTR(MZ_KCODE,1,1)='2' OR dbo.SUBSTR(MZ_KCODE,1,1)='5' OR dbo.SUBSTR(MZ_KCODE,1,1)='6' 
                                            OR dbo.SUBSTR(MZ_KCODE,1,1)='A' OR dbo.SUBSTR(MZ_KCODE,1,1)='B') ");
                }

                return o_DBFactory.ABC_toTest.Get_First_Field(strSQL, oracleParameters);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 獎懲發布謄稿重複檢核
        /// </summary>
        /// <param name="model"></param>
        /// <returns>重複:false 非重複:true</returns>
        public bool DuplicateCheckFor_APRK2(A_PRK2_Model model)
        {
            try
            {
                //string strSQL = string.Format(@"SELECT COUNT(*) c FROM A_PRK2 WHERE MZ_ID=@MZ_ID And MZ_PRK=@MZ_PRK And MZ_PRCT=@MZ_PRCT And MZ_DATE=@MZ_DATE ");

                //修改為僅檢查A_PRK2身分證、獎懲內容 20191001 by sky
                string strSQL = string.Format(@"SELECT COUNT(*) c FROM A_PRK2 WHERE MZ_ID=@MZ_ID And MZ_PRCT=@MZ_PRCT ");
                List<SqlParameter> oracleParameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", model.MZ_ID.SafeTrim()),
                    new SqlParameter("MZ_PRCT", model.MZ_PRCT.SafeTrim())
                };

                if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, oracleParameters) != "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable GetSalaryMedata(P_SalaryMetaData_Model model)
        {
            DataTable dt = new DataTable();
            try
            {
                string strSQL = string.Format(@"SELECT COUNT(*) c FROM A_PRKB WHERE MZ_ID=@MZ_ID AND MZ_PRCT=@MZ_PRCT ");

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT \n");
                sb.Append("A_DLBASE.MZ_ID 身分證號, \n");
                sb.Append("A_DLBASE.MZ_NAME 姓名, \n");
                sb.Append("A_DLBASE.MZ_SEX 性別, \n");
                sb.Append("A_DLBASE.MZ_BIR 出生日期, \n");
                sb.Append("A_DLBASE.MZ_PESN 人員區分, \n");
                sb.Append("A_DLBASE.MZ_AD \"編制(佔缺)機關\", \n");
                sb.Append("A_DLBASE.MZ_UNIT \"編制(佔缺)單位\", \n");
                sb.Append("A_DLBASE.MZ_EXAD 服務機關, \n");
                sb.Append("A_DLBASE.MZ_EXUNIT 服務單位, \n");
                sb.Append("A_DLBASE.PAY_AD 發薪機關, \n");
                sb.Append("A_DLBASE.MZ_OCCC 職稱, \n");
                sb.Append("A_DLBASE.MZ_SRANK 薪俸職等, \n");
                sb.Append("A_DLBASE.MZ_SLVC 俸階, \n");
                sb.Append("A_DLBASE.MZ_SPT 俸點, \n");
                sb.Append("A_DLBASE.MZ_SPT1 暫支俸點, \n");
                sb.Append("A_DLBASE.MZ_PCHIEF 主管級別, \n");
                sb.Append("A_DLBASE.MZ_EXTPOS_SRANK 兼代理職等, \n");
                sb.Append("A_DLBASE.MZ_SLFDATE 兵役起日期, \n");
                sb.Append("A_DLBASE.MZ_SLEDATE 兵役迄日期, \n");
                sb.Append("A_DLBASE.MZ_NREA 任到職原因, \n");
                sb.Append("A_DLBASE.MZ_ADATE 到職日期, \n");
                sb.Append("A_DLBASE.MZ_FDATE 初任公職日, \n");
                sb.Append("A_DLBASE.MZ_LDATE 離退職日期, \n");
                sb.Append("A_DLBASE.MZ_ZONE1 郵遞區號, \n");
                sb.Append("A_DLBASE.MZ_ADD1 戶籍地址, \n");
                sb.Append("A_DLBASE.MZ_ZONE2 郵遞區號, \n");
                sb.Append("A_DLBASE.MZ_ADD2 現住地址, \n");
                sb.Append("A_DLBASE.MZ_MOVETEL 行動電話, \n");
                sb.Append("A_DLBASE.MZ_AHP_RANK 權理職等, \n");
                sb.Append("A_DLBASE.MZ_ISPOLICE 是否為警職人員, \n");
                sb.Append("A_DLBASE.MZ_FYEAR 邊區年資, \n");
                sb.Append("B_BASE.CRIMELAB 是否為鑑識人員, \n");
                sb.Append("A_DLBASE.MZ_SALARY_ISDATE 薪資生效日, \n");
                sb.Append("(Select bbs_1.STOCKPILE_BANKID from B_BASE_STOCKPILE bbs_1 where bbs_1.\"GROUP\"='1' and bbs_1.IDCARD = B_BASE_STOCKPILE.IDCARD and A_DLBASE.PAY_AD = bbs_1.PAY_AD) 為優惠存款帳號, \n");
                sb.Append("(Select bbs_2.STOCKPILE_BANKID from B_BASE_STOCKPILE bbs_2 where bbs_2.\"GROUP\"='2' and bbs_2.IDCARD = B_BASE_STOCKPILE.IDCARD and A_DLBASE.PAY_AD = bbs_2.PAY_AD) 為薪資帳號, \n");
                sb.Append("A_DLBASE.MUSER 資料更新人ID, \n");
                sb.Append("A_DLBASE.MDATE 資料更新日期, \n");
                sb.Append("A_DLBASE.MZ_TDATE \"停(薪)職日\" \n");
                sb.Append("FROM A_DLBASE \n");
                sb.Append("left join B_BASE on B_BASE.IDCARD=A_DLBASE.MZ_ID \n");
                sb.Append("left join B_BASE_STOCKPILE on B_BASE_STOCKPILE.IDCARD=A_DLBASE.MZ_ID AND A_DLBASE.PAY_AD=B_BASE_STOCKPILE.PAY_AD \n");
                sb.Append(" \n");

                List<SqlParameter> oracleParameters = new List<SqlParameter>();


                if (model.AD != null && !string.IsNullOrEmpty(model.AD))
                {
                    sb.Append("WHERE (A_DLBASE.MZ_EXAD=@AD OR A_DLBASE.MZ_AD=@AD OR A_DLBASE.PAY_AD=@AD) AND A_DLBASE.MZ_STATUS2=@STATUS2 \n");
                    oracleParameters.Add(new SqlParameter("Status2", model.Status2.SafeTrim()));
                    oracleParameters.Add(new SqlParameter("AD", model.AD.SafeTrim()));
                }
                else
                {
                    sb.Append("WHERE A_DLBASE.MZ_STATUS2=@STATUS2 \n");
                    oracleParameters.Add(new SqlParameter("Status2", model.Status2.SafeTrim()));
                }

                sb.Append("  \n");

                dt = o_DBFactory.ABC_toTest.Create_Table(sb.ToString(), oracleParameters);
            }
            catch (Exception e)
            {
                throw;
            }

            return dt;
        }

    }

    /// <summary>
    /// 產生差勤薪資介接資料給北市府
    /// </summary>
    public class P_SalaryMetaData_Model
    {
        /// <summary>
        /// 有三種機關 編制、服務、發薪
        /// </summary>
        public string AD { get; set; }

        /// <summary>
        /// 是否離職 是→Y；否→N
        /// </summary>
        public string Status2 { get; set; }
    }

    public class A_PRKB_Model
    {
        /// <summary>
        /// 案號
        /// </summary>
        public string MZ_NO { get; set; }
        /// <summary>
        /// 身分證
        /// </summary>
        public string MZ_ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string MZ_NAME { get; set; }
        /// <summary>
        /// 服務機關代號
        /// </summary>
        public string MZ_AD { get; set; }
        /// <summary>
        /// 服務單位代號
        /// </summary>
        public string MZ_UNIT { get; set; }
        /// <summary>
        /// 職稱代碼
        /// </summary>
        public string MZ_OCCC { get; set; }
        /// <summary>
        /// 官職等起
        /// </summary>
        public string MZ_RANK { get; set; }
        /// <summary>
        /// 官職等迄
        /// </summary>
        public string MZ_RANK1 { get; set; }
        /// <summary>
        /// 薪俸職等
        /// </summary>
        public string MZ_SRANK { get; set; }
        /// <summary>
        /// 職序
        /// </summary>
        public string MZ_TBDV { get; set; }
        /// <summary>
        /// 獎懲內容
        /// </summary>
        public string MZ_PRCT { get; set; }
        /// <summary>
        /// 獎懲統計分類
        /// </summary>
        public string MZ_PRK1 { get; set; }
        /// <summary>
        /// 獎懲類別號
        /// </summary>
        public string MZ_PRK { get; set; }
        /// <summary>
        /// 獎懲結果
        /// </summary>
        public string MZ_PRRST { get; set; }
        /// <summary>
        /// 獎懲依據編號
        /// </summary>
        public string MZ_PROLNO { get; set; }
        /// <summary>
        /// 依據類別(固定H)
        /// </summary>
        public string MZ_POLK { get; set; }
        /// <summary>
        /// 是否分配(固定1)
        /// </summary>
        public string MZ_PCODE { get; set; }
        /// <summary>
        /// 說明
        /// </summary>
        public string MZ_MEMO { get; set; }
        /// <summary>
        /// 權責(系統判斷)
        /// </summary>
        public string MZ_SWT3 { get; set; }
        public string MZ_SWT4 { get; set; }
        public string MZ_SWT1 { get; set; }
        public string MZ_RDATE { get; set; }
        public string MZ_PCODEM { get; set; }
        public string MZ_REMARK { get; set; }
        /// <summary>
        /// 現服機關
        /// </summary>
        public string MZ_EXAD { get; set; }
        /// <summary>
        /// 現服單位
        /// </summary>
        public string MZ_EXUNIT { get; set; }
        public string MZ_PROLNO2 { get; set; }
        /// <summary>
        /// 匯入人MZ_ID
        /// </summary>
        public string MUSER { get; set; }
        /// <summary>
        /// 匯入日期(民國日期7碼)
        /// </summary>
        public string MDATE { get; set; }
        public string SWT4_USER { get; set; }
        public string SWT4_DATE { get; set; }
        /// <summary>
        /// 系統序號
        /// </summary>
        public string SN { get; set; }
    }
    public class A_PRK2_Model
    {
        public string MZ_NO { get; set; }
        public string MZ_ID { get; set; }
        public string MZ_NAME { get; set; }
        public string MZ_AD { get; set; }
        public string MZ_UNIT { get; set; }
        public string MZ_OCCC { get; set; }
        public string MZ_RANK { get; set; }
        public string MZ_RANK1 { get; set; }
        public string MZ_SRANK { get; set; }
        public string MZ_TBDV { get; set; }
        public string MZ_PRCT { get; set; }
        public string MZ_PRK { get; set; }
        public string MZ_POLK { get; set; }
        public string MZ_PRRST { get; set; }
        public string MZ_PRK1 { get; set; }
        public string MZ_DATE { get; set; }
        public string MZ_PRID { get; set; }
        public string MZ_PRID1 { get; set; }
        public string MZ_CHKAD { get; set; }
        public string MZ_IDATE { get; set; }
        public string MZ_MEMO { get; set; }
        public string MZ_SWT { get; set; }
        public string MZ_PRPASS { get; set; }
        public string MZ_PROLNO { get; set; }
        public string MZ_PCODE { get; set; }
        public string MZ_PCODEM { get; set; }
        public string MZ_SWT3 { get; set; }
        public string MZ_REMARK { get; set; }
        public string MZ_SWT2 { get; set; }
        public string MZ_PROLNO2 { get; set; }
        public string MZ_EXAD { get; set; }
        public string MZ_EXUNIT { get; set; }
        public string MUSER { get; set; }
        public string MDATE { get; set; }
        public string SPEED_NO { get; set; }
        public string PWD_NO { get; set; }
        public string MZ_FILENO { get; set; }
        public string MZ_YEARUSE { get; set; }
    }
}