using System.Text.Json.Serialization;

// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace QBot4Sharp.Model;

/// <summary>
/// 私信会话对象
/// </summary>
public class QBotDms
{
    [JsonPropertyName("guild_id")] public string GuildId { get; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; }
    [JsonPropertyName("create_time")] public string CreateTime { get; }
}