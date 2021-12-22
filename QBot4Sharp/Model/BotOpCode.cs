using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QBot4Sharp.Model
{
    public class BotOpCodeBase
    {
        [JsonPropertyName("op")] public int OpCode { get; set; }
        [JsonPropertyName("s")] public int CodeId { get; set; }
        [JsonPropertyName("d")] public object EventContent { get; set; } = null;
        [JsonPropertyName("t")] public string EventType { get; set; } = "";

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ResumeOpCode : BotOpCodeBase
    {
        public ResumeOpCode(string token, string session)
        {
            OpCode = 6;
            EventContent = new ResumeContent()
            {
                seq = 1377,
                session_id = session,
                token = token
            };
        }

        public class ResumeContent
        {
            public string token { get; set; }
            public string session_id { get; set; }
            public int seq { get; set; }
        }
    }

    public static class BotOpCode
    {
        /// <summary>
        /// 链接WS后首次收到的消息获取心跳包间隔。
        /// </summary>
        /// <returns></returns>
        public static int Get_Heartbeat_interval(string jsonText)
        {
            var json = JsonDocument.Parse(jsonText);
            return json.RootElement.GetProperty("d").GetProperty("heartbeat_interval").GetInt32();
        }

        /// <summary>
        /// 鉴权需要发送 OpCode 2 Identify 消息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Gen_OpCode_2_Identify_Json(string appid, string token, long intents)
        {
            using (var stream = new MemoryStream())
            {
                using (var jsonWriter = new Utf8JsonWriter(stream))

                {
                    jsonWriter.WriteStartObject();

                    jsonWriter.WriteNumber("op", 2);
                    jsonWriter.WriteStartObject("d");
                    //token
                    jsonWriter.WriteString("token",
                        $"Bot {appid}.{token}");
                    //需要监听的事件
                    jsonWriter.WriteNumber("intents", intents);

                    //OpCode2Identify 分片信息
                    jsonWriter.WriteStartArray("shard");
                    jsonWriter.WriteNumberValue(0);
                    jsonWriter.WriteNumberValue(1);
                    jsonWriter.WriteEndArray();

                    jsonWriter.WriteNull("properties");

                    //OpCode2Identify 属性信息。
                    /*jsonWriter.WriteStartObject("properties");
                    jsonWriter.WriteString("$os", "windows");
                    jsonWriter.WriteString("$browser", "heer_sdk");
                    jsonWriter.WriteString("$device", "heer_sdk");
                    jsonWriter.WriteEndObject();*/


                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                    jsonWriter.Flush();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 生成需要发送的心跳包JSON
        /// </summary>
        /// <param name="s2d">最新消息JSON返回的s部分值</param>
        /// <returns></returns>
        public static string Gen_OpCode_Heartbeat_Json(int s2d)
        {
            using (var stream = new MemoryStream())
            {
                using (var jsonWriter = new Utf8JsonWriter(stream))
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WriteNumber("op", 1);
                    if (s2d == 0)
                    {
                        jsonWriter.WriteNull("d");
                    }
                    else
                    {
                        jsonWriter.WriteNumber("d", s2d);
                    }

                    jsonWriter.WriteEndObject();
                    jsonWriter.Flush();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}