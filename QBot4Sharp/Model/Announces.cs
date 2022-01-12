using System.Text.Json.Serialization;

namespace QBot4Sharp.Model;

public class Announces
{
    [JsonPropertyName("guild_id")] public string GuildId { get; set; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; set; }
    [JsonPropertyName("message_id")] public string MessageId { get; set; }
}