using MetaParser.Json.Definitions;
using System.Text.Json.Serialization;

namespace MetaParser.Core;

[JsonSerializable(typeof(ParserDefinition))]
[JsonSerializable(typeof(ParsingStages))]
[JsonSerializable(typeof(LexingStageDefinition))]
[JsonSerializable(typeof(SyntaxStageDefinition))]
// Consumers
[JsonSerializable(typeof(ValueConsumerDeclaration))]
[JsonSerializable(typeof(TokenConsumerDeclaration))]
// Patterns
[JsonSerializable(typeof(ValuePatternDeclaration))]
[JsonSerializable(typeof(TokenPatternDeclaration))]
internal partial class MetaParserJsonSerializer : JsonSerializerContext
{
}
