# QBot4Sharp
A QBot SDK for .Net Developer

一个用于QQ频道Bot开发的.Net类库。

## 关于
感谢腾讯开放频道API!!!

终于可以合理使用机器人了!!!呜呜呜!!!

## 如何使用

### 1. Nuget安装类库

`ssss`

### 2. 创建入口代码

```c#
string MyToken = "abcd";
string AppId = "1234";
var core = new BotCore(AppId, MyToken);
```
### 3. 监听事件
```c#
core.On_AT_MESSAGE_CREATE += (core, msg) =>
{
    Console.WriteLine("收到消息啦");
    core.Api.ReplyMessage(msg.channel_id, new QBotMessageSend() { content = "你的意思是,"+msg.content+"吗?",msg_id = msg.id });
};
```

### 4. 启动Wss连接
```c#
core.StartWssConnection();
```
