using MetaParser.Graphs;
using MetaParser.Json.Definitions;

using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal record StageData
{
    #region Properties
    public readonly EParsingStage Stage;
    public readonly Dictionary<string, TokenClause> Items;
    public readonly DirectedNodeGraph<string> Graph;
    #endregion

    #region Constructors
    public StageData(EParsingStage stage)
    {
        Stage = stage;
        Items = new();
        Graph = new DirectedNodeGraph<string>();
    }

    public StageData(StageData other)
    {
        Stage = other.Stage;
        Items = new();
        Graph = new();
    }
    #endregion
}
