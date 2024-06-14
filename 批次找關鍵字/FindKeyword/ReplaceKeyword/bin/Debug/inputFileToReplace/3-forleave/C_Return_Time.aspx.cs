using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._3_forleave
{
    public partial class C_Return_Time : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                pageInit(ddlSAd);
                chk_TPMGroup();
            }
        }
                
        private void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                    
                    break;
                case "B":
                case "C":
                    ddlSAd.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    ddlSAd.Enabled = false;
                    break;
                                   
                case "D":                  
                case "E":
                    ddlSAd.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    ddlSAd.Enabled = false;
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }


        protected void ddlSAd_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageInit(ddlSAd);
           
        }


        private void pageInit(DropDownList ddl)
        {
            string btnSelected = ddl.SelectedValue;

            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' ";

            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            ddl.DataSource = temp;
            ddl.DataTextField = "MZ_KCHI";
            ddl.DataValueField = "MZ_KCODE";
            ddl.DataBind();


            if (temp.Rows.Count > 0)
            {
                foreach (ListItem li in ddl.Items)
                {
                    if (li.Text != "新北市政府警察局")
                        li.Text = li.Text.Replace("新北市政府警察局", string.Empty);
                }

                ddl.Items.Insert(0, "請選擇");

            }

            if (btnSelected != "0"  && btnSelected != "")
            {
                ddl.SelectedValue = btnSelected;
            }
            else
            {
                ddl.SelectedValue = Session["ADPMZ_EXAD"].ToString();
            }

        }


        protected void btSearch_Click(object sender, EventArgs e)
        {
            doSearch();
        }

        void doSearch()
        {
            string strSQL = string.Format("SELECT * FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_DATE='{0}' AND MZ_ID='{1}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_EXAD='{2}'  ", TextBox_MZ_DATE.Text, TextBox_MZ_ID.Text, ddlSAd.SelectedValue);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            DataTable gv_dt = new DataTable();

            gv_dt.Columns.Add("MZ_IDATE1", typeof(string));
            gv_dt.Columns.Add("MZ_HOUR", typeof(string));

            if (dt.Rows.Count > 0)
            {
                hdf_OTIME.Value = dt.Rows[0]["OTIME"].ToString();
                hdf_MZ_RESTDATE.Value = dt.Rows[0]["MZ_RESTDATE"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["MZ_RESTDATE"].ToString()))
                {
                    string[] s = dt.Rows[0]["MZ_RESTDATE"].ToString().Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < s.Count(); j++)
                    {
                        string[] y = s[j].Split('：');

                        DataRow gv_dr = gv_dt.NewRow();

                        gv_dr["MZ_IDATE1"] = y[0];
                        gv_dr["MZ_HOUR"] = y[1];

                        gv_dt.Rows.Add(gv_dr);
                    }
                }
            }

            if (gv_dt.Rows.Count > 0)
            {
                ViewState["gv_dt"] = gv_dt;
                gv_data.DataSource = gv_dt;
                gv_data.DataBind();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無此日期相關資料！');", true);
                gv_data.DataBind();
                return;
            }
        }

        protected void TextBox_MZ_HOUR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int index = gvr.RowIndex;

            TextBox txt_hour = (TextBox)gv.Rows[index].Cells[1].FindControl("TextBox_MZ_HOUR");

            int hour;

            int.TryParse(txt_hour.Text, out hour);

            DataTable dt = ViewState["gv_dt"] as DataTable;

            if (hour < 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('時數錯誤！');", true);
                return;
            }
            else if (hour == 0)
            {
                dt.Rows.Remove(dt.Rows[index]);
            }
            else
            {
                dt.Rows[index][1] = txt_hour.Text;
            }

            update_restdata(dt);

        }

        private void update_restdata(DataTable dt)
        {
            string MZ_RESTDATE = "";

            int RESTHOUR = 0;

            int parHour;

            int OTIME;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int.TryParse(dt.Rows[i][1].ToString(), out parHour);

                RESTHOUR += parHour;

                if (i == 0)
                    MZ_RESTDATE += dt.Rows[i][0].ToString() + "：" + dt.Rows[i][1].ToString();
                else
                    MZ_RESTDATE += "，" + dt.Rows[i][0].ToString() + "：" + dt.Rows[i][1].ToString();

            }

            int.TryParse(hdf_OTIME.Value, out OTIME);

            string strSQL = string.Format("UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_RESTDATE='{2}',MZ_RESTHOUR={3} WHERE MZ_DATE='{0}' AND MZ_ID='{1}' AND dbo.SUBSTR(RESTFLAG,1,1)='Y' ", TextBox_MZ_DATE.Text, TextBox_MZ_ID.Text, MZ_RESTDATE, (OTIME - RESTHOUR < 0 ? 0 : OTIME - RESTHOUR));

            //Log
            LogModel.saveLog("COHI", "U", strSQL, new List<System.Data.SqlClient.SqlParameter>(), Request.QueryString["TPM_FION"], "補休時數回復，更新可補休日");

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            doSearch();
        }

        protected void gv_data_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Del":
                    int index = Convert.ToInt32(e.CommandArgument);

                    DataTable dt = ViewState["gv_dt"] as DataTable;

                    dt.Rows.Remove(dt.Rows[index]);

                    update_restdata(dt);

                    break;
            }
        }
    }
}
