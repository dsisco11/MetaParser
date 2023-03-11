using System.Linq;
using System.Text.Json.Serialization;

using MetaParser.Core;
using MetaParser.Parsing.Constructs;

namespace MetaParser.Json.Definitions;

internal sealed record TokenPatternDeclaration : IPatternDeclaration
{
    #region Properties
    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("oneof")]
    public TokenPatternDeclaration[]? oneof { get; set; }
    #endregion

    public Pattern? Resolve(MetaParserContext context)
    {
        if (id is not null)
        {
            return ResolveToken(context);
        }
        else if (oneof is not null)
        {
            return ResolveOneOf(context);
        }

        return null;
    }
    private Pattern? ResolveToken(MetaParserContext context)
    {
        if (id is not null)
        {
            return new PatternTokenRef(CodeCommon.Format_Token_Key(id), context);
        }

        return null;
    }

    private Pattern? ResolveOneOf(MetaParserContext context)
    {
        if (oneof is null || oneof.Length == 0)
        {
            return null;
        }

        if (oneof.Length == 1)
        {
            return oneof.Single().Resolve(context);
        }

        var consts = oneof.Select(o => o.Resolve(context)!).ToArray();
        return new PatternGroup(EPatternCondition.OneOf, context, consts);
    }
}
