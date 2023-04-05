using MetaParser.Core;

using System;

namespace MetaParser.Builders.Interfaces;

internal abstract class FunctionBodyBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        writer.WriteLine("{");
        writer.Indent++;
        WriteContent(context with { State = context.State with { ActiveBuffer = 0 } });
        writer.Indent--;
        writer.WriteLine("}");
    }
}
