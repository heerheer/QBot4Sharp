using System.Text.Json.Serialization;

namespace QBot4Sharp.Model
{
    public class GuildInfo
    {
        [JsonPropertyName("id")] public string GuildId { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("icon")] public string IconUrl { get; set; }
        [JsonPropertyName("owner_id")] public string OwnerId { get; set; }
        [JsonPropertyName("owner")] public bool UserIsOwner { get; set; }
        [JsonPropertyName("member_count")] public int MemberCount { get; set; }
        [JsonPropertyName("max_members")] public int MaxMembers { get; set; }
        [JsonPropertyName("description")] public int Description { get; set; }
        [JsonPropertyName("joined_at")] public int UserJoinedTime { get; set; }
    }
}