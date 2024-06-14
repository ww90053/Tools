using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class B_SalarySole_ExcelToDB : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        DataTable dtSoleItem = new DataTable();
        double dblTaxPercent = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
            }
            Label_MSG.Text = "";
            Label_MSGT.Text = "";
        }

        protected void btExcelToDB_Click(object sender, EventArgs e)
        {
            ListBox_Data.Items.Clear();
            HttpPostedFile tUploadFile = FileUpload_ExcelToDB.PostedFile;

            if (FileUpload_ExcelToDB.HasFile)
            {
                if (bool_File_GetExtension(tUploadFile.FileName))
                {
                    if (bool_File_Search_GetExtension(tUploadFile.FileName))
                    {
                        Label_MSG.Text = "匯入成功";
                        Label_MSG.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        Label_MSG.Text = "匯入失敗";
                        Label_MSG.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    Label_MSG.Text = "請上傳Excel 格式檔";
                    Label_MSG.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                Label_MSG.Text = "請瀏覽檔案來源";
                Label_MSG.ForeColor = System.Drawing.Color.Red;
            }

            foreach (string item in listStrPass_Data)
            {
                ListBox_Data.Items.Add(item.ToString());
            }

            Label_MSGT.Text = "已產生筆數：" + listIntPass_Data.ToString();
        }

        private bool bool_File_GetExtension(string strFileName)
        {
            switch (Path.GetExtension(strFileName))
            {
                case ".xls":
                case ".xlsx":
                    return true;
                default:
                    return false;
            }
        }

        private static List<String> listStrPass_Data = new List<string>();
        private static int listIntPass_Data = 0;

        private bool bool_File_Search_GetExtension(string strDataSource_Data)
        {
            #region 上傳檔案到伺服器

            HttpPostedFile tUploadFile = FileUpload_ExcelToDB.PostedFile;
            int tFileLength = tUploadFile.ContentLength;
            byte[] tFileByte = new byte[tFileLength];
            tUploadFile.InputStream.Read(tFileByte, 0, tFileLength);

            FileStream tNewfile = new FileStream(System.Web.HttpContext.Current.Server.MapPath("upload/") + DateTime.Now.ToString("yyyyMMddhhmm") + "_upload.xls", FileMode.Create);
            tNewfile.Write(tFileByte, 0, tFileByte.Length);
            tNewfile.Close();

            #endregion

            // 這是一個全域變數，記錄excel的上傳路徑  
            string strFilePath = tNewfile.Name;

            // 取得所有的人事資料
            dt = o_DBFactory.ABC_toTest.Create_Table(@"(SELECT MZ_ID, MZ_NAME, MZ_POLNO FROM A_DLBASE) 
                                            UNION ALL 
                                            (SELECT IDCARD MZ_ID, NAME MZ_NAME, '' MZ_POLNO FROM B_MANUFACTURER_BASE)", "VW");
            // 取得單一發放項目資料
            dtSoleItem = o_DBFactory.ABC_toTest.Create_Table("SELECT ID, TAXES_YESNO, TAXES_ID FROM B_SOLEITEM", "VW");
            // 取得所得稅率
            dblTaxPercent = double.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT TAX_PERCENTAGE FROM B_TAX_SET").ToString());

            listStrPass_Data.Clear();
            listIntPass_Data = 0;

            String ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=Excel 12.0;";

            OleDbConnection objConn = new OleDbConnection(ConnString);
            string strCom = "SELECT * FROM [SOLE$]";

            objConn.Open();

            OleDbDataAdapter objCmd = new OleDbDataAdapter(strCom, objConn);
            DataSet objDS = new DataSet();

            objCmd.Fill(objDS);
            objConn.Close();

            //XX2013/06/18 
            objConn.Dispose();

            foreach (DataRow item in objDS.Tables[0].Rows)
            {
                string strIDCARD = item[0].ToString();
                string strMZ_NAME = dt.Select(string.Format(" MZ_ID = '{0}'", strIDCARD))[0]["MZ_NAME"].ToString();
                string strMZ_POLNO = dt.Select(string.Format(" MZ_ID = '{0}'", strIDCARD))[0]["MZ_POLNO"].ToString();
                _strMZ_POLNO = strMZ_POLNO;
                string strDA_INOUT_GROUP = "IN";
                string strDA = item[1].ToString();
                string strCASEID = item[2].ToString();
                string strNUM = DropDownList_NUM.SelectedValue;
                string strTAXES_ID1 = dtSoleItem.Select(string.Format(" ID = '{0}'", strNUM))[0]["TAXES_ID"].ToString();
                string strNOTE = "";// item[8].ToString();
                int intPAY = int.Parse(item[3].ToString());
                int intTAX = 0;
                int intPAY1 = 0;// int.Parse(item[11].ToString());
                int intPAY2 = 0;// int.Parse(item[12].ToString());
                int intPAY3 = 0;// int.Parse(item[13].ToString());

                string strTAXES_ID = SalarySole.strTAXES_ID(strNUM);
                string strTAXFLAG = "";
                if (SalarySole.boolTAXES_YESNO(strNUM))
                {
                    strTAXFLAG = "Y";
                }
                else
                {
                    strTAXFLAG = "N";
                }

                if (strTAXFLAG == "Y")// 要扣稅
                {
                    intTAX = SalaryPublic.intRound_Data(intPAY * dblTaxPercent);
                }

                intS_SIND_Data = SalarySole.intSole_S_SIND_Data(strIDCARD, strMZ_NAME, strMZ_POLNO, strDA_INOUT_GROUP, strDA, strCASEID, strNUM, strTAXES_ID1);

                if (intS_SIND_Data != 0)
                {
                    //20140701
                   // if (SalarySole.boolSole_Update(intS_SIND_Data, strIDCARD, strMZ_POLNO, strPAY_AD, strMZ_NAME, SalaryPublic.strMZ_ID_TO_MZ_OCCC_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_SRANK_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_SLVC_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_SPT_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_UNIT_Data(strIDCARD), strLOCKDB, strDA_INOUT_GROUP, strDA, strCASEID, strNUM, strTAXES_ID, strTAXES_ID1, intPAY, strTAXFLAG, intTAX, intPAY1, intPAY2, intPAY3, strNOTE) == false)

                    if (SalarySole.boolSole_Update(intS_SIND_Data, strIDCARD, strMZ_POLNO, strPAY_AD, strMZ_NAME,
                        SalaryPublic.str_A_Column("MZ_OCCC", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_SRANK", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_SLVC", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_SPT", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_UNIT", 1, strIDCARD),
                        strLOCKDB, strDA_INOUT_GROUP, strDA, strCASEID, strNUM, strTAXES_ID, strTAXES_ID1, intPAY, strTAXFLAG, intTAX, intPAY1, intPAY2, intPAY3, strNOTE) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    //20140701
                    //if (SalarySole.boolSole_Create(strIDCARD, strMZ_POLNO, strPAY_AD, strMZ_NAME, SalaryPublic.strMZ_ID_TO_MZ_OCCC_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_SRANK_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_SLVC_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_SPT_Data(strIDCARD), SalaryPublic.strMZ_ID_TO_MZ_UNIT_Data(strIDCARD), "N", strDA_INOUT_GROUP, strDA, strCASEID, strNUM, strTAXES_ID, strTAXES_ID1, intPAY, strTAXFLAG, intTAX, intPAY1, intPAY2, intPAY3, strNOTE) == false)

                    if (SalarySole.boolSole_Create(strIDCARD, strMZ_POLNO, strPAY_AD, strMZ_NAME,
                        SalaryPublic.str_A_Column("MZ_OCCC", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_SRANK", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_SLVC", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_SPT", 1, strIDCARD), SalaryPublic.str_A_Column("MZ_UNIT", 1, strIDCARD),
                        "N", strDA_INOUT_GROUP, strDA, strCASEID, strNUM, strTAXES_ID, strTAXES_ID1, intPAY, strTAXFLAG, intTAX, intPAY1, intPAY2, intPAY3, strNOTE) == false)
                    {
                        return false;
                    }
                }

                listStrPass_Data.Add(strMZ_POLNO);
                listIntPass_Data = listIntPass_Data + 1;
            }
            return true;
        }

        private static int intS_SIND_Data = 0;

        private string strPAY_AD
        {
            get
            {
                return Session["ADPMZ_EXAD"].ToString();
            }
        }

        private static string _strMZ_POLNO;

        private string strMZ_OCCC
        {
            get
            {
                string MZ_OCCC = "";
                switch (_strMZ_POLNO.Substring(0, 3))
                {
                    case "BSN":
                    case "HSN":
                        MZ_OCCC = "";
                        break;
                    default:
                        //20140701
                        //MZ_OCCC = SalaryPublic.strMZ_OCCC_Data(_strMZ_POLNO);
                        MZ_OCCC = SalaryPublic.str_A_Column("MZ_OCCC", 2, _strMZ_POLNO);
                        break;
                }
                return MZ_OCCC;
            }
        }

        private string strMZ_SRANK
        {
            get
            {
                string MZ_SRANK = "";
                switch (_strMZ_POLNO.Substring(0, 3))
                {
                    case "BSN":
                    case "HSN":
                        MZ_SRANK = "";
                        break;
                    default:
                        //20140701
                        //MZ_SRANK = SalaryPublic.strMZ_SRANK_Data(_strMZ_POLNO);
                        MZ_SRANK = SalaryPublic.str_A_Column("MZ_SRANK", 2, _strMZ_POLNO);
                        break;
                }
                return MZ_SRANK;
            }
        }

        private string strMZ_SLVC
        {
            get
            {
                string MZ_SLVC = "";
                switch (_strMZ_POLNO.Substring(0, 3))
                {
                    case "BSN":
                    case "HSN":
                        MZ_SLVC = "";
                        break;
                    default:
                        //20140701
                        MZ_SLVC = SalaryPublic.str_A_Column("MZ_SLVC", 2, _strMZ_POLNO);
                        //MZ_SLVC = SalaryPublic.strMZ_SLVC_Data(_strMZ_POLNO);
                        break;
                }
                return MZ_SLVC;
            }
        }

        private string strMZ_SPT
        {
            get
            {
                string MZ_SPT = "";
                switch (_strMZ_POLNO.Substring(0, 3))
                {
                    case "BSN":
                    case "HSN":
                        MZ_SPT = "";
                        break;
                    default:
                        //20140701
                        MZ_SPT = SalaryPublic.str_A_Column("MZ_SPT", 2, _strMZ_POLNO);
                        //MZ_SPT = SalaryPublic.strMZ_SPT_Data(_strMZ_POLNO);
                        break;
                }
                return MZ_SPT;
            }
        }

        private string strMZ_UNIT
        {
            get
            {
                return "";
            }
        }

        private string strLOCKDB
        {
            get
            {
                string LOCKDB = "Y";
                if (intS_SIND_Data != 0)
                {
                    LOCKDB = SalarySole.str_LOCKDB_Data_Serach(intS_SIND_Data);
                }
                return LOCKDB;
            }
        }
    }
}
