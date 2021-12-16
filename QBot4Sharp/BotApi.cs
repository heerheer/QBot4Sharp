using System;
using System.Text.Json;
using QBot4Sharp.Model;
using QBot4Sharp.Model.Messages;
using QBot4Sharp.Utils;

namespace QBot4Sharp
{
    public class BotApi
    {
        private string urlBase;

        private string _appId;
        private string _myToken;

        public BotApi(string appId, string myToken, bool isSandBoxMode)
        {
            _appId = appId;
            _myToken = myToken;

            urlBase = isSandBoxMode ? "https://sandbox.api.sgroup.qq.com" : "https://api.sgroup.qq.com";
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
            Console.WriteLine(urlBase);
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
            var url = urlBase + $"/guilds/{guildId}";
            var json = HttpUtil.GetWithAuth(url, GetAuthCode());
            Console.WriteLine("获取Guild信息");
            Console.WriteLine(json);
            return JsonSerializer.Deserialize<GuildInfo>(json);
        }

        /// <summary>
        /// 获取指定成员的信息
        /// </summary>
        /// <param name="guildId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MemberInfo? GetMemberInfo(string guildId, string userId)
        {
            var url = urlBase + $"/guilds/{guildId}/members/{userId}";
            var json = HttpUtil.GetWithAuth(url, GetAuthCode());
            return JsonSerializer.Deserialize<MemberInfo>(json);
        }

        /// <summary>
        /// 获取子频道的信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ChannelInfo? GetChannelInfo(string channelId)
        {
            var url = urlBase + $"/channels/{channelId}";
            var json = HttpUtil.GetWithAuth(url, GetAuthCode());
            return JsonSerializer.Deserialize<ChannelInfo>(json);
        }

        /// <summary>
        /// 获取频道中子频道列表
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public List<ChannelInfo> GetChannelList(string guildId)
        {
            var url = urlBase + $"/guilds/{guildId}/channels";
            var json = HttpUtil.GetWithAuth(url, GetAuthCode());
            return JsonSerializer.Deserialize<List<ChannelInfo>>(json) ?? new();
        }

        /// <summary>
        /// 获取一只小马。
        /// </summary>
        /// <returns>彩蛋？</returns>
        public String GetAHorse()
        {
            return "我是一只不会赛马的小马";
        }
    }
}