namespace QBot4Sharp
{
    /// <summary>
    /// 提供了消息Content中允许内嵌的格式码
    /// </summary>
    public class QBotCode
    {
        /// <summary>
        /// 解析为 @用户 标签
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string AtUser(string userId) => $"<@{userId}>";

        /// <summary>
        /// 解析为 #子频道 标签，点击可以跳转至子频道，仅支持当前频道内的子频道
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static string SubChannelLink(string channelId) => $"<#{channelId}>";
    }

    public enum QBotIntents
    {
        /// <summary>
        /// 频道相关事件
        /// </summary>
        GUILDS = 1 << 0,

        /// <summary>
        /// 频道成员有关事件
        /// </summary>
        GUILD_MEMBERS = 1 << 1,

        /// <summary>
        /// 需要申请权限
        /// 消息表情状态相关事件
        /// </summary>
        GUILD_MESSAGE_REACTIONS = 1 << 10,

        /// <summary>
        /// 私聊消息
        /// </summary>
        DIRECT_MESSAGE = 1 << 12,
        
        /// <summary>
        /// 主题贴
        /// </summary>
        FORUM_EVENT =1<<28,

        /// <summary>
        /// 音频消息
        /// </summary>
        AUDIO_ACTION = 1 << 29,

        /// <summary>
        /// 收到AT消息
        /// </summary>
        AT_MESSAGES = 1 << 30,
    }
}