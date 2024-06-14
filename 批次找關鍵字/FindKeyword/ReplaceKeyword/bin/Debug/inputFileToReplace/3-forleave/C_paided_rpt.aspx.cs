using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_paided_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                ///群組權限
                //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);


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
                TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
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
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }

                    DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_EXUNIT.Enabled = false;
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
            string SQLPART = "(SELECT dbo.SUBSTR(DA,1,3)+'/'+dbo.SUBSTR(DA,4,2)+'/'+dbo.SUBSTR(DA,6,2) AS DA,PAY,IDCARD FROM B_SOLE WHERE 1=1 ";
            string rs = "(SELECT dbo.SUBSTR(DA,1,3)+'/'+dbo.SUBSTR(DA,4,2)+'/'+dbo.SUBSTR(DA,6,2) AS DA,PAY,IDCARD FROM B_SOLE WHERE 1=1 ";
            string strSQL = "SELECT DA,PAY,MZ_ID,MZ_POLNO,MZ_OCCC,'1' AS UNIT,MZ_UNIT,PAY_AD,MZ_NAME,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE, " + SQLPART + "  WHERE 1=1 ";

            if (!string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue))
            {
                strSQL += " AND PAY_AD='" + DropDownList_EXAD.SelectedValue.Trim() + "'";
                SQLPART += " AND PAY_AD='" + DropDownList_EXAD.SelectedValue.Trim() + "'";
            }

            if (!string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
            {
                strSQL += " AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue.Trim() + "'";
                SQLPART += " AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(TextBox_MZ_YEAR.Text))
            {
                SQLPART += " AND dbo.SUBSTR(DA,1,3)='" + TextBox_MZ_YEAR.Text + "'";
            }
            strSQL += " AND MZ_ID = IDCARD";
            SQLPART += " AND NUM = '5') BS ";
            strSQL += " ORDER BY MZ_UNIT,TBDV,OCCC";
            strSQL = strSQL.Replace(rs, SQLPART);
            DataTable nopaid = new DataTable();

            nopaid = o_DBFactory.ABC_toTest.Create_Table(strSQL, "nopaid");

            for (int i = 0; i < nopaid.Rows.Count; i++)
            {
                nopaid.Rows[i]["MZ_UNIT"] = o_A_KTYPE.RUNIT(nopaid.Rows[i]["MZ_UNIT"].ToString());
                nopaid.Rows[i]["MZ_OCCC"] = o_A_DLBASE.OCCC(nopaid.Rows[i]["MZ_ID"].ToString());
                nopaid.Rows[i]["UNIT"] = o_A_DLBASE.CUNIT(nopaid.Rows[i]["MZ_ID"].ToString());
            }

            Session["TITLE"] = string.Format("{0}{1}年度", DropDownList_EXAD.SelectedItem.Text, int.Parse(TextBox_MZ_YEAR.Text).ToString());

            Session["rpt_dt"] = nopaid;

            string tmp_url = "C_rpt.aspx?fn=paided&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);


        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_YEAR.Text = string.Empty;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["C_strGID"].ToString() == "E")//權限E選擇所屬單位並鎖單位
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
