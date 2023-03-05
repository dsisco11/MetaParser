using MetaParser.Consumers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ValueConsumerDeclaration), typeDiscriminator: "data")]
[JsonDerivedType(typeof(TokenConsumerDeclaration), typeDiscriminator: "tokens")]
internal interface IConsumerDeclaration
{
    EConsumerType Type { get; }
    IEnumerable<IPatternDeclaration> Start { get; }
    IEnumerable<IPatternDeclaration> Consume { get; }
    IEnumerable<IPatternDeclaration> Stop { get; }
    IEnumerable<IPatternDeclaration> Escape { get; }
}