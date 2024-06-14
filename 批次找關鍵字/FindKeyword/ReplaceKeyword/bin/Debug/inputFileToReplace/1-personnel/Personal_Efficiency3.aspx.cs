using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;


namespace TPPDDB._1_personnel
{
    public partial class Personal_Efficiency3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
            A.set_Panel_EnterToTAB(ref this.Panel1);
            }

           
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");

            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_AD.Text = string.Empty;
                TextBox_MZ_AD1.Text = string.Empty;
                TextBox_MZ_AD.Focus();
            }
            else
            {
                TextBox_MZ_AD1.Text = CName;
                RadioButtonList1.Focus();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string SelectString;
            if (RadioButtonList1.SelectedValue == "全部人員")
            {
                SelectString = "SELECT * FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'";
            }
            else
            {
                //新進人員
                SelectString = "SELECT * FROM A_DLBASE WHERE  MZ_STATUS2='Y' AND MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "' AND MZ_ID NOT IN (SELECT MZ_ID FROM A_REV_BASE WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "')";
            }

            DataTable DLBASEdt = new DataTable();

            DLBASEdt = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                string DeleteString = "DELETE FROM A_REV_BASE WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim().ToUpper()) + "' AND MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim()) + "'";

                string InsertString = "INSERT INTO A_REV_BASE(MZ_YEAR,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT,MZ_OCCC," +
                                                            " MZ_TBDV,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_REVIEW,MZ_NUM,MZ_GRADE,MZ_P4001," +
                                                            " MZ_P4010,MZ_P4100,MZ_P5001,MZ_P5010,MZ_P5100,MZ_CODE01,MZ_CODE02,MZ_CODE18," +
                                                            " MZ_CODE04,MZ_CODE05,MZ_CODE08,MZ_CODE19,MZ_CODE20,MZ_POSIND,MZ_CHISI,MZ_PCHIEF," +
                                                            " MZ_SWT,MZ_MEMO,INSDATE,INSID,INSEXAD,UP_DATE,UP_ID,UP_EXAD,MZ_IDATE)" +
                                                            " VALUES " +
                                                            "(@MZ_YEAR,@MZ_ID,@MZ_NAME,@MZ_AD,@MZ_UNIT,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC," +
                                                            " @MZ_TBDV,@MZ_SRANK,@MZ_SLVC,@MZ_SPT,@MZ_REVIEW,@MZ_NUM,@MZ_GRADE,@MZ_P4001," +
                                                            " @MZ_P4010,@MZ_P4100,@MZ_P5001,@MZ_P5010,@MZ_P5100,@MZ_CODE01,@MZ_CODE02,@MZ_CODE18," +
                                                            " @MZ_CODE04,@MZ_CODE05,@MZ_CODE08,@MZ_CODE19,@MZ_CODE20,@MZ_POSIND,@MZ_CHISI,@MZ_PCHIEF," +
                                                            " @MZ_SWT,@MZ_MEMO,@INSDATE,@INSID,@INSEXAD,@UP_DATE,@UP_ID,@UP_EXAD,@MZ_IDATE)";

                SqlTransaction connTrans = conn.BeginTransaction();

                SqlCommand Deletecmd = new SqlCommand(DeleteString, conn);
                Deletecmd.Transaction = connTrans;

                SqlCommand Insertcmd = new SqlCommand(InsertString, conn);
                Insertcmd.Transaction = connTrans;

                for (int i = 0; i < DLBASEdt.Rows.Count; i++)
                {
                    string PRK2String = "SELECT MZ_PRRST FROM A_PRK2 WHERE MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "' AND dbo.SUBSTR(MZ_DATE,1,3)='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim()) + "'";
                    DataTable PRK2dt = new DataTable();
                    PRK2dt = o_DBFactory.ABC_toTest.Create_Table(PRK2String, "GET");
                    int P4001 = 0;
                    int P4010 = 0;
                    int P4100 = 0;
                    int P5001 = 0;
                    int P5010 = 0;
                    int P5100 = 0;

                    for (int j = 0; j < PRK2dt.Rows.Count; j++)
                    {
                        string PRRST = PRK2dt.Rows[j]["MZ_PRRST"].ToString().Trim();

                        if (PRRST == "4001")
                        {
                            P4001 += 1;
                        }
                        else if (PRRST == "4002")
                        {
                            P4001 += 2;
                        }
                        else if (PRRST == "4010")
                        {
                            P4010 += 1;
                        }
                        else if (PRRST == "4020")
                        {
                            P4010 += 2;
                        }
                        else if (PRRST == "4100")
                        {
                            P4100 += 1;
                        }
                        else if (PRRST == "4200")
                        {
                            P4100 += 2;
                        }
                        else if (PRRST == "5001")
                        {
                            P5001 += 1;
                        }
                        else if (PRRST == "5002")
                        {
                            P5001 += 2;
                        }
                        else if (PRRST == "5010")
                        {
                            P5010 += 1;
                        }
                        else if (PRRST == "5020")
                        {
                            P5010 += 2;
                        }
                        else if (PRRST == "5100")
                        {
                            P5100 += 1;
                        }
                        else if (PRRST == "5200")
                        {
                            P5100 += 2;
                        }
                    }

                    int total_P4001 = P4001 + P4010 * 3 + P4100 * 9 - P5001 - P5010 * 3 - P5100 * 9;

                    string MZ_MEMO = "";

                    if (total_P4001 > 0)
                    {
                        MZ_MEMO = "嘉獎共" + total_P4001.ToString() + "次";
                    }
                    else if (total_P4001 < 0)
                    {
                        MZ_MEMO = "申誡共" + (total_P4001 * -1).ToString() + "次";
                    }

                    string CODE01HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='01' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE02HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='02' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE18HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='18' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE04HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='04' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE05HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='05' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE08HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='08' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE19HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='19' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string CODE20HOUR = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='20' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");

                    int CODE01_HOUR = int.Parse(string.IsNullOrEmpty(CODE01HOUR) ? "0" : CODE01HOUR);
                    int CODE02_HOUR = int.Parse(string.IsNullOrEmpty(CODE02HOUR) ? "0" : CODE02HOUR);
                    int CODE18_HOUR = int.Parse(string.IsNullOrEmpty(CODE18HOUR) ? "0" : CODE18HOUR);
                    int CODE04_HOUR = int.Parse(string.IsNullOrEmpty(CODE04HOUR) ? "0" : CODE04HOUR);
                    int CODE05_HOUR = int.Parse(string.IsNullOrEmpty(CODE05HOUR) ? "0" : CODE05HOUR);
                    int CODE08_HOUR = int.Parse(string.IsNullOrEmpty(CODE08HOUR) ? "0" : CODE08HOUR);
                    int CODE19_HOUR = int.Parse(string.IsNullOrEmpty(CODE19HOUR) ? "0" : CODE19HOUR);
                    int CODE20_HOUR = int.Parse(string.IsNullOrEmpty(CODE20HOUR) ? "0" : CODE20HOUR);

                    string strCODE01 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='01' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE02 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='02' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE18 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='18' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE04 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='04' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE05 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='05' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE08 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='08' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE19 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='19' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");
                    string strCODE20 = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='20' AND dbo.SUBSTR(MZ_IDATE1,0,3)='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim() + "'");


                    string CODE01 = (int.Parse(string.IsNullOrEmpty(strCODE01) ? "0" : strCODE01) + CODE01_HOUR / 8).ToString() + '.' + (CODE01_HOUR % 8).ToString();
                    string CODE02 = (int.Parse(string.IsNullOrEmpty(strCODE02) ? "0" : strCODE02) + CODE02_HOUR / 8).ToString() + '.' + (CODE02_HOUR % 8).ToString();
                    string CODE18 = (int.Parse(string.IsNullOrEmpty(strCODE18) ? "0" : strCODE18) + CODE18_HOUR / 8).ToString() + '.' + (CODE18_HOUR % 8).ToString();
                    string CODE04 = (int.Parse(string.IsNullOrEmpty(strCODE04) ? "0" : strCODE04) + CODE04_HOUR / 8).ToString() + '.' + (CODE04_HOUR % 8).ToString();
                    string CODE05 = (int.Parse(string.IsNullOrEmpty(strCODE05) ? "0" : strCODE05) + CODE05_HOUR / 8).ToString() + '.' + (CODE05_HOUR % 8).ToString();
                    string CODE08 = (int.Parse(string.IsNullOrEmpty(strCODE08) ? "0" : strCODE08) + CODE08_HOUR / 8).ToString() + '.' + (CODE08_HOUR % 8).ToString();
                    string CODE19 = (int.Parse(string.IsNullOrEmpty(strCODE19) ? "0" : strCODE19) + CODE19_HOUR / 8).ToString() + '.' + (CODE19_HOUR % 8).ToString();
                    string CODE20 = (int.Parse(string.IsNullOrEmpty(strCODE20) ? "0" : strCODE20) + CODE20_HOUR / 8).ToString() + '.' + (CODE20_HOUR % 8).ToString();

                    if (float.Parse(CODE01) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，事假" + CODE01 + "天";
                        else
                            MZ_MEMO += "事假" + CODE01 + "天";
                    }
                    if (float.Parse(CODE02) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，病假" + CODE02 + "天";
                        else
                            MZ_MEMO += "病假" + CODE02 + "天";
                    }
                    if (float.Parse(CODE18) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，曠職" + CODE18 + "天";
                        else
                            MZ_MEMO += "曠職" + CODE18 + "天";
                    }
                    if (float.Parse(CODE19) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，遲到" + CODE19 + "天";
                        else
                            MZ_MEMO += "遲到" + CODE19 + "天";
                    }
                    if (float.Parse(CODE08) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，娩假" + CODE08 + "天";
                        else
                            MZ_MEMO += "娩假" + CODE08 + "天";
                    }
                    if (float.Parse(CODE04) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，婚假" + CODE04 + "天";
                        else
                            MZ_MEMO += "婚假" + CODE04 + "天";
                    }
                    if (float.Parse(CODE20) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，早退" + CODE20 + "天";
                        else
                            MZ_MEMO += "早退" + CODE20 + "天";
                    }
                    if (float.Parse(CODE05) > 0)
                    {
                        if (MZ_MEMO.Length > 0)
                            MZ_MEMO += "，喪假" + CODE05 + "天";
                        else
                            MZ_MEMO += "喪假" + CODE05 + "天";
                    }

                    if (MZ_MEMO.Length > 0)
                        MZ_MEMO += "。";



                    Insertcmd.Parameters.Add("MZ_YEAR", SqlDbType.VarChar).Value = TextBox_MZ_YEAR.Text.Trim();
                    Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_ID"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_NAME", SqlDbType.NVarChar).Value = DLBASEdt.Rows[i]["MZ_NAME"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_AD"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_UNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_EXAD", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_EXUNIT", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_OCCC"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_TBDV"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_SRANK"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_SLVC", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_SLVC"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_SPT", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_SPT"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_REVIEW", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_UNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_NUM", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_GRADE", SqlDbType.NVarChar).Value = Convert.DBNull; // DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_P4001", SqlDbType.Float).Value = P4001.ToString();
                    Insertcmd.Parameters.Add("MZ_P4010", SqlDbType.Float).Value = P4010.ToString();
                    Insertcmd.Parameters.Add("MZ_P4100", SqlDbType.Float).Value = P4100.ToString();
                    Insertcmd.Parameters.Add("MZ_P5001", SqlDbType.Float).Value = P5001.ToString();
                    Insertcmd.Parameters.Add("MZ_P5010", SqlDbType.Float).Value = P5010.ToString();
                    Insertcmd.Parameters.Add("MZ_P5100", SqlDbType.Float).Value = P5100.ToString();
                    Insertcmd.Parameters.Add("MZ_CODE01", SqlDbType.Float).Value = CODE01;
                    Insertcmd.Parameters.Add("MZ_CODE02", SqlDbType.Float).Value = CODE02;
                    Insertcmd.Parameters.Add("MZ_CODE18", SqlDbType.Float).Value = CODE18;
                    Insertcmd.Parameters.Add("MZ_CODE04", SqlDbType.Float).Value = CODE04;
                    Insertcmd.Parameters.Add("MZ_CODE05", SqlDbType.Float).Value = CODE05;
                    Insertcmd.Parameters.Add("MZ_CODE08", SqlDbType.Float).Value = CODE08;
                    Insertcmd.Parameters.Add("MZ_CODE19", SqlDbType.Float).Value = CODE19;
                    Insertcmd.Parameters.Add("MZ_CODE20", SqlDbType.Float).Value = CODE20;
                    Insertcmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_POSIND"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_CHISI"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = DLBASEdt.Rows[i]["MZ_PCHIEF"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_SWT", SqlDbType.VarChar).Value = "0"; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_MEMO", SqlDbType.NVarChar).Value = MZ_MEMO; // DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("INSDATE", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    Insertcmd.Parameters.Add("INSID", SqlDbType.VarChar).Value = Convert.DBNull; // DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("INSEXAD", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    Insertcmd.Parameters.Add("UP_DATE", SqlDbType.VarChar).Value = Convert.DBNull; // DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("UP_ID", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    Insertcmd.Parameters.Add("UP_EXAD", SqlDbType.VarChar).Value = Convert.DBNull; // DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    Insertcmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();
                    //Insertcmd.Parameters.Add("MZ_OC1", SqlDbType.VarChar).Value = Convert.DBNull; // DLBASEdt.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                    //Insertcmd.Parameters.Add("MZ_RK1", SqlDbType.VarChar).Value = Convert.DBNull; //DLBASEdt.Rows[i]["MZ_EXAD"].ToString().Trim();

                    try
                    {
                        if (RadioButtonList1.SelectedValue == "全部人員" && i == 0)
                        {
                            Deletecmd.ExecuteNonQuery();
                        }

                        Insertcmd.ExecuteNonQuery();

                        if (i == DLBASEdt.Rows.Count - 1)
                        {
                            connTrans.Commit();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('轉入成功')", true);
                        }
                    }
                    catch
                    {
                        connTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('轉入失敗')", true);
                    }
                }
                conn.Close();
                //XX2013/06/18 
                conn.Dispose();
            }

        }
    }
}
