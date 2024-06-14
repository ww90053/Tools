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

namespace TPPDDB._1_personnel
{
    public partial class Personal_Efficiency2 : System.Web.UI.Page
    {
        List<String> REV_BASE_MZ_ID = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
            }

            ViewState["MZ_ID"] = Session["PersonalSearch_ID"];
            Session.Remove("PersonalSearch_ID");


            ViewState["MZ_NAME"] = Session["PersonalSearch_NAME"];
            Session.Remove("PersonalSearch_NAME");

            ViewState["MZ_AD"] = Request["MZ_AD"];
            ViewState["MZ_UNIT"] = Request["MZ_UNIT"];
            ViewState["MZ_YEAR"] = Request["MZ_YEAR"];
            ViewState["MZ_SWT"] = Request["MZ_SWT"];

            A.set_Panel_EnterToTAB(ref this.Panel_REV_BASE);
            A.set_Panel_EnterToTAB(ref this.Panel3);

            if (!Page.IsPostBack)
            {

                TextBox_MZ_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXAD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_CHISI1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXUNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SLVC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SRANK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_UNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_TBDV1.Attributes.Add("onkeydown", "javascript:return false;");

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && REV_BASE_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < REV_BASE_MZ_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == REV_BASE_MZ_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = true;

                }

                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL = "SELECT MZ_ID,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_REV_BASE WHERE MZ_YEAR='" + ViewState["MZ_YEAR"].ToString() + "'";

                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND MZ_NAME LIKE '" + ViewState["MZ_NAME"].ToString().Trim() + "%'";
                    }

                    if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL += " AND MZ_AD='" + ViewState["MZ_AD"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_UNIT"].ToString() != "")
                    {
                        if (ViewState["MZ_AD"].ToString() == "382130100C" || ViewState["MZ_AD"].ToString() == "382130200C" || ViewState["MZ_AD"].ToString() == "382130300C")
                        {
                            strSQL += " AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString().Trim() + "'";
                        }
                        else
                        {
                            strSQL += " AND MZ_EXUNIT='" + ViewState["MZ_UNIT"].ToString().Trim() + "'";
                        }
                    }

                    if (ViewState["MZ_SWT"].ToString() != "")
                    {
                        strSQL += " AND MZ_SWT='" + ViewState["MZ_SWT"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_AD"].ToString() == "382130100C" || ViewState["MZ_AD"].ToString() == "382130200C" || ViewState["MZ_AD"].ToString() == "382130300C")
                    {
                        strSQL += " ORDER BY MZ_AD,MZ_UNIT,TBDV,MZ_OCCC,MZ_NUM";
                    }
                    else
                    {
                        strSQL += " ORDER BY MZ_AD,MZ_EXUNIT,TBDV,MZ_OCCC,MZ_NUM";
                    }

                    REV_BASE_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");

                    Session["REV_BASE_MZ_ID"] = REV_BASE_MZ_ID;

                    if (REV_BASE_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                        // Response.Write(@"<script language=javascript>window.alert('查無資料！');</script>");
                    }
                    else if (REV_BASE_MZ_ID.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        btNEXT.Enabled = true;
                    }

                    if (REV_BASE_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + REV_BASE_MZ_ID.Count.ToString() + "筆";
                    }
                }

                A.controlEnable(ref this.Panel_REV_BASE, false);
            }
        }

        protected void finddata(int DataCount)
        {
            REV_BASE_MZ_ID = Session["REV_BASE_MZ_ID"] as List<String>;

            string strSQL = "SELECT * FROM A_REV_BASE WHERE MZ_ID='" + REV_BASE_MZ_ID[DataCount].ToString().Trim() + "' AND MZ_YEAR='" + ViewState["MZ_YEAR"].ToString() + "'";

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 1)
            {
                TextBox_MZ_YEAR.Text = dt.Rows[0]["MZ_YEAR"].ToString().Trim();
                TextBox_MZ_ID.Text = dt.Rows[0]["MZ_ID"].ToString().Trim();
                TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString().Trim();
                TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_EXAD.Text = dt.Rows[0]["MZ_EXAD"].ToString().Trim();
                TextBox_MZ_EXUNIT.Text = dt.Rows[0]["MZ_EXUNIT"].ToString().Trim();
                TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_TBDV.Text = dt.Rows[0]["MZ_TBDV"].ToString().Trim();
                TextBox_MZ_SRANK.Text = dt.Rows[0]["MZ_SRANK"].ToString().Trim();
                TextBox_MZ_SLVC.Text = dt.Rows[0]["MZ_SLVC"].ToString().Trim();
                TextBox_MZ_SPT.Text = dt.Rows[0]["MZ_SPT"].ToString().Trim();
                TextBox_MZ_P4001.Text = dt.Rows[0]["MZ_P4001"].ToString().Trim();
                TextBox_MZ_P4010.Text = dt.Rows[0]["MZ_P4010"].ToString().Trim();
                TextBox_MZ_P4100.Text = dt.Rows[0]["MZ_P4100"].ToString().Trim();
                TextBox_MZ_P5001.Text = dt.Rows[0]["MZ_P5001"].ToString().Trim();
                TextBox_MZ_P5010.Text = dt.Rows[0]["MZ_P5010"].ToString().Trim();
                TextBox_MZ_P5100.Text = dt.Rows[0]["MZ_P5100"].ToString().Trim();
                TextBox_MZ_CODE01.Text = dt.Rows[0]["MZ_CODE01"].ToString().Trim();
                TextBox_MZ_CODE02.Text = dt.Rows[0]["MZ_CODE02"].ToString().Trim();
                TextBox_MZ_CODE18.Text = dt.Rows[0]["MZ_CODE18"].ToString().Trim();
                TextBox_MZ_CODE04.Text = dt.Rows[0]["MZ_CODE04"].ToString().Trim();
                TextBox_MZ_CODE05.Text = dt.Rows[0]["MZ_CODE05"].ToString().Trim();
                TextBox_MZ_CODE08.Text = dt.Rows[0]["MZ_CODE08"].ToString().Trim();
                TextBox_MZ_CODE19.Text = dt.Rows[0]["MZ_CODE19"].ToString().Trim();
                TextBox_MZ_CODE20.Text = dt.Rows[0]["MZ_CODE20"].ToString().Trim();
                TextBox_MZ_POSIND.Text = dt.Rows[0]["MZ_POSIND"].ToString().Trim();
                TextBox_MZ_CHISI.Text = dt.Rows[0]["MZ_CHISI"].ToString().Trim();
                TextBox_MZ_MEMO.Text = dt.Rows[0]["MZ_MEMO"].ToString();
                TextBox_MZ_PCHIEF.Text = dt.Rows[0]["MZ_PCHIEF"].ToString();
                TextBox_MZ_GRADE.Text = dt.Rows[0]["MZ_GRADE"].ToString();
                TextBox_MZ_NUM.Text = dt.Rows[0]["MZ_NUM"].ToString();
                string S = dt.Rows[0]["MZ_SWT"].ToString();
                DropDownList_MZ_SWT.SelectedValue = dt.Rows[0]["MZ_SWT"].ToString();


                TextBox_MZ_PCHIEF1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PCHIEF.Text, "56");
                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'");
                TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXAD.Text.Trim()) + "'");
                TextBox_MZ_CHISI1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_CHISI.Text, "23");
                TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXUNIT.Text.Trim()) + "'");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_SLVC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SLVC.Text, "64");
                TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "'");
                TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
            }
            btUpdate.Enabled = true;
            btDelete.Enabled = true;
        }
        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO A_REV_BASE(MZ_YEAR,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_TBDV,MZ_SRANK," +
                                                        " MZ_SLVC,MZ_SPT,MZ_REVIEW,MZ_NUM,MZ_GRADE,MZ_P4001,MZ_P4010,MZ_P4100,MZ_P5001," +
                                                        " MZ_P5010,MZ_P5100,MZ_CODE01,MZ_CODE02,MZ_CODE18,MZ_CODE04,MZ_CODE05,MZ_CODE08," +
                                                        " MZ_CODE19,MZ_CODE20,MZ_POSIND,MZ_CHISI,MZ_PCHIEF,MZ_SWT,MZ_MEMO,INSDATE,INSID," +
                                                        " INSEXAD,UP_DATE,UP_ID,UP_EXAD,MZ_IDATE,MZ_OC1,MZ_RK1)" +
                                                 " VALUES(@MZ_YEAR,@MZ_ID,@MZ_NAME,@MZ_AD,@MZ_UNIT,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC,@MZ_TBDV,@MZ_SRANK," +
                                                        " @MZ_SLVC,@MZ_SPT,@MZ_REVIEW,@MZ_NUM,@MZ_GRADE,@MZ_P4001,@MZ_P4010,@MZ_P4100,@MZ_P5001," +
                                                        " @MZ_P5010,@MZ_P5100,@MZ_CODE01,@MZ_CODE02,@MZ_CODE18,@MZ_CODE04,@MZ_CODE05,@MZ_CODE08," +
                                                        " @MZ_CODE19,@MZ_CODE20,@MZ_POSIND,@MZ_CHISI,@MZ_PCHIEF,@MZ_SWT,@MZ_MEMO,@INSDATE,@INSID," +
                                                        " @INSEXAD,@MZ_IDATE)";

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_REV_BASE, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);

        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";
            ViewState["CMDSQL"] = "UPDATE A_REV_BASE SET  MZ_YEAR=@MZ_YEAR,MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_EXAD=@MZ_EXAD," +
                                                        " MZ_EXUNIT=@MZ_EXUNIT,MZ_OCCC=@MZ_OCCC,MZ_TBDV=@MZ_TBDV,MZ_SRANK=@MZ_SRANK," +
                                                        " MZ_SLVC=@MZ_SLVC,MZ_SPT=@MZ_SPT,MZ_REVIEW=@MZ_REVIEW,MZ_NUM=@MZ_NUM,MZ_GRADE=@MZ_GRADE," +
                                                        " MZ_P4001=@MZ_P4001,MZ_P4010=@MZ_P4010,MZ_P4100=@MZ_P4100,MZ_P5001=@MZ_P5001," +
                                                        " MZ_P5010=@MZ_P5010,MZ_P5100=@MZ_P5100,MZ_CODE01=@MZ_CODE01,MZ_CODE02=@MZ_CODE02," +
                                                        " MZ_CODE18=@MZ_CODE18,MZ_CODE04=@MZ_CODE04,MZ_CODE05=@MZ_CODE05,MZ_CODE08=@MZ_CODE08," +
                                                        " MZ_CODE19=@MZ_CODE19,MZ_CODE20=@MZ_CODE20,MZ_POSIND=@MZ_POSIND,MZ_CHISI=@MZ_CHISI," +
                                                        " MZ_PCHIEF=@MZ_PCHIEF,MZ_SWT=@MZ_SWT,MZ_MEMO=@MZ_MEMO," +
                                                        " UP_DATE=@UP_DATE,UP_ID=@UP_ID,UP_EXAD=@UP_EXAD,MZ_IDATE=@MZ_IDATE" +
                                                        " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'";


            Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;

            Session["PKEY_MZ_YEAR"] = TextBox_MZ_YEAR.Text;

            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_REV_BASE, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_ID = "NULL";

            string old_YEAR = "NULL";

            string DATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            if (ViewState["Mode"].ToString() == "UPDATE")
            {

                old_ID = Session["PKEY_MZ_ID"].ToString();

                old_YEAR = Session["PKEY_MZ_YEAR"].ToString();
            }

            string pkey_check;

            if (old_ID == TextBox_MZ_ID.Text && old_YEAR == TextBox_MZ_YEAR.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "身分證號違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_REV_BASE.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_REV_BASE", tbox.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            tbox.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            tbox.BackColor = Color.White;
            //        }
            //    }
            //    else if (ob is DropDownList)
            //    {
            //        DropDownList dlist = (DropDownList)ob;

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_REV_BASE", dlist.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            dlist.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            dlist.BackColor = Color.White;
            //        }
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;


                cmd.Parameters.Add("MZ_YEAR", SqlDbType.VarChar).Value = TextBox_MZ_YEAR.Text.Trim();
                cmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                cmd.Parameters.Add("MZ_NAME", SqlDbType.VarChar).Value = TextBox_MZ_NAME.Text.Trim();
                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim();
                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim();
                cmd.Parameters.Add("MZ_EXAD", SqlDbType.VarChar).Value = TextBox_MZ_EXAD.Text.Trim();
                cmd.Parameters.Add("MZ_EXUNIT", SqlDbType.VarChar).Value = TextBox_MZ_EXUNIT.Text.Trim();
                cmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = TextBox_MZ_OCCC.Text.Trim();
                cmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = TextBox_MZ_TBDV.Text.Trim();
                cmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = TextBox_MZ_SRANK.Text.Trim();
                cmd.Parameters.Add("MZ_SLVC", SqlDbType.VarChar).Value = TextBox_MZ_SLVC.Text.Trim();
                cmd.Parameters.Add("MZ_SPT", SqlDbType.VarChar).Value = TextBox_MZ_SPT.Text.Trim();
                cmd.Parameters.Add("MZ_REVIEW", SqlDbType.VarChar).Value = Convert.DBNull;
                cmd.Parameters.Add("MZ_NUM", SqlDbType.VarChar).Value = TextBox_MZ_NUM.Text.Trim();
                cmd.Parameters.Add("MZ_GRADE", SqlDbType.VarChar).Value = TextBox_MZ_GRADE.Text.Trim();
                cmd.Parameters.Add("MZ_P4001", SqlDbType.Float).Value = TextBox_MZ_P4001.Text.Trim();
                cmd.Parameters.Add("MZ_P4010", SqlDbType.Float).Value = TextBox_MZ_P4010.Text.Trim();
                cmd.Parameters.Add("MZ_P4100", SqlDbType.Float).Value = TextBox_MZ_P4100.Text.Trim();
                cmd.Parameters.Add("MZ_P5001", SqlDbType.Float).Value = TextBox_MZ_P5001.Text.Trim();
                cmd.Parameters.Add("MZ_P5010", SqlDbType.Float).Value = TextBox_MZ_P5010.Text.Trim();
                cmd.Parameters.Add("MZ_P5100", SqlDbType.Float).Value = TextBox_MZ_P5100.Text.Trim();
                cmd.Parameters.Add("MZ_CODE01", SqlDbType.Float).Value = TextBox_MZ_CODE01.Text.Trim();
                cmd.Parameters.Add("MZ_CODE02", SqlDbType.Float).Value = TextBox_MZ_CODE02.Text.Trim();
                cmd.Parameters.Add("MZ_CODE18", SqlDbType.Float).Value = TextBox_MZ_CODE18.Text.Trim();
                cmd.Parameters.Add("MZ_CODE04", SqlDbType.Float).Value = TextBox_MZ_CODE04.Text.Trim();
                cmd.Parameters.Add("MZ_CODE05", SqlDbType.Float).Value = TextBox_MZ_CODE05.Text.Trim();
                cmd.Parameters.Add("MZ_CODE08", SqlDbType.Float).Value = TextBox_MZ_CODE08.Text.Trim();
                cmd.Parameters.Add("MZ_CODE19", SqlDbType.Float).Value = TextBox_MZ_CODE19.Text.Trim();
                cmd.Parameters.Add("MZ_CODE20", SqlDbType.Float).Value = TextBox_MZ_CODE20.Text.Trim();
                cmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = TextBox_MZ_POSIND.Text.Trim();
                cmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = TextBox_MZ_CHISI.Text.Trim();
                cmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = TextBox_MZ_PCHIEF.Text.Trim();
                cmd.Parameters.Add("MZ_SWT", SqlDbType.VarChar).Value = DropDownList_MZ_SWT.SelectedValue;
                cmd.Parameters.Add("MZ_MEMO", SqlDbType.VarChar).Value = TextBox_MZ_MEMO.Text.Trim();
                cmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = Convert.DBNull;

                if (ViewState["Mode"].ToString() == "INSERT")
                {
                    cmd.Parameters.Add("INSDATE", SqlDbType.VarChar).Value = DATE;
                    cmd.Parameters.Add("INSID", SqlDbType.VarChar).Value = Session["ADPMZ_ID"].ToString();
                    cmd.Parameters.Add("INSEXAD", SqlDbType.VarChar).Value = Session["ADPMZ_EXAD"].ToString();
                }

                if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    cmd.Parameters.Add("UP_DATE", SqlDbType.VarChar).Value = DATE;
                    cmd.Parameters.Add("UP_ID", SqlDbType.VarChar).Value = Session["ADPMZ_ID"].ToString();
                    cmd.Parameters.Add("UP_EXAD", SqlDbType.VarChar).Value = Session["ADPMZ_EXAD"].ToString();
                }

                try
                {
                    cmd.ExecuteNonQuery();

                    Session.Remove("PKEY_MZ_ID");
                    Session.Remove("PKEY_MZ_YEAR");

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href(''Personal_Efficiency2.aspx?MZ_ID=" + TextBox_MZ_ID.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                        REV_BASE_MZ_ID = Session["REV_BASE_MZ_ID"] as List<string>;

                        if (int.Parse(xcount.Text.Trim()) == 0 && REV_BASE_MZ_ID.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && REV_BASE_MZ_ID.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == REV_BASE_MZ_ID.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < REV_BASE_MZ_ID.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                    }
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                    ViewState.Remove("Mode");
                    A.controlEnable(ref this.Panel_REV_BASE, false);
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href(''Personal_Efficiency2.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    }
                }
                finally
                {
                    conn.Close();
                    //XX2013/06/18 
                    conn.Dispose();
                }

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_REV_BASE.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;
                        if (dl1.ID == "DropDownList1")
                        {
                            dl1.SelectedValue = "1";
                        }
                        else
                        {
                            dl1.SelectedValue = "";
                        }
                    }

                    if (dl is ComboBox)
                    {
                        ComboBox cm1 = dl as ComboBox;
                        cm1.SelectedValue = "";

                    }

                    if (dl is TextBox)
                    {
                        TextBox tbox = dl as TextBox;
                        tbox.Text = "";
                    }
                }
                btUpdate.Enabled = false;
            }
            else if (ViewState["Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));
                REV_BASE_MZ_ID = Session["POLICE_MZ_ID"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && REV_BASE_MZ_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && REV_BASE_MZ_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == REV_BASE_MZ_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < REV_BASE_MZ_ID.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btUpdate.Enabled = true;
            }

            Session.Remove("PKEY_MZ_ID");
            Session.Remove("PKEY_MZ_YEAR");
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            A.controlEnable(ref this.Panel_REV_BASE, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_REV_BASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                REV_BASE_MZ_ID = Session["REV_BASE_MZ_ID"] as List<string>;

                REV_BASE_MZ_ID.Remove(TextBox_MZ_ID.Text.Trim());

                if (REV_BASE_MZ_ID.Count == 0)
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal_Card.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                }
                else
                {
                    xcount.Text = "0";

                    finddata(int.Parse(xcount.Text));

                    if (REV_BASE_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }

                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;

                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + REV_BASE_MZ_ID.Count.ToString() + "筆";

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                }

                btInsert.Enabled = true;

                btOK.Enabled = false;

                btCancel.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch.aspx?TableName=REV_BASE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != REV_BASE_MZ_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + REV_BASE_MZ_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();

                btUpper.Enabled = true;

                finddata(int.Parse(xcount.Text));


                if (int.Parse(xcount.Text) == REV_BASE_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();

                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == REV_BASE_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + REV_BASE_MZ_ID.Count.ToString() + "筆";
        }

        protected void Can_not_empty(TextBox tb1, object obj, string fieldname)
        {
            if (string.IsNullOrEmpty(tb1.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + fieldname + "不可空白')", true);
            }
            else
            {
                if (obj is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }

        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {

            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }

            }
        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_EXAD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_EXAD1, TextBox_MZ_EXAD, TextBox_MZ_EXUNIT);
        }

        protected void TextBox_MZ_EXUNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXUNIT.Text, "25");

            Ktype_Cname_Check(CName, TextBox_MZ_EXUNIT1, TextBox_MZ_EXUNIT, TextBox_MZ_OCCC);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_TBDV);
        }

        protected void TextBox_MZ_SRANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_SRANK1, TextBox_MZ_SRANK, TextBox_MZ_CHISI);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_POSIND);
        }

        protected void TextBox_MZ_SLVC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_CHISI.Text, "23");

            Ktype_Cname_Check(CName, TextBox_MZ_CHISI1, TextBox_MZ_CHISI, TextBox_MZ_SLVC);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_EXAD);
        }

        protected void TextBox_MZ_CHISI_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_CHISI.Text, "23");

            Ktype_Cname_Check(CName, TextBox_MZ_CHISI1, TextBox_MZ_CHISI, TextBox_MZ_SLVC);
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
        }

        protected void btEXAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXAD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_EXAD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btEXUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
        }

        protected void btOCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_OCCC, TextBox_MZ_OCCC1, "26");
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void btCHISI_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_CHISI, TextBox_MZ_CHISI1, "23");
        }

        protected void btSRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SRANK, TextBox_MZ_SRANK1, "09");
        }

        protected void btSLVC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SLVC, TextBox_MZ_SLVC1, "64");
        }

        protected void CV_AD_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_EXAD_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_UNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_EXUNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_OCCC_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_TBDV_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_CHISI_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_SRANK_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_SLVC_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void btCheck_Click(object sender, EventArgs e)
        {
            int i = int.Parse(TextBox_MZ_P4001.Text) + int.Parse(TextBox_MZ_P4010.Text) * 3 + int.Parse(TextBox_MZ_P4100.Text) * 9 - int.Parse(TextBox_MZ_P5001.Text) - int.Parse(TextBox_MZ_P5010.Text) * 3 - int.Parse(TextBox_MZ_P5100.Text) * 9;
            if (i > 0)
                TextBox_MZ_MEMO.Text = "功過相抵：嘉獎共" + i + "次";
            else if (i < 0)
                TextBox_MZ_MEMO.Text = "功過相抵：申誡共" + i + "次";

            if (float.Parse(TextBox_MZ_CODE01.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，事假" + TextBox_MZ_CODE01.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE02.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，病假" + TextBox_MZ_CODE02.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE18.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，曠職" + TextBox_MZ_CODE18.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE19.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，遲到" + TextBox_MZ_CODE19.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE08.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，娩假" + TextBox_MZ_CODE08.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE04.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，婚假" + TextBox_MZ_CODE04.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE20.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，早退" + TextBox_MZ_CODE20.Text + "天";
            }
            if (float.Parse(TextBox_MZ_CODE05.Text) > 0)
            {
                TextBox_MZ_MEMO.Text += "，事假" + TextBox_MZ_CODE05.Text + "天";
            }

            TextBox_MZ_MEMO.Text += "。";
        }

        protected void TextBox_MZ_NUM_TextChanged(object sender, EventArgs e)
        {
            if (int.Parse(TextBox_MZ_NUM.Text.Trim()) >= 80)
            {
                TextBox_MZ_GRADE.Text = "甲";
            }
            else if (int.Parse(TextBox_MZ_NUM.Text.Trim()) < 80 && int.Parse(TextBox_MZ_NUM.Text.Trim()) >= 70)
            {
                TextBox_MZ_GRADE.Text = "乙";
            }
            else if (int.Parse(TextBox_MZ_NUM.Text.Trim()) < 70 && int.Parse(TextBox_MZ_NUM.Text.Trim()) >= 60)
            {
                TextBox_MZ_GRADE.Text = "丙";
            }
            else
            {
                TextBox_MZ_GRADE.Text = "丁";
            }

        }

        protected void btPCHIEF_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PCHIEF, TextBox_MZ_PCHIEF1, "56");
        }

        protected void TextBox_MZ_PCHIEF_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PCHIEF.Text, "56");

            Ktype_Cname_Check(CName, TextBox_MZ_PCHIEF1, TextBox_MZ_PCHIEF, DropDownList_MZ_SWT);
        }
    }
}
