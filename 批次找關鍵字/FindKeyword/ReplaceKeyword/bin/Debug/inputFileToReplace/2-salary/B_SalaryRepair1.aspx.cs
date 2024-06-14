using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Drawing;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryRepair1 : System.Web.UI.Page
    {
        string grade
        {
            get { return txt_Grade.Text; }
            set { txt_Grade.Text = value; }
        }
        string batch
        {
            set { txt_batch.Text = value; }
            get { return txt_batch.Text; }
        }
        List<int> searchList
        {
            set { ViewState["searchList"] = value; }
            get { return (List<int>)ViewState["searchList"]; }
        }
        int currentIndex
        {
            set { ViewState["currentIndex"] = value; }
            get { return (int)ViewState["currentIndex"]; }
        }
        Repair repair
        {
            set { ViewState["repair"] = value; }
            get { return (Repair)ViewState["repair"]; }
        }
        Police police
        {
            set { ViewState["police"] = value; }
            get { return (Police)ViewState["police"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserSelector1.UserSelectd += new GridViewCommandEventHandler(UserSelector1_UserSelectd);
            UserSelector1.PageIndexChanging += new GridViewPageEventHandler(UserSelector1_PageIndexChanging);
            SalaryPublic.checkPermission();
            strPayAD = SalaryPublic.strLoginEXAD;

            if (!IsPostBack)
            {
                CustomInit();
                voidClear();
            }
        }

        private void CustomInit()
        {
            searchList = new List<int>();
            strAMONTH = System.DateTime.Now.Year - 1911 + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
            batch = Repair.getCurrentBatch(SalaryPublic.strLoginEXAD, strAMONTH).ToString();

            SalaryPublic.FillSRankDropDownList(ref ddl_SSrank);
            SalaryPublic.FillSRankDropDownList(ref ddl_ESrank);
            SalaryPublic.fillDropDownList(ref ddl_searchPayad);

            TextBox_SDA.Focus();
        }

        // 計算補發金額
        protected void btCount_Click(object sender, EventArgs e)
        {
            // modify by alex chen, 2020/4/8
            float taxpercent = Tax.getTaxPercent();     // 所得稅比例
            int taxstart = Tax.getTaxStart();           // 需扣所得稅起始金額(不含)

            DateTime dateS;
            DateTime dateE;
            try
            {
                dateS = DateTime.Parse(SalaryPublic.ToADDateWithDash(strSDA));
                dateE = DateTime.Parse(SalaryPublic.ToADDateWithDash(strEDA));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + ex.Message + "');", true);
                return;
            }

            intSALARYPAY1 = 0;
            intBOSS = 0;
            intPROFESS = 0;
            intWORKP = 0;
            intTECHNICS = 0;
            intBONUS = 0;
            intADVENTIVE = 0;
            intFAR = 0;
            intELECTRIC = 0;
            intSALARY_2_INCREASE = 0;

            intHEALTHID = 0;
            intHEALTHMAN = 0;
            intHEALTHPAY = 0;
            intHEALTHPAY1 = 0;// 健保費扣款??不知道是幹麻的
            intMONTHPAY_TAX = 0;
            intMONTHPAY = 0;
            intTAX = 0;
            intINSURANCEPAY = 0;
            intCONCUR3PAY = 0;
            intEXTRA01 = 0;
            intEXTRA02 = 0;
            intEXTRA03 = 0;
            intEXTRA04 = 0;
            intEXTRA05 = 0;
            intEXTRA06 = 0;
            intEXTRA07 = 0;
            intEXTRA08 = 0;
            intEXTRA09 = 0;
            intSALARY_2_DECREASE = 0;

            int intTAXCHILD;
            double dblSalaryMonth = 0;
            double dblRepairMonth = 0;
            double dblGovMonth = 0;
            double dblGradeMonth = 0;
            double dblGradeMonth2 = 0;
            DateTime year30Date = SalaryBasic.GetFirstGovDate(strIDCARD).AddYears(30);
            //主管加給
            string strBS = "";
            string strBE = "";
            //判斷是否為鑑識人員
            string strID = TextBox_IDCARD.Text;
            string strCrim = o_DBFactory.ABC_toTest.vExecSQL("SELECT CRIMELAB FROM B_BASE WHERE IDCARD = '" + strID + "'");
            string strPS = "";
            string strPE = "";
            //鑑識人員技術加給
            string strSID = "";
            string strEID = "";
            string strSTID = "";
            string strETID = "";
            int iSMZ_TECH = 0, iEMZ_TECH = 0, iSMZ_SRANK = 0, iEMZ_SRANK = 0;
            //20140701
            if (police == null)
                police = new Police(strIDCARD);

            //string strOCCC = SalaryPublic.strMZ_ID_TO_MZ_OCCC_Data(strIDCARD);
            string strOCCC = SalaryPublic.str_A_Column("MZ_OCCC", 1, strIDCARD);

            int intSalary = Salary.getSalaryPay(police.policeType, police.isPolice_G, strEMZ_SPT, police.tempSpt) - Salary.getSalaryPay(police.policeType, police.isPolice_G, strSMZ_SPT, police.tempSpt);
            int intProfess = Salary.getProfessPay(strEMZ_SRANK, police.isCrimelab, police.AHP_RANK) - Salary.getProfessPay(strSMZ_SRANK, police.isCrimelab, police.AHP_RANK);
            //20140618            
            //計算保險費,根據升遷前後的俸點差異
            int intGov = police.getINSURANCEPAY_diff(strEMZ_SPT, strSMZ_SPT);
                //Salary.getInsurancePay(police.policeType, police.isPolice, strEMZ_SPT, police.tempSpt) - Salary.getInsurancePay(police.policeType, police.isPolice, strSMZ_SPT, police.tempSpt);
            int intConcur3 = Salary.getConcur3(police.policeType, police.isPolice_G, strEMZ_SPT, police.tempSpt) - Salary.getConcur3(police.policeType, police.isPolice_G, strSMZ_SPT, police.tempSpt);

            // 補發期間總天數
            double repairDays = (dateE - dateS).TotalDays;
            // 補發期間以起始日當月第一天到結束日當月最後一天的總天數
            double totalDays = (new DateTime(dateE.Year, dateE.Month, 1).AddMonths(1).AddDays(-1).Date - new DateTime(dateS.Year, dateS.Month, 1)).TotalDays;

            dblSalaryMonth = repairDays / totalDays;//dateE.Month - dateS.Month + ((double)dateE.Day / new DateTime(dateE.Year, dateE.Month, 1).AddMonths(1).AddDays(-1).Day);
            dblRepairMonth = dateE.Month - dateS.Month + 1;

            if (year30Date < dateS)
                dblGovMonth = 0;
            else if (year30Date > dateE)
                dblGovMonth = dblRepairMonth;
            else
            {
                // 補發任公職滿30年的那個月前的公保費(補發整個月)
                dblGovMonth = year30Date.Month - dateS.Month;

                // 當月取任公職滿30年的前一天 除以 當月總天數
                dblGovMonth += (double)(year30Date.Day - 1) / new DateTime(year30Date.Year, year30Date.Month, 1).AddMonths(1).AddDays(-1).Day;
            }
            //鑑識人員的技術加給
            if (strSMZ_SRANK == "G21" || strSMZ_SRANK == "P09")
            { strSID = "01"; }
            else if (strSMZ_SRANK == "G22" || strSMZ_SRANK == "P08")
            { strSID = "02"; }
            else if (strSMZ_SRANK == "G23" || strSMZ_SRANK == "P07")
            { strSID = "03"; }
            else if (strSMZ_SRANK == "G24" || strSMZ_SRANK == "P06")
            { strSID = "04"; }
            else if (strSMZ_SRANK == "G11" || strSMZ_SRANK == "P05")
            { strSID = "05"; }
            else { strSID = "00"; }

            if (strEMZ_SRANK == "G21" || strEMZ_SRANK == "P09")
            { strEID = "01"; }
            else if (strEMZ_SRANK == "G22" || strEMZ_SRANK == "P08")
            { strEID = "02"; }
            else if (strEMZ_SRANK == "G23" || strEMZ_SRANK == "P07")
            { strEID = "03"; }
            else if (strEMZ_SRANK == "G24" || strEMZ_SRANK == "P06")
            { strEID = "04"; }
            else if (strEMZ_SRANK == "G11" || strEMZ_SRANK == "P05")
            { strEID = "05"; }
            else { strEID = "00"; }

            switch (DropDownList_ATYPE.SelectedValue) // 補發型態
            {
                case "1":// 新進

                    //應發
                    intSALARYPAY1 = Repair.countRepair(strSDA, strEDA, police.salary);
                    intBOSS = Repair.countRepair(strSDA, strEDA, police.boss);
                    intPROFESS = Repair.countRepair(strSDA, strEDA, police.profess);
                    intWORKP = Repair.countRepair(strSDA, strEDA, police.workp);
                    intTECHNICS = Repair.countRepair(strSDA, strEDA, police.technic);
                    intBONUS = Repair.countRepair(strSDA, strEDA, police.bonus);
                    intADVENTIVE = Repair.countRepair(strSDA, strEDA, police.adventive);
                    intFAR = Repair.countRepair(strSDA, strEDA, police.far);
                    intELECTRIC = Repair.countRepair(strSDA, strEDA, police.electric);
                    intSALARY_2_INCREASE = Repair.countRepair(strSDA, strEDA, police.otheradd);

                    intHEALTHPAY = police.health * ((int)Math.Ceiling((dateE - dateS).TotalDays / 30));//健保費補整個月(看跨了幾個月)
                    intHEALTHPAY1 = 0;

                    intINSURANCEPAY = police.insurance * ((int)Math.Ceiling((dateE - dateS).TotalDays / 30));// 公(勞)保費和退撫金費補整個月(看跨了幾個月)
                    intCONCUR3PAY = police.concur3 * ((int)Math.Ceiling((dateE - dateS).TotalDays / 30));

                    intMONTHPAY_TAX = Repair.countRepair(strSDA, strEDA, police.salaryCostTax);
                    intMONTHPAY = Repair.countRepair(strSDA, strEDA, police.salaryCost);
                    intTAXCHILD = police.taxChild;
                    intTAX = police.tax;
                    intSALARY_2_DECREASE = Repair.countRepair(strSDA, strEDA, police.otherminus);

                    break;

                case "2":// 調入，不用扣退撫金

                    //應發
                    intSALARYPAY1 = Repair.countRepair(strSDA, strEDA, police.salary);
                    intBOSS = Repair.countRepair(strSDA, strEDA, police.boss);
                    intPROFESS = Repair.countRepair(strSDA, strEDA, police.profess);
                    intWORKP = Repair.countRepair(strSDA, strEDA, police.workp);
                    intTECHNICS = Repair.countRepair(strSDA, strEDA, police.technic);
                    intBONUS = Repair.countRepair(strSDA, strEDA, police.bonus);
                    intADVENTIVE = Repair.countRepair(strSDA, strEDA, police.adventive);
                    intFAR = Repair.countRepair(strSDA, strEDA, police.far);
                    intELECTRIC = Repair.countRepair(strSDA, strEDA, police.electric);
                    intSALARY_2_INCREASE = Repair.countRepair(strSDA, strEDA, police.otheradd);

                    intHEALTHPAY = police.health * ((int)Math.Ceiling((dateE - dateS).TotalDays / 30));//健保費補整個月(看跨了幾個月)
                    intHEALTHPAY1 = 0;

                    //如果是補發一整個月，公保、勞保也要補1個月的錢
                    intINSURANCEPAY = 0;
                    intCONCUR3PAY = 0;
                    if ((dateE - dateS).TotalDays + 1 == DateTime.DaysInMonth(dateS.Year, dateS.Month))
                    {
                        intINSURANCEPAY = police.insurance * ((int)Math.Ceiling((dateE - dateS).TotalDays / 30));// 公(勞)保費和退撫金費補整個月(看跨了幾個月)
                        intCONCUR3PAY = police.concur3 * ((int)Math.Ceiling((dateE - dateS).TotalDays / 30));
                    }

                    intMONTHPAY_TAX = Repair.countRepair(strSDA, strEDA, police.salaryCostTax);
                    intMONTHPAY = Repair.countRepair(strSDA, strEDA, police.salaryCost);
                    intTAXCHILD = police.taxChild;
                    intTAX = police.tax;
                    intSALARY_2_DECREASE = Repair.countRepair(strSDA, strEDA, police.otherminus);
                    break;

                case "3":// 薪級變更、其他不用算到考績等級，補差額就好
                    //薪俸職等若有變更，需計算專業加給與技術加給
                    int tmpPro = 0, tmpTech = 0, iSMZ_SPT = 0, iEMZ_SPT = 0, tmpSalary = 0;
                    int iSins_SPT = 0, iEins_SPT = 0, tmpIns = 0, iScon_SPT = 0, iEcon_SPT = 0, tmpCon = 0;
                    string strORGIN = "";

                    if (strSMZ_SRANK != strEMZ_SRANK)
                    {
                        //專業加給，判斷是否為鑑識人員
                        if (strCrim == "否")
                        {
                            iSMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'"));
                            iEMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'"));
                            tmpPro = iEMZ_SRANK - iSMZ_SRANK;
                            intPROFESS = Repair.countRepair(strSDA, strEDA, tmpPro);
                        }
                        else
                        {
                            //專業加給
                            strPS = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'");
                            strPE = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'");
                            if (strPS != "") { iSMZ_SRANK = Convert.ToInt32(strPS); }
                            if (strPE != "") { iEMZ_SRANK = Convert.ToInt32(strPE); }
                            tmpPro = iEMZ_SRANK - iSMZ_SRANK;
                            intPROFESS = Repair.countRepair(strSDA, strEDA, tmpPro);
                            //技術加給
                            strSTID = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_TECHNICS WHERE ID ='" + strSID + "'");
                            strETID = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_TECHNICS WHERE ID ='" + strEID + "'");
                            if (strSTID != "") { iSMZ_TECH = Convert.ToInt32(strSTID); }
                            if (strETID != "") { iEMZ_TECH = Convert.ToInt32(strETID); }
                            tmpTech = iEMZ_TECH - iSMZ_TECH;
                            intTECHNICS = Repair.countRepair(strSDA, strEDA, tmpTech);


                        }
                    }
                    //P為公務人員：俸點；G為警察：俸級
                    if (police.srank.Substring(0, 1) == "P")
                    { strORGIN = "ORIGIN1"; }
                    else { strORGIN = "ORIGIN2"; };

                    iSMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strSMZ_SPT + "'"));
                    iEMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strEMZ_SPT + "'"));
                    tmpSalary = iEMZ_SPT - iSMZ_SPT;
                    intSALARYPAY1 = Repair.countRepair(strSDA, strEDA, tmpSalary);
                    //公保、退撫和月支一樣算法
                    tmpIns = police.getINSURANCEPAY_diff(strEMZ_SPT, strSMZ_SPT);
                    intINSURANCEPAY = Salary.round(Repair.countRepair(strSDA, strEDA, tmpIns));

                    iScon_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT CONCUR3 FROM B_SALARY WHERE " + strORGIN + "='" + strSMZ_SPT + "'"));
                    iEcon_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT CONCUR3 FROM B_SALARY WHERE " + strORGIN + "='" + strEMZ_SPT + "'"));
                    tmpCon = iEcon_SPT - iScon_SPT;
                    intCONCUR3PAY = Salary.round(Repair.countRepair(strSDA, strEDA, tmpCon));



                    //intPROFESS = Repair.countRepair(strSDA, strEDA, Salary.getProfessPay(strEMZ_SRANK, police.isCrimelab, police.AHP_RANK) - Salary.getProfessPay(strSMZ_SRANK, police.isCrimelab, police.AHP_RANK));
                    //應扣
                    //intINSURANCEPAY = Salary.round(intGov * dblGovMonth);
                    //intCONCUR3PAY = Repair.countRepair(strSDA, strEDA, intConcur3);
                    // modify by alex, 2020/4/8, 重新計算公(勞)保費, 因此增加此項 (上面的應扣,關連到初次任職公職,與此無關,所以重新計算)
                    //intINSURANCEPAY = Salary.round(Repair.countRepair(strSDA, strEDA, intGov));

                    break;

                case "4": // 考績晉級不用算警勤加給
                    if (grade == "甲")
                        dblGradeMonth = 1;
                    else if (grade == "乙")
                        dblGradeMonth = 0.5;

                    if (police.srank.Substring(0, 1) == "P") { strORGIN = "ORIGIN1"; }
                    else { strORGIN = "ORIGIN2"; };

                    iSMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strSMZ_SPT + "'"));
                    iEMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strEMZ_SPT + "'"));

                    intSALARYPAY1 = iEMZ_SPT - iSMZ_SPT;
                    intPROFESS = Salary.round(Repair.countRepair(strSDA, strEDA, intProfess) + (intProfess * dblGradeMonth));

                    intINSURANCEPAY = Repair.countRepair(strSDA, strEDA, intGov);
                    intCONCUR3PAY = Repair.countRepair(strSDA, strEDA, intConcur3);
                    break;

                case "5":
                    if (grade == "甲")
                        dblGradeMonth = 1;
                    else if (grade == "乙")
                        dblGradeMonth = 0.5;

                    intSALARYPAY1 = Salary.round(Repair.countRepair(strSDA, strEDA, intSalary) + (intSalary * dblGradeMonth));

                    //專業加給
                    iSMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'"));
                    iEMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'"));
                    intPROFESS = iEMZ_SRANK - iSMZ_SRANK;

                    //技術加給：若本身有技術加給才照專業加給計算，若沒有則不用計算
                    if (police.technic > 0)
                    { intTECHNICS = iEMZ_SRANK - iSMZ_SRANK; }

                    intINSURANCEPAY = Repair.countRepair(strSDA, strEDA, intGov);
                    intCONCUR3PAY = Repair.countRepair(strSDA, strEDA, intConcur3);
                    break;

                case "6"://考績晉階晉級除了同4,5的處理外，如果職等、俸點都沒有變動的情況直接補發1個月薪水(應發總合)，如果金額超過68500(不含)要扣5%所得稅
                         // modify by alex chen, 2020/4/8, 扣所得稅起始金額, 從資料庫取得
                         //發放月數對本薪、考績對差額，兩者要分開計算後相加
                    string strmon = "";
                    int iSMZ_Boss = 0, iEMZ_Boss = 0, tmpBoss = 0;
                    if (strSMZ_SRANK == strEMZ_SRANK && strSMZ_SPT == strEMZ_SPT)
                    {   //薪俸職等跟俸點未變動，判斷發放月數

                        if (rbmon1.Checked)
                        { dblGradeMonth = 1; strmon = "補一個月"; }
                        else if (rbmon2.Checked)
                        { dblGradeMonth = 0.5; strmon = "補半個月"; }
                        else if (rbmon3.Checked)
                        { dblGradeMonth = 1.5; strmon = "補一個半月"; }
                        else { dblGradeMonth = 0; strmon = "毋須補"; }

                        TextBox_NOTE.Text = strmon + "考績獎金";
                        intSALARYPAY1 = Salary.round(police.salary * dblGradeMonth);
                        intBOSS = Salary.round(police.boss * dblGradeMonth);
                        intPROFESS = Salary.round(police.profess * dblGradeMonth);
                        intWORKP = Salary.round(police.workp * dblGradeMonth);
                        intTECHNICS = Salary.round(police.technic * dblGradeMonth);
                        intBONUS = Salary.round(police.bonus * dblGradeMonth);
                        intADVENTIVE = Salary.round(police.adventive * dblGradeMonth);
                        intFAR = Salary.round(police.far * dblGradeMonth);
                        intELECTRIC = Salary.round(police.electric * dblGradeMonth);
                    }
                    else
                    {
                        TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + ddl_SSrank.SelectedItem.Text + " ~ " + ddl_ESrank.SelectedItem.Text + " , " + TextBox_SMZ_SPT.Text.Trim() + " ~ " + TextBox_EMZ_SPT.Text.Trim() + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "差額)";
                        //P為公務人員：俸點；G為警察：俸級
                        if (police.srank.Substring(0, 1) == "P")
                        { strORGIN = "ORIGIN1"; }
                        else
                        { strORGIN = "ORIGIN2"; }

                        if (rbmon4.Checked)
                        {
                            if (grade == "甲") { dblGradeMonth2 = 1; }
                            else { dblGradeMonth2 = 0.5; }
                            //月支數額
                            iSMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strSMZ_SPT + "'"));
                            iEMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strEMZ_SPT + "'"));
                            tmpSalary = iEMZ_SPT - iSMZ_SPT;
                            intSALARYPAY1 = Salary.round(tmpSalary * dblGradeMonth2);

                            //專業加給(要判斷是否為鑑識人員)
                            if (strCrim == "否")
                            {
                                iSMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'"));
                                iEMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'"));
                                tmpPro = iEMZ_SRANK - iSMZ_SRANK;
                                intPROFESS = Salary.round(tmpPro * dblGradeMonth2);
                            }
                            else
                            {
                                strPS = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'");
                                strPE = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'");
                                if (strPS != "") { iSMZ_SRANK = Convert.ToInt32(strPS); }
                                if (strPE != "") { iEMZ_SRANK = Convert.ToInt32(strPE); }
                                tmpPro = iEMZ_SRANK - iSMZ_SRANK;
                                intPROFESS = Salary.round(tmpPro * dblGradeMonth2);

                                //技術加給
                                strSTID = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_TECHNICS WHERE ID ='" + strSID + "'");
                                strETID = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_TECHNICS WHERE ID ='" + strEID + "'");
                                if (strSTID != "") { iSMZ_TECH = Convert.ToInt32(strSTID); }
                                if (strETID != "") { iEMZ_TECH = Convert.ToInt32(strETID); }
                                tmpTech = iEMZ_TECH - iSMZ_TECH;
                                intTECHNICS = Salary.round(tmpTech * dblGradeMonth2);
                            }

                            //主管加給
                            if (police.boss != 0)
                            {
                                strBS = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_BOSS WHERE ID Like'" + strSMZ_SRANK + "%'");
                                strBE = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_BOSS WHERE ID Like'" + strEMZ_SRANK + "%'");
                                if (strBS != "") { iSMZ_Boss = Convert.ToInt32(strBS); }
                                if (strBE != "") { iEMZ_Boss = Convert.ToInt32(strBE); }
                                tmpBoss = iEMZ_Boss - iSMZ_Boss;
                                intBOSS = Salary.round(tmpBoss * dblGradeMonth2);
                            }
                        }
                        else
                        {   //發放月數對本薪
                            if (rbmon1.Checked)
                            { dblGradeMonth = 1; }
                            else if (rbmon2.Checked)
                            { dblGradeMonth = 0.5; }
                            else if (rbmon3.Checked)
                            { dblGradeMonth = 1.5; }
                            //考績對差額
                            if (grade == "甲") { dblGradeMonth2 = 1; }
                            else { dblGradeMonth2 = 0.5; }
                            //月支數額
                            iSMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strSMZ_SPT + "'"));
                            iEMZ_SPT = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE " + strORGIN + "='" + strEMZ_SPT + "'"));
                            tmpSalary = iEMZ_SPT - iSMZ_SPT;
                            intSALARYPAY1 = Salary.round(iEMZ_SPT * dblGradeMonth + tmpSalary * dblGradeMonth2);
                            //intSALARYPAY1 = Salary.round(Repair.countRepair(strSDA, strEDA, intSalary) + (intSalary * dblGradeMonth));
                            //專業加給，判斷是否為鑑識人員
                            if (strCrim == "否")
                            {
                                iSMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'"));
                                iEMZ_SRANK = Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'"));
                                tmpPro = iEMZ_SRANK - iSMZ_SRANK;
                                intPROFESS = Salary.round(iEMZ_SRANK * dblGradeMonth + tmpPro * dblGradeMonth2);
                                //intPROFESS = Salary.round(Repair.countRepair(strSDA, strEDA, intProfess) + (intProfess * dblGradeMonth));
                            }
                            else
                            {
                                strPS = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strSMZ_SRANK + "%'");
                                strPE = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE ID Like'" + strEMZ_SRANK + "%'");
                                if (strPS != "") { iSMZ_SRANK = Convert.ToInt32(strPS); }
                                if (strPE != "") { iEMZ_SRANK = Convert.ToInt32(strPE); }
                                tmpPro = iEMZ_SRANK - iSMZ_SRANK;
                                intPROFESS = Salary.round(iEMZ_SRANK * dblGradeMonth + tmpPro * dblGradeMonth2);
                                //技術加給
                                strSTID = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_TECHNICS WHERE ID ='" + strSID + "'");
                                strETID = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_TECHNICS WHERE ID ='" + strEID + "'");
                                if (strSTID != "") { iSMZ_TECH = Convert.ToInt32(strSTID); }
                                if (strETID != "") { iEMZ_TECH = Convert.ToInt32(strETID); }
                                tmpTech = iEMZ_TECH - iSMZ_TECH;
                                intTECHNICS = Salary.round(iEMZ_TECH * dblGradeMonth + tmpTech * dblGradeMonth2);
                            }

                            //主管加給
                            if (police.boss != 0)
                            {
                                strBS = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_BOSS WHERE ID Like'" + strSMZ_SRANK + "%'");
                                strBE = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_BOSS WHERE ID Like'" + strEMZ_SRANK + "%'");
                                if (strBS != "") { iSMZ_Boss = Convert.ToInt32(strBS); }
                                if (strBE != "") { iEMZ_Boss = Convert.ToInt32(strBE); }
                                tmpBoss = iEMZ_Boss - iSMZ_Boss;
                                intBOSS = Salary.round(iEMZ_Boss * dblGradeMonth + tmpBoss * dblGradeMonth2);
                            }

                            //勤務繁重加給
                            intELECTRIC = Salary.round(police.electric * dblGradeMonth);
                            //警勤加給
                            intWORKP = Salary.round(police.workp * dblGradeMonth);
                            //工作獎金對照表
                            intBONUS = Salary.round(police.bonus * dblGradeMonth);
                            //外事加給
                            intADVENTIVE = Salary.round(police.adventive * dblGradeMonth);
                            //偏遠加給
                            intFAR = Salary.round(police.far * dblGradeMonth);

                        }

                        //應扣
                        //intINSURANCEPAY = Salary.round(Repair.countRepair(strSDA, strEDA, intGov));
                        //intCONCUR3PAY = Salary.round(Repair.countRepair(strSDA, strEDA, intConcur3));
                    }

                    //總額扣除主管加給再判斷是否要扣所得稅
                    int total = intSALARYPAY1 + intPROFESS + intWORKP + intTECHNICS + intBONUS + intADVENTIVE + intFAR + intELECTRIC;
                    //if (total > 84501)
                    //    intTAX = Salary.round(((double)total * 0.05));

                    // modify by alex chen, 2020/4/8
                    // 增加一個月獎金, 金額較大就可能會達到要扣稅
                    // 扣所得稅起始金額, 從資料庫取得  (舊格式扣稅起點, 被固定住68500元)
                    if (total > taxstart) { intTAX = Salary.round((double)total * taxpercent); }

                    break;

                case "7":
                    intSALARYPAY1 = Repair.countRepair(strSDA, strEDA, Salary.getSalaryPay(police.policeType, police.isPolice_G, strEMZ_SPT, police.tempSpt) - Salary.getSalaryPay(police.policeType, police.isPolice_G, strSMZ_SPT, police.tempSpt));
                    intPROFESS = Repair.countRepair(strSDA, strEDA, Salary.getProfessPay(strEMZ_SRANK, police.isCrimelab, police.AHP_RANK) - Salary.getProfessPay(strSMZ_SRANK, police.isCrimelab, police.AHP_RANK));

                    //應扣
                    intINSURANCEPAY = Salary.round(intGov * dblGovMonth);
                    intCONCUR3PAY = Repair.countRepair(strSDA, strEDA, intConcur3);

                    // modify by alex, 2020/4/8, 重新計算公(勞)保費, 因此增加此項 (上面的應扣,關連到初次任職公職,與此無關,所以重新計算)
                    intINSURANCEPAY = Salary.round(Repair.countRepair(strSDA, strEDA, intGov));

                    break;

                case "8":
                    intSALARYPAY1 = Convert.ToInt32(Math.Round(police.salary * 0.5, 0, MidpointRounding.AwayFromZero));   //月支數額
                    intBOSS = Convert.ToInt32(Math.Round(police.boss * 0.5, 0, MidpointRounding.AwayFromZero));           //主管加給
                    intPROFESS = Convert.ToInt32(Math.Round(police.profess * 0.5, 0, MidpointRounding.AwayFromZero));    //專業加給
                    intWORKP = Convert.ToInt32(Math.Round(police.workp * 0.5, 0, MidpointRounding.AwayFromZero));       //警勤加給
                    intTECHNICS = Convert.ToInt32(Math.Round(police.technic * 0.5, 0, MidpointRounding.AwayFromZero));  //技術加給
                    intELECTRIC = Convert.ToInt32(Math.Round(police.electric * 0.5, 0, MidpointRounding.AwayFromZero)); //勤務繁重加給
                    intFAR = Convert.ToInt32(Math.Round(police.far * 0.5, 0, MidpointRounding.AwayFromZero));           //偏遠加給

                    break;
            }


            switch (DropDownList_ATYPE.SelectedValue)
            {
                case "1":
                case "2":
                    TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "薪資)";
                    break;
                case "3":
                    if (strSMZ_SRANK != strEMZ_SRANK)
                    {
                        TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + TextBox_SMZ_SPT.Text.Trim() + " ~ " + TextBox_EMZ_SPT.Text.Trim() + "、" + ddl_SSrank.SelectedItem.Text + " ~ " + ddl_ESrank.SelectedItem.Text + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "差額)";
                    }
                    else
                    {
                        TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + TextBox_SMZ_SPT.Text.Trim() + " ~ " + TextBox_EMZ_SPT.Text.Trim() + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "差額)";
                    }
                    break;
                case "4":
                    TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + TextBox_SMZ_SPT.Text.Trim() + " ~ " + TextBox_EMZ_SPT.Text.Trim() + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "及考績獎金差額)";
                    break;
                case "5":
                    TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + ddl_SSrank.SelectedItem.Text + " ~ " + ddl_ESrank.SelectedItem.Text + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "差額)";
                    break;
                case "7":
                    TextBox_NOTE.Text = DropDownList_ATYPE.SelectedItem.Text.Substring(2) + ddl_SSrank.SelectedItem.Text + " ~ " + ddl_ESrank.SelectedItem.Text + " , " + TextBox_SMZ_SPT.Text.Trim() + " ~ " + TextBox_EMZ_SPT.Text.Trim() + "(" + TextBox_SDA.Text + "至" + TextBox_EDA.Text + "差額)";
                    break;
                case "8":
                    TextBox_NOTE.Text = "";
                    break;
            }

            voidViewTatol();
        }

        // 新增資料
        protected void btCreate_Click(object sender, EventArgs e)
        {
            if (batch.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入批號')", true);
                return;
            }

            int bat;
            int.TryParse(batch, out bat);
            if (Repair.isLockDB(SalaryPublic.strLoginEXAD, strAMONTH, bat))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('此批已關帳，不能再新增')", true);
                return;
            }

            bool isOK = police.createRepair(SalaryPublic.strLoginEXAD, strAMONTH, batch, strSDA, strEDA, strSMZ_SRANK, strEMZ_SRANK, strSMZ_SPT, strEMZ_SPT, int.Parse(DropDownList_ATYPE.SelectedValue), (DateTime.Parse(SalaryPublic.ToADDateWithDash(strEDA)) - DateTime.Parse(SalaryPublic.ToADDateWithDash(strSDA))).Days, intSALARYPAY1, intWORKP, intPROFESS, intBOSS, intTECHNICS, intBONUS, intADVENTIVE, intFAR, intELECTRIC, intSALARY_2_INCREASE, intINSURANCEPAY, intHEALTHID, intHEALTHMAN, intHEALTHPAY, intHEALTHPAY1, intMONTHPAY_TAX, intMONTHPAY, intCONCUR3PAY, intTAX, intSALARY_2_DECREASE, intEXTRA01, intEXTRA02, intEXTRA03, intEXTRA04, intEXTRA05, intEXTRA06, intEXTRA07, intEXTRA08, intEXTRA09, strNOTE);
            if (isOK)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增成功')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增失敗')", true);
                return;
            }


            voidClear();
            TextBox_MZ_POLNO.Focus();
        }

        #region 變數

        string strPayAD;

        private string strAMONTH
        {
            get
            {
                return txt_AMonth.Text;
            }
            set
            {
                txt_AMonth.Text = value;
            }
        }

        private string strIDCARD
        {
            get
            {
                return TextBox_IDCARD.Text.ToUpper();
            }
            set
            {
                TextBox_IDCARD.Text = value;
            }
        }

        private string strMZ_NAME
        {
            get
            {
                return TextBox_MZ_NAME.Text;
            }
            set
            {
                TextBox_MZ_NAME.Text = value;
            }
        }

        private string strMZ_POLNO
        {
            get
            {
                return TextBox_MZ_POLNO.Text;
            }
            set
            {
                TextBox_MZ_POLNO.Text = value;
            }
        }

        private static string _strMZ_OCCC;
        private string strMZ_OCCC
        {
            get
            {
                return _strMZ_OCCC;
            }
            set
            {
                _strMZ_OCCC = value;
            }
        }

        private static string _strMZ_SRANK;
        private string strMZ_SRANK
        {
            get
            {
                return _strMZ_SRANK;
            }
            set
            {
                _strMZ_SRANK = value;
            }
        }

        private static string _strMZ_SLVC;
        private string strMZ_SLVC
        {
            get
            {
                return _strMZ_SLVC;
            }
            set
            {
                _strMZ_SLVC = value;
            }
        }

        private static string _strMZ_SPT;
        private string strMZ_SPT
        {
            get
            {
                return _strMZ_SPT;
            }
            set
            {
                _strMZ_SPT = value;
            }
        }

        private static string _strMZ_UNIT;
        private string strMZ_UNIT
        {
            get
            {
                return _strMZ_UNIT;
            }
            set
            {
                _strMZ_UNIT = value;
            }
        }

        private static string _strLOCKDB;
        private string strLOCKDB
        {
            get
            {
                return _strLOCKDB;
            }
            set
            {
                _strLOCKDB = value;
            }
        }

        //必要基本資料存取 --- E

        private string strSDA
        {
            get
            {
                return TextBox_SDA.Text;
            }
            set
            {
                TextBox_SDA.Text = value;
            }
        }

        private string strEDA
        {
            get
            {
                return TextBox_EDA.Text;
            }
            set
            {
                TextBox_EDA.Text = value;
            }
        }

        private string strSMZ_SPT
        {
            get
            {
                return TextBox_SMZ_SPT.Text;
            }
            set
            {
                TextBox_SMZ_SPT.Text = value;
            }
        }

        private string strEMZ_SPT
        {
            get
            {
                return TextBox_EMZ_SPT.Text;
            }
            set
            {
                TextBox_EMZ_SPT.Text = value;
            }
        }

        private string strSMZ_SRANK
        {
            get
            {
                return ddl_SSrank.SelectedValue;
                //return TextBox_SMZ_SRANK.Text;
            }
            set
            {
                ddl_SSrank.SelectedValue = value;
                //TextBox_SMZ_SRANK.Text = value;
            }
        }

        private string strEMZ_SRANK
        {
            get
            {
                return ddl_ESrank.SelectedValue;
                //return TextBox_EMZ_SRANK.Text;
            }
            set
            {
                ddl_ESrank.SelectedValue = value;
                //TextBox_EMZ_SRANK.Text = value;
            }
        }

        private int intINCREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(lb_paySum.Text);
            }
            set
            {
                lb_paySum.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intDECREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(lb_costSum.Text);
            }
            set
            {
                lb_costSum.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intTPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(lb_realSum.Text);
            }
            set
            {
                lb_realSum.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private string strNOTE
        {
            get
            {
                return TextBox_NOTE.Text;
            }
            set
            {
                TextBox_NOTE.Text = value;
            }
        }

        //以下應發
        private int intSALARYPAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SALARYPAY1.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_SALARYPAY1.Text = "";
                else
                    TextBox_SALARYPAY1.Text = value.ToString();
            }
        }

        private int intBOSS
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_BOSS.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_BOSS.Text = "";
                else
                    TextBox_BOSS.Text = value.ToString();
            }
        }

        private int intPROFESS
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PROFESS.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_PROFESS.Text = "";
                else
                    TextBox_PROFESS.Text = value.ToString();
            }
        }

        private int intWORKP
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_WORKP.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_WORKP.Text = "";
                else
                    TextBox_WORKP.Text = value.ToString();
            }
        }

        private int intTECHNICS
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TECHNICS.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_TECHNICS.Text = "";
                else
                    TextBox_TECHNICS.Text = value.ToString();
            }
        }

        private int intBONUS
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_BONUS.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_BONUS.Text = "";
                else
                    TextBox_BONUS.Text = value.ToString();
            }
        }

        private int intADVENTIVE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_ADVENTIVE.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_ADVENTIVE.Text = "";
                else
                    TextBox_ADVENTIVE.Text = value.ToString();
            }
        }

        private int intFAR
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_FAR.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_FAR.Text = "";
                else
                    TextBox_FAR.Text = value.ToString();
            }
        }

        private int intELECTRIC
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_ELECTRIC.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_ELECTRIC.Text = "";
                else
                    TextBox_ELECTRIC.Text = value.ToString();
            }
        }

        private int intSALARY_2_INCREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SALARY_2_INCREASE.Text);
            }
            set
            {
                TextBox_SALARY_2_INCREASE.Text = value.ToString();
            }
        }

        //以下應扣
        private int intHEALTHID
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALTHID.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_HEALTHID.Text = "";
                else
                    TextBox_HEALTHID.Text = value.ToString();
            }
        }

        private int intHEALTHMAN
        {
            get
            {
                return int.Parse(TextBox_HEALTHMAN.Text);
            }
            set
            {
                TextBox_HEALTHMAN.Text = value.ToString();
            }
        }

        private int intHEALTHPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALTHPAY.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_HEALTHPAY.Text = "";
                else
                    TextBox_HEALTHPAY.Text = value.ToString();
            }
        }

        private int intHEALTHPAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALTHPAY1.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_HEALTHPAY1.Text = "";
                else
                    TextBox_HEALTHPAY1.Text = value.ToString();
            }
        }

        private int intMONTHPAY_TAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_MONTHPAY_TAX.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_MONTHPAY_TAX.Text = "";
                else
                    TextBox_MONTHPAY_TAX.Text = value.ToString();
            }
        }

        private int intMONTHPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_MONTHPAY.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_MONTHPAY.Text = "";
                else
                    TextBox_MONTHPAY.Text = value.ToString();
            }
        }

        private int intTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TAX.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_TAX.Text = "";
                else
                    TextBox_TAX.Text = value.ToString();
            }
        }

        private int intINSURANCEPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_INSURANCEPAY.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_INSURANCEPAY.Text = "";
                else
                    TextBox_INSURANCEPAY.Text = value.ToString();
            }
        }

        private int intCONCUR3PAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_CONCUR3PAY.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_CONCUR3PAY.Text = "";
                else
                    TextBox_CONCUR3PAY.Text = value.ToString();
            }
        }

        private int intSALARY_2_DECREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SALARY_2_DECREASE.Text);
            }
            set
            {
                TextBox_SALARY_2_DECREASE.Text = value.ToString();
            }
        }

        //以下其他應扣
        private int intEXTRA01
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA01.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA01.Text = "";
                else
                    TextBox_EXTRA01.Text = value.ToString();
            }
        }

        private int intEXTRA02
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA02.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA02.Text = "";
                else
                    TextBox_EXTRA02.Text = value.ToString();
            }
        }

        private int intEXTRA03
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA03.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA03.Text = "";
                else
                    TextBox_EXTRA03.Text = value.ToString();
            }
        }

        private int intEXTRA04
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA04.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA04.Text = "";
                else
                    TextBox_EXTRA04.Text = value.ToString();
            }
        }

        private int intEXTRA05
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA05.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA05.Text = "";
                else
                    TextBox_EXTRA05.Text = value.ToString();
            }
        }

        private int intEXTRA06
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA06.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA06.Text = "";
                else
                    TextBox_EXTRA06.Text = value.ToString();
            }
        }

        private int intEXTRA07
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA07.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA07.Text = "";
                else
                    TextBox_EXTRA07.Text = value.ToString();
            }
        }

        private int intEXTRA08
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA08.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA08.Text = "";
                else
                    TextBox_EXTRA08.Text = value.ToString();
            }
        }

        private int intEXTRA09
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA09.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_EXTRA09.Text = "";
                else
                    TextBox_EXTRA09.Text = value.ToString();
            }
        }

        #endregion

        #region 用員工編號、身份證字號、姓名查詢的事件

        protected void TextBox_IDCARD_TextChanged(object sender, EventArgs e)
        {
            if (strIDCARD.Length == 0)
                return;

            police = new Police(strIDCARD);

            if (police.id == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('該身份字號查無資料')", true);
                return;
            }

            voidSearch();
        }

        protected void TextBox_MZ_NAME_TextChanged(object sender, EventArgs e)
        {
            if (strMZ_NAME.Length == 0)
                return;

            DataTable dt = Police.searchByName(strMZ_NAME);

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('該姓名查無資料')", true);
                return;
            }
            if (dt.Rows.Count > 1)
            {
                UserSelector1.SetData(dt);
                pl_Selector_ModalPopupExtender.Show();
                return;
            }

            police = new Police(dt.Rows[0]["IDCARD"].ToString());

            voidSearch();
        }

        protected void TextBox_MZ_POLNO_TextChanged(object sender, EventArgs e)
        {
            if (strMZ_POLNO.Length == 0)
                return;

            //先查詢機關內的員警，找不到再跨機關查詢
            DataTable dt = Police.searchInPayadbyPolno(strMZ_POLNO);
            if (dt.Rows.Count > 1)
            {
                UserSelector1.SetData(dt);
                pl_Selector_ModalPopupExtender.Show();
                return;
            }
            if (dt.Rows.Count == 1)
            {
                police = new Police(dt.Rows[0]["IDCARD"].ToString());
                voidSearch();
                return;
            }

            //這邊開始代表機關內找不到符合的員警，要跨機關查詢了
            dt = Police.searchByPolno(strMZ_POLNO);
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('該員工編號查無資料')", true);
                return;
            }
            if (dt.Rows.Count > 1)
            {
                UserSelector1.SetData(dt);
                pl_Selector_ModalPopupExtender.Show();
                return;
            }

            police = new Police(dt.Rows[0]["IDCARD"].ToString());

            voidSearch();
        }

        void UserSelector1_UserSelectd(object sender, GridViewCommandEventArgs e)
        {
            police = new Police(e.CommandArgument.ToString());
            voidSearch();
        }

        void UserSelector1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            pl_Selector_ModalPopupExtender.Show();
        }

        private void voidSearch()
        {
            strIDCARD = police.id;
            strMZ_POLNO = police.polno;
            strMZ_NAME = police.name;
            strSMZ_SRANK = police.srank;
            strEMZ_SRANK = police.srank;
            strSMZ_SPT = police.spt;
            strEMZ_SPT = police.spt;
            strMZ_OCCC = police.occc;
            strMZ_SRANK = police.srank;
            strMZ_SLVC = police.slvc;
            strMZ_SPT = police.spt;
            strMZ_UNIT = police.unit;
            grade = police.grade;
        }

        #endregion

        #region 切換應發、應扣、其他應扣版面

        protected void btIN_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btDE_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void btOtherDE_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        #endregion

        #region ButtonClick

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            repair.update(strMZ_POLNO, strSDA, strEDA, strSMZ_SRANK, strEMZ_SRANK, strSMZ_SPT, strEMZ_SPT, int.Parse(DropDownList_ATYPE.SelectedValue), intSALARYPAY1, intWORKP, intPROFESS, intBOSS, intTECHNICS, intBONUS, intADVENTIVE, intFAR, intELECTRIC, intSALARY_2_INCREASE, intINSURANCEPAY, intHEALTHID, intHEALTHMAN, intHEALTHPAY, intHEALTHPAY1, intMONTHPAY_TAX, intMONTHPAY, intCONCUR3PAY, intTAX, intSALARY_2_DECREASE, intEXTRA01, intEXTRA02, intEXTRA03, intEXTRA04, intEXTRA05, intEXTRA06, intEXTRA07, intEXTRA08, intEXTRA09, strNOTE);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改成功')", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            repair.delete();

            searchList.RemoveAt(currentIndex);

            if (searchList.Count == 0)// 刪除後如果沒資料了，回到初始狀態
            {
                voidClear();
            }
            else if (currentIndex >= searchList.Count)// 刪除資料後，如果現在的索引超出清單範圍，取清單的最後一筆資料
                currentIndex = searchList.Count - 1;

            ShowData();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功')", true);
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            voidClear();
        }

        protected void btBack_Data_Click(object sender, EventArgs e)
        {
            currentIndex--;
            ShowData();
        }

        protected void btNext_Data_Click(object sender, EventArgs e)
        {
            currentIndex++;
            ShowData();
        }

        #endregion

        #region TextChanged

        protected void TextBox_WORKP_TextChanged(object sender, EventArgs e)
        {
            if (intWORKP == 0)
            {
                TextBox_WORKP.Enabled = true;
                TextBox_TECHNICS.Enabled = true;
                intTECHNICS = 0;
            }
            else
            {
                TextBox_TECHNICS.Enabled = false;
            }

            voidChanged(sender, e);
        }

        protected void TextBox_TECHNICS_TextChanged(object sender, EventArgs e)
        {
            if (intTECHNICS == 0)
            {
                TextBox_TECHNICS.Enabled = true;
                TextBox_WORKP.Enabled = true;
            }
            else
            {
                TextBox_WORKP.Enabled = false;
            }

            voidChanged(sender, e);
        }

        #endregion

        protected void voidChanged(object sender, EventArgs e)
        {
            switch ((sender as TextBox).ID)
            {
                case "TextBox_WORKP":
                    intWORKP = SalaryPublic.intdelimiterChars(TextBox_WORKP.Text);
                    break;
                case "TextBox_TECHNICS":
                    intTECHNICS = SalaryPublic.intdelimiterChars(TextBox_TECHNICS.Text);
                    break;
            }

            voidViewTatol();
        }

        //應扣金額有改變時，計算應扣總計
        protected void costChanged(object sender, EventArgs e)
        {
            voidViewTatol();
        }

        private void voidViewTatol()
        {
            //應發
            intINCREASE = intSALARYPAY1 + intBOSS + intPROFESS + intWORKP + intTECHNICS + intBONUS + intADVENTIVE + intFAR + intELECTRIC +
                intSALARY_2_INCREASE;
            //應扣及其他應扣
            intDECREASE = intHEALTHPAY + intHEALTHPAY1 + intMONTHPAY_TAX + intMONTHPAY + intTAX + intINSURANCEPAY + intCONCUR3PAY +
                intSALARY_2_DECREASE + intEXTRA01 + intEXTRA02 + intEXTRA03 + intEXTRA04 + intEXTRA05 + intEXTRA06 + intEXTRA07 + intEXTRA08 + intEXTRA09;
            //實發
            intTPAY = intINCREASE - intDECREASE;
        }

        private void voidSelectControl(object ojNext)
        {
            if (ojNext is Button)
            {
                ((Button)ojNext).Focus();
            }
            else if (ojNext is TextBox)
            {
                ((TextBox)ojNext).Focus();
            }
            else if (ojNext is DropDownList)
            {
                ((DropDownList)ojNext).Focus();
            }
            else if (ojNext is RadioButtonList)
            {
                ((RadioButtonList)ojNext).Focus();
            }
        }

        #region 查詢補發薪資資料

        // 查詢
        protected void btn_searchConfirm_Click(object sender, EventArgs e)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            sql = "SELECT ROWNUM-1 RowIndex, R_SNID, AMONTH, BATCH_NUMBER, MZ_POLNO, MZ_NAME FROM B_REPAIRPAY WHERE PAY_AD=@PAY_AD ";
            ops.Add(new SqlParameter("PAY_AD", ddl_searchPayad.SelectedValue));

            if (txt_searchAmonth.Text.Length > 0)
            {
                sql += " AND AMONTH=@AMONTH";
                ops.Add(new SqlParameter("AMONTH", txt_searchAmonth.Text));
            }
            if (txt_searchBatch.Text.Length > 0)
            {
                sql += " AND BATCH_NUMBER=@BATCH_NUMBER";
                ops.Add(new SqlParameter("BATCH_NUMBER", txt_searchBatch.Text));
            }
            if (txt_searchName.Text.Length > 0)
            {
                sql += " AND MZ_NAME LIKE @MZ_NAME";
                ops.Add(new SqlParameter("MZ_NAME", "%" + txt_searchName.Text + "%"));
            }
            if (txt_searchPolno.Text.Length > 0)
            {
                sql += " AND MZ_POLNO LIKE @MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", "%" + txt_searchPolno.Text + "%"));
            }
            if (txt_searchId.Text.Length > 0)
            {
                sql += " AND IDCARD=@IDCARD";
                ops.Add(new SqlParameter("IDCARD", txt_searchId.Text));
            }

            sql += " ORDER BY AMONTH, BATCH_NUMBER, MZ_POLNO";

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
                return;
            }
            gv_Target.DataSource = dt;
            gv_Target.DataBind();

            searchList.Clear();
            foreach (DataRow row in dt.Rows)
            {
                searchList.Add(int.Parse(row["R_SNID"].ToString()));
            }

            currentIndex = 0;
            ShowData();
        }

        protected void btn_Selector_Click(object sender, EventArgs e)
        {
            btn_searchResult_ModalPopupExtender.Show();
        }

        protected void gv_Target_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            currentIndex = int.Parse(e.CommandArgument.ToString());
            ShowData();
        }

        // 取得當前查詢的repair資料
        protected void getRepair(int sn)
        {
            repair = new Repair(sn);

            batch = repair.batch.ToString();
            strAMONTH = repair.amonth;
            strIDCARD = repair.idcard;
            strMZ_NAME = repair.name;
            strMZ_POLNO = repair.polno;
            strSDA = repair.sda;
            strEDA = repair.eda;
            DropDownList_ATYPE.SelectedValue = repair.atype.ToString();
            strSMZ_SRANK = repair.ssrank;
            strEMZ_SRANK = repair.esrank;
            strSMZ_SPT = repair.sspt;
            strEMZ_SPT = repair.espt;

            intSALARYPAY1 = repair.salary;
            intBOSS = repair.boss;
            intPROFESS = repair.profess;
            intWORKP = repair.work;
            intTECHNICS = repair.tech;
            intBONUS = repair.bonus;
            intADVENTIVE = repair.adventive;
            intFAR = repair.far;
            intELECTRIC = repair.electric;
            intSALARY_2_INCREASE = repair.otheradd;

            intINSURANCEPAY = repair.insurance;
            intHEALTHID = repair.healthPersonal;
            intHEALTHMAN = repair.healthMan;
            intHEALTHPAY = repair.health;
            intHEALTHPAY1 = repair.healthpay1;
            intMONTHPAY_TAX = repair.monthpayTax;
            intMONTHPAY = repair.monthpay;
            intCONCUR3PAY = repair.concur3;
            intTAX = repair.tax;
            intSALARY_2_DECREASE = repair.otherminus;

            intEXTRA01 = repair.extra01;
            intEXTRA02 = repair.extra02;
            intEXTRA03 = repair.extra03;
            intEXTRA04 = repair.extra04;
            intEXTRA05 = repair.extra05;
            intEXTRA06 = repair.extra06;
            intEXTRA07 = repair.extra07;
            intEXTRA08 = repair.extra08;
            intEXTRA09 = repair.extra09;

            strNOTE = repair.note;
            grade = repair.grade;

            voidViewTatol();

            // 關帳就不能修改囉
            if (repair.lockdb)
            {
                btCreate.Enabled = false;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btExit.Enabled = true;
                lb_islock.Visible = true;
            }
            else
            {
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btExit.Enabled = true;
                lb_islock.Visible = false;
            }
        }

        // 呈現資料
        private void ShowData()
        {
            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;

            if (searchList.Count == 0)
            {
                Label_Pages.Visible = false;
                voidClear();
                return;
            }

            if (searchList.Count > 1)
            {
                // 不在第一筆的時候
                if (currentIndex != 0)
                    btBack_Data.Enabled = true;

                // 不在最後一筆的時候
                if (currentIndex != searchList.Count - 1)
                    btNext_Data.Enabled = true;
            }

            btExit.Visible = true;
            btExit.Enabled = true;

            Label_Pages.Visible = true;
            Label_Pages.Text = "第" + (currentIndex + 1).ToString() + "筆；共" + searchList.Count.ToString() + "筆";

            getRepair(searchList[currentIndex]);
        }

        #endregion

        //還原預設值
        private void voidClear()
        {
            MultiView1.ActiveViewIndex = 0;

            searchList.Clear();
            currentIndex = 0;
            police = null;

            Label_Pages.Visible = true;
            Label_Pages.Text = "第0筆；共0筆";

            btCreate.Enabled = true;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btExit.Enabled = false;
            btBack_Data.Enabled = false;
            btNext_Data.Enabled = false;

            //以下基本設定
            strIDCARD = "";
            strMZ_NAME = "";
            strMZ_POLNO = "";
            ddl_SSrank.SelectedIndex = 0;
            ddl_ESrank.SelectedIndex = 0;
            intINCREASE = 0;
            intDECREASE = 0;
            intTPAY = 0;
            strNOTE = "";
            //以下應發
            intSALARYPAY1 = 0;
            intBOSS = 0;
            intPROFESS = 0;
            intWORKP = 0;
            intTECHNICS = 0;
            intBONUS = 0;
            intADVENTIVE = 0;
            intFAR = 0;
            intELECTRIC = 0;
            intSALARY_2_INCREASE = 0;
            //以下應扣
            intHEALTHID = 0;
            intHEALTHMAN = 0;
            intHEALTHPAY = 0;
            intHEALTHPAY1 = 0;
            intMONTHPAY_TAX = 0;
            intMONTHPAY = 0;
            intTAX = 0;
            intINSURANCEPAY = 0;
            intCONCUR3PAY = 0;
            intSALARY_2_DECREASE = 0;
            //以下其他應扣
            intEXTRA01 = 0;
            intEXTRA02 = 0;
            intEXTRA03 = 0;
            intEXTRA04 = 0;
            intEXTRA05 = 0;
            intEXTRA06 = 0;
            intEXTRA07 = 0;
            intEXTRA08 = 0;
            intEXTRA09 = 0;
        }

        protected void DropDownList_ATYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 依照補發形態限制"薪俸職等"、"俸點"控制項
            switch (DropDownList_ATYPE.SelectedValue)
            {
                case "1":
                case "2":
                case "8":
                    rbmon1.Enabled = false;
                    rbmon2.Enabled = false;
                    rbmon3.Enabled = false;
                    rbmon4.Enabled = false;
                    ddl_SSrank.Enabled = false;
                    ddl_ESrank.Enabled = false;
                    TextBox_SMZ_SPT.Enabled = false;
                    TextBox_EMZ_SPT.Enabled = false;
                    break;
                case "4":
                    rbmon1.Enabled = false;
                    rbmon2.Enabled = false;
                    rbmon3.Enabled = false;
                    rbmon4.Enabled = false;
                    ddl_SSrank.Enabled = false;
                    ddl_ESrank.Enabled = false;
                    TextBox_SMZ_SPT.Enabled = true;
                    TextBox_EMZ_SPT.Enabled = true;
                    break;
                case "5":
                    rbmon1.Enabled = false;
                    rbmon2.Enabled = false;
                    rbmon3.Enabled = false;
                    rbmon4.Enabled = false;
                    ddl_SSrank.Enabled = true;
                    ddl_ESrank.Enabled = true;
                    TextBox_SMZ_SPT.Enabled = false;
                    TextBox_EMZ_SPT.Enabled = false;
                    break;
                case "3":
                    rbmon1.Enabled = false;
                    rbmon2.Enabled = false;
                    rbmon3.Enabled = false;
                    rbmon4.Enabled = false;
                    ddl_SSrank.Enabled = true;
                    ddl_ESrank.Enabled = true;
                    TextBox_SMZ_SPT.Enabled = true;
                    TextBox_EMZ_SPT.Enabled = true;
                    break;
                case "6":
                case "7":
                    rbmon1.Enabled = true;
                    rbmon2.Enabled = true;
                    rbmon3.Enabled = true;
                    rbmon4.Enabled = true;
                    ddl_SSrank.Enabled = true;
                    ddl_ESrank.Enabled = true;
                    TextBox_SMZ_SPT.Enabled = true;
                    TextBox_EMZ_SPT.Enabled = true;
                    break;
            }
        }

        //年月改了要重算批號
        //protected void txt_AMonth_TextChanged(object sender, EventArgs e)
        //{
        //    batch = Repair.getCurrentBatch(SalaryPublic.strLoginEXAD, strAMONTH).ToString();
        //}
    }
}
