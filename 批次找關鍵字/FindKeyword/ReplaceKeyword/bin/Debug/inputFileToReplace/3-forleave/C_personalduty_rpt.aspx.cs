using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.App_Code;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_personalduty_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                ///群組權限
                //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_EXAD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_EXAD);
                }
                chk_TPMGroup();
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
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

                    break;
                case "B":
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    }
                    break;
                case "C":
                case "E"://ropDownList_EXUNIT移到DropDownList_EXUNIT_DataBound事件
                    //如果是中和分局進來要可以多選中和一&中和二 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_EXAD.Enabled = false;
                    }
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;


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
                tb2.Focus();
            }
            else
            {
                tb1.Text = Cname;
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

        protected void btPrint_Click(object sender, EventArgs e)
        {

            DataTable personalduty = new DataTable();

            personalduty.Columns.Add("NAME", typeof(string));
            for (int a = 1; a < 10; a++)
            {
                personalduty.Columns.Add("CODE0" + a, typeof(string));
                personalduty.Columns.Add("CNAME0" + a, typeof(string));
            }
            for (int b = 10; b < 29; b++)
            {
                personalduty.Columns.Add("CODE" + b, typeof(string));
                personalduty.Columns.Add("CNAME" + b, typeof(string));

            }
            #region
            //personalduty.Columns.Add("CODE01", typeof(string));
            //personalduty.Columns.Add("CODE02", typeof(string));
            //personalduty.Columns.Add("CODE03", typeof(string));
            //personalduty.Columns.Add("CODE04", typeof(string));
            //personalduty.Columns.Add("CODE05", typeof(string));
            //personalduty.Columns.Add("CODE06", typeof(string));
            //personalduty.Columns.Add("CODE07", typeof(string));
            //personalduty.Columns.Add("CODE08", typeof(string));
            //personalduty.Columns.Add("CODE09", typeof(string));
            //personalduty.Columns.Add("CODE10", typeof(string));
            //personalduty.Columns.Add("CODE11", typeof(string));
            //personalduty.Columns.Add("CODE12", typeof(string));
            //personalduty.Columns.Add("CODE13", typeof(string));
            //personalduty.Columns.Add("CODE14", typeof(string));
            //personalduty.Columns.Add("CODE15", typeof(string));
            //personalduty.Columns.Add("CODE16", typeof(string));
            //personalduty.Columns.Add("CODE17", typeof(string));
            //personalduty.Columns.Add("CODE18", typeof(string));
            //personalduty.Columns.Add("CODE19", typeof(string));
            //personalduty.Columns.Add("CODE20", typeof(string));
            //personalduty.Columns.Add("CODE21", typeof(string));
            //personalduty.Columns.Add("CODE22", typeof(string));
            //personalduty.Columns.Add("CODE23", typeof(string));
            //personalduty.Columns.Add("CODE24", typeof(string));
            //personalduty.Columns.Add("CODE25", typeof(string));
            //personalduty.Columns.Add("CODE26", typeof(string));
            //personalduty.Columns.Add("CODE27", typeof(string));
            //personalduty.Columns.Add("CODE28", typeof(string));
            //personalduty.Columns.Add("CNAME1", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            //personalduty.Columns.Add("CNAME2", typeof(string));
            #endregion

            string IDSQL = "SELECT MZ_ID FROM A_DLBASE WHERE MZ_STATUS2='Y'";

            //修改避免E權限無法正常查詢的問題 20190131 by sky
            if (!string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue))
            {
                IDSQL += " AND MZ_EXAD='" + DropDownList_EXAD.SelectedValue + "'";
            }
            else
            {
                if (ViewState["C_strGID"].ToStringNullSafe() == "E")
                {
                    IDSQL += " AND MZ_EXAD='" + Session["ADPMZ_EXAD"].ToStringNullSafe() + "'";
                }
            }
            if (!string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
            {
                IDSQL += " AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
            }
            else
            {
                if (ViewState["C_strGID"].ToStringNullSafe() == "E")
                {
                    IDSQL += " AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"].ToStringNullSafe() + "'";
                }
            }
            string strSQLpart = "";
            string up = "";
            TextBox_MZ_DATE1.Text = TextBox_MZ_DATE1.Text.PadLeft(7, '0');
            TextBox_MZ_DATE2.Text = TextBox_MZ_DATE2.Text.PadLeft(7, '0');

            if (!string.IsNullOrEmpty(TextBox_MZ_DATE1.Text.Trim()) && !string.IsNullOrEmpty(TextBox_MZ_DATE2.Text.Trim()))
            {
                if (DateManange.Check_date(TextBox_MZ_DATE1.Text) && DateManange.Check_date(TextBox_MZ_DATE1.Text))
                {
                    strSQLpart += " AND MZ_IDATE1>='" + TextBox_MZ_DATE1.Text.Trim() + "' AND MZ_IDATE1<='" + TextBox_MZ_DATE2.Text.Trim() + "'";
                    up = "統計日期：" + TextBox_MZ_DATE1.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE1.Text.Substring(3, 2) + "月" + TextBox_MZ_DATE1.Text.Substring(5, 2) + "日至" + TextBox_MZ_DATE2.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE2.Text.Substring(3, 2) + "月" + TextBox_MZ_DATE2.Text.Substring(5, 2) + "日";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                    return;
                }
            }

            IDSQL += " ORDER BY MZ_EXUNIT,MZ_TBDV,MZ_OCCC";


            DataTable tempID = new DataTable();



            tempID = o_DBFactory.ABC_toTest.Create_Table(IDSQL, "GETVALUE2");
            //k:第k位員警
            for (int k = 0; k < tempID.Rows.Count; k++)
            {
                //預計欄位:
                // NAME     | CODE1 | CNAME
                // 員警姓名 | 代碼  | 
                DataRow dr = personalduty.NewRow();

                dr["NAME"] = o_A_DLBASE.CNAME(tempID.Rows[k]["MZ_ID"].ToString());

                DataTable temp = new DataTable();
                DataTable tempcode = new DataTable();

                string strSQL = string.Format("SELECT MZ_CODE,MZ_TDAY,MZ_TTIME,FUNERAL_TYPE From C_DLTB01 WHERE MZ_CHK1='Y' AND MZ_ID='{0}' {1}", tempID.Rows[k]["MZ_ID"].ToString(), strSQLpart);

                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                string codename = @"
--第幾位/假別代碼/假別名稱
SELECT MZ_CODE,MZ_CNAME
FROM C_DLCODE 
--排除掉某些代碼
WHERE MZ_CODE  NOT IN ('23','30','31','32','34','33','38','39','91','92','93')  
ORDER BY MZ_CODE
";
                tempcode = o_DBFactory.ABC_toTest.Create_Table(codename, "GETVALUE");


                // j: 1~28,對應到報表橫向欄位的假別位置,EX:[事假]|[病假]....
                // 那代表後面的假別就不秀了
                for (int j = 1; j < 29; j++)
                {
                    //目前在處理報表中的第J欄假別

                    //根據報表中的第J欄假別
                    DataRow Current_Code = tempcode.Rows[j-1];
                    //目前要抓的假別代碼是
                    string Current_MZ_CODE = Current_Code["MZ_CODE"].ToString();
                    //目前要抓的假別是
                    string Current_MZ_CNAME = Current_Code["MZ_CNAME"].ToString();

                    int TDAY = 0;
                    int TTIME = 0;
                    //i:SQL抓出的第幾筆資料
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        //如果 此報表欄位置j,找到對應的資料中排序第幾位ROWNUM
                        if (temp.Rows[i]["MZ_CODE"].ToString() == Current_MZ_CODE)
                        {
                            //將該列資料,納入報表中的請假日數和時間加總
                            TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                            TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                        }

                        //以下這段廢止了,以前是寫死的...
                        #region Del Code

                        /*
                         
                        //matthew 產前假的code是29 請假系統那邊可能寫錯編號先從這邊硬改
                        switch (j)
                        {

                            case 23:  //績優員警
                                string code24 = "24";
                                if (temp.Rows[i]["MZ_CODE"].ToString() == code24.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                            case 24:  //生理假
                                string code25 = "25";
                                if (temp.Rows[i]["MZ_CODE"].ToString() == code25.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                            case 25:  //家庭照顧
                                string code26 = "26";
                                if (temp.Rows[i]["MZ_CODE"].ToString() == code26.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                            case 26:  //防疫假
                                string code27 = "27";
                                if (temp.Rows[i]["MZ_CODE"].ToString() == code27.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                            case 27:  //公補
                                string code28 = "28";
                                if (temp.Rows[i]["MZ_CODE"].ToString() == code28.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                            case 28:  //產前假
                                string code29 = "29";
                                if (temp.Rows[i]["MZ_CODE"].ToString() == code29.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                            default:
                                if (temp.Rows[i]["MZ_CODE"].ToString() == j.ToString().PadLeft(2, '0'))
                                {
                                    TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                                    TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                                }
                                break;
                        }
                        
                         */

                        #endregion
                        #region DEL CODE
                        //if (j == 28)
                        //{
                        //    string code29 = "29";
                        //    if (temp.Rows[i]["MZ_CODE"].ToString() == code29.ToString().PadLeft(2, '0'))
                        //    {
                        //        TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                        //        TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                        //    }
                        //}
                        //else
                        //{
                        //    if (temp.Rows[i]["MZ_CODE"].ToString() == j.ToString().PadLeft(2, '0'))
                        //    {
                        //        TDAY += int.Parse(temp.Rows[i]["MZ_TDAY"].ToString());

                        //        TTIME += int.Parse(temp.Rows[i]["MZ_TTIME"].ToString());
                        //    }

                        //}
                        #endregion
                    }

                    TDAY += TTIME / 8;

                    TTIME = TTIME % 8;
                    //如果累積的請假時間非0
                    if (TTIME != 0)
                    {
                        dr["CODE" + j.ToString().PadLeft(2, '0')] = TDAY.ToString() + "." + TTIME.ToString();
                    }
                    else
                    {
                        dr["CODE" + j.ToString().PadLeft(2, '0')] = TDAY.ToString();
                    }
                    //如果在第13欄假別
                    if (j == 13)
                    {
                        // CNAME13 = "職災假"
                        // 感覺沒意義?
                        dr["CNAME" + j.ToString().PadLeft(2, '0')] = "職災假";
                    }
                    else
                    {
                        //CNAME J = 對應的 假別 
                        dr["CNAME" + j.ToString().PadLeft(2, '0')] = Current_MZ_CNAME;
                    }

                }
                personalduty.Rows.Add(dr);
            }
            if (personalduty.Rows.Count > 0)
            {
                Session["TITLE"] = string.Format("{0}{1}個別假別統計表", o_A_KTYPE.RAD(DropDownList_EXAD.SelectedValue), o_A_KTYPE.RUNIT(DropDownList_EXUNIT.SelectedValue));

                Session["personaldhtydate"] = up;

                Session["rpt_dt"] = personalduty;

                string tmp_url = "C_rpt.aspx?fn=personalduty&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }

        ///// <summary>
        ///// 根據報表的位置,回傳對應的代碼,這段我們只能寫死,因為水晶報表的格子是固定的...
        ///// </summary>
        ///// <param name="j">報表中,假別欄位的位置</param>
        ///// <returns></returns>
        //string Get_Code(int j)
        //{
        //    /*
        //        位置	代碼	假別
        //        1	01	事假
        //        2	02	病假
        //        3	03	休假    
        //        4	04	婚假    
        //        5	05	喪假
        //        6	06	公假
        //        7	07	公差    
        //        8	08	娩假
        //        9	09	流產假
        //        10	10	陪產檢及陪產假
        //        11	11	加班補休
        //        12	12	延長病假
        //        13	13	勞基法職業災害假
        //        14	14	天災假
        //        15	15	補休(停用)
        //        16	16	超勤補休
        //        17	17	留職停薪
        //        18	18	曠職
        //        19	19	遲到
        //        20	20	早退
        //        21	21	特別休假
        //        22	22	值日補休
        //        23	24	績優員警
        //        24	25	生理假
        //        25	26	家庭照顧
        //        26	28	公補
        //        27	29	產前假
        //        28	35	因公病假
        //        29	40	病假(停用)             
        //     */

        //    //大部分的代碼和報表位置都是一樣的,直到 第23位開始不同
        //    if (j <= 22)
        //    {
        //        return j.ToString().PadLeft(2, '0');
        //    }
        //    //超過28的也不要了
        //    if (j >= 29)
        //    {
        //        return "";
        //    }

        //    /*
        //     *  這些是不童的
        //        23	24	績優員警
        //        24	25	生理假
        //        25	26	家庭照顧
        //        26	28	公補
        //        27	29	產前假
        //        28	35	因公病假
        //        29	40	病假(停用)        
        //     */
        //    //字典
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    dic.Add(23, "24");
        //    dic.Add(24, "25");
        //    dic.Add(25, "26");
        //    dic.Add(26, "28");
        //    dic.Add(27, "29");
        //    dic.Add(28, "35");

        //    //根據字典回傳對應代碼
        //    return dic[j];
        //}

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
            if (ViewState["C_strGID"].ToString() == "E")//權限E選擇所屬單位並鎖單位
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
        }

    }
}
