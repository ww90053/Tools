using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Service;
using TPPDDB.Model;
using System.Data.SqlClient;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_BatchModify : System.Web.UI.Page
    {
        /// <summary>使用者模型</summary>
        public AccountModel UserModel
        {
            set
            {
                
            }
            get
            {
                String ID = Request.QueryString["id"];
                return AccountService.lookupAccount(ID);
            }
        }

        /// <summary>表單 : 載入</summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String ID = Request.QueryString["id"];

                //取得使用者傳入之使用者的基本資料
                this.UserModel = AccountService.lookupAccount(ID);

                //檢查此身分證字號是否能正常取得使用者資料
                if (String.IsNullOrEmpty(UserModel.Name))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此身份證字號「" + ID + "」找不到資料，請重新確認');window.close();", true);
                    return;
                }

                //載入使用者姓名及身分證字號
                TextBox_MZ_ID.Text = this.UserModel.ID;
                TextBox_MZ_NAME.Text = this.UserModel.Name;

                //載入編制機關
                DropDownList_MZ_AD.DataBind();
                DropDownList_MZ_AD.SelectedValue = this.UserModel.PayAD;

                //載入現服機關
                DropDownList_MZ_EXAD.DataBind();
                DropDownList_MZ_EXAD.SelectedValue = this.UserModel.ExAD;

                //依照使用者目前的編制機關及現服機關，來載入下一層單位之下拉選單
                BindingUnit(DropDownList_MZ_UNIT, this.UserModel.PayAD);
                BindingUnit(DropDownList_MZ_EXUNIT, this.UserModel.ExAD);
                BindingUnit(DropDownList_MZ_UNIT_Update, this.UserModel.PayAD);
                BindingUnit(DropDownList_MZ_EXUNIT_Update, this.UserModel.ExAD);

                try
                {
                    //依照使用者目前的編制機關及現服機關，預設選擇其單位
                    DropDownList_MZ_UNIT.SelectedValue = this.UserModel.Unit;
                    DropDownList_MZ_UNIT_Update.SelectedValue = this.UserModel.Unit;

                    DropDownList_MZ_EXUNIT.SelectedValue = this.UserModel.ExUnit;
                    DropDownList_MZ_EXUNIT_Update.SelectedValue = this.UserModel.ExUnit;
                }
                catch (Exception ex)
                {
                    String ErrMsg = "載入現服單位、編制單位時載入錯誤，有可能是該機關底下已無此單位存在；或為其他問題。參考錯誤訊息 : " + ex.Message.Replace("\r\n", " ");
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrMsg + "'); window.close();", true);
                    return;
                }

            }

            C.set_Panel_EnterToTAB(ref this.Panel1);
        }

        /// <summary>按鈕 : 確認送出</summary>
        protected void btOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_DUTYDATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請輸入欲修改之年度月份')", true);
                return;
            }

            //儲存新的單位
            String New_Unit = DropDownList_MZ_UNIT_Update.SelectedValue;
            String New_ExUnit = DropDownList_MZ_EXUNIT_Update.SelectedValue;
           
            List<SqlParameter> lstParameter = new List<SqlParameter>();
                                  lstParameter.Add(new SqlParameter("MZ_UNIT", SqlDbType.NVarChar)   { Value = New_Unit });
                                  lstParameter.Add(new SqlParameter("MZ_EXUINT", SqlDbType.NVarChar) { Value = New_ExUnit });
                                  lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar)     { Value = this.UserModel.ID });
                                  lstParameter.Add(new SqlParameter("MZ_AD", SqlDbType.NVarChar) { Value = this.UserModel.PayAD });

            String SQL = @"UPDATE C_DUTYTABLE_PERSONAL
                           SET 
                              MZ_UNIT = @MZ_UNIT,
                              MZ_EXUNIT = @MZ_EXUINT 
                           WHERE
                              MZ_ID = @MZ_ID AND DUTYDATE LIKE '{0}%' AND MZ_AD = @MZ_AD";
                   SQL = String.Format(SQL, TextBox_MZ_DUTYDATE.Text);

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParameter.ToArray());
            } 
            catch(Exception ex)
            {
                String ErrMsg = "更新資料庫時發生錯誤，請參考錯誤訊息 : " + ex.Message.Replace("\r\n", " ");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrMsg + "'); window.close();", true);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                "alert('修改完成!'); window.opener.location.href='C_ForLeaveOvertime_OVERTIME_FREE_KEYIN.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);

        }

        /// <summary>變更單位</summary>
        private void BindingUnit(DropDownList target, String MZ_AD)
        {
            String strSQL = @"SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE)
                                FROM A_KTYPE
                               WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')";
                    strSQL = String.Format(strSQL, MZ_AD);

            DataTable temp = new DataTable();
                      temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            target.DataSource = temp;
            target.DataTextField = "RTRIM(MZ_KCHI)";
            target.DataValueField = "RTRIM(MZ_KCODE)";
            target.DataBind();
            target.Items.Insert(0, "");
        }

    }
}
