using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QBot4Sharp.Model.Messages
{
    public class QBotMessageSend
    {
        [JsonPropertyName("content")] public string Content { get; set; }

        [JsonPropertyName("msg_id")] public string ReplyMsgId { get; set; }

        [JsonPropertyName("image")] public string ImageUrl { get; set; } = "";

        /// <summary>
        /// 要回复的消息id(Message.id), 在 AT_CREATE_MESSAGE 事件中获取
        /// 带了 msg_id 视为被动回复消息，否则视为主动推送消息
        /// </summary>
        [JsonPropertyName("ark")] public string ArkMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    
    /// <summary>
    /// Ark消息类
    /// </summary>
    public class MessageArk
    {
        [JsonPropertyName("template_id")]
        public int TemplateId { get; set; }

        [JsonPropertyName("kv")]
        public List<MessageArkKv> KvList { get; set; } = new();

        public class MessageArkKv
        {
            public string key { get; set; }
            public string value { get; set; }
            //public object obj { get; set; }

        }
            
            
    }
}