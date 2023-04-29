using MetaParser.Graphs;
using MetaParser.Json.Definitions;

using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal record StageData
{
    #region Properties
    public readonly EParsingStage Stage;
    public readonly List<string> Ignored;
    public readonly List<ConsumerClause> Dropped;
    public readonly Dictionary<string, TokenClause> Items;
    public readonly DirectedNodeGraph<string> graph;
    #endregion

    #region Constructors
    protected StageData()
    {
        Dropped = new();
        Ignored = new();
        Items = new();
        graph = new();
    }


    public StageData(EParsingStage stage) : this()
    {
        Stage = stage;
    }

    public StageData(StageData other)
    {
        Stage = other.Stage;
        Ignored = new (other.Ignored);
        Dropped = new ();
        Items = new ();
        graph = new ();
    }
    #endregion
}
