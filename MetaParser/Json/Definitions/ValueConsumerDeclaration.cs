using MetaParser.Consumers;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record ValueConsumerDeclaration : ConsumerDeclaration<ValuePatternDeclaration>
{
    [JsonPropertyName("$type")]
    public override EConsumerType Type { get => EConsumerType.Data; set { } }

    [JsonConstructor]
    public ValueConsumerDeclaration(IEnumerable<ValuePatternDeclaration>? start, IEnumerable<ValuePatternDeclaration>? consume, IEnumerable<ValuePatternDeclaration>? stop, IEnumerable<ValuePatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}
