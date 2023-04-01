using MetaParser.Json.Definitions;
using System.Text.Json.Serialization;

namespace MetaParser.Core;

[JsonSerializable(typeof(ParserDefinition))]
// Stages
[JsonSerializable(typeof(IParsingStageDefinition))]
[JsonSerializable(typeof(LexingStageDefinition))]
[JsonSerializable(typeof(SyntaxStageDefinition))]
// Consumers
[JsonSerializable(typeof(ValueConsumerDeclaration))]
[JsonSerializable(typeof(TokenConsumerDeclaration))]
// Patterns
[JsonSerializable(typeof(PatternDeclaration))]
internal partial class MetaParserJsonSerializer : JsonSerializerContext
{
}
