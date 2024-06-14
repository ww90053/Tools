using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace TPPDDB._1_personnel
{
    public partial class Personal_POWERSET : System.Web.UI.Page
    {
        List<String> POWER_ID = new List<string>();
        string A_strGID = "";

        string TPFID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPFID FROM TPF_FIONDATA WHERE TPFNAME='人事管理'");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
            ///群組權限
            A_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            //查詢ID



            //HttpCookie POWER_ID_Cookie = new HttpCookie("PersonalSearch_ID");
            //POWER_ID_Cookie = Request.Cookies["PersonalSearch_ID"];

            //if (POWER_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID"] = null;
            //    Response.Cookies["PersonalSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID"] = TPMPermissions._strDecod(POWER_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            ////查詢姓名
            //HttpCookie POWER_NAME_Cookie = new HttpCookie("PersonalSearch_NAME");
            //POWER_NAME_Cookie = Request.Cookies["PersonalSearch_NAME"];

            //if (POWER_NAME_Cookie == null)
            //{
            //    ViewState["MZ_NAME"] = null;
            //    Response.Cookies["PersonalSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_NAME"] = TPMPermissions._strDecod(POWER_NAME_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}

            ViewState["MZ_AD"] = Request["MZ_AD"];//查詢機關
            ViewState["MZ_UNIT"] = Request["MZ_UNIT"];//查詢單位
            ViewState["MZ_POWER"] = Request["MZ_POWER"];

            ViewState["MZ_ID"] = Session["PersonalSearch_ID"];
            Session.Remove("PersonalSearch_ID");

            ViewState["MZ_NAME"] = Session["PersonalSearch_NAME"];
            Session.Remove("PersonalSearch_NAME");

            //by MQ 20100315---------   
            A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel3);



            if (!IsPostBack)
            {
                Label_MZ_AD.Text = "&nbsp;";
                Label_MZ_EXAD.Text = "&nbsp;";


                string select_GID = "SELECT TPMN_DATANAME+'('+TPG_DATANAME+')' AS GID_NAME,TPMN_GID FROM TP_MODEL_NAME Inner Join TP_GROUP_DATA ON TP_GROUP_DATA.TPG_GID = TP_MODEL_NAME.TPG_GID  WHERE TPMN_TPFID = '" + TPFID + "'";

                if (A_strGID == "B")
                {
                    select_GID += " AND TPG_DATANAME!='A'";
                }
                else if (A_strGID == "C")
                {
                    select_GID += " AND TPG_DATANAME!='A' AND TPG_DATANAME!='B'";
                }

                select_GID += " ORDER BY TPMN_GID";

                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(select_GID, "getgid");

                DropDownList1.DataSource = temp;

                DropDownList1.DataTextField = "GID_NAME";
                DropDownList1.DataValueField = "TPMN_GID";
                DropDownList1.DataBind();

                if (ViewState["MZ_ID"] != null)
                {


                    string strSQL = @"SELECT AKO.MZ_KCHI  MZ_OCCC,
                                        MZ_NAME,MZ_ID,MZ_POWER,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV ,substr( MZ_ID,0,2  )+'XXXXX' +substr( MZ_ID,8,3  ) MZ_ID_Hide 
                                        FROM A_DLBASE   
                                        LEFT JOIN  A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26'
                                        WHERE 1=1 ";
                    //string strSQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC,MZ_NAME,MZ_ID,MZ_POWER,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV ,substr( MZ_ID,0,2  )+'XXXXX' +substr( MZ_ID,8,3  ) MZ_ID_Hide FROM A_DLBASE  WHERE 1=1";
                    //2013/09/03 隱碼身分證字號
                    if (string.IsNullOrEmpty(ViewState["MZ_ID"].ToString()) == false)
                    {
                        strSQL = strSQL + " AND MZ_ID LIKE'" + ViewState["MZ_ID"].ToString() + "%'";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_NAME LIKE '" + ViewState["MZ_NAME"].ToString() + "%'";
                    }

                    if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_EXAD ='" + ViewState["MZ_AD"].ToString() + "'";
                    }

                    if (ViewState["MZ_UNIT"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_EXUNIT='" + ViewState["MZ_UNIT"].ToString() + "'";
                    }
                    if (ViewState["MZ_POWER"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_POWER='" + ViewState["MZ_POWER"].ToString() + "'";
                    }
                    strSQL = strSQL + " AND MZ_STATUS2 ='Y'  ORDER BY MZ_EXAD,MZ_EXUNIT,TBDV ";
                    ViewState["POWERSETSQL"] = strSQL;

                    POWER_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");//塞查詢相關身分證號到list
                    Session["POWER_ID"] = POWER_ID;//跨網頁傳質問題！故先用session接值


                    DataTable gridview_dt = new DataTable();
                    gridview_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "getdata");

                    //Session["POWERSET_GRIDVIEWDT"] = gridview_dt;
                    GridView1.DataSource = gridview_dt;
                    GridView1.AllowPaging = true;
                    GridView1.PageSize = 10;
                    GridView1.DataBind();

                    if (TPMPermissions._boolAPA_ADMIN(1911))
                    {
                        //GridView1.Columns["權限"].Visible = false;
                        //GridView1.Columns["身分證號"].Visible = false;
                        GridView1.Columns[2].Visible = false;
                        GridView1.Columns[3].Visible = false;
                    }

                    if (gridview_dt.Rows.Count > 0)
                    {
                        GridView1.Visible = true;
                    }


                    if (POWER_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (POWER_ID.Count == 1)
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
                }
            }
        }

        protected void finddata(int dataCount)
        {
            POWER_ID = Session["POWER_ID"] as List<string>;

            string selectString = "SELECT * FROM A_DLBASE WHERE MZ_ID='" + POWER_ID[dataCount] + "'";

            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(selectString, "GETDATA");

            if (temp.Rows.Count == 1)
            {

                DropDownList1.SelectedValue = check_TMP_GROUP_PERMISSION(temp.Rows[0]["MZ_ID"].ToString());

                preLoad(POWER_ID[dataCount], 2);

                if (temp.Rows[0]["MZ_POWER"].ToString() != "")
                {
                    DropDownList_MZ_POWER.SelectedValue = temp.Rows[0]["MZ_POWER"].ToString();
                }
                else
                {
                    DropDownList_MZ_POWER.SelectedValue = "D";
                }
            }

            //by MQ 20100315
            moveGV_IndexMark();

            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + POWER_ID.Count.ToString() + "筆";
        }

        protected void preLoad(string MZ_ID, int MODE)
        {
            //預帶登入者相關資料（姓名 身分證號.....）

            TextBox_MZ_ID.Text = MZ_ID;

            //20140120
            //如果有建立人事代碼對應的View .這段可以修

            string strsql = @"SELECT MZ_NAME ,MZ_AD_CH,MZ_UNIT_CH,MZ_EXAD_CH, MZ_EXUNIT_CH,  MZ_OCCC_CH
                              FROM VW_A_DLBASE_S1  
                              
                              WHERE MZ_ID='" + MZ_ID + "'";

            DataTable person_data = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");

            if (person_data.Rows.Count > 0)
            {
                TextBox_MZ_NAME.Text = person_data.Rows[0]["MZ_NAME"].ToString();

                Label_MZ_AD.Text = person_data.Rows[0]["MZ_AD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_UNIT_CH"].ToString();

                Label_MZ_EXAD.Text = person_data.Rows[0]["MZ_EXAD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_EXUNIT_CH"].ToString() + "     " + person_data.Rows[0]["MZ_OCCC_CH"].ToString();
            }

            //20140317
            //if (MODE == 1 && !string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'")))
            if (MODE == 1 && person_data.Rows.Count > 0)
            {
                POWER_ID = Session["POWER_ID"] as List<string>;

                int d = POWER_ID.IndexOf(MZ_ID);
                xcount.Text = d.ToString();
                finddata(d);
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
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
                string updateString = "UPDATE A_DLBASE SET MZ_POWER='" + DropDownList_MZ_POWER.SelectedValue + "' WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(updateString);


                    string check_perssion = check_TMP_GROUP_PERMISSION(TextBox_MZ_ID.Text);

                    string TPMID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMID FROM TPM_MEMBER WHERE TPM_IDNO='" + TextBox_MZ_ID.Text + "'");

                    if (string.IsNullOrEmpty(TPMID))
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此人無權限帳號，請先新增權限帳號！');", true);
                        return;
                    }

                    if (!string.IsNullOrEmpty(check_perssion))
                    {

                        if (check_perssion == DropDownList1.SelectedValue)
                        {
                        }
                        else
                        {
                            string GROUP_KIND = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMM_GID FROM TP_MODEL_MEMBER WHERE TPMID='" + TPMID + "' AND TPMN_GID='" + check_perssion + "'");
                            string updatestring1 = "UPDATE TP_MODEL_MEMBER SET TPMN_GID='" + DropDownList1.SelectedValue + "' WHERE TPMM_GID='" + GROUP_KIND + "'";
                            o_DBFactory.ABC_toTest.Edit_Data(updatestring1);
                        }
                    }
                    else
                    {
                        string insertString = "Insert Into TP_MODEL_MEMBER (TPMM_GID, TPMN_GID, TPMID) "
                                            + "Values ( NEXT VALUE FOR dbo.SEQ_TP_GROUP, '" + DropDownList1.SelectedValue + "', '" + TPMID + "')";

                        o_DBFactory.ABC_toTest.Edit_Data(insertString);
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);

                    if (GridView1.Visible)
                    {
                        DataTable gridview_dt = new DataTable();

                        gridview_dt = o_DBFactory.ABC_toTest.Create_Table(ViewState["POWERSETSQL"].ToString(), "DATAGET");

                        if (gridview_dt.Rows.Count > 0)
                        {
                            GridView1.Visible = true;

                            //Session["POWERSET_GRIDVIEWDT"] = gridview_dt;
                            GridView1.DataSource = gridview_dt;
                            GridView1.AllowPaging = true;
                            GridView1.PageSize = 10;
                            GridView1.DataBind();
                        }
                    }
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                }
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch.aspx?TableName=POWER&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.Trim().ToUpper();

            preLoad(TextBox_MZ_ID.Text.Trim().ToUpper(), 1);

            GridView1.Visible = false;
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != POWER_ID.Count - 1)
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
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == POWER_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == POWER_ID.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable gridview_dt = new DataTable();

            //2013//11/22 
            gridview_dt = o_DBFactory.ABC_toTest.Create_Table(ViewState["POWERSETSQL"].ToString(), "get");
            //gridview_dt = Session["POWERSET_GRIDVIEWDT"] as DataTable;

            GridView1.DataSource = gridview_dt;
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();

            GridView1.SelectedIndex = -1;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                //2013/09/06 不至到隱藏什麼.依序退後
                // e.Row.Cells[4].Attributes.Add("Style", "display:none");
                e.Row.Cells[5].Attributes.Add("Style", "display:none");
            }


        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                //by MQ 20100315
                int a = (GridView1.PageIndex) * (GridView1.PageSize) + Convert.ToInt32(e.CommandArgument);
                xcount.Text = a.ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == 0)
                {
                    btUpper.Enabled = false;
                }
                else
                {
                    btUpper.Enabled = true;
                }
                if (int.Parse(xcount.Text) == POWER_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
                else
                {
                    btNEXT.Enabled = true;
                }
            }
        }

        protected DataTable GROUP_PERMISSION_KIND()
        {
            string strSQL = "SELECT TPMN_GID, TPMN_TPFID,  TPMN_DATANAME , TPG_DATANAME FROM TP_MODEL_NAME Inner Join TP_GROUP_DATA ON TP_GROUP_DATA.TPG_GID = TP_MODEL_NAME.TPG_GID  WHERE TPMN_TPFID = '" + TPFID + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE_TEMP");

            return temp;
        }

        protected string check_TMP_GROUP_PERMISSION(string MZ_ID)
        {
            DataTable TPMN_GID_DT = GROUP_PERMISSION_KIND();

            string TPMID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMID FROM TPM_MEMBER WHERE TPM_IDNO='" + MZ_ID + "'");

            string TPMN_GID = "";

            for (int i = 0; i < TPMN_GID_DT.Rows.Count; i++)
            {
                TPMN_GID = o_DBFactory.ABC_toTest.vExecSQL("SELECT TPMN_GID FROM TP_MODEL_MEMBER WHERE TPMID='" + TPMID + "' AND TPMN_GID='" + TPMN_GID_DT.Rows[i]["TPMN_GID"].ToString() + "'");
                if (!string.IsNullOrEmpty(TPMN_GID))
                    break;
            }

            return TPMN_GID;
        }

        protected void moveGV_IndexMark()
        {
            //by MQ 20100315

            int n = int.Parse(xcount.Text);

            DataTable PAGE_DT = new DataTable();
            PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(ViewState["POWERSETSQL"].ToString(), "get");
            //PAGE_DT = Session["POWERSET_GRIDVIEWDT"] as DataTable;

            GridView1.DataSource = PAGE_DT;

            GridView1.PageIndex = n / GridView1.PageSize;

            if ((n + 1) <= GridView1.PageSize)
            {
                GridView1.SelectedIndex = n;
            }
            else
            {
                GridView1.SelectedIndex = n % GridView1.PageSize;
            }

            GridView1.DataBind();
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList1.Items.Insert(0, li);
        }
    }
}
