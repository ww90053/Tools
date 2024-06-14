using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTYHOUR_CODE : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                
                C.set_Panel_EnterToTAB(ref this.Panel1);

              
                C.controlEnable(ref this.Panel1, false);

                btn_newType.Enabled = true;
                TextBox_Duty_TYPE.Enabled = true;

                Label1.Text = "輪值設定";
                DataTable dt = new DataTable();
                string sql = "select DUTY_KIND,DUTY_KINDNAME from  C_ONDUTY_CODE  where DUTY_TYPE='1'";
                dt=o_DBFactory.ABC_toTest.Create_Table(sql,"get");
               
                dp_TYPE.DataSource = dt;
                dp_TYPE.DataTextField = "DUTY_KINDNAME";
                dp_TYPE.DataValueField = "DUTY_KIND";
                dp_TYPE.DataBind();
               

            }
        }

        protected void execSQL()
        {
    
            string ErrorString = "";


            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_ONDUTY_CODE", tbox.Text);//??

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            tbox.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            tbox.BackColor = Color.White;
            //        }
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改輸入變色之欄位');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string result = "";//!!
                    result = o_DBFactory.ABC_toTest.vExecSQL("select DUTY_KIND from C_ONDUTY_CODE where DUTY_TYPE='1' and DUTY_KINDNAME='" + dp_TYPE.SelectedItem.Text+ "'");
      
                  
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        //!!
                        string sql = "select DUTY_KIND from C_ONDUTY_CODE where DUTY_TYPE='1' and DUTY_KINDNAME='" + dp_TYPE.SelectedItem.Text + "'";
                        string TYPE = o_DBFactory.ABC_toTest.vExecSQL(sql);
                        //!!
                        
                        string sqlKIND = @"select substr(DUTY_KIND,2,1) DUTY_KIND,DUTY_KINDNAME from C_ONDUTY_CODE where DUTY_TYPE='2'
                                        and substr( DUTY_KIND,0,1)='"+TYPE+"'";
                        string KIND = o_DBFactory.ABC_toTest.vExecSQL(sqlKIND);
                        if (KIND == "")
                        {
                            KIND = "0";
                        }
                        int KINDA = int.Parse(KIND)+1;
                        KIND = KINDA.ToString();
                        cmd.Parameters.Add("DUTY_KIND", SqlDbType.VarChar).Value = result + KIND;
                        cmd.Parameters.Add("DUTY_HOUR", SqlDbType.Float).Value = int.Parse(TextBox_DUTY_HOUR.Text);
                        cmd.Parameters.Add("DUTY_PAY", SqlDbType.Float).Value = int.Parse(TextBox_DUTY_PAY.Text);
                        cmd.Parameters.Add("DUTY_KINDNAME", SqlDbType.VarChar).Value = TextBox_Duty_TYPE.Text + DP_Duty_KIND.Text;
                        cmd.ExecuteNonQuery();
                    }

                    if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        
                        cmd.Parameters.Add("DUTY_HOUR", SqlDbType.Float).Value = int.Parse(TextBox_DUTY_HOUR.Text);
                        cmd.Parameters.Add("DUTY_PAY", SqlDbType.Float).Value = int.Parse(TextBox_DUTY_PAY.Text);
                        cmd.Parameters.Add("DUTY_KINDNAME", SqlDbType.VarChar).Value = DP_Duty_KIND.Text;
                        cmd.ExecuteNonQuery();
                    
                    }

               


                    if (ViewState["Mode"].ToString() == "INSERT")//!!
                    {


                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                       
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }

                    TextBox_Duty_TYPE.Text = string.Empty;
                    DP_Duty_KIND.Text = string.Empty;
                    TextBox_DUTY_HOUR.Text = string.Empty;
                    TextBox_DUTY_PAY.Text = string.Empty;

                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;

                    btInsert.Enabled = true;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    ViewState.Remove("Mode");

                    C.controlEnable(ref this.Panel1, false);

                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                    }
                }
                finally
                {
                    conn.Close();

                    conn.Dispose();
                }
            }
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
         

            ViewState["CMDSQL"] = "INSERT INTO C_ONDUTY_CODE(DUTY_TYPE,DUTY_KIND,DUTY_HOUR,DUTY_PAY,DUTY_KINDNAME) VALUES('2', @DUTY_KIND, @DUTY_HOUR,@DUTY_PAY,@DUTY_KINDNAME)";
            TextBox_Duty_TYPE.Text = string.Empty;
            DP_Duty_KIND.Text = string.Empty;
            TextBox_DUTY_HOUR.Text = string.Empty;
            TextBox_DUTY_PAY.Text = string.Empty;

            btCancel.Enabled = true;
            btOK.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;

            dp_TYPE.Enabled = true;

            C.controlEnable(ref this.Panel1, true);

        
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先新增資料')", true);
                return;
            }
            else if (GridView1.SelectedRow == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                return;
            }
            else
            {
                ViewState["Mode"] = "UPDATE";
                string KIND = "",sqlKind="";
                sqlKind = "select DUTY_KIND from C_ONDUTY_CODE where DUTY_KINDNAME='" + GridView1.SelectedRow.Cells[0].Text+"'";//??
                KIND = o_DBFactory.ABC_toTest.vExecSQL(sqlKind);

                string TYPE = "", sqlType = "";
               
                string strKIND = "select DUTY_KIND  from C_ONDUTY_CODE where DUTY_TYPE='2' and DUTY_KINDNAME='" + GridView1.SelectedRow.Cells[1].Text + "'";

                String value = o_DBFactory.ABC_toTest.vExecSQL(strKIND);

                sqlType = "select substr(DUTY_KIND,2,1) DUTY_KIND,DUTY_KINDNAME from C_ONDUTY_CODE where DUTY_TYPE='2' and DUTY_KIND='" + value + "'";
                TYPE = o_DBFactory.ABC_toTest.vExecSQL(sqlType);
                TYPE = KIND + TYPE;
                ViewState["CMDSQL"] = "UPDATE C_ONDUTY_CODE SET DUTY_HOUR=@DUTY_HOUR,DUTY_PAY=@DUTY_PAY,DUTY_KINDNAME=@DUTY_KINDNAME" +
                                                       " WHERE DUTY_TYPE= '2' and DUTY_KIND='" + TYPE + "'";

                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btOK.Enabled = true;
                

                C.controlEnable(ref this.Panel1, true);

                dp_TYPE.Enabled = false;
             
            }
        }
        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedRow == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                return;
            }
            else
            {
                //!!
                string KIND = "", sqlKind = "";
                sqlKind = "select DUTY_KIND from C_ONDUTY_CODE where DUTY_KINDNAME='" + GridView1.SelectedRow.Cells[0].Text + "'";
                KIND = o_DBFactory.ABC_toTest.vExecSQL(sqlKind);
                //!!

                string TYPE = "", sqlType = "";
                sqlType = "select DUTY_KIND from C_ONDUTY_CODE where DUTY_KINDNAME='" + GridView1.SelectedRow.Cells[1].Text + "'";
                TYPE = o_DBFactory.ABC_toTest.vExecSQL(sqlType);

                string DeleteString = "DELETE FROM C_ONDUTY_CODE WHERE DUTY_TYPE= '2' and DUTY_KIND='" + TYPE + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //LOG紀錄
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    search();
                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;

                    TextBox_Duty_TYPE.Text = string.Empty;
                    DP_Duty_KIND.Text = string.Empty;
                    TextBox_DUTY_HOUR.Text = string.Empty;
                    TextBox_DUTY_PAY.Text = string.Empty;
                    
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗')", true);
                }

            }
        }
        protected void btCancel_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            btDelete.Enabled = false;
            btOK.Enabled = false;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = true;
            TextBox_Duty_TYPE.Text = string.Empty;
            DP_Duty_KIND.Text = string.Empty;
            TextBox_DUTY_HOUR.Text = string.Empty;
            TextBox_DUTY_PAY.Text = string.Empty;


            C.controlEnable(ref this.Panel1, false);

            btn_newType.Enabled = true;
            TextBox_Duty_TYPE.Enabled = true;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btOK.Enabled = false;
                btCancel.Enabled = true;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btInsert.Enabled = false;

                dp_TYPE.SelectedItem.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                DP_Duty_KIND.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
                TextBox_DUTY_HOUR.Text = GridView1.SelectedRow.Cells[2].Text.Trim();
                TextBox_DUTY_PAY.Text = GridView1.SelectedRow.Cells[3].Text.Trim();
                dp_TYPE.Enabled = false;
                btSearch.Enabled = false;
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            execSQL();
            search();
            btn_newType.Enabled = true;
            TextBox_Duty_TYPE.Enabled = true;
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {

            string strSQL = @"select CO.DUTY_KINDNAME, CC.* from C_ONDUTY_CODE  CC
                    LEFT JOIN   C_ONDUTY_CODE   CO ON substr(CC. DUTY_KIND,0,1)=CO.DUTY_KIND 
                    where CC.DUTY_TYPE='2'";
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL,"CODE");
            GridView1.DataSource = dt;
            GridView1.DataBind();
           
        }

        protected void btnewType_Click(object sender, EventArgs e)
        {
            string sql="",result="";
            char resultC=' ';
            sql = "select MAX(DUTY_KIND) from C_ONDUTY_CODE where DUTY_TYPE=1";
            result=o_DBFactory.ABC_toTest.vExecSQL(sql);
            char result1 = Convert.ToChar(result);
            int resultNum = Convert.ToInt32(result1);
            resultNum = resultNum + 1;
            resultC = (char)resultNum;

            //!!
            string sqlKIND = "select DUTY_KIND from C_ONDUTY_CODE where DUTY_TYPE=1 and DUTY_KINDNAME='" + TextBox_Duty_TYPE.Text + "'";
            string KIND = "";
            KIND= o_DBFactory.ABC_toTest.vExecSQL(sqlKIND);

            if (KIND != "")
            {

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已有該輪值類型')", true);
                return;
            }
            //!!

            if (TextBox_Duty_TYPE.Text != "")
            {

                sql = string.Format("INSERT INTO   C_ONDUTY_CODE(DUTY_TYPE, DUTY_KIND,DUTY_KINDNAME) VALUES('{0}', '{1}', '{2}')", 1, resultC, TextBox_Duty_TYPE.Text);
                o_DBFactory.ABC_toTest.Edit_Data(sql);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                TextBox_Duty_TYPE.Text = String.Empty;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入輪值類型')", true);
                return;
            }

            //!!
            DataTable dt = new DataTable();
            string sqlTYPE = "select DUTY_KIND,DUTY_KINDNAME from  C_ONDUTY_CODE  where DUTY_TYPE='1'";
            dt = o_DBFactory.ABC_toTest.Create_Table(sqlTYPE, "get");

            dp_TYPE.DataSource = dt;
            dp_TYPE.DataTextField = "DUTY_KINDNAME";
            dp_TYPE.DataValueField = "DUTY_KIND";
            dp_TYPE.DataBind();
                //!!
            
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {

        }

        public void search()
        {
            string strSQL = @"select CO.DUTY_KINDNAME, CC.* from C_ONDUTY_CODE  CC
                    LEFT JOIN   C_ONDUTY_CODE   CO ON substr(CC. DUTY_KIND,0,1)=CO.DUTY_KIND 
                    where CC.DUTY_TYPE='2'";
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "CODE");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}
