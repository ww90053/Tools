using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindBig5ToUTF8
{
    class Program
    {

        static void Main(string[] args)
        {
            // 讀取來源與目標
            string sourceDirectory = ConfigurationManager.AppSettings["SourceDirectory"];
            string destinationDirectory = ConfigurationManager.AppSettings["DestinationDirectory"];

            if (string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(destinationDirectory))
            {
                Console.WriteLine("One or more configuration settings are missing or invalid.");
                return;
            }
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            //讀取來源資料夾內的所有檔案
            List<string> filePathList = GetAllFiles(sourceDirectory);

            foreach (var filePath in filePathList)
            {
                bool isUTF8 = IsUTF8Encoded(filePath);
                if (isUTF8 == false)
                {
                    Console.WriteLine(filePath);

                    string content = File.ReadAllText(filePath, Encoding.GetEncoding("Big5"));
                    //把來源路徑改成輸出路徑,得到新檔案路徑
                    string new_filePath = filePath.Replace(sourceDirectory, destinationDirectory);
                    //寫入文字內容 並帶有UTF8 BOM
                    WriteToFileWithUtf8Bom(new_filePath, content);

                }
            }
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
        /// 判斷big5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static bool IsBig5Encoded(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            try
            {
                // 嘗試使用 Big5 編碼來解碼
                Encoding big5 = Encoding.GetEncoding("Big5");
                string decodedString = big5.GetString(fileBytes);

                // 如果解碼成功且不會拋出異常，則可能是 Big5 編碼
                return true;
            }
            catch (DecoderFallbackException)
            {
                // 如果解碼時發生異常，則不是 Big5 編碼
                return false;
            }
        }

        static bool IsUTF8Encoded(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            try
            {
                // 嘗試使用 UTF-8 編碼來解碼
                Encoding utf8 = Encoding.UTF8;
                string decodedString = utf8.GetString(fileBytes);

                // 檢查 UTF-8 特殊字節標記（如果存在 BOM）
                if (fileBytes.Length >= 3 &&
                    fileBytes[0] == 0xEF &&
                    fileBytes[1] == 0xBB &&
                    fileBytes[2] == 0xBF)
                {
                    return true;
                }

                // 如果解碼成功且不會拋出異常，則可能是 UTF-8 編碼
                return false;
            }
            catch (DecoderFallbackException)
            {
                // 如果解碼時發生異常，則不是 UTF-8 編碼
                return false;
            }
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
