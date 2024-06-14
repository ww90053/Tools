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
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;
using System.IO;
using TPPDDB._2_salary;
using TPPDDB.App_Code;
using TPPDDB.Model.Const;

//using TPPDDB._2_salary;

namespace TPPDDB._1_personnel
{

   
    public partial class Personal1_1 : System.Web.UI.Page
    {

        string strGID = "";


        List<String> dataNext = new List<string>();//紀錄查詢條件所有身分證號

        List<String> Personal_PICPATH = new List<string>();
        string sqlString;//sql語法字串

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
            // 2012.03.01 關閉刪除資料的功能 by介入
            btDelete.Visible = false;

            ///群組權限
            strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            ///查詢ID
            ViewState["MZ_ID"] = Session["PersonalSearch_ID"];
            Session.Remove("PersonalSearch_ID");
            ///查詢姓名
            ViewState["MZ_NAME"] = Session["PersonalSearch_NAME"];
            Session.Remove("PersonalSearch_NAME");

            ViewState["MZ_ID1"] = Session["Personal_Search_Result_ID"];
            Session.Remove("Personal_Search_Result_ID");

            ViewState["AD"] = Request["AD"];//查詢機關
            ViewState["UNIT"] = Request["UNIT"];//查詢單位
            ViewState["XCOUNT"] = Request["XCOUNT"];//取消時重讀資料用

            A.set_Panel_EnterToTAB(ref this.Panel_Title);
            A.set_Panel_EnterToTAB(ref this.Panel_Basic);
            A.set_Panel_EnterToTAB(ref this.Panel_Now);
            A.set_Panel_EnterToTAB(ref this.Panel_ButtonChange);
            A.set_Panel_EnterToTAB(ref this.Panel_Card);
            A.set_Panel_EnterToTAB(ref this.Panel_Career);
            A.set_Panel_EnterToTAB(ref this.Panel_efficiency);
            A.set_Panel_EnterToTAB(ref this.Panel_Exam);
            A.set_Panel_EnterToTAB(ref this.Panel_FAMILY);

            A.set_Panel_EnterToTAB(ref this.Panel_TableEdu);
            A.set_Panel_EnterToTAB(ref this.Panel3);
            A.set_Panel_EnterToTAB(ref this.Panel4);
            A.set_Panel_EnterToTAB(ref this.Panel5);
            A.set_Panel_EnterToTAB(ref this.Panel6);
            A.set_Panel_EnterToTAB(ref this.Panel7);
            A.set_Panel_EnterToTAB(ref this.Panel8);

            if (!IsPostBack)
            {
                //不可輸入之TEXTBOX
                TextBox_MZ_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXAD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_CHISI1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXUNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_NREA1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PCHIEF1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SLVC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SRANK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_TBCD31.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_TBCD91.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_TBDV1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_UNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_NIN1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_NRT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PESN1.Attributes.Add("onkeydown", "javascript:return false;");

                TextBox_CAREER_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_CHISI1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_NREA1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_PCHIEF1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_RANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_RANK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_NIN1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_NRT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_PESN1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_CAREER_PCHIEF1.Attributes.Add("onkeydown", "javascript:return false;");

                TextBox_MZ_SCHOOL.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_DEPARTMENT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_TITLE1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_WORK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EDULEVEL1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EDUKIND1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EDUCLASS1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_ARMYSTATE1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_ARMYRANK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_ARMYKIND1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_ARMYCOURSE1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_EXAM_ADMISSION1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_EXAM_CLASS1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_EXAM_NAME1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXTPOS_SRANK1.Attributes.Add("onkeydown", "javascript:return false;");

                if (ViewState["XCOUNT"] != null)//刪除或修改資料時！紀錄reload的時候該抓哪筆資料
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount1.Text = "0";
                    findPic(int.Parse(xcount1.Text));//尋找照片
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    //判斷上筆下筆按鍵是否可按！
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && dataNext.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < dataNext.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == dataNext.Count - 1 && dataNext.Count > 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (dataNext.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + dataNext.Count.ToString() + "筆";
                    }

                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = true;
                }

                //計算查詢條件結果共幾筆
                if (ViewState["MZ_ID1"] != null)
                {
                    xcount.Text = "0";
                    dataNext = Session["datanext"] as List<String>;
                    dataNext.Clear();
                    dataNext.Insert(0, ViewState["MZ_ID1"].ToString());
                    Session["dataNext"] = dataNext;
                    finddata(int.Parse(xcount.Text));
                    xcount1.Text = "0";
                    findPic(int.Parse(xcount1.Text));
                    btInsert.Enabled = true;
                    btUpdate.Enabled = true;
                    btOK.Enabled = false;
                    btDelete.Enabled = true;
                    btCancel.Enabled = false;
                    btNEXT.Enabled = false;
                    btUpper.Enabled = false;
                }
                else if (ViewState["MZ_ID"] != null)
                {
                    SqlParameter[] ParamaterList ={
                    new SqlParameter("MZ_NAME",SqlDbType.NVarChar){Value = ViewState["MZ_NAME"].ToString()}
                    };

                    string strSQL = "SELECT MZ_ID,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE  WHERE 1=1";

                    if (string.IsNullOrEmpty(ViewState["MZ_ID"].ToString()) == false)
                    {
                        //strSQL += " AND MZ_ID LIKE'" + ViewState["MZ_ID"].ToString() + "%'";
                        strSQL += " AND MZ_ID ='" + ViewState["MZ_ID"].ToString() + "'";
                        dataNext = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");//塞查詢相關身分證號到list
                    }
                    else
                    {
                        if (ViewState["MZ_NAME"].ToString() != "")
                        {
                            strSQL += " AND MZ_NAME LIKE @MZ_NAME+'%'";
                        }
                        else
                        {
                            ParamaterList = null;
                        }

                        if (ViewState["AD"].ToString() != "" && ViewState["UNIT"].ToString() != "")
                        {
                            strSQL += " AND MZ_EXAD ='" + ViewState["AD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["UNIT"].ToString() + "'";
                        }
                        else if (ViewState["AD"].ToString() != "")
                        {
                            strSQL += " AND (MZ_EXAD='" + ViewState["AD"].ToString() + "' OR MZ_AD='" + ViewState["AD"].ToString() + "' OR PAY_AD='" + ViewState["AD"].ToString() + "')";
                        }

                        strSQL = strSQL + " ORDER BY MZ_EXAD,MZ_EXUNIT,TBDV,MZ_PCHIEF,MZ_OCCC";

                        dataNext = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID", ParamaterList);//塞查詢相關身分證號到list

                    }

                    Session["datanext"] = dataNext;//跨網頁傳質問題！故先用session接值

                    //判斷是否查詢條件有資料
                    if (dataNext.Count > 5)
                    {
                        //HttpCookie Cookie1 = new HttpCookie("Personal_NAME");
                        //Cookie1.Value = TPMPermissions._strEncood(ViewState["MZ_NAME"].ToString());
                        //Response.Cookies.Add(Cookie1);

                        Session["Personal_NAME"] = ViewState["MZ_NAME"];

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Search_Result.aspx?AD=" 
                            + (ViewState["AD"] == null ? "" : ViewState["AD"].ToString()) 
                            + "&UNIT=" + (ViewState["UNIT"] == null ? "" : ViewState["UNIT"].ToString()) 
                            + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin1','top=200,left=680,width=350,height=400,scrollbars=yes,toolbar=no,location=no,menubar=no,status=no,resizable=no,scrollbars=yes');", true);
                        
                    }

                    if (dataNext.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (dataNext.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        xcount1.Text = "0";
                        findPic(int.Parse(xcount1.Text));
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        xcount1.Text = "0";
                        findPic(int.Parse(xcount1.Text));
                    }
                    //共幾筆資料
                    if (dataNext.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + dataNext.Count.ToString() + "筆";
                    }
                }
                else
                {
                    xcount.Text = "0";
                    dataNext.Clear();
                    dataNext.Insert(0, Session["ADPMZ_ID"].ToString());
                    Session["dataNext"] = dataNext;
                    finddata(int.Parse(xcount.Text));
                    xcount1.Text = "0";
                    findPic(int.Parse(xcount1.Text));
                    btInsert.Enabled = true;
                    btUpdate.Enabled = true;
                    btOK.Enabled = false;
                    btDelete.Enabled = true;
                    btCancel.Enabled = false;
                    btNEXT.Enabled = false;
                    btUpper.Enabled = false;
                }

                //判斷此ID在資料庫是否存在～以決定相關BUTTON是否可按
                if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'")))
                {
                    btCareer.Enabled = false;
                    btEducation.Enabled = false;
                    btefficiency.Enabled = false;
                    btExam.Enabled = false;
                    btFamily.Enabled = false;
                    btCard.Enabled = false;
                    btefficiency_insert.Enabled = false;
                    //20150324
                    btefficiency.Enabled = false;
                    //btUpdateFromPosit.Enabled = false;
                }
                else
                {
                    btCareer.Enabled = true;
                    btEducation.Enabled = true;
                    btefficiency.Enabled = true;
                    btExam.Enabled = true;
                    btFamily.Enabled = true;
                    btCard.Enabled = true;
                    btefficiency_insert.Enabled = true;
                    //20150324
                    btefficiency.Enabled = true;
                    //btUpdateFromPosit.Enabled = true;
                }

                MultiView1.ActiveViewIndex = 0;//預設停留第一頁（基本資料）
                ChangeButtonColor(Panel_ButtonChange, btBasic);//修改當前頁面button顏色

                chk_TPMGroup();
                A.controlEnable(ref this.Panel_Title, false);
                A.controlEnable(ref this.Panel_Basic, false);
                A.controlEnable(ref this.Panel_Now, false);
            }

            // sam.hsu 20201211 人事基本資料的薪資帳號只有服務單位為 PAY0、PAZ0、0287 才能使用
            if (Session[ConstSession.EXUNIT].ToString().ToUpper().Equals("PAY0") ||
                Session[ConstSession.EXUNIT].ToString().ToUpper().Equals("PAZ0") ||
                Session[ConstSession.EXUNIT].ToString().ToUpper().Equals("0287"))
            {
                this.btBankSet.Enabled = true;
                this.btnChangSalary.Enabled = true;
            }
            else
            {
                this.btBankSet.Enabled = false;
                this.btnChangSalary.Enabled = false;
            }


        }
        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (strGID)
            {
                case "A":
                    btnExportSalaryMetaData.Visible = true;
                    break;
                case "B":
                case "C":
                    btnExportSalaryMetaData.Visible = true;
                    btefficiency_insert.Visible = false;
                    btefficiency.Enabled = false;
                    //2016/1/27 jack
                    string strSQL_Account = "select * from TP_MODEL_MEMBER where (tpmn_gid='1901' or tpmn_gid='1909') and TPMID='" + Session["TPM_MID"] + "'";

                    DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL_Account, "get");
                    if (temp.Rows.Count > 0)
                    {
                        btefficiency_insert.Visible = true;
                        btefficiency.Enabled = true;
                    }

                    break;

                case "E":
                    //btSearch.Visible = false;
                    //btNEXT.Visible = false;
                    //btUpper.Visible = false;
                    //btefficiency_insert.Visible = false;   
                    btnExportSalaryMetaData.Visible = false;
                    btefficiency.Visible = false;
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        TextBox_MZ_POLNO.ReadOnly = true;
                        TextBox_MZ_BIR.ReadOnly = true;
                        TextBox_MZ_OFFYY.ReadOnly = true;
                        TextBox_MZ_ADATE.ReadOnly = true;
                        TextBox_MZ_QUA_DATE.ReadOnly = true; //新增合格實授日欄位 20181120 by sky
                        TextBox_MZ_LDATE.ReadOnly = true;
                        TextBox_MZ_TDATE.ReadOnly = true;
                        TextBox_MZ_ODAY.ReadOnly = true;
                        TextBox_MZ_TBCD9.ReadOnly = true;
                        TextBox_MZ_FDATE.ReadOnly = true;
                        TextBox_MZ_PCHIEF.ReadOnly = true;
                        DropDownList_MZ_INSURANCEMODE.Enabled = false;

                        btInsert.Visible = false;
                        btDelete.Visible = false;
                        btUpdate.Visible = true;
                        btOK.Visible = true;
                        btCancel.Visible = true;

                    }
                    else if (MultiView1.ActiveViewIndex == 1)
                    {
                        btInsert.Visible = false;
                        btDelete.Visible = false;
                        btUpdate.Visible = false;
                        btOK.Visible = false;
                        btCancel.Visible = false;
                        foreach (object obj1 in Panel_Now.Controls)
                        {
                            if (obj1 is Button)
                            {
                                Button bt1 = obj1 as Button;
                                bt1.Visible = false;
                            }

                            if (obj1 is TextBox)
                            {
                                TextBox tb1 = obj1 as TextBox;
                                tb1.ReadOnly = true;
                            }
                        }
   
                    }
                    else if (MultiView1.ActiveViewIndex == 4 || MultiView1.ActiveViewIndex == 5)
                    {
                        btInsert.Visible = false;
                        btDelete.Visible = false;
                        btUpdate.Visible = false;
                        btOK.Visible = false;
                        btCancel.Visible = false;
      
                    }
                    else
                    {
                        btInsert.Visible = true;
                        //btDelete.Visible = true;
                        btUpdate.Visible = true;
                        btOK.Visible = true;
                        btCancel.Visible = true;
                
                    }
                    break;

                case "D":
                default:
                    btnExportSalaryMetaData.Visible = false;
                    btSearch.Visible = false;
                    btNEXT.Visible = false;
                    btUpper.Visible = false;
                    btefficiency_insert.Visible = false;
                    //20150324
                    btefficiency.Visible = false;
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        TextBox_MZ_POLNO.ReadOnly = true;
                        TextBox_MZ_BIR.ReadOnly = true;
                        TextBox_MZ_OFFYY.ReadOnly = true;
                        TextBox_MZ_ADATE.ReadOnly = true;
                        TextBox_MZ_QUA_DATE.ReadOnly = true; //新增合格實授日欄位 20181120 by sky
                        TextBox_MZ_LDATE.ReadOnly = true;
                        TextBox_MZ_TDATE.ReadOnly = true;
                        TextBox_MZ_ODAY.ReadOnly = true;
                        TextBox_MZ_TBCD9.ReadOnly = true;
                        TextBox_MZ_FDATE.ReadOnly = true;
                        TextBox_MZ_PCHIEF.ReadOnly = true;
                        DropDownList_MZ_INSURANCEMODE.Enabled = false;

                        btInsert.Visible = false;
                        btDelete.Visible = false;
                        btUpdate.Visible = true;
                        btOK.Visible = true;
                        btCancel.Visible = true;
                    }
                    else if (MultiView1.ActiveViewIndex == 1)
                    {
                        btInsert.Visible = false;
                        btDelete.Visible = false;
                        btUpdate.Visible = false;
                        btOK.Visible = false;
                        btCancel.Visible = false;
                        foreach (object obj1 in Panel_Now.Controls)
                        {
                            if (obj1 is Button)
                            {
                                Button bt1 = obj1 as Button;
                                bt1.Visible = false;
                            }

                            if (obj1 is TextBox)
                            {
                                TextBox tb1 = obj1 as TextBox;
                                tb1.ReadOnly = true;
                            }
                        }
                    }
                    else if (MultiView1.ActiveViewIndex == 4 || MultiView1.ActiveViewIndex == 5)
                    {
                        btInsert.Visible = false;
                        btDelete.Visible = false;
                        btUpdate.Visible = false;
                        btOK.Visible = false;
                        btCancel.Visible = false;
                    }
                    else
                    {
                        btInsert.Visible = true;
                        //btDelete.Visible = true;
                        btUpdate.Visible = true;
                        btOK.Visible = true;
                        btCancel.Visible = true;
                    }
                    break;
                                
            }
        }
        //判斷欄位是否可空白
        protected void Can_not_empty(TextBox tb1, object obj, string fieldname)
        {
            if (string.IsNullOrEmpty(tb1.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + fieldname + "不可空白')", true);
            }
            else
            {
                if (obj is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is RadioButtonList)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as RadioButtonList).ClientID + "').focus();$get('" + (obj as RadioButtonList).ClientID + "').focus();", true);
                }
            }
        }

        //搜尋相片是否存在
        protected void findPic(int PictureCount)
        {
            Personal_PICPATH = Session["Personal_PICPATH"] as List<string>;

            if (Personal_PICPATH == null || Personal_PICPATH.Count == 0)
            {
                btNextPic.Enabled = false;
                btUpperPic.Enabled = false;
                Image1.ImageUrl = "~/1-personnel/images/nopic.jpg";
            }
            else
            {
                Image1.ImageUrl = Personal_PICPATH[PictureCount];

                if (Personal_PICPATH.Count > 1)
                {
                    btNextPic.Enabled = true;
                }
            }
        }
        //查詢
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 1)
            {
                btInsert.Enabled = true;
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch.aspx?TableName=DLBASE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }
        /// <summary>
        /// 查詢資料用
        /// </summary>
        /// <param name="Datacount">第幾筆資料</param>
        protected void finddata(int Datacount)
        {
            dataNext = Session["datanext"] as List<string>;


            //2013/04/11 by 立廷
            string strSQL_Account = "SELECT TPMUSER FROM TPM_MEMBER WHERE TPM_IDNO='" + dataNext[Datacount] + "'";

            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL_Account, "get");

            
            if(temp.Rows.Count>0)
            {
                txt_Account.Text = temp.Rows[0][0].ToString(); 
            }
                
            else
            {
                txt_Account.Text = "無帳號";
            }
            //
            string strSQL2 = "SELECT * FROM VW_A_DLBASE WHERE MZ_ID='" + dataNext[Datacount] + "'";

            //20140122
            if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00001", TPMPermissions._boolTPM(), strSQL2) == "N")
            {
                TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00001", strSQL2);
            }
            
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL2, "GET");

            if (dt.Rows.Count == 1)
            {
                TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_ENAME.Text = dt.Rows[0]["MZ_ENAME"].ToString().Trim();
                TextBox_MZ_ID.Text = dt.Rows[0]["MZ_ID"].ToString().Trim();
                TextBox_MZ_BIR.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_BIR"].ToString().Trim());
                TextBox_MZ_TBCD3.Text = dt.Rows[0]["MZ_TBCD3"].ToString().Trim();
                TextBox_MZ_CITY.Text = dt.Rows[0]["MZ_CITY"].ToString().Trim();
                TextBox_MZ_BL.Text = dt.Rows[0]["MZ_BL"].ToString().Trim();
                try
                {
                    DropDownList_MZ_SM.SelectedValue = dt.Rows[0]["MZ_SM"].ToString().Trim();
                }
                catch
                {
                    DropDownList_MZ_SM.SelectedValue = "0";
                }
                try
                {
                    DropDownList_MZ_SEX.SelectedValue = dt.Rows[0]["MZ_SEX"].ToString().Trim();
                }
                catch
                {
                    DropDownList_MZ_SEX.SelectedValue = "1";
                }

                // sam 20200630 新增發薪機關
                TextBox_PAY_AD.Text = dt.Rows[0]["PAY_AD"].ToString().Trim();
                TextBox_PAY_AD1.Text = dt.Rows[0]["PAY_AD_CH"].ToString().Trim();


                TextBox_MZ_ADD1.Text = dt.Rows[0]["MZ_ADD1"].ToString().Trim();
                TextBox_MZ_ADD2.Text = dt.Rows[0]["MZ_ADD2"].ToString().Trim();
                TextBox_MZ_PHONE.Text = dt.Rows[0]["MZ_PHONE"].ToString().Trim();
                TextBox_MZ_OFFYY.Text = dt.Rows[0]["MZ_OFFYY"].ToString().Trim();
                TextBox_MZ_FYEAR.Text = dt.Rows[0]["MZ_FYEAR"].ToString().Trim();

                // sam.hsu 20201208
                dll_MZ_TRAINING.SelectedValue = dt.Rows[0]["MZ_TRAINING"].ToString().Trim();

                TextBox_MZ_TBCD9.Text = dt.Rows[0]["MZ_TBCD9"].ToString().Trim();
                TextBox_MZ_POLNO.Text = dt.Rows[0]["MZ_POLNO"].ToString().Trim();
                TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString().Trim();
                TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_EXAD.Text = dt.Rows[0]["MZ_EXAD"].ToString().Trim();
                TextBox_MZ_EXUNIT.Text = dt.Rows[0]["MZ_EXUNIT"].ToString().Trim();
                TextBox_MZ_RANK.Text = dt.Rows[0]["MZ_RANK"].ToString().Trim();
                TextBox_MZ_RANK1.Text = dt.Rows[0]["MZ_RANK1"].ToString().Trim();
                TextBox_MZ_SRANK.Text = dt.Rows[0]["MZ_SRANK"].ToString().Trim();
                TextBox_MZ_EXTPOS_SRANK.Text = dt.Rows[0]["MZ_EXTPOS_SRANK"].ToString().Trim();
                TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_CHISI.Text = dt.Rows[0]["MZ_CHISI"].ToString().Trim();
                TextBox_MZ_POSIND.Text = dt.Rows[0]["MZ_POSIND"].ToString().Trim();
                TextBox_MZ_NRT.Text = dt.Rows[0]["MZ_NRT"].ToString().Trim();
                TextBox_MZ_TBDV.Text = dt.Rows[0]["MZ_TBDV"].ToString().Trim();
                TextBox_MZ_PCHIEF.Text = dt.Rows[0]["MZ_PCHIEF"].ToString().Trim();
                TextBox_MZ_NREA.Text = dt.Rows[0]["MZ_NREA"].ToString().Trim();
                TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_DATE"].ToString().Trim());
                TextBox_MZ_NID.Text = dt.Rows[0]["MZ_NID"].ToString().Trim();
                TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_IDATE"].ToString().Trim());
                TextBox_MZ_ADATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_ADATE"].ToString().Trim());
                //新增合格實授日欄位 20181120 by sky
                TextBox_MZ_QUA_DATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_QUA_DATE"].ToString().Trim());
                TextBox_MZ_PHONH.Text = dt.Rows[0]["MZ_PHONH"].ToString().Trim();
                TextBox_MZ_SPT.Text = dt.Rows[0]["MZ_SPT"].ToString().Trim();
                TextBox_MZ_MOVETEL.Text = dt.Rows[0]["MZ_MOVETEL"].ToString().Trim();

                //新增權理職等、是否警職人員欄位 20190514 by sky
                txt_MZ_AHP_RANK.Text = dt.Rows[0]["MZ_AHP_RANK"].ToString().Trim();
                txt_MZ_AHP_RANK1.Text = dt.Rows[0]["MZ_AHP_RANK_CH"].ToString().Trim();
                ddl_MZ_ISPOLICE.SelectedValue = dt.Rows[0]["MZ_ISPOLICE"].ToString().Trim();

                //新增薪資生效日期、是否為鑑識人員 20190530 by sky
                txt_MZ_SALARY_ISDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_SALARY_ISDATE"].ToString().Trim());
                ddl_ISCRIMELAB.SelectedValue = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT CRIMELAB From B_BASE Where IDCARD ='{0}'", dt.Rows[0]["MZ_ID"].ToString().Trim()));

                try
                {
                    DropDownList_MZ_INSURANCEMODE.SelectedValue = dt.Rows[0]["MZ_INSURANCEMODE"].ToString().Trim();
                }
                catch
                {
                    DropDownList_MZ_INSURANCEMODE.SelectedValue = "1";
                }

                //TextBox_MZ_OPFDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_OPFDATE"].ToString().Trim());
                TextBox_MZ_SLFDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_SLFDATE"].ToString().Trim());
                TextBox_MZ_SLEDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_SLEDATE"].ToString().Trim());
                TextBox_MZ_OPEDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_OPEDATE"].ToString().Trim());
                TextBox_MZ_NIN.Text = dt.Rows[0]["MZ_NIN"].ToString().Trim();
                TextBox_MZ_SLVC.Text = dt.Rows[0]["MZ_SLVC"].ToString().Trim();
                DropDownList_MZ_PNO.SelectedValue = dt.Rows[0]["MZ_PNO"].ToString().Trim();
                TextBox_MZ_PNO1.Text = dt.Rows[0]["MZ_PNO1"].ToString().Trim();
                //2013/06/20
                DropDownList_MZ_PNO_Second.SelectedValue = dt.Rows[0]["MZ_PNO_SECOND"].ToString().Trim();
                TextBox_MZ_PNO1_Second.Text = dt.Rows[0]["MZ_PNO1_SECOND"].ToString().Trim();
                //2013/06/20
                TextBox_MZ_TNO.Text = dt.Rows[0]["MZ_TNO"].ToString().Trim();
                TextBox_MZ_SPT1.Text = dt.Rows[0]["MZ_SPT1"].ToString().Trim();
                TextBox_MZ_PESN.Text = dt.Rows[0]["MZ_PESN"].ToString().Trim();
                TextBox_MZ_PHONO.Text = dt.Rows[0]["MZ_PHONO"].ToString().Trim();
                TextBox_MZ_EMNAM.Text = dt.Rows[0]["MZ_EMNAM"].ToString().Trim();
                TextBox_MZ_RET.Text = dt.Rows[0]["MZ_RET"].ToString().Trim();
                TextBox_MZ_ZONE1.Text = dt.Rows[0]["MZ_ZONE1"].ToString().Trim();
                TextBox_MZ_ZONE2.Text = dt.Rows[0]["MZ_ZONE2"].ToString().Trim();
                dll_MZ_STATUS2.SelectedValue = dt.Rows[0]["MZ_STATUS2"].ToString().Trim();

                RadioButtonList_MZ_ISEXTPOS.SelectedValue = string.IsNullOrEmpty(dt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim()) ? "N" : dt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim();

                if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
                {
                    DropDownList_MZ_EXTPOS.DataBind();
                    DropDownList_MZ_EXTPOS.SelectedValue = dt.Rows[0]["MZ_EXTPOS"].ToString().Trim();
                    TextBox_MZ_EXTPOS_SRANK.Enabled = true;
                    bt_MZ_EXTPOS_SRANK.Enabled = true;
                }
                else
                {
                    DropDownList_MZ_EXTPOS.DataBind();
                    DropDownList_MZ_EXTPOS.SelectedValue = "";
                    TextBox_MZ_EXTPOS_SRANK.Enabled = false;
                    bt_MZ_EXTPOS_SRANK.Enabled = false;
                }
                TextBox_MZ_FDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_FDATE"].ToString().Trim());
                //TextBox_MZ_ODATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_ODATE"].ToString().Trim());
                TextBox_MZ_LDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_LDATE"].ToString().Trim());
                TextBox_MZ_TDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_TDATE"].ToString().Trim());
                TextBox_MZ_PB2.Text = dt.Rows[0]["MZ_PB2"].ToString().Trim();

          

                TextBox_MZ_AD1.Text = dt.Rows[0]["MZ_AD_CH"].ToString().Trim();
                TextBox_MZ_EXAD1.Text = dt.Rows[0]["MZ_EXAD_CH"].ToString().Trim();
                TextBox_MZ_CHISI1.Text = dt.Rows[0]["MZ_CHISI_CH"].ToString().Trim();
                TextBox_MZ_EXUNIT1.Text = dt.Rows[0]["MZ_EXUNIT_CH"].ToString().Trim();
                TextBox_MZ_NREA1.Text = dt.Rows[0]["MZ_NREA_CH"].ToString().Trim();
                TextBox_MZ_OCCC1.Text = dt.Rows[0]["MZ_OCCC_CH"].ToString().Trim();
                TextBox_MZ_PCHIEF1.Text = dt.Rows[0]["MZ_PCHIEF_CH"].ToString().Trim();
                TextBox_MZ_RANK_1.Text = dt.Rows[0]["MZ_RANK_CH"].ToString().Trim();
                TextBox_MZ_RANK1_1.Text = dt.Rows[0]["MZ_RANK1_CH"].ToString().Trim();
                TextBox_MZ_SLVC1.Text = dt.Rows[0]["MZ_SLVC_CH"].ToString().Trim();
                TextBox_MZ_SRANK1.Text = dt.Rows[0]["MZ_SRANK_CH"].ToString().Trim();
                TextBox_MZ_TBCD31.Text = dt.Rows[0]["MZ_TBCD3_CH"].ToString().Trim();
                TextBox_MZ_TBCD91.Text = dt.Rows[0]["MZ_TBCD9_CH"].ToString().Trim();
                TextBox_MZ_TBDV1.Text = dt.Rows[0]["MZ_TBDV_CH"].ToString().Trim();
                TextBox_MZ_UNIT1.Text = dt.Rows[0]["MZ_UNIT_CH"].ToString().Trim();
                TextBox_MZ_PESN1.Text = dt.Rows[0]["MZ_PESN_CH"].ToString().Trim();
                TextBox_MZ_NRT1.Text = dt.Rows[0]["MZ_NRT_CH"].ToString().Trim();
                TextBox_MZ_NIN1.Text = dt.Rows[0]["MZ_NIN_CH"].ToString().Trim();
                TextBox_MZ_EXTPOS_SRANK1.Text = dt.Rows[0]["MZ_EXTPOS_SRANK_CH"].ToString().Trim();

                RadioButtonList_MZ_ABORIGINE.SelectedValue = string.IsNullOrEmpty(dt.Rows[0]["MZ_ABORIGINE"].ToString().Trim()) ? "N" : dt.Rows[0]["MZ_ABORIGINE"].ToString().Trim();

               

                if (dt.Rows[0]["MZ_ABORIGINE"].ToString().Trim() != null)
                {
                    DropDownList_MZ_ABORIGINENAME.SelectedValue = dt.Rows[0]["MZ_ABORIGINENAME"].ToString().Trim();
                }
                else
                {
                    DropDownList_MZ_ABORIGINENAME.SelectedValue = "";
                }

                RadioButtonList_MZ_ABORIGINEKIND.SelectedValue = string.IsNullOrEmpty(dt.Rows[0]["MZ_ABORIGINEKIND"].ToString().Trim()) ? "1" : dt.Rows[0]["MZ_ABORIGINEKIND"].ToString().Trim();
                TextBox_MZ_ARMYCOURSE.Text = dt.Rows[0]["MZ_ARMYCOURSE"].ToString().Trim();
                TextBox_MZ_ARMYKIND.Text = dt.Rows[0]["MZ_ARMYKIND"].ToString().Trim();
                TextBox_MZ_ARMYSTATE.Text = dt.Rows[0]["MZ_ARMYSTATE"].ToString().Trim();
                TextBox_MZ_ARMYRANK.Text = dt.Rows[0]["MZ_ARMYRANK"].ToString().Trim();

              

                TextBox_MZ_ARMYCOURSE1.Text = dt.Rows[0]["MZ_ARMYCOURSE_CH"].ToString().Trim();
                TextBox_MZ_ARMYKIND1.Text = dt.Rows[0]["MZ_ARMYKIND_CH"].ToString().Trim();
                TextBox_MZ_ARMYSTATE1.Text = dt.Rows[0]["MZ_ARMYSTATE_CH"].ToString().Trim();
                TextBox_MZ_ARMYRANK1.Text = dt.Rows[0]["MZ_ARMYRANK_CH"].ToString().Trim();

                string picPath = "SELECT PICTUREPATH FROM A_PICPATH WHERE MZ_ID='" + dt.Rows[0]["MZ_ID"].ToString() + "' ORDER BY BUDATE DESC";

                Personal_PICPATH = o_DBFactory.ABC_toTest.DataListArray(picPath, "PICTUREPATH");

                Session["Personal_PICPATH"] = Personal_PICPATH;
            }

            btUpperPic.Enabled = false;
            btNextPic.Enabled = false;

            btUpdate.Enabled = true;
            btDelete.Enabled = true;

            ViewState["ClothEXAD"] = TextBox_MZ_EXAD.Text;
            ViewState["ClothEXUnit"] = TextBox_MZ_EXUNIT.Text;
        }
        //下一筆
        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));
                xcount1.Text = "0";
                findPic(int.Parse(xcount1.Text));

                if (int.Parse(xcount.Text) == dataNext.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));
                xcount1.Text = "0";
                findPic(int.Parse(xcount1.Text));

                if (int.Parse(xcount.Text) == dataNext.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + dataNext.Count.ToString() + "筆";
        }
        //上一筆
        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                xcount1.Text = "0";
                findPic(int.Parse(xcount1.Text));
                if (int.Parse(xcount.Text) != dataNext.Count - 1)
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
                xcount1.Text = "0";
                findPic(int.Parse(xcount1.Text));
                btUpper.Enabled = false;
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + dataNext.Count.ToString() + "筆";
        }
        /// <summary>
        /// 編制機關,單位 現服機關,單位 姓名,身分證號狀態切換
        /// </summary>
        protected void upper_Table_Enable_False()
        {

            TextBox_MZ_ID.Enabled = false;
            TextBox_MZ_NAME.Enabled = false;
            TextBox_MZ_AD.Enabled = false;
            TextBox_MZ_EXAD.Enabled = false;
            TextBox_MZ_EXUNIT.Enabled = false;
            TextBox_MZ_UNIT.Enabled = false;

            // sam 20200630 新增發薪機關
            TextBox_PAY_AD.Enabled = false;
            TextBox_PAY_AD1.Enabled = false;    

            btAD.Enabled = false;
            btEXAD.Enabled = false;
            btPAY_AD.Enabled = false;
            btUNIT.Enabled = false;
            btEXUNIT.Enabled = false;
        }
        /// <summary>
        /// 編制機關,單位 現服機關,單位 姓名,身分證號狀態切換
        /// </summary>
        protected void upper_Table_Enable_True()
        {
            if (strGID == "C" || strGID == "B" || strGID == "A")
            {
                TextBox_MZ_ID.Enabled = true;
                TextBox_MZ_NAME.Enabled = true;
                TextBox_MZ_AD.Enabled = true;
                TextBox_MZ_EXAD.Enabled = true;
                TextBox_MZ_EXUNIT.Enabled = true;
                TextBox_MZ_UNIT.Enabled = true;
                btAD.Enabled = true;
                btEXAD.Enabled = true;
                btUNIT.Enabled = true;
                btEXUNIT.Enabled = true;
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        protected void btInsert_Click(object sender, EventArgs e)
        {
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            ViewState["Mode"] = "INSERT";//紀錄現在是新增或是修改狀態

            if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NAME.ClientID + "').focus();$get('" + TextBox_MZ_NAME.ClientID + "').focus();", true);
                //TextBox_MZ_NAME.Focus();
                btNow.CausesValidation = true;
                btCareer.Enabled = false;
                btEducation.Enabled = false;
                btefficiency.Enabled = false;
                btExam.Enabled = false;
                btFamily.Enabled = false;
                btCard.Enabled = false;
                btefficiency_insert.Enabled = false;
                //20150324
                btefficiency.Enabled = false;
                upper_Table_Enable_True();
                A.controlEnable(ref this.Panel_Title, true);
                A.controlEnable(ref this.Panel_Basic, true);
                A.controlEnable(ref this.Panel_Now, true);
                ABORIGINE();
                EXTPOS();
                //清空控件資料
                foreach (object obj in Panel_Title.Controls)
                {
                    if (obj is TextBox)
                    {
                        TextBox tbox = obj as TextBox;
                        tbox.Text = string.Empty;
                    }
                    else if (obj is DropDownList)
                    {
                        DropDownList ddlist = obj as DropDownList;

                        ddlist.Text = "";

                    }
                }

                foreach (object obj in Panel_Basic.Controls)
                {
                    if (obj is TextBox)
                    {
                        TextBox tbox = obj as TextBox;
                        tbox.Text = string.Empty;
                    }
                    else if (obj is DropDownList)
                    {
                        DropDownList ddlist = obj as DropDownList;
                        if (ddlist.ID == "DropDownList_MZ_INSURANCEMODE" || ddlist.ID == "DropDownList_MZ_SEX" || ddlist.ID == "DropDownList_MZ_SM")//性別,婚姻狀況不清
                        {
                        }
                        else
                        {
                            ddlist.Text = "";
                        }
                    }
                }

                RadioButtonList_MZ_ABORIGINE.SelectedValue = "N";
                DropDownList_MZ_ABORIGINENAME.Enabled = false;
                RadioButtonList_MZ_ABORIGINEKIND.Enabled = false;

                foreach (object obj in Panel_Now.Controls)
                {
                    if (obj is TextBox)
                    {
                        TextBox tbox = obj as TextBox;
                        tbox.Text = string.Empty;
                    }
                    else if (obj is DropDownList)
                    {
                        DropDownList ddlist = obj as DropDownList;

                        ddlist.Text = "";
                    }
                }

                dll_MZ_STATUS2.SelectedValue = "Y";
                //[新增]SQL語法！
                
                sqlString = "INSERT INTO A_DLBASE(MZ_NAME,MZ_ID,MZ_BIR,MZ_TBCD3,MZ_CITY,MZ_BL,MZ_SM,MZ_SEX,MZ_ADD1,MZ_ADD2,MZ_FDATE," +
                                                                  " MZ_PHONE,MZ_OFFYY,MZ_FYEAR,MZ_TBCD9,MZ_POLNO,MZ_AD,MZ_UNIT,MZ_EXAD," +
                                                                  " MZ_EXUNIT,MZ_RANK,MZ_RANK1,MZ_OCCC,MZ_CHISI,MZ_POSIND,MZ_NRT,MZ_TBDV," +
                                                                  " MZ_PCHIEF,MZ_NREA,MZ_DATE,MZ_NID,MZ_IDATE,MZ_ADATE,MZ_PHONH,MZ_SPT,MZ_SRANK," +
                    /*" MZ_OPFDATE,*/" MZ_SLFDATE,MZ_SLEDATE,MZ_NIN,MZ_SLVC,MZ_PNO,MZ_SPT1,MZ_OPEDATE,MZ_PB2," +
                                                                  " MZ_PESN,MZ_PHONO,MZ_EMNAM,MZ_RET,MZ_ZONE1,MZ_ZONE2,MZ_ABORIGINE,MZ_ENAME,MZ_TNO," +
                                                                  " MZ_ARMYSTATE,MZ_ARMYRANK,MZ_ARMYKIND,MZ_ARMYCOURSE,MZ_ABORIGINENAME," +
                                                                  " MZ_PNO1,MZ_ISEXTPOS,MZ_EXTPOS,PAY_AD,MZ_ODATE,MZ_LDATE,MZ_TDATE,MZ_INSURANCEMODE,MZ_POWER,MZ_ABORIGINEKIND,MZ_MOVETEL,MZ_STATUS2,MZ_EXTPOS_SRANK,MUSER,MDATE,MZ_PNO_SECOND ,MZ_PNO1_SECOND," +
                                                                  " MZ_QUA_DATE, MZ_AHP_RANK, MZ_ISPOLICE, MZ_SALARY_ISDATE, MZ_TRAINING)" +
                                                          " values (@MZ_NAME,@MZ_ID,@MZ_BIR,@MZ_TBCD3,@MZ_CITY,@MZ_BL,@MZ_SM,@MZ_SEX,@MZ_ADD1,@MZ_ADD2,@MZ_FDATE, " +
                                                                  " @MZ_PHONE,@MZ_OFFYY,@MZ_FYEAR,@MZ_TBCD9,@MZ_POLNO,@MZ_AD,@MZ_UNIT,@MZ_EXAD," +
                                                                  " @MZ_EXUNIT,@MZ_RANK,@MZ_RANK1,@MZ_OCCC,@MZ_CHISI,@MZ_POSIND,@MZ_NRT,@MZ_TBDV," +
                                                                  " @MZ_PCHIEF,@MZ_NREA,@MZ_DATE,@MZ_NID,@MZ_IDATE,@MZ_ADATE,@MZ_PHONH,@MZ_SPT,@MZ_SRANK," +
                    /*" @MZ_OPFDATE,*/" @MZ_SLFDATE,@MZ_SLEDATE,@MZ_NIN,@MZ_SLVC,@MZ_PNO,@MZ_SPT1,@MZ_OPEDATE,@MZ_PB2," +
                                                                  " @MZ_PESN,@MZ_PHONO,@MZ_EMNAM,@MZ_RET,@MZ_ZONE1,@MZ_ZONE2,@MZ_ABORIGINE,@MZ_ENAME,@MZ_TNO," +
                                                                  " @MZ_ARMYSTATE,@MZ_ARMYRANK,@MZ_ARMYKIND,@MZ_ARMYCOURSE,@MZ_ABORIGINENAME," +
                                                                  " @MZ_PNO1,@MZ_ISEXTPOS,@MZ_EXTPOS,@PAY_AD,@MZ_ODATE,@MZ_LDATE,@MZ_TDATE,@MZ_INSURANCEMODE,@MZ_POWER,@MZ_ABORIGINEKIND,@MZ_MOVETEL,@MZ_STATUS2,@MZ_EXTPOS_SRANK,@MUSER,@MDATE ,@MZ_PNO_SECOND ,@MZ_PNO1_SECOND," +
                                                                  " @MZ_QUA_DATE, @MZ_AHP_RANK, @MZ_ISPOLICE, @MZ_SALARY_ISDATE, @MZ_TRAINING)";


                txt_Account.Enabled = false;
                //新增人員無法設定是否鑑識人員，請由修改操作。 20190530 by sky
                ddl_ISCRIMELAB.Enabled = false;
            }
            else if (MultiView1.ActiveViewIndex == 2)//學歷
            {
                sqlString = "Insert into A_EDUCATION(MZ_ID,MZ_SCHOOL,MZ_DEPARTMENT,MZ_YEAR,MZ_BEGINDATE,MZ_ENDDATE,MZ_EDUKIND,MZ_EDULEVEL,MZ_EDUCLASS) " +
                                           "  VALUES(@MZ_ID,@MZ_SCHOOL,@MZ_DEPARTMENT,@MZ_YEAR,@MZ_BEGINDATE,@MZ_ENDDATE,@MZ_EDUKIND,@MZ_EDULEVEL,@MZ_EDUCLASS)";
                GridView_Edu.Visible = false;
                Panel_TableEdu.Visible = true;
                TextBox_MZ_EDUKIND.Text = "1";
                TextBox_MZ_EDUKIND1.Text = "畢業";
            }
            else if (MultiView1.ActiveViewIndex == 3)
            {
                sqlString = "INSERT INTO A_EXAM(MZ_ID,EXAM_NAME,EXAM_CLASS,EXAM_ADMISSION,EXAM_DOCUMENTS,EXAM_YEAR)" +
                                       " VALUES(@MZ_ID,@EXAM_NAME,@EXAM_CLASS,@EXAM_ADMISSION,@EXAM_DOCUMENTS,@EXAM_YEAR)";
                GridView_Exam.Visible = false;

                Panel_Exam.Visible = true;
            }

            else if (MultiView1.ActiveViewIndex == 4)
            {
                sqlString = "INSERT INTO A_CAREER(MZ_AD,MZ_UNIT,MZ_OCCC,MZ_ID,MZ_RANK,MZ_RANK1,MZ_TBDV,MZ_CHISI,MZ_POSIND,MZ_PESN,MZ_PCHIEF," +
                                                 "MZ_NRT,MZ_DATE,MZ_IDATE,MZ_ADATE,MZ_TBDATE,MZ_TBID,MZ_EXID,MZ_NIN,MZ_NREA,MZ_TBNREA,MZ_EXTPOS,MZ_ISEXTPOS,SN)" +
                                         " VALUES(@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_ID,@MZ_RANK,@MZ_RANK1,@MZ_TBDV,@MZ_CHISI,@MZ_POSIND,@MZ_PESN,@MZ_PCHIEF," +
                                                 ":MZ_NRT,@MZ_DATE,@MZ_IDATE,@MZ_ADATE,@MZ_TBDATE,@MZ_TBID,@MZ_EXID,@MZ_NIN,@MZ_NREA,@MZ_TBNREA,@MZ_EXTPOS,@MZ_ISEXTPOS,nextval)";
                GridView_Career.Visible = false;
                Panel_Career.Visible = true;
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                sqlString = "Insert into A_EFFICIENCY(MZ_ID,T01,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27,T28,T29,T30,T31,T32)" +
                                        "VALUES(@MZ_ID,@T01,@T17,@T18,@T19,@T20,@T21,@T22,@T23,@T24,@T25,@T26,@T27,@T28,@T29,@T30,@T31,@T32)";
                GridView_efficiency.Visible = false;
                Panel_efficiency.Visible = true;
            }
            else if (MultiView1.ActiveViewIndex == 6)
            {
                sqlString = "Insert into A_FAMILY(MZ_ID,MZ_FAMILYID,MZ_FAMILYNAME,MZ_BIRTHDAY,MZ_WORK,MZ_TITLE,MZ_ISINSURANCE,MZ_INSURANCEMODE)" +
                                         " VALUES(@MZ_ID,@MZ_FAMILYID,@MZ_FAMILYNAME,@MZ_BIRTHDAY,@MZ_WORK,@MZ_TITLE,@MZ_ISINSURANCE,@MZ_INSURANCEMODE) ";


                GridView_FAMILY.Visible = false;
                Panel_FAMILY.Visible = true;
            }
            else if (MultiView1.ActiveViewIndex == 7)
            {
                sqlString = "INSERT INTO A_POLICE(MZ_IDNO,MZ_ID,MZ_SWT,MZ_DATE,MZ_MEMO1,MZ_ODATE,MZ_EDATE,MZ_MEMO,MZ_NO1,MZ_INO)" +
                                         " VALUES(@MZ_IDNO,@MZ_ID,@MZ_SWT,@MZ_RANK,@MZ_RANK1,@MZ_DATE,@MZ_MEMO1,@MZ_ODATE,@MZ_EDATE,@MZ_MEMO,@MZ_NO1,@MZ_INO)";

            }


            ViewState["sqlString"] = sqlString;//新增SQL語法..依MultiView的頁面不同更換
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            if (MultiView1.ActiveViewIndex == 0) //新增基本資料時！確認鍵到現職頁才會出現
            {
            }
            else
            {
                btOK.Enabled = true;
            }

            btCancel.Enabled = true;
        }
        //修改
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";//紀錄是新增或是修改狀態
            if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
            {
                if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'")))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                    return;
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NAME.ClientID + "').focus();$get('" + TextBox_MZ_NAME.ClientID + "').focus();", true);
                //TextBox_MZ_NAME.Focus();//新增預設停在姓名欄位
                btNow.CausesValidation = true;//新增或修改時 切換至現職頁面時 要引發控制項驗證

                sqlString = "UPDATE A_DLBASE SET MZ_NAME = @MZ_NAME,MZ_ID = @MZ_ID,MZ_AD = @MZ_AD,MZ_UNIT = @MZ_UNIT,MZ_EXAD = @MZ_EXAD," +
                                                "MZ_EXUNIT = @MZ_EXUNIT,MZ_OCCC = @MZ_OCCC,MZ_RANK = @MZ_RANK,MZ_RANK1 = @MZ_RANK1,MZ_CHISI = @MZ_CHISI," +
                                                "MZ_POSIND = @MZ_POSIND,MZ_TBDV = @MZ_TBDV,MZ_SRANK = @MZ_SRANK,MZ_SLVC = @MZ_SLVC,MZ_SPT = @MZ_SPT," +
                                                "MZ_SPT1 = @MZ_SPT1,MZ_SEX = @MZ_SEX,MZ_BIR = @MZ_BIR,MZ_TBCD3 = @MZ_TBCD3,MZ_CITY = @MZ_CITY,MZ_BL = @MZ_BL," +
                                                "MZ_SM = @MZ_SM,MZ_SLFDATE = @MZ_SLFDATE,MZ_SLEDATE = @MZ_SLEDATE,MZ_OFFYY = @MZ_OFFYY,"/*MZ_OFFMM = @MZ_OFFMM,*/+
                                                "MZ_TBCD9 = @MZ_TBCD9,MZ_FYEAR = @MZ_FYEAR,MZ_ZONE1 = @MZ_ZONE1,MZ_ADD1 = @MZ_ADD1,MZ_ZONE2 = @MZ_ZONE2,MZ_ADD2 = @MZ_ADD2," +
                                                "MZ_PHONE = @MZ_PHONE,MZ_MOVETEL = @MZ_MOVETEL,MZ_PCHIEF = @MZ_PCHIEF,MZ_NREA = @MZ_NREA,MZ_DATE = @MZ_DATE,MZ_IDATE = @MZ_IDATE," +
                                                "MZ_ADATE = @MZ_ADATE,MZ_NID = @MZ_NID,MZ_NRT = @MZ_NRT,MZ_NIN = @MZ_NIN,MZ_PESN = @MZ_PESN,MZ_RET = @MZ_RET," +//MZ_OPFDATE = @MZ_OPFDATE,"+
                                                "MZ_OPEDATE = @MZ_OPEDATE,MZ_EMNAM = @MZ_EMNAM,MZ_PHONO = @MZ_PHONO,MZ_PHONH = @MZ_PHONH,MZ_FDATE = @MZ_FDATE,MZ_ODATE = @MZ_ODATE," +
                   "MZ_PB2 = @MZ_PB2," + /*MZ_SUP = @MZ_SUP,MZ_STATUS2 = @MZ_STATUS2,*/"MZ_POLNO = @MZ_POLNO,MZ_PNO = @MZ_PNO,MZ_TNO = @MZ_TNO,MZ_ABORIGINE = @MZ_ABORIGINE," +
                                                "MZ_ARMYSTATE = @MZ_ARMYSTATE,MZ_ARMYRANK = @MZ_ARMYRANK,MZ_ARMYKIND = @MZ_ARMYKIND,MZ_ARMYCOURSE = @MZ_ARMYCOURSE,MZ_ABORIGINENAME = @MZ_ABORIGINENAME," +
                                                "MZ_ENAME = @MZ_ENAME,MZ_PNO1 = @MZ_PNO1,MZ_ISEXTPOS = @MZ_ISEXTPOS,MZ_EXTPOS = @MZ_EXTPOS,MZ_STATUS2=@MZ_STATUS2,PAY_AD = @PAY_AD, " +
                                                "MZ_LDATE=@MZ_LDATE,MZ_TDATE=@MZ_TDATE,MZ_INSURANCEMODE=@MZ_INSURANCEMODE,MZ_ABORIGINEKIND=@MZ_ABORIGINEKIND,MZ_EXTPOS_SRANK=@MZ_EXTPOS_SRANK,MUSER=@MUSER,MDATE=@MDATE ,MZ_PNO_SECOND = @MZ_PNO_SECOND, MZ_PNO1_SECOND = @MZ_PNO1_SECOND," +
                                                "MZ_QUA_DATE=@MZ_QUA_DATE, " +
                                                "MZ_AHP_RANK=@MZ_AHP_RANK, MZ_ISPOLICE=@MZ_ISPOLICE, MZ_SALARY_ISDATE=@MZ_SALARY_ISDATE, MZ_TRAINING=@MZ_TRAINING " +
                                                "WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";

                Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;

                //改為合格實授日計算休假 20181120 by sky
                //Session["PKEY_MZ_FDATE"] = TextBox_MZ_FDATE.Text;
                Session["PKEY_MZ_QUA_DATE"] = TextBox_MZ_QUA_DATE.Text;

                Session["Personal1-1_EXAD"] = TextBox_MZ_EXAD.Text;

                Session["Personal1-1_EXUNIT"] = TextBox_MZ_EXUNIT.Text;

                btCareer.Enabled = false;
                btEducation.Enabled = false;
                btefficiency.Enabled = false;
                btExam.Enabled = false;
                btFamily.Enabled = false;
                btCard.Enabled = false;

                upper_Table_Enable_True();

                btefficiency_insert.Enabled = false;

                //20150324
                btefficiency.Enabled = false;
                if (strGID == "D" || strGID == "E")
                    A.controlEnable(ref this.Panel_Title, false);
                else
                    A.controlEnable(ref this.Panel_Title, true);

                //開啟View內Panal的控制項
                A.controlEnable(ref this.Panel_Basic, true);

                //102.02.26  權限D.E鎖定部分控制項
                if (strGID == "D" || strGID == "E")
                {
                    TextBox_MZ_POLNO.Enabled = false;
                    TextBox_MZ_BIR.Enabled = false;
                    TextBox_MZ_OFFYY.Enabled = false;
                    TextBox_MZ_ADATE.Enabled = false;
                    TextBox_MZ_QUA_DATE.Enabled = false; //新增合格實授日欄位 20181120 by sky
                    TextBox_MZ_LDATE.Enabled = false;
                    TextBox_MZ_TDATE.Enabled = false;
                    TextBox_MZ_ODAY.Enabled = false;

                }
                //


                A.controlEnable(ref this.Panel_Now, true);
                ABORIGINE();
                EXTPOS();

                txt_Account.Enabled = false;

                //修改時不可變更編制機關 by Nick 20170302
                TextBox_MZ_AD.Enabled = false;
                btAD.Enabled = false;
                TextBox_MZ_AD1.Enabled = false;
            }

            #region 修改--學歷
            else if (MultiView1.ActiveViewIndex == 2)//學歷
            {
                if (GridView_Edu.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                    return;
                }
                else if (GridView_Edu.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                    return;
                }
                else if (GridView_Edu.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_Edu.SelectedRow == null)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                    return;
                }
                else
                {

                 Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;
                 string selectSql = " SELECT * " +
                                       " FROM A_EDUCATION " +
                                       " WHERE " +
                                       "  MZ_ID='" + TextBox_MZ_ID.Text.Trim() +
                                       "' AND MZ_DEPARTMENT='" + GridView_Edu.DataKeys[GridView_Edu.SelectedRow.RowIndex]["DEPT"].ToString() + "' AND MZ_SCHOOL='" + GridView_Edu.SelectedRow.Cells[0].Text.Trim() + "'";

                    DataTable selectValue = new DataTable();
                    selectValue = o_DBFactory.ABC_toTest.Create_Table(selectSql, "123");
                    TextBox_MZ_YEAR.Text = selectValue.Rows[0]["MZ_YEAR"].ToString();
                    TextBox_MZ_BEGINDATE.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_BEGINDATE"].ToString());
                    TextBox_MZ_ENDDATE.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_ENDDATE"].ToString());
                    TextBox_MZ_EDUKIND.Text = selectValue.Rows[0]["MZ_EDUKIND"].ToString();
                    TextBox_MZ_EDULEVEL.Text = selectValue.Rows[0]["MZ_EDULEVEL"].ToString();
                    TextBox_MZ_EDUCLASS.Text = selectValue.Rows[0]["MZ_EDUCLASS"].ToString();
                    TextBox_MZ_SCHOOL1.Text = selectValue.Rows[0]["MZ_SCHOOL"].ToString(); ;
                    TextBox_MZ_EDUKIND1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EDUKIND.Text, "EDT");
                    TextBox_MZ_EDULEVEL1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EDULEVEL.Text, "EDL");
                    TextBox_MZ_EDUCLASS1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EDUCLASS.Text, "@14");
                    TextBox_MZ_DEPARTMENT1.Text = selectValue.Rows[0]["MZ_DEPARTMENT"].ToString();

                    sqlString = "UPDATE A_EDUCATION SET MZ_ID=@MZ_ID,MZ_SCHOOL=@MZ_SCHOOL,MZ_DEPARTMENT=@MZ_DEPARTMENT,MZ_YEAR=@MZ_YEAR,MZ_BEGINDATE=@MZ_BEGINDATE," +
                                                 " MZ_ENDDATE=@MZ_ENDDATE,MZ_EDUKIND=@MZ_EDUKIND,MZ_EDULEVEL=@MZ_EDULEVEL,MZ_EDUCLASS=@MZ_EDUCLASS" +
                                                 " WHERE " +
                                                 " MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_DEPARTMENT='" + TextBox_MZ_DEPARTMENT1.Text.Trim() + "' AND MZ_SCHOOL='" + TextBox_MZ_SCHOOL1.Text.Trim() + "'";

   

                    Session["PKEY_MZ_SCHOOL"] = TextBox_MZ_SCHOOL.Text;

                    GridView_Edu.Visible = false;
                    Panel_TableEdu.Visible = true;
                }
            }
            #endregion 修改--學歷

           
            #region 修改--考試
            else if (MultiView1.ActiveViewIndex == 3)
            {

                if (GridView_Exam.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                    return;
                }
                else if (GridView_Exam.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                    return;
                }
                else if (GridView_Exam.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_Exam.SelectedRow == null)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                    return;
                }
                else
                {

                    sqlString = "UPDATE A_EXAM " +
                            "SET " +
                                  "MZ_ID=@MZ_ID,EXAM_NAME=@EXAM_NAME,EXAM_CLASS=@EXAM_CLASS,EXAM_ADMISSION=@EXAM_ADMISSION,EXAM_DOCUMENTS=@EXAM_DOCUMENTS,EXAM_YEAR=@EXAM_YEAR " +
                            "WHERE " +
                                  "MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND EXAM_NAME=(SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI='" + GridView_Exam.SelectedRow.Cells[1].Text.Trim() + "')";

                    Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;



                    string selectSql = "SELECT * FROM A_EXAM WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND EXAM_NAME=(SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI='" + GridView_Exam.SelectedRow.Cells[1].Text.Trim() + "')";
                    DataTable selectValue = new DataTable();
                    selectValue = o_DBFactory.ABC_toTest.Create_Table(selectSql, "123");

                    TextBox_EXAM_NAME.Text = selectValue.Rows[0]["EXAM_NAME"].ToString();
                    TextBox_EXAM_CLASS.Text = selectValue.Rows[0]["EXAM_CLASS"].ToString();
                    TextBox_EXAM_ADMISSION.Text = selectValue.Rows[0]["EXAM_ADMISSION"].ToString();
                    TextBox_EXAM_DOCUMENTS.Text = selectValue.Rows[0]["EXAM_DOCUMENTS"].ToString();
                    TextBox_EXAM_YEAR.Text = selectValue.Rows[0]["EXAM_YEAR"].ToString();

                    TextBox_EXAM_NAME1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_EXAM_NAME.Text, "EXK");
                    TextBox_EXAM_CLASS1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_EXAM_CLASS.Text, "EXS");
                    TextBox_EXAM_ADMISSION1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_EXAM_ADMISSION.Text, "EXG");

                    Session["PKEY_EXAM_NAME"] = TextBox_EXAM_NAME.Text;

                    GridView_Exam.Visible = false;
                    Panel_Exam.Visible = true;
                }
            }
            #endregion 修改--考試

            
            #region 修改--經歷

            else if (MultiView1.ActiveViewIndex == 4)
            {
                if (GridView_Career.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                    return;
                }
                else if (GridView_Career.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                    return;
                }
                else if (GridView_Career.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_Career.SelectedRow == null)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                    return;
                }
                else
                {

                    sqlString = "UPDATE A_CAREER SET MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_OCCC=@MZ_OCCC,MZ_ID=@MZ_ID,MZ_RANK=@MZ_RANK,MZ_RANK1=@MZ_RANK1,MZ_TBDV=@MZ_TBDV,MZ_CHISI=@MZ_CHISI,MZ_POSIND=@MZ_POSIND,MZ_PESN=@MZ_PESN,MZ_PCHIEF=@MZ_PCHIEF," +
                                                 "MZ_NRT=@MZ_NRT,MZ_DATE=@MZ_DATE,MZ_IDATE=@MZ_IDATE,MZ_ADATE=@MZ_ADATE,MZ_TBDATE=@MZ_TBDATE,MZ_TBID=@MZ_TBID,MZ_EXID=@MZ_EXID,MZ_NIN=@MZ_NIN,MZ_EXTPOS=@MZ_EXTPOS,MZ_ISEXTPOS=@MZ_ISEXTPOS,MZ_TBNREA=@MZ_TBNREA,MZ_NREA=@MZ_NREA " +
                                                 " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_AD='" + GridView_Career.SelectedRow.Cells[0].Text + "' AND MZ_UNIT='" + GridView_Career.SelectedRow.Cells[1].Text + "' AND MZ_OCCC='" + GridView_Career.SelectedRow.Cells[2].Text + "'";

                    Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;

                    string selectSql = "SELECT * FROM A_CAREER WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_AD= '" + GridView_Career.SelectedRow.Cells[0].Text.Trim() + "' AND MZ_UNIT= '" + GridView_Career.SelectedRow.Cells[1].Text.Trim() + "'";
                    DataTable selectValue = new DataTable();
                    selectValue = o_DBFactory.ABC_toTest.Create_Table(selectSql, "123");

                    TextBox_CAREER_AD.Text = selectValue.Rows[0]["MZ_AD"].ToString().Trim();
                    TextBox_CAREER_ADATE.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_ADATE"].ToString().Trim());
                    TextBox_CAREER_CHISI.Text = selectValue.Rows[0]["MZ_CHISI"].ToString().Trim();
                    TextBox_CAREER_DATE.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_DATE"].ToString().Trim());
                    TextBox_CAREER_EXID.Text = selectValue.Rows[0]["MZ_EXID"].ToString().Trim();
                    TextBox_CAREER_IDATE.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_IDATE"].ToString().Trim());
                    TextBox_CAREER_NIN.Text = selectValue.Rows[0]["MZ_NIN"].ToString().Trim();
                    TextBox_CAREER_NREA.Text = selectValue.Rows[0]["MZ_NREA"].ToString().Trim();
                    TextBox_CAREER_NRT.Text = selectValue.Rows[0]["MZ_NRT"].ToString().Trim();
                    TextBox_CAREER_OCCC.Text = selectValue.Rows[0]["MZ_OCCC"].ToString().Trim();
                    TextBox_CAREER_PCHIEF.Text = selectValue.Rows[0]["MZ_PCHIEF"].ToString().Trim();
                    TextBox_CAREER_PESN.Text = selectValue.Rows[0]["MZ_PESN"].ToString().Trim();
                    TextBox_CAREER_POSIND.Text = selectValue.Rows[0]["MZ_POSIND"].ToString().Trim();
                    TextBox_CAREER_RANK.Text = selectValue.Rows[0]["MZ_RANK"].ToString().Trim();
                    TextBox_CAREER_RANK1.Text = selectValue.Rows[0]["MZ_RANK1"].ToString().Trim();
                    TextBox_CAREER_TBDATE.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_TBDATE"].ToString().Trim());
                    TextBox_CAREER_TBDV.Text = selectValue.Rows[0]["MZ_TBDV"].ToString().Trim();
                    TextBox_CAREER_TBID.Text = selectValue.Rows[0]["MZ_TBID"].ToString().Trim();
                    TextBox_CAREER_TBNREA.Text = selectValue.Rows[0]["MZ_TBNREA"].ToString().Trim();
                    TextBox_CAREER_UNIT.Text = selectValue.Rows[0]["MZ_UNIT"].ToString().Trim();
                    DropDownList_CAREER_EXTPOS.DataBind();
                    DropDownList_CAREER_EXTPOS.SelectedValue = selectValue.Rows[0]["MZ_EXTPOS"].ToString().Trim();
                    RadioButtonList_CAREER_ISEXTPOS.SelectedValue = string.IsNullOrEmpty(selectValue.Rows[0]["MZ_ISEXTPOS"].ToString().Trim()) ? "N" : selectValue.Rows[0]["MZ_ISEXTPOS"].ToString().Trim();

                    TextBox_CAREER_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_CAREER_AD.Text.Trim()) + "'");
                    TextBox_CAREER_UNIT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_UNIT.Text, "25");
                    TextBox_CAREER_RANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_RANK.Text, "09");
                    TextBox_CAREER_RANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_RANK1.Text, "09");
                    TextBox_CAREER_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_OCCC.Text, "26");
                    TextBox_CAREER_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_TBDV.Text, "43");
                    TextBox_CAREER_CHISI.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_CHISI.Text, "23");
                    TextBox_CAREER_PCHIEF1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_PCHIEF.Text, "56");
                    TextBox_CAREER_NREA1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_NREA.Text, "11");
                    TextBox_CAREER_TBNREA1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_TBNREA.Text, "11");
                    TextBox_CAREER_PESN1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_PESN.Text, "05");
                    TextBox_CAREER_NRT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_NRT.Text, "53");
                    TextBox_CAREER_NIN1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_NIN.Text, "54");

                    Session["PKEY_CAREER_AD"] = TextBox_CAREER_AD.Text;

                    Session["PKEY_CAREER_UNIT"] = TextBox_CAREER_UNIT.Text;

                    Session["PKEY_CAREER_OCCC"] = TextBox_CAREER_OCCC.Text;

                    GridView_Career.Visible = false;
                    Panel_Career.Visible = true;
                }
            }
            #endregion 修改--經歷

           
            #region 修改--考績
            else if (MultiView1.ActiveViewIndex == 5)
            {

                if (GridView_efficiency.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                    return;
                }
                else if (GridView_efficiency.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                    return;
                }
                else if (GridView_efficiency.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_efficiency.SelectedRow == null)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                    return;
                }

                else
                {
                    sqlString = "UPDATE A_EFFICIENCY SET T01=@T01,T17=@T17,T18=@T18,T19=@T19,T20=@T20,T21=@T21,T22=@T22,T23=@T23,T24=@T24," +
                                               "T25=@T25,T26=@T26,T27=@T27,T28=@T28,T29=@T29,T30=@T30,T31=@T31,T32=@T32" +
                                              " WHERE  MZ_ID=@MZ_ID AND T01 = '" + Label_Year.Text + "'";
                    string selectSql = "SELECT * FROM A_EFFICIENCY WHERE MZ_ID ='" + TextBox_MZ_ID.Text.Trim() + "'  AND T01 = '" + Label_Year.Text + "'";

                    DataTable selectValue = new DataTable();
                    selectValue = o_DBFactory.ABC_toTest.Create_Table(selectSql, "123");
                    TextBox.Text = selectValue.Rows[0]["T01"].ToString();
                    TextBox17.Text = selectValue.Rows[0]["T17"].ToString();
                    TextBox18.Text = selectValue.Rows[0]["T18"].ToString();
                    TextBox19.Text = selectValue.Rows[0]["T19"].ToString();
                    TextBox20.Text = selectValue.Rows[0]["T20"].ToString();
                    TextBox21.Text = selectValue.Rows[0]["T21"].ToString();
                    TextBox22.Text = selectValue.Rows[0]["T22"].ToString();
                    TextBox23.Text = selectValue.Rows[0]["T23"].ToString();
                    TextBox24.Text = selectValue.Rows[0]["T24"].ToString();
                    TextBox25.Text = selectValue.Rows[0]["T25"].ToString();
                    TextBox26.Text = selectValue.Rows[0]["T26"].ToString();
                    TextBox27.Text = selectValue.Rows[0]["T27"].ToString();
                    TextBox28.Text = selectValue.Rows[0]["T28"].ToString();
                    TextBox29.Text = selectValue.Rows[0]["T29"].ToString();
                    TextBox30.Text = selectValue.Rows[0]["T30"].ToString();
                    TextBox31.Text = selectValue.Rows[0]["T31"].ToString();
                    TextBox32.Text = selectValue.Rows[0]["T32"].ToString();

                    GridView_efficiency.Visible = false;
                    Panel_efficiency.Visible = true;
                }
            }
            #endregion 修改--考績

           
            #region 修改--眷屬
            else if (MultiView1.ActiveViewIndex == 6)
            {

                if (GridView_FAMILY.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                    return;
                }
                else if (GridView_FAMILY.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                    return;
                }
                else if (GridView_FAMILY.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_FAMILY.SelectedRow == null)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                    return;
                }
                else
                {
                    sqlString = "UPDATE A_FAMILY SET MZ_ID=@MZ_ID,MZ_FAMILYID=@MZ_FAMILYID,MZ_FAMILYNAME=@MZ_FAMILYNAME,MZ_BIRTHDAY=@MZ_BIRTHDAY," +
                                               " MZ_WORK=@MZ_WORK,MZ_TITLE=@MZ_TITLE,MZ_ISINSURANCE=@MZ_ISINSURANCE,MZ_INSURANCEMODE=@MZ_INSURANCEMODE " +
                                               " WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_FAMILYID='" + GridView_FAMILY.SelectedRow.Cells[0].Text.Trim() + "'";

                    Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;

                    string selectSql = "SELECT * FROM A_FAMILY WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_FAMILYID='" + GridView_FAMILY.SelectedRow.Cells[0].Text.Trim() + "'";
                    DataTable selectValue = new DataTable();
                    selectValue = o_DBFactory.ABC_toTest.Create_Table(selectSql, "123");

                    TextBox_MZ_FAMILYID.Text = selectValue.Rows[0]["MZ_FAMILYID"].ToString();
                    TextBox_MZ_FAMILYNAME.Text = selectValue.Rows[0]["MZ_FAMILYNAME"].ToString();
                    TextBox_MZ_BIRTHDAY.Text = o_CommonService.Personal_ReturnDateString(selectValue.Rows[0]["MZ_BIRTHDAY"].ToString());
                    TextBox_MZ_WORK.Text = selectValue.Rows[0]["MZ_WORK"].ToString();
                    TextBox_MZ_TITLE.Text = selectValue.Rows[0]["MZ_TITLE"].ToString();
                    RadioButtonList_MZ_ISINSURANCE.SelectedValue = selectValue.Rows[0]["MZ_ISINSURANCE"].ToString();
                    DropDownList_Family_INSURANCEMODE.SelectedValue = selectValue.Rows[0]["MZ_INSURANCEMODE"].ToString();

                    TextBox_MZ_TITLE1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TITLE.Text, "FAP");
                    TextBox_MZ_WORK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_WORK.Text, "@03");

                    Session["PKEY_MZ_FAMILYID"] = TextBox_MZ_FAMILYID.Text;

                    GridView_FAMILY.Visible = false;
                    Panel_FAMILY.Visible = true;
                }
            }
            #endregion 修改--眷屬


            #region 修改--服務證
            else if (MultiView1.ActiveViewIndex == 7)
            {
                sqlString = "UPDATE A_POLICE SET MZ_IDNO=@MZ_IDNO,MZ_ID=@MZ_ID,MZ_SWT=@MZ_SWT,MZ_DATE=@MZ_DATE,MZ_MEMO1=@MZ_MEMO1," +
                                                               "MZ_ODATE=@MZ_ODATE,MZ_EDATE=@MZ_EDATE,MZ_MEMO=@MZ_MEMO,MZ_NO1=@MZ_NO1,MZ_INO=@MZ_INO";
            }
            #endregion


            ViewState["sqlString"] = sqlString;//修改SQL語法 依頁面不同更換
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btCancel.Enabled = true;
            btDelete.Enabled = true;
        }

        protected void insertBasicGroupPermission(int Mode)
        {

            string TPFID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPFID FROM TPF_FIONDATA WHERE TPFNAME='人事管理'");

            string TPMN_GID = o_DBFactory.ABC_toTest.vExecSQL("Select TPMN_GID from (SELECT TPMN_GID FROM TP_MODEL_NAME Inner Join TP_GROUP_DATA ON TP_GROUP_DATA.TPG_GID = TP_MODEL_NAME.TPG_GID  WHERE TPMN_TPFID = '" + TPFID + "' AND TPG_DATANAME='D' order by TPMN_GID) where ROWNUM=1");

            string TPMID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMID FROM TPM_MEMBER WHERE TPM_IDNO='" + TextBox_MZ_ID.Text + "'");

            string deleteString = "DELETE FROM TP_MODEL_MEMBER WHERE TPMID='" + TPMID + "'";

            string deleteString1 = "DELETE FROM TP_MODEL_MEMBER WHERE TPMID = '" + TPMID + "'";

            string insertString = "Insert Into TP_MODEL_MEMBER (TPMM_GID, TPMN_GID, TPMID) "
                                   + "Values ( NEXT VALUE FOR dbo.SEQ_TP_GROUP,'" + TPMN_GID + "','" + TPMID + "')";

            try
            {
                if (Mode == 1)
                {
                    o_DBFactory.ABC_toTest.Edit_Data(deleteString);
                    o_DBFactory.ABC_toTest.Edit_Data(deleteString1);
                }
                o_DBFactory.ABC_toTest.Edit_Data(insertString);
            }
            catch
            {

            }
        }

        //確認
        protected void btOK_Click(object sender, EventArgs e)
        {
            #region 檢查必填欄位
            string strMessage = "必填欄位未填!!\\r\\n";
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                strMessage += "*身分證號\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()))
            {
                strMessage += "*編制機關\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_UNIT.Text.Trim()))
            {
                strMessage += "*編制單位\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_EXAD.Text.Trim()))
            {
                strMessage += "*現服機關\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_EXUNIT.Text.Trim()))
            {
                strMessage += "*現服單位\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_ADATE.Text.Trim()))
            {
                strMessage += "*到職日期\\r\\n";
            }
            if (string.IsNullOrEmpty(ddl_MZ_ISPOLICE.Text.Trim()))
            {
                strMessage += "*是否警職人員\\r\\n";
            }
            if (string.IsNullOrEmpty(dll_MZ_STATUS2.Text.Trim()))
            {
                strMessage += "*任職狀態\\r\\n";
            }

            if (string.IsNullOrEmpty(TextBox_MZ_SRANK.Text.Trim()))
            {
                strMessage += "*薪俸職等\\r\\n";
            }          

            if (strMessage != "必填欄位未填!!\\r\\n")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + strMessage + "')", true);
                return;
            }
            #endregion
            sqlString = ViewState["sqlString"].ToString();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand Insertcmd = new SqlCommand(sqlString, conn);
                Insertcmd.CommandType = CommandType.Text;
                try
                {
                    #region Parameters
                    if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
                    {
                        #region 第一分頁(基本&現職)
                        string ErrorString = "";

                        string old_ID = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();
                        }

                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && ViewState["Mode"].ToString() == "UPDATE")
                            pkey_check = "0";
                        else
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");

                        if (pkey_check != "0")
                        {
                            ErrorString += "資料庫已有相同人員！請查詢！" + "\\r\\n";
                            TextBox_MZ_ID.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_ID.BackColor = Color.White;
                        }

                        if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
                            if (string.IsNullOrEmpty(DropDownList_MZ_EXTPOS.SelectedValue))
                            {
                                ErrorString += "兼代職名稱不可空白" + "\\r\\n";
                                DropDownList_MZ_EXTPOS.BackColor = Color.Orange;
                            }
                            else
                                DropDownList_MZ_EXTPOS.BackColor = Color.White;


                        if (string.IsNullOrEmpty(TextBox_MZ_AD.Text))
                        {
                            ErrorString += "編制機關不可空白" + "\\r\\n";
                            TextBox_MZ_AD.BackColor = Color.Orange;
                        }
                        else
                            TextBox_MZ_AD.BackColor = Color.White;

                        // sam 20200630
                        //sam WellSince Sam.Hsu 20200908 人事基本資料 發薪機關 若不輸入 第一次跳訊息提醒 不擋 (小隊長於 20200903提出
                        if (string.IsNullOrEmpty(TextBox_PAY_AD.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請注意，發薪機關為空白！');", true);

                            TextBox_PAY_AD.BackColor = Color.Orange;
                        }
                        else
                            TextBox_PAY_AD.BackColor = Color.White;


                        if (string.IsNullOrEmpty(TextBox_MZ_EXAD.Text))
                        {
                            ErrorString += "現服機關不可空白" + "\\r\\n";
                            TextBox_MZ_EXAD.BackColor = Color.Orange;
                        }
                        else
                        {
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT  COUNT(*) from A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'");
                            if (pkey_check == "0")
                            {
                                ErrorString += "查無此現服機關" + "\\r\\n";
                                TextBox_MZ_EXAD.BackColor = Color.Orange;
                            }
                            else
                            {
                                TextBox_MZ_EXAD.BackColor = Color.White;
                            }
                        }
                           

                        if (string.IsNullOrEmpty(TextBox_MZ_UNIT.Text))
                        {
                            ErrorString += "編制單位不可空白" + "\\r\\n";
                            TextBox_MZ_UNIT.BackColor = Color.Orange;
                        }
                        else
                            TextBox_MZ_UNIT.BackColor = Color.White;

                        //現服單位
                        TextBox_MZ_EXUNIT.BackColor = Color.White;
                        if (string.IsNullOrEmpty(TextBox_MZ_EXUNIT.Text))
                        {
                            ErrorString += "現服單位不可空白" + "\\r\\n";
                            TextBox_MZ_EXUNIT.BackColor = Color.Orange;
                        }
                        //現服單位 不可為:0360 偵查隊,這是給舊版使用的資料選項,應選「OO分局/OO偵查隊」
                        //此選項錯誤可能會導致WebAD系統資料異常,因為AD結構中並非每個分局都有0360 偵查隊
                        else if (TextBox_MZ_EXUNIT.Text == "0360")
                        {
                            ErrorString += "現服機關/單位請輸入「OO分局/OO偵查隊」！" + "\\r\\n";
                            TextBox_MZ_EXUNIT.BackColor = Color.Orange;
                        }
                        //MZ_EXAD如果是382130200C(新北市政府警察局刑事警察大隊)，則MZ_EXUNIT不得輸入 DH01、DH02.......DH18，(提示：現服機關/單位有誤，請輸入「OO分局/OO偵查隊」！)
                        else if(TextBox_MZ_EXAD.Text == "382130200C")
                        {
                            switch (TextBox_MZ_EXUNIT.Text)
                            {
                                case "DH01":
                                case "DH02":
                                case "DH03":
                                case "DH04":
                                case "DH05":
                                case "DH06":
                                case "DH07":
                                case "DH08":
                                case "DH09":
                                case "DH10":
                                case "DH11":
                                case "DH12":
                                case "DH13":
                                case "DH14":
                                case "DH15":
                                case "DH16":
                                case "DH17":
                                case "DH18":
                                    ErrorString += "現服機關/單位有誤，請輸入「OO分局/OO偵查隊」！" + "\\r\\n";
                                    TextBox_MZ_EXUNIT.BackColor = Color.Orange;
                                    break;
                            }
                        }
                        //MZ_EXAD如果是 382130300C(新北市政府警察局交通警察大隊)，則MZ_EXUNIT不得輸入DG01、DG02........DG18(提示：現服機關/單位有誤，請輸入「OO分局/OO分隊」！)
                        else if (TextBox_MZ_EXAD.Text == "382130300C")
                        {
                            switch (TextBox_MZ_EXUNIT.Text)
                            {
                                case "DG01":
                                case "DG02":
                                case "DG03":
                                case "DG04":
                                case "DG05":
                                case "DG06":
                                case "DG07":
                                case "DG08":
                                case "DG09":
                                case "DG10":
                                case "DG11":
                                case "DG12":
                                case "DG13":
                                case "DG14":
                                case "DG15":
                                case "DG16":
                                case "DG17":
                                case "DG18":
                                    ErrorString += "現服機關/單位有誤，請輸入「OO分局/OO分隊」！" + "\\r\\n";
                                    TextBox_MZ_EXUNIT.BackColor = Color.Orange;
                                    break;
                            }
                        }
                        else
                            TextBox_MZ_EXUNIT.BackColor = Color.White;

                        

                        if (RadioButtonList_MZ_ABORIGINE.SelectedValue == "Y")
                            if (string.IsNullOrEmpty(DropDownList_MZ_ABORIGINENAME.SelectedValue))
                            {
                                ErrorString += "族別不可空白" + "\\r\\n";
                                DropDownList_MZ_ABORIGINENAME.BackColor = Color.Orange;
                            }
                            else
                                DropDownList_MZ_ABORIGINENAME.BackColor = Color.White;
                        //2013/10/24
                        if (!string.IsNullOrEmpty(TextBox_MZ_PNO1.Text.Trim()))
                        {
                            if (TextBox_MZ_PNO1.Text.Trim().Length < 2)
                                ErrorString += "警勤區長度小於2" + "\\r\\n";
                        }
                        
                        if (!string.IsNullOrEmpty(TextBox_MZ_PNO1_Second.Text.Trim()))
                        {
                            if( TextBox_MZ_PNO1_Second.Text.Trim().Length<2)
                                ErrorString += "兼勤區長度小於2" + "\\r\\n";
                        }

                        //任主管月數 
                        //  如果沒寫則是0
                        if (string.IsNullOrEmpty(TextBox_MZ_PB2.Text.Trim()))
                        {
                            TextBox_MZ_PB2.Text = "0";
                        }
                        //  轉換成數字看看
                        int temp = 0;
                        bool isInt = int.TryParse(TextBox_MZ_PB2.Text, out temp);
                        //  不是數字
                        if (isInt == false)
                        {
                            ErrorString += "任主管月數 請輸入數字" + "\\r\\n";
                        }
                        //  不能小於0
                        if (temp < 0)
                        {
                            ErrorString += "任主管月數 請輸入0以上之數字" + "\\r\\n";
                        }


                        //2013/10/24

                        //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                        //foreach (Object ob in Panel_Basic.Controls)
                        //{
                        //    if (ob is TextBox)
                        //    {
                        //        TextBox tbox = (TextBox)ob;

                        //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_DLBASE", tbox.Text);

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

                        //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_DLBASE", dlist.Text);

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



                        //foreach (Object ob in Panel_Now.Controls)
                        //{
                        //    if (ob is TextBox)
                        //    {
                        //        TextBox tbox = (TextBox)ob;

                        //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_DLBASE", tbox.Text);

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

                        //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_DLBASE", dlist.Text);

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
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與不得空白之欄位');", true);
                            return;
                        }

                        upper_Table_Enable_False();
                        if (string.IsNullOrEmpty(TextBox_MZ_OCCC.Text.Trim()))
                        {
                        }
                        btNow.CausesValidation = false;

                        Insertcmd.Parameters.Add("MZ_NAME", SqlDbType.NVarChar).Value = TextBox_MZ_NAME.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ENAME", SqlDbType.VarChar).Value = TextBox_MZ_ENAME.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim().ToUpper();
                        Insertcmd.Parameters.Add("MZ_TBCD3", SqlDbType.VarChar).Value = TextBox_MZ_TBCD3.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_CITY", SqlDbType.NVarChar).Value = TextBox_MZ_CITY.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_BL", SqlDbType.VarChar).Value = TextBox_MZ_BL.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_SM", SqlDbType.VarChar).Value = DropDownList_MZ_SM.SelectedValue.Length != 0 ? DropDownList_MZ_SM.SelectedValue : Convert.DBNull;
                        Insertcmd.Parameters.Add("MZ_SEX", SqlDbType.VarChar).Value = DropDownList_MZ_SEX.SelectedValue.Length != 0 ? DropDownList_MZ_SEX.SelectedValue : Convert.DBNull;
                        Insertcmd.Parameters.Add("MZ_ADD1", SqlDbType.NVarChar).Value = TextBox_MZ_ADD1.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ADD2", SqlDbType.NVarChar).Value = TextBox_MZ_ADD2.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_PHONE", SqlDbType.VarChar).Value = TextBox_MZ_PHONE.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_OFFYY", SqlDbType.VarChar).Value = TextBox_MZ_OFFYY.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_FYEAR", SqlDbType.VarChar).Value = TextBox_MZ_FYEAR.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_TBCD9", SqlDbType.VarChar).Value = TextBox_MZ_TBCD9.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_POLNO", SqlDbType.VarChar).Value = TextBox_MZ_POLNO.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim();

                        // sam 20200630 發薪機關
                        Insertcmd.Parameters.Add("PAY_AD", SqlDbType.VarChar).Value = TextBox_PAY_AD.Text.Trim();
                                  
                        Insertcmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EXAD", SqlDbType.VarChar).Value = TextBox_MZ_EXAD.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EXUNIT", SqlDbType.VarChar).Value = TextBox_MZ_EXUNIT.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = TextBox_MZ_RANK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = TextBox_MZ_RANK1.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = TextBox_MZ_OCCC.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = TextBox_MZ_CHISI.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = TextBox_MZ_POSIND.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_NRT", SqlDbType.VarChar).Value = TextBox_MZ_NRT.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = TextBox_MZ_TBDV.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = TextBox_MZ_PCHIEF.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_NREA", SqlDbType.VarChar).Value = TextBox_MZ_NREA.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_PHONH", SqlDbType.VarChar).Value = TextBox_MZ_PHONH.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_MOVETEL", SqlDbType.VarChar).Value = TextBox_MZ_MOVETEL.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_SPT", SqlDbType.VarChar).Value = TextBox_MZ_SPT.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = TextBox_MZ_SRANK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_NIN", SqlDbType.VarChar).Value = TextBox_MZ_NIN.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_SLVC", SqlDbType.VarChar).Value = TextBox_MZ_SLVC.Text.Trim();//俸 階
                        Insertcmd.Parameters.Add("MZ_PNO", SqlDbType.VarChar).Value = DropDownList_MZ_PNO.SelectedValue;//// 警勤區繁雜程度
                        Insertcmd.Parameters.Add("MZ_PNO1", SqlDbType.VarChar).Value = TextBox_MZ_PNO1.Text.Trim();//// 第幾警勤區
                        Insertcmd.Parameters.Add("MZ_TNO", SqlDbType.VarChar).Value = TextBox_MZ_TNO.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_SPT1", SqlDbType.VarChar).Value = TextBox_MZ_SPT1.Text.Trim();//俸 點
                        Insertcmd.Parameters.Add("MZ_PESN", SqlDbType.VarChar).Value = TextBox_MZ_PESN.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_PHONO", SqlDbType.VarChar).Value = TextBox_MZ_PHONO.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EMNAM", SqlDbType.NVarChar).Value = TextBox_MZ_EMNAM.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_RET", SqlDbType.VarChar).Value = TextBox_MZ_RET.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ZONE1", SqlDbType.VarChar).Value = TextBox_MZ_ZONE1.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ZONE2", SqlDbType.VarChar).Value = TextBox_MZ_ZONE2.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EXTPOS", SqlDbType.NVarChar).Value = DropDownList_MZ_EXTPOS.SelectedValue.Trim();
                        Insertcmd.Parameters.Add("MZ_ISEXTPOS", SqlDbType.VarChar).Value = RadioButtonList_MZ_ISEXTPOS.SelectedValue.Length != 0 ? RadioButtonList_MZ_ISEXTPOS.SelectedValue : Convert.DBNull;
                        Insertcmd.Parameters.Add("MZ_ABORIGINE", SqlDbType.VarChar).Value = RadioButtonList_MZ_ABORIGINE.SelectedValue.Length != 0 ? RadioButtonList_MZ_ABORIGINE.SelectedValue : Convert.DBNull;
                        Insertcmd.Parameters.Add("MZ_NID", SqlDbType.NVarChar).Value = TextBox_MZ_NID.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EXTPOS_SRANK", SqlDbType.VarChar).Value = TextBox_MZ_EXTPOS_SRANK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_STATUS2", SqlDbType.VarChar).Value = dll_MZ_STATUS2.SelectedValue;
                        Insertcmd.Parameters.Add("MUSER", SqlDbType.VarChar).Value = Session["ADPMZ_ID"].ToString();
                        Insertcmd.Parameters.Add("MDATE", SqlDbType.DateTime).Value = DateTime.Now;
                        //Insertcmd.Parameters.Add("MZ_PB2", SqlDbType.VarChar).Value = TextBox_MZ_PB2.Text.Trim();
                       //2013/06/20
                        Insertcmd.Parameters.Add("MZ_PNO_SECOND", SqlDbType.VarChar).Value = DropDownList_MZ_PNO_Second.SelectedValue;//// 兼勤區繁雜程度
                        Insertcmd.Parameters.Add("MZ_PNO1_SECOND", SqlDbType.VarChar).Value = TextBox_MZ_PNO1_Second.Text.Trim();//// 兼第幾警勤區
                        //2013/06/20
                        if (ViewState["Mode"].ToString() == "INSERT")
                        {
                            //Insertcmd.Parameters.Add("PAY_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim().ToUpper();
                            Insertcmd.Parameters.Add("PAY_AD", SqlDbType.VarChar).Value = TextBox_PAY_AD.Text.Trim().ToUpper();
                            Insertcmd.Parameters.Add("MZ_POWER", SqlDbType.VarChar).Value = "D";//nick

                            // Insertcmd.Parameters.Add("MZ_STATUS2", SqlDbType.VarChar).Value = "Y";
                        }

                        Insertcmd.Parameters.Add("MZ_FDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_FDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_FDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_BIR", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_BIR.Text.Trim()) ? Convert.DBNull : TextBox_MZ_BIR.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_ADATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_ADATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        //新增合格實授日欄位 20181120 by sky
                        Insertcmd.Parameters.Add("MZ_QUA_DATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_QUA_DATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_QUA_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        //Insertcmd.Parameters.Add("MZ_OPFDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_OPFDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_OPFDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_OPEDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_OPEDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_OPEDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_SLFDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_SLFDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_SLFDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_SLEDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_SLEDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_SLEDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_ODATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_ODAY.Text.Trim()) ? Convert.DBNull : TextBox_MZ_ODAY.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_LDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_LDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_LDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_TDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_TDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_TDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');

                        //2013/01/24 任主管月數MZ_PB2未輸入則設為0
                        int iMZ_PB2 = 0;
                        int.TryParse(TextBox_MZ_PB2.Text, out iMZ_PB2);
                        TextBox_MZ_PB2.Text = iMZ_PB2.ToString();
                        Insertcmd.Parameters.Add("MZ_PB2", SqlDbType.VarChar).Value = iMZ_PB2.ToString();

                        Insertcmd.Parameters.Add("MZ_INSURANCEMODE", SqlDbType.VarChar).Value = DropDownList_MZ_INSURANCEMODE.SelectedValue.Trim();
                        Insertcmd.Parameters.Add("MZ_ARMYSTATE", SqlDbType.NVarChar).Value = TextBox_MZ_ARMYSTATE.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ARMYRANK", SqlDbType.VarChar).Value = TextBox_MZ_ARMYRANK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ARMYKIND", SqlDbType.VarChar).Value = TextBox_MZ_ARMYKIND.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ARMYCOURSE", SqlDbType.VarChar).Value = TextBox_MZ_ARMYCOURSE.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ABORIGINENAME", SqlDbType.VarChar).Value = DropDownList_MZ_ABORIGINENAME.SelectedValue.Trim();
                        Insertcmd.Parameters.Add("MZ_ABORIGINEKIND", SqlDbType.VarChar).Value = RadioButtonList_MZ_ABORIGINEKIND.SelectedValue.Length != 0 ? RadioButtonList_MZ_ABORIGINEKIND.SelectedValue : Convert.DBNull;

                        //新增權理職等、是否警職人員欄位 20190514 by sky
                        Insertcmd.Parameters.Add("MZ_AHP_RANK", SqlDbType.VarChar).Value = txt_MZ_AHP_RANK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ISPOLICE", SqlDbType.VarChar).Value = ddl_MZ_ISPOLICE.SelectedValue;

                        ////新增薪資生效日期 20190530 by sky
                        //Insertcmd.Parameters.Add("MZ_SALARY_ISDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(txt_MZ_SALARY_ISDATE.Text.Trim()) ? Convert.DBNull : txt_MZ_SALARY_ISDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');

                        // sam wellsince 20201008 薪資生效日 MZ_SALARY_ISDATE 若為空 預設塞 到職日期 
                        Insertcmd.Parameters.Add("MZ_SALARY_ISDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(txt_MZ_SALARY_ISDATE.Text.Trim()) ? TextBox_MZ_ADATE.Text.Trim().Replace("/", "").PadLeft(7, '0') : txt_MZ_SALARY_ISDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');

                        // sam.hsu wellsince 20201208
                        Insertcmd.Parameters.Add("MZ_TRAINING", SqlDbType.VarChar).Value = dll_MZ_TRAINING.SelectedValue;


                        txt_Account.Enabled = false;
                        #endregion
                    }
                    else if (MultiView1.ActiveViewIndex == 2)//學歷
                    {
                        #region 第三分頁
                        string ErrorString = "";

                        string old_ID = "NULL";
                        string old_SCHOOL = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();
                            old_SCHOOL = Session["PKEY_MZ_SCHOOL"].ToString();
                        }

                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && old_SCHOOL == TextBox_MZ_SCHOOL.Text && ViewState["Mode"].ToString() == "UPDATE")
                            pkey_check = "0";
                        else
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_EDUCATION WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_SCHOOL='" + TextBox_MZ_SCHOOL1.Text.Trim() + "' AND MZ_DEPARTMENT='" + TextBox_MZ_DEPARTMENT1.Text.Trim() + "'");

                        if (pkey_check != "0")
                        {
                            ErrorString += "學校與科系違反唯一值的條件" + "\\r\\n";
                            TextBox_MZ_SCHOOL.BackColor = Color.Orange;
                            TextBox_MZ_DEPARTMENT.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_SCHOOL.BackColor = Color.White;
                            TextBox_MZ_DEPARTMENT.BackColor = Color.White;
                        }


                        //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                        //foreach (Object ob in Panel_TableEdu.Controls)
                        //{
                        //    if (ob is TextBox)
                        //    {
                        //        TextBox tbox = (TextBox)ob;

                        //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_EDUCATION", tbox.Text);

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

                        //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_EDUCATION", dlist.Text);

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

                        Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_SCHOOL", SqlDbType.VarChar).Value = TextBox_MZ_SCHOOL1.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_DEPARTMENT", SqlDbType.VarChar).Value = TextBox_MZ_DEPARTMENT1.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_YEAR", SqlDbType.VarChar).Value = TextBox_MZ_YEAR.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EDUKIND", SqlDbType.VarChar).Value = TextBox_MZ_EDUKIND.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EDULEVEL", SqlDbType.VarChar).Value = TextBox_MZ_EDULEVEL.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EDUCLASS", SqlDbType.VarChar).Value = TextBox_MZ_EDUCLASS.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_BEGINDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_BEGINDATE.Text) ? Convert.DBNull : TextBox_MZ_BEGINDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_ENDDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_ENDDATE.Text) ? Convert.DBNull : TextBox_MZ_ENDDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        #endregion
                    }
                    else if (MultiView1.ActiveViewIndex == 3)
                    {
                        #region 第四分頁
                        string ErrorString = "";

                        string old_ID = "NULL";

                        string old_EXAM_NAME = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();

                            old_EXAM_NAME = Session["PKEY_EXAM_NAME"].ToString();
                        }
                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && old_EXAM_NAME == TextBox_EXAM_NAME.Text && ViewState["Mode"].ToString() == "UPDATE")
                            pkey_check = "0";
                        else
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_EXAM WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND EXAM_NAME='" + TextBox_EXAM_NAME.Text.Trim() + "'");

                        if (pkey_check != "0")
                        {
                            ErrorString += "考試名稱違反唯一值的條件" + "\\r\\n";
                            TextBox_EXAM_NAME.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_EXAM_NAME.BackColor = Color.White;
                        }
                        //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                        //foreach (Object ob in Panel_Exam.Controls)
                        //{
                        //    if (ob is TextBox)
                        //    {
                        //        TextBox tbox = (TextBox)ob;

                        //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_EXAM", tbox.Text);

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

                        //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_EXAM", dlist.Text);

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

                        Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim().ToUpper();
                        Insertcmd.Parameters.Add("EXAM_NAME", SqlDbType.VarChar).Value = TextBox_EXAM_NAME.Text.Trim();
                        Insertcmd.Parameters.Add("EXAM_CLASS", SqlDbType.VarChar).Value = TextBox_EXAM_CLASS.Text.Trim();
                        Insertcmd.Parameters.Add("EXAM_ADMISSION", SqlDbType.VarChar).Value = TextBox_EXAM_ADMISSION.Text.Trim();
                        Insertcmd.Parameters.Add("EXAM_DOCUMENTS", SqlDbType.VarChar).Value = TextBox_EXAM_DOCUMENTS.Text.Trim();
                        Insertcmd.Parameters.Add("EXAM_YEAR", SqlDbType.VarChar).Value = TextBox_EXAM_YEAR.Text.Trim();
                        #endregion
                    }
                    else if (MultiView1.ActiveViewIndex == 4)
                    {
                        #region 第五分頁
                        string ErrorString = "";

                        string old_ID = "NULL";

                        string old_CAREER_AD = "NULL";

                        string old_CAREER_UNIT = "NULL";

                        string old_CAREER_OCCC = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();

                            old_CAREER_AD = Session["PKEY_CAREER_AD"].ToString();

                            old_CAREER_UNIT = Session["PKEY_CAREER_UNIT"].ToString();

                            old_CAREER_OCCC = Session["PKEY_CAREER_OCCC"].ToString();
                        }

                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && old_CAREER_UNIT == TextBox_CAREER_UNIT.Text && old_CAREER_OCCC == TextBox_CAREER_OCCC.Text && old_CAREER_AD == TextBox_CAREER_AD.Text && ViewState["Mode"].ToString() == "UPDATE")
                            pkey_check = "0";
                        else
                            pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_CAREER WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_AD='" + TextBox_CAREER_AD.Text.Trim() + "' AND MZ_UNIT='" + TextBox_CAREER_UNIT.Text.Trim() + "' AND MZ_OCCC='" + TextBox_CAREER_OCCC.Text.Trim() + "'");

                        if (pkey_check != "0")
                        {
                            ErrorString += "機關與單位與職稱違反唯一值的條件" + "\\r\\n";
                            TextBox_CAREER_AD.BackColor = Color.Orange;
                            TextBox_CAREER_UNIT.BackColor = Color.Orange;
                            TextBox_CAREER_OCCC.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_CAREER_AD.BackColor = Color.White;
                            TextBox_CAREER_UNIT.BackColor = Color.White;
                            TextBox_CAREER_OCCC.BackColor = Color.White;
                        }

                        //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                        //foreach (Object ob in Panel_Career.Controls)
                        //{
                        //    if (ob is TextBox)
                        //    {
                        //        TextBox tbox = (TextBox)ob;

                        //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_CAREER", tbox.Text);

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

                        //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_CAREER", dlist.Text);

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

                        Insertcmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_CAREER_AD.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_CAREER_UNIT.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = TextBox_CAREER_OCCC.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim().ToUpper();
                        Insertcmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = TextBox_CAREER_RANK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = TextBox_CAREER_RANK1.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = TextBox_CAREER_TBDV.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = TextBox_CAREER_CHISI.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = TextBox_CAREER_POSIND.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_PESN", SqlDbType.VarChar).Value = TextBox_CAREER_PESN.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = TextBox_CAREER_PCHIEF.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_NRT", SqlDbType.VarChar).Value = TextBox_CAREER_NRT.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_CAREER_DATE.Text) ? Convert.DBNull : TextBox_CAREER_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_CAREER_IDATE.Text) ? Convert.DBNull : TextBox_CAREER_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_CAREER_ADATE.Text) ? Convert.DBNull : TextBox_CAREER_ADATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_TBDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_CAREER_TBDATE.Text) ? Convert.DBNull : TextBox_CAREER_TBDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("MZ_TBID", SqlDbType.VarChar).Value = TextBox_CAREER_TBID.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EXID", SqlDbType.VarChar).Value = TextBox_CAREER_EXID.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_NIN", SqlDbType.VarChar).Value = TextBox_CAREER_NIN.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_NREA", SqlDbType.VarChar).Value = TextBox_CAREER_NREA.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_TBNREA", SqlDbType.VarChar).Value = TextBox_CAREER_TBNREA.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_EXTPOS", SqlDbType.NVarChar).Value = DropDownList_CAREER_EXTPOS.SelectedValue;
                        Insertcmd.Parameters.Add("MZ_ISEXTPOS", SqlDbType.VarChar).Value = RadioButtonList_CAREER_ISEXTPOS.SelectedValue.Trim();

                        #endregion
                    }
                    else if (MultiView1.ActiveViewIndex == 5)
                    {
                        #region 第六分頁
                        //if (Label_Year.Text == TextBox.Text) //同年度
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('該年度已有資料')", true);
                        //    return;
                        //}
                        //else
                        //{
                        Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                        Insertcmd.Parameters.Add("T01", SqlDbType.VarChar).Value = TextBox.Text.Trim();
                        Insertcmd.Parameters.Add("T17", SqlDbType.VarChar).Value = TextBox17.Text.Trim();
                        Insertcmd.Parameters.Add("T18", SqlDbType.VarChar).Value = TextBox18.Text.Trim();
                        Insertcmd.Parameters.Add("T19", SqlDbType.VarChar).Value = TextBox19.Text.Trim();
                        Insertcmd.Parameters.Add("T20", SqlDbType.VarChar).Value = TextBox20.Text.Trim();
                        Insertcmd.Parameters.Add("T21", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox21.Text) ? Convert.DBNull : TextBox21.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("T22", SqlDbType.VarChar).Value = TextBox22.Text.Trim();
                        Insertcmd.Parameters.Add("T23", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox23.Text) ? Convert.DBNull : TextBox23.Text.Trim().Replace("/", "").PadLeft(7, '0');
                        Insertcmd.Parameters.Add("T24", SqlDbType.VarChar).Value = TextBox24.Text.Trim();
                        Insertcmd.Parameters.Add("T25", SqlDbType.VarChar).Value = TextBox25.Text.Trim();
                        Insertcmd.Parameters.Add("T26", SqlDbType.VarChar).Value = TextBox26.Text.Trim();
                        Insertcmd.Parameters.Add("T27", SqlDbType.VarChar).Value = TextBox27.Text.Trim();
                        Insertcmd.Parameters.Add("T28", SqlDbType.VarChar).Value = TextBox28.Text.Trim();
                        Insertcmd.Parameters.Add("T29", SqlDbType.VarChar).Value = TextBox29.Text.Trim();
                        Insertcmd.Parameters.Add("T30", SqlDbType.VarChar).Value = TextBox30.Text.Trim();
                        Insertcmd.Parameters.Add("T31", SqlDbType.VarChar).Value = TextBox31.Text.Trim();
                        Insertcmd.Parameters.Add("T32", SqlDbType.VarChar).Value = TextBox32.Text.Trim();
                        //}
                        #endregion 
                    }
                    else if (MultiView1.ActiveViewIndex == 6)
                    {
                        #region 第七分頁
                        string ErrorString = "";

                        string old_ID = "NULL";

                        string old_FAMILYID = "NULL";

                        if (ViewState["Mode"].ToString() == "UPDATE")
                        {
                            old_ID = Session["PKEY_MZ_ID"].ToString();

                            old_FAMILYID = Session["PKEY_MZ_FAMILYID"].ToString();

                        }

                        string pkey_check;

                        if (old_ID == TextBox_MZ_ID.Text && old_FAMILYID == TextBox_MZ_FAMILYID.Text && ViewState["Mode"].ToString() == "UPDATE")
                            pkey_check = "0";
                        else pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_FAMILY WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_FAMILYID='" + TextBox_MZ_FAMILYID.Text.Trim() + "'");

                        if (pkey_check != "0")
                        {
                            ErrorString += "家屬身分證號違反唯一值的條件" + "\\r\\n";
                            TextBox_MZ_FAMILYID.BackColor = Color.Orange;
                        }
                        else
                        {
                            TextBox_MZ_FAMILYID.BackColor = Color.White;
                        }


                        //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                        //foreach (Object ob in Panel_Career.Controls)
                        //{
                        //    if (ob is TextBox)
                        //    {
                        //        TextBox tbox = (TextBox)ob;

                        //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_FAMILY", tbox.Text);

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

                        //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_FAMILY", dlist.Text);

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

                        Insertcmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_FAMILYID", SqlDbType.VarChar).Value = TextBox_MZ_FAMILYID.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_FAMILYNAME", SqlDbType.VarChar).Value = TextBox_MZ_FAMILYNAME.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_WORK", SqlDbType.VarChar).Value = TextBox_MZ_WORK.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_TITLE", SqlDbType.VarChar).Value = TextBox_MZ_TITLE.Text.Trim();
                        Insertcmd.Parameters.Add("MZ_ISINSURANCE", SqlDbType.VarChar).Value = RadioButtonList_MZ_ISINSURANCE.SelectedValue.Trim();
                        Insertcmd.Parameters.Add("MZ_INSURANCEMODE", SqlDbType.VarChar).Value = DropDownList_Family_INSURANCEMODE.SelectedValue.Trim();
                        Insertcmd.Parameters.Add("MZ_BIRTHDAY", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_BIRTHDAY.Text) ? Convert.DBNull : TextBox_MZ_BIRTHDAY.Text.Trim().Replace("/", "").PadLeft(7, '0');

                        #endregion
                    }
                    else if (MultiView1.ActiveViewIndex == 7)
                    {
                        
                    }
                    #endregion

                    Insertcmd.ExecuteNonQuery();

                    



                    

                    Response.Cookies["PKEY_MZ_ID"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_FAMILYID"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_CAREER_AD"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_CAREER_UNIT"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_CAREER_OCCC"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_EXAM_NAME"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_SCHOOL"].Expires = DateTime.Now.AddYears(-1);



                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        // add by 介入 2012/02/24 建立人事資料時，同步建立薪資資料
                        try
                        {
                            _2_salary.Police police = new TPPDDB._2_salary.Police(TextBox_MZ_ID.Text.Trim());
                        }
                        catch { }

                        using (SqlConnection connPolice = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                        {
                            string HI = DropDownList_MZ_INSURANCEMODE.SelectedValue.Trim();
                            int iHI = 100;
                            switch (HI)
                            {
                                case "1":
                                    iHI = 100;
                                        break;
                                case "2":
                                    iHI = 75;
                                        break;
                                case "3":
                                    iHI = 50;
                                        break;
                                case "4":
                                    iHI = 0;
                                        break;
                            }
                            connPolice.Open();
                            string PoliceSQL = "UPDATE B_BASE SET  HEALPER_INSURANCE = "+iHI+" WHERE IDCARD = '"+ TextBox_MZ_ID.Text.Trim() + "'";
                            SqlCommand InserPolice = new SqlCommand(PoliceSQL, connPolice);
                            InserPolice.CommandType = CommandType.Text;
                            InserPolice.ExecuteNonQuery();
                            connPolice.Close();
                            connPolice.Dispose();
                        }


                            string LBMSG = "新增成功";
                        HttpCookie Cookie1 = new HttpCookie("PersonalSearch_ID");
                        Cookie1.Value = TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim());
                        Response.Cookies.Add(Cookie1);
                        btNow.CausesValidation = false;

                        if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
                        {
                            string _strADID = TextBox_MZ_ID.Text.Substring(0, 1) + TextBox_MZ_ID.Text.Substring(5, 5);
                            //string _strADPWD = _strADID;
                            string _strNAME = TextBox_MZ_NAME.Text;
                            string _strADPWD = "Drinfo1!";
                            string ad = "OU=" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + TextBox_MZ_EXAD.Text + "'");
                            string Check_UNIT = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode =(SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_EXAD.Text + "' AND MZ_UNIT='" + TextBox_MZ_EXUNIT.Text + "')");
                            string unit = "OU=" + Check_UNIT;
                            string cn = "CN=" + _strADID;


                            string strAddAP_MSG = strAddAP(_strADID, TextBox_MZ_NAME.Text, _strADPWD, "Y", cn + "," + unit + "," + ad, TextBox_MZ_ID.Text);

                            //加入基本權限
                            insertBasicGroupPermission(0);

                            TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));

                            if (strAddAP_MSG == "新增成功" && !string.IsNullOrEmpty(Check_UNIT))
                            {
                                #region AD相關

                                //DirectoryEntry deContainer;
                                //deContainer = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                                //try
                                //{

                                //    //if (DropDownList_MZ_UNIT.SelectedValue == "-1" || DropDownList_MZ_AD.SelectedValue == "-1")
                                //    //{
                                //    //    //Label_MSG.Text = "請選擇單位";
                                //    //}
                                //    //else
                                //    //{

                                //    DirectoryEntry de1 = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                                //    try
                                //    {
                                //        if (chkADroot(ad, unit) == true)
                                //        {
                                //            DirectoryEntry child = de1.Children.Add(cn + "," + unit + "," + ad, "user");
                                //            child.CommitChanges();
                                //            //child.Properties["sAMAccountName"].Add(_strADID);
                                //            child.Properties["sAMAccountName"].Value = _strADID;
                                //            child.CommitChanges();
                                //            child.Properties["userPrincipalName"].Value = _strADID;
                                //            child.CommitChanges();
                                //            child.Properties["displayName"].Value = _strNAME;
                                //            child.CommitChanges();
                                //            child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + TextBox_MZ_EXAD.Text + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + TextBox_MZ_EXUNIT.Text + "'");
                                //            child.Invoke("SetPassword", new object[] { _strADPWD });
                                //            child.Properties["userAccountControl"].Value = 0x200;
                                //            child.CommitChanges();
                                //        }
                                //        else
                                //        {
                                //            if (chkADroot(ad) == true)
                                //            {
                                //                DirectoryEntry child = de1.Children.Add(unit + "," + ad, "organizationalUnit");
                                //                child.CommitChanges();

                                //                child = de1.Children.Add(cn + "," + unit + "," + ad, "user");
                                //                child.CommitChanges();
                                //                child.Properties["sAMAccountName"].Value = _strADID;
                                //                child.CommitChanges();
                                //                child.Properties["displayName"].Value = _strNAME;
                                //                child.CommitChanges();
                                //                child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + TextBox_MZ_EXAD.Text + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + TextBox_MZ_EXUNIT.Text + "'");
                                //                child.Invoke("SetPassword", new object[] { _strADPWD });
                                //                child.Properties["userAccountControl"].Value = 0x200;
                                //                child.CommitChanges();
                                //            }
                                //            else
                                //            {
                                //                DirectoryEntry child = de1.Children.Add(ad, "organizationalUnit");
                                //                child.CommitChanges();

                                //                child = de1.Children.Add(unit + "," + ad, "organizationalUnit");
                                //                child.CommitChanges();

                                //                child = de1.Children.Add(cn + "," + unit + "," + ad, "user");
                                //                child.CommitChanges();
                                //                child.Properties["sAMAccountName"].Value = _strADID;
                                //                child.CommitChanges();
                                //                child.Properties["displayName"].Value = _strNAME;
                                //                child.CommitChanges();
                                //                child.Properties["physicalDeliveryOfficeName"].Value = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode = '" + TextBox_MZ_EXAD.Text + "'") + "-" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode = '" + TextBox_MZ_EXUNIT.Text + "'");
                                //                child.Invoke("SetPassword", new object[] { _strADPWD });
                                //                child.Properties["userAccountControl"].Value = 0x200;
                                //                child.CommitChanges();
                                //            }
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        //LBMSG = "人事資料新增成功但建立AD帳號時發生非預期的意外或現服單位有誤，請聯絡網管人員";
                                //    }
                                //}
                                //catch
                                //{
                                //}

                                #endregion

                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + LBMSG + "');", true);
                            }
                            else
                            {
                                LBMSG = "人事資料新增成功但建立AD帳號時發生非預期的意外或現服單位有誤，請聯絡網管人員";
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + LBMSG + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + LBMSG + "');", true);
                            ViewState.Remove("Mode");
                            //2010.06.04 LOG紀錄 by介入
                            TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));
                        }
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        string LBMSG = "修改成功";

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Insertcmd));


                        // add by 介入 2012/04/03 修改人事資料後，同步更新薪資資料
                        //20140317 本來是在薪資基本資料修改,但因市府規定,只能由人事室這邊1.1這邊修改
                        //所以只要俸階,俸點(詳細欄位函式裡面)有更動,薪資基本資料就要重算

                        _2_salary.Police police = new TPPDDB._2_salary.Police(TextBox_MZ_ID.Text.Trim());

                        using (SqlConnection connPolice = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                        {
                            string HI = DropDownList_MZ_INSURANCEMODE.SelectedValue.Trim();
                            int iHI = 100;
                            switch (HI)
                            {
                                case "1":
                                    iHI = 100;
                                    break;
                                case "2":
                                    iHI = 75;
                                    break;
                                case "3":
                                    iHI = 50;
                                    break;
                                case "4":
                                    iHI = 0;
                                    break;
                            }
                            connPolice.Open();
                            string PoliceSQL = "UPDATE B_BASE SET  HEALPER_INSURANCE = " + iHI + " WHERE IDCARD = '" + TextBox_MZ_ID.Text.Trim() + "'";
                            SqlCommand UpdatePolice = new SqlCommand(PoliceSQL, connPolice);
                            UpdatePolice.CommandType = CommandType.Text;
                            UpdatePolice.ExecuteNonQuery();
                            connPolice.Close();
                            connPolice.Dispose();
                        }

                        //技術加給代碼
                        string technics = string.Empty;
                        //技術加給金額
                        int technicspay = 0;

                        //同步薪資中的是否為鑑識人員資料 20190530 by sky
                        if (!string.IsNullOrEmpty(ddl_ISCRIMELAB.SelectedValue))
                        {
                            if (ddl_ISCRIMELAB.SelectedValue.ToUpper().Equals("是"))
                            {
                                //薪俸職等代碼
                                string srank = police.srank.ToUpper();
                                //薪俸職等代碼 與 技術加給代碼對照
                                switch (srank)
                                {
                                    case "P09":
                                    case "G21":
                                        technics = "01";
                                        break;
                                    case "P08":
                                    case "G22":
                                        technics = "02";
                                        break;
                                    case "P07":
                                    case "G23":
                                        technics = "03";
                                        break;
                                    case "P06":
                                    case "G24":
                                        technics = "04";
                                        break;
                                    case "G11":
                                        technics = "05";
                                        break;
                                }
                                //查詢 技術加給金額
                                if (technics != "")
                                {
                                    technicspay = Salary.getPay(technics, "B_TECHNICS");
                                }


                                police.updateSalaryData(new List<SqlParameter>()
                                {
                                    new SqlParameter("CRIMELAB", ddl_ISCRIMELAB.SelectedValue),
                                    new SqlParameter("technics", technics),
                                    new SqlParameter("technicspay", technicspay)
                                });

                                police.isCrimelab = true;
                            }
                            else
                            {
                                technics = "0";
                                technicspay = 0;

                                police.updateSalaryData(new List<SqlParameter>()
                                {
                                    new SqlParameter("CRIMELAB", ddl_ISCRIMELAB.SelectedValue),
                                    new SqlParameter("technics", technics),
                                    new SqlParameter("technicspay", technicspay)
                                });

                                police.isCrimelab = false;
                            }
                        }

                        //更新薪資計算
                        police.updateSalary();


                        dataNext = Session["datanext"] as List<string>;
                        if (int.Parse(xcount.Text.Trim()) == 0 && dataNext.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && dataNext.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == dataNext.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < dataNext.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                        try
                        {
                            //改為合格實授日計算 20181120 by sky
                            if (Session["PKEY_MZ_QUA_DATE"].ToString() != TextBox_MZ_QUA_DATE.Text)
                            {
                                ChangeHday(TextBox_MZ_ID.Text, TextBox_MZ_QUA_DATE.Text.Replace("/", "").PadLeft(7, '0'));
                            }
                        }
                        catch { }
                        if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
                        {
                            if (Session["Personal1-1_EXAD"].ToString() != TextBox_MZ_EXAD.Text || Session["Personal1-1_EXUNIT"].ToString() != TextBox_MZ_EXUNIT.Text)
                            {
                                deleteREVIEW_MANAGEMENT(TextBox_MZ_ID.Text);

                                insertBasicGroupPermission(1);
                                //DirectoryEntry moveAD;
                                //DirectoryEntry deContainer = new DirectoryEntry("" + strLDAPIP_S + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");

                                DataTable dt = new DataTable();

                                string selectADString = "SELECT * FROM TPM_MEMBER WHERE TPM_IDNO='" + TextBox_MZ_ID.Text + "'";

                                dt = o_DBFactory.ABC_toTest.Create_Table(selectADString, "ADVALUE");

                                string cn, dstAD, dstUNIT, srcAD = "", srcUNIT = "";
                                dstAD = "OU=" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kchimatch from a_kchi_match where mz_kcode='" + TextBox_MZ_EXAD.Text + "'");

                                string Check_UNIT = o_DBFactory.ABC_toTest.vExecSQL("select mz_kchi from a_ktype where mz_ktype = '25' and mz_kcode =(SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_EXAD.Text + "' AND MZ_UNIT='" + TextBox_MZ_EXUNIT.Text + "')");

                                dstUNIT = "OU=" + Check_UNIT;
                                cn = "CN=" + o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMUSER FROM TPM_MEMBER WHERE TPM_IDNO='" + TextBox_MZ_ID.Text + "'");

                                string[] tmpGroup = dt.Rows[0]["TPMADGROUP"].ToString().Split(',');

                                if (tmpGroup.Length == 3)
                                {
                                    srcAD = tmpGroup[1];
                                    srcUNIT = tmpGroup[2];
                                }

                                try
                                {
                                    string updatestring = "UPDATE TPM_MEMBER SET TPMADGROUP='" + cn + "," + dstAD + "," + dstUNIT + "' WHERE TPM_IDNO='" + TextBox_MZ_ID.Text + "'";

                                    o_DBFactory.ABC_toTest.Edit_Data(updatestring);

                                    if (!string.IsNullOrEmpty(Check_UNIT))
                                    {
                                        #region AD相關

                                        //if (chkADroot(srcAD, srcUNIT, cn))   //找得到AD帳號
                                        //{
                                        //    //拆字串LDAP://192.168.166.1/DC=drinfotech,DC=com,DC=tw
                                        //    string strLDAP = strLDAPIP_S;
                                        //    char[] charDelimiter = { '/' };
                                        //    string[] strWords = strLDAP.Split(charDelimiter);
                                        //    moveAD = deContainer.Children.Find(cn + "," + srcUNIT + "," + srcAD);
                                        //    if (chkADroot(dstAD, dstUNIT) == true)  //如果機關---單位---資料夾--有
                                        //    {
                                        //        //移動AD帳號
                                        //        moveAD.MoveTo(new DirectoryEntry("" + strWords[0] + "//" + strWords[2] + "/" + dstUNIT + "," + dstAD + "," + strWords[3] + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + ""));
                                        //    }
                                        //    else  //如果機關---單位---資料夾--沒有
                                        //    {
                                        //        if (chkADroot(dstAD) == true)   //如果機關---資料夾有---->新增單位
                                        //        {
                                        //            DirectoryEntry child = deContainer.Children.Add(dstUNIT + "," + dstAD, "organizationalUnit");
                                        //            child.CommitChanges();
                                        //            //移動AD帳號
                                        //            moveAD.MoveTo(new DirectoryEntry("" + strWords[0] + "//" + strWords[2] + "/" + dstUNIT + "," + dstAD + "," + strWords[3] + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + ""));
                                        //        }
                                        //        else   //如果機關---資料夾都沒有---->新增機關+單位
                                        //        {
                                        //            DirectoryEntry child = deContainer.Children.Add(dstAD, "organizationalUnit");
                                        //            child.CommitChanges();
                                        //            child = deContainer.Children.Add(dstUNIT + "," + dstAD, "organizationalUnit");
                                        //            child.CommitChanges();
                                        //            //移動AD帳號
                                        //            moveAD.MoveTo(new DirectoryEntry("" + strWords[0] + "//" + strWords[2] + "/" + dstUNIT + "," + dstAD + "," + strWords[3] + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + ""));
                                        //        }
                                        //    }
                                        //}
                                        //else  //找不到AD帳號
                                        //{
                                        //    if (chkADroot(dstAD, dstUNIT) == true)  //如果機關---單位---資料夾--有
                                        //    {   //新增帳號
                                        //        DirectoryEntry child = deContainer.Children.Add(cn + "," + dstUNIT + "," + dstAD, "user");
                                        //        child.CommitChanges();
                                        //        child.Properties["sAMAccountName"].Value = dt.Rows[0]["TPMUSER"].ToString();
                                        //        child.CommitChanges();
                                        //        child.Properties["userPrincipalName"].Value = dt.Rows[0]["TPMUSER"].ToString();
                                        //        child.CommitChanges();
                                        //        string strNAME = TextBox_MZ_NAME.Text;
                                        //        child.Properties["displayName"].Value = strNAME;
                                        //        child.CommitChanges();
                                        //        child.Properties["physicalDeliveryOfficeName"].Value = TextBox_MZ_EXAD1.Text + "-" + TextBox_MZ_EXUNIT1.Text;
                                        //        child.Invoke("SetPassword", new object[] { TPMPermissions._strDecod(dt.Rows[0]["TPMPWD"].ToString()) });
                                        //        child.Properties["userAccountControl"].Value = 0x200;
                                        //        child.CommitChanges();
                                        //    }
                                        //    else  //如果機關---單位---資料夾---沒有
                                        //    {
                                        //        if (chkADroot(dstAD) == true)  //如果機關---有
                                        //        {   //新增--->單位->帳號
                                        //            DirectoryEntry child = deContainer.Children.Add(dstUNIT + "," + dstAD, "organizationalUnit");
                                        //            child.CommitChanges();
                                        //            child = deContainer.Children.Add(cn + "," + dstUNIT + "," + dstAD, "user");
                                        //            child.CommitChanges();
                                        //            child.Properties["sAMAccountName"].Value = dt.Rows[0]["TPMUSER"].ToString();
                                        //            child.CommitChanges();
                                        //            child.Properties["userPrincipalName"].Value = dt.Rows[0]["TPMUSER"].ToString();
                                        //            child.CommitChanges();
                                        //            string strNAME = TextBox_MZ_NAME.Text;
                                        //            child.Properties["displayName"].Value = strNAME;
                                        //            child.CommitChanges();
                                        //            child.Properties["physicalDeliveryOfficeName"].Value = TextBox_MZ_EXAD1.Text + "-" + TextBox_MZ_EXUNIT1.Text;
                                        //            child.Invoke("SetPassword", new object[] { TPMPermissions._strDecod(dt.Rows[0]["TPMPWD"].ToString()) });
                                        //            child.Properties["userAccountControl"].Value = 0x200;
                                        //            child.CommitChanges();
                                        //        }
                                        //        else
                                        //        {   //新增--->機關->單位->帳號
                                        //            DirectoryEntry child = deContainer.Children.Add(dstAD, "organizationalUnit");
                                        //            child.CommitChanges();
                                        //            child = deContainer.Children.Add(dstUNIT + "," + dstAD, "organizationalUnit");
                                        //            child.CommitChanges();
                                        //            child = deContainer.Children.Add(cn + "," + dstUNIT + "," + dstAD, "user");
                                        //            child.CommitChanges();
                                        //            child.Properties["sAMAccountName"].Value = dt.Rows[0]["TPMUSER"].ToString();
                                        //            child.CommitChanges();
                                        //            child.Properties["userPrincipalName"].Value = dt.Rows[0]["TPMUSER"].ToString();
                                        //            child.CommitChanges();
                                        //            string strNAME = TextBox_MZ_NAME.Text;
                                        //            child.Properties["displayName"].Value = strNAME;
                                        //            child.CommitChanges();
                                        //            child.Properties["physicalDeliveryOfficeName"].Value = TextBox_MZ_EXAD1.Text + "-" + TextBox_MZ_EXUNIT1.Text;
                                        //            child.Invoke("SetPassword", new object[] { TPMPermissions._strDecod(dt.Rows[0]["TPMPWD"].ToString()) });
                                        //            child.Properties["userAccountControl"].Value = 0x200;
                                        //            child.CommitChanges();
                                        //        }
                                        //    }
                                        //}

                                        #endregion
                                    }
                                    else
                                    {
                                        LBMSG = "人事資料修改成功但建立AD帳號時發生非預期的意外或現服單位有誤，請聯絡網管人員";
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                           

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + LBMSG + "');", true);
                        ViewState.Remove("Mode");
                       

                        Session.Remove("PKEY_MZ_FDATE");
                        Session.Remove("PKEY_MZ_ID");
                        Session.Remove("PKEY_MZ_SCHOOL");
                        Session.Remove("PKEY_EXAM_NAME");
                        Session.Remove("PKEY_CAREER_AD");
                        Session.Remove("PKEY_CAREER_UNIT");
                        Session.Remove("PKEY_CAREER_OCCC");
                        Session.Remove("PKEY_MZ_FAMILYID");
                        Session.Remove("Personal1-1_EXAD");
                        Session.Remove("Personal1-1_EXUNIT");

                        btCareer.Enabled = true;
                        btEducation.Enabled = true;
                        btefficiency.Enabled = true;
                        btExam.Enabled = true;
                        btFamily.Enabled = true;
                        btCard.Enabled = true;
                        btefficiency_insert.Enabled = true;
                        //20150324
                        btefficiency.Enabled = true;
                    }



                    ViewState.Remove("Mode");

                    upper_Table_Enable_False();

                    #region 各view控制項
                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        A.controlEnable(ref this.Panel_Basic, false);
                        A.controlEnable(ref this.Panel_Now, false);
                        ABORIGINE();
                        EXTPOS();
                    }
                    else if (MultiView1.ActiveViewIndex == 1)
                    {
                        A.controlEnable(ref this.Panel_Basic, false);
                        A.controlEnable(ref this.Panel_Now, false);

                        EXTPOS();
                    }
                    else if (MultiView1.ActiveViewIndex == 2)
                    {
                        foreach (object tb in Panel_TableEdu.Controls)
                        {
                            if (tb is TextBox)
                            {
                                TextBox tbox = tb as TextBox;
                                tbox.Text = string.Empty;
                            }
                        }
                        GridView_Edu.Visible = true;
                        Panel_TableEdu.Visible = false;
                        GridView_Edu.DataBind();
                    }
                    else if (MultiView1.ActiveViewIndex == 3)
                    {
                        foreach (object tb in Panel_Exam.Controls)
                        {
                            if (tb is TextBox)
                            {
                                TextBox tbox = tb as TextBox;
                                tbox.Text = string.Empty;
                            }
                        }

                        GridView_Exam.Visible = true;
                        Panel_Exam.Visible = false;
                        GridView_Exam.DataBind();
                    }
                    else if (MultiView1.ActiveViewIndex == 4)
                    {
                        foreach (object tb in Panel_Career.Controls)
                        {
                            if (tb is TextBox)
                            {
                                TextBox tbox = tb as TextBox;
                                tbox.Text = string.Empty;
                            }
                        }

                        GridView_Career.Visible = true;
                        Panel_Career.Visible = false;
                        GridView_Career.DataBind();
                    }
                    else if (MultiView1.ActiveViewIndex == 5)
                    {
                        foreach (object tb in Panel_efficiency.Controls)
                        {
                            if (tb is TextBox)
                            {
                                TextBox tbox = tb as TextBox;
                                tbox.Text = string.Empty;
                            }
                        }

                        GridView_efficiency.Visible = true;
                        Panel_efficiency.Visible = false;
                        GridView_efficiency.DataBind();
                    }
                    else if (MultiView1.ActiveViewIndex == 6)
                    {
                        foreach (object tb in Panel_FAMILY.Controls)
                        {
                            if (tb is TextBox)
                            {
                                TextBox tbox = tb as TextBox;
                                tbox.Text = string.Empty;
                            }
                        }
                        RadioButtonList_MZ_ISINSURANCE.SelectedValue = "N";
                        DropDownList_Family_INSURANCEMODE.SelectedValue = "1";

                        GridView_FAMILY.Visible = true;
                        Panel_FAMILY.Visible = false;
                        GridView_FAMILY.DataBind();
                    }
                    else if (MultiView1.ActiveViewIndex == 7)
                    {
                        foreach (object tb in Panel_Card.Controls)
                        {
                            if (tb is TextBox)
                            {
                                TextBox tbox = tb as TextBox;
                                tbox.Text = string.Empty;
                            }
                        }

                    }
                    #endregion 各view控制項

                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btDelete.Enabled = true;
                    btCancel.Enabled = false;
                    btUpdate.Enabled = true;
                }
                catch(Exception ex)
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
                finally
                {
                    conn.Close();
                    //XX2013/06/18 
                    conn.Dispose();
                }





            }
        }
        /// <summary>
        /// 刪除差勤線上簽核權限
        /// </summary>
        /// <param name="MZ_ID">身分證號</param>
        private void deleteREVIEW_MANAGEMENT(string MZ_ID)
        {
            string strSQL = string.Format("DELETE FROM C_REVIEW_MANAGEMENT WHERE MZ_ID='{0}'", MZ_ID);

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);
        }

        private string strLDAPIP_S
        {
            get
            {                
                        string strSQL = "SELECT TP_LDAPIP FROM TP_LDAPIP ORDER BY TP_ORDERBY ASC";

                        DataTable ADtoAdt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");                       
                        if (ADtoAdt.Rows.Count == 1)
                        {
                            string LDAPIP = ADtoAdt.Rows[0]["TP_LDAPIP"].ToString();
                            return LDAPIP;
                        }
                        else
                        { return ""; }
                    
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
                string _strADID = TextBox_MZ_ID.Text.Substring(0, 1) + TextBox_MZ_ID.Text.Substring(5, 5);
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
                string _strADID = TextBox_MZ_ID.Text.Substring(0, 1) + TextBox_MZ_ID.Text.Substring(5, 5);
                DirectoryEntry de = new DirectoryEntry("" + Login.strTP_LDAPIP(0, _strADID) + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                de.Children.Find(ad);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected bool chkADroot(string ad, string unit, string cn)
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry("" + strLDAPIP_S + "", "" + Login.strLDAP_ADID() + "", "" + Login.strLDAP_ADPWD() + "");
                de.Children.Find(cn + "," + unit + "," + ad);
                return true;
            }
            catch
            {
                //Label_MSG.Text += "找不到AD伺服器的帳號";
                Session["Label_MSG"] += "找不到AD伺服器的帳號";
                return false;
            }
        }

        #region 功能列button切換
        //基本
        protected void btBasic_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            ChangeButtonColor(Panel_ButtonChange, btBasic);

            ChangeButtomKind();

            btOK.Enabled = false;
            chk_TPMGroup();

            if (ViewState["Mode"] != null)
            {
                if (MultiView1.ActiveViewIndex < 2)
                {
                    //鎖定 新增 確認 更新 按鈕
                    //解除鎖定 取消 按鈕
                    btInsert.Enabled = false;
                    btCancel.Enabled = true;
                    btUpdate.Enabled = false;
                    btOK.Enabled = false;
                }
                else
                {

                }
            }

        }
        //現職
        protected void btNow_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;

            ChangeButtonColor(Panel_ButtonChange, btNow);
            if (btCancel.Enabled != true)
            {
                ChangeButtomKind();

            }
            else
            {
                btOK.Enabled = true;
            }

            btInsert.Enabled = false;

            chk_TPMGroup();

            if (ViewState["Mode"] != null)
            {
                if (MultiView1.ActiveViewIndex < 2)
                {
                    btInsert.Enabled = false;
                    btCancel.Enabled = true;
                    btUpdate.Enabled = false;
                    btOK.Enabled = true;
                    btDelete.Enabled = false;
                }
            }


        }
        //學歷
        protected void btEducation_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
            ChangeButtonColor(Panel_ButtonChange, btEducation);
            GridView_Edu.Visible = true;
            Panel_TableEdu.Visible = false;
            ChangeButtomKind();
            chk_TPMGroup();
        }
        //考試
        protected void btExam_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
            ChangeButtonColor(Panel_ButtonChange, btExam);
            GridView_Exam.Visible = true;
            Panel_Exam.Visible = false;
            ChangeButtomKind();
            chk_TPMGroup();
        }
        //經歷
        protected void btCareer_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
            ChangeButtonColor(Panel_ButtonChange, btCareer);
            GridView_Career.Visible = true;
            Panel_Career.Visible = false;
            ChangeButtomKind();
            chk_TPMGroup();
        }
        //考績
        protected void btefficiency_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
            ChangeButtonColor(Panel_ButtonChange, btefficiency);
            GridView_efficiency.Visible = true;
            Panel_efficiency.Visible = false;
            ChangeButtomKind();
            chk_TPMGroup();
        }
        //眷屬
        protected void btFamily_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 6;
            ChangeButtonColor(Panel_ButtonChange, btFamily);
            GridView_FAMILY.Visible = true;
            Panel_FAMILY.Visible = false;
            ChangeButtomKind();
            chk_TPMGroup();
        }
        //服務證
        protected void btCard_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 7;
            ChangeButtonColor(Panel_ButtonChange, btCard);
            ChangeButtomKind();

            string selectstring = "select * from a_police where mz_id='" + TextBox_MZ_ID.Text + "' ORDER BY MZ_DATE DESC";

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(selectstring, "getcarddata");

            if (dt.Rows.Count > 0)
            {
                TextBox_MZ_IDNO.Text = dt.Rows[0]["MZ_IDNO"].ToString();

                DropDownList_MZ_INO.SelectedValue = dt.Rows[0]["MZ_INO"].ToString();

                DropDownList_MZ_MEMO1.SelectedValue = dt.Rows[0]["MZ_MEMO1"].ToString();

                TextBox_MZ_DATE1.Text = dt.Rows[0]["MZ_DATE"].ToString();

                TextBox_MZ_NO1.Text = dt.Rows[0]["MZ_NO1"].ToString();

                DropDownList_MZ_INO.Enabled = false;

                DropDownList_MZ_MEMO1.Enabled = false;

            }


            //if (!string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM")))
            //{
            //}
        }
        #endregion

       

        
        /// <summary>
        /// 切換按鈕ENABLED狀態
        /// </summary>
        protected void ChangeButtomKind()
        {
            if (ViewState["Mode"] != null)
            {
                btInsert.Enabled = true;

                dataNext = Session["datanext"] as List<string>;

                if (xcount.Text != "")
                {
                    if (int.Parse(xcount.Text) == 0 && dataNext.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text) > 0 && int.Parse(xcount.Text) < dataNext.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text) > 1 && int.Parse(xcount.Text) == dataNext.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }
                }
            }

            btInsert.Enabled = true;
            btUpdate.Enabled = true;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            btCancel.Enabled = false;
        }
        
        /// <summary>
        /// 更換導覽列BUTTON顏色
        /// </summary>
        /// <param name="PA1"></param>
        /// <param name="bn1"></param>
        protected void ChangeButtonColor(Panel PA1, Button bn1)
        {
            if (MultiView1.ActiveViewIndex == 7)
            {
                btInsert.Visible = false;
                btDelete.Visible = false;
                btUpdate.Visible = false;
                btOK.Visible = false;
                btCancel.Visible = false;
            }
            else
            {
                btInsert.Visible = true;
                //btDelete.Visible = true;
                btUpdate.Visible = true;
                btOK.Visible = true;
                btCancel.Visible = true;
            }

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
        //刪除
        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
            {
                if (xcount.Text != "" && !string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text) + "'")))
                {
                    sqlString = "DELETE FROM A_DLBASE WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢或新增資料');", true);
                    return;
                }

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                    dataNext = Session["datanext"] as List<string>;   //重讀紀錄在LIST裡的身分證號
                    dataNext.Remove(TextBox_MZ_ID.Text.Trim());       //刪除該筆在LIST裡的身分證號
                    if (dataNext.Count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                    }
                    else
                    {
                        //刪除完如LIST裡還有多筆資料！重讀回第一筆資料
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        xcount1.Text = "0";
                        findPic(int.Parse(xcount1.Text));
                        if (dataNext.Count > 1)
                        {
                            btNEXT.Enabled = true;
                        }
                        upper_Table_Enable_False();
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + dataNext.Count.ToString() + "筆";
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                    }
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }

            }
            //MULTIVIEW 第三頁以後都是透過GRIDVIEW做刪除動作！

                #region 刪除--學歷
            else if (MultiView1.ActiveViewIndex == 2)//學歷
            {
                if (GridView_Edu.Visible == true)
                {
                    if (GridView_Edu.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                        return;
                    }
                    else if (GridView_Edu.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                        return;
                    }
                    else if (GridView_Edu.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_Edu.SelectedRow == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                        return;
                    }
                    else
                    {
                        sqlString = "DELETE FROM A_EDUCATION WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_SCHOOL='" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kcode from a_ktype where mz_ktype = 'ORG' and mz_kchi = '" + GridView_Edu.SelectedRow.Cells[0].Text.Trim() + "'") + "' ";
                    }
                }
                else
                {
                    sqlString = "DELETE FROM A_EDUCATION WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_SCHOOL='" + o_str.tosql(TextBox_MZ_SCHOOL.Text.Trim()) + "' ";
                }
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);

                    GridView_Edu.Visible = true;
                    Panel_TableEdu.Visible = false;
                    GridView_Edu.DataBind();
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
                #endregion 刪除--學歷

                #region 刪除--考試
            else if (MultiView1.ActiveViewIndex == 3)
            {
                if (GridView_Exam.Visible == true)
                {
                    if (GridView_Exam.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                        return;
                    }
                    else if (GridView_Exam.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                        return;
                    }
                    else if (GridView_Exam.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_Exam.SelectedRow == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                        return;
                    }
                    else
                    {
                        sqlString = "DELETE FROM A_EXAM WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND EXAM_NAME='" + o_DBFactory.ABC_toTest.vExecSQL("select mz_kcode from a_ktype where mz_ktype = 'EXK' and mz_kchi = '" + GridView_Exam.SelectedRow.Cells[1].Text.Trim() + "'") + "' ";
                    }
                }
                else
                {
                    sqlString = "DELETE FROM A_EXAM WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND EXAM_NAME='" + o_str.tosql(TextBox_EXAM_NAME.Text.Trim()) + "' ";
                }
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);

                    GridView_Exam.Visible = true;
                    Panel_Exam.Visible = false;
                    GridView_Exam.DataBind();
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
                #endregion 刪除--考試
                          
                #region 刪除--經歷
            else if (MultiView1.ActiveViewIndex == 4)
            {
                if (GridView_Career.Visible == true)
                {
                    if (GridView_Career.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                        return;
                    }
                    else if (GridView_Career.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                        return;
                    }
                    else if (GridView_Career.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_Career.SelectedRow == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                        return;
                    }
                    else
                    {
                        sqlString = "DELETE FROM A_CAREER WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_AD='" + GridView_Career.SelectedRow.Cells[0].Text.Trim() + "' AND MZ_UNIT='" + GridView_Career.SelectedRow.Cells[1].Text.Trim() + "' AND MZ_OCCC='" + GridView_Career.SelectedRow.Cells[2].Text.Trim() + "'";
                    }
                }
                else
                {
                    sqlString = "DELETE FROM A_CAREER WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_AD='" + o_str.tosql(TextBox_CAREER_AD.Text.Trim()) + "' AND MZ_UNIT='" + o_str.tosql(TextBox_CAREER_UNIT.Text.Trim()) + "' AND MZ_OCCC='" + TextBox_CAREER_OCCC.Text.Trim() + "'";
                }

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);

                    GridView_Career.Visible = true;
                    Panel_Career.Visible = false;
                    GridView_Career.DataBind();

                }
                catch (Exception)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
            #endregion 刪除--經歷

                #region 刪除--考績
            else if (MultiView1.ActiveViewIndex == 5)
            {
                if (GridView_efficiency.Visible == true)
                {
                    if (GridView_efficiency.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                        return;
                    }
                    else if (GridView_efficiency.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                        return;
                    }
                    else if (GridView_efficiency.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_efficiency.SelectedRow == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                        return;
                    }
                    else
                    {
                        sqlString = "DELETE FROM A_EFFICIENCY WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND T01='" + Label_Year.Text + "'";
                    }
                }
                else
                {
                    sqlString = "DELETE FROM A_EFFICIENCY WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND T01='" + Label_Year.Text + "'";
                }

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);

                    GridView_efficiency.Visible = true;
                    Panel_efficiency.Visible = false;
                    GridView_efficiency.DataBind();
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
            #endregion 刪除--考績

                #region 刪除--眷屬
            else if (MultiView1.ActiveViewIndex == 6)
            {
                if (GridView_FAMILY.Visible == true)
                {
                    if (GridView_FAMILY.Rows.Count == 0 && TextBox_MZ_ID.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
                        return;
                    }
                    else if (GridView_FAMILY.Rows.Count == 0 && TextBox_MZ_ID.Text.Trim() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入資料')", true);
                        return;
                    }
                    else if (GridView_FAMILY.Rows.Count > 0 && TextBox_MZ_ID.Text != "" && GridView_FAMILY.SelectedRow == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                        return;
                    }
                    else
                    {
                        sqlString = "DELETE FROM A_FAMILY WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_FAMILYID='" + GridView_FAMILY.SelectedRow.Cells[0].Text.Trim() + "'";
                    }
                }
                else
                {
                    sqlString = "DELETE FROM A_FAMILY WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_FAMILYID='" + TextBox_MZ_FAMILYID.Text.Trim() + "'";
                }

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);

                    GridView_FAMILY.Visible = true;
                    Panel_FAMILY.Visible = false;
                    GridView_FAMILY.DataBind();
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
                
            }
                #endregion 刪除--眷屬
            //服務證只有一對一關係！
            else if (MultiView1.ActiveViewIndex == 7)
            {
                string DeleteString = "DELETE FROM A_POLICE WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                    foreach (object tb in Panel_Card.Controls)
                    {
                        if (tb is TextBox)
                        {
                            TextBox tbox = tb as TextBox;
                            tbox.Text = string.Empty;
                        }
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }

            btInsert.Enabled = true;
            btUpdate.Enabled = true;
            btOK.Enabled = false;
            btDelete.Enabled = false;
        }
        //載入相片
        protected void btPicture_Click(object sender, EventArgs e)
        {
            string findID = "SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";
            DataTable findIDTable = o_DBFactory.ABC_toTest.Create_Table(findID, "findIDTable");
            if (TextBox_MZ_ID.Text.Trim() != "" || findIDTable.Rows.Count != 0)
            {
                HttpCookie Cookie1 = new HttpCookie("PersonalPicInput_ID");
                Cookie1.Value = TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim());
                Response.Cookies.Add(Cookie1);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalPicInput.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','pop','Width=300,Height=130,left=450,top=350')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料')", true);
            }
        }
        //取消
        protected void btCancel_Click(object sender, EventArgs e)
        {
            //在新增或修改狀態的模式不同！新增直接回到空白頁！修改則重讀同筆資料
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);

                    txt_Account.Enabled = false;
                }
                else if (MultiView1.ActiveViewIndex == 2)//學歷
                {
                    GridView_Edu.Visible = true;
                    Panel_TableEdu.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 3)
                {
                    GridView_Exam.Visible = true;
                    Panel_Exam.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 4)
                {
                    GridView_Career.Visible = true;
                    Panel_Career.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 5)
                {
                    GridView_efficiency.Visible = true;
                    Panel_efficiency.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 6)
                {
                    GridView_FAMILY.Visible = true;
                    Panel_FAMILY.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 7)
                {
                    foreach (object tb in Panel_Card.Controls)
                    {
                        if (tb is TextBox)
                        {
                            TextBox tbox = tb as TextBox;
                            tbox.Text = string.Empty;
                        }
                    }
                }

                btInsert.Enabled = true;
                btUpdate.Enabled = true;
                btOK.Enabled = false;
                btDelete.Enabled = false;
                btCancel.Enabled = false;
            }
            else if (ViewState["Mode"].ToString() == "UPDATE")
            {
                btCareer.Enabled = true;
                btEducation.Enabled = true;
                btefficiency.Enabled = true;
                btExam.Enabled = true;
                btFamily.Enabled = true;
                btCard.Enabled = true;
                btefficiency_insert.Enabled = true;
                //20150324
                btefficiency.Enabled = true;
                //btUpdateFromPosit.Enabled = true;

                if (MultiView1.ActiveViewIndex == 0 || MultiView1.ActiveViewIndex == 1)
                {

                    finddata(int.Parse(xcount.Text.Trim()));
                    xcount1.Text = "0";
                    findPic(int.Parse(xcount1.Text));
                    upper_Table_Enable_False();
                    dataNext = Session["datanext"] as List<string>;
                    if (int.Parse(xcount.Text.Trim()) == 0 && dataNext.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && dataNext.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == dataNext.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < dataNext.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    A.controlEnable(ref this.Panel_Basic, false);
                    A.controlEnable(ref this.Panel_Now, false);
                }
                else if (MultiView1.ActiveViewIndex == 2)//學歷
                {
                    GridView_Edu.Visible = true;
                    Panel_TableEdu.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 3)
                {
                    GridView_Exam.Visible = true;
                    Panel_Exam.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 4)
                {
                    GridView_Career.Visible = true;
                    Panel_Career.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 5)
                {
                    GridView_efficiency.Visible = true;
                    Panel_efficiency.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 6)
                {
                    GridView_FAMILY.Visible = true;
                    Panel_FAMILY.Visible = false;
                }
                else if (MultiView1.ActiveViewIndex == 7)
                {
                    foreach (object tb in Panel_Card.Controls)
                    {
                        if (tb is TextBox)
                        {
                            TextBox tbox = tb as TextBox;
                            tbox.Text = string.Empty;
                        }
                    }
                }

                Session.Remove("PKEY_MZ_ID");
                Session.Remove("PKEY_MZ_SCHOOL");
                Session.Remove("PKEY_EXAM_NAME");
                Session.Remove("PKEY_CAREER_AD");
                Session.Remove("PKEY_CAREER_UNIT");
                Session.Remove("PKEY_CAREER_OCCC");
                Session.Remove("PKEY_MZ_FAMILYID");

                btInsert.Enabled = true;
                btUpdate.Enabled = true;
                btOK.Enabled = false;
                btDelete.Enabled = true;
                btCancel.Enabled = false;
            }
        }
        //是兼職勾選是後才能輸入兼職名稱！
        private void EXTPOS()
        {
            if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
            {
                DropDownList_MZ_EXTPOS.Enabled = true;
                TextBox_MZ_EXTPOS_SRANK.Enabled = true;
                bt_MZ_EXTPOS_SRANK.Enabled = true;
            }
            else
            {
                DropDownList_MZ_EXTPOS.SelectedValue = "";
                DropDownList_MZ_EXTPOS.Enabled = false;
                TextBox_MZ_EXTPOS_SRANK.Text = string.Empty;
                TextBox_MZ_EXTPOS_SRANK1.Text = string.Empty;
                TextBox_MZ_EXTPOS_SRANK.Enabled = false;
                bt_MZ_EXTPOS_SRANK.Enabled = false;
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EXTPOS();
        }
        //勾選是原住民後才能輸入族別
        private void ABORIGINE()
        {
            if (RadioButtonList_MZ_ABORIGINE.SelectedValue == "Y")
            {
                DropDownList_MZ_ABORIGINENAME.Enabled = true;
                RadioButtonList_MZ_ABORIGINEKIND.Enabled = true;
            }
            else
            {
                DropDownList_MZ_ABORIGINENAME.Enabled = false;
                RadioButtonList_MZ_ABORIGINEKIND.Enabled = false;
            }
        }

        protected void RadioButtonList_MZ_ABORIGINE_SelectedIndexChanged(object sender, EventArgs e)
        {
            ABORIGINE();
        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        /// <summary>
        /// 將代碼轉為中文並寫在對應得控制項
        /// </summary>
        /// <param name="Cname">預寫入的中文</param>
        /// <param name="tb1">預寫入的控制項</param>
        /// <param name="tb2">????</param>
        /// <param name="obj">下個要fcous的控制項</param>
        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                tb2.Focus();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);

            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    (obj as TextBox).Focus();
                    // ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true); 
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btEXAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXAD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_EXAD1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }
            
        protected void btPAY_AD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_PAY_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_PAY_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }
           
        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");
        }

        protected void btEXUNIT_Click(object sender, EventArgs e)
        {
            //Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入現服機關')", true);
                return;
            }
            Session["KTYPE_CID"] = TextBox_MZ_EXUNIT.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_EXUNIT1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_EXAD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btTBCD9_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBCD9, TextBox_MZ_TBCD91, "11");
        }

        protected void btOCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_OCCC, TextBox_MZ_OCCC1, "26");
        }

        protected void btRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_RANK, TextBox_MZ_RANK_1, "09");
        }

        protected void btRANK1_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_RANK1, TextBox_MZ_RANK1_1, "09");
        }

        protected void btSLVC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SLVC, TextBox_MZ_SLVC1, "64");
        }

        protected void btCHISI_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_CHISI, TextBox_MZ_CHISI1, "23");
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void btNREA_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_NREA, TextBox_MZ_NREA1, "11");
        }

        protected void btPCHIEF_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PCHIEF, TextBox_MZ_PCHIEF1, "56");
        }

        protected void btSRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SRANK, TextBox_MZ_SRANK1, "09");
        }

        protected void btTBCD3_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBCD3, TextBox_MZ_TBCD31, "57");
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim().ToUpper()) + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_EXAD);
        }

        protected void TextBox_MZ_EXAD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXAD.Text.Trim().ToUpper()) + "'");
            TextBox_MZ_EXUNIT.Text = string.Empty;
            TextBox_MZ_EXUNIT1.Text = string.Empty;
            Ktype_Cname_Check(CName, TextBox_MZ_EXAD1, TextBox_MZ_EXAD, TextBox_MZ_EXUNIT);
        }

        protected void TextBox_MZ_EXUNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXUNIT.Text, "25");

            Ktype_Cname_Check(CName, TextBox_MZ_EXUNIT1, TextBox_MZ_EXUNIT, TextBox_MZ_POLNO);
        }

        protected void TextBox_MZ_TBCD3_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBCD3.Text, "57");

            Ktype_Cname_Check(CName, TextBox_MZ_TBCD31, TextBox_MZ_TBCD3, TextBox_MZ_CITY);
        }

        protected void TextBox_MZ_TBCD9_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBCD9.Text, "11");

            Ktype_Cname_Check(CName, TextBox_MZ_TBCD91, TextBox_MZ_TBCD9, TextBox_MZ_FDATE);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, RadioButtonList_MZ_ISEXTPOS);
        }

        protected void TextBox_MZ_RANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK_1, TextBox_MZ_RANK, TextBox_MZ_RANK1);

            // sam wellsince 20201020 是否為警務人員
            if (TextBox_MZ_RANK.Text.Substring(0, 1).ToLower().Equals("g"))
            {
                if (string.IsNullOrEmpty(ddl_MZ_ISPOLICE.SelectedValue))
                {
                    ddl_MZ_ISPOLICE.SelectedValue = "Y";
                }
            }
            
        }

        protected void TextBox_MZ_RANK1_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK1_1, TextBox_MZ_RANK1, TextBox_MZ_TBCD9);
        }

        protected void TextBox_MZ_SRANK_TextChanged(object sender, EventArgs e)
        {
            //20140801  為什麼不用這個?
            //TextBox test = (TextBox)sender;
            //string strtest = test.Text;

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_SRANK1, TextBox_MZ_SRANK, TextBox_MZ_SLVC);
        }

        protected void TextBox_MZ_SLVC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SLVC.Text, "64");

            Ktype_Cname_Check(CName, TextBox_MZ_SLVC1, TextBox_MZ_SLVC, TextBox_MZ_SPT);
        }

        protected void TextBox_MZ_CHISI_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_CHISI.Text, "23");

            Ktype_Cname_Check(CName, TextBox_MZ_CHISI1, TextBox_MZ_CHISI, TextBox_MZ_PESN);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_NREA);
        }

        protected void TextBox_MZ_NREA_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NREA.Text, "11");

            Ktype_Cname_Check(CName, TextBox_MZ_NREA1, TextBox_MZ_NREA, TextBox_MZ_PCHIEF);
        }

        protected void TextBox_MZ_PCHIEF_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PCHIEF.Text, "56");

            Ktype_Cname_Check(CName, TextBox_MZ_PCHIEF1, TextBox_MZ_PCHIEF, TextBox_MZ_ENAME);
        }

        protected void TextBox_CAREER_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_CAREER_AD.Text.Trim().ToUpper()) + "'");
            Ktype_Cname_Check(CName, TextBox_CAREER_AD1, TextBox_CAREER_AD, TextBox_CAREER_UNIT);
        }

        protected void TextBox_CAREER_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_UNIT.Text, "25");

            Ktype_Cname_Check(CName, TextBox_CAREER_UNIT1, TextBox_CAREER_UNIT, TextBox_CAREER_RANK);
        }

        protected void TextBox_CAREER_RANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_RANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_CAREER_RANK_1, TextBox_CAREER_RANK, TextBox_CAREER_RANK1);
        }

        protected void TextBox_CAREER_RANK1_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_RANK1.Text, "09");

            Ktype_Cname_Check(CName, TextBox_CAREER_RANK1_1, TextBox_CAREER_RANK1, TextBox_CAREER_OCCC);
        }

        protected void TextBox_CAREER_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_CAREER_OCCC1, TextBox_CAREER_OCCC, TextBox_CAREER_TBDV);
        }

        protected void TextBox_CAREER_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_CAREER_TBDV1, TextBox_CAREER_TBDV, TextBox_CAREER_CHISI);
        }

        protected void TextBox_CAREER_CHISI_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_CHISI.Text, "23");

            Ktype_Cname_Check(CName, TextBox_CAREER_CHISI1, TextBox_CAREER_CHISI, TextBox_CAREER_POSIND);
        }

        protected void TextBox_CAREER_PCHIEF_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_PCHIEF.Text, "56");

            Ktype_Cname_Check(CName, TextBox_CAREER_PCHIEF1, TextBox_CAREER_PCHIEF, TextBox_CAREER_NRT);
        }

        protected void TextBox_CAREER_NREA_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_NREA.Text, "11");

            Ktype_Cname_Check(CName, TextBox_CAREER_NREA1, TextBox_CAREER_NREA, TextBox_CAREER_EXID);
        }

        protected void TextBox_CAREER_TBNREA_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_TBNREA.Text, "11");

            Ktype_Cname_Check(CName, TextBox_CAREER_TBNREA1, TextBox_CAREER_TBNREA, TextBox_CAREER_TBID);
        }

        protected void btCAREER_AD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_CAREER_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_CAREER_AD1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btCAREER_UNIT_Click(object sender, EventArgs e)
        {

            Ktype_Search(TextBox_CAREER_UNIT, TextBox_CAREER_UNIT1, "25");
       }

        protected void btCAREER_RANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_RANK, TextBox_CAREER_RANK_1, "09");
        }

        protected void btCAREER_RANK1_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_RANK1, TextBox_CAREER_RANK1_1, "09");
        }

        protected void btCAREER_OCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_OCCC, TextBox_CAREER_OCCC1, "26");
        }

        protected void btCAREER_TBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_TBDV, TextBox_CAREER_TBDV1, "43");
        }

        protected void btCAREER_CHISI_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_PCHIEF, TextBox_CAREER_PCHIEF1, "56");
        }

        protected void btCAREER_PCHIEF_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_PCHIEF, TextBox_CAREER_PCHIEF1, "56");
        }

        protected void btCAREER_NREA_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_NREA, TextBox_CAREER_NREA1, "11");
        }

        protected void btCAREER_TBNREA_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_TBNREA, TextBox_CAREER_TBNREA1, "11");
        }

        protected void btPESN_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PESN, TextBox_MZ_PESN1, "05");
        }

        protected void btNRT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_NRT, TextBox_MZ_NRT1, "53");
        }

        protected void btNIN_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_NIN, TextBox_MZ_NIN1, "54");
        }

        protected void TextBox_MZ_PESN_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PESN.Text, "05");

            Ktype_Cname_Check(CName, TextBox_MZ_PESN1, TextBox_MZ_PESN, TextBox_MZ_NRT);
        }

        protected void TextBox_MZ_NRT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NRT.Text, "53");

            Ktype_Cname_Check(CName, TextBox_MZ_NRT1, TextBox_MZ_NRT, TextBox_MZ_NIN);
        }

        protected void TextBox_MZ_NIN_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NIN.Text, "54");

            Ktype_Cname_Check(CName, TextBox_MZ_NIN1, TextBox_MZ_NIN, DropDownList_MZ_PNO);//////////////////////////////???沒有用再兼勤區
        }

        protected void TextBox_CAREER_PESN_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_PESN.Text, "05");

            Ktype_Cname_Check(CName, TextBox_CAREER_PESN1, TextBox_CAREER_PESN, TextBox_CAREER_PCHIEF);
        }

        protected void TextBox_MZ_SCHOOL_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                              " AND (dbo.SUBSTR(MZ_KCODE,10,1)='Q' OR dbo.SUBSTR(MZ_KCODE,10,1)='R' OR dbo.SUBSTR(MZ_KCODE,10,1)='U' OR dbo.SUBSTR(MZ_KCODE,10,1)='X' OR dbo.SUBSTR(MZ_KCODE,10,1)='Y' OR dbo.SUBSTR(MZ_KCODE,10,1)='T') " +
                                                              " AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_SCHOOL.Text.Trim().ToUpper()) + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_SCHOOL1, TextBox_MZ_SCHOOL, TextBox_MZ_EDUCLASS);
        }

        protected void TextBox_MZ_EDUCLASS_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EDUCLASS.Text, "@14");

            Ktype_Cname_Check(CName, TextBox_MZ_EDUCLASS1, TextBox_MZ_EDUCLASS, TextBox_MZ_EDULEVEL);
        }

        protected void TextBox_MZ_DEPARTMENT_TextChanged(object sender, EventArgs e)
        {
            string CName = "";

            if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "5"
             || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "6"
             || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "7")
            {
                CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_DEPARTMENT.Text, "DP1");
            }
            else if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "4")
            {
                CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_DEPARTMENT.Text, "DP2");
            }
            else if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "3")
            {
                CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_DEPARTMENT.Text, "DP3");
            }

            Ktype_Cname_Check(CName, TextBox_MZ_DEPARTMENT1, TextBox_MZ_DEPARTMENT, TextBox_MZ_YEAR);
        }

        protected void TextBox_MZ_EDULEVEL_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EDULEVEL.Text, "EDL");

            Ktype_Cname_Check(CName, TextBox_MZ_EDULEVEL1, TextBox_MZ_EDULEVEL, TextBox_MZ_DEPARTMENT);


            if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "3"
              || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "4"
              || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "5"
              || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "6"
              || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "7")
            {
                TextBox_MZ_DEPARTMENT.Enabled = true;
                btDEPARTMENT.Enabled = true;
            }
            else
            {
                TextBox_MZ_DEPARTMENT.Enabled = true;
                btDEPARTMENT.Enabled = true;
                //TextBox_MZ_DEPARTMENT.Enabled = false;
                //btDEPARTMENT.Enabled = false;
            }
        }

        protected void TextBox_MZ_EDUKIND_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EDUKIND.Text, "EDT");

            Ktype_Cname_Check(CName, TextBox_MZ_EDUKIND1, TextBox_MZ_EDUKIND, TextBox_MZ_EDUKIND);
        }

        protected void TextBox_EXAM_NAME_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_EXAM_NAME.Text, "EXK");

            Ktype_Cname_Check(CName, TextBox_EXAM_NAME1, TextBox_EXAM_NAME, TextBox_EXAM_CLASS);
        }

        protected void TextBox_EXAM_CLASS_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_EXAM_CLASS.Text, "EXS");

            Ktype_Cname_Check(CName, TextBox_EXAM_CLASS1, TextBox_EXAM_CLASS, TextBox_EXAM_ADMISSION);
        }

        protected void TextBox_EXAM_ADMISSION_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_EXAM_ADMISSION.Text, "EXG");

            Ktype_Cname_Check(CName, TextBox_EXAM_ADMISSION1, TextBox_EXAM_ADMISSION, TextBox_EXAM_DOCUMENTS);
        }

        protected void TextBox_MZ_TITLE_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TITLE.Text, "FAP");

            Ktype_Cname_Check(CName, TextBox_MZ_TITLE1, TextBox_MZ_TITLE, TextBox_MZ_FAMILYNAME);
        }

        protected void TextBox_MZ_WORK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_WORK.Text, "@03");

            Ktype_Cname_Check(CName, TextBox_MZ_WORK1, TextBox_MZ_WORK, RadioButtonList_MZ_ISINSURANCE);
        }

        protected void btSCHOOL_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_SCHOOL.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_SCHOOL1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=SCH&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btDEPARTMENT_Click(object sender, EventArgs e)
        {
            if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "5"
               || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "6"
               || TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "7")
            {
                Ktype_Search(TextBox_MZ_DEPARTMENT, TextBox_MZ_DEPARTMENT1, "DP1");
            }
            else if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "4")
            {
                Ktype_Search(TextBox_MZ_DEPARTMENT, TextBox_MZ_DEPARTMENT1, "DP2");
            }
            else if (TextBox_MZ_EDULEVEL.Text.Trim().ToUpper().Substring(0, 1) == "3")
            {
                Ktype_Search(TextBox_MZ_DEPARTMENT, TextBox_MZ_DEPARTMENT1, "DP3");
            }
        }

        protected void btEDUCLASS_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EDUCLASS, TextBox_MZ_EDUCLASS1, "@14");
        }

        protected void btEDULEVEL_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EDULEVEL, TextBox_MZ_EDULEVEL1, "EDL");
        }

        protected void btEDUKIND_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EDUKIND, TextBox_MZ_EDUKIND1, "EDT");
        }

        protected void btEXAM_NAME_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_EXAM_NAME, TextBox_EXAM_NAME1, "EXK");
        }

        protected void btEXAM_CLASS_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_EXAM_CLASS, TextBox_EXAM_CLASS1, "EXS");
        }

        protected void btEXAM_ADMISSION_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_EXAM_ADMISSION, TextBox_EXAM_ADMISSION1, "EXG");
        }

        protected void btTITLE_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TITLE, TextBox_MZ_TITLE1, "FAP");
        }

        protected void btWORK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_WORK, TextBox_MZ_WORK1, "@03");
        }

        protected void btARMYSTATE_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_ARMYSTATE, TextBox_MZ_ARMYSTATE1, "MIC");
        }

        protected void btARMYRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_ARMYRANK, TextBox_MZ_ARMYRANK1, "MIK");
        }

        protected void btARMYKIND_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_ARMYKIND, TextBox_MZ_ARMYKIND1, "MID");
        }

        protected void btARMYCOURSE_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_ARMYCOURSE, TextBox_MZ_ARMYCOURSE1, "MIR");
        }

        protected void TextBox_MZ_ARMYSTATE_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_ARMYSTATE.Text, "MIC");

            Ktype_Cname_Check(CName, TextBox_MZ_ARMYSTATE1, TextBox_MZ_ARMYSTATE, TextBox_MZ_ARMYRANK);
        }

        protected void TextBox_MZ_ARMYRANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_ARMYRANK.Text, "MIK");

            Ktype_Cname_Check(CName, TextBox_MZ_ARMYRANK1, TextBox_MZ_ARMYRANK, TextBox_MZ_ARMYKIND);
        }

        protected void TextBox_MZ_ARMYKIND_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_ARMYKIND.Text, "MID");

            Ktype_Cname_Check(CName, TextBox_MZ_ARMYKIND1, TextBox_MZ_ARMYKIND, TextBox_MZ_ARMYCOURSE);
        }

        protected void TextBox_MZ_ARMYCOURSE_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_ARMYCOURSE.Text, "MIR");

            Ktype_Cname_Check(CName, TextBox_MZ_ARMYCOURSE1, TextBox_MZ_ARMYCOURSE, TextBox_MZ_SLFDATE);
        }

        protected void btCAREER_PESN_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_PESN, TextBox_CAREER_PESN1, "05");
        }

        protected void btCAREER_NRT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_NRT, TextBox_CAREER_NRT1, "54");
        }

        protected void btCAREER_NIN_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_CAREER_NIN, TextBox_CAREER_NIN1, "54");
        }

        protected void TextBox_CAREER_NRT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_NRT.Text, "53");

            Ktype_Cname_Check(CName, TextBox_CAREER_NRT1, TextBox_CAREER_NRT, TextBox_CAREER_DATE);
        }

        protected void TextBox_CAREER_NIN_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_CAREER_NIN.Text, "54");

            Ktype_Cname_Check(CName, TextBox_CAREER_NIN1, TextBox_CAREER_NIN, TextBox_CAREER_NIN);
        }

        protected void TextBox_MZ_NAME_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_NAME, TextBox_MZ_ID, "姓名");
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_ID, TextBox_MZ_AD, "身分證號");
        }

        protected void TextBox_MZ_SPT_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_SPT, TextBox_MZ_SPT1, "俸點");
        }

        protected void RadioButtonList_MZ_ISINSURANCE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList_MZ_ISINSURANCE.SelectedValue == "Y")
            {
                DropDownList_Family_INSURANCEMODE.Enabled = true;
            }
            else
            {
                DropDownList_Family_INSURANCEMODE.Enabled = false;
            }
        }

        protected void returnSameDataType(TextBox tb, TextBox tb1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

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
                   
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                }
            }
        }

        protected void TextBox_MZ_ADATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ADATE, TextBox_MZ_LDATE);
        }

        protected void TextBox_MZ_FDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_FDATE, TextBox_MZ_TBDV);
        }

        /// <summary>
        /// 重新計算年度休假日數
        /// 改為合格實授日計算 20181120 by sky
        /// 調整計算公式為共用函數 20190124 by sky
        /// </summary>
        /// <param name="MZ_ID">人員ID</param>
        /// <param name="MZ_QUA_DATE">合格實授日</param>
        protected void ChangeHday(string MZ_ID, string MZ_QUA_DATE)
        {
            string MZ_YEAR = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

            string c = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DLTBB WHERE MZ_ID='" + MZ_ID + "' AND MZ_YEAR='" + MZ_YEAR + "'");
            if (c == "1")
            {
                #region 刪除該段程式，改用共用函數處理 20190124 by sky
                //string strSQL = "SELECT MZ_TYEAR,MZ_TMONTH FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

                //DataTable dt = new DataTable();
                //dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                //int tyear = int.Parse(string.IsNullOrEmpty(dt.Rows[0]["MZ_TYEAR"].ToString()) ? "0" : dt.Rows[0]["MZ_TYEAR"].ToString());

                //int tmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[0]["MZ_TMONTH"].ToString()) ? "0" : dt.Rows[0]["MZ_TMONTH"].ToString());

                //DateTime FDATE;

                //try
                //{
                //    FDATE = DateTime.Parse((int.Parse(MZ_QUA_DATE.Substring(0, 3)) + 1911).ToString() + "-" + MZ_QUA_DATE.Substring(3, 2) + "-01");
                //}
                //catch
                //{
                //    FDATE = DateTime.Now;
                //}

                //int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Parse((DateTime.Now.Year).ToString() + "-01-01"), Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

                //int MONTH = monthDiff;

                //int sday3;

                //int sday3_hour = 0;

                //int addMONTH = tyear * 12 + tmonth;

                //if (MONTH < 0)
                //{
                //    sday3 = 0;
                //}
                //else
                //{
                //    if (MONTH < 12)
                //    {
                //        //if (addMONTH == 0 && FDATE.Month == 1)
                //        //{
                //        //    sday3 = 7;
                //        //}
                //        //else
                //        //{
                //        if (addMONTH == 0)
                //        {
                //            double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

                //            string[] s = countsay.ToString().Split('.');

                //            sday3 = int.Parse(s[0]);

                //            if (s.Length == 2)
                //            {
                //                if (int.Parse(s[1]) > 5)
                //                {
                //                    sday3 = sday3 + 1;
                //                }
                //                else if (int.Parse(s[1]) > 0)
                //                {
                //                    sday3_hour = 4;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (addMONTH + MONTH < 36)
                //            {
                //                double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

                //                string[] s = countsay.ToString().Split('.');

                //                sday3 = int.Parse(s[0]);
                //                if (s.Length == 2)
                //                {
                //                    if (int.Parse(s[1]) > 5)
                //                    {
                //                        sday3 = sday3 + 1;
                //                    }
                //                    else if (int.Parse(s[1]) > 0)
                //                    {
                //                        sday3_hour = 4;
                //                    }
                //                }
                //            }
                //            else if (36 <= addMONTH + MONTH && addMONTH + MONTH < 72)
                //            {
                //                double countsay = MathHelper.Round(14 * double.Parse(MONTH.ToString()) / 12, 1);

                //                string[] s = countsay.ToString().Split('.');

                //                sday3 = int.Parse(s[0]);
                //                if (s.Length == 2)
                //                {
                //                    if (int.Parse(s[1]) > 5)
                //                    {
                //                        sday3 = sday3 + 1;
                //                    }
                //                    else if (int.Parse(s[1]) > 0)
                //                    {
                //                        sday3_hour = 4;
                //                    }
                //                }
                //            }
                //            else if (72 <= addMONTH + MONTH && addMONTH + MONTH < 108)
                //            {
                //                double countsay = MathHelper.Round(21 * double.Parse(MONTH.ToString()) / 12, 1);

                //                string[] s = countsay.ToString().Split('.');

                //                sday3 = int.Parse(s[0]);

                //                if (s.Length == 2)
                //                {
                //                    if (int.Parse(s[1]) > 5)
                //                    {
                //                        sday3 = sday3 + 1;
                //                    }
                //                    else if (int.Parse(s[1]) > 0)
                //                    {
                //                        sday3_hour = 4;
                //                    }
                //                }
                //            }
                //            else if (108 <= addMONTH + MONTH && addMONTH + MONTH < 148)
                //            {
                //                double countsay = MathHelper.Round(28 * double.Parse(MONTH.ToString()) / 12, 1);

                //                string[] s = countsay.ToString().Split('.');

                //                sday3 = int.Parse(s[0]);

                //                if (s.Length == 2)
                //                {
                //                    if (int.Parse(s[1]) > 5)
                //                    {
                //                        sday3 = sday3 + 1;
                //                    }
                //                    else if (int.Parse(s[1]) > 0)
                //                    {
                //                        sday3_hour = 4;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                double countsay = MathHelper.Round(30 * double.Parse(MONTH.ToString()) / 12, 1);

                //                string[] s = countsay.ToString().Split('.');

                //                sday3 = int.Parse(s[0]);

                //                if (s.Length == 2)
                //                {
                //                    if (int.Parse(s[1]) > 5)
                //                    {
                //                        sday3 = sday3 + 1;
                //                    }
                //                    else if (int.Parse(s[1]) > 0)
                //                    {
                //                        sday3_hour = 4;
                //                    }
                //                }
                //            }
                //        }
                //        //}
                //    }
                //    else if (12 <= MONTH + addMONTH && MONTH + addMONTH < 36)
                //    {
                //        sday3 = 7;
                //    }
                //    else if (36 <= MONTH + addMONTH && MONTH + addMONTH < 72)
                //    {
                //        sday3 = 14;
                //    }
                //    else if (72 <= MONTH + addMONTH && MONTH + addMONTH < 108)
                //    {
                //        sday3 = 21;
                //    }
                //    else if (108 <= MONTH + addMONTH && MONTH + addMONTH < 168)
                //    {
                //        sday3 = 28;
                //    }
                //    else
                //    {
                //        sday3 = 30;
                //    }
                //}

                //string UpdateString = "UPDATE C_DLTBB SET MZ_HDAY=" + sday3 + ",MZ_HTIME=" + sday3_hour + " WHERE MZ_ID='" + MZ_ID + "' AND MZ_YEAR='" + MZ_YEAR + "'";

                //try
                //{
                //    o_DBFactory.ABC_toTest.Edit_Data(UpdateString);
                //}
                //catch
                //{

                //}
                #endregion

                if (!string.IsNullOrEmpty(MZ_ID)) //確保不會空值導致全人員計算
                {
                    _3_forleave.C_ForLeave_YearVacation_Create.ComputeModel computeModel = new _3_forleave.C_ForLeave_YearVacation_Create.ComputeModel();
                    computeModel.MZ_ID = MZ_ID;
                    computeModel.StatisticsYear = MZ_YEAR;
                    if (Request.QueryString["TPM_FION"] != null)
                    {
                        computeModel.TPM_FION = Request.QueryString["TPM_FION"].ToString();
                    }

                    string errorMsg = string.Empty;
                    _3_forleave.C_ForLeave_YearVacation_Create.YearVacation(computeModel, ref errorMsg);
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + errorMsg + "');", true);
                    }
                }
            }
        }

        protected void TextBox_MZ_BIR_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_BIR, TextBox_MZ_ADATE);
        }

        protected void TextBox_MZ_SLFDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_SLFDATE, TextBox_MZ_SLEDATE);
        }

        protected void TextBox_MZ_SLEDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_SLEDATE, TextBox_MZ_SLEDATE);
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE, TextBox_MZ_IDATE);
        }

        protected void TextBox_MZ_IDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_IDATE, TextBox_MZ_POSIND);
        }

        protected void TextBox_MZ_OPEDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_OPEDATE, TextBox_MZ_FYEAR);
        }

        protected void TextBox_MZ_LDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_LDATE, TextBox_MZ_TDATE);
        }

        protected void TextBox_MZ_ODAY_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ODAY, TextBox_MZ_OFFYY);
        }

        protected void TextBox_MZ_TDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_TDATE, TextBox_MZ_ODAY);
        }

        protected void TextBox_CAREER_DATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_CAREER_DATE, TextBox_CAREER_IDATE);
        }

        protected void TextBox_CAREER_IDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_CAREER_IDATE, TextBox_CAREER_ADATE);
        }

        protected void TextBox_CAREER_ADATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_CAREER_ADATE, TextBox_CAREER_NREA);
        }

        protected void TextBox_CAREER_TBDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_CAREER_TBDATE, TextBox_CAREER_NIN);
        }

        protected void TextBox_MZ_BIRTHDAY_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_BIRTHDAY, TextBox_MZ_WORK);
        }

        protected void TextBox_MZ_DATE1_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE1, TextBox_MZ_NO1);
        }

        protected void GridView_Edu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView_Edu, "Select$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes.Add("Style", "display:none");
                e.Row.Cells[6].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView_Edu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView_Edu.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                btDelete.Enabled = true;
            }
        }

        protected void GridView_Exam_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView_Exam, "Select$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView_Exam_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView_Exam.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                btDelete.Enabled = true;
            }
        }

        protected void GridView_Career_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView_Career.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                btDelete.Enabled = true;
            }
        }

        protected void GridView_Career_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView_Career, "Select$" + e.Row.RowIndex);
                e.Row.Cells[9].Attributes.Add("Style", "display:none");
                e.Row.Cells[0].Attributes.Add("Style", "display:none");
                e.Row.Cells[1].Attributes.Add("Style", "display:none");
                e.Row.Cells[2].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView_FAMILY_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView_FAMILY, "Select$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView_FAMILY_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView_FAMILY.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                btDelete.Enabled = true;
            }
        }

        protected void GridView_efficiency_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView_efficiency, "Select$" + e.Row.RowIndex);
                e.Row.Cells[8].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView_efficiency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView_efficiency.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                Label_Year.Text = GridView_efficiency.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].Text;
                btDelete.Enabled = true;
            }
        }

        protected void btUpperPic_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount1.Text) != 0)
            {
                xcount1.Text = (Convert.ToInt32(xcount1.Text) - 1).ToString();

                findPic(int.Parse(xcount1.Text));
                if (int.Parse(xcount.Text) != Personal_PICPATH.Count - 1)
                {
                    btNextPic.Enabled = true;
                }
                if (int.Parse(xcount1.Text) == 0)
                {
                    btUpperPic.Enabled = false;
                }
            }
            else if (int.Parse(xcount1.Text) == 0)
            {
                findPic(int.Parse(xcount1.Text));
                btUpperPic.Enabled = false;
            }
        }

        protected void btNextPic_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount1.Text) == 0)
            {
                xcount1.Text = (Convert.ToInt32(xcount1.Text) + 1).ToString();
                btUpperPic.Enabled = true;

                findPic(int.Parse(xcount1.Text));

                if (int.Parse(xcount1.Text) == Personal_PICPATH.Count - 1)
                {
                    btNextPic.Enabled = false;
                }
            }
            else
            {
                xcount1.Text = (Convert.ToInt32(xcount1.Text) + 1).ToString();

                findPic(int.Parse(xcount1.Text));

                if (int.Parse(xcount1.Text) == Personal_PICPATH.Count - 1)
                {
                    btNextPic.Enabled = false;
                }
            }
        }

        protected void TextBox_MZ_FAMILYNAME_TextChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_FAMILYID.ClientID + "').focus();$get('" + TextBox_MZ_FAMILYID.ClientID + "').focus();", true);
        }

        protected void TextBox_MZ_ENDDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ENDDATE, TextBox_MZ_EDUKIND);
        }

        protected void TextBox_MZ_BEGINDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_BEGINDATE, TextBox_MZ_ENDDATE);
        }

      

        protected void btDeletePic_Click(object sender, EventArgs e)
        {
            Personal_PICPATH = Session["Personal_PICPATH"] as List<string>;

            if (Personal_PICPATH.Count == 0)
            {
                Image1.ImageUrl = "~/1-personnel/images/nopic.jpg";
            }
            else
            {
                File.Delete(Server.MapPath(Personal_PICPATH[int.Parse(xcount1.Text)]));

                string DeletePicString = "DELETE FROM A_PICPATH WHERE MZ_ID='" + TextBox_MZ_ID.Text + "' AND PICTUREPATH='" + Personal_PICPATH[int.Parse(xcount1.Text)] + "'";

                o_DBFactory.ABC_toTest.Edit_Data(DeletePicString);

                Personal_PICPATH.Remove(xcount1.Text);

                if (Personal_PICPATH.Count == 0)
                {
                    Image1.ImageUrl = "~/1-personnel/images/nopic.jpg";
                }
                else
                {
                    Image1.ImageUrl = Personal_PICPATH[Personal_PICPATH.Count - 1];
                }
            }
        }

        protected void TextBox_MZ_EXTPOS_SRANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXTPOS_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_EXTPOS_SRANK1, TextBox_MZ_EXTPOS_SRANK, TextBox_MZ_TBCD9);
        }

        protected void bt_MZ_EXTPOS_SRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXTPOS_SRANK, TextBox_MZ_EXTPOS_SRANK1, "09");
        }

        protected void btefficiency_insert_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "window.location='../2-salary/19-EffectImport.aspx'", true);
        }

        /// <summary>
        /// 合格實授日欄位調整顯示文字及檢核 20181120 by sky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_QUA_DATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_QUA_DATE, TextBox_MZ_ADATE);
        }
        /// <summary>
        /// 權理職等欄位調整顯示文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_MZ_AHP_RANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(txt_MZ_AHP_RANK.Text, "09");
            Ktype_Cname_Check(CName, txt_MZ_AHP_RANK1, txt_MZ_AHP_RANK, TextBox_MZ_NID);
        }
        /// <summary>
        /// 權理職等開啟選擇視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAHPRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(txt_MZ_AHP_RANK, txt_MZ_AHP_RANK1, "09");
        }
        /// <summary>
        /// 薪資生效日期欄位調整顯示文字及檢核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_MZ_SALARY_ISDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(txt_MZ_SALARY_ISDATE, txt_MZ_SALARY_ISDATE);
        }

        protected void btnExportSalaryMetaData_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Salary_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void TextBox_PAY_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_PAY_AD.Text.Trim().ToUpper()) + "'");
            Ktype_Cname_Check(CName, TextBox_PAY_AD1, TextBox_PAY_AD, null);

        }

        protected void btBankSet_Click(object sender, EventArgs e)
        {
            Session["Search_ID"] = this.TextBox_MZ_ID.Text.Trim();
            Response.Redirect("Personal1-1-BankSet.aspx?TPM_FION=9");
        }

        protected void btnChangSalary_Click(object sender, EventArgs e)
        {
            Session["Search_ID"] = this.TextBox_MZ_ID.Text.Trim();
            Response.Redirect("Personal1-1-Salary.aspx?TPM_FION=9");
        }
    }
}



