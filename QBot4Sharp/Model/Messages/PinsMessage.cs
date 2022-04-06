using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace QBot4Sharp.Model.Messages;

public class PinsMessage
{
    /// <summary>
    /// 频道 id
    /// </summary>
    [JsonPropertyName("guild_id")]
    public string GuildId { get; set; }

    /// <summary>
    /// 子频道 id
    /// </summary>
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }

    /// <summary>
    /// 子频道内精华消息 id 数组
    /// </summary>
    [JsonPropertyName("message_ids")]
    public string[] MessageIds { get; set; }
}