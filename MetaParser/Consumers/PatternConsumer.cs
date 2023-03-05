using MetaParser.Contexts;
using MetaParser.Exceptions;
using MetaParser.Patternization;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Consumers;

internal record PatternConsumer
{
    #region Properties
    public readonly int TokenIndex;
    public readonly string TokenName;
    public readonly ETokenType TokenType;

    public readonly int ConsumerIndex;
    public readonly EConsumerType ConsumerType;

    public readonly PatternGroup Start;
    public readonly PatternGroup? Consume;
    public readonly PatternGroup? Stop;
    public readonly PatternGroup? Escape;
    #endregion

    #region Accessors
    public bool IsOpen => IsDynamic && Stop is null;
    /// <summary> Returns if the pattern is "dynamic" and involves consuming a variable number of elements </summary>
    public bool IsDynamic => Consume is not null || Stop is not null;
    #endregion

    #region Constructors
    public PatternConsumer(ETokenType tokenType, EConsumerType consumerType, int tokenIndex, int consumerIndex, string idName, PatternGroup start, PatternGroup? consume, PatternGroup? stop, PatternGroup? escape)
    {
        TokenType = tokenType;
        ConsumerType = consumerType;
        TokenIndex = tokenIndex;
        ConsumerIndex = consumerIndex;
        TokenName = idName;
        Start = start;
        Consume = consume;
        Stop = stop;
        Escape = escape;
    }

    public PatternConsumer(ConsumerData data, IReadOnlyDictionary<string, ETokenType> tokenTypes, string tokenName, int tokenIndex)
    {
        TokenName = tokenName;
        TokenIndex = tokenIndex;
        ConsumerType = data.Type;
        ConsumerIndex = data.Index;

        Start = data.Start;
        Consume = data.Consume;
        Stop = data.Stop;
        Escape = data.Escape;

        switch (Start)
        {
            case null when Consume is null:
                throw new IllegalTokenException($@"Illegal token definition (""{tokenName}"") (tokens require at minimum either a START or CONSUME sequence)");
            case null when Consume is not null:
                {// If pattern has no START condition, then we use the CONSUME pattern as an implicit starting condition
                    Start = new PatternGroup(EPatternCondition.OneOf, Consume);
                    break;
                }
            case not null when Consume is not null && Stop is null:
                {// add the consume clause item to the end of the start clause so the parser only consumes this token if its possible for it to actually consume items
                    var concat = new List<Pattern>(Start.items)
                    {
                        Consume
                    };

                    Start = new PatternGroup(EPatternCondition.AllOf, concat.ToArray());
                    break;
                }
        }

        TokenType = data.Type == EConsumerType.Data ? ETokenType.Constant : ETokenType.Compound;
        // Any 'compound' token which consumes another 'compound' token is a 'complex' token
        bool isComplex = false;

        if (Consume is not null)
        {
            isComplex = Consume.GetSubPatterns().OfType<PatternConst>().Any(c => tokenTypes.TryGetValue(c.value, out ETokenType outType) && outType == ETokenType.Compound);
        }

        if (isComplex)
        {
            TokenType = ETokenType.Complex;
        }
    }
    #endregion

}
