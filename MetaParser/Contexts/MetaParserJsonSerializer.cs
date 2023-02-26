using MetaParser.Json.Definitions;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Contexts;

[JsonSerializable(typeof(ETokenType))]
[JsonSerializable(typeof(IConsumerDefinition))]
[JsonSerializable(typeof(Dictionary<string, IConsumerDefinition>))]
[JsonSerializable(typeof(ParserDefinition))]
internal partial class MetaParserJsonSerializer
{
}
