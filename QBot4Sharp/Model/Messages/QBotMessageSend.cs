using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace QBot4Sharp.Model.Messages
{
    public class QBotMessageSend
    {
        [JsonPropertyName("msg_id")] public string? ReplyMsgId { get; set; }
        [JsonPropertyName("content")] public string? Content { get; set; }


        [JsonPropertyName("image")] public string ImageUrl { get; set; } = "";

        /// <summary>
        /// 要回复的消息id(Message.id), 在 AT_CREATE_MESSAGE 事件中获取
        /// 带了 msg_id 视为被动回复消息，否则视为主动推送消息
        /// </summary>
        [JsonPropertyName("ark")]
        public MessageArk? ArkMessage { get; set; }

        [JsonPropertyName("embed")] public MessageEmbed? EmbedMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// 创建一个Reply的指定SendMsg
        /// </summary>
        /// <param name="replyId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static QBotMessageSend CreateReplyMsg(string replyId, string msg)
            => new QBotMessageSend() { ReplyMsgId = replyId, Content = msg };
    }
}