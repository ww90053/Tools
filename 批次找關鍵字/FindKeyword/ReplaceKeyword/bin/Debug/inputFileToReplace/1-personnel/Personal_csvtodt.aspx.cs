using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace TPPDDB._1_personnel
{
    public partial class Personal_csvtodt : System.Web.UI.Page
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

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                tempDT.Rows[i][0] = NAME_TO_CODE(tempDT.Rows[i][0].ToString(), "04");//AD
                tempDT.Rows[i][1] = NAME_TO_CODE(tempDT.Rows[i][1].ToString(), "25");//UNIT
                tempDT.Rows[i][3] = NAME_TO_CODE(tempDT.Rows[i][3].ToString(), "26");//OCCC

                if (!string.IsNullOrEmpty(tempDT.Rows[i][4].ToString()))
                {
                    string[] RANK = tempDT.Rows[i][4].ToString().Trim().Replace("　", string.Empty).Split('至');
                    tempDT.Rows[i][4] = NAME_TO_CODE(RANK[0], "09") + "至" + NAME_TO_CODE(RANK[1], "09");//RANK
                }

                tempDT.Rows[i][7] = tempDT.Rows[i][7].ToString().PadLeft(3, '0');//職序

                tempDT.Rows[i][13] = tempDT.Rows[i][13].ToString();//學校
                tempDT.Rows[i][16] = tempDT.Rows[i][16].ToString();//科系

                tempDT.Rows[i][17] = NAME_TO_CODE(tempDT.Rows[i][17].ToString(), "@14");//程度
                tempDT.Rows[i][18] = NAME_TO_CODE(tempDT.Rows[i][18].ToString(), "EDT");//狀態
                tempDT.Rows[i][21] = NAME_TO_CODE(tempDT.Rows[i][21].ToString(), "EXK");//考試名稱
                tempDT.Rows[i][22] = NAME_TO_CODE(tempDT.Rows[i][22].ToString(), "EXS");//考試類別
                tempDT.Rows[i][23] = NAME_TO_CODE(tempDT.Rows[i][23].ToString(), "EXG");//錄取等地
                tempDT.Rows[i][24] = NAME_TO_CODE(tempDT.Rows[i][24].ToString(), "09");//薪俸職等
                tempDT.Rows[i][25] = NAME_TO_CODE(tempDT.Rows[i][25].ToString(), "64");//俸階

               

                #region 經歷一
                tempDT.Rows[i][31] = NAME_TO_CODE(tempDT.Rows[i][31].ToString(), "04");//經歷一：機關

                //20170503 舊寫法_因應EXCEL欄位修正調整
                #region 舊寫法
                //string[] RANK1 = new string[] { };
                //if (!string.IsNullOrEmpty(tempDT.Rows[i][32].ToString()))
                //{
                //    RANK1 = tempDT.Rows[i][32].ToString().Trim().Replace("　", string.Empty).Split('至');
                //    tempDT.Rows[i][32] = NAME_TO_CODE(RANK1[0], "09") + "至" + NAME_TO_CODE(RANK1[1], "09");//經歷一：官職起迄
                //}
   
                //tempDT.Rows[i][33] = NAME_TO_CODE(tempDT.Rows[i][33].ToString(), "25");//經歷一：單位
                //tempDT.Rows[i][34] = NAME_TO_CODE(tempDT.Rows[i][34].ToString(), "26");//經歷一：職稱

                //tempDT.Rows[i][35] = NAME_TO_CODE(tempDT.Rows[i][35].ToString(), "04");//經歷二：機關

                //string[] RANK2 = new string[] { };

                //if (!string.IsNullOrEmpty(tempDT.Rows[i][36].ToString()))
                //{
                //    RANK2 = tempDT.Rows[i][36].ToString().Trim().Replace("　", string.Empty).Split('至');
                //    tempDT.Rows[i][36] = NAME_TO_CODE(RANK2[0], "09") + "至" + NAME_TO_CODE(RANK2[1], "09");//經歷二：官職起迄
                //}
                //tempDT.Rows[i][37] = NAME_TO_CODE(tempDT.Rows[i][37].ToString(), "25");//經歷二：單位
                //tempDT.Rows[i][38] = NAME_TO_CODE(tempDT.Rows[i][38].ToString(), "26");//經歷二：職稱

                //string[] RANK3 = new string[] { };

                //tempDT.Rows[i][39] = NAME_TO_CODE(tempDT.Rows[i][39].ToString(), "04");//經歷三：機關
                //if (!string.IsNullOrEmpty(tempDT.Rows[i][40].ToString()))
                //{
                //    RANK3 = tempDT.Rows[i][40].ToString().Trim().Replace("　", string.Empty).Split('至');
                //    tempDT.Rows[i][40] = NAME_TO_CODE(RANK3[0], "09") + "至" + NAME_TO_CODE(RANK3[1], "09");//經歷三：官職起迄
                //}
                //tempDT.Rows[i][41] = NAME_TO_CODE(tempDT.Rows[i][41].ToString(), "25");//經歷三：單位
                //tempDT.Rows[i][42] = NAME_TO_CODE(tempDT.Rows[i][42].ToString(), "26");//經歷三：職稱
                #endregion

                if (!string.IsNullOrEmpty(tempDT.Rows[i][32].ToString()))
                {
                    tempDT.Rows[i][32] = NAME_TO_CODE(tempDT.Rows[i][32].ToString(), "09");//經歷一：官職等一
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][34].ToString()))
                {
                    tempDT.Rows[i][34] = NAME_TO_CODE(tempDT.Rows[i][34].ToString(), "09");//經歷一：官職等二
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][35].ToString()))
                {
                    tempDT.Rows[i][35] = NAME_TO_CODE(tempDT.Rows[i][35].ToString(), "25");//經歷一：單位
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][36].ToString()))
                {
                    tempDT.Rows[i][36] = NAME_TO_CODE(tempDT.Rows[i][36].ToString(), "26");//經歷一：職稱
                }
                #endregion

                #region 經歷二
                if (!string.IsNullOrEmpty(tempDT.Rows[i][37].ToString()))
                {
                    tempDT.Rows[i][37] = NAME_TO_CODE(tempDT.Rows[i][37].ToString(), "04");//經歷二：機關
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][38].ToString()))
                {
                    tempDT.Rows[i][38] = NAME_TO_CODE(tempDT.Rows[i][38].ToString(), "09");//經歷二：官職等一
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][40].ToString()))
                {
                    tempDT.Rows[i][40] = NAME_TO_CODE(tempDT.Rows[i][40].ToString(), "09");//經歷二：官職等二
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][41].ToString()))
                {
                    tempDT.Rows[i][41] = NAME_TO_CODE(tempDT.Rows[i][41].ToString(), "25");//經歷二：單位
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][42].ToString()))
                {
                    tempDT.Rows[i][42] = NAME_TO_CODE(tempDT.Rows[i][42].ToString(), "26");//經歷二：職稱
                }
                #endregion

                #region 經歷三
                if (!string.IsNullOrEmpty(tempDT.Rows[i][43].ToString()))
                {
                    tempDT.Rows[i][43] = NAME_TO_CODE(tempDT.Rows[i][43].ToString(), "04");//經歷三：機關
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][44].ToString()))
                {
                    tempDT.Rows[i][44] = NAME_TO_CODE(tempDT.Rows[i][44].ToString(), "09");//經歷三：官職等一
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][46].ToString()))
                {
                    tempDT.Rows[i][46] = NAME_TO_CODE(tempDT.Rows[i][46].ToString(), "09");//經歷三：官職等二
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][47].ToString()))
                {
                    tempDT.Rows[i][47] = NAME_TO_CODE(tempDT.Rows[i][47].ToString(), "25");//經歷三：單位
                }
                if (!string.IsNullOrEmpty(tempDT.Rows[i][48].ToString()))
                {
                    tempDT.Rows[i][48] = NAME_TO_CODE(tempDT.Rows[i][48].ToString(), "26");//經歷三：職稱
                }
                #endregion

                string Delete_Exam_SQL = "DELETE FROM A_EXAM_TEMP WHERE MZ_ID='" + tempDT.Rows[i][8].ToString() + "'";

                string Insert_Exam_SQL = "";

                if (!string.IsNullOrEmpty(tempDT.Rows[i][21].ToString()))
                    try
                    {
                        Insert_Exam_SQL = "INSERT INTO A_EXAM_TEMP VALUES('" + tempDT.Rows[i][8].ToString() + "','" + tempDT.Rows[i][21].ToString() + "','" + tempDT.Rows[i][22].ToString() + "','" + tempDT.Rows[i][23].ToString() + "','" + tempDT.Rows[i][19].ToString() + "')";
                    }
                    catch
                    {
                        Insert_Exam_SQL = "";
                    }
                string Delete_Edu_SQL = "DELETE FROM A_EDUCATION_TEMP WHERE MZ_ID='" + tempDT.Rows[i][8].ToString() + "'";

                string Insert_Edu_SQL = "";

                if (!string.IsNullOrEmpty(tempDT.Rows[i][13].ToString()))
                    try
                    {
                        Insert_Edu_SQL = "INSERT INTO A_EDUCATION_TEMP VALUES('" + tempDT.Rows[i][8].ToString() + "','" + tempDT.Rows[i][13].ToString() + "','" + tempDT.Rows[i][16].ToString() + "','" + tempDT.Rows[i][14].ToString() + "','" + tempDT.Rows[i][18].ToString() + "')";
                    }
                    catch
                    {
                        Insert_Edu_SQL = "";
                    }

                string Delete_TBDV_SQL = "DELETE FROM A_TBDV_TEMP WHERE MZ_ID='" + tempDT.Rows[i][8].ToString() + "'";

                string Insert_TBDV_SQL = "";

                if (!string.IsNullOrEmpty(tempDT.Rows[i][7].ToString()))
                    try
                    {
                        Insert_TBDV_SQL = "INSERT INTO A_TBDV_TEMP VALUES('" + tempDT.Rows[i][8].ToString() + "','" + tempDT.Rows[i][7].ToString() + "')";
                    }
                    catch
                    {
                        Insert_TBDV_SQL = "";
                    }

                string Delete_Career_SQL = "DELETE FROM A_CAREER_TEMP WHERE MZ_ID='" + tempDT.Rows[i][8].ToString() + "'";
                string Insert_Career1_SQL = "";

                if (!string.IsNullOrEmpty(tempDT.Rows[i][31].ToString()))
                    try
                    {
                        Insert_Career1_SQL = "INSERT INTO A_CAREER_TEMP VALUES('" + tempDT.Rows[i][8].ToString() + "','" + tempDT.Rows[i][31].ToString() + "','" + tempDT.Rows[i][35].ToString() + "','" + tempDT.Rows[i][36].ToString() + "','" + tempDT.Rows[i][32].ToString() + "','" + tempDT.Rows[i][34].ToString() + "')";
                    
                    }
                    catch
                    {
                        Insert_Career1_SQL = "";
                    }

                string Insert_Career2_SQL = "";

                if (!string.IsNullOrEmpty(tempDT.Rows[i][37].ToString()))
                    try
                    {
                        Insert_Career2_SQL = "INSERT INTO A_CAREER_TEMP VALUES('" + tempDT.Rows[i][8].ToString() + "','" + tempDT.Rows[i][37].ToString() + "','" + tempDT.Rows[i][41].ToString() + "','" + tempDT.Rows[i][42].ToString() + "','" + tempDT.Rows[i][38].ToString() + "','" + tempDT.Rows[i][40].ToString() + "')";
                    }
                    catch
                    {
                        Insert_Career2_SQL = "";
                    }

                string Insert_Career3_SQL = "";

                if (!string.IsNullOrEmpty(tempDT.Rows[i][43].ToString()))
                    try
                    {
                        Insert_Career3_SQL = "INSERT INTO A_CAREER_TEMP VALUES('" + tempDT.Rows[i][8].ToString() + "','" + tempDT.Rows[i][43].ToString() + "','" + tempDT.Rows[i][47].ToString() + "','" + tempDT.Rows[i][48].ToString() + "','" + tempDT.Rows[i][44].ToString() + "','" + tempDT.Rows[i][46].ToString() + "')";
                    }
                    catch
                    {
                        Insert_Career3_SQL = "";
                    }

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(Delete_Exam_SQL);

                    if (!string.IsNullOrEmpty(Insert_Exam_SQL))
                        o_DBFactory.ABC_toTest.Edit_Data(Insert_Exam_SQL);

                    o_DBFactory.ABC_toTest.Edit_Data(Delete_Edu_SQL);

                    if (!string.IsNullOrEmpty(Insert_Edu_SQL))
                        o_DBFactory.ABC_toTest.Edit_Data(Insert_Edu_SQL);

                    o_DBFactory.ABC_toTest.Edit_Data(Delete_Career_SQL);

                    if (!string.IsNullOrEmpty(Insert_Career1_SQL))
                        o_DBFactory.ABC_toTest.Edit_Data(Insert_Career1_SQL);

                    o_DBFactory.ABC_toTest.Edit_Data(Delete_TBDV_SQL);

                    if (!string.IsNullOrEmpty(Insert_TBDV_SQL))
                        o_DBFactory.ABC_toTest.Edit_Data(Insert_TBDV_SQL);

                    if (!string.IsNullOrEmpty(Insert_Career2_SQL))
                        o_DBFactory.ABC_toTest.Edit_Data(Insert_Career2_SQL);

                    if (!string.IsNullOrEmpty(Insert_Career3_SQL))
                        o_DBFactory.ABC_toTest.Edit_Data(Insert_Career3_SQL);

                    if (i == tempDT.Rows.Count - 1)
                    {
                        Response.Write(@"<script language=javascript>window.alert('轉檔完成！');</script>");
                    }
                }
                catch
                {
                }
            }
        }

        public DataTable ConvertCSVtoDataTable(bool firstRowAsHeader)
        {
            DataTable t = new DataTable();
            using (StreamReader sr = new StreamReader(this.FileUpload1.PostedFile.InputStream, System.Text.Encoding.Default))
            {
                string line = null;
                bool colCreated = false;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] p = line.Split(',');
                    if (!colCreated)
                    {
                        int idx = 1;
                        foreach (string s in p)
                        {
                            try
                            {
                                t.Columns.Add(
                                    //第一欄是否有欄位名稱? 無則用C1, C2自動編號
                                    firstRowAsHeader ? s : "C" + idx.ToString(),
                                    typeof(string)
                                    );
                            }
                            catch (Exception e1)
                            {
                                throw new ApplicationException(
                                    "新增欄位失敗! 欄位名稱=" + s + "\n" + e1.ToString());
                            }
                            idx++;
                        }
                        colCreated = true;
                        //首欄若為欄名，則不當資料處理
                        if (firstRowAsHeader) continue;
                    }
                    try
                    {
                        t.Rows.Add(p);
                    }
                    catch (Exception e2)
                    {
                        throw new ApplicationException("資料匯入失敗!\n資料=" + line +
                            "\n錯誤訊息:" + e2.ToString());
                    }
                }
            }
            return t;
        }

        protected string NAME_TO_CODE(string NAME, string KTYPE)
        {
            string MZ_KCODE = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI='" + NAME + "' AND MZ_KTYPE='" + KTYPE + "'");

            return MZ_KCODE;
        }
    }
}
