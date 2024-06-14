using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_RESTDATE_SEARCH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //20150130
                //超勤補休能保留一年

                DateTime date365 = DateTime.Now.AddDays(-365);
                //DateTime date365 = DateTime.Now.AddDays((-365 * 2));
                string datestring365 = (date365.Year - 1911).ToString().PadLeft(3, '0') + date365.Month.ToString().PadLeft(2, '0') + date365.Day.ToString().PadLeft(2, '0');

                //其他則保留184天
                DateTime date184 = DateTime.Now.AddDays(-184);
                string datestring184 = (date184.Year - 1911).ToString().PadLeft(3, '0') + date184.Month.ToString().PadLeft(2, '0') + date184.Day.ToString().PadLeft(2, '0');


                //ViewState["SelectCommand"] = "SELECT MZ_DATE,MZ_RESTHOUR FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + Session["ForLaeveBasic_RESTDATE_ID"].ToString() + "' AND MZ_RESTHOUR>0 AND dbo.SUBSTR(RESTFLAG,1,1)='Y'  AND MZ_DATE>='" + datestring + "'";

                //修改從107/5/1開始不管哪種補休一律可保留365日，107/5/1以前則保留日期不變 20180508 by sky
                //SqlDataSource1.SelectCommand = "SELECT MZ_DATE,MZ_RESTHOUR FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + Session["ForLaeveBasic_RESTDATE_ID"].ToString() + "' AND MZ_RESTHOUR>0 AND (( RESTFLAG IN ('Y','YO','YD')  AND MZ_DATE>='" + datestring184 + "' ) OR (RESTFLAG='YU' AND MZ_DATE>='" + datestring365 + "'))  ORDER BY MZ_DATE";
                SqlDataSource1.SelectCommand = string.Format(@"SELECT MZ_DATE,MZ_RESTHOUR FROM C_OVERTIME_HOUR_INSIDE
                                               WHERE MZ_ID='{0}' AND MZ_RESTHOUR>0 
                                               AND ((MZ_DATE>='1070501' AND ( RESTFLAG IN ('Y','YO','YD','YU') AND MZ_DATE>='{1}'))
                                                    OR (MZ_DATE<'1070501' AND(( RESTFLAG IN ('Y','YO','YD')  AND MZ_DATE>='{2}' ) OR (RESTFLAG='YU' AND MZ_DATE>='{1}'))))
                                                ORDER BY MZ_DATE"
                                                , Session["ForLaeveBasic_RESTDATE_ID"].ToString()
                                                , datestring365
                                                , datestring184);
                //SqlDataSource1.SelectCommand = ViewState["SelectCommand"].ToString();
                GridView1.DataBind();

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<int> tmp = new List<int>();
            dt.Columns.Add("MZ_DATE", typeof(string));
            dt.Columns.Add("MZ_RESTHOUR", typeof(string));

            int totaltime = 0;
            int tmptime = 0;

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if ((GridView1.Rows[i].FindControl("CheckBox1") as CheckBox).Checked)
                {
                    DataRow dr = dt.NewRow();

                    dr["MZ_DATE"] = GridView1.Rows[i].Cells[1].Text;
                    dr["MZ_RESTHOUR"] = GridView1.Rows[i].Cells[2].Text;

                    totaltime += int.Parse(GridView1.Rows[i].Cells[2].Text);
                    tmp.Add(int.Parse(GridView1.Rows[i].Cells[2].Text));
                    dt.Rows.Add(dr);
                }
            }
            
            //
            tmp.Sort();
            tmp.Reverse();
            for (int j=0;j<tmp.Count -1 ;j++)
            {
                tmptime += tmp[j];
            }
            int leavetime = int.Parse(Session["LeaveTime"].ToString());
            if (tmptime >= leavetime)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數超過輸入時數，請重新輸入！')", true);
                return;
            }else if (totaltime < leavetime)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('所選補休時數小於輸入時數，請重新輸入！')", true);
                return;
            }

            Session["MZ_RESTHOUR_DT"] = dt;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["ForLeaveBasic_BT"] + "').click();window.close();", true);
        }
    }
}
