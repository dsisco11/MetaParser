using MetaParser.Contexts;
using MetaParser.Exceptions;
using MetaParser.Patternization;

using System;
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

    public readonly PatternGroup? Start;
    public readonly PatternGroup? Consume;
    public readonly PatternGroup? Stop;
    public readonly PatternGroup? Escape;
    #endregion

    #region Accessors
    public Pattern EffectiveStart => Start ?? Consume ?? throw new NotImplementedException();
    /// <summary> A consumer is considered open if it is dynamic and has no STOP criteria. </summary>
    public bool IsOpen => IsDynamic && Stop is null;
    /// <summary> 
    /// A consumer is considered dynamic if it is not constant, specifically if it has either a CONSUME or STOP criteria.
    /// So a consumer is "dynamic" if it involves consuming a variable number of elements.
    /// </summary>
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

        if (data.Start is null && data.Consume is null)
        {
            throw new IllegalTokenException($@"Illegal token definition (""{tokenName}"") (tokens require at minimum either a START or CONSUME sequence)");
        }

        Start = data.Start;
        Consume = data.Consume;
        Stop = data.Stop;
        Escape = data.Escape;
    }
    #endregion

}
