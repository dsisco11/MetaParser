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
    public IEnumerable<IPatternDeclaration> Start { get; }
    public IEnumerable<IPatternDeclaration> Consume { get; }
    public IEnumerable<IPatternDeclaration> Stop { get; }
    public IEnumerable<IPatternDeclaration> Escape { get; }
}