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
        public static string SubChannelLink(string channelId) => $"<@{channelId}>";
    }
}