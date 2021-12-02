using System;
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

        private string getAuth() => $"Bot {_appId}.{_myToken}";

        public void ReplyMessage(string channelId, QBotMessageSend msgToSend)
        {
            HttpUtil.PostWithAuth(urlBase + $"/channels/{channelId}/messages", msgToSend.ToString(), getAuth());
        }

        public void GetGuildInfo(string guildId)
        {
            var json = HttpUtil.GetWithAuth(urlBase + $"/guilds/{guildId}", getAuth());
        }
    }
}