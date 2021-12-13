using System.Text.Json.Serialization;

namespace QBot4Sharp.Model
{
    /// <summary>
    /// 频道信息，是频道！不是子频道。
    /// </summary>
    public class GuildInfo
    {
        /// <summary>
        /// 频道ID
        /// </summary>
        [JsonPropertyName("id")] public string GuildId { get; set; }
        /// <summary>
        /// Guild名称
        /// </summary>
        [JsonPropertyName("name")] public string Name { get; set; }
        /// <summary>
        /// 频道头像地址
        /// </summary>
        [JsonPropertyName("icon")] public string IconUrl { get; set; }
        
        [JsonPropertyName("owner_id")] public string OwnerId { get; set; }
        /// <summary>
        /// 当前人是否是创建人
        /// </summary>
        [JsonPropertyName("owner")] public bool UserIsOwner { get; set; }
        /// <summary>
        /// 成员数量
        /// </summary>
        [JsonPropertyName("member_count")] public int MemberCount { get; set; }
        /// <summary>
        /// 最大成员数
        /// </summary>
        [JsonPropertyName("max_members")] public int MaxMembers { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [JsonPropertyName("description")] public string Description { get; set; }
        /// <summary>
        /// 用户加入事件
        /// </summary>
        [JsonPropertyName("joined_at")] public string UserJoinedTime { get; set; }
    }
}