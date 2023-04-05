using MetaParser.Core;

using System;

namespace MetaParser.Builders.Interfaces;

internal class StatementBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        WriteContent(context);
        writer.WriteLine(";");
    }
}
