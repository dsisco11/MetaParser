using MetaParser.Parsing.Constructs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record TokenConsumerDeclaration : ConsumerDeclaration
{
    [JsonIgnore]
    public override EConsumerKind Type { get => EConsumerKind.Syntax; }
    public override EParsingStage Stage { get => EParsingStage.Syntax; }

    [JsonConstructor]
    public TokenConsumerDeclaration(IEnumerable<PatternDeclaration>? start, IEnumerable<PatternDeclaration>? consume, IEnumerable<PatternDeclaration>? stop, IEnumerable<PatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}
