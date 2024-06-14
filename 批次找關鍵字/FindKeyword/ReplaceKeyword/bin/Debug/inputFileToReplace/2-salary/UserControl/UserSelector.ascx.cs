using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary.UserControl
{
    public partial class UserSelector : System.Web.UI.UserControl
    {
        public int PageIndex
        {
            set { gv_Target.PageIndex = value; }
        }
        DataTable dtSource
        {
            set { ViewState["dtSource"] = value; }
            get { return (DataTable)ViewState["dtSource"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {

        }

        public void SetData(DataTable dt)
        {
            dtSource = dt;
            gv_Target.DataSource = dtSource;
            gv_Target.DataBind();
        }

        public event GridViewCommandEventHandler UserSelectd;
        protected void gv_Target_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnSelect")
            {
                if (UserSelectd != null)
                    UserSelectd(sender, e);
            }
        }

        public event GridViewPageEventHandler PageIndexChanging;
        protected void gv_Target_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Target.PageIndex = e.NewPageIndex;
            gv_Target.DataSource = dtSource;
            gv_Target.DataBind();

            if (PageIndexChanging != null)
                PageIndexChanging(sender, e);
        }
    }
}