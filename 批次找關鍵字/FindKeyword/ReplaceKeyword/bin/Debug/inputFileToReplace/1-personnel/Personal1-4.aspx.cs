using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.DirectoryServices;
using TPPDDB.Helpers;
using System.Text;

namespace TPPDDB._1_personnel
{
    public partial class Personal1_4 : System.Web.UI.Page
    {
        List<String> TDLBASE = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

            ViewState["MZ_AD"] = Request["AD"];
            ViewState["MZ_UNIT"] = Request["UNIT"];

            if (ViewState["MZ_AD"] == null)
                ViewState["MZ_AD"] = "";

            if (ViewState["MZ_UNIT"] == null)
                ViewState["MZ_UNIT"] = "";

            HttpCookie DLBASE_ID_Cookie = new HttpCookie("Personal1_4Search_ID");
            DLBASE_ID_Cookie = Request.Cookies["Personal1_4Search_ID"];

            if (DLBASE_ID_Cookie == null)
            {
                ViewState["MZ_ID"] = null;
                Response.Cookies["Personal1_4Search_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_ID"] = TPMPermissions._strDecod(DLBASE_ID_Cookie.Value.ToString());
                Response.Cookies["Personal1_4Search_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            if (!IsPostBack)
            {
                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL2 = "SELECT * FROM A_TDLBASE  WHERE 1=1";
                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL2 = strSQL2 + " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }

                    if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL2 += " AND MZ_AD='" + ViewState["MZ_AD"].ToString() + "'";
                    }

                    if (ViewState["MZ_UNIT"].ToString() != "")
                    {
                        strSQL2 += " AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "'";
                    }

                    strSQL2 = strSQL2 + " ORDER BY MZ_ID";

                    TDLBASE = o_DBFactory.ABC_toTest.DataListArray(strSQL2, "MZ_ID");

                    Session["TDLBASE_LT"] = TDLBASE;

                    if (TDLBASE.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal1-4.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (TDLBASE.Count == 1)
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

                    if (TDLBASE.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TDLBASE.Count.ToString() + "筆";
                    }
                }
            }
        }

        protected void finddata(int dataCount)
        {
            TDLBASE = Session["TDLBASE_LT"] as List<string>;

            string strSQL = "SELECT * FROM VW_A_DLBASE WHERE MZ_ID='" + TDLBASE[dataCount] + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (temp.Rows.Count == 1)
            {
                TextBox_MZ_NAME.Text = temp.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();
                TextBox_MZ_AD.Text = temp.Rows[0]["MZ_AD"].ToString();
                TextBox_MZ_UNIT.Text = temp.Rows[0]["MZ_UNIT"].ToString();
                TextBox_MZ_EXAD.Text = temp.Rows[0]["MZ_EXAD"].ToString();
                TextBox_MZ_EXUNIT.Text = temp.Rows[0]["MZ_EXUNIT"].ToString();
                TextBox_MZ_OCCC.Text = temp.Rows[0]["MZ_OCCC"].ToString();
                TextBox_MZ_RANK.Text = temp.Rows[0]["MZ_RANK"].ToString();
                TextBox_MZ_RANK1.Text = temp.Rows[0]["MZ_RANK1"].ToString();
                TextBox_MZ_CHISI.Text = temp.Rows[0]["MZ_CHISI"].ToString();
                TextBox_MZ_POSIND.Text = temp.Rows[0]["MZ_POSIND"].ToString();
                TextBox_MZ_TBDV.Text = temp.Rows[0]["MZ_TBDV"].ToString();
                TextBox_MZ_SRANK.Text = temp.Rows[0]["MZ_SRANK"].ToString();
                TextBox_MZ_SLVC.Text = temp.Rows[0]["MZ_SLVC"].ToString();
                TextBox_MZ_SPT.Text = temp.Rows[0]["MZ_SPT"].ToString();
                TextBox_MZ_SPT1.Text = temp.Rows[0]["MZ_SPT1"].ToString();

                //DropDownList_MZ_SEX.Text = temp.Rows[0]["MZ_SEX"].ToString();
                //20160726
                DropDownList_MZ_SEX.SelectedValue = temp.Rows[0]["MZ_SEX"].ToString();

                TextBox_MZ_BIR.Text = temp.Rows[0]["MZ_BIR"].ToString();
                TextBox_MZ_TBCD3.Text = temp.Rows[0]["MZ_TBCD3"].ToString();
                TextBox_MZ_CITY.Text = temp.Rows[0]["MZ_CITY"].ToString();
                TextBox_MZ_BL.Text = temp.Rows[0]["MZ_BL"].ToString();
                DropDownList_MZ_SM.SelectedValue = temp.Rows[0]["MZ_SM"].ToString();
                TextBox_MZ_SLFDATE.Text = temp.Rows[0]["MZ_SLFDATE"].ToString();
                TextBox_MZ_SLEDATE.Text = temp.Rows[0]["MZ_SLEDATE"].ToString();
                TextBox_MZ_OFFYY.Text = temp.Rows[0]["MZ_OFFYY"].ToString();
                TextBox_MZ_TBCD9.Text = temp.Rows[0]["MZ_TBCD9"].ToString();
                TextBox_MZ_ZONE1.Text = temp.Rows[0]["MZ_ZONE1"].ToString();
                TextBox_MZ_ADD1.Text = temp.Rows[0]["MZ_ADD1"].ToString();
                TextBox_MZ_ZONE2.Text = temp.Rows[0]["MZ_ZONE2"].ToString();
                TextBox_MZ_ADD2.Text = temp.Rows[0]["MZ_ADD2"].ToString();
                TextBox_MZ_PHONE.Text = temp.Rows[0]["MZ_PHONE"].ToString();
                TextBox_MZ_MOVETEL.Text = temp.Rows[0]["MZ_MOVETEL"].ToString();
                TextBox_MZ_PCHIEF.Text = temp.Rows[0]["MZ_PCHIEF"].ToString();
                TextBox_MZ_NREA.Text = temp.Rows[0]["MZ_NREA"].ToString();
                TextBox_MZ_DATE.Text = temp.Rows[0]["MZ_DATE"].ToString();
                TextBox_MZ_IDATE.Text = temp.Rows[0]["MZ_IDATE"].ToString();
                TextBox_MZ_ADATE.Text = temp.Rows[0]["MZ_ADATE"].ToString();
                TextBox_MZ_NID.Text = temp.Rows[0]["MZ_NID"].ToString();
                TextBox_MZ_NRT.Text = temp.Rows[0]["MZ_NRT"].ToString();
                TextBox_MZ_PESN.Text = temp.Rows[0]["MZ_PESN"].ToString();
                TextBox_MZ_EMNAM.Text = temp.Rows[0]["MZ_EMNAM"].ToString();
                TextBox_MZ_PHONO.Text = temp.Rows[0]["MZ_PHONO"].ToString();
                TextBox_MZ_PHONH.Text = temp.Rows[0]["MZ_PHONH"].ToString();
                TextBox_MZ_FDATE.Text = temp.Rows[0]["MZ_FDATE"].ToString();


                RadioButtonList_MZ_ABORIGINE.SelectedValue = temp.Rows[0]["MZ_ABORIGINE"].ToString();
                TextBox_MZ_ARMYSTATE.Text = temp.Rows[0]["MZ_ARMYSTATE"].ToString();
                TextBox_MZ_ARMYRANK.Text = temp.Rows[0]["MZ_ARMYRANK"].ToString();
                TextBox_MZ_ARMYKIND.Text = temp.Rows[0]["MZ_ARMYKIND"].ToString();
                TextBox_MZ_ARMYCOURSE.Text = temp.Rows[0]["MZ_ARMYCOURSE"].ToString();
                TextBox_MZ_ABORIGINENAME.Text = temp.Rows[0]["MZ_ABORIGINENAME"].ToString();
                TextBox_MZ_ENAME.Text = temp.Rows[0]["MZ_ENAME"].ToString();
                TextBox_MZ_EXTPOS.Text = temp.Rows[0]["MZ_EXTPOS"].ToString();
                TextBox_MZ_LDATE.Text = temp.Rows[0]["MZ_LDATE"].ToString();

                //20140407
                TextBox_MZ_AD1.Text = temp.Rows[0]["MZ_AD_CH"].ToString();
                TextBox_MZ_EXAD1.Text = temp.Rows[0]["MZ_EXAD_CH"].ToString();
                TextBox_MZ_CHISI1.Text = temp.Rows[0]["MZ_CHISI_CH"].ToString();
                TextBox_MZ_EXUNIT1.Text = temp.Rows[0]["MZ_EXUNIT_CH"].ToString();
                TextBox_MZ_NREA1.Text = temp.Rows[0]["MZ_NREA_CH"].ToString();
                TextBox_MZ_OCCC1.Text = temp.Rows[0]["MZ_OCCC_CH"].ToString();
                TextBox_MZ_PCHIEF1.Text = temp.Rows[0]["MZ_PCHIEF_CH"].ToString();
                TextBox_MZ_RANK_1.Text = temp.Rows[0]["MZ_RANK_CH"].ToString();
                TextBox_MZ_RANK1_1.Text = temp.Rows[0]["MZ_RANK1_CH"].ToString();
                TextBox_MZ_SLVC1.Text = temp.Rows[0]["MZ_SLVC_CH"].ToString();
                TextBox_MZ_SRANK1.Text = temp.Rows[0]["MZ_SRANK_CH"].ToString();
                TextBox_MZ_TBCD31.Text = temp.Rows[0]["MZ_TBCD3_CH"].ToString();
                TextBox_MZ_TBCD91.Text = temp.Rows[0]["MZ_TBCD9_CH"].ToString();
                TextBox_MZ_TBDV1.Text = temp.Rows[0]["MZ_TBDV_CH"].ToString();
                TextBox_MZ_UNIT1.Text = temp.Rows[0]["MZ_UNIT_CH"].ToString();
                TextBox_MZ_PESN1.Text = temp.Rows[0]["MZ_PESN_CH"].ToString();
                TextBox_MZ_NRT1.Text = temp.Rows[0]["MZ_NRT_CH"].ToString();

                TextBox_MZ_ARMYCOURSE1.Text = temp.Rows[0]["MZ_ARMYCOURSE_CH"].ToString();
                TextBox_MZ_ARMYKIND1.Text = temp.Rows[0]["MZ_ARMYKIND_CH"].ToString();
                TextBox_MZ_ARMYSTATE1.Text = temp.Rows[0]["MZ_ARMYSTATE_CH"].ToString();
                TextBox_MZ_ARMYRANK1.Text = temp.Rows[0]["MZ_ARMYRANK_CH"].ToString();

              
            }
        }
        /// <summary>
        /// 按鈕: 執行匯入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            string selectString = "SELECT * FROM A_TDLBASE";
            //判斷處理操作類型
            string mode = "";
            //紀錄處理人員ID
            string Last_mz_id = string.Empty;
            StringBuilder expErrorMsg = new StringBuilder();

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");

            Label1.Text = "資料量多，匯入中，請稍待…";
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                SqlTransaction oraTran = conn.BeginTransaction();
                SqlCommand Insertcmd;

                try
                {
                    //匯入前會把正式TABLE所有人改成非在職
                    //有INSERT 和UPDATE才會改成在職
                    using (SqlCommand cmd = new SqlCommand("UPDATE A_DLBASE SET MZ_STATUS2='N' ", conn))
                    {
                        cmd.Transaction = oraTran;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }

                    for (int i = 0; i < tempDT.Rows.Count; i++)
                    {
                        #region 查詢正式TABLE有無此人
                        string MZ_ID_FIND = string.Empty;
                        using (SqlCommand cmd = new SqlCommand("SELECT MZ_ID,MZ_ADD1,MZ_ADD2 FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'", conn))
                        {
                            cmd.Transaction = oraTran;
                            cmd.CommandType = CommandType.Text;

                            MZ_ID_FIND = cmd.ExecuteScalar().ToStringNullSafe();
                        }
                        string tmpMZ_ADD1 = string.Empty;
                        using (SqlCommand cmd = new SqlCommand("SELECT MZ_ADD1 FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'", conn))
                        {
                            cmd.Transaction = oraTran;
                            cmd.CommandType = CommandType.Text;

                            tmpMZ_ADD1 = cmd.ExecuteScalar().ToStringNullSafe();
                        }
                        string tmpMZ_ADD2 = string.Empty;
                        using (SqlCommand cmd = new SqlCommand("SELECT MZ_ADD2 FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'", conn))
                        {
                            cmd.Transaction = oraTran;
                            cmd.CommandType = CommandType.Text;

                            tmpMZ_ADD2 = cmd.ExecuteScalar().ToStringNullSafe();
                        }
                        #endregion

                        //紀錄目前處理人員ID
                        Last_mz_id = tempDT.Rows[i]["MZ_ID"].ToString();


                        if (string.IsNullOrEmpty(MZ_ID_FIND))
                        {

                            string insertString = "INSERT INTO A_DLBASE(MZ_NAME,MZ_ID,MZ_BIR,MZ_TBCD3,MZ_CITY,MZ_BL,MZ_SM,MZ_SEX,MZ_ADD1,MZ_ADD2, " +
                                                                              " MZ_PHONE,MZ_OFFYY,MZ_FYEAR,MZ_TBCD9,MZ_AD,MZ_UNIT,MZ_EXAD," +
                                                                              " MZ_EXUNIT,MZ_RANK,MZ_RANK1,MZ_OCCC,MZ_CHISI,MZ_POSIND,MZ_NRT,MZ_TBDV," +
                                                                              " MZ_PCHIEF,MZ_NREA,MZ_DATE,MZ_NID,MZ_IDATE,MZ_ADATE,MZ_PHONH,MZ_SPT,MZ_SRANK," +
                                                                              " MZ_OPFDATE,MZ_SLFDATE,MZ_SLEDATE,MZ_NIN,MZ_SLVC,MZ_PNO,MZ_SPT1,MZ_OPEDATE," +
                                                                              " MZ_PESN,MZ_PHONO,MZ_EMNAM,MZ_RET,MZ_POWER,MZ_ABORIGINE,PAY_AD,MZ_STATUS2,MZ_ISPOLICE,MZ_SALARY_ISDATE,MZ_INSURANCEMODE)" +
                                                                      " values (@MZ_NAME,@MZ_ID,@MZ_BIR,@MZ_TBCD3,@MZ_CITY,@MZ_BL,@MZ_SM,@MZ_SEX,@MZ_ADD1,@MZ_ADD2, " +
                                                                              " @MZ_PHONE,@MZ_OFFYY,@MZ_FYEAR,@MZ_TBCD9,@MZ_AD,@MZ_UNIT,@MZ_EXAD," +
                                                                              " @MZ_EXUNIT,@MZ_RANK,@MZ_RANK1,@MZ_OCCC,@MZ_CHISI,@MZ_POSIND,@MZ_NRT,@MZ_TBDV," +
                                                                              " @MZ_PCHIEF,@MZ_NREA,@MZ_DATE,@MZ_NID,@MZ_IDATE,@MZ_ADATE,@MZ_PHONH,@MZ_SPT,@MZ_SRANK," +
                                                                              " @MZ_OPFDATE,@MZ_SLFDATE,@MZ_SLEDATE,@MZ_NIN,@MZ_SLVC,@MZ_PNO,@MZ_SPT1,@MZ_OPEDATE," +
                                                                              " @MZ_PESN,@MZ_PHONO,@MZ_EMNAM,@MZ_RET,@MZ_POWER,@MZ_ABORIGINE,@PAY_AD,@MZ_STATUS2,@MZ_ISPOLICE,@MZ_SALARY_ISDATE,@MZ_INSURANCEMODE)";

                            Insertcmd = new SqlCommand(insertString, conn);
                            Insertcmd.Transaction = oraTran;

                            #region 各欄位處理
                            Insertcmd.CommandType = CommandType.Text;
                            Insertcmd.Parameters.Add("MZ_NAME", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_NAME"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ID"].ToString().Trim().ToUpper();
                            Insertcmd.Parameters.Add("MZ_BIR", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_BIR"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_TBCD3", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_TBCD3"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_CITY", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_CITY"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_BL", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_BL"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_SM", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SM"].ToString().Trim();
                            
                            ////20140214
                            ////Insertcmd.Parameters.Add("MZ_SEX", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SEX"].ToString().Trim();
                            //Insertcmd.Parameters.Add("MZ_SEX", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ID"].ToString().Trim().Substring(1,1);
                            //20160726 jack 
                            Insertcmd.Parameters.Add("MZ_SEX", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SEX"].ToString().Trim();

                            Insertcmd.Parameters.Add("MZ_ADD1", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ADD1"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_ADD2", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ADD2"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_PHONE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_PHONE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_OFFYY", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_OFFYY"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_FYEAR", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_FYEAR"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_TBCD9", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_TBCD9"].ToString().Trim();

                            Insertcmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_AD"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_UNIT"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_EXAD", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_EXAD"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_EXUNIT", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_RANK"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_RANK1"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_OCCC"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_CHISI"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_POSIND"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_NRT", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_NRT"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_TBDV"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_PCHIEF"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_NREA", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_NREA"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_DATE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_NID", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_NID"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_IDATE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ADATE"].ToString().Trim();

                            // sam wellsince 20201008 薪資生效日 MZ_SALARY_ISDATE 若為空 預設塞 到職日期 
                            Insertcmd.Parameters.Add("MZ_SALARY_ISDATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ADATE"].ToString().Trim();
                            
                            Insertcmd.Parameters.Add("MZ_PHONH", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_PHONH"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_SPT", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SPT"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SRANK"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_OPFDATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_OPFDATE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_SLFDATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SLFDATE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_SLEDATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SLEDATE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_NIN", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_NIN"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_SLVC", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SLVC"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_PNO", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(tempDT.Rows[i]["MZ_PNO"].ToString()) ? tempDT.Rows[i]["MZ_PNO"].ToString().Trim() : "";
                            Insertcmd.Parameters.Add("MZ_SPT1", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SPT1"].ToString() == "0000" ? "" : tempDT.Rows[i]["MZ_SPT1"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_OPEDATE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_OPEDATE"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_PESN", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_PESN"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_PHONO", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_PHONO"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_EMNAM", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_EMNAM"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_RET", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_RET"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_POWER", SqlDbType.VarChar).Value = "D";
                            Insertcmd.Parameters.Add("MZ_ABORIGINE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_ABORIGINE"].ToString().Trim() == "0" ? "N" : "Y";
                            Insertcmd.Parameters.Add("PAY_AD", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_AD"].ToString().Trim();
                            Insertcmd.Parameters.Add("MZ_STATUS2", SqlDbType.VarChar).Value = "Y";

                            //增加是否警職人員資料 20190514 by sky
                            Insertcmd.Parameters.Add("MZ_ISPOLICE", SqlDbType.VarChar).Value = tempDT.Rows[i]["MZ_SRANK"].ToStringNullSafe().SafeTrim().Substring(0, 1) == "G" ? "Y" : "N";

                            //健保利率
                            Insertcmd.Parameters.Add("MZ_INSURANCEMODE", SqlDbType.VarChar).Value = "1";

                            #endregion

                            mode = "insert";
                        }
                        else
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            string updateString = "UPDATE A_DLBASE SET MZ_STATUS2=@MZ_STATUS2,MZ_ID=@MZ_ID";
                            parameters.Add(new SqlParameter("MZ_STATUS2", "Y"));
                            parameters.Add(new SqlParameter("MZ_ID", tempDT.Rows[i]["MZ_ID"].ToStringNullSafe().ToUpper()));

                            #region 各欄位處理
                            string tempAD = tempDT.Rows[i]["MZ_AD"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempAD))
                            {
                                updateString += ",MZ_AD=@MZ_AD";
                                parameters.Add(new SqlParameter("MZ_AD", tempAD.SafeTrim()));
                            }

                            string tempUNIT = tempDT.Rows[i]["MZ_UNIT"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempUNIT))
                            {
                                updateString += ",MZ_UNIT=@MZ_UNIT";
                                parameters.Add(new SqlParameter("MZ_UNIT", tempUNIT.SafeTrim()));
                            }

                            //修改 by 介入 2012.8.29，姓名不更新，避免有已處理的特殊字又被蓋過
                            //因警政署資料已改unicode編碼(UTF8)，目前應不會遇到亂碼問題 20190730 by sky
                            string tempNAME = tempDT.Rows[i]["MZ_NAME"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempNAME))
                            {
                                updateString += ",MZ_NAME=@MZ_NAME";
                                parameters.Add(new SqlParameter("MZ_NAME", tempNAME.SafeTrim()));
                            }

                            string tempPESN = tempDT.Rows[i]["MZ_PESN"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempPESN))
                            {
                                updateString += ",MZ_PESN=@MZ_PESN";
                                parameters.Add(new SqlParameter("MZ_PESN", tempPESN.SafeTrim()));
                            }

                            string tempCHISI = tempDT.Rows[i]["MZ_CHISI"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempCHISI))
                            {
                                updateString += ",MZ_CHISI=@MZ_CHISI";
                                parameters.Add(new SqlParameter("MZ_CHISI", tempCHISI));
                            }

                            string tempENAME = tempDT.Rows[i]["MZ_ENAME"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempENAME))
                            {
                                updateString += ",MZ_ENAME=@MZ_ENAME";
                                parameters.Add(new SqlParameter("MZ_ENAME", tempENAME));
                            }

                            string tempBIR = tempDT.Rows[i]["MZ_BIR"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempBIR))
                            {
                                updateString += ",MZ_BIR=@MZ_BIR";
                                parameters.Add(new SqlParameter("MZ_BIR", tempBIR));
                            }

                            string tempSM = tempDT.Rows[i]["MZ_SM"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSM))
                            {
                                updateString += ",MZ_SM=@MZ_SM";
                                parameters.Add(new SqlParameter("MZ_SM", tempSM));
                            }

                            // sam.hsu 20201217
                            //修改 by 介入 2012.8.29，戶藉地址不更新，避免有出納修改過的資料又被蓋過
                            //A_DLBASE.MZ_ADD1 為空 才將tempADD1 更新進去
                            string tempADD1 = tempDT.Rows[i]["MZ_ADD1"].ToString().Trim();
                            if (tmpMZ_ADD1 == "Null" && !string.IsNullOrEmpty(tempADD1))
                            {
                                updateString += ",MZ_ADD1='" + tempADD1.Trim() + "'";
                            }

                            //A_DLBASE.MZ_ADD1 為空 才將tempADD1 更新進去
                            string tempADD2 = tempDT.Rows[i]["MZ_ADD2"].ToString().Trim();
                            if (tmpMZ_ADD2 == "Null" && !string.IsNullOrEmpty(tempADD2))
                            {
                                updateString += ",MZ_ADD2='" + tempADD2.Trim() + "'";
                            }                       

                            string tempPHONE = tempDT.Rows[i]["MZ_PHONE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempPHONE))
                            {
                                updateString += ",MZ_PHONE=@MZ_PHONE";
                                parameters.Add(new SqlParameter("MZ_PHONE", tempPHONE));
                            }

                            string tempEMNAM = tempDT.Rows[i]["MZ_EMNAM"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempEMNAM))
                            {
                                updateString += ",MZ_EMNAM=@MZ_EMNAM";
                                parameters.Add(new SqlParameter("MZ_EMNAM", tempEMNAM));
                            }

                            string tempPHONO = tempDT.Rows[i]["MZ_PHONO"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempPHONO))
                            {
                                updateString += ",MZ_PHONO=@MZ_PHONO";
                                parameters.Add(new SqlParameter("MZ_PHONO", tempPHONO));
                            }

                            string tempPHONH = tempDT.Rows[i]["MZ_PHONH"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempPHONH))
                            {
                                updateString += ",MZ_PHONH=@MZ_PHONH";
                                parameters.Add(new SqlParameter("MZ_PHONH", tempPHONH));
                            }

                            string tempARMYSTATE = tempDT.Rows[i]["MZ_ARMYSTATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempARMYSTATE))
                            {
                                updateString += ",MZ_ARMYSTATE=@MZ_ARMYSTATE";
                                parameters.Add(new SqlParameter("MZ_ARMYSTATE", tempARMYSTATE));
                            }

                            string tempARMYRANK = tempDT.Rows[i]["MZ_ARMYRANK"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempARMYRANK))
                            {
                                updateString += ",MZ_ARMYRANK=@MZ_ARMYRANK";
                                parameters.Add(new SqlParameter("MZ_ARMYRANK", tempARMYRANK));
                            }

                            string tempARMYKIND = tempDT.Rows[i]["MZ_ARMYKIND"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempARMYKIND))
                            {
                                updateString += ",MZ_ARMYKIND=@MZ_ARMYKIND";
                                parameters.Add(new SqlParameter("MZ_ARMYKIND", tempARMYKIND));
                            }

                            string tempARMYCOURSE = tempDT.Rows[i]["MZ_ARMYCOURSE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempARMYCOURSE))
                            {
                                updateString += ",MZ_ARMYCOURSE=@MZ_ARMYCOURSE";
                                parameters.Add(new SqlParameter("MZ_ARMYCOURSE", tempARMYCOURSE));
                            }

                            string tempSLFDATE = tempDT.Rows[i]["MZ_SLFDATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSLFDATE))
                            {
                                updateString += ",MZ_SLFDATE=@MZ_SLFDATE";
                                parameters.Add(new SqlParameter("MZ_SLFDATE", tempSLFDATE));
                            }

                            string tempSLEDATE = tempDT.Rows[i]["MZ_SLEDATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSLEDATE))
                            {
                                updateString += ",MZ_SLEDATE=@MZ_SLEDATE";
                                parameters.Add(new SqlParameter("MZ_SLEDATE", tempSLEDATE));
                            }

                            string tempTBCD9 = tempDT.Rows[i]["MZ_TBCD9"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempTBCD9))
                            {
                                updateString += ",MZ_TBCD9=@MZ_TBCD9";
                                parameters.Add(new SqlParameter("MZ_TBCD9", tempTBCD9));
                            }

                            string tempFDATE = tempDT.Rows[i]["MZ_FDATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempFDATE))
                            {
                                updateString += ",MZ_FDATE=@MZ_FDATE";
                                parameters.Add(new SqlParameter("MZ_FDATE", tempFDATE));
                            }

                            string tempZONE1 = tempDT.Rows[i]["MZ_ZONE1"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempZONE1))
                            {
                                updateString += ",MZ_ZONE1=@MZ_ZONE1";
                                parameters.Add(new SqlParameter("MZ_ZONE1", tempZONE1));
                            }

                            string tempZONE2 = tempDT.Rows[i]["MZ_ZONE2"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempZONE2))
                            {
                                updateString += ",MZ_ZONE2=@MZ_ZONE2";
                                parameters.Add(new SqlParameter("MZ_ZONE2", tempZONE2));
                            }

                            string tempABORIGINE = tempDT.Rows[i]["MZ_ABORIGINE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempABORIGINE))
                            {
                                updateString += ",MZ_ABORIGINE=@MZ_ABORIGINE";
                                parameters.Add(new SqlParameter("MZ_ABORIGINE", (tempABORIGINE == "0") ? "N" : "Y"));
                            }

                            string tempABORIGINENAME = tempDT.Rows[i]["MZ_ABORIGINENAME"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempABORIGINENAME))
                            {
                                updateString += ",MZ_ABORIGINENAME=@MZ_ABORIGINENAME";
                                parameters.Add(new SqlParameter("MZ_ABORIGINENAME", tempABORIGINENAME));
                            }

                            string tempMOVETEL = tempDT.Rows[i]["MZ_MOVETEL"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempMOVETEL))
                            {
                                updateString += ",MZ_MOVETEL=@MZ_MOVETEL";
                                parameters.Add(new SqlParameter("MZ_MOVETEL", tempMOVETEL));
                            }

                            //string tempEXAD = tempDT.Rows[i]["MZ_EXAD"].ToString().Trim();
                            //if (!string.IsNullOrEmpty(tempEXAD))
                            //{
                            //    updateString += ",MZ_EXAD='" + tempEXAD + "'";
                            //}

                            string tempPOSIND = tempDT.Rows[i]["MZ_POSIND"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempPOSIND))
                            {
                                updateString += ",MZ_POSIND=@MZ_POSIND";
                                parameters.Add(new SqlParameter("MZ_POSIND", tempPOSIND));
                            }

                            //string tempEXUNIT = tempDT.Rows[i]["MZ_EXUNIT"].ToString().Trim();
                            //if (!string.IsNullOrEmpty(tempEXUNIT))
                            //{
                            //    updateString += ",MZ_EXUNIT='" + tempEXUNIT + "'";
                            //}

                            string tempOCCC = tempDT.Rows[i]["MZ_OCCC"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempOCCC))
                            {
                                updateString += ",MZ_OCCC=@MZ_OCCC";
                                parameters.Add(new SqlParameter("MZ_OCCC", tempOCCC));
                            }

                            string tempEXTPOS = tempDT.Rows[i]["MZ_EXTPOS"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempEXTPOS))
                            {
                                updateString += ",MZ_EXTPOS=@MZ_EXTPOS";
                                parameters.Add(new SqlParameter("MZ_EXTPOS", tempEXTPOS));
                            }

                            string tempRANK = tempDT.Rows[i]["MZ_RANK"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempRANK))
                            {
                                updateString += ",MZ_RANK=@MZ_RANK";
                                parameters.Add(new SqlParameter("MZ_RANK", tempRANK));
                            }

                            string tempRANK1 = tempDT.Rows[i]["MZ_RANK1"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempRANK1))
                            {
                                updateString += ",MZ_RANK1=@MZ_RANK1";
                                parameters.Add(new SqlParameter("MZ_RANK1", tempRANK1));
                            }

                            //string tempPCHIEF = tempDT.Rows[i]["MZ_PCHIEF"].ToString().Trim();
                            //if (!string.IsNullOrEmpty(tempPCHIEF))
                            //{
                            //    updateString += ",MZ_PCHIEF='" + tempPCHIEF + "'";
                            //}

                            string tempNREA = tempDT.Rows[i]["MZ_NREA"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempNREA))
                            {
                                updateString += ",MZ_NREA=@MZ_NREA";
                                parameters.Add(new SqlParameter("MZ_NREA", tempNREA));
                            }

                            string tempIDATE = tempDT.Rows[i]["MZ_IDATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempIDATE))
                            {
                                updateString += ",MZ_IDATE=@MZ_IDATE";
                                parameters.Add(new SqlParameter("MZ_IDATE", tempIDATE));
                            }

                            string tempDATE = tempDT.Rows[i]["MZ_DATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempDATE))
                            {
                                updateString += ",MZ_DATE=@MZ_DATE";
                                parameters.Add(new SqlParameter("MZ_DATE", tempDATE));
                            }

                            string tempNID = tempDT.Rows[i]["MZ_NID"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempNID))
                            {
                                updateString += ",MZ_NID=@MZ_NID";
                                parameters.Add(new SqlParameter("MZ_NID", tempNID));
                            }

                            // sam wellsince 20201008 薪資生效日 MZ_SALARY_ISDATE 若為空 預設塞 到職日期
                            string tempADATE = tempDT.Rows[i]["MZ_ADATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempADATE))
                            {
                                updateString += ",MZ_ADATE=@MZ_ADATE";
                                parameters.Add(new SqlParameter("MZ_ADATE", tempADATE));

                                updateString += ",MZ_SALARY_ISDATE=@MZ_SALARY_ISDATE";
                                parameters.Add(new SqlParameter("MZ_SALARY_ISDATE", tempADATE));
                            }                                                                               

                            string tempSRANK = tempDT.Rows[i]["MZ_SRANK"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSRANK))
                            {
                                updateString += ",MZ_SRANK=@MZ_SRANK";
                                parameters.Add(new SqlParameter("MZ_SRANK", tempSRANK));
                            }

                            string tempSLVC = tempDT.Rows[i]["MZ_SLVC"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSLVC))
                            {
                                updateString += ",MZ_SLVC=@MZ_SLVC";
                                parameters.Add(new SqlParameter("MZ_SLVC", tempSLVC));
                            }

                            string tempSPT = tempDT.Rows[i]["MZ_SPT"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSPT))
                            {
                                updateString += ",MZ_SPT=@MZ_SPT";
                                parameters.Add(new SqlParameter("MZ_SPT", tempSPT));
                            }

                            string tempSPT1 = tempDT.Rows[i]["MZ_SPT1"].ToStringNullSafe().SafeTrim() == "0000" ? "" : tempDT.Rows[i]["MZ_SPT1"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempSPT1))
                            {
                                updateString += ",MZ_SPT1=@MZ_SPT1";
                                parameters.Add(new SqlParameter("MZ_SPT1", tempSPT1));
                            }

                            //string tempTBNREA = tempDT.Rows[i]["MZ_TBNREA"]ToString().Trim();
                            //if (!string.IsNullOrEmpty(tempTBNREA))
                            //{
                            //    updateString += ",MZ_TBNREA='" + tempTBNREA + "'";
                            //}

                            string tempLDATE = tempDT.Rows[i]["MZ_LDATE"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempLDATE))
                            {
                                updateString += ",MZ_LDATE=@MZ_LDATE";
                                parameters.Add(new SqlParameter("MZ_LDATE", tempLDATE));
                            }

                            string tempNRT = tempDT.Rows[i]["MZ_NRT"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempNRT))
                            {
                                updateString += ",MZ_NRT=@MZ_NRT";
                                parameters.Add(new SqlParameter("MZ_NRT", tempNRT));
                            }

                            //新增轉檔時一併更新A_DLBASE的MZ_TBDV欄位
                            string tempMZ_TBDV = tempDT.Rows[i]["MZ_TBDV"].ToStringNullSafe().SafeTrim();
                            if (!string.IsNullOrEmpty(tempMZ_TBDV))
                            {
                                updateString += ",MZ_TBDV=@MZ_TBDV";
                                parameters.Add(new SqlParameter("MZ_TBDV", tempMZ_TBDV));
                            }

                            //string tempPAY_AD = tempDT.Rows[i]["PAY_AD"]ToString().Trim();
                            //if (!string.IsNullOrEmpty(tempPAY_AD))
                            //{
                            //    updateString += ",PAY_AD='" + tempPAY_AD + "'";
                            //}

                            //string tempEXTPOS_RANK = tempDT.Rows[i]["MZ_EXTPOS"]ToString().Trim();
                            //if (!string.IsNullOrEmpty(tempEXTPOS_RANK))
                            //{
                            //    updateString += ",MZ_EXTPOS='" + tempEXTPOS_RANK + "'";
                            //}

                            #endregion

                            updateString += " WHERE MZ_ID=@MZ_ID";

                            Insertcmd = new SqlCommand(updateString, conn);
                            Insertcmd.Transaction = oraTran;

                            foreach (var item in parameters)
                            {
                                Insertcmd.Parameters.Add(item);
                            }

                            mode = "update";
                        }

                        Insertcmd.ExecuteNonQuery();

                        //寫入操作Log
                        if (mode == "update")
                        {
                            try
                            {
                                //2010.06.04 LOG紀錄 by伊珊
                                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));
                            }
                            catch
                            { }
                        }
                        else if (mode == "insert")
                        {
                            try
                            {
                                //2010.06.04 LOG紀錄 by伊珊
                                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));
                            }
                            catch
                            { }
                        }

                        //20140318本來是在薪資基本資料修改,但因市府規定,只能由人事室
                        //所以只要俸階,俸點(詳細欄位函式裡面)有更動,薪資基本資料就要重算
                        //修正新增帳號流程，確保每個帳號可正確對應ID，而不會導致權限新增錯誤。 20191024 by sky
                        if (mode == "insert")
                        {
                            string _strADID = tempDT.Rows[i]["MZ_ID"].ToStringNullSafe().Substring(0, 1) + tempDT.Rows[i]["MZ_ID"].ToStringNullSafe().Substring(5, 5);
                            string _strNAME = tempDT.Rows[i]["MZ_NAME"].ToStringNullSafe();
                            string _strADPWD = "Drinfo1!";
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = oraTran;
                                cmd.CommandType = CommandType.Text;

                                //取得現職機關、單位
                                cmd.CommandText = string.Format("select mz_kchimatch from a_kchi_match where mz_kcode = '{0}'", tempDT.Rows[i]["MZ_EXAD"].ToStringNullSafe());
                                string ad = "OU=" + cmd.ExecuteScalar().ToStringNullSafe();
                                cmd.CommandText = string.Format("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '{0}'", tempDT.Rows[i]["MZ_EXUNIT"].ToStringNullSafe());
                                string unit = "OU=" + cmd.ExecuteScalar().ToStringNullSafe();
                                string cn = "CN=" + _strADID;

                                //處理AP帳號
                                bool apCreate = false;
                                cmd.CommandText = string.Format("SELECT * FROM TPM_MEMBER WHERE LOWER(TPMUSER) = '{0}'", TPMPermissions._strLCase(_strADID));
                                DataTable tmDt = new DataTable();
                                tmDt.Load(cmd.ExecuteReader());
                                if (tmDt != null && tmDt.Rows.Count < 1)
                                {
                                    //新增AP帳號
                                    string insertSQL = string.Format(@"INSERT INTO TPM_MEMBER (TPMID,TPMUSER,TPMPWD,TPMADON,TPMADGROUP,TPM_IDNO,TPMPWD_OVERTIME)
                                                                        VALUES( NEXT VALUE FOR dbo.SEQ_TPM_MEMBER,@TPMUSER,@TPMPWD,@TPMADON,@TPMADGROUP,@TPM_IDNO,@TPMPWD_OVERTIME)");
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = insertSQL;
                                    cmd.Parameters.Add("TPMUSER", _strADID);
                                    cmd.Parameters.Add("TPMPWD", TPMPermissions._strEncood(_strADPWD));
                                    cmd.Parameters.Add("TPMADON", "Y");
                                    cmd.Parameters.Add("TPMADGROUP", cn + "," + unit + "," + ad);
                                    cmd.Parameters.Add("TPM_IDNO", tempDT.Rows[i]["MZ_ID"].ToStringNullSafe());
                                    cmd.Parameters.Add("TPMPWD_OVERTIME", SqlDbType.DateTime).Value = DateTime.Today;
                                    cmd.ExecuteNonQuery();

                                    apCreate = true;
                                }
                                else
                                {
                                    //已建過AP帳號，因帳號有機率出現重複，暫時不做處理
                                    apCreate = false;
                                    expErrorMsg.AppendLine(string.Format("人員ID:{0} AP帳號重複。", tempDT.Rows[i]["MZ_ID"].ToStringNullSafe()));
                                }

                                if (apCreate)
                                {
                                    #region 處理薪資資料
                                    // add by 介入 2012/02/24 建立人事資料時，同步建立薪資資料.CREATE 給新進人員
                                    //這應該沒有因為人事資料修改而重新計算薪資基本資料
                                    try
                                    {
                                        _2_salary.Police police = new _2_salary.Police(tempDT.Rows[i]["MZ_ID"].ToStringNullSafe().Trim());
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    #endregion

                                    #region AD相關(已停用)

                                    //20140627 經謝股及邱課確認 AD創帳號由WEB_AD去做,這是舊的已經停掉了
                                    //DirectoryEntry deContainer;
                                    //deContainer = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");

                                    //DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                                    //try
                                    //{
                                    //    if (chkADroot(ad, unit) == true)
                                    //    {
                                    //        DirectoryEntry child = de.Children.Add(cn + "," + unit + "," + ad, "user");
                                    //        child.CommitChanges();
                                    //        //child.Properties["sAMAccountName"].Add(_strADID);
                                    //        child.Properties["sAMAccountName"].Value = _strADID;
                                    //        child.CommitChanges();
                                    //        child.Properties["userPrincipalName"].Value = _strADID;
                                    //        child.CommitChanges();
                                    //        child.Properties["displayName"].Value = _strNAME;
                                    //        child.CommitChanges();
                                    //        child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + tempDT.Rows[i]["MZ_EXAD"].ToString() + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + tempDT.Rows[i]["MZ_EXUNIT"].ToString() + "'");
                                    //        child.Invoke("SetPassword", new object[] { _strADPWD });
                                    //        child.Properties["userAccountControl"].Value = 0x200;
                                    //        child.CommitChanges();
                                    //    }
                                    //    else
                                    //    {
                                    //        if (chkADroot(ad, _strADID) == true)
                                    //        {
                                    //            DirectoryEntry child = de.Children.Add(unit + "," + ad, "organizationalUnit");
                                    //            child.CommitChanges();

                                    //            child = de.Children.Add(cn + "," + unit + "," + ad, "user");
                                    //            child.CommitChanges();
                                    //            child.Properties["sAMAccountName"].Value = _strADID;
                                    //            child.CommitChanges();
                                    //            child.Properties["displayName"].Value = _strNAME;
                                    //            child.CommitChanges();
                                    //            child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + tempDT.Rows[i]["MZ_EXAD"].ToString() + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + tempDT.Rows[i]["MZ_EXUNIT"].ToString() + "'");
                                    //            child.Invoke("SetPassword", new object[] { _strADPWD });
                                    //            child.Properties["userAccountControl"].Value = 0x200;
                                    //            child.CommitChanges();
                                    //        }
                                    //        else
                                    //        {
                                    //            DirectoryEntry child = de.Children.Add(ad, "organizationalUnit");
                                    //            child.CommitChanges();

                                    //            child = de.Children.Add(unit + "," + ad, "organizationalUnit");
                                    //            child.CommitChanges();

                                    //            child = de.Children.Add(cn + "," + unit + "," + ad, "user");
                                    //            child.CommitChanges();
                                    //            child.Properties["sAMAccountName"].Value = _strADID;
                                    //            child.CommitChanges();
                                    //            child.Properties["displayName"].Value = _strNAME;
                                    //            child.CommitChanges();
                                    //            child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + tempDT.Rows[i]["MZ_EXAD"].ToString() + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + tempDT.Rows[i]["MZ_EXUNIT"].ToString() + "'");
                                    //            child.Invoke("SetPassword", new object[] { _strADPWD });
                                    //            child.Properties["userAccountControl"].Value = 0x200;
                                    //            child.CommitChanges();
                                    //        }
                                    //    }
                                    //}
                                    //catch
                                    //{

                                    //}

                                    #endregion

                                    #region 新增人員系統權限
                                    //insertBasicGroupPermission(tempDT.Rows[i]["MZ_ID"].ToString());

                                    try
                                    {
                                        cmd.Parameters.Clear();
                                        //取得人事管理系統相關權限代碼
                                        cmd.CommandText = "SELECT TPFID FROM TPF_FIONDATA WHERE TPFNAME='人事管理'";
                                        string TPFID = cmd.ExecuteScalar().ToStringNullSafe();
                                        cmd.CommandText = string.Format(@"SELECT TPMN_GID FROM TP_MODEL_NAME 
                                                                        Inner Join TP_GROUP_DATA ON TP_GROUP_DATA.TPG_GID = TP_MODEL_NAME.TPG_GID
                                                                        WHERE TPMN_TPFID = '{0}' AND TPG_DATANAME='D' AND ROWNUM=1",
                                                                        TPFID);
                                        string TPMN_GID = cmd.ExecuteScalar().ToStringNullSafe();

                                        //取得AP帳號
                                        cmd.CommandText = string.Format("SELECT TPMID FROM TPM_MEMBER WHERE TPM_IDNO='{0}'", tempDT.Rows[i]["MZ_ID"].ToStringNullSafe());
                                        string TPMID = cmd.ExecuteScalar().ToStringNullSafe();

                                        if (!string.IsNullOrEmpty(TPMID) && !string.IsNullOrEmpty(TPMN_GID))
                                        {
                                            //刪除該AP帳號已設權限
                                            cmd.CommandText = string.Format(@"DELETE FROM TP_MODEL_MEMBER WHERE TPMID='{0}'", TPMID);
                                            cmd.ExecuteNonQuery();
                                            //新增權限
                                            cmd.CommandText = string.Format("Insert Into TP_MODEL_MEMBER (TPMM_GID, TPMN_GID, TPMID) Values ( NEXT VALUE FOR dbo.SEQ_TP_GROUP,'{0}','{1}')", TPMN_GID, TPMID);
                                            cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            expErrorMsg.AppendLine(string.Format("人員ID:{0} AP帳號或人事管理權限新增異常。", tempDT.Rows[i]["MZ_ID"].ToStringNullSafe()));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    #endregion
                                }
                            }
                        }

                        //執行成功後清空
                        Last_mz_id = string.Empty;
                    }

                    Label1.Text = "送出資訊";
                    oraTran.Commit();
                    Label1.Text = "匯入成功";
                    if (!string.IsNullOrEmpty(expErrorMsg.ToString()))
                    {
                        Label1.Text += expErrorMsg.ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('匯入成功');", true);

                    string UPDATESQL = "UPDATE A_DLBASE SET MZ_STATUS2='Y' WHERE PAY_AD!='' AND PAY_AD IS NOT NULL";
                    o_DBFactory.ABC_toTest.Edit_Data(UPDATESQL);
                }
                catch (Exception ex)
                {
                    Label1.Text = "匯入失敗，原因:" + ex + "。 錯誤人員資料ID:" + Last_mz_id;
                    oraTran.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('匯入失敗');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToStringNullSafe(), Request.QueryString["TPM_FION"].ToStringNullSafe(), "error_", true, ex.Message);
                }
                finally
                {
                    conn.Close();
                    //XX2013/06/18 
                    conn.Dispose();
                }
            }
        }

        protected void btTP37_DLBASETable1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal1-4Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        private string strAddAP(string strAddID, string strAddName, string strAddPWD, string strAddChkAD, string strADGroup, string strIDNO)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                string strSQL = "SELECT * FROM TPM_MEMBER WHERE LOWER(TPMUSER) = '" + TPMPermissions._strLCase(strAddID) + "'";
                DataTable dt = new DataTable();
                SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));

                if (dt.Rows.Count < 1)
                {
                    if (boolAddAP_Create(strAddID, strAddPWD, strAddChkAD, strADGroup, strIDNO) == false)
                    {
                        return "AP帳號新增失敗";
                    }


                    return "新增成功";
                }
                else
                { return "新增失敗，帳號已重覆"; }

            }
        }

        private bool boolAddAP_Create(string strAddID, string strAddPWD, string strAddChkAD, string strADGroup, string strIDNO)
        {
            using (SqlConnection Selectconn1 = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn1.Open();
                try
                {
                    string insertString = "INSERT INTO TPM_MEMBER (TPMID,TPMUSER,TPMPWD,TPMADON,TPMADGROUP,TPM_IDNO,TPMPWD_OVERTIME) "
                    + " VALUES( NEXT VALUE FOR dbo.SEQ_TPM_MEMBER,'" + strAddID + "','" + TPMPermissions._strEncood(strAddPWD) + "','" + strAddChkAD + "','" + strADGroup + "','" + strIDNO + "',@TPMPWD_OVERTIME)";  //新增時已設自動編號
                    SqlCommand cmd = new SqlCommand(insertString, Selectconn1);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("TPMPWD_OVERTIME", SqlDbType.DateTime).Value = DateTime.Today;

                    string sqlstr = insertString;
                    //新增事件
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), sqlstr) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", sqlstr);
                    }
                    //新增權限
                    if (TPMPermissions._boolPermissionID(int.Parse(Session["TPM_MID"].ToString()), Request.QueryString["TPM_FION"].ToString(), "PADD") == false)
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001", sqlstr);
                        Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    }

                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch { return false; }
                finally
                { 
                    Selectconn1.Close();

                    //XX2013/06/18 
                    Selectconn1.Dispose();
                }
            }
        }

        //protected bool chkADroot(string ad, string unit, string mz_id)
        //{
        //    try
        //    {
        //        string _strADID = mz_id.Substring(0, 1) + mz_id.Substring(5, 5);
        //        DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
        //        de.Children.Find(unit + "," + ad);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //protected bool chkADroot(string ad, string mz_id)
        //{
        //    try
        //    {
        //        string _strADID = mz_id.Substring(0, 1) + mz_id.Substring(5, 5);
        //        DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
        //        de.Children.Find(ad);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != TDLBASE.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TDLBASE.Count.ToString() + "筆";
        }

        protected void insertBasicGroupPermission(string MZ_ID)
        {

            string TPFID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPFID FROM TPF_FIONDATA WHERE TPFNAME='人事管理'");

            string TPMN_GID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMN_GID FROM TP_MODEL_NAME Inner Join TP_GROUP_DATA ON TP_GROUP_DATA.TPG_GID = TP_MODEL_NAME.TPG_GID  WHERE TPMN_TPFID = '" + TPFID + "' AND TPG_DATANAME='D' AND ROWNUM=1");

            string TPMID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMID FROM TPM_MEMBER WHERE TPM_IDNO='" + MZ_ID + "'");

            string deleteString = "DELETE FROM TP_MODEL_MEMBER WHERE TPMID='" + TPMID + "'";

            string insertString = "Insert Into TP_MODEL_MEMBER (TPMM_GID, TPMN_GID, TPMID) "
                                   + "Values ( NEXT VALUE FOR dbo.SEQ_TP_GROUP,'" + TPMN_GID + "','" + TPMID + "')";

            o_DBFactory.ABC_toTest.Edit_Data(insertString);

        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == TDLBASE.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == TDLBASE.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TDLBASE.Count.ToString() + "筆";
        }
    }
}
