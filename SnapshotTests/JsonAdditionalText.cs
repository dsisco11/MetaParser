
using Microsoft.CodeAnalysis.Text;

public class JsonAdditionalText : AdditionalText
{
    private readonly string path;
    private readonly string source;

    public JsonAdditionalText(string path, string source)
    {
        this.path = path;
        this.source = source;
    }

    public override string Path => path;
    public override SourceText? GetText(CancellationToken cancellationToken = default)
    {
        return SourceText.From(source, System.Text.Encoding.UTF8);
    }
}