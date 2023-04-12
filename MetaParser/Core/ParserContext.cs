using MetaParser.Builders.Interfaces;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs.Stages;

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace MetaParser.Core;

internal record struct ParserContext : ICodeBuilderContext
{
    #region Fields
    private EntityRegistry? _registry;
    public CodeGenState State;
    #endregion

    #region Properties
    public ParserConfiguration Config { get; set; }
    public ImmutableArray<ParsingStageContext> Stages { get; set; }
    #endregion

    #region Accessors
    public IndentedTextWriter? Writer { get; set; }

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
