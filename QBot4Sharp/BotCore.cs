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
        const string apiUrl = "https://sandbox.api.sgroup.qq.com";
        const string wssUrl = "wss://sandbox.api.sgroup.qq.com/websocket";

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

        public delegate void MessageEventHandler(object botCore, QBotMessage? message);

        /// <summary>
        /// 收到AT消息事件。
        /// </summary>
        public event MessageEventHandler On_AT_MESSAGE_CREATE;

        #endregion

        public BotCore(string appId, string myToken)
        {
            AppId = appId;
            MyToken = myToken;
            Api = new BotApi(appId, myToken);
            WebSocket = new WebsocketClient(new Uri(wssUrl)); //创建ws客户端
        }


        public void StartWssConnection()
        {
            WebSocket.MessageReceived.Subscribe(msg =>
            {
                Console.WriteLine($"{msg.Text}");
                var msgObj = JsonSerializer.Deserialize<BotOpCodeBase>(msg.Text);
                S2d = msgObj.CodeId;
                if (msgObj.OpCode == 10)
                {
                    //Code为10是当客户端与网关建立ws连接之后，网关下发的第一条消息
                    Console.WriteLine("收到初始化消息");
                    //获取HeartBeat间隔
                    Heartbeat_Interval = BotOpCode.Get_Heartbeat_interval(msg.Text);
                    SendOpCode2Identify();
                }
                else if (msgObj.OpCode == 11)
                {
                    //11为当发送心跳成功之后，就会收到该消息。
                    Console.WriteLine("心跳包接触OK");
                }
                else if (msgObj.OpCode == 0)
                {
                    //服务端进行消息推送
                    if (msgObj.EventType == "AT_MESSAGE_CREATE")
                    {
                        Console.WriteLine("收到消息事件");
                        try
                        {
                            On_AT_MESSAGE_CREATE.Invoke(this,
                                JsonSerializer.Deserialize<QBotMessage>(
                                    ((JsonElement)msgObj.EventContent).ToString()
                                )
                            );
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    if (msgObj.EventType == "READY")
                    {
                        //鉴权成功
                        Console.WriteLine("鉴权成功");
                        BotId = ((OpCode_ReadyEventContent)msgObj.EventContent).user.id;
                        //鉴权成功后开始建立心跳包
                        StartHeartbeat();
                    }
                }
                else
                {
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
                    Console.WriteLine("尝试发送心跳包" + text);
                    WebSocket?.Send(text);
                    Thread.Sleep(Heartbeat_Interval);
                }
            });
        }

        private void SendOpCode2Identify()
        {
            //"{\"op\":2,\"d\":{\"token\":\"Bot 101985386.0wROp50baMyEt8EXWIYjkB3kdNUo4eg8\",\"intents\":1073741824,\"shard\":[0,1],\"properties\":null}}"
            Task.Run(() => { WebSocket.Send(BotOpCode.Gen_OpCode_2_Identify_Json(AppId, MyToken, 0 | this.Intents)); });
            //Console.WriteLine(BotOpCode.Gen_OpCode_2_Identify_Json(AppId, MyToken).Trim());
            Console.WriteLine("发送鉴权信息...");
        }
    }
}