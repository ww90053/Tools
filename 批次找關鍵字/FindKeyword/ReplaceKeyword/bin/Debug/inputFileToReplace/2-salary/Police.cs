using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    [Serializable]
    public class Police
    {
        public bool isNew;
        public string id;
        public string polno;
        public string name;
        public string occc;
        public string occcName;
        public string ad;
        public string adName;
        public string exad;
        public string exadName;
        public string payad;
        public string payadName;
        public string unit;
        public string unitName;
        public string exunit;
        public string exunitName;
        public string payunit;
        public string payunitName;
        public string birth;
        public string sex;// 性別
        public string phone;
        public string cellphone;
        public string contactAddress;
        public string address;
        public string srank;
        public string srankName;
        public string slvc;
        public string tempSpt;// 暫支俸點
        public string spt;
        public string grade;// 去年考績等級
        public string fdate;
        public string adate;
        public string ldate;
        public string tdate;// 停職日
        public string odate;// 復職日
        public string chief;// 主管級別
        public string chiefDate;// 任主管日期
        public string isExtpos;// 是否兼代職
        public string extpos;// 兼代職級別
        public string extposSrank;// 兼代職職等
        public string nrea;// 任(退)職原因

        //public bool is30Years;// 任公職是否滿30年

        /// <summary>
        /// 是否為公職? 注意這邊名稱取錯了,判斷警職應改用 isPolice_G
        /// </summary>
        public bool isPolice;
        /// <summary>
        /// 是否為警職?
        /// </summary>
        public bool isPolice_G
        {
            get
            {
                if ((this.srank ?? "").StartsWith("G"))
                    return true;

                return false;
            }
        }


        public bool ADLBASE_isPolice;// 人員基本資料表中是否為警察人員
        public bool isCrimelab;// 是否為鑑識人員
        public string policeType;// 人員身份：1代表警察、公務人員、約雇、臨時人員，2代表司機、工友、技工
        public bool isGovEmployee;// 是不是警職
        public bool isOffduty = false;// 是否停職
        public int salary = 0;// 薪俸
        public int boss = 0;// 主管加給
        public int profess = 0;// 專業加給
        public string technicCode = "";   // sam.hsu 20201218 技術加給的代碼
        public int technic = 0;           // sam.hsu 20201218 技術加給的金額                                 
        public string workpCode = "";
        public int workp = 0;// 警勤加給
        public string bonusCode = "";
        public int bonus = 0;
        public string adventiveCode = "";
        public int adventive = 0;
        public int far = 0;
        //public string electricCode = "";
        public int electric = 0;//繁重加給
        public int otheradd = 0;
        public string note = "";
        public string insuranceType;// 保險種類(公保/勞保)
        public int insurance = 0;// 公/勞保費
        public int concur3 = 0;// 退撫金費
        public int healthPersonal = 0;// 個人健保費
        public int deHealth = 0;// 健保加保人數
        public int health = 0;// 健保費
        public int tax = 0;// 所得稅
        public int taxChild = 0;// 所得稅撫養人
        public int salaryCost = 0;// 薪資扣款
        public int salaryCostTax = 0;// 薪資扣款(列入所得)
        public int extra01 = 0;// 法院扣款
        public int extra02 = 0;// 國宅貸款
        public int extra03 = 0;// 銀行貸款
        public int extra04 = 0;// 分期付款
        public int extra05 = 0;// 優惠存款
        public int extra06 = 0;// 員工宿舍費
        public int extra07 = 0;// 伙食費
        public int extra08 = 0;// 福利互助金費
        public int extra09 = 0;// 退撫金貸款
        public int otherminus = 0;// 其他應扣
        public int healthper = 0; // 健保百分比
        public int totalPay = 0;// 應發總額

        public int pb2 = 0; // mz_PB2

        public string AHP_RANK; //權理職等 Add by sky 20190730

        /// <summary>
        /// 邊區年資
        /// </summary>
        public string FYEAR { get; set; }
        /// <summary>
        /// 偏遠(地域加給)
        /// </summary>
        public int FARPAY { get; set; }

        /// <summary>
        /// 帶職進修 Y → 只領警勤加給 6745 並且 繁重加給 設為 0
        /// </summary>
        public string TRAINING { get; set; }

        /// <summary>
        /// 合格實授日期
        /// </summary>
        public string MZ_QUA_DATE { get; set; }


        /// <summary>
        /// 保費新舊制? 舊制D,新制F
        /// 用於判斷B_SALARY,要用INSURANCE_D或INSURANCE_F
        /// </summary>
        public string INSURANCE_TYPE
        {
            get
            {
                /*
                 舊制(D)：A_DLBASE欄位 MZ_FDATE (初任公職日)<’1120701’或 “空值”
                 新制(F)：A_DLBASE欄位 MZ_FDATE (初任公職日)>=’1120701’

舊制抓D,新制抓F 
(1)	A_DLBASE欄位MZ_FDATE<’1120101’或”空值”→抓INSURANCE_D
(2)	A_DLBASE欄位MZ_FDATE>=’1120701’→ 抓INSURANCE_F
(3)	A_DLBASE欄位MZ_FDATE介於1120101-1120630且MZ_QUA_DATE(合格實授日期)>’1120630’ → 抓INSURANCE_F
(4)	A_DLBASE欄位MZ_FDATE介於1120101-1120630且MZ_QUA_DATE<=’1120630’ →抓INSURANCE_D
(5)	A_DLBASE欄位MZ_FDATE介於1120101-1120630且MZ_QUA_DATE為空值 →抓INSURANCE_D

                 */
                //沒寫就是舊
                //(1)	A_DLBASE欄位MZ_FDATE<’1120101’或”空值”→抓INSURANCE_D
                if (string.IsNullOrEmpty(this.fdate))
                {
                    return "D";//舊

                }
                //沒超過1120101這天,也是舊
                if (string.Compare(this.fdate, "1120101") == -1)
                {
                    return "D";//舊
                }
                //(2)	A_DLBASE欄位MZ_FDATE>=’1120701’→ 抓INSURANCE_F
                if (string.Compare(this.fdate, "1120701") >= 0)
                {
                    return "F";//新
                }
                //以下都是 A_DLBASE欄位MZ_FDATE介於1120101-1120630
                //(3)	A_DLBASE欄位MZ_FDATE介於1120101-1120630且 MZ_QUA_DATE(合格實授日期)>’1120630’ → 抓INSURANCE_F
                if (string.Compare(this.MZ_QUA_DATE, "1120630") >= 1)
                {
                    return "F";//新
                }
                //(4)	A_DLBASE 欄位 MZ_FDATE 介於1120101-1120630 且 MZ_QUA_DATE<=’1120630’ →抓INSURANCE_D
                if (string.Compare(this.MZ_QUA_DATE, "1120630") <= 0)
                {
                    return "D";//舊
                }
                //(5)	A_DLBASE欄位MZ_FDATE介於1120101-1120630且MZ_QUA_DATE為空值 →抓INSURANCE_D                
                if (string.IsNullOrEmpty(this.MZ_QUA_DATE))
                {
                    return "D";//舊

                }

                return "F";//新
            }
        }

        public DataTable healthInfo;
        public DataTable accountInfo;

        public Police(string id)
        {
            getBasicData(id);

            if (this.id == null)
                return;
            getSalary();
        }

        /// <summary>
        /// 取得員警基本資料
        /// </summary>
        public void getBasicData(string id)
        {
            string sql;
            DataTable dt = new DataTable();
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"
--多join A_DLBASE純粹是為了抓取 MZ_QUA_DATE
--正確應該是去正式機將VW_A_DLBASE_S2追加MZ_QUA_DATE
SELECT S2.* ,A.MZ_QUA_DATE
FROM VW_A_DLBASE_S2  S2
join A_DLBASE A on S2.MZ_ID=A.MZ_ID
WHERE S2.MZ_ID=@MZ_ID ";

            ops.Add(new SqlParameter("MZ_ID", id));

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
                return;

            //20140417
            // 將此員警的基本資料填入屬性中
            this.id = dt.Rows[0]["MZ_ID"].ToString();
            this.polno = dt.Rows[0]["MZ_POLNO"].ToString();
            this.name = dt.Rows[0]["MZ_NAME"].ToString();

            this.occc = dt.Rows[0]["MZ_OCCC"].ToString();
            this.occcName = dt.Rows[0]["MZ_OCCC_CH"].ToString();

            this.ad = dt.Rows[0]["MZ_AD"].ToString();
            this.adName = dt.Rows[0]["MZ_AD_CH"].ToString();


            this.exad = dt.Rows[0]["MZ_EXAD"].ToString();
            this.exadName = dt.Rows[0]["MZ_EXAD_CH"].ToString();

            this.payad = dt.Rows[0]["PAY_AD"].ToString();
            this.payadName = dt.Rows[0]["PAY_AD_CH"].ToString();

            this.unit = dt.Rows[0]["MZ_UNIT"].ToString();
            this.unitName = dt.Rows[0]["MZ_UNIT_CH"].ToString();

            this.exunit = dt.Rows[0]["MZ_EXUNIT"].ToString();
            this.exunitName = dt.Rows[0]["MZ_EXUNIT_CH"].ToString();

            this.payunit = dt.Rows[0]["MZ_POLNO"].ToString().Length < 4 ? "" : dt.Rows[0]["MZ_POLNO"].ToString().Substring(0, 4);
            this.payunitName = o_A_KTYPE.CODE_TO_NAME(this.payunit, "25");
            int.TryParse(dt.Rows[0]["MZ_PB2"].ToString(), out this.pb2);

            this.birth = dt.Rows[0]["MZ_BIR"].ToString();
            this.sex = dt.Rows[0]["MZ_SEX"].ToString();
            this.phone = dt.Rows[0]["MZ_PHONE"].ToString();
            this.cellphone = dt.Rows[0]["MZ_PHONO"].ToString();
            this.contactAddress = dt.Rows[0]["MZ_ADD2"].ToString();
            this.address = dt.Rows[0]["MZ_ADD1"].ToString();

            this.srank = dt.Rows[0]["MZ_SRANK"].ToString().Trim();
            this.srankName = dt.Rows[0]["MZ_SRANK_CH"].ToString().Trim();

            this.slvc = dt.Rows[0]["MZ_SLVC"].ToString().Trim();
            this.tempSpt = dt.Rows[0]["MZ_SPT1"].ToString() == "" ? "" : dt.Rows[0]["MZ_SPT1"].ToString().PadLeft(4, '0');
            this.spt = dt.Rows[0]["MZ_SPT"].ToString().PadLeft(4, '0');
            this.fdate = dt.Rows[0]["MZ_FDATE"].ToString();
            this.adate = dt.Rows[0]["MZ_ADATE"].ToString();
            this.ldate = dt.Rows[0]["MZ_LDATE"].ToString();
            this.tdate = dt.Rows[0]["MZ_TDATE"].ToString();
            this.odate = dt.Rows[0]["MZ_ODATE"].ToString();
            this.chief = dt.Rows[0]["MZ_PCHIEF"].ToString().Trim();
            this.chiefDate = dt.Rows[0]["MZ_PCHIEFDATE"].ToString();
            this.isExtpos = dt.Rows[0]["MZ_ISEXTPOS"].ToString() == "" ? "N" : dt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim();
            this.extpos = dt.Rows[0]["MZ_EXTPOS"].ToString().Trim();
            this.extposSrank = dt.Rows[0]["MZ_EXTPOS_SRANK"].ToString().Trim();
            this.nrea = dt.Rows[0]["MZ_NREA"].ToString().Trim();

            this.AHP_RANK = dt.Rows[0]["MZ_AHP_RANK"].ToString();

            // sam.hsu 20201128 加上 邊區年資
            this.FYEAR = dt.Rows[0]["MZ_FYEAR"].ToString();

            // sam.hsu 20201208 加上 帶職進修
            this.TRAINING = dt.Rows[0]["MZ_TRAINING"].ToString();
            //合格實授日期
            this.MZ_QUA_DATE = dt.Rows[0]["MZ_QUA_DATE"].ToString();

            // 若MZ_SRANK職等為G開頭，表示警察人員、公務人員
            this.isPolice = checkIsPolice(this.srank);

            bool _ADLBASE_isPolice = false;

            if (dt.Rows[0]["mz_ispolice"] != null)
            {
                if (dt.Rows[0]["mz_ispolice"].ToString().ToUpper().Equals("Y"))
                {
                    _ADLBASE_isPolice = true;
                }
            }

            this.ADLBASE_isPolice = _ADLBASE_isPolice;

            this.policeType = getPoliceType(this.occc);

            if (this.srank.StartsWith("P") || this.srank.StartsWith("G") || this.srank.StartsWith("B"))
                this.insuranceType = "公保";
            else
                this.insuranceType = "勞保";
        }

        /// <summary>
        /// 薪資系統更新人事基本資料
        /// </summary>
        /// <param name="ops"></param>
        /// <param name="autoUpdateSalary"></param>
        public void updateBasicData(List<SqlParameter> ops, bool autoUpdateSalary)
        {
            if (ops.Count == 0)
                return;

            string sql;
            bool usalary = false;

            sql = "UPDATE A_DLBASE SET ";

            foreach (SqlParameter para in ops)
            {
                sql += para.ParameterName + "=:" + para.ParameterName + ",";
                if (para.ParameterName == "MZ_SRANK")
                    usalary = true;
                if (para.ParameterName == "MZ_SPT")
                    usalary = true;
                //暫支薪點的修改
                if (para.ParameterName == "MZ_SPT1")
                    usalary = true;

            }

            sql = sql.Substring(0, sql.Length - 1);
            sql += " WHERE MZ_ID=@MZ_ID";

            //20140327
            ops.Add(new SqlParameter("MZ_ID", this.id));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
            }

            //如果有改到職等、俸階、俸點，要重算應發(薪俸、專業、主管)、應扣金額(健保、公勞保、所得稅、退撫金)(選擇性，由autoUpdateSalary決定)
            if (autoUpdateSalary)
                if (usalary)
                {
                    Police tempPolice = new Police(this.id);
                    tempPolice.updateSalary();
                }
        }

        /// <summary>
        /// 取得員警薪資資料
        /// </summary>
        void getSalary()
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            sql = "SELECT * FROM B_BASE WHERE IDCARD=@IDCARD";
            ops.Add(new SqlParameter("IDCARD", this.id));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            if (dt.Rows.Count > 0)
            {
                this.isNew = false;

                //20150216 改抓人事的最後一筆考績等第
                string strGrade = "SELECT  T26 FROM A_EFFICIENCY WHERE MZ_ID=@MZ_ID ORDER BY T01 DESC  ";
                List<SqlParameter> para = new List<SqlParameter>();
                para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = this.id });

                this.grade = o_DBFactory.ABC_toTest.GetValue(strGrade, para);
                //20150216
                //this.grade = dt.Rows[0]["GRADE"].ToString();

                this.isGovEmployee = SalaryBasic.boolCHK_MZ_OCCC_Data_Serach(this.id);

                if (dt.Rows[0]["CRIMELAB"].ToString() == "是")
                    this.isCrimelab = true;
                else
                    this.isCrimelab = false;

                //20140618
                //if (dt.Rows[0]["IS30YEAR"].ToString() == "是")
                //    this.is30Years = true;
                //else
                //    this.is30Years = false;

                //取得健保加保狀態
                getHealthInfo();

                // 應發
                int.TryParse(dt.Rows[0]["SALARYPAY"].ToString(), out this.salary);
                int.TryParse(dt.Rows[0]["BOSSPAY"].ToString(), out this.boss);
                int.TryParse(dt.Rows[0]["PROFESSPAY"].ToString(), out this.profess);
                this.technicCode = dt.Rows[0]["TECHNICS"].ToString();
                int.TryParse(dt.Rows[0]["TECHNICSPAY"].ToString(), out this.technic);
                this.bonusCode = dt.Rows[0]["BONUS"].ToString();
                int.TryParse(dt.Rows[0]["BONUSPAY"].ToString(), out this.bonus);
                this.workpCode = dt.Rows[0]["WORKP"].ToString();
                int.TryParse(dt.Rows[0]["WORKPPAY"].ToString(), out this.workp);
                this.adventiveCode = dt.Rows[0]["ADVENTIVE"].ToString();
                int.TryParse(dt.Rows[0]["ADVENTIVEPAY"].ToString(), out this.adventive);
                int.TryParse(dt.Rows[0]["FARPAY"].ToString(), out this.far);
                //this.electricCode = dt.Rows[0]["ELECTRIC"].ToString();
                int.TryParse(dt.Rows[0]["ELECTRICPAY"].ToString(), out this.electric);
                this.note = dt.Rows[0]["NOTE"].ToString();
                int.TryParse(dt.Rows[0]["OTHERADD"].ToString(), out this.otheradd);

                refreshTotalPay();

                // 應扣
                int.TryParse(dt.Rows[0]["CONCUR3PAY"].ToString(), out this.concur3);
                int.TryParse(dt.Rows[0]["PERSONALHEALTHPAY"].ToString(), out this.healthPersonal);
                int.TryParse(dt.Rows[0]["HEALTHPAY"].ToString(), out this.health);
                int.TryParse(dt.Rows[0]["INSURANCEPAY"].ToString(), out this.insurance);
                int.TryParse(dt.Rows[0]["TAXPER"].ToString(), out this.tax);
                int.TryParse(dt.Rows[0]["TAXCHILD"].ToString(), out this.taxChild);
                int.TryParse(dt.Rows[0]["DE_HEALTH"].ToString(), out this.deHealth);
                int.TryParse(dt.Rows[0]["MONTHPAY_TAX"].ToString(), out this.salaryCostTax);
                int.TryParse(dt.Rows[0]["MONTHPAY"].ToString(), out this.salaryCost);
                int.TryParse(dt.Rows[0]["EXTRA01"].ToString(), out this.extra01);
                int.TryParse(dt.Rows[0]["EXTRA02"].ToString(), out this.extra02);
                int.TryParse(dt.Rows[0]["EXTRA03"].ToString(), out this.extra03);
                int.TryParse(dt.Rows[0]["EXTRA04"].ToString(), out this.extra04);
                int.TryParse(dt.Rows[0]["EXTRA05"].ToString(), out this.extra05);
                int.TryParse(dt.Rows[0]["EXTRA06"].ToString(), out this.extra06);
                int.TryParse(dt.Rows[0]["EXTRA07"].ToString(), out this.extra07);
                int.TryParse(dt.Rows[0]["EXTRA08"].ToString(), out this.extra08);
                int.TryParse(dt.Rows[0]["EXTRA09"].ToString(), out this.extra09);
                int.TryParse(dt.Rows[0]["OTHERMINUS"].ToString(), out this.otherminus);
                //百分比資料抓入20210115 Mark
                int.TryParse(dt.Rows[0]["HEALPER_INSURANCE"].ToString(), out this.healthper);

                if (dt.Rows[0]["ISOFFDUTY"].ToString() == "是")
                    this.isOffduty = true;
                else
                    this.isOffduty = false;

                // 取得帳戶資料
                ops.Clear();
                sql = "SELECT BS_SNID, IDCARD, BS.BANKID+'('+BANK_NAME+')' AS BANK_NAME_DATA, BS.BANKID, BANK_NAME, STOCKPILE_BANKID, PAY_AD, MZ_KCHI, CASE WHEN (\"GROUP\") = '1' THEN '優惠存款' WHEN (\"GROUP\") = '2' THEN '薪資轉帳' WHEN (\"GROUP\") = '3' THEN '國宅貸款' WHEN (\"GROUP\") = '4' THEN '退撫金貸款' WHEN (\"GROUP\") = '5' THEN '分期付款' WHEN (\"GROUP\") = '6' THEN '銀行代款' WHEN (\"GROUP\") = '7' THEN '法院扣款' END AS \"GROUP\" FROM B_BASE_STOCKPILE BS LEFT JOIN B_BANK_LIST BL ON BS.BANKID = BL.BANK_ID LEFT JOIN A_KTYPE ON PAY_AD = MZ_KCODE WHERE IDCARD=@IDCARD ORDER BY \"GROUP\"";
                ops.Add(new SqlParameter("IDCARD", this.id));
                this.accountInfo = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            }
            else// 新進人員，還沒有薪資資料
            {
                this.isNew = true;

                this.salary = Salary.getSalaryPay(this.policeType, this.isPolice_G, this.spt, this.tempSpt);
                this.boss = Salary.getBossPay(this.extposSrank, this.chief, this.srank);
                this.profess = Salary.getProfessPay(this.srank, this.isCrimelab, this.AHP_RANK);
                //20140618
                this.insurance = this.getInsurancePay();
                //this.insurance = Salary.getInsurancePay(this.policeType, this.isPolice, this.spt, this.is30Years);
                //新進人員警勤加給判斷，職等P開頭的一定沒有警勤加給
                if (this.srank.StartsWith("G"))
                {
                    this.workpCode = "03";// 8435
                    if (this.exad == "382130000C")
                        this.workpCode = "02";// 6745
                    if (this.exad == "382130100C" || this.exad == "382130300C")// 交大、保大
                        if (this.exunit == "0302")
                            this.workpCode = "03";// 8435
                        else
                            this.workpCode = "02";// 6745
                    if (this.occc == "1452" || this.occc == "1495" || this.exunit == "0104" || this.exunit == "0105" || this.exunit == "0106" || this.exunit == "0107" || this.exunit == "0108" || this.exunit == "0109" || this.exunit == "0110" || this.exunit == "0111" || this.exunit == "0112" || this.exunit == "0113" || this.exunit.StartsWith("DH"))
                        this.workpCode = "04";// 13496
                }
                if (this.srank.StartsWith("P"))
                    this.workpCode = null;

                //已經存成錢了 , 所以不要再以workpCode去抓出警勤加給 Mark 2021/1/15
                //this.workp = Salary.getPay(this.workpCode, "B_WORKP");

                this.healthPersonal = Salary.getHealthPersonal(this.salary, this.boss, this.profess, this.workp, this.technic, this.bonus, this.adventive, this.far, this.electric, this.insuranceType);
                this.health = this.healthPersonal;
                this.concur3 = Salary.getConcur3(this.policeType, this.isPolice_G, this.spt, this.tempSpt);

                createSalary();
            }
        }

        /// <summary>
        /// 重計應發總額
        /// </summary>
        void refreshTotalPay()
        {
            this.totalPay = this.salary + this.boss + this.profess + this.technic + this.bonus + this.workp + this.adventive + this.far + this.electric;
        }

        /// <summary>
        /// 更新薪資資料
        /// </summary>
        public void updateSalaryData(List<SqlParameter> ops)
        {
            if (ops.Count == 0)
                return;

            string sql;

            sql = "UPDATE B_BASE SET LASTDA=@LASTDA, LASTUSER=@LASTUSER, ";

            foreach (SqlParameter para in ops)
            {
                sql += para.ParameterName + "=:" + para.ParameterName + ",";
            }

            sql = sql.Substring(0, sql.Length - 1);
            sql += " WHERE IDCARD=@IDCARD";

            ops.Add(new SqlParameter("IDCARD", this.id));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("LASTUSER", SalaryPublic.strLoginID));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
            }
        }

        /// <summary>
        /// 建立薪資資料
        /// </summary>
        void createSalary()
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "INSERT INTO B_BASE(B_SNID, IDCARD, SALARYPAY, BOSSPAY, PROFESSPAY, INSURANCEPAY, PERSONALHEALTHPAY, HEALTHPAY, CONCUR3PAY, WORKP, WORKPPAY, LASTDA, LASTUSER) VALUES( NEXT VALUE FOR dbo.B_BASE_SN, @IDCARD, @SALARYPAY, @BOSSPAY, @PROFESSPAY, @INSURANCEPAY, @PERSONALHEALTHPAY, @HEALTHPAY, @CONCUR3PAY, @WORKP, @WORKPPAY, @LASTDA, @LASTUSER)";

            ops.Add(new SqlParameter("IDCARD", this.id));
            ops.Add(new SqlParameter("SALARYPAY", this.salary));
            ops.Add(new SqlParameter("BOSSPAY", this.boss));
            ops.Add(new SqlParameter("PROFESSPAY", this.profess));
            ops.Add(new SqlParameter("INSURANCEPAY", this.insurance));
            ops.Add(new SqlParameter("PERSONALHEALTHPAY", this.healthPersonal));
            ops.Add(new SqlParameter("HEALTHPAY", this.health));
            ops.Add(new SqlParameter("CONCUR3PAY", this.concur3));
            if (this.workpCode == null)
                ops.Add(new SqlParameter("WORKP", DBNull.Value));
            else
                ops.Add(new SqlParameter("WORKP", this.workpCode));
            ops.Add(new SqlParameter("WORKPPAY", this.workp));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("LASTUSER", SalaryPublic.strLoginID));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
        }

        /// <summary>
        /// 修改人事資料後，請呼叫此方法。這個方法會以修改後的人事資料重新計算薪資資料
        /// </summary>
        public void updateSalary()
        {
            //if (this.workp != 0)//20170721 測試警勤加給是0 就不計算
            //{

            if (this.srank.Substring(0, 1) == "P" || this.srank.Substring(0, 1) == "G")
            {
                string sql;
                List<SqlParameter> ops = new List<SqlParameter>();

                sql = "UPDATE B_BASE SET SALARYPAY=@SALARYPAY, BOSSPAY=@BOSSPAY, PROFESSPAY=@PROFESSPAY, INSURANCEPAY=@INSURANCEPAY, PERSONALHEALTHPAY=@PERSONALHEALTHPAY, HEALTHPAY=@HEALTHPAY, CONCUR3PAY=@CONCUR3PAY, WORKP=@WORKP, WORKPPAY=@WORKPPAY, LASTDA=@LASTDA, LASTUSER=@LASTUSER ,EXTRA01=@EXTRA01, FARPAY=@FARPAY, electricpay = @electricpay WHERE IDCARD=@IDCARD";


                List<int> getSalaryPay__InsurancePay_Concur3 = Salary.getSalaryPay__InsurancePay_Concur3(this.policeType, this.ADLBASE_isPolice, this.spt, this.tempSpt, this);

                //薪俸
                this.salary = getSalaryPay__InsurancePay_Concur3[0];
                //this.salary = Salary.getSalaryPay(this.policeType, this.isPolice, this.spt, this.tempSpt);
                //主管加給
                this.boss = Salary.getBossPay(this.extposSrank, this.chief, this.srank);
                //專業加給
                this.profess = Salary.getProfessPay(this.srank, this.isCrimelab, this.AHP_RANK);




                //20140618
                //公保          
                this.insurance = getInsurancePay();

                //退撫金
                this.concur3 = getSalaryPay__InsurancePay_Concur3[1];

                //計算個人健保費
                this.healthPersonal = Salary.getHealthPersonal(this.salary, this.boss, this.profess, this.workp, this.technic, this.bonus, this.adventive, this.far, this.electric, this.insuranceType);

                //用新的個人健保費更新加保人保費
                this.healthInfo = Salary.getHealthInfo(this.healthInfo, this.healthPersonal);
                //將更新後的加保人保費存進資料庫
                updateHealthInfo(this.healthInfo);

                //需再調整 20210115 Mark
                this.healthPersonal = (int)Math.Floor((decimal)this.healthPersonal * this.healthper / 100);

                //取得新的健保費總額
                //20140618
                this.health = Salary.getHealth(this.healthInfo, this.healthPersonal);
                //this.health = Salary.getHealth(this.healthInfo, this.healthPersonal, this.is30Years);

                // sam.hsu wellsince 20201128 警勤加給 
                this.workp = Salary.getWORKPPY(this);

                // sam.hsu wellsince 20201201 繁重加給 
                //繁重加給
                this.electric = Salary.getElectric(this);

                //20140326//
                //如果停職本俸就計算半薪
                //專業加給不給
                //目前直接寫在公式是.如有問題再挪出來
                if (this.isOffduty)
                {
                    this.salary = Convert.ToInt32(Math.Round((decimal)this.salary / 2, 0, MidpointRounding.AwayFromZero));

                    this.profess = 0;
                }

                //因為更動過資料後
                //法院扣款有些數值也會變
                //目前直接寫在公式是.如有問題再挪出來
                if (this.extra01 > 0)
                {
                    //this.extra01 = Salary.getExtra01(this.salary + this.boss + this.profess + Salary.getPay(this.technicCode, "B_TECHNICS") + Salary.getPay(this.bonusCode, "B_BONUS") + Salary.getPay(this.workpCode, "B_WORKP") + this.far + Salary.getPay(this.electricCode, "B_ELECTRIC"));

                    //20140721  原水電輔助 改成繁重加給 算入法院扣款中
                    this.extra01 = Salary.getExtra01(this.salary + this.boss + this.profess + Salary.getPay(this.technicCode, "B_TECHNICS")
                        + Salary.getPay(this.bonusCode, "B_BONUS") + Salary.getPay(this.workpCode, "B_WORKP")
                        + this.far + this.electric

                        );

                }

                // sam.hsu wellsince 計算偏遠(地域加給)
                this.FARPAY = Salary.getFARPAY(this);

                ops.Add(new SqlParameter("IDCARD", this.id));
                ops.Add(new SqlParameter("SALARYPAY", this.salary));
                ops.Add(new SqlParameter("BOSSPAY", this.boss));
                ops.Add(new SqlParameter("PROFESSPAY", this.profess));
                ops.Add(new SqlParameter("INSURANCEPAY", this.insurance));
                ops.Add(new SqlParameter("PERSONALHEALTHPAY", this.healthPersonal));
                ops.Add(new SqlParameter("HEALTHPAY", this.health));
                ops.Add(new SqlParameter("CONCUR3PAY", this.concur3));
                ops.Add(new SqlParameter("WORKP", this.workpCode));
                ops.Add(new SqlParameter("WORKPPAY", this.workp));
                ops.Add(new SqlParameter("LASTDA", DateTime.Now));
                ops.Add(new SqlParameter("LASTUSER", SalaryPublic.strLoginID));
                ops.Add(new SqlParameter("EXTRA01", this.extra01));
                ops.Add(new SqlParameter("FARPAY", this.FARPAY));
                ops.Add(new SqlParameter("electricpay", this.electric));

                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

                if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
                {
                    TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", SalaryPublic.RegixSQL(sql, ops));
                }
                //}
            }
        }

        void getHealthInfo()
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT ROWNUM SN, B_HEALTH_MODE.ID ModeID, B_HEALTH_MODE.TEXT \"Mode\", B_HEALTH_MODE.COST \"Percent\", B_HEALTH_RELATION.ID RelationID, B_HEALTH_RELATION.TEXT Relation, B_HEALTH_INFORMATION.COST \"Cost\" FROM B_HEALTH_INFORMATION JOIN B_HEALTH_MODE ON HEALTH_MODE_ID=B_HEALTH_MODE.ID LEFT JOIN B_HEALTH_RELATION ON RELATION=B_HEALTH_RELATION.ID WHERE IDCARD=@IDCARD";

            ops.Add(new SqlParameter("IDCARD", this.id));

            this.healthInfo = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
        }

        public void updateHealthInfo(DataTable healthInfo)
        {
            if (healthInfo == null)
                return;
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "DELETE B_HEALTH_INFORMATION WHERE IDCARD=@IDCARD";
            ops.Add(new SqlParameter("IDCARD", this.id));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            foreach (DataRow row in healthInfo.Rows)
            {
                ops.Clear();
                sql = "INSERT INTO B_HEALTH_INFORMATION(SN, IDCARD, HEALTH_MODE_ID, COST, RELATION) VALUES( NEXT VALUE FOR dbo.B_HEALTH_INFO_SN, @IDCARD, @HEALTH_MODE_ID, @COST, @RELATION)";
                ops.Add(new SqlParameter("IDCARD", this.id));
                ops.Add(new SqlParameter("HEALTH_MODE_ID", row["ModeID"].ToString()));
                ops.Add(new SqlParameter("COST", int.Parse(row["Cost"].ToString())));
                ops.Add(new SqlParameter("RELATION", row["RelationID"].ToString()));
                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
            }
        }

        public bool createRepair(string payad, string amonth, string batchNumber, string sda, string eda, string ssrank, string esrank, string sspt, string espt, int atype, int adays,
            int salary, int work, int profess, int boss, int tech, int bonus, int adventive, int far, int electric, int otheradd,
            int insurance, int healthPersonal, int healthMan, int health, int healthpay1, int monthpayTax, int monthpay, int concur3, int tax, int otherminus,
            int extra01, int extra02, int extra03, int extra04, int extra05, int extra06, int extra07, int extra08, int extra09, string note)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = @"INSERT INTO B_REPAIRPAY(R_SNID, IDCARD, MZ_NAME, AMONTH, BATCH_NUMBER, PAY_AD, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT, MZ_POLNO, MZ_UNIT, GRADE, 
                    SDA, EDA, SMZ_SRANK, EMZ_SRANK, SMZ_SPT, EMZ_SPT, ATYPE, ADAYS, 
                    SALARYPAY1, WORKPPAY, PROFESSPAY, BOSSPAY, TECHNICSPAY, BONUSPAY, ADVENTIVEPAY, FARPAY, ELECTRICPAY, OTHERADD, INSURANCEPAY, HEALTHID, HEALTHMAN, HEALTHPAY, HEALTHPAY1, MONTHPAY_TAX, MONTHPAY, CONCUR3PAY, TAX, OTHERMINUS, EXTRA01, EXTRA02, EXTRA03, EXTRA04, EXTRA05, EXTRA06, EXTRA07, EXTRA08, EXTRA09, NOTE, LOCKDB, LASTDA, CREATOR) 
                    VALUES( NEXT VALUE FOR dbo.B_REPAIRPAY_SN, @IDCARD, @MZ_NAME, @AMONTH, @BATCH_NUMBER, @PAY_AD, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @MZ_SPT, @MZ_POLNO, @MZ_UNIT, @GRADE, 
                    @SDA, @EDA, @SMZ_SRANK, @EMZ_SRANK, @SMZ_SPT, @EMZ_SPT, @ATYPE, @ADAYS, 
                    @SALARYPAY1, @WORKPPAY, @PROFESSPAY, @BOSSPAY, @TECHNICSPAY, @BONUSPAY, @ADVENTIVEPAY, @FARPAY, @ELECTRICPAY, @OTHERADD, @INSURANCEPAY, @HEALTHID, @HEALTHMAN, @HEALTHPAY, @HEALTHPAY1, @MONTHPAY_TAX, @MONTHPAY, @CONCUR3PAY, @TAX, @OTHERMINUS, @EXTRA01, @EXTRA02, @EXTRA03, @EXTRA04, @EXTRA05, @EXTRA06, @EXTRA07, @EXTRA08, @EXTRA09, @NOTE, @LOCKDB, @LASTDA, @CREATOR)";
            ops.Add(new SqlParameter("IDCARD", this.id));
            ops.Add(new SqlParameter("MZ_NAME", this.name));
            ops.Add(new SqlParameter("AMONTH", amonth));
            ops.Add(new SqlParameter("BATCH_NUMBER", batchNumber));
            ops.Add(new SqlParameter("PAY_AD", payad));
            ops.Add(new SqlParameter("MZ_OCCC", this.occc));
            ops.Add(new SqlParameter("MZ_SRANK", this.srank));
            ops.Add(new SqlParameter("MZ_SLVC", this.slvc));
            ops.Add(new SqlParameter("MZ_SPT", this.spt));
            ops.Add(new SqlParameter("MZ_POLNO", this.polno));
            ops.Add(new SqlParameter("MZ_UNIT", this.polno.Length >= 4 ? this.polno.Substring(0, 4) : ""));
            if (this.grade == "")
                ops.Add(new SqlParameter("GRADE", DBNull.Value));
            else
                ops.Add(new SqlParameter("GRADE", this.grade));
            ops.Add(new SqlParameter("SDA", sda));
            ops.Add(new SqlParameter("EDA", eda));
            ops.Add(new SqlParameter("SMZ_SRANK", ssrank));
            ops.Add(new SqlParameter("EMZ_SRANK", esrank));
            ops.Add(new SqlParameter("SMZ_SPT", sspt));
            ops.Add(new SqlParameter("EMZ_SPT", espt));
            ops.Add(new SqlParameter("ATYPE", atype));
            ops.Add(new SqlParameter("ADAYS", adays));
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
            ops.Add(new SqlParameter("LOCKDB", "N"));
            ops.Add(new SqlParameter("LASTDA", DateTime.Now));
            ops.Add(new SqlParameter("CREATOR", SalaryPublic.strLoginID));

            bool isOK = o_DBFactory.ABC_toTest.Edit_Data(sql, ops);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ops)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ops));
            }
            return isOK;
        }

        /// <summary>
        /// 建立年終獎金資料
        /// </summary>
        /// <param name="ayear"></param>
        /// <param name="payad"></param>
        /// <param name="plus"></param>
        /// <param name="amonth"></param>
        /// <param name="salarypay"></param>
        /// <param name="profess"></param>
        /// <param name="boss"></param>
        /// <param name="bossMonth"></param>
        /// <param name="extra01"></param>
        /// <param name="tax"></param>
        /// <param name="total"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool createYearEnd(string ayear, string payad, double plus, double amonth, int salarypay,
            int profess, int boss, double bossMonth, int extra01, int tax, int total, string note)
        {
            return YearEnd.insertData(id, name, ayear, payad, occc, srank, slvc, spt, polno, "N", plus, amonth, salarypay, profess, boss, bossMonth, extra01, tax, total, note);
        }

        /// <summary>
        /// 建立考績獎金資料
        /// </summary>
        /// <param name="ayear"></param>
        /// <param name="payad"></param>
        /// <param name="plus"></param>
        /// <param name="salarypay"></param>
        /// <param name="profess"></param>
        /// <param name="boss"></param>
        /// <param name="work"></param>
        /// <param name="tech"></param>
        /// <param name="far"></param>
        /// <param name="bossMonth"></param>
        /// <param name="extra01"></param>
        /// <param name="tax"></param>
        /// <param name="total"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool createEffect(string ayear, string payad, double plus, int salarypay,
            int profess, int boss, int work, int tech, int far, int electric, double bossMonth, int extra01, int tax, int total, string note, string grade)
        {
            return Effect.insertData(id, name, ayear, payad, occc, srank, slvc, spt, polno, "N", plus, salarypay, profess, boss, work, tech, far, electric, bossMonth, extra01, tax, total, note, grade);
        }

        /// <summary>
        /// 計算保險費
        /// </summary>
        public int getInsurancePay()
        {
            //這邊利用Salary模組去算就好
            //之所以故意在這邊宣告一個無參數的getInsurancePay()
            //純粹是為了方便,呼叫的時候,省去綁定一堆參數
            return Salary.getInsurancePay(this.policeType, this.isPolice_G, this.spt, this.tempSpt, this.INSURANCE_TYPE);
        }

        /// <summary>
        /// 計算保險費,根據俸點,並排除暫存俸點
        /// </summary>
        /// <param name="inputSPT">俸點</param>
        /// <returns></returns>
        public int getInsurancePay(string inputSPT)
        {
            //這邊利用Salary模組去算就好
            //之所以故意在這邊宣告一個無參數的getInsurancePay()
            //純粹是為了方便,呼叫的時候,省去綁定一堆參數
            return Salary.getInsurancePay(this.policeType, this.isPolice_G, inputSPT, "", this.INSURANCE_TYPE);
        }

        /// <summary>
        /// 計算保險費,根據升遷前後的俸點差異
        /// </summary>
        /// <param name="spt_NEW"></param>
        /// <param name="spt_OLD"></param>
        /// <returns></returns>
        public int getINSURANCEPAY_diff(string spt_NEW, string spt_OLD)
        {
            return getInsurancePay(spt_NEW) - getInsurancePay(spt_OLD);
        }

        /// <summary>
        /// 依員工編號查詢發薪機關在出納現服機關的員警
        /// </summary>
        /// <param name="polno">員工編號</param>
        /// <returns></returns>
        public static DataTable searchInPayadbyPolno(string polno)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> ops = new List<SqlParameter>();
            string sql;

            sql = "SELECT IDCARD, NAME, MZ_POLNO, CHIAD PAY_AD, CHIUNIT, CHIOCCC FROM VW_ALL_POLICE_DATA WHERE MZ_POLNO=@POLNO AND PAY_AD=@PAY_AD";
            ops.Add(new SqlParameter("POLNO", polno));
            ops.Add(new SqlParameter("PAY_AD", SalaryPublic.strLoginEXAD));

            return o_DBFactory.ABC_toTest.DataSelect(sql, ops);
        }

        public static DataTable searchByPolno(string polno)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> ops = new List<SqlParameter>();
            string sql;

            sql = "SELECT IDCARD, NAME, MZ_POLNO, CHIAD PAY_AD, CHIUNIT, CHIOCCC FROM VW_ALL_POLICE_DATA WHERE MZ_POLNO=@POLNO";
            ops.Add(new SqlParameter("POLNO", polno));
            //ops.Add(new SqlParameter("PAY_AD", SalaryPublic.strLoginEXAD));// 用員工編號查詢時不使用登入出納的現服機關為條件 2012.03.15

            return o_DBFactory.ABC_toTest.DataSelect(sql, ops);
        }

        public static DataTable searchByName(string name)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> ops = new List<SqlParameter>();
            string sql;
            ///2012.03.29 EDIT BY KIM OVER 2 RESULT NEED PAY_AD
            sql = "SELECT IDCARD, NAME, MZ_POLNO, CHIAD, CHIUNIT,CHIAD PAY_AD, CHIOCCC FROM VW_ALL_POLICE_DATA WHERE NAME=@NAME";
            ops.Add(new SqlParameter("NAME", name));

            return o_DBFactory.ABC_toTest.DataSelect(sql, ops);
        }

        public static string getPoliceType(string occc)
        {
            switch (occc)
            {
                case "0014":
                case "0015":
                case "0016":
                case "1179":
                case "Z014":
                case "Z015":
                case "Z016":
                    return "2";

                default:
                    return "1";
            }
        }

        public static bool checkIsPolice(string srank)
        {
            if (srank.StartsWith("G") || srank.StartsWith("P"))
                return true;

            return false;
        }

        public static void createData(string idcard, string name, string polno, string add2, string ad, string unit, string exad, string exunit, string payad, string occc, string srank, string slvc, string spt, string tempspt, string fdate, string adate, string chief, string nrea)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();

            sql = @"INSERT INTO A_DLBASE(MZ_ID, MZ_NAME, MZ_POLNO, MZ_ADD2, MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, PAY_AD, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT, MZ_SPT1, MZ_FDATE, MZ_ADATE, MZ_PCHIEF, MZ_NREA) 
                    VALUES(@MZ_ID, @MZ_NAME, @MZ_POLNO, @MZ_ADD2, @MZ_AD, @MZ_UNIT, @MZ_EXAD, @MZ_EXUNIT, @PAY_AD, @MZ_OCCC, @MZ_SRANK, @MZ_SLVC, @MZ_SPT, @MZ_SPT1, @MZ_FDATE, @MZ_ADATE, @MZ_PCHIEF, @MZ_NREA)";
            ps.Add(new SqlParameter("MZ_ID", idcard.ToUpper()));
            ps.Add(new SqlParameter("MZ_NAME", name));
            ps.Add(new SqlParameter("MZ_POLNO", polno));
            ps.Add(new SqlParameter("MZ_ADD2", polno));
            ps.Add(new SqlParameter("MZ_AD", ad));
            ps.Add(new SqlParameter("MZ_UNIT", unit));
            ps.Add(new SqlParameter("MZ_EXAD", exad));
            ps.Add(new SqlParameter("MZ_EXUNIT", exunit));
            ps.Add(new SqlParameter("PAY_AD", payad));
            ps.Add(new SqlParameter("MZ_OCCC", occc));
            ps.Add(new SqlParameter("MZ_SRANK", srank));
            ps.Add(new SqlParameter("MZ_SLVC", slvc));
            ps.Add(new SqlParameter("MZ_SPT", spt));
            ps.Add(new SqlParameter("MZ_SPT1", tempspt));
            ps.Add(new SqlParameter("MZ_FDATE", fdate));
            ps.Add(new SqlParameter("MZ_ADATE", adate));
            ps.Add(new SqlParameter("MZ_PCHIEF", chief));
            ps.Add(new SqlParameter("MZ_NREA", nrea));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);

            if (TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), SalaryPublic.RegixSQL(sql, ps)) == "N")
            {
                TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", SalaryPublic.RegixSQL(sql, ps));
            }
        }

        /// <summary>
        /// 判斷是不是可用的員工編號
        /// </summary>
        /// <param name="polno">員工編號</param>
        /// <param name="payad">發薪機關</param>
        /// <returns></returns>
        public static bool avaliblePolno(string polno, string payad)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();
            int result = 0;

            sql = "SELECT COUNT(*) FROM A_DLBASE WHERE MZ_POLNO=@POLNO AND PAY_AD=@PAYAD";
            ps.Add(new SqlParameter("POLNO", polno));
            ps.Add(new SqlParameter("PAYAD", payad));

            int.TryParse(o_DBFactory.ABC_toTest.GetValue(sql, ps), out result);
            if (result == 0)
                return true;

            return false;
        }

        /// <summary>
        /// 檢查身份證字號正確性
        /// </summary>
        /// <param name="user_id"></param>
        public static bool isValidID(string user_id)
        {
            int[] uid = new int[10];    //數字陣列存放身分證字號用
            int chkTotal;               //計算總和用

            if (user_id.Length == 10)    //檢查長度
            {
                user_id = user_id.ToUpper();    //將身分證字號英文改為大寫

                //將輸入的值存入陣列中
                for (int i = 1; i < user_id.Length; i++)
                {
                    uid[i] = Convert.ToInt32(user_id.Substring(i, 1));
                }
                //將開頭字母轉換為對應的數值
                switch (user_id.Substring(0, 1).ToUpper())
                {
                    case "A": uid[0] = 10; break;
                    case "B": uid[0] = 11; break;
                    case "C": uid[0] = 12; break;
                    case "D": uid[0] = 13; break;
                    case "E": uid[0] = 14; break;
                    case "F": uid[0] = 15; break;
                    case "G": uid[0] = 16; break;
                    case "H": uid[0] = 17; break;
                    case "I": uid[0] = 34; break;
                    case "J": uid[0] = 18; break;
                    case "K": uid[0] = 19; break;
                    case "L": uid[0] = 20; break;
                    case "M": uid[0] = 21; break;
                    case "N": uid[0] = 22; break;
                    case "O": uid[0] = 35; break;
                    case "P": uid[0] = 23; break;
                    case "Q": uid[0] = 24; break;
                    case "R": uid[0] = 25; break;
                    case "S": uid[0] = 26; break;
                    case "T": uid[0] = 27; break;
                    case "U": uid[0] = 28; break;
                    case "V": uid[0] = 29; break;
                    case "W": uid[0] = 32; break;
                    case "X": uid[0] = 30; break;
                    case "Y": uid[0] = 31; break;
                    case "Z": uid[0] = 33; break;
                }
                //檢查第一個數值是否為1.2(判斷性別)
                if (uid[1] == 1 || uid[1] == 2)
                {
                    chkTotal = (uid[0] / 10 * 1) + (uid[0] % 10 * 9);

                    int k = 8;
                    for (int j = 1; j < 9; j++)
                    {
                        chkTotal += uid[j] * k;
                        k--;
                    }

                    chkTotal += uid[9];

                    if (chkTotal % 10 != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }



        #region 沒用到的

        //public void setSrank(string srank)
        //{
        //    this.srank = srank;

        //    this.boss = Salary.getBossPay(this.extpos, this.chief, this.srank);
        //    this.profess = Salary.getProfessPay(this.srank, this.isCrimelab);
        //    setHealthPersonal(Salary.getHealthPersonal(this.salary, this.boss, this.profess, this.workp, this.technic, this.bonus, this.adventive, this.far, this.electric, this.insuranceType));
        //    refreshTotalPay();
        //}

        ///// <summary>
        ///// 設定俸點，順便更新相關薪資金額
        ///// </summary>
        ///// <param name="spt"></param>
        //public void setSpt(string spt)
        //{
        //    this.spt = spt.PadLeft(4, '0');

        //    this.salary = Salary.getSalaryPay(this.policeType, this.isPolice, this.spt, this.tempSpt);
        //    //20140618
        //    this.insurance = Salary.getInsurancePay(this.policeType, this.isPolice, this.spt, this.tempSpt);
        //    //this.insurance = Salary.getInsurancePay(this.policeType, this.isPolice, this.spt, this.is30Years);
        //    setHealthPersonal(Salary.getHealthPersonal(this.salary, this.boss, this.profess, this.workp, this.technic, this.bonus, this.adventive, this.far, this.electric, this.insuranceType));
        //    refreshTotalPay();
        //}

        //public void setChief(string chief)
        //{
        //    this.chief = chief;

        //    this.boss = Salary.getBossPay(this.extposSrank, this.chief, this.srank);
        //    setHealthPersonal(Salary.getHealthPersonal(this.salary, this.boss, this.profess, this.workp, this.technic, this.bonus, this.adventive, this.far, this.electric, this.insuranceType));
        //    refreshTotalPay();
        //}

        //public void setExtposSrank(string extposSrank)
        //{
        //    this.extposSrank = extposSrank;

        //    this.boss = Salary.getBossPay(this.extposSrank, this.chief, this.srank);
        //    setHealthPersonal(Salary.getHealthPersonal(this.salary, this.boss, this.profess, this.workp, this.technic, this.bonus, this.adventive, this.far, this.electric, this.insuranceType));
        //    refreshTotalPay();
        //}

        //public void setHealthPersonal(int healthPersonal)
        //{
        //    this.healthPersonal = healthPersonal;

        //    // 個人健保費有異動時，加保的保費也要更新
        //    foreach (DataRow row in this.healthInfo.Rows)
        //    {
        //        if (int.Parse(row["Percent"].ToString()) > 100)
        //            continue;

        //        row["Cost"] = this.healthPersonal * int.Parse(row["Percent"].ToString());
        //    }

        //    // 計算健保費總額
        //    this.health = this.healthPersonal;
        //    foreach (DataRow row in this.healthInfo.Rows)
        //    {
        //        this.health += int.Parse(row["Cost"].ToString());
        //    }
        //}

        #endregion
    }
}
