using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_And_Personal_Backup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {

                C.check_power();

                C.set_Panel_EnterToTAB(ref this.Panel1);
                foreach (object ob in Panel1.Controls)
                {
                    if (ob is DropDownList)
                    {
                        DropDownList dp = (DropDownList)ob;
                        dp.Items.Add((System.DateTime.Now.Year - 1911).ToString().PadLeft(3, '0'));
                        dp.Items.Add((System.DateTime.Now.Year - 1912).ToString().PadLeft(3, '0'));
                    }
                }
            }


        }

        protected void btDLBASE_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_DLBASE_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_DLBASE.SelectedItem.Text + "'");

            string InsertSQL = "INSERT INTO C_DLBASE_BK SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_DLBASE.SelectedItem.Text) + "',A_DLBASE.* FROM A_DLBASE ";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btPRKB_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_PRKB_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_PRKB.SelectedItem.Text + "'");
            string InsertSQL = "INSERT INTO C_PRKB_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_PRKB.SelectedItem.Text) + "',A_PRKB.* FROM A_PRKB WHERE dbo.SUBSTR(MDATE,1,3) = '" + DropDownList_MZ_YEAR_PRKB.SelectedItem.Text + "')";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btDLTB01_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_DLTB01_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_DLTB01.SelectedItem.Text + "'");
            string InsertSQL = "INSERT INTO C_DLTB01_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_DLTB01.SelectedItem.Text) + "',C_DLTB01.* FROM C_DLTB01 WHERE dbo.SUBSTR(MZ_IDATE1,1,3) ='" + DropDownList_MZ_YEAR_DLTB01.SelectedItem.Text + "')";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btPRK1_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_PRK1_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_PRK1.SelectedItem.Text + "'");

            string InsertSQL = "INSERT INTO C_PRK1_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_PRK1.SelectedItem.Text) + "',A_PRK1.* FROM A_PRK1  WHERE dbo.SUBSTR(MZ_DATE,1,3) ='" + DropDownList_MZ_YEAR_PRK1.SelectedItem.Text + "')";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btPRK2_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_PRK2_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_PRK2.SelectedItem.Text + "'");

            string InsertSQL = "INSERT INTO C_PRK2_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_PRK2.SelectedItem.Text) + "',A_PRK2.* FROM A_PRK2 WHERE dbo.SUBSTR(MZ_DATE,1,3) ='" + DropDownList_MZ_YEAR_PRK2.SelectedItem.Text + "')";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btPOSIT_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_POSIT_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_POSIT.SelectedItem.Text + "'");

            string InsertSQL = "INSERT INTO C_POSIT_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_POSIT.SelectedItem.Text) + "',A_POSIT.* FROM A_POSIT WHERE dbo.SUBSTR(MDATE,1,3) ='" + DropDownList_MZ_YEAR_POSIT.SelectedItem.Text + "')";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btPOSIT1_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_POSIT1_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_POSIT1.SelectedItem.Text + "'");

            string InsertSQL = "INSERT INTO C_POSIT1_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_POSIT1.SelectedItem.Text) + "',A_POSIT1.* FROM A_POSIT1 WHERE dbo.SUBSTR(MZ_DATE,1,3) ='" + DropDownList_MZ_YEAR_POSIT1.SelectedItem.Text + "')";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }
        }

        protected void btPOSIT2_BACKUP_Click(object sender, EventArgs e)
        {
            o_DBFactory.ABC_toTest.Edit_Data("DELETE C_POSIT2_BK WHERE MZ_YEAR = '" + DropDownList_MZ_YEAR_PRK1.SelectedItem.Text + "'");

            string InsertSQL = "INSERT INTO C_POSIT2_BK (SELECT '" + o_str.tosql(DropDownList_MZ_YEAR_PRK1.SelectedItem.Text) + "',A_POSIT2.* FROM A_POSIT2 WHERE dbo.SUBSTR(MZ_DATE,1,3) ='" + DropDownList_MZ_YEAR_PRK1.SelectedItem.Text + "')";
            try
            {

                o_DBFactory.ABC_toTest.Edit_Data(InsertSQL);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份完成');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('備份失敗');", true);
                throw;
            }

        }
    }
}
