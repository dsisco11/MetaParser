using MetaParser.Contexts;
using MetaParser.Exceptions;
using MetaParser.Json.Definitions;
using MetaParser.Patternization;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Structs;

internal record PatternConsumer
{
    #region Properties
    public readonly ETokenType TokenType;
    public readonly EConsumerType ConsumerType;
    public readonly int TokenIndex;
    public readonly string TokenName;
    public readonly int ConsumerIndex;
    public readonly PatternGroup Start;
    public readonly PatternGroup? Consume;
    public readonly PatternGroup? Stop;
    public readonly PatternGroup? Escape;
    #endregion

    #region Utility
    public bool HasConstLenStart => Start.IsConstantLength;
    public bool HasConstLenConsume => (Consume is null || Consume.IsConstantLength);
    public bool HasConstLenStop => (Stop is null || Stop.IsConstantLength);
    public bool HasConstLenEscape => (Escape is null || Escape.IsConstantLength);
    /// <summary>
    /// Returns if the pattern is "open-ended" and involves consuming a variable number of elements
    /// </summary>
    public bool IsOpenEnded => (Consume is not null || Stop is not null);
    #endregion

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

    public static PatternConsumer From(MetaParserContext context, IReadOnlyDictionary<string, ETokenType> tokenTypes, IConsumerDeclaration consumer, string tokenName, int tokenIndex, int consumerIndex)
    {
        var startClause = consumer.Start.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Start.Select(o => o.Resolve(context)).ToArray()) : null;
        var consumeClause = consumer.Consume.Any() ? new PatternGroup(EPatternCondition.OneOf, consumer.Consume.Select(o => o.Resolve(context)).ToArray()) : null;
        var stopClause = consumer.Stop.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Stop.Select(o => o.Resolve(context)).ToArray()) : null;
        var escapeClause = consumer.Escape.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Escape.Select(o => o.Resolve(context)).ToArray()) : null;

        switch (startClause)
        {
            case null when consumeClause is null:
                throw new IllegalTokenException($@"Illegal token definition (""{tokenName}"") (tokens require at minimum either a START or CONSUME sequence)");
            case null when consumeClause is not null:
                {// If pattern has no START condition, then we use the CONSUME pattern as an implicit starting condition
                    startClause = new PatternGroup(EPatternCondition.OneOf, consumer.Consume.Select(o => o.Resolve(context)).ToArray());
                    break;
                }
            case not null when consumeClause is not null:
                {// add the consume clause item to the end of the start clause so the parser only consumes this token if its possible for it to actually consume items
                    var concat = new List<Pattern>(startClause.items);
                    concat.Add(consumeClause);

                    startClause = new PatternGroup(EPatternCondition.AllOf, concat.ToArray());
                    break;
                }
        }

        ETokenType tokenType = consumer.Type == EConsumerType.Data ? ETokenType.Constant : ETokenType.Compound;
        // Any 'compound' token which consumes another 'compound' token is a 'complex' token
        bool isComplex = false;
        if (startClause is not null)
        {
            isComplex |= startClause.GetSubPatterns().OfType<PatternConst>().Any(c => tokenTypes.TryGetValue(c.value, out ETokenType outType) && outType == ETokenType.Compound);
        }

        if (!isComplex && consumeClause is not null)
        {
            isComplex |= consumeClause.GetSubPatterns().OfType<PatternConst>().Any(c => tokenTypes.TryGetValue(c.value, out ETokenType outType) && outType == ETokenType.Compound);
        }

        if (!isComplex && stopClause is not null)
        {
            isComplex |= stopClause.GetSubPatterns().OfType<PatternConst>().Any(c => tokenTypes.TryGetValue(c.value, out ETokenType outType) && outType == ETokenType.Compound);
        }

        if (isComplex)
        {
            tokenType = ETokenType.Complex;
        }

        return new PatternConsumer(tokenType, consumer.Type, tokenIndex, consumerIndex, tokenName, startClause!, consumeClause, stopClause, escapeClause);
    }
}
