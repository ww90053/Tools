using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Criminal_police2_rpt : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();

                A.fill_AD(DropDownList_AD);
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
          
                chk_TPMGroup();
            }
        }

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
        /// <summary>
        /// 刑事警察
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL_Click(object sender, EventArgs e)
        {
            //DataTable Criminal_police = new DataTable();

            //Criminal_police.Columns.Add("MZ_UNIT", typeof(string));
            //Criminal_police.Columns.Add("MZ_OCCC", typeof(string));
            //Criminal_police.Columns.Add("MZ_NAME", typeof(string));
            //Criminal_police.Columns.Add("MZ_IDNO", typeof(string));
            //Criminal_police.Columns.Add("MZ_NO1", typeof(string));
            //Criminal_police.Columns.Add("MZ_PIC", System.Type.GetType("System.Byte[]"));
            //Criminal_police.Columns.Add("MZ_DATE", typeof(string));
            //Criminal_police.Columns.Add("MZ_MEMO", typeof(string));
            //Criminal_police.Columns.Add("MZ_BIR", typeof(string));

            string strSQL = "SELECT A.*,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_POLICE A,A_DLBASE B WHERE MZ_IDNO!=' ' AND A.MZ_ID=B.MZ_ID AND MZ_SWT='2' ";

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                strSQL += " AND B.MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND B.MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            //2013/05/29
            if (string.IsNullOrEmpty(txt_Year.Text) || string.IsNullOrEmpty(txt_Year2.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請輸入發證日期起迄日期');", true);
                return;
            }
           //2013/05/29
            strSQL += " AND  A.MZ_DATE >= '" + txt_Year.Text + "' AND A.MZ_DATE <='" + txt_Year2.Text + "'";
           

            //2013/05/30
            //strSQL += " ORDER BY TBDV,B.MZ_OCCC";
            strSQL += " ORDER BY A.MZ_IDNO";
           

            Session["RPT_SQL_A"] = strSQL;
            
         

            Session["TITLE"] = string.Format("{0}請領{1}年刑事警察人員名冊", DropDownList_AD.SelectedItem.Text, DateTime.Now.Year - 1911);

           

            string tmp_url = "A_rpt.aspx?fn=Criminal_police_2";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

    
        /// <summary>
        /// 行政警察
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_MAKE_ALL1_Click(object sender, EventArgs e)
        {
            

            string strSQL = "SELECT A.*,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_POLICE A,A_DLBASE B WHERE MZ_IDNO!=' ' AND A.MZ_ID=B.MZ_ID AND (MZ_SWT='1' OR MZ_SWT='3' ) ";

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                strSQL += " AND B.MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND B.MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            //2013/05/29
            if (string.IsNullOrEmpty(txt_Year.Text) || string.IsNullOrEmpty(txt_Year2.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請輸入發證日期起迄日期');", true);
                return;
            }
            
            strSQL += " AND  A.MZ_DATE >= '" + txt_Year.Text + "' AND A.MZ_DATE <='" + txt_Year2.Text + "'";
                        
            //2013/05/29

            //2013/05/30
            //strSQL += " ORDER BY TBDV,B.MZ_OCCC";
            strSQL += " ORDER BY A.MZ_IDNO";
            //
            Session["RPT_SQL_A"] = strSQL;
            

            Session["TITLE"] = string.Format("{0}請領{1}年警察人員服務證名冊", DropDownList_AD.SelectedItem.Text, DateTime.Now.Year - 1911);

           

            string tmp_url = "A_rpt.aspx?fn=Normal_police_2";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

       

        protected void Button_MAKE_ALL2_Click(object sender, EventArgs e)
        {
            

            string strSQL = "SELECT A.*,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_POLICE A,A_DLBASE B WHERE MZ_IDNO!=' ' AND A.MZ_ID=B.MZ_ID AND ( MZ_SWT='4') ";

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                strSQL += " AND B.MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND B.MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            //2013/05/29
            if (string.IsNullOrEmpty(txt_Year.Text) || string.IsNullOrEmpty(txt_Year2.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請輸入發證日期起迄日期');", true);
                return;
            }
            
            strSQL += " AND  A.MZ_DATE >= '" + txt_Year.Text + "' AND A.MZ_DATE <='" + txt_Year2.Text + "'";
                        
            //2013/05/29

            //2013/05/30
            //strSQL += " ORDER BY TBDV,B.MZ_OCCC";
            strSQL += " ORDER BY A.MZ_IDNO";
            //
            Session["RPT_SQL_A"] = strSQL;
           

            Session["TITLE"] = string.Format("{0}請領{1}年一般行政人員服務證名冊", DropDownList_AD.SelectedItem.Text, DateTime.Now.Year - 1911);

           

            string tmp_url = "A_rpt.aspx?fn=Normal_police_2";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);


        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }

        public byte[] imageToByte(string path)
        {
            string file_path = string.Empty;
            try
            {
                file_path = Server.MapPath(path);
                System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                BinaryReader br = new BinaryReader(fileOpen);
                byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                fileOpen.Close();
                //XX2013/06/18 
                fileOpen.Dispose();
                return by;

            }
            catch
            {
                file_path = Server.MapPath("~/1-personnel/images/nopic.jpg");
                System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                BinaryReader br = new BinaryReader(fileOpen);
                byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                fileOpen.Close();
                //XX2013/06/18 
                fileOpen.Dispose();
                return by;
            }
        }
    }
}
