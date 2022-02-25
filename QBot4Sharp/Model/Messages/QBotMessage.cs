using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using QBot4Sharp.ArkGenerator;

#pragma warning disable CS8618


namespace QBot4Sharp.Model.Messages
{
    public class QBotMessage
    {
        public class Member
        {
            /// <summary>
            /// 用户加入频道的时间
            /// </summary>
            [JsonPropertyName("joined_at")]
            public DateTime JoinTime { get; set; }

            /// <summary>
            /// 用户在频道内的身份组ID, 默认值可参考
            ///1	全体成员
            ///2	管理员
            ///4	群主/创建者
            ///5	子频道管理员
            /// </summary>
            [JsonPropertyName("roles")]
            public List<string> Roles { get; set; }
        }


        /// <summary>
        /// 消息 id
        /// </summary>
        [JsonPropertyName("id")]
        public string MsgId { get; set; }

        /// <summary>
        /// 子频道ID
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 频道 id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// 消息时间戳
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 消息编辑时间
        /// </summary>
        [JsonPropertyName("edited_timestamp")]
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 是否是@全员消息
        /// </summary>
        [JsonPropertyName("mention_everyone")]
        public bool IsMentionEveryone { get; set; }

        /// <summary>
        /// 消息创建者
        /// </summary>
        [JsonPropertyName("author")]
        public UserInfo AuthorInfo { get; set; }


        /// <summary>
        /// 附件
        /// </summary>
        [JsonPropertyName("attachments")]
        public List<MessageAttachment> Attachments { get; set; }

        /// <summary>
        /// embeds
        /// </summary>
        [JsonPropertyName("embeds")]
        public List<MessageEmbed> Embeds { get; set; }


        /// <summary>
        /// 消息创建者的member信息
        /// </summary>
        /// <returns></returns>
        [JsonPropertyName("mentions")]
        public List<UserInfo> Mentions { get; set; }

        /// <summary>
        /// 消息创建者的member信息
        /// </summary>
        /// <returns></returns>
        [JsonPropertyName("member")]
        public Member MemberInfo { get; set; }


        /// <summary>
        /// ark消息
        /// </summary>
        [JsonPropertyName("ark")]
        public MessageArk Ark { get; set; }


        /// <summary>
        /// 用于消息间的排序，seq 在同一子频道中按从先到后的顺序递增，不同的子频道之前消息无法排序。(目前只在消息事件中有值，后续废弃)
        /// </summary>
        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        /// <summary>
        /// 子频道消息 seq，用于消息间的排序，seq 在同一子频道中按从先到后的顺序递增，不同的子频道之前消息无法排序
        /// </summary>
        [JsonPropertyName("seq_in_channel")]
        public string SeqInChannel { get; set; }


        /// <summary>
        /// 内嵌键盘, 仅markdown消息支持该字段
        /// </summary>
        [JsonPropertyName("keyboard")]
        public MessageKeyboard Keyboard { get; set; }


        /// <summary>
        /// 创建一个基础的文本型回复消息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public QBotMessageSend CreateReplyMessage(string content)
        {
            return QBotMessageSend.CreateReplyMsg(MsgId, content);
        }

        /// <summary>
        /// 创建Embed模板回复消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="prompt">提示消息</param>
        /// <param name="content">内容文本</param>
        /// <param name="thumbnailUrl">缩略图Url</param>
        /// <returns></returns>
        public QBotMessageSend CreateReplyMessage(string title, string prompt, string content, string thumbnailUrl = "")
        {
            var a = QBotMessageSend.CreateReplyMsg(MsgId, "");
            a.EmbedMessage = new MessageEmbed(title, prompt);
            a.EmbedMessage.Field = new List<MessageEmbed.MessageEmbedField> { new(content) };
            return a;
        }

        /// <summary>
        /// 用Ark填充，生成回复消息
        /// </summary>
        /// <param name="ark"></param>
        /// <returns></returns>
        public QBotMessageSend CreateReplyMessage(MessageArk ark)
        {
            var m = QBotMessageSend.CreateReplyMsg(MsgId, "");
            m.ArkMessage = ark;
            return m;
        }

        /// <summary>
        /// 用ArkGenerator填充，生成回复消息
        /// </summary>
        /// <param name="arkg">Ark生成器</param>
        /// <returns></returns>
        public QBotMessageSend CreateReplyMessage(IArkGenerator arkg)
        {
            var m = QBotMessageSend.CreateReplyMsg(MsgId, "");
            m.ArkMessage = arkg.GetArkMessage();
            return m;
        }

        /// <summary>
        /// 获取去掉@开头后的文本 
        /// </summary>
        /// <returns></returns>
        public string GetMessage(string botId)
        {
            Content = Content.Replace($"<@!{botId}>", "");
            Regex replaceSpace = new Regex(@"\s{1,}", RegexOptions.IgnoreCase);

            Content = replaceSpace.Replace(Content, " ").Trim();
            var c = Content.Trim().TrimStart($"<@!{botId}>".ToCharArray()).Trim().TrimEnd($"<@!{botId}>".ToCharArray())
                .Trim();
            return c.TrimStart('/').Trim();
        }
    }

    public class MessageAttachment
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}