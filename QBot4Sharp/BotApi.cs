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

        /// <summary>
        /// 获取拼接的Auth文本
        /// </summary>
        /// <returns></returns>
        private string GetAuthCode() => $"Bot {_appId}.{_myToken}";

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="msgToSend"></param>
        public void SendMessage(string channelId, QBotMessageSend msgToSend)
        {
            HttpUtil.PostWithAuth(urlBase + $"/channels/{channelId}/messages", msgToSend.ToString(), GetAuthCode());
        }

        /// <summary>
        /// 获取WSS Url(普通模式)
        /// </summary>
        /// <returns></returns>
        public string GetWebsocketLink()
        {
            return HttpUtil.GetWithAuth(urlBase + $"/gateway", GetAuthCode());
        }

        /// <summary>
        /// 获取频道信息
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public GuildInfo? GetGuildInfo(string guildId)
        {
            var json = HttpUtil.GetWithAuth(urlBase + $"/guilds/{guildId}", GetAuthCode());
            Console.WriteLine("获取Guild信息");
            Console.WriteLine(json);
            return JsonSerializer.Deserialize<GuildInfo>(json);
        }

        public MemberInfo? GetMemberInfo(string guildId, string userId)
        {
            var url = urlBase + $"/guilds/{guildId}/members/{userId}";
            var json = HttpUtil.GetWithAuth(url, GetAuthCode());
            Console.WriteLine(json);
            Console.WriteLine("获取Member信息");
            return JsonSerializer.Deserialize<MemberInfo>(json);
        }
    }
}