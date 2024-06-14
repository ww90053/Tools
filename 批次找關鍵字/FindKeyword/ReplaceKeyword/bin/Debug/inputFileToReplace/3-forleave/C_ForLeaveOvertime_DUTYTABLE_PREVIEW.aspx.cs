using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.Util;

using TPPDDB.App_Code; 

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYTABLE_PREVIEW : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                C.check_power();
            }

            //錯誤
            if (Session["error"] != null)
            {
                // Response.Write(Session["error"].ToString());
                DataTable dt = new DataTable();
                dt.Columns.Add("MZ_CNO", typeof(string));
                dt.Columns.Add("MZ_NAME", typeof(string));
                dt.Columns.Add("BECAUSE", typeof(string));
                string havedot = Session["error"].ToString();
                havedot = havedot.Substring(0, havedot.Length - 1);
                string[] shd = havedot.Split(',');
                foreach (string s in shd)
                {
                    string[] snm = s.Split('：');
                    string cno = Service_C_DUTYPEOPLE.get_pno(Request["AD"], Request["UNIT"], o_A_DLBASE.name_ID(snm[0].Trim()), Request["DUTYDATE"]);
                    DataRow[] dr = dt.Select("MZ_CNO='" + cno + "'");
                    if (dr.Count() == 0)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["MZ_CNO"] = !string.IsNullOrEmpty(cno) ? cno.PadLeft(2, '0') : cno;
                        dr1["MZ_NAME"] = snm[0].Trim();
                        dr1["BECAUSE"] = snm[1].Trim() + ",";
                        dt.Rows.Add(dr1);
                    }
                    else
                    {
                        dr[0]["BECAUSE"] += snm[1].Trim() + ",";
                    }
                }
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }

            ViewState["MZ_AD"] = Request["AD"];
            ViewState["MZ_UNIT"] = Request["UNIT"];
            ViewState["DUTYDATE"] = Request["DUTYDATE"];
            if (!Page.IsPostBack)
            {
                Label1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + ViewState["MZ_AD"].ToString() + "' ")
                           + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + ViewState["MZ_UNIT"] + "'")
                           + "勤務分配表";

                string[] SS = (DateManange.strtodate(ViewState["DUTYDATE"].ToString())).Split('/');

                DateTime dateNow = new DateTime(int.Parse(SS[0]), int.Parse(SS[1]), int.Parse(SS[2]));

                string DayOfWeek = "";

                switch (dateNow.DayOfWeek.ToString())
                {
                    case "Sunday":
                        DayOfWeek = "星期日";
                        break;
                    case "Monday":
                        DayOfWeek = "星期一";
                        break;
                    case "Tuesday":
                        DayOfWeek = "星期二";
                        break;
                    case "Wednesday":
                        DayOfWeek = "星期三";
                        break;
                    case "Thursday":
                        DayOfWeek = "星期四";
                        break;
                    case "Friday":
                        DayOfWeek = "星期五";
                        break;
                    case "Saturday":
                        DayOfWeek = "星期六";
                        break;


                }
                Label2.Text = (int.Parse(SS[0]) - 1911).ToString() + "年" + SS[1] + "月" + SS[2] + "日" + DayOfWeek;

                string StartTime = o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT dbo.SUBSTR(C_DUTYTABLE.TIME1,2,1) as TIME1 FROM C_DUTYTABLE WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'");

                int count;

                if (StartTime == "6")
                {
                    count = 26;
                }
                else if (StartTime == "7")
                {
                    count = 25;
                }
                else
                {
                    count = 24;
                }

                string SelectString = "SELECT (SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=C_DUTYTABLE.DUTY_NO) 勤務項目," +
                                            " DUTYMODE_NO_MEMO 勤務方式,'2' as o1,'2' as o2,'2' as o3,'2' as o4,'2' as o5,'2' as o6,'2' as o7,'2' as o8,'2' as o9,'2' as o10,COMM_CHANNEL 通訊代號,";

                string[] time = new string[] { "06\n|\n07", "07\n|\n08", "08\n|\n09", "09\n|\n10", "10\n|\n11", "11\n|\n12", "12\n|\n13", "13\n|\n14", "14\n|\n15", "15\n|\n16", 
                                             "16\n|\n17","17\n|\n18","18\n|\n19","19\n|\n20","20\n|\n21","21\n|\n22","22\n|\n23","23\n|\n24","00\n|\n01","01\n|\n02","02\n|\n03",
                                            "03\n|\n04","04\n|\n05","05\n|\n06","06\n|\n07 ","07\n|\n08 ","09\n|\n10"};
                switch (count)
                {
                    case 26:
                        for (int i = 1; i <= count; i++)
                        {
                            if (i == count)
                            {
                                SelectString += "PEOPLE" + i.ToString() + " AS \"" + time[i - 1] + "\"";
                            }
                            else
                            {
                                SelectString += "PEOPLE" + i.ToString() + " AS \"" + time[i - 1] + "\",";
                            }

                        }
                        break;
                    case 25:
                        for (int i = 1; i <= count; i++)
                        {
                            if (i == count)
                            {
                                SelectString += "PEOPLE" + i.ToString() + " AS \"" + time[i] + "\"";
                            }
                            else
                            {
                                SelectString += "PEOPLE" + i.ToString() + " AS \"" + time[i] + "\",";
                            }

                        }
                        break;
                    case 24:
                        for (int i = 1; i <= count; i++)
                        {
                            if (i == count)
                            {
                                SelectString += "PEOPLE" + i.ToString() + " AS \"" + time[i + 1] + "\"";
                            }
                            else
                            {
                                SelectString += "PEOPLE" + i.ToString() + " AS \"" + time[i + 1] + "\",";
                            }

                        }
                        break;
                }

                SelectString += " FROM C_DUTYTABLE WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'";
                SelectString += "order by DUTY_NO asc";
                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

                //for (int i = 0; i < temp.Rows.Count; i++)
                //{
                //    for (int j = 1; j <= count; j++)
                //    {
                //        temp.Rows[i]["PEOPLE" + j.ToString()] = SQLSPLIT(temp.Rows[i]["PEOPLE" + j.ToString()].ToString());
                //    }
                //}

                foreach (DataRow dr in temp.Rows)
                {
                    foreach (DataColumn dc in temp.Columns)
                    {
                        if (dc.ColumnName.IndexOf('|') > -1)
                        {
                            dr[dc] = SQLSPLIT(dr[dc].ToString());
                        }
                    }
                }
                ViewState["temp_sql"] = SelectString;
                ViewState["temp"] = temp;

                temp.Columns.Remove("o1");
                temp.Columns.Remove("o2");
                temp.Columns.Remove("o3");
                temp.Columns.Remove("o4");
                temp.Columns.Remove("o5");
                temp.Columns.Remove("o6");
                temp.Columns.Remove("o7");
                temp.Columns.Remove("o8");
                temp.Columns.Remove("o9");
                temp.Columns.Remove("o10");

                GridView1.DataSource = temp;
                GridView1.DataBind();

                DataTable people = new DataTable();
                people.Columns.Add("TITLE", typeof(string));
                people.Columns.Add("PNO1", typeof(string));
                people.Columns.Add("NAME1", typeof(string));
                people.Columns.Add("PNO2", typeof(string));
                people.Columns.Add("NAME2", typeof(string));
                people.Columns.Add("PNO3", typeof(string));
                people.Columns.Add("NAME3", typeof(string));
                people.Columns.Add("PNO4", typeof(string));
                people.Columns.Add("NAME4", typeof(string));
                people.Columns.Add("PNO5", typeof(string));
                people.Columns.Add("NAME5", typeof(string));
                people.Columns.Add("PNO6", typeof(string));
                people.Columns.Add("NAME6", typeof(string));

                DataTable pd = new DataTable();
                pd.Columns.Add("PNO", typeof(string));
                pd.Columns.Add("NAME", typeof(string));
                //組勤區編號人員對照表
                SelectString = "SELECT COUNT(*) FROM A_DLBASE  WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "'";
                DataTable all_dt = new DataTable();
                int all_c = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(SelectString));
                int all_count = all_c % 6 == 0 ? all_c / 6 : (all_c / 6) + 1;

                for (int i = 1; i <= all_c; i++)
                {
                    DataRow dr = pd.NewRow();
                    dr["PNO"] = i.ToString().PadLeft(2, '0');
                    dr["NAME"] = "\n";
                    pd.Rows.Add(dr);
                }

                //勤區或輪番的設定
                string NO_KIND = o_DBFactory.ABC_toTest.vExecSQL("select NO_KIND from C_DUTYTABLE WHERE DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "' GROUP BY NO_KIND");

                DataTable dt = new DataTable();
                if (NO_KIND=="C")
                {
                    SelectString = "SELECT MZ_CNO,MZ_ID FROM C_DUTYPEOPLE  WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "' ORDER BY MZ_CNO";
                }
                else
                {
                    
                    SelectString = "SELECT MZ_PNO,MZ_ID FROM C_DUTYPEOPLE  WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "' ORDER BY MZ_PNO";
                }
                dt = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");
                foreach (DataRow dr in dt.Rows)
                {
                    if (NO_KIND == "C")
                    {
                        DataRow[] have_dr = pd.Select("PNO='" + dr["MZ_CNO"].ToString().PadLeft(2, '0') + "'");
                        if (have_dr.Count() > 0)
                        {
                            have_dr[0]["NAME"] = o_A_DLBASE.OCCC(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.CNAME(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.ID_MZPNO_1(dr["MZ_ID"].ToString(), ViewState["DUTYDATE"].ToString());
                        }
                        else
                        {
                            DataRow new_dr = pd.NewRow();
                            new_dr["PNO"] = dr["MZ_CNO"].ToString();
                            new_dr["NAME"] = o_A_DLBASE.OCCC(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.CNAME(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.ID_MZPNO_1(dr["MZ_ID"].ToString(), ViewState["DUTYDATE"].ToString()); 
                            pd.Rows.Add(new_dr);
                        }
                    }
                    else{
                        DataRow[] have_dr = pd.Select("PNO='" + dr["MZ_PNO"].ToString().PadLeft(2, '0') + "'");
                        if (have_dr.Count() > 0)
                        {
                            have_dr[0]["NAME"] = o_A_DLBASE.OCCC(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.CNAME(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.ID_MZPNO_1(dr["MZ_ID"].ToString(), ViewState["DUTYDATE"].ToString());
                        }
                        else
                        {
                            DataRow new_dr = pd.NewRow();
                            new_dr["PNO"] = dr["MZ_PNO"].ToString();
                            new_dr["NAME"] = o_A_DLBASE.OCCC(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.CNAME(dr["MZ_ID"].ToString()) + " " + o_A_DLBASE.ID_MZPNO_1(dr["MZ_ID"].ToString(), ViewState["DUTYDATE"].ToString());
                            pd.Rows.Add(new_dr);
                        }
                    }
                 
                }

                int out_count = pd.Rows.Count % 6 == 0 ? pd.Rows.Count / 6 : (pd.Rows.Count / 6) + 1;
                for (int i = 0; i < out_count; i++)
                {
                    if (i + 1 != out_count)
                    {
                        DataRow dr = people.NewRow();
                        for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
                        {
                            int columns = j % 6 == 0 ? 6 : j % 6;
                            dr["PNO" + columns.ToString()] = pd.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0');
                            dr["NAME" + columns.ToString()] = pd.Rows[j - 1]["NAME"].ToString();
                        }
                        people.Rows.Add(dr);
                    }
                    else
                    {
                        DataRow dr = people.NewRow();
                        for (int j = 6 * i + 1; j <= pd.Rows.Count; j++)
                        {
                            int columns = j % 6 == 0 ? 6 : j % 6;
                            dr["PNO" + columns.ToString()] = pd.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0');
                            dr["NAME" + columns.ToString()] = pd.Rows[j - 1]["NAME"].ToString();
                        }
                        people.Rows.Add(dr);
                    }
                }
                ViewState["pd_count"] = pd.Rows.Count;
                ViewState["people"] = pd;
                GridView3.DataSource = people;
                GridView3.DataBind();
            }
        }

        protected string re_day_star(string day)
        {
            switch (day)
            {
                case "Sunday":
                    return "星期日";
                case "Monday":
                    return "星期一";
                case "Tuesday":
                    return "星期二";
                case "Wednesday":
                    return "星期三";
                case "Thursday":
                    return "星期四";
                case "Friday":
                    return "星期五";
                case "Saturday":
                    return "星期六";
                default:
                    return "星期一";
            }
        }

        public Stream RenderDataTableToExcel(DataTable SourceTable, DataTable dt2)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet();
            sheet.IsPrintGridlines = true;
            sheet.DisplayGridlines = true;
            int column_count = SourceTable.Columns.Count;
            HSSFRow dataRow1 = sheet.CreateRow(0);
            dataRow1 = sheet.CreateRow(1);

            string dutydate;
            DateTime dutyd;
            //標題用style
            HSSFFont titlefont1 = workbook.CreateFont();
            titlefont1.FontHeightInPoints = 18;
            titlefont1.Boldweight = 2;
            titlefont1.FontName = "標楷體";
            HSSFFont titlefont2 = workbook.CreateFont();
            titlefont2.FontHeightInPoints = 12;
            titlefont2.FontName = "標楷體";
            HSSFFont titlefont3 = workbook.CreateFont();
            titlefont3.FontHeightInPoints = 12;
            titlefont3.FontName = "標楷體";
            HSSFCellStyle titlestyle1 = workbook.CreateCellStyle();
            titlestyle1.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            titlestyle1.Alignment = HSSFCellStyle.ALIGN_LEFT;
            titlestyle1.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            //titlestyle1.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            titlestyle1.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            titlestyle1.SetFont(titlefont1);
            HSSFCellStyle titlestyle2 = workbook.CreateCellStyle();
            titlestyle2.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            titlestyle2.Alignment = HSSFCellStyle.ALIGN_LEFT;
            //titlestyle2.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            //titlestyle2.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            //titlestyle2.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            titlestyle2.SetFont(titlefont2);
            HSSFCellStyle titlestyle3 = workbook.CreateCellStyle();
            titlestyle3.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            titlestyle3.Alignment = HSSFCellStyle.ALIGN_LEFT;
            //titlestyle3.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            //titlestyle3.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            //titlestyle3.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            titlestyle3.SetFont(titlefont3);

            //欄style
            HSSFFont cfont1 = workbook.CreateFont();
            cfont1.FontHeightInPoints = 12;
            cfont1.Boldweight = 2;
            cfont1.FontName = "標楷體";
            HSSFCellStyle cstyle1 = workbook.CreateCellStyle();
            cstyle1.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle1.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle1.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle1.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle1.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle1.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle1.WrapText = true;
            cstyle1.SetFont(cfont1);

 

            HSSFFont cfont2 = workbook.CreateFont();
            cfont2.FontHeightInPoints = 10;
            cfont2.FontName = "標楷體";
            HSSFCellStyle cstyle2 = workbook.CreateCellStyle();
            cstyle2.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle2.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle2.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle2.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle2.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle2.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle2.SetFont(cfont2);

            HSSFFont cfont3 = workbook.CreateFont();
            cfont3.FontHeightInPoints = 10;
            cfont3.FontName = "標楷體";
            HSSFCellStyle cstyle3 = workbook.CreateCellStyle();
            cstyle3.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle3.Alignment = HSSFCellStyle.ALIGN_LEFT;
            cstyle3.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle3.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle3.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle3.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle3.SetFont(cfont3);
            //

            HSSFFont cfont_t = workbook.CreateFont();
            cfont_t.FontHeightInPoints = 12;
            cfont_t.Boldweight = 2;
            cfont_t.FontName = "標楷體";
            HSSFCellStyle cstyle_t = workbook.CreateCellStyle();
            cstyle_t.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle_t.Alignment = HSSFCellStyle.ALIGN_LEFT;
            cstyle_t.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle_t.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle_t.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle_t.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle_t.WrapText = true;
            cstyle_t.SetFont(cfont1);
            //

            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 0, 1, 29));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 30, 0, 37));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 30, 1, 37));
            sheet.GetRow(0).CreateCell(0).SetCellValue(string.Format("{0}{1} {2}人勤務分配表", o_A_KTYPE.RAD(Request["AD"]), o_A_KTYPE.RUNIT(Request["UNIT"]), dt2.Rows.Count));
            dutydate = Request["DUTYDATE"];
            dutyd = new DateTime(int.Parse(dutydate.Substring(0, 3)) + 1911, int.Parse(dutydate.Substring(3, 2)), int.Parse(dutydate.Substring(5, 2)));

            sheet.GetRow(0).CreateCell(30).SetCellValue(string.Format("{0} 年 {1} 月 {2} 日 {3}", dutydate.Substring(0, 3), dutydate.Substring(3, 2), dutydate.Substring(5, 2), re_day_star(dutyd.DayOfWeek.ToString())));
            sheet.GetRow(1).CreateCell(30).SetCellValue("所長：");

            //style
            sheet.GetRow(0).GetCell(0).CellStyle = titlestyle1;
            sheet.GetRow(0).GetCell(30).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(30).CellStyle = titlestyle3;

            //起班時間
            string DutyStartTime = o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT dbo.SUBSTR(C_DUTYTABLE.TIME1,2,1) as TIME1 FROM C_DUTYTABLE WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'");


            //switch (column_count - 8)
            //{
            //    case 29:
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 0, 1, 28));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 29, 0, 36));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 29, 1, 36));
            //        sheet.GetRow(0).CreateCell(0).SetCellValue(string.Format("{0}{1} {2}人勤務分配表", o_A_KTYPE.RAD(Request["AD"]), o_A_KTYPE.RUNIT(Request["UNIT"]), dt2.Rows.Count));
            //        dutydate = Request["DUTYDATE"];
            //        dutyd = new DateTime(int.Parse(dutydate.Substring(0, 3)) + 1911, int.Parse(dutydate.Substring(3, 2)), int.Parse(dutydate.Substring(5, 2)));

            //        sheet.GetRow(0).CreateCell(29).SetCellValue(string.Format("{0} 年 {1} 月 {2} 日 {3}", dutydate.Substring(0, 3), dutydate.Substring(3, 2), dutydate.Substring(5, 2), re_day_star(dutyd.DayOfWeek.ToString())));
            //        sheet.GetRow(1).CreateCell(29).SetCellValue("所長：");

            //        //style
            //        sheet.GetRow(0).GetCell(0).CellStyle = titlestyle1;
            //        sheet.GetRow(0).GetCell(29).CellStyle = titlestyle2;
            //        sheet.GetRow(1).GetCell(29).CellStyle = titlestyle3;
            //        break;
            //    case 28:
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 0, 1, 29));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 30, 0, 37));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 30, 1, 37));
            //        sheet.GetRow(0).CreateCell(0).SetCellValue(string.Format("{0}{1} {2}人勤務分配表", o_A_KTYPE.RAD(Request["AD"]), o_A_KTYPE.RUNIT(Request["UNIT"]), ViewState["pd_count"]));
            //        dutydate = Request["DUTYDATE"];
            //        dutyd = new DateTime(int.Parse(dutydate.Substring(0, 3)) + 1911, int.Parse(dutydate.Substring(3, 2)), int.Parse(dutydate.Substring(5, 2)));

            //        sheet.GetRow(0).CreateCell(28).SetCellValue(string.Format("{0} 年 {1} 月 {2} 日 {3}", dutydate.Substring(0, 3), dutydate.Substring(3, 2), dutydate.Substring(5, 2), re_day_star(dutyd.DayOfWeek.ToString())));
            //        sheet.GetRow(1).CreateCell(28).SetCellValue("所長：");

            //        //style
            //        sheet.GetRow(0).GetCell(0).CellStyle = titlestyle1;
            //        sheet.GetRow(0).GetCell(28).CellStyle = titlestyle2;
            //        sheet.GetRow(1).GetCell(28).CellStyle = titlestyle3;
            //        break;
            //    case 27:
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 0, 1, 26));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 27, 0, 34));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 27, 1, 34));
            //        sheet.GetRow(0).CreateCell(0).SetCellValue(string.Format("{0}{1} {2}人勤務分配表", o_A_KTYPE.RAD(Request["AD"]), o_A_KTYPE.RUNIT(Request["UNIT"]), ViewState["pd_count"]));
            //        dutydate = Request["DUTYDATE"];
            //        dutyd = new DateTime(int.Parse(dutydate.Substring(0, 3)) + 1911, int.Parse(dutydate.Substring(3, 2)), int.Parse(dutydate.Substring(5, 2)));

            //        sheet.GetRow(0).CreateCell(27).SetCellValue(string.Format("{0} 年 {1} 月 {2} 日 {3}", dutydate.Substring(0, 3), dutydate.Substring(3, 2), dutydate.Substring(5, 2), re_day_star(dutyd.DayOfWeek.ToString())));
            //        sheet.GetRow(1).CreateCell(27).SetCellValue("所長：");

            //        //style
            //        sheet.GetRow(0).GetCell(0).CellStyle = titlestyle1;
            //        sheet.GetRow(0).GetCell(27).CellStyle = titlestyle2;
            //        sheet.GetRow(1).GetCell(27).CellStyle = titlestyle3;
            //        break;
            //}

            HSSFRow headerRow = sheet.CreateRow(2);

            sheet.SetColumnWidth(0, 6 * 256);
            // sheet.SetColumnWidth(1, 10 * 256);
            sheet.SetColumnWidth(10, 5 * 256);
            for (int i = 1; i < SourceTable.Columns.Count; i++)
            {
                if (i != 10)
                {
                    if (i < 10)
                    {
                        sheet.SetColumnWidth(i, 4 * 256);
                    }
                    else
                    {
                        sheet.SetColumnWidth(i, 4 * 256);
                    }
                }
            }

            HSSFCellStyle cellstyle = workbook.CreateCellStyle();
            //cellstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.WHITE.index;
            //cellstyle.FillPattern = HSSFCellStyle.SQUARES;
            //cellstyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;

            cellstyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cellstyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cellstyle.WrapText = true;
            cellstyle.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cellstyle.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cellstyle.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cellstyle.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            HSSFFont class_font = workbook.CreateFont();
            class_font.FontName = "標楷體";
            class_font.FontHeightInPoints = 10;
            cellstyle.SetFont(class_font);


            //drawing line
            HSSFCellStyle linestyle = workbook.CreateCellStyle();
            linestyle.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            linestyle.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            linestyle.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            linestyle.BorderLeft = HSSFBorderFormatting.BORDER_THIN;

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                headerRow.GetCell(column.Ordinal).CellStyle = cellstyle;
                if (column.ColumnName == "勤務項目" ||column.ColumnName == "勤務方式"|| column.ColumnName == "通訊代號")
                {
                    headerRow.GetCell(column.Ordinal).CellStyle = cstyle1;
                }
            }


            //合併資料欄位
            string ori_s = string.Empty;
            int ori_i = 3;

            int count=0;
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                if (i == 0)
                {
                    ori_s = SourceTable.Rows[i]["勤務項目"].ToString();
                    
                }
                else
                {
                    if (ori_s != SourceTable.Rows[i]["勤務項目"].ToString())
                    {
                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(ori_i, 0, count-1 + ori_i, 0));
                        ori_i = ori_i + count;
                        count=0;
                        ori_s = SourceTable.Rows[i]["勤務項目"].ToString();
                    }
                    if (i == SourceTable.Rows.Count-1)
                    {
                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(ori_i, 0, count + ori_i, 0));
                    }
                }
                count++;
            }
            //title 合

     

            if (DutyStartTime == "6")
            {

            }
            else if (DutyStartTime == "7")
            {
 
            }
            else
            {
        
            }


            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(2, 1, 2, 11));

            #region 勤務方式
            // handling value.
            int rowIndex = 3;

            foreach (DataRow row in SourceTable.Rows)
            {
                HSSFRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());

                    dataRow.GetCell(column.Ordinal).CellStyle = cellstyle;
                    if (column.ColumnName.Contains("勤務方式"))
                    {
                        dataRow.GetCell(column.Ordinal).CellStyle = cstyle_t;
                    }
                }
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, 1, rowIndex, 11));
                rowIndex++;
            }
            #endregion

            #region 勤務人員代號職稱姓名
            int start = rowIndex;
            int out_count = dt2.Rows.Count % 6 == 0 ? dt2.Rows.Count / 6 : (dt2.Rows.Count / 6) + 1;

            for (int i = 0; i < out_count; i++)
            {
                //if (i + 1 != out_count)
                //{
                    int r = 1;
                    int v = 2;
                    HSSFRow dataRow = sheet.CreateRow(rowIndex);
                    dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
                    for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
                    {
                        if (j <= dt2.Rows.Count)
                        {
                            dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
                            dataRow.GetCell(r).CellStyle = cstyle2;
                            dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
                            var a = dt2.Rows[j - 1]["NAME"].ToString();
                            dataRow.GetCell(v).CellStyle = cstyle3;
                            try
                            {
                                if (j % 6 == 0)
                                {
                                    dataRow.CreateCell(v + 1);
                                    dataRow.CreateCell(v + 2);
                                    dataRow.CreateCell(v + 3);
                                    dataRow.CreateCell(v + 4);
                                    dataRow.CreateCell(v + 5);
                                    dataRow.GetCell(v + 1).CellStyle = linestyle;
                                    dataRow.GetCell(v + 2).CellStyle = linestyle;
                                    dataRow.GetCell(v + 3).CellStyle = linestyle;
                                    dataRow.GetCell(v + 4).CellStyle = linestyle;
                                    dataRow.GetCell(v + 5).CellStyle = linestyle;
                                }
                                else
                                {
                                    dataRow.CreateCell(v + 1);
                                    dataRow.CreateCell(v + 2);
                                    dataRow.CreateCell(v + 3);
                                    dataRow.CreateCell(v + 4);
                                    dataRow.GetCell(v + 1).CellStyle = linestyle;
                                    dataRow.GetCell(v + 2).CellStyle = linestyle;
                                    dataRow.GetCell(v + 3).CellStyle = linestyle;
                                    dataRow.GetCell(v + 4).CellStyle = linestyle;
                                }

                            }
                            catch { }
                            r = r + 6;
                            if (j + 1 == 6 * i + 1 + 6)
                            {
                                if (j % 6 == 0)
                                {
                                    sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 5));
                                    v = v + 6;
                                    r = v - 1;
                                }
                                else
                                {
                                    sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
                                    v = v + 6;
                                    r = v - 1;
                                }

                            }
                            else
                            {
                                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
                                v = v + 6;
                                r = v - 1;
                            }
                        }

                    }
                //}
                //else
                //{
                //    int r = 1;
                //    int v = 2;
                //    HSSFRow dataRow = sheet.CreateRow(rowIndex);
                //    dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
                  
                //    for (int j = 6 * i + 1; j <= dt2.Rows.Count; j++)
                //    {
                //        dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
                //        dataRow.GetCell(r).CellStyle = cstyle2;
                //        dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
                //        var a = dt2.Rows[j - 1]["NAME"].ToString();
                //        dataRow.GetCell(v).CellStyle = cstyle3;
                //        try
                //        {
                //            dataRow.CreateCell(v + 1);
                //            dataRow.CreateCell(v + 2);
                //            dataRow.CreateCell(v + 3);
                //            dataRow.CreateCell(v + 4);
                //            //dataRow.CreateCell(v + 5);
                //            dataRow.GetCell(v + 1).CellStyle = linestyle;
                //            dataRow.GetCell(v + 2).CellStyle = linestyle;
                //            dataRow.GetCell(v + 3).CellStyle = linestyle;
                //            dataRow.GetCell(v + 4).CellStyle = linestyle;
                //            //dataRow.GetCell(v + 5).CellStyle = linestyle;
                //        }
                //        catch { }
                //        r = r + 6;
                //        if (j + 1 == dt2.Rows.Count)
                //        {
                //            v = v + 6;
                //            r = v - 1;
                //            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
                //        }
                //        else
                //        {
                //            v = v + 6;
                //            r = v - 1;
                //            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
                //        }
                //    }
                //}
                rowIndex++;
            }

            //switch (column_count - 8)
            //{
            //    case 29:
            //        for (int i = 0; i < out_count; i++)
            //        {
            //            if (i + 1 != out_count)
            //            {
            //                int r = 1;
            //                int v = 2;
            //                HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //                dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //                for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
            //                {
            //                    dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
            //                    dataRow.GetCell(r).CellStyle = cstyle2;
            //                    dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
            //                    dataRow.GetCell(v).CellStyle = cstyle3;
            //                    try
            //                    {
            //                        dataRow.CreateCell(v + 1);
            //                        dataRow.CreateCell(v + 2);
            //                        dataRow.CreateCell(v + 3);
            //                        dataRow.CreateCell(v + 4);
            //                        dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 4).CellStyle = linestyle;
            //                    }
            //                    catch { }
            //                    NPOI.HSSF.Util.Region region = new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4);
            //                    sheet.AddMergedRegion(region);
            //                    r = r + 6;
            //                    v = v + 6;
            //                }
            //            }
            //            else
            //            {
            //                int r = 1;
            //                int v = 2;
            //                HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //                dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //                for (int j = 6 * i + 1; j <= dt2.Rows.Count; j++)
            //                {
            //                    dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
            //                    dataRow.GetCell(r).CellStyle = cstyle2;
            //                    dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
            //                    dataRow.GetCell(v).CellStyle = cstyle3;
            //                    try
            //                    {
            //                        dataRow.CreateCell(v + 1);
            //                        dataRow.CreateCell(v + 2);
            //                        dataRow.CreateCell(v + 3);
            //                        dataRow.CreateCell(v + 4);
            //                        dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 4).CellStyle = linestyle;
            //                    }
            //                    catch { }
            //                    sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                    r = r + 6;
            //                    v = v + 6;
            //                }
            //            }
            //            rowIndex++;
            //        }
            //        break;
            //    case 28:
            //        for (int i = 0; i < out_count; i++)
            //        {
            //            if (i + 1 != out_count)
            //            {
            //                int r = 1;
            //                int v = 2;
            //                HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //                dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //                for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
            //                {
            //                    dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
            //                    dataRow.GetCell(r).CellStyle = cstyle2;
            //                    dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
            //                    dataRow.GetCell(v).CellStyle = cstyle3;
            //                    try
            //                    {
            //                        dataRow.CreateCell(v + 1);
            //                        dataRow.CreateCell(v + 2);
            //                        dataRow.CreateCell(v + 3);
            //                        dataRow.CreateCell(v + 4);
            //                        dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 4).CellStyle = linestyle;
            //                    }
            //                    catch { }
            //                    r = r + 6;
            //                    if (j + 1 == 6 * i + 1 + 6)
            //                    {
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 3));
            //                        v = v + 5;
            //                        r = v - 1;
            //                    }
            //                    else
            //                    {
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                        v = v + 6;
            //                        r = v - 1;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                int r = 1;
            //                int v = 2;
            //                HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //                dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //                for (int j = 6 * i + 1; j <= dt2.Rows.Count; j++)
            //                {
            //                    dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
            //                    dataRow.GetCell(r).CellStyle = cstyle2;
            //                    dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
            //                    dataRow.GetCell(v).CellStyle = cstyle3;
            //                    try
            //                    {
            //                        dataRow.CreateCell(v + 1);
            //                        dataRow.CreateCell(v + 2);
            //                        dataRow.CreateCell(v + 3);
            //                        dataRow.CreateCell(v + 4);
            //                        dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                        dataRow.GetCell(v + 4).CellStyle = linestyle;
            //                    }
            //                    catch { }

            //                    if (j + 1 == dt2.Rows.Count)
            //                    {
            //                        v = v + 6;
            //                        r = v - 1;
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                    }
            //                    else
            //                    {
            //                        v = v + 6;
            //                        r = v - 1;
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                    }
            //                }
            //            }
            //            rowIndex++;
            //        }
            //        break;
            //    case 27:
            //        for (int i = 0; i < out_count; i++)
            //        {
            //            if (i + 1 != out_count)
            //            {
            //                int r = 1;
            //                int v = 2;
            //                HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //                dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //                for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
            //                {
            //                    dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
            //                    dataRow.GetCell(r).CellStyle = cstyle2;
            //                    dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
            //                    dataRow.GetCell(v).CellStyle = cstyle3;
            //                    if (j + 1 == 6 * i + 1 + 6 || j == 6 * i + 1)
            //                    {
            //                        try
            //                        {
            //                            dataRow.CreateCell(v + 1);
            //                            dataRow.CreateCell(v + 2);
            //                            dataRow.CreateCell(v + 3);
            //                            dataRow.CreateCell(v + 4);
            //                            dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                        }
            //                        catch { }
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                        v = v + 6;
            //                        r = v - 1;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            dataRow.CreateCell(v + 1);
            //                            dataRow.CreateCell(v + 2);
            //                            dataRow.CreateCell(v + 3);
            //                            dataRow.CreateCell(v + 4);
            //                            dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 4).CellStyle = linestyle;
            //                        }
            //                        catch { }
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                        v = v + 6;
            //                        r = v - 1;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                int r = 1;
            //                int v = 2;
            //                HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //                dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //                for (int j = 6 * i + 1; j <= dt2.Rows.Count; j++)
            //                {
            //                    dataRow.CreateCell(r).SetCellValue(dt2.Rows[j - 1]["PNO"].ToString().PadLeft(2, '0'));
            //                    dataRow.GetCell(r).CellStyle = cstyle2;
            //                    dataRow.CreateCell(v).SetCellValue(dt2.Rows[j - 1]["NAME"].ToString());
            //                    dataRow.GetCell(v).CellStyle = cstyle3;
            //                    r = r + 6;
            //                    if (j + 1 == 6 * i + 1 + 6 || j == 6 * i + 1)
            //                    {
            //                        try
            //                        {
            //                            dataRow.CreateCell(v + 1);
            //                            dataRow.CreateCell(v + 2);
            //                            dataRow.CreateCell(v + 3);
            //                            dataRow.CreateCell(v + 4);
            //                            dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                        }
            //                        catch { }
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                        v = v + 6;
            //                        r = v - 1;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            dataRow.CreateCell(v + 1);
            //                            dataRow.CreateCell(v + 2);
            //                            dataRow.CreateCell(v + 3);
            //                            dataRow.CreateCell(v + 4);
            //                            dataRow.GetCell(v + 1).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 2).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 3).CellStyle = linestyle;
            //                            dataRow.GetCell(v + 4).CellStyle = linestyle;
            //                        }
            //                        catch { }
            //                        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, v, rowIndex, v + 4));
            //                        v = v + 6;
            //                        r = v - 1;
            //                    }
            //                }
            //            }
            //            rowIndex++;
            //        }
            //        break;
            //}
            #endregion

            int end = rowIndex;
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 0, end - 1, 0));
            sheet.GetRow(start).GetCell(0).CellStyle = cstyle1;

            HSSFFont cfont4 = workbook.CreateFont();
            cfont4.FontHeightInPoints = 10;
            cfont4.FontName = "標楷體";
            HSSFCellStyle cstyle4 = workbook.CreateCellStyle();
            cstyle4.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle4.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle4.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle4.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle4.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle4.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle4.SetFont(cfont4);
            cstyle4.WrapText = true;

            HSSFFont cfont5 = workbook.CreateFont();
            cfont5.FontHeightInPoints = 10;
            cfont5.FontName = "標楷體";
            HSSFCellStyle cstyle5 = workbook.CreateCellStyle();
            cstyle5.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle5.Alignment = HSSFCellStyle.ALIGN_LEFT;
            cstyle5.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle5.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle5.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle5.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle5.WrapText = true;
            cstyle5.SetFont(cfont5);

            HSSFFont cfont6 = workbook.CreateFont();
            cfont6.FontHeightInPoints = 12;
            cfont6.Boldweight = 2;
            cfont6.FontName = "標楷體";
            HSSFCellStyle cstyle6 = workbook.CreateCellStyle();
            cstyle6.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle6.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle6.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle6.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle6.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle6.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle6.WrapText = true;
            cstyle6.SetFont(cfont6);

            //請假
            sheet.CreateRow(rowIndex).HeightInPoints = 50;
            sheet.GetRow(rowIndex).CreateCell(0).SetCellValue("請假人員");
            sheet.GetRow(rowIndex).GetCell(0).CellStyle = cstyle6;
            //sheet.CreateRow(rowIndex);
            for (int j = 1; j < SourceTable.Columns.Count; j++)
            {
                sheet.GetRow(rowIndex).CreateCell(j);
                sheet.GetRow(rowIndex).GetCell(j).CellStyle = cstyle4;
                sheet.GetRow(rowIndex).GetCell(j).CellStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_TOP;
                sheet.GetRow(rowIndex).GetCell(j).CellStyle.Alignment = HSSFCellStyle.ALIGN_LEFT;
            }
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, 0, rowIndex, 0));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, 1, rowIndex, SourceTable.Columns.Count - 1));

            string strSQL = "SELECT MZ_CODE,MZ_CNAME FROM C_DLCODE ORDER BY MZ_CODE";
            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            strSQL = "SELECT MZ_PNO,MZ_ID FROM C_DUTYPEOPLE  WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "' ORDER BY MZ_PNO";
            DataTable people = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            string all_s = string.Empty;
            string have_c = string.Empty;
            string NO_KIND = o_DBFactory.ABC_toTest.vExecSQL("select NO_KIND from C_DUTYTABLE WHERE DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "' GROUP BY NO_KIND");
            foreach (DataRow dr in temp.Rows)
            {
                have_c = string.Empty;
                foreach (DataRow pd in people.Rows)
                {
                    strSQL = string.Format("SELECT MZ_ID FROM C_DLTB01 WHERE MZ_IDATE1 = '{0}' AND MZ_ID = '{1}' AND MZ_CODE='{2}'", Request["DUTYDATE"], pd["MZ_ID"].ToString(), dr["MZ_CODE"].ToString());
                    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (dt.Rows.Count > 0)
                    {
                        //have_c += pd["MZ_ID"] + ",";

                        if (NO_KIND == "C")
                        {
                            have_c += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNO FROM C_DUTYPEOPLE  WHERE MZ_ID='" + pd["MZ_ID"] + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'")+",";
                        }
                        else
                        {
                            have_c += o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNO FROM C_DUTYPEOPLE  WHERE MZ_ID='" + pd["MZ_ID"] + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'") + ",";
                        }

                    }
                }
                if (have_c.Length > 0)
                {
                    all_s += dr["MZ_CNAME"].ToString().Trim() + "：" + have_c.Substring(0, have_c.Length - 1) + "  ";
                }
            }
            sheet.GetRow(rowIndex).GetCell(1).SetCellValue(all_s);
            rowIndex++;



            #region 記事
            //記事
            start = rowIndex;
            sheet.CreateRow(rowIndex).HeightInPoints = 100;
            sheet.GetRow(rowIndex).CreateCell(0).SetCellValue("重要記事");
            sheet.GetRow(rowIndex).GetCell(0).CellStyle = cstyle6;
            for (int j = 1; j < SourceTable.Columns.Count; j++)
            {
                sheet.GetRow(start).CreateCell(j);
                sheet.GetRow(start).GetCell(j).CellStyle = cstyle5;
                sheet.GetRow(start).GetCell(j).CellStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_TOP;
                sheet.GetRow(start).GetCell(j).CellStyle.Alignment = HSSFCellStyle.ALIGN_LEFT;
            }

            strSQL = string.Format("SELECT MZ_MEMO FROM C_DUTYMEMO WHERE MZ_AD='{0}' AND MZ_UNIT='{1}' AND MZ_DATE='{2}'", Request["AD"], Request["UNIT"], Request["DUTYDATE"]);
            DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 1, start, SourceTable.Columns.Count - 1));
            if (dt1.Rows.Count > 0)
            {
                sheet.GetRow(start).GetCell(1).SetCellValue(dt1.Rows[0][0].ToString());
            }


            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 0, start, 0));
            //foreach (DataRow row in dt2.Rows)
            //{
            //    HSSFRow dataRow = sheet.CreateRow(rowIndex);
            //    dataRow.CreateCell(0).SetCellValue("勤務人員代號職稱姓名");
            //    foreach (DataColumn column in dt2.Columns)
            //    {
            //        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
            //    }

            //    rowIndex++;
            //}
            rowIndex++;
            #endregion 

            #region 勤務變更

            //start = rowIndex;
            //int index = start;

            //for (int j = index; j < index + 5; j++)
            //{
            //    sheet.CreateRow(j);

            //}
            
            //sheet.GetRow(rowIndex).CreateCell(0).SetCellValue("勤務變更");
            
            //for (int j = 1; j <37; j++)
            //{
            //    sheet.GetRow(start).CreateCell(j);
            //    sheet.GetRow(start).GetCell(j).CellStyle = cstyle5;
            //    sheet.GetRow(start).GetCell(j).CellStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_TOP;
            //    sheet.GetRow(start).GetCell(j).CellStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;

            //}
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex, 0, rowIndex+5, 0));
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 1, start, 6));
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 7, start, 13));
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 14, start, 20));
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 21, start, 31));
            //sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(start, 32, start, 37));
            //    sheet.GetRow(start).GetCell(1).SetCellValue("服勤人員");
            //    sheet.GetRow(start).GetCell(6).SetCellValue("原定勤務");
            //    sheet.GetRow(start).GetCell(13).SetCellValue("變更後勤務");
            //    sheet.GetRow(start).GetCell(20).SetCellValue("變更原因");
            //    sheet.GetRow(start).GetCell(31).SetCellValue("主管簽章");
     
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex + 1, 1, rowIndex + 5, 6));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex + 1, 7, rowIndex + 5, 13));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex + 1, 14, rowIndex + 5, 20));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex + 1, 21, rowIndex + 5, 31));
            //        sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(rowIndex + 1, 32, rowIndex + 5, 37));


            //        sheet.GetRow(rowIndex).GetCell(0).CellStyle = cstyle6;

          
            #endregion

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        protected string SQLSPLIT(string S)
        {
            string result = "";
            string[] SS = S.Split('.');

            for (int i = 0; i < SS.Length; i++)
            {
                string back_v = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNO FROM C_DUTYPEOPLE WHERE MZ_ID='" + SS[i].ToString() + "' AND MZ_AD='" + ViewState["MZ_AD"].ToString() + "'AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "'");
                if (i == SS.Length - 1)
                {
                    result += !string.IsNullOrEmpty(back_v) ? back_v.PadLeft(2, '0') : back_v;
                }
                else
                {
                    result += !string.IsNullOrEmpty(back_v) ? back_v.PadLeft(2, '0') + "\n" : back_v + "\n";
                }
            }

            return result;
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gv = (GridView)sender;
                e.Row.Cells.Clear();
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                string s = "SELECT AAA.* FROM (SELECT DISTINCT dbo.SUBSTR(C_DUTYTABLE.TIME1,2,1) AS TIME1 FROM C_DUTYTABLE WHERE MZ_AD='" + ViewState["MZ_AD"].ToString() + "' AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "' AND DUTYDATE='" + ViewState["DUTYDATE"].ToString() + "') AAA WHERE TIME1 IN (6,7,8,9) ORDER BY TIME1";
                string StartTime = o_DBFactory.ABC_toTest.vExecSQL(s);
                if (StartTime == "6")
                {
                    TableCell tc1 = new TableCell();
                    tc1.Text = "勤務項目";
                    gvRow.Cells.Add(tc1);

                    TableCell tc28 = new TableCell();
                    tc28.Text = "勤務方式";
                    gvRow.Cells.Add(tc28);

                    TableCell tc29 = new TableCell();
                    tc29.Text = "通訊代號";
                    gvRow.Cells.Add(tc29);

                    TableCell tc2 = new TableCell();
                    tc2.Text = "6";
                    gvRow.Cells.Add(tc2);

                    TableCell tc3 = new TableCell();
                    tc3.Text = "7";
                    gvRow.Cells.Add(tc3);

                    TableCell tc4 = new TableCell();
                    tc4.Text = "8";
                    gvRow.Cells.Add(tc4);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                }
                else if (StartTime == "7")
                {
                    TableCell tc1 = new TableCell();
                    tc1.Text = "勤務項目";
                    gvRow.Cells.Add(tc1);

                    TableCell tc2 = new TableCell();
                    tc2.Text = "勤務方式";
                    gvRow.Cells.Add(tc2);

                    TableCell tc28 = new TableCell();
                    tc28.Text = "通訊代號";
                    gvRow.Cells.Add(tc28);

                    TableCell tc3 = new TableCell();
                    tc3.Text = "7";
                    gvRow.Cells.Add(tc3);

                    TableCell tc4 = new TableCell();
                    tc4.Text = "8";
                    gvRow.Cells.Add(tc4);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);
                }
                else if (StartTime == "8")
                {
                    TableCell tc1 = new TableCell();
                    tc1.Text = "勤務項目";
                    gvRow.Cells.Add(tc1);

                    TableCell tc2 = new TableCell();
                    tc2.Text = "勤務方式";
                    gvRow.Cells.Add(tc2);

                    TableCell tc3 = new TableCell();
                    tc3.Text = "通訊代號";
                    gvRow.Cells.Add(tc3);

                    TableCell tc4 = new TableCell();
                    tc4.Text = "8";
                    gvRow.Cells.Add(tc4);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                    TableCell t28 = new TableCell();
                    t28.Text = "8";
                    gvRow.Cells.Add(t28);
                }
                else if (StartTime == "9")
                {
                    TableCell tc1 = new TableCell();
                    tc1.Text = "勤務項目";
                    gvRow.Cells.Add(tc1);

                    TableCell tc2 = new TableCell();
                    tc2.Text = "勤務方式";
                    gvRow.Cells.Add(tc2);

                    TableCell tc3 = new TableCell();
                    tc3.Text = "通訊代號";
                    gvRow.Cells.Add(tc3);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                    TableCell t28 = new TableCell();
                    t28.Text = "8";
                    gvRow.Cells.Add(t28);
                }
                gv.Controls[0].Controls.AddAt(0, gvRow);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                int count = drv.Row.ItemArray.Count();
                if (ViewState["SELECT"] != null)
                {
                    string pno = ViewState["SELECT"].ToString();

                    for (int i = 3; i < count; i++)
                    {
                        e.Row.Cells[i].Width = 17;
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                        //e.Row.Cells[i].Wrap = false;
                        //e.Row.Cells[i].Style.Add("word-break", "break-all");
                        string[] sps = new string[] { "\n" };
                        string[] sp_s = drv[i].ToString().Split(sps, StringSplitOptions.None);
                        foreach (string s in sp_s)
                        {
                            if (s.Length == pno.ToString().PadLeft(2, '0').Length)
                            {
                                if (s.IndexOf(pno.ToString().PadLeft(2, '0')) > -1)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 3; i < count; i++)
                    {
                        e.Row.Cells[i].Width = 17;
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                        //e.Row.Cells[i].Wrap = false;
                        //e.Row.Cells[i].Style.Add("word-break", "break-all");
                    }
                }
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                ViewState["SELECT"] = GridView2.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString();
                GridView1.DataSource = ViewState["temp"] as DataTable;
                GridView1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt2 = new DataTable();
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(ViewState["temp_sql"].ToString(), "GET");
            foreach (DataRow dr in temp.Rows)
            {
                foreach (DataColumn dc in temp.Columns)
                {
                    if (dc.ColumnName.IndexOf('|') > -1)
                    {
                        dr[dc] = SQLSPLIT(dr[dc].ToString());
                    }
                }
            }
            MemoryStream ms = RenderDataTableToExcel(temp, ViewState["people"] as DataTable) as MemoryStream;
            // 設定強制下載標頭。
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=Download.xls"));
            // 輸出檔案。
            Response.BinaryWrite(ms.ToArray());

            ms.Close();
            ms.Dispose();
        }
    }
}
