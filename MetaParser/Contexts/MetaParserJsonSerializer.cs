using MetaParser.Json.Definitions;
using System.Text.Json.Serialization;

namespace MetaParser.Contexts;

[JsonSerializable(typeof(ParserDefinition))]
[JsonSerializable(typeof(ETokenType))]
// Consumers
[JsonSerializable(typeof(IConsumerDeclaration))]
[JsonSerializable(typeof(ValueConsumerDeclaration))]
[JsonSerializable(typeof(TokenConsumerDeclaration))]
// Patterns
[JsonSerializable(typeof(IPatternDeclaration))]
[JsonSerializable(typeof(ValuePatternDeclaration))]
[JsonSerializable(typeof(TokenPatternDeclaration))]
internal partial class MetaParserJsonSerializer : JsonSerializerContext
{
}
