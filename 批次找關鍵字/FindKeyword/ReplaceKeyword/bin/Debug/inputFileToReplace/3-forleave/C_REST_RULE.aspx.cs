using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_REST_RULE : System.Web.UI.Page
    {
        List<String> REST_RULE_RULE_NO; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            }

            ViewState["RULE_NO"] = Request["RULE_NO"];
            ViewState["RULE_DES"] = Request["RULE_DES"];

  
            if (!IsPostBack)
            {
                //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            C.set_Panel_EnterToTAB(ref this.Panel3);
                C.controlEnable(ref this.Panel1, false);
                string strSQL="SELECT RULE_NO FROM C_REST_RULE WHERE 1=1";
                if (ViewState["RULE_NO"] != null)
                {
                    if (ViewState["RULE_NO"].ToString()!="")
                    {
                        strSQL += " AND RULE_NO='" + ViewState["RULE_NO"].ToString().Trim()+ "'";
                    }
                    if (ViewState["RULE_DES"].ToString() != "")
                    {
                        strSQL += " AND RULE_DES LIKE '" + ViewState["RULE_DES"].ToString().Trim() + "%'";
                    }

                    strSQL += " ORDER BY RULE_NO";
                    
                    REST_RULE_RULE_NO = o_DBFactory.ABC_toTest.DataListArray(strSQL,"RULE_NO");
                    
                    Session["REST_RULE_RULE_NO"] = REST_RULE_RULE_NO;

                    if (REST_RULE_RULE_NO.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('C_REST_RULE.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");

                    }
                    else if (REST_RULE_RULE_NO.Count == 1)
                    {
                        xcount.Text = "0";
                        
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        
                    }
                    if (REST_RULE_RULE_NO.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + REST_RULE_RULE_NO.Count.ToString() + "筆";
                    }
                }
            }
        }

        

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO C_REST_RULE(RULE_SEQ,RULE_NO,RULE_DES) VALUES(@RULE_SEQ,@RULE_NO,@RULE_DES)";
          
            ViewState["Mode"] = "INSERT";
            C.controlEnable(ref this.Panel1, true );

        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "UPDATE C_REST_RULE SET RULE_SEQ=@RULE_SEQ,RULE_NO=@RULE_NO,RULE_DES=@RULE_DES WHERE RULE_NO='" + TextBox_RULE_NO.Text.Trim() + "'";
            
            ViewState["Mode"] = "UPDATE";
            C.controlEnable(ref this.Panel1, true );
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("RULE_NO", SqlDbType.VarChar).Value = TextBox_RULE_NO.Text.Trim();
                cmd.Parameters.Add("RULE_DES", SqlDbType.VarChar).Value = TextBox_RULE_DES.Text.Trim();
                C.controlEnable(ref this.Panel1, false);
            }

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            C.controlEnable(ref this.Panel1, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_P37_REST_RULE WHERE RULE_NO='" + TextBox_RULE_NO.Text + "'";
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                
                if (int.Parse(xcount.Text) != REST_RULE_RULE_NO.Count - 1)
                {
                    btNEXT.Enabled = true;
                }
                if (int.Parse(xcount.Text) == 0)
                {
                    btUpper.Enabled = false;
                }
            }
            else if (int.Parse(xcount.Text) == 0)
            {
                
                btUpper.Enabled = false;
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) - 1).ToString() + "筆共" + REST_RULE_RULE_NO.Count.ToString() + "筆";
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                

                if (int.Parse(xcount.Text) == REST_RULE_RULE_NO.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                

                if (int.Parse(xcount.Text) == REST_RULE_RULE_NO.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }

            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + REST_RULE_RULE_NO.Count.ToString() + "筆";
        }
    }
}
