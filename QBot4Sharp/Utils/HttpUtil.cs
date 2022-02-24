﻿using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using QBot4Sharp.Model.Messages;

namespace QBot4Sharp.Utils
{
    public class HttpUtil
    {
        public record OpenApiResult(string RespJson, string? TraceId);

        public static void Post(string url, string json)
        {
            using (var client = new HttpClient())
            {
                client.PostAsync(url, new StringContent(json));
            }
        }

        public static string PostWithAuth(string url, string json, string auth)
        {
            using (var client = new HttpClient())
            {
                var c = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", auth);

                var resp = client.PostAsync(url, c).Result.Content.ReadAsStringAsync();
                //BotCore.DebugLog(resp.Result);
                return resp.Result;
            }
        }

        public static string DeleteWithAuth(string url, string auth)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", auth);

                var resp = client.DeleteAsync(url).Result.Content.ReadAsStringAsync();
                return resp.Result;
            }
        }

        public static string GetWithAuth(string url, string auth)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", auth);

                var resp = client.GetAsync(url).Result.Content.ReadAsStringAsync();
                return resp.Result;
            }
        }

        public static Task<Stream> GetWithAuthAsync(string url, string auth)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", auth);

                var resp = client.GetAsync(url).Result.Content.ReadAsStreamAsync();
                return resp;
            }
        }

        public async static Task<OpenApiResult> PostWithAuthAsync(string url, string json, string auth)
        {
            using (var client = new HttpClient())
            {
                var c = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", auth);

                var resp = await client.PostAsync(url, c);


                //BotCore.DebugLog(resp.Result);
                return new(await resp.Content.ReadAsStringAsync(),
                    resp.Headers.GetValues("X-Tps-trace-ID").ToArray().FirstOrDefault());
            }
        }
    }
}