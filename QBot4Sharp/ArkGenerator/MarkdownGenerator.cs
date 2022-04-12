using System.Text;
using QBot4Sharp.Model.Messages;

namespace QBot4Sharp.ArkGenerator;

public class MarkdownGenerator
{
    private StringBuilder _builder;

    public MarkdownGenerator()
    {
        _builder = new();
    }

    public void AddLine(params string[] args)
    {
        _builder.AppendLine(string.Join("", args));
    }

    public void AddLine2(params string[] args)
    {
        _builder.AppendLine(string.Join(' ', args));
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}