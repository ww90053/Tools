using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary
{
    public partial class CodeManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CustomInit();
        }

        private void CustomInit()
        {
            sc_Technics.TableName = "B_TECHNICS";
            sc_Workp.TableName = "B_WORKP";
            sc_Bonus.TableName = "B_BONUS";
            sc_Adventive.TableName = "B_ADVENTIVE";
            sc_Electric.TableName = "B_ELECTRIC";
        }
    }
}
