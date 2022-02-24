using System;
using System.Text.Json;
using QBot4Sharp.Model;
using QBot4Sharp.Model.Messages;
using QBot4Sharp.Utils;

namespace QBot4Sharp
{
    public record BotApiResult<T>(string? TraceId, T? Data);

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
        /// 用于获取当前用户（机器人）详情。
        /// </summary>
        /// <returns></returns>
        public UserInfo? GetMe()
        {
            return JsonSerializer.Deserialize<UserInfo>(HttpUtil.GetWithAuth(urlBase + $"/user/@me", GetAuthCode()));
        }


        /// <summary>
        /// 向子频道发送消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="msgToSend"></param>
        public QBotMessage? SendMessage(string channelId, QBotMessageSend msgToSend)
        {
            var msgStr = HttpUtil.PostWithAuth(urlBase + $"/channels/{channelId}/messages", msgToSend.ToString(),
                GetAuthCode());
            BotCore.DebugLog(msgToSend.ToString());
            return JsonSerializer.Deserialize<QBotMessage>(msgStr);
        }

        public async Task<BotApiResult<QBotMessage>?> SendMessageAsync(string channelId, QBotMessageSend msgToSend)
        {
            var res = await HttpUtil.PostWithAuthAsync(urlBase + $"/channels/{channelId}/messages",
                msgToSend.ToString(),
                GetAuthCode());
            BotCore.DebugLog(msgToSend.ToString());
            BotCore.DebugLog($"TraceId:{res.TraceId}");
            return new(res.TraceId, JsonSerializer.Deserialize<QBotMessage>(res.RespJson));
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
        /// 获取分片Url
        /// </summary>
        /// <returns></returns>
        public string GetWebsocketLinkWithShard()
        {
            return HttpUtil.GetWithAuth(urlBase + $"/gateway/bot", GetAuthCode());
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
            BotCore.DebugLog("获取Guild信息");
            BotCore.DebugLog(json);
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
        /// 获取自己的频道列表
        /// </summary>
        /// <returns></returns>
        public List<GuildInfo> GetGuildList(string before = "", string after = "", int limit = 100)
        {
            return GetGuildListAsync(before, after, limit).Result;
        }


        private async Task<List<GuildInfo>> GetGuildListAsync(string before = "", string after = "", int limit = 100)
        {
            var url = urlBase + $"/users/@me/guilds?" +
                      $"{(before != "" ? "before=" + before + "&" : "")}" +
                      $"{(after != "" ? "after=" + after + "&" : "")}" +
                      $"limit={limit}";

            var json = await HttpUtil.GetWithAuthAsync(url, GetAuthCode());
            return await JsonSerializer.DeserializeAsync<List<GuildInfo>>(json) ?? new();
        }

        /// <summary>
        /// 直接获取所有GuildInfo
        /// </summary>
        /// <returns></returns>
        public async Task<List<GuildInfo>> GetAllGuildListAsync()
        {
            var end = false;
            List<GuildInfo> list = new();
            var after = "";
            while (!end)
            {
                var l = await GetGuildListAsync("", after);

                if (l.Exists(x => list.Contains(x)))
                {
                    break;
                }

                if (l.Count != 100)
                    end = true;


                list.AddRange(l);
                after = l.Last().GuildId;
            }

            return list;
        }

        /// <summary>
        /// 设置子频道公告
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="msgId"></param>
        public void SetChannelAnnounce(string channelId, string msgId)
        {
            var data = new { message_id = msgId };
            var str = HttpUtil.PostWithAuth(urlBase + $"/channels/{channelId}/announces",
                JsonSerializer.Serialize(data), GetAuthCode());
            BotCore.DebugLog(str);
        }

        /// <summary>
        /// 取消子频道公告
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="msgId"></param>
        public void UnsetChannelAnnounce(string channelId, string msgId)
        {
            //var data = new { message_id = msgId };
            HttpUtil.DeleteWithAuth(urlBase + $"/channels/{channelId}/announces/{msgId}",
                GetAuthCode());
        }


        /// <summary>
        /// 用于机器人和在同一个频道内的成员创建私信会话。
        /// </summary>
        /// <param name="recipientId">接收者 id</param>
        /// <param name="sourceGuildId">源频道 id</param>
        /// <returns>包含频道ID，用户ID和创建时间的私信会话对象。</returns>
        public QBotDms? CreateDms(string recipientId, string sourceGuildId)
        {
            var data = new { recipient_id = recipientId, source_guild_id = sourceGuildId };
            return JsonSerializer.Deserialize<QBotDms>(HttpUtil.PostWithAuth(urlBase + $"/users/@me/dms",
                JsonSerializer.Serialize(data),
                GetAuthCode()));
        }

        public QBotMessage? SendPrivateMessage(string guildId, QBotMessageSend messageSend)
        {
            return JsonSerializer.Deserialize<QBotMessage>(HttpUtil.PostWithAuth(urlBase + $"/dms/{guildId}/messages",
                JsonSerializer.Serialize(messageSend),
                GetAuthCode()));
        }

        /// <summary>
        /// 获取频道可用权限列表
        /// GET /guilds/{guild_id}/api_permission
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public async Task<List<ApiPermission>?> GetGuildPermissions(string guildId)
        {
            //GET /guilds/{guild_id}/api_permission
            var url = urlBase + $"/guilds/{guildId}/api_permission";

            var jsonStream = await HttpUtil.GetWithAuthAsync(url, GetAuthCode());
            //var t = new { apis = new List<ApiPermission>() };
            var data = await JsonSerializer.DeserializeAsync<GetGuildPermissionsObj>(jsonStream);
            return data?.apis;
        }

        /// <summary>
        /// 获取一只小马。
        /// </summary>
        /// <returns>彩蛋？</returns>
        public String GetAHorse()
        {
            return "我是一只不会赛马的小马";
        }

        class GetGuildPermissionsObj
        {
            public List<ApiPermission>? apis { get; set; }
        }
    }
}