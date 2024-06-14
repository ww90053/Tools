using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Logic;
using TPPDDB.Model;
using TPPDDB.Service;
using System.Data.SqlClient;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTY_Limit_HourCalc : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A": 
                    case "B":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "C":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        ChangeUnit();
                        break;
                    case "D":
                        if (!(Session["ADPMZ_ID"].ToString() == "K220886357"))//20141104先開給鳳美姊 之後再釐清邱課是怎麼開權限給他用超勤
                        {
                            DropDownList_MZ_EXAD.DataBind();
                            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                            DropDownList_MZ_EXAD.Enabled = false;
                            ChangeUnit();
                            DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                            DropDownList_MZ_EXUNIT.Enabled = false;
                            TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                            TextBox_MZ_ID.Enabled = false;
                        }
                        else
                        {
                            DropDownList_MZ_EXAD.DataBind();
                            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                            ChangeUnit();
                        }
                        break;
                    case "E":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        ChangeUnit();
                        DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_MZ_EXUNIT.Enabled = false;
                        break;
                }
            }
            C.set_Panel_EnterToTAB(ref this.Panel1);
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

        protected void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXUNIT.DataSource = temp;
            DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_EXUNIT.DataBind();
            DropDownList_MZ_EXUNIT.Items.Insert(0, "");
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            areaLogs.Visible = false;
            areaLogs.Value = "";


            //Step1. 先判斷前置之查詢條件是否有輸入完畢
            //若無輸入身分證字號，則至少要選機關跟單位
            if (String.IsNullOrEmpty(TextBox_MZ_ID.Text))
            {
                if (String.IsNullOrEmpty(DropDownList_MZ_EXAD.SelectedValue))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入機關');", true);
                    return;
                }
            }

            ////金額上限為 19000 元
            Int32 iMoneyLimit = 19000;
            Int32.TryParse(txtMONEY_LIMIT.Text, out iMoneyLimit);

            //Step2. 實際進行運算動作

            //取出所選機關單位下的所有使用者資料
            String MZ_ID = TextBox_MZ_ID.Text;
            String MZ_AD = DropDownList_MZ_EXAD.Text;
            String MZ_UNIT = DropDownList_MZ_EXUNIT.Text;

            List<String> AdditionalWhere = new List<String>();
                         AdditionalWhere.Add(String.Format(@" ({0} = '{1}' OR (MZ_EXUNIT='PAY0' OR MZ_UNIT='PAY0'))", "MZ_ISPOLICE", "Y"));
                         AdditionalWhere.Add(String.Format(@" {0} = '{1}' ", "MZ_STATUS2", "Y"));

            List<AccountModel> Users = new List<AccountModel>();

            Users.AddRange(AccountService.lookupAccount(MZ_ID, MZ_AD, MZ_UNIT,
                                                        "Now", //Now: 現服機關單位, Build: 編制機關單位
                                                        String.Join(" AND ", AdditionalWhere.ToArray())));
            Users.AddRange(AccountService.lookupAccount(MZ_ID, MZ_AD, MZ_UNIT,
                                                        "Build", //Now: 現服機關單位, Build: 編制機關單位
                                                        String.Join(" AND ", AdditionalWhere.ToArray())));
            Users.AddRange(AccountService.lookupAccount(MZ_ID, MZ_AD, MZ_UNIT,
                                                        "Pay", //發薪機關
                                                        String.Join(" AND ", AdditionalWhere.ToArray())));

            List<String> lstMsg = new List<String>();
            List<SqlParameter> lstParameter = new List<SqlParameter>();
            foreach (AccountModel item in Users)
            {
                //每小時支付額
                LogicOvertime Logic = new LogicOvertime();
                //Double dHourPay = MathHelper.Round(Logic.getHourPay(item.ID), 2) ;
                Double dHourPay = MathHelper.Round(Logic.getHourPay(item.ID));

                //超勤時數上限
                Int32 iHourLimit = 0;
                try
                {
                    Double d = iMoneyLimit / dHourPay;
                    iHourLimit = Convert.ToInt32(Math.Ceiling(d));
                }
                catch (Exception ex)
                {
                    iHourLimit = 0;
                }

                // 時數最大上限不能超過100 如果超過則以100計算
                // 改為不受限於100小時。 20200210 by sky
                //if (iHourLimit > 100)
                //{
                //    iHourLimit = 100;
                //}

                //正式開始插入資料
                lstParameter.Clear();
                lstParameter = new List<SqlParameter>();
                lstParameter.Add(new SqlParameter("MZ_AD", SqlDbType.NVarChar) { Value = item.ExAD });
                lstParameter.Add(new SqlParameter("MZ_UNIT", SqlDbType.NVarChar) { Value = item.ExUnit });
                lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = item.ID });
                lstParameter.Add(new SqlParameter("MZ_HOUR_LIMIT", SqlDbType.NVarChar) { Value = iHourLimit });
                lstParameter.Add(new SqlParameter("MZ_MONEY_LIMIT", SqlDbType.NVarChar) { Value = iMoneyLimit });

                String ErrMsg = "";

                if (DutyService.isUserSetOvertTimeLimit(item.ID, item.ExAD, item.ExUnit))
                    DutyService.Update_OverTime_Limit(lstParameter, ref ErrMsg);
                else
                    DutyService.Insert_OverTime_Limit(lstParameter, ref ErrMsg);

                if (!String.IsNullOrEmpty(ErrMsg))
                    lstMsg.Add("[失敗]" + item.ID + ", 錯誤訊息:" + ErrMsg);
                else
                    lstMsg.Add("[成功]" + item.ID );
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "$('body').unmask(); alert('處理完成! 請參閱下方結果訊息');", true);

            if (lstMsg.Count > 0)
            {
                areaLogs.Visible = true;
                areaLogs.Value = String.Join("\r\n", lstMsg.ToArray());
            }

        }
    }
}
