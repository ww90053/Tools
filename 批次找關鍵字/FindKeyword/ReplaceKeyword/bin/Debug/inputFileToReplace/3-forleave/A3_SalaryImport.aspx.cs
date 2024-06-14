using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._3_forleave
{
    public partial class A3_SalaryImport : o_score_Function
    {
        string _strGID
        {
            get { return ViewState["O_strGID"] != null ? ViewState["O_strGID"].ToString() : string.Empty; }
            set { ViewState["O_strGID"] = value; }
        }
        List<string> allowExtension = new List<string>() { ".xls" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 一般權限檢查
                chk_TPMPermissions();

                // 檢查群組權限
                //_strGID = chk_TPMGroup();
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            }
            //O.fill_AD(DropDownList_AD);
            if (!Page.IsPostBack)
            {
                pageInit();
                //group_control(_strGID);
                group_control();
            }
        }
        private void group_control()
        {
            //ViewState["C_strGID"].ToString()
            //switch (strGID)
            switch (ViewState["C_strGID"].ToString())
            {
                case "TPMIDISADMIN":
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                    break;
                default:
                    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                    break;

            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            string error = "";
            if (ddlAd.SelectedValue == "請選擇")
            {
                error += "請選擇機關\n";
            }
            if (string.IsNullOrEmpty(txt_Year.Text) || txt_Year.Text.Count() != 5)
            {
                error += "年度月份不能為空白或打錯\n";
            }
            if (!FileUpload_Excel.HasFile)
            {
                error += "請上傳檔案\n";
            }
            else if (!allowExtension.Exists(tmpStr => tmpStr.EndsWith(Path.GetExtension(FileUpload_Excel.FileName))))
            {
                error += "請上傳Excel檔(.xls)\n";
            }
            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                return;
            }
            else
            {
                HSSFWorkbook workbook = new HSSFWorkbook(FileUpload_Excel.FileContent);
                HSSFSheet sheet = workbook.GetSheetAt(0);
                HSSFRow headerRow = sheet.GetRow(1);
                //List<string> fileHeader = new List<string>();
                //for (int i = 0; i < headerRow.LastCellNum; i++)
                //{
                //    if (headerRow.GetCell(i) != null)
                //        fileHeader.Add(headerRow.GetCell(i).StringCellValue);
                //}              
                error = "";
                //先把資料清掉在新增
                RecordObject ro2 = new RecordObject();
                ro2.MZ_AD = ddlAd.SelectedValue.ToString();
                ro2.AMONTH = txt_Year.Text;
                DeleteRecord(ro2);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = sheet.GetRow(i);
                    RecordObject ro = new RecordObject();
                    //row.GetCell(1);
                    ro.EXUNIT_NAME = row.GetCell(0).ToString();
                    ro.MZ_ID = row.GetCell(1).ToString();
                    ro.MZ_NAME = row.GetCell(2).ToString();
                    ro.MZ_SPT = "0" + row.GetCell(3).ToString();
                    if (row.GetCell(5).ToString() != "")
                    {
                        ro.SALARYPAY1 = row.GetCell(5).ToString();
                    }
                    if (row.GetCell(7).ToString() != "")
                    {
                        ro.PROFESS = row.GetCell(7).ToString();
                    }
                    //如果職務加給表別類別不同要做不同處理 boss 要存 WORKP欄位的金額
                    if (row.GetCell(8).ToString() != "")
                    {
                        if (row.GetCell(8).ToString() == "C1001" || row.GetCell(8).ToString() == "C1003" || row.GetCell(8).ToString() == "C1013")
                        {
                            if (row.GetCell(9).ToString() != "")
                            {
                                ro.BOSS = row.GetCell(9).ToString();
                            }
                        }
                        else
                        {
                            if (row.GetCell(9).ToString() != "")
                            {
                                ro.WORKP = row.GetCell(9).ToString();
                            }
                        }
                        //TECHNICS 要存WORKP
                        if (row.GetCell(8).ToString() == "C2002")
                        {
                            if (row.GetCell(9).ToString() != "")
                            {
                                ro.TECHNICS = row.GetCell(9).ToString();
                                ro.WORKP = "0";
                            }
                        }
  
                    }
                    if (row.GetCell(11).ToString() != "")
                    {
                        ro.FAR = row.GetCell(11).ToString();
                    }
                    if (row.GetCell(14).ToString() == "F0057-0")
                    {
                        if (row.GetCell(15).ToString() != "")
                        {
                            ro.ADVENTIVE = row.GetCell(15).ToString();
                        }
                    }
                    else
                    {
                        if (row.GetCell(15).ToString() != "")
                        {
                            ro.OTHERADD = row.GetCell(15).ToString();
                        }
                    }
                    if (row.GetCell(17).ToString() != "")
                    {
                        ro.BONUS = row.GetCell(17).ToString();
                    }
                   
                    ro.MZ_AD = ddlAd.SelectedValue.ToString();
                    ro.AMONTH = txt_Year.Text;
                    SaveRecord(ro);
                }
                if (!string.IsNullOrEmpty(error))
                {
                    ErrorMsg.Text = error;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('匯入完成請修正下列錯誤');", true);
                }
                else
                {
                    ErrorMsg.Text = "";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('匯入完成');", true);
                }
            }
        }

        class RecordObject
        {
            public string MZ_ID = "";
            public string MZ_NAME = "";
            public string MZ_SPT = "";
            public string SALARYPAY1 = "0";
            public string PROFESS = "0";
            public string WORKP = "0";
            public string FAR = "0";
            public string OTHERADD = "0";
            public string BONUS = "0";
            public string EXUNIT_NAME = "";
            public string BOSS = "0";
            public string AMONTH = "";
            public string MZ_AD = "";
            public string TECHNICS = "0";
            public string ADVENTIVE = "0";
           
        }
        void SaveRecord(RecordObject ro)
        {
            SqlParameter[] parameterList = { 
                            new SqlParameter("MZ_ID", SqlDbType.NVarChar){Value = ro.MZ_ID},
                            new SqlParameter("MZ_NAME", SqlDbType.NVarChar){Value = ro.MZ_NAME},
                            new SqlParameter("MZ_SPT", SqlDbType.NVarChar){Value = ro.MZ_SPT},                         
                            new SqlParameter("SALARYPAY1", SqlDbType.NVarChar){Value = ro.SALARYPAY1},
                            new SqlParameter("PROFESS", SqlDbType.NVarChar){Value = ro.PROFESS},
                            new SqlParameter("WORKP", SqlDbType.NVarChar){Value = ro.WORKP},
                            new SqlParameter("FAR", SqlDbType.NVarChar){Value = ro.FAR},
                            new SqlParameter("OTHERADD", SqlDbType.NVarChar){Value = ro.OTHERADD},
                            new SqlParameter("BONUS", SqlDbType.NVarChar){Value = ro.BONUS},                         
                            new SqlParameter("EXUNIT_NAME", SqlDbType.NVarChar){Value = ro.EXUNIT_NAME},
                            new SqlParameter("BOSS", SqlDbType.NVarChar){Value = ro.BOSS},
                            new SqlParameter("AMONTH", SqlDbType.NVarChar){Value = ro.AMONTH},
                            new SqlParameter("MZ_AD", SqlDbType.NVarChar){Value = ro.MZ_AD},
                            new SqlParameter("TECHNICS", SqlDbType.NVarChar){Value = ro.TECHNICS},
                            new SqlParameter("ADVENTIVE", SqlDbType.NVarChar){Value = ro.ADVENTIVE},                           
                            };
            //用身分證字號加姓名跟年月份加單位來檢查是否重複
            SqlParameter[] parameterList3= { 
                            new SqlParameter("MZ_ID", SqlDbType.NVarChar){Value = ro.MZ_ID},
                            new SqlParameter("MZ_NAME", SqlDbType.NVarChar){Value = ro.MZ_NAME},
                            new SqlParameter("MZ_SPT", SqlDbType.NVarChar){Value = ro.MZ_SPT},                                                              
                            new SqlParameter("AMONTH", SqlDbType.NVarChar){Value = ro.AMONTH},
                            new SqlParameter("MZ_AD", SqlDbType.NVarChar){Value = ro.MZ_AD},
                            };
            //string sn = o_DBFactory.ABC_toTest.vExecSQL("SELECT SN FROM B_MONTHPAY_A3 WHERE MZ_ID = '" + ro.MZ_ID + "'" + "and AMONTH = " + "'" + ro.AMONTH + "'" + "and MZ_AD = " + "'" + ro.MZ_AD + "'");
            //List<string> sn = o_DBFactory.ABC_toTest.vExecSQL_A3("SELECT SN,WORKP,BOSS,TECHNICS FROM B_MONTHPAY_A3 WHERE MZ_ID = '" + ro.MZ_ID + "'" + "and AMONTH = " + "'" + ro.AMONTH + "'" + "and MZ_AD = " + "'" + ro.MZ_AD + "'");
            //List<string> sn = o_DBFactory.ABC_toTest.vExecSQL_A3("SELECT SN,WORKP,BOSS,TECHNICS FROM B_MONTHPAY_A3 WHERE MZ_ID = '" + ro.MZ_ID + "'" + "and AMONTH = " + "'" + ro.AMONTH + "'" + "and MZ_AD = " + "'" + ro.MZ_AD + "'" + "and MZ_SPT = " + "'" + ro.MZ_SPT + "'");
            //List<string> sn = o_DBFactory.ABC_toTest.vExecSQL_A3("SELECT SN,WORKP,BOSS,TECHNICS FROM B_MONTHPAY_A3 WHERE MZ_ID = '" + ro.MZ_ID + "'" + "and AMONTH = " + "'" + ro.AMONTH + "'" + "and MZ_AD = " + "'" + ro.MZ_AD + "'" + "and MZ_SPT = " + "'" + ro.MZ_SPT + "'" + "and EXUNIT_NAME = " + "'" + ro.EXUNIT_NAME + "'");
            String SQL = String.Empty;
            String WHERE = String.Empty;
            //SQL = "SELECT SN,WORKP,BOSS,TECHNICS FROM B_MONTHPAY_A3 WHERE MZ_ID = '" + ro.MZ_ID + "'" + "and AMONTH = " + "'" + ro.AMONTH + "'" + "and MZ_AD = " + "'" + ro.MZ_AD + "'" + "and MZ_SPT = " + "'" + ro.MZ_SPT + "'" + "and MZ_NAME = " + "'" + ro.MZ_NAME + "'";
            SQL = "SELECT SN,WORKP,BOSS,TECHNICS FROM B_MONTHPAY_A3 WHERE MZ_ID = @MZ_ID and AMONTH = @AMONTH and MZ_AD = @MZ_AD and MZ_SPT = @MZ_SPT and MZ_NAME = @MZ_NAME";
            List<string> sn = o_DBFactory.ABC_toTest.vExecSQL_A3(SQL, parameterList3);
            if (string.IsNullOrEmpty(sn[0]))
            {
                string strSQL = @"INSERT INTO B_MONTHPAY_A3
                            (MZ_ID,MZ_NAME,MZ_SPT,SALARYPAY1,PROFESS,WORKP,FAR,OTHERADD,BONUS,EXUNIT_NAME,BOSS,AMONTH,MZ_AD,TECHNICS,SN,ADVENTIVE)
                           VALUES (@MZ_ID,@MZ_NAME,@MZ_SPT,@SALARYPAY1,@PROFESS,@WORKP,@FAR,@OTHERADD,@BONUS,@EXUNIT_NAME,@BOSS,@AMONTH,@MZ_AD,@TECHNICS, NEXT VALUE FOR dbo.B_MONTHPAY_A3_SN,@ADVENTIVE) ";
                //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.ZHT32EUC");
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
            }
            else
            {              
                //先看是否是0如果沒值的再就全部UPDATE 如果是0就只更新WORKP
                string strSQL = "";
                if (sn[1] == "0" && sn[2] != "0")
                {
                    SqlParameter[] parameterList2 = { 
                            new SqlParameter("MZ_ID", SqlDbType.NVarChar){Value = ro.MZ_ID},
                            new SqlParameter("MZ_NAME", SqlDbType.NVarChar){Value = ro.MZ_NAME},                          
                            new SqlParameter("WORKP", SqlDbType.NVarChar){Value = ro.WORKP},
                            new SqlParameter("AMONTH", SqlDbType.NVarChar){Value = ro.AMONTH},
                            new SqlParameter("MZ_AD", SqlDbType.NVarChar){Value = ro.MZ_AD},
                            new SqlParameter("TECHNICS", SqlDbType.NVarChar){Value = ro.TECHNICS},
                            };
                    strSQL = @"UPDATE B_MONTHPAY_A3 SET MZ_ID = @MZ_ID, MZ_NAME = @MZ_NAME,
                                    WORKP=@WORKP,AMONTH=@AMONTH,MZ_AD=@MZ_AD,TECHNICS=@TECHNICS
                                    WHERE SN=@SN";
                    List<SqlParameter> tmp2 = parameterList2.ToList();
                    tmp2.Add(new SqlParameter("SN", SqlDbType.NVarChar) { Value = sn[0] });
                    parameterList2 = tmp2.ToArray();
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList2);
                }
                else
                {
                    List<SqlParameter> tmp = parameterList.ToList();
                    tmp.Add(new SqlParameter("SN", SqlDbType.NVarChar) { Value = sn[0] });
                    parameterList = tmp.ToArray();
                     strSQL = @"UPDATE B_MONTHPAY_A3 SET MZ_ID = @MZ_ID, MZ_NAME = @MZ_NAME, MZ_SPT=@MZ_SPT,
                                        SALARYPAY1=@SALARYPAY1,PROFESS=@PROFESS,WORKP=@WORKP,FAR=@FAR,
                                        OTHERADD=@OTHERADD, BONUS=@BONUS,
                                        EXUNIT_NAME=@EXUNIT_NAME,BOSS=@BOSS,AMONTH=@AMONTH,MZ_AD=@MZ_AD,TECHNICS=@TECHNICS,ADVENTIVE=@ADVENTIVE
                                    WHERE SN=@SN";
                     o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                }
            }
        }

        void DeleteRecord(RecordObject ro2)
        {
            SqlParameter[] parameterList = { 
                            new SqlParameter("AMONTH", SqlDbType.NVarChar){Value = ro2.AMONTH},
                            new SqlParameter("MZ_AD", SqlDbType.NVarChar){Value = ro2.MZ_AD},                      
                            };

            string strSQL = @"DELETE
                                FROM B_MONTHPAY_A3
                                WHERE AMONTH     = @AMONTH
                                AND MZ_AD     = @MZ_AD";
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
         
        }

        private void pageInit()
        {
            string strSQL = "";
            DataTable temp = new DataTable();
            //機關
            strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' and MZ_KCODE not in ('382133400C','382133500C')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                ddlAd.DataSource = temp;
                ddlAd.DataTextField = "MZ_KCHI";
                ddlAd.DataValueField = "MZ_KCODE";
                ddlAd.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                ddlAd.DataBind();
                ddlAd.Items.Insert(0, new ListItem("請選擇", "請選擇"));             
            }
        }


    }
}