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
using System.IO;
using TPPDDB.App_Code;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    //TODO SKY 正式上線時替換
    public partial class C_ForLeaveBasic_New : System.Web.UI.Page
    {
        CFService CFService = new CFService();
        List<String> DLTB01_MZ_ID = new List<string>();

        List<String> DLTB01_MZ_IDATE1 = new List<string>();

        List<String> DLTB01_MZ_ITIME1 = new List<string>();

        string F = string.Empty;
        string F1 = string.Empty;
        string F2 = string.Empty;

        /// <summary>
        /// 檢查補休情況
        /// </summary>
        protected bool check_15_Hour()
        {
            //僅檢查申請補休(15)
            if (TextBox_MZ_CODE.Text == "11" || TextBox_MZ_CODE.Text == "16" || TextBox_MZ_CODE.Text == "22")
            {
                string overtype = "";
                if (TextBox_MZ_CODE.Text == "11")
                {
                    overtype = "'OTB' OR OVERTIME_TYPE = 'OTT' ";
                }

                if (TextBox_MZ_CODE.Text == "16")
                {
                    overtype = "'OTU'";
                }

                if (TextBox_MZ_CODE.Text == "22")
                {
                    overtype = "'OTD'";
                }
                //修改差假
                if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    //取回已申請加班補休時數
                    string strSQL = @"Select cob.OVER_DAY, NVL(cofd.REST_HOUR, 0) As MZ_RESTHOUR, cob.OVERTIME_TYPE As OVER_TYPE From C_OVERTIME_FOR_DLTB01 cofd
                                      left join C_OVERTIME_BASE cob on cob.SN = cofd.OVERTIME_SN
                                        Where ( OverTime_type = @overtype ) and  MZ_DLTB01_SN=@MZ_DLTB01_SN 
                                      Order by cob.OVER_DAY ";
                    //List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("MZ_DLTB01_SN", ViewState["SN"].ToStringNullSafe()) };
                    List<SqlParameter> parameters = new List<SqlParameter>()
                                    {
                                        new SqlParameter("MZ_DLTB01_SN", ViewState["SN"].ToStringNullSafe()),
                                        new SqlParameter("overtype",overtype.ToStringNullSafe())
                                    };
                    DataTable oldApplyDay = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                    DataTable newApplyDay = new DataTable();
                    //判斷是否有重新選擇可補休日
                    if (Session["MZ_RESTHOUR_DT"] != null)
                    {
                        newApplyDay = Session["MZ_RESTHOUR_DT"] as DataTable;
                    }
                    //申請所需時數
                    int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 * 60 +
                            int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text) * 60;
                    //統計已選時數
                    int applyTotalTime = 0;
                    if (oldApplyDay != null && oldApplyDay.Rows.Count > 0)
                    {
                        foreach (DataRow item in oldApplyDay.Rows)
                        {
                            applyTotalTime += Convert.ToInt32(item["MZ_RESTHOUR"].ToStringNullSafe());
                        }
                    }
                    //如果 申請所需時數 > 以前已選時數
                    if (TTIME > applyTotalTime)
                    {
                        //所需時數不足，增加新選補休日資料

                        //如果有已選時數資料
                        if (newApplyDay != null && newApplyDay.Rows.Count > 0)
                        {
                            //計算這次還要總共申請多少?
                            //TTIME 設為 需要扣除的時數,EX: 480-120=360
                            TTIME -= applyTotalTime;
                            //重設applyTotalTime
                            applyTotalTime = 0;
                            //統計新選補休日時數
                            foreach (DataRow item in newApplyDay.Rows)
                            {
                                applyTotalTime += Convert.ToInt32(item["MZ_RESTHOUR"].ToStringNullSafe());
                            }
                            //如果這次的申請目標時數,仍然小於剛才已經勾選的補休假加總時數
                            if (TTIME < applyTotalTime)
                            {
                                //計算要刪除多餘申請時數,稱之為扣除額
                                //360=840-480
                                int s = applyTotalTime - TTIME;
                                //從尾到頭開始尋覽
                                for (int i = newApplyDay.Rows.Count - 1; i > 0; i--)
                                {
                                    //扣除額 大於 當日申請的時數,代表此日的申請可以省略,且減小扣除額(正常不該發生)
                                    if (s > int.Parse(newApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()))
                                    {
                                        s = s - int.Parse(newApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe());
                                        newApplyDay.Rows.Remove(newApplyDay.Rows[i]);
                                    }
                                    //扣除額 等於 當日申請的時數,代表此日的申請可以省略
                                    else if (s == int.Parse(newApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()))
                                    {
                                        newApplyDay.Rows.Remove(newApplyDay.Rows[i]);
                                        //跳脫迴圈
                                        break;
                                    }
                                    //扣除額 小於 當日申請的時數,就扣除掉當日的申請時數,這樣申請的時數就不會超過了
                                    else
                                    {
                                        newApplyDay.Rows[i]["MZ_RESTHOUR"] = int.Parse(newApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()) - s;
                                        //紀錄LOG
                                        SetLog_MZ_RESTHOUR_DT(newApplyDay.Rows[i], "check_15_Hour");
                                        //跳脫迴圈
                                        break;
                                    }
                                }
                            }
                            else if (TTIME > applyTotalTime)
                            {
                                TextBox_MZ_TDAY.Text = "0";
                                TextBox_MZ_TTIME.Text = "0";

                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('目前補休時數小於輸入時數，請重新輸入！')", true);
                                return false;
                            }

                            //將已申請補休日+新申請補休日
                            oldApplyDay.Merge(newApplyDay);
                            Session["MZ_RESTHOUR_DT"] = oldApplyDay;
                            BT_GV1_Click(BT_GV1, EventArgs.Empty);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所需補休時數不足，請再選擇可補休日！')", true);
                            return false;
                        }
                    }
                    else if (TTIME < applyTotalTime)
                    {
                        //所需時數減少，刪除已申請紀錄
                        int s = applyTotalTime - TTIME;
                        for (int i = oldApplyDay.Rows.Count - 1; i > 0; i--)
                        {
                            if (s > int.Parse(oldApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()))
                            {
                                s = s - int.Parse(oldApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe());
                                oldApplyDay.Rows.Remove(oldApplyDay.Rows[i]);
                            }
                            else if (s == int.Parse(oldApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()))
                            {
                                oldApplyDay.Rows.Remove(oldApplyDay.Rows[i]);
                                break;
                            }
                            else
                            {
                                oldApplyDay.Rows[i]["MZ_RESTHOUR"] = int.Parse(oldApplyDay.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()) - s;
                                //紀錄LOG
                                SetLog_MZ_RESTHOUR_DT(oldApplyDay.Rows[i], "check_15_Hour");
                                break;
                            }
                        }

                        Session["MZ_RESTHOUR_DT"] = oldApplyDay;
                        BT_GV1_Click(BT_GV1, EventArgs.Empty);
                    }
                    else
                    {
                        //未修改時數
                        Session["MZ_RESTHOUR_DT"] = oldApplyDay;
                    }
                }
                //新增差假
                else
                {
                    if (Session["MZ_RESTHOUR_DT"] != null)
                    {
                        DataTable dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                        //需要時數
                        int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 * 60 +
                            int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text) * 60;
                        //int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8  +
                        //    int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);
                        //統計已選加班時數
                        int totaltime = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            totaltime += int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());
                        }

                        if (TTIME > totaltime)
                        {
                            TextBox_MZ_TDAY.Text = "0";
                            TextBox_MZ_TTIME.Text = "0";

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                            return false;
                        }
                        else if (TTIME < totaltime)
                        {
                            //刪除多餘選擇休假
                            int s = totaltime - TTIME;
                            for (int i = dt.Rows.Count - 1; i > 0; i--)
                            {
                                if (s > int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()))
                                {
                                    s = s - int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe());
                                    dt.Rows.Remove(dt.Rows[i]);
                                }
                                else if (s == int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()))
                                {
                                    dt.Rows.Remove(dt.Rows[i]);
                                    break;
                                }
                                else
                                {
                                    dt.Rows[i]["MZ_RESTHOUR"] = int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()) - s;
                                    //紀錄LOG
                                    SetLog_MZ_RESTHOUR_DT(dt.Rows[i], "check_15_Hour");
                                    break;
                                }
                            }

                            //重新儲存已選補休日及時數
                            Session["MZ_RESTHOUR_DT"] = dt;
                            Session["CHECK"] = "true";
                            //按鈕(隱): 自動填寫請假事由
                            BT_GV1_Click(BT_GV1, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇可補休日！')", true);
                        return false;
                    }
                }
            }
            //通過檢查
            return true;
        }
        /// <summary>
        /// 申請補休額外處理
        /// </summary>
        protected void INSERT_15()
        {
            if (TextBox_MZ_CODE.Text == "11" || TextBox_MZ_CODE.Text == "22" || TextBox_MZ_CODE.Text == "16")
            {
                if (Session["MZ_RESTHOUR_DT"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                    int ITIME1 = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 +
                        int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        var MZ_RESTHOUR = dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe();

                        string strsql = string.Format(@"SELECT REST_DATE_RECORD FROM C_OVERTIME_BASE Where MZ_ID='{0}' And OVER_DAY='{1}' and OVERTIME_type = '{2}'", TextBox_MZ_ID.Text, dt.Rows[i]["OVER_DAY"].ToStringNullSafe(), dt.Rows[i]["OVER_TYPE"].ToStringNullSafe());
                        string REST_DATE_RECORD = o_DBFactory.ABC_toTest.vExecSQL(strsql);

                        if (!string.IsNullOrEmpty(REST_DATE_RECORD.Trim()))
                        {
                            REST_DATE_RECORD += TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + MZ_RESTHOUR + "，";
                        }
                        else
                        {
                            REST_DATE_RECORD = TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + MZ_RESTHOUR + "，";

                        }


                        switch (dt.Rows[i]["OVER_TYPE"].ToStringNullSafe())
                        {
                            case "OTB":
                            case "OTT":
                            case "OTD":
                            case "OTU":
                                //新加班補休
                                {
                                    //更新加班補休
                                    string upSQL = string.Format(@"UPDATE C_OVERTIME_BASE Set REST_HOUR=REST_HOUR + {0},REST_ID='{1}',REST_DATE=SYSDATE ,REST_DATE_RECORD='{2}' Where MZ_ID='{3}' And OVER_DAY='{4}' and OVERTIME_type = '{5}'",
                                        int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()) ? "0" : dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()),
                                        Session["ADPMZ_ID"].ToStringNullSafe(), REST_DATE_RECORD, TextBox_MZ_ID.Text, dt.Rows[i]["OVER_DAY"].ToStringNullSafe(), dt.Rows[i]["OVER_TYPE"].ToStringNullSafe());
                                    o_DBFactory.ABC_toTest.DealCommandLog(upSQL, null);
                                    //同步剩餘時數
                                    C_OVERTIME_BASE_Model model = new C_OVERTIME_BASE_Model()
                                    {
                                        MZ_ID = TextBox_MZ_ID.Text,
                                        OVER_DAY = dt.Rows[i]["OVER_DAY"].ToStringNullSafe(),
                                        REST_HOUR = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe()) ? "0" : dt.Rows[i]["MZ_RESTHOUR"].ToStringNullSafe())
                                    };
                                    CFService.SynchronizeOverTime(model, Request.QueryString["TPM_FION"].ToStringNullSafe());

                                    //新增參照檔C_OVERTIME_FOR_DLTB01

                                    //根據 人員ID 加班日期抓取資料,不過這樣似乎會抓錯? 應該要加上
                                    string strSQL = @"Select SN From C_OVERTIME_BASE Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY and OVERTIME_type =@OVERTIME_TYPE ";
                                    List<SqlParameter> parameters = new List<SqlParameter>()
                                    {
                                        new SqlParameter("MZ_ID", TextBox_MZ_ID.Text),
                                        new SqlParameter("OVER_DAY", dt.Rows[i]["OVER_DAY"].ToStringNullSafe()),
                                        new SqlParameter("OVERTIME_TYPE", dt.Rows[i]["OVER_TYPE"].ToStringNullSafe())
                                    };
                                    //根據指定的加班日期和員警ID,查詢中有哪些加班資料的PK?
                                    DataTable cobDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                                    if (cobDt != null && cobDt.Rows.Count > 0)
                                    {
                                        //檢查PK資料是否存在
                                        strSQL = string.Format(@"Select COUNT(*) From C_OVERTIME_FOR_DLTB01 Where OVERTIME_SN='{0}' And MZ_DLTB01_SN='{1}' ",
                                            cobDt.Rows[0]["SN"].ToStringNullSafe(), ViewState["SN"].ToStringNullSafe());
                                        if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, null) != "0")
                                        {
                                            //更新該參照檔
                                            strSQL = "UPDATE C_OVERTIME_FOR_DLTB01 Set REST_HOUR=::REST_HOUR Where OVERTIME_SN=@OVERTIME_SN And MZ_DLTB01_SN=@MZ_DLTB01_SN ";
                                        }
                                        else
                                        {
                                            //新增參照檔
                                            strSQL = "INSERT INTO C_OVERTIME_FOR_DLTB01(OVERTIME_SN, MZ_DLTB01_SN, REST_HOUR) Values(@OVERTIME_SN, @MZ_DLTB01_SN, @REST_HOUR) ";
                                        }
                                        parameters = new List<SqlParameter>()
                                        {
                                            new SqlParameter("REST_HOUR", model.REST_HOUR),
                                            new SqlParameter("OVERTIME_SN", cobDt.Rows[0]["SN"].ToStringNullSafe()),
                                            new SqlParameter("MZ_DLTB01_SN", ViewState["SN"].ToStringNullSafe())
                                        };
                                        o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters);

                                        LogModel.saveLog("COHI", "U", strSQL, parameters, Request.QueryString["TPM_FION"], "申請補休額外處理。");
                                    }
                                    else
                                    {
                                        LogModel.saveLog("COHI", "U", strSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "申請補休額外處理異常，無加班請假資料。");
                                    }
                                }
                                break;
                            default:
                                //舊加班補休
                                {
                                    //2021/03/26 dt.Rows[i]["MZ_DATE"].ToStringNullSafe() 無此欄位 改 dt.Rows[i]["OVER_DAY"].ToStringNullSafe()
                                    string selectSQL = string.Format(@"SELECT MZ_RESTDATE,MZ_RESTHOUR FROM C_OVERTIME_HOUR_INSIDE 
                                                                        WHERE MZ_DATE='{0}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{1}' AND MZ_RESTHOUR>0",
                                                                        dt.Rows[i]["OVER_DAY"].ToStringNullSafe(), TextBox_MZ_ID.Text);

                                    DataTable dt2 = new DataTable();
                                    dt2 = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getget");


                                    string MZ_RESDATE = "";

                                    string s = string.Empty;
                                    int hours = 0;
                                    if (dt2.Rows.Count > 0)
                                    {
                                        s = dt2.Rows[0]["MZ_RESTDATE"].ToString();

                                        hours = int.Parse(dt2.Rows[0]["MZ_RESTHOUR"].ToString());
                                    }
                                    int resthour_write;

                                    resthour_write = hours - ITIME1;

                                    if (resthour_write < 0)
                                    {
                                        ITIME1 = ITIME1 - hours;
                                    }

                                    if (!string.IsNullOrEmpty(s.Trim()))
                                    {
                                        if (resthour_write < 0)
                                        {
                                            MZ_RESDATE = s + "，" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + hours;
                                        }
                                        else
                                        {
                                            MZ_RESDATE = s + "，" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + ITIME1;
                                        }
                                    }
                                    else
                                    {
                                        if (resthour_write < 0)
                                        {
                                            MZ_RESDATE = TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + hours;
                                        }
                                        else
                                        {
                                            MZ_RESDATE = TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + ITIME1;
                                        }
                                    }

                                    int resthour;

                                    if (resthour_write < 0)
                                    {
                                        resthour = 0;
                                    }
                                    else
                                    {
                                        resthour = resthour_write;
                                    }

                                    //2021/03/26 dt.Rows[i]["MZ_DATE"].ToStringNullSafe() 無此欄位 改 dt.Rows[i]["OVER_DAY"].ToStringNullSafe()
                                    string updateSQL = string.Format(@"UPDATE C_OVERTIME_HOUR_INSIDE SET LOCK_FLAG='Y',MZ_RESTDATE='{0}',MZ_RESTHOUR={1} 
                                                                        WHERE MZ_DATE='{2}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{3}'",
                                                                        MZ_RESDATE, resthour, dt.Rows[i]["OVER_DAY"].ToString(), TextBox_MZ_ID.Text);
                                    //string updateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET LOCK_FLAG='Y',MZ_RESTDATE='" + MZ_RESDATE + "',MZ_RESTHOUR=" + resthour + " WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "'AND  dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text + "' "; //  AND MZ_RESTHOUR>0

                                    LogModel.saveLog("COHI", "U", updateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "新增修改假單，更新可補休日");

                                    o_DBFactory.ABC_toTest.Edit_Data(updateSQL);
                                }
                                break;
                        }
                    }

                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["s"] != null)
            {
                Panel_select_ModalPopupExtender.Show();
            }
            if (!IsPostBack)
            {
                C.check_power();//檢查權限

                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());//權限等級

                //預設局本部使用線上簽核
                if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382130000C" || Session["ADPMZ_EXAD"].ToStringNullSafe() == "382132500C")
                {
                    RadioButtonList_SIGN_KIND.SelectedValue = "2";
                }
            }

            ///群組權限
            //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            //查詢ID
            HttpCookie ForLeaveBasic_ID_Cookie = new HttpCookie("ForLeaveBasicSearch_ID");
            ForLeaveBasic_ID_Cookie = Request.Cookies["ForLeaveBasicSearch_ID"];

            if (ForLeaveBasic_ID_Cookie == null)
            {
                ViewState["ID"] = null;
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["ID"] = TPMPermissions._strDecod(ForLeaveBasic_ID_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            //查詢姓名
            HttpCookie ForLeaveBasic_NAME_Cookie = new HttpCookie("ForLeaveBasicSearch_NAME");
            ForLeaveBasic_NAME_Cookie = Request.Cookies["ForLeaveBasicSearch_NAME"];

            if (ForLeaveBasic_NAME_Cookie == null)
            {
                ViewState["NAME"] = null;
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["NAME"] = TPMPermissions._strDecod(ForLeaveBasic_NAME_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }

            //ViewState["MZ_EXAD"] = Request["MZ_EXAD"];
            //ViewState["MZ_EXUNIT"] = Request["MZ_EXUNIT"];
            ViewState["XCOUNT"] = Request["XCOUNT"];


            //ViewState["MZ_IDATE1"] = Request["MZ_IDATE1"];
            ViewState["SEARCHMODE"] = Request["SEARCHMODE"];

            if (ViewState["XCOUNT"] != null)
                ViewState["SEARCHMODE"] = "DLTB01";



            if (!IsPostBack)
            {
                //by MQ 20100312---------   

                C.set_Panel_EnterToTAB(ref this.Panel7);

                C.set_Panel_EnterToTAB(ref this.Panel2);

                if (ViewState["XCOUNT"] != null)//刪除或修改資料時！紀錄reload的時候該抓哪筆資料
                {
                    MultiView1.ActiveViewIndex = 0;
                    ViewState["SEARCHMODE"] = "DLTB01";
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    //判斷上筆下筆按鍵是否可按！
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && DLTB01_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < DLTB01_MZ_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == DLTB01_MZ_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (DLTB01_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DLTB01_MZ_ID.Count.ToString() + "筆";
                    }


                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btOK2.Enabled = false;
                    btCancel.Enabled = false;

                }
                else if (ViewState["ID"] == null)
                {
                    //預帶登入者相關資料（姓名 身分證號.....）



                    chk_Cardset(Session["ADPMZ_ID"].ToString());

                    //20140116
                    fill_person_data(Session["ADPMZ_ID"].ToString());//找個人資料


                    Count_hday(Session["ADPMZ_ID"].ToString());//計算休假天數及已休天數

                }
                else
                {
                    List<SqlParameter> listParms = new List<SqlParameter>();

                    if (ViewState["SEARCHMODE"].ToString() == "DLBASE")
                    {
                        string strSQL = "SELECT MZ_ID,MZ_EXAD,MZ_EXUNIT FROM A_DLBASE WHERE MZ_STATUS2='Y' ";

                        if (ViewState["ID"].ToString() != "")
                        {
                            strSQL += " AND MZ_ID=@ID ";
                            listParms.Add(new SqlParameter("ID", ViewState["ID"].ToStringNullSafe().Trim().ToUpper()));
                        }

                        if (ViewState["NAME"].ToString() != "")
                        {
                            strSQL += " AND MZ_NAME=@NAME ";
                            listParms.Add(new SqlParameter("NAME", ViewState["NAME"].ToStringNullSafe().Trim()));
                        }

                        if (Request["MZ_EXAD"] != "" && Request["MZ_EXUNIT"] != "")
                        {
                            strSQL += " AND MZ_EXAD =@MZ_EXAD AND MZ_EXUNIT=@MZ_EXUNIT ";
                            listParms.Add(new SqlParameter("MZ_EXAD", Request["MZ_EXAD"].ToStringNullSafe()));
                            listParms.Add(new SqlParameter("MZ_EXUNIT", Request["MZ_EXUNIT"].ToStringNullSafe()));
                        }
                        else if (Request["MZ_EXAD"] != "")
                        {
                            strSQL += " AND (MZ_EXAD=@MZ_EXAD OR MZ_AD=@MZ_EXAD OR PAY_AD=@MZ_EXAD) ";
                            listParms.Add(new SqlParameter("MZ_EXAD", Request["MZ_EXAD"].ToStringNullSafe()));
                        }

                        strSQL += " ORDER BY MZ_ID";

                        DLTB01_MZ_ID = o_DBFactory.ABC_toTest.Create_ListForField(strSQL, "MZ_ID", listParms.ToArray());
                        DLTB01_MZ_IDATE1 = o_DBFactory.ABC_toTest.Create_ListForField(strSQL, "MZ_ID", listParms.ToArray());
                        DLTB01_MZ_ITIME1 = o_DBFactory.ABC_toTest.Create_ListForField(strSQL, "MZ_ID", listParms.ToArray());


                        Session["DLTB01_MZ_ID"] = DLTB01_MZ_ID;//無法用VIEWSTATE
                        Session["DLTB01_MZ_IDATE1"] = DLTB01_MZ_IDATE1;
                        Session["DLTB01_MZ_ITIME1"] = DLTB01_MZ_ITIME1;

                        btInsert.Visible = true;
                        btUpdate.Visible = false;

                        rname_change("");
                    }
                    else
                    {
                        string strSQL = "SELECT C_DLTB01.MZ_ID,C_DLTB01.MZ_IDATE1,C_DLTB01.MZ_CODE,C_DLTB01.MZ_ITIME1 FROM C_DLTB01,A_DLBASE WHERE C_DLTB01.MZ_ID=A_DLBASE.MZ_ID";

                        if (ViewState["ID"].ToString() != "")
                        {
                            strSQL += " AND C_DLTB01.MZ_ID=@ID ";
                            listParms.Add(new SqlParameter("ID", ViewState["ID"].ToStringNullSafe().Trim().ToUpper()));
                        }

                        if (ViewState["NAME"].ToString() != "")
                        {
                            strSQL += " AND C_DLTB01.MZ_NAME=@NAME ";
                            listParms.Add(new SqlParameter("NAME", ViewState["NAME"].ToStringNullSafe().Trim()));
                        }

                        if (Request["MZ_EXAD"] != "" && Request["MZ_EXUNIT"] != "")
                        {
                            strSQL += " AND C_DLTB01.MZ_EXAD =@MZ_EXAD AND C_DLTB01.MZ_EXUNIT=@MZ_EXUNIT ";
                            listParms.Add(new SqlParameter("MZ_EXAD", Request["MZ_EXAD"].ToStringNullSafe()));
                            listParms.Add(new SqlParameter("MZ_EXUNIT", Request["MZ_EXUNIT"].ToStringNullSafe()));
                        }
                        else if (Request["MZ_EXAD"] != "")
                        {
                            strSQL += " AND (C_DLTB01.MZ_EXAD=@MZ_EXAD OR A_DLBASE.MZ_AD=@MZ_EXAD OR A_DLBASE.PAY_AD=@MZ_EXAD) ";
                            listParms.Add(new SqlParameter("MZ_EXAD", Request["MZ_EXAD"].ToStringNullSafe()));
                        }


                        if (Request["MZ_IDATE1"] != "")
                        {
                            strSQL += " AND C_DLTB01.MZ_IDATE1=@MZ_IDATE1 ";
                            listParms.Add(new SqlParameter("MZ_IDATE1", Request["MZ_IDATE1"].ToStringNullSafe().Trim()));
                        }
                        else
                        {
                            strSQL += " AND dbo.SUBSTR(MZ_IDATE1,1,3)>=@MZ_IDATE1 ";
                            listParms.Add(new SqlParameter("MZ_IDATE1", (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0')));
                        }

                        strSQL += " ORDER BY MZ_CHK1,MZ_IDATE1 DESC,C_DLTB01.MZ_EXUNIT,C_DLTB01.MZ_ID ";

                        DLTB01_MZ_ID = o_DBFactory.ABC_toTest.Create_ListForField(strSQL, "MZ_ID", listParms.ToArray());
                        DLTB01_MZ_IDATE1 = o_DBFactory.ABC_toTest.Create_ListForField(strSQL, "MZ_IDATE1", listParms.ToArray());
                        DLTB01_MZ_ITIME1 = o_DBFactory.ABC_toTest.Create_ListForField(strSQL, "MZ_ITIME1", listParms.ToArray());

                        Session["DLTB01_MZ_ID"] = DLTB01_MZ_ID;////無法用VIEWSTATE
                        Session["DLTB01_MZ_IDATE1"] = DLTB01_MZ_IDATE1;
                        Session["DLTB01_MZ_ITIME1"] = DLTB01_MZ_ITIME1;
                    }

                    MultiView1.ActiveViewIndex = 0;

                    if (ViewState["SEARCHMODE"].ToString() == "DLTB01")
                    {
                        Session["DLTB01_NAME"] = ViewState["NAME"];

                        Session["DLTB01_ID"] = ViewState["ID"];


                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeave_Search_Result.aspx?AD=" + (Request["MZ_EXAD"]) +
                                                                                                                                         "&UNIT=" + (Request["MZ_EXUNIT"]) +
                                                                                                                                         "&IDATE1=" + (Request["MZ_IDATE1"]) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin1','');", true);
                    }

                    if (DLTB01_MZ_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('C_ForLeaveBasic_New.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (DLTB01_MZ_ID.Count == 1)
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
                    //共幾筆資料
                    if (DLTB01_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DLTB01_MZ_ID.Count.ToString() + "筆";
                    }
                }
                MultiView1.ActiveViewIndex = 0;
                //20140317
                //Session["ForLeaveBasic_EXAD"] = o_A_DLBASE.PAD(TextBox_MZ_ID.Text);
                //Session["ForLeaveBasic_EXUNIT"] = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text);
                ChangeButtonColor(Panel_Button, DLTB_ADD);
                btSearch.Enabled = true;
                chk_TPMGroup();//群組權限

                C.controlEnable(ref this.Panel2, false);
                C.controlEnable(ref this.Panel7, false);

            }
        }

        protected void chk_Cardset(string MZ_ID)
        {
            string selectString = "SELECT MZ_CLOCK,MZ_OVERTIME FROM C_CARDSET WHERE MZ_ID='" + MZ_ID + "'";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(selectString, "GETCARDSET");

            if (tempDT.Rows.Count == 0)
            {
                DLTB_Clocking.Visible = false;
                // DLTB_RD.Visible = false;
            }
            else
            {
                if (tempDT.Rows[0]["MZ_CLOCK"].ToString() == "N" || string.IsNullOrEmpty(tempDT.Rows[0]["MZ_CLOCK"].ToString().Trim()))
                {
                    DLTB_Clocking.Visible = false;
                    DLTB_UNUSUAL.Visible = false;
                }
                else
                {
                    DLTB_Clocking.Visible = true;
                    DLTB_Clocking.Visible = true;
                }
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {

            switch (ViewState["C_strGID"].ToString())
            {
                case "A":
                case "B":
                case "C":
                    btOK2.Visible = true;
                    break;
                case "D":
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        Panel6.Visible = true;
                    }
                    else if (MultiView1.ActiveViewIndex == 1)
                    {
                        Panel6.Visible = false;
                    }
                    else if (MultiView1.ActiveViewIndex == 2)
                    {
                        Panel6.Visible = false;
                    }
                    else if (MultiView1.ActiveViewIndex == 3)
                    {
                        Panel6.Visible = false;
                    }
                    else if (MultiView1.ActiveViewIndex == 4)
                    {
                        Panel6.Visible = false;
                    }

                    if (TextBox_MZ_ID.Text.Trim() != Session["ADPMZ_ID"].ToString())
                    {
                        DLTB_RD.Visible = false;
                        DLTB_Clocking.Visible = false;
                        DLTB_DAYS.Visible = false;
                        DLTB_UNUSUAL.Visible = false;
                    }
                    else
                    {
                        DLTB_RD.Visible = true;
                        DLTB_Clocking.Visible = true;
                        DLTB_DAYS.Visible = true;
                        DLTB_UNUSUAL.Visible = true;
                    }
                    btOK2.Visible = false;
                    break;
                case "E":
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        Panel6.Visible = true;
                    }
                    else if (MultiView1.ActiveViewIndex == 1)
                    {
                        Panel6.Visible = false;
                    }
                    else if (MultiView1.ActiveViewIndex == 2)
                    {
                        Panel6.Visible = false;
                    }
                    else if (MultiView1.ActiveViewIndex == 3)
                    {
                        Panel6.Visible = false;
                    }
                    else if (MultiView1.ActiveViewIndex == 4)
                    {
                        Panel6.Visible = false;
                    }
                    btOK2.Visible = false;


                    break;
            }
        }

        /// <summary>
        /// 查詢資料(由基本資料帶入該查詢條件所有的人員)
        /// </summary>
        /// <param name="dataCount">第幾筆</param>
        /// 
        protected void finddata(int dataCount)
        {

            DLTB01_MZ_ID = Session["DLTB01_MZ_ID"] as List<String>;////無法用VIEWSTATE
            DLTB01_MZ_IDATE1 = Session["DLTB01_MZ_IDATE1"] as List<String>;
            DLTB01_MZ_ITIME1 = Session["DLTB01_MZ_ITIME1"] as List<String>;

            string aaa = DLTB01_MZ_ID[dataCount].ToString();
            //20140116
            fill_person_data(DLTB01_MZ_ID[dataCount].ToString());

            chk_Cardset(DLTB01_MZ_ID[dataCount].ToString());
            // Joy 修正 20150707 
            DLTB_DAYS_COUNT(DLTB01_MZ_ID[dataCount].ToString());


            if (MultiView1.ActiveViewIndex == 0)
            {
                rname_change("");
                Count_hday(DLTB01_MZ_ID[dataCount].ToString());


                if (DLTB01_MZ_IDATE1 != null)
                {
                    //查詢資料(由請假資料帶入該查詢條件所有的人員)
                    finddata2(DLTB01_MZ_IDATE1[dataCount], DLTB01_MZ_ID[dataCount], DLTB01_MZ_ITIME1[dataCount]);
                }
                else
                {
                    CheckBox_MZ_SWT.Checked = false;

                    TextBox_MZ_CODE.Text = string.Empty;
                    TextBox_MZ_CAUSE.Text = string.Empty;
                    TextBox_MZ_MEMO.Text = string.Empty;
                    TextBox_MZ_TADD.Text = string.Empty;
                    TextBox_MZ_TDAY.Text = string.Empty;
                    TextBox_MZ_TTIME.Text = string.Empty;
                    DropDownList_MZ_RNAME.SelectedValue = string.Empty;
                    TextBox_MZ_ROCCC.Text = string.Empty;
                    TextBox_MZ_CHK1.Text = string.Empty;
                    TextBox_MZ_LastYearJobLocation.Text = string.Empty;
                }

                chk_TPMGroup();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                Card_recoder();
            }
            else if (MultiView1.ActiveViewIndex == 3)
            {
                DataTable tempDT = new DataTable();

                tempDT = Count_Card_Record(TextBox_MZ_ID.Text.Trim());

                tempDT = tempDT.AsEnumerable().OrderByDescending(dr => dr["LOGDATE"]).CopyToDataTable();

                GridView_DLTB_UNUSUAL.DataSource = tempDT;

                Session["GridView_DLTB_UNUSUAL"] = tempDT;

                GridView_DLTB_UNUSUAL.AllowPaging = true;

                GridView_DLTB_UNUSUAL.PageSize = 14;

                GridView_DLTB_UNUSUAL.DataBind();
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                DLTB_DAYS_COUNT(TextBox_MZ_ID.Text);
            }
        }
        /// <summary>
        ///  查詢資料(由請假資料帶入該查詢條件所有的人員)
        /// </summary>
        /// <param name="MZ_IDATE1">請假日期</param>
        /// <param name="MZ_ID">身分證號</param>
        /// 
        protected void finddata2(string MZ_IDATE1, string MZ_ID, string MZ_ITIME1)
        {
            //string strSQL = "SELECT * FROM C_DLTB01 WHERE MZ_ID='" + MZ_ID + "' AND MZ_IDATE1='" + MZ_IDATE1 + "' AND MZ_ITIME1='" + MZ_ITIME1 + "'";
            string strSQL = "SELECT * FROM VW_C_DLTB01 WHERE MZ_ID='" + MZ_ID + "' AND MZ_IDATE1='" + MZ_IDATE1 + "' AND MZ_ITIME1='" + MZ_ITIME1 + "'";
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //Log
            LogModel.saveLog("C", "S", strSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "查詢假單資料");


            if (dt.Rows.Count == 1)
            {
                TextBox_MZ_CODE.Text = dt.Rows[0]["MZ_CODE"].ToString().Trim().ToUpper();
                TextBox_MZ_SYSDAY.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_SYSDAY"].ToString().Trim().ToUpper());
                TextBox_MZ_IDATE1.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_IDATE1"].ToString().Trim().ToUpper());
                TextBox_MZ_ITIME1.Text = dt.Rows[0]["MZ_ITIME1"].ToString().Trim().ToUpper();
                TextBox_MZ_ODATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_ODATE"].ToString().Trim().ToUpper());
                TextBox_MZ_OTIME.Text = dt.Rows[0]["MZ_OTIME"].ToString().Trim().ToUpper();
                TextBox_MZ_TDAY.Text = dt.Rows[0]["MZ_TDAY"].ToString().Trim().ToUpper();
                TextBox_MZ_TTIME.Text = dt.Rows[0]["MZ_TTIME"].ToString().Trim().ToUpper();
                TextBox_MZ_CAUSE.Text = dt.Rows[0]["MZ_CAUSE"].ToString().Trim().ToUpper();
                TextBox_MZ_MEMO.Text = dt.Rows[0]["MZ_MEMO"].ToString().Trim().ToUpper();

                TextBox_MZ_CHK1.Text = dt.Rows[0]["MZ_CHK1"].ToString().Trim().ToUpper();

                //顯示類型與事實發生日
                Update_Textchange(MZ_ID, MZ_IDATE1, MZ_ITIME1, false);

                //20140310 ki
                string[] ABDays = dt.Rows[0]["MZ_TADD"].ToString().Trim().ToUpper().Split('$');
                TextBox_MZ_TADD.Text = ABDays[0].ToString().Trim();
                if (dt.Rows[0]["MZ_CHINA"].ToString() == "Y" || dt.Rows[0]["MZ_FOREIGN"].ToString() == "Y")
                {
                    if (ABDays.Length > 1)
                    {

                        TextBox_AB_IDATE.Text = ABDays[1].ToString().Trim();
                        TextBox_AB_ITIME.Text = ABDays[2].ToString().Trim();

                        TextBox_AB_ODATE.Text = ABDays[3].ToString().Trim();
                        TextBox_AB_OTIME.Text = ABDays[4].ToString().Trim();
                        TextBox_AB_TDAY.Text = ABDays[5].ToString().Trim();
                        TextBox_AB_TTIME.Text = ABDays[6].ToString().Trim();

                    }
                }

                //20141210
                if (dt.Rows[0]["MZ_SWT"].ToString().Trim() == "Y")
                    CheckBox_MZ_SWT.Checked = true;

                ViewState["SN"] = dt.Rows[0]["MZ_DLTB01_SN"].ToString().Trim();

                //附件控制項設定

                string path = dt.Rows[0]["MZ_FILE"].ToString();
                string path1 = dt.Rows[0]["MZ_FILE1"].ToString();
                string path2 = dt.Rows[0]["MZ_FILE2"].ToString();

                if (path != "")
                {
                    HyperLink_FILENAME1.Visible = true;
                    HyperLink_FILENAME1.NavigateUrl = show_upload_path() + path;
                    Button_DelFILE1.Visible = true;
                    FileUpload1.Visible = false;
                }
                else
                {
                    HyperLink_FILENAME1.Visible = false;
                    Button_DelFILE1.Visible = false;
                    FileUpload1.Visible = true;
                }


                if (path1 != "")
                {
                    HyperLink_FILENAME2.Visible = true;
                    HyperLink_FILENAME2.NavigateUrl = show_upload_path() + path1;
                    Button_DelFILE2.Visible = true;
                    FileUpload2.Visible = false;
                }
                else
                {
                    HyperLink_FILENAME2.Visible = false;
                    Button_DelFILE2.Visible = false;
                    FileUpload2.Visible = true;
                }

                if (path2 != "")
                {
                    HyperLink_FILENAME3.Visible = true;
                    HyperLink_FILENAME3.NavigateUrl = show_upload_path() + path2;
                    Button_DelFILE3.Visible = true;
                    FileUpload3.Visible = false;
                }
                else
                {
                    HyperLink_FILENAME3.Visible = false;
                    Button_DelFILE3.Visible = false;
                    FileUpload3.Visible = true;
                }

                try
                {
                    DropDownList_MZ_RNAME.DataBind();
                    DropDownList_MZ_RNAME.SelectedValue = dt.Rows[0]["MZ_RID"].ToString(); //o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME='" + dt.Rows[0]["MZ_RNAME"].ToString().Trim().ToUpper() + "' AND MZ_OCCC='" + dt.Rows[0]["MZ_ROCCC"].ToString().Trim().ToUpper() + "'");
                }
                catch
                {
                    //20140317
                    //DropDownList_MZ_RNAME.DataBind();          
                    //DropDownList_MZ_RNAME.Items.Insert(0, o_A_DLBASE.CNAME(dt.Rows[0]["MZ_RNAME"].ToString().Trim().ToUpper()));
                    //DropDownList_MZ_RNAME.SelectedItem.Text = o_A_DLBASE.CNAME(dt.Rows[0]["MZ_RNAME"].ToString().Trim().ToUpper());
                }

                if (string.IsNullOrEmpty(dt.Rows[0]["SIGN_KIND"].ToString().Trim()))
                {
                    RadioButtonList_SIGN_KIND.SelectedValue = "1";
                }
                else
                {
                    RadioButtonList_SIGN_KIND.SelectedValue = dt.Rows[0]["SIGN_KIND"].ToString();
                }

                TextBox_MZ_ROCCC.Text = dt.Rows[0]["MZ_ROCCC_CH"].ToString().Trim();

                TextBox_MZ_SYSTIME.Text = dt.Rows[0]["MZ_SYSTIME"].ToString().Trim().ToUpper();

                TextBox_MZ_CODE1.Text = dt.Rows[0]["MZ_CODE_CH"].ToString().Trim();

                //是否有勾選前往大陸地區
                if (dt.Rows[0]["MZ_CHINA"].ToString() == "Y")
                {
                    CheckBox_MZ_CHINA.Checked = true;

                    //20140312 Ting
                    AB_DAY.Visible = true;
                    tableLastYearJobLocation.Visible = true; //顯示最近一年工作職掌
                    TextBox_MZ_LastYearJobLocation.Text = dt.Rows[0]["MZ_LastYearJobLocation"].ToString();
                }
                else
                {
                    //20140312 Ting
                    AB_DAY.Visible = false;
                    CheckBox_MZ_CHINA.Checked = false;
                }

                if (dt.Rows[0]["MZ_FOREIGN"].ToString() == "Y")
                {
                    //20140312 Ting
                    AB_DAY.Visible = true;
                    CheckBox_MZ_FOREIGN.Checked = true;
                }
                else
                {
                    //20140312 Ting
                    AB_DAY.Visible = false;
                    CheckBox_MZ_FOREIGN.Checked = false;
                }
                //判斷刪除和編輯按鈕是否解鎖?

                //如果搜尋模式為 "DLBASE" 就鎖定 刪除和編輯按鈕
                if (ViewState["SEARCHMODE"].ToString() == "DLBASE")
                {
                    btDelete.Enabled = false;
                    btUpdate.Enabled = false;
                }
                else
                {
                    //如果審核過 則 鎖定 刪除和編輯按鈕
                    if (TextBox_MZ_CHK1.Text == "Y")
                    {
                        btDelete.Enabled = false;
                        btUpdate.Enabled = false;
                    }
                    //如果還沒審核過 且 權限等級在A~C 則 解鎖 刪除和編輯按鈕
                    else if (TextBox_MZ_CHK1.Text != "Y" && (ViewState["C_strGID"].ToString() == "A" || ViewState["C_strGID"].ToString() == "B" || ViewState["C_strGID"].ToString() == "C"))
                    {
                        btUpdate.Enabled = true;
                        btDelete.Enabled = true;
                    }
                    //否則 開放 編輯 禁止 刪除
                    else
                    {
                        btUpdate.Enabled = true;
                        btDelete.Enabled = false;
                    }
                    //當使用者人事管理系統的資料權限(A.MZ_POWER)為D或E時，
                    //如假別(C_DLTB01的MZ_CODE)為11、16、22，強制將「修改」及「刪除」按鍵lock住鎖定。
                    switch (ViewState["C_strGID"].ToString())
                    {
                        case "D":
                        case "E":
                            switch (this.TextBox_MZ_CODE.Text)
                            {
                                case "11":
                                case "16":
                                case "22":
                                    //鎖住按鈕
                                    this.btUpdate.Enabled = false;
                                    this.btDelete.Enabled = false;
                                    break;
                            }
                            break;
                    }

                }
            }
        }


        /// <summary>
        /// 新增預帶的日期、時間
        /// </summary>
        protected void preload()
        {
            string sysdate = o_CommonService.Personal_ReturnDateString((DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'));

            TextBox_MZ_SYSDAY.Text = sysdate;

            TextBox_MZ_SYSTIME.Text = DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0');

            TextBox_MZ_IDATE1.Text = sysdate;

            TextBox_MZ_ODATE.Text = sysdate;

            TextBox_MZ_ITIME1.Text = "0830";

            TextBox_MZ_OTIME.Text = "1730";
        }

        protected void Card_recoder()
        {
            DataTable temp = new DataTable();

            string LOGDATE = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Day.ToString().PadLeft(2, '0');

            string strSQL = "SELECT * FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + TextBox_MZ_ID.Text + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN' ";

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETCARDRECORDER");

            for (int i = 0; i < temp.Rows.Count; i++)
            {
                if (temp.Rows[i]["VERIFY"].ToString() == "IN")
                {
                    temp.Rows[i]["VERIFY"] = "成功";
                }

                if (temp.Rows[i]["FKEY"].ToString() == "NONE")
                {
                    temp.Rows[i]["FKEY"] = "";
                }
                else if (temp.Rows[i]["FKEY"].ToString() == "F3")
                {
                    temp.Rows[i]["FKEY"] = "加班下班";
                }
            }

            TBGridView1.DataSource = temp;

            Session["TBGridView1"] = temp;

            TBGridView1.AllowPaging = true;

            TBGridView1.PageSize = 14;

            TBGridView1.DataBind();
        }

        /// <summary>
        /// 計算休假天數及已休天數
        /// 調整計算公式為共用函數 20190124 by sky
        /// </summary>
        /// <param name="MZ_ID">身分證號</param>
        protected void Count_hday(string MZ_ID)
        {
            preload();//新增預帶的日期、時間

            TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString();


            //檢查差假基本檔（DLTBB）是否有資料,無資料直接撈人事資料塞一筆進去
            string mz = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT MZ_ID FROM C_DLTBB WHERE MZ_ID='{0}' AND MZ_YEAR='{1}' ", MZ_ID, TextBox_MZ_YEAR.Text.PadLeft(3, '0')));
            if (string.IsNullOrEmpty(mz))
            {
                #region 刪除該段程式，改用共用函數處理 20190124 by sky
                ////改為合格實授日計算休假日 20181120 by sky
                //string strSQL = "SELECT MZ_QUA_DATE,MZ_TYEAR,MZ_TMONTH FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

                //DataTable dt = new DataTable();

                //dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                //int DLTBB_hday;

                //string MZ_YEAR = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

                //int tyear = int.Parse(string.IsNullOrEmpty(dt.Rows[0]["MZ_TYEAR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_TYEAR"].ToString());

                //int tmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[0]["MZ_TMONTH"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_TMONTH"].ToString());

                //DateTime FDATE;

                //try
                //{
                //    //統一計算基準日為當月1號 20181120 by sky
                //    FDATE = DateTime.Parse((int.Parse(dt.Rows[0]["MZ_QUA_DATE"].ToString().Substring(0, 3)) + 1911).ToString() + "-" + dt.Rows[0]["MZ_QUA_DATE"].ToString().Substring(3, 2) + "-01");
                //}
                //catch
                //{
                //    FDATE = DateTime.Now;
                //}

                //int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Parse((int.Parse(TextBox_MZ_YEAR.Text) - 1 + 1911).ToString() + "-12-31").AddDays(-1), Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

                //int MONTH = monthDiff;

                //int DLTBB_htime = 0;

                //int addMONTH = tyear * 12 + tmonth;

                //if (MONTH < 0)
                //{
                //    DLTBB_hday = 0;
                //}
                //else
                //{
                //    if (MONTH < 12)
                //    {
                //        if (addMONTH == 0 && FDATE.Month == 1)
                //        {
                //            DLTBB_hday = 7;
                //        }
                //        else
                //        {
                //            if (addMONTH == 0)
                //            {
                //                double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

                //                string[] s = countsay.ToString().Split('.');

                //                DLTBB_hday = int.Parse(s[0]);

                //                if (s.Length == 2)
                //                {
                //                    if (int.Parse(s[1]) > 5)
                //                    {
                //                        DLTBB_hday = DLTBB_hday + 1;
                //                    }
                //                    else if (int.Parse(s[1]) > 0)
                //                    {
                //                        DLTBB_htime = 4;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (addMONTH + MONTH < 36)
                //                {
                //                    double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

                //                    string[] s = countsay.ToString().Split('.');

                //                    DLTBB_hday = int.Parse(s[0]);
                //                    if (s.Length == 2)
                //                    {
                //                        if (int.Parse(s[1]) > 5)
                //                        {
                //                            DLTBB_hday = DLTBB_hday + 1;
                //                        }
                //                        else if (int.Parse(s[1]) > 0)
                //                        {
                //                            DLTBB_htime = 4;
                //                        }
                //                    }
                //                }
                //                else if (36 <= addMONTH + MONTH && addMONTH + MONTH < 72)
                //                {
                //                    double countsay = MathHelper.Round(14 * double.Parse(MONTH.ToString()) / 12, 1);

                //                    string[] s = countsay.ToString().Split('.');

                //                    DLTBB_hday = int.Parse(s[0]);
                //                    if (s.Length == 2)
                //                    {
                //                        if (int.Parse(s[1]) > 5)
                //                        {
                //                            DLTBB_hday = DLTBB_hday + 1;
                //                        }
                //                        else if (int.Parse(s[1]) > 0)
                //                        {
                //                            DLTBB_htime = 4;
                //                        }
                //                    }
                //                }
                //                else if (72 <= addMONTH + MONTH && addMONTH + MONTH < 108)
                //                {
                //                    double countsay = MathHelper.Round(21 * double.Parse(MONTH.ToString()) / 12, 1);

                //                    string[] s = countsay.ToString().Split('.');

                //                    DLTBB_hday = int.Parse(s[0]);

                //                    if (s.Length == 2)
                //                    {
                //                        if (int.Parse(s[1]) > 5)
                //                        {
                //                            DLTBB_hday = DLTBB_hday + 1;
                //                        }
                //                        else if (int.Parse(s[1]) > 0)
                //                        {
                //                            DLTBB_htime = 4;
                //                        }
                //                    }
                //                }
                //                else if (108 <= addMONTH + MONTH && addMONTH + MONTH < 148)
                //                {
                //                    double countsay = MathHelper.Round(28 * double.Parse(MONTH.ToString()) / 12, 1);

                //                    string[] s = countsay.ToString().Split('.');

                //                    DLTBB_hday = int.Parse(s[0]);

                //                    if (s.Length == 2)
                //                    {
                //                        if (int.Parse(s[1]) > 5)
                //                        {
                //                            DLTBB_hday = DLTBB_hday + 1;
                //                        }
                //                        else if (int.Parse(s[1]) > 0)
                //                        {
                //                            DLTBB_htime = 4;
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    double countsay = MathHelper.Round(30 * double.Parse(MONTH.ToString()) / 12, 1);

                //                    string[] s = countsay.ToString().Split('.');

                //                    DLTBB_hday = int.Parse(s[0]);

                //                    if (s.Length == 2)
                //                    {
                //                        if (int.Parse(s[1]) > 5)
                //                        {
                //                            DLTBB_hday = DLTBB_hday + 1;
                //                        }
                //                        else if (int.Parse(s[1]) > 0)
                //                        {
                //                            DLTBB_htime = 4;
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    else if (12 <= MONTH + addMONTH && MONTH + addMONTH < 36)
                //    {
                //        DLTBB_hday = 7;
                //    }
                //    else if (36 <= MONTH + addMONTH && MONTH + addMONTH < 72)
                //    {
                //        DLTBB_hday = 14;
                //    }
                //    else if (72 <= MONTH + addMONTH && MONTH + addMONTH < 108)
                //    {
                //        DLTBB_hday = 21;
                //    }
                //    else if (108 <= MONTH + addMONTH && MONTH + addMONTH < 168)
                //    {
                //        DLTBB_hday = 28;
                //    }
                //    else
                //    {
                //        DLTBB_hday = 30;
                //    }
                //}

                //string InsertSQL = "INSERT INTO C_DLTBB(MZ_YEAR,MZ_ID,MZ_HDAY,MZ_HTIME) VALUES(@MZ_YEAR,@MZ_ID,@MZ_HDAY,@MZ_HTIME)";

                //SqlParameter[] parameterList ={
                //new SqlParameter("MZ_YEAR",SqlDbType.VarChar){Value=MZ_YEAR},
                //new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=MZ_ID},
                //new SqlParameter("MZ_HDAY",SqlDbType.Float){Value=DLTBB_hday},
                //new SqlParameter("MZ_HTIME",SqlDbType.Float){Value=DLTBB_htime},
                //};

                //o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertSQL, parameterList);

                ////Log
                //LogModel.saveLog("C", "A", strSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "新增休假檔");
                #endregion

                if (!string.IsNullOrEmpty(MZ_ID)) //確保不會空值導致全人員計算
                {
                    C_ForLeave_YearVacation_Create.ComputeModel computeModel = new C_ForLeave_YearVacation_Create.ComputeModel();
                    computeModel.MZ_ID = MZ_ID;
                    computeModel.StatisticsYear = TextBox_MZ_YEAR.Text.PadLeft(3, '0');
                    if (Request.QueryString["TPM_FION"] != null)
                    {
                        computeModel.TPM_FION = Request.QueryString["TPM_FION"].ToString();
                    }

                    string errorMsg = string.Empty;
                    C_ForLeave_YearVacation_Create.YearVacation(computeModel, ref errorMsg);
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + errorMsg + "');", true);
                    }
                }
            }

            int hday_used;

            int htime_used;

            int hday = 0;
            int htime = 0;

            //20140313
            C_COUNT_HDAY COUNT = new C_COUNT_HDAY() { r = MZ_ID };
            List<int> count_result = COUNT.COUNT_HDAY;
            hday_used = count_result[0];
            htime_used = count_result[1];
            hday = count_result[2];
            htime = count_result[3];

            TextBox_HDAY.Text = hday.ToString();
            TextBox_HTIME.Text = htime.ToString();



            TextBox_USED_HDAY.Text = hday_used.ToString();
            TextBox_USED_HTIME.Text = htime_used.ToString();


        }
        /// <summary>
        /// 按鈕變色
        /// </summary>
        /// <param name="PA1">導覽列所在位置</param>
        /// <param name="bn1">要變色的按鈕</param>
        protected void ChangeButtonColor(Panel PA1, Button bn1)
        {
            foreach (object bn in PA1.Controls)
            {
                if (bn is Button)
                {
                    Button button = bn as Button;
                    button.BackColor = Color.Empty;
                    button.ForeColor = Color.Maroon;
                }
            }
            bn1.BackColor = Color.Pink;
            bn1.ForeColor = Color.Red;
        }
        /// <summary>
        /// 控制功能列是否可按
        /// </summary>
        protected void CHANGEBOTTONKIND()
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                btSearch.Enabled = true;
                btInsert.Enabled = true;
            }
            else
            {
                btInsert.Enabled = false;
                btSearch.Enabled = false;
                btUpdate.Enabled = false;
            }

            btOK.Enabled = false;
            btOK2.Enabled = false;
            btCancel.Enabled = false;
            btDelete.Enabled = false;

        }

        protected void DLTB_ADD_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            ChangeButtonColor(Panel_Button, DLTB_ADD);
            CHANGEBOTTONKIND();
            chk_TPMGroup();
        }

        protected void DLTB_RD_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
            ChangeButtonColor(Panel_Button, DLTB_RD);
            CHANGEBOTTONKIND();
            chk_TPMGroup();
            GridView1.DataBind();
        }

        //TODO與4.18報表用的公式雷同,看能不能合併
        public static DataTable Count_Card_Record(string MZ_ID)
        {
            DataTable tempDT = new DataTable();

            tempDT.Columns.Clear();
            tempDT.Columns.Add("LOGDATE", typeof(string));
            tempDT.Columns.Add("INTIME", typeof(string));
            tempDT.Columns.Add("OUTTIME", typeof(string));
            tempDT.Columns.Add("CODE", typeof(string));
            tempDT.Columns.Add("KIND", typeof(string));
            tempDT.Columns.Add("MEMO", typeof(string));
            //正常的上下班時間,用int表示,相當於把時間格式的:拔掉,EX: 09:01:00 => 90100
            int INTIME = 90100, OUTTIME = 170000;

            for (int i = 1; i <= DateTime.Now.Day; i++)
            {
                string LOGDATESTRING = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + i.ToString().PadLeft(2, '0');

                DateTime LOGDATE = DateTime.Parse(LOGDATESTRING);

                string LOGDATE1 = (LOGDATE.Year - 1911).ToString().PadLeft(3, '0') + LOGDATE.Month.ToString().PadLeft(2, '0') + LOGDATE.Day.ToString().PadLeft(2, '0');

                List<String> temp = new List<string>();
                //計算未刷卡當天是否有請假
                temp = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATESTRING, 1);

                if (LOGDATE >= Convert.ToDateTime("2021/05/17") && LOGDATE <= Convert.ToDateTime("2021/07/26"))
                {
                    INTIME = 100100;
                    OUTTIME = 160000;
                }

                if (LOGDATE >= Convert.ToDateTime("2021/07/27") && LOGDATE <= Convert.ToDateTime("2022/12/31"))
                {
                    INTIME = 093100;
                    OUTTIME = 163000;
                }
                //抓取當天簽到退時間,這邊是用最早和最晚的紀錄判斷,可能是因為打卡機沒有上下班的區別?
                string selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATESTRING + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                string selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATESTRING + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                //string selectINOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");
                string selectOUTOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATESTRING + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");


                //20140120
                string KIND = "";
                string MEMO = "";
                string CODE = "";


                // 當天無刷卡紀錄
                if (string.IsNullOrEmpty(selectINTIME.Trim()) && string.IsNullOrEmpty(selectOUTTIME.Trim()))
                {
                    // 當天為周休二日
                    if (LOGDATE.DayOfWeek == DayOfWeek.Saturday || LOGDATE.DayOfWeek == DayOfWeek.Sunday)
                    {

                        MEMO = "例假日";
                    }
                    // 當天為正常上班日，且無請假紀錄 
                    else if (temp.Count == 0) ///無刷卡狀態(temp為當天請假紀錄)
                    {
                        // Today
                        if (LOGDATE.Year.ToString() + LOGDATE.Month.ToString() + LOGDATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                        {
                            int NOWTIME = int.Parse(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'));

                            if (NOWTIME <= INTIME)///本日九點前
                            {
                                KIND = "";
                                MEMO = "";
                            }
                            else if (NOWTIME > INTIME)///本日九點後
                            {
                                KIND = "上班未刷卡";
                                MEMO = "上班異常";
                            }
                        }
                        else///過本日後
                        {
                            KIND = "未刷卡";
                            MEMO = "上班異常";
                        }
                    }
                    // 當天 非周休二日 且 有請假紀錄
                    else
                    {
                        if (temp.Count > 1)///有請假
                        {
                            CODE = temp[0];

                            if (int.Parse(LOGDATE1) >= int.Parse(temp[1]) && int.Parse(LOGDATE1) <= int.Parse(temp[3]) && int.Parse(temp[6]) > 0)///正常
                            {
                                KIND = "";
                                MEMO = "請假";
                            }
                            else
                            {
                                KIND = "未刷卡";
                                MEMO = "上班異常";
                            }
                        }
                        else if (temp.Count == 1)
                        {
                            KIND = "";
                            MEMO = "國定假日";
                        }
                    }
                }
                //else 當天有刷卡紀錄
                else
                {

                    // 當天為周休二日
                    if (LOGDATE.DayOfWeek == DayOfWeek.Saturday || LOGDATE.DayOfWeek == DayOfWeek.Sunday)
                    {
                        MEMO = "例假日";
                    }
                    // 目前系統時間尚在當天(相當於還沒下班就查人家出退勤)
                    else if (LOGDATE.Year.ToString() + LOGDATE.Month.ToString() + LOGDATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                    {
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///當日刷9點前的卡
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料而且無請假
                            {
                                KIND = "";
                                MEMO = "";
                                selectOUTTIME = "";
                            }
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只有一筆刷卡資料而且有請假
                            {
                                selectOUTTIME = "";

                                CODE = temp[0];
                                //打卡日期+請假起時
                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + temp[2] + ":00");///跟請假起時比

                                if (DateTime.Now > TS1)///超過就算下班未刷卡
                                {
                                    selectOUTTIME = "";
                                    KIND = "下班未刷卡";
                                    MEMO = "上班異常";
                                }

                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)
                            {
                                KIND = "";
                                MEMO = "";
                                selectOUTTIME = "";
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班時間少於17點
                            {
                                if (temp.Count > 1)///有請假
                                {
                                    CODE = temp[0];
                                    //似乎有bug
                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        MEMO = "請假";
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else ///下班時間大於17點
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];
                                }

                                DateTime TS1 = DateTime.Parse(LOGDATESTRING + " " + selectINTIME);

                                DateTime TS2 = DateTime.Parse(LOGDATESTRING + " " + selectOUTTIME);

                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours > 8)
                                {
                                    MEMO = "";
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                        }
                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///當天刷卡時間大於9點1分
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡紀錄
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)///記錄大於17點
                                {
                                    KIND = "上班未刷卡";
                                    MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                                else
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只有一筆刷卡紀錄有請假
                            {
                                CODE = temp[0];

                                if (int.Parse(temp[2].Replace(":", "")) <= 900 && int.Parse(temp[4].Replace(":", "")) <= 1330)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)///未超過1330刷卡
                                    {
                                        KIND = "";
                                        MEMO = "";
                                        selectOUTTIME = "";
                                    }
                                    else///超過1330
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else if (int.Parse(temp[2].Replace(":", "")) <= 900)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(temp[4].Replace(":", string.Empty) + "00"))
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) > 173000)
                                        {
                                            KIND = "上班未刷卡";
                                            MEMO = "上班異常";
                                            selectINTIME = "";
                                        }
                                        else
                                        {
                                            KIND = "下班未刷卡";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + temp[2] + ":00");

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS1 - TS2;

                                    if (TS.Hours + int.Parse(temp[6]) < 8)
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                    }
                                    else
                                    {
                                        KIND = "遲到早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    KIND = "遲到早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    MEMO = "";
                                }
                                else
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                }
                            }

                        }
                    }
                    // 當天 非周休二日 且 目前系統時間已經不在當天
                    else
                    {
                        //當日 刷卡時間未超過9點1分
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)
                        {
                            //如果 當天的刷卡起訖時間相同,且沒有請假,代表下班未刷卡
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料，且當日無請假紀錄
                            {
                                KIND = "下班未刷卡";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            // Joy 加入是否請假紀錄判斷
                            //如果 當天簽退時間比 表定上班時間早 且 沒有請假,代表 上班打卡兩次以上,但是下班未刷卡
                            else if ((int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME) && temp.Count == 0)///多筆資料都在九點前，且當日無請假紀錄
                            {
                                KIND = "下班未刷卡";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            //當天的簽退時間比 表定上班時間晚 且 又比表定下班時間5點早,代表早退
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班刷卡時間早於5點
                            {
                                //如果有請假,且
                                if (temp.Count > 1) ///有請假
                                {
                                    CODE = temp[0];
                                    //計算:簽到退的起迄時間差,也就是當天實際上班時數
                                    DateTime TS1 = DateTime.Parse(LOGDATESTRING + " " + selectINTIME);
                                    DateTime TS2 = DateTime.Parse(LOGDATESTRING + " " + selectOUTTIME);
                                    TimeSpan TS = TS2 - TS1;
                                    //如果:當天實際上班時數 + 當天的請假小時數 ,大於等於8
                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        MEMO = "下午請假";
                                    }
                                    //反之,確定是早退了
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                            //當天的簽退時間比 表定上班時間晚 且 又比表定下班時間5點晚
                            else
                            {
                                //計算:簽到退的起迄時間差,也就是當天實際上班時數
                                DateTime TS1 = DateTime.Parse(LOGDATESTRING + " " + selectINTIME);
                                DateTime TS2 = DateTime.Parse(LOGDATESTRING + " " + selectOUTTIME);
                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours > 8)
                                {
                                    MEMO = "";
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                        }
                        //當日 上班刷卡時間超過9點1分
                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///上班刷卡時間大於9點1分
                        {
                            //如果 上班刷卡時間超過9點1分 且 當日簽到退時間相同 且 沒有請假,代表應該是忘記刷上下班卡其中之一
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只一筆刷卡資料
                            {
                                //如果 上班刷卡時間晚於 應下班時間,代表上班未忘記刷卡
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)
                                {
                                    KIND = "上班未刷卡";
                                    MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                                //否則 代表 遲到又下班未刷卡
                                else
                                {
                                    KIND = "遲到下班未刷卡";
                                    MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                            }
                            //如果 上班刷卡時間超過9點1分 且  當日簽到退時間相同 且 有請假
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只一筆有請假
                            {
                                CODE = temp[0];
                                //如果 請假起訖時間剛好是 八點半和12點半
                                if (temp[2] == "08:30" && temp[4] == "12:30")
                                {

                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)
                                    {
                                        KIND = "下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                    else if (int.Parse(selectINTIME.Replace(":", string.Empty)) >= 173000)
                                    {
                                        KIND = "上班未刷卡";
                                        MEMO = "上班異常";
                                        selectINTIME = "";
                                    }
                                    else
                                    {
                                        KIND = "遲到下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }

                                }
                                //如果 請假起始時間 早於 早上九點
                                else if (int.Parse(temp[2].Replace(":", "")) <= 900)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(temp[4].Replace(":", string.Empty) + "00"))
                                    {
                                        KIND = "下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                    else
                                    {
                                        KIND = "遲到下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                //
                                else
                                {
                                    KIND = "遲到下班未刷卡";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            //如果 上班刷卡時間超過9點1分 且 簽到退時間不同 且 簽退時間早於 18點
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                            {
                                //如果 有請假
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATESTRING + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATESTRING + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        KIND = "";
                                        MEMO = "早上請假";
                                    }
                                    else
                                    {
                                        KIND = "遲到早退";
                                        //MEMO = "上班異常";   // 當天有請假紀錄，則不出現上班異常選項
                                    }
                                }
                                //如果 上班刷卡時間超過9點1分 且 簽到退時間不同 且 簽退時間晚於 18點
                                else
                                {
                                    KIND = "遲到早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    MEMO = "正常";
                                }
                                else
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                }
                            }
                        }
                        //當日根本沒有上班刷卡時間
                        else if (string.IsNullOrEmpty(selectINTIME.Trim()))
                        {
                            //如果 下班刷卡時間也早於17點 (照理說沒有上班卡就沒有下班卡,所以應該是沒差?)
                            if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)
                            {
                                KIND = "遲到早退上班未刷卡";

                                MEMO = "上班異常";
                            }
                            else
                            {
                                KIND = "遲到上班未刷卡";

                                MEMO = "上班異常";
                            }
                        }
                    }
                }

                DataRow dr = tempDT.NewRow();
                dr["LOGDATE"] = LOGDATESTRING;
                dr["INTIME"] = selectINTIME;
                dr["OUTTIME"] = selectOUTTIME;
                dr["CODE"] = CODE;
                dr["KIND"] = KIND;
                dr["MEMO"] = MEMO;

                tempDT.Rows.Add(dr);
            }

            return tempDT;
        }

        protected void DLTB_Clocking_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
            ChangeButtonColor(Panel_Button, DLTB_Clocking);
            CHANGEBOTTONKIND();
            chk_TPMGroup();
            Card_recoder();
        }

        protected void DLTB_UNUSUAL_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
            ChangeButtonColor(Panel_Button, DLTB_UNUSUAL);
            CHANGEBOTTONKIND();
            chk_TPMGroup();

            DataTable tempDT = new DataTable();

            tempDT = Count_Card_Record(TextBox_MZ_ID.Text.Trim());

            tempDT = tempDT.AsEnumerable().OrderByDescending(dr => dr["LOGDATE"]).CopyToDataTable();

            GridView_DLTB_UNUSUAL.DataSource = tempDT;

            GridView_DLTB_UNUSUAL.AllowPaging = true;

            Session["GridView_DLTB_UNUSUAL"] = tempDT;

            GridView_DLTB_UNUSUAL.PageSize = 14;

            GridView_DLTB_UNUSUAL.DataBind();
        }

        protected void DLTB_DAYS_COUNT(string MZ_ID)
        {
            TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

            string selectSQL = "SELECT MZ_FDATE, NVL(MZ_TYEAR,'0') MZ_TYEAR, NVL(MZ_TMONTH,'0') MZ_TMONTH, MZ_MEMO, NVL(MZ_RYEAR, '0') MZ_RYEAR, NVL(MZ_RMONTH, '0') MZ_RMONTH,MZ_QUA_DATE FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GET");

            int tyear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TYEAR"].ToString().Trim()) ? "0" : temp.Rows[0]["MZ_TYEAR"].ToString());

            int tmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TMONTH"].ToString().Trim()) ? "0" : temp.Rows[0]["MZ_TMONTH"].ToString());

            int ryear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_RYEAR"].ToString().Trim()) ? "0" : temp.Rows[0]["MZ_RYEAR"].ToString());

            int rmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_RMONTH"].ToString().Trim()) ? "0" : temp.Rows[0]["MZ_RMONTH"].ToString());

            string FDATE = temp.Rows[0]["MZ_FDATE"].ToString();

            if (string.IsNullOrEmpty(FDATE.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + o_A_DLBASE.CNAME(TextBox_MZ_ID.Text) + "初任公職日有誤請檢查！');", true);
                return;
            }
            else
            {
                TextBox_MZ_FDATE.Text = o_CommonService.Personal_ReturnDateString(FDATE);
            }

            //TextBox_MZ_TYEAR.Text = temp.Rows[0]["MZ_TYEAR"].ToString();

            //TextBox_MZ_TMONTH.Text = temp.Rows[0]["MZ_TMONTH"].ToString();

            TextBox_MZ_TMEMO.Text = temp.Rows[0]["MZ_MEMO"].ToString();

            //合格實授日欄位
            string QUADATE = temp.Rows[0]["MZ_QUA_DATE"].ToString();
            if (!string.IsNullOrEmpty(QUADATE))
            {
                TextBox_MZ_QUA_DATE.Text = o_CommonService.Personal_ReturnDateString(QUADATE);
            }

            //任公職年資月數
            System.DateTime dt1 = DateTime.Parse((int.Parse(FDATE.Substring(0, 3)) + 1911).ToString() + "-" + FDATE.Substring(3, 2) + "-" + FDATE.Substring(5, 2));

            System.DateTime dt2 = DateTime.Now;

            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth - ryear * 12 - rmonth;

            if (monthDiff < 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + o_A_DLBASE.CNAME(TextBox_MZ_ID.Text) + "初任公職日有誤請檢查！');", true);
                return;
            }

            TextBox_MZ_OFFYY.Text = (monthDiff / 12).ToString() + "." + (monthDiff % 12).ToString();

            if (TextBox_MZ_OFFYY.Text == "0.0")
            {
                TextBox_MZ_OFFYY.Text = "0";
            }

            TextBox_MZ_TYEAR.Text = (tyear - ryear).ToString();  //併計年資

            TextBox_MZ_TMONTH.Text = (tmonth - rmonth).ToString();  //併計年資月

            if (float.Parse(TextBox_MZ_OFFYY.Text.Trim()) < 4)
            {
                Panel_SDAY.Visible = false;
            }
            // Joy 修正 如果服務年資大於4則顯示
            else
            {
                Panel_SDAY.Visible = true;
            }

            string strSQL = "SELECT * FROM C_DLTBB WHERE MZ_ID='" + MZ_ID + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 1)
            {
                TextBox_MZ_SDAY.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SDAY"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SDAY"].ToString();
                TextBox_MZ_SDAY_HOUR.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SDAY_HOUR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SDAY_HOUR"].ToString();
                TextBox_MZ_SDAY2.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SDAY2"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SDAY2"].ToString();
                TextBox_MZ_SDAY2_HOUR.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SDAY2_HOUR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SDAY2_HOUR"].ToString();
                TextBox_MZ_HDAY.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_HDAY"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_hday"].ToString();
                TextBox_MZ_HTIME.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_HTIME"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_htime"].ToString();
                TextBox_MZ_PDAY.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_PDAY"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_PDAY"].ToString();
                TextBox_MZ_PHOUR.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_PHOUR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_PHOUR"].ToString();
                TextBox_MZ_SICKDAY.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SICKDAY"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SICKDAY"].ToString();
                TextBox_MZ_SICKHOUR.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SICKHOUR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SICKHOUR"].ToString();
                TextBox_MZ_HCAREDAY.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_HCAREDAY"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_HCAREDAY"].ToString();
                TextBox_MZ_HCAREHOUR.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_HCAREHOUR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_HCAREHOUR"].ToString();

                TextBox_MZ_SDAY3.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SDAY3"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SDAY3"].ToString();
                TextBox_MZ_SDAY3_HOUR.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_SDAY3_HOUR"].ToString().Trim()) ? "0" : dt.Rows[0]["MZ_SDAY3_HOUR"].ToString();

                TextBox_MZ_SDAY_CANUSE.Text = "0";
                TextBox_MZ_SDAY_HOUR_CANUSE.Text = "0";

                TextBox_MZ_SDAY2_CANUSE.Text = "0";
                TextBox_MZ_SDAY2_HOUR_CANUSE.Text = "0";
                TextBox_MZ_HDAY_CANUSE.Text = "0";
                TextBox_MZ_HTIME_CANUSE.Text = "0";
                //20140120
                int sday = 0;
                int sday_hour = 0;
                int sday2 = 0;
                int sday2_hour = 0;
                int hday = 0;
                int htime = 0;
                //希望再修改SQL句
                string strsql_COUNT = string.Format(@"SELECT * FROM  
                                                      (SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY11 ,nvl(SUM(MZ_TTIME),0) MZ_TTIME11 
                                                      FROM C_DLTB01 
                                                      WHERE MZ_CODE='31' AND MZ_ID='{0}' AND dbo.SUBSTR(MZ_IDATE1,1,3)='{1}') 
                                                      ,
                                                      (SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY22 ,nvl(SUM(MZ_TTIME),0) MZ_TTIME22
                                                      FROM C_DLTB01 
                                                      WHERE MZ_CODE='22' AND MZ_ID='{0}' AND dbo.SUBSTR(MZ_IDATE1,1,3)='{1}') 
                                                      ,
                                                      (SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY03 ,nvl(SUM(MZ_TTIME),0) MZ_TTIME03 
                                                      FROM C_DLTB01 
                                                      WHERE MZ_CODE='03' AND MZ_ID='{0}' AND dbo.SUBSTR(MZ_IDATE1,1,3)='{1}') ", MZ_ID, (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0'));


                DataTable dCount = o_DBFactory.ABC_toTest.Create_Table(strsql_COUNT, "get");

                if (dCount.Rows.Count > 0)
                {
                    sday = int.Parse(dCount.Rows[0]["MZ_TDAY11"].ToString());
                    sday_hour = int.Parse(dCount.Rows[0]["MZ_TTIME11"].ToString());
                    sday2 = int.Parse(dCount.Rows[0]["MZ_TDAY22"].ToString());
                    sday2_hour = int.Parse(dCount.Rows[0]["MZ_TTIME22"].ToString());
                    hday = int.Parse(dCount.Rows[0]["MZ_TDAY03"].ToString());
                    htime = int.Parse(dCount.Rows[0]["MZ_TTIME03"].ToString());

                }

                if (int.Parse(TextBox_MZ_SDAY_HOUR.Text.Trim()) - sday_hour < 0)
                {
                    if ((sday_hour % 8) != 0)
                    {
                        TextBox_MZ_SDAY_HOUR_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY_HOUR.Text.Trim()) + 8 - (sday_hour % 8)).ToString();
                        TextBox_MZ_SDAY_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY.Text.Trim()) - sday - (sday_hour / 8) - 1).ToString();
                    }
                    else
                    {
                        TextBox_MZ_SDAY_CANUSE.Text = "0";
                        TextBox_MZ_SDAY_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY.Text.Trim()) - sday - (sday_hour / 8)).ToString();
                    }
                }
                else
                {
                    TextBox_MZ_SDAY_HOUR_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY_HOUR.Text.Trim()) - sday_hour).ToString();
                    TextBox_MZ_SDAY_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY.Text.Trim()) - sday).ToString();
                }

                if (int.Parse(TextBox_MZ_SDAY2_HOUR.Text.Trim()) - sday2_hour < 0)
                {
                    if ((sday2_hour % 8) != 0)
                    {
                        TextBox_MZ_SDAY2_HOUR_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY2_HOUR.Text.Trim()) + 8 - (sday2_hour % 8)).ToString();
                        TextBox_MZ_SDAY2_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY2.Text.Trim()) - sday2 - (sday2_hour / 8) - 1).ToString();
                    }
                    else
                    {
                        TextBox_MZ_SDAY2_HOUR_CANUSE.Text = "0";
                        TextBox_MZ_SDAY2_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY2.Text.Trim()) - sday2 - (sday2_hour / 8)).ToString();
                    }
                }
                else
                {
                    TextBox_MZ_SDAY2_HOUR_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY2_HOUR.Text.Trim()) - sday2_hour).ToString();
                    TextBox_MZ_SDAY2_CANUSE.Text = (int.Parse(TextBox_MZ_SDAY2.Text.Trim()) - sday2).ToString();
                }
                //現在改成,無論如何都 休假（慰勞假）第二年 剩餘天數,都給0
                TextBox_MZ_SDAY2_HOUR_CANUSE.Text = "0";
                TextBox_MZ_SDAY2_CANUSE.Text = "0";

                if (int.Parse(TextBox_MZ_HTIME.Text.Trim()) - htime < 0)
                {
                    if ((htime % 8) != 0)
                    {
                        TextBox_MZ_HTIME_CANUSE.Text = (int.Parse(TextBox_MZ_HTIME.Text.Trim()) + 8 - (htime % 8)).ToString();
                        TextBox_MZ_HDAY_CANUSE.Text = (int.Parse(TextBox_MZ_HDAY.Text.Trim()) - hday - (htime / 8) - 1).ToString();
                    }
                    else
                    {
                        TextBox_MZ_HTIME_CANUSE.Text = "0";
                        TextBox_MZ_HDAY_CANUSE.Text = (int.Parse(TextBox_MZ_HDAY.Text.Trim()) - hday - (htime / 8)).ToString();
                    }
                }
                else
                {
                    TextBox_MZ_HTIME_CANUSE.Text = (int.Parse(TextBox_MZ_HTIME.Text.Trim()) - htime).ToString();
                    TextBox_MZ_HDAY_CANUSE.Text = (int.Parse(TextBox_MZ_HDAY.Text.Trim()) - hday).ToString();
                }


                //事假

                //20140120

                int pday = 0;
                int ptime = 0;

                string strPDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='01' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                DataTable PDAY_Count = o_DBFactory.ABC_toTest.Create_Table(strPDAY_Count, "get");

                if (PDAY_Count.Rows.Count > 0)
                {
                    pday = int.Parse(PDAY_Count.Rows[0]["MZ_TDAY"].ToString());

                    ptime = int.Parse(PDAY_Count.Rows[0]["MZ_TTIME"].ToString());
                }

                //調整計算方式 20190716 by sky
                //剩餘事假天數 = 總事假天數 - (已請事假天數 + (已請事假時數 / 8))
                int pday_canused = int.Parse(TextBox_MZ_PDAY.Text) - (pday + (ptime / 8));
                //剩餘事假時數 = 8 - (已請事假時數 % 8)
                int ptime_canused = 8 - (ptime % 8);

                //若有餘已請事假時數，則要多扣一天
                if (ptime != 0 && (ptime % 8) > 0)
                {
                    pday_canused--;
                }
                else
                {
                    ptime_canused = 0;
                }

                TextBox_MZ_PDAY_CANUSE.Text = int.Parse(pday_canused.ToString()) < 0 ? "0" : pday_canused.ToString();
                TextBox_MZ_PDAY_HOUR_CANUSE.Text = int.Parse(ptime_canused.ToString()) < 0 ? "0" : ptime_canused.ToString();


                //病假

                //20140120
                int sday1 = 0;
                int stime = 0;//已經請假的小時數

                string strSDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='02' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                DataTable SDAY_Count = o_DBFactory.ABC_toTest.Create_Table(strSDAY_Count, "get");

                if (SDAY_Count.Rows.Count > 0)
                {
                    sday1 = int.Parse(SDAY_Count.Rows[0]["MZ_TDAY"].ToString());

                    stime = int.Parse(SDAY_Count.Rows[0]["MZ_TTIME"].ToString());
                }


                //20140120
                int sday_used;//已經請病假的天數

                int stime_used;//已經請假的小時數(不滿整日 超過八小時則取餘數)

                int sday_canused;//剩餘病假天數

                int stime_canused = 0;

                //計算剩餘病假的天數和小時數
                //如果已經病請假的小時數非0, EX:請了N天6小時
                if (stime != 0)
                {   //小時數除以8得天數
                    int sday_Add = stime / 8;
                    //將已經請假的天數加上剛才得到的天數
                    sday_used = sday1 + sday_Add;
                    //已經請假的小時數(不滿整日 超過八小時則取餘數)
                    stime_used = stime % 8;
                    //剩餘病假天數=UI上的病假總天數 - 已經用掉的天數 - 1(因為小時數非0)
                    sday_canused = int.Parse(TextBox_MZ_SICKDAY.Text) - sday_used - 1;

                    //剩餘病假小時數=8-已經請病假的小時數
                    stime_canused = 8 - stime_used;
                    //Allen:以下這段邏輯整個錯誤,先停用
                    //2013/07/08
                    //Allen:如果,剩餘天數為0 且 已經請假的小時數低於8
                    //if (sday_canused <= 0 && stime_used < 8)
                    //{   //Allen:已經請假的小時數以0計算? 這段不太懂為何?
                    //    stime_used = 0;
                    //}
                    //else
                    //{
                    //    //Allen:將 已經請假的小時數,帶入病假剩餘的小時數,這段似乎是錯誤的?
                    //    stime_canused = stime_used;
                    //}
                    //2013/07/08
                }
                else
                {   //如果已經病請假的小時數為0, EX:請了N天
                    //已經請病假的天數
                    sday_used = sday1;
                    //已經請病假的小時數為0
                    stime_used = 0;
                    //剩餘的病假天數=總病假天數-已經請病假的天數
                    sday_canused = int.Parse(TextBox_MZ_SICKDAY.Text) - sday_used;
                    //剩餘的病假小時數為0
                    stime_canused = 0;
                }

                TextBox_MZ_SICKDAY_CANUSE.Text = int.Parse(sday_canused.ToString()) < 0 ? "0" : sday_canused.ToString();
                //UI 病假剩餘小時數
                TextBox_MZ_SICKDAY_HOUR_CANUSE.Text = int.Parse(stime_canused.ToString()) < 0 ? "0" : stime_canused.ToString();

            }
        }

        protected void DLTB_DAYS_Click(object sender, EventArgs e)
        {
            chk_TPMGroup();
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btOK2.Enabled = false;
            btSearch.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            MultiView1.ActiveViewIndex = 4;

            ChangeButtonColor(Panel_Button, DLTB_DAYS);

            DLTB_DAYS_COUNT(TextBox_MZ_ID.Text);
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ViewState.Remove("SN");
            Session["Mode"] = "SEARCH";

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveBasicSearch_New.aspx?TableName=BASIC&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        /// <summary>
        /// 按鈕 : 新增請假紀錄
        /// </summary>
        protected void btInsert_Click(object sender, EventArgs e)
        {
            //20140312 Ting
            AB_DAY.Visible = false;
            tableLastYearJobLocation.Visible = false;

            CheckBox_MZ_FOREIGN.Checked = false;
            CheckBox_MZ_CHINA.Checked = false;

            ViewState.Remove("SN");
            Button_DelFILE1.Visible = false;
            HyperLink_FILENAME1.Visible = false;
            Button_DelFILE2.Visible = false;
            HyperLink_FILENAME2.Visible = false;
            Button_DelFILE3.Visible = false;
            HyperLink_FILENAME3.Visible = false;
            FileUpload1.Visible = true;
            FileUpload1.Enabled = true;
            FileUpload2.Visible = true;
            FileUpload2.Enabled = true;
            FileUpload3.Visible = true;
            FileUpload3.Enabled = true;
            ViewState["Mode"] = "INSERT";

            if (MultiView1.ActiveViewIndex == 0)
            {
                preload();

                CheckBox_MZ_SWT.Checked = false;

                TextBox_MZ_CODE.Text = string.Empty;
                TextBox_MZ_CAUSE.Text = string.Empty;
                TextBox_MZ_MEMO.Text = string.Empty;
                TextBox_MZ_TADD.Text = string.Empty;
                TextBox_MZ_TDAY.Text = "0";
                TextBox_MZ_TTIME.Text = "0";
                TextBox_MZ_LastYearJobLocation.Text = string.Empty;

                TextBox_MZ_ROCCC.Text = string.Empty;
                TextBox_MZ_CHK1.Text = string.Empty;

                ViewState["CMDSQL"] = "INSERT INTO " +
                                                     "C_DLTB01" +
                                                           "(MZ_ID,MZ_NAME,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_CODE,MZ_RANK1,MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1," +
                                                           "MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME,MZ_ROCCC," +
                                                           "MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA,MZ_DLTB01_SN,MZ_FILE,MZ_RID,SIGN_KIND,MZ_FILE1,MZ_FILE2, MZ_LastYearJobLocation,MZ_AD,MZ_UNIT,DAYFACT,FUNERAL_TYPE)" +
                                                     " VALUES" +
                                                           "(@MZ_ID,@MZ_NAME,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC,@MZ_CODE,@MZ_RANK1,@MZ_SYSDAY,@MZ_IDATE1,@MZ_ITIME1," +
                                                           ":MZ_ODATE,@MZ_OTIME,@MZ_TDAY,@MZ_TTIME,@MZ_CAUSE,@MZ_MEMO,@MZ_SWT,@MZ_TADD,@MZ_RNAME,@MZ_ROCCC," +
                                                           ":MZ_CHK1,@MZ_SYSTIME,@MZ_FOREIGN,@MZ_CHINA, NEXT VALUE FOR dbo.C_DLTB01_SN,@MZ_FILE,@MZ_RID,@SIGN_KIND,@MZ_FILE1,@MZ_FILE2, @MZ_LastYearJobLocation,@MZ_AD,@MZ_UNIT,@DAYFACT,@FUNERAL_TYPE)";
            }


            rname_change("");

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btCancel.Enabled = true;
            btDelete.Enabled = false;
            //20140317
            btSearch.Enabled = false;
            //20140317


            C.controlEnable(ref this.Panel2, true);

            C.controlEnable(ref this.Panel7, true);
            btRESTDATECHECK.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);

            TextBox_MZ_CHK1.Enabled = false;
            // RadioButtonList_SIGN_KIND.Enabled = false;
        }


        //2013/11/16
        public void rname_databind(string SQL)
        {
            DataTable rname = o_DBFactory.ABC_toTest.Create_Table(SQL, "get");

            DropDownList_MZ_RNAME.DataSource = rname;
            DropDownList_MZ_RNAME.DataTextField = "MZ_NAME";
            DropDownList_MZ_RNAME.DataValueField = "MZ_ID";
            DropDownList_MZ_RNAME.DataBind();


        }

        private void rname_change(string MZ_RID)
        {
            //20140317 by 立廷
            //20140315    
            string SQL = "SELECT MZ_ID,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_OCCC_CH FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text.ToUpper() + "'";
            DataTable AD = o_DBFactory.ABC_toTest.Create_Table(SQL, "get");
            if (AD.Rows.Count > 0)
            {
                //代理人選單針對新增狀態增加在職條件 20190808 by sky
                string sqlStr = string.Empty;
                DataTable dt = new DataTable();
                //switch (o_A_DLBASE.OCCC(TextBox_MZ_ID.Text))
                switch (AD.Rows[0]["MZ_OCCC_CH"].ToStringNullSafe().Trim())
                {
                    case "局長":
                        sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' AND MZ_OCCC='{2}' {4}
                                                union  all 
                                                SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}'  AND MZ_TBDV='{3}' {4} "
                                                , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "0327", "1021", "006"
                                                , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        break;
                    case "分局長":
                        sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' AND MZ_OCCC='{2}' {4}  
                                                union  all 
                                                SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}'  AND MZ_TBDV='{3}' {4} "
                                                , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "0329", "A402", "006"
                                                , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        dt = o_DBFactory.ABC_toTest.Create_Table(sqlStr, "get");
                        if (dt.Rows.Count <= 0)
                        {
                            goto default;
                        }
                        break;
                    case "組長":
                    case "主任":
                        if (Session["ADPMZ_EXAD"].ToStringNullSafe() != "382130000C")
                        {
                            sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}'  AND MZ_TBDV='{1}' {3} 
                                                    union all 
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{2}' {3} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "006", AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe()
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    //2013/03/12 by vincent : 工友代理人可跨科室
                    //2013/03/14 by 立廷  :   工友代理人可跨科室只有警察局可以
                    //2013/05/02 by 立廷  :   因秘書室林敏堂 和A222019873 關西.工友互代外還加上自己科室臨時人員
                    //增加技工駕駛工友可以互相代理 20190917 by sky Add
                    case "工友":
                    case "技工":
                    case "駕駛":
                        //如果是總局的 工友等職務
                        if (AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() == "382130000C")
                        {
                            sqlStr = string.Format(@"
                                                    --同分局  Z014駕駛 Z015工友 Z016技工 可擔任代理人
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_OCCC in ({1}) {3}
                                                    union all 
                                                    --同分局 同單位 職稱代碼為Z011臨時人員
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_OCCC='Z011' AND MZ_EXUNIT='{2}' {3} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe()
                                                    , "'Z014','Z015','Z016'"
                                                    , AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe()
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                            //總局工友等職務,可由同科室協助擔任代理人。 2023-07-06
                            sqlStr += string.Format(@"union all SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' {2} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe()
                                                    , AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe()
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");

                            //最後 過濾掉重複的名單
                            sqlStr = string.Format(@"Select distinct a.* from ({0}) a ", sqlStr);
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    //2013/03/21  by 立廷
                    case "副分局長":
                        if (AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() != "382130000C" &&
                            AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() != "382130100C" &&
                            AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() != "382130200C" &&
                            AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() != "382130300C" &&
                            AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() != "382130500C" &&
                            AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() != "382135000C")
                        {
                            //不是分局的單位
                            sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='{0}'  AND MZ_OCCC='{1}' AND MZ_ID !='{2}' {4}
                                                    union  all 
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE  WHERE MZ_EXAD='{0}'  AND MZ_OCCC='{3}' {4} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "A402", TextBox_MZ_ID.Text.ToUpper(), "1080"
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    //2013/04/02  by 立廷
                    case "副大隊長":
                        if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382130200C")
                        {
                            //刑大多+ 技正及專員
                            sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='{0}'  AND MZ_OCCC='{1}' AND MZ_ID !='{3}' {4} 
                                                    union  all 
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE  WHERE MZ_EXAD='{0}'  AND MZ_OCCC='{2}' {4}  
                                                    union  all  
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE  WHERE MZ_EXAD='{0}'  AND (MZ_OCCC='1801' OR MZ_OCCC='1086') {4} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "1404", "1080", TextBox_MZ_ID.Text.ToUpper()
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        }
                        else
                        {
                            sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='{0}'  AND MZ_OCCC='{1}' AND MZ_ID !='{3}' {4} 
                                                    union  all 
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE  WHERE MZ_EXAD='{0}'  AND MZ_OCCC='{2}' {4} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "1404", "1080", TextBox_MZ_ID.Text.ToUpper()
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        }
                        break;
                    case "副隊長":
                        sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='{0}'  AND MZ_OCCC='{1}' AND MZ_ID !='{3}' {4} 
                                                union  all 
                                                SELECT MZ_NAME,MZ_ID FROM A_DLBASE  WHERE MZ_EXAD='{0}'  AND MZ_OCCC='{2}' {4} "
                                                , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "1414", "1080", TextBox_MZ_ID.Text.ToUpper()
                                                , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        break;
                    case "大隊長":
                        sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}'  AND MZ_OCCC='{1}' {2} "
                                                , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "1404"
                                                , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        break;
                    case "隊長":
                        sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}'  AND MZ_OCCC='{1}' {2} "
                                                , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), "1414"
                                                , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        dt = o_DBFactory.ABC_toTest.Create_Table(sqlStr, "get");
                        if (dt.Rows.Count <= 0)
                        {
                            goto default;
                        }
                        break;
                    default:
                        sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' {2} "
                                                , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe(), AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe()
                                                , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                        break;
                }

                rname_databind(sqlStr);
            }
            if (MZ_RID != "")
            {
                DropDownList_MZ_RNAME.SelectedValue = MZ_RID;

                //TextBox_MZ_ROCCC.Text = AD.Rows[0]["MZ_OCCC_CH"].ToString();//??
            }
        }

        /// <summary>
        /// 按鈕 : 變更請假紀錄表
        /// </summary>
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";

            TextBox_MZ_ROCCC.Text = "";

            Update_Textchange(TextBox_MZ_ID.Text, TextBox_MZ_IDATE1.Text, TextBox_MZ_ITIME1.Text, true);

            //finddata(int.Parse(xcount.Text));
            if (FileUpload1.Visible == true)
            {
                FileUpload1.Enabled = true;
            }
            if (FileUpload2.Visible == true)
            {
                FileUpload2.Enabled = true;
            }
            if (FileUpload3.Visible == true)
            {
                FileUpload3.Enabled = true;
            }
            if (MultiView1.ActiveViewIndex == 0)
            {
                ViewState["CMDSQL"] = "UPDATE C_DLTB01 SET  MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_EXAD=@MZ_EXAD,MZ_EXUNIT=@MZ_EXUNIT,MZ_OCCC=@MZ_OCCC,MZ_CODE=@MZ_CODE," +
                                                           "MZ_RANK1=@MZ_RANK1,MZ_SYSDAY=@MZ_SYSDAY,MZ_IDATE1=@MZ_IDATE1,MZ_ITIME1=@MZ_ITIME1," +
                                                           "MZ_ODATE=@MZ_ODATE,MZ_OTIME=@MZ_OTIME,MZ_TDAY=@MZ_TDAY,MZ_TTIME=@MZ_TTIME,MZ_CAUSE=@MZ_CAUSE," +
                                                           "MZ_MEMO=@MZ_MEMO,MZ_SWT=@MZ_SWT,MZ_TADD=@MZ_TADD,MZ_RNAME=@MZ_RNAME,MZ_ROCCC=@MZ_ROCCC," +
                                                           "MZ_CHK1=@MZ_CHK1,MZ_SYSTIME=@MZ_SYSTIME,MZ_FOREIGN=@MZ_FOREIGN,MZ_CHINA=@MZ_CHINA,MZ_FILE=@MZ_FILE,MZ_RID=@MZ_RID,SIGN_KIND=@SIGN_KIND,MZ_FILE1=@MZ_FILE1,MZ_FILE2=@MZ_FILE2, " +
                                                           "MZ_LastYearJobLocation = @MZ_LastYearJobLocation " +
                                                           " ,DAYFACT=@DAYFACT , FUNERAL_TYPE=@FUNERAL_TYPE" +
                                                           " WHERE MZ_ID='" + TextBox_MZ_ID.Text + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') + "' AND MZ_ITIME1='" + TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2) + "'";

                Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;
                //請假總時數
                int iday = 0;
                int itime = int.Parse(TextBox_MZ_TTIME.Text);
                if (TextBox_MZ_TDAY.Text != "") { iday = int.Parse(TextBox_MZ_TDAY.Text) * 8; }
                Session["PKEY_TOTAL_TIME"] = (iday + itime);
                //請假日期
                Session["PKEY_MZ_IDATE1"] = TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
                Session["PKEY_MZ_ODATE"] = TextBox_MZ_ODATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
                //共計日時
                Session["PKEY_MZ_ITIME1"] = TextBox_MZ_ITIME1.Text;
                Session["PKEY_MZ_OTIME"] = TextBox_MZ_OTIME.Text;
                //請假假別
                Session["PKEY_MZ_CODE"] = TextBox_MZ_CODE.Text;
                Session["PKEY_MZ_CODE1"] = TextBox_MZ_CODE1.Text;
                //請假事由
                Session["PKEY_MZ_CAUSE"] = TextBox_MZ_CAUSE.Text;
                //代理人
                Session["PKEY_MZ_RNAME"] = DropDownList_MZ_RNAME.SelectedValue;
                Session["PKEY_MZ_ROCCC"] = TextBox_MZ_ROCCC.Text;

                ViewState["old_CODE"] = TextBox_MZ_CODE.Text;
                //第一二年保留假及今年慰勞假 須控管天數
                if (TextBox_MZ_CODE.Text.Trim() == "03" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22")
                {
                    ViewState["old_TDAY"] = TextBox_MZ_TDAY.Text;

                    ViewState["old_TTIME"] = TextBox_MZ_TTIME.Text;
                }
                else
                {
                    ViewState["old_TDAY"] = "0";

                    ViewState["old_TTIME"] = "0";
                }

            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                //暫時刪除以確保不會再修改到資料 20200303 by sky
                //ViewState["CMDSQL"] = "UPDATE C_DLTBB SET MZ_ID = @MZ_ID,MZ_HDAY = @MZ_HDAY,MZ_HTIME = @MZ_HTIME," +
                //                                        " MZ_SDAY = @MZ_SDAY,MZ_SDAYHOUR = @MZ_SDAYHOUR,MZ_SDAY2 = @MZ_SDAY2,MZ_SDAY2HOUR = @MZ_SDAY2HOUR," +
                //                                        " MZ_SDAY3 = @MZ_SDAY3,MZ_SDAY3HOUR = @MZ_SDAY3HOUR,MZ_PDAY = @MZ_PDAY,MZ_PHOUR = @MZ_PHOUR," +
                //                                        " MZ_SICKDAY = @MZ_SICKDAY,MZ_SICKHOUR = @MZ_SICKHOUR,MZ_HCAREDAY = @MZ_HCAREDAY,MZ_HCAREHOUR = @MZ_HCAREHOUR," +
                //                                        " MZ_YEAR = @MZ_YEAR WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
            }

            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btInsert.Enabled = false;
            btOK.Enabled = true;
            btOK2.Enabled = true;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            //20140317
            btSearch.Enabled = false;
            //20140317
            C.controlEnable(ref this.Panel2, true);
            C.controlEnable(ref this.Panel7, true);
            TextBox_MZ_ID.Enabled = false;
            TextBox_MZ_NAME.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);

            if (TextBox_MZ_CODE.Text == "15")
            {
                btRESTDATECHECK.Enabled = false;
                ViewState["D1"] = TextBox_MZ_IDATE1.Text;
                TextBox_MZ_CAUSE.Enabled = false;
            }
            else if (TextBox_MZ_CODE.Text == "11" || TextBox_MZ_CODE.Text == "16" || TextBox_MZ_CODE.Text == "22")
            {
                btRESTDATECHECK.Enabled = false;
                TextBox_MZ_CAUSE.Enabled = false;
            }
            else
            {
                btRESTDATECHECK.Enabled = false;
            }

            rname_change(DropDownList_MZ_RNAME.SelectedValue);

            TextBox_MZ_CHK1.Enabled = false;
        }

        /// <summary>
        /// 變更補休類別的更新時數
        /// </summary>
        protected void CHECK_15()
        {
            if (ViewState["old_CODE"] != null)
            {
                if (ViewState["old_CODE"].ToString() == "15")
                {
                    //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                    string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE  MZ_RESTDATE LIKE '%" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";

                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getdeletevalue");

                    //判斷將補休變更為其他休假類別
                    if (TextBox_MZ_CODE.Text != ViewState["old_CODE"].ToString())
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                            if (s.Count() == 1)
                            {
                                //只有請過一筆資料
                                string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='',MZ_RESTHOUR=OTIME,LOCK_FLAG='N' WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' ";

                                //Log
                                LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假類別，更新可補休日");

                                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                            }
                            else
                            {
                                int ReturnHour = 0;
                                string RESTDATE = "";
                                for (int j = 0; j < s.Count(); j++)
                                {
                                    //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                                    if (s[j].Substring(0, 7) == TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'))
                                    {
                                        RESTDATE = dt.Rows[i]["MZ_RESTDATE"].ToString().Replace("，" + s[j], "");

                                        string[] y = s[j].Split('：');

                                        ReturnHour = int.Parse(y[1]) + int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());

                                        break;
                                    }
                                }

                                if (RESTDATE != "")
                                {
                                    string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + RESTDATE + "',MZ_RESTHOUR=" + ReturnHour + " WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y'  AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' ";

                                    //Log
                                    LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假類別，更新可補休日");

                                    o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                }
                            }
                        }
                    }
                    else
                    {
                        //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                        if (TextBox_MZ_IDATE1.Text.PadLeft(7, '0') != TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'))
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                                if (s.Count() == 1)
                                {
                                    //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                                    string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + s[0].Replace(TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'), TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0')) + "',MZ_RESTHOUR=MZ_RESTHOUR WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_RESTHOUR>0";

                                    //Log
                                    LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假日期，更新可補休日");

                                    o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                }
                                else
                                {
                                    for (int j = 0; j < s.Count(); j++)
                                    {
                                        if (s[j].Substring(0, 7) == TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0'))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此日期已有補休日期！')", true);
                                            return;
                                        }
                                    }

                                    string RESTDATE = "";
                                    for (int j = 0; j < s.Count(); j++)
                                    {
                                        //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                                        if (s[j].Substring(0, 7) == TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'))
                                        {
                                            RESTDATE = dt.Rows[i]["MZ_RESTDATE"].ToString().Replace("，" + s[j], TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0'));
                                            break;
                                        }
                                    }

                                    if (RESTDATE != "")
                                    {
                                        string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + RESTDATE + "' WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_RESTHOUR>0 ";

                                        //Log
                                        LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假日期，更新可補休日");

                                        o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 檢查上傳檔案
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ErrorString"></param>
        /// <param name="fu"></param>
        /// <param name="btn"></param>
        /// <param name="hl"></param>
        /// <param name="field">DB的欄位名稱</param>
        /// <param name="dPath"></param>
        /// <returns></returns>
        protected string CHECK_UPLOADFILE(string path, string ErrorString, FileUpload fu, Button btn, HyperLink hl, string field, string dPath)
        {
            //檢查有無上傳檔案
            if (path != "")
            {
                del_file(path);
                ViewState.Remove(dPath);

                fu.Visible = true;
                //fu.Enabled = false;
                btn.Visible = false;
                hl.Visible = false;

            }
            //如果 檔案上傳UI元件看不到,代表目前不允許上傳,則從DB抓取原本的檔案名稱
            if (fu.Visible == false && btn.Visible == true)
            {
                return Find_Url(ViewState["SN"].ToString(), field);
            }
            else if (fu.HasFile)
            {
                string file_name = fu.FileName;// +"." + vc[vc.Count() - 1].ToLower();
                if (!saveUploadFile(fu.PostedFile, file_name))
                {
                    ErrorString += "檔案上傳有誤" + "\\r\\n";
                    return string.Empty;
                }

                //fu.Visible = false;
                //btn.Visible = true;
                //btn.Enabled = false;
                //hl.Visible = true;
                return ViewState["path"].ToString();
            }
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// 按鈕 : 確定
        /// </summary>
        protected void btOK_Click(object sender, EventArgs e)
        {
            //檢核 請假日期起日不能大於迄日
            if ((int.Parse(TextBox_MZ_IDATE1.Text)) > (int.Parse(TextBox_MZ_ODATE.Text)))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假日期起日不能大於迄日');", true);
                return;
            }

            //檢核 日時欄位不應該出現負值
            if (int.Parse(TextBox_MZ_TDAY.Text) < 0 || int.Parse(TextBox_MZ_TTIME.Text) < 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('假單總時數不應輸入負值！');", true);
                return;
            }
            //假單總時數應大於0小時
            if ((int.Parse(TextBox_MZ_TDAY.Text) + int.Parse(TextBox_MZ_TTIME.Text)) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('假單總時數應大於0小時！');", true);
                return;
            }

            bool isok = true;
            if (ViewState["D1"] != null)
            {
                if (ViewState["D1"].ToString().Replace(@"/", string.Empty) != TextBox_MZ_IDATE1.Text)
                {
                    ViewState.Remove("D1");
                    isok = false;
                }
            }
            if (isok)
            {
                try
                {
                    if (TextBox_dayfact.Text != "")
                    {
                        string strTextBox_dayfact = TextBox_dayfact.Text;

                        if (strTextBox_dayfact.Length != 7)
                        {
                            TextBox_dayfact.BackColor = Color.Orange;
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請填寫事實發生日共7碼！');", true);
                            return;
                        }
                        else
                        {
                            bool check_dayfact = DateManange.Datetimeisok(DateManange.strtodate(TextBox_dayfact.Text.PadLeft(7, '0')));

                            if (!check_dayfact)
                            {
                                TextBox_dayfact.BackColor = Color.Orange;
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('事實發生日日期有誤！');", true);
                                return;
                            }
                            else
                            {
                                TextBox_dayfact.BackColor = Color.White;
                            }
                        }


                    }


                    ViewState["old_CODE"] = TextBox_MZ_CODE.Text;
                    //檢查補休時數
                    bool isOK_check_15_Hour = true;
                    if (TextBox_MZ_CODE.Text == "15")
                    {
                        //檢查補休情況 舊版 針對請假假別 15
                        check_15_Hour_old();
                    }
                    else
                    {   //檢查補休情況
                        isOK_check_15_Hour = check_15_Hour();
                    }
                    //檢查沒過
                    if (isOK_check_15_Hour == false)
                    {
                        return;
                    }

                    //編輯請假登錄資料
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        //2010.06.07 by 伊珊
                        string ErrorString = "";

                        string IDATE1 = string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim().Trim()) ? "" : TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0');

                        string old_ID = "NULL";

                        string old_IDATE1 = "NULL";

                        string old_ITIME1 = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();

                            //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                            old_IDATE1 = TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                            old_ITIME1 = Session["PKEY_MZ_ITIME1"].ToString();
                        }

                        #region 檢查是否已有請假資料
                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && old_IDATE1 == IDATE1 && old_ITIME1.Replace(":", string.Empty) == TextBox_MZ_ITIME1.Text && ViewState["Mode"].ToString() == "UPDATE")
                        {
                            pkey_check = "0";
                        }
                        else
                        {
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT COUNT(*) FROM C_DLTB01 WHERE MZ_ID='{0}' AND MZ_IDATE1='{1}' AND MZ_ITIME1='{2}'"
                                                                            , TextBox_MZ_ID.Text.Trim()
                                                                            , IDATE1
                                                                            , TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2)));
                        }
                        if (pkey_check != "0")
                        {
                            ErrorString += "已有相同資料！請修改請假日期及請假時間！" + "\\r\\n";
                            TextBox_MZ_ID.BackColor = Color.Orange;
                            TextBox_MZ_IDATE1.BackColor = Color.Orange;
                            TextBox_MZ_ITIME1.BackColor = Color.Orange;
                            TextBox_MZ_CODE.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_ID.BackColor = Color.White;
                            TextBox_MZ_ITIME1.BackColor = Color.White;
                            TextBox_MZ_IDATE1.BackColor = Color.White;
                            TextBox_MZ_CODE.BackColor = Color.White;
                        }
                        #endregion

                        #region 檢查請假時間
                        //檢核前後三年內日期
                        bool check_MZ_IDATE1 = DateManange.check_Date(TextBox_MZ_IDATE1.Text.PadLeft(7, '0'));
                        bool check_MZ_ODATE1 = DateManange.check_Date(TextBox_MZ_ODATE.Text.Trim().PadLeft(7, '0'));

                        //檢核日期格式
                        bool check_MZ_IDATE2 = DateManange.Datetimeisok(DateManange.strtodate(TextBox_MZ_IDATE1.Text.PadLeft(7, '0')));
                        bool check_MZ_ODATE2 = DateManange.Datetimeisok(DateManange.strtodate(TextBox_MZ_ODATE.Text.PadLeft(7, '0')));

                        //判斷輸入的日期小時是否為數字?
                        if ((string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())) && (string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())))
                        {
                            ErrorString += "請假日期小時有誤～請重輸入" + "\\r\\n";
                            TextBox_MZ_TTIME.BackColor = Color.Orange;
                            TextBox_MZ_TDAY.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_TTIME.BackColor = Color.White;
                            TextBox_MZ_TDAY.BackColor = Color.White;
                        }

                        if (int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim()) ? "0" : TextBox_MZ_TTIME.Text.Trim()) > 7)
                        {
                            ErrorString += "請假小時有誤" + "\\r\\n";
                            TextBox_MZ_TTIME.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_TTIME.BackColor = Color.White;
                        }

                        if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'")))
                        {
                            ErrorString += "請假假別有誤" + "\\r\\n";
                            TextBox_MZ_CODE.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_CODE.BackColor = Color.White;
                        }

                        if (!check_MZ_IDATE1 || !check_MZ_IDATE2)
                        {
                            ErrorString += "請假日期起日期有誤" + "\\r\\n";
                            TextBox_MZ_IDATE1.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_IDATE1.BackColor = Color.White;
                        }

                        if (!check_MZ_ODATE1 || !check_MZ_ODATE2)
                        {
                            ErrorString += "請假日期迄日期有誤" + "\\r\\n";
                            TextBox_MZ_ODATE.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_ODATE.BackColor = Color.White;
                        }
                        #endregion

                        #region 檢查出差旅遊
                        // Joy 新增判斷 出差(旅遊)地點若為大陸 則最近一年工作職掌為必填欄位
                        if (CheckBox_MZ_CHINA.Checked)
                        {
                            if (string.IsNullOrEmpty(TextBox_MZ_LastYearJobLocation.Text))
                            {
                                //ErrorString += "出差旅遊地點為大陸，最近一年工作職掌為必填欄位" + "\\r\\n";
                                //TextBox_MZ_LastYearJobLocation.BackColor = Color.Orange;
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('赴大陸相關地區,請填寫最近一年工作職掌！');", true);
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TADD.ClientID + "').focus();$get('" + TextBox_MZ_TADD.ClientID + "').focus();", true);
                                return;
                            }
                        }

                        if (CheckBox_MZ_FOREIGN.Checked || CheckBox_MZ_CHINA.Checked)
                        {

                            if (string.IsNullOrEmpty(TextBox_AB_IDATE.Text.Trim()) || string.IsNullOrEmpty(TextBox_AB_ITIME.Text.Trim()) ||
                                string.IsNullOrEmpty(TextBox_AB_ODATE.Text.Trim()) || string.IsNullOrEmpty(TextBox_AB_OTIME.Text.Trim()) ||
                                string.IsNullOrEmpty(TextBox_AB_TDAY.Text.Trim()) || string.IsNullOrEmpty(TextBox_AB_TTIME.Text.Trim()))
                            {
                                ErrorString += "請輸入完整出國實際天數" + "\\r\\n";
                                TextBox_AB_IDATE.BackColor = Color.Orange;
                                TextBox_AB_ITIME.BackColor = Color.Orange;
                                TextBox_AB_ODATE.BackColor = Color.Orange;
                                TextBox_AB_OTIME.BackColor = Color.Orange;
                                TextBox_AB_TDAY.BackColor = Color.Orange;
                                TextBox_AB_TTIME.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_IDATE.BackColor = Color.White;
                                TextBox_AB_ITIME.BackColor = Color.White;
                                TextBox_AB_ODATE.BackColor = Color.White;
                                TextBox_AB_OTIME.BackColor = Color.White;
                                TextBox_AB_TDAY.BackColor = Color.White;
                                TextBox_AB_TTIME.BackColor = Color.White;
                            }


                            bool check_AB_MZ_IDATE1 = DateManange.check_Date(TextBox_AB_IDATE.Text.PadLeft(7, '0'));

                            if (!DateManange.check_Date(TextBox_AB_IDATE.Text.PadLeft(7, '0')))
                            {
                                ErrorString += "實際出國日期起有誤" + "\\r\\n";
                                TextBox_AB_IDATE.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_IDATE.BackColor = Color.White;
                            }

                            if (TextBox_AB_ITIME.Text.Length != 4)
                            {
                                ErrorString += "實際出國時數格式錯誤" + "\\r\\n";
                                TextBox_AB_ITIME.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_ITIME.BackColor = Color.White;
                            }

                            if (!DateManange.check_Date(TextBox_AB_ODATE.Text.PadLeft(7, '0')))
                            {
                                ErrorString += "實際出國日期迄有誤" + "\\r\\n";
                                TextBox_AB_ODATE.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_ODATE.BackColor = Color.White;
                            }

                            if (TextBox_AB_OTIME.Text.Length != 4)
                            {
                                ErrorString += "實際出國時數格式錯誤" + "\\r\\n";
                                TextBox_AB_OTIME.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_OTIME.BackColor = Color.White;
                            }

                            //出國需填寫詳細旅遊地點
                            if (string.IsNullOrEmpty(TextBox_MZ_TADD.Text.Trim()))
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('出國或赴大陸相關地區,請詳細填寫地點！');", true);
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TADD.ClientID + "').focus();$get('" + TextBox_MZ_TADD.ClientID + "').focus();", true);
                                return;
                            }
                        }
                        #endregion

                        #region 檢查上傳檔案
                        F = CHECK_UPLOADFILE(ViewState["dpath1"] == null ? "" : ViewState["dpath1"].ToString(), ErrorString, FileUpload1, Button_DelFILE1, HyperLink_FILENAME1, "MZ_FILE", "dpath1");
                        F1 = CHECK_UPLOADFILE(ViewState["dpath2"] == null ? "" : ViewState["dpath2"].ToString(), ErrorString, FileUpload2, Button_DelFILE2, HyperLink_FILENAME2, "MZ_FILE1", "dpath2");
                        F2 = CHECK_UPLOADFILE(ViewState["dpath3"] == null ? "" : ViewState["dpath3"].ToString(), ErrorString, FileUpload3, Button_DelFILE3, HyperLink_FILENAME3, "MZ_FILE2", "dpath3");
                        #endregion

                        if (!string.IsNullOrEmpty(ErrorString.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改有問題之欄位！');", true);
                            return;
                        }
                        //檢查必填欄位
                        if (!Check_Field())
                        {
                            return;
                        }

                        if (RadioButtonList_SIGN_KIND.SelectedValue.ToString() == "2")
                        {
                            if (TextBox_MZ_CODE.Text.Trim() == "38")
                            {
                                if (F == string.Empty && F1 == string.Empty && F2 == string.Empty)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('疫苗接種假必須檢核附件！');", true);
                                    return;
                                }
                            }

                            if (TextBox_MZ_CODE.Text.Trim() == "15" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22" || TextBox_MZ_CODE.Text.Trim() == "16")
                            {
                                if (F == string.Empty && F1 == string.Empty && F2 == string.Empty)
                                {
                                    switch (TextBox_MZ_CODE.Text.Trim())
                                    {
                                        case "11":
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('加班補休假必須檢核附件！');", true);
                                            return;

                                        case "15":
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('加班補休假必須檢核附件！');", true);
                                            return;

                                        case "16":
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('超勤補休假必須檢核附件！');", true);
                                            return;

                                        case "22":
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日補休假必須檢核附件！');", true);
                                            return;

                                    }

                                }
                            }
                        }

                        string DAYFACT = "", FUNERAL_TYPE = "";


                        if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "05" || TextBox_MZ_CODE.Text == "08" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "10" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "06")
                        {
                            if (TextBox_MZ_CODE.Text == "06")//公假
                            {
                                FUNERAL_TYPE = DropDownList_funeral_type6.SelectedValue;
                                DAYFACT = TextBox_dayfact.Text;

                                if (DropDownList_funeral_type6.SelectedValue == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請挑選類型！');", true);
                                    return;
                                }
                                else
                                {
                                    if (DropDownList_funeral_type6.SelectedValue == "3")
                                    {
                                        if (TextBox_dayfact.Text == "")
                                        {
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                            return;
                                        }
                                    }

                                    if (DropDownList_funeral_type6.SelectedValue == "1")
                                    {
                                        if (RadioButtonList_SIGN_KIND.SelectedValue.ToString() == "2")
                                        {
                                            if (F == string.Empty && F1 == string.Empty && F2 == string.Empty)
                                            {
                                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('公假必須檢核附件！');", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }

                            if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "10")//婚假 流產假
                            {
                                FUNERAL_TYPE = "";
                                DAYFACT = TextBox_dayfact.Text;

                                if (TextBox_dayfact.Text == "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                    return;
                                }
                            }

                            if (TextBox_MZ_CODE.Text == "08")//娩假
                            {
                                FUNERAL_TYPE = DropDownList_funeral_type8.SelectedValue;
                                DAYFACT = TextBox_dayfact.Text;

                                if (DropDownList_funeral_type8.SelectedValue == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請挑選類型！');", true);
                                    return;
                                }
                                else
                                {
                                    if (TextBox_dayfact.Text == "")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                        return;
                                    }
                                }
                            }

                            if (TextBox_MZ_CODE.Text == "05")//喪假
                            {
                                FUNERAL_TYPE = DropDownList_funeral_type5.SelectedValue;
                                DAYFACT = TextBox_dayfact.Text;

                                if (DropDownList_funeral_type5.SelectedValue == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請挑選類型！');", true);
                                    return;
                                }
                                else
                                {
                                    if (TextBox_dayfact.Text == "")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                        return;
                                    }
                                }
                            }
                        }

                        #region 檢查選補休是否已選可補休日
                        if (TextBox_MZ_CODE.Text.Trim() == "15" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22" || TextBox_MZ_CODE.Text.Trim() == "16")
                        {
                            if (Session["MZ_RESTHOUR_DT"] == null)
                            {
                                //未選補休日
                                if (ViewState["Mode"].ToString() == "UPDATE")
                                {
                                    //檢查已申請日
                                    //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                                    //Mark暫時取消判斷
                                    //int c = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='{0}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_RESTDATE LIKE '%{1}%'", TextBox_MZ_ID.Text.Trim(), TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'))));
                                    //c += int.Parse(o_DBFactory.ABC_toTest.Get_First_Field(string.Format(@"Select COUNT(*) REST_HOUR From C_OVERTIME_FOR_DLTB01 
                                    //                                                            Where MZ_DLTB01_SN='{0}' ", ViewState["SN"].ToStringNullSafe()), null));
                                    //if (c <= 0)
                                    //{
                                    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                                    //    return;
                                    //}
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = new DataTable();
                                dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                                if (dt.Rows.Count == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                                    return;
                                }
                            }
                        }
                        #endregion


                        //第一二年保留假及今年慰勞假 須控管天數
                        //irk 請明年的假 102/01/02
                        int year = int.Parse(TextBox_MZ_IDATE1.Text.Substring(0, 3));
                        //if (TextBox_MZ_CODE.Text.Trim() == "03" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22")
                        if (TextBox_MZ_CODE.Text.Trim() == "03")
                        {
                            bool can = true;//是否能在請假

                            string SelectString = "";
                            //求出該人所有慰勞假
                            if (TextBox_MZ_CODE.Text == "03")
                            {
                                SelectString = " SELECT MZ_HDAY,MZ_HTIME " +
                                               " FROM C_DLTBB " +
                                               " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            }
                            //Mark新版加班語差勤隱藏
                            //else if (TextBox_MZ_CODE.Text == "11")
                            //{
                            //    SelectString = " SELECT MZ_SDAY,MZ_SDAY_HOUR " +
                            //                   " FROM C_DLTBB " +
                            //                   " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            //}
                            //else if (TextBox_MZ_CODE.Text == "22")
                            //{
                            //    SelectString = " SELECT MZ_SDAY2,MZ_SDAY2_HOUR " +
                            //                   " FROM C_DLTBB " +
                            //                   " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            //}

                            DataTable temp = new DataTable();

                            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

                            //以下是計算是否還能再請假！
                            //20140120
                            int day = 0;
                            int hour = 0;

                            string strDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (year).ToString().PadLeft(3, '0') + "'";
                            DataTable DAY_Count = o_DBFactory.ABC_toTest.Create_Table(strDAY_Count, "get");

                            if (DAY_Count.Rows.Count > 0)
                            {
                                day = int.Parse(DAY_Count.Rows[0]["MZ_TDAY"].ToString());

                                hour = int.Parse(DAY_Count.Rows[0]["MZ_TTIME"].ToString());
                            }

                            //以下是計算是否還能再請假！
                            int day_can_use = 0;
                            int hour_can_use = 0;

                            int old_TDAY = 0;
                            int old_TTIME = 0;

                            if (ViewState["Mode"].ToString() == "UPDATE")
                            {
                                old_TDAY = int.Parse(ViewState["old_TDAY"].ToString());
                                old_TTIME = int.Parse(ViewState["old_TTIME"].ToString());
                            }

                            if (temp == null || temp.Rows.Count == 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + year + "年休假日未計算！')", true);
                                btCancel.Enabled = true;
                                return;
                            }

                            if (Convert.ToInt32(temp.Rows[0][1]) + old_TTIME - hour % 8 < 0)
                            {
                                day_can_use = Convert.ToInt32(temp.Rows[0][0]) + old_TDAY - day - hour / 8 - 1;
                                hour_can_use = Convert.ToInt32(temp.Rows[0][1]) + old_TDAY + 8 - hour % 8;
                            }
                            else
                            {
                                day_can_use = Convert.ToInt32(temp.Rows[0][0]) + old_TDAY - day - hour / 8;
                                hour_can_use = Convert.ToInt32(temp.Rows[0][1]) + old_TTIME;
                            }

                            if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            {
                                if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                                {
                                    can = false;
                                }
                                else if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) == 0 && hour_can_use < 0)
                                {
                                    can = false;
                                }
                                else
                                {
                                    can = true;
                                }
                            }
                            else if (string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            {
                                if (day_can_use == 0 && hour_can_use - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                                {
                                    can = false;
                                }
                                else if (day_can_use - 1 == 0 && hour_can_use + 8 - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                                {
                                    can = false;
                                }
                                else
                                {
                                    can = true;
                                }
                            }
                            else if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            {
                                if (hour_can_use - int.Parse(TextBox_MZ_TTIME.Text) < 0)
                                {
                                    if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) - 1 < 0)
                                    {
                                        can = false;
                                    }
                                    else
                                    {
                                        can = true;
                                    }
                                }
                                else
                                {
                                    if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                                    {
                                        can = false;
                                    }
                                    else
                                    {
                                        can = true;
                                    }
                                }
                            }
                            if (TextBox_MZ_CODE.Text.Trim() == "15")
                            {
                                CHECK_15_old();
                            }
                            else
                            {
                                CHECK_15();
                            }


                            //沒超過才能塞資料庫！
                            if (!can)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + TextBox_MZ_CODE1.Text + "已超過上限！')", true);
                                btCancel.Enabled = true;
                            }
                            else
                            {
                                //20140310 ki
                                string ABDAYS = "";
                                if (TextBox_AB_IDATE.Text.Trim() == "" || TextBox_AB_ODATE.Text.Trim() == "")
                                {
                                    ABDAYS = TextBox_MZ_TADD.Text;
                                }
                                else
                                {
                                    if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                                    {
                                        ABDAYS = TextBox_MZ_TADD.Text + "$" +
                                            TextBox_AB_IDATE.Text.Trim() + "$" + TextBox_AB_ITIME.Text.Trim() + "$" +
                                            TextBox_AB_ODATE.Text.Trim() + "$" + TextBox_AB_OTIME.Text.Trim() + "$" +
                                            TextBox_AB_TDAY.Text.Trim() + "$" + TextBox_AB_TTIME.Text.Trim();
                                    }
                                }

                                if (ViewState["Mode"].ToString() == "INSERT")
                                {
                                    SqlParameter[] parameterList = {
                                    new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
                                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = TextBox_MZ_NAME.Text},
                                    new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_CODE",SqlDbType.VarChar){Value = TextBox_MZ_CODE.Text},
                                    new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_SYSDAY",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_SYSDAY.Text.Trim())?Convert.DBNull: TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_IDATE1",SqlDbType.VarChar){Value =  string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim())?Convert.DBNull:TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_ITIME1",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2)},
                                    new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ODATE.Text.Trim())?Convert.DBNull:TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_OTIME",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2)},
                                    new SqlParameter("MZ_TDAY",SqlDbType.Float){Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())?"0":TextBox_MZ_TDAY.Text)},
                                    new SqlParameter("MZ_TTIME",SqlDbType.Float){Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())?"0":TextBox_MZ_TTIME.Text)},
                                    new SqlParameter("MZ_CAUSE",SqlDbType.VarChar){Value = TextBox_MZ_CAUSE.Text},
                                    new SqlParameter("MZ_MEMO",SqlDbType.VarChar){Value = TextBox_MZ_MEMO.Text},
                                    new SqlParameter("MZ_SWT",SqlDbType.VarChar){Value = CheckBox_MZ_SWT.Checked?"Y":"N"},
                                    new SqlParameter("MZ_TADD",SqlDbType.VarChar){Value = ABDAYS},
                                    new SqlParameter("MZ_LastYearJobLocation",SqlDbType.VarChar){Value = TextBox_MZ_LastYearJobLocation.Text},
                                    new SqlParameter("MZ_RNAME",SqlDbType.VarChar){Value = DropDownList_MZ_RNAME.SelectedItem.Text},
                                    new SqlParameter("MZ_ROCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'")},
                                    new SqlParameter("MZ_CHK1",SqlDbType.VarChar){Value = TextBox_MZ_CHK1.Text},
                                    new SqlParameter("MZ_SYSTIME",SqlDbType.VarChar){Value = TextBox_MZ_SYSTIME.Text},
                                    new SqlParameter("MZ_FOREIGN",SqlDbType.VarChar){Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N"},
                                    new SqlParameter("MZ_CHINA",SqlDbType.VarChar){Value = CheckBox_MZ_CHINA.Checked?"Y":"N"},
                                    new SqlParameter("MZ_FILE",SqlDbType.VarChar){Value = F},
                                    new SqlParameter("MZ_FILE1",SqlDbType.VarChar){Value = F1},
                                    new SqlParameter("MZ_FILE2",SqlDbType.VarChar){Value = F2},
                                    new SqlParameter("MZ_RID",SqlDbType.VarChar){Value = DropDownList_MZ_RNAME.SelectedValue},
                                    new SqlParameter("SIGN_KIND",SqlDbType.VarChar){Value = RadioButtonList_SIGN_KIND.SelectedValue},
                                    new SqlParameter("MZ_AD", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'")  },
                                    new SqlParameter("MZ_UNIT", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'") },
                                    new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                    new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                                };

                                    o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);


                                    //Log
                                    LogModel.saveLog("C", "A", ViewState["CMDSQL"].ToString(), parameterList, Request.QueryString["TPM_FION"], "新增假單");

                                }
                                else if (ViewState["Mode"].ToString() == "UPDATE")
                                {
                                    SqlParameter[] parameterList = {
                                    new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
                                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = TextBox_MZ_NAME.Text},
                                    new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_CODE",SqlDbType.VarChar){Value = TextBox_MZ_CODE.Text},
                                    new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_SYSDAY",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_SYSDAY.Text.Trim())?Convert.DBNull: TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_IDATE1",SqlDbType.VarChar){Value =  string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim())?Convert.DBNull:TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_ITIME1",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2)},
                                    new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ODATE.Text.Trim())?Convert.DBNull:TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_OTIME",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2)},
                                    new SqlParameter("MZ_TDAY",SqlDbType.Float){Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())?"0":TextBox_MZ_TDAY.Text)},
                                    new SqlParameter("MZ_TTIME",SqlDbType.Float){Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())?"0":TextBox_MZ_TTIME.Text)},
                                    new SqlParameter("MZ_CAUSE",SqlDbType.VarChar){Value = TextBox_MZ_CAUSE.Text},
                                    new SqlParameter("MZ_MEMO",SqlDbType.VarChar){Value = TextBox_MZ_MEMO.Text},
                                    new SqlParameter("MZ_SWT",SqlDbType.VarChar){Value = CheckBox_MZ_SWT.Checked?"Y":"N"},
                                    new SqlParameter("MZ_TADD",SqlDbType.VarChar){Value = ABDAYS},
                                    new SqlParameter("MZ_LastYearJobLocation",SqlDbType.VarChar){Value = TextBox_MZ_LastYearJobLocation.Text},
                                    new SqlParameter("MZ_RNAME",SqlDbType.VarChar){Value = DropDownList_MZ_RNAME.SelectedItem.Text},
                                    new SqlParameter("MZ_ROCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'")},
                                    new SqlParameter("MZ_CHK1",SqlDbType.VarChar){Value = TextBox_MZ_CHK1.Text},
                                    new SqlParameter("MZ_SYSTIME",SqlDbType.VarChar){Value = TextBox_MZ_SYSTIME.Text},
                                    new SqlParameter("MZ_FOREIGN",SqlDbType.VarChar){Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N"},
                                    new SqlParameter("MZ_CHINA",SqlDbType.VarChar){Value = CheckBox_MZ_CHINA.Checked?"Y":"N"},
                                    new SqlParameter("MZ_FILE",SqlDbType.VarChar){Value = F},
                                    new SqlParameter("MZ_FILE1",SqlDbType.VarChar){Value = F1},
                                    new SqlParameter("MZ_FILE2",SqlDbType.VarChar){Value = F2},
                                    new SqlParameter("MZ_RID",SqlDbType.VarChar){Value = DropDownList_MZ_RNAME.SelectedValue},
                                    new SqlParameter("SIGN_KIND",SqlDbType.VarChar){Value = RadioButtonList_SIGN_KIND.SelectedValue},
                                    new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                    new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                                };

                                    o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);


                                    //Log
                                    LogModel.saveLog("C", "A", ViewState["CMDSQL"].ToString(), parameterList, Request.QueryString["TPM_FION"], "修改假單");

                                }


                                Session.Remove("PKEY_MZ_ID");
                                Session.Remove("PKEY_MZ_IDATE1");
                                Session.Remove("PKEY_MZ_ITIME1");
                            }
                        }
                        else
                        {
                            //變更補休類別的更新時數
                            if (TextBox_MZ_CODE.Text.Trim() == "15")
                            {
                                CHECK_15_old();
                            }
                            else
                            {
                                CHECK_15();
                            }

                            //檢查該假別是否有設定上限
                            string overCodeDay = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'");

                            bool can = true;

                            if (!string.IsNullOrEmpty(overCodeDay.Trim()))
                            {

                                int code_day = 0;
                                int code_hour = 0;

                                //取得已請天數時數
                                string strcodeDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                                DataTable codeDAY_Count = o_DBFactory.ABC_toTest.Create_Table(strcodeDAY_Count, "get");

                                if (codeDAY_Count.Rows.Count > 0)
                                {
                                    code_day = int.Parse(codeDAY_Count.Rows[0]["MZ_TDAY"].ToString());

                                    code_hour = int.Parse(codeDAY_Count.Rows[0]["MZ_TTIME"].ToString());
                                }

                                //20140120
                                int code_day_can_use = 0;
                                int code_hour_can_use = 0;

                                int old_TDAY = 0;
                                int old_TTIME = 0;

                                if (ViewState["Mode"].ToString() == "UPDATE")
                                {
                                    //修改狀態時取回已請天數時數
                                    old_TDAY = int.Parse(ViewState["old_TDAY"].ToString());
                                    old_TTIME = int.Parse(ViewState["old_TTIME"].ToString());
                                }

                                if (int.Parse(overCodeDay) - code_hour % 8 < 0)
                                {
                                    code_day_can_use = int.Parse(overCodeDay) + old_TDAY - code_day - code_hour / 8 - 1;
                                    code_hour_can_use = 8 - code_hour % 8;
                                }
                                else
                                {
                                    code_day_can_use = int.Parse(overCodeDay) + old_TDAY - code_day - code_hour / 8;
                                }

                                if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                                {
                                    if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                                    {
                                        can = false;
                                    }
                                    else if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) == 0 && code_day_can_use < 0)
                                    {
                                        can = false;
                                    }
                                    else
                                    {
                                        can = true;
                                    }
                                }
                                else if (string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                                {
                                    if (code_day_can_use == 0 && code_hour_can_use - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                                    {
                                        can = false;
                                    }
                                    else if (code_day_can_use - 1 == 0 && code_hour_can_use + 8 - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                                    {
                                        can = false;
                                    }
                                    else
                                    {
                                        can = true;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                                {
                                    if (code_hour_can_use - int.Parse(TextBox_MZ_TTIME.Text) < 0)
                                    {
                                        if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) - 1 < 0)
                                        {
                                            can = false;
                                        }
                                        else
                                        {
                                            can = true;
                                        }
                                    }
                                    else
                                    {
                                        if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                                        {
                                            can = false;
                                        }
                                        else
                                        {
                                            can = true;
                                        }
                                    }
                                }
                            }

                            if (!can)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + TextBox_MZ_CODE1.Text + "已超過上限！請注意')", true);
                            }

                            //20140310 ki
                            string ABDAYS = "";
                            if (TextBox_AB_IDATE.Text.Trim() == "" || TextBox_AB_ODATE.Text.Trim() == "")
                            {
                                ABDAYS = TextBox_MZ_TADD.Text;
                            }
                            else
                            {
                                if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                                {
                                    ABDAYS = TextBox_MZ_TADD.Text + "$" +
                                            TextBox_AB_IDATE.Text.Trim() + "$" + TextBox_AB_ITIME.Text.Trim() + "$" +
                                            TextBox_AB_ODATE.Text.Trim() + "$" + TextBox_AB_OTIME.Text.Trim() + "$" +
                                            TextBox_AB_TDAY.Text.Trim() + "$" + TextBox_AB_TTIME.Text.Trim();
                                }

                            }

                            if (ViewState["Mode"].ToString() == "INSERT")
                            {
                                SqlParameter[] parameterList = {
                                new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text },
                                new SqlParameter("MZ_NAME", SqlDbType.VarChar) { Value = TextBox_MZ_NAME.Text },
                                new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value =o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value =  o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_CODE", SqlDbType.VarChar) { Value = TextBox_MZ_CODE.Text },
                                new SqlParameter("MZ_RANK1", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_SYSDAY", SqlDbType.VarChar) { Value = TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_IDATE1", SqlDbType.VarChar) { Value = TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_ITIME1", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2) },
                                new SqlParameter("MZ_ODATE", SqlDbType.VarChar) { Value = TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_OTIME", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2) },
                                new SqlParameter("MZ_TDAY", SqlDbType.Float) { Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())?"0":TextBox_MZ_TDAY.Text) },
                                new SqlParameter("MZ_TTIME", SqlDbType.Float) { Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())?"0":TextBox_MZ_TTIME.Text) },
                                new SqlParameter("MZ_CAUSE", SqlDbType.VarChar) { Value = TextBox_MZ_CAUSE.Text },
                                new SqlParameter("MZ_MEMO", SqlDbType.VarChar) { Value = TextBox_MZ_MEMO.Text },
                                new SqlParameter("MZ_SWT", SqlDbType.VarChar) { Value = CheckBox_MZ_SWT.Checked?"Y":"N" },
                                new SqlParameter("MZ_TADD", SqlDbType.VarChar) { Value = ABDAYS },
                                new SqlParameter("MZ_LastYearJobLocation", SqlDbType.VarChar) { Value = TextBox_MZ_LastYearJobLocation.Text },
                                new SqlParameter("MZ_RNAME", SqlDbType.VarChar) { Value = DropDownList_MZ_RNAME.SelectedItem.Text },
                                new SqlParameter("MZ_ROCCC", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'") },
                                new SqlParameter("MZ_CHK1", SqlDbType.VarChar) { Value = TextBox_MZ_CHK1.Text },
                                new SqlParameter("MZ_SYSTIME", SqlDbType.VarChar) { Value = TextBox_MZ_SYSTIME.Text },
                                new SqlParameter("MZ_FOREIGN", SqlDbType.VarChar) { Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N" },
                                new SqlParameter("MZ_CHINA", SqlDbType.VarChar) { Value = CheckBox_MZ_CHINA.Checked?"Y":"N" },
                                new SqlParameter("MZ_FILE", SqlDbType.VarChar) { Value = F },
                                new SqlParameter("MZ_FILE1", SqlDbType.VarChar) { Value = F1 },
                                new SqlParameter("MZ_FILE2", SqlDbType.VarChar) { Value = F2 },
                                new SqlParameter("MZ_RID", SqlDbType.VarChar) { Value = DropDownList_MZ_RNAME.SelectedValue },
                                new SqlParameter("SIGN_KIND", SqlDbType.VarChar) { Value = RadioButtonList_SIGN_KIND.SelectedValue },
                                new SqlParameter("MZ_AD", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'")  },
                                new SqlParameter("MZ_UNIT", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'") },
                                new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                            };

                                //Log
                                LogModel.saveLog("C", "U", ViewState["CMDSQL"].ToString(), parameterList, Request.QueryString["TPM_FION"], "新增假單");
                                //Mark暫時隱藏
                                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                            }
                            else if (ViewState["Mode"].ToString() == "UPDATE")
                            {
                                SqlParameter[] parameterList = {
                                new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text },
                                new SqlParameter("MZ_NAME", SqlDbType.VarChar) { Value = TextBox_MZ_NAME.Text },
                                new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value =o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value =  o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_CODE", SqlDbType.VarChar) { Value = TextBox_MZ_CODE.Text },
                                new SqlParameter("MZ_RANK1", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_SYSDAY", SqlDbType.VarChar) { Value = TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_IDATE1", SqlDbType.VarChar) { Value = TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_ITIME1", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2) },
                                new SqlParameter("MZ_ODATE", SqlDbType.VarChar) { Value = TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_OTIME", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2) },
                                new SqlParameter("MZ_TDAY", SqlDbType.Float) { Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())?"0":TextBox_MZ_TDAY.Text) },
                                new SqlParameter("MZ_TTIME", SqlDbType.Float) { Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())?"0":TextBox_MZ_TTIME.Text) },
                                new SqlParameter("MZ_CAUSE", SqlDbType.VarChar) { Value = TextBox_MZ_CAUSE.Text },
                                new SqlParameter("MZ_MEMO", SqlDbType.VarChar) { Value = TextBox_MZ_MEMO.Text },
                                new SqlParameter("MZ_SWT", SqlDbType.VarChar) { Value = CheckBox_MZ_SWT.Checked?"Y":"N" },
                                new SqlParameter("MZ_TADD", SqlDbType.VarChar) { Value = ABDAYS },
                                new SqlParameter("MZ_LastYearJobLocation", SqlDbType.VarChar) { Value = TextBox_MZ_LastYearJobLocation.Text },
                                new SqlParameter("MZ_RNAME", SqlDbType.VarChar) { Value = DropDownList_MZ_RNAME.SelectedItem.Text },
                                new SqlParameter("MZ_ROCCC", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'") },
                                new SqlParameter("MZ_CHK1", SqlDbType.VarChar) { Value = TextBox_MZ_CHK1.Text },
                                new SqlParameter("MZ_SYSTIME", SqlDbType.VarChar) { Value = TextBox_MZ_SYSTIME.Text },
                                new SqlParameter("MZ_FOREIGN", SqlDbType.VarChar) { Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N" },
                                new SqlParameter("MZ_CHINA", SqlDbType.VarChar) { Value = CheckBox_MZ_CHINA.Checked?"Y":"N" },
                                new SqlParameter("MZ_FILE", SqlDbType.VarChar) { Value = F },
                                new SqlParameter("MZ_FILE1", SqlDbType.VarChar) { Value = F1 },
                                new SqlParameter("MZ_FILE2", SqlDbType.VarChar) { Value = F2 },
                                new SqlParameter("MZ_RID", SqlDbType.VarChar) { Value = DropDownList_MZ_RNAME.SelectedValue },
                                new SqlParameter("SIGN_KIND", SqlDbType.VarChar) { Value = RadioButtonList_SIGN_KIND.SelectedValue },
                                new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                            };

                                //Log
                                LogModel.saveLog("C", "U", ViewState["CMDSQL"].ToString(), parameterList, Request.QueryString["TPM_FION"], "修改假單");
                                //Mark暫時隱藏
                                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                            }


                            Session.Remove("PKEY_MZ_ID");
                            Session.Remove("PKEY_MZ_IDATE1");
                            Session.Remove("PKEY_MZ_ITIME1");
                            ViewState.Remove("old_CODE");
                        }
                    }
                    //編輯個人差假天數
                    else if (MultiView1.ActiveViewIndex == 4)
                    {
                        //暫時刪除以確保不會再修改到資料 20200303 by sky
                        //SqlParameter[] parameterList = {
                        //    new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text },
                        //    new SqlParameter("MZ_HDAY", SqlDbType.Float) { Value = TextBox_MZ_HDAY.Text },
                        //    new SqlParameter("MZ_HTIME", SqlDbType.Float) { Value = TextBox_MZ_HTIME.Text },
                        //    new SqlParameter("MZ_SDAY", SqlDbType.Float) { Value = TextBox_MZ_SDAY.Text },
                        //    new SqlParameter("MZ_SDAYHOUR", SqlDbType.Float) { Value = TextBox_MZ_SDAY_HOUR.Text },
                        //    new SqlParameter("MZ_SDAY2", SqlDbType.Float) { Value = TextBox_MZ_SDAY2.Text },
                        //    new SqlParameter("MZ_SDAY2HOUR", SqlDbType.Float) { Value = TextBox_MZ_SDAY2_HOUR.Text },
                        //    new SqlParameter("MZ_SDAY3", SqlDbType.Float) { Value = TextBox_MZ_HDAY.Text },
                        //    new SqlParameter("MZ_SDAY3HOUR", SqlDbType.Float) { Value = TextBox_MZ_HTIME.Text },
                        //    new SqlParameter("MZ_PDAY", SqlDbType.Float) { Value = TextBox_MZ_PDAY.Text },
                        //    new SqlParameter("MZ_PHOUR", SqlDbType.Float) { Value = TextBox_MZ_PHOUR.Text },
                        //    new SqlParameter("MZ_SICKDAY", SqlDbType.Float) { Value = TextBox_MZ_SICKDAY.Text },
                        //    new SqlParameter("MZ_SICKHOUR", SqlDbType.Float) { Value = TextBox_MZ_SICKHOUR.Text },
                        //    new SqlParameter("MZ_HCAREDAY", SqlDbType.Float) { Value = TextBox_MZ_HCAREDAY.Text },
                        //    new SqlParameter("MZ_HCAREHOUR", SqlDbType.Float) { Value = TextBox_MZ_HCAREHOUR.Text },
                        //    new SqlParameter("MZ_YEAR", SqlDbType.VarChar) { Value = TextBox_MZ_YEAR.Text }
                        //};

                        //o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                        //string updateStr = string.Format(@"UPDATE A_DLBASE SET MZ_TYEAR='{0}', MZ_TMONTH='{1}', MZ_MEMO='{2}' WHERE MZ_ID='{3}'"
                        //                                , TextBox_MZ_TYEAR.Text.Trim()
                        //                                , TextBox_MZ_TMONTH.Text.Trim()
                        //                                , TextBox_MZ_TMEMO.Text.Trim()
                        //                                , TextBox_MZ_ID.Text.Trim());

                        //o_DBFactory.ABC_toTest.Edit_Data(updateStr);

                        //2010.06.04 LOG紀錄 by伊珊
                        //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), updateString);
                    }


                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        //Mark暫時隱藏
                        isOnline(ViewState["Mode"].ToString());

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增假單成功');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        if (((Button)sender).ID == "btOK")
                            isOnline(ViewState["Mode"].ToString());

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改假單成功');", true);
                    }

                    //處理申請補休
                    if (TextBox_MZ_CODE.Text.Trim() == "15")
                    {
                        INSERT_15_old();
                    }
                    else
                    {
                        INSERT_15();
                    }


                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btOK2.Enabled = false;
                    btCancel.Enabled = false;
                    btSearch.Enabled = true;

                    if ((ViewState["C_strGID"].ToString() == "A" || ViewState["C_strGID"].ToString() == "B" || ViewState["C_strGID"].ToString() == "C"))
                    {
                        btDelete.Enabled = true;
                    }
                    else
                    {
                        btDelete.Enabled = false;
                    }

                    ViewState.Remove("Mode");
                    C.controlEnable(ref this.Panel2, false);
                    C.controlEnable(ref this.Panel7, false);


                }
                catch (Exception ex)
                {
                    string aa = ex.Message.ToString();
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無法修改補休日期，若要修改，\\r\\n請差假承辦人刪除該筆假單後，重新新增。');", true);
            }
        }
        #region 不重跑線上流程
        /// <summary>
        /// 按鈕 : 不重跑線上流程
        /// </summary>
        protected void btOK2_Click(object sender, EventArgs e)
        {

            if ((int.Parse(TextBox_MZ_IDATE1.Text)) > (int.Parse(TextBox_MZ_ODATE.Text)))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假日期起日不能大於迄日');", true);
                return;
            }
            bool isok = true;
            if (ViewState["D1"] != null)
            {
                if (ViewState["D1"].ToString().Replace(@"/", string.Empty) != TextBox_MZ_IDATE1.Text)
                {
                    ViewState.Remove("D1");
                    isok = false;
                }
            }
            if (isok)
            {
                try
                {
                    #region 判斷事實發生日是否有填寫
                    if (TextBox_dayfact.Text != "")  //假別不更動
                    {
                        string strTextBox_dayfact = TextBox_dayfact.Text;

                        if (strTextBox_dayfact.Length != 7)
                        {
                            TextBox_dayfact.BackColor = Color.Orange;
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請填寫事實發生日共7碼！');", true);
                            return;
                        }
                        else
                        {
                            bool check_dayfact = DateManange.Datetimeisok(DateManange.strtodate(TextBox_dayfact.Text.PadLeft(7, '0')));

                            if (!check_dayfact)
                            {
                                TextBox_dayfact.BackColor = Color.Orange;
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('事實發生日日期有誤！');", true);
                                return;
                            }
                            else
                            {
                                TextBox_dayfact.BackColor = Color.White;
                            }
                        }


                    }
                    #endregion



                    //ViewState["old_CODE"] = TextBox_MZ_CODE.Text;
                    # region 檢查補休時數
                    //if (TextBox_MZ_CODE.Text == "15")
                    //{
                    //    check_15_Hour_old();
                    //}
                    //else
                    //{
                    //    check_15_Hour();
                    //}
                    #endregion
                    //編輯請假登錄資料
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        //2010.06.07 by 伊珊
                        string ErrorString = "";

                        string IDATE1 = string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim().Trim()) ? "" : TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0');

                        string old_ID = "NULL";

                        string old_IDATE1 = "NULL";

                        string old_ITIME1 = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();

                            //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                            old_IDATE1 = TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                            old_ITIME1 = Session["PKEY_MZ_ITIME1"].ToString();
                        }

                        #region 檢查是否已有請假資料
                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && old_IDATE1 == IDATE1 && old_ITIME1.Replace(":", string.Empty) == TextBox_MZ_ITIME1.Text && ViewState["Mode"].ToString() == "UPDATE")
                        {
                            pkey_check = "0";
                        }
                        else
                        {
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT COUNT(*) FROM C_DLTB01 WHERE MZ_ID='{0}' AND MZ_IDATE1='{1}' AND MZ_ITIME1='{2}'"
                                                                            , TextBox_MZ_ID.Text.Trim()
                                                                            , IDATE1
                                                                            , TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2)));
                        }
                        if (pkey_check != "0")
                        {
                            ErrorString += "已有相同資料！請修改請假日期及請假時間！" + "\\r\\n";
                            TextBox_MZ_ID.BackColor = Color.Orange;
                            TextBox_MZ_IDATE1.BackColor = Color.Orange;
                            TextBox_MZ_ITIME1.BackColor = Color.Orange;
                            TextBox_MZ_CODE.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_ID.BackColor = Color.White;
                            TextBox_MZ_ITIME1.BackColor = Color.White;
                            TextBox_MZ_IDATE1.BackColor = Color.White;
                            TextBox_MZ_CODE.BackColor = Color.White;
                        }
                        #endregion

                        #region 檢查請假時間
                        //檢核前後三年內日期
                        bool check_MZ_IDATE1 = DateManange.check_Date(TextBox_MZ_IDATE1.Text.PadLeft(7, '0'));
                        bool check_MZ_ODATE1 = DateManange.check_Date(TextBox_MZ_ODATE.Text.Trim().PadLeft(7, '0'));

                        //檢核日期格式
                        bool check_MZ_IDATE2 = DateManange.Datetimeisok(DateManange.strtodate(TextBox_MZ_IDATE1.Text.PadLeft(7, '0')));
                        bool check_MZ_ODATE2 = DateManange.Datetimeisok(DateManange.strtodate(TextBox_MZ_ODATE.Text.PadLeft(7, '0')));

                        if ((string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())) && (string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())))
                        {
                            ErrorString += "請假日期小時有誤～請重輸入" + "\\r\\n";
                            TextBox_MZ_TTIME.BackColor = Color.Orange;
                            TextBox_MZ_TDAY.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_TTIME.BackColor = Color.White;
                            TextBox_MZ_TDAY.BackColor = Color.White;
                        }

                        if (int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim()) ? "0" : TextBox_MZ_TTIME.Text.Trim()) > 7)
                        {
                            ErrorString += "請假小時有誤" + "\\r\\n";
                            TextBox_MZ_TTIME.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_TTIME.BackColor = Color.White;
                        }

                        if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'")))
                        {
                            ErrorString += "請假假別有誤" + "\\r\\n";
                            TextBox_MZ_CODE.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_CODE.BackColor = Color.White;
                        }

                        if (!check_MZ_IDATE1 || !check_MZ_IDATE2)
                        {
                            ErrorString += "請假日期起日期有誤" + "\\r\\n";
                            TextBox_MZ_IDATE1.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_IDATE1.BackColor = Color.White;
                        }

                        if (!check_MZ_ODATE1 || !check_MZ_ODATE2)
                        {
                            ErrorString += "請假日期迄日期有誤" + "\\r\\n";
                            TextBox_MZ_ODATE.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_ODATE.BackColor = Color.White;
                        }
                        #endregion

                        #region 檢查出差旅遊
                        // Joy 新增判斷 出差(旅遊)地點若為大陸 則最近一年工作職掌為必填欄位
                        if (CheckBox_MZ_CHINA.Checked)
                        {
                            if (string.IsNullOrEmpty(TextBox_MZ_LastYearJobLocation.Text))
                            {
                                //ErrorString += "出差旅遊地點為大陸，最近一年工作職掌為必填欄位" + "\\r\\n";
                                //TextBox_MZ_LastYearJobLocation.BackColor = Color.Orange;
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('赴大陸相關地區,請填寫最近一年工作職掌！');", true);
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TADD.ClientID + "').focus();$get('" + TextBox_MZ_TADD.ClientID + "').focus();", true);
                                return;
                            }
                        }

                        if (CheckBox_MZ_FOREIGN.Checked || CheckBox_MZ_CHINA.Checked)
                        {

                            if (string.IsNullOrEmpty(TextBox_AB_IDATE.Text.Trim()) || string.IsNullOrEmpty(TextBox_AB_ITIME.Text.Trim()) ||
                                string.IsNullOrEmpty(TextBox_AB_ODATE.Text.Trim()) || string.IsNullOrEmpty(TextBox_AB_OTIME.Text.Trim()) ||
                                string.IsNullOrEmpty(TextBox_AB_TDAY.Text.Trim()) || string.IsNullOrEmpty(TextBox_AB_TTIME.Text.Trim()))
                            {
                                ErrorString += "請輸入完整出國實際天數" + "\\r\\n";
                                TextBox_AB_IDATE.BackColor = Color.Orange;
                                TextBox_AB_ITIME.BackColor = Color.Orange;
                                TextBox_AB_ODATE.BackColor = Color.Orange;
                                TextBox_AB_OTIME.BackColor = Color.Orange;
                                TextBox_AB_TDAY.BackColor = Color.Orange;
                                TextBox_AB_TTIME.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_IDATE.BackColor = Color.White;
                                TextBox_AB_ITIME.BackColor = Color.White;
                                TextBox_AB_ODATE.BackColor = Color.White;
                                TextBox_AB_OTIME.BackColor = Color.White;
                                TextBox_AB_TDAY.BackColor = Color.White;
                                TextBox_AB_TTIME.BackColor = Color.White;
                            }


                            bool check_AB_MZ_IDATE1 = DateManange.check_Date(TextBox_AB_IDATE.Text.PadLeft(7, '0'));

                            if (!DateManange.check_Date(TextBox_AB_IDATE.Text.PadLeft(7, '0')))
                            {
                                ErrorString += "實際出國日期起有誤" + "\\r\\n";
                                TextBox_AB_IDATE.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_IDATE.BackColor = Color.White;
                            }

                            if (TextBox_AB_ITIME.Text.Length != 4)
                            {
                                ErrorString += "實際出國時數格式錯誤" + "\\r\\n";
                                TextBox_AB_ITIME.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_ITIME.BackColor = Color.White;
                            }

                            if (!DateManange.check_Date(TextBox_AB_ODATE.Text.PadLeft(7, '0')))
                            {
                                ErrorString += "實際出國日期迄有誤" + "\\r\\n";
                                TextBox_AB_ODATE.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_ODATE.BackColor = Color.White;
                            }

                            if (TextBox_AB_OTIME.Text.Length != 4)
                            {
                                ErrorString += "實際出國時數格式錯誤" + "\\r\\n";
                                TextBox_AB_OTIME.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_AB_OTIME.BackColor = Color.White;
                            }

                            //出國需填寫詳細旅遊地點
                            if (string.IsNullOrEmpty(TextBox_MZ_TADD.Text.Trim()))
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('出國或赴大陸相關地區,請詳細填寫地點！');", true);
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TADD.ClientID + "').focus();$get('" + TextBox_MZ_TADD.ClientID + "').focus();", true);
                                return;
                            }
                        }
                        #endregion

                        #region 檢查上傳檔案
                        F = CHECK_UPLOADFILE(ViewState["dpath1"] == null ? "" : ViewState["dpath1"].ToString(), ErrorString, FileUpload1, Button_DelFILE1, HyperLink_FILENAME1, "MZ_FILE", "dpath1");
                        F1 = CHECK_UPLOADFILE(ViewState["dpath2"] == null ? "" : ViewState["dpath2"].ToString(), ErrorString, FileUpload2, Button_DelFILE2, HyperLink_FILENAME2, "MZ_FILE1", "dpath2");
                        F2 = CHECK_UPLOADFILE(ViewState["dpath3"] == null ? "" : ViewState["dpath3"].ToString(), ErrorString, FileUpload3, Button_DelFILE3, HyperLink_FILENAME3, "MZ_FILE2", "dpath3");
                        #endregion

                        if (!string.IsNullOrEmpty(ErrorString.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改有問題之欄位！');", true);
                            return;
                        }
                        //檢查必填欄位
                        if (!Check_Field())
                        {
                            return;
                        }

                        if (RadioButtonList_SIGN_KIND.SelectedValue.ToString() == "2")
                        {
                            if (TextBox_MZ_CODE.Text.Trim() == "38")
                            {
                                if (F == string.Empty && F1 == string.Empty && F2 == string.Empty)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('疫苗接種假必須檢核附件！');", true);
                                    return;
                                }
                            }

                            if (TextBox_MZ_CODE.Text.Trim() == "15" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22" || TextBox_MZ_CODE.Text.Trim() == "16")
                            {
                                if (F == string.Empty && F1 == string.Empty && F2 == string.Empty)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('加班補休假必須檢核附件！');", true);
                                    return;
                                }
                            }
                        }

                        string DAYFACT = "", FUNERAL_TYPE = "";


                        if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "05" || TextBox_MZ_CODE.Text == "08" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "10" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "06")
                        {
                            if (TextBox_MZ_CODE.Text == "06")//公假
                            {
                                FUNERAL_TYPE = DropDownList_funeral_type6.SelectedValue;
                                DAYFACT = TextBox_dayfact.Text;

                                if (DropDownList_funeral_type6.SelectedValue == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請挑選類型！');", true);
                                    return;
                                }
                                else
                                {
                                    if (DropDownList_funeral_type6.SelectedValue == "3")
                                    {
                                        if (TextBox_dayfact.Text == "")
                                        {
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                            return;
                                        }
                                    }

                                    if (DropDownList_funeral_type6.SelectedValue == "1")
                                    {
                                        if (RadioButtonList_SIGN_KIND.SelectedValue.ToString() == "2")
                                        {
                                            if (F == string.Empty && F1 == string.Empty && F2 == string.Empty)
                                            {
                                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('公假必須檢核附件！');", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }

                            if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "10")//婚假 流產假
                            {
                                FUNERAL_TYPE = "";
                                DAYFACT = TextBox_dayfact.Text;

                                if (TextBox_dayfact.Text == "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                    return;
                                }
                            }

                            if (TextBox_MZ_CODE.Text == "08")//娩假
                            {
                                FUNERAL_TYPE = DropDownList_funeral_type8.SelectedValue;
                                DAYFACT = TextBox_dayfact.Text;

                                if (DropDownList_funeral_type8.SelectedValue == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請挑選類型！');", true);
                                    return;
                                }
                                else
                                {
                                    if (TextBox_dayfact.Text == "")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                        return;
                                    }
                                }
                            }

                            if (TextBox_MZ_CODE.Text == "05")//喪假
                            {
                                FUNERAL_TYPE = DropDownList_funeral_type5.SelectedValue;
                                DAYFACT = TextBox_dayfact.Text;

                                if (DropDownList_funeral_type5.SelectedValue == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請挑選類型！');", true);
                                    return;
                                }
                                else
                                {
                                    if (TextBox_dayfact.Text == "")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入事實發生日！');", true);
                                        return;
                                    }
                                }
                            }
                        }

                        #region 檢查選補休是否已選可補休日
                        //if (TextBox_MZ_CODE.Text.Trim() == "15" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22" || TextBox_MZ_CODE.Text.Trim() == "16")
                        //{
                        //    if (Session["MZ_RESTHOUR_DT"] == null)
                        //    {
                        //        //未選補休日
                        //        if (ViewState["Mode"].ToString() == "UPDATE")
                        //        {
                        //            //檢查已申請日
                        //            //2021/03/26 Session["PKEY_MZ_IDATE1"].ToString() 無值 改 TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                        //            //Mark暫時取消判斷
                        //            //int c = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='{0}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_RESTDATE LIKE '%{1}%'", TextBox_MZ_ID.Text.Trim(), TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'))));
                        //            //c += int.Parse(o_DBFactory.ABC_toTest.Get_First_Field(string.Format(@"Select COUNT(*) REST_HOUR From C_OVERTIME_FOR_DLTB01 
                        //            //                                                            Where MZ_DLTB01_SN='{0}' ", ViewState["SN"].ToStringNullSafe()), null));
                        //            //if (c <= 0)
                        //            //{
                        //            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                        //            //    return;
                        //            //}
                        //        }
                        //        else
                        //        {
                        //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                        //            return;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        DataTable dt = new DataTable();
                        //        dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                        //        if (dt.Rows.Count == 0)
                        //        {
                        //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                        //            return;
                        //        }
                        //    }
                        //}
                        #endregion


                        //第一二年保留假及今年慰勞假 須控管天數
                        //irk 請明年的假 102/01/02
                        int year = int.Parse(TextBox_MZ_IDATE1.Text.Substring(0, 3));
                        //if (TextBox_MZ_CODE.Text.Trim() == "03" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22")
                        if (TextBox_MZ_CODE.Text.Trim() == "03")  //03休假
                        {
                            #region 判斷是否能再請假
                            //bool can = true;//是否能在請假

                            //string SelectString = "";
                            ////求出該人所有慰勞假
                            //if (TextBox_MZ_CODE.Text == "03")
                            //{
                            //    SelectString = " SELECT MZ_HDAY,MZ_HTIME " +
                            //                   " FROM C_DLTBB " +
                            //                   " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            //}
                            ////Mark新版加班語差勤隱藏
                            ////else if (TextBox_MZ_CODE.Text == "11")
                            ////{
                            ////    SelectString = " SELECT MZ_SDAY,MZ_SDAY_HOUR " +
                            ////                   " FROM C_DLTBB " +
                            ////                   " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            ////}
                            ////else if (TextBox_MZ_CODE.Text == "22")
                            ////{
                            ////    SelectString = " SELECT MZ_SDAY2,MZ_SDAY2_HOUR " +
                            ////                   " FROM C_DLTBB " +
                            ////                   " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            ////}

                            //DataTable temp = new DataTable();

                            //temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

                            ////以下是計算是否還能再請假！
                            ////20140120
                            //int day = 0;
                            //int hour = 0;

                            //string strDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (year).ToString().PadLeft(3, '0') + "'";
                            //DataTable DAY_Count = o_DBFactory.ABC_toTest.Create_Table(strDAY_Count, "get");

                            //if (DAY_Count.Rows.Count > 0)
                            //{
                            //    day = int.Parse(DAY_Count.Rows[0]["MZ_TDAY"].ToString());

                            //    hour = int.Parse(DAY_Count.Rows[0]["MZ_TTIME"].ToString());
                            //}

                            ////以下是計算是否還能再請假！
                            //int day_can_use = 0;
                            //int hour_can_use = 0;

                            //int old_TDAY = 0;
                            //int old_TTIME = 0;

                            //if (ViewState["Mode"].ToString() == "UPDATE")
                            //{
                            //    old_TDAY = int.Parse(ViewState["old_TDAY"].ToString());
                            //    old_TTIME = int.Parse(ViewState["old_TTIME"].ToString());
                            //}

                            //if (temp == null || temp.Rows.Count == 0)
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + year + "年休假日未計算！')", true);
                            //    btCancel.Enabled = true;
                            //    return;
                            //}

                            //if (Convert.ToInt32(temp.Rows[0][1]) + old_TTIME - hour % 8 < 0)
                            //{
                            //    day_can_use = Convert.ToInt32(temp.Rows[0][0]) + old_TDAY - day - hour / 8 - 1;
                            //    hour_can_use = Convert.ToInt32(temp.Rows[0][1]) + old_TDAY + 8 - hour % 8;
                            //}
                            //else
                            //{
                            //    day_can_use = Convert.ToInt32(temp.Rows[0][0]) + old_TDAY - day - hour / 8;
                            //    hour_can_use = Convert.ToInt32(temp.Rows[0][1]) + old_TTIME;
                            //}

                            //if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            //{
                            //    if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                            //    {
                            //        can = false;
                            //    }
                            //    else if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) == 0 && hour_can_use < 0)
                            //    {
                            //        can = false;
                            //    }
                            //    else
                            //    {
                            //        can = true;
                            //    }
                            //}
                            //else if (string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            //{
                            //    if (day_can_use == 0 && hour_can_use - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                            //    {
                            //        can = false;
                            //    }
                            //    else if (day_can_use - 1 == 0 && hour_can_use + 8 - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                            //    {
                            //        can = false;
                            //    }
                            //    else
                            //    {
                            //        can = true;
                            //    }
                            //}
                            //else if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            //{
                            //    if (hour_can_use - int.Parse(TextBox_MZ_TTIME.Text) < 0)
                            //    {
                            //        if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) - 1 < 0)
                            //        {
                            //            can = false;
                            //        }
                            //        else
                            //        {
                            //            can = true;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                            //        {
                            //            can = false;
                            //        }
                            //        else
                            //        {
                            //            can = true;
                            //        }
                            //    }
                            //}

                            ////沒超過才能塞資料庫！
                            //if (!can)
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + TextBox_MZ_CODE1.Text + "已超過上限！')", true);
                            //    btCancel.Enabled = true;
                            //}
                            //else
                            //{

                            //}
                            #endregion
                            //20140310 ki
                            string ABDAYS = "";
                            if (TextBox_AB_IDATE.Text.Trim() == "" || TextBox_AB_ODATE.Text.Trim() == "")
                            {
                                ABDAYS = TextBox_MZ_TADD.Text;
                            }
                            else
                            {
                                if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                                {
                                    ABDAYS = TextBox_MZ_TADD.Text + "$" +
                                        TextBox_AB_IDATE.Text.Trim() + "$" + TextBox_AB_ITIME.Text.Trim() + "$" +
                                        TextBox_AB_ODATE.Text.Trim() + "$" + TextBox_AB_OTIME.Text.Trim() + "$" +
                                        TextBox_AB_TDAY.Text.Trim() + "$" + TextBox_AB_TTIME.Text.Trim();
                                }
                            }
                            string oldCODE = Session["PKEY_MZ_CODE"].ToString();
                            //補休不得更改請假事由、請假日期、共計時日
                            if (oldCODE == "11" || oldCODE == "15" || oldCODE == "16" || oldCODE == "22")
                            {
                                int iday = 0;
                                int itime = int.Parse(TextBox_MZ_TTIME.Text);
                                if (TextBox_MZ_TDAY.Text != "0")
                                { iday = int.Parse(TextBox_MZ_TDAY.Text) * 8; }
                                string strnow = Convert.ToString(iday + itime);
                                string strold = Session["PKEY_TOTAL_TIME"].ToString();
                                if (strnow != strold)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假總時數不得更動！');", true);
                                    return;
                                }
                            }




                            string strSQL = "";
                            if (oldCODE == "11" || oldCODE == "15" || oldCODE == "16" || oldCODE == "22")
                            {
                                strSQL = "UPDATE C_DLTB01 SET MZ_ID =@MZ_ID,MZ_NAME =@MZ_NAME,MZ_EXAD =@MZ_EXAD,MZ_EXUNIT =@MZ_EXUNIT,MZ_OCCC =@MZ_OCCC,MZ_RANK1 =@MZ_RANK1,MZ_SYSDAY =@MZ_SYSDAY,"
                                    + "MZ_IDATE1 =@MZ_IDATE1,MZ_ITIME1 =@MZ_ITIME1,MZ_ODATE =@MZ_ODATE,MZ_OTIME =@MZ_OTIME,MZ_MEMO =@MZ_MEMO,MZ_SWT =@MZ_SWT,MZ_TADD =@MZ_TADD,MZ_ROCCC =@MZ_ROCCC,MZ_CHK1 =@MZ_CHK1,MZ_SYSTIME =@MZ_SYSTIME,MZ_FOREIGN =@MZ_FOREIGN,MZ_CHINA =@MZ_CHINA,"
                                    + "MZ_FILE =@MZ_FILE,SIGN_KIND =@SIGN_KIND,MZ_FILE1 =@MZ_FILE1,MZ_FILE2 =@MZ_FILE2 ,MZ_LastYearJobLocation = @MZ_LastYearJobLocation,DAYFACT =@DAYFACT , FUNERAL_TYPE =@FUNERAL_TYPE"
                                    + " WHERE MZ_ID = '" + Session["PKEY_MZ_ID"] + "' AND MZ_IDATE1 = '" + Session["PKEY_MZ_IDATE1"] + "' AND MZ_ITIME1 = '" + Session["PKEY_MZ_ITIME1"].ToString().Substring(0, 2) + ":" + Session["PKEY_MZ_ITIME1"].ToString().Substring(2, 2) + "'";
                            }
                            else
                            {
                                strSQL = "UPDATE C_DLTB01 SET MZ_ID =@MZ_ID,MZ_NAME =@MZ_NAME,MZ_EXAD =@MZ_EXAD,MZ_EXUNIT =@MZ_EXUNIT,MZ_OCCC =@MZ_OCCC,MZ_RANK1 =@MZ_RANK1,MZ_SYSDAY =@MZ_SYSDAY,"
                                    + " MZ_IDATE1 =@MZ_IDATE1,MZ_ITIME1 =@MZ_ITIME1,MZ_ODATE =@MZ_ODATE,MZ_OTIME =@MZ_OTIME,MZ_TDAY =@MZ_TDAY,MZ_TTIME =@MZ_TTIME,MZ_CAUSE =@MZ_CAUSE,MZ_MEMO =@MZ_MEMO,"
                                    + "MZ_SWT =@MZ_SWT,MZ_TADD =@MZ_TADD,MZ_ROCCC =@MZ_ROCCC,MZ_CHK1 =@MZ_CHK1,MZ_SYSTIME =@MZ_SYSTIME,MZ_FOREIGN =@MZ_FOREIGN,MZ_CHINA =@MZ_CHINA,MZ_FILE =@MZ_FILE,"
                                    + "SIGN_KIND =@SIGN_KIND,MZ_FILE1 =@MZ_FILE1,MZ_FILE2 =@MZ_FILE2, MZ_LastYearJobLocation = @MZ_LastYearJobLocation  ,DAYFACT=@DAYFACT , FUNERAL_TYPE=@FUNERAL_TYPE"
                                    + " WHERE MZ_ID='" + Session["PKEY_MZ_ID"] + "' AND MZ_IDATE1='" + Session["PKEY_MZ_IDATE1"] + "' AND MZ_ITIME1='" + Session["PKEY_MZ_ITIME1"].ToString().Substring(0, 2) + ":" + Session["PKEY_MZ_ITIME1"].ToString().Substring(2, 2) + "'";

                            }
                            if (oldCODE == "11" || oldCODE == "15" || oldCODE == "16" || oldCODE == "22")
                            {
                                SqlParameter[] parameterList = {
                                    new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
                                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = TextBox_MZ_NAME.Text},
                                    new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_SYSDAY",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_SYSDAY.Text.Trim())?Convert.DBNull: TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_IDATE1",SqlDbType.VarChar){Value =  string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim())?Convert.DBNull:TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_ITIME1",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2)},
                                    new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ODATE.Text.Trim())?Convert.DBNull:TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_OTIME",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2)},
                                    new SqlParameter("MZ_MEMO",SqlDbType.VarChar){Value = TextBox_MZ_MEMO.Text},
                                    new SqlParameter("MZ_SWT",SqlDbType.VarChar){Value = CheckBox_MZ_SWT.Checked?"Y":"N"},
                                    new SqlParameter("MZ_TADD",SqlDbType.VarChar){Value = ABDAYS},
                                    new SqlParameter("MZ_LastYearJobLocation",SqlDbType.VarChar){Value = TextBox_MZ_LastYearJobLocation.Text},
                                    new SqlParameter("MZ_ROCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'")},
                                    new SqlParameter("MZ_CHK1",SqlDbType.VarChar){Value = TextBox_MZ_CHK1.Text},
                                    new SqlParameter("MZ_SYSTIME",SqlDbType.VarChar){Value = TextBox_MZ_SYSTIME.Text},
                                    new SqlParameter("MZ_FOREIGN",SqlDbType.VarChar){Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N"},
                                    new SqlParameter("MZ_CHINA",SqlDbType.VarChar){Value = CheckBox_MZ_CHINA.Checked?"Y":"N"},
                                    new SqlParameter("MZ_FILE",SqlDbType.VarChar){Value = F},
                                    new SqlParameter("MZ_FILE1",SqlDbType.VarChar){Value = F1},
                                    new SqlParameter("MZ_FILE2",SqlDbType.VarChar){Value = F2},
                                    new SqlParameter("SIGN_KIND",SqlDbType.VarChar){Value = RadioButtonList_SIGN_KIND.SelectedValue},
                                    new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                    new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                                    };

                                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                                LogModel.saveLog("C", "A", strSQL, parameterList, Request.QueryString["TPM_FION"], "修改假單");
                            }
                            else
                            {
                                SqlParameter[] parameterList = {
                                    new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
                                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = TextBox_MZ_NAME.Text},
                                    new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text)},
                                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'")},
                                    new SqlParameter("MZ_SYSDAY",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_SYSDAY.Text.Trim())?Convert.DBNull: TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_IDATE1",SqlDbType.VarChar){Value =  string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim())?Convert.DBNull:TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_ITIME1",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2)},
                                    new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_ODATE.Text.Trim())?Convert.DBNull:TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                                    new SqlParameter("MZ_OTIME",SqlDbType.VarChar){Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2)},
                                    new SqlParameter("MZ_TDAY",SqlDbType.Float){Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())?"0":TextBox_MZ_TDAY.Text)},
                                    new SqlParameter("MZ_TTIME",SqlDbType.Float){Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())?"0":TextBox_MZ_TTIME.Text)},
                                    new SqlParameter("MZ_CAUSE",SqlDbType.VarChar){Value = TextBox_MZ_CAUSE.Text},
                                    new SqlParameter("MZ_MEMO",SqlDbType.VarChar){Value = TextBox_MZ_MEMO.Text},
                                    new SqlParameter("MZ_SWT",SqlDbType.VarChar){Value = CheckBox_MZ_SWT.Checked?"Y":"N"},
                                    new SqlParameter("MZ_TADD",SqlDbType.VarChar){Value = ABDAYS},
                                    new SqlParameter("MZ_LastYearJobLocation",SqlDbType.VarChar){Value = TextBox_MZ_LastYearJobLocation.Text},
                                    new SqlParameter("MZ_ROCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'")},
                                    new SqlParameter("MZ_CHK1",SqlDbType.VarChar){Value = TextBox_MZ_CHK1.Text},
                                    new SqlParameter("MZ_SYSTIME",SqlDbType.VarChar){Value = TextBox_MZ_SYSTIME.Text},
                                    new SqlParameter("MZ_FOREIGN",SqlDbType.VarChar){Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N"},
                                    new SqlParameter("MZ_CHINA",SqlDbType.VarChar){Value = CheckBox_MZ_CHINA.Checked?"Y":"N"},
                                    new SqlParameter("MZ_FILE",SqlDbType.VarChar){Value = F},
                                    new SqlParameter("MZ_FILE1",SqlDbType.VarChar){Value = F1},
                                    new SqlParameter("MZ_FILE2",SqlDbType.VarChar){Value = F2},
                                    new SqlParameter("SIGN_KIND",SqlDbType.VarChar){Value = RadioButtonList_SIGN_KIND.SelectedValue},
                                    new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                    new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                                    };

                                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                                LogModel.saveLog("C", "A", strSQL, parameterList, Request.QueryString["TPM_FION"], "修改假單");

                            }

                            //o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);
                            //Log
                            //    LogModel.saveLog("C", "A", ViewState["CMDSQL"].ToString(), parameterList, Request.QueryString["TPM_FION"], "修改假單");

                            //}


                            Session.Remove("PKEY_MZ_ID");
                            Session.Remove("PKEY_MZ_IDATE1");
                            Session.Remove("PKEY_MZ_ITIME1");
                        }
                        else
                        {
                            #region 變更補休類別的更新時數
                            //if (TextBox_MZ_CODE.Text.Trim() == "15")
                            //{
                            //    CHECK_15_old();
                            //}
                            //else
                            //{
                            //    CHECK_15();
                            //}
                            #endregion

                            #region 檢查該假別是否有設定上限
                            //string overCodeDay = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'");

                            //bool can = true;

                            //if (!string.IsNullOrEmpty(overCodeDay.Trim()))
                            //{

                            //    int code_day = 0;
                            //    int code_hour = 0;

                            //    //取得已請天數時數
                            //    string strcodeDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                            //    DataTable codeDAY_Count = o_DBFactory.ABC_toTest.Create_Table(strcodeDAY_Count, "get");

                            //    if (codeDAY_Count.Rows.Count > 0)
                            //    {
                            //        code_day = int.Parse(codeDAY_Count.Rows[0]["MZ_TDAY"].ToString());

                            //        code_hour = int.Parse(codeDAY_Count.Rows[0]["MZ_TTIME"].ToString());
                            //    }

                            //    //20140120
                            //    int code_day_can_use = 0;
                            //    int code_hour_can_use = 0;

                            //    int old_TDAY = 0;
                            //    int old_TTIME = 0;

                            //    if (ViewState["Mode"].ToString() == "UPDATE")
                            //    {
                            //        //修改狀態時取回已請天數時數
                            //        old_TDAY = int.Parse(ViewState["old_TDAY"].ToString());
                            //        old_TTIME = int.Parse(ViewState["old_TTIME"].ToString());
                            //    }

                            //    if (int.Parse(overCodeDay) - code_hour % 8 < 0)
                            //    {
                            //        code_day_can_use = int.Parse(overCodeDay) + old_TDAY - code_day - code_hour / 8 - 1;
                            //        code_hour_can_use = 8 - code_hour % 8;
                            //    }
                            //    else
                            //    {
                            //        code_day_can_use = int.Parse(overCodeDay) + old_TDAY - code_day - code_hour / 8;
                            //    }

                            //    if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            //    {
                            //        if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                            //        {
                            //            can = false;
                            //        }
                            //        else if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) == 0 && code_day_can_use < 0)
                            //        {
                            //            can = false;
                            //        }
                            //        else
                            //        {
                            //            can = true;
                            //        }
                            //    }
                            //    else if (string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            //    {
                            //        if (code_day_can_use == 0 && code_hour_can_use - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                            //        {
                            //            can = false;
                            //        }
                            //        else if (code_day_can_use - 1 == 0 && code_hour_can_use + 8 - int.Parse(TextBox_MZ_TTIME.Text.Trim()) < 0)
                            //        {
                            //            can = false;
                            //        }
                            //        else
                            //        {
                            //            can = true;
                            //        }
                            //    }
                            //    else if (!string.IsNullOrEmpty(TextBox_MZ_TDAY.Text == "0" ? "" : TextBox_MZ_TDAY.Text) && !string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim() == "0" ? "" : TextBox_MZ_TTIME.Text))
                            //    {
                            //        if (code_hour_can_use - int.Parse(TextBox_MZ_TTIME.Text) < 0)
                            //        {
                            //            if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) - 1 < 0)
                            //            {
                            //                can = false;
                            //            }
                            //            else
                            //            {
                            //                can = true;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            if (code_day_can_use - int.Parse(TextBox_MZ_TDAY.Text.Trim()) < 0)
                            //            {
                            //                can = false;
                            //            }
                            //            else
                            //            {
                            //                can = true;
                            //            }
                            //        }
                            //    }
                            //}

                            //if (!can)
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + TextBox_MZ_CODE1.Text + "已超過上限！請注意')", true);
                            //}
                            #endregion
                            //20140310 ki
                            string ABDAYS = "";
                            if (TextBox_AB_IDATE.Text.Trim() == "" || TextBox_AB_ODATE.Text.Trim() == "")
                            {
                                ABDAYS = TextBox_MZ_TADD.Text;
                            }
                            else
                            {
                                if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                                {
                                    ABDAYS = TextBox_MZ_TADD.Text + "$" +
                                            TextBox_AB_IDATE.Text.Trim() + "$" + TextBox_AB_ITIME.Text.Trim() + "$" +
                                            TextBox_AB_ODATE.Text.Trim() + "$" + TextBox_AB_OTIME.Text.Trim() + "$" +
                                            TextBox_AB_TDAY.Text.Trim() + "$" + TextBox_AB_TTIME.Text.Trim();
                                }

                            }

                            string oldCODE = Session["PKEY_MZ_CODE"].ToString();
                            if (oldCODE == "11" || oldCODE == "15" || oldCODE == "16" || oldCODE == "22")
                            {
                                int iday = 0;
                                int itime = int.Parse(TextBox_MZ_TTIME.Text);
                                if (TextBox_MZ_TDAY.Text != "0")
                                { iday = int.Parse(TextBox_MZ_TDAY.Text) * 8; }
                                string strnow = Convert.ToString(iday + itime);
                                string strold = Session["PKEY_TOTAL_TIME"].ToString();
                                if (strnow != strold)
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假總時數不得更動！');", true);
                                    return;
                                }
                            }

                            string strSQL = "";
                            if (oldCODE == "11" || oldCODE == "15" || oldCODE == "16" || oldCODE == "22")
                            {
                                strSQL = "UPDATE C_DLTB01 SET MZ_ID =@MZ_ID,MZ_NAME =@MZ_NAME,MZ_EXAD =@MZ_EXAD,MZ_EXUNIT =@MZ_EXUNIT,MZ_OCCC =@MZ_OCCC,MZ_RANK1 =@MZ_RANK1,MZ_SYSDAY =@MZ_SYSDAY,"
                                    + "MZ_IDATE1 =@MZ_IDATE1,MZ_ITIME1 =@MZ_ITIME1,MZ_ODATE =@MZ_ODATE,MZ_OTIME =@MZ_OTIME,MZ_MEMO =@MZ_MEMO,MZ_SWT =@MZ_SWT,MZ_TADD =@MZ_TADD,MZ_ROCCC =@MZ_ROCCC,MZ_CHK1 =@MZ_CHK1,MZ_SYSTIME =@MZ_SYSTIME,MZ_FOREIGN =@MZ_FOREIGN,MZ_CHINA =@MZ_CHINA,"
                                    + "MZ_FILE =@MZ_FILE,SIGN_KIND =@SIGN_KIND,MZ_FILE1 =@MZ_FILE1,MZ_FILE2 =@MZ_FILE2 ,MZ_LastYearJobLocation = @MZ_LastYearJobLocation,DAYFACT =@DAYFACT , FUNERAL_TYPE =@FUNERAL_TYPE"
                                    + " WHERE MZ_ID = '" + Session["PKEY_MZ_ID"] + "' AND MZ_IDATE1 = '" + Session["PKEY_MZ_IDATE1"] + "' AND MZ_ITIME1 = '" + Session["PKEY_MZ_ITIME1"].ToString().Substring(0, 2) + ":" + Session["PKEY_MZ_ITIME1"].ToString().Substring(2, 2) + "'";
                            }
                            else
                            {
                                strSQL = "UPDATE C_DLTB01 SET MZ_ID =@MZ_ID,MZ_NAME =@MZ_NAME,MZ_EXAD =@MZ_EXAD,MZ_EXUNIT =@MZ_EXUNIT,MZ_OCCC =@MZ_OCCC,MZ_RANK1 =@MZ_RANK1,MZ_SYSDAY =@MZ_SYSDAY,"
                                    + " MZ_IDATE1 =@MZ_IDATE1,MZ_ITIME1 =@MZ_ITIME1,MZ_ODATE =@MZ_ODATE,MZ_OTIME =@MZ_OTIME,MZ_TDAY =@MZ_TDAY,MZ_TTIME =@MZ_TTIME,MZ_CAUSE =@MZ_CAUSE,MZ_MEMO =@MZ_MEMO,"
                                    + "MZ_SWT =@MZ_SWT,MZ_TADD =@MZ_TADD,MZ_ROCCC =@MZ_ROCCC,MZ_CHK1 =@MZ_CHK1,MZ_SYSTIME =@MZ_SYSTIME,MZ_FOREIGN =@MZ_FOREIGN,MZ_CHINA =@MZ_CHINA,MZ_FILE =@MZ_FILE,"
                                    + "SIGN_KIND =@SIGN_KIND,MZ_FILE1 =@MZ_FILE1,MZ_FILE2 =@MZ_FILE2, MZ_LastYearJobLocation = @MZ_LastYearJobLocation  ,DAYFACT=@DAYFACT , FUNERAL_TYPE=@FUNERAL_TYPE"
                                    + " WHERE MZ_ID='" + Session["PKEY_MZ_ID"] + "' AND MZ_IDATE1='" + Session["PKEY_MZ_IDATE1"] + "' AND MZ_ITIME1='" + Session["PKEY_MZ_ITIME1"].ToString().Substring(0, 2) + ":" + Session["PKEY_MZ_ITIME1"].ToString().Substring(2, 2) + "'";

                            }

                            if (oldCODE == "11" || oldCODE == "15" || oldCODE == "16" || oldCODE == "22")
                            {
                                SqlParameter[] parameterList = {
                                new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text },
                                new SqlParameter("MZ_NAME", SqlDbType.VarChar) { Value = TextBox_MZ_NAME.Text },
                                new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value =o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value =  o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_RANK1", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_SYSDAY", SqlDbType.VarChar) { Value = TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_IDATE1", SqlDbType.VarChar) { Value = TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_ITIME1", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2) },
                                new SqlParameter("MZ_ODATE", SqlDbType.VarChar) { Value = TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_OTIME", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2) },
                                new SqlParameter("MZ_MEMO", SqlDbType.VarChar) { Value = TextBox_MZ_MEMO.Text },
                                new SqlParameter("MZ_SWT", SqlDbType.VarChar) { Value = CheckBox_MZ_SWT.Checked?"Y":"N" },
                                new SqlParameter("MZ_TADD", SqlDbType.VarChar) { Value = ABDAYS },
                                new SqlParameter("MZ_LastYearJobLocation", SqlDbType.VarChar) { Value = TextBox_MZ_LastYearJobLocation.Text },
                                new SqlParameter("MZ_ROCCC", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'") },
                                new SqlParameter("MZ_CHK1", SqlDbType.VarChar) { Value = TextBox_MZ_CHK1.Text },
                                new SqlParameter("MZ_SYSTIME", SqlDbType.VarChar) { Value = TextBox_MZ_SYSTIME.Text },
                                new SqlParameter("MZ_FOREIGN", SqlDbType.VarChar) { Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N" },
                                new SqlParameter("MZ_CHINA", SqlDbType.VarChar) { Value = CheckBox_MZ_CHINA.Checked?"Y":"N" },
                                new SqlParameter("MZ_FILE", SqlDbType.VarChar) { Value = F },
                                new SqlParameter("MZ_FILE1", SqlDbType.VarChar) { Value = F1 },
                                new SqlParameter("MZ_FILE2", SqlDbType.VarChar) { Value = F2 },
                                new SqlParameter("SIGN_KIND", SqlDbType.VarChar) { Value = RadioButtonList_SIGN_KIND.SelectedValue },
                                new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                                };

                                //Log
                                LogModel.saveLog("C", "U", strSQL, parameterList, Request.QueryString["TPM_FION"], "修改假單");
                                //Mark暫時隱藏
                                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                                TextBox_MZ_CAUSE.Text = Session["PKEY_MZ_CAUSE"].ToString();
                            }
                            else
                            {
                                SqlParameter[] parameterList = {
                                new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text },
                                new SqlParameter("MZ_NAME", SqlDbType.VarChar) { Value = TextBox_MZ_NAME.Text },
                                new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value =o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) },
                                new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value =  o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_RANK1", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='"+TextBox_MZ_ID.Text+"'") },
                                new SqlParameter("MZ_SYSDAY", SqlDbType.VarChar) { Value = TextBox_MZ_SYSDAY.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_IDATE1", SqlDbType.VarChar) { Value = TextBox_MZ_IDATE1.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_ITIME1", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_ITIME1.Text.Trim())?"":TextBox_MZ_ITIME1.Text.Substring(0,2)+":"+TextBox_MZ_ITIME1.Text.Substring(2,2) },
                                new SqlParameter("MZ_ODATE", SqlDbType.VarChar) { Value = TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0') },
                                new SqlParameter("MZ_OTIME", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_OTIME.Text.Trim())?"":TextBox_MZ_OTIME.Text.Substring(0,2)+":"+TextBox_MZ_OTIME.Text.Substring(2,2) },
                                new SqlParameter("MZ_TDAY", SqlDbType.Float) { Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim())?"0":TextBox_MZ_TDAY.Text) },
                                new SqlParameter("MZ_TTIME", SqlDbType.Float) { Value = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim())?"0":TextBox_MZ_TTIME.Text) },
                                new SqlParameter("MZ_CAUSE", SqlDbType.VarChar) { Value = TextBox_MZ_CAUSE.Text },
                                new SqlParameter("MZ_MEMO", SqlDbType.VarChar) { Value = TextBox_MZ_MEMO.Text },
                                new SqlParameter("MZ_SWT", SqlDbType.VarChar) { Value = CheckBox_MZ_SWT.Checked?"Y":"N" },
                                new SqlParameter("MZ_TADD", SqlDbType.VarChar) { Value = ABDAYS },
                                new SqlParameter("MZ_LastYearJobLocation", SqlDbType.VarChar) { Value = TextBox_MZ_LastYearJobLocation.Text },
                                new SqlParameter("MZ_ROCCC", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_RNAME.SelectedValue.Trim()+"'") },
                                new SqlParameter("MZ_CHK1", SqlDbType.VarChar) { Value = TextBox_MZ_CHK1.Text },
                                new SqlParameter("MZ_SYSTIME", SqlDbType.VarChar) { Value = TextBox_MZ_SYSTIME.Text },
                                new SqlParameter("MZ_FOREIGN", SqlDbType.VarChar) { Value = CheckBox_MZ_FOREIGN.Checked?"Y":"N" },
                                new SqlParameter("MZ_CHINA", SqlDbType.VarChar) { Value = CheckBox_MZ_CHINA.Checked?"Y":"N" },
                                new SqlParameter("MZ_FILE", SqlDbType.VarChar) { Value = F },
                                new SqlParameter("MZ_FILE1", SqlDbType.VarChar) { Value = F1 },
                                new SqlParameter("MZ_FILE2", SqlDbType.VarChar) { Value = F2 },
                                new SqlParameter("SIGN_KIND", SqlDbType.VarChar) { Value = RadioButtonList_SIGN_KIND.SelectedValue },
                                new SqlParameter("DAYFACT", SqlDbType.VarChar) { Value = DAYFACT },
                                new SqlParameter("FUNERAL_TYPE", SqlDbType.VarChar) { Value = FUNERAL_TYPE }
                                };

                                //Log
                                LogModel.saveLog("C", "U", strSQL, parameterList, Request.QueryString["TPM_FION"], "修改假單");
                                //Mark暫時隱藏
                                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                            }

                        }
                        TextBox_MZ_CODE.Text = Session["PKEY_MZ_CODE"].ToString();
                        TextBox_MZ_CODE1.Text = Session["PKEY_MZ_CODE1"].ToString();
                        DropDownList_MZ_RNAME.SelectedValue = Session["PKEY_MZ_RNAME"].ToString();
                        TextBox_MZ_ROCCC.Text = Session["PKEY_MZ_ROCCC"].ToString();


                        Session.Remove("PKEY_MZ_ID");
                        Session.Remove("PKEY_MZ_IDATE1");
                        Session.Remove("PKEY_MZ_ODATE");
                        Session.Remove("PKEY_MZ_ITIME1");
                        Session.Remove("PKEY_MZ_OTIME");
                        Session.Remove("PKEY_MZ_CODE");
                        Session.Remove("PKEY_MZ_RNAME");
                        Session.Remove("PKEY_MZ_CAUSE");
                        Session.Remove("PKEY_MZ_ROCCC0");

                    }

                    //編輯個人差假天數
                    else if (MultiView1.ActiveViewIndex == 4)
                    {
                        //暫時刪除以確保不會再修改到資料 20200303 by sky
                        //SqlParameter[] parameterList = {
                        //    new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text },
                        //    new SqlParameter("MZ_HDAY", SqlDbType.Float) { Value = TextBox_MZ_HDAY.Text },
                        //    new SqlParameter("MZ_HTIME", SqlDbType.Float) { Value = TextBox_MZ_HTIME.Text },
                        //    new SqlParameter("MZ_SDAY", SqlDbType.Float) { Value = TextBox_MZ_SDAY.Text },
                        //    new SqlParameter("MZ_SDAYHOUR", SqlDbType.Float) { Value = TextBox_MZ_SDAY_HOUR.Text },
                        //    new SqlParameter("MZ_SDAY2", SqlDbType.Float) { Value = TextBox_MZ_SDAY2.Text },
                        //    new SqlParameter("MZ_SDAY2HOUR", SqlDbType.Float) { Value = TextBox_MZ_SDAY2_HOUR.Text },
                        //    new SqlParameter("MZ_SDAY3", SqlDbType.Float) { Value = TextBox_MZ_HDAY.Text },
                        //    new SqlParameter("MZ_SDAY3HOUR", SqlDbType.Float) { Value = TextBox_MZ_HTIME.Text },
                        //    new SqlParameter("MZ_PDAY", SqlDbType.Float) { Value = TextBox_MZ_PDAY.Text },
                        //    new SqlParameter("MZ_PHOUR", SqlDbType.Float) { Value = TextBox_MZ_PHOUR.Text },
                        //    new SqlParameter("MZ_SICKDAY", SqlDbType.Float) { Value = TextBox_MZ_SICKDAY.Text },
                        //    new SqlParameter("MZ_SICKHOUR", SqlDbType.Float) { Value = TextBox_MZ_SICKHOUR.Text },
                        //    new SqlParameter("MZ_HCAREDAY", SqlDbType.Float) { Value = TextBox_MZ_HCAREDAY.Text },
                        //    new SqlParameter("MZ_HCAREHOUR", SqlDbType.Float) { Value = TextBox_MZ_HCAREHOUR.Text },
                        //    new SqlParameter("MZ_YEAR", SqlDbType.VarChar) { Value = TextBox_MZ_YEAR.Text }
                        //};

                        //o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                        //string updateStr = string.Format(@"UPDATE A_DLBASE SET MZ_TYEAR='{0}', MZ_TMONTH='{1}', MZ_MEMO='{2}' WHERE MZ_ID='{3}'"
                        //                                , TextBox_MZ_TYEAR.Text.Trim()
                        //                                , TextBox_MZ_TMONTH.Text.Trim()
                        //                                , TextBox_MZ_TMEMO.Text.Trim()
                        //                                , TextBox_MZ_ID.Text.Trim());

                        //o_DBFactory.ABC_toTest.Edit_Data(updateStr);

                        //2010.06.04 LOG紀錄 by伊珊
                        //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), updateString);
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改假單成功');", true);

                    #endregion
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btOK2.Enabled = false;
                    btCancel.Enabled = false;
                    btSearch.Enabled = true;

                    if ((ViewState["C_strGID"].ToString() == "A" || ViewState["C_strGID"].ToString() == "B" || ViewState["C_strGID"].ToString() == "C"))
                    {
                        btDelete.Enabled = true;
                    }
                    else
                    {
                        btDelete.Enabled = false;
                    }

                    ViewState.Remove("Mode");
                    C.controlEnable(ref this.Panel2, false);
                    C.controlEnable(ref this.Panel7, false);


                }
                catch (Exception ex)
                {
                    string aa = ex.Message.ToString();
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無法修改補休日期，若要修改，\\r\\n請差假承辦人刪除該筆假單後，重新新增。');", true);
            }
        }

        protected void isOnline(string Mode)
        {
            if (RadioButtonList_SIGN_KIND.SelectedValue == "2")
            {
                int count = System.Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_REVIEW_MANAGEMENT WHERE MZ_EXAD = '" + o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_EXUNIT = '" + o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) + "'  AND REVIEW_LEVEL = '2'"));

                if (count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('找不到該單位主管，請洽相關人員')", true);
                    return;
                }
                else
                {
                    DataTable temp = new DataTable();
                    temp = o_DBFactory.ABC_toTest.Create_Table("SELECT * FROM C_REVIEW_MANAGEMENT WHERE MZ_EXAD = '" + o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_EXUNIT = '" + o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) + "' AND REVIEW_LEVEL = '2'", "get");

                    DataTable temp2 = o_DBFactory.ABC_toTest.Create_Table("SELECT MZ_DLTB01_SN,MZ_FILE,MZ_FILE1,MZ_FILE2 FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_SYSTIME='" + TextBox_MZ_SYSTIME.Text + "'", "get");

                    ViewState["SN"] = temp2.Rows[0]["MZ_DLTB01_SN"].ToString();
                    //ViewState["SN"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DLTB01_SN FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_SYSTIME='" + TextBox_MZ_SYSTIME.Text + "'");

                    CSreportSign Sign = new CSreportSign(LEAVE_SNCHECK(), ViewState["SN"].ToString());

                    if (Mode == "UPDATE" && Sign.getReturn())
                    {
                        return;
                    }

                    o_DBFactory.ABC_toTest.Edit_Data(string.Format(@"DELETE FROM C_LEAVE_HISTORY WHERE DLTB01_SN={0} AND RETURN_FLAG IS NULL", ViewState["SN"].ToString()));

                    //HyperLink_FILENAME1.NavigateUrl = show_upload_path() + temp2.Rows[0]["MZ_FILE"].ToString();
                    //HyperLink_FILENAME2.NavigateUrl = show_upload_path() + temp2.Rows[0]["MZ_FILE1"].ToString();
                    //HyperLink_FILENAME3.NavigateUrl = show_upload_path() + temp2.Rows[0]["MZ_FILE2"].ToString();

                    string path = temp2.Rows[0]["MZ_FILE"].ToString();
                    string path1 = temp2.Rows[0]["MZ_FILE1"].ToString();
                    string path2 = temp2.Rows[0]["MZ_FILE2"].ToString();

                    if (path != "")
                    {
                        HyperLink_FILENAME1.Visible = true;
                        HyperLink_FILENAME1.NavigateUrl = show_upload_path() + path;
                        Button_DelFILE1.Visible = true;
                        FileUpload1.Visible = false;
                    }
                    else
                    {
                        HyperLink_FILENAME1.Visible = false;
                        Button_DelFILE1.Visible = false;
                        FileUpload1.Visible = true;
                    }


                    if (path1 != "")
                    {
                        HyperLink_FILENAME2.Visible = true;
                        HyperLink_FILENAME2.NavigateUrl = show_upload_path() + path1;
                        Button_DelFILE2.Visible = true;
                        FileUpload2.Visible = false;
                    }
                    else
                    {
                        HyperLink_FILENAME2.Visible = false;
                        Button_DelFILE2.Visible = false;
                        FileUpload2.Visible = true;
                    }

                    if (path2 != "")
                    {
                        HyperLink_FILENAME3.Visible = true;
                        HyperLink_FILENAME3.NavigateUrl = show_upload_path() + path2;
                        Button_DelFILE3.Visible = true;
                        FileUpload3.Visible = false;
                    }
                    else
                    {
                        HyperLink_FILENAME3.Visible = false;
                        Button_DelFILE3.Visible = false;
                        FileUpload3.Visible = true;
                    }

                    //HyperLink_FILENAME1.NavigateUrl = show_upload_path() + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FILE FROM C_DLTB01 WHERE MZ_DLTB01_SN = '" + ViewState["SN"].ToString() + "'");
                    //HyperLink_FILENAME2.NavigateUrl = show_upload_path() + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FILE1 FROM C_DLTB01 WHERE MZ_DLTB01_SN = '" + ViewState["SN"].ToString() + "'");
                    //HyperLink_FILENAME3.NavigateUrl = show_upload_path() + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FILE2 FROM C_DLTB01 WHERE MZ_DLTB01_SN = '" + ViewState["SN"].ToString() + "'");
                    //if (FileUpload1.Visible == true)
                    //{
                    //    FileUpload1.Enabled = false;
                    //}
                    //if (Button_DelFILE1.Visible == true)
                    //{
                    //    Button_DelFILE1.Enabled = false;
                    //}
                    //if (FileUpload2.Visible == true)
                    //{
                    //    FileUpload2.Enabled = false;
                    //}
                    //if (Button_DelFILE2.Visible == true)
                    //{
                    //    Button_DelFILE2.Enabled = false;
                    //}
                    //if (FileUpload3.Visible == true)
                    //{
                    //    FileUpload3.Enabled = false;
                    //}
                    //if (Button_DelFILE3.Visible == true)
                    //{
                    //    Button_DelFILE3.Enabled = false;
                    //}
                    GV_CHECK_show();
                    Panel_select_ModalPopupExtender.Show();
                }
            }
            else if (RadioButtonList_SIGN_KIND.SelectedValue == "1")
            {
                ViewState["SN"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DLTB01_SN FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_SYSTIME='" + TextBox_MZ_SYSTIME.Text + "'");
                o_DBFactory.ABC_toTest.Edit_Data(string.Format(@"DELETE FROM C_LEAVE_HISTORY WHERE DLTB01_SN={0}", ViewState["SN"].ToString()));
            }
        }

        /// <summary>
        /// 檢查必填欄位
        /// </summary>
        /// <returns></returns>
        protected bool Check_Field()
        {
            bool result = true;

            if (string.IsNullOrEmpty(TextBox_MZ_CAUSE.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請假事由不可空白！');", true);
                result = false;
            }
            else if (string.IsNullOrEmpty(TextBox_MZ_TADD.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('旅遊或出差地點不可空白！');", true);
                result = false;
            }
            else if (string.IsNullOrEmpty(DropDownList_MZ_RNAME.SelectedItem.Text.Trim()))
            {
                if (TextBox_MZ_CODE.Text != "18")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('代理人不可空白！');", true);
                    result = false;
                }
            }
            return result;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                if (FileUpload1.Visible == true)
                {
                    FileUpload1.Enabled = false;
                }
                if (Button_DelFILE1.Visible == true)
                {
                    Button_DelFILE1.Enabled = false;
                }
                if (FileUpload2.Visible == true)
                {
                    FileUpload2.Enabled = false;
                }
                if (Button_DelFILE2.Visible == true)
                {
                    Button_DelFILE2.Enabled = false;
                }
                if (FileUpload3.Visible == true)
                {
                    FileUpload3.Enabled = false;
                }
                if (Button_DelFILE3.Visible == true)
                {
                    Button_DelFILE3.Enabled = false;
                }
                if (ViewState["CMDSQL"].ToString() == "INSERT")
                {
                    foreach (object dl in Panel2.Controls)
                    {
                        if (dl is DropDownList)
                        {
                            DropDownList dl1 = dl as DropDownList;
                        }

                        if (dl is TextBox)
                        {
                            TextBox tbox = dl as TextBox;
                            tbox.Text = "";
                        }
                    }
                    btUpdate.Enabled = false;
                }
                else if (ViewState["CMDSQL"].ToString() == "UPDATE")
                {
                    finddata(int.Parse(xcount.Text.Trim()));


                    DLTB01_MZ_ID = Session["DLTB01_MZ_ID"] as List<string>;////無法用VIEWSTATE
                    if (int.Parse(xcount.Text.Trim()) == 0 && DLTB01_MZ_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && DLTB01_MZ_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == DLTB01_MZ_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < DLTB01_MZ_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                }
                Session.Remove("PKEY_MZ_ID");
                Session.Remove("PKEY_MZ_IDATE1");
                Session.Remove("PKEY_MZ_ITIME1");
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btOK2.Enabled = false;
                btCancel.Enabled = false;

                //20140317
                btSearch.Enabled = true;
                //20140317

            }
            else if (MultiView1.ActiveViewIndex == 4)
            {

            }

            C.controlEnable(ref this.Panel2, false);
            C.controlEnable(ref this.Panel7, false);
        }
        /// <summary>
        /// 刪除假單按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                if (o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CHK1 FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ITIME1='" + TextBox_MZ_ITIME1.Text.Trim() + "'") != "Y")
                {
                    string DeleteString = "DELETE FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ITIME1='" + TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2) + "'";

                    string sn = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DLTB01_SN FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ITIME1='" + TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2) + "'");

                    string DelSQL = "DELETE FROM C_LEAVE_HISTORY WHERE DLTB01_SN = '" + sn + "'";

                    string path = Find_Url(sn, " MZ_FILE");

                    if (!string.IsNullOrEmpty(path.Trim()))
                    {
                        del_file(path);
                    }

                    string path1 = Find_Url(sn, " MZ_FILE1");
                    if (!string.IsNullOrEmpty(path1.Trim()))
                    {
                        del_file(path1);
                    }

                    string path2 = Find_Url(sn, " MZ_FILE2");
                    if (!string.IsNullOrEmpty(path2.Trim()))
                    {
                        del_file(path2);
                    }

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                        {
                            conn.Open();
                            SqlTransaction Ltransation;
                            Ltransation = conn.BeginTransaction();
                            try
                            {
                                SqlCommand cmd = new SqlCommand(DeleteString, conn);
                                cmd.Transaction = Ltransation;
                                cmd.ExecuteNonQuery();
                                //Log
                                LogModel.saveLog("C", "D", DeleteString, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "刪除假單");

                                cmd = new SqlCommand(DelSQL, conn);
                                cmd.Transaction = Ltransation;
                                cmd.ExecuteNonQuery();
                                //Log
                                LogModel.saveLog("C", "D", DelSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "刪除假單歷程");

                                Ltransation.Commit();
                            }
                            catch/*(Exception ex)*/
                            {
                                Ltransation.Rollback();
                            }
                        }

                        //o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                        //o_DBFactory.ABC_toTest.Edit_Data(DelSQL);//刪除請假歷程



                        if (!string.IsNullOrEmpty(xcount.Text.Trim()))
                        {

                            DLTB01_MZ_ID = Session["DLTB01_MZ_ID"] as List<string>;////無法用VIEWSTATE
                            DLTB01_MZ_IDATE1 = Session["DLTB01_MZ_IDATE1"] as List<string>;

                            DLTB01_MZ_ITIME1 = Session["DLTB01_MZ_ITIME1"] as List<String>;

                            DLTB01_MZ_ID.RemoveAt(int.Parse(xcount.Text));
                            DLTB01_MZ_IDATE1.RemoveAt(int.Parse(xcount.Text));

                            DLTB01_MZ_ITIME1.RemoveAt(int.Parse(xcount.Text));
                        }

                        if (TextBox_MZ_CODE.Text == "15")
                        {
                            //處理更新舊加班資料
                            // string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND  MZ_RESTDATE LIKE '%" + TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0') + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y'";
                            //string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND  MZ_RESTDATE LIKE '%" + TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0') + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y'";

                            //DataTable dt = new DataTable();

                            //dt = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getdeletevalue");

                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{

                            //    string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                            //    if (s.Count() == 1)
                            //    {
                            //        string UpdateSQL = string.Format(@"UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE=null,MZ_RESTHOUR=OTIME,LOCK_FLAG='N' WHERE MZ_DATE='{0}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{1}'"
                            //                                        , dt.Rows[i]["MZ_DATE"].ToString()
                            //                                        , TextBox_MZ_ID.Text.Trim());

                            //        //Log
                            //        LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "刪除假單，更新可補休日");

                            //        o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                            //    }
                            //    else
                            //    {
                            //        int hour = 0;
                            //        string RESTDATE = "";
                            //        for (int j = 0; j < s.Count(); j++)
                            //        {
                            //            if (s[j].Substring(0, 7) == TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0'))
                            //            {
                            //                RESTDATE = dt.Rows[i]["MZ_RESTDATE"].ToString().Replace("，" + s[j], "");

                            //                string[] y = s[j].Split('：');

                            //                hour = int.Parse(y[1]) + int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());

                            //                break;
                            //            }
                            //        }

                            //        if (RESTDATE != "")
                            //        {
                            //            //TODO 補休,建議建立流水號.用流水號做KEY修改補休時數使用情形,否則一下面這一句.如果同一天有兩種不同種類的加班.都會鎖住,
                            //            // 所以其他相對應的使用(包含收尋補休狀態都要調整)
                            //            string UpdateSQL = string.Format(@"UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='{0}',MZ_RESTHOUR={1} 
                            //                                                 WHERE MZ_DATE='{2}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{3}' "
                            //                                            , RESTDATE, hour, dt.Rows[i]["MZ_DATE"].ToString(), TextBox_MZ_ID.Text.Trim());

                            //            //Log
                            //            LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "刪除假單，更新可補休日");

                            //            o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                            //        }
                            //    }
                            //}

                            //處理新加班資料 , 還原原來加班資料 , 刪除關聯檔 Mark 20210530
                            string strSQL = string.Format(@"Select * From C_OVERTIME_FOR_DLTB01 Where MZ_DLTB01_SN='{0}' ", sn);
                            DataTable cobDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_FOR_DLTB01");
                            for (int i = 0; i < cobDt.Rows.Count; i++)
                            {
                                strSQL = string.Format("UPDATE C_OVERTIME_BASE Set REST_HOUR=(REST_HOUR-{1}), REST_ID='' Where SN='{0}' ",
                                    cobDt.Rows[i]["OVERTIME_SN"].ToStringNullSafe(), cobDt.Rows[i]["REST_HOUR"].ToStringNullSafe());
                                o_DBFactory.ABC_toTest.DealCommandLog(strSQL, null);

                                C_OVERTIME_BASE_Model cobModel = new C_OVERTIME_BASE_Model()
                                {
                                    SN = int.Parse(cobDt.Rows[i]["OVERTIME_SN"].ToStringNullSafe())
                                };
                                CFService.SynchronizeOverTime(cobModel, Request.QueryString["TPM_FION"].ToStringNullSafe());

                                strSQL = string.Format("DELETE FROM C_OVERTIME_FOR_DLTB01 Where OVERTIME_SN='{0}' And MZ_DLTB01_SN='{1}' ",
                                    cobDt.Rows[i]["OVERTIME_SN"].ToStringNullSafe(), cobDt.Rows[i]["MZ_DLTB01_SN"].ToStringNullSafe());
                                o_DBFactory.ABC_toTest.DealCommandLog(strSQL, null);
                            }
                        }

                        if (DLTB01_MZ_ID.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_ForLeaveBasic_New.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                            ViewState.Remove("SN");
                        }
                        else
                        {
                            xcount.Text = "0";
                            finddata(int.Parse(xcount.Text));

                            if (DLTB01_MZ_ID.Count > 1)
                            {
                                btNEXT.Enabled = true;
                            }

                            btUpdate.Visible = true;
                            btDelete.Visible = true;
                            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DLTB01_MZ_ID.Count.ToString() + "筆";
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                        }

                    }
                    catch
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已核定！不可刪除！');", true);
                }
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
            }
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {

            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != DLTB01_MZ_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DLTB01_MZ_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == DLTB01_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == DLTB01_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + DLTB01_MZ_ID.Count.ToString() + "筆";

        }

        protected void DropDownList_MZ_RNAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ROCCC.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_RNAME.SelectedValue.Trim() + "'");
        }

        /// <summary>
        /// 按鈕: 選擇請假假別
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCODE_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_CODE.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_CODE1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=MZ_CODE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=550,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }
        /// <summary>
        /// 輸入框: 假別代碼變更及檢查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_CODE_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_CODE.Text = TextBox_MZ_CODE.Text.ToUpper();
            //選假別 開放與清空請假事由
            TextBox_MZ_CAUSE.Enabled = true;
            TextBox_MZ_CAUSE.Text = "";
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'");

            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_CODE1.Text = string.Empty;
                TextBox_MZ_CODE.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);
            }
            else
            {
                TextBox_MZ_CODE1.Text = CName;

            }

            if (TextBox_MZ_CODE.Text == "03")
            {
                CheckBox_MZ_SWT.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + CheckBox_MZ_SWT.ClientID + "').focus();$get('" + CheckBox_MZ_SWT.ClientID + "').focus();", true);
            }
            else
            {
                CheckBox_MZ_SWT.Enabled = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();", true);

            }

            if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "05" || TextBox_MZ_CODE.Text == "08" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "10" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "06")
            {
                dayfacttable.Visible = true;
                ftype.Visible = false;//標題  類型
                ftype6.Visible = false;//公假
                ftype8.Visible = false;//娩假
                ftype5.Visible = false;//喪亡對象
                qfact.Visible = false;//標題 事實發生日
                qfact1.Visible = false;//事實發生日

                if (TextBox_MZ_CODE.Text == "06")//公假
                {
                    ftype.Visible = true;//標題  類型
                    ftype6.Visible = true;//公假

                    if (DropDownList_funeral_type6.SelectedValue == "3")
                    {
                        qfact.Visible = true;
                        qfact1.Visible = true;//事實發生日
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + DropDownList_funeral_type6.ClientID + "').focus();$get('" + DropDownList_funeral_type6.ClientID + "').focus();", true);
                }

                if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "10")//婚假 流產假
                {
                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_dayfact.ClientID + "').focus();$get('" + TextBox_dayfact.ClientID + "').focus();", true);
                }

                if (TextBox_MZ_CODE.Text == "08")//娩假
                {
                    ftype.Visible = true;//標題  類型
                    ftype8.Visible = true;//娩假

                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + DropDownList_funeral_type8.ClientID + "').focus();$get('" + DropDownList_funeral_type8.ClientID + "').focus();", true);
                }

                if (TextBox_MZ_CODE.Text == "05")//喪假
                {
                    ftype.Visible = true;//標題  類型
                    ftype5.Visible = true;//喪假

                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + DropDownList_funeral_type5.ClientID + "').focus();$get('" + DropDownList_funeral_type5.ClientID + "').focus();", true);
                }
            }
            else
            {
                dayfacttable.Visible = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();", true);
            }

            if (TextBox_MZ_CODE.Text == "15" || TextBox_MZ_CODE.Text == "11" || TextBox_MZ_CODE.Text == "16" || TextBox_MZ_CODE.Text == "22")
            {
                if (ViewState["Mode"].ToString() != "Update")
                {
                    btRESTDATECHECK.Enabled = true;
                    TextBox_MZ_CAUSE.Enabled = false;
                    TextBox_MZ_CAUSE.Text = "";
                }
                else
                {
                    btRESTDATECHECK.Enabled = false;
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + btRESTDATECHECK.ClientID + "').focus();$get('" + btRESTDATECHECK.ClientID + "').focus();", true);
            }
            else
            {

                btRESTDATECHECK.Enabled = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();", true);
            }
        }

        protected void DropDownList_funeral_type6Changed(object sender, EventArgs e)
        {
            if (TextBox_MZ_CODE.Text == "06")
            {
                if (DropDownList_funeral_type6.SelectedValue == "3")
                {
                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日
                }
            }
            else
            {
                CheckBox_MZ_SWT.Enabled = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();$get('" + TextBox_MZ_IDATE1.ClientID + "').focus();", true);

            }
        }

        protected void returnSameDataType(TextBox tb, TextBox tb1)
        {
            tb.Text = tb.Text.Trim().Replace("/", "");

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
            }
        }



        protected void TextBox_MZ_SYSDAY_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_SYSDAY, TextBox_MZ_SYSTIME);
        }
        /// <summary>
        /// 按鈕: 打開可補休日頁面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btRESTDATECHECK_Click(object sender, EventArgs e)
        {
            int time = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text) ? "0" : TextBox_MZ_TDAY.Text) * 8 +
                int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);
            if (time == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先填寫請假時數。')", true);
                return;
            }

            Session["ForLeaveBasic_BT"] = BT_GV1.ClientID;
            //預計申請補休的小時數
            Session["ForLeaveBasic_CID1"] = time;
            Session["ForLaeveBasic_RESTDATE_ID"] = TextBox_MZ_ID.Text.Trim();
            Session["LeaveTime"] = time.ToString();
            //請假日
            Session["MZ_IDATE1"] = TextBox_MZ_IDATE1.Text;

            //加密參數傳遞模組
            EncryptParam ep = new EncryptParam();
            ep.dataDictionary["ForLeaveBasic_BT"] = BT_GV1.ClientID;
            //預計申請補休的小時數
            ep.dataDictionary["ForLeaveBasic_CID1"] = time;
            ep.dataDictionary["ForLaeveBasic_RESTDATE_ID"] = TextBox_MZ_ID.Text.Trim();
            ep.dataDictionary["LeaveTime"] = time.ToString();
            //請假日
            ep.dataDictionary["MZ_IDATE1"] = TextBox_MZ_IDATE1.Text;
            if (TextBox_MZ_CODE.Text == "15")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                    "window.open('C_ForLeaveOvertime_RESTDATE_SEARCH.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                    "window.open('C_ForLeaveOvertime_RESTDATE_SEARCH_New1.aspx?mz_code=" + TextBox_MZ_CODE.Text + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "&Param=" + ep.Export() + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
            }
        }


        int TPM_FION = 0;

        #region 報表

        /// <summary>
        /// 印請假單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_offduty_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {


                Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text;

                string tmp_url = "C_rpt.aspx?fn=offduty&DATE=" + TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0').Trim() +
                    "&TIME=" + (TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2)).Trim() +
                    "&MZ_NAME=" +
                    "&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
        }

        /// <summary>
        /// 印出差單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_businessTrip_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {


                Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

                string tmp_url = "C_rpt.aspx?fn=businessTrip" +
                    "&DATE=" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
                    //"&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim())+  
                    "&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
        }

        //印請假報告單
        protected void Button4_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_askleave_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            Response.Redirect("C_abroad_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"]);
        }


        //印出國請假單
        protected void Button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_abroad_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {

                Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

                string tmp_url = "C_rpt.aspx?fn=abroad&TPM_FION=" + TPM_FION +
                "&DATE=" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
                "&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim());

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
        }

        //印非請假出國報備單
        protected void Button8_Click(object sender, EventArgs e)
        {
            //Joy 20151007 原先設定 *-註解-*
            //if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_ondutyabroad_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            //}
            //else
            //{

            //    Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

            //    string tmp_url = "C_rpt.aspx?fn=ondutyabroad&TPM_FION=" + TPM_FION +
            //       "&DATE=" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
            //       "&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim());

            //    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            //}

            Response.Redirect("C_ondutyabroad_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"]);
        }

        #endregion 報表

        /// <summary> 核取 : 勾選大陸地區 </summary>
        protected void CheckBox_MZ_CHINA_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_MZ_CHINA.Checked == true)
            {
                CheckBox_MZ_FOREIGN.Checked = true;         //是否為國外
                tableLastYearJobLocation.Visible = true;    //最近一年工作職掌
                AB_DAY.Visible = true;                      //實際出國天數
            }
            else
            {
                CheckBox_MZ_FOREIGN.Checked = false;
                tableLastYearJobLocation.Visible = false;
                AB_DAY.Visible = false;
            }
        }

        protected void CheckBox_MZ_FOREIGN_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_MZ_FOREIGN.Checked == true || CheckBox_MZ_CHINA.Checked == true)
                AB_DAY.Visible = true;
            else
            {
                AB_DAY.Visible = false;

            }

        }

        protected void TBGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TBGridView1.PageIndex = e.NewPageIndex;

            TBGridView1.DataSource = Session["TBGridView1"] as DataTable;

            TBGridView1.DataBind();
        }

        protected void GridView_DLTB_UNUSUAL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_DLTB_UNUSUAL.PageIndex = e.NewPageIndex;

            GridView_DLTB_UNUSUAL.DataSource = Session["GridView_DLTB_UNUSUAL"] as DataTable;

            GridView_DLTB_UNUSUAL.DataBind();
        }

        /// <summary>
        /// 資料模型-每個月的加班時數
        /// </summary>
        public class MonthlyOverTime
        {
            /// <summary>
            /// 民國年月 YYYMM
            /// </summary>
            public string YearMonth { get; set; }

            /// <summary>
            /// 當月剩餘可申請補修的加班時間
            /// </summary>
            public int Rest_Minute { get; set; }

            /// <summary>
            /// 當月預計目標要申請補修的加班時間,必須為60倍數,且不能讓整體超過要申請的上限
            /// </summary>
            public int Target_Minute { get; set; }

            /// <summary>
            /// 當月已經申請補休的加班時間
            /// </summary>
            public int Applyed_Minute { get; set; }

        }

        /// <summary>
        /// 按鈕(隱): 自動填寫請假事由
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BT_GV1_Click(object sender, EventArgs e)
        {
            //選擇要申請補休的可補休日

            //從 Session["MZ_RESTHOUR_DT"] 抓取資料並且轉換成DataTable
            //經過重新計算之後,再寫回 Session["MZ_RESTHOUR_DT"]

            //裡面的資料欄位規劃的有點問題,目前已知定義如下
            //  OVER_DAY:加班日或者加班月,這邊比較麻煩的是同一個欄位會有兩種定義,這邊可能是用字碼長短來區別之
            //  MZ_ID:員警的身分證
            //  MZ_RESTHOUR:預計要申請補休的時數或分鐘數,根據Session["CHECK"]區別之,true為分鐘數,反之為時數
            //  另外,
            //  MZ_ID和MZ_RESTHOUR正常情況下,DataTable內所有資料,該欄位內容都會長一樣
            //
            //  EX: 員警A123456789,110年02月~04月的加班要申請8小時補休
            //      OVER_DAY    MZ_ID   MZ_RESTHOUR
            //      11002   A123456789  8
            //      11003   A123456789  8
            //      11004   A123456789  8
            //
            //  EX: 員警A123456789,110年02月16日~19日的加班要申請8小時補休
            //      OVER_DAY    MZ_ID       MZ_RESTHOUR
            //      1100216     A123456789  8
            //      1100217     A123456789  8
            //      1100218     A123456789  8
            DataTable dt = Session["MZ_RESTHOUR_DT"] as DataTable;
            string code = TextBox_MZ_CODE.Text;
            //如果是舊版的補休(目前應該已經被廢止了)
            if (code == "15")
            {
                string result = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (i == 0)
                        result += dt.Rows[i]["MZ_DATE"].ToString().Substring(0, 3) + "年" + dt.Rows[i]["MZ_DATE"].ToString().Substring(3, 2) + "月" + dt.Rows[i]["MZ_DATE"].ToString().Substring(5, 2) + "日";
                    else
                        result += "，" + dt.Rows[i]["MZ_DATE"].ToString().Substring(0, 3) + "年" + dt.Rows[i]["MZ_DATE"].ToString().Substring(3, 2) + "月" + dt.Rows[i]["MZ_DATE"].ToString().Substring(5, 2) + "日";

                    if (i == dt.Rows.Count - 1)
                    {
                        if (result != "")
                            result += "加班補休。";
                    }
                }

                TextBox_MZ_CAUSE.Text = result;
                TextBox_MZ_CAUSE.Enabled = false;  //請假事由鎖住

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TDAY.ClientID + "').focus();$get('" + TextBox_MZ_TDAY.ClientID + "').focus();", true);
            }
            else
            {
                //預計要輸出的資料(透過Session) 
                DataTable dtdata_b = new DataTable();
                dtdata_b.Columns.Add("OVER_DAY", typeof(string));//加班日
                dtdata_b.Columns.Add("OVER_TYPE", typeof(string));//加班類型代碼
                dtdata_b.Columns.Add("MZ_ID", typeof(string));//員警身分證
                dtdata_b.Columns.Add("MZ_RESTHOUR", typeof(string));//預計要申請加班的時數(實際上是分鐘數)

                //抓取SQL語法 指定日期的加班資料
                string MZ_IDATE1 = TextBox_MZ_IDATE1.Text;
                //int a = 0; //判斷有沒有多勾選不必要的天數

                if (dt != null && dt.Rows.Count > 0)
                {
                    /*
                     概念如下
                     -逐筆尋覽 Session["MZ_RESTHOUR_DT"]裡面的資料
                     -抓取出對應日期或月份的加班資料
                     -取得加班日剩餘的可加班時數(實際上是分鐘數)
                     -扣除掉可加班時數(實際上是分鐘數)
                     */

                    List<string> result = new List<string>();
                    //累加已經申請的_可補休分鐘數
                    //int surplusTotal_Applyed = 0;


                    //先統計每個月到底有多少時間可以申請補休,每個月預計目標要安排多少分鐘?
                    List<MonthlyOverTime> list_MonthlyOverTime = new List<MonthlyOverTime>();
                    //迴圈結構如下:
                    //for   已經勾選要申請補休的加班日期(或月份)
                    //  for     每天加班的資料(一天可能會有多筆?)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string dateStr = dt.Rows[i]["OVER_DAY"].ToStringNullSafe();
                        //抓取加班資料
                        string mz_id = dt.Rows[i]["MZ_ID"].ToStringNullSafe();
                        string sql = GetSQL_C_OVERTIME_BASE(code, dateStr, mz_id, MZ_IDATE1);
                        DataTable dtdata = o_DBFactory.ABC_toTest.Create_Table(sql, "C_OVERTIME_BASE");
                        //  for     每天加班的資料
                        for (int j = 0; j < dtdata.Rows.Count; j++)
                        {
                            //當天的 加班剩餘總時數(其實是分鐘數)
                            int SURPLUS_TOTAL = Convert.ToInt32(dtdata.Rows[j]["SURPLUS_TOTAL"]);
                            //判斷字典里面,對應月份是否存在?
                            //抓取月份=> yyyMM
                            string YearMonth = dateStr.Substring(0, 5);
                            //當月的統計摘要
                            //找不到的話,就納入
                            var month_info = list_MonthlyOverTime.Find(x => x.YearMonth == YearMonth);
                            if (month_info == null)
                            {
                                month_info = new MonthlyOverTime();
                                //綁定年月,納入清單中
                                month_info.YearMonth = YearMonth;
                                list_MonthlyOverTime.Add(month_info);
                            }
                            //將可申請加班的時數(其實是分鐘數)累加進去
                            month_info.Rest_Minute += SURPLUS_TOTAL;
                        }
                    }

                    //預計要申請補休的分鐘數,須為整小時,也就是60倍數
                    int targetTotal_resthour =
                        int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text) ? "0" : TextBox_MZ_TDAY.Text) * 8
                        + int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);
                    //預定要申請的總分鐘數
                    targetTotal_resthour = targetTotal_resthour * 60;

                    //改計算方法,不管選哪個結果都不變了
                    //if (Session["CHECK"].ToString() == "true")
                    //{
                    //    //targetTotal_resthour = int.Parse(dt.Rows[0]["MZ_RESTHOUR"].ToString());
                    //}
                    //else {
                    //    //targetTotal_resthour = int.Parse(dt.Rows[0]["MZ_RESTHOUR"].ToString()) * 60;
                    //}

                    //計算當月預計申請目標
                    /*
                     計算說明
                     1.判斷這個月可申請的數目 如果 超過預計申請目標 則以申請目標為主
                     EX: 這個月可申請365分鐘,但 剩餘的預計申請目標只要300分鐘,那麼這個月就只要請300分鐘
                     2.
                     這個月可申請的分鐘數 沒超過 剩餘的預計申請目標,代表這個月可以All IN
                     但是要注意每個月申請分鐘數為60倍數的問題
                     EX: 這個月可申請325分鐘,但 剩餘的預計申請目標要480分鐘,這個月申請目標只能到300分鐘(60倍數)

                     */
                    int 剩餘的預計申請目標 = targetTotal_resthour;
                    foreach (var item in list_MonthlyOverTime)
                    {

                        //判斷這個月可申請的數目 如果 超過預計申請目標 則以申請目標為主
                        if (item.Rest_Minute > 剩餘的預計申請目標)
                        {
                            item.Target_Minute = 剩餘的預計申請目標;
                        }
                        else
                        {   //這個月可申請的分鐘數 還沒超過 剩餘的預計申請目標,代表這個月可以All IN
                            //但是要注意每個月申請分鐘數為60倍數的問題  

                            //計算本月為60倍數的最大值,作為本月的預計申請分鐘數
                            item.Target_Minute = (item.Rest_Minute / 60) * 60;
                        }
                        //這個月的申請目標已經決定後,再從 剩餘的預計申請目標 扣除之
                        剩餘的預計申請目標 = 剩餘的預計申請目標 - item.Target_Minute;
                    }

                    //將記錄到的可加班分鐘數抄錄下來
                    string msg = DateTime.Now.ToString();
                    foreach (var item in list_MonthlyOverTime)
                    {
                        msg += ";加班月份" + item.YearMonth
                            + ":可申請分鐘數" + item.Rest_Minute
                            + ":當月目標分鐘數" + item.Target_Minute
                            ;
                    }
                    Log.SaveLog("C_ForLeaveBasic_New", "1", msg);

                    //for   已經勾選要申請補休的加班日期
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string dateStr = dt.Rows[i]["OVER_DAY"].ToStringNullSafe();
                        //判斷是可補休還是刪除多餘申請時數

                        string mz_id = dt.Rows[i]["MZ_ID"].ToStringNullSafe();

                        //抓取SQL語法 指定日期的加班資料
                        string sql = GetSQL_C_OVERTIME_BASE(code, dateStr, mz_id, MZ_IDATE1);
                        //抓取加班資料
                        //string sql = "select * from C_OVERTIME_BASE where MZ_ID = '" + mz_id + "' and " + stroverday + " = " + dateStr + "  and SURPLUS_TOTAL > 0 and ( OVERTIME_TYPE = '" + strtype + "' ) order by OVER_DAY";
                        DataTable dtdata = o_DBFactory.ABC_toTest.Create_Table(sql, "C_OVERTIME_BASE");

                        //  for     每天加班的資料
                        for (int j = 0; j < dtdata.Rows.Count; j++)
                        {
                            DataRow dr = dtdata_b.NewRow();
                            //加班日
                            dr["OVER_DAY"] = dtdata.Rows[j]["OVER_DAY"].ToStringNullSafe();
                            //抓取月份=> yyyMM
                            string YearMonth = dateStr.Substring(0, 5);


                            //抓取當月可申請加班的時數(其實是分鐘數)
                            var month_Info = list_MonthlyOverTime.Find(x => x.YearMonth == YearMonth);
                            //查無則重新建構,但是不該發生
                            if (month_Info == null) month_Info = new MonthlyOverTime();

                            Log.SaveLog("C_ForLeaveBasic_New", "1", YearMonth + "月份剩餘" + month_Info.Rest_Minute + "分鐘");



                            //Mark增加ID
                            dr["MZ_ID"] = mz_id;
                            //累進當天的 加班剩餘總時數(其實是分鐘數)
                            //surplusTotal_Applyed += Convert.ToInt32(dtdata.Rows[j]["SURPLUS_TOTAL"]);
                            //本回合 當天加班剩餘總時數(其實是分鐘數)
                            int itotal = Convert.ToInt32(dtdata.Rows[j]["SURPLUS_TOTAL"]);
                            //如果是 11加班補休 16超勤補休 22值日補休
                            if (code == "11" || code == "16" || code == "22")
                            {
                                int MZ_RESTHOUR = 0;//要申請捕休的分鐘數

                                //如果預計要申請的加總已經達到目標,後面就不用再動作了
                                if (month_Info.Applyed_Minute >= month_Info.Target_Minute)
                                {
                                    continue;
                                }
                                //判斷 今天的分鐘數All in 會不會超過本月預計目標?
                                bool isOverTarget = (itotal + month_Info.Applyed_Minute) > month_Info.Target_Minute;
                                if (isOverTarget)
                                {
                                    //會爆掉,看距離目標還差多少? 只申請這些差額
                                    MZ_RESTHOUR = month_Info.Target_Minute - month_Info.Applyed_Minute;
                                }
                                else
                                {   //今天的分鐘數All in
                                    MZ_RESTHOUR = itotal;
                                }
                                //將已申請的分鐘數累加與紀錄,方便後續回合使用
                                month_Info.Applyed_Minute = month_Info.Applyed_Minute + MZ_RESTHOUR;
                                //紀錄LOG
                                Log.SaveLog("C_ForLeaveBasic_New", "1", YearMonth + "月份申請" + MZ_RESTHOUR + "分鐘,目前累積:" + month_Info.Applyed_Minute + "分鐘,目標:" + month_Info.Target_Minute + "分鐘");

                                //則將當天的 可補休分鐘數 算入
                                dr["MZ_RESTHOUR"] = MZ_RESTHOUR;
                                dr["OVER_TYPE"] = dtdata.Rows[j]["OVERTIME_TYPE"];
                                dateStr = dtdata.Rows[j]["OVER_DAY"].ToString();
                                result.Add(string.Format("{0}年{1}月{2}日", dateStr.SubstringOutToEmpty(0, 3), dateStr.SubstringOutToEmpty(3, 2), dateStr.SubstringOutToEmpty(5, 2)));

                                //如果當天真的有分鐘數,才納入
                                if (MZ_RESTHOUR > 0)
                                {
                                    //紀錄LOG
                                    SetLog_MZ_RESTHOUR_DT(dr, "BT_GV1_Click");

                                    dtdata_b.Rows.Add(dr);
                                }
                            }
                            ////如果不是11加班補休
                            //else
                            //{
                            //    //如果 預計申請的時數 小於等於 累進當天的加班剩餘總時數(其實是分鐘數)
                            //    //代表 本回合累加的總時數(其實是分鐘數) 剛好跨過 申請目標
                            //    if (targetTotal_resthour < surplusTotal_Applyed)
                            //    {
                            //        //已申請的累加減去總目標,得之間的差值
                            //        dr["MZ_RESTHOUR"] = surplusTotal_Applyed - targetTotal_resthour;
                            //        dr["OVER_TYPE"] = dtdata.Rows[j]["OVERTIME_TYPE"];
                            //        dateStr = dtdata.Rows[j]["OVER_DAY"].ToString();
                            //        result.Add(string.Format("{0}年{1}月{2}日", dateStr.SubstringOutToEmpty(0, 3), dateStr.SubstringOutToEmpty(3, 2), dateStr.SubstringOutToEmpty(5, 2)));
                            //        //目標不用清0
                            //        //target_resthour = 0;

                            //        //紀錄LOG
                            //        SetLog_MZ_RESTHOUR_DT(dr);
                            //        dtdata_b.Rows.Add(dr);
                            //        //跳脫這次迴圈
                            //        break;
                            //    }
                            //    else
                            //    {
                            //        dr["MZ_RESTHOUR"] = itotal;
                            //        dr["OVER_TYPE"] = dtdata.Rows[j]["OVERTIME_TYPE"];
                            //        dateStr = dtdata.Rows[j]["OVER_DAY"].ToString();
                            //        result.Add(string.Format("{0}年{1}月{2}日", dateStr.SubstringOutToEmpty(0, 3), dateStr.SubstringOutToEmpty(3, 2), dateStr.SubstringOutToEmpty(5, 2)));
                            //        //紀錄LOG
                            //        SetLog_MZ_RESTHOUR_DT(dr);
                            //        dtdata_b.Rows.Add(dr);
                            //    }
                            //}

                        }
                    }
                    Session["MZ_RESTHOUR_DT"] = dtdata_b;
                    //逐筆尋覽預計要申請的資料


                    TextBox_MZ_CAUSE.Text = string.Join("，", result.ToArray());
                    if (result.Count > 0 && TextBox_MZ_CODE.Text == "11")
                    {
                        TextBox_MZ_CAUSE.Text += "加班補休。";
                        TextBox_MZ_CAUSE.Enabled = false; //請假事由鎖住
                    }
                    else if (result.Count > 0 && TextBox_MZ_CODE.Text == "16")
                    {
                        TextBox_MZ_CAUSE.Text += "超勤補休。";
                        TextBox_MZ_CAUSE.Enabled = false; //請假事由鎖住
                    }
                    else if (result.Count > 0 && TextBox_MZ_CODE.Text == "22")
                    {
                        TextBox_MZ_CAUSE.Text += "值日補休。";
                        TextBox_MZ_CAUSE.Enabled = false; //請假事由鎖住
                    }
                    //「開始日」(TextBox_MZ_IDATE1)、「結束日」(TextBox_MZ_ODATE)一併反灰 
                    TextBox_MZ_IDATE1.Enabled = false;
                    TextBox_MZ_ODATE.Enabled = false;

                    ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(string), "", string.Format("$get('{0}').focus();$get('{0}').focus();", TextBox_MZ_TDAY.ClientID), true);
                }
            }

        }

        private static void SetLog_MZ_RESTHOUR_DT(DataRow dr, string funcName = "")
        {
            string msg = funcName + " 加班日:" + dr["OVER_DAY"].ToString()
                                                    + ",OVER_TYPE:" + dr["OVER_TYPE"].ToString()
                                                    + ",MZ_ID:" + dr["MZ_ID"].ToString()
                                                    + ",MZ_RESTHOUR:" + dr["MZ_RESTHOUR"].ToString();
            Log.SaveLog("C_ForLeaveBasic_New", "1", msg);
            //return msg;
        }

        /// <summary>
        /// 抓取SQL語法 指定日期的加班資料
        /// </summary>
        /// <param name="code">請假假別
        /// 11 加班補休
        /// 22 值日捕休
        /// 16 超勤補休
        /// </param>
        /// <param name="dateStr">要搜尋的日期字串,注意有可能是五碼或者七碼</param>
        /// <param name="mz_id">身分證號</param>
        /// <param name="MZ_IDATE1">請補修日期,YYYMMDD 
        /// 針對dateStr五碼的版本,因為五碼是抓一整個月,
        /// 有可能遇到從加班到補休日太久,規定上不能申請的問題,所以需要此參數推算最早可以抓到的補休日在哪一天</param>
        /// <returns></returns>
        private static string GetSQL_C_OVERTIME_BASE(string code, string dateStr, string mz_id, string MZ_IDATE1)
        {

            //西元轉民國年
            var tmp = new System.Globalization.TaiwanCalendar();
            //篩選加班類型
            string strtype = "";
            //判斷加班類型

            //加班日條件,注意這邊有可能是5碼(YYYMM) 或者 7碼(YYYMMDD)
            //預設給7碼日期(YYYMMDD)
            string cond_OVER_DAY = " and OVER_DAY =  '" + dateStr + @"' ";
            //如果是給5碼年月(YYYMM), 11 加班補休 有可能會用這種抓法
            if (dateStr.Length == 5)
            {
                //如果你沒定義MZ_IDATE1
                if (MZ_IDATE1.IsNullOrEmpty())
                {   //改從當月一號開始,不過正常不應該發生
                    MZ_IDATE1 = dateStr + "01";
                }
                //計算:如果這天請補休,那最早可以使用哪一天開始的加班?
                string OVERDAY_LIMIT = CFService.Caculate_OVERDAY_LIMIT(MZ_IDATE1);

                //今天的年月日
                //string today = DateTime.Now.ToString("yyyyMMdd");
                //日期字串 去年的今月今日 超過一年就不能申請了
                //string strOverday = string.Format("{0}{1}{2}", tmp.GetYear(DateTime.Now.AddYears(-1)), today.Substring(4, 2), today.Substring(6, 2));
                //只比對 年月 ,且排除掉一年以前的加班資料
                cond_OVER_DAY = "and substr(OVER_DAY,0,5)='" + dateStr.Substring(0, 5) + "' and OVER_DAY >= '" + OVERDAY_LIMIT + "' ";
            }
            //11 加班補休
            if (code == "11")
            {
                strtype = "OTB' OR OVERTIME_TYPE = 'OTT";
            }
            //16 超勤補休
            else if (code == "16") { strtype = "OTU"; }
            //22 值日捕休
            else { strtype = "OTD"; }
            //抓取有加班的資料

            string SQL = @" 
select * from C_OVERTIME_BASE 
where MZ_ID = '" + mz_id + "' "
+ cond_OVER_DAY
+ @" and SURPLUS_TOTAL > 0 and ( OVERTIME_TYPE = '" + strtype + "' ) order by OVER_DAY";
            //記錄拼接出來的SQL語法到LOG
            Log.SaveLog("C_ForLeaveBasic_New.aspx", "1", SQL);

            return SQL;
        }

        protected void btn_check_Click(object sender, EventArgs e)
        {

            string code = LEAVE_SNCHECK();
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");

            string strSQL = "INSERT INTO " + "C_LEAVE_HISTORY" +
                                                           "(SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN)" +
                                                     " VALUES" +
                                                           "( NEXT VALUE FOR dbo.C_LEAVE_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN)";

            SqlParameter[] parameterList = {

                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = code},

                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = "0001"},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = ViewState["SN"].ToString()},
                    };

            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
            ViewState.Remove("s");
            Panel_select_ModalPopupExtender.Hide();
        }
        //決定請假流程
        protected string LEAVE_SNCHECK()
        {
            string EXAD = o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim());
            string EXUNIT = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim());
            string OCCC = string.Empty;
            string code = string.Empty;
            OCCC = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'");
            if (EXAD == "382130000C")
            {
                if (TextBox_MZ_CODE.Text == "07" || TextBox_MZ_CODE.Text == "06")
                {
                    code = "02";
                }
                else if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                {
                    code = "01";
                }
                else
                {
                    code = "03";
                }
            }
            else if (EXAD == "382130100C" || EXAD == "382130200C" || EXAD == "382130300C")
            {
                if (TextBox_MZ_CODE.Text == "07" || TextBox_MZ_CODE.Text == "06")
                {
                    code = "05";
                }
                else if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                {
                    code = "04";
                }
                else if (OCCC == "1020" || OCCC == "1403" || OCCC == "1413")//設定局長或隊長編號
                {
                    code = "07";
                }
                else
                {
                    code = "06";
                }
            }
            else if (EXAD == "382130400C" || EXAD == "382135000C")
            {
                if (TextBox_MZ_CODE.Text == "07" || TextBox_MZ_CODE.Text == "06")
                {
                    code = "13";
                }
                else if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                {
                    code = "12";
                }
                else if (OCCC == "1413")//設定局長或隊長編號
                {
                    code = "15";
                }
                else
                {
                    code = "14";
                }
            }
            else
            {
                if (TextBox_MZ_CODE.Text == "07" || TextBox_MZ_CODE.Text == "06")
                {
                    code = "09";
                }
                else if (CheckBox_MZ_CHINA.Checked == true || CheckBox_MZ_FOREIGN.Checked == true)
                {
                    code = "08";
                }
                else if (OCCC == "A401")//設定局長或隊長編號
                {
                    code = "11";
                }
                else
                {
                    code = "10";
                }
            }
            return code;
        }

        protected void btn_exit_Click(object sender, EventArgs e)
        {
            ViewState.Remove("s");
            Panel_select_ModalPopupExtender.Hide();
        }

        protected void GV_CHECK_show()
        {
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table("SELECT c_review_management.sn,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC) AS MZ_OCCC,a_dlbase.mz_name FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + o_A_DLBASE.PAD(TextBox_MZ_ID.Text.Trim()) + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text.Trim()) + "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'", "get");
            GV_CHECKER.DataSource = temp;
            GV_CHECKER.DataBind();
            ViewState["s"] = "y";
        }

        protected void GV_CHECKER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "checker")
            {
                string RID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RID FROM C_DLTB01 WHERE MZ_DLTB01_SN = '" + ViewState["SN"].ToString() + "'");
                string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + e.CommandArgument.ToString() + "'");
                string code = LEAVE_SNCHECK();
                string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                string today = year + DateTime.Now.ToString("MMdd");
                string now = DateTime.Now.ToString("HH:mm:ss");

                string strSQL = "INSERT INTO " + "C_LEAVE_HISTORY" +
                                                               "(SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID)" +
                                                         " VALUES" +
                                                               "( NEXT VALUE FOR dbo.C_LEAVE_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";

                SqlParameter[] parameterList = {
                new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = code},
                new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = RID},
                new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = "0001"},
                new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = ViewState["SN"].ToString()},
                new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                new SqlParameter("MZ_MID",SqlDbType.VarChar){Value = temp},
                                              };


                if (checSend(ViewState["SN"].ToString()))
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                ViewState.Remove("s");
                Panel_select_ModalPopupExtender.Hide();
            }
        }

        private bool checSend(string DLTB01_SN)
        {
            bool result = true;

            string strSQL = string.Format("SELECT COUNT(*) FROM C_LEAVE_HISTORY WHERE DLTB01_SN={0} AND RETURN_FLAG IS NULL", DLTB01_SN);

            int count;

            int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(strSQL), out count);

            result = count > 0 ? false : true;

            return result;
        }

        protected void GV_CHECKER_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_CHECKER.PageIndex = e.NewPageIndex;
            GV_CHECK_show();
        }

        protected void Button_DelFILE1_Command(object sender, CommandEventArgs e)
        {
            //del_file(e.CommandArgument.ToString());
            string path = string.Empty;
            if (ViewState["SN"] == null)
            {
                return;
            }
            path = Find_Url(ViewState["SN"].ToString(), "MZ_FILE");
            if (!string.IsNullOrEmpty(path.Trim()))
            {
                ViewState["dpath1"] = path;
            }
            FileUpload1.Visible = true;
            FileUpload1.Enabled = true;


            Button_DelFILE1.Visible = false;
            HyperLink_FILENAME1.Visible = false;

        }

        protected void Button_DelFILE2_Command(object sender, CommandEventArgs e)
        {
            //del_file(e.CommandArgument.ToString());
            string path = string.Empty;
            if (ViewState["SN"] == null)
            {
                return;
            }
            path = Find_Url(ViewState["SN"].ToString(), "MZ_FILE1");
            if (!string.IsNullOrEmpty(path.Trim()))
            {
                ViewState["dpath2"] = path;
            }
            FileUpload2.Visible = true;
            FileUpload2.Enabled = true;


            Button_DelFILE2.Visible = false;
            HyperLink_FILENAME2.Visible = false;
        }

        protected void Button_DelFILE3_Command(object sender, CommandEventArgs e)
        {
            //del_file(e.CommandArgument.ToString());
            string path = string.Empty;
            if (ViewState["SN"] == null)
            {
                return;
            }
            path = Find_Url(ViewState["SN"].ToString(), "MZ_FILE2");
            if (!string.IsNullOrEmpty(path.Trim()))
            {
                ViewState["dpath3"] = path;
            }
            FileUpload3.Visible = true;
            FileUpload3.Enabled = true;

            Button_DelFILE3.Visible = false;
            HyperLink_FILENAME3.Visible = false;
        }

        public string Find_Url(string bbsid, string filed)
        {
            string strSQL = @"select " + filed + @" FROM C_DLTB01 WHERE MZ_DLTB01_SN ='" + bbsid + "'";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            string File_Url;
            File_Url = temp.Rows[0][0].ToString();
            return File_Url;
        }

        public static string get_upload_path()
        {
            string rs = System.Configuration.ConfigurationManager.AppSettings["c_upload_filepath"];
            rs = System.Web.HttpContext.Current.Server.MapPath(rs);
            return rs;
        }

        public static string show_upload_path()
        {
            string rs = System.Configuration.ConfigurationManager.AppSettings["c_upload_showpath"];
            return rs;
        }

        protected void del_file(string del_url)
        {
            try
            {
                del_url = get_upload_path() + del_url;
                System.IO.File.Delete(del_url);
            }
            catch { }
        }

        public bool saveUploadFile(HttpPostedFile fu, string file_name)
        {
            try
            {
                HttpPostedFile tUploadFile = fu;
                int tFileLength = tUploadFile.ContentLength;
                byte[] tFileByte = new byte[tFileLength];
                tUploadFile.InputStream.Read(tFileByte, 0, tFileLength);
                if (tFileLength <= 10000000)  //限制檔案大小10000000
                {
                    // 如無此路徑，則先建立
                    if (!Directory.Exists(get_upload_path()))
                    {
                        Directory.CreateDirectory(get_upload_path());
                    }
                    var Extension = Path.GetExtension(file_name);//取得附檔名
                    file_name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Session["TPM_MID"].ToString() + Guid.NewGuid().ToString() + Extension.ToString();
                    FileStream tNewfile = new FileStream(get_upload_path() + file_name, FileMode.Create);
                    tNewfile.Write(tFileByte, 0, tFileByte.Length);
                    tNewfile.Close();
                    ViewState["path"] = file_name;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('檔案大小超過限制！');", true);
                    return false;
                }
            }

            catch
            {
                ViewState["path"] = string.Empty;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請選擇檔案！');", true);
                return false;
            }
            return true;

        }

        protected void DropDownList_MZ_RNAME_DataBound(object sender, EventArgs e)
        {
            DropDownList_MZ_RNAME.Items.Insert(0, new ListItem(" ", ""));
        }


        //2014/01/16
        /// <summary>
        /// 找個人資料
        /// </summary>
        /// <param name="ID_NO">身分證字號</param>
        public void fill_person_data(string ID_NO)
        {

            TextBox_MZ_ID.Text = ID_NO;
            TextBox_MZ_ID_Masked.Text = "XXXXX";
            //顯示遮罩後的身分證
            if (ID_NO.Length >= 10)
            {
                TextBox_MZ_ID_Masked.Text = ID_NO.Substring(0, 2) + "XXXXX" + ID_NO.Substring(7, 3);
            }

            //如果有建立人事代碼對應的View .這段可以修

            string strsql = @"SELECT MZ_NAME ,MZ_AD_CH,MZ_UNIT_CH,MZ_EXAD_CH, MZ_EXUNIT_CH,  MZ_OCCC_CH
                              FROM VW_A_DLBASE_S1  
                              
                              WHERE MZ_ID='" + ID_NO + "'";

            DataTable person_data = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");

            if (person_data.Rows.Count > 0)
            {
                TextBox_MZ_NAME.Text = person_data.Rows[0]["MZ_NAME"].ToString();
                //姓名遮罩
                TextBox_MZ_NAME_Masked.Text = MaskName(TextBox_MZ_NAME.Text, 'Ｏ');

                Label_MZ_AD.Text = person_data.Rows[0]["MZ_AD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_UNIT_CH"].ToString();

                Label_MZ_EXAD.Text = person_data.Rows[0]["MZ_EXAD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_EXUNIT_CH"].ToString() + "     " + person_data.Rows[0]["MZ_OCCC_CH"].ToString();
            }


        }
        /// <summary>
        /// 姓名遮罩
        /// Ｏ
        /// 一Ｏ
        /// 一Ｏ三
        /// 一ＯＯ四
        /// 一ＯＯＯ五
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        static string MaskName(string name, char maskChar)
        {
            if (name.Length <= 1)
            {
                return maskChar.ToString();
            }
            string visiblePartA = name.Substring(0, 1);
            if (name.Length <= 2)
            {
                return visiblePartA + maskChar.ToString();
            }
            int lengthToMask = Math.Max(name.Length - 2, 0); // Keep at least 1 character visible
            string maskedPart = new string(maskChar, lengthToMask);
            string visiblePartB = name.Substring(name.Length - 1, 1);

            return visiblePartA + maskedPart + visiblePartB;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string[] ABDays = e.Row.Cells[3].Text.ToString().Trim().ToUpper().Split('$');

                if (ABDays.Length > 1)
                {
                    e.Row.Cells[3].Text = ABDays[0].ToString().Trim();
                }
            }
        }

        /// <summary>
        /// 檢查補休情況
        /// </summary>
        protected bool check_15_Hour_old()
        {
            if (TextBox_MZ_CODE.Text == "15")//如果是補休
            {
                if (ViewState["Mode"].ToString() == "UPDATE")//如果是修改
                {
                    if (Session["MZ_RESTHOUR_DT"] != null)//有選填可補休日
                    {
                        DataTable dt = new DataTable();

                        dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                        string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND  MZ_RESTDATE LIKE '%" + Session["PKEY_MZ_IDATE1"].ToString() + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y'";

                        DataTable dt1 = new DataTable();

                        dt1 = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getdeletevalue");

                        int ReturnHour = 0;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string[] s = dt1.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                            if (s.Count() == 1)
                            {
                                ReturnHour += int.Parse(dt1.Rows[i]["MZ_RESTHOUR"].ToString());
                            }
                            else
                            {
                                for (int j = 0; j < s.Count(); j++)
                                {
                                    if (s[j].Substring(0, 7) == Session["PKEY_MZ_IDATE1"].ToString())
                                    {
                                        string[] y = s[j].Split('：');

                                        ReturnHour += int.Parse(y[1]);
                                    }
                                }

                            }
                        }

                        int totaltime = 0;

                        int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 + int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            totaltime += int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());
                        }
                        //如果 輸入時數 大於 所選補休時數
                        if (TTIME > totaltime + ReturnHour)
                        {
                            TextBox_MZ_TDAY.Text = "0";

                            TextBox_MZ_TTIME.Text = "0";

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                            return false;
                        }
                        //如果 所選補休時數 大於 輸入時數
                        else if (totaltime + ReturnHour > TTIME)
                        {
                            //計算參數s?
                            //加班時數加總 + ReturnHour(用途不明) - 預計要申請的時數
                            //應該是指超出申請的時數? 問題是兩者的單位不桐
                            int s = totaltime + ReturnHour - TTIME;
                            //int y = 0;
                            for (int i = dt.Rows.Count - 1; i > 0; i--)
                            {
                                //如果 
                                if (s > int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString()))
                                {
                                    s = s - int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());

                                    dt.Rows.Remove(dt.Rows[i]);

                                }
                                else if (s == int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString()))
                                {
                                    dt.Rows.Remove(dt.Rows[i]);
                                }
                                else
                                {
                                    dt.Rows[i]["MZ_RESTHOUR"] = int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString()) - s;
                                    //紀錄LOG
                                    SetLog_MZ_RESTHOUR_DT(dt.Rows[i], "check_15_Hour_old");
                                    break;
                                }
                            }
                        }
                    }
                    //沒選填可補休日.但其實有補修日可休
                    else if (o_DBFactory.ABC_toTest.vExecSQL("SELECT count(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND   MZ_RESTDATE LIKE '%" + Session["PKEY_MZ_IDATE1"].ToString() + "%'") != "0")
                    {
                        string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE   MZ_RESTDATE LIKE '%" + Session["PKEY_MZ_IDATE1"].ToString() + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' ";

                        DataTable dt1 = new DataTable();

                        dt1 = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getdeletevalue");

                        int ReturnHour = 0;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string[] s = dt1.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                            if (s.Count() == 1)
                            {
                                ReturnHour += int.Parse(dt1.Rows[i]["MZ_RESTHOUR"].ToString());

                                string[] y = s[0].Split('：');

                                ReturnHour += int.Parse(y[1]);

                            }
                            else
                            {
                                for (int j = 0; j < s.Count(); j++)
                                {
                                    if (s[j].Substring(0, 7) == Session["PKEY_MZ_IDATE1"].ToString())
                                    {
                                        string[] y = s[j].Split('：');

                                        ReturnHour += int.Parse(y[1]);
                                    }
                                }

                            }
                        }

                        int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 + int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);

                        if (TTIME > ReturnHour)
                        {
                            TextBox_MZ_TDAY.Text = "0";

                            TextBox_MZ_TTIME.Text = "0";

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                            return false;
                        }
                    }
                    else//沒有選填可補休日
                    {
                        TextBox_MZ_TDAY.Text = "0";

                        TextBox_MZ_TTIME.Text = "0";

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                        return false;
                    }
                }
                else//如果不是修改
                {
                    if (Session["MZ_RESTHOUR_DT"] != null)
                    {
                        DataTable dt = new DataTable();

                        dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                        int totaltime = 0;

                        int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 + int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            totaltime += int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());
                        }
                        //這邊似乎會有問題,TTIME的單位是小時,totaltime的單位是分鐘,所以兩者不應該比大小
                        //因為totaltime會大過TTIME,所以下面這條判斷式不會成立
                        if (TTIME > totaltime)
                        {
                            TextBox_MZ_TDAY.Text = "0";

                            TextBox_MZ_TTIME.Text = "0";

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                            return false;
                        }
                        //因為totaltime會大過TTIME,所以下面必跑
                        else if (totaltime > TTIME)
                        {
                            int s = totaltime - TTIME;
                            //int y = 0;
                            for (int i = dt.Rows.Count - 1; i > 0; i--)
                            {
                                if (s > int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString()))
                                {
                                    s = s - int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());

                                    dt.Rows.Remove(dt.Rows[i]);

                                }
                                else if (s == int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString()))
                                {
                                    dt.Rows.Remove(dt.Rows[i]);
                                }
                                else
                                {
                                    dt.Rows[i]["MZ_RESTHOUR"] = int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString()) - s;
                                    //紀錄LOG
                                    SetLog_MZ_RESTHOUR_DT(dt.Rows[i], "check_15_Hour_old");
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        TextBox_MZ_TDAY.Text = "0";

                        TextBox_MZ_TTIME.Text = "0";

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                        return false;
                    }
                }
            }//if
            //檢核通過
            return true;
        }

        /// <summary>
        /// 變更補休類別的更新時數
        /// </summary>
        protected void CHECK_15_old()
        {
            if (ViewState["old_CODE"] != null)
            {
                if (ViewState["old_CODE"].ToString() == "15")
                {
                    //vae Session["PKEY_MZ_IDATE1"].ToString()

                    string PKEY_MZ_IDATE1 = TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');


                    string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE  MZ_RESTDATE LIKE '%" + PKEY_MZ_IDATE1 + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";

                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getdeletevalue");

                    //判斷將補休變更為其他休假類別
                    if (TextBox_MZ_CODE.Text != ViewState["old_CODE"].ToString())
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                            if (s.Count() == 1)
                            {
                                //只有請過一筆資料
                                string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='',MZ_RESTHOUR=OTIME,LOCK_FLAG='N' WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' ";

                                //Log
                                LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假類別，更新可補休日");

                                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                            }
                            else
                            {
                                int ReturnHour = 0;
                                string RESTDATE = "";
                                for (int j = 0; j < s.Count(); j++)
                                {
                                    if (s[j].Substring(0, 7) == PKEY_MZ_IDATE1)
                                    {
                                        RESTDATE = dt.Rows[i]["MZ_RESTDATE"].ToString().Replace("，" + s[j], "");

                                        string[] y = s[j].Split('：');

                                        ReturnHour = int.Parse(y[1]) + int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());

                                        break;
                                    }
                                }

                                if (RESTDATE != "")
                                {
                                    string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + RESTDATE + "',MZ_RESTHOUR=" + ReturnHour + " WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y'  AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' ";

                                    //Log
                                    LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假類別，更新可補休日");

                                    o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TextBox_MZ_IDATE1.Text.PadLeft(7, '0') != PKEY_MZ_IDATE1)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                                if (s.Count() == 1)
                                {
                                    string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + s[0].Replace(PKEY_MZ_IDATE1, TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0')) + "',MZ_RESTHOUR=MZ_RESTHOUR WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_RESTHOUR>0";

                                    //Log
                                    LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假日期，更新可補休日");

                                    o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                }
                                else
                                {
                                    for (int j = 0; j < s.Count(); j++)
                                    {
                                        if (s[j].Substring(0, 7) == TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0'))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此日期已有補休日期！')", true);
                                            return;
                                        }
                                    }

                                    string RESTDATE = "";
                                    for (int j = 0; j < s.Count(); j++)
                                    {
                                        if (s[j].Substring(0, 7) == PKEY_MZ_IDATE1)
                                        {
                                            RESTDATE = dt.Rows[i]["MZ_RESTDATE"].ToString().Replace("，" + s[j], TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0'));
                                            break;
                                        }
                                    }

                                    if (RESTDATE != "")
                                    {
                                        string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + RESTDATE + "' WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_RESTHOUR>0 ";

                                        //Log
                                        LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "修改休假日期，更新可補休日");

                                        o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 申請補休額外處理
        /// </summary>
        protected void INSERT_15_old()
        {
            if (TextBox_MZ_CODE.Text == "15")
            {
                DataTable dt = new DataTable();

                if (Session["MZ_RESTHOUR_DT"] != null)
                {
                    dt = Session["MZ_RESTHOUR_DT"] as DataTable;

                    int ITIME1 = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 + int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text + "' AND MZ_RESTHOUR>0 ";

                        DataTable dt2 = new DataTable();

                        dt2 = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getget");


                        string MZ_RESDATE = "";

                        string s = string.Empty;
                        int hours = 0;
                        if (dt2.Rows.Count > 0)
                        {
                            s = dt2.Rows[0]["MZ_RESTDATE"].ToString();

                            hours = int.Parse(dt2.Rows[0]["MZ_RESTHOUR"].ToString());
                        }
                        int resthour_write;

                        resthour_write = hours - ITIME1;

                        if (resthour_write < 0)
                        {
                            ITIME1 = ITIME1 - hours;
                        }

                        if (!string.IsNullOrEmpty(s.Trim()))
                        {
                            if (resthour_write < 0)
                            {
                                MZ_RESDATE = s + "，" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + hours;
                            }
                            else
                            {
                                MZ_RESDATE = s + "，" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + ITIME1;
                            }
                        }
                        else
                        {
                            if (resthour_write < 0)
                            {
                                MZ_RESDATE = TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + hours;
                            }
                            else
                            {
                                MZ_RESDATE = TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "：" + ITIME1;
                            }
                        }

                        int resthour;

                        if (resthour_write < 0)
                        {
                            resthour = 0;
                        }
                        else
                        {
                            resthour = resthour_write;
                        }

                        string updateSQL = string.Format(@"UPDATE C_OVERTIME_HOUR_INSIDE SET LOCK_FLAG='Y',MZ_RESTDATE='{0}',MZ_RESTHOUR={1} 
                                                            WHERE MZ_DATE='{2}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{3}'"
                                                        , MZ_RESDATE, resthour, dt.Rows[i]["MZ_DATE"].ToString(), TextBox_MZ_ID.Text);
                        //string updateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET LOCK_FLAG='Y',MZ_RESTDATE='" + MZ_RESDATE + "',MZ_RESTHOUR=" + resthour + " WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "'AND  dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text + "' "; //  AND MZ_RESTHOUR>0

                        //Log
                        LogModel.saveLog("COHI", "U", updateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "新增修改假單，更新可補休日");

                        o_DBFactory.ABC_toTest.Edit_Data(updateSQL);
                    }

                }
            }
        }


        protected void Update_Textchange(string mz_id, string mz_idate, string mz_itime, bool flag)
        {
            #region
            //顯示類型與事實發生日
            int temp = mz_itime.Length;
            string time = "";
            if (temp == 5)
            { time = mz_itime.Substring(0, 2) + ":" + mz_itime.Substring(3, 2); }
            else
            { time = mz_itime.Substring(0, 2) + ":" + mz_itime.Substring(2, 2); }

            string date = "";
            int datemp = mz_idate.Length;
            if (datemp == 9) { date = mz_idate.Substring(0, 3) + mz_idate.Substring(4, 2) + mz_idate.Substring(7, 2); }
            else { date = mz_idate; }

            string strSQL1 = "SELECT * FROM C_DLTB01 WHERE MZ_ID='" + mz_id + "' AND MZ_IDATE1='" + date + "' AND MZ_ITIME1='" + time + "'";
            DataTable dt1 = new DataTable();
            dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL1, "GET");

            if (dt1.Rows[0]["FUNERAL_TYPE"].ToString() != "" || dt1.Rows[0]["DAYFACT"].ToString() != "")
            {

                dayfacttable.Visible = true;
                ftype.Visible = false;//標題  類型
                ftype6.Visible = false;//公假
                ftype8.Visible = false;//娩假
                ftype5.Visible = false;//喪亡對象
                qfact.Visible = false;//標題 事實發生日
                qfact1.Visible = false;//事實發生日

                if (TextBox_MZ_CODE.Text == "06")//公假
                {
                    ftype.Visible = true;//標題  類型
                    ftype6.Visible = true;//公假
                    DropDownList_funeral_type6.SelectedValue = dt1.Rows[0]["FUNERAL_TYPE"].ToString();
                    //類型欄位
                    if (flag)
                    {   //修改
                        DropDownList_funeral_type6.Enabled = true;
                    }
                    else
                    {   //顯示畫面而已
                        DropDownList_funeral_type6.Enabled = false;
                    }
                    TextBox_dayfact.Text = dt1.Rows[0]["DAYFACT"].ToString();

                    if (DropDownList_funeral_type6.SelectedValue == "3")
                    {
                        qfact.Visible = true;
                        qfact1.Visible = true;//事實發生日
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + DropDownList_funeral_type6.ClientID + "').focus();$get('" + DropDownList_funeral_type6.ClientID + "').focus();", true);
                }

                if (TextBox_MZ_CODE.Text == "04" || TextBox_MZ_CODE.Text == "09" || TextBox_MZ_CODE.Text == "29" || TextBox_MZ_CODE.Text == "10")//婚假 流產假
                {
                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日
                    TextBox_dayfact.Text = dt1.Rows[0]["DAYFACT"].ToString();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_dayfact.ClientID + "').focus();$get('" + TextBox_dayfact.ClientID + "').focus();", true);
                }

                if (TextBox_MZ_CODE.Text == "08")//娩假
                {
                    DropDownList_funeral_type8.SelectedValue = dt1.Rows[0]["FUNERAL_TYPE"].ToString();
                    if (flag)
                    {
                        DropDownList_funeral_type8.Enabled = true;

                    }
                    else
                    {
                        DropDownList_funeral_type8.Enabled = false;

                    }
                    TextBox_dayfact.Text = dt1.Rows[0]["DAYFACT"].ToString();

                    ftype.Visible = true;//標題  類型
                    ftype8.Visible = true;//娩假

                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + DropDownList_funeral_type8.ClientID + "').focus();$get('" + DropDownList_funeral_type8.ClientID + "').focus();", true);
                }

                if (TextBox_MZ_CODE.Text == "05")//喪假
                {
                    DropDownList_funeral_type5.SelectedValue = dt1.Rows[0]["FUNERAL_TYPE"].ToString();
                    if (flag)
                    {
                        DropDownList_funeral_type5.Enabled = true;

                    }
                    else
                    {
                        DropDownList_funeral_type5.Enabled = false;

                    }
                    TextBox_dayfact.Text = dt1.Rows[0]["DAYFACT"].ToString();

                    ftype.Visible = true;//標題  類型
                    ftype5.Visible = true;//喪假

                    qfact.Visible = true;
                    qfact1.Visible = true;//事實發生日

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + DropDownList_funeral_type5.ClientID + "').focus();$get('" + DropDownList_funeral_type5.ClientID + "').focus();", true);
                }
            }

            #endregion
        }


    }
}