using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary
{
    public partial class _0_test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            B.set_Panel_EnterToTAB(ref this.Panel1);            
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            change(TextBox2);
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "id2pay", "setTimeout(\"$get('" + TextBox2.ClientID + "').focus();\", 100);setTimeout(\"$get('" + TextBox2.ClientID + "').focus();\", 100);", true);
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            change(TextBox3);
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "id2pay", "setTimeout(\"$get('" + TextBox3.ClientID + "').focus();\", 100);setTimeout(\"$get('" + TextBox3.ClientID + "').focus();\", 100);", true);
        }

        protected void TextBox3_TextChanged(object sender, EventArgs e)
        {
            change(TextBox4);
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "id2pay", "setTimeout(\"$get('" + TextBox4.ClientID + "').focus();\", 100);setTimeout(\"$get('" + TextBox4.ClientID + "').focus();\", 100);", true);
        }

        protected void TextBox4_TextChanged(object sender, EventArgs e)
        {
            change(TextBox1);
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "id2pay", "setTimeout(\"$get('" + TextBox1.ClientID + "').focus();\", 100);setTimeout(\"$get('" + TextBox1.ClientID + "').focus();\", 100);", true);
        }

        private void change(TextBox tb)
        {
            if (tb is TextBox)
            {
                tb.Text = "aaaa";
                //(tb as TextBox).Focus();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (tb as TextBox).ClientID + "').focus();$get('" + (tb as TextBox).ClientID + "').focus();", true); 
            }
        }
    }
}
