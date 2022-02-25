using System.Text.Json.Serialization;

// ReSharper disable All
#pragma warning disable CS8618

namespace QBot4Sharp.Model;

public class OpCodeReadyEventContent
{
    [JsonPropertyName("version")] public int Version { get; set; }
    [JsonPropertyName("session_id")] public string SessionId { get; set; }
    [JsonPropertyName("user")] public OpCodeReadyEventUser User { get; set; }
    [JsonPropertyName("shard")] public List<int> Shard { get; set; }

    public class OpCodeReadyEventUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool bot { get; set; }
    }
}