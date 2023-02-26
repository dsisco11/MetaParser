using System.Text.Json.Serialization;
using System.Text.Json;
using System;
using MetaParser.Contexts;
using MetaParser.Patternization;

namespace MetaParser.Json.Definitions;

internal record TokenPattern : PatternDefinition
{
    [JsonIgnore]
    public bool IsResolved => true;
    [JsonInclude]
    public string id;
    [JsonInclude]
    public bool optional;// TODO: How to implement this?

    [JsonConstructor]
    public TokenPattern(string id, bool optional = false)
    {
        this.id = id;
        this.optional = optional;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        return new PatternConst(context.Get_TokenId_Ref(id));
    }
}

internal class TokenPatternConverter : JsonConverter<TokenPattern>
{
    public override TokenPattern? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.StartObject:
                {
                    throw new NotImplementedException();
                }
            case JsonTokenType.String:
                {
                    var val = reader.GetString();
                    return val is not null ? new TokenPattern(val) : null;
                }
            default:
                {
                    throw new JsonException();
                }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, TokenPattern value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
