using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Graphs;
using MetaParser.Tokens;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;

namespace MetaParser.Core
{
    internal record MetaParserContext : ICodeBuilderContext
    {
        public IndentedTextWriter writer { get; set; } = new IndentedTextWriter(new StringWriter());
        public MetaParserConfig Config { get; set; }
        public VertexGraph TokenGraph { get; set; }
        public ImmutableDictionary<string, TokenInfo> Tokens = ImmutableDictionary<string, TokenInfo>.Empty;
        public ConsumerList Consumers { get; set; } = new();
    }
}
