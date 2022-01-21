using System.Text.Json.Serialization;

namespace QBot4Sharp.Model.Messages;

/// <summary>
/// Ark消息对象，用于填充MessageSend的Ark部分
/// </summary>
public class MessageArk
{
    [JsonPropertyName("template_id")] public int TemplateId { get; set; }

    [JsonPropertyName("kv")] public List<MessageArkKv> KvList { get; set; } = new();

    public class MessageArkKv
    {
        public string key { get; set; }

        public string value { get; set; }

        public List<MessageArkKvObject> obj { get; set; }

        public MessageArkKv(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public MessageArkKv(string key)
        {
            this.key = key;
        }
    }

    public class MessageArkKvObject
    {
        [JsonPropertyName("obj_kv")] public List<MessageArkKv> KvList { get; set; }
    }
}