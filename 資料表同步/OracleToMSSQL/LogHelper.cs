using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleToMSSQL
{
    /// <summary>
    /// Log幫手,這邊先用Console就好,以後如果有需要可以改成別種形式的Log
    /// </summary>
    public static class LogHelper
    {
        public static void SaveLog(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
