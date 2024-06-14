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



namespace TPPDDB._2_salary
{
    public class B
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
        /// 8.7保費統計表
        /// 
        /// (原本有一個B_InsuranceStatistics ,不過這個模組應該廢止了?)
        /// </summary>
        public class B_InsuranceStatistics_2
        {
            public DataTable doSearch(string strPAY_AD, string strDATE)
            {
                initRPTDT();

                //前人分批做,一份報表要跑20分鐘
                //公(勞)保費總計-依年度從1月到輸入月份
                // string strSQL = string.Format("SELECT SUM(INSURANCEPAY) FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}'", strMZ_ID, strDATE, strDATE.Substring(0, 3) + "01");
                //健保費總計-依年度從1月到輸入月份  
                // string strSQL = string.Format("SELECT SUM(HEALTHPAY) FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}'", strMZ_ID, strDATE, strDATE.Substring(0, 3) + "01");
                //退公(勞)保費-來自單一發放-入帳(依年度從1月1日到輸入月份最後一日)       
                //string strSQL = string.Format("SELECT SUM(PAY2) FROM B_SOLE WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}' AND DA_INOUT_GROUP='IN'", strMZ_ID, strDATE + "31", strDATE.Substring(0, 3) + "0101");
                //退健保費-來自單一發放-入帳(依年度從1月1日到輸入月份最後一日)
                //string strSQL = string.Format("SELECT SUM(PAY1) FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}' AND DA_INOUT_GROUP='IN'", strMZ_ID, strDATE + "31", strDATE.Substring(0, 3) + "0101");
                        

                string strRANK = String.Empty;
                string strSQL = String.Format("SELECT A.MZ_UNIT, A.MZ_NAME, A.IDCARD, A.INSURANCEPAY, A.HEALTHMAN, A.HEALTHPAY , "+
                " SUM_INSURANCEPAY, SUM_HEALTHPAY, PAY2, PAY1  FROM B_MONTHPAY_MAIN   A "+
                " LEFT JOIN (SELECT SUM(INSURANCEPAY)  SUM_INSURANCEPAY , SUM(HEALTHPAY) SUM_HEALTHPAY ,IDCARD FROM B_MONTHPAY_MAIN WHERE  AMONTH<='{1}' AND AMONTH>='{2}'   group by IDCARD )   B on A.IDCARD = B.IDCARD "+
                " LEFT JOIN (SELECT SUM(PAY2) PAY2 , SUM(PAY1)  PAY1 ,IDCARD FROM B_SOLE WHERE  DA<='{3}' AND DA>='{4}' AND DA_INOUT_GROUP='IN' group by IDCARD )   C on A.IDCARD = C.IDCARD "+
                " WHERE MZ_SRANK IS NOT NULL AND  AMONTH='{1}' AND PAY_AD='{0}' ORDER BY A.MZ_UNIT", strPAY_AD, strDATE, strDATE.Substring(0, 3) + "01", strDATE + "31", strDATE.Substring(0, 3) + "0101");
                
                
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "MONTHPAYDATA");
                foreach (DataRow drMONTHPAYDATA in dt.Rows)
                {
                                       
                    DataRow drToRpt = dt_TO_rpt.NewRow();
                    drToRpt["MZ_UNIT"] = o_A_KTYPE.RUNIT(drMONTHPAYDATA["MZ_UNIT"].ToString());
                    drToRpt["MZ_ID"] = drMONTHPAYDATA["IDCARD"].ToString();
                    drToRpt["MZ_NAME"] = drMONTHPAYDATA["MZ_NAME"].ToString();
                    drToRpt["IPAY"] = Convert.ToInt32(drMONTHPAYDATA["INSURANCEPAY"]);
                    drToRpt["ISUM"] = drMONTHPAYDATA["SUM_INSURANCEPAY"] == DBNull.Value  ? 0 : Convert.ToInt32(drMONTHPAYDATA["SUM_INSURANCEPAY"]);
                    drToRpt["HCOUNT"] = Convert.ToInt16(drMONTHPAYDATA["HEALTHMAN"]);
                    drToRpt["HPAY"] = Convert.ToInt32(drMONTHPAYDATA["HEALTHPAY"]);
                    drToRpt["HSUM"] = drMONTHPAYDATA["SUM_HEALTHPAY"] == DBNull.Value ? 0 : Convert.ToInt32(drMONTHPAYDATA["SUM_HEALTHPAY"]);
                    drToRpt["IREFUND"] = drMONTHPAYDATA["PAY2"] == DBNull.Value ? 0 : Convert.ToInt32(drMONTHPAYDATA["PAY2"]);
                    drToRpt["HREFUND"] = drMONTHPAYDATA["PAY1"] == DBNull.Value ? 0 : Convert.ToInt32(drMONTHPAYDATA["PAY1"]);
                    dt_TO_rpt.Rows.Add(drToRpt);
                }
                return dt_TO_rpt;
            }


            //送至報表之DataTable
            private static DataTable dt_TO_rpt = new DataTable();

            //初始化送至報表的DataTable欄位
            private void initRPTDT()
            {
                dt_TO_rpt.Clear();
                dt_TO_rpt.Columns.Clear();

                dt_TO_rpt.Columns.Add("MZ_UNIT", typeof(String));
                dt_TO_rpt.Columns.Add("MZ_ID", typeof(String));
                dt_TO_rpt.Columns.Add("MZ_NAME", typeof(String));
                dt_TO_rpt.Columns.Add("IPAY", typeof(int));
                dt_TO_rpt.Columns.Add("ISUM", typeof(int));
                dt_TO_rpt.Columns.Add("HCOUNT", typeof(int));
                dt_TO_rpt.Columns.Add("HPAY", typeof(int));
                dt_TO_rpt.Columns.Add("HSUM", typeof(int));
                dt_TO_rpt.Columns.Add("IREFUND", typeof(int));
                dt_TO_rpt.Columns.Add("HREFUND", typeof(int));
            }

           

        }
        //機關控制項在 SalaryPublic.cs
    }
}
