using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public class OtherPeople
    {
        public int sn;
        public string id;
        public string payad;
        public string name;
        public string polno;

        public OtherPeople(string payad, string id)
        {
            string sql = "SELECT MB_SNID,IDCARD,NAME,MZ_POLNO FROM B_MANUFACTURER_BASE WHERE PAY_AD=@payad AND IDCARD=@id";
            List<SqlParameter> ps = new List<SqlParameter>();
            DataTable dt;

            ps.Add(new SqlParameter("payad", payad));
            ps.Add(new SqlParameter("id", id));
            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            if (dt.Rows.Count == 0)
                return;

            this.sn = int.Parse(dt.Rows[0]["MB_SNID"].ToString());
            this.id = dt.Rows[0]["IDCARD"].ToString();
            this.payad = payad;
            this.name = dt.Rows[0]["NAME"].ToString();
            this.polno = dt.Rows[0]["MZ_POLNO"].ToString();
        }
    }
}
