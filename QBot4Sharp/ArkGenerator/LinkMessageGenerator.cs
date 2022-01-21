using QBot4Sharp.Model.Messages;

namespace QBot4Sharp.ArkGenerator;

/// <summary>
/// 用于创建模板消息申请
/// <para></para>
/// Link:https://bot.q.qq.com/wiki/develop/api/openapi/message/template/template_23.html
/// </summary>
public class LinkMessageGenerator : IArkGenerator
{
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 提示消息
    /// </summary>
    public string Prompt { get; set; }

    private QBotMessageSend _messageSend = new();

    private List<MessageArk.MessageArkKvObject> _lines = new();

    public LinkMessageGenerator(string desc, string prompt)
    {
        Desc = desc;
        Prompt = prompt;
    }

    /// <summary>
    /// 对Link中列表内添加一行文字
    /// </summary>
    /// <param name="desc"></param>
    public void AddLine(string desc)
    {
        _lines.Add(new()
        {
            KvList = new()
            {
                new MessageArk.MessageArkKv("desc", desc),
            }
        });
    }

    /// <summary>
    /// 对Link内添加一行Link(特殊格式。)
    /// </summary>
    /// <param name="desc"></param>
    /// <param name="link"></param>
    public void AddLine(string desc, string link)
    {
        _lines.Add(new()
        {
            KvList = new()
            {
                new MessageArk.MessageArkKv("desc", desc),
                new MessageArk.MessageArkKv("link", link),
            }
        });
    }

    public QBotMessageSend Message => _messageSend;

    public MessageArk GetArkMessage()
    {
        var ark = new MessageArk() { TemplateId = 23, KvList = new() };

        ark.KvList.Add(new("#DESC#", Desc));
        ark.KvList.Add(new("#PROMPT#", Prompt));
        ark.KvList.Add(new("#LIST#"));
        ark.KvList.First(x => x.key == "#LIST#").obj = _lines;

        return ark;
    }
}