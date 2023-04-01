using MetaParser.Builders.Interfaces;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs.Stages;

using System.CodeDom.Compiler;

namespace MetaParser.Core;

internal record ParserContext : ICodeBuilderContext
{
    #region Fields
    private EntityRegistry? _registry;
    public CodeGenState State;
    #endregion

    #region Properties
    public ParserConfiguration Config { get; set; }
    public DirectedGraph DepsGraph { get; set; }
    public WorkingSet WorkingSet { get; set; } = new();
    public ParsingStageContext Stage { get; set; }
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
    #endregion

    #region Methods
    public void Increment_Active_Bufffer() => State.ActiveBuffer += 1;
    public void Decrement_Active_Bufffer() => State.ActiveBuffer -= 1;
    #endregion
}
