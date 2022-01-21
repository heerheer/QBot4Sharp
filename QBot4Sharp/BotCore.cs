using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using QBot4Sharp.Model;
using QBot4Sharp.Model.Messages;
using Websocket.Client;

namespace QBot4Sharp
{
    public class BotCore
    {
        const string wssUrl_sandbox = "wss://sandbox.api.sgroup.qq.com/websocket";

        const string wssUrl_public = "wss://api.sgroup.qq.com/websocket";

        string MyToken;
        string AppId;
        public int S2d = 0; //保存下行消息得到的index？
        public WebsocketClient WebSocket;
        public int Heartbeat_Interval;
        public string BotId;


        private string session = "";

        private bool resuming = false;

        private Timer _heartbeatTimer = null;

        /// <summary>
        /// 用于调用API
        /// </summary>
        public BotApi Api;

        /// <summary>
        /// 需要申请的事件
        /// 用 | 表示同时申请
        /// </summary>
        public long Intents { get; set; }

        #region Events

        public delegate void OriginEventHandler(BotCore core, BotOpCodeBase opCode);

        public delegate void MessageEventHandler(BotCore botCore, QBotMessage? message);

        public delegate void GuildEventHandler(BotCore botCore, GuildInfo? message);

        public delegate void GuildMemberEventHandler(BotCore botCore, MemberInfo? message);


        /// <summary>
        /// OpCode为0时，服务端进行消息推送事件(未细分)
        /// </summary>
        public event OriginEventHandler OnDispatch;

        /// <summary>
        /// 收到AT消息事件。
        /// </summary>
        public event MessageEventHandler AT_MESSAGE_CREATE;


        /// <summary>
        /// 当机器人加入新guild时
        /// </summary>
        public event GuildEventHandler GUILD_CREATE;

        /// <summary>
        /// 当guild资料发生变更时
        /// </summary>
        public event GuildEventHandler GUILD_UPDATE;

        /// <summary>
        /// 当机器人退出guild时
        /// </summary>
        public event GuildEventHandler GUILD_DELETE;


        /// <summary>
        /// 当channel被创建时
        /// </summary>
        public event GuildEventHandler CHANNEL_CREATE;

        /// <summary>
        /// 当channel被更新时
        /// </summary>
        public event GuildEventHandler CHANNEL_UPDATE;

        /// <summary>
        /// 当channel被删除时
        /// </summary>
        public event GuildEventHandler CHANNEL_DELETE;


        /// <summary>
        /// 新用户加入频道
        /// </summary>
        public event GuildMemberEventHandler GUILD_MEMBER_ADD;

        /// <summary>
        /// 当成员资料变更时
        /// </summary>
        public event GuildMemberEventHandler GUILD_MEMBER_UPDATE;

        /// <summary>
        /// 用户离开频道
        /// </summary>
        public event GuildMemberEventHandler GUILD_MEMBER_REMOVE;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="myToken"></param>
        /// <param name="isSandBoxMode">是否是沙箱模式</param>
        public BotCore(string appId, string myToken, bool isSandBoxMode = true)
        {
            AppId = appId;
            MyToken = myToken;
            Api = new BotApi(appId, myToken, isSandBoxMode);

            var wssUrl = isSandBoxMode ? wssUrl_sandbox : wssUrl_public;

            WebSocket = new WebsocketClient(new Uri(wssUrl)); //创建ws客户端
        }


        public void StartWssConnection()
        {
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
                        Heartbeat_Interval = BotOpCode.Get_Heartbeat_interval(msg.Text);
                        if (session != "")
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
                        OnDispatch?.Invoke(this, msgObj);
                        var eventType = msgObj.EventType;
                        /*
                        var eventInfo = this.GetType().GetEvent(eventType);
                        if (eventInfo != null)
                        {
                           var paramType = eventInfo.EventHandlerType?.GetMethod("Invoke")?.GetParameters()[1].ParameterType;
                           eventInfo.GetRaiseMethod().Invoke(),
                        }*/


                        //服务端进行消息推送
                        if (eventType == "AT_MESSAGE_CREATE")
                        {
                            AT_MESSAGE_CREATE?.Invoke(this,
                                JsonSerializer.Deserialize<QBotMessage>(
                                    ((JsonElement)msgObj.EventContent).ToString()
                                )
                            );
                        }

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

                        if (eventType.StartsWith("GUILD_MEMBER"))
                        {
                            var memberInfo =
                                JsonSerializer.Deserialize<MemberInfo>(((JsonElement)msgObj.EventContent).ToString());
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
                                ?.user.id ?? "";
                            session = botReadyContent.session_id;
                            //鉴权成功后开始建立心跳包
                            StartHeartbeat();
                            Log("鉴权完成，建立心跳包。", 1);
                        }

                        if (eventType == "RESUMED")
                        {
                            DebugLog("重连补发完成");
                            resuming = false;
                            StartHeartbeat();
                        }
                    }
                    else if (msgObj.OpCode == 9)
                    {
                        if (resuming)
                        {
                            DebugLog("Resume错误,清空session");
                            session = "";
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
        }


        public void StartHeartbeat()
        {
            if (_heartbeatTimer != null)
            {
                _heartbeatTimer.Change(-1, 0);
                _heartbeatTimer.Dispose();
            }


            _heartbeatTimer = new Timer(obj =>
            {
                var text = BotOpCode.Gen_OpCode_Heartbeat_Json(S2d);
                DebugLog($"[{DateTime.Now.ToShortTimeString()}][Wss]发送心跳包..." + text);
                WebSocket.Send(text);
            });

            _heartbeatTimer.Change(0, Heartbeat_Interval);
        }

        private void SendOpCode2Identify()
        {
            Task.Run(() => { WebSocket.Send(BotOpCode.Gen_OpCode_2_Identify_Json(AppId, MyToken, 0 | this.Intents)); });
        }

        private void TryResume()
        {
            if (session == "")
                return;
            resuming = true;
            WebSocket.Send(new ResumeOpCode(MyToken, session).ToString());
        }

        /// <summary>
        /// Release模式下不会输出
        /// </summary>
        /// <param name="obj"></param>
        public static void DebugLog(object obj)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[Debug]" + obj.ToString());
            Console.ForegroundColor = ConsoleColor.White;

#endif
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
}