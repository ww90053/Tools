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

namespace TPPDDB._3_forleave
{
    public class C
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
                        else if (ob is TBGridView)
                        {
                            TBGridView tb1 = (TBGridView)ob;
                            tb1.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (ob is GridView)
                        {
                            GridView tb2 = (GridView)ob;
                            tb2.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
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
                else if (ob is AjaxControlToolkit.ComboBox)
                {
                    AjaxControlToolkit.ComboBox acb = (AjaxControlToolkit.ComboBox)ob;
                    acb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
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
               
                    //20140312 TING
                    //這一段是為了4.1 請假資料輸入
                //由於出國一定要填實際休假天數.沒有就要隱藏
                //所以整個TABLE 屬性設定.在這function 會視為一個物件
                //裡面的控制項,就不再深入修改
                else if (ob is System.Web.UI.HtmlControls.HtmlTable)
                {
                    //那修改的部分並不是很好看.請有技術人員將他修好一點
                    System.Web.UI.HtmlControls.HtmlTable TABLE = (System.Web.UI.HtmlControls.HtmlTable)ob;


                    foreach (System.Web.UI.HtmlControls.HtmlTableCell TCELL in TABLE.Rows[0].Cells)//因為只有那case ,那那個case又只有一列,所以寫死,之後要改活再說
                    {
                        foreach (Object txt in TCELL.Controls)
                        {
                            if (txt is TextBox)
                            {
                                TextBox tbox = (TextBox)txt;
                                tbox.Enabled = sw;
                            }
                        }
                    }

                }

                else if (ob is CheckBox)
                {
                    CheckBox cb = (CheckBox)ob;
                    cb.Enabled = sw;
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
            strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE ='04' ORDER BY MZ_KCODE";
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

            ddl.Items.Add(new ListItem("全部", ""));

        }

        /// <summary>
        /// 發文字第
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="orderby">1.新北在前  2.北縣在前</param>
        public static void fill_MZ_PRID(ListBox ddl, int orderby)
        {
            string strSQL = "";
            strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD  ";

            switch (orderby)
            {
                case 1:
                    strSQL += "ORDER BY dbo.SUBSTR(MZ_AD,1,5) desc,dbo.SUBSTR(MZ_AD,6,4)";
                    break;
                case 2:
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

        /// <summary>
        /// matthew中和分局合併後下拉選單要有中和一&中和二(包含台北縣)
        /// </summary>
        /// <param name="ddl"></param>
        public static void fill_DLL_ONE_TWO(DropDownList ddl)
        {
            string strSQL = "";
            //strSQL = "(SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE in ('382133400C','382133500C','382133600C')) UNION ALL (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE in( '376411823C','376411824C'))";
            strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE in ('382133400C','382133500C','382133600C')";

            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind();
        }
        /// <summary>
        /// matthew中和分局合併後下拉選單要有中和一&中和二
        /// </summary>
        /// <param name="ddl"></param>
        public static void fill_DLL_New(DropDownList ddl)
        {
            string strSQL = "";
            strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE in ('382133400C','382133500C','382133600C')";

            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl.DataSource = temp;

            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind();
        }

    }
}
