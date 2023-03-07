using MetaParser.Core;
using MetaParser.Json.Definitions;
using MetaParser.Patternization;

using System.Linq;

namespace MetaParser.Consumers;

internal record ConsumerData
{
    public readonly int Index;
    public readonly EConsumerType Type;
    public readonly PatternGroup? Start;
    public readonly PatternGroup? Consume;
    public readonly PatternGroup? Stop;
    public readonly PatternGroup? Escape;

    public ConsumerData(MetaParserContext context, IConsumerDeclaration consumer, int index)
    {
        Index = index;
        Type = consumer.Type;
        Start = consumer.Start.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Start.Select(o => o.Resolve(context)).ToArray()) : null;
        Consume = consumer.Consume.Any() ? new PatternGroup(EPatternCondition.OneOf, consumer.Consume.Select(o => o.Resolve(context)).ToArray()) : null;
        Stop = consumer.Stop.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Stop.Select(o => o.Resolve(context)).ToArray()) : null;
        Escape = consumer.Escape.Any() ? new PatternGroup(EPatternCondition.AllOf, consumer.Escape.Select(o => o.Resolve(context)).ToArray()) : null;
    }
}
