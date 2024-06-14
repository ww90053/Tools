using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    /// <summary>
    /// 人事系統基本頁面
    /// </summary>
    public class C_BasePage : System.Web.UI.Page
    {
        public CFService CFService = new CFService();
        /// <summary>
        /// 頁面標題
        /// </summary>
        public static string PageTitle = string.Empty;
        /// <summary>
        /// 模組代碼
        /// </summary>
        public string _TPM_FION
        {
            get { return ViewState["TPM_FION"] != null ? ViewState["TPM_FION"].ToStringNullSafe() : string.Empty; }
            set { ViewState["TPM_FION"] = value; }
        }

        public string _strGID
        {
            get { return ViewState["STRGID"] != null ? ViewState["STRGID"].ToStringNullSafe() : string.Empty; }
            set { ViewState["STRGID"] = value; }
        }
        public C_BasePage() : base()
        {

        }

        public static string strGID(string MZ_ID)
        {
            string MZ_POWER = "";
            if (HttpContext.Current.Session["MZ_POWER"] != null)
            {
                MZ_POWER = HttpContext.Current.Session["MZ_POWER"].ToString();
            }
            else
            {
                MZ_POWER = o_DBFactory.ABC_toTest.Get_First_Field(string.Format("SELECT MZ_POWER FROM A_DLBASE WHERE MZ_ID='{0}' ", MZ_ID), null);
                HttpContext.Current.Session["MZ_POWER"] = MZ_POWER;
            }
            return MZ_POWER;
        }
    }
}