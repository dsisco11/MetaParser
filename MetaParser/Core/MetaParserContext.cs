using MetaParser.Builders.Interfaces;
using MetaParser.Graphs;

using System.CodeDom.Compiler;
using System.IO;

namespace MetaParser.Core;

internal record MetaParserContext : ICodeBuilderContext
{
    #region Fields
    private MetaParserRegistry? _registry;
    #endregion

    #region Properties
    public IndentedTextWriter writer { get; set; } = new IndentedTextWriter(new StringWriter());
    public MetaParserConfig Config { get; set; }
    public MetaParserRegistry Registry 
    { 
        get
        {
            _registry ??= new MetaParserRegistry();
            return _registry;
        }
    }
    public DirectedGraph DepsGraph { get; set; }
    public WorkingSet WorkingSet { get; set; } = new();
    public int ActiveBuffer { get; set; }
    #endregion

    #region Accessors
    public string LastBufferName => FormatBufferName(ActiveBuffer - 1);
    public string ActiveBufferName => FormatBufferName(ActiveBuffer);
    public string NextBufferName => FormatBufferName(ActiveBuffer + 1);
    #endregion

    #region Methods
    private static string FormatBufferName(int buffer) => $"buffer{buffer}";
    #endregion
}
