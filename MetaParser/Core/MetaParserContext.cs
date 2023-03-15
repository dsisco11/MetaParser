using MetaParser.Builders.Interfaces;
using MetaParser.Graphs;

using System.CodeDom.Compiler;
using System.IO;

namespace MetaParser.Core;

internal record MetaParserContext : ICodeBuilderContext
{
    #region Properties
    public IndentedTextWriter writer { get; set; } = new IndentedTextWriter(new StringWriter());
    public MetaParserConfig Config { get; set; }
    public MetaParserRegistry Registry { get; set; } = new();
    public DirectedGraph DepsGraph { get; set; }
    public WorkingSet WorkingSet { get; set; } = new ();
    public int ActiveBuffer { get; set; }
    #endregion

    #region Accessors
    public string LastBufferName => GetBufferName(ActiveBuffer - 1);
    public string ActiveBufferName => GetBufferName(ActiveBuffer);
    public string NextBufferName => GetBufferName(ActiveBuffer + 1);
    #endregion

    #region Methods
    private string GetBufferName(int buffer) => $"buffer{buffer}";
    #endregion
}
