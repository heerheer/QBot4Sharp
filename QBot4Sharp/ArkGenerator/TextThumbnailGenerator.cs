using QBot4Sharp.Model.Messages;

namespace QBot4Sharp.ArkGenerator;

/// <summary>
/// 文本缩略图模板
/// Link:https://bot.q.qq.com/wiki/develop/api/openapi/message/template/template_23.html
/// </summary>
public class TextThumbnailGenerator : IArkGenerator
{
    public TextThumbnailGenerator(string desc = "", string prompt = "", string title = "", string metaDesc = "",
        string imgUrl = "", string link = "", string subTitle = "")
    {
        Desc = desc;
        Prompt = prompt;
        Title = title;
        MetaDesc = metaDesc;
        ImgUrl = imgUrl;
        Link = link;
        SubTitle = subTitle;
    }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 提示文本
    /// </summary>
    public string Prompt { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 详情描述
    /// </summary>
    public string MetaDesc { get; set; }

    /// <summary>
    /// 图片链接
    /// </summary>
    public string ImgUrl { get; set; }

    /// <summary>
    /// 跳转链接
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public string SubTitle { get; set; }

    public MessageArk GetArkMessage()
    {
        var ark = new MessageArk();
        ark.TemplateId = 24;
        ark.KvList = new();
        ark.KvList.Add(new("#DESC#", Desc));
        ark.KvList.Add(new("#PROMPT#", Prompt));
        ark.KvList.Add(new("#TITLE#", Title));
        ark.KvList.Add(new("#METADESC#", MetaDesc));
        ark.KvList.Add(new("#IMG#", ImgUrl));
        ark.KvList.Add(new("#LINK#", Link));
        ark.KvList.Add(new("#SUBTITLE#", SubTitle));

        return ark;
    }
}