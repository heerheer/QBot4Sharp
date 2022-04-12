using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace QBot4Sharp.Model.Messages
{
    public class QBotMessageSend
    {
        /// <summary>
        /// 选填，要回复的消息id(Message.id), 在 AT_CREATE_MESSAGE 事件中获取。
        /// </summary>
        [JsonPropertyName("msg_id")]
        public string? ReplyMsgId { get; set; }

        /// <summary>
        /// 选填，消息内容，文本内容，支持内嵌格式
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }


        /// <summary>
        /// 选填，图片url地址，平台会转存该图片，用于下发图片消息
        /// </summary>
        [JsonPropertyName("image")]
        public string? ImageUrl { get; set; } = "";

        /// <summary>
        /// 要回复的消息id(Message.id), 在 AT_CREATE_MESSAGE 事件中获取
        /// 带了 msg_id 视为被动回复消息，否则视为主动推送消息
        /// </summary>
        [JsonPropertyName("ark")]
        public MessageArk? ArkMessage { get; set; }

        /// <summary>
        /// 选填，引用消息
        /// </summary>
        [JsonPropertyName("message_reference")]
        public MessageReference? Reference { get; set; }

        /// <summary>
        /// 选填，embed 消息，一种特殊的 ark，详情参考Embed消息
        /// </summary>
        [JsonPropertyName("embed")]
        public MessageEmbed? EmbedMessage { get; set; }


        /// <summary>
        /// 选填，markdown 消息
        /// </summary>
        [JsonPropertyName("markdown")]
        public MessageMarkdown? Markdown { get; set; }

        /// <summary>
        /// 获取Json文本内容
        /// </summary>
        /// <returns></returns>
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