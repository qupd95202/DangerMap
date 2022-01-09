using Nancy.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DangerMap
{
    public class Global
    {
        /// <summary>
        /// 能使用於 AccountID 和密碼的字元, 字元限制 6~12 & 不能出現以下提到之外的字元
        /// </summary>
        public static readonly Regex LEGAL_ID_PASSWORD = new Regex(@"^[a-zA-Z0-9_]{6,12}$");

        /// <summary>
        /// 留言禁止字元
        /// </summary>
        public static readonly Regex ILLEGAL_WORD = new Regex("{[(賤婊)|(低能兒)|(狗屎爛蛋)|(line)]}");

        /// <summary>
        /// 限制留言字元數量
        /// </summary>
        public static readonly int COMMENT_LENGTH = 50;

        /// <summary>
        /// 限制事件標題字元數量
        /// </summary>
        public static readonly int TITLE_LENGTH = 50;

        /// <summary>
        /// 限制地點描述字元數量
        /// </summary>
        public static readonly int LOCATION_LENGTH = 50;

        /// <summary>
        /// 限制事件描述字元數量
        /// </summary>
        public static readonly int EVENT_DETAIL_LENGTH = 200;

        /// <summary>
        /// 搜尋範圍 (經緯度轉換)
        /// </summary>
        public static readonly double SEARCH_RANGE_DOUBLE = 0.01d;

        ///// <summary>
        ///// 簡單字串加密 (MD5)
        ///// </summary>
        ///// <param name="strs">密碼</param>
        ///// <returns>加密後之字串</returns>
        public static string ToMD5(string encodeStr)
        {
            using (var cryptoMD5 = System.Security.Cryptography.MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(encodeStr);
                var hash = cryptoMD5.ComputeHash(bytes);
                var md5 = BitConverter.ToString(hash)
                    .Replace("-", String.Empty);
                return md5.ToLower();
            }
        }

        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="mailReceiver">接收者信箱</param>
        /// <param name="sender">寄信者名字</param>
        /// <param name="subject">信件標題</param>
        /// <param name="body">信件內容</param>
        public static void SendMail(string mailReceiver, string sender, string subject, string body)
        {
            using (MailMessage msg = new MailMessage())
            {
                //收件者，以逗號分隔不同收件者 ex "test@gmail.com,test2@gmail.com"
                msg.To.Add(mailReceiver);
                msg.From = new MailAddress("email@gmail.com", sender, System.Text.Encoding.UTF8);
                //郵件標題 
                msg.Subject = subject;
                //郵件標題編碼  
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                //郵件內容
                msg.Body = body;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                msg.Priority = MailPriority.Normal;//郵件優先級 

                //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                #region 其它 Host
                /*
                 *  outlook.com smtp.live.com port:25
                 *  yahoo smtp.mailReceiver.yahoo.com.tw port:465
                */
                #endregion
                using (SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    //設定你的帳號密碼
                    MySmtp.Credentials = new System.Net.NetworkCredential("DangerMap2022@gmail.com", "ourdangermap2022");
                    //Gmial 的 smtp 使用 SSL
                    MySmtp.EnableSsl = true;
                    MySmtp.Send(msg);
                }
            }
        }

        /// <summary>
        /// 推播發送消息
        /// </summary>
        /// <param name="message">推播內容</param>
        /// <param name="title">推播標題</param>
        /// <param name="id">裝置id</param>
        /// <returns></returns>
        public static string SendNotification(string message, string title, string id)
        {
            string SERVER_API_KEY = "AAAAdEnBsrk:APA91bHQvda7hfit7ss3J5lfVfLxzuGq2c8dq5fGrIxsy_wqYxD5s-YkK72rHWYiOuCXFLZiCsohV4E6qpyRJZE5Vp08yqTa49PxSv_phbY1-yc9Po3kXND4M3kBqMDPR5L5kRUlY60Q";
            var SENDER_ID = 499453637305;

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";


            string deviceId = id;/*"/topics/marketing"*/
            var data = new
            {
                to = deviceId,
                notification = new
                {
                    body = message,
                    title = title,
                    sound = "Enabled"
                }
            };
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            tRequest.ContentLength = byteArray.Length;
            //return tRequest.Headers.ToString();
            tRequest.Headers.Add(string.Format("project_id:{0}", SENDER_ID));
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }

        /// <summary>
        /// 推播發送消息(廣播)
        /// </summary>
        /// <param name="message">推播內容</param>
        /// <param name="title">推播標題</param>
        /// <param name="id">裝置id</param>
        /// <returns></returns>
        public static string SendNotification(string message, string title, string[] ids)
        {
            string SERVER_API_KEY = "AAAAdEnBsrk:APA91bHQvda7hfit7ss3J5lfVfLxzuGq2c8dq5fGrIxsy_wqYxD5s-YkK72rHWYiOuCXFLZiCsohV4E6qpyRJZE5Vp08yqTa49PxSv_phbY1-yc9Po3kXND4M3kBqMDPR5L5kRUlY60Q";
            var SENDER_ID = 499453637305;

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";


            var data = new
            {
                registration_ids = ids,
                notification = new
                {
                    body = message,
                    title = title,
                    sound = "Enabled",
                }
            };
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            tRequest.ContentLength = byteArray.Length;
            //return tRequest.Headers.ToString();
            tRequest.Headers.Add(string.Format("project_id:{0}", SENDER_ID));
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }

        /// <summary>
        /// 想搜尋之公尺轉換
        /// </summary>
        /// <param name="meter">想搜尋的範圍</param>
        /// <returns>經緯度約莫是差多少度</returns>
        public static double MeterChange2Tude(double meter)
        {
            return meter * 0.00001;
        }
    }
}