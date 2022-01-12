using System.Text.Json.Serialization;

namespace QBot4Sharp.Model.Messages;

public class QBotMessageEmbed
{
    public QBotMessageEmbed(string title, string prompt)
    {
        Title = title;
        Prompt = prompt;
    }

    /// <summary>
    /// 标题
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// 消息弹窗内容
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    /// <summary>
    /// 缩略图
    /// </summary>
    [JsonPropertyName("thumbnail")]
    public MessageEmbedThumbnail Thumbnail { get; set; }

    /// <summary>
    /// 消息创建时间
    /// </summary>
    [JsonPropertyName("fields")]
    public List<MessageEmbedField> Field { get; set; }

    public class MessageEmbedThumbnail
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class MessageEmbedField
    {
        public MessageEmbedField(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}