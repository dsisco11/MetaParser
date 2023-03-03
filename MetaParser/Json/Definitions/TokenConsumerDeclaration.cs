using MetaParser.Structs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record TokenConsumerDeclaration : ConsumerDeclaration<TokenPatternDeclaration>
{
    [JsonPropertyName("$type")]
    public override EConsumerType Type { get => EConsumerType.Token; set { } }

    [JsonConstructor]
    public TokenConsumerDeclaration(IEnumerable<TokenPatternDeclaration>? start, IEnumerable<TokenPatternDeclaration>? consume, IEnumerable<TokenPatternDeclaration>? stop, IEnumerable<TokenPatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}
