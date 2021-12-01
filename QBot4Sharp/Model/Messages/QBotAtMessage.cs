﻿using System;
using System.Collections.Generic;

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
        /// 频道ID
        /// </summary>
        public string channel_id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
        public string guild_id { get; set; }
        
        //消息ID
        public string id { get; set; }
        
        /// <summary>
        /// 消息创建者的member信息
        /// </summary>
        /// <returns></returns>
        public Member member { get; set; }
        
        /// <summary>
        /// 消息时间戳
        /// </summary>
        public DateTime timestamp { get; set; }
    }
    




}