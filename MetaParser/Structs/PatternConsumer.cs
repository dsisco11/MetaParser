using MetaParser.Patternization;

namespace MetaParser.Structs;

internal record PatternConsumer
{
    #region Properties
    public ETokenType Type;
    public int TokenIndex;
    public int ConsumerIndex;
    public string IdName;
    public PatternGroup? Start;
    public PatternGroup? Consume;
    public PatternGroup? Stop;
    public PatternGroup? Escape;
    #endregion

    public PatternConsumer(ETokenType type, int tokenIndex, int consumerIndex, string idName, PatternGroup? start, PatternGroup? consume, PatternGroup? stop, PatternGroup? escape)
    {
        Type = type;
        TokenIndex = tokenIndex;
        ConsumerIndex = consumerIndex;
        IdName = idName;
        Start = start;
        Consume = consume;
        Stop = stop;
        Escape = escape;
    }
}
