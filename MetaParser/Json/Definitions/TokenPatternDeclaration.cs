using System.Linq;
using System.Text.Json.Serialization;
using MetaParser.Contexts;
using MetaParser.Patternization;

namespace MetaParser.Json.Definitions;

internal sealed record TokenPatternDeclaration : IPatternDeclaration
{
    #region Properties
    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("oneof")]
    public TokenPatternDeclaration[]? oneof { get; set; }
    #endregion

    public Pattern Resolve(MetaParserContext context)
    {
        if (id is not null)
        {
            return ResolveConst(context);
        }
        else if (oneof is not null)
        {
            return ResolveOneOf(context);
        }

        return Pattern.Empty;
    }
    private Pattern ResolveConst(MetaParserContext context)
    {
        if (id is not null)
        {
            return new PatternConst(MetaParserContext.Get_TokenId_Ref(id));
        }

        return Pattern.Empty;
    }

    private Pattern ResolveOneOf(MetaParserContext context)
    {
        if (oneof is null || oneof.Length == 0)
        {
            return Pattern.Empty;
        }

        if (oneof.Length == 1)
        {
            return oneof.Single().Resolve(context);
        }

        var consts = oneof.Select(o => o.Resolve(context)).ToArray();
        return new PatternGroup(EPatternCondition.OneOf, consts);
    }
}
