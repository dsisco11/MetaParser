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
        const string VarNameUnkContent = "unk_content";
        const string VarNameUnkContentSize = "unk_content_size";
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        writer.WriteLine($"if ({context.State.ActiveBufferName}.Length != {context.State.NextBufferName}.Length)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {VarNameUnkContentSize} = {context.State.ActiveBufferName}.Length - {context.State.NextBufferName}.Length;");
        //writer.WriteLine($"var {VarNameUnkContent} = {context.State.ActiveBufferName}.Slice(0, {VarNameUnkContentSize});");
        writer.WriteLine($"outId.Span[outIndex] = {Format_Token_Id_Const_Ref(UnknownToken)};");
        writer.WriteLine($"outLength.Span[outIndex] = {VarNameUnkContentSize};");
        writer.WriteLine($"inIndex += {VarNameUnkContentSize};");
        writer.WriteLine($"outIndex++;");
        writer.WriteLine($"{context.State.ActiveBufferName} = {context.State.ActiveBufferName}.Slice({VarNameUnkContentSize});");
        writer.WriteLine($"{context.State.NextBufferName} = {context.State.ActiveBufferName}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
    }
}
