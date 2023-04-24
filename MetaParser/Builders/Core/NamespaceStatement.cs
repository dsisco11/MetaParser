using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;

namespace MetaParser.Builders.Core;

internal class NamespaceStatement : MetaCodeBuilder
{
    public static readonly NamespaceStatement Instance = new NamespaceStatement();

    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        writer.WriteLine($"namespace {context.Config.Namespace};");
    }
}
