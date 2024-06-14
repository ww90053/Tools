using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace TPPDDB._1_personnel
{
    public partial class Personal_Ktype_Search : System.Web.UI.Page
    {
        string stringSQL = "";
        string strSQL1 = "";
        string CID_NAME = "";
        string MZ_KTYPE = "";
        string AD = "";
        string UNIT = "";
        string FIRST = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            CID_NAME = Request["CID_NAME"];
            MZ_KTYPE = Request["MZ_KTYPE"];
            AD = Request["AD"];
            UNIT = Request["UNIT"];
            FIRST = Request["FIRST"];

            //ViewState["CID_NAME"] = Request["CID_NAME"];
            //ViewState["MZ_KTYPE"] = Request["MZ_KTYPE"];
            //ViewState["AD"] = Request["AD"];
            //ViewState["UNIT"] = Request["UNIT"];
            //ViewState["FIRST"] = Request["FIRST"];

            //if (!IsPostBack)//2013/06/24 有沒有postback都要
            //{
                if (!string.IsNullOrEmpty(CID_NAME) )
                {
                    if (CID_NAME.Trim() == "AD")
                    {
                        // stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE 1=1";
                        
                        stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' ) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' ) WHERE 1=1";
                    }
                    if (CID_NAME.Trim() == "UNIT")
                    {
                        stringSQL = " SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + AD.Trim() + "') ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + AD.Trim() + "') ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    if (CID_NAME.Trim() == "DUTYMODE")
                    {
                        stringSQL = " SELECT DUTY_NO AS MZ_KCODE,DUTY_NAME AS MZ_KCHI FROM C_DUTYITEM order by DUTY_NO ";
                        strSQL1 = "SELECT DUTY_NO AS MZ_KCODE,DUTY_NAME AS MZ_KCHI FROM C_DUTYITEM order by DUTY_NO";
                    }
                    if (CID_NAME.Trim() == "DUTYPATROL")
                    {
                        stringSQL = " SELECT MZ_DUTYPATROL_NO AS MZ_KCODE,MZ_DUTYPATROL AS MZ_KCHI FROM C_DUTYPATROL WHERE MZ_AD='" + AD.Trim() + "' AND MZ_UNIT='" + UNIT.Trim() + "'";
                        strSQL1 = "SELECT MZ_DUTYPATROL_NO AS MZ_KCODE,MZ_DUTYPATROL AS MZ_KCHI FROM C_DUTYPATROL WHERE MZ_AD='" + AD.Trim() + "' AND MZ_UNIT='" + UNIT.Trim() + "'";
                    }
                    if (CID_NAME.Trim() == "DUTYTARGET")
                    {
                        stringSQL = " SELECT MZ_DUTYTARGET_NO AS MZ_KCODE,MZ_DUTYTARGET AS MZ_KCHI FROM C_DUTYTARGET WHERE MZ_AD='" + AD.Trim() + "' AND MZ_UNIT='" + UNIT.Trim() + "'";
                        strSQL1 = "SELECT MZ_DUTYTARGET_NO AS MZ_KCODE,MZ_DUTYTARGET AS MZ_KCHI FROM C_DUTYTARGET WHERE MZ_AD='" + AD.Trim() + "' AND MZ_UNIT='" + UNIT.Trim() + "'";
                    }
                    if (CID_NAME.ToString().Trim() == "MZ_CODE")//休假假別
                    {
                        stringSQL = " SELECT * FROM (SELECT MZ_CODE AS MZ_KCODE,MZ_CNAME AS MZ_KCHI FROM C_DLCODE ORDER BY MZ_KCODE )  WHERE 1=1 ";
                        strSQL1 = " SELECT * FROM (SELECT MZ_CODE AS MZ_KCODE,MZ_CNAME AS MZ_KCHI FROM C_DLCODE ORDER BY MZ_KCODE )  WHERE 1=1 ";
                    }
                    if (CID_NAME.Trim() == "SCH")
                    {
                        stringSQL = " SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' AND (dbo.SUBSTR(MZ_KCODE,10,1)='Q' OR dbo.SUBSTR(MZ_KCODE,10,1)='R' OR dbo.SUBSTR(MZ_KCODE,10,1)='U' OR dbo.SUBSTR(MZ_KCODE,10,1)='X' OR dbo.SUBSTR(MZ_KCODE,10,1)='Y' OR dbo.SUBSTR(MZ_KCODE,10,1)='T') ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = " SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='ORG' AND (dbo.SUBSTR(MZ_KCODE,10,1)='Q' OR dbo.SUBSTR(MZ_KCODE,10,1)='R' OR dbo.SUBSTR(MZ_KCODE,10,1)='U' OR dbo.SUBSTR(MZ_KCODE,10,1)='X' OR dbo.SUBSTR(MZ_KCODE,10,1)='Y' OR dbo.SUBSTR(MZ_KCODE,10,1)='T') ORDER BY MZ_KCODE) WHERE 1=1";
                    }

                    // 權限用的，只出新北市 by 介入 2010/12/22
                    if (CID_NAME.Trim() == "ADAU")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213') WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213') WHERE 1=1";
                    }
                }
                else if (!(string.IsNullOrEmpty(MZ_KTYPE)))
                {
                    if (MZ_KTYPE == "PRID")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_AD AS MZ_KCODE,MZ_PRID AS MZ_KCHI FROM A_CHKAD ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_AD AS MZ_KCODE,MZ_PRID AS MZ_KCHI FROM A_CHKAD ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "DUTYMODE")
                    {
                        stringSQL = "SELECT * FROM (SELECT DUTYMODE_NO AS MZ_KCODE,DUTYMODE_NAME AS MZ_KCHI FROM C_DUTYMODE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"] + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT DUTYMODE_NO AS MZ_KCODE,DUTYMODE_NAME AS MZ_KCHI FROM C_DUTYMODE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"] + "' ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "DUTYPATROL")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_DUTYPATROL_NO AS MZ_KCODE,MZ_DUTYPATROL AS MZ_KCHI FROM C_DUTYPATROL WHERE MZ_AD='" + Session["ADPMZ_AD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_UNIT"] + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_DUTYPATROL_NO AS MZ_KCODE,MZ_DUTYPATROL AS MZ_KCHI FROM C_DUTYPATROL WHERE MZ_AD='" + Session["ADPMZ_AD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_UNIT"] + "' ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "DUTYTARGET")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_DUTYTARGET_NO AS MZ_KCODE,MZ_DUTYTARGET AS MZ_KCHI FROM C_DUTYTARGET WHERE MZ_AD='" + Session["ADPMZ_AD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_UNIT"] + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_DUTYTARGET_NO AS MZ_KCODE,MZ_DUTYTARGET AS MZ_KCHI FROM C_DUTYTARGET WHERE MZ_AD='" + Session["ADPMZ_AD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_UNIT"] + "' ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "PROLNO")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_PROLNO AS MZ_KCODE,MZ_PRONAME AS MZ_KCHI FROM A_PROLNO ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_PROLNO AS MZ_KCODE,MZ_PRONAME AS MZ_KCHI FROM A_PROLNO ORDER BY MZ_KCODE) WHERE 1=1";

                    }
                    else if (MZ_KTYPE == "PCODEM")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_POLNO AS MZ_KCODE,MZ_PNAME AS MZ_KCHI FROM A_POLNUM ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_POLNO AS MZ_KCODE,MZ_PNAME AS MZ_KCHI FROM A_POLNUM ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "NOTE")
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_NOTE AS MZ_KCODE,MZ_NOTE_NAME AS MZ_KCHI FROM A_NOTE WHERE MZ_NOTE='" + Session["ADPMZ_ID"].ToString() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_NOTE AS MZ_KCODE,MZ_NOTE_NAME AS MZ_KCHI  FROM A_NOTE WHERE MZ_NOTE='" + Session["ADPMZ_ID"].ToString() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "STATIC")
                    {
                        stringSQL = "SELECT * FROM (SELECT STATIC_NO AS MZ_KCODE,STATIC_NAME AS MZ_KCHI FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='" + FIRST.Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT STATIC_NO AS MZ_KCODE,STATIC_NAME AS MZ_KCHI FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='" + FIRST.Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                    else if (MZ_KTYPE == "24")
                    {
                        if (!(string.IsNullOrEmpty(FIRST)))
                        {
                            if (FIRST == "A")
                            {
                                stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' " +
                                                           " AND (dbo.SUBSTR(MZ_KCODE,1,1)='4' OR dbo.SUBSTR(MZ_KCODE,1,1)='7' OR dbo.SUBSTR(MZ_KCODE,1,1)='8'" +
                                                           " OR dbo.SUBSTR(MZ_KCODE,1,1)='9') ORDER BY MZ_KCODE) WHERE 1=1";
                               
                                strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' " +
                                                             " AND (dbo.SUBSTR(MZ_KCODE,1,1)='4' OR dbo.SUBSTR(MZ_KCODE,1,1)='7' OR dbo.SUBSTR(MZ_KCODE,1,1)='8'" +
                                                             " OR dbo.SUBSTR(MZ_KCODE,1,1)='9') ORDER BY MZ_KCODE) WHERE 1=1";

                            }
                            else
                            {
                                stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' " +
                                                             " AND (dbo.SUBSTR(MZ_KCODE,1,1)='2' OR dbo.SUBSTR(MZ_KCODE,1,1)='5' OR dbo.SUBSTR(MZ_KCODE,1,1)='6' " +
                                                             " OR dbo.SUBSTR(MZ_KCODE,1,1)='A' OR dbo.SUBSTR(MZ_KCODE,1,1)='B') ORDER BY MZ_KCODE) WHERE 1=1";
                                

                               strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' " +
                                                             " AND (dbo.SUBSTR(MZ_KCODE,1,1)='2' OR dbo.SUBSTR(MZ_KCODE,1,1)='5' OR dbo.SUBSTR(MZ_KCODE,1,1)='6' " +
                                                             " OR dbo.SUBSTR(MZ_KCODE,1,1)='A' OR dbo.SUBSTR(MZ_KCODE,1,1)='B') ORDER BY MZ_KCODE) WHERE 1=1"; ;


                            }
                        }
                        else
                        {
                            stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='" + MZ_KTYPE.Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                            

                            strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='" + MZ_KTYPE.Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        }
                    }
                    else if (MZ_KTYPE == "09")
                    {
                        //stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09'  AND (dbo.SUBSTR(MZ_KCODE,1,1)='B' OR dbo.SUBSTR(MZ_KCODE,1,1)='G'  OR dbo.SUBSTR(MZ_KCODE,1,1)='P')" +
                        //                             "UNION ALL " +
                        //                             "SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09'  AND dbo.SUBSTR(MZ_KCODE,1,1)!='B' AND dbo.SUBSTR(MZ_KCODE,1,1)!='G'  AND dbo.SUBSTR(MZ_KCODE,1,1)!='P') WHERE 1=1";


                        stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09'  )  WHERE 1=1";



                        strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09'  )  WHERE 1=1";

                    }
                    else if (MZ_KTYPE == "EXK")
                    {
                        //stringSQL = "SELECT * FROM (SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='EXK'  AND dbo.SUBSTR(MZ_KCODE,1,1)='G' " +
                        //                             "UNION ALL " +
                        //                             "SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='EXK' AND dbo.SUBSTR(MZ_KCODE,1,1)!='G') ORDER BY MZ_KCODE) WHERE 1=1";


                        stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='EXK'    ORDER BY MZ_KCODE       ) WHERE 1=1";


                        strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='EXK'    ORDER BY MZ_KCODE       ) WHERE 1=1";
                    }
                    else
                    {
                        stringSQL = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='" + MZ_KTYPE.Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        //ViewState["SelectCommand1"] = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='" + ViewState["MZ_KTYPE"].ToString().Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                        strSQL1 = "SELECT * FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='" + MZ_KTYPE.Trim() + "' ORDER BY MZ_KCODE) WHERE 1=1";
                    }
                }

            //}

            SqlDataSource1.SelectCommand = stringSQL;
        }

        protected void ddlPageJump_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlPage = (DropDownList)GridView1.BottomPagerRow.FindControl("ddlPageJump");
            GridView1.PageIndex = ddlPage.SelectedIndex;
        }

        protected void Quick_Click(object sender, EventArgs e)
        {
            int intPageIndex = 0;
            LinkButton lkbtn = (LinkButton)sender;
            switch (lkbtn.Text.Trim())
            {
                case "上一頁":
                    if (GridView1.PageIndex > 1)
                        intPageIndex = GridView1.PageIndex - 1;
                    break;
                case "下一頁":
                    if (GridView1.PageIndex < GridView1.PageCount - 1)
                        intPageIndex = GridView1.PageIndex + 1;
                    break;
                case "最後頁":
                    intPageIndex = GridView1.PageCount - 1;
                    break;
            }
            GridView1.PageIndex = intPageIndex;
        }

        protected void PageSet(object gvQuery)
        {
            GridView gv = (GridView)gvQuery;
            Label lbPage = (Label)gv.BottomPagerRow.FindControl("lbAllPage");
            DropDownList ddlJumpPage = (DropDownList)gv.BottomPagerRow.FindControl("ddlPageJump");

            lbPage.Text = "共" + gv.PageCount.ToString() + "頁";

            for (int i = 1; i <= gv.PageCount; i++)
                ddlJumpPage.Items.Add(new ListItem(i.ToString()));
            if (gv.PageIndex == 0)
            {
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkFirst")).Enabled = false;
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkPrevious")).Enabled = false;
            }
            else if (gv.PageIndex == (gv.PageCount - 1))
            {
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkNext")).Enabled = false;
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkLast")).Enabled = false;
            }

            ddlJumpPage.SelectedIndex = gv.PageIndex;
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(stringSQL, "GET");

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料,請先新增資料');window.close();", true);
            }
            else
            {
                PageSet(GridView1);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //strSQL = ViewState["SelectCommand1"].ToString();
            strSQL1 += " AND MZ_KCODE LIKE '" + TextBox_MZ_KCODE.Text.Trim() + "%' ORDER BY MZ_KCODE";
            stringSQL = strSQL1;
            SqlDataSource1.SelectCommand = strSQL1;
            GridView1.DataBind();
        }

        protected void btKCHI_Search_Click(object sender, EventArgs e)
        {

            //strSQL = ViewState["SelectCommand1"].ToString();
            strSQL1 += " AND MZ_KCHI LIKE '%" + TextBox_MZ_KCHI.Text.Trim() + "%' ORDER BY MZ_KCHI";
            stringSQL = strSQL1;
            SqlDataSource1.SelectCommand = strSQL1;
            GridView1.DataBind();

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                if (!(string.IsNullOrEmpty(MZ_KTYPE)))
                {
                    if (MZ_KTYPE == "DUTYTARGET" || MZ_KTYPE == "DUTYPATROL" || MZ_KTYPE == "NOTE" || MZ_KTYPE == "PRID")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["KTYPE_CID"] + "').value='" + this.GridView1.SelectedRow.Cells[2].Text.Trim().Replace("\r\n", "\\r\\n") + "';window.close();", true);
                    }
                    else if (MZ_KTYPE == "22")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["KTYPE_CID"] + "').value='" + this.GridView1.SelectedRow.Cells[1].Text.Trim() +
                                                                                                       "';window.opener.document.getElementById('" + Session["KTYPE_CID1"] + "').value='" + this.GridView1.SelectedRow.Cells[2].Text.Trim() +
                                                                                                       "';window.opener.__doPostBack('" + Session["KTYPE_CID"] + "','');window.close();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["KTYPE_CID"] + "').value='" + this.GridView1.SelectedRow.Cells[1].Text.Trim() +
                                                                                                       "';window.opener.document.getElementById('" + Session["KTYPE_CID1"] + "').value='" + this.GridView1.SelectedRow.Cells[2].Text.Trim() +
                                                                                                       "';window.opener.__doPostBack('" + Session["KTYPE_CID"] + "','');window.close();", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["KTYPE_CID"] + "').value='" + this.GridView1.SelectedRow.Cells[1].Text.Trim() +
                                                                                                   "';window.opener.document.getElementById('" + Session["KTYPE_CID1"] + "').value='" + this.GridView1.SelectedRow.Cells[2].Text.Trim() +
                                                                                                   "';window.opener.__doPostBack('" + Session["KTYPE_CID"] + "','');window.close();", true);
                }
            }
        }
    }
}
