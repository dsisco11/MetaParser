using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ValueConsumerDefinition), typeDiscriminator: "constant")]
[JsonDerivedType(typeof(TokenConsumerDefinition), typeDiscriminator: "compound")]
internal interface IConsumerDefinition
{
    ETokenType Type { get; }
    public IEnumerable<PatternDefinition> Start { get; }
    public IEnumerable<PatternDefinition> Consume { get; }
    public IEnumerable<PatternDefinition> Stop { get; }
    public IEnumerable<PatternDefinition> Escape { get; }
}