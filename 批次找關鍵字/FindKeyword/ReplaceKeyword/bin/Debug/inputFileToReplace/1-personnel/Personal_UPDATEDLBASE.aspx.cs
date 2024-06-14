using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.DirectoryServices;

namespace TPPDDB._1_personnel
{
    public partial class Personal_UPDATEDLBASE : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();

            A.set_Panel_EnterToTAB(ref this.Panel1);
         
                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":

                    break;
                case "C":
                case "D":
                case "E":
                    DropDownList_MZ_PRID.DataBind();
                    DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_MZ_PRID.Enabled = false;
                    break;
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(MZ_ID) FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料請重新輸入');", true);
            }
            else
            {
                string UpdateFromPositString = "SELECT * FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";
                DataTable Updatedt = new DataTable();
                Updatedt = o_DBFactory.ABC_toTest.Create_Table(UpdateFromPositString, "GET");

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    conn.Open();

                    bool check = true;
                    SqlTransaction oraTran = conn.BeginTransaction();
                    SqlCommand Insertcmd = new SqlCommand();
                    SqlCommand InsertCareercmd = new SqlCommand();

                    try
                    {
                        for (int i = 0; i < Updatedt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(Updatedt.Rows[i]["MZ_EXOAD"].ToString()))
                            {
                                string careerSelectString = "SELECT * FROM A_CAREER WHERE MZ_ID='" + Updatedt.Rows[i]["MZ_ID"].ToString() + "'";

                                DataTable tempCAREER = new DataTable();

                                tempCAREER = o_DBFactory.ABC_toTest.Create_Table(careerSelectString, "SELECT");

                                if (tempCAREER.Rows.Count == 0)
                                {
                                    string InsertString = "INSERT INTO A_CAREER(MZ_AD,MZ_UNIT,MZ_OCCC,MZ_ID,MZ_RANK,MZ_RANK1,MZ_PCHIEF,MZ_TBDATE,MZ_TBID,MZ_NREA,MZ_TBNREA,MZ_EXID,MZ_EXTPOS,MZ_ISEXTPOS,SN) " +
                                                                      " VALUES (@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_ID,@MZ_RANK,@MZ_RANK1,@MZ_PCHIEF,@MZ_TBDATE,@MZ_TBID,@MZ_NREA,@MZ_TBNREA,@MZ_EXID,@MZ_EXTPOS,@MZ_ISEXTPOS,nextval) ";

                                    InsertCareercmd = new SqlCommand(InsertString, conn);
                                    InsertCareercmd.Transaction = oraTran;

                                    InsertCareercmd.Parameters.Add("MZ_AD", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOAD"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_UNIT", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOUNIT"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_OCCC", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOOCC"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_ID", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_ID"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_RANK", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXORANK"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_RANK1", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXRANK1"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_PCHIEF", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOPCHIEF"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_TBDATE", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_ADATE"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_TBID", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXID"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_NREA", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_TBNREA"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_TBNREA", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_NREA"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_EXID", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_TBNREA"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_EXTPOS", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXTPOS"].ToString().Trim();
                                    InsertCareercmd.Parameters.Add("MZ_ISEXTPOS", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_ISEXTPOS"].ToString().Trim();
                                }
                                else
                                {


                                    for (int j = 0; j < tempCAREER.Rows.Count; j++)
                                    {
                                        if (tempCAREER.Rows[j]["MZ_AD"].ToString() == Updatedt.Rows[i]["MZ_EXOAD"].ToString()
                                            && tempCAREER.Rows[j]["MZ_UNIT"].ToString() == Updatedt.Rows[i]["MZ_EXOUNIT"].ToString()
                                            && tempCAREER.Rows[j]["MZ_OCCC"].ToString() == Updatedt.Rows[i]["MZ_EXOOCC"].ToString())
                                        {
                                            check = false;
                                        }
                                    }

                                    if (check)
                                    {
                                        string InsertString = "INSERT INTO A_CAREER(MZ_AD,MZ_UNIT,MZ_OCCC,MZ_ID,MZ_RANK,MZ_RANK1,MZ_PCHIEF,MZ_TBDATE,MZ_TBID,MZ_NREA,MZ_TBNREA,MZ_EXID,MZ_EXTPOS,MZ_ISEXTPOS,SN) " +
                                                                      " VALUES (@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_ID,@MZ_RANK,@MZ_RANK1,@MZ_PCHIEF,@MZ_TBDATE,@MZ_TBID,@MZ_NREA,@MZ_TBNREA,@MZ_EXID,@MZ_EXTPOS,@MZ_ISEXTPOS,nextval) ";

                                        InsertCareercmd = new SqlCommand(InsertString, conn);
                                        InsertCareercmd.Transaction = oraTran;

                                        InsertCareercmd.Parameters.Add("MZ_AD", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOAD"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_UNIT", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOUNIT"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_OCCC", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOOCC"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_ID", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_ID"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_RANK", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXORANK"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_RANK1", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXRANK1"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_PCHIEF", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXOPCHIEF"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_TBDATE", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_ADATE"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_TBID", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXID"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_NREA", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_TBNREA"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_TBNREA", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_NREA"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_EXID", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_TBNREA"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_EXTPOS", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_EXTPOS"].ToString().Trim();
                                        InsertCareercmd.Parameters.Add("MZ_ISEXTPOS", SqlDbType.NVarChar).Value = Updatedt.Rows[i]["MZ_ISEXTPOS"].ToString().Trim();

                                    }
                                }
                            }
                            else
                            {
                                check = false;
                            }

                            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + Updatedt.Rows[i]["MZ_ID"].ToString() + "'")))
                            {
                                ViewState["modelcmd"] = "insert";
                                string InsertString = "INSERT INTO A_DLBASE (MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_RANK,MZ_RANK1,MZ_OCCC,MZ_CHISI,MZ_TBDV,MZ_POSIND,MZ_NIN,MZ_PESN, " +
                                                                         " MZ_PCHIEF,MZ_PNO,MZ_PNO1,MZ_TNO,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_RET," +
                                                                         " MZ_NRT,MZ_EXTPOS,MZ_ISEXTPOS,MZ_POWER,MZ_DATE,MZ_IDATE,MZ_ADATE,MZ_FDATE) " +
                                                                   "VALUES(@MZ_ID,@MZ_NAME,@MZ_AD,@MZ_UNIT,@MZ_RANK,@MZ_RANK1,@MZ_OCCC,@MZ_CHISI,@MZ_TBDV,@MZ_POSIND,@MZ_NIN,@MZ_PESN, " +
                                                                         " @MZ_PCHIEF,@MZ_PNO,@MZ_PNO1,@MZ_TNO,@MZ_SRANK,@MZ_SLVC,@MZ_SPT,@MZ_SPT1,@MZ_RET," +
                                                                         " @MZ_NRT,@MZ_EXTPOS,@MZ_ISEXTPOS,@MZ_POWER,@MZ_DATE,@MZ_IDATE,@MZ_ADATE,@MZ_FDATE)";

                                Insertcmd = new SqlCommand(InsertString, conn);
                                Insertcmd.Transaction = oraTran;
                                Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_ID"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_NAME", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_NAME"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_AD"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_UNIT"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_RANK"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_RANK1"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_OCCC"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_CHISI"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_TBDV"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_POSIND"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_NIN", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_NIN"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_PESN", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_PESN"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_PCHIEF"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_PNO", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_PNO"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_PNO1", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_PNO1"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_TNO", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_TNO"].ToString().Trim();
                                // Insertcmd.Parameters.Add("MZ_OPFDATE", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_WDATE"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_SRANK"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_SLVC", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_SLVC"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_SPT", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_SPT"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_SPT1", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_SPT1"].ToString().Trim();
                                //Insertcmd.Parameters.Add("MZ_REMARK", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_REMARK"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_RET", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_RET"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_NRT", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_NRT"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_EXTPOS", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_EXTPOS2"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_ISEXTPOS", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_ISEXTPOS2"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_DATE"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_IDATE"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_ADATE"].ToString().Trim();
                                Insertcmd.Parameters.Add("MZ_POWER", SqlDbType.VarChar).Value = "D";
                                Insertcmd.Parameters.Add("MZ_FDATE", SqlDbType.VarChar).Value = Updatedt.Rows[i]["MZ_DATE"].ToString().Trim();

                            }
                            else
                            {
                                //string selectString = "SELECT MZ_AD,MZ_UNIT,MZ_RANK,MZ_RANK1,MZ_OCCC,MZ_CHISI,MZ_TBDV,MZ_POSIND,MZ_NIN,MZ_PESN, " +
                                //                                         " MZ_PCHIEF,MZ_PNO,MZ_PNO1,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_REMARK,MZ_RET," +
                                //                                         " MZ_SWT,MZ_NRT,MZ_EXTPOS,MZ_ISEXTPOS " +
                                //                       "FROM A_DLBASE WHERE MZ_ID='" + Updatedt.Rows[i]["MZ_ID"].ToString() + "'";

                                //DataTable selectDT = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET2");
                                ViewState["modelcmd"] = "update";
                                string InsertString = "UPDATE A_DLBASE SET MZ_ID='" + Updatedt.Rows[i]["MZ_ID"].ToString() + "'";

                                string MZ_AD = Updatedt.Rows[i]["MZ_AD"].ToString().Trim();
                                string MZ_UNIT = Updatedt.Rows[i]["MZ_UNIT"].ToString().Trim();
                                string MZ_RANK = Updatedt.Rows[i]["MZ_RANK"].ToString().Trim();
                                string MZ_RANK1 = Updatedt.Rows[i]["MZ_RANK1"].ToString().Trim();
                                string MZ_OCCC = Updatedt.Rows[i]["MZ_OCCC"].ToString().Trim();
                                string MZ_CHISI = Updatedt.Rows[i]["MZ_CHISI"].ToString().Trim();
                                string MZ_TBDV = Updatedt.Rows[i]["MZ_TBDV"].ToString().Trim();
                                string MZ_POSIND = Updatedt.Rows[i]["MZ_POSIND"].ToString().Trim();
                                string MZ_NIN = Updatedt.Rows[i]["MZ_NIN"].ToString().Trim();
                                string MZ_PESN = Updatedt.Rows[i]["MZ_PESN"].ToString().Trim();
                                string MZ_PCHIEF = Updatedt.Rows[i]["MZ_PCHIEF"].ToString().Trim();
                                string MZ_PNO = Updatedt.Rows[i]["MZ_PNO"].ToString().Trim();
                                string MZ_PNO1 = Updatedt.Rows[i]["MZ_PNO1"].ToString().Trim();
                                string MZ_TNO = Updatedt.Rows[i]["MZ_TNO"].ToString().Trim();
                                string MZ_WDATE = Updatedt.Rows[i]["MZ_WDATE"].ToString().Trim();
                                string MZ_SRANK = Updatedt.Rows[i]["MZ_SRANK"].ToString().Trim();
                                string MZ_SLVC = Updatedt.Rows[i]["MZ_SLVC"].ToString().Trim();
                                string MZ_SPT = Updatedt.Rows[i]["MZ_SPT"].ToString().Trim();
                                string MZ_SPT1 = Updatedt.Rows[i]["MZ_SPT1"].ToString().Trim();
                                //string MZ_REMARK = Updatedt.Rows[i]["MZ_REMARK"].ToString().Trim();
                                string MZ_RET = Updatedt.Rows[i]["MZ_RET"].ToString().Trim();
                                string MZ_NRT = Updatedt.Rows[i]["MZ_NRT"].ToString().Trim();
                                string MZ_EXTPOS2 = Updatedt.Rows[i]["MZ_EXTPOS2"].ToString().Trim();
                                string MZ_ISEXTPOS2 = Updatedt.Rows[i]["MZ_ISEXTPOS2"].ToString().Trim();
                                string MZ_DATE = Updatedt.Rows[i]["MZ_DATE"].ToString().Trim();
                                string MZ_IDATE = Updatedt.Rows[i]["MZ_IDATE"].ToString().Trim();
                                string MZ_ADATE = Updatedt.Rows[i]["MZ_ADATE"].ToString().Trim();

                                if (!string.IsNullOrEmpty(MZ_AD))
                                {
                                    InsertString += ",MZ_AD='" + MZ_AD + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_UNIT))
                                {
                                    InsertString += ",MZ_UNIT='" + MZ_UNIT + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_RANK))
                                {
                                    InsertString += ",MZ_RANK='" + MZ_RANK + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_RANK1))
                                {
                                    InsertString += ",MZ_RANK1='" + MZ_RANK1 + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_OCCC))
                                {
                                    InsertString += ",MZ_OCCC='" + MZ_OCCC + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_CHISI))
                                {
                                    InsertString += ",MZ_CHISI='" + MZ_CHISI + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_TBDV))
                                {
                                    InsertString += ",MZ_TBDV='" + MZ_TBDV + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_POSIND))
                                {
                                    InsertString += ",MZ_POSIND='" + MZ_POSIND + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_NIN))
                                {
                                    InsertString += ",MZ_NIN='" + MZ_NIN + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_PESN))
                                {
                                    InsertString += ",MZ_PESN='" + MZ_PESN + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_PCHIEF))
                                {
                                    InsertString += ",MZ_PCHIEF='" + MZ_PCHIEF + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_PNO1))
                                {
                                    InsertString += ",MZ_PNO1='" + MZ_PNO1 + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_PNO))
                                {
                                    InsertString += ",MZ_PNO='" + MZ_PNO + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_TNO))
                                {
                                    InsertString += ",MZ_TNO='" + MZ_TNO + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_WDATE))
                                {
                                    InsertString += ",MZ_WDATE='" + MZ_WDATE + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_SRANK))
                                {
                                    InsertString += ",MZ_SRANK='" + MZ_SRANK + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_SLVC))
                                {
                                    InsertString += ",MZ_SLVC='" + MZ_SLVC + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_SPT))
                                {
                                    InsertString += ",MZ_SPT='" + MZ_SPT + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_SPT1))
                                {
                                    InsertString += ",MZ_SPT1='" + MZ_SPT1 + "'";
                                }

                                //if (!string.IsNullOrEmpty(MZ_REMARK))
                                //{
                                //    InsertString += ",MZ_REMARK='" + MZ_REMARK + "'";
                                //}

                                if (!string.IsNullOrEmpty(MZ_RET))
                                {
                                    InsertString += ",MZ_RET='" + MZ_RET + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_NRT))
                                {
                                    InsertString += ",MZ_NRT='" + MZ_NRT + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_EXTPOS2))
                                {
                                    InsertString += ",MZ_EXTPOS='" + MZ_EXTPOS2 + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_ISEXTPOS2))
                                {
                                    InsertString += ",MZ_ISEXTPOS='" + MZ_ISEXTPOS2 + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_DATE))
                                {
                                    InsertString += ",MZ_DATE='" + MZ_DATE + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_IDATE))
                                {
                                    InsertString += ",MZ_IDATE='" + MZ_IDATE + "'";
                                }

                                if (!string.IsNullOrEmpty(MZ_ADATE))
                                {
                                    InsertString += ",MZ_ADATE='" + MZ_ADATE + "'";
                                }

                                InsertString += " WHERE MZ_ID='" + Updatedt.Rows[i]["MZ_ID"].ToString() + "'";

                                Insertcmd = new SqlCommand(InsertString, conn);

                                Insertcmd.Transaction = oraTran;
                            }

                            if (check)
                            {
                                InsertCareercmd.ExecuteNonQuery();
                            }

                            Insertcmd.ExecuteNonQuery();
                            
                            if (ViewState["modelcmd"].ToString() == "insert")
                            {
                                string LBMSG = "新增成功";
                                string _strADID = Updatedt.Rows[i]["MZ_ID"].ToString().Substring(0, 1) + Updatedt.Rows[i]["MZ_ID"].ToString().Substring(5, 5);
                                string _strADPWD = "tcp" + Updatedt.Rows[i]["MZ_ID"].ToString().Substring(0, 5) + "!";
                                string ad = "OU=" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + Updatedt.Rows[i]["MZ_AD"].ToString().Trim() + "'");
                                string unit = "OU=" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + Updatedt.Rows[i]["MZ_UNIT"].ToString().Trim() + "'");
                                string cn = "CN=" + _strADID;
                                string strAddAP_MSG = strAddAP(_strADID, o_A_DLBASE.CNAME(Updatedt.Rows[i]["MZ_ID"].ToString().Trim()), _strADPWD, "Y", cn + "," + unit + "," + ad, Updatedt.Rows[i]["MZ_ID"].ToString());
                                if (strAddAP_MSG == "新增成功")
                                {
                                    #region AD相關

                                    DirectoryEntry deContainer;
                                    deContainer = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                                    try
                                    {

                                        //if (DropDownList_MZ_UNIT.SelectedValue == "-1" || DropDownList_MZ_AD.SelectedValue == "-1")
                                        //{
                                        //    //Label_MSG.Text = "請選擇單位";
                                        //}
                                        //else
                                        //{

                                        DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                                        try
                                        {
                                            if (chkADroot(ad, unit) == true)
                                            {
                                                DirectoryEntry child = de.Children.Add(cn + "," + unit + "," + ad, "user");
                                                child.CommitChanges();
                                                //child.Properties["sAMAccountName"].Add(_strADID);
                                                child.Properties["sAMAccountName"].Value = _strADID;
                                                child.CommitChanges();
                                                child.Properties["userPrincipalName"].Value = _strADID;
                                                child.CommitChanges();
                                                child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + Updatedt.Rows[i]["MZ_AD"].ToString().Trim() + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + Updatedt.Rows[i]["MZ_UNIT"].ToString().Trim() + "'");
                                                child.Invoke("SetPassword", new object[] { _strADPWD });
                                                child.Properties["userAccountControl"].Value = 0x200;
                                                child.CommitChanges();
                                            }
                                            else
                                            {
                                                if (chkADroot(ad) == true)
                                                {
                                                    DirectoryEntry child = de.Children.Add(unit + "," + ad, "organizationalUnit");
                                                    child.CommitChanges();

                                                    child = de.Children.Add(cn + "," + unit + "," + ad, "user");
                                                    child.CommitChanges();
                                                    child.Properties["sAMAccountName"].Value = _strADID;
                                                    child.CommitChanges();
                                                    child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + Updatedt.Rows[i]["MZ_AD"].ToString().Trim() + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + Updatedt.Rows[i]["MZ_UNIT"].ToString().Trim() + "'");
                                                    child.Invoke("SetPassword", new object[] { _strADPWD });
                                                    child.Properties["userAccountControl"].Value = 0x200;
                                                    child.CommitChanges();
                                                }
                                                else
                                                {
                                                    DirectoryEntry child = de.Children.Add(ad, "organizationalUnit");
                                                    child.CommitChanges();

                                                    child = de.Children.Add(unit + "," + ad, "organizationalUnit");
                                                    child.CommitChanges();

                                                    child = de.Children.Add(cn + "," + unit + "," + ad, "user");
                                                    child.CommitChanges();
                                                    child.Properties["sAMAccountName"].Value = _strADID;
                                                    child.CommitChanges();
                                                    child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + Updatedt.Rows[i]["MZ_AD"].ToString().Trim() + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + Updatedt.Rows[i]["MZ_UNIT"].ToString().Trim() + "'");
                                                    child.Invoke("SetPassword", new object[] { _strADPWD });
                                                    child.Properties["userAccountControl"].Value = 0x200;
                                                    child.CommitChanges();
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            LBMSG = "人事資料新增成功但建立AD帳號時發生非預期的意外，請聯絡網管人員";
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    #endregion

                                    string TPFID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPFID FROM TPF_FIONDATA WHERE TPFNAME='人事管理'");

                                    string TPMN_GID = "SELECT TPMN_GID FROM TP_MODEL_NAME Inner Join TP_GROUP_DATA ON TP_GROUP_DATA.TPG_GID = TP_MODEL_NAME.TPG_GID  WHERE TPMN_TPFID = '" + TPFID + "' AND TPG_DATANAME='D'";

                                    string TPMID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMID FROM TPM_MEMBER WHERE TPM_IDNO='" + Updatedt.Rows[i]["MZ_ID"].ToString().Trim() + "'");

                                    string insertString = "Insert Into TP_MODEL_MEMBER (TPMM_GID, TPMN_GID, TPMID) "
                                                           + "Values ( NEXT VALUE FOR dbo.SEQ_TP_GROUP, '" + TPMN_GID + "', '" + TPMID + "')";

                                    try
                                    {
                                        o_DBFactory.ABC_toTest.Edit_Data(insertString);
                                    }
                                    catch
                                    {

                                    }

                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + LBMSG + "');", true);

                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + LBMSG + "');location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('人事資料新增成功但建立AD帳號及系統帳號時發生非預期的意外，請聯絡網管人');location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                                }

                                ViewState["modelcmd"] = string.Empty;
                            }
                            if (i == Updatedt.Rows.Count - 1)
                            {
                                oraTran.Commit();
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更新成功');", true);
                            }
                        }
                    }
                    catch
                    {
                        oraTran.Rollback();

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更新失敗');", true);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
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
        protected bool chkADroot(string ad, string unit)
        {
            try
            {
                string mz_id = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'");
                string _strADID = mz_id.Substring(0, 1) + mz_id.Substring(5, 5);
                DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                de.Children.Find(unit + "," + ad);
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected bool chkADroot(string ad)
        {
            try
            {
                string mz_id = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'");
                string _strADID = mz_id.Substring(0, 1) + mz_id.Substring(5, 5);
                DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                de.Children.Find(ad);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
        }
    }
}
