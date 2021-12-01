using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace QBot4Sharp.Utils
{
    public class HttpUtil
    {
        public static void Post(string url,string json)
        {
            using (var client = new HttpClient())
            {
                client.PostAsync(url,new StringContent(json));
            }
        }

        public static void PostWithAuth(string url, string json, string auth)
        {
            using (var client = new HttpClient())
            {
                var c = new StringContent(json,Encoding.UTF8,"application/json");
                client.DefaultRequestHeaders.Add("Authorization",auth);
                
                var resp = client.PostAsync(url,c).Result.Content.ReadAsStringAsync();
                Console.WriteLine(resp.Result);
            }  
        }
    }
}