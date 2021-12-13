using QBot4Sharp.Model.Messages;

namespace QBot4Sharp;
/// <summary>
/// 用于创建模板消息申请
/// </summary>
public class LinkMessageTemplateGenerator
{
    private QBotMessageSend _messageSend = new();

    public LinkMessageTemplateGenerator()
    {
        _messageSend.ArkMessage = new(){TemplateId = 23,KvList = new()};
        
        _messageSend.ArkMessage.KvList.Add(new ("#DESC#","测试内容"));
        _messageSend.ArkMessage.KvList.Add(new ("#PROMPT#","测试提示消息"));
        _messageSend.ArkMessage.KvList.Add(new("#LIST#")
        {
            obj = new ()
            {
                new()
                {
                    KvList = new()
                    {
                        new("desc","需求标题：UI问题解决")
                    }
                },
                new()
                {
                    KvList = new()
                    {
                        new("desc","已排期")
                    }
                },
                new()
                {
                KvList = new()
                {
                new("desc","增量测试中")
            }
        }
            }
            
        });
        
        
        
        
    }

    public QBotMessageSend Message => _messageSend;
}