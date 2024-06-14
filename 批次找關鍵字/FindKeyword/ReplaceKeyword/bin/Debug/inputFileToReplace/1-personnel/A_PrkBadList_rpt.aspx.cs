using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class A_PrkBadList_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
               

                A.fill_AD_POST(DropDownList_AD);
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = " AND MZ_AD='" + DropDownList_AD.SelectedValue + "' AND dbo.SUBSTR(MZ_DATE,1,5)='" + TextBox_MZ_DATE1.Text + "'";

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            DataTable rpt_dt = new DataTable();

            for (int i = 1; i <= 26; i++)
            {
                rpt_dt.Columns.Add("B" + i.ToString().PadLeft(2, '0'), typeof(string));
            }

            rpt_dt.Columns.Add("GROUP", typeof(string));

            List<String> PRRST = new List<string>();

            PRRST.Insert(0, "");
            PRRST.Insert(1, "('6010','6020','6030','6040','6050')");
            PRRST.Insert(2, "('6106','610A','6110','6120','6130','6140','6150')");
            PRRST.Insert(3, "('6201','6202')");
            PRRST.Insert(4, "('6405','6410','6420')");
            PRRST.Insert(5, "('6601','6602')");
            PRRST.Insert(6, "('6800')");
            PRRST.Insert(7, "('2203','2205','2206')");
            PRRST.Insert(8, "('2502','2503','2504')");
            PRRST.Insert(9, "2204");
            PRRST.Insert(10, "2501");
            PRRST.Insert(11, "5100");
            PRRST.Insert(12, "5020");
            PRRST.Insert(13, "5010");
            PRRST.Insert(14, "5002");
            PRRST.Insert(15, "5001");



            string[] SRANK = { "('G21','G22','G23','G24','P09','P08','P07','P06')", "('G01','G02','G03','G04','G11','G12','G13','G14','P01','P02','P03','P04','P05')" };

            for (int i = 0; i < SRANK.Length; i++)
            {
                for (int j = 0; j < PRRST.Count; j++)
                {
                    DataRow dr = rpt_dt.NewRow();

                    for (int k = 1; k <= 26; k++)
                    {
                        if (j == 0)
                        {
                            string selectSQL = "SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRK1='B" + k.ToString().PadLeft(2, '0') + "' AND MZ_SRANK IN " + SRANK[i] + " " + strSQL + "";
                            dr[k - 1] = o_DBFactory.ABC_toTest.vExecSQL(selectSQL);
                        }
                        else if (j <= 8)
                        {
                            string selectSQL = "SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRK1='B" + k.ToString().PadLeft(2, '0') + "' AND MZ_PRRST IN " + PRRST[j] + " AND MZ_SRANK IN " + SRANK[i] + " " + strSQL + "";
                            dr[k - 1] = o_DBFactory.ABC_toTest.vExecSQL(selectSQL);
                        }
                        else
                        {
                            string selectSQL = "SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRK1='B" + k.ToString().PadLeft(2, '0') + "' AND MZ_PRRST = " + PRRST[j] + " AND MZ_SRANK IN " + SRANK[i] + " " + strSQL + "";
                            dr[k - 1] = o_DBFactory.ABC_toTest.vExecSQL(selectSQL);
                        }
                    }
                    rpt_dt.Rows.Add(dr);
                }
            }

            Session["rpt_dt"] = rpt_dt;

            Session["TITLE"] = string.Format("{0}{1} {2}年{3}月警察人員懲戒統計表 ", DropDownList_AD.SelectedItem.Text, DropDownList_UNIT.SelectedItem.Text, int.Parse(TextBox_MZ_DATE1.Text.Substring(0, 3)), TextBox_MZ_DATE1.Text.Substring(3, 2));

            string tmp_url = "A_rpt.aspx?fn=PrkBadList&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue); 
        }
    }
}
