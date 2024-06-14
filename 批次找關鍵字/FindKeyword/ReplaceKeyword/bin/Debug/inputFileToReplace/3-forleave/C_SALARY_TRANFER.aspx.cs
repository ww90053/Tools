using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_SALARY_TRANFER : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //matthew 先註解掉 中和分局合併多寫了一個ChangeDropDownList_AD()的方法
                //DropDownList_AD.DataBind();
                //DropDownList_AD.SelectedValue = Session["ADPMZ_AD"].ToString();
                chk_TPMGroup();
            }

        }

        protected void chk_TPMGroup()
        {
            switch (TPPDDB._2_salary.SalaryPublic.GetGroupPermission())
            {
                case"TPMIDISAdmin":
                    ChangeDropDownList_AD();
                    break;
                case "A":
                    ChangeDropDownList_AD();
                    break;
                case "B":
                    ChangeDropDownList_AD();
                    break;
                case "C":
                    //matthew 中和分局下拉要多中和一&中和二
                    ChangeDropDownList_AD();
                    if (Session["ADPMZ_AD"].ToString() == "382133600C")
                    {
                        DropDownList_AD.Enabled = true;
                    }
                    else
                    {
                        DropDownList_AD.Enabled = false;
                    }
                    //DropDownList_AD.Enabled = false;
                    break;
                case "D":
                    ChangeDropDownList_AD();
                    break;
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        //matthew 中和分局
        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_AD"].ToString() == "382133600C")
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_AD.DataSource = dt;
            DropDownList_AD.DataTextField = "MZ_KCHI";
            DropDownList_AD.DataValueField = "MZ_KCODE";
            DropDownList_AD.DataBind();

            DropDownList_AD.SelectedValue = Session["ADPMZ_AD"].ToString();

        }

        protected void bt_TRANFER_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            //15不休假獎金用
            string strSQL1 = "";

            string TODAY = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            switch (RadioButtonList1.SelectedValue)
            {
                case "1":
                    strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM C_DUTYMONTHOVERTIME_HOUR WHERE " +
                                             "MZ_AD='" + DropDownList_AD.SelectedValue +
                                        (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                                         " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                       "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() +
                                       "' AND MZ_KIND='1'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);

                    strSQL = "SELECT MZ_YEAR,MZ_MONTH,C_DUTYMONTHOVERTIME_HOUR.MZ_ID,MZ_OVERTIME_PAY,'1','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', C_DUTYMONTHOVERTIME_HOUR.MZ_UNIT ,C_DUTYMONTHOVERTIME_HOUR.MZ_AD FROM C_DUTYMONTHOVERTIME_HOUR  WHERE C_DUTYMONTHOVERTIME_HOUR.MZ_AD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                       " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                      "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() + "'" +
                                      " AND NVL(MZ_VERIFY, 'N') = 'Y'";
                    break;
                case "2":
                    strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                                           "PAY_AD='" + DropDownList_AD.SelectedValue +
                                     (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                                      " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                     "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() +
                                     "' AND MZ_KIND='2'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);


                    strSQL = "SELECT dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,0,3),dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2),A_DLBASE.MZ_ID,SUM(PAY_SUM),'2','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_EXUNIT FROM C_OVERTIME_HOUR_INSIDE LEFT JOIN A_DLBASE ON C_OVERTIME_HOUR_INSIDE.MZ_ID=A_DLBASE.MZ_ID WHERE C_OVERTIME_HOUR_INSIDE.MZ_EXAD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                       " AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,0,3)='" + TextBox_MZ_YEAR.Text.Trim() +
                                      "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2)='" + TextBox_MZ_MONTH.Text.Trim() +
                                      "' GROUP BY A_DLBASE.MZ_ID,dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,0,3),dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2), A_DLBASE.MZ_EXUNIT ";
                    break;
                case "3":

                    strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                                         "PAY_AD='" + DropDownList_AD.SelectedValue +
                                   (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                                    " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                   "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() +
                                   "' AND MZ_KIND='3'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);


                    strSQL = "SELECT MZ_YEAR,MZ_MONTH,A_DLBASE.MZ_ID,MZ_TOTAL_PAY,'1','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT FROM C_ONDUTY_HOUR LEFT JOIN A_DLBASE ON C_ONDUTY_HOUR.MZ_ID=A_DLBASE.MZ_ID WHERE A_DLBASE.PAY_AD='" + DropDownList_AD.SelectedValue +
                         (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                       " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                      "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() + "'";
                    break;
                case "4":
                    
                    //strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                    //                     "PAY_AD='" + DropDownList_AD.SelectedValue +
                    //             (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                    //                " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                    //               "' AND MZ_KIND='4'";

                    //strSQL1 = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                    //                     "PAY_AD='" + DropDownList_AD.SelectedValue +
                    //             (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                    //                " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                    //               "' AND MZ_KIND='6'";

                    strSQL = "DELETE FROM C_SALARY WHERE PAD='" + DropDownList_AD.SelectedValue +
                                 (DropDownList_UNIT.SelectedValue == "" ? "'" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                    " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                   "' AND MZ_KIND='4'";

                    strSQL1 = "DELETE FROM C_SALARY WHERE PAD='" + DropDownList_AD.SelectedValue +
                                 (DropDownList_UNIT.SelectedValue == "" ? "'" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                    " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                   "' AND MZ_KIND='6'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                    o_DBFactory.ABC_toTest.Edit_Data(strSQL1);

                    //strSQL = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_NOPLAYDAY_TOTAL,'4','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,'" + Session["ADPMZ_EXAD"].ToString() + "' PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                    //    (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                    //                 " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

                    //strSQL1 = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_ODAYPAY_TOTAL,'6','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,'" + Session["ADPMZ_EXAD"].ToString() +  "' PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                    //    (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") + " AND  MZ_ODAYPAY_TOTAL > 0" +
                    //                 " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

                    strSQL = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_NOPLAYDAY_TOTAL,'4','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,C_NOPLAYPAY.PAY_AD PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                     " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

                    strSQL1 = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_ODAYPAY_TOTAL,'6','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,C_NOPLAYPAY.PAY_AD PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") + " AND  MZ_ODAYPAY_TOTAL > 0" +
                                     " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";
                    break;
            }

            string insertString = "INSERT INTO C_SALARY(MZ_YEAR,MZ_MONTH,MZ_ID,AMOUNT,MZ_KIND,FLAG_ID,FLAG_TIME, MZ_UNIT,PAD) " + strSQL;
            //15不休假獎金用
            string insertString15 = "INSERT INTO C_SALARY(MZ_YEAR,MZ_MONTH,MZ_ID,AMOUNT,MZ_KIND,FLAG_ID,FLAG_TIME, MZ_UNIT,PAD) " + strSQL1;

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                String Message = (dt.Rows.Count <= 0) ? "執行完成，無任何資料轉檔" : "轉檔成功";

                o_DBFactory.ABC_toTest.Edit_Data(insertString);
                if (RadioButtonList1.SelectedValue == "4")
                    o_DBFactory.ABC_toTest.Edit_Data(insertString15);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + Message + "');", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('轉檔失敗');", true);
            }
        }

        protected void bt_TRANFER_Click1(object sender, EventArgs e)
        {
            string strSQL = "";
            //15不休假獎金用
            string strSQL1 = "";

            string TODAY = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            switch (RadioButtonList1.SelectedValue)
            {
                case "1":
                    strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM C_DUTYMONTHOVERTIME_HOUR WHERE " +
                                             "MZ_AD='" + DropDownList_AD.SelectedValue +
                                        (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                                         " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                       "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() +
                                       "' AND MZ_KIND='1'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);

                    strSQL = "SELECT MZ_YEAR,MZ_MONTH,C_DUTYMONTHOVERTIME_HOUR.MZ_ID,MZ_OVERTIME_PAY,'1','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', C_DUTYMONTHOVERTIME_HOUR.MZ_UNIT ,C_DUTYMONTHOVERTIME_HOUR.MZ_AD FROM C_DUTYMONTHOVERTIME_HOUR  WHERE C_DUTYMONTHOVERTIME_HOUR.MZ_AD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                       " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                      "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() + "'" +
                                      " AND NVL(MZ_VERIFY, 'N') = 'Y'";
                    break;
                case "2":
                    strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                                           "PAY_AD='" + DropDownList_AD.SelectedValue +
                                     (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                                      " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                     "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() +
                                     "' AND MZ_KIND='2'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);


                    strSQL = "SELECT dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,0,3),dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2),A_DLBASE.MZ_ID,SUM(PAY_SUM),'2','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_EXUNIT FROM C_OVERTIME_HOUR_INSIDE LEFT JOIN A_DLBASE ON C_OVERTIME_HOUR_INSIDE.MZ_ID=A_DLBASE.MZ_ID WHERE C_OVERTIME_HOUR_INSIDE.MZ_EXAD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                       " AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,0,3)='" + TextBox_MZ_YEAR.Text.Trim() +
                                      "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2)='" + TextBox_MZ_MONTH.Text.Trim() +
                                      "' GROUP BY A_DLBASE.MZ_ID,dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,0,3),dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2), A_DLBASE.MZ_EXUNIT ";
                    break;
                case "3":

                    strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                                         "PAY_AD='" + DropDownList_AD.SelectedValue +
                                   (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                                    " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                   "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() +
                                   "' AND MZ_KIND='3'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);


                    strSQL = "SELECT MZ_YEAR,MZ_MONTH,A_DLBASE.MZ_ID,MZ_TOTAL_PAY,'1','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT FROM C_ONDUTY_HOUR LEFT JOIN A_DLBASE ON C_ONDUTY_HOUR.MZ_ID=A_DLBASE.MZ_ID WHERE A_DLBASE.PAY_AD='" + DropDownList_AD.SelectedValue +
                         (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                       " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                      "' AND MZ_MONTH='" + TextBox_MZ_MONTH.Text.Trim() + "'";
                    break;
                case "4":

                    //strSQL = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                    //                     "PAY_AD='" + DropDownList_AD.SelectedValue +
                    //             (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                    //                " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                    //               "' AND MZ_KIND='4'";

                    //strSQL1 = "DELETE FROM C_SALARY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE " +
                    //                     "PAY_AD='" + DropDownList_AD.SelectedValue +
                    //             (DropDownList_UNIT.SelectedValue == "" ? "')" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "')") +
                    //                " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                    //               "' AND MZ_KIND='6'";

                    strSQL = "DELETE FROM C_SALARY WHERE PAD='" + DropDownList_AD.SelectedValue +
                                 (DropDownList_UNIT.SelectedValue == "" ? "'" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                    " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                   "' AND MZ_KIND='4'";

                    strSQL1 = "DELETE FROM C_SALARY WHERE PAD='" + DropDownList_AD.SelectedValue +
                                 (DropDownList_UNIT.SelectedValue == "" ? "'" : "' AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                    " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() +
                                   "' AND MZ_KIND='6'";

                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                    o_DBFactory.ABC_toTest.Edit_Data(strSQL1);

                    //strSQL = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_NOPLAYDAY_TOTAL,'4','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,'" + Session["ADPMZ_EXAD"].ToString() + "' PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                    //    (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                    //                 " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

                    //strSQL1 = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_ODAYPAY_TOTAL,'6','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,'" + Session["ADPMZ_EXAD"].ToString() +  "' PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                    //    (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") + " AND  MZ_ODAYPAY_TOTAL > 0" +
                    //                 " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

                    strSQL = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_NOPLAYDAY_TOTAL,'4','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,C_NOPLAYPAY.PAY_AD PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") +
                                     " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

                    strSQL1 = "SELECT MZ_YEAR,'  ',A_DLBASE.MZ_ID,MZ_ODAYPAY_TOTAL,'6','" + Session["ADPMZ_ID"].ToString() + "','" + TODAY + "', A_DLBASE.MZ_UNIT,C_NOPLAYPAY.PAY_AD PAD FROM C_NOPLAYPAY LEFT JOIN A_DLBASE ON C_NOPLAYPAY.MZ_ID=A_DLBASE.MZ_ID WHERE C_NOPLAYPAY.PAY_AD='" + DropDownList_AD.SelectedValue +
                        (DropDownList_UNIT.SelectedValue == "" ? "' " : "' AND A_DLBASE.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'") + " AND  MZ_ODAYPAY_TOTAL > 0" +
                                     " AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";
                    break;
            }

            string insertString = "INSERT INTO C_SALARY(MZ_YEAR,MZ_MONTH,MZ_ID,AMOUNT,MZ_KIND,FLAG_ID,FLAG_TIME, MZ_UNIT,PAD) " + strSQL;
            //15不休假獎金用
            string insertString15 = "INSERT INTO C_SALARY(MZ_YEAR,MZ_MONTH,MZ_ID,AMOUNT,MZ_KIND,FLAG_ID,FLAG_TIME, MZ_UNIT,PAD) " + strSQL1;

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                String Message = (dt.Rows.Count <= 0) ? "執行完成，無任何資料轉檔" : "轉檔成功";

                o_DBFactory.ABC_toTest.Edit_Data(insertString);
                if (RadioButtonList1.SelectedValue == "4")
                    o_DBFactory.ABC_toTest.Edit_Data(insertString15);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + Message + "');", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('轉檔失敗');", true);
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "4")
            {
                TextBox_MZ_MONTH.Text = string.Empty;
                TextBox_MZ_MONTH.Enabled = false;
            }
            else
            {
                TextBox_MZ_MONTH.Enabled = true;
            }
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem("", ""));
        }
    }
}
