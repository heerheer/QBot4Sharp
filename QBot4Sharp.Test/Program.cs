// See https://aka.ms/new-console-template for more information

using System;
using System.Net.Http;
using System.Text.Json;
using QBot4Sharp;
using QBot4Sharp.Model.Messages;
using Websocket.Client;
using Websocket.Client.Models;

//Console.Write(OpCode.Gen_OpCode_2_Identify_Json("233"));

const string apiUrl = "https://sandbox.api.sgroup.qq.com";
const string wssUrl = "wss://sandbox.api.sgroup.qq.com/websocket";

/*Console.WriteLine("Hello, World!");
var http = new HttpClient();
http.DefaultRequestHeaders.Add("Authorization","Bot 101985386.0wROp50baMyEt8EXWIYjkB3kdNUo4eg8");

var resp = await http.GetAsync($"{apiUrl}/gateway/bot");

Console.WriteLine(await resp.Content.ReadAsStringAsync());
*/
string MyToken = "ouAhnS5dXLlnsCYNbnoupk5CSdhiHap4";
string AppId = "101985302";
var core = new BotCore(AppId, MyToken);
core.On_AT_MESSAGE_CREATE += (c, a) =>
{
    Console.WriteLine("aaa");
    core.Api.ReplyMessage(a.channel_id, new QBotMessageSend() { content = "你的意思是,"+a.content+"吗?",msg_id = a.id });
};
core.StartWssConnection();
Console.ReadKey();