using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindTableFromSQL
{
    class Program
    {
        /// <summary>
        /// 輸入來源檔案,需要跟應用程式放在同一資料夾
        /// </summary>
        static string inputFile = "input.txt";

        static void Main(string[] args)
        {
            //根據輸入的SQL字串,找出可能是要抓的資料表
            //此工具主要是用來分析系統中可能會用到那些資料表

            string SQLText = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + inputFile);
            //先轉大寫
            SQLText = SQLText.ToUpper();

            //要找到的輸出目標
            List<string> List = new List<string>();
            // 根據 關鍵字 SQL指令,找出SQL中的資料表
            Get_TableName_inSQL("FROM", SQLText, ref List);
            Get_TableName_inSQL("JOIN", SQLText, ref List);
            Get_TableName_inSQL("INTO", SQLText, ref List);
            //把結果過濾不重複
            List = List.Distinct().OrderBy(x => x).ToList();
            string filePath = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "output.txt";
            //File.Create(filePath);
            foreach (var item in List)
            {
                File.WriteAllLines(filePath, List, Encoding.UTF8);
            }
        }
        /// <summary>
        /// 根據 關鍵字 SQL指令,找出SQL中的資料表
        /// EX:  JOIN XXXXX
        /// EX:  FROM XXXXX
        /// </summary>
        /// <param name="SQLText"></param>
        /// <param name="matches"></param>
        /// <param name="List"></param>
        private static void Get_TableName_inSQL(string KeyWord, string SQLText,  ref List<string> List)
        {
            // 透過正則運算式
            // EX: FROM\s+(\w+) 
            MatchCollection matches;
            matches = Regex.Matches(SQLText, KeyWord + @"\s+(\w+)");
            // 如果找到匹配项，则输出每个匹配项后面的第一个单词
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string firstWordAfterFrom = match.Groups[1].Value;
                    Console.WriteLine(firstWordAfterFrom);
                    //轉大寫,納入結果
                    List.Add(firstWordAfterFrom.ToUpper());
                }
            }
            else
            {
                Console.WriteLine("FROM NO MATCH");
            }
        }
    }
}
