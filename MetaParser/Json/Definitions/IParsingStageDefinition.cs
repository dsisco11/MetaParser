using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(LexingStageDefinition), "lexer")]
[JsonDerivedType(typeof(SyntaxStageDefinition), "syntax")]
internal interface IParsingStageDefinition
{
    [JsonPropertyName("$type")]
    EParsingStage Type { get; }

    [JsonPropertyName("ignored")]
    string[] Ignored { get; }

    [JsonPropertyName("dropped")]
    ConsumerDeclaration[] Dropped { get; }

    [JsonPropertyName("consumers")]
    IEnumerable<KeyValuePair<string, IEnumerable<ConsumerDeclaration>>> Consumers { get; }
}
