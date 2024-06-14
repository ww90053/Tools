using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Text;

using TPPDDB._2_salary;
using TPPDDB.Model.Const;

namespace TPPDDB._1_personnel
{
    public partial class Personal1_1_BankSet : System.Web.UI.Page
    {

        private string strBANKID_Data
        {
            get
            {
                return DropDownList_BANK.SelectedValue;
            }
            set
            {
                if (value == "")
                    DropDownList_BANK.SelectedIndex = 0;
                else
                    DropDownList_BANK.SelectedValue = value;
            }
        }

        private string strSTOCKPILE_BANKID_Data
        {
            get
            {
                return TextBox_STOCKPILE_BANKID.Text;
            }
            set
            {
                TextBox_STOCKPILE_BANKID.Text = value;
            }
        }

        string GroupPermission { get { return SalaryPublic.GetGroupPermission(); } }

        string searchSQL
        {
            get { return ViewState["searchSQL"].ToString(); }
            set { ViewState["searchSQL"] = value; }
        }

        private static string _strGROUP_Data;
        private string strGROUP_Data
        {
            get
            {
                return DropDownList_GROUP.SelectedValue;
            }
            set
            {
                _strGROUP_Data = value;
                DropDownList_GROUP.SelectedValue = value;
            }
        }


        Police police
        {
            get { return (Police)ViewState["police"]; }
            set { ViewState["police"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //檢查權限
                //SalaryPublic.checkPermission();

                A.check_power();

                string strSQL_Account = "SELECT TPMUSER FROM TPM_MEMBER WHERE TPM_IDNO='" + Session["Search_ID"] + "'";

                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL_Account, "get");

                if (temp.Rows.Count > 0)
                {
                    txt_Account.Text = temp.Rows[0][0].ToString();
                }

                else
                {
                    txt_Account.Text = "無帳號";
                }

                string sql = "SELECT * FROM VW_A_DLBASE WHERE MZ_ID='" + Session["Search_ID"] + "'";

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(sql, "get");

                if (dt.Rows.Count > 0)
                {
                    TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString().Trim();
                    TextBox_MZ_ID.Text = dt.Rows[0]["MZ_ID"].ToString().Trim();
                    TextBox_PAY_AD.Text = dt.Rows[0]["PAY_AD"].ToString().Trim();
                    TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString().Trim();
                    TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString().Trim();
                    TextBox_MZ_EXAD.Text = dt.Rows[0]["MZ_EXAD"].ToString().Trim();
                    TextBox_MZ_EXUNIT.Text = dt.Rows[0]["MZ_EXUNIT"].ToString().Trim();

                    TextBox_PAY_AD1.Text = dt.Rows[0]["PAY_AD_CH"].ToString().Trim();
                    TextBox_MZ_AD1.Text = dt.Rows[0]["MZ_AD_CH"].ToString().Trim();
                    TextBox_MZ_UNIT1.Text = dt.Rows[0]["MZ_UNIT_CH"].ToString().Trim();
                    TextBox_MZ_EXAD1.Text = dt.Rows[0]["MZ_EXAD_CH"].ToString().Trim();
                    TextBox_MZ_EXUNIT1.Text = dt.Rows[0]["MZ_EXUNIT_CH"].ToString().Trim();
                }


                btCreate.Enabled = true;
                btUpdate.Enabled = false;

                btExit.Enabled = true;
                btBack_Data.Enabled = false;
                btNext_Data.Enabled = false;


                MakeDataList();
                ShowData();
            }

            PoliceSearchPanel1.postbacked += new TPPDDB._2_salary.UserControl.PoliceSearchPanel.Postbacked(PoliceSearchPanel1_postbacked);
            PoliceSearchPanel1.searchClicked += new TPPDDB._2_salary.UserControl.PoliceSearchPanel.SearchClicked(PoliceSearchPanel1_searchClicked);
        }

        void PoliceSearchPanel1_searchClicked()
        {
            MakeDataList();
            ShowData();
        }

        // 呈現資料
        private void ShowData()
        {
            if (ViewState["CurrentIndex"] == null)
                return;

            List<string> listData = (List<string>)ViewState["DataList"];
            int intCurrentIndex = (int)ViewState["CurrentIndex"];

            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;

            if (listData.Count == 0)
            {
                return;
            }

            // 避免輸入的索引大於資料總數
            if ((intCurrentIndex + 1) > listData.Count)
            {
                intCurrentIndex = listData.Count - 1;
                ViewState["CurrentIndex"] = intCurrentIndex;
            }

            if (listData.Count > 1)
            {
                // 不在第一筆的時候
                if (intCurrentIndex != 0)
                    btBack_Data.Enabled = true;

                // 不在最後一筆的時候
                if (intCurrentIndex != listData.Count - 1)
                    btNext_Data.Enabled = true;
            }

            finddata(intCurrentIndex);

            // A權限不受限
            if (GroupPermission == "A")
                return;
        }

        // 產生查詢的結果清單
        private void MakeDataList()
        {
            string strID;
            string strMZ_POLNO;
            string strMZ_NAME;
            string strPAY_AD;
            string strPAY_UNIT;

            //strID = PoliceSearchPanel1.idcard; ;
            //strMZ_POLNO = PoliceSearchPanel1.polno;
            //strMZ_NAME = PoliceSearchPanel1.name;
            //strPAY_AD = PoliceSearchPanel1.ad;
            //strPAY_UNIT = PoliceSearchPanel1.unit;


            strID = TextBox_MZ_ID.Text;
            //strMZ_POLNO = PoliceSearchPanel1.polno;
            //strMZ_NAME = Session[ConstSession.nam].ToString();
            strPAY_AD = TextBox_PAY_AD.Text;
            strPAY_UNIT = TextBox_MZ_EXUNIT.Text;

            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;

            string strSQL = @"SELECT AKP.MZ_KCHI PAY_AD, MZ_ID IDCARD, MZ_POLNO, MZ_NAME NAME, AKO.MZ_KCHI CHIOCCC FROM A_DLBASE 
                                LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE = A_DLBASE.PAY_AD AND AKP.MZ_KTYPE = '04' 
                                LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE = A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE = '26' WHERE 1=1 ";

            // 用在匯出excel的
            //irk 增加欄位 發薪機關、現服務單位、員工編號、職稱、姓名、身份證號、薪俸職等、俸階、俸點、考績等次、戶籍地址、現住地址
            string sql = @"SELECT AKP.MZ_KCHI AD, AKU.MZ_KCHI UNIT, MZ_POLNO POLNO, AKO.MZ_KCHI OCCC, MZ_NAME NAME, MZ_ID ID,MZ_SRANK,AKSR.MZ_KCHI SRANK
                                  ,MZ_SLVC,AKS.MZ_KCHI SLVC,MZ_SPT, '' SRANKNEW, '' SLVCNEW, '' SPTNEW,GRADE,MZ_ADD1,MZ_ADD2
                            FROM A_DLBASE 
                                LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE = A_DLBASE.PAY_AD AND AKP.MZ_KTYPE = '04'
                                LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE = A_DLBASE.MZ_EXUNIT AND AKU.MZ_KTYPE = '25'
                                LEFT JOIN A_KTYPE AKR ON AKR.MZ_KCODE = A_DLBASE.MZ_SRANK AND AKR.MZ_KTYPE = '09'
                                LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE = A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE = '26'
                                LEFT JOIN A_KTYPE AKS ON AKS.MZ_KCODE = A_DLBASE.MZ_SLVC AND AKS.MZ_KTYPE = '64' 
			                    LEFT JOIN A_KTYPE AKSR ON AKSR.MZ_KCODE = A_DLBASE.MZ_SRANK AND AKSR.MZ_KTYPE = '09'
                                LEFT JOIN B_BASE ON A_DLBASE.MZ_ID=B_BASE.IDCARD
                            WHERE 1=1";


            if (strID != "")
            {
                strSQL = strSQL + " AND MZ_ID = '" + strID + "'";
                sql += " AND MZ_ID = '" + strID + "'";
            }

            //if (strMZ_POLNO != "")
            //{
            //    strSQL = strSQL + " AND MZ_POLNO = '" + strMZ_POLNO + "'";
            //    sql += " AND MZ_POLNO = '" + strMZ_POLNO + "'";
            //}

            //if (strMZ_NAME != "")
            //{
            //    strSQL = strSQL + " AND MZ_NAME LIKE N'%" + strMZ_NAME + "%'";
            //    sql += " AND MZ_NAME LIKE N'%" + strMZ_NAME + "%'";
            //}

            if (strPAY_AD != "")
            {
                strSQL = strSQL + " AND PAY_AD = '" + strPAY_AD + "'";
                sql += " AND PAY_AD = '" + strPAY_AD + "'";
            }

            //if (strPAY_UNIT != "")// 小隊長需求：strPAY_UNIT做為員工編號前4碼的判斷 2012.04.11
            //{
            //    strSQL = strSQL + " AND dbo.SUBSTR(MZ_POLNO, 1, 4) LIKE '" + strPAY_UNIT + "%'";
            //    sql += " AND dbo.SUBSTR(MZ_POLNO, 1, 4)  LIKE '" + strPAY_UNIT + "%'";
            //}

            strSQL = strSQL + " ORDER BY MZ_POLNO";
            sql += " ORDER BY MZ_POLNO";

            searchSQL = sql;

            ViewState["DataList"] = o_DBFactory.ABC_toTest.DataListArray(strSQL, "IDCARD");
            //UserSelector1.SetData(o_DBFactory.ABC_toTest.DataSelect(strSQL));

            if (((List<string>)ViewState["DataList"]).Count == 0)
            {
                //lockControls();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料！');", true);
            }

            ViewState["CurrentIndex"] = 0;
        }

        protected void finddata(int Datacount)
        {
            List<string> dataNext = (List<string>)ViewState["DataList"];
            this.police = new Police(dataNext[Datacount]);

            //銀行帳號
            voidSTOCKPILE_SQL_Command();

        }

        void PoliceSearchPanel1_postbacked()
        {
            btn_showSearch_ModalPopupExtender.Show();
        }

        #region 銀行帳戶

        private void voidBASE_STOCKPILE_Clear()
        {
            strBANKID_Data = "001";
            strSTOCKPILE_BANKID_Data = "";
            DropDownList_GROUP.DataBind();
        }

        private void voidSTOCKPILE_SQL_Command()
        {
            GridView_STOCKPILE.DataSource = police.accountInfo;
            GridView_STOCKPILE.DataBind();
        }

        protected void GridView_STOCKPILE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ((GridView)sender).PageIndex = e.NewPageIndex;
            voidSTOCKPILE_SQL_Command();
        }

        protected void GridView_STOCKPILE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            string payAD = SalaryPublic.strLoginEXAD;

            if (((GridView)sender).DataKeys[e.Row.RowIndex].Values["PAY_AD"].ToString() != payAD)
            {
                ((Button)e.Row.Cells[0].FindControl("btnSelect")).Enabled = false;
                ((Button)e.Row.Cells[5].FindControl("btn_DeleteAccount")).Enabled = false;
            }
        }

        protected void GridView_STOCKPILE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                if (e.CommandName == "btSelect")
                {
                    btUpdate.Enabled = true;


                    //int index = Convert.ToInt32(e.CommandArgument);
                    string strKeyword = e.CommandArgument.ToString();

                    Session["B_STOCKPILE_Keyword"] = strKeyword;

                    string strSQL = "SELECT BS_SNID, IDCARD, BANKID, STOCKPILE_BANKID, \"GROUP\" FROM B_BASE_STOCKPILE WHERE BS_SNID = '" + strKeyword + "' ORDER BY \"GROUP\"";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        strBANKID_Data = dr["BANKID"].ToString();
                        strSTOCKPILE_BANKID_Data = dr["STOCKPILE_BANKID"].ToString();
                        //strGROUP_Data = dr["GROUP"].ToString();

                        btCreate.Enabled = false;
                        btExit.Enabled = true;
                    }
                }
                else
                {
                    string strKeyword = e.CommandArgument.ToString();

                    if (SalaryBasic.boolSalaryPay_Stockpile_Set_Delete(strKeyword))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除完成');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除失敗');", true);
                    }
                    
                    voidSTOCKPILE_SQL_Command();
                    voidBASE_STOCKPILE_Clear(); 

                    MakeDataList();
                    ShowData();
                }
                Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
            }
        }

        #endregion


        protected void btTable_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../2-salary/B_SalaryBasic1.aspx?TPM_FION=232&reID=BASIC','查詢','top=80,left=20,width=400,height=350,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            if (ViewState["CurrentIndex"] == null)
                return;

            List<string> dataNext = (List<string>)ViewState["DataList"];
            int intCurrentIndex = (int)ViewState["CurrentIndex"];
            string strID = dataNext[intCurrentIndex];

            //irk 4/18 增加 銀行帳號驗証
            if (!validDataAccount())
                return;

            //銀行帳戶
            if (SalaryBasic.boolCHK_STOCKPILE_Data(this.TextBox_MZ_ID.Text, strGROUP_Data, SalaryPublic.strLoginEXAD))
            {
                if (SalaryBasic.boolSalaryPay_Stockpile_Set_Create(strID, strBANKID_Data, strSTOCKPILE_BANKID_Data, int.Parse(strGROUP_Data), SalaryPublic.strLoginEXAD))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增完成');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增失敗');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('該群組已有帳戶');", true);
            }

            ShowData();
        }

        //4/18 Irk增加 銀行帳號驗証
        //驗證必填欄位
        bool validDataAccount()
        {
            string error = "";

            if (strSTOCKPILE_BANKID_Data.Trim().Length == 0)
                error += "銀行帳號不可空白\\r\\n";

            if (error.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + error + "');", true);
                return false;
            }

            return true;
        }

        protected void btBack_Data_Click(object sender, EventArgs e)
        {
            int intCurrentIndex = (int)ViewState["CurrentIndex"];

            intCurrentIndex--;

            ViewState["CurrentIndex"] = intCurrentIndex;

            ShowData();
        }

        protected void btNext_Data_Click(object sender, EventArgs e)
        {
            int intCurrentIndex = (int)ViewState["CurrentIndex"];

            intCurrentIndex++;

            ViewState["CurrentIndex"] = intCurrentIndex;

            ShowData();
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (SalaryBasic.boolSalaryPay_Stockpile_Set_Update(Session["B_STOCKPILE_Keyword"].ToString(), strBANKID_Data, strSTOCKPILE_BANKID_Data, int.Parse(strGROUP_Data), SalaryPublic.strLoginEXAD))
            {       
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改完成');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改失敗');", true);
            }

            voidSTOCKPILE_SQL_Command();
            btCreate.Enabled = true;
            btUpdate.Enabled = false;            
        }
    }
}