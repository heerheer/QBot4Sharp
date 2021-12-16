# QBot4Sharp

A QBot SDK for .Net Developer

一个用于QQ频道Bot开发的.Net类库。

## 关于

感谢腾讯开放频道API!!!

终于可以合理使用机器人了!!!呜呜呜!!!

## 如何使用

### 1. Nuget安装类库

`Install-Package QBot4Sharp -Version 1.0.3-alpha`

### 2. 创建入口代码

```c#
string MyToken = "abcd";
string AppId = "1234";
var core = new BotCore(AppId, MyToken,false);
//core的第三个参数代表是否开始SandBox模式，默认开启。
```

### 3. 监听事件

```c#
core.AT_MESSAGE_CREATE += (c, msg) =>
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
core.AT_MESSAGE_CREATE += (c, msg) =>
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



### v1.0.3 2021-12-16
1. 更新了一些注释（官方文档怎么突然更新）
2. 增加了一些可监听的event，但实际还没做（
3. 鉴权IntentsEnum中添加了FORUM_EVENT(1<<28)？
4. 允许在实例化BotCore的时候选择是否为沙盒模式。
5. 增加了OpCode未0时传递的原始事件BotCore.OnDispatch(事件参数为原始OpCode，可自行对其进行处理)


### v1.0.2 2021-12-14
1. 增加了Channel子频道获取API

### v1.0.1 2021-12-13
1. 增加了GuildInfo和MemberInfo获取的API
2. 将1.0.0的Author修改为UserInfo结构
3. 允许通过core.Intents (QBotIntents枚举类) 修改监听事件
4. 删掉了OpCode_ReadyEvent这个奇怪的类。现在它变成了OpCodeReadyEventContent！
5. LinkMessageTemplateGenerator妄图成为Ark构造器，但是似乎赫尔没有Ark发送的力量...
6. 增加了注释，减少了期末考试的复习时间。（土木工程施工为什么感觉好多。）
7. 现在似乎可以在SDK里面发现一只不会赛马的小马！

## 交流
![](https://bbs.cf-lol.com/template/wic_random/static/logo.png)
访问[插件船坞](https://bbs.cf-lol.com)论坛，随时发现更多应用。
