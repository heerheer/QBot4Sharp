using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QBot4Sharp.Model.Messages
{
    public class QBotAtMessage
    {
        public class Author
        {
            /// <summary>
            /// 头像地址
            /// </summary>
            public string avatar { get; set; }

            /// <summary>
            /// 是否是Bot
            /// </summary>
            public bool bot { get; set; }

            /// <summary>
            /// 用户 id
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// 用户名
            /// </summary>
            public string username { get; set; }
        }

        public class Member
        {
            /// <summary>
            /// 用户加入频道的时间
            /// </summary>
            public DateTime joined_at { get; set; }

            /// <summary>
            /// 用户在频道内的身份组ID, 默认值可参考
            ///1	全体成员
            ///2	管理员
            ///4	群主/创建者
            ///5	子频道管理员
            /// </summary>
            public List<string> roles { get; set; }
        }

        /// <summary>
        /// 消息创建者
        /// </summary>
        public Author author { get; set; }

        /// <summary>
        /// 子频道ID
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 频道 id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; set; }

        /// <summary>
        /// 消息 id
        /// </summary>
        [JsonPropertyName("id")]
        public string MsgId { get; set; }

        /// <summary>
        /// 消息创建者的member信息
        /// </summary>
        /// <returns></returns>
        public Member member { get; set; }

        /// <summary>
        /// 消息时间戳
        /// </summary>
        public DateTime timestamp { get; set; }
        
        /// <summary>
        /// 是否是@全员消息
        /// </summary>
        public bool mention_everyone { get; set; }
        
        /// <summary>
        /// 消息编辑时间
        /// </summary>
        public DateTime edited_timestamp { get; set; }
    }
}