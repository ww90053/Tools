using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Drawing;

using TPPDDB.App_Code;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYTABLE_SET : System.Web.UI.Page
    {
        List<String> DUTYTABLE_SN = new List<string>();

        string SqlField;

        string SqlField1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            }

            ViewState["DUTYMODE"] = Request["DUTYMODE"];
            ViewState["DUTYDATE"] = Request["DUTYDATE"];



            if (!IsPostBack)
            {
                //MQ ---------------------20100331   
                C.set_Panel_EnterToTAB(ref this.Panel3);
                C.set_Panel_EnterToTAB(ref this.Panel1);

                Label27.Text = string.Format(@"{0}勤務分配表輸入", o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_A_DLBASE.PMZUNIT(Session["ADPMZ_EXUNIT"].ToString()) + "'"));
                //預帶明天日期
                DateTime tomorrow = DateTime.Now.AddDays(1);
                TextBox_DUTYDATE.Text = o_CommonService.Personal_ReturnDateString((tomorrow.Year - 1911).ToString().PadLeft(3, '0') + tomorrow.Month.ToString().PadLeft(2, '0') + tomorrow.Day.ToString().PadLeft(2, '0'));


                if (ViewState["XCOUNT"] != null)//刪除或修改資料時！紀錄reload的時候該抓哪筆資料
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));

                    xcount.Text = ViewState["XCOUNT"].ToString();
                    //判斷上筆下筆按鍵是否可按！
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && DUTYTABLE_SN.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < DUTYTABLE_SN.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == DUTYTABLE_SN.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (DUTYTABLE_SN.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label28.Visible = true;
                        Label28.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DUTYTABLE_SN.Count.ToString() + "筆";
                    }


                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btDelete.Enabled = true;
                }

                if (ViewState["DUTYMODE"] != null)
                {
                    string strSQL = "SELECT MZ_DUTYTABLE_SN FROM C_DUTYTABLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND  1=1";

                    if (ViewState["DUTYMODE"].ToString() != "")
                    {
                        strSQL += " AND DUTY_NO='" + ViewState["DUTYMODE"].ToString() + "'";
                    }
                    if (ViewState["DUTYDATE"].ToString() != "")
                    {
                        strSQL += " AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'";
                    }

                    strSQL += " ORDER BY MZ_DUTYTABLE_SN";

                    DUTYTABLE_SN = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_DUTYTABLE_SN");
                    Session["MZ_DUTYTABLE_SN"] = DUTYTABLE_SN;

                    if (DUTYTABLE_SN.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('C_ForLeaveOvertime_DUTYTABLE_SET.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (DUTYTABLE_SN.Count == 1)
                    {

                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));

                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));

                    }
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00001", TPMPermissions._boolTPM(), strSQL) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00001", strSQL);
                    }

                    //共幾筆資料
                    if (DUTYTABLE_SN.Count == 0)
                    {
                        Label28.Visible = false;
                    }
                    else
                    {
                        Label28.Visible = true;
                        Label28.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DUTYTABLE_SN.Count.ToString() + "筆";
                    }
                }
            }
        }

        protected string SQLSPLIT(string S)
        {
            string result = "";
            string[] SS = S.Split('.');

            for (int i = 0; i < SS.Length; i++)
            {
                if (i == SS.Length - 1)
                {
                    result += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNO FROM C_DUTYPEOPLE WHERE MZ_ID='" + SS[i].ToString() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'");
                }
                else
                {
                    result += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNO FROM C_DUTYPEOPLE WHERE MZ_ID='" + SS[i].ToString() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'") + '.';
                }
            }

            return result;
        }

        protected string PNOSPLIT(string S)
        {
            string result = "";
            string[] SS = S.Split('.');

            for (int i = 0; i < SS.Length; i++)
            {
                if (i == SS.Length - 1)
                {
                    result += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNO FROM C_DUTYPEOPLE WHERE MZ_PNO='" + SS[i].ToString() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'");
                }
                else
                {
                    result += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNO FROM C_DUTYPEOPLE WHERE MZ_PNO='" + SS[i].ToString() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'") + '.';
                }
            }

            return result;
        }

        protected void finddata(int DataCount)
        {
            DUTYTABLE_SN = Session["MZ_DUTYTABLE_SN"] as List<string>;

            string strSQL = "SELECT * FROM C_DUTYTABLE WHERE MZ_DUTYTABLE_SN='" + DUTYTABLE_SN[DataCount] + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (temp.Rows.Count == 1)
            {
                switch (temp.Rows[0]["TIME1"].ToString())
                {
                    case "06-07":
                        RadioButtonList1.SelectedValue = "6";
                        break;
                    case "07-08":
                        RadioButtonList1.SelectedValue = "7";
                        break;
                    case "08-09":
                        RadioButtonList1.SelectedValue = "8";
                        break;
                    case "09-10":
                        RadioButtonList1.SelectedValue = "9";
                        break;
                }

                TextBox_DUTYNO.Text = temp.Rows[0]["DUTY_NO"].ToString();
                TextBox_DUTYNO1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO='" + temp.Rows[0]["DUTY_NO"].ToString() + "'");

                TextBox_DUTYMODE.Text = temp.Rows[0]["DUTYMODE_NO"].ToString();
                TextBox_DUTYMODE_NO_MEMO.Text = temp.Rows[0]["DUTYMODE_NO_MEMO"].ToString();
                TextBox_DUTYDATE.Text = temp.Rows[0]["DUTYDATE"].ToString();
                TextBox_MZ_DUTYTARGET_NO.Text = temp.Rows[0]["MZ_DUTYTARGET_NO"].ToString();
                //TextBox_MZ_DUTYTARGET_NO1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DUTYTARGET FROM C_DUTYTARGET WHERE MZ_DUTYTARGET_NO='" + temp.Rows[0]["MZ_DUTYTARGET_NO"].ToString() + "'");
                TextBox_MZ_DUTYPATROL_NO.Text = temp.Rows[0]["MZ_DUTYPATROL_NO"].ToString();
                //TextBox_MZ_DUTYPATROL_NO1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DUTYPATROL FROM C_DUTYPATROL WHERE MZ_DUTYPATROL_NO='" + temp.Rows[0]["MZ_DUTYPATROL_NO"].ToString() + "'");

                Label1.Text = temp.Rows[0]["TIME1"].ToString();
                TextBox_MZ_PNO1.Text = SQLSPLIT(temp.Rows[0]["PEOPLE1"].ToString());
                Label2.Text = temp.Rows[0]["TIME2"].ToString();
                TextBox_MZ_PNO2.Text = SQLSPLIT(temp.Rows[0]["PEOPLE2"].ToString());
                Label3.Text = temp.Rows[0]["TIME3"].ToString();
                TextBox_MZ_PNO3.Text = SQLSPLIT(temp.Rows[0]["PEOPLE3"].ToString());
                Label4.Text = temp.Rows[0]["TIME4"].ToString();
                TextBox_MZ_PNO4.Text = SQLSPLIT(temp.Rows[0]["PEOPLE4"].ToString());
                Label5.Text = temp.Rows[0]["TIME5"].ToString();
                TextBox_MZ_PNO5.Text = SQLSPLIT(temp.Rows[0]["PEOPLE5"].ToString());
                Label6.Text = temp.Rows[0]["TIME6"].ToString();
                TextBox_MZ_PNO6.Text = SQLSPLIT(temp.Rows[0]["PEOPLE6"].ToString());
                Label7.Text = temp.Rows[0]["TIME7"].ToString();
                TextBox_MZ_PNO7.Text = SQLSPLIT(temp.Rows[0]["PEOPLE7"].ToString());
                Label8.Text = temp.Rows[0]["TIME8"].ToString();
                TextBox_MZ_PNO8.Text = SQLSPLIT(temp.Rows[0]["PEOPLE8"].ToString());
                Label9.Text = temp.Rows[0]["TIME9"].ToString();
                TextBox_MZ_PNO9.Text = SQLSPLIT(temp.Rows[0]["PEOPLE9"].ToString());
                Label10.Text = temp.Rows[0]["TIME10"].ToString();
                TextBox_MZ_PNO10.Text = SQLSPLIT(temp.Rows[0]["PEOPLE10"].ToString());
                Label11.Text = temp.Rows[0]["TIME11"].ToString();
                TextBox_MZ_PNO11.Text = SQLSPLIT(temp.Rows[0]["PEOPLE11"].ToString());
                Label12.Text = temp.Rows[0]["TIME12"].ToString();
                TextBox_MZ_PNO12.Text = SQLSPLIT(temp.Rows[0]["PEOPLE12"].ToString());
                Label13.Text = temp.Rows[0]["TIME13"].ToString();
                TextBox_MZ_PNO13.Text = SQLSPLIT(temp.Rows[0]["PEOPLE13"].ToString());
                Label14.Text = temp.Rows[0]["TIME14"].ToString();
                TextBox_MZ_PNO14.Text = SQLSPLIT(temp.Rows[0]["PEOPLE14"].ToString());
                Label15.Text = temp.Rows[0]["TIME15"].ToString();
                TextBox_MZ_PNO15.Text = SQLSPLIT(temp.Rows[0]["PEOPLE15"].ToString());
                Label16.Text = temp.Rows[0]["TIME16"].ToString();
                TextBox_MZ_PNO16.Text = SQLSPLIT(temp.Rows[0]["PEOPLE16"].ToString());
                Label17.Text = temp.Rows[0]["TIME17"].ToString();
                TextBox_MZ_PNO17.Text = SQLSPLIT(temp.Rows[0]["PEOPLE17"].ToString());
                Label18.Text = temp.Rows[0]["TIME18"].ToString();
                TextBox_MZ_PNO18.Text = SQLSPLIT(temp.Rows[0]["PEOPLE18"].ToString());
                Label19.Text = temp.Rows[0]["TIME19"].ToString();
                TextBox_MZ_PNO19.Text = SQLSPLIT(temp.Rows[0]["PEOPLE19"].ToString());
                Label20.Text = temp.Rows[0]["TIME20"].ToString();
                TextBox_MZ_PNO20.Text = SQLSPLIT(temp.Rows[0]["PEOPLE20"].ToString());
                Label21.Text = temp.Rows[0]["TIME21"].ToString();
                TextBox_MZ_PNO21.Text = SQLSPLIT(temp.Rows[0]["PEOPLE21"].ToString());
                Label22.Text = temp.Rows[0]["TIME22"].ToString();
                TextBox_MZ_PNO22.Text = SQLSPLIT(temp.Rows[0]["PEOPLE22"].ToString());
                Label23.Text = temp.Rows[0]["TIME23"].ToString();
                TextBox_MZ_PNO23.Text = SQLSPLIT(temp.Rows[0]["PEOPLE23"].ToString());
                Label24.Text = temp.Rows[0]["TIME24"].ToString();
                TextBox_MZ_PNO24.Text = SQLSPLIT(temp.Rows[0]["PEOPLE24"].ToString());
                TextBox_DutyCheck1.Text = temp.Rows[0]["MARK1"].ToString();
                TextBox_DutyCheck2.Text = temp.Rows[0]["MARK2"].ToString();
                TextBox_DutyCheck3.Text = temp.Rows[0]["MARK3"].ToString();
                TextBox_DutyCheck4.Text = temp.Rows[0]["MARK4"].ToString();
                TextBox_DutyCheck5.Text = temp.Rows[0]["MARK5"].ToString();
                TextBox_DutyCheck6.Text = temp.Rows[0]["MARK6"].ToString();
                TextBox_DutyCheck7.Text = temp.Rows[0]["MARK7"].ToString();
                TextBox_DutyCheck8.Text = temp.Rows[0]["MARK8"].ToString();
                TextBox_DutyCheck9.Text = temp.Rows[0]["MARK9"].ToString();
                TextBox_DutyCheck10.Text = temp.Rows[0]["MARK10"].ToString();
                TextBox_DutyCheck11.Text = temp.Rows[0]["MARK11"].ToString();
                TextBox_DutyCheck12.Text = temp.Rows[0]["MARK12"].ToString();
                TextBox_DutyCheck13.Text = temp.Rows[0]["MARK13"].ToString();
                TextBox_DutyCheck14.Text = temp.Rows[0]["MARK14"].ToString();
                TextBox_DutyCheck15.Text = temp.Rows[0]["MARK15"].ToString();
                TextBox_DutyCheck16.Text = temp.Rows[0]["MARK16"].ToString();
                TextBox_DutyCheck17.Text = temp.Rows[0]["MARK17"].ToString();
                TextBox_DutyCheck18.Text = temp.Rows[0]["MARK18"].ToString();
                TextBox_DutyCheck19.Text = temp.Rows[0]["MARK19"].ToString();
                TextBox_DutyCheck20.Text = temp.Rows[0]["MARK20"].ToString();
                TextBox_DutyCheck21.Text = temp.Rows[0]["MARK21"].ToString();
                TextBox_DutyCheck22.Text = temp.Rows[0]["MARK22"].ToString();
                TextBox_DutyCheck23.Text = temp.Rows[0]["MARK23"].ToString();
                TextBox_DutyCheck24.Text = temp.Rows[0]["MARK24"].ToString();

                

                if (temp.Rows[0]["TIME25"].ToString() != "xx")
                {
                    Label25.Text = temp.Rows[0]["TIME25"].ToString();
                    TextBox_MZ_PNO25.Text = SQLSPLIT(temp.Rows[0]["PEOPLE25"].ToString());
                    TextBox_DutyCheck25.Text = temp.Rows[0]["MARK25"].ToString();
                    TextBox_MZ_PNO25.Enabled = true;
                    TextBox_MZ_CNO25.Enabled = true;
                    TextBox_DutyCheck25.Enabled = true;
                }
                else
                {
                    Label25.Text = temp.Rows[0]["TIME25"].ToString();
                    TextBox_MZ_PNO25.Enabled = false;
                    TextBox_MZ_CNO25.Enabled = false;
                    TextBox_DutyCheck25.Enabled = false;
                    TextBox_MZ_PNO25.Text = string.Empty;
                    TextBox_MZ_CNO25.Text = string.Empty;
                    TextBox_DutyCheck25.Text = string.Empty;
                }
                if (temp.Rows[0]["TIME26"].ToString() != "xx")
                {
                    Label26.Text = temp.Rows[0]["TIME26"].ToString();
                    TextBox_MZ_PNO26.Text = SQLSPLIT(temp.Rows[0]["PEOPLE26"].ToString());
                    TextBox_DutyCheck26.Text = temp.Rows[0]["MARK26"].ToString();
                    TextBox_MZ_PNO26.Enabled = true;
                    TextBox_MZ_CNO26.Enabled = true;
                    TextBox_DutyCheck26.Enabled = true;
                }
                else
                {
                    Label26.Text = temp.Rows[0]["TIME26"].ToString();
                    TextBox_MZ_PNO26.Enabled = false;
                    TextBox_MZ_CNO26.Enabled = false;
                    TextBox_MZ_CNO26.Text = string.Empty;
                    TextBox_MZ_PNO26.Text = string.Empty;
                }
                TextBox_MZ_CNO1.Text = PNOSPLIT(TextBox_MZ_PNO1.Text);
                TextBox_MZ_CNO2.Text = PNOSPLIT(TextBox_MZ_PNO2.Text);
                TextBox_MZ_CNO3.Text = PNOSPLIT(TextBox_MZ_PNO3.Text);
                TextBox_MZ_CNO4.Text = PNOSPLIT(TextBox_MZ_PNO4.Text);
                TextBox_MZ_CNO5.Text = PNOSPLIT(TextBox_MZ_PNO5.Text);
                TextBox_MZ_CNO6.Text = PNOSPLIT(TextBox_MZ_PNO6.Text);
                TextBox_MZ_CNO7.Text = PNOSPLIT(TextBox_MZ_PNO7.Text);
                TextBox_MZ_CNO8.Text = PNOSPLIT(TextBox_MZ_PNO8.Text);
                TextBox_MZ_CNO9.Text = PNOSPLIT(TextBox_MZ_PNO9.Text);
                TextBox_MZ_CNO10.Text = PNOSPLIT(TextBox_MZ_PNO10.Text);
                TextBox_MZ_CNO11.Text = PNOSPLIT(TextBox_MZ_PNO11.Text);
                TextBox_MZ_CNO12.Text = PNOSPLIT(TextBox_MZ_PNO12.Text);
                TextBox_MZ_CNO13.Text = PNOSPLIT(TextBox_MZ_PNO13.Text);
                TextBox_MZ_CNO14.Text = PNOSPLIT(TextBox_MZ_PNO14.Text);
                TextBox_MZ_CNO15.Text = PNOSPLIT(TextBox_MZ_PNO15.Text);
                TextBox_MZ_CNO16.Text = PNOSPLIT(TextBox_MZ_PNO16.Text);
                TextBox_MZ_CNO17.Text = PNOSPLIT(TextBox_MZ_PNO17.Text);
                TextBox_MZ_CNO18.Text = PNOSPLIT(TextBox_MZ_PNO18.Text);
                TextBox_MZ_CNO19.Text = PNOSPLIT(TextBox_MZ_PNO19.Text);
                TextBox_MZ_CNO20.Text = PNOSPLIT(TextBox_MZ_PNO20.Text);
                TextBox_MZ_CNO21.Text = PNOSPLIT(TextBox_MZ_PNO21.Text);
                TextBox_MZ_CNO22.Text = PNOSPLIT(TextBox_MZ_PNO22.Text);
                TextBox_MZ_CNO23.Text = PNOSPLIT(TextBox_MZ_PNO23.Text);
                TextBox_MZ_CNO24.Text = PNOSPLIT(TextBox_MZ_PNO24.Text);
                TextBox_MZ_CNO25.Text = PNOSPLIT(TextBox_MZ_PNO25.Text);
                TextBox_MZ_CNO26.Text = PNOSPLIT(TextBox_MZ_PNO26.Text);
            }
            btDelete.Enabled = true;
            btUpdate.Enabled = true;
            btCancel.Enabled = true;
            Panel1.Visible = true;

            if (temp.Rows[0]["NO_KIND"].ToString() == "C")
            {
                TextBox_MZ_PNO1.Enabled = false;
                TextBox_MZ_PNO2.Enabled = false;
                TextBox_MZ_PNO3.Enabled = false;
                TextBox_MZ_PNO4.Enabled = false;
                TextBox_MZ_PNO5.Enabled = false;
                TextBox_MZ_PNO6.Enabled = false;
                TextBox_MZ_PNO7.Enabled = false;
                TextBox_MZ_PNO8.Enabled = false;
                TextBox_MZ_PNO9.Enabled = false;
                TextBox_MZ_PNO10.Enabled = false;
                TextBox_MZ_PNO11.Enabled = false;
                TextBox_MZ_PNO12.Enabled = false;
                TextBox_MZ_PNO13.Enabled = false;
                TextBox_MZ_PNO14.Enabled = false;
                TextBox_MZ_PNO15.Enabled = false;
                TextBox_MZ_PNO16.Enabled = false;
                TextBox_MZ_PNO17.Enabled = false;
                TextBox_MZ_PNO18.Enabled = false;
                TextBox_MZ_PNO19.Enabled = false;
                TextBox_MZ_PNO20.Enabled = false;
                TextBox_MZ_PNO21.Enabled = false;
                TextBox_MZ_PNO22.Enabled = false;
                TextBox_MZ_PNO23.Enabled = false;
                TextBox_MZ_PNO24.Enabled = false;
                TextBox_MZ_PNO25.Enabled = false;
                TextBox_MZ_PNO26.Enabled = false;
            }
            else
            {

                TextBox_MZ_CNO1.Enabled = false;
                TextBox_MZ_CNO2.Enabled = false;
                TextBox_MZ_CNO3.Enabled = false;
                TextBox_MZ_CNO4.Enabled = false;
                TextBox_MZ_CNO5.Enabled = false;
                TextBox_MZ_CNO6.Enabled = false;
                TextBox_MZ_CNO7.Enabled = false;
                TextBox_MZ_CNO8.Enabled = false;
                TextBox_MZ_CNO9.Enabled = false;
                TextBox_MZ_CNO10.Enabled = false;
                TextBox_MZ_CNO11.Enabled = false;
                TextBox_MZ_CNO12.Enabled = false;
                TextBox_MZ_CNO13.Enabled = false;
                TextBox_MZ_CNO14.Enabled = false;
                TextBox_MZ_CNO15.Enabled = false;
                TextBox_MZ_CNO16.Enabled = false;
                TextBox_MZ_CNO17.Enabled = false;
                TextBox_MZ_CNO18.Enabled = false;
                TextBox_MZ_CNO19.Enabled = false;
                TextBox_MZ_CNO20.Enabled = false;
                TextBox_MZ_CNO21.Enabled = false;
                TextBox_MZ_CNO22.Enabled = false;
                TextBox_MZ_CNO23.Enabled = false;
                TextBox_MZ_CNO24.Enabled = false;
                TextBox_MZ_CNO25.Enabled = false;
                TextBox_MZ_CNO26.Enabled = false;
            }
        }

        protected void btPREVIEW_Click(object sender, EventArgs e)
        {
            Session.Remove("error");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_DUTYTABLE_PREVIEW.aspx?" +
                                                                                                        "AD=" + Session["ADPMZ_EXAD"].ToString() +
                                                                                                        "&UNIT=" + Session["ADPMZ_EXUNIT"].ToString() +
                                                                                                        "&DUTYDATE=" + TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                                                                                                        "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_DUTYTABLE_SEARCH.aspx?TableName=BASIC&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=600,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            DUTYTABLE_SN = Session["MZ_DUTYTABLE_SN"] as List<string>;

            string DeleteString = "DELETE FROM C_DUTYTABLE WHERE MZ_DUTYTABLE_SN = '" + DUTYTABLE_SN[int.Parse(xcount.Text.Trim())] + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);


                DUTYTABLE_SN.RemoveAt(int.Parse(xcount.Text.Trim()));

                if (DUTYTABLE_SN.Count == 0)
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_ForLeaveOvertime_DUTYTABLE_SET.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    //2014/01/16
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", DeleteString);
                    }

                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (DUTYTABLE_SN.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }

                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DUTYTABLE_SN.Count.ToString() + "筆";
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    //2014/01/16
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", DeleteString);
                    }

                }
                btInsert.Enabled = true;

                btCancel.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != DUTYTABLE_SN.Count - 1)
                {
                    btNEXT.Enabled = true;
                }
                if (int.Parse(xcount.Text) == 0)
                {
                    btUpper.Enabled = false;
                }
            }
            else if (int.Parse(xcount.Text) == 0)
            {
                finddata(int.Parse(xcount.Text));
                btUpper.Enabled = false;
            }
            Label28.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DUTYTABLE_SN.Count.ToString() + "筆";

        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == DUTYTABLE_SN.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == DUTYTABLE_SN.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }

        }

        //6點起班 排26, 七點起班 排25,8.9點起班排24 
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "6")
            {
                Label25.Visible = true;
                Label26.Visible = true;
                TextBox_MZ_CNO25.Visible = true;
                TextBox_MZ_CNO26.Visible = true;
                TextBox_MZ_PNO25.Visible = true;
                TextBox_MZ_PNO26.Visible = true;
                TextBox_DutyCheck25.Visible = true;
                TextBox_DutyCheck26.Visible = true;
            }
            else if (RadioButtonList1.SelectedValue == "7")
            {
                Label25.Visible = true;
                Label26.Visible = false;
                TextBox_MZ_CNO25.Visible = true;
                TextBox_MZ_CNO26.Visible = false;
                TextBox_MZ_PNO25.Visible = true;
                TextBox_MZ_PNO26.Visible = false;
                TextBox_DutyCheck25.Visible = true;
                TextBox_DutyCheck26.Visible = false;
            }
            else
            {
                Label25.Visible = false;
                Label26.Visible = false;
                TextBox_MZ_CNO25.Visible = false;
                TextBox_MZ_CNO26.Visible = false;
                TextBox_MZ_PNO25.Visible = false;
                TextBox_MZ_PNO26.Visible = false;
                TextBox_DutyCheck25.Visible = false;
                TextBox_DutyCheck26.Visible = false;
            }
            //計算上班時段
            List<string> stringField = new List<string>();//<--儲存上班時段

            for (int i = int.Parse(RadioButtonList1.SelectedValue); i < 24; i++)
            {
                if (i < 9)
                {
                    stringField.Add("0" + i.ToString() + "-0" + (i + 1).ToString());
                }
                else if (i == 9)
                {
                    stringField.Add("0" + i.ToString() + "-" + (i + 1).ToString());
                }
                else
                {
                    stringField.Add(i.ToString() + "-" + (i + 1).ToString());
                }

            }

            for (int j = 1; j < int.Parse(RadioButtonList1.SelectedValue) + 1; j++)
            {
                stringField.Add("0" + (j - 1).ToString() + "-0" + j.ToString());
            }

            if (RadioButtonList1.SelectedValue == "6")
            {
                stringField.Add("06-07");
                stringField.Add("07-08");
            }
            else if (RadioButtonList1.SelectedValue == "7") //沒時段的直接填(XX)
            {
                stringField.Add("07-08");
                stringField.Add("xx");
            }
            else
            {
                stringField.Add("xx");
                stringField.Add("xx");
            }

            for (int k = 1; k < 27; k++)
            {
                (Panel1.FindControl("Label" + k.ToString()) as Label).Text = stringField[k - 1];
            }

            stringField.Clear();
        }

        protected void DropDownList_DUTYMODE_NO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedIndex == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取起始時段');", true);
            }
            else
            {
                Panel1.Visible = true;
            }

        }
        //兩種編號只能輸入一種！另一種直接預帶！而且預帶的不可輸入！
        protected void PNOCHANGE(TextBox PNO, TextBox CNO, TextBox PNO1)
        {
            if (!string.IsNullOrEmpty(PNO.Text))
            {
                List<string> x = new List<string>();

                string DutyDate = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));

                Check_DUTYPEOPLE(DutyDate);

                for (int i = 1; i < 27; i++)
                {
                    if ((Panel2.FindControl("TextBox_MZ_PNO" + i.ToString()) as TextBox).Text.Trim() != "")
                    {
                        x.Add("1");
                    }
                }

                if (x.Count == 0)
                {
                    for (int i = 1; i < 27; i++)
                    {
                        (Panel2.FindControl("TextBox_MZ_CNO" + i.ToString()) as TextBox).Enabled = true;
                    }
                }
                else
                {
                    for (int i = 1; i < 27; i++)
                    {
                        (Panel2.FindControl("TextBox_MZ_CNO" + i.ToString()) as TextBox).Enabled = false;
                    }
                }

                string[] ss = PNO.Text.Trim().Split(new char[] { '.' });

                CNO.Text = "";
                if (ss.Length == 1)
                {
                    string CNO_STRING = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNO FROM C_DUTYPEOPLE WHERE DUTYDATE='" + DutyDate + "' AND MZ_PNO='" + ss[0] + "'");

                    if (!string.IsNullOrEmpty(CNO_STRING))
                    {
                        CNO.Text = CNO_STRING;
                    }
                    else
                    {
                        CNO.Text = string.Empty;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ss[0] + "無此勤區編號或勤區編號重覆，請檢查或重新輸入！');", true);
                    }
                }
                else
                {

                    for (int j = 0; j < ss.Length; j++)
                    {
                        string CNO_STRING = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNO FROM C_DUTYPEOPLE WHERE DUTYDATE='" + DutyDate + "' AND MZ_PNO='" + ss[j] + "'");

                        if (!string.IsNullOrEmpty(CNO_STRING))
                        {
                            if (j == ss.Length - 1)
                            {
                                CNO.Text += CNO_STRING;
                            }
                            else
                            {
                                CNO.Text += CNO_STRING + ".";
                            }
                        }
                        else
                        {
                            CNO.Text = string.Empty;
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ss[j] + "無此勤區編號！請重新輸入！');", true);
                            return;
                        }
                    }
                }
                if (PNO1.Visible)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + PNO1.ClientID + "').focus();$get('" + PNO1.ClientID + "').focus();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + PNO.ClientID + "').focus();$get('" + PNO.ClientID + "').focus();", true);
                }
            }
            else
            {
                CNO.Text = string.Empty;
                PNO.Text = string.Empty;
            }
        }

        protected void Check_DUTYPEOPLE(string DATE)
        {
            string SelectString = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYPEOPLE WHERE DUTYDATE='" + DATE + "'");

            if (string.IsNullOrEmpty(SelectString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先新增當日勤務人員');", true);
                return;
            }
        }

        //兩種編號只能輸入一種！另一種直接預帶！而且預帶的不可輸入！
        protected void CNOCHANGE(TextBox CNO, TextBox PNO, TextBox CNO1)
        {
            if (!string.IsNullOrEmpty(CNO.Text))
            {
                List<String> x = new List<string>();

                string DutyDate = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));

                Check_DUTYPEOPLE(DutyDate);

                for (int i = 1; i < 27; i++)
                {
                    if ((Panel2.FindControl("TextBox_MZ_CNO" + i.ToString()) as TextBox).Text.Trim() != "")
                    {
                        x.Add("1");
                    }
                }

                if (x.Count == 0)
                {
                    for (int i = 1; i < 27; i++)
                    {
                        (Panel2.FindControl("TextBox_MZ_PNO" + i.ToString()) as TextBox).Enabled = true;
                    }
                }
                else
                {
                    for (int i = 1; i < 27; i++)
                    {
                        (Panel2.FindControl("TextBox_MZ_PNO" + i.ToString()) as TextBox).Enabled = false;
                    }
                }

                string[] ss = CNO.Text.Trim().Split(new char[] { '.' });

                PNO.Text = "";
                if (ss.Length == 1)
                {
                    string PNO_STRING = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNO FROM C_DUTYPEOPLE WHERE DUTYDATE='" + DutyDate + "' AND MZ_CNO='" + ss[0] + "'");

                    if (!string.IsNullOrEmpty(PNO_STRING))
                    {
                        PNO.Text += PNO_STRING;
                    }
                    else
                    {
                        PNO.Text = string.Empty;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ss[0] + "無此輪番編號！請重新輸入！');", true);
                    }
                }
                else
                {
                    for (int j = 0; j < ss.Length; j++)
                    {
                        string PNO_STRING = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNO FROM C_DUTYPEOPLE WHERE DUTYDATE='" + DutyDate + "' AND MZ_CNO='" + ss[j] + "'");

                        if (!string.IsNullOrEmpty(PNO_STRING))
                        {
                            if (j == ss.Length - 1)
                            {
                                PNO.Text += PNO_STRING;
                            }
                            else
                            {
                                PNO.Text += PNO_STRING + ".";
                            }
                        }
                        else
                        {
                            PNO.Text = string.Empty;
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ss[j] + "無此輪番編號！請重新輸入！');", true);
                        }
                    }
                }
                if (CNO1.Visible)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + CNO1.ClientID + "').focus();$get('" + CNO1.ClientID + "').focus();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + CNO.ClientID + "').focus();$get('" + CNO.ClientID + "').focus();", true);
                }
            }
            else
            {
                CNO.Text = string.Empty;
                PNO.Text = string.Empty;
            }
        }

        #region TextChanged
        protected void TextBox_MZ_PNO1_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO1, TextBox_MZ_CNO1, TextBox_MZ_PNO2);
        }

        protected void TextBox_MZ_CNO1_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO1, TextBox_MZ_PNO1, TextBox_MZ_CNO2);
        }

        protected void TextBox_MZ_PNO2_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO2, TextBox_MZ_CNO2, TextBox_MZ_PNO3);
        }

        protected void TextBox_MZ_CNO2_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO2, TextBox_MZ_PNO2, TextBox_MZ_CNO3);
        }

        protected void TextBox_MZ_PNO5_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO5, TextBox_MZ_CNO5, TextBox_MZ_PNO6);
        }

        protected void TextBox_MZ_CNO5_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO5, TextBox_MZ_PNO5, TextBox_MZ_CNO6);
        }

        protected void TextBox_MZ_PNO6_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO6, TextBox_MZ_CNO6, TextBox_MZ_PNO7);
        }

        protected void TextBox_MZ_CNO6_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO6, TextBox_MZ_PNO6, TextBox_MZ_CNO7);
        }

        protected void TextBox_MZ_PNO7_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO7, TextBox_MZ_CNO7, TextBox_MZ_PNO8);
        }

        protected void TextBox_MZ_CNO7_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO7, TextBox_MZ_PNO7, TextBox_MZ_CNO8);
        }

        protected void TextBox_MZ_PNO8_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO8, TextBox_MZ_CNO8, TextBox_MZ_PNO9);
        }

        protected void TextBox_MZ_CNO8_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO8, TextBox_MZ_PNO8, TextBox_MZ_CNO9);
        }

        protected void TextBox_MZ_PNO9_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO9, TextBox_MZ_CNO9, TextBox_MZ_PNO10);
        }

        protected void TextBox_MZ_CNO9_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO9, TextBox_MZ_PNO9, TextBox_MZ_CNO10);
        }

        protected void TextBox_MZ_PNO10_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO10, TextBox_MZ_CNO10, TextBox_MZ_PNO11);
        }

        protected void TextBox_MZ_CNO10_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO10, TextBox_MZ_PNO10, TextBox_MZ_CNO11);
        }

        protected void TextBox_MZ_PNO11_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO11, TextBox_MZ_CNO11, TextBox_MZ_PNO12);
        }

        protected void TextBox_MZ_CNO11_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO11, TextBox_MZ_PNO11, TextBox_MZ_CNO12);
        }

        protected void TextBox_MZ_PNO12_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO12, TextBox_MZ_CNO12, TextBox_MZ_PNO13);
        }

        protected void TextBox_MZ_CNO12_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO12, TextBox_MZ_PNO12, TextBox_MZ_CNO13);
        }

        protected void TextBox_MZ_PNO13_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO13, TextBox_MZ_CNO13, TextBox_MZ_PNO14);
        }

        protected void TextBox_MZ_CNO13_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO13, TextBox_MZ_PNO13, TextBox_MZ_CNO14);
        }

        protected void TextBox_MZ_PNO14_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO14, TextBox_MZ_CNO14, TextBox_MZ_PNO15);
        }

        protected void TextBox_MZ_CNO14_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO14, TextBox_MZ_PNO14, TextBox_MZ_CNO15);
        }

        protected void TextBox_MZ_PNO15_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO15, TextBox_MZ_CNO15, TextBox_MZ_PNO16);
        }

        protected void TextBox_MZ_CNO15_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO15, TextBox_MZ_PNO15, TextBox_MZ_CNO16);
        }

        protected void TextBox_MZ_PNO16_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO16, TextBox_MZ_CNO16, TextBox_MZ_PNO17);
        }

        protected void TextBox_MZ_CNO16_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO16, TextBox_MZ_PNO16, TextBox_MZ_CNO17);
        }

        protected void TextBox_MZ_PNO17_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO17, TextBox_MZ_CNO17, TextBox_MZ_PNO18);
        }

        protected void TextBox_MZ_CNO17_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO17, TextBox_MZ_PNO17, TextBox_MZ_CNO18);
        }

        protected void TextBox_MZ_PNO18_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO18, TextBox_MZ_CNO18, TextBox_MZ_PNO19);
        }

        protected void TextBox_MZ_CNO18_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO18, TextBox_MZ_PNO18, TextBox_MZ_CNO19);
        }

        protected void TextBox_MZ_PNO19_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO19, TextBox_MZ_CNO19, TextBox_MZ_PNO20);
        }

        protected void TextBox_MZ_CNO19_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO19, TextBox_MZ_PNO19, TextBox_MZ_CNO20);
        }

        protected void TextBox_MZ_PNO20_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO20, TextBox_MZ_CNO20, TextBox_MZ_PNO21);
        }

        protected void TextBox_MZ_CNO20_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO20, TextBox_MZ_PNO20, TextBox_MZ_CNO21);
        }

        protected void TextBox_MZ_PNO21_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO21, TextBox_MZ_CNO21, TextBox_MZ_PNO22);
        }

        protected void TextBox_MZ_CNO21_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO21, TextBox_MZ_PNO21, TextBox_MZ_CNO22);
        }

        protected void TextBox_MZ_PNO22_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO22, TextBox_MZ_CNO22, TextBox_MZ_PNO23);
        }

        protected void TextBox_MZ_CNO22_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO22, TextBox_MZ_PNO22, TextBox_MZ_CNO23);
        }

        protected void TextBox_MZ_PNO23_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO23, TextBox_MZ_CNO23, TextBox_MZ_PNO24);
        }

        protected void TextBox_MZ_CNO23_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO23, TextBox_MZ_PNO23, TextBox_MZ_CNO24);
        }

        protected void TextBox_MZ_PNO24_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO24, TextBox_MZ_CNO24, TextBox_MZ_PNO25);
        }

        protected void TextBox_MZ_CNO24_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO24, TextBox_MZ_PNO24, TextBox_MZ_CNO25);
        }

        protected void TextBox_MZ_PNO3_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO3, TextBox_MZ_CNO3, TextBox_MZ_PNO4);
        }

        protected void TextBox_MZ_CNO3_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO3, TextBox_MZ_PNO3, TextBox_MZ_CNO4);
        }

        protected void TextBox_MZ_PNO4_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO4, TextBox_MZ_CNO4, TextBox_MZ_PNO5);
        }

        protected void TextBox_MZ_CNO4_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO4, TextBox_MZ_PNO4, TextBox_MZ_CNO5);
        }

        protected void TextBox_MZ_PNO25_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO25, TextBox_MZ_CNO25, TextBox_MZ_PNO26);
        }

        protected void TextBox_MZ_CNO25_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO25, TextBox_MZ_PNO25, TextBox_MZ_CNO26);
        }

        protected void TextBox_MZ_PNO26_TextChanged(object sender, EventArgs e)
        {
            PNOCHANGE(TextBox_MZ_PNO26, TextBox_MZ_CNO26, TextBox_MZ_PNO26);
        }

        protected void TextBox_MZ_CNO26_TextChanged(object sender, EventArgs e)
        {
            CNOCHANGE(TextBox_MZ_CNO26, TextBox_MZ_PNO26, TextBox_MZ_CNO26);
        }
        #endregion

        //由勤區編號找出身分證號！
        protected string IDString(string TextBoxString)
        {
            string[] IDList = TextBoxString.Split(new char[] { '.' });
            string IDstr = "";
            for (int i = 0; i < IDList.Length; i++)
            {
                if (i == IDList.Length - 1)
                {
                    IDstr += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_DUTYPEOPLE WHERE MZ_PNO='" + IDList[i].ToString() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + TextBox_DUTYDATE.Text.Replace("/", "").PadLeft(7, '0') + "'");
                }
                else
                {
                    IDstr += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_DUTYPEOPLE WHERE MZ_PNO='" + IDList[i].ToString() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + TextBox_DUTYDATE.Text.Replace("/", "").PadLeft(7, '0') + "'") + '.';
                }
            }

            return IDstr;
        }
        //組SQL語法 一個一個寫太麻煩！
        protected void btInsert_Click(object sender, EventArgs e)
        {

            if (Check_DUTYMODE_IsExsist())
            {
                for (int i = 1; i <= 26; i++)
                {
                    if (i == 26)
                    {
                        SqlField += "TIME" + i.ToString() + ",PEOPLE" + i.ToString() + ",MARK" + i.ToString();
                        SqlField1 += ":TIME" + i.ToString() + ",@PEOPLE" + i.ToString() + ",@MARK" + i.ToString();
                        
                    }
                    else
                    {
                        SqlField += "TIME" + i.ToString() + ",PEOPLE" + i.ToString() + ",MARK" + i.ToString() + ",";
                        SqlField1 += ":TIME" + i.ToString() + ",@PEOPLE" + i.ToString() + ",@MARK" + i.ToString() + ",";
                    }
                }
                //塞資料庫
                using (SqlConnection Insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Insertconn.Open();
                    string InsertString = "INSERT INTO C_DUTYTABLE(MZ_DUTYTABLE_SN,MZ_AD,MZ_UNIT,DUTY_NO,DUTYMODE_NO,COMM_CHANNEL,DUTYDATE,NO_KIND,MZ_DUTYPATROL_NO,MZ_DUTYTARGET_NO,DUTYMODE_NO_MEMO," + SqlField +
                                          ") VALUES( NEXT VALUE FOR dbo.C_DUTYTABLE_SN,@MZ_EXAD,@MZ_EXUNIT,@DUTY_NO,@DUTYMODE_NO,@COMM_CHANNEL,@DUTYDATE,@NO_KIND,@MZ_DUTYPATROL_NO,@MZ_DUTYTARGET_NO,@DUTYMODE_NO_MEMO," + SqlField1 + ")";
                    SqlCommand Insertcmd = new SqlCommand(InsertString, Insertconn);
                    Insertcmd.CommandType = CommandType.Text;

                    if (TextBox_MZ_CNO1.Enabled == true)
                    {
                        Insertcmd.Parameters.Add("NO_KIND", SqlDbType.VarChar).Value = "C";
                    }
                    else
                    {
                        Insertcmd.Parameters.Add("NO_KIND", SqlDbType.VarChar).Value = "P";
                    }

                    Insertcmd.Parameters.Add("MZ_EXAD", SqlDbType.VarChar).Value = Session["ADPMZ_EXAD"].ToString();
                    Insertcmd.Parameters.Add("MZ_EXUNIT", SqlDbType.VarChar).Value = Session["ADPMZ_EXUNIT"].ToString();
                    Insertcmd.Parameters.Add("DUTY_NO", SqlDbType.VarChar).Value = TextBox_DUTYNO.Text.Trim();
                    Insertcmd.Parameters.Add("DUTYMODE_NO", SqlDbType.VarChar).Value = TextBox_DUTYMODE.Text.Trim();
                    Insertcmd.Parameters.Add("COMM_CHANNEL", SqlDbType.VarChar).Value = TextBox_COMM_CHANNEL.Text.Trim();
                    Insertcmd.Parameters.Add("DUTYDATE", SqlDbType.VarChar).Value = TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    Insertcmd.Parameters.Add("MZ_DUTYPATROL_NO", SqlDbType.VarChar).Value = TextBox_MZ_DUTYPATROL_NO.Text.Trim();
                    Insertcmd.Parameters.Add("MZ_DUTYTARGET_NO", SqlDbType.VarChar).Value = TextBox_MZ_DUTYTARGET_NO.Text.Trim();
                    Insertcmd.Parameters.Add("DUTYMODE_NO_MEMO", SqlDbType.VarChar).Value = TextBox_DUTYMODE_NO_MEMO.Text.Trim();

                    Insertcmd.Parameters.Add("TIME1", SqlDbType.VarChar).Value = Label1.Text.Trim();
                    Insertcmd.Parameters.Add("TIME2", SqlDbType.VarChar).Value = Label2.Text.Trim();
                    Insertcmd.Parameters.Add("TIME3", SqlDbType.VarChar).Value = Label3.Text.Trim();
                    Insertcmd.Parameters.Add("TIME4", SqlDbType.VarChar).Value = Label4.Text.Trim();
                    Insertcmd.Parameters.Add("TIME5", SqlDbType.VarChar).Value = Label5.Text.Trim();
                    Insertcmd.Parameters.Add("TIME6", SqlDbType.VarChar).Value = Label6.Text.Trim();
                    Insertcmd.Parameters.Add("TIME7", SqlDbType.VarChar).Value = Label7.Text.Trim();
                    Insertcmd.Parameters.Add("TIME8", SqlDbType.VarChar).Value = Label8.Text.Trim();
                    Insertcmd.Parameters.Add("TIME9", SqlDbType.VarChar).Value = Label9.Text.Trim();
                    Insertcmd.Parameters.Add("TIME10", SqlDbType.VarChar).Value = Label10.Text.Trim();
                    Insertcmd.Parameters.Add("TIME11", SqlDbType.VarChar).Value = Label11.Text.Trim();
                    Insertcmd.Parameters.Add("TIME12", SqlDbType.VarChar).Value = Label12.Text.Trim();
                    Insertcmd.Parameters.Add("TIME13", SqlDbType.VarChar).Value = Label13.Text.Trim();
                    Insertcmd.Parameters.Add("TIME14", SqlDbType.VarChar).Value = Label14.Text.Trim();
                    Insertcmd.Parameters.Add("TIME15", SqlDbType.VarChar).Value = Label15.Text.Trim();
                    Insertcmd.Parameters.Add("TIME16", SqlDbType.VarChar).Value = Label16.Text.Trim();
                    Insertcmd.Parameters.Add("TIME17", SqlDbType.VarChar).Value = Label17.Text.Trim();
                    Insertcmd.Parameters.Add("TIME18", SqlDbType.VarChar).Value = Label18.Text.Trim();
                    Insertcmd.Parameters.Add("TIME19", SqlDbType.VarChar).Value = Label19.Text.Trim();
                    Insertcmd.Parameters.Add("TIME20", SqlDbType.VarChar).Value = Label20.Text.Trim();
                    Insertcmd.Parameters.Add("TIME21", SqlDbType.VarChar).Value = Label21.Text.Trim();
                    Insertcmd.Parameters.Add("TIME22", SqlDbType.VarChar).Value = Label22.Text.Trim();
                    Insertcmd.Parameters.Add("TIME23", SqlDbType.VarChar).Value = Label23.Text.Trim();
                    Insertcmd.Parameters.Add("TIME24", SqlDbType.VarChar).Value = Label24.Text.Trim();
                    Insertcmd.Parameters.Add("TIME25", SqlDbType.VarChar).Value = Label25.Text.Trim();
                    Insertcmd.Parameters.Add("TIME26", SqlDbType.VarChar).Value = Label26.Text.Trim();
                    //資料庫裡存身分證號！不存勤區代碼！
                    Insertcmd.Parameters.Add("PEOPLE1", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO1.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE2", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO2.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE3", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO3.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE4", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO4.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE5", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO5.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE6", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO6.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE7", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO7.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE8", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO8.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE9", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO9.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE10", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO10.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE11", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO11.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE12", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO12.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE13", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO13.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE14", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO14.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE15", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO15.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE16", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO16.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE17", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO17.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE18", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO18.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE19", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO19.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE20", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO20.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE21", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO21.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE22", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO22.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE23", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO23.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE24", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO24.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE25", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO25.Text.Trim());
                    Insertcmd.Parameters.Add("PEOPLE26", SqlDbType.VarChar).Value = IDString(TextBox_MZ_PNO26.Text.Trim());
                    Insertcmd.Parameters.Add("MARK1", SqlDbType.VarChar).Value = TextBox_DutyCheck1.Text.Trim();
                    Insertcmd.Parameters.Add("MARK2", SqlDbType.VarChar).Value = TextBox_DutyCheck2.Text.Trim();
                    Insertcmd.Parameters.Add("MARK3", SqlDbType.VarChar).Value = TextBox_DutyCheck3.Text.Trim();
                    Insertcmd.Parameters.Add("MARK4", SqlDbType.VarChar).Value = TextBox_DutyCheck4.Text.Trim();
                    Insertcmd.Parameters.Add("MARK5", SqlDbType.VarChar).Value = TextBox_DutyCheck5.Text.Trim();
                    Insertcmd.Parameters.Add("MARK6", SqlDbType.VarChar).Value = TextBox_DutyCheck6.Text.Trim();
                    Insertcmd.Parameters.Add("MARK7", SqlDbType.VarChar).Value = TextBox_DutyCheck7.Text.Trim();
                    Insertcmd.Parameters.Add("MARK8", SqlDbType.VarChar).Value = TextBox_DutyCheck8.Text.Trim();
                    Insertcmd.Parameters.Add("MARK9", SqlDbType.VarChar).Value = TextBox_DutyCheck9.Text.Trim();
                    Insertcmd.Parameters.Add("MARK10", SqlDbType.VarChar).Value = TextBox_DutyCheck10.Text.Trim();
                    Insertcmd.Parameters.Add("MARK11", SqlDbType.VarChar).Value = TextBox_DutyCheck11.Text.Trim();
                    Insertcmd.Parameters.Add("MARK12", SqlDbType.VarChar).Value = TextBox_DutyCheck12.Text.Trim();
                    Insertcmd.Parameters.Add("MARK13", SqlDbType.VarChar).Value = TextBox_DutyCheck13.Text.Trim();
                    Insertcmd.Parameters.Add("MARK14", SqlDbType.VarChar).Value = TextBox_DutyCheck14.Text.Trim();
                    Insertcmd.Parameters.Add("MARK15", SqlDbType.VarChar).Value = TextBox_DutyCheck15.Text.Trim();
                    Insertcmd.Parameters.Add("MARK16", SqlDbType.VarChar).Value = TextBox_DutyCheck16.Text.Trim();
                    Insertcmd.Parameters.Add("MARK17", SqlDbType.VarChar).Value = TextBox_DutyCheck17.Text.Trim();
                    Insertcmd.Parameters.Add("MARK18", SqlDbType.VarChar).Value = TextBox_DutyCheck18.Text.Trim();
                    Insertcmd.Parameters.Add("MARK19", SqlDbType.VarChar).Value = TextBox_DutyCheck19.Text.Trim();
                    Insertcmd.Parameters.Add("MARK20", SqlDbType.VarChar).Value = TextBox_DutyCheck20.Text.Trim();
                    Insertcmd.Parameters.Add("MARK21", SqlDbType.VarChar).Value = TextBox_DutyCheck21.Text.Trim();
                    Insertcmd.Parameters.Add("MARK22", SqlDbType.VarChar).Value = TextBox_DutyCheck22.Text.Trim();
                    Insertcmd.Parameters.Add("MARK23", SqlDbType.VarChar).Value = TextBox_DutyCheck23.Text.Trim();
                    Insertcmd.Parameters.Add("MARK24", SqlDbType.VarChar).Value = TextBox_DutyCheck24.Text.Trim();
                    Insertcmd.Parameters.Add("MARK25", SqlDbType.VarChar).Value = TextBox_DutyCheck25.Text.Trim();
                    Insertcmd.Parameters.Add("MARK26", SqlDbType.VarChar).Value = TextBox_DutyCheck26.Text.Trim();
                    try
                    {

                        Insertcmd.ExecuteNonQuery();

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                        Clean_Text();
                        //2010.06.04 LOG紀錄 by伊珊
                        //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));
                        if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd)) == "N")
                        {
                            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));
                        }
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);

                    }
                }
            }

         
        }


        string[] zz; //分割"."號,存字串之陣列
        int j;

        protected void btCreate_Click(object sender, EventArgs e)
        {
            string repeat = "";//紀錄重複時段 
            string strangeTime = "";//紀錄異常時數

            List<String> z = new List<string>();//計算每日排班時數用

            string DutyDate = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));

            //共有幾筆勤務項目
            string SelectString = "SELECT * FROM C_DUTYTABLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                  "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + DutyDate + "' AND DUTY_NO !='Y' ORDER BY DUTY_NO,DUTYMODE_NO";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");




           

            if (temp.Rows.Count > 0)
            {

                List<String> y = new List<string>();//紀錄與比較同時段人員是否重複的DATALIST
                List<String> A = new List<string>();//紀錄與比較同時段人員是否重複的DATALIST

                for (j = 0; j < 26; j++)
                {

                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        zz = temp.Rows[i]["PEOPLE" + (j + 1).ToString()].ToString().Split(new char[] { '.' });
                        for (int o = 0; o < zz.Length; o++)
                        {
                            A.Add(zz[o].ToString());
                        }

                    }
                    for (int k = 0; k < A.Count; k++)
                    {
                        if (!string.IsNullOrEmpty(A[k]))
                        {
                            y.Add(A[k]);
                            z.Add(A[k]);
                        }
                    }

                    for (int l = 0; l < y.Count; l++)
                    {
                        string now = y[l].ToString();

                        for (int m = l + 1; m < y.Count; m++)
                        {
                            if (now == y[m])
                            {
                                repeat += o_A_DLBASE.CNAME(now) + "：重複" + ",";
                            }
                        }
                        if (l == y.Count - 1)
                        {
                            y.Clear();
                            A.Clear();
                        }
                    }
                }
            }

            string countID;//稽核用ID

            List<string> gg = new List<string>();//紀錄已計算過的身分證號

            for (int p = 0; p < z.Count; p++)
            {
                bool sss = true;//同身分證號是否跳過的判斷～～

                if (z.Count == 1)
                {
                    p = 0;
                }
                countID = z[p].ToString();

                for (int i = 0; i < gg.Count; i++)
                {
                    if (countID == gg[i])
                    {
                        sss = false;
                    }
                }

                if (sss == false)
                {
                    continue;
                }

                int count = 1;//紀錄時數

                for (int q = p + 1; q < z.Count; q++)
                {
                    if (z.Count == 1)
                    {
                        q = 0;
                    }

                    if (countID == z[q].ToString())
                    {
                        count++;
                    }

                    if (q == z.Count - 1)
                    {
                        if (count > 12 || count < 8)
                        {
                            strangeTime +=o_A_DLBASE.CNAME(countID) + "：時數異常" + count.ToString() + "小時,";
                        }
                    }

                    if (z.Count == 0)
                    {
                        break;
                    }
                }

                gg.Insert(gg.Count, countID);
            }

            if (repeat != "" || strangeTime != "")
            {
                string ErrorMessage = repeat + strangeTime;
                Session["error"] = ErrorMessage;
                //20140731
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorMessage + "');window.open('C_ForLeaveOvertime_DUTYTABLE_PREVIEW.aspx?" +
                                                                                                        "AD=" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString()) +
                                                                                                        "&UNIT=" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString()) +
                                                                                                        "&DUTYDATE=" + TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                                                                                                        "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
                return;
            }
            //20140731
            string Deletestring = "DELETE FROM C_DUTYTABLE_PERSONAL WHERE MZ_AD='" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString())
                                                       + "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString())
                                                       + "' AND DUTYDATE='" + DutyDate + "'";
            o_DBFactory.ABC_toTest.Edit_Data(Deletestring);
            string nowID;
            //計算每日勤務表時數！
            for (int p = 0; p < z.Count; p++)
            {
                if (z.Count == 1)
                {
                    p = 0;
                }

                nowID = z[p].ToString();

                if (nowID == o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + nowID + "' AND DUTYDATE='" + DutyDate + "'"))
                {
                    continue;
                }

                int count = 1;

                for (int q = p + 1; q < z.Count; q++)
                {
                    if (z.Count == 1)
                    {
                        q = 0;
                    }

                    if (nowID == z[q].ToString())
                    {
                        count++;
                    }

                    if (q == z.Count - 1)
                    {
                        string InsertString = "INSERT INTO C_DUTYTABLE_PERSONAL (MZ_AD,MZ_UNIT,MZ_ID,DUTYDATE,TIME1,DUTYITEM1,TIME2,DUTYITEM2,TIME3,DUTYITEM3," +
                                                                                "TIME4,DUTYITEM4,TIME5,DUTYITEM5,TIME6,DUTYITEM6,TIME7,DUTYITEM7,TIME8,DUTYITEM8," +
                                                                                "TIME9,DUTYITEM9,TIME10,DUTYITEM10,TIME11,DUTYITEM11,TIME12,DUTYITEM12,TIME13,DUTYITEM13," +
                                                                                "TIME14,DUTYITEM14,TIME15,DUTYITEM15,TIME16,DUTYITEM16,TIME17,DUTYITEM17,TIME18,DUTYITEM18," +
                                                                                "TIME19,DUTYITEM19,TIME20,DUTYITEM20,TIME21,DUTYITEM21,TIME22,DUTYITEM22,TIME23,DUTYITEM23," +
                                                                                "TIME24,DUTYITEM24,TIME25,DUTYITEM25,TIME26,DUTYITEM26,TOTAL_HOURS,ISDIRECTTIME " +
                                                                                ",MARK1,MARK2,MARK3,MARK4,MARK5,MARK6,MARK7,MARK8,MARK9,MARK10,MARK11,MARK12,MARK13,"+
                                                                                "MARK14,MARK15,MARK16,MARK17,MARK18,MARK19,MARK20,MARK21,MARK22,MARK23,MARK24,MARK25,MARK26)" +
                                                                      " VALUES (@MZ_EXAD,@MZ_EXUNIT,@MZ_ID,@DUTYDATE,@TIME1,@DUTYITEM1,@TIME2,@DUTYITEM2,@TIME3,@DUTYITEM3," +
                                                                               ":TIME4,@DUTYITEM4,@TIME5,@DUTYITEM5,@TIME6,@DUTYITEM6,@TIME7,@DUTYITEM7,@TIME8,@DUTYITEM8," +
                                                                               ":TIME9,@DUTYITEM9,@TIME10,@DUTYITEM10,@TIME11,@DUTYITEM11,@TIME12,@DUTYITEM12,@TIME13,@DUTYITEM13," +
                                                                               ":TIME14,@DUTYITEM14,@TIME15,@DUTYITEM15,@TIME16,@DUTYITEM16,@TIME17,@DUTYITEM17,@TIME18,@DUTYITEM18," +
                                                                               ":TIME19,@DUTYITEM19,@TIME20,@DUTYITEM20,@TIME21,@DUTYITEM21,@TIME22,@DUTYITEM22,@TIME23,@DUTYITEM23," +
                                                                               ":TIME24,@DUTYITEM24,@TIME25,@DUTYITEM25,@TIME26,@DUTYITEM26,@TOTAL_HOURS,@ISDIRECTTIME"+
                                                                                ",@MARK1,@MARK2,@MARK3,@MARK4,@MARK5,@MARK6,@MARK7,@MARK8,@MARK9,@MARK10,@MARK11,@MARK12,@MARK13,"+
                                                                                ":MARK14,@MARK15,@MARK16,@MARK17,@MARK18,@MARK19,@MARK20,@MARK21,@MARK22,@MARK23,@MARK24,@MARK25,@MARK26)";
                 

                        SqlParameter[] parameterList = {
                            new SqlParameter("MZ_EXAD",SqlDbType.Char){Value = o_A_DLBASE.PAD(nowID)},
                            new SqlParameter("MZ_EXUNIT",SqlDbType.Char){Value = o_A_DLBASE.PUNIT(nowID)},
                            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = nowID},
                            new SqlParameter("DUTYDATE",SqlDbType.VarChar){Value = DutyDate},
                            new SqlParameter("TIME1",SqlDbType.VarChar){Value = Label1.Text.Trim()},
                            new SqlParameter("DUTYITEM1",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME1='"+Label1.Text.Trim()+"' AND PEOPLE1 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME2",SqlDbType.VarChar){Value = Label2.Text.Trim()},
                            new SqlParameter("DUTYITEM2",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME2='"+Label2.Text.Trim()+"' AND PEOPLE2 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'" )},
                            new SqlParameter("TIME3",SqlDbType.VarChar){Value = Label3.Text.Trim()},
                            new SqlParameter("DUTYITEM3",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME3='"+Label3.Text.Trim()+"' AND PEOPLE3 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME4",SqlDbType.VarChar){Value = Label4.Text.Trim()},
                            new SqlParameter("DUTYITEM4",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME4='"+Label4.Text.Trim()+"' AND PEOPLE4 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME5",SqlDbType.VarChar){Value = Label5.Text.Trim()},
                            new SqlParameter("DUTYITEM5",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME5='"+Label5.Text.Trim()+"' AND PEOPLE5 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME6",SqlDbType.VarChar){Value = Label6.Text.Trim()},
                            new SqlParameter("DUTYITEM6",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME6='"+Label6.Text.Trim()+"' AND PEOPLE6 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME7",SqlDbType.VarChar){Value = Label7.Text.Trim()},
                            new SqlParameter("DUTYITEM7",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME7='"+Label7.Text.Trim()+"' AND PEOPLE7 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME8",SqlDbType.VarChar){Value = Label8.Text.Trim()},
                            new SqlParameter("DUTYITEM8",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME8='"+Label8.Text.Trim()+"' AND PEOPLE8 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME9",SqlDbType.VarChar){Value = Label9.Text.Trim()},
                            new SqlParameter("DUTYITEM9",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME9='"+Label9.Text.Trim()+"' AND PEOPLE9 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME10",SqlDbType.VarChar){Value = Label10.Text.Trim()},
                            new SqlParameter("DUTYITEM10",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME10='"+Label10.Text.Trim()+"' AND PEOPLE10 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME11",SqlDbType.VarChar){Value = Label11.Text.Trim()},
                            new SqlParameter("DUTYITEM11",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME11='"+Label11.Text.Trim()+"' AND PEOPLE11 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME12",SqlDbType.VarChar){Value = Label12.Text.Trim()},
                            new SqlParameter("DUTYITEM12",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME12='"+Label12.Text.Trim()+"' AND PEOPLE12 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME13",SqlDbType.VarChar){Value = Label13.Text.Trim()},
                            new SqlParameter("DUTYITEM13",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME13='"+Label13.Text.Trim()+"' AND PEOPLE13 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME14",SqlDbType.VarChar){Value = Label14.Text.Trim()},
                            new SqlParameter("DUTYITEM14",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME14='"+Label14.Text.Trim()+"' AND PEOPLE14 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME15",SqlDbType.VarChar){Value = Label15.Text.Trim()},
                            new SqlParameter("DUTYITEM15",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME15='"+Label15.Text.Trim()+"' AND PEOPLE15 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME16",SqlDbType.VarChar){Value = Label16.Text.Trim()},
                            new SqlParameter("DUTYITEM16",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME16='"+Label16.Text.Trim()+"' AND PEOPLE16 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME17",SqlDbType.VarChar){Value = Label17.Text.Trim()},
                            new SqlParameter("DUTYITEM17",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME17='"+Label17.Text.Trim()+"' AND PEOPLE17 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME18",SqlDbType.VarChar){Value = Label18.Text.Trim()},
                            new SqlParameter("DUTYITEM18",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME18='"+Label18.Text.Trim()+"' AND PEOPLE18 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME19",SqlDbType.VarChar){Value = Label19.Text.Trim()},
                            new SqlParameter("DUTYITEM19",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME19='"+Label19.Text.Trim()+"' AND PEOPLE19 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME20",SqlDbType.VarChar){Value = Label20.Text.Trim()},
                            new SqlParameter("DUTYITEM20",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME20='"+Label20.Text.Trim()+"' AND PEOPLE20 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME21",SqlDbType.VarChar){Value = Label21.Text.Trim()},
                            new SqlParameter("DUTYITEM21",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME21='"+Label21.Text.Trim()+"' AND PEOPLE21 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME22",SqlDbType.VarChar){Value = Label22.Text.Trim()},
                            new SqlParameter("DUTYITEM22",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME22='"+Label22.Text.Trim()+"' AND PEOPLE22 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME23",SqlDbType.VarChar){Value = Label23.Text.Trim()},
                            new SqlParameter("DUTYITEM23",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME23='"+Label23.Text.Trim()+"' AND PEOPLE23 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME24",SqlDbType.VarChar){Value = Label24.Text.Trim()},
                            new SqlParameter("DUTYITEM24",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME24='"+Label24.Text.Trim()+"' AND PEOPLE24 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME25",SqlDbType.VarChar){Value = Label25.Text.Trim()},
                            new SqlParameter("DUTYITEM25",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME25='"+Label25.Text.Trim()+"' AND PEOPLE25 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TIME26",SqlDbType.VarChar){Value = Label26.Text.Trim()},
                            new SqlParameter("DUTYITEM26",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NO FROM C_DUTYTABLE WHERE TIME26='"+Label26.Text.Trim()+"' AND PEOPLE26 LIKE '%"+nowID+"%' AND DUTYDATE='"+DutyDate+"'")},
                            new SqlParameter("TOTAL_HOURS",SqlDbType.Float){Value =count},
                            new SqlParameter("ISDIRECTTIME",SqlDbType.Float){Value =0},
                            new SqlParameter("MARK1",SqlDbType.VarChar){Value =TextBox_DutyCheck1.Text.Trim()},
                            new SqlParameter("MARK2",SqlDbType.VarChar){Value =TextBox_DutyCheck2.Text.Trim()},
                            new SqlParameter("MARK3",SqlDbType.VarChar){Value =TextBox_DutyCheck3.Text.Trim()},
                            new SqlParameter("MARK4",SqlDbType.VarChar){Value =TextBox_DutyCheck4.Text.Trim()},
                            new SqlParameter("MARK5",SqlDbType.VarChar){Value =TextBox_DutyCheck5.Text.Trim()},
                            new SqlParameter("MARK6",SqlDbType.VarChar){Value =TextBox_DutyCheck6.Text.Trim()},
                            new SqlParameter("MARK7",SqlDbType.VarChar){Value =TextBox_DutyCheck7.Text.Trim()},
                            new SqlParameter("MARK8",SqlDbType.VarChar){Value =TextBox_DutyCheck8.Text.Trim()},
                            new SqlParameter("MARK9",SqlDbType.VarChar){Value =TextBox_DutyCheck9.Text.Trim()},
                            new SqlParameter("MARK10",SqlDbType.VarChar){Value =TextBox_DutyCheck10.Text.Trim()},
                            new SqlParameter("MARK11",SqlDbType.VarChar){Value =TextBox_DutyCheck11.Text.Trim()},
                            new SqlParameter("MARK12",SqlDbType.VarChar){Value =TextBox_DutyCheck12.Text.Trim()},
                            new SqlParameter("MARK13",SqlDbType.VarChar){Value =TextBox_DutyCheck13.Text.Trim()},
                            new SqlParameter("MARK14",SqlDbType.VarChar){Value =TextBox_DutyCheck14.Text.Trim()},
                            new SqlParameter("MARK15",SqlDbType.VarChar){Value =TextBox_DutyCheck15.Text.Trim()},
                            new SqlParameter("MARK16",SqlDbType.VarChar){Value =TextBox_DutyCheck16.Text.Trim()},
                            new SqlParameter("MARK17",SqlDbType.VarChar){Value =TextBox_DutyCheck17.Text.Trim()},
                            new SqlParameter("MARK18",SqlDbType.VarChar){Value =TextBox_DutyCheck18.Text.Trim()},
                            new SqlParameter("MARK19",SqlDbType.VarChar){Value =TextBox_DutyCheck19.Text.Trim()},
                            new SqlParameter("MARK20",SqlDbType.VarChar){Value =TextBox_DutyCheck20.Text.Trim()},
                            new SqlParameter("MARK21",SqlDbType.VarChar){Value =TextBox_DutyCheck21.Text.Trim()},
                            new SqlParameter("MARK22",SqlDbType.VarChar){Value =TextBox_DutyCheck22.Text.Trim()},
                            new SqlParameter("MARK23",SqlDbType.VarChar){Value =TextBox_DutyCheck23.Text.Trim()},
                            new SqlParameter("MARK24",SqlDbType.VarChar){Value =TextBox_DutyCheck24.Text.Trim()},
                            new SqlParameter("MARK25",SqlDbType.VarChar){Value =TextBox_DutyCheck25.Text.Trim()},
                            new SqlParameter("MARK26",SqlDbType.VarChar){Value =TextBox_DutyCheck26.Text.Trim()},
                            };

                        try
                        {
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertString, parameterList);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生成功');", true);

                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生失敗');", true);
                            throw;
                        }
                    }
                    if (z.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        //}

        protected void btCancel_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 27; i++)
            {
                (Panel2.FindControl("TextBox_MZ_PNO" + i.ToString()) as TextBox).Text = string.Empty;
                (Panel2.FindControl("TextBox_MZ_CNO" + i.ToString()) as TextBox).Text = string.Empty;
            }


        }



        protected void TextBox_DUTYMODE_TextChanged(object sender, EventArgs e)
        {


            Panel1.Visible = true;
            btCancel.Enabled = true;
            btCreate.Enabled = true;
            btInsert.Enabled = true;
            btPREVIEW.Enabled = true;

            TextBox_DUTYNO.Text = TextBox_DUTYNO.Text.ToUpper();
            string Cname = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO='" + TextBox_DUTYNO.Text.Trim() + "'");
            if (!string.IsNullOrEmpty(Cname))
            {
                TextBox_DUTYNO1.Text = Cname;
            }
            else
            {
                TextBox_DUTYNO1.Text = string.Empty;
            }


        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {


            if (Check_DUTYMODE_IsExsist())
            {
                DUTYTABLE_SN = Session["MZ_DUTYTABLE_SN"] as List<string>;

                string strSQL = "UPDATE " +
                                           "C_DUTYTABLE " +
                                "SET " +
                                           "MZ_AD = @MZ_EXAD,MZ_UNIT = @MZ_EXUNIT,DUTY_NO = @DUTY_NO,DUTYMODE_NO = @DUTYMODE_NO," +
                                           "COMM_CHANNEL = @COMM_CHANNEL,DUTYDATE = @DUTYDATE,TIME1 = @TIME1,PEOPLE1 = @PEOPLE1," +
                                           "TIME2 = @TIME2,PEOPLE2 = @PEOPLE2,TIME3 = @TIME3,PEOPLE3 = @PEOPLE3,TIME4 = @TIME4," +
                                           "PEOPLE4 = @PEOPLE4,TIME5 = @TIME5,PEOPLE5 = @PEOPLE5,TIME6 = @TIME6,PEOPLE6 = @PEOPLE6," +
                                           "TIME7 = @TIME7,PEOPLE7 = @PEOPLE7,TIME8 = @TIME8,PEOPLE8 = @PEOPLE8,TIME9 = @TIME9," +
                                           "PEOPLE9 = @PEOPLE9,TIME10 = @TIME10,PEOPLE10 = @PEOPLE10,TIME11 = @TIME11,PEOPLE11 = @PEOPLE11," +
                                           "TIME12 = @TIME12,PEOPLE12 = @PEOPLE12,TIME13 = @TIME13,PEOPLE13 = @PEOPLE13,TIME14 = @TIME14," +
                                           "PEOPLE14 = @PEOPLE14,TIME15 = @TIME15,PEOPLE15 = @PEOPLE15,TIME16 = @TIME16,PEOPLE16 = @PEOPLE16," +
                                           "TIME17 = @TIME17,PEOPLE17 = @PEOPLE17,TIME18 = @TIME18,PEOPLE18 = @PEOPLE18,TIME19 = @TIME19," +
                                           "PEOPLE19 = @PEOPLE19,TIME20 = @TIME20,PEOPLE20 = @PEOPLE20,TIME21 = @TIME21,PEOPLE21 = @PEOPLE21," +
                                           "TIME22 = @TIME22,PEOPLE22 = @PEOPLE22,TIME23 = @TIME23,PEOPLE23 = @PEOPLE23,TIME24 = @TIME24," +
                                           "PEOPLE24 = @PEOPLE24,TIME25 = @TIME25,PEOPLE25 = @PEOPLE25,TIME26 = @TIME26,PEOPLE26 = @PEOPLE26," +
                                           "NO_KIND = @NO_KIND,MZ_DUTYTABLE_SN = @MZ_DUTYTABLE_SN,MZ_DUTYPATROL_NO = @MZ_DUTYPATROL_NO," +
                                           "MZ_DUTYTARGET_NO = @MZ_DUTYTARGET_NO "
                                           + ",MARK1=@MARK1"
                                            + ",MARK2=@MARK2"
                                            + ",MARK3=@MARK3"
                                            + ",MARK4=@MARK4"
                                            + ",MARK5=@MARK5"
                                            + ",MARK6=@MARK6"
                                            + ",MARK7=@MARK7"
                                            + ",MARK8=@MARK8"
                                            + ",MARK9=@MARK9"
                                            + ",MARK10=@MARK10"
                                            + ",MARK11=@MARK11"
                                            + ",MARK12=@MARK12"
                                            + ",MARK13=@MARK13"
                                            + ",MARK14=@MARK14"
                                            + ",MARK15=@MARK15"
                                            + ",MARK16=@MARK16"
                                            + ",MARK17=@MARK17"
                                            + ",MARK18=@MARK18"
                                            + ",MARK19=@MARK19"
                                            + ",MARK20=@MARK20"
                                            + ",MARK21=@MARK21"
                                            + ",MARK22=@MARK22"
                                            + ",MARK23=@MARK23"
                                            + ",MARK24=@MARK24"
                                            + ",MARK25=@MARK25"
                                            + ",MARK26=@MARK26" +
                                " WHERE " +
                                           "MZ_DUTYTABLE_SN='" + DUTYTABLE_SN[int.Parse(xcount.Text)] + "'";

                SqlParameter[] parameterList = {
            new SqlParameter("MZ_EXAD",SqlDbType.Char){Value = Session["ADPMZ_EXAD"].ToString()},
            new SqlParameter("MZ_EXUNIT",SqlDbType.Char){Value = Session["ADPMZ_EXUNIT"]},
            new SqlParameter("DUTY_NO",SqlDbType.VarChar){Value = TextBox_DUTYMODE.Text.Trim().Substring(0, 1)},
            new SqlParameter("DUTYMODE_NO",SqlDbType.VarChar){Value = TextBox_DUTYMODE.Text},
            new SqlParameter("COMM_CHANNEL",SqlDbType.VarChar){Value = TextBox_COMM_CHANNEL.Text},
            new SqlParameter("DUTYDATE",SqlDbType.VarChar){Value = TextBox_DUTYDATE.Text},
            new SqlParameter("TIME1",SqlDbType.VarChar){Value = Label1.Text},
            new SqlParameter("PEOPLE1",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO1.Text.Trim())},
            new SqlParameter("TIME2",SqlDbType.VarChar){Value = Label2.Text},
            new SqlParameter("PEOPLE2",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO2.Text.Trim())},
            new SqlParameter("TIME3",SqlDbType.VarChar){Value = Label3.Text},
            new SqlParameter("PEOPLE3",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO3.Text.Trim())},
            new SqlParameter("TIME4",SqlDbType.VarChar){Value = Label4.Text},
            new SqlParameter("PEOPLE4",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO4.Text.Trim())},
            new SqlParameter("TIME5",SqlDbType.VarChar){Value = Label5.Text},
            new SqlParameter("PEOPLE5",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO5.Text.Trim())},
            new SqlParameter("TIME6",SqlDbType.VarChar){Value = Label6.Text},
            new SqlParameter("PEOPLE6",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO6.Text.Trim())},
            new SqlParameter("TIME7",SqlDbType.VarChar){Value = Label7.Text},
            new SqlParameter("PEOPLE7",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO7.Text.Trim())},
            new SqlParameter("TIME8",SqlDbType.VarChar){Value = Label8.Text},
            new SqlParameter("PEOPLE8",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO8.Text.Trim())},
            new SqlParameter("TIME9",SqlDbType.VarChar){Value = Label9.Text},
            new SqlParameter("PEOPLE9",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO9.Text.Trim())},
            new SqlParameter("TIME10",SqlDbType.VarChar){Value = Label10.Text},
            new SqlParameter("PEOPLE10",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO10.Text.Trim())},
            new SqlParameter("TIME11",SqlDbType.VarChar){Value = Label11.Text},
            new SqlParameter("PEOPLE11",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO11.Text.Trim())},
            new SqlParameter("TIME12",SqlDbType.VarChar){Value = Label12.Text},
            new SqlParameter("PEOPLE12",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO12.Text.Trim())},
            new SqlParameter("TIME13",SqlDbType.VarChar){Value = Label13.Text},
            new SqlParameter("PEOPLE13",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO13.Text.Trim())},
            new SqlParameter("TIME14",SqlDbType.VarChar){Value = Label14.Text},
            new SqlParameter("PEOPLE14",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO14.Text.Trim())},
            new SqlParameter("TIME15",SqlDbType.VarChar){Value = Label15.Text},
            new SqlParameter("PEOPLE15",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO15.Text.Trim())},
            new SqlParameter("TIME16",SqlDbType.VarChar){Value = Label16.Text},
            new SqlParameter("PEOPLE16",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO16.Text.Trim())},
            new SqlParameter("TIME17",SqlDbType.VarChar){Value = Label17.Text},
            new SqlParameter("PEOPLE17",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO17.Text.Trim())},
            new SqlParameter("TIME18",SqlDbType.VarChar){Value = Label18.Text},
            new SqlParameter("PEOPLE18",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO18.Text.Trim())},
            new SqlParameter("TIME19",SqlDbType.VarChar){Value = Label19.Text},
            new SqlParameter("PEOPLE19",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO19.Text.Trim())},
            new SqlParameter("TIME20",SqlDbType.VarChar){Value = Label20.Text},
            new SqlParameter("PEOPLE20",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO20.Text.Trim())},
            new SqlParameter("TIME21",SqlDbType.VarChar){Value = Label21.Text},
            new SqlParameter("PEOPLE21",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO21.Text.Trim())},
            new SqlParameter("TIME22",SqlDbType.VarChar){Value = Label22.Text},
            new SqlParameter("PEOPLE22",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO22.Text.Trim())},
            new SqlParameter("TIME23",SqlDbType.VarChar){Value = Label23.Text},
            new SqlParameter("PEOPLE23",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO23.Text.Trim())},
            new SqlParameter("TIME24",SqlDbType.VarChar){Value = Label24.Text},
            new SqlParameter("PEOPLE24",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO24.Text.Trim())},
            new SqlParameter("TIME25",SqlDbType.VarChar){Value = Label25.Text},
            new SqlParameter("PEOPLE25",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO25.Text.Trim())},
            new SqlParameter("TIME26",SqlDbType.VarChar){Value = Label26.Text},
            new SqlParameter("PEOPLE26",SqlDbType.VarChar){Value = IDString(TextBox_MZ_PNO26.Text.Trim())},
            new SqlParameter("NO_KIND",SqlDbType.VarChar){Value = TextBox_MZ_CNO1.Enabled?"C":"P"},
            new SqlParameter("MZ_DUTYTABLE_SN",SqlDbType.Float){Value = DUTYTABLE_SN[int.Parse(xcount.Text)]},
            new SqlParameter("MZ_DUTYPATROL_NO",SqlDbType.VarChar){Value = TextBox_MZ_DUTYPATROL_NO.Text},
            new SqlParameter("MZ_DUTYTARGET_NO",SqlDbType.VarChar){Value = TextBox_MZ_DUTYTARGET_NO.Text},
            new SqlParameter("MARK1",SqlDbType.VarChar){Value =TextBox_DutyCheck1.Text.Trim()},
            new SqlParameter("MARK2",SqlDbType.VarChar){Value =TextBox_DutyCheck2.Text.Trim()},
            new SqlParameter("MARK3",SqlDbType.VarChar){Value =TextBox_DutyCheck3.Text.Trim()},
            new SqlParameter("MARK4",SqlDbType.VarChar){Value =TextBox_DutyCheck4.Text.Trim()},
            new SqlParameter("MARK5",SqlDbType.VarChar){Value =TextBox_DutyCheck5.Text.Trim()},
            new SqlParameter("MARK6",SqlDbType.VarChar){Value =TextBox_DutyCheck6.Text.Trim()},
            new SqlParameter("MARK7",SqlDbType.VarChar){Value =TextBox_DutyCheck7.Text.Trim()},
            new SqlParameter("MARK8",SqlDbType.VarChar){Value =TextBox_DutyCheck8.Text.Trim()},
            new SqlParameter("MARK9",SqlDbType.VarChar){Value =TextBox_DutyCheck9.Text.Trim()},
            new SqlParameter("MARK10",SqlDbType.VarChar){Value =TextBox_DutyCheck10.Text.Trim()},
            new SqlParameter("MARK11",SqlDbType.VarChar){Value =TextBox_DutyCheck11.Text.Trim()},
            new SqlParameter("MARK12",SqlDbType.VarChar){Value =TextBox_DutyCheck12.Text.Trim()},
            new SqlParameter("MARK13",SqlDbType.VarChar){Value =TextBox_DutyCheck13.Text.Trim()},
            new SqlParameter("MARK14",SqlDbType.VarChar){Value =TextBox_DutyCheck14.Text.Trim()},
            new SqlParameter("MARK15",SqlDbType.VarChar){Value =TextBox_DutyCheck15.Text.Trim()},
            new SqlParameter("MARK16",SqlDbType.VarChar){Value =TextBox_DutyCheck16.Text.Trim()},
            new SqlParameter("MARK17",SqlDbType.VarChar){Value =TextBox_DutyCheck17.Text.Trim()},
            new SqlParameter("MARK18",SqlDbType.VarChar){Value =TextBox_DutyCheck18.Text.Trim()},
            new SqlParameter("MARK19",SqlDbType.VarChar){Value =TextBox_DutyCheck19.Text.Trim()},
            new SqlParameter("MARK20",SqlDbType.VarChar){Value =TextBox_DutyCheck20.Text.Trim()},
            new SqlParameter("MARK21",SqlDbType.VarChar){Value =TextBox_DutyCheck21.Text.Trim()},
            new SqlParameter("MARK22",SqlDbType.VarChar){Value =TextBox_DutyCheck22.Text.Trim()},
            new SqlParameter("MARK23",SqlDbType.VarChar){Value =TextBox_DutyCheck23.Text.Trim()},
            new SqlParameter("MARK24",SqlDbType.VarChar){Value =TextBox_DutyCheck24.Text.Trim()},
            new SqlParameter("MARK25",SqlDbType.VarChar){Value =TextBox_DutyCheck25.Text.Trim()},
            new SqlParameter("MARK26",SqlDbType.VarChar){Value =TextBox_DutyCheck26.Text.Trim()}
            };
                try
                {
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), strSQL);
                    //2014/01/07
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), strSQL) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", strSQL);
                    }

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);

                }
            }

        }



        protected void Button1_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_ForLeaveOvertime_DUTYTABLE_SET.aspx?TPM_FION=" + Request.QueryString["TPM_FION"]);
        }

        protected void TextBox_DUTYDATE_TextChanged(object sender, EventArgs e)
        {

            TextBox_DUTYDATE.Text = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", ""));

            if (TextBox_DUTYDATE.Text != "")
            {
                if (!DateManange.Check_date(TextBox_DUTYDATE.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    TextBox_DUTYDATE.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_DUTYDATE.ClientID + "').focus();$get('" + TextBox_DUTYDATE.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_DUTYDATE.Text = o_CommonService.Personal_ReturnDateString(TextBox_DUTYDATE.Text.Trim());
                    RadioButtonList1.Focus();
                }
            }
        }

        protected void btDUTYMODE_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_DUTYNO.ClientID;
            Session["KTYPE_CID1"] = TextBox_DUTYNO1.ClientID;                                                                                                                              //20147031
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=DUTYMODE&AD=" + o_A_DLBASE.PMZAD(Session["ADPMZ_ID"].ToString()) + "&UNIT=" + o_A_DLBASE.PMZUNIT(Session["ADPMZ_ID"].ToString()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btDUTYMODE_NO_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_DUTYMODE.ClientID;
            Session["KTYPE_CID1"] = TextBox_DUTYMODE_NO_MEMO.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?MZ_KTYPE=DUTYMODE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btDUTYPATROL_NO_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_DUTYPATROL_NO.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?MZ_KTYPE=DUTYPATROL&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btDUTYTARGET_NO_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_DUTYTARGET_NO.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?MZ_KTYPE=DUTYTARGET&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void TextBox_DUTYMODE_TextChanged1(object sender, EventArgs e)
        {
            TextBox_DUTYMODE.Text = TextBox_DUTYMODE.Text.ToUpper();

            string Cname = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTYMODE_NAME FROM C_DUTYMODE WHERE DUTYMODE_NO='" + TextBox_DUTYMODE.Text.Trim() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
            if (string.IsNullOrEmpty(Cname))
            {

                TextBox_DUTYMODE_NO_MEMO.Text = string.Empty;
                TextBox_COMM_CHANNEL.Text = string.Empty;
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
                //TextBox_DUTYMODE.Text = string.Empty;
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_DUTYMODE.ClientID + "').focus();$get('" + TextBox_DUTYMODE.ClientID + "').focus();", true);
            }
            else
            {
                TextBox_COMM_CHANNEL.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT COMM_CHANNEL FROM C_DUTYMODE WHERE DUTYMODE_NO='" + TextBox_DUTYMODE.Text.Trim() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
                TextBox_DUTYMODE_NO_MEMO.Text = Cname;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_COMM_CHANNEL.ClientID + "').focus();$get('" + TextBox_COMM_CHANNEL.ClientID + "').focus();", true);
            }
        }

        /// <summary>
        /// 判斷片語是否存在，若無則新增
        /// </summary>
        /// <returns></returns>
        private bool Check_DUTYMODE_IsExsist()
        {

            TextBox_DUTYMODE.Text = TextBox_DUTYMODE.Text.ToUpper();
            string Cname = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTYMODE_NAME FROM C_DUTYMODE WHERE DUTYMODE_NO='" + TextBox_DUTYMODE.Text.Trim() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
            if (string.IsNullOrEmpty(RadioButtonList1.SelectedValue))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('每日起班時間不能為空值');", true);
                return false;
            }
            else if (string.IsNullOrEmpty(TextBox_DUTYMODE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('勤務說明英文代碼不能為空值');", true);
                return false;
            }
            else if (string.IsNullOrEmpty(Cname))
            {
                if (TextBox_DUTYMODE.Text.Substring(0, 1) != TextBox_DUTYNO.Text.Trim())
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('勤務說明英文代碼需與勤務項目相同');", true);
                    TextBox_DUTYMODE.Text = string.Empty;
                    return false;
                }
                else if (String.IsNullOrEmpty(TextBox_DUTYMODE_NO_MEMO.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('勤務說明敘述不得為空');", true);

                    return false;
                }
                else
                {
                    try
                    {
                        o_DBFactory.ABC_toTest.vExecSQL(@"INSERT INTO C_DUTYMODE (DUTY_NO, DUTYMODE_NO,DUTYMODE_NAME,MZ_AD,MZ_UNIT,COMM_CHANNEL)
                                      VALUES('" + TextBox_DUTYNO.Text.Trim() + "','" + TextBox_DUTYMODE.Text.Trim() + "', '" + TextBox_DUTYMODE_NO_MEMO.Text.Trim() + "', '" + Session["ADPMZ_EXAD"].ToString() + "','" + Session["ADPMZ_EXUNIT"].ToString() + "','" + TextBox_COMM_CHANNEL.Text.Trim() + "')");
                        return true; 
                    }
                    catch
                    {
                        return false;
                    }


                }

            }
            else
            {
                try
                {
                    o_DBFactory.ABC_toTest.vExecSQL(@"UPDATE C_DUTYMODE SET DUTYMODE_NAME ='" + TextBox_DUTYMODE_NO_MEMO.Text.Trim() + "',COMM_CHANNEL ='" + TextBox_COMM_CHANNEL.Text.Trim() + "' WHERE  DUTYMODE_NO='" + TextBox_DUTYMODE.Text.Trim() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
                    return true; 
                }
                catch
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 新增後清除TEXT值
        /// </summary>
        private void Clean_Text()
        {
            TextBox_MZ_PNO1.Text = string.Empty;
            TextBox_MZ_PNO2.Text = string.Empty;
            TextBox_MZ_PNO3.Text = string.Empty;
            TextBox_MZ_PNO4.Text = string.Empty;
            TextBox_MZ_PNO5.Text = string.Empty;
            TextBox_MZ_PNO6.Text = string.Empty;
            TextBox_MZ_PNO7.Text = string.Empty;
            TextBox_MZ_PNO8.Text = string.Empty;
            TextBox_MZ_PNO9.Text = string.Empty;
            TextBox_MZ_PNO10.Text = string.Empty;
            TextBox_MZ_PNO11.Text = string.Empty;
            TextBox_MZ_PNO12.Text = string.Empty;
            TextBox_MZ_PNO13.Text = string.Empty;
            TextBox_MZ_PNO14.Text = string.Empty;
            TextBox_MZ_PNO15.Text = string.Empty;
            TextBox_MZ_PNO16.Text = string.Empty;
            TextBox_MZ_PNO17.Text = string.Empty;
            TextBox_MZ_PNO18.Text = string.Empty;
            TextBox_MZ_PNO19.Text = string.Empty;
            TextBox_MZ_PNO20.Text = string.Empty;
            TextBox_MZ_PNO21.Text = string.Empty;
            TextBox_MZ_PNO22.Text = string.Empty;
            TextBox_MZ_PNO23.Text = string.Empty;
            TextBox_MZ_PNO24.Text = string.Empty;
            TextBox_MZ_PNO25.Text = string.Empty;
            TextBox_MZ_PNO26.Text = string.Empty;
            TextBox_MZ_CNO1.Text = string.Empty;
            TextBox_MZ_CNO2.Text = string.Empty;
            TextBox_MZ_CNO3.Text = string.Empty;
            TextBox_MZ_CNO4.Text = string.Empty;
            TextBox_MZ_CNO5.Text = string.Empty;
            TextBox_MZ_CNO6.Text = string.Empty;
            TextBox_MZ_CNO7.Text = string.Empty;
            TextBox_MZ_CNO8.Text = string.Empty;
            TextBox_MZ_CNO9.Text = string.Empty;
            TextBox_MZ_CNO10.Text = string.Empty;
            TextBox_MZ_CNO11.Text = string.Empty;
            TextBox_MZ_CNO12.Text = string.Empty;
            TextBox_MZ_CNO13.Text = string.Empty;
            TextBox_MZ_CNO14.Text = string.Empty;
            TextBox_MZ_CNO15.Text = string.Empty;
            TextBox_MZ_CNO16.Text = string.Empty;
            TextBox_MZ_CNO17.Text = string.Empty;
            TextBox_MZ_CNO18.Text = string.Empty;
            TextBox_MZ_CNO19.Text = string.Empty;
            TextBox_MZ_CNO20.Text = string.Empty;
            TextBox_MZ_CNO21.Text = string.Empty;
            TextBox_MZ_CNO22.Text = string.Empty;
            TextBox_MZ_CNO23.Text = string.Empty;
            TextBox_MZ_CNO24.Text = string.Empty;
            TextBox_MZ_CNO25.Text = string.Empty;
            TextBox_MZ_CNO26.Text = string.Empty;
                TextBox_MZ_CNO1.Enabled = true;
                TextBox_MZ_CNO2.Enabled = true;
                TextBox_MZ_CNO3.Enabled = true;
                TextBox_MZ_CNO4.Enabled = true;
                TextBox_MZ_CNO5.Enabled = true;
                TextBox_MZ_CNO6.Enabled = true;
                TextBox_MZ_CNO7.Enabled = true;
                TextBox_MZ_CNO8.Enabled = true;
                TextBox_MZ_CNO9.Enabled = true;
                TextBox_MZ_CNO10.Enabled = true;
                TextBox_MZ_CNO11.Enabled = true;
                TextBox_MZ_CNO12.Enabled = true;
                TextBox_MZ_CNO13.Enabled = true;
                TextBox_MZ_CNO14.Enabled = true;
                TextBox_MZ_CNO15.Enabled = true;
                TextBox_MZ_CNO16.Enabled = true;
                TextBox_MZ_CNO17.Enabled = true;
                TextBox_MZ_CNO18.Enabled = true;
                TextBox_MZ_CNO19.Enabled = true;
                TextBox_MZ_CNO20.Enabled = true;
                TextBox_MZ_CNO21.Enabled = true;
                TextBox_MZ_CNO22.Enabled = true;
                TextBox_MZ_CNO23.Enabled = true;
                TextBox_MZ_CNO24.Enabled = true;
                TextBox_MZ_CNO25.Enabled = true;
                TextBox_MZ_CNO26.Enabled = true;
                TextBox_MZ_PNO1.Enabled = true;
                TextBox_MZ_PNO2.Enabled = true;
                TextBox_MZ_PNO3.Enabled = true;
                TextBox_MZ_PNO4.Enabled = true;
                TextBox_MZ_PNO5.Enabled = true;
                TextBox_MZ_PNO6.Enabled = true;
                TextBox_MZ_PNO7.Enabled = true;
                TextBox_MZ_PNO8.Enabled = true;
                TextBox_MZ_PNO9.Enabled = true;
                TextBox_MZ_PNO10.Enabled = true;
                TextBox_MZ_PNO11.Enabled = true;
                TextBox_MZ_PNO12.Enabled = true;
                TextBox_MZ_PNO13.Enabled = true;
                TextBox_MZ_PNO14.Enabled = true;
                TextBox_MZ_PNO15.Enabled = true;
                TextBox_MZ_PNO16.Enabled = true;
                TextBox_MZ_PNO17.Enabled = true;
                TextBox_MZ_PNO18.Enabled = true;
                TextBox_MZ_PNO19.Enabled = true;
                TextBox_MZ_PNO20.Enabled = true;
                TextBox_MZ_PNO21.Enabled = true;
                TextBox_MZ_PNO22.Enabled = true;
                TextBox_MZ_PNO23.Enabled = true;
                TextBox_MZ_PNO24.Enabled = true;
                TextBox_MZ_PNO25.Enabled = true;
                TextBox_MZ_PNO26.Enabled = true;
                TextBox_DutyCheck1.Text = string.Empty;
                TextBox_DutyCheck2.Text = string.Empty;
                TextBox_DutyCheck3.Text = string.Empty;
                TextBox_DutyCheck4.Text = string.Empty;
                TextBox_DutyCheck5.Text = string.Empty;
                TextBox_DutyCheck6.Text = string.Empty;
                TextBox_DutyCheck7.Text = string.Empty;
                TextBox_DutyCheck8.Text = string.Empty;
                TextBox_DutyCheck9.Text = string.Empty;
                TextBox_DutyCheck10.Text = string.Empty;
                TextBox_DutyCheck11.Text = string.Empty;
                TextBox_DutyCheck12.Text = string.Empty;
                TextBox_DutyCheck13.Text = string.Empty;
                TextBox_DutyCheck14.Text = string.Empty;
                TextBox_DutyCheck15.Text = string.Empty;
                TextBox_DutyCheck16.Text = string.Empty;
                TextBox_DutyCheck17.Text = string.Empty;
                TextBox_DutyCheck18.Text = string.Empty;
                TextBox_DutyCheck19.Text = string.Empty;
                TextBox_DutyCheck20.Text = string.Empty;
                TextBox_DutyCheck21.Text = string.Empty;
                TextBox_DutyCheck22.Text = string.Empty;
                TextBox_DutyCheck23.Text = string.Empty;
                TextBox_DutyCheck24.Text = string.Empty;
                TextBox_DutyCheck25.Text = string.Empty;
                TextBox_DutyCheck26.Text = string.Empty;
        }
    }
}
