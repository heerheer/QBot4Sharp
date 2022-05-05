using System.Text.Json.Serialization;

namespace QBot4Sharp.Model;

public class MessageSetting
{
    /// <summary>
    /// 是否允许创建私信
    /// </summary>
    /// <returns></returns>
    [JsonPropertyName("disable_create_dm")]
    public bool IsDisableCreateDm { get; set; }

    /// <summary>
    /// 是否允许发主动消息
    /// </summary>
    [JsonPropertyName("disable_push_msg")]
    public bool IsDisablePushMsg { get; set; }

    /// <summary>
    /// 数组	子频道 id 数组
    /// </summary>
    [JsonPropertyName("channel_ids")]
    public string[] Channels { get; set; }

    /// <summary>
    /// 每个子频道允许主动推送消息最大消息条数 
    /// </summary>
    [JsonPropertyName("channel_push_max_num")]
    public uint ChannelPushMaxNum { get; set; }
}