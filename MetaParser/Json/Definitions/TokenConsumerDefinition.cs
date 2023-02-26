using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record TokenConsumerDefinition : IConsumerDefinition
{
    #region Properties
    [JsonPropertyName("$type")]
    public ETokenType Type => ETokenType.Compound;

    [JsonPropertyName("start")]
    public IEnumerable<TokenPattern> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<TokenPattern> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<TokenPattern> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<TokenPattern> Escape { get; set; }
    #endregion

    #region IConsumerDefinition
    IEnumerable<PatternDefinition> IConsumerDefinition.Start => Start as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Consume => Consume as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Stop => Stop as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Escape => Escape as IEnumerable<PatternDefinition>;
    #endregion

    [JsonConstructor]
    public TokenConsumerDefinition(IEnumerable<TokenPattern>? start, IEnumerable<TokenPattern>? consume, IEnumerable<TokenPattern>? stop, IEnumerable<TokenPattern>? escape)
    {
        Start = start ?? Array.Empty<TokenPattern>();
        Consume = consume ?? Array.Empty<TokenPattern>();
        Stop = stop ?? Array.Empty<TokenPattern>();
        Escape = escape ?? Array.Empty<TokenPattern>();
    }
}
