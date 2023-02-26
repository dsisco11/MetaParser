using MetaParser.Contexts;
using MetaParser.Json.JsonTypeConverters;
using MetaParser.Patternization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic]
[JsonDerivedType(typeof(ValuePattern), typeDiscriminator: "constant")]
[JsonDerivedType(typeof(TokenPattern), typeDiscriminator: "compound")]
internal abstract record PatternDefinition
{
    [JsonPropertyName("$type")]
    public ETokenType Type { get; set; }
    public abstract Pattern Resolve(MetaParserContext context);
}

//[JsonConverter(typeof(JsonOneToManyDictionaryConverter))]
//internal class PatternDefinitionList : List<PatternDefinition>
//{
//}