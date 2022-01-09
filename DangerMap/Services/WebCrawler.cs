using DangerMap.Models.db;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class WebCrawler
    {
        public  string GetData()
        {
            HttpWebRequest request = null;

            List<TrafficAccident> list = new List<TrafficAccident>();

            // 建立 WebRequest 並指定目標的 url
            request = (HttpWebRequest)WebRequest.Create("https://od.moi.gov.tw/api/v1/rest/datastore/A01010000C-000499-193");
            // 將 HttpWebRequest 的 Method 屬性設置為 GET
            request.Method = "GET";
            // API回傳的字串
            string result = string.Empty;
            int index = 0;

            // 發出Request
            using (WebResponse response = request.GetResponse())
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                while (!sr.EndOfStream)
                {
                    result += sr.ReadLine();
                }
                sr.Close();
            }
            return result;
        }

        public JToken Parse_data(string data)
        {

            //讀數組型json {"aaa":[{'a':'a1','b':'b1'},{'a':'a2','b':'b2'}]};

            JObject jObject = JObject.Parse(data);

            var data1 = jObject;

            //foreach (JObject item in data1)
            //{
            //    *** = item["a"]; //即可得到a1,a2，作後續使用，使用item["**"].ToString()可以轉爲string
            //    如果有其他來源的數據我想整合到Json的每一條數據中可以試用這個方法爲Jobject添加項
            //    var newJsonObj = JObject.Parse("{\"項的名字\":\"" + 項的值 + "\",\"項2的名字\":\"" + 項2的值);
            //    item.Add(newJsonObj.Properties());
            //}

            ////直接讀Json字符串
            //JsonReader reader = new JsonTextReader(new StringReader(data));
            //while (reader.Read())
            //{
            //    Console.WriteLine(reader.Value + "\r\n");
            //}

            return data1;
        }
    }
}
