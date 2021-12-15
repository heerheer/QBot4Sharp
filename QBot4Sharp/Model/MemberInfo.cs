using System.Text.Json.Serialization;

namespace QBot4Sharp.Model;

/// <summary>
/// 成员信息
/// </summary>
public class MemberInfo
{
    /// <summary>
    /// 用户基础信息，来自QQ资料，只有成员相关接口中会填充此信息
    /// </summary>
    [JsonPropertyName("user")]
    public UserInfo User { get; set; }

    /// <summary>
    /// 用户在频道内的昵称
    /// </summary>
    [JsonPropertyName("nick")]
    public string Nick { get; set; }

    /// <summary>
    /// 用户在频道内的身份组ID, 默认值可参考DefaultRoles
    /// </summary>
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; }

    /// <summary>
    /// 用户加入频道的时间
    /// </summary>
    [JsonPropertyName("joined_at")]
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// 频道id
    /// 只会在GUILD_MEMBER事件中引发
    /// </summary>
    [JsonPropertyName("guild_id	")]
    public string GuildId { get; set; }
}