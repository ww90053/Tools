using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using TPPDDB.App_Code;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public class RPT
    {
        #region 獎懲


        public class punish
        {


            /// <summary>
            /// 2.5.1獎懲建議名冊
            /// </summary>
            public class A_gradesuglist
            {

                /// <summary>
                /// 收尋PRKB資料
                /// </summary>
                /// <param name="EXAD">機關</param>
                /// <param name="EXUNIT">單位</param>
                /// <param name="MZ_SWT3"></param>
                /// <param name="MZ_NO1">案號起 (如沒有迄就直接等於此案號)</param>
                /// <param name="MZ_NO2">案號迄</param>
                /// <param name="orderby">排序:1依機關-職等-職稱-單位,排序:2依流水號</param>
                /// <returns></returns>
                public static DataTable doSearch(string EXAD, string EXUNIT, string MZ_SWT3, string MZ_NO1, string MZ_NO2, string orderby, string MUSER)
                {
                    string strSQL = @"  SELECT MZ_AD,AKD.MZ_KCHI      MZ_AD_NAME ,    AKU.MZ_KCHI   MZ_UNIT   , 
                                  AKO.MZ_KCHI   MZ_OCCC  , MZ_OCCC AS MZ_OCCC1, MZ_RANK, AKR.MZ_KCHI   MZ_SRANK , 
                                  MZ_NO,MZ_NAME,MZ_ID,MZ_PRCT,MZ_PROLNO,AKP.MZ_KCHI    MZ_PRRST , 
                                  MZ_MEMO, dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV ,MZ_UNIT AS MZ_UNIT1 
                                  FROM A_PRKB 
                                  LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRKB .MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                  LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRKB .MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                                  LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRKB .MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                  LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_PRKB .MZ_SRANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                                  LEFT JOIN A_KTYPE AKP ON RTRIM(AKP.MZ_KCODE)=RTRIM(A_PRKB .MZ_PRRST) AND RTRIM(AKP.MZ_KTYPE)='24' 
                                  WHERE 1=1 ";

                    if (MZ_NO1.Trim() != "" && MZ_NO2.Trim() != "")
                    {
                        strSQL += " AND MZ_NO>='" + MZ_NO1.Trim() + "' AND MZ_NO<='" + MZ_NO2.Trim() + "'";
                    }
                    //因為條件有說迄未輸入時以起值 , 因此兩 textbox 拆開條件判斷
                    else if (MZ_NO1.Trim() != "")
                    {
                        strSQL += " AND MZ_NO='" + MZ_NO1.Trim() + "'";
                    }

                    if (EXUNIT != string.Empty)
                    {
                        strSQL += " AND MZ_UNIT='" + EXUNIT.Trim() + "' ";
                    }
                    if (EXAD != string.Empty)
                    {
                        strSQL += " AND MZ_AD='" + EXAD.Trim() + "'";
                    }
                    if (MZ_SWT3 != "")
                    {
                        strSQL += " AND MZ_SWT3='" + MZ_SWT3.Trim() + "'";
                    }

                    if (MUSER != "")
                    {
                        strSQL += " AND MUSER='" + MUSER.Trim() + "'";
                    }


                    if (orderby == "1")
                    {
                        //2013/11/27 如果有分季要靠MZ_PRCT 但數字前的字要一模一樣才有辦法排序                    
                        //strSQL += " ORDER BY MZ_TBDV,MZ_OCCC1,dbo.SUBSTR(MZ_AD,1,9),MZ_UNIT,MZ_ID,MZ_PRCT";
                        //增加主管級別(MZ_PCHIEF)排序 20190103 by sky
                        strSQL += @" ORDER BY dbo.SUBSTR(MZ_AD,1,9), MZ_TBDV,
                                        (SELECT MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = A_PRKB.MZ_ID),
                                        MZ_OCCC1, MZ_UNIT1, MZ_ID, MZ_PRCT ";

                        //2013/12/02
                    }
                    else if (orderby == "2")//依流水號
                    {
                        //增加主管級別(MZ_PCHIEF)排序 20190103 by sky
                        strSQL += @" ORDER BY MZ_NO, dbo.SUBSTR(MZ_AD,1,9), MZ_UNIT, MZ_TBDV, 
                                        (SELECT MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = A_PRKB.MZ_ID),
                                        MZ_OCCC1, MZ_ID";

                    }




                    DataTable gradesuglist = new DataTable();

                    gradesuglist = o_DBFactory.ABC_toTest.Create_Table(strSQL, "suglist");

                    for (int i = 0; i < gradesuglist.Rows.Count; i++)
                    {
                        gradesuglist.Rows[i]["MZ_PRCT"] = o_CommonService.d_report_break_line(gradesuglist.Rows[i]["MZ_PRCT"].ToString(), 26, "&N");
                    }

                    return gradesuglist;

                }

                public static string doSUM(string EXAD, string EXUNIT, string MZ_SWT3, string MZ_NO1, string MZ_NO2, DataTable gradesuglist, string MUSER)
                {
                    List<string> where = new List<string>();
                    if (EXAD != string.Empty)
                    {
                        where.Add("MZ_AD='" + EXAD.Trim() + "'");
                    }

                    if (EXUNIT != string.Empty)
                    {
                        where.Add("MZ_UNIT='" + EXUNIT.Trim() + "'");
                    }

                    if (MZ_SWT3 != "")
                    {
                        where.Add("MZ_SWT3='" + MZ_SWT3.Trim() + "'");
                    }

                    if (MUSER != "")
                    {
                        where.Add("MUSER='" + MUSER.Trim() + "'");
                    }

                    if (MZ_NO1.Trim() != "" && MZ_NO2.Trim() != "")
                    {
                        where.Add("MZ_NO>='" + MZ_NO1.Trim() + "' AND MZ_NO<='" + MZ_NO2.Trim() + "'");
                    }
                    //因為條件有說迄未輸入時以起值 , 因此兩 textbox 拆開條件判斷..091207.By Susan 
                    else if (MZ_NO1.Trim() != "")
                    {
                        where.Add("MZ_NO='" + MZ_NO1.Trim() + "'");
                    }

                    string wheres = (where.Count > 0 ? " AND " + string.Join(" AND ", where.ToArray()) : string.Empty);



                    List<string> prrst = new List<string>();
                    prrst.Add("4200");
                    prrst.Add("4100");
                    prrst.Add("4020");
                    prrst.Add("4010");
                    prrst.Add("4002");
                    prrst.Add("4001");
                    prrst.Add("5100");
                    prrst.Add("5020");
                    prrst.Add("5010");
                    prrst.Add("5002");
                    prrst.Add("5001");

                    DataTable gradeSum = new DataTable();
                    gradeSum.Columns.Add("R1", typeof(int));
                    gradeSum.Columns.Add("R2", typeof(int));
                    gradeSum.Columns.Add("R3", typeof(int));
                    gradeSum.Columns.Add("R4", typeof(int));
                    gradeSum.Columns.Add("R5", typeof(int));
                    gradeSum.Columns.Add("R6", typeof(int));
                    gradeSum.Columns.Add("R7", typeof(int));
                    gradeSum.Columns.Add("R8", typeof(int));
                    gradeSum.Columns.Add("R9", typeof(int));
                    gradeSum.Columns.Add("R10", typeof(int));
                    gradeSum.Columns.Add("R11", typeof(int));

                    DataRow newdr = gradeSum.NewRow();
                    for (int i = 0; i < prrst.Count; i++)
                    {
                        string strSQL = string.Format("SELECT COUNT(*) FROM A_PRKB WHERE MZ_PRRST='{0}' {1} ", prrst[i], wheres);
                        DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                        newdr["R" + (i + 1)] = int.Parse(temp.Rows[0][0].ToString());
                    }
                    gradeSum.Rows.Add(newdr);

                    string sum = string.Empty;
                    if (gradeSum.Rows.Count > 0)
                    {
                        sum += "一次記二大功： " + gradeSum.Rows[0]["R1"].ToString() + " 人次 ";
                        sum += "記一大功： " + gradeSum.Rows[0]["R2"].ToString() + " 人次 ";
                        sum += "記功二次： " + gradeSum.Rows[0]["R3"].ToString() + " 人次 ";
                        sum += "記功一次： " + gradeSum.Rows[0]["R4"].ToString() + " 人次&N";
                        sum += "嘉獎二次： " + gradeSum.Rows[0]["R5"].ToString() + " 人次 ";
                        sum += "嘉獎一次： " + gradeSum.Rows[0]["R6"].ToString() + " 人次 ";
                        sum += "記一大過： " + gradeSum.Rows[0]["R7"].ToString() + " 人次 ";
                        sum += "記過二次： " + gradeSum.Rows[0]["R8"].ToString() + " 人次&N";
                        sum += "記過一次： " + gradeSum.Rows[0]["R9"].ToString() + " 人次 ";
                        sum += "申誡二次： " + gradeSum.Rows[0]["R10"].ToString() + " 人次 ";
                        sum += "申誡一次： " + gradeSum.Rows[0]["R11"].ToString() + " 人次 ";
                    }

                    return sum;
                }

            }

            /// <summary>
            /// 2.5..2獎懲案件請示單
            /// </summary>
            public class A_caserequest
            {

                public static DataTable doSearch(string MZ_NO1, string MZ_NO2)
                {
                    string strSQL = @"SELECT MZ_NO,MZ_DATE,MZ_PRID,MZ_PRID1,MZ_MASTER_NAME,MZ_CAUSE,MZ_DESC,MZ_MAX,MZ_PRE,
                                OAD.MZ_KCHI MZ_OAD, US.MZ_KCHI  MZ_USER
                                FROM  A_PRK3 
                                LEFT JOIN   A_KTYPE OAD ON OAD.MZ_KTYPE='04' AND OAD.MZ_KCODE LIKE '382138%' AND OAD.MZ_KCODE=MZ_OAD
                                LEFT JOIN   A_KTYPE  US ON  US.MZ_KTYPE='04' AND US.MZ_KCODE=A_PRK3 .MZ_USER  ";

                    if (MZ_NO1 != "" && MZ_NO2 != "")
                    {
                        strSQL += "  WHERE MZ_NO>='" + MZ_NO1 + "' AND MZ_NO<='" + MZ_NO2 + "'";
                    }
                    else if (MZ_NO1 != "")
                    {
                        strSQL += " WHERE MZ_NO='" + MZ_NO1 + "'";
                    }

                    DataTable source = new DataTable();

                    source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "suglist");


                    return source;


                }

            }

            /// <summary>
            /// 2.5..3獎懲令
            /// </summary>
            public class A_punish
            {
                public static DataTable doSearch(string PRID, string PRID1, string MZ_SRANK)
                {
                    string strSQL = @"Select    AKD.MZ_KCHI MZ_AD_NAME ,  A_PRK2.MZ_NAME,A_PRK2.MZ_ID,
                            AKO.MZ_KCHI MZ_OCCC_NAME ,  A_PRK2.MZ_OCCC, 
                            AKS.MZ_KCHI MZ_SRANK_NAME,  A_PRK2.MZ_SRANK, 
                            AKP.MZ_KCHI MZ_PRRST_NAME,  A_PRK2.MZ_PRRST, 
                            A_PRK2.MZ_PRID,A_PRK2.MZ_PRID1,(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(A_PRK2.MZ_DATE,0,3)))+'年'+dbo.SUBSTR(A_PRK2.MZ_DATE,4,2)+'月'+dbo.SUBSTR(A_PRK2.MZ_DATE,6,2)+'日') as MZ_DATE,A_PRK2.MZ_PRCT,A_PRK2.MZ_MEMO,A_PRK2.MZ_REMARK, 
                            AP. MZ_PRONAME   MZ_PROLNO,  
                            ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO      ,
                            ACK. MZ_FILENO  MZ_FILENO , ACK. MZ_YEARUSE  MZ_YEARUSE   ,
                            dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                            AKU.MZ_KCHI   MZ_UNIT_NAME,MZ_AD,MZ_PRK,MZ_UNIT,MZ_EXAD,MZ_EXUNIT 
                             From A_PRK2 
                            LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRK2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                            LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRK2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                            LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_PRK2.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' 
                            LEFT JOIN A_KTYPE AKP ON RTRIM(AKP.MZ_KCODE)=RTRIM(A_PRK2.MZ_PRRST) AND RTRIM(AKP.MZ_KTYPE)='24' 
                            LEFT JOIN A_PROLNO AP ON RTRIM(AP.MZ_PROLNO)=RTRIM(A_PRK2.MZ_PROLNO)  
                            LEFT JOIN A_PRK1 ACK ON  ACK.MZ_PRID=A_PRK2.MZ_PRID AND ACK.MZ_PRID1=A_PRK2.MZ_PRID1
                            LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRK2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                            Where 1=1  ";


                    if (PRID != "")
                    {
                        //20141231
                        if (PRID == "新北警人")
                        {
                            strSQL += " AND (A_PRK2 .MZ_PRID='" + PRID + "' OR  A_PRK2 .MZ_PRID='北警人')";
                        }
                        else
                        {
                            strSQL += " AND A_PRK2 .MZ_PRID='" + PRID + "'";
                        }

                        // strSQL += " A_PRK2 .MZ_PRID='" + PRID + "' ";
                    }


                    if (MZ_SRANK != string.Empty)
                    {
                        if (MZ_SRANK == "P8G22")//警正二階以下
                        {
                            strSQL += " and A_PRK2.MZ_SRANK NOT IN ('G22','G21','G31','G32','G33','G34','P08','P09','P10','P11','P12','P13','P14')";

                        }
                        else if (MZ_SRANK == "P9G23")//警正二階以上
                        {
                            strSQL += " and A_PRK2.MZ_SRANK IN ('G22','G21','P08','P09','P10','P11','P12','P13','P14')";

                        }
                        else if (MZ_SRANK == "G3")
                        {
                            strSQL += " and A_PRK2.MZ_SRANK IN ('G31','G32','G33','G34')";

                        }
                    }

                    if (PRID1 != "")
                    {

                        strSQL += " AND A_PRK2.MZ_PRID1='" + o_str.tosql(PRID1) + "'";
                    }

                    //新增剔除條件108/12/01啟用 20191114 by sky
                    if (DateTime.Now >= new DateTime(2019, 12, 1))
                    {
                        strSQL += " And A_PRK2.MZ_PRRST NOT IN ('4001','4002','4010','4020')";
                    }

                    strSQL += " ORDER BY TBDV,MZ_OCCC,MZ_AD,MZ_UNIT,MZ_ID";

                    DataTable punish = new DataTable();

                    punish = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");

                    return punish;

                }


                public static DataTable doTable(string PRID, string PRID1, string CHKAD, DataTable punish)
                {


                    DataTable rpt = new DataTable();
                    rpt.Columns.Add("TOJ", typeof(string));
                    rpt.Columns.Add("MZ_NAME", typeof(string));
                    rpt.Columns.Add("MZ_ID", typeof(string));
                    rpt.Columns.Add("MZ_PRRST", typeof(string));
                    rpt.Columns.Add("MZ_PRID", typeof(string));
                    rpt.Columns.Add("MZ_PRID1", typeof(string));
                    rpt.Columns.Add("MZ_DATE", typeof(string));
                    rpt.Columns.Add("MZ_PRCT", typeof(string));
                    rpt.Columns.Add("MZ_PROLNO", typeof(string));
                    rpt.Columns.Add("MZ_MEMO", typeof(string));
                    rpt.Columns.Add("MZ_REMARK", typeof(string));
                    rpt.Columns.Add("SPEED_NO", typeof(string));
                    rpt.Columns.Add("PWD_NO", typeof(string));
                    rpt.Columns.Add("MZ_FILENO", typeof(string));
                    rpt.Columns.Add("MZ_YEARUSE", typeof(string));
                    rpt.Columns.Add("MZ_PRRST_NAME", typeof(string));
                    rpt.Columns.Add("PRID3", typeof(string));
                    rpt.Columns.Add("PRINTGROUP", typeof(string));
                    rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
                    rpt.Columns.Add("NOW", typeof(string));
                    rpt.Columns.Add("PAGECOUNT", typeof(string));

                    for (int i = 0; i < punish.Rows.Count; i += 2)
                    {

                        if (i - punish.Rows.Count == -1)
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                            newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(punish, i);
                            newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "１";
                            newdr["NOW"] = NOW(punish, i);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);
                        }
                        else
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                            newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(punish, i);
                            newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(punish, i);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);

                            newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_NAME"] = punish.Rows[i + 1]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i + 1]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i + 1]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i + 1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i + 1]["MZ_PRID1"];
                            newdr["MZ_DATE"] = punish.Rows[i + 1]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(punish, i + 1);
                            newdr["MZ_PROLNO"] = punish.Rows[i + 1]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i + 1]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i + 1]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i + 1]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i + 1]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i + 1]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i + 1]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i + 1]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(punish, i + 1);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);

                            if (punish.Rows[i + 1]["MZ_NAME"].ToString() != punish.Rows[i]["MZ_NAME"].ToString())
                            {
                                DataRow newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = punish.Rows[i + 1]["MZ_NAME"];
                                newdr1["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                                newdr1["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                                newdr1["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                                newdr1["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                                newdr1["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                                newdr1["MZ_PRCT"] = PRCT(punish, i);
                                newdr1["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                                newdr1["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                                newdr1["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                                newdr1["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                                newdr1["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                                newdr1["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                                newdr1["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                                newdr1["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "1";
                                newdr1["COUNTPAGEMAN"] = "２";
                                newdr1["NOW"] = NOW(punish, i);
                                newdr1["PAGECOUNT"] = "";

                                rpt.Rows.Add(newdr1);

                                newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = string.Empty;
                                newdr1["MZ_NAME"] = punish.Rows[i + 1]["MZ_NAME"];
                                newdr1["MZ_ID"] = punish.Rows[i + 1]["MZ_ID"];
                                newdr1["MZ_PRRST"] = punish.Rows[i + 1]["MZ_PRRST"];
                                newdr1["MZ_PRID"] = punish.Rows[i + 1]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = punish.Rows[i + 1]["MZ_PRID1"];
                                newdr1["MZ_DATE"] = punish.Rows[i + 1]["MZ_DATE"];
                                newdr1["MZ_PRCT"] = PRCT(punish, i + 1);
                                newdr1["MZ_PROLNO"] = punish.Rows[i + 1]["MZ_PROLNO"];
                                newdr1["MZ_MEMO"] = punish.Rows[i + 1]["MZ_MEMO"];
                                newdr1["MZ_REMARK"] = punish.Rows[i + 1]["MZ_REMARK"];
                                newdr1["SPEED_NO"] = punish.Rows[i + 1]["SPEED_NO"];
                                newdr1["PWD_NO"] = punish.Rows[i + 1]["PWD_NO"];
                                newdr1["MZ_FILENO"] = punish.Rows[i + 1]["MZ_FILENO"];
                                newdr1["MZ_YEARUSE"] = punish.Rows[i + 1]["MZ_YEARUSE"];
                                newdr1["MZ_PRRST_NAME"] = punish.Rows[i + 1]["MZ_PRRST_NAME"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "1";
                                newdr1["COUNTPAGEMAN"] = "２";
                                newdr1["NOW"] = NOW(punish, i + 1);
                                newdr1["PAGECOUNT"] = "";

                                rpt.Rows.Add(newdr1);
                            }
                        }
                    }


                    for (int i = 0; i < punish.Rows.Count; i += 2)
                    {
                        if (i - punish.Rows.Count == -1)
                        {
                            string TOJ = "";
                            if (CHKAD == "382130200C" || CHKAD == "382130300C")
                            {
                                if (punish.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || punish.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                                {
                                    TOJ = o_A_KTYPE.RAD(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM A_UNIT_AD WHERE MZ_UNIT='" + punish.Rows[i]["MZ_UNIT"].ToString() + "' AND MZ_AD LIKE '38213%' AND MZ_AD!='382130200C' AND MZ_AD!='382130300C' "));
                                }
                                else
                                {
                                    TOJ = o_A_KTYPE.RUNIT(punish.Rows[i]["MZ_UNIT"].ToString());
                                }
                            }
                            else if (CHKAD == punish.Rows[i]["MZ_AD"].ToString())
                            {
                                TOJ = o_A_KTYPE.RUNIT(punish.Rows[i]["MZ_UNIT"].ToString());
                            }
                            else
                            {
                                TOJ = o_A_KTYPE.RAD(punish.Rows[i]["MZ_AD"].ToString());
                            }

                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = TOJ;
                            newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                            newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(punish, i);
                            newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "2";
                            newdr["COUNTPAGEMAN"] = "１";
                            newdr["NOW"] = NOW(punish, i);
                            newdr["PAGECOUNT"] = "";

                            if (TOJ == "隊本部")
                            {

                            }
                            else
                            {
                                rpt.Rows.Add(newdr);
                            }
                        }
                        else
                        {
                            string TOJ = "";

                            if (CHKAD == "382130200C" || CHKAD == "382130300C")
                            {
                                if (punish.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || punish.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                                {
                                    TOJ = o_A_KTYPE.RAD(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM A_UNIT_AD WHERE MZ_UNIT='" + punish.Rows[i]["MZ_UNIT"].ToString() + "' AND MZ_AD LIKE '38213%' AND MZ_AD!='382130200C' AND MZ_AD!='382130300C' "));
                                }
                                else
                                {
                                    TOJ = o_A_KTYPE.RUNIT(punish.Rows[i]["MZ_UNIT"].ToString());
                                }
                            }
                            else if (CHKAD == punish.Rows[i]["MZ_AD"].ToString())
                            {
                                TOJ = o_A_KTYPE.RUNIT(punish.Rows[i]["MZ_UNIT"].ToString());
                            }
                            else
                            {
                                TOJ = o_A_KTYPE.RAD(punish.Rows[i]["MZ_AD"].ToString());
                            }

                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = TOJ;
                            newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                            newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(punish, i);
                            newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "2";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(punish, i);
                            newdr["PAGECOUNT"] = "";

                            if (TOJ == "隊本部")
                            {

                            }
                            else
                            {
                                rpt.Rows.Add(newdr);
                            }

                            newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_NAME"] = punish.Rows[i + 1]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i + 1]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i + 1]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i + 1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i + 1]["MZ_PRID1"];
                            newdr["MZ_DATE"] = punish.Rows[i + 1]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(punish, i + 1);
                            newdr["MZ_PROLNO"] = punish.Rows[i + 1]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i + 1]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i + 1]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i + 1]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i + 1]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i + 1]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i + 1]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i + 1]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "2";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(punish, i + 1);
                            newdr["PAGECOUNT"] = "";

                            if (TOJ == "隊本部")
                            {

                            }
                            else
                            {
                                rpt.Rows.Add(newdr);
                            }

                            if (CHKAD == punish.Rows[i + 1]["MZ_AD"].ToString() && punish.Rows[i + 1]["MZ_UNIT"].ToString() != punish.Rows[i]["MZ_UNIT"].ToString())
                            {
                                if (CHKAD == "382130200C" || CHKAD == "382130300C")
                                {
                                    if (punish.Rows[i + 1]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || punish.Rows[i + 1]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                                    {
                                        TOJ = o_A_KTYPE.RAD(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM A_UNIT_AD WHERE MZ_UNIT='" + punish.Rows[i + 1]["MZ_UNIT"].ToString() + "' AND MZ_AD LIKE '38213%' AND MZ_AD!='382130200C' AND MZ_AD!='382130300C' "));
                                    }
                                    else
                                    {
                                        TOJ = o_A_KTYPE.RUNIT(punish.Rows[i + 1]["MZ_UNIT"].ToString());
                                    }
                                }
                                else if (CHKAD == punish.Rows[i + 1]["MZ_AD"].ToString())
                                {
                                    TOJ = o_A_KTYPE.RUNIT(punish.Rows[i + 1]["MZ_UNIT"].ToString());
                                }
                                else
                                {
                                    TOJ = o_A_KTYPE.RAD(punish.Rows[i + 1]["MZ_AD"].ToString());
                                }

                                DataRow newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = TOJ;
                                newdr1["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                                newdr1["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                                newdr1["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                                newdr1["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                                newdr1["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                                newdr1["MZ_PRCT"] = PRCT(punish, i);
                                newdr1["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                                newdr1["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                                newdr1["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                                newdr1["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                                newdr1["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                                newdr1["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                                newdr1["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                                newdr1["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "2";
                                newdr1["COUNTPAGEMAN"] = "２";
                                newdr1["NOW"] = NOW(punish, i);
                                newdr1["PAGECOUNT"] = "";

                                if (TOJ == "隊本部")
                                {

                                }
                                else
                                {
                                    rpt.Rows.Add(newdr1);
                                }

                                newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = string.Empty;
                                newdr1["MZ_NAME"] = punish.Rows[i + 1]["MZ_NAME"];
                                newdr1["MZ_ID"] = punish.Rows[i + 1]["MZ_ID"];
                                newdr1["MZ_PRRST"] = punish.Rows[i + 1]["MZ_PRRST"];
                                newdr1["MZ_PRID"] = punish.Rows[i + 1]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = punish.Rows[i + 1]["MZ_PRID1"];
                                newdr1["MZ_DATE"] = punish.Rows[i + 1]["MZ_DATE"];
                                newdr1["MZ_PRCT"] = PRCT(punish, i + 1);
                                newdr1["MZ_PROLNO"] = punish.Rows[i + 1]["MZ_PROLNO"];
                                newdr1["MZ_MEMO"] = punish.Rows[i + 1]["MZ_MEMO"];
                                newdr1["MZ_REMARK"] = punish.Rows[i + 1]["MZ_REMARK"];
                                newdr1["SPEED_NO"] = punish.Rows[i + 1]["SPEED_NO"];
                                newdr1["PWD_NO"] = punish.Rows[i + 1]["PWD_NO"];
                                newdr1["MZ_FILENO"] = punish.Rows[i + 1]["MZ_FILENO"];
                                newdr1["MZ_YEARUSE"] = punish.Rows[i + 1]["MZ_YEARUSE"];
                                newdr1["MZ_PRRST_NAME"] = punish.Rows[i + 1]["MZ_PRRST_NAME"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "2";
                                newdr1["COUNTPAGEMAN"] = "２";
                                newdr1["NOW"] = NOW(punish, i + 1);
                                newdr1["PAGECOUNT"] = "";

                                if (TOJ == "隊本部")
                                {

                                }
                                else
                                {
                                    rpt.Rows.Add(newdr1);
                                }
                            }
                            else if (CHKAD != punish.Rows[i + 1]["MZ_AD"].ToString())
                            {
                                if (CHKAD == "382130200C" || CHKAD == "382130300C")
                                {
                                    if (punish.Rows[i + 1]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || punish.Rows[i + 1]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                                    {
                                        TOJ = o_A_KTYPE.RAD(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM A_UNIT_AD WHERE MZ_UNIT='" + punish.Rows[i + 1]["MZ_UNIT"].ToString() + "' AND MZ_AD LIKE '38213%' AND MZ_AD!='382130200C' AND MZ_AD!='382130300C' "));
                                    }
                                }
                                else
                                    TOJ = o_A_KTYPE.RAD(punish.Rows[i + 1]["MZ_AD"].ToString());

                                DataRow newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = TOJ;
                                newdr1["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                                newdr1["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                                newdr1["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                                newdr1["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                                newdr1["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                                newdr1["MZ_PRCT"] = PRCT(punish, i);
                                newdr1["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                                newdr1["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                                newdr1["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                                newdr1["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                                newdr1["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                                newdr1["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                                newdr1["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                                newdr1["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "2";
                                newdr1["COUNTPAGEMAN"] = "２";
                                newdr1["NOW"] = NOW(punish, i);
                                newdr1["PAGECOUNT"] = "";

                                rpt.Rows.Add(newdr1);

                                newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = string.Empty;
                                newdr1["MZ_NAME"] = punish.Rows[i + 1]["MZ_NAME"];
                                newdr1["MZ_ID"] = punish.Rows[i + 1]["MZ_ID"];
                                newdr1["MZ_PRRST"] = punish.Rows[i + 1]["MZ_PRRST"];
                                newdr1["MZ_PRID"] = punish.Rows[i + 1]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = punish.Rows[i + 1]["MZ_PRID1"];
                                newdr1["MZ_DATE"] = punish.Rows[i + 1]["MZ_DATE"];
                                newdr1["MZ_PRCT"] = PRCT(punish, i + 1);
                                newdr1["MZ_PROLNO"] = punish.Rows[i + 1]["MZ_PROLNO"];
                                newdr1["MZ_MEMO"] = punish.Rows[i + 1]["MZ_MEMO"];
                                newdr1["MZ_REMARK"] = punish.Rows[i + 1]["MZ_REMARK"];
                                newdr1["SPEED_NO"] = punish.Rows[i + 1]["SPEED_NO"];
                                newdr1["PWD_NO"] = punish.Rows[i + 1]["PWD_NO"];
                                newdr1["MZ_FILENO"] = punish.Rows[i + 1]["MZ_FILENO"];
                                newdr1["MZ_YEARUSE"] = punish.Rows[i + 1]["MZ_YEARUSE"];
                                newdr1["MZ_PRRST_NAME"] = punish.Rows[i + 1]["MZ_PRRST_NAME"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "2";
                                newdr1["COUNTPAGEMAN"] = "２";
                                newdr1["NOW"] = NOW(punish, i + 1);
                                newdr1["PAGECOUNT"] = "";

                                rpt.Rows.Add(newdr1);

                            }

                        }
                    }


                    punish.DefaultView.RowFilter = "MZ_SRANK IN ('P10','P11','P12','P13','P14','G31','G32','G33','G34') AND SUBSTRING(MZ_PRK,1,1)<>'A'";

                    DataTable temp1 = punish.DefaultView.ToTable();


                    #region 受文者監察院內文

                    //20150327
                    int group_number = 4; //從第四筆開始累加計算 避免與下面其他受文者列印群組重複

                    for (int i = 0; i < temp1.Rows.Count; i++)
                    {



                        if (i % 2 == 0)
                        {
                            //20150327
                            group_number = group_number + 1 + i;

                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = "監察院";
                            newdr["MZ_NAME"] = temp1.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = temp1.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = temp1.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = temp1.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = temp1.Rows[i]["MZ_PRID1"];
                            newdr["MZ_DATE"] = temp1.Rows[i]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(temp1, i);
                            newdr["MZ_PROLNO"] = temp1.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = temp1.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = temp1.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = temp1.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = temp1.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = temp1.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = temp1.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = temp1.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            //20150327
                            //newdr["PRINTGROUP"] = (4 + i).ToString();
                            newdr["PRINTGROUP"] = group_number.ToString();
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(temp1, i);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);
                        }
                        else
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_NAME"] = temp1.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = temp1.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = temp1.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = temp1.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = temp1.Rows[i]["MZ_PRID1"];
                            newdr["MZ_DATE"] = temp1.Rows[i]["MZ_DATE"];
                            newdr["MZ_PRCT"] = PRCT(temp1, i);
                            newdr["MZ_PROLNO"] = temp1.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = temp1.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = temp1.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = temp1.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = temp1.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = temp1.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = temp1.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = temp1.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            //20150327
                            //newdr["PRINTGROUP"] = (4 + i).ToString();
                            newdr["PRINTGROUP"] = group_number.ToString();
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(temp1, i);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);
                        }
                    }

                    if (temp1.Rows.Count > 0)
                    {
                        if (temp1.Rows.Count % 2 == 1)
                        {
                            rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
                        }
                    }

                    #endregion 受文者監察院內文



                    #region 其他受文者內文

                    //20150327
                    //string[] explain = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID='" + PRID + "' AND MZ_PRID1='" + PRID1 + "'").Split(new char[] { '、' });
                    string[] explain;

                    if (PRID == "北警人" || PRID == "新北警人")
                    {
                        explain = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID IN ('北警人','新北警人') AND MZ_PRID1='" + PRID1 + "'").Split(new char[] { '、' });

                    }
                    else
                    {
                        explain = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID='" + PRID + "' AND MZ_PRID1='" + PRID1 + "'").Split(new char[] { '、' });
                    }
                    if (explain.Length == 1 && string.IsNullOrEmpty(explain[0]))
                    {
                    }
                    else
                    {
                        for (int j = 0; j < explain.Length; j++)
                        {
                            //20150327
                            group_number = group_number + +1 + j;

                            for (int i = 0; i < punish.Rows.Count; i++)
                            {

                                if (i % 2 == 0)
                                {
                                    DataRow newdr = rpt.NewRow();
                                    newdr["TOJ"] = explain[j];
                                    newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                                    newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                                    newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                                    newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                                    newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                                    newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                                    newdr["MZ_PRCT"] = PRCT(punish, i);
                                    newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                                    newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                                    newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                                    newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                                    newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                                    newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                                    newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                                    newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                                    newdr["PRID3"] = (i / 2).ToString();
                                    //newdr["PRINTGROUP"] = (4 + j).ToString();
                                    newdr["PRINTGROUP"] = group_number.ToString();

                                    newdr["COUNTPAGEMAN"] = "２";

                                    newdr["NOW"] = NOW(punish, i);
                                    newdr["PAGECOUNT"] = "";

                                    rpt.Rows.Add(newdr);
                                }
                                else
                                {
                                    DataRow newdr = rpt.NewRow();
                                    newdr["TOJ"] = string.Empty;
                                    newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                                    newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                                    newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                                    newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                                    newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                                    newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                                    newdr["MZ_PRCT"] = PRCT(punish, i);
                                    newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                                    newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                                    newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                                    newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                                    newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                                    newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                                    newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                                    newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                                    newdr["PRID3"] = (i / 2).ToString();
                                    newdr["PRINTGROUP"] = group_number.ToString();
                                    //newdr["PRINTGROUP"] = (4 + j).ToString();
                                    newdr["COUNTPAGEMAN"] = "２";
                                    newdr["NOW"] = NOW(punish, i);
                                    newdr["PAGECOUNT"] = "";

                                    rpt.Rows.Add(newdr);
                                }
                            }

                            //20150327 不是只有分局人事室設定為１.之前可能還有其他受文者，固定設定為全型２的話會造成明明只有一員卻顯示兩員的情況
                            if (punish.Rows.Count % 2 == 1)
                            {
                                rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";


                            }



                        }
                        ////20150327 不是只有分局人事室設定為１.之前可能還有其他受文者，固定設定為全型２的話會造成明明只有一員卻顯示兩員的情況
                        //if (punish.Rows.Count % 2 == 1 )
                        //{
                        //    rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";


                        //}
                    }

                    #endregion 其他受文者內文

                    int y = 0;
                    for (int i = 0; i < rpt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(rpt.Rows[i]["TOJ"].ToString()))
                        {
                            y++;
                            rpt.Rows[i]["PAGECOUNT"] = y.ToString().PadLeft(4, '0');
                        }
                        else if (string.IsNullOrEmpty(rpt.Rows[i]["TOJ"].ToString()))
                        {
                            rpt.Rows[i]["PAGECOUNT"] = y.ToString().PadLeft(4, '0');
                        }
                    }

                    return rpt;

                }

                public static string PRCT(DataTable dt, int row)
                {
                    string result = dt.Rows[row]["MZ_PRCT"].ToString() + "(" + dt.Rows[row]["MZ_PRK"].ToString() + ")。";

                    result = o_CommonService.d_report_break_line(result, 50, "&N");

                    return result;
                }

                public static string NOW(DataTable dt, int row)
                {
                    string result = "";

                    if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_AD"].ToString()))
                    {
                        result += dt.Rows[row]["MZ_AD_NAME"].ToString() + dt.Rows[row]["MZ_UNIT_NAME"].ToString() + "(" + dt.Rows[row]["MZ_AD"].ToString() + ")";
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_OCCC"].ToString()))
                    {
                        result += "，" + dt.Rows[row]["MZ_OCCC_NAME"].ToString() + "(" + dt.Rows[row]["MZ_OCCC"].ToString() + ")";
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_SRANK"].ToString()))
                    {
                        result += "，" + dt.Rows[row]["MZ_SRANK_NAME"].ToString() + "(" + dt.Rows[row]["MZ_SRANK"].ToString() + ")";
                    }

                    result += "。";

                    result = o_CommonService.d_report_break_line(result, 52, "&N");

                    return result;
                }

            }

            /// <summary>
            /// 2.5..3獎懲令 (稿)  有拿A_punish的FUNCTION來用
            /// </summary>
            public class A_punish1
            {
                public static DataTable doSearch(string PRID, string PRID1, string MZ_SRANK)
                {
                    string strSQL = @"Select AKD.MZ_KCHI MZ_AD_NAME ,  A_PRK2.MZ_NAME,A_PRK2.MZ_ID,
                                    AKO.MZ_KCHI MZ_OCCC_NAME ,  A_PRK2.MZ_OCCC, 
                                    AKS.MZ_KCHI MZ_SRANK_NAME,  A_PRK2.MZ_SRANK, 
                                    AKP.MZ_KCHI MZ_PRRST_NAME,  A_PRK2.MZ_PRRST,                                      
                                    A_PRK2.MZ_PRID,A_PRK2.MZ_PRID1,(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(A_PRK2.MZ_DATE,0,3)))+'年'+dbo.SUBSTR(A_PRK2.MZ_DATE,4,2)+'月'+dbo.SUBSTR(A_PRK2.MZ_DATE,6,2)+'日') as MZ_DATE,MZ_PRCT,MZ_MEMO,MZ_REMARK, 
                                    AP. MZ_PRONAME   MZ_PROLNO,  
                                    ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO       ,  ACK. MZ_FILENO  MZ_FILENO , ACK. MZ_YEARUSE  MZ_YEARUSE,
                                    dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                                    AKU.MZ_KCHI   MZ_UNIT_NAME,MZ_AD,MZ_PRK 
                            From A_PRK2 
                                    LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRK2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                    LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRK2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                    LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_PRK2.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' 
                                    LEFT JOIN A_KTYPE AKP ON RTRIM(AKP.MZ_KCODE)=RTRIM(A_PRK2.MZ_PRRST) AND RTRIM(AKP.MZ_KTYPE)='24' 
                                    LEFT JOIN A_PROLNO AP ON RTRIM(AP.MZ_PROLNO)=RTRIM(A_PRK2.MZ_PROLNO)  
                                    LEFT JOIN A_PRK1 ACK ON  ACK.MZ_PRID=A_PRK2.MZ_PRID AND ACK.MZ_PRID1=A_PRK2.MZ_PRID1
                                    LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRK2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 

 Where 1=1 ";


                    if (PRID != "")
                    {
                        //20141231
                        if (PRID == "新北警人")
                        {
                            strSQL += " AND (A_PRK2 .MZ_PRID='" + PRID + "' OR  A_PRK2 .MZ_PRID='北警人')";
                        }
                        else
                        {
                            strSQL += " AND A_PRK2 .MZ_PRID='" + PRID + "'";
                        }

                        // strSQL += " A_PRK2 .MZ_PRID='" + PRID + "' ";
                    }
                    // Where A_PRK2.MZ_PRID='" + PRID + "' ";

                    if (MZ_SRANK != string.Empty)
                    {
                        if (MZ_SRANK == "P8G22")//警正二階以下
                        {
                            strSQL += " and A_PRK2.MZ_SRANK NOT IN ('G22','G21','G31','G32','G33','G34','P08','P09','P10','P11','P12','P13','P14')";

                        }
                        else if (MZ_SRANK == "P9G23")//警正二階(含)以上
                        {
                            strSQL += " and A_PRK2.MZ_SRANK IN ('G22','G21','P08','P09','P10','P11','P12','P13','P14')";

                        }
                        else if (MZ_SRANK == "G3")
                        {
                            strSQL += " and A_PRK2.MZ_SRANK IN ('G31','G32','G33','G34')";

                        }
                    }
                    if (PRID1 != "")
                    {
                        strSQL += " AND A_PRK2.MZ_PRID1='" + o_str.tosql(PRID1) + "'";
                    }

                    strSQL += " ORDER BY TBDV,MZ_OCCC,MZ_AD,MZ_UNIT,MZ_ID";


                    DataTable punish1 = new DataTable();

                    punish1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");

                    return punish1;
                }


                public static DataTable doTable(string PRID, string PRID1, string CHKAD, DataTable punish)
                {
                    DataTable rpt = new DataTable();
                    rpt.Columns.Add("TOJ", typeof(string));
                    rpt.Columns.Add("MZ_NAME", typeof(string));
                    rpt.Columns.Add("MZ_ID", typeof(string));
                    rpt.Columns.Add("MZ_PRRST", typeof(string));
                    rpt.Columns.Add("MZ_PRID", typeof(string));
                    rpt.Columns.Add("MZ_PRID1", typeof(string));
                    rpt.Columns.Add("MZ_DATE", typeof(string));
                    rpt.Columns.Add("MZ_PRCT", typeof(string));
                    rpt.Columns.Add("MZ_PROLNO", typeof(string));
                    rpt.Columns.Add("MZ_MEMO", typeof(string));
                    rpt.Columns.Add("MZ_REMARK", typeof(string));
                    rpt.Columns.Add("SPEED_NO", typeof(string));
                    rpt.Columns.Add("PWD_NO", typeof(string));
                    rpt.Columns.Add("MZ_FILENO", typeof(string));
                    rpt.Columns.Add("MZ_YEARUSE", typeof(string));
                    rpt.Columns.Add("MZ_PRRST_NAME", typeof(string));
                    rpt.Columns.Add("PRID3", typeof(string));
                    rpt.Columns.Add("PRINTGROUP", typeof(string));
                    rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
                    rpt.Columns.Add("NOW", typeof(string));
                    rpt.Columns.Add("PAGECOUNT", typeof(string));

                    for (int i = 0; i < punish.Rows.Count; i += 2)
                    {

                        if (i - punish.Rows.Count == -1)
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                            //令(稿) 因未簽准故"日"需要手寫。 20190829 by sky
                            //DateTime dt = DateTime.Parse(punish.Rows[i]["MZ_DATE"].ToString());
                            DateTime dt = ForDateTime.TW_YYYMMDD_To_DateTime(punish.Rows[i]["MZ_DATE"].ToString()) ?? DateTime.Now;
                            newdr["MZ_DATE"] = string.Format("{0}年{1}月　日", (dt.Year-1911).FillStrLeft("0", 3), dt.Month.FillStrLeft("0", 2));
                            newdr["MZ_PRCT"] = RPT.punish.A_punish.PRCT(punish, i);
                            newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "１";
                            newdr["NOW"] = RPT.punish.A_punish.NOW(punish, i);
                            newdr["PAGECOUNT"] = (i / 2).ToString().PadLeft(4, '0');

                            rpt.Rows.Add(newdr);
                        }
                        else
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                            //令(稿) 因未簽准故"日"需要手寫。 20190829 by sky
                            DateTime dt = ForDateTime.TW_YYYMMDD_To_DateTime(punish.Rows[i]["MZ_DATE"].ToString()) ?? DateTime.Now;
                            newdr["MZ_DATE"] = string.Format("{0}年{1}月　日", (dt.Year-1911).FillStrLeft("0", 3), dt.Month.FillStrLeft("0", 2));
                            newdr["MZ_PRCT"] = RPT.punish.A_punish.PRCT(punish, i);
                            newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = RPT.punish.A_punish.NOW(punish, i);
                            newdr["PAGECOUNT"] = (i / 2).ToString().PadLeft(4, '0');

                            rpt.Rows.Add(newdr);

                            newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_NAME"] = punish.Rows[i + 1]["MZ_NAME"];
                            newdr["MZ_ID"] = punish.Rows[i + 1]["MZ_ID"];
                            newdr["MZ_PRRST"] = punish.Rows[i + 1]["MZ_PRRST"];
                            newdr["MZ_PRID"] = punish.Rows[i + 1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = punish.Rows[i + 1]["MZ_PRID1"];
                            //令(稿) 因未簽准故"日"需要手寫。 20190829 by sky
                            dt = ForDateTime.TW_YYYMMDD_To_DateTime(punish.Rows[i + 1]["MZ_DATE"].ToString()) ?? DateTime.Now;
                            newdr["MZ_DATE"] = string.Format("{0}年{1}月　日", (dt.Year-1911).FillStrLeft("0", 3), dt.Month.FillStrLeft("0", 2));
                            newdr["MZ_PRCT"] = RPT.punish.A_punish.PRCT(punish, i + 1);
                            newdr["MZ_PROLNO"] = punish.Rows[i + 1]["MZ_PROLNO"];
                            newdr["MZ_MEMO"] = punish.Rows[i + 1]["MZ_MEMO"];
                            newdr["MZ_REMARK"] = punish.Rows[i + 1]["MZ_REMARK"];
                            newdr["SPEED_NO"] = punish.Rows[i + 1]["SPEED_NO"];
                            newdr["PWD_NO"] = punish.Rows[i + 1]["PWD_NO"];
                            newdr["MZ_FILENO"] = punish.Rows[i + 1]["MZ_FILENO"];
                            newdr["MZ_YEARUSE"] = punish.Rows[i + 1]["MZ_YEARUSE"];
                            newdr["MZ_PRRST_NAME"] = punish.Rows[i + 1]["MZ_PRRST_NAME"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = RPT.punish.A_punish.NOW(punish, i + 1);
                            newdr["PAGECOUNT"] = (i / 2).ToString().PadLeft(4, '0');

                            rpt.Rows.Add(newdr);
                        }
                    }

                    return rpt;



                }

            }

            /// <summary>
            /// 2.5.4獎懲建議函 有拿A_punish的FUNCTION來用
            /// </summary>
            public class A_punishSug
            {
                public static DataTable doSearch(string PRID, string PRID1, string MZ_SRANK)
                {
                    string strSQL = @"Select AKD.MZ_KCHI MZ_AD_NAME ,  A_PRK2.MZ_NAME,A_PRK2.MZ_ID,
                                    AKO.MZ_KCHI MZ_OCCC_NAME ,  A_PRK2.MZ_OCCC, 
                                   AKS.MZ_KCHI MZ_SRANK_NAME,  A_PRK2.MZ_SRANK, 
                                   AKP.MZ_KCHI MZ_PRRST_NAME,  A_PRK2.MZ_PRRST,            
                                   A_PRK2.MZ_PRID,A_PRK2.MZ_PRID1,(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(A_PRK2.MZ_DATE,0,3)))+'年'+dbo.SUBSTR(A_PRK2.MZ_DATE,4,2)+'月'+dbo.SUBSTR(A_PRK2.MZ_DATE,6,2)+'日') as MZ_DATE,MZ_PRCT,MZ_MEMO,MZ_REMARK, 
                                   AP. MZ_PRONAME   MZ_PROLNO,  
                                   ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO       ,                                    
                                   dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                                   AKU.MZ_KCHI   MZ_UNIT_NAME,MZ_AD,MZ_PRK,ACK.MZ_FILENO,ACK.MZ_YEARUSE 
                                   From A_PRK2 
                                     LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRK2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                  LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRK2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                  LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_PRK2.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' 
                                  LEFT JOIN A_KTYPE AKP ON RTRIM(AKP.MZ_KCODE)=RTRIM(A_PRK2.MZ_PRRST) AND RTRIM(AKP.MZ_KTYPE)='24' 
                                   LEFT JOIN A_PROLNO AP ON RTRIM(AP.MZ_PROLNO)=RTRIM(A_PRK2.MZ_PROLNO)  
                                  LEFT JOIN A_PRK1 ACK ON  ACK.MZ_PRID=A_PRK2.MZ_PRID AND ACK.MZ_PRID1=A_PRK2.MZ_PRID1
                                   LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRK2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                                    Where 1=1  ";


                    if (PRID != "")
                    {
                        //20141231
                        if (PRID == "新北警人")
                        {
                            strSQL += " AND (A_PRK2 .MZ_PRID='" + PRID + "' OR  A_PRK2 .MZ_PRID='北警人')";
                        }
                        else
                        {
                            strSQL += " AND A_PRK2 .MZ_PRID='" + PRID + "'";
                        }

                        // strSQL += " A_PRK2 .MZ_PRID='" + PRID + "' ";
                    }

                    //Where A_PRK2.MZ_PRID='" + PRID + "' ";


                    if (MZ_SRANK != string.Empty)
                    {
                        if (MZ_SRANK == "P8G22")
                        {
                            strSQL += " and A_PRK2.MZ_SRANK NOT IN ('G22','G21','G31','G32','G33','G34','P08','P09','P10','P11','P12','P13','P14')";
                        }

                        else if (MZ_SRANK == "P9G23")
                        {
                            //strSQL += " and MZ_SRANK IN ('G22','G21','G31','G32','G33','G34','P08','P09','P10','P11','P12','P13','P14')";
                            strSQL += " and A_PRK2.MZ_SRANK IN ('G22','G21','P08','P09','P10','P11','P12','P13','P14')";

                        }
                        else if (MZ_SRANK == "G3")
                        {
                            strSQL += " and A_PRK2.MZ_SRANK IN ('G31','G32','G33','G34')";

                        }
                    }

                    if (PRID1 != "")
                    {
                        strSQL += " AND A_PRK2.MZ_PRID1='" + o_str.tosql(PRID1) + "'";
                    }

                    strSQL += " ORDER BY TBDV,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_ID";

                    DataTable punishSug = new DataTable();

                    punishSug = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");

                    return punishSug;

                }

                public static DataTable doTable(string PRID, string PRID1, string CHKAD, DataTable punish)
                {
                    DataTable rpt = new DataTable();
                    rpt.Columns.Add("TOJ", typeof(string));
                    rpt.Columns.Add("MZ_NAME", typeof(string));
                    rpt.Columns.Add("MZ_ID", typeof(string));
                    rpt.Columns.Add("MZ_PRRST", typeof(string));
                    rpt.Columns.Add("MZ_PRID", typeof(string));
                    rpt.Columns.Add("MZ_PRID1", typeof(string));
                    rpt.Columns.Add("MZ_DATE", typeof(string));
                    rpt.Columns.Add("MZ_PRCT", typeof(string));
                    rpt.Columns.Add("MZ_PROLNO", typeof(string));
                    rpt.Columns.Add("MZ_MEMO", typeof(string));
                    rpt.Columns.Add("MZ_REMARK", typeof(string));
                    rpt.Columns.Add("SPEED_NO", typeof(string));
                    rpt.Columns.Add("PWD_NO", typeof(string));
                    rpt.Columns.Add("MZ_FILENO", typeof(string));
                    rpt.Columns.Add("MZ_YEARUSE", typeof(string));
                    rpt.Columns.Add("MZ_PRRST_NAME", typeof(string));
                    rpt.Columns.Add("PRID3", typeof(string));
                    rpt.Columns.Add("PRINTGROUP", typeof(string));
                    rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
                    rpt.Columns.Add("NOW", typeof(string));
                    rpt.Columns.Add("PAGECOUNT", typeof(string));

                    for (int i = 0; i < punish.Rows.Count; i++)
                    {
                        DataRow newdr = rpt.NewRow();
                        newdr["TOJ"] = string.Empty;
                        newdr["MZ_NAME"] = punish.Rows[i]["MZ_NAME"];
                        newdr["MZ_ID"] = punish.Rows[i]["MZ_ID"];
                        newdr["MZ_PRRST"] = punish.Rows[i]["MZ_PRRST"];
                        newdr["MZ_PRID"] = punish.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = punish.Rows[i]["MZ_PRID1"];
                        newdr["MZ_DATE"] = punish.Rows[i]["MZ_DATE"];
                        newdr["MZ_PRCT"] = RPT.punish.A_punish.PRCT(punish, i);
                        newdr["MZ_PROLNO"] = punish.Rows[i]["MZ_PROLNO"];
                        newdr["MZ_MEMO"] = punish.Rows[i]["MZ_MEMO"];
                        newdr["MZ_REMARK"] = punish.Rows[i]["MZ_REMARK"];
                        newdr["SPEED_NO"] = punish.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = punish.Rows[i]["PWD_NO"];
                        newdr["MZ_FILENO"] = punish.Rows[i]["MZ_FILENO"];
                        newdr["MZ_YEARUSE"] = punish.Rows[i]["MZ_YEARUSE"];
                        newdr["MZ_PRRST_NAME"] = punish.Rows[i]["MZ_PRRST_NAME"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = "1";

                        //20140123
                        newdr["COUNTPAGEMAN"] = DateManange.toChinese((i + 1).ToString(), 1);
                        //newdr["COUNTPAGEMAN"] = o_crystal.toChinese((i + 1).ToString(), 1);
                        newdr["NOW"] = RPT.punish.A_punish.NOW(punish, i);
                        newdr["PAGECOUNT"] = (i / 2).ToString().PadLeft(4, '0');

                        rpt.Rows.Add(newdr);
                    }


                    return rpt;



                }

            }

            /// <summary>
            /// 2.5.5 獎懲核定名冊_全部_依機關_皆適用
            /// </summary>
            public class A_checklist
            {
                public static DataTable doSearch(string MZ_SWT3, string MZ_NO1, string MZ_NO2)
                {
                    string strSQL = @"SELECT MZ_ID,MZ_NAME,MZ_NO,MZ_MEMO,MZ_PRCT, 
                             AKD.MZ_KCHI  MZ_AD, AKU.MZ_KCHI  MZ_UNIT , AKO.MZ_KCHI  MZ_OCCC , AKS.MZ_KCHI  MZ_SRANK , AKP.MZ_KCHI  MZ_PRRST , 
                             dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,MZ_OCCC AS OCCC
                             FROM A_PRKB
                             LEFT JOIN A_KTYPE AKD ON AKD.MZ_KCODE=MZ_AD AND AKD.MZ_KTYPE='04' AND (AKD.MZ_KCODE LIKE '38213%' OR AKD.MZ_KCODE LIKE '3764118%')
                             LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE=MZ_UNIT AND AKU.MZ_KTYPE='25' 
                             LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26' 
                             LEFT JOIN A_KTYPE AKS ON AKS.MZ_KCODE=MZ_SRANK AND AKS.MZ_KTYPE='09' 
                             LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE=MZ_PRRST AND AKP.MZ_KTYPE='24' 
                             WHERE      MZ_SWT3='" + MZ_SWT3 + "' ";

                    if (MZ_NO1 != "" && MZ_NO2 != "")
                    {
                        strSQL += " AND MZ_NO>='" + MZ_NO1 + "' AND MZ_NO<='" + MZ_NO2 + "'";
                    }
                    else
                    {
                        strSQL += " AND MZ_NO='" + MZ_NO1 + "'";
                    }


                    strSQL += " ORDER BY TBDV,MZ_AD,MZ_UNIT,OCCC,MZ_ID";

                    DataTable checklist = new DataTable();

                    checklist = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");

                    for (int i = 0; i < checklist.Rows.Count; i++)
                    {
                        checklist.Rows[i]["MZ_PRCT"] = o_CommonService.d_report_break_line(checklist.Rows[i]["MZ_PRCT"].ToString(), 32, "&N");
                    }


                    return checklist;
                }

            }

            /// <summary>
            /// 2.5.6 個人獎懲明細
            /// </summary>
            public class A_gradedetail
            {
                public static DataTable doSearch(string MZ_ID, string MZ_NAME, string MZ_IDATE1, string MZ_IDATE2)
                {
                    string sqlPart = "";// "dbo.SUBSTR(MZ_IDATE,1,3)='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) + "'";



                    if (!string.IsNullOrEmpty(MZ_ID))
                    {
                        sqlPart += " AND MZ_ID='" + MZ_ID + "'";
                    }

                    if (!string.IsNullOrEmpty(MZ_NAME))
                    {
                        sqlPart += " AND MZ_NAME='" + MZ_NAME + "'";
                    }

                    if (!string.IsNullOrEmpty(MZ_IDATE1) && (!string.IsNullOrEmpty(MZ_IDATE2)))
                    {
                        sqlPart += " AND  MZ_IDATE>='" + MZ_IDATE1.PadLeft(7, '0') + "' AND MZ_IDATE<='" + MZ_IDATE2.PadLeft(7, '0') + "'";

                    }
                    else if (!string.IsNullOrEmpty(MZ_IDATE1))
                    {
                        sqlPart += " AND  MZ_IDATE='" + MZ_IDATE1 + "'";

                    }

                    string strsql = @"SELECT A.MZ_PRRST,NVL( B.TOTAL , 0) TOTAL  FROM A_PRK2 A 
LEFT JOIN (SELECT MZ_PRRST, COUNT(*) AS TOTAL FROM A_PRK2  WHERE 1=1 " + sqlPart + @"  GROUP BY MZ_PRRST ) B ON A.MZ_PRRST = B.MZ_PRRST 
WHERE    A.MZ_PRRST  IN ('4001','4002','4010','4020','4100','4200','5001','5002','5010','5020','5100') GROUP BY A.MZ_PRRST,B.TOTAL ORDER BY A.MZ_PRRST";

                    DataTable caluation = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");
                    int[] arry = new int[5101];
                    for (int j = 0; j < caluation.Rows.Count; j++)
                    {
                        arry[int.Parse(caluation.Rows[j][0].ToString())] = int.Parse(caluation.Rows[j][1].ToString());
                    }
                    int i4001 = arry[4001] + arry[4002] * 2;
                    int i4010 = arry[4010] + arry[4020] * 2;
                    int i4100 = arry[4100];
                    int i4200 = arry[4200];
                    int i5001 = arry[5001] + arry[5002] * 2;
                    int i5010 = arry[5010] + arry[5020] * 2;
                    int i5100 = arry[5100];
                    //int i4001 = int.Parse(caluation.Rows[0][1].ToString()) + int.Parse(caluation.Rows[1][1].ToString()) * 2;
                    //int i4010 = int.Parse(caluation.Rows[2][1].ToString()) + int.Parse(caluation.Rows[3][1].ToString()) * 2;
                    //int i4100 = int.Parse(caluation.Rows[4][1].ToString());
                    //int i4200 = int.Parse(caluation.Rows[5][1].ToString());

                    //int i5001 = int.Parse(caluation.Rows[6][1].ToString()) + int.Parse(caluation.Rows[7][1].ToString()) * 2;
                    //int i5010 = int.Parse(caluation.Rows[8][1].ToString()) + int.Parse(caluation.Rows[9][1].ToString()) * 2;
                    //int i5100 = int.Parse(caluation.Rows[10][1].ToString());

                    int TOTAL = i4001 + i4010 * 3 + i4100 * 9 + i4200 * 2 * 9 - i5001 - i5010 * 3 - i5100 * 9;

                    string Cname = o_DBFactory.ABC_toTest.vExecSQL(@"SELECT AKD.MZ_KCHI + AKU.MZ_KCHI + ' ' +AKO.MZ_KCHI + ' ' +MZ_NAME NAME
 FROM A_DLBASE 
LEFT JOIN A_KTYPE AKD ON AKD.MZ_KCODE=A_DLBASE.MZ_AD AND AKD.MZ_KTYPE='04'
LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE=A_DLBASE.MZ_UNIT AND AKU.MZ_KTYPE='25'
LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE='26'
WHERE MZ_ID='" + MZ_ID + "'");

                    //
                    strsql = "SELECT " + i4001 + " AS I4001," + i4010 + " AS I4010," + i4100 + " AS I4100," + i4200 + " AS I4200, " +
                                               i5001 + " AS I5001," + i5010 + " AS I5010," + i5100 + " AS I5100," + TOTAL + " AS TOTAL, " +
                                           "MZ_ID,'" + Cname + "' AS MZ_NAME,MZ_IDATE,MZ_PRID,MZ_PRID1,MZ_PRCT,AKP.MZ_KCHI MZ_PRRST  " +
                                   "FROM " +
                                           "A_PRK2  " +
                                           " LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE=A_PRK2.MZ_PRRST AND AKP.MZ_KTYPE='24'" +
                                   "WHERE 1=1" + sqlPart + " ORDER BY MZ_IDATE DESC";

                    DataTable gradedetail = new DataTable();

                    gradedetail = o_DBFactory.ABC_toTest.Create_Table(strsql, "DETAIL");

                    if (gradedetail.Rows.Count == 0)
                    {//2013/12/02

                        strsql = "SELECT 0 AS I4001,0 AS I4010,0 AS I4100,0 AS I4200,0 AS I5001,0 AS I5010,0 AS I5100,0 AS TOTAL, " +
                                                 "MZ_ID,'" + Cname + "' AS MZ_NAME,'' AS MZ_IDATE,'' AS MZ_PRID,'' AS MZ_PRID1,'' AS MZ_PRCT,'' AS MZ_PRRST " +
                                         "FROM " +
                                                 "A_DLBASE " +
                                         "WHERE MZ_ID='" + MZ_ID + "'";
                        gradedetail = o_DBFactory.ABC_toTest.Create_Table(strsql, "DETAIL");
                    }

                    return gradedetail;
                }

                public static string COUNTDATE(string MZ_IDATE1, string MZ_IDATE2)
                {
                    string DATE = "";
                    if (!string.IsNullOrEmpty(MZ_IDATE1) && (!string.IsNullOrEmpty(MZ_IDATE2)))
                    {


                        DATE = int.Parse(MZ_IDATE1.Substring(0, 3)).ToString() + "年" + int.Parse(MZ_IDATE1.Substring(3, 2)).ToString() + "月" + int.Parse(MZ_IDATE2.Substring(5, 2)).ToString() + "日至" + int.Parse(MZ_IDATE2.Substring(0, 3)).ToString() + "年" + int.Parse(MZ_IDATE2.Substring(3, 2)).ToString() + "月" + int.Parse(MZ_IDATE2.Substring(5, 2)).ToString() + "日";
                    }
                    else if (!string.IsNullOrEmpty(MZ_IDATE1))
                    {

                        DATE = int.Parse(MZ_IDATE1.Substring(0, 3)).ToString() + "年" + int.Parse(MZ_IDATE1.Substring(3, 2)).ToString() + "月" + int.Parse(MZ_IDATE2.Substring(5, 2)).ToString() + "日";
                    }
                    return DATE;
                }


            }


            /// <summary>
            /// 2.5.7 調他機關名冊
            /// </summary>
            public class A_othergrade
            {
                public static DataTable doSearch(string MZ_SWT3, string MZ_NO1, string MZ_NO2)
                {
                    string strSQL = @"SELECT AKD.MZ_KCHI  MZ_AD, AKU.MZ_KCHI  MZ_UNIT , AKS.MZ_KCHI  MZ_SRANK , AKO.MZ_KCHI  MZ_OCCC , AKP.MZ_KCHI  MZ_PRRST , 
                                   dbo.SUBSTR(MZ_ID,0,6) + '****' MZ_ID,MZ_NAME,MZ_PRCT,MZ_MEMO 
                            FROM A_PRKB         
                            LEFT JOIN A_KTYPE AKD ON AKD.MZ_KCODE=MZ_EXAD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                            LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26' 
                            LEFT JOIN A_KTYPE AKS ON AKS.MZ_KCODE=MZ_SRANK AND AKS.MZ_KTYPE='09' 
                            LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE=MZ_PRRST AND AKP.MZ_KTYPE='24'            
                            WHERE MZ_SWT3='" + MZ_SWT3 + "' ";

                    if (MZ_NO1 != "" && MZ_NO2 != "")
                    {
                        strSQL += " AND MZ_NO>='" + MZ_NO1 + "' AND MZ_NO<='" + MZ_NO2 + "'";
                    }
                    else
                    {
                        strSQL += " AND MZ_NO='" + MZ_NO1 + "'";
                    }


                    strSQL += " Order By MZ_EXAD,MZ_TBDV ";

                    DataTable othergrade = new DataTable();

                    othergrade = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");

                    for (int i = 0; i < othergrade.Rows.Count; i++)
                    {
                        othergrade.Rows[i]["MZ_PRCT"] = o_CommonService.d_report_break_line(othergrade.Rows[i]["MZ_PRCT"].ToString(), 36, "&N");
                    }


                    return othergrade;
                }

            }



            /// <summary>
            /// 2.5.9 功過相抵申誡以上報表
            /// </summary>
            public class A_goodtobad
            {
                public static DataTable doSearch(string MZ_AD, string MZ_DATE1, string MZ_DATE2)
                {
                    string strPart = "";
                    DataTable Goodtobad = new DataTable();

                    Goodtobad.Columns.Add("MZ_UNIT", typeof(string));
                    Goodtobad.Columns.Add("MZ_OCCC", typeof(string));
                    Goodtobad.Columns.Add("MZ_NAME", typeof(string));
                    Goodtobad.Columns.Add("MZ_ID", typeof(string));
                    Goodtobad.Columns.Add("I4001", typeof(int));
                    Goodtobad.Columns.Add("I4010", typeof(int));
                    Goodtobad.Columns.Add("I4100", typeof(int));
                    Goodtobad.Columns.Add("I4200", typeof(int));
                    Goodtobad.Columns.Add("I5001", typeof(int));
                    Goodtobad.Columns.Add("I5010", typeof(int));
                    Goodtobad.Columns.Add("I5100", typeof(int));
                    Goodtobad.Columns.Add("TOTAL", typeof(int));

                    if (!string.IsNullOrEmpty(MZ_DATE1) &&
                    !string.IsNullOrEmpty(MZ_DATE2))
                    {
                        strPart += " AND MZ_DATE>='" + o_str.tosql(MZ_DATE1) +
                                "' AND MZ_DATE<='" + o_str.tosql(MZ_DATE2) + "' ";
                    }
                    else if (!string.IsNullOrEmpty(MZ_DATE1))
                    {
                        strPart += " AND MZ_DATE='" + o_str.tosql(MZ_DATE1) + "' ";
                    }

                    if (!string.IsNullOrEmpty(MZ_AD))
                    {
                        strPart += "AND MZ_AD='" + MZ_AD + "' ";
                    }

                    //string strSQL = "SELECT DISTINCT MZ_ID FROM A_PRK2 WHERE 1=1 " + strPart;

                    //DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                    string strSQL = @"SELECT   MZ_ID , 
 COUNT(DECODE(MZ_PRRST,4001,'X',NULL)) +(COUNT(DECODE(MZ_PRRST,4002,'X',NULL))  *2) AS I4001,
COUNT(DECODE(MZ_PRRST,4010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,4020,'X',NULL))  *2) AS I4010,
COUNT(DECODE(MZ_PRRST,4100,'X',NULL)) AS I4100,
COUNT(DECODE(MZ_PRRST,4200,'X',NULL)) AS I4200,
COUNT(DECODE(MZ_PRRST,5001,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5002,'X',NULL)) *2)  AS I5001,
COUNT(DECODE(MZ_PRRST,5010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5020,'X',NULL))  *2)  AS I5010,
COUNT(DECODE(MZ_PRRST,5100,'X',NULL)) AS  I5100 ,
(
 (COUNT(DECODE(MZ_PRRST,4001,'X',NULL)) +(COUNT(DECODE(MZ_PRRST,4002,'X',NULL))  *2) ) +
(COUNT(DECODE(MZ_PRRST,4010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,4020,'X',NULL))  *2)  )*3 +
(COUNT(DECODE(MZ_PRRST,4100,'X',NULL)) ) *9 -
(COUNT(DECODE(MZ_PRRST,5001,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5002,'X',NULL)) *2) ) -
(  COUNT(DECODE(MZ_PRRST,5010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5020,'X',NULL))  *2)  )*3 -
COUNT(DECODE(MZ_PRRST,5100,'X',NULL)) *9 
) AS TOTAL
FROM A_PRK2
LEFT JOIN A_KTYPE AKU  ON  AKU.MZ_KCODE=A_PRK2.MZ_UNIT AND AKU.MZ_KTYPE='25' 
LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=A_PRK2.MZ_OCCC AND AKO.MZ_KTYPE='26'
WHERE MZ_PRRST IN ('4001','4002','4010','4020','4100','4200','5001','5002','5010','5020','5100') "
    //AND MZ_ID IN ( SELECT DISTINCT MZ_ID FROM A_PRK2 WHERE 1=1 " + strPart +") "
    + strPart + "GROUP BY MZ_ID";

                    DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


                    for (int i = 0; i < source.Rows.Count; i++)
                    {
                        if (int.Parse(source.Rows[i]["TOTAL"].ToString()) < 0)
                        {
                            strSQL = @"SELECT AKU.MZ_KCHI  MZ_UNIT ,AKO.MZ_KCHI MZ_OCCC , MZ_NAME  
                                       FROM A_PRK2
                                       LEFT JOIN A_KTYPE AKU  ON  AKU.MZ_KCODE=A_PRK2.MZ_UNIT AND AKU.MZ_KTYPE='25' 
                                       LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=A_PRK2.MZ_OCCC AND AKO.MZ_KTYPE='26'
                                      WHERE 1=1 AND MZ_ID='" + source.Rows[i]["MZ_ID"].ToString() + "' " + strPart;
                            DataTable NAME = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


                            DataRow tempRow = Goodtobad.NewRow();

                            tempRow["MZ_UNIT"] = NAME.Rows[0]["MZ_UNIT"].ToString();
                            tempRow["MZ_OCCC"] = NAME.Rows[0]["MZ_OCCC"].ToString();
                            tempRow["MZ_NAME"] = NAME.Rows[0]["MZ_NAME"].ToString();
                            tempRow["MZ_ID"] = source.Rows[i][0].ToString();
                            tempRow["I4001"] = int.Parse(source.Rows[i]["I4001"].ToString());
                            tempRow["I4010"] = int.Parse(source.Rows[i]["I4010"].ToString());
                            tempRow["I4100"] = int.Parse(source.Rows[i]["I4100"].ToString());
                            tempRow["I4200"] = int.Parse(source.Rows[i]["I4200"].ToString());
                            tempRow["I5001"] = int.Parse(source.Rows[i]["I5001"].ToString());
                            tempRow["I5010"] = int.Parse(source.Rows[i]["I5010"].ToString());
                            tempRow["I5100"] = int.Parse(source.Rows[i]["I5100"].ToString());
                            tempRow["TOTAL"] = int.Parse(source.Rows[i]["TOTAL"].ToString()) * -1;
                            Goodtobad.Rows.Add(tempRow);
                        }

                    }

                    //舊的原始碼 如果機關有300人 再加獎懲項目 假設6個 就要進資料庫 300*6 次
                    //                    for (int i = 0; i < source.Rows.Count; i++)
                    //                    {
                    //                        string strsql = @"SELECT A.MZ_PRRST,NVL( B.TOTAL , 0) TOTAL  FROM A_PRK2 A 
                    //LEFT JOIN (SELECT MZ_PRRST, COUNT(*) AS TOTAL FROM A_PRK2  WHERE 1=1  AND MZ_ID='" + source.Rows[i][0].ToString() + "'" + strPart + @"  GROUP BY MZ_PRRST ) B ON A.MZ_PRRST = B.MZ_PRRST 
                    //WHERE    A.MZ_PRRST  IN ('4001','4002','4010','4020','4100','4200','5001','5002','5010','5020','5100') GROUP BY A.MZ_PRRST,B.TOTAL ORDER BY A.MZ_PRRST";


                    //                        DataTable caluation = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");

                    //                        int i4001 = int.Parse(caluation.Rows[0][1].ToString()) + int.Parse(caluation.Rows[1][1].ToString()) * 2;
                    //                        int i4010 = int.Parse(caluation.Rows[2][1].ToString()) + int.Parse(caluation.Rows[3][1].ToString()) * 2;
                    //                        int i4100 = int.Parse(caluation.Rows[4][1].ToString());
                    //                        int i4200 = int.Parse(caluation.Rows[5][1].ToString());

                    //                        int i5001 = int.Parse(caluation.Rows[6][1].ToString()) + int.Parse(caluation.Rows[7][1].ToString()) * 2;
                    //                        int i5010 = int.Parse(caluation.Rows[8][1].ToString()) + int.Parse(caluation.Rows[9][1].ToString()) * 2;
                    //                        int i5100 = int.Parse(caluation.Rows[10][1].ToString());

                    //                        int TOTAL = i4001 + i4010 * 3 + i4100 * 9 + i4200 * 2 * 9 - i5001 - i5010 * 3 - i5100 * 9;

                    //                        if (TOTAL < 0)
                    //                        {
                    //                            DataRow tempRow = Goodtobad.NewRow();

                    //                            strSQL = @"SELECT AKU.MZ_KCHI  MZ_UNIT ,AKO.MZ_KCHI, MZ_OCCC , MZ_NAME  
                    //                                       FROM A_PRK2
                    //                                       LEFT JOIN A_KTYPE AKU  ON  AKU.MZ_KCODE=A_PRK2.MZ_UNIT AND AKU.MZ_KTYPE='25' 
                    //                                       LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=A_PRK2.MZ_OCCC AND AKO.MZ_KTYPE='26'
                    //                                       WHERE MZ_PRRST IN ('4001','4002','4010','4020','4100','4200','5001','5002','5010','5020','5100') " + strPart;
                    //                            DataTable NAME = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


                    //                            //tempRow["MZ_UNIT"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=(SELECT MZ_UNIT FROM A_DLBASE WHERE MZ_ID='" + source.Rows[i][0].ToString() + "')");
                    //                            //tempRow["MZ_OCCC"] = o_A_DLBASE.OCCC(source.Rows[i][0].ToString());
                    //                            //tempRow["MZ_NAME"] = o_A_DLBASE.CNAME(source.Rows[i][0].ToString());

                    //                            tempRow["MZ_UNIT"] = NAME.Rows[0]["MZ_UNIT"].ToString();
                    //                            tempRow["MZ_OCCC"] = NAME.Rows[0]["MZ_OCCC"].ToString();
                    //                            tempRow["MZ_NAME"] = NAME.Rows[0]["MZ_NAME"].ToString();
                    //                            tempRow["MZ_ID"] = source.Rows[i][0].ToString();
                    //                            tempRow["I4001"] = i4001;
                    //                            tempRow["I4010"] = i4010;
                    //                            tempRow["I4100"] = i4100;
                    //                            tempRow["I4200"] = i4200;
                    //                            tempRow["I5001"] = i5001;
                    //                            tempRow["I5010"] = i5010;
                    //                            tempRow["I5100"] = i5100;
                    //                            tempRow["TOTAL"] = TOTAL * -1;

                    //                            Goodtobad.Rows.Add(tempRow);
                    //                        }
                    //                    }



                    return Goodtobad;

                }

            }

            /// <summary>
            /// 2.5.11 各機關年度獎懲統計表
            /// </summary>
            public class A_yearofgradelist
            {
                public static DataTable doSearch(string MZ_AD, string MZ_DATE1, string MZ_DATE2, int TYPE)
                {
                    DataTable Yearofgradelist = new DataTable();
                    if (TYPE == 1)
                    {
                        Yearofgradelist.Columns.Add("單位", typeof(string));
                        Yearofgradelist.Columns.Add("職稱", typeof(string));
                        Yearofgradelist.Columns.Add("姓名", typeof(string));
                        Yearofgradelist.Columns.Add("身份證字號", typeof(string));
                        Yearofgradelist.Columns.Add("嘉獎", typeof(int));
                        Yearofgradelist.Columns.Add("記功", typeof(int));
                        Yearofgradelist.Columns.Add("記一大功", typeof(int));
                        Yearofgradelist.Columns.Add("記二大功", typeof(int));
                        Yearofgradelist.Columns.Add("申誡", typeof(int));
                        Yearofgradelist.Columns.Add("記過", typeof(int));
                        Yearofgradelist.Columns.Add("記一大過", typeof(int));
                        //Yearofgradelist.Columns.Add("TBDV", typeof(int));
                        //Yearofgradelist.Columns.Add("OCCC", typeof(string));
                        Yearofgradelist.Columns.Add("功過相抵", typeof(int));
                    }
                    else if (TYPE == 2)
                    {
                        Yearofgradelist.Columns.Add("MZ_UNIT", typeof(string));
                        Yearofgradelist.Columns.Add("MZ_OCCC", typeof(string));
                        Yearofgradelist.Columns.Add("MZ_NAME", typeof(string));
                        Yearofgradelist.Columns.Add("MZ_ID", typeof(string));
                        Yearofgradelist.Columns.Add("I4001", typeof(int));
                        Yearofgradelist.Columns.Add("I4010", typeof(int));
                        Yearofgradelist.Columns.Add("I4100", typeof(int));
                        Yearofgradelist.Columns.Add("I4200", typeof(int));
                        Yearofgradelist.Columns.Add("I5001", typeof(int));
                        Yearofgradelist.Columns.Add("I5010", typeof(int));
                        Yearofgradelist.Columns.Add("I5100", typeof(int));
                        Yearofgradelist.Columns.Add("TBDV", typeof(int));
                        Yearofgradelist.Columns.Add("OCCC", typeof(string));
                        Yearofgradelist.Columns.Add("TOTAL", typeof(int));
                    }

                    string sqlPart = "";

                    if (MZ_AD != "")
                    {
                        sqlPart = " AND MZ_AD='" + MZ_AD + "'";
                    }

                    if (!string.IsNullOrEmpty(MZ_DATE1) && !string.IsNullOrEmpty(MZ_DATE2.Trim()))
                    {
                        sqlPart += " AND MZ_DATE>='" + o_str.tosql(MZ_DATE1.Replace("/", "").PadLeft(7, '0')) + "' AND MZ_DATE<='" + o_str.tosql(MZ_DATE2.Replace("/", "").PadLeft(7, '0')) + "'";
                    }
                    else if (!string.IsNullOrEmpty(MZ_DATE1))
                    {
                        sqlPart += " AND MZ_DATE='" + o_str.tosql(MZ_DATE1.Replace("/", "").PadLeft(7, '0')) + "'";
                    }

                    //20140411因為排序問題才寫那麼難看,如果後續有強手請更正
                    string strSQL = string.Format(@"SELECT A.MZ_ID, I4001, I4010, I4100, I4200, I5001, I5010, I5100, TOTAL FROM 
                                                        (SELECT MZ_ID,ROWNUM NUM FROM  (SELECT  MZ_ID  FROM (SELECT MZ_ID FROM A_PRK2 WHERE 1=1 {0} ORDER BY  MZ_TBDV,MZ_UNIT,MZ_OCCC  ) GROUP BY MZ_ID  ORDER BY min(ROWNUM) )) A 
                                                    INNER JOIN 
                                                    (
                                                        SELECT  MZ_ID , 
                                                                COUNT(DECODE(MZ_PRRST,4001,'X',NULL)) +(COUNT(DECODE(MZ_PRRST,4002,'X',NULL))  *2) AS I4001,
                                                                COUNT(DECODE(MZ_PRRST,4010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,4020,'X',NULL))  *2) AS I4010,
                                                                COUNT(DECODE(MZ_PRRST,4100,'X',NULL)) AS I4100,
                                                                COUNT(DECODE(MZ_PRRST,4200,'X',NULL)) AS I4200,
                                                                COUNT(DECODE(MZ_PRRST,5001,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5002,'X',NULL)) *2)  AS I5001,
                                                                COUNT(DECODE(MZ_PRRST,5010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5020,'X',NULL))  *2)  AS I5010,
                                                                COUNT(DECODE(MZ_PRRST,5100,'X',NULL)) AS I5100 ,
                                                                ((COUNT(DECODE(MZ_PRRST,4001,'X',NULL)) +(COUNT(DECODE(MZ_PRRST,4002,'X',NULL))  *2) ) +
                                                                 (COUNT(DECODE(MZ_PRRST,4010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,4020,'X',NULL))  *2)  )*3 +
                                                                 (COUNT(DECODE(MZ_PRRST,4100,'X',NULL)) ) *9 -
                                                                 (COUNT(DECODE(MZ_PRRST,5001,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5002,'X',NULL)) *2) ) -
                                                                 (COUNT(DECODE(MZ_PRRST,5010,'X',NULL)) + (COUNT(DECODE(MZ_PRRST,5020,'X',NULL))  *2)  )*3 -
                                                                 COUNT(DECODE(MZ_PRRST,5100,'X',NULL)) *9) AS TOTAL
                                                        FROM A_PRK2
                                                        LEFT JOIN A_KTYPE AKU  ON  AKU.MZ_KCODE=A_PRK2.MZ_UNIT AND AKU.MZ_KTYPE='25' 
                                                        LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=A_PRK2.MZ_OCCC AND AKO.MZ_KTYPE='26'
                                                        WHERE MZ_PRRST IN ('4001','4002','4010','4020','4100','4200','5001','5002','5010','5020','5100') 
                                                        {0} GROUP BY MZ_ID
                                                    ) B ON A.MZ_ID=B.MZ_ID ORDER BY NUM ", sqlPart);
                    DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


                    for (int i = 0; i < source.Rows.Count; i++)
                    {
                        strSQL = string.Format(@"SELECT AKU.MZ_KCHI MZ_UNIT, AKO.MZ_KCHI MZ_OCCC, MZ_NAME 
                                                FROM A_PRK2
                                                LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE=A_PRK2.MZ_UNIT AND AKU.MZ_KTYPE='25' 
                                                LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=A_PRK2.MZ_OCCC AND AKO.MZ_KTYPE='26'
                                                WHERE 1=1 AND MZ_ID = '{0}' {1} ", source.Rows[i]["MZ_ID"].ToString(), sqlPart);

                        DataTable NAME = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


                        DataRow tempRow = Yearofgradelist.NewRow();
                        if (TYPE == 1)
                        {
                            tempRow["單位"] = NAME.Rows[0]["MZ_UNIT"].ToString();
                            tempRow["職稱"] = NAME.Rows[0]["MZ_OCCC"].ToString();
                            tempRow["姓名"] = NAME.Rows[0]["MZ_NAME"].ToString();
                            tempRow["身份證字號"] = source.Rows[i][0].ToString();
                            tempRow["嘉獎"] = int.Parse(source.Rows[i]["I4001"].ToString());
                            tempRow["記功"] = int.Parse(source.Rows[i]["I4010"].ToString());
                            tempRow["記一大功"] = int.Parse(source.Rows[i]["I4100"].ToString());
                            tempRow["記二大功"] = int.Parse(source.Rows[i]["I4200"].ToString());
                            tempRow["申誡"] = int.Parse(source.Rows[i]["I5001"].ToString());
                            tempRow["記過"] = int.Parse(source.Rows[i]["I5010"].ToString());
                            tempRow["記一大過"] = int.Parse(source.Rows[i]["I5100"].ToString());
                            //tempRow["TBDV"] = rpt_dt.Rows[i]["TBDV"].ToString();
                            //tempRow["OCCC"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='" + rpt_dt.Rows[i][0].ToString() + "'");
                            tempRow["功過相抵"] = int.Parse(source.Rows[i]["TOTAL"].ToString());
                        }
                        else if (TYPE == 2)
                        {

                            tempRow["MZ_UNIT"] = NAME.Rows[0]["MZ_UNIT"].ToString();
                            tempRow["MZ_OCCC"] = NAME.Rows[0]["MZ_OCCC"].ToString();
                            tempRow["MZ_NAME"] = NAME.Rows[0]["MZ_NAME"].ToString();
                            tempRow["MZ_ID"] = source.Rows[i][0].ToString();
                            tempRow["I4001"] = int.Parse(source.Rows[i]["I4001"].ToString());
                            tempRow["I4010"] = int.Parse(source.Rows[i]["I4010"].ToString());
                            tempRow["I4100"] = int.Parse(source.Rows[i]["I4100"].ToString());
                            tempRow["I4200"] = int.Parse(source.Rows[i]["I4200"].ToString());
                            tempRow["I5001"] = int.Parse(source.Rows[i]["I5001"].ToString());
                            tempRow["I5010"] = int.Parse(source.Rows[i]["I5010"].ToString());
                            tempRow["I5100"] = int.Parse(source.Rows[i]["I5100"].ToString());
                            tempRow["TOTAL"] = int.Parse(source.Rows[i]["TOTAL"].ToString());

                        }
                        Yearofgradelist.Rows.Add(tempRow);
                    }

                    return Yearofgradelist;
                }

            }

            /// <summary>
            /// 2.5.14 記功過以上獎懲明細報表
            /// </summary>
            public class biggradelist
            {
                public static DataTable doSearch(string MZ_PRRST, string MZ_DATE1, string MZ_DATE2)
                {


                    string strSQL = @"SELECT  MZ_NAME,MZ_ID,MZ_DATE,MZ_PRID,MZ_PRID1,MZ_PRCT,
                             AKP.MZ_KCHI MZ_PRRST 
                             FROM A_PRK2 
                             LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE=MZ_PRRST AND AKP.MZ_KTYPE='24'
                             WHERE                              
                             MZ_CHKAD='" + HttpContext.Current.Session["ADPMZ_EXAD"].ToString().Trim() +
                                    "' AND MZ_PRRST='" + MZ_PRRST + "'";
                    //matthew 如果是中和 要把中一跟中二都列出來
                    if (HttpContext.Current.Session["ADPMZ_EXAD"].ToString().Trim() == "382133600C")
                    {
                        strSQL = @"SELECT  MZ_NAME,MZ_ID,MZ_DATE,MZ_PRID,MZ_PRID1,MZ_PRCT,
                             AKP.MZ_KCHI MZ_PRRST 
                             FROM A_PRK2 
                             LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE=MZ_PRRST AND AKP.MZ_KTYPE='24'
                             WHERE                              
                             MZ_CHKAD in ('382133400C','382133500C','382133600C')" + " AND MZ_PRRST='" + MZ_PRRST + "'";
                    }
                    if (!string.IsNullOrEmpty(MZ_DATE1) && !string.IsNullOrEmpty(MZ_DATE2))
                    {
                        strSQL += " AND MZ_DATE BETWEEN '" + MZ_DATE1.Trim() + "' AND '" + MZ_DATE2.Trim() + "'";
                    }
                    else if (!string.IsNullOrEmpty(MZ_DATE1))
                    {
                        strSQL += " AND MZ_DATE='" + MZ_DATE1.Trim() + "'";
                    }

                    strSQL += "  ORDER BY MZ_DATE,MZ_OCCC";


                    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                    return dt;


                }

            }


        }



        #endregion 獎懲

        #region 服務證
        /// <summary>
        /// 服務證印領清冊-刑事警察
        /// </summary>
        public class Criminal_police_2
        {
            public static DataTable doSearch(DataTable rpt_dt)
            {
                DataTable Criminal_police = new DataTable();

                Criminal_police.Columns.Add("MZ_UNIT", typeof(string));
                Criminal_police.Columns.Add("MZ_OCCC", typeof(string));
                Criminal_police.Columns.Add("MZ_NAME", typeof(string));
                Criminal_police.Columns.Add("MZ_IDNO", typeof(string));
                Criminal_police.Columns.Add("MZ_NO1", typeof(string));
                Criminal_police.Columns.Add("MZ_PIC", System.Type.GetType("System.Byte[]"));
                Criminal_police.Columns.Add("MZ_DATE", typeof(string));
                Criminal_police.Columns.Add("MZ_MEMO", typeof(string));
                Criminal_police.Columns.Add("MZ_BIR", typeof(string));


                for (int i = 0; i < rpt_dt.Rows.Count; i++)
                {
                    DataRow dr = Criminal_police.NewRow();

                    dr["MZ_UNIT"] = o_A_KTYPE.CODE_TO_NAME(o_A_DLBASE.PUNIT(rpt_dt.Rows[i]["MZ_ID"].ToString()), "25");
                    dr["MZ_OCCC"] = rpt_dt.Rows[i]["MZ_OCCC"].ToString();
                    dr["MZ_NAME"] = rpt_dt.Rows[i]["MZ_NAME"].ToString();
                    dr["MZ_IDNO"] = rpt_dt.Rows[i]["MZ_IDNO"].ToString();
                    dr["MZ_NO1"] = rpt_dt.Rows[i]["MZ_NO1"].ToString();
                    dr["MZ_PIC"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + rpt_dt.Rows[i]["MZ_ID"].ToString() + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + rpt_dt.Rows[i]["MZ_ID"].ToString() + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1"))); ;
                    string DATE = rpt_dt.Rows[i]["MZ_DATE"].ToString();
                    dr["MZ_DATE"] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "/" + DATE.Substring(3, 2).PadLeft(2, '0') + "/" + DATE.Substring(5, 2).PadLeft(2, '0');
                    dr["MZ_MEMO"] = rpt_dt.Rows[i]["MZ_MEMO"].ToString();
                    string BIR = rpt_dt.Rows[i]["MZ_BIR"].ToString();
                    if (BIR.Length < 7)
                        BIR = "0" + BIR;
                    dr["MZ_BIR"] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "/" + BIR.Substring(3, 2).PadLeft(2, '0') + "/" + BIR.Substring(5, 2).PadLeft(2, '0');
                    Criminal_police.Rows.Add(dr);
                }



                return Criminal_police;
            }


            public static byte[] imageToByte(string path)
            {
                string file_path = string.Empty;
                try
                {
                    file_path = HttpContext.Current.Server.MapPath(path);
                    System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fileOpen);
                    byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                    fileOpen.Close();
                    //XX2013/06/18 
                    fileOpen.Dispose();
                    return by;

                }
                catch
                {
                    file_path = HttpContext.Current.Server.MapPath("~/1-personnel/images/nopic.jpg");
                    System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fileOpen);
                    byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                    fileOpen.Close();
                    //XX2013/06/18 
                    fileOpen.Dispose();
                    return by;
                }
            }

        }
        //到時候檢查兩個內容是否一樣
        /// <summary>
        /// 服務證印領清冊-行政警察與一般行政
        /// </summary>
        public class Normal_police_2
        {
            public static DataTable doSearch(DataTable rpt_dt)
            {
                DataTable Criminal_police = new DataTable();

                Criminal_police.Columns.Add("MZ_UNIT", typeof(string));
                Criminal_police.Columns.Add("MZ_OCCC", typeof(string));
                Criminal_police.Columns.Add("MZ_NAME", typeof(string));
                Criminal_police.Columns.Add("MZ_IDNO", typeof(string));
                Criminal_police.Columns.Add("MZ_NO1", typeof(string));
                Criminal_police.Columns.Add("MZ_PIC", System.Type.GetType("System.Byte[]"));
                Criminal_police.Columns.Add("MZ_DATE", typeof(string));
                Criminal_police.Columns.Add("MZ_MEMO", typeof(string));
                Criminal_police.Columns.Add("MZ_BIR", typeof(string));


                for (int i = 0; i < rpt_dt.Rows.Count; i++)
                {
                    DataRow dr = Criminal_police.NewRow();

                    dr["MZ_UNIT"] = o_A_KTYPE.CODE_TO_NAME(o_A_DLBASE.PUNIT(rpt_dt.Rows[i]["MZ_ID"].ToString()), "25");
                    dr["MZ_OCCC"] = rpt_dt.Rows[i]["MZ_OCCC"].ToString();
                    dr["MZ_NAME"] = rpt_dt.Rows[i]["MZ_NAME"].ToString();
                    dr["MZ_IDNO"] = rpt_dt.Rows[i]["MZ_IDNO"].ToString();
                    dr["MZ_NO1"] = rpt_dt.Rows[i]["MZ_NO1"].ToString();
                    dr["MZ_PIC"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + rpt_dt.Rows[i]["MZ_ID"].ToString() + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + rpt_dt.Rows[i]["MZ_ID"].ToString() + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1"))); ;
                    string DATE = rpt_dt.Rows[i]["MZ_DATE"].ToString();
                    dr["MZ_DATE"] = string.IsNullOrEmpty(DATE) ? "" : DATE.Substring(0, 3) + "/" + DATE.Substring(3, 2).PadLeft(2, '0') + "/" + DATE.Substring(5, 2).PadLeft(2, '0');
                    dr["MZ_MEMO"] = rpt_dt.Rows[i]["MZ_MEMO"].ToString();
                    string BIR = rpt_dt.Rows[i]["MZ_BIR"].ToString();
                    if (BIR.Length < 7)
                        BIR = "0" + BIR;
                    dr["MZ_BIR"] = string.IsNullOrEmpty(BIR) ? "" : BIR.Substring(0, 3) + "/" + BIR.Substring(3, 2).PadLeft(2, '0') + "/" + BIR.Substring(5, 2).PadLeft(2, '0');
                    Criminal_police.Rows.Add(dr);
                }



                return Criminal_police;
            }


            public static byte[] imageToByte(string path)
            {
                string file_path = string.Empty;
                try
                {
                    file_path = HttpContext.Current.Server.MapPath(path);
                    System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fileOpen);
                    byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                    fileOpen.Close();
                    //XX2013/06/18 
                    fileOpen.Dispose();
                    return by;

                }
                catch
                {
                    file_path = HttpContext.Current.Server.MapPath("~/1-personnel/images/nopic.jpg");
                    System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fileOpen);
                    byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                    fileOpen.Close();
                    //XX2013/06/18 
                    fileOpen.Dispose();
                    return by;
                }
            }

        }

        #endregion 服務證

        /// <summary>
        /// 1.2.3全員基本資料名冊
        /// </summary>
        public class basic_all
        {
            public static DataTable doSearch(string AD, string unit)
            {
                DataTable basicAll = new DataTable();
                basicAll.Columns.Add("SN", typeof(string));
                basicAll.Columns.Add("MZ_OCCC", typeof(string));
                basicAll.Columns.Add("MZ_NAME", typeof(string));
                basicAll.Columns.Add("MZ_BIR", typeof(string));
                basicAll.Columns.Add("MZ_CAREER_AD", typeof(string));
                basicAll.Columns.Add("MZ_CAREER_UNIT", typeof(string));
                basicAll.Columns.Add("MZ_CAREER_ADATE", typeof(string));
                basicAll.Columns.Add("MZ_PHONO", typeof(string));
                basicAll.Columns.Add("MZ_PHONH", typeof(string));
                basicAll.Columns.Add("MZ_MOVETEL", typeof(string));
                basicAll.Columns.Add("MZ_SCHOOL", typeof(string));
                basicAll.Columns.Add("MZ_SCHOOL_DEPARTMENT", typeof(string));
                basicAll.Columns.Add("MZ_SCHOOL_YEAR", typeof(string));
                basicAll.Columns.Add("EXAM_NAME", typeof(string));
                basicAll.Columns.Add("UNIT", typeof(string));
                basicAll.Columns.Add("ADATE", typeof(string));
                basicAll.Columns.Add("IMG", System.Type.GetType("System.Byte[]"));

                string strSQL = @"SELECT MZ_EXTPOS,MZ_OCCC as OCCC, AKO.MZ_KCHI  MZ_OCCC,
                                 MZ_NAME,MZ_MOVETEL,MZ_PHONO,MZ_PHONH,A_DLBASE.MZ_ID,
(dbo.SUBSTR(MZ_BIR,1,3)+'年'+dbo.SUBSTR(MZ_BIR,4,2)+'月'+dbo.SUBSTR(MZ_BIR,6,2)+'日') MZ_BIR,(dbo.SUBSTR(MZ_ADATE,1,3) + '年' + dbo.SUBSTR(MZ_ADATE,4,2) + '月' + dbo.SUBSTR(MZ_ADATE,6,2)+'日') ADATE
                                  FROM A_DLBASE 
LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE =A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE='26'
WHERE MZ_STATUS2='Y' ";

                if (!string.IsNullOrEmpty(AD))
                    strSQL += " AND MZ_EXAD='" + AD + "'";

                if (!string.IsNullOrEmpty(unit))
                    strSQL += " AND MZ_EXUNIT='" + unit + "'";

                strSQL += " ORDER BY MZ_TBDV,OCCC";

                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                int num = 1;
                foreach (DataRow dr in temp.Rows)
                {
                    //兼
                    dr["MZ_OCCC"] = dr["MZ_OCCC"].ToString() + o_A_KTYPE.CODE_TO_NAME(dr["MZ_EXTPOS"].ToString(), "@91");
                    //經歷及基本資料
                    DataRow merge_dr = basicAll.NewRow();
                    //strSQL = string.Format("SELECT * FROM (SELECT MZ_ID,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KTYPE='04') MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') MZ_UNIT,(dbo.SUBSTR(MZ_ADATE,1,3) +'年'+dbo.SUBSTR(MZ_ADATE,4,2)+'月'+dbo.SUBSTR(MZ_ADATE,6,2)+'日') MZ_ADATE,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_CAREER.MZ_OCCC AND MZ_KTYPE='26') MZ_OCCC  FROM A_CAREER WHERE MZ_ID='{0}'  ORDER BY MZ_ADATE DESC ) WHERE ROWNUM<4", dr["MZ_ID"]);
                    //20140224
                    strSQL = string.Format(@"SELECT * FROM 
(SELECT MZ_ID,AKD.MZ_KCHI MZ_AD,AKU.MZ_KCHI  MZ_UNIT,
(dbo.SUBSTR(MZ_ADATE,1,3) +'年'+dbo.SUBSTR(MZ_ADATE,4,2)+'月'+dbo.SUBSTR(MZ_ADATE,6,2)+'日') MZ_ADATE,
AKO.MZ_KCHI  MZ_OCCC  ,ROWNUM NUM
FROM A_CAREER 
LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_AD AND AKD.MZ_KTYPE='04'
LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_UNIT AND AKU.MZ_KTYPE='25'
LEFT JOIN  A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26'
WHERE MZ_ID='{0}'  ORDER BY SN DESC ) WHERE  ROWNUM<4 
", dr["MZ_ID"]);
                    //SN 是後來加的序列, 因為基本資料名冊經歷排序有問題
                    //如果之後SN排序有問題,就只有改成用ROWNUM去排



                    DataTable temp1 = new DataTable();
                    temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    merge_dr["SN"] = num;
                    merge_dr["UNIT"] = o_A_DLBASE.CUNIT(dr["MZ_ID"].ToString());
                    merge_dr["ADATE"] = dr["ADATE"];
                    merge_dr["MZ_OCCC"] = dr["MZ_OCCC"];
                    merge_dr["MZ_NAME"] = dr["MZ_NAME"];
                    merge_dr["MZ_BIR"] = dr["MZ_BIR"];
                    string MZ_PHONO = string.IsNullOrEmpty(dr["MZ_PHONO"].ToString()) ? string.Empty : dr["MZ_PHONO"].ToString();
                    string MZ_PHONH = string.IsNullOrEmpty(dr["MZ_PHONH"].ToString()) ? string.Empty : dr["MZ_PHONH"].ToString();
                    string MZ_MOVETEL = string.IsNullOrEmpty(dr["MZ_MOVETEL"].ToString()) ? string.Empty : dr["MZ_MOVETEL"].ToString();
                    merge_dr["MZ_PHONO"] = "公：" + MZ_PHONO;
                    merge_dr["MZ_PHONH"] = "宅：" + MZ_PHONH;
                    merge_dr["MZ_MOVETEL"] = "行：" + MZ_MOVETEL;

                    if (temp1.Rows.Count > 0)
                    {
                        try
                        {
                            //最近經歷
                            try
                            {
                                merge_dr["MZ_CAREER_ADATE"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_ADATE"].ToString().Replace("年", "").Replace("月", "").Replace("日", "") + temp1.Rows[0]["MZ_AD"].ToString() + temp1.Rows[0]["MZ_UNIT"].ToString() + RANK1_NAME(temp1.Rows[0]["MZ_ID"].ToString(), 1) + temp1.Rows[0]["MZ_OCCC"].ToString(), 26, "&N");
                            }
                            catch { }
                            //第二近
                            try
                            {
                                merge_dr["MZ_CAREER_AD"] = o_CommonService.d_report_break_line(temp1.Rows[1]["MZ_ADATE"].ToString().Replace("年", "").Replace("月", "").Replace("日", "") + temp1.Rows[1]["MZ_AD"].ToString() + temp1.Rows[1]["MZ_UNIT"].ToString() + RANK1_NAME(temp1.Rows[1]["MZ_ID"].ToString(), 2) + temp1.Rows[1]["MZ_OCCC"].ToString(), 26, "&N");
                            }
                            catch { }
                            //第三近
                            try
                            {
                                merge_dr["MZ_CAREER_UNIT"] = o_CommonService.d_report_break_line(temp1.Rows[2]["MZ_ADATE"].ToString().Replace("年", "").Replace("月", "").Replace("日", "")
                                    + temp1.Rows[2]["MZ_AD"].ToString() + temp1.Rows[2]["MZ_UNIT"].ToString() + RANK1_NAME(temp1.Rows[2]["MZ_ID"].ToString(), 3) + temp1.Rows[2]["MZ_OCCC"].ToString(), 26, "&N");
                            }
                            catch { }
                        }
                        catch { }
                    }

                    //學歷
                    strSQL = string.Format("SELECT * FROM (SELECT MZ_YEAR,MZ_SCHOOL,MZ_DEPARTMENT,(dbo.SUBSTR(MZ_ENDDATE,1,3) +'年'+dbo.SUBSTR(MZ_ENDDATE,4,2)+'月'+dbo.SUBSTR(MZ_ENDDATE,6,2)+'日') MZ_ENDDATE  FROM A_EDUCATION WHERE MZ_ID='{0}'  ORDER BY MZ_ENDDATE DESC ) WHERE ROWNUM=1", dr["MZ_ID"]);
                    temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    if (temp1.Rows.Count > 0)
                    {
                        string year = string.Empty;
                        if (!string.IsNullOrEmpty(temp1.Rows[0]["MZ_YEAR"].ToString()))
                            year = "第" + temp1.Rows[0]["MZ_YEAR"] + "期";

                        merge_dr["MZ_SCHOOL"] = o_CommonService.d_report_break_line(temp1.Rows[0]["MZ_SCHOOL"].ToString() + temp1.Rows[0]["MZ_DEPARTMENT"].ToString() + year, 20, "&N");

                    }

                    //考試別
                    strSQL = string.Format("SELECT * FROM (SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=EXAM_NAME AND MZ_KTYPE='EXK') AS EXAM_NAME FROM A_EXAM WHERE MZ_ID='{0}' ORDER BY EXAM_YEAR) WHERE ROWNUM = 1", dr["MZ_ID"]);
                    temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (temp1.Rows.Count > 0)
                    {
                        merge_dr["EXAM_NAME"] = temp1.Rows[0]["EXAM_NAME"];
                    }

                    try
                    {
                        merge_dr["IMG"] = (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")) ? imageToByte("~/1-personnel/images/nopic.jpg") : imageToByte(o_DBFactory.ABC_toTest.vExecSQL("SELECT BBB.PICTUREPATH FROM (SELECT ROWNUM AS R , AAA.* FROM (SELECT MZ_ID,PICTUREPATH,BUDATE FROM A_PICPATH WHERE MZ_ID = '" + dr["MZ_ID"] + "' ORDER BY BUDATE DESC) AAA)BBB WHERE R = 1")));
                    }
                    catch
                    {
                    }

                    basicAll.Rows.Add(merge_dr);
                    num++;
                }

                return basicAll;


            }
            protected static string RANK1_NAME(string IDNO, int ROWNUM)
            {
                string strSQL = "SELECT * FROM (SELECT MZ_RANK1,ROWNUM AS num FROM (SELECT MZ_RANK1 FROM A_CAREER WHERE MZ_ID='" + IDNO + "' ORDER BY MZ_ADATE DESC)) WHERE num =" + ROWNUM + "";
                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                if (temp.Rows.Count == 1)
                {
                    return o_A_KTYPE.CODE_TO_NAME(temp.Rows[0][0].ToString(), "09");
                }
                else
                {
                    return "";
                }
            }

            public static byte[] imageToByte(string path)
            {
                string file_path = string.Empty;
                try
                {
                    file_path = HttpContext.Current.Server.MapPath(path);
                    System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fileOpen);
                    byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                    fileOpen.Close();

                    //XX2013/06/18 
                    fileOpen.Dispose();

                    return by;
                }
                catch
                {
                    file_path = HttpContext.Current.Server.MapPath("~/1-personnel/images/nopic.jpg");
                    System.IO.FileStream fileOpen = new FileStream(file_path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fileOpen);
                    byte[] by = br.ReadBytes(int.Parse(br.BaseStream.Length.ToString()));
                    fileOpen.Close();
                    //XX2013/06/18 
                    fileOpen.Dispose();

                    return by;
                }
            }


        }


    }
}
