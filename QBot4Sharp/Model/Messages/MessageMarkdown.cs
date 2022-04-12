using System.Text.Json.Serialization;


namespace QBot4Sharp.Model.Messages;

public class MessageMarkdown
{
    /// <summary>
    /// markdown 模板 id
    /// </summary>
    [JsonPropertyName("template_id")]
    public int? TemplateId { get; set; }

    /// <summary>
    /// markdown 模板模板参数
    /// </summary>
    [JsonPropertyName("params")]
    public MessageMarkdownParams? Params { get; set; }

    /// <summary>
    /// 原生 markdown 内容,与 template_id 和 params参数互斥,参数都传值将报错。
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

public class MessageMarkdownParams
{
    /// <summary>
    /// markdown 模版 key
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }

    /// <summary>
    /// markdown 模版 key 对应的 values ，列表长度大小为 1 代表单 value 值，长度大于1则为列表类型的参数 values 传参数
    /// </summary>
    [JsonPropertyName("values")]
    public List<string> Content { get; set; }
}