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

namespace TPPDDB._18_Online_Leave
{
    public class Online
    {
        public static void set_Panel_EnterToTAB(ref Panel Panel1)
        {
            foreach (Object ob in Panel1.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;

                    tbox.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;

                    ddlist.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is Button)
                {
                    Button bt = (Button)ob;

                    bt.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                    bt.TabIndex = -1;
                }
                else if (ob is CheckBox)
                {
                    CheckBox cb = (CheckBox)ob;

                    cb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is RadioButtonList)
                {
                    RadioButtonList rbl = (RadioButtonList)ob;

                    rbl.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is Panel)
                {
                    Panel pl = (Panel)ob;

                    foreach (Object pl1 in pl.Controls)
                    {
                        if (pl1 is TextBox)
                        {
                            TextBox tbox = (TextBox)pl1;

                            tbox.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (pl1 is DropDownList)
                        {
                            DropDownList ddlist = (DropDownList)pl1;

                            ddlist.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (pl1 is Button)
                        {
                            Button bt = (Button)pl1;

                            bt.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                            bt.TabIndex = -1;
                        }
                        else if (pl1 is CheckBox)
                        {
                            CheckBox cb = (CheckBox)pl1;

                            cb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (pl1 is RadioButtonList)
                        {
                            RadioButtonList rbl = (RadioButtonList)pl1;

                            rbl.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (ob is TBGridView)
                        {
                            TBGridView tb1 = (TBGridView)ob;
                            tb1.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                        else if (ob is GridView)
                        {
                            GridView tb2 = (GridView)ob;
                            tb2.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                        }
                    }
                }
                else if (ob is TBGridView)
                {
                    TBGridView tb = (TBGridView)ob;
                    tb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is GridView)
                {
                    GridView tb = (GridView)ob;
                    tb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
                else if (ob is AjaxControlToolkit.ComboBox)
                {
                    AjaxControlToolkit.ComboBox acb = (AjaxControlToolkit.ComboBox)ob;
                    acb.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
                }
            }
        }     

        //指定 目標容器中 控制項的開或關
        public static void controlEnable(ref Panel Pl, Boolean sw)
        {
            foreach (Object ob in Pl.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;
                    tbox.Enabled = sw;
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;
                    ddlist.Enabled = sw;
                }
                else if (ob is RadioButton)
                {
                    RadioButton rd = (RadioButton)ob;
                    rd.Enabled = sw;
                }
                else if (ob is RadioButtonList)
                {
                    RadioButtonList rd = (RadioButtonList)ob;
                    rd.Enabled = sw;
                }
                else if (ob is ImageButton)
                {
                    ImageButton IB = (ImageButton)ob;
                    IB.Enabled = sw;
                }
                else if (ob is Button)
                {
                    Button bt = (Button)ob;
                    bt.Enabled = sw;
                }
                else if (ob is CheckBox )
                {
                    CheckBox cb = (CheckBox)ob;
                    cb.Enabled = sw;
                }
            }
        }
    }
}
