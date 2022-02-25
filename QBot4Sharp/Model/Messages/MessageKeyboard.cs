using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace QBot4Sharp.Model.Messages;

public class MessageKeyboard
{
    /// <summary>
    /// 消息键盘的模板ID，请联系平台申请，申请后平台人员会提供id给到开发者
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}