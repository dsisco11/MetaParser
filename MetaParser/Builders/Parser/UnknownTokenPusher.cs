using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class UnknownTokenPusher : MetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new UnknownTokenPusher();

    protected override void Write(ParserContext context)
    {
        var wr = context.Writer;
        wr.WriteLine($"if ({context.State.ActiveBufferName}.Length != {context.State.NextBufferName}.Length)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"var unk_content_size = {context.State.ActiveBufferName}.Length - {context.State.NextBufferName}.Length;");
        wr.WriteLine($"var unk_content = {context.State.ActiveBufferName}.Slice(0, unk_content_size);");
        wr.WriteLine($"results.Add(new {TokenValueStructName}({Format_Token_Id_Const_Ref(UnknownToken)}, unk_content));");
        wr.WriteLine($"{context.State.ActiveBufferName} = {context.State.ActiveBufferName}.Slice(unk_content_size);");
        wr.WriteLine($"{context.State.NextBufferName} = {context.State.ActiveBufferName}.Span;");
        wr.Indent--;
        wr.WriteLine("}");
    }
}
