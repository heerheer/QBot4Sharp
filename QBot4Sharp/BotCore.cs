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
        const string apiUrl_sandBox = "https://sandbox.api.sgroup.qq.com";
        const string wssUrl_sandbox = "wss://sandbox.api.sgroup.qq.com/websocket";

        const string apiUrl_public = "https://api.sgroup.qq.com";
        const string wssUrl_public = "wss://api.sgroup.qq.com/websocket";

        string MyToken;
        string AppId;
        public int S2d = 0; //保存下行消息得到的index？
        public WebsocketClient WebSocket;
        public int Heartbeat_Interval;
        public string BotId;

        /// <summary>
        /// 用于调用API
        /// </summary>
        public BotApi Api;

        /// <summary>
        /// 需要申请的事件
        /// 用 | 表示同时申请
        /// </summary>
        public long Intents { get; set; } = 0;

        #region Events

        public delegate void OriginEventHandler(BotCore core, BotOpCodeBase opCode);

        public delegate void MessageEventHandler(object botCore, QBotMessage? message);

        public delegate void GuildEventHandler(object botCore, GuildInfo? message);

        public delegate void GuildMemberEventHandler(object botCore, MemberInfo? message);


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
                    Console.WriteLine($"{msg.Text}");
                    var msgObj = JsonSerializer.Deserialize<BotOpCodeBase>(msg.Text);
                    S2d = msgObj.CodeId;
                    if (msgObj.OpCode == 10)
                    {
                        //Code为10是当客户端与网关建立ws连接之后，网关下发的第一条消息
                        Console.WriteLine("[Wss]收到初始化消息");
                        //获取HeartBeat间隔
                        Heartbeat_Interval = BotOpCode.Get_Heartbeat_interval(msg.Text);
                        SendOpCode2Identify();
                    }
                    else if (msgObj.OpCode == 11)
                    {
                        //11为当发送心跳成功之后，就会收到该消息。
                        Console.WriteLine("[Wss]心跳包接触OK");
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
                            AT_MESSAGE_CREATE(this,
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
                                    GUILD_CREATE(this, guildInfo);
                                    break;

                                case "GUILD_UPDATE":
                                    GUILD_UPDATE(this, guildInfo);
                                    break;

                                case "GUILD_DELETE":
                                    GUILD_DELETE(this, guildInfo);
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
                                    GUILD_MEMBER_ADD.Invoke(this, memberInfo);
                                    break;

                                case "GUILD_MEMBER_UPDATE":
                                    GUILD_MEMBER_UPDATE.Invoke(this, memberInfo);
                                    break;

                                case "GUILD_MEMBER_REMOVE":
                                    GUILD_MEMBER_REMOVE.Invoke(this, memberInfo);
                                    break;
                            }
                        }

                        if (eventType == "READY")
                        {
                            //鉴权成功
                            Console.WriteLine("[Wss]鉴权成功");
                            BotId = JsonSerializer
                                .Deserialize<OpCodeReadyEventContent>(((JsonElement)msgObj.EventContent).ToString())
                                ?.user.id ?? "";
                            //鉴权成功后开始建立心跳包
                            StartHeartbeat();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            WebSocket.Start();
            Console.WriteLine("wss连接已开始");
        }


        private void StartHeartbeat()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var text = BotOpCode.Gen_OpCode_Heartbeat_Json(S2d);
                    Console.WriteLine("[Wss]尝试心跳包..." + text);
                    WebSocket?.Send(text);
                    Thread.Sleep(Heartbeat_Interval);
                }
            });
        }

        private void SendOpCode2Identify()
        {
            Task.Run(() => { WebSocket.Send(BotOpCode.Gen_OpCode_2_Identify_Json(AppId, MyToken, 0 | this.Intents)); });
            //Console.WriteLine(BotOpCode.Gen_OpCode_2_Identify_Json(AppId, MyToken).Trim());
            Console.WriteLine("发送鉴权信息...");
        }
    }
}