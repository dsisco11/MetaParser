using MetaParser.Builders.Interfaces;
using MetaParser.Graphs;
using MetaParser.Tokens;

using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;

namespace MetaParser.Core;

internal record MetaParserContext : ICodeBuilderContext
{
    public IndentedTextWriter writer { get; set; } = new IndentedTextWriter(new StringWriter());
    public MetaParserConfig Config { get; set; }
    public MetaParserRegistry Registry { get; set; } = new();
    public DirectedGraph<GraphNodeKey> DepsGraph { get; set; }
    public WorkingSet WorkingSet { get; set; } = new();
}
