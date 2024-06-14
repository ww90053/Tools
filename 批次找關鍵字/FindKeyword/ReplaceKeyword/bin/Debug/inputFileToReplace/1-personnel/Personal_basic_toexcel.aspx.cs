using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace TPPDDB._1_personnel
{
    public partial class Personal_basic_toexcel : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //判斷使用者是否有權限進入系統
            if (!Page.IsPostBack)
            {
                A.check_power();

                DropDownList_AD.DataBind();
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
            }
        }

        ///<summary>
        ///群組權限
        ///</summary>
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

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void bt_TRANFER_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT";
            int k = 0;

            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    k++;

                    if (k == 1)
                    {
                        strSQL += " " + CheckBoxList1.Items[i].Value + " AS " + CheckBoxList1.Items[i].Text + "";
                    }
                    else
                    {
                        strSQL += "," + CheckBoxList1.Items[i].Value + " AS " + CheckBoxList1.Items[i].Text + "";
                    }
                }
            }

            strSQL += " FROM A_DLBASE WHERE 1=1";//AND MZ_STATUS2 = 'Y' AND MZ_AD='382130300C'

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                strSQL += " AND MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            strSQL += " AND dbo.SUBSTR(MZ_EXAD,1,5)='38213'";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                try
                {
                    tempDT.Rows[i]["現服機關"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["現服機關"].ToString(), "04");
                }
                catch
                {
                }

                try
                {
                    tempDT.Rows[i]["現服單位"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["現服單位"].ToString(), "25");
                }
                catch
                {
                }

                try
                {
                    tempDT.Rows[i]["職稱"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["職稱"].ToString(), "26");
                }
                catch
                {
                }
                // tempDT.Rows[i]["MZ_TBDV"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["MZ_TBDV"].ToString(), "43");
                try
                {
                    tempDT.Rows[i]["職等"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["職等"].ToString(), "09");
                }
                catch
                {
                }

                try
                {
                    tempDT.Rows[i]["編制機關"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["編制機關"].ToString(), "04");
                }
                catch
                {
                }

                try
                {
                    tempDT.Rows[i]["編制單位"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["編制單位"].ToString(), "25");
                }
                catch
                {
                }
                try
                {
                    tempDT.Rows[i]["兵種"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["兵種"].ToString(), "MIK");
                }
                catch
                {
                }
                try
                {
                    tempDT.Rows[i]["官階"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["官階"].ToString(), "MIR");
                }
                catch
                {
                }
            }
            
            // 20140123
            App_Code.ToExcel.Dt2Excel(tempDT, "Download");
            //o_DBFactory.ABC_toTest.Dt2Excel(tempDT, "");
        }

        protected void DropDownList_AD_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_AD.Items.Insert(0, li);
        }
    }
}
