using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPPDDB.Online_Leave
{
    /// <summary>
    /// 線上簽核系統用的功能代碼
    /// </summary>
    public class Const_Mode
    {
        public const string 差假線上簽核 = "temp";
        public const string 代理人簽核 = "AGENTS";
        public const string 差旅費線上簽核 = "b_show";
        public const string 抽回差假簽核 = "re";
        public const string 抽回差旅費簽核 = "b_re";
        public const string 抽回銷假簽核 = "c_re";
        public const string 查詢簽核狀態 = "FLOW_ALL";
        public const string 已決行案件 = "Person";
        public const string 已核定假單 = "allOK";
        public const string 簽核狀態 = "FLOW";

        /*         
         "re"
        Const_Mode.抽回差假簽核
         */
    }
}