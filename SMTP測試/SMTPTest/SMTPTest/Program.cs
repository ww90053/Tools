using System;
using System.Configuration; // 使用 System.Configuration 命名空間來讀取 app.config 中的設定
using System.Net;
using System.Net.Mail;

class Program
{
    static void Main()
    {
        try
        {
            // 從設定檔中讀取參數
            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"]; // SMTP 伺服器地址
            string port = ConfigurationManager.AppSettings["port"];            
            string senderEmail = ConfigurationManager.AppSettings["SenderEmail"]; // 發件人的電子郵件地址
            string senderPassword = ConfigurationManager.AppSettings["SenderPassword"]; // 發件人的郵箱密碼
            string recipientEmail = ConfigurationManager.AppSettings["RecipientEmail"]; // 收件人的電子郵件地址

            // 設定 SMTP 伺服器地址和端口
            SmtpClient client = new SmtpClient(smtpServer, Convert.ToInt32(port));

            // 設定發件人的郵箱地址和密碼
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // 創建郵件
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(senderEmail);
            mailMessage.To.Add(recipientEmail);
            mailMessage.Subject = "測試郵件"; // 郵件主題
            mailMessage.Body = "這是一封測試郵件。"; // 郵件正文

            // 發送郵件
            client.Send(mailMessage);

            Console.WriteLine("郵件發送成功。");
        }
        catch (Exception ex)
        {
            Console.WriteLine("錯誤：" + ex.Message);
        }
    }
}
