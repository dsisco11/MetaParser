using MetaParser.Json.Definitions;
using System.Text.Json.Serialization;

namespace MetaParser.Contexts;

[JsonSerializable(typeof(ParserDefinition))]
[JsonSerializable(typeof(ETokenType))]
[JsonSerializable(typeof(IConsumerDeclaration))]
[JsonSerializable(typeof(PatternDeclaration))]
[JsonSerializable(typeof(ValueConsumerDeclaration))]
[JsonSerializable(typeof(TokenConsumerDeclaration))]
internal partial class MetaParserJsonSerializer : JsonSerializerContext
{
}
