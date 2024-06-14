using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_RPT1 : System.Web.UI.Page
    {
       

        int TPM_FION=0;

        DataTable temp = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            int j = 0;
            for (int i = 100; i <= DateTime.Now.Year-1911; i++)
            {
                
                DropDownList_YEAR.Items.Insert(j, new ListItem(i.ToString(), i.ToString()));

                j++;
            }

            }
        }

        

        protected void Button_MAKE_RPT_Click(object sender, EventArgs e)
        {
            
            DataTable temp = new DataTable();

            temp.Columns.Add("IDNO1", typeof(string));
            temp.Columns.Add("IDNO2", typeof(string));
            temp.Columns.Add("IDNO3", typeof(string));

            string strSQL = string.Format("SELECT MZ_ID FROM C_ONDUTY_HOUR WHERE MZ_YEAR='{2}' AND MZ_MONTH='{3}' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}')", DropDownList_AD.SelectedValue
                                   , DropDownList_UNIT.SelectedValue, DropDownList_YEAR.SelectedValue, DropDownList_MONTH.SelectedValue.PadLeft(2, '0'));

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (tempDT.Rows.Count > 0)
            {

                int out_count = tempDT.Rows.Count % 3 == 0 ? tempDT.Rows.Count / 3 : (tempDT.Rows.Count / 3) + 1;
                for (int i = 0; i < out_count; i++)
                {
                    if (i + 1 != out_count)
                    {
                        DataRow dr = temp.NewRow();
                        for (int j = 3 * i + 1; j < 3 * i + 1 + 3; j++)
                        {
                            int columns = j % 3 == 0 ? 3 : j % 3;
                            dr["IDNO" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_ID"].ToString();
                        }
                        temp.Rows.Add(dr);
                    }
                    else
                    {
                        DataRow dr = temp.NewRow();
                        for (int j = 3 * i + 1; j <= tempDT.Rows.Count; j++)
                        {
                            int columns = j % 3 == 0 ? 3 : j % 3;
                            dr["IDNO" + columns.ToString()] = tempDT.Rows[j - 1]["MZ_ID"].ToString();
                        }
                        temp.Rows.Add(dr);
                    }
                }
            }






            Session["TITLE"] = string.Format("{0}{1}{2}年{3}月輪值表", DropDownList_AD.SelectedItem.Text, DropDownList_UNIT.SelectedItem.Text, DropDownList_YEAR.SelectedValue, DropDownList_MONTH.SelectedValue);
            Session["MZ_YEAR"] = DropDownList_YEAR.SelectedValue.PadLeft(3, '0');
            Session["MZ_MONTH"] = DropDownList_MONTH.SelectedValue.PadLeft(2, '0');
            DateTime DT = new DateTime(1911 + int.Parse(DropDownList_YEAR.SelectedValue), int.Parse(DropDownList_MONTH.SelectedValue), 1);
            Session["WEEKDAY"] = (int)DT.DayOfWeek;
            Session["MONTHDAYS"] = DT.AddMonths(1).AddDays(-1).Day;
            Session["rpt_dt"] = temp;
            string tmp_url = "C_rpt.aspx?fn=monthturns&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {

        }
    }
}
