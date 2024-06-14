using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TPPDDB._2_salary
{
    public class Branch
    {
        public int sn;
        public string id;
        public string ip;
        /// <summary>
        /// 單位代碼(縣市別+機關別)
        /// </summary>
        public string taxunit;
        /// <summary>
        /// 統一編號
        /// </summary>
        public string taxinvoice;
        public string taxname;
        public string taxaddr;
        public string taxpers;
        public string ticketnum;

        public Branch(string id)
        {
            DataTable dt;
            string sql = string.Format("SELECT B_SNID, IP , TAXUNIT , TAXINVOICE ,TAXNAME,TAXADDR ,TAXPERS ,TICKETNUM     FROM B_BRANCH WHERE ID='{0}'", id);
            dt = o_DBFactory.ABC_toTest.DataSelect(sql);

            if(dt.Rows.Count==0)
                return;

            this.sn = int.Parse(dt.Rows[0]["B_SNID"].ToString());
            this.id = id;
            this.ip = dt.Rows[0]["IP"].ToString();
            this.taxunit = dt.Rows[0]["TAXUNIT"].ToString();
            this.taxinvoice = dt.Rows[0]["TAXINVOICE"].ToString();
            this.taxname = dt.Rows[0]["TAXNAME"].ToString();
            this.taxaddr = dt.Rows[0]["TAXADDR"].ToString();
            this.taxpers = dt.Rows[0]["TAXPERS"].ToString();
            this.ticketnum = dt.Rows[0]["TICKETNUM"].ToString();
        }
    }
}
