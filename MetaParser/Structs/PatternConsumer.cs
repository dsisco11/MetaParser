using MetaParser.Contexts;
using MetaParser.Exceptions;
using MetaParser.Json.Definitions;
using MetaParser.Patternization;
using System.Linq;

using System.Security.Cryptography;

namespace MetaParser.Structs;

internal record PatternConsumer
{
    #region Properties
    public ETokenType Type;
    public int TokenIndex;
    public string TokenName;
    public int ConsumerIndex;
    public PatternGroup? Start;
    public PatternGroup? Consume;
    public PatternGroup? Stop;
    public PatternGroup? Escape;
    #endregion

    #region Utility
    private bool HasConstLenStart => (Start is null || Start.IsConstantLength);
    private bool HasConstLenConsume => (Consume is null || Consume.IsConstantLength);
    private bool HasConstLenStop => (Stop is null || Stop.IsConstantLength);
    private bool HasConstLenEscape => (Escape is null || Escape.IsConstantLength);
    /// <summary>
    /// Returns true/false whether the pattern has a constant length and will always match a consistent number of items
    /// </summary>
    public bool IsConstantLength => (HasConstLenStart && HasConstLenConsume && HasConstLenStop && HasConstLenEscape);
    #endregion

    public PatternConsumer(ETokenType type, int tokenIndex, int consumerIndex, string idName, PatternGroup? start, PatternGroup? consume, PatternGroup? stop, PatternGroup? escape)
    {
        Type = type;
        TokenIndex = tokenIndex;
        ConsumerIndex = consumerIndex;
        TokenName = idName;
        Start = start;
        Consume = consume;
        Stop = stop;
        Escape = escape;
    }

    public static PatternConsumer From(MetaParserContext context, IConsumerDefinition consumer, string tokenName, int tokenIndex, int consumerIndex)
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

        return new PatternConsumer(consumer.Type, tokenIndex, consumerIndex, tokenName, startClause, consumeClause, stopClause, escapeClause);
    }
}
