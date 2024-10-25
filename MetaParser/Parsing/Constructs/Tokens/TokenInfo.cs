using MetaParser.Json.Definitions;

namespace MetaParser.Parsing.Constructs.Tokens;

internal record TokenInfo
{
    #region Fields
    public readonly string Name;
    public readonly bool IsVirtual;
    public readonly EParsingStage Stage;
    #endregion

    #region Constructors
    public TokenInfo(string name, EParsingStage stage, bool isVirtual)
    {
        Name = name;
        Stage = stage;
        IsVirtual = isVirtual;
    }
    #endregion
}
