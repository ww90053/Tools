using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace TPPDDB._3_forleave
{
    public partial class C_ReviewManagement : System.Web.UI.Page
    {
      
        string C_strGID = "";

        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                C.check_power();
            

                C.set_Panel_EnterToTAB(ref this.Panel1);           
               
                //20141028
                //查詢下拉
                C.fill_AD_POST(DropDownList_MZ_EXAD);
                DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();               
                C.fill_unit(DropDownList_MZ_EXUNIT, DropDownList_MZ_EXAD.SelectedValue);
                DropDownList_MZ_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_MZ_EXUNIT.Items.Insert(0, new ListItem("", ""));
                //20141028

                //姓名查詢下拉
               
                C.fill_AD_POST(ddl_SearchName_MZ_EXAD);
                ddl_SearchName_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                C.fill_unit(ddl_SearchName_MZ_EXUNIT, ddl_SearchName_MZ_EXAD.SelectedValue);
                ddl_SearchName_MZ_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                ddl_SearchName_MZ_EXUNIT.Items.Insert(0, new ListItem("", ""));


                C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                C.controlEnable(ref this.Panel1, false);

                TextBox_MZ_ID.BackColor = Color.White;
                Label_MZ_EXUNIT.Text = "";
                Label_MZ_EXAD.Text = "";
                Label_OCCC.Text = "";
  
            }
        }

        protected void preLoad(string MZ_ID)
        {
            TextBox_MZ_ID.Text = MZ_ID;

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table("SELECT MZ_NAME,MZ_EXAD_CH ,MZ_EXUNIT_CH,MZ_OCCC_CH FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + MZ_ID + "' ", "get");


            if (dt.Rows.Count > 0)
            {
                TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString();
                Label_MZ_EXUNIT.Text = dt.Rows[0]["MZ_EXUNIT_CH"].ToString();
                Label_MZ_EXAD.Text = dt.Rows[0]["MZ_EXAD_CH"].ToString();
                Label_OCCC.Text = dt.Rows[0]["MZ_OCCC_CH"].ToString();
            
            }
          
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.Trim().ToUpper();
            preLoad(TextBox_MZ_ID.Text);

        }

        protected void Btsearch_Click(object sender, EventArgs e)
        {
            DropDownList_MZ_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
            
            btn_search_ModalPopupExtender.Show();
        
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["MODE"] = "INSERT";
         
            emptyMonitor(ref this.Panel1);

          
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            C.controlEnable(ref this.Panel1, true);
        }

        private void emptyMonitor(ref Panel Pl)
        {
            foreach (Object ob in Pl.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;
                    tbox.Text = string.Empty;
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;

                    ddlist.SelectedIndex = -1;
                }
                else if (ob is CheckBox)
                {
                    CheckBox cb = (CheckBox)ob;
                    cb.Checked = false;
                }
                else if (ob is Label)
                {
                    Label cb = (Label)ob;
                    cb.Text = string.Empty;
                }
            }

            Label2.Text = "差假審核資格管理";
        }


        protected void btOK_Click(object sender, EventArgs e)
        {
            string ErrorString = "";
            
            string strSQL = "";

            if (rbl.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇層級');", true);
                    return;
            }
            

            int CHECK_LEVEL = 0;

            CHECK_LEVEL = int.Parse(rbl.SelectedValue.ToString());

           

            string pkey_check;

           
            if (ViewState["MODE"].ToString() == "UPDATE")
                pkey_check = "0";

            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_REVIEW_MANAGEMENT WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND REVIEW_LEVEL=" + CHECK_LEVEL + "");

            if (pkey_check != "0")
            {
                ErrorString += "身分證號或審核資格違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.White;
            }


            string poccc = "";

            poccc = o_A_DLBASE.POCCC(TextBox_MZ_ID.Text);

            if (poccc == "")
            {
                poccc = "0";
            }

            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            string AD= o_A_DLBASE.PAD(TextBox_MZ_ID.Text);
            string UNIT = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text);


            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value =AD });//可能更改現職機關單位職稱.如果有調的話
            para.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value =UNIT  });
            para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = TextBox_MZ_ID.Text });
            para.Add(new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value = poccc });
            para.Add(new SqlParameter("REVIEW_LEVEL", SqlDbType.Char) { Value = CHECK_LEVEL });
                   

            try
            {
               string   strRegixSQL="";
                if (ViewState["MODE"].ToString() == "INSERT")
                {
                    strSQL = "INSERT INTO C_REVIEW_MANAGEMENT (SN,MZ_EXAD,MZ_EXUNIT,MZ_ID,MZ_OCCC,REVIEW_LEVEL) VALUES ( NEXT VALUE FOR dbo.c_review_management_sn,@MZ_EXAD,@MZ_EXUNIT,@MZ_ID,@MZ_OCCC,@REVIEW_LEVEL) ";
                    strRegixSQL = o_DBFactory.ABC_toTest.RegexSQL("INSERT", strSQL, para);

                }
                else if (ViewState["MODE"].ToString() == "UPDATE")
                {
                    strSQL = "UPDATE C_REVIEW_MANAGEMENT SET MZ_ID = @MZ_ID,MZ_EXUNIT=@MZ_EXUNIT,MZ_EXAD=@MZ_EXAD,MZ_OCCC=@MZ_OCCC,REVIEW_LEVEL=@REVIEW_LEVEL WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND REVIEW_LEVEL='" + CHECK_LEVEL + "'";
                    strRegixSQL = o_DBFactory.ABC_toTest.RegexSQL("UPDATE", strSQL, para);

                }

                o_DBFactory.ABC_toTest.SQLExecute(strSQL, para);
                                

                if (ViewState["MODE"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);

                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(),  strRegixSQL);

                    C.controlEnable(ref this.Panel1, false);


                }
                else if (ViewState["MODE"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);

                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(),  strRegixSQL);

                }

                DoSearch("", "", AD, UNIT);
                Label1.Text = "共" + lb_count.Text + "筆";

                btUpdate.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = true;
                ViewState.Remove("MODE");
                C.controlEnable(ref this.Panel1, false);
            }
            catch
            {
                if (ViewState["MODE"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (ViewState["MODE"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                }

            };

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM C_REVIEW_MANAGEMENT WHERE SN=@SN";

            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter("SN", SqlDbType.Float) { Value = int.Parse(GridView1.DataKeys[GridView1.SelectedIndex]["SN"].ToString()) });            
            string strRegixSQL = o_DBFactory.ABC_toTest.RegexSQL("DELETE", DeleteString, para);

            try
            {

                o_DBFactory.ABC_toTest.SQLExecute(DeleteString, para);


                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), strRegixSQL);
                string AD = o_A_DLBASE.PAD(TextBox_MZ_ID.Text);
                string UNIT = o_A_DLBASE.PUNIT(TextBox_MZ_ID.Text);
                DoSearch("", "", AD, UNIT);
                Label1.Text = "共" + lb_count.Text + "筆";


            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }



        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
            Label_MZ_EXAD.Text = string.Empty;
            Label_MZ_EXUNIT.Text = string.Empty;
            Label_OCCC.Text = string.Empty;
            //20140128
            rbl.SelectedIndex = -1;
            
           
            TextBox_MZ_ID.BackColor = Color.White;

           
            btInsert.Enabled = true;
            btOK.Enabled = false;
            btCancel.Enabled = false;

            C.controlEnable(ref this.Panel1, false);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
          
            ViewState["MODE"] = "UPDATE";
            
            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            C.controlEnable(ref this.Panel1, true);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECT")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);


                rbl.SelectedValue = GridView1.DataKeys[GridView1.SelectedIndex]["REVIEW_LEVEL"].ToString();

                TextBox_MZ_ID.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[0].Text;
                TextBox_MZ_NAME.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[1].Text;
                Label_MZ_EXAD.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text;
                Label_MZ_EXUNIT.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[3].Text;
               
                Label_OCCC.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[4].Text;
               
            
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;

                Label1.Text = "第" + ((GridView1.PageIndex *10)+ (GridView1.SelectedIndex + 1)) + "筆共" + lb_count.Text + "筆";

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[GridView1.Columns.Count - 1].Attributes.Add("Style", "display:none");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              
                switch (e.Row.Cells[5].Text)
                {
                    case "1":
                        e.Row.Cells[5].Text = "單位承辦人";
                        break;
                    case "2":
                        e.Row.Cells[5].Text = "單位主管";
                        break;
                    case "3":
                        e.Row.Cells[5].Text = "值日官";
                        break;
                    case "4":
                        e.Row.Cells[5].Text = "差勤承辦人";
                        break;
                    case "5":
                        e.Row.Cells[5].Text = "加班承辦人";

                        break;
                }

            }
        }


        //TODO最好做分頁查詢
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.SelectedIndex =-1;
            DoSearch(txt_search_ID.Text, txt_search_NAME.Text, DropDownList_MZ_EXAD.SelectedValue, DropDownList_MZ_EXUNIT.SelectedValue);
            Label1.Text = "共" + lb_count.Text + "筆";
          
        }


        protected void btGroup_Update_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ReviewManagement_GROUPUPDATE.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=550,height=300,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {          
            pl_SearchName_ModalPopupExtender.Show();
        }

        

        protected void ck_isTopManager_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;

            GridView gv = (GridView)gvr.NamingContainer;

            int index = gvr.RowIndex;

            CheckBox ck1 = (CheckBox)gv.Rows[index].Cells[7].FindControl("ck_isTopManager");

            if (ck1.Checked)
            {
                o_DBFactory.ABC_toTest.Edit_Data("UPDATE C_REVIEW_MANAGEMENT SET TOP_MANAGER='Y' WHERE SN=" + gv.DataKeys[index]["SN"].ToString());
            }
            else
            {
                o_DBFactory.ABC_toTest.Edit_Data("UPDATE C_REVIEW_MANAGEMENT SET TOP_MANAGER=null WHERE SN=" + gv.DataKeys[index]["SN"].ToString());
            }
        }

        #region 查詢panel

        //20141028
        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_MZ_EXUNIT, DropDownList_MZ_EXAD.SelectedValue);
            DropDownList_MZ_EXUNIT.Items.Insert(0,new ListItem("",""));
            btn_search_ModalPopupExtender.Show();
        }

        protected void txt_search_OK_Click(object sender, EventArgs e)
        {

            DoSearch(txt_search_ID.Text,txt_search_NAME.Text,DropDownList_MZ_EXAD.SelectedValue,DropDownList_MZ_EXUNIT.SelectedValue);
            Label1.Text = "共" + lb_count.Text + "筆";
            btn_search_ModalPopupExtender.Hide();
        }




        protected void txt_search_cancel_Click(object sender, EventArgs e)
        {
            btn_search_ModalPopupExtender.Hide();
        }

        public void DoSearch(string MZ_ID,string MZ_NAME, string MZ_EXAD, string MZ_EXUNIT)
        {
            //TODO之後沒問題看要不要把代碼欄未清掉
            string GVSQL = @"SELECT TOP_MANAGER,SN,CC.MZ_ID, MZ_NAME  AS NAME,CC.MZ_EXAD ,AKED.MZ_KCHI  MZ_EXAD_CH ,CC.MZ_EXUNIT, AKEU.MZ_KCHI  MZ_EXUNIT_CH,CC.MZ_OCCC,AKO.MZ_KCHI  MZ_OCCC_CH,REVIEW_LEVEL 
                        FROM C_REVIEW_MANAGEMENT  CC
                        LEFT JOIN A_DLBASE ON A_DLBASE.MZ_ID=CC.MZ_ID
                        LEFT JOIN  A_KTYPE AKED ON AKED.MZ_KCODE=CC.MZ_EXAD AND AKED.MZ_KTYPE='04'
                        LEFT JOIN  A_KTYPE AKEU ON AKEU.MZ_KCODE=CC.MZ_EXUNIT AND AKEU.MZ_KTYPE='25'
                        LEFT JOIN  A_KTYPE AKO ON AKO.MZ_KCODE=CC.MZ_OCCC AND AKO.MZ_KTYPE='26'
                        WHERE 1=1";

            string where = "";

            if (!string.IsNullOrEmpty(MZ_ID))
            {
                where += " AND CC.MZ_ID='" + MZ_ID + "' ";

            }

            if (!string.IsNullOrEmpty(txt_search_NAME.Text))
            {
                where += " AND A_DLBASE.MZ_NAME ='" + MZ_NAME.Trim() + "' ";

            }

            if (!string.IsNullOrEmpty(MZ_EXAD))
            {
                where += "  AND CC.MZ_EXAD = '" + MZ_EXAD.Trim() + "' ";

            }

            where += "  AND CC.MZ_EXAD = '" + DropDownList_MZ_EXAD.SelectedValue.Trim() + "' ";

            if ((!string.IsNullOrEmpty(MZ_EXUNIT)))
            {
                where += " AND CC.MZ_EXUNIT='" + MZ_EXUNIT.Trim() + "'";

            }

            where += " ORDER BY CC.MZ_ID";
            GVSQL += where;

            DataTable gvDT = new DataTable();
            gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE");            
            GridView1.DataSource = gvDT;           
            GridView1.DataBind();
            lb_count.Text = gvDT.Rows.Count.ToString();
        }
        #endregion 查詢panel

        #region 姓名查詢

        protected void ddl_SearchName_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(ddl_SearchName_MZ_EXUNIT, ddl_SearchName_MZ_EXAD.SelectedValue);
            ddl_SearchName_MZ_EXUNIT.Items.Insert(0, new ListItem("", ""));
            pl_SearchName_ModalPopupExtender.Show();
        }


        protected void btn_SearchName_OK_Click(object sender, EventArgs e)
        {
            GetValue();
            pl_SearchName_ModalPopupExtender.Show();

        }

        protected void btn_SearchName_cancel_Click(object sender, EventArgs e)
        {
            pl_SearchName_ModalPopupExtender.Hide();
            //是否需清掉GridView2
        }



    


        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = int.Parse(e.CommandArgument.ToString());

                TextBox_MZ_ID.Text = GridView2.Rows[GridView1.SelectedIndex].Cells[1].Text;
                TextBox_MZ_NAME.Text = GridView2.Rows[GridView1.SelectedIndex].Cells[2].Text;
                Label_MZ_EXAD.Text = GridView2.Rows[GridView1.SelectedIndex].Cells[3].Text;
                Label_MZ_EXUNIT.Text = GridView2.Rows[GridView1.SelectedIndex].Cells[4].Text;
                //Label_OCCC.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[5].Text;
            }
            pl_SearchName_ModalPopupExtender.Hide();
            //是否需清掉GridView2
        }

        //TODO最好做分頁查詢
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            GetValue();
            pl_SearchName_ModalPopupExtender.Show();
        }

       
        protected void GetValue()
        {
           
            string Selectstring = @"SELECT MZ_NAME,MZ_ID, AKD.MZ_KCHI MZ_EXAD ,  AKU.MZ_KCHI MZ_EXUNIT ,  AKO.MZ_KCHI MZ_OCCC 
                                    FROM A_DLBASE A 
                                    LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A.MZ_EXAD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                    LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A.MZ_EXUNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                                    LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                    WHERE 1=1";
            if (!string.IsNullOrEmpty(txt_search_NAME.Text))
            {
                Selectstring += " AND RTRIM(MZ_NAME) LIKE '" + txt_search_NAME.Text.Trim() + "%'";
            }

            if (!string.IsNullOrEmpty(DropDownList_MZ_EXAD.SelectedValue))
            {
                Selectstring += " AND MZ_EXAD='" + ddl_SearchName_MZ_EXAD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(ddl_SearchName_MZ_EXUNIT.SelectedValue))
            {
                Selectstring += " AND MZ_EXUNIT='" + ddl_SearchName_MZ_EXUNIT.SelectedValue + "'";
            }

          
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(Selectstring, "123");

            GridView2.DataSource = dt;
            GridView2.DataBind();
        }

        #endregion 姓名查詢

    }
}
