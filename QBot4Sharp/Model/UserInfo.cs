using System.Text.Json.Serialization;

namespace QBot4Sharp.Model;

public class UserInfo
{
    /// <summary>
    /// 头像地址
    /// </summary>
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; }

    /// <summary>
    /// 是否是Bot
    /// </summary>
    [JsonPropertyName("bot")]
    public bool IsBot { get; set; }

    /// <summary>
    /// 用户 id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; }
}