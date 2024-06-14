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
    public partial class Personal_Education_temp : System.Web.UI.Page
    {
        List<String> MZ_ID = new List<string>();
        List<String> MZ_SCHOOL = new List<string>();
        List<String> MZ_DEPARTMENT = new List<string>();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    string strSQL = "SELECT * FROM A_EDUCATION_TEMP WHERE 1=1";

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
                    MZ_SCHOOL = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_SCHOOL");
                    MZ_DEPARTMENT = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_DEPARTMENT");


                    Session["ED_T_MZ_ID"] = MZ_ID;
                    Session["ED_T_MZ_SCHOOL"] = MZ_SCHOOL;
                    Session["ED_T_MZ_DEPARTMENT"] = MZ_DEPARTMENT;

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

                    GRIDVIEW_DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE3");

                    Session["ED_T_GD_DT"] = GRIDVIEW_DT;

                    GridView1.DataSource = GRIDVIEW_DT;

                    GridView1.AllowPaging = true;

                    GridView1.PageSize = 10;

                    GridView1.DataBind();
                }
            }
        }

        protected void finddata(int DataCount)
        {
            MZ_ID = Session["ED_T_MZ_ID"] as List<string>;
            MZ_SCHOOL = Session["ED_T_MZ_SCHOOL"] as List<string>;
            MZ_DEPARTMENT = Session["ED_T_MZ_DEPARTMENT"] as List<string>;

            string strSQL = "SELECT * FROM A_EDUCATION_TEMP WHERE 1=1 ";

            if (!string.IsNullOrEmpty(MZ_ID[DataCount]))
            {
                strSQL += " AND MZ_ID='" + MZ_ID[DataCount] + "'";
            }
            if (!string.IsNullOrEmpty(MZ_SCHOOL[DataCount]))
            {
                strSQL += " AND MZ_SCHOOL='" + MZ_SCHOOL[DataCount] + "'";
            }

            if (!string.IsNullOrEmpty(MZ_DEPARTMENT[DataCount]))
            {
                strSQL += " AND MZ_DEPARTMENT='" + MZ_DEPARTMENT[DataCount] + "'";
            }

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (tempDT.Rows.Count > 0)
            {
                TextBox_MZ_ID.Text = tempDT.Rows[0]["MZ_ID"].ToString();
                Label_MZ_NAME.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_SCHOOL.Text = tempDT.Rows[0]["MZ_SCHOOL"].ToString();
                Label_MZ_SCHOOL.Text = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[0]["MZ_SCHOOL"].ToString(), "ORG");
                TextBox_MZ_DEPARTMENT.Text = tempDT.Rows[0]["MZ_DEPARTMENT"].ToString();
                Label_MZ_DEPARTMENT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE='" + tempDT.Rows[0]["MZ_DEPARTMENT"].ToString() + "' AND (MZ_KTYPE='DP1' OR MZ_KTYPE='DP2' OR MZ_KTYPE='DP3')");
                TextBox_MZ_YEAR.Text = tempDT.Rows[0]["MZ_YEAR"].ToString();
                TextBox_MZ_EDUKIND.Text = tempDT.Rows[0]["MZ_EDUKIND"].ToString();
                Label_MZ_EDUKIND.Text = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[0]["MZ_EDUKIND"].ToString(), "EDT");
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch.aspx?TableName=EDUCATION_TEMP&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
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
            string strSQL = "SELECT * FROM A_EDUCATION_TEMP";

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                string repeat = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_EDUCATION WHERE MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "' AND MZ_SCHOOL='" + tempDT.Rows[i]["MZ_SCHOOL"].ToString() + "' AND MZ_DEPARTMENT='" + tempDT.Rows[i]["MZ_DEPARTMENT"].ToString() + "'");

                if (repeat == "0")
                {
                    string InsertSQL = "INSERT INTO A_EDUCATION(MZ_ID,MZ_SCHOOL,MZ_DEPARTMENT,MZ_YEAR,MZ_EDUKIND) VALUES('" + tempDT.Rows[i]["MZ_ID"].ToString() + "','" + tempDT.Rows[i]["MZ_SCHOOL"].ToString() + "','" + tempDT.Rows[i]["MZ_DEPARTMENT"].ToString() + "','" + tempDT.Rows[i]["MZ_YEAR"].ToString() + "','" + tempDT.Rows[i]["MZ_EDUKIND"].ToString() + "')";
                 
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
                }

                if (i == tempDT.Rows.Count - 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('匯入完成！')", true);
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable tempDT = Session["ED_T_GD_DT"] as DataTable;

            GridView1.PageIndex = e.NewPageIndex;

            GridView1.DataSource = tempDT;

            GridView1.DataBind();
        }
    }
}
