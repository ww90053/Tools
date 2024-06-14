using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Basic1_rpt : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();

                A.set_Panel_EnterToTAB(ref this.Panel1);
                DropDownList_AD.DataBind();
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
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
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    DropDownList_AD.Enabled = false;
                    try
                    {
                        DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    }
                    catch
                    {
                        DropDownList_UNIT.SelectedValue = "";
                    }
                    DropDownList_UNIT.Enabled = false;
                    break;
            }
        }

        protected void Button_MAKE_ALL_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable personal = new DataTable();
            personal.Columns.Add("PIC", System.Type.GetType("System.Byte[]"));
            personal.Columns.Add("MZ_SRANK", typeof(string));
            personal.Columns.Add("MZ_NAME", typeof(string));
            personal.Columns.Add("MZ_ID", typeof(string));
            personal.Columns.Add("MZ_BIR", typeof(string));
            personal.Columns.Add("MZ_SCHOOL", typeof(string));
            personal.Columns.Add("PIC1", System.Type.GetType("System.Byte[]"));
            personal.Columns.Add("MZ_SRANK1", typeof(string));
            personal.Columns.Add("MZ_NAME1", typeof(string));
            personal.Columns.Add("MZ_ID1", typeof(string));
            personal.Columns.Add("MZ_BIR1", typeof(string));
            personal.Columns.Add("MZ_SCHOOL1", typeof(string));

            dt = o_DBFactory.ABC_toTest.Create_Table("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE= MZ_OCCC AND MZ_KTYPE='26')MZ_SRANK,MZ_NAME,MZ_ID,MZ_BIR,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD = '" + DropDownList_AD.SelectedValue + "' AND MZ_EXUNIT = '" + DropDownList_UNIT.SelectedValue + "' AND MZ_STATUS2='Y' ORDER BY TBDV,OCCC ", "personal");
            string strSQL = string.Empty;
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (i % 2 == 1)
                {
                    try
                    {
                      
                        ViewState ["PIC"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")));
                    }
                    catch
                    {
                    }

                    ViewState["MZ_SRANK"] = dr["MZ_SRANK"];
                    ViewState["MZ_NAME"] = dr["MZ_NAME"];
                    ViewState["MZ_ID"] = dr["MZ_ID"];
                    ViewState["MZ_BIR"] = dr["MZ_BIR"];

                  

                    strSQL = string.Format("SELECT * FROM (SELECT MZ_YEAR,MZ_SCHOOL,MZ_DEPARTMENT,(dbo.SUBSTR(MZ_ENDDATE,1,3) +'年'+dbo.SUBSTR(MZ_ENDDATE,4,2)+'月'+dbo.SUBSTR(MZ_ENDDATE,6,2)+'日') MZ_ENDDATE  FROM A_EDUCATION WHERE MZ_ID='{0}'  ORDER BY MZ_ENDDATE DESC ) WHERE ROWNUM=1", dr["MZ_ID"]);
                    DataTable temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    if (temp1.Rows.Count > 0)
                    {
                        string year = string.Empty;
                        if (!string.IsNullOrEmpty(temp1.Rows[0]["MZ_YEAR"].ToString()))
                            year = "第" + temp1.Rows[0]["MZ_YEAR"] + "期";
                      
                       ViewState["MZ_SCHOOL"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_SCHOOL"].ToString() + temp1.Rows[0]["MZ_DEPARTMENT"].ToString() + year, 20, "&N");
                    }
                    else
                    {
                        ViewState["MZ_SCHOOL"] = "";
                       
                    }

                    if (dt.Rows.Count == i)//20140219總計基數最後一筆
                    {
                        DataRow dr1 = personal.NewRow();
                        dr1["PIC"] = ViewState["PIC"];
                        dr1["MZ_SRANK"] = ViewState["MZ_SRANK"];
                        dr1["MZ_NAME"] = ViewState["MZ_NAME"];
                        dr1["MZ_ID"] = ViewState["MZ_ID"];
                        dr1["MZ_BIR"] = ViewState["MZ_BIR"];
                        dr1["MZ_SCHOOL"] = ViewState["MZ_SCHOOL"];

                        //dr1["PIC"] = Session["PIC"];
                        //dr1["MZ_SRANK"] = Session["MZ_SRANK"];
                        //dr1["MZ_NAME"] = Session["MZ_NAME"];
                        //dr1["MZ_ID"] = Session["MZ_ID"];
                        //dr1["MZ_BIR"] = Session["MZ_BIR"];
                        //dr1["MZ_SCHOOL"] = Session["MZ_SCHOOL"];

                        personal.Rows.Add(dr1);
                    }
                }
                else if (dt.Rows.Count == i)
                {
                    DataRow dr1 = personal.NewRow();
                    dr1["PIC"] = ViewState["PIC"];
                    dr1["MZ_SRANK"] = ViewState["MZ_SRANK"];
                    dr1["MZ_NAME"] = ViewState["MZ_NAME"];
                    dr1["MZ_ID"] = ViewState["MZ_ID"];
                    dr1["MZ_BIR"] = ViewState["MZ_BIR"];
                    dr1["MZ_SCHOOL"] = ViewState["MZ_SCHOOL"];

                    //dr1["PIC"] = Session["PIC"];
                    //dr1["MZ_SRANK"] = Session["MZ_SRANK"];
                    //dr1["MZ_NAME"] = Session["MZ_NAME"];
                    //dr1["MZ_ID"] = Session["MZ_ID"];
                    //dr1["MZ_BIR"] = Session["MZ_BIR"];
                    //dr1["MZ_SCHOOL"] = Session["MZ_SCHOOL"];

                    //20140219總計偶數最後一筆
                    dr1["PIC1"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")));
                    dr1["MZ_SRANK1"] = dr["MZ_SRANK"];
                    dr1["MZ_NAME1"] = dr["MZ_NAME"];
                    dr1["MZ_ID1"] = dr["MZ_ID"];
                    dr1["MZ_BIR1"] = dr["MZ_BIR"];

                    strSQL = string.Format("SELECT * FROM (SELECT MZ_YEAR,MZ_SCHOOL,MZ_DEPARTMENT,(dbo.SUBSTR(MZ_ENDDATE,1,3) +'年'+dbo.SUBSTR(MZ_ENDDATE,4,2)+'月'+dbo.SUBSTR(MZ_ENDDATE,6,2)+'日') MZ_ENDDATE  FROM A_EDUCATION WHERE MZ_ID='{0}'  ORDER BY MZ_ENDDATE DESC ) WHERE ROWNUM=1", dr["MZ_ID"]);
                    DataTable temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (temp1.Rows.Count > 0)
                    {
                        string year = string.Empty;
                        if (!string.IsNullOrEmpty(temp1.Rows[0]["MZ_YEAR"].ToString()))
                            year = "第" + temp1.Rows[0]["MZ_YEAR"] + "期";

                        dr1["MZ_SCHOOL1"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_SCHOOL"].ToString() + temp1.Rows[0]["MZ_DEPARTMENT"].ToString() + year, 20, "&N");
                    } 

                    personal.Rows.Add(dr1);
                }
                else if (i % 2 == 0)
                {
                    DataRow dr1 = personal.NewRow();
                    dr1["PIC"] = ViewState["PIC"];
                    dr1["MZ_SRANK"] = ViewState["MZ_SRANK"];
                    dr1["MZ_NAME"] = ViewState["MZ_NAME"];
                    dr1["MZ_ID"] = ViewState["MZ_ID"];
                    dr1["MZ_BIR"] = ViewState["MZ_BIR"];
                    dr1["MZ_SCHOOL"] = ViewState["MZ_SCHOOL"];

                    //dr1["PIC"] = Session["PIC"];
                    //dr1["MZ_SRANK"] = Session["MZ_SRANK"];
                    //dr1["MZ_NAME"] = Session["MZ_NAME"];
                    //dr1["MZ_ID"] = Session["MZ_ID"];
                    //dr1["MZ_BIR"] = Session["MZ_BIR"];
                    //dr1["MZ_SCHOOL"] = Session["MZ_SCHOOL"];


                    dr1["PIC1"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")));
                    dr1["MZ_SRANK1"] = dr["MZ_SRANK"];
                    dr1["MZ_NAME1"] = dr["MZ_NAME"];
                    dr1["MZ_ID1"] = dr["MZ_ID"];
                    dr1["MZ_BIR1"] = dr["MZ_BIR"];

                    strSQL = string.Format("SELECT * FROM (SELECT MZ_YEAR,MZ_SCHOOL,MZ_DEPARTMENT,(dbo.SUBSTR(MZ_ENDDATE,1,3) +'年'+dbo.SUBSTR(MZ_ENDDATE,4,2)+'月'+dbo.SUBSTR(MZ_ENDDATE,6,2)+'日') MZ_ENDDATE  FROM A_EDUCATION WHERE MZ_ID='{0}'  ORDER BY MZ_ENDDATE DESC ) WHERE ROWNUM=1", dr["MZ_ID"]);
                    DataTable temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (temp1.Rows.Count > 0)
                    {
                        string year = string.Empty;
                        if (!string.IsNullOrEmpty(temp1.Rows[0]["MZ_YEAR"].ToString()))
                            year = "第" + temp1.Rows[0]["MZ_YEAR"] + "期";

                        dr1["MZ_SCHOOL1"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_SCHOOL"].ToString() + temp1.Rows[0]["MZ_DEPARTMENT"].ToString() + year, 20, "&N");
                    } 
                    
                    personal.Rows.Add(dr1);
                }
                i++;
            }
            Session["title_ad"] = DropDownList_AD.SelectedItem.Text;
            Session["title_unit"] = DropDownList_UNIT.SelectedItem.Text.Substring(0, DropDownList_UNIT.SelectedItem.Text.Length);
            Session["rpt_dt"] = personal;
            string tmp_url = "A_rpt.aspx?fn=basic1";
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        public byte[] imageToByte(string path)
        {
            string file_path = string.Empty;
            try
            {
                file_path = Server.MapPath(path);
                System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                BinaryReader br = new BinaryReader(fileOpen);
                byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                fileOpen.Close();
                //XX2013/06/18 
                fileOpen.Dispose();
                return by;
            }
            catch
            {
                file_path = Server.MapPath("~/1-personnel/images/nopic.jpg");
                System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                BinaryReader br = new BinaryReader(fileOpen);
                byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                fileOpen.Close();
                fileOpen.Dispose();
                return by;
            }
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }
    }
}
