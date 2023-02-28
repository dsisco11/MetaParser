using MetaParser.Contexts;
using MetaParser.Exceptions;
using MetaParser.Json.Definitions;
using MetaParser.Patternization;
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
    public readonly PatternGroup? Start;
    public readonly PatternGroup? Consume;
    public readonly PatternGroup? Stop;
    public readonly PatternGroup? Escape;
    #endregion

    #region Utility
    private bool HasConstLenStart => (Start is null || Start.IsConstantLength);
    private bool HasConstLenConsume => (Consume is null || Consume.IsConstantLength);
    private bool HasConstLenStop => (Stop is null || Stop.IsConstantLength);
    private bool HasConstLenEscape => (Escape is null || Escape.IsConstantLength);
    /// <summary>
    /// Returns if the pattern is "open-ended" and involves consuming a variable number of elements
    /// </summary>
    public bool IsOpenEnded => (Consume is not null || Stop is not null);
    #endregion

    public PatternConsumer(ETokenType tokenType, EConsumerType consumerType, int tokenIndex, int consumerIndex, string idName, PatternGroup? start, PatternGroup? consume, PatternGroup? stop, PatternGroup? escape)
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

    public static PatternConsumer From(MetaParserContext context, IConsumerDeclaration consumer, string tokenName, int tokenIndex, int consumerIndex)
    {
        var startClause = consumer.Start.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Start.Select(o => o.Resolve(context)).ToArray()) : null;
        var consumeClause = consumer.Consume.Any() ? new PatternGroup(EPatternCondition.OneOf, consumer.Consume.Select(o => o.Resolve(context)).ToArray()) : null;
        var stopClause = consumer.Stop.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Stop.Select(o => o.Resolve(context)).ToArray()) : null;
        var escapeClause = consumer.Escape.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Escape.Select(o => o.Resolve(context)).ToArray()) : null;

        if (startClause is null && consumeClause is null)
        {
            throw new IllegalTokenException($@"Illegal token definition (""{tokenName}"") (tokens require at minimum either a START or CONSUME sequence)");
        }

        if (startClause is null && consumeClause is not null)
        {// If pattern has no START condition, then we use the CONSUME pattern as an implicit starting condition
            startClause = new PatternGroup(EPatternCondition.OneOf, consumer.Consume.Select(o => o.Resolve(context)).ToArray());
        }

        ETokenType tokenType = consumer.Type == EConsumerType.Data ? ETokenType.Constant : ETokenType.Compound;
        // TODO: IF PATTERN CONSUMES TOKENS AND REFERENCES A TOKEN WHICH HAS A CONSUMER WHICH ALSO CONSUMES TOKENS, THEN IT IS A 'COMPLEX' TYPE

        return new PatternConsumer(tokenType, consumer.Type, tokenIndex, consumerIndex, tokenName, startClause, consumeClause, stopClause, escapeClause);
    }
}
