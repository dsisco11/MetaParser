using MetaParser.Builders.Interfaces;
using MetaParser.Graphs;

using System.CodeDom.Compiler;

namespace MetaParser.Core;

internal record ParserContext : ICodeBuilderContext
{
    #region Fields
    private EntityRegistry? _registry;
    #endregion

    #region Properties
    public ParserConfiguration Config { get; set; }
    public DirectedGraph DepsGraph { get; set; }
    public WorkingSet WorkingSet { get; set; } = new();
    public int ActiveBuffer { get; set; }
    #endregion

    #region Accessors
    public IndentedTextWriter Writer { get; set; }

    public EntityRegistry Registry
    {
        get
        {
            _registry ??= new EntityRegistry();
            return _registry;
        }
        set { _registry = value; }
    }


    public string LastBufferName => FormatBufferName(ActiveBuffer - 1);
    public string ActiveBufferName => FormatBufferName(ActiveBuffer);
    public string NextBufferName => FormatBufferName(ActiveBuffer + 1);
    #endregion

    #region Methods
    private static string FormatBufferName(int buffer) => $"buffer{buffer}";
    #endregion
}
