using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateToTextFile
{
    class Program
    {
        static void Main(string[] args)
        {
            // 取得執行檔案的路徑
            string currentDirectory = Directory.GetCurrentDirectory();// AppDomain.CurrentDomain.BaseDirectory;

            // 關鍵字檔案的路徑
            string keywordsFilePath = Path.Combine(currentDirectory, "input\\keywords.txt");

            // 範本檔案的路徑
            string templateFilePath = Path.Combine(currentDirectory, "input\\template.txt");

            string fileNamePath = Path.Combine(currentDirectory, "input\\fileName.txt");
            // 檔名格式範例
            string outputFormat = File.ReadAllText(fileNamePath);
            //輸出檔案名稱與路徑
            string outputfileNamePath = Path.Combine(currentDirectory, "output\\"+ outputFormat);

            try
            {
                // 讀取關鍵字
                string[] keywords = File.ReadAllLines(keywordsFilePath);

                // 讀取範本內容
                string templateContent = File.ReadAllText(templateFilePath);

                // 依序處理每個關鍵字
                foreach (string keyword in keywords)
                {
                    // 替換範本中的關鍵字
                    string outputContent = templateContent.Replace("#WORD#", keyword);

                    // 組合檔名
                    string outputFileName = outputfileNamePath.Replace("#WORD#", keyword);

                    // 寫入輸出檔案
                    File.WriteAllText(outputFileName, outputContent,Encoding.UTF8);

                    Console.WriteLine($"已生成檔案：{outputFileName}");
                }

                Console.WriteLine("完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
            }
        }
    }

}
