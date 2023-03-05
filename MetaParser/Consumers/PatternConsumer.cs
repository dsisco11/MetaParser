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

        TokenType = data.Type == EConsumerType.Data ? ETokenType.Constant : ETokenType.Compound;

        if (data.Consume is not null)
        {
            // Any 'compound' token which consumes another 'compound' token is a 'complex' token
            bool isComplex = data.Consume.GetSubPatterns().OfType<PatternConst>().Any(c => tokenTypes.TryGetValue(c.value, out ETokenType outType) && outType == ETokenType.Compound);

            if (isComplex)
            {
                TokenType = ETokenType.Complex;
            }
        }

        switch (data.Start)
        {
            case null when data.Consume is null:
                throw new IllegalTokenException($@"Illegal token definition (""{tokenName}"") (tokens require at minimum either a START or CONSUME sequence)");
            case null when data.Consume is not null:
                {// If pattern has no START condition, then we use the CONSUME pattern as an implicit starting condition
                    Start = new PatternGroup(EPatternCondition.OneOf, data.Consume);
                    break;
                }
            case not null when data.Consume is not null && data.Stop is null:
                {// add the consume clause item to the end of the start clause so the parser only consumes this token if its possible for it to actually consume items
                    var concat = new List<Pattern>(data.Start.items)
                    {
                        data.Consume
                    };

                    Start = new PatternGroup(EPatternCondition.AllOf, concat.ToArray());
                    break;
                }
            default:
                {
                    Start = data.Start!;
                    break;
                }
        }

        Consume = data.Consume;
        Stop = data.Stop;
        Escape = data.Escape;
    }
    #endregion

}
