using MetaParser.Parsing.Constructs;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal abstract record ConsumerDeclaration
{
    #region Properties
    public abstract EConsumerKind Type { get; }
    public abstract EParsingStage Stage { get; }

    [JsonPropertyName("start")]
    public IEnumerable<PatternDeclaration> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<PatternDeclaration> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<PatternDeclaration> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<PatternDeclaration> Escape { get; set; }
    #endregion

    [JsonConstructor]
    protected ConsumerDeclaration(IEnumerable<PatternDeclaration>? start, IEnumerable<PatternDeclaration>? consume, IEnumerable<PatternDeclaration>? stop, IEnumerable<PatternDeclaration>? escape)
    {
        Start = start ?? Array.Empty<PatternDeclaration>();
        Consume = consume ?? Array.Empty<PatternDeclaration>();
        Stop = stop ?? Array.Empty<PatternDeclaration>();
        Escape = escape ?? Array.Empty<PatternDeclaration>();
    }
}
