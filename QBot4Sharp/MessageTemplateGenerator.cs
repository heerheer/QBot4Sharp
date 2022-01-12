using QBot4Sharp.Model.Messages;

namespace QBot4Sharp;

/// <summary>
/// 用于创建模板消息申请
/// </summary>
public class LinkMessageTemplateGenerator
{
    private QBotMessageSend _messageSend = new();

    private List<MessageArk.MessageArkKvObject> _lines = new();

    public LinkMessageTemplateGenerator(string desc, string prompt)
    {
        _messageSend.ArkMessage = new() { TemplateId = 23, KvList = new() };

        _messageSend.ArkMessage.KvList.Add(new("#DESC#", desc));
        _messageSend.ArkMessage.KvList.Add(new("#PROMPT#", prompt));
        _messageSend.ArkMessage.KvList.Add(new("#LIST#")
        );
    }

    public QBotMessageSend GenerateMessage()
    {
        _messageSend.ArkMessage.KvList.First(x => x.key == "#LIST#").obj = _lines;
        return _messageSend;
    }

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
}