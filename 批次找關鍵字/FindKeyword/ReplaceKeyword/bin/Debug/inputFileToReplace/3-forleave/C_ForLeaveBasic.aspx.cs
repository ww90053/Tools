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

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveBasic : System.Web.UI.Page
    {
        List<String> DLTB01_MZ_ID = new List<string>();

        List<String> DLTB01_MZ_IDATE1 = new List<string>();

        List<String> DLTB01_MZ_ITIME1 = new List<string>();

        string F = string.Empty;
        string F1 = string.Empty;
        string F2 = string.Empty;
        
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["s"] != null)
            {
                Panel_select_ModalPopupExtender.Show();
            }
            if (!IsPostBack)
            {
                C.check_power();

                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                //預設局本部使用線上簽核
                if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382130000C")
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
                    fill_person_data(Session["ADPMZ_ID"].ToString());


                    Count_hday(Session["ADPMZ_ID"].ToString());

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
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('C_ForLeaveBasic.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
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
                chk_TPMGroup();

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

                tempDT = Count_Card_Record();

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

                if (ViewState["SEARCHMODE"].ToString() == "DLBASE")
                {
                    btDelete.Enabled = false;
                    btUpdate.Enabled = false;
                }
                else
                {
                    if (TextBox_MZ_CHK1.Text == "Y")
                    {
                        btDelete.Enabled = false;
                        btUpdate.Enabled = false;
                    }
                    else if (TextBox_MZ_CHK1.Text != "Y" && (ViewState["C_strGID"].ToString() == "A" || ViewState["C_strGID"].ToString() == "B" || ViewState["C_strGID"].ToString() == "C"))
                    {
                        btUpdate.Enabled = true;
                        btDelete.Enabled = true;
                    }
                    else
                    {
                        btUpdate.Enabled = true;
                        btDelete.Enabled = false;
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
            preload();

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
        protected DataTable Count_Card_Record()
        {
            DataTable tempDT = new DataTable();

            tempDT.Columns.Clear();
            tempDT.Columns.Add("LOGDATE", typeof(string));
            tempDT.Columns.Add("INTIME", typeof(string));
            tempDT.Columns.Add("OUTTIME", typeof(string));
            tempDT.Columns.Add("CODE", typeof(string));
            tempDT.Columns.Add("KIND", typeof(string));
            tempDT.Columns.Add("MEMO", typeof(string));
             
            int INTIME = 90100, OUTTIME = 170000;

            // sam test new add
            //for (int i = 1; i <= 31; i++)
            for (int i = 1; i <= DateTime.Now.Day; i++)
            {
                // sam test new add
                //string LOGDATESTRING = DateTime.Now.Year.ToString() + "/" + DateTime.Now.AddMonths(-1).Month.ToString().PadLeft(2, '0') + "/" + i.ToString().PadLeft(2, '0');

                string LOGDATESTRING = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + i.ToString().PadLeft(2, '0');

                DateTime LOGDATE = DateTime.Parse(LOGDATESTRING);

                string LOGDATE1 = (LOGDATE.Year - 1911).ToString().PadLeft(3, '0') + LOGDATE.Month.ToString().PadLeft(2, '0') + LOGDATE.Day.ToString().PadLeft(2, '0');

                List<String> temp = new List<string>();

                temp = C_CountCardRecord.list_Abnormal(TextBox_MZ_ID.Text.Trim(), LOGDATESTRING, 1);

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


                string selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + TextBox_MZ_ID.Text.Trim() + "' AND LOGDATE='" + LOGDATESTRING + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                string selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + TextBox_MZ_ID.Text.Trim() + "' AND LOGDATE='" + LOGDATESTRING + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                //string selectINOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + TextBox_MZ_ID.Text.Trim() + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");
                string selectOUTOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + TextBox_MZ_ID.Text.Trim() + "' AND LOGDATE='" + LOGDATESTRING + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");


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
                    // 當天為正常上班日，且無刷卡紀錄 
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
                else
                {
                    // 當天為周休二日
                    if (LOGDATE.DayOfWeek == DayOfWeek.Saturday || LOGDATE.DayOfWeek == DayOfWeek.Sunday)
                    {
                        MEMO = "例假日";
                    }
                    // 當天之紀錄
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
                    else
                    {
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///非當日刷卡時間未超過9點1分
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料，且當日無請假紀錄
                            {
                                KIND = "下班未刷卡";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            // Joy 加入是否請假紀錄判斷
                            else if ((int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME) && temp.Count == 0)///多筆資料都在九點前，且當日無請假紀錄
                            {
                                KIND = "下班未刷卡";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班刷卡時間早於5點
                            {
                                if (temp.Count > 1) ///有請假
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATESTRING + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATESTRING + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        MEMO = "下午請假";
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
                            else
                            {
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
                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///上班刷卡時間大於9點1分
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只一筆刷卡資料
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)
                                {
                                    KIND = "上班未刷卡";
                                    MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                                else
                                {
                                    KIND = "遲到下班未刷卡";
                                    MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                            }
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只一筆有請假
                            {
                                CODE = temp[0];

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
                                else
                                {
                                    KIND = "遲到下班未刷卡";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                            {
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
                        else if (string.IsNullOrEmpty(selectINTIME.Trim()))
                        {
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

            tempDT = Count_Card_Record();

            tempDT = tempDT.AsEnumerable().OrderByDescending(dr => dr["LOGDATE"]).CopyToDataTable();

            GridView_DLTB_UNUSUAL.DataSource = tempDT;

            GridView_DLTB_UNUSUAL.AllowPaging = true;

            Session["GridView_DLTB_UNUSUAL"] = tempDT;

            GridView_DLTB_UNUSUAL.PageSize = 10;

            GridView_DLTB_UNUSUAL.DataBind();
        }

        protected void DLTB_DAYS_COUNT(string MZ_ID)
        {
            TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

            string selectSQL = "SELECT MZ_FDATE,MZ_TYEAR,MZ_TMONTH,MZ_MEMO,MZ_QUA_DATE FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GET");

            int tyear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TYEAR"].ToString().Trim()) ? "0" : temp.Rows[0]["MZ_TYEAR"].ToString());

            int tmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TMONTH"].ToString().Trim()) ? "0" : temp.Rows[0]["MZ_TMONTH"].ToString());

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

            TextBox_MZ_TYEAR.Text = string.IsNullOrEmpty(temp.Rows[0]["MZ_TYEAR"].ToString()) ? "0" : temp.Rows[0]["MZ_TYEAR"].ToString();

            TextBox_MZ_TMONTH.Text = string.IsNullOrEmpty(temp.Rows[0]["MZ_TMONTH"].ToString()) ? "0" : temp.Rows[0]["MZ_TMONTH"].ToString();

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

            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth;

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

            //修改判斷為服務年資+併計年資大於5年即顯示 by sky
            int checkYM = monthDiff + int.Parse(TextBox_MZ_TYEAR.Text) + int.Parse(TextBox_MZ_TMONTH.Text);
            if (float.Parse((checkYM / 12).ToStringNullSafe() + "." + (checkYM % 12).ToStringNullSafe()) > 5)
            {
                Panel_SDAY.Visible = true;
            }
            else
            {
                Panel_SDAY.Visible = false;
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
                                                      WHERE MZ_CODE='11' AND MZ_ID='{0}' AND dbo.SUBSTR(MZ_IDATE1,1,3)='{1}') 
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

                if (int.Parse(TextBox_MZ_HTIME.Text.Trim()) - htime < 0)
                {
                    if ((htime % 8) != 0)
                    {
                        //sam.hsu wellsince 20201229
                        hday = hday + int.Parse((htime / 8).ToString());
                        htime = int.Parse((htime % 8).ToString());

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
                int stime = 0;

                string strSDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='02' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                DataTable SDAY_Count = o_DBFactory.ABC_toTest.Create_Table(strSDAY_Count, "get");

                if (SDAY_Count.Rows.Count > 0)
                {
                    sday1 = int.Parse(SDAY_Count.Rows[0]["MZ_TDAY"].ToString());

                    stime = int.Parse(SDAY_Count.Rows[0]["MZ_TTIME"].ToString());
                }

              
                //20140120
                int sday_used;

                int stime_used;

                int sday_canused;

                int stime_canused=0;

                if (stime != 0)
                {
                    int sday_Add = stime / 8;
                    sday_used = sday1 + sday_Add;
                    stime_used = stime % 8;
                    sday_canused = int.Parse(TextBox_MZ_SICKDAY.Text) - sday_used - 1;

                    //2013/07/08
                    //stime_canused = 8 - stime_used;
                    if (sday_canused <= 0 && stime_used < 8)
                    {
                        stime_used = 0;
                    }
                    else
                    {
                        stime_canused = 8 - stime_used;
                    }
                    //2013/07/08
                }
                else
                {
                    sday_used = sday1;
                    stime_used = 0;
                    sday_canused = int.Parse(TextBox_MZ_SICKDAY.Text) - sday_used;
                    stime_canused = 0;
                }

                TextBox_MZ_SICKDAY_CANUSE.Text = int.Parse(sday_canused.ToString()) < 0 ? "0" : sday_canused.ToString();
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
           
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveBasicSearch.aspx?TableName=BASIC&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
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
            DropDownList_MZ_RNAME.DataTextField="MZ_NAME";
            DropDownList_MZ_RNAME.DataValueField="MZ_ID";
            DropDownList_MZ_RNAME.DataBind();
        
        
        }

        private void rname_change(string MZ_RID)
        {
            //20140317 by 立廷
            //20140315    
            string SQL = "SELECT MZ_ID,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_OCCC_CH FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text.ToUpper() + "'";
            DataTable  AD =  o_DBFactory.ABC_toTest.Create_Table (SQL,"get");
            if (AD.Rows.Count > 0)
            {
                //代理人選單針對新增狀態增加在職條件 20190808 by sky
                string sqlStr = string.Empty;
                DataTable dt = new DataTable();
                //switch (o_A_DLBASE.OCCC(TextBox_MZ_ID.Text))
                switch (AD.Rows[0]["MZ_OCCC_CH"].ToStringNullSafe().Trim ())
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
                        if (AD.Rows[0]["MZ_EXAD"].ToStringNullSafe() == "382130000C")
                        {
                            //條件:如果是總局的工友,則 MZ_OCCC(職稱ID) 包含以下
                            //1453    警員
                            //Z014    駕駛
                            //Z015    工友
                            //Z016    技工
                            sqlStr = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_OCCC in ({1}) {3}
                                                    union all 
                                                    SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_OCCC='Z011' AND MZ_EXUNIT='{2}' {3} "
                                                    , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe()
                                                    , "'Z014','Z015','Z016','1453'"
                                                    , AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe()
                                                    , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                            //增加資訊室工友可同科室協助。 20190917 by sky Add
                            if (AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe() == "0287")
                            {
                                sqlStr += string.Format(@"union all SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' {2} "
                                                        , AD.Rows[0]["MZ_EXAD"].ToStringNullSafe()
                                                        , AD.Rows[0]["MZ_EXUNIT"].ToStringNullSafe()
                                                        , ViewState["Mode"].ToStringNullSafe() == "INSERT" ? "AND MZ_STATUS2='Y'" : "");
                            }

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

                Session["PKEY_MZ_IDATE1"] = TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                Session["PKEY_MZ_ITIME1"] = TextBox_MZ_ITIME1.Text;

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);

            if (TextBox_MZ_CODE.Text == "15")
            {
                btRESTDATECHECK.Enabled = false;
                ViewState["D1"] = TextBox_MZ_IDATE1.Text;
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
                    
                    string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE  MZ_RESTDATE LIKE '%" + Session["PKEY_MZ_IDATE1"].ToString() + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";

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
                                    if (s[j].Substring(0, 7) == Session["PKEY_MZ_IDATE1"].ToString())
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
                        if (TextBox_MZ_IDATE1.Text.PadLeft(7, '0') != Session["PKEY_MZ_IDATE1"].ToString())
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                                if (s.Count() == 1)
                                {
                                    string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='" + s[0].Replace(Session["PKEY_MZ_IDATE1"].ToString(), TextBox_MZ_IDATE1.Text.Replace("/", "").PadLeft(7, '0')) + "',MZ_RESTHOUR=MZ_RESTHOUR WHERE MZ_DATE='" + dt.Rows[i]["MZ_DATE"].ToString() + "' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_RESTHOUR>0";

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
                                        if (s[j].Substring(0, 7) == Session["PKEY_MZ_IDATE1"].ToString())
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
        /// <param name="field"></param>
        /// <param name="dPath"></param>
        /// <returns></returns>
        protected string CHECK_UPLOADFILE(string path, string ErrorString, FileUpload fu, Button btn, HyperLink hl, string field,string dPath)
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
                    //檢查補休時數
                    check_15_Hour();

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

                            old_IDATE1 = Session["PKEY_MZ_IDATE1"].ToString();

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

                        if ((string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) || TextBox_MZ_TDAY.Text == "0") && (string.IsNullOrEmpty(TextBox_MZ_TTIME.Text.Trim()) || TextBox_MZ_TTIME.Text == "0"))
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
                            
                            if (TextBox_MZ_CODE.Text.Trim() == "15")
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
                        if (TextBox_MZ_CODE.Text.Trim() == "15")
                        {
                            if (Session["MZ_RESTHOUR_DT"] == null)
                            {
                                if (ViewState["Mode"].ToString() == "UPDATE")
                                {
                                    int c = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='{0}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_RESTDATE LIKE '%{1}%'", TextBox_MZ_ID.Text.Trim(), Session["PKEY_MZ_IDATE1"].ToString())));
                                    if (c <= 0)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇補休日期！');", true);
                                        return;
                                    }
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
                        if (TextBox_MZ_CODE.Text.Trim() == "03" || TextBox_MZ_CODE.Text.Trim() == "11" || TextBox_MZ_CODE.Text.Trim() == "22")
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
                            else if (TextBox_MZ_CODE.Text == "11")
                            {
                                SelectString = " SELECT MZ_SDAY,MZ_SDAY_HOUR " +
                                               " FROM C_DLTBB " +
                                               " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            }
                            else if (TextBox_MZ_CODE.Text == "22")
                            {
                                SelectString = " SELECT MZ_SDAY2,MZ_SDAY2_HOUR " +
                                               " FROM C_DLTBB " +
                                               " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + (year).ToString().PadLeft(3, '0') + "'";
                            }

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

                            CHECK_15();


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
                                    new SqlParameter("MZ_UNIT", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM VW_A_DLBASE_S1 WHERE MZ_ID='" +TextBox_MZ_ID.Text +"'") },
                                    new SqlParameter("MZ_AD", SqlDbType.VarChar) { Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM VW_A_DLBASE_S1 WHERE MZ_ID='" +TextBox_MZ_ID.Text +"'") },
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
                            CHECK_15();

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
                    INSERT_15();

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
        /// 申請補休額外處理
        /// </summary>
        protected void INSERT_15()
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
                if (o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CHK1 FROM C_DLTB01 WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDATE1='" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ITIME1='" + TextBox_MZ_ITIME1 + "'") != "Y")
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
                            string selectSQL = "SELECT MZ_RESTDATE,MZ_RESTHOUR,MZ_DATE FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND  MZ_RESTDATE LIKE '%" + TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0') + "%' AND dbo.SUBSTR(RESTFLAG,1,1)='Y'";

                            DataTable dt = new DataTable();

                            dt = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "getdeletevalue");

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {

                                string[] s = dt.Rows[i]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                                if (s.Count() == 1)
                                {
                                    string UpdateSQL = string.Format(@"UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE=null,MZ_RESTHOUR=OTIME,LOCK_FLAG='N' WHERE MZ_DATE='{0}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{1}'"
                                                                    , dt.Rows[i]["MZ_DATE"].ToString()
                                                                    , TextBox_MZ_ID.Text.Trim());

                                    //Log
                                    LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "刪除假單，更新可補休日");

                                    o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                }
                                else
                                {
                                    int hour = 0;
                                    string RESTDATE = "";
                                    for (int j = 0; j < s.Count(); j++)
                                    {
                                        if (s[j].Substring(0, 7) == TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0'))
                                        {
                                            RESTDATE = dt.Rows[i]["MZ_RESTDATE"].ToString().Replace("，" + s[j], "");

                                            string[] y = s[j].Split('：');

                                            hour = int.Parse(y[1]) + int.Parse(dt.Rows[i]["MZ_RESTHOUR"].ToString());

                                            break;
                                        }
                                    }

                                    if (RESTDATE != "")
                                    {
                                        //TODO 補休,建議建立流水號.用流水號做KEY修改補休時數使用情形,否則一下面這一句.如果同一天有兩種不同種類的加班.都會鎖住,
                                        // 所以其他相對應的使用(包含收尋補休狀態都要調整)
                                        string UpdateSQL = string.Format(@"UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='{0}',MZ_RESTHOUR={1} 
                                                                             WHERE MZ_DATE='{2}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='{3}' "
                                                                        , RESTDATE, hour, dt.Rows[i]["MZ_DATE"].ToString(), TextBox_MZ_ID.Text.Trim());

                                        //Log
                                        LogModel.saveLog("COHI", "U", UpdateSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], "刪除假單，更新可補休日");

                                        o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
                                    }
                                }
                            }
                        }


                        if (DLTB01_MZ_ID.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_ForLeaveBasic.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
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

       
        protected void btCODE_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_CODE.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_CODE1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=MZ_CODE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_CODE_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_CODE.Text = TextBox_MZ_CODE.Text.ToUpper();

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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_dayfact_FilteredTextBoxExtender.ClientID + "').focus();$get('" + TextBox_dayfact_FilteredTextBoxExtender.ClientID + "').focus();", true);
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

            if (TextBox_MZ_CODE.Text == "15")
            {
                if (ViewState["Mode"].ToString() != "Update")
                {
                    btRESTDATECHECK.Enabled = true;
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

        protected void btRESTDATECHECK_Click(object sender, EventArgs e)
        {
            Session["ForLeaveBasic_BT"] = BT_GV1.ClientID;
            Session["ForLeaveBasic_CID1"] = TextBox_MZ_TTIME.ClientID;
            Session["ForLaeveBasic_RESTDATE_ID"] = TextBox_MZ_ID.Text.Trim();
            int TTIME = int.Parse(string.IsNullOrEmpty(TextBox_MZ_TDAY.Text.Trim()) ? "0" : TextBox_MZ_TDAY.Text) * 8 + int.Parse(string.IsNullOrEmpty(TextBox_MZ_TTIME.Text) ? "0" : TextBox_MZ_TTIME.Text);
            Session["LeaveTime"] = TTIME.ToString();  //使用者申請的請假時數
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_RESTDATE_SEARCH.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        
        int TPM_FION=0;

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

                string tmp_url = "C_rpt.aspx?fn=offduty&DATE=" + TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0').Trim () + 
                    "&TIME="+(TextBox_MZ_ITIME1.Text.Substring(0, 2) + ":" + TextBox_MZ_ITIME1.Text.Substring(2, 2)).Trim()+
                    "&MZ_NAME="+
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

                    string tmp_url = "C_rpt.aspx?fn=abroad&TPM_FION=" + TPM_FION  +
                    "&DATE=" + TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
                    "&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim())  ;

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
        /// 檢查補休情況
        /// </summary>
        protected void check_15_Hour()
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

                        if (TTIME > totaltime + ReturnHour)
                        {
                            TextBox_MZ_TDAY.Text = "0";

                            TextBox_MZ_TTIME.Text = "0";

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                            return;
                        }
                        else if (totaltime + ReturnHour > TTIME)
                        {
                            int s = totaltime + ReturnHour - TTIME;
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
                            return;
                        }
                    }
                    else//沒有選填可補休日
                    {
                        TextBox_MZ_TDAY.Text = "0";

                        TextBox_MZ_TTIME.Text = "0";

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                        return;
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

                        if (TTIME > totaltime)
                        {
                            TextBox_MZ_TDAY.Text = "0";

                            TextBox_MZ_TTIME.Text = "0";

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                            return;
                        }
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
                        return;
                    }
                }
            }
        }

        protected void BT_GV1_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["MZ_RESTHOUR_DT"] as DataTable;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TDAY.ClientID + "').focus();$get('" + TextBox_MZ_TDAY.ClientID + "').focus();", true);
                }
            }
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
                    file_name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Session["TPM_MID"].ToString() + Extension.ToString();
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

            //如果有建立人事代碼對應的View .這段可以修

            string strsql = @"SELECT MZ_NAME ,MZ_AD_CH,MZ_UNIT_CH,MZ_EXAD_CH, MZ_EXUNIT_CH,  MZ_OCCC_CH
                              FROM VW_A_DLBASE_S1  
                              
                              WHERE MZ_ID='" +ID_NO +"'";
            
            DataTable person_data = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");

            if (person_data.Rows.Count > 0)
            {
                TextBox_MZ_NAME.Text = person_data.Rows[0]["MZ_NAME"].ToString();

                Label_MZ_AD.Text = person_data.Rows[0]["MZ_AD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_UNIT_CH"].ToString();

                Label_MZ_EXAD.Text = person_data.Rows[0]["MZ_EXAD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_EXUNIT_CH"].ToString() + "     " + person_data.Rows[0]["MZ_OCCC_CH"].ToString();
            }

        
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

        

    }
}
