using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ValueConsumerDeclaration), typeDiscriminator: "constant")]
[JsonDerivedType(typeof(TokenConsumerDeclaration), typeDiscriminator: "compound")]
internal interface IConsumerDefinition
{
    [JsonPropertyName("$type")]
    string Type { get; }

    [JsonPropertyName("start")]
    public IEnumerable<PatternDefinition> Start { get; }

    [JsonPropertyName("consume")]
    public IEnumerable<PatternDefinition> Consume { get; }

    [JsonPropertyName("stop")]
    public IEnumerable<PatternDefinition> Stop { get; }

    [JsonPropertyName("escape")]
    public IEnumerable<PatternDefinition> Escape { get; }
}