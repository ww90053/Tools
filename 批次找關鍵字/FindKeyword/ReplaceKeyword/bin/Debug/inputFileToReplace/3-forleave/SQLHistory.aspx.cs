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
    public partial class SQLHistory : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                typeInit();
                SqlDataSource_unit1.SelectCommand = get_ad_sql();
                DropDownList_unit1.DataBind();
            }

        }

        //類型產生
        private void typeInit()
        {
            strSQL = "select distinct MEMO from C_SQLHISTORY";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                ddlType.DataSource = temp;
                ddlType.DataTextField = "MEMO";
                ddlType.DataValueField = "MEMO";
                ddlType.DataBind();
            }
            ddlType.Items.Insert(0, new ListItem("請選擇"));
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

        protected DataTable doSearch()
        {
            temp = new DataTable();

            string action = ddlType.SelectedValue;
            string finalstrSQL = string.Empty;

            string start = tbStart.Text;
            string end = tbEnd.Text;

            DateTime dt1;
            DateTime dt2;

            string ad = DropDownList_unit1.SelectedValue;
            string unit = DropDownList_unit2.SelectedValue;



            List<string> condition = new List<string>();
            List<SqlParameter> ParmeterList = new List<SqlParameter>();

            if (ad != "全部")
            {
                condition.Add("AD=@AD");
                ParmeterList.Add(new SqlParameter("AD", SqlDbType.VarChar) { Value = ad });
            }

            if (unit != "全部" && unit != "請先選擇機關")
            {
                condition.Add("UNIT=@UNIT");
                ParmeterList.Add(new SqlParameter("UNIT", SqlDbType.VarChar) { Value = unit });
            }

            //MEMO
            if (action != "請選擇")
            {
                condition.Add("MEMO=@MEMO");
                ParmeterList.Add(new SqlParameter("MEMO", SqlDbType.VarChar) { Value = action });
            }

            if (DateTime.TryParse(start, out dt1))
            {
                condition.Add(string.Format("ADDDATE >= dbo.TO_DATE('{0} 00:00:00','yyyy/mm/dd hh24:mi:ss')", dt1.ToShortDateString()));
            }
            else
            {
                tbStart.Text = string.Empty;
            }

            if (DateTime.TryParse(end, out dt2))
            {
                condition.Add(string.Format("ADDDATE <= dbo.TO_DATE('{0} 23:59:59','yyyy/mm/dd hh24:mi:ss')", dt2.ToShortDateString()));
            }
            else
            {
                tbEnd.Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(tbSQL.Text))
            {
                condition.Add("SQLTEXT LIKE @SQLTEXT");
                ParmeterList.Add(new SqlParameter("SQLTEXT", SqlDbType.VarChar) { Value = "%" + o_str.tosql(tbSQL.Text) + "%" });
            }
            if (condition.Count > 0)
            {
                condition.Add("RoWnum <= 100");
            }
            else
            {
                condition.Add("RoWnum = 0");
            }
            strSQL = string.Format(@"SELECT SN,MEMO,SQLTEXT,ADDDATE,MZ_NAME,TF.TPFNAME,TF.TPFVALUE,AK1.MZ_KCHI ADNAME,AK2.MZ_KCHI UNITNAME
                                    FROM C_SQLHISTORY MS LEFT JOIN TPF_FIONDATA TF ON MS.TPFID = TF.TPFID 
                                    LEFT JOIN A_KTYPE AK1 ON AD=AK1.MZ_KCODE AND AK1.MZ_KTYPE='04'    
                                    LEFT JOIN A_KTYPE AK2 ON UNIT=AK2.MZ_KCODE AND AK2.MZ_KTYPE='25'
                                    LEFT JOIN A_DLBASE ON MZ_ID=ADDUSER {0} ORDER BY SN DESC",
                  condition.Count() > 0 ? " Where " + string.Join(" And ", condition.ToArray()) : string.Empty);
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, ParmeterList);

            return temp;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            temp = doSearch();
            if (temp.Rows.Count > 0)
            {
                temp.Columns[0].ColumnName = "編號";
                temp.Columns[1].ColumnName = "動作";
                temp.Columns[2].ColumnName = "指令";
                temp.Columns[3].ColumnName = "時間";
                temp.Columns[4].ColumnName = "新增人";
                temp.Columns[5].ColumnName = "頁面";
                temp.Columns[6].ColumnName = "頁面檔案";
                temp.Columns[7].ColumnName = "機關";
                temp.Columns[8].ColumnName = "單位";
                App_Code.ToExcel.Dt2Excel(temp, "EXPORT");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            temp = doSearch();

            gvData.DataSource = temp;
            gvData.DataBind();

        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                lbSQL.Text = gvData.DataKeys[index]["SQLTEXT"].ToString();
                Button1_ModalPopupExtender.Show();
            }
        }
    }
}
