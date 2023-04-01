using MetaParser.Json.Definitions;

using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal record StageData
{
    #region Properties
    public readonly EParsingStage Stage;
    public readonly Dictionary<string, TokenClause> Items = new();
    #endregion

    #region Constructors
    public StageData(EParsingStage stage)
    {
        Stage = stage;
    }

    public StageData(EParsingStage stage, Dictionary<string, TokenClause> items)
    {
        Stage = stage;
        Items = items;
    }
    #endregion
}
