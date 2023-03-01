using System.Text.Json.Serialization;
using MetaParser.Contexts;
using MetaParser.Patternization;
using MetaParser.Json.Attributes;

namespace MetaParser.Json.Definitions;

internal sealed record TokenPattern : PatternDeclaration
{
    #region Properties
    [JsonPrimaryProperty]
    [JsonPropertyName("id")]
    public string? id { get; set; }
    #endregion

    [JsonConstructor]
    public TokenPattern(string? id)
    {
        this.id = id;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        if (id is not null)
        {
            return new PatternConst(MetaParserContext.Get_TokenId_Ref(id));
        }

        return base.Resolve(context);
    }
}
