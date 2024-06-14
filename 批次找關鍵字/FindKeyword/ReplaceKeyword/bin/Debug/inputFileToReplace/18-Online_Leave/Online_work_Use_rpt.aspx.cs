using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._18_Online_Leave
{
    public partial class Online_work_Use_rpt : System.Web.UI.Page
    {
        string C_strGID = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["ADServerID"] != null)
            {
                switch (Request.QueryString["TPM_FION"])
                {
                    case "":
                    case null:
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), "0", "TPFXXX0001");
                        Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                        break;
                    default:
                        if (TPMPermissions._boolPermissionID(int.Parse(Session["TPM_MID"].ToString()), Request.QueryString["TPM_FION"].ToString(), "PVIEW") == false)
                        {
                            //無權限
                            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                            Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                        }
                        break;
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }

            ///群組權限
            C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            //by MQ ------------------------------20100331
            Online.set_Panel_EnterToTAB(ref this.Panel1);
            Online.set_Panel_EnterToTAB(ref this.Panel2);

            if (!IsPostBack)
            {
                DropDownList_MZ_AD.DataBind();
                DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
            }
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            SqlDataSource1.SelectCommand = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            DropDownList_MZ_AD.DataBind();
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (C_strGID)
            {
                case "A":

                    break;
                case "B":

                    break;
                case "C":
                    ChangeDropDownList_AD();
                    DropDownList_MZ_AD.Enabled = false;
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    ChangeDropDownList_AD();
                    break;
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_IDATE1.Text = string.Empty;
            TextBox_IDATE2.Text = string.Empty;
        }

        protected void DropDownList_MZ_AD_DataBound(object sender, EventArgs e)
        {
            DropDownList_MZ_AD.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = "";

            if (!string.IsNullOrEmpty(DropDownList_MZ_AD.SelectedValue))
            {
                strSQL += string.Format("SELECT MZ_EXAD,MZ_EXUNIT,COUNT(*) AS COUNT FROM C_DLTB01  WHERE SIGN_KIND='2' AND MZ_IDATE1>='{1}' AND MZ_IDATE1<='{2}' AND MZ_EXAD='{0}'  GROUP BY MZ_EXAD,MZ_EXUNIT ORDER BY MZ_EXAD,MZ_EXUNIT", DropDownList_MZ_AD.SelectedValue, TextBox_IDATE1.Text, TextBox_IDATE2.Text);
            }
            else
            {
                strSQL += string.Format("SELECT MZ_EXAD,COUNT(*) FROM C_DLTB01  WHERE SIGN_KIND='2' AND MZ_IDATE1>='{0}' AND MZ_IDATE1<='{1}' GROUP BY MZ_EXAD ORDER BY MZ_EXAD", TextBox_IDATE1.Text, TextBox_IDATE2.Text);
            }

            DataTable DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            DataTable EXCEL_DT = new DataTable();

            EXCEL_DT.Columns.Add("機關", typeof(string));


            if (!string.IsNullOrEmpty(DropDownList_MZ_AD.SelectedValue))
            {
                EXCEL_DT.Columns.Add("單位", typeof(string));
            }

            EXCEL_DT.Columns.Add("次數", typeof(string));

            foreach (DataRow dr in DT.Rows)
            {
                DataRow EXCEL_DR = EXCEL_DT.NewRow();

                EXCEL_DR["機關"] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_EXAD"].ToString(), "04");

                if (!string.IsNullOrEmpty(DropDownList_MZ_AD.SelectedValue))
                {
                    EXCEL_DR["單位"] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_EXUNIT"].ToString(), "25");
                }

                EXCEL_DR["次數"] = dr["COUNT"].ToString();

                EXCEL_DT.Rows.Add(EXCEL_DR);
            }

            App_Code.ToExcel.Dt2Excel(EXCEL_DT, "");
        }
    }
}
