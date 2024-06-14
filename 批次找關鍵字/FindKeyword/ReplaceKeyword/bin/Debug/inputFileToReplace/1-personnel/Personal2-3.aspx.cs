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
    public partial class Personal2_3 : System.Web.UI.Page
    {
        List<String> PRK2_MZ_ID = new List<string>();
        List<String> PRK2_MZ_NO = new List<string>();
        List<String> PRK2_MZ_PRID = new List<string>();
        List<String> PRK2_MZ_PRID1 = new List<string>();
        List<String> PRK2_MZ_PRCT = new List<string>();

        bool checkPRK1 = true;
        bool checkPRRST = true;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();

                chk_TPMGroup();

            }

          

            //HttpCookie PRK2_PRID_Cookie = new HttpCookie("PRK2_PRID");
            //PRK2_PRID_Cookie = Request.Cookies["PRK2_PRID"];

            //if (PRK2_PRID_Cookie == null)
            //{
            //    ViewState["PRK2_PRID"] = null;
            //    Response.Cookies["PRK2_PRID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["PRK2_PRID"] = TPMPermissions._strDecod(PRK2_PRID_Cookie.Value.ToString());
            //    Response.Cookies["PRK2_PRID"].Expires = DateTime.Now.AddYears(-1);
            //}

            //身分證號還是用session
            ViewState["PRK2_ID"] = Session["PRK2_ID"];
            Session.Remove("PRK2_ID");

            //姓名 還是用session
            ViewState["PRK2_NAME"] = Session["PRK2_NAME"];
            Session.Remove("PRK2_NAME");

            //文號字
            string PRK2_PRID = "";
            if (Request["PRK2_PRID"] != null)
            {
                if (!string.IsNullOrEmpty(Request["PRK2_PRID"].ToString()))
                {
                    PRK2_PRID = HttpUtility.UrlDecode(Request["PRK2_PRID"].ToString());
                    ViewState["PRK2_PRID_2"] = PRK2_PRID;
                }
            }
            //ViewState["PRK2_PRID"] = Session["PRK2_PRID"];

            //if (ViewState["PRK2_PRID"] != null)
            //    ViewState["PRK2_PRID_2"] = Session["PRK2_PRID"];

            //Session.Remove("PRK2_PRID");

            //發文文號
            ViewState["PRK2_PRID1"] = Request["PRK2_PRID1"];

            //年度
            ViewState["PRK2_YEAR"] = Request["YEAR"];

            //獎懲依據
            string PRK2_PROLNO = "";
            if (Request["PROLNO"] != null)
            {
                if (!string.IsNullOrEmpty(Request["PROLNO"].ToString()))
                {
                    PRK2_PROLNO = Request["PROLNO"].ToString();
                }
            } 
            //ViewState["PRK2_PROLNO"] = Session["PRK2_PROLNO"];
            //Session.Remove("PRK2_PROLNO");

            //發文日期起
            string PRK2_SDATE = "";
            if (Request["SDATE"] != null)
            {
                if (!string.IsNullOrEmpty(Request["SDATE"].ToString()))
                PRK2_SDATE = Request["SDATE"].ToString();                
            } 
            //發文日期迄
            string PRK2_EDATE = "";
            if (Request["EDATE"] != null)
            {
                if (!string.IsNullOrEmpty(Request["EDATE"].ToString()))
                PRK2_EDATE = Request["EDATE"].ToString();
            }

            //ViewState["PRK2_SDATE"] = Session["PRK2_SDATE"];
           
            //ViewState["PRK2_EDATE"] = Session["PRK2_EDATE"];

            


           

           
            //沒在用
            //ViewState["PRK2_DATE"] = Request["DATE"];

            ViewState["XCOUNT"] = Request["XCOUNT"];
            //2013/12/09
            //A.set_Panel_EnterToTAB(ref this.Panel1);

            A.set_Panel_EnterToTAB(ref this.Panel2);
            #region 之前就註解

            //TextBox_MZ_ID.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_ID.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_ID.ClientID)
            //    Can_not_empty(TextBox_MZ_ID, TextBox_MZ_NAME, "身分證號");

            //TextBox_MZ_NAME.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_NAME.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_NAME.ClientID)
            //    Can_not_empty(TextBox_MZ_NAME, TextBox_MZ_IDATE, "姓名");

            //TextBox_MZ_IDATE.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_IDATE.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_IDATE.ClientID)
            //    Can_not_empty(TextBox_MZ_IDATE, TextBox_MZ_DATE, "生效日期");

            //TextBox_MZ_DATE.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_DATE.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_IDATE.ClientID)
            //    Can_not_empty(TextBox_MZ_DATE, TextBox_MZ_PRID1, "發文日期");

            //TextBox_MZ_PRID1.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PRID1.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_IDATE.ClientID)
            //    Can_not_empty(TextBox_MZ_PRID1, TextBox_MZ_PRID, "發文文號");

            //TextBox_MZ_PRID.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PRID.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_PRID.ClientID)
            //    Can_not_empty(TextBox_MZ_PRID, TextBox_MZ_CHKAD, "發文字號");

            //TextBox_MZ_CHKAD.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_CHKAD.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_CHKAD.ClientID)
            //    Can_not_empty(TextBox_MZ_CHKAD, TextBox_MZ_AD, "核定機關");

            //TextBox_MZ_AD.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_AD.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_AD.ClientID)
            //    Can_not_empty(TextBox_MZ_AD, TextBox_MZ_UNIT, "編制機關");

            //TextBox_MZ_UNIT.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_UNIT.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_UNIT.ClientID)
            //    Can_not_empty(TextBox_MZ_UNIT, TextBox_MZ_TBDV, "編制單位");

            //TextBox_MZ_TBDV.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_TBDV.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_TBDV.ClientID)
            //    Can_not_empty(TextBox_MZ_TBDV, TextBox_MZ_SRANK, "職序");

            //TextBox_MZ_OCCC.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_OCCC.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_OCCC.ClientID)
            //    Can_not_empty(TextBox_MZ_OCCC, TextBox_MZ_RANK, "職稱");

            //TextBox_MZ_RANK.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_RANK.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_RANK.ClientID)
            //    Can_not_empty(TextBox_MZ_RANK, TextBox_MZ_RANK1, "官職等起");

            //TextBox_MZ_RANK1.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_RANK1.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_RANK1.ClientID)
            //    Can_not_empty(TextBox_MZ_RANK1, TextBox_MZ_PRK, "官職等迄");

            //TextBox_MZ_PRK.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PRK.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_PRK.ClientID)
            //    Can_not_empty(TextBox_MZ_PRK, TextBox_MZ_SRANK, "獎懲類別");

            //TextBox_MZ_SRANK.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_SRANK.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_SRANK.ClientID)
            //    Can_not_empty(TextBox_MZ_SRANK, TextBox_MZ_PRCT, "薪俸職等");

            //TextBox_MZ_PRCT.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PRCT.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_PRCT.ClientID)
            //    Can_not_empty(TextBox_MZ_PRCT, TextBox_MZ_PRRST, "獎懲內容");

            //TextBox_MZ_PRRST.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PRRST.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_PRRST.ClientID)
            //    Can_not_empty(TextBox_MZ_PRRST, TextBox_MZ_PRK1, "獎懲結果");

            //TextBox_MZ_PRK1.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PRK1.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_PRK1.ClientID)
            //    Can_not_empty(TextBox_MZ_PRK1, TextBox_MZ_PROLNO, "獎懲統計分類");

            //TextBox_MZ_PROLNO.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_PROLNO.ClientID + "','')}");

            //if (Request.Form["__EVENTTARGET"] == TextBox_MZ_PROLNO.ClientID)
            //    Can_not_empty(TextBox_MZ_PROLNO, TextBox_MZ_PROLNO2, "獎懲依據");
            #endregion 之前就註解
            if (!IsPostBack)
            {

                TextBox_MZ_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SRANK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_UNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PROLNO1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PRK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PRK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PRRST1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_CHKAD1.Attributes.Add("onkeydown", "javascript:return false;");

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && PRK2_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < PRK2_MZ_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == PRK2_MZ_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (PRK2_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                        btDetail.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        btDetail.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRK2_MZ_ID.Count.ToString() + "筆";
                    }

                    btUpdate.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                }
                //else if (ViewState["PRK2_PRID"] != null)
                    else if (!string.IsNullOrEmpty (PRK2_PRID))
                {
                    string where = "";
                    string strSQL = "SELECT MZ_ID,MZ_NO,MZ_PRID,MZ_PRID1,MZ_PRCT,MZ_PRRST,MZ_NAME,MZ_DATE FROM A_PRK2 WHERE 1=1";
                    string detailSQL = @"SELECT MZ_ID,MZ_NO,MZ_PRCT,MZ_ID 身份證號,MZ_NAME 姓名,AK1.MZ_KCHI 編制機關,AK2.MZ_KCHI 編制單位,AK3.MZ_KCHI 現服機關,AK4.MZ_KCHI 現服單位,AK5.MZ_KCHI 職稱,AK6.MZ_KCHI 官職等起,AK7.MZ_KCHI 官職等迄,AK8.MZ_KCHI 薪俸職等,AK9.MZ_KCHI 職序,MZ_PRCT 獎懲內容,AK10.MZ_KCHI 獎懲類別,STATIC_NAME 獎懲統計分類,AK11.MZ_KCHI 獎懲結果,MZ_PRONAME 獎懲依據編號 ,MZ_POLK 依據類別,CASE WHEN MZ_PCODE='9' THEN '不配分' ELSE '配分' END 是否配分,MZ_PRONAME 配分款項,MZ_PROLNO2 加重條款
                                            FROM A_PRK2 
                                            LEFT JOIN A_KTYPE AK1 ON MZ_AD=AK1.MZ_KCODE AND AK1.MZ_KTYPE='04'
                                            LEFT JOIN A_KTYPE AK2 ON MZ_UNIT=AK2.MZ_KCODE AND AK2.MZ_KTYPE='25'
                                            LEFT JOIN A_KTYPE AK3 ON MZ_EXAD=AK3.MZ_KCODE AND AK3.MZ_KTYPE='04'
                                            LEFT JOIN A_KTYPE AK4 ON MZ_EXUNIT=AK4.MZ_KCODE AND AK4.MZ_KTYPE='25'
                                            LEFT JOIN A_KTYPE AK5 ON MZ_OCCC=AK5.MZ_KCODE AND AK5.MZ_KTYPE='26'
                                            LEFT JOIN A_KTYPE AK6 ON MZ_RANK=AK6.MZ_KCODE AND AK6.MZ_KTYPE='09'
                                            LEFT JOIN A_KTYPE AK7 ON MZ_RANK1=AK7.MZ_KCODE AND AK7.MZ_KTYPE='09'
                                            LEFT JOIN A_KTYPE AK8 ON MZ_SRANK=AK8.MZ_KCODE AND AK8.MZ_KTYPE='09'
                                            LEFT JOIN A_KTYPE AK9 ON MZ_TBDV=AK9.MZ_KCODE AND AK9.MZ_KTYPE='43'
                                            LEFT JOIN A_KTYPE AK10 ON MZ_PRK=AK10.MZ_KCODE AND AK10.MZ_KTYPE='22'
                                            LEFT JOIN A_STATIC ON STATIC_NO=MZ_PRK1
                                            LEFT JOIN A_PROLNO ON A_PROLNO.MZ_PROLNO=A_PRK2.MZ_PROLNO
                                            LEFT JOIN A_KTYPE AK11 ON MZ_PRRST=AK11.MZ_KCODE AND AK11.MZ_KTYPE='24'
                                            LEFT JOIN A_POLNUM ON MZ_POLNO =MZ_PCODEM
                                          WHERE 1=1 ";
                    string excelSQL = @"SELECT MZ_ID 身份證字號,MZ_NAME 姓名,MZ_PRID + '字第' + MZ_PRID1 +'號' 發文文號,dbo.SUBSTR(MZ_DATE,1,3) +'.'+ dbo.SUBSTR(MZ_DATE,4,2) + '.'+ dbo.SUBSTR(MZ_DATE,6,2) 發文日期,AK1.MZ_KCHI 編制機關,AK2.MZ_KCHI 編制單位,AK3.MZ_KCHI 現服機關,AK4.MZ_KCHI 現服單位,AK5.MZ_KCHI 職稱,AK6.MZ_KCHI 官職等起,AK7.MZ_KCHI 官職等迄,AK8.MZ_KCHI 薪俸職等,AK9.MZ_KCHI 職序,MZ_PRCT 獎懲內容,AK10.MZ_KCHI 獎懲類別,STATIC_NAME 獎懲統計分類,AK11.MZ_KCHI 獎懲結果,MZ_PRONAME 獎懲依據編號 ,MZ_POLK 依據類別,CASE WHEN MZ_PCODE='9' THEN '不配分' ELSE '配分' END 是否配分,MZ_PRONAME 配分款項,MZ_PROLNO2 加重條款
                                        FROM A_PRK2 
                                        LEFT JOIN A_KTYPE AK1 ON MZ_AD=AK1.MZ_KCODE AND AK1.MZ_KTYPE='04'
                                        LEFT JOIN A_KTYPE AK2 ON MZ_UNIT=AK2.MZ_KCODE AND AK2.MZ_KTYPE='25'
                                        LEFT JOIN A_KTYPE AK3 ON MZ_EXAD=AK3.MZ_KCODE AND AK3.MZ_KTYPE='04'
                                        LEFT JOIN A_KTYPE AK4 ON MZ_EXUNIT=AK4.MZ_KCODE AND AK4.MZ_KTYPE='25'
                                        LEFT JOIN A_KTYPE AK5 ON MZ_OCCC=AK5.MZ_KCODE AND AK5.MZ_KTYPE='26'
                                        LEFT JOIN A_KTYPE AK6 ON MZ_RANK=AK6.MZ_KCODE AND AK6.MZ_KTYPE='09'
                                        LEFT JOIN A_KTYPE AK7 ON MZ_RANK1=AK7.MZ_KCODE AND AK7.MZ_KTYPE='09'
                                        LEFT JOIN A_KTYPE AK8 ON MZ_SRANK=AK8.MZ_KCODE AND AK8.MZ_KTYPE='09'
                                        LEFT JOIN A_KTYPE AK9 ON MZ_TBDV=AK9.MZ_KCODE AND AK9.MZ_KTYPE='43'
                                        LEFT JOIN A_KTYPE AK10 ON MZ_PRK=AK10.MZ_KCODE AND AK10.MZ_KTYPE='22'
                                        LEFT JOIN A_STATIC ON STATIC_NO=MZ_PRK1
                                        LEFT JOIN A_PROLNO ON A_PROLNO.MZ_PROLNO=A_PRK2.MZ_PROLNO
                                        LEFT JOIN A_KTYPE AK11 ON MZ_PRRST=AK11.MZ_KCODE AND AK11.MZ_KTYPE='24'
                                        LEFT JOIN A_POLNUM ON MZ_POLNO =MZ_PCODEM
                                        WHERE 1=1 ";
                    //if (ViewState["PRK2_PRID"].ToString() != "")
                        if (!string.IsNullOrEmpty (PRK2_PRID))
                    {


                        //20141231
                        if (PRK2_PRID == "新北警人")
                        {
                            where += " AND (MZ_PRID='" + PRK2_PRID + "' OR  MZ_PRID='北警人')";
                        }
                        else
                        {
                            where += " AND MZ_PRID='" + PRK2_PRID + "'";
                        } 


                        //where += " AND MZ_PRID='" + PRK2_PRID + "'";


                        //where += " AND MZ_PRID='" + ViewState["PRK2_PRID"].ToString() + "'";
                    }



                    if (ViewState["PRK2_PRID1"].ToString() != "")
                    {
                        where += " AND MZ_PRID1='" + ViewState["PRK2_PRID1"].ToString() + "'";
                    }
                    if (ViewState["PRK2_ID"].ToString() != "")
                    {
                        where += " AND MZ_ID='" + ViewState["PRK2_ID"].ToString() + "'";
                    }
                    if (ViewState["PRK2_NAME"].ToString() != "")
                    {
                        where += " AND MZ_NAME='" + ViewState["PRK2_NAME"].ToString() + "'";
                    }
                    if (ViewState["PRK2_YEAR"].ToString() != "")
                    {
                        where += " AND dbo.SUBSTR(MZ_DATE,1,3)='" + ViewState["PRK2_YEAR"].ToString() + "'";
                    }
                    if (!string.IsNullOrEmpty(PRK2_PROLNO))
                    {
                        where += " AND MZ_PROLNO='" + PRK2_PROLNO + "'";
                    }

                    //if (ViewState["PRK2_PROLNO"].ToString() != "")
                    //{
                    //    where += " AND MZ_PROLNO='" + ViewState["PRK2_PROLNO"].ToString() + "'";
                    //}
                    if (!string.IsNullOrEmpty (PRK2_SDATE))
                    {
                        where += " AND MZ_DATE >= '" + PRK2_SDATE + "'";
                    }

                    //if (ViewState["PRK2_SDATE"].ToString() != "")
                    //{
                    //    where += " AND MZ_DATE >= '" + ViewState["PRK2_SDATE"].ToString() + "'";
                    //}
                    if (!string.IsNullOrEmpty(PRK2_EDATE))
                    {
                        where += " AND MZ_DATE <= '" + PRK2_EDATE + "'";
                    }




                    where += " ORDER BY MZ_PRID,MZ_PRID1,MZ_AD,MZ_UNIT,MZ_ID";
                    strSQL = strSQL + where;
                    Session["23SQL"] = detailSQL + where;
                    ViewState["excelSQL"] = excelSQL + where;

                    PRK2_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                    PRK2_MZ_NO = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_NO");
                    PRK2_MZ_PRID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID");
                    PRK2_MZ_PRID1 = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID1");
                    PRK2_MZ_PRCT = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRCT");
                    ViewState["searchsql"] = strSQL;
                    Session["PRK2_MZ_ID"] = PRK2_MZ_ID;
                    Session["PRK2_MZ_NO"] = PRK2_MZ_NO;
                    Session["PRK2_MZ_PRID"] = PRK2_MZ_PRID;
                    Session["PRK2_MZ_PRID1"] = PRK2_MZ_PRID1;
                    Session["PRK2_MZ_PRCT"] = PRK2_MZ_PRCT;

                    if (PRK2_MZ_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal2-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (PRK2_MZ_ID.Count == 1)
                    {

                        xcount.Text = "0";
                        btDetail.Visible = false;
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }

                    if (PRK2_MZ_ID.Count == 0)
                    {
                        btDetail.Visible = false;
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        btDetail.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRK2_MZ_ID.Count.ToString() + "筆";
                    }
                }

                A.controlEnable(ref this.Panel2, false);
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
                    btDelete.Visible = true;
                    break;
                case "C":
                case "D":
                case "E":
                    btDelete.Visible = false;
                    break;
            }
        }

        protected void Can_not_empty(TextBox tb1, object obj, string fieldname)
        {
            if (string.IsNullOrEmpty(tb1.Text.Trim()))
            {
                tb1.Focus();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + fieldname + "不可空白')", true);
            }
            else
            {
                if (obj is TextBox)
                {
                    (obj as TextBox).Focus();
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSerch3.aspx?TableName=PRK2&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=380,height=270,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_SWT FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'") == "Y")
            {
                bool ADPMZ_ID = false;
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                    case "B":
                        ADPMZ_ID = true;
                        break;
                }
                if (ADPMZ_ID == false)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已傳檔不可刪除');", true);
                    return;
                }
            }

            string DeleteString = "DELETE FROM A_PRK2 WHERE" +
                                            "  MZ_NO='" + Session["A_PRK2_MZ_NO"].ToString() + "'" +
                                            "  AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'" +
                                            "  AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "'" +
                                            "  AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'" +
                                            "  AND MZ_PRCT='" + TextBox_MZ_PRCT.Text.Trim() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                string strSQL = "UPDATE A_PRKB SET MZ_SWT1='N' WHERE MZ_NO='" + Session["A_PRK2_MZ_NO"].ToString() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";

                o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                PRK2_MZ_ID = Session["PRK2_MZ_ID"] as List<string>;
                PRK2_MZ_NO = Session["PRK2_MZ_NO"] as List<string>;
                PRK2_MZ_PRCT = Session["PRK2_MZ_PRCT"] as List<string>;
                PRK2_MZ_PRID = Session["PRK2_MZ_PRID"] as List<string>;
                PRK2_MZ_PRID1 = Session["PRK2_MZ_PRID1"] as List<string>;


                PRK2_MZ_ID.RemoveAt(int.Parse(xcount.Text.Trim()));
                PRK2_MZ_NO.RemoveAt(int.Parse(xcount.Text.Trim()));
                PRK2_MZ_PRCT.RemoveAt(int.Parse(xcount.Text.Trim()));
                PRK2_MZ_PRID.RemoveAt(int.Parse(xcount.Text.Trim()));
                PRK2_MZ_PRID1.RemoveAt(int.Parse(xcount.Text.Trim()));


                if (PRK2_MZ_ID.Count == 0)
                {
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal2-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (PRK2_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRK2_MZ_ID.Count.ToString() + "筆";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal2-3.aspx?XCOUNT=" + xcount.Text + "&PRID=" + TextBox_MZ_PRID.Text.Trim() + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                }

                btUpdate.Enabled = true;
                btOK.Enabled = false;
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                throw;
            }
        }

        protected void finddata(int Datacount)
        {

            PRK2_MZ_ID = Session["PRK2_MZ_ID"] as List<string>;
            PRK2_MZ_NO = Session["PRK2_MZ_NO"] as List<string>;
            PRK2_MZ_PRCT = Session["PRK2_MZ_PRCT"] as List<string>;
            PRK2_MZ_PRID = Session["PRK2_MZ_PRID"] as List<string>;
            PRK2_MZ_PRID1 = Session["PRK2_MZ_PRID1"] as List<string>;

            string strSQL = "SELECT * FROM A_PRK2 WHERE " +
                                      "  MZ_ID='" + PRK2_MZ_ID[Datacount].ToString().Trim() +
                                      "' AND MZ_NO='" + PRK2_MZ_NO[Datacount].ToString().Trim() +
                                      "' AND MZ_PRID='" + PRK2_MZ_PRID[Datacount].ToString().Trim() +
                                      "' AND MZ_PRID1='" + PRK2_MZ_PRID1[Datacount].ToString().Trim() +
                                      "' AND MZ_PRCT= @MZ_PRCT";


            SqlParameter[] ParamaterList ={
            new SqlParameter("MZ_PRCT",SqlDbType.NVarChar){Value = PRK2_MZ_PRCT[Datacount]}
            };

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            conn.Open();
            SqlCommand oracmd = new SqlCommand(strSQL, conn);

            if (ParamaterList != null && ParamaterList.Count() > 0)
            {
                foreach (SqlParameter para in ParamaterList)
                {
                    oracmd.Parameters.Add(para);
                }
            }

            SqlDataReader read = oracmd.ExecuteReader();

            if (read.Read())
            {
                Session["A_PRK2_MZ_NO"] = read["MZ_NO"].ToString().Trim();

                TextBox_MZ_ID.Text = read["MZ_ID"].ToString().Trim();
                TextBox_MZ_NAME.Text = read["MZ_NAME"].ToString().Trim();
                TextBox_MZ_AD.Text = read["MZ_AD"].ToString().Trim();
                TextBox_MZ_UNIT.Text = read["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_OCCC.Text = read["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_RANK.Text = read["MZ_RANK"].ToString().Trim();
                TextBox_MZ_RANK1.Text = read["MZ_RANK1"].ToString().Trim();
                TextBox_MZ_SRANK.Text = read["MZ_SRANK"].ToString().Trim();
                TextBox_MZ_TBDV.Text = read["MZ_TBDV"].ToString().Trim();
                TextBox_MZ_PRK.Text = read["MZ_PRK"].ToString().Trim();
                TextBox_MZ_PRRST.Text = read["MZ_PRRST"].ToString().Trim();
                TextBox_MZ_PRK1.Text = read["MZ_PRK1"].ToString().Trim();
                TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(read["MZ_DATE"].ToString().Trim());
                TextBox_MZ_PRID.Text = read["MZ_PRID"].ToString().Trim();
                TextBox_MZ_PRID1.Text = read["MZ_PRID1"].ToString().Trim();
                TextBox_MZ_CHKAD.Text = read["MZ_CHKAD"].ToString().Trim();
                TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(read["MZ_IDATE"].ToString().Trim());
                TextBox_MZ_MEMO.Text = read["MZ_MEMO"].ToString().Trim();
                TextBox_MZ_SWT.Text = read["MZ_SWT"].ToString().Trim();
                TextBox_MZ_PRPASS.Text = read["MZ_PRPASS"].ToString().Trim();
                TextBox_MZ_PROLNO.Text = read["MZ_PROLNO"].ToString().Trim();
                TextBox_MZ_REMARK.Text = read["MZ_REMARK"].ToString().Trim();
                TextBox_MZ_SWT2.Text = read["MZ_SWT2"].ToString().Trim();
                TextBox_MZ_PRCT.Text = read["MZ_PRCT"].ToString().Trim();
                TextBox_MZ_PROLNO2.Text = read["MZ_PROLNO2"].ToString().Trim();
                //TODO  代碼轉換請在SQL句兜出來
                TextBox_MZ_CHKAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_CHKAD.Text.Trim() + "'");
                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_RANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");
                TextBox_MZ_RANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");
                TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND  MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");
                TextBox_MZ_PROLNO1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME AS MZ_KCHI FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text.Trim() + "'");
                TextBox_MZ_PRK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRK.Text, "22");
                TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
                TextBox_MZ_PRRST1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE='" + TextBox_MZ_PRRST.Text.Trim().ToUpper() + "' ");
                TextBox_MZ_PRK1_1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE STATIC_NO='" + TextBox_MZ_PRK1.Text.Trim().ToUpper() + "'");
            }

            conn.Close();

            //XX2013/06/18 

            conn.Dispose();

            btUpdate.Enabled = true;
            btDelete.Enabled = true;

        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_PRK2 WHERE " +
                                                                  "  MZ_NO='" + Session["A_PRK2_MZ_NO"].ToString() +
                                                                  "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim().ToUpper() +
                                                                  "' AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() +
                                                                  "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() +
                                                                  "' AND MZ_PRCT='" + TextBox_MZ_PRCT.Text.Trim() + "' ")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料');", true);
                return;
            }

            //if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_SWT FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'") == "Y")
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已傳檔不可修改');", true);
            //    return;
            //}

           
            //20140411
            ViewState["CMDSQL"] = "UPDATE A_PRK2 SET " +
                                           " MZ_NO=@MZ_NO,MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_AD=@MZ_AD," +
                                           " MZ_UNIT=@MZ_UNIT,MZ_OCCC=@MZ_OCCC,MZ_RANK=@MZ_RANK," +
                                           " MZ_RANK1=@MZ_RANK1,MZ_SRANK=@MZ_SRANK,MZ_TBDV=@MZ_TBDV," +
                                           " MZ_PRCT=@MZ_PRCT,MZ_PRK=@MZ_PRK,MZ_PRRST=@MZ_PRRST," +
                                           " MZ_PRK1=@MZ_PRK1,MZ_DATE=@MZ_DATE,MZ_PRID=@MZ_PRID,MZ_PRID1=@MZ_PRID1," +
                                           " MZ_CHKAD=@MZ_CHKAD,MZ_IDATE=@MZ_IDATE,MZ_MEMO=@MZ_MEMO," +
                                           " MZ_PROLNO=@MZ_PROLNO," +
                                           " MZ_PCODEM=@MZ_PCODEM,MZ_SWT3=@MZ_SWT3,MZ_REMARK=@MZ_REMARK,MZ_PROLNO2=@MZ_PROLNO2,MZ_SWT=@MZ_SWT ,MZ_PRPASS=@MZ_PRPASS " +
                                  " WHERE " +
                                           " MZ_NO='" + Session["A_PRK2_MZ_NO"].ToString() + "'" +
                                           " AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'" +
                                           " AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "'" +
                                           " AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'" +
                                           " AND MZ_PRCT='" + TextBox_MZ_PRCT.Text.Trim() + "'";

            //ViewState["CMDSQL"] = "UPDATE " +
            //                               "A_PRK2" +
            //                     " SET " +
            //                              " MZ_NO=@MZ_NO,MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_AD=@MZ_AD," +
            //                              " MZ_UNIT=@MZ_UNIT,MZ_OCCC=@MZ_OCCC,MZ_RANK=@MZ_RANK," +
            //                              " MZ_RANK1=@MZ_RANK1,MZ_SRANK=@MZ_SRANK,MZ_TBDV=@MZ_TBDV," +
            //                              " MZ_PRCT=@MZ_PRCT,MZ_PRK=@MZ_PRK,MZ_PRRST=@MZ_PRRST," +
            //                              " MZ_PRK1=@MZ_PRK1,MZ_DATE=@MZ_DATE,MZ_PRID=@MZ_PRID,MZ_PRID1=@MZ_PRID1," +
            //                              " MZ_CHKAD=@MZ_CHKAD,MZ_IDATE=@MZ_IDATE,MZ_MEMO=@MZ_MEMO," +
            //                              " MZ_SWT=@MZ_SWT,MZ_PRPASS=@MZ_PRPASS,MZ_PROLNO=@MZ_PROLNO," +
            //                              " MZ_PCODEM=@MZ_PCODEM,MZ_SWT3=@MZ_SWT3,MZ_REMARK=@MZ_REMARK,MZ_SWT2=@MZ_SWT2,MZ_PROLNO2=@MZ_PROLNO2" +
            //                     " WHERE " +
            //                              " MZ_NO='" + Session["A_PRK2_MZ_NO"].ToString() + "'" +
            //                              " AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'" +
            //                              " AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "'" +
            //                              " AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'" +
            //                              " AND MZ_PRCT='" + TextBox_MZ_PRCT.Text.Trim() + "'";


            //Session["PKEY_MZ_PRID"] = TextBox_MZ_PRID.Text;
            //Session["PKEY_MZ_PRID1"] = TextBox_MZ_PRID1.Text;
            //Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;
            //Session["PKEY_MZ_PRCT"] = TextBox_MZ_PRCT.Text;


            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = true;
            btOK.Enabled = true;
            btCancel.Enabled = true;

            //2016/1/25 jack
            A.controlEnable(ref this.Panel2, true);
            if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_SWT FROM A_PRK2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'") == "Y")
            {
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已傳檔不可修改');", true);
                //return;
                A.controlEnable(ref this.Panel2, false);
                TextBox_MZ_MEMO.Enabled = true;
            }
            else
            {
                A.controlEnable(ref this.Panel2, true);
            }

            //20140411
            TextBox_MZ_SWT.Enabled = true;
            TextBox_MZ_SWT2.Enabled = false;
            TextBox_MZ_PRPASS.Enabled = true;
            //20140411
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != PRK2_MZ_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRK2_MZ_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PRK2_MZ_ID.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PRK2_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRK2_MZ_ID.Count.ToString() + "筆";
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            string ErrorString = "";
            #region 之前就註解
            //HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_PRID");
            //Cookie1 = Request.Cookies["PKEY_MZ_PRID"];
            //string old_PRID = Cookie1.Value.ToString();
            //Response.Cookies["PKEY_MZ_PRID"].Expires = DateTime.Now.AddYears(-1);

            //HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_PRID1");
            //Cookie2 = Request.Cookies["PKEY_MZ_PRID1"];
            //string old_PRID1 = Cookie2.Value.ToString();
            //Response.Cookies["PKEY_MZ_PRID1"].Expires = DateTime.Now.AddYears(-1);

            //HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_ID");
            //Cookie3 = Request.Cookies["PKEY_MZ_ID"];
            //string old_ID = Cookie3.Value.ToString();
            //Response.Cookies["PKEY_MZ_ID"].Expires = DateTime.Now.AddYears(-1);

            //HttpCookie Cookie4 = new HttpCookie("PKEY_MZ_PRCT");
            //Cookie4 = Request.Cookies["PKEY_MZ_PRID1"];
            //string old_PRCT = Cookie4.Value.ToString();
            //Response.Cookies["PKEY_MZ_PRID1"].Expires = DateTime.Now.AddYears(-1);

            //string pkey_check;

            //if (old_ID == TextBox_MZ_ID.Text && old_PRCT == TextBox_MZ_PRCT.Text && old_PRID == TextBox_MZ_PRID.Text && old_PRID1 == TextBox_MZ_PRID1.Text)
            //    pkey_check = "0";
            //else
            //    pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "' AND MZ_PRCT='" + TextBox_MZ_PRCT.Text.Trim() + "' AND MZ_NO='" + Session["A_PRK2_MZ_NO"].ToString() + "'");

            //if (pkey_check != "0")
            //{
            //    ErrorString += "發文文號與發文字號與獎懲內容與身分證號與案號違反唯一值的條件" + "\\r\\n";
            //    TextBox_MZ_ID.BackColor = Color.Orange;
            //    TextBox_MZ_PRCT.BackColor = Color.Orange;
            //    TextBox_MZ_PRID.BackColor = Color.Orange;
            //    TextBox_MZ_PRID1.BackColor = Color.Orange;
            //}
            //else
            //{
            //    TextBox_MZ_ID.BackColor = Color.White;
            //    TextBox_MZ_PRCT.BackColor = Color.White;
            //    TextBox_MZ_PRID.BackColor = Color.White;
            //    TextBox_MZ_PRID1.BackColor = Color.White;
            //}
            #endregion 之前就註解
            bool check_MZ_DATE = App_Code.DateManange.check_Date(TextBox_MZ_DATE.Text.Replace("/", "").PadLeft(7, '0'));

            bool check_MZ_IDATE = App_Code.DateManange.check_Date(TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));

            if (!check_MZ_DATE)
            {
                ErrorString += "發文日期日期有誤" + "\\r\\n";
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_DATE.BackColor = Color.White;
            }

            if (!check_MZ_IDATE)
            {
                ErrorString += "生效日期日期有誤" + "\\r\\n";
                TextBox_MZ_IDATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_IDATE.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_PRK2", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_PRK2", dlist.Text);

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

            check_PRK1(2);

            check_PRRST(2);

            if (!checkPRK1 || !checkPRRST)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("MZ_NO", SqlDbType.VarChar).Value = Session["A_PRK2_MZ_NO"].ToString().Trim();
                cmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                cmd.Parameters.Add("MZ_NAME", SqlDbType.VarChar).Value = TextBox_MZ_NAME.Text.Trim();
                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim();
                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim();
                cmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = TextBox_MZ_OCCC.Text.Trim();
                cmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = TextBox_MZ_RANK.Text.Trim();
                cmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = TextBox_MZ_RANK1.Text.Trim();
                cmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = TextBox_MZ_SRANK.Text.Trim();
                cmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = TextBox_MZ_TBDV.Text.Trim();
                cmd.Parameters.Add("MZ_PRK", SqlDbType.VarChar).Value = TextBox_MZ_PRK.Text.Trim();
                cmd.Parameters.Add("MZ_PRRST", SqlDbType.VarChar).Value = TextBox_MZ_PRRST.Text.Trim();
                cmd.Parameters.Add("MZ_PRK1", SqlDbType.VarChar).Value = TextBox_MZ_PRK1.Text.Trim();
                cmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_PRID", SqlDbType.VarChar).Value = TextBox_MZ_PRID.Text.Trim();
                cmd.Parameters.Add("MZ_PRID1", SqlDbType.VarChar).Value = TextBox_MZ_PRID1.Text.Trim();
                cmd.Parameters.Add("MZ_CHKAD", SqlDbType.VarChar).Value = TextBox_MZ_CHKAD.Text.Trim();
                cmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_MEMO", SqlDbType.VarChar).Value = TextBox_MZ_MEMO.Text.Trim();
                cmd.Parameters.Add("MZ_SWT", SqlDbType.VarChar).Value = TextBox_MZ_SWT.Text.Trim();
                cmd.Parameters.Add("MZ_PRPASS", SqlDbType.VarChar).Value = TextBox_MZ_PRPASS.Text.Trim();
                cmd.Parameters.Add("MZ_PROLNO", SqlDbType.VarChar).Value = TextBox_MZ_PROLNO.Text.Trim();
                cmd.Parameters.Add("MZ_REMARK", SqlDbType.VarChar).Value = TextBox_MZ_REMARK.Text.Trim();
                //cmd.Parameters.Add("MZ_SWT2", SqlDbType.VarChar).Value = TextBox_MZ_SWT2.Text.Trim();
                cmd.Parameters.Add("MZ_PROLNO2", SqlDbType.VarChar).Value = TextBox_MZ_PROLNO2.Text.Trim();
                cmd.Parameters.Add("MZ_PRCT", SqlDbType.VarChar).Value = TextBox_MZ_PRCT.Text.Trim();
                cmd.Parameters.Add("MZ_PCODEM", SqlDbType.VarChar).Value = Convert.DBNull;
                //2013/10/01
                //cmd.Parameters.Add("MZ_PCODE", SqlDbType.VarChar).Value = Convert.DBNull;
                
                //2013/10/01
                cmd.Parameters.Add("MZ_SWT3", SqlDbType.VarChar).Value = Convert.DBNull;

                //2013/10/01
                //cmd.Parameters.Add("MZ_POLK", SqlDbType.VarChar).Value = Convert.DBNull;
                
                //2013/10/01


                try
                {
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));


                    if (ViewState["searchsql"] != null)
                    {
                        String strSQL = ViewState["searchsql"].ToString();

                        PRK2_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                        PRK2_MZ_NO = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_NO");
                        PRK2_MZ_PRID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID");
                        PRK2_MZ_PRID1 = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID1");
                        PRK2_MZ_PRCT = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRCT");

                        Session["PRK2_MZ_ID"] = PRK2_MZ_ID;
                        Session["PRK2_MZ_NO"] = PRK2_MZ_NO;
                        Session["PRK2_MZ_PRID"] = PRK2_MZ_PRID;
                        Session["PRK2_MZ_PRID1"] = PRK2_MZ_PRID1;
                        Session["PRK2_MZ_PRCT"] = PRK2_MZ_PRCT;
                    }
                    if (int.Parse(xcount.Text.Trim()) == 0 && PRK2_MZ_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && PRK2_MZ_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == PRK2_MZ_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PRK2_MZ_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }

                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                    btOK.Enabled = false;
                    A.controlEnable(ref this.Panel2, false);
                }
                catch(Exception ex)
                {
                    //記錄拼接出來的SQL語法到LOG
                    string msg
                        = DateTime.Now.ToString() + " \r\n"
                        + ex.Message + " \r\n"
                        + cmd.CommandText;
                    TPPDDB.App_Code.Log.SaveLog("Personal2_3", "1", msg);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal2-3.aspx?XCOUNT=" + xcount.Text.Trim() + "&PRID=" + TextBox_MZ_PRID.Text.Trim() + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "')", true);
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

            finddata(int.Parse(xcount.Text.Trim()));

            PRK2_MZ_ID = Session["PRK2_MZ_ID"] as List<string>;
            if (int.Parse(xcount.Text.Trim()) == 0 && PRK2_MZ_ID.Count == 1)
            {
                btUpper.Enabled = false;
                btNEXT.Enabled = false;
            }
            else if (int.Parse(xcount.Text.Trim()) == 0 && PRK2_MZ_ID.Count > 1)
            {
                btUpper.Enabled = false;
                btNEXT.Enabled = true;
            }
            else if (int.Parse(xcount.Text.Trim()) + 1 == PRK2_MZ_ID.Count)
            {
                btUpper.Enabled = true;
                btNEXT.Enabled = false;
            }
            else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PRK2_MZ_ID.Count)
            {
                btNEXT.Enabled = true;
                btUpper.Enabled = true;
            }

            btUpdate.Enabled = true;
            btDelete.Enabled = true;
            btOK.Enabled = false;
            A.controlEnable(ref this.Panel2, false);
        }

        protected void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, TextBox tb3)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb3.ClientID + "').focus();$get('" + tb3.ClientID + "').focus();", true);
            }
        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");

            //if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'")))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入編制機關')", true);
            //}
            //Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
            //Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_AD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }


        protected void btAD_Click(object sender, EventArgs e)
        {

            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
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

        protected void btSRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SRANK, TextBox_MZ_SRANK1, "09");
        }

        protected void btPRK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PRK, TextBox_MZ_PRK_1, "22");
        }

        protected void btPRCT_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PRCT.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void TextBox_MZ_PRK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRK.Text, "22");

            Ktype_Cname_Check(CName, TextBox_MZ_PRK_1, TextBox_MZ_PRK, TextBox_MZ_SRANK);
            #region 之前就註解
            //if (TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "A")
            //{
            //    DropDownList_MZ_PRK1.Items.Clear();
            //    DropDownList_MZ_PRK1.AppendDataBoundItems = false;
            //    SqlDataSource1.SelectCommand = "SELECT RTRIM(STATIC_NO)+RTRIM(STATIC_NAME),RTRIM(STATIC_NO) FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='A'";
            //    DropDownList_MZ_PRK1.DataTextField = "RTRIM(STATIC_NO)+RTRIM(STATIC_NAME)";
            //    DropDownList_MZ_PRK1.DataValueField = "RTRIM(STATIC_NO)";
            //    DropDownList_MZ_PRK1.DataBind();

            //    DropDownList_MZ_PRRST.Items.Clear();
            //    DropDownList_MZ_PRRST.AppendDataBoundItems = false;
            //    SqlDataSource2.SelectCommand = "SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='24' " +
            //                                   " AND (dbo.SUBSTR(MZ_KCODE,1,1)='4' OR dbo.SUBSTR(MZ_KCODE,1,1)='7' OR dbo.SUBSTR(MZ_KCODE,1,1)='8'" +
            //                                   " OR dbo.SUBSTR(MZ_KCODE,1,1)='9')";
            //    DropDownList_MZ_PRRST.DataTextField = "RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)";
            //    DropDownList_MZ_PRRST.DataValueField = "RTRIM(MZ_KCODE)";
            //    DropDownList_MZ_PRRST.DataBind();
            //}
            //else if (TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "B" || TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "C" || TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "D")
            //{
            //    DropDownList_MZ_PRK1.Items.Clear();
            //    DropDownList_MZ_PRK1.AppendDataBoundItems = false;
            //    SqlDataSource1.SelectCommand = "SELECT RTRIM(STATIC_NO)+RTRIM(STATIC_NAME),RTRIM(STATIC_NO) FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='B'";
            //    DropDownList_MZ_PRK1.DataTextField = "RTRIM(STATIC_NO)+RTRIM(STATIC_NAME)";
            //    DropDownList_MZ_PRK1.DataValueField = "RTRIM(STATIC_NO)";
            //    DropDownList_MZ_PRK1.DataBind();

            //    DropDownList_MZ_PRRST.Items.Clear();
            //    DropDownList_MZ_PRRST.AppendDataBoundItems = false;
            //    SqlDataSource2.SelectCommand = "SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='24' " +
            //                                   " AND (dbo.SUBSTR(MZ_KCODE,1,1)='2' OR dbo.SUBSTR(MZ_KCODE,1,1)='5' OR dbo.SUBSTR(MZ_KCODE,1,1)='6' " +
            //                                   " OR dbo.SUBSTR(MZ_KCODE,1,1)='A' OR dbo.SUBSTR(MZ_KCODE,1,1)='B')";
            //    DropDownList_MZ_PRRST.DataTextField = "RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)";
            //    DropDownList_MZ_PRRST.DataValueField = "RTRIM(MZ_KCODE)";
            //    DropDownList_MZ_PRRST.DataBind();
            //}
            #endregion 之前就註解
        }

        protected void btPROLNO_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PROLNO.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PROLNO1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PROLNO&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "') AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_TBDV);
        }

        protected void TextBox_MZ_RANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK_1, TextBox_MZ_RANK, TextBox_MZ_RANK1);
        }

        protected void TextBox_MZ_RANK1_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK1_1, TextBox_MZ_RANK1, TextBox_MZ_PRK);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_RANK);
        }

        protected void TextBox_MZ_SRANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_SRANK1, TextBox_MZ_SRANK, TextBox_MZ_PRCT);
        }

        protected void TextBox_MZ_PROLNO_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME AS MZ_KCHI FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text.Trim() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_PROLNO1, TextBox_MZ_PROLNO, TextBox_MZ_PROLNO2);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_OCCC);
        }

        protected void check_PRRST(int mode)
        {
            TextBox_MZ_PRRST.Text = TextBox_MZ_PRRST.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }
            string CName;

            if (TextBox_MZ_PRK.Text.Trim().ToUpper().Substring(0, 1) == "A")
            {
                CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE='" + TextBox_MZ_PRRST.Text.Trim().ToUpper() + "' " +
                                            " AND (dbo.SUBSTR(MZ_KCODE,1,1)='4' OR dbo.SUBSTR(MZ_KCODE,1,1)='7' OR dbo.SUBSTR(MZ_KCODE,1,1)='8'" +
                                            " OR dbo.SUBSTR(MZ_KCODE,1,1)='9')");
            }
            else
            {
                CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE='" + TextBox_MZ_PRRST.Text.Trim().ToUpper() + "' " +
                                                        " AND (dbo.SUBSTR(MZ_KCODE,1,1)='2' OR dbo.SUBSTR(MZ_KCODE,1,1)='5' OR dbo.SUBSTR(MZ_KCODE,1,1)='6' " +
                                                        " OR dbo.SUBSTR(MZ_KCODE,1,1)='A' OR dbo.SUBSTR(MZ_KCODE,1,1)='B')");
            }

            if (mode == 1)
                if (string.IsNullOrEmpty(CName))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料或此獎懲類別無此獎懲結果')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRRST.ClientID + "').focus();$get('" + TextBox_MZ_PRRST.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_MZ_PRRST1.Text = CName;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRK1.ClientID + "').focus();$get('" + TextBox_MZ_PRK1.ClientID + "').focus();", true);
                }
            else
            {
                if (string.IsNullOrEmpty(CName))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料或此獎懲類別無此獎懲結果')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRRST.ClientID + "').focus();$get('" + TextBox_MZ_PRRST.ClientID + "').focus();", true);
                    checkPRRST = true;
                }
            }
        }

        protected void TextBox_MZ_PRRST_TextChanged(object sender, EventArgs e)
        {
            check_PRRST(1);
        }

        protected void btPRRST_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }

            Session["KTYPE_CID"] = TextBox_MZ_PRRST.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PRRST1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=24&FIRST=" + TextBox_MZ_PRK.Text.Trim().Substring(0, 1) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btPRK1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }

            Session["KTYPE_CID"] = TextBox_MZ_PRK1.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PRK1_1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=STATIC&FIRST=" + TextBox_MZ_PRK.Text.Trim().Substring(0, 1) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void check_PRK1(int mode)
        {
            TextBox_MZ_PRK1.Text = TextBox_MZ_PRK1.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }

            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='" + TextBox_MZ_PRK.Text.Trim().ToUpper().Substring(0, 1) + "' AND STATIC_NO='" + TextBox_MZ_PRK1.Text.Trim().ToUpper() + "'");

            if (mode == 1)
            {
                if (string.IsNullOrEmpty(CName))
                {
                    TextBox_MZ_PRK1.Text = string.Empty;
                    TextBox_MZ_PRK1_1.Text = string.Empty;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料或此獎懲類別無此統計分類')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRK1.ClientID + "').focus();$get('" + TextBox_MZ_PRK1.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_MZ_PRK1_1.Text = CName;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PROLNO.ClientID + "').focus();$get('" + TextBox_MZ_PROLNO.ClientID + "').focus();", true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(CName))
                {
                    TextBox_MZ_PRK1.Text = string.Empty;
                    TextBox_MZ_PRK1_1.Text = string.Empty;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料或此獎懲類別無此統計分類')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRK1.ClientID + "').focus();$get('" + TextBox_MZ_PRK1.ClientID + "').focus();", true);
                    checkPRK1 = false;
                }
            }
        }

        protected void TextBox_MZ_PRK1_TextChanged(object sender, EventArgs e)
        {
            check_PRK1(1);
        }
        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_NAME, TextBox_MZ_ID, "姓名");
        }

        protected void CV_ID_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_ID.BackColor = Color.White;
            }
        }

        protected void CV_NAME_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_NAME.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_NAME.BackColor = Color.White;
            }
        }

        protected void CV_IDATE_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_IDATE.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_IDATE.BackColor = Color.White;
            }
        }

        protected void CV_PRID1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRID1.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRID1.BackColor = Color.White;
            }
        }

        protected void CV_DATE_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_DATE.BackColor = Color.White;
            }
        }

        protected void CV_PRID_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_DATE.BackColor = Color.White;
            }
        }

        protected void CV_CHKAD_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_CHKAD.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_CHKAD.BackColor = Color.White;
            }
        }

        protected void CV_AD_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_AD.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_AD.BackColor = Color.White;
            }
        }

        protected void CV_UNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_UNIT.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_UNIT.BackColor = Color.White;
            }
        }

        protected void CV_TBDV_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_TBDV.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_TBDV.BackColor = Color.White;
            }
        }

        protected void CV_OCCC_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_OCCC.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_OCCC.BackColor = Color.White;
            }
        }

        protected void CV_RANK1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_RANK1.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_RANK1.BackColor = Color.White;
            }
        }

        protected void CV_PRK_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRK.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRK.BackColor = Color.White;
            }
        }

        protected void CV_SRANK_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_SRANK.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_SRANK.BackColor = Color.White;
            }
        }

        protected void CV_PRCT_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRCT.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRCT.BackColor = Color.White;
            }
        }

        protected void CV_PRRST_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRRST.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRRST.BackColor = Color.White;
            }
        }

        protected void CV_PRK1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRK1.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRK1.BackColor = Color.White;
            }
        }

        protected void CV_PROLNO_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PROLNO.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PROLNO.BackColor = Color.White;
            }
        }

        protected void TextBox_MZ_IDATE_TextChanged(object sender, EventArgs e)
        {
            if (!App_Code.DateManange.Check_date(TextBox_MZ_IDATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
            }
        }

        protected void TextBox_MZ_NAME_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_NAME, TextBox_MZ_ID, "姓名");
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            if (!App_Code.DateManange.Check_date(TextBox_MZ_DATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
            }
        }

        protected void btSWT2_Click(object sender, EventArgs e)
        {
            Session["PRK3_PRID"] = TextBox_MZ_PRID.Text;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRK2_UPDATE_SWT2.aspx?MZ_PRID=" + HttpUtility.UrlEncode  (  TextBox_MZ_PRID1.Text) + "&MZ_PRID1=" + TextBox_MZ_PRID1.Text +
                                                                                          "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢'," +
                                                                                          "'top=190,left=200,width=550,height=400,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btDetail_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRK2_Detail.aspx?MZ_PRID=" + Server.UrlEncode(ViewState["PRK2_PRID_2"].ToString()) + "&MZ_PRID1=" + Server.UrlEncode(ViewState["PRK2_PRID1"].ToString()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','');", true);
        }

        protected void btEXCEL_Click(object sender, EventArgs e)
        {
            if (ViewState["excelSQL"] != null)
            {
                DataTable dt = new DataTable();

                dt = o_DBFactory.ABC_toTest.Create_Table(ViewState["excelSQL"].ToString(), "get");
                //20140123

                App_Code.ToExcel.Dt2Excel(dt, HttpUtility.UrlEncode("獎懲匯出內容", System.Text.Encoding.UTF8));

            }
        }
    }
}
