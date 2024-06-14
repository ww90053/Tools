using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NPOI.Util;
using NPOI.HSSF.UserModel;
using System.IO;


namespace TPPDDB._3_forleave
{
    public partial class C_noplaylistTotal_rpt : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);


                //C.fill_AD_POST(DropDownList_AD);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_AD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_AD);
                }
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
                TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

            }
        }


        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    break;
                case "C":
                case "E":
                    //DropDownList_AD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_AD.Enabled = false;
                    }
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            if (RadioButtonList2.SelectedValue == "0")
            {
                DataTable rpt_dt = new DataTable();

                rpt_dt.Columns.Add("UNIT", typeof(string));
                rpt_dt.Columns.Add("TOTAL_PAY", typeof(string));
                rpt_dt.Columns.Add("MEMO", typeof(string));

                string unitSQL = "SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_AD.SelectedValue + "'";

                DataTable unit_dt = new DataTable();

                unit_dt = o_DBFactory.ABC_toTest.Create_Table(unitSQL, "getunit");

                for (int i = 0; i < unit_dt.Rows.Count; i++)
                {
                    DataRow dr = rpt_dt.NewRow();

                    string unit_code = unit_dt.Rows[i]["MZ_UNIT"].ToString();

                    string unit = o_A_KTYPE.CODE_TO_NAME(unit_code, "25");

                    string strSQL = "";

                    if (RadioButtonList1.SelectedValue == "0")
                        strSQL = "SELECT {0} FROM C_NOPLAYPAY WHERE MZ_OCCC NOT IN('Z014','Z015','Z016') AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD='" + DropDownList_AD.SelectedValue + "'  AND MZ_UNIT='" + unit_code + "') {1}";
                    else
                        strSQL = "SELECT {0} FROM C_NOPLAYPAY WHERE MZ_OCCC IN('Z014','Z015','Z016') AND PAY_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' AND MZ_EXUNIT='" + unit_code + "' {1}";

                    //string count = o_DBFactory.ABC_toTest.vExecSQL(string.Format(strSQL, "COUNT(*)", ""));

                    string MZ_TOTAL_PAY = o_DBFactory.ABC_toTest.vExecSQL(string.Format(strSQL, "NVL(SUM(MZ_TOTAL_PAY),0)", " ORDER BY PAY_AD,MZ_EXUNIT"));

                    dr[0] = unit;
                    dr[1] = MZ_TOTAL_PAY;
                    dr[2] = "";

                    rpt_dt.Rows.Add(dr);
                }

                App_Code.ToExcel.Dt2Excel(rpt_dt, DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            else
            {
                DataTable rpt_dt = new DataTable();

                rpt_dt.Columns.Add("UNIT", typeof(string));
                rpt_dt.Columns.Add("TOTAL_PAY", typeof(string));
                rpt_dt.Columns.Add("MEMO", typeof(string));


                string strSQL = "";

                if (RadioButtonList3.SelectedValue == "2")
                {
                    DataRow dr = rpt_dt.NewRow();

                    strSQL = "SELECT {0} FROM C_NOPLAYPAY WHERE MZ_OCCC NOT IN('Z014','Z015','Z016') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD!=MZ_EXAD AND PAY_AD='" + DropDownList_AD.SelectedValue + "') AND PAY_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' {1}";

                    string MZ_TOTAL_PAY = o_DBFactory.ABC_toTest.vExecSQL(string.Format(strSQL, "NVL(SUM(MZ_TOTAL_PAY),0)", " ORDER BY PAY_AD"));

                    dr[0] = "支援其他機關";
                    dr[1] = MZ_TOTAL_PAY;
                    dr[2] = "";

                    rpt_dt.Rows.Add(dr);
                }
                else
                {
                    string unitSQL = "SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_AD.SelectedValue + "'";

                    DataTable unit_dt = new DataTable();

                    unit_dt = o_DBFactory.ABC_toTest.Create_Table(unitSQL, "getunit");

                    for (int i = 0; i < unit_dt.Rows.Count; i++)
                    {
                        DataRow dr = rpt_dt.NewRow();

                        string unit_code = unit_dt.Rows[i]["MZ_UNIT"].ToString();

                        string unit = o_A_KTYPE.CODE_TO_NAME(unit_code, "25");


                        if (RadioButtonList3.SelectedValue == "0")
                            strSQL = "SELECT {0} FROM C_NOPLAYPAY WHERE MZ_OCCC NOT IN('Z014','Z015','Z016') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD=MZ_EXAD AND PAY_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_EXUNIT='" + unit_code + "') AND PAY_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' {1}";
                        else if (RadioButtonList3.SelectedValue == "1")
                            strSQL = "SELECT {0} FROM C_NOPLAYPAY WHERE MZ_OCCC IN ('Z014','Z015','Z016') AND PAY_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' AND MZ_EXUNIT='" + unit_code + "' {1}";

                        string MZ_TOTAL_PAY = o_DBFactory.ABC_toTest.vExecSQL(string.Format(strSQL, "NVL(SUM(MZ_TOTAL_PAY),0)", " ORDER BY PAY_AD,MZ_EXUNIT"));

                        dr[0] = unit;
                        dr[1] = MZ_TOTAL_PAY;
                        dr[2] = "";

                        rpt_dt.Rows.Add(dr);
                    }
                }

                App_Code.ToExcel.Dt2Excel(rpt_dt, DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
        }
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_YEAR.Text = string.Empty;
        }

        public Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            string strSQL = string.Empty;
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet();

            HSSFFont titlefont2 = workbook.CreateFont();
            titlefont2.FontHeightInPoints = 12;
            titlefont2.FontName = "標楷體";
            //titlefont2.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;

            HSSFCellStyle titlestyle2 = workbook.CreateCellStyle();
            titlestyle2.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            titlestyle2.Alignment = HSSFCellStyle.ALIGN_CENTER;
            titlestyle2.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            titlestyle2.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            titlestyle2.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            titlestyle2.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            titlestyle2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.WHITE.index;
            titlestyle2.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
            titlestyle2.SetFont(titlefont2);

            HSSFFont titlefont3 = workbook.CreateFont();
            titlefont3.FontHeightInPoints = 12;
            titlefont3.FontName = "標楷體";
            titlefont3.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
            HSSFCellStyle titlestyle3 = workbook.CreateCellStyle();
            titlestyle3.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            titlestyle3.Alignment = HSSFCellStyle.ALIGN_LEFT;
            titlestyle3.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            titlestyle3.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            titlestyle3.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            titlestyle3.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            titlestyle3.WrapText = true;
            titlestyle3.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
            titlestyle3.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
            titlestyle3.SetFont(titlefont2);
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

            sheet.IsPrintGridlines = true;
            sheet.DisplayGridlines = true;
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 0, 0, 2));

            sheet.CreateRow(0).CreateCell(0).CellStyle = titlestyle2;
            sheet.CreateRow(1).CreateCell(0).CellStyle = titlestyle2;

            if (RadioButtonList1.SelectedValue == "0")
                sheet.GetRow(0).GetCell(0).SetCellValue(DropDownList_AD.SelectedItem.Text + " " + int.Parse(TextBox_MZ_YEAR.Text).ToString() + "年員警因公未休假改發加班費統計表");
            else if (RadioButtonList1.SelectedValue == "1")
                sheet.GetRow(0).GetCell(0).SetCellValue(DropDownList_AD.SelectedItem.Text + " " + int.Parse(TextBox_MZ_YEAR.Text).ToString() + "年駕駛工友因公未休假改發加班費統計表");
            else
                sheet.GetRow(0).GetCell(0).SetCellValue(DropDownList_AD.SelectedItem.Text + " " + int.Parse(TextBox_MZ_YEAR.Text).ToString() + "年支援他機關因公未休假改發加班費統計表");


            for (int i = 1; i < 3; i++)
            {
                sheet.CreateRow(0).CreateCell(i).CellStyle = titlestyle2;
            }

            sheet.CreateRow(1).CreateCell(0).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(0).SetCellValue("單位");

            sheet.CreateRow(1).CreateCell(1).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(1).SetCellValue("未休假改發加班費金額");

            sheet.CreateRow(1).CreateCell(2).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(2).SetCellValue("備考");

            int Total_all = 0;
            int row = 0;

            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                Total_all += int.Parse(SourceTable.Rows[i]["TOTAL_PAY"].ToString());

                row = 2 + i;

                sheet.CreateRow(row).CreateCell(0).CellStyle = cstyle1;
                sheet.GetRow(row).GetCell(0).SetCellValue(SourceTable.Rows[i]["UNIT"].ToString());


                sheet.CreateRow(row).CreateCell(1).CellStyle = cstyle1;
                sheet.GetRow(row).GetCell(1).SetCellValue(SourceTable.Rows[i]["TOTAL_PAY"].ToString());


                sheet.CreateRow(row).CreateCell(2).CellStyle = cstyle1;
                sheet.GetRow(row).GetCell(2).SetCellValue(SourceTable.Rows[i]["MEMO"].ToString());

            }

            sheet.CreateRow(row + 1).CreateCell(0).CellStyle = cstyle1;
            sheet.GetRow(row + 1).GetCell(0).SetCellValue("總計");

            sheet.CreateRow(row + 1).CreateCell(1).CellStyle = cstyle1;
            sheet.GetRow(row + 1).GetCell(1).SetCellValue(Total_all.ToString());

            sheet.CreateRow(row + 1).CreateCell(2).CellStyle = cstyle1;
            sheet.GetRow(row + 1).GetCell(2).SetCellValue("");

            sheet.SetColumnWidth(0, 18 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 30 * 256);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            //headerRow = null;
            workbook = null;

            return ms;
        }



        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList2.SelectedValue == "0")
            {
                RadioButtonList1.Visible = true;
                RadioButtonList3.Visible = false;
            }
            else
            {
                RadioButtonList3.Visible = true;
                RadioButtonList1.Visible = false;
            }
        }
    }
}
