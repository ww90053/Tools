using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class UseHistory : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        public enum SearchType
        {
            新增,
            查詢
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                yearInit();
                typeInit();
                SqlDataSource_unit1.SelectCommand = get_ad_sql();
                DropDownList_unit1.DataBind();
            }

        }

        //類型產生
        private void typeInit()
        {
            ddlType.DataSource = Enum.GetNames(typeof(SearchType));
            ddlType.DataBind();
        }
        //年份產生
        private void yearInit()
        {
            strSQL = "select distinct YEAR from C_USEHISTORY";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                ddlYear.DataSource = temp;
                ddlYear.DataTextField = "YEAR";
                ddlYear.DataValueField = "YEAR";
                ddlYear.DataBind();
            }
            else
            {
                ListItem li = new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString());
                ddlYear.Items.Add(li);
            }
        }

        protected void DropDownList_unit1_DataBound(object sender, EventArgs e)
        {
            DropDownList_unit1.Items.Insert(0, add_nullitem_v2("全部"));
        }

        protected void DropDownList_unit2_DataBound(object sender, EventArgs e)
        {
            DropDownList_unit2.Items.Insert(0, add_nullitem_v2("全部"));
        }

        protected void DropDownList_unit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SqlDataSource_unit2.SelectCommand = get_unit_sql(DropDownList_unit1.SelectedValue);
            //DropDownList_unit2.DataBind();
            if (DropDownList_unit1.SelectedValue != "全部")
            {
                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(get_unit_sql(DropDownList_unit1.SelectedValue), "GET");
                if (dt.Rows.Count > 0)
                {
                    DropDownList_unit2.DataSource = dt;
                    DropDownList_unit2.DataTextField = "MZ_KCHI";
                    DropDownList_unit2.DataValueField = "MZ_KCODE";
                    DropDownList_unit2.DataBind();
                }
            }
            else
            {
                DropDownList_unit2.Items.Clear();
                DropDownList_unit2.Items.Add(new ListItem("請先選擇機關"));
            }
        }

        public string get_ad_sql()
        {
            string rs = " SELECT MZ_KCODE, MZ_KCHI" +
                        " FROM A_KTYPE  " +
                        " WHERE MZ_KTYPE='04' " +
                        " 	AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' ";
            return rs;
        }

        public string get_unit_sql(string tmp_mz_ad)
        {
            string rs = " SELECT MZ_KCODE, MZ_KCHI " +
                        " FROM A_KTYPE " +
                        " WHERE MZ_KTYPE='25' " +
                        " AND MZ_KCODE IN (SELECT MZ_UNIT FROM VW_A_UNIT_AD  WHERE MZ_AD='" + tmp_mz_ad + "')";
            return rs;
        }

        public static ListItem add_nullitem_v2(string tmp_text)
        {
            ListItem nulldata = new ListItem();
            //nulldata.Value = "";
            nulldata.Text = tmp_text;
            nulldata.Value = "全部";
            return nulldata;
        }

        protected void RadioButton_g1_CheckedChanged(object sender, EventArgs e)
        {
            tr_unit2.Visible = false;
        }

        protected void RadioButton_g2_CheckedChanged(object sender, EventArgs e)
        {
            tr_unit2.Visible = true;
        }

        protected DataTable doSearch()
        {
            temp = new DataTable();

            string action = string.Empty;
            switch ((SearchType)Enum.Parse(typeof(SearchType), ddlType.SelectedValue))
            {
                case SearchType.新增:
                    action += " AND ACTION='A' ";
                    break;
                case SearchType.查詢:
                    action += " AND ACTION='S' ";
                    break;
                default:
                    break;
            }
            string year = string.Empty;
            string month = string.Empty;
            string finalstrSQL = string.Empty;
            string ad = string.Empty;
            string unit = string.Empty;


            year += string.Format(" AND YEAR={0} ", ddlYear.SelectedValue);
            if (ddlMonth.SelectedValue != "請選擇")
            {
                month += string.Format(" AND MONTH={0} ", ddlMonth.SelectedValue);
            }

            if (RadioButton_g1.Checked)
            {

                if (DropDownList_unit1.SelectedValue != "全部")
                {
                    finalstrSQL += string.Format(@"(SELECT '{4}' name,nvl(sum(amount),0) amount FROM C_USEHISTORY 
                           WHERE 1=1 {0} {1} {2} {3}) UNION", string.Format(" AND AD='{0}' ", DropDownList_unit1.SelectedValue)
                                                          , year, month, action, DropDownList_unit1.SelectedItem.Text);
                }
                else
                {
                    foreach (ListItem li in DropDownList_unit1.Items)
                    {
                        if (li.Value != "全部")
                        {
                            finalstrSQL += string.Format(@"(SELECT '{4}' name,nvl(sum(amount),0) amount FROM C_USEHISTORY 
                           WHERE 1=1 {0} {1} {2} {3}) UNION", string.Format(" AND AD='{0}' ", li.Value)
                                                              , year, month, action, li.Text);
                        }
                    }
                }
            }
            else if (RadioButton_g2.Checked)
            {
                if (DropDownList_unit2.SelectedValue == "請先選擇機關")
                {
                    ScriptManager.RegisterClientScriptBlock(ddlMonth, this.GetType(), "click", "alert('請選擇機關!!')", true);
                    return temp;
                }
                else if (DropDownList_unit2.SelectedValue != "全部")
                {
                    finalstrSQL += string.Format(@"(SELECT '{4}' name,nvl(sum(amount),0) amount FROM C_USEHISTORY 
                           WHERE 1=1 {0} {1} {2} {3} {5}) UNION", string.Format(" AND AD='{0}' ", DropDownList_unit1.SelectedValue)
                                                            , year, month, action, string.Format("{0} - {1}", DropDownList_unit1.SelectedItem.Text, DropDownList_unit2.SelectedItem.Text), string.Format(" AND UNIT='{0}' ", DropDownList_unit2.SelectedValue));

                }
                else
                {
                    foreach (ListItem li in DropDownList_unit2.Items)
                    {
                        if (li.Value != "全部")
                        {
                            finalstrSQL += string.Format(@"(SELECT '{4}' name,nvl(sum(amount),0) amount FROM C_USEHISTORY 
                           WHERE 1=1 {0} {1} {2} {3} {5}) UNION", string.Format(" AND AD='{0}' ", DropDownList_unit1.SelectedValue)
                                                           , year, month, action, string.Format("{0} - {1}", DropDownList_unit1.SelectedItem.Text, li.Text), string.Format(" AND UNIT='{0}' ", li.Value));
                        }
                    }
                }
            }
            else
            {
                return temp;
            }


            strSQL = string.Format(@"SELECT name 單位,amount 次數 FROM (
                                     {0})
                                     ", finalstrSQL.Substring(0, finalstrSQL.Length - 5));
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            return temp;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            temp = doSearch();
            if (temp.Rows.Count > 0)
            {
                App_Code.ToExcel.Dt2Excel(temp, "EXPORT");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            temp = doSearch();
            if (temp.Rows.Count > 0)
            {
                gvData.DataSource = temp;
                gvData.DataBind();
            }
        }
    }

    public partial class ExportFormat
    {
        public string AD { get; set; }
        public string UNIT { get; set; }
        public int AMOUNT { get; set; }
        public int YEAR { get; set; }
        public int MONTH { get; set; }
        public string ACTION { get; set; }
    }
}
