using MetaParser.Json.Definitions;
using System.Text.Json.Serialization;

namespace MetaParser.Contexts;

[JsonSerializable(typeof(ParserDefinition))]
[JsonSerializable(typeof(ETokenType))]
[JsonSerializable(typeof(IConsumerDefinition))]
[JsonSerializable(typeof(PatternDefinition))]
[JsonSerializable(typeof(ValueConsumerDefinition))]
[JsonSerializable(typeof(TokenConsumerDefinition))]
internal partial class MetaParserJsonSerializer : JsonSerializerContext
{
}
