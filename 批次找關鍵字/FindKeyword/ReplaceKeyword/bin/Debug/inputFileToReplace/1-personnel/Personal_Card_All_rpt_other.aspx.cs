using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Card_All_rpt_other : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //判斷使用者是否有權限進入系統
            if (!IsPostBack)
            {
                A.check_power();
                A.fill_AD(DropDownList_AD);
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
                         
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
                    DropDownList_AD.Enabled = false;
                    break;
                case "D":                    
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        /// <summary>
        /// 刑事警察
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL_Click(object sender, EventArgs e)
        {
            DataTable Card_All = new DataTable();

            #region 資料欄位定義
            Card_All.Columns.Add("MZ_AD1", typeof(string));
            Card_All.Columns.Add("MZ_OCCC1", typeof(string));
            Card_All.Columns.Add("MZ_NAME1", typeof(string));
            Card_All.Columns.Add("MZ_BIR1", typeof(string));
            Card_All.Columns.Add("MZ_IDNO1", typeof(string));
            Card_All.Columns.Add("MZ_NO1_1", typeof(string));
            Card_All.Columns.Add("MZ_DATE1", typeof(string));
            Card_All.Columns.Add("MZ_AD2", typeof(string));
            Card_All.Columns.Add("MZ_OCCC2", typeof(string));
            Card_All.Columns.Add("MZ_NAME2", typeof(string));
            Card_All.Columns.Add("MZ_BIR2", typeof(string));
            Card_All.Columns.Add("MZ_IDNO2", typeof(string));
            Card_All.Columns.Add("MZ_NO1_2", typeof(string));
            Card_All.Columns.Add("MZ_DATE2", typeof(string));
            Card_All.Columns.Add("MZ_AD3", typeof(string));
            Card_All.Columns.Add("MZ_OCCC3", typeof(string));
            Card_All.Columns.Add("MZ_NAME3", typeof(string));
            Card_All.Columns.Add("MZ_BIR3", typeof(string));
            Card_All.Columns.Add("MZ_IDNO3", typeof(string));
            Card_All.Columns.Add("MZ_NO1_3", typeof(string));
            Card_All.Columns.Add("MZ_DATE3", typeof(string));
            Card_All.Columns.Add("MZ_AD4", typeof(string));
            Card_All.Columns.Add("MZ_OCCC4", typeof(string));
            Card_All.Columns.Add("MZ_NAME4", typeof(string));
            Card_All.Columns.Add("MZ_BIR4", typeof(string));
            Card_All.Columns.Add("MZ_IDNO4", typeof(string));
            Card_All.Columns.Add("MZ_NO1_4", typeof(string));
            Card_All.Columns.Add("MZ_DATE4", typeof(string));
            Card_All.Columns.Add("MZ_AD5", typeof(string));
            Card_All.Columns.Add("MZ_OCCC5", typeof(string));
            Card_All.Columns.Add("MZ_NAME5", typeof(string));
            Card_All.Columns.Add("MZ_BIR5", typeof(string));
            Card_All.Columns.Add("MZ_IDNO5", typeof(string));
            Card_All.Columns.Add("MZ_NO1_5", typeof(string));
            Card_All.Columns.Add("MZ_DATE5", typeof(string));
            Card_All.Columns.Add("MZ_AD6", typeof(string));
            Card_All.Columns.Add("MZ_OCCC6", typeof(string));
            Card_All.Columns.Add("MZ_NAME6", typeof(string));
            Card_All.Columns.Add("MZ_BIR6", typeof(string));
            Card_All.Columns.Add("MZ_IDNO6", typeof(string));
            Card_All.Columns.Add("MZ_NO1_6", typeof(string));
            Card_All.Columns.Add("MZ_DATE6", typeof(string));
            Card_All.Columns.Add("PAGE_COUNT", typeof(string));

            Card_All.Columns.Add("EXP_DATE1", typeof(string));
            Card_All.Columns.Add("EXP_DATE2", typeof(string));
            Card_All.Columns.Add("EXP_DATE3", typeof(string));
            Card_All.Columns.Add("EXP_DATE4", typeof(string));
            Card_All.Columns.Add("EXP_DATE5", typeof(string));
            Card_All.Columns.Add("EXP_DATE6", typeof(string));
            #endregion

            #region 取得列印資料
            string strSQL = "SELECT * FROM A_POLICE WHERE MZ_IDNO IS NOT NULL AND MZ_SWT='2' AND (MZ_MEMO1='N' OR MZ_MEMO1 IS NULL) ";

            if (DropDownList_Print_Mode.SelectedValue == "0")
            {
                if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
                {
                    strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='" + DropDownList_AD.SelectedValue + "')";
                }

                if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
                {
                    strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "')";
                }

                if (!string.IsNullOrEmpty(TextBox_MZ_OCCC.Text))
                {
                    strSQL += " AND MZ_OCCC='" + TextBox_MZ_OCCC.Text + "'";
                }
            }

            strSQL += " ORDER BY  MZ_IDNO";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");
            #endregion

            //刑事警察
            if (tempDT.Rows.Count > 0)
            {
                int k = 0; //頁數
                int out_count = tempDT.Rows.Count % 6 == 0 ? tempDT.Rows.Count / 6 : (tempDT.Rows.Count / 6) + 1;
                for (int i = 0; i < out_count; i++)
                {
                    if (i + 1 != out_count)
                    {
                        DataRow dr = Card_All.NewRow();
                        for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
                        {
                            int columns = j % 6 == 0 ? 6 : j % 6;
                            dr["MZ_AD" + columns.ToString()] = "新北市政府警察局";
                            dr["MZ_OCCC" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_OCCC"].ToString();
                            string BIR = tempDT.Rows[j - 1]["MZ_BIR"].ToString();
                            if (BIR.Length < 7)
                                BIR = BIR.PadLeft(7, '0');
                            dr["MZ_BIR" + columns.ToString()] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "  " + BIR.Substring(3, 2).PadLeft(2, '0') + "  " + BIR.Substring(5, 2).PadLeft(2, '0');

                            dr["MZ_NAME" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[j - 1]["MZ_ID"].ToString());
                            dr["MZ_IDNO" + columns.ToString()] = "新北警字第" + tempDT.Rows[j - 1]["MZ_IDNO"].ToString() + "號";
                            dr["MZ_NO1_" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NO1"].ToString();

                            string DATE = tempDT.Rows[j - 1]["MZ_DATE"].ToString();
                            dr["MZ_DATE" + columns.ToString()] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "年" + DATE.Substring(3, 2).PadLeft(2, '0') + "月" + DATE.Substring(5, 2).PadLeft(2, '0') + "日";

                            //有效期限
                            dr["EXP_DATE" + columns.ToString()] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";
                        }
                        k++;
                        dr["PAGE_COUNT"] = k.ToString();
                        Card_All.Rows.Add(dr);
                    }
                    else//最後一頁
                    {
                        DataRow dr = Card_All.NewRow();
                        for (int j = 6 * i + 1; j <= tempDT.Rows.Count; j++)
                        {
                            int columns = j % 6 == 0 ? 6 : j % 6;
                            dr["MZ_AD" + columns.ToString()] = "新北市政府警察局";
                            dr["MZ_OCCC" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_OCCC"].ToString();
                            string BIR = tempDT.Rows[j - 1]["MZ_BIR"].ToString();
                            if (BIR.Length < 7)
                                BIR = BIR.PadLeft(7, '0');
                            dr["MZ_BIR" + columns.ToString()] = BIR.Substring(0, 3) + "  " + BIR.Substring(3, 2).PadLeft(2, '0') + "  " + BIR.Substring(5, 2).PadLeft(2, '0');

                            dr["MZ_NAME" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[j - 1]["MZ_ID"].ToString());
                            dr["MZ_IDNO" + columns.ToString()] = "新北警字第" + tempDT.Rows[j - 1]["MZ_IDNO"].ToString() + "號";
                            dr["MZ_NO1_" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NO1"].ToString();

                            string DATE = tempDT.Rows[j - 1]["MZ_DATE"].ToString();
                            dr["MZ_DATE" + columns.ToString()] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "年" + DATE.Substring(3, 2).PadLeft(2, '0') + "月" + DATE.Substring(5, 2).PadLeft(2, '0') + "日";

                            //有效期限
                            dr["EXP_DATE"+ columns.ToString()] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";
                        }
                        k++;
                        dr["PAGE_COUNT"] = k.ToString();
                        Card_All.Rows.Add(dr);
                    }
                }
            }

            Session["rpt_dt"] = Card_All;
            string tmp_url = "A_rpt.aspx?fn=Card_All_Criminal_Vertical";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        /// <summary>
        /// 行政警察
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL1_Click(object sender, EventArgs e)
        {
            DataTable Card_All = new DataTable();

            #region 資料欄位定義
            Card_All.Columns.Add("MZ_AD1", typeof(string));
            Card_All.Columns.Add("MZ_OCCC1", typeof(string));
            Card_All.Columns.Add("MZ_NAME1", typeof(string));
            Card_All.Columns.Add("MZ_BIR1", typeof(string));
            Card_All.Columns.Add("MZ_IDNO1", typeof(string));
            Card_All.Columns.Add("MZ_DATE1", typeof(string));
            Card_All.Columns.Add("MZ_AD2", typeof(string));
            Card_All.Columns.Add("MZ_OCCC2", typeof(string));
            Card_All.Columns.Add("MZ_NAME2", typeof(string));
            Card_All.Columns.Add("MZ_BIR2", typeof(string));
            Card_All.Columns.Add("MZ_IDNO2", typeof(string));
            Card_All.Columns.Add("MZ_DATE2", typeof(string));
            Card_All.Columns.Add("MZ_AD3", typeof(string));
            Card_All.Columns.Add("MZ_OCCC3", typeof(string));
            Card_All.Columns.Add("MZ_NAME3", typeof(string));
            Card_All.Columns.Add("MZ_BIR3", typeof(string));
            Card_All.Columns.Add("MZ_IDNO3", typeof(string));
            Card_All.Columns.Add("MZ_DATE3", typeof(string));
            Card_All.Columns.Add("MZ_AD4", typeof(string));
            Card_All.Columns.Add("MZ_OCCC4", typeof(string));
            Card_All.Columns.Add("MZ_NAME4", typeof(string));
            Card_All.Columns.Add("MZ_BIR4", typeof(string));
            Card_All.Columns.Add("MZ_IDNO4", typeof(string));
            Card_All.Columns.Add("MZ_DATE4", typeof(string));
            Card_All.Columns.Add("MZ_AD5", typeof(string));
            Card_All.Columns.Add("MZ_OCCC5", typeof(string));
            Card_All.Columns.Add("MZ_NAME5", typeof(string));
            Card_All.Columns.Add("MZ_BIR5", typeof(string));
            Card_All.Columns.Add("MZ_IDNO5", typeof(string));
            Card_All.Columns.Add("MZ_DATE5", typeof(string));
            Card_All.Columns.Add("MZ_AD6", typeof(string));
            Card_All.Columns.Add("MZ_OCCC6", typeof(string));
            Card_All.Columns.Add("MZ_NAME6", typeof(string));
            Card_All.Columns.Add("MZ_BIR6", typeof(string));
            Card_All.Columns.Add("MZ_IDNO6", typeof(string));
            Card_All.Columns.Add("MZ_DATE6", typeof(string));
            Card_All.Columns.Add("MZ_AD7", typeof(string));
            Card_All.Columns.Add("MZ_OCCC7", typeof(string));
            Card_All.Columns.Add("MZ_NAME7", typeof(string));
            Card_All.Columns.Add("MZ_BIR7", typeof(string));
            Card_All.Columns.Add("MZ_IDNO7", typeof(string));
            Card_All.Columns.Add("MZ_DATE7", typeof(string));
            Card_All.Columns.Add("MZ_AD8", typeof(string));
            Card_All.Columns.Add("MZ_OCCC8", typeof(string));
            Card_All.Columns.Add("MZ_NAME8", typeof(string));
            Card_All.Columns.Add("MZ_BIR8", typeof(string));
            Card_All.Columns.Add("MZ_IDNO8", typeof(string));
            Card_All.Columns.Add("MZ_DATE8", typeof(string));
            Card_All.Columns.Add("PAGE_COUNT", typeof(string));

            Card_All.Columns.Add("EXP_DATE1", typeof(string));
            Card_All.Columns.Add("EXP_DATE2", typeof(string));
            Card_All.Columns.Add("EXP_DATE3", typeof(string));
            Card_All.Columns.Add("EXP_DATE4", typeof(string));
            Card_All.Columns.Add("EXP_DATE5", typeof(string));
            Card_All.Columns.Add("EXP_DATE6", typeof(string));
            Card_All.Columns.Add("EXP_DATE7", typeof(string));
            Card_All.Columns.Add("EXP_DATE8", typeof(string));
            #endregion

            #region 取得列印資料
            //MZ_SWT='1' OR MZ_SWT='3' OR MZ_SWT='4'

            string strSQL = "SELECT * FROM A_POLICE WHERE MZ_IDNO IS NOT NULL AND (MZ_SWT='1' OR MZ_SWT='3' ) AND (MZ_MEMO1='N' OR MZ_MEMO1 IS NULL)";

            if (DropDownList_Print_Mode.SelectedValue == "0")
            {
                if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
                {
                    strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='" + DropDownList_AD.SelectedValue + "')";
                }

                if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
                {
                    strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "')";
                }

                if (!string.IsNullOrEmpty(TextBox_MZ_OCCC.Text))
                {
                    strSQL += " AND MZ_OCCC='" + TextBox_MZ_OCCC.Text + "'";
                }
            }

            strSQL += " ORDER BY  MZ_IDNO";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");
            #endregion


            //行政警察
            if (tempDT.Rows.Count > 0)
            {
                int k = 0; //頁數
                int out_count = tempDT.Rows.Count % 8 == 0 ? tempDT.Rows.Count / 8 : (tempDT.Rows.Count / 8) + 1;
                for (int i = 0; i < out_count; i++)
                {
                    if (i + 1 != out_count)
                    {
                        DataRow dr = Card_All.NewRow();
                        for (int j = 8 * i + 1; j < 8 * i + 1 + 8; j++)
                        {
                            int columns = j % 8 == 0 ? 8 : j % 8;
                            dr["MZ_AD" + columns.ToString()] = "新北市政府警察局";
                            dr["MZ_OCCC" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_OCCC"].ToString();
                            string BIR = tempDT.Rows[j - 1]["MZ_BIR"].ToString();
                            if (BIR.Length < 7)
                                BIR = BIR.PadLeft(7, '0');
                            dr["MZ_BIR" + columns.ToString()] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "　 " + BIR.Substring(3, 2).PadLeft(2, '0') + "　　" + BIR.Substring(5, 2).PadLeft(2, '0');

                            dr["MZ_NAME" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[j - 1]["MZ_ID"].ToString());
                            dr["MZ_IDNO" + columns.ToString()] = "新北警　　" + tempDT.Rows[j - 1]["MZ_IDNO"].ToString();

                            string DATE = tempDT.Rows[j - 1]["MZ_DATE"].ToString();
                            dr["MZ_DATE" + columns.ToString()] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "　 " + DATE.Substring(3, 2).PadLeft(2, '0') + "　　" + DATE.Substring(5, 2).PadLeft(2, '0');

                            //有效期限
                            dr["EXP_DATE" + columns.ToString()] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";
                        }
                        k++;
                        dr["PAGE_COUNT"] = k.ToString();
                        Card_All.Rows.Add(dr);
                    }
                    else//最後一頁
                    {
                        DataRow dr = Card_All.NewRow();
                        for (int j = 8 * i + 1; j <= tempDT.Rows.Count; j++)
                        {
                            int columns = j % 8 == 0 ? 8 : j % 8;
                            dr["MZ_AD" + columns.ToString()] = "新北市政府警察局";
                            dr["MZ_OCCC" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_OCCC"].ToString();
                            string BIR = tempDT.Rows[j - 1]["MZ_BIR"].ToString();
                            if (BIR.Length < 7)
                                BIR = BIR.PadLeft(7, '0');
                            dr["MZ_BIR" + columns.ToString()] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "　 " + BIR.Substring(3, 2).PadLeft(2, '0') + "　　" + BIR.Substring(5, 2).PadLeft(2, '0');

                            dr["MZ_NAME" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[j - 1]["MZ_ID"].ToString());
                            dr["MZ_IDNO" + columns.ToString()] = "新北警　　" + tempDT.Rows[j - 1]["MZ_IDNO"].ToString();

                            string DATE = tempDT.Rows[j - 1]["MZ_DATE"].ToString();
                            dr["MZ_DATE" + columns.ToString()] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "　 " + DATE.Substring(3, 2).PadLeft(2, '0') + "　　" + DATE.Substring(5, 2).PadLeft(2, '0');

                            //有效期限
                            dr["EXP_DATE" + columns.ToString()] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";
                        }
                        k++;
                        dr["PAGE_COUNT"] = k.ToString();
                        Card_All.Rows.Add(dr);
                    }
                }
            }

            Session["rpt_dt"] = Card_All;
            string tmp_url = "A_rpt.aspx?fn=Card_All_Horizontal";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        /// <summary>
        /// 一般行政
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL2_Click(object sender, EventArgs e)
        {
            DataTable Card_All = new DataTable();

            #region 資料欄位定義
            Card_All.Columns.Add("MZ_AD1", typeof(string));
            Card_All.Columns.Add("MZ_OCCC1", typeof(string));
            Card_All.Columns.Add("MZ_NAME1", typeof(string));
            Card_All.Columns.Add("MZ_BIR1", typeof(string));
            Card_All.Columns.Add("MZ_IDNO1", typeof(string));
            Card_All.Columns.Add("MZ_DATE1", typeof(string));
            Card_All.Columns.Add("MZ_AD2", typeof(string));
            Card_All.Columns.Add("MZ_OCCC2", typeof(string));
            Card_All.Columns.Add("MZ_NAME2", typeof(string));
            Card_All.Columns.Add("MZ_BIR2", typeof(string));
            Card_All.Columns.Add("MZ_IDNO2", typeof(string));
            Card_All.Columns.Add("MZ_DATE2", typeof(string));
            Card_All.Columns.Add("MZ_AD3", typeof(string));
            Card_All.Columns.Add("MZ_OCCC3", typeof(string));
            Card_All.Columns.Add("MZ_NAME3", typeof(string));
            Card_All.Columns.Add("MZ_BIR3", typeof(string));
            Card_All.Columns.Add("MZ_IDNO3", typeof(string));
            Card_All.Columns.Add("MZ_DATE3", typeof(string));
            Card_All.Columns.Add("MZ_AD4", typeof(string));
            Card_All.Columns.Add("MZ_OCCC4", typeof(string));
            Card_All.Columns.Add("MZ_NAME4", typeof(string));
            Card_All.Columns.Add("MZ_BIR4", typeof(string));
            Card_All.Columns.Add("MZ_IDNO4", typeof(string));
            Card_All.Columns.Add("MZ_DATE4", typeof(string));
            Card_All.Columns.Add("MZ_AD5", typeof(string));
            Card_All.Columns.Add("MZ_OCCC5", typeof(string));
            Card_All.Columns.Add("MZ_NAME5", typeof(string));
            Card_All.Columns.Add("MZ_BIR5", typeof(string));
            Card_All.Columns.Add("MZ_IDNO5", typeof(string));
            Card_All.Columns.Add("MZ_DATE5", typeof(string));
            Card_All.Columns.Add("MZ_AD6", typeof(string));
            Card_All.Columns.Add("MZ_OCCC6", typeof(string));
            Card_All.Columns.Add("MZ_NAME6", typeof(string));
            Card_All.Columns.Add("MZ_BIR6", typeof(string));
            Card_All.Columns.Add("MZ_IDNO6", typeof(string));
            Card_All.Columns.Add("MZ_DATE6", typeof(string));
            Card_All.Columns.Add("MZ_AD7", typeof(string));
            Card_All.Columns.Add("MZ_OCCC7", typeof(string));
            Card_All.Columns.Add("MZ_NAME7", typeof(string));
            Card_All.Columns.Add("MZ_BIR7", typeof(string));
            Card_All.Columns.Add("MZ_IDNO7", typeof(string));
            Card_All.Columns.Add("MZ_DATE7", typeof(string));
            Card_All.Columns.Add("MZ_AD8", typeof(string));
            Card_All.Columns.Add("MZ_OCCC8", typeof(string));
            Card_All.Columns.Add("MZ_NAME8", typeof(string));
            Card_All.Columns.Add("MZ_BIR8", typeof(string));
            Card_All.Columns.Add("MZ_IDNO8", typeof(string));
            Card_All.Columns.Add("MZ_DATE8", typeof(string));
            Card_All.Columns.Add("PAGE_COUNT", typeof(string));

            Card_All.Columns.Add("EXP_DATE1", typeof(string));
            Card_All.Columns.Add("EXP_DATE2", typeof(string));
            Card_All.Columns.Add("EXP_DATE3", typeof(string));
            Card_All.Columns.Add("EXP_DATE4", typeof(string));
            Card_All.Columns.Add("EXP_DATE5", typeof(string));
            Card_All.Columns.Add("EXP_DATE6", typeof(string));
            Card_All.Columns.Add("EXP_DATE7", typeof(string));
            Card_All.Columns.Add("EXP_DATE8", typeof(string));
            #endregion

            #region 取得列印資料
            string strSQL = "SELECT * FROM A_POLICE WHERE MZ_IDNO IS NOT NULL AND ( MZ_SWT='4') AND (MZ_MEMO1='N' OR MZ_MEMO1 IS NULL)";

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='" + DropDownList_AD.SelectedValue + "')";
            }

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "')";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_OCCC.Text))
            {
                strSQL += " AND MZ_OCCC='" + TextBox_MZ_OCCC.Text + "'";
            }

            strSQL += " ORDER BY  MZ_IDNO";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");
            #endregion

            //一般行政
            if (tempDT.Rows.Count > 0)
            {
                int k = 0; //頁數
                int out_count = tempDT.Rows.Count % 8 == 0 ? tempDT.Rows.Count / 8 : (tempDT.Rows.Count / 8) + 1;
                for (int i = 0; i < out_count; i++)
                {
                    if (i + 1 != out_count)
                    {
                        DataRow dr = Card_All.NewRow();
                        for (int j = 8 * i + 1; j < 8 * i + 1 + 8; j++)
                        {
                            int columns = j % 8 == 0 ? 8 : j % 8;
                            dr["MZ_AD" + columns.ToString()] = "新北市政府警察局";
                            dr["MZ_OCCC" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_OCCC"].ToString();
                            string BIR = tempDT.Rows[j - 1]["MZ_BIR"].ToString();
                            if (BIR.Length < 7)
                                BIR = BIR.PadLeft(7, '0');
                            dr["MZ_BIR" + columns.ToString()] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "　 " + BIR.Substring(3, 2).PadLeft(2, '0') + "　　" + BIR.Substring(5, 2).PadLeft(2, '0');

                            dr["MZ_NAME" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[j - 1]["MZ_ID"].ToString());
                            dr["MZ_IDNO" + columns.ToString()] = "新北警　　" + tempDT.Rows[j - 1]["MZ_IDNO"].ToString();

                            string DATE = tempDT.Rows[j - 1]["MZ_DATE"].ToString();
                            dr["MZ_DATE" + columns.ToString()] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "　 " + DATE.Substring(3, 2).PadLeft(2, '0') + "　　" + DATE.Substring(5, 2).PadLeft(2, '0');

                            //有效期限
                            dr["EXP_DATE" + columns.ToString()] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";
                        }
                        k++;
                        dr["PAGE_COUNT"] = k.ToString();
                        Card_All.Rows.Add(dr);
                    }
                    else//最後一頁
                    {
                        DataRow dr = Card_All.NewRow();
                        for (int j = 8 * i + 1; j <= tempDT.Rows.Count; j++)
                        {
                            int columns = j % 8 == 0 ? 8 : j % 8;
                            dr["MZ_AD" + columns.ToString()] = "新北市政府警察局";
                            dr["MZ_OCCC" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_OCCC"].ToString();
                            string BIR = tempDT.Rows[j - 1]["MZ_BIR"].ToString();
                            if (BIR.Length < 7)
                                BIR = BIR.PadLeft(7, '0');
                            dr["MZ_BIR" + columns.ToString()] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "　 " + BIR.Substring(3, 2).PadLeft(2, '0') + "　　" + BIR.Substring(5, 2).PadLeft(2, '0');

                            dr["MZ_NAME" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_NAME"].ToString(); //o_A_DLBASE.CNAME(tempDT.Rows[j - 1]["MZ_ID"].ToString());
                            dr["MZ_IDNO" + columns.ToString()] = "新北警　　" + tempDT.Rows[j - 1]["MZ_IDNO"].ToString();

                            string DATE = tempDT.Rows[j - 1]["MZ_DATE"].ToString();
                            dr["MZ_DATE" + columns.ToString()] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "　 " + DATE.Substring(3, 2).PadLeft(2, '0') + "　　" + DATE.Substring(5, 2).PadLeft(2, '0');

                            //有效期限
                            dr["EXP_DATE" + columns.ToString()] = EXP_DATE.Value.Substring(0, 3) + "年" + EXP_DATE.Value.Substring(3, 2).PadLeft(2, '0') + "月" + EXP_DATE.Value.Substring(5, 2).PadLeft(2, '0') + "日";
                        }
                        k++;
                        dr["PAGE_COUNT"] = k.ToString();
                        Card_All.Rows.Add(dr);
                    }
                }
            }

            Session["rpt_dt"] = Card_All;
            string tmp_url = "A_rpt.aspx?fn=Card_All_Horizontal";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_AD_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_AD.Items.Insert(0, li);
        }



        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }
    

    }

}
