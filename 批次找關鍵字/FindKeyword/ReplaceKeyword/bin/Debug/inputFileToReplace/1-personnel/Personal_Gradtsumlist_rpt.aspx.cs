using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Gradtsumlist_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        string strSQL;
        //string A_strGID = "";
        DataTable temp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
              A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel3);
             TextBox_MD.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');

                A.fill_AD_POST_BOSS(DropDownList_CHKAD,1);
                DropDownList_CHKAD.DataBind();
                DropDownList_CHKAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
            }


          
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            //matthew 如果現職單位是中和分局 中和一&中和二要一起加進來
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='382133400C' OR MZ_KCODE='382133500C' OR MZ_KCODE='382133600C')";
            }
            DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_CHKAD.DataSource = source;
            DropDownList_CHKAD.DataTextField = "MZ_KCHI";
            DropDownList_CHKAD.DataValueField = "MZ_KCODE";
            DropDownList_CHKAD.DataBind();

            DropDownList_CHKAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["A_strGID"].ToString() )
            {
                case "A":
                case "B":

                    break;
                case "C":
                    ChangeDropDownList_AD();
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            //List<String> PRRST = new List<string>();
            //PRRST.Insert(0,"");
            string sqlPart = "";
            if (TextBox_MD.Text.Trim() != "")
            {
                string beginD = TextBox_MD.Text.Trim().PadLeft(5, '0') + "01";
                string endD = TextBox_MD.Text.Trim().PadLeft(5, '0') + "30";

                sqlPart += " and MZ_DATE between '" + beginD + "' and '" + endD + "' ";
            }

            if (DropDownList_CHKAD.SelectedValue.Trim() != "")
            {
                sqlPart += "and MZ_AD='" + DropDownList_CHKAD.SelectedValue.Trim() + "' ";
            }
            //初始化
            List<string> prrst = new List<string>();
            //獎勵
            prrst.Add("7013");
            prrst.Add("4200");
            prrst.Add("4100");
            prrst.Add("4020");
            prrst.Add("4010");
            prrst.Add("4002");
            prrst.Add("4001");
            prrst.Add("獎勵小計");
            //懲處
            prrst.Add("5200");
            prrst.Add("無代號");
            prrst.Add("2503");
            prrst.Add("5100");
            prrst.Add("5020");
            prrst.Add("5010");
            prrst.Add("5002");
            prrst.Add("5001");
            prrst.Add("懲處小計");
            prrst.Add("合計");

            //職等
            //警監
            string s1 = "'G31','G32','G33','G34'";

            //警正
            //string s2 = "'G21','G22','G23','G24'";
            string s2 = "'G21','G22','G23','G24','P06','P07','P08','P09'";
            //警正
            //string s3 = "'G11','G12','G13','G14'";
            string s3 = "'G11','G12','G13','G14','P01','P02','P03','P04','P05'";

            DataTable gradeSumList = new DataTable();

            gradeSumList.Columns.Add("G31_34", typeof(int));
            gradeSumList.Columns.Add("G21_24", typeof(int));
            gradeSumList.Columns.Add("G11_14", typeof(int));

            int good_sum1 = 0;
            int good_sum2 = 0;
            int good_sum3 = 0;
            int bad_sum1 = 0;
            int bad_sum2 = 0;
            int bad_sum3 = 0;
            for (int i = 0; i < prrst.Count(); i++)
            {
                DataRow newdr;
                switch (prrst[i])
                {
                    case "獎勵小計":
                        newdr = gradeSumList.NewRow();
                        newdr["G31_34"] = good_sum1;
                        newdr["G21_24"] = good_sum2;
                        newdr["G11_14"] = good_sum3;
                        gradeSumList.Rows.Add(newdr);
                        break;
                    case "懲處小計":
                        newdr = gradeSumList.NewRow();
                        newdr["G31_34"] = bad_sum1;
                        newdr["G21_24"] = bad_sum2;
                        newdr["G11_14"] = bad_sum3;
                        gradeSumList.Rows.Add(newdr);
                        break;
                    case "合計":
                        newdr = gradeSumList.NewRow();
                        newdr["G31_34"] = bad_sum1 + good_sum1;
                        newdr["G21_24"] = bad_sum2 + good_sum2;
                        newdr["G11_14"] = bad_sum3 + good_sum3;
                        gradeSumList.Rows.Add(newdr);
                        break;
                    default:
                        if (i <= 6)
                        {
                            //獎勵
                            newdr = gradeSumList.NewRow();
                            //找出此分局下人
                            //符合警監
                            strSQL = string.Format("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='{3}' AND  MZ_CHKAD='{0}' AND  MZ_SRANK IN ({1}) AND MZ_DATE LIKE '{2}%'", DropDownList_CHKAD.SelectedValue, s1, TextBox_MD.Text.Trim().PadLeft(5, '0'), prrst[i]);
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                            newdr["G31_34"] = temp.Rows[0][0].ToString();
                            good_sum1 = good_sum1 + int.Parse(temp.Rows[0][0].ToString());
                            //找出此分局下人
                            //符合警正
                            strSQL = string.Format("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='{3}' AND  MZ_CHKAD='{0}' AND  MZ_SRANK IN ({1}) AND MZ_DATE LIKE '{2}%'", DropDownList_CHKAD.SelectedValue, s2, TextBox_MD.Text.Trim().PadLeft(5, '0'), prrst[i]);
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                            newdr["G21_24"] = temp.Rows[0][0].ToString();
                            good_sum2 = good_sum2 + int.Parse(temp.Rows[0][0].ToString());

                            //找出此分局下人
                            //符合警佐
                            strSQL = string.Format("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='{3}' AND  MZ_CHKAD='{0}' AND  MZ_SRANK IN ({1}) AND MZ_DATE LIKE '{2}%'", DropDownList_CHKAD.SelectedValue, s3, TextBox_MD.Text.Trim().PadLeft(5, '0'), prrst[i]);
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                            newdr["G11_14"] = temp.Rows[0][0].ToString();
                            gradeSumList.Rows.Add(newdr);
                            good_sum3 = good_sum3 + int.Parse(temp.Rows[0][0].ToString());
                        }
                        else
                        {
                            //懲處
                            newdr = gradeSumList.NewRow();
                            //找出此分局下人
                            //符合警監
                            strSQL = string.Format("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='{3}' AND  MZ_CHKAD='{0}' AND  MZ_SRANK IN ({1}) AND MZ_DATE LIKE '{2}%'", DropDownList_CHKAD.SelectedValue, s1, TextBox_MD.Text.Trim().PadLeft(5, '0'), prrst[i]);
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                            newdr["G31_34"] = temp.Rows[0][0].ToString();
                            bad_sum1 = bad_sum1 + int.Parse(temp.Rows[0][0].ToString());
                            //找出此分局下人
                            //符合警正
                            strSQL = string.Format("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='{3}' AND  MZ_CHKAD='{0}' AND  MZ_SRANK IN ({1}) AND MZ_DATE LIKE '{2}%'", DropDownList_CHKAD.SelectedValue, s2, TextBox_MD.Text.Trim().PadLeft(5, '0'), prrst[i]);
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                            newdr["G21_24"] = temp.Rows[0][0].ToString();
                            bad_sum2 = bad_sum2 + int.Parse(temp.Rows[0][0].ToString());

                            //找出此分局下人
                            //符合警佐
                            strSQL = string.Format("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='{3}' AND  MZ_CHKAD='{0}' AND  MZ_SRANK IN ({1}) AND MZ_DATE LIKE '{2}%'", DropDownList_CHKAD.SelectedValue, s3, TextBox_MD.Text.Trim().PadLeft(5, '0'), prrst[i]);
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                            newdr["G11_14"] = temp.Rows[0][0].ToString();
                            gradeSumList.Rows.Add(newdr);
                            bad_sum3 = bad_sum3 + int.Parse(temp.Rows[0][0].ToString());
                        }
                        break;
                }
            }

            //string strSQL = "SELECT " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='7013' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a7013, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='7013' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b7013, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='7013' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c7013, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='7013' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL7013, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4200' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a4200, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4200' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b4200, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4200' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c4200, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4200' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL4200, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4100' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a4100, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4100' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b4100, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4100' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c4100, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4100' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL4100, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4020' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a4020, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4020' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b4020, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4020' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c4020, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4020' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL4020, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4010' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a4010, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4010' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b4010, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4010' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c4010, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4010' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL4010, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4002' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a4002, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4002' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b4002, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4002' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c4002, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4002' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL4002, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4001' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a4001, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4001' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b4001, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4001' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c4001, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='4001' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL4001, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5200' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a5200, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5200' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b5200, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5200' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c5200, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5200' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL5200, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5100' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a5100, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5100' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b5100, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5100' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c5100, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5100' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL5100, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5020' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a5020, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5020' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b5020, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5020' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c5020, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5020' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL5020, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5010' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a5010, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5010' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b5010, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5010' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c5010, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5010' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL5010, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5002' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a5002, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5002' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b5002, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5002' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c5002, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5002' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL5002, " +

            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5001' and MZ_SRANK Between 'G31' and 'G34' " + sqlPart + ") as a5001, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5001' and MZ_SRANK Between 'G21' and 'G24' " + sqlPart + ") as b5001, " +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5001' and MZ_SRANK Between 'G11' and 'G14' " + sqlPart + ") as c5001, "  +
            //                        "(Select count(MZ_PRRST) From A_PRK2 Where MZ_PRRST='5001' and (MZ_SRANK Between 'G31' and 'G34'  or MZ_SRANK Between 'G21' and 'G24' or MZ_SRANK Between 'G11' and 'G14') " + sqlPart + ") as TTL5001 " +

            //                "FROM  " +
            //                "A_PRK2 ";

            //string where = string.Format(" WHERE MZ_DATE LIKE '%{0}%' AND MZ_CHKAD='{1}'",TextBox_MD.Text,DropDownList_EXAD.SelectedValue);
            //strSQL = strSQL + where;
            //DataTable gradeSumList = new DataTable();

            //gradeSumList.Columns.Add("G31_34", typeof(int));
            //gradeSumList.Columns.Add("G21_24", typeof(int));
            //gradeSumList.Columns.Add("G11_14", typeof(int));
            ////gradeSumList.Columns.Add("TOTAL" , typeof(int));

            //DataRow gradRow = gradeSumList.NewRow();
            ////寫不下去


            //gradeSumList = o_DBFactory.ABC_toTest.Create_Table(strSQL, "sumList");

            //Session["TITLE"] = DropDownList_EXAD.SelectedValue.Trim() + TextBox_MD.Text.Trim() + "月獎懲統計表";
            Session["TITLE"] = string.Format("{0}{1}月獎懲統計表", o_A_KTYPE.RAD(DropDownList_CHKAD.SelectedValue.Trim()), TextBox_MD.Text.Substring(0, 3) + "年" + TextBox_MD.Text.Substring(3, 2));
            Session["rpt_dt"] = gradeSumList;

            string tmp_url = "A_rpt.aspx?fn=gradeSumList&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MD.Text = string.Empty;
        }
    }
}
