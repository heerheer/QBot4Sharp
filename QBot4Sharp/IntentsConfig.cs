namespace QBot4Sharp;

#pragma warning disable CS0067
public class IntentsConfig
{
    /// <summary>
    /// 频道与子频道相关事件
    /// <para>机器人被添加进某个频道与删除也会触发</para>
    /// <para>Link:https://bot.q.qq.com/wiki/develop/api/gateway/guild.html#guild-create</para>
    /// <para>Link2:https://bot.q.qq.com/wiki/develop/api/gateway/channel.html</para>
    /// </summary>
    public bool GuildEvent { get; set; } = false;

    /// <summary>
    /// 频道与子频道成员相关事件
    /// <para>Link:https://bot.q.qq.com/wiki/develop/api/gateway/guild_member.html#guild-member-add</para>
    /// </summary>
    public bool GuildMemberEvent { get; set; } = false;


    /// <summary>
    /// 消息事件，仅 *私域* 机器人能够设置此 intents
    /// </summary>
    public bool GuildMessagesEvent { get; set; } = false;

    /// <summary>
    /// 表情表态事件
    /// </summary>
    public bool GuildMessageReactionsEvent { get; set; } = false;

    /// <summary>
    /// 私信事件
    /// </summary>
    public bool DirectMessageEvent { get; set; } = false;

    /// <summary>
    /// 互动事件
    /// </summary>
    public bool InteractionEvent { get; set; }

    /// <summary>
    /// 消息审核事件
    /// </summary>
    public bool MessageAuditEvent { get; set; } = false;

    /// <summary>
    /// 帖子与评论事件
    /// </summary>
    public bool ForumEvent { get; set; } = false;


    /// <summary>
    /// 音频事件
    /// </summary>
    public bool AudioActionEvent { get; set; } = false;

    /// <summary>
    /// 当收到@机器人的消息时
    /// </summary>
    public bool PublicGuildMessagesEvent { get; set; } = false;

    public long Value =>
        0
        | (uint)(GuildEvent ? 1 << 0 : 0)
        | (uint)(GuildMemberEvent ? 1 << 1 : 0)
        | (uint)(GuildMessagesEvent ? 1 << 9 : 0)
        | (uint)(GuildMessageReactionsEvent ? 1 << 10 : 0)
        | (uint)(DirectMessageEvent ? 1 << 12 : 0)
        | (uint)(InteractionEvent ? 1 << 26 : 0)
        | (uint)(MessageAuditEvent ? 1 << 27 : 0)
        | (uint)(ForumEvent ? 1 << 28 : 0)
        | (uint)(AudioActionEvent ? 1 << 29 : 0)
        | (uint)(PublicGuildMessagesEvent ? 1 << 30 : 0);
}