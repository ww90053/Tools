using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class A_Effective_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            DataTable rpt_dt = new DataTable();

            rpt_dt.Columns.Add("TOTAL1", typeof(int));
            rpt_dt.Columns.Add("TOTAL2", typeof(int));
            rpt_dt.Columns.Add("TOTAL3", typeof(int));
            rpt_dt.Columns.Add("TOTAL4", typeof(int));
            rpt_dt.Columns.Add("TOTAL1_IN", typeof(int));
            rpt_dt.Columns.Add("TOTAL2_IN", typeof(int));
            rpt_dt.Columns.Add("TOTAL3_IN", typeof(int));
            rpt_dt.Columns.Add("TOTAL4_IN", typeof(int));
            rpt_dt.Columns.Add("TOTAL1_OUT", typeof(int));
            rpt_dt.Columns.Add("TOTAL2_OUT", typeof(int));
            rpt_dt.Columns.Add("TOTAL3_OUT", typeof(int));
            rpt_dt.Columns.Add("TOTAL4_OUT", typeof(int));
            rpt_dt.Columns.Add("TOTAL1_OTHER", typeof(int));
            rpt_dt.Columns.Add("TOTAL2_OTHER", typeof(int));
            rpt_dt.Columns.Add("TOTAL3_OTHER", typeof(int));
            rpt_dt.Columns.Add("TOTAL4_OTHER", typeof(int));
            rpt_dt.Columns.Add("A1", typeof(int));
            rpt_dt.Columns.Add("A2", typeof(int));
            rpt_dt.Columns.Add("A3", typeof(int));
            rpt_dt.Columns.Add("A4", typeof(int));
            rpt_dt.Columns.Add("B1", typeof(int));
            rpt_dt.Columns.Add("B2", typeof(int));
            rpt_dt.Columns.Add("B3", typeof(int));
            rpt_dt.Columns.Add("B4", typeof(int));
            rpt_dt.Columns.Add("C1", typeof(int));
            rpt_dt.Columns.Add("C2", typeof(int));
            rpt_dt.Columns.Add("C3", typeof(int));
            rpt_dt.Columns.Add("C4", typeof(int));
            rpt_dt.Columns.Add("D1", typeof(int));
            rpt_dt.Columns.Add("D2", typeof(int));
            rpt_dt.Columns.Add("D3", typeof(int));
            rpt_dt.Columns.Add("D4", typeof(int));


            string strSQL1 = "WHERE MZ_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_YEAR='" + TextBox_MZ_DATE1.Text + "'";

            string strSQL = "SELECT MZ_GRADE,MZ_SRANK FROM A_REV_BASE " + strSQL1 + " AND MZ_SWT='0'";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            int A1 = 0;//甲等警正
            int A2 = 0;
            int A3 = 0;
            int A4 = 0;
            int B1 = 0;//乙等警正
            int B2 = 0;
            int B3 = 0;
            int B4 = 0;
            int C1 = 0;//丙等警正
            int C2 = 0;
            int C3 = 0;
            int C4 = 0;
            int D1 = 0;//丁等警正
            int D2 = 0;
            int D3 = 0;
            int D4 = 0;


            if (tempDT.Rows.Count > 0)
            {
                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(tempDT.Rows[i]["MZ_SRANK"].ToString()))
                    {
                        continue;
                    }

                    if (tempDT.Rows[i]["MZ_SRANK"].ToString().Substring(0, 2) == "G2"
                     || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P06"
                     || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P07"
                     || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P08"
                     || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P09"
                       )
                    {
                        if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "甲")
                            A1++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "乙")
                            B1++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丙")
                            C1++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丁")
                            D1++;

                    }
                    else if (tempDT.Rows[i]["MZ_SRANK"].ToString().Substring(0, 2) == "G1"
                          || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P01"
                          || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P02"
                          || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P03"
                          || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P04"
                          || tempDT.Rows[i]["MZ_SRANK"].ToString() == "P05"
                          )
                    {
                        if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "甲")
                            A2++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "乙")
                            B2++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丙")
                            C2++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丁")
                            D2++;

                    }
                    else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "B00")
                    {
                        if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "甲")
                            A3++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "乙")
                            B3++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丙")
                            C3++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丁")
                            D3++;

                    }
                    else if (tempDT.Rows[i]["MZ_SRANK"].ToString().Substring(0, 2) == "G0")
                    {
                        if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "甲")
                            A4++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "乙")
                            B4++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丙")
                            C4++;
                        else if (tempDT.Rows[i]["MZ_GRADE"].ToString() == "丁")
                            D4++;

                    }
                }

                DataRow dr = rpt_dt.NewRow();

                dr["TOTAL1"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G21','G22','G23','G24','P06','P07','P08','P09') ");
                dr["TOTAL2"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G11','G12','G13','G14','P01','P02','P03','P04','P05') ");
                dr["TOTAL3"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK='B00' ");
                dr["TOTAL4"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G01','G02','G03','G04') ");
                dr["TOTAL1_IN"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G21','G22','G23','G24','P06','P07','P08','P09') AND MZ_SWT='0' ");
                dr["TOTAL2_IN"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G11','G12','G13','G14','P01','P02','P03','P04','P05') AND MZ_SWT='0' ");
                dr["TOTAL3_IN"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK='B00' AND MZ_SWT='0' ");
                dr["TOTAL4_IN"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G01','G02','G03','G04') AND MZ_SWT='0' ");
                dr["TOTAL1_OUT"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G21','G22','G23','G24','P06','P07','P08','P09') AND MZ_SWT='1' ");
                dr["TOTAL2_OUT"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G11','G12','G13','G14','P01','P02','P03','P04','P05') AND MZ_SWT='1' ");
                dr["TOTAL3_OUT"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK='B00' AND MZ_SWT='1' ");
                dr["TOTAL4_OUT"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G01','G02','G03','G04') AND MZ_SWT='1' ");
                dr["TOTAL1_OTHER"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G21','G22','G23','G24','P06','P07','P08','P09') AND MZ_SWT='3' ");
                dr["TOTAL2_OTHER"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G11','G12','G13','G14','P01','P02','P03','P04','P05') AND MZ_SWT='3' ");
                dr["TOTAL3_OTHER"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK='B00' AND MZ_SWT='3' ");
                dr["TOTAL4_OTHER"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_REV_BASE " + strSQL1 + " AND MZ_SRANK IN ('G01','G02','G03','G04') AND MZ_SWT='3' ");
                dr["A1"] = A1;
                dr["A2"] = A2;
                dr["A3"] = A3;
                dr["A4"] = A4;
                dr["B1"] = B1;
                dr["B2"] = B2;
                dr["B3"] = B3;
                dr["B4"] = B4;
                dr["C1"] = C1;
                dr["C2"] = C2;
                dr["C3"] = C3;
                dr["C4"] = C4;
                dr["D1"] = D1;
                dr["D2"] = D2;
                dr["D3"] = D3;
                dr["D4"] = D4;

                rpt_dt.Rows.Add(dr);
            }

            Session["rpt_dt"] = rpt_dt;

            Session["TITLE"] = string.Format("{0}{1}年考績（成）人數統計表", DropDownList_AD.SelectedItem.Text, int.Parse(TextBox_MZ_DATE1.Text));

            string tmp_url = "A_rpt.aspx?fn=Effective&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
        }
    }
}
