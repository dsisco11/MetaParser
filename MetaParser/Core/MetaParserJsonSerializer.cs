using MetaParser.Json.Definitions;
using System.Text.Json.Serialization;

namespace MetaParser.Core;

[JsonSerializable(typeof(ParserDefinition))]
// Consumers
[JsonSerializable(typeof(IConsumerDeclaration))]
[JsonSerializable(typeof(ValueConsumerDeclaration))]
[JsonSerializable(typeof(TokenConsumerDeclaration))]
// Patterns
[JsonSerializable(typeof(IPatternDeclaration))]
[JsonSerializable(typeof(ValuePatternDeclaration))]
[JsonSerializable(typeof(TokenPatternDeclaration))]
//// Stuff
//[JsonSerializable(typeof(NodeKey))]
//[JsonSerializable(typeof(NodeType))]
//[JsonSerializable(typeof(NodeDepth))]
//[JsonSerializable(typeof(ResolvedNode))]
//[JsonSerializable(typeof(MetaParserRegistry))]
internal partial class MetaParserJsonSerializer : JsonSerializerContext
{
}
