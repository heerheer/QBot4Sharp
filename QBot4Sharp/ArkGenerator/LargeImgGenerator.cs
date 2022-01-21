using QBot4Sharp.Model.Messages;

namespace QBot4Sharp.ArkGenerator;

/// <summary>
/// 大图模板
/// <para></para>
/// Link:https://bot.q.qq.com/wiki/develop/api/openapi/message/template/template_37.html
/// </summary>
public class LargeImgGenerator : IArkGenerator
{
    public LargeImgGenerator(string prompt = "", string metatitle = "", string metacover = "", string metaurl = "",
        string metasubtitle = "")
    {
        Prompt = prompt;
        Title = metatitle;
        MetaCover = metacover;
        MetaUrl = metaurl;
        SubTitle = metasubtitle;
    }

    /// <summary>
    /// 提示文本
    /// </summary>
    public string Prompt { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 大图，尺寸为 975*540
    /// </summary>
    public string MetaCover { get; set; }

    /// <summary>
    /// 跳转链接
    /// </summary>
    public string MetaUrl { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public string SubTitle { get; set; }

    public MessageArk GetArkMessage()
    {
        var ark = new MessageArk();
        ark.TemplateId = 37;
        ark.KvList = new();
        ark.KvList.Add(new("#PROMPT#", Prompt));
        ark.KvList.Add(new("#METATITLE#", Title));
        ark.KvList.Add(new("#METACOVER#", MetaCover));
        ark.KvList.Add(new("#METAURL#", MetaUrl));
        ark.KvList.Add(new("#METASUBTITLE#", SubTitle));

        return ark;
    }
}