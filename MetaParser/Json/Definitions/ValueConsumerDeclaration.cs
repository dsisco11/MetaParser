using MetaParser.Parsing.Constructs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record ValueConsumerDeclaration : ConsumerDeclaration<ValuePatternDeclaration>
{
    [JsonIgnore]
    public override EConsumerType Type { get => EConsumerType.Lexer; }

    [JsonConstructor]
    public ValueConsumerDeclaration(IEnumerable<ValuePatternDeclaration>? start, IEnumerable<ValuePatternDeclaration>? consume, IEnumerable<ValuePatternDeclaration>? stop, IEnumerable<ValuePatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}
