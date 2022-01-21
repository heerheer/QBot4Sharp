using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace QBot4Sharp.Model;

public class Announces
{
    [JsonPropertyName("guild_id")] public string GuildId { get; set; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; set; }
    [JsonPropertyName("message_id")] public string MessageId { get; set; }
}