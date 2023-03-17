using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class UnknownTokenPusher : MetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new UnknownTokenPusher();

    protected override void Write(MetaParserContext context)
    {
        var wr = context.Writer;
        wr.WriteLine($"if ({context.ActiveBufferName}.Length != {context.NextBufferName}.Length)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"var unk_content_size = {context.ActiveBufferName}.Length - {context.NextBufferName}.Length;");
        wr.WriteLine($"var unk_content = {context.ActiveBufferName}.Slice(0, unk_content_size);");
        wr.WriteLine($"results.Add(new {TokenValueStructName}({Format_Token_Id_Const_Ref(UnknownToken)}, unk_content));");
        wr.WriteLine($"{context.ActiveBufferName} = {context.ActiveBufferName}.Slice(unk_content_size);");
        wr.WriteLine($"{context.NextBufferName} = {context.ActiveBufferName}.Span;");
        wr.Indent--;
        wr.WriteLine("}");
    }
}
