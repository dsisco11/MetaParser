using MetaParser.Parsing.Constructs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record TokenConsumerDeclaration : ConsumerDeclaration<TokenPatternDeclaration>
{
    [JsonIgnore]
    public override EConsumerType Type { get => EConsumerType.Syntax; }

    [JsonConstructor]
    public TokenConsumerDeclaration(IEnumerable<TokenPatternDeclaration>? start, IEnumerable<TokenPatternDeclaration>? consume, IEnumerable<TokenPatternDeclaration>? stop, IEnumerable<TokenPatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}
