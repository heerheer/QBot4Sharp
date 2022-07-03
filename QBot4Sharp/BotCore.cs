using System.Text.Json;
using QBot4Sharp.Model;
using QBot4Sharp.Model.Messages;
using Websocket.Client;

#pragma warning disable CS0067

namespace QBot4Sharp;

public class BotCore
{
    const string WssUrlSandbox = "wss://sandbox.api.sgroup.qq.com/websocket";

    const string WssUrlPublic = "wss://api.sgroup.qq.com/websocket";

    private readonly string _token;
    private static bool _debug;
    private readonly string _appId;
    public int S2d; //保存下行消息得到的index？
    public WebsocketClient WebSocket;
    public int HeartbeatInterval;

    public string BotId = "";


    private string _session = "";

    private bool _resuming;

    private Timer? _heartbeatTimer;

    /// <summary>
    /// 用于调用API
    /// </summary>
    public BotApi Api;

    /// <summary>
    /// 需要申请的事件
    /// </summary>
    public IntentsConfig IntentsConfig { get; set; } = new();

    /// <summary>
    /// Websocket状态
    /// </summary>
    public bool Connected { get; set; } = false;

    #region Events

    public delegate void OriginEventHandler(BotCore core, BotOpCodeBase opCode);

    public delegate void MessageEventHandler(BotCore botCore, QBotMessage? message);

    public delegate void GuildEventHandler(BotCore botCore, GuildInfo? message);

    public delegate void GuildMemberEventHandler(BotCore botCore, MemberInfoWithGildId? message);


    /// <summary>
    /// OpCode为0时，服务端进行消息推送事件(未细分)
    /// </summary>
    public event OriginEventHandler? ON_DISPATCH;


    // ------ GUILDS (1 << 0)

    /// <summary>
    /// 当机器人加入新guild时
    /// </summary>
    public event GuildEventHandler? GUILD_CREATE;

    /// <summary>
    /// 当guild资料发生变更时
    /// </summary>
    public event GuildEventHandler? GUILD_UPDATE;

    /// <summary>
    /// 当机器人退出guild时
    /// </summary>
    public event GuildEventHandler? GUILD_DELETE;


    /// <summary>
    /// 当channel被创建时
    /// </summary>
    public event GuildEventHandler? CHANNEL_CREATE;

    /// <summary>
    /// 当channel被更新时
    /// </summary>
    public event GuildEventHandler? CHANNEL_UPDATE;

    /// <summary>
    /// 当channel被删除时
    /// </summary>
    public event GuildEventHandler? CHANNEL_DELETE;


    // ------- GUILD_MEMBERS (1 << 1)

    /// <summary>
    /// 新用户加入频道
    /// </summary>
    public event GuildMemberEventHandler? GUILD_MEMBER_ADD;

    /// <summary>
    /// 当成员资料变更时
    /// </summary>
    public event GuildMemberEventHandler? GUILD_MEMBER_UPDATE;

    /// <summary>
    /// 用户离开频道
    /// </summary>
    public event GuildMemberEventHandler? GUILD_MEMBER_REMOVE;


    // ------ GUILD_MESSAGE_REACTIONS

    //TODO
    public event EventHandler? MESSAGE_REACTION_ADD;

    public event EventHandler? MESSAGE_REACTION_REMOVE;


    // ------ DIRECT_MESSAGE (1 << 12)

    /// <summary>
    /// 当收到用户发给机器人的私信消息时
    /// </summary>
    public event MessageEventHandler? DIRECT_MESSAGE_CREATE;

    /// <summary>
    /// 删除（撤回）消息事件
    /// </summary>
    public event MessageEventHandler? DIRECT_MESSAGE_DELETE;

    // ------ INTERACTION (1 << 12)
    //TODO
    public event EventHandler? INTERACTION_CREATE;


    // -------PUBLIC_GUILD_MESSAGES (1 << 30) // 消息事件，此为公域的消息事件

    /// <summary>
    /// 当收到@机器人的消息时
    /// </summary>
    public event MessageEventHandler? AT_MESSAGE_CREATE;

    /// <summary>
    /// 当频道的消息被删除时
    /// </summary>
    public event MessageEventHandler? PUBLIC_MESSAGE_DELETE;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="token"></param>
    /// <param name="isSandBoxMode">是否是沙箱模式</param>
    /// <param name="debug">是否进行debug输出</param>
    public BotCore(string appId, string token, bool isSandBoxMode = true, bool debug = false)
    {
        _appId = appId;
        _token = token;
        _debug = debug;
        Api = new BotApi(appId, token, isSandBoxMode);

        var wssUrl = isSandBoxMode ? WssUrlSandbox : WssUrlPublic;

        WebSocket = new WebsocketClient(new Uri(wssUrl)); //创建ws客户端
    }


    public void StartWssConnection()
    {
        WebSocket.DisconnectionHappened.Subscribe(x => { Connected = false; });
        WebSocket.MessageReceived.Subscribe(msg =>
        {
            try
            {
                var msgObj = JsonSerializer.Deserialize<BotOpCodeBase>(msg.Text) ?? new();
                S2d = msgObj.CodeId;
                if (msgObj.OpCode == 10)
                {
                    //Code为10是当客户端与网关建立ws连接之后，网关下发的第一条消息
                    DebugLog("[Wss]收到初始化消息");
                    //获取HeartBeat间隔
                    HeartbeatInterval = BotOpCode.Get_Heartbeat_interval(msg.Text);
                    if (_session != "")
                    {
                        TryResume();
                    }
                    else
                    {
                        SendOpCode2Identify();
                    }
                }
                else if (msgObj.OpCode == 11)
                {
                    //11为当发送心跳成功之后，就会收到该消息。
                    DebugLog($"[{DateTime.Now.ToShortTimeString()}][Wss]心跳包接触OK");
                }
                else if (msgObj.OpCode == 0)
                {
                    ON_DISPATCH?.Invoke(this, msgObj);
                    var eventType = msgObj.EventType;
                    /*
                    var eventInfo = this.GetType().GetEvent(eventType);
                    if (eventInfo != null)
                    {
                       var paramType = eventInfo.EventHandlerType?.GetMethod("Invoke")?.GetParameters()[1].ParameterType;
                       eventInfo.GetRaiseMethod().Invoke(),
                    }*/

                    if (eventType == "DIRECT_MESSAGE_CREATE")
                    {
                        //当收到用户发给机器人的私信消息时
                        DIRECT_MESSAGE_CREATE?.Invoke(this,
                            JsonSerializer.Deserialize<QBotMessage>(((JsonElement)msgObj.EventContent).ToString()));
                    }

                    if (eventType == "DIRECT_MESSAGE_DELETE")
                    {
                        //当收到用户发给机器人的私信消息时
                        DIRECT_MESSAGE_DELETE?.Invoke(this,
                            JsonSerializer.Deserialize<QBotMessage>(((JsonElement)msgObj.EventContent).ToString()));
                    }


                    //服务端进行消息推送
                    if (eventType == "AT_MESSAGE_CREATE")
                    {
                        AT_MESSAGE_CREATE?.Invoke(this,
                            JsonSerializer.Deserialize<QBotMessage>(
                                ((JsonElement)msgObj.EventContent).ToString()
                            )
                        );
                    }

                    //Guild相关事件
                    if (eventType is "GUILD_CREATE" or "GUILD_UPDATE" or "GUILD_DELETE")
                    {
                        var guildInfo =
                            JsonSerializer.Deserialize<GuildInfo>(((JsonElement)msgObj.EventContent).ToString());
                        switch (eventType)
                        {
                            case "GUILD_CREATE":
                                GUILD_CREATE?.Invoke(this, guildInfo);
                                break;

                            case "GUILD_UPDATE":
                                GUILD_UPDATE?.Invoke(this, guildInfo);
                                break;

                            case "GUILD_DELETE":
                                GUILD_DELETE?.Invoke(this, guildInfo);
                                break;
                        }
                    }

                    //GuildMember相关事件
                    if (eventType.StartsWith("GUILD_MEMBER"))
                    {
                        var memberInfo =
                            JsonSerializer.Deserialize<MemberInfoWithGildId>(
                                ((JsonElement)msgObj.EventContent).ToString());
                        switch (eventType)
                        {
                            case "GUILD_MEMBER_ADD":
                                GUILD_MEMBER_ADD?.Invoke(this, memberInfo);
                                break;

                            case "GUILD_MEMBER_UPDATE":
                                GUILD_MEMBER_UPDATE?.Invoke(this, memberInfo);
                                break;

                            case "GUILD_MEMBER_REMOVE":
                                GUILD_MEMBER_REMOVE?.Invoke(this, memberInfo);
                                break;
                        }
                    }

                    if (eventType == "READY")
                    {
                        //鉴权成功
                        DebugLog("[Wss]鉴权成功");
                        var botReadyContent = JsonSerializer
                            .Deserialize<OpCodeReadyEventContent>(((JsonElement)msgObj.EventContent).ToString());
                        BotId = botReadyContent
                            ?.User.id ?? "";
                        _session = botReadyContent?.SessionId ?? "";
                        //鉴权成功后开始建立心跳包
                        StartHeartbeat();
                        Log("鉴权完成，建立心跳包。", 1);
                    }

                    if (eventType == "RESUMED")
                    {
                        DebugLog("重连补发完成");
                        _resuming = false;
                        StartHeartbeat();
                    }
                }
                else if (msgObj.OpCode == 9)
                {
                    if (_resuming)
                    {
                        DebugLog("Resume错误,清空session");
                        _session = "";
                    }
                    else
                    {
                        Log("连接:OpCode9错误...", 3);
                    }
                }
                else
                {
                }
            }
            catch (Exception e)
            {
                Log(e, 3);
            }
        });
        WebSocket.Start();
        Connected = true;
    }


    private void StartHeartbeat()
    {
        if (_heartbeatTimer != null)
        {
            _heartbeatTimer.Change(-1, 0);
            _heartbeatTimer.Dispose();
        }


        _heartbeatTimer = new Timer(_ =>
        {
            var text = BotOpCode.Gen_OpCode_Heartbeat_Json(S2d);
            DebugLog($"[{DateTime.Now.ToShortTimeString()}][Wss]发送心跳包..." + text);
            WebSocket.Send(text);
        });

        _heartbeatTimer.Change(0, HeartbeatInterval);
    }

    private void SendOpCode2Identify()
    {
        Task.Run(() => { WebSocket.Send(BotOpCode.Gen_OpCode_2_Identify_Json(_appId, _token, IntentsConfig.Value)); });
    }

    private void TryResume()
    {
        if (_session == "")
            return;
        _resuming = true;
        WebSocket.Send(new ResumeOpCode(_token, _session).ToString());
    }

    /// <summary>
    /// Release模式下不会输出
    /// </summary>
    /// <param name="obj"></param>
    public static void DebugLog(object? obj)
    {
        if (_debug)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[Debug]" + obj);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }


    /// <summary>
    /// 输出日志
    /// </summary>
    /// <param name="obj"> 0 正常输出 1 提示信息 2 警告 3 异常 4 错误 </param>
    /// <param name="logType"></param>
    public static void Log(object obj, int logType = 0)
    {
        Console.ForegroundColor = logType switch
        {
            1 => ConsoleColor.Green,
            2 => ConsoleColor.Yellow,
            3 => ConsoleColor.Magenta,
            4 => ConsoleColor.Red,
            _ => Console.ForegroundColor
        };

        Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}]{obj}");

        Console.ForegroundColor = ConsoleColor.White;
    }
}