using MetaParser.Parsing.Constructs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record ValueConsumerDeclaration : ConsumerDeclaration
{
    [JsonIgnore]
    public override EConsumerKind Type { get => EConsumerKind.Lexer; }
    public override EParsingStage Stage { get => EParsingStage.Lexer; }

    [JsonConstructor]
    public ValueConsumerDeclaration(IEnumerable<PatternDeclaration>? start, IEnumerable<PatternDeclaration>? consume, IEnumerable<PatternDeclaration>? stop, IEnumerable<PatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}
