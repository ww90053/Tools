using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace TPPDDB._1_personnel
{
    public class A
    {
        public static void set_Panel_EnterToTAB(ref Panel Panel1)
        {
            foreach (Object ob in Panel1.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;

                    tbox.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;

                    ddlist.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is Button)
                {
                    Button bt = (Button)ob;

                    bt.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                    bt.TabIndex = -1;
                }
                else if (ob is CheckBox)
                {
                    CheckBox cb = (CheckBox)ob;

                    cb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is RadioButtonList)
                {
                    RadioButtonList rbl = (RadioButtonList)ob;

                    rbl.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is Panel)
                {
                    Panel pl = (Panel)ob;

                    foreach (Object pl1 in pl.Controls)
                    {
                        if (pl1 is TextBox)
                        {
                            TextBox tbox = (TextBox)pl1;

                            tbox.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (pl1 is DropDownList)
                        {
                            DropDownList ddlist = (DropDownList)pl1;

                            ddlist.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (pl1 is Button)
                        {
                            Button bt = (Button)pl1;

                            bt.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                            bt.TabIndex = -1;
                        }
                        else if (pl1 is CheckBox)
                        {
                            CheckBox cb = (CheckBox)pl1;

                            cb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (pl1 is RadioButtonList)
                        {
                            RadioButtonList rbl = (RadioButtonList)pl1;

                            rbl.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }                       
                    }
                }
                else if (ob is TBGridView)
                {
                    TBGridView tb = (TBGridView)ob;
                    tb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is GridView)
                {
                    GridView tb = (GridView)ob;
                    tb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
            }
        }     

        //指定 目標容器中 控制項的開或關
        public static void controlEnable(ref Panel Pl, Boolean sw)
        {
            foreach (Object ob in Pl.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;
                    tbox.Enabled = sw;
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;
                    ddlist.Enabled = sw;
                }
                else if (ob is RadioButton)
                {
                    RadioButton rd = (RadioButton)ob;
                    rd.Enabled = sw;
                }
                else if (ob is RadioButtonList)
                {
                    RadioButtonList rd = (RadioButtonList)ob;
                    rd.Enabled = sw;
                }
                else if (ob is ImageButton)
                {
                    ImageButton IB = (ImageButton)ob;
                    IB.Enabled = sw;
                }
                else if (ob is Button)
                {
                    Button bt = (Button)ob;
                    bt.Enabled = sw;
                }
            }
        }
  
        /// <summary>
        /// 檢查權限
        /// </summary>
        public static void check_power()
        {
            if (HttpContext.Current.Session["ADServerID"] != null)
            {
                switch (HttpContext.Current.Request.QueryString["TPM_FION"])
                {
                    case "":
                    case null:
                        TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), "0", "TPFXXX0001");
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                        break;
                    default:
                        if (TPMPermissions._boolPermissionID(int.Parse(HttpContext.Current.Session["TPM_MID"].ToString()), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "PVIEW") == false)
                        {
                            //無權限
                            TPMPermissions._boolErrorData(HttpContext.Current.Session["ADServerID"].ToString(), HttpContext.Current.Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                        }
                        break;
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }
        
        }
       
        /// <summary>
        /// 新北市機關(包含台北縣)
        /// </summary>
        /// <param name="ddl"></param>
        public static void fill_AD_POST(DropDownList ddl)
        {
            string strSQL = "";
            strSQL = "(SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') UNION ALL (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '376411%')";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind();

        }
        

        /// <summary>
        /// 新北市機關(不包含台北縣)
        /// </summary>
        /// <param name="ddl"></param>
        public static void fill_AD(DropDownList ddl)
        {
            string strSQL = "";
            strSQL = "SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind(); 
        
        }
        
        
        /// <summary>
        /// 新北市機關(包含台北縣,含警政署)
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="i">1 內政部警政署在後 2內政部警政署在前 </param>
        public static void fill_AD_POST_BOSS(DropDownList ddl,int i)
        {
            string strSQL = "";
            strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE IN (SELECT MZ_AD FROM A_CHKAD ) ";
            
            if(i==1)
            {

                strSQL += " ORDER BY dbo.SUBSTR(MZ_KCODE,1,5) DESC,dbo.SUBSTR(MZ_KCODE,6,2)";
            }
            else if (i==2)
            {
                strSQL += " ORDER BY MZ_KCODE";
            }

            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind();

        }

        /// <summary>
        /// 單位TEXT和CODE同時帶出
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="AD"></param>
        public static void fill_unit_code_text(DropDownList ddl, string AD)
        {
            string strSQL = "";
            strSQL = "SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI) MZ_KCHI ,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + AD + "')";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind();


        }


        /// <summary>
        /// 只有顯示單位中文
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="AD"></param>
        public static void fill_unit(DropDownList ddl, string AD)
        {
            string strSQL = "";
            strSQL = "SELECT RTRIM(MZ_KCHI) ,RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + AD + "')  ORDER BY MZ_KCODE";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "RTRIM(MZ_KCHI)";
            ddl.DataValueField = "RTRIM(MZ_KCODE)";
            ddl.DataBind(); 


        }

       
        /// <summary>
        /// 發文字第
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="orderby">1.新北在前  2.北縣在前</param>
        public static void fill_MZ_PRID(DropDownList ddl,int orderby)
        {
            string strSQL = "";
            strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD  ";
            
            switch (orderby )
            {
                case 1 :
                    strSQL += "ORDER BY dbo.SUBSTR(MZ_AD,1,5) desc,dbo.SUBSTR(MZ_AD,6,4)";
                        break;
                case 2 :
                    strSQL += "ORDER BY MZ_AD";
                    break;
            
            }
            
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_PRID";
            ddl.DataValueField = "MZ_AD";
            ddl.DataBind();


        }


        public static void fill_MZ_PROLNO(DropDownList ddl)
        {
            string strSQL = "";
            strSQL = "SELECT MZ_PROLNO, MZ_PRONAME FROM A_PROLNO  ";

           
                


            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_PRONAME";
            ddl.DataValueField = "MZ_PROLNO";
            ddl.DataBind();


        }


    }
}
