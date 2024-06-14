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
using System.IO;
namespace TPPDDB._1_personnel
{
    public partial class Personal_Basic_rpt : System.Web.UI.Page
    {
        
        DataTable temp = new DataTable();
        //DataTable basicAll;
        //DataTable basicBoss;
        int TPM_FION=0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
          
                A.fill_AD(DropDownList_AD );
                DropDownList_AD.SelectedValue = Session["ADPMZ_AD"].ToString();
                A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
            }
        }

        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    break;
                case "C":
                    DropDownList_AD.Enabled = false;
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void Button_MAKE_ALL_Click(object sender, EventArgs e)
        {
            
            string tmp_url = "A_rpt.aspx?fn=basic_all&AD=" + DropDownList_AD.SelectedValue + "&UNIT=" + DropDownList_UNIT.SelectedValue + "&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        //protected DataTable basicAll_init()
        //{
        //    basicAll = new DataTable();
        //    basicAll.Columns.Add("SN", typeof(string));
        //    basicAll.Columns.Add("MZ_OCCC", typeof(string));
        //    basicAll.Columns.Add("MZ_NAME", typeof(string));
        //    basicAll.Columns.Add("MZ_BIR", typeof(string));
        //    basicAll.Columns.Add("MZ_CAREER_AD", typeof(string));
        //    basicAll.Columns.Add("MZ_CAREER_UNIT", typeof(string));
        //    basicAll.Columns.Add("MZ_CAREER_ADATE", typeof(string));
        //    basicAll.Columns.Add("MZ_PHONO", typeof(string));
        //    basicAll.Columns.Add("MZ_PHONH", typeof(string));
        //    basicAll.Columns.Add("MZ_MOVETEL", typeof(string));
        //    basicAll.Columns.Add("MZ_SCHOOL", typeof(string));
        //    basicAll.Columns.Add("MZ_SCHOOL_DEPARTMENT", typeof(string));
        //    basicAll.Columns.Add("MZ_SCHOOL_YEAR", typeof(string));
        //    basicAll.Columns.Add("EXAM_NAME", typeof(string));
        //    basicAll.Columns.Add("UNIT", typeof(string));
        //    basicAll.Columns.Add("ADATE", typeof(string));
        //    basicAll.Columns.Add("IMG", System.Type.GetType("System.Byte[]"));

        //    return basicAll;
        //}

        //protected void merge_basicAll(DataTable basic)
        //{
        //    basicAll = basic;
        //    strSQL = "SELECT MZ_EXTPOS,MZ_OCCC as OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_OCCC AND MZ_KTYPE='26') MZ_OCCC," +
        //                          "MZ_NAME,MZ_MOVETEL,MZ_PHONO,MZ_PHONH,A_DLBASE.MZ_ID,(dbo.SUBSTR(MZ_BIR,1,3)+'年'+dbo.SUBSTR(MZ_BIR,4,2)+'月'+dbo.SUBSTR(MZ_BIR,6,2)+'日') MZ_BIR,(dbo.SUBSTR(MZ_ADATE,1,3) + '年' + dbo.SUBSTR(MZ_ADATE,4,2) + '月' + dbo.SUBSTR(MZ_ADATE,6,2)+'日') ADATE" +
        //                          " FROM A_DLBASE WHERE MZ_STATUS2='Y'";

        //    if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
        //        strSQL += " AND MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";

        //    if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
        //        strSQL += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";

        //    strSQL += " ORDER BY MZ_TBDV,OCCC";

        //    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

        //    int num = 1;
        //    foreach (DataRow dr in temp.Rows)
        //    {
        //        //兼
        //        dr["MZ_OCCC"] = dr["MZ_OCCC"].ToString() + o_A_KTYPE.CODE_TO_NAME(dr["MZ_EXTPOS"].ToString(), "@91");
        //        //經歷及基本資料
        //        DataRow merge_dr = basicAll.NewRow();
        //        strSQL = string.Format("SELECT * FROM (SELECT MZ_ID,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KTYPE='04') MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') MZ_UNIT,(dbo.SUBSTR(MZ_ADATE,1,3) +'年'+dbo.SUBSTR(MZ_ADATE,4,2)+'月'+dbo.SUBSTR(MZ_ADATE,6,2)+'日') MZ_ADATE,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_CAREER.MZ_OCCC AND MZ_KTYPE='26') MZ_OCCC  FROM A_CAREER WHERE MZ_ID='{0}'  ORDER BY MZ_ADATE DESC ) WHERE ROWNUM<4", dr["MZ_ID"]);
        //        DataTable temp1 = new DataTable();
        //        temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
        //        merge_dr["SN"] = num;
        //        merge_dr["UNIT"] = o_A_DLBASE.CUNIT(dr["MZ_ID"].ToString());
        //        merge_dr["ADATE"] = dr["ADATE"];
        //        merge_dr["MZ_OCCC"] = dr["MZ_OCCC"];
        //        merge_dr["MZ_NAME"] = dr["MZ_NAME"];
        //        merge_dr["MZ_BIR"] = dr["MZ_BIR"];
        //        string MZ_PHONO = string.IsNullOrEmpty(dr["MZ_PHONO"].ToString()) ? string.Empty : dr["MZ_PHONO"].ToString();
        //        string MZ_PHONH = string.IsNullOrEmpty(dr["MZ_PHONH"].ToString()) ? string.Empty : dr["MZ_PHONH"].ToString();
        //        string MZ_MOVETEL = string.IsNullOrEmpty(dr["MZ_MOVETEL"].ToString()) ? string.Empty : dr["MZ_MOVETEL"].ToString();
        //        merge_dr["MZ_PHONO"] = "公：" + MZ_PHONO;
        //        merge_dr["MZ_PHONH"] = "宅：" + MZ_PHONH;
        //        merge_dr["MZ_MOVETEL"] = "行：" + MZ_MOVETEL;

        //        if (temp1.Rows.Count > 0)
        //        {
        //            try
        //            {
        //                //最近經歷
        //                try
        //                {
        //                    merge_dr["MZ_CAREER_ADATE"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_ADATE"].ToString().Replace("年", "").Replace("月", "").Replace("日", "") + temp1.Rows[0]["MZ_AD"].ToString() + temp1.Rows[0]["MZ_UNIT"].ToString() + RANK1_NAME(temp1.Rows[0]["MZ_ID"].ToString(), 1) + temp1.Rows[0]["MZ_OCCC"].ToString(), 26, "&N");
        //                }
        //                catch { }
        //                //第二近
        //                try
        //                {
        //                    merge_dr["MZ_CAREER_AD"] = o_CommonService.d_report_break_line(temp1.Rows[1]["MZ_ADATE"].ToString().Replace("年", "").Replace("月", "").Replace("日", "") + temp1.Rows[1]["MZ_AD"].ToString() + temp1.Rows[1]["MZ_UNIT"].ToString() + RANK1_NAME(temp1.Rows[1]["MZ_ID"].ToString(), 2) + temp1.Rows[1]["MZ_OCCC"].ToString(), 26, "&N");
        //                }
        //                catch { }
        //                //第三近
        //                try
        //                {
        //                    merge_dr["MZ_CAREER_UNIT"] = o_CommonService.d_report_break_line(temp1.Rows[2]["MZ_ADATE"].ToString().Replace("年", "").Replace("月", "").Replace("日", "") + temp1.Rows[2]["MZ_AD"].ToString() + temp1.Rows[2]["MZ_UNIT"].ToString() + RANK1_NAME(temp1.Rows[2]["MZ_ID"].ToString(), 3) + temp1.Rows[2]["MZ_OCCC"].ToString(), 26, "&N");
        //                }
        //                catch { }
        //            }
        //            catch { }
        //        }

        //        //學歷
        //        strSQL = string.Format("SELECT * FROM (SELECT MZ_YEAR,MZ_SCHOOL,MZ_DEPARTMENT,(dbo.SUBSTR(MZ_ENDDATE,1,3) +'年'+dbo.SUBSTR(MZ_ENDDATE,4,2)+'月'+dbo.SUBSTR(MZ_ENDDATE,6,2)+'日') MZ_ENDDATE  FROM A_EDUCATION WHERE MZ_ID='{0}'  ORDER BY MZ_ENDDATE DESC ) WHERE ROWNUM=1", dr["MZ_ID"]);
        //        temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

        //        if (temp1.Rows.Count > 0)
        //        {
        //            string year = string.Empty;
        //            if (!string.IsNullOrEmpty(temp1.Rows[0]["MZ_YEAR"].ToString()))
        //                year = "第" + temp1.Rows[0]["MZ_YEAR"] + "期";

        //            merge_dr["MZ_SCHOOL"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_SCHOOL"].ToString() + temp1.Rows[0]["MZ_DEPARTMENT"].ToString() + year, 20, "&N");
        //            //merge_dr["MZ_SCHOOL_DEPARTMENT"] = temp1.Rows[0]["MZ_DEPARTMENT"];
        //            //merge_dr["MZ_SCHOOL_YEAR"] = temp1.Rows[0]["MZ_ENDDATE"];

        //        }

        //        //考試別
        //        strSQL = string.Format("SELECT * FROM (SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=EXAM_NAME AND MZ_KTYPE='EXK') AS EXAM_NAME FROM A_EXAM WHERE MZ_ID='{0}' ORDER BY EXAM_YEAR) WHERE ROWNUM = 1", dr["MZ_ID"]);
        //        temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
        //        if (temp1.Rows.Count > 0)
        //        {
        //            merge_dr["EXAM_NAME"] = temp1.Rows[0]["EXAM_NAME"];
        //        }

        //        try
        //        {
        //            merge_dr["IMG"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")));
        //        }
        //        catch
        //        {
        //        }

        //        basicAll.Rows.Add(merge_dr);
        //        num++;
        //    }

        //    ViewState["basicAll"] = basicAll;
        //}

        //protected string RANK1_NAME(string IDNO, int ROWNUM)
        //{
        //    string strSQL = "SELECT * FROM (SELECT MZ_RANK1,ROWNUM AS num FROM (SELECT MZ_RANK1 FROM A_CAREER WHERE MZ_ID='" + IDNO + "' ORDER BY MZ_ADATE DESC)) WHERE num =" + ROWNUM + "";
        //    DataTable temp = new DataTable();
        //    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

        //    if (temp.Rows.Count == 1)
        //    {
        //        return o_A_KTYPE.CODE_TO_NAME(temp.Rows[0][0].ToString(), "09");
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //public byte[] imageToByte(string path)
        //{
        //    string file_path = string.Empty;
        //    try
        //    {
        //        file_path = Server.MapPath(path);
        //        System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
        //        BinaryReader br = new BinaryReader(fileOpen);
        //        byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
        //        fileOpen.Close();

        //        //XX2013/06/18 
        //        fileOpen.Dispose();

        //        return by;
        //    }
        //    catch
        //    {
        //        file_path = Server.MapPath("~/1-personnel/images/nopic.jpg");
        //        System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
        //        BinaryReader br = new BinaryReader(fileOpen);
        //        byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
        //        fileOpen.Close();
        //        //XX2013/06/18 
        //        fileOpen.Dispose();
                
        //        return by;
        //    }
        //}

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }
    }
}
