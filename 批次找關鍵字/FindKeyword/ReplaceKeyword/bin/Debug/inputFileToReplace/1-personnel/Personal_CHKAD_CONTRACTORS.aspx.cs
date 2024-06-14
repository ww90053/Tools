using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Drawing;

namespace TPPDDB._1_personnel
{
    public partial class Personal_CHKAD_CONTRACTORS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
            

            //by MQ 20100311---------
            A.set_Panel_EnterToTAB(ref this.Panel1);
           
                string strSQL = "SELECT * FROM A_CHKAD_CONTRACTORS WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");


                if (temp.Rows.Count == 1)
                {
                    TextBox_MZ_CHKAD.Text = temp.Rows[0]["MZ_CHKAD"].ToString();
                    lbl_MZ_CHKAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_CHKAD.Text.Trim()) + "'");
                    TextBox_MZ_NAME.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'");
                    TextBox_MZ_NAME.Enabled = false;
                    Label1.Text = temp.Rows[0]["MZ_CHKAD"].ToString();
                    TextBox_MZ_EMAIL.Text = temp.Rows[0]["MZ_EMAIL"].ToString();
                    TextBox_MZ_FAXNO.Text = temp.Rows[0]["MZ_FAXNO"].ToString();
                    TextBox_MZ_TELNO.Text = temp.Rows[0]["MZ_TELNO"].ToString();
                }
                else
                {
                    TextBox_MZ_NAME.Text = o_A_DLBASE.CNAME(Session["ADPMZ_ID"].ToString());
                    TextBox_MZ_NAME.Enabled = false;
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CHKAD.ClientID + "').focus();$get('" + TextBox_MZ_CHKAD.ClientID + "').focus();", true);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSQL = "";
          
            string mzid = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_CHKAD_CONTRACTORS WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'");

            if (!string.IsNullOrEmpty(mzid))
            {
                strSQL = "UPDATE A_CHKAD_CONTRACTORS SET MZ_CHKAD = @MZ_CHKAD,MZ_ID = @MZ_ID,MZ_TELNO = @MZ_TELNO,MZ_FAXNO = @MZ_FAXNO,MZ_EMAIL = @MZ_EMAIL WHERE  MZ_ID='" + Session["ADPMZ_ID"].ToString() + "' AND MZ_CHKAD = '" + Label1.Text + "'";
            }
            else
            {
                strSQL = "INSERT INTO A_CHKAD_CONTRACTORS (MZ_CHKAD,MZ_ID,MZ_TELNO,MZ_FAXNO,MZ_EMAIL) VALUES (@MZ_CHKAD,@MZ_ID,@MZ_TELNO,@MZ_FAXNO,@MZ_EMAIL) ";
            }

            SqlParameter[] parameterList = {
            new SqlParameter("MZ_CHKAD",SqlDbType.VarChar){Value = TextBox_MZ_CHKAD.Text.Trim ()},
            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = Session["ADPMZ_ID"].ToString()},
            new SqlParameter("MZ_TELNO",SqlDbType.VarChar){Value = TextBox_MZ_TELNO.Text.Trim ()},
            new SqlParameter("MZ_FAXNO",SqlDbType.VarChar){Value = TextBox_MZ_FAXNO.Text.Trim ()},
            new SqlParameter("MZ_EMAIL",SqlDbType.VarChar){Value = TextBox_MZ_EMAIL.Text.Trim ()}
            };

            if (strSQL.IndexOf("INSERT") < 0)
            {
                //2010.06.07 by 伊珊
                string ErrorString = "";

                string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_CHKAD_CONTRACTORS WHERE MZ_CHKAD='" + TextBox_MZ_CHKAD.Text.Trim() + "' AND MZ_ID='"+mzid+"'");

                if (pkey_check != "0")
                {
                    ErrorString += "核定機關違反唯一值的條件" + "\\r\\n";
                    TextBox_MZ_CHKAD.BackColor = Color.Orange;
                }
                //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                //foreach (Object ob in Panel1.Controls)
                //{
                //    if (ob is TextBox)
                //    {
                //        TextBox tbox = (TextBox)ob;

                //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_CHKAD_CONTRACTORS", tbox.Text);

                //        if (!string.IsNullOrEmpty(result))
                //        {
                //            ErrorString += result + "\\r\\n";
                //            tbox.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            tbox.BackColor = Color.White;
                //        }
                //    }
                //}
                //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                if (!string.IsNullOrEmpty(ErrorString))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                    return;
                }
            }

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('成功');", true);
                //2010.06.04 LOG紀錄 by伊珊
                if (strSQL.IndexOf("INSERT") < 0)
                {
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));

                }
                else
                {
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('失敗');", true);
            }
        }

        protected void TextBox_MZ_CHKAD_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_CHKAD.Text = TextBox_MZ_CHKAD.Text.ToUpper();

            lbl_MZ_CHKAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_CHKAD.Text.Trim()) + "'");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TELNO.ClientID + "').focus();$get('" + TextBox_MZ_TELNO.ClientID + "').focus();", true);

        }
    }
}
