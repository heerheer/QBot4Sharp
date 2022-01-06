using System.Text.Json.Serialization;

namespace QBot4Sharp.Model
{
    /// <summary>
    /// 子频道信息。
    /// </summary>
    public class ChannelInfo
    {
        /// <summary>
        /// 子频道ID
        /// </summary>
        [JsonPropertyName("id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 频道ID
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; set; }

        /// <summary>
        /// 子频道名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }


        /// <summary>
        /// 子频道类型 ChannelType
        /// </summary>
        [JsonPropertyName("type")]
        public ChannelType ChannelType { get; set; }

        /// <summary>
        /// 子频道子类型 ChannelSubType
        /// </summary>
        [JsonPropertyName("sub_type")]
        public int ChannelSubType { get; set; }

        /// <summary>
        /// 创建人 id
        /// </summary>
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        /// <summary>
        /// 排序，必填，而且不能够和其他子频道的值重复
        /// </summary>
        [JsonPropertyName("position")]
        public int Position { get; set; }

        /// <summary>
        /// 分组 id
        /// </summary>
        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        /// <summary>
        /// 子频道私密类型
        /// </summary>
        [JsonPropertyName("private_type")]
        public ChannelPrivateType PrivateType { get; set; }

        public enum ChannelPrivateType
        {
            Public = 0,
            Admin = 1,
            Special = 2,
        }
    }

    /// <summary>
    /// 注：由于QQ频道还在持续的迭代中，经常会有新的子频道类型增加，文档更新不一定及时，开发者识别 ChannelType 时，请注意相关的未知 ID 的处理。
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        /// 文字子频道
        /// </summary>
        TextChannel = 0,

        //1
        /// <summary>
        /// 语音子频道
        /// </summary>
        AudioChannel = 2,

        //3
        /// <summary>
        /// 子频道分组
        /// </summary>
        ChannelGroup = 4,

        /// <summary>
        /// 应用子频道
        /// </summary>
        LiveChannel = 10005,

        /// <summary>
        /// 应用子频道
        /// </summary>
        ApplicationChannel = 10006,

        /// <summary>
        /// 论坛子频道
        /// </summary>
        FourmChannel = 10007,
    }

    /// <summary>
    /// 注：目前只有文字子频道具有 ChannelSubType 二级分类，同时二级分类也可能会不断增加，开发者也需要注意对未知 ID 的处理。
    /// </summary>
    public enum ChannelSubType
    {
        /// <summary>
        /// 闲聊
        /// </summary>
        Chat = 0,

        /// <summary>
        /// 公告
        /// </summary>
        Annouce = 1,

        /// <summary>
        /// 攻略
        /// </summary>
        Raiders = 2,

        /// <summary>
        /// 开黑
        /// </summary>
        GameTeam = 3,
    }
}