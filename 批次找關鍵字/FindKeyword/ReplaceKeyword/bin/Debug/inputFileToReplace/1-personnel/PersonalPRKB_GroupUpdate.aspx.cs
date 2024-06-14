using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PRKB_GroupUpdate : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            A.set_Panel_EnterToTAB(ref this.Panel1);

            TextBox_MZ_NO.Text = Request["MZ_NO"].ToString();

        }
        

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            
            //2013/08/08 似乎有例外擋不住
            string ErrorString = "";
            
            if (TextBox_MZ_PRCT.Text.Length > 50)
            {
                ErrorString += "獎懲內容不能超過50個字" + "\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_PROLNO.Text))
            {
                ErrorString += "獎懲依據不可為空白" + "\\r\\n";
            }
            else
            {

                string check_PROLNO = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM " +
                   " (SELECT * FROM(SELECT MZ_PROLNO AS MZ_KCODE,MZ_PRONAME AS MZ_KCHI FROM A_PROLNO ORDER BY MZ_KCODE) WHERE 1=1)" +
                    " WHERE MZ_KCODE='" + TextBox_MZ_PROLNO.Text + "'");

                if (string.IsNullOrEmpty(check_PROLNO))
                {
                    ErrorString += "獎懲依據錯誤，請重新選擇" + "\\r\\n";

                }
            }
           

            if (string.IsNullOrEmpty(DropDownList_MZ_PCODE.SelectedValue))
            {
                ErrorString += "是否配分不可為空白" + "\\r\\n";
            }
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "');", true);
                return;
            }
            //2013/08/08 似乎有例外擋不住





            string strSQL1 = "";
            string strSQL2 = "";

            if (!string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()))
            {
                strSQL1 += " AND MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_OCCC.Text.Trim()))
            {
                strSQL1 += " AND MZ_OCCC='" + TextBox_MZ_OCCC.Text.ToString() + "'";
            }
            //20140407小隊長說先解掉
            //switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            //{
            //    case "A":
            //    case "B":

            //        break;
            //    case "C":
            //        strSQL2 = " AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'";
            //        break;

            //    case "D":
            //    case "E":
            //    default:
            //        strSQL2 = " AND MUSER='" + Session["ADPMZ_ID"].ToString() + "'";
            //        break;
            //}

            //20141203
            //因新增時案號會有「-」符號字串，若刪除會導致查無資料，故拿掉tosql函式。 20180516 by sky
            //string selectString = "SELECT COUNT(MZ_NO) FROM A_PRKB WHERE MZ_NO='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "' AND MZ_PRRST='" + TextBox_MZ_PRRST.Text.Trim() + "'" + strSQL1 + strSQL2;
            string selectString = "SELECT COUNT(MZ_NO) FROM A_PRKB WHERE MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "' AND MZ_PRRST='" + TextBox_MZ_PRRST.Text.Trim() + "'" + strSQL1 + strSQL2;

            //DataTable temp = new DataTable();

            string temp = o_DBFactory.ABC_toTest.vExecSQL(selectString);

            if (temp == "0")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料或無權限！');", true);
            }
            else
            {
                //2013/
                string strSQL = "UPDATE A_PRKB SET MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "'";//可能拿來1=1一樣當頭
                if (!string.IsNullOrEmpty(TextBox_MZ_PRCT.Text.Trim()))
                {
                    strSQL += ",MZ_PRCT='" + o_str.tosql(TextBox_MZ_PRCT.Text.Trim()) + "'";
                }

                if (!string.IsNullOrEmpty(TextBox_MZ_PROLNO.Text.Trim()))
                {
                    strSQL += ",MZ_PROLNO='" + TextBox_MZ_PROLNO.Text.Trim() + "'";
                }

                if (!string.IsNullOrEmpty(DropDownList_MZ_PCODE.Text.Trim()))
                {
                    strSQL += ",MZ_PCODE='" + DropDownList_MZ_PCODE.SelectedValue.ToString() + "'";
                }

                if (!string.IsNullOrEmpty(TextBox_MZ_PCODEM.Text.Trim()))
                {
                    strSQL += ",MZ_PCODEM='" + o_str.tosql(TextBox_MZ_PCODEM.Text.Trim()) + "'";
                }

                strSQL += "WHERE " +
                                 "MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                           "' AND MZ_PRRST='" + TextBox_MZ_PRRST.Text.Trim() +
                           //"' AND (MZ_SWT4 IS NULL OR MZ_SWT4<>'Y') " +
                           "' AND (MZ_SWT4 !='Y') " +
                           // "  AND (MZ_SWT1<>'Y' OR MZ_SWT1 IS NULL)";
                           "  AND (MZ_SWT1 !='Y' )";

                strSQL += strSQL1 + strSQL2;

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('整批修改成功！');window.opener.location.href='Personal2-1.aspx?Personl_PRKB_SWT4_MZ_NO=" + TextBox_MZ_NO.Text.ToString() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                catch (Exception)
                {
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "P-GUPDATE", TPMPermissions._boolTPM(), strSQL) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "P-GUPDATE", strSQL);
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('整批修改失敗！');", true);
                }

            }

        }
        protected void TextBox_MZ_NO_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_NO FROM A_PRKB WHERE MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "'")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無此案號')", true);
                return;
            }
        }

        protected void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object tb3)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;

                if (tb3 is Button)
                {
                    (tb3 as Button).Focus();
                }
                else if (tb3 is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (tb3 as TextBox).ClientID + "').focus();$get('" + (tb3 as TextBox).ClientID + "').focus();", true);
                    // (tb3 as TextBox).Focus();
                }
                else if (tb3 is DropDownList)
                {
                    (tb3 as DropDownList).Focus();
                }
            }
        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btPRCT_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PRCT.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btPROLNO_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PROLNO.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PROLNO1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PROLNO&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btOCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_OCCC, TextBox_MZ_OCCC1, "26");
        }

        protected void btPRRST_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PRRST.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PRRST1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=24&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btPCODEM_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PCODEM.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PCODEM1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PCODEM&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_PROLNO_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME FROM A_PROLNO WHERE MZ_PROLNO='" + o_str.tosql(TextBox_MZ_PROLNO.Text.Trim().ToUpper()) + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_PROLNO1, TextBox_MZ_PROLNO, DropDownList_MZ_PCODE);
        }

        protected void TextBox_MZ_PCODEM_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + o_str.tosql(TextBox_MZ_PCODEM.Text.Trim().ToUpper()) + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_PCODEM1, TextBox_MZ_PCODEM, Button1);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim().ToUpper()) + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_OCCC);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_PRRST);
        }

        protected void TextBox_MZ_PRRST_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRRST.Text, "24");

            Ktype_Cname_Check(CName, TextBox_MZ_PRRST1, TextBox_MZ_PRRST, TextBox_MZ_PRCT);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string strSQL1 = "";

            if (!string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()))
            {
                strSQL1 += " AND MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_OCCC.Text.Trim()))
            {
                strSQL1 += " AND MZ_OCCC='" + o_str.tosql(TextBox_MZ_OCCC.Text.ToString()) + "'";
            }

            GridView1.Visible = true;

            string strSQL = "SELECT " +
                                     "MZ_NO,MZ_ID,MZ_NAME," +
                                     "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC," +
                                     "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_PRRST AND MZ_KTYPE='24') AS MZ_PRRST " +
                             "FROM " +
                                     "A_PRKB " +
                             "WHERE " +
                                     "MZ_NO='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) +
                                     "' AND MZ_PRRST='" + o_str.tosql(TextBox_MZ_PRRST.Text.Trim()) +
                                     "' AND (MZ_SWT4 IS NULL OR MZ_SWT4<>'Y')" +
                                     "  AND (MZ_SWT1!='Y' )";


            strSQL += strSQL1;

            SqlDataSource1.SelectCommand = strSQL;

            GridView1.DataBind();
        }

        protected void DropDownList_MZ_PCODE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_MZ_PCODE.SelectedIndex == 1)
            {
                TextBox_MZ_PCODEM.Enabled = true;
                btPCODEM.Enabled = true;
                TextBox_MZ_PCODEM1.Enabled = true;
            }
            else
            {
                TextBox_MZ_PCODEM.Enabled = false;
                btPCODEM.Enabled = false;
                TextBox_MZ_PCODEM1.Enabled = false;
            }
        }
    }
}
