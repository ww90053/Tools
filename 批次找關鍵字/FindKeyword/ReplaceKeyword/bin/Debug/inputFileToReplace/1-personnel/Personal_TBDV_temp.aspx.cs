using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web.Configuration;

namespace TPPDDB._1_personnel
{
    public partial class Personal_TBDV_temp : System.Web.UI.Page
    {
       

        List<String> MZ_ID = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
          

            //查詢ID
            HttpCookie DLBASE_ID_Cookie = new HttpCookie("PersonalSearch_ID");
            DLBASE_ID_Cookie = Request.Cookies["PersonalSearch_ID"];

            if (DLBASE_ID_Cookie == null)
            {
                ViewState["MZ_ID"] = null;
                Response.Cookies["PersonalSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_ID"] = TPMPermissions._strDecod(DLBASE_ID_Cookie.Value.ToString());
                Response.Cookies["PersonalSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            //查詢姓名
            HttpCookie DLBASE_NAME_Cookie = new HttpCookie("PersonalSearch_NAME");
            DLBASE_NAME_Cookie = Request.Cookies["PersonalSearch_NAME"];

            if (DLBASE_NAME_Cookie == null)
            {
                ViewState["MZ_NAME"] = null;
                Response.Cookies["PersonalSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_NAME"] = TPMPermissions._strDecod(DLBASE_NAME_Cookie.Value.ToString());
                Response.Cookies["PersonalSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }

            ViewState["AD"] = Request["MZ_AD"];//查詢機關
            ViewState["UNIT"] = Request["MZ_UNIT"];//查詢單位

            if (!Page.IsPostBack)
            {
                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL = "SELECT * FROM A_TBDV_TEMP WHERE 1=1";

                    if (string.IsNullOrEmpty(ViewState["MZ_ID"].ToString()) == false)
                    {
                        strSQL += " AND MZ_ID LIKE'" + ViewState["MZ_ID"].ToString() + "%'";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '" + ViewState["MZ_NAME"].ToString() + "%') ";
                    }

                    if (ViewState["AD"].ToString() != "" && ViewState["UNIT"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD ='" + ViewState["AD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["UNIT"].ToString() + "')";
                    }
                    else if (ViewState["AD"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE  (MZ_EXAD='" + ViewState["AD"].ToString() + "' OR MZ_AD='" + ViewState["AD"].ToString() + "' OR PAY_AD='" + ViewState["AD"].ToString() + "'))";
                    }


                    MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");

                    Session["TBDV_MZ_ID"] = MZ_ID;


                    if (MZ_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (MZ_ID.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    //共幾筆資料
                    if (MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + MZ_ID.Count.ToString() + "筆";
                    }

                    DataTable GRIDVIEW_DT = new DataTable();

                    GRIDVIEW_DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE4");

                    Session["TBDV_MZ_ID"] = GRIDVIEW_DT;

                    GridView1.DataSource = GRIDVIEW_DT;

                    GridView1.AllowPaging = true;

                    GridView1.PageSize = 10;

                    GridView1.DataBind();
                }
            }
        }

        protected void finddata(int DataCount)
        {
            MZ_ID = Session["TBDV_MZ_ID"] as List<string>;


            string strSQL = "SELECT * FROM A_TBDV_TEMP WHERE 1=1 ";

            if (!string.IsNullOrEmpty(MZ_ID[DataCount]))
            {
                strSQL += " AND MZ_ID='" + MZ_ID[DataCount] + "'";
            }

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (tempDT.Rows.Count > 0)
            {
                TextBox_MZ_ID.Text = tempDT.Rows[0]["MZ_ID"].ToString();
                Label_MZ_NAME.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_TBDV.Text = tempDT.Rows[0]["MZ_TBDV"].ToString();
                Label_MZ_TBDV.Text = o_A_KTYPE.CODE_TO_NAME(TextBox_MZ_TBDV.Text, "43");
            }
        }


        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch.aspx?TableName=TBDV_TEMP&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) != MZ_ID.Count - 1)
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
                finddata(int.Parse(xcount.Text));

                btUpper.Enabled = false;
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + MZ_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) == MZ_ID.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + MZ_ID.Count.ToString() + "筆";
        }

        protected void btInsert_all_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT * FROM A_TBDV_TEMP";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {

                string InsertSQL = "UPDATE A_DLBASE SET MZ_TBDV='" + tempDT.Rows[i]["MZ_TBDV"].ToString() + "' WHERE MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'";

                SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(InsertSQL, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    //XX2013/06/18 
                    //conn.Close();
                }
                conn.Close();
                //XX2013/06/18 
                conn.Dispose();


                if (i == tempDT.Rows.Count - 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('匯入完成！')", true);
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable tempDT = Session["TBDV_MZ_ID"] as DataTable;

            GridView1.PageIndex = e.NewPageIndex;

            GridView1.DataSource = tempDT;

            GridView1.DataBind();
        }
    }
}
