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



namespace TPPDDB._1_personnel
{
    public partial class A_EffectiveList_rpt : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            int YEAR = int.Parse(TextBox_MZ_DATE1.Text);

            DataTable EXCEL_DT = new DataTable();

            EXCEL_DT.Columns.Add("編號", typeof(string));
            EXCEL_DT.Columns.Add("單位", typeof(string));
            EXCEL_DT.Columns.Add("職稱", typeof(string));
            EXCEL_DT.Columns.Add("姓名", typeof(string));
            EXCEL_DT.Columns.Add("身分證號", typeof(string));
            EXCEL_DT.Columns.Add("官職等", typeof(string));
            EXCEL_DT.Columns.Add("俸點", typeof(string));
            EXCEL_DT.Columns.Add("分數", typeof(string));
            EXCEL_DT.Columns.Add("等次", typeof(string));
            EXCEL_DT.Columns.Add("4001", typeof(string));
            EXCEL_DT.Columns.Add("4010", typeof(string));
            EXCEL_DT.Columns.Add("4100", typeof(string));
            EXCEL_DT.Columns.Add("5001", typeof(string));
            EXCEL_DT.Columns.Add("5010", typeof(string));
            EXCEL_DT.Columns.Add("5100", typeof(string));
            EXCEL_DT.Columns.Add("差勤記錄", typeof(string));
            EXCEL_DT.Columns.Add((YEAR - 3).ToString(), typeof(string));
            EXCEL_DT.Columns.Add((YEAR - 2).ToString(), typeof(string));
            EXCEL_DT.Columns.Add((YEAR - 1).ToString(), typeof(string));
            EXCEL_DT.Columns.Add("本局考績委員會議", typeof(string));
            EXCEL_DT.Columns.Add("備註", typeof(string));



            string strSQL = "SELECT AAA.*,ROWNUM AS NUM FROM (SELECT A_REV_BASE.*,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,'' as YEAR1,'' AS YEAR2,'' AS YEAR3 FROM A_REV_BASE WHERE MZ_YEAR='" + TextBox_MZ_DATE1.Text + "' AND MZ_AD='" + DropDownList_AD.SelectedValue + "' ";

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                if (DropDownList_AD.SelectedValue == "382130100C" || DropDownList_AD.SelectedValue == "382130200C" || DropDownList_AD.SelectedValue == "382130300C")
                {
                    strSQL += " AND MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'";
                }
                else
                {
                    strSQL += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
                }
            }

            if (!string.IsNullOrEmpty(DropDownList_MZ_SWT.SelectedValue))
            {
                strSQL += " AND MZ_SWT='" + DropDownList_MZ_SWT.SelectedValue + "'";
            }

            if (DropDownList_AD.SelectedValue == "382130100C" || DropDownList_AD.SelectedValue == "382130200C" || DropDownList_AD.SelectedValue == "382130300C")
            {
                strSQL += " ORDER BY MZ_AD,MZ_UNIT,TBDV,MZ_OCCC,MZ_NUM ) AAA";
            }
            else
            {
                strSQL += " ORDER BY MZ_AD,MZ_EXUNIT,TBDV,MZ_OCCC,MZ_NUM ) AAA";
            }


            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                if (DropDownList_AD.SelectedValue == "382130100C" || DropDownList_AD.SelectedValue == "382130200C" || DropDownList_AD.SelectedValue == "382130300C")
                {
                    tempDT.Rows[i]["MZ_UNIT"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["MZ_UNIT"].ToString(), "25");
                }
                else
                {
                    tempDT.Rows[i]["MZ_UNIT"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["MZ_EXUNIT"].ToString(), "25");
                }

                tempDT.Rows[i]["MZ_OCCC"] = o_A_DLBASE.EXTPOS_OR_OCCC(tempDT.Rows[i]["MZ_ID"].ToString(), tempDT.Rows[i]["MZ_OCCC"].ToString());
                tempDT.Rows[i]["MZ_SRANK"] = o_A_KTYPE.CODE_TO_NAME(tempDT.Rows[i]["MZ_SRANK"].ToString(), "09");
                tempDT.Rows[i]["YEAR1"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_GRADE FROM A_REV_BASE WHERE MZ_YEAR='" + (YEAR - 1).ToString().PadLeft(3, '0') + "' AND MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'");
                tempDT.Rows[i]["YEAR2"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_GRADE FROM A_REV_BASE WHERE MZ_YEAR='" + (YEAR - 2).ToString().PadLeft(3, '0') + "' AND MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'");
                tempDT.Rows[i]["YEAR3"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_GRADE FROM A_REV_BASE WHERE MZ_YEAR='" + (YEAR - 3).ToString().PadLeft(3, '0') + "' AND MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'");
            }

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                DataRow dr = EXCEL_DT.NewRow();

                dr["編號"] = int.Parse(TextBox_MZ_DATE1.Text).ToString() + tempDT.Rows[i]["NUM"].ToString().PadLeft(3, '0');
                dr["單位"] = tempDT.Rows[i]["MZ_UNIT"].ToString();
                dr["職稱"] = tempDT.Rows[i]["MZ_OCCC"].ToString();
                dr["姓名"] = tempDT.Rows[i]["MZ_NAME"].ToString();
                dr["身分證號"] = tempDT.Rows[i]["MZ_ID"].ToString();
                dr["官職等"] = tempDT.Rows[i]["MZ_SRANK"].ToString();
                dr["俸點"] = tempDT.Rows[i]["MZ_SPT"].ToString();
                dr["分數"] = tempDT.Rows[i]["MZ_NUM"].ToString();
                dr["等次"] = tempDT.Rows[i]["MZ_GRADE"].ToString();
                dr["4001"] = "嘉獎：" + tempDT.Rows[i]["MZ_P4001"].ToString() + "次";
                dr["4010"] = "記功：" + tempDT.Rows[i]["MZ_P4010"].ToString() + "次";
                dr["4100"] = "記大功：" + tempDT.Rows[i]["MZ_P4100"].ToString() + "次";
                dr["5001"] = "申誡：" + tempDT.Rows[i]["MZ_P5001"].ToString() + "次";
                dr["5010"] = "記過：" + tempDT.Rows[i]["MZ_P5010"].ToString() + "次";
                dr["5100"] = "記大過：" + tempDT.Rows[i]["MZ_P5100"].ToString() + "次";
                dr["差勤記錄"] = "事假：" + tempDT.Rows[i]["MZ_CODE01"].ToString() + "日 病假：" + tempDT.Rows[i]["MZ_CODE02"].ToString() + "日";
                dr[(YEAR - 3).ToString()] = tempDT.Rows[i]["YEAR1"].ToString();
                dr[(YEAR - 2).ToString()] = tempDT.Rows[i]["YEAR2"].ToString();
                dr[(YEAR - 1).ToString()] = tempDT.Rows[i]["YEAR3"].ToString();
                dr["本局考績委員會議"] = "";
                dr["備註"] = tempDT.Rows[i]["MZ_MEMO"].ToString();

                EXCEL_DT.Rows.Add(dr);
            }

            //20140123
            App_Code.ToExcel.Dt2Excel(EXCEL_DT, DateTime.Now.ToString("yyyyMMddHHmmss"));

            
            //    Session["rpt_dt"] = tempDT;

            //    Session["TITLE"] = string.Format("{0}{1}{2}年度警察人員年終（另予）考績（成）清冊", DropDownList_AD.SelectedItem.Text, DropDownList_UNIT.SelectedItem.Text, int.Parse(TextBox_MZ_DATE1.Text));

            //    string tmp_url = "A_rpt.aspx?fn=EffectiveList&TPM_FION=" + TPM_FION;

            //    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
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



            HSSFCellStyle cstyle2 = workbook.CreateCellStyle();
            cstyle2.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle2.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle2.BorderRight = HSSFBorderFormatting.BORDER_NONE;
            cstyle2.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle2.BorderBottom = HSSFBorderFormatting.BORDER_NONE;
            cstyle2.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle2.WrapText = true;
            cstyle2.SetFont(cfont1);

            HSSFCellStyle cstyle3 = workbook.CreateCellStyle();
            cstyle3.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle3.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle3.BorderRight = HSSFBorderFormatting.BORDER_NONE;
            cstyle3.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle3.BorderBottom = HSSFBorderFormatting.BORDER_NONE;
            cstyle3.BorderLeft = HSSFBorderFormatting.BORDER_NONE;
            cstyle3.WrapText = true;
            cstyle3.SetFont(cfont1);

            HSSFCellStyle cstyle4 = workbook.CreateCellStyle();
            cstyle4.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle4.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle4.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle4.BorderTop = HSSFBorderFormatting.BORDER_THIN;
            cstyle4.BorderBottom = HSSFBorderFormatting.BORDER_NONE;
            cstyle4.BorderLeft = HSSFBorderFormatting.BORDER_NONE;
            cstyle4.WrapText = true;
            cstyle4.SetFont(cfont1);

            HSSFCellStyle cstyle5 = workbook.CreateCellStyle();
            cstyle5.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle5.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle5.BorderRight = HSSFBorderFormatting.BORDER_NONE;
            cstyle5.BorderTop = HSSFBorderFormatting.BORDER_NONE;
            cstyle5.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle5.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
            cstyle5.WrapText = true;
            cstyle5.SetFont(cfont1);

            HSSFCellStyle cstyle6 = workbook.CreateCellStyle();
            cstyle6.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle6.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle6.BorderRight = HSSFBorderFormatting.BORDER_NONE;
            cstyle6.BorderTop = HSSFBorderFormatting.BORDER_NONE;
            cstyle6.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle6.BorderLeft = HSSFBorderFormatting.BORDER_NONE;
            cstyle6.WrapText = true;
            cstyle6.SetFont(cfont1);

            HSSFCellStyle cstyle7 = workbook.CreateCellStyle();
            cstyle7.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            cstyle7.Alignment = HSSFCellStyle.ALIGN_CENTER;
            cstyle7.BorderRight = HSSFBorderFormatting.BORDER_THIN;
            cstyle7.BorderTop = HSSFBorderFormatting.BORDER_NONE;
            cstyle7.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
            cstyle7.BorderLeft = HSSFBorderFormatting.BORDER_NONE;
            cstyle7.WrapText = true;
            cstyle7.SetFont(cfont1);

            sheet.IsPrintGridlines = true;
            sheet.DisplayGridlines = true;
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(0, 0, 0, 16));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 0, 2, 0));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 1, 2, 1));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 2, 2, 2));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 4, 2, 4));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 5, 2, 5));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 6, 2, 6));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 7, 2, 7));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 8, 2, 10));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 11, 2, 11));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 12, 1, 14));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 15, 2, 15));
            sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(1, 16, 2, 16));


            sheet.CreateRow(0).CreateCell(0).CellStyle = titlestyle2;
            sheet.CreateRow(1).CreateCell(0).CellStyle = titlestyle2;
            sheet.GetRow(0).GetCell(0).SetCellValue(DropDownList_AD.SelectedItem.Text + DropDownList_UNIT.SelectedItem.Text + " " + int.Parse(TextBox_MZ_DATE1.Text).ToString() + "年考績清冊");

            for (int i = 1; i < 17; i++)
            {
                sheet.CreateRow(0).CreateCell(i).CellStyle = titlestyle2;
            }

            sheet.CreateRow(1).CreateCell(0).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(0).SetCellValue("編號");
            sheet.CreateRow(2).CreateCell(0).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(1).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(1).SetCellValue("單位");
            sheet.CreateRow(2).CreateCell(1).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(2).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(2).SetCellValue("職稱");
            sheet.CreateRow(2).CreateCell(2).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(3).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(3).SetCellValue("姓名");
            sheet.CreateRow(2).CreateCell(3).CellStyle = titlestyle2;
            sheet.GetRow(2).GetCell(3).SetCellValue("身分證號");

            sheet.CreateRow(1).CreateCell(4).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(4).SetCellValue("官職等");
            sheet.CreateRow(2).CreateCell(4).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(5).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(5).SetCellValue("俸點");
            sheet.CreateRow(2).CreateCell(5).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(6).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(6).SetCellValue("分數");
            sheet.CreateRow(2).CreateCell(6).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(7).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(7).SetCellValue("等次");
            sheet.CreateRow(2).CreateCell(7).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(8).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(8).SetCellValue("獎懲記錄");
            sheet.CreateRow(2).CreateCell(8).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(11).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(11).SetCellValue("差勤記錄");
            sheet.CreateRow(2).CreateCell(11).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(12).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(12).SetCellValue("最近三年考績");
            sheet.CreateRow(1).CreateCell(13).CellStyle = titlestyle2;
            sheet.CreateRow(1).CreateCell(14).CellStyle = titlestyle2;

            sheet.CreateRow(2).CreateCell(12).CellStyle = titlestyle2;
            sheet.GetRow(2).GetCell(12).SetCellValue((int.Parse(TextBox_MZ_DATE1.Text) - 3).ToString());

            sheet.CreateRow(2).CreateCell(13).CellStyle = titlestyle2;
            sheet.GetRow(2).GetCell(13).SetCellValue((int.Parse(TextBox_MZ_DATE1.Text) - 2).ToString());

            sheet.CreateRow(2).CreateCell(14).CellStyle = titlestyle2;
            sheet.GetRow(2).GetCell(14).SetCellValue((int.Parse(TextBox_MZ_DATE1.Text) - 1).ToString());

            sheet.CreateRow(1).CreateCell(15).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(15).SetCellValue("本局考績委員會議");
            sheet.CreateRow(2).CreateCell(15).CellStyle = titlestyle2;

            sheet.CreateRow(1).CreateCell(16).CellStyle = titlestyle2;
            sheet.GetRow(1).GetCell(16).SetCellValue("備註");
            sheet.CreateRow(2).CreateCell(16).CellStyle = titlestyle2;

            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                int up = 3 + i * 2;
                int down = 4 + i * 2;

                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 0, down, 0));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 1, down, 1));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 2, down, 2));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 4, down, 4));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 5, down, 5));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 6, down, 6));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 7, down, 7));
                // sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 8, down, 8));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 11, down, 11));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 12, down, 12));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 13, down, 13));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 14, down, 14));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 15, down, 15));
                sheet.AddMergedRegion(new NPOI.HSSF.Util.Region(up, 16, down, 16));

                sheet.CreateRow(up).CreateCell(0).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(0).SetCellValue(SourceTable.Rows[i]["編號"].ToString());
                sheet.CreateRow(down).CreateCell(0).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(1).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(1).SetCellValue(SourceTable.Rows[i]["單位"].ToString());
                sheet.CreateRow(down).CreateCell(1).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(2).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(2).SetCellValue(SourceTable.Rows[i]["職稱"].ToString());
                sheet.CreateRow(down).CreateCell(2).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(3).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(3).SetCellValue(SourceTable.Rows[i]["姓名"].ToString());
                sheet.CreateRow(down).CreateCell(3).CellStyle = cstyle1;
                sheet.GetRow(down).GetCell(3).SetCellValue(SourceTable.Rows[i]["身分證號"].ToString());

                sheet.CreateRow(up).CreateCell(4).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(4).SetCellValue(SourceTable.Rows[i]["官職等"].ToString());
                sheet.CreateRow(down).CreateCell(4).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(5).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(5).SetCellValue(SourceTable.Rows[i]["俸點"].ToString());
                sheet.CreateRow(down).CreateCell(5).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(6).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(6).SetCellValue(SourceTable.Rows[i]["分數"].ToString());
                sheet.CreateRow(down).CreateCell(6).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(7).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(7).SetCellValue(SourceTable.Rows[i]["等次"].ToString());
                sheet.CreateRow(down).CreateCell(7).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(8).CellStyle = cstyle2;
                sheet.GetRow(up).GetCell(8).SetCellValue(SourceTable.Rows[i]["4001"].ToString());
                sheet.CreateRow(down).CreateCell(8).CellStyle = cstyle5;
                sheet.GetRow(down).GetCell(8).SetCellValue(SourceTable.Rows[i]["5001"].ToString());

                sheet.CreateRow(up).CreateCell(9).CellStyle = cstyle3;
                sheet.GetRow(up).GetCell(9).SetCellValue(SourceTable.Rows[i]["4010"].ToString());
                sheet.CreateRow(down).CreateCell(9).CellStyle = cstyle6;
                sheet.GetRow(down).GetCell(9).SetCellValue(SourceTable.Rows[i]["5010"].ToString());

                sheet.CreateRow(up).CreateCell(10).CellStyle = cstyle4;
                sheet.GetRow(up).GetCell(10).SetCellValue(SourceTable.Rows[i]["4100"].ToString());
                sheet.CreateRow(down).CreateCell(10).CellStyle = cstyle7;
                sheet.GetRow(down).GetCell(10).SetCellValue(SourceTable.Rows[i]["5100"].ToString());

                sheet.CreateRow(up).CreateCell(11).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(11).SetCellValue(SourceTable.Rows[i]["差勤記錄"].ToString());
                sheet.CreateRow(down).CreateCell(11).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(12).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(12).SetCellValue(SourceTable.Rows[i][(int.Parse(TextBox_MZ_DATE1.Text) - 3).ToString()].ToString());
                sheet.CreateRow(down).CreateCell(12).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(13).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(13).SetCellValue(SourceTable.Rows[i][(int.Parse(TextBox_MZ_DATE1.Text) - 2).ToString()].ToString());
                sheet.CreateRow(down).CreateCell(13).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(14).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(14).SetCellValue(SourceTable.Rows[i][(int.Parse(TextBox_MZ_DATE1.Text) - 1).ToString()].ToString());
                sheet.CreateRow(down).CreateCell(14).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(15).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(15).SetCellValue(SourceTable.Rows[i]["本局考績委員會議"].ToString());
                sheet.CreateRow(down).CreateCell(15).CellStyle = cstyle1;

                sheet.CreateRow(up).CreateCell(16).CellStyle = cstyle1;
                sheet.GetRow(up).GetCell(16).SetCellValue(SourceTable.Rows[i]["備註"].ToString());
                sheet.CreateRow(down).CreateCell(16).CellStyle = cstyle1;

            }

            sheet.SetColumnWidth(0, 10 * 256);
            sheet.SetColumnWidth(1, 18 * 256);
            sheet.SetColumnWidth(2, 15 * 256);
            sheet.SetColumnWidth(3, 15 * 256);
            sheet.SetColumnWidth(4, 15 * 256);
            sheet.SetColumnWidth(5, 7 * 256);
            sheet.SetColumnWidth(6, 7 * 256);
            sheet.SetColumnWidth(7, 7 * 256);
            sheet.SetColumnWidth(8, 17 * 256);
            sheet.SetColumnWidth(9, 17 * 256);
            sheet.SetColumnWidth(10, 17 * 256);
            sheet.SetColumnWidth(11, 30 * 256);
            sheet.SetColumnWidth(12, 7 * 256);
            sheet.SetColumnWidth(13, 7 * 256);
            sheet.SetColumnWidth(14, 7 * 256);
            sheet.SetColumnWidth(15, 15 * 256);
            sheet.SetColumnWidth(16, 30 * 256);



            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            //headerRow = null;
            workbook = null;

            return ms;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            DropDownList_UNIT.SelectedValue = string.Empty;
            TextBox_MZ_DATE1.Text = string.Empty;
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem(" ", ""));
        }

        
    }
}
