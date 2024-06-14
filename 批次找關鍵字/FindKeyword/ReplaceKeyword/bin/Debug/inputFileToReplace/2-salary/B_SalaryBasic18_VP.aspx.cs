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
using System.Drawing;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryBasic18_VP : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        DataTable searchResult
        {
            get { return (DataTable)ViewState["searchResult"]; }
            set { ViewState["searchResult"] = value; }
        }

        #region 頁面變數

        string key
        {
            set { ViewState["MB_SNID"] = value; }
            get { return ViewState["MB_SNID"].ToString(); }
        }

        bool isNew
        {
            set { ViewState["isNew"] = value; }
            get { return (bool)ViewState["isNew"]; }
        }

        private string strIDCARD
        {
            get
            {
                return TextBox_IDCARD.Text.Trim().ToUpper();
            }
            set
            {
                TextBox_IDCARD.Text = value;
            }
        }

        private string strNAME
        {
            get
            {
                return TextBox_NAME.Text;
            }
            set
            {
                TextBox_NAME.Text = value;
            }
        }

        private string strADDRESS1
        {
            get
            {
                return TextBox_ADDRESS1.Text;
            }
            set
            {
                TextBox_ADDRESS1.Text = value;
            }
        }

        private string strADDRESS2
        {
            get
            {
                return TextBox_ADDRESS2.Text;
            }
            set
            {
                TextBox_ADDRESS2.Text = value;
            }
        }

        private string strRENTNUM
        {
            get
            {
                return TextBox_RENTNUM.Text;
            }
            set
            {
                TextBox_RENTNUM.Text = value;
            }
        }

        private string strCELLPHONE
        {
            get
            {
                return TextBox_CELLPHONE.Text;
            }
            set
            {
                TextBox_CELLPHONE.Text = value;
            }
        }

        private string strTEL
        {
            get
            {
                return TextBox_TEL.Text;
            }
            set
            {
                TextBox_TEL.Text = value;
            }
        }

        private string strEXT
        {
            get
            {
                return TextBox_EXT.Text;
            }
            set
            {
                TextBox_EXT.Text = value;
            }
        }

        private string strBANK_CODE
        {
            get
            {
                return DropDownList_BANK_CODE.SelectedValue;
            }
            set
            {
                try
                {
                    DropDownList_BANK_CODE.SelectedValue = value;
                }
                catch
                {
                    DropDownList_BANK_CODE.SelectedIndex = 0;
                }
            }
        }

        private string strBANK_ID
        {
            get
            {
                return TextBox_BANK_ID.Text;
            }
            set
            {
                TextBox_BANK_ID.Text = value;
            }
        }

        private string strNOTE
        {
            get
            {
                return TextBox_NOTE.Text;
            }
            set
            {
                TextBox_NOTE.Text = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                TextBox_IDCARD.BackColor = Color.White;
                trDay183.Visible = false;
                trNC.Visible = false;
                trRC.Visible = false;
            }
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            isNew = true;
            dataClear();
            btn_showPopup_ModalPopupExtender.Show();
        }

        protected void dataClear()
        {
            HiddenField_IDCARD.Value = string.Empty;
            strIDCARD = string.Empty;
            strNAME = string.Empty;
            strADDRESS1 = string.Empty;
            strADDRESS2 = string.Empty;
            strRENTNUM = string.Empty;
            strCELLPHONE = string.Empty;
            strTEL = string.Empty;
            strEXT = string.Empty;
            strBANK_CODE = "001";
            strBANK_ID = string.Empty;
            strNOTE = string.Empty;
            TextBox_MZPOLNO.Text = string.Empty;
            RadioButtonList_Day183.SelectedValue = "Y";
            DropDownList_IDTYPE.SelectedValue = "0";
            //Label_LASTDA.Text = string.Empty;
        }

        #region 查詢

        protected void Button_SearchDone_Click(object sender, EventArgs e)
        {
            searchResult = getSearchResult();

            if (searchResult.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無資料')", true);
                return;
            }

            GridView_MANUFACTURER_BASE.DataSource = searchResult;
            GridView_MANUFACTURER_BASE.DataBind();
        }

        protected void GridView_MANUFACTURER_BASE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_MANUFACTURER_BASE.PageIndex = e.NewPageIndex;
            GridView_MANUFACTURER_BASE.DataSource = searchResult;
            GridView_MANUFACTURER_BASE.DataBind();
        }

        DataTable getSearchResult()
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listParameter = new List<SqlParameter>();

            strSQL = "SELECT MB_SNID, MZ_POLNO, IDCARD, NAME, BANK_CODE, BANK_ID, LASTDA FROM B_MANUFACTURER_BASE WHERE 1=1 AND PAY_AD=@PAY_AD";
            listParameter.Add(new SqlParameter("PAY_AD", SalaryPublic.strLoginEXAD));

            if (TextBox_SearchID.Text.Length != 0)
            {
                strSQL += " AND IDCARD LIKE @IDCARD";
                listParameter.Add(new SqlParameter("IDCARD", "%" + TextBox_SearchID.Text + "%"));
            }
            if (TextBox_SearchPOLNO.Text.Length != 0)
            {
                strSQL += " AND MZ_POLNO LIKE @MZ_POLNO";
                listParameter.Add(new SqlParameter("MZ_POLNO", "%" + TextBox_SearchPOLNO.Text + "%"));
            }
            if (TextBox_SearchName.Text.Length != 0)
            {
                strSQL += " AND NAME LIKE @NAME";
                listParameter.Add(new SqlParameter("NAME", "%" + TextBox_SearchName.Text + "%"));
            }

            strSQL += " ORDER BY MZ_POLNO ASC";
           
            //20141001
            //return SalaryPublic.DataSelect(strSQL, listParameter);
            
            return o_DBFactory.ABC_toTest.GetDataTable(strSQL, listParameter);
        }

        #endregion

        #region 選取資料列

        protected void GridView_MANUFACTURER_BASE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                isNew = false;

                key = e.CommandArgument.ToString();
                dataShow(key);

                btn_showPopup_ModalPopupExtender.Show();
            }
            else if (e.CommandName == "del")
            {
                key = e.CommandArgument.ToString();
                deleteData(key);
                searchResult = getSearchResult();
                GridView_MANUFACTURER_BASE.DataSource = searchResult;
                GridView_MANUFACTURER_BASE.DataBind();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功')", true);
            }
        }

        //填入選取或查詢到的該筆資料至控制項
        protected void dataShow(string key)
        {
            strSQL = string.Format("SELECT IDCARD,NAME,ADDRESS1,ADDRESS2,RENTNUM,CELLPHONE,TEL,EXT,BANK_CODE,BANK_ID,NOTE,LASTDA,IDTYPE,NATIONCODE,DAY183,RENTCODE, MZ_POLNO FROM B_MANUFACTURER_BASE WHERE mb_snid='{0}'", key); ;
            temp = new DataTable();
            temp.Load(OracleHelper.ExecuteReader(OracleHelper.connStr, CommandType.Text, strSQL));

            HiddenField_IDCARD.Value = key;
            strIDCARD = temp.Rows[0]["IDCARD"].ToString();
            TextBox_MZPOLNO.Text = temp.Rows[0]["MZ_POLNO"].ToString();
            strNAME = temp.Rows[0]["NAME"].ToString();
            strADDRESS1 = temp.Rows[0]["ADDRESS1"].ToString();
            strADDRESS2 = temp.Rows[0]["ADDRESS2"].ToString();
            strRENTNUM = temp.Rows[0]["RENTNUM"].ToString();
            strCELLPHONE = temp.Rows[0]["CELLPHONE"].ToString();
            strTEL = temp.Rows[0]["TEL"].ToString();
            strEXT = temp.Rows[0]["EXT"].ToString();
            strBANK_CODE = temp.Rows[0]["BANK_CODE"].ToString();
            strBANK_ID = temp.Rows[0]["BANK_ID"].ToString();
            strNOTE = temp.Rows[0]["NOTE"].ToString();
            //Label_LASTDA.Text = temp.Rows[0]["LASTDA"].ToString();
            DropDownList_IDTYPE.Items.FindByValue(DropDownList_IDTYPE.SelectedValue).Selected = false;
            if (!temp.Rows[0]["IDTYPE"].ToString().Equals(""))
            {
                DropDownList_IDTYPE.Items.FindByValue(temp.Rows[0]["IDTYPE"].ToString()).Selected = true;
            }
            TextBox_NationCode.Text = "";
            if (!temp.Rows[0]["NATIONCODE"].ToString().Equals(""))
            {
                TextBox_NationCode.Text = temp.Rows[0]["NATIONCODE"].ToString();
            }
            RadioButtonList_Day183.Items[1].Selected = false;
            if (!temp.Rows[0]["DAY183"].ToString().Equals(""))
            {
                RadioButtonList_Day183.Items.FindByValue(temp.Rows[0]["DAY183"].ToString()).Selected = true;
            }
            TextBox_RentCode.Text = "";
            if (!temp.Rows[0]["NATIONCODE"].ToString().Equals(""))
            {
                TextBox_RentCode.Text = temp.Rows[0]["NATIONCODE"].ToString();
            }

            string strIC = DropDownList_IDTYPE.SelectedValue;
            switch (strIC)
            {
                case "0":
                case "1":
                case "3":
                    trDay183.Visible = false;
                    trNC.Visible = false;
                    trRC.Visible = false;
                    break;
                default:
                    trDay183.Visible = true;
                    trNC.Visible = true;
                    trRC.Visible = true;
                    break;
            }
        }

        void deleteData(string key)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "DELETE B_MANUFACTURER_BASE WHERE MB_SNID=@MB_SNID";

            ops.Add(new SqlParameter("MB_SNID", key));

            o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
        }

        #endregion

        #region popup視窗

        protected void btn_popupSave_Click(object sender, EventArgs e)
        {
            if (strIDCARD.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('身份證字號/統一編號欄位不可空白！')", true);
                TextBox_IDCARD.BackColor = Color.Orange;
                return;
            }
            if (cb_check.Checked)
            {
                //2012/12/26 小隊長要求拿掉判別
                //if (!Police.isValidID(strIDCARD))
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('身份證字號格式不正確！')", true);
                //    TextBox_IDCARD.BackColor = Color.Orange;
                //    return;
                //}
            }

            if (isNew)
            {
                if (boolMANUFACTURER_BASE_Create())
                {
                    GridView_MANUFACTURER_BASE.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增完成')", true);

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增失敗')", true);
                }
            }
            else
            {
                if (boolMANUFACTURER_BASE_Update())
                {
                    GridView_MANUFACTURER_BASE.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改完成')", true);

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改失敗')", true);
                }
            }

            searchResult = getSearchResult();
            GridView_MANUFACTURER_BASE.DataSource = searchResult;
            GridView_MANUFACTURER_BASE.DataBind();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            switch (hasSameID(strIDCARD))
            {
                case 0:
                    LabelCheck.Text = "無相同資料";
                    LabelCheck.ForeColor = Color.Blue;
                    break;

                case 1:
                    LabelCheck.Text = "其他基本資料中已有此身份證號";
                    LabelCheck.ForeColor = Color.Red;
                    break;

                case 2:
                    LabelCheck.Text = "人事資料中已有此身份證號";
                    LabelCheck.ForeColor = Color.Red;
                    break;
            }

            LabelCheck.Visible = true;
            btn_showPopup_ModalPopupExtender.Show();
        }

        protected void Button_SAME_Click(object sender, EventArgs e)
        {
            strADDRESS2 = strADDRESS1;
            btn_showPopup_ModalPopupExtender.Show();
        }

        protected void DropDownList_IDTYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strIC = DropDownList_IDTYPE.SelectedValue;
            switch (strIC)
            {
                case "0":
                case "1":
                case "3":
                    trDay183.Visible = false;
                    trNC.Visible = false;
                    trRC.Visible = false;
                    break;
                case "6":
                case "8":
                    trDay183.Visible = true;
                    //證號別6,8都預設為滿183天
                    RadioButtonList_Day183.SelectedValue = "Y";
                    trNC.Visible = true;
                    trRC.Visible = true;
                    break;
                default:
                    trDay183.Visible = true;
                    //證號別5,7,9都預設為未滿183天
                    RadioButtonList_Day183.SelectedValue = "N";
                    trNC.Visible = true;
                    trRC.Visible = true;
                    break;
            }

            btn_showPopup_ModalPopupExtender.Show();
        }

        private bool boolMANUFACTURER_BASE_Create()
        {
            try
            {
                string strIC = DropDownList_IDTYPE.SelectedValue;
                string strNC = "";
                string strDay183 = "";
                string strRC = "";
                switch (strIC)
                {
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        strNC = TextBox_NationCode.Text.ToUpper().Trim();
                        strDay183 = RadioButtonList_Day183.SelectedValue;
                        strRC = TextBox_RentCode.Text.ToUpper().Trim();
                        break;
                    default:
                        break;
                }

                //strSQL = "select  NEXT VALUE FOR dbo.B_MANUFACTURER_BASE_SN ";
                //int sn = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(strSQL).ToString());
                strSQL = "INSERT INTO B_MANUFACTURER_BASE (MB_SNID,IDCARD,NAME,ADDRESS1,ADDRESS2,RENTNUM,CELLPHONE,TEL,EXT,BANK_CODE,BANK_ID,NOTE,LASTDA,IDTYPE,NATIONCODE,DAY183,RENTCODE, MZ_POLNO, PAY_AD) VALUES ( NEXT VALUE FOR dbo.B_MANUFACTURER_BASE_SN,@IDCARD,@NAME,@ADDRESS1,@ADDRESS2,@RENTNUM,@CELLPHONE,@TEL,@EXT,@BANK_CODE,@BANK_ID,@NOTE,@LASTDA,@IDTYPE,@NATIONCODE,@DAY183,@RENTCODE, @MZ_POLNO, @PAY_AD) ";
                SqlParameter[] parameterList = { 
                    //new SqlParameter("MB_SNID",SqlDbType.Float){Value = sn},
                    new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = SalaryPublic.strLoginEXAD},
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = strIDCARD},
                    new SqlParameter("NAME",SqlDbType.VarChar){Value = strNAME},
                    new SqlParameter("ADDRESS1",SqlDbType.VarChar){Value = strADDRESS1},
                    new SqlParameter("ADDRESS2",SqlDbType.VarChar){Value = strADDRESS2},
                    new SqlParameter("RENTNUM",SqlDbType.VarChar){Value=strRENTNUM},
                    new SqlParameter("CELLPHONE",SqlDbType.VarChar){Value = strCELLPHONE},
                    new SqlParameter("TEL",SqlDbType.VarChar){Value = strTEL},
                    new SqlParameter("EXT",SqlDbType.VarChar){Value = strEXT},
                    new SqlParameter("BANK_CODE",SqlDbType.VarChar){Value = strBANK_CODE},
                    new SqlParameter("BANK_ID",SqlDbType.VarChar){Value = strBANK_ID},
                    new SqlParameter("NOTE",SqlDbType.VarChar){Value = strNOTE},
                    new SqlParameter("LASTDA",SqlDbType.DateTime){Value = DateTime.Now},
                    new SqlParameter("IDTYPE",SqlDbType.VarChar){Value = DropDownList_IDTYPE.SelectedValue},
                    new SqlParameter("NATIONCODE",SqlDbType.VarChar){Value = strNC},
                    new SqlParameter("DAY183",SqlDbType.VarChar){Value = strDay183},
                    new SqlParameter("RENTCODE",SqlDbType.VarChar){Value = strRC},
                    new SqlParameter("MZ_POLNO", SqlDbType.VarChar){ Value = TextBox_MZPOLNO.Text == "" ? (object)DBNull.Value : TextBox_MZPOLNO.Text }// 2011/03/10 增加「自訂編號」，可作為單一發放查詢條件
                    };
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool boolMANUFACTURER_BASE_Update()
        {
            if (key == null || key.ToString() == "")
                return false;

            try
            {
                string strIC = DropDownList_IDTYPE.SelectedValue;
                string strNC = "";
                string strDay183 = "";
                string strRC = "";
                switch (strIC)
                {
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        strNC = TextBox_NationCode.Text.ToUpper().Trim();
                        strDay183 = RadioButtonList_Day183.SelectedValue;
                        strRC = TextBox_RentCode.Text.ToUpper().Trim();
                        break;
                    default:
                        break;
                }

                strSQL = "UPDATE B_MANUFACTURER_BASE SET IDCARD = @IDCARD,NAME = @NAME,ADDRESS1 = @ADDRESS1,ADDRESS2 = @ADDRESS2, RENTNUM = @RENTNUM,CELLPHONE = @CELLPHONE,TEL = @TEL,EXT = @EXT,BANK_CODE = @BANK_CODE,BANK_ID = @BANK_ID,NOTE = @NOTE,LASTDA = @LASTDA,IDTYPE = @IDTYPE, NATIONCODE = @NATIONCODE, DAY183 = @DAY183, RENTCODE = @RENTCODE, MZ_POLNO = @MZ_POLNO WHERE MB_SNID = @MB_SNID";
                //strSQL = "UPDATE B_MANUFACTURER_BASE SET IDCARD = @IDCARD,NAME = @NAME,ADDRESS1 = @ADDRESS1,ADDRESS2 = @ADDRESS2, RENTNUM = @RENTNUM,CELLPHONE = @CELLPHONE,TEL = @TEL,EXT = @EXT,BANK_CODE = @BANK_CODE,BANK_ID = @BANK_ID,NOTE = @NOTE,LASTDA = @LASTDA,IDTYPE = @IDTYPE, NATIONCODE = @NATIONCODE, DAY183 = @DAY183, RENTCODE = @RENTCODE WHERE IDCARD = @IDCARD";
                SqlParameter[] parameterList = { 
                    new SqlParameter("MB_SNID",SqlDbType.Float){Value = int.Parse(key.ToString())},
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = strIDCARD},
                    new SqlParameter("NAME",SqlDbType.VarChar){Value = strNAME},
                    new SqlParameter("ADDRESS1",SqlDbType.VarChar){Value = strADDRESS1},
                    new SqlParameter("ADDRESS2",SqlDbType.VarChar){Value = strADDRESS2},
                    new SqlParameter("RENTNUM",SqlDbType.VarChar){Value=strRENTNUM},
                    new SqlParameter("CELLPHONE",SqlDbType.VarChar){Value = strCELLPHONE},
                    new SqlParameter("TEL",SqlDbType.VarChar){Value = strTEL},
                    new SqlParameter("EXT",SqlDbType.VarChar){Value = strEXT},
                    new SqlParameter("BANK_CODE",SqlDbType.VarChar){Value = strBANK_CODE},
                    new SqlParameter("BANK_ID",SqlDbType.VarChar){Value = strBANK_ID},
                    new SqlParameter("NOTE",SqlDbType.VarChar){Value = strNOTE},
                    new SqlParameter("LASTDA",SqlDbType.DateTime){Value = DateTime.Now},
                    new SqlParameter("IDTYPE",SqlDbType.VarChar){Value = DropDownList_IDTYPE.SelectedValue},
                    new SqlParameter("NATIONCODE",SqlDbType.VarChar){Value = strNC},
                    new SqlParameter("DAY183",SqlDbType.VarChar){Value = strDay183},
                    new SqlParameter("RENTCODE",SqlDbType.VarChar){Value = strRC},
                    new SqlParameter("MZ_POLNO", SqlDbType.VarChar){ Value = TextBox_MZPOLNO.Text == "" ? (object)DBNull.Value : TextBox_MZPOLNO.Text}// 2011/03/10 增加「自訂編號」，可作為單一發放查詢條件
                    };
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //檢查身分證號/統一編號是否重複
        private int hasSameID(string strIDCARD)
        {
            string strSQL;
            DataTable dt = new DataTable();

            //找其他基本資料(B_MANUFACTURER_BASE)
            strSQL = string.Format("SELECT mb_snid FROM B_MANUFACTURER_BASE WHERE IDCARD='{0}'", this.strIDCARD);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            if (dt.Rows.Count > 0)
            {
                dataShow(dt.Rows[0]["mb_snid"].ToString());
                TextBox_IDCARD.BackColor = Color.White;
                return 1;
            }

            //找人事資料(A_DLBASE)
            strSQL = string.Format("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='{0}'", this.strIDCARD);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            if (dt.Rows.Count > 0)
            {
                strNAME = string.Empty;
                strADDRESS1 = string.Empty;
                strADDRESS2 = string.Empty;
                strRENTNUM = string.Empty;
                strCELLPHONE = string.Empty;
                strTEL = string.Empty;
                strEXT = string.Empty;
                strBANK_CODE = "001";
                strBANK_ID = string.Empty;
                strNOTE = string.Empty;
                //Label_LASTDA.Text = string.Empty;

                return 2;
            }

            return 0;
        }

        #endregion

        protected void btn_export_Click(object sender, EventArgs e)
        {
            DataTable dt = getSearchResult();

            dt.Columns.Remove(dt.Columns["MB_SNID"]);
            dt.Columns.Remove(dt.Columns["LASTDA"]);

            dt.Columns["MZ_POLNO"].ColumnName = "自訂編號";
            dt.Columns["IDCARD"].ColumnName = "身分證號碼／統一編號";
            dt.Columns["NAME"].ColumnName = "姓名/抬頭";
            dt.Columns["BANK_CODE"].ColumnName = "銀行代碼";
            dt.Columns["BANK_ID"].ColumnName = "銀行帳號";

            NPOIS.Dt2Excel(dt, "otherBase");
        }
    }
}
