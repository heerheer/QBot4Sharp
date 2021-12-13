# QBot4Sharp

A QBot SDK for .Net Developer

一个用于QQ频道Bot开发的.Net类库。

## 关于

感谢腾讯开放频道API!!!

终于可以合理使用机器人了!!!呜呜呜!!!

## 如何使用

### 1. Nuget安装类库

`Install-Package QBot4Sharp -Version 1.0.0-alpha`

### 2. 创建入口代码

```c#
string MyToken = "abcd";
string AppId = "1234";
var core = new BotCore(AppId, MyToken);
```

### 3. 监听事件

```c#
core.On_AT_MESSAGE_CREATE += (c, msg) =>
{
    Console.WriteLine("收到消息啦");
};

core.Intents = (long)QBotIntents.AT_MESSAGES;

//监听多个事件请使用 或(|) 运算
//core.Intents = (long)QBotIntents.AT_MESSAGES |(long)QBotIntents.GUILDS;
```

### 4. 启动Wss连接

```c#
core.StartWssConnection();
```

### 5. 如何回复消息

```c#
core.On_AT_MESSAGE_CREATE += (c, msg) =>
{
    var message = msg.GetMessage(core.BotId);//获取去掉开头<@!>的文本
    
    if (message == "回我消息")
    {
        core.Api.SendMessage(
        msg.ChannelId,
        msg.CreateReplyMessage("回你啦"));
        //使用msg.CreateReplyMessage快速创建文本消息回复对象
        //当然你也可以选择QBotMessageSend.CreateReplyMsg(msgId,content)
    }
}
```

## 更新历史

v 1.0.1 2021-12-13
1. 增加了GuildInfo和MemberInfo获取的API
2. 将1.0.0的Author修改为UserInfo结构
3. 允许通过core.Intents (QBotIntents枚举类) 修改监听事件
4. 删掉了OpCode_ReadyEvent这个奇怪的类。现在它变成了OpCodeReadyEventContent！
5. LinkMessageTemplateGenerator妄图成为Ark构造器，但是似乎赫尔没有Ark发送的力量...
6. 增加了注释，减少了期末考试的复习时间。（土木工程施工为什么感觉好多。）
7. 现在似乎可以在SDK里面发现一只不会赛马的小马！
