using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_LVHistoryM : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp;

        #region 查詢條件
        public string sAd { get { return ddlSAd.SelectedValue; } set { ddlSAd.SelectedValue = value; } }
        public string sUnit { get { return ddlSUnit.SelectedValue; } set { ddlSUnit.SelectedValue = value; } }
        public string sPeople { get { return ddlSPeople.SelectedValue; } set { ddlSPeople.SelectedValue = value; } }
        public string sId { get { return tbSId.Text; } set { tbSId.Text = value; } }
        public string sSDate { get { return tbStart.Text; } set { tbStart.Text = value; } }
        public string sEDate { get { return tbEnd.Text; } set { tbEnd.Text = value; } }
        public string sMemo { get { return tbSMemo.Text; } set { tbSMemo.Text = value; } }
        public string sBack { get { return rblBack.SelectedValue; } }


        //public string iAd { get { return ddlIAd.SelectedValue; } set { ddlIAd.SelectedValue = value; } }
        //public string iUnit { get { return ddlIUnit.SelectedValue; } set { ddlIUnit.SelectedValue = value; } }
        
        //public string iPeople { get { return ddlIPeople.SelectedValue; } set { ddlIPeople.SelectedValue = value; } }
        public string iPeople { get { return txt_IDCard_Insert.Text; } set { txt_IDCard_Insert.Text = value; } }
        public string AD { get { return lb_Ad_Insert.Text; } set { lb_Ad_Insert.Text = value; } }
        public string Unit { get { return lb_Unit_Insert.Text; } set { lb_Unit_Insert.Text = value; } }
        public string Name { get { return lb_Name_Insert.Text; } set { lb_Name_Insert.Text = value; } }
        
        


        //public string iId { get { return tbIId.Text; } set { tbIId.Text = value; } }
        //public string iId { get { return txt_IDCard_Insert.Text; } set { txt_IDCard_Insert.Text = value; } }
       
        
        public string iSDate { get { return tbIStart.Text; } set { tbIStart.Text = value; } }
        public string iEDate { get { return tbIEnd.Text; } set { tbIEnd.Text = value; } }
        public string iMemo { get { return tbIMemo.Text; } set { tbIMemo.Text = value; } }
        #endregion

       

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["ADPMZ_ID"] = "A220048572";
            

            if (!Page.IsPostBack)
            {
                 
                pageInit(ddlSAd);
               
            }
            group_control(o_a_Function.strGID(Session["ADPMZ_ID"].ToString()));
        }

        // 依照ABCD權限、做功能隱藏的動作
        public void group_control(string strGID)
        {
            switch (strGID)
            {
                case "A":
                case "B":
                    break;
                case "C":
                    ddlSAd.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    
                    unitInit(ddlSAd,ddlSUnit);
                   
                    ddlSUnit.Items.Insert(0, "全部");


                    unitIInit();
                    break;
                case "D":
                case "E":
                default:
                    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                    break;
            }
        }

        private void dataRead()
        {
            strSQL = @"SELECT SN,CL.MZ_ID,AD1.MZ_NAME MZ_NAME,SDATE,EDATE,CASE WHEN BACK=1 THEN '是' ELSE '否' END BACK,MEMO,UPDDATE,AD2.MZ_NAME UPDUSER
                       FROM C_LVHISTORY CL
                       LEFT JOIN A_DLBASE AD1 ON AD1.MZ_ID=CL.MZ_ID 
                       LEFT JOIN A_DLBASE AD2 ON AD2.MZ_ID =CL.UPDUSER
                       WHERE ROWNUM<=10
                       ORDER BY SN DESC";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            gvData.DataSource = temp;
            gvData.DataBind();
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                iPeople = gvData.DataKeys[index]["MZ_ID"].ToString();
                iSDate = CommonCS.ConvertDate7(gvData.DataKeys[index]["SDATE"].ToString());
                iEDate = CommonCS.ConvertDate7(gvData.DataKeys[index]["EDATE"].ToString());
                iMemo = gvData.DataKeys[index]["MEMO"].ToString();
                btnInsert.CommandArgument = gvData.DataKeys[index]["SN"].ToString();
                Label1.Text = "修改";
                btnInsert.Text = "儲存";


                strSQL = "SELECT MZ_NAME,MZ_AD_NAME , MZ_UNIT_NAME FROM VW_DLBASE_BASIC WHERE MZ_ID='" + iPeople + "'";

                DataTable SelectPerson = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                if (SelectPerson.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('找不到人員')", true);
                    btnI_ModalPopupExtender.Show();
                    return;

                }

                AD = SelectPerson.Rows[0]["MZ_AD_NAME"].ToString();
                Unit = SelectPerson.Rows[0]["MZ_UNIT_NAME"].ToString();
                Name = SelectPerson.Rows[0]["MZ_NAME"].ToString();

               // tr1.Visible = false;
               // tr2.Visible = false;
               // ddlIPeople.Visible = false;
               // tbIId.Visible = true;
                btnI_ModalPopupExtender.Show();
            }
        }

        #region 查詢機關單位下拉
        private void pageInit(DropDownList ddl)
        {
            
            ddl.Items.Clear();
            //機關
            strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%'";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                ddl.DataSource = temp;
                ddl.DataTextField = "MZ_KCHI";
                ddl.DataValueField = "MZ_KCODE";
                ddl.DataBind();


                foreach (ListItem li in ddl.Items)
                {
                    if (li.Text != "新北市政府警察局")
                        li.Text = li.Text.Replace("新北市政府警察局", string.Empty);
                }


                if (ddl.ClientID.Substring(ddl.ClientID.Length-6,6) == "ddlSAd")
                {
                    ddl.Items.Insert(0, new ListItem("全部", "全部"));
                }

                if (ddl.ClientID.Substring(ddl.ClientID.Length - 6, 6) == "ddlIAd")
                {
                    ddl.Items.Insert(0, new ListItem("請選擇", "請選擇"));
                }

                if (ddl.ClientID.Substring(ddl.ClientID.Length - 13, 13) == "ddl_Select_AD")
                {
                    ddl.Items.Insert(0, "請選擇");
                    ddl_Select_Unit.Items.Clear();
                    ddl_Select_Unit.Items.Insert(0,"請先選擇機關");
                    ddlSPeople.Items.Clear();
                    ddlSPeople.Items.Insert(0, "請先選擇機關單位");
                }
                //ddlSAd.Items.Insert(0, new ListItem("全部", "全部"));
                //ddlIAd.Items.Insert(0, new ListItem("請選擇", "請選擇"));
                //ddl_Select_AD.Items.Insert(0, new ListItem("請選擇", "請選擇"));
            }


        }

        protected void ddlSAd_SelectedIndexChanged(object sender, EventArgs e)
        {
            unitInit(ddlSAd,ddlSUnit);
            ddlSUnit.Items.Insert(0, "全部");

            //btnS_ModalPopupExtender.Show();
            Where_Condition();
        }

        protected void unitInit(DropDownList ddl_self, DropDownList ddl_other )
        {
           

           
            if (ddl_self.SelectedValue != "0")
            {
                strSQL = "SELECT AAA.* FROM (SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM VW_A_UNIT_AD WHERE MZ_AD=@MZ_AD)) AAA ";
                SqlParameter[] parameterList ={
                              new SqlParameter("MZ_AD",SqlDbType.VarChar ){Value= ddl_self.SelectedValue}
                              };
                temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList).Tables[0];
                ddl_other.DataSource = temp;
                ddl_other.DataTextField = "MZ_KCHI";
                ddl_other.DataValueField = "MZ_KCODE";
                ddl_other.DataBind();
                //ddl.Items.Insert(0, "全部");
            }
            else
            {
                ddl_self.Items.Clear();
                ddl_self.Items.Insert(0, "請先選擇機關");

                if (ddl_other.ClientID.Substring(ddl_other.ClientID.Length - 10, 10) == "ddlSPeople")
                {

                    ddlSPeople.Items.Clear();
                    ddlSPeople.Items.Insert(0, "請先選擇機關單位");
                }
                sId = string.Empty;
            }
        }

        protected void ddlSUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //people();
            //btnS_ModalPopupExtender.Show();

            Where_Condition();
        }

        protected void people()
        {
            if (ddl_Select_Unit.SelectedValue != "0")
            {
                ddlSPeople.Items.Clear();
                strSQL = "SELECT MZ_ID,MZ_NAME FROM VW_DLBASE_BASIC WHERE MZ_EXAD=@AD AND MZ_EXUNIT=@UNIT";
                SqlParameter[] parameterList ={
                              new SqlParameter("AD",SqlDbType.VarChar ){Value= ddl_Select_AD.SelectedValue},
                              new SqlParameter("UNIT",SqlDbType.VarChar ){Value= ddl_Select_Unit.SelectedValue}
                              };
                temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList).Tables[0];
                if (temp.Rows.Count > 0)
                {
                    ddlSPeople.DataSource = temp;
                    ddlSPeople.DataTextField = "MZ_NAME";
                    ddlSPeople.DataValueField = "MZ_ID";
                    ddlSPeople.DataBind();
                }
                ddlSPeople.Items.Insert(0, "請選擇");

            }
            else
            {
                ddlSPeople.Items.Clear();
                ddlSPeople.Items.Insert(0, "請先選擇機關單位");
                sId = string.Empty;
            }
        }

        protected void ddlIAd_SelectedIndexChanged(object sender, EventArgs e)
        {
            unitIInit();
            btnI_ModalPopupExtender.Show();
            
        }

        protected void unitIInit()
        {
            if (sAd != "請選擇")
            {
                //strSQL = "SELECT AAA.* FROM (SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM VW_A_UNIT_AD WHERE MZ_AD=@MZ_AD)) AAA ";
                //SqlParameter[] parameterList ={
                //              new SqlParameter("MZ_AD",SqlDbType.VarChar ){Value= iAd}
                //              };
                //temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList).Tables[0];
                ////ddlIUnit.DataSource = temp;
                //ddlIUnit.DataTextField = "MZ_KCHI";
                //ddlIUnit.DataValueField = "MZ_KCODE";
                //ddlIUnit.DataBind();

                //ddlIUnit.Items.Insert(0, new ListItem("請選擇", "請選擇"));
            }
            else
            {
                //ddlIUnit.Items.Clear();
                //ddlIUnit.Items.Insert(0, "請先選擇機關");
            }
        }

        protected void ddlIUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ipeople();
            btnI_ModalPopupExtender.Show();

            
        }

        protected void Ipeople()
        {
            if (sUnit != "請選擇")
            {
                //ddlIPeople.Items.Clear();
                //strSQL = "SELECT MZ_ID,MZ_NAME FROM VW_DLBASE_BASIC WHERE MZ_EXAD=@AD AND MZ_EXUNIT=@UNIT";
                //SqlParameter[] parameterList ={
                //              new SqlParameter("AD",SqlDbType.VarChar ){Value= iAd},
                //              new SqlParameter("UNIT",SqlDbType.VarChar ){Value= iUnit}
                //              };
                //temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList).Tables[0];
                //if (temp.Rows.Count > 0)
                {
                    //ddlIPeople.DataSource = temp;
                    //ddlIPeople.DataTextField = "MZ_NAME";
                    //ddlIPeople.DataValueField = "MZ_ID";
                    //ddlIPeople.DataBind();

                    //ddlIPeople.Items.Insert(0, new ListItem("請選擇", "請選擇"));

                }
            }
            else
            {
                //ddlIPeople.Items.Clear();
                //ddlIPeople.Items.Insert(0, "請先選擇機關單位");
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            doSearch();
           
        }

        private void doSearch()
        {
                
            strSQL = @"SELECT BB.* FROM (SELECT SN,CL.MZ_ID,AD1.MZ_NAME MZ_NAME,SDATE,EDATE,CASE WHEN BACK=1 THEN '是' ELSE '否' END BACK,BACK BACKS,MEMO,UPDDATE,AD2.MZ_NAME UPDUSER,AD1.MZ_EXAD,AD1.MZ_EXUNIT
                       FROM C_LVHISTORY CL
                       LEFT JOIN A_DLBASE AD1 ON AD1.MZ_ID=CL.MZ_ID 
                       LEFT JOIN A_DLBASE AD2 ON AD2.MZ_ID =CL.UPDUSER) BB
                       WHERE BB.MZ_ID='" + tbSId.Text + "'";
         
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL,"get");

            gvData.DataSource = temp;
            gvData.DataBind();


            tbSId.Text = "";
            

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            string error = columnCheck();
            if (string.IsNullOrEmpty(error))
            {
                if (btnInsert.Text == "新增")
                {
                    strSQL = "SELECT  NEXT VALUE FOR dbo.C_LVHISTORY_SN ";
                    int no = int.Parse(o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET").Rows[0][0].ToString());
                    strSQL = @"
                       INSERT INTO C_LVHISTORY(SN,MZ_ID,SDATE,EDATE,MEMO,BACK,UPDDATE,UPDUSER) 
                       VALUES(@SN,@MZ_ID,@SDATE,@EDATE,@MEMO,@BACK,@UPDDATE,@UPDUSER)
                      ";
                    SqlParameter[] parameterList ={
                              new SqlParameter("SN",SqlDbType.VarChar ){Value= no},
                              new SqlParameter("MZ_ID",SqlDbType.VarChar ){Value= iPeople},
                              new SqlParameter("SDATE",SqlDbType.DateTime ){Value= CommonCS.DateConvert(iSDate)},
                              new SqlParameter("EDATE",SqlDbType.DateTime ){Value= CommonCS.DateConvert(iEDate)},
                              new SqlParameter("MEMO",SqlDbType.NVarChar ){Value= iMemo},
                              new SqlParameter("BACK",SqlDbType.Char ){Value= 0}, //1為確認
                              new SqlParameter("UPDUSER",SqlDbType.VarChar ){Value= Session["ADPMZ_ID"].ToString()},
                              new SqlParameter("UPDDATE",SqlDbType.DateTime ){Value= DateTime.Now.ToString()}
                              };
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    //CommonCS.ShowMessage(tbIId, "新增成功");
                    CommonCS.ShowMessage(txt_IDCard_Insert, "新增成功");
                   
                    dataRead();
                }
                else
                {                    
                    strSQL = @"UPDATE C_LVHISTORY SET SDATE=@SDATE,EDATE=@EDATE,MEMO=@MEMO,UPDDATE=@UPDDATE,UPDUSER=@UPDUSER ,MZ_ID=@MZ_ID WHERE SN=@SN";

                    SqlParameter[] parameterList ={
                              new SqlParameter("SN",SqlDbType.VarChar ){Value= btnInsert.CommandArgument.ToString()},
                              new SqlParameter("SDATE",SqlDbType.DateTime ){Value= CommonCS.DateConvert(iSDate)},
                              new SqlParameter("EDATE",SqlDbType.DateTime ){Value= CommonCS.DateConvert(iEDate)},
                              new SqlParameter("MEMO",SqlDbType.NVarChar ){Value= iMemo},
                              new SqlParameter("UPDUSER",SqlDbType.VarChar ){Value= Session["ADPMZ_ID"].ToString()},
                              new SqlParameter("UPDDATE",SqlDbType.DateTime ){Value= DateTime.Now.ToString()},
                              new SqlParameter("MZ_ID",SqlDbType.NVarChar ){Value= iPeople}
         
                              };
                    if (o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList) > 0)
                    {
                       
                        CommonCS.ShowMessage(txt_IDCard_Insert, "修改成功");
                        dataRead();
                    }
                    else
                        CommonCS.ShowMessage(txt_IDCard_Insert, "修改失敗");
                    
                }
            }
            else
            {
                CommonCS.ShowMessage(txt_IDCard_Insert, error);
                
                btnI_ModalPopupExtender.Show();
            }
        }

        private string columnCheck()
        {
            string error = string.Empty;

            if (btnInsert.Text == "新增")
            {
                if (iPeople == "請先選擇機關單位" || iPeople == "請選擇")
                    error = "請選擇欲新增人員\\r\\n";
            }

            if (iSDate.Trim() == string.Empty)
            {
                error += "請輸入開始日期\\r\\n";
            }
            else
            {
                if (!CommonCS.DateJudge(iSDate))
                {
                    error += "請輸入正確的開始日期(範例：1010315)\\r\\n";
                }
            }
            if (iEDate.Trim() == string.Empty)
            {
                error += "請輸入結束日期\\r\\n";
            }
            else
            {
                if (!CommonCS.DateJudge(iEDate))
                {
                    error += "請輸入正確的結束日期(範例：1010315)\\r\\n";
                }
            }
            if (iMemo.Trim() == string.Empty)
                error += "請輸入備註\\r\\n";

            return error;
        }

        protected void Button1_Command(object sender, CommandEventArgs e)
        {
            strSQL = string.Format("UPDATE C_LVHISTORY SET BACK=1 WHERE SN={0}", e.CommandArgument.ToString());
            o_DBFactory.ABC_toTest.Edit_Data(strSQL);
            CommonCS.ShowMessage(txt_IDCard_Insert, "確認成功");
            //CommonCS.ShowMessage(tbIId, "確認成功");
            dataRead();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Label1.Text = "新增";
            btnInsert.Text = "新增";
          
            iPeople = string.Empty;
            AD = "請先輸入身分證字號";
            Unit = "請先輸入身分證字號";
            Name = "請先輸入身分證字號";


            iSDate = string.Empty;
            iEDate = string.Empty;
            
            iMemo = string.Empty;

            btnI_ModalPopupExtender.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSPeople_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sPeople != "全部")
                sId = sPeople;
            else
                sId = string.Empty;

            tr_unit1.Visible = false;
            tr6.Visible = false;
            tr_unit2.Visible = false;

         
            btnS_ModalPopupExtender.Show();
        }

        //2013/04/11 
        protected void rblBack_SelectedIndexChanged(object sender, EventArgs e)
        {
            Where_Condition();
        }

        /// <summary>
        /// 篩選條件
        /// </summary>
        public void Where_Condition()
        {

            List<string> condition = new List<string>();
            List<SqlParameter> ParmeterList = new List<SqlParameter>();


            if (sAd != "全部")
            {
                condition.Add("MZ_EXAD=@AD");
                ParmeterList.Add(new SqlParameter("AD", SqlDbType.VarChar) { Value = sAd });
                if (sUnit != "全部")
                {
                    condition.Add("MZ_EXUNIT=@UNIT");
                    ParmeterList.Add(new SqlParameter("UNIT", SqlDbType.VarChar) { Value = sUnit });
                   
                }
            }


            if (!string.IsNullOrEmpty(sBack))
            {
                if (sBack != "2")
                {
                    condition.Add("BACKS=@BACK");
                    ParmeterList.Add(new SqlParameter("BACK", SqlDbType.VarChar) { Value = sBack });
                }
            }

            strSQL = string.Format(@"SELECT BB.* FROM (SELECT SN,CL.MZ_ID,AD1.MZ_NAME MZ_NAME,SDATE,EDATE,CASE WHEN BACK=1 THEN '是' ELSE '否' END BACK,BACK BACKS,MEMO,UPDDATE,AD2.MZ_NAME UPDUSER,AD1.MZ_EXAD,AD1.MZ_EXUNIT
                       FROM C_LVHISTORY CL
                       LEFT JOIN A_DLBASE AD1 ON AD1.MZ_ID=CL.MZ_ID 
                       LEFT JOIN A_DLBASE AD2 ON AD2.MZ_ID =CL.UPDUSER) BB
                       {0} ORDER BY SN",
                     condition.Count() > 0 ? " Where " + string.Join(" And ", condition.ToArray()) : string.Empty);
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, ParmeterList);
            gvData.DataSource = temp;
            gvData.DataBind();
        
        }

        protected void ddl_Select_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            unitInit(ddl_Select_AD,ddl_Select_Unit);
            ddl_Select_Unit.Items.Insert(0,"請選擇");
            btnS_ModalPopupExtender.Show();
        }

        protected void ddl_Select_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
           people();
         

            btnS_ModalPopupExtender.Show();

        }

        protected void btn_Search_People_Click(object sender, EventArgs e)
        {
            pageInit(ddl_Select_AD);
       

            tr_unit1.Visible = true;
            tr6.Visible = true;
            tr_unit2.Visible = true;
            btnS_ModalPopupExtender.Show();

        }

        protected void btnSCancel_Click(object sender, EventArgs e)
        {
            pageInit(ddl_Select_AD);
            //unitInit(ddl_Select_Unit, ddlSPeople);
            tbSId.Text = "";

            tr_unit1.Visible = false;
            tr6.Visible = false;
            tr_unit2.Visible = false;
           
         
        }

        protected void txt_IDCard_Insert_TextChanged(object sender, EventArgs e)
        {
            strSQL = "SELECT MZ_NAME,MZ_AD_NAME , MZ_UNIT_NAME FROM VW_DLBASE_BASIC WHERE MZ_ID='" + iPeople + "'";

            DataTable SelectPerson = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (SelectPerson.Rows.Count==0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('找不到人員')", true);
                btnI_ModalPopupExtender.Show();
                return;
            
            }

            AD = SelectPerson.Rows[0]["MZ_AD_NAME"].ToString();
            Unit = SelectPerson.Rows[0]["MZ_UNIT_NAME"].ToString();
            Name = SelectPerson.Rows[0]["MZ_NAME"].ToString();

            btnI_ModalPopupExtender.Show();

        }

       
    }
}
