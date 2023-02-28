using MetaParser.Contexts;
using MetaParser.Json.Attributes;
using MetaParser.Patternization;

using System.Linq;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(PatternAndSequenceDeclaration), "and")]
[JsonDerivedType(typeof(PatternOrSequenceDeclaration), "or")]
internal abstract record PatternDeclaration
{
    public abstract Pattern Resolve(MetaParserContext context);
}

internal sealed record PatternAndSequenceDeclaration : PatternDeclaration
{
    [JsonPropertyName("and")]
    [JsonPrimaryProperty]
    public PatternDeclaration[] items { get; set; }

    public override Pattern Resolve(MetaParserContext context)
    {
        var resolvedItems = items.Select(o => o.Resolve(context)).ToArray();
        return new PatternGroup(EPatternCondition.AllOf, resolvedItems);
    }
}

internal sealed record PatternOrSequenceDeclaration : PatternDeclaration
{
    [JsonPropertyName("or")]
    public PatternDeclaration[] items { get; set; }

    public override Pattern Resolve(MetaParserContext context)
    {
        var resolvedItems = items.Select(o => o.Resolve(context)).ToArray();
        return new PatternGroup(EPatternCondition.OneOf, resolvedItems);
    }
}