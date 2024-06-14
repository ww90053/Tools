using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class A_EffectiveAll_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            DataTable rpt_dt = new DataTable();

            rpt_dt.Columns.Add("MZ_AD", typeof(string));
           
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


            string strADSQL = "SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE LIKE '382138%' AND MZ_KTYPE='04' ORDER BY MZ_KCODE";

            DataTable dt_ad = new DataTable();

            dt_ad = o_DBFactory.ABC_toTest.Create_Table(strADSQL, "GETADVALUE");

            for (int j = 0; j < dt_ad.Rows.Count; j++)
            {
                string strSQL1 = " WHERE MZ_AD='" + dt_ad.Rows[j]["MZ_KCODE"].ToString() + "' AND MZ_YEAR='" + TextBox_MZ_DATE1.Text + "'";

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
                            if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "甲")
                                A1++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "乙")
                                B1++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丙")
                                C1++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丁")
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
                            if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "甲")
                                A2++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "乙")
                                B2++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丙")
                                C2++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丁")
                                D2++;

                        }
                        else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "B00")
                        {
                            if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "甲")
                                A3++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "乙")
                                B3++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丙")
                                C3++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丁")
                                D3++;

                        }
                        else if (tempDT.Rows[i]["MZ_SRANK"].ToString().Substring(0, 2) == "G0")
                        {
                            if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "甲")
                                A4++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "乙")
                                B4++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丙")
                                C4++;
                            else if (tempDT.Rows[i]["MZ_SRANK"].ToString() == "丁")
                                D4++;

                        }
                    }

                    DataRow dr = rpt_dt.NewRow();

                    dr["MZ_AD"] = dt_ad.Rows[j]["MZ_KCHI"].ToString();
                    
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
            }

            Session["rpt_dt"] = rpt_dt;

            Session["TITLE"] = string.Format("新北市政府{0}年考績等次統計比例表", int.Parse(TextBox_MZ_DATE1.Text));

            string tmp_url = "A_rpt.aspx?fn=EffectiveAll&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
        }

    }
}
