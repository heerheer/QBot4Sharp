# QBot4Sharp
![](logo.png)

A QBot SDK for .Net Developer

一个用于QQ频道Bot开发的.Net类库。

## 关于

感谢QQ频道开放机器人API!!!

- [频道Bot官方开发文档](https://bot.q.qq.com/wiki)
- [频道Bot开放平台](https://bot.q.qq.com/open/)

## 如何使用

### 1. Nuget安装类库

`Install-Package QBot4Sharp -Version 1.1.2.6

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

//修改IntentsConfig确定鉴权时发送的事件监听需求。
core.IntentsConfig = new() { AtMessagesEvent = true };

//同时可以直接core.IntentsConfig.对应属性 = true; 来确定需要监听什么事件。
```

### 4. 启动Wss连接

```c#
core.StartWssConnection();
```

### 5. 如何回复消息

通过事件传入的QBotMessage类型参数，可以快速获取回复用消息。

使用`msg.CreateReplyMessage();`其中允许传入参数

- 文本类型(用于直接回复文本)
- MessageArk类型(直接填充Ark对象)
- IArkGenerator(无需手动获取Ark生成器生成后的值)

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

### 6. 使用Ark生成器

- TextThumbnailGenerator 文字+缩略图
- LinkMessageGenerator 链接+文本列表
- LargeImgGenerator 大图

```c#
core.AT_MESSAGE_CREATE += (bot, msg) =>
{
    if (message == "aaa")
    {
        const string iurl = "https://server1.heerdev.top/img/ns.png";
        bot.Api.SendMessage(msg.ChannelId, msg.CreateReplyMessage(
            new TextThumbnailGenerator(prompt: "提示", title: "标题-Ark", metaDesc: "巴拉巴拉Meta", subTitle: "巴拉子标题",
                imgUrl: iurl, link: "https://server1.heerdev.top/img/ns.png")
        ));
    }
}

```

## 更新历史

### v1.1.2.6

1.增加GetGuildMessageSetting异步方法。用于获取MessageSetting对象。

### v1.1.2.5

1. Markdown相关API，与一个方便的构造器。
2. 给发消息API增加了原始JSON的Debug模式下输出。

### v1.1.2.4

1. 增加精华消息相关API

### v1.1.2.2(3)

1. 修改MemberInfoWithGuildId类
2. 同步Message对象与文档字段。
3. BotApi类中方法改为Async异步调用，同时可以获取TraceId。（有异步方法的同步API已弃用）
4. 版本:1.1.3 将增加频道身份组相关API封装。

### v1.1.2.2

1. 添加 SendMessageAsync 方法，异步发送消息，同时可以获取 TraceId

### v1.1.2.1

1. 修复DMS对象无法set(JSON反序列化失败)的问题。

### v1.1.2

1. 新增API GetAllGuildListAsync 以获取全部的频道列表。
2. 异步化工程进行中.JPG
3. 获取频道可用权限列表API

### v1.1.1

1. 优化鉴权时事件监听策略IntentsConfig
2. 新增私信事件与相关API,Model.
3. QBotMessage对象新增一个CreateReplyMessage重载方法，可快速创建Embed模板消息。

### v1.1.0

1. BotCore类新增Static函数 Log & DebugLog，用于在DEBUG与RELEASE模式下输出日志。
2. 新增LargeArk模板与TextThumbnailArk模板。
3. QBotMessage的CreateReplyMsg方法新增两个重载。
4. DebugLog与普通日志输出的格式进行了优化。识别度++。

### v1.0.4.6-alpha

1. 更新了之前忘记更新的更新历史.JPEG
2. 增加LinkArk的模板生成器

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

