using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class UnknownTokenPusher : MetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new UnknownTokenPusher();

    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        writer.WriteLine($"if ({context.State.ActiveBufferName}.Length != {context.State.NextBufferName}.Length)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var unk_content_size = {context.State.ActiveBufferName}.Length - {context.State.NextBufferName}.Length;");
        writer.WriteLine($"var unk_content = {context.State.ActiveBufferName}.Slice(0, unk_content_size);");
        writer.WriteLine($"results.Add(new {TokenValueStructName}({Format_Token_Id_Const_Ref(UnknownToken)}, unk_content));");
        writer.WriteLine($"{context.State.ActiveBufferName} = {context.State.ActiveBufferName}.Slice(unk_content_size);");
        writer.WriteLine($"{context.State.NextBufferName} = {context.State.ActiveBufferName}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
    }
}
