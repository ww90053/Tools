using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.App_Code;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public partial class Personal2_2 : System.Web.UI.Page
    {
        List<string> PRK1_MZ_PRID1 = new List<string>();
        List<String> PRK1_MZ_PRID = new List<string>();
        string A_strGID = "";
        string PRK1_PRID = "";
        string PRK1_PRID1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                A.check_power();

                A.fill_AD_POST_BOSS(DropDownList_MZ_CHKAD, 2);

            }
            //取得網址回傳參數
            try
            {

                GridView1.Visible = true;

                GridView2.Visible = false;

                A.set_Panel_EnterToTAB(ref this.Panel_PRK1);

                TextBox_MZ_EXPLAIN.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 13;");

                ViewState["XCOUNT"] = Request["XCOUNT"];


            }
            catch (Exception ex)
            {

                try
                {
                    string error = "";
                    error += "方法: Page_Load(SESSION帶值): " + Session["ADPMZ_ID"].ToString() + "  原因: " + ex.Message;
                    Log.SaveLog("B_Personal2-2.aspx", "1", error);
                }

                catch { }

            }
            //取得網址回傳參數


            //導入資料
            try
            {
                if (!IsPostBack)
                {


                    DropDownList_MZ_EXPLAIN0.SelectedValue = "受文機關及各受令人";
                    TextBox_MZ_EXPLAIN1.Text = "新北市政府警察局人事室";


                    //導入第幾筆資料但似乎沒看到在使用XCOUNT的地方
                    if (ViewState["XCOUNT"] != null)
                    {
                        finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                        xcount.Text = ViewState["XCOUNT"].ToString();
                        if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && PRK1_MZ_PRID1.Count > 1)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = false;
                        }
                        else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < PRK1_MZ_PRID1.Count - 1)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                        else if (int.Parse(ViewState["XCOUNT"].ToString()) == PRK1_MZ_PRID1.Count - 1)
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
                        btDelete.Enabled = false;
                    }
                    //導入第幾筆資料但似乎沒看到在使用XCOUNT的地方


                    PRK1_PRID = HttpUtility.UrlDecode(Request["PRK1_PRID"]);

                    PRK1_PRID1 = Request["PRK1_PRID1"];

                    //查詢後會進入
                    if (!string.IsNullOrEmpty(PRK1_PRID))
                    {


                        string strSQL = "SELECT MZ_PRID,MZ_PRID1 FROM A_PRK1 WHERE 1=1";

                        //20141231
                        if (PRK1_PRID == "新北警人")
                        {
                            strSQL += " AND (MZ_PRID='" + PRK1_PRID + "' OR  MZ_PRID='北警人')";
                        }
                        else
                        {
                            strSQL += " AND MZ_PRID='" + PRK1_PRID + "'";
                        }


                        if (!string.IsNullOrEmpty(PRK1_PRID1))
                        {
                            strSQL += " AND MZ_PRID1='" + PRK1_PRID1 + "'";
                        }


                        strSQL += " ORDER BY MZ_PRID,MZ_PRID1";

                        PRK1_MZ_PRID1 = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID1");
                        PRK1_MZ_PRID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID");

                        Session["PRK1_MZ_PRID1"] = PRK1_MZ_PRID1;
                        Session["PRK1_MZ_PRID"] = PRK1_MZ_PRID;


                        if (PRK1_MZ_PRID.Count == 0)
                        {
                            Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal2-2.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                        }
                        else if (PRK1_MZ_PRID.Count == 1)
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
                    }


                    A.controlEnable(ref this.Panel_PRK1, false);
                    chk_TPMGroup();
                }

            }

            catch (Exception ex)
            {

                try
                {
                    string error = "";
                    error += "方法: if (!IsPostBack): " + Session["ADPMZ_ID"].ToString() + "  原因: " + ex.Message;
                    Log.SaveLog("B_Personal2-2.aspx", "1", error);
                }

                catch { }
            }

            //導入資料



            try
            {
                if (ViewState["Mode"] != null)
                {
                    if (ViewState["Mode"].ToString() == "INSERT" && int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text + "'")) > 0)
                    {
                        ViewState.Remove("GV2_DB");

                        GridView2.Visible = false;

                        GridView1.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {

                try
                {
                    string error = "";
                    error += "方法: if (ViewState[\"Mode\"] != null): " + Session["ADPMZ_ID"].ToString() + "  原因: " + ex.Message;
                    Log.SaveLog("B_Personal2-2.aspx", "1", error);
                }

                catch { }


            }


            try
            {
                //如果有執行謄稿動作
                if (ViewState["GV2_DB"] != null)
                {
                    GridView2.DataSource = ViewState["GV2_DB"] as DataTable;

                    GridView2.AllowPaging = true;

                    GridView2.PageSize = 6;

                    GridView2.DataBind();

                    TextBox6.Text = (ViewState["GV2_DB"] as DataTable).Rows.Count.ToString();

                    GridView2.Visible = true;

                    GridView1.Visible = false;
                }
                else
                {

                    Session.Remove("PRK1_TEMP_DT2");

                    GridView2.Visible = false;

                    GridView1.Visible = true;
                }
            }

            catch (Exception ex)
            {


                try
                {
                    string error = "";
                    error += "方法: if (ViewState[\"Mode\"] != null): " + Session["ADPMZ_ID"].ToString() + "  原因: " + ex.Message;
                    Log.SaveLog("B_Personal2-2.aspx", "1", error);
                }

                catch { }


            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (A_strGID)
            {
                case "A":
                case "B":

                    break;
                case "C":
                    DropDownList_MZ_CHKAD.Enabled = false;
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void COUNT_GV_ROWS(string PRID, string PRID1)
        {
            TextBox6.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(1) FROM A_PRK2 WHERE MZ_PRID='" + PRID + "' AND MZ_PRID1='" + PRID1 + "'");
        }

        protected void finddata(int Datacount)
        {
            try
            {
                PRK1_MZ_PRID = Session["PRK1_MZ_PRID"] as List<string>;
                PRK1_MZ_PRID1 = Session["PRK1_MZ_PRID1"] as List<string>;

                string Findsql = "";

                if (PRK1_MZ_PRID[Datacount].ToString().Trim() == "新北警人")
                {
                    Findsql = "SELECT * FROM A_PRK1 WHERE MZ_PRID1='" + PRK1_MZ_PRID1[Datacount].ToString() + "' AND (MZ_PRID='" + PRK1_MZ_PRID[Datacount].ToString().Trim() + "' OR  MZ_PRID='北警人' )";

                }
                else
                {

                    Findsql = "SELECT * FROM A_PRK1 WHERE MZ_PRID1='" + PRK1_MZ_PRID1[Datacount].ToString() + "' AND MZ_PRID='" + PRK1_MZ_PRID[Datacount].ToString().Trim() + "'";
                }
                DataTable Finddt = o_DBFactory.ABC_toTest.Create_Table(Findsql, "123");
                if (Finddt.Rows.Count == 1)
                {
                    DropDownList_MZ_CHKAD.SelectedValue = Finddt.Rows[0]["MZ_CHKAD"].ToString();
                    TextBox_MZ_EXPLAIN.Text = Finddt.Rows[0]["MZ_EXPLAIN"].ToString();
                    DropDownList_MZ_EXPLAIN0.Text = Finddt.Rows[0]["MZ_EXPLAIN0"].ToString();
                    TextBox_MZ_EXPLAIN1.Text = Finddt.Rows[0]["MZ_EXPLAIN1"].ToString();
                    TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_DATE"].ToString());
                    TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_IDATE"].ToString());
                    TextBox_MZ_PRID.Text = Finddt.Rows[0]["MZ_PRID"].ToString();
                    TextBox_MZ_PRID1.Text = Finddt.Rows[0]["MZ_PRID1"].ToString();
                    TextBox_MZ_YEARUSE.Text = Finddt.Rows[0]["MZ_YEARUSE"].ToString();
                    TextBox_SPEED_NO.Text = Finddt.Rows[0]["SPEED_NO"].ToString();
                    TextBox_MZ_FILENO.Text = Finddt.Rows[0]["MZ_FILENO"].ToString();
                    TextBox_PWD_NO.Text = Finddt.Rows[0]["PWD_NO"].ToString();
                    COUNT_GV_ROWS(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
                }

                gridview_bind(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);


                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }

            catch (Exception ex)
            {


                try
                {
                    string error = "";
                    error += "方法: finddata() " + Session["ADPMZ_ID"].ToString() + "  原因: " + ex.Message;
                    Log.SaveLog("B_Personal2-2.aspx", "1", error);
                }

                catch { }


            }
        }


        //修改找尋方式  --
        //20141231
        //protected void finddata(string MZ_PRID, string MZ_PRID1)
        //{
        //    try
        //    {
        //        string Findsql ="";
        //        if (MZ_PRID.Trim() == "新北警人")
        //        {
        //            Findsql = "SELECT * FROM A_PRK1 WHERE MZ_PRID1='" + MZ_PRID1 + "' AND (MZ_PRID='" + MZ_PRID + "' OR  MZ_PRID='北警人' )";
        //        }
        //        else
        //        {
        //            Findsql = "SELECT * FROM A_PRK1 WHERE MZ_PRID1='" + MZ_PRID1 + "' AND MZ_PRID='" + MZ_PRID + "'";
        //        }

        //        DataTable Finddt = o_DBFactory.ABC_toTest.Create_Table(Findsql, "123");
        //        if (Finddt.Rows.Count == 1)
        //        {
        //            DropDownList_MZ_CHKAD.SelectedValue = Finddt.Rows[0]["MZ_CHKAD"].ToString();
        //            TextBox_MZ_EXPLAIN.Text = Finddt.Rows[0]["MZ_EXPLAIN"].ToString();
        //            DropDownList_MZ_EXPLAIN0.Text = Finddt.Rows[0]["MZ_EXPLAIN0"].ToString();
        //            TextBox_MZ_EXPLAIN1.Text = Finddt.Rows[0]["MZ_EXPLAIN1"].ToString();
        //            TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_DATE"].ToString());
        //            TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_IDATE"].ToString());
        //            TextBox_MZ_PRID.Text = Finddt.Rows[0]["MZ_PRID"].ToString();
        //            TextBox_MZ_PRID1.Text = Finddt.Rows[0]["MZ_PRID1"].ToString();
        //            TextBox_MZ_YEARUSE.Text = Finddt.Rows[0]["MZ_YEARUSE"].ToString();
        //            TextBox_SPEED_NO.Text = Finddt.Rows[0]["SPEED_NO"].ToString();
        //            TextBox_MZ_FILENO.Text = Finddt.Rows[0]["MZ_FILENO"].ToString();
        //            TextBox_PWD_NO.Text = Finddt.Rows[0]["PWD_NO"].ToString();
        //            COUNT_GV_ROWS(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
        //        }

        //        gridview_bind(MZ_PRID, MZ_PRID1);


        //        btUpdate.Enabled = true;
        //        btDelete.Enabled = true;
        //    }

        //    catch (Exception ex)
        //    {


        //        try
        //        {
        //            string error = "";
        //            error += "方法: finddata() " + Session["ADPMZ_ID"].ToString() + "  原因: " + ex.Message;
        //            Log.SaveLog("B_Personal2-2.aspx", "1", error);
        //        }

        //        catch { }


        //    }
        //}


        protected void btInsert_Click(object sender, EventArgs e)
        {

            foreach (object dl in Panel_PRK1.Controls)
            {
                if (dl is DropDownList)
                {
                    DropDownList dl1 = dl as DropDownList;
                    dl1.SelectedValue = "";
                }

                if (dl is TextBox)
                {
                    TextBox tbox = dl as TextBox;
                    tbox.Text = "";
                }
            }

            if (Session["ADPMZ_EXAD"] != null)
            {
                DropDownList_MZ_CHKAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                DataTable dt = new DataTable();

                string selectSQL = "SELECT * FROM A_CHKAD WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'";

                dt = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GET");

                if (dt.Rows.Count == 1)
                {
                    TextBox_MZ_PRID.Text = dt.Rows[0]["MZ_PRID"].ToString();
                    TextBox_MZ_YEARUSE.Text = dt.Rows[0]["MZ_YEARUSE"].ToString();
                    TextBox_SPEED_NO.Text = dt.Rows[0]["SPEED_NO"].ToString();
                    TextBox_MZ_FILENO.Text = dt.Rows[0]["MZ_FILENO"].ToString();
                    //特殊:檔號格式 112/020411/1 ,最前面是年度,可能年度會有錯誤
                    //EX: 2023年 112年,如果出現  111/...... 代表有錯,需要重帶
                    string FILENO = TextBox_MZ_FILENO.Text;
                    if (FILENO.Length > 5 /*夠長*/
                        && FILENO.Substring(3, 1) == "/" /*有那個"/" */
                        && FILENO.Substring(0, 3) != (DateTime.Now.Year - 1911).ToString()) /*前三碼年度不符合*/
                    {
                        //把前三碼改成今年的
                        TextBox_MZ_FILENO.Text = (DateTime.Now.Year - 1911).ToString() + "/" + FILENO.Substring(4);
                    }
                    TextBox_PWD_NO.Text = dt.Rows[0]["PWD_NO"].ToString();
                }
            }

            ViewState["Mode"] = "INSERT";

            ViewState["CMDSQL"] = "INSERT INTO A_PRK1(MZ_DATE,MZ_PRID,MZ_PRID1,MZ_CHKAD,MZ_EXPLAIN,MZ_EXPLAIN0," +
                                             "  MZ_EXPLAIN1,MZ_SWT2,MZ_CLASS1,MZ_CLASS2,MZ_IDATE,MUSER,MDATE,SPEED_NO,PWD_NO,MZ_FILENO,MZ_YEARUSE)" +
                                     "　values(@MZ_DATE,@MZ_PRID,@MZ_PRID1,@MZ_CHKAD,@MZ_EXPLAIN,@MZ_EXPLAIN0," +
                                            "  @MZ_EXPLAIN1,@MZ_SWT2,@MZ_CLASS1,@MZ_CLASS2,@MZ_IDATE,@MUSER,@MDATE,@SPEED_NO,@PWD_NO,@MZ_FILENO,@MZ_YEARUSE)";

            DropDownList_MZ_EXPLAIN0.SelectedValue = "受文機關及各受令人";

            TextBox_MZ_EXPLAIN1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT CC_NO FROM A_CHKAD WHERE MZ_AD='" + DropDownList_MZ_CHKAD.SelectedValue + "'");

            Button_IDSearch.Enabled = true;
            TextBox_MZ_PRID1.Enabled = true;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            ViewState.Remove("GV2_DB");

            Session.Remove("PRK1_TEMP_DT2");

            GridView2.DataSource = null;
            GridView2.DataBind();

            GridView1.DataSource = null;
            GridView1.DataBind();

            A.controlEnable(ref this.Panel_PRK1, true);
            chk_TPMGroup();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRID1.ClientID + "').focus();$get('" + TextBox_MZ_PRID1.ClientID + "').focus();", true);
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal2-2_Serch.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=380,height=200,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);


        }

        protected void btDelete_Click(object sender, EventArgs e)
        {


            //刪除動作:
            //找出PRK1文號 透過文號找出PRK2案號
            //PRK1 PRK2刪除文號
            //PRKB 將案號改為與PEK2相同者 SWT1改為N
            if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_SWT FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'") == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已傳檔不可刪除');", true);
                return;
            }

            string DeleteString = "DELETE FROM A_PRK1 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";

            string DelectPRK2String = "DELETE FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'"; ;

            string strSQL = "SELECT DISTINCT A_PRK2.MZ_NO FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";


            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSON" + tempDT.Rows[0]["MZ_NO"].ToString(), TPMPermissions._boolTPM(), strSQL) == "N")
                {
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSON" + tempDT.Rows[0]["MZ_NO"].ToString(), strSQL);
                }

                string UpdatePOSITString = string.Format("UPDATE A_PRKB SET MZ_SWT1='N' WHERE A_PRKB.MZ_NO in ({0})", strSQL);



                try
                {

                    o_DBFactory.ABC_toTest.Edit_Data(UpdatePOSITString);
                }
                catch
                {

                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSONU", TPMPermissions._boolTPM(), UpdatePOSITString) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSONU", DeleteString);
                    }


                }


                //SqlCommand cmd3 = new SqlCommand(SB_Update.ToString(), conn);

                //cmd3.Transaction = oraTran;

                //cmd3.ExecuteNonQuery();



                //}
                //2013/11/04
                conn.Open();
                //2013/12/13
                //SqlTransaction oraTran = conn.BeginTransaction();




                SqlCommand cmd1 = new SqlCommand(DeleteString, conn);
                //cmd1.Transaction = oraTran;

                SqlCommand cmd2 = new SqlCommand(DelectPRK2String, conn);
                //cmd2.Transaction = oraTran;

                try
                {
                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch
                    {
                        if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSON221", TPMPermissions._boolTPM(), DeleteString) == "N")
                        {
                            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSON221", DeleteString);
                        }
                    }

                    try
                    {
                        cmd2.ExecuteNonQuery();
                    }
                    catch
                    {
                        if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSON222", TPMPermissions._boolTPM(), DelectPRK2String) == "N")
                        {
                            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "PERSON222", DelectPRK2String);
                        }
                    }


                    if (!string.IsNullOrEmpty(xcount.Text))
                    {
                        PRK1_MZ_PRID = Session["PRK1_MZ_PRID"] as List<string>;
                        PRK1_MZ_PRID1 = Session["PRK1_MZ_PRID1"] as List<string>;
                        PRK1_MZ_PRID.RemoveAt(int.Parse(xcount.Text.Trim()));
                        PRK1_MZ_PRID1.RemoveAt(int.Parse(xcount.Text.Trim()));
                    }

                    if (PRK1_MZ_PRID.Count == 0)
                    {
                        btUpdate.Enabled = false;
                        btDelete.Enabled = false;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal2-2.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd1));
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd2));
                    }
                    else
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        if (PRK1_MZ_PRID.Count > 1)
                        {
                            btNEXT.Enabled = true;
                        }
                        btUpdate.Enabled = true;
                        btDelete.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    }

                    //oraTran.Commit();
                    Button_IDSearch.Enabled = false;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;

                }
                catch
                {
                    //oraTran.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
                finally
                {
                    conn.Close();

                    //XX2013/06/18 

                    conn.Dispose();
                }
            }


        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_SWT FROM A_PRK2 WHERE MZ_PRID='" + o_str.tosql(TextBox_MZ_PRID.Text.Trim()) + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'") == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已傳檔不可修改');", true);
                return;
            }

            ViewState["Mode"] = "UPDATE";


            ViewState["CMDSQL"] = "UPDATE A_PRK1 SET  MZ_DATE=@MZ_DATE,MZ_PRID=@MZ_PRID,MZ_PRID1=@MZ_PRID1," +
                                                      "  MZ_CHKAD=@MZ_CHKAD,MZ_EXPLAIN=@MZ_EXPLAIN,MZ_EXPLAIN0=@MZ_EXPLAIN0," +
                                                      "  MZ_EXPLAIN1=@MZ_EXPLAIN1,MZ_SWT2=@MZ_SWT2,MZ_CLASS1=@MZ_CLASS1," +
                                                      "  MZ_CLASS2=@MZ_CLASS2,MZ_IDATE=@MZ_IDATE,MUSER=@MUSER,MDATE=@MDATE, " +
                                                      "  SPEED_NO=@SPEED_NO,PWD_NO=@PWD_NO,MZ_FILENO=@MZ_FILENO,MZ_YEARUSE=@MZ_YEARUSE " +
                                                      "  WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() +
                                                      "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";

            Session["PKEY_MZ_PRID"] = TextBox_MZ_PRID.Text;

            Session["PKEY_MZ_PRID1"] = TextBox_MZ_PRID1.Text;

            TextBox_MZ_PRID1.Enabled = false;
            Button_IDSearch.Enabled = true;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_PRK1, true);

            gridview_bind(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
            chk_TPMGroup();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRID1.ClientID + "').focus();$get('" + TextBox_MZ_PRID1.ClientID + "').focus();", true);
        }

        protected void Button_IDSearch_Click(object sender, EventArgs e)
        {
            //謄稿作業完成後 BT_GV1 會Click
            Session["PRK1_BT"] = BT_GV1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalPRK1IDSearch.aspx?MZ_PRID=" + o_str.tosql(TextBox_MZ_PRID.Text.Trim()) + "&MZ_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&MZ_CHKAD=" + DropDownList_MZ_CHKAD.SelectedValue.Trim() + "&MZ_DATE=" + o_str.tosql(TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0')) + "&MZ_IDATE=" + o_str.tosql(TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0')) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=450,height=200,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != PRK1_MZ_PRID.Count - 1)
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
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PRK1_MZ_PRID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PRK1_MZ_PRID.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            #region 驗證
            string ErrorString = "";

            string old_PRID = "NULL";

            string old_PRID1 = "NULL";

            string MDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                old_PRID = Session["PKEY_MZ_PRID"].ToString();

                old_PRID1 = Session["PKEY_MZ_PRID1"].ToString();
            }

            #region 文號

            if (string.IsNullOrEmpty(TextBox_MZ_PRID.Text) || string.IsNullOrEmpty(TextBox_MZ_PRID1.Text))
            {
                ErrorString += "請輸入發文文號" + "\\r\\n";
                TextBox_MZ_PRID.BackColor = Color.Orange;
                TextBox_MZ_EXPLAIN1.BackColor = Color.Orange;
            }
            else
            {
                string pkey_check;

                if (old_PRID == TextBox_MZ_PRID.Text && old_PRID1 == TextBox_MZ_PRID1.Text && ViewState["Mode"].ToString() == "UPDATE")
                    pkey_check = "0";
                else
                    pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK1 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'");

                if (pkey_check != "0")
                {
                    ErrorString += "發文文號與發文字號違反唯一值的條件" + "\\r\\n";
                    TextBox_MZ_PRID.BackColor = Color.Orange;
                    TextBox_MZ_EXPLAIN1.BackColor = Color.Orange;
                }
                else
                {
                    TextBox_MZ_PRID.BackColor = Color.White;
                    TextBox_MZ_EXPLAIN1.BackColor = Color.White;
                }
            }
            #endregion 文號

            #region 發文日期
            if (string.IsNullOrEmpty(TextBox_MZ_DATE.Text))
            {
                ErrorString += "請輸入發文日期" + "\\r\\n";
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                bool check_MZ_DATE = DateManange.check_Date(TextBox_MZ_DATE.Text.Replace("/", "").PadLeft(7, '0'));


                if (!check_MZ_DATE)
                {
                    ErrorString += "發文日期日期有誤" + "\\r\\n";
                    TextBox_MZ_DATE.BackColor = Color.Orange;
                }
                else
                {
                    TextBox_MZ_DATE.BackColor = Color.White;
                }

            }
            #endregion 發文日期

            #region 生效日期
            if (string.IsNullOrEmpty(TextBox_MZ_IDATE.Text))
            {
                ErrorString += "請輸入生效日期" + "\\r\\n";
                TextBox_MZ_IDATE.BackColor = Color.Orange;
            }
            else
            {
                bool check_MZ_IDATE = DateManange.check_Date(TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));


                if (!check_MZ_IDATE)
                {
                    ErrorString += "生效日期日期有誤" + "\\r\\n";
                    TextBox_MZ_IDATE.BackColor = Color.Orange;
                }
                else
                {
                    TextBox_MZ_IDATE.BackColor = Color.White;
                }
            }
            #endregion 生效日期

            #region 20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料

            //foreach (Object ob in Panel_PRK1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_PRK1", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_PRK1", dlist.Text);

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

            #endregion

            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            #endregion 驗證

            DataTable tempDT1 = new DataTable();

            tempDT1 = Session["PRK1_tempDT"] as DataTable;

            if (GridView1.Rows.Count == 0 && tempDT1.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先謄稿');", true);
                return;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    conn.Open();
                    //2013/12/13
                    // SqlTransaction oraTran = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);

                    //cmd.Transaction = oraTran;
                    cmd.Parameters.Add("MZ_PRID", SqlDbType.VarChar).Value = TextBox_MZ_PRID.Text.Trim();
                    cmd.Parameters.Add("MZ_PRID1", SqlDbType.VarChar).Value = TextBox_MZ_PRID1.Text.Trim();
                    cmd.Parameters.Add("MZ_CHKAD", SqlDbType.VarChar).Value = DropDownList_MZ_CHKAD.SelectedValue.Trim();
                    cmd.Parameters.Add("MZ_EXPLAIN", SqlDbType.VarChar).Value = TextBox_MZ_EXPLAIN.Text.Trim();
                    cmd.Parameters.Add("MZ_EXPLAIN0", SqlDbType.VarChar).Value = DropDownList_MZ_EXPLAIN0.SelectedValue.Trim();
                    cmd.Parameters.Add("MZ_EXPLAIN1", SqlDbType.VarChar).Value = TextBox_MZ_EXPLAIN1.Text.Trim();
                    cmd.Parameters.Add("MZ_SWT2", SqlDbType.VarChar).Value = DBNull.Value; //TextBox_MZ_SWT2.Text;
                    cmd.Parameters.Add("MZ_CLASS1", SqlDbType.VarChar).Value = DBNull.Value; //TextBox_MZ_CLASS1.Text;
                    cmd.Parameters.Add("MZ_CLASS2", SqlDbType.VarChar).Value = DBNull.Value; //TextBox_MZ_CLASS2.Text;
                    cmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    cmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    cmd.Parameters.Add("MUSER", SqlDbType.VarChar).Value = Session["ADPMZ_ID"].ToString();
                    cmd.Parameters.Add("MDATE", SqlDbType.VarChar).Value = MDATE;
                    cmd.Parameters.Add("SPEED_NO", SqlDbType.VarChar).Value = TextBox_SPEED_NO.Text.Trim();
                    cmd.Parameters.Add("PWD_NO", SqlDbType.VarChar).Value = TextBox_PWD_NO.Text.Trim();
                    cmd.Parameters.Add("MZ_FILENO", SqlDbType.VarChar).Value = TextBox_MZ_FILENO.Text.Trim();
                    cmd.Parameters.Add("MZ_YEARUSE", SqlDbType.VarChar).Value = TextBox_MZ_YEARUSE.Text.Trim();

                    string errorMsg = "";
                    try
                    {
                        #region 20210420 搬移至上方先行判斷

                        //DataTable tempDT1 = new DataTable();

                        //var ww = Session["PRK1_tempDT"];
                        //tempDT1 = Session["PRK1_tempDT"] as DataTable;

                        //if (GridView1.Rows.Count == 0 && tempDT1.Rows.Count == 0)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先謄稿');", true);
                        //    return;
                        //}
                        //else
                        //{
                        //    cmd.ExecuteNonQuery();
                        //}

                        #endregion

                        cmd.ExecuteNonQuery();

                        //因只能註冊一次Script 修改註冊位置 20180522 by sky
                        if (ViewState["Mode"].ToString() == "INSERT")
                        {
                            //2010.06.04 LOG紀錄 by伊珊
                            TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                        }
                        else if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            //2010.06.04 LOG紀錄 by伊珊
                            TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                            string UpdateString = "UPDATE A_PRK2 SET MZ_DATE='" + TextBox_MZ_DATE.Text.Replace("/", "").PadLeft(7, '0') + "',MZ_IDATE='" + TextBox_MZ_IDATE.Text.Replace("/", "").PadLeft(7, '0') + "' WHERE MZ_PRID='" + TextBox_MZ_PRID.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text + "'";

                            o_DBFactory.ABC_toTest.Edit_Data(UpdateString);

                            PRK1_MZ_PRID1 = Session["PRK1_MZ_PRID1"] as List<string>;
                            if (int.Parse(xcount.Text.Trim()) == 0 && PRK1_MZ_PRID1.Count == 1)
                            {
                                btUpper.Enabled = false;
                                btNEXT.Enabled = false;
                            }
                            else if (int.Parse(xcount.Text.Trim()) == 0 && PRK1_MZ_PRID1.Count > 1)
                            {
                                btUpper.Enabled = false;
                                btNEXT.Enabled = true;
                            }
                            else if (int.Parse(xcount.Text.Trim()) + 1 == PRK1_MZ_PRID1.Count)
                            {
                                btUpper.Enabled = true;
                                btNEXT.Enabled = false;
                            }
                            else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PRK1_MZ_PRID1.Count)
                            {
                                btNEXT.Enabled = true;
                                btUpper.Enabled = true;
                            }
                        }

                        //string updatePRK2 = "UPDATE A_PRK2 SET MZ_DATE='" + TextBox_MZ_DATE.Text.Trim().Replace("/", "") + "' AND MZ_IDATE='" + TextBox_MZ_IDATE.Text.Trim().Replace("/", "") + "' WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";

                        if (Session["PRK1_tempDT"] != null)
                        {
                            if (tempDT1.Rows.Count > 0)
                            {
                                DataTable repeatDt = new DataTable();
                                repeatDt.Columns.Add("身分證", typeof(string));
                                repeatDt.Columns.Add("獎懲類別", typeof(string));
                                repeatDt.Columns.Add("獎懲內容", typeof(string));
                                repeatDt.Columns.Add("案號", typeof(string));
                                repeatDt.Columns.Add("發文日期", typeof(string));

                                string strPartSQL = string.Format(@" MZ_NO=@MZ_NO AND MZ_PRCT=@MZ_PRCT AND MZ_ID=@MZ_ID AND ROWNUM=1");
                                string MZ_DATE = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
                                string MZ_IDATE = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_IDATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                                //將已發布的資料已謄稿狀態變更
                                string UpdateSWT1sql = "UPDATE A_PRKB SET MZ_SWT1='Y' WHERE " + strPartSQL;
                                string strSQL = string.Format(@"INSERT INTO A_PRK2(MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK, 
                                                                                    MZ_TBDV,MZ_PRCT,MZ_PRK, MZ_POLK,
                                                                                    MZ_PRRST,MZ_PRK1,MZ_MEMO,MZ_PROLNO, MZ_PCODE, 
                                                                                    MZ_SWT3,MZ_REMARK,MZ_EXAD,MZ_EXUNIT,
                                                                                    MZ_PRID,MZ_PRID1,MZ_CHKAD,MZ_DATE,MZ_IDATE,MUSER,MDATE)
                                                                            SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK,
                                                                                    MZ_TBDV,MZ_PRCT,MZ_PRK, case when MZ_POLK   is null  then 'H' else MZ_POLK end MZ_POLK,
                                                                                    MZ_PRRST,MZ_PRK1,MZ_MEMO,MZ_PROLNO, case when MZ_PCODE   is null  then '9' else MZ_PCODE end MZ_PCODE,
                                                                                    MZ_SWT3,MZ_REMARK,MZ_EXAD,MZ_EXUNIT,'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' 
                                                                            FROM A_PRKB WHERE {7} "
                                                                   , TextBox_MZ_PRID.Text.Trim()
                                                                   , TextBox_MZ_PRID1.Text.Trim()
                                                                   , DropDownList_MZ_CHKAD.SelectedValue
                                                                   , MZ_DATE
                                                                   , MZ_IDATE
                                                                   , Session["ADPMZ_ID"].ToString()
                                                                   , MDATE
                                                                   , strPartSQL);

                                for (int i = 0; i < tempDT1.Rows.Count; i++)
                                {
                                    //新增重複資料檢核 20180329 by sky
                                    //1.檢查A_PRK2身分證、獎懲類別、獎懲內容、發文日期欄位
                                    //2.匯出A_PRKB身分證、獎懲類別、獎懲內容、案號
                                    //修改為僅檢查A_PRK2身分證、獎懲內容 20191001 by sky
                                    string mzdate = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
                                    A_PRK2_Model a_PRK2_Model = new A_PRK2_Model()
                                    {
                                        MZ_ID = tempDT1.Rows[i]["MZ_ID"].ToString(),
                                        //MZ_PRK = tempDT1.Rows[i]["MZ_PRK"].ToString(),
                                        MZ_PRCT = tempDT1.Rows[i]["MZ_PRCT"].ToString(),
                                        //MZ_DATE = mzdate
                                    };
                                    if (!new PersonalService().DuplicateCheckFor_APRK2(a_PRK2_Model))
                                    {
                                        DataRow dr = repeatDt.NewRow();
                                        dr["身分證"] = tempDT1.Rows[i]["MZ_ID"].ToString();
                                        dr["獎懲類別"] = tempDT1.Rows[i]["MZ_PRK"].ToString();
                                        dr["獎懲內容"] = tempDT1.Rows[i]["MZ_PRCT"].ToString();
                                        dr["案號"] = tempDT1.Rows[i]["MZ_NO"].ToString();
                                        dr["發文日期"] = mzdate;
                                        repeatDt.Rows.Add(dr);

                                        //匯出excel有異常 暫時用alert替代 20180605 by sky
                                        //errorMsg += "身分證:" + tempDT1.Rows[i]["MZ_ID"].ToString() + "，發文日期:" + mzdate + "重複資料\n";
                                        continue;
                                    }

                                    List<SqlParameter> parameters = new List<SqlParameter>()
                                    {
                                        new SqlParameter("MZ_NO", SqlDbType.NVarChar) { Value = tempDT1.Rows[i]["MZ_NO"].ToString() },
                                        new SqlParameter("MZ_PRCT", SqlDbType.NVarChar) { Value = tempDT1.Rows[i]["MZ_PRCT"].ToString() },
                                        new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = tempDT1.Rows[i]["MZ_ID"].ToString() }
                                    };

                                    if (o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                                    {
                                        o_DBFactory.ABC_toTest.DealCommandLog(UpdateSWT1sql, parameters);
                                    }

                                    ViewState.Remove("GV2_DB");

                                    parameters.Clear();
                                }

                                //判斷是否要匯出重複Excel檔案
                                if (repeatDt.Rows.Count > 0)
                                {
                                    Session["msData"] = new OfficeHelpers.ExcelHelpers().RenderDataTable_ToExcelFileStream(repeatDt, true);
                                    Session["fileName"] = "獎懲重複資料";
                                    errorMsg = "alert('請依據回傳Excel檔案處理重複資料。');window.open('ExcelDowload.ashx');";
                                }
                            }
                        }
                        //oraTran.Commit();

                        GridView1.Visible = true;

                        GridView2.Visible = false;
                        gridview_bind(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
                        // GridView1.DataBind();

                        Session.Remove("PKEY_MZ_PRID");

                        Session.Remove("PKEY_MZ_PRID1");

                        btUpdate.Enabled = true;
                        btInsert.Enabled = true;
                        btOK.Enabled = false;
                        btCancel.Enabled = false;
                        btDelete.Enabled = true;
                        Button_IDSearch.Enabled = false;
                        COUNT_GV_ROWS(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
                        A.controlEnable(ref this.Panel_PRK1, false);
                    }
                    catch (Exception ex)
                    {
                        if (ViewState["Mode"].ToString() == "INSERT")
                        {
                            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                            errorMsg = string.Format("alert('新增失敗 {0}');", errorMsg + ex.Message.Replace("\n", "").Replace("\n\r", ""));
                        }
                        else if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal2-2.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                            errorMsg = string.Format("alert('修改失敗 {0}');location.href('Personal2-2.aspx?XCOUNT={1}&TPM_FION={2}');", errorMsg + ex.Message.Replace("\n", "").Replace("\n\r", ""), xcount.Text.Trim(), Request.QueryString["TPM_FION"]);
                        }

                        //oraTran.Rollback();
                    }
                    finally
                    {
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            if (ViewState["Mode"].ToString() == "INSERT")
                            {
                                errorMsg = "alert('新增成功');";
                            }
                            else if (ViewState["Mode"].ToString() == "UPDATE")
                            {
                                errorMsg = "alert('修改成功');";
                            }
                        }

                        conn.Close();

                        //XX2013/06/18 
                        cmd.Dispose();
                        conn.Dispose();
                        ViewState.Remove("Mode");

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", errorMsg, true);
                    }
                }
            }



        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_PRK1.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;
                        dl1.SelectedValue = "";
                    }

                    if (dl is TextBox)
                    {
                        TextBox tbox = dl as TextBox;
                        tbox.Text = "";
                    }
                }
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
            }
            else if (ViewState["Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                PRK1_MZ_PRID1 = Session["PRK1_MZ_PRID1"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && PRK1_MZ_PRID1.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && PRK1_MZ_PRID1.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == PRK1_MZ_PRID1.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PRK1_MZ_PRID1.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btDelete.Enabled = true;
                btUpdate.Enabled = true;
            }
            Button_IDSearch.Enabled = false;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;

            GridView2.DataSource = null;
            GridView2.DataBind();

            GridView1.DataSource = null;
            GridView1.DataBind();

            Session.Remove("PKEY_MZ_PRID");
            Session.Remove("PKEY_MZ_PRID1");
            ViewState.Remove("GV2_DB");
            //gridview_bind(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text); 
            COUNT_GV_ROWS(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
            A.controlEnable(ref this.Panel_PRK1, false);
        }

        protected void btPRCT1_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXPLAIN.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void returnSameDataType(TextBox tb, TextBox tb1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                }
            }
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            // returnSameDataType(TextBox_MZ_DATE, TextBox_MZ_IDATE);

            string sysday = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            if (int.Parse(sysday) > int.Parse(TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0')))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('發文日期不得小於系統日期！')", true);
                TextBox_MZ_DATE.Text = string.Empty;
                TextBox_MZ_IDATE.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DATE.ClientID + "').focus();get('" + TextBox_MZ_DATE.ClientID + "').focus();", true);
            }

            TextBox_MZ_IDATE.Text = TextBox_MZ_DATE.Text;
        }

        protected void TextBox_MZ_IDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_IDATE, TextBox_MZ_EXPLAIN1);
        }



        protected void BT_GV1_Click(object sender, EventArgs e)
        {
            GridView1.Visible = false;

            GridView2.Visible = true;

            //新增獎懲類別欄位 20180329 by sky
            string strSQL = @"SELECT MZ_NO ,MZ_PRK ,MZ_PRCT ,MZ_ID ,MZ_NAME 
,C1.mz_kchi AS mz_ad ,C2.mz_kchi AS mz_unit ,C3.mz_kchi AS mz_occc ,C4.mz_kchi AS mz_srank ,C5.mz_kchi AS mz_prrst
FROM  A_PRK2 A 
LEFT JOIN A_KTYPE C1 ON C1.MZ_KTYPE = '04' AND A.MZ_AD = C1.MZ_KCODE
LEFT JOIN A_KTYPE C2 ON C2.MZ_KTYPE = '25' AND A.MZ_UNIT = C2.MZ_KCODE
LEFT JOIN A_KTYPE C3 ON C3.MZ_KTYPE = '26' AND A.MZ_OCCC = C3.MZ_KCODE
LEFT JOIN A_KTYPE C4 ON C4.MZ_KTYPE = '09' AND A.MZ_SRANK = C4.MZ_KCODE
LEFT JOIN A_KTYPE C5 ON C5.MZ_KTYPE = '24' AND A.MZ_PRRST = C5.MZ_KCODE
WHERE A.MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND A.MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";


            DataTable GV2_DT = new DataTable();

            GV2_DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET_DATA");

            DataTable tempKB = new DataTable();
            //OK_click SQL寫入 SESSION 
            //找出PRKB還未謄稿資料
            tempKB = o_DBFactory.ABC_toTest.Create_Table(Session["PRK1_GV1"].ToString(), "GETDATA");

            DataTable dt = new DataTable();



            //如果同一案號[12]用兩次謄稿作業.出來GV2 會變 [12] [12]
            if (Session["PRK1_TEMP_DT2"] != null)
                dt = Session["PRK1_TEMP_DT2"] as DataTable;

            Session["PRK1_TEMP_DT2"] = o_CommonService.Union(tempKB, dt);

            DataTable tempDT = new DataTable();

            tempDT = Session["PRK1_TEMP_DT2"] as DataTable;
            //如果同一案號[12]用兩次謄稿作業.出來GV2 會變 [12] [12]




            Session["PRK1_tempDT"] = tempDT;


            if (tempDT.Rows.Count > 0)
            {
                //將完成的PRK2 和未完成的PRKB 合起來
                GV2_DT = o_CommonService.Union(GV2_DT, tempDT);

                ViewState["GV2_DB"] = GV2_DT;

                GridView2.DataSource = GV2_DT;

                GridView2.AllowPaging = true;

                GridView2.PageSize = 6;

                GridView2.DataBind();

                TextBox6.Text = GV2_DT.Rows.Count.ToString();
            }

            if (GV2_DT.Rows.Count == 0)
            {
                GridView1.Visible = true;

                GridView2.Visible = false;
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Attributes.Add("Style", "display:none");
                e.Row.Cells[1].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            if (GridView2.Rows.Count > 0)
            {
                TextBox6.Text = (ViewState["GV2_DB"] as DataTable).Rows.Count.ToString();
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable tempDT = new DataTable();

            tempDT = ViewState["GV2_DB"] as DataTable;

            GridView2.PageIndex = e.NewPageIndex;

            GridView2.DataSource = tempDT;

            GridView2.AllowPaging = true;

            GridView2.PageSize = 6;

            GridView2.DataBind();

            TextBox6.Text = tempDT.Rows.Count.ToString();
        }

        protected void bt_EXPLAIN1_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXPLAIN1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btAddNote_Click(object sender, EventArgs e)
        {
            string values = "";

            if ((sender as Button).ID == "btAddNote")
            {
                values = TextBox_MZ_EXPLAIN1.Text;
            }
            else
            {
                values = TextBox_MZ_EXPLAIN.Text;
            }

            if (string.IsNullOrEmpty(values))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增片語內容不可空白！')", true);
                return;
            }

            string InsertString = "INSERT INTO A_NOTE(MZ_NOTE,MZ_NOTE_NAME) VALUES('" + Session["ADPMZ_ID"].ToString() + "','" + values + "')";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertString);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗！')", true);
            }
        }

        //獎懲令
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRID.Text.Trim()) && string.IsNullOrEmpty(TextBox_MZ_PRID1.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_punish_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {


                string tmp_url = "A_rpt.aspx?fn=punish&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(TextBox_MZ_PRID.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=&DATAFROM=2";

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }

        }

        //獎懲建議函??
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRID.Text.Trim()) && string.IsNullOrEmpty(TextBox_MZ_PRID1.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_punishSug_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {

                string tmp_url = "A_rpt.aspx?fn=punishSug&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(TextBox_MZ_PRID.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=";

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
        }

        //獎懲令(稿)
        protected void Button3_Click(object sender, EventArgs e)
        {

            string tmp_url = "A_rpt.aspx?fn=punish1&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(TextBox_MZ_PRID.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=";

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }


        int TPM_FION = 0;


        protected void TextBox_MZ_EXPLAIN_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXPLAIN.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode =13;");
        }

        protected void DropDownList_MZ_CHKAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_MZ_PRID.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRID FROM A_CHKAD WHERE MZ_AD='" + DropDownList_MZ_CHKAD.SelectedValue + "'");
        }

        public void gridview_bind(string PRID, string PRID1)
        {
            string sql = string.Format(@"SELECT MZ_NO,MZ_PRCT,MZ_ID,MZ_NAME,(AKD. MZ_KCHI) MZ_AD,
                          (AKU.MZ_KCHI)  MZ_UNIT,  (AKO.MZ_KCHI)  MZ_OCCC, (AKS.MZ_KCHI) MZ_SRANK , (AKP. MZ_KCHI) MZ_PRRST 
                          FROM A_PRK2 A 
                          LEFT JOIN A_KTYPE AKD ON  AKD.MZ_KCODE = MZ_AD AND  AKD.MZ_KTYPE = '04'
                          LEFT JOIN A_KTYPE AKU ON  AKU.MZ_KCODE = MZ_UNIT AND  AKU.MZ_KTYPE = '25'
                          LEFT JOIN A_KTYPE AKO ON  AKO.MZ_KCODE = MZ_OCCC AND  AKO.MZ_KTYPE = '26'
                          LEFT JOIN A_KTYPE AKS ON  AKS.MZ_KCODE = MZ_SRANK AND  AKS.MZ_KTYPE = '09'
                          LEFT JOIN A_KTYPE AKP ON  AKP.MZ_KCODE = MZ_PRRST AND  AKP.MZ_KTYPE = '24'
                          WHERE A.MZ_PRID='{0}' AND A.MZ_PRID1='{1}'", PRID, PRID1);
            DataTable source = o_DBFactory.ABC_toTest.Create_Table(sql, "get");

            GridView1.DataSource = source;
            GridView1.DataBind();


        }

        protected void TextBox_MZ_PRID1_TextChanged(object sender, EventArgs e)
        {
            gridview_bind(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            gridview_bind(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
        }
    }
}
