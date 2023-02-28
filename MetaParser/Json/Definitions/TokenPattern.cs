using System.Text.Json.Serialization;
using MetaParser.Contexts;
using MetaParser.Patternization;
using MetaParser.Json.Attributes;

namespace MetaParser.Json.Definitions;

internal record TokenPattern : PatternDeclaration
{
    [JsonInclude]
    [JsonPrimaryProperty]
    public string id { get; set; }

    [JsonConstructor]
    public TokenPattern(string id)
    {
        this.id = id;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        return new PatternConst(MetaParserContext.Get_TokenId_Ref(id));
    }
}
