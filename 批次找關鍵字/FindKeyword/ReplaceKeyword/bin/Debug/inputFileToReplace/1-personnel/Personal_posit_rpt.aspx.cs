using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace TPPDDB._1_personnel
{
    public partial class Personal_posit_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();

            A.set_Panel_EnterToTAB(ref this.Panel1);
          
                DropDownList_MZ_PRID.DataBind();
                DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
            }
        }

        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":

                    break;
                case "C":
                    DropDownList_MZ_PRID.Enabled = false;
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }
        }

        private string NOW(DataTable dt, int row)
        {
            string result = "";

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOAD"].ToString()))
            {
                result += dt.Rows[row]["MZ_EXOAD_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXOAD"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOUNIT"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXOUNIT_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXOUNIT"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOPNO"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXOPNO"].ToString() + "(" + dt.Rows[row]["MZ_EXOPNO1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOOCC"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXOOCC_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXOOCC"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXORANK"].ToString()) && !string.IsNullOrEmpty(dt.Rows[row]["MZ_EXRANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXORANK_NAME"].ToString() + "至" + dt.Rows[row]["MZ_EXRANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXORANK"].ToString() + "-" + dt.Rows[row]["MZ_EXRANK1"].ToString() + ")";
            }
            else if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXRANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXRANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXRANK1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_POSIND"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_POSIND"].ToString() + "'";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_CHISI"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_CHISI_NAME"].ToString() + "(" + dt.Rows[row]["MZ_CHISI"].ToString() + ")";
            }

            result += "。";

            result = o_CommonService.d_report_break_line(result, 60, "&N");

            return result;
        }

        private string AFTER(DataTable dt, int row)
        {
            string result = "";

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_AD"].ToString()))
            {
                result += dt.Rows[row]["MZ_AD_NAME"].ToString() + "(" + dt.Rows[row]["MZ_AD"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_UNIT"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_UNIT_NAME"].ToString() + "(" + dt.Rows[row]["MZ_UNIT"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_PNO"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_PNO"].ToString() + "(" + dt.Rows[row]["MZ_PNO1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_OCCC"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_OCCC_NAME"].ToString() + "(" + dt.Rows[row]["MZ_OCCC"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_RANK"].ToString()) && !string.IsNullOrEmpty(dt.Rows[row]["MZ_RANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_RANK_NAME"].ToString() + "至" + dt.Rows[row]["MZ_RANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_RANK"].ToString() + "-" + dt.Rows[row]["MZ_RANK1"].ToString() + ")";
            }
            else if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_RANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_RANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_RANK1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_POSIND"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_POSIND"].ToString() + "'";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_CHISI"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_CHISI_NAME"].ToString() + "(" + dt.Rows[row]["MZ_CHISI"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_SRANK"].ToString()))
            {
                result += "，暫支" + dt.Rows[row]["MZ_SRANK_NAME"].ToString() + dt.Rows[row]["MZ_SLVC_NAME"].ToString() + dt.Rows[row]["MZ_SPT"].ToString() + "元";
            }

            result += "。";

            result = o_CommonService.d_report_break_line(result, 60, "&N");

            return result;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
//            @"SELECT MZ_ID,MZ_NAME,MZ_PRID,MZ_PRID1,MZ_EXOPOS,
//
//                            MZ_NREA,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='11' AND MZ_KCODE=MZ_NREA) AS MZ_NREA_NAME,
//                            MZ_EXOAD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_EXOAD) AS MZ_EXOAD_NAME,
//                            MZ_EXOUNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=MZ_EXOUNIT) AS MZ_EXOUNIT_NAME,
//                            MZ_EXOOCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=MZ_EXOOCC) AS MZ_EXOOCC_NAME,
//                            MZ_EXORANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_EXORANK) AS MZ_EXORANK_NAME,
//                            MZ_EXRANK1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_EXRANK1) AS MZ_EXRANK1_NAME,
//                            MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1,
//                            MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_AD) AS MZ_AD_NAME,
//                            MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=MZ_EXOUNIT) AS MZ_UNIT_NAME,
//                            MZ_OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=MZ_OCCC) AS MZ_OCCC_NAME,
//                            MZ_RANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK) AS MZ_RANK_NAME,
//                            MZ_RANK1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK1) AS MZ_RANK1_NAME,
//                            MZ_SRANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK1) AS MZ_SRANK_NAME,
//                            MZ_CHISI,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='23' AND MZ_KCODE=MZ_CHISI) AS MZ_CHISI_NAME, 
//                            MZ_SLVC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='64' AND MZ_KCODE=MZ_SLVC) AS MZ_SLVC_NAME, 
//                            (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE,
//                            (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE,
//                            (Select SPEED_NO From A_CHKAD Where MZ_PRID=MZ_PRID and MZ_AD=MZ_CHKAD) AS SPEED_NO, 
//                            (Select PWD_NO From A_CHKAD Where MZ_PRID=MZ_PRID and MZ_AD=MZ_CHKAD) AS PWD_NO, 
//                            dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV  
//                            FROM A_POSIT2 
//
//
//                            WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";


            string strSQL = @"SELECT MZ_ID,MZ_NAME,A_POSIT2.MZ_PRID,A_POSIT2.MZ_PRID1,MZ_EXOPOS,
                             MZ_NREA,  AKNR.MZ_KCHI  MZ_NREA_NAME,  MZ_EXOAD, AKEXOD.MZ_KCHI   MZ_EXOAD_NAME, 
                             MZ_EXOUNIT, AKEXOU.MZ_KCHI   MZ_EXOUNIT_NAME  ,   MZ_EXOOCC,  AKEXOO.MZ_KCHI  MZ_EXOOCC_NAME ,                     
                             MZ_EXORANK,  AKEXOR.MZ_KCHI    MZ_EXORANK_NAME,   MZ_EXRANK1,   AKEXOR1.MZ_KCHI    MZ_EXORANK1_NAME, 
                            
                             MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1,

                             A_POSIT2.MZ_AD, AKD.MZ_KCHI      MZ_AD_NAME ,  MZ_UNIT,  AKU.MZ_KCHI   MZ_UNIT_NAME   , 
                             MZ_OCCC,  AKO.MZ_KCHI   MZ_OCCC_NAME  ,  MZ_RANK, AKR.MZ_KCHI   MZ_RANK_NAME  ,  
                             MZ_RANK1,  AKR1.MZ_KCHI   MZ_RANK1_NAME ,MZ_SRANK,AKSR.MZ_KCHI  MZ_SRANK_NAME, 
                             MZ_CHISI, AKCH.MZ_KCHI  MZ_CHISI_NAME,  MZ_SLVC,   AKSL.MZ_KCHI   MZ_SLVC_NAME ,  
                             (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE,
                             (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE,
                             ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO       ,
                             dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV  
                             FROM A_POSIT2 
                             LEFT JOIN A_KTYPE AKNR ON RTRIM(AKNR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_NREA) AND RTRIM(AKNR.MZ_KTYPE)='11' 
                             LEFT JOIN A_KTYPE AKEXOD ON RTRIM(AKEXOD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOAD) AND RTRIM(AKEXOD.MZ_KTYPE)='04' 
                             LEFT JOIN A_KTYPE AKEXOU ON RTRIM(AKEXOU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOUNIT) AND RTRIM(AKEXOU.MZ_KTYPE)='25' 
                             LEFT JOIN A_KTYPE AKEXOO ON RTRIM(AKEXOO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOOCC) AND RTRIM(AKEXOO.MZ_KTYPE)='26' 
                             LEFT JOIN A_KTYPE AKEXOR ON RTRIM(AKEXOR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXORANK) AND RTRIM(AKEXOR.MZ_KTYPE)='09' 
                             LEFT JOIN A_KTYPE AKEXOR1 ON RTRIM(AKEXOR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXRANK1) AND RTRIM(AKEXOR1.MZ_KTYPE)='09' 

                             LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                             LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                             LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                             LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                             LEFT JOIN A_KTYPE AKR1 ON RTRIM(AKR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKR1.MZ_KTYPE)='09' 
                             LEFT JOIN A_KTYPE AKSR ON RTRIM(AKSR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKSR.MZ_KTYPE)='09' 
                             LEFT JOIN A_KTYPE AKCH ON RTRIM(AKCH.MZ_KCODE)=RTRIM(A_POSIT2.MZ_CHISI) AND RTRIM(AKCH.MZ_KTYPE)='23' 
                             LEFT JOIN A_KTYPE AKSL ON RTRIM(AKSL.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SLVC) AND RTRIM(AKSL.MZ_KTYPE)='64' 

                             LEFT JOIN A_CHKAD ACK ON  ACK.MZ_PRID=A_POSIT2.MZ_PRID and ACK.MZ_AD=A_POSIT2.MZ_CHKAD


                            WHERE A_POSIT2.MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND A_POSIT2.MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";

            strSQL += " ORDER BY TBDV,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_ID";

            string CHKAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT  MZ_CHKAD FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "' AND ROWNUM=1");

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            DataTable rpt = new DataTable();

            rpt.Columns.Add("TOJ", typeof(string));
            rpt.Columns.Add("MZ_ID", typeof(string));
            rpt.Columns.Add("MZ_NAME", typeof(string));
            rpt.Columns.Add("MZ_PRID", typeof(string));
            rpt.Columns.Add("MZ_PRID1", typeof(string));
            rpt.Columns.Add("MZ_NREA", typeof(string));
            rpt.Columns.Add("MZ_NREA_NAME", typeof(string));
            rpt.Columns.Add("OTH_THING", typeof(string));
            rpt.Columns.Add("MZ_REMARK", typeof(string));
            rpt.Columns.Add("CONDITION", typeof(string));
            rpt.Columns.Add("MZ_POSIND", typeof(string));
            rpt.Columns.Add("MZ_DATE", typeof(string));
            rpt.Columns.Add("MZ_IDATE", typeof(string));
            rpt.Columns.Add("SPEED_NO", typeof(string));
            rpt.Columns.Add("PWD_NO", typeof(string));
            rpt.Columns.Add("PRID3", typeof(string));
            rpt.Columns.Add("PRINTGROUP", typeof(string));
            rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
            rpt.Columns.Add("NOW", typeof(string));
            rpt.Columns.Add("AFTER", typeof(string));
            rpt.Columns.Add("PAGECOUNT", typeof(string));

            for (int i = 0; i < tempDT.Rows.Count; i += 2)
            {
                if (i - tempDT.Rows.Count == -1)
                {
                    DataRow newdr = rpt.NewRow();

                    newdr["TOJ"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "1";
                    newdr["COUNTPAGEMAN"] = "1";
                    newdr["NOW"] = NOW(tempDT, i);
                    newdr["AFTER"] = AFTER(tempDT, i);
                    newdr["PAGECOUNT"] = "";

                    rpt.Rows.Add(newdr);
                }
                else
                {
                    DataRow newdr = rpt.NewRow();

                    newdr["TOJ"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "1";
                    newdr["COUNTPAGEMAN"] = "1";
                    newdr["NOW"] = NOW(tempDT, i);
                    newdr["AFTER"] = AFTER(tempDT, i);
                    newdr["PAGECOUNT"] = "";

                    rpt.Rows.Add(newdr);

                    newdr = rpt.NewRow();
                    newdr["TOJ"] = string.Empty;
                    newdr["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "1";
                    newdr["COUNTPAGEMAN"] = "1";
                    newdr["NOW"] = NOW(tempDT, i + 1);
                    newdr["AFTER"] = AFTER(tempDT, i + 1);
                    newdr["PAGECOUNT"] = "";

                    rpt.Rows.Add(newdr);

                    if (tempDT.Rows[i + 1]["MZ_NAME"].ToString() != tempDT.Rows[i]["MZ_NAME"].ToString())
                    {
                        DataRow newdr1 = rpt.NewRow();

                        newdr1["TOJ"] = tempDT.Rows[i + 1]["MZ_NAME"];
                        newdr1["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr1["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr1["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr1["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr1["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr1["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr1["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr1["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr1["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr1["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr1["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr1["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr1["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr1["PRID3"] = (i / 2).ToString();
                        newdr1["PRINTGROUP"] = "1";
                        newdr1["COUNTPAGEMAN"] = "1";
                        newdr1["NOW"] = NOW(tempDT, i);
                        newdr1["AFTER"] = AFTER(tempDT, i);
                        newdr1["PAGECOUNT"] = "";

                        rpt.Rows.Add(newdr1);

                        newdr1 = rpt.NewRow();
                        newdr1["TOJ"] = string.Empty;
                        newdr1["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                        newdr1["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                        newdr1["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                        newdr1["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                        newdr1["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                        newdr1["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                        newdr1["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                        newdr1["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                        newdr1["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                        newdr1["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                        newdr1["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                        newdr1["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                        newdr1["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                        newdr1["PRID3"] = (i / 2).ToString();
                        newdr1["PRINTGROUP"] = "1";
                        newdr1["COUNTPAGEMAN"] = "1";
                        newdr1["NOW"] = NOW(tempDT, i + 1);
                        newdr1["AFTER"] = AFTER(tempDT, i + 1);
                        newdr1["PAGECOUNT"] = "";

                        rpt.Rows.Add(newdr1);
                    }
                }
            }

            //int side1 = 0;

            //int count = 0;

            //for (int i = 0; i < tempDT.Rows.Count; i++)
            //{
            //    if (tempDT.Rows.Count - i >= 2)
            //    {
            //        for (int k = side1; k <= side1 + 1; k++)
            //        {
            //            DataRow newdr = rpt.NewRow();
            //            newdr["TOJ"] = string.Empty;
            //            newdr["MZ_ID"] = tempDT.Rows[k]["MZ_ID"];
            //            newdr["MZ_NAME"] = tempDT.Rows[k]["MZ_NAME"];
            //            newdr["MZ_PRID"] = tempDT.Rows[k]["MZ_PRID"];
            //            newdr["MZ_PRID1"] = tempDT.Rows[k]["MZ_PRID1"];
            //            newdr["MZ_NREA"] = tempDT.Rows[k]["MZ_NREA"];
            //            newdr["MZ_NREA_NAME"] = tempDT.Rows[k]["MZ_NREA_NAME"];
            //            newdr["OTH_THING"] = tempDT.Rows[k]["OTH_THING"];
            //            newdr["MZ_REMARK"] = tempDT.Rows[k]["MZ_REMARK"];
            //            newdr["CONDITION"] = tempDT.Rows[k]["CONDITION"];
            //            newdr["MZ_DATE"] = tempDT.Rows[k]["MZ_DATE"];
            //            newdr["MZ_IDATE"] = tempDT.Rows[k]["MZ_IDATE"];
            //            newdr["SPEED_NO"] = tempDT.Rows[k]["SPEED_NO"];
            //            newdr["PWD_NO"] = tempDT.Rows[k]["PWD_NO"];
            //            newdr["PRID3"] = (i / 2).ToString();
            //            newdr["PRINTGROUP"] = "1";
            //            newdr["COUNTPAGEMAN"] = "２";
            //            newdr["NOW"] = NOW(tempDT, k);
            //            newdr["AFTER"] = AFTER(tempDT, k);
            //            rpt.Rows.Add(newdr);
            //        }
            //    }
            //    else
            //    {
            //        if (tempDT.Rows.Count % 2 == 0)
            //        {
            //            DataRow newdr = rpt.NewRow();
            //            newdr["TOJ"] = string.Empty;
            //            newdr["MZ_ID"] = tempDT.Rows[side1]["MZ_ID"];
            //            newdr["MZ_NAME"] = tempDT.Rows[side1]["MZ_NAME"];
            //            newdr["MZ_PRID"] = tempDT.Rows[side1]["MZ_PRID"];
            //            newdr["MZ_PRID1"] = tempDT.Rows[side1]["MZ_PRID1"];
            //            newdr["MZ_NREA"] = tempDT.Rows[side1]["MZ_NREA"];
            //            newdr["MZ_NREA_NAME"] = tempDT.Rows[side1]["MZ_NREA_NAME"];
            //            newdr["OTH_THING"] = tempDT.Rows[side1]["OTH_THING"];
            //            newdr["MZ_REMARK"] = tempDT.Rows[side1]["MZ_REMARK"];
            //            newdr["CONDITION"] = tempDT.Rows[side1]["CONDITION"];
            //            newdr["MZ_DATE"] = tempDT.Rows[side1]["MZ_DATE"];
            //            newdr["MZ_IDATE"] = tempDT.Rows[side1]["MZ_IDATE"];
            //            newdr["SPEED_NO"] = tempDT.Rows[side1]["SPEED_NO"];
            //            newdr["PWD_NO"] = tempDT.Rows[side1]["PWD_NO"];
            //            newdr["PRID3"] = (i / 2).ToString();
            //            newdr["PRINTGROUP"] = "1";
            //            newdr["COUNTPAGEMAN"] = "２";
            //            newdr["NOW"] = NOW(tempDT, side1);
            //            newdr["AFTER"] = AFTER(tempDT, side1);
            //            rpt.Rows.Add(newdr);

            //            newdr = rpt.NewRow();
            //            newdr["TOJ"] = string.Empty;
            //            newdr["MZ_ID"] = tempDT.Rows[side1 + 1]["MZ_ID"];
            //            newdr["MZ_NAME"] = tempDT.Rows[side1 + 1]["MZ_NAME"];
            //            newdr["MZ_PRID"] = tempDT.Rows[side1 + 1]["MZ_PRID"];
            //            newdr["MZ_PRID1"] = tempDT.Rows[side1 + 1]["MZ_PRID1"];
            //            newdr["MZ_NREA"] = tempDT.Rows[side1 + 1]["MZ_NREA"];
            //            newdr["MZ_NREA_NAME"] = tempDT.Rows[side1 + 1]["MZ_NREA_NAME"];
            //            newdr["OTH_THING"] = tempDT.Rows[side1 + 1]["OTH_THING"];
            //            newdr["MZ_REMARK"] = tempDT.Rows[side1 + 1]["MZ_REMARK"];
            //            newdr["CONDITION"] = tempDT.Rows[side1 + 1]["CONDITION"];
            //            newdr["MZ_DATE"] = tempDT.Rows[side1 + 1]["MZ_DATE"];
            //            newdr["MZ_IDATE"] = tempDT.Rows[side1 + 1]["MZ_IDATE"];
            //            newdr["SPEED_NO"] = tempDT.Rows[side1 + 1]["SPEED_NO"];
            //            newdr["PWD_NO"] = tempDT.Rows[side1 + 1]["PWD_NO"];
            //            newdr["PRID3"] = (i / 2).ToString();
            //            newdr["PRINTGROUP"] = "1";
            //            newdr["COUNTPAGEMAN"] = "２";
            //            newdr["NOW"] = NOW(tempDT, side1 + 1);
            //            newdr["AFTER"] = AFTER(tempDT, side1 + 1);
            //            rpt.Rows.Add(newdr);
            //        }
            //        else
            //        {

            //            DataRow newdr = rpt.NewRow();
            //            newdr["TOJ"] = string.Empty;
            //            newdr["MZ_ID"] = tempDT.Rows[side1]["MZ_ID"];
            //            newdr["MZ_NAME"] = tempDT.Rows[side1]["MZ_NAME"];
            //            newdr["MZ_PRID"] = tempDT.Rows[side1]["MZ_PRID"];
            //            newdr["MZ_PRID1"] = tempDT.Rows[side1]["MZ_PRID1"];
            //            newdr["MZ_NREA"] = tempDT.Rows[side1]["MZ_NREA"];
            //            newdr["MZ_NREA_NAME"] = tempDT.Rows[side1]["MZ_NREA_NAME"];
            //            newdr["OTH_THING"] = tempDT.Rows[side1]["OTH_THING"];
            //            newdr["MZ_REMARK"] = tempDT.Rows[side1]["MZ_REMARK"];
            //            newdr["CONDITION"] = tempDT.Rows[side1]["CONDITION"];
            //            newdr["MZ_DATE"] = tempDT.Rows[side1]["MZ_DATE"];
            //            newdr["MZ_IDATE"] = tempDT.Rows[side1]["MZ_IDATE"];
            //            newdr["SPEED_NO"] = tempDT.Rows[side1]["SPEED_NO"];
            //            newdr["PWD_NO"] = tempDT.Rows[side1]["PWD_NO"];
            //            newdr["PRID3"] = (i / 2).ToString();
            //            newdr["PRINTGROUP"] = "1";
            //            newdr["COUNTPAGEMAN"] = "２";
            //            newdr["NOW"] = NOW(tempDT, side1);
            //            newdr["AFTER"] = AFTER(tempDT, side1);
            //            rpt.Rows.Add(newdr);

            //        }
            //    }
            //    count++;

            //    if (count == 2)
            //    {
            //        side1 = side1 + 2;
            //        count = 0;
            //    }
            //}

            //int side = 0;

            //count = 0;

            //for (int i = 0; i < rpt.Rows.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        rpt.Rows[i]["TOJ"] = tempDT.Rows[side]["MZ_NAME"].ToString();
            //        side++;
            //    }
            //}


            for (int i = 0; i < tempDT.Rows.Count; i += 2)
            {
                if (i - tempDT.Rows.Count == -1)
                {
                    string TOJ = "";

                    if (CHKAD == tempDT.Rows[i]["MZ_AD"].ToString())
                    {
                        TOJ = o_A_KTYPE.RUNIT(tempDT.Rows[i]["MZ_UNIT"].ToString());
                    }
                    else
                    {
                        TOJ = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_AD"].ToString());
                    }

                    DataRow newdr = rpt.NewRow();
                    newdr["TOJ"] = TOJ;
                    newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "1";
                    newdr["COUNTPAGEMAN"] = "1";
                    newdr["NOW"] = NOW(tempDT, i);
                    newdr["AFTER"] = AFTER(tempDT, i);
                    newdr["PAGECOUNT"] = "";
                    rpt.Rows.Add(newdr);
                }
                else
                {
                    string TOJ = "";

                    if (CHKAD == tempDT.Rows[i]["MZ_AD"].ToString())
                    {
                        TOJ = o_A_KTYPE.RUNIT(tempDT.Rows[i]["MZ_UNIT"].ToString());
                    }
                    else
                    {
                        TOJ = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_AD"].ToString());
                    }

                    DataRow newdr = rpt.NewRow();
                    newdr["TOJ"] = TOJ;
                    newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "1";
                    newdr["COUNTPAGEMAN"] = "1";
                    newdr["NOW"] = NOW(tempDT, i);
                    newdr["AFTER"] = AFTER(tempDT, i);
                    newdr["PAGECOUNT"] = "";

                    rpt.Rows.Add(newdr);

                    newdr = rpt.NewRow();
                    newdr["TOJ"] = string.Empty;
                    newdr["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "1";
                    newdr["COUNTPAGEMAN"] = "1";
                    newdr["NOW"] = NOW(tempDT, i + 1);
                    newdr["AFTER"] = AFTER(tempDT, i + 1);
                    newdr["PAGECOUNT"] = "";

                    rpt.Rows.Add(newdr);

                    if (CHKAD == tempDT.Rows[i + 1]["MZ_AD"].ToString() && tempDT.Rows[i + 1]["MZ_UNIT"].ToString() != tempDT.Rows[i]["MZ_UNIT"].ToString())
                    {
                        if (CHKAD == tempDT.Rows[i + 1]["MZ_AD"].ToString())
                        {
                            TOJ = o_A_KTYPE.RUNIT(tempDT.Rows[i + 1]["MZ_UNIT"].ToString());
                        }
                        else
                        {
                            TOJ = o_A_KTYPE.RAD(tempDT.Rows[i + 1]["MZ_AD"].ToString());
                        }

                        DataRow newdr1 = rpt.NewRow();
                        newdr1["TOJ"] = TOJ;
                        newdr1["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr1["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr1["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr1["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr1["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr1["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr1["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr1["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr1["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr1["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr1["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr1["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr1["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr1["PRID3"] = (i / 2).ToString();
                        newdr1["PRINTGROUP"] = "1";
                        newdr1["COUNTPAGEMAN"] = "1";
                        newdr1["NOW"] = NOW(tempDT, i);
                        newdr1["AFTER"] = AFTER(tempDT, i);
                        newdr1["PAGECOUNT"] = "";

                        rpt.Rows.Add(newdr1);

                        newdr1 = rpt.NewRow();
                        newdr1["TOJ"] = string.Empty;
                        newdr1["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                        newdr1["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                        newdr1["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                        newdr1["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                        newdr1["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                        newdr1["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                        newdr1["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                        newdr1["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                        newdr1["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                        newdr1["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                        newdr1["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                        newdr1["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                        newdr1["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                        newdr1["PRID3"] = (i / 2).ToString();
                        newdr1["PRINTGROUP"] = "1";
                        newdr1["COUNTPAGEMAN"] = "1";
                        newdr1["NOW"] = NOW(tempDT, i + 1);
                        newdr1["AFTER"] = AFTER(tempDT, i + 1);
                        newdr1["PAGECOUNT"] = "";

                        rpt.Rows.Add(newdr1);
                    }
                    else if (CHKAD != tempDT.Rows[i + 1]["MZ_AD"].ToString())
                    {
                        TOJ = o_A_KTYPE.RAD(tempDT.Rows[i + 1]["MZ_AD"].ToString());

                        DataRow newdr1 = rpt.NewRow();
                        newdr1["TOJ"] = TOJ;
                        newdr1["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr1["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr1["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr1["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr1["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr1["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr1["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr1["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr1["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr1["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr1["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr1["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr1["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr1["PRID3"] = (i / 2).ToString();
                        newdr1["PRINTGROUP"] = "1";
                        newdr1["COUNTPAGEMAN"] = "1";
                        newdr1["NOW"] = NOW(tempDT, i);
                        newdr1["AFTER"] = AFTER(tempDT, i);
                        newdr1["PAGECOUNT"] = "";

                        rpt.Rows.Add(newdr1);

                        newdr1 = rpt.NewRow();
                        newdr1["TOJ"] = string.Empty;
                        newdr1["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                        newdr1["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                        newdr1["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                        newdr1["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                        newdr1["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                        newdr1["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                        newdr1["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                        newdr1["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                        newdr1["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                        newdr1["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                        newdr1["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                        newdr1["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                        newdr1["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                        newdr1["PRID3"] = (i / 2).ToString();
                        newdr1["PRINTGROUP"] = "1";
                        newdr1["COUNTPAGEMAN"] = "1";
                        newdr1["NOW"] = NOW(tempDT, i + 1);
                        newdr1["AFTER"] = AFTER(tempDT, i + 1);
                        newdr1["PAGECOUNT"] = "";

                        rpt.Rows.Add(newdr1);

                    }

                }
            }

            //string oldAD = "";

            //for (int i = 0; i < tempDT.Rows.Count; i++)
            //{
            //    if (oldAD != "" && oldAD != tempDT.Rows[i]["MZ_AD"].ToString() && i % 2 == 1)
            //    {
            //        DataRow newdr = rpt.NewRow();
            //        newdr["TOJ"] = string.Empty;
            //        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
            //        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
            //        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
            //        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
            //        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
            //        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
            //        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
            //        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
            //        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
            //        newdr["MZ_POSIND"] = tempDT.Rows[i]["MZ_POSIND"];
            //        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
            //        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
            //        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
            //        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
            //        newdr["PRID3"] = (i / 2).ToString();
            //        newdr["PRINTGROUP"] = "2";
            //        newdr["COUNTPAGEMAN"] = "２";
            //        newdr["NOW"] = NOW(tempDT, i);
            //        newdr["AFTER"] = AFTER(tempDT, i);

            //        rpt.Rows.Add(newdr);

            //        newdr = rpt.NewRow();
            //        newdr["TOJ"] = tempDT.Rows[i - 1]["MZ_AD_NAME"];
            //        newdr["MZ_ID"] = tempDT.Rows[i - 1]["MZ_ID"];
            //        newdr["MZ_NAME"] = tempDT.Rows[i - 1]["MZ_NAME"];
            //        newdr["MZ_PRID"] = tempDT.Rows[i - 1]["MZ_PRID"];
            //        newdr["MZ_PRID1"] = tempDT.Rows[i - 1]["MZ_PRID1"];
            //        newdr["MZ_NREA"] = tempDT.Rows[i - 1]["MZ_NREA"];
            //        newdr["MZ_NREA_NAME"] = tempDT.Rows[i - 1]["MZ_NREA_NAME"];
            //        newdr["OTH_THING"] = tempDT.Rows[i - 1]["OTH_THING"];
            //        newdr["MZ_REMARK"] = tempDT.Rows[i - 1]["MZ_REMARK"];
            //        newdr["CONDITION"] = tempDT.Rows[i - 1]["CONDITION"];
            //        newdr["MZ_POSIND"] = tempDT.Rows[i - 1]["MZ_POSIND"];
            //        newdr["MZ_DATE"] = tempDT.Rows[i - 1]["MZ_DATE"];
            //        newdr["MZ_IDATE"] = tempDT.Rows[i - 1]["MZ_IDATE"];
            //        newdr["SPEED_NO"] = tempDT.Rows[i - 1]["SPEED_NO"];
            //        newdr["PWD_NO"] = tempDT.Rows[i - 1]["PWD_NO"];
            //        newdr["PRID3"] = (i / 2).ToString();
            //        newdr["PRINTGROUP"] = "2";
            //        newdr["COUNTPAGEMAN"] = "２";
            //        newdr["NOW"] = NOW(tempDT, i);
            //        newdr["AFTER"] = AFTER(tempDT, i);
            //        rpt.Rows.Add(newdr);

            //        newdr = rpt.NewRow();
            //        newdr["TOJ"] = string.Empty;
            //        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
            //        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
            //        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
            //        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
            //        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
            //        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
            //        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
            //        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
            //        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
            //        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
            //        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
            //        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
            //        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
            //        newdr["PRID3"] = (i / 2).ToString();
            //        newdr["PRINTGROUP"] = "2";
            //        newdr["COUNTPAGEMAN"] = "２";
            //        newdr["NOW"] = NOW(tempDT, i);
            //        newdr["AFTER"] = AFTER(tempDT, i);
            //        rpt.Rows.Add(newdr);
            //    }
            //    else if (i % 2 == 0)
            //    {
            //        DataRow newdr = rpt.NewRow();
            //        newdr["TOJ"] = tempDT.Rows[i]["MZ_AD_NAME"];
            //        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
            //        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
            //        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
            //        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
            //        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
            //        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
            //        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
            //        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
            //        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
            //        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
            //        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
            //        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
            //        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
            //        newdr["PRID3"] = (i / 2).ToString();
            //        newdr["PRINTGROUP"] = "2";
            //        newdr["COUNTPAGEMAN"] = "２";
            //        newdr["NOW"] = NOW(tempDT, i);
            //        newdr["AFTER"] = AFTER(tempDT, i);
            //        rpt.Rows.Add(newdr);
            //    }
            //    else
            //    {
            //        DataRow newdr = rpt.NewRow();
            //        newdr["TOJ"] = string.Empty;
            //        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
            //        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
            //        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
            //        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
            //        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
            //        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
            //        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
            //        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
            //        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
            //        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
            //        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
            //        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
            //        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
            //        newdr["PRID3"] = (i / 2).ToString();
            //        newdr["PRINTGROUP"] = "2";
            //        newdr["COUNTPAGEMAN"] = "２";
            //        newdr["NOW"] = NOW(tempDT, i);
            //        newdr["AFTER"] = AFTER(tempDT, i);
            //        rpt.Rows.Add(newdr);
            //    }

            //    oldAD = tempDT.Rows[i]["MZ_AD"].ToString();
            //}

            //if (tempDT.Rows.Count % 2 == 1)
            //{
            //    rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
            //}

            if (CHKAD == "382130100C" || CHKAD == "382130300C")
            {
                for (int i = 0; i < tempDT.Rows.Count; i += 2)
                {
                    if (i - tempDT.Rows.Count == -1)
                    {
                        if (tempDT.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || tempDT.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                        {

                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_EXAD"].ToString());
                            newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "1";
                            newdr["NOW"] = NOW(tempDT, i);
                            newdr["AFTER"] = AFTER(tempDT, i);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);
                        }
                    }
                    else
                    {
                        if (tempDT.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || tempDT.Rows[i]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_EXAD"].ToString());
                            newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "1";
                            newdr["NOW"] = NOW(tempDT, i);
                            newdr["AFTER"] = AFTER(tempDT, i);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);

                            newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "1";
                            newdr["NOW"] = NOW(tempDT, i + 1);
                            newdr["AFTER"] = AFTER(tempDT, i + 1);
                            newdr["PAGECOUNT"] = "";

                            rpt.Rows.Add(newdr);
                        }

                        if (tempDT.Rows[i + 1]["MZ_UNIT"].ToString() != tempDT.Rows[i]["MZ_UNIT"].ToString())
                        {
                            if (tempDT.Rows[i + 1]["MZ_UNIT"].ToString().Substring(0, 2) == "DH" || tempDT.Rows[i + 1]["MZ_UNIT"].ToString().Substring(0, 2) == "DG")
                            {

                                DataRow newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = o_A_KTYPE.RAD(tempDT.Rows[i + 1]["MZ_EXAD"].ToString());
                                newdr1["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                                newdr1["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                                newdr1["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                                newdr1["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                                newdr1["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                                newdr1["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                                newdr1["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                                newdr1["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                                newdr1["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                                newdr1["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                                newdr1["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                                newdr1["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "1";
                                newdr1["COUNTPAGEMAN"] = "1";
                                newdr1["NOW"] = NOW(tempDT, i);
                                newdr1["AFTER"] = AFTER(tempDT, i);
                                newdr1["PAGECOUNT"] = "";

                                rpt.Rows.Add(newdr1);

                                newdr1 = rpt.NewRow();
                                newdr1["TOJ"] = string.Empty;
                                newdr1["MZ_ID"] = tempDT.Rows[i + 1]["MZ_ID"];
                                newdr1["MZ_NAME"] = tempDT.Rows[i + 1]["MZ_NAME"];
                                newdr1["MZ_PRID"] = tempDT.Rows[i + 1]["MZ_PRID"];
                                newdr1["MZ_PRID1"] = tempDT.Rows[i + 1]["MZ_PRID1"];
                                newdr1["MZ_NREA"] = tempDT.Rows[i + 1]["MZ_NREA"];
                                newdr1["MZ_NREA_NAME"] = tempDT.Rows[i + 1]["MZ_NREA_NAME"];
                                newdr1["OTH_THING"] = tempDT.Rows[i + 1]["OTH_THING"];
                                newdr1["MZ_REMARK"] = tempDT.Rows[i + 1]["MZ_REMARK"];
                                newdr1["CONDITION"] = tempDT.Rows[i + 1]["CONDITION"];
                                newdr1["MZ_DATE"] = tempDT.Rows[i + 1]["MZ_DATE"];
                                newdr1["MZ_IDATE"] = tempDT.Rows[i + 1]["MZ_IDATE"];
                                newdr1["SPEED_NO"] = tempDT.Rows[i + 1]["SPEED_NO"];
                                newdr1["PWD_NO"] = tempDT.Rows[i + 1]["PWD_NO"];
                                newdr1["PRID3"] = (i / 2).ToString();
                                newdr1["PRINTGROUP"] = "1";
                                newdr1["COUNTPAGEMAN"] = "1";
                                newdr1["NOW"] = NOW(tempDT, i + 1);
                                newdr1["AFTER"] = AFTER(tempDT, i + 1);
                                newdr1["PAGECOUNT"] = "";

                                rpt.Rows.Add(newdr1);
                            }
                        }
                    }
                }
            }

            string[] explain = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'").Split(new char[] { '、' });

            for (int j = 0; j < explain.Length; j++)
            {
                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        DataRow newdr = rpt.NewRow();
                        newdr["TOJ"] = explain[j];
                        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr["MZ_POSIND"] = tempDT.Rows[i]["MZ_POSIND"];
                        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = (4 + j).ToString();
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);
                        newdr["PAGECOUNT"] = "";
                        rpt.Rows.Add(newdr);
                    }
                    else
                    {
                        DataRow newdr = rpt.NewRow();
                        newdr["TOJ"] = string.Empty;
                        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = (4 + j).ToString();
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);
                        newdr["PAGECOUNT"] = "";
                        rpt.Rows.Add(newdr);
                    }
                }

                if (tempDT.Rows.Count % 2 == 1)
                {
                    rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
                }
            }

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

            Session["rpt_dt"] = rpt;

            Session["TITLE"] = string.Format("{0}  令", o_A_KTYPE.RAD(CHKAD));

            strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", DropDownList_MZ_PRID.SelectedItem.Text, TextBox_MZ_PRID1.Text.Trim());
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            string memo = string.Empty;
            string memoString = "";

            if (temp.Rows.Count > 0)
            {
                string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                {
                    string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < exps.Count(); i++)
                    {
                        memo = exps[i] + "。";
                        memo = o_CommonService.d_report_break_line(memo, 70, "&N　　");
                        memoString += memo;
                    }

                    memoString += "&N";

                }
                else
                {
                    memoString = o_CommonService.d_report_break_line(exp, 70, "&N");
                }

                Session["EXP"] = memoString;
                Session["EXP1"] = exp0;
                Session["EXP2"] = o_CommonService.d_report_break_line(exp1, 70, "&N");
            }
            else
            {
                string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                Session["EXP"] = memo;
                Session["EXP1"] = exp0;
                Session["EXP2"] = o_CommonService.d_report_break_line(exp1, 70, "&N");
            }


            strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", DropDownList_MZ_PRID.SelectedItem.Text.Trim());
            DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (dt1.Rows.Count > 0)
            {
                Session["MAN"] = dt1.Rows[0][1].ToString() + "   " + dt1.Rows[0][0].ToString();
            }
            else
            {
                Session["MAN"] = string.Empty;
            }

            strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD");
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            string result = string.Empty;
            if (temp.Rows.Count > 0)
            {
                result += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                result += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                result += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";
                Session["CONN"] = result;
            }
            else
            {
                Session["CONN"] = result;
            }


            string tmp_url = "A_rpt.aspx?fn=posit&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSQL = @"SELECT MZ_ID,MZ_NAME,A_POSIT2.MZ_PRID,A_POSIT2.MZ_PRID1,MZ_EXOPOS,
                            MZ_NREA,  AKNR.MZ_KCHI  MZ_NREA_NAME,  MZ_EXOAD, AKEXOD.MZ_KCHI   MZ_EXOAD_NAME, 
                            MZ_EXOUNIT, AKEXOU.MZ_KCHI   MZ_EXOUNIT_NAME  ,   MZ_EXOOCC,  AKEXOO.MZ_KCHI  MZ_EXOOCC_NAME ,                     
                            MZ_EXORANK,  AKEXOR.MZ_KCHI    MZ_EXORANK_NAME,   MZ_EXRANK1,   AKEXOR1.MZ_KCHI    MZ_EXORANK1_NAME, 
                            A_POSIT2.MZ_AD, AKD.MZ_KCHI      MZ_AD_NAME ,  MZ_UNIT,  AKU.MZ_KCHI   MZ_UNIT_NAME   , 
                            MZ_OCCC,  AKO.MZ_KCHI   MZ_OCCC_NAME  ,  MZ_RANK, AKR.MZ_KCHI   MZ_RANK_NAME  ,  
                            MZ_RANK1,  AKR1.MZ_KCHI   MZ_RANK1_NAME ,MZ_SRANK,AKSR.MZ_KCHI  MZ_SRANK_NAME, 
                            MZ_CHISI, AKCH.MZ_KCHI  MZ_CHISI_NAME,  MZ_SLVC,   AKSL.MZ_KCHI   MZ_SLVC_NAME ,  
                            (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE,
                            (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE,
                            ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO       ,
                            dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV 
                            FROM A_POSIT2 
                            LEFT JOIN A_KTYPE AKNR ON RTRIM(AKNR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_NREA) AND RTRIM(AKNR.MZ_KTYPE)='11' 
                            LEFT JOIN A_KTYPE AKEXOD ON RTRIM(AKEXOD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOAD) AND RTRIM(AKEXOD.MZ_KTYPE)='04' 
                            LEFT JOIN A_KTYPE AKEXOU ON RTRIM(AKEXOU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOUNIT) AND RTRIM(AKEXOU.MZ_KTYPE)='25' 
                            LEFT JOIN A_KTYPE AKEXOO ON RTRIM(AKEXOO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOOCC) AND RTRIM(AKEXOO.MZ_KTYPE)='26' 
                            LEFT JOIN A_KTYPE AKEXOR ON RTRIM(AKEXOR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXORANK) AND RTRIM(AKEXOR.MZ_KTYPE)='09' 
                            LEFT JOIN A_KTYPE AKEXOR1 ON RTRIM(AKEXOR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXRANK1) AND RTRIM(AKEXOR1.MZ_KTYPE)='09' 

                            LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                            LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                            LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                            LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                            LEFT JOIN A_KTYPE AKR1 ON RTRIM(AKR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKR1.MZ_KTYPE)='09' 
                            LEFT JOIN A_KTYPE AKSR ON RTRIM(AKSR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKSR.MZ_KTYPE)='09' 
                            LEFT JOIN A_KTYPE AKCH ON RTRIM(AKCH.MZ_KCODE)=RTRIM(A_POSIT2.MZ_CHISI) AND RTRIM(AKCH.MZ_KTYPE)='23' 
                            LEFT JOIN A_KTYPE AKSL ON RTRIM(AKSL.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SLVC) AND RTRIM(AKSL.MZ_KTYPE)='64' 

                            LEFT JOIN A_CHKAD ACK ON  ACK.MZ_PRID=A_POSIT2.MZ_PRID and ACK.MZ_AD=A_POSIT2.MZ_CHKAD




                            WHERE A_POSIT2.MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND A_POSIT2.MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

            strSQL += " ORDER BY TBDV,MZ_AD,MZ_UNIT,MZ_OCCC";


            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            DataTable rpt = new DataTable();

            rpt.Columns.Add("TOJ", typeof(string));
            rpt.Columns.Add("MZ_ID", typeof(string));
            rpt.Columns.Add("MZ_NAME", typeof(string));
            rpt.Columns.Add("MZ_PRID", typeof(string));
            rpt.Columns.Add("MZ_PRID1", typeof(string));
            rpt.Columns.Add("MZ_NREA", typeof(string));
            rpt.Columns.Add("MZ_NREA_NAME", typeof(string));
            rpt.Columns.Add("OTH_THING", typeof(string));
            rpt.Columns.Add("MZ_REMARK", typeof(string));
            rpt.Columns.Add("CONDITION", typeof(string));
            rpt.Columns.Add("MZ_DATE", typeof(string));
            rpt.Columns.Add("MZ_IDATE", typeof(string));
            rpt.Columns.Add("SPEED_NO", typeof(string));
            rpt.Columns.Add("PWD_NO", typeof(string));
            rpt.Columns.Add("PRID3", typeof(string));
            rpt.Columns.Add("PRINTGROUP", typeof(string));
            rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
            rpt.Columns.Add("NOW", typeof(string));
            rpt.Columns.Add("AFTER", typeof(string));

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                DataRow newdr = rpt.NewRow();
                newdr["TOJ"] = string.Empty;
                newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                newdr["PRID3"] = (i / 2).ToString();
                newdr["PRINTGROUP"] = "3";
                newdr["COUNTPAGEMAN"] = "２";
                newdr["NOW"] = NOW(tempDT, i);
                newdr["AFTER"] = AFTER(tempDT, i);
                rpt.Rows.Add(newdr);
            }

            if (tempDT.Rows.Count % 2 == 1)
            {
                rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
            }

            Session["rpt_dt"] = rpt;

            Session["TITLE"] = string.Format("{0}   令(稿)", o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + Session["ADPMZ_EXAD"].ToString() + "'"));

            strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", DropDownList_MZ_PRID.SelectedItem.Text, TextBox_MZ_PRID1.Text.Trim());
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            string memo = string.Empty;
            string memoString = "";

            if (temp.Rows.Count > 0)
            {
                string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                {
                    string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < exps.Count(); i++)
                    {
                        memo = exps[i] + "。";
                        memo = o_CommonService.d_report_break_line(memo, 70, "&N　　");
                        memoString += memo;
                    }

                    memoString += "&N";

                }
                else
                {
                    memoString = o_CommonService.d_report_break_line(exp, 70, "&N");
                }

                Session["EXP"] = memoString;
                Session["EXP1"] = exp0;
                Session["EXP2"] = o_CommonService.d_report_break_line(exp1, 70, "&N");
            }
            else
            {
                string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                Session["EXP"] = memo;
                Session["EXP1"] = exp0;
                Session["EXP2"] = o_CommonService.d_report_break_line(exp1, 70, "&N");
            }


            strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", DropDownList_MZ_PRID.SelectedItem.Text.Trim());
            DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            string name = dt1.Rows[0]["MZ_MASTER_NAME"].ToString();
            string result_name = string.Empty;
            string result_namemark = string.Empty;

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < name.Length; i++)
                {
                    result_name += name.Substring(i, 1) + "  ";
                    if (i == 0)
                        result_namemark += name.Substring(i, 1) + "  ";
                    else
                        result_namemark += "○" + "  ";
                }
                Session["MAN"] = dt1.Rows[0][1].ToString() + "  " + result_namemark;
            }
            else
            {
                Session["MAN"] = string.Empty;
            }

            strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD");
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            string result = string.Empty;
            if (temp.Rows.Count > 0)
            {
                result += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                result += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                result += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";
                Session["CONN"] = result;
            }
            else
            {
                Session["CONN"] = result;
            }

            string tmp_url = "A_rpt.aspx?fn=posit1&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
