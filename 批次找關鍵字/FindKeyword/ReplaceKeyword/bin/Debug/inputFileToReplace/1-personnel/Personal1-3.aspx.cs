using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;


namespace TPPDDB._1_personnel
{
    public partial class Personal1_3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            byte[] strBytes;
            string tempDlbaseData;
            lab_Msg.Text = "";
            //string DeleteString = "DELETE FROM A_TDLBASE";

            //o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

            string NOW = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            using (StreamReader sr = new StreamReader(this.FileUpload1.PostedFile.InputStream, System.Text.Encoding.Default))
            {
                tempDlbaseData = sr.ReadLine();
            }
            strBytes = Encoding.Default.GetBytes(tempDlbaseData);
            int s = strBytes.Length / 720;
            if (s > 0)
            {
                //try
                //{
                for (int i = 0; i < s; i++)
                {
                    string tempDlbaseDataPart = Encoding.Default.GetString(strBytes, i * 720, 720);
                    byte[] strBytes1;
                    strBytes1 = Encoding.Default.GetBytes(tempDlbaseDataPart);
                    string tempAD = Encoding.Default.GetString(strBytes1, 0, 10);//================================
                    string tempID = Encoding.Default.GetString(strBytes1, 10, 10);
                    string tempNAME = Encoding.Default.GetString(strBytes1, 20, 12);
                    string tempSHEET = Encoding.Default.GetString(strBytes1, 32, 2);
                    string tempChangCode = Encoding.Default.GetString(strBytes1, 34, 1);
                    string tempChangTime = Encoding.Default.GetString(strBytes1, 35, 13);
                    string tempDataTranferCode = Encoding.Default.GetString(strBytes1, 48, 6);
                    string tempPESN = Encoding.Default.GetString(strBytes1, 54, 2);
                    string tempCHISI = Encoding.Default.GetString(strBytes1, 56, 4);
                    string tempDataControl = Encoding.Default.GetString(strBytes1, 60, 3);
                    string tempPrepare = Encoding.Default.GetString(strBytes1, 63, 7);
                    string tempNickName = Encoding.Default.GetString(strBytes1, 70, 4);
                    string tempENAME = Encoding.Default.GetString(strBytes1, 74, 30);
                    string tempBIR = Encoding.Default.GetString(strBytes1, 104, 7);
                    string tempNationality = Encoding.Default.GetString(strBytes1, 111, 3);
                    string tempPassportNUM = Encoding.Default.GetString(strBytes1, 114, 10);
                    string tempSM = Encoding.Default.GetString(strBytes1, 124, 1);
                    string tempADD1 = Encoding.Default.GetString(strBytes1, 125, 60);
                    string tempADD2 = Encoding.Default.GetString(strBytes1, 185, 60);
                    string tempPHONE = Encoding.Default.GetString(strBytes1, 245, 12);
                    string tempEMNAM = Encoding.Default.GetString(strBytes1, 257, 12);
                    string tempPHONO = Encoding.Default.GetString(strBytes1, 269, 19);
                    string tempPHONH = Encoding.Default.GetString(strBytes1, 288, 12);
                    string tempLoan = Encoding.Default.GetString(strBytes1, 300, 1);
                    string tempARMYSTATE = Encoding.Default.GetString(strBytes1, 301, 1);
                    string tempARMYRANK = Encoding.Default.GetString(strBytes1, 302, 1);
                    string tempARMYKIND = Encoding.Default.GetString(strBytes1, 303, 2);
                    string tempARMYCOURSE = Encoding.Default.GetString(strBytes1, 305, 2);
                    string tempSLFDATE = Encoding.Default.GetString(strBytes1, 307, 7);
                    string tempSLEDATE = Encoding.Default.GetString(strBytes1, 314, 7);
                    string tempTBCD9 = Encoding.Default.GetString(strBytes1, 321, 4);
                    string tempFDATE = Encoding.Default.GetString(strBytes1, 325, 7);
                    string tempBreakYear = Encoding.Default.GetString(strBytes1, 332, 2);
                    string tempBreakMonth = Encoding.Default.GetString(strBytes1, 334, 2);
                    string tempZONE1 = Encoding.Default.GetString(strBytes1, 336, 3);
                    string tempZONE2 = Encoding.Default.GetString(strBytes1, 339, 3);
                    string tempHandicappedKind = Encoding.Default.GetString(strBytes1, 342, 2);
                    string tempHandicappedDegree = Encoding.Default.GetString(strBytes1, 344, 1);
                    string tempABORIGINE = Encoding.Default.GetString(strBytes1, 345, 1);
                    string tempABORIGINENAME = Encoding.Default.GetString(strBytes1, 346, 2);
                    string tempMOVETEL = Encoding.Default.GetString(strBytes1, 348, 12);
                    string tempEXAD = Encoding.Default.GetString(strBytes1, 430, 10); //=============================
                    string tempPOSIND = Encoding.Default.GetString(strBytes1, 440, 10);
                    string tempUNIT = Encoding.Default.GetString(strBytes1, 450, 4);//==============================
                    string tempEXUNITNAME = Encoding.Default.GetString(strBytes1, 454, 20);
                    string tempLackAD = Encoding.Default.GetString(strBytes1, 474, 10);
                    string tempLackUNIT = Encoding.Default.GetString(strBytes1, 484, 4);
                    string tempLackUNITNAME = Encoding.Default.GetString(strBytes1, 488, 20);
                    string tempOCCC = Encoding.Default.GetString(strBytes1, 508, 4);
                    string tempEXTPOS = Encoding.Default.GetString(strBytes1, 512, 4);
                    string tempRANK = Encoding.Default.GetString(strBytes1, 516, 3);
                    string tempRANK1 = Encoding.Default.GetString(strBytes1, 519, 3);
                    string tempRANK_1 = Encoding.Default.GetString(strBytes1, 522, 3);
                    string tempRANK1_1 = Encoding.Default.GetString(strBytes1, 525, 3);
                    string tempCHISI1 = Encoding.Default.GetString(strBytes1, 528, 4);
                    string tempPCHIEF = Encoding.Default.GetString(strBytes1, 532, 1);
                    string tempNREA = Encoding.Default.GetString(strBytes1, 533, 4);
                    string tempIDATE = Encoding.Default.GetString(strBytes1, 537, 7);
                    string tempDATE = Encoding.Default.GetString(strBytes1, 544, 7);
                    string tempNID = Encoding.Default.GetString(strBytes1, 551, 36);
                    string tempADATE = Encoding.Default.GetString(strBytes1, 587, 7);
                    string tempSRANK = Encoding.Default.GetString(strBytes1, 594, 3);
                    string tempSLVC = Encoding.Default.GetString(strBytes1, 597, 3);
                    string tempSPT = Encoding.Default.GetString(strBytes1, 600, 4);
                    string tempSPT1 = Encoding.Default.GetString(strBytes1, 604, 4);
                    string tempTBNREA = Encoding.Default.GetString(strBytes1, 608, 4);
                    string tempDismiss_IDATE = Encoding.Default.GetString(strBytes1, 612, 7);
                    string tempDismiss_DATE = Encoding.Default.GetString(strBytes1, 619, 7);
                    string tempDismiss_NREA = Encoding.Default.GetString(strBytes1, 626, 36);
                    string tempLDATE = Encoding.Default.GetString(strBytes1, 662, 7);
                    string tempNRT = Encoding.Default.GetString(strBytes1, 669, 1);
                    string tempPESN1 = Encoding.Default.GetString(strBytes1, 670, 2);
                    string tempRetireDate = Encoding.Default.GetString(strBytes1, 672, 5);
                    string tempOfficeTel = Encoding.Default.GetString(strBytes1, 677, 19);
                    string tempPAY_AD = Encoding.Default.GetString(strBytes1, 696, 10);
                    string tempChange = Encoding.Default.GetString(strBytes1, 706, 1);
                    string tempEXTPOS_RANK = Encoding.Default.GetString(strBytes1, 707, 3);
                    string tempEMPTY = Encoding.Default.GetString(strBytes1, 710, 10);
                    string tempSEX = tempID.Substring(1, 1);

                    string DeleteString = "DELETE FROM A_TDLBASE WHERE MZ_ID='" + tempID + "'";

                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    int x = 0;


                    string InsertString = "INSERT INTO A_TDLBASE (MZ_NAME,MZ_ID,MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_CHISI,MZ_POSIND"/*,MZ_TBDV*/+ ",MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_BIR,MZ_SLFDATE,MZ_SLEDATE,MZ_TBCD9,MZ_ZONE1,MZ_ADD1,MZ_ZONE2,MZ_ADD2,MZ_PHONE,MZ_MOVETEL,MZ_PCHIEF,MZ_NREA,MZ_DATE,MZ_IDATE,MZ_ADATE,MZ_NID,MZ_NRT,MZ_PESN,MZ_EMNAM,MZ_PHONO,MZ_PHONH,MZ_FDATE,MZ_ABORIGINE,MZ_ARMYSTATE,MZ_ARMYRANK,MZ_ARMYKIND,MZ_ARMYCOURSE,MZ_ABORIGINENAME,MZ_ENAME,MZ_EXTPOS,PAY_AD,MZ_LDATE,MZ_SM,MZ_SEX) VALUES (@MZ_NAME,@MZ_ID,@MZ_AD,@MZ_UNIT,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC,@MZ_RANK,@MZ_RANK1,@MZ_CHISI,@MZ_POSIND"/*,@MZ_TBDV*/+ ",@MZ_SRANK,@MZ_SLVC,@MZ_SPT,@MZ_SPT1,@MZ_BIR,@MZ_SLFDATE,@MZ_SLEDATE,@MZ_TBCD9,@MZ_ZONE1,@MZ_ADD1,@MZ_ZONE2,@MZ_ADD2,@MZ_PHONE,@MZ_MOVETEL,@MZ_PCHIEF,@MZ_NREA,@MZ_DATE,@MZ_IDATE,@MZ_ADATE,@MZ_NID,@MZ_NRT,@MZ_PESN,@MZ_EMNAM,@MZ_PHONO,@MZ_PHONH,@MZ_FDATE,@MZ_ABORIGINE,@MZ_ARMYSTATE,@MZ_ARMYRANK,@MZ_ARMYKIND,@MZ_ARMYCOURSE,@MZ_ABORIGINENAME,@MZ_ENAME,@MZ_EXTPOS,@PAY_AD,@MZ_LDATE,@MZ_SM,@MZ_SEX) ";
                    SqlParameter[] parameterList = {
                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = tempNAME.Replace("　","")},
                    new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = tempID.Replace("　","")},
                    new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = tempAD.Replace("　","")},
                    new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = tempUNIT.Replace("　","")},
                    new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = tempAD.Replace("　","")},
                    new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = tempUNIT.Replace("　","")},
                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = tempOCCC.Replace("　","")},
                    new SqlParameter("MZ_RANK",SqlDbType.VarChar){Value = tempRANK.Replace("　","")},
                    new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = tempRANK1.Replace("　","")},
                    new SqlParameter("MZ_CHISI",SqlDbType.VarChar){Value = tempCHISI.Replace("　","")},
                    new SqlParameter("MZ_POSIND",SqlDbType.VarChar){Value = tempPOSIND.Replace("　","")},
                  //new SqlParameter("MZ_TBDV",SqlDbType.VarChar){Value = tempTBDV},
                    new SqlParameter("MZ_SRANK",SqlDbType.VarChar){Value = tempSRANK.Replace("　","")},
                    new SqlParameter("MZ_SLVC",SqlDbType.VarChar){Value = tempSLVC.Replace("　","")},
                    new SqlParameter("MZ_SPT",SqlDbType.VarChar){Value = tempSPT.Replace("　","")},
                    new SqlParameter("MZ_SPT1",SqlDbType.VarChar){Value = tempSPT1.Replace("　","")},
                    new SqlParameter("MZ_BIR",SqlDbType.VarChar){Value = tempBIR.Replace("　","")},
                    new SqlParameter("MZ_SLFDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempSLFDATE.Replace("　",""))?"":tempSLFDATE.Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_SLEDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempSLEDATE.Replace("　",""))?"":tempSLEDATE.Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_TBCD9",SqlDbType.VarChar){Value = tempTBCD9.Replace("　","")},
                    new SqlParameter("MZ_ZONE1",SqlDbType.VarChar){Value = tempZONE1.Replace("　","")},
                    new SqlParameter("MZ_ADD1",SqlDbType.VarChar){Value = tempADD1.Replace("　","")},
                    new SqlParameter("MZ_ZONE2",SqlDbType.VarChar){Value = tempZONE2.Replace("　","")},
                    new SqlParameter("MZ_ADD2",SqlDbType.VarChar){Value = tempADD2.Replace("　","")},
                    new SqlParameter("MZ_PHONE",SqlDbType.VarChar){Value = tempPHONE.Replace("　","")},
                    new SqlParameter("MZ_MOVETEL",SqlDbType.VarChar){Value = tempMOVETEL.Replace("　","")},
                    new SqlParameter("MZ_PCHIEF",SqlDbType.VarChar){Value = tempPCHIEF.Replace("　","").Replace(" ","")},
                    new SqlParameter("MZ_NREA",SqlDbType.VarChar){Value = tempNREA.Replace("　","")},
                    new SqlParameter("MZ_DATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDATE.Replace("　",""))?"":tempDATE.Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_IDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempIDATE.Replace("　",""))?"":tempIDATE.Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_ADATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempADATE.Replace("　",""))?"":tempADATE.Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_NID",SqlDbType.VarChar){Value = tempNID.Replace("　","")},
                    new SqlParameter("MZ_NRT",SqlDbType.VarChar){Value = tempNRT.Replace("　","")},
                    new SqlParameter("MZ_PESN",SqlDbType.VarChar){Value = tempPESN.Replace("　","")},
                    new SqlParameter("MZ_EMNAM",SqlDbType.VarChar){Value = tempEMNAM.Replace("　","")},
                    new SqlParameter("MZ_PHONO",SqlDbType.VarChar){Value = tempPHONO.Replace("　","")},
                    new SqlParameter("MZ_PHONH",SqlDbType.VarChar){Value = tempPHONH.Replace("　","")},
                    new SqlParameter("MZ_FDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempFDATE.Replace("　",""))?NOW:tempFDATE.Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_ABORIGINE",SqlDbType.VarChar){Value = tempABORIGINE.Replace("　","")},
                    new SqlParameter("MZ_ARMYSTATE",SqlDbType.VarChar){Value = tempARMYSTATE.Replace("　","")},
                    new SqlParameter("MZ_ARMYRANK",SqlDbType.VarChar){Value = tempARMYRANK.Replace("　","")},
                    new SqlParameter("MZ_ARMYKIND",SqlDbType.VarChar){Value = tempARMYKIND.Replace("　","")},
                    new SqlParameter("MZ_ARMYCOURSE",SqlDbType.VarChar){Value = tempARMYCOURSE.Replace("　","")},
                    new SqlParameter("MZ_ABORIGINENAME",SqlDbType.VarChar){Value = tempABORIGINENAME.Replace("　","")},
                    new SqlParameter("MZ_ENAME",SqlDbType.VarChar){Value = tempENAME.Replace("　","")},
                    new SqlParameter("MZ_EXTPOS",SqlDbType.VarChar){Value = tempEXTPOS.Replace("　","")},
                    new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = tempPAY_AD.Replace("　","")},
                    new SqlParameter("MZ_LDATE",SqlDbType.VarChar){Value = tempLDATE.Replace("　","")},
                    new SqlParameter("MZ_SM",SqlDbType.VarChar){Value = tempSM.Replace("　","")},
                    new SqlParameter("MZ_SEX",SqlDbType.VarChar){Value = tempSEX.Replace("　","")}
                };
                    try
                    {
                        o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertString, parameterList);
                    }
                    catch (Exception ex)
                    {
                        lab_Msg.Text += tempNAME + ":" + ex.Message;
                    }
                }
                if(lab_Msg.Text=="")
                    Response.Write(@"<script language=javascript>window.alert('轉檔成功！');location.href('Personal1-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                //}
                //catch
                //{
                //    Response.Write(@"<script language=javascript>window.alert('轉檔失敗，請檢查檔案！');location.href('Personal1-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                //}
            }
            else
            {
                Response.Write(@"<script language=javascript>window.alert('資料格式有誤，請檢查');location.href('Personal1-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
            }
        }
    }
}
