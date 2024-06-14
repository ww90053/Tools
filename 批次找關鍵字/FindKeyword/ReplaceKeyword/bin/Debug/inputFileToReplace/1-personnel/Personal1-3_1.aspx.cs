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
using NPOI.HSSF.UserModel;
using NPOI.SS;


namespace TPPDDB._1_personnel
{
    public partial class Personal1_3_1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable tempDT = new DataTable();
            tempDT = ConvertCSVtoDataTable(true);
            string empty = "";
            string err = "";
            string NOW = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            
            string DeleteString = "DELETE FROM A_TDLBASE WHERE 1=1";
            o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
               
                try
                {
                    //string DeleteString = "DELETE FROM A_TDLBASE WHERE MZ_ID='" + tempDT.Rows[i][0].ToString() + "'";

                    //o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    int x = 0;


                    string InsertString = "INSERT INTO A_TDLBASE (MZ_NAME,MZ_ID,MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_CHISI,MZ_POSIND,MZ_TBDV,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_BIR,MZ_SLFDATE,MZ_SLEDATE,MZ_OFFYY,MZ_OFFMM,MZ_TBCD9,MZ_ZONE1,MZ_ADD1,MZ_ZONE2,MZ_ADD2,MZ_PHONE,MZ_MOVETEL,MZ_PCHIEF,MZ_NREA,MZ_DATE,MZ_IDATE,MZ_ADATE,MZ_NID,MZ_NRT,MZ_PESN,MZ_EMNAM,MZ_PHONO,MZ_PHONH,MZ_FDATE,MZ_PNO,MZ_ABORIGINE,MZ_ARMYSTATE,MZ_ARMYRANK,MZ_ARMYKIND,MZ_ARMYCOURSE,MZ_ABORIGINENAME,MZ_ENAME,MZ_EXTPOS,PAY_AD,MZ_LDATE,MZ_SM,MZ_SEX) VALUES (@MZ_NAME,@MZ_ID,@MZ_AD,@MZ_UNIT,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC,@MZ_RANK,@MZ_RANK1,@MZ_CHISI,@MZ_POSIND,@MZ_TBDV,@MZ_SRANK,@MZ_SLVC,@MZ_SPT,@MZ_SPT1,@MZ_BIR,@MZ_SLFDATE,@MZ_SLEDATE,@MZ_OFFYY,@MZ_OFFMM,@MZ_TBCD9,@MZ_ZONE1,@MZ_ADD1,@MZ_ZONE2,@MZ_ADD2,@MZ_PHONE,@MZ_MOVETEL,@MZ_PCHIEF,@MZ_NREA,@MZ_DATE,@MZ_IDATE,@MZ_ADATE,@MZ_NID,@MZ_NRT,@MZ_PESN,@MZ_EMNAM,@MZ_PHONO,@MZ_PHONH,@MZ_FDATE,@MZ_PNO,@MZ_ABORIGINE,@MZ_ARMYSTATE,@MZ_ARMYRANK,@MZ_ARMYKIND,@MZ_ARMYCOURSE,@MZ_ABORIGINENAME,@MZ_ENAME,@MZ_EXTPOS,@PAY_AD,@MZ_LDATE,@MZ_SM,@MZ_SEX) ";
                    SqlParameter[] parameterList = {
                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = tempDT.Rows[i][0].ToString().Replace("　","")},
                    new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = tempDT.Rows[i][1].ToString().Replace("　","")},
                    new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = tempDT.Rows[i][2].ToString().Replace("　","")},
                    new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = tempDT.Rows[i][4].ToString().Replace("　","")},
                    new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = empty},
                    new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = empty},
                    new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = tempDT.Rows[i][5].ToString().Replace("　","")},
                    new SqlParameter("MZ_RANK",SqlDbType.VarChar){Value = tempDT.Rows[i][7].ToString().Replace("　","")},
                    new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = tempDT.Rows[i][9].ToString().Replace("　","")},
                    new SqlParameter("MZ_CHISI",SqlDbType.VarChar){Value = tempDT.Rows[i][11].ToString().Replace("　","")},
                    new SqlParameter("MZ_POSIND",SqlDbType.VarChar){Value = tempDT.Rows[i][13].ToString().Replace("　","")},
                    new SqlParameter("MZ_TBDV",SqlDbType.VarChar){Value = tempDT.Rows[i][14].ToString()},
                    new SqlParameter("MZ_SRANK",SqlDbType.VarChar){Value = tempDT.Rows[i][16].ToString().Replace("　","")},
                    new SqlParameter("MZ_SLVC",SqlDbType.VarChar){Value = tempDT.Rows[i][18].ToString().Replace("　","")},
                    new SqlParameter("MZ_SPT",SqlDbType.VarChar){Value = tempDT.Rows[i][20].ToString().Replace("　","")},
                    new SqlParameter("MZ_SPT1",SqlDbType.VarChar){Value = tempDT.Rows[i][21].ToString().Replace("　","")},
                    new SqlParameter("MZ_BIR",SqlDbType.VarChar){Value = tempDT.Rows[i][24].ToString().Replace("　","")},
                    new SqlParameter("MZ_SLFDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDT.Rows[i][27].ToString().Replace("　",""))?"":tempDT.Rows[i][27].ToString().Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_SLEDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDT.Rows[i][28].ToString().Replace("　",""))?"":tempDT.Rows[i][28].ToString().Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_OFFYY",SqlDbType.VarChar){Value = tempDT.Rows[i][29].ToString().Replace("　","")},
                    new SqlParameter("MZ_OFFMM",SqlDbType.VarChar){Value = tempDT.Rows[i][30].ToString().Replace("　","")},
                    new SqlParameter("MZ_TBCD9",SqlDbType.VarChar){Value = tempDT.Rows[i][31].ToString().Replace("　","")},
                    new SqlParameter("MZ_ZONE1",SqlDbType.VarChar){Value = tempDT.Rows[i][33].ToString().Replace("　","")},
                    new SqlParameter("MZ_ADD1",SqlDbType.VarChar){Value = tempDT.Rows[i][34].ToString().Replace("　","")},
                    new SqlParameter("MZ_ZONE2",SqlDbType.VarChar){Value = tempDT.Rows[i][35].ToString().Replace("　","")},
                    new SqlParameter("MZ_ADD2",SqlDbType.VarChar){Value = tempDT.Rows[i][36].ToString().Replace("　","")},
                    new SqlParameter("MZ_PHONE",SqlDbType.VarChar){Value = tempDT.Rows[i][37].ToString().Replace("　","")},
                    new SqlParameter("MZ_MOVETEL",SqlDbType.VarChar){Value = tempDT.Rows[i][38].ToString().Replace("　","")},
                    new SqlParameter("MZ_PCHIEF",SqlDbType.VarChar){Value = tempDT.Rows[i][39].ToString().Replace("　","")},
                    new SqlParameter("MZ_NREA",SqlDbType.VarChar){Value = tempDT.Rows[i][41].ToString().Replace("　","")},
                    new SqlParameter("MZ_DATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDT.Rows[i][43].ToString().Replace("　",""))?"":tempDT.Rows[i][43].ToString().Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_IDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDT.Rows[i][44].ToString().Replace("　",""))?"":tempDT.Rows[i][44].ToString().Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_ADATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDT.Rows[i][45].ToString().Replace("　",""))?"":tempDT.Rows[i][45].ToString().Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_NID",SqlDbType.VarChar){Value = tempDT.Rows[i][46].ToString().Replace("　","")},
                    new SqlParameter("MZ_NRT",SqlDbType.VarChar){Value = tempDT.Rows[i][47].ToString().Replace("　","")},
                    new SqlParameter("MZ_PESN",SqlDbType.VarChar){Value = tempDT.Rows[i][48].ToString().Replace("　","")},
                    new SqlParameter("MZ_EMNAM",SqlDbType.VarChar){Value = tempDT.Rows[i][50].ToString().Replace("　","")},
                    new SqlParameter("MZ_PHONO",SqlDbType.VarChar){Value = tempDT.Rows[i][51].ToString().Replace("　","")},
                    new SqlParameter("MZ_PHONH",SqlDbType.VarChar){Value = tempDT.Rows[i][52].ToString().Replace("　","")},
                    new SqlParameter("MZ_FDATE",SqlDbType.VarChar){Value = string.IsNullOrEmpty(tempDT.Rows[i][53].ToString().Replace("　",""))?NOW:tempDT.Rows[i][53].ToString().Replace("　","").PadLeft(7,'0')},
                    new SqlParameter("MZ_PNO",SqlDbType.VarChar){Value = tempDT.Rows[i][55].ToString().Replace("　","")},
                    new SqlParameter("MZ_ABORIGINE",SqlDbType.VarChar){Value = tempDT.Rows[i][56].ToString().Replace("　","")},
                    new SqlParameter("MZ_ARMYSTATE",SqlDbType.VarChar){Value = tempDT.Rows[i][58].ToString().Replace("　","")},
                    new SqlParameter("MZ_ARMYRANK",SqlDbType.VarChar){Value = tempDT.Rows[i][60].ToString().Replace("　","")},
                    new SqlParameter("MZ_ARMYKIND",SqlDbType.VarChar){Value = tempDT.Rows[i][62].ToString().Replace("　","")},
                    new SqlParameter("MZ_ARMYCOURSE",SqlDbType.VarChar){Value = tempDT.Rows[i][64].ToString().Replace("　","")},
                    new SqlParameter("MZ_ABORIGINENAME",SqlDbType.VarChar){Value = empty.Replace("　","")},
                    new SqlParameter("MZ_ENAME",SqlDbType.VarChar){Value = tempDT.Rows[i][66].ToString().Replace("　","")},
                    new SqlParameter("MZ_EXTPOS",SqlDbType.VarChar){Value = empty.ToString().Replace("　","")},
                    new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = empty.Replace("　","")},
                    new SqlParameter("MZ_LDATE",SqlDbType.VarChar){Value = tempDT.Rows[i][67].ToString().Replace("　","")},
                    new SqlParameter("MZ_SM",SqlDbType.VarChar){Value = tempDT.Rows[i][25].ToString().Replace("　","")},
                    new SqlParameter("MZ_SEX",SqlDbType.VarChar){Value = tempDT.Rows[i][22].ToString().Replace("　","")}
                };
                    try
                    {
                        if (!String.IsNullOrEmpty(parameterList[22].Value.ToString()) && parameterList[22].Value.ToString().Length>3)
                        {
                            parameterList[22].Value = parameterList[22].Value.ToString().Substring(0, 3);
                        }
                        if (!String.IsNullOrEmpty(parameterList[24].Value.ToString()) && parameterList[24].Value.ToString().Length > 3)
                        {
                            parameterList[24].Value = parameterList[24].Value.ToString().Substring(0, 3);
                        }
                        o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertString, parameterList);
                      
                        
 
                    
                    }
                    catch (Exception ex)
                    {
                        err +=tempDT.Rows[i][0].ToString()+tempDT.Rows[i][1].ToString()+"、";
                    }

                  
                }
                catch
                {
                    
                }
 
            }
            if (String.IsNullOrEmpty(err))
                Response.Write(@"<script language=javascript>window.alert('轉檔完成！ 筆數:" + tempDT.Rows.Count + "');</script>");
            else
            {
                var errcount = err.Split('、');
                if(err.Length<1000)
                    Response.Write(@"<script language=javascript>window.alert('轉檔失敗！ 成功:" + (tempDT.Rows.Count - errcount.Length) + " 失敗:" + errcount.Length + " 失敗人員:" + err + "');</script>");
                else
                    Response.Write(@"<script language=javascript>window.alert('轉檔失敗！ 成功:" + (tempDT.Rows.Count - errcount.Length) + " 失敗:" + errcount.Length + "');</script>");

            }
               
        }

        public DataTable ConvertCSVtoDataTable(bool firstRowAsHeader)
        {
            DataTable t = new DataTable();
            //using (StreamReader sr = new StreamReader(this.FileUpload.PostedFile.InputStream, System.Text.Encoding.Default))
            //{
            //    string line = null;
            //    bool colCreated = false;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] p = line.Split(',');
            //        if (!colCreated)
            //        {
            //            int idx = 1;
            //            foreach (string s in p)
            //            {
            //                try
            //                {
            //                    t.Columns.Add(
            //                        //第一欄是否有欄位名稱? 無則用C1, C2自動編號
            //                        firstRowAsHeader ? s : "C" + idx.ToString(),
            //                        typeof(string)
            //                        );
            //                }
            //                catch (Exception e1)
            //                {
            //                    throw new ApplicationException(
            //                        "新增欄位失敗! 欄位名稱=" + s + "\n" + e1.ToString());
            //                }
            //                idx++;
            //            }
            //            colCreated = true;
            //            //首欄若為欄名，則不當資料處理
            //            if (firstRowAsHeader) continue;
            //        }
            //        try
            //        {
            //            t.Rows.Add(p);
            //        }
            //        catch (Exception e2)
            //        {
            //            throw new ApplicationException("資料匯入失敗!\n資料=" + line +
            //                "\n錯誤訊息:" + e2.ToString());
            //        }
            //    }
            //}


            HSSFWorkbook wb = new HSSFWorkbook(FileUpload_Excel.FileContent);
            HSSFSheet sheet = wb.GetSheetAt(0);
            DataTable table = new DataTable();
            //由第一列取標題做為欄位名稱
            HSSFRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                //以欄位文字為名新增欄位，此處全視為字串型別以求簡化
                table.Columns.Add(
                    new DataColumn(headerRow.GetCell(i).StringCellValue));

            //略過第零列(標題列)，一直處理至最後一列
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i);
                if (row == null) continue;
                DataRow dataRow = table.NewRow();
                //依先前取得的欄位數逐一設定欄位內容
                for (int j = row.FirstCellNum; j < cellCount; j++)
                    if (row.GetCell(j) != null)
                        //如要針對不同型別做個別處理，可善用.CellType判斷型別
                        //再用.StringCellValue, .DateCellValue, .NumericCellValue...取值
                        //此處只簡單轉成字串
                        dataRow[j] = row.GetCell(j).ToString();
                table.Rows.Add(dataRow);
            }

            //HSSFWorkbook workbook = new HSSFWorkbook(FileUpload_Excel.FileContent);
            //HSSFSheet sheet = workbook.GetSheet("Sheet1");

            //HSSFRow headerRow = sheet.GetRow(1);
            //List<string> fileHeader = new List<string>();
            //for (int i = 0; i < headerRow.LastCellNum; i++)
            //{
            //    fileHeader.Add(headerRow.GetCell(i).StringCellValue);
            //}         
            return table;
        }

        protected string NAME_TO_CODE(string NAME, string KTYPE)
        {
            string MZ_KCODE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI='" + NAME + "' AND MZ_KTYPE='" + KTYPE + "'");

            return MZ_KCODE;
        }
    }
}
