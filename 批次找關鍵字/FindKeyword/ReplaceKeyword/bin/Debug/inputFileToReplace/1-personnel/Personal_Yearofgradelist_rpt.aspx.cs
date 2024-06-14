using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;



namespace TPPDDB._1_personnel
{
    public partial class Personal_Yearofgradelist_rpt : System.Web.UI.Page
    {

        int TPM_FION=0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {//判斷使用者是否有權限進入系統
                A.check_power();
                A.fill_AD_POST(DropDownList_MZ_AD); 
                DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                ViewState ["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                chk_TPMGroup();
            }
            

                      

        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //2013/12/02
            strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            //matthew 如果現職單位是中和分局 中和一&中和二要一起加進來
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='382133400C' OR MZ_KCODE='382133500C' OR MZ_KCODE='382133600C')";
            }
            DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_MZ_AD.DataSource = source;
            DropDownList_MZ_AD.DataTextField = "MZ_KCHI";
            DropDownList_MZ_AD.DataValueField = "MZ_KCODE";
            DropDownList_MZ_AD.DataBind();

            DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["A_strGID"].ToString() )
            {
                case "A":
                case "B":

                    break;
                case "C":
                    ChangeDropDownList_AD();
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            
            string tmp_url = "A_rpt.aspx?fn=yearofgradelist&TPM_FION=" + TPM_FION + "&MZ_AD=" + DropDownList_MZ_AD.SelectedValue.Trim() +
                "&MZ_DATE1=" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "&MZ_DATE2=" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0');

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
        }

        protected void returnSameDataType(TextBox tb, object obj1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!App_Code.DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Focus();
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());

                    if (obj1 is TextBox)
                    {
                        TextBox tb1 = obj1 as TextBox;
                        tb1.Focus();
                    }
                    else if (obj1 is DropDownList)
                    {
                        DropDownList dl1 = obj1 as DropDownList;
                        dl1.Focus();
                    }

                }
            }
        }

        protected void btnexcel_Click(object sender, EventArgs e)
        {
            DataTable Yearofgradelist = RPT.punish.A_yearofgradelist.doSearch(DropDownList_MZ_AD.SelectedValue, TextBox_MZ_DATE1.Text.Trim(), TextBox_MZ_DATE2.Text.Trim(),1);


            //DataTable Yearofgradelist = new DataTable();

            //Yearofgradelist.Columns.Add("單位", typeof(string));
            //Yearofgradelist.Columns.Add("職稱", typeof(string));
            //Yearofgradelist.Columns.Add("姓名", typeof(string));
            //Yearofgradelist.Columns.Add("身份證字號", typeof(string));
            //Yearofgradelist.Columns.Add("嘉獎", typeof(int));
            //Yearofgradelist.Columns.Add("記功", typeof(int));
            //Yearofgradelist.Columns.Add("記一大功", typeof(int));
            //Yearofgradelist.Columns.Add("記二大功", typeof(int));
            //Yearofgradelist.Columns.Add("申誡", typeof(int));
            //Yearofgradelist.Columns.Add("記過", typeof(int));
            //Yearofgradelist.Columns.Add("記一大過", typeof(int));
            ////Yearofgradelist.Columns.Add("TBDV", typeof(int));
            ////Yearofgradelist.Columns.Add("OCCC", typeof(string));
            //Yearofgradelist.Columns.Add("功過相抵", typeof(int));

            //string sqlPart = "";

            //if (DropDownList_MZ_AD.SelectedValue != "")
            //{
            //    sqlPart = " MZ_AD='" + DropDownList_MZ_AD.SelectedValue + "'";
            //}

            //if (!string.IsNullOrEmpty(TextBox_MZ_DATE1.Text.Trim()) && !string.IsNullOrEmpty(TextBox_MZ_DATE2.Text.Trim()))
            //{
            //    sqlPart += " AND MZ_DATE>='" + o_str.tosql(TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0')) + "' AND MZ_DATE<='" + o_str.tosql(TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0')) + "'";
            //}
            //else if (!string.IsNullOrEmpty(TextBox_MZ_DATE1.Text.Trim()))
            //{
            //    sqlPart += " AND MZ_DATE='" + o_str.tosql(TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0')) + "'";
            //}

            //string selectString = "SELECT MZ_ID,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_ID IN (SELECT DISTINCT MZ_ID FROM A_PRK2 WHERE " + sqlPart + ") ORDER BY TBDV,MZ_UNIT,MZ_OCCC"; //"SELECT DISTINCT MZ_ID,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_PRK2 WHERE " + sqlPart + " ORDER BY TBDV,MZ_UNIT,MZ_OCCC";

            //DataTable rpt_dt = o_DBFactory.ABC_toTest.Create_Table(selectString, "get");     
          

            
            //for (int i = 0; i < rpt_dt.Rows.Count; i++)
            //{

               

                //string P4001 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4001' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P4002 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4002' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P4010 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4010' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P4020 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4020' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P4100 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4100' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P4200 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4200' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P5001 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5001' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P5002 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5002' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P5010 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5010' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P5020 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5020' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);
                //string P5100 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5100' AND MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "' AND" + sqlPart);

                //int i4001 = int.Parse(string.IsNullOrEmpty(P4001) ? "0" : P4001) + int.Parse(string.IsNullOrEmpty(P4002) ? "0" : P4002) * 2;
                //int i4010 = int.Parse(string.IsNullOrEmpty(P4010) ? "0" : P4010) + int.Parse(string.IsNullOrEmpty(P4020) ? "0" : P4020) * 2;
                //int i4100 = int.Parse(string.IsNullOrEmpty(P4100) ? "0" : P4100);
                //int i4200 = int.Parse(string.IsNullOrEmpty(P4200) ? "0" : P4200);

                //int i5001 = int.Parse(string.IsNullOrEmpty(P4001) ? "0" : P5001) + int.Parse(string.IsNullOrEmpty(P5002) ? "0" : P5002) * 2;
                //int i5010 = int.Parse(string.IsNullOrEmpty(P4010) ? "0" : P5010) + int.Parse(string.IsNullOrEmpty(P5020) ? "0" : P5020) * 2;
                //int i5100 = int.Parse(string.IsNullOrEmpty(P4100) ? "0" : P5100);

                //int TOTAL = i4001 + i4010 * 3 + i4100 * 9 + i4200 * 2 * 9 - i5001 - i5010 * 3 - i5100 * 9;

                //DataRow tempRow = Yearofgradelist.NewRow();

                //tempRow["單位"] = o_A_DLBASE.CUNIT(rpt_dt.Rows[i][0].ToString());
                //tempRow["職稱"] = o_A_DLBASE.OCCC(rpt_dt.Rows[i][0].ToString());
                //tempRow["姓名"] = o_A_DLBASE.CNAME(rpt_dt.Rows[i][0].ToString());
                //tempRow["身份證字號"] = rpt_dt.Rows[i][0].ToString();
                //tempRow["嘉獎"] = i4001;
                //tempRow["記功"] = i4010;
                //tempRow["記一大功"] = i4100;
                //tempRow["記二大功"] = i4200;
                //tempRow["申誡"] = i5001;
                //tempRow["記過"] = i5010;
                //tempRow["記一大過"] = i5100;
                ////tempRow["TBDV"] = rpt_dt.Rows[i]["TBDV"].ToString();
                ////tempRow["OCCC"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "'");
                //tempRow["功過相抵"] = TOTAL;

                //Yearofgradelist.Rows.Add(tempRow);
            //}
            string Title = string.Format("{0}{1}年度獎懲統計表", o_A_KTYPE.RAD(DropDownList_MZ_AD.SelectedValue.Trim()), (DateTime.Now.Year - 1911).ToString());

            App_Code.ToExcel.Dt2Excel(Yearofgradelist, Title);


        }

        

        //protected void TextBox_MZ_DATE1_TextChanged(object sender, EventArgs e)
        //{
        //    returnSameDataType(TextBox_MZ_DATE1, TextBox_MZ_DATE2);
        //}

        //protected void TextBox_MZ_DATE2_TextChanged(object sender, EventArgs e)
        //{
        //    returnSameDataType(TextBox_MZ_DATE2, DropDownList_MZ_AD);
        //}

    }
}
