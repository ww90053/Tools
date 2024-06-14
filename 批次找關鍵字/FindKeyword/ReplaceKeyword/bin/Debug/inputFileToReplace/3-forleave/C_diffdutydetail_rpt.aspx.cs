using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TPPDDB.App_Code; 

namespace TPPDDB._3_forleave
{
    public partial class C_diffdutydetail_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                //by MQ ------------------------------20100331           
                C.set_Panel_EnterToTAB(ref this.Panel1);
            

                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                //C.fill_AD_POST(DropDownList_EXAD);
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
                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
               
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                

                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch ( o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    DropDownList_EXAD.Items.Insert(0, new ListItem("", ""));
                    break;
                case "C":
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    DropDownList_EXAD.Items.Insert(0, new ListItem("", ""));
                    break;
                case "D":

                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }

                    DropDownList_EXUNIT.Enabled = false;

                    TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                    TextBox_MZ_ID.Enabled = false;
                    break;
                case "E":

                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    DropDownList_EXUNIT.Enabled = false;
                    DropDownList_EXAD.Items.Insert(0, new ListItem("", ""));
                    break;
            }
        }

        protected void btPrint1_Click(object sender, EventArgs e)
        {
            if (TextBox_LOGDATE1.Text.Trim() != string.Empty && TextBox_LOGDATE2.Text.Trim() != string.Empty)
            {
                if (!(DateManange.Check_date(TextBox_LOGDATE1.Text) && DateManange.Check_date(TextBox_LOGDATE2.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                    return;
                }

                if (!(DateManange.Check_date(TextBox_LOGDATE1.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起始日期');", true);
                    return;
                }
            }

            if (string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue)
                && string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue)
                && string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim())
                && string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('機關、單位、身分證、姓名不可全空');", true);
                return;
            }

            string connstr = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();

            string tmp_url = "C_diffdutydetail_xls.aspx?MZ_ID="+ TextBox_MZ_ID.Text.Trim() + "&PM_FION=" + TPM_FION +
                    "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                    "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                    "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "window.open('" + tmp_url + "');", true);
        }

        protected void btPrint2_Click(object sender, EventArgs e)
        {
            if (TextBox_LOGDATE1.Text.Trim() != string.Empty && TextBox_LOGDATE2.Text.Trim() != string.Empty)
            {
                if (!(DateManange.Check_date(TextBox_LOGDATE1.Text) && DateManange.Check_date(TextBox_LOGDATE2.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                    return;
                }

                if (!(DateManange.Check_date(TextBox_LOGDATE1.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起始日期');", true);
                    return;
                }
            }

            if (string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue)
                && string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue)
                && string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim())
                && string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('機關、單位、身分證、姓名不可全空');", true);
                return;
            }
            string pdflink1 = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();

            string tmp_url = "C_diffdutydetail_doc.aspx?pdf=1&MZ_ID=" + TextBox_MZ_ID.Text.Trim() + "&PM_FION=" + TPM_FION +
                     "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                     "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                     "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            string filename = "/userfiles/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".pdf";
            string url = "http://" + Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_port"] + "/3-forleave/" + tmp_url;
            System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 3 -R 3 -T 5 -B 0 " + url + " " + Server.MapPath(filename));
            //System.Threading.Thread.Sleep(15000);
            int tmpSleep = 15000;
            int allsec = 0;
            while (true)
            {
                if (System.IO.File.Exists(Server.MapPath(filename)))
                {
                    System.IO.FileInfo fInfo = new System.IO.FileInfo(Server.MapPath(filename));
                    if (fInfo.Length > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "window.open('" + filename + "');", true);
                        break;
                    }
                    else
                    {
                        if (allsec >= 300)
                        {
                            return ;
                        }
                    }
                }

                allsec += 5;
                System.Threading.Thread.Sleep(tmpSleep);
            }

            //ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "window.open('" + filename + "');", true);
        }


        protected void btPrint_Click(object sender, EventArgs e)
        {


            if (TextBox_LOGDATE1.Text.Trim() != string.Empty && TextBox_LOGDATE2.Text.Trim() != string.Empty)
            {
                if (!(DateManange.Check_date(TextBox_LOGDATE1.Text) && DateManange.Check_date(TextBox_LOGDATE2.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                    return;
                }

                if (!(DateManange.Check_date(TextBox_LOGDATE1.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起始日期');", true);
                    return;
                }
            }

            if (string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue) 
                && string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue) 
                && string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim())
                && string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('機關、單位、身分證、姓名不可全空');", true);
                return;
            }


            string tmp_url = "";

            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();
           
            if (rbl_check.SelectedValue == "0")//CheckBox3.Checked
            {

                tmp_url = "C_rpt.aspx?fn=diffdutydetail2&TPM_FION=" + TPM_FION +
                    "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                    "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                    "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            }
            else if (rbl_check.SelectedValue == "1")//CheckBox1.Checked
            {

                tmp_url = "C_rpt.aspx?fn=diffdutydetail&TPM_FION=" + TPM_FION +
                    "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                    "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                    "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            }
            else if (rbl_check.SelectedValue == "2")//CheckBox2.Checked
            {
                
                tmp_url = "C_rpt.aspx?fn=diffdutydetail2&TPM_FION=" + TPM_FION +
                    "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                    "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                    "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            }
            else if (rbl_check.SelectedValue == "3")//CheckBox2.Checked
            {
               
                tmp_url = "C_rpt.aspx?fn=over_detail&TPM_FION=" + TPM_FION +
                    "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                    "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                    "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            }
            else if (rbl_check.SelectedValue == "4")//CheckBox2.Checked
            {
               
                tmp_url = "C_rpt.aspx?fn=day_detail&TPM_FION=" + TPM_FION +
                    "&DATE1=" + TextBox_LOGDATE1.Text + "&DATE2=" + TextBox_LOGDATE2.Text +
                    "&AD=" + DropDownList_EXAD.SelectedValue + "&UNIT=" + DropDownList_EXUNIT.SelectedValue +
                    "&NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) + "&TYPE=" + rbl_check.SelectedValue;
            }

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            


        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }


        protected void returnSameDataType(TextBox tb, object ob1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Focus();
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());

                    if (ob1 is DropDownList)
                    {
                        (ob1 as DropDownList).Focus();
                    }
                    else if (ob1 is TextBox)
                    {
                        (ob1 as TextBox).Focus();
                    }
                }
            }
        }

        protected void TextBox_LOGDATE1_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_LOGDATE1, TextBox_LOGDATE2);
        }

        protected void TextBox_LOGDATE2_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_LOGDATE2, TextBox_LOGDATE2);
        }
        //這殺小.用CheckBox稿RadioButton的功能.不要鬧了
        //protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (CheckBox3.Checked)
        //    {
        //        CheckBox2.Checked = false;
        //        CheckBox1.Checked = false;
        //    }
        //}

        //protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (CheckBox2.Checked)
        //    {
        //        CheckBox3.Checked = false;
        //        CheckBox1.Checked = false;
        //    }
        //}

        //protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (CheckBox1.Checked)
        //    {
        //        CheckBox2.Checked = false;
        //        CheckBox3.Checked = false;
        //    }
        //}

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //權限E & D 選擇所屬單位並鎖單位 matthew
            if (ViewState["C_strGID"].ToString() == "E" || ViewState["C_strGID"].ToString() == "D")
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
            else
            {
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
            }
        }
    }
}
