using System;
using System.Text.Json;
using QBot4Sharp.Model;
using QBot4Sharp.Model.Messages;
using QBot4Sharp.Utils;

namespace QBot4Sharp
{
    public class BotApi
    {
        const string urlBase = "https://sandbox.api.sgroup.qq.com";
        private string _appId;
        private string _myToken;

        public BotApi(string appId, string myToken)
        {
            _appId = appId;
            _myToken = myToken;
        }

        private string GetAuthCode() => $"Bot {_appId}.{_myToken}";

        public void SendMessage(string channelId, QBotMessageSend msgToSend)
        {
            HttpUtil.PostWithAuth(urlBase + $"/channels/{channelId}/messages", msgToSend.ToString(), GetAuthCode());
        }

        public GuildInfo? GetGuildInfo(string guildId)
        {
            var json = HttpUtil.GetWithAuth(urlBase + $"/guilds/{guildId}", GetAuthCode());
            Console.WriteLine("获取Guild信息");
            Console.WriteLine(json);
            return JsonSerializer.Deserialize<GuildInfo>(json);
        }
    }
}