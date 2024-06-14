using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB._3_forleave;

namespace TPPDDB._2_salary
{
    public partial class TEST : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////檢查PK資料是否存在
            //string strSQL = string.Format(@"Select COUNT(*) From C_OVERTIME_FOR_DLTB01 Where OVERTIME_SN='{0}' And MZ_DLTB01_SN='{1}' ",
            //    cobDt.Rows[0]["SN"].ToStringNullSafe(), ViewState["SN"].ToStringNullSafe());
            //if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, null) != "0")
            //{
            //    //更新該參照檔
            //    strSQL = "UPDATE C_OVERTIME_FOR_DLTB01 Set REST_HOUR=::REST_HOUR Where OVERTIME_SN=@OVERTIME_SN And MZ_DLTB01_SN=@MZ_DLTB01_SN ";
            //}
            //else
            //{
            //    //新增參照檔
            //    }
            string strSQL = "INSERT INTO C_OVERTIME_FOR_DLTB01(OVERTIME_SN, MZ_DLTB01_SN, REST_HOUR) Values(@OVERTIME_SN, @MZ_DLTB01_SN, @REST_HOUR) ";

            var parameters = new List<SqlParameter>()
                                        {
                                            new SqlParameter("REST_HOUR", 1),
                                            new SqlParameter("OVERTIME_SN", "2"),
                                            new SqlParameter("MZ_DLTB01_SN", 333)
                                        };
            o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters);

            throw new NotImplementedException();
           //bool a= C_ForLeave_YearVacation_Create.checkExist_C_DLTBB("","");
          //var data=  MathHelper.Round((decimal.Parse("44770") + decimal.Parse("24160") + decimal.Parse("4350")) /240, 0);
        }

    }
}