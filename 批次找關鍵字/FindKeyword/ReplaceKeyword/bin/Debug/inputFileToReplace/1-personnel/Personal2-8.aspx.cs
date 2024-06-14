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
using Microsoft.VisualBasic;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public partial class Personal2_8 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                A.set_Panel_EnterToTAB(ref Panel1);

                //初始化核定機關
                A.fill_AD_POST_BOSS(DropDownList_MZ_CHKAD, 2);
                //預設載入目前使用者所屬機關
                if (Session["ADPMZ_EXAD"] != null)
                {
                    DropDownList_MZ_CHKAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                }
                //取得權限
                string _strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //根據權限判斷是否要綁定核定機關
                switch (_strGID)
                {
                    case "A":
                    case "B":
                        //不鎖定
                        DropDownList_MZ_CHKAD.Enabled = true;
                        break;
                    case "C":
                        //鎖定,只能用自家單位
                        DropDownList_MZ_CHKAD.Enabled = false;
                        break;
                    case "D":
                    case "E":
                        //無權限
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                        Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                        break;
                }

                //初始化,發布人ID
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();

            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //避免「型別 xxx 的控制項 xxx 必須置於有 runat=server 的表單標記之中。」的問題
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //檢查輸入
            string ErrorMsg = CheckInput();
            if (ErrorMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('"+ ErrorMsg + "');", true);
                return;
            }


            //查詢資料
            DataTable Exceldt = Query();

            ////轉全形
            //for (int i = 0; i < Exceldt.Rows.Count; i++)
            //{
            //    string pcode = Exceldt.Rows[i]["MZ_PCODE"].ToString();
            //    //if (pcode == "1")
            //    //    Exceldt.Rows[i]["MZ_PCODE"] = "配分";
            //    //else if(pcode == "9")
            //    //    Exceldt.Rows[i]["MZ_PCODE"] = "不配分";
            //    ////


            //    Exceldt.Rows[i]["MZ_NAME"] = Strings.StrConv(Exceldt.Rows[i]["MZ_NAME"].ToString(), VbStrConv.Wide, 2052).PadRight(6, '　');//20140402 為解決罕見字 所以設定2052
            //    Exceldt.Rows[i]["MZ_PRCT"] = Strings.StrConv(Exceldt.Rows[i]["MZ_PRCT"].ToString(), VbStrConv.Wide, 0).PadRight(50, '　');
            //    Exceldt.Rows[i]["MZ_PRID"] = Strings.StrConv(Exceldt.Rows[i]["MZ_PRID"].ToString(), VbStrConv.Wide, 0).PadRight(40, '　');
            //}

            if (Exceldt.Rows.Count > 0)
            {
                string strdate = GetDateTWString();
                string UpdateString
                    = "UPDATE A_PRK2 SET MZ_PRPASS='" + TextBox_MZ_PRPASS.Text.Trim() + "',MZ_SWT='Y' ,MZ_UPDATE = '" + strdate + @"'
                     ,UPLOAD_USER_MZ_ID='" + Session["ADPMZ_ID"].ToString() + @"' 
                     WHERE MZ_DATE>='" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                                     "'  AND MZ_DATE<='" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND (MZ_SWT!='Y' OR MZ_SWT IS NULL) AND (MZ_PRPASS IS NULL or MZ_PRPASS = '') ";

                //如果有選擇 核定機關 下拉選單
                if (DropDownList_MZ_CHKAD.SelectedValue != "")
                {
                    UpdateString += @" AND MZ_CHKAD='" + DropDownList_MZ_CHKAD.SelectedValue + "' ";
                }

                if (TextBox_MZ_ID.Text != "")
                {
                    UpdateString += @" AND MUSER='" + TextBox_MZ_ID.Text + "' ";
                }
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(UpdateString);

                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), UpdateString);


                }
                catch (Exception ex)
                {
                    string strmsg = ex.Message;
                }
            }

            // 將查詢結果轉CSV
            string columnNames = @"身分證號,姓名,事由類別,事由內容,核定結果,核定日期,核定字號,核定機關,銓敘部登記日期,銓敘部登記文號";
            WriteToCSV(Exceldt, columnNames);
        }
        /// <summary>
        /// 取得目前系統時間的民國年日期字串 YYYMMDD
        /// </summary>
        /// <returns></returns>
        private static string GetDateTWString()
        {
            var tmp = new System.Globalization.TaiwanCalendar();
            string stryear = tmp.GetYear(DateTime.Now).ToString();
            string strdate = stryear + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2");
            return strdate;
        }

        private string CheckInput()
        {
            return CheckInput(this.TextBox_MZ_DATE1.Text, this.TextBox_MZ_DATE2.Text);
        }

        public static string CheckInput(string DATE1,string DATE2)
        {
            bool isOK;
            string ErrorMsg = "";
            //檢查發文日期格式 且 七碼
            isOK = ForDateTime.Check_date(DATE1);
            isOK = isOK && ForDateTime.Check_date(DATE2);
            isOK = isOK && (DATE1.Length == 7);
            isOK = isOK && (DATE2.Length == 7);
            if (isOK == false)
            {
                return  "發文日期格式錯誤,應為民國年 YYYMMDD";
            }
            int intDATE1 = Convert.ToInt32(DATE1);
            int intDATE2 = Convert.ToInt32(DATE2);
            string DateNow = GetDateTWString();
            int intDateNow = Convert.ToInt32(DateNow);
            if (intDATE1> intDATE2)
            {
                return "發文日期,起日不得晚於迄日";
            }
            if (intDATE2 > intDateNow)
            {
                return "發文日期迄不得大於系統日期";
            }


            return "";
        }

        protected void btTEST_Click(object sender, EventArgs e)
        {
            //查詢資料
            DataTable Exceldt = Query();

            // 將查詢結果轉CSV
            string columnNames = @"身分證號,姓名,事由類別,事由內容,核定結果,核定日期,核定字號,核定機關,銓敘部登記日期,銓敘部登記文號";
            WriteToCSV(Exceldt, columnNames);
        }



        /// <summary>
        /// 查詢資料
        /// </summary>
        /// <returns></returns>
        private DataTable Query()
        {
            //MZ_PRRST,MZ_IDATE,MZ_CHKAD,
            string strSQL = @"SELECT MZ_ID,
                            MZ_NAME,
                            MZ_PRK,
                            MZ_PRK1+'H'+dbo.SUBSTR(MZ_PROLNO,1,4)+'@'+REPLACE(MZ_PRCT, ',' ,'') as content,
                            MZ_PRRST,
                            MZ_DATE,
                            MZ_PRID+'字第'+MZ_PRID1+'號' as check_num,
                            MZ_AD,
                            ''as sign_date,
                            ''as sign_num" +
                            " FROM A_PRK2 WHERE MZ_DATE>='" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                            "' AND MZ_DATE<='" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0') + @"' AND (MZ_SWT!='Y' OR MZ_SWT IS NULL) AND (MZ_PRPASS IS NULL or MZ_PRPASS = '') 
                            
";

            //如果有選擇 核定機關 下拉選單
            if (DropDownList_MZ_CHKAD.SelectedValue != "")
            {
                strSQL += @" AND MZ_CHKAD='" + DropDownList_MZ_CHKAD.SelectedValue + "' ";
            }

            if (TextBox_MZ_ID.Text != "")
            {
                strSQL += @" AND MUSER='" + TextBox_MZ_ID.Text + "' ";
            }
            //排序條件
            strSQL += @" ORDER BY MZ_ID";
            DataTable Exceldt = new DataTable();


            Exceldt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            return Exceldt;
        }


        /// <summary>
        /// 將查詢結果轉CSV
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="columnNames">表頭,用逗點區隔之</param>
        public static void WriteToCSV(DataTable DT, string columnNames)
        {


            //System.IO.StreamWriter sw = new System.IO.StreamWriter(HttpContext.Current.Response.OutputStream, Encoding.Default);
            System.IO.StringWriter sw = new System.IO.StringWriter();

            //WriteColumnName(sw);
            //string columnNames = @"身分證字號, 課程名稱, 開課起始日期, 開課起始時間, 開課結束日期, 開課結束時間, 姓名, 學位學分, 課程類別代碼, 上課縣市, 期別, 訓練總數, 訓練總數單位, 訓練成績, 證件字號, 出勤上課狀況, 生日, 學習性質, 數位時數, 實體時數, 課程代碼, 實際上課起始日期, 實際上課起始時間, 實際上課結束日期, 實際上課結束時間";
            sw.Write(columnNames);
            sw.WriteLine();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
               
                for (int j = 0; j < DT.Columns.Count; j++)
                {
                    //AddComma(DT.Rows[i][j].ToString(), stringBuilder);
                    string value = DT.Rows[i][j].ToString();
                    //先將內文的逗點過濾掉,避免誤判
                    stringBuilder.Append(value.Replace(',', ' '));
                    //如果不是最後一欄,則補上逗點分隔
                    if(j != (DT.Columns.Count - 1))
                    {
                        stringBuilder.Append(",");
                    }
                }
                sw.Write(stringBuilder.ToString());
                sw.WriteLine();
            }

            //sw.Close();
            //sw.Dispose();

            string filename = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".csv";
            String attachment = "attachment; filename=" + filename;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 將查詢結果轉換成Excel
        /// </summary>
        /// <param name="Exceldt"></param>
        private void ResponseToExcel(DataTable Exceldt)
        {
            ExcelTable.DataSource = Exceldt;



            ExcelTable.AllowPaging = false;
            ExcelTable.AllowSorting = false;
            ExcelTable.EnableViewState = false;
            ExcelTable.DataBind();


            Response.ClearContent();
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            Response.Write("<style>");
            Response.Write("td{mso-number-format:\"\\@\";}"); //將所有欄位格式改為"文字"
            Response.Write("</style>");
            string filename = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Page.Server.UrlEncode(filename));
            Response.ContentType = "application/excel";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            ExcelTable.RenderControl(objHtmlTextWriter);
            Response.Write(objStringWriter.ToString());
            Response.End();
        }




    }
}
