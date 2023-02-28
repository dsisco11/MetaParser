using MetaParser.Structs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ValueConsumerDeclaration), typeDiscriminator: "data")]
[JsonDerivedType(typeof(TokenConsumerDeclaration), typeDiscriminator: "tokens")]
internal interface IConsumerDeclaration
{
    EConsumerType Type { get; }
    public IEnumerable<PatternDeclaration> Start { get; }
    public IEnumerable<PatternDeclaration> Consume { get; }
    public IEnumerable<PatternDeclaration> Stop { get; }
    public IEnumerable<PatternDeclaration> Escape { get; }
}