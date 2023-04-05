using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;

namespace MetaParser.Builders.Interfaces;

internal sealed class UsingBuilder : MetaCodeBuilder
{
    #region Properties
    public IEnumerable<UsingDirectiveSyntax> Usings { get; }
    #endregion

    #region Constructors
    public UsingBuilder(IEnumerable<UsingDirectiveSyntax> usings)
    {
        Usings = usings;
    }
    #endregion

    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        foreach (var ns in Usings)
        {
            ns.WriteTo(writer);
            writer.WriteLine();
        }
        writer.WriteLine();
    }
}