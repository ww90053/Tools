using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceKeyword
{
    class Program
    {
        static void Main()
        {
            // Read configuration settings
            string rulesFilePath = ConfigurationManager.AppSettings["RulesFilePath"];
            string sourceDirectory = ConfigurationManager.AppSettings["SourceDirectory"];
            string destinationDirectory = ConfigurationManager.AppSettings["DestinationDirectory"];

            // Validate configuration settings
            if (string.IsNullOrEmpty(rulesFilePath) || string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(destinationDirectory))
            {
                Console.WriteLine("One or more configuration settings are missing or invalid.");
                return;
            }

            //轉換一下來源和目的地檔案路徑,因為等一下寫入資料需要
            //sourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), sourceDirectory);
            //destinationDirectory = Path.Combine(Directory.GetCurrentDirectory(), destinationDirectory);

            // Step 1: 讀取來源關鍵字
            Dictionary<string, int> keywordDic = GetKeyWord_Dic(rulesFilePath);

            // Step 2: Ensure the destination directory exists
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            //讀取來源資料夾內的所有檔案
            List<string> filePathList = GetAllFiles(sourceDirectory);



            // Step 3: Process each file in the source directory
            List<string> list_keyword = new List<string>();
            list_keyword = keywordDic.Select(x => x.Key).ToList();
            foreach (string filePath in filePathList)
            {
                //把檔案路徑從來源取代程目標資料夾
                //string destinationFilePath = filePath.Replace(sourceDirectory,destinationDirectory);

                // Read the content of the file
                string content = File.ReadAllText(filePath);

                // Replace keywords based on the rules
                foreach (var keyword in list_keyword)
                {   //有找到就+1
                    if (content.Contains(keyword))
                    {
                        keywordDic[keyword] = keywordDic[keyword] + 1;
                    }
                }
            }
            //把檔案輸出
            string outputText = "";
            foreach (var item in keywordDic.OrderByDescending(x => x.Value))
            {
                outputText += $"{item.Key} hit {item.Value}\r\n";
            }
            File.WriteAllText(destinationDirectory+"output.txt", outputText);
        }

        /// <summary>
        /// 讀取要替換關鍵字
        /// </summary>
        /// <param name="rulesFilePath"></param>
        /// <returns></returns>
        static Dictionary<string, int> GetKeyWord_Dic(string rulesFilePath)
        {
            var dic = new Dictionary<string, int>();
            string[] lines = File.ReadAllLines(rulesFilePath);

            foreach (string key in lines)
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    dic.Add(key, 0);
                }
            }

            return dic;
        }

        /// <summary>
        /// 根據資料夾路徑,讀取裡面所有檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static List<string> GetAllFiles(string path)
        {
            List<string> filesList = new List<string>();

            try
            {
                // 取得指定路徑下的所有檔案
                string[] files1 = Directory.GetFiles(path, "*.cs");
                string[] files2 = Directory.GetFiles(path, "*.aspx");
                filesList.AddRange(files1);
                filesList.AddRange(files2);

                // 取得指定路徑下的所有子資料夾
                string[] subDirectories = Directory.GetDirectories(path);
                foreach (string subDirectory in subDirectories)
                {
                    // 遞迴呼叫，取得子資料夾中的所有檔案
                    filesList.AddRange(GetAllFiles(subDirectory));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"錯誤: {ex.Message}");
            }

            return filesList;
        }

        /// <summary>
        /// 寫入帶有UTF8 BOM
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        static void WriteToFileWithUtf8Bom(string filePath, string content)
        {
            try
            {


                // 使用 UTF-8 帶 BOM 編碼
                UTF8Encoding utf8WithBom = new UTF8Encoding(true);

                // 將內容轉換為位元組陣列
                byte[] encodedBytes = utf8WithBom.GetBytes(content);

                //確保資料夾存在
                string dir = Path.GetDirectoryName(filePath);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                // 寫入檔案
                //File.WriteAllBytes(filePath, encodedBytes);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    // Encoding.UTF8 同 new UTF8Encoding(true)
                    // Create a UTF-8 encoding that supports a BOM.
                    using (StreamWriter sw = new StreamWriter(fileStream, new UTF8Encoding(true)))
                    {
                        sw.Write(content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"錯誤: {ex.Message}");
            }
        }
    }
}
