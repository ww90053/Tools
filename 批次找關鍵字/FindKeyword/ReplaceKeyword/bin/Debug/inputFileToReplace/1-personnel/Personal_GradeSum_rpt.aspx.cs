using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_GradeSum_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        string strSQL;
        DataTable temp;
        //string A_strGID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
 
                A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel3);

                //2013/12/02
                A.fill_MZ_PRID(DropDownList_MZ_PRID, 2);
                DropDownList_MZ_PRID.DataBind();
                DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                chk_TPMGroup();

               
            }
          
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
                    string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
                    DataTable tempDT = new DataTable();
                    tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    //2013/12/02
                    strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD WHERE (MZ_AD='" + tempDT.Rows[0][0].ToString() + "' OR MZ_AD='" + tempDT.Rows[0][1].ToString() + "')";
                    //matthew 中和要可以看到中一&中二
                    if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                    {
                        strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD where MZ_AD in ('382133400C','382133500C','382133600C')";
                    }
                    DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    DropDownList_MZ_PRID.DataSource = source;
                    DropDownList_MZ_PRID.DataTextField = "MZ_PRID";
                    DropDownList_MZ_PRID.DataValueField = "MZ_AD";                  
                    DropDownList_MZ_PRID.DataBind();
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
            Session["NO"] = string.Empty;
            Session["PRID"] = string.Empty;
            string tableName = "";
            List<string> condition = new List<string>();
            if (TextBox_MZ_NO.Text.Trim() != "")
            {
                condition.Add("MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "'");
                tableName = "A_PRKB";
                Session["NO"] = TextBox_MZ_NO.Text;

            }
            else if (DropDownList_MZ_PRID.SelectedValue.Trim() != "")
            {
                //20141231
                if (DropDownList_MZ_PRID.SelectedItem.Text == "新北警人")
                {
                    condition.Add(" AND (MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' OR  MZ_PRID='北警人')");
                }
                else
                {
                    condition.Add("MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "'");
                } 


                //condition.Add("MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "'");
                Session["PRID"] = DropDownList_MZ_PRID.SelectedItem.Text + "字";
                tableName = "A_PRK2";
                if (TextBox_MZ_PRID1.Text.Trim() != "")
                {
                    condition.Add("MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'");
                    string prid = Session["PRID"].ToString();
                    prid += "第 " + TextBox_MZ_PRID1.Text + " 號";
                    Session["PRID"] = prid;
                }
                else
                {
                    string prid = Session["PRID"].ToString();
                    prid += "第號";
                    Session["PRID"] = prid;
                }
            }
            string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);
            //初始化
            List<string> prrst = new List<string>();
            prrst.Add("4200");
            prrst.Add("4100");
            prrst.Add("4020");
            prrst.Add("4010");
            prrst.Add("4002");
            prrst.Add("4001");
            prrst.Add("5100");
            prrst.Add("5020");
            prrst.Add("5010");
            prrst.Add("5002");
            prrst.Add("5001");

            //職等
            //警正一階、二階
            string s1 = "'G21','G22'";

            //警正三階、四階
            string s2 = "'G23','G24'";

            //警佐
            string s3 = "'G11','G12','G13','G14'";

            //簡、薦、委任
            string s4 = "'P01','P02','P03','P04','P05','P06','P07','P08','P09','P10','P11','P12','P13','P14'";

            //合計
            int r1 = 0;
            int r2 = 0;
            int r3 = 0;
            int r4 = 0;
            int r5 = 0;
            int r6 = 0;
            int r7 = 0;
            int r8 = 0;
            int r9 = 0;
            int r10 = 0;
            int r11 = 0;

            DataTable gradeSum = new DataTable();
            gradeSum.Columns.Add("R1", typeof(int));
            gradeSum.Columns.Add("R2", typeof(int));
            gradeSum.Columns.Add("R3", typeof(int));
            gradeSum.Columns.Add("R4", typeof(int));
            gradeSum.Columns.Add("R5", typeof(int));
            gradeSum.Columns.Add("R6", typeof(int));
            gradeSum.Columns.Add("R7", typeof(int));
            gradeSum.Columns.Add("R8", typeof(int));
            gradeSum.Columns.Add("R9", typeof(int));
            gradeSum.Columns.Add("R10", typeof(int));
            gradeSum.Columns.Add("R11", typeof(int));
            gradeSum.Columns.Add("TITLE", typeof(string));

            //警正一、二
            DataRow newdr = gradeSum.NewRow();
            for (int i = 0; i < prrst.Count; i++)
            {
                strSQL = string.Format("SELECT COUNT(*) FROM " + tableName + " WHERE MZ_PRRST='{0}' AND  MZ_SRANK IN ({1}) {2} ", prrst[i], s1, where);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                newdr["R" + (i + 1)] = int.Parse(temp.Rows[0][0].ToString());
            }
            newdr["TITLE"] = "警正一階、警正二階：";
            gradeSum.Rows.Add(newdr);
            //警正三、四
            newdr = gradeSum.NewRow();
            for (int i = 0; i < prrst.Count; i++)
            {
                strSQL = string.Format("SELECT COUNT(*) FROM " + tableName + " WHERE MZ_PRRST='{0}' AND  MZ_SRANK IN ({1}) {2} ", prrst[i], s2, where);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                newdr["R" + (i + 1)] = int.Parse(temp.Rows[0][0].ToString());
            }
            newdr["TITLE"] = "警正三階、警正四階：";
            gradeSum.Rows.Add(newdr);
            //警佐
            newdr = gradeSum.NewRow();
            for (int i = 0; i < prrst.Count; i++)
            {
                strSQL = string.Format("SELECT COUNT(*) FROM " + tableName + " WHERE MZ_PRRST='{0}' AND  MZ_SRANK IN ({1}) {2} ", prrst[i], s3, where);
                newdr["R" + (i + 1)] = int.Parse(temp.Rows[0][0].ToString());
            }
            newdr["TITLE"] = "警佐：";
            gradeSum.Rows.Add(newdr);
            //簡、薦、委任
            newdr = gradeSum.NewRow();
            for (int i = 0; i < prrst.Count; i++)
            {
                strSQL = string.Format("SELECT COUNT(*) FROM " + tableName + " WHERE MZ_PRRST='{0}' AND  MZ_SRANK IN ({1}) {2} ", prrst[i], s4, where);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                newdr["R" + (i + 1)] = int.Parse(temp.Rows[0][0].ToString());
            }
            newdr["TITLE"] = "簡、薦、委任：";
            gradeSum.Rows.Add(newdr);
            //合計
            foreach (DataRow dr in gradeSum.Rows)
            {
                r1 += int.Parse(dr["R1"].ToString());
                r2 += int.Parse(dr["R2"].ToString());
                r3 += int.Parse(dr["R3"].ToString());
                r4 += int.Parse(dr["R4"].ToString());
                r5 += int.Parse(dr["R5"].ToString());
                r6 += int.Parse(dr["R6"].ToString());
                r7 += int.Parse(dr["R7"].ToString());
                r8 += int.Parse(dr["R8"].ToString());
                r9 += int.Parse(dr["R9"].ToString());
                r10 += int.Parse(dr["R10"].ToString());
                r11 += int.Parse(dr["R11"].ToString());
            }
            newdr = gradeSum.NewRow();
            newdr["R1"] = r1;
            newdr["R2"] = r2;
            newdr["R3"] = r3;
            newdr["R4"] = r4;
            newdr["R5"] = r5;
            newdr["R6"] = r6;
            newdr["R7"] = r7;
            newdr["R8"] = r8;
            newdr["R9"] = r9;
            newdr["R10"] = r10;
            newdr["R11"] = r11;
            newdr["TITLE"] = "合計";
            gradeSum.Rows.Add(newdr);

            int total = r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8 + r9 + r10 + r11;

            Session["TOTAL"] = total;
            Session["TITLE"] = string.Format("{0}獎懲個案件統計表", o_A_KTYPE.RAD(DropDownList_MZ_PRID.SelectedValue));

            Session["rpt_dt"] = gradeSum;

            string tmp_url = "A_rpt.aspx?fn=gradeSum&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            DropDownList_MZ_PRID.SelectedValue = "";
            TextBox_MZ_NO.Text = string.Empty;
            TextBox_MZ_PRID1.Text = string.Empty;
            DropDownList_MZ_PRID.Enabled = true;
            TextBox_MZ_NO.Enabled = true;
            TextBox_MZ_PRID1.Enabled = true;
        }

        protected void TextBox_MZ_NO_TextChanged(object sender, EventArgs e)
        {
            DropDownList_MZ_PRID.SelectedValue = "";
            TextBox_MZ_PRID1.Text = string.Empty;
            DropDownList_MZ_PRID.Enabled = false;
            TextBox_MZ_PRID1.Enabled = false;
        }

        protected void DropDownList_MZ_PRID_SelectedIndexChanged(object sender, EventArgs e)
        {
            chk_TPMGroup();
            TextBox_MZ_NO.Text = string.Empty;
            TextBox_MZ_NO.Enabled = false;
        }

    }
}
