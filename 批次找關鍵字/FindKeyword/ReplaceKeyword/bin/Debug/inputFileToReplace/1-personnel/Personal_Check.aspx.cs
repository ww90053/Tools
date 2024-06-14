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

using TPPDDB.App_Code; 

namespace TPPDDB._1_personnel
{
    public partial class Personal_Check : System.Web.UI.Page
    {
        List<String> PER_CHECK_ID = new List<string>();
        List<String> PER_CHECK_IDATE = new List<string>();
        List<String> PER_CHECK_CONTENT = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                A.check_power();
            }

            ViewState["MZ_ID"] = Session["Personal_Check_ID"];
            Session.Remove("Personal_Check_ID");

            ViewState["MZ_NAME"] = Session["Personal_Check_NAME"];
            Session.Remove("Personal_Check_NAME");

            ViewState["MZ_ID1"] = Session["PersonalSearchIDwithNAME_MZ_ID2"];
            Session.Remove("PersonalSearchIDwithNAME_MZ_ID2");

            ViewState["MZ_RETRIEVE"] = Session["Personal_Check_RETRIEVE"];
            Session.Remove("Personal_Check_RETRIEVE");
            //查詢ID
            //HttpCookie Check_ID_Cookie = new HttpCookie("Personal_Check_ID");
            //Check_ID_Cookie = Request.Cookies["Personal_Check_ID"];

            //A.set_Panel_EnterToTAB(ref this.Panel_Personal_Check);
            //A.set_Panel_EnterToTAB(ref this.Panel1);

            //if (Check_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID"] = null;
            //    Response.Cookies["Personal_Check_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID"] = TPMPermissions._strDecod(Check_ID_Cookie.Value.ToString());
            //    Response.Cookies["Personal_Check_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            //查詢姓名
            //HttpCookie Check_NAME_Cookie = new HttpCookie("Personal_Check_NAME");
            //Check_NAME_Cookie = Request.Cookies["Personal_Check_NAME"];

            //if (Check_NAME_Cookie == null)
            //{
            //    ViewState["MZ_NAME"] = null;
            //    Response.Cookies["Personal_Check_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_NAME"] = TPMPermissions._strDecod(Check_NAME_Cookie.Value.ToString());
            //    Response.Cookies["Personal_Check_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}


            //HttpCookie PER_CHECK_ID_Cookie = new HttpCookie("PersonalSearchIDwithNAME_MZ_ID2");
            //PER_CHECK_ID_Cookie = Request.Cookies["PersonalSearchIDwithNAME_MZ_ID2"];

            //if (PER_CHECK_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID1"] = null;
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID2"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID1"] = TPMPermissions._strDecod(PER_CHECK_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID2"].Expires = DateTime.Now.AddYears(-1);
            //}

            if (!IsPostBack)
            {
                bool Change = false;

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && PER_CHECK_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < PER_CHECK_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == PER_CHECK_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (PER_CHECK_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PER_CHECK_ID.Count.ToString() + "筆";
                    }

                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btok.Enabled = false;
                    btCancel.Enabled = false;
                    Button2.Enabled = false;
                    btDelete.Enabled = true;
                    Change = false;
                }
                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL = "SELECT A.MZ_ID,A.MZ_IDATE,A.MZ_CONTENT FROM A_PER_CHECK A,A_DLBASE B WHERE A.MZ_ID=B.MZ_ID ";

                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL += " AND A.MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND B.MZ_NAME='" + ViewState["MZ_NAME"].ToString() + "'";
                    }

                    if (!string.IsNullOrEmpty(Session["Personal_CHeck_IDATE1"].ToString()) && !string.IsNullOrEmpty(Session["Personal_CHeck_IDATE2"].ToString()))
                    {
                        strSQL += " AND A.MZ_IDATE>='" + Session["Personal_CHeck_IDATE1"].ToString() + "' AND  A.MZ_IDATE<='" + Session["Personal_CHeck_IDATE2"].ToString() + "'";
                    }
                    else if (!string.IsNullOrEmpty(Session["Personal_CHeck_IDATE1"].ToString()))
                    {
                        strSQL += " AND A.MZ_IDATE='" + Session["Personal_CHeck_IDATE1"].ToString() + "'";
                    }

                    if (!string.IsNullOrEmpty(Session["Personal_Check_UNAD"].ToString()))
                    {
                        strSQL += " AND A.MZ_UNAD='" + Session["Personal_Check_UNAD"].ToString() + "'";
                    }

                    if (!string.IsNullOrEmpty(Session["Personal_MZ_CONTENT"].ToString()))
                    {
                        strSQL += " AND A.MZ_CONTENT='" + Session["Personal_MZ_CONTENT"].ToString() + "'";
                    }

                    if (!string.IsNullOrEmpty(ViewState["MZ_RETRIEVE"].ToString()))
                    {
                        strSQL += " AND B.MZ_RETRIEVE LIKE '" + ViewState["MZ_RETRIEVE"].ToString() + "%'";
                    }

                    strSQL += " ORDER BY A.MZ_IDATE";

                    PER_CHECK_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                    PER_CHECK_IDATE = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_IDATE");
                    PER_CHECK_CONTENT = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_CONTENT");

                    Session["PER_CHECK_ID"] = PER_CHECK_ID;
                    Session["PER_CHECK_IDATE"] = PER_CHECK_IDATE;
                    Session["PER_CHECK_CONTENT"] = PER_CHECK_CONTENT;

                    if (PER_CHECK_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (PER_CHECK_ID.Count == 1)
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
                    if (PER_CHECK_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PER_CHECK_ID.Count.ToString() + "筆";
                    }
                    Change = false;
                }

                if (ViewState["MZ_ID1"] != null)
                {
                    preLoad(ViewState["MZ_ID1"].ToString());
                    Change = true;
                }

                A.controlEnable(ref this.Panel_Personal_Check, Change);
            }
        }
        /// <summary>
        /// 預帶人事基本資料
        /// </summary>
        /// <param name="MZ_ID">身分證號</param>
        protected void preLoad(string MZ_ID)
        {
            if (Session["CHECK_MODE"] != null)
            {
                if (Session["CHECK_MODE"].ToString() == "INSERT")
                {
                    btInsert.Enabled = false;
                    btok.Enabled = true;
                    btUpdate.Enabled = false;
                    btCancel.Enabled = true;
                }
                else if (Session["CHECK_MODE"].ToString() == "SEARCH")
                {
                    btInsert.Enabled = true;
                    btok.Enabled = true;
                    btUpdate.Enabled = false;
                    btCancel.Enabled = false;
                    btok.Enabled = false;
                }
            }
            else
            {
                btInsert.Enabled = true;
                btok.Enabled = true;
                btUpdate.Enabled = true;
                btCancel.Enabled = false;
            }

            Button2.Enabled = true;

            string strSQL = "SELECT * FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            TextBox_MZ_ID.Text = dt.Rows[0]["MZ_ID"].ToString();
            TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString();
            TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString();
            TextBox_MZ_BIR.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_BIR"].ToString());
            TextBox_MZ_ADATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_ADATE"].ToString());
            TextBox_MZ_BIR.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_BIR"].ToString());
            TextBox_MZ_EXAD.Text = dt.Rows[0]["MZ_EXAD"].ToString();
            TextBox_MZ_EXUNIT.Text = dt.Rows[0]["MZ_EXUNIT"].ToString();
            TextBox_MZ_FDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_FDATE"].ToString());
            TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString();
            TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString();
            DropDownList_MZ_SM.SelectedValue = dt.Rows[0]["MZ_SM"].ToString();
            TextBox_MZ_ADD2.Text = dt.Rows[0]["MZ_ADD2"].ToString();
            TextBox_MZ_RETRIEVE.Text = dt.Rows[0]["MZ_RETRIEVE"].ToString();
            TextBox_MZ_ODDMY.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_ODDMY"].ToString());
            TextBox_MZ_MEMO1.Text = dt.Rows[0]["MZ_MEMO1"].ToString();
            TextBox_MZ_MEMO2.Text = dt.Rows[0]["MZ_MEMO2"].ToString();


            //string SelectString = "SELECT * FROM A_EDUCATION WHERE MZ_ID='" + MZ_ID + "'";

            //string SelectString = "SELECT AE.*,(SELECT MZ_KCODE FROM A_KTYPE AK WHERE AK.MZ_KCHI=AE.MZ_SCHOOL AND MZ_KTYPE='ORG') SCHOOL_CODE, "+
            //    "(SELECT MZ_KCODE FROM A_KTYPE AK WHERE AK.MZ_KCHI=AE.MZ_DEPARTMENT ) DEPARTMENT_CODE FROM A_EDUCATION AE WHERE MZ_ID='" + MZ_ID + "'";

            string SelectString = "SELECT AE.*,(SELECT MZ_KCODE FROM A_KTYPE AK WHERE AK.MZ_KCHI=AE.MZ_SCHOOL AND (MZ_KTYPE='ORG' OR MZ_KTYPE='04') AND ROWNUM = 1) SCHOOL_CODE " +
                            " FROM A_EDUCATION AE WHERE MZ_ID='" + MZ_ID + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

            if (temp.Rows.Count > 0)
            {
                int x = 0;
                int Y = 0;
                string school = "";
                string department = "";
                string police_school = "";
                string police_department = "";
                string school_begindate = "";
                string school_enddate = "";
                string police_begindate = "";
                string police_enddate = "";
                string police_level = "";
                string school_level = "";
                string police_year = "";
                string School_Code = "";


                for (int i = 0; i < temp.Rows.Count; i++)
                {

                    if (temp.Rows[i]["SCHOOL_CODE"].ToString() != "" && temp.Rows[i]["SCHOOL_CODE"].ToString() != null)
                    {



                        if (string.IsNullOrEmpty(temp.Rows[i]["MZ_ENDDATE"].ToString()))
                        {


                            //if (temp.Rows[i]["MZ_SCHOOL"].ToString().Substring(9, 1) != "T")
                            if (temp.Rows[i]["SCHOOL_CODE"].ToString().Substring(temp.Rows[i]["SCHOOL_CODE"].ToString().Length - 1, 1) != "T")
                            {
                                school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                                department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                                school_begindate = temp.Rows[i]["MZ_BEGINDATE"].ToString();
                                school_enddate = temp.Rows[i]["MZ_ENDDATE"].ToString();
                                school_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                                School_Code = temp.Rows[i]["SCHOOL_CODE"].ToString();

                            }
                            //if (temp.Rows[i]["MZ_SCHOOL"].ToString().Substring(9, 1) == "T")
                            if (temp.Rows[i]["SCHOOL_CODE"].ToString().Substring(temp.Rows[i]["SCHOOL_CODE"].ToString().Length - 1, 1) == "T")
                            {
                                police_school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                                police_department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                                police_begindate = temp.Rows[i]["MZ_BEGINDATE"].ToString();
                                police_enddate = temp.Rows[i]["MZ_ENDDATE"].ToString();
                                police_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                                police_year = temp.Rows[i]["MZ_YEAR"].ToString();
                                School_Code = temp.Rows[i]["SCHOOL_CODE"].ToString();

                            }

                        }
                        else
                        {
                            //if (temp.Rows[i]["MZ_SCHOOL"].ToString().Substring(9, 1) != "T")
                            if (temp.Rows[i]["SCHOOL_CODE"].ToString().Substring(temp.Rows[i]["SCHOOL_CODE"].ToString().Length - 1, 1) != "T")
                            {
                                if (x < int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString()))
                                {
                                    x = int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString());
                                    school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                                    department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                                    school_begindate = temp.Rows[i]["MZ_BEGINDATE"].ToString();
                                    school_enddate = temp.Rows[i]["MZ_ENDDATE"].ToString();
                                    school_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                                    School_Code = temp.Rows[i]["SCHOOL_CODE"].ToString();
                                }
                            }
                            //if (temp.Rows[i]["MZ_SCHOOL"].ToString().Substring(9, 1) == "T")
                            if (temp.Rows[i]["SCHOOL_CODE"].ToString().Substring(temp.Rows[i]["SCHOOL_CODE"].ToString().Length - 1, 1) == "T")
                            {
                                if (Y < int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString()))
                                {
                                    Y = int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString());
                                    police_school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                                    police_department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                                    police_begindate = temp.Rows[i]["MZ_BEGINDATE"].ToString();
                                    police_enddate = temp.Rows[i]["MZ_ENDDATE"].ToString();
                                    police_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                                    police_year = temp.Rows[i]["MZ_YEAR"].ToString();
                                    School_Code = temp.Rows[i]["SCHOOL_CODE"].ToString();
                                }
                            }
                        }

                    }

                    else                    
                    {
                        school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                        department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                        school_begindate = temp.Rows[i]["MZ_BEGINDATE"].ToString();
                        school_enddate = temp.Rows[i]["MZ_ENDDATE"].ToString();
                        school_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                        School_Code = "";
                    }
                }

                TextBox_SCHOOL.Text = school;

                TextBox_SCHOOL_DEPARTMENT.Text = department;

                TextBox_POLICE_SCHOOL.Text = police_school;

                TextBox_POLICE_DEPARTMENT.Text = police_department;

                TextBox_MZ_YEAR.Text = police_year;

                lb_School_Code.Text = School_Code;////

                //if (!string.IsNullOrEmpty(school_begindate))
                //{
                //    TextBox_MZ_BEGINDATE.Text = o_CommonService.Personal_ReturnDateString(school_begindate);
                //}
                //else
                //{
                //    TextBox_MZ_BEGINDATE.Text = string.Empty;
                //}

                //if (!string.IsNullOrEmpty(school_enddate))
                //{
                //    TextBox_MZ_ENDDATE.Text = o_CommonService.Personal_ReturnDateString(school_enddate);
                //}
                //else
                //{
                //    TextBox_MZ_ENDDATE.Text = string.Empty;
                //}

                //if (!string.IsNullOrEmpty(police_begindate))
                //{
                //    TextBox_MZ_BEGINDATE.Text = o_CommonService.Personal_ReturnDateString(police_begindate);
                //}
                //else
                //{
                //    TextBox_MZ_BEGINDATE.Text = string.Empty;
                //}

                //if (!string.IsNullOrEmpty(police_enddate))
                //{
                //    TextBox_MZ_ENDDATE.Text = o_CommonService.Personal_ReturnDateString(police_enddate);
                //}
                //else
                //{
                //    TextBox_MZ_ENDDATE.Text = string.Empty;
                //}

                if (!string.IsNullOrEmpty(department))
                {
                    //btSCHOOL_DEPARTMENT.Enabled = true;
                }
                else
                {
                    btSCHOOL_DEPARTMENT.Enabled = false;
                }

                if (!string.IsNullOrEmpty(police_department))
                {
                    //btPOLICE_DEPARTMENT.Enabled = true;
                }
                else
                {
                    btPOLICE_DEPARTMENT.Enabled = false;
                }

                TextBox_SCHOOL1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                           " AND (dbo.SUBSTR(MZ_KCODE,10,1)='Q' OR dbo.SUBSTR(MZ_KCODE,10,1)='R' OR dbo.SUBSTR(MZ_KCODE,10,1)='U' OR dbo.SUBSTR(MZ_KCODE,10,1)='X' OR dbo.SUBSTR(MZ_KCODE,10,1)='Y' OR dbo.SUBSTR(MZ_KCODE,10,1)='T') " +
                                                           " AND MZ_KCODE='" + TextBox_SCHOOL.Text.Trim().ToUpper() + "'"); ;

                TextBox_POLICE_SCHOOL1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                                  " AND (dbo.SUBSTR(MZ_KCODE,10,1)='T') " +
                                                                  " AND MZ_KCODE='" + TextBox_POLICE_SCHOOL.Text.Trim().ToUpper() + "'"); ;
                //if (!string.IsNullOrEmpty(school_level))
                //{
                //if (school_level.Trim().ToUpper().Substring(0, 1) == "5"
                // || school_level.Trim().ToUpper().Substring(0, 1) == "6"
                // || school_level.Trim().ToUpper().Substring(0, 1) == "7")
                //{
                //    TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP1");
                //}
                //else if (school_level.Trim().ToUpper().Substring(0, 1) == "4")
                //{
                //    TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP2");
                //}
                //else if (school_level.Trim().ToUpper().Substring(0, 1) == "3")
                //{
                //    TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP3");
                //}
                //else
                if (!string.IsNullOrEmpty(TextBox_SCHOOL.Text))
                {

                    if (School_Code != "" && School_Code != null)
                    {

                        //if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "Q" || TextBox_SCHOOL.Text.Trim().ToUpper() == "301210000T")



                        if (School_Code.Substring(School_Code.Length - 1, 1) == "Q" || School_Code.Trim().ToUpper() == "301210000T")
                        {
                            TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP1");
                        }
                        // else if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "R" || (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "T" && TextBox_SCHOOL.Text.Trim().ToUpper() != "301210000T"))

                        else if (School_Code.Substring(School_Code.Length - 1, 1) == "R" || (School_Code.Substring(School_Code.Length - 1, 1) == "T" && School_Code.Trim().ToUpper() != "301210000T"))
                        {
                            TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP2");
                        }
                        //else if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "U")

                        else if (School_Code.Substring(School_Code.Length - 1, 1) == "U")
                        {
                            TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP3");
                        }
                        else
                        {
                            TextBox_SCHOOL_DEPARTMENT1.Text = string.Empty;
                            btSCHOOL_DEPARTMENT.Enabled = false;
                        }
                    }

                    else
                    {
                        TextBox_SCHOOL_DEPARTMENT1.Text = string.Empty;
                        btSCHOOL_DEPARTMENT.Enabled = false;
                    }
                }

                if (!string.IsNullOrEmpty(TextBox_POLICE_SCHOOL.Text))
                {
                    if (TextBox_POLICE_SCHOOL.Text.Trim() == "301210000T")
                    {
                        TextBox_POLICE_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP1");
                    }
                    else if (TextBox_POLICE_SCHOOL.Text.Trim() != "301210000T" && !string.IsNullOrEmpty(TextBox_POLICE_SCHOOL.Text))
                    {
                        TextBox_POLICE_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP2");
                    }

                    else
                    {
                        TextBox_POLICE_DEPARTMENT1.Text = string.Empty;

                        btPOLICE_DEPARTMENT.Enabled = false;
                    }
                }
                else
                {
                    TextBox_POLICE_DEPARTMENT1.Text = string.Empty;

                    btPOLICE_DEPARTMENT.Enabled = false;
                }


                //if (!string.IsNullOrEmpty(police_level))
                //{
                //    if (police_level.Trim().ToUpper().Substring(0, 1) == "5"
                //     || police_level.Trim().ToUpper().Substring(0, 1) == "6"
                //     || police_level.Trim().ToUpper().Substring(0, 1) == "7")
                //    {
                //        TextBox_POLICE_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP1");
                //    }
                //    else if (police_level.Trim().ToUpper().Substring(0, 1) == "4")
                //    {
                //        TextBox_POLICE_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP2");
                //    }
                //    else if (police_level.Trim().ToUpper().Substring(0, 1) == "3")
                //    {
                //        TextBox_POLICE_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP3");
                //    }
                //    else
                //    {
                //        TextBox_POLICE_DEPARTMENT1.Text = string.Empty;

                //        btPOLICE_DEPARTMENT.Enabled = false;
                //    }
                //}
            }

            TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
            TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'");
            TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_EXUNIT.Text.Trim() + "'");
            TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
            TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");

            DropDownList_MZ_CONTENT.Focus();
        }
        /// <summary>
        /// 查詢資料用
        /// </summary>
        /// <param name="DataCount">第幾筆資料</param>
        protected void finddata(int DataCount)
        {
            PER_CHECK_ID = Session["PER_CHECK_ID"] as List<string>;
            PER_CHECK_IDATE = Session["PER_CHECK_IDATE"] as List<string>;
            PER_CHECK_CONTENT = Session["PER_CHECK_CONTENT"] as List<string>;

            string strSQL = "SELECT * FROM A_PER_CHECK WHERE MZ_ID='" + PER_CHECK_ID[DataCount] +
                                                      "' AND MZ_IDATE='" + PER_CHECK_IDATE[DataCount] +
                                                      "' AND MZ_CONTENT='" + PER_CHECK_CONTENT[DataCount] + "'";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //2014/01/09
            if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00001", TPMPermissions._boolTPM(), strSQL) == "N")
            {
                TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00001", strSQL);
            }
            TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();
            TextBox_MZ_NAME.Text = temp.Rows[0]["MZ_NAME"].ToString();

            try
            {
                DropDownList_MZ_CONTENT.SelectedValue = temp.Rows[0]["MZ_CONTENT"].ToString();
            }
            catch
            {
            }

            try
            {
                TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(temp.Rows[0]["MZ_IDATE"].ToString());
            }
            catch
            {
                TextBox_MZ_IDATE.Text = string.Empty;
            }
            try
            {
                DropDownList_MZ_UNAD.DataBind();
                DropDownList_MZ_UNAD.SelectedValue = temp.Rows[0]["MZ_UNAD"].ToString();
            }
            catch
            {
                DropDownList_MZ_UNAD.SelectedValue = "";
            }

            TextBox_MZ_RESULT.Text = temp.Rows[0]["MZ_RESULT"].ToString();
            try
            {
                TextBox_MZ_ODATE.Text = o_CommonService.Personal_ReturnDateString(temp.Rows[0]["MZ_ODATE"].ToString());
            }
            catch
            {
                TextBox_MZ_ODATE.Text = string.Empty;
            }
            try
            {
                DropDownList_MZ_OUNAD.DataBind();
                DropDownList_MZ_OUNAD.SelectedValue = temp.Rows[0]["MZ_OUNAD"].ToString();
            }
            catch
            {
                DropDownList_MZ_OUNAD.SelectedValue = "";
            }

            TextBox_MZ_PRCT.Text = temp.Rows[0]["MZ_PRCT"].ToString();

            preLoad(TextBox_MZ_ID.Text.Trim());
btUpdate.Enabled = true;
           btDelete.Enabled = true;


        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            Session["CHECK_MODE"] = "SEARCH";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Check_Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            Session["CHECK_MODE"] = "INSERT";
            Session["CHECK_CMDSQL"] = "INSERT INTO A_PER_CHECK (MZ_ID,MZ_NAME,MZ_CONTENT,MZ_IDATE,MZ_UNAD,MZ_RESULT,MZ_ODATE,MZ_OUNAD,MZ_PRCT) VALUES (@MZ_ID,@MZ_NAME,@MZ_CONTENT,@MZ_IDATE,@MZ_UNAD,@MZ_RESULT,@MZ_ODATE,@MZ_OUNAD,@MZ_PRCT) ";

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btok.Enabled = true;
            Button2.Enabled = true;
            btCancel.Enabled = true;
            btDelete.Enabled = false;
            btUpper.Enabled = false;
            btNEXT.Enabled = false;

            A.controlEnable(ref this.Panel_Personal_Check, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);

            //2013/04/10
            btPOLICE_SCHOOL.Enabled = false;
            btPOLICE_DEPARTMENT.Enabled = false;
            btSCHOOL.Enabled = false;
            btSCHOOL_DEPARTMENT.Enabled = false;
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            Session["CHECK_MODE"] = "UPDATE";
            Session["CHECK_CMDSQL"] = "UPDATE A_PER_CHECK SET MZ_ID = @MZ_ID,MZ_NAME = @MZ_NAME,MZ_CONTENT = @MZ_CONTENT,MZ_IDATE = @MZ_IDATE,MZ_UNAD = @MZ_UNAD,MZ_RESULT = @MZ_RESULT,MZ_ODATE = @MZ_ODATE,MZ_OUNAD = @MZ_OUNAD,MZ_PRCT = @MZ_PRCT " +
                                      " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDATE='" + TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_CONTENT='" + DropDownList_MZ_CONTENT.SelectedValue + "'";

            Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;

            Session["PKEY_MZ_IDATE"] = TextBox_MZ_IDATE.Text;

            Session["PKEY_MZ_CONTENT"] = DropDownList_MZ_CONTENT.SelectedValue;

            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btInsert.Enabled = false;
            btok.Enabled = true;
            Button2.Enabled = false;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;

            A.controlEnable(ref this.Panel_Personal_Check, true);
            Button2.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);

            //2013/04/10
            btPOLICE_SCHOOL.Enabled = false;
            btPOLICE_DEPARTMENT.Enabled = false;
            btSCHOOL.Enabled = false;
            btSCHOOL_DEPARTMENT.Enabled = false;

        }

        protected void btok_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_ID = "NULL";

            string old_IDATE = "NULL";

            string old_CONTENT = "NULL";

            if (Session["CHECK_MODE"].ToString() == "UPDATE")
            {

                old_ID = Session["PKEY_MZ_ID"].ToString();

                old_IDATE = Session["PKEY_MZ_IDATE"].ToString();

                old_CONTENT = Session["PKEY_MZ_CONTENT"].ToString();
            }
            string pkey_check;

            if (old_ID == TextBox_MZ_ID.Text && old_IDATE == TextBox_MZ_IDATE.Text && old_CONTENT == TextBox_MZ_PRCT.Text && Session["CHECK_MODE"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PER_CHECK WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_CONTENT='" + DropDownList_MZ_CONTENT.SelectedValue + "'AND MZ_IDATE='" + TextBox_MZ_IDATE.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "身分證號與列管別與列管日期違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
                DropDownList_MZ_CONTENT.BackColor = Color.Orange;
                TextBox_MZ_IDATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.White;
                DropDownList_MZ_CONTENT.BackColor = Color.White;
                TextBox_MZ_IDATE.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_Personal_Check.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_PER_CHECK", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_PER_CHECK", dlist.Text);

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

            //    if (!string.IsNullOrEmpty(ErrorString))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
            //        return;
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            try
            {
                SqlParameter[] parameterList = {
                new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
                new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = TextBox_MZ_NAME.Text},
                new SqlParameter("MZ_CONTENT",SqlDbType.VarChar){Value = DropDownList_MZ_CONTENT.SelectedValue},
                new SqlParameter("MZ_IDATE",SqlDbType.VarChar){Value = TextBox_MZ_IDATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                new SqlParameter("MZ_UNAD",SqlDbType.VarChar){Value = DropDownList_MZ_UNAD.SelectedValue},
                new SqlParameter("MZ_RESULT",SqlDbType.VarChar){Value = TextBox_MZ_RESULT.Text},
                new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value =string.IsNullOrEmpty(TextBox_MZ_ODATE.Text)?Convert.DBNull:TextBox_MZ_ODATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                new SqlParameter("MZ_OUNAD",SqlDbType.VarChar){Value = DropDownList_MZ_OUNAD.SelectedValue},
                new SqlParameter("MZ_PRCT",SqlDbType.VarChar){Value = TextBox_MZ_PRCT.Text}
                };

                o_DBFactory.ABC_toTest.ExecuteNonQuery( Session["CHECK_CMDSQL"].ToString(), parameterList);

                Session.Remove("PKEY_MZ_ID");
                Session.Remove("PKEY_MZ_IDATE");
                Session.Remove("PKEY_MZ_CONTENT");

                if (Session["CHECK_MODE"].ToString() == "INSERT")
                {
                    //2014/01/09
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["CHECK_CMDSQL"].ToString(), parameterList)) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", Session["CHECK_CMDSQL"].ToString());
                    }


                    HttpCookie Cookie1 = new HttpCookie("PersonalSearchIDwithNAME_MZ_ID2");
                    Cookie1.Value = TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim());
                    Response.Cookies.Add(Cookie1);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                }
                else if (Session["CHECK_MODE"].ToString() == "UPDATE")
                {
                    //2014/01/09
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["CHECK_CMDSQL"].ToString(), parameterList)) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", Session["CHECK_CMDSQL"].ToString());
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);

                    PER_CHECK_ID = Session["PER_CHECK_ID"] as List<string>;

                    if (int.Parse(xcount.Text.Trim()) == 0 && PER_CHECK_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && PER_CHECK_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == PER_CHECK_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PER_CHECK_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                }

                btCancel.Enabled = false;
                btok.Enabled = false;
                btDelete.Enabled = false;
                btInsert.Enabled = true;
                btUpdate.Enabled = true;
                Session.Remove("CHECK_MODE");
                A.controlEnable(ref this.Panel_Personal_Check, false);
                Button2.Enabled = false;
            }
            catch
            {
                if (Session["CHECK_MODE"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (Session["CHECK_MODE"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal_Check.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                }
            }

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (Session["CHECK_MODE"].ToString() == "INSERT")
            {

                foreach (object dl in Panel_Personal_Check.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;

                        if (dl1.ID == "DropDownList_MZ_SM")
                        {
                            dl1.SelectedValue = "1";
                        }
                        else if (dl1.ID == "DropDownList_MZ_CONTENT")
                        {
                            dl1.SelectedValue = "1";
                        }
                        else
                        {
                            dl1.SelectedValue = "";
                        }


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
            else if (Session["CHECK_MODE"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                PER_CHECK_ID = Session["PER_CHECK_ID"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && PER_CHECK_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && PER_CHECK_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == PER_CHECK_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PER_CHECK_ID.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }

            Session.Remove("PKEY_MZ_ID");
            Session.Remove("PKEY_MZ_IDATE");
            Session.Remove("PKEY_MZ_CONTENT");
            btok.Enabled = false;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            A.controlEnable(ref this.Panel_Personal_Check, false);
            Button2.Enabled = false;
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_PER_CHECK WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() +
                                                          "' AND MZ_IDATE='" + TextBox_MZ_IDATE.Text.Trim().Trim().Replace("/", "").PadLeft(7, '0') +
                                                          "' AND MZ_CONTENT='" + DropDownList_MZ_CONTENT.SelectedValue.Trim() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                PER_CHECK_ID = Session["PER_CHECK_ID"] as List<string>;
                PER_CHECK_IDATE = Session["PER_CHECK_IDATE"] as List<string>;
                PER_CHECK_CONTENT = Session["PER_CHECK_CONTENT"] as List<string>;
                PER_CHECK_ID.RemoveAt(int.Parse(xcount.Text.Trim()));
                PER_CHECK_IDATE.RemoveAt(int.Parse(xcount.Text.Trim()));
                PER_CHECK_CONTENT.RemoveAt(int.Parse(xcount.Text.Trim()));


                if (PER_CHECK_ID.Count == 0)
                {
                    btUpdate.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2014/01/09
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", DeleteString);
                    }
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (PER_CHECK_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PER_CHECK_ID.Count.ToString() + "筆";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2014/01/09
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", DeleteString);
                    }
                }

                btCancel.Enabled = false;
                btok.Enabled = false;
                btDelete.Enabled = false;
                btInsert.Enabled = true;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != PER_CHECK_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PER_CHECK_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PER_CHECK_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PER_CHECK_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PER_CHECK_ID.Count.ToString() + "筆";
        }
          ////protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        protected void Ktype_Search(Label tb1, TextBox tb2, string ktype)
        {
            //回傳值
            //Session["KTYPE_CID"] = tb1.ClientID;
            //Session["KTYPE_CID1"] = tb2.ClientID;
            //2013/04/10
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb1.ClientID;


            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearchIDwithNAME.aspx?TableName=PER_CHECK&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=750,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        }

        protected void TextBox_SCHOOL_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                                 " AND (dbo.SUBSTR(MZ_KCODE,10,1)='Q' OR dbo.SUBSTR(MZ_KCODE,10,1)='R' OR dbo.SUBSTR(MZ_KCODE,10,1)='U' OR dbo.SUBSTR(MZ_KCODE,10,1)='X' OR dbo.SUBSTR(MZ_KCODE,10,1)='Y') " +
                                                                 " AND MZ_KCODE='" + TextBox_SCHOOL.Text.Trim().ToUpper() + "'");

           
            Ktype_Cname_Check(CName, TextBox_SCHOOL1, TextBox_SCHOOL, TextBox_SCHOOL_DEPARTMENT);
        }

        protected void TextBox_POLICE_SCHOOL_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                              " AND dbo.SUBSTR(MZ_KCODE,10,1)='T' " +
                                                              " AND MZ_KCODE='" + TextBox_POLICE_SCHOOL.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_POLICE_SCHOOL1, TextBox_POLICE_SCHOOL, TextBox_POLICE_DEPARTMENT);
        }

        protected void TextBox_POLICE_DEPARTMENT_TextChanged(object sender, EventArgs e)
        {
            string CName = "";

            if (TextBox_POLICE_SCHOOL.Text.Trim() == "301210000T")
            {
                CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP1");
            }
            else
            {
                CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP2");
            }

            Ktype_Cname_Check(CName, TextBox_POLICE_DEPARTMENT1, TextBox_POLICE_DEPARTMENT, TextBox_MZ_RETRIEVE);
        }

        protected void TextBox_SCHOOL_DEPARTMENT_TextChanged(object sender, EventArgs e)
        {
            string CName = "";
            if (lb_School_Code.Text != "" && lb_School_Code.Text != null && lb_School_Code.Text.Length ==10)
            {
                //if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "Q")
                //{
                //    CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP1");
                //}
                //else if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "R")
                //{
                //    CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP2");
                //}
                //else if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "U")
                //{
                //    CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP3");
                //}

                if (lb_School_Code.Text.Trim().ToUpper().Substring(lb_School_Code.Text.Trim().Length - 1, 1) == "Q")
                {
                    CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP1");
                }
                else if (lb_School_Code.Text.Trim().ToUpper().Substring(lb_School_Code.Text.Trim().Length - 1, 1) == "R")
                {
                    CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP2");
                }
                else if (lb_School_Code.Text.Trim().ToUpper().Substring(lb_School_Code.Text.Trim().Length - 1, 1) == "U")
                {
                    CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_SCHOOL_DEPARTMENT.Text, "DP3");
                }
            }
            Ktype_Cname_Check(CName, TextBox_SCHOOL_DEPARTMENT1, TextBox_SCHOOL_DEPARTMENT, TextBox_MZ_YEAR);
        }

        protected void btPOLICE_SCHOOL_Click(object sender, EventArgs e)
        {
            //Session["KTYPE_CID"] = TextBox_POLICE_SCHOOL.ClientID;
            //Session["KTYPE_CID1"] = TextBox_POLICE_SCHOOL1.ClientID;

            Session["KTYPE_CID"] = lb_School_Code.ClientID;
            Session["KTYPE_CID1"] = TextBox_POLICE_SCHOOL.ClientID;
            //2013/04/10


            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=SCH&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        }

        protected void btSCHOOL_Click(object sender, EventArgs e)
        {
            //Session["KTYPE_CID"] = TextBox_SCHOOL.ClientID;
            //Session["KTYPE_CID1"] = TextBox_SCHOOL1.ClientID;


            Session["KTYPE_CID"] = lb_School_Code.ClientID;
            Session["KTYPE_CID1"] = TextBox_SCHOOL.ClientID;
            //2013/04/10

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=SCH&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        }

        protected void btPOLICE_DEPARTMENT_Click(object sender, EventArgs e)
        {
            if (TextBox_POLICE_SCHOOL.Text.Trim() == "301210000T")
            {
                //Ktype_Search(TextBox_POLICE_DEPARTMENT, TextBox_POLICE_DEPARTMENT1, "DP1");
          
                Ktype_Search(lb_School_Code, TextBox_POLICE_DEPARTMENT1, "DP1");
            }
            else
            {
               // Ktype_Search(TextBox_POLICE_DEPARTMENT, TextBox_POLICE_DEPARTMENT1, "DP2");


                Ktype_Search(lb_School_Code, TextBox_POLICE_DEPARTMENT1, "DP2");
            }
        }

        protected void btDEPARTMENT_Click(object sender, EventArgs e)
        {
            if (lb_School_Code.Text != "" && lb_School_Code.Text != null && lb_School_Code.Text.Length == 10)
            {
                if (lb_School_Code.Text.Trim().ToUpper().Substring(lb_School_Code.Text.Trim().Length-1, 1) == "Q")
                {
                    //Ktype_Search(TextBox_SCHOOL_DEPARTMENT, TextBox_SCHOOL_DEPARTMENT1, "DP1");

                    Ktype_Search(lb_School_Code, TextBox_SCHOOL_DEPARTMENT1, "DP1");
                }
                else if (lb_School_Code.Text.Trim().ToUpper().Substring(lb_School_Code.Text.Trim().Length - 1, 1) == "R")
                {
                    //Ktype_Search(TextBox_SCHOOL_DEPARTMENT, TextBox_SCHOOL_DEPARTMENT1, "DP2");

                    Ktype_Search(lb_School_Code, TextBox_SCHOOL_DEPARTMENT1, "DP2");
                }
                else if (lb_School_Code.Text.Trim().ToUpper().Substring(lb_School_Code.Text.Trim().Length - 1, 1) == "U")
                {
                    //Ktype_Search(TextBox_SCHOOL_DEPARTMENT, TextBox_SCHOOL_DEPARTMENT1, "DP3");

                    Ktype_Search(lb_School_Code, TextBox_SCHOOL_DEPARTMENT1, "DP3");
                }
            }
            //if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "Q")
            //{
            //    Ktype_Search(TextBox_SCHOOL_DEPARTMENT, TextBox_SCHOOL_DEPARTMENT1, "DP1");
            //}
            //else if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "R")
            //{
            //    Ktype_Search(TextBox_SCHOOL_DEPARTMENT, TextBox_SCHOOL_DEPARTMENT1, "DP2");
            //}
            //else if (TextBox_SCHOOL.Text.Trim().ToUpper().Substring(9, 1) == "U")
            //{
            //    Ktype_Search(TextBox_SCHOOL_DEPARTMENT, TextBox_SCHOOL_DEPARTMENT1, "DP3");
            //}
        }

        protected void bt_UPDATEDLBASE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_POLICE_DEPARTMENT.Text) && string.IsNullOrEmpty(TextBox_SCHOOL_DEPARTMENT.Text)
                && string.IsNullOrEmpty(TextBox_POLICE_SCHOOL.Text) && string.IsNullOrEmpty(TextBox_SCHOOL.Text)
                && string.IsNullOrEmpty(TextBox_MZ_ADD2.Text) && string.IsNullOrEmpty(TextBox_MZ_MEMO1.Text)
                && string.IsNullOrEmpty(TextBox_MZ_ODDMY.Text) && string.IsNullOrEmpty(TextBox_MZ_RETRIEVE.Text)
                && string.IsNullOrEmpty(TextBox_MZ_YEAR.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無資料可更新');", true);
                return;
            }

            string school = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SCHOOL FROM A_EDUCATION WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_SCHOOL='" + TextBox_SCHOOL.Text.Trim() + "'");

            string police_school = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SCHOOL FROM A_EDUCATION WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_SCHOOL='" + TextBox_POLICE_SCHOOL.Text.Trim() + "'");

            string department = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DEPARTMENT FROM A_EDUCATION WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_SCHOOL='" + TextBox_SCHOOL.Text.Trim() + "' AND MZ_DEPARTMENT='" + TextBox_SCHOOL_DEPARTMENT.Text.Trim() + "'");

            string police_department = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DEPARTMENT FROM A_EDUCATION WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_SCHOOL='" + TextBox_POLICE_SCHOOL.Text.Trim() + "' AND MZ_DEPARTMENT='" + TextBox_POLICE_DEPARTMENT.Text.Trim() + "'");

            string strSQL = "";

            string strSQL1 = "";

            string strSQL2 = "UPDATE A_DLBASE SET MZ_ID= '" + TextBox_MZ_ID.Text.Trim() + "'";

            //if (!string.IsNullOrEmpty(TextBox_MZ_ADD2.Text.Trim()))
            //{
            strSQL2 += ",MZ_ADD2='" + TextBox_MZ_ADD2.Text.Trim() + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_YEAR.Text.Trim()))
            //{
            //    strSQL2 += ",MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim()) + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_ADD2.Text.Trim()))
            //{
            strSQL2 += ",MZ_SM='" + DropDownList_MZ_SM.SelectedValue.Trim() + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_RETRIEVE.Text.Trim()))
            //{
            strSQL2 += ",MZ_RETRIEVE='" + TextBox_MZ_RETRIEVE.Text.Trim() + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_MEMO1.Text.Trim()))
            //{
            strSQL2 += ",MZ_MEMO1='" + TextBox_MZ_MEMO1.Text.Trim() + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_MEMO2.Text.Trim()))
            //{
            strSQL2 += ",MZ_MEMO2='" + TextBox_MZ_MEMO2.Text.Trim() + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_ODDMY.Text.Trim()))
            //{
            string ODDMY = string.IsNullOrEmpty(TextBox_MZ_ODDMY.Text) ? "" : TextBox_MZ_ODDMY.Text.Trim().Replace("/", "").PadLeft(7, '0');

            strSQL2 += ",MZ_ODDMY='" + ODDMY + "'";
            //}

            strSQL2 += " WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";


            if (string.IsNullOrEmpty(school) && !string.IsNullOrEmpty(TextBox_SCHOOL.Text) && !string.IsNullOrEmpty(TextBox_SCHOOL_DEPARTMENT.Text))
            {
                strSQL = "INSERT INTO A_EDUCATION " +
                                 "(MZ_ID,MZ_SCHOOL,MZ_EDUCLASS,MZ_DEPARTMENT,MZ_YEAR) " +
                         " VALUES " +
                                 "('" + TextBox_MZ_ID.Text.Trim() + "','" + TextBox_SCHOOL.Text.Trim() + "','A','" + TextBox_SCHOOL_DEPARTMENT.Text.Trim() + "','" + TextBox_MZ_YEAR.Text + "')";
            }
            else if (!string.IsNullOrEmpty(school) && !string.IsNullOrEmpty(TextBox_SCHOOL_DEPARTMENT.Text.Trim()) && string.IsNullOrEmpty(department))
            {
                strSQL = "INSERT INTO A_EDUCATION " +
                                 "(MZ_ID,MZ_SCHOOL,MZ_EDUCLASS,MZ_DEPARTMENT) " +
                         " VALUES " +
                                 "('" + TextBox_MZ_ID.Text.Trim() + "','" + TextBox_SCHOOL.Text.Trim() + "','A','" + TextBox_SCHOOL_DEPARTMENT.Text.Trim() + "')";
            }
            else //if (TextBox_SCHOOL.Text == school && TextBox_SCHOOL_DEPARTMENT.Text == department && !string.IsNullOrEmpty(TextBox_SCHOOL_DEPARTMENT.Text) && !string.IsNullOrEmpty(TextBox_SCHOOL.Text))
            {
                strSQL = "NULL";
            }

            if (string.IsNullOrEmpty(police_school) && !string.IsNullOrEmpty(TextBox_POLICE_SCHOOL.Text) && !string.IsNullOrEmpty(TextBox_POLICE_DEPARTMENT.Text))
            {
                strSQL1 = "INSERT INTO A_EDUCATION " +
                                 "(MZ_ID,MZ_SCHOOL,MZ_EDUCLASS,MZ_DEPARTMENT) " +
                         " VALUES " +
                                 "('" + TextBox_MZ_ID.Text.Trim() + "','" + TextBox_POLICE_SCHOOL.Text.Trim() + "','C','" + TextBox_POLICE_DEPARTMENT.Text.Trim() + "')";
            }
            else if (!string.IsNullOrEmpty(police_school) && !string.IsNullOrEmpty(TextBox_POLICE_DEPARTMENT.Text.Trim()) && string.IsNullOrEmpty(police_department))
            {
                strSQL1 = "INSERT INTO A_EDUCATION " +
                                 "(MZ_ID,MZ_SCHOOL,MZ_EDUCLASS,MZ_DEPARTMENT) " +
                         " VALUES " +
                                 "('" + TextBox_MZ_ID.Text.Trim() + "','" + TextBox_POLICE_SCHOOL.Text.Trim() + "','C','" + TextBox_POLICE_DEPARTMENT.Text.Trim() + "')";
            }
            else if (!string.IsNullOrEmpty(police_school) && !string.IsNullOrEmpty(TextBox_POLICE_DEPARTMENT.Text.Trim()) && !string.IsNullOrEmpty(police_department))
            {
                strSQL1 = "UPDATE A_EDUCATION SET MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' WHERE MZ_ID='" + TextBox_MZ_ID.Text + "' AND MZ_SCHOOL='" + TextBox_POLICE_SCHOOL.Text + "' AND MZ_DEPARTMENT='" + TextBox_POLICE_DEPARTMENT.Text + "'";
            }
            else //if (TextBox_POLICE_SCHOOL.Text == police_school && TextBox_POLICE_DEPARTMENT.Text == police_department && !string.IsNullOrEmpty(TextBox_POLICE_DEPARTMENT.Text) && !string.IsNullOrEmpty(TextBox_POLICE_SCHOOL.Text))
            {
                strSQL1 = "NULL";
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                SqlTransaction oraTran = conn.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand(strSQL, conn);
                cmd1.Transaction = oraTran;

                SqlCommand cmd2 = new SqlCommand(strSQL1, conn);
                cmd2.Transaction = oraTran;

                SqlCommand cmd3 = new SqlCommand(strSQL2, conn);
                cmd3.Transaction = oraTran;

                try
                {
                    if (strSQL != "NULL")
                    {
                        cmd1.ExecuteNonQuery();
                    }

                    if (strSQL1 != "NULL")
                    {
                        cmd2.ExecuteNonQuery();
                    }

                    cmd3.ExecuteNonQuery();

                    oraTran.Commit();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更新成功')", true);
                }
                catch
                {
                    oraTran.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更新失敗')", true);
                }
                finally
                {
                    conn.Close();
                    //XX2013/06/18 
                    conn.Close();
                }

            }
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            preLoad(TextBox_MZ_ID.Text.Trim().ToUpper());
        }

        protected string police_school(string MZ_ID)
        {
            //string SelectString = "SELECT * FROM A_EDUCATION WHERE MZ_ID='" + MZ_ID + "'";

            string SelectString = "SELECT AE.*,(SELECT MZ_KCODE FROM A_KTYPE AK WHERE AK.MZ_KCHI=AE.MZ_SCHOOL AND (MZ_KTYPE='ORG' OR MZ_KTYPE='04') AND ROWNUM = 1) SCHOOL_CODE " +
                      " FROM A_EDUCATION AE WHERE MZ_ID='" + MZ_ID + "'";
            //
            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

            int Y = 0;
            string police_school = "";
            string police_department = "";
            string police_level = "";
            string police_year = "";
            string School_Code = "";

            if (temp.Rows.Count > 0)
            {
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    if (temp.Rows[i]["SCHOOL_CODE"].ToString() != "" && temp.Rows[i]["SCHOOL_CODE"].ToString() != null)////
                    {

                        //if (temp.Rows[i]["MZ_SCHOOL"].ToString().Substring(9, 1) == "T")
                        if (temp.Rows[i]["SCHOOL_CODE"].ToString().Substring(temp.Rows[i]["SCHOOL_CODE"].ToString().Length - 1, 1) == "T")
                        {
                            if (string.IsNullOrEmpty(temp.Rows[i]["MZ_ENDDATE"].ToString()))
                            {

                                police_school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                                police_department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                                police_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                                police_year = temp.Rows[i]["MZ_YEAR"].ToString();

                                School_Code = temp.Rows[i]["SCHOOL_CODE"].ToString();////
                            }
                            else
                            {

                                if (Y < int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString()))
                                {
                                    Y = int.Parse(temp.Rows[i]["MZ_ENDDATE"].ToString());
                                    police_school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                                    police_department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                                    police_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();
                                    police_year = temp.Rows[i]["MZ_YEAR"].ToString();

                                    School_Code = temp.Rows[i]["SCHOOL_CODE"].ToString();////
                                }

                            }
                        }

                    }

                    else ////
                    {
                        police_school = temp.Rows[i]["MZ_SCHOOL"].ToString();
                        police_department = temp.Rows[i]["MZ_DEPARTMENT"].ToString();
                        police_level = temp.Rows[i]["MZ_EDULEVEL"].ToString();

                        School_Code = "";
                    }
                }

                //string SCHOOL = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                //                                                  " AND (dbo.SUBSTR(MZ_KCODE,10,1)='T') " +
                //                                                  " AND MZ_KCODE='" + police_school + "'");

                string SCHOOL = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' " +
                                                                  " AND (dbo.SUBSTR(MZ_KCODE,10,1)='T') " +
                                                                  " AND MZ_KCODE='" + School_Code + "'");


                string DEPARTMENT = "";

                //if (police_level.Trim().ToUpper().Substring(0, 1) == "5"
                // || police_level.Trim().ToUpper().Substring(0, 1) == "6"
                // || police_level.Trim().ToUpper().Substring(0, 1) == "7")
                //{
                //    DEPARTMENT = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP1");
                //}
                //else if (police_level.Trim().ToUpper().Substring(0, 1) == "4")
                //{
                //    TextBox_SCHOOL_DEPARTMENT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP2");
                //}
                //else if (police_level.Trim().ToUpper().Substring(0, 1) == "3")
                //{
                //    DEPARTMENT = o_A_KTYPE.Find_Ktype_Cname(TextBox_POLICE_DEPARTMENT.Text, "DP3");
                //}
                //else
                //{
                //    DEPARTMENT = string.Empty;
                //}

                if (!string.IsNullOrEmpty(School_Code))
                {
                    if (School_Code.Trim() == "301210000T")
                    {
                        DEPARTMENT = o_A_KTYPE.Find_Ktype_Cname(police_department, "DP1");
                    }
                    else if (School_Code != "301210000T" && !string.IsNullOrEmpty(School_Code))
                    {
                        DEPARTMENT = o_A_KTYPE.Find_Ktype_Cname(police_department, "DP2");
                    }

                    else
                    {
                        DEPARTMENT = string.Empty;
                    }
                }
                else
                {
                    DEPARTMENT = string.Empty;
                }
               


                if(string.IsNullOrEmpty( SCHOOL)  && string.IsNullOrEmpty( DEPARTMENT) )
                {
                    return "";
                
                }

                if(string.IsNullOrEmpty(police_year))
 {
                    return SCHOOL + DEPARTMENT;
                
                }

                return SCHOOL + DEPARTMENT + "第" + police_year;
            }
            else
            {
                return "";
            }

        }

        int TPM_FION=0;

        protected void Button3_Click(object sender, EventArgs e)
        {
            Panel_Print.Visible = true;
        }

        protected void TextBox_MZ_IDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_IDATE, DropDownList_MZ_UNAD);
        }

        protected void returnSameDataType(TextBox tb, object ob1)
        {
            tb.Text = tb.Text.Trim().Replace("/", "");

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());

                    if (ob1 is TextBox)
                    {
                        TextBox tb1 = ob1 as TextBox;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                    }

                    if (ob1 is DropDownList)
                    {
                        DropDownList dp1 = ob1 as DropDownList;
                        dp1.Focus();
                    }
                }
            }
        }

        protected void TextBox_MZ_ODDMY_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ODDMY, TextBox_MZ_MEMO2);
        }

        protected void bt_EXPLAIN1_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_RESULT.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void DropDownList_MZ_OUNAD_DataBound(object sender, EventArgs e)
        {
            DropDownList_MZ_OUNAD.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void DropDownList_MZ_UNAD_DataBound(object sender, EventArgs e)
        {
            DropDownList_MZ_UNAD.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void btn_Print_ok_Click(object sender, EventArgs e)
        {
            Panel_Print.Visible = false;

            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_Check_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {
                List<String> condition = new List<string>();

                DataTable Check = new DataTable();

                Check.Columns.Clear();
                Check.Columns.Add("MZ_ADUNIT", typeof(string));
                Check.Columns.Add("MZ_OCCC", typeof(string));
                Check.Columns.Add("MZ_NAME", typeof(string));
                Check.Columns.Add("MZ_ID", typeof(string));
                Check.Columns.Add("MZ_BIR", typeof(string));
                Check.Columns.Add("MZ_FDATE", typeof(string));
                Check.Columns.Add("MZ_ADATE", typeof(string));
                Check.Columns.Add("MZ_POLICE_SCHOOL", typeof(string));
                Check.Columns.Add("MZ_SM", typeof(string));
                Check.Columns.Add("MZ_ADD2", typeof(string));
                Check.Columns.Add("MZ_CONTENT", typeof(string));
                Check.Columns.Add("MZ_IDATE", typeof(string));
                Check.Columns.Add("MZ_UNAD", typeof(string));
                Check.Columns.Add("MZ_PRCT", typeof(string));
                Check.Columns.Add("MZ_RESULT", typeof(string));
                Check.Columns.Add("MZ_ODATE", typeof(string));
                Check.Columns.Add("MZ_OUNAD", typeof(string));
                Check.Columns.Add("MZ_RETRIEVE", typeof(string));

                if (CheckBox1.Checked)
                {
                    DataRow dr = Check.NewRow();

                    dr["MZ_ADUNIT"] = o_A_DLBASE.CAD(TextBox_MZ_ID.Text.Trim()) + o_A_DLBASE.CUNIT(TextBox_MZ_ID.Text.Trim());
                    dr["MZ_OCCC"] = o_A_DLBASE.OCCC(TextBox_MZ_ID.Text.Trim());
                    dr["MZ_NAME"] = o_A_DLBASE.CNAME(TextBox_MZ_ID.Text.Trim());
                    dr["MZ_ID"] = TextBox_MZ_ID.Text.Trim();
                    string MZ_BIR = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");
                    dr["MZ_BIR"] = o_CommonService.Personal_ReturnDateString(MZ_BIR).Replace("/", ".");
                    string MZ_FDATE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FDATE FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");
                    dr["MZ_FDATE"] = o_CommonService.Personal_ReturnDateString(MZ_FDATE).Replace("/", ".");
                    string MZ_ADATE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ADATE FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");
                    dr["MZ_ADATE"] = o_CommonService.Personal_ReturnDateString(MZ_ADATE).Replace("/", ".");
                    dr["MZ_POLICE_SCHOOL"] = police_school(TextBox_MZ_ID.Text.Trim());
                    dr["MZ_SM"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SM FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'") == "1" ? "未婚" : "已婚";
                    dr["MZ_ADD2"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ADD2 FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");
                    dr["MZ_CONTENT"] = "";
                    dr["MZ_IDATE"] = "";
                    dr["MZ_UNAD"] = "";
                    dr["MZ_PRCT"] = "";
                    dr["MZ_RESULT"] = "";
                    dr["MZ_ODATE"] = "";
                    dr["MZ_OUNAD"] = "";
                    dr["MZ_PRCT"] = "";
                    dr["MZ_RETRIEVE"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RETRIEVE FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");
                    Check.Rows.Add(dr);
                }
                else
                {
                    if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
                    {
                        condition.Add("MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");
                    }

                    if (!string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim()))
                    {
                        condition.Add("MZ_ID=(SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME='" + TextBox_MZ_NAME.Text.Trim() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "') ");
                    }

                    string where = (condition.Count > 0 ? string.Join(" AND ", condition.ToArray()) : string.Empty);

                    string strSQL = string.Format("SELECT * FROM A_PER_CHECK WHERE {0}", where);

                    strSQL += "ORDER BY MZ_IDATE";

                    DataTable temp = new DataTable();

                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");


                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        DataRow dr = Check.NewRow();

                        string CONTENT = temp.Rows[i]["MZ_CONTENT"].ToString();

                        if (CONTENT == "01")
                            CONTENT = "教育輔導";
                        else if (CONTENT == "02")
                            CONTENT = "風紀評估";
                        else if (CONTENT == "03")
                            CONTENT = "違法案件";
                        else if (CONTENT == "04")
                            CONTENT = "違紀案件";
                        else
                            CONTENT = "其他不良案件";


                        dr["MZ_ADUNIT"] = o_A_DLBASE.CAD(temp.Rows[i]["MZ_ID"].ToString()) + o_A_DLBASE.CUNIT(temp.Rows[i]["MZ_ID"].ToString());
                        dr["MZ_OCCC"] = o_A_DLBASE.OCCC(temp.Rows[i]["MZ_ID"].ToString());
                        dr["MZ_NAME"] = o_A_DLBASE.CNAME(temp.Rows[i]["MZ_ID"].ToString());
                        dr["MZ_ID"] = temp.Rows[i]["MZ_ID"].ToString();
                        string MZ_BIR = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                        dr["MZ_BIR"] = o_CommonService.Personal_ReturnDateString(MZ_BIR).Replace("/", ".");
                        string MZ_FDATE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FDATE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                        dr["MZ_FDATE"] = o_CommonService.Personal_ReturnDateString(MZ_FDATE).Replace("/", ".");
                        string MZ_ADATE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ADATE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                        dr["MZ_ADATE"] = o_CommonService.Personal_ReturnDateString(MZ_ADATE).Replace("/", ".");
                        dr["MZ_POLICE_SCHOOL"] = police_school(temp.Rows[i]["MZ_ID"].ToString());
                        dr["MZ_SM"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SM FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'") == "1" ? "未婚" : "已婚";
                        dr["MZ_ADD2"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ADD2 FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                        dr["MZ_CONTENT"] = CONTENT;
                        dr["MZ_IDATE"] = o_CommonService.Personal_ReturnDateString(temp.Rows[i]["MZ_IDATE"].ToString()).Replace("/", ".");
                        dr["MZ_UNAD"] = o_CommonService.d_report_break_line(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='@92' AND MZ_KCODE='" + temp.Rows[i]["MZ_UNAD"].ToString() + "'"), 8, "&N");
                        dr["MZ_PRCT"] = temp.Rows[i]["MZ_PRCT"].ToString();
                        dr["MZ_RESULT"] = o_CommonService.d_report_break_line(temp.Rows[i]["MZ_RESULT"].ToString(), 12, "&N");
                        dr["MZ_ODATE"] = o_CommonService.Personal_ReturnDateString(temp.Rows[i]["MZ_ODATE"].ToString()).Replace("/", ".");
                        dr["MZ_OUNAD"] = o_CommonService.d_report_break_line(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='@92' AND MZ_KCODE='" + temp.Rows[i]["MZ_OUNAD"].ToString() + "'"), 8, "&N");
                        dr["MZ_PRCT"] = temp.Rows[i]["MZ_PRCT"].ToString();
                        dr["MZ_RETRIEVE"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RETRIEVE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                        Check.Rows.Add(dr);
                    }
                }

                Session["rpt_dt"] = Check;

                string tmp_url = "A_rpt.aspx?fn=check&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
        }

        protected void btn_Print_Leave_Click(object sender, EventArgs e)
        {
            Panel_Print.Visible = false;
        }
    }
}
